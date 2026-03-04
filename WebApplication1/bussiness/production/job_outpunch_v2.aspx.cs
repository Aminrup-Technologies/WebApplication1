using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class job_outpunch_v2 : System.Web.UI.Page
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
                    Response.Redirect("~/login.aspx", false);
                    return;
                }

                Bind_AttendnaceCode();
                OUTpunchPanel_Row.Visible = true;
                ActiveJOB_Checker();

                // SMART ROUTING: Check for Masked JOBID in URL
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

        // =================================================================================
        // URL MASKING UTILITIES
        // =================================================================================
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
        // INITIALIZATION & BINDING
        // =================================================================================
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
            // Bypass the old CountChecker (CC) class and align perfectly with the Smart Dashboard!
            // We explicitly check for MasterStatusCode='3' and EntryExit='Entry'
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
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["FinalUpldStatus"].ToString() == "Yes";
                }
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
                    lbl_jobcreatorname.Text = dt.Rows[0]["Creator_Name"].ToString();
                    lbl_creatorwrk.Text = dt.Rows[0]["Creator_Workman"].ToString();
                    lbl_creatorregion.Text = dt.Rows[0]["Creator_Region"].ToString();
                    lbl_creatorcompany.Text = dt.Rows[0]["Creator_Company"].ToString();
                    lbl_wrkordr.Text = dt.Rows[0]["WorkOrderNo"].ToString();
                    lbl_jobid.Text = dt.Rows[0]["JOBID"].ToString();
                    lbl_jobsite.Text = dt.Rows[0]["JOB_Site"].ToString();
                    lbl_inchargename.Text = dt.Rows[0]["JOB_InchargeName"].ToString();
                    lbl_jobloc.Text = dt.Rows[0]["JOB_Location"].ToString();
                    lbl_jobshift.Text = dt.Rows[0]["JOB_Shift"].ToString();
                    lbl_permitno.Text = dt.Rows[0]["JOB_PermitNo"].ToString();

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

        // =================================================================================
        // OUT-PUNCH CORE LOGIC
        // =================================================================================
        private void CheckPendingOUT()
        {
            string ddljobid = DDL_JOBID.SelectedValue;
            string supv = Session["WORKMAN"].ToString();

            if (CC.CheckforPendingOUT(ddljobid, supv) == 0)
            {
                ViewState_TableRow.Visible = false;
                NoPenidngPunch.Visible = true;

                // Update Master Status Code to 4 (Exit)
                string jobStatus = CC.CheckforPendingPermit(ddljobid, supv) == 0 ? "Blocked" : "Active";
                UpdateJOBTable1(jobStatus, "Out-Punch Done", "4", "Exit");
            }
            else
            {
                NoPenidngPunch.Visible = false;
                ViewState_TableRow.Visible = true;
                Bind_GridView(ddljobid);
            }
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
            string cmdString = "Select * from tbl_attendance where Creator_Workman=@Workman and JOBID=@JOBID and AttendanceStatus='Entry' order by Id";
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
            int id = Convert.ToInt32((sender as Button).CommandArgument);

            // Hide the grid, show the form
            ViewState_TableRow.Visible = false;
            PunchOutForm_Row.Visible = true;

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand cmd = new SqlCommand("Select Id, EmployeeName, EmployeeWrk, WourkHours, Inpunch_Time from tbl_attendance where Id=@Id", dbcl.Conn))
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
                        }
                    }
                }
                dbcl.DisconnectDb();

                // Pre-fill current date/time for convenience
                txt_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txt_time.Text = DateTime.Now.ToString("HH:mm");
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = ((Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id")).Text;
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand cmd = new SqlCommand("delete from tbl_attendance where Id=@Id", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
                dbcl.DisconnectDb();
                ShowNotification("Deleted", "Record deleted successfully.", "success");
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }

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

            // Check allowed duration
            if (!CanPunchOutWithin32Hours(intime))
            {
                ShowNotification("Timeout Error", "Only 24/32 Hour back entry is allowed from IN-Time.", "error");
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
                dbcl.DisconnectDb();
                return true;
            }
            catch (Exception ex)
            {
                ShowNotification("Database Error", ex.Message, "error");
                return false;
            }
        }

        public bool CanPunchOutWithin32Hours(string emp_intime)
        {
            DateTime intm = DateTime.Parse(emp_intime);
            DateTime outm = DateTime.Now;

            string configDuration = ConfigurationManager.AppSettings["PunchOutDurationMinutes"];
            int punchOutDurationMinutes = string.IsNullOrEmpty(configDuration) ? 1920 : int.Parse(configDuration); // Default to 32 hours (1920 mins)

            TimeSpan duration = outm - intm;
            return duration.TotalMinutes <= punchOutDurationMinutes;
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            PunchOutForm_Row.Visible = false;
            CheckPendingOUT();
        }
    }
}