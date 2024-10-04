using System;
using System.Data.SqlClient;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq;
using DocumentFormat.OpenXml.Bibliography;
using System.Drawing;

namespace WebApplication1.bussiness.production
{
    public partial class generate_atdncsheet : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        string str = string.Empty;
        Int32 dayss = 0;
        Int32 ots = 0;
        DataTable dt_emps = new DataTable();

        public static string date1 = string.Empty;
        public static string date2 = string.Empty;
        DataTable dt_present = new DataTable();
        DataTable dt_ot = new DataTable();
        DataTable dt_presentot = new DataTable();

        public static string state = string.Empty;
        public static string region = string.Empty;
        public static string comp = string.Empty;


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
                    if (Session["Changer"] != null)
                    {
                        string[] retrievedArray = (string[])Session["Changer"];
                        region = retrievedArray[1].ToString();
                        comp = retrievedArray[2].ToString();
                        state = retrievedArray[0].ToString();

                        Session["Changer"] = null;
                    }
                    else
                    {
                        region = Session["REGION"].ToString();
                        comp = Session["COMPANY_CODE"].ToString();
                        state = Session["STATE"].ToString();
                    }

                    string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = 'IN' and State_Code ='"+ state + "' order by Id ";
                    BindRegions(CmdString1);

                    DDL_Region.SelectedValue = region;

                    dbcl.CalDateCombo1(DDL_Day, DDL_Month, DDL_Year);
                    dbcl.CalDateCombo1(DDL_D2, DDL_M2, DDL_Y2);

                    if (Session["REGION"].ToString() != "GBL")
                    {
                        CheckforUser();
                    }
                }
            }
        }

        private void CheckforUser()
        {
            DDL_Region.SelectedValue = Session["REGION"].ToString();
            DDL_Region.Enabled = false;
            string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='"+ state + "' and Work_Region_Code = '" + region + "' order by Id ";
            BindCompany(CmdString3);
        }

        private void BindRegions(string CmdString)
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

        protected void DDL_Region_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='OD' and Work_Region_Code = '" + DDL_Region.SelectedValue.ToString() + "' order by Id ";
            BindCompany(CmdString3);
        }

        private void BindCompany(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Company.DataSource = Cmd.ExecuteReader();
            DDL_Company.DataTextField = "Company_Name";
            DDL_Company.DataValueField = "Company_Code";
            DDL_Company.DataBind();
            DDL_Company.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            //string strtday = "01";
            string strtday = DDL_Day.SelectedItem.Text.ToString();
            Int32 minday = Convert.ToInt32(strtday);

            //string endday = "30";
            string endday = DDL_D2.SelectedItem.Text.ToString();
            Int32 maxday = Convert.ToInt32(endday);

            string current_year = DDL_Year.SelectedItem.Text.ToString();
            string current_month1 = DDL_Month.SelectedItem.Text.ToString();
            string region = DDL_Region.SelectedValue.ToString();

            //added on 25-feb-2023 for directly sending the start and end date to the SQL query
            date1 = current_year + "-" + current_month1 + "-" + strtday;
            date2 = current_year + "-" + current_month1 + "-" + endday;

            string status = DDL_EmpWorkStatus.SelectedItem.Text.ToString();
            //Session["WRKRGN"] = region;

            if (DDL_ReportType.SelectedIndex == 1)
            {
                //Only Present Sheet
                Bind_PresentRpt(minday, maxday, current_month1, current_year, region);
            }
            else if (DDL_ReportType.SelectedIndex == 2)
            {
                //Only OT Sheets
                Bind_OverTimeRpt(minday, maxday, current_month1, current_year, region);
            }
            else if (DDL_ReportType.SelectedIndex == 3)
            {
                //Combined Sheet - Single Line
                BindAttendanceHead1(minday, maxday, current_month1, current_year, region);
            }
            else if (DDL_ReportType.SelectedIndex == 4)
            {
                //Combined Sheets : Preset & OT - Double Line
                //Response.Write("<script>window.open ('/bussiness/production/rpts/rpt_CombinedF16.aspx?Year=" + current_year + "&Month=" + current_month1 + "&Region=" + region + "&minday=" + minday + "&maxday=" + maxday + "&status=" + status + "','_blank');</script>");

                Bind_PresentOT_Rpt(minday, maxday, current_month1, current_year, region);
            }

            btnExport.Enabled = true;
        }

        private static string CalculateReportTime(DateTime DT1, DateTime DT2)
        {
            int Hours = DT2.Subtract(DT1).Hours;
            int Minutes = DT2.Subtract(DT1).Minutes;
            int Seconds = DT2.Subtract(DT1).Seconds;
            return String.Format("{0} Hour(s) {1} Minute(s) {2} Second(s)", Hours, Minutes, Seconds);
        }

        private void BindAttendanceHead1(Int32 minday, Int32 maxday, string month_no, string year, string region)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='background-color:#00a8f3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center' colspan='35'> " + month_no.ToString() + " " + year.ToString() + "</span></td></tr></table>";
            BindHeading1(minday, maxday, month_no, year, region);
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='font:normal 16px/16px Century Gothic; padding:5px 20px 5px 20px;' align='cen ter'></td></tr></table>";
            lblTotalData.Text = str;
        }
        //Function for binding Absent / Present Row
        private void BindHeading1(Int32 minday, Int32 maxday, string month_no, string year, string region)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>S. NO</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>WORK SL</td>";
            str = str + "<td width='18%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EMPLOYEE NAME</td>";
            str = str + "<td width='18%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EMPLOYEE DESG</td>";
            for (int i = minday; i <= maxday; i++)
            {
                str = str + "<td width='2%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + i + "</td>";
            }
            str = str + "<td width='10%' style='background-color:#92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>TOTAL PRESENTS</td></tr></table>";

            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            int Sl = 1;
            string findempquery = "";
            if (DDL_EmpWorkStatus.SelectedIndex == 1)
            {
                findempquery = "select WorkmanSL, FullName, SkillDesignation from tbl_Employee_Mustertable where WorkRegion='" + region + "' and F16_YesNo='Yes' order by Id";
            }
            else
            {
                findempquery = "select WorkmanSL, FullName, SkillDesignation from tbl_Employee_Mustertable where WorkRegion='" + region + "' and WorkStatus='" + DDL_EmpWorkStatus.SelectedItem.Text.ToString() + "' and F16_YesNo='Yes' order by Id";
            }

            //string findempquery = "select WorkmanSL, FullName from tbl_Employee_Mustertable where WorkRegion='" + region + "' and WorkStatus='"+DDL_EmpWorkStatus.SelectedItem.Text.ToString()+"' order by Id";
            //string findempquery = "select WorkmanSL, FullName from tbl_Employee_Mustertable where WorkRegion='" + region + "' and WorkStatus='Active' order by Id";

            SqlCommand cmd = new SqlCommand(findempquery, dbcl.Conn);
            using (SqlDataReader re = cmd.ExecuteReader())
            {
                while (re.Read())
                {
                    Int32 daycount = 0;
                    Int32 ttlot = 0;
                    string wrkman = re["WorkmanSL"].ToString();
                    string emp_name = re["FullName"].ToString();
                    string emp_desg = re["SkillDesignation"].ToString();
                    str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:white; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + Sl + "</td>";
                    str = str + "<td width='5%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + wrkman + "</td>";
                    str = str + "<td width='18%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + emp_name + "</td>";
                    Int32 day = minday;
                    for (int j = minday; j <= maxday; j++)
                    {
                        string days = "";
                        if (day <= 9)
                        {
                            days = "0" + day.ToString();
                        }
                        else
                        {
                            days = day.ToString();
                        }
                        FindAttendance1(days, month_no, year, wrkman, ref daycount, ref ttlot, region);
                        day = day + 1;
                    }
                    str = str + "<td width='10%' style='background-color:white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + daycount + "|" + ttlot + "</td></tr></table>";
                    //BindHeadingRow(minday, maxday, month_no, year, region, wrkman, emp_name);
                    Sl = Sl + 1;
                    dayss = 0;
                }
            }
        }
        private void FindAttendance1(string day, string month, string year, string wrk, ref Int32 dayss, ref Int32 ttlot, string region)
        {
            string date = year + "-" + month + "-" + day;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdstring1 = "select max(AttendanceStatus) as AttendanceStatus, AttendanceCode, COALESCE(SUM(ProvidedOT),0) as OT from tbl_attendance where CreatedDate = '" + date + "' and EmployeeWrk='" + wrk + "' and SiteIncharge_Approval='Approved' group by AttendanceCode";
            SqlCommand cmd1 = new SqlCommand(cmdstring1, dbcl.Conn);
            SqlDataReader re1 = cmd1.ExecuteReader();

            if (re1.HasRows)
            {
                while (re1.Read())
                {
                    string present = re1["AttendanceStatus"].ToString();
                    string status = re1["AttendanceCode"].ToString();
                    string dbot = re1["OT"].ToString();
                    Int32 OT = Convert.ToInt32(dbot);
                    if (OT > 0)
                    {
                        ttlot = ttlot + OT;
                    }

                    if (present == "Present")
                    {
                        switch (status)
                        {
                            case "P":
                                dayss = dayss + 1;
                                str = str + "<td width='2%' style='background-color: #91ee3a; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "|" + dbot + "</td>";
                                break;

                            case "NH":
                                dayss = dayss + 1;
                                str = str + "<td width='2%' style='background-color: #00cbf3; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "|" + dbot + "</td>";
                                break;

                            case "FL":
                                dayss = dayss + 1;
                                str = str + "<td width='2%' style='background-color: #00cbf3; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "|" + dbot + "</td>";
                                break;

                            case "OD":
                                str = str + "<td width='2%' style='background-color: #c4ff0e; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "|" + dbot + "</td>";
                                break;

                            default:
                                str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A|0</td>";
                                break;
                        }
                    }
                    else
                    {
                        str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A|0</td>";
                    }
                }
            }
            else
            {
                str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A|0</td>";
            }
            dbcl.Conn.Close();
        }
        //Function for Binding OT Row
        private void BindHeadingRow1(Int32 minday, Int32 maxday, string month_no, string year, string region, string wrkman, string emp_name)
        {
            int Sl = 1;
            Int32 daycount = 0;
            Int32 ots = 0;
            Int32 ttlot = 0;
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:white; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'></td>";
            str = str + "<td width='5%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + wrkman + "</td>";
            str = str + "<td width='18%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + emp_name + "</td>";
            Int32 day = minday;
            for (int j = minday; j <= maxday; j++)
            {
                string days = "";
                if (day <= 9)
                {
                    days = "0" + day.ToString();
                }
                else
                {
                    days = day.ToString();
                }
                FindAttendanceOT1(days, month_no, year, wrkman, ref daycount, ref ots, region);
                day = day + 1;
                ttlot = ttlot + ots;
            }
            str = str + "<td width='10%' style='background-color:white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + ttlot + "</td></tr></table>";
            Sl = Sl + 1;
            dayss = 0;
            ttlot = 0;
        }
        private void FindAttendanceOT1(string day, string month, string year, string wrk, ref Int32 dayss, ref Int32 ots, string region)
        {
            string date = year + "-" + month + "-" + day;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdstring1 = "select COALESCE(SUM(ProvidedOT),0) as OT from tbl_attendance where CreatedDate = '" + date + "' and EmployeeWrk='" + wrk + "'and SiteIncharge_Approval='Approved'";
            SqlCommand cmd1 = new SqlCommand(cmdstring1, dbcl.Conn);
            SqlDataReader re1 = cmd1.ExecuteReader();

            if (re1.HasRows)
            {
                while (re1.Read())
                {
                    string dbot = re1["OT"].ToString();
                    Int32 OT = Convert.ToInt32(dbot);
                    if (OT > 0)
                    {
                        dayss = dayss + 1;
                        ots = OT;
                        str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + OT + "</td>";
                    }
                    else
                    {
                        ots = 0;
                        str = str + "<td width='2%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'></td>";
                    }
                }
            }
            else
            {
                str = str + "<td width='2%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'></td>";
            }
            dbcl.Conn.Close();
        }



        //---------------------------------------------- Only Present --------------------------------------------------------//

        private void Bind_PresentRpt(Int32 minday, Int32 maxday, string month_no, string year, string region)
        {
            DateTime stamp1 = DateTime.Now;
            Label1.Text = stamp1.ToString();

            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='background-color:#00a8f3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center' colspan='35'> " + month_no.ToString() + " " + year.ToString() + "</span></td></tr></table>";
            Bind_PresentHdr(minday, maxday, month_no, year, region);
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='font:normal 16px/16px Century Gothic; padding:5px 20px 5px 20px;' align='cen ter'></td></tr></table>";
            lblTotalData.Text = str;
            DateTime stamp2 = DateTime.Now;
            Label2.Text = stamp2.ToString();
            Label3.Text = CalculateReportTime(stamp1, stamp2);
        }

        private void Bind_PresentHdr(Int32 minday, Int32 maxday, string month_no, string year, string region)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>S. NO</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>WORK SL</td>";
            str = str + "<td width='9%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EMPLOYEE NAME</td>";
            str = str + "<td width='9%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EMPLOYEE DESG</td>";
            for (int i = minday; i <= maxday; i++)
            {
                str = str + "<td width='2%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + i + "</td>";
            }
            str = str + "<td width='10%' style='background-color:#92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>TOTAL PRESENTS</td></tr></table>";

            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            int Sl = 1;
            string findempquery = "";
            if (DDL_EmpWorkStatus.SelectedIndex == 1)
            {
                findempquery = "select WorkmanSL, FullName,SkillDesignation  from tbl_Employee_Mustertable where WorkRegion='" + region + "' and F16_YesNo='Yes' order by Id";
            }
            else
            {
                findempquery = "select WorkmanSL, FullName,SkillDesignation  from tbl_Employee_Mustertable where WorkRegion='" + region + "' and WorkStatus='" + DDL_EmpWorkStatus.SelectedItem.Text.ToString() + "' and F16_YesNo='Yes' order by Id";
            }

            //findempquery = "select top(20) WorkmanSL, FullName from tbl_Employee_Mustertable where WorkRegion='" + region + "' and WorkStatus='Active' order by Id";
            SqlCommand cmd = new SqlCommand(findempquery, dbcl.Conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt_emps);
            dbcl.Sqlconnection(); dbcl.ConnectDb();

            if ((dt_emps != null) && (dt_emps.Rows.Count > 0))
            {
                string EmpWrk = string.Empty;
                string EmpName = string.Empty;
                string EmpDesg = string.Empty;
                Int32 TotaPresentCount = 0;

                for (int i = 0; i < dt_emps.Rows.Count; i++)
                {
                    EmpWrk = dt_emps.Rows[i][0].ToString();
                    EmpName = dt_emps.Rows[i][1].ToString();
                    EmpDesg = dt_emps.Rows[i][2].ToString();


                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    string findempquery1 = "select CreatedDate, EmployeeWrk,EmployeeName, AttendanceStatus,AttendanceCode, SUM(ProvidedOT) as ProvidedOT from tbl_attendance where CreatedDate between '" + date1 + "' and '" + date2 + "' and EmployeeWrk='" + EmpWrk + "' and SiteIncharge_Approval='Approved' Group by CreatedDate, EmployeeWrk,EmployeeName, AttendanceStatus,AttendanceCode order by CreatedDate";
                    SqlCommand cmd1 = new SqlCommand(findempquery1, dbcl.Conn);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);

                    dt_present.Rows.Clear();
                    da1.Fill(dt_present);
                    dbcl.DisconnectDb();
                    dbcl.Conn.Close();


                    Int32 daycount = 0;
                    Int32 halfdaycount = 0;
                    str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:white; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + Sl + "</td>";
                    str = str + "<td width='5%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + EmpWrk + "</td>";
                    str = str + "<td width='9%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + EmpName + "</td>";
                    str = str + "<td width='9%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + EmpDesg + "</td>";
                    Int32 day = minday;
                    for (int j = minday; j <= maxday; j++)
                    {
                        string days = "";
                        if (day <= 9)
                        { days = "0" + day.ToString(); }
                        else
                        { days = day.ToString(); }
                        Find_Present(days, month_no, year, EmpWrk, ref daycount, ref halfdaycount);

                        day = day + 1;
                    }
                    Calculate_TTLpresent(daycount, halfdaycount, ref TotaPresentCount);
                    str = str + "<td width='10%' style='background-color:white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + TotaPresentCount + "</td></tr></table>";
                    Sl = Sl + 1;
                    dayss = 0;
                }
            }

            //using (SqlDataReader re = cmd.ExecuteReader())
            //{
            //    Int32 TotaPresentCount = 0;
            //    while (re.Read())
            //    {
            //        Int32 daycount = 0;
            //        Int32 halfdaycount = 0;
            //        //Int32 ots = 0;
            //        string wrkman = re["WorkmanSL"].ToString();
            //        //string wrkman = "1224";
            //        str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:white; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + Sl + "</td>";
            //        str = str + "<td width='5%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + wrkman + "</td>";
            //        str = str + "<td width='18%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["FullName"] + "</td>";
            //        Int32 day = minday;
            //        for (int j = minday; j <= maxday; j++)
            //        {
            //            string days = "";
            //            if (day <= 9)
            //            {
            //                days = "0" + day.ToString();
            //            }
            //            else
            //            {
            //                days = day.ToString();
            //            }
            //            FindAttendance2(days, month_no, year, wrkman, ref daycount, ref halfdaycount);
            //            day = day + 1;
            //        }
            //        CalculatePresents2(daycount, halfdaycount, ref TotaPresentCount);
            //        str = str + "<td width='10%' style='background-color:white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + TotaPresentCount + "</td></tr></table>";
            //        Sl = Sl + 1;
            //        dayss = 0;
            //    }
            //}
        }

        private void Calculate_TTLpresent(Int32 daycount, Int32 TOtalHalfPresents, ref Int32 TotaPresentCount)
        {
            Int32 HPtoP = 0;
            Int32 HP2 = 0;
            Int32 HP3 = 0;

            if (TOtalHalfPresents % 2 == 0)
            {
                HPtoP = TOtalHalfPresents / 2;
            }
            else
            {
                if (TOtalHalfPresents == 1)
                {
                    HPtoP = 1;
                }
                else
                {
                    HP2 = TOtalHalfPresents - 1;
                    if (HP2 % 2 == 0)
                    {
                        HP3 = HP2 / 2;
                        HPtoP = HP3 + 1;
                    }
                }
            }
            Int32 TOtalPresents = daycount + TOtalHalfPresents;
            TotaPresentCount = TOtalPresents + HPtoP;
        }
        private void Find_Present(string day, string month, string year, string wrk, ref Int32 dayss, ref Int32 Halfdays)
        {
            string date = year + "-" + month + "-" + day;
            DataRow[] result = new DataRow[0];
            DataTable dtTemp = new DataTable();

            if ((dt_present != null) && (dt_present.Rows.Count > 0))
            {
                result = dt_present.Select("CreatedDate = #" + date + "#");
                if (result.Length > 0)
                {
                    dtTemp = result.CopyToDataTable();
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        string present = dtTemp.Rows[i][3].ToString();
                        string status = dtTemp.Rows[i][4].ToString();

                        if (present == "Present")
                        {
                            switch (status)
                            {
                                case "P":
                                    dayss = dayss + 1;
                                    str = str + "<td width='2%' style='background-color: #91ee3a; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
                                    break;

                                case "NH":
                                    dayss = dayss + 1;
                                    str = str + "<td width='2%' style='background-color: #00cbf3; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
                                    break;

                                case "FL":
                                    dayss = dayss + 1;
                                    str = str + "<td width='2%' style='background-color: #00cbf3; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
                                    break;

                                case "OD":
                                    str = str + "<td width='2%' style='background-color: #c4ff0e; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
                                    break;

                                default:
                                    str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
                                    break;
                            }
                        }
                        else
                        {
                            str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
                        }
                    }
                    dtTemp.Clear();
                }
                else
                {
                    str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
                }
            }
            else
            {
                str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
            }


            //dbcl.Sqlconnection();
            //dbcl.ConnectDb();
            //string cmdstring1 = "select max(AttendanceStatus) as AttendanceStatus, AttendanceCode, COALESCE(SUM(ProvidedOT),0) as OT from tbl_attendance where CreatedDate = '" + date + "' and EmployeeWrk='" + wrk + "' and SiteIncharge_Approval='Approved' group by AttendanceCode";
            //SqlCommand cmd1 = new SqlCommand(cmdstring1, dbcl.Conn);
            //SqlDataReader re1 = cmd1.ExecuteReader();

            //if (re1.HasRows)
            //{
            //    while (re1.Read())
            //    {
            //        string present = re1["AttendanceStatus"].ToString();
            //        string status = re1["AttendanceCode"].ToString();
            //        if (present == "Present")
            //        {
            //            switch (status)
            //            {
            //                case "P":
            //                    dayss = dayss + 1;
            //                    str = str + "<td width='2%' style='background-color: #91ee3a; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
            //                    break;

            //                case "NH":
            //                    dayss = dayss + 1;
            //                    str = str + "<td width='2%' style='background-color: #00cbf3; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
            //                    break;

            //                case "FL":
            //                    dayss = dayss + 1;
            //                    str = str + "<td width='2%' style='background-color: #00cbf3; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
            //                    break;

            //                case "OD":
            //                    str = str + "<td width='2%' style='background-color: #c4ff0e; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
            //                    break;

            //                default:
            //                    str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
            //                    break;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
            //}
            //dbcl.Conn.Close();
        }

        //----------------------------------------------- OT Sheets --------------------------------------------------------------//

        private void Bind_OverTimeRpt(Int32 minday, Int32 maxday, string month_no, string year, string region)
        {
            DateTime stamp1 = DateTime.Now;
            Label1.Text = stamp1.ToString();

            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='background-color:#00a8f3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center' colspan='35'> " + month_no.ToString() + " " + year.ToString() + "</span></td></tr></table>";
            Bind_OvertimeHdr(minday, maxday, month_no, year, region);
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='font:normal 16px/16px Century Gothic; padding:5px 20px 5px 20px;' align='cen ter'></td></tr></table>";
            lblTotalData.Text = str;
            DateTime stamp2 = DateTime.Now;
            Label2.Text = stamp2.ToString();

            Label3.Text = CalculateReportTime(stamp1, stamp2);
        }

        private void Bind_OvertimeHdr(Int32 minday, Int32 maxday, string month_no, string year, string region)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>S. NO</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>WORK SL</td>";
            str = str + "<td width='9%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EMPLOYEE NAME</td>";
            str = str + "<td width='9%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EMPLOYEE DESG</td>";
            for (int i = minday; i <= maxday; i++)
            {
                str = str + "<td width='2%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + i + "</td>";
            }
            str = str + "<td width='10%' style='background-color:#92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>TOTAL PRESENTS</td></tr></table>";

            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            int Sl = 1;
            string findempquery = "";
            if (DDL_EmpWorkStatus.SelectedIndex == 1)
            {
                findempquery = "select WorkmanSL, FullName, SkillDesignation from tbl_Employee_Mustertable where WorkRegion='" + region + "' and F16_YesNo='Yes' order by Id";
            }
            else
            {
                findempquery = "select WorkmanSL, FullName, SkillDesignation from tbl_Employee_Mustertable where WorkRegion='" + region + "' and WorkStatus = '" + DDL_EmpWorkStatus.SelectedItem.Text.ToString() + "' and F16_YesNo='Yes' order by Id";
            }
            //findempquery = "select top(20) WorkmanSL, FullName from tbl_Employee_Mustertable where WorkRegion='" + region + "' and WorkStatus='" + DDL_EmpWorkStatus.SelectedItem.Text.ToString() + "' order by Id";

            SqlCommand cmd = new SqlCommand(findempquery, dbcl.Conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            dt_emps.Rows.Clear();
            da.Fill(dt_emps);
            dbcl.Sqlconnection(); dbcl.ConnectDb();

            if ((dt_emps != null) && (dt_emps.Rows.Count > 0))
            {
                string EmpWrk = string.Empty;
                string EmpName = string.Empty;
                string EmpDesg = string.Empty;

                for (int i = 0; i < dt_emps.Rows.Count; i++)
                {
                    EmpWrk = dt_emps.Rows[i][0].ToString();
                    EmpName = dt_emps.Rows[i][1].ToString();
                    EmpDesg = dt_emps.Rows[i][2].ToString();

                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    string findempquery1 = "select CreatedDate, EmployeeWrk, SUM(ProvidedOT) as ProvidedOT from tbl_attendance where CreatedDate between '" + date1 + "' and '" + date2 + "' and EmployeeWrk='" + EmpWrk + "' and SiteIncharge_Approval='Approved' Group by CreatedDate, EmployeeWrk order by CreatedDate";
                    SqlCommand cmd1 = new SqlCommand(findempquery1, dbcl.Conn);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                    dt_ot.Rows.Clear();
                    da1.Fill(dt_ot);
                    dbcl.DisconnectDb();
                    dbcl.Conn.Close();

                    decimal ots = .0m;
                    decimal ttlot = .0m;
                    str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:white; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + Sl + "</td>";
                    str = str + "<td width='5%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + EmpWrk + "</td>";
                    str = str + "<td width='9%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + EmpName + "</td>";
                    str = str + "<td width='9%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + EmpDesg + "</td>";
                    Int32 day = minday;
                    for (int j = minday; j <= maxday; j++)
                    {
                        string days = "";
                        if (day <= 9)
                        { days = "0" + day.ToString(); }
                        else
                        { days = day.ToString(); }

                        DateTime Crntdate = DateTime.Now;
                        string date = year + "-" + month_no + "-" + day;
                        Crntdate = Convert.ToDateTime(date);

                        DataRow[] result = new DataRow[0];
                        DataRow[] result2 = new DataRow[0];
                        DataTable dtTemp = new DataTable();
                        DateTime matchdt = DateTime.Now;
                        decimal OT = .0m;
                        if ((dt_ot != null) && (dt_ot.Rows.Count > 0))
                        {
                            //result = dt_ot.Select("CreatedDate = #" + date + "# AND ProvidedOT >= 1");
                            result = dt_ot.Select("ProvidedOT >= 1");
                            if (result.Length > 0)
                            {
                                dtTemp = result.CopyToDataTable();
                                result2 = dtTemp.Select("CreatedDate = #" + date + "#");
                                if (result2.Length > 0)
                                {
                                    DataTable dtTemp2 = new DataTable();
                                    dtTemp2 = result2.CopyToDataTable();

                                    for (int k = 0; k < dtTemp2.Rows.Count; k++)
                                    {
                                        matchdt = Convert.ToDateTime(dtTemp2.Rows[k][0]);
                                        string dbot = dtTemp2.Rows[k][2].ToString();
                                        OT = Convert.ToDecimal(dbot);

                                        if (matchdt == Crntdate)
                                        {
                                            dayss = dayss + 1;
                                            ots = OT;
                                            str = str + "<td width='2%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + OT + "</td>";
                                        }

                                    }
                                }
                                else
                                {
                                    str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>0</td>";
                                }
                            }
                            else
                            {
                                str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>0</td>";
                            }
                        }
                        else
                        {
                            str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>0</td>";
                        }
                        //FindAttendance3(days, month_no, year, EmpWrk, ref daycount, ref ots);
                        day = day + 1;
                        ttlot = ttlot + ots;
                        ots = .0m;
                    }
                    str = str + "<td width='10%' style='background-color:white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + ttlot + "</td></tr></table>";
                    Sl = Sl + 1;
                    dayss = 0;
                    ttlot = 0;
                }
            }
        }

        private void FindAttendance3(string day, string month, string year, string wrk, ref Int32 dayss, ref decimal ots)
        {

            DateTime Crntdate = DateTime.Now;
            string date = year + "-" + month + "-" + day;
            Crntdate = Convert.ToDateTime(date);

            DataRow[] result = new DataRow[0];
            DataTable dtTemp = new DataTable();

            DateTime matchdt = DateTime.Now;
            if ((dt_ot != null) && (dt_ot.Rows.Count > 0))
            {
                //result = dt_ot.Select("CreatedDate = #" + date + "# AND ProvidedOT >= 1");
                result = dt_ot.Select("ProvidedOT >= 1");
                if (result.Length > 0)
                {
                    dtTemp = result.CopyToDataTable();
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        matchdt = Convert.ToDateTime(dtTemp.Rows[i][0]);
                        if (matchdt == Crntdate)
                        {
                            string dbot = dtTemp.Rows[i][2].ToString();
                            decimal OT = Convert.ToDecimal(dbot);
                            dayss = dayss + 1;
                            ots = OT;
                            str = str + "<td width='2%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + OT + "</td>";
                        }
                        else
                        {
                            ots = .0m;
                            str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>0</td>";
                        }


                        //string dbot = dtTemp.Rows[i][2].ToString();
                        //decimal OT = Convert.ToDecimal(dbot);
                        //if (OT > 0)
                        //{
                        //    dayss = dayss + 1;
                        //    ots = OT;
                        //    str = str + "<td width='2%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + OT + "</td>";
                        //}
                        //else
                        //{
                        //    ots = .0m;
                        //    str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>0</td>";
                        //}
                    }
                    dtTemp.Clear();
                }
                else
                {
                    str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>0</td>";
                }
            }
            else
            {
                str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>0</td>";
            }

            //string date = year + "-" + month + "-" + day;
            //dbcl.Sqlconnection();
            //dbcl.ConnectDb();
            //string cmdstring1 = "select COALESCE(SUM(ProvidedOT),0) as OT from tbl_attendance where CreatedDate = '" + date + "' and EmployeeWrk='" + wrk + "' and SiteIncharge_Approval='Approved'";
            //SqlCommand cmd1 = new SqlCommand(cmdstring1, dbcl.Conn);
            //SqlDataReader re1 = cmd1.ExecuteReader();
            //if (re1.HasRows)
            //{
            //    while (re1.Read())
            //    {
            //        string dbot = re1["OT"].ToString();
            //        decimal OT = Convert.ToDecimal(dbot);
            //        if (OT > 0)
            //        {
            //            dayss = dayss + 1;
            //            ots = OT;
            //            str = str + "<td width='2%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + OT + "</td>";
            //        }
            //        else
            //        {
            //            ots = .0m;
            //            str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>0</td>";
            //        }
            //    }
            //}
            //else
            //{
            //    str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>0</td>";
            //}
            //dbcl.Conn.Close();
        }

        //----------------------------------- Combined Sheets - Double Line ----------------- 28/05/2022-------------------------------------------------//

        private void Bind_PresentOT_Rpt(Int32 minday, Int32 maxday, string month_no, string year, string region)
        {
            DateTime stamp1 = DateTime.Now;
            Label1.Text = stamp1.ToString();

            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='background-color:#00a8f3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center' colspan='35'> Form XVI || " + region + " || " + month_no.ToString() + " / " + year.ToString() + "</span></td></tr></table>";
            Bind_CombHdr(minday, maxday, month_no, year, region);
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='font:normal 16px/16px Century Gothic; padding:5px 20px 5px 20px;' align='cen ter'></td></tr></table>";
            lblTotalData.Text = str;

            DateTime stamp2 = DateTime.Now;
            Label2.Text = stamp2.ToString();
            Label3.Text = CalculateReportTime(stamp1, stamp2);
        }


        //Function for binding Absent / Present Row
        private void Bind_CombHdr(Int32 minday, Int32 maxday, string month_no, string year, string region)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>S. NO</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>WORK SL</td>";
            str = str + "<td width='9%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EMPLOYEE NAME</td>";
            str = str + "<td width='9%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EMPLOYEE DESG</td>";
            for (int i = minday; i <= maxday; i++)
            {
                str = str + "<td width='2%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + i + "</td>";
            }
            str = str + "<td width='10%' style='background-color:#92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>TOTAL PRESENTS</td></tr></table>";

            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            int Sl = 1;

            string findempquery = "";
            if (DDL_EmpWorkStatus.SelectedIndex == 1)
            {
                findempquery = "select WorkmanSL, FullName,SkillDesignation from tbl_Employee_Mustertable where WorkRegion='" + region + "' and F16_YesNo='Yes' order by Id";
            }
            else
            {
                findempquery = "select WorkmanSL, FullName,SkillDesignation from tbl_Employee_Mustertable where WorkRegion='" + region + "' and WorkStatus='" + DDL_EmpWorkStatus.SelectedItem.Text.ToString() + "' and F16_YesNo='Yes' order by Id";
            }

            //findempquery = "select top(10) WorkmanSL, FullName from tbl_Employee_Mustertable where WorkRegion='" + region + "' and WorkStatus='" + DDL_EmpWorkStatus.SelectedItem.Text.ToString() + "' order by Id";
            SqlCommand cmd = new SqlCommand(findempquery, dbcl.Conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            dt_emps.Rows.Clear();
            da.Fill(dt_emps);
            dbcl.Sqlconnection(); dbcl.ConnectDb();

            if ((dt_emps != null) && (dt_emps.Rows.Count > 0))
            {
                string EmpWrk = string.Empty;
                string EmpName = string.Empty;
                string EmpDesg = string.Empty;
                Int32 TotaPresentCount = 0;

                for (int i = 0; i < dt_emps.Rows.Count; i++)
                {
                    EmpWrk = dt_emps.Rows[i][0].ToString();
                    EmpName = dt_emps.Rows[i][1].ToString();
                    EmpDesg = dt_emps.Rows[i][2].ToString();

                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    string findempquery1 = "select CreatedDate, EmployeeWrk,EmployeeName, AttendanceStatus,AttendanceCode, SUM(ProvidedOT) as ProvidedOT from tbl_attendance where CreatedDate between '" + date1 + "' and '" + date2 + "' and EmployeeWrk='" + EmpWrk + "' and SiteIncharge_Approval='Approved' Group by CreatedDate, EmployeeWrk,EmployeeName, AttendanceStatus,AttendanceCode order by CreatedDate";
                    SqlCommand cmd1 = new SqlCommand(findempquery1, dbcl.Conn);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                    dt_present.Rows.Clear();
                    da1.Fill(dt_present);
                    dbcl.DisconnectDb();
                    dbcl.Conn.Close();

                    Int32 daycount = 0;
                    Int32 halfdaycount = 0;
                    str = str + "<table width='100%' rowspan=2 style='border-collapse:collapse;'><tr><td width='5%' style='background-color:white; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + Sl + "</td>";
                    str = str + "<td width='5%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + EmpWrk + "</td>";
                    str = str + "<td width='9%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + EmpName + "</td>";
                    str = str + "<td width='9%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + EmpDesg + "</td>";
                    Int32 day = minday;
                    for (int j = minday; j <= maxday; j++)
                    {
                        string days = "";
                        if (day <= 9)
                        { days = "0" + day.ToString(); }
                        else
                        { days = day.ToString(); }
                        Find_CombPresent(days, month_no, year, EmpWrk, ref daycount, ref halfdaycount);

                        day = day + 1;
                    }
                    Calculate_TTLpresent(daycount, halfdaycount, ref TotaPresentCount);
                    str = str + "<td width='10%' style='background-color:white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + TotaPresentCount + "</td></tr></table>";
                    //here goes the function call for OT Row

                    BindHeadingOTRow4(minday, maxday, month_no, year, region, EmpWrk, EmpName, EmpDesg);


                    Sl = Sl + 1;
                    dayss = 0;
                }
            }


            //The below lines are commented on 26-Feb-2023 to alternate process for PresentOT report
            //DataTable dt = new DataTable();
            //using (SqlCommand cmd = new SqlCommand(findempquery, dbcl.Conn))
            //{
            //    cmd.CommandType = CommandType.Text;
            //    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            //    {
            //        sda.Fill(dt);
            //    }
            //}

            //foreach (DataRow row in dt_emps.Rows)
            //{
            //    Int32 daycount = 0;
            //    //Int32 ots = 0;
            //    string wrkman = row["WorkmanSL"].ToString();
            //    string emp_name = row["FullName"].ToString();
            //    str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:white; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + Sl + "</td>";
            //    str = str + "<td width='5%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + wrkman + "</td>";
            //    str = str + "<td width='18%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + emp_name + "</td>";
            //    Int32 day = minday;
            //    for (int j = minday; j <= maxday; j++)
            //    {
            //        string days = "";
            //        if (day <= 9)
            //        {
            //            days = "0" + day.ToString();
            //        }
            //        else
            //        {
            //            days = day.ToString();
            //        }
            //        FindAttendance4(days, month_no, year, wrkman, ref daycount, region);
            //        day = day + 1;
            //    }
            //    str = str + "<td width='10%' style='background-color:white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + daycount + "</td></tr></table>";
            //    BindHeadingOTRow4(minday, maxday, month_no, year, region, wrkman, emp_name);
            //    Sl = Sl + 1;
            //    dayss = 0;
            //}




            //SqlCommand cmd1 = new SqlCommand(findempquery, dbcl.Conn);
            //using (SqlDataReader re = cmd1.ExecuteReader())
            //{
            //    while (re.Read())
            //    {
            //        Int32 daycount = 0;
            //        //Int32 ots = 0;
            //        string wrkman = re["WorkmanSL"].ToString();
            //        string emp_name = re["FullName"].ToString();
            //        str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:white; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + Sl + "</td>";
            //        str = str + "<td width='5%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + wrkman + "</td>";
            //        str = str + "<td width='18%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + emp_name + "</td>";
            //        Int32 day = minday;
            //        for (int j = minday; j <= maxday; j++)
            //        {
            //            string days = "";
            //            if (day <= 9)
            //            {
            //                days = "0" + day.ToString();
            //            }
            //            else
            //            {
            //                days = day.ToString();
            //            }
            //            FindAttendance4(days, month_no, year, wrkman, ref daycount, region);
            //            day = day + 1;
            //        }
            //        str = str + "<td width='10%' style='background-color:white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + daycount + "</td></tr></table>";
            //        BindHeadingRow4(minday, maxday, month_no, year, region, wrkman, emp_name);
            //        Sl = Sl + 1;
            //        dayss = 0;
            //    }
            //}
        }

        private void Find_CombPresent(string day, string month, string year, string wrk, ref Int32 dayss, ref Int32 Halfdays)
        {
            string date = year + "-" + month + "-" + day;
            DataRow[] result = new DataRow[0];
            DataTable dtTemp = new DataTable();

            if ((dt_present != null) && (dt_present.Rows.Count > 0))
            {
                result = dt_present.Select("CreatedDate = #" + date + "#");
                if (result.Length > 0)
                {
                    dtTemp = result.CopyToDataTable();
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        string present = dtTemp.Rows[i][3].ToString();
                        string status = dtTemp.Rows[i][4].ToString();

                        if (present == "Present")
                        {
                            switch (status)
                            {
                                case "P":
                                    dayss = dayss + 1;
                                    str = str + "<td width='2%' style='background-color: #91ee3a; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
                                    break;

                                case "NH":
                                    dayss = dayss + 1;
                                    str = str + "<td width='2%' style='background-color: #00cbf3; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
                                    break;

                                case "FL":
                                    dayss = dayss + 1;
                                    str = str + "<td width='2%' style='background-color: #00cbf3; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
                                    break;

                                case "OD":
                                    str = str + "<td width='2%' style='background-color: #c4ff0e; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
                                    break;

                                default:
                                    str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
                                    break;
                            }
                        }
                        else
                        {
                            str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
                        }
                    }
                    dtTemp.Clear();
                }
                else
                {
                    str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
                }
            }
            else
            {
                str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
            }


            //dbcl.Sqlconnection();
            //dbcl.ConnectDb();
            //string cmdstring1 = "select max(AttendanceStatus) as AttendanceStatus, AttendanceCode, COALESCE(SUM(ProvidedOT),0) as OT from tbl_attendance where CreatedDate = '" + date + "' and EmployeeWrk='" + wrk + "' and SiteIncharge_Approval='Approved' group by AttendanceCode";
            //SqlCommand cmd1 = new SqlCommand(cmdstring1, dbcl.Conn);
            //SqlDataReader re1 = cmd1.ExecuteReader();

            //if (re1.HasRows)
            //{
            //    while (re1.Read())
            //    {
            //        string present = re1["AttendanceStatus"].ToString();
            //        string status = re1["AttendanceCode"].ToString();
            //        if (present == "Present")
            //        {
            //            switch (status)
            //            {
            //                case "P":
            //                    dayss = dayss + 1;
            //                    str = str + "<td width='2%' style='background-color: #91ee3a; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
            //                    break;

            //                case "NH":
            //                    dayss = dayss + 1;
            //                    str = str + "<td width='2%' style='background-color: #00cbf3; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
            //                    break;

            //                case "FL":
            //                    dayss = dayss + 1;
            //                    str = str + "<td width='2%' style='background-color: #00cbf3; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
            //                    break;

            //                case "OD":
            //                    str = str + "<td width='2%' style='background-color: #c4ff0e; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
            //                    break;

            //                default:
            //                    str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
            //                    break;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
            //}
            //dbcl.Conn.Close();
        }

        private void FindAttendance4(string day, string month, string year, string wrk, ref Int32 dayss, string region)
        {
            string date = year + "-" + month + "-" + day;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            string cmdstring1 = "select max(AttendanceStatus) as AttendanceStatus, AttendanceCode, COALESCE(SUM(ProvidedOT),0) as OT from tbl_attendance where CreatedDate = '" + date + "' and EmployeeWrk='" + wrk + "' and SiteIncharge_Approval='Approved' group by AttendanceCode";
            SqlCommand cmd1 = new SqlCommand(cmdstring1, dbcl.Conn);
            SqlDataReader re1 = cmd1.ExecuteReader();

            if (re1.HasRows)
            {
                while (re1.Read())
                {
                    string present = re1["AttendanceStatus"].ToString();
                    string status = re1["AttendanceCode"].ToString();
                    if (present == "Present")
                    {
                        switch (status)
                        {
                            case "P":
                                dayss = dayss + 1;
                                str = str + "<td width='2%' style='background-color: #91ee3a; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
                                break;

                            case "NH":
                                dayss = dayss + 1;
                                str = str + "<td width='2%' style='background-color: #00cbf3; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
                                break;

                            case "FL":
                                dayss = dayss + 1;
                                str = str + "<td width='2%' style='background-color: #00cbf3; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
                                break;

                            case "OD":
                                str = str + "<td width='2%' style='background-color: #c4ff0e; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + status + "</td>";
                                break;

                            default:
                                str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
                                break;
                        }
                    }
                    //else
                    //{
                    //    str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
                    //}
                }
            }
            else
            {
                str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
            }
            dbcl.Conn.Close();
        }

        //Function for Binding OT Row
        private void BindHeadingOTRow4(Int32 minday, Int32 maxday, string month_no, string year, string region, string wrkman, string emp_name, string emp_desg)
        {

            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string findempquery2 = "select CreatedDate, EmployeeWrk, SUM(ProvidedOT) as ProvidedOT from tbl_attendance where CreatedDate between '" + date1 + "' and '" + date2 + "' and EmployeeWrk='" + wrkman + "' and SiteIncharge_Approval='Approved' Group by CreatedDate, EmployeeWrk order by CreatedDate";
            SqlCommand cmd2 = new SqlCommand(findempquery2, dbcl.Conn);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            dt_ot.Rows.Clear();
            da2.Fill(dt_ot);
            dbcl.DisconnectDb();
            dbcl.Conn.Close();


            int Sl = 1;
            Int32 daycount = 0;
            decimal ots = .0m;
            decimal ttlot = .0m;
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:white; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'></td>";
            str = str + "<td width='5%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + wrkman + "</td>";
            str = str + "<td width='9%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + emp_name + "</td>";
            str = str + "<td width='9%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + emp_desg + "</td>";
            Int32 day = minday;
            for (int j = minday; j <= maxday; j++)
            {
                string days = "";
                if (day <= 9)
                {
                    days = "0" + day.ToString();
                }
                else
                {
                    days = day.ToString();
                }
                DateTime Crntdate = DateTime.Now;
                string date = year + "-" + month_no + "-" + day;
                Crntdate = Convert.ToDateTime(date);

                DataRow[] result = new DataRow[0];
                DataRow[] result2 = new DataRow[0];
                DataTable dtTemp = new DataTable();
                DateTime matchdt = DateTime.Now;
                decimal OT = .0m;
                if ((dt_ot != null) && (dt_ot.Rows.Count > 0))
                {
                    //result = dt_ot.Select("CreatedDate = #" + date + "# AND ProvidedOT >= 1");
                    result = dt_ot.Select("ProvidedOT >= 1");
                    if (result.Length > 0)
                    {
                        dtTemp = result.CopyToDataTable();
                        result2 = dtTemp.Select("CreatedDate = #" + date + "#");
                        if (result2.Length > 0)
                        {
                            DataTable dtTemp2 = new DataTable();
                            dtTemp2 = result2.CopyToDataTable();

                            for (int k = 0; k < dtTemp2.Rows.Count; k++)
                            {
                                matchdt = Convert.ToDateTime(dtTemp2.Rows[k][0]);
                                string dbot = dtTemp2.Rows[k][2].ToString();
                                OT = Convert.ToDecimal(dbot);

                                if (matchdt == Crntdate)
                                {
                                    dayss = dayss + 1;
                                    ots = OT;
                                    str = str + "<td width='2%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + OT + "</td>";
                                }

                            }
                        }
                        else
                        {
                            str = str + "<td width='2%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>0</td>";
                        }
                    }
                    else
                    {
                        str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>0</td>";
                    }
                }
                else
                {
                    str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>0</td>";
                }
                //FindAttendance3(days, month_no, year, EmpWrk, ref daycount, ref ots);
                day = day + 1;
                ttlot = ttlot + ots;
                ots = .0m;
            }
            str = str + "<td width='10%' style='background-color:white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + ttlot + "</td></tr></table>";
            Sl = Sl + 1;
            dayss = 0;
            ttlot = .0m;
        }

        private void FindAttendanceOT4(string day, string month, string year, string wrk, ref Int32 dayss, ref decimal ots, string region)
        {
            string date = year + "-" + month + "-" + day;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdstring1 = "select COALESCE(SUM(ProvidedOT),0) as OT from tbl_attendance where CreatedDate = '" + date + "' and EmployeeWrk='" + wrk + "' and SiteIncharge_Approval='Approved'";
            SqlCommand cmd1 = new SqlCommand(cmdstring1, dbcl.Conn);
            SqlDataReader re1 = cmd1.ExecuteReader();

            if (re1.HasRows)
            {
                while (re1.Read())
                {
                    string dbot = re1["OT"].ToString();
                    decimal OT = Convert.ToDecimal(dbot);
                    if (OT > 0)
                    {
                        dayss = dayss + 1;
                        ots = OT;
                        str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + OT + "</td>";
                    }
                    else
                    {
                        ots = .0m;
                        str = str + "<td width='2%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'></td>";
                    }
                }
            }
            else
            {
                str = str + "<td width='2%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'></td>";
            }
            dbcl.Conn.Close();
        }

        protected void btn_excelexport_Click(object sender, EventArgs e)
        {
            string strt = "AttendanceReport";
            string regn = DDL_Region.SelectedItem.Text.ToString();
            string month = DDL_Month.SelectedItem.Text.ToString();
            string year = DDL_Year.SelectedItem.Text.ToString();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + strt + "_" + regn + "_" + month + "_" + year + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.Output.Write(Request.Form[hfGridHtml.UniqueID]);
            Response.Flush();
            Response.End();
        }

        
    }
}