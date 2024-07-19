using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Configuration;

namespace WebApplication1.bussiness.production
{
    public class DatabaseHelper
    {
        // Get the connection string from web.config
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
        }

        // Establish a connection to the database
        public static SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(GetConnectionString());
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                // Handle connection error, if any
                // You might want to log the exception or take other actions
                throw ex;
            }
            return connection;
        }

        public static void BindWithDefaultNoRecords(DropDownList ddl)
        {
            // Clear existing items and add the default "No Records Found" item
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("No Records Found", ""));
        }


        public static void BindDropDownList1(string query, DropDownList ddl, string textField, string valueField)
        {
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                ddl.DataSource = reader;
                ddl.DataTextField = textField;
                ddl.DataValueField = valueField;
                ddl.DataBind();

                reader.Close();
                connection.Close();
            }

            // Add a default item to the DropDownList
            ddl.Items.Insert(0, new ListItem("Select", ""));
        }

        public static void BindDropDownList(string query, DropDownList ddl, string textField, string valueField, out bool recordsBound)
        {
            // Initialize the flag
            recordsBound = false;

            using (SqlConnection connection = GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                // Check if there are any records to bind
                if (reader.HasRows)
                {
                    ddl.DataSource = reader;
                    ddl.DataTextField = textField;
                    ddl.DataValueField = valueField;
                    ddl.DataBind();

                    // Set the flag to indicate that records were bound
                    recordsBound = true;
                }

                reader.Close();
                connection.Close();
            }

            // Add a default item to the DropDownList
            ddl.Items.Insert(0, new ListItem("Select", "0"));
        }



        // Overload method with parameterized query
        public static void BindDropDownList1(string query, DropDownList ddl, string textField, string valueField, SqlParameter parameter)
        {
            // Create a connection and command
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(parameter);
                    SqlDataReader reader = command.ExecuteReader();
                    ddl.DataSource = reader;
                    ddl.DataTextField = textField;
                    ddl.DataValueField = valueField;
                    ddl.DataBind();
                    reader.Close();
                    connection.Close();
                }
            }
        }

        public static void BindDropDownList2(string query, DropDownList ddl, string textField, string valueField, SqlParameter parameter, out bool recordsBound)
        {
            // Initialize the flag
            recordsBound = false;

            // Create a connection and command
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add the parameter to the command
                    command.Parameters.Add(parameter);

                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        // Check if there are any records to bind
                        if (reader.HasRows)
                        {
                            ddl.DataSource = reader;
                            ddl.DataTextField = textField;
                            ddl.DataValueField = valueField;
                            ddl.DataBind();

                            // Set the flag to indicate that records were bound
                            recordsBound = true;
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception (e.g., log the error, display a message)
                        // For simplicity, you can just rethrow the exception
                        throw new Exception("An error occurred while executing the query: " + ex.Message, ex);
                    }
                }
            }
        }


        public static void BindDropDownList(string query, DropDownList ddl, string textField, string valueField, SqlParameter parameter, out bool recordsBound)
        {
            // Initialize the flag
            recordsBound = false;

            // Create a connection and command
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add the parameter to the command
                    command.Parameters.Add(parameter);

                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        // Check if there are any records to bind
                        if (reader.HasRows)
                        {
                            ddl.DataSource = reader;
                            ddl.DataTextField = textField;
                            ddl.DataValueField = valueField;
                            ddl.DataBind();

                            // Set the flag to indicate that records were bound
                            recordsBound = true;
                        }
                        else
                        {
                            // If no records found, bind a default record
                            ddl.Items.Clear();
                            ddl.Items.Add(new ListItem("No records found", ""));
                        }
                        connection.Close();
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        // Handle the exception (e.g., log the error, display a message)
                        // For simplicity, you can just rethrow the exception
                        throw new Exception("An error occurred while executing the query: " + ex.Message, ex);
                    }

                }
            }
            // Add a default item to the DropDownList
            ddl.Items.Insert(0, new ListItem("Select", "0"));
        }

        public static void BindDropDownList(string query, DropDownList ddl, string textField, string valueField, SqlParameter[] parameters, out bool recordsBound)
        {
            // Initialize the flag
            recordsBound = false;

            // Create a connection and command
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters to the command
                    command.Parameters.AddRange(parameters);

                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        // Check if there are any records to bind
                        if (reader.HasRows)
                        {
                            ddl.DataSource = reader;
                            ddl.DataTextField = textField;
                            ddl.DataValueField = valueField;
                            ddl.DataBind();

                            // Set the flag to indicate that records were bound
                            recordsBound = true;
                        }

                        else
                        {
                            // If no records found, bind a default record
                            ddl.Items.Clear();
                            ddl.Items.Add(new ListItem("No records found", ""));
                            ddl.DataBind(); // Ensure the default record is displayed
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception (e.g., log the error, display a message)
                        // For simplicity, you can just rethrow the exception
                        throw new Exception("An error occurred while executing the query: " + ex.Message, ex);
                    }
                }
            }
            // Add a default item to the DropDownList
            ddl.Items.Insert(0, new ListItem("Select", "0"));
        }


        public static DataTable GetBrandFieldsControlByBrandId(int brandId)
        {
            DataTable dataTable = new DataTable();

            // Create a SqlConnection
            using (SqlConnection connection = GetConnection())
            {
                // Open the connection

                // Create a SqlCommand for the stored procedure
                using (SqlCommand command = new SqlCommand("GetBrandFieldsControlByBrandId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    command.Parameters.AddWithValue("@brand_id", brandId);

                    // Execute the SqlCommand and load results into the DataTable
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
                connection.Close();
            }

            return dataTable;
        }
    }
}