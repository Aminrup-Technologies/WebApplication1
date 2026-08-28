using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace WebApplication1.bussiness.production
{
    public partial class attach_manpower : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        DataTable dt = new DataTable();
        CountChecker CC = new CountChecker();

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
                    Bind_JOBIDDetails(jobid);
                    JOBIDDetails_Row.Visible = true;

                    string CmdString4 = "Select Status_Name,Status_Code from tlb_attendancecodes where Approver='Yes' and Status='Present' order by slno";
                    Bind_AttendnaceCode(CmdString4);

                    AddDefaultFirstRecord();

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

        private void Bind_JOBIDDetails(string jobid)
        {
            try
            {
                string query = "select CreatedDate, Creator_Name, Creator_Workman, Creator_Region, Creator_Company, Creator_Site, Creator_SiteCode, WorkOrderNo, JOBID, JOB_Region, JOB_Company, JOB_Site, JOB_SiteCode, JOB_InchargeWrk, JOB_InchargeName, JOB_Dept, JOB_Location, JOB_Shift, JOB_PermitNo from tbl_jobs where JOBID=@JOBID";
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
                    CheckforAttachedAttendnace(jobid);
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void CheckforAttachedAttendnace(string jobid)
        {
            dbcl.Sqlconnection();
            string cmdstring = "select count (Id) from tbl_attendance where JOBID= '" + jobid + "'";
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
                Bind_AttendanceGridView(jobid);
            }
            dbcl.DisconnectDb();
        }

        private void Bind_AttendanceGridView(string jobid)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdString = "Select * from tbl_attendance where JOBID='" + jobid + "' order by Id";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dtbl = new DataTable();
            ad.Fill(dtbl);
            GridView2.DataSource = dtbl;
            GridView2.DataBind();
            dbcl.Conn.Close();
        }




        //---------------------------------------------------------------------------------------------------------//
        protected void txt_empworkman_TextChanged(object sender, EventArgs e)
        {
            string entered_workman = txt_empworkman.Text.TrimEnd().ToString();
            PendingOUTMsg.Visible = false;
            if (entered_workman != string.Empty)
            {
                if (CheckSDuplicateEntry() == false)
                {
                    if (dbcl.CheckEmployeeActiveStatus(entered_workman) == true)
                    {
                        if (CC.Check_EmployeePunchOUT(entered_workman) == 0 || CC.Check_EmployeePunchOUT(entered_workman) != 0)
                        {
                            string name = string.Empty;
                            string workhours = string.Empty;
                            string worksitename = string.Empty;
                            string worksitecode = string.Empty;
                            string category = string.Empty;
                            string categorycode = string.Empty;//Added on 24.11.22
                            string designation = string.Empty;
                            string designationcode = string.Empty;//Added on 24.11.22
                            string gpno = string.Empty;
                            string gpexp = string.Empty;
                            string sftyno = string.Empty;
                            string sftyexp = string.Empty;
                            string pvexp = string.Empty;

                            dbcl.FindEmployeeDataforInPunch(entered_workman, ref name, ref workhours, ref worksitename, ref worksitecode, ref category, ref categorycode, ref designation, ref designationcode, ref gpno, ref gpexp, ref sftyno, ref sftyexp, ref pvexp);

                            string po_skill = ""; //Added on 24.11.2022
                            string po_skillcode = ""; //Added on 24.11.2022
                            PO_SkillPull(ref designation, ref po_skill, ref po_skillcode);  //Added on 24.11.2022
                            //Below logic is added on 24.11.2022
                            if (po_skill == "" && po_skillcode == "")
                            {
                                po_skill = category;
                                po_skillcode = categorycode;

                                string title = "Notifications :";
                                string body = "No Mapped PO Skill Category Found";
                                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                            }

                            lbl_pocategoryname.Text = po_skill; //Added on 24.11.2022
                            lbl_pocategorycode.Text = po_skillcode; //Added on 24.11.2022

                            txt_empname.Text = name;
                            lbl_workhours.Text = workhours;
                            txt_worksite.Text = worksitename;
                            lbl_worksitecode.Text = worksitecode;
                            lbl_category.Text = category;
                            lbl_designation.Text = designation;
                            lbl_categorycode.Text = categorycode;//Added on 24.11.22
                            lbl_designationcode.Text = designationcode;//Added on 24.11.22
                            lbl_gpno.Text = gpno;

                            EmployeeName_Row.Visible = true;
                            EmployeeWorksite_Row.Visible = true;
                            InPunchDate_Row.Visible = true;
                            InPunchTime_Row.Visible = true;

                            OUTPunchDate_Row.Visible = true;
                            OUTPunchTime_Row.Visible = true;
                            LunchFactorRow.Visible = true;
                            OTRow.Visible = true;
                            AttenCode.Visible = true;
                            Addto_buttons.Visible = true;

                            btn_submit.Enabled = true;

                            ViewState_TableRow.Visible = true;
                            //AddDefaultFirstRecord();
                        }
                        else
                        {
                            //Find PJOBIDrevious OUT Punch Pending Details and display to the USER
                            string JOBID = "";
                            string jobdate = "";
                            string submittername = "";
                            Pull_PendingOUTDetails(entered_workman, ref JOBID, ref jobdate, ref submittername);

                            txt_empworkman.Text = "";
                            PendingOUTMsg.Visible = true;


                            string title = "Notifications :";
                            lbl_pendingmsg.Text = "Previous OUT Punch Pending against JOBID : '" + JOBID + "', Dated ON : '" + jobdate + "', Submitted By : '" + submittername + "'. Kindly Punch OUT First.";
                            string body = "Previous OUT Punch Pending";
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);


                            //ViewState_TableRow.Visible = false;
                            //AddDefaultFirstRecord();

                            EmployeeName_Row.Visible = false;
                            EmployeeWorksite_Row.Visible = false;
                            InPunchDate_Row.Visible = false;
                            InPunchTime_Row.Visible = false;
                            Addto_buttons.Visible = false;
                        }
                    }
                    else
                    {
                        string title = "Notifications :";
                        string body = "No data found against the entered workman";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                        txt_empworkman.Text = "";
                        txt_empworkman.Focus();

                        EmployeeName_Row.Visible = false;
                        EmployeeWorksite_Row.Visible = false;
                        InPunchDate_Row.Visible = false;
                        InPunchTime_Row.Visible = false;
                        Addto_buttons.Visible = false;
                    }
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Employee already added";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                    txt_empworkman.Text = "";
                }
            }
            else
            {
                txt_empworkman.Focus();
            }
        }

        //Added on 24.11.2022
        private void PO_SkillPull(ref string designation, ref string po_skill, ref string po_skillcode)
        {
            if (ViewState["dt1"] != null)
            {
                DataTable dt = (DataTable)ViewState["dt1"];
                foreach (DataRow row in dt.Rows)
                {
                    string desg = row["Designation_Type"].ToString();
                    if (designation == desg)
                    {
                        po_skill = row["WO_Category_Type"].ToString();
                        po_skillcode = row["WO_Category_Code"].ToString();
                    }
                }
            }
        }

        protected Boolean CheckSDuplicateEntry()
        {
            Boolean flag = false;

            DataTable dt1;
            dt1 = (DataTable)ViewState["Attendance"];
            if (dt1 != null)
            {
                Int32 i;
                for (i = 0; i <= dt1.Rows.Count - 1; i++)
                {
                    string wrk = ((Label)GridView1.Rows[i].FindControl("lbl_wrk")).Text;
                    if (wrk == txt_empworkman.Text.ToString())
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            return flag;
        }

        private void Pull_PendingOUTDetails(string workman, ref string JOBID, ref string jobdate, ref string submittername)
        {
            try
            {
                string query = "select * from tbl_attendance where EmployeeWrk=@EmployeeWrk and AttendanceStatus='Entry' and Outpunch_Time is NULL";
                SqlParameter[] pram = {
                                          new SqlParameter("@EmployeeWrk",workman),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    jobdate = dt.Rows[0]["CreatedDate"].ToString();
                    submittername = dt.Rows[0]["Creator_Name"].ToString();
                    JOBID = dt.Rows[0]["JOBID"].ToString();
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }



        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (txt_empname.Text.ToString() != "" && txt_worksite.Text.ToString() != "" && txt_indate.Text.ToString() != "" && txt_intime.Text.ToString() != "")
            {
                ADDTOLIST();
                ViewState_TableRow.Visible = true;
                SendAttendance_Buttons.Visible = true;
            }
            else
            {
                txt_empworkman.Focus();
                btn_submit.Enabled = false;
            }
        }

        protected void ADDTOLIST()
        {
            if (ViewState["Attendance"] != null)
            {
                //get datatable from view state
                DataTable dtCurrentTable = (DataTable)ViewState["Attendance"];
                DataRow drCurrentRow = null;

                string region = lbl_jobrgn.Text.ToString();
                string emp_wrkhrs = lbl_workhours.Text.ToString();
                Int32 emp_wrkhours = Convert.ToInt32(emp_wrkhrs);
                Int32 emp_wrkmnis = emp_wrkhours * 60;
                string intime = txt_indate.Text.TrimEnd().ToString() + " " + txt_intime.Text.ToString();
                string outtime = txt_outdate.Text.TrimEnd().ToString() + " " + txt_outtime.Text.ToString();
                string current = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string lunchyesno = RBTN_LunchFactor.SelectedValue.ToString();

                //The below function is used to Check When user in Performing INPUNCH ---- Relaxation to Entry
                Int32 DelayedHours = 0;
                dbcl.FindInpunchEligibilty(intime, current, ref DelayedHours);

                Int32 workedmins = 0;
                decimal workedhours = .0m;
                dbcl.FindEmployeeWorkedTime(intime, outtime, ref workedmins, ref workedhours);
                decimal emp_calOThrs = .0m;

                if (region == "NINL")
                {
                    dbcl.CalculateOvertimeRev(emp_wrkmnis, workedmins, lunchyesno, ref emp_calOThrs);
                }
                else
                {
                    dbcl.CalculateOvertime(emp_wrkmnis, workedmins, lunchyesno, ref emp_calOThrs);
                }

                //dbcl.CalculateOvertime(emp_wrkmnis, workedmins, lunchyesno, ref emp_calOThrs);

                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        drCurrentRow = dtCurrentTable.NewRow();

                        drCurrentRow["name"] = txt_empname.Text.Trim().ToString();
                        drCurrentRow["wrk"] = txt_empworkman.Text.Trim().ToUpper().ToString();
                        drCurrentRow["wrkhrs"] = lbl_workhours.Text.Trim().ToString();

                        //----------Added on 13-05-20201--------------------------------//
                        drCurrentRow["category"] = lbl_category.Text.Trim().ToString();
                        drCurrentRow["categorycode"] = lbl_categorycode.Text.Trim().ToString(); //Added on 24.11.2022

                        //----------Added on 24.11.2022--------------------------------//
                        drCurrentRow["po_category"] = lbl_pocategoryname.Text.Trim().ToString();
                        drCurrentRow["po_categorycode"] = lbl_pocategorycode.Text.Trim().ToString();
                        //----------Added on 24.11.2022--------------------------------//

                        drCurrentRow["designation"] = lbl_designation.Text.Trim().ToString();
                        drCurrentRow["designationcode"] = lbl_designationcode.Text.Trim().ToString(); //Added on 24.11.2022

                        drCurrentRow["gpno"] = lbl_gpno.Text.Trim().ToString();
                        //----------Added on 13-05-20201--------------------------------//

                        drCurrentRow["wrksitename"] = txt_worksite.Text.ToString();
                        drCurrentRow["wrksitecode"] = lbl_worksitecode.Text.ToString();
                        drCurrentRow["in"] = intime.ToString();


                        drCurrentRow["out"] = outtime;
                        drCurrentRow["lunch"] = RBTN_LunchFactor.SelectedValue.ToString();
                        drCurrentRow["wrkmin"] = workedmins;
                        drCurrentRow["wrkdhrs"] = workedhours;
                        drCurrentRow["cot"] = emp_calOThrs.ToString();
                        drCurrentRow["pot"] = txt_ot.Text.ToString();
                        drCurrentRow["presentstatus"] = DDL_AttenCode.SelectedValue.ToString();
                    }

                    if (dtCurrentTable.Rows[0][0].ToString() != "")
                    {
                        dtCurrentTable.Rows[0].Delete();
                        dtCurrentTable.AcceptChanges();
                    }


                    //add created Rows into dataTable
                    dtCurrentTable.Rows.Add(drCurrentRow);

                    //Save Data table into view state after creating each row
                    ViewState["Attendance"] = dtCurrentTable;

                    //Bind Gridview with latest Row
                    BindMyGridview();


                    txt_empworkman.Text = "";
                    txt_empname.Text = "";
                    lbl_workhours.Text = "";
                    lbl_worksitecode.Text = "";
                    txt_worksite.Text = "";
                    lbl_category.Text = ""; lbl_categorycode.Text = ""; //Added on 24.11.2022
                    lbl_designation.Text = ""; lbl_designationcode.Text = ""; //Added on 24.11.2022
                    lbl_gpno.Text = "";

                    EmployeeName_Row.Visible = false;
                    EmployeeWorksite_Row.Visible = false;
                    InPunchDate_Row.Visible = false;
                    InPunchTime_Row.Visible = false;

                    OUTPunchDate_Row.Visible = false;
                    OUTPunchTime_Row.Visible = false;
                    LunchFactorRow.Visible = false;
                    OTRow.Visible = false;
                    AttenCode.Visible = false;

                    btn_submit.Enabled = false;
                }
            }
        }
        public void BindMyGridview()
        {
            if (ViewState["Attendance"] != null)
            {
                DataTable dt = (DataTable)ViewState["Attendance"];

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

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["Attendance"];
            DataRow dr = dtCurrentTable.Rows[e.RowIndex];
            dtCurrentTable.Rows.Remove(dr);
            GridView1.EditIndex = -1;
            BindMyGridview();
        }

        protected void btn_finalsubmit_Click(object sender, EventArgs e)
        {

            if (DataTablePull() == true)
            {
                AttendanceSentMesg.Visible = true;
                SendAttendance_Buttons.Visible = false;
                ViewState_TableRow.Visible = false;
                Addto_buttons.Visible = false;
                InpunchPanel_Row.Visible = true;
                AttachedAttendanceRow.Visible = true;
                //inpunched_buttons.Visible = true;
                //btn_tbtpage.Visible = true;

                CheckforAttachedAttendnace();
                Bind_JOBIDDetails(lbl_jobid.Text.ToString());

                AddDefaultFirstRecord();
            }
            else
            {
                AttendanceSentMesg.Visible = false;
                SendAttendance_Buttons.Visible = true;
                ViewState_TableRow.Visible = true;
                Addto_buttons.Visible = true;
                InpunchPanel_Row.Visible = true;
            }

        }

        private void CheckforAttachedAttendnace()
        {
            string ddljobid = lbl_jobid.Text.ToString();
            dbcl.Sqlconnection();
            string cmdstring = "select count (Id) from tbl_attendance where JOBID= '" + ddljobid + "' and Creator_Workman='" + Session["WORKMAN"].ToString() + "'  and AttendanceStatus = 'Entry'";
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
                //Update manpower count in job table for view in list, added on 15-04-2023
                dbcl.UPDT_ManpowerCountJOBID(count, ddljobid);
                AttachedAttendanceRow.Visible = true;
                Bind_AttendanceGridView(ddljobid);
            }
            dbcl.DisconnectDb();
        }

        protected Boolean DataTablePull()
        {
            Boolean flag = false;

            DataTable dt1;
            dt1 = (DataTable)ViewState["Attendance"];
            if (dt1 != null)
            {
                Int32 i;
                for (i = 0; i <= dt1.Rows.Count - 1; i++)
                {
                    string wrk = ((Label)GridView1.Rows[i].FindControl("lbl_wrk")).Text;
                    string name = ((Label)GridView1.Rows[i].FindControl("lbl_name")).Text;
                    string wrkhrs = ((Label)GridView1.Rows[i].FindControl("lbl_wrkhrs")).Text;

                    string category = ((Label)GridView1.Rows[i].FindControl("lbl_category")).Text;
                    string categorycode = ((Label)GridView1.Rows[i].FindControl("lbl_categorycode")).Text; //Added on 24.11.2022

                    string po_category = ((Label)GridView1.Rows[i].FindControl("lbl_po_category")).Text;
                    string po_categorycode = ((Label)GridView1.Rows[i].FindControl("lbl_po_categorycode")).Text;

                    string designation = ((Label)GridView1.Rows[i].FindControl("lbl_designation")).Text;
                    string designationcode = ((Label)GridView1.Rows[i].FindControl("lbl_designationcode")).Text; //Added on 24.11.2022
                    string gpno = ((Label)GridView1.Rows[i].FindControl("lbl_gpno")).Text;

                    string site = ((Label)GridView1.Rows[i].FindControl("lbl_wrksitename")).Text;
                    string sitecode = ((Label)GridView1.Rows[i].FindControl("lbl_wrksitecode")).Text;
                    string timein = ((Label)GridView1.Rows[i].FindControl("lbl_in")).Text;

                    string timeout = ((Label)GridView1.Rows[i].FindControl("lbl_out")).Text;
                    string lunch = ((Label)GridView1.Rows[i].FindControl("lbl_lunch")).Text;
                    string wrkmin = ((Label)GridView1.Rows[i].FindControl("lbl_wrkmin")).Text;
                    string wrkdhrs = ((Label)GridView1.Rows[i].FindControl("lbl_wrkdhrs")).Text;
                    string cot = ((Label)GridView1.Rows[i].FindControl("lbl_cot")).Text;
                    string ot = ((Label)GridView1.Rows[i].FindControl("lbl_pot")).Text;
                    string presentstatus = ((Label)GridView1.Rows[i].FindControl("lbl_presentstatus")).Text;

                    Int32 returnflag = InsertIntoAttendanceTable(wrk, name, site, sitecode, wrkhrs, category, categorycode, po_category, po_categorycode, designation, designationcode, gpno, timein, timeout, lunch, wrkmin, wrkdhrs, cot, ot, presentstatus);
                    if (returnflag != 0)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            return flag;
        }
        private Int32 InsertIntoAttendanceTable(string EmployeeWrk, string EmployeName, string SiteName, string SiteCode, string Workhours, string category, string categorycode, string po_category, string po_categorycode, string designation, string designationcode, string gpno, string InPunchTime, string OutPunchTime, string lunch, string wrkmin, string wrkdhrs, string cot, string pot, string presentstatus)
        {
            Int32 flag = 0;

            try
            {
                dbcl.Sqlconnection();
                SqlCommand cmd = new SqlCommand("SP_Insert_EmployeeAttendance", dbcl.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(lbl_jobiddate.Text.ToString()));
                cmd.Parameters.AddWithValue("@Creator_Name", lbl_jobcreatorname.Text.ToString());
                cmd.Parameters.AddWithValue("@Creator_Workman", lbl_creatorwrk.Text.ToString());
                cmd.Parameters.AddWithValue("@Creator_Region", lbl_creatorregion.Text.ToString());
                cmd.Parameters.AddWithValue("@Creator_Company", lbl_creatorcompany.Text.ToString());
                cmd.Parameters.AddWithValue("@Creator_SiteName", lbl_crtrsitename.Text.ToString());
                cmd.Parameters.AddWithValue("@Creator_SiteCode", lbl_crtrsitecode.Text.ToString());
                cmd.Parameters.AddWithValue("@JOBID", lbl_jobid.Text.ToString());
                cmd.Parameters.AddWithValue("@WorkOrderNo", lbl_wrkordr.Text.ToString());
                cmd.Parameters.AddWithValue("@PermitNo", lbl_permitno.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_Region", lbl_jobrgn.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_Company", lbl_jobcompay.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_SiteName", lbl_jobsite.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_SiteCode", lbl_jobsitecode.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_InchargeWrk", lbl_inchargewrk.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_InchargeName", lbl_inchargename.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_Dept", lbl_dept.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_Location", lbl_jobloc.Text.ToString());
                cmd.Parameters.AddWithValue("@SiteIncharge_Approval", "Approved");
                cmd.Parameters.AddWithValue("@SubmitterName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@SubmitterWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@SubmitterStatus", "Exit");
                cmd.Parameters.AddWithValue("@EmployeeName", EmployeName);
                cmd.Parameters.AddWithValue("@EmployeeWrk", EmployeeWrk);
                cmd.Parameters.AddWithValue("@Employee_Worksite", SiteName);
                cmd.Parameters.AddWithValue("@Employee_WorksiteCode", SiteCode);
                cmd.Parameters.AddWithValue("@WourkHours", Convert.ToInt32(Workhours));

                cmd.Parameters.AddWithValue("@EmpCategory", category);
                cmd.Parameters.AddWithValue("@Category_DB", categorycode); //Added on 24.11.2022


                cmd.Parameters.AddWithValue("@PO_SkillCategory", po_category); //Added on 23.11.2022
                cmd.Parameters.AddWithValue("@PO_SkillCategoryCode", po_categorycode); //Added on 23.11.2022

                cmd.Parameters.AddWithValue("@EmpDesignation", designation);
                cmd.Parameters.AddWithValue("@Designation_DB", designationcode); //Added on 24.11.2022

                cmd.Parameters.AddWithValue("@GatePassNo", gpno);

                cmd.Parameters.AddWithValue("@Inpunch_Time", InPunchTime);
                cmd.Parameters.AddWithValue("@Outpunch_Time", OutPunchTime);

                cmd.Parameters.AddWithValue("@LunchFactor", lunch);

                cmd.Parameters.AddWithValue("@WorkedTime", wrkmin);
                cmd.Parameters.AddWithValue("@WorkedHours", wrkdhrs);

                cmd.Parameters.AddWithValue("@Calc_OT", Convert.ToDecimal(cot));
                cmd.Parameters.AddWithValue("@ProvidedOT", Convert.ToDecimal(pot));

                cmd.Parameters.AddWithValue("@LastModified", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));

                cmd.Parameters.AddWithValue("@AttendanceStatus", "Present");
                cmd.Parameters.AddWithValue("@AttendanceCode", presentstatus);

                cmd.Parameters.AddWithValue("@Approval_Date", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));

                dbcl.ConnectDb();
                flag = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                lbl_finalmsg.Visible = true;
                lbl_finalmsg.Text = ex.Message;
                lbl_finalmsg.ForeColor = System.Drawing.Color.IndianRed;
                dbcl.DisconnectDb();
            }

            if (flag != 0)
            {
                lbl_finalmsg.Visible = true;
                lbl_finalmsg.Text = "Record Inserted Successfully..!";
                lbl_finalmsg.ForeColor = System.Drawing.Color.DarkGreen;
                dbcl.DisconnectDb();
            }
            else
            {
                //lbl_msg.Visible = true;
                //lbl_msg.Text = "Records Connot be Inserted into the Database";
                //lbl_msg.ForeColor = System.Drawing.Color.IndianRed;
                //dbcl.DisconnectDb();
            }
            return flag;
        }

        private void AddDefaultFirstRecord()
        {
            //creating dataTable
            DataTable dt = new DataTable();
            DataRow dr;
            dt.TableName = "Attendance";
            dt.Columns.Add(new DataColumn("slno", typeof(string)));
            dt.Columns.Add(new DataColumn("wrk", typeof(string)));
            dt.Columns.Add(new DataColumn("name", typeof(string)));
            dt.Columns.Add(new DataColumn("wrkhrs", typeof(string)));
            dt.Columns.Add(new DataColumn("category", typeof(string)));
            dt.Columns.Add(new DataColumn("categorycode", typeof(string))); //Added on 24.11.2022

            dt.Columns.Add(new DataColumn("designation", typeof(string)));
            dt.Columns.Add(new DataColumn("designationcode", typeof(string))); //Added on 24.11.2022
            //added on 23.11.2022
            dt.Columns.Add(new DataColumn("po_category", typeof(string))); //Added on 23.11.2022
            dt.Columns.Add(new DataColumn("po_categorycode", typeof(string))); //Added on 23.11.2022
            dt.Columns.Add(new DataColumn("gpno", typeof(string)));
            dt.Columns.Add(new DataColumn("wrksitename", typeof(string)));
            dt.Columns.Add(new DataColumn("wrksitecode", typeof(string)));
            dt.Columns.Add(new DataColumn("in", typeof(string)));
            dt.Columns.Add(new DataColumn("out", typeof(string)));
            dt.Columns.Add(new DataColumn("lunch", typeof(string)));
            dt.Columns.Add(new DataColumn("wrkmin", typeof(string)));
            dt.Columns.Add(new DataColumn("wrkdhrs", typeof(string)));
            dt.Columns.Add(new DataColumn("cot", typeof(string)));
            dt.Columns.Add(new DataColumn("pot", typeof(string)));
            dt.Columns.Add(new DataColumn("presentstatus", typeof(string)));
            dr = dt.NewRow();
            dt.Rows.Add(new object[] { 1, "No Data", "No Data", "No Data", "No Data", "No Data", "No Data", "No Data", "No Data", "No Data", "No Data", "No Data", "No Data", "No Data", "No Data", "No Data" , "No Data" });

            //saving databale into viewstate
            ViewState["Attendance"] = dt;

            //bind Gridview
            BindMyGridview();
        }

        protected void btn_back_Click(object sender, EventArgs e)
        {
            if (Session["WORKMAN"].ToString() == lbl_inchargewrk.Text.ToString())
            {
                Response.Redirect("jobapprovalpage.aspx?JOBID=" + lbl_jobid.Text.ToString() + "", false);
            }
            else
            {
                Response.Redirect("view_dailyjobs.aspx?Date=" + lbl_jobiddate.Text.ToString() + "", false);
            }
        }
    }
}