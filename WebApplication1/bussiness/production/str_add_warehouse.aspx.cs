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
    public partial class str_add_warehouse : System.Web.UI.Page
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
            }
        }
        private void MakeInputsReadOnly()
        {
            DDL_WorkCountry.Enabled = false;

            TB_WarehouseName.ReadOnly = true;
            TB_WarehouseCode.ReadOnly = true;
            TB_Manager1_Wrk.ReadOnly = true;
            TB_Manager2_Wrk.ReadOnly = true;
            btn_submit.Enabled = false;
            btn_submit.Text = "SAVED";
            btn_submit.CssClass = "btn btn-sm btn-success";

            string Data_SuccessScript = @"<script type='text/javascript'>
                            new PNotify({
                                title: 'Data Success',
                                text: 'Recorded Successfully!!',
                                type: 'success',
                                styling: 'bootstrap3'
                            });
                        </script>";

            // RegisterStartupScript adds the JavaScript code to the page
            ClientScript.RegisterStartupScript(this.GetType(), "ShowDataSuccessNotification", Data_SuccessScript, false);
        }
        private string GenerateUniqueWHId_PK()
        {

            string newWHIdValue;
            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Fetch the maximum WHId value
                    string query = "SELECT ISNULL(MAX(CAST(SUBSTRING(Id, 4, LEN(Id) - 2) AS INT)), 0) FROM str_add_warehouse";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();
                        int maxWHIdValue = Convert.ToInt32(result);

                        // Increment the numeric part
                        int numericPart = maxWHIdValue + 1;

                        // Format the new value
                        newWHIdValue = $"WH{numericPart:D3}"; // Ensure three digits (e.g., WH001, WH002)
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error generating WHId_PK: " + ex.Message);
                throw;
            }

            PKId = newWHIdValue;
            return newWHIdValue;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)

        {
            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;

            string CountryName = DDL_WorkCountry.SelectedItem.Text;
            string CountryCode = DDL_WorkCountry.SelectedValue;
            string WarehouseName = TB_WarehouseName.Text;
            string WarehouseCode = TB_WarehouseCode.Text;
            string Manager1Code = TB_Manager1_Wrk.Text;
            string Manager1Name = Lbl_Manager1_Name.Text.StartsWith("Employee Name: ") ? Lbl_Manager1_Name.Text.Replace("Employee Name: ", "").Trim() : null; ;
            string Manager2Code = TB_Manager2_Wrk.Text;
            string Manager2Name = Lbl_Manager2_Name.Text.StartsWith("Employee Name: ")? Lbl_Manager2_Name.Text.Replace("Employee Name: ", "").Trim(): null;
            DateTime TimeStamp = DateTime.Now;  // Current Date
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // SQL Insert Query
                    string query = @"INSERT INTO str_add_warehouse (ID ,Country_Name ,Country_Code ,Wh_Name ,Wh_Code ,StoreManager1_EmpCode ,StoreManager1_Name ,StoreManager2_EmpCode ,StoreManager2_Name,TimeStamp) VALUES (@ID, @Country_Name, @Country_Code, @Wh_Name, @Wh_Code, @StoreManager1_EmpCode, @StoreManager1_Name, @StoreManager2_EmpCode, @StoreManager2_Name, @TimeStamp)";


                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@ID", GenerateUniqueWHId_PK());
                        cmd.Parameters.AddWithValue("@Country_Name", CountryName);
                        cmd.Parameters.AddWithValue("@Country_Code", CountryCode);
                        cmd.Parameters.AddWithValue("@Wh_Name", WarehouseName);
                        cmd.Parameters.AddWithValue("@Wh_Code", WarehouseCode);
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