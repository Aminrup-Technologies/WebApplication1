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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Ensure only Admins/HR can see the Payroll Audit Dashboard
                if (Session["USERID"] == null || (Session["USERTYPE"].ToString() != "Admin" && Session["USERTYPE"].ToString() != "Office Staff"))
                {
                    Response.Redirect("~/login.aspx", false);
                    return;
                }

                // Default to Current Payroll Month
                txtFromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                BindRegionDropdown();
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
            LoadSummaryCards();
            LoadAnomalyDetails();

            lblCurrentFilter.Text = hfSelectedAnomaly.Value == "ALL" ? "" : $" (Filtered: {hfSelectedAnomaly.Value})";
        }

        private void LoadSummaryCards()
        {
            string qry = @"
                SELECT 
                    Anomaly_Type,
                    COUNT(Id) AS Anomaly_Count,
                    Severity_Level,
                    CASE Severity_Level 
                        WHEN 'Critical' THEN '#E74C3C' 
                        WHEN 'High' THEN '#E67E22' 
                        WHEN 'Medium' THEN '#F1C40F' 
                        ELSE '#3498DB' 
                    END AS Severity_Color
                FROM (
                    SELECT 
                        Id,
                        CASE 
                            WHEN Outpunch_Time IS NOT NULL AND Outpunch_Time <= Inpunch_Time THEN 'Negative or Zero Shift Time'
                            WHEN ISNULL(WorkedHours, 0) > 16 THEN 'Excessive Shift (>16 Hours)'
                            WHEN ISNULL(ProvidedOT, 0) > ISNULL(Calc_OT, 0) THEN 'Manual OT Exceeds System OT'
                            WHEN AttendanceCode IN ('A', 'Ab') AND ISNULL(WorkedHours, 0) > 0 THEN 'Absent Code but Hours Logged'
                            WHEN AttendanceCode NOT IN ('A', 'Ab', 'OD', 'FL') AND Outpunch_Time IS NULL THEN 'Present Code but Missing OUT-Punch'
                            WHEN DATEDIFF(DAY, CreatedDate, CAST(TimeStamp AS DATE)) > 3 THEN 'Late System Entry (>3 Days Backdated)'
                            ELSE 'Clean'
                        END AS Anomaly_Type,
                        CASE 
                            WHEN Outpunch_Time IS NOT NULL AND Outpunch_Time <= Inpunch_Time THEN 'Critical'
                            WHEN ISNULL(WorkedHours, 0) > 16 THEN 'High'
                            WHEN ISNULL(ProvidedOT, 0) > ISNULL(Calc_OT, 0) THEN 'Medium'
                            WHEN AttendanceCode IN ('A', 'Ab') AND ISNULL(WorkedHours, 0) > 0 THEN 'High'
                            WHEN AttendanceCode NOT IN ('A', 'Ab', 'OD', 'FL') AND Outpunch_Time IS NULL THEN 'Critical'
                            WHEN DATEDIFF(DAY, CreatedDate, CAST(TimeStamp AS DATE)) > 3 THEN 'Warning'
                            ELSE 'Clean'
                        END AS Severity_Level
                    FROM tbl_attendance
                    WHERE SiteIncharge_Approval = 'Approved' 
                      AND ISNULL(DeleteStatus, 0) = 0
                      AND CONVERT(date, CreatedDate) BETWEEN @FromDate AND @ToDate
                      AND (@Region = 'ALL' OR JOB_Region = @Region)
                ) AS AuditData
                WHERE Anomaly_Type != 'Clean'
                GROUP BY Anomaly_Type, Severity_Level
                ORDER BY CASE Severity_Level WHEN 'Critical' THEN 1 WHEN 'High' THEN 2 WHEN 'Medium' THEN 3 ELSE 4 END;";

            SqlParameter[] param = {
                new SqlParameter("@FromDate", txtFromDate.Text),
                new SqlParameter("@ToDate", txtToDate.Text),
                new SqlParameter("@Region", ddlRegion.SelectedValue)
            };

            rptSummary.DataSource = dbcl.SPreturn_dt(qry, param);
            rptSummary.DataBind();
        }

        private void LoadAnomalyDetails()
        {
            string qry = @"
                SELECT * FROM (
                    SELECT 
                        a.JOBID,
                        CONVERT(VARCHAR, a.CreatedDate, 106) AS Job_Date,
                        a.JOB_Region,
                        a.JOB_SiteName AS Worksite,
                        
                        a.Creator_Name + ' [' + a.Creator_Workman + ']' AS Creator_Details,
                        a.JOB_InchargeName + ' [' + a.JOB_InchargeWrk + ']' AS Approver_Details,
                        
                        a.EmployeeName + ' [' + a.EmployeeWrk + ']' AS Employee_Details,
                        a.EmployeeWrk, 
                        MONTH(a.CreatedDate) AS CreatedMonth, 
                        YEAR(a.CreatedDate) AS CreatedYear,

                        a.AttendanceCode,
                        FORMAT(a.Inpunch_Time, 'dd-MMM-yyyy hh:mm tt') AS IN_Time,
                        ISNULL(FORMAT(a.Outpunch_Time, 'dd-MMM-yyyy hh:mm tt'), 'MISSING OUT-PUNCH') AS OUT_Time,
                        a.WorkedHours,
                        a.WourkHours AS Registered_Hours,
                        a.Calc_OT AS System_OT,
                        a.ProvidedOT AS Final_OT,
                        FORMAT(a.TimeStamp, 'dd-MMM-yyyy hh:mm tt') AS System_Entry_Time,
                        
                        CASE 
                            WHEN a.Outpunch_Time IS NOT NULL AND a.Outpunch_Time <= a.Inpunch_Time THEN 'Negative or Zero Shift Time'
                            WHEN ISNULL(a.WorkedHours, 0) > 16 THEN 'Excessive Shift (>16 Hours)'
                            WHEN ISNULL(a.ProvidedOT, 0) > ISNULL(a.Calc_OT, 0) THEN 'Manual OT Exceeds System OT'
                            WHEN a.AttendanceCode IN ('A', 'Ab') AND ISNULL(a.WorkedHours, 0) > 0 THEN 'Absent Code but Hours Logged'
                            WHEN a.AttendanceCode NOT IN ('A', 'Ab', 'OD', 'FL') AND a.Outpunch_Time IS NULL THEN 'Present Code but Missing OUT-Punch'
                            WHEN DATEDIFF(DAY, a.CreatedDate, CAST(a.TimeStamp AS DATE)) > 3 THEN 'Late System Entry (>3 Days Backdated)'
                            ELSE 'Clean'
                        END AS Anomaly_Type

                    FROM tbl_attendance a
                    WHERE a.SiteIncharge_Approval = 'Approved' 
                      AND ISNULL(a.DeleteStatus, 0) = 0
                      AND CONVERT(date, a.CreatedDate) BETWEEN @FromDate AND @ToDate
                      AND (@Region = 'ALL' OR a.JOB_Region = @Region)
                ) AS FinalData 
                WHERE Anomaly_Type != 'Clean' 
                  AND (@Category = 'ALL' OR Anomaly_Type = @Category)
                ORDER BY Job_Date DESC, JOBID;";

            SqlParameter[] param = {
                new SqlParameter("@FromDate", txtFromDate.Text),
                new SqlParameter("@ToDate", txtToDate.Text),
                new SqlParameter("@Region", ddlRegion.SelectedValue),
                new SqlParameter("@Category", hfSelectedAnomaly.Value)
            };

            DataTable dtDetails = dbcl.SPreturn_dt(qry, param);
            gvAnomalies.DataSource = dtDetails;
            gvAnomalies.DataBind();

            // Store for Excel Export
            Session["AnomalyExportData"] = dtDetails;
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (Session["AnomalyExportData"] != null)
            {
                DataTable dt = (DataTable)Session["AnomalyExportData"];

                using (XLWorkbook wb = new XLWorkbook())
                {
                    // Clean up hidden columns before export
                    DataTable exportDt = dt.Copy();
                    if (exportDt.Columns.Contains("EmployeeWrk")) exportDt.Columns.Remove("EmployeeWrk");
                    if (exportDt.Columns.Contains("CreatedMonth")) exportDt.Columns.Remove("CreatedMonth");
                    if (exportDt.Columns.Contains("CreatedYear")) exportDt.Columns.Remove("CreatedYear");

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