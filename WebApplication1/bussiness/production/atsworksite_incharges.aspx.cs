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
    public partial class atsworksite_incharges : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        public static string state = string.Empty;
        public static string region = string.Empty;
        public static string comp = string.Empty;
        public static string datalock = string.Empty;

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
                    if (Session["Changer"] != null)
                    {
                        string[] retrievedArray = (string[])Session["Changer"];
                        region = retrievedArray[1].ToString();
                        comp = retrievedArray[2].ToString();
                        state = retrievedArray[0].ToString();
                        datalock = retrievedArray[3].ToString();
                        //Session["Changer"]= null;
                    }
                    else
                    {
                        region = Session["REGION"].ToString();
                        comp = Session["COMPANY_CODE"].ToString();
                        state = Session["STATE"].ToString();
                        datalock = "0";
                    }

                    if (state == "PI")
                    {
                        string CmdString1 = "select Worksite_Name, DB_Code from tlb_atsworksites";
                        BindWorksites(CmdString1);

                        string CmdString2 = "select * from tlb_atsworksiteIncharges order by Id";
                        BindGrid(CmdString2);
                    }
                    else
                    {
                        string CmdString1 = "select Worksite_Name, DB_Code from tlb_atsworksites where WorkRegion_Code = '" + region + "'";
                        BindWorksites(CmdString1);

                        string CmdString2 = "select * from tlb_atsworksiteIncharges where Region_Code = '" + region + "' order by Id";
                        BindGrid(CmdString2);
                    }
                }
            }
        }

        private void BindWorksites(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_worksites.DataSource = Cmd.ExecuteReader();
            DDL_worksites.DataTextField = "Worksite_Name";
            DDL_worksites.DataValueField = "DB_Code";
            DDL_worksites.DataBind();
            DDL_worksites.Items.Insert(0, "Please Select Option");
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

        protected void txt_empworkman_TextChanged(object sender, EventArgs e)
        {
            string entered_workman = txt_empworkman.Text.TrimEnd().ToString();

            if (entered_workman != string.Empty)
            {
                if (dbcl.CheckEmployeeActiveStatus(entered_workman) == true)
                {
                    string name = string.Empty;

                    dbcl.FindEmployeeName(entered_workman, ref name);
                    txt_empname.Text = name;
                }
                else
                {
                    string title = "Notifications :";
                    string body = "No data found against the entered workman";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                    txt_empworkman.Text = "";
                    txt_empworkman.Focus();
                }
            }
            else
            {
                txt_empworkman.Focus();
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string InsertQuery = "INSERT into tlb_atsworksiteIncharges(Country_Code,State_Code,Region_Code,Company_Code,DB_Code,Worksite_Name,Employee_Workman,Employee_Name,Status,JOB_Approver) VALUES(@Country_Code,@State_Code,@Region_Code,@Company_Code,@DB_Code,@Worksite_Name,@Employee_Workman,@Employee_Name,@Status,@JOB_Approver)";
                SqlCommand cmd = new SqlCommand(InsertQuery, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Country_Code", "IN");
                cmd.Parameters.AddWithValue("@State_Code", state);
                cmd.Parameters.AddWithValue("@Region_Code", region);
                cmd.Parameters.AddWithValue("@Company_Code", comp);
                cmd.Parameters.AddWithValue("@DB_Code", DDL_worksites.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Worksite_Name", DDL_worksites.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Employee_Workman", txt_empworkman.Text.ToString());
                cmd.Parameters.AddWithValue("@Employee_Name", txt_empname.Text.ToString());
                cmd.Parameters.AddWithValue("@Status", "Active");
                cmd.Parameters.AddWithValue("@JOB_Approver", "Yes");
                cmd.ExecuteNonQuery();
                dbcl.DisconnectDb();
                dbcl.Conn.Close();

                string CmdString2 = "select * from tlb_atsworksiteIncharges where Region_Code = '" + region+ "' order by Id";
                BindGrid(CmdString2);

                string title = "Notifications :";
                string body = "Data saved Successfully";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                txt_empworkman.Text = "";
                txt_empname.Text = "";
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

        protected void txt_Employee_Workman_TextChanged(object sender, EventArgs e)
        {
            GridViewRow currentRow1 = (GridViewRow)((TextBox)sender).Parent.Parent;
            TextBox txt1 = (TextBox)currentRow1.FindControl("txt_Employee_Workman");

            GridViewRow currentRow2 = (GridViewRow)((TextBox)sender).Parent.Parent;
            TextBox txt2 = (TextBox)currentRow2.FindControl("txt_Employee_Name");

            string name = string.Empty;

            dbcl.FindEmployeeName(txt1.Text, ref name);
            txt2.Text = name;
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            string CmdString2 = "select * from tlb_atsworksiteIncharges where Region_Code = '" + region + "' order by Id";
            BindGrid(CmdString2);
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            string CmdString2 = "select * from tlb_atsworksiteIncharges where Region_Code = '" + region + "' order by Id";
            BindGrid(CmdString2);

            string title = "Notifications :";
            string body = "No Changes Made";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            Label DBCode = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_DB_Code");
            string dbcode = DBCode.Text.ToString();

            TextBox empwrk = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_Employee_Workman");
            string new_empwrk = empwrk.Text.ToString();

            TextBox empname = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_Employee_Name");
            string new_empname = empname.Text.ToString();

            DropDownList status = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDL_Status");
            string new_status = status.SelectedItem.Text.ToString();
            string new_deptcode = status.SelectedValue.ToString();

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "UPDATE tlb_atsworksiteIncharges set Employee_Workman=@Employee_Workman, Employee_Name=@Employee_Name, Status=@Status, Last_ModifiedDate=@Last_ModifiedDate where Id='" + id + "' and DB_Code='" + dbcode + "'  ";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Employee_Workman", new_empwrk);
                cmd.Parameters.AddWithValue("@Employee_Name", new_empname);
                cmd.Parameters.AddWithValue("@Status", new_status);
                cmd.Parameters.AddWithValue("@Last_ModifiedDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                //cmd.Parameters.AddWithValue("@Withdraw_Date", new_loccode);
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
            string CmdString2 = "select * from tlb_atsworksiteIncharges where Region_Code = '" + region+ "' order by Id";
            BindGrid(CmdString2);
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string title = "Notifications :";
            string body = "Delete NOT Allowed, Add NEW..!!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);


            string CmdString2 = "select * from tlb_atsworksiteIncharges where Region_Code = '" + region + "' order by Id";
            BindGrid(CmdString2);

            //Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            //string id = ID.Text.ToString();

            //Label DBCode = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_DB_Code");
            //string dbcode = DBCode.Text.ToString();

            //try
            //{
            //    dbcl.Sqlconnection();
            //    dbcl.ConnectDb();
            //    string cmdString = "delete from tlb_atsworksiteIncharges where Id='" + id + "' and DB_Code='" + dbcode + "'  ";
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

            //string CmdString2 = "select * from tlb_atsworksites where WorkRegion_Code = '" + Session["REGION"].ToString() + "' order by Id";
            //BindGrid(CmdString2);
        }

        protected void DDL_worksites_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CmdString2 = "select * from tlb_atsworksiteIncharges where Region_Code = '" +region + "' and DB_Code='"+DDL_worksites.SelectedValue.ToString()+"' order by Id";
            BindGrid(CmdString2);
        }
    }
}