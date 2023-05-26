using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Reflection.Emit;
using System.Linq;
using Label = System.Web.UI.WebControls.Label;
using DataTable = System.Data.DataTable;
using Color = System.Drawing.Color;

namespace WebApplication1.bussiness.production
{
    public partial class create_supplymemo : System.Web.UI.Page
    {
        //SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString);
        public static string SQLQRY_MAX_SPID_NO = "SELECT TOP 1 TRM_REQUEST_NO FROM T_REQUEST_MASTER ORDER BY TRM_ID DESC";

        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
        DataTable dt = new DataTable();
        DataTable FirstDatatable;
        Boolean flag = false;

        DataTable dt_lineitems = new DataTable();
        DataTable dt_selectedrows = new DataTable();

        public static string viewid = string.Empty;
        public static string yr = string.Empty;
        public static string mnt = string.Empty;

        // Default folder


        static readonly string rootFolder = @"C:\atswork.in\wwwroot\erp_images\Permits";

        //static readonly string rootFolder = @"D:\OH4Y Works\OH4Y_2021\Demo\WebApplication1\WebApplication1\erp_images\Permits";

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
                    //ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                    string jobid = Request.QueryString["JOBID"];
                    viewid = Request.QueryString["viewid"];

                    yr = Request.QueryString["y"];
                    mnt = Request.QueryString["m"];
                    Bind_JOBIDDetails(jobid);

                    //Checker();
                }
            }
            else
            {
                if (flag == true)
                {
                    string title = "Notifications 49 :";
                    string body = "Data updated succesfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
            }
        }

        //The below function is to allow user to attach missed or new manpower to the current JOBID
        private void Checker()
        {
            //if (Session["WORKMAN"].ToString() == "A84" || Session["WORKMAN"].ToString() == "K208")
            //{
            //    attachmanpowerrow.Visible = true;
            //}
            //else
            //{
            //    attachmanpowerrow.Visible = false;
            //}
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
                    if (dt.Rows[0]["BillingCode"].ToString() == "MS")
                    {
                        string jobdate = dt.Rows[0]["CreatedDate"].ToString();

                        DateTime dt1 = DateTime.Parse(jobdate);
                        DayOfWeek dow = dt1.DayOfWeek; //enum
                        string str = dow.ToString(); //string
                        txt_jobday.Text = str;

                        txt_jobshift.Text = dt.Rows[0]["JOB_Shift"].ToString();
                        txt_jobdate.Text = DateBinder(jobdate);
                        string abc2 = DateBinder(jobdate) + " [" + str + "]" + " [" + dt.Rows[0]["JOB_Shift"].ToString() + "]";
                        lbl_jobdaydetails.Text = abc2.ToString();

                        txt_jobsupv.Text = dt.Rows[0]["Creator_Name"].ToString();
                        lbl_creatorwrk.Text = dt.Rows[0]["Creator_Workman"].ToString();
                        string wo_number = dt.Rows[0]["WorkOrderNo"].ToString();
                        txt_workorderno.Text = wo_number;
                        txt_jobid.Text = dt.Rows[0]["JOBID"].ToString();
                        string jobidstatus = dt.Rows[0]["JOBID_Status"].ToString();

                        if (jobidstatus == "Active")
                        {
                            txt_jobid.ForeColor = Color.Green;
                        }
                        else
                        {
                            txt_jobid.ForeColor = System.Drawing.Color.Blue;
                        }

                        lbl_jobrgn.Text = dt.Rows[0]["JOB_Region"].ToString();
                        lbl_jobcompay.Text = dt.Rows[0]["JOB_Company"].ToString();
                        txt_worksitename.Text = dt.Rows[0]["JOB_Site"].ToString();
                        lbl_worksitedbcode.Text = dt.Rows[0]["JOB_SiteCode"].ToString();
                        lbl_inchargewrk.Text = dt.Rows[0]["JOB_InchargeWrk"].ToString();
                        txt_inchargename.Text = dt.Rows[0]["JOB_InchargeName"].ToString();
                        txt_jobdept.Text = dt.Rows[0]["JOB_Dept"].ToString();
                        txt_jobloc.Text = dt.Rows[0]["JOB_Location"].ToString();
                        txt_permitno.Text = dt.Rows[0]["JOB_PermitNo"].ToString();
                        lbl_permitdeleteddate.Text = dt.Rows[0]["PermitDeleteDate"].ToString();
                        lbl_permitdeletedby.Text = dt.Rows[0]["PermitDeletedByName"].ToString();


                        string uploadstatus = dt.Rows[0]["FinalUpldStatus"].ToString();
                        string JOB_Status = dt.Rows[0]["JOB_Status"].ToString();

                        if (uploadstatus == "Yes")
                        {
                            //GridView1.Columns[5].Visible = true;
                            //lbl_prmtupldstatus.Text = "Uploaded";
                            txt_permitno.ForeColor = Color.Green;
                        }
                        else
                        {
                            GridView1.Columns[5].Visible = false;
                            //lbl_prmtupldstatus.Text = "Pending";
                            txt_permitno.ForeColor = System.Drawing.Color.Red;
                        }


                        string approvalstatus = dt.Rows[0]["Incharge_Approval"].ToString();
                        string entryexitstatus = dt.Rows[0]["EntryExit"].ToString();
                        if (approvalstatus == "Approved")
                        {
                            //lbl_approvalstatus.Text = "Approved";
                            //lbl_approvalstatus.ForeColor = Color.Green;
                            txt_inchargename.ForeColor = Color.Green;
                            //btn_update.Enabled = false;
                            //btn_update.Text = "Sorry!! NOT Allowed";
                            GridView1.Columns[5].Visible = true;
                            GridView2.Columns[12].Visible = true;
                        }
                        else
                        {
                            attachmanpowerrow.Visible = true;
                            if (entryexitstatus == "Entry")
                            {
                                GridView1.Columns[5].Visible = true;
                                GridView2.Columns[12].Visible = false;
                            }
                            else
                            {
                                //GridView2.Columns[12].Visible = true;
                                GridView1.Columns[5].Visible = true;
                                //btn_update.Enabled = true;
                            }
                            //lbl_approvalstatus.Text = "Pending";
                            //lbl_approvalstatus.ForeColor = Color.Red;
                            txt_inchargename.ForeColor = Color.Red;
                        }

                        if (jobidstatus == "Blocked" && uploadstatus == "Yes" && approvalstatus == "Approved" && entryexitstatus == "Exit" && JOB_Status == "Level1MemoCreated")
                        {
                            btn_crtspm.Text = "Print Memo";
                            btn_crtspm.CssClass = "btn btn-success btn-sm";
                            btn_crtspm.ToolTip = "Click to print Supply Memo";
                        }
                        else if (jobidstatus == "Blocked" && uploadstatus == "Yes" && approvalstatus == "Approved" && entryexitstatus == "Exit" && JOB_Status != "Level1MemoCreated")
                        {
                            btn_crtspm.Text = "Save";
                            btn_crtspm.CssClass = "btn btn-primary btn-sm";
                            btn_crtspm.ToolTip = "Click to CREATE Supply Memo";
                        }
                        else
                        {
                            btn_crtspm.Text = "Save";
                            btn_crtspm.CssClass = "btn btn-primary btn-sm";
                            btn_crtspm.ToolTip = "Click to CREATE Supply Memo";
                        }

                        txt_jobtitle.Text = dt.Rows[0]["JOB_Title"].ToString();
                        //txt_approverrmrks.Text = dt.Rows[0]["JOB_Title"].ToString();
                        txt_approverrmrks.Text = "N/A";

                        string CmdString2 = "select * from tbl_jobspermit where JOBID='" + jobid + "' order by Id desc";
                        BindGrid(CmdString2);

                        string CmdString3 = "select Id, WODB_Code, WOI_DBCode, ItemNO, LineNumber, ServiceNumber, Service_Description, Order_Quantity, Rate, PerUnit_Value from tlb_WO_LineItems_Data where WO_Number='" + wo_number + "' order by Id";
                        BindGrid3(CmdString3);

                        btn_proceednxt.Enabled = false;

                        //string CmdString3 = "select * from tbl_attendance where JOBID='" + jobid + "' order by Id desc";
                        //BindGrid2(CmdString3);
                        //string memogridbinder = "SELECT Id, CreatedDate, JOBID, JOB_Region, JOB_Company, JOB_SiteName, JOB_SiteCode, EmployeeName, EmployeeWrk, EmpCategory, EmpDesignation, Employee_Worksite, Employee_WorksiteCode, WourkHours, Inpunch_Time, Outpunch_Time, WorkedTime, WorkedHours, LunchFactor, Calc_OT, ProvidedOT, AttendanceStatus, AttendanceCode, GatePassNo FROM tbl_attendance where JOBID='" + jobid + "' order by Id desc";
                        BindGrid2();
                    }
                    else
                    {
                        string title = "Notifications 253 :";
                        string body = "Selected JOB is NOT a Manpower Supply JOB.....!!";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }   
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                lbl_msg.ForeColor = System.Drawing.Color.Red;
                lbl_msg.Text = "Error 263 : " + ex.Message.ToString();

                string title = "Notifications 265 :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
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

        private void BindGrid3(string cmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            dt_lineitems.Rows.Clear();
            ad.Fill(dt_lineitems);
            GridView3.DataSource = dt_lineitems;
            GridView3.DataBind();
            dbcl.Conn.Close();
        }

        private void BindGrid2()
        {
            string cmdString = "select a.Id, a.JOBID, a.CreatedDate, a.JOB_Region, a.JOB_Company, a.EmployeeWrk, a.EmployeeName, a.EmpCategory,a.PO_SkillCategory, a.EmpDesignation, a.Employee_Worksite, a.Employee_WorksiteCode, a.Inpunch_Time, a.Outpunch_Time, a.WorkedHours, a.WourkHours, a.LunchFactor, a.ProvidedOT, a.AttendanceStatus, a.AttendanceCode, a.GatePassNo,a.SafetyPassNo, Round(IIF(a.LunchFactor ='Yes',(WorkedHours-1)/8,WorkedHours/8),2) as ShiftCalc from tbl_attendance a, tbl_Employee_Mustertable b  where a.JOBID='" + txt_jobid.Text.ToString() + "' and a.EmployeeWrk=b.WorkmanSL order by a.Id desc";

            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();
            FirstDatatable = null;
            if (dr.Read())
            {
                FirstDatatable = dbcl.GetDataTable(cmdString);
                NewGridrmemo();
            }
            dbcl.Conn.Close();
        }

        private void NewGridrmemo()
        {
            DataTable dt1;
            dt1 = FirstDatatable;
            DataTable Dt1 = new DataTable("Table1");
            DataRow dr = null;
            DataColumn Id = new DataColumn("Id", typeof(Int32));
            Dt1.Columns.Add(Id);
            DataColumn JOBID = new DataColumn("JOBID", typeof(string));
            Dt1.Columns.Add(JOBID);
            DataColumn CreatedDate = new DataColumn("CreatedDate", typeof(DateTime));
            Dt1.Columns.Add(CreatedDate);
            DataColumn JOB_Region = new DataColumn("JOB_Region", typeof(string));
            Dt1.Columns.Add(JOB_Region);
            DataColumn JOB_Company = new DataColumn("JOB_Company", typeof(string));
            Dt1.Columns.Add(JOB_Company);
            DataColumn EmployeeWrk = new DataColumn("EmployeeWrk", typeof(string));
            Dt1.Columns.Add(EmployeeWrk);
            DataColumn EmployeeName = new DataColumn("EmployeeName", typeof(string));
            Dt1.Columns.Add(EmployeeName);
            DataColumn EmpCategory = new DataColumn("EmpCategory", typeof(string));
            Dt1.Columns.Add(EmpCategory);
            DataColumn PO_SkillCategory = new DataColumn("PO_SkillCategory", typeof(string));
            Dt1.Columns.Add(PO_SkillCategory);
            DataColumn PO_EmpDesignation = new DataColumn("PO_EmpDesignation", typeof(string));
            Dt1.Columns.Add(PO_EmpDesignation);
            DataColumn Employee_Worksite = new DataColumn("Employee_Worksite", typeof(string));
            Dt1.Columns.Add(Employee_Worksite);
            DataColumn Employee_WorksiteCode = new DataColumn("Employee_WorksiteCode", typeof(string));
            Dt1.Columns.Add(Employee_WorksiteCode);
            DataColumn Inpunch_Time = new DataColumn("Inpunch_Time", typeof(string));
            Dt1.Columns.Add(Inpunch_Time);
            DataColumn Outpunch_Time = new DataColumn("Outpunch_Time", typeof(string));
            Dt1.Columns.Add(Outpunch_Time);
            DataColumn WorkedHours = new DataColumn("WorkedHours", typeof(decimal));
            Dt1.Columns.Add(WorkedHours);
            DataColumn WourkHours = new DataColumn("WourkHours", typeof(Int32));
            Dt1.Columns.Add(WourkHours);
            DataColumn LunchFactor = new DataColumn("LunchFactor", typeof(string));
            Dt1.Columns.Add(LunchFactor);
            DataColumn ProvidedOT = new DataColumn("ProvidedOT", typeof(decimal));
            Dt1.Columns.Add(ProvidedOT);
            DataColumn AttendanceStatus = new DataColumn("AttendanceStatus", typeof(string));
            Dt1.Columns.Add(AttendanceStatus);
            DataColumn AttendanceCode = new DataColumn("AttendanceCode", typeof(string));
            Dt1.Columns.Add(AttendanceCode);
            DataColumn GatePassNo = new DataColumn("GatePassNo", typeof(string));
            Dt1.Columns.Add(GatePassNo);
            DataColumn SafetyPassNo = new DataColumn("SafetyPassNo", typeof(string));
            Dt1.Columns.Add(SafetyPassNo);
            DataColumn ShiftCalc = new DataColumn("ShiftCalc", typeof(decimal));
            Dt1.Columns.Add(ShiftCalc);
            //DataColumn GatePassExpiry = new DataColumn("GatePassExpiry", typeof(DateTime));
            //Dt1.Columns.Add(GatePassExpiry);

            for (int i = 0; i <= dt1.Rows.Count - 1; i++)
            {
                Int32 _Id = (Int32)FirstDatatable.Rows[i][0];
                string _JOBID = (String)FirstDatatable.Rows[i][1];
                DateTime _CreatedDate = (DateTime)FirstDatatable.Rows[i][2];
                string _JOB_Region = (String)FirstDatatable.Rows[i][3];
                string _JOB_Company = (String)FirstDatatable.Rows[i][4];
                string _EmployeeWrk = (String)FirstDatatable.Rows[i][5];
                string _EmployeeName = (String)FirstDatatable.Rows[i][6];
                string _EmpCategory = (String)FirstDatatable.Rows[i][7];
                string _PO_SkillCategory = (String)FirstDatatable.Rows[i][8];
                string _PO_EmpDesignation = (string)FirstDatatable.Rows[i][9];
                string _Employee_Worksite = (string)FirstDatatable.Rows[i][10];
                string _Employee_WorksiteCode = (string)FirstDatatable.Rows[i][11];
                DateTime _Inpunch_Time = (DateTime)FirstDatatable.Rows[i][12];
                DateTime _Outpunch_Time = (DateTime)FirstDatatable.Rows[i][13];
                decimal _WorkedHours = (decimal)FirstDatatable.Rows[i][14];
                Int32 _WourkHours = (Int32)FirstDatatable.Rows[i][15];
                string _LunchFactor = (String)FirstDatatable.Rows[i][16];
                decimal _ProvidedOT = (decimal)FirstDatatable.Rows[i][17];
                string _AttendanceStatus = (String)FirstDatatable.Rows[i][18];
                string _AttendanceCode = (String)FirstDatatable.Rows[i][19];
                string _GatePassNo = (String)FirstDatatable.Rows[i][20];
                decimal _ShiftCalc = (decimal)FirstDatatable.Rows[i][22];
                string _SafetyPassNo = (String)FirstDatatable.Rows[i][21];
                //DateTime _GatePassExpiry = (DateTime)FirstDatatable.Rows[i][20];

                dr = Dt1.NewRow();
                dr["Id"] = _Id.ToString();
                dr["JOBID"] = _JOBID.ToString();
                dr["CreatedDate"] = _CreatedDate.ToString();
                dr["JOB_Region"] = _JOB_Region.ToString();
                dr["JOB_Company"] = _JOB_Company.ToString();
                dr["EmployeeWrk"] = _EmployeeWrk.ToString();
                dr["EmployeeName"] = _EmployeeName.ToString();
                dr["EmpCategory"] = _EmpCategory.ToString();
                dr["PO_SkillCategory"] = _PO_SkillCategory.ToString();
                dr["PO_EmpDesignation"] = _PO_EmpDesignation.ToString();
                dr["Employee_Worksite"] = _Employee_Worksite.ToString();
                dr["Employee_WorksiteCode"] = _Employee_WorksiteCode.ToString();
                dr["Inpunch_Time"] = _Inpunch_Time.ToString();
                dr["Outpunch_Time"] = _Outpunch_Time.ToString();
                dr["WorkedHours"] = _WorkedHours.ToString();
                dr["WourkHours"] = _WourkHours.ToString();
                dr["LunchFactor"] = _LunchFactor.ToString();
                dr["ProvidedOT"] = _ProvidedOT.ToString();
                dr["AttendanceStatus"] = _AttendanceStatus.ToString();
                dr["AttendanceCode"] = _AttendanceCode.ToString();
                dr["GatePassNo"] = _GatePassNo.ToString();
                //dr["GatePassExpiry"] = _GatePassExpiry.ToString();
                dr["ShiftCalc"] = _ShiftCalc.ToString();
                dr["SafetyPassNo"] = _SafetyPassNo.ToString();
                Dt1.Rows.Add(dr);
            }
            ViewState["Manpower"] = Dt1;
            GridView2.Visible = true;
            GridView2.Visible = true;
            GridView2.Enabled = true;
            GridView2.DataSource = Dt1;
            GridView2.DataBind();

        }

        public void BindMyGridview()
        {
            if (ViewState["Manpower"] != null)
            {
                DataTable dt = (DataTable)ViewState["Manpower"];

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
                        string title = "Notifications 440 :";
                        string body = "NO Physical File Found...!!";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }
                catch (IOException ioExp)
                {
                    string title = "Notifications 447 :";
                    string body = ioExp.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                lbl_msg.ForeColor = System.Drawing.Color.Red;
                lbl_msg.Text = "Error 456: " + ex.Message.ToString();
                //throw;
            }
        }

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["Manpower"];
            DataRow dr = dtCurrentTable.Rows[e.RowIndex];
            dtCurrentTable.Rows.Remove(dr);
            GridView2.EditIndex = -1;
            BindMyGridview();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            System.Web.UI.WebControls.Label ID = (System.Web.UI.WebControls.Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            System.Web.UI.WebControls.Label JOBID = (System.Web.UI.WebControls.Label)GridView1.Rows[e.RowIndex].FindControl("lbl_JOBID");
            string jobid = JOBID.Text.ToString();

            System.Web.UI.WebControls.Label filename = (System.Web.UI.WebControls.Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Name");
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

                    string title = "Notifications 533:";
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

                    string title = "Notifications 553:";
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

                    string title = "Notifications 568:";
                    string body = "Attachment Deleted Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }

                string ddljobid = jobid;
                Bind_JOBIDDetails(ddljobid);
            }
            catch (Exception ex)
            {
                string title = "Notifications 578 :";
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
                string title = "Notifications 607 :";
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
            //if (btn_update.Text == "Update")
            //{
            //    txt_permitno.ReadOnly = false;
            //    txt_jobtitle.ReadOnly = false;
            //    txt_jobshift.ReadOnly = false;

            //    //btn_update.Text = "Save Changes";
            //    //btn_cancel.Visible = true;
            //    //btn_cancel.Enabled = true;

            //    txt_permitno.Focus();
            //}
            //else
            //{
            //    //btn_update.Text = "Update";
            //    //btn_cancel.Visible = false;
            //    //btn_cancel.Enabled = false;

            //    UpdateBasicJOBData(jobid);
            //    Bind_JOBIDDetails(jobid);

            //    txt_permitno.ReadOnly = true;
            //    txt_jobtitle.ReadOnly = true;
            //    txt_jobshift.ReadOnly = true;

            //    //btn_update.Text = "Update";
            //    //btn_cancel.Visible = false;
            //    //btn_cancel.Enabled = false;

            //    string title = "Notifications 687 :";
            //    string body = "Data has been UPDATED !!";
            //    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            //}
        }

        private void UpdateBasicJOBData(string jobid)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_jobs set JOB_PermitNo=@JOB_PermitNo, JOB_Title=@JOB_Title, JOB_Shift=@JOB_Shift  where JOBID=@JOBID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                cmd.Parameters.AddWithValue("@JOB_PermitNo", txt_permitno.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_Title", txt_jobtitle.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_Shift", txt_jobshift.Text.ToString());
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error 714 : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView2.EditIndex = e.NewEditIndex;
            //BindGrid2();
            BindMyGridview();
        }

        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView2.EditIndex = -1;
            string jobid = txt_jobid.Text.ToString();
            //BindGrid2();
            BindMyGridview();
        }

        private void ForceNoEdit()
        {
            //GridView2.EditIndex = -1;
            //string jobid = txt_jobid.Text.ToString();
            //BindMyGridview();
        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal HS_Shift = .0m;
            decimal S_Shift = .0m;
            decimal SS_Shift = .0m;
            decimal US_Shift = .0m;

            if (e.Row.RowType == DataControlRowType.DataRow && GridView2.EditIndex == e.Row.RowIndex)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var DDL_AttendanceStatus = e.Row.FindControl("DDL_AttendanceStatus") as DropDownList;
                    if (DDL_AttendanceStatus != null)
                    {
                        try
                        {
                            var dt1 = new DataTable();
                            string cnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString();
                            using (var con = new SqlConnection(cnnString1))
                            {
                                con.Open();
                                var cmd1 = new SqlCommand("Select DISTINCT Status from tlb_attendancecodes where MemoCreator='Yes'", con);
                                var da1 = new SqlDataAdapter(cmd1);
                                da1.Fill(dt1);
                                con.Close();
                            }

                            DDL_AttendanceStatus.DataSource = dt1;
                            DDL_AttendanceStatus.DataTextField = "Status";
                            DDL_AttendanceStatus.DataValueField = "Status";
                            DDL_AttendanceStatus.DataBind();
                            string AttendanceStatus = DataBinder.Eval(e.Row.DataItem, "AttendanceStatus").ToString();
                            DDL_AttendanceStatus.Items.FindByText(AttendanceStatus).Selected = true;
                        }
                        catch (Exception ex)
                        {
                            string title = "Error :";
                            string body = "No Sttaus Code Mapping found...!! " + ex.Message;
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                            ForceNoEdit();
                            //throw;
                        }
                    }

                    var DDL_AttendanceCode = e.Row.FindControl("DDL_AttendanceCode") as DropDownList;
                    if (DDL_AttendanceCode != null)
                    {
                        try
                        {
                            var dt2 = new DataTable();
                            string cnnString2 = System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString();
                            using (var con = new SqlConnection(cnnString2))
                            {
                                con.Open();
                                var cmd2 = new SqlCommand("Select Status_Name,Status_Code from tlb_attendancecodes where MemoCreator='Yes' order by slno", con);
                                var da2 = new SqlDataAdapter(cmd2);
                                da2.Fill(dt2);
                                con.Close();
                            }

                            DDL_AttendanceCode.DataSource = dt2;
                            DDL_AttendanceCode.DataTextField = "Status_Name";
                            DDL_AttendanceCode.DataValueField = "Status_Code";
                            DDL_AttendanceCode.DataBind();
                            string AttendanceCode = DataBinder.Eval(e.Row.DataItem, "AttendanceCode").ToString();
                            DDL_AttendanceCode.Items.FindByValue(AttendanceCode).Selected = true;
                        }
                        catch (Exception ex)
                        {
                            string title = "Error :";
                            string body = "No Sttaus Code Mapping found...!!";
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                            ForceNoEdit();
                            //throw;
                        }
                    }

                    string EmpCategory = "";
                    string wrkodrno = txt_workorderno.Text.ToString();
                    var DDL_EmpCategory = e.Row.FindControl("DDL_EmpCategory") as DropDownList;
                    if (DDL_EmpCategory != null)
                    {
                        try
                        {
                            var dt1 = new DataTable();
                            string cnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString();

                            using (var con = new SqlConnection(cnnString1))
                            {
                                con.Open();
                                //var cmd1 = new SqlCommand("SELECT Category_Type, Category_DB  FROM tlb_payroll_category where WorkRegion_Code='" + Session["REGION"].ToString() + "'", con);
                                var cmd1 = new SqlCommand("SELECT distinct WO_Category_Type, WO_Category_Code  FROM tlb_WO_SkillCategory where Work_Region_Code='" + Session["REGION"].ToString() + "'  and WO_Number='" + wrkodrno + "'", con);
                                var da1 = new SqlDataAdapter(cmd1);
                                da1.Fill(dt1);
                                con.Close();
                            }

                            DDL_EmpCategory.DataSource = dt1;
                            DDL_EmpCategory.DataTextField = "WO_Category_Type";
                            DDL_EmpCategory.DataValueField = "WO_Category_Code";
                            //DDL_EmpCategory.DataTextField = "Category_Type";
                            //DDL_EmpCategory.DataValueField = "Category_DB";
                            DDL_EmpCategory.DataBind();
                            EmpCategory = DataBinder.Eval(e.Row.DataItem, "PO_SkillCategory").ToString();
                            DDL_EmpCategory.Items.FindByText(EmpCategory).Selected = true;
                        }
                        catch (Exception)
                        {
                            string title = "Error :";
                            string body = "No PO Skill Category found mapped with PO...!";
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                            ForceNoEdit();
                            //throw;
                        }
                    }

                    var DDL_EmpDesignation = e.Row.FindControl("DDL_EmpDesignation") as DropDownList;
                    if (DDL_EmpDesignation != null)
                    {
                        try
                        {
                            var dt4 = new DataTable();
                            string cnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString();
                            using (var con = new SqlConnection(cnnString1))
                            {
                                con.Open();
                                //var cmd1 = new SqlCommand("SELECT Designation_Type, Designation_DB FROM tlb_payroll_designation where WorkRegion_Code='" + Session["REGION"].ToString() + "' and Category_Type='"+ EmpCategory + "' order by Designation_Type", con);
                                var cmd1 = new SqlCommand("SELECT Designation_Type, Designation_DB FROM tlb_WO_SkillCategory where Work_Region_Code='" + Session["REGION"].ToString() + "' and WO_Number='" + wrkodrno + "' and WO_Category_Type='" + EmpCategory + "' order by Designation_Type", con);
                                var da4 = new SqlDataAdapter(cmd1);
                                da4.Fill(dt4);
                                con.Close();
                            }

                            DDL_EmpDesignation.DataSource = dt4;
                            DDL_EmpDesignation.DataTextField = "Designation_Type";
                            DDL_EmpDesignation.DataValueField = "Designation_DB";
                            DDL_EmpDesignation.DataBind();
                            string EmpDesignation = DataBinder.Eval(e.Row.DataItem, "PO_EmpDesignation").ToString();
                            DDL_EmpDesignation.Items.FindByText(EmpDesignation).Selected = true;
                        }
                        catch (Exception ex)
                        {
                            string title = "Error :";
                            string body = "No PO Designation found mapped with PO...!";
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                            ForceNoEdit();
                            //throw;
                        }
                    }
                }
            }

            //The below code is added on 14-01-2023 to evaluate the shift value for individual employees -- Purnima
            for (int i = 0; i <= GridView2.Rows.Count - 1; i++)
            {
                System.Web.UI.WebControls.Label lbl_PO_SkillCategory = ((System.Web.UI.WebControls.Label)GridView2.Rows[i].FindControl("lbl_PO_SkillCategory"));
                System.Web.UI.WebControls.DropDownList DDL_EmpCategory = ((System.Web.UI.WebControls.DropDownList)GridView2.Rows[i].FindControl("DDL_EmpCategory"));

                System.Web.UI.WebControls.Label lbl_Inpunch_Time = ((System.Web.UI.WebControls.Label)GridView2.Rows[i].FindControl("lbl_Inpunch_Time"));
                System.Web.UI.WebControls.Label lbl_Outpunch_Time = ((System.Web.UI.WebControls.Label)GridView2.Rows[i].FindControl("lbl_Outpunch_Time"));
                System.Web.UI.WebControls.Label lbl_ShiftCalc = ((System.Web.UI.WebControls.Label)GridView2.Rows[i].FindControl("lbl_ShiftCalc"));
                System.Web.UI.WebControls.Label lbl_WorkedHours = ((System.Web.UI.WebControls.Label)GridView2.Rows[i].FindControl("lbl_WorkedHours"));

                System.Web.UI.WebControls.Label lbl_LunchFactor = ((System.Web.UI.WebControls.Label)GridView2.Rows[i].FindControl("lbl_LunchFactor"));
                System.Web.UI.WebControls.DropDownList DDL_LunchFactor = ((System.Web.UI.WebControls.DropDownList)GridView2.Rows[i].FindControl("DDL_LunchYesNo"));
                //decimal CalcShift = Convert.ToDecimal(lbl_ShiftCalc.Text.ToString());

                decimal workedhours = Convert.ToDecimal(lbl_WorkedHours.Text.ToString());
                string LunchFactor = "";
                if (lbl_LunchFactor == null)
                {
                    LunchFactor = DDL_LunchFactor.SelectedItem.Text.ToString();
                }
                else
                {
                    LunchFactor = Convert.ToString(lbl_LunchFactor.Text.ToString());
                }
                decimal Revworkedhours = .0m;
                decimal ShiftCalc = .0m;
                decimal FnlShiftCalc = .0m;
                decimal ShiftHours = 8;
                if (workedhours > 0)
                {
                    if (LunchFactor == "Yes")
                    {
                        Revworkedhours = workedhours - 1;
                    }
                    else
                    {
                        Revworkedhours = workedhours;
                    }
                    ShiftCalc = Revworkedhours / ShiftHours;
                    FnlShiftCalc = Math.Round(ShiftCalc, 3);
                }
                lbl_ShiftCalc.Text = FnlShiftCalc.ToString();

                string PO_Category = "";
                if (lbl_PO_SkillCategory == null)
                {
                    PO_Category = DDL_EmpCategory.SelectedItem.Text.ToString();
                }
                else
                {
                    PO_Category = Convert.ToString(lbl_PO_SkillCategory.Text.ToString());
                }

                if (PO_Category == "HIGHLY-SKILLED")
                {
                    HS_Shift = HS_Shift + FnlShiftCalc;
                }
                else if (PO_Category == "SKILLED")
                {
                    S_Shift = S_Shift + FnlShiftCalc;
                }
                else if (PO_Category == "SEMI-SKILLED")
                {
                    SS_Shift = SS_Shift + FnlShiftCalc;
                }
                else
                {
                    US_Shift = US_Shift + FnlShiftCalc;
                }

                lbl_HS_ShiftCount.Text = HS_Shift.ToString();
                lbl_S_ShiftCount.Text = S_Shift.ToString();
                lbl_SS_ShiftCount.Text = SS_Shift.ToString();
                lbl_US_ShiftCount.Text = US_Shift.ToString();
            }
        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //Label ID = (Label)GridView2.Rows[e.RowIndex].FindControl("lbl_Id");
            //string id = ID.Text.ToString();

            //Label JOBID = (Label)GridView2.Rows[e.RowIndex].FindControl("lbl_JOBID");
            //string jobid = JOBID.Text.ToString();

            //Label empwrk = (Label)GridView2.Rows[e.RowIndex].FindControl("lbl_EmployeeWrk");
            //string workmansl = empwrk.Text.ToString();

            //Label wrkhrs = (Label)GridView2.Rows[e.RowIndex].FindControl("lbl_WourkHours");
            //Int32 emp_wrkhours = Convert.ToInt32(wrkhrs.Text.ToString());
            //Int32 emp_wrkmnis = emp_wrkhours * 60;

            //DropDownList DDL_EmpCategory = (DropDownList)GridView2.Rows[e.RowIndex].FindControl("DDL_EmpCategory");
            //string new_DDL_EmpCategory = DDL_EmpCategory.SelectedItem.Text.ToString();

            //DropDownList DDL_EmpDesignation = (DropDownList)GridView2.Rows[e.RowIndex].FindControl("DDL_EmpDesignation");
            //string new_DDL_EmpDesignation = DDL_EmpDesignation.SelectedItem.Text.ToString();

            //TextBox TextBoxWithIntime = (TextBox)GridView2.Rows[e.RowIndex].FindControl("txt_Inpunch_Time");
            //string new_intitme = TextBoxWithIntime.Text.ToString();
            //DateTime timein = DateTime.ParseExact(new_intitme, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
            //string convtimein = timein.ToString("yyyy-MM-dd hh:mm:ss tt");


            //TextBox TextBoxWithOuttime = (TextBox)GridView2.Rows[e.RowIndex].FindControl("txt_Outpunch_Time");
            //string new_outtime = TextBoxWithOuttime.Text.ToString();
            //DateTime timeout = DateTime.ParseExact(new_outtime, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
            //string convtimeout = timeout.ToString("yyyy-MM-dd hh:mm:ss tt");


            //DropDownList DDL_Lunch = (DropDownList)GridView2.Rows[e.RowIndex].FindControl("DDL_LunchYesNo");
            //string lunchyesno = DDL_Lunch.SelectedItem.Text.ToString();

            //TextBox TextBoxWithOt = (TextBox)GridView2.Rows[e.RowIndex].FindControl("txt_ProvidedOT");
            //string new_ot = TextBoxWithOt.Text.ToString();
            //decimal new_pot = Convert.ToDecimal(new_ot);

            //DropDownList AttendanceStatus = (DropDownList)GridView2.Rows[e.RowIndex].FindControl("DDL_AttendanceStatus");
            //string ddl_newattensttaus = AttendanceStatus.SelectedItem.Text.ToString();

            //DropDownList AttendanceCode = (DropDownList)GridView2.Rows[e.RowIndex].FindControl("DDL_AttendanceCode");
            //string ddl_newattencode = AttendanceCode.SelectedValue.ToString();


            //TextBox TextBoxWithGPExpiry = (TextBox)GridView2.Rows[e.RowIndex].FindControl("txt_GatePassExpiry");
            //string new_gpexpiry = TextBoxWithGPExpiry.Text.ToString();
            //DateTime gpexpiry = DateTime.ParseExact(new_gpexpiry, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
            //string convgpexpiry = gpexpiry.ToString("yyyy-MM-dd hh:mm:ss tt");

            //Int32 workdmins = 0;
            //decimal workedhours = .0m;
            //dbcl.FindEmployeeWorkedTime(convtimein, convtimeout, ref workdmins, ref workedhours);
            //decimal emp_calOThrs = .0m;
            //dbcl.CalculateOvertime(emp_wrkmnis, workdmins, lunchyesno, ref emp_calOThrs);

            //UpdateDetails(id, jobid, workmansl, convtimein, convtimeout, lunchyesno, workdmins,
            //    workedhours, emp_calOThrs, new_pot, ddl_newattensttaus, ddl_newattencode);

            //UpdateEmployeeGPExpiry_MemoPage(workmansl, convgpexpiry);


            string EmpCategory = ((DropDownList)GridView2.Rows[e.RowIndex].FindControl("DDL_EmpCategory")).SelectedItem.Text.ToString();
            string EmpDesignation = ((DropDownList)GridView2.Rows[e.RowIndex].FindControl("DDL_EmpDesignation")).SelectedItem.Text.ToString();
            string lunchyesno = ((DropDownList)GridView2.Rows[e.RowIndex].FindControl("DDL_LunchYesNo")).SelectedValue;
            //string GPExpiry = ((TextBox)GridView2.Rows[e.RowIndex].FindControl("txt_GatePassExpiry")).Text;
            DataTable dt = (DataTable)ViewState["Manpower"];
            DataRow dr = dt.Rows[e.RowIndex];
            dr["PO_SkillCategory"] = EmpCategory;
            dr["EmpDesignation"] = EmpDesignation;
            dr["LunchFactor"] = lunchyesno;
            //dr["GatePassExpiry"] = GPExpiry;
            dr.AcceptChanges();
            ViewState["Manpower"] = dt;
            GridView2.EditIndex = -1;
            BindMyGridview();


            //GridView2.EditIndex = -1;
            //BindGrid2();
            flag = true;
            string title = "Notifications 1001 :";
            string body = "Data row has been successfully updated...!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
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
                //cmd.Parameters.AddWithValue("@GatePassExpiry", ddl_newattencode);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                string title = "Notifications 1044 :";
                string body = "Data has been UPDATED !!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications 1050 :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }

            //Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            txt_permitno.ReadOnly = true;
            txt_jobtitle.ReadOnly = true;
            txt_jobshift.ReadOnly = true;

            //btn_update.Text = "Update";
            //btn_cancel.Visible = false;
            //btn_cancel.Enabled = false;
        }

        protected void btn_back_Click(object sender, EventArgs e)
        {
            //object refUrl = ViewState["RefUrl"];
            //if (refUrl != null)
            //    Response.Redirect((string)refUrl);
            Response.Redirect("vw_supplyjobs.aspx");
        }

        protected void btn_attachmanpower_Click(object sender, EventArgs e)
        {
            Response.Redirect("attach_manpower.aspx?JOBID=" + txt_jobid.Text.ToString() + "");
        }

        protected void btn_delete_Click(object sender, EventArgs e)
        {

        }

        private void UpdateEmployeeGPExpiry_MemoPage(string workmansl, string convgpexpiry)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_Employee_Mustertable set /*GatePassNo=@GatePassNo,*/ GatePassExpiry=@GatePassExpiry, GP_ModifierWrk=@GP_ModifierWrk, GP_ModifierName=@GP_ModifierName, GP_ModifiedDate=@GP_ModifiedDate, GP_UpdateApproval=@GP_UpdateApproval where WorkmanSL=@WorkmanSL";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@WorkmanSL", workmansl);
                //cmd.Parameters.AddWithValue("@GatePassNo", nwgpno);
                cmd.Parameters.AddWithValue("@GatePassExpiry", convgpexpiry);
                cmd.Parameters.AddWithValue("@GP_ModifierWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@GP_ModifierName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@GP_ModifiedDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@GP_UpdateApproval", "Pending");
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {

                string title = "Notifications 1111 :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }

            //Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void DDL_EmpCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow currentRow1 = (GridViewRow)((DropDownList)sender).Parent.Parent;
            DropDownList ddl1 = (DropDownList)currentRow1.FindControl("DDL_EmpCategory");
            //string ddl1_val = ddl1.SelectedValue.ToString();
            string ddl1_val = ddl1.SelectedItem.Text.ToString();

            GridViewRow currentRow2 = (GridViewRow)((DropDownList)sender).Parent.Parent;
            DropDownList ddl2 = (DropDownList)currentRow2.FindControl("DDL_EmpDesignation");
            string ddl2_val = ddl2.SelectedValue.ToString();

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string CmdString = "select Designation_Type, Designation_DB from tlb_WO_SkillCategory where Work_Region_Code='" + Session["REGION"].ToString() + "' and WO_Number='" + txt_workorderno.Text.ToString() + "' and WO_Category_Type =  '" + ddl1_val + "' order by Id";
                SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
                Cmd.CommandType = CommandType.Text;
                ddl2.DataSource = Cmd.ExecuteReader();
                ddl2.DataTextField = "Designation_Type";
                ddl2.DataValueField = "Designation_DB";
                ddl2.DataBind();
                ddl2.Items.Insert(0, "Please Select Option");
                dbcl.DisconnectDb();
            }
            catch (Exception ex)
            {
                string title = "Error :";
                string body = "Error 1155 : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView2.Columns[19].Visible = false; //Gatepass number Column
            GridView2.Columns[18].HeaderStyle.Width = 2;

            GridView2.Columns[16].HeaderStyle.Width = 1; // Provide OT Column
            GridView2.Columns[14].Visible = false; //WorkTime column

            GridView2.Columns[13].HeaderStyle.Width = 3; // Out Punch
            GridView2.Columns[12].HeaderStyle.Width = 3; // In Punch Column

            GridView2.Columns[12].HeaderStyle.Width = 2; // In Punch Column

            GridView2.Columns[9].HeaderStyle.Width = 1; // Designation
            GridView2.Columns[8].HeaderStyle.Width = 2; // Skill Grade
        }

        protected void btn_backpage_Click(object sender, EventArgs e)
        {
            if (viewid == "1")
            {
                Response.Redirect("vw_inchsupmem.aspx?y="+yr+"&m="+mnt+"");
            }
            else if (viewid == "2")
            {
                Response.Redirect("vw_supplyjobs.aspx");
            }
        }

        protected void btn_home_Click(object sender, EventArgs e)
        {
            Response.Redirect("homepage.aspx");
        }

        protected void btn_crtspm_Click(object sender, EventArgs e)
        {
            //-----     here goes a function   ---- PURNIMA 14-01-2023
            //-----1 . Save details into the DB table (2)
            //-----2 . Update the Memo Creation Status in JOB table with SMID as input to the JOB table
            //-----3 . Create the PDF and Download option

            if (btn_crtspm.Text=="Save")
            {
                //Function 1 --------- SAVE the MEMO Details into the DB
                CollectDetailFromDT();
                //Response.Redirect("create_supplymemo.aspx?JOBID=" + txt_jobid.Text.ToString() + "");
                string title = "Success :";
                string body = "Memo already Created";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                btn_crtspm.Text = "Print Memo";
            }
            else
            {
                Response.Write("<script>window.open ('rpts/supplymemo.aspx?JOBID=" + txt_jobid.Text.ToString() + "','_blank');</script>");
                //Response.Redirect("rpts/supplymemo.aspx?JOBID=" + txt_jobid.Text.ToString() + "");
                //Function 2 --------- PRINT the MEMO
            }


            //select b.EmployeeWrk, b.EmployeeName, a.PO_SkillCategory, a.PO_EmpDesignation, (CONVERT(varchar, b.Inpunch_Time, 13)) as Inpunch_Time, (CONVERT(varchar, b.Outpunch_Time, 13)) as Outpunch_Time, b.GatePassNo, a.ShiftCalc from tbl_supplymemojobmanpower a, tbl_attendance b where a.Ref_JOBID = b.JOBID and a.RefDBId = b.id
        }
        private void CollectDetailFromDT()
        {
            DataTable dtDraftedCalc = (DataTable)ViewState["Manpower"];

            Int32 distinctCalc = dtDraftedCalc.Rows.Count;
            if (dtDraftedCalc != null && distinctCalc > 1)
            {
                dbcl.Sqlconnection();
                int ExeValue = 0;
                string ExeMsg = "Pending"; //This is to save the return type from the execution query of the first Insert Task
                string StatusChanged = "No";
                if (dbcl.Conn.State == ConnectionState.Closed)
                { dbcl.ConnectDb(); }

                SqlTransaction sqlTrans = dbcl.Conn.BeginTransaction();
                try
                {
                    string SuppluMemoIDNo = GenerateId(ref sqlTrans);

                    //-----------------------------------------1
                    ExeValue = InsertIntoDB1(ref sqlTrans, SuppluMemoIDNo);
                    Int32 i;
                    for (i = 0; i <= dtDraftedCalc.Rows.Count - 1; i++)
                    {
                        string RefDBID = ((Label)GridView2.Rows[i].FindControl("lbl_Id")).Text;
                        string RefJOBID = ((Label)GridView2.Rows[i].FindControl("lbl_JOBID")).Text;
                        string RefJOBdate = ((Label)GridView2.Rows[i].FindControl("lbl_CreatedDate")).Text;
                        string EmpWrk = ((Label)GridView2.Rows[i].FindControl("lbl_EmployeeWrk")).Text;
                        string EmpName = ((Label)GridView2.Rows[i].FindControl("lbl_EmployeeName")).Text;
                        string PO_SC_name = ((Label)GridView2.Rows[i].FindControl("lbl_PO_SkillCategory")).Text;
                        string PO_DG_name = ((Label)GridView2.Rows[i].FindControl("lbl_PO_EmpDesignation")).Text;
                        string shiftcalc = ((Label)GridView2.Rows[i].FindControl("lbl_ShiftCalc")).Text;

                        //-----------------------------------------2
                        ExeMsg = InsertIntoDB2(ref sqlTrans, SuppluMemoIDNo,  RefDBID,  RefJOBID,  RefJOBdate,  EmpWrk,  EmpName,  PO_SC_name,  PO_DG_name,  shiftcalc);
                    }

                    //-----------------------------------------3
                    string CmdString = "UPDATE tbl_jobs set Level1_BillingCode=@Level1_BillingCode, JOB_Status=@JOB_Status where JOBID=@JOBID";
                    StatusChanged = UpdateJOBTableStatus(ref sqlTrans, SuppluMemoIDNo, txt_jobid.Text.ToString(), CmdString);

                    if (ExeMsg.Equals("Done") && ExeValue == -1 && StatusChanged=="Yes")
                    {
                        sqlTrans.Commit();
                        string title = "Success :";
                        string body = "Success 1286 : Data saved & Memo Created";
                        lbl_msg.Text = body;
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                    else
                    {
                        sqlTrans.Rollback();
                        string title = "Error :";
                        string body = "Error 1294 : Unsuccessfull attempt, Rollback Done.";
                        lbl_msg.Text = body;
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }
                catch (Exception ex)
                {
                    sqlTrans.Rollback();
                    string title = "Catch Box :";
                    string body = "Error 1203 : " + ex.Message;
                    lbl_msg.Text = body;
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                finally
                {

                    if (dbcl.Conn.State == ConnectionState.Open)
                        dbcl.Conn.Close();
                }
            }
        }

        protected string UpdateJOBTableStatus(ref SqlTransaction sqlTran, string SuppluMemoIDNo, string RefJOBID, string CmdString)
        {
            string msg = string.Empty;
            using (SqlCommand cmdSPDetails = new SqlCommand(CmdString, dbcl.Conn, sqlTran))
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@JOBID", RefJOBID);
                cmd.Parameters.AddWithValue("@Level1_BillingCode", SuppluMemoIDNo);
                cmd.Parameters.AddWithValue("@JOB_Status", "Level1MemoCreated");
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                msg = "Yes";
            }
            return msg;
        }

        private string GenerateId(ref SqlTransaction sqlTrans)
        {
            DataTable dtMax = new DataTable();
            string SMIDNo = "";

            string today = DateTime.Now.ToString("ddMyy");
            string SQLQRY_MAX_SPID_NO = "SELECT TOP 1 SMJID FROM tbl_supplymemojobsdetails ORDER BY Id DESC";
            SqlCommand cmdMax = new SqlCommand(SQLQRY_MAX_SPID_NO, dbcl.Conn, sqlTrans);
            SqlDataAdapter daMax = new SqlDataAdapter(cmdMax);
            daMax.Fill(dtMax);

            if (dtMax != null && dtMax.Rows.Count > 0 && dtMax.Rows[0][0].ToString().Trim() != "")
            {
                var stringArr = dtMax.Rows[0].ItemArray.Select(x => x.ToString().Split('/')).ToArray();

                string dateVar = stringArr[0][1];
                int sequence = Convert.ToInt32(stringArr[0][2]);


                if (dateVar.Equals(today))
                {
                    sequence += 1;
                    SMIDNo = stringArr[0][0] + "/" + dateVar + "/" + sequence;
                }
                else
                {
                    SMIDNo = stringArr[0][0] + "/" + today + "/1";
                }
            }
            else
            {
                SMIDNo = "ATS-BIL/" + today + "/1";
            }
            return SMIDNo;
        }

        private Int32 InsertIntoDB1(ref SqlTransaction sqlTran, string SuppluMemoIDNo)
        {
            //string msg = "";
            int Count = 0;
            using (SqlCommand cmdSPDetails = new SqlCommand("SP_InsertInto_SMJTable", dbcl.Conn, sqlTran))
            {


                decimal count1 = Convert.ToDecimal(lbl_HS_ShiftCount.Text.ToString());
                decimal count2 = Convert.ToDecimal(lbl_S_ShiftCount.Text.ToString());
                decimal count3 = Convert.ToDecimal(lbl_SS_ShiftCount.Text.ToString());
                decimal count4 = Convert.ToDecimal(lbl_US_ShiftCount.Text.ToString());
                decimal totalshift = count1  + count2 + count3 + count4;

                cmdSPDetails.Parameters.Clear();
                cmdSPDetails.CommandType = CommandType.StoredProcedure;
                cmdSPDetails.Parameters.AddWithValue("@SMJID", SuppluMemoIDNo);
                cmdSPDetails.Parameters.AddWithValue("@TimeStamp", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmdSPDetails.Parameters.AddWithValue("@SMJ_Createdate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmdSPDetails.Parameters.AddWithValue("@CreatorName", Session["USERNAME"].ToString());
                cmdSPDetails.Parameters.AddWithValue("@CreatorWorkmen", Session["WORKMAN"].ToString());
                cmdSPDetails.Parameters.AddWithValue("@CreatorRegion", Session["REGION"]);
                cmdSPDetails.Parameters.AddWithValue("@CreatorCompany", Session["COMPANY_CODE"].ToString());
                cmdSPDetails.Parameters.AddWithValue("@Ref_JOBID", txt_jobid.Text.ToString());
                cmdSPDetails.Parameters.AddWithValue("@Ref_JOBDate", Convert.ToDateTime(txt_jobdate.Text.ToString()));
                cmdSPDetails.Parameters.AddWithValue("@Ref_JOBRegion", lbl_jobrgn.Text.ToString());
                cmdSPDetails.Parameters.AddWithValue("@Ref_JOBCompany", lbl_jobcompay.Text.ToString());
                cmdSPDetails.Parameters.AddWithValue("@Ref_JOBWorksite", txt_worksitename.Text.ToString());
                cmdSPDetails.Parameters.AddWithValue("@Ref_JOBSiteCode", lbl_worksitedbcode.Text.ToString());
                cmdSPDetails.Parameters.AddWithValue("@Ref_JOBSiteIncharge", txt_inchargename.Text.ToString());
                cmdSPDetails.Parameters.AddWithValue("@Ref_JOBInchargeName", lbl_inchargewrk.Text.ToString());
                cmdSPDetails.Parameters.AddWithValue("@Total_HSShiftCount", count1);
                cmdSPDetails.Parameters.AddWithValue("@Total_SShiftCount", count2);
                cmdSPDetails.Parameters.AddWithValue("@Total_SSShiftCount", count3);
                cmdSPDetails.Parameters.AddWithValue("@Total_USShiftCount", count4);
                cmdSPDetails.Parameters.AddWithValue("@Total_ShiftCount", totalshift);
                cmdSPDetails.Parameters.AddWithValue("@DepartmentOfficer", String.Empty);
                //msg = "Done";
                Count = cmdSPDetails.ExecuteNonQuery();

            }
            return Count;
        }

        private string InsertIntoDB2(ref SqlTransaction sqlTran,string SuppluMemoIDNo, string RefDBID, string RefJOBID, string RefJOBdate, string EmpWrk, string EmpName, string PO_SC_name, string PO_DG_name, string shiftcalc)
        {
            string msg = string.Empty;
            using (SqlCommand cmdSPManpower = new SqlCommand("SP_InsertInto_SupplyManpower", dbcl.Conn, sqlTran))
            {
                try
                {
                    cmdSPManpower.Parameters.Clear();
                    cmdSPManpower.CommandType = CommandType.StoredProcedure;
                    cmdSPManpower.Parameters.AddWithValue("@RefDBId", RefDBID);
                    cmdSPManpower.Parameters.AddWithValue("@Ref_JOBID", RefJOBID);
                    cmdSPManpower.Parameters.AddWithValue("@Ref_JOBDate", Convert.ToDateTime(RefJOBdate).ToString("yyyy-MM-dd"));
                    cmdSPManpower.Parameters.AddWithValue("@SMJID", SuppluMemoIDNo);
                    cmdSPManpower.Parameters.AddWithValue("@SMJ_Createdate", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmdSPManpower.Parameters.AddWithValue("@EmployeeName", EmpName);
                    cmdSPManpower.Parameters.AddWithValue("@EmployeeWrk", EmpWrk);
                    cmdSPManpower.Parameters.AddWithValue("@PO_SkillCategory", PO_SC_name);
                    cmdSPManpower.Parameters.AddWithValue("@PO_EmpDesignation", PO_DG_name);
                    cmdSPManpower.Parameters.AddWithValue("@ShiftCalc", Convert.ToDecimal(shiftcalc));
                    cmdSPManpower.ExecuteNonQuery();
                    msg = "Done";
                }
                catch (Exception ex)
                {

                    //throw;
                }
            }
            return msg;
        }

        protected void btn_addlineitems_Click(object sender, EventArgs e)
        {
            dt_selectedrows.Clear();
            dt_selectedrows.Columns.AddRange(new DataColumn[10] { new DataColumn("Id"), new DataColumn("WODB_Code"), new DataColumn("WOI_DBCode"), new DataColumn("ItemNO"), new DataColumn("LineNumber"), new DataColumn("ServiceNumber"), new DataColumn("Service_Description"), new DataColumn("Order_Quantity"), new DataColumn("Rate"), new DataColumn("PerUnit_Value") });
            foreach (GridViewRow row in GridView3.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    System.Web.UI.WebControls.CheckBox chkRow = (row.Cells[10].FindControl("CheckRow") as System.Web.UI.WebControls.CheckBox);
                    if (chkRow.Checked)
                    {
                        string Id = (row.Cells[1].FindControl("lbl_Id") as Label).Text;
                        string WODB_Code = (row.Cells[2].FindControl("lbl_WODB_Code") as Label).Text;
                        string WOI_DBCode = (row.Cells[3].FindControl("lbl_WOI_DBCode") as Label).Text;
                        string ItemNO = (row.Cells[4].FindControl("lbl_ItemNO") as Label).Text;
                        string LineNumber = (row.Cells[5].FindControl("lbl_LineNumber") as Label).Text;
                        string ServiceNumber = (row.Cells[6].FindControl("lbl_ServiceNumber") as Label).Text;
                        string Service_Description = (row.Cells[7].FindControl("lbl_Service_Description") as Label).Text;
                        string Order_Quantity = (row.Cells[8].FindControl("lbl_Order_Quantity") as Label).Text;
                        string Rate = (row.Cells[9].FindControl("lbl_Rate") as Label).Text;
                        string PerUnit_Value = (row.Cells[10].FindControl("lbl_PerUnit_Value") as Label).Text;
                        dt_selectedrows.Rows.Add(Id, WODB_Code, WOI_DBCode,ItemNO, LineNumber, ServiceNumber, Service_Description, Order_Quantity, Rate, PerUnit_Value);
                    }
                }
            }
            GridView4.DataSource = dt_selectedrows;
            GridView4.DataBind();

            btn_proceednxt.Enabled = true;
            btn_reset.Enabled = true; ;
        }

        protected void btn_proceednxt_Click(object sender, EventArgs e)
        {

        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            dt_selectedrows.Rows.Clear();
            GridView4.DataSource = dt_selectedrows;
            GridView4.DataBind();

            string CmdString3 = "select Id, WODB_Code, WOI_DBCode, ItemNO, LineNumber, ServiceNumber, Service_Description, Order_Quantity, Rate, PerUnit_Value from tlb_WO_LineItems_Data where WO_Number='" + txt_workorderno.Text.ToString() + "' order by Id";
            BindGrid3(CmdString3);

            btn_proceednxt.Enabled = false;
        }
    }
}