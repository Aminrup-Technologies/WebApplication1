using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.IO;

namespace WebApplication1.bussiness.production
{
    public partial class manage_job_exceptions : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || (Session["USERTYPE"].ToString() != "Admin" && Session["USERTYPE"].ToString() != "Office Staff"))
                {
                    Response.Redirect("~/login.aspx", false);
                    return;
                }

                // Initialize Dates to Current Month
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
            hfSelectedCategory.Value = "ALL"; // Reset card filter on new search
            LoadDashboard();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtFromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
            txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            ddlRegion.SelectedIndex = 0;
            hfSelectedCategory.Value = "ALL";
            LoadDashboard();
        }

        protected void rptSummary_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "FilterCategory")
            {
                // Toggle logic: If clicking the already selected card, reset to ALL. Otherwise, set to selected.
                if (hfSelectedCategory.Value == e.CommandArgument.ToString())
                    hfSelectedCategory.Value = "ALL";
                else
                    hfSelectedCategory.Value = e.CommandArgument.ToString();

                LoadDashboard();
            }
        }

        private void LoadDashboard()
        {
            LoadSummaryCards();
            LoadExceptionDetails();

            lblCurrentFilter.Text = hfSelectedCategory.Value == "ALL" ? "" : $" (Filtered: {hfSelectedCategory.Value})";
        }

        private void LoadSummaryCards()
        {
            string qry = @"
        -- 1. Grand Total Card
        SELECT 
            'Total Pending Exceptions' AS Workflow_Stage, 
            COUNT(1) AS Total_Jobs, 
            0 AS SortOrder
        FROM tbl_jobs
        WHERE CONVERT(date, CreatedDate) BETWEEN @FromDate AND @ToDate
          AND (@Region = 'ALL' OR JOB_Region = @Region)
          AND Incharge_Approval != 'Approved' 
          AND ISNULL(DeleteStatus, 0) = 0
          
        UNION ALL
        
        -- 2. Categorized Cards
        SELECT 
            Workflow_Stage, 
            COUNT(1) AS Total_Jobs, 
            1 AS SortOrder
        FROM (
            SELECT 
                CASE 
                    WHEN ISNULL(DeleteStatus, 0) = 1 THEN '0 - Deleted/Cancelled'
                    WHEN Incharge_Approval = 'Approved' THEN '6 - Fully Approved (Closed)'
                    WHEN MasterStatusCode = '4' AND EntryExit = 'Exit' THEN '5 - Awaiting Final Approval'
                    WHEN Incharge_Approval IN ('Rejected', 'Returned') THEN '5b - Rejected by Approver'
                    WHEN IsBlocked = 1 OR JOBID_Status = 'Blocked' THEN 'X - Blocked (72h Auto-Lock)' 
                    WHEN MasterStatusCode = '3' AND EntryExit = 'Entry' THEN '4 - Zombie Shift (Forgot OUT-Punch)'
                    WHEN MasterStatusCode = '3' AND EntryExit = 'Created' THEN '3 - Ready for IN-Punch (Never Started)'
                    WHEN MasterStatusCode = '1' THEN '2 - Awaiting Permit Upload'
                    ELSE '1 - Unknown State'
                END AS Workflow_Stage
            FROM tbl_jobs
            WHERE CONVERT(date, CreatedDate) BETWEEN @FromDate AND @ToDate
              AND (@Region = 'ALL' OR JOB_Region = @Region)
              AND Incharge_Approval != 'Approved' 
              AND ISNULL(DeleteStatus, 0) = 0
        ) AS SubQuery
        GROUP BY Workflow_Stage 
        ORDER BY SortOrder ASC, Workflow_Stage ASC;";

            SqlParameter[] param = {
        new SqlParameter("@FromDate", txtFromDate.Text),
        new SqlParameter("@ToDate", txtToDate.Text),
        new SqlParameter("@Region", ddlRegion.SelectedValue)
    };

            rptSummary.DataSource = dbcl.SPreturn_dt(qry, param);
            rptSummary.DataBind();
        }

        private void LoadExceptionDetails()
        {
            string qry = @"
        SELECT * FROM (
            SELECT 
                JOBID, 
                CONVERT(VARCHAR, CreatedDate, 106) AS Job_Date, 
                JOB_Region, 
                JOB_Site AS Worksite,
                (Creator_Name + ' [' + Creator_Workman + ']') AS Creator_Details,
                (JOB_InchargeName + ' [' + JOB_InchargeWrk + ']') AS Approver_Details,
                DATEDIFF(DAY, CreatedDate, GETDATE()) AS Days_Aging,
                CASE 
                    WHEN ISNULL(DeleteStatus, 0) = 1 THEN '0 - Deleted/Cancelled'
                    WHEN Incharge_Approval = 'Approved' THEN '6 - Fully Approved (Closed)'
                    WHEN MasterStatusCode = '4' AND EntryExit = 'Exit' THEN '5 - Awaiting Final Approval'
                    WHEN Incharge_Approval IN ('Rejected', 'Returned') THEN '5b - Rejected by Approver'
                    WHEN IsBlocked = 1 OR JOBID_Status = 'Blocked' THEN 'X - Blocked (72h Auto-Lock)' 
                    WHEN MasterStatusCode = '3' AND EntryExit = 'Entry' THEN '4 - Zombie Shift (Forgot OUT-Punch)'
                    WHEN MasterStatusCode = '3' AND EntryExit = 'Created' THEN '3 - Ready for IN-Punch (Never Started)'
                    WHEN MasterStatusCode = '1' THEN '2 - Awaiting Permit Upload'
                    ELSE '1 - Unknown State'
                END AS Workflow_Stage,
                CASE 
                    WHEN IsBlocked = 1 THEN 'Action: Use [Unblock JOB]'
                    WHEN Incharge_Approval IN ('Rejected', 'Returned') THEN 'Action: Use [Fix & Resubmit]'
                    WHEN MasterStatusCode = '4' AND EntryExit = 'Exit' THEN 'Action: Chase Approver'
                    WHEN MasterStatusCode = '3' AND EntryExit = 'Entry' THEN 'Action: Use [Force OUT-Punch]'
                    WHEN MasterStatusCode = '3' AND EntryExit = 'Created' THEN 'Action: Use [Delete JOB]'
                    WHEN MasterStatusCode = '1' THEN 'Action: Use [Delete JOB] or [Upload Permit]'
                    ELSE 'Review Manually'
                END AS Recommended_Admin_Action
            FROM tbl_jobs
            WHERE CONVERT(date, CreatedDate) BETWEEN @FromDate AND @ToDate
              AND (@Region = 'ALL' OR JOB_Region = @Region)
              AND Incharge_Approval != 'Approved'
              AND ISNULL(DeleteStatus, 0) = 0
        ) AS FinalData 
        WHERE (@Category = 'ALL' OR @Category = 'Total Pending Exceptions' OR Workflow_Stage = @Category)
        ORDER BY Days_Aging DESC;";

            SqlParameter[] param = {
        new SqlParameter("@FromDate", txtFromDate.Text),
        new SqlParameter("@ToDate", txtToDate.Text),
        new SqlParameter("@Region", ddlRegion.SelectedValue),
        new SqlParameter("@Category", hfSelectedCategory.Value)
    };

            DataTable dtDetails = dbcl.SPreturn_dt(qry, param);
            gvExceptions.DataSource = dtDetails;
            gvExceptions.DataBind();

            // Store in session for Excel Export without re-querying
            Session["ExceptionExportData"] = dtDetails;
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (Session["ExceptionExportData"] != null)
            {
                DataTable dt = (DataTable)Session["ExceptionExportData"];

                using (XLWorkbook wb = new XLWorkbook())
                {
                    // Remove the "Recommended_Admin_Action" column if you don't want it in the clean ledger export
                    DataTable exportDt = dt.Copy();
                    exportDt.Columns.Remove("Recommended_Admin_Action");

                    wb.Worksheets.Add(exportDt, "Exceptions_Ledger");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", $"attachment;filename=Job_Exceptions_{DateTime.Now.ToString("yyyyMMdd")}.xlsx");

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