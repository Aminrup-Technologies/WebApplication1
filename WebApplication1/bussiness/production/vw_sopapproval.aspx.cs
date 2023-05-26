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
    public partial class vw_sopapproval : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
        public static string viewerid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["USERTYPE"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    DateTime now = DateTime.Now;

                    lbl_year.Text = DateTime.Now.Year.ToString();
                    lbl_monthcode.Text = DateTime.Now.Month.ToString();
                    lbl_month.Text = now.ToString("MMMM");

                    string viewerid = Request.QueryString["vw"];


                    if (viewerid=="sopvw1")
                    {
                        string CmdString2 = "select * from tbl_soptraining where SafetySupvWrk='" + Session["WORKMAN"].ToString() + "' and SafetySupvApprovalStatus='Pending' and YEAR(SOP_Date)='" + DateTime.Now.Year.ToString() + "' and MONTH(SOP_Date)='" + DateTime.Now.Month.ToString() + "' order by SOP_Date desc";
                        BindGrid(CmdString2);
                    }
                    else if (viewerid == "sopvw2")
                    {
                        string CmdString2 = "select * from tbl_soptraining where SafetyOfficerWrk='" + Session["WORKMAN"].ToString() + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved' and YEAR(SOP_Date)='" + DateTime.Now.Year.ToString() + "' and MONTH(SOP_Date)='" + DateTime.Now.Month.ToString() + "' order by SOP_Date desc";
                        BindGrid(CmdString2);
                    }

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


            string CmdString2 = "select * from tbl_soptraining where SafetySupvWrk='" + Session["WORKMAN"].ToString() + "' and SafetySupvApprovalStatus='Pending' and YEAR(SOP_Date)='" + Year + "' and MONTH(SOP_Date)='" + Month + "' order by SOP_Date desc";
            BindGrid(CmdString2);
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            lbl_year.Text = DateTime.Now.Year.ToString();
            lbl_monthcode.Text = DateTime.Now.Month.ToString();
            lbl_month.Text = now.ToString("MMMM");

            try
            {
                string CmdString2 = "select * from tbl_soptraining where SafetySupvWrk='" + Session["WORKMAN"].ToString() + "' and SafetySupvApprovalStatus='Pending' and YEAR(SOP_Date)='" + DateTime.Now.Year.ToString() + "' and MONTH(SOP_Date)='" + DateTime.Now.Month.ToString() + "' order by SOP_Date desc";
                BindGrid(CmdString2);
            }
            catch (Exception)
            {
                string title = "Notifications :";
                string body = "Erorr in Executor";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }

            DDL_TBTDept.SelectedIndex = 0;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string Year = lbl_year.Text.ToString();
            string Month = lbl_monthcode.Text.ToString();
            GridBinder(Year, Month);
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
            string tbtid = (row.FindControl("lbl_SOP_ID") as Label).Text;

            if (e.CommandName == "View_JOBDetails")
            {
                Response.Redirect("view_jobdetails.aspx?JOBID=" + jobid + "&vw="+viewerid+"");
            }
            else if (e.CommandName == "View_Details")
            {
                //Response.Write("<script>window.open ('vw_csm_toolboxtalkdetails.aspx?JOBID=" + jobid + "','_blank');</script>");
                //Response.Redirect("vw_csm_toolboxtalkdetails.aspx?JOBID=" + jobid + "&vw=" + viewerid + "");
            }
        }
    }
}