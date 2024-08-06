using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace WebApplication1.bussiness.production
{
    public partial class pyrl_approvedeductions : System.Web.UI.Page
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
                    }
                    else
                    {
                        region = Session["REGION"].ToString();
                        comp = Session["COMPANY_CODE"].ToString();
                        state = Session["STATE"].ToString();
                        datalock = "0";
                    }

                    PageLoader();
                }
            }
        }

        private void PageLoader()
        {
            // Set the connection string to your database
            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;

            // Create a new SqlConnection using the connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create a new SqlCommand with the stored procedure name and connection
                using (SqlCommand command = new SqlCommand("GetPayrollData", connection))
                {
                    // Set the command type as stored procedure
                    command.CommandType = CommandType.StoredProcedure;

                    // Add the required parameters and their values
                    command.Parameters.AddWithValue("@Region", region);
                    command.Parameters.AddWithValue("@Company", comp);
                    //command.Parameters.AddWithValue("@Year", );
                    //command.Parameters.AddWithValue("@SalaryMonth", 7);

                    // Open the connection
                    connection.Open();

                    // Create a new SqlDataAdapter to execute the command and fill the result in a DataTable
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to the GridView
                        GridView1.DataSource = dataTable;
                        GridView1.DataBind();
                    }
                }
            }
        }
    }
}