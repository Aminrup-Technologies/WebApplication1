using System;
using System.Configuration;
using System.Data.SqlClient;

namespace atsweb
{
    public partial class contact_us : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void SubmitBtn_Click(object sender, EventArgs e)
        {

            string userName = name.Text;
            string userEmail = email.Text;
            string userPhone = phone.Text;
            string userAddress = address.Text;
            string userNote = note.Text;

            // Retrieve the connection string from Web.config
            string connectionString = ConfigurationManager.ConnectionStrings["atsDB"]?.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                success.Visible = false;
                error.Visible = true;
                return;
            }

            string query = "INSERT INTO ContactMessages (Name, Email, Phone, Address, Note) VALUES (@Name, @Email, @Phone, @Address, @Note)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", userName);
                        command.Parameters.AddWithValue("@Email", userEmail);
                        command.Parameters.AddWithValue("@Phone", userPhone);
                        command.Parameters.AddWithValue("@Address", userAddress);
                        command.Parameters.AddWithValue("@Note", userNote);

                        connection.Open();
                        command.ExecuteNonQuery();
                        success.Visible = true;
                        error.Visible = false;
                    }
                }
            }
            catch (Exception )
            {
                success.Visible = false;
                error.Visible = true;
            }
        }
    }
}
