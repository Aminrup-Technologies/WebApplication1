using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DocumentFormat.OpenXml.Bibliography;
using System.Drawing;

namespace WebApplication1.bussiness.production.rpts
{
    public partial class f29 : System.Web.UI.Page
    {
        DB_Utility_OH4Y DbCL = new DB_Utility_OH4Y();
        string str = string.Empty;
        Int32 CalMonthDays = 0;
        Int32 StartDay = 0;
        Int32 EndDay = 0;


        DataTable dt_emps = new DataTable();
        DataTable dt_firsthalf = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            string Year = Request.QueryString["Year"].ToString();
            string Month = Request.QueryString["Month"].ToString();
            string Region = Request.QueryString["Region"].ToString();


            Binder(Year, Month, Region);
            CheckforMonthDay(Year, Month, Region, ref CalMonthDays, ref StartDay, ref EndDay);
            BindDefaultHeaderYES(Year, Month, Region, CalMonthDays);
            BindSecondHeader(Year, Month, CalMonthDays, StartDay, EndDay);
            BindEMployeeData(Year, Month, Region, CalMonthDays, StartDay, EndDay);
        }


        private void Binder(string Year, string Month, string Region)
        {
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            string cmdString2 = "select a.FullName, b.GatePassNo, b.Fathername, FORMAT (b.DOB, 'dd-MM-yyyy') as dob, a.SkillCategory, a.SkillDesignation, FORMAT (b.DOJ, 'dd-MM-yyyy') as doj, b.ESICNo, b.UANNo, a.Present, a.OverTime, a.BasicSalary,a.OTSalary,a.OthersPay,a.HRAPay,a.ConvPay,a.WashPay, a.ActualGross,a.ESICGross, a.PFPay,a.ESICPay,a.NetPay1,a.NetPay2, a.Advance,a.Fines,a.Others,a.TotalDeduction,a.NetPayFinal, a.Date from tbl_trialpayroll a, tbl_Employee_Mustertable b where a.SalaryYear='" + Year + "' and a.SalaryMonth='" + Month + "' and a.Region='" + Region + "' and b.WorkmanSL=a.WorkmanSL order by a.Id";
            SqlCommand cmd2 = new SqlCommand(cmdString2, DbCL.Conn);
            //cmd2.CommandType = CommandType.Text;
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt_firsthalf);
            DbCL.Sqlconnection(); DbCL.ConnectDb();
        }

        private void BindDefaultHeaderYES(string Year, string Month, string Region, Int32 CalMonthDays)
        {
            Int32 Multi = CalMonthDays;
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>1</td>";
            str = str + "<td width='5%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>2</td>";
            str = str + "<td width='5%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>2A</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>3</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>4</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>5</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>6</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>7</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>7A</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>8</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>9</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>10</td>";
            str = str + "<td colspan=" + CalMonthDays + " width=" + Multi + "  style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Attendance Sheet (Unit of Workdone)</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>12</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>12A</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>13</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>14</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>15</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>16</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>17</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>18</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>19</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>20</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>21</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>22</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>23</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>24</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>24A</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>25</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>26</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>27</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>28</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>29</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>30</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>31</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>32</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>33</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>34</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>35</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>36</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>37</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>37A</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>38</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>39</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>40</td></tr>";
            //str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='font:normal 16px/16px Century Gothic; padding:5px 20px 5px 20px;' align='center'></td></tr></table>";
            lblTotalData.Text = str;
        }

        private void CheckforMonthDay(string Year, string Month, string Region, ref Int32 CalMonthDays, ref Int32 StartDay, ref Int32 EndDay)
        {
            string cmdString = "select PayrrollMonthDays,PayrollStartDay,PayrollEndDay from tbl_MonthlyPayrollStatus where PayrollYear='" + Year + "' and PayrollMonth = '" + Month + "' and PayrollRegion = '" + Region + "'";
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                CalMonthDays = Convert.ToInt32(Rdr["PayrrollMonthDays"].ToString());
                StartDay = Convert.ToInt32(Rdr["PayrollStartDay"].ToString());
                EndDay = Convert.ToInt32(Rdr["PayrollEndDay"].ToString());
            }
            DbCL.Conn.Close();
        }

        private void BindSecondHeader(string Year, string Month, Int32 CalMonthDays, Int32 StartDay, Int32 EndDay)
        {
            str = str + "<tr><td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>SL. No.</td>";
            str = str + "<td width='5%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Name of Workman</td>";
            str = str + "<td width='5%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Gatepass</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Father Name</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>SEX (M/F)</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>DOB</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Emp. No/ Sl No. in register of Employees</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Skill.</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Desig.</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>DOJ</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>ESIC IP NO.</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>PF NO.</td>";


            for (int i = StartDay; i <= EndDay; i++)
            {
                str = str + "<td width='1%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + i + "</td>";
            }

            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>No of Payable days /Total Work done</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>OT</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Name of N&FH for Which Wages have been Paid</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Basic Wages</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>D.A/ VDA</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>HRA</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Conv. Allowance</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Med.Allow</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Wash. Allowance </td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>ATT/Allow. Bonous</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Spl. Allowance</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>OT Wages</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Misc. Earnings</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Others</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Actual Gross</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>ESIC Gross</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>ESI Pay</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>PF Pay</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Socy.</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Insurance</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Sal. Adv</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>PT</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>TDS</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Advance</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Fine</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Others</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Total Ded.</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Net Payable 1</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Net Payable 1 Final</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Net Payable 2</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Date Of Payment</td>";
            str = str + "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Signature /Thumb Impression</td></tr>";

            lblTotalData.Text = str;
            //str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='font:normal 16px/16px Century Gothic; padding:5px 20px 5px 20px;' align='center'></td></tr></table>";
        }

        private void BindEMployeeData(string Year, string Month, string Region, Int32 CalMonthDays, Int32 StartDay, Int32 EndDay)
        {
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            string cmdString = "select top(10) WorkmanSL from tbl_trialpayroll where SalaryYear='" + Year + "' and SalaryMonth='" + Month + "' and Region='" + Region + "' order by Id";      
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);     
            cmd.CommandType = CommandType.Text;    
            int Sl = 1;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt_emps);
            DbCL.Sqlconnection(); DbCL.ConnectDb();

            if ((dt_emps != null) && (dt_emps.Rows.Count > 0))
            {
                string EmpWrk = string.Empty;

                for (int i = 0; i < dt_emps.Rows.Count; i++)
                {
                    EmpWrk = dt_emps.Rows[i][0].ToString();
                    str = str + "<tr><td height='70' width='3%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + Sl + "</td>";
                    //FindFirstHalfData(Year, Month, Region, CalMonthDays, StartDay, EndDay, EmpWrk);
                    Sl = Sl + 1;
                    lblTotalData.Text = str;
                }
            }

            //using (SqlDataReader re = cmd.ExecuteReader())
            //{
            //    while (re.Read())
            //    {
            //        string workman = re["WorkmanSL"].ToString();
            //        //string workman = "K1";
            //        str = str + "<tr><td height='70' width='3%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + Sl + "</td>";
            //        FindFirstHalfData(Year, Month, Region, CalMonthDays, StartDay, EndDay, workman);
            //        Sl = Sl + 1;
            //        lblTotalData.Text = str;
            //    }
            //}
        }

        private void FindFirstHalfData(string Year, string Month, string Region, Int32 CalMonthDays, Int32 StartDay, Int32 EndDay, string workman)
        {
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            string cmdString = "select a.FullName, b.GatePassNo, b.Fathername, FORMAT (b.DOB, 'dd-MM-yyyy') as dob, a.SkillCategory, a.SkillDesignation, FORMAT (b.DOJ, 'dd-MM-yyyy') as doj, b.ESICNo, b.UANNo, a.Present, a.OverTime, a.BasicSalary,a.OTSalary,a.OthersPay,a.HRAPay,a.ConvPay,a.WashPay, a.ActualGross,a.ESICGross, a.PFPay,a.ESICPay,a.NetPay1,a.NetPay2, a.Advance,a.Fines,a.Others,a.TotalDeduction,a.NetPayFinal, a.Date from tbl_trialpayroll a, tbl_Employee_Mustertable b where a.SalaryYear='" + Year + "' and a.SalaryMonth='" + Month + "' and a.Region='" + Region + "' and a.WorkmanSL='" + workman + "' and a.WorkmanSL=b.WorkmanSL";
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            cmd.CommandType = CommandType.Text;
            using (SqlDataReader re = cmd.ExecuteReader())
            {
                while (re.Read())
                {
                    str = str + "<td width='5%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + re["FullName"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + re["GatePassNo"].ToString() + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + re["Fathername"].ToString() + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>Male</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + re["dob"].ToString() + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + workman + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + re["SkillCategory"].ToString() + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + re["SkillDesignation"].ToString() + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + re["doj"].ToString() + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>'" + re["ESICNo"].ToString() + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>'" + re["UANNo"].ToString() + "</td>";


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
                        FindAttendance(days, Month, Year, workman, ref daycount, Region);
                        day = day + 1;
                    }
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Present"].ToString() + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["OverTime"].ToString() + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";

                    decimal basicsal = Convert.ToDecimal(re["BasicSalary"].ToString());
                    decimal basicotpay = Convert.ToDecimal(re["OTSalary"].ToString());
                    decimal otherspay = Convert.ToDecimal(re["OthersPay"].ToString());

                    decimal pf = Convert.ToDecimal(re["PFPay"].ToString());
                    decimal esi = Convert.ToDecimal(re["ESICPay"].ToString());
                    decimal netpay = Convert.ToDecimal(re["NetPay1"].ToString());

                    decimal Advance = Convert.ToDecimal(re["Advance"].ToString());
                    decimal Fines = Convert.ToDecimal(re["Fines"].ToString());
                    decimal Others = Convert.ToDecimal(re["Others"].ToString());
                    decimal ded = Convert.ToDecimal(re["TotalDeduction"].ToString());
                    decimal NetPayFinal = Convert.ToDecimal(re["NetPayFinal"].ToString());

                    //decimal ded = Convert.ToDecimal(re["ttl_deductions"].ToString());
                    //decimal ded = .0m;

                    decimal gross1 = basicsal + basicotpay + otherspay;
                    decimal gross2 = netpay + esi + pf + ded;

                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + basicsal + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["HRAPay"].ToString() + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["ConvPay"].ToString() + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["WashPay"].ToString() + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + basicotpay + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["OthersPay"].ToString() + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["ActualGross"].ToString() + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["ESICGross"].ToString() + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + esi + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + pf + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>0.00</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>-</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>"+Advance.ToString()+"</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>"+Fines.ToString()+"</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>"+Others.ToString()+"</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + ded + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + netpay.ToString() + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + NetPayFinal.ToString() + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["NetPay2"].ToString() + "</td>";
                    string dt = re["Date"].ToString();
                    DateTime oDate = Convert.ToDateTime(dt);
                    string date = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + date + "</td>";
                    str = str + "<td width='2%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'></td>";
                    lblTotalData.Text = str;


                }
            }
        }

        private void FindAttendance(string day, string month, string year, string wrk, ref Int32 dayss, string region)
        {

            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            string cmdstring1 = "select max(AttendanceStatus) as AttendanceStatus, AttendanceCode, COALESCE(SUM(ProvidedOT),0) as OT from tbl_attendance where MONTH(CreatedDate)='" + month + "' and DAY(CreatedDate)='" + day + "' and YEAR(CreatedDate)='" + year + "' and EmployeeWrk='" + wrk + "' and SiteIncharge_Approval='Approved' group by AttendanceCode";
            SqlCommand cmd1 = new SqlCommand(cmdstring1, DbCL.Conn);
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
                                str = str + "<td width='1%' style='background-color:#d3ffcf; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + status + "</td>";
                                break;

                            case "NH":
                                dayss = dayss + 1;
                                str = str + "<td width='1%' style='background-color:#cff5ff; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + status + "</td>";
                                break;

                            case "FL":
                                dayss = dayss + 1;
                                str = str + "<td width='1%' style='background-color:#cff5ff; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + status + "</td>";
                                break;

                            case "OD":
                                str = str + "<td width='1%' style='background-color:#ff5f5f; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + status + "</td>";
                                break;

                            default:
                                str = str + "<td width='1%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>" + status + "</td>";
                                break;
                        }
                    }
                    else
                    {
                        str = str + "<td width='1%' style='background-color:white; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>A</td>";
                    }
                }
            }
            else
            {
                str = str + "<td width='1%' style='background-color:white; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>A</td>";
            }
            DbCL.Conn.Close();
        }

        protected void btn_export_Click(object sender, EventArgs e)
        {
            string strt = "F29";
            string regn = Session["REGION"].ToString();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + strt + "_" + regn + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.Output.Write(Request.Form[hfGridHtml.UniqueID]);
            Response.Flush();
            Response.End();
        }
    }
}