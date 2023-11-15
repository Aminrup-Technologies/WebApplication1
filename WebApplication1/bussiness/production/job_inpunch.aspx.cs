using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Drawing;
namespace WebApplication1.bussiness.production
{
    public partial class job_inpunch : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
        DataTable dt = new DataTable();
        DataTable FirstDatatable = new DataTable();
        public static Boolean jhanda = false;
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
                    InpunchPanel_Row.Visible = true;
                    ActiveJOB_Checker();
                }
            }
        }

        private void ActiveJOB_Checker()
        {
            Int32 Activejobcount = CC.Find_ActiveJOBCountforINPunch(Session["WORKMAN"].ToString(), Session["REGION"].ToString());
            if (Activejobcount > 0)
            {

                dbcl.FillCombo(DDL_JOBID, "select TOP 3 JOBID from tbl_jobs where Creator_Workman='" + Session["WORKMAN"].ToString() + "' and JOBID_Status='Active' order by CreatedDate desc ");

                AddDefaultFirstRecord();

                //if (Session["REGION"].ToString() ==  "AGL")
                //{
                //    dbcl.FillCombo(DDL_JOBID, "select JOBID from tbl_jobs where Creator_Workman='" + Session["WORKMAN"].ToString() + "' and JOBID_Status='Active' order by CreatedDate desc ");

                //    AddDefaultFirstRecord();
                //}
                //else
                //{
                //    dbcl.FillCombo(DDL_JOBID, "select JOBID from tbl_jobs where Creator_Workman='" + Session["WORKMAN"].ToString() + "' and JOBID_Status='Active' and FinalUpldStatus='Yes' order by CreatedDate desc ");

                //    AddDefaultFirstRecord();
                //}
            }
            else
            {
                string title = "Notifications :";
                string body = "NO Active JOB ID Found...! Kindly create a JOB ID and proceed.";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void btn_createjobid_Click(object sender, EventArgs e)
        {
            Response.Redirect("create_jobid.aspx");
        }

        protected void txt_empworkman_TextChanged(object sender, EventArgs e)
        {
            string entered_workman = txt_empworkman.Text.TrimEnd().ToString();
            PendingOUTMsg.Visible = false;
            //Int32 count = 0;
            //if (count == 0)
            //{
            //    jhanda = false;
            //}
            //else
            //{
            //    jhanda = true;
            //}
            if (jhanda == false)
            {
                if (entered_workman != string.Empty)
                {
                    if (CheckSDuplicateEntry() == false)
                    {
                        if (dbcl.CheckEmployeeActiveStatus(entered_workman) == true)
                        {
                            if (CC.Check_EmployeePunchOUT(entered_workman) == 0)
                            {
                                _EmployeeDataBinder(entered_workman);
                                //count++;
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

                                GPValidty_row.Visible = false;
                                EmployeeWorksite_Row.Visible = false;
                                InPunchDate_Row.Visible = false;
                                InPunchTime_Row.Visible = false;
                                btn_submit.Visible = false;
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
                            GPValidty_row.Visible = false;
                        }
                    }
                    else
                    {
                        string title = "Notifications :";
                        string body = "Employee already added";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                        txt_empworkman.Text = "";

                        EmployeeName_Row.Visible = false;
                        GPValidty_row.Visible = false;
                        EmployeeWorksite_Row.Visible = false;
                        InPunchDate_Row.Visible = false;
                        InPunchTime_Row.Visible = false;
                        Addto_buttons.Visible = false;

                    }
                }
                else
                {
                    txt_empworkman.Focus();
                }
            }
            else
            {

            }
        }

        private void _EmployeeDataBinder(string entered_workman)
        {
            try
            {
                Boolean flag1 = false;
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

                //Modified on 13-0-2022 for fetching gatepass expiry, safety validty and PV Expiry
                dbcl.FindEmployeeDataforInPunch(entered_workman, ref name, ref workhours, ref worksitename, ref worksitecode, ref category, ref categorycode, ref designation, ref designationcode, ref gpno, ref gpexp, ref sftyno, ref sftyexp, ref pvexp);

                string po_skill = ""; //Added on 24.11.2022
                string po_skillcode = ""; //Added on 24.11.2022
                PO_SkillPull(ref designation, ref po_skill, ref po_skillcode);  //Added on 24.11.2022
                                                                                //Below logic is added on 24.11.2022
                if (po_skill == "" && po_skillcode == "")
                {
                    po_skill = category;
                    po_skillcode = categorycode;
                    flag1 = true;
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

                string gpvaldt = DateBinder(gpexp);
                lbl_oldgpno.Text = gpno;
                lbl_gpvalidty.Text = gpvaldt.ToString();
                lbl_oldgpvalidity.Text = gpvaldt;

                lbl_oldsftyno.Text = sftyno;
                lbl_sftyno.Text = sftyno;  //Added on 27-01-2023

                string rfidvaldt = DateBinder(sftyexp);
                lbl_oldsftyval.Text = rfidvaldt;
                txt_nwsftyvalidity.Text = rfidvaldt;

                string pvvaldt = DateBinder(pvexp);
                lbl_oldpvvalidity.Text = pvvaldt;
                txt_nwpvvalidity.Text = pvvaldt;

                Int32 gpdays = 0;
                FindDaysLeft(gpexp, ref gpdays);
                if (gpdays < 0)
                {
                    lbl_gpdays.Text = gpdays.ToString();
                    lbl_gpdays.ForeColor = System.Drawing.Color.Red;
                    btn_submit.Visible = false;
                    Image1.Visible = true;

                    btnShowPopup2.Visible = true;
                    string title = "Notifications :";
                    string body = "Kindly update your Gatepass Data, Your Gatepass has expired...!!!";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    btnShowPopup2.Visible = false;
                    btn_submit.Visible = true;
                    btn_submit.Enabled = true;
                    Image1.Visible = false;
                    lbl_gpdays.Text = gpdays.ToString();
                    lbl_gpdays.ForeColor = System.Drawing.Color.Green;
                }



                EmployeeName_Row.Visible = true;
                GPValidty_row.Visible = true;
                EmployeeWorksite_Row.Visible = true;
                InPunchDate_Row.Visible = true;
                InPunchTime_Row.Visible = true;
                Addto_buttons.Visible = true;

                if (flag1 == true)
                {
                    string title = "Notifications :";
                    string body = "No Mapped PO Skill Category Found";
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

        public void FindDaysLeft(string emp_outtime, ref Int32 day)
        {
            string currentdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            DateTime intm = DateTime.Parse(currentdate.ToString());
            DateTime outm = DateTime.Parse(emp_outtime.ToString());

            TimeSpan duration = outm.Subtract(intm);
            day = duration.Days;
        }

        private string DateBinder(string date)
        {
            string newdate = "";
            DateTime oDate = Convert.ToDateTime(date);
            string day = "";
            string month = "";
            if (oDate.Day < 10)
            {
                day = "0" + oDate.Day.ToString();
            }
            else
            {
                day = oDate.Day.ToString();
            }
            if (oDate.Month < 10)
            {
                month = "0" + oDate.Month.ToString();
            }
            else
            {
                month = oDate.Month.ToString();
            }
            return newdate = day + "-" + month + "-" + oDate.Year;
        }
        protected void DDL_JOBID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ddljobid = DDL_JOBID.SelectedItem.Text.ToString();
            Bind_JOBIDDetails(ddljobid);
            JOBIDDetails_Row.Visible = true;
            WorkmanInput_Row.Visible = true;
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
                    string date = dt.Rows[0]["CreatedDate"].ToString();
                    lbl_jobiddate.Text = date;
                    txt_date.Text = date;

                    lbl_jobcreatorname.Text = dt.Rows[0]["Creator_Name"].ToString();
                    lbl_creatorwrk.Text = dt.Rows[0]["Creator_Workman"].ToString();
                    lbl_creatorregion.Text = dt.Rows[0]["Creator_Region"].ToString();
                    lbl_creatorcompany.Text = dt.Rows[0]["Creator_Company"].ToString();
                    lbl_crtrsitename.Text = dt.Rows[0]["Creator_Site"].ToString();
                    lbl_crtrsitecode.Text = dt.Rows[0]["Creator_SiteCode"].ToString();

                    string wrkordr = dt.Rows[0]["WorkOrderNo"].ToString();
                    lbl_wrkordr.Text = wrkordr;

                    lbl_jobid.Text = dt.Rows[0]["JOBID"].ToString();

                    string rgn = dt.Rows[0]["JOB_Region"].ToString();
                    lbl_jobrgn.Text = rgn;

                    string comp = dt.Rows[0]["JOB_Company"].ToString();
                    lbl_jobcompay.Text = comp;

                    DT_Binder(rgn, comp, wrkordr);

                    lbl_jobsite.Text = dt.Rows[0]["JOB_Site"].ToString();
                    lbl_jobsitecode.Text = dt.Rows[0]["JOB_SiteCode"].ToString();
                    lbl_inchargewrk.Text = dt.Rows[0]["JOB_InchargeWrk"].ToString();
                    lbl_inchargename.Text = dt.Rows[0]["JOB_InchargeName"].ToString();
                    lbl_dept.Text = dt.Rows[0]["JOB_Dept"].ToString();
                    lbl_jobloc.Text = dt.Rows[0]["JOB_Location"].ToString();
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


        //Added on 24.11.2022
        private void DT_Binder(string rgn, string comp, string wrkordr)
        {
            try
            {
                string cmdString = "Select Designation_Type, Designation_DB, WO_Category_Type,WO_Category_Code from tlb_WO_SkillCategory where Work_Region_Code='" + rgn + "' and Company_Code='" + comp + "' and WO_Number = '"+ wrkordr + "' order by Id";
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                FirstDatatable = null;
                if (dr.Read())
                {
                    FirstDatatable = dbcl.GetDataTable(cmdString);
                }
                ViewState["dt1"] = FirstDatatable;
                dbcl.Conn.Close();
            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        private void CheckforAttachedAttendnace()
        {
            string ddljobid = DDL_JOBID.SelectedItem.Text.ToString();
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
                Bind_AttendanceGridView();
            }
            dbcl.DisconnectDb();
        }

        private void Bind_AttendanceGridView()
        {
            string ddljobid = DDL_JOBID.SelectedItem.Text.ToString();
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdString = "Select * from tbl_attendance where Creator_Workman='" + Session["WORKMAN"].ToString() + "' and JOBID='" + ddljobid + "' and AttendanceStatus = 'Entry' order by Id";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dtbl = new DataTable();
            ad.Fill(dtbl);
            GridView2.DataSource = dtbl;
            GridView2.DataBind();
            dbcl.Conn.Close();
        }

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label ID = (Label)GridView2.Rows[e.RowIndex].FindControl("lbl_Id");
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

                string ddljobid = DDL_JOBID.SelectedItem.Text.ToString();
                Bind_JOBIDDetails(ddljobid);

                string title = "Notifications :";
                string body = "Employee Removed Successfully";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                //throw;
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
            dt.Columns.Add(new DataColumn("sftyno", typeof(string))); //Added on 27-01-2023

            dt.Columns.Add(new DataColumn("wrksitename", typeof(string)));
            dt.Columns.Add(new DataColumn("wrksitecode", typeof(string)));
            dt.Columns.Add(new DataColumn("in", typeof(string)));
            dr = dt.NewRow();
            dt.Rows.Add(new object[] { 1, "No Data" });

            //saving databale into viewstate
            ViewState["Attendance"] = dt;

            //bind Gridview
            BindMyGridview();
        }

        protected void ADDTOLIST()
        {
            if (ViewState["Attendance"] != null)
            {
                //get datatable from view state
                DataTable dtCurrentTable = (DataTable)ViewState["Attendance"];
                DataRow drCurrentRow = null;

                string intime = txt_date.Text.TrimEnd().ToString() + " " + txt_time.Text.ToString();
                string current = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                DateTime crnttym = DateTime.ParseExact(current, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);

                //The below function is used to Check When user in Performing INPUNCH ---- Relaxation to Entry
                Int32 DelayedHours = 0;
                dbcl.FindInpunchEligibilty(intime, current, ref DelayedHours);

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

                        drCurrentRow["designation"] = lbl_designation.Text.Trim().ToString();
                        drCurrentRow["designationcode"] = lbl_designationcode.Text.Trim().ToString(); //Added on 24.11.2022

                        //----------Added on 24.11.2022--------------------------------//
                        drCurrentRow["po_category"] = lbl_pocategoryname.Text.Trim().ToString();
                        drCurrentRow["po_categorycode"] = lbl_pocategorycode.Text.Trim().ToString();
                        //----------Added on 24.11.2022--------------------------------//

                        drCurrentRow["gpno"] = lbl_gpno.Text.Trim().ToString();
                        //----------Added on 13-05-20201--------------------------------//

                        drCurrentRow["sftyno"] = lbl_sftyno.Text.Trim().ToString(); //----------Added on 27-01-2023----------------Purnima----------------//

                        drCurrentRow["wrksitename"] = txt_worksite.Text.ToString();
                        drCurrentRow["wrksitecode"] = lbl_worksitecode.Text.ToString();
                        drCurrentRow["in"] = intime.ToString();
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
                    lbl_pocategoryname.Text = ""; //Added on 24.11.2022
                    lbl_pocategorycode.Text = ""; //Added on 24.11.2022

                    EmployeeName_Row.Visible = false;
                    EmployeeWorksite_Row.Visible = false;
                    InPunchDate_Row.Visible = false;
                    InPunchTime_Row.Visible = false;
                    GPValidty_row.Visible = false;
                    Addto_buttons.Visible = false;
                    btn_submit.Enabled = false;
                }
            }
        }

        //function to delete rows from the view list
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["Attendance"];
            DataRow dr = dtCurrentTable.Rows[e.RowIndex];
            dtCurrentTable.Rows.Remove(dr);
            GridView1.EditIndex = -1;
            BindMyGridview();
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (txt_empname.Text.ToString() != "" && txt_worksite.Text.ToString() != "" && txt_date.Text.ToString() != "" && txt_time.Text.ToString() != "")
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

        protected void btn_finalsubmit_Click(object sender, EventArgs e)
        {

            if (DataTablePull() == true)
            {
                AttendanceSentMesg.Visible = true;
                SendAttendance_Buttons.Visible = false;
                ViewState_TableRow.Visible = false;
                Addto_buttons.Visible = false;
                InpunchPanel_Row.Visible = false;

                if (Session["REGION"].ToString() == "NINL")
                {
                    inpunched_buttons.Visible = false;
                }
                else
                {
                    inpunched_buttons.Visible = true;
                }
                btn_tbtpage.Visible = true;
                btn_soppage.Visible = true;
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

        protected Boolean CheckSDuplicateEntry()
        {
            Boolean flag = false;
            Int32 dupcount = 0;
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
                        dupcount++;
                    }
                }
            }

            if (dupcount > 0 )
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
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
                    string sftyno = ((Label)GridView1.Rows[i].FindControl("lbl_sftyno")).Text;

                    string site = ((Label)GridView1.Rows[i].FindControl("lbl_wrksitename")).Text;
                    string sitecode = ((Label)GridView1.Rows[i].FindControl("lbl_wrksitecode")).Text;
                    string timein = ((Label)GridView1.Rows[i].FindControl("lbl_in")).Text;

                    Int32 returnflag = InsertIntoAttendanceTable(wrk, name, site, sitecode, wrkhrs, category, categorycode, po_category, po_categorycode, designation, designationcode, gpno, sftyno, timein);
                    if (returnflag != 0)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                //Update manpower count in job table for view in list, added on 15-04-2023
                dbcl.UPDT_ManpowerCountJOBID(dt1.Rows.Count, lbl_jobid.Text.ToString());
            }
            return flag;
        }
        private Int32 InsertIntoAttendanceTable(string EmployeeWrk, string EmployeName, string SiteName, string SiteCode, string Workhours, string category, string categorycode, string po_category, string po_categorycode, string designation, string designationcode, string gpno, string sftyno, string InPunchTime)
        {
            Int32 flag = 0;

            try
            {
                dbcl.Sqlconnection();
                SqlCommand cmd = new SqlCommand("SP_InsertInto_AttendanceTable", dbcl.Conn);
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
                cmd.Parameters.AddWithValue("@SiteIncharge_Approval", "Pending");
                cmd.Parameters.AddWithValue("@SubmitterName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@SubmitterWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@SubmitterStatus","Entry");
                cmd.Parameters.AddWithValue("@EmployeeName", EmployeName);
                cmd.Parameters.AddWithValue("@EmployeeWrk", EmployeeWrk);
                cmd.Parameters.AddWithValue("@Employee_Worksite", SiteName);
                cmd.Parameters.AddWithValue("@Employee_WorksiteCode", SiteCode);
                cmd.Parameters.AddWithValue("@WourkHours", Workhours);

                cmd.Parameters.AddWithValue("@EmpCategory", category);
                cmd.Parameters.AddWithValue("@Category_DB", categorycode); //Added on 24.11.2022


                cmd.Parameters.AddWithValue("@PO_SkillCategory", po_category); //Added on 23.11.2022
                cmd.Parameters.AddWithValue("@PO_SkillCategoryCode", po_categorycode); //Added on 23.11.2022

                cmd.Parameters.AddWithValue("@EmpDesignation", designation);
                cmd.Parameters.AddWithValue("@Designation_DB", designationcode); //Added on 24.11.2022

                cmd.Parameters.AddWithValue("@GatePassNo", gpno);
                cmd.Parameters.AddWithValue("@SafetyPassNo", sftyno); //Added on 27-01-2023 , ATS-GL-- Anupam Sharma

                cmd.Parameters.AddWithValue("@Inpunch_Time", InPunchTime);
                cmd.Parameters.AddWithValue("@AttendanceStatus", "Entry");
                cmd.Parameters.AddWithValue("@AttendanceCode", "Ab");

                dbcl.ConnectDb();
                flag = cmd.ExecuteNonQuery();

                UpdateJOBTableStatus();
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

        protected void UpdateJOBTableStatus()
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_jobs set JOB_Status=@JOB_Status, MasterStatusCode=@MasterStatusCode , EntryExit=@EntryExit where JOBID=@JOBID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@JOBID", lbl_jobid.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_Status", "In-Punch Done");
                cmd.Parameters.AddWithValue("@MasterStatusCode", "3");  // JOBID created, Permit Uploaded, Can Proceed to Entry Page
                cmd.Parameters.AddWithValue("@EntryExit", "Entry");
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

        protected void btn_tbtpage_Click(object sender, EventArgs e)
        {
            Response.Redirect("csm_toolboxtalk.aspx");
        }

        protected void btn_hmpg_Click(object sender, EventArgs e)
        {
            Response.Redirect("homepage.aspx");
        }



        protected void btn_gtpsedit_Click(object sender, EventArgs e)
        {
            if (btn_gtpsedit.Text.ToString() == "Make Changes")
            {
                nwgprow1.Visible = true;
                nwgprow2.Visible = true;

                nwgpvalrow1.Visible = true;
                nwgpvalrow2.Visible = true;

                nwsftyrow1.Visible = true;
                nwsftyrow2.Visible = true;

                nwrfidrow1.Visible = true;
                nwrfidrow2.Visible = true;

                nwpvrow1.Visible = true;
                nwpvrow2.Visible = true;

                btn_gtpsedit.Text = "Save Changes";

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup2();", true);
            }
            else if (btn_gtpsedit.Text.ToString() == "Save Changes")
            {

                ReflectNewGPData();

                nwgprow1.Visible = false;
                nwgprow2.Visible = false;

                nwgpvalrow1.Visible = false;
                nwgpvalrow2.Visible = false;

                nwsftyrow1.Visible = false;
                nwsftyrow2.Visible = false;

                nwrfidrow1.Visible = false;
                nwrfidrow2.Visible = false;

                nwpvrow1.Visible = false;
                nwpvrow2.Visible = false;

                btn_gtpsedit.Text = "Make Changes";

                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup2();", true);
            }
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {


            nwgprow1.Visible = false;
            nwgprow2.Visible = false;

            nwgpvalrow1.Visible = false;
            nwgpvalrow2.Visible = false;

            nwsftyrow1.Visible = false;
            nwsftyrow2.Visible = false;

            nwrfidrow1.Visible = false;
            nwrfidrow2.Visible = false;

            nwpvrow1.Visible = false;
            nwpvrow2.Visible = false;

            btn_gtpsedit.Text = "Make Changes";
        }


        private void ReflectNewGPData()
        {
            string nwgpno = txt_nwgpno.Text.ToString();
            string nwgpval = txt_nwgpvalidity.Text.ToString();

            string nwsftyno = txt_nwsftyno.Text.ToString();
            string nwsftyval = txt_nwsftyvalidity.Text.ToString();

            string nepvval = txt_nwpvvalidity.Text.ToString();

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_Employee_Mustertable set GatePassNo=@GatePassNo, GatePassExpiry=@GatePassExpiry, SafetyPassNo=@SafetyPassNo, SafetyPassExpiry=@SafetyPassExpiry, PVExpiry=@PVExpiry, GP_ModifierWrk=@GP_ModifierWrk, GP_ModifierName=@GP_ModifierName, GP_ModifiedDate=@GP_ModifiedDate, GP_UpdateApproval=@GP_UpdateApproval where WorkmanSL=@WorkmanSL";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@WorkmanSL", txt_empworkman.Text.ToString());
                cmd.Parameters.AddWithValue("@GatePassNo", nwgpno);
                cmd.Parameters.AddWithValue("@GatePassExpiry", nwgpval);
                cmd.Parameters.AddWithValue("@SafetyPassNo", nwsftyno);
                cmd.Parameters.AddWithValue("@SafetyPassExpiry", nwsftyval);
                cmd.Parameters.AddWithValue("@PVExpiry", nepvval);
                cmd.Parameters.AddWithValue("@GP_ModifierWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@GP_ModifierName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@GP_ModifiedDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@GP_UpdateApproval", "Pending");
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                //EmployeeDataLoader();

                _EmployeeDataBinder(txt_empworkman.Text.ToString());

                string title = "Notifications :";
                string body = "Data saved Successfully";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message.ToString();
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void EmployeeDataLoader()
        {
            string query = "select * from tbl_Employee_Mustertable where WorkmanSL=@WorkmanSL and LoginID=@LoginID";
            SqlParameter[] pram = {
                                          new SqlParameter("@WorkmanSL",Session["WORKMAN"].ToString()),
                                          new SqlParameter("@LoginID",Session["USERID"].ToString()),
                                      };
            dt = dbcl.SPreturn_dt(query, pram);
            if (dt.Rows.Count > 0)
            {
                string WorkStatus = dt.Rows[0]["WorkStatus"].ToString();

                if (WorkStatus == "Active")
                {
                    string gpno = dt.Rows[0]["GatePassNo"].ToString();
                    lbl_gpno.Text = gpno;
                    lbl_oldgpno.Text = gpno;
                    txt_nwgpno.Text = gpno;

                    string gpval = dt.Rows[0]["GatePassExpiry"].ToString();
                    Int32 gpdays = 0;
                    FindDaysLeft(gpval, ref gpdays);
                    if (gpdays < 14)
                    {
                        lbl_gpno.ForeColor = Color.OrangeRed;
                        lbl_oldgpno.ForeColor = Color.OrangeRed;
                    }
                    string gpvaldt = DateBinder(gpval);
                    lbl_oldgpvalidity.Text = gpvaldt;
                    txt_nwgpvalidity.Text = gpvaldt;

                    string rfidno = dt.Rows[0]["SafetyPassNo"].ToString();
                    lbl_oldsftyno.Text = rfidno;
                    txt_nwsftyno.Text = rfidno;

                    string rfidval = dt.Rows[0]["SafetyPassExpiry"].ToString();
                    string rfidvaldt = DateBinder(rfidval);
                    lbl_oldsftyval.Text = rfidvaldt;
                    txt_nwsftyvalidity.Text = rfidvaldt;

                    string pvvalidity = dt.Rows[0]["PVExpiry"].ToString();
                    string pvvaldt = DateBinder(pvvalidity);
                    lbl_oldpvvalidity.Text = pvvaldt;
                    txt_nwpvvalidity.Text = pvvaldt;

                    dbcl.DisconnectDb();
                }
                else
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "AlertMessage", "<script>alert('User ID is InActive');</script>");
                }
            }
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            txt_empworkman.Text = "";
            txt_empworkman.Focus();

            EmployeeName_Row.Visible = false;
            EmployeeWorksite_Row.Visible = false;
            InPunchDate_Row.Visible = false;
            InPunchTime_Row.Visible = false;
            Addto_buttons.Visible = false;
            GPValidty_row.Visible = false;
        }
    }
}