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
    public partial class add_expense_subheads : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
            {
                Response.Redirect("~/login.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    string CmdString1 = "select ExpenseHead, ExpenseHeadCode from tlb_expheads where Region='" + Session["REGION"].ToString() + "' order by Id";
                    BindCountry(CmdString1);

                    UserCheck();
                    if (Session["USTATE"].ToString() == "PI")
                    {
                        string CmdString2 = "select * from tlb_expsubheads where Region='" + Session["REGION"].ToString() + "' order by Id";
                        BindGrid(CmdString2);
                    }
                    else
                    {
                        string CmdString2 = "select * from tlb_expsubheads where Region='" + Session["REGION"].ToString() + "' order by Id";
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

        private void BindCountry(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_ExpenseHead.DataSource = Cmd.ExecuteReader();
            DDL_ExpenseHead.DataTextField = "ExpenseHead";
            DDL_ExpenseHead.DataValueField = "ExpenseHeadCode";
            DDL_ExpenseHead.DataBind();
            DDL_ExpenseHead.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        private void BindGrid(string cmdString)
        {
            string query = "";
            if (DDL_ExpenseHead.SelectedIndex == 0)
            {
                query = cmdString;
            }
            else
            {
                query = "select * from tlb_expsubheads where HeadCode='" + DDL_ExpenseHead.SelectedValue.ToString() + "' and Region='" + Session["REGION"].ToString() + "'";
            }
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(query, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            dbcl.Conn.Close();
        }

        protected void DDL_ExpenseHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_ExpenseHead.SelectedIndex != 0)
            {
                string CmdString2 = "select * from tlb_expsubheads where HeadCode='" + DDL_ExpenseHead.SelectedValue.ToString() + "' and Region='" + Session["REGION"].ToString() + "' order by Id";
                BindGrid(CmdString2);

                btn_submit.Enabled = true;

                PullSlNo();
            }
            else
            {
                string CmdString2 = "select * from tlb_expsubheads where Region='" + Session["REGION"].ToString() + "' order by Id";
                BindGrid(CmdString2);
            }
        }

        private void PullSlNo()
        {
            string cmdString = "select SlNo from tlb_expheads where ExpenseHeadCode='" + DDL_ExpenseHead.SelectedValue.ToString() + "'";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                lbl_hdslno.Text = Rdr["SlNo"].ToString();
            }
            dbcl.Conn.Close();
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string name = txt_subhead.Text.ToString();
            string code = txt_subheadcode.Text.ToUpper().ToString();

            if (name != string.Empty && code != string.Empty)
            {
                try
                {
                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    string InsertQuery = "INSERT into tlb_expsubheads(SlNo,Head,HeadCode,SubHead,SubHeadCode,Region,Company_Code,AddedByWorkMan) VALUES(@SlNo,@Head,@HeadCode,@SubHead,@SubHeadCode,@Region,@Company_Code,@AddedByWorkMan)";
                    SqlCommand cmd = new SqlCommand(InsertQuery, dbcl.Conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@SlNo", lbl_hdslno.Text.ToString());
                    cmd.Parameters.AddWithValue("@Head", DDL_ExpenseHead.SelectedItem.Text.ToString());
                    cmd.Parameters.AddWithValue("@HeadCode", DDL_ExpenseHead.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@SubHead", name);
                    cmd.Parameters.AddWithValue("@SubHeadCode", code);
                    cmd.Parameters.AddWithValue("@Region", Session["REGION"].ToString());
                    cmd.Parameters.AddWithValue("@Company_Code", Session["COMPANY_CODE"].ToString());
                    cmd.Parameters.AddWithValue("@AddedByWorkMan", Session["WORKMAN"].ToString());
                    cmd.ExecuteNonQuery();
                    dbcl.DisconnectDb();
                    dbcl.Conn.Close();

                    txt_subhead.Text = "";
                    txt_subheadcode.Text = "";
                    name = string.Empty; code = string.Empty;

                    string CmdString2 = "select * from tlb_expsubheads where HeadCode ='"+DDL_ExpenseHead.SelectedValue.ToString()+ "' and Region='" + Session["REGION"].ToString() + "' order by Id";
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
                txt_subhead.Focus();
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            string CmdString2 = "select * from tlb_expsubheads where Region='" + Session["REGION"].ToString() + "' order by Id";
            BindGrid(CmdString2);
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            string CmdString2 = "select * from tlb_expsubheads where Region='" + Session["REGION"].ToString() + "' order by Id";
            BindGrid(CmdString2);

            string title = "Notifications :";
            string body = "No Changes Made";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            TextBox sitename = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_SubHead");
            string nw_exphd = sitename.Text.ToString();

            TextBox sitecode = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_SubHeadCode");
            string nw_hdcode = sitecode.Text.ToString();

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "UPDATE tlb_expsubheads set SubHead=@SubHead, SubHeadCode=@SubHeadCode where Id='" + id + "'";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@SubHead", nw_exphd);
                cmd.Parameters.AddWithValue("@SubHeadCode", nw_hdcode);
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
            string CmdString2 = "select * from tlb_expsubheads where Region='" + Session["REGION"].ToString() + "' order by Id";
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
            //    string cmdString = "delete from tlb_expsubheads where Id='" + id + " ";
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

            //string CmdString2 = "select * from tlb_expsubheads order by Id";
            //BindGrid(CmdString2);

            string title = "Notifications :";
            string body = "Delete Not Allowed, Contact ADMIN";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
        }
    }
}