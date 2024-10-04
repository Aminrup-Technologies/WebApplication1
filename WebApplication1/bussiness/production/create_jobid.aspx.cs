using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace WebApplication1.bussiness.production
{
    public partial class create_jobid : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        DataTable dt = new DataTable();

        //Below are the string to fetch the Selected Work Order No & DB Code
        static string WorkorderNo = "";
        static string WorkorderDBID = "";

        static string region = "";

        //Below are the string to save the Data fetched from the DB  against the selected WOrk Order
        static string DB_WOWorkRegion = "";
        static string DB_WOCompnayCode = "";
        static string DB_WODeptCode = "";
        static string DB_WODeptDBCode = "";
        static string DB_WODeptName = "";
        static string DB_WOType = "";


        //below are the string to save the Selected Work-site Name & DB Code
        static string Worksite_Name = "";
        static string Worksite_DBCode = "";


        //below are the string to save the respective session values of the Login User
        static string USR_WorksiteCode = "";
        static string USR_WorksiteName = "";



        //below are the string to save the DATA fetched from the DB against the selected Worksite
        static string DB_WKSDept = "";
        static string DB_WKSDeptCode = "";
        static string DB_WKSDeptDBCode = "";

        static string DB_DeptLocation = "";
        static string DB_DeptLocationCode = String.Empty;

        static string SiteIncharge_Wrk = String.Empty;
        static string SiteIncharge_Name = String.Empty;

        static string Server_FileName = String.Empty;
        static string Server_FilePath = String.Empty;
        static string FileType = String.Empty;

        static Byte[] bytes = { 0 };

        Boolean FileFlag = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string CmdString5 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where JOBID_Menu='Yes' order by Id";
                Bind_WorkRegion(CmdString5);
                DDL_Region.SelectedValue = Session["REGION"].ToString();

                Workorder_Binder();

                string CmdString = "select BilingType, BillingCode from tlb_JOB_BillingType order by Id";
                Bind_BillingType(CmdString);

                string CmdString4 = "Select Status_Name,Status_Code from tlb_attendancecodes where CreateJOBID='Yes' and Status='Present' order by slno";
                Bind_AttendnaceCode(CmdString4);


                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    //lbl_fileyesno.Text = "No";
                    FileFlag = false;

                    jobid_creation.Visible = true;
                    jobid_creation_buttons.Visible = true;
                }

                lbl_jobdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                DayOfWeek today = DateTime.Today.DayOfWeek;
                lbl_jobday.Text = today.ToString();
                if (today.ToString() == "Sunday")
                {
                    DDL_AttenCode.SelectedValue = "OD";
                    DDL_AttenCode.Enabled = true;
                }
                else
                {
                    DDL_AttenCode.SelectedValue = "P";
                    DDL_AttenCode.Enabled = true;
                }



            }
        }


        private void Bind_WorkRegion(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Region.DataSource = Cmd.ExecuteReader();
            DDL_Region.DataTextField = "Work_Region_Name";
            DDL_Region.DataValueField = "Work_Region_Code";
            DDL_Region.DataBind();
            DDL_Region.Items.Insert(0, "Please Select Option");
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

        private void BindWorkorder(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Workorder.DataSource = Cmd.ExecuteReader();
            DDL_Workorder.DataTextField = "WO_Number";
            DDL_Workorder.DataValueField = "DB_Code";
            DDL_Workorder.DataBind();
            DDL_Workorder.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            Response.Redirect("create_jobid.aspx");
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("homepage.aspx");
        }

        protected void DDL_Workorder_SelectedIndexChanged(object sender, EventArgs e)
        {
            WorkorderNo = DDL_Workorder.SelectedItem.Text.ToString();
            WorkorderDBID = DDL_Workorder.SelectedValue.ToString();

            try
            {
                string query = "select * from tlb_WO_Data where WO_Number=@WO_Number and DB_Code=@DB_Code";
                SqlParameter[] pram = {
                                          new SqlParameter("@WO_Number",WorkorderNo),
                                          new SqlParameter("@DB_Code",WorkorderDBID),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    DB_WOType = dt.Rows[0]["WO_Type"].ToString();
                    lbl_wotype.Text = DB_WOType;

                    DB_WOWorkRegion = dt.Rows[0]["Work_Region_Code"].ToString();
                    DB_WOCompnayCode = dt.Rows[0]["Company_Code"].ToString();
                    DB_WODeptName = dt.Rows[0]["Department_Name"].ToString(); //Actual Department name from WO table
                    DB_WODeptCode = dt.Rows[0]["Department_Code"].ToString();
                    DB_WODeptDBCode = dt.Rows[0]["Dept_DBCode"].ToString();   //actual Department DBID

                    DataChecker();
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                //throw;
            }

            //DDL_Worksite.SelectedIndex = 0;
            //lbl_worksitecode.Text = "";

            //DDL_Location.SelectedIndex = 0;
            //llb_siteinchargewrk.Text = "";
        }

        protected void DataChecker()
        {
            if (Session["REGION"].ToString() == DB_WOWorkRegion) // when user work region and selected work-order region is same
            {
                txt_workregion.Text = Session["REGION"].ToString();  //This will bind the Selected Work order --- Work Region Code

                if (Session["COMPANY_CODE"].ToString() == DB_WOCompnayCode) // when user work company and work-order company code is smae
                {
                    txt_company.Text = Session["COMPANY_CODE"].ToString();   // This will bind the Selected Work order ---- Company Code
                    if (DB_WOType != "ARC")
                    {
                        AutoBinder();

                        txt_permitno.Text = "N/A";
                        txt_permitno.ReadOnly = true;
                    }
                    else
                    {
                        WorksiteBinder();
                        txt_permitno.Text = "";
                        txt_permitno.ReadOnly = false;
                    }
                }
                else // When selected WO-Company NOT EQUAL to Login-ed User Company
                {
                    txt_company.Text = DB_WOCompnayCode;
                }
            }
            else
            {
                WorksiteBinder();
                txt_workregion.Text = DB_WOWorkRegion;
            }
        }

        protected void WorksiteBinder()
        {
            string CmdString2 = "select Worksite_Name, DB_Code from tlb_atsworksites where WorkRegion_Code='" + region + "' order by Id";
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

        protected void DDL_Worksite_SelectedIndexChanged(object sender, EventArgs e)
        {
            Worksite_Name = DDL_Worksite.SelectedItem.Text.ToString();
            Worksite_DBCode = DDL_Worksite.SelectedValue.ToString();

            USR_WorksiteName = Session["U_SITE"].ToString();
            USR_WorksiteCode = Session["U_SITECODE"].ToString();

            if (Worksite_DBCode == USR_WorksiteCode || Worksite_Name == USR_WorksiteName)
            {
                try
                {
                    string query = "select * from tlb_atsworksites where DB_Code=@DB_Code";
                    SqlParameter[] pram = {
                                          new SqlParameter("@DB_Code",Worksite_DBCode),
                                      };
                    dt = dbcl.SPreturn_dt(query, pram);
                    if (dt.Rows.Count > 0)
                    {
                        string worksitecode = dt.Rows[0]["Worksite_Code"].ToString();
                        lbl_worksitecode.Text = worksitecode;

                        DB_WKSDept = dt.Rows[0]["Company_Department"].ToString();
                        DB_WKSDeptCode = dt.Rows[0]["CompDept_Code"].ToString();
                        DB_WKSDeptDBCode = dt.Rows[0]["Dept_DBCode"].ToString();

                        DataChecker2();
                        Bind_SiteInchargeName();

                        if (DB_WOType == "ARC")
                        {
                            BindDeptLocation(DB_WKSDept, DB_WKSDeptDBCode);
                        }
                        else
                        {
                            Location_AutoBinder();
                        }

                    }
                }
                catch (Exception ex)
                {
                    string title = "Notifications :";
                    string body = ex.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    //throw;
                }
            }
            else
            {
                //Bind Department Name against the Selected Worksite
                try
                {
                    string query = "select Worksite_Code,Company_Department,CompDept_Code,Dept_DBCode from tlb_atsworksites where DB_Code=@DB_Code";
                    SqlParameter[] pram = {
                                          new SqlParameter("@DB_Code",Worksite_DBCode),
                                      };
                    dt = dbcl.SPreturn_dt(query, pram);
                    if (dt.Rows.Count > 0)
                    {
                        lbl_worksitecode.Text = dt.Rows[0]["Worksite_Code"].ToString();
                        DB_WKSDept = dt.Rows[0]["Company_Department"].ToString();
                        DB_WKSDeptCode = dt.Rows[0]["CompDept_Code"].ToString();
                        DB_WKSDeptDBCode = dt.Rows[0]["Dept_DBCode"].ToString();

                        txt_dept.Text = DB_WKSDept;

                        Bind_SiteInchargeName();
                        if (DB_WOType == "ARC")
                        {
                            BindDeptLocation(DB_WKSDept, DB_WKSDeptDBCode);
                        }
                        else
                        {
                            Location_AutoBinder();
                        }

                    }
                }
                catch (Exception ex)
                {
                    string title = "Notifications :";
                    string body = ex.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    //throw;
                }
            }

        }


        protected void Bind_SiteInchargeName()
        {
            string CmdString = "select Employee_Name, Employee_Workman from tlb_atsworksiteIncharges where DB_Code='" + Worksite_DBCode + "' and Status='Active' order by Id";
            Bind_Approver(CmdString);


            //try
            //{
            //    string query = "select * from tlb_atsworksiteIncharges where Region_Code=@Region_Code";
            //    SqlParameter[] pram = {
            //                              new SqlParameter("@Region_Code",DDL_Region.SelectedValue.ToString()),
            //                          };
            //    dt = dbcl.SPreturn_dt(query, pram);
            //    if (dt.Rows.Count > 0)
            //    {
            //        SiteIncharge_Wrk = dt.Rows[0]["Employee_Workman"].ToString();
            //        SiteIncharge_Name = dt.Rows[0]["Employee_Name"].ToString();

            //        txt_siteincharge.Text = SiteIncharge_Name;
            //        llb_siteinchargewrk.Text = SiteIncharge_Wrk;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    string title = "Notifications :";
            //    string body = ex.Message;
            //    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            //    //throw;
            //}
        }


        //-----------------------added on 10-07-2021 --------------------------------//

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

        protected void DataChecker2()
        {
            if (DB_WKSDeptDBCode == DB_WODeptDBCode) // when Worksite department & Workorder departments are same
            {
                //Bind Department name
                txt_dept.Text = DB_WKSDept;
            }
            else
            {
                txt_dept.Text = DB_WKSDept;
            }
        }

        private void BindDeptLocation(string DB_WKSDept, string DB_WKSDeptCode)
        {
            string query = "select CompDept_Location,DB_Code from tlb_workregion_compdept_loc where Dept_DBCode='" + DB_WKSDeptCode + "' or Company_Department='" + DB_WKSDept + "'";
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

        protected void btn_submit_Click(object sender, EventArgs e)
        {

            if (Insert_JOBData() != 0)
            {
                jobid_created.Visible = true;
                jobid_created_buttons.Visible = true;

                jobid_creation.Visible = false;
                jobid_creation_buttons.Visible = false;
                //Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {

            }
        }

        private string Find_DBCode()
        {
            string aa = null;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string kk = null;
            string cmdString1 = "select Id,JOBID from tbl_jobs where Id=(select max(Id)from tbl_jobs)";
            SqlCommand com1 = new SqlCommand(cmdString1, dbcl.Conn);
            SqlDataReader DR1 = com1.ExecuteReader();
            if (DR1.Read())
            {
                aa = DR1.GetValue(1).ToString();
                string bb = aa.Substring(5);
                int k = Convert.ToInt32(bb);
                k = k + 1;
                string q = Convert.ToString(k);
                kk = "JOB00" + q;
            }
            else
            {
                kk = "JOB001";
            }
            dbcl.DisconnectDb();
            return kk;
        }

        private Int32 Insert_JOBData()
        {
            //Code to Generate Unique Employee ID Goes here
            string JOBID = Find_DBCode();


            //Code to Insert values into the DB goes here
            int flag = 0;
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand("SP_InsertInto_JOBSTable", dbcl.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CreatedDate", lbl_jobdate.Text.ToString());
                cmd.Parameters.AddWithValue("@Creator_Name", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@Creator_Workman", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@Creator_Region", Session["REGION"].ToString());
                cmd.Parameters.AddWithValue("@Creator_Company", Session["COMPANY_CODE"].ToString());
                cmd.Parameters.AddWithValue("@Creator_Site", Session["U_SITE"].ToString());
                cmd.Parameters.AddWithValue("@Creator_SiteCode", Session["U_SITECODE"].ToString());
                cmd.Parameters.AddWithValue("@WorkOrderNo", DDL_Workorder.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Workorder_Type", lbl_wotype.Text.ToString());
                cmd.Parameters.AddWithValue("@JOBID", JOBID);
                cmd.Parameters.AddWithValue("@JOBID_Status", "Active");
                cmd.Parameters.AddWithValue("@JOB_Region", DDL_Region.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@JOB_Company", txt_company.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_Site", DDL_Worksite.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_SiteCode", DDL_Worksite.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@JOB_InchargeWrk", DDL_Approver.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@JOB_InchargeName", DDL_Approver.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_Dept", txt_dept.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_Location", DDL_Location.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_Shift", txt_jobshift.Text.ToUpper().ToString());
                cmd.Parameters.AddWithValue("@JOB_Title", txt_jobtitle.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_PermitNo", txt_permitno.Text.ToString());


                if (DB_WOType == "ARC")
                {
                    if (DDL_Region.SelectedValue.ToString() == "AGL" || DDL_Region.SelectedValue.ToString() == "KPO" || DDL_Region.SelectedValue.ToString() == "NINL" || DDL_Region.SelectedValue.ToString() == "JSR")
                    {
                        cmd.Parameters.AddWithValue("@JOB_Status", "Created");
                        cmd.Parameters.AddWithValue("@FinalUpldStatus", "No");
                        cmd.Parameters.AddWithValue("@PermitUpload", "No");
                        cmd.Parameters.AddWithValue("@FileCount", "0");
                        cmd.Parameters.AddWithValue("@MasterStatusCode", "1");  // JOBID Created, Can Proceed to Permit Upload Page
                        cmd.Parameters.AddWithValue("@CSM_Documents", "Yes");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@JOB_Status", "Created");
                        cmd.Parameters.AddWithValue("@FinalUpldStatus", "No");
                        cmd.Parameters.AddWithValue("@PermitUpload", "No");
                        cmd.Parameters.AddWithValue("@FileCount", "0");
                        cmd.Parameters.AddWithValue("@MasterStatusCode", "1");  // JOBID Created, Can Proceed to Permit Upload Page
                        cmd.Parameters.AddWithValue("@CSM_Documents", "No");    // Disabled for NINL and JSR
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@JOB_Status", "Permit Uploaded");
                    cmd.Parameters.AddWithValue("@FinalUpldStatus", "Yes");
                    cmd.Parameters.AddWithValue("@PermitUpload", "N/A");
                    cmd.Parameters.AddWithValue("@FileCount", "0");
                    cmd.Parameters.AddWithValue("@MasterStatusCode", "3");  // JOBID created, NO Upload Required, Can Proceed to Entry Page
                    cmd.Parameters.AddWithValue("@CSM_Documents", "No");
                }
                cmd.Parameters.AddWithValue("@Incharge_Approval", "Pending");
                cmd.Parameters.AddWithValue("@EntryExit", "Created");
                cmd.Parameters.AddWithValue("@BillingType", DDL_BillingType.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@BillingCode", DDL_BillingType.SelectedValue);
                cmd.Parameters.AddWithValue("@AttendanceCode", DDL_AttenCode.SelectedValue);
                flag = cmd.ExecuteNonQuery();

                dbcl.DisconnectDb();
            }
            catch (Exception ex)
            {
                lbl_msg.Visible = true;
                lbl_msg.Text = ex.Message;
                lbl_msg.ForeColor = System.Drawing.Color.IndianRed;
                dbcl.DisconnectDb();
            }

            if (flag != 0)
            {
                lbl_msg.Visible = true;
                lbl_msg.Text = "Record Inserted Successfully..!";
                lbl_msg.ForeColor = System.Drawing.Color.DarkGreen;
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

        protected void btn_inpunch_Click(object sender, EventArgs e)
        {
            Response.Redirect("job_inpunch.aspx");
        }

        protected void btn_upload_Click(object sender, EventArgs e)
        {
            Response.Redirect("job_permitupload.aspx");
        }



        private void AutoBinder()
        {

            string CmdString2 = "select Worksite_Name, DB_Code from tlb_atsworksites where WorkRegion_Code='" + DDL_Region.SelectedValue.ToString() + "'";
            BindWorkSites(CmdString2);


        }

        private void Location_AutoBinder()
        {
            string query = "select CompDept_Location,DB_Code from tlb_workregion_compdept_loc where Work_Region_Code='" + DDL_Region.SelectedValue.ToString() + "' and Dept_DBCode='" + DB_WKSDeptDBCode + "'";
            BindDepLoc(query);

            string currentdt = DateTime.Now.Date.ToString("dd-MM-yyyy");
            txt_jobtitle.Text = DB_WOType + " attendance for (" + currentdt + ")";
        }

        protected void DDL_Region_SelectedIndexChanged(object sender, EventArgs e)
        {
            Workorder_Binder();
        }


        private void Workorder_Binder()
        {
            region = DDL_Region.SelectedValue.ToString();

            try
            {
                if (Session["USERTYPE"].ToString() == "Office Staff")
                {
                    string CmdString2 = "select WO_Number, DB_Code from tlb_WO_Data where Work_Region_Code='" + region + "' and WO_Status='Active' and JOBID_Menu='Yes' order by WO_Type";
                    BindWorkorder(CmdString2);
                }
                else if (Session["USERTYPE"].ToString() == "Site Staff")
                {
                    string CmdString2 = "select WO_Number, DB_Code from tlb_WO_Data where Work_Region_Code='" + region + "' and WO_Type='ARC' and WO_Status='Active' and JOBID_Menu='Yes' order by Id";
                    BindWorkorder(CmdString2);
                }
                else
                {
                    string CmdString2 = "select WO_Number, DB_Code from tlb_WO_Data where Work_Region_Code='" + region + "' and WO_Status='Active' and JOBID_Menu='Yes' order by WO_Type";
                    BindWorkorder(CmdString2);
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                //throw;
            }
        }

        protected void btn_dateswap_Click(object sender, EventArgs e)
        {
            if (btn_dateswap.Text == "Today")
            {
                DateTime dateTime = DateTime.Now;
                string today = dateTime.DayOfWeek.ToString();
                string yesterday = dateTime.AddDays(-1).DayOfWeek.ToString(); //Fetch day i.e. Mon, Tues
                string result = dateTime.AddDays(-1).ToString("dd-MMM-yyyy");

                lbl_jobday.Text = yesterday.ToString();
                lbl_jobdate.Text = result.ToString();
                btn_dateswap.Text = "Yesterday";
                if (yesterday.ToString() == "Sunday")
                {
                    DDL_AttenCode.SelectedValue = "OD";
                    DDL_AttenCode.Enabled = true;
                }
                else
                {
                    DDL_AttenCode.SelectedValue = "P";
                    DDL_AttenCode.Enabled = true;
                }
            }
            else
            {
                string result = DateTime.Now.ToString("dd-MMM-yyyy");
                lbl_jobdate.Text = result;
                btn_dateswap.Text = "Today";
                DayOfWeek today = DateTime.Today.DayOfWeek;
                lbl_jobday.Text = today.ToString();
                if (today.ToString() == "Sunday")
                {
                    DDL_AttenCode.SelectedValue = "OD";
                    DDL_AttenCode.Enabled = true;
                }
                else
                {
                    DDL_AttenCode.SelectedValue = "P";
                    DDL_AttenCode.Enabled = true;
                }
            }
        }

    }
}