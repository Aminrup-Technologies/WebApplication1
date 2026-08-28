using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace WebApplication1.bussiness.production
{
    public partial class vw_inchsupmem : System.Web.UI.Page
    {
        public static string yr = string.Empty;
        public static string mnt = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 1. Security & Session Check
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("~/login.aspx", false);
                    return;
                }

                // 2. Determine Year and Month (from QueryString or Current Date)
                yr = Request.QueryString["y"];
                mnt = Request.QueryString["m"];

                int targetYear = DateTime.Now.Year;
                int targetMonth = DateTime.Now.Month;

                if (!string.IsNullOrEmpty(yr) && !string.IsNullOrEmpty(mnt))
                {
                    int.TryParse(yr, out targetYear);
                    int.TryParse(mnt, out targetMonth);
                }

                // 3. Load the Data
                LoadData(targetYear, targetMonth);
            }
        }

        // ==========================================
        // UNIFIED DATA LOADER
        // ==========================================
        private void LoadData(int year, int month)
        {
            // Set UI Labels
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = startDate.AddMonths(1);

            lbl_year.Text = year.ToString();
            lbl_monthcode.Text = month.ToString("00"); // Keeps format consistent (e.g., "05")
            lbl_month.Text = startDate.ToString("MMMM");

            // Parameterized Query (JOB_InchargeName filter removed)
            string query = @"
                SELECT
                    Id, CreatedDate, Creator_Workman, Creator_Name, WorkOrderNo, JOBID,
                    IIF(JOB_Status != 'Level1MemoCreated', JOBID_Status, Level1_BillingCode) AS JOBID_Status,
                    JOBID_Status as Orig_JOBID_Status, JOB_Site, JOB_InchargeName, JOB_Shift, JOB_Title, JOB_PermitNo,
                    JOB_Status, FinalUpldStatus, Incharge_Approval
                FROM tbl_jobs
                WHERE JOB_InchargeWrk = @Workman
                  AND CreatedDate >= @StartDate
                  AND CreatedDate < @EndDate
                  AND JOBID_Status = 'Blocked'
                  AND EntryExit = 'Exit'
                  AND FinalUpldStatus = 'Yes'
                  AND Incharge_Approval = 'Approved'
                  AND BillingCode = 'MS'
                ORDER BY CreatedDate DESC";

            // @Username parameter removed
            SqlParameter[] parameters = {
                new SqlParameter("@Workman", Session["WORKMAN"].ToString()),
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate)
            };

            BindGridSafe(query, parameters);
        }

        // ==========================================
        // SECURE GRID BINDER
        // ==========================================
        private void BindGridSafe(string cmdString, SqlParameter[] parameters)
        {
            string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(cmdString, con))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    try
                    {
                        con.Open();
                        using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            ad.Fill(ds);
                            GridView1.DataSource = ds;
                            GridView1.DataBind();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the error safely
                        string title = "Error loading data";
                        string message = ex.Message.Replace("'", "\\'"); // Escape quotes for JS
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", $"ShowPopup('{title}', '{message}');", true);
                    }
                }
            }
        }

        // ==========================================
        // EFFICIENT ROW DATA BOUND (O(1) per row)
        // ==========================================
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Only execute for Data Rows (ignores Headers/Footers)
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbl_JOBID_Status = (Label)e.Row.FindControl("lbl_JOBID_Status");
                Label lbl_JOB_InchargeName = (Label)e.Row.FindControl("lbl_JOB_InchargeName");
                Label lbl_Incharge_Approval = (Label)e.Row.FindControl("lbl_Incharge_Approval");
                Label lbl_JOB_PermitNo = (Label)e.Row.FindControl("lbl_JOB_PermitNo");
                Label lbl_FinalUpldStatus = (Label)e.Row.FindControl("lbl_FinalUpldStatus");
                Button btn_smemo = (Button)e.Row.FindControl("btn_createsupmemo");

                if (lbl_JOBID_Status != null && lbl_Incharge_Approval != null && lbl_FinalUpldStatus != null && btn_smemo != null)
                {
                    string jobidstatus = lbl_JOBID_Status.Text;
                    string approvalstatus = lbl_Incharge_Approval.Text;
                    string upldstatus = lbl_FinalUpldStatus.Text;

                    if (jobidstatus == "Blocked" && approvalstatus == "Approved" && upldstatus == "Yes")
                    {
                        lbl_JOB_InchargeName.ForeColor = System.Drawing.Color.Green;
                        lbl_JOB_PermitNo.ForeColor = System.Drawing.Color.Green;
                        btn_smemo.Enabled = true;
                    }
                    else if (jobidstatus != "Blocked" && approvalstatus == "Approved" && upldstatus == "Yes")
                    {
                        btn_smemo.Enabled = true;
                        btn_smemo.Text = jobidstatus;
                        btn_smemo.CssClass = "btn btn-sm btn-success";
                    }
                    else
                    {
                        lbl_JOB_InchargeName.ForeColor = System.Drawing.Color.Red;
                        lbl_JOB_PermitNo.ForeColor = System.Drawing.Color.Red;
                        btn_smemo.Enabled = false;
                    }
                }
            }
        }

        // ==========================================
        // ROW COMMAND HANDLER
        // ==========================================
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View_Details" || e.CommandName == "CSUPMEM")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GridView1.Rows[rowIndex];

                string dbid = (row.FindControl("lbl_Id") as Label).Text;
                string jobid = (row.FindControl("lbl_JOBID") as Label).Text;
                string supv = (row.FindControl("lbl_Creator_Workman") as Label).Text;

                if (e.CommandName == "View_Details")
                {
                    Response.Redirect($"view_jobdetails.aspx?JOBID={jobid}&dbid={dbid}&supv={supv}", false);
                }
                else if (e.CommandName == "CSUPMEM")
                {
                    Response.Redirect($"create_supplymemo.aspx?JOBID={jobid}&dbid={dbid}&supv={supv}&viewid=1&y={lbl_year.Text}&m={lbl_monthcode.Text}", false);
                }
            }
        }

        // ==========================================
        // NAVIGATION BUTTONS
        // ==========================================
        protected void btn_prevmonth_Click(object sender, EventArgs e)
        {
            int year = Convert.ToInt32(lbl_year.Text);
            int month = Convert.ToInt32(lbl_monthcode.Text);

            if (month == 1)
            {
                month = 12;
                year -= 1;
            }
            else
            {
                month -= 1;
            }

            LoadData(year, month);
        }

        protected void btn_currentdata_Click(object sender, EventArgs e)
        {
            LoadData(DateTime.Now.Year, DateTime.Now.Month);
        }

        protected void btn_nextmonth_Click(object sender, EventArgs e)
        {
            int year = Convert.ToInt32(lbl_year.Text);
            int month = Convert.ToInt32(lbl_monthcode.Text);

            if (month == 12)
            {
                month = 1;
                year += 1;
            }
            else
            {
                month += 1;
            }

            LoadData(year, month);
        }
    }
}