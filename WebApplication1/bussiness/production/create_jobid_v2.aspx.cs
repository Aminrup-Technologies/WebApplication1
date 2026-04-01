/*
======================================================================================
File_Name: create_jobid_v2_aspx_cs
When: March 30, 2026
Why: To fix ADO.NET parameter type mismatches ensuring seamless integration with the revised UAT database schema and preventing implicit SQL conversion overhead.
What: Updated AddWithValue parameters in Insert_JOBData() to use strong native data types (int for FileCount, DateTime for CreatedDate) mapping precisely to the SP_InsertInto_JOBSTable SQL signature.
======================================================================================
*/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class create_jobid_v2 : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        DataTable dt = new DataTable();

        // -------------------------------------------------------------------------
        // DB Controller ViewState Properties (V2 Smart Variables)
        // -------------------------------------------------------------------------
        private string DB_WOType { get { return ViewState["DB_WOType"] as string ?? ""; } set { ViewState["DB_WOType"] = value; } }
        private string DB_WOWorkRegion { get { return ViewState["DB_WOWorkRegion"] as string ?? ""; } set { ViewState["DB_WOWorkRegion"] = value; } }
        private string DB_WOCompnayCode { get { return ViewState["DB_WOCompnayCode"] as string ?? ""; } set { ViewState["DB_WOCompnayCode"] = value; } }
        private string DB_WODeptDBCode { get { return ViewState["DB_WODeptDBCode"] as string ?? ""; } set { ViewState["DB_WODeptDBCode"] = value; } }
        private string DB_WKSDept { get { return ViewState["DB_WKSDept"] as string ?? ""; } set { ViewState["DB_WKSDept"] = value; } }
        private string DB_WKSDeptDBCode { get { return ViewState["DB_WKSDeptDBCode"] as string ?? ""; } set { ViewState["DB_WKSDeptDBCode"] = value; } }

        // New V2 Matrix Flags
        private string WO_ContractNature { get { return ViewState["WO_ContractNature"] as string ?? ""; } set { ViewState["WO_ContractNature"] = value; } }
        private string WO_BillingNature { get { return ViewState["WO_BillingNature"] as string ?? ""; } set { ViewState["WO_BillingNature"] = value; } }
        private bool WO_ReqCSM { get { return ViewState["WO_ReqCSM"] != null && (bool)ViewState["WO_ReqCSM"]; } set { ViewState["WO_ReqCSM"] = value; } }
        private bool WO_AutoTitle { get { return ViewState["WO_AutoTitle"] != null && (bool)ViewState["WO_AutoTitle"]; } set { ViewState["WO_AutoTitle"] = value; } }
        private string WO_MasterStatusCode { get { return ViewState["WO_MasterStatusCode"] as string ?? "1"; } set { ViewState["WO_MasterStatusCode"] = value; } }

        private static readonly object jobInsertLock = new object();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["WORKMAN"] == null)
                {
                    Response.Redirect("~/login.aspx");
                    return;
                }

                Bind_WorkRegion();
                if (DDL_Region.Items.FindByValue(Session["REGION"].ToString()) != null)
                {
                    DDL_Region.SelectedValue = Session["REGION"].ToString();
                    WorkRegion_ControllerCheck(); // V2 Check
                }

                Workorder_Binder();
                Bind_BillingType(Session["REGION"].ToString());
                Bind_AttendnaceCode();

                // --- NEW: Dynamic Date Picker Constraints ---
                int backdateLimit = GetUserBackdateLimit(Session["WORKMAN"].ToString());

                txt_jobdate.Text = DateTime.Now.ToString("yyyy-MM-dd"); // Default to Today
                txt_jobdate.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd"); // Block future dates
                txt_jobdate.Attributes["min"] = DateTime.Now.AddDays(-backdateLimit).ToString("yyyy-MM-dd"); // Block unauthorized backdating

                lbl_jobday.Text = DateTime.Today.DayOfWeek.ToString();
                CheckExistingJobsForDate(txt_jobdate.Text);
                DDL_AttenCode.SelectedValue = (DateTime.Today.DayOfWeek == DayOfWeek.Sunday) ? "OD" : "P";
            }
        }

        private int GetUserBackdateLimit(string workmanID)
        {
            int limit = 2; // Global Default: 2 days back allowed

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // NEW: Check AND IsActive = 1 so deactivated exceptions are completely ignored
                string qry = "SELECT ISNULL(Max_Backdate_Days, 2) FROM tlb_Backdate_Exceptions WHERE Employee_Workman = @Workman AND IsActive = 1";
                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Workman", workmanID);
                    object res = cmd.ExecuteScalar();
                    int dbLimit;
                    if (res != null && int.TryParse(res.ToString(), out dbLimit))
                    {
                        limit = dbLimit;
                    }
                }
            }
            catch { /* Keep the default 2-day limit if error occurs */ }
            finally { dbcl.DisconnectDb(); }

            return limit;
        }

        private void ShowNotification(string title, string message, string type)
        {
            string script = $"showPNotify('{title}', '{message.Replace("'", "\\'")}', '{type}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PNotify", script, true);
        }

        // =================================================================================
        // DB CONTROLLER LOGIC 
        // =================================================================================

        protected void txt_jobdate_TextChanged(object sender, EventArgs e)
        {
            DateTime selectedDate;
            if (DateTime.TryParse(txt_jobdate.Text, out selectedDate))
            {
                lbl_jobday.Text = selectedDate.DayOfWeek.ToString();
                EvaluateAttendanceCode();
                CheckExistingJobsForDate(txt_jobdate.Text);

                // Update auto-generated title dynamically ONLY if the DB Matrix allows it
                if (WO_AutoTitle)
                {
                    string prefix = string.IsNullOrEmpty(DB_WOType) ? "ARC" : DB_WOType;
                    txt_jobtitle.Text = $"{prefix} attendance for ({selectedDate.ToString("dd-MM-yyyy")})";
                }
            }
        }

        protected void DDL_BillingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // As per V2 Rules: Billing jobs ALWAYS require manual entry.
            // We ensure the field remains clear and ready for manual input when they switch Billing Types.
            if (WO_BillingNature == "Billing")
            {
                txt_jobtitle.Text = "";
                txt_jobtitle.Attributes.Add("placeholder", "Enter specific JOB Title details manually...");
            }
        }

        protected void DDL_Region_SelectedIndexChanged(object sender, EventArgs e)
        {
            WorkRegion_ControllerCheck();
            Workorder_Binder();
        }

        private void WorkRegion_ControllerCheck()
        {
            string region = DDL_Region.SelectedValue;
            if (string.IsNullOrEmpty(region)) return;

            try
            {
                // 1. Check Region Safety Flags from tlb_work_state_region
                string flagQuery = "SELECT Req_GPS_Tagging FROM tlb_work_state_region WHERE Work_Region_Code = @RegionCode";
                SqlParameter[] flagParams = { new SqlParameter("@RegionCode", region) };
                DataTable dtFlags = dbcl.SPreturn_dt(flagQuery, flagParams);

                if (dtFlags.Rows.Count > 0 && dtFlags.Rows[0]["Req_GPS_Tagging"].ToString() == "Yes")
                {
                    // Show the UI indicator and fire the JavaScript Geolocation API
                    div_gps_status.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "GPS", "requestGPSLocation();", true);
                }
                else
                {
                    // Hide the indicator and clear out any old coordinates
                    div_gps_status.Visible = false;
                    hf_latitude.Value = "";
                    hf_longitude.Value = "";
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Error", "Region Check Failed: " + ex.Message, "error");
            }
        }

        private void Bind_RequiredDocuments(string regionCode)
        {
            string query = @"
                SELECT d.Doc_ID, d.Doc_Category, d.Doc_Name, rd.IsMandatory 
                FROM tlb_DocumentMaster d
                LEFT JOIN tlb_Region_Documents rd ON d.Doc_ID = rd.Doc_ID AND rd.Work_Region_Code = @RegionCode
                WHERE d.IsActive = 1
                ORDER BY d.Doc_Category, d.Doc_Name";

            SqlParameter[] parameters = { new SqlParameter("@RegionCode", regionCode) };
            DataTable dtDocs = dbcl.SPreturn_dt(query, parameters);

            CBL_Documents.Items.Clear();

            if (dtDocs.Rows.Count > 0)
            {
                div_documents.Visible = true;
                foreach (DataRow row in dtDocs.Rows)
                {
                    ListItem item = new ListItem();
                    item.Text = $" [{row["Doc_Category"]}] {row["Doc_Name"]}";
                    item.Value = row["Doc_ID"].ToString();

                    bool isMandatory = row["IsMandatory"] != DBNull.Value && Convert.ToBoolean(row["IsMandatory"]);
                    if (isMandatory)
                    {
                        item.Selected = true;
                        item.Attributes.Add("onclick", "return false;"); // Lock checkbox
                        item.Attributes.CssStyle.Add("color", "darkred");
                        item.Attributes.CssStyle.Add("font-weight", "bold");
                    }
                    CBL_Documents.Items.Add(item);
                }
            }
            else { div_documents.Visible = false; }
        }

        /*
        ======================================================================================
        Method: DDL_Workorder_SelectedIndexChanged
        When: March 30, 2026
        Why: Added Matrix_Match_ID to the SQL query to detect silent LEFT JOIN failures. Includes logic to alert the UI and log the missing Rule Matrix mapping for administrators.
        ======================================================================================
        */
        protected void DDL_Workorder_SelectedIndexChanged(object sender, EventArgs e)
        {
            string workorderDBID = DDL_Workorder.SelectedValue;
            if (string.IsNullOrEmpty(workorderDBID)) return;

            try
            {
                // Join WO_Data with the Rule Matrix. 
                // We select m.Id AS Matrix_Match_ID to actively detect if the LEFT JOIN succeeded.
                string query = @"
                SELECT 
                    w.WO_Type, w.Work_Region_Code, w.Company_Code, w.Dept_DBCode, 
                    w.Contract_Nature, w.Billing_Nature, w.Execution_Type,
                    ISNULL(m.Req_PermitNo, 1) AS Req_PermitNo,
                    ISNULL(m.Req_CSM_Docs, 1) AS Req_CSM_Docs,
                    ISNULL(m.Req_Attendance, 1) AS Req_Attendance,
                    ISNULL(m.Auto_Generate_Title, 0) AS Auto_Generate_Title,
                    ISNULL(m.Default_MasterStatusCode, '1') AS Default_MasterStatusCode,
                    m.Id AS Matrix_Match_ID
                FROM tlb_WO_Data w
                LEFT JOIN tlb_WO_Rule_Matrix m 
                    ON w.Billing_Nature = m.Billing_Nature AND w.Execution_Type = m.Execution_Type
                WHERE w.DB_Code = @DB_Code";

                SqlParameter[] pram = { new SqlParameter("@DB_Code", workorderDBID) };
                dt = dbcl.SPreturn_dt(query, pram);

                if (dt.Rows.Count > 0)
                {
                    DB_WOType = dt.Rows[0]["WO_Type"].ToString();
                    DB_WOWorkRegion = dt.Rows[0]["Work_Region_Code"].ToString();
                    DB_WOCompnayCode = dt.Rows[0]["Company_Code"].ToString();
                    DB_WODeptDBCode = dt.Rows[0]["Dept_DBCode"].ToString();

                    string contractNature = dt.Rows[0]["Contract_Nature"].ToString();
                    string billingNature = dt.Rows[0]["Billing_Nature"].ToString();
                    string executionType = dt.Rows[0]["Execution_Type"].ToString();
                    WO_MasterStatusCode = dt.Rows[0]["Default_MasterStatusCode"].ToString();

                    // =====================================================================
                    // V2 MAPPING DETECTION & LOGGING
                    // =====================================================================
                    if (dt.Rows[0]["Matrix_Match_ID"] == DBNull.Value)
                    {
                        // The LEFT JOIN Failed! Point it out to the user on screen...
                        ShowNotification("Rule Mapping Missing!", $"No Rule Matrix defined for [{billingNature}] + [{executionType}]. Strict default rules applied.", "error");

                        // ...and log it permanently for the Admin to fix.
                        LogSystemIssue("Missing Rule Matrix", $"WO_DBCode: {workorderDBID} | Failed mapping for Billing_Nature: '{billingNature}' and Execution_Type: '{executionType}'.");
                    }
                    // =====================================================================

                    // 1. Handle "Non-Billing" Dropdown Restriction
                    ListItem nonBillingItem = DDL_BillingType.Items.FindByText("Non-Billing");

                    if (nonBillingItem != null)
                    {
                        nonBillingItem.Enabled = true;

                        // If it is strictly ARC and Billing, disable the Non-Billing option
                        if (contractNature == "ARC" && billingNature == "Billing")
                        {
                            nonBillingItem.Enabled = false;
                        }
                    }

                    // Setup Badges
                    div_wo_badges.Visible = true;
                    lbl_ContractNature.Text = contractNature;
                    lbl_BillingNature.Text = billingNature;

                    bool reqPermit = Convert.ToBoolean(dt.Rows[0]["Req_PermitNo"]);
                    bool reqCSM = Convert.ToBoolean(dt.Rows[0]["Req_CSM_Docs"]);
                    WO_ReqCSM = reqCSM;
                    bool reqAttendance = Convert.ToBoolean(dt.Rows[0]["Req_Attendance"]);

                    // 1. DYNAMIC PERMIT LOGIC
                    if (reqPermit)
                    {
                        txt_permitno.Text = "";
                        txt_permitno.ReadOnly = false;
                        RequiredFieldValidator3.Enabled = true;
                    }
                    else
                    {
                        txt_permitno.Text = "N/A";
                        txt_permitno.ReadOnly = true;
                        RequiredFieldValidator3.Enabled = false;
                    }

                    // 2. DYNAMIC CSM DOCS LOGIC
                    if (reqCSM)
                    {
                        div_documents.Visible = true;
                        Bind_RequiredDocuments(DB_WOWorkRegion);
                    }
                    else
                    {
                        div_documents.Visible = false;
                        CBL_Documents.Items.Clear();
                    }

                    // 3. DYNAMIC ATTENDANCE/SHIFT LOGIC
                    if (reqAttendance)
                    {
                        DDL_AttenCode.Enabled = true;
                        Bind_AttendnaceCode();
                        txt_jobshift.Text = "";
                        txt_jobshift.ReadOnly = false;
                    }
                    else
                    {
                        DDL_AttenCode.Enabled = false;
                        DDL_AttenCode.Items.Clear();
                        DDL_AttenCode.Items.Insert(0, new ListItem("N/A", "NA"));
                        txt_jobshift.Text = "G";
                        txt_jobshift.ReadOnly = false;
                    }

                    // 4. BILLING TYPE DROPDOWN (Safe Evaluation)
                    if (billingNature == "Non-Billing")
                    {
                        div_BillingType.Visible = false;
                        div_NonBillingAlert.Visible = true;
                        RFV1.Enabled = false;
                        DDL_BillingType.ClearSelection();
                    }
                    else
                    {
                        div_BillingType.Visible = true;
                        div_NonBillingAlert.Visible = false;
                        RFV1.Enabled = true;
                    }

                    // Store the flag in ViewState so other events (like Date change) can use it
                    WO_AutoTitle = Convert.ToBoolean(dt.Rows[0]["Auto_Generate_Title"]);
                    // =========================================================
                    // 5. DYNAMIC JOB TITLE LOGIC (DB CONTROLLED)
                    // =========================================================
                    if (WO_AutoTitle)
                    {
                        DateTime jobDate;
                        string dateStr = DateTime.TryParse(txt_jobdate.Text, out jobDate) ? jobDate.ToString("dd-MM-yyyy") : txt_jobdate.Text;
                        string prefix = string.IsNullOrEmpty(DB_WOType) ? "ARC" : DB_WOType;

                        txt_jobtitle.Text = $"{prefix} attendance for ({dateStr})";
                        txt_jobtitle.Attributes.Remove("placeholder");
                        txt_jobtitle.ReadOnly = true; // Lock it so users don't mess up the auto-format
                    }
                    else
                    {
                        // Admin Matrix mandates manual entry for this WO type
                        txt_jobtitle.Text = "";
                        txt_jobtitle.Attributes.Add("placeholder", "Enter specific JOB Title details manually...");
                        txt_jobtitle.ReadOnly = false;
                    }
                    // =========================================================

                    DataChecker();
                    EvaluateAttendanceCode();
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Error", ex.Message, "error");
            }
        }

        private void EvaluateAttendanceCode()
        {
            if (DDL_AttenCode.Enabled == false && DDL_AttenCode.Items.FindByValue("NA") != null)
            {
                return;
            }

            string companyCode = txt_company.Text;
            if (string.IsNullOrEmpty(companyCode)) return;

            DateTime jobDate;
            if (!DateTime.TryParse(txt_jobdate.Text, out jobDate)) return;

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                string query = "SELECT Day_Type FROM tlb_Company_Calendar WHERE Company_Code=@Comp AND Cal_Date=@Date";
                string dayType = "";

                using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Comp", companyCode);
                    cmd.Parameters.AddWithValue("@Date", jobDate.ToString("yyyy-MM-dd"));

                    object result = cmd.ExecuteScalar();
                    dayType = result != null ? result.ToString() : "";
                }

                Bind_AttendnaceCode();

                System.Collections.Generic.List<string> allowedCodes = new System.Collections.Generic.List<string>();
                string defaultSelection = "P";

                if (dayType == "FL")
                {
                    allowedCodes.AddRange(new string[] { "FL", "FP" });
                    defaultSelection = "FL";
                }
                else if (dayType == "NH")
                {
                    allowedCodes.AddRange(new string[] { "NH", "HP" });
                    defaultSelection = "NH";
                }
                else if (dayType == "OD" || jobDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    allowedCodes.AddRange(new string[] { "OD", "P" });
                    defaultSelection = "OD";
                }
                else
                {
                    allowedCodes.AddRange(new string[] { "P", "HD", "Ab" });
                    defaultSelection = "P";
                }

                for (int i = DDL_AttenCode.Items.Count - 1; i >= 0; i--)
                {
                    if (!allowedCodes.Contains(DDL_AttenCode.Items[i].Value))
                    {
                        DDL_AttenCode.Items.RemoveAt(i);
                    }
                }

                if (DDL_AttenCode.Items.FindByValue(defaultSelection) != null)
                {
                    DDL_AttenCode.SelectedValue = defaultSelection;
                }

                DDL_AttenCode.Enabled = true;
                DDL_AttenCode.ToolTip = "Options have been filtered based on the Master Company Calendar.";

            }
            catch (Exception ex)
            {
                ShowNotification("Calendar Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        public static string EncodeJobID(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return "";
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

        // =================================================================================
        // INSERTION AND SMART ROUTING
        // =================================================================================

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string[] words = txt_jobtitle.Text.Trim().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length <= 3)
            {
                ShowNotification("Validation Error", "JOB Title must contain more than 3 words to provide sufficient detail.", "error");
                return;
            }

            string generatedJobId = Insert_JOBData();

            if (!string.IsNullOrEmpty(generatedJobId))
            {
                string maskedJobId = EncodeJobID(generatedJobId);

                // V2 DYNAMIC ROUTING
                if (WO_MasterStatusCode == "3")
                {
                    Response.Redirect($"job_inpunch_v2.aspx?jobid={maskedJobId}", false);
                }
                else
                {
                    Response.Redirect($"job_permitupload_v2.aspx?jobid={maskedJobId}", false);
                }
            }
            else
            {
                ShowNotification("Failed", "Could not create the JOB ID.", "error");
            }
        }

        private string Insert_JOBData()
        {
            lock (jobInsertLock)
            {
                string JOBID = Find_DBCode();

                try
                {
                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    SqlCommand cmd = new SqlCommand("SP_InsertInto_JOBSTable", dbcl.Conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // ADO.NET FIX: Pass as a native DateTime object so SQL properly maps it to DATE type
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(txt_jobdate.Text).Date);
                    cmd.Parameters.AddWithValue("@Creator_Name", Session["USERNAME"].ToString());
                    cmd.Parameters.AddWithValue("@Creator_Workman", Session["WORKMAN"].ToString());
                    cmd.Parameters.AddWithValue("@Creator_Region", Session["REGION"].ToString());
                    cmd.Parameters.AddWithValue("@Creator_Company", Session["COMPANY_CODE"].ToString());
                    cmd.Parameters.AddWithValue("@Creator_Site", Session["U_SITE"].ToString());
                    cmd.Parameters.AddWithValue("@Creator_SiteCode", Session["U_SITECODE"].ToString());
                    cmd.Parameters.AddWithValue("@WorkOrderNo", DDL_Workorder.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@Workorder_Type", DB_WOType);
                    cmd.Parameters.AddWithValue("@JOBID", JOBID);
                    cmd.Parameters.AddWithValue("@JOBID_Status", "Active");
                    cmd.Parameters.AddWithValue("@JOB_Region", DDL_Region.SelectedValue);
                    cmd.Parameters.AddWithValue("@JOB_Company", txt_company.Text);
                    cmd.Parameters.AddWithValue("@JOB_Site", DDL_Worksite.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@JOB_SiteCode", DDL_Worksite.SelectedValue);
                    cmd.Parameters.AddWithValue("@JOB_InchargeWrk", DDL_Approver.SelectedValue);
                    cmd.Parameters.AddWithValue("@JOB_InchargeName", DDL_Approver.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@JOB_Dept", txt_dept.Text);
                    cmd.Parameters.AddWithValue("@JOB_Location", DDL_Location.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@JOB_Shift", txt_jobshift.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@JOB_Title", txt_jobtitle.Text);
                    cmd.Parameters.AddWithValue("@JOB_PermitNo", txt_permitno.Text);
                    cmd.Parameters.AddWithValue("@Incharge_Approval", "Pending");
                    cmd.Parameters.AddWithValue("@EntryExit", "Created");
                    cmd.Parameters.AddWithValue("@BillingType", WO_BillingNature == "Non-Billing" ? "Non-Billing" : (DDL_BillingType.SelectedItem != null ? DDL_BillingType.SelectedItem.Text : ""));
                    cmd.Parameters.AddWithValue("@BillingCode", WO_BillingNature == "Non-Billing" ? "NB" : DDL_BillingType.SelectedValue);
                    cmd.Parameters.AddWithValue("@AttendanceCode", DDL_AttenCode.SelectedValue);

                    //// Master Status Code Logic
                    //if (WO_BillingNature == "Non-Billing")
                    //{
                    //    cmd.Parameters.AddWithValue("@JOB_Status", "Permit Uploaded");
                    //    cmd.Parameters.AddWithValue("@FinalUpldStatus", "Yes");
                    //    cmd.Parameters.AddWithValue("@PermitUpload", "N/A");

                    //    // ADO.NET FIX: Pass as integer 0 to match INT schema requirement
                    //    cmd.Parameters.AddWithValue("@FileCount", 0);
                    //    cmd.Parameters.AddWithValue("@MasterStatusCode", "3");
                    //    cmd.Parameters.AddWithValue("@CSM_Documents", WO_ReqCSM ? "Yes" : "No");
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@JOB_Status", "Created");
                    //    cmd.Parameters.AddWithValue("@FinalUpldStatus", "No");
                    //    cmd.Parameters.AddWithValue("@PermitUpload", "No");

                    //    // ADO.NET FIX: Pass as integer 0 to match INT schema requirement
                    //    cmd.Parameters.AddWithValue("@FileCount", 0);
                    //    cmd.Parameters.AddWithValue("@MasterStatusCode", "1");
                    //    cmd.Parameters.AddWithValue("@CSM_Documents", WO_ReqCSM ? "Yes" : "No");
                    //}

                    // =========================================================
                    // V2 DYNAMIC DB INSERT LOGIC 
                    // (Driven entirely by the Matrix, no hardcoded "If Non-Billing")
                    // =========================================================
                    cmd.Parameters.AddWithValue("@MasterStatusCode", WO_MasterStatusCode);
                    cmd.Parameters.AddWithValue("@FileCount", 0);
                    cmd.Parameters.AddWithValue("@CSM_Documents", WO_ReqCSM ? "Yes" : "No");

                    if (WO_MasterStatusCode == "3")
                    {
                        // Direct to In-Punch Configuration
                        cmd.Parameters.AddWithValue("@JOB_Status", "Permit Uploaded");
                        cmd.Parameters.AddWithValue("@FinalUpldStatus", "Yes");
                        cmd.Parameters.AddWithValue("@PermitUpload", "N/A");
                    }
                    else
                    {
                        // Standard Permit Upload Configuration
                        cmd.Parameters.AddWithValue("@JOB_Status", "Created");
                        cmd.Parameters.AddWithValue("@FinalUpldStatus", "No");
                        cmd.Parameters.AddWithValue("@PermitUpload", "No");
                    }

                    cmd.ExecuteNonQuery();

                    // 2. RUN UPDATE FOR NEW V2 COLUMNS 
                    string selectedDocs = GetSelectedDocuments();
                    string lat = hf_latitude.Value;
                    string lon = hf_longitude.Value;

                    string updateQry = "UPDATE tbl_jobs SET Required_Documents=@Docs, GPS_Latitude=@Lat, GPS_Longitude=@Lon WHERE JOBID=@JobID";
                    using (SqlCommand updCmd = new SqlCommand(updateQry, dbcl.Conn))
                    {
                        updCmd.Parameters.AddWithValue("@Docs", selectedDocs);
                        updCmd.Parameters.AddWithValue("@Lat", string.IsNullOrEmpty(lat) ? (object)DBNull.Value : lat);
                        updCmd.Parameters.AddWithValue("@Lon", string.IsNullOrEmpty(lon) ? (object)DBNull.Value : lon);
                        updCmd.Parameters.AddWithValue("@JobID", JOBID);
                        updCmd.ExecuteNonQuery();
                    }

                    dbcl.DisconnectDb();
                    return JOBID;
                }
                catch (Exception ex)
                {
                    ShowNotification("Database Error", ex.Message, "error");
                    dbcl.DisconnectDb();
                    return null;
                }
            }
        }

        private string GetSelectedDocuments()
        {
            string docs = "";
            foreach (ListItem item in CBL_Documents.Items)
            {
                if (item.Selected) docs += item.Value + ",";
            }
            return docs.TrimEnd(',');
        }

        private string Find_DBCode()
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string newCode;
            bool isUnique = false;
            Random rnd = new Random();
            do
            {
                newCode = $"JOB{DateTime.Now.ToString("yyMMdd")}{rnd.Next(0, 1000):D3}";
                string query = "SELECT COUNT(*) FROM tbl_jobs WHERE JOBID = @jobid";
                using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@jobid", newCode);
                    isUnique = ((int)cmd.ExecuteScalar() == 0);
                }
            } while (!isUnique);
            dbcl.DisconnectDb();
            return newCode;
        }

        // =================================================================================
        // STANDARD BINDING HELPERS
        // =================================================================================

        private void Bind_WorkRegion()
        {
            string query = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where JOBID_Menu='Yes' order by Id";
            ExecuteAndBindDDL(query, DDL_Region, "Work_Region_Name", "Work_Region_Code", null);
        }

        public void Bind_BillingType(string regionCode)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                string query = @"
            SELECT bt.BilingType, bt.BilingType AS DropdownValue 
            FROM tlb_JOB_BillingType bt 
            INNER JOIN tlb_WorkRegion_BillingMapping map ON bt.Id = map.BillingTypeId 
            WHERE map.Work_Region_Code = @Region AND map.IsActive = 1
            ORDER BY bt.Id";

                using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Region", regionCode);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        DDL_BillingType.DataSource = rdr;
                        DDL_BillingType.DataTextField = "BilingType";
                        DDL_BillingType.DataValueField = "DropdownValue";
                        DDL_BillingType.DataBind();
                    }
                    DDL_BillingType.Items.Insert(0, new ListItem("-- Select JOB Type --", ""));
                }
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        private void Bind_AttendnaceCode()
        {
            string query = "Select Status_Name,Status_Code from tlb_attendancecodes where CreateJOBID='Yes' and Status='Present' order by slno";
            ExecuteAndBindDDL(query, DDL_AttenCode, "Status_Name", "Status_Code", null);
        }

        private void Workorder_Binder()
        {
            string regionCode = DDL_Region.SelectedValue;
            if (string.IsNullOrEmpty(regionCode)) return;

            string query = Session["USERTYPE"].ToString() == "Site Staff"
                ? "SELECT WO_Number, DB_Code FROM tlb_WO_Data WHERE Work_Region_Code=@RegionCode AND WO_Type='ARC' AND WO_Status='Active' AND JOBID_Menu='Yes' ORDER BY Id"
                : "SELECT WO_Number, DB_Code FROM tlb_WO_Data WHERE Work_Region_Code=@RegionCode AND WO_Status='Active' AND JOBID_Menu='Yes' ORDER BY WO_Type";

            SqlParameter[] parameters = { new SqlParameter("@RegionCode", regionCode) };
            ExecuteAndBindDDL(query, DDL_Workorder, "WO_Number", "DB_Code", parameters);
        }

        private void BindWorkSites(string regionCode)
        {
            string query = "select Worksite_Name, DB_Code from tlb_atsworksites where WorkRegion_Code=@RegionCode order by Id";
            SqlParameter[] parameters = { new SqlParameter("@RegionCode", regionCode) };
            ExecuteAndBindDDL(query, DDL_Worksite, "Worksite_Name", "DB_Code", parameters);
        }

        private void BindDepLoc(string deptDbCode, string companyDept, string regionCode = "")
        {
            string query = string.IsNullOrEmpty(regionCode)
                ? "select CompDept_Location,DB_Code from tlb_workregion_compdept_loc where Dept_DBCode=@DeptDbCode or Company_Department=@CompanyDept"
                : "select CompDept_Location,DB_Code from tlb_workregion_compdept_loc where Work_Region_Code=@RegionCode and Dept_DBCode=@DeptDbCode";

            SqlParameter[] parameters = { new SqlParameter("@RegionCode", regionCode), new SqlParameter("@DeptDbCode", deptDbCode), new SqlParameter("@CompanyDept", companyDept) };
            ExecuteAndBindDDL(query, DDL_Location, "CompDept_Location", "DB_Code", parameters);
        }

        private void ExecuteAndBindDDL(string query, DropDownList ddl, string textField, string valueField, SqlParameter[] parameters)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            using (SqlCommand Cmd = new SqlCommand(query, dbcl.Conn))
            {
                Cmd.CommandType = CommandType.Text;
                if (parameters != null) Cmd.Parameters.AddRange(parameters);
                ddl.DataSource = Cmd.ExecuteReader();
                ddl.DataTextField = textField;
                ddl.DataValueField = valueField;
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("Please Select Option", ""));
            }
            dbcl.DisconnectDb();
        }

        protected void DataChecker()
        {
            if (Session["REGION"].ToString() == DB_WOWorkRegion)
            {
                txt_workregion.Text = Session["REGION"].ToString();

                if (Session["COMPANY_CODE"].ToString() == DB_WOCompnayCode)
                {
                    txt_company.Text = Session["COMPANY_CODE"].ToString();
                    BindWorkSites(DDL_Region.SelectedValue);
                }
                else
                {
                    txt_company.Text = DB_WOCompnayCode;
                    BindWorkSites(DDL_Region.SelectedValue);
                }
            }
            else
            {
                txt_workregion.Text = DB_WOWorkRegion;
                BindWorkSites(DDL_Region.SelectedValue);
            }
        }

        protected void DDL_Worksite_SelectedIndexChanged(object sender, EventArgs e)
        {
            string worksite_DBCode = DDL_Worksite.SelectedValue;
            if (string.IsNullOrEmpty(worksite_DBCode)) return;

            try
            {
                string query = "select * from tlb_atsworksites where DB_Code=@DB_Code";
                SqlParameter[] pram = { new SqlParameter("@DB_Code", worksite_DBCode) };
                dt = dbcl.SPreturn_dt(query, pram);

                if (dt.Rows.Count > 0)
                {
                    DB_WKSDept = dt.Rows[0]["Company_Department"].ToString();
                    DB_WKSDeptDBCode = dt.Rows[0]["Dept_DBCode"].ToString();
                    txt_dept.Text = DB_WKSDept;

                    string appQuery = "select Employee_Name, Employee_Workman from tlb_atsworksiteIncharges where DB_Code=@DBCode and Status='Active' order by Id";
                    ExecuteAndBindDDL(appQuery, DDL_Approver, "Employee_Name", "Employee_Workman", new SqlParameter[] { new SqlParameter("@DBCode", worksite_DBCode) });

                    if (WO_ContractNature == "ARC")
                    {
                        BindDepLoc(DB_WKSDeptDBCode, DB_WKSDept);
                    }
                    else
                    {
                        BindDepLoc(DB_WKSDeptDBCode, "", DDL_Region.SelectedValue);
                    }

                    EvaluateAttendanceCode();
                }
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
        }

        protected void btn_cancel_Click(object sender, EventArgs e) { Response.Redirect("homepage.aspx", false); }

        private void CheckExistingJobsForDate(string selectedDateStr)
        {
            if (string.IsNullOrEmpty(selectedDateStr)) return;

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                DateTime date = Convert.ToDateTime(selectedDateStr);
                string qry = "SELECT JOBID FROM tbl_jobs WHERE Creator_Workman=@Workman AND CAST(CreatedDate AS DATE) = CAST(@Date AS DATE) AND JOBID_Status='Active'";

                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Workman", Session["WORKMAN"].ToString());
                    cmd.Parameters.AddWithValue("@Date", date);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        System.Collections.Generic.List<string> jobs = new System.Collections.Generic.List<string>();
                        while (rdr.Read())
                        {
                            jobs.Add(rdr["JOBID"].ToString());
                        }

                        if (jobs.Count > 0)
                        {
                            div_existing_jobs.Visible = true;
                            lbl_existing_jobs_list.Text = string.Join(", ", jobs);
                        }
                        else
                        {
                            div_existing_jobs.Visible = false;
                            lbl_existing_jobs_list.Text = "";
                        }
                    }
                }
            }
            catch { /* Silently fail UI if string parsing fails */ }
            finally { dbcl.DisconnectDb(); }
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            txt_jobdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            lbl_jobday.Text = DateTime.Today.DayOfWeek.ToString();
            div_existing_jobs.Visible = false;
            lbl_existing_jobs_list.Text = "";

            DDL_Workorder.ClearSelection();
            DDL_BillingType.ClearSelection();
            DDL_Worksite.ClearSelection();
            DDL_Approver.ClearSelection();
            DDL_Location.ClearSelection();

            Bind_AttendnaceCode();
            DDL_AttenCode.SelectedValue = (DateTime.Today.DayOfWeek == DayOfWeek.Sunday) ? "OD" : "P";
            DDL_AttenCode.Enabled = true;

            txt_permitno.Text = "";
            txt_jobshift.Text = "";
            txt_jobtitle.Text = "";
            txt_permitno.ReadOnly = false;
            txt_jobshift.ReadOnly = false;
            txt_jobtitle.Attributes.Remove("placeholder");

            div_gps_status.Visible = false;
            div_wo_badges.Visible = false;
            div_documents.Visible = false;
            div_NonBillingAlert.Visible = false;
            div_BillingType.Visible = true;

            hf_latitude.Value = "";
            hf_longitude.Value = "";
            CBL_Documents.Items.Clear();

            string resetScript = "document.getElementById('permitCount').innerHTML = '0 / 100'; document.getElementById('charCount').innerHTML = '0 / 200';";
            ScriptManager.RegisterStartupScript(this, GetType(), "resetCounters", resetScript, true);

            CheckExistingJobsForDate(txt_jobdate.Text);
        }

        private void LogSystemIssue(string logType, string logMessage)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string qry = "INSERT INTO tlb_System_Logs (Log_Type, Log_Message, Triggered_By_Workman, Page_Name) VALUES (@Type, @Msg, @Workman, @Page)";
                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Type", logType);
                    cmd.Parameters.AddWithValue("@Msg", logMessage);
                    cmd.Parameters.AddWithValue("@Workman", Session["WORKMAN"] != null ? Session["WORKMAN"].ToString() : "Unknown");
                    cmd.Parameters.AddWithValue("@Page", "create_jobid_v2.aspx");
                    cmd.ExecuteNonQuery();
                }
            }
            catch { /* Fail silently to not disrupt the user's workflow */ }
            finally { dbcl.DisconnectDb(); }
        }
    }
}