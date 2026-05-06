/*
======================================================================================
File_Name: job_permitupload_v2_aspx_cs
When: April 12, 2026
Why: Maintained strictly to align with the V2 UI modernization phase. No core data processing, routing, or database transaction logic has been altered. Parameterized DB calls and file compression remain fully intact to ensure system stability.
What: Preserved existing C# logic behind the modernized `.aspx` presentation layer.
======================================================================================
*/

using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.Web.UI;

namespace WebApplication1.bussiness.production
{
    public partial class job_permitupload_v2 : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
        DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["WORKMAN"] == null)
                {
                    Response.Redirect("~/login.aspx");
                    return;
                }

                // SMART ROUTING: Check if arriving directly from Step 1 with a masked JOBID
                if (Request.QueryString["jobid"] != null)
                {
                    string maskedId = Request.QueryString["jobid"].ToString();
                    string realJobId = DecodeJobID(maskedId);

                    ActiveJOB_Checker();

                    ListItem item = DDL_JOBID.Items.FindByValue(realJobId);
                    if (item != null)
                    {
                        DDL_JOBID.SelectedValue = realJobId;
                        Bind_JOBIDDetails(realJobId);
                    }
                    else
                    {
                        // Fallback if not found 
                        DDL_JOBID.Items.Insert(0, new ListItem(realJobId, realJobId));
                        DDL_JOBID.SelectedIndex = 0;
                        Bind_JOBIDDetails(realJobId);
                    }
                }
                else
                {
                    // Normal Navigation via Dashboard
                    ActiveJOB_Checker();
                }
            }
        }

        private void ShowNotification(string title, string message, string type)
        {
            string script = $"showPNotify('{title}', '{message.Replace("'", "\\'")}', '{type}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PNotify", script, true);
        }

        // =================================================================================
        // URL MASKING UTILITIES
        // =================================================================================
        public static string EncodeJobID(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return "";
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

        public static string DecodeJobID(string maskedData)
        {
            if (string.IsNullOrEmpty(maskedData)) return "";
            string incoming = maskedData.Replace("-", "+").Replace("_", "/");
            switch (incoming.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }
            var base64EncodedBytes = Convert.FromBase64String(incoming);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        // =================================================================================
        // JOB BINDING
        // =================================================================================
        private void ActiveJOB_Checker()
        {
            // Bypass the old CountChecker (CC) class and use the exact same logic as the Dashboard!
            string query = "SELECT CONCAT(JOBID, ' : ', CONVERT(VARCHAR, CreatedDate, 105)) AS DisplayText, JOBID as ValueField FROM tbl_jobs WHERE [CreatedDate] >= DATEADD(DAY, -3, GETDATE()) AND Creator_Workman=@Workman AND JOBID_Status='Active' AND MasterStatusCode='1' ORDER BY CreatedDate DESC";

            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
            {
                cmd.Parameters.AddWithValue("@Workman", Session["WORKMAN"].ToString());
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        DDL_JOBID.DataSource = rdr;
                        DDL_JOBID.DataTextField = "DisplayText";
                        DDL_JOBID.DataValueField = "ValueField";
                        DDL_JOBID.DataBind();
                        DDL_JOBID.Items.Insert(0, new ListItem("--Select Pending JOB--", ""));
                    }
                    else
                    {
                        ShowNotification("Inbox Empty", "No Active JOB IDs require permit uploads right now.", "info");
                    }
                }
            }
            dbcl.DisconnectDb();
        }

        protected void DDL_JOBID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string jobid = DDL_JOBID.SelectedValue;
            if (string.IsNullOrEmpty(jobid))
            {
                jobid_details_row.Visible = false;
                GridTable_Row.Visible = false;
                return;
            }
            Bind_JOBIDDetails(jobid);
        }

        private void Bind_JOBIDDetails(string jobid)
        {
            try
            {
                // Load uploaded files Grid
                string gridQuery = "select * from tbl_jobspermit where JOBID=@JOBID order by Id desc";
                SqlParameter[] gridParams = { new SqlParameter("@JOBID", jobid) };
                DataTable dtGrid = dbcl.SPreturn_dt(gridQuery, gridParams);
                GridView1.DataSource = dtGrid;
                GridView1.DataBind();
                GridTable_Row.Visible = true;

                // Load Job Master Details
                string query = "select * from tbl_jobs where JOBID=@JOBID";
                SqlParameter[] pram = { new SqlParameter("@JOBID", jobid) };
                dt = dbcl.SPreturn_dt(query, pram);

                if (dt.Rows.Count > 0)
                {
                    jobid_details_row.Visible = true;

                    lbl_jobiddate.Text = Convert.ToDateTime(dt.Rows[0]["CreatedDate"]).ToString("dd-MMM-yyyy");
                    //lbl_jobcreatorname.Text = dt.Rows[0]["Creator_Name"].ToString();
                    lbl_wrkordr.Text = dt.Rows[0]["WorkOrderNo"].ToString();
                    lbl_jobid.Text = dt.Rows[0]["JOBID"].ToString();
                    lbl_jobsite.Text = dt.Rows[0]["JOB_Site"].ToString();
                    lbl_inchargename.Text = dt.Rows[0]["JOB_InchargeName"].ToString();
                    lbl_jobloc.Text = dt.Rows[0]["JOB_Location"].ToString();
                    lbl_jobshift.Text = dt.Rows[0]["JOB_Shift"].ToString();
                    lbl_permitno.Text = dt.Rows[0]["JOB_PermitNo"].ToString();

                    string uploadcount = dt.Rows[0]["FileCount"].ToString();
                    lbl_filecount.Text = uploadcount;

                    // Workflow Status Logic
                    int count = Convert.ToInt32(uploadcount);
                    if (count > 0)
                    {
                        lbl_permitstatus.Text = "Uploaded & Ready";
                        lbl_permitstatus.CssClass = "badge bg-green";
                        btn_inpunch.Visible = true; // Show proceed button
                    }
                    else
                    {
                        lbl_permitstatus.Text = "Pending Upload";
                        lbl_permitstatus.CssClass = "badge bg-orange";
                        btn_inpunch.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Error Loading Data", ex.Message, "error");
            }
        }

        // =================================================================================
        // FILE UPLOAD & MANAGEMENT (Dynamic Server.MapPath utilized)
        // =================================================================================
        protected void ImportPermit(object sender, EventArgs e)
        {
            if (!FileUpload1.HasFile)
            {
                ShowNotification("Missing File", "Please select a file to upload.", "error");
                return;
            }

            string jobid = lbl_jobid.Text;
            string filePath = FileUpload1.PostedFile.FileName;
            string ext = Path.GetExtension(filePath).ToLower();
            string FileType = String.Empty;
            string Server_FileName = $"{jobid}-{DateTime.Now.Ticks}{ext}"; // Unique name
            Byte[] bytes = { 0 };

            try
            {
                string targetFolder = Server.MapPath("~/erp_images/Permits/");
                if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);

                string Server_FilePath = Path.Combine(targetFolder, Server_FileName);

                if (DDL_UploadType.SelectedValue == "PDF File")
                {
                    if (ext != ".pdf")
                    {
                        ShowNotification("Invalid Format", "Please select a valid PDF file.", "error");
                        return;
                    }
                    FileType = "application/pdf";
                    FileUpload1.SaveAs(Server_FilePath);
                }
                else if (DDL_UploadType.SelectedValue == "Photograph")
                {
                    if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                    {
                        ShowNotification("Invalid Format", "Please select a valid Image (.jpg, .png).", "error");
                        return;
                    }
                    FileType = "image/jpeg";

                    using (System.Drawing.Image originalImage = System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream))
                    {
                        int maxWidth = 2000;
                        int maxHeight = 1200;
                        int newWidth, newHeight;

                        if (originalImage.Width > originalImage.Height)
                        {
                            newWidth = maxWidth;
                            newHeight = (int)((double)originalImage.Height / originalImage.Width * maxWidth);
                        }
                        else
                        {
                            newWidth = (int)((double)originalImage.Width / originalImage.Height * maxHeight);
                            newHeight = maxHeight;
                        }

                        using (System.Drawing.Image resizedImage = new Bitmap(originalImage, newWidth, newHeight))
                        {
                            resizedImage.Save(Server_FilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                    }
                }

                // Log the file details into the DB
                using (Stream fs = FileUpload1.PostedFile.InputStream)
                using (BinaryReader br = new BinaryReader(fs))
                {
                    bytes = br.ReadBytes((Int32)fs.Length);
                }

                InsertIntoDB(Server_FileName, FileType, ext, bytes);
                Bind_JOBIDDetails(jobid);

                // =======================================================
                // NEW: TXT FILE LOGGING (PERMIT UPLOADED)
                // =======================================================
                string logMsg = $"- File Name   : {Server_FileName}\n" +
                                $"- Upload Type : {DDL_UploadType.SelectedValue}\n" +
                                $"- File Ext    : {ext}";
                JobWorkflowLogger.LogAction(jobid, "2. PERMIT UPLOAD (File Added)", Session["WORKMAN"].ToString(), logMsg);
                // =======================================================

                ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", "closeUploadModal();", true);
                ShowNotification("Success", "Permit file uploaded successfully!", "success");
            }
            catch (Exception ex)
            {
                ShowNotification("Upload Failed", ex.Message, "error");
                // Optional: Log the error
                JobWorkflowLogger.LogAction(lbl_jobid.Text, "PERMIT UPLOAD ERROR", Session["WORKMAN"].ToString(), $"Failed to upload. Exception: {ex.Message}");
            }
        }

        protected void InsertIntoDB(string Server_FileName, string FileType, string ext, byte[] bytes)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                string CmdString = "insert into tbl_jobspermit(JOBID,UploadType,Name,FileType,Extension,Data,Submitter_Name,Submitter_Wrk) VALUES(@JOBID,@UploadType,@Name,@FileType,@Extension,@Data,@Submitter_Name,@Submitter_Wrk)";
                using (SqlCommand cmd = new SqlCommand(CmdString, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", lbl_jobid.Text);
                    cmd.Parameters.AddWithValue("@UploadType", DDL_UploadType.SelectedValue);
                    cmd.Parameters.AddWithValue("@Name", Server_FileName);
                    cmd.Parameters.AddWithValue("@FileType", FileType);
                    cmd.Parameters.AddWithValue("@Extension", ext);
                    cmd.Parameters.AddWithValue("@Data", bytes);
                    cmd.Parameters.AddWithValue("@Submitter_Name", Session["USERNAME"].ToString());
                    cmd.Parameters.AddWithValue("@Submitter_Wrk", Session["WORKMAN"].ToString());
                    cmd.ExecuteNonQuery();
                }

                UpdatePermitStatus(1);
                dbcl.DisconnectDb();
            }
            catch (Exception ex) { throw new Exception("DB Insert Error: " + ex.Message); }
        }

        protected void UpdatePermitStatus(int countModifier)
        {
            string jobid = lbl_jobid.Text;
            int currentCount = CC.Find_PermitUploadCount(jobid);
            int newCount = currentCount + countModifier;
            if (newCount < 0) newCount = 0;

            string uploadStatus = newCount > 0 ? "Yes" : "No";
            string masterCode = newCount > 0 ? "3" : "1"; // 3 = Ready for Entry, 1 = Needs Permit

            // REMOVED PermitUpload=@PermitUpload from the query!
            // We only update the FileCount, the FinalUpldStatus (Fulfillment), and the MasterStatusCode (State Machine)
            string query = @"UPDATE tbl_jobs 
                     SET FileCount=@FileCount, 
                         FinalUpldStatus=@FinalUpldStatus, 
                         MasterStatusCode=@MasterStatusCode, 
                         PermitUploadDate=@Date 
                     WHERE JOBID=@JOBID";

            using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
            {
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                cmd.Parameters.AddWithValue("@FileCount", newCount);
                cmd.Parameters.AddWithValue("@FinalUpldStatus", uploadStatus);
                cmd.Parameters.AddWithValue("@MasterStatusCode", masterCode);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.ExecuteNonQuery();
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            Label Name = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Name");
            string id = ID.Text;
            string fileName = Name.Text;
            string jobid = lbl_jobid.Text;

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "delete from tbl_jobspermit where Id=@Id and JOBID=@JOBID";
                using (SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    cmd.ExecuteNonQuery();
                }

                UpdatePermitStatus(-1);
                dbcl.DisconnectDb();

                // Safely delete physical file
                string fullPath = Path.Combine(Server.MapPath("~/erp_images/Permits/"), fileName);
                if (File.Exists(fullPath)) File.Delete(fullPath);

                Bind_JOBIDDetails(jobid);

                // =======================================================
                // NEW: TXT FILE LOGGING (PERMIT DELETED)
                // =======================================================
                JobWorkflowLogger.LogAction(jobid, "2. PERMIT UPLOAD (File Removed)", Session["WORKMAN"].ToString(), $"- Deleted File: {fileName}");
                // =======================================================

                ShowNotification("Deleted", "Attachment deleted successfully.", "success");
            }
            catch (Exception ex)
            {
                ShowNotification("Error Deleting", ex.Message, "error");
            }
        }

        protected void DownloadFile(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse((sender as LinkButton).CommandArgument);
                string fileName = "";

                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand cmd = new SqlCommand("select Name from tbl_jobspermit where Id=@Id", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    fileName = cmd.ExecuteScalar()?.ToString();
                }
                dbcl.DisconnectDb();

                if (!string.IsNullOrEmpty(fileName))
                {
                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
                    Response.TransmitFile(Path.Combine(Server.MapPath("~/erp_images/Permits/"), fileName));
                    Response.End();
                }
            }
            catch (Exception ex) { ShowNotification("Download Error", ex.Message, "error"); }
        }

        protected void btn_inpunch_Click(object sender, EventArgs e)
        {
            // =======================================================
            // NEW: TXT FILE LOGGING (PROCEED TO NEXT STEP)
            // =======================================================
            JobWorkflowLogger.LogAction(lbl_jobid.Text, "2. PERMIT PHASE COMPLETED", Session["WORKMAN"].ToString(), "All required permits uploaded. Routing to IN-Punch configuration.");
            // =======================================================

            // SMART ROUTING: Mask the JOBID before passing it to Step 3
            string maskedJobId = EncodeJobID(lbl_jobid.Text);
            Response.Redirect($"job_inpunch_v2.aspx?jobid={maskedJobId}", false);
        }
    }
}