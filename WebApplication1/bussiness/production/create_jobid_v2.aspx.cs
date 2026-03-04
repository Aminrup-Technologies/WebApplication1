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

                lbl_jobdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                lbl_jobday.Text = DateTime.Today.DayOfWeek.ToString();
                DDL_AttenCode.SelectedValue = (DateTime.Today.DayOfWeek == DayOfWeek.Sunday) ? "OD" : "P";
            }
        }

        private void ShowNotification(string title, string message, string type)
        {
            string script = $"showPNotify('{title}', '{message.Replace("'", "\\'")}', '{type}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PNotify", script, true);
        }

        // =================================================================================
        // DB CONTROLLER LOGIC 
        // =================================================================================

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
                // 1. Check Region Safety Flags
                string flagQuery = "SELECT Req_GPS_Tagging FROM tlb_work_state_region WHERE Work_Region_Code = @RegionCode";
                SqlParameter[] flagParams = { new SqlParameter("@RegionCode", region) };
                DataTable dtFlags = dbcl.SPreturn_dt(flagQuery, flagParams);

                if (dtFlags.Rows.Count > 0 && dtFlags.Rows[0]["Req_GPS_Tagging"].ToString() == "Yes")
                {
                    div_gps_status.Visible = true;
                    // Trigger browser GPS API via JS
                    ScriptManager.RegisterStartupScript(this, GetType(), "GPS", "requestGPSLocation();", true);
                }
                else
                {
                    div_gps_status.Visible = false;
                }

                // 2. Bind the Digital Document Checklist
                Bind_RequiredDocuments(region);
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
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

        protected void DDL_Workorder_SelectedIndexChanged(object sender, EventArgs e)
        {
            string workorderDBID = DDL_Workorder.SelectedValue;
            if (string.IsNullOrEmpty(workorderDBID)) return;

            try
            {
                string query = "select WO_Type, Work_Region_Code, Company_Code, Dept_DBCode, Contract_Nature, Billing_Nature from tlb_WO_Data where DB_Code=@DB_Code";
                SqlParameter[] pram = { new SqlParameter("@DB_Code", workorderDBID) };
                dt = dbcl.SPreturn_dt(query, pram);

                if (dt.Rows.Count > 0)
                {
                    DB_WOType = dt.Rows[0]["WO_Type"].ToString();
                    DB_WOWorkRegion = dt.Rows[0]["Work_Region_Code"].ToString();
                    DB_WOCompnayCode = dt.Rows[0]["Company_Code"].ToString();
                    DB_WODeptDBCode = dt.Rows[0]["Dept_DBCode"].ToString();

                    // Fetch Matrix
                    WO_ContractNature = dt.Rows[0]["Contract_Nature"].ToString();
                    WO_BillingNature = dt.Rows[0]["Billing_Nature"].ToString();

                    // Update Badges
                    div_wo_badges.Visible = true;
                    lbl_ContractNature.Text = WO_ContractNature;
                    lbl_BillingNature.Text = WO_BillingNature;

                    // Matrix Rule: Hide Billing dropdown & permits for Non-Billing jobs
                    if (WO_BillingNature == "Non-Billing")
                    {
                        div_BillingType.Visible = false;
                        div_NonBillingAlert.Visible = true;
                        div_documents.Visible = false; // Non-billing skips docs usually
                        RFV1.Enabled = false;
                    }
                    else
                    {
                        div_BillingType.Visible = true;
                        div_NonBillingAlert.Visible = false;
                        div_documents.Visible = true;
                        RFV1.Enabled = true;
                    }

                    DataChecker(); // Validates Company logic
                }
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
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
            string generatedJobId = Insert_JOBData();

            if (!string.IsNullOrEmpty(generatedJobId))
            {
                // Mask the JOBID before sending it in the URL
                string maskedJobId = EncodeJobID(generatedJobId);

                // SMART ROUTING: Determine the next step based on the DB Controller
                if (WO_BillingNature == "Non-Billing")
                {
                    // MasterStatusCode 3 -> Redirect directly to In-Punch
                    Response.Redirect($"job_inpunch_v2.aspx?jobid={maskedJobId}", false);
                }
                else
                {
                    // MasterStatusCode 1 -> Redirect to Permit Upload
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
                    // 1. EXECUTE EXISTING SP TO CREATE THE RECORD SAFELY
                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    SqlCommand cmd = new SqlCommand("SP_InsertInto_JOBSTable", dbcl.Conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CreatedDate", lbl_jobdate.Text);
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
                    cmd.Parameters.AddWithValue("@BillingType", WO_BillingNature == "Non-Billing" ? "Non-Billing" : DDL_BillingType.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@BillingCode", WO_BillingNature == "Non-Billing" ? "NB" : DDL_BillingType.SelectedValue);
                    cmd.Parameters.AddWithValue("@AttendanceCode", DDL_AttenCode.SelectedValue);

                    // Master Status Code Logic
                    if (WO_BillingNature == "Non-Billing")
                    {
                        cmd.Parameters.AddWithValue("@JOB_Status", "Permit Uploaded");
                        cmd.Parameters.AddWithValue("@FinalUpldStatus", "Yes");
                        cmd.Parameters.AddWithValue("@PermitUpload", "N/A");
                        cmd.Parameters.AddWithValue("@FileCount", "0");
                        cmd.Parameters.AddWithValue("@MasterStatusCode", "3");
                        cmd.Parameters.AddWithValue("@CSM_Documents", "No");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@JOB_Status", "Created");
                        cmd.Parameters.AddWithValue("@FinalUpldStatus", "No");
                        cmd.Parameters.AddWithValue("@PermitUpload", "No");
                        cmd.Parameters.AddWithValue("@FileCount", "0");
                        cmd.Parameters.AddWithValue("@MasterStatusCode", "1");
                        cmd.Parameters.AddWithValue("@CSM_Documents", "Yes");
                    }

                    cmd.ExecuteNonQuery();

                    // 2. RUN UPDATE FOR NEW V2 COLUMNS (To avoid modifying your original SP)
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
                    return JOBID; // Return ID for routing
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

        private void Bind_BillingType(string regionCode)
        {
            string query = "SELECT DISTINCT b.BilingType, b.BillingCode FROM tlb_WorkRegion_BillingMapping m JOIN tlb_JOB_BillingType b ON m.BillingTypeId = b.Id WHERE m.Work_Region_Code = @RegionCode AND m.IsActive = 1;";
            SqlParameter[] parameters = { new SqlParameter("@RegionCode", regionCode) };
            ExecuteAndBindDDL(query, DDL_BillingType, "BilingType", "BillingCode", parameters);
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
                ? "select WO_Number, DB_Code from tlb_WO_Data where Work_Region_Code=@RegionCode and WO_Type='ARC' and WO_Status='Active' order by Id"
                : "select WO_Number, DB_Code from tlb_WO_Data where Work_Region_Code=@RegionCode and WO_Status='Active' order by WO_Type";

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
                    txt_permitno.Text = (WO_ContractNature != "ARC") ? "N/A" : "";
                    txt_permitno.ReadOnly = (WO_ContractNature != "ARC");
                }
                else { txt_company.Text = DB_WOCompnayCode; }
            }
            else
            {
                BindWorkSites(DDL_Region.SelectedValue);
                txt_workregion.Text = DB_WOWorkRegion;
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

                    // Bind Approver
                    string appQuery = "select Employee_Name, Employee_Workman from tlb_atsworksiteIncharges where DB_Code=@DBCode and Status='Active' order by Id";
                    ExecuteAndBindDDL(appQuery, DDL_Approver, "Employee_Name", "Employee_Workman", new SqlParameter[] { new SqlParameter("@DBCode", worksite_DBCode) });

                    if (WO_ContractNature == "ARC") BindDepLoc(DB_WKSDeptDBCode, DB_WKSDept);
                    else
                    {
                        BindDepLoc(DB_WKSDeptDBCode, "", DDL_Region.SelectedValue);
                        txt_jobtitle.Text = $"{DB_WOType} attendance for ({DateTime.Now.Date:dd-MM-yyyy})";
                    }
                }
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
        }

        protected void btn_dateswap_Click(object sender, EventArgs e)
        {
            DateTime dt = (btn_dateswap.Text == "Today") ? DateTime.Now.AddDays(-1) : DateTime.Now;
            lbl_jobdate.Text = dt.ToString("dd-MMM-yyyy");
            lbl_jobday.Text = dt.DayOfWeek.ToString();
            btn_dateswap.Text = (btn_dateswap.Text == "Today") ? "Yesterday" : "Today";
            DDL_AttenCode.SelectedValue = (dt.DayOfWeek == DayOfWeek.Sunday) ? "OD" : "P";
        }

        protected void btn_cancel_Click(object sender, EventArgs e) { Response.Redirect("homepage.aspx", false); }
    }
}