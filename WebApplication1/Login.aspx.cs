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
            }
        }

        // --- NEW FIX: Server-side Tab Control ---
        protected void Page_PreRender(object sender, EventArgs e)
        {
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

        private void PerformLogin(string id, string pass)
        {
            Stopwatch sw = Stopwatch.StartNew();

            string inputHashedPass = HashPassword(pass);
            string query = @"
                SELECT TOP 1 
                    LoginID, LoginPassword, WorkStatus, DOR, PasswordExpiry, WorkmanSL, FirstName, FullName,
                    User_RoleType, UserRoleDB, RolePermissionDB, WorkRegion, WorkState, WorkCompany,
                    WorkSite, Worksite_Code, SkillDesignation, SkillCategory, PrfPicFile
                FROM tbl_Employee_Mustertable WHERE LoginID = @LoginID";

            DataTable dt = dbcl.SPreturn_dt(query, new SqlParameter[] { new SqlParameter("@LoginID", id) });
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

            // --- ALL VALIDATIONS PASSED: PROCEED WITH LOGIN ---

            // Single batched post-auth write: audit entry + LastLogin/LoginStatus update.
            // One round-trip instead of two. (Legacy password migration runs separately above.)
            string postAuthSql = @"
                INSERT INTO tbl_UserLoginAudit (LoginID, WorkmanSL, LoginTime, LoginResult, FailureReason, IPAddress, UserAgent, SessionID)
                VALUES (@LoginID, @WorkmanSL, GETDATE(), 'SUCCESS', NULL, @IP, @Agent, @SessionID);
                UPDATE tbl_Employee_Mustertable SET LastLogin = GETDATE(), LoginStatus = 1 WHERE LoginID = @LoginID;";

            List<SqlParameter> postAuthParams = new List<SqlParameter>
            {
                new SqlParameter("@LoginID", id),
                new SqlParameter("@WorkmanSL", (object)workmanSL ?? DBNull.Value),
                new SqlParameter("@IP", Request.UserHostAddress ?? ""),
                new SqlParameter("@Agent", Request.UserAgent ?? ""),
                new SqlParameter("@SessionID", Session.SessionID)
            };

            dbcl.SPreturn_dt(postAuthSql, postAuthParams.ToArray());
            LogLoginTiming("postAuthWrite", sw, id);

            // Handle 'Remember Me' Cookie
            if (chk_remember.Checked) { Response.Cookies.Add(new HttpCookie("ATS_SavedID", id) { Expires = DateTime.Now.AddDays(15) }); }
            else if (Request.Cookies["ATS_SavedID"] != null) { Response.Cookies["ATS_SavedID"].Expires = DateTime.Now.AddDays(-1); }

            string loginID = row["LoginID"].ToString();

            // Assign Session Variables using the strongly-typed SessionKeys class
            Session[SessionKeys.UserID] = loginID;
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
            LogLoginTiming("sessionSet", sw, id);

            // Handle Profile Picture securely
            string photo = row["PrfPicFile"].ToString();
            Session[SessionKeys.UserPhoto] = (!string.IsNullOrEmpty(photo) && Directory.Exists(rootFolder) && File.Exists(Path.Combine(rootFolder, photo)))
                                            ? photo
                                            : "No_Image.jpg";

            LogLoginTiming("total", sw, id);

            // Redirect to new optimized dashboard
            Response.Redirect("~/bussiness/production/homepage_v2.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
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