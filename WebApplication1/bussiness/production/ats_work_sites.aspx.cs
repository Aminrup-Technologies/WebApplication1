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
    public partial class work_sites : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    string CmdString1 = "select Country_Name, Country_Code from tlb_work_country";
                    BindCountry(CmdString1);

                    string CmdString2 = "select * from tlb_atsworksites where WorkRegion_Code = '" + Session["REGION"].ToString() + "' order by Id";
                    BindGrid(CmdString2);
                }
            }
        }

        private void BindCountry(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_WorkCountry.DataSource = Cmd.ExecuteReader();
            DDL_WorkCountry.DataTextField = "Country_Name";
            DDL_WorkCountry.DataValueField = "Country_Code";
            DDL_WorkCountry.DataBind();
            DDL_WorkCountry.Items.Insert(0, "Please Select Option");
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

        protected void DDL_WorkCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CmdString3 = "select State_Name, State_Code from tlb_work_state where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "'";
            BindCountryState(CmdString3);
        }

        private void BindCountryState(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_WorkStates.DataSource = Cmd.ExecuteReader();
            DDL_WorkStates.DataTextField = "State_Name";
            DDL_WorkStates.DataValueField = "State_Code";
            DDL_WorkStates.DataBind();
            DDL_WorkStates.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_WorkStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CmdString3 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' order by Id ";
            BindRegions(CmdString3);
        }

        private void BindRegions(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Region.DataSource = Cmd.ExecuteReader();
            DDL_Region.DataTextField = "Work_Region_Name";
            DDL_Region.DataValueField = "Work_Region_Code";
            DDL_Region.DataBind();
            DDL_Region.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_Region_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' and Work_Region_Code = '" + DDL_Region.SelectedValue.ToString() + "' order by Id ";
            BindCompany(CmdString3);
        }

        private void BindCompany(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Company.DataSource = Cmd.ExecuteReader();
            DDL_Company.DataTextField = "Company_Name";
            DDL_Company.DataValueField = "Company_Code";
            DDL_Company.DataBind();
            DDL_Company.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_Company_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CmdString3 = "select Company_Department, DB_Code from tlb_workregion_compdept where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' and Work_Region_Code = '" + DDL_Region.SelectedValue.ToString() + "' and Company_Code = '" + DDL_Company.SelectedValue.ToString() + "'  order by Id ";
            BindCompanyDept(CmdString3);
        }

        private void BindCompanyDept(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDl_Departments.DataSource = Cmd.ExecuteReader();
            DDl_Departments.DataTextField = "Company_Department";
            DDl_Departments.DataValueField = "DB_Code";
            DDl_Departments.DataBind();
            DDl_Departments.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        private string Find_DBCode()
        {
            string aa = null;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string kk = null;
            string cmdString1 = "select Id,DB_Code from tlb_atsworksites where Id=(select max(Id)from tlb_atsworksites)";
            SqlCommand com1 = new SqlCommand(cmdString1, dbcl.Conn);
            SqlDataReader DR1 = com1.ExecuteReader();
            if (DR1.Read())
            {
                aa = DR1.GetValue(1).ToString();
                string bb = aa.Substring(5);
                int k = Convert.ToInt32(bb);
                k = k + 1;
                string q = Convert.ToString(k);
                kk = "WKS00" + q;
            }
            else
            {
                kk = "WKS001";
            }
            dbcl.DisconnectDb();
            return kk;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string locname = txt_worksite_Name.Text.ToUpper().ToString();
            string loccode = txt_worksite_code.Text.ToUpper().ToString();

            string WKSID = Find_DBCode();

            if (locname != string.Empty && loccode != string.Empty)
            {
                try
                {
                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    string InsertQuery = "INSERT into tlb_atsworksites(Country_Code,Country_Name,State_Code,Sate_Name,WorkRegion_Code,WorkRegion_Name,Company_Code,Company_Name,Company_Department,Dept_DBCode,Worksite_Name,Worksite_Code,DB_Code) VALUES(@Country_Code,@Country_Name,@State_Code,@Sate_Name,@WorkRegion_Code,@WorkRegion_Name,@Company_Code,@Company_Name,@Company_Department,@Dept_DBCode,@Worksite_Name,@Worksite_Code,@DB_Code)";
                    SqlCommand cmd = new SqlCommand(InsertQuery, dbcl.Conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Country_Code", DDL_WorkCountry.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Country_Name", DDL_WorkCountry.SelectedItem.Text.ToString());
                    cmd.Parameters.AddWithValue("@State_Code", DDL_WorkStates.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Sate_Name", DDL_WorkStates.SelectedItem.Text.ToString());
                    cmd.Parameters.AddWithValue("@WorkRegion_Code", DDL_Region.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@WorkRegion_Name", DDL_Region.SelectedItem.Text.ToString());
                    cmd.Parameters.AddWithValue("@Company_Code", DDL_Company.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Company_Name", DDL_Company.SelectedItem.Text.ToString());
                    cmd.Parameters.AddWithValue("@Company_Department", DDl_Departments.SelectedItem.Text.ToString());
                    cmd.Parameters.AddWithValue("@Dept_DBCode", DDl_Departments.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Worksite_Name", locname);
                    cmd.Parameters.AddWithValue("@Worksite_Code", loccode);
                    cmd.Parameters.AddWithValue("@DB_Code", WKSID);
                    cmd.ExecuteNonQuery();
                    dbcl.DisconnectDb();
                    dbcl.Conn.Close();

                    txt_worksite_Name.Text = "";
                    txt_worksite_code.Text = "";

                    string CmdString2 = "select * from tlb_atsworksites where WorkRegion_Code = '" + Session["REGION"].ToString() + "' order by Id";
                    BindGrid(CmdString2);

                    lbl_msg.Text = "Success : Data Saved";

                    string title = "Notifications :";
                    string body = "Success : Data Saved";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                catch (Exception ex)
                {
                    lbl_msg.Text = "Error : " + ex.Message;

                    string title = "Notifications :";
                    string body = ex.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
            }
            else
            {
                txt_worksite_Name.Focus();
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            string CmdString2 = "select * from tlb_atsworksites where WorkRegion_Code = '" + Session["REGION"].ToString() + "' order by Id";
            BindGrid(CmdString2);
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            string CmdString2 = "select * from tlb_atsworksites where WorkRegion_Code = '" + Session["REGION"].ToString() + "' order by Id";
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

            TextBox sitename = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_Worksite_Name");
            string new_locname = sitename.Text.ToString();

            TextBox sitecode = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_Worksite_Code");
            string new_loccode = sitecode.Text.ToString();

            DropDownList deptname = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDL_NewDept");
            string new_deptname = deptname.SelectedItem.Text.ToString();
            string new_deptcode = deptname.SelectedValue.ToString();

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "UPDATE tlb_atsworksites set Dept_DBCode=@Dept_DBCode, Company_Department=@Company_Department, Worksite_Name=@Worksite_Name, Worksite_Code=@Worksite_Code where Id='" + id + "' and DB_Code='" + dbcode + "'  ";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Dept_DBCode", new_deptcode);
                cmd.Parameters.AddWithValue("@Company_Department", new_deptname);
                cmd.Parameters.AddWithValue("@Worksite_Name", new_locname);
                cmd.Parameters.AddWithValue("@Worksite_Code", new_loccode);
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
            string CmdString2 = "select * from tlb_atsworksites where WorkRegion_Code = '" + Session["REGION"].ToString() + "' order by Id";
            BindGrid(CmdString2);
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            Label DBCode = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_DB_Code");
            string dbcode = DBCode.Text.ToString();

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "delete from tlb_atsworksites where Id='" + id + "' and DB_Code='" + dbcode + "'  ";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                dbcl.Conn.Close();

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

            string CmdString2 = "select * from tlb_atsworksites where WorkRegion_Code = '" + Session["REGION"].ToString() + "' order by Id";
            BindGrid(CmdString2);
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && GridView1.EditIndex == e.Row.RowIndex)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var DropDownList1 = e.Row.FindControl("DDL_NewDept") as DropDownList;
                    if (DropDownList1 != null)
                    {
                        var dt = new DataTable();
                        string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString();
                        using (var con = new SqlConnection(cnnString))
                        {
                            con.Open();
                            var cmd = new SqlCommand("Select Company_Department,DB_Code from tlb_workregion_compdept where Work_Region_Code= '" + Session["REGION"].ToString() + "' order by Id", con);
                            var da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                        }

                        DropDownList1.DataSource = dt;
                        DropDownList1.DataTextField = "Company_Department";
                        DropDownList1.DataValueField = "DB_Code";
                        DropDownList1.DataBind();
                        string selectedCity = DataBinder.Eval(e.Row.DataItem, "Company_Department").ToString();
                        DropDownList1.Items.FindByText(selectedCity).Selected = true;
                    }
                }
            }
        }
    }
}