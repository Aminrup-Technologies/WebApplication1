using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace WebApplication1.bussiness.production
{
    /// <summary>
    /// Portal + module switches for Email and WhatsApp sends.
    /// Notification modules require portal AND module to be on.
    /// Authentication OTPs (login MFA, password reset, profile verification) stay
    /// active by default and ignore the portal kill switch.
    /// Missing table: fail open (current production behavior).
    /// </summary>
    public static class NotificationTriggerHelper
    {
        public const string ScopePortal = "Portal";
        public const string ScopeModule = "Module";
        public const string ScopeAuth = "Auth";

        public const string KeyPortal = "PORTAL";
        public const string ModuleLoginMfa = "LOGIN_MFA";
        public const string ModulePasswordReset = "PASSWORD_RESET";
        public const string ModuleProfileOtp = "PROFILE_OTP";
        public const string ModuleJobAlert = "JOB_ALERT";
        public const string ModuleJobInpunch = "JOB_INPUNCH";
        public const string ModuleHelpdesk = "HELPDESK";
        public const string ModulePayroll = "PAYROLL";
        public const string ModuleSupplyMemo = "SUPPLY_MEMO";
        public const string ModuleEmployeeMaster = "EMPLOYEE_MASTER";
        public const string ModuleSystem = "SYSTEM";

        const string CacheKey = "ATS_NotificationTriggers";
        const int CacheSeconds = 30;

        public struct ChannelFlags
        {
            public bool Email;
            public bool WhatsApp;
        }

        public class TriggerRow
        {
            public string TriggerKey { get; set; }
            public string DisplayName { get; set; }
            public string Scope { get; set; }
            public int SortOrder { get; set; }
            public bool EmailEnabled { get; set; }
            public bool WhatsAppEnabled { get; set; }
        }

        static readonly TriggerRow[] Defaults = new TriggerRow[]
        {
            new TriggerRow { TriggerKey = KeyPortal, DisplayName = "Portal (all modules)", Scope = ScopePortal, SortOrder = 0 },
            new TriggerRow { TriggerKey = ModuleLoginMfa, DisplayName = "Login MFA OTP", Scope = ScopeAuth, SortOrder = 10 },
            new TriggerRow { TriggerKey = ModulePasswordReset, DisplayName = "Login password reset OTP", Scope = ScopeAuth, SortOrder = 20 },
            new TriggerRow { TriggerKey = ModuleProfileOtp, DisplayName = "Profile / email verification OTP", Scope = ScopeAuth, SortOrder = 30 },
            new TriggerRow { TriggerKey = ModuleJobAlert, DisplayName = "Job out-punch / share alerts", Scope = ScopeModule, SortOrder = 40 },
            new TriggerRow { TriggerKey = ModuleJobInpunch, DisplayName = "Job in-punch alerts", Scope = ScopeModule, SortOrder = 50 },
            new TriggerRow { TriggerKey = ModuleHelpdesk, DisplayName = "Helpdesk tickets", Scope = ScopeModule, SortOrder = 60 },
            new TriggerRow { TriggerKey = ModulePayroll, DisplayName = "Payroll notifications", Scope = ScopeModule, SortOrder = 70 },
            new TriggerRow { TriggerKey = ModuleSupplyMemo, DisplayName = "Supply memo", Scope = ScopeModule, SortOrder = 80 },
            new TriggerRow { TriggerKey = ModuleEmployeeMaster, DisplayName = "Employee master emails", Scope = ScopeModule, SortOrder = 90 },
            new TriggerRow { TriggerKey = ModuleSystem, DisplayName = "System / utility emails", Scope = ScopeModule, SortOrder = 100 }
        };

        public static bool IsAuthenticationOtp(string moduleKey)
        {
            if (string.IsNullOrWhiteSpace(moduleKey)) return false;
            return moduleKey.Equals(ModuleLoginMfa, StringComparison.OrdinalIgnoreCase)
                || moduleKey.Equals(ModulePasswordReset, StringComparison.OrdinalIgnoreCase)
                || moduleKey.Equals(ModuleProfileOtp, StringComparison.OrdinalIgnoreCase);
        }

        public static bool EffectiveEnabled(bool portalOn, bool moduleOn, bool tableMissing, bool isAuthOtp)
        {
            if (tableMissing) return true;
            if (isAuthOtp) return moduleOn;
            return portalOn && moduleOn;
        }

        public static bool IsEmailEnabled(string moduleKey)
        {
            Dictionary<string, ChannelFlags> map;
            if (!TryLoadMap(out map)) return true;
            bool isAuth = IsAuthenticationOtp(moduleKey);
            return EffectiveEnabled(GetFlags(map, KeyPortal).Email, GetFlags(map, moduleKey).Email, false, isAuth);
        }

        public static bool IsWhatsAppEnabled(string moduleKey)
        {
            Dictionary<string, ChannelFlags> map;
            if (!TryLoadMap(out map)) return true;
            bool isAuth = IsAuthenticationOtp(moduleKey);
            return EffectiveEnabled(GetFlags(map, KeyPortal).WhatsApp, GetFlags(map, moduleKey).WhatsApp, false, isAuth);
        }

        public static bool IsMissingTableException(Exception ex)
        {
            if (ex == null) return false;
            string msg = ex.Message ?? "";
            return msg.IndexOf("Invalid object name", StringComparison.OrdinalIgnoreCase) >= 0
                || msg.IndexOf("tbl_Notification_Triggers", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static void InvalidateCache()
        {
            try { HttpRuntime.Cache.Remove(CacheKey); }
            catch { }
        }

        public static bool TryLoadRows(out List<TriggerRow> rows, out string error)
        {
            rows = new List<TriggerRow>();
            error = "";
            try
            {
                EnsureDefaultRows();
                DB_Utility_OH4Y db = new DB_Utility_OH4Y();
                DataTable dt = db.SPreturn_dt(
                    @"SELECT TriggerKey, DisplayName, Scope, SortOrder, EmailEnabled, WhatsAppEnabled
                      FROM tbl_Notification_Triggers ORDER BY SortOrder, TriggerKey",
                    null);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    TriggerRow row = new TriggerRow();
                    row.TriggerKey = r["TriggerKey"].ToString();
                    row.DisplayName = r["DisplayName"].ToString();
                    row.Scope = r["Scope"].ToString();
                    row.SortOrder = r["SortOrder"] == DBNull.Value ? 0 : Convert.ToInt32(r["SortOrder"]);
                    row.EmailEnabled = MfaAuthHelper.IsEnabled(r["EmailEnabled"]);
                    row.WhatsAppEnabled = MfaAuthHelper.IsEnabled(r["WhatsAppEnabled"]);
                    rows.Add(row);
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public static bool TrySaveRow(string triggerKey, bool emailEnabled, bool whatsAppEnabled, string updatedBy, out string error)
        {
            error = "";
            try
            {
                DB_Utility_OH4Y db = new DB_Utility_OH4Y();
                db.SPreturn_dt(
                    @"UPDATE tbl_Notification_Triggers
                      SET EmailEnabled=@Email, WhatsAppEnabled=@WA, UpdatedBy=@By, UpdatedOn=GETDATE()
                      WHERE TriggerKey=@Key",
                    new SqlParameter[]
                    {
                        new SqlParameter("@Email", emailEnabled ? 1 : 0),
                        new SqlParameter("@WA", whatsAppEnabled ? 1 : 0),
                        new SqlParameter("@By", (object)updatedBy ?? DBNull.Value),
                        new SqlParameter("@Key", triggerKey)
                    });
                InvalidateCache();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        static ChannelFlags GetFlags(Dictionary<string, ChannelFlags> map, string key)
        {
            ChannelFlags flags;
            if (string.IsNullOrWhiteSpace(key) || !map.TryGetValue(key, out flags))
            {
                flags.Email = true;
                flags.WhatsApp = true;
            }
            return flags;
        }

        static bool TryLoadMap(out Dictionary<string, ChannelFlags> map)
        {
            map = HttpRuntime.Cache[CacheKey] as Dictionary<string, ChannelFlags>;
            if (map != null) return true;

            try
            {
                DB_Utility_OH4Y db = new DB_Utility_OH4Y();
                DataTable dt = db.SPreturn_dt(
                    "SELECT TriggerKey, EmailEnabled, WhatsAppEnabled FROM tbl_Notification_Triggers",
                    null);
                map = new Dictionary<string, ChannelFlags>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    ChannelFlags flags;
                    flags.Email = MfaAuthHelper.IsEnabled(r["EmailEnabled"]);
                    flags.WhatsApp = MfaAuthHelper.IsEnabled(r["WhatsAppEnabled"]);
                    map[r["TriggerKey"].ToString()] = flags;
                }
                HttpRuntime.Cache.Insert(
                    CacheKey,
                    map,
                    null,
                    DateTime.UtcNow.AddSeconds(CacheSeconds),
                    System.Web.Caching.Cache.NoSlidingExpiration);
                return true;
            }
            catch (Exception ex)
            {
                map = null;
                if (IsMissingTableException(ex)) return false;
                return false;
            }
        }

        static void EnsureDefaultRows()
        {
            DB_Utility_OH4Y db = new DB_Utility_OH4Y();
            for (int i = 0; i < Defaults.Length; i++)
            {
                TriggerRow def = Defaults[i];
                db.SPreturn_dt(
                    @"IF NOT EXISTS (SELECT 1 FROM tbl_Notification_Triggers WHERE TriggerKey=@Key)
                      INSERT INTO tbl_Notification_Triggers (TriggerKey, DisplayName, Scope, SortOrder, EmailEnabled, WhatsAppEnabled)
                      VALUES (@Key, @Name, @Scope, @Sort, 1, 1)
                      ELSE
                      UPDATE tbl_Notification_Triggers SET DisplayName=@Name, Scope=@Scope, SortOrder=@Sort WHERE TriggerKey=@Key",
                    new SqlParameter[]
                    {
                        new SqlParameter("@Key", def.TriggerKey),
                        new SqlParameter("@Name", def.DisplayName),
                        new SqlParameter("@Scope", def.Scope),
                        new SqlParameter("@Sort", def.SortOrder)
                    });
            }
            InvalidateCache();
        }
    }
}
