using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using DocumentFormat.OpenXml.Math;

namespace WebApplication1.bussiness.production
{
    public partial class job_outpunch : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
        DataTable dt = new DataTable();

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
                    string CmdString4 = "Select Status_Name,Status_Code from tlb_attendancecodes where Approver='Yes' and Status='Present' order by slno";
                    Bind_AttendnaceCode(CmdString4);

                    OUTpunchPanel_Row.Visible = true;
                    ActiveJOB_Checker();
                }
            }
        }

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

        private void ActiveJOB_Checker()
        {
            Int32 Activejobcount = CC.Find_ActiveJOBCountforOUTPunch(Session["WORKMAN"].ToString(), Session["REGION"].ToString());
            if (Activejobcount > 0)
            {
                if (Session["REGION"].ToString() == "AGL")
                {
                    dbcl.FillCombo(DDL_JOBID, "select JOBID from tbl_jobs where Creator_Workman='" + Session["WORKMAN"].ToString() + "' and JOBID_Status='Active' and EntryExit='Entry' order by CreatedDate desc ");
                }
                else
                {
                    dbcl.FillCombo(DDL_JOBID, "select JOBID from tbl_jobs where Creator_Workman='" + Session["WORKMAN"].ToString() + "' and JOBID_Status='Active' and EntryExit='Entry' order by CreatedDate desc ");
                }

                //AddDefaultFirstRecord();
            }
            else
            {
                string title = "Notifications :";
                string body = "NO Active JOB ID Found...! Kindly create a JOB ID and proceed.";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void DDL_JOBID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ddljobid = DDL_JOBID.SelectedItem.Text.ToString();
            if (Pull_PermitStatus(ddljobid) == true)
            {
                Bind_JOBIDDetails(ddljobid);
                JOBIDDetails_Row.Visible = true;
            }
            else
            {
                string title = "Notifications :";
                string body = "Permit NOT Upload...Aganist the selected JOBID!!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private Boolean Pull_PermitStatus(string jobid)
        {
            Boolean flag = false;
            try
            {
                string query = "select * from tbl_jobs where JOBID=@JOBID and JOBID_Status='Active' and EntryExit='Entry'";
                SqlParameter[] pram = {
                                          new SqlParameter("@JOBID",jobid),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    string uploadstatus = dt.Rows[0]["FinalUpldStatus"].ToString();
                    if (uploadstatus == "Yes")
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            return flag;
        }

        private void CheckPendingOUT()
        {
            string ddljobid = DDL_JOBID.SelectedItem.Text.ToString();
            string supv = Session["WORKMAN"].ToString();
            if (CC.CheckforPendingOUT(ddljobid, supv) == 0)
            {
                if (CC.CheckforPendingPermit(ddljobid, supv) == 0)
                {
                    ViewState_TableRow.Visible = false;
                    NoPenidngPunch.Visible = true;
                    UpdateJOBTable1("Blocked", "Out-Punch Done","4","Exit");
                }
                else
                {
                    ViewState_TableRow.Visible = false;
                    NoPenidngPunch.Visible = true;
                    UpdateJOBTable1("Active", "Out-Punch Done", "4", "Exit");
                }
            }
            else
            {
                ViewState_TableRow.Visible = true;
                Bind_GridView();
            }
        }


        private void UpdateJOBTable1(string jobidstatus, string jobstatus, string mastercode, string entryexitstatus)
        {
            string ddljobid = DDL_JOBID.SelectedItem.Text.ToString();
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_jobs set JOBID_Status=@JOBID_Status, JOB_Status=@JOB_Status, MasterStatusCode=@MasterStatusCode , EntryExit=@EntryExit where JOBID=@JOBID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@JOBID", lbl_jobid.Text.ToString());
                cmd.Parameters.AddWithValue("@JOBID_Status", jobidstatus);
                cmd.Parameters.AddWithValue("@JOB_Status", jobstatus);
                cmd.Parameters.AddWithValue("@MasterStatusCode", mastercode);  // JOBID created, Permit Uploaded, Can Proceed to Entry Page
                cmd.Parameters.AddWithValue("@EntryExit", entryexitstatus);
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


        private void Bind_JOBIDDetails(string jobid)
        {
            try
            {
                string query = "select CreatedDate,Creator_Name,Creator_Workman,Creator_Region,Creator_Company,Creator_Site,Creator_SiteCode,WorkOrderNo,JOBID,JOB_Region,JOB_Company,JOB_Site,JOB_SiteCode,JOB_InchargeWrk,JOB_InchargeName,JOB_Dept,JOB_Location,JOB_Shift,JOB_PermitNo,CSM_Documents from tbl_jobs where JOBID=@JOBID";
                SqlParameter[] pram = {
                                          new SqlParameter("@JOBID",jobid),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    lbl_jobiddate.Text = dt.Rows[0]["CreatedDate"].ToString();
                    lbl_jobcreatorname.Text = dt.Rows[0]["Creator_Name"].ToString();
                    lbl_creatorwrk.Text = dt.Rows[0]["Creator_Workman"].ToString();
                    lbl_creatorregion.Text = dt.Rows[0]["Creator_Region"].ToString();
                    lbl_creatorcompany.Text = dt.Rows[0]["Creator_Company"].ToString();
                    lbl_crtrsitename.Text = dt.Rows[0]["Creator_Site"].ToString();
                    lbl_crtrsitecode.Text = dt.Rows[0]["Creator_SiteCode"].ToString();
                    lbl_wrkordr.Text = dt.Rows[0]["WorkOrderNo"].ToString();
                    lbl_jobid.Text = dt.Rows[0]["JOBID"].ToString();
                    lbl_jobrgn.Text = dt.Rows[0]["JOB_Region"].ToString();
                    lbl_jobcompay.Text = dt.Rows[0]["JOB_Company"].ToString();
                    lbl_jobsite.Text = dt.Rows[0]["JOB_Site"].ToString();
                    lbl_jobsitecode.Text = dt.Rows[0]["JOB_SiteCode"].ToString();
                    lbl_inchargewrk.Text = dt.Rows[0]["JOB_InchargeWrk"].ToString();
                    lbl_inchargename.Text = dt.Rows[0]["JOB_InchargeName"].ToString();
                    lbl_dept.Text = dt.Rows[0]["JOB_Dept"].ToString();
                    lbl_jobloc.Text = dt.Rows[0]["JOB_Location"].ToString();
                    lbl_jobshift.Text = dt.Rows[0]["JOB_Shift"].ToString();
                    lbl_permitno.Text = dt.Rows[0]["JOB_PermitNo"].ToString();

                    string CSM_DocYesNo = dt.Rows[0]["CSM_Documents"].ToString();
                    if (CSM_DocYesNo == "Yes")
                    {
                        CSMRow.Visible = true;
                        //First check for CSM Docs Counts then go for Punch OUT Display
                        CSM_Documents_CountbyJOBID(jobid);
                    }
                    else
                    {
                        CheckPendingOUT();
                    }
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void CSM_Documents_CountbyJOBID(string jobid)
        {
            try
            {
                string query = "select TBT_Count, SOP_Count from tbl_jobs where JOBID=@JOBID";
                SqlParameter[] pram = {
                                          new SqlParameter("@JOBID",jobid),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    Int32 tbtcount = Convert.ToInt32(dt.Rows[0]["TBT_Count"].ToString());
                    lbl_tbtcount.Text = tbtcount.ToString();

                    Int32 sopcount = Convert.ToInt32(dt.Rows[0]["SOP_Count"].ToString());
                    lbl_sopcount.Text = sopcount.ToString();

                    if (tbtcount > 0 && sopcount > 0)
                    {
                        IncompleteCSM.Visible = false;
                        CheckPendingOUT();
                    }
                    else
                    {
                        IncompleteCSM.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void Bind_GridView()
        {
            string ddljobid = DDL_JOBID.SelectedItem.Text.ToString();
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdString = "Select * from tbl_attendance where Creator_Workman='" + Session["WORKMAN"].ToString() + "' and JOBID='"+ ddljobid + "' and AttendanceStatus = 'Entry' order by Id";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dtbl = new DataTable();
            ad.Fill(dtbl);
            GridView1.DataSource = dtbl;
            GridView1.DataBind();
            dbcl.Conn.Close();
        }

        protected void PunchOUT_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as Button).CommandArgument);
            ViewState_TableRow.Visible = false;

            EmployeeName_Row.Visible = true;
            InPunchDate_Row.Visible = true;
            OUTPunchDate_Row.Visible = true;
            OUTPunchTime_Row.Visible = true;
            PunchOUT_buttons.Visible = true;
            LunchFactorRow.Visible = true;
            AttenCode.Visible = true;
            OTRow.Visible = true;

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select Id,EmployeeName,EmployeeWrk,WourkHours,Inpunch_Time from tbl_attendance where Id = @Id", dbcl.Conn);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Id", id);
                sqlDa.SelectCommand.CommandType = CommandType.Text;
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);

                lbl_Id.Text = dtbl.Rows[0][0].ToString();
                txt_empname.Text = dtbl.Rows[0][1].ToString();
                lbl_empworkman.Text = dtbl.Rows[0][2].ToString();
                lbl_workhours.Text = dtbl.Rows[0][3].ToString();
                txt_inpunchtime.Text = dtbl.Rows[0][4].ToString();
                dbcl.Conn.Close();

                txt_time.Text= DateTime.Now.ToString("hh:mm tt");
            }
            catch (Exception ex)
            {
                lbl_msg.Text = "Error : " + ex.Message;
                throw;
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "delete from tbl_attendance where Id='" + id + "' ";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                dbcl.Conn.Close();

                string title = "Notifications :";
                string body = "Data Deleted Successfully";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                //throw;
            }

            CheckPendingOUT();
        }

        protected void btn_punchout_Click(object sender, EventArgs e)
        {
            if (UpdateAttendanceTable() == true)
            {
                EmployeeName_Row.Visible = false;
                InPunchDate_Row.Visible = false;
                OUTPunchDate_Row.Visible = false;
                OUTPunchTime_Row.Visible = false;
                PunchOUT_buttons.Visible = false;
                LunchFactorRow.Visible = false;
                AttenCode.Visible = false;
                OTRow.Visible = false;

                CheckPendingOUT();
            }
            else
            {

            }
        }

        private Boolean UpdateAttendanceTable()
        {

            Boolean flag = false;

            string jobid = DDL_JOBID.SelectedItem.Text.ToString();
            string dbid = lbl_Id.Text.ToString();
            string emp_wrkman = lbl_empworkman.Text.ToString();

            string emp_wrkhrs = lbl_workhours.Text.ToString();
            Int32 emp_wrkhours = Convert.ToInt32(emp_wrkhrs);

            // Employee Working Hours for OT Calucaltions ----------- (8 / 12 / 24) Hours ----- (480 / 720 / 1440) Minutes
            Int32 emp_wrkmnis = emp_wrkhours * 60;

            string outtime = txt_date.Text.TrimEnd().ToString() + " " + txt_time.Text.ToString();
            string intime = txt_inpunchtime.Text.ToString();
            DateTime dtout = DateTime.Parse(outtime.ToString());
            DateTime dtin = DateTime.Parse(intime);
            string lunchyesno = RBTN_LunchFactor.SelectedValue.ToString();


            Int32 workdmins = 0;
            string current = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            DateTime crnttym = DateTime.ParseExact(current, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
            dbcl.Findworktime1(outtime, current, ref workdmins);

            if (workdmins <= 2880) //32=2880
            {
                Int32 workedmins = 0;
                decimal workedhours = .0m;
                dbcl.FindEmployeeWorkedTime(intime, outtime, ref workedmins, ref workedhours);
                decimal emp_calOThrs = .0m;
                dbcl.CalculateOvertime(emp_wrkmnis, workedmins, lunchyesno, ref emp_calOThrs);

                try
                {
                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    SqlCommand cmd = new SqlCommand("SP_Update_AttendancePunchOUT", dbcl.Conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.AddWithValue("@Id", dbid);
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    cmd.Parameters.AddWithValue("@SubmitterStatus", "Exit");
                    cmd.Parameters.AddWithValue("@EmployeeWrk", emp_wrkman);
                    cmd.Parameters.AddWithValue("@Outpunch_Time", dtout);
                    cmd.Parameters.AddWithValue("@LunchFactor", lunchyesno);
                    cmd.Parameters.AddWithValue("@WorkedTime", workedmins);
                    cmd.Parameters.AddWithValue("@WorkedHours", workedhours);
                    cmd.Parameters.AddWithValue("@Calc_OT", emp_calOThrs);
                    cmd.Parameters.AddWithValue("@ProvidedOT", txt_ot.Text.ToString());
                    cmd.Parameters.AddWithValue("@LastModified", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                    cmd.Parameters.AddWithValue("@AttendanceStatus", "Exit");
                    cmd.Parameters.AddWithValue("@AttendanceCode", DDL_AttenCode.SelectedValue.ToString());
                    cmd.ExecuteNonQuery();
                    dbcl.Conn.Close();
                    flag = true;

                    //DDL_AttenCode.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    lbl_msg.Text = "Error : " + ex.Message ;
                    flag = false;
                }
            }

            else
            {
                lbl_msg.Text = "Only 24 Hour Back entry is allowed";
            }
            return flag;
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            EmployeeName_Row.Visible = false;
            InPunchDate_Row.Visible = false;
            OUTPunchDate_Row.Visible = false;
            OUTPunchTime_Row.Visible = false;
            PunchOUT_buttons.Visible = false;
            LunchFactorRow.Visible = false;
            OTRow.Visible = false;
            CheckPendingOUT();
        }
    }
}