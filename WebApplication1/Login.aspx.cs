using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;

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
                // Remember Me
                if (Request.Cookies["ATS_SavedID"] != null)
                {
                    txt_loginid.Text = Request.Cookies["ATS_SavedID"].Value;
                    chk_remember.Checked = true;
                }

                ViewState["ForgotMode"] = false;
            }

            // Restore forgot-password UI after postback
            if (ViewState["ForgotMode"] != null && (bool)ViewState["ForgotMode"])
            {
                ph_otp.Visible = true;
                btn_send_otp.Visible = false;
                btn_verify_reset.Visible = true;
            }
        }

        /* ================= LOGIN ================= */

        protected void btn_login_Click(object sender, EventArgs e)
        {
            try
            {
                PerformLogin(txt_loginid.Text.Trim(), txt_password.Text.Trim());
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                Notify("Error", "Unexpected error occurred.", "error");
                dbcl.WriteToFile(ex.ToString());
            }
        }

        private void PerformLogin_OLD(string id, string pass)
        {
            string hashedPass = HashPassword(pass);

            string query = @"
                SELECT TOP 1 
                    LoginID, WorkStatus, WorkmanSL,
                    FirstName, FullName,
                    User_RoleType, UserRoleDB, RolePermissionDB,
                    WorkRegion, WorkState, WorkCompany,
                    WorkSite, Worksite_Code,
                    SkillDesignation, SkillCategory,
                    PrfPicFile
                FROM tbl_Employee_Mustertable
                WHERE LoginID=@LoginID AND LoginPassword=@LoginPassword";

            SqlParameter[] pram =
            {
                new SqlParameter("@LoginID", id),
                new SqlParameter("@LoginPassword", hashedPass)
            };

            DataTable dt = dbcl.SPreturn_dt(query, pram);

            if (dt == null || dt.Rows.Count == 0)
            {
                Notify("Failed", "Invalid ID or Password.", "error");
                return;
            }

            DataRow row = dt.Rows[0];

            if (row["WorkStatus"].ToString() != "Active")
            {
                Notify("Denied", "Account is Inactive.", "error");
                return;
            }

            // Remember Me cookie
            if (chk_remember.Checked)
            {
                Response.Cookies.Add(new HttpCookie("ATS_SavedID", id)
                {
                    Expires = DateTime.Now.AddDays(15)
                });
            }

            // Sessions (NO password stored)
            Session["USERID"] = row["LoginID"].ToString();
            Session["WORKMAN"] = row["WorkmanSL"].ToString();
            Session["USERFNAME"] = row["FirstName"].ToString();
            Session["USERNAME"] = row["FullName"].ToString();
            Session["USERTYPE"] = row["User_RoleType"].ToString();
            Session["UserRoleDB"] = row["UserRoleDB"].ToString();
            Session["RolePermissionDB"] = row["RolePermissionDB"].ToString();
            Session["REGION"] = row["WorkRegion"].ToString();
            Session["STATE"] = row["WorkState"].ToString();
            Session["COMPANY_CODE"] = row["WorkCompany"].ToString();
            Session["U_SITE"] = row["WorkSite"].ToString();
            Session["U_SITECODE"] = row["Worksite_Code"].ToString();
            Session["U_DESG"] = row["SkillDesignation"].ToString();
            Session["U_SKILL"] = row["SkillCategory"].ToString();

            string photo = row["PrfPicFile"].ToString();
            Session["User_Photo"] =
                (!string.IsNullOrEmpty(photo) && File.Exists(Path.Combine(rootFolder, photo)))
                ? photo
                : "No_Image.jpg";

            dbcl.WriteToFile($"User {Session["USERNAME"]} Logged In");

            Response.Redirect("~/bussiness/production/homepage.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void PerformLogin_13Feb26(string id, string pass)
        {
            string hashedPass = HashPassword(pass);
            //string hashedPass = pass;

            string query = @"
            SELECT TOP 1 
                LoginID, LoginPassword,
                WorkStatus, DOR, PasswordExpiry,
                WorkmanSL, FirstName, FullName,
                User_RoleType, UserRoleDB, RolePermissionDB,
                WorkRegion, WorkState, WorkCompany,
                WorkSite, Worksite_Code,
                SkillDesignation, SkillCategory,
                PrfPicFile
            FROM tbl_Employee_Mustertable
            WHERE LoginID = @LoginID";

            DataTable dt = dbcl.SPreturn_dt(
            query,
            new SqlParameter[]
            {
                new SqlParameter("@LoginID", id)
            });


            if (dt.Rows.Count == 0)
            {
                InsertLoginAudit(id, null, "FAILED", "InvalidUser");
                Notify("Failed", "Invalid ID or Password.", "error");
                return;
            }

            DataRow row = dt.Rows[0];

            // Password check
            if (row["LoginPassword"].ToString() != hashedPass)
            {
                InsertLoginAudit(id, row["WorkmanSL"].ToString(), "FAILED", "InvalidPassword");
                Notify("Failed", "Invalid ID or Password.", "error");
                return;
            }

            // Work status
            if (row["WorkStatus"].ToString() != "Active")
            {
                InsertLoginAudit(id, row["WorkmanSL"].ToString(), "BLOCKED", "Inactive");
                Notify("Denied", "Account is inactive.", "error");
                return;
            }

            // Exit / Relieving check (DOR)
            if (row["DOR"] != DBNull.Value && Convert.ToDateTime(row["DOR"]) <= DateTime.Now)
            {
                InsertLoginAudit(id, row["WorkmanSL"].ToString(), "BLOCKED", "DORExpired");
                Notify("Denied", "User access has been closed.", "error");
                return;
            }

            // Password expiry
            if (row["PasswordExpiry"] != DBNull.Value &&
                Convert.ToDateTime(row["PasswordExpiry"]) < DateTime.Now)
            {
                InsertLoginAudit(id, row["WorkmanSL"].ToString(), "BLOCKED", "PasswordExpired");
                Notify("Expired", "Password expired. Reset required.", "error");
                return;
            }

            // SUCCESS LOGIN AUDIT
            InsertLoginAudit(id, row["WorkmanSL"].ToString(), "SUCCESS", null);

            // Update master summary
            dbcl.SPreturn_dt(
                "UPDATE tbl_Employee_Mustertable SET LastLogin=GETDATE(), LoginStatus=1 WHERE LoginID=@ID",
                new SqlParameter[]
                {
                    new SqlParameter("@ID", id)
                });


            // Sessions (no password)
            Session["USERID"] = row["LoginID"].ToString();
            Session["WORKMAN"] = row["WorkmanSL"].ToString();
            Session["USERFNAME"] = row["FirstName"].ToString();
            Session["USERNAME"] = row["FullName"].ToString();
            Session["USERTYPE"] = row["User_RoleType"].ToString();
            Session["UserRoleDB"] = row["UserRoleDB"].ToString();
            Session["RolePermissionDB"] = row["RolePermissionDB"].ToString();
            Session["REGION"] = row["WorkRegion"].ToString();
            Session["STATE"] = row["WorkState"].ToString();
            Session["COMPANY_CODE"] = row["WorkCompany"].ToString();
            Session["U_SITE"] = row["WorkSite"].ToString();
            Session["U_SITECODE"] = row["Worksite_Code"].ToString();
            Session["U_DESG"] = row["SkillDesignation"].ToString();
            Session["U_SKILL"] = row["SkillCategory"].ToString();

            string photo = row["PrfPicFile"].ToString();
            Session["User_Photo"] =
                (!string.IsNullOrEmpty(photo) && File.Exists(Path.Combine(rootFolder, photo)))
                ? photo
                : "No_Image.jpg";

            Response.Redirect("~/bussiness/production/homepage.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void PerformLogin(string id, string pass)
        {
            // 1. Calculate the hash of the input password immediately
            string inputHashedPass = HashPassword(pass);

            string query = @"
    SELECT TOP 1 
        LoginID, LoginPassword,
        WorkStatus, DOR, PasswordExpiry,
        WorkmanSL, FirstName, FullName,
        User_RoleType, UserRoleDB, RolePermissionDB,
        WorkRegion, WorkState, WorkCompany,
        WorkSite, Worksite_Code,
        SkillDesignation, SkillCategory,
        PrfPicFile
    FROM tbl_Employee_Mustertable
    WHERE LoginID = @LoginID";

            DataTable dt = dbcl.SPreturn_dt(
                query,
                new SqlParameter[]
                {
            new SqlParameter("@LoginID", id)
                });

            // 2. Check if User Exists
            if (dt.Rows.Count == 0)
            {
                InsertLoginAudit(id, null, "FAILED", "InvalidUser");
                Notify("Failed", "Invalid ID or Password.", "error");
                return;
            }

            DataRow row = dt.Rows[0];
            string storedPassword = row["LoginPassword"].ToString();
            string workmanSL = row["WorkmanSL"].ToString();

            // =========================================================================
            // HYBRID PASSWORD CHECK (Migration Logic)
            // =========================================================================
            bool isPasswordValid = false;
            bool needsMigration = false;

            // Check A: Is it already hashed? (Standard secure login)
            if (storedPassword == inputHashedPass)
            {
                isPasswordValid = true;
            }
            // Check B: Is it plain text? (Legacy login)
            else if (storedPassword == pass)
            {
                isPasswordValid = true;
                needsMigration = true; // Mark for update
            }

            if (!isPasswordValid)
            {
                InsertLoginAudit(id, workmanSL, "FAILED", "InvalidPassword");
                Notify("Failed", "Invalid ID or Password.", "error");
                return;
            }

            // 3. If it was a legacy plain-text login, update DB to hash immediately
            if (needsMigration)
            {
                string updatePassQuery = "UPDATE tbl_Employee_Mustertable SET LoginPassword=@NewHash WHERE LoginID=@ID";
                dbcl.SPreturn_dt(updatePassQuery, new SqlParameter[] {
            new SqlParameter("@NewHash", inputHashedPass),
            new SqlParameter("@ID", id)
        });
            }
            // =========================================================================

            // 4. Work status Check
            if (row["WorkStatus"].ToString() != "Active")
            {
                InsertLoginAudit(id, workmanSL, "BLOCKED", "Inactive");
                Notify("Denied", "Account is inactive.", "error");
                return;
            }

            // 5. Exit / Relieving check (DOR)
            if (row["DOR"] != DBNull.Value && Convert.ToDateTime(row["DOR"]) <= DateTime.Now)
            {
                InsertLoginAudit(id, workmanSL, "BLOCKED", "DORExpired");
                Notify("Denied", "User access has been closed.", "error");
                return;
            }

            // 6. Password expiry
            if (row["PasswordExpiry"] != DBNull.Value &&
                Convert.ToDateTime(row["PasswordExpiry"]) < DateTime.Now)
            {
                InsertLoginAudit(id, workmanSL, "BLOCKED", "PasswordExpired");
                Notify("Expired", "Password expired. Reset required.", "error");
                return;
            }

            // 7. SUCCESS LOGIN AUDIT
            InsertLoginAudit(id, workmanSL, "SUCCESS", null);

            // 8. Update master summary (LastLogin)
            dbcl.SPreturn_dt(
                "UPDATE tbl_Employee_Mustertable SET LastLogin=GETDATE(), LoginStatus=1 WHERE LoginID=@ID",
                new SqlParameter[]
                {
            new SqlParameter("@ID", id)
                });

            // 9. Set Sessions
            Session["USERID"] = row["LoginID"].ToString();
            Session["WORKMAN"] = workmanSL;
            Session["USERFNAME"] = row["FirstName"].ToString();
            Session["USERNAME"] = row["FullName"].ToString();
            Session["USERTYPE"] = row["User_RoleType"].ToString();
            Session["UserRoleDB"] = row["UserRoleDB"].ToString();
            Session["RolePermissionDB"] = row["RolePermissionDB"].ToString();
            Session["REGION"] = row["WorkRegion"].ToString();
            Session["STATE"] = row["WorkState"].ToString();
            Session["COMPANY_CODE"] = row["WorkCompany"].ToString();
            Session["U_SITE"] = row["WorkSite"].ToString();
            Session["U_SITECODE"] = row["Worksite_Code"].ToString();
            Session["U_DESG"] = row["SkillDesignation"].ToString();
            Session["U_SKILL"] = row["SkillCategory"].ToString();

            string photo = row["PrfPicFile"].ToString();
            // Assuming rootFolder is defined at class level or passed in context
            // If rootFolder isn't available in this scope, ensure you define it or use Server.MapPath
            string photoPath = string.Empty;

            // Safety check for path combination
            if (!string.IsNullOrEmpty(photo))
            {
                // Adjust this based on where 'rootFolder' comes from in your actual code
                // e.g., string rootFolder = Server.MapPath("~/UserPhotos/"); 
                if (Directory.Exists(rootFolder) && File.Exists(Path.Combine(rootFolder, photo)))
                {
                    photoPath = photo;
                }
                else
                {
                    photoPath = "No_Image.jpg";
                }
            }
            else
            {
                photoPath = "No_Image.jpg";
            }

            Session["User_Photo"] = photoPath;

            Response.Redirect("~/bussiness/production/homepage.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        /* ================= FORGOT PASSWORD ================= */

        protected void btn_send_otp_Click(object sender, EventArgs e)
        {
            DataTable dt = dbcl.SPreturn_dt(
                "SELECT FullName FROM tbl_Employee_Mustertable WHERE LoginID=@ID AND Email=@Email",
                new SqlParameter[]
                {
                    new SqlParameter("@ID", txt_reset_code.Text.Trim()),
                    new SqlParameter("@Email", txt_reset_email.Text.Trim())
                });

            if (dt.Rows.Count == 0)
            {
                Notify("Not Found", "Details do not match.", "notice");
                return;
            }

            string otp = GenerateOTP();

            Session["OTP"] = otp;
            Session["OTP_USER"] = txt_reset_code.Text.Trim();
            Session["OTP_EXP"] = DateTime.Now.AddMinutes(5);
            Session["OTP_TRY"] = 0;

            if (!SendOTPEmail(txt_reset_email.Text.Trim(), otp, dt.Rows[0]["FullName"].ToString()))
            {
                Notify("Error", "Could not send email.", "error");
                return;
            }

            Notify("Sent", "OTP sent to your registered email.", "success");

            ph_otp.Visible = true;
            btn_send_otp.Visible = false;
            btn_verify_reset.Visible = true;

            ViewState["ForgotMode"] = true;
        }

        protected void btn_verify_reset_Click(object sender, EventArgs e)
        {
            if (Session["OTP"] == null || Session["OTP_EXP"] == null)
            {
                Notify("Expired", "OTP expired. Try again.", "error");
                return;
            }

            if (DateTime.Now > (DateTime)Session["OTP_EXP"])
            {
                Notify("Expired", "OTP expired. Try again.", "error");
                return;
            }

            Session["OTP_TRY"] = (int)(Session["OTP_TRY"] ?? 0) + 1;
            if ((int)Session["OTP_TRY"] > 3)
            {
                Notify("Blocked", "Too many attempts.", "error");
                return;
            }

            if (txt_otp.Text.Trim() != Session["OTP"].ToString())
            {
                Notify("Invalid", "Wrong OTP.", "error");
                return;
            }

            string hashedPass = HashPassword(txt_new_pass.Text.Trim());

            dbcl.SPreturn_dt(
                "UPDATE tbl_Employee_Mustertable SET LoginPassword=@P WHERE LoginID=@ID",
                new SqlParameter[]
                {
                    new SqlParameter("@P", hashedPass),
                    new SqlParameter("@ID", Session["OTP_USER"].ToString())
                });

            Session.Remove("OTP");
            Session.Remove("OTP_USER");
            Session.Remove("OTP_EXP");
            Session.Remove("OTP_TRY");

            Notify("Success", "Password reset successful.", "success");
            Response.Redirect("login.aspx");
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
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);
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

                SmtpClient smtp = new SmtpClient("smtp.zoho.in", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential("it.support@aminruptechnologies.co.in", "TPw800QrVMU2")
                };

                smtp.Send(mm);
                return true;
            }
            catch (Exception ex)
            {
                dbcl.WriteToFile(ex.ToString());
                return false;
            }
        }

        private void Notify(string title, string msg, string type)
        {
            string script = $@"
                window.setTimeout(function () {{
                    if (window.notify) {{
                        notify('{title}', '{msg}', '{type}');
                    }} else {{
                        alert('{title}: {msg}');
                    }}
                }}, 50);
            ";

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                Guid.NewGuid().ToString(),
                script,
                true);
        }



        private void InsertLoginAudit(string loginId, string workmanSL, string result, string reason)
        {
            dbcl.SPreturn_dt(@"
            INSERT INTO tbl_UserLoginAudit
            (
                LoginID, WorkmanSL, LoginTime,
                LoginResult, FailureReason,
                IPAddress, UserAgent, SessionID
            )
            VALUES
            (
                @LoginID, @WorkmanSL, GETDATE(),
                @Result, @Reason,
                @IP, @Agent, @SessionID
            )",
                new SqlParameter[]
                {
                    new SqlParameter("@LoginID", loginId),
                    new SqlParameter("@WorkmanSL", (object)workmanSL ?? DBNull.Value),
                    new SqlParameter("@Result", result),
                    new SqlParameter("@Reason", (object)reason ?? DBNull.Value),
                    new SqlParameter("@IP", Request.UserHostAddress ?? ""),
                    new SqlParameter("@Agent", Request.UserAgent ?? ""),
                    new SqlParameter("@SessionID", Session.SessionID)
                });
        }

    }
}
