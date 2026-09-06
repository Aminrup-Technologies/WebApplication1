using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class job_360_view : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 1. Core Security & Session Validation
                if (Session["USERID"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null)
                {
                    Response.Redirect("~/login.aspx", false);
                    return;
                }

                // 2. NEW: Auto-Load Logic (Catches the hyperlink from the Exceptions Dashboard)
                if (Request.QueryString["jobid"] != null)
                {
                    string incomingJobId = Request.QueryString["jobid"].ToString().Trim();

                    // Populate the text box so the user sees what they are looking at
                    txt_jobid.Text = incomingJobId;

                    // Immediately fire the core data loader
                    Load360View(incomingJobId);
                }
                else
                {
                    // Only set focus to the search box if they arrived manually without a URL parameter
                    txt_jobid.Focus();
                }
            }
        }

        // =================================================================================
        // ADMIN AUTHORIZATION HELPER
        // =================================================================================
        protected bool IsAdmin()
        {
            // Validates against standard Administrative Roles across the ATS Platform
            return Session["USERTYPE"] != null &&
                   (Session["USERTYPE"].ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                    Session["USERTYPE"].ToString().Equals("Office Staff", StringComparison.OrdinalIgnoreCase));
        }

        private void ShowNotification(string title, string message, string type)
        {
            if (string.IsNullOrEmpty(message)) message = "An unknown error occurred.";

            // Ensure no breaking line breaks or unescaped quotes break the JavaScript execution
            string cleanMessage = message.Replace("'", "\\'").Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "<br/>");

            string script = $"showPNotify('{title}', '{cleanMessage}', '{type}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PNotify", script, true);
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_jobid.Text))
            {
                Load360View(txt_jobid.Text.Trim());
            }
            else
            {
                ShowNotification("Validation", "Please enter a JOBID to search.", "warning");
                txt_jobid.Focus();
            }
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            txt_jobid.Text = "";
            MainDashboardRow.Visible = false;
        }

        private void Load360View(string jobid)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                string query = "SELECT * FROM tbl_jobs WHERE JOBID = @JOBID";
                using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dtJob = new DataTable();
                        da.Fill(dtJob);

                        if (dtJob.Rows.Count > 0)
                        {
                            MainDashboardRow.Visible = true;
                            DataRow row = dtJob.Rows[0];

                            // 1. CORE DETAILS & BACKDATED BADGE
                            DateTime dtCreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                            DateTime dtTimeStamp = Convert.ToDateTime(row["TimeStamp"]);

                            string backdatedBadge = "";
                            if (dtCreatedDate.Date < dtTimeStamp.Date)
                            {
                                backdatedBadge = $" <span class='badge bg-danger' title='System Recorded On: {dtTimeStamp.ToString("dd-MMM-yyyy hh:mm tt")}'>Backdated</span>";
                            }

                            lbl_jobdate.Text = dtCreatedDate.ToString("dd-MMM-yyyy") + backdatedBadge;
                            lbl_creator.Text = GetSafeString(row, "Creator_Name", "") + " [" + GetSafeString(row, "Creator_Workman", "") + "]";
                            lbl_worksite.Text = GetSafeString(row, "JOB_Site", "");
                            lbl_title.Text = GetSafeString(row, "JOB_Title", "");
                            lbl_shift.Text = GetSafeString(row, "JOB_Shift", "");
                            lbl_wo.Text = GetSafeString(row, "WorkOrderNo", "");
                            lbl_filecount.Text = GetSafeString(row, "FileCount", "0");
                            lbl_jobstatus.Text = GetSafeString(row, "JOB_Status", "");

                            // =========================================================
                            // EXTENDED ADMIN / FINANCIAL BINDINGS (Now Bulletproof)
                            // =========================================================
                            lbl_billingstatus.Text = GetSafeString(row, "Billing_Status", "Pending");
                            lbl_l1billing.Text = GetSafeString(row, "Level1_BillingCode", "N/A");
                            lbl_emc.Text = GetSafeString(row, "EMC_Number", "N/A");

                            string lumpSum = GetSafeString(row, "Lumpsum_Amount", "N/A");
                            lbl_lumpsum.Text = lumpSum == "N/A" ? "N/A" : "₹" + lumpSum;

                            lbl_tbtverifiedby.Text = GetSafeString(row, "TBT_VerifiedBy", "N/A");
                            lbl_originalpermit.Text = GetSafeString(row, "JOB_PermitNo_Original", "N/A");

                            // ---------------------------------------------------------
                            // NEW: GPS COORDINATES & GOOGLE MAPS GENERATOR
                            // ---------------------------------------------------------
                            string lat = GetSafeString(row, "GPS_Latitude", "");
                            string lon = GetSafeString(row, "GPS_Longitude", "");

                            if (!string.IsNullOrEmpty(lat) && !string.IsNullOrEmpty(lon) && lat != "N/A" && lon != "N/A")
                            {
                                // Generate a clickable Google Maps link pointing to the exact coordinates
                                lbl_gps.Text = $"<a href='https://www.google.com/maps/search/?api=1&query={lat},{lon}' " +
                                               $"target='_blank' class='text-primary font-weight-bold' style='text-decoration: none;' " +
                                               $"title='Click to view physical location on Google Maps'>" +
                                               $"<i class='fa fa-map-marker text-danger fa-lg mr-1'></i> {lat}, {lon}</a>";
                            }
                            else
                            {
                                lbl_gps.Text = "<span class='text-muted font-italic'><i class='fa fa-map-marker'></i> Not Captured</span>";
                            }

                            if (row.Table.Columns.Contains("UnblockedUntil") && row["UnblockedUntil"] != DBNull.Value)
                            {
                                DateTime unblockedTime = Convert.ToDateTime(row["UnblockedUntil"]);
                                if (unblockedTime > DateTime.Now)
                                    lbl_unblockeduntil.Text = unblockedTime.ToString("dd-MMM-yyyy hh:mm tt") + " <span class='badge bg-green'>Active</span>";
                                else
                                    lbl_unblockeduntil.Text = unblockedTime.ToString("dd-MMM-yyyy hh:mm tt") + " <span class='badge bg-red'>Expired</span>";
                            }
                            else
                            {
                                lbl_unblockeduntil.Text = "N/A";
                            }

                            // 2. BIND ALL GRIDS
                            LoadPermits(jobid);
                            LoadTBT(jobid);
                            LoadSOP(jobid);
                            LoadManpower(jobid);

                            // 3. EVALUATE LIFECYCLE & ACTION BAR
                            EvaluateSmartLifecycle(row, jobid, dtTimeStamp);

                            ShowNotification("Loaded", "JOB details loaded successfully.", "success");
                        }
                        else
                        {
                            MainDashboardRow.Visible = false;
                            ShowNotification("Not Found", "JOBID not found in the database!", "info");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Database Error", "Core Load Failed: " + ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        // =================================================================================
        // DATA GRID BINDERS
        // =================================================================================
        private void LoadPermits(string jobid)
        {
            string query = "SELECT Id, Name, UploadType, Extension, TimeStamp, Submitter_Name, Submitter_Wrk, DownloadStatus FROM tbl_jobspermit WHERE JOBID = @JOBID AND DeleteStatus=0 ORDER BY TimeStamp ASC";
            using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
            {
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable(); da.Fill(dt);
                    gvPermits.DataSource = dt; gvPermits.DataBind();
                }
            }
        }

        private void LoadTBT(string jobid)
        {
            string query = "SELECT TBT_ID, TimeStamp, TBT_SupvName, ContractEmployees, SafetySupvApprovalStatus FROM tbl_tbt WHERE Ref_JOBID = @JOBID ORDER BY TimeStamp ASC";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable(); da.Fill(dt);
                        gvTBT.DataSource = dt; gvTBT.DataBind();
                    }
                }
            }
            catch { }
        }

        private void LoadSOP(string jobid)
        {
            string query = "SELECT SOP_ID, SOPTitle, SOPTrainer, SOPDuration, SafetySupvApprovalStatus FROM tbl_sop WHERE Ref_JOBID = @JOBID ORDER BY TimeStamp ASC";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable(); da.Fill(dt);
                        gvSOP.DataSource = dt; gvSOP.DataBind();
                    }
                }
            }
            catch { }
        }

        protected void chk_ShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_jobid.Text))
            {
                try
                {
                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    LoadManpower(txt_jobid.Text.Trim());
                }
                catch (Exception ex)
                {
                    ShowNotification("Error", "Could not reload manpower: " + ex.Message, "error");
                }
                finally
                {
                    dbcl.DisconnectDb();
                }
            }
        }

        private void LoadManpower(string jobid)
        {
            string deleteFilter = chk_ShowDeleted.Checked ? "" : " AND (DeleteStatus = 0 OR DeleteStatus IS NULL)";

            string query = $@"SELECT Id, EmployeeName, EmployeeWrk, EmpDesignation, Inpunch_Time, Outpunch_Time, 
                                     TimeStamp, LastModified, WorkedHours, Calc_OT, ProvidedOT, 
                                     AttendanceStatus, AttendanceCode, DeleteStatus 
                              FROM tbl_attendance 
                              WHERE JOBID = @JOBID {deleteFilter} 
                              ORDER BY Inpunch_Time ASC";

            using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
            {
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvManpower.DataSource = dt;
                    gvManpower.DataBind();
                }
            }
        }

        protected void gvManpower_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // GAP 4 IMPLEMENTATION: Granular Roster Purging (Soft Delete)
            if (e.CommandName == "InvalidateWorker")
            {
                string workmanWrk = e.CommandArgument.ToString();
                string jobid = txt_jobid.Text.Trim();
                string adminUser = Session["USERNAME"] != null ? Session["USERNAME"].ToString() : "ADMIN";

                try
                {
                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();

                    // 1. Invalidate the worker
                    string updAtt = @"UPDATE tbl_attendance 
                                      SET DeleteStatus = 1, 
                                          AttendanceStatus = 'Invalidated', 
                                          LastModified = GETDATE(), 
                                          ModifiedByName = @Admin 
                                      WHERE JOBID = @JOBID AND EmployeeWrk = @Workman";

                    using (SqlCommand cmdAtt = new SqlCommand(updAtt, dbcl.Conn))
                    {
                        cmdAtt.Parameters.AddWithValue("@Admin", adminUser);
                        cmdAtt.Parameters.AddWithValue("@JOBID", jobid);
                        cmdAtt.Parameters.AddWithValue("@Workman", workmanWrk);
                        cmdAtt.ExecuteNonQuery();
                    }

                    // 2. Recalculate Active Manpower Headcount on Master Table
                    string updJob = @"UPDATE tbl_jobs 
                                      WHERE JOBID = @JOBID";

                    using (SqlCommand cmdJob = new SqlCommand(updJob, dbcl.Conn))
                    {
                        cmdJob.Parameters.AddWithValue("@JOBID", jobid);
                        cmdJob.ExecuteNonQuery();
                    }

                    ShowNotification("Worker Removed", $"Worker {workmanWrk} has been invalidated and removed from the active roster.", "success");
                    Load360View(jobid); // Full refresh to sync state
                }
                catch (Exception ex)
                {
                    ShowNotification("Error Invalidating Worker", ex.Message, "error");
                }
                finally
                {
                    dbcl.DisconnectDb();
                }
            }
            if (e.CommandName == "EditWorker")
            {
                string attendanceId = e.CommandArgument.ToString();
                try
                {
                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    string qry = "SELECT Id, EmployeeName, EmployeeWrk, Inpunch_Time, Outpunch_Time, ProvidedOT, AttendanceStatus FROM tbl_attendance WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", attendanceId);
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                hf_EditWorkerId.Value = rdr["Id"].ToString();
                                lbl_EditWorkerName.InnerText = $"{rdr["EmployeeName"]} [{rdr["EmployeeWrk"]}]";

                                // Format specifically for HTML5 datetime-local inputs (yyyy-MM-ddTHH:mm)
                                txt_EditInTime.Text = rdr["Inpunch_Time"] != DBNull.Value ? Convert.ToDateTime(rdr["Inpunch_Time"]).ToString("yyyy-MM-ddTHH:mm") : "";
                                txt_EditOutTime.Text = rdr["Outpunch_Time"] != DBNull.Value ? Convert.ToDateTime(rdr["Outpunch_Time"]).ToString("yyyy-MM-ddTHH:mm") : "";

                                txt_EditOT.Text = rdr["ProvidedOT"].ToString();

                                if (ddl_EditStatus.Items.FindByValue(rdr["AttendanceStatus"].ToString()) != null)
                                    ddl_EditStatus.SelectedValue = rdr["AttendanceStatus"].ToString();

                                // Trigger Modal
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowEdit", "$('#modalEditWorker').modal('show');", true);
                            }
                        }
                    }
                }
                catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
                finally { dbcl.DisconnectDb(); }
            }
        }


        protected void gvManpower_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Get the data
                DateTime inPunch = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "Inpunch_Time"));
                DateTime systemLog = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "TimeStamp"));

                // If system log is > 30 minutes AFTER the claimed in-punch, it's backdated
                if (systemLog > inPunch.AddMinutes(30))
                {
                    e.Row.BackColor = System.Drawing.Color.MistyRose; // Soft red highlight
                    e.Row.ToolTip = "Backdated Entry Detected!";
                }
            }
        }

        protected void btnSaveEdit_Click(object sender, EventArgs e)
        {
            string id = hf_EditWorkerId.Value;
            string jobid = txt_jobid.Text.Trim();

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // Validate Inputs
                if (string.IsNullOrEmpty(txt_EditInTime.Text) || string.IsNullOrEmpty(txt_EditOutTime.Text))
                {
                    ShowNotification("Validation Error", "In-Punch and Out-Punch times are required.", "warning");
                    return;
                }

                // Update the record with Transaction safety
                string updQry = @"UPDATE tbl_attendance 
                          SET Inpunch_Time = @In, 
                              Outpunch_Time = @Out, 
                              ProvidedOT = @OT, 
                              AttendanceStatus = @Status,
                              LastModified = GETDATE(),
                              ModifiedByName = @Admin
                          WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(updQry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@In", txt_EditInTime.Text);
                    cmd.Parameters.AddWithValue("@Out", txt_EditOutTime.Text);
                    cmd.Parameters.AddWithValue("@OT", string.IsNullOrEmpty(txt_EditOT.Text) ? 0 : decimal.Parse(txt_EditOT.Text));
                    cmd.Parameters.AddWithValue("@Status", ddl_EditStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@Admin", Session["USERNAME"].ToString());
                    cmd.ExecuteNonQuery();
                }

                // Log the change
                JobWorkflowLogger.LogAction(jobid, "ADMIN: EDIT WORKER", Session["WORKMAN"].ToString(), $"Attendance record {id} updated. New Status: {ddl_EditStatus.SelectedValue}");

                ShowNotification("Update Success", "Worker attendance updated.", "success");
                Load360View(jobid);
            }
            catch (Exception ex)
            {
                ShowNotification("Update Error", ex.Message, "error");
            }
            finally { dbcl.DisconnectDb(); }
        }

        protected void btn_EditCoreDetails_Click(object sender, EventArgs e)
        {
            try
            {
                // Pre-fill the textboxes with the current data from the screen
                txt_EditShift.Text = lbl_shift.Text;
                txt_EditTitle.Text = lbl_title.Text;

                // Trigger the modal to open via JavaScript
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowCoreEdit", "$('#modalEditCoreDetails').modal('show');", true);
            }
            catch (Exception ex)
            {
                ShowNotification("Error", "Could not open edit menu: " + ex.Message, "error");
            }
        }

        protected void btn_SaveCoreDetails_Click(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                string qry = @"UPDATE tbl_jobs 
                       SET JOB_Shift = @Shift, 
                           JOB_Title = @Title, 
                           UpdatedOn = GETDATE(),
                           UpdatedBy = @Admin
                       WHERE JOBID = @JOBID";

                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", txt_jobid.Text);
                    cmd.Parameters.AddWithValue("@Shift", txt_EditShift.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@Title", txt_EditTitle.Text);
                    cmd.Parameters.AddWithValue("@Admin", Session["USERNAME"].ToString());

                    cmd.ExecuteNonQuery();
                }
                ShowNotification("Success", "Core JOB details updated.", "success");
                Load360View(txt_jobid.Text);
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }


        protected void btn_SaveWorkerEdit_Click(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                string qry = @"UPDATE tbl_attendance 
                       SET Inpunch_Time = @InTime, 
                           Outpunch_Time = @OutTime, 
                           ProvidedOT = @OT, 
                           AttendanceStatus = @Status,
                           LastModified = GETDATE(),
                           ModifiedByName = @Admin,
                           ModifiedByWrk = @AdminWrk
                       WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Id", hf_EditWorkerId.Value);
                    cmd.Parameters.AddWithValue("@InTime", string.IsNullOrEmpty(txt_EditInTime.Text) ? (object)DBNull.Value : Convert.ToDateTime(txt_EditInTime.Text));
                    cmd.Parameters.AddWithValue("@OutTime", string.IsNullOrEmpty(txt_EditOutTime.Text) ? (object)DBNull.Value : Convert.ToDateTime(txt_EditOutTime.Text));
                    cmd.Parameters.AddWithValue("@OT", string.IsNullOrEmpty(txt_EditOT.Text) ? 0 : Convert.ToDecimal(txt_EditOT.Text));
                    cmd.Parameters.AddWithValue("@Status", ddl_EditStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@Admin", Session["USERNAME"].ToString());
                    cmd.Parameters.AddWithValue("@AdminWrk", Session["WORKMAN"].ToString());

                    cmd.ExecuteNonQuery();
                }
                ShowNotification("Worker Updated", "Attendance details updated successfully.", "success");
                Load360View(txt_jobid.Text); // Refresh the dashboard
            }
            catch (Exception ex) { ShowNotification("Update Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        protected void gvPermits_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DownloadDoc")
            {
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    string fileName = "";

                    dbcl.Sqlconnection(); dbcl.ConnectDb();
                    using (SqlCommand cmd = new SqlCommand("SELECT Name FROM tbl_jobspermit WHERE Id=@Id", dbcl.Conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        fileName = cmd.ExecuteScalar()?.ToString();
                    }

                    using (SqlCommand cmdUpd = new SqlCommand("UPDATE tbl_jobspermit SET DownloadStatus=1 WHERE Id=@Id", dbcl.Conn))
                    {
                        cmdUpd.Parameters.AddWithValue("@Id", id);
                        cmdUpd.ExecuteNonQuery();
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
        }
        private string GetSafeString(DataRow row, string colName, string defaultVal = "N/A")
        {
            try
            {
                if (row.Table.Columns.Contains(colName) && row[colName] != DBNull.Value)
                {
                    string val = row[colName].ToString().Trim();
                    return string.IsNullOrEmpty(val) ? defaultVal : val;
                }
            }
            catch { }
            return defaultVal;
        }

        // =================================================================================
        // DATA-RICH PIPELINE EVALUATION ENGINE (Bulletproofed)
        // =================================================================================
        private void EvaluateSmartLifecycle(DataRow row, string jobid, DateTime dtMasterJobSysCreation)
        {
            // 1. Reset Stepper Classes First
            step1.Attributes["class"] = "stepper-item"; step2.Attributes["class"] = "stepper-item";
            step3.Attributes["class"] = "stepper-item"; step4.Attributes["class"] = "stepper-item";
            step5.Attributes["class"] = "stepper-item"; step6.Attributes["class"] = "stepper-item";

            lit_step1_details.Text = lit_step2_details.Text = lit_step3_details.Text = "";
            lit_step4_details.Text = lit_step5_details.Text = lit_step6_details.Text = "";
            divBottleneck.Attributes["class"] = "alert alert-warning text-dark font-weight-bold";

            try
            {
                // 2. Extract Master Row Data Safely
                string permitUploadReq = GetSafeString(row, "PermitUpload", "No");
                string csmReq = GetSafeString(row, "CSM_Documents", "No");
                string entryExitStatus = GetSafeString(row, "EntryExit", "Created");
                string inchargeApproval = GetSafeString(row, "Incharge_Approval", "Pending");
                string finalUpldStatus = GetSafeString(row, "FinalUpldStatus", "No");

                int fileCount = row.Table.Columns.Contains("FileCount") && row["FileCount"] != DBNull.Value ? Convert.ToInt32(row["FileCount"]) : 0;
                int tbtCount = row.Table.Columns.Contains("TBT_Count") && row["TBT_Count"] != DBNull.Value ? Convert.ToInt32(row["TBT_Count"]) : 0;
                int sopCount = row.Table.Columns.Contains("SOP_Count") && row["SOP_Count"] != DBNull.Value ? Convert.ToInt32(row["SOP_Count"]) : 0;

                string creatorName = GetSafeString(row, "Creator_Name", "Unknown");
                string creatorWrk = GetSafeString(row, "Creator_Workman", "Unknown");
                string inchargeName = GetSafeString(row, "JOB_InchargeName", "Unknown");
                string inchargeWrk = GetSafeString(row, "JOB_InchargeWrk", "Unknown");

                // 3. Extract Timestamps
                DateTime? dtPermit = row.Table.Columns.Contains("PermitUploadDate") ? ParseDateSafe(row["PermitUploadDate"]) : null;
                DateTime? dtApproval = row.Table.Columns.Contains("Incharge_ApprovalDate") ? ParseDateSafe(row["Incharge_ApprovalDate"]) : null;

                DateTime? dtFirstInPunchLogic = null;
                DateTime? dtFirstInPunchSys = null;
                DateTime? dtLastOutPunchUpdated = null;
                int actualWorkerCount = 0;
                decimal totalOT = 0;

                // 4. Aggregated Attendance Data
                string attQuery = @"SELECT MIN(Inpunch_Time), MIN(TimeStamp), MAX(LastModified), COUNT(Id), SUM(ISNULL(ProvidedOT,0)) 
                            FROM tbl_attendance 
                            WHERE JOBID=@JOBID AND (DeleteStatus = 0 OR DeleteStatus IS NULL)";
                using (SqlCommand cmd = new SqlCommand(attQuery, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read() && rdr[3] != DBNull.Value && Convert.ToInt32(rdr[3]) > 0)
                        {
                            dtFirstInPunchLogic = ParseDateSafe(rdr[0]);
                            dtFirstInPunchSys = ParseDateSafe(rdr[1]);
                            dtLastOutPunchUpdated = ParseDateSafe(rdr[2]);
                            actualWorkerCount = Convert.ToInt32(rdr[3]);
                            totalOT = rdr[4] != DBNull.Value ? Convert.ToDecimal(rdr[4]) : 0;
                        }
                    }
                }

                int currentStep = 1;

                // --- STEP 1: CREATE ---
                step1.Attributes["class"] = "stepper-item completed";
                lit_step1_details.Text = $"<div class='step-details'>" +
                                         $"<strong>By:</strong> {creatorName} [{creatorWrk}]<br/>" +
                                         $"<strong>System Log:</strong> {FormatDate(dtMasterJobSysCreation)}<br/>" +
                                         $"<strong>Type:</strong> {GetSafeString(row, "Workorder_Type")} | {GetSafeString(row, "BillingType")}" +
                                         $"</div>";
                currentStep = 2;

                // --- STEP 2: PERMIT UPLOAD ---
                if (permitUploadReq != "Yes" && permitUploadReq != "Bypassed")
                {
                    step2.Attributes["class"] = "stepper-item skipped";
                    lit_step2_details.Text = "<div class='step-details text-muted'><em>Skipped (Not Req.)</em></div>";
                    currentStep = 3;
                }
                else
                {
                    if (fileCount > 0 || finalUpldStatus == "Yes")
                    {
                        step2.Attributes["class"] = "stepper-item completed";
                        string byPassNotice = permitUploadReq == "Bypassed" ? "<span class='text-danger font-weight-bold'>[Bypassed]</span>" : $"<strong>Files:</strong> {fileCount} ({GetSafeString(row, "UploadType", "Doc")})";

                        lit_step2_details.Text = $"<div class='step-details'>" +
                                                 $"{byPassNotice}<br/>" +
                                                 $"<strong>System Log:</strong> {FormatDate(dtPermit)}<br/>" +
                                                 $"{GetTATBadge(dtMasterJobSysCreation, dtPermit, "TAT")}" +
                                                 $"</div>";
                        currentStep = 3;
                    }
                    else
                    {
                        step2.Attributes["class"] = "stepper-item active";
                        lbl_bottleneck.Text = $"Awaiting Safety Permit Upload by Creator: {creatorName}";
                    }
                }

                // --- STEP 3: IN-PUNCH ---
                if (currentStep == 3)
                {
                    if (actualWorkerCount > 0)
                    {
                        step3.Attributes["class"] = "stepper-item completed";

                        string timeNotice = "";
                        if (dtFirstInPunchLogic.HasValue && dtFirstInPunchSys.HasValue && (dtFirstInPunchSys.Value - dtFirstInPunchLogic.Value).TotalHours > 12)
                        {
                            timeNotice = $"<br/><span class='text-danger' style='font-size:10px;'><i class='fa fa-warning'></i> Backdated Entry</span>";
                        }

                        lit_step3_details.Text = $"<div class='step-details'>" +
                                                 $"<strong>Headcount:</strong> {actualWorkerCount} Workers<br/>" +
                                                 $"<strong>First IN:</strong> {FormatDate(dtFirstInPunchLogic)}{timeNotice}<br/>" +
                                                 $"{GetTATBadge(dtPermit ?? dtMasterJobSysCreation, dtFirstInPunchSys, "Delay")}" +
                                                 $"</div>";
                        currentStep = 4;
                    }
                    else
                    {
                        step3.Attributes["class"] = "stepper-item active";
                        if (GetSafeString(row, "JOB_Status") == "In-Punch Done" || entryExitStatus == "Entry")
                        {
                            divBottleneck.Attributes["class"] = "alert alert-danger text-white font-weight-bold";
                            lbl_bottleneck.Text = "EMPTY SHIFT DETECTED: Supervisor opened the shift but scanned 0 workers. Admin must IN-Punch manpower or Cancel the job.";
                        }
                        else
                        {
                            lbl_bottleneck.Text = $"Job is active. Ready for Manpower IN-Punch scanning.";
                        }
                    }
                }

                // --- STEP 4: SITE DOCS (CSM) ---
                if (currentStep == 4)
                {
                    if (csmReq != "Yes")
                    {
                        step4.Attributes["class"] = "stepper-item skipped";
                        lit_step4_details.Text = "<div class='step-details text-muted'><em>Skipped (Not Req.)</em></div>";
                        currentStep = 5;
                    }
                    else
                    {
                        if (tbtCount > 0 || sopCount > 0)
                        {
                            step4.Attributes["class"] = "stepper-item completed";
                            lit_step4_details.Text = $"<div class='step-details'>" +
                                                     $"<strong>TBT:</strong> {GetSafeString(row, "TBTID")}<br/>" +
                                                     $"<strong>SOP:</strong> {GetSafeString(row, "SOPID")}<br/>" +
                                                     $"<strong>Verified:</strong> {GetSafeString(row, "TBT_VerifiedBy")}" +
                                                     $"</div>";
                            currentStep = 5;
                        }
                        else
                        {
                            step4.Attributes["class"] = "stepper-item active";
                            lbl_bottleneck.Text = $"Awaiting mandatory Site Documents (TBT / SOP).";
                        }
                    }
                }

                // --- STEP 5: OUT-PUNCH ---
                if (currentStep == 5)
                {
                    if (entryExitStatus == "Exit" || dtLastOutPunchUpdated.HasValue)
                    {
                        step5.Attributes["class"] = "stepper-item completed";
                        DateTime? validOutTime = dtLastOutPunchUpdated ?? dtFirstInPunchSys;
                        lit_step5_details.Text = $"<div class='step-details'>" +
                                                 $"<strong>System Log:</strong> {FormatDate(validOutTime)}<br/>" +
                                                 $"<strong>Total OT Logged:</strong> {totalOT} Hrs<br/>" +
                                                 $"{GetTATBadge(dtFirstInPunchSys, validOutTime, "Shift Duration")}" +
                                                 $"</div>";
                        currentStep = 6;
                    }
                    else
                    {
                        step5.Attributes["class"] = "stepper-item active";
                        lbl_bottleneck.Text = "Job is actively running. Awaiting Shift Completion and OUT-Punch.";
                    }
                }

                // --- STEP 6: FINAL APPROVAL ---
                if (currentStep == 6)
                {
                    if (inchargeApproval == "Approved")
                    {
                        step6.Attributes["class"] = "stepper-item completed";
                        lit_step6_details.Text = $"<div class='step-details'>" +
                                                 $"<strong>By:</strong> {inchargeName} [{inchargeWrk}]<br/>" +
                                                 $"<strong>Approved On:</strong> {FormatDate(dtApproval)}<br/>" +
                                                 $"{GetTATBadge(dtLastOutPunchUpdated ?? dtFirstInPunchSys, dtApproval, "Approval TAT")}" +
                                                 $"</div>";
                        divBottleneck.Attributes["class"] = "alert alert-success text-dark font-weight-bold";
                        lbl_bottleneck.Text = $"Job Lifecycle Completed. Ready for Billing (Level 1 Code: {GetSafeString(row, "Level1_BillingCode")}).";
                    }
                    else if (inchargeApproval == "Rejected" || inchargeApproval == "Returned")
                    {
                        step6.Attributes["class"] = "stepper-item failed";
                        divBottleneck.Attributes["class"] = "alert alert-danger text-white font-weight-bold";
                        lbl_bottleneck.Text = $"Job was {inchargeApproval.ToUpper()} by Final Approver: {inchargeName}. Reason: {GetSafeString(row, "Incharge_Remarks")}";

                        lit_step6_details.Text = $"<div class='step-details text-danger'>" +
                                                 $"<strong>Status:</strong> {inchargeApproval}<br/>" +
                                                 $"<strong>By:</strong> {inchargeName}" +
                                                 $"</div>";
                    }
                    else
                    {
                        step6.Attributes["class"] = "stepper-item active";
                        lbl_bottleneck.Text = $"Awaiting Final Approval & Verification from: {inchargeName}";

                        lit_step6_details.Text = $"<div class='step-details text-muted'>" +
                                                 $"<strong>Pending:</strong> {inchargeName}" +
                                                 $"</div>";
                    }
                }

                // =========================================================
                // PIPELINE ABORT OVERRIDE (For Deleted/Cancelled Jobs)
                // =========================================================
                bool isDeleted = row.Table.Columns.Contains("DeleteStatus") && row["DeleteStatus"] != DBNull.Value && (row["DeleteStatus"].ToString() == "1" || row["DeleteStatus"].ToString().ToLower() == "true");
                string jobidStatus = GetSafeString(row, "JOBID_Status");

                if (isDeleted || jobidStatus == "Deleted" || jobidStatus == "Cancelled")
                {
                    if (currentStep == 2) step2.Attributes["class"] = "stepper-item failed";
                    else if (currentStep == 3) step3.Attributes["class"] = "stepper-item failed";
                    else if (currentStep == 4) step4.Attributes["class"] = "stepper-item failed";
                    else if (currentStep == 5) step5.Attributes["class"] = "stepper-item failed";
                    else if (currentStep == 6) step6.Attributes["class"] = "stepper-item failed";

                    divBottleneck.Attributes["class"] = "alert alert-danger text-white font-weight-bold";
                    lbl_bottleneck.Text = $"<i class='fa fa-ban'></i> PIPELINE ABORTED: This JOB was {jobidStatus.ToUpper()} on {FormatDate(ParseDateSafe(GetSafeString(row, "DeleteOn")))} by {GetSafeString(row, "DeletedBy")}.";
                }
                else if (row.Table.Columns.Contains("IsBlocked") && row["IsBlocked"] != DBNull.Value && Convert.ToBoolean(row["IsBlocked"]))
                {
                    divBottleneck.Attributes["class"] = "alert alert-danger text-white font-weight-bold";
                    lbl_bottleneck.Text = "SYSTEM ALERT: This JOBID has been administratively BLOCKED due to 72-Hour timeout.";
                }

                bool isBlocked = row.Table.Columns.Contains("IsBlocked") && row["IsBlocked"] != DBNull.Value && Convert.ToBoolean(row["IsBlocked"]);
                ActionBarRow.Visible = true;
                EvaluateActionMatrix(row, currentStep, isBlocked, dtFirstInPunchLogic, ParseDateSafe(row["CreatedDate"]), actualWorkerCount);
            }
            catch (Exception ex)
            {
                // THIS CATCHES THE SILENT CRASH AND PRINTS IT IN THE YELLOW BOX!
                divBottleneck.Attributes["class"] = "alert alert-danger text-white font-weight-bold";
                lbl_bottleneck.Text = "PIPELINE RENDER ERROR: " + ex.Message;
            }
        }

        private void EvaluateActionMatrix_OLD(DataRow row, int pipelineStep, bool isBlocked, DateTime? dtFirstInPunchLogic, DateTime? dtCreatedDate, int actualWorkerCount)
        {
            // 1. Hide all buttons initially
            btn_Act_UploadPermit.Visible = false; btn_Act_InPunch.Visible = false; btn_Act_AddDocs.Visible = false;
            btn_Act_OutPunch.Visible = false; btn_Act_Unblock.Visible = false; btn_Act_Resubmit.Visible = false;
            btn_Act_ForceOut.Visible = false; btn_Act_SwapDate.Visible = false; btn_Act_Delete.Visible = false;

            btn_Act_ForcePermitBypass.Visible = false; btn_Act_ResetToCreated.Visible = false;
            btn_Act_CancelShift.Visible = false; btn_Act_AdminRollback.Visible = false;

            string approvalStatus = row["Incharge_Approval"].ToString();
            string entryExitStatus = row["EntryExit"].ToString();
            string masterCode = row["MasterStatusCode"].ToString();

            // Safely parse the DeleteStatus
            bool isDeleted = row["DeleteStatus"] != DBNull.Value &&
                             (row["DeleteStatus"].ToString() == "1" || row["DeleteStatus"].ToString().ToLower() == "true");

            if (isDeleted || row["JOBID_Status"].ToString() == "Deleted" || row["JOBID_Status"].ToString() == "Cancelled")
            {
                divBottleneck.Attributes["class"] = "alert alert-danger text-white font-weight-bold";
                lbl_bottleneck.Text = $"<i class='fa fa-ban'></i> ARCHIVED/VOID RECORD: This JOB was DELETED/CANCELLED on {FormatDate(ParseDateSafe(row["DeleteOn"]))}. No further actions allowed.";
                return;
            }

            // ----------------------------------------------------------------------
            // ADMIN OVERRIDES (God Mode Gaps)
            // ----------------------------------------------------------------------
            if (IsAdmin())
            {
                // GAP 1: Force Permit Bypass (Stuck at Step 1/2)
                if (masterCode == "1")
                {
                    btn_Act_ForcePermitBypass.Visible = true;
                    btn_Act_CancelShift.Visible = true;
                }

                // GAP 2 & 3: Reset to Created or Cancel Shift (At Step 3, but NO workers IN-Punched)
                if (masterCode == "3" && actualWorkerCount == 0)
                {
                    btn_Act_ResetToCreated.Visible = true;
                    btn_Act_CancelShift.Visible = true;
                }

                // Master Approval Rollback
                if (approvalStatus == "Approved")
                {
                    btn_Act_AdminRollback.Visible = true;
                    return; // Halt standard matrix
                }
            }
            else if (approvalStatus == "Approved")
            {
                return; // Normal users see nothing
            }

            // 3. UNBLOCK PRIORITY
            if ((isBlocked || row["JOBID_Status"].ToString() == "Blocked") && entryExitStatus != "Exit")
            {
                if (IsAdmin()) btn_Act_Unblock.Visible = true;
                return;
            }

            // 4. EVALUATE 3-DAY WINDOW
            bool isWithin3Days = false;
            if (dtCreatedDate.HasValue)
            {
                isWithin3Days = dtCreatedDate.Value.Date >= DateTime.Now.AddDays(-3).Date;
            }

            if (isWithin3Days)
            {
                if (pipelineStep == 2) btn_Act_UploadPermit.Visible = true;
                if (pipelineStep == 3) btn_Act_InPunch.Visible = true;
                if (pipelineStep == 4) btn_Act_AddDocs.Visible = true;
                if (pipelineStep == 5) btn_Act_OutPunch.Visible = true;

                if (entryExitStatus == "Entry" && dtFirstInPunchLogic.HasValue && dtCreatedDate.HasValue && dtCreatedDate.Value.Date < DateTime.Now.Date)
                {
                    btn_Act_ForceOut.Visible = true;
                }
            }
            else
            {
                lbl_bottleneck.Text = "SYSTEM LOCKOUT: Job is older than 3 days. Standard processing is disabled.";
                if ((entryExitStatus == "Entry" || entryExitStatus == "Created") && dtFirstInPunchLogic.HasValue)
                {
                    if (IsAdmin()) btn_Act_ForceOut.Visible = true;
                    lbl_bottleneck.Text += " Admin must Force OUT-Punch to close this shift.";
                }
            }

            // 5. FIX & RESUBMIT
            if (approvalStatus == "Rejected" || approvalStatus == "Returned" || approvalStatus == "Cancelled")
            {
                btn_Act_Resubmit.Visible = true;
            }

            // 6. PRE-PUNCH CONTROLS
            if (!dtFirstInPunchLogic.HasValue)
            {
                btn_Act_SwapDate.Visible = true;
                btn_Act_Delete.Visible = true;
                if (!isWithin3Days) lbl_bottleneck.Text += " No attendance logged. Please Swap the Job Date to a current date, or Delete the record.";
            }
        }

        private void EvaluateActionMatrix(DataRow row, int pipelineStep, bool isBlocked, DateTime? dtFirstInPunchLogic, DateTime? dtCreatedDate, int actualWorkerCount)
        {
            ResetActionButtonVisibility();

            divBottleneck.Attributes["class"] = "d-none";
            lbl_bottleneck.Text = "";

            string approvalStatus = row["Incharge_Approval"].ToString();
            string entryExitStatus = row["EntryExit"].ToString();
            string masterCode = row["MasterStatusCode"].ToString();
            string jobStatus = row["JOBID_Status"].ToString();

            bool isDeleted = row["DeleteStatus"] != DBNull.Value &&
                             (row["DeleteStatus"].ToString() == "1" || row["DeleteStatus"].ToString().ToLower() == "true");

            if (isDeleted || jobStatus == "Deleted" || jobStatus == "Cancelled")
            {
                divBottleneck.Attributes["class"] = "alert alert-danger text-white font-weight-bold";
                lbl_bottleneck.Text = $"<i class='fa fa-ban'></i> ARCHIVED/VOID RECORD: This JOB was DELETED/CANCELLED. No further actions allowed.";
                ActionBarRow.Visible = false;
                return;
            }

            ApplyAdminOverrideVisibility(approvalStatus, masterCode, actualWorkerCount);

            bool isWithin3Days = dtCreatedDate.HasValue && dtCreatedDate.Value.Date >= DateTime.Now.AddDays(-3).Date;
            DateTime? graceExpiry = row["UnblockedUntil"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["UnblockedUntil"]) : null;
            bool isGraceActive = graceExpiry.HasValue && DateTime.Now < graceExpiry.Value;
            bool windowOpen = isWithin3Days || isGraceActive;

            ApplyUnblockVisibility(isBlocked, jobStatus, entryExitStatus);

            if (windowOpen)
            {
                if (isGraceActive)
                {
                    divBottleneck.Attributes["class"] = "alert alert-warning text-dark font-weight-bold";
                    lbl_bottleneck.Text = $"<i class='fa fa-clock-o'></i> GRACE PERIOD ACTIVE: Job unblocked until {graceExpiry.Value:dd-MMM HH:mm}";
                }

                ApplySupervisorHopVisibility(row, pipelineStep, entryExitStatus, actualWorkerCount);
                ApplyInWindowForceOutVisibility(entryExitStatus, dtFirstInPunchLogic, dtCreatedDate);
            }
            else
            {
                divBottleneck.Attributes["class"] = "alert alert-danger text-white font-weight-bold";
                lbl_bottleneck.Text = "SYSTEM LOCKOUT: Job is older than 3 days. Standard processing is disabled.";
                ApplyLockoutForceOutVisibility(entryExitStatus, dtFirstInPunchLogic);
            }

            ApplyResubmitVisibility(approvalStatus);
            ApplyPrePunchOverrideVisibility(dtFirstInPunchLogic, windowOpen);

            ActionBarRow.Visible = btn_Act_UploadPermit.Visible
                || btn_Act_InPunch.Visible
                || btn_Act_AddDocs.Visible
                || btn_Act_OutPunch.Visible;
        }

        private void ResetActionButtonVisibility()
        {
            btn_Act_UploadPermit.Visible = false;
            btn_Act_InPunch.Visible = false;
            btn_Act_AddDocs.Visible = false;
            btn_Act_OutPunch.Visible = false;
            btn_Act_Unblock.Visible = false;
            btn_Act_Resubmit.Visible = false;
            btn_Act_ForceOut.Visible = false;
            btn_Act_SwapDate.Visible = false;
            btn_Act_Delete.Visible = false;
            btn_Act_ForcePermitBypass.Visible = false;
            btn_Act_ResetToCreated.Visible = false;
            btn_Act_CancelShift.Visible = false;
            btn_Act_AdminRollback.Visible = false;
            // Phase D / Phase C: stay hidden. Do not enable here.
            btn_Act_ViewRawData.Visible = false;
            btn_EditCoreDetails.Visible = false;
        }

        private void ApplyAdminOverrideVisibility(string approvalStatus, string masterCode, int actualWorkerCount)
        {
            if (!IsAdmin()) return;

            if (masterCode == "1")
            {
                btn_Act_ForcePermitBypass.Visible = true;
                btn_Act_CancelShift.Visible = true;
            }

            if (masterCode == "3" && actualWorkerCount == 0)
            {
                btn_Act_ResetToCreated.Visible = true;
                btn_Act_CancelShift.Visible = true;
            }

            if (approvalStatus == "Approved")
            {
                btn_Act_AdminRollback.Visible = true;
            }
        }

        private void ApplyUnblockVisibility(bool isBlocked, string jobStatus, string entryExitStatus)
        {
            if ((isBlocked || jobStatus == "Blocked") && entryExitStatus != "Exit" && IsAdmin())
            {
                btn_Act_Unblock.Visible = true;
            }
        }

        /// <summary>
        /// Overview hops follow the frozen M1–M6 order: Create → IN → Permit → OUT.
        /// Permit outstanding must not hide IN. V2 inboxes remain the write surface.
        /// </summary>
        private void ApplySupervisorHopVisibility(DataRow row, int pipelineStep, string entryExitStatus, int actualWorkerCount)
        {
            bool isCreated = entryExitStatus == "Created";
            bool isEntry = entryExitStatus == "Entry";
            bool isExit = entryExitStatus == "Exit";

            if (isExit) return;

            // IN at Created (including permit-required). Empty Entry shift keeps the existing IN hop.
            if (isCreated || (isEntry && actualWorkerCount == 0))
            {
                btn_Act_InPunch.Visible = true;
            }

            // Permit: optional at Created while outstanding; always while Entry (PR #64 inbox).
            if ((isCreated && IsPermitOutstanding(row)) || isEntry)
            {
                btn_Act_UploadPermit.Visible = true;
            }

            // Add Docs: existing CSM step predicate (pipelineStep 4).
            if (pipelineStep == 4)
            {
                btn_Act_AddDocs.Visible = true;
            }

            // OUT while Entry with manpower (Close & Send stays on job_outpunch_v2).
            if (isEntry && actualWorkerCount > 0)
            {
                btn_Act_OutPunch.Visible = true;
            }
        }

        private static bool IsPermitOutstanding(DataRow row)
        {
            string permitUpload = row.Table.Columns.Contains("PermitUpload") ? row["PermitUpload"].ToString() : "";
            string finalUpldStatus = row.Table.Columns.Contains("FinalUpldStatus") ? row["FinalUpldStatus"].ToString() : "";
            int fileCount = 0;
            if (row.Table.Columns.Contains("FileCount") && row["FileCount"] != DBNull.Value)
            {
                int.TryParse(row["FileCount"].ToString(), out fileCount);
            }

            return permitUpload == "Yes" && fileCount == 0 && finalUpldStatus != "Yes";
        }

        private void ApplyInWindowForceOutVisibility(string entryExitStatus, DateTime? dtFirstInPunchLogic, DateTime? dtCreatedDate)
        {
            if (entryExitStatus == "Entry" && dtFirstInPunchLogic.HasValue && dtCreatedDate.HasValue && dtCreatedDate.Value.Date < DateTime.Now.Date)
            {
                btn_Act_ForceOut.Visible = true;
            }
        }

        private void ApplyLockoutForceOutVisibility(string entryExitStatus, DateTime? dtFirstInPunchLogic)
        {
            if ((entryExitStatus == "Entry" || entryExitStatus == "Created") && dtFirstInPunchLogic.HasValue)
            {
                if (IsAdmin()) btn_Act_ForceOut.Visible = true;
                lbl_bottleneck.Text += " Admin must Force OUT-Punch to close this shift.";
            }
        }

        private void ApplyResubmitVisibility(string approvalStatus)
        {
            if (approvalStatus == "Rejected" || approvalStatus == "Returned" || approvalStatus == "Cancelled")
            {
                btn_Act_Resubmit.Visible = true;
            }
        }

        private void ApplyPrePunchOverrideVisibility(DateTime? dtFirstInPunchLogic, bool windowOpen)
        {
            if (dtFirstInPunchLogic.HasValue) return;

            btn_Act_SwapDate.Visible = true;
            btn_Act_Delete.Visible = true;
            if (!windowOpen) lbl_bottleneck.Text += " No attendance logged. Swap Date or Delete.";
        }

        // =================================================================================
        // ACTION BUTTON EXECUTORS
        // =================================================================================
        // V2 pages DecodeJobID() (URL-safe Base64). Encode here; do not pass raw JOByyMMddXXX.
        protected void btn_Act_UploadPermit_Click(object sender, EventArgs e)
        {
            string jobid = (txt_jobid.Text ?? "").Trim();
            if (string.IsNullOrEmpty(jobid)) return;
            Response.Redirect($"job_permitupload_v2.aspx?jobid={create_jobid_v2.EncodeJobID(jobid)}", false);
        }
        protected void btn_Act_InPunch_Click(object sender, EventArgs e)
        {
            string jobid = (txt_jobid.Text ?? "").Trim();
            if (string.IsNullOrEmpty(jobid)) return;
            Response.Redirect($"job_inpunch_v2.aspx?jobid={create_jobid_v2.EncodeJobID(jobid)}", false);
        }
        protected void btn_Act_OutPunch_Click(object sender, EventArgs e)
        {
            string jobid = (txt_jobid.Text ?? "").Trim();
            if (string.IsNullOrEmpty(jobid)) return;
            Response.Redirect($"job_outpunch_v2.aspx?jobid={create_jobid_v2.EncodeJobID(jobid)}", false);
        }
        protected void btn_Act_AddDocs_Click(object sender, EventArgs e) { Response.Redirect($"manage_compliance_docs.aspx?jobid={txt_jobid.Text}", false); }
        protected void btn_Act_SwapDate_Click(object sender, EventArgs e) { Response.Redirect($"swap_jobdate.aspx?jobid={txt_jobid.Text}", false); }

        protected void btn_Act_Unblock_Click(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection(); dbcl.ConnectDb();
                string qry = "UPDATE tbl_jobs SET IsBlocked = 0, JOBID_Status = 'Active', BlockedTimestamp = NULL, UnblockedUntil = DATEADD(HOUR, 24, GETDATE()) WHERE JOBID = @JOBID";
                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", txt_jobid.Text);
                    cmd.ExecuteNonQuery();
                }
                ShowNotification("Unblocked", "JOB has been unblocked. A 24-hour grace period has been applied.", "success");
                Load360View(txt_jobid.Text);
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        // =================================================================================
        // ADMIN END-TO-END CONTROLS
        // =================================================================================

        // GAP 1: Emergency Compliance Bypass
        protected void btn_Act_ForcePermitBypass_Click(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection(); dbcl.ConnectDb();
                string qry = "UPDATE tbl_jobs SET PermitUpload='Bypassed', FinalUpldStatus='Yes', JOB_Status='Permit Bypassed (Emergency)', MasterStatusCode='3', EntryExit='Entry' WHERE JOBID = @JOBID";
                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", txt_jobid.Text);
                    cmd.ExecuteNonQuery();
                }
                ShowNotification("Bypassed", "Safety Permits have been bypassed. The JOB is unlocked for IN-Punching.", "success");
                Load360View(txt_jobid.Text);
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        // GAP 2: Granular State Rollback (Step 3 -> Step 1)
        protected void btn_Act_ResetToCreated_Click(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection(); dbcl.ConnectDb();

                using (SqlCommand cmdDel = new SqlCommand("DELETE FROM tbl_jobspermit WHERE JOBID=@JOBID", dbcl.Conn))
                {
                    cmdDel.Parameters.AddWithValue("@JOBID", txt_jobid.Text);
                    cmdDel.ExecuteNonQuery();
                }

                string qry = "UPDATE tbl_jobs SET PermitUpload='No', FinalUpldStatus='No', JOB_Status='Created', MasterStatusCode='1', EntryExit='Created', FileCount=0 WHERE JOBID = @JOBID";
                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", txt_jobid.Text);
                    cmd.ExecuteNonQuery();
                }
                ShowNotification("Reset Successful", "JOB has been rolled back to Step 1 (Created). Permits must be re-uploaded.", "success");
                Load360View(txt_jobid.Text);
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        // GAP 3: Shift Cancellation / Ghost Jobs
        protected void btn_Act_CancelShift_Click(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection(); dbcl.ConnectDb();
                string qry = @"UPDATE tbl_jobs 
                               SET JOBID_Status='Cancelled', JOB_Status='Voided by Admin', 
                                   MasterStatusCode='6', EntryExit='Deleted', 
                                   DeleteStatus=1, DeleteOn=GETDATE(), DeletedBy=@User 
                               WHERE JOBID = @JOBID";
                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", txt_jobid.Text);
                    cmd.Parameters.AddWithValue("@User", Session["USERNAME"].ToString());
                    cmd.ExecuteNonQuery();
                }
                ShowNotification("Shift Voided", "The Ghost Shift has been cancelled and permanently archived.", "success");
                Load360View(txt_jobid.Text);
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        protected void btn_Act_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                string qry = @"UPDATE tbl_jobs 
                               SET DeleteStatus = 1, 
                                   DeleteOn = GETDATE(), 
                                   DeletedBy = @User,
                                   JOBID_Status = 'Deleted',
                                   MasterStatusCode = '0',
                                   JOB_Status = 'Deleted',
                                   EntryExit = 'Deleted',
                                   Incharge_Approval = 'Cancelled',
                                   IsBlocked = 0 

                               WHERE JOBID = @JOBID";

                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", txt_jobid.Text);
                    cmd.Parameters.AddWithValue("@User", Session["USERNAME"].ToString());
                    cmd.ExecuteNonQuery();
                }

                ShowNotification("Deleted", "JOB has been securely deleted and completely removed from all active operational workflows.", "success");
                btn_reset_Click(null, null); // Clear the dashboard UI
            }
            catch (Exception ex)
            {
                ShowNotification("Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        protected void btn_Act_Resubmit_Click(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // 1. INITIATE TRANSACTION
                SqlTransaction transaction = dbcl.Conn.BeginTransaction();

                try
                {
                    // 2. Execute State Rollback
                    string qryJob = @"UPDATE tbl_jobs 
                              SET MasterStatusCode = '3', 
                                  JOB_Status = 'Permit Uploaded', 
                                  EntryExit = 'Entry', 
                                  UpdatedBy = @AdminName, 
                                  UpdatedOn = GETDATE() 
                              WHERE JOBID = @JOBID";

                    using (SqlCommand cmdJob = new SqlCommand(qryJob, dbcl.Conn, transaction))
                    {
                        cmdJob.Parameters.AddWithValue("@JOBID", txt_jobid.Text);
                        cmdJob.Parameters.AddWithValue("@AdminName", Session["USERNAME"].ToString());
                        cmdJob.ExecuteNonQuery();
                    }

                    // 3. COMMIT TRANSACTION
                    transaction.Commit();

                    // 4. LOG THE ACTION USING YOUR NATIVE LOGGER
                    JobWorkflowLogger.LogAction(txt_jobid.Text, "ADMIN OVERRIDE: FIX & RESUBMIT", Session["WORKMAN"].ToString(), "JOB reset to Step 3 (IN-Punch phase) to allow supervisor adjustments.");

                    ShowNotification("Job Reset", "The JOBID has been reset to the IN-Punch phase.", "success");
                    Load360View(txt_jobid.Text);
                }
                catch (Exception exTransaction)
                {
                    transaction.Rollback();
                    throw new Exception("Reset failed: " + exTransaction.Message);
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Critical Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        protected void btn_Act_Resubmit_Click_OLD(object sender, EventArgs e)
        {
            string jobid = txt_jobid.Text.Trim();
            string adminUser = Session["USERNAME"] != null ? Session["USERNAME"].ToString() : "ADMIN";

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                string qryJob = @"UPDATE tbl_jobs 
                                  SET Incharge_Approval = 'Pending', 
                                      Incharge_Remarks = CONCAT(ISNULL(Incharge_Remarks,''), ' | ADMIN RESUBMIT: Opened for corrections by ', @AdminUser),
                                      EntryExit = 'Entry',
                                      JOB_Status = 'In-Punch Done', 
                                      MasterStatusCode = '3',
                                      IsBlocked = 0, 
                                      JOBID_Status = 'Active',
                                      BlockedTimestamp = NULL,
                                      UnblockedUntil = DATEADD(HOUR, 24, GETDATE()),
                                      UpdatedBy = @AdminUser,
                                      UpdatedOn = GETDATE()                  
                                  WHERE JOBID = @JOBID";

                using (SqlCommand cmdJob = new SqlCommand(qryJob, dbcl.Conn))
                {
                    cmdJob.Parameters.AddWithValue("@JOBID", jobid);
                    cmdJob.Parameters.AddWithValue("@AdminUser", adminUser);
                    cmdJob.ExecuteNonQuery();
                }

                string qryAtt = @"UPDATE tbl_attendance 
                                      Approval_Date = NULL,
                                      AttendanceStatus = 'Absent',
                                      LastModified = GETDATE(),
                                      ModifiedByName = @AdminUser
                                  WHERE JOBID = @JOBID";

                using (SqlCommand cmdAtt = new SqlCommand(qryAtt, dbcl.Conn))
                {
                    cmdAtt.Parameters.AddWithValue("@JOBID", jobid);
                    cmdAtt.Parameters.AddWithValue("@AdminUser", adminUser);
                    cmdAtt.ExecuteNonQuery();
                }

                ShowNotification("Resubmitted", "JOB rolled back successfully. Worker attendance statuses have been reset.", "success");
                Load360View(jobid);
            }
            catch (Exception ex)
            {
                ShowNotification("Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        protected void btn_Act_ForceOut_Click(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // 1. INITIATE TRANSACTION
                SqlTransaction transaction = dbcl.Conn.BeginTransaction();

                try
                {
                    // 2. Fetch Stuck Workers (Pass transaction to the command)
                    string getStuckWorkersQry = "SELECT Id, EmployeeWrk, Inpunch_Time, WourkHours, LunchFactor FROM tbl_attendance WHERE JOBID = @JOBID AND Outpunch_Time IS NULL AND (DeleteStatus = 0 OR DeleteStatus IS NULL)";

                    DataTable dtStuckWorkers = new DataTable();
                    using (SqlCommand cmdGet = new SqlCommand(getStuckWorkersQry, dbcl.Conn, transaction))
                    {
                        cmdGet.Parameters.AddWithValue("@JOBID", txt_jobid.Text);
                        using (SqlDataReader rdr = cmdGet.ExecuteReader())
                        {
                            dtStuckWorkers.Load(rdr);
                        }
                    }

                    // 3. Loop and Force Close EACH worker (Must use the transaction)
                    foreach (DataRow worker in dtStuckWorkers.Rows)
                    {
                        int id = Convert.ToInt32(worker["Id"]);
                        string empWrk = worker["EmployeeWrk"].ToString();
                        DateTime inTime = Convert.ToDateTime(worker["Inpunch_Time"]);
                        int standardHours = worker["WourkHours"] != DBNull.Value ? Convert.ToInt32(worker["WourkHours"]) : 8;
                        string lunchFactor = worker["LunchFactor"] != DBNull.Value ? worker["LunchFactor"].ToString() : "No";

                        DateTime outTime = inTime.AddHours(standardHours);
                        int workedTimeMins = standardHours * 60;
                        decimal workedHoursDec = Convert.ToDecimal(standardHours);

                        using (SqlCommand cmdSP = new SqlCommand("SP_Update_AttendancePunchOUT", dbcl.Conn, transaction))
                        {
                            cmdSP.CommandType = CommandType.StoredProcedure;
                            cmdSP.Parameters.AddWithValue("@Id", id);
                            cmdSP.Parameters.AddWithValue("@JOBID", txt_jobid.Text);
                            cmdSP.Parameters.AddWithValue("@SubmitterStatus", "Exit");
                            cmdSP.Parameters.AddWithValue("@EmployeeWrk", empWrk);
                            cmdSP.Parameters.AddWithValue("@Outpunch_Time", outTime);
                            cmdSP.Parameters.AddWithValue("@WorkedTime", workedTimeMins);
                            cmdSP.Parameters.AddWithValue("@WorkedHours", workedHoursDec);
                            cmdSP.Parameters.AddWithValue("@LunchFactor", lunchFactor);
                            cmdSP.Parameters.AddWithValue("@Calc_OT", 0);
                            cmdSP.Parameters.AddWithValue("@ProvidedOT", 0);
                            cmdSP.Parameters.AddWithValue("@LastModified", DateTime.Now);
                            cmdSP.Parameters.AddWithValue("@AttendanceStatus", "Present");
                            cmdSP.Parameters.AddWithValue("@AttendanceCode", "P");

                            cmdSP.ExecuteNonQuery();
                        }
                    }

                    // 4. Update the Master Job Status
                    string qryJob = "UPDATE tbl_jobs SET JOBID_Status = 'Active', JOB_Status = 'Out-Punch Done', MasterStatusCode = '4', EntryExit = 'Exit' WHERE JOBID = @JOBID";
                    using (SqlCommand cmdJob = new SqlCommand(qryJob, dbcl.Conn, transaction))
                    {
                        cmdJob.Parameters.AddWithValue("@JOBID", txt_jobid.Text);
                        cmdJob.ExecuteNonQuery();
                    }

                    // 5. IF EVERYTHING SUCCEEDED, COMMIT THE DATABASE CHANGES
                    transaction.Commit();
                    JobWorkflowLogger.LogAction(txt_jobid.Text, "ADMIN OVERRIDE: FORCE OUT-PUNCH", Session["WORKMAN"].ToString(), "Admin forcefully closed shift for all active workers via 360 View.");
                    ShowNotification("Forced Closed", "All active manpower forcefully clocked out safely. JOB advanced to Approver queue.", "success");
                    Load360View(txt_jobid.Text);

                    // EMAIL NOTIFICATION INTEGRATION
                    // dbcl.SendEmail("supervisor_email@domain.com", $"ATS Alert: {txt_jobid.Text} Force Closed", emailBody);
                }
                catch (Exception exTransaction)
                {
                    // IF ANYTHING FAILS (Server crash, type mismatch), REVERT ALL CHANGES
                    transaction.Rollback();
                    throw new Exception("Transaction Failed and Rolled Back. Reason: " + exTransaction.Message);
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Critical Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        protected void btn_Act_ForceOut_Click_OLD(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection(); dbcl.ConnectDb();

                string getStuckWorkersQry = "SELECT Id, EmployeeWrk, Inpunch_Time, WourkHours, LunchFactor FROM tbl_attendance WHERE JOBID = @JOBID AND Outpunch_Time IS NULL AND (DeleteStatus = 0 OR DeleteStatus IS NULL)";
                using (SqlCommand cmdGet = new SqlCommand(getStuckWorkersQry, dbcl.Conn))
                {
                    cmdGet.Parameters.AddWithValue("@JOBID", txt_jobid.Text);
                    using (SqlDataReader rdr = cmdGet.ExecuteReader())
                    {
                        DataTable dtStuckWorkers = new DataTable();
                        dtStuckWorkers.Load(rdr);

                        foreach (DataRow worker in dtStuckWorkers.Rows)
                        {
                            int id = Convert.ToInt32(worker["Id"]);
                            string empWrk = worker["EmployeeWrk"].ToString();
                            DateTime inTime = Convert.ToDateTime(worker["Inpunch_Time"]);

                            int standardHours = worker["WourkHours"] != DBNull.Value ? Convert.ToInt32(worker["WourkHours"]) : 8;
                            string lunchFactor = worker["LunchFactor"] != DBNull.Value ? worker["LunchFactor"].ToString() : "No";

                            DateTime outTime = inTime.AddHours(standardHours);
                            int workedTimeMins = standardHours * 60;
                            decimal workedHoursDec = Convert.ToDecimal(standardHours);

                            using (SqlCommand cmdSP = new SqlCommand("SP_Update_AttendancePunchOUT", dbcl.Conn))
                            {
                                cmdSP.CommandType = CommandType.StoredProcedure;
                                cmdSP.Parameters.AddWithValue("@Id", id);
                                cmdSP.Parameters.AddWithValue("@JOBID", txt_jobid.Text);
                                cmdSP.Parameters.AddWithValue("@SubmitterStatus", "Exit");
                                cmdSP.Parameters.AddWithValue("@EmployeeWrk", empWrk);
                                cmdSP.Parameters.AddWithValue("@Outpunch_Time", outTime);
                                cmdSP.Parameters.AddWithValue("@WorkedTime", workedTimeMins);
                                cmdSP.Parameters.AddWithValue("@WorkedHours", workedHoursDec);
                                cmdSP.Parameters.AddWithValue("@LunchFactor", lunchFactor);
                                cmdSP.Parameters.AddWithValue("@Calc_OT", 0);
                                cmdSP.Parameters.AddWithValue("@ProvidedOT", 0);
                                cmdSP.Parameters.AddWithValue("@LastModified", DateTime.Now);
                                cmdSP.Parameters.AddWithValue("@AttendanceStatus", "Present");
                                cmdSP.Parameters.AddWithValue("@AttendanceCode", "P");
                                cmdSP.ExecuteNonQuery();
                            }
                        }
                    }
                }

                string qryJob = "UPDATE tbl_jobs SET JOBID_Status = 'Active', JOB_Status = 'Out-Punch Done', MasterStatusCode = '4', EntryExit = 'Exit' WHERE JOBID = @JOBID";
                using (SqlCommand cmdJob = new SqlCommand(qryJob, dbcl.Conn))
                {
                    cmdJob.Parameters.AddWithValue("@JOBID", txt_jobid.Text);
                    cmdJob.ExecuteNonQuery();
                }

                ShowNotification("Forced Closed", "All active manpower forcefully clocked out based on standard shift hours. JOB advanced to Approver queue.", "success");
                Load360View(txt_jobid.Text);
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        protected void btn_Act_AdminRollback_Click(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // 1. INITIATE TRANSACTION
                SqlTransaction transaction = dbcl.Conn.BeginTransaction();

                try
                {
                    string adminName = Session["USERNAME"] != null ? Session["USERNAME"].ToString() : "SYSTEM ADMIN";
                    string remark = $"ADMIN OVERRIDE: Approval revoked and returned for corrections by {adminName}.";

                    // 2. Roll the JOBID back to Step 4 (Out-Punch Done)
                    string qryJob = @"UPDATE tbl_jobs 
                              SET Incharge_Approval = 'Returned', 
                                  MasterStatusCode = '4', 
                                  JOB_Status = 'Out-Punch Done', 
                                  Incharge_Remarks = @Remark, 
                                  UpdatedBy = @AdminName, 
                                  UpdatedOn = GETDATE() 
                              WHERE JOBID = @JOBID";

                    using (SqlCommand cmdJob = new SqlCommand(qryJob, dbcl.Conn, transaction))
                    {
                        cmdJob.Parameters.AddWithValue("@JOBID", txt_jobid.Text);
                        cmdJob.Parameters.AddWithValue("@Remark", remark);
                        cmdJob.Parameters.AddWithValue("@AdminName", adminName);
                        cmdJob.ExecuteNonQuery();
                    }

                    // 3. COMMIT TRANSACTION
                    transaction.Commit();
                    JobWorkflowLogger.LogAction(txt_jobid.Text, "ADMIN OVERRIDE: APPROVAL REVOKED", Session["WORKMAN"].ToString(), "Pipeline rolled back from Step 6 to Step 4 for data correction.");

                    ShowNotification("Rollback Successful", "JOBID returned to Supervisor queue.", "success");
                    Load360View(txt_jobid.Text);
                }
                catch (Exception exTransaction)
                {
                    transaction.Rollback();
                    throw new Exception("Rollback failed: " + exTransaction.Message);
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Critical Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        protected void btn_Act_AdminRollback_Click_OLD(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                string qry = @"UPDATE tbl_jobs 
                       SET Incharge_Approval = 'Returned', 
                           Incharge_Remarks = 'ADMIN OVERRIDE: Approval revoked and returned for corrections.',
                           JOB_Status = 'Out-Punch Done', 
                           MasterStatusCode = '4',
                           IsBlocked = 0, 
                           JOBID_Status = 'Active',
                           UpdatedOn = GETDATE(),
                           UpdatedBy = @User
                       WHERE JOBID = @JOBID";

                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", txt_jobid.Text);
                    cmd.Parameters.AddWithValue("@User", Session["USERNAME"] != null ? Session["USERNAME"].ToString() : "ADMIN");
                    cmd.ExecuteNonQuery();
                }

                ShowNotification("Admin Rollback", "JOB approval has been revoked. It has been successfully rolled back to the Supervisor for corrections.", "success");
                Load360View(txt_jobid.Text);
            }
            catch (Exception ex)
            {
                ShowNotification("Rollback Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        // =================================================================================
        // DATE HELPERS
        // =================================================================================
        private DateTime? ParseDateSafe(object dbValue)
        {
            if (dbValue != null && dbValue != DBNull.Value)
            {
                DateTime dt;
                if (DateTime.TryParse(dbValue.ToString(), out dt))
                {
                    return dt;
                }
            }
            return null;
        }
        private string FormatDate(DateTime? dt) => dt.HasValue ? dt.Value.ToString("dd-MMM HH:mm") : "N/A";
        private string GetTATBadge(DateTime? start, DateTime? end, string prefix = "TAT") { if (!start.HasValue || !end.HasValue || start.Value > end.Value) return ""; TimeSpan ts = end.Value - start.Value; string tatString = ts.TotalDays >= 1 ? $"{(int)ts.TotalDays}d {ts.Hours}h {ts.Minutes}m" : ts.TotalHours >= 1 ? $"{ts.Hours}h {ts.Minutes}m" : $"{ts.Minutes}m"; return $"<span class='tat-badge'><i class='fa fa-clock-o'></i> {prefix}: {tatString}</span>"; }

        protected void btn_Act_ViewRawData_Click(object sender, EventArgs e)
        {
            string jobid = txt_jobid.Text.Trim();
            if (string.IsNullOrEmpty(jobid)) return;

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // 1. Load tbl_jobs
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_jobs WHERE JOBID = @JOBID", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dtJobs = new DataTable(); da.Fill(dtJobs);
                        gvRawJobs.DataSource = dtJobs; gvRawJobs.DataBind();
                    }
                }

                // 2. Load tbl_attendance
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_attendance WHERE JOBID = @JOBID ORDER BY Id ASC", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dtAtt = new DataTable(); da.Fill(dtAtt);
                        gvRawAttendance.DataSource = dtAtt; gvRawAttendance.DataBind();
                    }
                }

                // 3. Load tbl_jobspermit (Excluding the raw binary data column to prevent memory crashing)
                string permitQry = @"SELECT Id, JOBID, UploadType, Name, FileType, Extension, TimeStamp, Submitter_Name, 
                                    Submitter_Wrk, ViewStatus, DeleteStatus, DownloadStatus 
                             FROM tbl_jobspermit WHERE JOBID = @JOBID ORDER BY Id ASC";
                using (SqlCommand cmd = new SqlCommand(permitQry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dtPerm = new DataTable(); da.Fill(dtPerm);
                        gvRawPermits.DataSource = dtPerm; gvRawPermits.DataBind();
                    }
                }

                // 4. Load tbl_tbt
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_tbt WHERE Ref_JOBID = @JOBID ORDER BY Id ASC", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dtTBT = new DataTable(); da.Fill(dtTBT);
                        gvRawTBT.DataSource = dtTBT; gvRawTBT.DataBind();
                    }
                }

                // 5. Load tbl_sop
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_sop WHERE Ref_JOBID = @JOBID ORDER BY Id ASC", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dtSOP = new DataTable(); da.Fill(dtSOP);
                        gvRawSOP.DataSource = dtSOP; gvRawSOP.DataBind();
                    }
                }

                // Finally, trigger the modal via JavaScript
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowRawData", "$('#modalRawData').modal('show');", true);
                ShowNotification("Inspector Loaded", "Raw database tables retrieved successfully.", "info");
            }
            catch (Exception ex)
            {
                ShowNotification("Inspector Error", "Failed to load raw data: " + ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        protected void btn_Act_UnblockJob_Click(object sender, EventArgs e)
        {
            // Security Guard: Ensure only Admins can execute this
            if (!IsAdmin())
            {
                ShowNotification("Access Denied", "You do not have permission to perform this override.", "error");
                return;
            }

            //try
            //{
            //    dbcl.Sqlconnection();
            //    dbcl.ConnectDb();

            //    // SQL: Lift the block and set the grace period expiration
            //    string qry = @"UPDATE tbl_jobs 
            //           SET IsBlocked = 0, 
            //               UnblockedUntil = DATEADD(hour, 24, GETDATE()) 
            //           WHERE JOBID = @JOBID";

            //    using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
            //    {
            //        cmd.Parameters.AddWithValue("@JOBID", txt_jobid.Text.Trim());
            //        cmd.ExecuteNonQuery();
            //    }

            //    // AUDIT LOGGING (Using your centralized logger)
            //    JobWorkflowLogger.LogAction(txt_jobid.Text, "ADMIN: UNBLOCK JOB", Session["WORKMAN"].ToString(),
            //        "Granted 24-hour grace period to blocked job. Standard processing re-enabled.");

            //    ShowNotification("Job Unblocked", "The job has been unblocked for the next 24 hours.", "success");

            //    // Refresh the Control Tower view
            //    Load360View(txt_jobid.Text);
            //}
            //catch (Exception ex)
            //{
            //    ShowNotification("Unblock Error", ex.Message, "error");
            //}
            //finally
            //{
            //    dbcl.DisconnectDb();
            //}
        }
    }
}