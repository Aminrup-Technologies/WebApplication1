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
using System.Net.Mail;
using System.Text;

namespace WebApplication1.bussiness.production
{
    public partial class view_emp_mastertbldata_v2 : System.Web.UI.Page
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
                    }
                    else
                    {
                        region = Session["REGION"].ToString();
                        comp = Session["COMPANY_CODE"].ToString();
                        state = Session["STATE"].ToString();
                        datalock = "0";
                    }

                    LoadGridData();
                }
            }
        }

        private void LoadGridData()
        {
            string optimizedQuery = @"
                SELECT Id, WorkmanSL, WorkStatus, FullName, SkillCategory, SkillDesignation, LoginID, 
                       Fathername, DOR, DOJ, MobileNo, Email, WorkSite, SafetyPassNo, BloodGroup, SafetyPassExpiry, UANNo 
                FROM tbl_Employee_Mustertable WHERE WorkRegion = '" + region + "' AND WorkCompany='" + comp + "' ORDER BY Id DESC";
            BindGrid(optimizedQuery);
        }

        protected void ExportExcel(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select ROW_NUMBER() OVER (ORDER BY Id) AS SlNo, WorkStatus, WorkRegion, WorkCompany, WorkmanSL, FirstName, MiddleName, LastName, FullName, Fathername, BloodGroup, MobileNo, DOB, Qualification, DOJ, DOR, WorkSite, SkillCategory, SkillDesignation, User_RoleType, Role_Permission, SafetyPassNo, SafetyPassExpiry, GatePassNo, GatePassExpiry, PVExpiry, UANNo, ESICNo, Payment_Bank, Payment_Account, Payment_IFSC, BankBranch, QualificationDB from tbl_Employee_Mustertable where WorkRegion = '" + region + "' and WorkCompany='" + comp + "' order by Id"))
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
                                wb.Worksheets.Add(dt, "Employee_Data");
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

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count > 0)
            {
                GridView1.UseAccessibleHeader = true;
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Swap_WorkStatus")
            {
                string[] args = e.CommandArgument.ToString().Split(',');

                hfStatusEmpDbId.Value = args[0];
                hfStatusEmpCode.Value = args[1];
                hfCurrentStatus.Value = args[2];

                string newStatus = (args[2] == "Active") ? "InActive" : "Active";
                bool isDeactivating = (newStatus == "InActive");

                ddlChangeType.Value = "";
                ddlSpecificReason.Value = "";
                txtDOR.Text = "";
                txtDOE.Text = "";
                txtStatusRemarks.Text = "";

                string script = $"openStatusModal('{args[1]}', '{newStatus}', {isDeactivating.ToString().ToLower()});";
                ClientScript.RegisterStartupScript(this.GetType(), "OpenStatusModal", script, true);
            }
            else if (e.CommandName == "View_Details")
            {
                string empWrk = e.CommandArgument.ToString();
                Response.Redirect("viewupdate_empmustertabledata_v2.aspx?ID=" + empWrk);
            }
            else if (e.CommandName == "Reset_Password")
            {
                string[] args = e.CommandArgument.ToString().Split('|');
                hfResetEmpWrk.Value = args[0];
                string email = args.Length > 1 ? args[1] : "";
                hfResetEmpName.Value = args.Length > 2 ? args[2].Replace("'", "&#39;") : "Employee";
                hfResetLoginID.Value = args.Length > 3 ? args[3] : "N/A";

                txtCurrentEmail.Text = string.IsNullOrEmpty(email) ? "N/A" : email;
                txtNewEmail.Text = "";
                txtOTP.Text = "";

                string script = $"openEmailModal('{hfResetEmpName.Value}', false);";
                ClientScript.RegisterStartupScript(this.GetType(), "OpenEmailModal", script, true);
            }
        }

        protected void btnConfirmStatusChange_Click(object sender, EventArgs e)
        {
            string dbId = hfStatusEmpDbId.Value;
            string empWrk = hfStatusEmpCode.Value;
            string currentStatus = hfCurrentStatus.Value;
            string newStatus = (currentStatus == "Active") ? "InActive" : "Active";

            string changeType = ddlChangeType.Value;
            string reason = ddlSpecificReason.Value;
            string remarks = txtStatusRemarks.Text;

            if (newStatus == "Active")
            {
                changeType = "System Reactivation";
                reason = "Returned/Reactivated";
            }

            try
            {
                string sessionUser = Session["WORKMAN"].ToString();

                string updateQry = @"UPDATE tbl_Employee_Mustertable 
                                     SET WorkStatus = @WorkStatus,
                                         StatusChangeType = @Type, 
                                         StatusChangeReason = @Reason, 
                                         StatusRemarks = @Remarks,
                                         StatusChangedByWrk = @ChangedBy,
                                         StatusChangedDate = GETDATE()";

                if (changeType == "Permanent")
                {
                    updateQry += ", DOR = @DOR, DOE = @DOE ";
                }
                else if (newStatus == "Active")
                {
                    updateQry += ", DOR = NULL, DOE = NULL, DO_Relief = NULL ";
                }

                updateQry += " WHERE Id = @Id";

                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand cmd = new SqlCommand(updateQry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@WorkStatus", newStatus);
                    cmd.Parameters.AddWithValue("@Id", dbId);
                    cmd.Parameters.AddWithValue("@Type", changeType);
                    cmd.Parameters.AddWithValue("@Reason", reason);
                    cmd.Parameters.AddWithValue("@Remarks", remarks);
                    cmd.Parameters.AddWithValue("@ChangedBy", sessionUser);

                    if (changeType == "Permanent")
                    {
                        cmd.Parameters.AddWithValue("@DOR", string.IsNullOrEmpty(txtDOR.Text) ? (object)DBNull.Value : txtDOR.Text);
                        cmd.Parameters.AddWithValue("@DOE", string.IsNullOrEmpty(txtDOE.Text) ? (object)DBNull.Value : txtDOE.Text);
                    }

                    cmd.ExecuteNonQuery();
                }

                string closeScript = "$('#StatusChangeModal').modal('hide');";
                ClientScript.RegisterStartupScript(this.GetType(), "CloseModal", closeScript, true);
                ShowPopup("Success", $"Employee <b>{empWrk}</b> status successfully updated to {newStatus}.");
                LoadGridData();
            }
            catch (Exception ex)
            {
                ShowPopup("Database Error", "Could not save status: " + ex.Message);
            }
            finally
            {
                if (dbcl.Conn != null && dbcl.Conn.State == ConnectionState.Open) { dbcl.Conn.Close(); }
            }
        }

        protected void btnSendOTP_Click(object sender, EventArgs e)
        {
            string newEmail = txtNewEmail.Text.Trim();
            string otp = new Random().Next(100000, 999999).ToString();

            Session["RESET_OTP"] = otp;
            Session["RESET_EMAIL"] = newEmail;

            try
            {
                SendOTPEmail(newEmail, hfResetEmpName.Value, otp);
                ShowPopup("OTP Sent", $"A verification code has been sent to <b>{newEmail}</b>. Please enter it below.");
            }
            catch (Exception ex)
            {
                ShowPopup("Email Error", "Could not send OTP. " + ex.Message);
            }

            string script = $"openEmailModal('{hfResetEmpName.Value}', true);";
            ClientScript.RegisterStartupScript(this.GetType(), "ReopenModal", script, true);
        }

        protected void btnConfirmAndReset_Click_OLD(object sender, EventArgs e)
        {
            string targetEmail = txtCurrentEmail.Text;
            string workmanSL = hfResetEmpWrk.Value;
            string empName = hfResetEmpName.Value;
            string loginID = hfResetLoginID.Value;

            if (hfIsEmailEditMode.Value == "true")
            {
                if (Session["RESET_OTP"] == null || txtOTP.Text.Trim() != Session["RESET_OTP"].ToString())
                {
                    ShowPopup("Verification Failed", "The OTP entered is incorrect or expired. Please try again.");
                    string reopenScript = $"openEmailModal('{empName}');";
                    ClientScript.RegisterStartupScript(this.GetType(), "ReopenModal", reopenScript, true);
                    return;
                }

                targetEmail = Session["RESET_EMAIL"].ToString();

                try
                {
                    string qry = "UPDATE tbl_Employee_Mustertable SET Email = @Email WHERE WorkmanSL = @WorkmanSL";
                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", targetEmail);
                        cmd.Parameters.AddWithValue("@WorkmanSL", workmanSL);
                        cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    if (dbcl.Conn != null && dbcl.Conn.State == ConnectionState.Open) { dbcl.Conn.Close(); }
                }

                Session.Remove("RESET_OTP");
                Session.Remove("RESET_EMAIL");
            }

            string newPassword = "ATS@" + new Random().Next(1000, 9999).ToString();
            string adminUser = Session["USERID"].ToString();

            try
            {
                bool isUpdated = UpdatePasswordInDB(workmanSL, newPassword, adminUser);
                if (isUpdated)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "CloseModal", "$('#EmailConfirmModal').modal('hide');", true);

                    if (!string.IsNullOrEmpty(targetEmail) && targetEmail != "N/A" && targetEmail.Length > 5)
                    {
                        try
                        {
                            SendPasswordEmail(targetEmail, empName, workmanSL, newPassword);
                            ShowPopup("Success", $"Password reset successfully. Email sent to: <b>{targetEmail}</b>");
                        }
                        catch (Exception)
                        {
                            ShowPopup("Partial Success", "Password reset in system, but Email failed to send.");
                        }
                    }
                    else
                    {
                        string title = "Manual Action Required";
                        string shareText = "DO NOT REPLY - ATS ERP Credentials&#13;&#10;" +
                                           "Name: " + empName + "&#13;&#10;" +
                                           "Login ID: " + loginID + "&#13;&#10;" +
                                           "Password: " + newPassword + "&#13;&#10;" +
                                           "Please change on first login.";

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
                                      "<textarea id=\"txtShare\" style=\"display:none;\">" + shareText + "</textarea>" +
                                      "<div class='text-center' style='margin-top:15px;'>" +
                                          "<button type=\"button\" class=\"btn btn-primary\" onclick=\"CopyShareText()\">" +
                                              "<i class=\"fa fa-copy\"></i> Copy to Clipboard" +
                                          "</button>" +
                                      "</div>" +
                                      "</div>";

                        ShowPopup(title, body);
                    }

                    LoadGridData();
                }
            }
            catch (Exception ex)
            {
                ShowPopup("Error", "Failed to reset password: " + ex.Message);
            }
        }

        protected void btnConfirmAndReset_Click_OLD2(object sender, EventArgs e)
        {
            string targetEmail = txtCurrentEmail.Text;
            string workmanSL = hfResetEmpWrk.Value;
            string empName = hfResetEmpName.Value;
            string loginID = hfResetLoginID.Value;

            // -----------------------------------------------------------------
            // 1. CHECK IF THEY ARE UPDATING THE EMAIL VIA OTP
            // -----------------------------------------------------------------
            if (hfIsEmailEditMode.Value == "true")
            {
                // If HR typed an email, verify the OTP and save it
                if (!string.IsNullOrEmpty(txtNewEmail.Text.Trim()))
                {
                    if (Session["RESET_OTP"] == null || txtOTP.Text.Trim() != Session["RESET_OTP"].ToString())
                    {
                        ShowPopup("Verification Failed", "The OTP entered is incorrect or expired. Please try again.");
                        string reopenScript = $"openEmailModal('{empName}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "ReopenModal", reopenScript, true);
                        return;
                    }

                    targetEmail = Session["RESET_EMAIL"].ToString();

                    try
                    {
                        string qry = "UPDATE tbl_Employee_Mustertable SET Email = @Email WHERE WorkmanSL = @WorkmanSL";
                        dbcl.Sqlconnection();
                        dbcl.ConnectDb();
                        using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                        {
                            cmd.Parameters.AddWithValue("@Email", targetEmail);
                            cmd.Parameters.AddWithValue("@WorkmanSL", workmanSL);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    finally
                    {
                        if (dbcl.Conn != null && dbcl.Conn.State == ConnectionState.Open) { dbcl.Conn.Close(); }
                    }

                    Session.Remove("RESET_OTP");
                    Session.Remove("RESET_EMAIL");
                }
                else
                {
                    // If HR clicked Edit but left the New Email blank, they are intentionally SKIPPING the email.
                    // Set targetEmail to N/A so it forces the Manual Fallback copy screen!
                    targetEmail = "N/A";
                }
            }

            // -----------------------------------------------------------------
            // 2. PROCEED WITH PASSWORD RESET (Leave the rest of your method exactly as it is!)
            // -----------------------------------------------------------------
            string newPassword = "ATS@" + new Random().Next(1000, 9999).ToString();
            string adminUser = Session["USERID"].ToString();

            try
            {
                bool isUpdated = UpdatePasswordInDB(workmanSL, newPassword, adminUser);
                if (isUpdated)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "CloseModal", "$('#EmailConfirmModal').modal('hide');", true);

                    if (!string.IsNullOrEmpty(targetEmail) && targetEmail != "N/A" && targetEmail.Length > 5)
                    {
                        try
                        {
                            SendPasswordEmail(targetEmail, empName, workmanSL, newPassword);
                            ShowPopup("Success", $"Password reset successfully. Email sent to: <b>{targetEmail}</b>");
                        }
                        catch (Exception)
                        {
                            ShowPopup("Partial Success", "Password reset in system, but Email failed to send.");
                        }
                    }
                    else
                    {
                        string title = "Manual Action Required";
                        string shareText = "DO NOT REPLY - ATS ERP Credentials&#13;&#10;" +
                                           "Name: " + empName + "&#13;&#10;" +
                                           "Login ID: " + loginID + "&#13;&#10;" +
                                           "Password: " + newPassword + "&#13;&#10;" +
                                           "Please change on first login.";

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
                                      "<textarea id=\"txtShare\" style=\"display:none;\">" + shareText + "</textarea>" +
                                      "<div class='text-center' style='margin-top:15px;'>" +
                                          "<button type=\"button\" class=\"btn btn-primary\" onclick=\"CopyShareText()\">" +
                                              "<i class=\"fa fa-copy\"></i> Copy to Clipboard" +
                                          "</button>" +
                                      "</div>" +
                                      "</div>";

                        ShowPopup(title, body);
                    }

                    LoadGridData();
                }
            }
            catch (Exception ex)
            {
                ShowPopup("Error", "Failed to reset password: " + ex.Message);
            }
        }

        protected void btnConfirmAndReset_Click(object sender, EventArgs e)
        {
            string resetMethod = hfResetMethod.Value; // Will be "email" or "manual"
            string targetEmail = txtCurrentEmail.Text;
            string workmanSL = hfResetEmpWrk.Value;
            string empName = hfResetEmpName.Value;
            string loginID = hfResetLoginID.Value;

            // -----------------------------------------------------------------
            // 1. IF USER SELECTED EMAIL -> CHECK IF THEY ARE UPDATING IT
            // -----------------------------------------------------------------
            if (resetMethod == "email" && hfIsEmailEditMode.Value == "true")
            {
                if (Session["RESET_OTP"] == null || txtOTP.Text.Trim() != Session["RESET_OTP"].ToString())
                {
                    ShowPopup("Verification Failed", "The OTP entered is incorrect or expired. Please try again.");
                    ClientScript.RegisterStartupScript(this.GetType(), "ReopenModal", $"openEmailModal('{empName}', true);", true);
                    return;
                }

                targetEmail = Session["RESET_EMAIL"].ToString();

                try
                {
                    string qry = "UPDATE tbl_Employee_Mustertable SET Email = @Email WHERE WorkmanSL = @WorkmanSL";
                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", targetEmail);
                        cmd.Parameters.AddWithValue("@WorkmanSL", workmanSL);
                        cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    if (dbcl.Conn != null && dbcl.Conn.State == ConnectionState.Open) { dbcl.Conn.Close(); }
                }

                Session.Remove("RESET_OTP");
                Session.Remove("RESET_EMAIL");
            }

            // -----------------------------------------------------------------
            // 2. GENERATE AND DISTRIBUTE PASSWORD
            // -----------------------------------------------------------------
            string newPassword = "ATS@" + new Random().Next(1000, 9999).ToString();
            string adminUser = Session["USERID"].ToString();

            try
            {
                bool isUpdated = UpdatePasswordInDB(workmanSL, newPassword, adminUser);
                if (isUpdated)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "CloseModal", "$('#EmailConfirmModal').modal('hide');", true);

                    // SCENARIO A: User explicitly chose Option 1 (Email)
                    if (resetMethod == "email" && !string.IsNullOrEmpty(targetEmail) && targetEmail != "N/A")
                    {
                        try
                        {
                            SendPasswordEmail(targetEmail, empName, workmanSL, newPassword);
                            ShowPopup("Success", $"Password reset successfully. Email sent to: <b>{targetEmail}</b>");
                        }
                        catch (Exception)
                        {
                            ShowPopup("Partial Success", "Password reset in system, but Email failed to send.");
                        }
                    }
                    // SCENARIO B: User explicitly chose Option 2 (Manual Copy)
                    else
                    {
                        string title = "Manual Action Required";
                        string shareText = "DO NOT REPLY - ATS ERP Credentials&#13;&#10;" +
                                           "Name: " + empName + "&#13;&#10;" +
                                           "Login ID: " + loginID + "&#13;&#10;" +
                                           "Password: " + newPassword + "&#13;&#10;" +
                                           "Please change on first login.";

                        string body = "<div class='text-left'>" +
                                      "Password generated successfully. Please share these details manually:<br/><br/>" +
                                      "<div style=\"background-color:#f7f7f7; border-left: 5px solid #d9534f; padding: 15px;\">" +
                                          "<h4 style=\"margin-top:0; color:#d9534f;\">" + newPassword + "</h4>" +
                                          "<small style=\"color:#555;\">Password</small>" +
                                      "</div>" +
                                      "<div style=\"margin-top:10px; border:1px solid #eee; padding:10px;\">" +
                                          "<strong>Login ID:</strong> " + loginID + "<br/>" +
                                          "<strong>Emp Code:</strong> " + workmanSL +
                                      "</div>" +
                                      "<textarea id=\"txtShare\" style=\"display:none;\">" + shareText + "</textarea>" +
                                      "<div class='text-center' style='margin-top:15px;'>" +
                                          "<button type=\"button\" class=\"btn btn-primary\" onclick=\"CopyShareText()\">" +
                                              "<i class=\"fa fa-copy\"></i> Copy to Clipboard" +
                                          "</button>" +
                                      "</div>" +
                                      "</div>";

                        ShowPopup(title, body);
                    }

                    LoadGridData();
                }
            }
            catch (Exception ex)
            {
                ShowPopup("Error", "Failed to reset password: " + ex.Message);
            }
        }

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
                    cmd.Parameters.AddWithValue("@NewPassword", newPass);
                    cmd.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dbcl.Conn != null && dbcl.Conn.State == ConnectionState.Open) { dbcl.Conn.Close(); }
            }
        }

        private void SendPasswordEmail(string toEmail, string name, string empCode, string password)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                using (SmtpClient SmtpServer = new SmtpClient("smtp.zoho.in"))
                {
                    mail.From = new MailAddress("it.support@aminruptechnologies.co.in", "ATS ERP System");
                    mail.To.Add(toEmail);
                    mail.Subject = "Security Alert: Password Reset Successful";
                    mail.IsBodyHtml = true;

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

                    SmtpServer.Port = 587;
                    SmtpServer.EnableSsl = true;

                    string smtpUser = ConfigurationManager.AppSettings["SmtpUser"] ?? "it.support@aminruptechnologies.co.in";
                    string smtpPass = ConfigurationManager.AppSettings["SmtpPass"] ?? "TPw800QrVMU2";
                    SmtpServer.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass);

                    SmtpServer.Send(mail);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DB Updated, but Email Failed: " + ex.Message, ex);
            }
        }

        private void SendOTPEmail(string toEmail, string name, string otp)
        {
            using (MailMessage mail = new MailMessage())
            using (SmtpClient SmtpServer = new SmtpClient("smtp.zoho.in"))
            {
                mail.From = new MailAddress("it.support@aminruptechnologies.co.in", "ATS ERP System");
                mail.To.Add(toEmail);
                mail.Subject = "Email Verification - OTP";
                mail.IsBodyHtml = true;

                StringBuilder body = new StringBuilder();
                body.Append("<div style='font-family:Arial,sans-serif; padding:20px; border:1px solid #ddd;'>");
                body.Append("<h2 style='color:#3498DB;'>Email Verification Code</h2>");
                body.Append($"<p>Dear {name},</p>");
                body.Append("<p>You are attempting to update your registered email address for the ATS ERP System. Please use the following One-Time Password (OTP) to verify this address.</p>");
                body.Append("<div style='background:#f7f7f7; padding:15px; border-left:4px solid #3498DB; font-size: 1.5em; letter-spacing: 5px; text-align: center; color: #333;'>");
                body.Append($"<strong>{otp}</strong>");
                body.Append("</div>");
                body.Append("<p>If you did not request this change, please contact the Administrator.</p>");
                body.Append("</div>");

                mail.Body = body.ToString();

                SmtpServer.Port = 587;
                SmtpServer.EnableSsl = true;

                string smtpUser = ConfigurationManager.AppSettings["SmtpUser"] ?? "it.support@aminruptechnologies.co.in";
                string smtpPass = ConfigurationManager.AppSettings["SmtpPass"] ?? "TPw800QrVMU2";
                SmtpServer.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass);

                SmtpServer.Send(mail);
            }
        }

        private void ShowPopup(string title, string body)
        {
            string safeBody = body.Replace("'", "\\'");
            string safeTitle = title.Replace("'", "\\'");
            safeBody = safeBody.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
            string script = "ShowPopup('" + safeTitle + "', '" + safeBody + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
        }
    }
}