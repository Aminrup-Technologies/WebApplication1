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
    public partial class vw_tbt_masterapproval : System.Web.UI.Page
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

                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + DateTime.Now.Year.ToString() + "' and MONTH(TBT_Date)='" + DateTime.Now.Month.ToString() + "' order by TBT_Date desc";
                    BindGrid(CmdString2);

                    //string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' order by TBT_Date desc";
                    //BindGrid(CmdString2);

                    //Label lbl_pendingforappjob = (Label)Page.Master.FindControl("lbl_jobspendingcount");
                    //lbl_pendingforappjob.Text = Convert.ToString(CC.GetApprovedJOBApprovalCount(Session["WORKMAN"].ToString()));

                    string CmdString = "select Company_Department, Company_Department from tlb_workregion_compdept where Work_Region_Code = '" + Session["REGION"].ToString() + "' order by Id";
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
            DDL_TBTDept.DataSource = Cmd.ExecuteReader();
            DDL_TBTDept.DataTextField = "Company_Department";
            DDL_TBTDept.DataValueField = "Company_Department";
            DDL_TBTDept.DataBind();
            DDL_TBTDept.Items.Insert(0, "Please Select Option");
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

            string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and SO_ApprovalStatus='Pending' order by TBT_Date desc"; BindGrid(CmdString2);
            Response.Redirect(Request.Url.AbsoluteUri);
        }


        private void Delete_from_JOBTable(string id, string dbcode)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "delete from tbl_toolboxtalkdata where Id='" + id + "' and JOBID='" + dbcode + "'  ";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
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
                string cmdString = "delete from tbl_attendance where JOBID='" + dbcode + "'  ";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
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
                //Label lbl_JOBID_Status = (Label)GridView1.Rows[i].FindControl("lbl_JOBID_Status");
                //Label lbl_JOB_InchargeName = (Label)GridView1.Rows[i].FindControl("lbl_JOB_InchargeName");
                //Label lbl_Incharge_Approval = (Label)GridView1.Rows[i].FindControl("lbl_Incharge_Approval");

                //Label lbl_JOB_PermitNo = (Label)GridView1.Rows[i].FindControl("lbl_JOB_PermitNo");
                //Label lbl_FinalUpldStatus = (Label)GridView1.Rows[i].FindControl("lbl_FinalUpldStatus");

                //Button btn_viewdetails = (Button)GridView1.Rows[i].FindControl("btn_viewdetails");
                //Button btn_status = (Button)GridView1.Rows[i].FindControl("btn_jobidstatus");

                //string approvalstatus = lbl_Incharge_Approval.Text.ToString();

                //string prmtno = lbl_JOB_PermitNo.Text.ToString();
                //string lblupldstatus = lbl_FinalUpldStatus.Text.ToString();

                //if (approvalstatus == "Approved")
                //{
                //    lbl_Incharge_Approval.ForeColor = System.Drawing.Color.DarkSeaGreen;
                //    lbl_JOB_InchargeName.ForeColor = System.Drawing.Color.DarkSeaGreen;
                //    btn_status.Enabled = false;
                //}
                //else
                //{
                //    lbl_Incharge_Approval.ForeColor = System.Drawing.Color.Red;
                //    lbl_JOB_InchargeName.ForeColor = System.Drawing.Color.Red;
                //    btn_status.Enabled = true;
                //}

                //if (lblupldstatus == "Yes")
                //{
                //    lbl_JOB_PermitNo.ForeColor = System.Drawing.Color.DarkSeaGreen;
                //}
                //else
                //{
                //    lbl_JOB_PermitNo.ForeColor = System.Drawing.Color.Red;
                //}
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
            string jobid = (row.FindControl("lbl_Ref_JOBID") as Label).Text;

            if (e.CommandName == "View_JOBDetails")
            {
                Response.Redirect("view_jobdetails.aspx?JOBID=" + jobid + "");
            }
            else if (e.CommandName == "View_TBTDetails")
            {
                Response.Redirect("vw_csm_toolboxtalkdetails.aspx?JOBID=" + jobid + "");
            }
            else if (e.CommandName == "Delete")
            {
                //JOBID_Delete(dbid, jobid);
            }
        }

        private void JOBID_Status_Swaper(string jobid, string dbid)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string statusdata = "";
            string statusdata1 = "";
            string cmdstring = "select JOBID_Status from tbl_toolboxtalkdata where JOBID='" + jobid + "' and Id = '" + dbid + "'";
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            SqlDataReader re = cmd.ExecuteReader();
            if (re.Read())
            {
                statusdata = re["JOBID_Status"].ToString();
            }


            if (statusdata == "Active")
            {
                statusdata1 = "Blocked";

                dbcl.executeRdr("update tbl_toolboxtalkdata set JOBID_Status ='" + statusdata1 + "' where JOBID='" + jobid + "' and Id = '" + dbid + "'");
            }
            else
            {
                statusdata1 = "Active";
                dbcl.executeRdr("update tbl_toolboxtalkdata set JOBID_Status ='" + statusdata1 + "' where JOBID='" + jobid + "' and Id = '" + dbid + "'");
            }
            dbcl.Conn.Close();

            string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' order by TBT_Date desc";
            BindGrid(CmdString2);
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

            if (DDL_TBTDept.SelectedIndex == 0 && DDL_TBTStatus.SelectedIndex == 0)
            {
                string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' order by TBT_Date desc";
                BindGrid(CmdString2);
            }
            else if (DDL_TBTStatus.SelectedIndex != 0 && DDL_TBTDept.SelectedIndex == 0)
            {
                if (DDL_TBTStatus.SelectedValue == "0")
                {
                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' order by TBT_Date desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_TBTStatus.SelectedValue == "1")
                {
                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' and Panel2_Status='Complete' order by TBT_Date desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_TBTStatus.SelectedValue == "2")
                {
                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' and Panel2_Status!='Complete' order by TBT_Date desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_TBTStatus.SelectedValue == "3")
                {
                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' and TBTPhoto!='Uploaded' order by TBT_Date desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_TBTStatus.SelectedValue == "4")
                {
                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' and TBTPhoto='Uploaded' order by TBT_Date desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_TBTStatus.SelectedValue == "5")
                {
                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' order by TBT_Date desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_TBTStatus.SelectedValue == "6")
                {
                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Approved' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' order by TBT_Date desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_TBTStatus.SelectedValue == "7")
                {
                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SafetySupvApprovalStatus='Returned' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' order by TBT_Date desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_TBTStatus.SelectedValue == "8")
                {
                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SafetySupvApprovalStatus='Rejected' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' order by TBT_Date desc";
                    BindGrid(CmdString2);
                }
            }
            else if (DDL_TBTStatus.SelectedIndex == 0 && DDL_TBTDept.SelectedIndex != 0)
            {
                string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' and TBT_Dept='" + DDL_TBTDept.SelectedValue.ToString() + "' order by TBT_Date desc";
                BindGrid(CmdString2);
            }
            else if (DDL_TBTDept.SelectedIndex != 0 && DDL_TBTStatus.SelectedIndex != 0)
            {
                if (DDL_TBTStatus.SelectedValue == "1")
                {
                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' and Panel2_Status='Complete' and TBT_Dept='" + DDL_TBTDept.SelectedValue.ToString() + "' order by TBT_Date desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_TBTStatus.SelectedValue == "2")
                {
                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' and Panel2_Status!='Complete' and TBT_Dept='" + DDL_TBTDept.SelectedValue.ToString() + "' order by TBT_Date desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_TBTStatus.SelectedValue == "3")
                {
                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' and TBTPhoto!='Uploaded' and TBT_Dept='" + DDL_TBTDept.SelectedValue.ToString() + "' order by TBT_Date desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_TBTStatus.SelectedValue == "4")
                {
                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' and TBTPhoto='Uploaded' and TBT_Dept='" + DDL_TBTDept.SelectedValue.ToString() + "' order by TBT_Date desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_TBTStatus.SelectedValue == "5")
                {
                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' and TBT_Dept='" + DDL_TBTDept.SelectedValue.ToString() + "' order by TBT_Date desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_TBTStatus.SelectedValue == "6")
                {
                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Approved' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' and TBT_Dept='" + DDL_TBTDept.SelectedValue.ToString() + "' order by TBT_Date desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_TBTStatus.SelectedValue == "7")
                {
                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SafetySupvApprovalStatus='Returned' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' and TBT_Dept='" + DDL_TBTDept.SelectedValue.ToString() + "' order by TBT_Date desc";
                    BindGrid(CmdString2);
                }
                else if (DDL_TBTStatus.SelectedValue == "8")
                {
                    string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SafetySupvApprovalStatus='Rejected' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' and TBT_Dept='" + DDL_TBTDept.SelectedValue.ToString() + "' order by TBT_Date desc";
                    BindGrid(CmdString2);
                }
            }
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            lbl_year.Text = DateTime.Now.Year.ToString();
            lbl_monthcode.Text = DateTime.Now.Month.ToString();
            lbl_month.Text = now.ToString("MMMM");

            string CmdString2 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + DateTime.Now.Year.ToString() + "' and MONTH(TBT_Date)='" + DateTime.Now.Month.ToString() + "' order by TBT_Date desc";
            BindGrid(CmdString2);

            DDL_TBTDept.SelectedIndex = 0;
            DDL_TBTStatus.SelectedIndex = 0;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string Year = lbl_year.Text.ToString();
            string Month = lbl_monthcode.Text.ToString();
            GridBinder(Year, Month);
        }
    }
}