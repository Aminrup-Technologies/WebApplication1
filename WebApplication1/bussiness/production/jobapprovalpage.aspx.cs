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

        private void Bind_Approver(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
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
            return elapsedTime.TotalHours >= thresholdHours;
        }

        public string GetElapsedTime(DateTime startDate)
        {
            TimeSpan elapsedTime = DateTime.Now - startDate;
            return string.Format("{0} days, {1} hours, {2} minutes, {3} seconds",
                                  elapsedTime.Days, elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
        }

        private void Bind_JOBIDDetails(string jobid)
        {
            string CmdString2 = "select * from tbl_jobspermit where JOBID='" + jobid + "' order by Id desc";
            BindGrid(CmdString2);

            string CmdString3 = "select * from tbl_attendance where JOBID='" + jobid + "' order by Id desc";
            BindGrid2(CmdString3);
            try
            {
                string query = "select * from tbl_jobs where JOBID=@JOBID";
                SqlParameter[] pram = {
                                          new SqlParameter("@JOBID",jobid),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    DateTime createdDate = Convert.ToDateTime(dt.Rows[0]["CreatedDate"]);
                    txt_jobdate.Text = createdDate.ToString("dd-MM-yyyy");
                    bool isBlocked = Convert.ToBoolean(dt.Rows[0]["IsBlocked"]);
                    bool isElapsed = Is72HoursElapsed(createdDate);

                    //lbl_jobcreatorname.Text = dt.Rows[0]["Creator_Name"].ToString();
                    //lbl_creatorwrk.Text = dt.Rows[0]["Creator_Workman"].ToString();
                    //lbl_creatorregion.Text = dt.Rows[0]["Creator_Region"].ToString();
                    //lbl_creatorcompany.Text = dt.Rows[0]["Creator_Company"].ToString();
                    //lbl_crtrsitename.Text = dt.Rows[0]["Creator_Site"].ToString();
                    //lbl_crtrsitecode.Text = dt.Rows[0]["Creator_SiteCode"].ToString();
                    txt_workorderno.Text = dt.Rows[0]["WorkOrderNo"].ToString();
                    txt_jobid.Text = dt.Rows[0]["JOBID"].ToString();
                    lbl_jobidsstatus.Text = dt.Rows[0]["JOBID_Status"].ToString();
                    //lbl_jobrgn.Text = dt.Rows[0]["JOB_Region"].ToString();
                    //lbl_jobcompay.Text = dt.Rows[0]["JOB_Company"].ToString();
                    txt_worksitename.Text = dt.Rows[0]["JOB_Site"].ToString();
                    lbl_worksitedbcode.Text = dt.Rows[0]["JOB_SiteCode"].ToString();

                    DDL_Worksite.SelectedValue = lbl_worksitedbcode.Text.ToString();

                    string CmdString = "select Employee_Name, Employee_Workman from tlb_atsworksiteIncharges where DB_Code='" + lbl_worksitedbcode.Text.ToString() + "' order by Id";
                    Bind_Approver(CmdString);

                    DDL_Approver.SelectedValue = lbl_worksitedbcode.Text.ToString();

                    lbl_inchargewrk.Text = dt.Rows[0]["JOB_InchargeWrk"].ToString();

                    txt_inchargename.Text = dt.Rows[0]["JOB_InchargeName"].ToString();
                    txt_jobdept.Text = dt.Rows[0]["JOB_Dept"].ToString();
                    txt_jobloc.Text = dt.Rows[0]["JOB_Location"].ToString();
                    txt_jobshift.Text = dt.Rows[0]["JOB_Shift"].ToString();
                    txt_permitno.Text = dt.Rows[0]["JOB_PermitNo"].ToString();

                    lbl_permituploaddate.Text = dt.Rows[0]["PermitUploadDate"].ToString();
                    lbl_filecount.Text = dt.Rows[0]["FileCount"].ToString();
                    lbl_permitdeleteddate.Text = dt.Rows[0]["PermitDeleteDate"].ToString();
                    lbl_permitdeletedby.Text = dt.Rows[0]["PermitDeletedByName"].ToString();

                    string uploadstatus = dt.Rows[0]["FinalUpldStatus"].ToString();

                    if (uploadstatus == "Yes")
                    {
                        lbl_prmtupldstatus.Text = "Uploaded";
                        lbl_prmtupldstatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lbl_prmtupldstatus.Text = "Pending";
                        lbl_prmtupldstatus.ForeColor = Color.Red;
                    }


                    string approvalstatus = dt.Rows[0]["Incharge_Approval"].ToString();
                    if (approvalstatus == "Approved")
                    {
                        lbl_approvalstatus.Text = "Approved";
                        lbl_approvalstatus.ForeColor = Color.Green;

                        btn_approve.Visible = true;
                        btn_approve.Enabled = false;
                        btn_approve.Text = "Approved";
                        btn_reject.Visible = false;

                    }
                    else if (approvalstatus == "Rejected")
                    {
                        lbl_approvalstatus.Text = "Rejected";
                        lbl_approvalstatus.ForeColor = Color.Green;

                        btn_approve.Visible = false;
                        btn_reject.Visible = true;
                        btn_reject.Enabled = false;
                        btn_reject.Text = "Rejected";

                    }
                    else
                    {
                        if (isElapsed || isBlocked)
                        {
                            btn_approve.Enabled = false;
                            btn_reject.Enabled = false;
                            btn_update.Enabled = false;
                            btn_update.Visible = false;
                            string title = "Notifications :";
                            string body = "72 hours have elapsed since the job creation. Elapsed Time: " + GetElapsedTime(createdDate);
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                        }
                        else
                        {
                            btn_approve.Enabled = true;
                            btn_reject.Enabled = true;
                            btn_update.Enabled = true;
                            btn_update.Visible = true;
                        }

                        lbl_approvalstatus.Text = "Pending";
                        lbl_approvalstatus.ForeColor = Color.Red;

                        //btn_update.Enabled = true;
                        //btn_update.Visible = true;
                        //btn_approve.Enabled = true;
                        //btn_reject.Enabled = true;
                    }

                    string billingtype = dt.Rows[0]["BillingCode"].ToString();
                    DDL_BillingType.SelectedValue = billingtype;

                    string attencode = dt.Rows[0]["AttendanceCode"].ToString();
                    DDL_AttenCode.SelectedValue = attencode;

                    txt_jobtitle.Text = dt.Rows[0]["JOB_Title"].ToString();
                    //txt_approverrmrks.Text = dt.Rows[0]["JOB_Title"].ToString();
                    txt_approverrmrks.Text = "N/A";

                    //string CmdString2 = "select * from tbl_jobspermit where JOBID='" + jobid + "' order by Id desc";
                    //BindGrid(CmdString2);

                    //string CmdString3 = "select * from tbl_attendance where JOBID='" + jobid + "' order by Id desc";
                    //BindGrid2(CmdString3);
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
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
            string CmdString3 = "select * from tbl_attendance where JOBID='" + jobid + "' order by Id desc";
            BindGrid2(CmdString3);
        }

        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView2.EditIndex = -1;

            string jobid = txt_jobid.Text.ToString();
            string CmdString3 = "select * from tbl_attendance where JOBID='" + jobid + "' order by Id desc";
            BindGrid2(CmdString3);
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

            string CmdString3 = "select * from tbl_attendance where JOBID='" + jobid + "' order by Id desc";
            BindGrid2(CmdString3);

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
                string cmdString = "delete from tbl_attendance where Id='" + id + "' and JOBID='" + dbjobid + "'  ";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
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
            string CmdString3 = "select * from tbl_attendance where JOBID='" + jobid + "' order by Id desc";
            BindGrid2(CmdString3);
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



        protected void btn_approve_Click(object sender, EventArgs e)
        {
            try
            {
                string jobid = txt_jobid.Text.ToString();
                string remarks = txt_remarks.Text.ToString();
                string attendancecode = DDL_AttenCode.SelectedValue.ToString();

                if (Update_AttendanceTableStatus(jobid, "Approved", "Present", attendancecode) == true)
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

        protected void btn_reject_Click(object sender, EventArgs e)
        {
            try
            {
                string jobid = txt_jobid.Text.ToString();
                string remarks = txt_remarks.Text.ToString();

                if (Update_AttendanceTableStatus(jobid, "Rejected", "Absent", "Ab") == true)
                {
                    Update_JOBTableStatus(jobid, "Blocked", "Rejected by Approver", "6", "Rejected", remarks);
                    txt_remarks.ReadOnly = true;
                    btn_approve.Visible = false;
                    btn_reject.Enabled = false;
                    btn_reject.Text = "Rejected";

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

        private Boolean Update_AttendanceTableStatus(string jobid, string status, string attenstatus, string code)
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
                    string cmdString = "delete from tbl_jobspermit where Id='" + id + "' and JOBID= '" + jobid + "' ";
                    SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
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
                    string cmdString = "delete from tbl_jobspermit where Id='" + id + "' and JOBID= '" + jobid + "' ";
                    SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
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
                    string cmdString = "delete from tbl_jobspermit where Id='" + id + "' and JOBID= '" + jobid + "' ";
                    SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
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
            string CmdString = "select Employee_Name, Employee_Workman from tlb_atsworksiteIncharges where DB_Code='" + DDL_Worksite.SelectedValue.ToString() + "' order by Id";
            Bind_Approver(CmdString);
        }

        protected void btn_attachmanpower_Click(object sender, EventArgs e)
        {
            Response.Redirect("attach_manpower.aspx?JOBID=" + txt_jobid.Text.ToString() + "");
        }
    }
}