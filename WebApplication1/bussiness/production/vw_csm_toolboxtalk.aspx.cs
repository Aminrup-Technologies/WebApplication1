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
    public partial class vw_csm_toolboxtalk : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();

        public static string viewerid = "";
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
                    viewerid = Request.QueryString["vw"];

                    DateTime now = DateTime.Now;

                    lbl_year.Text = DateTime.Now.Year.ToString();
                    lbl_monthcode.Text = DateTime.Now.Month.ToString();
                    lbl_month.Text = now.ToString("MMMM");

                    if (viewerid == "rpt0" || viewerid == "vw0")
                    {
                        //string CmdString0 = "select * from tbl_toolboxtalkdata order by TBT_Date desc";
                        //BindGrid(CmdString0);
                    }
                    else if (viewerid == "rpt1" || viewerid == "vw1")
                    {
                        string CmdString1 = "select * from tbl_toolboxtalkdata where TBT_SupvWrk='" + Session["WORKMAN"].ToString() + "' and YEAR(TBT_Date)='" + DateTime.Now.Year.ToString() + "' and MONTH(TBT_Date)='" + DateTime.Now.Month.ToString() + "' order by TBT_Date desc";
                        BindGrid(CmdString1);
                    }
                    else if (viewerid == "rpt2" || viewerid == "vw2")
                    {
                        string CmdString2 = "select * from tbl_toolboxtalkdata where SafetySupvWrk='" + Session["WORKMAN"].ToString() + "' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + DateTime.Now.Year.ToString() + "' and MONTH(TBT_Date)='" + DateTime.Now.Month.ToString() + "' order by TBT_Date desc";
                        BindGrid(CmdString2);
                    }
                    else if (viewerid == "rpt3" || viewerid == "vw3")
                    {
                        string CmdString3 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Approved' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + DateTime.Now.Year.ToString() + "' and MONTH(TBT_Date)='" + DateTime.Now.Month.ToString() + "' order by TBT_Date desc";
                        BindGrid(CmdString3);
                    }
                }
            }
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
                Delete_from_PermitTable(dbcode);


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

            string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and JOB_Status='Out-Punch Done' and EntryExit='Exit' order by CreatedDate desc"; BindGrid(CmdString2);
            Response.Redirect(Request.Url.AbsoluteUri);
        }


        private void Delete_from_JOBTable(string id, string dbcode)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "delete from tbl_jobs where Id='" + id + "' and JOBID='" + dbcode + "'  ";
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

        private void Delete_from_PermitTable(string dbcode)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "delete from tbl_jobspermit where JOBID='" + dbcode + "'  ";
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
                Label lbl_SafetySupvName = (Label)GridView1.Rows[i].FindControl("lbl_SafetySupvName");
                Label lbl_SafetySupvApprovalStatus = (Label)GridView1.Rows[i].FindControl("lbl_SafetySupvApprovalStatus");

                Label lbl_SafetyOfficerName = (Label)GridView1.Rows[i].FindControl("lbl_SafetyOfficerName");
                Label lbl_SO_ApprovalStatus = (Label)GridView1.Rows[i].FindControl("lbl_SO_ApprovalStatus");

                //ImageButton ImgBtn_ViewDetails = (ImageButton)GridView1.Rows[i].FindControl("ImgBtn_ViewDetails");

                string SafetySupvApprovalStatus = lbl_SafetySupvApprovalStatus.Text.ToString();
                string SafetyOfcrApprovalStatus = lbl_SO_ApprovalStatus.Text.ToString();

                if (SafetySupvApprovalStatus == "Approved" && SafetyOfcrApprovalStatus == "Approved")
                {
                    lbl_SafetySupvName.ForeColor = System.Drawing.Color.Green;
                    lbl_SafetyOfficerName.ForeColor= System.Drawing.Color.Green;
                    //ImgBtn_ViewDetails.BackColor = System.Drawing.Color.Green;
                }
                else if (SafetySupvApprovalStatus == "Approved")
                {
                    lbl_SafetySupvName.ForeColor = System.Drawing.Color.Green;
                    lbl_SafetyOfficerName.ForeColor = System.Drawing.Color.Red;
                    //ImgBtn_ViewDetails.BackColor = System.Drawing.Color.Orange;
                }
                else if(SafetyOfcrApprovalStatus == "Approved")
                {
                    lbl_SafetySupvName.ForeColor = System.Drawing.Color.Red;
                    lbl_SafetyOfficerName.ForeColor = System.Drawing.Color.Green;
                    //ImgBtn_ViewDetails.BackColor = System.Drawing.Color.Orange;
                }
                else
                {
                    lbl_SafetySupvName.ForeColor = System.Drawing.Color.Red;
                    lbl_SafetyOfficerName.ForeColor = System.Drawing.Color.Red;
                    //ImgBtn_ViewDetails.BackColor = System.Drawing.Color.Red;
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
            string jobid = (row.FindControl("lbl_Ref_JOBID") as Label).Text;
            string tbtid = (row.FindControl("lbl_TBT_ID") as Label).Text;

            if (e.CommandName == "View_JOBDetails")
            {
                Response.Redirect("view_jobdetails.aspx?JOBID=" + jobid + "");
            }
            else if (e.CommandName == "View_TBTDetails")
            {
                Response.Redirect("vw_csm_toolboxtalkdetails.aspx?JOBID=" + jobid + "&vw=" + viewerid + "");
            }
            else if (e.CommandName == "Delete")
            {
                //JOBID_Delete(dbid, jobid);
            }
            else if (e.CommandName == "tbtrpt")
            {
                Response.Write("<script>window.open ('/bussiness/production/rpts/tbttalk_rpt.aspx?TBTID=" + tbtid + "','_blank');</script>");
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

            if (viewerid == "rpt0")
            {
                //string CmdString0 = "select * from tbl_toolboxtalkdata order by TBT_Date desc";
                //BindGrid(CmdString0);
            }
            else if (viewerid == "rpt1")
            {
                string CmdString1 = "select * from tbl_toolboxtalkdata where TBT_SupvWrk='" + Session["WORKMAN"].ToString() + "' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' order by TBT_Date desc";
                BindGrid(CmdString1);
            }
            else if (viewerid == "rpt2")
            {
                string CmdString2 = "select * from tbl_toolboxtalkdata where SafetySupvWrk='" + Session["WORKMAN"].ToString() + "' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' order by TBT_Date desc";
                BindGrid(CmdString2);
            }
            else if (viewerid == "rpt3")
            {
                string CmdString3 = "select * from tbl_toolboxtalkdata where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Approved' and SafetySupvApprovalStatus='Approved' and YEAR(TBT_Date)='" + Year + "' and MONTH(TBT_Date)='" + Month + "' order by TBT_Date desc";
                BindGrid(CmdString3);
            }
        }
    }
}