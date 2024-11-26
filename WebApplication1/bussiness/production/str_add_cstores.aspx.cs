using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;


namespace WebApplication1.bussiness.production
{
    public partial class str_add_cstores : System.Web.UI.Page
    {
        public static string PKId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["USERTYPE"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    BindCountry();
                }
            }

        }
        public void BindCountry()
        {
            string query = "SELECT [Country_Name], [Country_Code] FROM [ats_erp].[dbo].[tlb_work_country] ORDER BY [Country_Name]";
            string textField = "Country_Name";
            string valueField = "Country_Code";

            bool recordsBound;

            // Bind the DropDownList and get the flag indicating whether records were bound
            DB_Utility_OH4Y.BindDropDownList(query, DDL_WorkCountry, textField, valueField, out recordsBound);

            // Check if any records were bound
            if (!recordsBound)
            {
                DB_Utility_OH4Y.BindWithDefaultNoRecords(DDL_WorkCountry);
                string BindCountry_Error_script = @"<script type='text/javascript'>
                            new PNotify({
                                title: 'Error',
                                text: 'An error occurred!',
                                type: 'error',
                                styling: 'bootstrap3'
                            });
                        </script>";

                // RegisterStartupScript adds the JavaScript code to the page
                ClientScript.RegisterStartupScript(this.GetType(), "ShowBindCountryErrorNotification", BindCountry_Error_script, false);

            }
        }
        protected void DDL_WorkCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_WorkCountry.SelectedIndex != 0)
            {
                string selectedWorkCountryValue = DDL_WorkCountry.SelectedValue;
                LBL_DDL_WorkCountry_Value.Text = selectedWorkCountryValue;
                BindState(selectedWorkCountryValue);
            }
            else
            {
                DB_Utility_OH4Y.BindWithDefaultNoRecords(DDL_State);

                string DDL_State_Error_script = @"<script type='text/javascript'>
                            new PNotify({
                                title: 'Error',
                                text: 'Invalid Selection!',
                                type: 'error',
                                styling: 'bootstrap3'
                            });
                        </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowStateInvalidErrorNotification", DDL_State_Error_script, false);
            }
        }
        private void BindState(string selectedWorkCountryValue)
        {
            string query = "SELECT [State_Name],[State_Code] FROM [ats_erp].[dbo].[tlb_work_state] ORDER BY [State_Name]";
            string textField = "State_Name";
            string valueField = "State_Code";

            bool recordsBound;
            DB_Utility_OH4Y.BindDropDownList(query, DDL_State, textField, valueField, new SqlParameter("@selectedWorkCountryValue", selectedWorkCountryValue), out recordsBound);

            if (!recordsBound)
            {
                DB_Utility_OH4Y.BindWithDefaultNoRecords(DDL_State);

                string BindState_Error_script = @"<script type='text/javascript'>
                            new PNotify({
                                title: 'Error',
                                text: 'An error occurred!',
                                type: 'error',
                                styling: 'bootstrap3'
                            });
                        </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowBindStateErrorNotification", BindState_Error_script, false);
            }
        }
        protected void DDL_State_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_State.SelectedIndex != 0)
            {
                string selectedWorkCountryValue = DDL_WorkCountry.SelectedValue.ToString();
                string selectedStateValue = DDL_State.SelectedValue.ToString();
                LBL_DDL_State_Value.Text = selectedStateValue;
            }
        }
        private void MakeInputsReadOnly()
        {
            DDL_WorkCountry.Enabled = false;
            DDL_State.Enabled = false;
            TB_CStoreName.ReadOnly = true;
            TB_CStoreCode.ReadOnly = true;
            TB_Manager1_Wrk.ReadOnly = true;
            TB_Manager2_Wrk.ReadOnly = true;
            btn_submit.Enabled = false;
            btn_submit.Text = "SAVED";
            btn_submit.CssClass = "btn btn-sm btn-success";

            string Data_SuccessScript1 = @"<script type='text/javascript'>
                            new PNotify({
                                title: 'Data Success',
                                text: 'Recorded Successfully!!',
                                type: 'success',
                                styling: 'bootstrap3'
                            });
                        </script>";

            // RegisterStartupScript adds the JavaScript code to the page
            ClientScript.RegisterStartupScript(this.GetType(), "ShowDataSuccessNotification", Data_SuccessScript1, false);
        }
        private string GenerateUniqueCSId_PK()
        {

            string newCSIdValue;
            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Fetch the maximum WHId value
                    string query = "SELECT ISNULL(MAX(CAST(SUBSTRING(ID, 4, LEN(ID) - 2) AS INT)), 0) FROM [ats_erp].[dbo].[str_add_CentralStrore]";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();
                        int maxCSIdValue = Convert.ToInt32(result);

                        // Increment the numeric part
                        int numericPart = maxCSIdValue + 1;

                        // Format the new value
                        newCSIdValue = $"CS{numericPart:D3}"; // Ensure three digits (e.g., CS001, CS002)
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error generating CSId_PK: " + ex.Message);
                throw;
            }

            PKId = newCSIdValue;
            return newCSIdValue;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)

        {
            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;

            string CountryName = DDL_WorkCountry.SelectedItem.Text;
            string CountryCode = DDL_WorkCountry.SelectedValue;
            string StateName = DDL_State.SelectedItem.Text;
            string StateCode = DDL_State.SelectedValue;
            string CStoreName = TB_CStoreName.Text;
            string CStoreCode = TB_CStoreCode.Text;
            string Manager1Code = TB_Manager1_Wrk.Text;
            string Manager1Name = Lbl_Manager1_Name.Text.StartsWith("Employee Name: ") ? Lbl_Manager1_Name.Text.Replace("Employee Name: ", "").Trim() : null; ;
            string Manager2Code = TB_Manager2_Wrk.Text;
            string Manager2Name = Lbl_Manager2_Name.Text.StartsWith("Employee Name: ") ? Lbl_Manager2_Name.Text.Replace("Employee Name: ", "").Trim() : null;
            DateTime TimeStamp = DateTime.Now;  // Current Date
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // SQL Insert Query
                    string query = @"INSERT INTO [ats_erp].[dbo].[str_add_CentralStrore] (ID ,Country_Name ,Country_Code ,State_Name ,State_Code ,CS_Name ,CS_Code ,StoreManager1_EmpCode ,StoreManager1_Name ,StoreManager2_EmpCode , StoreManager2_Name,TimeStamp) VALUES (@ID, @Country_Name, @Country_Code, @State_Name ,@State_Code ,@CS_Name ,@CS_Code, @StoreManager1_EmpCode, @StoreManager1_Name, @StoreManager2_EmpCode, @StoreManager2_Name, @TimeStamp)";


                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@ID", GenerateUniqueCSId_PK());
                        cmd.Parameters.AddWithValue("@Country_Name", CountryName);
                        cmd.Parameters.AddWithValue("@Country_Code", CountryCode);
                        cmd.Parameters.AddWithValue("@State_Name", StateName);
                        cmd.Parameters.AddWithValue("@State_Code", StateCode);
                        cmd.Parameters.AddWithValue("@CS_Name", CStoreName);
                        cmd.Parameters.AddWithValue("@CS_Code", CStoreCode);
                        cmd.Parameters.AddWithValue("@StoreManager1_EmpCode", Manager1Code);
                        cmd.Parameters.AddWithValue("@StoreManager1_Name", Manager1Name);
                        cmd.Parameters.AddWithValue("@StoreManager2_EmpCode", Manager2Code);
                        cmd.Parameters.AddWithValue("@StoreManager2_Name", Manager2Name);
                        cmd.Parameters.AddWithValue("@TimeStamp", TimeStamp);

                        // Execute the query
                        cmd.ExecuteNonQuery();
                        MakeInputsReadOnly();
                    }

                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message.Replace("'", "\\'"); // Escape single quotes in the error message
                string errorScript = "<script type='text/javascript'>\n" +
                                     $"new PNotify({{\n" +
                                     "    title: 'Error',\n" +
                                     $"    text: '{errorMessage}',\n" +
                                     "    type: 'error',\n" +
                                     "    styling: 'bootstrap3'\n" +
                                     "});\n" +
                                     "</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowErrorNotification", errorScript, false);

            }
        }
        protected void TB_Manager1_Wrk_TextChanged(object sender, EventArgs e)
        {
            // Get Employee Name for Manager 1
            string employeeCode = TB_Manager1_Wrk.Text;
            string employeeName = GetEmployeeNameFromDatabase(employeeCode);
            Lbl_Manager1_Name.Text = string.IsNullOrEmpty(employeeName) ? "Employee not found." : "Employee Name: " + employeeName;
        }
        protected void TB_Manager2_Wrk_TextChanged(object sender, EventArgs e)
        {
            // Get Employee Name for Manager 2
            string employeeCode = TB_Manager2_Wrk.Text;
            string employeeName = GetEmployeeNameFromDatabase(employeeCode);
            Lbl_Manager2_Name.Text = string.IsNullOrEmpty(employeeName) ? "Employee not found." : "Employee Name: " + employeeName;
        }
        private string GetEmployeeNameFromDatabase(string employeeCode)
        {
            string employeeName = string.Empty;

            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT FullName FROM [tbl_Employee_Mustertable] WHERE WorkmanSL = @WorkmanSL";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@WorkmanSL", employeeCode);
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        employeeName = result.ToString();
                    }
                }
            }
            return employeeName;
        }
    }
}