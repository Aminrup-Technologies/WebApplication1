/*
 * WHEN: 2026-02-27
 * WHY: Fixing the tab reset bug by manipulating the tab HTML classes directly from the server.
 * WHAT: Implemented server-side control over tab_login_btn, tab_forgot_btn, pane_login, and pane_forgot in Page_PreRender.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using WebApplication1.bussiness.production;

namespace WebApplication1.bussiness.production
{
    public partial class login : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        static readonly string rootFolder = HostingEnvironment.MapPath("~/erp_images/ProfilePhoto");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["ATS_SavedID"] != null)
                {
                    txt_loginid.Text = Request.Cookies["ATS_SavedID"].Value;
                    chk_remember.Checked = true;
                }
                ViewState["ForgotMode"] = false;
                ViewState["MfaMode"] = Session[SessionKeys.MfaPendingLoginId] != null;
                if ((bool)ViewState["MfaMode"])
                {
                    RefreshMfaHint();
                }
            }
        }

        // --- NEW FIX: Server-side Tab Control ---
        protected void Page_PreRender(object sender, EventArgs e)
        {
            bool mfaMode = (ViewState["MfaMode"] != null && (bool)ViewState["MfaMode"])
                || Session[SessionKeys.MfaPendingLoginId] != null;

            if (mfaMode)
            {
                ViewState["MfaMode"] = true;
                loginTabs.Visible = false;
                pane_login.Attributes["class"] = "tab-pane fade";
                pane_forgot.Attributes["class"] = "tab-pane fade";
                pane_mfa.Visible = true;
                pane_mfa.Attributes["class"] = "tab-pane fade show active";
                ApplyMfaPaneState();
                return;
            }

            loginTabs.Visible = true;
            pane_mfa.Visible = false;
            pane_mfa.Attributes["class"] = "tab-pane fade";

            if (ViewState["ForgotMode"] != null && (bool)ViewState["ForgotMode"])
            {
                // Deactivate Login Tab
                tab_login_btn.Attributes["class"] = "nav-link";
                pane_login.Attributes["class"] = "tab-pane fade";
                tab_login_btn.Attributes["aria-selected"] = "false";

                // Activate Forgot Password Tab
                tab_forgot_btn.Attributes["class"] = "nav-link active";
                pane_forgot.Attributes["class"] = "tab-pane fade show active";
                tab_forgot_btn.Attributes["aria-selected"] = "true";
            }
            else
            {
                // Activate Login Tab
                tab_login_btn.Attributes["class"] = "nav-link active";
                pane_login.Attributes["class"] = "tab-pane fade show active";
                tab_login_btn.Attributes["aria-selected"] = "true";

                // Deactivate Forgot Password Tab
                tab_forgot_btn.Attributes["class"] = "nav-link";
                pane_forgot.Attributes["class"] = "tab-pane fade";
                tab_forgot_btn.Attributes["aria-selected"] = "false";
            }
        }

        /* ================= LOGIN ================= */

        protected void btn_login_Click(object sender, EventArgs e)
        {
            try { PerformLogin(txt_loginid.Text.Trim(), txt_password.Text.Trim()); }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                Notify("Error", "Unexpected error occurred.", "error");
                dbcl.WriteToFile(ex.ToString());
            }
        }

        // Opt-in phase timings for diagnosing slow logins on a specific environment.
        // Enable by adding <add key="EnableLoginTimingLog" value="true" /> to appSettings.
        private static readonly bool EnableLoginTimingLog =
            string.Equals(System.Configuration.ConfigurationManager.AppSettings["EnableLoginTimingLog"],
                          "true", StringComparison.OrdinalIgnoreCase);

        private void LogLoginTiming(string phase, Stopwatch sw, string loginId)
        {
            if (!EnableLoginTimingLog) return;
            try
            {
                dbcl.WriteToFile("LOGIN-TIMING [" + loginId + "] [" + phase + "] " + sw.ElapsedMilliseconds + "ms");
            }
            catch
            {
                // Logging must never break the login flow; swallow I/O failures.
            }
        }

        private DataTable FetchLoginUser(string id)
        {
            string query = @"
                SELECT TOP 1 
                    LoginID, LoginPassword, WorkStatus, DOR, PasswordExpiry, WorkmanSL, FirstName, FullName,
                    User_RoleType, UserRoleDB, RolePermissionDB, WorkRegion, WorkState, WorkCompany,
                    WorkSite, Worksite_Code, SkillDesignation, SkillCategory, PrfPicFile, Email
                FROM tbl_Employee_Mustertable WHERE LoginID = @LoginID";

            return dbcl.SPreturn_dt(query, new SqlParameter[] { new SqlParameter("@LoginID", id) });
        }

        private void PerformLogin(string id, string pass)
        {
            Stopwatch sw = Stopwatch.StartNew();

            string inputHashedPass = HashPassword(pass);
            DataTable dt = FetchLoginUser(id);
            LogLoginTiming("fetchUser", sw, id);

            if (dt.Rows.Count == 0)
            {
                InsertLoginAudit(id, null, "FAILED", "InvalidUser");
                Notify("Failed", "Invalid ID or Password.", "error");
                return;
            }

            DataRow row = dt.Rows[0];
            string storedPassword = row["LoginPassword"].ToString();
            string workmanSL = row["WorkmanSL"].ToString();

            bool isPasswordValid = false;
            bool needsMigration = false;

            // Password Validation (With Legacy Migration Check)
            if (storedPassword == inputHashedPass) { isPasswordValid = true; }
            else if (storedPassword == pass) { isPasswordValid = true; needsMigration = true; }

            if (!isPasswordValid)
            {
                InsertLoginAudit(id, workmanSL, "FAILED", "InvalidPassword");
                Notify("Failed", "Invalid ID or Password.", "error");
                return;
            }

            // Migrate legacy plain-text password to hash regardless of account status,
            // so plaintext never lingers for blocked accounts either.
            if (needsMigration)
            {
                dbcl.SPreturn_dt("UPDATE tbl_Employee_Mustertable SET LoginPassword=@NewHash WHERE LoginID=@ID",
                    new SqlParameter[] { new SqlParameter("@NewHash", inputHashedPass), new SqlParameter("@ID", id) });
            }

            // Active Status Check
            if (row["WorkStatus"].ToString() != "Active")
            {
                InsertLoginAudit(id, workmanSL, "BLOCKED", "Inactive");
                Notify("Denied", "Account is inactive.", "error");
                return;
            }

            bool mfaEnabled = false;
            string mfaMethod = MfaAuthHelper.MethodEmailOtp;
            bool totpEnrolled = false;
            TryReadMfaSettings(id, ref mfaEnabled, ref mfaMethod, ref totpEnrolled);
            if (mfaEnabled)
            {
                if (MfaAuthHelper.IsAuthenticator(mfaMethod))
                {
                    if (totpEnrolled)
                    {
                        StartTotpChallenge(id, workmanSL, chk_remember.Checked);
                    }
                    else
                    {
                        StartTotpEnroll(id, workmanSL, chk_remember.Checked);
                    }
                    LogLoginTiming("mfaChallenge", sw, id);
                    return;
                }

                string email = row["Email"] != DBNull.Value ? row["Email"].ToString() : "";
                if (!MfaAuthHelper.HasEmail(email))
                {
                    InsertLoginAudit(id, workmanSL, "MFA_NO_EMAIL", "MfaEmailMissing");
                    Notify("MFA Required", "MFA is enabled for this account but no email is registered. Contact your administrator.", "error");
                    return;
                }

                StartMfaChallenge(id, workmanSL, email, row["FullName"].ToString(), chk_remember.Checked);
                LogLoginTiming("mfaChallenge", sw, id);
                return;
            }

            GrantAuthenticatedSession(row, chk_remember.Checked, "SUCCESS", false);
            LogLoginTiming("total", sw, id);
        }

        private void TryReadMfaSettings(string loginId, ref bool enabled, ref string method, ref bool totpEnrolled)
        {
            enabled = false;
            method = MfaAuthHelper.MethodEmailOtp;
            totpEnrolled = false;
            try
            {
                DataTable dt = dbcl.SPreturn_dt(
                    @"SELECT ISNULL(MFAEnabled, 0) AS MFAEnabled,
                             ISNULL(MFAMethod, 'EmailOTP') AS MFAMethod,
                             ISNULL(MFATotpEnrolled, 0) AS MFATotpEnrolled
                      FROM tbl_Employee_Mustertable WHERE LoginID=@LoginID",
                    new SqlParameter[] { new SqlParameter("@LoginID", loginId) });
                if (dt.Rows.Count == 0) return;
                enabled = MfaAuthHelper.IsEnabled(dt.Rows[0]["MFAEnabled"]);
                method = MfaAuthHelper.NormalizeMethod(dt.Rows[0]["MFAMethod"].ToString());
                totpEnrolled = MfaAuthHelper.IsEnabled(dt.Rows[0]["MFATotpEnrolled"]);
            }
            catch (Exception ex)
            {
                if (!MfaAuthHelper.IsMissingColumnException(ex)) throw;
                try
                {
                    DataTable fallback = dbcl.SPreturn_dt(
                        @"SELECT ISNULL(MFAEnabled, 0) AS MFAEnabled,
                                 ISNULL(MFAMethod, 'EmailOTP') AS MFAMethod
                          FROM tbl_Employee_Mustertable WHERE LoginID=@LoginID",
                        new SqlParameter[] { new SqlParameter("@LoginID", loginId) });
                    if (fallback.Rows.Count == 0) return;
                    enabled = MfaAuthHelper.IsEnabled(fallback.Rows[0]["MFAEnabled"]);
                    method = MfaAuthHelper.NormalizeMethod(fallback.Rows[0]["MFAMethod"].ToString());
                    totpEnrolled = false;
                }
                catch (Exception inner)
                {
                    if (!MfaAuthHelper.IsMissingColumnException(inner)) throw;
                    enabled = TryReadMfaEnabled(loginId);
                    method = MfaAuthHelper.MethodEmailOtp;
                    totpEnrolled = false;
                }
            }
        }

        private bool TryReadMfaEnabled(string loginId)
        {
            try
            {
                DataTable dt = dbcl.SPreturn_dt(
                    "SELECT ISNULL(MFAEnabled, 0) AS MFAEnabled FROM tbl_Employee_Mustertable WHERE LoginID=@LoginID",
                    new SqlParameter[] { new SqlParameter("@LoginID", loginId) });
                if (dt.Rows.Count == 0) return false;
                return MfaAuthHelper.IsEnabled(dt.Rows[0]["MFAEnabled"]);
            }
            catch (Exception ex)
            {
                if (MfaAuthHelper.IsMissingColumnException(ex))
                {
                    dbcl.WriteToFile("MFA columns not present; treating MFA as disabled for " + loginId);
                    return false;
                }
                throw;
            }
        }

        private void StartMfaChallenge(string loginId, string workmanSL, string email, string fullName, bool rememberMe)
        {
            ClearMfaSession();

            string otp = GenerateOTP();
            Session[SessionKeys.MfaPendingLoginId] = loginId;
            Session[SessionKeys.MfaOtpHash] = HashPassword(otp);
            Session[SessionKeys.MfaOtpExp] = DateTime.Now.AddMinutes(MfaAuthHelper.OtpLifetimeMinutes);
            Session[SessionKeys.MfaOtpTry] = 0;
            Session[SessionKeys.MfaOtpEmail] = email.Trim();
            Session[SessionKeys.MfaRemember] = rememberMe;
            Session[SessionKeys.MfaResendAt] = DateTime.Now.AddSeconds(MfaAuthHelper.ResendCooldownSeconds);
            Session[SessionKeys.MfaMethod] = MfaAuthHelper.MethodEmailOtp;
            Session[SessionKeys.MfaTotpEnroll] = false;

            InsertLoginAudit(loginId, workmanSL, "MFA_CHALLENGE", null);

            ViewState["MfaMode"] = true;
            ViewState["ForgotMode"] = false;
            RefreshMfaHint();

            if (!SendMfaOtpEmail(email.Trim(), otp, fullName))
            {
                Session[SessionKeys.MfaResendAt] = DateTime.Now;
                Notify("Error", "Could not send the verification code. Use Resend Code, or contact support.", "error");
                return;
            }

            Notify("Code Sent", "A verification code was sent to " + MfaAuthHelper.MaskEmail(email) + ".", "success");
        }

        private void RefreshMfaHint()
        {
            if (MfaAuthHelper.IsAuthenticator(Session[SessionKeys.MfaMethod] as string))
            {
                if (Session[SessionKeys.MfaTotpEnroll] is bool && (bool)Session[SessionKeys.MfaTotpEnroll])
                {
                    lbl_mfa_hint.Text = "Scan the QR code with Google Authenticator or Microsoft Authenticator, then enter the 6-digit code.";
                }
                else
                {
                    lbl_mfa_hint.Text = "Enter the 6-digit code from your authenticator app. Codes refresh every 30 seconds.";
                }
                return;
            }

            string email = Session[SessionKeys.MfaOtpEmail] as string;
            lbl_mfa_hint.Text = "Enter the 6-digit code sent to " + MfaAuthHelper.MaskEmail(email) + ". The code expires in "
                + MfaAuthHelper.OtpLifetimeMinutes + " minutes.";
        }

        private void ApplyMfaPaneState()
        {
            bool totp = MfaAuthHelper.IsAuthenticator(Session[SessionKeys.MfaMethod] as string);
            bool enroll = totp && Session[SessionKeys.MfaTotpEnroll] is bool && (bool)Session[SessionKeys.MfaTotpEnroll];
            btn_mfa_resend.Visible = !totp;
            ph_mfa_enroll.Visible = enroll;
            if (enroll)
            {
                string secret = Session[SessionKeys.MfaTotpSecret] as string;
                string loginId = Session[SessionKeys.MfaPendingLoginId] as string;
                hf_mfa_otpauth.Value = MfaTotpHelper.BuildOtpAuthUri(loginId, secret);
                lbl_mfa_manual.Text = secret ?? "";
                ScriptManager.RegisterStartupScript(this, GetType(), "mfaqr",
                    "window.setTimeout(function(){ if (window.renderMfaQr) { renderMfaQr(); } }, 50);", true);
            }
            else
            {
                hf_mfa_otpauth.Value = "";
                lbl_mfa_manual.Text = "";
            }
            RefreshMfaHint();
        }

        private void StartTotpChallenge(string loginId, string workmanSL, bool rememberMe)
        {
            ClearMfaSession();
            Session[SessionKeys.MfaPendingLoginId] = loginId;
            Session[SessionKeys.MfaRemember] = rememberMe;
            Session[SessionKeys.MfaMethod] = MfaAuthHelper.MethodAuthenticator;
            Session[SessionKeys.MfaTotpEnroll] = false;
            Session[SessionKeys.MfaOtpTry] = 0;
            Session[SessionKeys.MfaOtpExp] = DateTime.Now.AddMinutes(10);
            InsertLoginAudit(loginId, workmanSL, "MFA_CHALLENGE", "Totp");
            ViewState["MfaMode"] = true;
            ViewState["ForgotMode"] = false;
            RefreshMfaHint();
            Notify("Authenticator", "Enter the code from your authenticator app.", "info");
        }

        private void StartTotpEnroll(string loginId, string workmanSL, bool rememberMe)
        {
            ClearMfaSession();
            Session[SessionKeys.MfaPendingLoginId] = loginId;
            Session[SessionKeys.MfaRemember] = rememberMe;
            Session[SessionKeys.MfaMethod] = MfaAuthHelper.MethodAuthenticator;
            Session[SessionKeys.MfaTotpEnroll] = true;
            Session[SessionKeys.MfaTotpSecret] = MfaTotpHelper.GenerateSecret();
            Session[SessionKeys.MfaOtpTry] = 0;
            Session[SessionKeys.MfaOtpExp] = DateTime.Now.AddMinutes(10);
            InsertLoginAudit(loginId, workmanSL, "MFA_CHALLENGE", "TotpEnroll");
            ViewState["MfaMode"] = true;
            ViewState["ForgotMode"] = false;
            RefreshMfaHint();
            Notify("Set up authenticator", "Scan the QR code, then enter the 6-digit code to finish setup.", "info");
        }

        protected void btn_mfa_verify_Click(object sender, EventArgs e)
        {
            try { VerifyMfaAndCompleteLogin(); }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                ViewState["MfaMode"] = true;
                Notify("Error", "Unexpected error occurred.", "error");
                dbcl.WriteToFile(ex.ToString());
            }
        }

        private void VerifyMfaAndCompleteLogin()
        {
            ViewState["MfaMode"] = true;

            if (MfaAuthHelper.IsAuthenticator(Session[SessionKeys.MfaMethod] as string))
            {
                VerifyTotpAndCompleteLogin();
                return;
            }

            string pendingId = Session[SessionKeys.MfaPendingLoginId] as string;
            if (string.IsNullOrEmpty(pendingId) || Session[SessionKeys.MfaOtpHash] == null || Session[SessionKeys.MfaOtpExp] == null)
            {
                ClearMfaSession();
                ViewState["MfaMode"] = false;
                Notify("Expired", "Verification session expired. Please log in again.", "error");
                return;
            }

            if (DateTime.Now > (DateTime)Session[SessionKeys.MfaOtpExp])
            {
                InsertLoginAudit(pendingId, null, "MFA_EXPIRED", "OtpExpired");
                Notify("Expired", "Verification code expired. Use Resend Code.", "error");
                return;
            }

            Session[SessionKeys.MfaOtpTry] = (int)(Session[SessionKeys.MfaOtpTry] ?? 0) + 1;
            if ((int)Session[SessionKeys.MfaOtpTry] > MfaAuthHelper.MaxOtpAttempts)
            {
                InsertLoginAudit(pendingId, null, "MFA_FAILED", "TooManyAttempts");
                ClearMfaSession();
                ViewState["MfaMode"] = false;
                Notify("Blocked", "Too many attempts. Please log in again.", "error");
                return;
            }

            string enteredHash = HashPassword((txt_mfa_otp.Text ?? "").Trim());
            if (!MfaAuthHelper.SlowEquals(enteredHash, Session[SessionKeys.MfaOtpHash].ToString()))
            {
                InsertLoginAudit(pendingId, null, "MFA_FAILED", "InvalidOtp");
                Notify("Invalid", "Wrong verification code.", "error");
                return;
            }

            bool rememberMe = Session[SessionKeys.MfaRemember] is bool && (bool)Session[SessionKeys.MfaRemember];
            DataTable dt = FetchLoginUser(pendingId);
            if (dt.Rows.Count == 0)
            {
                ClearMfaSession();
                ViewState["MfaMode"] = false;
                Notify("Failed", "Account was not found. Please log in again.", "error");
                return;
            }

            DataRow row = dt.Rows[0];
            if (row["WorkStatus"].ToString() != "Active")
            {
                InsertLoginAudit(pendingId, row["WorkmanSL"].ToString(), "BLOCKED", "Inactive");
                ClearMfaSession();
                ViewState["MfaMode"] = false;
                Notify("Denied", "Account is inactive.", "error");
                return;
            }

            ClearMfaSession();
            ViewState["MfaMode"] = false;
            GrantAuthenticatedSession(row, rememberMe, "SUCCESS_MFA", true);
        }

        private void VerifyTotpAndCompleteLogin()
        {
            string pendingId = Session[SessionKeys.MfaPendingLoginId] as string;
            if (string.IsNullOrEmpty(pendingId) || Session[SessionKeys.MfaOtpExp] == null)
            {
                ClearMfaSession();
                ViewState["MfaMode"] = false;
                Notify("Expired", "Verification session expired. Please log in again.", "error");
                return;
            }

            if (DateTime.Now > (DateTime)Session[SessionKeys.MfaOtpExp])
            {
                InsertLoginAudit(pendingId, null, "MFA_EXPIRED", "TotpSessionExpired");
                ClearMfaSession();
                ViewState["MfaMode"] = false;
                Notify("Expired", "Verification session expired. Please log in again.", "error");
                return;
            }

            Session[SessionKeys.MfaOtpTry] = (int)(Session[SessionKeys.MfaOtpTry] ?? 0) + 1;
            if ((int)Session[SessionKeys.MfaOtpTry] > MfaAuthHelper.MaxOtpAttempts)
            {
                InsertLoginAudit(pendingId, null, "MFA_FAILED", "TooManyAttempts");
                ClearMfaSession();
                ViewState["MfaMode"] = false;
                Notify("Blocked", "Too many attempts. Please log in again.", "error");
                return;
            }

            bool enroll = Session[SessionKeys.MfaTotpEnroll] is bool && (bool)Session[SessionKeys.MfaTotpEnroll];
            string secret = enroll
                ? (Session[SessionKeys.MfaTotpSecret] as string)
                : ReadStoredTotpSecret(pendingId);

            if (string.IsNullOrEmpty(secret) || !MfaTotpHelper.ValidateCode(secret, txt_mfa_otp.Text))
            {
                InsertLoginAudit(pendingId, null, "MFA_FAILED", enroll ? "InvalidTotpEnroll" : "InvalidTotp");
                Notify("Invalid", "Wrong authenticator code.", "error");
                return;
            }

            DataTable dt = FetchLoginUser(pendingId);
            if (dt.Rows.Count == 0)
            {
                ClearMfaSession();
                ViewState["MfaMode"] = false;
                Notify("Failed", "Account was not found. Please log in again.", "error");
                return;
            }

            DataRow row = dt.Rows[0];
            if (row["WorkStatus"].ToString() != "Active")
            {
                InsertLoginAudit(pendingId, row["WorkmanSL"].ToString(), "BLOCKED", "Inactive");
                ClearMfaSession();
                ViewState["MfaMode"] = false;
                Notify("Denied", "Account is inactive.", "error");
                return;
            }

            if (enroll && !PersistTotpEnrollment(pendingId, secret))
            {
                Notify("Error", "Could not save authenticator setup. Ask an administrator to run the TOTP database script.", "error");
                return;
            }

            bool rememberMe = Session[SessionKeys.MfaRemember] is bool && (bool)Session[SessionKeys.MfaRemember];
            ClearMfaSession();
            ViewState["MfaMode"] = false;
            GrantAuthenticatedSession(row, rememberMe, "SUCCESS_MFA", true);
        }

        private string ReadStoredTotpSecret(string loginId)
        {
            try
            {
                DataTable dt = dbcl.SPreturn_dt(
                    "SELECT MFATotpSecret FROM tbl_Employee_Mustertable WHERE LoginID=@LoginID",
                    new SqlParameter[] { new SqlParameter("@LoginID", loginId) });
                if (dt.Rows.Count == 0 || dt.Rows[0]["MFATotpSecret"] == DBNull.Value) return "";
                return dt.Rows[0]["MFATotpSecret"].ToString().Trim();
            }
            catch (Exception ex)
            {
                if (MfaAuthHelper.IsMissingColumnException(ex)) return "";
                throw;
            }
        }

        private bool PersistTotpEnrollment(string loginId, string secret)
        {
            try
            {
                dbcl.SPreturn_dt(
                    @"UPDATE tbl_Employee_Mustertable
                      SET MFATotpSecret=@Secret, MFATotpEnrolled=1, MFAMethod=@Method
                      WHERE LoginID=@LoginID",
                    new SqlParameter[]
                    {
                        new SqlParameter("@Secret", secret),
                        new SqlParameter("@Method", MfaAuthHelper.MethodAuthenticator),
                        new SqlParameter("@LoginID", loginId)
                    });
                return true;
            }
            catch (Exception ex)
            {
                dbcl.WriteToFile("TOTP enrollment persist failed: " + ex);
                return false;
            }
        }

        protected void btn_mfa_resend_Click(object sender, EventArgs e)
        {
            try { ResendMfaOtp(); }
            catch (Exception ex)
            {
                ViewState["MfaMode"] = true;
                Notify("Error", "Unexpected error occurred.", "error");
                dbcl.WriteToFile(ex.ToString());
            }
        }

        private void ResendMfaOtp()
        {
            ViewState["MfaMode"] = true;
            if (MfaAuthHelper.IsAuthenticator(Session[SessionKeys.MfaMethod] as string))
            {
                Notify("Authenticator", "Authenticator codes refresh every 30 seconds. Wait for the next code.", "notice");
                return;
            }
            string pendingId = Session[SessionKeys.MfaPendingLoginId] as string;
            if (string.IsNullOrEmpty(pendingId))
            {
                ViewState["MfaMode"] = false;
                Notify("Expired", "Verification session expired. Please log in again.", "error");
                return;
            }

            if (Session[SessionKeys.MfaResendAt] != null)
            {
                DateTime resendAt = (DateTime)Session[SessionKeys.MfaResendAt];
                if (DateTime.Now < resendAt)
                {
                    int wait = Math.Max(1, (int)Math.Ceiling((resendAt - DateTime.Now).TotalSeconds));
                    Notify("Wait", "Please wait " + wait + " seconds before requesting another code.", "notice");
                    return;
                }
            }

            DataTable dt = FetchLoginUser(pendingId);
            if (dt.Rows.Count == 0)
            {
                ClearMfaSession();
                ViewState["MfaMode"] = false;
                Notify("Failed", "Account was not found. Please log in again.", "error");
                return;
            }

            DataRow row = dt.Rows[0];
            string email = row["Email"] != DBNull.Value ? row["Email"].ToString() : "";
            if (!MfaAuthHelper.HasEmail(email))
            {
                InsertLoginAudit(pendingId, row["WorkmanSL"].ToString(), "MFA_NO_EMAIL", "MfaEmailMissing");
                Notify("MFA Required", "MFA is enabled for this account but no email is registered. Contact your administrator.", "error");
                return;
            }

            bool rememberMe = Session[SessionKeys.MfaRemember] is bool && (bool)Session[SessionKeys.MfaRemember];
            StartMfaChallenge(pendingId, row["WorkmanSL"].ToString(), email, row["FullName"].ToString(), rememberMe);
        }

        protected void btn_mfa_back_Click(object sender, EventArgs e)
        {
            ClearMfaSession();
            ViewState["MfaMode"] = false;
            txt_mfa_otp.Text = "";
            Notify("Login", "Verification cancelled. Enter your password again.", "info");
        }

        private void ClearMfaSession()
        {
            Session.Remove(SessionKeys.MfaPendingLoginId);
            Session.Remove(SessionKeys.MfaOtpHash);
            Session.Remove(SessionKeys.MfaOtpExp);
            Session.Remove(SessionKeys.MfaOtpTry);
            Session.Remove(SessionKeys.MfaOtpEmail);
            Session.Remove(SessionKeys.MfaRemember);
            Session.Remove(SessionKeys.MfaResendAt);
            Session.Remove(SessionKeys.MfaMethod);
            Session.Remove(SessionKeys.MfaTotpEnroll);
            Session.Remove(SessionKeys.MfaTotpSecret);
        }

        private void GrantAuthenticatedSession(DataRow row, bool rememberMe, string auditResult, bool markMfaVerified)
        {
            string id = row["LoginID"].ToString();
            string workmanSL = row["WorkmanSL"].ToString();

            string postAuthSql = @"
                INSERT INTO tbl_UserLoginAudit (LoginID, WorkmanSL, LoginTime, LoginResult, FailureReason, IPAddress, UserAgent, SessionID)
                VALUES (@LoginID, @WorkmanSL, GETDATE(), @Result, NULL, @IP, @Agent, @SessionID);
                UPDATE tbl_Employee_Mustertable SET LastLogin = GETDATE(), LoginStatus = 1 WHERE LoginID = @LoginID;";

            List<SqlParameter> postAuthParams = new List<SqlParameter>
            {
                new SqlParameter("@LoginID", id),
                new SqlParameter("@WorkmanSL", (object)workmanSL ?? DBNull.Value),
                new SqlParameter("@Result", auditResult),
                new SqlParameter("@IP", Request.UserHostAddress ?? ""),
                new SqlParameter("@Agent", Request.UserAgent ?? ""),
                new SqlParameter("@SessionID", Session.SessionID)
            };

            dbcl.SPreturn_dt(postAuthSql, postAuthParams.ToArray());

            if (markMfaVerified)
            {
                try
                {
                    dbcl.SPreturn_dt(
                        "UPDATE tbl_Employee_Mustertable SET MFALastVerified = GETDATE() WHERE LoginID = @LoginID",
                        new SqlParameter[] { new SqlParameter("@LoginID", id) });
                }
                catch (Exception ex)
                {
                    if (!MfaAuthHelper.IsMissingColumnException(ex))
                    {
                        dbcl.WriteToFile("MFALastVerified update failed: " + ex);
                    }
                }
            }

            if (rememberMe) { Response.Cookies.Add(new HttpCookie("ATS_SavedID", id) { Expires = DateTime.Now.AddDays(15) }); }
            else if (Request.Cookies["ATS_SavedID"] != null) { Response.Cookies["ATS_SavedID"].Expires = DateTime.Now.AddDays(-1); }

            Session[SessionKeys.UserID] = id;
            Session[SessionKeys.WorkmanSL] = workmanSL;
            Session[SessionKeys.UserFirstName] = row["FirstName"].ToString();
            Session[SessionKeys.UserName] = row["FullName"].ToString();
            Session[SessionKeys.UserType] = row["User_RoleType"].ToString();
            Session[SessionKeys.UserRoleDB] = row["UserRoleDB"].ToString();
            Session[SessionKeys.RolePermissionDB] = row["RolePermissionDB"].ToString();
            Session[SessionKeys.Region] = row["WorkRegion"].ToString();
            Session[SessionKeys.UserState] = row["WorkState"].ToString();
            Session[SessionKeys.CompanyCode] = row["WorkCompany"].ToString();
            Session[SessionKeys.WorkSite] = row["WorkSite"].ToString();
            Session[SessionKeys.SiteCode] = row["Worksite_Code"].ToString();
            Session[SessionKeys.Designation] = row["SkillDesignation"].ToString();
            Session[SessionKeys.Skill] = row["SkillCategory"].ToString();

            string photo = row["PrfPicFile"].ToString();
            Session[SessionKeys.UserPhoto] = (!string.IsNullOrEmpty(photo) && Directory.Exists(rootFolder) && File.Exists(Path.Combine(rootFolder, photo)))
                                            ? photo
                                            : "No_Image.jpg";

            Response.Redirect("~/bussiness/production/homepage_v2.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private bool SendMfaOtpEmail(string to, string otp, string name)
        {
            try
            {
                MailMessage mm = new MailMessage("it.support@aminruptechnologies.co.in", to);
                mm.Subject = "ATS Login Verification OTP";
                mm.Body = "Hi " + name + ",\n\nYour login verification code is: " + otp
                    + "\nThis code is valid for " + MfaAuthHelper.OtpLifetimeMinutes + " minutes.\n\nIf you did not try to sign in, contact IT support.";
                string smtpUser = System.Configuration.ConfigurationManager.AppSettings["SmtpUser"] ?? "";
                string smtpPass = System.Configuration.ConfigurationManager.AppSettings["SmtpPass"] ?? "";

                if (string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPass))
                {
                    dbcl.WriteToFile("MFA OTP email skipped: SMTP credentials not configured (SmtpUser/SmtpPass).");
                    return false;
                }

                SmtpClient smtp = new SmtpClient("smtp.zoho.in", 587) { EnableSsl = true, Credentials = new NetworkCredential(smtpUser, smtpPass) };
                smtp.Send(mm); return true;
            }
            catch (Exception ex) { dbcl.WriteToFile(ex.ToString()); return false; }
        }

        /* ================= FORGOT PASSWORD ================= */

        protected void btn_fetch_email_Click(object sender, EventArgs e)
        {
            ViewState["ForgotMode"] = true;
            DataTable dt = dbcl.SPreturn_dt("SELECT Email FROM tbl_Employee_Mustertable WHERE LoginID=@ID AND WorkStatus='Active'", new SqlParameter[] { new SqlParameter("@ID", txt_reset_code.Text.Trim()) });

            if (dt.Rows.Count > 0)
            {
                string existingEmail = dt.Rows[0]["Email"].ToString();
                txt_reset_email.Text = existingEmail;
                ViewState["OriginalEmail"] = existingEmail;

                ph_email_section.Visible = true;
                btn_fetch_email.Visible = false;
                txt_reset_code.Enabled = false;

                Notify("Found", "Details found. Edit your email if needed before sending OTP.", "info");
            }
            else { Notify("Not Found", "Invalid or inactive Login ID.", "error"); }
        }

        protected void btn_send_otp_Click(object sender, EventArgs e)
        {
            ViewState["ForgotMode"] = true;
            DataTable dt = dbcl.SPreturn_dt("SELECT FullName FROM tbl_Employee_Mustertable WHERE LoginID=@ID", new SqlParameter[] { new SqlParameter("@ID", txt_reset_code.Text.Trim()) });
            if (dt.Rows.Count == 0) return;

            string otp = GenerateOTP();
            string emailToSendTo = txt_reset_email.Text.Trim();

            Session["OTP"] = otp;
            Session["OTP_USER"] = txt_reset_code.Text.Trim();
            Session["OTP_EMAIL"] = emailToSendTo;
            Session["OTP_EXP"] = DateTime.Now.AddMinutes(5);
            Session["OTP_TRY"] = 0;

            if (!SendOTPEmail(emailToSendTo, otp, dt.Rows[0]["FullName"].ToString()))
            {
                Notify("Error", "Could not send email.", "error");
                return;
            }

            Notify("Sent", "OTP sent to " + emailToSendTo, "success");
            ph_otp.Visible = true;
            btn_send_otp.Visible = false;
            txt_reset_email.Enabled = false;
        }

        protected void btn_verify_reset_Click(object sender, EventArgs e)
        {
            ViewState["ForgotMode"] = true;

            if (Session["OTP"] == null || Session["OTP_EXP"] == null || DateTime.Now > (DateTime)Session["OTP_EXP"])
            {
                Notify("Expired", "OTP expired. Try again.", "error"); return;
            }

            Session["OTP_TRY"] = (int)(Session["OTP_TRY"] ?? 0) + 1;
            if ((int)Session["OTP_TRY"] > 3)
            {
                Notify("Blocked", "Too many attempts.", "error"); return;
            }

            if (txt_otp.Text.Trim() != Session["OTP"].ToString())
            {
                Notify("Invalid", "Wrong OTP.", "error"); return;
            }

            string hashedPass = HashPassword(txt_new_pass.Text.Trim());
            string newEmail = Session["OTP_EMAIL"].ToString();
            string origEmail = ViewState["OriginalEmail"]?.ToString();
            string userId = Session["OTP_USER"].ToString();

            if (newEmail != origEmail)
            {
                dbcl.SPreturn_dt("UPDATE tbl_Employee_Mustertable SET LoginPassword=@P, Email=@E WHERE LoginID=@ID",
                    new SqlParameter[] { new SqlParameter("@P", hashedPass), new SqlParameter("@E", newEmail), new SqlParameter("@ID", userId) });
            }
            else
            {
                dbcl.SPreturn_dt("UPDATE tbl_Employee_Mustertable SET LoginPassword=@P WHERE LoginID=@ID",
                    new SqlParameter[] { new SqlParameter("@P", hashedPass), new SqlParameter("@ID", userId) });
            }

            Session.Remove("OTP"); Session.Remove("OTP_USER"); Session.Remove("OTP_EMAIL"); Session.Remove("OTP_EXP"); Session.Remove("OTP_TRY");
            ViewState["ForgotMode"] = false;

            Notify("Success", "Reset successful. Redirecting...", "success");
            ScriptManager.RegisterStartupScript(this, GetType(), "redirect", "setTimeout(function(){ window.location.href='login.aspx'; }, 2000);", true);
        }

        /* ================= HELPERS ================= */

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private string GenerateOTP()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[4]; rng.GetBytes(bytes);
                return (BitConverter.ToUInt32(bytes, 0) % 900000 + 100000).ToString();
            }
        }

        private bool SendOTPEmail(string to, string otp, string name)
        {
            try
            {
                MailMessage mm = new MailMessage("it.support@aminruptechnologies.co.in", to);
                mm.Subject = "Password Reset OTP";
                mm.Body = $"Hi {name},\n\nYour OTP is: {otp}\nThis OTP is valid for 5 minutes.";
                string smtpUser = System.Configuration.ConfigurationManager.AppSettings["SmtpUser"] ?? "";
                string smtpPass = System.Configuration.ConfigurationManager.AppSettings["SmtpPass"] ?? "";

                if (string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPass))
                {
                    dbcl.WriteToFile("OTP email skipped: SMTP credentials not configured (SmtpUser/SmtpPass).");
                    return false;
                }

                SmtpClient smtp = new SmtpClient("smtp.zoho.in", 587) { EnableSsl = true, Credentials = new NetworkCredential(smtpUser, smtpPass) };
                smtp.Send(mm); return true;
            }
            catch (Exception ex) { dbcl.WriteToFile(ex.ToString()); return false; }
        }

        private void Notify(string title, string msg, string type)
        {
            string script = $"window.setTimeout(function () {{ if (window.notify) {{ notify('{title}', '{msg}', '{type}'); }} else {{ alert('{title}: {msg}'); }} }}, 50);";
            ScriptManager.RegisterStartupScript(this, GetType(), Guid.NewGuid().ToString(), script, true);
        }

        private void InsertLoginAudit(string loginId, string workmanSL, string result, string reason)
        {
            dbcl.SPreturn_dt(@"INSERT INTO tbl_UserLoginAudit (LoginID, WorkmanSL, LoginTime, LoginResult, FailureReason, IPAddress, UserAgent, SessionID) VALUES (@LoginID, @WorkmanSL, GETDATE(), @Result, @Reason, @IP, @Agent, @SessionID)",
                new SqlParameter[] { new SqlParameter("@LoginID", loginId), new SqlParameter("@WorkmanSL", (object)workmanSL ?? DBNull.Value), new SqlParameter("@Result", result), new SqlParameter("@Reason", (object)reason ?? DBNull.Value), new SqlParameter("@IP", Request.UserHostAddress ?? ""), new SqlParameter("@Agent", Request.UserAgent ?? ""), new SqlParameter("@SessionID", Session.SessionID) });
        }
    }
}