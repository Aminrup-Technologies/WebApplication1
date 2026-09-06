/*
======================================================================================
File_Name: job_inpunch_v2_aspx_cs
When: April 12, 2026
Why: Updated alongside the V2 UI modernization phase. Business logic, duplicate prevention, and Gatepass validation remain fully preserved. Added a standard try/finally block to the `btn_gtpsedit_Click` method to ensure the SQL connection safely closes even if an error is thrown during Gatepass logging.
What: Preserved existing C# logic behind the modernized `.aspx` presentation layer.
When: 05-Sep-2026
Why: UAT-006 / UAT-021 — restore legacy IN-Punch inbox eligibility so permit-required (ARC, MasterStatusCode='1') jobs appear immediately after create. Duplicate Entry checks and parameterized SQL are unchanged.
When: 05-Sep-2026
Why: UAT-046 / UAT-047 / UAT-048 / UAT-049 — Page_Load catches FormatException from DecodeJobID so invalid jobid tokens do not 500. Decode algorithm and inbox authorization are unchanged.
======================================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class job_inpunch_v2 : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
        DataTable dt = new DataTable();

        // -------------------------------------------------------------------------
        // Thread-Safe ViewState Properties (Replacing old 'static' variables)
        // -------------------------------------------------------------------------
        //private DataTable MailDataTable
        //{
        //    get { return (DataTable)ViewState["MailDataTable"]; }
        //    set { ViewState["MailDataTable"] = value; }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["WORKMAN"] == null)
                {
                    Response.Redirect("~/login.aspx");
                    return;
                }

                InpunchPanel_Row.Visible = true;
                ActiveJOB_Checker();

                //// Initialize Mail Table for HR notifications
                //DataTable dtMail = new DataTable();
                //dtMail.Columns.Add("JobRegion", typeof(string));
                //dtMail.Columns.Add("JobCompany", typeof(string));
                //dtMail.Columns.Add("WorkOrderNo", typeof(string));
                //dtMail.Columns.Add("EmployeeDesignation", typeof(string));
                //dtMail.Columns.Add("EmployeeCategory", typeof(string));
                //MailDataTable = dtMail;

                // SMART ROUTING: Check for Masked JOBID in URL (URL-safe Base64; invalid tokens fail closed)
                if (Request.QueryString["jobid"] != null)
                {
                    string maskedId = Request.QueryString["jobid"].ToString();
                    string realJobId = "";
                    try
                    {
                        realJobId = DecodeJobID(maskedId);
                    }
                    catch (FormatException)
                    {
                        realJobId = "";
                    }

                    if (!string.IsNullOrEmpty(realJobId))
                    {
                        ListItem item = DDL_JOBID.Items.FindByValue(realJobId);
                        if (item != null)
                        {
                            DDL_JOBID.SelectedValue = realJobId;
                            TriggerJobSelection(realJobId);
                        }
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
        // MASKING UTILITIES (URL-Safe Base64)
        // Note: You can move these to DB_Utility_OH4Y.cs for use across all pages
        // =================================================================================
        public static string EncodeJobID(string plainText)
        {
            return JobIdCodec.Encode(plainText);
        }

        public static string DecodeJobID(string maskedData)
        {
            return JobIdCodec.Decode(maskedData);
        }

        // =================================================================================
        // JOB SELECTION & DATA BINDING
        // =================================================================================
        private void ActiveJOB_Checker()
        {
            // Legacy parity with job_inpunch.aspx.cs ActiveJOB_Checker():
            // permit-required (ARC / MasterStatusCode='1') jobs are IN-Punch eligible immediately after create.
            // Do not require MasterStatusCode='3'. Keep the 3-day Active creator inbox and parameterized SQL.
            string query = "SELECT CONCAT(JOBID, ' : ', CONVERT(VARCHAR, CreatedDate, 105)) AS DisplayText, JOBID as ValueField FROM tbl_jobs WHERE [CreatedDate] >= DATEADD(DAY, -3, GETDATE()) AND Creator_Workman=@Workman AND JOBID_Status='Active' ORDER BY CreatedDate DESC";

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

                        // Initialize the staging grid
                        AddDefaultFirstRecord();
                    }
                    else
                    {
                        ShowNotification("Inbox Empty", "NO Active JOB IDs found for IN-Punch.", "notice");
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
            Bind_JOBIDDetails(jobid);
            BindExistingWorkers(jobid);

            // FIX: Wipe the staging grid clean so previous scans don't bleed into the newly selected JOB
            AddDefaultFirstRecord();

            JOBIDDetails_Row.Visible = true;
            WorkmanInput_Row.Visible = true;
            txt_empworkman.Focus();
        }

        private void Bind_JOBIDDetails(string jobid)
        {
            try
            {
                string query = "select * from tbl_jobs where JOBID=@JOBID";
                SqlParameter[] pram = { new SqlParameter("@JOBID", jobid) };
                dt = dbcl.SPreturn_dt(query, pram);

                if (dt.Rows.Count > 0)
                {
                    string jobDateDisplay = Convert.ToDateTime(dt.Rows[0]["CreatedDate"]).ToString("dd-MMM-yyyy");
                    string jobDateValue = Convert.ToDateTime(dt.Rows[0]["CreatedDate"]).ToString("yyyy-MM-dd");

                    lbl_jobiddate.Text = jobDateDisplay;
                    txt_date.Text = jobDateValue;

                    // Lock the HTML5 date picker to only allow this specific date
                    txt_date.Attributes["min"] = jobDateValue;
                    txt_date.Attributes["max"] = jobDateValue;

                    txt_time.Text = DateTime.Now.ToString("HH:mm");

                    lbl_jobcreatorname.Text = dt.Rows[0]["Creator_Name"].ToString();
                    lbl_creatorwrk.Text = dt.Rows[0]["Creator_Workman"].ToString();
                    lbl_creatorregion.Text = dt.Rows[0]["Creator_Region"].ToString();
                    lbl_creatorcompany.Text = dt.Rows[0]["Creator_Company"].ToString();
                    lbl_crtrsitename.Text = dt.Rows[0]["Creator_Site"].ToString();
                    lbl_crtrsitecode.Text = dt.Rows[0]["Creator_SiteCode"].ToString();
                    lbl_wrkordr.Text = dt.Rows[0]["WorkOrderNo"].ToString();
                    lbl_jobid.Text = dt.Rows[0]["JOBID"].ToString();
                    lbl_jobrgn.Text = dt.Rows[0]["JOB_Region"].ToString();
                    lbl_jobcompay.Text = dt.Rows[0]["JOB_Company"].ToString();

                    DT_Binder(lbl_jobrgn.Text, lbl_jobcompay.Text, lbl_wrkordr.Text);

                    lbl_jobsite.Text = dt.Rows[0]["JOB_Site"].ToString();
                    lbl_jobsitecode.Text = dt.Rows[0]["JOB_SiteCode"].ToString();
                    lbl_inchargewrk.Text = dt.Rows[0]["JOB_InchargeWrk"].ToString();
                    lbl_inchargename.Text = dt.Rows[0]["JOB_InchargeName"].ToString();
                    lbl_dept.Text = dt.Rows[0]["JOB_Dept"].ToString();
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
                }
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
        }

        private void BindExistingWorkers(string jobid)
        {
            // Make sure 'Id' is selected in the query!
            string query = "SELECT Id, EmployeeWrk, EmployeeName, Inpunch_Time FROM tbl_attendance WHERE JOBID = @JOBID AND AttendanceStatus = 'Entry' ORDER BY Id ASC";
            SqlParameter[] pram = { new SqlParameter("@JOBID", jobid) };
            DataTable dtExisting = dbcl.SPreturn_dt(query, pram);

            if (dtExisting.Rows.Count > 0)
            {
                gvExistingWorkers.DataSource = dtExisting;
                gvExistingWorkers.DataBind();
                ExistingWorkers_Row.Visible = true;
            }
            else
            {
                ExistingWorkers_Row.Visible = false;
            }
        }

        protected void gvExistingWorkers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvExistingWorkers.DataKeys[e.RowIndex].Value);
            string jobid = lbl_jobid.Text;

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // 1. Delete the specific record
                using (SqlCommand cmd = new SqlCommand("DELETE FROM tbl_attendance WHERE Id = @Id", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }

                // 2. Automatically update the master Job Manpower Count
                string updateCount = "UPDATE tbl_jobs SET ManpowerCount = (SELECT COUNT(*) FROM tbl_attendance WHERE JOBID=@JOBID AND AttendanceStatus='Entry') WHERE JOBID=@JOBID";
                using (SqlCommand cmd = new SqlCommand(updateCount, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    cmd.ExecuteNonQuery();
                }

                dbcl.DisconnectDb();

                ShowNotification("Removed", "Worker has been removed from the active job.", "success");

                // 3. Refresh the grid
                BindExistingWorkers(jobid);
            }
            catch (Exception ex)
            {
                ShowNotification("Error Deleting", ex.Message, "error");
                dbcl.DisconnectDb();
            }
        }

        // =================================================================================
        // WORKMAN SCANNER & VALIDATION LOGIC
        // =================================================================================
        protected void txt_empworkman_TextChanged(object sender, EventArgs e)
        {
            string entered_workman = txt_empworkman.Text.TrimEnd().ToUpper();

            if (string.IsNullOrEmpty(entered_workman)) return;

            if (CheckDuplicateEntry(entered_workman, lbl_jobid.Text))
            {
                ShowNotification("Duplicate", "Employee is already staged in the list below.", "error");
                // NEW: Log the rejection
                JobWorkflowLogger.LogAction(lbl_jobid.Text, "3. IN-PUNCH SCAN (Rejected)", Session["WORKMAN"].ToString(), $"- Workman: {entered_workman}\n- Reason: Already staged in current session.");

                txt_empworkman.Text = "";
                return;
            }

            if (!dbcl.CheckEmployeeActiveStatus(entered_workman))
            {
                ShowNotification("Inactive", "Employee ID is invalid or inactive.", "error");
                // NEW: Log the rejection
                JobWorkflowLogger.LogAction(lbl_jobid.Text, "3. IN-PUNCH SCAN (Rejected)", Session["WORKMAN"].ToString(), $"- Workman: {entered_workman}\n- Reason: ID is invalid or marked as Inactive in Master.");

                ResetScanner();
                return;
            }

            if (CC.Check_EmployeePunchOUT(entered_workman) > 0)
            {
                string pJobID = "", pJobDate = "", pSubmitter = "";
                Pull_PendingOUTDetails(entered_workman, ref pJobID, ref pJobDate, ref pSubmitter);
                ShowNotification("Pending OUT-Punch", $"Worker must punch out of {pJobID} ({pJobDate}) submitted by {pSubmitter}.", "error");

                // NEW: Log the rejection
                JobWorkflowLogger.LogAction(lbl_jobid.Text, "3. IN-PUNCH SCAN (Rejected)", Session["WORKMAN"].ToString(), $"- Workman: {entered_workman}\n- Reason: Pending OUT-Punch on previous job ({pJobID}).");

                ResetScanner();
                return;
            }

            // If all checks pass, load the data
            _EmployeeDataBinder(entered_workman);
        }

        private void Pull_PendingOUTDetails(string workman, ref string pJobID, ref string pJobDate, ref string pSubmitter)
        {
            string query = "select JOBID, CreatedDate, Creator_Name from tbl_attendance where EmployeeWrk=@EmployeeWrk and AttendanceStatus='Entry' and Outpunch_Time is NULL AND (DeleteStatus = 0 OR DeleteStatus IS NULL)";
            SqlParameter[] pram = { new SqlParameter("@EmployeeWrk", workman) };
            DataTable dtOut = dbcl.SPreturn_dt(query, pram);
            if (dtOut.Rows.Count > 0)
            {
                pJobDate = Convert.ToDateTime(dtOut.Rows[0]["CreatedDate"]).ToString("dd-MMM-yyyy");
                pSubmitter = dtOut.Rows[0]["Creator_Name"].ToString();
                pJobID = dtOut.Rows[0]["JOBID"].ToString();
            }
        }

        private void _EmployeeDataBinder(string entered_workman)
        {
            string name = "", workhours = "", worksitename = "", worksitecode = "";
            string category = "", categorycode = "", designation = "", designationcode = "";
            string gpno = "", gpexp = "", sftyno = "", sftyexp = "", pvexp = "";

            dbcl.FindEmployeeDataforInPunch(entered_workman, ref name, ref workhours, ref worksitename, ref worksitecode, ref category, ref categorycode, ref designation, ref designationcode, ref gpno, ref gpexp, ref sftyno, ref sftyexp, ref pvexp);

            string po_skill = "", po_skillcode = "";
            PO_SkillPull(ref designation, ref po_skill, ref po_skillcode);

            if (string.IsNullOrEmpty(po_skill) && string.IsNullOrEmpty(po_skillcode))
            {
                po_skill = category;
                po_skillcode = categorycode;

                // Queue the Skill Mismatch Notification
                string subject = $"PO Skill Mismatch: {entered_workman} on {lbl_jobid.Text}";
                string payload = $"{{\"Designation\":\"{designation}\", \"Category\":\"{category}\", \"WorkOrder\":\"{lbl_wrkordr.Text}\", \"Region\":\"{lbl_jobrgn.Text}\"}}";

                // This will safely insert it ONLY if a pending one doesn't already exist
                QueueNotification("SKILL_MISMATCH", entered_workman, subject, payload);

                ShowNotification("Notice", "PO Skill Mismatch detected. HR has been notified.", "notice");
            }

            txt_empname.Text = name;
            lbl_workhours.Text = workhours;
            txt_worksite.Text = worksitename;
            lbl_worksitecode.Text = worksitecode;
            lbl_category.Text = category;
            lbl_designation.Text = designation;
            lbl_categorycode.Text = categorycode;
            lbl_designationcode.Text = designationcode;
            lbl_gpno.Text = gpno;
            lbl_sftyno.Text = sftyno;
            lbl_pocategoryname.Text = po_skill;
            lbl_pocategorycode.Text = po_skillcode;

            // Date validation logic
            Int32 gpdays = 0;
            FindDaysLeft(gpexp, ref gpdays);
            lbl_gpvalidty.Text = Convert.ToDateTime(gpexp).ToString("dd-MMM-yyyy");

            if (gpdays < 0)
            {
                // Expired Status
                lbl_gpdays.Text = $"Expired ({Math.Abs(gpdays)} days ago)";
                lbl_gpdays.CssClass = "badge bg-red"; // Use Bootstrap badge class

                btn_submit.Visible = false;
                btnShowPopup2.Visible = true;

                txt_nwgpno.Text = gpno;
                txt_nwsftyno.Text = sftyno;
                ShowNotification("Expired", "Gatepass is expired. Please update it.", "error");
            }
            else
            {
                // Active Status
                lbl_gpdays.Text = $"Active ({gpdays} days left)";
                lbl_gpdays.CssClass = "badge bg-green"; // Use Bootstrap badge class

                btn_submit.Visible = true;
                btnShowPopup2.Visible = false;
            }

            EmployeeData_Row.Visible = true;
        }

        // =================================================================================
        // STAGING GRIDVIEW LOGIC
        // =================================================================================
        private void AddDefaultFirstRecord()
        {
            DataTable dtStaging = new DataTable();
            dtStaging.Columns.Add("slno"); dtStaging.Columns.Add("wrk"); dtStaging.Columns.Add("name");
            dtStaging.Columns.Add("wrkhrs"); dtStaging.Columns.Add("category"); dtStaging.Columns.Add("categorycode");
            dtStaging.Columns.Add("designation"); dtStaging.Columns.Add("designationcode");
            dtStaging.Columns.Add("po_category"); dtStaging.Columns.Add("po_categorycode");
            dtStaging.Columns.Add("gpno"); dtStaging.Columns.Add("sftyno");
            dtStaging.Columns.Add("wrksitename"); dtStaging.Columns.Add("wrksitecode"); dtStaging.Columns.Add("in");

            dtStaging.Rows.Add("1", "No Data");
            ViewState["Attendance"] = dtStaging;
            BindMyGridview();
        }

        public void BindMyGridview()
        {
            if (ViewState["Attendance"] != null)
            {
                DataTable dtGrid = (DataTable)ViewState["Attendance"];
                if (dtGrid.Rows.Count > 0 && dtGrid.Rows[0]["wrk"].ToString() != "No Data")
                {
                    GridView1.DataSource = dtGrid;
                    GridView1.DataBind();
                    ViewState_TableRow.Visible = true;
                    SendAttendance_Buttons.Visible = true;
                }
                else
                {
                    ViewState_TableRow.Visible = false;
                    SendAttendance_Buttons.Visible = false;
                }
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_empname.Text) && !string.IsNullOrEmpty(txt_empworkman.Text))
            {
                DataTable dtCurrentTable = (DataTable)ViewState["Attendance"];

                // Clear default row if it exists
                if (dtCurrentTable.Rows.Count == 1 && dtCurrentTable.Rows[0]["wrk"].ToString() == "No Data")
                {
                    dtCurrentTable.Rows[0].Delete();
                    dtCurrentTable.AcceptChanges();
                }

                DataRow dr = dtCurrentTable.NewRow();
                dr["name"] = txt_empname.Text;
                dr["wrk"] = txt_empworkman.Text;
                dr["wrkhrs"] = lbl_workhours.Text;
                dr["category"] = lbl_category.Text;
                dr["categorycode"] = lbl_categorycode.Text;
                dr["designation"] = lbl_designation.Text;
                dr["designationcode"] = lbl_designationcode.Text;
                dr["po_category"] = lbl_pocategoryname.Text;
                dr["po_categorycode"] = lbl_pocategorycode.Text;
                dr["gpno"] = lbl_gpno.Text;
                dr["sftyno"] = lbl_sftyno.Text;
                dr["wrksitename"] = txt_worksite.Text;
                dr["wrksitecode"] = lbl_worksitecode.Text;
                dr["in"] = $"{txt_date.Text} {txt_time.Text}";

                dtCurrentTable.Rows.Add(dr);
                ViewState["Attendance"] = dtCurrentTable;

                BindMyGridview();
                ResetScanner();
                ShowNotification("Added", "Worker staged for IN-Punch.", "success");
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["Attendance"];
            dtCurrentTable.Rows[e.RowIndex].Delete();
            dtCurrentTable.AcceptChanges();

            if (dtCurrentTable.Rows.Count == 0) AddDefaultFirstRecord();
            else BindMyGridview();
        }

        // =================================================================================
        // FINAL DATABASE SUBMISSION
        // =================================================================================
        protected void btn_finalsubmit_Click(object sender, EventArgs e)
        {
            string currentJobId = lbl_jobid.Text.Trim();
            if (string.IsNullOrEmpty(currentJobId)) return;

            int successCount = 0;
            int duplicateCount = 0;

            // NEW: Prepare a string builder to collect log details for the .txt file
            System.Text.StringBuilder logDetails = new System.Text.StringBuilder();

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                DataTable dtFinal = (DataTable)ViewState["Attendance"];

                // 1. Process New Workers (If any exist in the staging grid)
                if (dtFinal != null && dtFinal.Rows.Count > 0 && dtFinal.Rows[0]["wrk"].ToString() != "No Data")
                {
                    foreach (DataRow row in dtFinal.Rows)
                    {
                        string workmanId = row["wrk"].ToString();

                        // THE ULTIMATE SERVER-SIDE CHECK
                        string checkQry = "SELECT COUNT(1) FROM tbl_attendance WHERE JOBID=@JOBID AND EmployeeWrk=@Workman AND AttendanceStatus='Entry'";
                        using (SqlCommand chkCmd = new SqlCommand(checkQry, dbcl.Conn))
                        {
                            chkCmd.Parameters.AddWithValue("@JOBID", currentJobId);
                            chkCmd.Parameters.AddWithValue("@Workman", workmanId);
                            if (Convert.ToInt32(chkCmd.ExecuteScalar()) > 0)
                            {
                                duplicateCount++;
                                // Log the blocked duplicate attempt
                                logDetails.AppendLine($"- [BLOCKED] Duplicate Entry for: {workmanId} ({row["name"]})");
                                continue;
                            }
                        }

                        // Proceed with INSERT
                        using (SqlCommand cmd = new SqlCommand("SP_InsertInto_AttendanceTable", dbcl.Conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(lbl_jobiddate.Text));
                            cmd.Parameters.AddWithValue("@Creator_Name", lbl_jobcreatorname.Text);
                            cmd.Parameters.AddWithValue("@Creator_Workman", lbl_creatorwrk.Text);
                            cmd.Parameters.AddWithValue("@Creator_Region", lbl_creatorregion.Text);
                            cmd.Parameters.AddWithValue("@Creator_Company", lbl_creatorcompany.Text);
                            cmd.Parameters.AddWithValue("@Creator_SiteName", lbl_crtrsitename.Text);
                            cmd.Parameters.AddWithValue("@Creator_SiteCode", lbl_crtrsitecode.Text);
                            cmd.Parameters.AddWithValue("@JOBID", currentJobId);
                            cmd.Parameters.AddWithValue("@WorkOrderNo", lbl_wrkordr.Text);
                            cmd.Parameters.AddWithValue("@PermitNo", lbl_permitno.Text);
                            cmd.Parameters.AddWithValue("@JOB_Region", lbl_jobrgn.Text);
                            cmd.Parameters.AddWithValue("@JOB_Company", lbl_jobcompay.Text);
                            cmd.Parameters.AddWithValue("@JOB_SiteName", lbl_jobsite.Text);
                            cmd.Parameters.AddWithValue("@JOB_SiteCode", lbl_jobsitecode.Text);
                            cmd.Parameters.AddWithValue("@JOB_InchargeWrk", lbl_inchargewrk.Text);
                            cmd.Parameters.AddWithValue("@JOB_InchargeName", lbl_inchargename.Text);
                            cmd.Parameters.AddWithValue("@JOB_Dept", lbl_dept.Text);
                            cmd.Parameters.AddWithValue("@JOB_Location", lbl_jobloc.Text);
                            cmd.Parameters.AddWithValue("@SiteIncharge_Approval", "Pending");
                            cmd.Parameters.AddWithValue("@SubmitterName", Session["USERNAME"].ToString());
                            cmd.Parameters.AddWithValue("@SubmitterWrk", Session["WORKMAN"].ToString());
                            cmd.Parameters.AddWithValue("@SubmitterStatus", "Entry");
                            cmd.Parameters.AddWithValue("@EmployeeName", row["name"].ToString());
                            cmd.Parameters.AddWithValue("@EmployeeWrk", row["wrk"].ToString());
                            cmd.Parameters.AddWithValue("@Employee_Worksite", row["wrksitename"].ToString());
                            cmd.Parameters.AddWithValue("@Employee_WorksiteCode", row["wrksitecode"].ToString());
                            cmd.Parameters.AddWithValue("@WourkHours", row["wrkhrs"].ToString());
                            cmd.Parameters.AddWithValue("@EmpCategory", row["category"].ToString());
                            cmd.Parameters.AddWithValue("@Category_DB", row["categorycode"].ToString());
                            cmd.Parameters.AddWithValue("@PO_SkillCategory", row["po_category"].ToString());
                            cmd.Parameters.AddWithValue("@PO_SkillCategoryCode", row["po_categorycode"].ToString());
                            cmd.Parameters.AddWithValue("@EmpDesignation", row["designation"].ToString());
                            cmd.Parameters.AddWithValue("@Designation_DB", row["designationcode"].ToString());
                            cmd.Parameters.AddWithValue("@GatePassNo", row["gpno"].ToString());
                            cmd.Parameters.AddWithValue("@SafetyPassNo", row["sftyno"].ToString());
                            cmd.Parameters.AddWithValue("@Inpunch_Time", row["in"].ToString());
                            cmd.Parameters.AddWithValue("@AttendanceStatus", "Entry");
                            cmd.Parameters.AddWithValue("@AttendanceCode", "Ab");

                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                successCount++;
                                // Collect successful entry details for the text log
                                logDetails.AppendLine($"- [SUCCESS] IN-Punched: {workmanId} ({row["name"].ToString()}) at {row["in"].ToString()}");
                            }
                        }
                    }
                }

                // 2. HEALING BLOCK: ALWAYS run this to ensure the master table is synced to "Entry"
                using (SqlCommand upd = new SqlCommand("UPDATE tbl_jobs SET JOB_Status='In-Punch Done', MasterStatusCode='3', EntryExit='Entry', ManpowerCount=(SELECT COUNT(1) FROM tbl_attendance WHERE JOBID=@JOBID AND AttendanceStatus='Entry') WHERE JOBID=@JOBID", dbcl.Conn))
                {
                    upd.Parameters.AddWithValue("@JOBID", currentJobId);
                    upd.ExecuteNonQuery();
                }

                // =======================================================
                // NEW: TXT FILE LOGGING (WRITING TO JOBID.txt)
                // =======================================================
                if (successCount > 0 || duplicateCount > 0)
                {
                    logDetails.Insert(0, $"Processed IN-Punch batch. Success: {successCount}, Duplicates: {duplicateCount}\n");
                    JobWorkflowLogger.LogAction(currentJobId, "IN-PUNCH BATCH PROCESSING", Session["WORKMAN"].ToString(), logDetails.ToString());
                }
                else
                {
                    JobWorkflowLogger.LogAction(currentJobId, "IN-PUNCH FINALIZATION", Session["WORKMAN"].ToString(), "Master status synced to 'In-Punch Done'. No new workers added in this transaction.");
                }
                // =======================================================

                // 3. UI Feedback
                if (successCount > 0)
                {
                    string msg = $"{successCount} workers successfully IN-Punched!";
                    if (duplicateCount > 0) msg += $" ({duplicateCount} duplicates blocked).";
                    ShowNotification("Success", msg, "success");
                }
                else if (duplicateCount > 0)
                {
                    ShowNotification("Notice", $"No new workers added. {duplicateCount} duplicates blocked.", "notice");
                }
                else
                {
                    ShowNotification("Finalized", "Job status synced and finalized.", "info");
                }

                // 4. Cleanup and Refresh UI
                BindExistingWorkers(currentJobId);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hideLoader", "hideLoader();", true);

                AddDefaultFirstRecord();
                ResetScanner();
                InpunchPanel_Row.Visible = false; // Hide dropdown to confirm lock-in
            }
            catch (Exception ex)
            {
                ShowNotification("System Error", ex.Message, "error");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hideLoader", "hideLoader();", true);

                // Optional: Log errors to the TXT file too!
                JobWorkflowLogger.LogAction(currentJobId, "IN-PUNCH ERROR", Session["WORKMAN"].ToString(), $"Exception: {ex.Message}");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        // =================================================================================
        // HELPERS
        // =================================================================================
        protected bool CheckDuplicateEntry(string workman, string jobid)
        {
            // 1. Check Staging ViewState (Has the supervisor just scanned them?)
            DataTable dtStaging = (DataTable)ViewState["Attendance"];
            if (dtStaging != null)
            {
                foreach (DataRow row in dtStaging.Rows)
                {
                    if (row["wrk"].ToString() == workman) return true;
                }
            }

            // 2. Check Database (Are they already punched in from a previous visit to this page?)
            string query = "SELECT COUNT(1) FROM tbl_attendance WHERE JOBID = @JOBID AND EmployeeWrk = @Workman AND AttendanceStatus = 'Entry'";

            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
            {
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                cmd.Parameters.AddWithValue("@Workman", workman);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                dbcl.DisconnectDb();

                if (count > 0) return true;
            }

            return false;
        }

        private void ResetScanner()
        {
            txt_empworkman.Text = "";
            txt_empworkman.Focus();
            EmployeeData_Row.Visible = false;
        }

        protected void btn_reset_Click(object sender, EventArgs e) { ResetScanner(); }

        public void FindDaysLeft(string expiry_date, ref Int32 day)
        {
            DateTime expDate; // Declare the variable first for older C# versions
            if (DateTime.TryParse(expiry_date, out expDate))
            {
                TimeSpan duration = expDate.Subtract(DateTime.Now);
                day = duration.Days;
            }
        }

        private void DT_Binder(string rgn, string comp, string wrkordr)
        {
            string cmdString = "Select Designation_Type, Designation_DB, WO_Category_Type, WO_Category_Code from tlb_WO_SkillCategory where Work_Region_Code=@rgn and Company_Code=@comp and WO_Number=@wo order by Id";
            SqlParameter[] p = { new SqlParameter("@rgn", rgn), new SqlParameter("@comp", comp), new SqlParameter("@wo", wrkordr) };
            ViewState["dt1"] = dbcl.SPreturn_dt(cmdString, p);
        }

        private void PO_SkillPull(ref string designation, ref string po_skill, ref string po_skillcode)
        {
            DataTable dtSkills = (DataTable)ViewState["dt1"];
            if (dtSkills != null)
            {
                foreach (DataRow row in dtSkills.Rows)
                {
                    if (designation == row["Designation_Type"].ToString())
                    {
                        po_skill = row["WO_Category_Type"].ToString();
                        po_skillcode = row["WO_Category_Code"].ToString();
                        break;
                    }
                }
            }
        }

        static string DataTableToHtml(DataTable dataTable)
        {
            string htmlTable = "<html><body><div><p>Dear ATS Team, Please note that one of your ATS Portal Users has created a JOBID, Where they are trying to add manpower with the Designation and Skill Category mentioned below, but finding issues as the manpower Skill Category is NOT mapped with P.O. or Workorder Skill Category. Kindly map the following for further smooth processing.</p></div><br/><table width='100%' style='border-collapse:collapse;'>";
            htmlTable += "<tr>";
            foreach (DataColumn column in dataTable.Columns)
            {
                htmlTable += "<th style='border:1px solid #595959;'>" + column.ColumnName + "</th>";
            }
            htmlTable += "</tr>";

            foreach (DataRow row in dataTable.Rows)
            {
                htmlTable += "<tr>";
                foreach (var item in row.ItemArray)
                {
                    htmlTable += "<td style='border:1px solid #595959;'>" + item.ToString() + "</td>";
                }
                htmlTable += "</tr>";
            }
            htmlTable += "</table><br/></body></html>";
            return htmlTable;
        }

        protected void btn_gtpsedit_Click(object sender, EventArgs e)
        {
            string workman = txt_empworkman.Text.Trim();
            Dictionary<string, string> original = new Dictionary<string, string>();
            List<string> changes = new List<string>();

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // 1. Fetch Original Values for Comparison
                using (SqlCommand sel = new SqlCommand("SELECT GatePassNo, CONVERT(varchar, GatePassExpiry, 23) as GatePassExpiry, SafetyPassNo, CONVERT(varchar, SafetyPassExpiry, 23) as SafetyPassExpiry, CONVERT(varchar, PVExpiry, 23) as PVExpiry FROM tbl_Employee_Mustertable WHERE WorkmanSL=@WorkmanSL", dbcl.Conn))
                {
                    sel.Parameters.AddWithValue("@WorkmanSL", workman);
                    using (SqlDataReader rdr = sel.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            original["GatePassNo"] = rdr["GatePassNo"].ToString();
                            original["GatePassExpiry"] = rdr["GatePassExpiry"].ToString();
                            original["SafetyPassNo"] = rdr["SafetyPassNo"].ToString();
                            original["SafetyPassExpiry"] = rdr["SafetyPassExpiry"].ToString();
                            original["PVExpiry"] = rdr["PVExpiry"].ToString();
                        }
                    }
                }

                // 2. Compare Values
                CompareValue(changes, original, "GatePassNo", txt_nwgpno.Text);
                CompareValue(changes, original, "GatePassExpiry", txt_nwgpvalidity.Text);
                CompareValue(changes, original, "SafetyPassNo", txt_nwsftyno.Text);
                CompareValue(changes, original, "SafetyPassExpiry", txt_nwsftyvalidity.Text);
                CompareValue(changes, original, "PVExpiry", txt_nwpvvalidity.Text);

                if (changes.Count > 0)
                {
                    string changesStr = string.Join(" | ", changes);

                    // 3. Write physical log (Your existing employee master log)
                    string auditDetail = string.Format("Reason: Gatepass Update from Site{0}CHANGES DETECTED:{0}{1}", Environment.NewLine, changesStr);
                    LogAudit(workman, "MASTER_UPDATE", auditDetail);

                    // =======================================================
                    // NEW: TXT FILE LOGGING (GATEPASS OVERRIDE ON JOB)
                    // =======================================================
                    JobWorkflowLogger.LogAction(lbl_jobid.Text, "3. IN-PUNCH (Gatepass Override)", Session["WORKMAN"].ToString(), $"- Workman: {workman}\n- Changes: {changesStr}");
                    // =======================================================

                    // 4. Update Database
                    string CmdString = "UPDATE tbl_Employee_Mustertable SET GatePassNo=@GatePassNo, GatePassExpiry=@GatePassExpiry, SafetyPassNo=@SafetyPassNo, SafetyPassExpiry=@SafetyPassExpiry, PVExpiry=@PVExpiry, GP_ModifierWrk=@GP_ModifierWrk, GP_ModifierName=@GP_ModifierName, GP_ModifiedDate=@GP_ModifiedDate, GP_UpdateApproval=@GP_UpdateApproval WHERE WorkmanSL=@WorkmanSL";
                    using (SqlCommand cmd = new SqlCommand(CmdString, dbcl.Conn))
                    {
                        cmd.Parameters.AddWithValue("@WorkmanSL", workman);
                        cmd.Parameters.AddWithValue("@GatePassNo", txt_nwgpno.Text);
                        cmd.Parameters.AddWithValue("@GatePassExpiry", string.IsNullOrEmpty(txt_nwgpvalidity.Text) ? (object)DBNull.Value : txt_nwgpvalidity.Text);
                        cmd.Parameters.AddWithValue("@SafetyPassNo", txt_nwsftyno.Text);
                        cmd.Parameters.AddWithValue("@SafetyPassExpiry", string.IsNullOrEmpty(txt_nwsftyvalidity.Text) ? (object)DBNull.Value : txt_nwsftyvalidity.Text);
                        cmd.Parameters.AddWithValue("@PVExpiry", string.IsNullOrEmpty(txt_nwpvvalidity.Text) ? (object)DBNull.Value : txt_nwpvvalidity.Text);
                        cmd.Parameters.AddWithValue("@GP_ModifierWrk", Session["WORKMAN"].ToString());
                        cmd.Parameters.AddWithValue("@GP_ModifierName", Session["USERNAME"].ToString());
                        cmd.Parameters.AddWithValue("@GP_ModifiedDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                        cmd.Parameters.AddWithValue("@GP_UpdateApproval", "Pending");
                        cmd.ExecuteNonQuery();
                    }

                    // 5. Queue Notification for HR Admin
                    string subj = $"Gatepass Approval Required: {workman}";
                    string payload = $"{{\"Modifier\":\"{Session["USERNAME"]}\", \"Changes\":\"{changesStr}\"}}";
                    QueueNotification("GP_UPDATE", workman, subj, payload);
                }

                _EmployeeDataBinder(workman);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", "$('#myModal2').modal('hide');", true);
                ShowNotification("Updated", "Gatepass details updated and sent for HR approval.", "success");
            }
            catch (Exception ex)
            {
                ShowNotification("Error Updating Gatepass", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb(); // Ensures connection is closed even if an exception occurs
            }
        }


        // =================================================================================
        // AUDIT & NOTIFICATION QUEUE HELPERS
        // =================================================================================

        private void CompareValue(List<string> changes, Dictionary<string, string> original, string key, string currentValue)
        {
            string oldValue = original.ContainsKey(key) ? (original[key] ?? "").Trim() : "";
            string newValue = (currentValue ?? "").Trim();

            if (oldValue != newValue)
            {
                changes.Add(string.Format("{0}: '{1}' -> '{2}'", key, oldValue, newValue));
            }
        }

        private void LogAudit(string emp, string type, string details)
        {
            try
            {
                string logPath = Server.MapPath("~/bussiness/production/Logs/EmployeeEdits/");
                if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);

                string line = string.Format("{0:yyyy-MM-dd HH:mm:ss} | {1} | {2} | By: {3}{4}", DateTime.Now, type, details, Session["USERNAME"], Environment.NewLine);
                File.AppendAllText(Path.Combine(logPath, "EmpLog_" + emp + ".txt"), line);
            }
            catch { /* Silently fail if file is locked */ }
        }

        private void QueueNotification(string templateCode, string workman, string subject, string payload)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            try
            {
                // 1. Duplicate Prevention: Check if a Pending notification with the same Subject & Workman already exists
                string checkQry = "SELECT COUNT(1) FROM NotificationQueue WHERE EmployeeID=@Emp AND Subject=@Sub AND Status='Pending'";
                using (SqlCommand chkCmd = new SqlCommand(checkQry, dbcl.Conn))
                {
                    chkCmd.Parameters.AddWithValue("@Emp", workman);
                    chkCmd.Parameters.AddWithValue("@Sub", subject);
                    int exists = Convert.ToInt32(chkCmd.ExecuteScalar());

                    if (exists > 0) return; // Duplicate blocked!
                }

                // 2. Fetch Template ID
                int templateId = 0;
                using (SqlCommand tplCmd = new SqlCommand("SELECT TemplateID FROM NotificationTemplates WHERE TemplateCode=@Code", dbcl.Conn))
                {
                    tplCmd.Parameters.AddWithValue("@Code", templateCode);
                    object res = tplCmd.ExecuteScalar();
                    if (res != null) templateId = Convert.ToInt32(res);
                }

                // 3. Insert into Queue
                string insQry = "INSERT INTO NotificationQueue (NotificationID, EmployeeID, Channel, Payload, Status, Subject) VALUES (@Tpl, @Emp, 'System', @Payload, 'Pending', @Sub)";
                using (SqlCommand cmd = new SqlCommand(insQry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Tpl", templateId > 0 ? (object)templateId : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Emp", workman);
                    cmd.Parameters.AddWithValue("@Payload", payload);
                    cmd.Parameters.AddWithValue("@Sub", subject);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }
        /*
        ======================================================================
        When: May 05, 2026
        Why: Captures user feedback from the modal, logs it to the database, and redirects them to the old V1 page. Includes a silent fail mechanism to ensure workflow isn't blocked if logging fails.
        What: Insert_Version_Switch_Log_Method
        ======================================================================
        */
        protected void btn_confirm_switch_Click(object sender, EventArgs e)
        {
            string reason = DDL_SwitchReason.SelectedValue;
            string remarks = txt_switch_remarks.Text.Trim();
            string workman = Session["WORKMAN"] != null ? Session["WORKMAN"].ToString() : "Unknown";

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                string qry = "INSERT INTO tbl_Version_Switch_Log (Workman_ID, Switch_Reason, Remarks, Page_From) VALUES (@Workman, @Reason, @Remarks, @Page)";
                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Workman", workman);
                    cmd.Parameters.AddWithValue("@Reason", reason);
                    cmd.Parameters.AddWithValue("@Remarks", string.IsNullOrEmpty(remarks) ? (object)DBNull.Value : remarks);

                    // THE FIX: Changed to the correct page name!
                    cmd.Parameters.AddWithValue("@Page", "job_inpunch_v2.aspx");
                    cmd.ExecuteNonQuery();
                }

                // =======================================================
                // NEW: TXT FILE LOGGING (ABANDONED V2)
                // =======================================================
                JobWorkflowLogger.LogAction(lbl_jobid.Text, "UI DOWNGRADE", workman, $"User abandoned V2 IN-Punch and switched to V1.\n- Reason: {reason}\n- Remarks: {remarks}");
            }
            catch
            {
                // Fail silently
            }
            finally
            {
                dbcl.DisconnectDb();
            }

            // Redirect to the old version immediately after logging
            // Ensure you change this to the old In-Punch page, not the create page!
            Response.Redirect("job_inpunch.aspx", false);
        }
    }
}