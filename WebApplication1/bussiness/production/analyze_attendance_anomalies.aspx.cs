using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.IO;

namespace WebApplication1.bussiness.production
{
    public partial class analyze_attendance_anomalies : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        protected int AuditMonth { get; private set; }
        protected int AuditYear { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Ensure only Admins/HR can see the Payroll Audit Dashboard
                if (Session["USERID"] == null || !AuthorizationService.CanAccess(AuthorizationFeatureCodes.AttendanceOverride))
                {
                    Response.Redirect("~/login.aspx", false);
                    return;
                }

                // Default to Current Payroll Month
                txtFromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                BindRegionDropdown();
                BindCompanyDropdown();
                LoadDashboard();
            }
        }

        private void BindRegionDropdown()
        {
            string qry = "SELECT Work_Region_Name + ' (' + Work_Region_Code + ')' AS DisplayName, Work_Region_Code FROM tlb_work_state_region ORDER BY Work_Region_Name";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
            {
                ddlRegion.DataSource = cmd.ExecuteReader();
                ddlRegion.DataTextField = "DisplayName";
                ddlRegion.DataValueField = "Work_Region_Code";
                ddlRegion.DataBind();
            }
            dbcl.DisconnectDb();
            ddlRegion.Items.Insert(0, new ListItem("-- ALL REGIONS --", "ALL"));
        }

        private void BindCompanyDropdown()
        {
            string qry = "SELECT DISTINCT Company_Name, Company_Code FROM tlb_workregion_company ORDER BY Company_Name";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
            {
                ddlCompany.DataSource = cmd.ExecuteReader();
                ddlCompany.DataTextField = "Company_Name";
                ddlCompany.DataValueField = "Company_Code";
                ddlCompany.DataBind();
            }
            dbcl.DisconnectDb();
            ddlCompany.Items.Insert(0, new ListItem("-- ALL COMPANIES --", "ALL"));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            hfSelectedAnomaly.Value = "ALL"; // Reset card filter on new date/region search
            LoadDashboard();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtFromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
            txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            ddlRegion.SelectedIndex = 0;
            if (ddlCompany.Items.Count > 0)
                ddlCompany.SelectedIndex = 0;
            hfSelectedAnomaly.Value = "ALL";
            LoadDashboard();
        }

        protected void rptSummary_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "FilterCategory")
            {
                if (hfSelectedAnomaly.Value == e.CommandArgument.ToString())
                    hfSelectedAnomaly.Value = "ALL";
                else
                    hfSelectedAnomaly.Value = e.CommandArgument.ToString();

                LoadDashboard();
            }
        }

        private void LoadDashboard()
        {
            try
            {
                DataTable dtAudit = ExecutePrePayrollAudit();
                LoadSummaryCards(dtAudit);
                LoadAnomalyDetails(dtAudit);

                lblCurrentFilter.Text = hfSelectedAnomaly.Value == "ALL" ? "" : $" (Filtered: {hfSelectedAnomaly.Value})";
            }
            catch (Exception ex)
            {
                LoadSummaryCards(new DataTable());
                LoadAnomalyDetails(new DataTable());
                lblCurrentFilter.Text = " (Audit load failed)";
                string safeMessage = System.Web.HttpUtility.JavaScriptStringEncode(ex.Message);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "AuditSpError",
                    $"showPNotify('Audit Error', '{safeMessage}', 'error');", true);
            }
        }

        private DataTable ExecutePrePayrollAudit()
        {
            DataTable dt = new DataTable();

            object targetYear = DBNull.Value;
            object targetMonth = DBNull.Value;
            ResolveAuditPeriod(out targetYear, out targetMonth);

            string region = ddlRegion.SelectedValue;
            object targetRegion = (string.IsNullOrEmpty(region) || region == "ALL") ? (object)DBNull.Value : region;

            string company = ddlCompany.SelectedValue;
            object targetCompany = (string.IsNullOrEmpty(company) || company == "ALL") ? (object)DBNull.Value : company;

            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            try
            {
                using (SqlCommand cmd = new SqlCommand("USP_Pre_Payroll_Audit", dbcl.Conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.Int).Value = targetYear;
                    cmd.Parameters.Add("@TargetMonth", SqlDbType.Int).Value = targetMonth;
                    cmd.Parameters.Add("@TargetRegion", SqlDbType.VarChar, 50).Value = targetRegion;
                    cmd.Parameters.Add("@TargetCompany", SqlDbType.VarChar, 50).Value = targetCompany;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            finally
            {
                dbcl.DisconnectDb();
            }

            return dt;
        }

        private void ResolveAuditPeriod(out object targetYear, out object targetMonth)
        {
            targetYear = DBNull.Value;
            targetMonth = DBNull.Value;
            AuditYear = DateTime.Now.Year;
            AuditMonth = DateTime.Now.Month;

            DateTime fromDate;
            if (DateTime.TryParse(txtFromDate.Text, out fromDate))
            {
                AuditYear = fromDate.Year;
                AuditMonth = fromDate.Month;
                targetYear = fromDate.Year;
                targetMonth = fromDate.Month;
                return;
            }

            DateTime toDate;
            if (DateTime.TryParse(txtToDate.Text, out toDate))
            {
                AuditYear = toDate.Year;
                AuditMonth = toDate.Month;
                targetYear = toDate.Year;
                targetMonth = toDate.Month;
            }
        }

        private void LoadSummaryCards(DataTable dtAudit)
        {
            DataTable dtSummary = new DataTable();
            dtSummary.Columns.Add("AnomalyType", typeof(string));
            dtSummary.Columns.Add("Anomaly_Type", typeof(string));
            dtSummary.Columns.Add("Anomaly_Count", typeof(int));
            dtSummary.Columns.Add("Severity_Level", typeof(string));
            dtSummary.Columns.Add("Severity_Color", typeof(string));

            if (dtAudit != null && dtAudit.Rows.Count > 0 && dtAudit.Columns.Contains("AnomalyType"))
            {
                DataTable dtDistinct = dtAudit.DefaultView.ToTable(true, "AnomalyType");
                foreach (DataRow typeRow in dtDistinct.Rows)
                {
                    string anomalyType = Convert.ToString(typeRow["AnomalyType"]);
                    if (string.IsNullOrWhiteSpace(anomalyType))
                        continue;

                    int count = 0;
                    string severityFromSp = null;
                    foreach (DataRow row in dtAudit.Rows)
                    {
                        if (string.Equals(Convert.ToString(row["AnomalyType"]), anomalyType, StringComparison.OrdinalIgnoreCase))
                        {
                            count++;
                            if (severityFromSp == null && dtAudit.Columns.Contains("Severity_Level") && row["Severity_Level"] != DBNull.Value)
                                severityFromSp = Convert.ToString(row["Severity_Level"]);
                        }
                    }

                    string severityLevel;
                    string severityColor;
                    MapSeverity(anomalyType, severityFromSp, out severityLevel, out severityColor);

                    DataRow summaryRow = dtSummary.NewRow();
                    summaryRow["AnomalyType"] = anomalyType;
                    summaryRow["Anomaly_Type"] = anomalyType;
                    summaryRow["Anomaly_Count"] = count;
                    summaryRow["Severity_Level"] = severityLevel;
                    summaryRow["Severity_Color"] = severityColor;
                    dtSummary.Rows.Add(summaryRow);
                }
            }

            rptSummary.DataSource = dtSummary;
            rptSummary.DataBind();
        }

        private static void MapSeverity(string anomalyType, string severityFromSp, out string severityLevel, out string severityColor)
        {
            if (!string.IsNullOrWhiteSpace(severityFromSp))
            {
                severityLevel = severityFromSp;
                switch (severityFromSp.Trim().ToLowerInvariant())
                {
                    case "critical":
                        severityColor = "#E74C3C";
                        return;
                    case "high":
                        severityColor = "#E67E22";
                        return;
                    case "medium":
                        severityColor = "#F1C40F";
                        return;
                    default:
                        severityColor = "#3498DB";
                        return;
                }
            }

            string type = (anomalyType ?? string.Empty).ToLowerInvariant();
            if (type.Contains("negative") || type.Contains("zero") || type.Contains("missing") || type.Contains("critical"))
            {
                severityLevel = "Critical";
                severityColor = "#E74C3C";
            }
            else if (type.Contains("excessive") || type.Contains("16") || type.Contains("absent"))
            {
                severityLevel = "High";
                severityColor = "#E67E22";
            }
            else if (type.Contains("ot") || type.Contains("override") || type.Contains("manual"))
            {
                severityLevel = "Medium";
                severityColor = "#F1C40F";
            }
            else if (type.Contains("backdate") || type.Contains("late") || type.Contains("abuse"))
            {
                severityLevel = "Warning";
                severityColor = "#3498DB";
            }
            else
            {
                severityLevel = "High";
                severityColor = "#E67E22";
            }
        }

        private void LoadAnomalyDetails(DataTable dtAudit)
        {
            DataTable dtDetails = dtAudit == null ? new DataTable() : dtAudit.Clone();
            string selected = hfSelectedAnomaly.Value ?? "ALL";

            if (dtAudit != null)
            {
                foreach (DataRow row in dtAudit.Rows)
                {
                    string anomalyType = dtAudit.Columns.Contains("AnomalyType") ? Convert.ToString(row["AnomalyType"]) : string.Empty;
                    if (selected == "ALL" || string.Equals(anomalyType, selected, StringComparison.OrdinalIgnoreCase))
                        dtDetails.ImportRow(row);
                }
            }

            gvAnomalies.DataSource = dtDetails;
            gvAnomalies.DataBind();

            Session["AnomalyExportData"] = dtDetails;
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (Session["AnomalyExportData"] != null)
            {
                DataTable dt = (DataTable)Session["AnomalyExportData"];

                using (XLWorkbook wb = new XLWorkbook())
                {
                    DataTable exportDt = dt.Copy();
                    wb.Worksheets.Add(exportDt, "Attendance_Anomalies");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", $"attachment;filename=Payroll_Anomalies_{DateTime.Now.ToString("yyyyMMdd")}.xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
        }
    }
}
