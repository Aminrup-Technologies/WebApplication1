using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Text;

namespace WebApplication1.bussiness.production
{
    public partial class csm_soptraining : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
        DataTable dt = new DataTable();

        static string imglink = "~\\images\\No_Image.jpg";
        static string imgfilename = "N/A";

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

                    ActiveJOB_Checker();
                    JOBIDDetails_Row.Visible = false;
                    AttachedAttendanceRow.Visible = false;

                }
            }
        }

        private void ActiveJOB_Checker()
        {
            Int32 Activejobcount = CC.Find_ActiveJOBCountforSOP(Session["WORKMAN"].ToString(), Session["REGION"].ToString());
            if (Activejobcount > 0)
            {
                dbcl.FillCombo(DDL_JOBID, "select CONCAT(JOBID, ' : ', CONVERT(VARCHAR, CreatedDate, 105)) AS JOBID from tbl_jobs where [CreatedDate] >= DATEADD(DAY, -3, GETDATE()) and Creator_Workman='" + Session["WORKMAN"].ToString() + "' and JOBID_Status='Active' and SOP_Count=0 and SOPID is null order by CreatedDate desc ");
            }
            else
            {
                string title = "53 :Notifications :";
                string body = "NO Active JOB ID Found...! Kindly create a JOB ID and proceed.";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup_53", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void DDL_JOBID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_JOBID.SelectedIndex == 0)
            {
                string title = "63 : Notifications :";
                string body = "Select Valid JOBID";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup_63", "ShowPopup('" + title + "', '" + body + "');", true);

                JOBIDDetails_Row.Visible = false;
                AttachedAttendanceRow.Visible = false;
            }
            else
            {
                string jobID = DDL_JOBID.SelectedItem.Text.ToString();
                string ddljobid = "";
                string jobdate = "";
                string[] parts = jobID.Split(new string[] { " : " }, StringSplitOptions.None);

                if (parts.Length == 2)
                {
                    ddljobid = parts[0]; // This will contain "JOB0095467"
                    //jobdate = parts[1]; // This will contain "2024-02-02"

                    string[] dateParts = parts[1].Split('-');
                    if (dateParts.Length == 3)
                    {
                        // Convert date to "YYYY-MM-DD" format
                        jobdate = $"{dateParts[2]}-{dateParts[1]}-{dateParts[0]}";
                    }
                }

                Bind_JOBIDDetails(ddljobid, jobdate);
                JOBIDDetails_Row.Visible = true;


                sftysupvrow.Visible = true;
                string CmdString = "select FullName, WorkmanSL from tbl_Employee_Mustertable where SkillDesignation = 'SAFETY SUPERVISOR' and WorkStatus = 'Active' and WorkRegion = '" + Session["REGION"].ToString() + "' order by Id";
                Bind_SafetySupervisors(CmdString);

                sftyofcrrow.Visible = true;
                string CmdString1 = "select FullName, WorkmanSL from tbl_Employee_Mustertable where SkillDesignation = 'SAFETY OFFICER' and WorkStatus = 'Active' and WorkRegion = '" + Session["REGION"].ToString() + "' order by Id";
                Bind_SafetyOfficer(DDL_SftyOfcr, CmdString1);

                //Check if SOP already done
                Int32 SOPCount = CC.GetSOPCount(ddljobid);
                if (SOPCount >= 1)
                {
                    BindBasicSOPDetails(ddljobid);
                    txt_sopno.ReadOnly = true;
                    txt_sopdesc.ReadOnly = true;
                    txt_sopfaculty.ReadOnly = true;
                    txt_duration.ReadOnly = true;

                    CreateSOPIDRow.Visible = false;

                    ID_CreatedMsg.Visible = true;

                    ID_CreatedMsgHR.Visible = true;
                }
                else
                {

                }
            }
        }

        private void BindBasicSOPDetails(string jobid)
        {
            try
            {
                string query = "select SOP_ID, SOPNumber, SOPTitle, SOPTrainer, SOPDuration, SafetySupvWrk, SafetyOfficerWrk, SOPPhoto, SOP_PhotoID, SOP_PhotoFile from tbl_soptraining where Ref_JOBID=@Ref_JOBID";
                SqlParameter[] pram = {
                                          new SqlParameter("@Ref_JOBID",jobid),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    string panel1success = dt.Rows[0]["Panel1_Status"].ToString();
                    if (panel1success == "Completed")
                    {
                        lbl_ID.Text = dt.Rows[0]["SOP_ID"].ToString();
                        txt_sopno.Text = dt.Rows[0]["SOPNumber"].ToString();
                        txt_sopdesc.Text = dt.Rows[0]["SOPTitle"].ToString();
                        txt_sopfaculty.Text = dt.Rows[0]["SOPTrainer"].ToString();
                        txt_duration.Text = dt.Rows[0]["SOPDuration"].ToString();

                        string sftysupv = dt.Rows[0]["SafetySupvWrk"].ToString();
                        DDL_SftySupv.SelectedValue = sftysupv;

                        string sftyofcr = dt.Rows[0]["SafetyOfficerWrk"].ToString();
                        DDL_SftyOfcr.SelectedValue = sftyofcr;

                        string sopphoto = dt.Rows[0]["SOPPhoto"].ToString();
                        if (sopphoto == "Uploaded")
                        {
                            PhotographRow.Visible = true; UploadedPhotoRow1.Visible = true; UploadedPhotoRow2.Visible = true;
                            string photoid = dt.Rows[0]["SOP_PhotoID"].ToString();
                            lbl_photoid.Text = photoid;

                            string filename = dt.Rows[0]["SOP_PhotoFile"].ToString();
                            string prefix = @"\erp_images\SOPPhoto\";
                            ImgDisplay.ImageUrl = prefix + filename;

                            uploadbuttonrow1.Visible = false; uploadbuttonrow2.Visible = false;
                            PhotoUploaded.Visible = true;
                        }
                        else
                        {
                            PhotographRow.Visible = true; UploadedPhotoRow1.Visible = false; UploadedPhotoRow2.Visible = false;
                            uploadbuttonrow1.Visible = true; uploadbuttonrow2.Visible = true;
                            PhotoUploaded.Visible = false;
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                string title = "164 : Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup_164", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }
        private void Bind_SafetySupervisors(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_SftySupv.DataSource = Cmd.ExecuteReader();
            DDL_SftySupv.DataTextField = "FullName";
            DDL_SftySupv.DataValueField = "WorkmanSL";
            DDL_SftySupv.DataBind();
            DDL_SftySupv.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        public void Bind_SafetyOfficer(DropDownList cmbName, string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_SftyOfcr.DataSource = Cmd.ExecuteReader();
            DDL_SftyOfcr.DataTextField = "FullName";
            DDL_SftyOfcr.DataValueField = "WorkmanSL";
            DDL_SftyOfcr.DataBind();
            DDL_SftyOfcr.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        private void Bind_JOBIDDetails(string jobid, string jobdate)
        {
            try
            {
                string query = "select CreatedDate, Creator_Site, Creator_Workman, Creator_Region, Creator_Company, Creator_Name, Creator_SiteCode, WorkOrderNo, JOB_Title, JOB_Region, JOB_Company, JOB_Site, JOB_SiteCode, JOB_InchargeWrk, JOB_InchargeName, JOB_Dept, JOB_Location, JOB_Shift, JOB_PermitNo from tbl_jobs where JOBID=@JOBID and CreatedDate=@CreatedDate";
                SqlParameter[] pram = {
                                          new SqlParameter("@JOBID",jobid),
                                          new SqlParameter("@CreatedDate",jobdate),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    string Date = dt.Rows[0]["CreatedDate"].ToString();
                    string date3 = "";
                    if (Date == null || Date == "")
                    {
                        lbl_jobiddate.Text = "";
                    }
                    else
                    {
                        DateTime oDate2 = Convert.ToDateTime(Date);
                        string day2 = "";
                        string month2 = "";
                        if (oDate2.Day < 10)
                        {
                            day2 = "0" + oDate2.Day.ToString();
                        }
                        else
                        {
                            day2 = oDate2.Day.ToString();
                        }
                        if (oDate2.Month < 10)
                        {
                            month2 = "0" + oDate2.Month.ToString();
                        }
                        else
                        {
                            month2 = oDate2.Month.ToString();
                        }
                        date3 = oDate2.Year + "-" + month2 + "-" + day2;
                        lbl_jobiddate.Text = date3;
                    }

                    lbl_crtrsitename.Text = dt.Rows[0]["Creator_Site"].ToString();
                    lbl_creatorwrk.Text = dt.Rows[0]["Creator_Workman"].ToString();
                    lbl_creatorregion.Text = dt.Rows[0]["Creator_Region"].ToString();
                    lbl_creatorcompany.Text = dt.Rows[0]["Creator_Company"].ToString();

                    string supv = dt.Rows[0]["Creator_Name"].ToString();
                    lbl_jobcreatorname.Text = supv;
                    txt_sopfaculty.Text = supv;

                    lbl_crtrsitecode.Text = dt.Rows[0]["Creator_SiteCode"].ToString();
                    lbl_wrkordr.Text = dt.Rows[0]["WorkOrderNo"].ToString();
                    lbl_jobid.Text = jobid;
                    lbl_jobtitle.Text = dt.Rows[0]["JOB_Title"].ToString();
                    lbl_jobrgn.Text = dt.Rows[0]["JOB_Region"].ToString();
                    lbl_jobcompay.Text = dt.Rows[0]["JOB_Company"].ToString();
                    lbl_jobsite.Text = dt.Rows[0]["JOB_Site"].ToString();
                    lbl_jobsitecode.Text = dt.Rows[0]["JOB_SiteCode"].ToString();
                    lbl_inchargewrk.Text = dt.Rows[0]["JOB_InchargeWrk"].ToString();

                    inchargerow.Visible = true;
                    string incharge = dt.Rows[0]["JOB_InchargeName"].ToString();
                    lbl_inchargename.Text = incharge;
                    txt_incharge.Text = incharge;

                    string dept = dt.Rows[0]["JOB_Dept"].ToString();
                    txt_dept.Text = dept;
                    lbl_dept.Text = dept;

                    string loc = dt.Rows[0]["JOB_Location"].ToString();
                    lbl_jobloc.Text = loc;
                    txt_location.Text = loc;

                    lbl_jobshift.Text = dt.Rows[0]["JOB_Shift"].ToString();
                    lbl_permitno.Text = dt.Rows[0]["JOB_PermitNo"].ToString();

                    CheckforAttachedAttendnace();
                }
            }
            catch (Exception ex)
            {
                string title = "279 : Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup_281", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void CheckforAttachedAttendnace0()
        {
            string jobID = DDL_JOBID.SelectedItem.Text.ToString();
            string ddljobid = "";
            string jobdate = "";
            string[] parts = jobID.Split(new string[] { " : " }, StringSplitOptions.None);

            if (parts.Length == 2)
            {
                ddljobid = parts[0]; // This will contain "JOB0095467"
                                     //jobdate = parts[1]; // This will contain "2024-02-02"

                string[] dateParts = parts[1].Split('-');
                if (dateParts.Length == 3)
                {
                    // Convert date to "YYYY-MM-DD" format
                    jobdate = $"{dateParts[2]}-{dateParts[1]}-{dateParts[0]}";
                }
            }
            dbcl.Sqlconnection();
            string cmdstring = "select count (JOBID) from tbl_attendance where JOBID= '" + ddljobid.ToString() + "' and Creator_Workman='" + Session["WORKMAN"].ToString() + "'  and AttendanceStatus = 'Entry'";
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count == 0)
            {
                AttachedAttendanceRow.Visible = false;
            }
            else
            {
                AttachedAttendanceRow.Visible = true;
                Bind_AttendanceGridView();

                sopinput_1.Visible = true;
                sopinput_2.Visible = true;
                sopinput_3.Visible = true;
                sopinput_4.Visible = true;

                CreateSOPIDRow.Visible = true;
            }
            dbcl.DisconnectDb();
        }

        private void CheckforAttachedAttendnace()
        {
            string jobID = DDL_JOBID.SelectedItem.Text.ToString();
            string ddljobid = "";
            string jobdate = "";
            string[] parts = jobID.Split(new string[] { " : " }, StringSplitOptions.None);

            if (parts.Length == 2)
            {
                ddljobid = parts[0]; // This will contain "JOB0095467"
                string[] dateParts = parts[1].Split('-');
                if (dateParts.Length == 3)
                {
                    // Convert date to "YYYY-MM-DD" format
                    jobdate = $"{dateParts[2]}-{dateParts[1]}-{dateParts[0]}";
                }
            }

            // Use parameterized query to prevent SQL injection
            string cmdstring = "SELECT COUNT(JOBID) FROM tbl_attendance WHERE JOBID = @ddljobid AND CreatedDate=@CreatedDate and Creator_Workman = @workman AND AttendanceStatus = 'Entry'";
            dbcl.Sqlconnection();
            using (SqlConnection conn = dbcl.Conn)
            {
                using (SqlCommand cmd = new SqlCommand(cmdstring, conn))
                {
                    cmd.Parameters.AddWithValue("@ddljobid", ddljobid);
                    cmd.Parameters.AddWithValue("@workman", Session["WORKMAN"].ToString());
                    cmd.Parameters.AddWithValue("@CreatedDate", jobdate);
                    try
                    {
                        conn.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        AttachedAttendanceRow.Visible = count != 0;

                        if (AttachedAttendanceRow.Visible)
                        {
                            Bind_AttendanceGridView();
                            sopinput_1.Visible = true;
                            sopinput_2.Visible = true;
                            sopinput_3.Visible = true;
                            sopinput_4.Visible = true;
                            CreateSOPIDRow.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        string title = "279 : Notifications :";
                        string body = ex.Message;
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup_281", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }
            }
        }


        private void Bind_AttendanceGridView()
        {
            string jobID = DDL_JOBID.SelectedItem.Text.ToString();
            string ddljobid = "";
            string jobdate = "";
            string[] parts = jobID.Split(new string[] { " : " }, StringSplitOptions.None);

            if (parts.Length == 2)
            {
                ddljobid = parts[0]; // This will contain "JOB0095467"
                                     //jobdate = parts[1]; // This will contain "2024-02-02"

                string[] dateParts = parts[1].Split('-');
                if (dateParts.Length == 3)
                {
                    // Convert date to "YYYY-MM-DD" format
                    jobdate = $"{dateParts[2]}-{dateParts[1]}-{dateParts[0]}";
                }
            }
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string CmdString = "Select EmployeeWrk,EmployeeName,EmpDesignation,GatePassNo from tbl_attendance where Creator_Workman='" + Session["WORKMAN"].ToString() + "' and JOBID='" + ddljobid + "' and CreatedDate='" + jobdate + "' and AttendanceStatus = 'Entry' order by Id";

            //string CmdString = "Select a.EmployeeWrk, a.EmployeeName, a.EmpDesignation , b.GatePassNo from tbl_attendance a, tbl_Employee_Mustertable b where a.Creator_Workman='" + Session["WORKMAN"].ToString() + "' and a.JOBID='" + ddljobid + "' and a.AttendanceStatus = 'Entry' and a.EmployeeWrk=b.WorkmanSL order by a.Id";
            SqlCommand cmd = new SqlCommand(CmdString, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dtbl = new DataTable();
            ad.Fill(dtbl);
            EmpGrid.DataSource = dtbl;
            EmpGrid.DataBind();
            dbcl.Conn.Close();
        }

        protected void EmpGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //Get the Total RowCount values
                e.Row.Cells[0].Text = "Total Employee Count is :" + EmpGrid.Rows.Count;

                ContractEmployeesrow.Visible = true;
                txt_cntrctemp.Text = EmpGrid.Rows.Count.ToString();
            }
        }

        protected void EmpGrid_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //Check if the row in Footer
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //Apply colspan to your gridview column
                //You need to change this as per your gridview
                e.Row.Cells[0].ColumnSpan = 5;
                //Remove other rows in your gridview
                e.Row.Cells.RemoveAt(4);
                e.Row.Cells.RemoveAt(3);
                e.Row.Cells.RemoveAt(2);
                e.Row.Cells.RemoveAt(1);
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string sopnum = txt_sopno.Text.ToString();
            string soptitle = txt_sopdesc.Text.ToString();


            if (sopnum != "" && soptitle!= "")
            {
                if (Insert_SOPIDCreationData() == true)
                {
                    JOB_TableUpdate();
                    ID_CreatedMsg.Visible = true;
                    ID_CreatedMsg.Visible = true;
                    ID_CreatedMsgHR.Visible = true;

                    jobselectionpanel.Visible = false;

                    CreateSOPIDRow.Visible = false;

                    PhotographRow.Visible = true; uploadbuttonrow1.Visible = true; uploadbuttonrow2.Visible = true;
                }
                else
                {
                    string title = "382 : Notifications :";
                    string body = "Record cannot be inserted";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup_382", "ShowPopup('" + title + "', '" + body + "');", true);
                }
            }
            else
            {
                string title = "389 : Notifications :";
                string body = "SOP Number * SOP Title Required";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup_389", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void JOB_TableUpdate()
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_jobs set SOPID=@SOPID,SOP_Count=@SOP_Count where JOBID=@JOBID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@SOPID", lbl_ID.Text.ToString());
                cmd.Parameters.AddWithValue("@SOP_Count", "1");
                cmd.Parameters.AddWithValue("@JOBID", lbl_jobid.Text.ToString());
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                //string title = "Notifications :";
                //string body = "Panel 3 Data Saved";
                //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "419 : Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup_419", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private string FindSOPID()
        {
            string aa = null;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string kk = null;
            string cmdString1 = "select Id,SOP_ID from tbl_soptraining where Id=(select max(Id)from tbl_soptraining)";
            SqlCommand com1 = new SqlCommand(cmdString1, dbcl.Conn);
            SqlDataReader DR1 = com1.ExecuteReader();
            if (DR1.Read())
            {
                aa = DR1.GetValue(1).ToString();
                string bb = aa.Substring(5);
                int k = Convert.ToInt32(bb);
                k = k + 1;
                string q = Convert.ToString(k);
                kk = "SOP00" + q;
            }
            else
            {
                kk = "SOP001";
            }
            return kk;
        }

        private Boolean Insert_SOPIDCreationData()
        {
            Boolean datasaved = false;
            int flag = 0;

            try
            {
                string id = FindSOPID();
                lbl_ID.Text = id;
                dbcl.Sqlconnection();
                SqlCommand cmd = new SqlCommand("SP_InsertInto_tbl_soptraining", dbcl.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ref_JOBID", lbl_jobid.Text.ToString());
                cmd.Parameters.AddWithValue("@Ref_JOBTitle", lbl_jobtitle.Text.ToString());
                cmd.Parameters.AddWithValue("@Ref_JOBDate", lbl_jobiddate.Text.ToString());
                cmd.Parameters.AddWithValue("@Ref_JOBShift", lbl_jobshift.Text.ToString());
                cmd.Parameters.AddWithValue("@Ref_JOBRegion", lbl_jobrgn.Text.ToString());
                cmd.Parameters.AddWithValue("@Ref_JOBSupvName", lbl_jobcreatorname.Text.ToString());
                cmd.Parameters.AddWithValue("@Ref_JOBSupvWrk", lbl_creatorwrk.Text.ToString());
                cmd.Parameters.AddWithValue("@Ref_JOBSite", lbl_jobsite.Text.ToString());
                cmd.Parameters.AddWithValue("@Ref_JOBSiteCode", lbl_jobsitecode.Text.ToString());
                cmd.Parameters.AddWithValue("@Ref_JOBIncharge", lbl_inchargename.Text.ToString() );
                cmd.Parameters.AddWithValue("@Ref_JOBInchargeWrk", lbl_inchargewrk.Text.ToString());

                cmd.Parameters.AddWithValue("@SOP_Date", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@SOP_ID", id);
                cmd.Parameters.AddWithValue("@SOP_Region", Session["REGION"].ToString());
                cmd.Parameters.AddWithValue("@SOP_Dept", txt_dept.Text.ToString());
                cmd.Parameters.AddWithValue("@SOP_Location", txt_location.Text.ToString());
                cmd.Parameters.AddWithValue("@SOP_SubmitterWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@SOP_SubmitterName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@ContractEmployees", txt_cntrctemp.Text.ToString());
                cmd.Parameters.AddWithValue("@SOPNumber", txt_sopno.Text.ToUpper().ToString());
                cmd.Parameters.AddWithValue("@SOPTitle", txt_sopdesc.Text.ToUpper().ToString());
                cmd.Parameters.AddWithValue("@SOPTrainer", txt_sopfaculty.Text.ToUpper().ToString());
                cmd.Parameters.AddWithValue("@SOPDuration", txt_duration.Text.ToString());

                cmd.Parameters.AddWithValue("@SOPPhoto", "Pending");

                cmd.Parameters.AddWithValue("@SafetySupvWrk", DDL_SftySupv.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@SafetySupvName", DDL_SftySupv.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@SafetySupvApprovalStatus", "Pending");

                cmd.Parameters.AddWithValue("@SafetyOfficerWrk", DDL_SftyOfcr.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@SafetyOfficerName", DDL_SftyOfcr.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@SO_ApprovalStatus", "Pending");

                cmd.Parameters.AddWithValue("@Panel1_Status", "Completed");

                dbcl.ConnectDb();
                flag = cmd.ExecuteNonQuery();

                if (flag != 0)
                {
                    datasaved = true;
                    lbl_msg.Visible = true;
                    lbl_msg.Text = "Record Inserted Successfully..!";
                    lbl_msg.ForeColor = System.Drawing.Color.DarkGreen;
                    dbcl.DisconnectDb();
                }
                else
                {
                    datasaved = false;
                }
            }
            catch (Exception ex)
            {
                string title = "517 : Notifications :";
                string body = ex.Message.ToString();
                ClientScript.RegisterStartupScript(this.GetType(), "Popup_517", "ShowPopup('" + title + "', '" + body + "');", true);
                datasaved = false;
            }

            return datasaved;
        }



        //-----Photograph------------//
        private Boolean UploadTBMImage1()
        {
            Boolean imgsaved = false;

            DateTime d = DateTime.Now;
            string month = d.Month.ToString();
            string year = d.Year.ToString();
            string day = d.Day.ToString();
            string imgdate = day + month + year;

            // Check file exist or not
            if (FileUploader.PostedFile != null)
            {
                // Check the extension of image
                string extension = Path.GetExtension(FileUploader.FileName);
                if (extension.ToLower() == ".png" || extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg")
                {
                    Stream strm = FileUploader.PostedFile.InputStream;
                    using (var image = System.Drawing.Image.FromStream(strm))
                    {
                        string PhotoId = lbl_ID.Text.ToString();
                        lbl_photoid.Text = PhotoId;


                        // Print Original Size of file (Height or Width)
                        //lblprev.Text = image.Size.ToString();

                        int newWidth = 440; // New Width of Image in Pixel
                        int newHeight = 540; // New Height of Image in Pixel
                        var thumbImg = new Bitmap(newWidth, newHeight);
                        var thumbGraph = Graphics.FromImage(thumbImg);

                        thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                        thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                        thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        var imgRectangle = new Rectangle(0, 0, newWidth, newHeight);
                        thumbGraph.DrawImage(image, imgRectangle);

                        // Save the file
                        string targetPath = Server.MapPath(@"\erp_images\SOPPhoto\") + PhotoId + "_" + imgdate + ".jpg";
                        FileUploader.SaveAs(Server.MapPath(@"\erp_images\SOPPhoto\") + PhotoId + "_" + imgdate + ".jpg");

                        //the below will be saved as database value
                        imglink = "\\erp_images\\SOPPhoto\\" + PhotoId + "_" + imgdate + ".jpg";
                        thumbImg.Save(targetPath, image.RawFormat);
                        imgfilename = PhotoId + "_" + imgdate + ".jpg";

                        // Print new Size of file (height or Width)
                        //lblaftr.Text = thumbImg.Size.ToString();

                        //Show Image instantly

                        ImgDisplay.ImageUrl = @"\erp_images\SOPPhoto\" + PhotoId + "_" + imgdate + ".jpg";

                        //on successfully image is saved
                        imgsaved = true;

                        string title = "586 : Notifications :";
                        string body = "Photograph Uploaded Successfully";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup_586", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }
                else
                {
                    string title = "593 : Notifications :";
                    string body = "Kindly Select Appropriate File Type";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup_593", "ShowPopup('" + title + "', '" + body + "');", true);
                }
            }
            return imgsaved;
        }

        //------------------ TBT Panel 3 Data Saver-------------------------//
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (UploadTBMImage() == true && TBT_Panel3Data() == true)
            {
                uploadbuttonrow1.Visible = false;
                uploadbuttonrow2.Visible = false;
                UploadedPhotoRow1.Visible = true;
                UploadedPhotoRow2.Visible = true;

                SavePanel3Data.Visible = true;
            }
            else
            {
                FileUploader.Focus();
                FileUploader.BorderColor = Color.Red;

                UploadedPhotoRow1.Visible = false;
                UploadedPhotoRow2.Visible = false;


                SavePanel3Data.Visible = false;
            }
        }

        private Boolean TBT_Panel3Data()
        {
            Boolean datasaved = false;

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_soptraining set SOPPhoto=@SOPPhoto,SOP_PhotoID=@SOP_PhotoID, SOP_PhotoFile=@SOP_PhotoFile,SOP_PhotoPath=@SOP_PhotoPath, SOP_PhotoTimeStamp=@SOP_PhotoTimeStamp where SOP_ID=@SOP_ID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@SOP_ID", lbl_ID.Text.ToString());
                cmd.Parameters.AddWithValue("@SOPPhoto", "Uploaded");
                cmd.Parameters.AddWithValue("@SOP_PhotoID", lbl_photoid.Text.ToString());
                cmd.Parameters.AddWithValue("@SOP_PhotoFile", imgfilename);
                cmd.Parameters.AddWithValue("@SOP_PhotoPath", imglink);
                cmd.Parameters.AddWithValue("@SOP_PhotoTimeStamp", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                datasaved = true;
                string title = "650 : Notifications :";
                string body = "Panel 3 Data Saved";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup_650", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "656 :Notifications";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup_650", "ShowPopup('" + title + "', '" + body + "');", true);
                datasaved = false;
            }

            return datasaved;
        }

        protected void btn_saveTBTPhoto_Click(object sender, EventArgs e)
        {
            PhotoUploaded.Visible = true;
            SavePanel3Data.Visible = false;
            PhotographRow.Visible = false;

            Response.Redirect("csms_mainview.aspx");
        }


        private bool UploadTBMImage()
        {
            bool imgSaved = false;

            try
            {
                DateTime now = DateTime.Now;
                string month = now.Month.ToString();
                string year = now.Year.ToString();
                string day = now.Day.ToString();
                string imgDate = day + month + year;

                if (FileUploader.PostedFile != null)
                {
                    string extension = Path.GetExtension(FileUploader.FileName);
                    string[] allowedExtensions = { ".png", ".jpg", ".jpeg" };

                    if (allowedExtensions.Contains(extension.ToLower()))
                    {
                        using (Stream stream = FileUploader.PostedFile.InputStream)
                        {
                            using (var image = System.Drawing.Image.FromStream(stream))
                            {
                                string photoId = lbl_ID.Text.ToString();
                                lbl_photoid.Text = photoId;

                                int newWidth = 440; // New Width of Image in Pixels
                                int newHeight = 540; // New Height of Image in Pixels

                                using (var thumbImg = new Bitmap(newWidth, newHeight))
                                {
                                    using (var thumbGraph = Graphics.FromImage(thumbImg))
                                    {
                                        thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                                        thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                                        thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

                                        var imgRectangle = new Rectangle(0, 0, newWidth, newHeight);
                                        thumbGraph.DrawImage(image, imgRectangle);

                                        string directoryPath = Server.MapPath(@"\erp_images\SOPPhoto\");
                                        if (!Directory.Exists(directoryPath))
                                        {
                                            Directory.CreateDirectory(directoryPath);
                                        }

                                        string targetPath = Path.Combine(directoryPath, photoId + "_" + imgDate + ".jpg");
                                        thumbImg.Save(targetPath, image.RawFormat);

                                        imglink = @"\erp_images\SOPPhoto\" + photoId + "_" + imgDate + ".jpg";
                                        imgfilename = photoId + "_" + imgDate + ".jpg";

                                        ImgDisplay.ImageUrl = @"\erp_images\SOPPhoto\" + photoId + "_" + imgDate + ".jpg";

                                        imgSaved = true;

                                        ShowNotification("Photograph Uploaded Successfully");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ShowNotification("Kindly Select Appropriate File Type");
                    }
                }
            }
            catch (Exception ex)
            {
                string title = "745 : Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup_745", "ShowPopup('" + title + "', '" + body + "');", true);
            }

            return imgSaved;
        }

        private void ShowNotification(string message)
        {
            string title = "755 : Notifications";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup_755", "ShowPopup('" + title + "', '" + message + "');", true);
        }

    }
}