using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Text;

namespace WebApplication1.bussiness.production
{
    public partial class manage_jobid_v2 : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null)
                {
                    Response.Redirect("~/login.aspx", false);
                    return;
                }

                Bind_BillingType();

                // SOLUTION 2: Check if returning from Details page with saved Session state
                if (Session["Grid_Year"] != null && Session["Grid_Month"] != null)
                {
                    lbl_year.Text = Session["Grid_Year"].ToString();
                    lbl_monthcode.Text = Session["Grid_Month"].ToString();
                }
                else
                {
                    // First time visit, use current date
                    DateTime now = DateTime.Now;
                    lbl_year.Text = now.Year.ToString();
                    lbl_monthcode.Text = now.Month.ToString().PadLeft(2, '0');
                }

                GridBinder(lbl_year.Text, lbl_monthcode.Text);
            }
        }

        // APPLIED BUG FIX: Properly escaping newlines and quotes to prevent JS crashes
        private void ShowNotification(string title, string message, string type)
        {
            if (string.IsNullOrEmpty(message)) message = "An unknown error occurred.";

            string cleanMessage = message.Replace("'", "\\'")      // Escape single quotes
                                         .Replace("\"", "\\\"")    // Escape double quotes
                                         .Replace("\r", "")        // Strip carriage returns
                                         .Replace("\n", "<br/>");  // Convert newlines to HTML breaks

            string script = $"showPNotify('{title}', '{cleanMessage}', '{type}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PNotify", script, true);
        }

        private void Bind_BillingType()
        {
            try
            {
                string CmdString = "SELECT BilingType, BillingCode FROM tlb_JOB_BillingType ORDER BY Id";
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn))
                {
                    DDL_BillingType.DataSource = Cmd.ExecuteReader();
                    DDL_BillingType.DataTextField = "BilingType";
                    DDL_BillingType.DataValueField = "BillingCode";
                    DDL_BillingType.DataBind();
                }
                DDL_BillingType.Items.Insert(0, new ListItem("-- All Job Types --", "0"));
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        // =================================================================================
        // DYNAMIC QUERY BUILDER (Eliminates 150 lines of redundant code)
        // =================================================================================
        private void GridBinder(string year, string month)
        {
            // SOLUTION 2: Save state to session every time the grid binds
            Session["Grid_Year"] = year;
            Session["Grid_Month"] = month;

            string monthName = "";
            dbcl.FindMonthName(month, ref monthName);
            lbl_month.Text = monthName;
            lbl_year.Text = year;
            lbl_monthcode.Text = month;

            try
            {
                StringBuilder query = new StringBuilder();
                query.Append("SELECT Id, CreatedDate, Creator_Workman, JOBID, JOBID_Status, FinalUpldStatus, JOB_Site, JOB_Location, JOB_InchargeName, Incharge_Approval, JOB_Shift, JOB_PermitNo, ManpowerCount, JOB_Title FROM tbl_jobs WHERE Creator_Workman=@Workman AND YEAR(CreatedDate)=@Year AND MONTH(CreatedDate)=@Month AND ISNULL(DeleteStatus, 0) = 0 ");

                // Dynamically append Status Filters
                string statusFilter = DDL_JobStatus.SelectedValue;
                switch (statusFilter)
                {
                    case "1": query.Append("AND JOBID_Status='Active' "); break;
                    case "2": query.Append("AND JOBID_Status!='Active' "); break;
                    case "3": query.Append("AND FinalUpldStatus!='Yes' "); break;
                    case "4": query.Append("AND FinalUpldStatus='Yes' "); break;
                    case "5": query.Append("AND Incharge_Approval='Pending' "); break;
                    case "6": query.Append("AND Incharge_Approval='Approved' "); break;
                    case "7": query.Append("AND Incharge_Approval='Returned' "); break;
                    case "8": query.Append("AND Incharge_Approval='Rejected' "); break;
                }

                // Dynamically append Type Filters
                if (DDL_BillingType.SelectedValue != "0")
                {
                    query.Append("AND BillingCode=@BillingCode ");
                }

                query.Append("ORDER BY CreatedDate DESC");

                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand cmd = new SqlCommand(query.ToString(), dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Workman", Session["WORKMAN"].ToString());
                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.Parameters.AddWithValue("@Month", month);

                    if (DDL_BillingType.SelectedValue != "0")
                        cmd.Parameters.AddWithValue("@BillingCode", DDL_BillingType.SelectedValue);

                    using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        ad.Fill(dt);
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                }
            }
            catch (Exception ex) { ShowNotification("Database Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        // =================================================================================
        // UI & GRIDVIEW EVENTS
        // =================================================================================
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbl_JOBID_Status = (Label)e.Row.FindControl("lbl_JOBID_Status");
                Label lbl_Incharge_Approval = (Label)e.Row.FindControl("lbl_Incharge_Approval");
                Label lbl_JOB_PermitNo = (Label)e.Row.FindControl("lbl_JOB_PermitNo");
                Label lbl_FinalUpldStatus = (Label)e.Row.FindControl("lbl_FinalUpldStatus");
                Button btn_status = (Button)e.Row.FindControl("btn_jobidstatus");

                string jobidstatus = lbl_JOBID_Status.Text;
                string approvalstatus = lbl_Incharge_Approval.Text;
                string lblupldstatus = lbl_FinalUpldStatus.Text;

                // 1. System Status Button Styling
                if (jobidstatus == "Active")
                {
                    btn_status.CssClass = "btn btn-success grid-action-btn";
                }
                else
                {
                    btn_status.CssClass = "btn btn-danger grid-action-btn";
                }

                // 2. Approval Status Styling
                if (approvalstatus == "Approved")
                {
                    lbl_Incharge_Approval.CssClass = "small font-weight-bold text-success";
                    btn_status.Enabled = false; // Lock status swapping if approved
                }
                else if (approvalstatus == "Rejected" || approvalstatus == "Returned")
                {
                    lbl_Incharge_Approval.CssClass = "small font-weight-bold text-danger";
                    btn_status.Enabled = true;
                }
                else
                {
                    lbl_Incharge_Approval.CssClass = "small font-weight-bold text-warning";
                    btn_status.Enabled = true;
                }

                // 3. Permit Styling
                if (lblupldstatus == "Yes")
                {
                    lbl_JOB_PermitNo.CssClass = "badge bg-green";
                }
                else
                {
                    lbl_JOB_PermitNo.CssClass = "badge bg-red";
                }
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridView1.Rows[rowIndex];

            string dbid = (row.FindControl("lbl_Id") as Label).Text;
            string jobid = (row.FindControl("lbl_JOBID") as Label).Text;
            string supv = (row.FindControl("lbl_Creator_Workman") as Label).Text;
            string jobidstatus = (row.FindControl("lbl_JOBID_Status") as Label).Text;

            if (e.CommandName == "Swap_JOBIDStatus")
            {
                if (jobidstatus == "Blocked")
                {
                    JOBID_Status_Swaper(jobid, dbid, "Active");
                }
                else
                {
                    if (CC.CheckforPendingOUT(jobid, supv) == 0)
                    {
                        if (CC.CheckforPendingPermit(jobid, supv) == 0)
                        {
                            JOBID_Status_Swaper(jobid, dbid, "Blocked");
                        }
                        else
                        {
                            ShowNotification("Blocked", "Permit NOT Uploaded Yet.", "warning");
                        }
                    }
                    else
                    {
                        ShowNotification("Blocked", "There are Pending OUT Punches. Cannot block.", "error");
                    }
                }
            }
            else if (e.CommandName == "View_Details")
            {
                Response.Redirect($"view_jobdetails_v2.aspx?JOBID={jobid}&dbid={dbid}&supv={supv}", false);
            }
            else if (e.CommandName == "DeleteRec") // Safely renamed to prevent native Delete conflicts
            {
                JOBID_Delete(dbid, jobid);
            }
        }

        // =================================================================================
        // SECURE DATABASE OPERATIONS
        // =================================================================================
        private void JOBID_Status_Swaper(string jobid, string dbid, string newStatus)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand cmd = new SqlCommand("UPDATE tbl_jobs SET JOBID_Status = @Status WHERE JOBID = @JOBID AND Id = @Id", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    cmd.Parameters.AddWithValue("@Id", dbid);
                    cmd.ExecuteNonQuery();
                }

                JobWorkflowLogger.LogAction(jobid, "STATUS SWAP (BLOCK/UNBLOCK)", Session["WORKMAN"] != null ? Session["WORKMAN"].ToString() : "Unknown",
                    $"JOB {jobid} status changed to '{newStatus}'.");

                ShowNotification("Success", $"JOB Status changed to {newStatus}", "success");
                GridBinder(lbl_year.Text, lbl_monthcode.Text); // Refresh UI
            }
            catch (Exception ex) { ShowNotification("Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        protected void JOBID_Delete(string id, string dbcode)
        {
            string deletedBy = Session["WORKMAN"] != null ? Session["WORKMAN"].ToString() : "Unknown";

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // 1. BEGIN THE TRANSACTION
                using (SqlTransaction transaction = dbcl.Conn.BeginTransaction())
                {
                    try
                    {
                        // Soft Delete Main Job
                        string qry1 = "UPDATE tbl_jobs SET DeleteStatus = 1, ViewStatus = 0, DeletedOn = GETDATE(), DeletedBy = @DeletedBy WHERE Id=@Id AND JOBID=@JOBID";
                        using (SqlCommand cmd1 = new SqlCommand(qry1, dbcl.Conn, transaction))
                        {
                            cmd1.Parameters.AddWithValue("@Id", id);
                            cmd1.Parameters.AddWithValue("@JOBID", dbcode);
                            cmd1.Parameters.AddWithValue("@DeletedBy", deletedBy);
                            cmd1.ExecuteNonQuery();
                        }

                        // Soft Delete Attendance
                        string qry2 = "UPDATE tbl_attendance SET DeleteStatus = 1, ViewStatus = 0, DeletedOn = GETDATE(), DeletedBy = @DeletedBy WHERE JOBID=@JOBID";
                        using (SqlCommand cmd2 = new SqlCommand(qry2, dbcl.Conn, transaction))
                        {
                            cmd2.Parameters.AddWithValue("@JOBID", dbcode);
                            cmd2.Parameters.AddWithValue("@DeletedBy", deletedBy);
                            cmd2.ExecuteNonQuery();
                        }

                        // Soft Delete Permits
                        string qry3 = "UPDATE tbl_jobspermit SET DeleteStatus = 1, ViewStatus = 0, DeletedOn = GETDATE(), DeletedBy = @DeletedBy WHERE JOBID=@JOBID";
                        using (SqlCommand cmd3 = new SqlCommand(qry3, dbcl.Conn, transaction))
                        {
                            cmd3.Parameters.AddWithValue("@JOBID", dbcode);
                            cmd3.Parameters.AddWithValue("@DeletedBy", deletedBy);
                            cmd3.ExecuteNonQuery();
                        }

                        // 2. IF ALL SUCCEED, COMMIT THE CHANGES TO DATABASE
                        transaction.Commit();

                        JobWorkflowLogger.LogAction(dbcode, "JOB DELETED (SOFT)", deletedBy,
                            $"JOB {dbcode} (Id={id}) soft-deleted along with its attendance and permit records.");

                        ShowNotification("Deleted", "Job and associated records have been removed successfully.", "success");
                        GridBinder(lbl_year.Text, lbl_monthcode.Text); // Refresh UI
                    }
                    catch (Exception ex)
                    {
                        // 3. IF ANY STEP FAILS, ROLLBACK EVERYTHING!
                        transaction.Rollback();
                        throw new Exception("Database update interrupted. All changes reversed. Error: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Delete Failed", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        // =================================================================================
        // BUTTON EVENTS
        // =================================================================================
        protected void btn_submit_Click(object sender, EventArgs e)
        {
            GridBinder(lbl_year.Text, lbl_monthcode.Text);
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            txt_quicksearch.Text = "";
            DDL_JobStatus.SelectedIndex = 0;
            DDL_BillingType.SelectedIndex = 0;

            lbl_year.Text = DateTime.Now.Year.ToString();
            lbl_monthcode.Text = DateTime.Now.Month.ToString().PadLeft(2, '0');

            // SOLUTION 2: Clear saved state when explicitly resetting
            Session.Remove("Grid_Year");
            Session.Remove("Grid_Month");

            GridBinder(lbl_year.Text, lbl_monthcode.Text);
        }

        protected void btn_prevmonth_Click(object sender, EventArgs e)
        {
            int year = Convert.ToInt32(lbl_year.Text);
            int month = Convert.ToInt32(lbl_monthcode.Text);

            if (month == 1)
            {
                month = 12;
                year -= 1;
            }
            else { month -= 1; }

            GridBinder(year.ToString(), month.ToString().PadLeft(2, '0'));
        }

        protected void btn_currentdata_Click(object sender, EventArgs e)
        {
            GridBinder(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString().PadLeft(2, '0'));
        }

        protected void btn_nextmonth_Click(object sender, EventArgs e)
        {
            int year = Convert.ToInt32(lbl_year.Text);
            int month = Convert.ToInt32(lbl_monthcode.Text);

            if (month == 12)
            {
                month = 1;
                year += 1;
            }
            else { month += 1; }

            GridBinder(year.ToString(), month.ToString().PadLeft(2, '0'));
        }
    }
}