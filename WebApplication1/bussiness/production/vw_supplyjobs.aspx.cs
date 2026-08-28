using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication1.bussiness.production
{
    public partial class vw_supplyjobs : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {

                    DateTime now = DateTime.Now;

                    lbl_year.Text = now.Year.ToString();
                    lbl_monthcode.Text = now.Month.ToString();
                    lbl_month.Text = now.ToString("MMMM");

                    DateTime startDate = new DateTime(now.Year, now.Month, 1);
                    DateTime endDate = startDate.AddMonths(1);

                    //string CmdString2 = "select * from tbl_jobs where Creator_Workman='" + Session["WORKMAN"].ToString() + "' and Creator_Name='" + Session["USERNAME"].ToString() + "' and YEAR(CreatedDate)='" + DateTime.Now.Year.ToString() + "' and JOBID_Status='Blocked' and MONTH(CreatedDate)='" + DateTime.Now.Month.ToString() + "' and JOBID_Status='Blocked' and EntryExit='Exit' and FinalUpldStatus='Yes' and Incharge_Approval='Approved' and BillingCode='MS' order by CreatedDate desc";

                    string CmdString2 = @"
                        SELECT Id,CreatedDate,Creator_Workman,WorkOrderNo,JOBID,IIF(JOB_Status !='Level1MemoCreated',JOBID_Status,Level1_BillingCode) as JOBID_Status,JOBID_Status,JOB_Site,JOB_InchargeName,JOB_Shift,JOB_Title,JOB_PermitNo,JOB_Status,FinalUpldStatus,Incharge_Approval
                        FROM tbl_jobs
                        WHERE Creator_Workman = @Workman
                          AND Creator_Name = @Username
                          AND CreatedDate >= @StartDate
                          AND CreatedDate < @EndDate
                          AND JOBID_Status = 'Blocked'
                          AND EntryExit = 'Exit'
                          AND FinalUpldStatus = 'Yes'
                          AND Incharge_Approval = 'Approved'
                          AND BillingCode = 'MS'
                        ORDER BY CreatedDate DESC";

                    SqlParameter[] parameters = {
                        new SqlParameter("@Workman", Session["WORKMAN"].ToString()),
                        new SqlParameter("@Username", Session["USERNAME"].ToString()),
                        new SqlParameter("@StartDate", startDate),
                        new SqlParameter("@EndDate", endDate)
                    };

                    BindGrid(CmdString2, parameters);
                }
            }
        }

        private void BindGrid(string cmdString, SqlParameter[] parameters = null)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            dbcl.Conn.Close();
        }

        protected void btn_prevmonth_Click(object sender, EventArgs e)
        {
            Int32 year = Convert.ToInt32(lbl_year.Text.ToString());
            Int32 month = Convert.ToInt32(lbl_monthcode.Text.ToString());

            if (month == 01 || month == 1)
            {
                month = 12;
                year = year - 1;
            }
            else
            {
                month = month - 1;
            }

            string Year = Convert.ToString(year);
            string Month = "";
            if (month <= 9)
            {
                Month = "0" + month.ToString();
            }
            else
            {
                Month = month.ToString();
            }

            GridBinder(Year, Month);
        }

        protected void btn_currentdata_Click(object sender, EventArgs e)
        {

            string Year = DateTime.Now.Year.ToString();
            string Month = DateTime.Now.Month.ToString();

            GridBinder(Year, Month);
        }

        protected void btn_nextmonth_Click(object sender, EventArgs e)
        {
            Int32 year = Convert.ToInt32(lbl_year.Text.ToString());
            Int32 month = Convert.ToInt32(lbl_monthcode.Text.ToString());

            if (month == 12)
            {
                month = 1;
                year = year + 1;
            }
            else
            {
                month = month + 1;
            }

            string Year = Convert.ToString(year);
            string Month = "";
            if (month <= 9)
            {
                Month = "0" + month.ToString();
            }
            else
            {
                Month = month.ToString();
            }

            GridBinder(Year, Month);
        }

        private void GridBinder(string Year, string Month)
        {
            string Monthname = "";
            dbcl.FindMonthName(Month, ref Monthname);
            lbl_month.Text = Monthname;
            lbl_year.Text = Year;
            lbl_monthcode.Text = Month;

            int year = Convert.ToInt32(Year);
            int month = Convert.ToInt32(Month);
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = startDate.AddMonths(1);

            string CmdString2 = @"
                        SELECT Id,CreatedDate,Creator_Workman,WorkOrderNo,JOBID,IIF(JOB_Status !='Level1MemoCreated',JOBID_Status,Level1_BillingCode) as JOBID_Status,JOBID_Status,JOB_Site,JOB_InchargeName,JOB_Shift,JOB_Title,JOB_PermitNo,JOB_Status,FinalUpldStatus,Incharge_Approval
                        FROM tbl_jobs
                        WHERE Creator_Workman = @Workman
                          AND Creator_Name = @Username
                          AND CreatedDate >= @StartDate
                          AND CreatedDate < @EndDate
                          AND JOBID_Status = 'Blocked'
                          AND EntryExit = 'Exit'
                          AND FinalUpldStatus = 'Yes'
                          AND Incharge_Approval = 'Approved'
                          AND BillingCode = 'MS'
                        ORDER BY CreatedDate DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@Workman", Session["WORKMAN"].ToString()),
                new SqlParameter("@Username", Session["USERNAME"].ToString()),
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate)
            };

            BindGrid(CmdString2, parameters);
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                Label lbl_JOBID_Status = (Label)GridView1.Rows[i].FindControl("lbl_JOBID_Status");              //----------------- Active / Blocked
                Label lbl_JOB_InchargeName = (Label)GridView1.Rows[i].FindControl("lbl_JOB_InchargeName");
                Label lbl_Incharge_Approval = (Label)GridView1.Rows[i].FindControl("lbl_Incharge_Approval");    //------------ Approved / Blocked

                Label lbl_JOB_PermitNo = (Label)GridView1.Rows[i].FindControl("lbl_JOB_PermitNo");
                Label lbl_FinalUpldStatus = (Label)GridView1.Rows[i].FindControl("lbl_FinalUpldStatus");        //------------ Yes / No

                Button btn_smemo = (Button)GridView1.Rows[i].FindControl("btn_createsupmemo");

                string jobidstatus = lbl_JOBID_Status.Text.ToString();
                string approvalstatus = lbl_Incharge_Approval.Text.ToString();
                string upldstatus = lbl_FinalUpldStatus.Text.ToString();

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

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //string jobid = Convert.ToString(e.CommandArgument);

            //Determine the RowIndex of the Row whose Button was clicked.
            int rowIndex = Convert.ToInt32(e.CommandArgument);

            //Reference the GridView Row.
            GridViewRow row = GridView1.Rows[rowIndex];

            //Fetch value of Name.
            string dbid = (row.FindControl("lbl_Id") as Label).Text;
            string jobid = (row.FindControl("lbl_JOBID") as Label).Text;
            string supv = (row.FindControl("lbl_Creator_Workman") as Label).Text;
            string jobidstatus = (row.FindControl("lbl_JOBID_Status") as Label).Text;

            if (e.CommandName == "View_Details")
            {
                Response.Redirect("view_jobdetails.aspx?JOBID=" + jobid + "&dbid=" + dbid + "&supv=" + supv);
            }
            else if (e.CommandName == "CSUPMEM")
            {
                //Response.Redirect("create_supplymemo.aspx?JOBID=" + jobid + "&viewid=2");

                //Response.Redirect("create_supplymemo.aspx?JOBID=" + jobid + "&dbid=" + dbid + "&supv=" + supv + "&viewid=2");

                Response.Redirect("create_supplymemo.aspx?JOBID=" + jobid
                 + "&dbid=" + dbid
                 + "&supv=" + supv
                 + "&viewid=2"
                 + "&y=" + lbl_year.Text.ToString()
                 + "&m=" + lbl_monthcode.Text.ToString());

            }
        }
    }
}