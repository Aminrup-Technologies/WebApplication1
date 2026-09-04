using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace WebApplication1.bussiness.production
{
    public partial class db_controller : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Security Check: Restrict to Admins or specific roles
                if (Session["USERID"] == null || Session["USERNAME"] == null)
                {
                    Response.Redirect("~/login.aspx", false);
                    return;
                }

                LoadInitialData();
            }
        }

        private void LoadInitialData()
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // 1. Load Regions safely for TAB 1 (Safety Compliance)
                string qryRegion = "SELECT Work_Region_Name + ' (' + Work_Region_Code + ')' AS DisplayName, Work_Region_Code FROM tlb_work_state_region ORDER BY Work_Region_Name";
                using (SqlCommand cmd = new SqlCommand(qryRegion, dbcl.Conn))
                {
                    using (SqlDataReader rdrRegion = cmd.ExecuteReader())
                    {
                        ddl_Regions.DataSource = rdrRegion;
                        ddl_Regions.DataTextField = "DisplayName";
                        ddl_Regions.DataValueField = "Work_Region_Code";
                        ddl_Regions.DataBind();
                    }
                    ddl_Regions.Items.Insert(0, new ListItem("-- Select Region --", ""));
                }

                // 2. Load Active Work Order Regions (Level 1 Drill-Down) for TAB 2
                string qryWORegion = "SELECT DISTINCT Work_Region_Name + ' (' + Work_Region_Code + ')' AS DisplayName, Work_Region_Code FROM tlb_WO_Data WHERE WO_Status='Active' ORDER BY DisplayName";
                using (SqlCommand cmdWOReg = new SqlCommand(qryWORegion, dbcl.Conn))
                {
                    using (SqlDataReader rdrWOReg = cmdWOReg.ExecuteReader())
                    {
                        ddl_wo_region.DataSource = rdrWOReg;
                        ddl_wo_region.DataTextField = "DisplayName";
                        ddl_wo_region.DataValueField = "Work_Region_Code";
                        ddl_wo_region.DataBind();
                    }
                    ddl_wo_region.Items.Insert(0, new ListItem("-- Select Region --", ""));
                }

                // Reset downstream WO dropdowns
                ddl_wo_company.Items.Insert(0, new ListItem("-- Select Company --", ""));
                ddl_wo_dept.Items.Insert(0, new ListItem("-- Select Dept --", ""));
                ddl_wo_number.Items.Insert(0, new ListItem("-- Select WO --", ""));

                // Load Companies for Smart Calendar Tab
                string qryCalComp = "SELECT DISTINCT Company_Name + ' (' + Company_Code + ')' AS DisplayName, Company_Code FROM tlb_WO_Data WHERE WO_Status='Active' ORDER BY DisplayName";
                using (SqlCommand cmdCalComp = new SqlCommand(qryCalComp, dbcl.Conn))
                {
                    using (SqlDataReader rdrCalComp = cmdCalComp.ExecuteReader())
                    {
                        ddl_cal_company.DataSource = rdrCalComp;
                        ddl_cal_company.DataTextField = "DisplayName";
                        ddl_cal_company.DataValueField = "Company_Code";
                        ddl_cal_company.DataBind();
                    }
                    ddl_cal_company.Items.Insert(0, new ListItem("-- Select Company --", ""));
                }

                // Load Data Grids
                BindDocMasterGrid();
                BindMatrixGrid();
                BindBackdateGrid();
                BindWOSummaryGrid();
            }
            catch (Exception ex)
            {
                ShowNotification(this, "Load Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }

            BindTriggerSettings();
        }

        // APPLIED BUG FIX: Safely escape messages for UpdatePanel callbacks
        private void ShowNotification(Control control, string title, string message, string type)
        {
            if (string.IsNullOrEmpty(message)) message = "An error occurred.";
            string safeMessage = message.Replace("'", "\\'").Replace("\"", "\\\"").Replace("\n", "<br/>").Replace("\r", "");
            string script = $"showPNotify('{title}', '{safeMessage}', '{type}');";

            ScriptManager.RegisterStartupScript(control, control.GetType(), "PNotify", script, true);
        }

        private void BindBackdateGrid()
        {
            using (SqlCommand cmd = new SqlCommand("SELECT Employee_Workman, Max_Backdate_Days, Remarks, IsActive FROM tlb_Backdate_Exceptions ORDER BY TimeStamp DESC", dbcl.Conn))
            {
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    gv_BackdateExceptions.DataSource = rdr;
                    gv_BackdateExceptions.DataBind();
                }
            }
        }

        // =================================================================================
        // TAB 6: BACKDATE EXCEPTIONS
        // =================================================================================
        protected void btn_SaveException_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_exc_workman.Text))
            {
                ShowNotification(btn_SaveException, "Warning", "Please enter a Workman SL.", "notice");
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_exc_remarks.Text))
            {
                ShowNotification(btn_SaveException, "Warning", "Remarks are required to grant an exception.", "notice");
                return;
            }

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // Upsert Logic
                string upsertQry = @"
            IF EXISTS (SELECT 1 FROM tlb_Backdate_Exceptions WHERE Employee_Workman = @Workman)
                UPDATE tlb_Backdate_Exceptions SET Max_Backdate_Days = @Days, Remarks = @Remarks, IsActive = 1, TimeStamp = GETDATE() WHERE Employee_Workman = @Workman
            ELSE
                INSERT INTO tlb_Backdate_Exceptions (Employee_Workman, Max_Backdate_Days, Remarks, IsActive) VALUES (@Workman, @Days, @Remarks, 1)";

                using (SqlCommand cmd = new SqlCommand(upsertQry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Workman", txt_exc_workman.Text.Trim());
                    cmd.Parameters.AddWithValue("@Days", ddl_exc_days.SelectedValue);
                    cmd.Parameters.AddWithValue("@Remarks", txt_exc_remarks.Text.Trim());
                    cmd.ExecuteNonQuery();
                }

                txt_exc_workman.Text = "";
                txt_exc_remarks.Text = "";
                BindBackdateGrid();
                ShowNotification(btn_SaveException, "Success", "Backdate exception saved and activated.", "success");
            }
            catch (Exception ex)
            {
                ShowNotification(btn_SaveException, "Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        protected void gv_BackdateExceptions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ToggleStatus")
            {
                try
                {
                    string[] args = e.CommandArgument.ToString().Split('|');
                    string workman = args[0];
                    bool currentStatus = Convert.ToBoolean(args[1]);
                    int newStatus = currentStatus ? 0 : 1;

                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();

                    string updateQry = "UPDATE tlb_Backdate_Exceptions SET IsActive = @NewStatus WHERE Employee_Workman = @Workman";
                    using (SqlCommand cmd = new SqlCommand(updateQry, dbcl.Conn))
                    {
                        cmd.Parameters.AddWithValue("@NewStatus", newStatus);
                        cmd.Parameters.AddWithValue("@Workman", workman);
                        cmd.ExecuteNonQuery();
                    }

                    BindBackdateGrid();

                    string msg = newStatus == 1 ? "Exception Reactivated!" : "Exception Deactivated. User reverted to standard 2 days.";
                    ShowNotification(gv_BackdateExceptions, "Status Updated", msg, "success");
                }
                catch (Exception ex)
                {
                    ShowNotification(gv_BackdateExceptions, "Error", ex.Message, "error");
                }
                finally
                {
                    dbcl.DisconnectDb();
                }
            }
        }

        // =================================================================================
        // TAB 5: SMART CALENDAR WEBMETHODS (AJAX)
        // =================================================================================

        [WebMethod]
        public static string SaveCalendarEvent(string companyCode, string calDate, string tagCode)
        {
            DB_Utility_OH4Y db = new DB_Utility_OH4Y();
            try
            {
                db.Sqlconnection();
                db.ConnectDb();

                string checkQry = "SELECT COUNT(*) FROM tlb_Company_Calendar WHERE Company_Code=@Comp AND Cal_Date=@Date";
                int count = 0;
                using (SqlCommand cmdCheck = new SqlCommand(checkQry, db.Conn))
                {
                    cmdCheck.Parameters.AddWithValue("@Comp", companyCode);
                    cmdCheck.Parameters.AddWithValue("@Date", calDate);
                    count = (int)cmdCheck.ExecuteScalar();
                }

                if (count > 0)
                {
                    string updQry = "UPDATE tlb_Company_Calendar SET Day_Type=@Tag WHERE Company_Code=@Comp AND Cal_Date=@Date";
                    using (SqlCommand cmdUpd = new SqlCommand(updQry, db.Conn))
                    {
                        cmdUpd.Parameters.AddWithValue("@Tag", tagCode);
                        cmdUpd.Parameters.AddWithValue("@Comp", companyCode);
                        cmdUpd.Parameters.AddWithValue("@Date", calDate);
                        cmdUpd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string insQry = "INSERT INTO tlb_Company_Calendar (Company_Code, Cal_Date, Day_Type) VALUES (@Comp, @Date, @Tag)";
                    using (SqlCommand cmdIns = new SqlCommand(insQry, db.Conn))
                    {
                        cmdIns.Parameters.AddWithValue("@Comp", companyCode);
                        cmdIns.Parameters.AddWithValue("@Date", calDate);
                        cmdIns.Parameters.AddWithValue("@Tag", tagCode);
                        cmdIns.ExecuteNonQuery();
                    }
                }

                return "Success";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
            finally
            {
                db.DisconnectDb();
            }
        }

        [WebMethod]
        public static string GetCalendarEvents(string companyCode)
        {
            if (string.IsNullOrEmpty(companyCode)) return "[]";

            DB_Utility_OH4Y db = new DB_Utility_OH4Y();
            System.Text.StringBuilder json = new System.Text.StringBuilder();

            try
            {
                db.Sqlconnection();
                db.ConnectDb();

                string qry = "SELECT Cal_Date, Day_Type FROM tlb_Company_Calendar WHERE Company_Code=@Comp";
                using (SqlCommand cmd = new SqlCommand(qry, db.Conn))
                {
                    cmd.Parameters.AddWithValue("@Comp", companyCode);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        json.Append("[");
                        bool isFirst = true;

                        while (rdr.Read())
                        {
                            if (!isFirst) json.Append(",");

                            string dateStr = Convert.ToDateTime(rdr["Cal_Date"]).ToString("yyyy-MM-dd");
                            string tag = rdr["Day_Type"].ToString();

                            string title = tag == "FL" ? "Festival Leave" : (tag == "OD" ? "Weekly Off" : "National Holiday");
                            string color = tag == "FL" ? "#e74c3c" : (tag == "OD" ? "#3498db" : "#9b59b6");

                            json.Append("{");
                            json.Append($"\"title\": \"{title}\",");
                            json.Append($"\"start\": \"{dateStr}\",");
                            json.Append($"\"backgroundColor\": \"{color}\",");
                            json.Append($"\"borderColor\": \"{color}\"");
                            json.Append("}");

                            isFirst = false;
                        }
                        json.Append("]");
                    }
                }
                return json.ToString();
            }
            catch
            {
                return "[]";
            }
            finally
            {
                db.DisconnectDb();
            }
        }

        [WebMethod]
        public static string DeleteCalendarEvent(string companyCode, string calDate)
        {
            DB_Utility_OH4Y db = new DB_Utility_OH4Y();
            try
            {
                db.Sqlconnection();
                db.ConnectDb();

                string delQry = "DELETE FROM tlb_Company_Calendar WHERE Company_Code=@Comp AND Cal_Date=@Date";
                using (SqlCommand cmd = new SqlCommand(delQry, db.Conn))
                {
                    cmd.Parameters.AddWithValue("@Comp", companyCode);
                    cmd.Parameters.AddWithValue("@Date", calDate);
                    cmd.ExecuteNonQuery();
                }

                return "Success";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
            finally
            {
                db.DisconnectDb();
            }
        }

        private void BindDocMasterGrid()
        {
            using (SqlCommand cmd = new SqlCommand("SELECT Doc_ID, Doc_Category, Doc_Name, IsActive FROM tlb_DocumentMaster ORDER BY Doc_Category, Doc_Name", dbcl.Conn))
            {
                using (SqlDataReader rdrDoc = cmd.ExecuteReader())
                {
                    gv_DocMaster.DataSource = rdrDoc;
                    gv_DocMaster.DataBind();
                }
            }
        }

        private void BindMatrixGrid()
        {
            string query = @"
                SELECT 
                    Billing_Nature, 
                    Execution_Type, 
                    Req_PermitNo, 
                    Req_CSM_Docs, 
                    Req_Attendance, 
                    ISNULL(Auto_Generate_Title, 0) AS Auto_Generate_Title, 
                    ISNULL(Default_MasterStatusCode, '1') AS Default_MasterStatusCode 
                FROM tlb_WO_Rule_Matrix 
                ORDER BY Billing_Nature, Execution_Type";

            using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
            {
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    gv_RuleMatrix.DataSource = rdr;
                    gv_RuleMatrix.DataBind();
                }
            }
        }

        // =================================================================================
        // TAB 1: REGION SAFETY & COMPLIANCE (Location Only)
        // =================================================================================
        protected void ddl_Regions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddl_Regions.SelectedValue))
            {
                RegionConfigRow.Visible = false;
                return;
            }

            string selectedRegionCode = ddl_Regions.SelectedValue;
            RegionConfigRow.Visible = true;

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                using (SqlCommand cmd = new SqlCommand("SELECT Req_GPS_Tagging FROM tlb_work_state_region WHERE Work_Region_Code=@Region", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Region", selectedRegionCode);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            ddl_req_gps.SelectedValue = rdr["Req_GPS_Tagging"] != DBNull.Value ? rdr["Req_GPS_Tagging"].ToString() : "No";
                        }
                    }
                }

                cbl_BillingTypes.Items.Clear();
                string btQuery = "SELECT Id, BilingType FROM tlb_JOB_BillingType ORDER BY Id";
                using (SqlCommand cmdBt = new SqlCommand(btQuery, dbcl.Conn))
                {
                    using (SqlDataReader rdrBt = cmdBt.ExecuteReader())
                    {
                        while (rdrBt.Read())
                        {
                            ListItem item = new ListItem(rdrBt["BilingType"].ToString(), rdrBt["Id"].ToString());
                            cbl_BillingTypes.Items.Add(item);
                        }
                    }
                }

                string mapQuery = "SELECT BillingTypeId FROM tlb_WorkRegion_BillingMapping WHERE Work_Region_Code=@Region AND IsActive=1";
                using (SqlCommand cmdMap = new SqlCommand(mapQuery, dbcl.Conn))
                {
                    cmdMap.Parameters.AddWithValue("@Region", selectedRegionCode);
                    using (SqlDataReader rdrMap = cmdMap.ExecuteReader())
                    {
                        while (rdrMap.Read())
                        {
                            string activeId = rdrMap["BillingTypeId"].ToString();
                            ListItem item = cbl_BillingTypes.Items.FindByValue(activeId);
                            if (item != null) item.Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowNotification(ddl_Regions, "Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        protected void btn_SaveRegion_Click(object sender, EventArgs e)
        {
            string regionCode = ddl_Regions.SelectedValue;
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                using (SqlCommand cmdUpdate = new SqlCommand("UPDATE tlb_work_state_region SET Req_GPS_Tagging=@GPS WHERE Work_Region_Code=@Region", dbcl.Conn))
                {
                    cmdUpdate.Parameters.AddWithValue("@GPS", ddl_req_gps.SelectedValue);
                    cmdUpdate.Parameters.AddWithValue("@Region", regionCode);
                    cmdUpdate.ExecuteNonQuery();
                }

                foreach (ListItem item in cbl_BillingTypes.Items)
                {
                    int billingId = int.Parse(item.Value);
                    int isActive = item.Selected ? 1 : 0;

                    string upsertQry = @"
                IF EXISTS (SELECT 1 FROM tlb_WorkRegion_BillingMapping WHERE Work_Region_Code=@Reg AND BillingTypeId=@BT)
                    UPDATE tlb_WorkRegion_BillingMapping SET IsActive=@Active WHERE Work_Region_Code=@Reg AND BillingTypeId=@BT
                ELSE
                    INSERT INTO tlb_WorkRegion_BillingMapping (Work_Region_Code, BillingTypeId, IsActive) VALUES (@Reg, @BT, @Active)";

                    using (SqlCommand cmdMap = new SqlCommand(upsertQry, dbcl.Conn))
                    {
                        cmdMap.Parameters.AddWithValue("@Reg", regionCode);
                        cmdMap.Parameters.AddWithValue("@BT", billingId);
                        cmdMap.Parameters.AddWithValue("@Active", isActive);
                        cmdMap.ExecuteNonQuery();
                    }
                }

                ShowNotification(btn_SaveRegion, "Success", $"Rules for {ddl_Regions.SelectedItem.Text} Updated!", "success");
            }
            catch (Exception ex)
            {
                ShowNotification(btn_SaveRegion, "Save Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        // =================================================================================
        // TAB 2: WORK ORDER CONTROLLER (Drill-Down Matrix)
        // =================================================================================
        protected void ddl_wo_region_SelectedIndexChanged(object sender, EventArgs e)
        {
            WOConfigRow.Visible = false;
            ddl_wo_company.Items.Clear();
            ddl_wo_dept.Items.Clear();
            ddl_wo_number.Items.Clear();
            ddl_wo_dept.Items.Insert(0, new ListItem("-- Select Dept --", ""));
            ddl_wo_number.Items.Insert(0, new ListItem("-- Select WO --", ""));

            if (string.IsNullOrEmpty(ddl_wo_region.SelectedValue))
            {
                ddl_wo_company.Items.Insert(0, new ListItem("-- Select Company --", ""));
                return;
            }

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string qry = "SELECT DISTINCT Company_Name + ' (' + Company_Code + ')' AS DisplayName, Company_Code FROM tlb_WO_Data WHERE WO_Status='Active' AND Work_Region_Code=@Reg ORDER BY DisplayName";
                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Reg", ddl_wo_region.SelectedValue);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        ddl_wo_company.DataSource = rdr;
                        ddl_wo_company.DataTextField = "DisplayName";
                        ddl_wo_company.DataValueField = "Company_Code";
                        ddl_wo_company.DataBind();
                    }
                    ddl_wo_company.Items.Insert(0, new ListItem("-- Select Company --", ""));
                }
                BindWOSummaryGrid();
            }
            catch (Exception ex) { ShowNotification(ddl_wo_region, "Error", ex.Message, "error"); }
            
            finally { dbcl.DisconnectDb(); }
        }

        protected void ddl_wo_company_SelectedIndexChanged(object sender, EventArgs e)
        {
            WOConfigRow.Visible = false;
            ddl_wo_dept.Items.Clear();
            ddl_wo_number.Items.Clear();
            ddl_wo_number.Items.Insert(0, new ListItem("-- Select WO --", ""));

            if (string.IsNullOrEmpty(ddl_wo_company.SelectedValue))
            {
                ddl_wo_dept.Items.Insert(0, new ListItem("-- Select Dept --", ""));
                BindWOSummaryGrid(); // Keep grid dynamic!
                return;
            }

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // ADDED 'DISTINCT' HERE ->
                string qry = "SELECT DISTINCT Department_Name + ' (' + Dept_DBCode + ')' AS DisplayName, Dept_DBCode FROM tlb_WO_Data WHERE WO_Status='Active' AND Work_Region_Code=@Reg AND Company_Code=@Comp ORDER BY DisplayName";

                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Reg", ddl_wo_region.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comp", ddl_wo_company.SelectedValue);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        ddl_wo_dept.DataSource = rdr;
                        ddl_wo_dept.DataTextField = "DisplayName";
                        ddl_wo_dept.DataValueField = "Dept_DBCode";
                        ddl_wo_dept.DataBind();
                    }
                    ddl_wo_dept.Items.Insert(0, new ListItem("-- Select Dept --", ""));
                }
            }
            catch (Exception ex)
            {
                ShowNotification(ddl_wo_company, "Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
                BindWOSummaryGrid(); // Refreshes the summary grid as discussed earlier
            }
        }

        protected void ddl_wo_dept_SelectedIndexChanged(object sender, EventArgs e)
        {
            WOConfigRow.Visible = false;
            ddl_wo_number.Items.Clear();

            if (string.IsNullOrEmpty(ddl_wo_dept.SelectedValue))
            {
                ddl_wo_number.Items.Insert(0, new ListItem("-- Select WO --", ""));
                return;
            }

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string qry = "SELECT WO_Number, DB_Code FROM tlb_WO_Data WHERE WO_Status='Active' AND Work_Region_Code=@Reg AND Company_Code=@Comp AND Dept_DBCode=@Dept ORDER BY WO_Number";
                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Reg", ddl_wo_region.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comp", ddl_wo_company.SelectedValue);
                    cmd.Parameters.AddWithValue("@Dept", ddl_wo_dept.SelectedValue);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        ddl_wo_number.DataSource = rdr;
                        ddl_wo_number.DataTextField = "WO_Number";
                        ddl_wo_number.DataValueField = "DB_Code";
                        ddl_wo_number.DataBind();
                    }
                    ddl_wo_number.Items.Insert(0, new ListItem("-- Select WO --", ""));
                }
                BindWOSummaryGrid();
            }
            catch (Exception ex) { ShowNotification(ddl_wo_dept, "Error", ex.Message, "error"); }
           
            finally { dbcl.DisconnectDb(); }
        }

        protected void ddl_wo_number_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddl_wo_number.SelectedValue))
            {
                WOConfigRow.Visible = false;
                return;
            }

            WOConfigRow.Visible = true;
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand cmd = new SqlCommand("SELECT Contract_Nature, Billing_Nature, Execution_Type FROM tlb_WO_Data WHERE DB_Code=@WO", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@WO", ddl_wo_number.SelectedValue);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            if (rdr["Contract_Nature"] != DBNull.Value) ddl_contract_nature.SelectedValue = rdr["Contract_Nature"].ToString();
                            if (rdr["Billing_Nature"] != DBNull.Value) ddl_billing_nature.SelectedValue = rdr["Billing_Nature"].ToString();
                            if (rdr["Execution_Type"] != DBNull.Value) ddl_execution_type.SelectedValue = rdr["Execution_Type"].ToString();
                        }
                    }
                }
                BindWOSummaryGrid();
            }
            catch (Exception ex)
            {
                ShowNotification(ddl_wo_number, "Error", ex.Message, "error");
            }
            
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        protected void btn_SaveWO_Click(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // Changed WHERE WO_Number=@WO to WHERE DB_Code=@WO
                string updateQry = "UPDATE tlb_WO_Data SET Contract_Nature=@CN, Billing_Nature=@BN, Execution_Type=@ET WHERE DB_Code=@WO";

                using (SqlCommand cmd = new SqlCommand(updateQry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@CN", ddl_contract_nature.SelectedValue);
                    cmd.Parameters.AddWithValue("@BN", ddl_billing_nature.SelectedValue);
                    cmd.Parameters.AddWithValue("@ET", ddl_execution_type.SelectedValue);
                    cmd.Parameters.AddWithValue("@WO", ddl_wo_number.SelectedValue);

                    int rowsAffected = cmd.ExecuteNonQuery(); // Good practice to check if it actually updated

                    if (rowsAffected > 0)
                    {
                        ShowNotification(btn_SaveWO, "Success", "Work Order parameters updated.", "success");
                    }
                    else
                    {
                        ShowNotification(btn_SaveWO, "Warning", "Save failed: Work Order not found in database.", "notice");
                    }
                }

                // ⚡ REFRESH THE GRID IMMEDIATELY AFTER SAVING
                BindWOSummaryGrid();
            }
            catch (Exception ex)
            {
                ShowNotification(btn_SaveWO, "Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        // =================================================================================
        // TAB 3: DOCUMENT MASTER
        // =================================================================================
        protected void btn_AddDoc_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_newdoc_name.Text))
            {
                ShowNotification(btn_AddDoc, "Warning", "Document Name cannot be empty.", "notice");
                return;
            }

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO tlb_DocumentMaster (Doc_Category, Doc_Name, IsActive) VALUES (@Cat, @Name, 1)", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Cat", ddl_newdoc_category.SelectedValue);
                    cmd.Parameters.AddWithValue("@Name", txt_newdoc_name.Text.Trim());
                    cmd.ExecuteNonQuery();
                }

                txt_newdoc_name.Text = "";
                BindDocMasterGrid();

                ShowNotification(btn_AddDoc, "Added", "New document added to Dictionary.", "success");
            }
            catch (Exception ex)
            {
                ShowNotification(btn_AddDoc, "Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        // =================================================================================
        // TAB 4: RULE MATRIX CONTROLLER
        // =================================================================================
        protected void btn_SaveMatrix_Click(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                string checkQry = "SELECT COUNT(*) FROM tlb_WO_Rule_Matrix WHERE Billing_Nature=@BN AND Execution_Type=@ET";
                int count = 0;
                using (SqlCommand cmdCheck = new SqlCommand(checkQry, dbcl.Conn))
                {
                    cmdCheck.Parameters.AddWithValue("@BN", ddl_matrix_billing.SelectedValue);
                    cmdCheck.Parameters.AddWithValue("@ET", ddl_matrix_execution.SelectedValue);
                    count = (int)cmdCheck.ExecuteScalar();
                }

                int permit = chk_req_permit.Checked ? 1 : 0;
                int csm = chk_req_csm.Checked ? 1 : 0;
                int attendance = chk_req_attendance.Checked ? 1 : 0;
                int autoTitle = chk_auto_title.Checked ? 1 : 0;

                if (count > 0)
                {
                    string updQry = "UPDATE tlb_WO_Rule_Matrix SET Req_PermitNo=@Permit, Req_CSM_Docs=@CSM, Req_Attendance=@Att, Auto_Generate_Title=@AutoTitle, Default_MasterStatusCode=@RouteCode WHERE Billing_Nature=@BN AND Execution_Type=@ET";
                    using (SqlCommand cmdUpd = new SqlCommand(updQry, dbcl.Conn))
                    {
                        cmdUpd.Parameters.AddWithValue("@Permit", permit);
                        cmdUpd.Parameters.AddWithValue("@CSM", csm);
                        cmdUpd.Parameters.AddWithValue("@Att", attendance);
                        cmdUpd.Parameters.AddWithValue("@AutoTitle", autoTitle);
                        cmdUpd.Parameters.AddWithValue("@RouteCode", ddl_matrix_routing.SelectedValue);
                        cmdUpd.Parameters.AddWithValue("@BN", ddl_matrix_billing.SelectedValue);
                        cmdUpd.Parameters.AddWithValue("@ET", ddl_matrix_execution.SelectedValue);
                        cmdUpd.ExecuteNonQuery();
                    }
                    ShowNotification(btn_SaveMatrix, "Updated", "Matrix rule updated successfully.", "success");
                }
                else
                {
                    string insQry = "INSERT INTO tlb_WO_Rule_Matrix (Billing_Nature, Execution_Type, Req_PermitNo, Req_CSM_Docs, Req_Attendance, Auto_Generate_Title, Default_MasterStatusCode) VALUES (@BN, @ET, @Permit, @CSM, @Att, @AutoTitle, @RouteCode)";
                    using (SqlCommand cmdIns = new SqlCommand(insQry, dbcl.Conn))
                    {
                        cmdIns.Parameters.AddWithValue("@BN", ddl_matrix_billing.SelectedValue);
                        cmdIns.Parameters.AddWithValue("@ET", ddl_matrix_execution.SelectedValue);
                        cmdIns.Parameters.AddWithValue("@Permit", permit);
                        cmdIns.Parameters.AddWithValue("@CSM", csm);
                        cmdIns.Parameters.AddWithValue("@Att", attendance);
                        cmdIns.Parameters.AddWithValue("@AutoTitle", autoTitle);
                        cmdIns.Parameters.AddWithValue("@RouteCode", ddl_matrix_routing.SelectedValue);
                        cmdIns.ExecuteNonQuery();
                    }
                    ShowNotification(btn_SaveMatrix, "Created", "New matrix rule created successfully.", "success");
                }

                BindMatrixGrid();
                BindWOSummaryGrid();
            }
            catch (Exception ex)
            {
                ShowNotification(btn_SaveMatrix, "Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        private void BindWOSummaryGrid()
        {
            // Ensure connection is open (you can handle this safely in your standard try/catch blocks)
            bool closeConnection = false;
            if (dbcl.Conn.State == ConnectionState.Closed)
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                closeConnection = true;
            }

            try
            {
                string region = ddl_wo_region.SelectedValue;
                string company = ddl_wo_company.SelectedValue;
                string dept = ddl_wo_dept.SelectedValue;

                string query = @"
            SELECT 
                Company_Name, 
                Department_Name, 
                WO_Number, 
                ISNULL(Contract_Nature, 'UNASSIGNED') AS Contract_Nature, 
                ISNULL(Billing_Nature, 'UNASSIGNED') AS Billing_Nature, 
                ISNULL(Execution_Type, 'UNASSIGNED') AS Execution_Type 
            FROM tlb_WO_Data 
            WHERE WO_Status = 'Active' ";

                // Dynamically append filters based on what the user has selected so far
                if (!string.IsNullOrEmpty(region)) query += " AND Work_Region_Code = @Reg";
                if (!string.IsNullOrEmpty(company)) query += " AND Company_Code = @Comp";
                if (!string.IsNullOrEmpty(dept)) query += " AND Dept_DBCode = @Dept";

                query += " ORDER BY Company_Name, Department_Name, WO_Number";

                using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
                {
                    if (!string.IsNullOrEmpty(region)) cmd.Parameters.AddWithValue("@Reg", region);
                    if (!string.IsNullOrEmpty(company)) cmd.Parameters.AddWithValue("@Comp", company);
                    if (!string.IsNullOrEmpty(dept)) cmd.Parameters.AddWithValue("@Dept", dept);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        gv_WO_Summary.DataSource = rdr;
                        gv_WO_Summary.DataBind();
                    }
                }
            }
            finally
            {
                if (closeConnection) dbcl.DisconnectDb();
            }
        }

        private void BindTriggerSettings()
        {
            List<NotificationTriggerHelper.TriggerRow> rows;
            string error;
            if (!NotificationTriggerHelper.TryLoadRows(out rows, out error))
            {
                bool missingTable = NotificationTriggerHelper.IsMissingTableException(new Exception(error ?? ""));
                pnl_triggers_missing.Visible = missingTable;
                pnl_triggers_ready.Visible = false;
                if (!missingTable)
                {
                    ShowNotification(this, "Triggers", string.IsNullOrEmpty(error) ? "Could not load notification triggers." : error, "error");
                }
                return;
            }

            pnl_triggers_missing.Visible = false;
            pnl_triggers_ready.Visible = true;

            NotificationTriggerHelper.TriggerRow portal = null;
            List<NotificationTriggerHelper.TriggerRow> authRows = new List<NotificationTriggerHelper.TriggerRow>();
            List<NotificationTriggerHelper.TriggerRow> modules = new List<NotificationTriggerHelper.TriggerRow>();
            for (int i = 0; i < rows.Count; i++)
            {
                if (string.Equals(rows[i].TriggerKey, NotificationTriggerHelper.KeyPortal, StringComparison.OrdinalIgnoreCase))
                {
                    portal = rows[i];
                }
                else if (NotificationTriggerHelper.IsAuthenticationOtp(rows[i].TriggerKey))
                {
                    authRows.Add(rows[i]);
                }
                else
                {
                    modules.Add(rows[i]);
                }
            }

            if (portal != null)
            {
                chk_portal_email.Checked = portal.EmailEnabled;
                chk_portal_whatsapp.Checked = portal.WhatsAppEnabled;
            }

            gv_TriggerAuth.DataSource = authRows;
            gv_TriggerAuth.DataBind();
            gv_TriggerModules.DataSource = modules;
            gv_TriggerModules.DataBind();
        }

        protected void chk_Portal_CheckedChanged(object sender, EventArgs e)
        {
            SavePortalTriggers();
        }

        protected void chk_TriggerRow_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            GridViewRow row = chk != null ? chk.NamingContainer as GridViewRow : null;
            GridView grid = row != null ? row.NamingContainer as GridView : null;
            if (grid == null || row.RowIndex < 0 || grid.DataKeys[row.RowIndex] == null)
            {
                ShowNotification(upTriggers, "Save failed", "Could not identify the trigger row.", "error");
                ShowTriggerTab();
                return;
            }

            string key = grid.DataKeys[row.RowIndex].Value.ToString();
            CheckBox chkEmail = row.FindControl("chk_row_email") as CheckBox;
            CheckBox chkWa = row.FindControl("chk_row_whatsapp") as CheckBox;
            bool emailOn = chkEmail != null && chkEmail.Checked;
            bool waOn = chkWa != null && chkWa.Checked;

            string error;
            string updatedBy = Session["USERID"] != null ? Session["USERID"].ToString() : "";
            if (!NotificationTriggerHelper.TrySaveRow(key, emailOn, waOn, updatedBy, out error))
            {
                ShowNotification(upTriggers, "Save failed", string.IsNullOrEmpty(error) ? "Could not save trigger." : error, "error");
                ShowTriggerTab();
                return;
            }

            BindTriggerSettings();
            ShowNotification(upTriggers, "Saved", "Trigger updated.", "success");
            ShowTriggerTab();
        }

        private void SavePortalTriggers()
        {
            string error;
            string updatedBy = Session["USERID"] != null ? Session["USERID"].ToString() : "";
            if (!NotificationTriggerHelper.TrySaveRow(
                NotificationTriggerHelper.KeyPortal,
                chk_portal_email.Checked,
                chk_portal_whatsapp.Checked,
                updatedBy,
                out error))
            {
                ShowNotification(upTriggers, "Save failed", string.IsNullOrEmpty(error) ? "Could not save portal triggers." : error, "error");
                ShowTriggerTab();
                return;
            }

            BindTriggerSettings();
            ShowNotification(upTriggers, "Saved", "Portal Email/WhatsApp updated.", "success");
            ShowTriggerTab();
        }

        private void ShowTriggerTab()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showTriggerTab", "showTriggerTab();", true);
        }
    }
}