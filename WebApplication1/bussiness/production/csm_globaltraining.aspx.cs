using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.bussiness.production
{
    public partial class csm_globaltraining : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        DataTable dt = new DataTable();

        public static string trntype = string.Empty;
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
                    trntype = Request.QueryString["type"];
                    RBTN_JOBYesNo.Focus();

                    WorksiteBinder();
                    Bind_SiteInchargeName();

                    sftysupvrow.Visible = true;
                    string CmdString = "select FullName, WorkmanSL from tbl_Employee_Mustertable where SkillDesignation = 'SAFETY SUPERVISOR' and WorkStatus = 'Active' and WorkRegion = '" + Session["REGION"].ToString() + "' order by Id";
                    Bind_SafetySupervisors(CmdString);

                    sftyofcrrow.Visible = true;
                    string CmdString1 = "select FullName, WorkmanSL from tbl_Employee_Mustertable where SkillDesignation = 'SAFETY OFFICER' and WorkStatus = 'Active' and WorkRegion = '" + Session["REGION"].ToString() + "' order by Id";
                    Bind_SafetyOfficer(DDL_SftyOfcr, CmdString1);

                    traningbasicdata.Visible = false;


                    traningagenda.Visible = true;
                    AddDefaultFirstRecord();
                }
            }
        }

        protected void WorksiteBinder()
        {
            string CmdString2 = "select Worksite_Name, DB_Code from tlb_atsworksites where WorkRegion_Code='" + Session["REGION"].ToString() + "' order by Id";
            BindWorkSites(CmdString2);
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

        protected void Bind_SiteInchargeName()
        {
            string Worksite_DBCode = DDL_Worksite.SelectedValue.ToString();
            string CmdString = "select Employee_Name, Employee_Workman from tlb_atsworksiteIncharges where DB_Code='" + Worksite_DBCode + "' and Status='Active' order by Id";
            Bind_Approver(CmdString);
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

        protected void RBTN_JOBYesNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBTN_JOBYesNo.SelectedIndex == 0)
            {
                JOBIDInputRow.Visible = false;

                traningbasicdata.Visible = true;

                WorksiteBinder();

                DDL_Approver.Enabled = false;
                DDL_Location.Enabled = false;
                JOBIDDetails_Row.Visible = false;
                AttachedAttendanceRow.Visible = false;

                txt_cntrctemp.Text = "";
                DDL_Approver.SelectedIndex = 0;
                DDL_SftyOfcr.SelectedIndex = 0;
                DDL_SftySupv.SelectedIndex = 0;
                //DDL_Location.SelectedIndex = 0;
                txt_sopno.Text = "";
                txt_sopdesc.Text = "";
                txt_faculty.Text = Session["USERNAME"].ToString();
                txt_duration.Text = "";
            }
            else
            {
                Bind_SiteInchargeName();
                JOBIDInputRow.Visible = true;
                txt_jobid.Focus();
                txt_jobid.Text = "";

                traningbasicdata.Visible = false;
            }
        }

        protected void txt_jobid_TextChanged(object sender, EventArgs e)
        {
            string jobid = txt_jobid.Text.ToUpper().ToString();
            JOBIDDetails_Row.Visible = true;
            AttachedAttendanceRow.Visible = true;

            Bind_JOBIDDetails(jobid);

            //traningbasicdata.Visible = true;

            worksiterow.Visible = true;
            sftyofcrrow.Visible = true;
            sftysupvrow.Visible = true;
            sopinput_1.Visible = true;
            sopinput_2.Visible = true;
            sopinput_3.Visible = true;
            sopinput_4.Visible = true;

            CreateSOPIDRow.Visible = true;
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
                    string jobregion = dt.Rows[0]["JOB_Region"].ToString();

                    if (jobregion == Session["REGION"].ToString())
                    {
                        traningbasicdata.Visible = true;
                        lbl_jobrgn.Text = jobregion;

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
                        lbl_jobshift.Text = dt.Rows[0]["JOB_Shift"].ToString();
                        lbl_permitno.Text = dt.Rows[0]["JOB_PermitNo"].ToString();
                        string supv = dt.Rows[0]["Creator_Name"].ToString();
                        lbl_jobcreatorname.Text = supv;


                        //txt_faculty.Text = supv;
                        txt_faculty.Text = Session["USERNAME"].ToString();

                        lbl_crtrsitecode.Text = dt.Rows[0]["Creator_SiteCode"].ToString();
                        lbl_wrkordr.Text = dt.Rows[0]["WorkOrderNo"].ToString();
                        lbl_jobid.Text = dt.Rows[0]["JOBID"].ToString();
                        lbl_jobtitle.Text = dt.Rows[0]["JOB_Title"].ToString();

                        lbl_jobcompay.Text = dt.Rows[0]["JOB_Company"].ToString();
                        lbl_jobsite.Text = dt.Rows[0]["JOB_Site"].ToString();

                        string worksitecode = dt.Rows[0]["JOB_SiteCode"].ToString();
                        lbl_jobsitecode.Text = worksitecode;

                        string loc = dt.Rows[0]["JOB_Location"].ToString();
                        lbl_jobloc.Text = loc;

                        string incharge = dt.Rows[0]["JOB_InchargeName"].ToString();
                        lbl_inchargename.Text = incharge;

                        string dept = dt.Rows[0]["JOB_Dept"].ToString();
                        lbl_dept.Text = dept;

                        Bind_SiteInchargeName();

                        string inchargwrk = dt.Rows[0]["JOB_InchargeWrk"].ToString();
                        lbl_inchargewrk.Text = inchargwrk;
                        DDL_Approver.SelectedValue = inchargwrk;
                        DDL_Approver.Enabled = false;

                        DDL_Worksite.SelectedValue = worksitecode;
                        DDL_Worksite.Enabled = false;

                        DDL_Location.SelectedItem.Text = loc;
                        DDL_Location.Enabled = false;

                        BindDeptLocation(dept);

                        CheckforAttachedAttendnace();
                    }
                    else
                    {
                        string title = "Notifications :";
                        string body = "Enter JOBID from your Work Area...!!";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                        JOBIDDetails_Row.Visible = false;
                        AttachedAttendanceRow.Visible = false;
                        traningbasicdata.Visible = false;
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
            string ddljobid = txt_jobid.Text.ToUpper().ToString();
            dbcl.Sqlconnection();
            string cmdstring = "select count (Id) from tbl_attendance where JOBID= '" + ddljobid.ToString() + "'";
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

        private void Bind_AttendanceGridView()
        {
            string ddljobid = txt_jobid.Text.ToUpper().ToString();
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string CmdString = "Select * from tbl_attendance where JOBID='" + ddljobid + "' order by Id";
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


            if (sopnum != "" && soptitle != "")
            {
                if (Insert_SOPIDCreationData() == true)
                {
                    ID_CreatedMsg.Visible = true;

                    CreateSOPIDRow.Visible = false;
                    ContractEmployeesrow.Visible = false;
                    sftyofcrrow.Visible = false;
                    sftysupvrow.Visible = false;

                    worksiterow.Visible = false;
                    approverrow.Visible = false;
                    Locationrow.Visible = false;

                    sopinput_1.Visible = false;
                    sopinput_2.Visible = false;
                    sopinput_3.Visible = false;
                    sopinput_4.Visible = false;



                    string title = "Notifications :";
                    string body = "Training ID Created";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                    RBTN_JOBYesNo.Enabled = false;
                    txt_jobid.ReadOnly = true;

                    traningagenda.Visible = true;
                    AddDefaultFirstRecord();
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Record cannot be inserted";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "SOP Number * SOP Title Required";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
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
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
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
                SqlCommand cmd = new SqlCommand("SP_InsertInto_tbl_trainingrecords", dbcl.Conn);
                cmd.CommandType = CommandType.StoredProcedure;


                if (RBTN_JOBYesNo.SelectedIndex ==0)
                {
                    cmd.Parameters.AddWithValue("@Ref_JOBID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ref_JOBTitle", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ref_JOBDate", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ref_JOBShift", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ref_JOBRegion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ref_JOBSupvName", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ref_JOBSupvWrk", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ref_JOBSite", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ref_JOBSiteCode", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ref_JOBIncharge", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ref_JOBInchargeWrk", DBNull.Value);
                }
                else if (RBTN_JOBYesNo.SelectedIndex ==1)
                {
                    cmd.Parameters.AddWithValue("@Ref_JOBID", lbl_jobid.Text.ToString());
                    cmd.Parameters.AddWithValue("@Ref_JOBTitle", lbl_jobtitle.Text.ToString());
                    cmd.Parameters.AddWithValue("@Ref_JOBDate", lbl_jobiddate.Text.ToString());
                    cmd.Parameters.AddWithValue("@Ref_JOBShift", lbl_jobshift.Text.ToString());
                    cmd.Parameters.AddWithValue("@Ref_JOBRegion", lbl_jobrgn.Text.ToString());
                    cmd.Parameters.AddWithValue("@Ref_JOBSupvName", lbl_jobcreatorname.Text.ToString());
                    cmd.Parameters.AddWithValue("@Ref_JOBSupvWrk", lbl_creatorwrk.Text.ToString());
                    cmd.Parameters.AddWithValue("@Ref_JOBSite", lbl_jobsite.Text.ToString());
                    cmd.Parameters.AddWithValue("@Ref_JOBSiteCode", lbl_jobsitecode.Text.ToString());
                    cmd.Parameters.AddWithValue("@Ref_JOBIncharge", lbl_inchargename.Text.ToString());
                    cmd.Parameters.AddWithValue("@Ref_JOBInchargeWrk", lbl_inchargewrk.Text.ToString());
                }
                cmd.Parameters.AddWithValue("@TRN_Type", trntype);
                cmd.Parameters.AddWithValue("@TRN_Date", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@TRN_ID", id);
                cmd.Parameters.AddWithValue("@TRN_Region", Session["REGION"].ToString());
                cmd.Parameters.AddWithValue("@TRN_Dept", DDL_Worksite.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@TRN_Location", DDL_Location.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@TRN_SubmitterWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@TRN_SubmitterName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@ContractEmployees", txt_cntrctemp.Text.ToString());
                cmd.Parameters.AddWithValue("@TRNNumber", txt_sopno.Text.ToUpper().ToString());
                cmd.Parameters.AddWithValue("@TRNTitle", txt_sopdesc.Text.ToUpper().ToString());
                cmd.Parameters.AddWithValue("@TRNTrainer", txt_faculty.Text.ToUpper().ToString());
                cmd.Parameters.AddWithValue("@TRNDuration", txt_duration.Text.ToString());

                cmd.Parameters.AddWithValue("@TRNPhoto", "Pending");

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
                string title = "Notifications :";
                string body = ex.Message.ToString();
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                datasaved = false;
            }

            return datasaved;
        }

        protected void DDL_Worksite_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_SiteInchargeName();

            string Worksite_DBCode = DDL_Worksite.SelectedValue.ToString();

            try
            {
                string query = "select * from tlb_atsworksites where DB_Code=@DB_Code";
                SqlParameter[] pram = {
                                          new SqlParameter("@DB_Code",Worksite_DBCode),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    string dbcode = dt.Rows[0]["DB_Code"].ToString();
                    lbl_worksitecode.Text = dbcode;

                    string DB_WKSDept = dt.Rows[0]["Company_Department"].ToString();
                    string DB_WKSDeptDBCode = dt.Rows[0]["Dept_DBCode"].ToString();

                    Bind_SiteInchargeName();
                    BindDeptLocation(DB_WKSDept, DB_WKSDeptDBCode);

                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                //throw;
            }

            DDL_Approver.Enabled = true;
            DDL_Location.Enabled = true;
        }



        //Location Binder Functions------------Kaushik
        private void BindDeptLocation(string DB_WKSDept, string DB_WKSDeptCode)
        {
            string query = "select CompDept_Location,DB_Code from tlb_workregion_compdept_loc where Dept_DBCode='" + DB_WKSDeptCode + "' or Company_Department='" + DB_WKSDept.ToUpper() + "'";
            BindDepLoc(query);
        }
        private void BindDeptLocation(string DB_WKSDept)
        {
            string query = "select CompDept_Location,DB_Code from tlb_workregion_compdept_loc where Company_Department='" + DB_WKSDept + "' order by Id";
            BindDepLoc(query);
        }
        private void BindDepLoc(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Location.DataSource = Cmd.ExecuteReader();
            DDL_Location.DataTextField = "CompDept_Location";
            DDL_Location.DataValueField = "DB_Code";
            DDL_Location.DataBind();
            DDL_Location.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }



        //---------------------Below goes the code for Panel-3 -------------------------------------------//

        private void AddDefaultFirstRecord()
        {
            //creating dataTable
            DataTable dt = new DataTable();
            DataRow dr;
            dt.TableName = "AgendaDetails";
            dt.Columns.Add(new DataColumn("slno", typeof(string)));
            dt.Columns.Add(new DataColumn("agendadescp", typeof(string)));
            dr = dt.NewRow();
            dt.Rows.Add(new object[] { 1, "No Data" });

            //saving databale into viewstate
            ViewState["AgendaDetails"] = dt;

            //bind Gridview
            BindMyGridview();
        }

        public void BindMyGridview()
        {
            if (ViewState["AgendaDetails"] != null)
            {
                DataTable dt = (DataTable)ViewState["AgendaDetails"];

                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    AgendaGrid.Visible = true;
                    AgendaGrid.DataSource = dt;
                    AgendaGrid.DataBind();
                }
                else
                {
                    AddDefaultFirstRecord();
                    AgendaGrid.Visible = true;
                }
            }
        }

        protected void AgendaGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            AgendaGrid.EditIndex = e.NewEditIndex;
            BindMyGridview();
        }

        protected void AgendaGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            AgendaGrid.EditIndex = -1;
            BindMyGridview();
        }

        protected void AgendaGrid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox TextBoxWithName = (TextBox)AgendaGrid.Rows[e.RowIndex].FindControl("txt_trnagenda");
            string NewName = TextBoxWithName.Text.ToString();
            DataTable dt = (DataTable)ViewState["AgendaDetails"];
            DataRow dr = dt.Rows[e.RowIndex];
            dr["agendadescp"] = NewName;
            dr.AcceptChanges();
            ViewState["AgendaDetails"] = dt;
            AgendaGrid.EditIndex = -1;
            BindMyGridview();
        }

        protected void AgendaGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["AgendaDetails"];
            DataRow dr = dtCurrentTable.Rows[e.RowIndex];
            dtCurrentTable.Rows.Remove(dr);
            AgendaGrid.EditIndex = -1;
            BindMyGridview();
        }

        protected void btn_AddAgenda_Click(object sender, EventArgs e)
        {
            if (ViewState["AgendaDetails"] != null)
            {
                //get datatable from view state
                DataTable dtCurrentTable = (DataTable)ViewState["AgendaDetails"];
                DataRow drCurrentRow = null;

                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["agendadescp"] = txt_trnagenda.Text.Trim().ToString();
                    }

                    if (dtCurrentTable.Rows[0][0].ToString() != "")
                    {
                        dtCurrentTable.Rows[0].Delete();
                        dtCurrentTable.AcceptChanges();
                    }


                    //add created Rows into dataTable
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    //Save Data table into view state after creating each row
                    ViewState["AgendaDetails"] = dtCurrentTable;
                    //Bind Gridview with latest Row
                    BindMyGridview();


                    txt_trnagenda.Text = "";
                }

            }
        }
    }
}