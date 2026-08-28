using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class view_jobdetails_v2 : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
        DataTable dt = new DataTable();

        public static string jobid = string.Empty;
        public static string dbid = string.Empty;
        public static string supv = string.Empty;
        static string message = "";

        static readonly string rootFolder = @"C:\atswork.in\wwwroot\erp_images\Permits";
        private static readonly HttpClient httpClient = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null)
                {
                    Response.Redirect("~/login.aspx", false);
                    return;
                }

                message = "Today's JOB Details,\r\n\r\n";

                jobid = Request.QueryString["JOBID"];
                dbid = Request.QueryString["dbid"];
                supv = Request.QueryString["supv"];

                Bind_JOBIDDetails(jobid, dbid, supv);
                Checker();
            }
        }

        private void ShowNotification(string title, string message, string type)
        {
            if (string.IsNullOrEmpty(message)) message = "An unknown error occurred.";

            string cleanMessage = message.Replace("'", "\\'")
                                         .Replace("\"", "\\\"")
                                         .Replace("\r", "")
                                         .Replace("\n", "<br/>");

            string script = $"ShowPopup('{title}', '{cleanMessage}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", script, true);
        }

        private void Checker()
        {
            string workman = Session["WORKMAN"].ToString();
            if (workman == "A84" || workman == "K208" || workman == "N21" || workman == "J8")
            {
                attachmanpowerrow.Visible = true;
            }
            else
            {
                attachmanpowerrow.Visible = false;
            }
        }

        private string DateBinder(string date)
        {
            DateTime oDate = Convert.ToDateTime(date);
            return oDate.ToString("dd-MM-yyyy");
        }

        private void Bind_JOBIDDetails(string jobid, string dbid, string supv)
        {
            try
            {
                string query = "select TOP 10 Id, CreatedDate, JOB_Shift, Creator_Workman, Creator_Name, WorkOrderNo, JOB_PermitNo, JOBID, JOB_Title, JOBID_Status, JOB_Site, JOB_SiteCode, JOB_InchargeWrk, JOB_InchargeName, JOB_Dept, JOB_Location, PermitDeleteDate, PermitDeletedByName, FinalUpldStatus, Incharge_Approval, EntryExit from tbl_jobs where JOBID=@JOBID and Id=@Id and Creator_Workman=@Creator_Workman";
                SqlParameter[] pram = {
                    new SqlParameter("@JOBID",jobid),
                    new SqlParameter("@Id", dbid),
                    new SqlParameter("@Creator_Workman", supv)
                };

                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    string jobdate = dt.Rows[0]["CreatedDate"].ToString();
                    txt_jobdate.Text = DateBinder(jobdate);

                    DateTime dt1 = DateTime.Parse(jobdate);
                    txt_jobday.Text = dt1.DayOfWeek.ToString();
                    txt_jobshift.Text = dt.Rows[0]["JOB_Shift"].ToString();

                    lbl_creatorwrk.Text = dt.Rows[0]["Creator_Workman"].ToString();
                    txt_jobsupv.Text = dt.Rows[0]["Creator_Name"].ToString();

                    txt_workorderno.Text = dt.Rows[0]["WorkOrderNo"].ToString();
                    txt_permitno.Text = dt.Rows[0]["JOB_PermitNo"].ToString();

                    txt_jobid.Text = dt.Rows[0]["JOBID"].ToString();
                    txt_jobtitle.Text = dt.Rows[0]["JOB_Title"].ToString();

                    string jobidstatus = dt.Rows[0]["JOBID_Status"].ToString();
                    txt_jobid.ForeColor = jobidstatus == "Active" ? Color.Green : Color.Blue;

                    txt_worksitename.Text = dt.Rows[0]["JOB_Site"].ToString() + " [" + dt.Rows[0]["JOB_SiteCode"].ToString() + "]";
                    lbl_worksitedbcode.Text = dt.Rows[0]["JOB_SiteCode"].ToString();

                    lbl_inchargewrk.Text = dt.Rows[0]["JOB_InchargeWrk"].ToString();
                    txt_inchargename.Text = dt.Rows[0]["JOB_InchargeName"].ToString();

                    txt_jobdept.Text = dt.Rows[0]["JOB_Dept"].ToString();
                    txt_jobloc.Text = dt.Rows[0]["JOB_Location"].ToString();

                    string uploadstatus = dt.Rows[0]["FinalUpldStatus"].ToString();
                    if (uploadstatus == "Yes")
                    {
                        GridView1.Columns[5].Visible = true; // Restored original index mapping
                        txt_permitno.ForeColor = Color.Green;
                    }
                    else
                    {
                        GridView1.Columns[5].Visible = false; // Restored original index mapping
                        txt_permitno.ForeColor = Color.Red;
                    }

                    string approvalstatus = dt.Rows[0]["Incharge_Approval"].ToString();
                    string entryexitstatus = dt.Rows[0]["EntryExit"].ToString();

                    if (approvalstatus == "Approved")
                    {
                        txt_inchargename.ForeColor = Color.Green;
                        btn_update.Enabled = false;
                        btn_update.Text = "UPDATE NOT Allowed";
                        GridView1.Columns[5].Visible = true;
                        GridView2.Columns[13].Visible = false; // Restored exact original index 
                    }
                    else if (approvalstatus == "Pending")
                    {
                        if (entryexitstatus == "Entry")
                        {
                            GridView1.Columns[5].Visible = true;
                            GridView2.Columns[13].Visible = true;
                        }
                        else
                        {
                            GridView2.Columns[13].Visible = true;
                            GridView1.Columns[5].Visible = true;
                            btn_update.Enabled = true;
                        }
                        txt_inchargename.ForeColor = Color.Red;
                    }
                    else if (approvalstatus == "Rejected")
                    {
                        ResendApp_Div.Visible = true;
                        btn_resendapp.Enabled = true;
                        ShowNotification("Notifications", "JOBID Rejected by Approver", "warning");
                    }

                    // Enforce Soft Delete Query Filters
                    string CmdString2 = "select TOP 100 Id, JOBID, Name, TimeStamp from tbl_jobspermit where JOBID='" + jobid + "' and Submitter_Wrk='" + supv + "' AND ISNULL(DeleteStatus, 0) = 0 order by Id desc";
                    BindGrid(CmdString2);

                    string CmdString3 = "select TOP 100 Id, JOBID, JOB_Region, CreatedDate, EmployeeWrk, EmployeeName, EmpDesignation, WourkHours, Inpunch_Time, Outpunch_Time, LunchFactor, ProvidedOT, AttendanceStatus, AttendanceCode from tbl_attendance where JOBID='" + jobid + "' and Creator_Workman='" + supv + "' and DeleteStatus=0 order by Id desc";
                    BindGrid2(CmdString3);
                }
            }
            catch (Exception ex)
            {
                ShowNotification("System Error", "Error-251: " + ex.Message, "error");
            }
        }

        private void BindGrid(string cmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            dbcl.Conn.Close();
        }

        private void BindGrid2(string cmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            GridView2.DataSource = dt;
            GridView2.DataBind();
            dbcl.Conn.Close();
        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            string jobid = txt_jobid.Text.ToString();
            if (btn_update.Text == "Edit Data")
            {
                txt_permitno.ReadOnly = false;
                txt_jobtitle.ReadOnly = false;
                txt_jobshift.ReadOnly = false;

                btn_update.Text = "Save Changes";
                btn_cancel.Visible = true;
                btn_cancel.Enabled = true;
                txt_permitno.Focus();
            }
            else
            {
                UpdateBasicJOBData(jobid);
                Bind_JOBIDDetails(jobid, dbid, supv);

                txt_permitno.ReadOnly = true;
                txt_jobtitle.ReadOnly = true;
                txt_jobshift.ReadOnly = true;

                btn_update.Text = "Edit Data";
                btn_cancel.Visible = false;
                btn_cancel.Enabled = false;

                ShowNotification("Success", "Data has been UPDATED !!", "success");
            }
        }

        private void UpdateBasicJOBData(string jobid)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand cmd = new SqlCommand("UPDATE tbl_jobs set JOB_PermitNo=@JOB_PermitNo, JOB_Title=@JOB_Title, JOB_Shift=@JOB_Shift where JOBID=@JOBID", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    cmd.Parameters.AddWithValue("@JOB_PermitNo", txt_permitno.Text.ToString());
                    cmd.Parameters.AddWithValue("@JOB_Title", txt_jobtitle.Text.ToString());
                    cmd.Parameters.AddWithValue("@JOB_Shift", txt_jobshift.Text.ToString());
                    cmd.ExecuteNonQuery();
                }

                JobWorkflowLogger.LogAction(jobid, "JOB DETAILS EDITED", Session["WORKMAN"] != null ? Session["WORKMAN"].ToString() : "Unknown",
                    $"PermitNo='{txt_permitno.Text}', Title='{txt_jobtitle.Text}', Shift='{txt_jobshift.Text}'.");
            }
            catch (Exception) { ShowNotification("Update Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            txt_permitno.ReadOnly = true;
            txt_jobtitle.ReadOnly = true;
            txt_jobshift.ReadOnly = true;
            btn_update.Text = "Edit Data";
            btn_cancel.Visible = false;
            btn_cancel.Enabled = false;
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = ((Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id")).Text;
            string permitJobId = ((Label)GridView1.Rows[e.RowIndex].FindControl("lbl_JOBID")).Text;
            string file = ((Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Name")).Text;
            string deletedBy = Session["WORKMAN"] != null ? Session["WORKMAN"].ToString() : "Unknown";

            try
            {
                Int32 count = CC.Find_PermitUploadCount(permitJobId);
                if (count == 1) UpdatePermitZeroCount(permitJobId);
                else if (count >= 1) UpdatePermitCount(permitJobId, count - 1);

                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "UPDATE tbl_jobspermit SET DeleteStatus = 1, DeletedOn = GETDATE(), DeletedBy = @DeletedBy WHERE Id=@Id AND JOBID=@JOBID";
                using (SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@JOBID", permitJobId);
                    cmd.Parameters.AddWithValue("@DeletedBy", deletedBy);
                    cmd.ExecuteNonQuery();
                }

                JobWorkflowLogger.LogAction(permitJobId, "PERMIT DOCUMENT DELETED", deletedBy,
                    $"Permit file '{file}' (Id={id}) soft-deleted from JOB {permitJobId}.");

                ShowNotification("Deleted", "Attachment Soft-Deleted Successfully", "success");
                Bind_JOBIDDetails(jobid, dbid, supv);
            }
            catch (Exception) { ShowNotification("Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = ((Label)GridView2.Rows[e.RowIndex].FindControl("lbl_Id")).Text;
            string manpowerJobId = ((Label)GridView2.Rows[e.RowIndex].FindControl("lbl_JOBID")).Text;
            string deletedBy = Session["WORKMAN"] != null ? Session["WORKMAN"].ToString() : "Unknown";

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "UPDATE tbl_attendance SET DeleteStatus = 1, DeletedOn = GETDATE(), DeletedBy = @DeletedBy WHERE Id=@Id AND JOBID=@JOBID";
                using (SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@JOBID", manpowerJobId);
                    cmd.Parameters.AddWithValue("@DeletedBy", deletedBy);
                    cmd.ExecuteNonQuery();
                }

                JobWorkflowLogger.LogAction(manpowerJobId, "ATTENDANCE DELETED", deletedBy,
                    $"Attendance record Id={id} soft-deleted for JOB {manpowerJobId}.");

                ShowNotification("Deleted", "Manpower Soft-Deleted Successfully", "success");
                string CmdString3 = "select * from tbl_attendance where JOBID='" + txt_jobid.Text + "' and DeleteStatus=0 order by Id desc";
                BindGrid2(CmdString3);
            }
            catch (Exception) { ShowNotification("Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView2.EditIndex = e.NewEditIndex;
            BindGrid2("select * from tbl_attendance where JOBID='" + txt_jobid.Text + "' and DeleteStatus=0 order by Id desc");
        }

        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView2.EditIndex = -1;
            BindGrid2("select * from tbl_attendance where JOBID='" + txt_jobid.Text + "' and DeleteStatus=0 order by Id desc");
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
            Int32 new_pot = Convert.ToInt32(new_ot);

            DropDownList AttendanceStatus = (DropDownList)GridView2.Rows[e.RowIndex].FindControl("DDL_AttendanceStatus");
            string ddl_newattensttaus = AttendanceStatus.SelectedItem.Text.ToString();

            DropDownList AttendanceCode = (DropDownList)GridView2.Rows[e.RowIndex].FindControl("DDL_AttendanceCode");
            string ddl_newattencode = AttendanceCode.SelectedValue.ToString();

            Int32 workdmins = 0;
            decimal workedhours = .0m;
            dbcl.FindEmployeeWorkedTime(convtimein, convtimeout, ref workdmins, ref workedhours);
            decimal emp_calOThrs = .0m;

            if (region == "NINL")
            {
                dbcl.CalculateOvertimeRev(emp_wrkmnis, workdmins, lunchyesno, ref emp_calOThrs);
            }
            else
            {
                dbcl.CalculateOvertime(emp_wrkmnis, workdmins, lunchyesno, ref emp_calOThrs);
            }

            UpdateDetails(id, jobid, workmansl, convtimein, convtimeout, lunchyesno, workdmins,
                workedhours, emp_calOThrs, new_pot, ddl_newattensttaus, ddl_newattencode);

            GridView2.EditIndex = -1;
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        private void UpdateDetails(string id, string jobid, string empwrk, string intime, string outime, string lunchyesno, Int32 workdmins, decimal workedhours, decimal emp_calOThrs, Int32 new_pot, string ddl_newattensttaus, string ddl_newattencode)
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
                cmd.Parameters.AddWithValue("@LastModified", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@ModifiedByWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@ModifiedByName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@AttendanceStatus", ddl_newattensttaus);
                cmd.Parameters.AddWithValue("@AttendanceCode", ddl_newattencode);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbcl.DisconnectDb();

                JobWorkflowLogger.LogAction(jobid, "ATTENDANCE EDITED", Session["WORKMAN"] != null ? Session["WORKMAN"].ToString() : "Unknown",
                    $"Employee {empwrk}: In={convtimein}, Out={convtimeout}, Lunch={lunchyesno}, WorkedHrs={workedhours}, CalcOT={emp_calOThrs}, ProvidedOT={new_pot}, Status={ddl_newattensttaus}, Code={ddl_newattencode}.");

                ShowNotification("Success", "Data has been UPDATED !!", "success");
            }
            catch (Exception ex)
            {
                ShowNotification("Error", ex.Message, "error");
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && GridView2.EditIndex == e.Row.RowIndex)
            {
                var DDL_AttendanceStatus = e.Row.FindControl("DDL_AttendanceStatus") as DropDownList;
                if (DDL_AttendanceStatus != null)
                {
                    var dt1 = new DataTable();
                    string cnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString();
                    using (var con = new SqlConnection(cnnString1))
                    {
                        con.Open();
                        var cmd1 = new SqlCommand("Select DISTINCT Status from tlb_attendancecodes where Supervisor='Yes'", con);
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
                        var cmd2 = new SqlCommand("Select Status_Name,Status_Code from tlb_attendancecodes where Supervisor='Yes' order by slno", con);
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

        protected void DownloadFile(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse((sender as LinkButton).CommandArgument);
                string fileName = "";
                string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "SELECT * FROM tbl_jobspermit WHERE Id=@Id";
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.Read())
                            {
                                fileName = sdr["Name"].ToString();
                            }
                            else
                            {
                                throw new Exception("File record not found in database.");
                            }
                        }
                        con.Close();
                    }
                }

                string serverFolder = Server.MapPath(@"\erp_images\Permits\");
                string fullFilePath = Path.Combine(serverFolder, fileName);

                if (File.Exists(fullFilePath))
                {
                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
                    Response.TransmitFile(fullFilePath);
                    Response.End();
                }
                else
                {
                    ShowNotification("Not Found", "Physical file is missing from the server.", "error");
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Download Error", ex.Message, "error");
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
            dbcl.DisconnectDb();
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
            cmd.Parameters.AddWithValue("@MasterStatusCode", "1");
            cmd.Parameters.AddWithValue("@PermitUploadDate", "");
            cmd.Parameters.AddWithValue("@PermitDeleteDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
            cmd.Parameters.AddWithValue("@PermitDeletedByName", Session["USERNAME"].ToString());
            cmd.Parameters.AddWithValue("@PermitDeletedByWrk", Session["WORKMAN"].ToString());
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            dbcl.DisconnectDb();
        }

        private void UpdateJOBTable1(string jobidstatus, string jobstatus, string mastercode, string entryexitstatus)
        {
            string jobID = txt_jobid.Text.ToString();

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_jobs set JOBID_Status=@JOBID_Status, JOB_Status=@JOB_Status, MasterStatusCode=@MasterStatusCode , EntryExit=@EntryExit, Incharge_Approval='Pending' where JOBID=@JOBID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@JOBID", jobID);
                cmd.Parameters.AddWithValue("@JOBID_Status", jobidstatus);
                cmd.Parameters.AddWithValue("@JOB_Status", jobstatus);
                cmd.Parameters.AddWithValue("@MasterStatusCode", mastercode);
                cmd.Parameters.AddWithValue("@EntryExit", entryexitstatus);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                btn_resendapp.Enabled = false;
                btn_resendapp.Text = "Success";
                ShowNotification("Notifications", "JOBID Sent for Approval", "success");
            }
            catch (Exception ex)
            {
                ShowNotification("System Error", "Error 310: " + ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        protected void btn_resendapp_Click(object sender, EventArgs e)
        {
            JobWorkflowLogger.LogAction(txt_jobid.Text, "RE-SEND FOR APPROVAL", Session["WORKMAN"] != null ? Session["WORKMAN"].ToString() : "Unknown",
                "JOB reset to 'Pending' approval (status Blocked, MasterStatusCode 4 / Exit).");
            UpdateJOBTable1("Blocked", "Out-Punch Done", "4", "Exit");
        }

        protected void btn_attachmanpower_Click(object sender, EventArgs e)
        {
            JobWorkflowLogger.LogAction(txt_jobid.Text, "ATTACH MANPOWER", Session["WORKMAN"] != null ? Session["WORKMAN"].ToString() : "Unknown",
                "Navigated to manpower attachment screen.");
            Response.Redirect("attach_manpower.aspx?JOBID=" + txt_jobid.Text.ToString());
        }

        protected void btn_share_whatsapp_Click(object sender, EventArgs e)
        {
            string jobid = txt_jobid.Text.Trim();
            if (string.IsNullOrEmpty(jobid))
            {
                ShowNotification("Warning", "No active JOB ID found to share.", "warning");
                return;
            }

            Task.Run(() => TriggerShareNotifications(jobid));

            ShowNotification("Success", "Request queued! The Approver will receive WhatsApp and Email alerts shortly.", "success");
        }

        private void LogSystemEvent(string module, string status, string message, string details = "")
        {
            try
            {
                string dateFolder = DateTime.Now.ToString("yyyy-MM-dd");
                string logDirectory = Server.MapPath($"~/Logs/OutPunch/{dateFolder}/");
                if (!Directory.Exists(logDirectory)) Directory.CreateDirectory(logDirectory);

                string filePath = Path.Combine(logDirectory, "Notification_Log.txt");
                string logEntry = $"[{DateTime.Now:HH:mm:ss}] [{module}] [{status}] - {message}{(string.IsNullOrEmpty(details) ? "" : $" | Details: {details}")}{Environment.NewLine}";
                File.AppendAllText(filePath, logEntry);
            }
            catch { }
        }

        private async Task TriggerShareNotifications(string jobid)
        {
            DB_Utility_OH4Y asyncDbcl = new DB_Utility_OH4Y();
            try
            {
                asyncDbcl.Sqlconnection();
                asyncDbcl.ConnectDb();

                string jobDateStr = "", shift = "", supv = "", wo = "", permit = "", title = "", site = "", inchargeName = "", inchargeWrk = "", dept = "", loc = "";
                string qryJob = "SELECT CreatedDate, JOB_Shift, Creator_Name, Creator_Workman, WorkOrderNo, JOB_PermitNo, JOB_Title, JOB_Site, JOB_SiteCode, JOB_InchargeName, JOB_InchargeWrk, JOB_Dept, JOB_Location FROM tbl_jobs WHERE JOBID=@JOBID";

                using (SqlCommand cmd = new SqlCommand(qryJob, asyncDbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            DateTime dtJob = Convert.ToDateTime(rdr["CreatedDate"]);
                            jobDateStr = dtJob.ToString("dd-MM-yyyy") + " (" + dtJob.DayOfWeek.ToString() + ")";
                            shift = rdr["JOB_Shift"].ToString();
                            supv = rdr["Creator_Name"].ToString() + " (" + rdr["Creator_Workman"].ToString() + ")";
                            wo = rdr["WorkOrderNo"].ToString();
                            permit = rdr["JOB_PermitNo"].ToString().Trim();
                            title = rdr["JOB_Title"].ToString().Trim();
                            site = rdr["JOB_Site"].ToString() + " [" + rdr["JOB_SiteCode"].ToString() + "]";
                            inchargeName = rdr["JOB_InchargeName"].ToString() + " (" + rdr["JOB_InchargeWrk"].ToString() + ")";
                            inchargeWrk = rdr["JOB_InchargeWrk"].ToString();
                            dept = rdr["JOB_Dept"].ToString();
                            loc = rdr["JOB_Location"].ToString();
                        }
                    }
                }

                string mobileNo = "", emailAddress = "";
                using (SqlCommand cmd = new SqlCommand("SELECT MobileNo, Email FROM tbl_Employee_Mustertable WHERE WorkmanSL = @Wrk", asyncDbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Wrk", inchargeWrk);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            mobileNo = rdr["MobileNo"] != DBNull.Value ? rdr["MobileNo"].ToString().Trim() : "";
                            emailAddress = rdr["Email"] != DBNull.Value ? rdr["Email"].ToString().Trim() : "";
                        }
                    }
                }

                List<string> manpowerItems = new List<string>();
                using (SqlCommand cmd = new SqlCommand("SELECT EmployeeName, EmployeeWrk, EmpDesignation FROM tbl_attendance WHERE JOBID=@JOBID AND DeleteStatus=0 ORDER BY Id ASC", asyncDbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        int count = 1;
                        while (rdr.Read())
                        {
                            manpowerItems.Add($"{count}. {rdr["EmployeeName"]} [{rdr["EmployeeWrk"]}] - {rdr["EmpDesignation"]}");
                            count++;
                        }
                    }
                }

                asyncDbcl.DisconnectDb();

                string waManpowerList = manpowerItems.Count > 0 ? string.Join(",  ", manpowerItems) : "No Manpower Scanned.";
                string emailManpowerList = manpowerItems.Count > 0 ? string.Join("<br/>", manpowerItems) : "No Manpower Scanned.";

                if (!string.IsNullOrEmpty(mobileNo) && mobileNo.Length >= 10)
                {
                    await SendWhatsAppMsg91Async(mobileNo, inchargeName, supv, jobid, shift, jobDateStr, title, site, loc, dept, wo, permit, waManpowerList);
                }
                else
                {
                    LogSystemEvent("ShareAPI", "SKIPPED", $"No valid Mobile Number found for In-Charge {inchargeWrk}");
                }

                if (!string.IsNullOrEmpty(emailAddress) && emailAddress.Contains("@"))
                {
                    SendEmailAlert(emailAddress, inchargeName, supv, jobid, shift, jobDateStr, title, site, loc, dept, wo, permit, emailManpowerList);
                }
                else
                {
                    LogSystemEvent("ShareAPI", "SKIPPED", $"No valid Email ID found for In-Charge {inchargeWrk}");
                }
            }
            catch (Exception ex)
            {
                LogSystemEvent("ShareAPI", "CRASH", "Engine failure.", ex.Message);
            }
            finally
            {
                if (asyncDbcl.Conn != null && asyncDbcl.Conn.State == ConnectionState.Open) asyncDbcl.DisconnectDb();
            }
        }

        private async Task SendWhatsAppMsg91Async(string mobileNo, string inchargeName, string supv, string jobid, string shift, string jobDateStr, string title, string site, string loc, string dept, string wo, string permit, string manpowerList)
        {
            try
            {
                string authKey = ConfigurationManager.AppSettings["Msg91AuthKey"]?.Trim();
                string integratedNumber = ConfigurationManager.AppSettings["Msg91IntegratedNumber"]?.Trim();

                if (string.IsNullOrEmpty(authKey) || string.IsNullOrEmpty(integratedNumber)) { LogSystemEvent("WhatsApp", "SKIPPED", "Missing config keys."); return; }

                string cleanPhone = mobileNo.Replace("+", "").Replace(" ", "").Trim();
                if (!cleanPhone.StartsWith("91")) cleanPhone = "91" + cleanPhone;

                var payload = new
                {
                    integrated_number = integratedNumber,
                    content_type = "template",
                    payload = new
                    {
                        messaging_product = "whatsapp",
                        type = "template",
                        template = new
                        {
                            name = "job_daily_details",
                            language = new { code = "en", policy = "deterministic" },
                            @namespace = "af05507b_02e4_4d95_8f8c_164ce03fc2df",
                            to_and_components = new[]
                            {
                                new
                                {
                                    to = new[] { cleanPhone },
                                    components = new Dictionary<string, object>
                                    {
                                        { "body_1", new { type = "text", value = inchargeName } },
                                        { "body_2", new { type = "text", value = supv } },
                                        { "body_3", new { type = "text", value = jobid } },
                                        { "body_4", new { type = "text", value = shift } },
                                        { "body_5", new { type = "text", value = jobDateStr } },
                                        { "body_6", new { type = "text", value = title } },
                                        { "body_7", new { type = "text", value = site } },
                                        { "body_8", new { type = "text", value = loc } },
                                        { "body_9", new { type = "text", value = dept } },
                                        { "body_10", new { type = "text", value = wo } },
                                        { "body_11", new { type = "text", value = permit } },
                                        { "body_12", new { type = "text", value = manpowerList } }
                                    }
                                }
                            }
                        }
                    }
                };

                string jsonPayload = new JavaScriptSerializer().Serialize(payload).Replace("\"@namespace\"", "\"namespace\"");
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("authkey", authKey);

                HttpResponseMessage response = await httpClient.PostAsync("https://api.msg91.com/api/v5/whatsapp/whatsapp-outbound-message/bulk/", content);
                string result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode) LogSystemEvent("WhatsApp", "SUCCESS", $"Sent to {cleanPhone}.", result);
                else LogSystemEvent("WhatsApp", "API_ERROR", $"MSG91 rejected {cleanPhone}.", result);
            }
            catch (Exception ex) { LogSystemEvent("WhatsApp", "CRASH", "Exception.", ex.Message); }
        }

        private void SendEmailAlert(string emailAddress, string inchargeName, string supv, string jobid, string shift, string jobDateStr, string title, string site, string loc, string dept, string wo, string permit, string htmlManpowerList)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(emailAddress);
                mail.From = new MailAddress("it.support@aminruptechnologies.co.in", "Work-Sure 360");
                mail.Subject = $"ACTION REQUIRED: Shift Closure Approval for JOB: {jobid}";
                mail.IsBodyHtml = true;

                mail.Body = $@"
                    <div style='font-family: Arial, sans-serif; color: #333;'>
                        <h2 style='color: #E74C3C;'>Action Required: Shift Closure Pending</h2>
                        <p>Dear <b>{inchargeName}</b>,</p>
                        <p>Please be informed that the following job has been completed and OUT-Punched by <b>{supv}</b>. It is now in your queue for Final Approval to generate the Invoice Memo.</p>
                        <table style='width: 100%; border-collapse: collapse; margin-top: 15px;'>
                            <tr><td style='padding: 5px;'><b>JOB ID:</b></td><td>{jobid} ({shift} Shift)</td></tr>
                            <tr><td style='padding: 5px;'><b>Date:</b></td><td>{jobDateStr}</td></tr>
                            <tr><td style='padding: 5px;'><b>Job Title:</b></td><td>{title}</td></tr>
                            <tr><td style='padding: 5px;'><b>Worksite:</b></td><td>{site} | {loc}</td></tr>
                            <tr><td style='padding: 5px;'><b>Department:</b></td><td>{dept}</td></tr>
                            <tr><td style='padding: 5px;'><b>WO / Permit:</b></td><td>{wo} | {permit}</td></tr>
                        </table>
                        <h4 style='margin-top:20px; border-bottom: 1px solid #ddd; padding-bottom: 5px;'>Scanned Manpower</h4>
                        <p>{htmlManpowerList}</p>
                        <br/><br/>
                        <a href='https://atswork.co.in/' style='background-color:#1ABB9C; color:#fff; text-decoration:none; padding:10px 20px; border-radius:5px; font-weight:bold;'>Open ATS Portal to Approve</a>
                    </div>";

                string smtpUser = System.Configuration.ConfigurationManager.AppSettings["SmtpUser"] ?? "";
                string smtpPass = System.Configuration.ConfigurationManager.AppSettings["SmtpPass"] ?? "";

                if (string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPass))
                {
                    LogSystemEvent("Email", "SKIP", $"SMTP not configured - approval email to {emailAddress} skipped.");
                    return;
                }

                SmtpClient smtp = new SmtpClient { Host = "smtp.zoho.in", Port = 587, EnableSsl = true, Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass) };
                smtp.Send(mail);
                LogSystemEvent("Email", "SUCCESS", $"Sent to {emailAddress}");
            }
            catch (Exception) { LogSystemEvent("Email", "CRASH", $"SMTP Failed for {emailAddress}.", ex.Message); }
        }

        protected void btn_back_Click(object sender, EventArgs e)
        {
            // Do not clear the HF_Msg or state, just redirect back.
            // manage_jobid_v2 will detect the Session["Grid_Month"] and load the correct data.
            Response.Redirect("manage_jobid_v2.aspx", false);
        }
    }
}