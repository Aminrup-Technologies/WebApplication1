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
    public partial class jobapp_controller : System.Web.UI.Page
    {
        string connString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("~/login.aspx", false);
                }
                else
                {
                    DateTime now = DateTime.Now;

                    lbl_year.Text = now.Year.ToString();
                    lbl_monthcode.Text = now.Month.ToString();
                    lbl_month.Text = now.ToString("MMMM");

                    ddlViewLevel.SelectedValue = "3";

                    int viewLevel = int.Parse(ddlViewLevel.SelectedValue);
                    int year = now.Year;
                    int month = now.Month;

                    BindGridNew(3, year, month, null, null, null);

                    //BindGrid(1, null, null, null); // Default view (Region Level)
                }
            }
        }

        private void BindGrid(int viewLevel, string region, string inchargeWrk, string inchargeName)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetPendingApprovals", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ViewLevel", viewLevel);
                    cmd.Parameters.AddWithValue("@Region", (object)region ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@InchargeWrk", (object)inchargeWrk ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@InchargeName", (object)inchargeName ?? DBNull.Value);

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvPendingApprovals.DataSource = dt;
                        gvPendingApprovals.DataBind();
                    }
                }
            }
        }

        protected void gvPendingApprovals_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPendingApprovals.PageIndex = e.NewPageIndex;
            //BindGrid(1, null, null, null); // Re-bind grid with default parameters

            Int32 year = Convert.ToInt32(lbl_year.Text.ToString());
            Int32 month = Convert.ToInt32(lbl_monthcode.Text.ToString());
            int viewLevel = int.Parse(ddlViewLevel.SelectedValue);
            //BindGridNew(viewLevel, year, month, null, null, null);
            if (viewLevel == 3)
            {
                BindGridNew(viewLevel, year, month, null, null, null);
            }
            else
            {
                BindGridNew_OLD(viewLevel, year, month, null, null, null);
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            
        }

        protected void ddlViewLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int viewLevel = int.Parse(ddlViewLevel.SelectedValue);
            //BindGrid(viewLevel, null, null, null);

            Int32 year = Convert.ToInt32(lbl_year.Text.ToString());
            Int32 month = Convert.ToInt32(lbl_monthcode.Text.ToString());
            int viewLevel = int.Parse(ddlViewLevel.SelectedValue);

            if (viewLevel == 3)
            {
                BindGridNew(viewLevel, year, month, null, null, null);
            }
            else
            {
                BindGridNew_OLD(viewLevel, year, month, null, null, null);
            }
            
        }


        protected void btn_prevmonth_Click(object sender, EventArgs e)
        {
            Int32 year = Convert.ToInt32(lbl_year.Text.ToString());
            Int32 month = Convert.ToInt32(lbl_monthcode.Text.ToString());

            if (month == 01 || month == 1)
            {
                month = 12;
                year = year - 1;
            }
            else
            {
                month = month - 1;
            }

            string Year = Convert.ToString(year);
            string Month = "";
            if (month <= 9)
            {
                Month = "0" + month.ToString();
            }
            else
            {
                Month = month.ToString();
            }

            string Monthname = "";
            dbcl.FindMonthName(Month, ref Monthname);
            lbl_month.Text = Monthname;
            lbl_year.Text = Year;
            lbl_monthcode.Text = Month;

            int viewLevel = int.Parse(ddlViewLevel.SelectedValue);
            //BindGridNew(viewLevel, year, month, null, null, null);
            if (viewLevel == 3)
            {
                BindGridNew(viewLevel, year, month, null, null, null);
            }
            else
            {
                BindGridNew_OLD(viewLevel, year, month, null, null, null);
            }
        }

        protected void btn_currentdata_Click(object sender, EventArgs e)
        {

            int year = DateTime.Now.Year;  // Keep as integer
            int month = DateTime.Now.Month;  // Keep as integer
            int viewLevel = int.Parse(ddlViewLevel.SelectedValue);

            string Year = Convert.ToString(year);
            string Month = Convert.ToString(month);

            string Monthname = "";
            dbcl.FindMonthName(Month, ref Monthname);
            lbl_month.Text = Monthname;
            lbl_year.Text = Year;
            lbl_monthcode.Text = Month;

            //BindGridNew(viewLevel, year, month, null, null, null);

            if (viewLevel == 3)
            {
                BindGridNew(viewLevel, year, month, null, null, null);
            }
            else
            {
                BindGridNew_OLD(viewLevel, year, month, null, null, null);
            }
        }
        protected void btn_nextmonth_Click(object sender, EventArgs e)
        {
            Int32 year = Convert.ToInt32(lbl_year.Text.ToString());
            Int32 month = Convert.ToInt32(lbl_monthcode.Text.ToString());

            if (month == 12)
            {
                month = 1;
                year = year + 1;
            }
            else
            {
                month = month + 1;
            }

            string Year = Convert.ToString(year);
            string Month = "";
            if (month <= 9)
            {
                Month = "0" + month.ToString();
            }
            else
            {
                Month = month.ToString();
            }

            string Monthname = "";
            dbcl.FindMonthName(Month, ref Monthname);
            lbl_month.Text = Monthname;
            lbl_year.Text = Year;
            lbl_monthcode.Text = Month;

            int viewLevel = int.Parse(ddlViewLevel.SelectedValue);
            //BindGridNew(viewLevel, year, month, null, null, null);

            if (viewLevel == 3)
            {
                BindGridNew(viewLevel, year, month, null, null, null);
            }
            else
            {
                BindGridNew_OLD(viewLevel, year, month, null, null, null);
            }
        }

        private void BindGridNew_OLD(int viewLevel, int year, int month, string region, string inchargeWrk, string inchargeName)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetPendingApprovalsDynamic", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ViewLevel", viewLevel);
                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.Parameters.AddWithValue("@Month", month);
                    cmd.Parameters.AddWithValue("@Region", (object)region ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@InchargeWrk", (object)inchargeWrk ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@InchargeName", (object)inchargeName ?? DBNull.Value);

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        btnBulkUnblock.Enabled = false;
                        gvPendingApprovalsold.Visible = true;
                        gvPendingApprovals.Visible = false;

                        gvPendingApprovalsold.DataSource = dt;
                        gvPendingApprovalsold.DataBind();
                    }
                }
            }
        }

        private void BindGridNew(int viewLevel, int year, int month, string region, string inchargeWrk, string inchargeName)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetPendingApprovalsDynamic", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ViewLevel", viewLevel);
                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.Parameters.AddWithValue("@Month", month);
                    cmd.Parameters.AddWithValue("@Region", (object)region ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@InchargeWrk", (object)inchargeWrk ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@InchargeName", (object)inchargeName ?? DBNull.Value);

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        btnBulkUnblock.Enabled = true;
                        gvPendingApprovalsold.Visible = false;
                        gvPendingApprovals.Visible = true;

                        gvPendingApprovals.DataSource = dt;
                        gvPendingApprovals.DataBind();
                    }
                }
            }
        }

        protected void gvPendingApprovals_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Unblock")
            {
                string Id = e.CommandArgument.ToString();

                try
                {
                    UnblockJob(Id);
                    ShowNotification($"Job with ID {Id} has been successfully unblocked.", "success");
                }
                catch (Exception ex)
                {
                    ShowNotification($"Error unblocking job with ID {Id}: {ex.Message}", "error");
                    return;  // Exit early if unblock fails.
                }

                // Refresh the grid after unblocking the job
                int year = Convert.ToInt32(lbl_year.Text);  // No need for ToString() here
                int month = Convert.ToInt32(lbl_monthcode.Text);  // No need for ToString() here
                int viewLevel = int.Parse(ddlViewLevel.SelectedValue);

                if (viewLevel == 3)
                {
                    BindGridNew(viewLevel, year, month, null, null, null);
                }
                else
                {
                    BindGridNew_OLD(viewLevel, year, month, null, null, null);
                }
            }
        }


        //protected void gvPendingApprovals_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "Unblock")
        //    {
        //        string Id = e.CommandArgument.ToString();
        //        UnblockJob(Id);

        //        Int32 year = Convert.ToInt32(lbl_year.Text.ToString());
        //        Int32 month = Convert.ToInt32(lbl_monthcode.Text.ToString());
        //        int viewLevel = int.Parse(ddlViewLevel.SelectedValue);

        //        if (viewLevel == 3)
        //        {
        //            BindGridNew(viewLevel, year, month, null, null, null);
        //        }
        //        else
        //        {
        //            BindGridNew_OLD(viewLevel, year, month, null, null, null);
        //        }
        //    }
        //}

        protected void btnBulkUnblock_Click(object sender, EventArgs e)
        {
            bool isAnyJobUnblocked = false;

            foreach (GridViewRow row in gvPendingApprovals.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");

                if (chkSelect != null && chkSelect.Checked)
                {
                    string jobId = gvPendingApprovals.DataKeys[row.RowIndex]["Id"].ToString();

                    try
                    {
                        UnblockJob(jobId);
                        isAnyJobUnblocked = true;
                    }
                    catch (Exception ex)
                    {
                        ShowNotification($"Error unblocking job ID {jobId}: {ex.Message}", "error");
                    }
                }
            }

            if (isAnyJobUnblocked)
            {
                ShowNotification("Selected jobs have been successfully unblocked.", "success");
            }
            else
            {
                ShowNotification("No jobs were selected for unblocking.", "info");
            }

            // Refresh the grid
            int year = Convert.ToInt32(lbl_year.Text);
            int month = Convert.ToInt32(lbl_monthcode.Text);
            int viewLevel = int.Parse(ddlViewLevel.SelectedValue);

            if (viewLevel == 3)
            {
                BindGridNew(viewLevel, year, month, null, null, null);
            }
            else
            {
                BindGridNew_OLD(viewLevel, year, month, null, null, null);
            }
        }


        //protected void btnBulkUnblock_Click(object sender, EventArgs e)
        //{
        //    foreach (GridViewRow row in gvPendingApprovals.Rows)
        //    {
        //        CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");

        //        if (chkSelect != null && chkSelect.Checked)
        //        {
        //            string jobId = gvPendingApprovals.DataKeys[row.RowIndex]["Id"].ToString();
        //            UnblockJob(jobId);
        //        }
        //    }

        //    Int32 year = Convert.ToInt32(lbl_year.Text.ToString());
        //    Int32 month = Convert.ToInt32(lbl_monthcode.Text.ToString());
        //    int viewLevel = int.Parse(ddlViewLevel.SelectedValue);

        //    if (viewLevel == 3)
        //    {
        //        BindGridNew(viewLevel, year, month, null, null, null);
        //    }
        //    else
        //    {
        //        BindGridNew_OLD(viewLevel, year, month, null, null, null);
        //    }
        //}

        public void ShowNotification(string message, string type)
        {
            string script = $@"<script type='text/javascript'>
                new PNotify({{
                    title: 'Notification',
                    text: '{message}',
                    type: '{type}',
                    styling: 'bootstrap3'
                }});
            </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "ShowNotification", script, false);
        }

        private void UnblockJob(string Id)
        {
            // Ensure a valid Id is passed
            if (string.IsNullOrEmpty(Id))
            {
                // Log or throw an error here if necessary
                return;
            }

            try
            {
                // Open the SQL connection
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    // Start a transaction to ensure atomic operation
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Define the SQL command with parameters to unblock the job
                            using (SqlCommand cmd = new SqlCommand("UPDATE tbl_jobs SET IsBlocked = 0, BlockedTimestamp = NULL WHERE Id = @ID", conn, transaction))
                            {
                                // Add the ID parameter to the command
                                cmd.Parameters.AddWithValue("@ID", Id);

                                // Execute the SQL command
                                int rowsAffected = cmd.ExecuteNonQuery();

                                // Check if any rows were affected
                                if (rowsAffected == 0)
                                {
                                    ShowNotification($"No job found with ID {Id} or the job is not blocked.", "info");
                                }
                                else
                                {
                                    ShowNotification($"Job with ID {Id} has been successfully unblocked.", "success");
                                }

                                // Commit the transaction if everything was successful
                                transaction.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction in case of an error
                            transaction.Rollback();
                            throw new Exception("An error occurred while unblocking the job.", ex);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("A database error occurred while unblocking the job.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while unblocking the job.", ex);
            }
        }

    }
}