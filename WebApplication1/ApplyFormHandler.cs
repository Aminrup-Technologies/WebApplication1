using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public class ApplyFormHandler
    {
        public void ExecuteApplyFormSp(string action, int? formId, string name, string email, string mobile, string address, string postCode, string imageUrl, int? jobId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ApplyformSp", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Action", action);
                    command.Parameters.AddWithValue("@FormId", (object)formId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Name", (object)name ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", (object)email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Mobile", (object)mobile ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Address", (object)address ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PostCode", (object)postCode ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ImagrUrl", (object)imageUrl ?? DBNull.Value);
                    command.Parameters.AddWithValue("@JobID", (object)jobId ?? DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}