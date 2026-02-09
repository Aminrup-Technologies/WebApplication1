using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Hosting;
using System.Net.Mail;
using System.Net;
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
                if (Request.Cookies["ATS_SavedID"] != null)
                {
                    txt_loginid.Text = Request.Cookies["ATS_SavedID"].Value;
                    chk_remember.Checked = true;
                }
            }
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            try { PerformLogin(txt_loginid.Text.Trim(), txt_password.Text.Trim()); }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex) { Notify("Error", ex.Message, "error"); }
        }

        private void PerformLogin(string id, string pass)
        {
            string query = "select TOP 1 * from tbl_Employee_Mustertable where LoginID=@LoginID and LoginPassword=@LoginPassword";
            SqlParameter[] pram = { new SqlParameter("@LoginID", id), new SqlParameter("@LoginPassword", pass) };
            DataTable dt = dbcl.SPreturn_dt(query, pram);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                if (row["WorkStatus"].ToString() == "Active")
                {
                    if (chk_remember.Checked)
                        Response.Cookies.Add(new HttpCookie("ATS_SavedID", id) { Expires = DateTime.Now.AddDays(15) });

                    // RESTORING ALL ORIGINAL SESSIONS
                    Session["USERID"] = row["LoginID"].ToString();
                    Session["Password"] = pass;
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
                    Session["User_Photo"] = (!string.IsNullOrEmpty(photo) && File.Exists(Path.Combine(rootFolder, photo))) ? photo : "No_Image.jpg";

                    dbcl.WriteToFile($"User {Session["USERNAME"]} Logged In");
                    Response.Redirect("~/bussiness/production/homepage.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else { Notify("Denied", "Account is Inactive.", "error"); }
            }
            else { Notify("Failed", "Invalid ID or Password.", "error"); }
        }

        protected void btn_send_otp_Click(object sender, EventArgs e)
        {
            DataTable dt = dbcl.SPreturn_dt("SELECT FullName FROM tbl_Employee_Mustertable WHERE LoginID=@ID AND Email=@Email",
                           new SqlParameter[] { new SqlParameter("@ID", txt_reset_code.Text.Trim()), new SqlParameter("@Email", txt_reset_email.Text.Trim()) });

            if (dt.Rows.Count > 0)
            {
                string otp = new Random().Next(100000, 999999).ToString();
                Session["OTP"] = otp;
                Session["OTP_USER"] = txt_reset_code.Text.Trim();

                if (SendOTPEmail(txt_reset_email.Text, otp, dt.Rows[0]["FullName"].ToString()))
                {
                    Notify("Sent", "Check your email for OTP.", "success");
                    ph_otp.Visible = true;
                    btn_send_otp.Visible = false;
                    btn_verify_reset.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "toggle", "toggleUI(true);", true);
                }
                else { Notify("Error", "Could not send email.", "error"); }
            }
            else { Notify("Not Found", "Details do not match.", "notice"); }
        }

        protected void btn_verify_reset_Click(object sender, EventArgs e)
        {
            if (txt_otp.Text == Session["OTP"]?.ToString())
            {
                dbcl.SPreturn_dt("UPDATE tbl_Employee_Mustertable SET LoginPassword=@P WHERE LoginID=@ID",
                new SqlParameter[] { new SqlParameter("@P", txt_new_pass.Text), new SqlParameter("@ID", Session["OTP_USER"].ToString()) });
                Response.Redirect("login.aspx");
            }
            else { Notify("Invalid", "Wrong OTP code.", "error"); }
        }

        private bool SendOTPEmail(string to, string otp, string name)
        {
            try
            {
                MailMessage mm = new MailMessage("it.support@aminruptechnologies.co.in", to);
                mm.Subject = "Your OTP Code";
                mm.Body = $"Hi {name}, your password reset code is: {otp}";
                SmtpClient smtp = new SmtpClient("smtp.zoho.in")
                {
                    Credentials = new NetworkCredential("it.support@aminruptechnologies.co.in", "TPw800QrVMU2"),
                    EnableSsl = true,
                    Port = 587
                };
                smtp.Send(mm);
                return true;
            }
            catch { return false; }
        }

        private void Notify(string title, string msg, string type)
        {
            string script = $"notify('{title}', '{msg}', '{type}');";
            ScriptManager.RegisterStartupScript(this, GetType(), Guid.NewGuid().ToString(), script, true);
        }
    }
}