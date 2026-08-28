using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class manage_rollsaccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    PlantBinder();
                    BindGridView();
                }
            }
        }

        public void PlantBinder()
        {
            string query = "SELECT EmpType_Value, Employee_Type FROM tlb_emp_roles";
            string textField = "Employee_Type";
            string valueField = "EmpType_Value";

            bool recordsBound;

            // Bind the DropDownList and get the flag indicating whether records were bound
            DatabaseHelper.BindDropDownList(query, DDL_Employee_Type, textField, valueField, out recordsBound);

            // Check if any records were bound
            if (!recordsBound)
            {
                DatabaseHelper.BindWithDefaultNoRecords(DDL_Employee_Type);
                string PlantBinder_Error_script = @"<script type='text/javascript'>
                            new PNotify({
                                title: 'Error',
                                text: 'An error occurred!',
                                type: 'error',
                                styling: 'bootstrap3'
                            });
                        </script>";

                // RegisterStartupScript adds the JavaScript code to the page
                ClientScript.RegisterStartupScript(this.GetType(), "ShowPlantBinderErrorNotification", PlantBinder_Error_script, false);

            }
        }

        protected void DDL_Plant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_Employee_Type.SelectedIndex != 0)
            {
                string selectedEmpType_Value = DDL_Employee_Type.SelectedValue.ToString();
                lbl_DDL_EmpType_Value.Text = selectedEmpType_Value;
                BindGridViewbyPlant(selectedEmpType_Value);

            }
            else
            {
                string DDL_Plant_Error_script = @"<script type='text/javascript'>
                            new PNotify({
                                title: 'Error',
                                text: 'Invalid Selection!',
                                type: 'error',
                                styling: 'bootstrap3'
                            });
                        </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowPlantInvalidErrorNotification", DDL_Plant_Error_script, false);
            }
        }

        private void BindGridViewbyPlant(string EmpType_Value)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            string query = "SELECT * FROM [dbo].[tlb_emp_roles_permission] where EmpType_Value='" + EmpType_Value + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();

                try
                {
                    connection.Open();
                    adapter.Fill(dataTable);
                    GridViewPlantLines.DataSource = dataTable;
                    GridViewPlantLines.DataBind();
                }
                catch (Exception)
                {
                    // Handle exceptions (log or display error message)
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            int plant_id = int.Parse(DDL_Employee_Type.SelectedValue); // Assuming DDL_Plant is your DropDownList for Plant ID
            int line_id = int.Parse(TB_Emp_PermissionValue.Text.Trim());
            string line_name = TB_Emp_PermissionText.Text.Trim();

            // Check if the line_name already exists for the given plant_id
            if (IsLineNameDuplicate(plant_id, line_name))
            {
                // Handle duplicate record scenario
                // You can show a message or handle it as per your application's requirement
                return;
            }

            // Insert into database using transaction
            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Insert command
                    string insertQuery = @"INSERT INTO tlb_emp_roles_permission (EmpType_Value, Emp_PermissionValue, Emp_PermissionText) 
                                   VALUES (@EmpType_Value, @Emp_PermissionValue, @Emp_PermissionText)";
                    SqlCommand cmdInsert = new SqlCommand(insertQuery, connection, transaction);
                    cmdInsert.Parameters.AddWithValue("@EmpType_Value", plant_id);
                    cmdInsert.Parameters.AddWithValue("@Emp_PermissionValue", line_id);
                    cmdInsert.Parameters.AddWithValue("@Emp_PermissionText", line_name);

                    cmdInsert.ExecuteNonQuery();

                    transaction.Commit(); // Commit transaction if successful
                    ClearForm(); // Clear form inputs after successful insertion
                    BindGridView(); // Update GridView after successful insertion
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private bool IsLineNameDuplicate(int plant_id, string line_name)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM tlb_emp_roles_permission WHERE EmpType_Value = @EmpType_Value AND Emp_PermissionText = @Emp_PermissionText";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@EmpType_Value", plant_id);
                cmd.Parameters.AddWithValue("@Emp_PermissionText", line_name);

                connection.Open();
                int count = (int)cmd.ExecuteScalar();
                connection.Close();

                return count > 0;
            }
        }

        // Method to clear form inputs after successful insertion
        private void ClearForm()
        {
            DDL_Employee_Type.SelectedIndex = 0;
            TB_Emp_PermissionText.Text = "";
            TB_Emp_PermissionValue.Text = "";
        }

        // Method to bind GridView after successful insertion
        private void BindGridView()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            string query = "SELECT * FROM [dbo].[tlb_emp_roles_permission]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();

                try
                {
                    connection.Open();
                    adapter.Fill(dataTable);
                    GridViewPlantLines.DataSource = dataTable;
                    GridViewPlantLines.DataBind();
                }
                catch (Exception)
                {
                    // Handle exceptions (log or display error message)
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected void GridViewPlantLines_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewPlantLines.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void GridViewPlantLines_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewPlantLines.EditIndex = -1;
            BindGridView();
        }

        protected void GridViewPlantLines_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridViewPlantLines.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridViewPlantLines.DataKeys[e.RowIndex].Values[0]);

            // Extract updated values
            string empPermissionValue = (row.FindControl("txtEmp_PermissionValue") as TextBox).Text;
            string empPermissionText = (row.FindControl("txtEmp_PermissionText") as TextBox).Text;

            int viewStatus = (row.FindControl("chkViewStatusEdit") as CheckBox)?.Checked == true ? 1 : 0;
            int deleteStatus = (row.FindControl("chkDeleteStatusEdit") as CheckBox)?.Checked == true ? 1 : 0;

            // Update database
            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            string query = "UPDATE tlb_emp_roles_permission SET Emp_PermissionValue = @Emp_PermissionValue, Emp_PermissionText = @Emp_PermissionText, view_status = @view_status, delete_status=@delete_status WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Emp_PermissionValue", empPermissionValue);
                cmd.Parameters.AddWithValue("@Emp_PermissionText", empPermissionText);
                cmd.Parameters.AddWithValue("@view_status", viewStatus);
                cmd.Parameters.AddWithValue("@delete_status", deleteStatus);
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    GridViewPlantLines.EditIndex = -1;
                    
                }
                catch (Exception ex)
                {
                    // Handle exceptions (log or display error message)
                    // Example: Log the error message or display it
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                BindGridView();
            }
        }

        protected void GridViewPlantLines_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridViewPlantLines.DataKeys[e.RowIndex].Values["id"]);

            // Soft delete record (set delete_status = 1)
            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            string query = "UPDATE tlb_emp_roles_permission SET delete_status = 1 WHERE id = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    BindGridView();
                }
                catch (Exception)
                {
                    // Handle exceptions (log or display error message)
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected void GridViewPlantLines_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                // Prepopulate fields during edit mode
                int id = Convert.ToInt32(GridViewPlantLines.DataKeys[e.Row.RowIndex].Values["id"]);
                string local_name = DataBinder.Eval(e.Row.DataItem, "Emp_PermissionValue").ToString();
                string line_sap_code = DataBinder.Eval(e.Row.DataItem, "Emp_PermissionText").ToString();

                TextBox TB_Edit_Local_Name = e.Row.FindControl("txtEmp_PermissionValue") as TextBox;
                TextBox TB_Edit_Line_Sap_Code = e.Row.FindControl("txtEmp_PermissionText") as TextBox;

                if (TB_Edit_Local_Name != null)
                    TB_Edit_Local_Name.Text = local_name;

                if (TB_Edit_Line_Sap_Code != null)
                    TB_Edit_Line_Sap_Code.Text = line_sap_code;
            }
        }

        protected void GridViewPlantLines_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewPlantLines.PageIndex = e.NewPageIndex;
            BindGridView();
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("magician.aspx");
        }
    }
}