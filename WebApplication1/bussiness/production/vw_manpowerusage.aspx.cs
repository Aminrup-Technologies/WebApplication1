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
    public partial class vw_manpowerusage : System.Web.UI.Page
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
                    txt_date.Focus();

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
                }
            }
        }

        private DataTable GetDataFromStoredProcedure(string targetDate, string region)
        {
            DataTable dt = new DataTable();

            dbcl.Sqlconnection();

            using (SqlConnection connection = dbcl.Conn)
            {
                using (SqlCommand command = new SqlCommand("GetAttendanceDuplicates", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TargetDate", targetDate);
                    command.Parameters.AddWithValue("@Region", region);

                    try
                    {
                        connection.Open();

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }
                    }
                    catch (Exception ex)
                    {
                        string title = "Notifications :";
                        string body = ex.Message;
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }
            }

            return dt;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string txtinput = txt_date.Text.ToString();
            // Call the method to fetch data from the stored procedure
            DataTable dt = GetDataFromStoredProcedure(txtinput, region);

            // Bind the data to the GridView
            GridView1.DataSource = dt;
            GridView1.DataBind();

        }

        protected void YourFunctionToBindGridView(string employeeWrk)
        {
            // Assuming you have a method to fetch data based on EmployeeWrk
            // Replace 'YourDataFetchingMethod' with your actual data retrieval logic.

            string txtinput = txt_date.Text.ToString();
            DataTable data = YourDataFetchingMethod(txtinput, region,employeeWrk);

            // Bind the data to the second GridView
            GridView2.DataSource = data;
            GridView2.DataBind();
        }

        // Your method to fetch data based on EmployeeWrk
        private DataTable YourDataFetchingMethod(string targetDate, string region,string employeeWrk)
        {
            DataTable dt = new DataTable();

            dbcl.Sqlconnection();

            using (SqlConnection connection = dbcl.Conn)
            {
                using (SqlCommand command = new SqlCommand("GetDupAttendanceRecords", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TargetDate", targetDate);
                    command.Parameters.AddWithValue("@Region", region);
                    command.Parameters.AddWithValue("@EmployeeWrk", employeeWrk);

                    try
                    {
                        connection.Open();

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }
                    }
                    catch (Exception ex)
                    {
                        string title = "Notifications :";
                        string body = ex.Message;
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }
            }

            return dt;
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);

            //Reference the GridView Row.
            GridViewRow row = GridView1.Rows[rowIndex];

            //Fetch value of Name.
            string empwrk = (row.FindControl("lbl_EmployeeWrk") as Label).Text;
            if (e.CommandName == "View_Details")
            {
                YourFunctionToBindGridView(empwrk);
            }
        }
    }
}