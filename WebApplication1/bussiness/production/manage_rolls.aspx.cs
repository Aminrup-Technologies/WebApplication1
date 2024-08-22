using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class manage_rolls : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    TB_Employee_Type.Focus();
                    BindGridView();
                }
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string companyDescription = TB_Employee_Type.Text.Trim();
            string companyID = TB_EmpType_Value.Text.Trim();

            // Retrieve connection string from web.config
            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;

            // Check if record already exists
            bool recordExists = CheckIfRecordExists(connectionString, companyID);

            if (recordExists)
            {
                lbl_msg.Text = "Record with the same ID already exists.";
                return; // Exit if record already exists
            }

            // Insert data using transaction
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string insertQuery = "INSERT INTO tlb_emp_roles (Employee_Type, EmpType_Value, ViewMode, DeleteMode, AddedByWrk, TimeStamp) VALUES (@Employee_Type, @EmpType_Value, @ViewMode, @DeleteMode, @AddedByWrk, @TimeStamp)";
                    SqlCommand cmd = new SqlCommand(insertQuery, conn, transaction);

                    // Add parameters
                    cmd.Parameters.AddWithValue("@Employee_Type", companyDescription);
                    cmd.Parameters.AddWithValue("@EmpType_Value", companyID);
                    cmd.Parameters.AddWithValue("@ViewMode", 1);
                    cmd.Parameters.AddWithValue("@DeleteMode", 0);
                    cmd.Parameters.AddWithValue("@AddedByWrk", Session["WORKMAN"].ToString());
                    cmd.Parameters.AddWithValue("@TimeStamp", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));

                    // Execute insert command
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Commit transaction if successful
                    transaction.Commit();

                    if (rowsAffected > 0)
                    {
                        BindGridView();
                        lbl_msg.Text = "Data inserted successfully!";
                    }
                    else
                    {
                        lbl_msg.Text = "Failed to insert data.";
                    }
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    lbl_msg.Text = "Error: " + ex.Message;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private bool CheckIfRecordExists(string connectionString, string companyID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM tlb_emp_roles WHERE EmpType_Value = @EmpType_Value";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmpType_Value", companyID);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }


        private void BindGridView()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Employee_Type, EmpType_Value FROM tlb_emp_roles order by Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGridView();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            string companyDescription = (row.FindControl("TextBoxCompanyDescription") as TextBox).Text;
            string companyID = (row.FindControl("TextBoxCompanyID") as TextBox).Text;

            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE tlb_emp_roles SET Employee_Type = @Employee_Type, EmpType_Value = @EmpType_Value WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Employee_Type", companyDescription);
                cmd.Parameters.AddWithValue("@EmpType_Value", companyID);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            GridView1.EditIndex = -1;
            BindGridView();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE tlb_emp_roles SET ViewMode = 0, DeleteMode = 1, DeletedOn=@DeletedOn, DeletedByWrk=@DeletedByWrk WHERE Id = @Id";
                //string query = "DELETE FROM tlb_emp_roles WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DeletedOn", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@DeletedByWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                //cmd.ExecuteNonQuery();
            }

            BindGridView();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                // Handle page index changing here if needed
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindGridView();
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("magician.aspx");
        }
    }
}