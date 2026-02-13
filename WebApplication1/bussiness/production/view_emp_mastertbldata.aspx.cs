using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using ClosedXML.Excel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail; // Required for Email
using System.Text;

namespace WebApplication1.bussiness.production
{
    public partial class view_emp_mastertbldata : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        public static string state = string.Empty;
        public static string region = string.Empty;
        public static string comp = string.Empty;
        public static string datalock = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    if (Session["Changer"] != null)
                    {
                        string[] retrievedArray = (string[])Session["Changer"];
                        region = retrievedArray[1].ToString();
                        comp = retrievedArray[2].ToString();
                        state = retrievedArray[0].ToString();
                        datalock = retrievedArray[3].ToString();
                        //Session["Changer"]= null;
                    }
                    else
                    {
                        region = Session["REGION"].ToString();
                        comp = Session["COMPANY_CODE"].ToString();
                        state = Session["STATE"].ToString();
                        datalock = "0";
                    }

                    // OPTIMIZED QUERY: Only select columns you display
                    string optimizedQuery = @"
                        SELECT Id, WorkmanSL, WorkStatus, FullName, SkillCategory, SkillDesignation, LoginID, 
                               Fathername, DOR, DOJ, MobileNo, Email, WorkSite, SafetyPassNo, BloodGroup, SafetyPassExpiry, UANNo  -- <--- ADDED THESE COLUMNS
                        FROM tbl_Employee_Mustertable WHERE WorkRegion = '" + region + "' AND WorkCompany='" + comp + "' ORDER BY Id DESC";
                    BindGrid(optimizedQuery);
                }
            }
        }

        protected void ExportExcel(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select ROW_NUMBER() OVER (ORDER BY Id) AS SlNo, WorkStatus, WorkRegion, WorkCompany, WorkmanSL, FirstName, MiddleName, LastName, FullName, Fathername, BloodGroup, MobileNo, DOB, Qualification, DOJ, DOR, WorkSite, SkillCategory, SkillDesignation, User_RoleType, Role_Permission, SafetyPassNo, SafetyPassExpiry, GatePassNo, GatePassExpiry, PVExpiry, UANNo, ESICNo, Payment_Bank, Payment_Account, Payment_IFSC, BankBranch, QualificationDB from tbl_Employee_Mustertable where WorkRegion = '" + region + "' and WorkCompany='" + comp + "' order by Id"))

                //using (SqlCommand cmd = new SqlCommand("select ROW_NUMBER() OVER (ORDER BY Id) AS SlNo,WorkStatus, LoginID, WorkRegion, WorkCompany, WorkmanSL, FirstName, MiddleName, LastName, FullName, Fathername, BloodGroup, MobileNo, DOB, Qualification, DOJ, DOR, WorkSite, SkillCategory, SkillDesignation, User_RoleType, Role_Permission, WorkHours, OTFactor, SafetyPassNo, SafetyPassExpiry, GatePassNo, GatePassExpiry, PVExpiry, UANNo, ESICNo, Payment_Bank, Payment_Account, Payment_IFSC, BankBranch, QualificationDB, FixedSalary_YesNo, FixedAmount, DA_VDA, HRA, Conv_Allowance, Medical_Allowance, Washing_Allowance, ATT_Allowance, SPCL_Allowance, Misc_Earnings, OTMultiplier, OT_Divisibility from tbl_Employee_Mustertable where WorkRegion = '" + region + "' and WorkCompany='" + comp + "' order by Id"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dt, "Customers");

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=EmpExport.xlsx");
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void BindGrid(string cmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            using (SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn))
            {
                using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                {
                    DataTable ds = new DataTable();
                    ad.Fill(ds);

                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                }
            }
            dbcl.Conn.Close();
        }

        // REQUIRED FOR GENTELELLA DATATABLES TO WORK
        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count > 0)
            {
                // This is REQUIRED for DataTables to work
                GridView1.UseAccessibleHeader = true;
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Swap_WorkStatus")
            {
                // SPLIT THE ARGUMENT WE PASSED IN ASPX
                // Format: "ID,WorkmanSL,CurrentStatus"
                string[] args = e.CommandArgument.ToString().Split(',');
                string dbId = args[0];
                string empWrk = args[1];
                string currentStatus = args[2];

                string newStatus = (currentStatus == "Active") ? "InActive" : "Active";

                // Update Database
                string updateQry = "UPDATE tbl_Employee_Mustertable SET WorkStatus = '" + newStatus + "' WHERE Id = '" + dbId + "'";
                dbcl.executeRdr(updateQry);

                // Show Notification
                string title = "Success";
                string body = "Employee " + empWrk + " is now " + newStatus;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                // Rebind
                string optimizedQuery = @"SELECT Id, WorkmanSL, WorkStatus, FullName, SkillCategory, SkillDesignation, Fathername, DOR, DOJ, MobileNo, Email, WorkSite, SafetyPassNo FROM tbl_Employee_Mustertable WHERE WorkRegion = '" + region + "' AND WorkCompany='" + comp + "' ORDER BY Id DESC";
                BindGrid(optimizedQuery);
            }
            else if (e.CommandName == "View_Details")
            {
                string empWrk = e.CommandArgument.ToString();
                Response.Redirect("viewupdate_empmustertabledata.aspx?ID=" + empWrk);
            }

            else if (e.CommandName == "Reset_Password")
            {
                string[] args = e.CommandArgument.ToString().Split('|');
                string workmanSL = args[0];
                string email = args.Length > 1 ? args[1] : "";
                string empName = args.Length > 2 ? args[2] : "Employee";
                string loginID = args.Length > 3 ? args[3] : "N/A";

                empName = empName.Replace("'", "&#39;"); // Sanitize Name

                string newPassword = "ATS@" + new Random().Next(1000, 9999).ToString();
                string adminUser = Session["USERID"].ToString();

                try
                {
                    // Update DB...
                    bool isUpdated = UpdatePasswordInDB(workmanSL, newPassword, adminUser);

                    if (isUpdated)
                    {
                        if (!string.IsNullOrEmpty(email) && email != "N/A" && email.Length > 5)
                        {
                            // SCENARIO A: EMAIL EXISTS (Email Logic Here...)
                            try
                            {
                                SendPasswordEmail(email, empName, workmanSL, newPassword);
                                ShowPopup("Success", "Password reset successfully. Email sent to: <b>" + email + "</b>");
                            }
                            catch (Exception)
                            {
                                ShowPopup("Partial Success", "Password reset, but Email failed.");
                            }
                        }
                        else
                        {
                            // SCENARIO B: NO EMAIL (Business Continuity)
                            string title = "Manual Action Required";

                            // 1. Prepare Copy-Paste Text (Use \\n for valid JS line breaks)
                            string shareText = "DO NOT REPLY - ATS ERP Credentials\\n" +
                                               "Name: " + empName + "\\n" +
                                               "Login ID: " + loginID + "\\n" +
                                               "Password: " + newPassword + "\\n" +
                                               "Please change on first login.";

                            // 2. Build HTML Body
                            string body = "<div class='text-left'>" +
                                          "Email ID is missing for <b>" + empName + "</b>.<br/>" +
                                          "Please share these details manually:<br/><br/>" +

                                          "<div style=\"background-color:#f7f7f7; border-left: 5px solid #d9534f; padding: 15px;\">" +
                                              "<h4 style=\"margin-top:0; color:#d9534f;\">" + newPassword + "</h4>" +
                                              "<small style=\"color:#555;\">Password</small>" +
                                          "</div>" +

                                          "<div style=\"margin-top:10px; border:1px solid #eee; padding:10px;\">" +
                                              "<strong>Login ID:</strong> " + loginID + "<br/>" +
                                              "<strong>Emp Code:</strong> " + workmanSL +
                                          "</div>" +

                                          // Hidden Textarea for Copy Function
                                          "<textarea id=\"txtShare\" style=\"display:none;\">" + shareText + "</textarea>" +

                                          "<div class='text-center' style='margin-top:15px;'>" +
                                              "<button type=\"button\" class=\"btn btn-primary\" onclick=\"CopyShareText()\">" +
                                                  "<i class=\"fa fa-copy\"></i> Copy to Clipboard" +
                                              "</button>" +
                                          "</div>" +
                                          "</div>";

                            ShowPopup(title, body);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowPopup("System Error", "Error: " + ex.Message);
                }
            }
        }

        // ---------------------------------------------------------
        // HELPER METHOD 1: Database Update
        // ---------------------------------------------------------
        private bool UpdatePasswordInDB(string workmanSL, string newPass, string updatedBy)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                using (SqlCommand cmd = new SqlCommand("USP_Update_Employee_Password_Reset", dbcl.Conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@WorkmanSL", workmanSL);
                    cmd.Parameters.AddWithValue("@NewPassword", newPass); // Ensure your SP encrypts this if needed
                    cmd.Parameters.AddWithValue("@UpdatedBy", updatedBy);

                    cmd.ExecuteNonQuery();
                }
                dbcl.Conn.Close();
                return true;
            }
            catch (Exception)
            {
                dbcl.Conn.Close();
                throw; // Re-throw to catch in main loop
            }
        }

        // ---------------------------------------------------------
        // HELPER METHOD 2: Email Trigger
        // ---------------------------------------------------------
        private void SendPasswordEmail(string toEmail, string name, string empCode, string password)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.zoho.in"); // Replace with your SMTP Host

                mail.From = new MailAddress("it.support@aminruptechnologies.co.in", "ATS ERP System");
                mail.To.Add(toEmail);
                mail.Subject = "Security Alert: Password Reset Successful";
                mail.IsBodyHtml = true;

                // Professional HTML Email Template
                StringBuilder body = new StringBuilder();
                body.Append("<div style='font-family:Arial,sans-serif; padding:20px; border:1px solid #ddd;'>");
                body.Append("<h2 style='color:#2A3F54;'>Password Reset Notification</h2>");
                body.Append("<p>Dear " + name + ",</p>");
                body.Append("<p>Your password for the ATS ERP System has been reset by the Administrator.</p>");
                body.Append("<div style='background:#f7f7f7; padding:15px; border-left:4px solid #1ABB9C;'>");
                body.Append("<strong>User ID:</strong> " + empCode + "<br/>");
                body.Append("<strong>New Password:</strong> <span style='font-size:1.2em; color:#d9534f;'>" + password + "</span>");
                body.Append("</div>");
                body.Append("<p>Please login and change this password immediately.</p>");
                body.Append("<br/><small>This is an automated message. Do not reply.</small>");
                body.Append("</div>");

                mail.Body = body.ToString();

                // SMTP Config (Move these to Web.Config in production)
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("it.support@aminruptechnologies.co.in", "TPw800QrVMU2");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                // Log email failure but don't crash the app
                // You might want to show a warning that DB updated but Email failed
                throw new Exception("DB Updated, but Email Failed: " + ex.Message);
            }
        }

        // ---------------------------------------------------------
        // HELPER: CENTRALIZED POPUP CALLER
        // ---------------------------------------------------------
        private void ShowPopup(string title, string body)
        {
            // 1. Escape single quotes to prevent breaking JS strings
            string safeBody = body.Replace("'", "\\'");
            string safeTitle = title.Replace("'", "\\'");

            // 2. CRITICAL FIX: Remove physical NewLines (\r\n) from the C# string.
            // If these exist, they break the JavaScript function call.
            safeBody = safeBody.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");

            // 3. Register the cleaned script
            string script = "ShowPopup('" + safeTitle + "', '" + safeBody + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
        }
    }
}