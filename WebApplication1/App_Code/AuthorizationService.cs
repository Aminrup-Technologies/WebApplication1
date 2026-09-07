/*
 * WHEN: 2026-09-07
 * WHY: Authorization modernization PR A. Wrap the hybrid Session / USERTYPE / WORKMAN model
 *      documented in docs/ROLE_PERMISSION_ARCHITECTURE_AUDIT.md without changing any page.
 * WHAT: Additive AuthorizationService. Overlay grants come from PermissionRepository
 *      (direct + group, 5-minute cache). SWITCH_USER still requires CanImpersonate
 *      or (Admin + overlay) so Office Staff cannot gain impersonation from an empty
 *      overlay catalog. IsAdmin() is USERTYPE == Admin only. DescribeIdentity evaluates
 *      another employee through the same gates and restores the live Session.
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.SessionState;

namespace WebApplication1.bussiness.production
{
    public sealed class EffectivePermission
    {
        public string Code { get; set; }
        public bool Granted { get; set; }
        public string Source { get; set; }
        public string Detail { get; set; }
    }

    public static class AuthorizationFeatureCodes
    {
        public const string SwitchUser = "SWITCH_USER";
        public const string PayrollOverride = "PAYROLL_OVERRIDE";
        public const string Job360Override = "JOB360_OVERRIDE";
        public const string AttendanceOverride = "ATTENDANCE_OVERRIDE";
        public const string ExportPayroll = "EXPORT_PAYROLL";
        public const string UserAdmin = "USER_ADMIN";

        public const string LegacyPayrollDashboard = "LEGACY_PAYROLL_DASHBOARD";
        public const string LegacyAttachManpower = "LEGACY_ATTACH_MANPOWER";
        public const string LegacyExpenseHeads = "LEGACY_EXPENSE_HEADS";
    }

    public static class AuthorizationService
    {
        public const string SourceUserType = "USERTYPE";
        public const string SourceGroup = "GROUP";
        public const string SourceDirect = "DIRECT";
        public const string SourceLegacyConfig = "LEGACY_CONFIG";
        public const string SourceLegacyHardcoded = "LEGACY_HARDCODED";
        public const string SourceModuleException = "MODULE_EXCEPTION";
        public const string SourceNone = "NONE";

        public const string OfficeStaffUserType = "Office Staff";

        public static bool IsAuthenticated()
        {
            return IsAuthenticated(CurrentSession());
        }

        public static bool IsAuthenticated(HttpSessionState session)
        {
            if (session == null) return false;
            return HasValue(session, SessionKeys.UserID)
                && HasValue(session, SessionKeys.RolePermissionDB)
                && HasValue(session, SessionKeys.UserRoleDB)
                && HasValue(session, SessionKeys.UserName)
                && HasValue(session, SessionKeys.WorkmanSL);
        }

        public static bool IsAdmin()
        {
            return IsAdmin(CurrentSession());
        }

        public static bool IsAdmin(HttpSessionState session)
        {
            if (session == null) return false;
            return string.Equals(Read(session, SessionKeys.UserType), ImpersonationAudit.AdminUserType, StringComparison.OrdinalIgnoreCase);
        }

        public static bool HasPermission(string code)
        {
            return HasPermission(CurrentSession(), code);
        }

        public static bool HasPermission(HttpSessionState session, string code)
        {
            return CanAccess(session, code);
        }

        public static bool CanAccess(string feature)
        {
            return CanAccess(CurrentSession(), feature);
        }

        public static bool CanAccess(HttpSessionState session, string feature)
        {
            if (string.IsNullOrWhiteSpace(feature)) return false;
            if (!IsAuthenticated(session)) return false;

            string code = feature.Trim();

            if (MatchesLegacyModuleAuthority(session, code)) return true;

            string overlaySource;
            if (HasOverlayPermission(session, code, out overlaySource))
            {
                if (string.Equals(code, AuthorizationFeatureCodes.SwitchUser, StringComparison.OrdinalIgnoreCase))
                {
                    if (IsAdmin(session) && !ImpersonationAudit.IsImpersonating(session)) return true;
                }
                else
                {
                    return true;
                }
            }

            if (IsWorkmanOnConfigAllowlist(session, code)) return true;
            if (IsWorkmanOnHardcodedList(session, code)) return true;
            return false;
        }

        public static bool IsWorkmanAllowed(string code)
        {
            return IsWorkmanAllowed(CurrentSession(), code);
        }

        public static bool IsWorkmanAllowed(HttpSessionState session, string code)
        {
            if (session == null || string.IsNullOrWhiteSpace(code)) return false;
            string overlayIgnored;
            if (HasOverlayPermission(session, code, out overlayIgnored)) return true;
            if (IsWorkmanOnConfigAllowlist(session, code)) return true;
            return IsWorkmanOnHardcodedList(session, code);
        }

        public static IList<EffectivePermission> GetEffectivePermissions()
        {
            return GetEffectivePermissions(CurrentSession());
        }

        public static IList<EffectivePermission> GetEffectivePermissions(HttpSessionState session)
        {
            List<EffectivePermission> list = new List<EffectivePermission>();
            string[] codes = GetTrackedCodes();

            for (int i = 0; i < codes.Length; i++)
            {
                list.Add(Describe(session, codes[i]));
            }
            return list;
        }

        public static string[] GetTrackedCodes()
        {
            return new string[]
            {
                AuthorizationFeatureCodes.SwitchUser,
                AuthorizationFeatureCodes.PayrollOverride,
                AuthorizationFeatureCodes.Job360Override,
                AuthorizationFeatureCodes.AttendanceOverride,
                AuthorizationFeatureCodes.ExportPayroll,
                AuthorizationFeatureCodes.UserAdmin,
                AuthorizationFeatureCodes.LegacyPayrollDashboard,
                AuthorizationFeatureCodes.LegacyAttachManpower,
                AuthorizationFeatureCodes.LegacyExpenseHeads
            };
        }

        public static string DisplaySource(string source, bool granted)
        {
            if (!granted) return "Denied";
            if (string.IsNullOrWhiteSpace(source)
                || string.Equals(source, SourceNone, StringComparison.OrdinalIgnoreCase))
            {
                return "Denied";
            }
            if (string.Equals(source, SourceDirect, StringComparison.OrdinalIgnoreCase)) return "Direct";
            if (string.Equals(source, SourceGroup, StringComparison.OrdinalIgnoreCase)) return "Group";
            if (string.Equals(source, SourceLegacyConfig, StringComparison.OrdinalIgnoreCase)) return "Config";
            if (string.Equals(source, SourceLegacyHardcoded, StringComparison.OrdinalIgnoreCase)) return "Hardcoded";
            if (string.Equals(source, SourceModuleException, StringComparison.OrdinalIgnoreCase)) return "Module";
            if (string.Equals(source, SourceUserType, StringComparison.OrdinalIgnoreCase)) return "Admin";
            if (source.IndexOf(SourceUserType, StringComparison.OrdinalIgnoreCase) >= 0
                && source.IndexOf(SourceLegacyConfig, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Admin+Config";
            }
            return source;
        }

        /// <summary>
        /// Evaluate the canonical gates as if this employee were logged in (not impersonating).
        /// Restores the live Session in finally so the inspector never leaks subject identity.
        /// </summary>
        public static void DescribeIdentity(
            HttpSessionState session,
            string userId,
            string rolePermissionDb,
            string userRoleDb,
            string userName,
            string workmanSL,
            string userType,
            out bool authenticated,
            out bool admin,
            out bool canImpersonate,
            out bool payrollConfigAllowlist,
            out IList<EffectivePermission> permissions)
        {
            authenticated = false;
            admin = false;
            canImpersonate = false;
            payrollConfigAllowlist = false;
            permissions = new List<EffectivePermission>();
            if (session == null) return;

            object oldUserId = session[SessionKeys.UserID];
            object oldRolePermission = session[SessionKeys.RolePermissionDB];
            object oldUserRole = session[SessionKeys.UserRoleDB];
            object oldUserName = session[SessionKeys.UserName];
            object oldWorkman = session[SessionKeys.WorkmanSL];
            object oldUserType = session[SessionKeys.UserType];
            object oldImpersonating = session[SessionKeys.IsImpersonating];
            try
            {
                session[SessionKeys.UserID] = userId ?? "";
                session[SessionKeys.RolePermissionDB] = rolePermissionDb ?? "";
                session[SessionKeys.UserRoleDB] = userRoleDb ?? "";
                session[SessionKeys.UserName] = userName ?? "";
                session[SessionKeys.WorkmanSL] = workmanSL ?? "";
                session[SessionKeys.UserType] = userType ?? "";
                session.Remove(SessionKeys.IsImpersonating);

                authenticated = IsAuthenticated(session);
                admin = IsAdmin(session);
                canImpersonate = ImpersonationAudit.CanImpersonate(session);
                payrollConfigAllowlist = IsWorkmanOnConfigAllowlist(session, AuthorizationFeatureCodes.PayrollOverride);
                permissions = GetEffectivePermissions(session);
            }
            finally
            {
                RestoreSessionValue(session, SessionKeys.UserID, oldUserId);
                RestoreSessionValue(session, SessionKeys.RolePermissionDB, oldRolePermission);
                RestoreSessionValue(session, SessionKeys.UserRoleDB, oldUserRole);
                RestoreSessionValue(session, SessionKeys.UserName, oldUserName);
                RestoreSessionValue(session, SessionKeys.WorkmanSL, oldWorkman);
                RestoreSessionValue(session, SessionKeys.UserType, oldUserType);
                RestoreSessionValue(session, SessionKeys.IsImpersonating, oldImpersonating);
            }
        }

        private static void RestoreSessionValue(HttpSessionState session, string key, object previous)
        {
            if (session == null || string.IsNullOrEmpty(key)) return;
            if (previous == null) session.Remove(key);
            else session[key] = previous;
        }

        private static EffectivePermission Describe(HttpSessionState session, string code)
        {
            EffectivePermission item = new EffectivePermission();
            item.Code = code;
            item.Granted = false;
            item.Source = SourceNone;
            item.Detail = "";

            if (!IsAuthenticated(session))
            {
                item.Detail = "Not authenticated.";
                return item;
            }

            if (MatchesLegacyModuleAuthority(session, code))
            {
                item.Granted = true;
                if (string.Equals(code, AuthorizationFeatureCodes.SwitchUser, StringComparison.OrdinalIgnoreCase))
                {
                    item.Source = SourceUserType + "+" + SourceLegacyConfig;
                    item.Detail = "USERTYPE=Admin and SwitchUserAuthorizedUsers.";
                }
                else
                {
                    item.Source = SourceModuleException;
                    item.Detail = "USERTYPE Admin or Office Staff (module-local).";
                }
                return item;
            }

            string overlaySource;
            if (HasOverlayPermission(session, code, out overlaySource))
            {
                bool overlayUsable = !string.Equals(code, AuthorizationFeatureCodes.SwitchUser, StringComparison.OrdinalIgnoreCase)
                    || (IsAdmin(session) && !ImpersonationAudit.IsImpersonating(session));
                if (overlayUsable)
                {
                    item.Granted = true;
                    item.Source = string.Equals(overlaySource, PermissionRepository.SourceGroup, StringComparison.OrdinalIgnoreCase)
                        ? SourceGroup
                        : SourceDirect;
                    item.Detail = "Overlay " + overlaySource + ".";
                    return item;
                }
            }

            if (IsWorkmanOnConfigAllowlist(session, code))
            {
                item.Granted = true;
                item.Source = SourceLegacyConfig;
                item.Detail = ConfigKeyFor(code);
                return item;
            }

            if (IsWorkmanOnHardcodedList(session, code))
            {
                item.Granted = true;
                item.Source = SourceLegacyHardcoded;
                item.Detail = "Hardcoded WorkmanSL list.";
                return item;
            }

            if (string.Equals(code, AuthorizationFeatureCodes.SwitchUser, StringComparison.OrdinalIgnoreCase))
            {
                if (ImpersonationAudit.IsImpersonating(session))
                {
                    item.Detail = "Already impersonating.";
                }
                else if (!IsAdmin(session))
                {
                    item.Source = SourceUserType;
                    item.Detail = "USERTYPE is not Admin.";
                }
                else
                {
                    item.Source = SourceLegacyConfig;
                    item.Detail = "Not on SwitchUserAuthorizedUsers and no overlay grant.";
                }
                return item;
            }

            item.Detail = "No matching overlay, config, hardcoded, or module exception.";
            return item;
        }

        private static bool MatchesLegacyModuleAuthority(HttpSessionState session, string code)
        {
            if (string.Equals(code, AuthorizationFeatureCodes.SwitchUser, StringComparison.OrdinalIgnoreCase))
            {
                return ImpersonationAudit.CanImpersonate(session);
            }
            return MatchesModuleException(session, code);
        }

        private static bool HasOverlayPermission(HttpSessionState session, string code, out string source)
        {
            source = "";
            if (session == null || string.IsNullOrWhiteSpace(code)) return false;
            return PermissionRepository.TryGetSource(Read(session, SessionKeys.WorkmanSL), code, out source);
        }

        private static bool IsWorkmanOnConfigAllowlist(HttpSessionState session, string code)
        {
            string key = ConfigKeyFor(code);
            if (string.IsNullOrEmpty(key)) return false;
            return IsWorkmanInCsv(Read(session, SessionKeys.WorkmanSL), ConfigurationManager.AppSettings[key]);
        }

        private static bool IsWorkmanOnHardcodedList(HttpSessionState session, string code)
        {
            string workman = Read(session, SessionKeys.WorkmanSL);
            if (string.IsNullOrWhiteSpace(workman)) return false;

            if (string.Equals(code, AuthorizationFeatureCodes.ExportPayroll, StringComparison.OrdinalIgnoreCase)
                || string.Equals(code, AuthorizationFeatureCodes.LegacyPayrollDashboard, StringComparison.OrdinalIgnoreCase))
            {
                return WorkmanEquals(workman, "J8");
            }
            if (string.Equals(code, AuthorizationFeatureCodes.LegacyAttachManpower, StringComparison.OrdinalIgnoreCase))
            {
                return WorkmanEquals(workman, "J8")
                    || WorkmanEquals(workman, "A84")
                    || WorkmanEquals(workman, "K208")
                    || WorkmanEquals(workman, "N21");
            }
            if (string.Equals(code, AuthorizationFeatureCodes.LegacyExpenseHeads, StringComparison.OrdinalIgnoreCase))
            {
                return WorkmanEquals(workman, "J8")
                    || WorkmanEquals(workman, "A84")
                    || WorkmanEquals(workman, "K208");
            }
            return false;
        }

        private static bool MatchesModuleException(HttpSessionState session, string code)
        {
            if (!string.Equals(code, AuthorizationFeatureCodes.Job360Override, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(code, AuthorizationFeatureCodes.AttendanceOverride, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (IsAdmin(session)) return true;
            return string.Equals(Read(session, SessionKeys.UserType), OfficeStaffUserType, StringComparison.OrdinalIgnoreCase);
        }

        private static string ConfigKeyFor(string code)
        {
            if (string.Equals(code, AuthorizationFeatureCodes.SwitchUser, StringComparison.OrdinalIgnoreCase))
            {
                return ImpersonationAudit.AuthorizedUsersAppSetting;
            }
            if (string.Equals(code, AuthorizationFeatureCodes.PayrollOverride, StringComparison.OrdinalIgnoreCase))
            {
                return "PayrollAuthorizedUsers";
            }
            return "";
        }

        private static bool IsWorkmanInCsv(string workmanSL, string csv)
        {
            if (string.IsNullOrWhiteSpace(workmanSL) || string.IsNullOrEmpty(csv)) return false;
            string needle = workmanSL.Trim().ToUpperInvariant();
            string[] parts = csv.Split(',');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Trim().ToUpperInvariant() == needle) return true;
            }
            return false;
        }

        private static bool WorkmanEquals(string workman, string expected)
        {
            return string.Equals((workman ?? "").Trim(), expected, StringComparison.OrdinalIgnoreCase);
        }

        private static HttpSessionState CurrentSession()
        {
            if (HttpContext.Current == null) return null;
            return HttpContext.Current.Session;
        }

        private static bool HasValue(HttpSessionState session, string key)
        {
            return !string.IsNullOrWhiteSpace(Read(session, key));
        }

        private static string Read(HttpSessionState session, string key)
        {
            if (session == null || session[key] == null) return "";
            return session[key].ToString();
        }
    }
}
