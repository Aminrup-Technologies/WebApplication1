using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1.bussiness.production
{
    public partial class payroll_controller : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;

        public static string state = string.Empty;
        public static string region = string.Empty;
        public static string comp = string.Empty;

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

                        //Session["Changer"] = null;
                    }
                    else
                    {
                        region = Session["REGION"].ToString();
                        comp = Session["COMPANY_CODE"].ToString();
                        state = Session["STATE"].ToString();
                    }

                    string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = 'IN' and State_Code ='" + state + "' order by Id ";
                    BindRegions(CmdString1);

                    string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='" + state + "' and Work_Region_Code = '" + region + "' order by Id ";
                    BindCompany(CmdString3);

                    DDL_Region.SelectedValue = region;
                    DDL_Company.SelectedValue = comp;

                    dbcl.CalDateCombo1(DDL_Day, DDL_Month, DDL_Year);
                    dbcl.CalDateCombo1(DDL_D2, DDL_M2, DDL_Y2);

                    if (Session["REGION"].ToString() != "GBL")
                    {
                        CheckforUser();
                    }
                }
            }
        }

        private void CheckforUser()
        {
            DDL_Region.SelectedValue = Session["REGION"].ToString();
            DDL_Region.Enabled = false;
            string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='" + state + "' and Work_Region_Code = '" + region + "' order by Id ";
            BindCompany(CmdString3);
        }

        private void BindRegions(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Region.DataSource = Cmd.ExecuteReader();
            DDL_Region.DataTextField = "Work_Region_Name";
            DDL_Region.DataValueField = "Work_Region_Code";
            DDL_Region.DataBind();
            DDL_Region.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_Region_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='OD' and Work_Region_Code = '" + DDL_Region.SelectedValue.ToString() + "' order by Id ";
            BindCompany(CmdString3);
        }

        private void BindCompany(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Company.DataSource = Cmd.ExecuteReader();
            DDL_Company.DataTextField = "Company_Name";
            DDL_Company.DataValueField = "Company_Code";
            DDL_Company.DataBind();
            DDL_Company.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }



        protected void gvPayrollStatus_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Set the GridView to edit mode
            gvPayrollStatus.EditIndex = e.NewEditIndex;
            // Re-bind the GridView to reflect the edit mode
            FilterGridView();  // You can modify this method to rebind data from the database
        }

        protected void gvPayrollStatus_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Get the row being updated
            GridViewRow row = gvPayrollStatus.Rows[e.RowIndex];

            // Access data from BoundFields using Cells (not FindControl)
            string payrollRegion = row.Cells[0].Text.Trim();
            string payrollCompany = row.Cells[1].Text.Trim();

            // Access DropDownList values from TemplateFields
            string f17TrialStatus = ((DropDownList)row.FindControl("ddlF17TrialStatus")).SelectedValue;
            string finalStatus = ((DropDownList)row.FindControl("ddlFinalStatus")).SelectedValue;
            string deductionLocked = ((DropDownList)row.FindControl("ddlDeductionLocked")).SelectedValue;
            string finalF17Status = ((DropDownList)row.FindControl("ddlFinalF17Status")).SelectedValue;
            string payslipVisibility = ((DropDownList)row.FindControl("ddlPayslipVisibility")).SelectedValue;

            // Get the primary key (Id) of the row
            int id = Convert.ToInt32(gvPayrollStatus.DataKeys[e.RowIndex].Value);

            // Get audit information from session
            string updatedBy = Session["WORKMAN"]?.ToString() ?? "System";
            DateTime updatedOn = DateTime.Now;

            // Update query including audit fields
            string query = @"
        UPDATE tbl_MonthlyPayrollStatus 
        SET F17_TrialStatus = @F17TrialStatus,
            FinalStatus = @FinalStatus,
            DeductionLocked = @DeductionLocked,
            FinalF17_Status = @FinalF17Status,
            PayslipVisibility = @PayslipVisibility,
            LastUpdatedByUserId = @UpdatedBy,
            LastUpdatedOn = @UpdatedOn
        WHERE Id = @Id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@F17TrialStatus", f17TrialStatus);
                    cmd.Parameters.AddWithValue("@FinalStatus", finalStatus);
                    cmd.Parameters.AddWithValue("@DeductionLocked", deductionLocked);
                    cmd.Parameters.AddWithValue("@FinalF17Status", finalF17Status);
                    cmd.Parameters.AddWithValue("@PayslipVisibility", payslipVisibility);
                    cmd.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedOn", updatedOn);
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // Reset edit mode and refresh data
            gvPayrollStatus.EditIndex = -1;
            FilterGridView();
        }


        protected void gvPayrollStatus_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Cancel the editing and exit edit mode
            gvPayrollStatus.EditIndex = -1;
            // Rebind the data
            FilterGridView();
        }

        // Filter GridView based on selected year and month
        protected void FilterGridView()
        {
            string year = DDL_Year.SelectedValue;
            string month = DDL_Month.SelectedValue;

            if (string.IsNullOrEmpty(year) || month == "0")
            {
                // Display an error or message if selections are invalid
                return;
            }

            // Query to fetch data based on selected year and month
            string query = @"
            SELECT * FROM tbl_MonthlyPayrollStatus WHERE PayrollYear = @Year AND PayrollMonth = @Month";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Month", month);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvPayrollStatus.DataSource = dt;
                gvPayrollStatus.DataBind();
            }
        }

        // Update selected records
        protected void GridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateStatus")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvPayrollStatus.Rows[index];

                string payrollRegion = ((Label)row.FindControl("lblPayrollRegion")).Text;
                string payrollCompany = ((Label)row.FindControl("lblPayrollCompany")).Text;
                string f17Status = ((DropDownList)row.FindControl("ddlF17Status")).SelectedValue;
                string finalStatus = ((DropDownList)row.FindControl("ddlFinalStatus")).SelectedValue;
                string deductionLocked = ((DropDownList)row.FindControl("ddlDeductionLocked")).SelectedValue;
                string f17FinalStatus = ((DropDownList)row.FindControl("ddlF17FinalStatus")).SelectedValue;
                string payslipVisibility = ((DropDownList)row.FindControl("ddlPayslipVisibility")).SelectedValue;

                // Execute update query
                string query = @"
                        UPDATE tbl_MonthlyPayrollStatus 
                        SET F17_TrialStatus = @F17Status,
                            FinalStatus = @FinalStatus,
                            DeductionLocked = @DeductionLocked,
                            FinalF17_Status = @F17FinalStatus,
                            PayslipVisibility = @PayslipVisibility,
                            LastUpdatedByUserId = @UserId,
                            LastUpdatedOn = @UpdatedOn
                        WHERE PayrollYear = @Year 
                          AND PayrollMonth = @Month
                          AND PayrollRegion = @Region
                          AND PayrollCompany = @Company";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@F17Status", f17Status);
                    cmd.Parameters.AddWithValue("@FinalStatus", finalStatus);
                    cmd.Parameters.AddWithValue("@DeductionLocked", deductionLocked);
                    cmd.Parameters.AddWithValue("@F17FinalStatus", f17FinalStatus);
                    cmd.Parameters.AddWithValue("@PayslipVisibility", payslipVisibility);
                    cmd.Parameters.AddWithValue("@Year", DDL_Year.SelectedValue);
                    cmd.Parameters.AddWithValue("@Month", DDL_Month.SelectedValue);
                    cmd.Parameters.AddWithValue("@Region", payrollRegion);
                    cmd.Parameters.AddWithValue("@Company", payrollCompany);
                    cmd.Parameters.AddWithValue("@UserId", Session["USERID"].ToString());
                    cmd.Parameters.AddWithValue("@UpdatedOn", DateTime.Now);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Status updated successfully.');", true);
                    conn.Close();
                }

                // Rebind GridView after update
                FilterGridView();
            }
        }

        protected void gvPayrollStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && gvPayrollStatus.EditIndex == e.Row.RowIndex)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                ((DropDownList)e.Row.FindControl("ddlF17TrialStatus")).SelectedValue = rowView["F17_TrialStatus"].ToString();
                ((DropDownList)e.Row.FindControl("ddlFinalStatus")).SelectedValue = rowView["FinalStatus"].ToString();
                ((DropDownList)e.Row.FindControl("ddlDeductionLocked")).SelectedValue = rowView["DeductionLocked"].ToString();
                ((DropDownList)e.Row.FindControl("ddlFinalF17Status")).SelectedValue = rowView["FinalF17_Status"].ToString();
                ((DropDownList)e.Row.FindControl("ddlPayslipVisibility")).SelectedValue = rowView["PayslipVisibility"].ToString();
            }
        }

        protected void gvPayrollStatus_RowDataBound_OLD(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                // Get dropdowns inside the edit row
                DropDownList ddlF17TrialStatus = (DropDownList)e.Row.FindControl("ddlF17TrialStatus");
                DropDownList ddlFinalStatus = (DropDownList)e.Row.FindControl("ddlFinalStatus");
                DropDownList ddlDeductionLocked = (DropDownList)e.Row.FindControl("ddlDeductionLocked");
                DropDownList ddlPayslipVisibility = (DropDownList)e.Row.FindControl("ddlPayslipVisibility");

                // Example data binding (replace with real DB queries if needed)
                if (ddlF17TrialStatus != null)
                {
                    ddlF17TrialStatus.DataSource = GetDropdownData("F17_TrialStatus");
                    ddlF17TrialStatus.DataTextField = "Text";
                    ddlF17TrialStatus.DataValueField = "Value";
                    ddlF17TrialStatus.DataBind();

                    // Set the selected value
                    Label lblF17TrialStatus = (Label)e.Row.FindControl("lblF17TrialStatusHidden");
                    if (lblF17TrialStatus != null)
                        ddlF17TrialStatus.SelectedValue = lblF17TrialStatus.Text;
                }

                if (ddlFinalStatus != null)
                {
                    ddlFinalStatus.DataSource = GetDropdownData("FinalStatus");
                    ddlFinalStatus.DataTextField = "Text";
                    ddlFinalStatus.DataValueField = "Value";
                    ddlFinalStatus.DataBind();

                    Label lblFinalStatus = (Label)e.Row.FindControl("lblFinalStatusHidden");
                    if (lblFinalStatus != null)
                        ddlFinalStatus.SelectedValue = lblFinalStatus.Text;
                }

                if (ddlDeductionLocked != null)
                {
                    ddlDeductionLocked.DataSource = GetDropdownData("DeductionLocked");
                    ddlDeductionLocked.DataTextField = "Text";
                    ddlDeductionLocked.DataValueField = "Value";
                    ddlDeductionLocked.DataBind();

                    Label lblDeductionLocked = (Label)e.Row.FindControl("lblDeductionLockedHidden");
                    if (lblDeductionLocked != null)
                        ddlDeductionLocked.SelectedValue = lblDeductionLocked.Text;
                }

                if (ddlPayslipVisibility != null)
                {
                    ddlPayslipVisibility.DataSource = GetDropdownData("PayslipVisibility");
                    ddlPayslipVisibility.DataTextField = "Text";
                    ddlPayslipVisibility.DataValueField = "Value";
                    ddlPayslipVisibility.DataBind();

                    Label lblPayslipVisibility = (Label)e.Row.FindControl("lblPayslipVisibilityHidden");
                    if (lblPayslipVisibility != null)
                        ddlPayslipVisibility.SelectedValue = lblPayslipVisibility.Text;
                }
            }
        }

        private DataTable GetDropdownData(string type)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text");
            dt.Columns.Add("Value");

            if (type == "F17_TrialStatus" || type == "FinalStatus" || type == "FinalF17_Status")
            {
                dt.Rows.Add("Open", "Open");
                dt.Rows.Add("Locked", "Locked");
            }
            else if (type == "DeductionLocked")
            {
                dt.Rows.Add("Yes", "Yes");
                dt.Rows.Add("No", "No");
            }
            else if (type == "PayslipVisibility")
            {
                dt.Rows.Add("Visible", "Visible");
                dt.Rows.Add("Hidden", "Hidden");
            }

            return dt;
        }


        protected void btn_submit_Click(object sender, EventArgs e)
        {
            FilterGridView();
        }
    }
}