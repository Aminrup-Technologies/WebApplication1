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

        public static string state = string.Empty;
        public static string region = string.Empty;
        public static string comp = string.Empty;
        public static string datalock = string.Empty;


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

                        string CmdString3 = "select State_Name, State_Code from tlb_work_state where Country_Code = 'IN' and State_Code='" + state + "' order by Id";
                        BindCountryState(CmdString3);

                        DDL_WorkStates.SelectedValue = state;
                        DDL_WorkStates.Enabled = false;


                        string CmdString4 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = 'IN' and State_Code ='" + state + "' and Work_Region_Code='" + region + "' order by Id ";
                        BindRegions(CmdString4);

                        DDL_Region.SelectedValue = region;
                        DDL_Region.Enabled = false;


                        string CmdString5 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='" + state + "' and Work_Region_Code = '" + region + "' and Company_Code = '" + comp + "' order by Id ";
                        BindCompany(CmdString5);

                        DDL_Company.SelectedValue = comp;
                        DDL_Company.Enabled = false;


                        string CmdString3a = "select Category_Type, Category_DB from tlb_payroll_category where Country_Code = 'IN' and State_Code ='" + state + "' and WorkRegion_Code = '" + region + "' and Company_Code='" + comp + "' order by Id ";
                        BindSkillCategory(CmdString3a);


                        string CmdString2 = "select * from tlb_payroll_wages where Country_Code='IN' and State_Code='" + state + "' and WorkRegion_Code='" + region + "' and Company_Code='" + comp + "' and Status='Active' order by Id";
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

                string CmdString2 = "select * from tlb_payroll_wages where Country_Code = 'IN' and State_Code='" + state + "' and WorkRegion_Code ='" + region + "' order by Id";
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
                string CmdString2 = "select * from tlb_payroll_wages where Country_Code = 'IN' and State_Code ='" + state + "' and WorkRegion_Code='" + region + "' and Company_Code = '" + comp + "' order by Id";
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
                string CmdString2 = "select * from tlb_payroll_wages where Country_Code = 'IN' and State_Code ='" + state + "' and WorkRegion_Code='" + region + "' and Company_Code = '" + comp + "' and Category_DB='"+DDl_Category_type.SelectedValue.ToString()+ "' and Status='Active' order by Id";
                BindGrid(CmdString2);
            }
        }

        public class PayrollWages
        {
            public string Country_Code { get; set; }
            public string Country_Name { get; set; }
            public string State_Code { get; set; }
            public string State_Name { get; set; }
            public string WorkRegion_Code { get; set; }
            public string WorkRegion_Name { get; set; }
            public string Company_Name { get; set; }
            public string Company_Code { get; set; }
            public string Category_DB { get; set; }
            public string Category_Type { get; set; }
            public string Category_Code { get; set; }
            public string Wages_DB { get; set; }
            public decimal Wages_Rate { get; set; }
            public decimal VDA_Rate { get; set; }
            public decimal Total_Wages { get; set; }
            public DateTime Active_Date { get; set; }
            public string Status { get; set; }
        }

        public string GenerateOutput(string variable1, string variable2)
        {
            string output = variable1;

            switch (variable2)
            {
                case "UN-SKILLED":
                    output += "-UN";
                    break;
                case "SKILLED":
                    output += "-S";
                    break;
                case "SEMI-SKILLED":
                    output += "-SS";
                    break;
                case "HIGHLY-SKILLED":
                    output += "-HS";
                    break;
                default:
                    // Handle other cases as needed
                    break;
            }

            return output;
        }

        private string Find_DBCode()
        {
            string aa = null;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string kk = null;
            string cmdString1 = "select Id,Wages_DB from tlb_payroll_wages where Id=(select max(Id)from tlb_payroll_wages)";
            SqlCommand com1 = new SqlCommand(cmdString1, dbcl.Conn);
            SqlDataReader DR1 = com1.ExecuteReader();
            if (DR1.Read())
            {
                aa = DR1.GetValue(1).ToString();
                string bb = aa.Substring(5);
                int k = Convert.ToInt32(bb);
                k = k + 1;
                string q = Convert.ToString(k);
                kk = "WGR00" + q;
            }
            else
            {
                kk = "WGR001";
            }
            dbcl.DisconnectDb();
            return kk;
        }



        protected void btn_submit_Click(object sender, EventArgs e)
        {
            PayrollWages payrollWages = new PayrollWages();
            payrollWages.Country_Code = DDL_WorkCountry.SelectedValue.ToString();
            payrollWages.Country_Name = DDL_WorkCountry.SelectedItem.Text.ToString();
            payrollWages.State_Code = DDL_WorkStates.SelectedValue.ToString();
            payrollWages.State_Name = DDL_WorkStates.SelectedItem.Text.ToString();
            payrollWages.WorkRegion_Code = DDL_Region.SelectedValue.ToString();
            payrollWages.WorkRegion_Name = DDL_Region.SelectedItem.Text.ToString();
            payrollWages.Company_Name = DDL_Company.SelectedItem.Text.ToString();
            payrollWages.Company_Code = DDL_Company.SelectedValue.ToString();
            payrollWages.Category_DB = DDl_Category_type.SelectedValue.ToString();
            payrollWages.Category_Type = DDl_Category_type.SelectedItem.Text.ToString();

            string output = GenerateOutput(payrollWages.Company_Code, payrollWages.Category_Type);

            payrollWages.Category_Code = output;

            string DBID = Find_DBCode();
            payrollWages.Wages_DB = DBID;
            payrollWages.Wages_Rate = Convert.ToDecimal(txt_WagesRate.Text.ToString());
            payrollWages.VDA_Rate = Convert.ToDecimal(txt_VDARate.Text.ToString());
            payrollWages.Total_Wages = Convert.ToDecimal(txt_totalRate.Text.ToString());
            payrollWages.Active_Date = Convert.ToDateTime(txt_effdt.Text.ToString());
            payrollWages.Status = "Active";

            InsertPayrollWages(payrollWages.Country_Code, payrollWages.Country_Name, payrollWages.State_Code, payrollWages.State_Name, payrollWages.WorkRegion_Code, payrollWages.WorkRegion_Name, payrollWages.Company_Name, payrollWages.Company_Code, payrollWages.Category_DB, payrollWages.Category_Type, payrollWages.Category_Code, payrollWages.Wages_DB, payrollWages.Wages_Rate, payrollWages.VDA_Rate, payrollWages.Total_Wages, payrollWages.Active_Date, payrollWages.Status);


            //DataSaver(); ------------ PENDING
        }

        public void InsertPayrollWages(string Country_Code, string Country_Name, string State_Code, string State_Name, string WorkRegion_Code, string WorkRegion_Name, string Company_Name, string Company_Code, string Category_DB, string Category_Type, string Category_Code, string Wages_DB, decimal Wages_Rate, decimal VDA_Rate, decimal Total_Wages, DateTime Active_Date, string Status)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                //connection = new SqlConnection(connectionString);
                command = new SqlCommand("InsertPayrollWages", dbcl.Conn);
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters
                command.Parameters.AddWithValue("@Country_Code", Country_Code);
                command.Parameters.AddWithValue("@Country_Name", Country_Name);
                command.Parameters.AddWithValue("@State_Code", State_Code);
                command.Parameters.AddWithValue("@State_Name", State_Name);
                command.Parameters.AddWithValue("@WorkRegion_Code", WorkRegion_Code);
                command.Parameters.AddWithValue("@WorkRegion_Name", WorkRegion_Name);
                command.Parameters.AddWithValue("@Company_Name", Company_Name);
                command.Parameters.AddWithValue("@Company_Code", Company_Code);
                command.Parameters.AddWithValue("@Category_DB", Category_DB);
                command.Parameters.AddWithValue("@Category_Type", Category_Type);
                command.Parameters.AddWithValue("@Category_Code", Category_Code);
                command.Parameters.AddWithValue("@Wages_DB", Wages_DB);
                command.Parameters.AddWithValue("@Wages_Rate", Wages_Rate);
                command.Parameters.AddWithValue("@VDA_Rate", VDA_Rate);
                command.Parameters.AddWithValue("@Total_Wages", Total_Wages);
                command.Parameters.AddWithValue("@Active_Date", Active_Date);
                command.Parameters.AddWithValue("@Status", Status);

                //connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Handle the exception as needed, e.g., log the error or throw it further.
                // For simplicity, rethrow the exception here.
                //throw ex;
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            finally
            {
                // Close the connection and command in the finally block to ensure proper cleanup.
                if (command != null)
                {
                    command.Dispose();
                }
                dbcl.DisconnectDb();
            }
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