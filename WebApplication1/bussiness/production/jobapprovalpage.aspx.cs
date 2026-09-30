/* When: 2026-09-30 | Why: Approve and Reject postbacks changed tbl_jobs and tbl_attendance without proving the signed-in operator is that job's in-charge, without a conditional pending-state predicate, and without one transaction, so a repeated postback or a failed master update could leave a partial approval. | What: The live Approve and Reject path now re-reads tbl_jobs, authorizes Session WORKMAN against JOB_InchargeWrk, applies the existing out-punch approval predicate and 72-hour/admin lock inside one transaction, requires the job update to affect one row and the attendance update to affect every matching non-deleted row, rolls back when either mutation does not, and writes the existing success audit only after commit. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace WebApplication1.bussiness.production
{
    public partial class jobapprovalpage : System.Web.UI.Page
    {
        // Default folder
        static readonly string rootFolder = @"C:\atswork.in\wwwroot\erp_images\Permits";

        //static readonly string rootFolder = @"D:\OH4Y Works\OH4Y_2021\Demo\WebApplication1\WebApplication1\erp_images\Permits";

        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
        DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    //ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                    string jobid = Request.QueryString["JOBID"];

                    string CmdString2 = "select BilingType, BillingCode from tlb_JOB_BillingType order by Id";
                    Bind_BillingType(CmdString2);

                    string CmdString4 = "Select Status_Name,Status_Code from tlb_attendancecodes where Approver='Yes' and Status='Present' order by slno";
                    Bind_AttendnaceCode(CmdString4);

                    string CmdString5 = "select Worksite_Name, DB_Code from tlb_atsworksites order by Id";
                    BindWorkSites(CmdString5);

                    string CmdString = "select Employee_Name, Employee_Workman from tlb_atsworksiteIncharges order by Id";
                    Bind_Approver(CmdString);

                    Bind_JOBIDDetails(jobid);
                }
            }
        }

        private void BindWorkSites(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Worksite.DataSource = Cmd.ExecuteReader();
            DDL_Worksite.DataTextField = "Worksite_Name";
            DDL_Worksite.DataValueField = "DB_Code";
            DDL_Worksite.DataBind();
            DDL_Worksite.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        private void Bind_Approver(string CmdString, SqlParameter parameter = null)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            if (parameter != null)
            {
                Cmd.Parameters.Add(parameter);
            }
            Cmd.CommandType = CommandType.Text;
            DDL_Approver.DataSource = Cmd.ExecuteReader();
            DDL_Approver.DataTextField = "Employee_Name";
            DDL_Approver.DataValueField = "Employee_Workman";
            DDL_Approver.DataBind();
            DDL_Approver.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        private void Bind_BillingType(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_BillingType.DataSource = Cmd.ExecuteReader();
            DDL_BillingType.DataTextField = "BilingType";
            DDL_BillingType.DataValueField = "BillingCode";
            DDL_BillingType.DataBind();
            DDL_BillingType.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        //private void Bind_AttendnaceType(string CmdString)
        //{
        //    dbcl.Sqlconnection();
        //    dbcl.ConnectDb();
        //    SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
        //    Cmd.CommandType = CommandType.Text;
        //    DDL_AttenType.DataSource = Cmd.ExecuteReader();
        //    DDL_AttenType.DataTextField = "Status";
        //    DDL_AttenType.DataValueField = "Status";
        //    DDL_AttenType.DataBind();
        //    DDL_AttenType.Items.Insert(0, "Please Select Option");
        //    dbcl.DisconnectDb();
        //}

        private void Bind_AttendnaceCode(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_AttenCode.DataSource = Cmd.ExecuteReader();
            DDL_AttenCode.DataTextField = "Status_Name";
            DDL_AttenCode.DataValueField = "Status_Code";
            DDL_AttenCode.DataBind();
            DDL_AttenCode.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }


        public bool Is72HoursElapsed(DateTime startDate)
        {
            // Retrieve the threshold hours value from web.config
            int thresholdHours = Convert.ToInt32(ConfigurationManager.AppSettings["TimeThresholdHours"]);

            // Calculate the difference between current time and the start date
            TimeSpan elapsedTime = DateTime.Now - startDate;

            // Check if the elapsed time is greater than or equal to the threshold hours
            return elapsedTime.TotalHours <= thresholdHours;
        }

        public string GetElapsedTime(DateTime startDate)
        {
            TimeSpan elapsedTime = DateTime.Now - startDate;
            return string.Format("{0} days, {1} hours, {2} minutes, {3} seconds",
                                  elapsedTime.Days, elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
        }


        // 3. The Enforcer Method
        public bool Is72HoursElapsed(DateTime startDate, object unblockedUntilObj)
        {
            // RULE 1: Check if HR Admin has granted a 24-hour shield
            if (unblockedUntilObj != DBNull.Value && unblockedUntilObj != null)
            {
                DateTime unblockedUntil = Convert.ToDateTime(unblockedUntilObj);
                if (DateTime.Now <= unblockedUntil)
                {
                    // Shield is active! The Approver is temporarily allowed to approve.
                    return false;
                }
            }

            // RULE 2: If there is no shield, calculate the strict 72-hour rule
            int thresholdHours = Convert.ToInt32(ConfigurationManager.AppSettings["TimeThresholdHours"]);
            TimeSpan elapsedTime = DateTime.Now - startDate;

            // If 72 hours have passed, this returns TRUE and locks the Approver out.
            return elapsedTime.TotalHours >= thresholdHours;
        }

        private void Bind_JOBIDDetails(string jobid)
        {
            try
            {
                string query = "select * from tbl_jobs where JOBID=@JOBID";
                SqlParameter[] pram = { new SqlParameter("@JOBID", jobid) };
                dt = dbcl.SPreturn_dt(query, pram);

                if (dt.Rows.Count > 0)
                {
                    // 1. Safe Data Extraction (Handles DBNulls gracefully)
                    DataRow row = dt.Rows[0];
                    DateTime createdDate = row["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(row["CreatedDate"]) : DateTime.Now;
                    bool isDbBlocked = row["IsBlocked"] != DBNull.Value && Convert.ToBoolean(row["IsBlocked"]);
                    object unblockedUntilObj = row["UnblockedUntil"];
                    string entryExitStatus = row["EntryExit"].ToString();
                    string jobIdStatus = row["JOBID_Status"].ToString();

                    // 2. Evaluate Pipeline State
                    bool isElapsed = Is72HoursElapsed(createdDate, unblockedUntilObj);
                    bool finalBlock = isDbBlocked || isElapsed;

                    // 3. Populate Basic UI Elements
                    txt_jobdate.Text = createdDate.ToString("dd-MM-yyyy");
                    txt_workorderno.Text = row["WorkOrderNo"].ToString();
                    txt_jobid.Text = row["JOBID"].ToString();
                    lbl_jobidsstatus.Text = jobIdStatus;
                    txt_worksitename.Text = row["JOB_Site"].ToString();
                    lbl_worksitedbcode.Text = row["JOB_SiteCode"].ToString();

                    // 4. Bind Dependent Dropdowns
                    DDL_Worksite.SelectedValue = lbl_worksitedbcode.Text;
                    string CmdString = "select Employee_Name, Employee_Workman from tlb_atsworksiteIncharges where DB_Code=@DBCode order by Id";
                    Bind_Approver(CmdString, new SqlParameter("@DBCode", lbl_worksitedbcode.Text));
                    DDL_Approver.SelectedValue = lbl_worksitedbcode.Text;

                    // 5. Populate Remaining Job Info
                    lbl_inchargewrk.Text = row["JOB_InchargeWrk"].ToString();
                    txt_inchargename.Text = row["JOB_InchargeName"].ToString();
                    txt_jobdept.Text = row["JOB_Dept"].ToString();
                    txt_jobloc.Text = row["JOB_Location"].ToString();
                    txt_jobshift.Text = row["JOB_Shift"].ToString();
                    txt_permitno.Text = row["JOB_PermitNo"].ToString();
                    lbl_permituploaddate.Text = row["PermitUploadDate"].ToString();
                    lbl_filecount.Text = row["FileCount"].ToString();
                    lbl_permitdeleteddate.Text = row["PermitDeleteDate"].ToString();
                    lbl_permitdeletedby.Text = row["PermitDeletedByName"].ToString();
                    txt_jobtitle.Text = row["JOB_Title"].ToString();
                    txt_approverrmrks.Text = "N/A"; // Or bind from DB if applicable

                    // 6. Set Dropdown Values
                    DDL_BillingType.SelectedValue = row["BillingCode"].ToString();
                    DDL_AttenCode.SelectedValue = row["AttendanceCode"].ToString();

                    // 7. Evaluate Permit Upload Status
                    if (row["FinalUpldStatus"].ToString() == "Yes")
                    {
                        lbl_prmtupldstatus.Text = "Uploaded";
                        lbl_prmtupldstatus.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lbl_prmtupldstatus.Text = "Pending";
                        lbl_prmtupldstatus.ForeColor = System.Drawing.Color.Red;
                    }

                    // 8. The Core Logic: Evaluate Incharge Approval Status
                    string approvalstatus = row["Incharge_Approval"].ToString();

                    if (approvalstatus == "Approved")
                    {
                        lbl_approvalstatus.Text = "Approved";
                        lbl_approvalstatus.ForeColor = System.Drawing.Color.Green;
                        btn_approve.Visible = true;
                        btn_approve.Enabled = false;
                        btn_approve.Text = "Approved";
                        btn_reject.Visible = false;
                    }
                    else if (approvalstatus == "Rejected")
                    {
                        lbl_approvalstatus.Text = "Rejected";
                        lbl_approvalstatus.ForeColor = System.Drawing.Color.Green;
                        btn_approve.Visible = false;
                        btn_reject.Visible = true;
                        btn_reject.Enabled = false;
                        btn_reject.Text = "Rejected";
                    }
                    else // "Pending" state
                    {
                        // Verify if the system is enforcing the 72-hour or Admin Lockout
                        if (finalBlock && entryExitStatus == "Exit" && jobIdStatus == "Blocked")
                        {
                            btn_approve.Enabled = false;
                            btn_reject.Enabled = false;
                            btn_update.Enabled = false;
                            btn_update.Visible = false;

                            string title = "Approval Window Closed:";
                            string body = isElapsed
                                ? "The 72-hour window to approve this job has expired. Please contact HR Admin to request a 24-hour unblock."
                                : "This job is currently blocked by the system.";

                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                            lbl_approvalstatus.Text = isElapsed ? "Blocked (Time Expired)" : "Blocked (System)";
                            lbl_approvalstatus.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            // Open for normal approval
                            btn_approve.Enabled = true;
                            btn_reject.Enabled = true;
                            btn_update.Enabled = true;
                            btn_update.Visible = true;

                            lbl_approvalstatus.Text = "Pending";
                            lbl_approvalstatus.ForeColor = System.Drawing.Color.Red;
                        }
                    }

                    // 9. Bind Dependent Grids AFTER we know the job exists
                    string CmdString2 = "select * from tbl_jobspermit where JOBID=@JOBID order by Id desc";
                    BindGrid(CmdString2, new SqlParameter[] { new SqlParameter("@JOBID", jobid) });

                    string CmdString3 = "select * from tbl_attendance where JOBID=@JOBID order by Id desc";
                    BindGrid2(CmdString3, new SqlParameter[] { new SqlParameter("@JOBID", jobid) });
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void BindGrid(string cmdString, SqlParameter[] parameters = null)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            dbcl.Conn.Close();
        }

        private void BindGrid2(string cmdString, SqlParameter[] parameters = null)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            GridView2.DataSource = ds;
            GridView2.DataBind();
            dbcl.Conn.Close();
        }

        //protected void DownloadFile(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int id = int.Parse((sender as LinkButton).CommandArgument);
        //        byte[] bytes;
        //        string fileName, contentType;
        //        string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
        //        using (SqlConnection con = new SqlConnection(constr))
        //        {
        //            using (SqlCommand cmd = new SqlCommand())
        //            {
        //                cmd.CommandText = "select Name, Data, FileType from tbl_jobspermit where Id=@Id";
        //                cmd.Parameters.AddWithValue("@Id", id);
        //                cmd.Connection = con;
        //                con.Open();
        //                using (SqlDataReader sdr = cmd.ExecuteReader())
        //                {
        //                    sdr.Read();
        //                    bytes = (byte[])sdr["Data"];
        //                    contentType = sdr["FileType"].ToString();
        //                    fileName = sdr["Name"].ToString();
        //                }
        //                con.Close();
        //            }
        //        }
        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.Charset = "";
        //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        Response.ContentType = contentType;
        //        Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
        //        Response.BinaryWrite(bytes);
        //        Response.Flush();
        //        Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        string title = "Notifications :";
        //        string body = "Error : " + ex.Message;
        //        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
        //    }
        //}
        protected void DownloadFile_0(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse((sender as LinkButton).CommandArgument);
                string fileName;
                string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "select Name from tbl_jobspermit where Id=@Id";
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            sdr.Read();
                            fileName = sdr["Name"].ToString();
                        }
                        con.Close();
                    }
                }
                try
                {
                    // Check if file exists with its full path
                    if (File.Exists(Path.Combine(rootFolder, fileName)))
                    {
                        Response.Clear();
                        Response.ContentType = "application/octect-stream";
                        Response.AppendHeader("content-disposition", "filename=" + fileName);
                        Response.TransmitFile(Server.MapPath(@"\erp_images\Permits\") + fileName);
                        Response.End();

                        Update_permitDownloadStatus(id.ToString());
                    }
                    else
                    {
                        string title = "Notifications :";
                        string body = "NO Physical File Found...!!";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }
                catch (IOException ioExp)
                {
                    string title = "Notifications :";
                    string body = ioExp.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void DownloadFile(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse((sender as LinkButton).CommandArgument);
                string fileName;
                string folderPath = Server.MapPath(@"\erp_images\Permits\"); // Specify the folder path
                string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "select Name from tbl_jobspermit where Id=@Id";
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            sdr.Read();
                            fileName = sdr["Name"].ToString();
                        }
                        con.Close();
                    }
                }
                try
                {
                    string filePath = Path.Combine(folderPath, fileName);
                    // Check if file exists with its full path
                    if (File.Exists(filePath))
                    {
                        Update_permitDownloadStatus(id.ToString());

                        Response.Clear();
                        Response.ContentType = "application/octect-stream";
                        Response.AppendHeader("content-disposition", "filename=" + fileName);
                        Response.TransmitFile(filePath);
                        Response.End();


                    }
                    else
                    {
                        string title = "Notifications :";
                        string body = "NO Physical File Found...!!";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }
                catch (IOException ioExp)
                {
                    string title = "Notifications :";
                    string body = ioExp.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }


        private void Update_permitDownloadStatus(string dbid)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_jobspermit set DownloadStatus=@DownloadStatus where JOBID=@JOBID and Id=@Id";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", dbid);
                cmd.Parameters.AddWithValue("@JOBID", txt_jobid.Text.ToString());
                cmd.Parameters.AddWithValue("@DownloadStatus", 1);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }



        //----------------------------- Gridview - Action Buttons ---------------------------------------//
        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView2.EditIndex = e.NewEditIndex;

            string jobid = txt_jobid.Text.ToString();
            string CmdString3 = "select * from tbl_attendance where JOBID=@JOBID order by Id desc";
            BindGrid2(CmdString3, new SqlParameter[] { new SqlParameter("@JOBID", jobid) });
        }

        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView2.EditIndex = -1;

            string jobid = txt_jobid.Text.ToString();
            string CmdString3 = "select * from tbl_attendance where JOBID=@JOBID order by Id desc";
            BindGrid2(CmdString3, new SqlParameter[] { new SqlParameter("@JOBID", jobid) });
        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label ID = (Label)GridView2.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            Label JOBID = (Label)GridView2.Rows[e.RowIndex].FindControl("lbl_JOBID");
            string jobid = JOBID.Text.ToString();

            Label lbl_JOB_Region = (Label)GridView2.Rows[e.RowIndex].FindControl("lbl_JOB_Region");
            string region = lbl_JOB_Region.Text.ToString();

            Label empwrk = (Label)GridView2.Rows[e.RowIndex].FindControl("lbl_EmployeeWrk");
            string workmansl = empwrk.Text.ToString();

            Label wrkhrs = (Label)GridView2.Rows[e.RowIndex].FindControl("lbl_WourkHours");
            Int32 emp_wrkhours = Convert.ToInt32(wrkhrs.Text.ToString());
            Int32 emp_wrkmnis = emp_wrkhours * 60;

            TextBox TextBoxWithIntime = (TextBox)GridView2.Rows[e.RowIndex].FindControl("txt_Inpunch_Time");
            string new_intitme = TextBoxWithIntime.Text.ToString();
            DateTime timein = DateTime.ParseExact(new_intitme, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
            string convtimein = timein.ToString("yyyy-MM-dd hh:mm:ss tt");


            TextBox TextBoxWithOuttime = (TextBox)GridView2.Rows[e.RowIndex].FindControl("txt_Outpunch_Time");
            string new_outtime = TextBoxWithOuttime.Text.ToString();
            DateTime timeout = DateTime.ParseExact(new_outtime, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
            string convtimeout = timeout.ToString("yyyy-MM-dd hh:mm:ss tt");


            DropDownList DDL_Lunch = (DropDownList)GridView2.Rows[e.RowIndex].FindControl("DDL_LunchYesNo");
            string lunchyesno = DDL_Lunch.SelectedItem.Text.ToString();

            TextBox TextBoxWithOt = (TextBox)GridView2.Rows[e.RowIndex].FindControl("txt_ProvidedOT");
            string new_ot = TextBoxWithOt.Text.ToString();
            decimal new_pot = Convert.ToDecimal(new_ot);

            DropDownList AttendanceStatus = (DropDownList)GridView2.Rows[e.RowIndex].FindControl("DDL_AttendanceStatus");
            string ddl_newattensttaus = AttendanceStatus.SelectedItem.Text.ToString();

            DropDownList AttendanceCode = (DropDownList)GridView2.Rows[e.RowIndex].FindControl("DDL_AttendanceCode");
            string ddl_newattencode = AttendanceCode.SelectedValue.ToString();

            Label lbl_Calc_OT = (Label)GridView2.Rows[e.RowIndex].FindControl("lbl_Calc_OT");

            Int32 workdmins = 0;
            decimal workedhours = .0m;
            dbcl.FindEmployeeWorkedTime(convtimein, convtimeout, ref workdmins, ref workedhours);
            decimal emp_calOThrs = .0m ;

            if (region == "NINL")
            {
                dbcl.CalculateOvertimeRev(emp_wrkmnis, workdmins, lunchyesno, ref emp_calOThrs);
            }
            else
            {
                dbcl.CalculateOvertime(emp_wrkmnis, workdmins, lunchyesno, ref emp_calOThrs);
            }

            //dbcl.CalculateOvertime(emp_wrkmnis, workdmins, lunchyesno, ref emp_calOThrs);
            lbl_Calc_OT.Text = emp_calOThrs.ToString();

            UpdateDetails(id, jobid, workmansl, convtimein, convtimeout, lunchyesno, workdmins, workedhours, emp_calOThrs, new_pot,  ddl_newattensttaus, ddl_newattencode);

            GridView2.EditIndex = -1;

            string CmdString3 = "select * from tbl_attendance where JOBID=@JOBID order by Id desc";
            BindGrid2(CmdString3, new SqlParameter[] { new SqlParameter("@JOBID", jobid) });

            Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label ID = (Label)GridView2.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            Label JOBID = (Label)GridView2.Rows[e.RowIndex].FindControl("lbl_JOBID");
            string dbjobid = JOBID.Text.ToString();

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "delete from tbl_attendance where Id=@Id and JOBID=@JOBID";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add(new SqlParameter("@Id", id));
                cmd.Parameters.Add(new SqlParameter("@JOBID", dbjobid));
                cmd.ExecuteNonQuery();
                dbcl.Conn.Close();

                string title = "Notifications :";
                string body = "Data has been DELETED !!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }



            string jobid = txt_jobid.Text.ToString();
            string CmdString3 = "select * from tbl_attendance where JOBID=@JOBID order by Id desc";
            BindGrid2(CmdString3, new SqlParameter[] { new SqlParameter("@JOBID", jobid) });
        }



        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && GridView2.EditIndex == e.Row.RowIndex)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var DDL_AttendanceStatus = e.Row.FindControl("DDL_AttendanceStatus") as DropDownList;
                    if (DDL_AttendanceStatus != null)
                    {
                        var dt1 = new DataTable();
                        string cnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString();
                        using (var con = new SqlConnection(cnnString1))
                        {
                            con.Open();
                            var cmd1 = new SqlCommand("Select DISTINCT Status from tlb_attendancecodes where Approver='Yes'", con);
                            var da1 = new SqlDataAdapter(cmd1);
                            da1.Fill(dt1);
                            con.Close();
                        }

                        DDL_AttendanceStatus.DataSource = dt1;
                        DDL_AttendanceStatus.DataTextField = "Status";
                        DDL_AttendanceStatus.DataValueField = "Status";
                        DDL_AttendanceStatus.DataBind();
                        string AttendanceStatus = DataBinder.Eval(e.Row.DataItem, "AttendanceStatus").ToString();
                        DDL_AttendanceStatus.Items.FindByText(AttendanceStatus).Selected = true;
                    }



                    var DDL_AttendanceCode = e.Row.FindControl("DDL_AttendanceCode") as DropDownList;
                    if (DDL_AttendanceCode != null)
                    {
                        var dt2 = new DataTable();
                        string cnnString2 = System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString();
                        using (var con = new SqlConnection(cnnString2))
                        {
                            con.Open();
                            var cmd2 = new SqlCommand("Select Status_Name,Status_Code from tlb_attendancecodes where Approver='Yes' order by slno", con);
                            var da2 = new SqlDataAdapter(cmd2);
                            da2.Fill(dt2);
                            con.Close();
                        }

                        DDL_AttendanceCode.DataSource = dt2;
                        DDL_AttendanceCode.DataTextField = "Status_Name";
                        DDL_AttendanceCode.DataValueField = "Status_Code";
                        DDL_AttendanceCode.DataBind();
                        string AttendanceCode = DataBinder.Eval(e.Row.DataItem, "AttendanceCode").ToString();
                        DDL_AttendanceCode.Items.FindByValue(AttendanceCode).Selected = true;
                    }
                }
            }

            for (int i = 0; i <= GridView2.Rows.Count - 1; i++)
            {
                Label lbl_Calc_OT = (Label)GridView2.Rows[i].FindControl("lbl_Calc_OT");
                Label lbl_ProvidedOT = (Label)GridView2.Rows[i].FindControl("lbl_ProvidedOT");
                TextBox txt_ProvidedOT = (TextBox)GridView2.Rows[i].FindControl("txt_ProvidedOT");

                Label lbl_WourkHours = (Label)GridView2.Rows[i].FindControl("lbl_WourkHours");
                Label lbl_WorkedHours = (Label)GridView2.Rows[i].FindControl("lbl_WorkedHours");

                decimal wrkhrs = Convert.ToDecimal(lbl_WourkHours.Text.ToString());
                decimal wrkdhrs = Convert.ToDecimal(lbl_WorkedHours.Text.ToString());

                decimal provided_ot = .0m;
                if (lbl_ProvidedOT == null)
                {
                    provided_ot = Convert.ToDecimal(txt_ProvidedOT.Text.ToString());
                }
                else
                {
                    provided_ot = Convert.ToDecimal(lbl_ProvidedOT.Text.ToString());
                }

                if (wrkdhrs > wrkhrs)
                {
                    if (wrkdhrs == (wrkhrs + 1) )
                    {
                        lbl_WourkHours.ForeColor = Color.Blue;
                        lbl_WorkedHours.ForeColor = Color.Blue;
                        GridView2.Rows[i].Cells[8].BackColor = Color.SkyBlue;
                        GridView2.Rows[i].Cells[12].BackColor = Color.SkyBlue;
                    }
                    else
                    {
                        lbl_WorkedHours.ForeColor = Color.DarkGreen;
                        GridView2.Rows[i].Cells[12].BackColor = Color.Yellow;
                    }

                }
                else if (wrkdhrs == wrkhrs)
                {
                    lbl_WourkHours.ForeColor = Color.Blue;
                    lbl_WorkedHours.ForeColor = Color.Blue;
                    GridView2.Rows[i].Cells[8].BackColor = Color.SkyBlue;
                    GridView2.Rows[i].Cells[12].BackColor = Color.SkyBlue;
                }
                else
                {
                    lbl_WourkHours.ForeColor = Color.Black;
                }

                double prvd_ot = Convert.ToDouble(provided_ot);
                double calc_ot = Convert.ToDouble(lbl_Calc_OT.Text.ToString());

                if (calc_ot > prvd_ot)
                {
                    GridView2.Rows[i].Cells[14].BackColor = Color.Yellow;
                    lbl_Calc_OT.ForeColor = Color.Red;
                }
                else if (prvd_ot > calc_ot)
                {
                    GridView2.Rows[i].Cells[15].BackColor = Color.Yellow;
                    //lbl_emp_overtime.ForeColor = Color.Red;
                }
                else
                {
                    GridView2.Rows[i].Cells[14].BackColor = Color.LightGreen;
                    lbl_Calc_OT.ForeColor = Color.Black;
                    GridView2.Rows[i].Cells[15].BackColor = Color.LightGreen;
                    //lbl_emp_overtime.ForeColor = Color.Black;
                }
            }
        }


        private void UpdateDetails(string id, string jobid, string empwrk, string intime, string outime, string lunchyesno, Int32 workdmins, decimal workedhours, decimal emp_calOThrs, decimal new_pot, string ddl_newattensttaus, string ddl_newattencode)
        {
            try
            {
                DateTime timein = DateTime.ParseExact(intime, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
                string convtimein = timein.ToString("yyyy-MM-dd hh:mm:ss tt");

                DateTime timeout = DateTime.ParseExact(outime, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
                string convtimeout = timeout.ToString("yyyy-MM-dd hh:mm:ss tt");

                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_attendance set Inpunch_Time=@Inpunch_Time, Outpunch_Time=@Outpunch_Time, WorkedTime=@WorkedTime , WorkedHours=@WorkedHours, LunchFactor=@LunchFactor, Calc_OT=@Calc_OT,  ProvidedOT=@ProvidedOT, LastModified=@LastModified, ModifiedByWrk=@ModifiedByWrk,ModifiedByName=@ModifiedByName,  AttendanceStatus=@AttendanceStatus, AttendanceCode=@AttendanceCode where Id=@Id and JOBID=@JOBID and EmployeeWrk=@EmployeeWrk";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                cmd.Parameters.AddWithValue("@EmployeeWrk", empwrk);

                cmd.Parameters.AddWithValue("@Inpunch_Time", convtimein);
                cmd.Parameters.AddWithValue("@Outpunch_Time", convtimeout);
                cmd.Parameters.AddWithValue("@WorkedTime", workdmins);
                cmd.Parameters.AddWithValue("@WorkedHours", workedhours);
                cmd.Parameters.AddWithValue("@LunchFactor", lunchyesno);
                cmd.Parameters.AddWithValue("@Calc_OT", emp_calOThrs);
                cmd.Parameters.AddWithValue("@ProvidedOT", new_pot);
                //cmd.Parameters.AddWithValue("@Approval_Date", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@LastModified", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@ModifiedByWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@ModifiedByName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@AttendanceStatus", ddl_newattensttaus);
                cmd.Parameters.AddWithValue("@AttendanceCode", ddl_newattencode);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                string title = "Notifications :";
                string body = "Data has been UPDATED !!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }

            Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void btn_approve_Click_OLD(object sender, EventArgs e)
        {
            try
            {
                string jobid = txt_jobid.Text.ToString();
                string remarks = txt_remarks.Text.ToString();
                string attendancecode = DDL_AttenCode.SelectedValue.ToString();

                if (Update_AttendanceTableStatus_OLD(jobid, "Approved", "Present", attendancecode) == true)
                {
                    Update_JOBTableStatus(jobid, "Blocked", "Approved by Approver", "5", "Approved", remarks);
                    txt_remarks.ReadOnly = true;
                    btn_approve.Enabled = false;
                    btn_approve.Text = "Approved";
                    btn_reject.Visible = false;

                    Bind_JOBIDDetails(jobid);
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }

            Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void btn_approve_Click(object sender, EventArgs e)
        {
            ApplyInchargeDecision("Approved", "Approved by Approver", "APPROVER: JOB APPROVED", "Shift approved by Site In-Charge. Attendance codes preserved.", "JOBID has been Approved.", true);
        }

        protected void btn_reject_Click(object sender, EventArgs e)
        {
            string remarks = txt_remarks.Text.Trim();
            if (string.IsNullOrEmpty(remarks) || remarks.Equals("N/A", StringComparison.OrdinalIgnoreCase))
            {
                ShowNotice("Validation Error:", "Please provide remarks for rejecting this job.");
                return;
            }

            ApplyInchargeDecision("Rejected", "Rejected by Approver", "APPROVER: JOB REJECTED", "Shift rejected by Site In-Charge.", "JOBID has been Rejected.", false);
        }

        private void ApplyInchargeDecision(string approvalStatus, string jobStatusText, string auditStep, string auditDetail, string successBody, bool isApprove)
        {
            string jobid = txt_jobid.Text == null ? "" : txt_jobid.Text.Trim();
            if (string.IsNullOrEmpty(jobid))
            {
                ShowNotice("Error :", "A valid JOBID is required.");
                return;
            }

            string operatorWrk = Session["WORKMAN"] == null ? "" : Session["WORKMAN"].ToString();
            if (string.IsNullOrWhiteSpace(operatorWrk))
            {
                ShowNotice("Error :", "Your session has expired. Please log in again.");
                return;
            }

            int thresholdHours;
            try
            {
                thresholdHours = Convert.ToInt32(ConfigurationManager.AppSettings["TimeThresholdHours"]);
            }
            catch (Exception ex)
            {
                ShowNotice("Error :", ex.Message);
                return;
            }

            DataRow jobRow = ReadAuthoritativeJob(jobid);
            if (jobRow == null)
            {
                ShowNotice("Error :", "This JOBID was not found.");
                return;
            }

            string denial = DescribeApprovalDenial(jobRow, operatorWrk);
            if (denial != null)
            {
                ShowNotice("Error :", denial);
                return;
            }

            string remarks = isApprove ? txt_remarks.Text.ToString() : txt_remarks.Text.Trim();
            string billingType;
            string billingCode;
            try
            {
                billingType = DDL_BillingType.SelectedItem.Text.ToString();
                billingCode = DDL_BillingType.SelectedValue.ToString();
            }
            catch (Exception ex)
            {
                ShowNotice("Error :", ex.Message);
                return;
            }

            bool committed = false;
            SqlTransaction transaction = null;
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                transaction = dbcl.Conn.BeginTransaction();
                DateTime asOf = DateTime.Now;

                int jobRows = ExecuteConditionalJobUpdate(transaction, jobid, operatorWrk, thresholdHours, asOf, "Blocked", jobStatusText, JobStatusConstants.CodeApproved, approvalStatus, remarks, billingType, billingCode);
                if (jobRows != 1)
                {
                    transaction.Rollback();
                    transaction = null;
                    ShowNotice("Error :", "This job could not be updated. It is no longer eligible for approval. No change was saved.");
                    return;
                }

                int expectedAttendance = CountLiveAttendance(transaction, jobid);
                int attendanceRows = ExecuteAttendanceApproval(transaction, jobid, approvalStatus);
                if (attendanceRows != expectedAttendance)
                {
                    transaction.Rollback();
                    transaction = null;
                    ShowNotice("Error :", "Attendance could not be updated for this job. No change was saved.");
                    return;
                }

                transaction.Commit();
                committed = true;
                transaction = null;
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    try { transaction.Rollback(); } catch { }
                }
                ShowNotice("Error :", ex.Message);
                return;
            }
            finally
            {
                try
                {
                    if (dbcl.Conn != null && dbcl.Conn.State != ConnectionState.Closed)
                    {
                        dbcl.DisconnectDb();
                    }
                }
                catch { }
            }

            if (!committed)
            {
                return;
            }

            JobWorkflowLogger.LogAction(jobid, auditStep, operatorWrk, auditDetail);

            if (isApprove)
            {
                txt_remarks.ReadOnly = true;
                btn_approve.Enabled = false;
                btn_approve.Text = "Approved";
                btn_reject.Visible = false;
            }
            else
            {
                txt_remarks.ReadOnly = true;
                btn_reject.Enabled = false;
                btn_reject.Text = "Rejected";
                btn_approve.Visible = false;
            }

            Bind_JOBIDDetails(jobid);
            ShowNotice("Success:", successBody);
        }

        private DataRow ReadAuthoritativeJob(string jobid)
        {
            string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand cmd = new SqlCommand("SELECT JOB_InchargeWrk, Incharge_Approval, JOB_Status, EntryExit, MasterStatusCode, JOBID_Status, IsBlocked, UnblockedUntil, CreatedDate FROM tbl_jobs WHERE JOBID=@JOBID", con))
            {
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    DataTable table = new DataTable();
                    table.Load(reader);
                    if (table.Rows.Count != 1)
                    {
                        return null;
                    }
                    return table.Rows[0];
                }
            }
        }

        private string DescribeApprovalDenial(DataRow row, string operatorWrk)
        {
            string incharge = row["JOB_InchargeWrk"] == DBNull.Value ? "" : row["JOB_InchargeWrk"].ToString();
            if (!string.Equals(incharge, operatorWrk, StringComparison.OrdinalIgnoreCase))
            {
                return "You are not authorized to approve or reject this job.";
            }

            string approval = row["Incharge_Approval"] == DBNull.Value ? "" : row["Incharge_Approval"].ToString();
            if (approval == "Approved")
            {
                return "This job is already approved.";
            }
            if (approval == "Rejected")
            {
                return "This job is already rejected.";
            }
            if (approval != "Pending" && approval != "Returned")
            {
                return "This job is not awaiting in-charge approval.";
            }

            string jobStatus = row["JOB_Status"] == DBNull.Value ? "" : row["JOB_Status"].ToString();
            string entryExit = row["EntryExit"] == DBNull.Value ? "" : row["EntryExit"].ToString();
            string masterCode = row["MasterStatusCode"] == DBNull.Value ? "" : row["MasterStatusCode"].ToString();
            if (jobStatus != JobStatusConstants.StatusOutPunchDone || entryExit != JobStatusConstants.EntryExitExit || masterCode != JobStatusConstants.CodeClosed)
            {
                return "This job is not in a state that can be approved or rejected.";
            }

            DateTime createdDate = row["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(row["CreatedDate"]) : DateTime.Now;
            bool isDbBlocked = row["IsBlocked"] != DBNull.Value && Convert.ToBoolean(row["IsBlocked"]);
            string jobIdStatus = row["JOBID_Status"] == DBNull.Value ? "" : row["JOBID_Status"].ToString();
            bool isElapsed = Is72HoursElapsed(createdDate, row["UnblockedUntil"]);
            bool finalBlock = isDbBlocked || isElapsed;
            if (finalBlock && entryExit == JobStatusConstants.EntryExitExit && jobIdStatus == "Blocked")
            {
                if (isElapsed)
                {
                    return "The 72-hour window to approve this job has expired. Please contact HR Admin to request a 24-hour unblock.";
                }
                return "This job is currently blocked by the system.";
            }

            return null;
        }

        private int ExecuteConditionalJobUpdate(SqlTransaction transaction, string jobid, string operatorWrk, int thresholdHours, DateTime asOf, string jobidStatus, string jobStatusText, string masterCode, string approvalStatus, string remarks, string billingType, string billingCode)
        {
            string cmdString = @"UPDATE tbl_jobs
                SET JOBID_Status=@JOBID_Status,
                    JOB_Status=@JOB_Status,
                    MasterStatusCode=@MasterStatusCode,
                    Incharge_Approval=@Incharge_Approval,
                    Incharge_Remarks=@Incharge_Remarks,
                    Incharge_ApprovalDate=@Incharge_ApprovalDate,
                    BillingType=@BillingType,
                    BillingCode=@BillingCode
                WHERE JOBID=@JOBID
                  AND JOB_InchargeWrk=@Workman
                  AND JOB_Status=@EligibleJobStatus
                  AND EntryExit=@EligibleEntryExit
                  AND MasterStatusCode=@EligibleMasterCode
                  AND Incharge_Approval IN (@PendingStatus, @ReturnedStatus)
                  AND NOT (
                        JOBID_Status = @BlockedStatus
                        AND (
                            ISNULL(IsBlocked, 0) = 1
                            OR (
                                CreatedDate IS NOT NULL
                                AND NOT (UnblockedUntil IS NOT NULL AND @AsOf <= UnblockedUntil)
                                AND DATEADD(HOUR, @ThresholdHours, CAST(CreatedDate AS datetime)) <= @AsOf
                            )
                        )
                  )";

            using (SqlCommand cmd = new SqlCommand(cmdString, transaction.Connection, transaction))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                cmd.Parameters.AddWithValue("@Workman", operatorWrk);
                cmd.Parameters.AddWithValue("@JOBID_Status", jobidStatus);
                cmd.Parameters.AddWithValue("@JOB_Status", jobStatusText);
                cmd.Parameters.AddWithValue("@MasterStatusCode", masterCode);
                cmd.Parameters.AddWithValue("@Incharge_Approval", approvalStatus);
                cmd.Parameters.AddWithValue("@Incharge_Remarks", remarks);
                cmd.Parameters.AddWithValue("@Incharge_ApprovalDate", asOf.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@BillingType", billingType);
                cmd.Parameters.AddWithValue("@BillingCode", billingCode);
                cmd.Parameters.AddWithValue("@EligibleJobStatus", JobStatusConstants.StatusOutPunchDone);
                cmd.Parameters.AddWithValue("@EligibleEntryExit", JobStatusConstants.EntryExitExit);
                cmd.Parameters.AddWithValue("@EligibleMasterCode", JobStatusConstants.CodeClosed);
                cmd.Parameters.AddWithValue("@PendingStatus", "Pending");
                cmd.Parameters.AddWithValue("@ReturnedStatus", "Returned");
                cmd.Parameters.AddWithValue("@BlockedStatus", "Blocked");
                cmd.Parameters.AddWithValue("@ThresholdHours", thresholdHours);
                cmd.Parameters.AddWithValue("@AsOf", asOf);
                return cmd.ExecuteNonQuery();
            }
        }

        private int CountLiveAttendance(SqlTransaction transaction, string jobid)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM tbl_attendance WHERE JOBID=@JOBID AND DeleteStatus=0", transaction.Connection, transaction))
            {
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private int ExecuteAttendanceApproval(SqlTransaction transaction, string jobid, string approvalStatus)
        {
            string cmdString = @"UPDATE tbl_attendance
                             SET SiteIncharge_Approval = @SiteIncharge_Approval,
                                 Approval_Date = GETDATE(),
                                 AttendanceStatus = 'Present'
                             WHERE JOBID = @JOBID AND DeleteStatus = 0";
            using (SqlCommand cmd = new SqlCommand(cmdString, transaction.Connection, transaction))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                cmd.Parameters.AddWithValue("@SiteIncharge_Approval", approvalStatus);
                return cmd.ExecuteNonQuery();
            }
        }

        private void ShowNotice(string title, string body)
        {
            string safeTitle = (title ?? "").Replace("'", "");
            string safeBody = (body ?? "").Replace("'", "").Replace("\r", " ").Replace("\n", " ");
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + safeTitle + "', '" + safeBody + "');", true);
        }

        private Boolean Update_AttendanceTableStatus(string jobid, string status)
        {
            Boolean flag = false;
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // REVISED SQL: Update approval fields AND overwrite AttendanceStatus to 'Present'
                string CmdString = @"UPDATE tbl_attendance 
                             SET SiteIncharge_Approval = @SiteIncharge_Approval, 
                                 Approval_Date = GETDATE(),
                                 AttendanceStatus = 'Present'
                             WHERE JOBID = @JOBID AND DeleteStatus = 0";

                using (SqlCommand cmd = new SqlCommand(CmdString, dbcl.Conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    cmd.Parameters.AddWithValue("@SiteIncharge_Approval", status);
                    cmd.ExecuteNonQuery();
                }

                flag = true;
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            finally
            {
                dbcl.DisconnectDb();
            }
            return flag;
        }

        private Boolean Update_AttendanceTableStatus_OLD(string jobid, string status, string attenstatus, string code)
        {
            Boolean flag = false;
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_attendance set SiteIncharge_Approval=@SiteIncharge_Approval, Approval_Date=@Approval_Date, AttendanceStatus=@AttendanceStatus where JOBID=@JOBID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                cmd.Parameters.AddWithValue("@SiteIncharge_Approval", status);
                cmd.Parameters.AddWithValue("@Approval_Date", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@AttendanceStatus", attenstatus);
                //cmd.Parameters.AddWithValue("@AttendanceCode", code);
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                flag = true;
                string title = "Notifications :";
                string body = "Data has been UPDATED !!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            return flag;
        }

        private void Update_JOBTableStatus(string jobid, string jobidstatus, string jobstatus, string mastercode,string approvalstatus, string remarks)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_jobs set JOBID_Status=@JOBID_Status, JOB_Status=@JOB_Status, MasterStatusCode=@MasterStatusCode , Incharge_Approval=@Incharge_Approval, Incharge_Remarks=@Incharge_Remarks, Incharge_ApprovalDate=@Incharge_ApprovalDate, BillingType=@BillingType , BillingCode=@BillingCode where JOBID=@JOBID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                cmd.Parameters.AddWithValue("@JOBID_Status", jobidstatus);
                cmd.Parameters.AddWithValue("@JOB_Status", jobstatus);
                cmd.Parameters.AddWithValue("@MasterStatusCode", mastercode);
                cmd.Parameters.AddWithValue("@Incharge_Approval", approvalstatus);
                cmd.Parameters.AddWithValue("@Incharge_Remarks", remarks);
                cmd.Parameters.AddWithValue("@Incharge_ApprovalDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@BillingType", DDL_BillingType.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@BillingCode", DDL_BillingType.SelectedValue.ToString());
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                string title = "Notifications :";
                string body = "Respective JOBID has been" + approvalstatus + "!!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("view_jobsforapproval.aspx");

            //object refUrl = ViewState["RefUrl"];
            //if (refUrl != null)
            //    Response.Redirect((string)refUrl);
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            Label JOBID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_JOBID");
            string jobid = JOBID.Text.ToString();

            Label filename = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Name");
            string file = filename.Text.ToString();

            try
            {


                Int32 count = CC.Find_PermitUploadCount(jobid);
                Int32 newcount = 0;
                if (count == 1)
                {
                    newcount = 0;
                    UpdatePermitZeroCount(jobid);

                    DeletefromFolder(file);

                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    string cmdString = "delete from tbl_jobspermit where Id=@Id and JOBID=@JOBID";
                    SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.Parameters.Add(new SqlParameter("@JOBID", jobid));
                    cmd.ExecuteNonQuery();
                    dbcl.Conn.Close();

                    string title = "Notifications :";
                    string body = "Attachment Deleted Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else if (count >= 1)
                {
                    newcount = count - 1;
                    UpdatePermitCount(jobid, newcount);

                    DeletefromFolder(file);

                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    string cmdString = "delete from tbl_jobspermit where Id=@Id and JOBID=@JOBID";
                    SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.Parameters.Add(new SqlParameter("@JOBID", jobid));
                    cmd.ExecuteNonQuery();
                    dbcl.Conn.Close();

                    string title = "Notifications :";
                    string body = "Attachment Deleted Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    string cmdString = "delete from tbl_jobspermit where Id=@Id and JOBID=@JOBID";
                    SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.Parameters.Add(new SqlParameter("@JOBID", jobid));
                    cmd.ExecuteNonQuery();
                    dbcl.Conn.Close();

                    string title = "Notifications :";
                    string body = "Attachment Deleted Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }

                string ddljobid = jobid;
                Bind_JOBIDDetails(ddljobid);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                //throw;
            }
        }

        private void DeletefromFolder(string authorsFile)
        {
            //string authorsFile = "Authors.txt";

            try
            {
                // Check if file exists with its full path
                // Check if file exists with its full path
                if (File.Exists(Path.Combine(rootFolder, authorsFile)))
                {
                    // If file found, delete it
                    File.Delete(Path.Combine(rootFolder, authorsFile));
                    //Console.WriteLine("File deleted.");
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Data Row Deleted, NO Hard File Found...!!";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
            }
            catch (IOException ioExp)
            {
                string title = "Notifications :";
                string body = ioExp.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void UpdatePermitCount(string jobid, Int32 newcount)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbcl.Conn;
            string CmdString = "UPDATE tbl_jobs set FileCount=@FileCount, PermitDeleteDate=@PermitDeleteDate, PermitDeletedByName=@PermitDeletedByName , PermitDeletedByWrk=@PermitDeletedByWrk where JOBID=@JOBID";
            cmd.CommandText = CmdString;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@JOBID", jobid);
            cmd.Parameters.AddWithValue("@FileCount", newcount);
            cmd.Parameters.AddWithValue("@PermitDeleteDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
            cmd.Parameters.AddWithValue("@PermitDeletedByName", Session["USERNAME"].ToString());
            cmd.Parameters.AddWithValue("@PermitDeletedByWrk", Session["WORKMAN"].ToString());
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        private void UpdatePermitZeroCount(string jobid)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbcl.Conn;
            string CmdString = "UPDATE tbl_jobs set UploadType=@UploadType,JOB_Status=@JOB_Status, FileCount=@FileCount,FinalUpldStatus=@FinalUpldStatus, PermitUpload=@PermitUpload, PermitUploadDate=@PermitUploadDate, PermitDeleteDate=@PermitDeleteDate, MasterStatusCode=@MasterStatusCode, PermitDeletedByName=@PermitDeletedByName, PermitDeletedByWrk=@PermitDeletedByWrk where JOBID=@JOBID";
            cmd.CommandText = CmdString;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@JOBID", jobid);
            cmd.Parameters.AddWithValue("@UploadType", "");
            cmd.Parameters.AddWithValue("@JOB_Status", "Created");
            cmd.Parameters.AddWithValue("@FinalUpldStatus", "No");
            cmd.Parameters.AddWithValue("@FileCount", "0");
            cmd.Parameters.AddWithValue("@PermitUpload", "No");
            cmd.Parameters.AddWithValue("@MasterStatusCode", "1");  // JOBID created, Permit Uploaded, Can Proceed to Entry Page
            cmd.Parameters.AddWithValue("@PermitUploadDate", "");
            cmd.Parameters.AddWithValue("@PermitDeleteDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
            cmd.Parameters.AddWithValue("@PermitDeletedByName", Session["USERNAME"].ToString());
            cmd.Parameters.AddWithValue("@PermitDeletedByWrk", Session["WORKMAN"].ToString());
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            string jobid = txt_jobid.Text.ToString();
            if (btn_update.Text == "Update")
            {
                txt_permitno.ReadOnly = false;
                txt_jobtitle.ReadOnly = false;
                txt_jobshift.ReadOnly = false;

                worksite_row1.Visible = true;
                worksite_row2.Visible = true;

                approver_row1.Visible = true;
                approver_row2.Visible = true;

                btn_update.Text = "Save Changes";
                btn_cancelupdate.Visible = true;
                btn_cancelupdate.Enabled = true;

                txt_permitno.Focus();
            }
            else
            {
                btn_update.Text = "Update";
                btn_cancelupdate.Visible = false;
                btn_cancelupdate.Enabled = false;

                UpdateBasicJOBData(jobid);
                Bind_JOBIDDetails(jobid);

                txt_permitno.ReadOnly = true;
                txt_jobtitle.ReadOnly = true;
                txt_jobshift.ReadOnly = true;

                worksite_row1.Visible = false;
                worksite_row2.Visible = false;

                approver_row1.Visible = false;
                approver_row2.Visible = false;

                btn_update.Text = "Update";
                btn_cancelupdate.Visible = false;
                btn_cancelupdate.Enabled = false;

                string title = "Notifications :";
                string body = "Data has been UPDATED !!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void UpdateBasicJOBData(string jobid)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_jobs set JOB_PermitNo=@JOB_PermitNo, JOB_Title=@JOB_Title, JOB_Shift=@JOB_Shift, JOB_Site=@JOB_Site, JOB_SiteCode=@JOB_SiteCode, JOB_InchargeWrk=@JOB_InchargeWrk, JOB_InchargeName=@JOB_InchargeName where JOBID=@JOBID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                cmd.Parameters.AddWithValue("@JOB_PermitNo", txt_permitno.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_Title", txt_jobtitle.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_Shift", txt_jobshift.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_Site", DDL_Worksite.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_SiteCode", DDL_Worksite.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@JOB_InchargeWrk", DDL_Approver.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@JOB_InchargeName", DDL_Approver.SelectedItem.Text.ToString());

                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void btn_cancelupdate_Click(object sender, EventArgs e)
        {
            txt_permitno.ReadOnly = true;
            txt_jobtitle.ReadOnly = true;
            txt_jobshift.ReadOnly = true;

            btn_update.Text = "Update";
            btn_cancelupdate.Visible = false;
            btn_cancelupdate.Enabled = false;
        }

        protected void btn_back_Click(object sender, EventArgs e)
        {
            Response.Redirect("view_jobsforapproval.aspx");

            //object refUrl = ViewState["RefUrl"];
            //if (refUrl != null)
            //    Response.Redirect((string)refUrl);
        }

        protected void DDL_Worksite_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CmdString = "select Employee_Name, Employee_Workman from tlb_atsworksiteIncharges where DB_Code=@DBCode order by Id";
            Bind_Approver(CmdString, new SqlParameter("@DBCode", DDL_Worksite.SelectedValue.ToString()));
        }

        protected void btn_attachmanpower_Click(object sender, EventArgs e)
        {
            Response.Redirect("attach_manpower.aspx?JOBID=" + txt_jobid.Text.ToString() + "");
        }

    }
}