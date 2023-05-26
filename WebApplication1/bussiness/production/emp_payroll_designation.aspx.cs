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
    public partial class emp_payroll_desgination : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
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
                    if (Session["USTATE"].ToString() == "PI")
                    {
                        string CmdString1 = "select Country_Name, Country_Code from tlb_work_country";
                        BindCountry(CmdString1);

                        string CmdString2 = "select * from tlb_payroll_designation order by Id";
                        BindGrid(CmdString2);
                    }
                    else
                    {
                        string CmdString1 = "select Country_Name, Country_Code from tlb_work_country";
                        BindCountry(CmdString1);

                        DDL_WorkCountry.SelectedValue = "IN";
                        DDL_WorkCountry.Enabled = false;

                        string CmdString3 = "select State_Name, State_Code from tlb_work_state where Country_Code = 'IN' and State_Code='" + Session["USTATE"].ToString() + "' order by Id";
                        BindCountryState(CmdString3);

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

                        string CmdString3a = "select Category_Type, Category_DB from tlb_payroll_category where Country_Code = 'IN' and State_Code ='" + Session["USTATE"].ToString() + "' and WorkRegion_Code = '" + Session["REGION"].ToString() + "' and Company_Code='" + Session["COMPANY_CODE"].ToString() + "' order by Id ";
                        BindSkillCategory(CmdString3a);

                        string CmdString2 = "select * from tlb_payroll_designation where Country_Code='IN' and State_Code='" + Session["USTATE"].ToString() + "' and WorkRegion_Code='" + Session["REGION"].ToString() + "' and Company_Code='" + Session["COMPANY_CODE"].ToString() + "' order by Id";
                        BindGrid(CmdString2);
                    }
                }
                //string CmdString1 = "select Country_Name, Country_Code from tlb_work_country";
                //BindCountry(CmdString1);

                //string CmdString2 = "select * from tlb_payroll_designation order by Id";
                //BindGrid(CmdString2);
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

        protected void DDL_WorkCountry_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (Session["USTATE"].ToString() == "PI")
            {
                string CmdString3 = "select State_Name, State_Code from tlb_work_state where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "'";
                BindCountryState(CmdString3);

                string CmdString2 = "select * from tlb_payroll_designation where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' order by Id";
                BindGrid(CmdString2);
            }
            else
            {
                string CmdString3 = "select State_Name, State_Code from tlb_work_state where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "'";
                BindCountryState(CmdString3);

                string CmdString2 = "select * from tlb_payroll_designation where Country_Code = 'IN' and State_Code='" + Session["USTATE"].ToString() + "' and WorkRegion_Code ='" + Session["REGION"].ToString() + "' order by Id";
                BindGrid(CmdString2);
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
            cmd.Dispose();
            GridView1.DataBind();
            dbcl.Conn.Close();
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
                string CmdString2 = "select * from tlb_payroll_designation where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' order by Id";
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

                string CmdString2 = "select * from tlb_payroll_designation where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' and WorkRegion_Code='" + DDL_Region.SelectedValue.ToString() + "' order by Id";
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
                string CmdString2 = "select * from tlb_payroll_designation where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' and WorkRegion_Code='" + DDL_Region.SelectedValue.ToString() + "' and Company_Code = '" + DDL_Company.SelectedValue.ToString() + "' order by Id";
                BindGrid(CmdString2);

                string CmdString3 = "select Category_Type, Category_DB from tlb_payroll_category where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' and WorkRegion_Code = '" + DDL_Region.SelectedValue.ToString() + "' and Company_Code='"+ DDL_Company.SelectedValue.ToString() + "' order by Id ";
                BindSkillCategory(CmdString3);
            }
            else
            {
                string CmdString2 = "select * from tlb_payroll_designation where Country_Code = 'IN' and State_Code ='" + Session["USTATE"].ToString() + "' and WorkRegion_Code='" + Session["REGION"].ToString() + "' and Company_Code = '" + Session["COMPANY_CODE"].ToString() + "' order by Id";
                BindGrid(CmdString2);
            }
        }

        private void BindSkillCategory(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDl_Category_type.DataSource = Cmd.ExecuteReader();
            DDl_Category_type.DataTextField = "Category_Type";
            DDl_Category_type.DataValueField = "Category_DB";
            DDl_Category_type.DataBind();
            DDl_Category_type.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        private string Find_DBCode()
        {
            string aa = null;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string kk = null;
            string cmdString1 = "select Id,Designation_DB from tlb_payroll_designation where Id=(select max(Id)from tlb_payroll_designation)";
            SqlCommand com1 = new SqlCommand(cmdString1, dbcl.Conn);
            SqlDataReader DR1 = com1.ExecuteReader();
            if (DR1.Read())
            {
                aa = DR1.GetValue(1).ToString();
                string bb = aa.Substring(5);
                int k = Convert.ToInt32(bb);
                k = k + 1;
                string q = Convert.ToString(k);
                kk = "DEG00" + q;
            }
            else
            {
                kk = "DEG001";
            }
            dbcl.DisconnectDb();
            return kk;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string deptname = txt_designation_Name.Text.ToUpper().ToString();

            string DPTID = Find_DBCode();


            if (deptname != string.Empty)
            {
                try
                {
                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    string InsertQuery = "INSERT into tlb_payroll_designation(Country_Code,Country_Name,State_Code,State_Name,WorkRegion_Code,WorkRegion_Name,Company_Name,Company_Code,Category_Type,Category_DB,Designation_DB,Designation_Type) VALUES(@Country_Code,@Country_Name,@State_Code,@State_Name,@WorkRegion_Code,@WorkRegion_Name,@Company_Name,@Company_Code,@Category_Type,@Category_DB,@Designation_DB,@Designation_Type)";
                    SqlCommand cmd = new SqlCommand(InsertQuery, dbcl.Conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Country_Code", DDL_WorkCountry.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Country_Name", DDL_WorkCountry.SelectedItem.Text.ToString());
                    cmd.Parameters.AddWithValue("@State_Code", DDL_WorkStates.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@State_Name", DDL_WorkStates.SelectedItem.Text.ToString());
                    cmd.Parameters.AddWithValue("@WorkRegion_Code", DDL_Region.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@WorkRegion_Name", DDL_Region.SelectedItem.Text.ToString());
                    cmd.Parameters.AddWithValue("@Company_Name", DDL_Company.SelectedItem.Text.ToString());
                    cmd.Parameters.AddWithValue("@Company_Code", DDL_Company.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Category_Type", DDl_Category_type.SelectedItem.Text.ToString());
                    cmd.Parameters.AddWithValue("@Category_DB", DDl_Category_type.SelectedValue.ToString());
                    //cmd.Parameters.AddWithValue("@Category_Code", DDl_Category_type.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Designation_Type", deptname);
                    cmd.Parameters.AddWithValue("@Designation_DB", DPTID);
                    cmd.ExecuteNonQuery();
                    dbcl.DisconnectDb();
                    dbcl.Conn.Close();

                    txt_designation_Name.Text = "";

                    string CmdString2 = "select * from tlb_payroll_designation where WorkRegion_Code = '" + Session["REGION"].ToString() + "' AND Company_Code='" + Session["COMPANY_CODE"].ToString() + "' and Category_DB = '"+ DDl_Category_type.SelectedValue.ToString() + "' order by Id";
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
                txt_designation_Name.Focus();
            }
        }

        protected void DDl_Category_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["USTATE"].ToString() == "PI")
            {
                string CmdString2 = "select * from tlb_payroll_designation where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' and WorkRegion_Code='" + DDL_Region.SelectedValue.ToString() + "' and Company_Code = '" + DDL_Company.SelectedValue.ToString() + "' and Category_DB='"+DDl_Category_type.SelectedValue.ToString()+"' order by Id";
                BindGrid(CmdString2);
            }
            else
            {
                string CmdString2 = "select * from tlb_payroll_designation where Country_Code = 'IN' and State_Code ='" + Session["USTATE"].ToString() + "' and WorkRegion_Code='" + Session["REGION"].ToString() + "' and Company_Code = '" + Session["COMPANY_CODE"].ToString() + "' and Category_DB='"+DDl_Category_type.SelectedValue.ToString()+"' order by Id";
                BindGrid(CmdString2);
            }
        }
    }
}