/*
 * WHEN: 2026-09-06
 * WHY: PR #89 impersonation audit foundation; PR #90 adds the restore-gate helper.
 * WHAT: Dedicated IMPERSONATE / IMPERSONATE_RETURN writers against tbl_UserLoginAudit (LoginResult),
 *       snapshot helpers for ORIGINAL_* Session keys, IsOriginalIdentityCaptured (all six ORIGINAL_*
 *       keys must be present), and IsImpersonating / CanImpersonate / CanReturnFromImpersonation guards.
 *       Existing login InsertLoginAudit is untouched.
 */

using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.SessionState;

namespace WebApplication1.bussiness.production
{
    public static class ImpersonationAudit
    {
        public const string EventImpersonate = "IMPERSONATE";
        public const string EventImpersonateReturn = "IMPERSONATE_RETURN";
        public const string OutcomeSuccess = "SUCCESS";
        public const string OutcomeFailure = "FAILURE";
        public const string AuthorizedUsersAppSetting = "SwitchUserAuthorizedUsers";
        public const string AdminUserType = "Admin";

        public static Guid NewCorrelationId()
        {
            return Guid.NewGuid();
        }

        public static bool IsImpersonating(HttpSessionState session)
        {
            if (session == null) return false;
            object flag = session[SessionKeys.IsImpersonating];
            if (flag is bool) return (bool)flag;
            if (flag == null || flag == DBNull.Value) return false;
            string text = flag.ToString().Trim();
            return text == "1" || text.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        public static bool CanImpersonate(HttpSessionState session)
        {
            if (session == null) return false;
            if (IsImpersonating(session)) return false;
            if (string.IsNullOrWhiteSpace(Read(session, SessionKeys.UserID))) return false;
            if (string.IsNullOrWhiteSpace(Read(session, SessionKeys.WorkmanSL))) return false;
            if (!string.Equals(Read(session, SessionKeys.UserType), AdminUserType, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return IsWorkmanAuthorized(Read(session, SessionKeys.WorkmanSL));
        }

        public static bool IsOriginalIdentityCaptured(HttpSessionState session)
        {
            if (session == null) return false;
            return HasRequired(session, SessionKeys.OriginalUserID)
                && HasRequired(session, SessionKeys.OriginalWorkmanSL)
                && HasRequired(session, SessionKeys.OriginalUserName)
                && HasRequired(session, SessionKeys.OriginalUserType)
                && HasRequired(session, SessionKeys.OriginalUserRoleDB)
                && HasRequired(session, SessionKeys.OriginalRolePermissionDB);
        }

        public static bool CanReturnFromImpersonation(HttpSessionState session)
        {
            if (session == null) return false;
            if (!IsImpersonating(session)) return false;
            return IsOriginalIdentityCaptured(session);
        }

        public static void StoreCorrelationId(HttpSessionState session, Guid correlationId)
        {
            if (session == null) return;
            session[SessionKeys.ImpersonationCorrelation] = correlationId.ToString("D");
        }

        public static Guid GetCorrelationId(HttpSessionState session)
        {
            Guid parsed;
            if (Guid.TryParse(Read(session, SessionKeys.ImpersonationCorrelation), out parsed))
            {
                return parsed;
            }
            return NewCorrelationId();
        }

        public static void CaptureOriginalIdentity(HttpSessionState session)
        {
            if (session == null) return;
            if (!string.IsNullOrWhiteSpace(Read(session, SessionKeys.OriginalWorkmanSL))) return;

            session[SessionKeys.OriginalUserID] = Read(session, SessionKeys.UserID);
            session[SessionKeys.OriginalWorkmanSL] = Read(session, SessionKeys.WorkmanSL);
            session[SessionKeys.OriginalUserName] = Read(session, SessionKeys.UserName);
            session[SessionKeys.OriginalUserType] = Read(session, SessionKeys.UserType);
            session[SessionKeys.OriginalUserRoleDB] = Read(session, SessionKeys.UserRoleDB);
            session[SessionKeys.OriginalRolePermissionDB] = Read(session, SessionKeys.RolePermissionDB);
        }

        public static void ClearOriginalIdentity(HttpSessionState session)
        {
            if (session == null) return;
            session.Remove(SessionKeys.IsImpersonating);
            session.Remove(SessionKeys.OriginalUserID);
            session.Remove(SessionKeys.OriginalWorkmanSL);
            session.Remove(SessionKeys.OriginalUserName);
            session.Remove(SessionKeys.OriginalUserType);
            session.Remove(SessionKeys.OriginalUserRoleDB);
            session.Remove(SessionKeys.OriginalRolePermissionDB);
            session.Remove(SessionKeys.ImpersonationCorrelation);
        }

        public static void Write(
            DB_Utility_OH4Y dbcl,
            string eventType,
            string adminLoginId,
            string adminWorkman,
            string targetLoginId,
            string targetWorkman,
            string outcome,
            string ipAddress,
            string userAgent,
            string sessionId,
            Guid correlationId)
        {
            if (dbcl == null) throw new ArgumentNullException("dbcl");
            if (eventType != EventImpersonate && eventType != EventImpersonateReturn)
            {
                throw new ArgumentOutOfRangeException("eventType");
            }

            string reason = "Corr=" + correlationId.ToString("D")
                + ";TargetUser=" + NullToEmpty(targetLoginId)
                + ";TargetWrk=" + NullToEmpty(targetWorkman)
                + ";Outcome=" + NullToEmpty(outcome);

            dbcl.SPreturn_dt(
                @"INSERT INTO tbl_UserLoginAudit (LoginID, WorkmanSL, LoginTime, LoginResult, FailureReason, IPAddress, UserAgent, SessionID)
                  VALUES (@LoginID, @WorkmanSL, GETDATE(), @Result, @Reason, @IP, @Agent, @SessionID)",
                new SqlParameter[]
                {
                    new SqlParameter("@LoginID", (object)NullToEmpty(adminLoginId)),
                    new SqlParameter("@WorkmanSL", string.IsNullOrEmpty(adminWorkman) ? (object)DBNull.Value : adminWorkman),
                    new SqlParameter("@Result", eventType),
                    new SqlParameter("@Reason", reason),
                    new SqlParameter("@IP", NullToEmpty(ipAddress)),
                    new SqlParameter("@Agent", NullToEmpty(userAgent)),
                    new SqlParameter("@SessionID", NullToEmpty(sessionId))
                });
        }

        public static void WriteFromRequest(
            DB_Utility_OH4Y dbcl,
            HttpRequest request,
            HttpSessionState session,
            string eventType,
            string targetLoginId,
            string targetWorkman,
            string outcome,
            Guid correlationId)
        {
            string adminLoginId = Read(session, SessionKeys.OriginalUserID);
            string adminWorkman = Read(session, SessionKeys.OriginalWorkmanSL);
            if (string.IsNullOrEmpty(adminLoginId)) adminLoginId = Read(session, SessionKeys.UserID);
            if (string.IsNullOrEmpty(adminWorkman)) adminWorkman = Read(session, SessionKeys.WorkmanSL);

            Write(
                dbcl,
                eventType,
                adminLoginId,
                adminWorkman,
                targetLoginId,
                targetWorkman,
                outcome,
                request != null ? (request.UserHostAddress ?? "") : "",
                request != null ? (request.UserAgent ?? "") : "",
                session != null ? session.SessionID : "",
                correlationId);
        }

        private static bool IsWorkmanAuthorized(string workmanSL)
        {
            if (string.IsNullOrWhiteSpace(workmanSL)) return false;
            string authConfig = ConfigurationManager.AppSettings[AuthorizedUsersAppSetting];
            if (string.IsNullOrEmpty(authConfig)) return false;

            string needle = workmanSL.Trim().ToUpperInvariant();
            string[] parts = authConfig.Split(',');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Trim().ToUpperInvariant() == needle) return true;
            }
            return false;
        }

        private static bool HasRequired(HttpSessionState session, string key)
        {
            return !string.IsNullOrWhiteSpace(Read(session, key));
        }

        private static string Read(HttpSessionState session, string key)
        {
            if (session == null || session[key] == null) return "";
            return session[key].ToString();
        }

        private static string NullToEmpty(string value)
        {
            return value ?? "";
        }
    }
}
