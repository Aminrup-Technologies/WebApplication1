using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DocumentFormat.OpenXml.Bibliography;
using System.Drawing;

namespace WebApplication1.bussiness.production
{
    public partial class emp_payroll_wages : System.Web.UI.Page
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

                        string CmdString2 = "select * from tlb_payroll_wages and Status='Active' order by Id";
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


                        string CmdString2 = "select * from tlb_payroll_wages where Country_Code='IN' and State_Code='" + Session["USTATE"].ToString() + "' and WorkRegion_Code='" + Session["REGION"].ToString() + "' and Company_Code='" + Session["COMPANY_CODE"].ToString() + "' and Status='Active' order by Id";
                        BindGrid(CmdString2);
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
                string CmdString3 = "select State_Name, State_Code from tlb_work_state where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "'";
                BindCountryState(CmdString3);

                string CmdString2 = "select * from tlb_payroll_wages where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' order by Id";
                BindGrid(CmdString2);
            }
            else
            {
                string CmdString3 = "select State_Name, State_Code from tlb_work_state where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "'";
                BindCountryState(CmdString3);

                string CmdString2 = "select * from tlb_payroll_wages where Country_Code = 'IN' and State_Code='" + Session["USTATE"].ToString() + "' and WorkRegion_Code ='" + Session["REGION"].ToString() + "' order by Id";
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

        protected void DDL_WorkStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["USTATE"].ToString() == "PI")
            {
                string CmdString2 = "select * from tlb_payroll_wages where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' order by Id";
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

        protected void DDL_Region_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["USTATE"].ToString() == "PI")
            {
                string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' and Work_Region_Code = '" + DDL_Region.SelectedValue.ToString() + "' order by Id ";
                BindCompany(CmdString3);

                string CmdString2 = "select * from tlb_payroll_wages where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' and WorkRegion_Code='" + DDL_Region.SelectedValue.ToString() + "' order by Id";
                BindGrid(CmdString2);
            }
            else
            {
                string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' and Work_Region_Code = '" + DDL_Region.SelectedValue.ToString() + "' order by Id ";
                BindCompany(CmdString3);
            }
        }

        protected void DDL_Company_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["USTATE"].ToString() == "PI")
            {
                string CmdString2 = "select * from tlb_payroll_wages where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' and WorkRegion_Code='" + DDL_Region.SelectedValue.ToString() + "' and Company_Code = '" + DDL_Company.SelectedValue.ToString() + "' order by Id";
                BindGrid(CmdString2);
            }
            else
            {
                string CmdString2 = "select * from tlb_payroll_wages where Country_Code = 'IN' and State_Code ='" + Session["USTATE"].ToString() + "' and WorkRegion_Code='" + Session["REGION"].ToString() + "' and Company_Code = '" + Session["COMPANY_CODE"].ToString() + "' order by Id";
                BindGrid(CmdString2);
            }
        }

        protected void DDl_Category_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["USTATE"].ToString() == "PI")
            {
                string CmdString2 = "select * from tlb_payroll_designation where Country_Code = '" + DDL_WorkCountry.SelectedValue.ToString() + "' and State_Code ='" + DDL_WorkStates.SelectedValue.ToString() + "' and WorkRegion_Code='" + DDL_Region.SelectedValue.ToString() + "' and Company_Code = '" + DDL_Company.SelectedValue.ToString() + "' and Category_DB='" + DDl_Category_type.SelectedValue.ToString() + "' order by Id";
                BindGrid(CmdString2);
            }
            else
            {
                string CmdString2 = "select * from tlb_payroll_wages where Country_Code = 'IN' and State_Code ='" + Session["USTATE"].ToString() + "' and WorkRegion_Code='" + Session["REGION"].ToString() + "' and Company_Code = '" + Session["COMPANY_CODE"].ToString() + "' and Category_DB='"+DDl_Category_type.SelectedValue.ToString()+ "' and Status='Active' order by Id";
                BindGrid(CmdString2);
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            //DataSaver(); ------------ PENDING
        }

        private void DataSaver()
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "INSERT INTO tbl_MonthlyPayrollStatus set F17_TrialStatus=@F17_TrialStatus, F17_TrialTimeStamp=@F17_TrialTimeStamp where PayrollYear=@PayrollYear and PayrollMonth=@PayrollMonth and PayrollRegion=@PayrollRegion and PayrollCompany=@PayrollCompany";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@F17_TrialStatus", "Yes");
                cmd.Parameters.AddWithValue("@F17_TrialTimeStamp", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                string title = "Notifications :";
                string body = "Data saved Successfully";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }
    }
}