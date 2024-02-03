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
    public partial class csm_tbtform : System.Web.UI.Page
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
                if (Session["USERID"] == null || Session["USERTYPE"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {

                    if (ActiveJobChecker())
                    {
                        BindTBTCheckLists();
                        //JOBIDDetails_Row.Visible = false;
                        //AttachedAttendanceRow.Visible = false;
                        //Default_Buttons.Visible = false;
                    }
                    else
                    {
                        //Default_Buttons.Visible = true;
                    }

                }
            }
        }

        protected void BindTBTCheckLists()
        {
            //string cmdString = "select * from tlb_personalresponsibilities order by Id";
            //dbcl.Sqlconnection();
            //dbcl.ConnectDb();
            //SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataTable dt = new DataTable();
            //da.Fill(dt);
            //chkbxrspons.DataTextField = "PS_ItemText";
            //chkbxrspons.DataValueField = "PS_ItemID";
            //chkbxrspons.DataSource = dt;
            //chkbxrspons.DataBind();
            //dbcl.DisconnectDb();
        }

        private bool ActiveJobChecker()
        {
            if (Session["WORKMAN"] != null && Session["REGION"] != null)
            {
                Int32 activeJobCount = CC.Find_ActiveJOBCountforINPunch(Session["WORKMAN"].ToString(), Session["REGION"].ToString());

                if (activeJobCount > 0)
                {
                    dbcl.FillCombo(DDL_JOBID, "SELECT CONCAT(JOBID, ' : ', CONVERT(VARCHAR, CreatedDate, 105)) AS JOBID FROM tbl_jobs WHERE Creator_Workman = '" + Session["WORKMAN"].ToString() + "' AND JOBID_Status = 'Active' AND [CreatedDate] >= DATEADD(DAY, -7, GETDATE()) ORDER BY CreatedDate DESC");

                    // Return true if there are active jobs
                    return true;
                }
                else
                {
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
                //CreateTBTIDRow.Visible = false;
                jobsupv.Visible = false;
                sftyofcrrow.Visible = false;
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
                    jobdate = parts[1]; // This will contain "2024-02-02"

                    // Now you can use jobID and dateStr as needed.
                }
                else
                {
                    // Handle the case where the string format is unexpected.
                }

                //sftysupvrow.Visible = true;
                //sftyofcrrow.Visible = true;

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

                    if (Panel1_flag == true && Panel2_flag == true && Panel3_flag == true)
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
                    string date2 = "";
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
                        //Panel2_flag = true;

                        //panel2heading.Visible = false;
                        //TBTPanel2.Visible = true;

                        //SavePanel2Data.Visible = false;
                        //TBT_ItemsSavedMsg.Visible = true;

                        //txt_sftyintrst.ReadOnly = true;
                        //txt_sftyintrst.Text = dt.Rows[0]["SafetyInterestItems"].ToString();

                        ////Checkbox for personal responsiblities
                        //string chkditemsfromdb = dt.Rows[0]["EmpPersnlItems"].ToString();

                        //string[] items = chkditemsfromdb.Split(',');
                        //for (int i = 0; i <= items.GetUpperBound(0); i++)
                        //{
                        //    ListItem currentcheckbox = chkbxrspons.Items.FindByText(items[i].ToString());
                        //    if (currentcheckbox != null)
                        //    {
                        //        currentcheckbox.Selected = true;
                        //    }
                        //}

                        //txt_sopno.ReadOnly = true;
                        //txt_sopno.Text = dt.Rows[0]["SOPNumber"].ToString();

                        //txt_hazards.ReadOnly = true;
                        //txt_hazards.Text = dt.Rows[0]["HazardMaterialItems"].ToString();

                        //txt_sftmsg.ReadOnly = true;
                        //txt_sftmsg.Text = dt.Rows[0]["SafetyMessageItems"].ToString();

                        ////txt_sftalert.ReadOnly = true;
                        ////txt_sftalert.Text = dt.Rows[0]["SafetyAlertItems"].ToString();
                    }
                    else
                    {
                        //panel2heading.Visible = true;
                        //TBTPanel2.Visible = true;

                        //SavePanel2Data.Visible = true;
                        //TBT_ItemsSavedMsg.Visible = false;

                        //txt_sftyintrst.ReadOnly = false;
                        //txt_sftyintrst.Text = "";

                        //txt_sopno.ReadOnly = false;
                        //txt_sopno.Text = "";

                        //txt_hazards.ReadOnly = false;
                        //txt_hazards.Text = "";

                        //txt_sftmsg.ReadOnly = false;
                        //txt_sftmsg.Text = "";

                        ////txt_sftalert.ReadOnly = false;
                        ////txt_sftalert.Text = "";

                        //TBT_ItemsSavedMsgHR.Visible = true;
                    }


                    string Panel3status = dt.Rows[0]["TBTPhoto"].ToString();
                    string filename = dt.Rows[0]["TBT_PhotoFile"].ToString();
                    if (Panel3status == "Uploaded")
                    {
                        //Panel3_flag = true;
                        //TBTPhotographRow.Visible = true;

                        //uploadbuttonrow1.Visible = false;
                        //uploadbuttonrow2.Visible = false;

                        //UploadedPhotoRow1.Visible = true;
                        //UploadedPhotoRow2.Visible = true;

                        //SavePanel3Data.Visible = false;

                        //TBTPhotoUploaded.Visible = true;

                        //string prefix = @"\erp_images\TBTPhoto\";
                        //ImgDisplay.ImageUrl = prefix + filename;
                    }
                    else
                    {
                        //TBTPhotographRow.Visible = true;

                        //uploadbuttonrow1.Visible = true;
                        //uploadbuttonrow2.Visible = true;

                        //UploadedPhotoRow1.Visible = false;
                        //UploadedPhotoRow2.Visible = false;

                        //SavePanel3Data.Visible = true;

                        //TBTPhotoUploaded.Visible = false;
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

        private void CheckforAttachedAttendnace()
        {
            string ddljobid = DDL_JOBID.SelectedItem.Text.ToString();
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
            string ddljobid = DDL_JOBID.SelectedItem.Text.ToString();
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

    }
}