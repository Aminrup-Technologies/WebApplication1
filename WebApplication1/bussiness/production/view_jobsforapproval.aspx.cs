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
    public partial class approve_jobandmanpower : System.Web.UI.Page
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

                    lbl_year.Text = DateTime.Now.Year.ToString();
                    lbl_monthcode.Text = DateTime.Now.Month.ToString();
                    lbl_month.Text = now.ToString("MMMM");

                    DateTime startDate = new DateTime(now.Year, now.Month, 1);
                    DateTime endDate = startDate.AddMonths(1);

                    string CmdString2 = @"
                        SELECT *
                        FROM tbl_jobs
                        WHERE JOB_InchargeWrk = @Workman
                          AND JOB_Status = 'Out-Punch Done'
                          AND EntryExit = 'Exit'
                          AND CreatedDate >= @StartDate
                          AND CreatedDate < @EndDate
                        ORDER BY CreatedDate DESC";

                    SqlParameter[] parameters = {
                        new SqlParameter("@Workman", Session["WORKMAN"].ToString()),
                        new SqlParameter("@StartDate", startDate),
                        new SqlParameter("@EndDate", endDate)
                    };

                    BindGrid(CmdString2, parameters);

                    //string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_Status='Out-Punch Done' and EntryExit='Exit' order by CreatedDate desc";
                    //BindGrid(CmdString2);

                    //Label lbl_pendingforappjob = (Label)Page.Master.FindControl("lbl_jobspendingcount");
                    //lbl_pendingforappjob.Text = Convert.ToString(CC.GetPendingJOBApprovalCount(Session["WORKMAN"].ToString()));

                    string CmdString = "select BilingType, BillingCode from tlb_JOB_BillingType order by Id";
                    Bind_BillingType(CmdString);
                }
            }
        }

        private void Bind_BillingType(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_BillingType.DataSource = Cmd.ExecuteReader();
            DDL_BillingType.DataTextField = "BilingType";
            DDL_BillingType.DataValueField = "BillingCode";
            DDL_BillingType.DataBind();
            DDL_BillingType.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
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

        protected void JOBID_Delete(string id, string dbcode)
        {
            try
            {
                Delete_from_JOBTable(id, dbcode);
                Delete_from_AttendanceTable(dbcode);


                string title = "Notifications :";
                string body = "Data Deleted Successfully";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }

            GridBinder(lbl_year.Text, lbl_monthcode.Text);
        }


        private void Delete_from_JOBTable(string id, string dbcode)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "delete from tbl_jobs where Id=@Id and JOBID=@JOBID";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@JOBID", dbcode);
                cmd.ExecuteNonQuery();
                dbcl.Conn.Close();
            }
            catch (Exception ex)
            {

                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void Delete_from_AttendanceTable(string dbcode)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "delete from tbl_attendance where JOBID=@JOBID";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@JOBID", dbcode);
                cmd.ExecuteNonQuery();
                dbcl.Conn.Close();
            }
            catch (Exception ex)
            {

                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                Label lbl_JOBID_Status = (Label)GridView1.Rows[i].FindControl("lbl_JOBID_Status");
                Label lbl_JOB_InchargeName = (Label)GridView1.Rows[i].FindControl("lbl_JOB_InchargeName");
                Label lbl_Incharge_Approval = (Label)GridView1.Rows[i].FindControl("lbl_Incharge_Approval");

                Label lbl_JOB_PermitNo = (Label)GridView1.Rows[i].FindControl("lbl_JOB_PermitNo");
                Label lbl_FinalUpldStatus = (Label)GridView1.Rows[i].FindControl("lbl_FinalUpldStatus");

                Button btn_viewdetails = (Button)GridView1.Rows[i].FindControl("btn_viewdetails");
                Button btn_status = (Button)GridView1.Rows[i].FindControl("btn_jobidstatus");

                string jobidstatus = lbl_JOBID_Status.Text.ToString();
                string approvalstatus = lbl_Incharge_Approval.Text.ToString();

                string lblupldstatus = lbl_FinalUpldStatus.Text.ToString();

                if (jobidstatus == "Active")
                {
                    btn_status.CssClass = "btn btn-success btn-sm";
                    btn_viewdetails.Enabled = false;
                    btn_status.Enabled = true;
                }
                else
                {
                    btn_status.CssClass = "btn btn-sm btn-danger";
                    btn_viewdetails.Enabled = true;
                    btn_status.Enabled = false;
                }

                if (approvalstatus == "Approved")
                {
                    lbl_Incharge_Approval.ForeColor = System.Drawing.Color.DarkSeaGreen;
                    lbl_JOB_InchargeName.ForeColor = System.Drawing.Color.DarkSeaGreen;
                    btn_status.Enabled = false;
                }
                else
                {
                    lbl_Incharge_Approval.ForeColor = System.Drawing.Color.Red;
                    lbl_JOB_InchargeName.ForeColor = System.Drawing.Color.Red;
                    btn_status.Enabled = true;
                }

                if (lblupldstatus == "Yes")
                {
                    lbl_JOB_PermitNo.ForeColor = System.Drawing.Color.DarkSeaGreen;
                }
                else
                {
                    lbl_JOB_PermitNo.ForeColor = System.Drawing.Color.Red;
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

            if (e.CommandName == "Swap_JOBIDStatus")
            {
                if (jobidstatus == "Blocked")
                {
                    JOBID_Status_Swaper(jobid, dbid);
                    //Response.Redirect(Request.Url.AbsoluteUri);

                    string title = "Notification :";
                    string body = "JOBID Status Changed";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    if (CC.CheckforPendingOUT(jobid, supv) == 0)
                    {
                        if (CC.CheckforPendingPermit(jobid, supv) == 0)
                        {
                            JOBID_Status_Swaper(jobid, dbid);
                            //Response.Redirect(Request.Url.AbsoluteUri);
                        }
                        else
                        {
                            string title = "Notification :";
                            string body = "Permit NOT Uploaded Yet";
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                        }
                    }
                    else
                    {
                        string title = "Notification :";
                        string body = "Their are Pending OUT Punch";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }

            }
            else if (e.CommandName == "Approve")
            {
                Response.Redirect("jobapprovalpage.aspx?JOBID=" + jobid + "");
            }
            else if (e.CommandName == "Delete")
            {
                JOBID_Delete(dbid, jobid);
            }
        }

        private void JOBID_Status_Swaper(string jobid, string dbid)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string statusdata = "";
            string statusdata1 = "";
            string cmdstring = "select JOBID_Status from tbl_jobs where JOBID=@JOBID and Id=@Id";
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            cmd.Parameters.AddWithValue("@JOBID", jobid);
            cmd.Parameters.AddWithValue("@Id", dbid);
            SqlDataReader re = cmd.ExecuteReader();
            if (re.Read())
            {
                statusdata = re["JOBID_Status"].ToString();
            }
            re.Close();


            if (statusdata == "Active")
            {
                statusdata1 = "Blocked";

                SqlCommand upd = new SqlCommand("update tbl_jobs set JOBID_Status=@JOBID_Status where JOBID=@JOBID and Id=@Id", dbcl.Conn);
                upd.Parameters.AddWithValue("@JOBID_Status", statusdata1);
                upd.Parameters.AddWithValue("@JOBID", jobid);
                upd.Parameters.AddWithValue("@Id", dbid);
                upd.CommandTimeout = 0;
                upd.ExecuteNonQuery();
            }
            else
            {
                statusdata1 = "Active";
                SqlCommand upd = new SqlCommand("update tbl_jobs set JOBID_Status=@JOBID_Status where JOBID=@JOBID and Id=@Id", dbcl.Conn);
                upd.Parameters.AddWithValue("@JOBID_Status", statusdata1);
                upd.Parameters.AddWithValue("@JOBID", jobid);
                upd.Parameters.AddWithValue("@Id", dbid);
                upd.CommandTimeout = 0;
                upd.ExecuteNonQuery();
            }
            dbcl.Conn.Close();

            GridBinder(lbl_year.Text, lbl_monthcode.Text);
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

            SqlParameter[] monthParams = {
                new SqlParameter("@Workman", Session["WORKMAN"].ToString()),
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate)
            };
            SqlParameter[] billingParams = {
                new SqlParameter("@Workman", Session["WORKMAN"].ToString()),
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate),
                new SqlParameter("@BillingCode", DDL_BillingType.SelectedValue.ToString())
            };

            if (DDL_JobStatus.SelectedIndex == 0 && DDL_BillingType.SelectedIndex == 0)
            {
                string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                    ORDER BY CreatedDate DESC";
                BindGrid(CmdString2, monthParams);
            }
            else if (DDL_JobStatus.SelectedIndex != 0 && DDL_BillingType.SelectedIndex == 0)
            {
                if (DDL_JobStatus.SelectedValue == "0")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, monthParams);
                }
                else if (DDL_JobStatus.SelectedValue == "1")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND JOBID_Status = 'Active'
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, monthParams);
                }
                else if (DDL_JobStatus.SelectedValue == "2")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND JOBID_Status != 'Active'
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, monthParams);
                }
                else if (DDL_JobStatus.SelectedValue == "3")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND FinalUpldStatus != 'Yes'
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, monthParams);
                }
                else if (DDL_JobStatus.SelectedValue == "4")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND FinalUpldStatus = 'Yes'
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, monthParams);
                }
                else if (DDL_JobStatus.SelectedValue == "5")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND Incharge_Approval = 'Pending'
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, monthParams);
                }
                else if (DDL_JobStatus.SelectedValue == "6")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND Incharge_Approval = 'Approved'
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, monthParams);
                }
                else if (DDL_JobStatus.SelectedValue == "7")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND Incharge_Approval = 'Returned'
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, monthParams);
                }
                else if (DDL_JobStatus.SelectedValue == "8")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND Incharge_Approval = 'Rejected'
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, monthParams);
                }
            }
            else if (DDL_JobStatus.SelectedIndex == 0 && DDL_BillingType.SelectedIndex != 0)
            {
                string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND BillingCode = @BillingCode
                    ORDER BY CreatedDate DESC";
                BindGrid(CmdString2, billingParams);
            }
            else if (DDL_JobStatus.SelectedIndex != 0 && DDL_BillingType.SelectedIndex != 0)
            {
                if (DDL_JobStatus.SelectedValue == "0")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND BillingCode = @BillingCode
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, billingParams);
                }
                else if (DDL_JobStatus.SelectedValue == "1")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND JOBID_Status = 'Active'
                      AND BillingCode = @BillingCode
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, billingParams);
                }
                else if (DDL_JobStatus.SelectedValue == "2")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND JOBID_Status != 'Active'
                      AND BillingCode = @BillingCode
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, billingParams);
                }
                else if (DDL_JobStatus.SelectedValue == "3")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND FinalUpldStatus != 'Yes'
                      AND BillingCode = @BillingCode
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, billingParams);
                }
                else if (DDL_JobStatus.SelectedValue == "4")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND FinalUpldStatus = 'Yes'
                      AND BillingCode = @BillingCode
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, billingParams);
                }
                else if (DDL_JobStatus.SelectedValue == "5")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND Incharge_Approval = 'Pending'
                      AND BillingCode = @BillingCode
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, billingParams);
                }
                else if (DDL_JobStatus.SelectedValue == "6")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND Incharge_Approval = 'Approved'
                      AND BillingCode = @BillingCode
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, billingParams);
                }
                else if (DDL_JobStatus.SelectedValue == "7")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND Incharge_Approval = 'Returned'
                      AND BillingCode = @BillingCode
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, billingParams);
                }
                else if (DDL_JobStatus.SelectedValue == "8")
                {
                    string CmdString2 = @"
                    SELECT *
                    FROM tbl_jobs
                    WHERE JOB_InchargeWrk = @Workman
                      AND JOB_Status = 'Out-Punch Done'
                      AND EntryExit = 'Exit'
                      AND CreatedDate >= @StartDate
                      AND CreatedDate < @EndDate
                      AND Incharge_Approval = 'Rejected'
                      AND BillingCode = @BillingCode
                    ORDER BY CreatedDate DESC";
                    BindGrid(CmdString2, billingParams);
                }
            }
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            DDL_JobStatus.SelectedIndex = 0;
            DDL_BillingType.SelectedIndex = 0;

            string Year = DateTime.Now.Year.ToString();
            string Month = DateTime.Now.Month.ToString();
            GridBinder(Year, Month);
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string Year = lbl_year.Text.ToString();
            string Month = lbl_monthcode.Text.ToString();
            GridBinder(Year, Month);
        }
    }
}