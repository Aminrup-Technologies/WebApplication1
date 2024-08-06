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
using System.Globalization;

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
        public static Boolean Panel4_flag = false;
        public static Boolean Final_flag = false;

        DataTable temporaryDataTable = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {

                    if (ActiveJobChecker())
                    {
                        BindTBTCheckLists();

                        string CmdString1 = "select ActionType, PointID from ActionableItems order by PointID";
                        BindPriorityLevel(CmdString1);

                        AddDefaultActionableRecord();
                        //AddDefaultLastReviewRecord();

                        Default_Buttons.Visible = false;

                        TBT_Status_Panel_NoJOBDView();

                        Panel1_Tags_Hidden();
                        Panel2_Tags_Hidden();
                        Panel3_Tags_Hidden();
                        Panel4_Tags_Hidden();
                    }
                    else
                    {
                        string msg = "There is No ACTIVE jobid";
                        string noActiveJobIdNotificationScript = @"<script type='text/javascript'>
            new PNotify({
                title: 'Oh No !!!',
                text: '" + msg + @"',
                type: 'error',
                styling: 'bootstrap3'
            });
        </script>";

                        ClientScript.RegisterStartupScript(this.GetType(), "NoActiveJOBID_Notification", noActiveJobIdNotificationScript, false);
                    }

                }
            }
        }

        private void AddDefaultActionableRecord()
        {
            // Create a new DataTable
            DataTable dt = new DataTable();

            // Define the structure of the DataTable
            dt.TableName = "Actionables";
            dt.Columns.Add(new DataColumn("ActionableType", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionableDescription", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionableRemarks", typeof(string)));
            dt.Columns.Add(new DataColumn("Priority", typeof(string)));

            // Create a new DataRow and add it to the DataTable
            DataRow dr = dt.NewRow();
            dr["ActionableType"] = "Default Type";
            dr["ActionableDescription"] = "Default Description";
            dr["ActionableRemarks"] = "Default Remarks";
            dr["Priority"] = "Default Priority";
            dt.Rows.Add(dr);

            // Save the DataTable into ViewState
            ViewState["Actionables"] = dt;

            // Bind the GridView
            BindMyGridview();
        }

        private void AddDefaultLastReviewRecord()
        {
            // Create a new DataTable
            DataTable dt = new DataTable();

            // Define the structure of the DataTable
            dt.TableName = "LastActionables";
            dt.Columns.Add(new DataColumn("ActionableType", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionableDescription", typeof(string)));
            //dt.Columns.Add(new DataColumn("ActionableRemarks", typeof(string)));
            dt.Columns.Add(new DataColumn("Priority", typeof(string)));

            // Create a new DataRow and add it to the DataTable
            DataRow dr = dt.NewRow();
            dr["ActionableType"] = "N/A";
            dr["ActionableDescription"] = "No Records";
            //dr["ActionableRemarks"] = "No Records";
            dr["Priority"] = "N/A";
            dt.Rows.Add(dr);

            // Save the DataTable into ViewState
            ViewState["LastActionables"] = dt;

            // Bind the GridView
            BindMyGridLastview();
        }


        public void BindMyGridview()
        {
            if (ViewState["Actionables"] != null)
            {
                DataTable dt = (DataTable)ViewState["Actionables"];

                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    GridView1.Visible = true;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                else
                {
                    GridView1.Visible = false;
                }
            }
        }

        public void BindMyGridLastview()
        {
            if (ViewState["LastActionables"] != null)
            {
                DataTable dt = (DataTable)ViewState["LastActionables"];

                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    GridView2.Visible = true;
                    GridView2.DataSource = dt;
                    GridView2.DataBind();
                }
                else
                {
                    GridView2.Visible = false;
                }
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["Actionables"];
            DataRow dr = dtCurrentTable.Rows[e.RowIndex];
            dtCurrentTable.Rows.Remove(dr);

            // Check if there are no more rows in the DataTable
            if (dtCurrentTable.Rows.Count == 0)
            {
                // Add a dummy record
                DataRow dummyRow = dtCurrentTable.NewRow();
                // Assuming you have three columns in your DataTable: ActionableType, ActionableDescription, Priority
                dummyRow["ActionableType"] = "Default Type";
                dummyRow["ActionableDescription"] = "Default Description";
                dummyRow["ActionableRemarks"] = "Default Remarks";
                dummyRow["Priority"] = "Default Priority";
                dtCurrentTable.Rows.Add(dummyRow);
            }

            // Update the ViewState with the modified DataTable
            ViewState["Actionables"] = dtCurrentTable;

            // Exit edit mode (if it was in edit mode)
            GridView1.EditIndex = -1;

            // Rebind the GridView
            BindMyGridview();

            ScriptManager.RegisterStartupScript(this, GetType(), "OpenActionableTab", "OpenActionableTab();", true);
        }



        private void BindPriorityLevel(string cmdString)
        {
            dbcl.Sqlconnection();
            using (SqlConnection connection = dbcl.Conn)
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(cmdString, connection))
                    {
                        command.CommandType = CommandType.Text;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DDL_AcType.DataSource = reader;
                            DDL_AcType.DataTextField = "ActionType";
                            DDL_AcType.DataValueField = "PointID";
                            DDL_AcType.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string noActiveJobIdNotificationScript = @"<script type='text/javascript'>
            new PNotify({
                title: 'Oh No !!!',
                text: '" + ex.Message + @"',
                type: 'error',
                styling: 'bootstrap3'
            });
        </script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "NoActiveJOBID_Notification", noActiveJobIdNotificationScript, false);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            DDL_AcType.Items.Insert(0, "Please Select Option");
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

        private bool ActiveJobChecker()
        {
            if (Session["WORKMAN"] != null && Session["REGION"] != null)
            {
                Int32 activeJobCount = CC.Find_ActiveJOBCountforINPunch(Session["WORKMAN"].ToString(), Session["REGION"].ToString());

                if (activeJobCount > 0)
                {
                    dbcl.FillCombo(DDL_JOBID, "SELECT CONCAT(JOBID, ' : ', CONVERT(VARCHAR, CreatedDate, 105)) AS JOBID FROM tbl_jobs WHERE Creator_Workman = '" + Session["WORKMAN"].ToString() + "' AND JOBID_Status = 'Active' AND [CreatedDate] >= DATEADD(DAY, -3, GETDATE())  and CSM_Documents='Yes' ORDER BY CreatedDate DESC");

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

            Panel1_flag = false;
            Panel2_flag = false;
            Panel3_flag = false;
            Panel4_flag = false;
            Final_flag = false;

            if (DDL_JOBID.SelectedIndex == 0)
            {
                Default_Buttons.Visible = true;

                TBT_Status_Panel_NoJOBDView();

                Panel1_Tags_Hidden();
                Panel2_Tags_Hidden();
                Panel3_Tags_Hidden();
                Panel4_Tags_Hidden();


                string JOBID_Selector_script = @"<script type='text/javascript'>
                            new PNotify({
                                title: 'Guide',
                                text: 'Please select a valid & active JOBID',
                                type: 'info',
                                styling: 'bootstrap3'
                            });
                        </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "JOBID_Selector_Notification", JOBID_Selector_script, false);
            }
            else
            {
                string CmdString = "select FullName, WorkmanSL from tbl_Employee_Mustertable where SkillDesignation = 'SAFETY SUPERVISOR' and WorkStatus = 'Active' and WorkRegion = '" + Session["REGION"].ToString() + "' order by Id";
                Bind_SafetySupervisors(CmdString);

                string CmdString1 = "select FullName, WorkmanSL from tbl_Employee_Mustertable where SkillDesignation = 'SAFETY OFFICER' and WorkStatus = 'Active' and WorkRegion = '" + Session["REGION"].ToString() + "' order by Id";
                Bind_SafetyOfficer(DDL_SftyOfcr, CmdString1);

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

                //Check if TBT is already added aganist this JOBID

                bool exists = CC.TBTRecordExist(ddljobid, jobdate);
                if (exists == true)
                {
                    //Bind_JOBIDDetails(ddljobid, jobdate);
                    Bind_TBTDetails(ddljobid, jobdate);

                    //Function call to bind the last TBT details
                    string workState = Session["STATE"].ToString();
                    string workRegion = Session["REGION"].ToString();
                    string workCompany = Session["COMPANY_CODE"].ToString();
                    string refCSMFormName = "CSM-TBT";
                    string submittedByWrk = Session["WORKMAN"].ToString();
                    DataTable feedbackData = GetFeedbackData(workState, workRegion, workCompany, refCSMFormName, submittedByWrk);
                    GridView2.DataSource = feedbackData;
                    GridView2.DataBind();
                }
                else
                {
                    // If TBT record does not exist, bind JOBID details and show related rows
                    Bind_JOBIDDetails(ddljobid, jobdate);
                    //Bind_TBTDetails(ddljobid, jobdate);

                    Panel1_Success.Visible = false;
                    Img_Success1.Visible = false;
                    Img_Cross1.Visible = false;
                    lbl_panel1_msg.Text = "";

                    Panel2_Success.Visible = true;
                    Img_Success2.Visible = false;
                    Img_Cross2.Visible = false;
                    lbl_panel2_msg.Text = "Complete Step-1";

                    Panel3_Success.Visible = true;
                    Img_Success3.Visible = false;
                    Img_Cross3.Visible = false;
                    lbl_panel3_msg.Text = "Complete Step-1 & Step-2";

                    Panel4_Success.Visible = true;
                    Img_Success4.Visible = false;
                    Img_Cross4.Visible = false;
                    lbl_panel4_msg.Text = "Complete Step-1, Step-2 & Step-3";

                    lbl_TBTID.Text = "";
                    lbl_TBTID.Visible = false;


                    // Popup to notify user of TBT completion
                    string title = "Notifications :";
                    string body = "";

                    if (Panel1_flag == true && Panel2_flag == true && Panel3_flag == true && Panel4_flag == true)
                    {
                        body = "You have already added a Tool Box talk against the selected JOBID";
                    }
                    else if (Panel1_flag == true && Panel2_flag != true && Panel3_flag != true && Panel4_flag != true)
                    {
                        body = "ID Created, Incomplete Submission....!!";
                    }
                    else if (Panel1_flag == true && Panel2_flag == true && Panel3_flag != true && Panel4_flag != true)
                    {
                        body = "Photograph Not uploaded...!!!";
                    }
                    else if (Panel1_flag == true && Panel2_flag == true && Panel3_flag == true && Panel4_flag != true)
                    {
                        body = "No Actionables Added...!!!";
                    }
                    else
                    {
                        body = "TBT Incomplete....!!";
                    }

                    // Display PNotify notification
                    string script = @"<script type='text/javascript'>
                            new PNotify({
                                title: '" + title + @"',
                                text: '" + body + @"',
                                type: 'info',
                                styling: 'bootstrap3'
                            });
                        </script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "PNotifyNotification", script, false);
                }
            }
        }

        public DataTable GetFeedbackData(string workState, string workRegion, string workCompany, string refCSMFormName, string submittedByWrk)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = dbcl.Conn)
            {
                string query = "SELECT AcnID, RefCSMFormID, AcnType, AcnDescription, PriorityLevel, AssignedToName, CurrentStatus, TargetCompletionDate FROM CSM_Feedbacks WHERE WorkState = @WorkState AND WorkrRegion = @WorkrRegion AND WorkCompany = @WorkCompany AND RefCSMFormName = @RefCSMFormName AND SubmittedByWrk = @SubmittedByWrk and DateSubmitted >= DATEADD(DAY, -3, GETDATE())";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@WorkState", workState);
                    command.Parameters.AddWithValue("@WorkrRegion", workRegion);
                    command.Parameters.AddWithValue("@WorkCompany", workCompany);
                    command.Parameters.AddWithValue("@RefCSMFormName", refCSMFormName);
                    command.Parameters.AddWithValue("@SubmittedByWrk", submittedByWrk);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }

        private void Panel1_Tags_Hidden()
        {
            Panel1_Buttons.Visible = false;

            //--------- Hide all Rows in Panel1-----------
            JOBIDDetails_Row.Visible = false;
            AttachedAttendanceRow.Visible = false;
            locationrow.Visible = false;
            deptrow.Visible = false;
            jobsupv.Visible = false;
            LineManagerRow.Visible = false;
            sftyofcrrow.Visible = false;
            sftysupvrow.Visible = false;
            supvrow.Visible = false;
            ContractEmployeesrow.Visible = false;

            Panel1_Success.Visible = true;
            Img_Success1.Visible = false;
            Img_Cross1.Visible = true;
            lbl_panel1_msg.Text = "No JOBID Selected";

            lbl_TBTID.Visible = false;
        }

        private void Panel2_Tags_Hidden()
        {
            Panel2_Buttons.Visible = false;
            //--------- Hide all Rows in Panel2-----------
            GridView2.Visible = false;
            Annexure0.Visible = false;
            Annexure1.Visible = false;
            Annexure2.Visible = false;
            Annexure3.Visible = false;
            Annexure4.Visible = false;
            Annexure5.Visible = false;
            Annexure6.Visible = false;
            Annexure7.Visible = false;
            //Annexure8.Visible = false;

            Panel2_Success.Visible = true;
            Img_Success2.Visible = false;
            Img_Cross2.Visible = true;
            lbl_panel2_msg.Text = "No JOBID Selected";
        }

        private void Panel3_Tags_Hidden()
        {
            Panel3_Buttons.Visible = false;

            //--------- Hide all Rows in Panel3-----------
            TBTPhotographRow.Visible = false;
            //ImgDisplay.ImageUrl = @"\erp_images\ProfilePhoto\No_Image.jpg";
            lbl_tbtphotoid.Text = "";
            lbl_tbtphotoid.Visible = false;

            Panel3_Success.Visible = true;
            Img_Success3.Visible = false;
            Img_Cross3.Visible = true;
            lbl_panel3_msg.Text = "No JOBID Selected";
        }

        private void Panel4_Tags_Hidden()
        {
            Panel4_Buttons.Visible = false;

            //--------- Hide all Rows in Panel4-----------
            Acn_Type_row.Visible = false;
            Acn_Descp_row.Visible = false;
            Acn_lvl_row.Visible = false;
            Acn_remarks_row.Visible = false;
            Acn_btns_row.Visible = false;

            Acn_tempgrid_row.Visible = false;
            Panel4_Buttons.Visible = false;
            Panel4_Success.Visible = true;
            Img_Success4.Visible = false;
            Img_Cross4.Visible = true;
            lbl_panel4_msg.Text = "No JOBID Selected";
        }

        private void TBT_Status_Panel_NoJOBDView()
        {
            Label1.Text = "Pending";
            Label1.ForeColor = Color.Red;
            Label2.Text = "Pending";
            Label2.ForeColor = Color.Red;
            Label3.Text = "Pending";
            Label3.ForeColor = Color.Red;
            Label4.Text = "Pending";
            Label4.ForeColor = Color.Red;
            //Label5.Text = "Pending";
            //Label5.ForeColor = Color.Red;
            LabelOverall.Text = "Pending";
            LabelOverall.ForeColor = Color.Red;
        }

        private void Bind_JOBIDDetails(string jobid, string date)
        {
            JOBIDDetails_Row.Visible = true;

            try
            {
                string query = "select CreatedDate, Creator_Site, Creator_Workman, Creator_Region, Creator_Company, Creator_Name, Creator_SiteCode, WorkOrderNo, JOBID, JOB_Region, JOB_Company, JOB_Site, JOB_SiteCode, JOB_InchargeWrk, JOB_InchargeName, JOB_Dept, JOB_Location, JOB_Shift, JOB_PermitNo from tbl_jobs where JOBID=@JOBID and CreatedDate=@CreatedDate";
                SqlParameter[] pram = {
                                          new SqlParameter("@JOBID",jobid),
                                          new SqlParameter("@CreatedDate",date),
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
                    txt_supv.Text = supv;

                    lbl_crtrsitecode.Text = dt.Rows[0]["Creator_SiteCode"].ToString();
                    lbl_wrkordr.Text = dt.Rows[0]["WorkOrderNo"].ToString();
                    lbl_jobid.Text = dt.Rows[0]["JOBID"].ToString();
                    lbl_jobrgn.Text = dt.Rows[0]["JOB_Region"].ToString();
                    lbl_jobcompay.Text = dt.Rows[0]["JOB_Company"].ToString();
                    lbl_jobsite.Text = dt.Rows[0]["JOB_Site"].ToString();
                    lbl_jobsitecode.Text = dt.Rows[0]["JOB_SiteCode"].ToString();
                    lbl_inchargewrk.Text = dt.Rows[0]["JOB_InchargeWrk"].ToString();

                    string incharge = dt.Rows[0]["JOB_InchargeName"].ToString();
                    lbl_inchargename.Text = incharge;
                    txt_incharge.Text = incharge;

                    string dept = dt.Rows[0]["JOB_Dept"].ToString();
                    lbl_dept.Text = dept;
                    txt_dept.Text = dept;

                    string loc = dt.Rows[0]["JOB_Location"].ToString();
                    lbl_jobloc.Text = loc;
                    txt_location.Text = loc;

                    lbl_jobshift.Text = dt.Rows[0]["JOB_Shift"].ToString();
                    lbl_permitno.Text = dt.Rows[0]["JOB_PermitNo"].ToString();

                    JOBIDDetails_Row.Visible = true;
                    AttachedAttendanceRow.Visible = true;
                    locationrow.Visible = true;
                    deptrow.Visible = true;
                    jobsupv.Visible = true;
                    LineManagerRow.Visible = true;
                    sftyofcrrow.Visible = true;
                    sftysupvrow.Visible = true;
                    supvrow.Visible = true;
                    

                    CheckforAttachedAttendnace(jobid, date);
                    ContractEmployeesrow.Visible = true;
                }
            }
            catch (Exception ex)
            {
                string title = "634 : Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (TBT_Panel1Data() == true)
            {
                Panel1_Buttons.Visible = false;
                Panel1_Success.Visible = true;
                Img_Success1.Visible = true;
                Img_Cross1.Visible = false;
                lbl_panel1_msg.Text = "TBT ID Created Successfully";

                Panel2_Buttons.Visible = true;
                Panel2_Success.Visible = false;
                Img_Success2.Visible = false;
                Img_Cross2.Visible = false;

                string title = "Notifications :";
                string body = "Panel 1 Data Saved";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                Panel2_Buttons.Visible = true;
                //--------- Hide all Rows in Panel2-----------
                GridView2.Visible = true;
                Annexure0.Visible = true;
                Annexure1.Visible = true;
                Annexure2.Visible = true;
                Annexure3.Visible = true;
                Annexure4.Visible = true;
                Annexure5.Visible = true;
                Annexure6.Visible = true;
                Annexure7.Visible = true;
                //Annexure8.Visible = false;

                Panel2_Success.Visible = false;
                Img_Success2.Visible = false;
                Img_Cross2.Visible = false;
                lbl_panel2_msg.Text = "";

                Label1.Text = "Completed";
                Label1.ForeColor = Color.Green;
                Label2.Text = "Pending";
                Label2.ForeColor = Color.Red;
                Label3.Text = "Pending";
                Label3.ForeColor = Color.Red;
                Label4.Text = "Pending";
                Label4.ForeColor = Color.Red;
                LabelOverall.Text = "Pending";
                LabelOverall.ForeColor = Color.Red;

                ScriptManager.RegisterStartupScript(this, GetType(), "OpenItemsTab", "OpenItemsTab();", true);
            }
            else
            {
                //jobselectionpanel.Visible = true;
                Panel1_Buttons.Visible = true;
                Panel1_Success.Visible = false;
                //Panel1_SuccessHR.Visible = false;


                //panel2heading.Visible = false;
                //TBTPanel2.Visible = false;
                Panel2_Buttons.Visible = false;

                ScriptManager.RegisterStartupScript(this, GetType(), "OpenJOBIDTab", "OpenJOBIDTab();", true);
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
                lbl_TBTID.Visible = true;
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

        private void Bind_TBTDetails(string jobid, string jobdate)
        {
            lbl_jobid.Text = jobid;
            try
            {
                string query = "select * from tbl_toolboxtalkdata where Ref_JOBID=@Ref_JOBID and Ref_JOBDate=@Ref_JOBDate";
                SqlParameter[] pram = {
                                          new SqlParameter("@Ref_JOBID",jobid),
                                          new SqlParameter("@Ref_JOBDate",jobdate),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);

                if (dt.Rows.Count > 0)
                {
                    string Panel1status = dt.Rows[0]["Panel1_Status"].ToString();
                    string Panel2status = dt.Rows[0]["Panel2_Status"].ToString();
                    string Panel3status = dt.Rows[0]["TBTPhoto"].ToString();
                    string Panel4status = dt.Rows[0]["ActionablesAdded"].ToString();
                    string Finalstatus = dt.Rows[0]["FinalClick"].ToString();


                    if (Panel1status == "Complete" && Panel2status == "Pending" && Panel3status == "Pending" && Panel4status == "0" && Finalstatus == "0")
                    {
                        Panel1_flag = true;
                        Panel2_flag = false;
                        Panel3_flag = false;
                        Panel4_flag = false;
                        Final_flag = false;

                        Label1.Text = "Completed";
                        Label1.ForeColor = Color.Green;
                        Label2.Text = "Pending";
                        Label2.ForeColor = Color.Red;
                        Label3.Text = "Pending";
                        Label3.ForeColor = Color.Red;
                        Label4.Text = "Pending";
                        Label4.ForeColor = Color.Red;
                        LabelOverall.Text = "Pending";
                        LabelOverall.ForeColor = Color.Red;

                        lbl_TBTID.Text = dt.Rows[0]["TBT_ID"].ToString();
                        lbl_TBTID.Visible = true;

                        Panel1_Buttons.Visible = false;
                        Panel1_Success.Visible = true;
                        Img_Success1.Visible = true;
                        Img_Cross1.Visible = false;
                        lbl_panel1_msg.Text = "Step-1 : Completed";

                        locationrow.Visible = true;
                        txt_location.Text = dt.Rows[0]["TBT_Location"].ToString();

                        deptrow.Visible = true;
                        txt_dept.Text = dt.Rows[0]["TBT_Dept"].ToString();

                        jobsupv.Visible = true;
                        txt_supv.Text = dt.Rows[0]["TBT_SupvName"].ToString();

                        LineManagerRow.Visible = true;
                        txt_linemanager.Text = dt.Rows[0]["LineManager"].ToString();

                        string sftysupv = dt.Rows[0]["SafetySupvWrk"].ToString();
                        DDL_SftySupv.SelectedValue = sftysupv;
                        DDL_SftySupv.Enabled = false;
                        sftysupvrow.Visible = true;

                        sftyofcrrow.Visible = true;
                        string sftyofcsupv = dt.Rows[0]["SafetyOfficerWrk"].ToString();
                        DDL_SftyOfcr.SelectedValue = sftyofcsupv;
                        DDL_SftyOfcr.Enabled = false;

                        supvrow.Visible = true;
                        txt_incharge.Text = dt.Rows[0]["AreaInchargeName"].ToString();

                        ContractEmployeesrow.Visible = true;
                        txt_cntrctemp.ReadOnly = true;
                        txt_cntrctemp.Text = dt.Rows[0]["ContractEmployees"].ToString();

                        Panel2_Buttons.Visible = true;
                        //--------- Hide all Rows in Panel2-----------
                        GridView2.Visible = true;
                        Annexure0.Visible = true;
                        Annexure1.Visible = true;
                        Annexure2.Visible = true;
                        Annexure3.Visible = true;
                        Annexure4.Visible = true;
                        Annexure5.Visible = true;
                        Annexure6.Visible = true;
                        Annexure7.Visible = true;
                        //Annexure8.Visible = false;

                        Panel2_Success.Visible = false;
                        Img_Success2.Visible = false;
                        Img_Cross2.Visible = false;
                        lbl_panel2_msg.Text = "";

                        ScriptManager.RegisterStartupScript(this, GetType(), "OpenAnnexureTab", "OpenItemsTab();", true);
                    }

                    else if (Panel1status == "Complete" && Panel2status == "Complete" && Panel3status == "Pending" && Panel4status == "0" && Finalstatus == "0")
                    {
                        Panel1_flag = true;
                        Panel2_flag = true;
                        Panel3_flag = false;
                        Panel4_flag = false;
                        Final_flag = false;

                        lbl_TBTID.Text = dt.Rows[0]["TBT_ID"].ToString();
                        lbl_TBTID.Visible = true;

                        Label1.Text = "Completed";
                        Label1.ForeColor = Color.Green;
                        Label2.Text = "Completed";
                        Label2.ForeColor = Color.Green;
                        Label3.Text = "Pending";
                        Label3.ForeColor = Color.Red;
                        Label4.Text = "Pending";
                        Label4.ForeColor = Color.Red;
                        //Label5.Text = "Pending";
                        //Label5.ForeColor = Color.Red;
                        LabelOverall.Text = "Pending";
                        LabelOverall.ForeColor = Color.Red;

                        Panel1_Buttons.Visible = false;
                        Panel1_Success.Visible = true;
                        Img_Success1.Visible = true;
                        Img_Cross1.Visible = false;
                        lbl_panel1_msg.Text = "Step-1 : Completed";

                        Panel2_Buttons.Visible = false;
                        Panel2_Success.Visible = true;
                        Img_Success2.Visible = true;
                        Img_Cross2.Visible = false;
                        lbl_panel2_msg.Text = "Step-2 : Completed";

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

                        ////txt_sftalert.ReadOnly = true;
                        ////txt_sftalert.Text = dt.Rows[0]["SafetyAlertItems"].ToString();

                        Panel3_Buttons.Visible = true;
                        Panel3_Success.Visible = false;
                        Img_Success3.Visible = false;
                        Img_Cross3.Visible = false;
                        lbl_panel3_msg.Text = "";

                        TBTPhotographRow.Visible = true;
                        uploadbuttonrow1.Visible = true;
                        uploadbuttonrow2.Visible = true;


                        ScriptManager.RegisterStartupScript(this, GetType(), "OpenPhotoTab", "OpenPhotoTab();", true);
                    }
                    else if (Panel1status == "Complete" && Panel2status == "Complete" && Panel3status == "Uploaded" && Panel4status == "0" && Finalstatus == "0")
                    {
                        string filename = dt.Rows[0]["TBT_PhotoFile"].ToString();
                        Panel1_flag = true;
                        Panel2_flag = true;
                        Panel3_flag = true;
                        Panel4_flag = false;
                        Final_flag = false;

                        lbl_TBTID.Text = dt.Rows[0]["TBT_ID"].ToString();
                        lbl_TBTID.Visible = true;

                        Label1.Text = "Completed";
                        Label1.ForeColor = Color.Green;
                        Label2.Text = "Completed";
                        Label2.ForeColor = Color.Green;
                        Label3.Text = "Completed";
                        Label3.ForeColor = Color.Green;
                        Label4.Text = "Pending";
                        Label4.ForeColor = Color.Red;
                        LabelOverall.Text = "Pending";
                        LabelOverall.ForeColor = Color.Red;


                        TBTPhotographRow.Visible = true;

                        uploadbuttonrow1.Visible = false;
                        uploadbuttonrow2.Visible = false;

                        UploadedPhotoRow1.Visible = true;
                        UploadedPhotoRow2.Visible = true;

                        Panel1_Buttons.Visible = false;
                        Panel1_Success.Visible = true;
                        Img_Success1.Visible = true;
                        Img_Cross1.Visible = false;
                        lbl_panel1_msg.Text = "Step-1 : Completed";

                        Panel2_Buttons.Visible = false;
                        Panel2_Success.Visible = true;
                        Img_Success2.Visible = true;
                        Img_Cross2.Visible = false;
                        lbl_panel2_msg.Text = "Step-2 : Completed";

                        Panel3_Buttons.Visible = false;
                        Panel3_Success.Visible = true;
                        Img_Success3.Visible = true;
                        Img_Cross3.Visible = false;
                        lbl_panel3_msg.Visible = true;
                        lbl_panel3_msg.Text = "Step-3 : Completed";

                        ImgDisplay.Visible = true;
                        string prefix = @"\erp_images\TBTPhoto\";
                        ImgDisplay.ImageUrl = prefix + filename;



                        Panel4_Buttons.Visible = false;

                        //--------- Hide all Rows in Panel4-----------
                        Acn_Type_row.Visible = true;
                        Acn_Descp_row.Visible = true;
                        Acn_lvl_row.Visible = true;
                        Acn_remarks_row.Visible = true;
                        Acn_btns_row.Visible = true;

                        Acn_tempgrid_row.Visible = true;

                        Panel4_Success.Visible = false;
                        Img_Success4.Visible = false;
                        Img_Cross4.Visible = false;
                        lbl_panel4_msg.Visible = false;
                        lbl_panel4_msg.Text = "";

                        ScriptManager.RegisterStartupScript(this, GetType(), "OpenActionableTab", "OpenActionableTab();", true);
                    }

                    else if (Panel1status == "Complete" && Panel2status == "Complete" && Panel3status == "Uploaded" && Panel4status != "0" && Finalstatus == "0")
                    {
                        Panel1_flag = true;
                        Panel2_flag = true;
                        Panel3_flag = true;
                        Panel4_flag = true;
                        Final_flag = false;

                        lbl_TBTID.Text = dt.Rows[0]["TBT_ID"].ToString();
                        lbl_TBTID.Visible = true;

                        Panel1_Buttons.Visible = false;
                        Panel1_Success.Visible = true;
                        Img_Success1.Visible = true;
                        Img_Cross1.Visible = false;
                        lbl_panel1_msg.Text = "Step-1 : Completed";

                        Panel2_Buttons.Visible = false;
                        Panel2_Success.Visible = true;
                        Img_Success2.Visible = true;
                        Img_Cross2.Visible = false;
                        lbl_panel2_msg.Text = "Step-2 : Completed";

                        Panel3_Buttons.Visible = false;
                        Panel3_Success.Visible = true;
                        Img_Success3.Visible = true;
                        Img_Cross3.Visible = false;
                        lbl_panel3_msg.Visible = true;
                        lbl_panel3_msg.Text = "Step-3 : Completed";


                        Label1.Text = "Completed";
                        Label1.ForeColor = Color.Green;
                        Label2.Text = "Completed";
                        Label2.ForeColor = Color.Green;
                        Label3.Text = "Completed";
                        Label3.ForeColor = Color.Green;
                        Label4.Text = "Completed";
                        Label4.ForeColor = Color.Green;
                        LabelOverall.Text = "Pending";
                        LabelOverall.ForeColor = Color.Red;


                        Panel4_Success.Visible = true;
                        Img_Success4.Visible = true;
                        Img_Cross4.Visible = false;
                        lbl_panel4_msg.Visible = true;
                        lbl_panel4_msg.Text = "Step-4 : Completed";

                        TBTFinalStep.Visible = true;

                        ScriptManager.RegisterStartupScript(this, GetType(), "OpenFinalTab", "OpenFinalTab();", true);

                    }
                    else if (Panel1status == "Complete" && Panel2status == "Complete" && Panel3status == "Uploaded" && Panel4status != "0" && Finalstatus != "0")
                    {
                        lbl_TBTID.Text = dt.Rows[0]["TBT_ID"].ToString();
                        lbl_TBTID.Visible = true;

                        Panel1_flag = true;
                        Panel2_flag = true;
                        Panel3_flag = true;
                        Panel4_flag = true;
                        Final_flag = true;

                        Label1.Text = "Completed";
                        Label1.ForeColor = Color.Green;
                        Label2.Text = "Completed";
                        Label2.ForeColor = Color.Green;
                        Label3.Text = "Completed";
                        Label3.ForeColor = Color.Green;
                        Label4.Text = "Completed";
                        Label4.ForeColor = Color.Green;
                        LabelOverall.Text = "Completed";
                        LabelOverall.ForeColor = Color.Green;

                        Panel1_Buttons.Visible = false;
                        Panel1_Success.Visible = true;
                        Img_Success1.Visible = true;
                        Img_Cross1.Visible = false;
                        lbl_panel1_msg.Text = "Step-1 : Completed";

                        Panel2_Buttons.Visible = false;
                        Panel2_Success.Visible = true;
                        Img_Success2.Visible = true;
                        Img_Cross2.Visible = false;
                        lbl_panel2_msg.Text = "Step-2 : Completed";

                        Panel3_Buttons.Visible = false;
                        Panel3_Success.Visible = true;
                        Img_Success3.Visible = true;
                        Img_Cross3.Visible = false;
                        lbl_panel3_msg.Visible = true;
                        lbl_panel3_msg.Text = "Step-3 : Completed";

                        Panel4_Buttons.Visible = true;
                        Panel4_Success.Visible = true;
                        Img_Success4.Visible = true;
                        Img_Cross4.Visible = false;
                        lbl_panel4_msg.Text = "Step-4 : Completed";

                        Panel4_Buttons.Visible = false;

                        //--------- Hide all Rows in Panel4-----------
                        Acn_Type_row.Visible = false;
                        Acn_Descp_row.Visible = false;
                        Acn_lvl_row.Visible = false;
                        Acn_remarks_row.Visible = false;
                        Acn_btns_row.Visible = false;

                        Acn_tempgrid_row.Visible = false;

                        ScriptManager.RegisterStartupScript(this, GetType(), "OpenFinalTab", "OpenFinalTab();", true);
                    }
                    else
                    {
                        Panel1_flag = false;
                        Panel2_flag = false;
                        Panel3_flag = false;
                        Panel4_flag = false;
                        Final_flag = false;

                        ScriptManager.RegisterStartupScript(this, GetType(), "OpenFinalTab", "OpenFinalTab();", true);
                    }

                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                string title = "1171 : Notifications :";
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

        private void CheckforAttachedAttendnace1()
        {
            string jobID = DDL_JOBID.SelectedItem.Text.ToString();
            string ddljobid = "";
            string jobdate = "";
            string[] parts = jobID.Split(new string[] { " : " }, StringSplitOptions.None);

            if (parts.Length == 2)
            {
                ddljobid = parts[0]; // This will contain "JOB0095467"
                jobdate = parts[1]; // This will contain "2024-02-02"
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
                //Bind_AttendanceGridView();
            }
            dbcl.DisconnectDb();
        }


        private void CheckforAttachedAttendnace(string jobid, string date)
        {
            try
            {
                using (SqlConnection connection = dbcl.Conn)
                {
                    connection.Open();

                    string cmdString = "SELECT COUNT(Id) FROM tbl_attendance WHERE JOBID = @JobID AND Creator_Workman = @Creator AND CreatedDate = @CreatedDate";
                    using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                    {
                        cmd.Parameters.AddWithValue("@JobID", jobid);
                        cmd.Parameters.AddWithValue("@CreatedDate", date);
                        cmd.Parameters.AddWithValue("@Creator", Session["WORKMAN"].ToString());

                        int count = (int)cmd.ExecuteScalar();
                        AttachedAttendanceRow.Visible = (count > 0);

                        if (AttachedAttendanceRow.Visible)
                        {
                            Bind_AttendanceGridView(jobid, date);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exception
                // Log or display error message
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                // Log or display error message
            }
        }


        private void Bind_AttendanceGridView(string jobid, string date)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string CmdString = "Select EmployeeWrk,EmployeeName,EmpDesignation,GatePassNo from tbl_attendance where Creator_Workman='" + Session["WORKMAN"].ToString() + "' and JOBID='" + jobid + "' and CreatedDate='"+date+"' and AttendanceStatus = 'Entry' order by Id";

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
                Panel1_Buttons.Visible = true;
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

        protected void btn_savetbtdata_Click(object sender, EventArgs e)
        {
            if (TBT_Panel2Data() == true)
            {
                Panel2_Buttons.Visible = false;

                Panel2_Success.Visible = true;
                Img_Success2.Visible = true;
                Img_Cross2.Visible = false;
                lbl_panel2_msg.Text = "Step-2 : Completed";

                TBTPhotographRow.Visible = true;
                uploadbuttonrow1.Visible = true;
                uploadbuttonrow2.Visible = true;

                UploadedPhotoRow1.Visible = false;
                UploadedPhotoRow2.Visible = false;

                Panel3_Buttons.Visible = true;

                ImgDisplay.ImageUrl = @"\erp_images\ProfilePhoto\No_Image.jpg";
                lbl_tbtphotoid.Text = "";
                lbl_tbtphotoid.Visible = false;

                Panel3_Success.Visible = false;
                Img_Success3.Visible = false;
                Img_Cross3.Visible = false;
                lbl_panel3_msg.Text = "";

                Label1.Text = "Completed";
                Label1.ForeColor = Color.Green;
                Label2.Text = "Completed";
                Label2.ForeColor = Color.Green;
                Label3.Text = "Pending";
                Label3.ForeColor = Color.Red;
                Label4.Text = "Pending";
                Label4.ForeColor = Color.Red;
                LabelOverall.Text = "Pending";
                LabelOverall.ForeColor = Color.Red;


                ScriptManager.RegisterStartupScript(this, GetType(), "OpenPhotoTab", "OpenPhotoTab();", true);
            }
            else
            {
                //panel2heading.Visible = true;
                //TBTPanel2.Visible = true;
                Panel2_Buttons.Visible = true;

                Panel2_Success.Visible = false;


                TBTPhotographRow.Visible = false;

                string title = "Notifications :";
                string body = "Panel 2 Data NOT Saved or TBTID NOT attched";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
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
                    cmd.Parameters.AddWithValue("@PrevActionItem", "Yes");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PrevActionItem", "No");
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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (UploadTBMImage() == true && TBT_Panel3Data() == true)
            {
                
                uploadbuttonrow1.Visible = false;
                uploadbuttonrow2.Visible = false;
                UploadedPhotoRow1.Visible = true;
                UploadedPhotoRow2.Visible = true;

                Panel3_Buttons.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "OpenPhotoTab", "OpenPhotoTab();", true);
            }
            else
            {
                TBM_FileUploader.Focus();
                TBM_FileUploader.BorderColor = Color.Red;

                UploadedPhotoRow1.Visible = false;
                UploadedPhotoRow2.Visible = false;


                Panel3_Buttons.Visible = false;
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

        private Boolean UploadTBMImage1()
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

        private Boolean UploadTBMImage()
        {
            Boolean imgsaved = false;

            DateTime d = DateTime.Now;
            string month = d.Month.ToString();
            string year = d.Year.ToString();
            string day = d.Day.ToString();
            string imgdate = day + month + year;

            // Define the target folder path
            string targetFolderPath = Server.MapPath(@"\erp_images\TBTPhoto\");

            // Check if the target folder exists, if not, create it
            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
            }

            // Check file exist or not
            if (TBM_FileUploader.PostedFile != null)
            {
                // Check the extension of the image
                string extension = Path.GetExtension(TBM_FileUploader.FileName);
                if (extension.ToLower() == ".png" || extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg")
                {
                    Stream strm = TBM_FileUploader.PostedFile.InputStream;
                    using (var image = System.Drawing.Image.FromStream(strm))
                    {
                        string TBPhotoId = lbl_TBTID.Text.ToString();
                        lbl_tbtphotoid.Text = TBPhotoId;

                        int newWidth = 440; // New Width of Image in Pixel
                        int newHeight = 540; // New Height of Image in Pixel
                        var thumbImg = new Bitmap(newWidth, newHeight);
                        var thumbGraph = Graphics.FromImage(thumbImg);

                        thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                        thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                        thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        var imgRectangle = new Rectangle(0, 0, newWidth, newHeight);
                        thumbGraph.DrawImage(image, imgRectangle);

                        // Save the file to the target folder
                        string targetPath = Path.Combine(targetFolderPath, TBPhotoId + "_" + imgdate + ".jpg");
                        TBM_FileUploader.SaveAs(targetPath);

                        // Set the image link for database
                        imglink = "\\erp_images\\TBTPhoto\\" + TBPhotoId + "_" + imgdate + ".jpg";
                        thumbImg.Save(targetPath, image.RawFormat);
                        imgfilename = TBPhotoId + "_" + imgdate + ".jpg";

                        // Show the image instantly
                        ImgDisplay.ImageUrl = @"\erp_images\TBTPhoto\" + TBPhotoId + "_" + imgdate + ".jpg";

                        // Image saved successfully
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

        private void JOB_TableUpdate1()
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
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "TBT ID cannot be attched to JOBID";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void btn_saveTBTPhoto_Click(object sender, EventArgs e)
        {
            Panel3_Success.Visible = true;
            Img_Success3.Visible = true;
            Img_Cross3.Visible = false;
            lbl_panel3_msg.Text = "Step-3 : Completed";

            Panel3_Buttons.Visible = false;
            TBTPhotographRow.Visible = true;
            uploadbuttonrow1.Visible = false;
            uploadbuttonrow2.Visible = false;

            UploadedPhotoRow1.Visible = true;
            UploadedPhotoRow2.Visible = true;


            //--------- Hide all Rows in Panel4-----------
            Acn_Type_row.Visible = true;
            Acn_Descp_row.Visible = true;
            Acn_lvl_row.Visible = true;
            Acn_remarks_row.Visible = true;
            Acn_btns_row.Visible = true;

            Acn_tempgrid_row.Visible = true;

            Label1.Text = "Completed";
            Label1.ForeColor = Color.Green;
            Label2.Text = "Completed";
            Label2.ForeColor = Color.Green;
            Label3.Text = "Completed";
            Label3.ForeColor = Color.Green;
            Label4.Text = "Pending";
            Label4.ForeColor = Color.Red;
            LabelOverall.Text = "Pending";
            LabelOverall.ForeColor = Color.Red;

            Panel4_Buttons.Visible = false;
            Panel4_Success.Visible = false;
            Img_Success4.Visible = false;
            Img_Cross4.Visible = false;
            lbl_panel4_msg.Visible = false;
            lbl_panel4_msg.Text = "No JOBID Selected";

            ScriptManager.RegisterStartupScript(this, GetType(), "OpenActionableTab", "OpenActionableTab();", true);
        }


        protected void btn_finalstep_Click(object sender, EventArgs e)
        {
            bool updateFinalSuccess = UpdateFinalClick(lbl_jobid.Text.ToString(), lbl_TBTID.Text.ToString());
            bool jobTableUpdateSuccess = JOB_TableUpdate();

            if (updateFinalSuccess && jobTableUpdateSuccess)
            {
                Response.Redirect("csms_mainview.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "OpenActionableTab", "OpenActionableTab();", true);
            }
        }


        private void FormCleaner()
        {
            DDL_JOBID.SelectedIndex = 0;

            JOBIDDetails_Row.Visible = true;
            lbl_jobid.Text = "";
            lbl_jobiddate.Text = "";
            lbl_jobcreatorname.Text = "";
            lbl_creatorwrk.Text = "";
            lbl_jobsite.Text = "";
            lbl_jobsitecode.Text = "";
            lbl_creatorregion.Text = "";
            lbl_creatorcompany.Text = "";
            lbl_crtrsitename.Text = "";
            lbl_crtrsitecode.Text = "";
            lbl_wrkordr.Text = "";
            lbl_permitno.Text = "";
            lbl_jobrgn.Text = "";
            lbl_jobcompay.Text = "";
            lbl_inchargename.Text = "";
            lbl_inchargewrk.Text = "";
            lbl_jobloc.Text = "";
            lbl_jobshift.Text = "";
            lbl_dept.Text = "";

            AttachedAttendanceRow.Visible = true;
            BindEmptyRowsToGridView();

            locationrow.Visible = true;
            txt_location.Text = "";

            deptrow.Visible = true;
            txt_dept.Text = "";

            jobsupv.Visible = true;
            txt_supv.Text = "";

            jobsupv.Visible = true;
            txt_supv.Text = "";

            LineManagerRow.Visible = true;
            txt_linemanager.Text = "";

            sftyofcrrow.Visible = true;
            DDL_SftyOfcr.SelectedIndex = 0;

            sftysupvrow.Visible = true;
            DDL_SftySupv.SelectedIndex = 0;

            supvrow.Visible = true;
            txt_incharge.Text = "";

            ContractEmployeesrow.Visible = true;
            txt_cntrctemp.Text = "";

            Panel1_Buttons.Visible = true;

            Panel1_Success.Visible = false;
        }

        private void BindEmptyRowsToGridView()
        {
            // Define the number of empty rows you want to bind
            int numRows = 2; // Change this value to the desired number of empty rows

            // Create a DataTable with the desired structure
            DataTable dt = new DataTable();
            dt.Columns.Add("EmployeeWrk");
            dt.Columns.Add("EmployeeName");
            dt.Columns.Add("EmpDesignation");
            dt.Columns.Add("GatePassNo");
            // Add more columns if needed

            // Add empty rows to the DataTable
            for (int i = 0; i < numRows; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            // Bind the DataTable to the GridView
            EmpGrid.DataSource = dt;
            EmpGrid.DataBind();
        }
        protected void btn_sv_acn_Click(object sender, EventArgs e)
        {
            AddToGridView();

            DDL_AcType.SelectedIndex = 0;
            txt_acdescp.Text = "";
            DDL_Priority.SelectedIndex = 0;

            Panel4_Buttons.Visible = true;


            ScriptManager.RegisterStartupScript(this, GetType(), "OpenActionableTab", "OpenActionableTab();", true);
        }
        protected void AddToGridView()
        {
            if (ViewState["Actionables"] != null)
            {
                // Retrieve the DataTable from ViewState
                DataTable dtCurrentTable = (DataTable)ViewState["Actionables"];

                // Check if the DataTable contains only the dummy record
                if (dtCurrentTable.Rows.Count == 1 && dtCurrentTable.Rows[0]["ActionableType"].ToString() == "Default Type")
                {
                    // Remove the dummy record
                    dtCurrentTable.Rows.Clear();
                }


                // Create a new DataRow to add to the DataTable
                DataRow drCurrentRow = dtCurrentTable.NewRow();

                // Retrieve user inputs from the form fields
                string actionableType = DDL_AcType.SelectedItem.Text.ToString();
                string actionableDescription = txt_acdescp.Text;
                string actionableRemarks = txt_acn_rmrks.Text;
                string priority = DDL_Priority.SelectedItem.Text.ToString(); ;

                // Populate the DataRow with user inputs
                drCurrentRow["ActionableType"] = actionableType;
                drCurrentRow["ActionableDescription"] = actionableDescription;
                drCurrentRow["ActionableRemarks"] = actionableRemarks;
                drCurrentRow["Priority"] = priority;

                // Add the DataRow to the DataTable
                dtCurrentTable.Rows.Add(drCurrentRow);

                // Save the updated DataTable back to ViewState
                ViewState["Actionables"] = dtCurrentTable;

                // Bind the GridView with the updated DataTable
                BindMyGridview();
            }
        }

        protected void btn_insertacns_Click(object sender, EventArgs e)
        {
            string title = "Notifications :";
            string body = "";

            if (DataTablePull() == true)
            {
                Panel4_Success.Visible = true;
                Img_Success4.Visible = true;
                Img_Cross4.Visible = false;
                lbl_panel4_msg.Visible = true;
                lbl_panel4_msg.Text = "Step-4 : Completed";

                Acn_Type_row.Visible = false;
                Acn_Descp_row.Visible = false;
                Acn_lvl_row.Visible = false;
                Acn_remarks_row.Visible = false;

                Panel4_Buttons.Visible = false;
                Acn_btns_row.Visible = false;

                Label1.Text = "Completed";
                Label1.ForeColor = Color.Green;
                Label2.Text = "Completed";
                Label2.ForeColor = Color.Green;
                Label3.Text = "Completed";
                Label3.ForeColor = Color.Green;
                Label4.Text = "Completed";
                Label4.ForeColor = Color.Green;
                LabelOverall.Text = "Pending";
                LabelOverall.ForeColor = Color.Red;

                TBTFinalStep.Visible = true;

                ScriptManager.RegisterStartupScript(this, GetType(), "OpenFinalTab", "OpenFinalTab();", true);

                body = "Actionables Added Successfully";
                // Display PNotify notification
                string script = @"<script type='text/javascript'>
                            new PNotify({
                                title: '" + title + @"',
                                text: '" + body + @"',
                                type: 'success',
                                styling: 'bootstrap3'
                            });
                        </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "PNotifyNotification", script, false);
            }
            else
            {
                Panel4_Success.Visible = false;
                Panel4_Buttons.Visible = true;
                Acn_btns_row.Visible = false;
            }
        }

        protected Boolean DataTablePull()
        {
            Boolean flag = false;
            DataTable dt1 = (DataTable)ViewState["Actionables"];

            if (dt1 != null)
            {
                try
                {
                    foreach (DataRow row in dt1.Rows)
                    {
                        string acnType = row["ActionableType"].ToString();
                        string acnDescription = row["ActionableDescription"].ToString();
                        string acnRemarks = row["ActionableRemarks"].ToString();
                        string priorityLevel = row["Priority"].ToString();

                        // Insert feedback for each row in the DataTable
                        bool insertionSuccess = InsertFeedback(acnType, acnDescription, acnRemarks, priorityLevel);
                        if (insertionSuccess)
                        {
                            // Set flag to true if any row insertion is successful
                            flag = true;
                        }
                    }

                    if (flag)
                    {
                        // Provide acknowledgment of successful insertion
                        // For example, display a success message
                        // You can use a label or any other control to display the message
                        //successLabel.Text = "Feedback inserted successfully.";
                    }
                    else
                    {
                        // Provide acknowledgment of unsuccessful insertion
                        // For example, display an error message
                        //errorLabel.Text = "Failed to insert feedback.";
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the exception

                    Console.WriteLine("An error occurred while inserting feedback: " + ex.Message);
                }
            }
            return flag;
        }

        public bool InsertFeedback(string acnType, string acnDescription, string actionableAction, string priorityLevel)
        {
            bool isSuccess = false;
            dbcl.Sqlconnection();
            using (SqlConnection connection = dbcl.Conn)
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    SqlCommand command = connection.CreateCommand();
                    command.Transaction = transaction;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_InsertFeedbacks";

                    command.Parameters.AddWithValue("@AcnSrc", "AttachedwithTBT");
                    command.Parameters.AddWithValue("@DateSubmitted", DateTime.Now.Date);
                    command.Parameters.AddWithValue("@TimeSubmitted", DateTime.Now.TimeOfDay);
                    command.Parameters.AddWithValue("@WorkState", Session["STATE"].ToString());
                    command.Parameters.AddWithValue("@WorkrRegion", Session["REGION"].ToString());
                    command.Parameters.AddWithValue("@WorkCompany", Session["COMPANY_CODE"].ToString());
                    command.Parameters.AddWithValue("@RefCSMFormName", "CSM-TBT");
                    command.Parameters.AddWithValue("@RefCSMFormID", lbl_TBTID.Text.ToString());
                    command.Parameters.AddWithValue("@SubmittedByName", Session["USERNAME"].ToString());
                    command.Parameters.AddWithValue("@SubmittedByWrk", Session["WORKMAN"].ToString());
                    command.Parameters.AddWithValue("@AcnType", acnType);
                    command.Parameters.AddWithValue("@AcnDescription", acnDescription);
                    command.Parameters.AddWithValue("@ActionableAction", actionableAction);
                    command.Parameters.AddWithValue("@PriorityLevel", priorityLevel);
                    command.Parameters.AddWithValue("@AssignedToWrk", Session["WORKMAN"].ToString());
                    command.Parameters.AddWithValue("@AssignedToName", Session["USERNAME"].ToString());
                    command.Parameters.AddWithValue("@AssignedByWrk", Session["WORKMAN"].ToString());
                    command.Parameters.AddWithValue("@AssignedByName", Session["USERNAME"].ToString());
                    command.Parameters.AddWithValue("@CurrentStatus", "Open");
                    command.Parameters.AddWithValue("@TargetCompletionDate", DateTime.Now.Date.AddDays(7));
                    TimeSpan targetCompletionTime = DateTime.Now.Add(TimeSpan.FromDays(7)).TimeOfDay;
                    command.Parameters.AddWithValue("@TargetCompletionTime", targetCompletionTime);
                    //command.Parameters.AddWithValue("@ResolutionOutcome", resolutionOutcome);
                    //command.Parameters.AddWithValue("@ClosedByName", closedByName);
                    //command.Parameters.AddWithValue("@ClosedByWrk", closedByWrk);
                    command.ExecuteNonQuery();

                    // Update ActionablesAdded-------------------------------------------------------------------------------//
                    string sqlQuery = "UPDATE tbl_toolboxtalkdata SET ActionablesAdded = 1 WHERE Ref_JOBID = @Ref_JOBID AND TBT_ID = @TBT_ID";
                    command.CommandType = CommandType.Text; // This line is removed
                    command.CommandText = sqlQuery;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Ref_JOBID", lbl_jobid.Text.ToString());
                    command.Parameters.AddWithValue("@TBT_ID", lbl_TBTID.Text.ToString());
                    command.ExecuteNonQuery();

                    // Commit transaction if everything succeeded
                    transaction.Commit();
                    isSuccess = true;

                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    //Console.WriteLine("An error occurred: " + ex.Message);
                    string noActiveJobIdNotificationScript = @"<script type='text/javascript'>
                            new PNotify({
                                title: 'Oh No !!!',
                                text: 'An error occured with SP',
                                type: 'error',
                                styling: 'bootstrap3'
                            });
                        </script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "NoActiveJOBID_Notification", noActiveJobIdNotificationScript, false);

                    // Log the exception or handle it as needed
                    //string errorMessage = "An error occurred: " + ex.Message;
                    //Console.WriteLine(errorMessage);
                    // Display error message using JavaScript or any other means
                    //string errorScript = "<script>alert('" + errorMessage + "');</script>";
                    //ClientScript.RegisterStartupScript(this.GetType(), "ErrorNotification", errorScript);

                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        // Log the exception or handle it as needed
                        Console.WriteLine("Rollback failed: " + exRollback.Message);
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                } 
            }
            return isSuccess;

        }


        public void UpdateFinalClick1(string refJobID, string tbtID)
        {
            try
            {
                dbcl.Sqlconnection();
                using (SqlConnection connection = dbcl.Conn)
                {
                    // SQL UPDATE statement
                    string query = "UPDATE [tbl_toolboxtalkdata] SET [FinalClick] = 1 WHERE [Ref_JOBID] = @Ref_JOBID AND [TBT_ID] = @TBT_ID";

                    // Create SqlCommand object
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to the command
                        command.Parameters.AddWithValue("@Ref_JOBID", refJobID);
                        command.Parameters.AddWithValue("@TBT_ID", tbtID);
                        //command.Parameters.AddWithValue("@TBT_Date", tbtDate);

                        // Open the connection
                        connection.Open();

                        // Execute the command
                        command.ExecuteNonQuery();
                    }
                }

                // Successful execution
                Console.WriteLine("Update operation completed successfully.");
            }
            catch (Exception ex)
            {
                // Exception occurred, handle it
                Console.WriteLine("Error: " + ex.Message);
            }

        }



        public bool UpdateFinalClick(string refJobID, string tbtID)
        {
            try
            {
                dbcl.Sqlconnection();
                using (SqlConnection connection = dbcl.Conn)
                {
                    string query = "UPDATE [tbl_toolboxtalkdata] SET [FinalClick] = 1 WHERE [Ref_JOBID] = @Ref_JOBID AND [TBT_ID] = @TBT_ID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Ref_JOBID", refJobID);
                        command.Parameters.AddWithValue("@TBT_ID", tbtID);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                return true; // Indicate success
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                return false; // Indicate failure
            }
        }

        private bool JOB_TableUpdate()
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
                return true; // Indicate success
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                return false; // Indicate failure
            }
        }

        public bool UpdateActionablesAdded(string refJobId, string tbtId)
        {
            bool isSuccess = false;
            dbcl.Sqlconnection();

            using (SqlConnection connection = dbcl.Conn)
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    SqlCommand command = connection.CreateCommand();
                    command.Transaction = transaction;

                    string sqlQuery = "UPDATE tbl_toolboxtalkdata SET ActionablesAdded = 3 WHERE Ref_JOBID = @Ref_JOBID AND TBT_ID = @TBT_ID";
                    command.CommandType = CommandType.Text;
                    command.CommandText = sqlQuery;

                    command.Parameters.AddWithValue("@Ref_JOBID", refJobId);
                    command.Parameters.AddWithValue("@TBT_ID", tbtId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        transaction.Commit();
                        isSuccess = true;
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    transaction.Rollback();
                    string title = "Notifications :";
                    string body = ex.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return isSuccess;
        }

        protected void btn_skip_Click(object sender, EventArgs e)
        {
            if (UpdateActionablesAdded(lbl_jobid.Text.ToString(), lbl_TBTID.Text.ToString()) == true)
            {
                Panel4_Success.Visible = true;
                Img_Success4.Visible = true;
                Img_Cross4.Visible = false;
                lbl_panel4_msg.Visible = true;
                lbl_panel4_msg.Text = "Step-4 : Completed";

                Acn_Type_row.Visible = false;
                Acn_Descp_row.Visible = false;
                Acn_lvl_row.Visible = false;
                Acn_remarks_row.Visible = false;

                Panel4_Buttons.Visible = false;
                Acn_btns_row.Visible = false;

                Label1.Text = "Completed";
                Label1.ForeColor = Color.Green;
                Label2.Text = "Completed";
                Label2.ForeColor = Color.Green;
                Label3.Text = "Completed";
                Label3.ForeColor = Color.Green;
                Label4.Text = "Completed";
                Label4.ForeColor = Color.Green;
                LabelOverall.Text = "Pending";
                LabelOverall.ForeColor = Color.Red;

                TBTFinalStep.Visible = true;

                ScriptManager.RegisterStartupScript(this, GetType(), "OpenFinalTab", "OpenFinalTab();", true);
                string title = "Notifications :";
                string body = "";
                body = "Actionables Skipped Successfully";
                // Display PNotify notification
                string script = @"<script type='text/javascript'>
                            new PNotify({
                                title: '" + title + @"',
                                text: '" + body + @"',
                                type: 'success',
                                styling: 'bootstrap3'
                            });
                        </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "PNotifyNotification", script, false);
            }
            else
            {
                Panel4_Success.Visible = false;
                Panel4_Buttons.Visible = true;
                Acn_btns_row.Visible = false;
            }
        }
    }
}