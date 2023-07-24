using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Drawing;

namespace WebApplication1.bussiness.production
{
    public partial class pyrl_managedashbrd : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        public static Int32 PayrollFactorStatus = 0;
        public static Int32 BankFactorStatus = 0;
        public static Int32 activeDedEmpCount = 0;
        public static Int32 inactiveDedEmpCount = 0;
        public static Int32 worksiteCount = 0;

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
                    lbl_factorsstatus.Text = "***";
                    lbl_bankdatastatus.Text = "***";
                    lbl_activedeductions.Text = "***";
                    lbl_inactivedeductions.Text = "***";
                    lbl_activeworksites.Text = "***";

                    if (Session["Changer"] != null)
                    {
                        string[] retrievedArray = (string[])Session["Changer"];
                        region = retrievedArray[1].ToString();
                        comp = retrievedArray[2].ToString();
                        state = retrievedArray[0].ToString();
                        datalock = retrievedArray[3].ToString();
                        //Session["Changer"] = null;

                        if (datalock == "1")
                        {
                            btn_datalocker.Text = "Un-Lock";
                            btn_datalocker.CssClass = "btn btn-danger btn-sm";
                        }
                        else
                        {
                            btn_datalocker.Text = "Lock";
                            btn_datalocker.CssClass = "btn btn-success btn-sm";
                        }
                    }
                    else
                    {
                        region = Session["REGION"].ToString();
                        comp = Session["COMPANY_CODE"].ToString();
                        state = Session["STATE"].ToString();
                        datalock = "0";
                    }

                    if (Session["WORKMAN"].ToString() == "J8")
                    {
                        AGL_F17.Visible = false; KPO_F17.Visible = false; NINL_F17.Visible = false; JSR_F17.Visible = false; ATS_F17.Visible = true;
                        StateSelector.Visible = true; RegionSelector.Visible = true; RegionComSelector.Visible = true;
                        //CheckforUser();
                    }
                    else
                    {
                        AGL_F17.Visible = false; KPO_F17.Visible = false; NINL_F17.Visible = false; JSR_F17.Visible = false; ATS_F17.Visible = false;
                        StateSelector.Visible = false; DDL_WorkStates.Enabled = false;
                    }

                    PageLoaderData();   
                }
            }
        }

        private void PageLoaderData()
        {
            string CmdString3 = "select State_Name, State_Code from tlb_work_state";
            BindCountryState(CmdString3);

            string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region order by Id";
            BindWorkRegion(CmdString1);

            string CmdString2 = "select Company_Name, Company_Code from tlb_workregion_company order by Id";
            BindCompany(CmdString2);

            PayrollFactorsInputs_Checker(state, region, comp);
            BindSP_GetEmpBankFactor_Status(state, region, comp);
            BindDeductionStatus(state, region, comp);
            GetWorksiteCount("IN", state, region, comp);
            Indicator();

            DDL_WorkRegion.SelectedValue = region;
            DDL_WorkRegion.Enabled = false;

            DDL_WorkStates.SelectedValue = state;
            DDL_WorkStates.Enabled = true;

            DDL_Company.SelectedValue = comp;
            DDL_Company.Enabled = false;

            string[] Bindervalue = { state, region, comp, "1"};
            Session["Changer"] = null;
            Session["Changer"] = Bindervalue;
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

        private void CheckforUser()
        {
            DDL_WorkRegion.SelectedValue = Session["REGION"].ToString();
            DDL_WorkRegion.Enabled = false;

            DDL_WorkStates.SelectedValue = Session["STATE"].ToString();
            DDL_WorkStates.Enabled = true;

            DDL_Company.SelectedValue = Session["COMPANY_CODE"].ToString();
            DDL_Company.Enabled = false;
        }

        private void BindWorkRegion(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_WorkRegion.DataSource = Cmd.ExecuteReader();
            DDL_WorkRegion.DataTextField = "Work_Region_Name";
            DDL_WorkRegion.DataValueField = "Work_Region_Code";
            DDL_WorkRegion.DataBind();
            DDL_WorkRegion.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_WorkRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_factorsstatus.Text = "***";
            lbl_bankdatastatus.Text = "***";
            lbl_activedeductions.Text = "***";
            lbl_inactivedeductions.Text = "***";
            lbl_activeworksites.Text = "***";

            string DDL_String = DDL_WorkRegion.SelectedItem.Text.ToString();
            string DDL_Value = DDL_WorkRegion.SelectedValue.ToString();

            string CmdString2 = "select Company_Name, Company_Code from tlb_workregion_company where Work_Region_Code='" + DDL_Value + "' order by Id";
            BindCompany(CmdString2);

            DDL_Company.Enabled = true;
            RegionComSelector.Visible = true;
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
            string DDL_StateValue = DDL_WorkStates.SelectedValue.ToString();
            string DDL_Value = DDL_WorkRegion.SelectedValue.ToString();
            string DDL_CompValue = DDL_Company.SelectedValue.ToString();
            
            lbl_factorsstatus.Text = "***";
            lbl_bankdatastatus.Text = "***";
            lbl_activedeductions.Text = "***";
            lbl_inactivedeductions.Text = "***";
            lbl_activeworksites.Text = "***";

            PayrollFactorsInputs_Checker(DDL_StateValue, DDL_Value, DDL_CompValue);
            BindSP_GetEmpBankFactor_Status(DDL_StateValue, DDL_Value, DDL_CompValue);
            BindDeductionStatus(DDL_StateValue, DDL_Value, DDL_CompValue);
            GetWorksiteCount("IN", DDL_StateValue, DDL_Value, DDL_CompValue);
            Indicator();

            string[] Bindervalue = { DDL_StateValue, DDL_Value, DDL_CompValue, "1" };
            Session["Changer"] = null;
            Session["Changer"] = Bindervalue;

            DataLocker.Visible = true;
            btn_datalocker.Enabled = true;
        }


        private void PayrollFactorsInputs_Checker(string DDL_StateValue, string DDL_Value, string DDL_CompValue)
        {
            // Set up the database connection
            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create the SqlCommand object for calling the stored procedure
                SqlCommand command = new SqlCommand("SP_GetEmpPayrollFactor_Status", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Add input parameter
                command.Parameters.AddWithValue("@RegionID", DDL_Value);
                command.Parameters.AddWithValue("@CompanyID", DDL_CompValue);
                command.Parameters.AddWithValue("@WorkState", DDL_StateValue);

                // Add output parameters
                command.Parameters.Add("@EmployeeCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@HS_EmpCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@S_EmpCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@SS_EmpCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@US_EmpCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@F16_Yes_EmpCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@F16_No_EmpCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@F17_Yes_EmpCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@F17_No_EmpCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@FS_Yes_NZCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@FS_Yes_ZCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@FS_No_ZCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@FS_No_NZCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@CompleteDataEmpCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@InCompleteDataEmpCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@Status", SqlDbType.Int).Direction = ParameterDirection.Output;

                // Open the connection and execute the stored procedure
                connection.Open();
                command.ExecuteNonQuery();

                // Retrieve the values of the output parameters
                int employeeCount = Convert.ToInt32(command.Parameters["@EmployeeCount"].Value);
                int hsEmpCount = Convert.ToInt32(command.Parameters["@HS_EmpCount"].Value);
                int sEmpCount = Convert.ToInt32(command.Parameters["@S_EmpCount"].Value);
                int ssEmpCount = Convert.ToInt32(command.Parameters["@SS_EmpCount"].Value);
                int usEmpCount = Convert.ToInt32(command.Parameters["@US_EmpCount"].Value);
                int f16YesEmpCount = Convert.ToInt32(command.Parameters["@F16_Yes_EmpCount"].Value);
                int f16NoEmpCount = Convert.ToInt32(command.Parameters["@F16_No_EmpCount"].Value);
                int f17YesEmpCount = Convert.ToInt32(command.Parameters["@F17_Yes_EmpCount"].Value);
                int f17NoEmpCount = Convert.ToInt32(command.Parameters["@F17_No_EmpCount"].Value);
                int fsYesnzEmpCount = Convert.ToInt32(command.Parameters["@FS_Yes_NZCount"].Value);
                int fsYeszEmpCount = Convert.ToInt32(command.Parameters["@FS_Yes_ZCount"].Value);
                int fsNozEmpCount = Convert.ToInt32(command.Parameters["@FS_No_ZCount"].Value);
                int fsnonzEmpCount = Convert.ToInt32(command.Parameters["@FS_No_NZCount"].Value);
                int CompDataEmpCount = Convert.ToInt32(command.Parameters["@CompleteDataEmpCount"].Value);
                int InCompDataEmpCount = Convert.ToInt32(command.Parameters["@InCompleteDataEmpCount"].Value);
                int Status = Convert.ToInt32(command.Parameters["@Status"].Value);

                PayrollFactorStatus = Status;
                // Retrieve other output parameters in a similar manner

                // Close the connection
                connection.Close();

            }
        }

        public void BindSP_GetEmpBankFactor_Status(string DDL_StateValue, string regionID, string companyID)
        {
            // Set up the database connection
            string connectionString1 = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection connection1 = new SqlConnection(connectionString1))
            {
                // Create the SqlCommand object for calling the stored procedure
                SqlCommand command1 = new SqlCommand("SP_GetEmpBankFactor_Status", connection1);
                command1.CommandType = CommandType.StoredProcedure;

                // Input parameters
                command1.Parameters.AddWithValue("@RegionID", regionID);
                command1.Parameters.AddWithValue("@CompanyID", companyID);
                command1.Parameters.AddWithValue("@WorkState", DDL_StateValue);

                // Output parameters
                command1.Parameters.Add("@EmployeeCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command1.Parameters.Add("@CompleteDataEmpCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command1.Parameters.Add("@InCompleteDataEmpCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command1.Parameters.Add("@Status", SqlDbType.Int).Direction = ParameterDirection.Output;

                // Execute the stored procedure
                connection1.Open();
                command1.ExecuteNonQuery();

                // Get the output parameter values
                int employeeCount = Convert.ToInt32(command1.Parameters["@EmployeeCount"].Value);
                int completeDataEmpCount = Convert.ToInt32(command1.Parameters["@CompleteDataEmpCount"].Value);
                int inCompleteDataEmpCount = Convert.ToInt32(command1.Parameters["@InCompleteDataEmpCount"].Value);
                int status = Convert.ToInt32(command1.Parameters["@Status"].Value);

                BankFactorStatus = status;

                // Close the connection
                connection1.Close();
            }
        }

        public void BindDeductionStatus(string DDL_StateValue, string regionId, string companyId)
        {
            int employeeCount = 0;
            //int activeDedEmpCount = 0;
            int inactiveDedEmpCount = 0;
            int status = 0;

            // Set up the database connection
            string connectionString2 = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection connection2 = new SqlConnection(connectionString2))
            {
                // Create the SqlCommand object for calling the stored procedure
                SqlCommand command2 = new SqlCommand("SP_GetDeduction_Status", connection2);
                command2.CommandType = CommandType.StoredProcedure;

                command2.Parameters.AddWithValue("@RegionID", regionId);
                command2.Parameters.AddWithValue("@CompanyID", companyId);
                command2.Parameters.AddWithValue("@WorkState", DDL_StateValue);

                command2.Parameters.Add("@EmployeeCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command2.Parameters.Add("@ActiveDedEmpCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command2.Parameters.Add("@InactiveDedEmpCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command2.Parameters.Add("@Status", SqlDbType.Int).Direction = ParameterDirection.Output;

                connection2.Open();
                command2.ExecuteNonQuery();

                // Retrieve the output parameter values
                employeeCount = (int)command2.Parameters["@EmployeeCount"].Value;
                activeDedEmpCount = (int)command2.Parameters["@ActiveDedEmpCount"].Value;
                inactiveDedEmpCount = (int)command2.Parameters["@InactiveDedEmpCount"].Value;
                status = (int)command2.Parameters["@Status"].Value;

                connection2.Close();
            }

            lbl_inactivedeductions.Visible = true;
            lbl_inactivedeductions.Text = inactiveDedEmpCount.ToString();

            lbl_activedeductions.Visible = true;
            lbl_activedeductions.Text = activeDedEmpCount.ToString();

            // Perform further operations with the retrieved values
            // For example, you can bind them to your UI controls
            // or use them for further processing
        }

        public void GetWorksiteCount(string countryCode, string stateCode, string workRegionCode, string companyCode)
        {
            //int worksiteCount = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("GetWorksiteCount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Input parameters
                        command.Parameters.AddWithValue("@CountryCode", countryCode);
                        command.Parameters.AddWithValue("@StateCode", stateCode);
                        command.Parameters.AddWithValue("@WorkRegionCode", workRegionCode);
                        command.Parameters.AddWithValue("@CompanyCode", companyCode);

                        // Output parameter
                        SqlParameter worksiteCountParam = new SqlParameter("@WorksiteCount", SqlDbType.Int);
                        worksiteCountParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(worksiteCountParam);

                        command.ExecuteNonQuery();

                        worksiteCount = (int)command.Parameters["@WorksiteCount"].Value;
                        lbl_activeworksites.Visible = true;
                        lbl_activeworksites.Text = worksiteCount.ToString();
                        div_activeworksites.Attributes["class"] = "badge bg-blue";
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions here
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            //return worksiteCount;
        }
        private void Indicator()
        {
            if (PayrollFactorStatus == 1)
            {
                lbl_factorsstatus.Visible = true;
                lbl_factorsstatus.Text = "Error!";
                div_factorsstatus.Attributes["class"] = "badge bg-red";
            }
            else
            {
                lbl_factorsstatus.Visible = true;
                lbl_factorsstatus.Text = "Ok";
                div_factorsstatus.Attributes["class"] = "badge bg-green";
            }

            if (BankFactorStatus == 1)
            {
                lbl_bankdatastatus.Visible = true;
                lbl_bankdatastatus.Text = "Error!";
                div_bankdatastatus.Attributes["class"] = "badge bg-red";
            }
            else
            {
                lbl_bankdatastatus.Visible = true;
                lbl_bankdatastatus.Text = "Ok";
                div_bankdatastatus.Attributes["class"] = "badge bg-green";
            }


            if (PayrollFactorStatus == 1 && BankFactorStatus == 1)
            {
                ATS_F17.Disabled = false;
            }
            else
            {
                ATS_F17.Disabled = true;
            }
        }

        protected void DDL_WorkStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where State_Code ='"+DDL_WorkStates.SelectedValue.ToString()+"' order by Id";
            BindWorkRegion(CmdString1);

            RegionSelector.Visible= true;
            DDL_WorkRegion.Enabled = true;

            //DDL_WorkRegion.Enabled = false;
            DDL_Company.Enabled = false;
        }

        protected void btn_datalocker_Click(object sender, EventArgs e)
        {
            string DDL_StateValue = DDL_WorkStates.SelectedValue.ToString();
            string DDL_Value = DDL_WorkRegion.SelectedValue.ToString();
            string DDL_CompValue = DDL_Company.SelectedValue.ToString();

            if (btn_datalocker.Text=="Lock")
            {
                string[] Bindervalue = { DDL_StateValue, DDL_Value, DDL_CompValue, "1" };
                Session["Changer"] = null;
                Session["Changer"] = Bindervalue;

                DataLocker.Visible = true;
                btn_datalocker.Text = "Un-Lock";
                btn_datalocker.CssClass = "btn btn-danger btn-sm";
            }
            else if (btn_datalocker.Text == "Un-Lock")
            {
                string[] Bindervalue = { DDL_StateValue, DDL_Value, DDL_CompValue, "0" };
                Session["Changer"] = null;
                Session["Changer"] = Bindervalue;

                DataLocker.Visible = true;
                btn_datalocker.Text = "Lock";
                btn_datalocker.CssClass = "btn btn-success btn-sm";
            }
        }
    }
}