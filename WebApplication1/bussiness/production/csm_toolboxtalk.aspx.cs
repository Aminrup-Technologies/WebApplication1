using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.Drawing.Drawing2D;
using System.Text;

namespace WebApplication1.bussiness.production
{
    public partial class toolboxtalk : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
        DataTable dt = new DataTable();

        static string imglink = "~\\images\\No_Image.jpg";
        static string imgfilename = "N/A";

        public static Boolean Panel1_flag = false;
        public static Boolean Panel2_flag = false;
        public static Boolean Panel3_flag = false;

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

                    if (ActiveJobChecker())
                    {
                        BindTBTCheckLists();
                        JOBIDDetails_Row.Visible = false;
                        AttachedAttendanceRow.Visible = false;
                        Default_Buttons.Visible = false;
                    }
                    else
                    {
                        Default_Buttons.Visible = true;
                    }

                }
            }
        }

        private void ActiveJOB_Checker1()
        {
            Int32 Activejobcount = CC.Find_ActiveJOBCountforINPunch(Session["WORKMAN"].ToString(), Session["REGION"].ToString());
            if (Activejobcount > 0)
            {
                dbcl.FillCombo(DDL_JOBID, "SELECT JOBID FROM tbl_jobs WHERE Creator_Workman = '" + Session["WORKMAN"].ToString() + "' AND JOBID_Status = 'Active' ORDER BY CreatedDate DESC");
            }
            else
            {
                string NoActiveJOBID_Notification_script = @"<script type='text/javascript'>
                            new PNotify({
                                title: 'Oh No !!!',
                                text: 'You Don\'t have an ACTIVE JOB',
                                type: 'error',
                                styling: 'bootstrap3'
                            });
                        </script>";

                ClientScript.RegisterStartupScript(this.GetType(), "NoActiveJOBID_Notification", NoActiveJOBID_Notification_script, false);
            }
        }

        private bool ActiveJobChecker()
        {
            if (Session["WORKMAN"] != null && Session["REGION"] != null)
            {
                Int32 activeJobCount = CC.Find_ActiveJOBCountforINPunch(Session["WORKMAN"].ToString(), Session["REGION"].ToString());

                if (activeJobCount > 0)
                {
                    dbcl.FillCombo(DDL_JOBID, "SELECT CONCAT(JOBID, ' : ', CONVERT(VARCHAR, CreatedDate, 105)) AS JOBID FROM tbl_jobs WHERE Creator_Workman = '" + Session["WORKMAN"].ToString() + "' AND JOBID_Status = 'Active' AND [CreatedDate] >= DATEADD(DAY, -3, GETDATE()) ORDER BY CreatedDate DESC");

                    // Return true if there are active jobs
                    return true;
                }
                else
                {
                    DDL_SftyOfcr.Items.Insert(0, "Please Select Option");

                    string noActiveJobIdNotificationScript = @"<script type='text/javascript'>
                            new PNotify({
                                title: 'Oh No !!!',
                                text: 'You Don\'t have an ACTIVE JOB',
                                type: 'error',
                                styling: 'bootstrap3'
                            });
                        </script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "NoActiveJOBID_Notification", noActiveJobIdNotificationScript, false);

                    // Return false if there are no active jobs
                    return false;
                }
            }
            else
            {
                // Handle session variables being null
                return false;
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


        protected void DDL_JOBID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_JOBID.SelectedIndex == 0)
            {
                string JOBID_Selector_script = @"<script type='text/javascript'>
                            new PNotify({
                                title: 'Guide',
                                text: 'Please select a valid & active JOBID',
                                type: 'info',
                                styling: 'bootstrap3'
                            });
                        </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "JOBID_Selector_Notification", JOBID_Selector_script, false);

                //string title = "Notifications :";
                //string body = "Select Valid JOBID";
                //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                JOBIDDetails_Row.Visible = false;
                AttachedAttendanceRow.Visible = false;
                locationrow.Visible = false;
                deptrow.Visible = false;
                LineManagerRow.Visible = false;
                sftysupvrow.Visible = false;
                supvrow.Visible = false;
                ContractEmployeesrow.Visible = false;
                CreateTBTIDRow.Visible = false;
                jobsupv.Visible = false;
                sftyofcrrow.Visible = false;
            }
            else
            {
                string jobID  = DDL_JOBID.SelectedItem.Text.ToString();
                string ddljobid = "";
                string jobdate = "";
                string[] parts = jobID.Split(new string[] { " : " }, StringSplitOptions.None);

                if (parts.Length == 2)
                {
                    ddljobid = parts[0]; // This will contain "JOB0095467"
                    jobdate = parts[1]; // This will contain "2024-02-02"

                    // Now you can use jobID and dateStr as needed.
                }
                else
                {
                    // Handle the case where the string format is unexpected.
                }

                sftysupvrow.Visible = true;
                sftyofcrrow.Visible = true;

                string CmdString = "select FullName, WorkmanSL from tbl_Employee_Mustertable where SkillDesignation = 'SAFETY SUPERVISOR' and WorkStatus = 'Active' and WorkRegion = '" + Session["REGION"].ToString() + "' order by Id";
                Bind_SafetySupervisors(CmdString);

                string CmdString1 = "select FullName, WorkmanSL from tbl_Employee_Mustertable where SkillDesignation = 'SAFETY OFFICER' and WorkStatus = 'Active' and WorkRegion = '" + Session["REGION"].ToString() + "' order by Id";
                Bind_SafetyOfficer(DDL_SftyOfcr, CmdString1);


                //Check if TBT is already added aganist this JOBID
                //Int32 TBTCount = CC.GetTBTCount(ddljobid);
                if (!CC.TBTRecordExist(ddljobid, jobdate))
                {
                    Bind_JOBIDDetails(ddljobid);
                    JOBIDDetails_Row.Visible = true;

                    LineManagerRow.Visible = true;
                    ContractEmployeesrow.Visible = true;

                }
                else
                {
                    Bind_JOBIDDetails(ddljobid);
                    JOBIDDetails_Row.Visible = true;

                    Bind_TBTDetails(ddljobid);

                    //Popup to notify user of TBT completion

                    if (Panel1_flag ==true && Panel2_flag ==true && Panel3_flag == true)
                    {
                        string title = "Notifications :";
                        string body = "You have already added a Tool Box talk against the selected JOBID";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                    else if (Panel1_flag == true && Panel2_flag != true && Panel3_flag != true)
                    {
                        string Panel1_Completed_script = @"<script type='text/javascript'>
                            new PNotify({
                                title: 'Guide',
                                text: 'ID Created TBT Incomplete....!!',
                                type: 'success',
                                styling: 'bootstrap3'
                            });
                        </script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "Panel1_Completed_Notification", Panel1_Completed_script, false);

                        //string title = "Notifications :";
                        //string body = "TBT ID Created, Incomplete submission....!!";
                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                    else if (Panel1_flag == true && Panel2_flag == true && Panel3_flag != true)
                    {
                        string title = "Notifications :";
                        string body = "TBT Photograph Not uploaded...!!!";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }
            }
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

                    supvrow.Visible = true;
                    string supv = dt.Rows[0]["Creator_Name"].ToString();
                    lbl_jobcreatorname.Text = supv;
                    txt_supv.Text = supv;

                    lbl_crtrsitecode.Text = dt.Rows[0]["Creator_SiteCode"].ToString();
                    lbl_wrkordr.Text = dt.Rows[0]["WorkOrderNo"].ToString();
                    lbl_jobid.Text = dt.Rows[0]["JOBID"].ToString();
                    lbl_jobrgn.Text = dt.Rows[0]["JOB_Region"].ToString();
                    lbl_jobcompay.Text = dt.Rows[0]["JOB_Company"].ToString();
                    lbl_jobsite.Text = dt.Rows[0]["JOB_Site"].ToString();
                    lbl_jobsitecode.Text = dt.Rows[0]["JOB_SiteCode"].ToString();
                    lbl_inchargewrk.Text = dt.Rows[0]["JOB_InchargeWrk"].ToString();

                    jobsupv.Visible = true;
                    string incharge = dt.Rows[0]["JOB_InchargeName"].ToString();
                    lbl_inchargename.Text = incharge;
                    txt_incharge.Text = incharge;

                    string dept = dt.Rows[0]["JOB_Dept"].ToString();
                    deptrow.Visible = true;
                    lbl_dept.Text = dept;
                    txt_dept.Text = dept;

                    locationrow.Visible = true;
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
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }


        private void Bind_TBTDetails(string jobid)
        {

            try
            {
                string query = "select * from tbl_toolboxtalkdata where Ref_JOBID=@Ref_JOBID";
                SqlParameter[] pram = {
                                          new SqlParameter("@Ref_JOBID",jobid),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    string Panel1status = dt.Rows[0]["Panel1_Status"].ToString();
                    if (Panel1status == "Complete")
                    {
                        Panel1_flag = true;
                        lbl_TBTID.Text = dt.Rows[0]["TBT_ID"].ToString();
                        lbl_TBTID.Visible = true;
                        CreateTBTIDRow.Visible = false;
                        TBTID_CreatedMsg.Visible = true;


                        txt_location.Text = dt.Rows[0]["TBT_Location"].ToString();
                        txt_dept.Text = dt.Rows[0]["TBT_Dept"].ToString();
                        txt_supv.Text = dt.Rows[0]["TBT_SupvName"].ToString();
                        txt_linemanager.Text = dt.Rows[0]["LineManager"].ToString();

                        supvrow.Visible = true;
                        string sftysupv = dt.Rows[0]["SafetySupvWrk"].ToString();
                        DDL_SftySupv.SelectedValue = sftysupv;
                        DDL_SftySupv.Enabled = false;

                        sftyofcrrow.Visible = true;
                        string sftyofcsupv = dt.Rows[0]["SafetyOfficerWrk"].ToString();
                        DDL_SftyOfcr.SelectedValue = sftyofcsupv;
                        DDL_SftyOfcr.Enabled = false;

                        txt_incharge.Text = dt.Rows[0]["AreaInchargeName"].ToString();

                        txt_cntrctemp.ReadOnly = true;
                        txt_cntrctemp.Text = dt.Rows[0]["ContractEmployees"].ToString();
                    }
                    else
                    {

                    }

                    string Panel2status = dt.Rows[0]["Panel2_Status"].ToString();
                    if (Panel2status == "Complete")
                    {
                        Panel2_flag = true;
                        panel2heading.Visible = false;
                        TBTPanel2.Visible = true;

                        SavePanel2Data.Visible = false;
                        TBT_ItemsSavedMsg.Visible = true;

                        txt_sftyintrst.ReadOnly = true;
                        txt_sftyintrst.Text = dt.Rows[0]["SafetyInterestItems"].ToString();

                        //Checkbox for personal responsiblities
                        string chkditemsfromdb = dt.Rows[0]["EmpPersnlItems"].ToString();

                        string[] items = chkditemsfromdb.Split(',');
                        for (int i = 0; i <= items.GetUpperBound(0); i++)
                        {
                            ListItem currentcheckbox = chkbxrspons.Items.FindByText(items[i].ToString());
                            if (currentcheckbox != null)
                            {
                                currentcheckbox.Selected = true;
                            }
                        }

                        txt_sopno.ReadOnly = true;
                        txt_sopno.Text = dt.Rows[0]["SOPNumber"].ToString();

                        txt_hazards.ReadOnly = true;
                        txt_hazards.Text = dt.Rows[0]["HazardMaterialItems"].ToString();

                        txt_sftmsg.ReadOnly = true;
                        txt_sftmsg.Text = dt.Rows[0]["SafetyMessageItems"].ToString();

                        //txt_sftalert.ReadOnly = true;
                        //txt_sftalert.Text = dt.Rows[0]["SafetyAlertItems"].ToString();
                    }
                    else
                    {
                        panel2heading.Visible = true;
                        TBTPanel2.Visible = true;

                        SavePanel2Data.Visible = true;
                        TBT_ItemsSavedMsg.Visible = false;

                        txt_sftyintrst.ReadOnly = false;
                        txt_sftyintrst.Text = "";

                        txt_sopno.ReadOnly = false;
                        txt_sopno.Text = "";

                        txt_hazards.ReadOnly = false;
                        txt_hazards.Text = "";

                        txt_sftmsg.ReadOnly = false;
                        txt_sftmsg.Text = "";

                        //txt_sftalert.ReadOnly = false;
                        //txt_sftalert.Text = "";

                        TBT_ItemsSavedMsgHR.Visible = true;
                    }


                    string Panel3status = dt.Rows[0]["TBTPhoto"].ToString();
                    string filename = dt.Rows[0]["TBT_PhotoFile"].ToString();
                    if (Panel3status == "Uploaded")
                    {
                        Panel3_flag = true;
                        TBTPhotographRow.Visible = true;

                        uploadbuttonrow1.Visible = false;
                        uploadbuttonrow2.Visible = false;

                        UploadedPhotoRow1.Visible = true;
                        UploadedPhotoRow2.Visible = true;

                        SavePanel3Data.Visible = false;

                        TBTPhotoUploaded.Visible = true;

                        string prefix = @"\erp_images\TBTPhoto\";
                        ImgDisplay.ImageUrl = prefix + filename ;
                    }
                    else
                    {
                        TBTPhotographRow.Visible = true;

                        uploadbuttonrow1.Visible = true;
                        uploadbuttonrow2.Visible = true;

                        UploadedPhotoRow1.Visible = false;
                        UploadedPhotoRow2.Visible = false;

                        SavePanel3Data.Visible = true;

                        TBTPhotoUploaded.Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }


        private void CheckforAttachedAttendnace()
        {
            //string ddljobid = DDL_JOBID.SelectedItem.Text.ToString();
            string jobID = DDL_JOBID.SelectedItem.Text.ToString();
            string ddljobid = "";
            string jobdate = "";
            string[] parts = jobID.Split(new string[] { " : " }, StringSplitOptions.None);

            if (parts.Length == 2)
            {
                ddljobid = parts[0]; // This will contain "JOB0095467"
                jobdate = parts[1]; // This will contain "2024-02-02"

                // Now you can use jobID and dateStr as needed.
            }
            else
            {
                // Handle the case where the string format is unexpected.
            }

            dbcl.Sqlconnection();
            string cmdstring = "select count (Id) from tbl_attendance where JOBID= '" + ddljobid.ToString() + "' and Creator_Workman='" + Session["WORKMAN"].ToString() + "'  and AttendanceStatus = 'Entry'";
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
            }
            dbcl.DisconnectDb();
        }

        private void Bind_AttendanceGridView()
        {
            //string ddljobid = DDL_JOBID.SelectedItem.Text.ToString();
            string jobID = DDL_JOBID.SelectedItem.Text.ToString();
            string ddljobid = "";
            string jobdate = "";
            string[] parts = jobID.Split(new string[] { " : " }, StringSplitOptions.None);

            if (parts.Length == 2)
            {
                ddljobid = parts[0]; // This will contain "JOB0095467"
                jobdate = parts[1]; // This will contain "2024-02-02"

                // Now you can use jobID and dateStr as needed.
            }
            else
            {
                // Handle the case where the string format is unexpected.
            }

            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string CmdString = "Select * from tbl_attendance where Creator_Workman='" + Session["WORKMAN"].ToString() + "' and JOBID='" + ddljobid + "' and AttendanceStatus = 'Entry' order by Id";

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
                CreateTBTIDRow.Visible = true;
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


        protected void BindTBTCheckLists()
        {
            string cmdString = "select * from tlb_personalresponsibilities order by Id";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            chkbxrspons.DataTextField = "PS_ItemText";
            chkbxrspons.DataValueField = "PS_ItemID";
            chkbxrspons.DataSource = dt;
            chkbxrspons.DataBind();
            dbcl.DisconnectDb();
        }

        //------------------ TBT Panel 1 Data Saver-------------------------//
        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (TBT_Panel1Data() == true)
            {
                jobselectionpanel.Visible = false;
                CreateTBTIDRow.Visible = false;
                TBTID_CreatedMsg.Visible = true;
                TBTID_CreatedMsgHR.Visible = true;


                panel2heading.Visible = true;
                TBTPanel2.Visible = true;
                SavePanel2Data.Visible = true;

                string title = "Notifications :";
                string body = "Panel 1 Data Saved";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            else
            {
                jobselectionpanel.Visible = true;
                CreateTBTIDRow.Visible = true;
                TBTID_CreatedMsg.Visible = false;
                TBTID_CreatedMsgHR.Visible = false;


                panel2heading.Visible = false;
                TBTPanel2.Visible = false;
                SavePanel2Data.Visible = false;
            }
        }

        private string FindTBTID()
        {
            string aa = null;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string kk = null;
            string cmdString1 = "select Id,TBT_ID from tbl_toolboxtalkdata where Id=(select max(Id)from tbl_toolboxtalkdata)";
            SqlCommand com1 = new SqlCommand(cmdString1, dbcl.Conn);
            SqlDataReader DR1 = com1.ExecuteReader();
            if (DR1.Read())
            {
                aa = DR1.GetValue(1).ToString();
                string bb = aa.Substring(5);
                int k = Convert.ToInt32(bb);
                k = k + 1;
                string q = Convert.ToString(k);
                kk = "TBT00" + q;
            }
            else
            {
                kk = "TBT001";
            }
            return kk;
        }


        private Boolean TBT_Panel1Data()
        {
            Boolean datasaved = false;
            int flag = 0;

            try
            {
                string tbtid = FindTBTID();
                lbl_TBTID.Text = tbtid;
                dbcl.Sqlconnection();
                SqlCommand cmd = new SqlCommand("SP_InsertInto_TBTDataTable", dbcl.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ref_JOBID", lbl_jobid.Text.ToString());
                cmd.Parameters.AddWithValue("@Ref_JOBDate", lbl_jobiddate.Text.ToString());
                cmd.Parameters.AddWithValue("@Ref_JOBRegion", lbl_jobrgn.Text.ToString());
                cmd.Parameters.AddWithValue("@Ref_JOBSupvName", lbl_jobcreatorname.Text.ToString());
                cmd.Parameters.AddWithValue("@Ref_JOBSupvWrk", lbl_creatorwrk.Text.ToString());
                cmd.Parameters.AddWithValue("@TBT_Date", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@TBT_ID", tbtid);
                cmd.Parameters.AddWithValue("@TBT_Region", Session["REGION"].ToString());
                cmd.Parameters.AddWithValue("@TBT_Dept", txt_dept.Text.ToString());
                cmd.Parameters.AddWithValue("@TBT_Location", txt_location.Text.ToString());
                cmd.Parameters.AddWithValue("@TBT_SupvWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@TBT_SupvName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@SafetySupvWrk", DDL_SftySupv.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@SafetySupvName", DDL_SftySupv.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@SafetySupvApprovalStatus", "Pending");
                cmd.Parameters.AddWithValue("@LineManager", txt_linemanager.Text.ToUpper().ToString());
                cmd.Parameters.AddWithValue("@AreaInchargeWrk", lbl_inchargewrk.Text.ToString());
                cmd.Parameters.AddWithValue("@AreaInchargeName", lbl_inchargename.Text.ToString());
                cmd.Parameters.AddWithValue("@ContractEmployees", txt_cntrctemp.Text.ToUpper().ToString());
                cmd.Parameters.AddWithValue("@Panel1_Status", "Complete");
                cmd.Parameters.AddWithValue("@Panel1_TimeStamp", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@Panel2_Status", "Pending");
                cmd.Parameters.AddWithValue("@TBTPhoto", "Pending");
                cmd.Parameters.AddWithValue("@SafetyOfficerWrk", DDL_SftyOfcr.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@SafetyOfficerName", DDL_SftyOfcr.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@SO_ApprovalStatus", "Pending");
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
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                datasaved = false;
            }

            return datasaved;
        }


        //------------------ TBT Panel 2 Data Saver-------------------------//
        protected void btn_savetbtdata_Click(object sender, EventArgs e)
        {
            if (TBT_Panel2Data() == true)
            {
                JOB_TableUpdate();
                panel2heading.Visible = false;
                TBTPanel2.Visible = false;
                SavePanel2Data.Visible = false;

                TBT_ItemsSavedMsg.Visible = true;
                TBT_ItemsSavedMsgHR.Visible = true;


                TBTPhotographRow.Visible = true;
            }
            else
            {
                panel2heading.Visible = true;
                TBTPanel2.Visible = true;
                SavePanel2Data.Visible = true;

                TBT_ItemsSavedMsg.Visible = false;
                TBT_ItemsSavedMsgHR.Visible = false;


                TBTPhotographRow.Visible = false;

                string title = "Notifications :";
                string body = "Panel 2 Data NOT Saved or TBTID NOT attched";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private string FindChkdItems()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < chkbxrspons.Items.Count; i++)
            {
                if (chkbxrspons.Items[i].Selected == true)
                {
                    sb.Append(chkbxrspons.Items[i].Text + ",");
                }
            }
            string chkditms = sb.ToString().TrimEnd(',');
            return chkditms;
        }

        private Boolean TBT_Panel2Data()
        {
            Boolean datasaved = false;

            try
            {
                string prsnl_resp = FindChkdItems();
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_toolboxtalkdata set PrevActionItem=@PrevActionItem,NewIncidentItem=@NewIncidentItem, Ref_IncidentID=@Ref_IncidentID,SafetyInterest=@SafetyInterest, SafetyInterestItems=@SafetyInterestItems, SOPYesNo=@SOPYesNo, SOPNumber=@SOPNumber,EmpPrsnlResponsibilty=@EmpPrsnlResponsibilty, EmpPersnlItems=@EmpPersnlItems, HazardMaterial=@HazardMaterial, HazardMaterialItems=@HazardMaterialItems, SafetyMessage=@SafetyMessage, SafetyMessageItems=@SafetyMessageItems, SafetyAlert=@SafetyAlert, SafetyAlertItems=@SafetyAlertItems, Ref_ActionItem=@Ref_ActionItem, Ref_ActionID=@Ref_ActionID, Panel2_Status=@Panel2_Status, Panel2_TimeStamp=@Panel2_TimeStamp  where TBT_ID=@TBT_ID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@TBT_ID", lbl_TBTID.Text.ToString());


                bool Point1 = (Page.Request.Form["BoxName1"] == "on") ? true : false;
                if (Point1 == true)
                {
                    cmd.Parameters.AddWithValue("@PrevActionItem","Yes");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PrevActionItem","No");
                }

                bool Point2 = (Page.Request.Form["BoxName2"] == "on") ? true : false;
                if (Point2 == true)
                {
                    cmd.Parameters.AddWithValue("@NewIncidentItem", "Yes");
                    cmd.Parameters.AddWithValue("@Ref_IncidentID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@NewIncidentItem", "No");
                    cmd.Parameters.AddWithValue("@Ref_IncidentID", DBNull.Value);
                }

                bool Point3 = (Page.Request.Form["BoxName3"] == "on") ? true : false;
                if (Point3 == true)
                {
                    cmd.Parameters.AddWithValue("@SafetyInterest", "Yes");
                    cmd.Parameters.AddWithValue("@SafetyInterestItems", txt_sftyintrst.Text.ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SafetyInterest", "No");
                    cmd.Parameters.AddWithValue("@SafetyInterestItems", DBNull.Value);
                }

                bool Point4 = (Page.Request.Form["BoxName4"] == "on") ? true : false;
                if (Point4 == true)
                {
                    cmd.Parameters.AddWithValue("@SOPYesNo", "Yes");
                    cmd.Parameters.AddWithValue("@SOPNumber", txt_sopno.Text.ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SOPYesNo", "No");
                    cmd.Parameters.AddWithValue("@SOPNumber", DBNull.Value);
                }

                bool Point5 = (Page.Request.Form["BoxName5"] == "on") ? true : false;
                if (Point5 == true)
                {
                    cmd.Parameters.AddWithValue("@EmpPrsnlResponsibilty", "Yes");
                    cmd.Parameters.AddWithValue("@EmpPersnlItems", prsnl_resp);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@EmpPrsnlResponsibilty", "No");
                    cmd.Parameters.AddWithValue("@EmpPersnlItems", prsnl_resp);
                }


                bool Point6 = (Page.Request.Form["BoxName6"] == "on") ? true : false;
                if (Point6 == true)
                {
                    cmd.Parameters.AddWithValue("@HazardMaterial", "Yes");
                    cmd.Parameters.AddWithValue("@HazardMaterialItems", txt_hazards.Text.ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@HazardMaterial", "No");
                    cmd.Parameters.AddWithValue("@HazardMaterialItems", DBNull.Value);
                }

                bool Point7 = (Page.Request.Form["BoxName7"] == "on") ? true : false;
                if (Point7 == true)
                {
                    cmd.Parameters.AddWithValue("@SafetyMessage", "Yes");
                    cmd.Parameters.AddWithValue("@SafetyMessageItems", txt_sftmsg.Text.ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SafetyMessage", "No");
                    cmd.Parameters.AddWithValue("@SafetyMessageItems", DBNull.Value);
                }


                //bool Point8 = (Page.Request.Form["BoxName8"] == "on") ? true : false;
                //if (Point8 == true)
                //{
                //    cmd.Parameters.AddWithValue("@SafetyAlert", "Yes");
                //    cmd.Parameters.AddWithValue("@SafetyAlertItems", txt_sftalert.Text.ToString());
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@SafetyAlert", "No");
                //    cmd.Parameters.AddWithValue("@SafetyAlertItems", DBNull.Value);
                //}

                cmd.Parameters.AddWithValue("@SafetyAlert", "No");
                cmd.Parameters.AddWithValue("@SafetyAlertItems", DBNull.Value);


                bool Point9 = (Page.Request.Form["BoxName9"] == "on") ? true : false;
                if (Point9 == true)
                {
                    cmd.Parameters.AddWithValue("@Ref_ActionItem", "Yes");
                    cmd.Parameters.AddWithValue("@Ref_ActionID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Ref_ActionItem", "No");
                    cmd.Parameters.AddWithValue("@Ref_ActionID", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@Panel2_Status", "Complete");
                cmd.Parameters.AddWithValue("@Panel2_TimeStamp", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                datasaved = true;
                string title = "Notifications :";
                string body = "Panel 2 Data Saved";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                datasaved = false;
            }

            return datasaved;
        }

        private string FindTBMPhotoId()
        {
            string aa = null;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string kk = null;
            string cmdString1 = "select Id,TBT_PhotoID from tbl_toolboxtalkdata where Id=(select max(Id)from tbl_toolboxtalkdata)";
            SqlCommand com1 = new SqlCommand(cmdString1, dbcl.Conn);
            SqlDataReader DR1 = com1.ExecuteReader();
            if (DR1.Read())
            {
                aa = DR1.GetValue(1).ToString();
                if (aa == null || aa == "")
                {
                    kk = "TBTP01";
                }
                else
                {
                    string bb = aa.Substring(5);
                    int k = Convert.ToInt32(bb);
                    k = k + 1;
                    string q = Convert.ToString(k);
                    kk = "TBTP0" + q;
                }
            }
            else
            {
                kk = "TBTP01";
            }
            dbcl.Conn.Close();
            return kk;
        }

        private Boolean UploadTBMImage()
        {
            Boolean imgsaved = false;

            DateTime d = DateTime.Now;
            string month = d.Month.ToString();
            string year = d.Year.ToString();
            string day = d.Day.ToString();
            string imgdate = day + month + year;

            // Check file exist or not
            if (TBM_FileUploader.PostedFile != null)
            {
                // Check the extension of image
                string extension = Path.GetExtension(TBM_FileUploader.FileName);
                if (extension.ToLower() == ".png" || extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg")
                {
                    Stream strm = TBM_FileUploader.PostedFile.InputStream;
                    using (var image = System.Drawing.Image.FromStream(strm))
                    {
                        string TBPhotoId = lbl_TBTID.Text.ToString();
                        lbl_tbtphotoid.Text = TBPhotoId;


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
                        string targetPath = Server.MapPath(@"\erp_images\TBTPhoto\") + TBPhotoId + "_" + imgdate + ".jpg";
                        TBM_FileUploader.SaveAs(Server.MapPath(@"\erp_images\TBTPhoto\") + TBPhotoId + "_" + imgdate + ".jpg");

                        //the below will be saved as database value
                        imglink = "\\erp_images\\TBTPhoto\\" + TBPhotoId + "_" + imgdate + ".jpg";
                        thumbImg.Save(targetPath, image.RawFormat);
                        imgfilename = TBPhotoId + "_" + imgdate + ".jpg";

                        // Print new Size of file (height or Width)
                        //lblaftr.Text = thumbImg.Size.ToString();

                        //Show Image instantly

                        ImgDisplay.ImageUrl = @"\erp_images\TBTPhoto\" + TBPhotoId + "_" + imgdate + ".jpg";

                        //on successfully image is saved
                        imgsaved = true;

                        string title = "Notifications :";
                        string body = "TBT Photograph Uploaded Successfully";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Kindly Select Appropriate File Type";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
            }
            return imgsaved;
        }

        //------------------ TBT Panel 3 Data Saver-------------------------//
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (UploadTBMImage() == true && TBT_Panel3Data() == true)
            {
                JOB_TableUpdate();
                uploadbuttonrow1.Visible = false;
                uploadbuttonrow2.Visible = false;
                UploadedPhotoRow1.Visible = true;
                UploadedPhotoRow2.Visible = true;

                SavePanel3Data.Visible = true;
            }
            else
            {
                TBM_FileUploader.Focus();
                TBM_FileUploader.BorderColor = Color.Red;

                UploadedPhotoRow1.Visible = false;
                UploadedPhotoRow2.Visible = false;


                SavePanel3Data.Visible = false;
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
                string CmdString = "UPDATE tbl_jobs set TBTID=@TBTID,TBT_Count=@TBT_Count where JOBID=@JOBID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@TBTID", lbl_TBTID.Text.ToString());
                cmd.Parameters.AddWithValue("@TBT_Count", "1");
                cmd.Parameters.AddWithValue("@JOBID", lbl_jobid.Text.ToString());
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                //string title = "Notifications :";
                //string body = "Panel 3 Data Saved";
                //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception)
            {
                string title = "Notifications :";
                string body = "TBT ID cannot be attched to JOBID";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
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
                string CmdString = "UPDATE tbl_toolboxtalkdata set TBTPhoto=@TBTPhoto,TBT_PhotoID=@TBT_PhotoID, TBT_PhotoFile=@TBT_PhotoFile,TBT_PhotoPath=@TBT_PhotoPath, TBT_PhotoTimeStamp=@TBT_PhotoTimeStamp where TBT_ID=@TBT_ID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@TBT_ID", lbl_TBTID.Text.ToString());
                cmd.Parameters.AddWithValue("@TBTPhoto", "Uploaded");
                cmd.Parameters.AddWithValue("@TBT_PhotoID", lbl_tbtphotoid.Text.ToString());
                cmd.Parameters.AddWithValue("@TBT_PhotoFile", imgfilename);
                cmd.Parameters.AddWithValue("@TBT_PhotoPath", imglink);
                cmd.Parameters.AddWithValue("@TBT_PhotoTimeStamp", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                datasaved = true;
                string title = "Notifications :";
                string body = "Panel 3 Data Saved";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                datasaved = false;
            }

            return datasaved;
        }

        protected void btn_saveTBTPhoto_Click(object sender, EventArgs e)
        {
            TBTPhotoUploaded.Visible = true;

            SavePanel3Data.Visible = false;
            TBTPhotographRow.Visible = false;

            TBTFinalStep.Visible = true;
        }

        protected void btn_finalstep_Click(object sender, EventArgs e)
        {
            Response.Redirect("csms_mainview.aspx");
        }
    }
}