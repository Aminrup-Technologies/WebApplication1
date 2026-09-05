using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class view_approvedjobs : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("~/login.aspx", false);
                    return;
                }

                string CmdString = "SELECT BilingType, BillingCode FROM tlb_JOB_BillingType ORDER BY Id";
                Bind_BillingType(CmdString);

                DateTime now = DateTime.Now;
                lbl_year.Text = now.Year.ToString();
                lbl_monthcode.Text = now.Month.ToString();
                lbl_month.Text = now.ToString("MMMM");

                // Initial Load
                GridBinder(lbl_year.Text, lbl_monthcode.Text);
            }
        }

        private void Bind_BillingType(string CmdString)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn))
                {
                    Cmd.CommandType = CommandType.Text;
                    using (SqlDataReader rdr = Cmd.ExecuteReader())
                    {
                        DDL_BillingType.DataSource = rdr;
                        DDL_BillingType.DataTextField = "BilingType";
                        DDL_BillingType.DataValueField = "BillingCode";
                        DDL_BillingType.DataBind();
                    }
                }
                DDL_BillingType.Items.Insert(0, new ListItem("Please Select Option", "0"));
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        // ==========================================
        // SMART DYNAMIC GRID BINDER
        // ==========================================
        private void GridBinder(string Year, string Month)
        {
            string Monthname = "";
            dbcl.FindMonthName(Month, ref Monthname);
            lbl_month.Text = Monthname;
            lbl_year.Text = Year;
            lbl_monthcode.Text = Month;

            // 1. Build Base Query
            string query = @"SELECT * FROM tbl_jobs 
                             AND Incharge_Approval = 'Approved' 
                             AND YEAR(CreatedDate) = @Year 
                             AND MONTH(CreatedDate) = @Month ";

            // 2. Append Dynamic Filters
            if (DDL_JobStatus.SelectedIndex > 0 && DDL_JobStatus.SelectedValue != "0")
            {
                switch (DDL_JobStatus.SelectedValue)
                {
                    case "1": query += " AND JOBID_Status='Active' "; break;
                    case "2": query += " AND JOBID_Status!='Active' "; break;
                    case "3": query += " AND FinalUpldStatus!='Yes' "; break;
                    case "4": query += " AND FinalUpldStatus='Yes' "; break;
                    case "5": query += " AND Incharge_Approval='Pending' "; break;
                    case "6": query += " AND Incharge_Approval='Approved' "; break;
                    case "7": query += " AND Incharge_Approval='Returned' "; break;
                    case "8": query += " AND Incharge_Approval='Rejected' "; break;
                }
            }

            if (DDL_BillingType.SelectedIndex > 0)
            {
                query += " AND BillingCode = @BillingCode ";
            }

            query += " ORDER BY CreatedDate DESC";

            // 3. Execute Parameterized Query
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Workman", Session["WORKMAN"].ToString());
                    cmd.Parameters.AddWithValue("@Year", Year);
                    cmd.Parameters.AddWithValue("@Month", Month);

                    if (DDL_BillingType.SelectedIndex > 0)
                    {
                        cmd.Parameters.AddWithValue("@BillingCode", DDL_BillingType.SelectedValue);
                    }

                    using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        ad.Fill(ds);
                        GridView1.DataSource = ds;
                        GridView1.DataBind();
                    }
                }
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        // ==========================================
        // ROW DATA BOUND (Optimized O(1) per row)
        // ==========================================
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbl_JOBID_Status = (Label)e.Row.FindControl("lbl_JOBID_Status");
                Label lbl_JOB_InchargeName = (Label)e.Row.FindControl("lbl_JOB_InchargeName");
                Label lbl_Incharge_Approval = (Label)e.Row.FindControl("lbl_Incharge_Approval");
                Label lbl_JOB_PermitNo = (Label)e.Row.FindControl("lbl_JOB_PermitNo");
                Label lbl_FinalUpldStatus = (Label)e.Row.FindControl("lbl_FinalUpldStatus");
                Button btn_status = (Button)e.Row.FindControl("btn_jobidstatus");

                if (lbl_JOBID_Status != null && btn_status != null)
                {
                    if (lbl_JOBID_Status.Text == "Active")
                        btn_status.CssClass = "btn btn-success btn-sm";
                    else
                        btn_status.CssClass = "btn btn-danger btn-sm";
                }

                if (lbl_Incharge_Approval != null)
                {
                    if (lbl_Incharge_Approval.Text == "Approved")
                    {
                        lbl_Incharge_Approval.ForeColor = System.Drawing.Color.DarkSeaGreen;
                        lbl_JOB_InchargeName.ForeColor = System.Drawing.Color.DarkSeaGreen;
                    }
                    else
                    {
                        lbl_Incharge_Approval.ForeColor = System.Drawing.Color.Red;
                        lbl_JOB_InchargeName.ForeColor = System.Drawing.Color.Red;
                    }
                }

                if (lbl_JOB_PermitNo != null && lbl_FinalUpldStatus != null)
                {
                    if (lbl_FinalUpldStatus.Text == "Yes")
                        lbl_JOB_PermitNo.ForeColor = System.Drawing.Color.DarkSeaGreen;
                    else
                        lbl_JOB_PermitNo.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View_Details")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GridView1.Rows[rowIndex];

                string dbid = (row.FindControl("lbl_Id") as Label).Text;
                string jobid = (row.FindControl("lbl_JOBID") as Label).Text;
                string supv = (row.FindControl("lbl_Creator_Workman") as Label).Text;

                Response.Redirect("view_jobdetails.aspx?JOBID=" + jobid + "&dbid=" + dbid + "&supv=" + supv, false);
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

            GridBinder(year.ToString(), month.ToString("00"));
        }

        protected void btn_currentdata_Click(object sender, EventArgs e)
        {
            GridBinder(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString("00"));
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

            GridBinder(year.ToString(), month.ToString("00"));
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            DDL_JobStatus.SelectedIndex = 0;
            DDL_BillingType.SelectedIndex = 0;
            GridBinder(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString("00"));
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            GridBinder(lbl_year.Text, lbl_monthcode.Text);
        }
    }
}