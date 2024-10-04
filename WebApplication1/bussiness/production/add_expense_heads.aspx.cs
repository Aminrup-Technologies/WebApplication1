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
    public partial class add_expense_heads : System.Web.UI.Page
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
                    UserCheck();
                    if (Session["USTATE"].ToString() == "PI")
                    {
                        string CmdString2 = "select * from tlb_expheads where Region='" + Session["REGION"].ToString() + "' order by Id";
                        BindGrid(CmdString2);
                    }
                    else
                    {
                        string CmdString2 = "select * from tlb_expheads where Region='" + Session["REGION"].ToString() + "' order by Id";
                        BindGrid(CmdString2);
                    }
                }
            }
        }

        private void UserCheck()
        {
            if (Session["WORKMAN"].ToString() == "J8" || Session["WORKMAN"].ToString() == "A84" || Session["WORKMAN"].ToString() == "K208")
            {
                add_panel.Visible = true;
            }
            else
            {
                add_panel.Visible = false;
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

        private string Find_SLNO()
        {
            string aa = null;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string kk = null;
            string cmdString1 = "select Id,SlNo from tlb_expheads where Id=(select max(Id)from tlb_expheads)";
            SqlCommand com1 = new SqlCommand(cmdString1, dbcl.Conn);
            SqlDataReader DR1 = com1.ExecuteReader();
            if (DR1.Read())
            {
                aa = DR1.GetValue(1).ToString();
                ///string bb = aa.Substring(5);
                int k = Convert.ToInt32(aa);
                k = k + 1;
                string q = Convert.ToString(k);
                kk = q;
            }
            else
            {
                kk = "1";
            }
            dbcl.DisconnectDb();
            return kk;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (txt_exphead.Text != "" && txt_expheadcode.Text !="")
            {
                try
                {
                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    string InsertQuery = "INSERT into tlb_expheads(SlNo,ExpenseHead,ExpenseHeadCode,Region,Company_Code,AddedByWorkMan) VALUES(@SlNo,@ExpenseHead,@ExpenseHeadCode,@Region,@Company_Code,@AddedByWorkMan)";
                    SqlCommand cmd = new SqlCommand(InsertQuery, dbcl.Conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@SlNo", Find_SLNO());
                    cmd.Parameters.AddWithValue("@ExpenseHead", txt_exphead.Text.ToString());
                    cmd.Parameters.AddWithValue("@ExpenseHeadCode", txt_exphead.Text.ToUpper().ToString());
                    cmd.Parameters.AddWithValue("@Region", Session["REGION"].ToString());
                    cmd.Parameters.AddWithValue("@Company_Code", Session["COMPANY_CODE"].ToString());
                    cmd.Parameters.AddWithValue("@AddedByWorkMan", Session["WORKMAN"].ToString());
                    cmd.ExecuteNonQuery();
                    dbcl.DisconnectDb();
                    dbcl.Conn.Close();

                    txt_exphead.Text = "";
                    txt_expheadcode.Text = "";

                    string CmdString2 = "select * from tlb_expheads order by Id";
                    BindGrid(CmdString2);

                    string title = "Notifications :";
                    string body = "Data saved Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                    lbl_msg.Text = "Success : Data Saved";
                }
                catch (Exception ex)
                {
                    lbl_msg.Text = "Error : " + ex.Message;

                    string title = "Notifications :";
                    string body = ex.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    //throw;
                }
            }
            else
            {

            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            string CmdString2 = "select * from tlb_expheads where Region='" + Session["REGION"].ToString() + "' order by Id";
            BindGrid(CmdString2);
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            string CmdString2 = "select * from tlb_expheads where Region='" + Session["REGION"].ToString() + "' order by Id";
            BindGrid(CmdString2);

            string title = "Notifications :";
            string body = "No Changes Made";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            TextBox sitename = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_ExpenseHead");
            string nw_exphd = sitename.Text.ToString();

            TextBox sitecode = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_ExpenseHeadCode");
            string nw_hdcode = sitecode.Text.ToString();

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "UPDATE tlb_expheads set ExpenseHead=@ExpenseHead, ExpenseHeadCode=@ExpenseHeadCode where Id='" + id + "'";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ExpenseHead", nw_exphd);
                cmd.Parameters.AddWithValue("@ExpenseHeadCode", nw_hdcode);
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                dbcl.Conn.Close();

                string title = "Notifications :";
                string body = "Data Updated Successfully";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {

                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }

            GridView1.EditIndex = -1;
            string CmdString2 = "select * from tlb_expheads order by Id";
            BindGrid(CmdString2);
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            //string id = ID.Text.ToString();

            //try
            //{
            //    dbcl.Sqlconnection();
            //    dbcl.ConnectDb();
            //    string cmdString = "delete from tlb_expheads where Id='" + id + " ";
            //    SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            //    cmd.CommandType = CommandType.Text;
            //    cmd.CommandTimeout = 0;
            //    cmd.ExecuteNonQuery();
            //    dbcl.Conn.Close();

            //    string title = "Notifications :";
            //    string body = "Data Deleted Successfully";
            //    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            //}
            //catch (Exception ex)
            //{
            //    string title = "Notifications :";
            //    string body = ex.Message;
            //    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            //}

            //string CmdString2 = "select * from tlb_expheads order by Id";
            //BindGrid(CmdString2);

            string title = "Notifications :";
            string body = "Delete Not Allowed, Contact ADMIN";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
        }
    }
}