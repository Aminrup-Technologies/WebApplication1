using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.IO;

namespace WebApplication1.bussiness.production
{
    public partial class helpdesk_ticketdetails : System.Web.UI.Page
    {
        string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string ticketId = Request.QueryString["ticket_id"]; // Updated to match query string key

                if (!string.IsNullOrEmpty(ticketId))
                {
                    // Get and bind ticket data
                    BindTicketDetails(ticketId);

                    // Check if the ticket is already assigned or closed
                    string ticketStatus = GetTicketStatus(ticketId);

                    if (ticketStatus == "Assigned")
                    {
                        // Disable the "Assign Ticket" button and show a message
                        btnAssignTicket.Enabled = false;
                        lblTicketStatusMessage.Text = "✅ This ticket has already been assigned.";
                        lblTicketStatusMessage.Style.Add("display", "block");
                        lblTicketStatusMessage.Style.Add("background-color", "#fff3cd");
                        lblTicketStatusMessage.Style.Add("color", "#856404");
                        lblTicketStatusMessage.Style.Add("padding", "15px");
                        lblTicketStatusMessage.Style.Add("border", "1px solid #ffeeba");
                        lblTicketStatusMessage.Style.Add("border-radius", "8px");
                        lblTicketStatusMessage.Style.Add("font-size", "15px");
                        lblTicketStatusMessage.Style.Add("margin-bottom", "20px");
                        lblTicketStatusMessage.Style.Add("font-weight", "500");

                        btnAssignTicket.Style.Add("background-color", "#ccc");
                        btnAssignTicket.Style.Add("cursor", "not-allowed");
                    }
                    else if (ticketStatus == "Closed")
                    {
                        // Disable the "Assign Ticket" and "Close Ticket" buttons and show closed status message
                        btnAssignTicket.Enabled = false;
                        btnCloseTicket.Enabled = false;

                        lblTicketStatusMessage.Text = "🚫 This ticket is closed and cannot be assigned or closed.";
                        lblTicketStatusMessage.Style.Add("display", "block");
                        lblTicketStatusMessage.Style.Add("background-color", "#f8d7da");
                        lblTicketStatusMessage.Style.Add("color", "#721c24");
                        lblTicketStatusMessage.Style.Add("padding", "15px");
                        lblTicketStatusMessage.Style.Add("border", "1px solid #f5c6cb");
                        lblTicketStatusMessage.Style.Add("border-radius", "8px");
                        lblTicketStatusMessage.Style.Add("font-size", "15px");
                        lblTicketStatusMessage.Style.Add("margin-bottom", "20px");
                        lblTicketStatusMessage.Style.Add("font-weight", "500");

                        btnAssignTicket.Style.Add("background-color", "#ccc");
                        btnAssignTicket.Style.Add("cursor", "not-allowed");

                        btnCloseTicket.Style.Add("background-color", "#ccc");
                        btnCloseTicket.Style.Add("cursor", "not-allowed");
                    }
                    else
                    {
                        lblTicketStatusMessage.Text = "";
                    }
                }
                else
                {
                    lblDescription.Text = "<span class='text-danger'>No Ticket ID provided.</span>";
                }
            }
        }


        // Method to bind ticket details
        private void BindTicketDetails(string ticketId)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = @"
                SELECT 
                    ticket_id, CreatedOn, CreatedByName, CreatorRegion, CreatorComp, status, 
                    priority_level, root1_value, root2_value, root3_value, Date_Open, Date_Closed, 
                    AssignedTo, AssignedOn, description, ImagePath 
                FROM tbl_helpdesktickets 
                WHERE ticket_id = @ticket_id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ticket_id", ticketId);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblTicketId.Text = reader["ticket_id"].ToString();
                            lblCreatedOn.Text = Convert.ToDateTime(reader["CreatedOn"]).ToString("dd MMM yyyy");
                            lblCreatedBy.Text = reader["CreatedByName"].ToString();
                            lblRegion.Text = reader["CreatorRegion"].ToString();
                            lblCompany.Text = reader["CreatorComp"].ToString();
                            lblStatus.Text = reader["status"].ToString();
                            lblPriority.Text = reader["priority_level"].ToString();
                            lblRoot1.Text = reader["root1_value"].ToString();
                            lblRoot2.Text = reader["root2_value"].ToString();
                            lblRoot3.Text = reader["root3_value"].ToString();
                            lblDateOpen.Text = reader["Date_Open"] != DBNull.Value ? Convert.ToDateTime(reader["Date_Open"]).ToString("dd MMM yyyy") : "-";
                            lblDateClosed.Text = reader["Date_Closed"] != DBNull.Value ? Convert.ToDateTime(reader["Date_Closed"]).ToString("dd MMM yyyy") : "-";
                            lblAssignedTo.Text = reader["AssignedTo"].ToString();
                            lblAssignedOn.Text = reader["AssignedOn"] != DBNull.Value ? Convert.ToDateTime(reader["AssignedOn"]).ToString("dd MMM yyyy") : "-";
                            lblDescription.Text = reader["description"].ToString().Replace(Environment.NewLine, "<br />");

                            // Handle Image Preview
                            string imagePath = reader["ImagePath"]?.ToString();
                            if (!string.IsNullOrEmpty(imagePath))
                            {
                                imgPreview1.ImageUrl = "~/images/" + imagePath; // Path to the uploaded image
                                imgPreview1.Visible = true;
                            }
                            else
                            {
                                imgPreview1.Visible = false;
                            }
                        }
                        else
                        {
                            lblDescription.Text = "<span class='text-danger'>Ticket not found.</span>";
                        }
                    }
                }
            }
        }

        // 🔥 New Code: Employee Code fetch Full Name Logic
        protected void txtEmployeeCode_TextChanged(object sender, EventArgs e)
        {
            string employeeCode = txtEmployeeCode.Text.Trim();

            if (!string.IsNullOrEmpty(employeeCode))
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string query = @"
                    SELECT FullName 
                    FROM [tbl_Employee_Mustertable] 
                    WHERE WorkmanSL = @WorkmanSL";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@WorkmanSL", employeeCode);
                        con.Open();

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            txtEmployeeName.Text = result.ToString();
                            txtEmployeeName.ReadOnly = true; // Make it non-editable
                        }
                        else
                        {
                            txtEmployeeName.Text = "";
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No employee found with the given code.');", true);
                        }
                    }
                }
            }
            else
            {
                txtEmployeeName.Text = "";
            }
        }

        // 🔥 New Code: Assign Ticket Logic
        protected void btnAssignTicket_Click(object sender, EventArgs e)
        {
            string ticketId = Request.QueryString["ticket_id"];
            string employeeCode = txtEmployeeCode.Text.Trim();
            string employeeName = txtEmployeeName.Text.Trim();

            if (string.IsNullOrEmpty(employeeCode) || string.IsNullOrEmpty(employeeName))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please provide valid employee details.');", true);
                return;
            }

            // Check if the ticket is already assigned
            string currentStatus = GetTicketStatus(ticketId); // Get current ticket status
            if (currentStatus == "Assigned")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('This ticket is already assigned.');", true);
                return;
            }

            // Update ticket status and assigned person in database
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = @"
            UPDATE [tbl_helpdesktickets] 
            SET [AssignedTo] = @AssignedTo, 
                [AssignedOn] = GETDATE(), 
                [NewAssignedPerson] = @NewAssignedPerson, 
                [status] = 'Assigned' 
            WHERE [ticket_id] = @TicketId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@AssignedTo", employeeCode);
                    cmd.Parameters.AddWithValue("@NewAssignedPerson", employeeCode); // ← Save only employee code
                    cmd.Parameters.AddWithValue("@TicketId", ticketId);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Successfully assigned
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Ticket successfully assigned!');", true);
                        // Disable the button and show the message
                        btnAssignTicket.Enabled = false;
                        lblTicketStatusMessage.Text = "This ticket has already been assigned.";
                        BindTicketDetails(ticketId); // Re-bind the grid to show updated data
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Failed to assign the ticket.');", true);
                    }
                }
            }
        }


        protected void btnCloseTicket_Click(object sender, EventArgs e)
        {
            string ticketId = Request.QueryString["ticket_id"];
            string remarks = txtClosingRemarks.Text.Trim();
            string userName = (Session["UserName"] != null) ? Session["UserName"].ToString() : "Unknown";
            string photoFileName = null;

            // Validate that remarks are filled
            if (string.IsNullOrWhiteSpace(remarks))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please enter closing remarks before submitting.');", true);
                return;
            }

            // Check if ticket is already closed
            string currentStatus = GetTicketStatus(ticketId);
            if (currentStatus == "Closed")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('This ticket is already closed.');", true);
                return;
            }

            // Handle image upload (optional)
            if (fuClosingPhoto.HasFile)
            {
                try
                {
                    string ext = Path.GetExtension(fuClosingPhoto.FileName);
                    if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Only JPG, JPEG, or PNG files are allowed.');", true);
                        return;
                    }

                    string uploadsFolder = Server.MapPath("~/Images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileName = "ticket_" + ticketId + "_closed_" + DateTime.Now.Ticks.ToString() + ext;
                    string fullPath = Path.Combine(uploadsFolder, fileName);
                    fuClosingPhoto.SaveAs(fullPath);

                    photoFileName = "HelpdeskUploads/" + fileName; // Store relative path
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Image upload failed: " + ex.Message + "');", true);
                    return;
                }
            }

            // Update ticket in DB
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = @"
            UPDATE [StoreModule].[dbo].[tbl_helpdesktickets] 
            SET [status] = 'Closed', 
                [Date_Closed] = GETDATE(), 
                [AssignedTo] = @AssignedTo, 
                [description] = @Remarks, 
                [ImagePath] = ISNULL(@ImagePath, [ImagePath]) 
            WHERE [ticket_id] = @TicketId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TicketId", ticketId);
                    cmd.Parameters.AddWithValue("@AssignedTo", userName);
                    cmd.Parameters.AddWithValue("@Remarks", remarks);
                    cmd.Parameters.AddWithValue("@ImagePath", (object)photoFileName ?? DBNull.Value); // Save image path to DB

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Ticket successfully closed.');", true);
                        btnCloseTicket.Enabled = false;
                        lblTicketStatusMessage.Text = "This ticket is now closed.";
                        BindTicketDetails(ticketId); 
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Failed to close the ticket.');", true);
                    }
                }
            }
        }


        // Method to get the ticket status
        private string GetTicketStatus(string ticketId)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "SELECT [status] FROM [tbl_helpdesktickets] WHERE [ticket_id] = @TicketId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TicketId", ticketId);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    return result?.ToString() ?? string.Empty;
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Redirect to GrievanceReq_View.aspx
            Response.Redirect("GrievanceReq_View.aspx");
        }

        protected void btnWithdrawTicket_Click(object sender, EventArgs e)
        {
            string ticketId = Request.QueryString["ticket_id"];

            if (string.IsNullOrEmpty(ticketId))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Ticket ID is missing.');", true);
                return;
            }

            try
            {
                if (!TicketExists(ticketId))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No ticket found with the given ID.');", true);
                    return;
                }

                WithdrawTicketFromDatabase(ticketId);

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('The ticket has been successfully withdrawn.');", true);
                Response.Redirect("GrievanceReq_View.aspx");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('An error occurred while withdrawing the ticket.');", true);
            }
        }

        private bool TicketExists(string ticketId)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string query = "SELECT COUNT(1) FROM [tbl_helpdesktickets] WHERE ticket_id = @ticket_id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ticket_id", ticketId);
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        private void WithdrawTicketFromDatabase(string ticketId)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string query = "UPDATE [tbl_helpdesktickets] SET status = 'Withdrawn' WHERE ticket_id = @ticket_id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ticket_id", ticketId);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected <= 0)
                    {
                        throw new Exception("No ticket found with the given ID or the ticket could not be withdrawn.");
                    }
                }
            }
        }
    }
}
