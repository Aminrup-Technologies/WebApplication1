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
                if (Session["USERID"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null)
                {
                    Response.Redirect("~/login.aspx", false);
                    return;
                }
                txt_jobid.Focus();
            }
        }

        private void ShowNotification(string title, string message, string type)
        {
            if (string.IsNullOrEmpty(message)) message = "An unknown error occurred.";
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
                            lbl_creator.Text = row["Creator_Name"].ToString() + " [" + row["Creator_Workman"].ToString() + "]";
                            lbl_worksite.Text = row["JOB_Site"].ToString();
                            lbl_title.Text = row["JOB_Title"].ToString();
                            lbl_shift.Text = row["JOB_Shift"].ToString();
                            lbl_wo.Text = row["WorkOrderNo"].ToString();
                            lbl_filecount.Text = row["FileCount"].ToString();
                            lbl_jobstatus.Text = row["JOB_Status"].ToString();

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
                ShowNotification("Database Error", ex.Message, "error");
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

        private void LoadManpower(string jobid)
        {
            string query = "SELECT EmployeeName, EmployeeWrk, EmpDesignation, Inpunch_Time, Outpunch_Time, TimeStamp, LastModified, WorkedHours, Calc_OT, ProvidedOT, AttendanceStatus, AttendanceCode FROM tbl_attendance WHERE JOBID = @JOBID ORDER BY Inpunch_Time ASC";
            using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
            {
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable(); da.Fill(dt);
                    gvManpower.DataSource = dt; gvManpower.DataBind();
                }
            }
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

        // =================================================================================
        // PIPELINE EVALUATION ENGINE
        // =================================================================================
        // =================================================================================
        // PIPELINE EVALUATION ENGINE
        // =================================================================================
        private void EvaluateSmartLifecycle(DataRow row, string jobid, DateTime dtMasterJobSysCreation)
        {
            step1.Attributes["class"] = "stepper-item"; step2.Attributes["class"] = "stepper-item";
            step3.Attributes["class"] = "stepper-item"; step4.Attributes["class"] = "stepper-item";
            step5.Attributes["class"] = "stepper-item"; step6.Attributes["class"] = "stepper-item";
            lit_step1_details.Text = lit_step2_details.Text = lit_step3_details.Text = lit_step4_details.Text = lit_step5_details.Text = lit_step6_details.Text = "";
            divBottleneck.Attributes["class"] = "alert alert-warning text-dark font-weight-bold";

            string permitUploadReq = row["PermitUpload"].ToString();
            string csmReq = row["CSM_Documents"].ToString();
            string entryExitStatus = row["EntryExit"].ToString();
            string inchargeApproval = row["Incharge_Approval"].ToString();

            int fileCount = row["FileCount"] != DBNull.Value ? Convert.ToInt32(row["FileCount"]) : 0;
            int tbtCount = row["TBT_Count"] != DBNull.Value ? Convert.ToInt32(row["TBT_Count"]) : 0;
            int sopCount = row["SOP_Count"] != DBNull.Value ? Convert.ToInt32(row["SOP_Count"]) : 0;

            string creatorName = row["Creator_Name"].ToString();
            string inchargeName = row["JOB_InchargeName"].ToString();

            DateTime? dtPermit = ParseDateSafe(row["PermitUploadDate"]);
            DateTime? dtApproval = ParseDateSafe(row["Incharge_ApprovalDate"]);

            DateTime? dtFirstInPunchLogic = null;
            DateTime? dtFirstInPunchSys = null;
            DateTime? dtLastOutPunchUpdated = null;
            int actualWorkerCount = 0; // Added variable to track true manpower

            // Added COUNT(Id) to explicitly verify if workers exist
            string attQuery = "SELECT MIN(Inpunch_Time), MIN(TimeStamp), MAX(LastModified), COUNT(Id) FROM tbl_attendance WHERE JOBID=@JOBID";
            using (SqlCommand cmd = new SqlCommand(attQuery, dbcl.Conn))
            {
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        dtFirstInPunchLogic = ParseDateSafe(rdr[0]);
                        dtFirstInPunchSys = ParseDateSafe(rdr[1]);
                        dtLastOutPunchUpdated = ParseDateSafe(rdr[2]);
                        actualWorkerCount = rdr[3] != DBNull.Value ? Convert.ToInt32(rdr[3]) : 0;
                    }
                }
            }

            int currentStep = 1;

            // --- STEP 1: CREATE ---
            step1.Attributes["class"] = "stepper-item completed";
            lit_step1_details.Text = $"<div class='step-details'><strong>By:</strong> {creatorName}<br/><strong>System Log:</strong> {FormatDate(dtMasterJobSysCreation)}</div>";
            currentStep = 2;

            // --- STEP 2: PERMIT UPLOAD ---
            if (permitUploadReq != "Yes")
            {
                step2.Attributes["class"] = "stepper-item skipped";
                lit_step2_details.Text = "<div class='step-details text-muted'><em>Skipped (Not Req.)</em></div>";
                currentStep = 3;
            }
            else
            {
                if (fileCount > 0 || row["FinalUpldStatus"].ToString() == "Yes")
                {
                    step2.Attributes["class"] = "stepper-item completed";
                    lit_step2_details.Text = $"<div class='step-details'><strong>Status:</strong> Uploaded<br/><strong>System Log:</strong> {FormatDate(dtPermit)}<br/>{GetTATBadge(dtMasterJobSysCreation, dtPermit)}</div>";
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
                    // Shift has actual workers in it!
                    step3.Attributes["class"] = "stepper-item completed";
                    lit_step3_details.Text = $"<div class='step-details'><strong>Log:</strong> {actualWorkerCount} Workers Scanned<br/><strong>System Log:</strong> {FormatDate(dtFirstInPunchSys)}<br/>{GetTATBadge(dtPermit ?? dtMasterJobSysCreation, dtFirstInPunchSys)}</div>";
                    currentStep = 4;
                }
                else
                {
                    step3.Attributes["class"] = "stepper-item active";

                    // SMART CATCH: Supervisor marked it 'Done' but added 0 workers!
                    if (row["JOB_Status"].ToString() == "In-Punch Done" || entryExitStatus == "Entry")
                    {
                        divBottleneck.Attributes["class"] = "alert alert-danger text-white font-weight-bold";
                        lbl_bottleneck.Text = "EMPTY SHIFT DETECTED: Supervisor opened the shift but scanned 0 workers. Admin must IN-Punch manpower or Delete the job.";
                    }
                    else
                    {
                        lbl_bottleneck.Text = $"Job is active. Ready for Manpower IN-Punch scanning.";
                    }
                }
            }

            // --- STEP 4: SITE DOCS ---
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
                        lit_step4_details.Text = $"<div class='step-details'><strong>Status:</strong> Docs Captured<br/><strong>By:</strong> Site Team</div>";
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
                    lit_step5_details.Text = $"<div class='step-details'><strong>Log:</strong> Shift Closed<br/><strong>System Log:</strong> {FormatDate(validOutTime)}<br/>{GetTATBadge(dtFirstInPunchSys, validOutTime, "Shift Execution")}</div>";
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
                    lit_step6_details.Text = $"<div class='step-details'><strong>By:</strong> {inchargeName}<br/><strong>System Log:</strong> {FormatDate(dtApproval)}<br/>{GetTATBadge(dtLastOutPunchUpdated ?? dtFirstInPunchSys, dtApproval)}</div>";
                    divBottleneck.Attributes["class"] = "alert alert-success text-dark font-weight-bold";
                    lbl_bottleneck.Text = "Job Lifecycle Completed and Approved Successfully.";
                }
                else if (inchargeApproval == "Rejected" || inchargeApproval == "Returned")
                {
                    step6.Attributes["class"] = "stepper-item failed";
                    divBottleneck.Attributes["class"] = "alert alert-danger text-white font-weight-bold";
                    lbl_bottleneck.Text = $"Job was {inchargeApproval.ToUpper()} by Final Approver: {inchargeName}.";
                }
                else
                {
                    step6.Attributes["class"] = "stepper-item active";
                    lbl_bottleneck.Text = $"Awaiting Final Approval & Verification from: {inchargeName}";
                }
            }

            bool isBlocked = row["IsBlocked"] != DBNull.Value && Convert.ToBoolean(row["IsBlocked"]);
            if (isBlocked)
            {
                divBottleneck.Attributes["class"] = "alert alert-danger text-white font-weight-bold";
                lbl_bottleneck.Text = "SYSTEM ALERT: This JOBID has been administratively BLOCKED.";
            }

            // TRIGGER ACTION BAR MATRIX
            ActionBarRow.Visible = true;
            EvaluateActionMatrix(row, currentStep, isBlocked, dtFirstInPunchLogic, ParseDateSafe(row["CreatedDate"]));
        }

        // =================================================================================
        // ACTION MATRIX CONTROLLER
        // =================================================================================
        private void EvaluateActionMatrix(DataRow row, int pipelineStep, bool isBlocked, DateTime? dtFirstInPunchLogic, DateTime? dtCreatedDate)
        {
            // 1. Hide all buttons initially
            btn_Act_UploadPermit.Visible = false; btn_Act_InPunch.Visible = false; btn_Act_AddDocs.Visible = false;
            btn_Act_OutPunch.Visible = false; btn_Act_Unblock.Visible = false; btn_Act_Resubmit.Visible = false;
            btn_Act_ForceOut.Visible = false; btn_Act_SwapDate.Visible = false; btn_Act_Delete.Visible = false;

            string approvalStatus = row["Incharge_Approval"].ToString();
            string entryExitStatus = row["EntryExit"].ToString();

            // Safely parse the DeleteStatus
            bool isDeleted = row["DeleteStatus"] != DBNull.Value &&
                             (row["DeleteStatus"].ToString() == "1" || row["DeleteStatus"].ToString().ToLower() == "true");

            // ----------------------------------------------------------------------
            // 2. THE NEW GOLDEN RULE: If Approved OR Deleted, NO actions are allowed!
            // ----------------------------------------------------------------------
            if (isDeleted || row["JOBID_Status"].ToString() == "Deleted")
            {
                divBottleneck.Attributes["class"] = "alert alert-danger text-white font-weight-bold";
                lbl_bottleneck.Text = $"<i class='fa fa-trash'></i> ARCHIVED RECORD: This JOB was DELETED on {FormatDate(ParseDateSafe(row["DeleteOn"]))} by {row["DeletedBy"]}. No further actions allowed.";
                return; // Immediately halt the matrix!
            }

            if (approvalStatus == "Approved")
            {
                return; // Approved jobs are locked.
            }

            // 3. UNBLOCK PRIORITY
            if ((isBlocked || row["JOBID_Status"].ToString() == "Blocked") && entryExitStatus != "Exit")
            {
                btn_Act_Unblock.Visible = true;
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
                    btn_Act_ForceOut.Visible = true;
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

        // =================================================================================
        // ACTION BUTTON EXECUTORS
        // =================================================================================
        protected void btn_Act_UploadPermit_Click(object sender, EventArgs e) { Response.Redirect($"job_permitupload_v2.aspx?jobid={txt_jobid.Text}", false); }
        protected void btn_Act_InPunch_Click(object sender, EventArgs e) { Response.Redirect($"job_inpunch_v2.aspx?jobid={txt_jobid.Text}", false); }
        protected void btn_Act_OutPunch_Click(object sender, EventArgs e) { Response.Redirect($"job_outpunch_v2.aspx?jobid={txt_jobid.Text}", false); }
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
        // DELETE JOB (Safe Soft-Delete with Total State Flatline)
        // =================================================================================
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
                dbcl.Sqlconnection(); dbcl.ConnectDb();
                //string qry = @"UPDATE tbl_jobs SET Incharge_Approval = 'Pending', Incharge_Remarks = 'Resubmitted by Admin for Correction', EntryExit = 'Entry', JOB_Status = 'In-Punch Done', MasterStatusCode = '3' WHERE JOBID = @JOBID";

                string qry = @"UPDATE tbl_jobs 
                   SET Incharge_Approval = 'Pending', 
                       Incharge_Remarks = 'Resubmitted by Admin for Correction',
                       EntryExit = 'Entry',
                       JOB_Status = 'In-Punch Done', 
                       MasterStatusCode = '3',
                       IsBlocked = 0, 
                       JOBID_Status = 'Active',
                       BlockedTimestamp = NULL,
                       UnblockedUntil = DATEADD(HOUR, 24, GETDATE())                  
                   WHERE JOBID = @JOBID";
                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", txt_jobid.Text);
                    cmd.ExecuteNonQuery();
                }
                ShowNotification("Resubmitted", "JOB has been rolled back to the Entry phase. The Supervisor can now edit it.", "success");
                Load360View(txt_jobid.Text);
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        protected void btn_Act_ForceOut_Click(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection(); dbcl.ConnectDb();

                string getStuckWorkersQry = "SELECT Id, EmployeeWrk, Inpunch_Time, WourkHours, LunchFactor FROM tbl_attendance WHERE JOBID = @JOBID AND Outpunch_Time IS NULL";
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

        // =================================================================================
        // DATE HELPERS
        // =================================================================================
        private DateTime? ParseDateSafe(object dbValue)
        {
            if (dbValue != null && dbValue != DBNull.Value)
            {
                DateTime dt; // Declare variable first for C# 5.0 compatibility
                if (DateTime.TryParse(dbValue.ToString(), out dt))
                {
                    return dt;
                }
            }
            return null;
        }
        private string FormatDate(DateTime? dt) => dt.HasValue ? dt.Value.ToString("dd-MMM HH:mm") : "N/A";
        private string GetTATBadge(DateTime? start, DateTime? end, string prefix = "TAT") { if (!start.HasValue || !end.HasValue || start.Value > end.Value) return ""; TimeSpan ts = end.Value - start.Value; string tatString = ts.TotalDays >= 1 ? $"{(int)ts.TotalDays}d {ts.Hours}h {ts.Minutes}m" : ts.TotalHours >= 1 ? $"{ts.Hours}h {ts.Minutes}m" : $"{ts.Minutes}m"; return $"<span class='tat-badge'><i class='fa fa-clock-o'></i> {prefix}: {tatString}</span>"; }
    }
}