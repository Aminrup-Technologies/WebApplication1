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
    public partial class department_locations : System.Web.UI.Page
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
                    string CmdString1 = "select Country_Name, Country_Code from tlb_work_country";
                    BindCountry(CmdString1);

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

                    

                    if (Session["USTATE"].ToString() == "PI")
                    {
                        string CmdString2 = "select * from tlb_workregion_compdept_loc order by Id";
                        BindGrid(CmdString2);
                    }
                    else
                    {
                        string CmdString3 = "select * from tlb_workregion_compdept_loc where Work_Region_Code = '" + Session["REGION"].ToString() + "' order by Id";
                        BindGrid(CmdString3);

                        DDL_WorkCountry.SelectedValue = "IN";
                        DDL_WorkCountry.Enabled = false;

                        string CmdString3a = "select State_Name, State_Code from tlb_work_state where Country_Code = 'IN' and State_Code='" + Session["USTATE"].ToString() + "' order by Id";
                        BindCountryState(CmdString3a);

                        DDL_WorkStates.SelectedValue = Session["USTATE"].ToString();
                        DDL_WorkStates.Enabled = false;


                        string CmdString4 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = 'IN' and State_Code ='" + Session["USTATE"].ToString() + "' and Work_Region_Code='" + Session["REGION"].ToString() + "' order by Id ";
                        BindRegions(CmdString4);

                        DDL_Region.SelectedValue = Session["REGION"].ToString();
                        DDL_Region.Enabled = false;


                        string CmdString5 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='" + Session["USTATE"].ToString() + "' and Work_Region_Code = '" + Session["REGION"].ToString() + "' and Company_Code = '" + Session["COMPANY_CODE"].ToString() + "' order by Id ";
                        BindCompany(CmdString5);

                        DDL_Company.SelectedValue = Session["COMPANY_CODE"].ToString();
                        DDL_Company.Enabled = false;


                        string CmdString3b = "select Company_Department, DB_Code from tlb_workregion_compdept where Country_Code = 'IN' and State_Code ='" + Session["USTATE"].ToString() + "' and Work_Region_Code = '" + Session["REGION"].ToString() + "' and Company_Code = '" + Session["COMPANY_CODE"].ToString() + "'  order by Id ";
                        BindCompanyDept(CmdString3b);
                    }
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
            if (Session["USTATE"].ToString() == "PI")
            {
                string CmdString3a = "select State_Name, State_Code from tlb_work_state where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "'";
                BindCountryState(CmdString3a);

                string CmdString2 = "select * from tlb_workregion_compdept_loc where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' order by Id";
                BindGrid(CmdString2);
            }
            else
            {
                string CmdString3 = "select State_Name, State_Code from tlb_work_state where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "'";
                BindCountryState(CmdString3);

                string CmdString2 = "select * from tlb_workregion_compdept_loc where Country_Code = 'IN' and State_Code='" + Session["USTATE"].ToString() + "' and Work_Region_Code ='" + Session["REGION"].ToString() + "' order by Id";
                BindGrid(CmdString2);
            }
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
            if (Session["USTATE"].ToString() == "PI")
            {
                string CmdString2 = "select * from tlb_workregion_compdept_loc where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' order by Id";
                BindGrid(CmdString2);

                string CmdString3 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' order by Id ";
                BindRegions(CmdString3);
            }
            else
            {
                string CmdString3 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' order by Id ";
                BindRegions(CmdString3);
            }
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
            if (Session["USTATE"].ToString() == "PI")
            {
                string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' and Work_Region_Code = '" + DDL_Region.SelectedValue.ToString() + "' order by Id ";
                BindCompany(CmdString3);

                string CmdString2 = "select * from tlb_workregion_compdept_loc where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' and Work_Region_Code='" + DDL_Region.SelectedValue.ToString() + "' order by Id";
                BindGrid(CmdString2);
            }
            else
            {
                string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' and Work_Region_Code = '" + DDL_Region.SelectedValue.ToString() + "' order by Id ";
                BindCompany(CmdString3);
            }
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
            if (Session["USTATE"].ToString() == "PI")
            {
                string CmdString3 = "select Company_Department, DB_Code from tlb_workregion_compdept where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' and Work_Region_Code = '" + DDL_Region.SelectedValue.ToString() + "' and Company_Code = '" + DDL_Company.SelectedValue.ToString() + "'  order by Id ";
                BindCompanyDept(CmdString3);

                string CmdString2 = "select * from tlb_workregion_compdept_loc where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' and Work_Region_Code='" + DDL_Region.SelectedValue.ToString() + "' and Company_Code = '"+DDL_Company.SelectedValue.ToString()+"' order by Id";
                BindGrid(CmdString2);
            }
            else
            {
                string CmdString3 = "select Company_Department, DB_Code from tlb_workregion_compdept where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' and Work_Region_Code = '" + DDL_Region.SelectedValue.ToString() + "' and Company_Code = '" + DDL_Company.SelectedValue.ToString() + "'  order by Id ";
                BindCompanyDept(CmdString3);
            }
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
            string cmdString1 = "select Id,DB_Code from tlb_workregion_compdept_loc where Id=(select max(Id)from tlb_workregion_compdept_loc)";
            SqlCommand com1 = new SqlCommand(cmdString1, dbcl.Conn);
            SqlDataReader DR1 = com1.ExecuteReader();
            if (DR1.Read())
            {
                aa = DR1.GetValue(1).ToString();
                string bb = aa.Substring(5);
                int k = Convert.ToInt32(bb);
                k = k + 1;
                string q = Convert.ToString(k);
                kk = "LOC00" + q;
            }
            else
            {
                kk = "LOC001";
            }
            dbcl.DisconnectDb();
            return kk;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string locname = txt_locname.Text.ToUpper().ToString();
            string loccode = txt_loccode.Text.ToUpper().ToString();

            string LOCID = Find_DBCode();

            if (locname != string.Empty && loccode != string.Empty)
            {
                try
                {
                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    string InsertQuery = "INSERT into tlb_workregion_compdept_loc(Country_Code,State_Code,Work_Region_Code,Company_Code,Company_Department,Dept_DBCode,CompDept_Location,CompDept_Location_Code,DB_Code) VALUES(@Country_Code,@State_Code,@Work_Region_Code,@Company_Code,@Company_Department,@Dept_DBCode,@CompDept_Location,@CompDept_Location_Code,@DB_Code)";
                    SqlCommand cmd = new SqlCommand(InsertQuery, dbcl.Conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Country_Code", DDL_WorkCountry.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@State_Code", DDL_WorkStates.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Work_Region_Code", DDL_Region.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Company_Code", DDL_Company.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Company_Department", DDl_Departments.SelectedItem.Text.ToString());
                    cmd.Parameters.AddWithValue("@Dept_DBCode", DDl_Departments.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@CompDept_Location", locname);
                    cmd.Parameters.AddWithValue("@CompDept_Location_Code", loccode);
                    cmd.Parameters.AddWithValue("@DB_Code", LOCID);
                    cmd.ExecuteNonQuery();
                    dbcl.DisconnectDb();
                    dbcl.Conn.Close();

                    txt_locname.Text = "";
                    txt_loccode.Text = "";

                    string CmdString2 = "select * from tlb_workregion_compdept_loc where Work_Region_Code = '" + Session["REGION"].ToString() + "' order by Id";
                    BindGrid(CmdString2);

                    lbl_msg.Text = "Success : Data Saved";
                }
                catch (Exception ex)
                {
                    lbl_msg.Text = "Error : " + ex.Message;
                    //throw;
                }
            }
            else
            {
                txt_locname.Focus();
            }
        }


        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;

            GridBinder();

            //string CmdString2 = "select * from tlb_workregion_compdept_loc where Work_Region_Code = '" + Session["REGION"].ToString() + "' order by Id";
            //BindGrid(CmdString2);
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;

            GridBinder();

            //string CmdString2 = "select * from tlb_workregion_compdept_loc where Work_Region_Code = '" + Session["REGION"].ToString() + "' order by Id";
            //BindGrid(CmdString2);
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            Label DBCode = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_DB_Code");
            string dbcode = DBCode.Text.ToString();

            TextBox locname = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_CompDept_Location");
            string new_locname = locname.Text.ToUpper().ToString();

            TextBox loccode = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_CompDept_Location_Code");
            string new_loccode = loccode.Text.ToUpper().ToString();

            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdString = "UPDATE tlb_workregion_compdept_loc set CompDept_Location=@CompDept_Location, CompDept_Location_Code=@CompDept_Location_Code where Id='" + id + "' and DB_Code='" + dbcode + "'  ";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@CompDept_Location", new_locname);
            cmd.Parameters.AddWithValue("@CompDept_Location_Code", new_loccode);
            cmd.CommandTimeout = 0;
            cmd.ExecuteNonQuery();
            dbcl.Conn.Close();

            GridView1.EditIndex = -1;

            GridBinder();

            //string CmdString2 = "select * from tlb_workregion_compdept_loc where Work_Region_Code = '" + Session["REGION"].ToString() + "' order by Id";
            //BindGrid(CmdString2);
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            Label DBCode = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_DB_Code");
            string dbcode = DBCode.Text.ToString();

            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdString = "delete from tlb_workregion_compdept_loc where Id='" + id + "' and DB_Code='" + dbcode + "'  ";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            cmd.ExecuteNonQuery();
            dbcl.Conn.Close();

            GridBinder();

            //string CmdString2 = "select * from tlb_workregion_compdept_loc where Work_Region_Code = '" + Session["REGION"].ToString() + "' order by Id";
            //BindGrid(CmdString2);
        }

        protected void DDl_Departments_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridBinder();
        }


        private void GridBinder()
        {
            string country = DDL_WorkCountry.SelectedValue.ToString();
            string state = DDL_WorkStates.SelectedValue.ToString();
            string region = DDL_Region.SelectedValue.ToString();
            string company = DDL_Company.SelectedValue.ToString();
            string compdept = DDl_Departments.SelectedValue.ToString();


            if (DDl_Departments.SelectedIndex == 0)
            {
                string CmdString2 = "select * from tlb_workregion_compdept_loc where Work_Region_Code = '" + Session["REGION"].ToString() + "' order by Id";
                BindGrid(CmdString2);
            }
            else
            {
                string CmdString2 = "select * from tlb_workregion_compdept_loc where Country_Code='" + country + "' and State_Code='" + state + "' and Work_Region_Code = '" + region + "' and Company_Code='" + company + "' and Dept_DBCode='" + compdept + "' order by Id";
                BindGrid(CmdString2);
            }
        }
    }
}