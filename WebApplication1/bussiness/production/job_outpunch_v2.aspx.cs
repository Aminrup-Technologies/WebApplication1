using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;

namespace WebApplication1.bussiness.production
{
    public partial class job_outpunch_v2 : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
        DataTable dt = new DataTable();

        // Single static HttpClient for optimal connection pooling
        private static readonly HttpClient httpClient = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["WORKMAN"] == null)
                {
                    Response.Redirect("~/login.aspx", false);
                    return;
                }

                Bind_AttendnaceCode();
                OUTpunchPanel_Row.Visible = true;
                ActiveJOB_Checker();

                if (Request.QueryString["jobid"] != null)
                {
                    string maskedId = Request.QueryString["jobid"].ToString();
                    string realJobId = DecodeJobID(maskedId);

                    ListItem item = DDL_JOBID.Items.FindByValue(realJobId);
                    if (item != null)
                    {
                        DDL_JOBID.SelectedValue = realJobId;
                        TriggerJobSelection(realJobId);
                    }
                }
            }
        }

        private void ShowNotification(string title, string message, string type)
        {
            string script = $"showPNotify('{title}', '{message.Replace("'", "\\'")}', '{type}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PNotify", script, true);
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

        private void Bind_AttendnaceCode()
        {
            string query = "Select Status_Name, Status_Code from tlb_attendancecodes where Approver='Yes' and Status='Present' order by slno";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            using (SqlCommand Cmd = new SqlCommand(query, dbcl.Conn))
            {
                DDL_AttenCode.DataSource = Cmd.ExecuteReader();
                DDL_AttenCode.DataTextField = "Status_Name";
                DDL_AttenCode.DataValueField = "Status_Code";
                DDL_AttenCode.DataBind();
                DDL_AttenCode.Items.Insert(0, new ListItem("Please Select Option", ""));
            }
            dbcl.DisconnectDb();
        }

        private void ActiveJOB_Checker()
        {
            string query = "SELECT CONCAT(JOBID, ' : ', CONVERT(VARCHAR, CreatedDate, 105)) AS DisplayText, JOBID as ValueField FROM tbl_jobs WHERE [CreatedDate] >= DATEADD(DAY, -3, GETDATE()) AND Creator_Workman=@Workman AND JOBID_Status='Active' AND MasterStatusCode='3' AND EntryExit='Entry' ORDER BY CreatedDate DESC";

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
                        DDL_JOBID.Items.Insert(0, new ListItem("--Select Active Job--", ""));
                    }
                    else
                    {
                        ShowNotification("Inbox Empty", "NO Active JOB IDs require Out-Punch right now.", "notice");
                    }
                }
            }
            dbcl.DisconnectDb();
        }

        protected void DDL_JOBID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string jobid = DDL_JOBID.SelectedValue;
            if (!string.IsNullOrEmpty(jobid)) TriggerJobSelection(jobid);
        }

        private void TriggerJobSelection(string jobid)
        {
            string jobdate = "";
            string displayTxt = DDL_JOBID.SelectedItem.Text;
            string[] parts = displayTxt.Split(new string[] { " : " }, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                string[] dateParts = parts[1].Split('-');
                if (dateParts.Length == 3) jobdate = $"{dateParts[2]}-{dateParts[1]}-{dateParts[0]}";
            }

            if (Pull_PermitStatus(jobid, jobdate))
            {
                Bind_JOBIDDetails(jobid, jobdate);
                JOBIDDetails_Row.Visible = true;
            }
            else
            {
                ShowNotification("Blocked", "Permit NOT Uploaded against the selected JOBID!", "error");
            }
        }

        private Boolean Pull_PermitStatus(string jobid, string jobdate)
        {
            try
            {
                string query = "select FinalUpldStatus from tbl_jobs where JOBID=@JOBID and CreatedDate=@CreatedDate and JOBID_Status='Active' and EntryExit='Entry'";
                SqlParameter[] pram = { new SqlParameter("@JOBID", jobid), new SqlParameter("@CreatedDate", jobdate) };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0) return dt.Rows[0]["FinalUpldStatus"].ToString() == "Yes";
            }
            catch (Exception) { return false; }
            return false;
        }

        private void Bind_JOBIDDetails(string jobid, string jobdate)
        {
            try
            {
                string query = "select * from tbl_jobs where JOBID=@JOBID and CreatedDate=@CreatedDate";
                SqlParameter[] pram = { new SqlParameter("@JOBID", jobid), new SqlParameter("@CreatedDate", jobdate) };
                dt = dbcl.SPreturn_dt(query, pram);

                if (dt.Rows.Count > 0)
                {
                    lbl_jobiddate.Text = Convert.ToDateTime(dt.Rows[0]["CreatedDate"]).ToString("dd-MMM-yyyy");

                    // 1. Fetch the Job Date from your database (usually CreatedDate)
                    DateTime jobDate = Convert.ToDateTime(dt.Rows[0]["CreatedDate"]);

                    // 2. Calculate the Day Before
                    DateTime dayBefore = jobDate.AddDays(-1);

                    // 3. Convert to yyyy-MM-dd format required by HTML5 <input type="date">
                    string maxDateValue = jobDate.ToString("yyyy-MM-dd");
                    string minDateValue = dayBefore.ToString("yyyy-MM-dd");

                    // 4. Bind the default text (usually defaults to the Job Date)
                    txt_date.Text = maxDateValue;

                    // 5. Lock the HTML5 date picker to only allow this 2-day window
                    txt_date.Attributes["min"] = minDateValue;
                    txt_date.Attributes["max"] = maxDateValue;

                    lbl_jobcreatorname.Text = dt.Rows[0]["Creator_Name"].ToString();
                    lbl_creatorwrk.Text = dt.Rows[0]["Creator_Workman"].ToString();
                    lbl_creatorregion.Text = dt.Rows[0]["Creator_Region"].ToString();
                    lbl_creatorcompany.Text = dt.Rows[0]["Creator_Company"].ToString();
                    lbl_wrkordr.Text = dt.Rows[0]["WorkOrderNo"].ToString();
                    lbl_jobid.Text = dt.Rows[0]["JOBID"].ToString();
                    lbl_jobsite.Text = dt.Rows[0]["JOB_Site"].ToString();
                    lbl_inchargename.Text = dt.Rows[0]["JOB_InchargeName"].ToString();
                    lbl_inchargewrk.Text = dt.Rows[0]["JOB_InchargeWrk"].ToString(); // Safely keep incharge wrk
                    lbl_jobloc.Text = dt.Rows[0]["JOB_Location"].ToString();
                    lbl_jobshift.Text = dt.Rows[0]["JOB_Shift"].ToString();
                    lbl_permitno.Text = dt.Rows[0]["JOB_PermitNo"].ToString();

                    // =======================================================
                    // NEW: MAP GPS BINDING LOGIC
                    // =======================================================
                    string dbLat = dt.Rows[0]["GPS_Latitude"] != DBNull.Value ? dt.Rows[0]["GPS_Latitude"].ToString() : "";
                    string dbLon = dt.Rows[0]["GPS_Longitude"] != DBNull.Value ? dt.Rows[0]["GPS_Longitude"].ToString() : "";

                    if (!string.IsNullOrEmpty(dbLat) && !string.IsNullOrEmpty(dbLon))
                    {
                        hf_db_lat.Value = dbLat;
                        hf_db_lon.Value = dbLon;
                        div_saved_map.Style["display"] = "block"; // Unhide the map container

                        // Trigger JS to draw the map.
                        // We use a 300ms timeout to ensure the UpdatePanel has finished rendering the unhidden DIV first.
                        ScriptManager.RegisterStartupScript(this, GetType(), "DrawSavedMap", "setTimeout(renderSavedMap, 300);", true);
                    }
                    else
                    {
                        // Hide it if no GPS data is found for this JOBID
                        div_saved_map.Style["display"] = "none";
                        hf_db_lat.Value = "";
                        hf_db_lon.Value = "";
                    }
                    // =======================================================

                    if (dt.Rows[0]["CSM_Documents"].ToString() == "Yes")
                    {
                        CSMRow.Visible = true;
                        CSM_Documents_CountbyJOBID(jobid, jobdate, lbl_creatorwrk.Text, lbl_creatorregion.Text, lbl_creatorcompany.Text);
                    }
                    else
                    {
                        CSMRow.Visible = false;
                        CheckPendingOUT();
                    }
                }
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
        }

        private void CSM_Documents_CountbyJOBID(string jobid, string jobdate, string crtrwrk, string crtrrgn, string crtrcomp)
        {
            try
            {
                string formattedDate = DateTime.Parse(jobdate).ToString("yyyy-MM-dd");
                string query = "select TBT_Count, SOP_Count from tbl_jobs where CreatedDate=@CreatedDate and Creator_Workman=@Creator_Workman and Creator_Region=@Creator_Region and Creator_Company=@Creator_Company and JOBID=@JOBID";
                SqlParameter[] pram = {
                    new SqlParameter("@JOBID", jobid),
                    new SqlParameter("@CreatedDate", formattedDate),
                    new SqlParameter("@Creator_Workman", crtrwrk),
                    new SqlParameter("@Creator_Region", crtrrgn),
                    new SqlParameter("@Creator_Company", crtrcomp)
                };

                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    Int32 tbtcount = Convert.ToInt32(dt.Rows[0]["TBT_Count"]);
                    Int32 sopcount = Convert.ToInt32(dt.Rows[0]["SOP_Count"]);
                    lbl_tbtcount.Text = tbtcount.ToString();
                    lbl_sopcount.Text = sopcount.ToString();

                    if (tbtcount > 0 && sopcount > 0)
                    {
                        IncompleteCSM.Visible = false;
                        CheckPendingOUT();
                    }
                    else
                    {
                        IncompleteCSM.Visible = true;
                        ViewState_TableRow.Visible = false;
                    }
                }
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
        }

        private void CheckPendingOUT()
        {
            string ddljobid = DDL_JOBID.SelectedValue;
            string supv = Session["WORKMAN"].ToString();

            ViewState_TableRow.Visible = true;
            Bind_GridView(ddljobid);

            if (CC.CheckforPendingOUT(ddljobid, supv) == 0) NoPenidngPunch.Visible = true;
            else NoPenidngPunch.Visible = false;
        }

        private void UpdateJOBTable1(string jobidstatus, string jobstatus, string mastercode, string entryexitstatus)
        {
            try
            {
                string CmdString = "UPDATE tbl_jobs set JOBID_Status=@JOBID_Status, JOB_Status=@JOB_Status, MasterStatusCode=@MasterStatusCode, EntryExit=@EntryExit where JOBID=@JOBID";
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand cmd = new SqlCommand(CmdString, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", lbl_jobid.Text);
                    cmd.Parameters.AddWithValue("@JOBID_Status", jobidstatus);
                    cmd.Parameters.AddWithValue("@JOB_Status", jobstatus);
                    cmd.Parameters.AddWithValue("@MasterStatusCode", mastercode);
                    cmd.Parameters.AddWithValue("@EntryExit", entryexitstatus);
                    cmd.ExecuteNonQuery();
                }
                dbcl.DisconnectDb();
            }
            catch (Exception ex) { ShowNotification("Error Closing Job", ex.Message, "error"); }
        }

        private void Bind_GridView(string jobid)
        {
            string cmdString = "Select * from tbl_attendance where Creator_Workman=@Workman and JOBID=@JOBID order by Id";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            using (SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn))
            {
                cmd.Parameters.AddWithValue("@Workman", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dtbl = new DataTable();
                ad.Fill(dtbl);
                GridView1.DataSource = dtbl;
                GridView1.DataBind();
            }
            dbcl.DisconnectDb();
        }

        protected void PunchOUT_Click(object sender, EventArgs e)
        {
            // CORRECTED: Explicitly cast to LinkButton to access CommandArgument
            int id = Convert.ToInt32(((LinkButton)sender).CommandArgument);

            ViewState_TableRow.Visible = false;
            PunchOutForm_Row.Visible = true;

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand cmd = new SqlCommand("Select Id, EmployeeName, EmployeeWrk, WourkHours, Inpunch_Time, Outpunch_Time from tbl_attendance where Id=@Id", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.Read())
                        {
                            lbl_Id.Text = sdr["Id"].ToString();
                            txt_empname.Text = sdr["EmployeeName"].ToString();
                            lbl_empworkman.Text = sdr["EmployeeWrk"].ToString();
                            lbl_workhours.Text = sdr["WourkHours"].ToString();
                            txt_inpunchtime.Text = sdr["Inpunch_Time"].ToString();

                            if (sdr["Outpunch_Time"] != DBNull.Value)
                            {
                                DateTime existingOut = Convert.ToDateTime(sdr["Outpunch_Time"]);
                                txt_date.Text = existingOut.ToString("yyyy-MM-dd");
                                txt_time.Text = existingOut.ToString("HH:mm");
                            }
                            else
                            {
                                txt_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
                                txt_time.Text = DateTime.Now.ToString("HH:mm");
                            }
                        }
                    }
                }
                dbcl.DisconnectDb();
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = ((Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id")).Text;
            string jobid = lbl_jobid.Text;

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // =======================================================
                // NEW: FETCH DETAILS BEFORE DELETING FOR THE AUDIT LOG
                // =======================================================
                string deletedWorker = "Unknown";
                using (SqlCommand fetchCmd = new SqlCommand("SELECT EmployeeWrk, EmployeeName FROM tbl_attendance WHERE Id=@Id", dbcl.Conn))
                {
                    fetchCmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader rdr = fetchCmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            deletedWorker = $"{rdr["EmployeeWrk"]} ({rdr["EmployeeName"]})";
                        }
                    }
                }
                // =======================================================

                using (SqlCommand cmd = new SqlCommand("delete from tbl_attendance where Id=@Id", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
                dbcl.DisconnectDb();

                // =======================================================
                // NEW: TXT FILE LOGGING (DESTRUCTIVE ACTION)
                // =======================================================
                JobWorkflowLogger.LogAction(jobid, "4. OUT-PUNCH (Record Deleted)", Session["WORKMAN"].ToString(), $"- Supervisor completely removed IN-Punch record for: {deletedWorker}");
                // =======================================================

                ShowNotification("Deleted", "Record deleted successfully.", "success");
            }
            catch (Exception ex)
            {
                ShowNotification("Error", ex.Message, "error");
            }

            CheckPendingOUT();
        }

        protected void btn_punchout_Click(object sender, EventArgs e)
        {
            if (UpdateAttendanceTable())
            {
                PunchOutForm_Row.Visible = false;
                ShowNotification("Success", $"{txt_empname.Text} has been punched out.", "success");
                CheckPendingOUT();
            }
        }

        private Boolean UpdateAttendanceTable()
        {
            string outtime = $"{txt_date.Text.Trim()} {txt_time.Text}";
            string intime = txt_inpunchtime.Text;
            DateTime dtout = DateTime.Parse(outtime);

            if (!CanPunchOutWithin32Hours(intime))
            {
                ShowNotification("Timeout Error", "Only 24/32 Hour back entry is allowed from IN-Time.", "error");

                // =======================================================
                // NEW: TXT FILE LOGGING (BLOCKED ACTION)
                // =======================================================
                JobWorkflowLogger.LogAction(DDL_JOBID.SelectedValue, "4. OUT-PUNCH (Blocked)", Session["WORKMAN"].ToString(), $"- Workman: {lbl_empworkman.Text}\n- Reason: Exceeded 24/32 hour timeout limit.\n- Attempted Out-Time: {outtime}");
                // =======================================================

                return false;
            }

            string dbid = lbl_Id.Text;
            Int32 emp_wrkhours = Convert.ToInt32(lbl_workhours.Text);
            Int32 emp_wrkmnis = emp_wrkhours * 60;
            string lunchyesno = RBTN_LunchFactor.SelectedValue;

            Int32 workedmins = 0;
            decimal workedhours = .0m;
            dbcl.FindEmployeeWorkedTime(intime, outtime, ref workedmins, ref workedhours);

            decimal emp_calOThrs = .0m;
            if (Session["REGION"].ToString() == "NINL") dbcl.CalculateOvertimeRev(emp_wrkmnis, workedmins, lunchyesno, ref emp_calOThrs);
            else dbcl.CalculateOvertime(emp_wrkmnis, workedmins, lunchyesno, ref emp_calOThrs);

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand cmd = new SqlCommand("SP_Update_AttendancePunchOUT", dbcl.Conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", dbid);
                    cmd.Parameters.AddWithValue("@JOBID", DDL_JOBID.SelectedValue);
                    cmd.Parameters.AddWithValue("@SubmitterStatus", "Exit");
                    cmd.Parameters.AddWithValue("@EmployeeWrk", lbl_empworkman.Text);
                    cmd.Parameters.AddWithValue("@Outpunch_Time", dtout);
                    cmd.Parameters.AddWithValue("@LunchFactor", lunchyesno);
                    cmd.Parameters.AddWithValue("@WorkedTime", workedmins);
                    cmd.Parameters.AddWithValue("@WorkedHours", workedhours);
                    cmd.Parameters.AddWithValue("@Calc_OT", emp_calOThrs);
                    cmd.Parameters.AddWithValue("@ProvidedOT", txt_ot.Text);
                    cmd.Parameters.AddWithValue("@LastModified", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                    cmd.Parameters.AddWithValue("@AttendanceStatus", "Exit");
                    cmd.Parameters.AddWithValue("@AttendanceCode", DDL_AttenCode.SelectedValue);
                    cmd.ExecuteNonQuery();
                }

                // =======================================================
                // NEW: TXT FILE LOGGING (INDIVIDUAL OUT-PUNCH)
                // =======================================================
                string logDetails = $"- [OUT-PUNCH] Workman: {lbl_empworkman.Text} ({txt_empname.Text})\n" +
                                    $"- Out-Time      : {dtout.ToString("yyyy-MM-dd HH:mm")}\n" +
                                    $"- Worked Hours  : {workedhours} (Lunch: {lunchyesno})\n" +
                                    $"- Calc OT       : {emp_calOThrs} | Provided OT: {txt_ot.Text}\n" +
                                    $"- Attn Code     : {DDL_AttenCode.SelectedValue}";

                JobWorkflowLogger.LogAction(DDL_JOBID.SelectedValue, "4. OUT-PUNCH (Individual)", Session["WORKMAN"].ToString(), logDetails);
                // =======================================================

                dbcl.DisconnectDb();
                return true;
            }
            catch (Exception ex)
            {
                ShowNotification("Database Error", ex.Message, "error");

                // Optional: Log errors to the TXT file
                JobWorkflowLogger.LogAction(DDL_JOBID.SelectedValue, "OUT-PUNCH ERROR", Session["WORKMAN"].ToString(), $"Failed for {lbl_empworkman.Text}. Exception: {ex.Message}");

                return false;
            }
        }

        public bool CanPunchOutWithin32Hours(string emp_intime)
        {
            DateTime intm = DateTime.Parse(emp_intime);
            DateTime outm = DateTime.Now;
            string configDuration = ConfigurationManager.AppSettings["PunchOutDurationMinutes"];
            int punchOutDurationMinutes = string.IsNullOrEmpty(configDuration) ? 1920 : int.Parse(configDuration);
            TimeSpan duration = outm - intm;
            return duration.TotalMinutes <= punchOutDurationMinutes;
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            PunchOutForm_Row.Visible = false;
            CheckPendingOUT();
        }

        // =================================================================================
        // FINALIZATION & NOTIFICATION TRIGGER
        // =================================================================================
        protected void btn_FinalizeShift_Click(object sender, EventArgs e)
        {
            string ddljobid = DDL_JOBID.SelectedValue;
            if (string.IsNullOrEmpty(ddljobid)) return;

            string supv = Session["WORKMAN"].ToString();

            // Double check that no one is missing an out-punch before sealing
            if (CC.CheckforPendingOUT(ddljobid, supv) == 0)
            {
                // 1. Update Master Status Code to 4 (Exit)
                string jobStatus = CC.CheckforPendingPermit(ddljobid, supv) == 0 ? "Blocked" : "Active";
                UpdateJOBTable1(jobStatus, "Out-Punch Done", "4", "Exit");

                // =======================================================
                // NEW: TXT FILE LOGGING (SHIFT CLOSURE)
                // =======================================================
                JobWorkflowLogger.LogAction(ddljobid, "5. SHIFT CLOSED & FINALIZED", supv, "All workers successfully out-punched. Master Status Code updated to 4 (Exit). Job routed to Site Approver.");
                // =======================================================

                // 2. Fire and Forget Notifications (Does not block the UI)
                Task.Run(() => TriggerShiftClosureNotifications(ddljobid));

                // 3. Update UI Immediately
                ShowNotification("Shift Closed", "Shift has been successfully finalized and sent to the Approver.", "success");
                ViewState_TableRow.Visible = false;
                NoPenidngPunch.Visible = false;
                ActiveJOB_Checker();
            }
            else
            {
                ShowNotification("Warning", "You still have missing OUT-Punches. Please complete them first.", "warning");
            }
        }

        // =================================================================================
        // THE NOTIFICATION ENGINE
        // =================================================================================
        private async Task TriggerShiftClosureNotifications(string jobid)
        {
            DB_Utility_OH4Y asyncDbcl = new DB_Utility_OH4Y(); // Use isolated connection for background task
            try
            {
                asyncDbcl.Sqlconnection();
                asyncDbcl.ConnectDb();

                // 1. Fetch Job Information
                string jobDateStr = "", shift = "", supv = "", wo = "", permit = "", title = "", site = "", inchargeName = "", inchargeWrk = "", dept = "", loc = "";
                string qryJob = "SELECT CreatedDate, JOB_Shift, Creator_Name, Creator_Workman, WorkOrderNo, JOB_PermitNo, JOB_Title, JOB_Site, JOB_SiteCode, JOB_InchargeName, JOB_InchargeWrk, JOB_Dept, JOB_Location FROM tbl_jobs WHERE JOBID=@JOBID";

                using (SqlCommand cmd = new SqlCommand(qryJob, asyncDbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            DateTime dtJob = Convert.ToDateTime(rdr["CreatedDate"]);
                            jobDateStr = dtJob.ToString("dd-MM-yyyy") + " (" + dtJob.DayOfWeek.ToString() + ")";
                            shift = rdr["JOB_Shift"].ToString();
                            supv = rdr["Creator_Name"].ToString() + " (" + rdr["Creator_Workman"].ToString() + ")";
                            wo = rdr["WorkOrderNo"].ToString();
                            permit = rdr["JOB_PermitNo"].ToString().Trim();
                            title = rdr["JOB_Title"].ToString().Trim();
                            site = rdr["JOB_Site"].ToString() + " [" + rdr["JOB_SiteCode"].ToString() + "]";
                            inchargeName = rdr["JOB_InchargeName"].ToString() + " (" + rdr["JOB_InchargeWrk"].ToString() + ")";
                            inchargeWrk = rdr["JOB_InchargeWrk"].ToString();
                            dept = rdr["JOB_Dept"].ToString();
                            loc = rdr["JOB_Location"].ToString();
                        }
                    }
                }

                // 2. Fetch Contact Information Safely
                string mobileNo = "", emailAddress = "";
                using (SqlCommand cmd = new SqlCommand("SELECT MobileNo, Email FROM tbl_Employee_Mustertable WHERE WorkmanSL = @Wrk", asyncDbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Wrk", inchargeWrk);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            mobileNo = rdr["MobileNo"] != DBNull.Value ? rdr["MobileNo"].ToString().Trim() : "";
                            emailAddress = rdr["Email"] != DBNull.Value ? rdr["Email"].ToString().Trim() : "";
                        }
                    }
                }

                // 3. Build Manpower List (Filters Soft Deletes!)
                List<string> manpowerItems = new List<string>();
                using (SqlCommand cmd = new SqlCommand("SELECT EmployeeName, EmployeeWrk, EmpDesignation FROM tbl_attendance WHERE JOBID=@JOBID AND DeleteStatus=0 ORDER BY Id ASC", asyncDbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        int count = 1;
                        while (rdr.Read())
                        {
                            manpowerItems.Add($"{count}. {rdr["EmployeeName"]} [{rdr["EmployeeWrk"]}] - {rdr["EmpDesignation"]}");
                            count++;
                        }
                    }
                }

                asyncDbcl.DisconnectDb();

                // 🌟 FIX: WhatsApp STRICTLY forbids newlines (\n) in variables. We must use a comma separator.
                string waManpowerList = manpowerItems.Count > 0 ? string.Join(",  ", manpowerItems) : "No Manpower Scanned.";

                // 🌟 FIX: Emails DO support newlines via HTML <br/> tags.
                string emailManpowerList = manpowerItems.Count > 0 ? string.Join("<br/>", manpowerItems) : "No Manpower Scanned.";

                // 4. FIRE APIS
                if (!string.IsNullOrEmpty(mobileNo) && mobileNo.Length >= 10)
                {
                    // Pass the WhatsApp-Safe list (waManpowerList)
                    await SendWhatsAppMsg91Async(mobileNo, inchargeName, supv, jobid, shift, jobDateStr, title, site, loc, dept, wo, permit, waManpowerList);
                }
                else
                {
                    LogSystemEvent("ShareAPI", "SKIPPED", $"No valid Mobile Number found for In-Charge {inchargeWrk}");
                }

                if (!string.IsNullOrEmpty(emailAddress) && emailAddress.Contains("@"))
                {
                    // Pass the Email-Safe list (emailManpowerList)
                    SendEmailAlert(emailAddress, inchargeName, supv, jobid, shift, jobDateStr, title, site, loc, dept, wo, permit, emailManpowerList);
                }
                else
                {
                    LogSystemEvent("ShareAPI", "SKIPPED", $"No valid Email ID found for In-Charge {inchargeWrk}");
                }
            }
            catch (Exception ex)
            {
                LogSystemEvent("Engine", "CRASH", "Critical failure inside Notification Engine.", ex.Message);
            }
            finally
            {
                if (asyncDbcl.Conn != null && asyncDbcl.Conn.State == ConnectionState.Open) asyncDbcl.DisconnectDb();
            }
        }

        private async Task SendWhatsAppMsg91Async(string mobileNo, string inchargeName, string supv, string jobid, string shift, string jobDateStr, string title, string site, string loc, string dept, string wo, string permit, string manpowerList)
        {
            try
            {
                string authKey = ConfigurationManager.AppSettings["Msg91AuthKey"]?.Trim();
                string integratedNumber = ConfigurationManager.AppSettings["Msg91IntegratedNumber"]?.Trim();

                if (string.IsNullOrEmpty(authKey) || string.IsNullOrEmpty(integratedNumber))
                {
                    LogSystemEvent("WhatsApp", "SKIPPED", "MSG91 AuthKey or IntegratedNumber missing in web.config.");
                    return;
                }

                string cleanPhone = mobileNo.Replace("+", "").Replace(" ", "").Trim();
                if (!cleanPhone.StartsWith("91")) cleanPhone = "91" + cleanPhone;

                string url = "https://api.msg91.com/api/v5/whatsapp/whatsapp-outbound-message/bulk/";

                var payload = new
                {
                    integrated_number = integratedNumber,
                    content_type = "template",
                    payload = new
                    {
                        messaging_product = "whatsapp",
                        type = "template",
                        template = new
                        {
                            name = "job_daily_details",
                            language = new { code = "en", policy = "deterministic" },
                            @namespace = "af05507b_02e4_4d95_8f8c_164ce03fc2df",
                            to_and_components = new[]
                            {
                                new
                                {
                                    to = new[] { cleanPhone },
                                    components = new Dictionary<string, object>
                                    {
                                        { "body_1", new { type = "text", value = inchargeName } },
                                        { "body_2", new { type = "text", value = supv } },
                                        { "body_3", new { type = "text", value = jobid } },
                                        { "body_4", new { type = "text", value = shift } },
                                        { "body_5", new { type = "text", value = jobDateStr } },
                                        { "body_6", new { type = "text", value = title } },
                                        { "body_7", new { type = "text", value = site } },
                                        { "body_8", new { type = "text", value = loc } },
                                        { "body_9", new { type = "text", value = dept } },
                                        { "body_10", new { type = "text", value = wo } },
                                        { "body_11", new { type = "text", value = permit } },
                                        { "body_12", new { type = "text", value = manpowerList } }
                                    }
                                }
                            }
                        }
                    }
                };

                string jsonPayload = new JavaScriptSerializer().Serialize(payload).Replace("\"@namespace\"", "\"namespace\"");
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("authkey", authKey);

                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    LogSystemEvent("WhatsApp", "SUCCESS", $"Message sent successfully to {cleanPhone}.", result);
                }
                else
                {
                    LogSystemEvent("WhatsApp", "API_ERROR", $"MSG91 rejected the request for {cleanPhone}.", result);
                }
            }
            catch (Exception ex)
            {
                LogSystemEvent("WhatsApp", "CRASH", "Code Exception in SendWhatsAppMsg91Async.", ex.Message);
            }
        }

        private void SendEmailAlert(string emailAddress, string inchargeName, string supv, string jobid, string shift, string jobDateStr, string title, string site, string loc, string dept, string wo, string permit, string htmlManpowerList)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(emailAddress);
                mail.From = new MailAddress("it.support@aminruptechnologies.co.in", "Work-Sure 360");
                mail.Subject = $"ACTION REQUIRED: Shift Closure Approval for JOB: {jobid}";
                mail.IsBodyHtml = true;

                string htmlBody = $@"
                    <div style='font-family: Arial, sans-serif; color: #333;'>
                        <h2 style='color: #E74C3C;'>Action Required: Shift Closure Pending</h2>
                        <p>Dear <b>{inchargeName}</b>,</p>
                        <p>Please be informed that the following job has been completed and OUT-Punched by <b>{supv}</b>. It is now in your queue for Final Approval to generate the Invoice Memo.</p>
                        
                        <table style='width: 100%; border-collapse: collapse; margin-top: 15px;'>
                            <tr><td style='padding: 5px;'><b>JOB ID:</b></td><td>{jobid} ({shift} Shift)</td></tr>
                            <tr><td style='padding: 5px;'><b>Date:</b></td><td>{jobDateStr}</td></tr>
                            <tr><td style='padding: 5px;'><b>Job Title:</b></td><td>{title}</td></tr>
                            <tr><td style='padding: 5px;'><b>Worksite:</b></td><td>{site} | {loc}</td></tr>
                            <tr><td style='padding: 5px;'><b>Department:</b></td><td>{dept}</td></tr>
                            <tr><td style='padding: 5px;'><b>WO / Permit:</b></td><td>{wo} | {permit}</td></tr>
                        </table>

                        <h4 style='margin-top:20px; border-bottom: 1px solid #ddd; padding-bottom: 5px;'>Scanned Manpower</h4>
                        <p>{htmlManpowerList}</p>

                        <br/><br/>
                        <a href='https://atswork.co.in/' style='background-color:#1ABB9C; color:#fff; text-decoration:none; padding:10px 20px; border-radius:5px; font-weight:bold; display: inline-block;'>Open ATS Portal to Approve</a>
                        
                        <p style='margin-top: 40px; font-size: 11px; color: #888;'>This is an automated message generated by Aminrup Technologies on behalf of ATS. Please do not reply to this email.</p>
                    </div>";

                mail.Body = htmlBody;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.zoho.in"; // Ensure your SMTP Host is configured here
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                string smtpUser = System.Configuration.ConfigurationManager.AppSettings["SmtpUser"] ?? "";
                string smtpPass = System.Configuration.ConfigurationManager.AppSettings["SmtpPass"] ?? "";

                if (string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPass))
                {
                    LogSystemEvent("Email", "SKIP", $"SMTP not configured - approval email to {emailAddress} skipped.");
                    return;
                }

                smtp.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass);
                smtp.EnableSsl = true;

                smtp.Send(mail);
                System.Diagnostics.Debug.WriteLine($"[LOG - SUCCESS] Email sent successfully to {emailAddress}");
            }
            catch (Exception ex)
            {
                LogSystemEvent("Email", "CRASH", $"SMTP Failed for {emailAddress}.", ex.Message);
            }
        }

        // =================================================================================
        // DAILY TEXT FILE LOGGER
        // =================================================================================
        private void LogSystemEvent(string module, string status, string message, string details = "")
        {
            try
            {
                // 1. Define the directory: ~/Logs/OutPunch/YYYY-MM-DD/
                string dateFolder = DateTime.Now.ToString("yyyy-MM-dd");
                string logDirectory = Server.MapPath($"~/Logs/OutPunch/{dateFolder}/");

                // 2. Create the directory if it doesn't exist
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // 3. Define the file name
                string filePath = Path.Combine(logDirectory, "Notification_Log.txt");

                // 4. Format the log entry
                string logEntry = $"[{DateTime.Now:HH:mm:ss}] [{module}] [{status}] - {message}";
                if (!string.IsNullOrEmpty(details))
                {
                    logEntry += $" | Details: {details}";
                }
                logEntry += Environment.NewLine;

                // 5. Append to the file
                File.AppendAllText(filePath, logEntry);
            }
            catch
            {
                // Failsafe: If the server denies write permissions, fail silently so the app doesn't crash
            }
        }
    }
}