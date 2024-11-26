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
    public partial class view_approvedjobs : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
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
                    string CmdString = "select BilingType, BillingCode from tlb_JOB_BillingType order by Id";
                    Bind_BillingType(CmdString);

                    DateTime now = DateTime.Now;

                    lbl_year.Text = DateTime.Now.Year.ToString();
                    lbl_monthcode.Text = DateTime.Now.Month.ToString();
                    lbl_month.Text = now.ToString("MMMM");

                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + DateTime.Now.Year.ToString() + "' and MONTH(CreatedDate)='" + DateTime.Now.Month.ToString() + "' order by CreatedDate desc";
                    BindGrid(CmdString2);

                    //string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' order by CreatedDate desc";
                    //BindGrid(CmdString2);
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

        private void BindGrid(string cmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            dbcl.Conn.Close();
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

                Button btn_status = (Button)GridView1.Rows[i].FindControl("btn_jobidstatus");

                string jobidstatus = lbl_JOBID_Status.Text.ToString();
                string approvalstatus = lbl_Incharge_Approval.Text.ToString();

                string prmtno = lbl_JOB_PermitNo.Text.ToString();
                string lblupldstatus = lbl_FinalUpldStatus.Text.ToString();

                if (jobidstatus == "Active")
                {
                    btn_status.CssClass = "btn btn-success btn-sm";
                }
                else
                {
                    btn_status.CssClass = "btn btn-sm btn-danger";
                }

                if (approvalstatus == "Approved")
                {
                    lbl_Incharge_Approval.ForeColor = System.Drawing.Color.DarkSeaGreen;
                    lbl_JOB_InchargeName.ForeColor = System.Drawing.Color.DarkSeaGreen;
                }
                else
                {
                    lbl_Incharge_Approval.ForeColor = System.Drawing.Color.Red;
                    lbl_JOB_InchargeName.ForeColor = System.Drawing.Color.Red;
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
            if (e.CommandName == "View_Details")
            {
                //Response.Redirect("view_jobdetails.aspx?JOBID=" + jobid + "");
                //The below code is added on 28-10-2024 and above is commented

                Response.Redirect("view_jobdetails.aspx?JOBID=" + jobid + "&dbid=" + dbid + "&supv=" + supv, false);
            }
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

            if (DDL_JobStatus.SelectedIndex == 0 && DDL_BillingType.SelectedIndex == 0)
            {
                string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' order by CreatedDate desc";
                BindGrid(CmdString2);
            }
            else if (DDL_JobStatus.SelectedIndex != 0 && DDL_BillingType.SelectedIndex == 0)
            {
                if (DDL_JobStatus.SelectedValue == "0")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_JobStatus.SelectedValue == "1")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and JOBID_Status='Active' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_JobStatus.SelectedValue == "2")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and JOBID_Status!='Active' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_JobStatus.SelectedValue == "3")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and FinalUpldStatus!='Yes' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_JobStatus.SelectedValue == "4")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and FinalUpldStatus='Yes' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_JobStatus.SelectedValue == "5")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and Incharge_Approval='Pending' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_JobStatus.SelectedValue == "6")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and Incharge_Approval='Approved' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_JobStatus.SelectedValue == "7")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and Incharge_Approval='Returned' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_JobStatus.SelectedValue == "8")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and Incharge_Approval='Rejected' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
            }
            else if (DDL_JobStatus.SelectedIndex == 0 && DDL_BillingType.SelectedIndex != 0)
            {
                string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and BillingCode='" + DDL_BillingType.SelectedValue.ToString() + "' order by CreatedDate desc";
                BindGrid(CmdString2);
            }
            else if (DDL_JobStatus.SelectedIndex != 0 && DDL_BillingType.SelectedIndex != 0)
            {
                if (DDL_JobStatus.SelectedValue == "0")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and BillingCode='" + DDL_BillingType.SelectedValue.ToString() + "' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_JobStatus.SelectedValue == "1")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and JOBID_Status='Active' and BillingCode='" + DDL_BillingType.SelectedValue.ToString() + "' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_JobStatus.SelectedValue == "2")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and JOBID_Status!='Active' and BillingCode='" + DDL_BillingType.SelectedValue.ToString() + "' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_JobStatus.SelectedValue == "3")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and FinalUpldStatus!='Yes' and BillingCode='" + DDL_BillingType.SelectedValue.ToString() + "' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_JobStatus.SelectedValue == "4")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and FinalUpldStatus='Yes' and BillingCode='" + DDL_BillingType.SelectedValue.ToString() + "' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_JobStatus.SelectedValue == "5")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and Incharge_Approval='Pending' and BillingCode='" + DDL_BillingType.SelectedValue.ToString() + "' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_JobStatus.SelectedValue == "6")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and Incharge_Approval='Approved' and BillingCode='" + DDL_BillingType.SelectedValue.ToString() + "' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_JobStatus.SelectedValue == "7")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and Incharge_Approval='Returned' and BillingCode='" + DDL_BillingType.SelectedValue.ToString() + "' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_JobStatus.SelectedValue == "8")
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and Incharge_Approval='Rejected' and BillingCode='" + DDL_BillingType.SelectedValue.ToString() + "' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
            }
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Approved' and YEAR(CreatedDate)='" + DateTime.Now.Year.ToString() + "' and MONTH(CreatedDate)='" + DateTime.Now.Month.ToString() + "' order by CreatedDate desc";
            BindGrid(CmdString2);
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string Year = lbl_year.Text.ToString();
            string Month = lbl_monthcode.Text.ToString();
            GridBinder(Year, Month);
        }
    }
}