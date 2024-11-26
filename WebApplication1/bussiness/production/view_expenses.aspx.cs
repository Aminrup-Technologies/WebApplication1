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
    public partial class view_expenses : System.Web.UI.Page
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

                    //string CmdString = "select ExpenseHead, ExpenseHeadCode from tlb_expheads order by Id";
                    //Bind_Heads(CmdString);


                    string CmdString2 = "select DB_Code, CompDept_Location from tlb_workregion_compdept_loc where Country_Code = 'IN' and State_Code ='" + Session["USTATE"].ToString() + "' and Work_Region_Code = '" + Session["REGION"].ToString() + "' and Company_Code = '" + Session["COMPANY_CODE"].ToString() + "' order by Id";
                    Bind_Heads(CmdString2);

                    DateTime now = DateTime.Now;

                    lbl_year.Text = DateTime.Now.Year.ToString();
                    lbl_monthcode.Text = DateTime.Now.Month.ToString();
                    lbl_month.Text = now.ToString("MMMM");

                    string CmdString3 = "select * from tbl_expenselogs where LoggedByWrk='" + Session["WORKMAN"].ToString() + "' and YEAR(LoggedOn)='" + DateTime.Now.Year.ToString() + "' and MONTH(LoggedOn)='" + DateTime.Now.Month.ToString() + "' order by Id desc";
                    BindGrid(CmdString3);
                }
            }
        }

        private void Bind_Heads(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Location.DataSource = Cmd.ExecuteReader();
            DDL_Location.DataTextField = "CompDept_Location";
            DDL_Location.DataValueField = "DB_Code";
            DDL_Location.DataBind();
            DDL_Location.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }


        //private void Bind_SubHeads(string CmdString)
        //{
        //    dbcl.Sqlconnection();
        //    dbcl.ConnectDb();
        //    SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
        //    Cmd.CommandType = CommandType.Text;
        //    DDL_SubHeads.DataSource = Cmd.ExecuteReader();
        //    DDL_SubHeads.DataTextField = "SubHead";
        //    DDL_SubHeads.DataValueField = "SubHead";
        //    DDL_SubHeads.DataBind();
        //    DDL_SubHeads.Items.Insert(0, "Please Select Option");
        //    dbcl.DisconnectDb();
        //}

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

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //string jobid = Convert.ToString(e.CommandArgument);

            //Determine the RowIndex of the Row whose Button was clicked.
            int rowIndex = Convert.ToInt32(e.CommandArgument);

            //Reference the GridView Row.
            GridViewRow row = GridView1.Rows[rowIndex];

            //Fetch value of Name.
            string dbid = (row.FindControl("lbl_Id") as Label).Text;
            string expid = (row.FindControl("lbl_ExpenseID") as Label).Text;

            if (e.CommandName == "View_Details")
            {
                Response.Redirect("view_expensedetails.aspx?EXPID=" + expid + "");

            }
            else if (e.CommandName == "Delete")
            {
                ExpenseDelete(dbid, expid);
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

            //if (DDL_Heads.SelectedIndex == 0 && DDL_SubHeads.SelectedIndex == 0)
            //{
            //    string CmdString2 = "select * from tbl_expenselogs where LoggedByWrk='" + Session["WORKMAN"].ToString() + "' and YEAR(LoggedOn)='" + Year + "' and MONTH(LoggedOn)='" + Month + "' order by Id desc";
            //    BindGrid(CmdString2);
            //}
            //else if (DDL_Heads.SelectedIndex != 0 && DDL_SubHeads.SelectedIndex==0)
            //{
            //    string CmdString2 = "select * from tbl_expenselogs where LoggedByWrk='" + Session["WORKMAN"].ToString() + "' and ExpHead='"+DDL_Heads.SelectedItem.Text.ToString()+"' and YEAR(LoggedOn)='" + Year + "' and MONTH(LoggedOn)='" + Month + "' order by Id desc";
            //    BindGrid(CmdString2);
            //}
            //else if (DDL_Heads.SelectedIndex != 0 && DDL_SubHeads.SelectedIndex != 0)
            //{
            //    string CmdString2 = "select * from tbl_expenselogs where LoggedByWrk='" + Session["WORKMAN"].ToString() + "' and ExpHead='" + DDL_Heads.SelectedItem.Text.ToString() + "' and ExpSubHead='"+DDL_SubHeads.SelectedItem.Text.ToString()+"' and YEAR(LoggedOn)='" + Year + "' and MONTH(LoggedOn)='" + Month + "' order by Id desc";
            //    BindGrid(CmdString2);
            //}

            if (DDL_Location.SelectedIndex == 0)
            {
                string CmdString2 = "select * from tbl_expenselogs where LoggedByWrk='" + Session["WORKMAN"].ToString() + "' and YEAR(LoggedOn)='" + Year + "' and MONTH(LoggedOn)='" + Month + "' order by Id desc";
                BindGrid(CmdString2);
            }
            else
            {
                string CmdString2 = "select * from tbl_expenselogs where LoggedByWrk='" + Session["WORKMAN"].ToString() + "' and YEAR(LoggedOn)='" + Year + "' and MONTH(LoggedOn)='" + Month + "'  and Location='" + DDL_Location.SelectedItem.Text.ToString() + "' order by Id desc";
                BindGrid(CmdString2);
            }
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            string CmdString2 = "select * from tbl_expenselogs where LoggedByWrk='" + Session["WORKMAN"].ToString() + "' and YEAR(LoggedOn)='" + DateTime.Now.Year.ToString() + "' and MONTH(LoggedOn)='" + DateTime.Now.Month.ToString() + "' order by Id desc";
            BindGrid(CmdString2);
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string Year = lbl_year.Text.ToString();
            string Month = lbl_monthcode.Text.ToString();
            GridBinder(Year, Month);
        }

        protected void DDL_Heads_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string CmdString2 = "select SubHead, SubHead from tlb_expsubheads where HeadCode='" + DDL_Heads.SelectedValue.ToString() +"' order by Id";
            //Bind_SubHeads(CmdString2);
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            Label ExpenseID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_ExpenseID");
            string expid = ExpenseID.Text.ToString();

            try
            {
                ExpenseDelete(id, expid);
                string title = "Notifications :";
                string body = "Expense Data has been DELETED !!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }

            string Year = lbl_year.Text.ToString();
            string Month = lbl_monthcode.Text.ToString();
            GridBinder(Year, Month);
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        private void ExpenseDelete(string id, string expid)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "delete from tbl_expenselogs where Id='" + id + "' and ExpenseID='" + expid + "'  ";
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

        protected void DDL_Location_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Year = lbl_year.Text.ToString();
            string Month = lbl_monthcode.Text.ToString();
            GridBinder(Year, Month);
        }
    }
}