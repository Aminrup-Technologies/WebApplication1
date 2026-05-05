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
    public partial class swap_jobdate : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
        DataTable dt = new DataTable();

        // Default folder
        static readonly string rootFolder = @"C:\atswork.in\wwwroot\erp_images\Permits";

        //static readonly string rootFolder = @"D:\OH4Y Works\OH4Y_2021\Demo\WebApplication1\WebApplication1\erp_images\Permits";
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
                    txt_jobid.Focus();

                    string CmdString5 = "select Worksite_Name, DB_Code from tlb_atsworksites order by Id";
                    BindWorkSites(CmdString5);

                    string CmdString = "select Employee_Name, Employee_Workman from tlb_atsworksiteIncharges order by Id";
                    Bind_Approver(CmdString);
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

        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (txt_jobid.Text != "")
            {
                Bind_JOBIDDetails(txt_jobid.Text.ToString());

                detailsrow1.Visible = true;
                detailsrow2.Visible = true;
                detailsrow3.Visible = true;
            }
            else
            {
                detailsrow1.Visible = false;
                detailsrow2.Visible = false;
                detailsrow3.Visible = false;
                string title = "Notifications :";
                string body = "Please enter JOBID....!!!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("homepage.aspx");
        }


        protected void btn_reset_Click(object sender, EventArgs e)
        {
            detailsrow1.Visible = false;
            detailsrow2.Visible = false;
            detailsrow3.Visible = false;
            txt_jobid.Text = "";
        }

        private void Bind_JOBIDDetails(string jobid)
        {
            try
            {
                string query = "select * from tbl_jobs where JOBID=@JOBID";
                SqlParameter[] pram = {
                                          new SqlParameter("@JOBID",jobid),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    txt_jobiddb.Text = jobid;
                    txt_jobdate.Text = dt.Rows[0]["CreatedDate"].ToString();
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
                        GridView1.Columns[5].Visible = true;
                        lbl_prmtupldstatus.Text = "Uploaded";
                        lbl_prmtupldstatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        GridView1.Columns[5].Visible = false;
                        lbl_prmtupldstatus.Text = "Pending";
                        lbl_prmtupldstatus.ForeColor = Color.Red;
                    }


                    string approvalstatus = dt.Rows[0]["Incharge_Approval"].ToString();
                    string entryexitstatus = dt.Rows[0]["EntryExit"].ToString();
                    if (approvalstatus == "Approved")
                    {
                        lbl_approvalstatus.Text = "Approved";
                        lbl_approvalstatus.ForeColor = Color.Green;
                        btn_update.Enabled = true;
                        //btn_update.Text = "Sorry!! Update NOT Allowed";
                        //GridView1.Columns[5].Visible = false;
                        //GridView2.Columns[12].Visible = false;
                    }
                    else
                    {
                        if (entryexitstatus == "Entry")
                        {
                            GridView1.Columns[5].Visible = true;
                            GridView2.Columns[12].Visible = true;
                        }
                        else
                        {
                            GridView2.Columns[12].Visible = true;
                            GridView1.Columns[5].Visible = true;
                            btn_update.Enabled = true;
                        }
                        lbl_approvalstatus.Text = "Pending";
                        lbl_approvalstatus.ForeColor = Color.Red;
                    }

                    txt_jobtitle.Text = dt.Rows[0]["JOB_Title"].ToString();
                    //txt_approverrmrks.Text = dt.Rows[0]["JOB_Title"].ToString();
                    txt_approverrmrks.Text = "N/A";

                    string CmdString2 = "select * from tbl_jobspermit where JOBID='" + jobid + "' order by Id desc";
                    BindGrid(CmdString2);

                    string CmdString3 = "select * from tbl_attendance where JOBID='" + jobid + "' order by Id desc";
                    BindGrid2(CmdString3);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                lbl_msg.ForeColor = System.Drawing.Color.Red;
                lbl_msg.Text = "Error: " + ex.Message.ToString();
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
        protected void DownloadFile(object sender, EventArgs e)
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
                        cmd.CommandText = "select * from tbl_jobspermit where Id=@Id";
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
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                lbl_msg.ForeColor = System.Drawing.Color.Red;
                lbl_msg.Text = "Error: " + ex.Message.ToString();
                //throw;
            }
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

            //dbcl.CalculateOvertime(emp_wrkmnis, workdmins, lunchyesno, ref emp_calOThrs);

            UpdateDetails(id, jobid, workmansl, convtimein, convtimeout, lunchyesno, workdmins, workedhours, emp_calOThrs, new_pot, ddl_newattensttaus, ddl_newattencode);

            GridView2.EditIndex = -1;

            string CmdString3 = "select * from tbl_attendance where JOBID='" + jobid + "' order by Id desc";
            BindGrid2(CmdString3);

            //Response.Redirect(Request.Url.AbsoluteUri);
        }


        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && GridView2.EditIndex == e.Row.RowIndex)
            {
                // 1. SAFELY BIND ATTENDANCE STATUS
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
                    DDL_AttendanceStatus.Items.Insert(0, new ListItem("-- Select --", ""));

                    // SAFE SELECTION LOGIC
                    string attendanceStatus = DataBinder.Eval(e.Row.DataItem, "AttendanceStatus")?.ToString();
                    if (!string.IsNullOrEmpty(attendanceStatus))
                    {
                        ListItem item = DDL_AttendanceStatus.Items.FindByText(attendanceStatus);
                        if (item != null)
                        {
                            DDL_AttendanceStatus.ClearSelection();
                            item.Selected = true;
                        }
                    }
                }


                // 2. SAFELY BIND ATTENDANCE CODE
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
                    DDL_AttendanceCode.Items.Insert(0, new ListItem("-- Select --", ""));

                    // SAFE SELECTION LOGIC
                    string attendanceCode = DataBinder.Eval(e.Row.DataItem, "AttendanceCode")?.ToString();
                    if (!string.IsNullOrEmpty(attendanceCode))
                    {
                        ListItem item = DDL_AttendanceCode.Items.FindByValue(attendanceCode);
                        if (item != null)
                        {
                            DDL_AttendanceCode.ClearSelection();
                            item.Selected = true;
                        }
                    }
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

            //Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void DDL_Worksite_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CmdString = "select Employee_Name, Employee_Workman from tlb_atsworksiteIncharges where DB_Code='" + DDL_Worksite.SelectedValue.ToString() + "' order by Id";
            Bind_Approver(CmdString);
        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            detailsrow1.Visible = false;
            detailsrow2.Visible = false;
            detailsrow3.Visible = false;

            txt_jobid.Text = "";
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

        protected void btn_updatedate_Click(object sender, EventArgs e)
        {
            if (btn_updatedate.Text == "Swap Date")
            {
                swapdate1.Visible = true;
                swapdate2.Visible = true;
                btn_updatedate.Text = "Save New Date";

                txt_date.Focus();

                string title = "Notifications :";
                string body = "Select New JOB Date !!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

            }
            else
            {
                swapdate1.Visible = false;
                swapdate2.Visible = false;
                btn_updatedate.Text = "Swap Date";

                string jobid = txt_jobiddb.Text.ToString();
                UpdateJOBDate(jobid);
                UpdateAttendanceDate(jobid);
                Bind_JOBIDDetails(txt_jobid.Text.ToString());
                string title = "Notifications :";
                string body = "JOB Date has been UPDATED !!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }


        private void UpdateJOBDate(string jobid)
        {
            string intime = txt_date.Text.TrimEnd().ToString() + " 12:00:00 AM";
            //string current = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            DateTime crnttym = DateTime.ParseExact(intime, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_jobs set CreatedDate=@CreatedDate where JOBID=@JOBID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                cmd.Parameters.AddWithValue("@CreatedDate", crnttym);
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

        private void UpdateAttendanceDate(string jobid)
        {
            string intime = txt_date.Text.TrimEnd().ToString() + " 12:00:00 AM";
            string current = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            DateTime crnttym = DateTime.ParseExact(intime, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_attendance set CreatedDate=@CreatedDate where JOBID=@JOBID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                cmd.Parameters.AddWithValue("@CreatedDate", crnttym);
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
    }
}