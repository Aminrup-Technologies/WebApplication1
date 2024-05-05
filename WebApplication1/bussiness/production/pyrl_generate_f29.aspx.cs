using System;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.bussiness.production
{
    public partial class pyrl_generate_f29 : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        string str = string.Empty;
        Int32 CalMonthDays = 0;
        Int32 StartDay = 0;
        Int32 EndDay = 0;

        public static string state = string.Empty;
        public static string region = string.Empty;
        public static string comp = string.Empty;

        public static string StartDate = string.Empty;
        public static string EndDate = string.Empty;

        DataTable dt_emps = new DataTable();
        DataTable dt_firsthalf = new DataTable();
        DataTable dt_present = new DataTable();

        private static decimal TTL_Present = .0m;
        private static decimal TTL_OverTime = .0m;
        private static decimal TTL_BasicSalary = .0m;
        private static decimal TTL_HRAPay = .0m;
        private static decimal TTL_ConvPay = .0m;
        private static decimal TTL_WashPay = .0m;
        private static decimal TTL_OTSalary = .0m;
        private static decimal TTL_OthersPay = .0m;
        private static decimal TTL_ActualGross = .0m;
        private static decimal TTL_ESICGross = .0m;
        private static decimal TTL_ESICPay = .0m;
        private static decimal TTL_PFPay = .0m;
        private static decimal TTL_Advance = .0m;
        private static decimal TTL_Fines = .0m;
        private static decimal TTL_Others = .0m;
        private static decimal TTL_TotalDeduction = .0m;
        private static decimal TTL_NetPay1 = .0m;
        private static decimal TTL_NetPayFinal = .0m;
        private static decimal TTL_NetPay2 = .0m;

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
                    if (Session["Changer"] != null)
                    {
                        string[] retrievedArray = (string[])Session["Changer"];
                        region = retrievedArray[1].ToString();
                        comp = retrievedArray[2].ToString();
                        state = retrievedArray[0].ToString();

                        //Session["Changer"] = null;
                    }
                    else
                    {
                        region = Session["REGION"].ToString();
                        comp = Session["COMPANY_CODE"].ToString();
                        state = Session["STATE"].ToString();
                    }

                    string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = 'IN' and State_Code ='" + state + "' order by Id ";
                    BindRegions(CmdString1);

                    string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='" + state + "' and Work_Region_Code = '" + region + "' order by Id ";
                    BindCompany(CmdString3);

                    DDL_Region.SelectedValue = region;
                    DDL_Company.SelectedValue = comp;

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
            string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='" + state + "' and Work_Region_Code = '" + region + "' order by Id ";
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
            DateTime stamp1 = DateTime.Now;
            Label1.Text = stamp1.ToString();

            //string strtday = "01";
            string strtday = DDL_Day.SelectedItem.Text.ToString();
            Int32 minday = Convert.ToInt32(strtday);

            //string endday = "30";
            string endday = DDL_D2.SelectedItem.Text.ToString();
            Int32 maxday = Convert.ToInt32(endday);

            string current_year = DDL_Year.SelectedItem.Text.ToString();
            string current_month1 = DDL_Month.SelectedItem.Text.ToString();
            string Region = DDL_Region.SelectedValue.ToString();

            string F17_TrialStatus = string.Empty;
            string FinalStatus = string.Empty;
            string FinalF17_Status = string.Empty;

            CheckforMonthDay(current_year, current_month1, Region, ref CalMonthDays, ref StartDay, ref EndDay, ref F17_TrialStatus, ref FinalStatus, ref FinalF17_Status);

            StartDate = StartDay + " / " + current_month1 + " / " + current_year + "";
            EndDate = EndDay + " / " + current_month1 + " / " + current_year + "";

            if (F17_TrialStatus == "Yes" && FinalF17_Status == "Settled")
            {
                TTL_Present = .0m;
                TTL_OverTime = .0m;
                TTL_BasicSalary = .0m;
                TTL_HRAPay = .0m;
                TTL_ConvPay = .0m;
                TTL_WashPay = .0m;
                TTL_OTSalary = .0m;
                TTL_OthersPay = .0m;
                TTL_ActualGross = .0m;
                TTL_ESICGross = .0m;
                TTL_ESICPay = .0m;
                TTL_PFPay = .0m;
                TTL_Advance = .0m;
                TTL_Fines = .0m;
                TTL_Others = .0m;
                TTL_TotalDeduction = .0m;
                TTL_NetPay1 = .0m;
                TTL_NetPayFinal = .0m;
                TTL_NetPay2 = .0m;

                Binder(current_year, current_month1, Region);
                //Binder2(current_year, current_month1, Region);
                BindDefaultHeaderYES(current_year, current_month1, Region, CalMonthDays);
                BindSecondHeader(current_year, current_month1, CalMonthDays, StartDay, EndDay);
                BindEMployeeData(current_year, current_month1, Region, CalMonthDays, StartDay, EndDay);
                BindTotalRow();

                //Response.Write("<script>window.open ('/bussiness/production/rpts/f29.aspx?Year=" + current_year + "&Month=" + current_month1 + "&Region=" + region + "','_blank');</script>");
                btnExport.Enabled = true;
            }
            else
            {
                string title = "Notifications :";
                string body = "Payroll NOT Generated...!!!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }

            DateTime stamp2 = DateTime.Now;
            Label2.Text = stamp2.ToString();
            Label3.Text = CalculateReportTime(stamp1, stamp2);
        }

        public void Binder(string Year, string Month, string Region)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            dt_firsthalf.Clear();
            string cmdString2 = "select a.WorkmanSL, a.FullName, b.SafetyPassNo, b.Fathername, FORMAT (b.DOB, 'dd-MM-yyyy') as dob, a.SkillCategory, a.SkillDesignation, FORMAT (b.DOJ, 'dd-MM-yyyy') as doj, b.ESICNo, b.UANNo, a.Present, a.OverTime, a.BasicSalary,a.OTSalary,a.OthersPay,a.HRAPay,a.ConvPay,a.WashPay, a.ActualGross,a.ESICGross, a.PFPay,a.ESICPay,a.NetPay1,a.NetPay2, a.Advance,a.Fines,a.Others,a.TotalDeduction,a.NetPayFinal, a.Date from tbl_trialpayroll a, tbl_Employee_Mustertable b where a.SalaryYear='" + Year + "' and a.SalaryMonth='" + Month + "' and a.Region='" + Region + "' and b.WorkmanSL=a.WorkmanSL order by a.Id";
            SqlCommand cmd2 = new SqlCommand(cmdString2, dbcl.Conn);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt_firsthalf);
            dbcl.Sqlconnection(); dbcl.ConnectDb();
        }

        public void Binder2(string Year, string Month, string Region)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            dt_present.Clear();
            string cmdString2 = "select CreatedDate, EmployeeWrk,EmployeeName, AttendanceStatus,AttendanceCode, SUM(ProvidedOT) as ProvidedOT from tbl_attendance where YEAR(CreatedDate)='" + Year + "' and  MONTH(CreatedDate)='" + Month + "' and SiteIncharge_Approval='Approved' and JOB_Region='" + Region + "' Group by CreatedDate, EmployeeWrk,EmployeeName, AttendanceStatus,AttendanceCode order by CreatedDate";
            SqlCommand cmd2 = new SqlCommand(cmdString2, dbcl.Conn);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt_present);
            dbcl.Sqlconnection(); dbcl.ConnectDb();
        }

        public void EmployeeAttendanceBinder(string Year, string Month, string EmployeeWrk)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            dt_present.Clear();
            string cmdString2 = "select CreatedDate, EmployeeWrk,EmployeeName, AttendanceStatus,AttendanceCode, SUM(ProvidedOT) as ProvidedOT from tbl_attendance where YEAR(CreatedDate)='" + Year + "' and  MONTH(CreatedDate)='" + Month + "' and SiteIncharge_Approval='Approved' and EmployeeWrk='" + EmployeeWrk + "' Group by CreatedDate, EmployeeWrk,EmployeeName, AttendanceStatus,AttendanceCode order by CreatedDate";
            SqlCommand cmd2 = new SqlCommand(cmdString2, dbcl.Conn);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt_present);
            dbcl.Sqlconnection(); dbcl.ConnectDb();
        }

        public void BindDefaultHeaderYES(string Year, string Month, string Region, Int32 CalMonthDays)
        {
            Int32 Multi = CalMonthDays;

            str = str + "<table width='100%' style='border-collapse:collapse; color:black;'><tr><td colspan='75' width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 16px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>FORM 29</td></tr>";
            str = str + "<tr><td colspan='75' width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Combined Muster Roll-cum-Register of Wages</td></tr>";
            str = str + "<tr><td colspan='30' width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Name and Address of the Contractor : TECHNICAL & AUTOMATION SERVICES, Near Samudayik Vikas Bhawan, Jemco Basti, Telco, Jamshedpur - 831004</td>" +
                "<td colspan='15' width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>[See rule 72 and rule 77(2)]</td>" +
                "<td colspan='30' width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Name and Address of Establishment is/under which contract is carried on : " + DDL_Company.SelectedItem.Text.ToString() + "</td></tr>";
            str = str + "<tr><td colspan='30' width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Name and Location of Work : " + DDL_Region.SelectedItem.Text.ToString() + "</td>" +
                "<td colspan='15' width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'></td>" +
                "<td colspan='30' width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Name and Address of Principal Employeer : " + DDL_Company.SelectedItem.Text.ToString() + ", " + state + "." + "</td></tr>";
            str = str + "<tr><td colspan='30' width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Wages period from " + StartDate + " to " + EndDate + "</td>" +
                "<td colspan='15' width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'></td>" +
                "<td colspan='30' width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'></td></tr>";
            str = str + "<tr><td colspan='75' width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>--</td></tr>";
            str = str + "<tr><td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>1</td>";
            str = str + "<td width='5%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>2</td>";
            str = str + "<td width='5%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>3</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>4</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>5</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>6</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>7</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>8</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>9</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>10</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>11</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>12</td>";
            str = str + "<td colspan=" + CalMonthDays + " width=" + Multi + "  style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Attendance Sheet (Unit of Workdone)</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>13</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>14</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>15</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>16</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>17</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>18</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>19</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>20</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>21</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>22</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>23</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>24</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>25</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>26</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>27</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>28</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>29</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>30</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>31</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>32</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>33</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>34</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>35</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>36</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>37</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>38</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>39</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>40</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>41</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>42</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>43</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>44</td></tr></table>";
            lblTotalData.Text = str;
        }

        public void CheckforMonthDay(string Year, string Month, string Region, ref Int32 CalMonthDays, ref Int32 StartDay, ref Int32 EndDay, ref string F17_TrialStatus, ref string FinalStatus, ref string FinalF17_Status)
        {
            string cmdString = "select F17_TrialStatus,FinalStatus, FinalF17_Status, PayrrollMonthDays,PayrollStartDay,PayrollEndDay from tbl_MonthlyPayrollStatus where PayrollYear='" + Year + "' and PayrollMonth = '" + Month + "' and PayrollRegion = '" + Region + "'";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                F17_TrialStatus = Rdr["F17_TrialStatus"].ToString();
                FinalStatus = Rdr["FinalStatus"].ToString();
                FinalF17_Status = Rdr["FinalF17_Status"].ToString();

                CalMonthDays = Convert.ToInt32(Rdr["PayrrollMonthDays"].ToString());
                StartDay = Convert.ToInt32(Rdr["PayrollStartDay"].ToString());
                EndDay = Convert.ToInt32(Rdr["PayrollEndDay"].ToString());
            }
            dbcl.Conn.Close();
        }

        public void BindSecondHeader(string Year, string Month, Int32 CalMonthDays, Int32 StartDay, Int32 EndDay)
        {
            str = str + "<table width='100%' style='border-collapse:collapse; color:black;'><tr><td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>SL. No.</td>";
            str = str + "<td width='5%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Name of Workman</td>";
            str = str + "<td width='5%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>SafetyPass No.</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Father Name</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>SEX (M/F)</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>DOB</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Emp. No/ Sl No. in register of Employees</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Skill.</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Desig.</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>DOJ</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>ESIC IP NO.</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>PF NO.</td>";


            for (int i = StartDay; i <= EndDay; i++)
            {
                str = str + "<td width='1%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + i + "</td>";
            }

            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>No of Payable days /Total Work done</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>OT</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Name of N&FH for Which Wages have been Paid</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Basic Wages</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>D.A/ VDA</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>HRA</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Conv. Allowance</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Med.Allow</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Wash. Allowance </td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>ATT/Allow. Bonous</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Spl. Allowance</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>OT Wages</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Misc. Earnings</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Others</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Actual Gross</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>ESIC Gross</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>ESI Pay</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>PF Pay</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Socy.</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Insurance</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Sal. Adv</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>PT</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>TDS</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Advance</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Fine</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Others</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Total Ded.</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Net Payable 1</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Net Payable 1 Final</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Net Payable 2</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Date Of Payment</td>";
            str = str + "<td width='2%' style='background-color:#22b9e3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Signature /Thumb Impression</td></tr></table>";

            lblTotalData.Text = str;
            //str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='font:normal 16px/16px Century Gothic; padding:5px 20px 5px 20px;' align='center'></td></tr></table>";
        }

        public void BindEMployeeData(string Year, string Month, string Region, Int32 CalMonthDays, Int32 StartDay, Int32 EndDay)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            dt_emps.Clear();
            string cmdString = "select WorkmanSL from tbl_trialpayroll where SalaryYear='" + Year + "' and SalaryMonth='" + Month + "' and Region='" + Region + "' order by Id";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            int Sl = 1;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt_emps);
            dbcl.Sqlconnection(); dbcl.ConnectDb();

            if ((dt_emps != null) && (dt_emps.Rows.Count > 0))
            {
                string EmpWrk = string.Empty;

                for (int i = 0; i < dt_emps.Rows.Count; i++)
                {
                    EmpWrk = dt_emps.Rows[i][0].ToString();
                    str = str + "<table width='100%' style='border-collapse:collapse; color:black;'><tr><td height='70' width='3%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + Sl + "</td>";
                    //FindFirstHalfData(Year, Month, Region, CalMonthDays, StartDay, EndDay, EmpWrk);
                    EmployeeAttendanceBinder(Year, Month, EmpWrk);
                    BindFirstHalfData(Year, Month, Region, CalMonthDays, StartDay, EndDay, EmpWrk);
                    
                    Sl = Sl + 1;
                    lblTotalData.Text = str;
                }
            }
        }

        public void BindFirstHalfData(string Year, string Month, string Region, Int32 CalMonthDays, Int32 StartDay, Int32 EndDay, string workman)
        {
            // Assuming you have the desired WorkmanSL value stored in a variable called "desiredWorkmanSL"
            string desiredWorkmanSL = workman; // Replace this with the actual value you want to search for

            // Use LINQ to filter the DataTable based on the WorkmanSL value
            DataRow[] matchingRows = dt_firsthalf.Select($"WorkmanSL = '{desiredWorkmanSL}'");

            // Check if any records are found
            if (matchingRows.Length > 0)
            {
                // Retrieve the first matching record (you can loop through matchingRows for multiple matches)
                DataRow row = matchingRows[0];

                // Retrieve values from the DataRow and bind them to the label or other controls
                // Assuming you have the DataRow named "row" containing the data
                //string workmanSL = row["WorkmanSL"].ToString();
                //string fullName = row["FullName"].ToString();

                str = str + "<td width='5%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + row["FullName"].ToString() + "</td>";
                str = str + "<td width='5%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + row["SafetyPassNo"].ToString() + "</td>";
                str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + row["Fathername"].ToString() + "</td>";
                str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Male</td>";
                str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + row["dob"].ToString() + "</td>";
                str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + workman + "</td>";
                str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + row["SkillCategory"].ToString() + "</td>";
                str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + row["SkillDesignation"].ToString() + "</td>";
                str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + row["doj"].ToString() + "</td>";
                str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>'" + row["ESICNo"].ToString() + "</td>";
                str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>'" + row["UANNo"].ToString() + "</td>";

                Int32 daycount = 0;
                Int32 day = StartDay;
                for (int j = StartDay; j <= EndDay; j++)
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
                    FindAttendanceRev(days, Month, Year, workman, ref daycount, Region);
                    day = day + 1;
                }

                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + row["Present"].ToString() + "</td>";
                //str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + decimal.Parse(daycount.ToString()) + "</td>";
                TTL_Present = TTL_Present + decimal.Parse(row["Present"].ToString());

                //TTL_Present = decimal.Parse(daycount.ToString());

                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + row["OverTime"].ToString() + "</td>";
                TTL_OverTime = TTL_OverTime + decimal.Parse(row["OverTime"].ToString());

                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";

                decimal basicsal = Convert.ToDecimal(row["BasicSalary"].ToString());
                
                decimal basicotpay = Convert.ToDecimal(row["OTSalary"].ToString());
                decimal otherspay = Convert.ToDecimal(row["OthersPay"].ToString());

                decimal pf = Convert.ToDecimal(row["PFPay"].ToString());
                decimal esi = Convert.ToDecimal(row["ESICPay"].ToString());
                decimal netpay = Convert.ToDecimal(row["NetPay1"].ToString());

                decimal Advance = Convert.ToDecimal(row["Advance"].ToString());
                decimal Fines = Convert.ToDecimal(row["Fines"].ToString());
                decimal Others = Convert.ToDecimal(row["Others"].ToString());
                decimal ded = Convert.ToDecimal(row["TotalDeduction"].ToString());
                decimal NetPayFinal = Convert.ToDecimal(row["NetPayFinal"].ToString());

                decimal gross1 = basicsal + basicotpay + otherspay;
                decimal gross2 = netpay + esi + pf + ded;

                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + basicsal + "</td>";
                TTL_BasicSalary = TTL_BasicSalary + decimal.Parse(row["BasicSalary"].ToString());
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + row["HRAPay"].ToString() + "</td>";
                TTL_HRAPay = TTL_HRAPay + decimal.Parse(row["HRAPay"].ToString());
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + row["ConvPay"].ToString() + "</td>";
                TTL_ConvPay = TTL_ConvPay + decimal.Parse(row["ConvPay"].ToString());
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + row["WashPay"].ToString() + "</td>";
                TTL_WashPay = TTL_WashPay + decimal.Parse(row["WashPay"].ToString());
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + basicotpay + "</td>";
                TTL_OTSalary = TTL_OTSalary + decimal.Parse(row["OTSalary"].ToString());
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + row["OthersPay"].ToString() + "</td>";
                TTL_OthersPay = TTL_OthersPay + decimal.Parse(row["OthersPay"].ToString());
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + row["ActualGross"].ToString() + "</td>";
                TTL_ActualGross = TTL_ActualGross + decimal.Parse(row["ActualGross"].ToString());
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + row["ESICGross"].ToString() + "</td>";
                TTL_ESICGross = TTL_ESICGross + decimal.Parse(row["ESICGross"].ToString());
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + esi + "</td>";
                TTL_ESICPay = TTL_ESICPay + decimal.Parse(row["ESICPay"].ToString());
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + pf + "</td>";
                TTL_PFPay = TTL_PFPay + decimal.Parse(row["PFPay"].ToString());
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>0.00</td>";
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + Advance.ToString() + "</td>";
                TTL_Advance = TTL_Advance + decimal.Parse(row["Advance"].ToString());
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + Fines.ToString() + "</td>";
                TTL_Fines = TTL_Fines + decimal.Parse(row["Fines"].ToString());
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + Others.ToString() + "</td>";
                TTL_Others = TTL_Others + decimal.Parse(row["Others"].ToString());
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + ded + "</td>";
                TTL_TotalDeduction = TTL_TotalDeduction + decimal.Parse(row["TotalDeduction"].ToString());
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + netpay.ToString() + "</td>";
                TTL_NetPay1 = TTL_NetPay1 + decimal.Parse(row["NetPay1"].ToString());
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + NetPayFinal.ToString() + "</td>";
                TTL_NetPayFinal = TTL_NetPayFinal + decimal.Parse(row["NetPayFinal"].ToString());
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + row["NetPay2"].ToString() + "</td>";
                TTL_NetPay2 = TTL_NetPay2 + decimal.Parse(row["NetPay2"].ToString());
                string dt = row["Date"].ToString();
                DateTime oDate = Convert.ToDateTime(dt);
                string date = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + date + "</td>";
                str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'></td></tr></table>";
                lblTotalData.Text = str;
            }
            else
            {
                // No matching record found for the given WorkmanSL
            }

        }

        private void BindTotalRow()
        {
            Int32 Multi = CalMonthDays;

            str = str + "<table width='100%' style='border-collapse:collapse; color:black;'><tr><td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>1</td>";
            str = str + "<td width='5%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>2</td>";
            str = str + "<td width='5%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>3</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>4</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>5</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>6</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>7</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>8</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>9</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>10</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>11</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>12</td>";
            str = str + "<td colspan=" + CalMonthDays + " width=" + Multi + "  style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Attendance Sheet (Unit of Workdone)</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+TTL_Present+"</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+TTL_OverTime+"</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>15</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+TTL_BasicSalary+"</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>17</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+TTL_HRAPay+"</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>19</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>20</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+TTL_WashPay+"</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>22</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>23</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+TTL_OTSalary+"</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>25</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+TTL_OthersPay+"</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+ TTL_ActualGross + "</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+ TTL_ESICGross + "</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+ TTL_ESICPay + "</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+ TTL_PFPay + "</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>31</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>32</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>33</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>34</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>35</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+ TTL_Advance + "</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+ TTL_Fines + "</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+ TTL_Others + "</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+ TTL_TotalDeduction + "</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+ TTL_NetPay1 + "</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+ TTL_NetPayFinal + "</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>"+ TTL_NetPay2 + "</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>43</td>";
            str = str + "<td width='2%' style='background-color:#2c78db; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>44</td></tr></table>";
            lblTotalData.Text = str;
        }

        private void FindAttendanceRev(string day, string month, string year, string wrk, ref Int32 dayss, string region)
        {

            string date = year + "-" + month + "-" + day;
            DataRow[] result = new DataRow[0];
            DataTable dtTemp = new DataTable();

            if ((dt_present != null) && (dt_present.Rows.Count > 0))
            {
                result = dt_present.Select("CreatedDate = #" + date + "# AND EmployeeWrk = '" + wrk + "'");

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
        }


        private static string CalculateReportTime(DateTime DT1, DateTime DT2)
        {
            int Hours = DT2.Subtract(DT1).Hours;
            int Minutes = DT2.Subtract(DT1).Minutes;
            int Seconds = DT2.Subtract(DT1).Seconds;
            return String.Format("{0} Hour(s) {1} Minute(s) {2} Second(s)", Hours, Minutes, Seconds);
        }



        protected void btn_excelexport_Click(object sender, EventArgs e)
        {
            string strt = "Form-29:";
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