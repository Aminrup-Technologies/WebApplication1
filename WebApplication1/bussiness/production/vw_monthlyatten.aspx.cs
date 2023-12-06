using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Diagnostics;

namespace WebApplication1.bussiness.production
{
    public partial class vw_monthlyatten : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        Payroll_OH4Y PayRoll = new Payroll_OH4Y();

        public static decimal GorssBreaker_KPO = 18500;
        public static decimal GorssBreaker_AGL = 18500;

        public static decimal GorssBreaker = 18500;
        public static decimal Allowances_Allow = 12000;

        public static Decimal Allow_Multi = .05m; //5%
        public static decimal DaVdaPay = .0m;
        public static decimal HRAPay = .0m;
        public static decimal ConvPay = .0m;
        public static decimal MedPay = .0m;
        public static decimal WashPay = .0m;
        public static decimal AttPay = .0m;
        public static decimal SPCLPay = .0m;
        public static decimal MiscPay = .0m;


        public static decimal fxddeductions = .0m;

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

                    CurrentDataBinder();
                }
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

        protected void btn_prevmonth_Click(object sender, EventArgs e)
        {
            Int32 year = Convert.ToInt32(lbl_year.Text.ToString());
            Int32 month = Convert.ToInt32(lbl_monthcode.Text.ToString());

            if (month == 01 || month == 1)
            {
                month = 12;
                year = year - 1;
            }
            else
            {
                month = month - 1;
            }

            string Year = Convert.ToString(year);
            string Month = "";
            if (month <= 9)
            {
                Month = "0" + month.ToString();
            }
            else
            {
                Month = month.ToString();
            }

            GridBinder(Year, Month);
            PaymentDadaBinder(Year, Month);
        }

        protected void btn_currentdata_Click(object sender, EventArgs e)
        {
            finalized.Visible = false;
            realtime.Visible = true;
            CurrentDataBinder();
        }

        private void CurrentDataBinder()
        {
            string Year = DateTime.Now.Year.ToString();
            string Month = DateTime.Now.ToString("MM");
            string MonthName = DateTime.Now.ToString("MMMM");

            finalized.Visible = false;
            realtime.Visible = true;
            GridBinderReal(Year, Month);
        }

        protected void btn_nextmonth_Click(object sender, EventArgs e)
        {
            Int32 year = Convert.ToInt32(lbl_year.Text.ToString());
            Int32 month = Convert.ToInt32(lbl_monthcode.Text.ToString());

            if (month == 12)
            {
                month = 1;
                year = year + 1;
            }
            else
            {
                month = month + 1;
            }

            string Year = Convert.ToString(year);
            string Month = "";
            if (month <= 9)
            {
                Month = "0" + month.ToString();
            }
            else
            {
                Month = month.ToString();
            }

            GridBinder(Year, Month);
            PaymentDadaBinder(Year, Month);
        }

        private void GridBinder(string Year, string Month)
        {
            string Monthname = "";
            dbcl.FindMonthName(Month, ref Monthname);
            lbl_month.Text = Monthname;
            lbl_year.Text = Year;
            lbl_monthcode.Text = Month;

            string query = "select * from tbl_attendance where YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and EmployeeWrk = '" + Session["WORKMAN"].ToString() + "' and SubmitterStatus='Exit' and SiteIncharge_Approval='Approved' and AttendanceStatus='Present' order by CreatedDate";
            BindGrid(query);

            RegularAttendanceDataBinder(Year, Month, Monthname);
        }

        private void GridBinderReal(string Year, string Month)
        {
            string Monthname = "";
            dbcl.FindMonthName(Month, ref Monthname);
            lbl_month.Text = Monthname;
            lbl_year.Text = Year;
            lbl_monthcode.Text = Month;

            string query = "select * from tbl_attendance where YEAR(CreatedDate)='" + Year + "' and MONTH(CreatedDate)='" + Month + "' and EmployeeWrk = '" + Session["WORKMAN"].ToString() + "' and SubmitterStatus='Exit' order by CreatedDate";
            BindGrid(query);

            RegularAttendanceDataBinder(Year, Month, Monthname);
        }


        private void RegularAttendanceDataBinder(string Year, string Month, string Monthname)
        {

            Int32 int_month = Convert.ToInt32(Month);

            lbl_calmonth.Text = lbl_paymonth.Text = Monthname;

            Int32 int_year = Convert.ToInt32(Year);
            lbl_calyear.Text = lbl_payyear.Text = int_year.ToString();

            string empwrk = Session["WORKMAN"].ToString().Trim();
            Int32 caldays = DateTime.DaysInMonth(int_year, int_month);
            lbl_caldays.Text = caldays.ToString();

            Int32 ttl_days = 0;
            PayRoll.FindEmployeeTotalDaysByMonth(Month, Year, empwrk, ref ttl_days);
            lbl_totalpresent.Text = ttl_days.ToString();
            lbl_dayswrkd.Text = ttl_days.ToString();

            Int32 ttl_p = 0;
            PayRoll.FindEmployeeTotalPresentByMonth(Month, Year, empwrk, ref ttl_p);
            lbl_presentdayscount.Text = ttl_p.ToString();

            Int32 ttl_od = 0;
            PayRoll.FindEmployeeTotalODByMonth(Month, Year, empwrk, ref ttl_od);
            lbl_oddayscount.Text = ttl_od.ToString();

            Int32 ttl_nh = 0;
            PayRoll.FindEmployeeTotalNHByMonth(Month, Year, empwrk, ref ttl_nh);
            lbl_nhcount.Text = ttl_nh.ToString();

            Int32 ttl_fl = 0;
            PayRoll.FindEmployeeTotalFLByMonth(Month, Year, empwrk, ref ttl_fl);
            lbl_flcount.Text = ttl_fl.ToString();

            decimal ttl_ot = .0m;
            PayRoll.FindEmployeeTotalOTByMonth(Month, Year, empwrk, ref ttl_ot);
            lbl_totalot.Text = ttl_ot.ToString();
        }


        static int NumberOfParticularDaysInMonth(int year, int month, DayOfWeek dayOfWeek)
        {
            DateTime startDate = new DateTime(year, month, 1);
            int totalDays = startDate.AddMonths(1).Subtract(startDate).Days;

            int answer = Enumerable.Range(1, totalDays)
                .Select(item => new DateTime(year, month, item))
                .Where(date => date.DayOfWeek == dayOfWeek)
                .Count();

            return totalDays-answer;
        }




        //The below function is used to bind the attendance data in real time from DB Attendance Table, it is not fetching data from the payment table where----------KPO
        private void Dynamic_AttendanceDataBinder(string Year, string Month, string Monthname)
        {
            string WR = Session["REGION"].ToString(); //_________________ EMP_01 : Work Region [WR]___________01__________//

            fxddeductions = 0;

            //Inputs taken here are Calender Year, Month, and Monthname
            realtime.Visible = true;
            finalized.Visible = false;

            Int32 CM = Convert.ToInt32(Month);//_________________________ DBM_01 : Calender Month [CM]___________02____________//

            lbl_calmonth.Text = lbl_paymonth.Text = Monthname;

            Int32 CY = Convert.ToInt32(Year);//___________________________ DBM_02 : Calender Year[CY]______________03_____________//
            lbl_calyear.Text = lbl_payyear.Text = CY.ToString();


            //----------------- Function call to find out the Calender Working Days ------DBM_03. Calender Working Days [CWD]---------04----//
            Int32 CWD = 0;
            PayRoll.FindCalWorkDays(Year, Month, WR, ref CWD);
            if (CWD == 0)
            {
                int numberOfSundays = NumberOfParticularDaysInMonth(Convert.ToInt32(Year), Convert.ToInt32(Month), DayOfWeek.Sunday);
                CWD = numberOfSundays;
            }


            string EWRK = Session["WORKMAN"].ToString().Trim();//___________________EMP_02 : Employee Workmen SL [WRK] _____05______//
            string ESKILL = Session["SKIL"].ToString().Trim();//____________________EMP_03 : EMployee Skill [ESKILL]_______06_______//


            //----------------- Function call to find out the Daily Pay Rate aganist the Employee Skill Category----------------//
            decimal DSR = 0.0m;   //______________________DBM_03 : Daily Skill Rate [DSR] _______________07______________//
            PayRoll.FindPayCadre(ESKILL, WR, ref DSR);

            //----------------- Function call to find out Employee Payroll Factors in BULK ------------------//
            string FixedSalary_YesNo = "";
            string OTMultiplier = "";

            Int32 WorkHours = 0;
            Int32 OTFactor = 0;
            Int32 OT_Divisibility = 0;

            decimal FixedAmount = .0m;
            decimal DA_VDA = .0m;
            decimal DB_HRA = .0m;
            decimal Conv_Allowance = .0m;
            decimal Medical_Allowance = .0m;
            decimal Washing_Allowance = .0m;
            decimal ATT_Allowance = .0m;
            decimal SPCL_Allowance = .0m;
            decimal Misc_Earnings = .0m;

            int Advance = 0;
            int Fines = 0;
            int Others = 0;

            PayRoll.EmployeePayrollFactors(EWRK, ref FixedSalary_YesNo, ref FixedAmount, ref WorkHours, ref OTFactor, ref OTMultiplier, ref DA_VDA, ref DB_HRA, ref Conv_Allowance, ref Medical_Allowance, ref Washing_Allowance, ref ATT_Allowance, ref SPCL_Allowance, ref Misc_Earnings, ref OT_Divisibility, ref Advance, ref Fines, ref Others);

            Int32 CD = DateTime.DaysInMonth(CY, CM);
            lbl_caldays.Text = CD.ToString();

            Int32 ttl_days = 0;
            PayRoll.FindEmployeeTotalDaysByMonth(Month, Year, EWRK, ref ttl_days);
            lbl_totalpresent.Text = ttl_days.ToString();
            lbl_dayswrkd.Text = ttl_days.ToString();

            Int32 ttl_p = 0;  //---------------------- Total Present [P]--------------08-----------//
            PayRoll.FindEmployeeTotalPresentByMonth(Month, Year, EWRK, ref ttl_p);
            lbl_presentdayscount.Text = ttl_p.ToString();

            Int32 ttl_od = 0;
            PayRoll.FindEmployeeTotalODByMonth(Month, Year, EWRK, ref ttl_od);
            lbl_oddayscount.Text = ttl_od.ToString();

            Int32 ttl_nh = 0;
            PayRoll.FindEmployeeTotalNHByMonth(Month, Year, EWRK, ref ttl_nh);
            lbl_nhcount.Text = ttl_nh.ToString();

            Int32 ttl_fl = 0;
            PayRoll.FindEmployeeTotalFLByMonth(Month, Year, EWRK, ref ttl_fl);
            lbl_flcount.Text = ttl_fl.ToString();

            decimal ttl_ot = .0m;   //---------------------- Total Over Time [OT]--------------09-----------//
            PayRoll.FindEmployeeTotalOTByMonth(Month, Year, EWRK, ref ttl_ot);
            lbl_totalot.Text = ttl_ot.ToString();

            //Here goes the code for real time salary calculations
            //this will shows real time but not ACTUAL DATA

            //-------------Basic Salary or Basic Wages  -----------   Daily PayRate x Present Days
            decimal BW = 0.0m;
            PayRoll.BasicSalaryCalculation(ttl_p, DSR, ref BW);   //---------------------- Basic Wages [BW]-----------------[P1]--------//


            decimal _temp_1 = .0m;
            decimal WOFR = .0m;
            if (FixedSalary_YesNo == "Yes")
            {
                _temp_1 = Math.Round(FixedAmount / CWD, 2);
                WOFR = Math.Ceiling(Math.Round(_temp_1 * ttl_p, 0));  //---------------------- Wage of Fixed Rate [WOFR]-------------[P2]------------//
            }
            else
            {
                WOFR = .0m;
            }

            //--------------- PF Calucations -----------[P10]---------//
            decimal PFPay = 0.0m;
            PayRoll.PFPayCalculation(BW, ref PFPay);

            decimal otpay = 0.0m;   //---------------- OT Pay -------- [P4]--------------------//
            decimal GrossTotal = 0.0m;

            decimal otherspay = .0m;

            if (WR == "KPO")
            {
                if (WOFR > Allowances_Allow)  //---------------------Allowances [P3]-------------------//
                {
                    DaVdaPay = .0m;
                    HRAPay = WOFR * Allow_Multi;
                    ConvPay = WOFR * Allow_Multi;
                    MedPay = .0m;
                    WashPay = Math.Round(BW * Allow_Multi, 0);
                    MedPay = .0m;
                    AttPay = .0m;
                    SPCLPay = .0m;
                    MiscPay = .0m;
                }

                if (FixedSalary_YesNo == "Yes")  ///Check whether the employee is in Fixed or Daily Rate Payroll
                {
                    if (OTMultiplier == "Gross")
                    {
                        if (ttl_ot >= 1)
                        {
                            decimal otdays = ttl_ot / OT_Divisibility;
                            otpay = Math.Round(_temp_1 * otdays * OTFactor, 0);
                        }
                        else
                        {
                            otpay = .0m;
                        }
                    }
                    else
                    {
                        if (ttl_ot >= 1)
                        {
                            decimal otdays = ttl_ot / OT_Divisibility;
                            otpay = Math.Round(DSR * otdays * OTFactor, 0);
                        }
                        else
                        {
                            otpay = .0m;
                        }
                    }
                }
                else
                {
                    if (OTMultiplier == "Gross")
                    {
                        if (ttl_ot >= 1)
                        {
                            decimal otdays = ttl_ot / OT_Divisibility;
                            otpay = Math.Round(_temp_1 * otdays * OTFactor, 0);
                        }
                        else
                        {
                            otpay = .0m;
                        }
                    }
                    else
                    {
                        if (ttl_ot >= 1)
                        {
                            decimal otdays = ttl_ot / OT_Divisibility;
                            otpay = Math.Round(DSR * otdays * OTFactor, 0);
                        }
                        else
                        {
                            otpay = .0m;
                        }
                    }
                }

                decimal temp5a = .0m;
                decimal temp5b = .0m;
                otherspay = .0m;

                temp5a = BW + DaVdaPay + HRAPay + ConvPay + MedPay + AttPay + SPCLPay + MiscPay + WashPay;
                otherspay = WOFR - temp5a;

                temp5b = BW + DaVdaPay + HRAPay + ConvPay + MedPay + AttPay + SPCLPay + MiscPay + WashPay + otpay;
                GrossTotal = otherspay + temp5b;
            }

            //----------------Gross Calculation & NET Payment 2 -------------------------------//
            decimal ESIC_Gross = 0.0m;

            decimal GrossAmount = GrossTotal - (ConvPay + WashPay + fxddeductions);  // advance amount also needs to be added inorder to deduct the same
            decimal washgross = GrossTotal + WashPay + ConvPay;
            decimal esicpay = 0.0m;
            decimal netpay1 = 0.0m;
            decimal netpay2 = 0.0m;
            decimal netpay2final = .0m;
            decimal ttldeductions = .0m;


            if (WR == "KPO")
            {
                if (GrossAmount > GorssBreaker_KPO)
                {
                    ESIC_Gross = GorssBreaker_KPO;
                    netpay2 = GrossAmount - GorssBreaker_KPO;
                }
                else
                {
                    ESIC_Gross = GrossAmount;
                }
                esicpay = Math.Round(ESIC_Gross * 0.0075m, 0); //--------------------- ESIC pay---------------------------------------------------//

                //--------------------- NET Payment 1 -------------------------------------------------//
                netpay1 = Math.Round(ESIC_Gross - (PFPay + esicpay) + WashPay + ConvPay, 0);

                ttldeductions = Advance + Fines + Others;
                netpay2final = netpay2;   //--------------------- NET Payment 2 -------------------------------------------------//
            }

            lbl_grosspay.Text = ESIC_Gross.ToString();
            lbl_basic.Text = BW.ToString();
            lbl_otpay.Text = otpay.ToString();
            lbl_netpay.Text = netpay1.ToString();
            decimal ttlded = PFPay + esicpay;
            lbl_ttldeductions.Text = ttlded.ToString();
            lbl_pfpay.Text = PFPay.ToString();
            lbl_esicpay.Text = esicpay.ToString();
            lbl_allowances.Text = otherspay.ToString();
            lbl_advance.Text = ttldeductions.ToString();
            lbl_netpay2.Text = netpay2final.ToString();
        }


        //The below function is used to bind the attendance data in real time from DB Attendance Table, it is not fetching data from the payment table where----------AGL
        private void AttendanceDataBinder(string Year, string Month, string Monthname)
        {
            realtime.Visible = true;
            finalized.Visible = false;
            Int32 int_month = Convert.ToInt32(Month);

            lbl_calmonth.Text = lbl_paymonth.Text = Monthname;

            Int32 int_year = Convert.ToInt32(Year);
            lbl_calyear.Text = lbl_payyear.Text = int_year.ToString();

            string empwrk = Session["WORKMAN"].ToString().Trim();
            string empskill = Session["SKIL"].ToString().Trim();
            string empregion = Session["REGION"].ToString();

            //----------------- Function call to find out the Daily Pay Rate aganist the Employee Skill Category----------------//
            decimal dailyrate = 0.0m;
            PayRoll.FindPayCadre(empskill, empregion, ref dailyrate);

            //----------------- Function call to find out the Calender Working Days --------------------------------------------//
            Int32 calwrkdays = 0;
            PayRoll.FindCalWorkDays(Year, Month, empregion, ref calwrkdays);
            if (calwrkdays == 0)
            {
                int numberOfSundays = NumberOfParticularDaysInMonth(Convert.ToInt32(Year), Convert.ToInt32(Month), DayOfWeek.Sunday);
                calwrkdays = numberOfSundays;
            }

            //----------------- Function call to find out Employee Payroll Factors in BULK ------------------//
            string FixedSalary_YesNo = "";
            decimal FixedAmount = .0m;
            Int32 WorkHours = 0;
            Int32 OTFactor = 0;
            string OTMultiplier = "";
            decimal DA_VDA = .0m;
            decimal HRA = .0m;
            decimal Conv_Allowance = .0m;
            decimal Medical_Allowance = .0m;
            decimal Washing_Allowance = .0m;
            decimal ATT_Allowance = .0m;
            decimal SPCL_Allowance = .0m;
            decimal Misc_Earnings = .0m;
            Int32 OT_Divisibility = 0;
            int Advance = 0;
            int Fines = 0;
            int Others = 0;

            PayRoll.EmployeePayrollFactors(empwrk, ref FixedSalary_YesNo, ref FixedAmount, ref WorkHours, ref OTFactor, ref OTMultiplier, ref DA_VDA, ref HRA, ref Conv_Allowance, ref Medical_Allowance, ref Washing_Allowance, ref ATT_Allowance, ref SPCL_Allowance, ref Misc_Earnings, ref OT_Divisibility, ref Advance, ref Fines, ref Others);

            Int32 caldays = DateTime.DaysInMonth(int_year, int_month);
            lbl_caldays.Text = caldays.ToString();

            Int32 ttl_days = 0;
            PayRoll.FindEmployeeTotalDaysByMonth(Month, Year, empwrk, ref ttl_days);
            lbl_totalpresent.Text = ttl_days.ToString();
            lbl_dayswrkd.Text = ttl_days.ToString();

            Int32 ttl_p = 0;
            PayRoll.FindEmployeeTotalPresentByMonth(Month, Year, empwrk, ref ttl_p);
            lbl_presentdayscount.Text = ttl_p.ToString();

            Int32 ttl_od = 0;
            PayRoll.FindEmployeeTotalODByMonth(Month, Year, empwrk, ref ttl_od);
            lbl_oddayscount.Text = ttl_od.ToString();

            Int32 ttl_nh = 0;
            PayRoll.FindEmployeeTotalNHByMonth(Month, Year, empwrk, ref ttl_nh);
            lbl_nhcount.Text = ttl_nh.ToString();

            Int32 ttl_fl = 0;
            PayRoll.FindEmployeeTotalFLByMonth(Month, Year, empwrk, ref ttl_fl);
            lbl_flcount.Text = ttl_fl.ToString();

            decimal ttl_ot = .0m;
            PayRoll.FindEmployeeTotalOTByMonth(Month, Year, empwrk, ref ttl_ot);
            lbl_totalot.Text = ttl_ot.ToString();

            //Here goes the code for real time salary calculations
            //this will shows real time but not ACTUAL DATA

            //-------------Basic Salary or Basic Wages  -----------   Daily PayRate x Present Days
            decimal BasicSalary = 0.0m;
            PayRoll.BasicSalaryCalculation(ttl_p, dailyrate, ref BasicSalary);


            decimal fxdrt = .0m;
            decimal fnlfdr = .0m;
            decimal FixRateSalary = .0m;
            if (FixedSalary_YesNo == "Yes")
            {
                //----------------- Calculation for Employee who are in Fixed Salary----------------//
                fxdrt = Math.Round(FixedAmount / calwrkdays, 2);
                fnlfdr = Math.Ceiling(Math.Round(fxdrt, 0));

                //------------ Wage of Fixed rate ---------------   ( FixedAmount / CalenderDays ) x  PresentDays
                FixRateSalary = Math.Ceiling(Math.Round(fxdrt * ttl_p, 0));
            }

            decimal DaVdaPay = .0m;
            decimal HRAPay = .0m;
            decimal ConvPay = .0m;
            decimal MedPay = .0m;
            decimal WashPay = .0m;
            decimal AttPay = .0m;
            decimal SPCLPay = .0m;
            decimal MiscPay = .0m;

            //---------------- DA/VDA Alowances Cal-------------------------------------//
            PayRoll.EmployeeOthersPayCalculations1(ttl_p, calwrkdays, DA_VDA, ref DaVdaPay);

            //---------------- HRA Alowances Cal-------------------------------------//
            PayRoll.EmployeeOthersPayCalculations2(ttl_p, calwrkdays, HRA, ref HRAPay);

            //---------------- Conv Alowances Cal-------------------------------------//
            PayRoll.EmployeeOthersPayCalculations3(ttl_p, calwrkdays, Conv_Allowance, ref ConvPay);

            //---------------- Medical Alowances Cal-------------------------------------//
            PayRoll.EmployeeOthersPayCalculations4(ttl_p, calwrkdays, Medical_Allowance, ref MedPay);

            //---------------- Wash Alowances Cal-------------------------------------//
            PayRoll.EmployeeOthersPayCalculations5(ttl_p, calwrkdays, Washing_Allowance, ref WashPay);

            //---------------- Att Alowances Cal-------------------------------------//
            PayRoll.EmployeeOthersPayCalculations6(ttl_p, calwrkdays, ATT_Allowance, ref AttPay);

            //---------------- SPCL Alowances Cal-------------------------------------//
            PayRoll.EmployeeOthersPayCalculations7(ttl_p, calwrkdays, SPCL_Allowance, ref SPCLPay);

            //---------------- MISC Alowances Cal-------------------------------------//
            PayRoll.EmployeeOthersPayCalculations8(ttl_p, calwrkdays, Misc_Earnings, ref MiscPay);

            //--------------- PF Calucations --------------------//
            decimal PFPay = 0.0m;
            PayRoll.PFPayCalculation(BasicSalary, ref PFPay);

            //---------------- OT Pay -------- Gross Rate
            decimal otpay = 0.0m;
            decimal actualgross = 0.0m;

            if (FixedSalary_YesNo == "Yes")  ///Check whether the employee is in Fixed or Daily Rate Payroll
            {
                if (OTMultiplier == "Gross")
                {
                    if (ttl_ot >= 1)
                    {
                        decimal otdays = ttl_ot / OT_Divisibility;
                        // OTPay on FixedRate
                        otpay = Math.Round(fnlfdr * otdays * OTFactor, 0);
                    }
                    else
                    {
                    }

                    //actualgross = FixRateSalary + otpay + DaVdaPay + HRAPay + ConvPay + MedPay + AttPay + SPCLPay + MiscPay;
                    //actualgross = FixRateSalary + otpay + DaVdaPay + HRAPay + MedPay + AttPay + SPCLPay + MiscPay; ---------- Commented on 01.05.2022
                    actualgross = FixRateSalary + otpay + DaVdaPay + MedPay + AttPay + SPCLPay + MiscPay;
                }
                else
                {
                    if (ttl_ot >= 1)
                    {
                        decimal otdays = ttl_ot / OT_Divisibility;
                        //OTPay on DailyRate
                        otpay = Math.Round(dailyrate * otdays * OTFactor, 0);
                    }
                    else
                    {

                    }
                    //actualgross = FixRateSalary + otpay + DaVdaPay + HRAPay + ConvPay + MedPay + AttPay + SPCLPay + MiscPay;
                    //actualgross = FixRateSalary + otpay + DaVdaPay + HRAPay + MedPay + AttPay + SPCLPay + MiscPay;
                    actualgross = FixRateSalary + otpay + DaVdaPay + MedPay + AttPay + SPCLPay + MiscPay;
                }
            }
            else
            {
                if (OTMultiplier == "Gross")
                {
                    if (ttl_ot >= 1)
                    {
                        decimal otdays = ttl_ot / OT_Divisibility;
                        // OTPay on FixedRate
                        otpay = Math.Round(fnlfdr * otdays * OTFactor, 0);
                    }
                    else
                    {

                    }

                    //actualgross = FixRateSalary + otpay + DaVdaPay + HRAPay + ConvPay + MedPay + AttPay + SPCLPay + MiscPay;
                    //actualgross = FixRateSalary + otpay + DaVdaPay + HRAPay + MedPay + AttPay + SPCLPay + MiscPay;
                    actualgross = FixRateSalary + otpay + DaVdaPay + MedPay + AttPay + SPCLPay + MiscPay;
                }
                else
                {
                    if (ttl_ot >= 1)
                    {
                        decimal otdays = ttl_ot / OT_Divisibility;
                        //OTPay on DailyRate
                        otpay = Math.Round(dailyrate * otdays * OTFactor, 0);
                    }
                    else
                    {

                    }
                    //actualgross = BasicSalary + otpay + DaVdaPay + HRAPay + ConvPay + MedPay + AttPay + SPCLPay + MiscPay;
                    //actualgross = BasicSalary + otpay + DaVdaPay + HRAPay + MedPay + AttPay + SPCLPay + MiscPay;
                    actualgross = BasicSalary + otpay + DaVdaPay + MedPay + AttPay + SPCLPay + MiscPay;
                }
            }
            //----------------Gross Calculation & NET Payment 2 -------------------------------//
            decimal grossesic = 0.0m;
            decimal washgross = actualgross + WashPay + ConvPay;
            if (washgross > GorssBreaker)
            {
                decimal minus = WashPay + ConvPay;
                //grossesic = GorssBreaker-WashPay;
                grossesic = GorssBreaker - minus;
            }
            else
            {
                //grossesic = washgross - WashPay;
                decimal minus = WashPay + ConvPay;
                grossesic = washgross - minus;
            }
            decimal otherpay = grossesic - BasicSalary;

            //--------------------- ESIC pay---------------------------------------------------//

            decimal esicpay = 0.0m;
            esicpay = Math.Round(grossesic * 0.0075m, 0);


            //--------------------- NET Payment -------------------------------------------------//
            decimal netpay1 = 0.0m;
            decimal newgross = grossesic;
            netpay1 = Math.Round(newgross - PFPay - esicpay + WashPay + ConvPay, 0);

            decimal ttldeductions = Advance + Fines + Others;

            decimal netpayfinal = netpay1 - ttldeductions;


            decimal netpay2 = 0.0m;
            if (FixedSalary_YesNo == "Yes")
            {
                netpay2 = actualgross - newgross + HRAPay;
            }
            else
            {
                decimal p = netpay1;
                netpay2 = actualgross - p - PFPay - esicpay + HRAPay;
            }
            decimal totalgross = BasicSalary + (grossesic - BasicSalary) + (WashPay + ConvPay);
            lbl_grosspay.Text = totalgross.ToString();
            lbl_basic.Text = BasicSalary.ToString();
            lbl_otpay.Text = otpay.ToString();
            lbl_netpay.Text = netpayfinal.ToString();
            decimal earning = BasicSalary + otpay;
            decimal ttlded = PFPay + esicpay + Advance + Fines + Others;
            lbl_ttldeductions.Text = ttlded.ToString();
            lbl_pfpay.Text = PFPay.ToString();
            lbl_esicpay.Text = esicpay.ToString();
            otherpay = grossesic - earning - ttlded;
            lbl_allowances.Text = otherpay.ToString();
            lbl_advance.Text = ttldeductions.ToString();
            lbl_netpay2.Text = netpay2.ToString();
        }

        private void PaymentDadaBinder(string Year, string Month)
        {
            Clear();

            if (DateTime.Now.Year.ToString() == Year && DateTime.Now.Month.ToString("MM") == Month)
            {
                finalized.Visible = false;
                realtime.Visible = true;
            }
            else
            {
                finalized.Visible = true;
                realtime.Visible = false;
                salary_pdfrow.Visible = true;
            }
            string cmdString = "select * from tbl_trialpayroll where SalaryMonth = '" + Month + "' AND SalaryYear = '" + Year + "' and WorkmanSL='" + Session["WORKMAN"].ToString() + "'";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                Int32 esicgross = Convert.ToInt32(Rdr["ESICGross"].ToString());
                lbl_grosspay.Text = esicgross.ToString();

                Int32 basicpay = Convert.ToInt32(Rdr["BasicSalary"].ToString());
                lbl_basic.Text = basicpay.ToString();

                Int32 otpay = Convert.ToInt32(Rdr["OTSalary"].ToString());
                lbl_otpay.Text = otpay.ToString();

                Int32 netpay = Convert.ToInt32(Rdr["NetPayFinal"].ToString());
                lbl_netpay.Text = netpay.ToString();

                if (netpay > 1 && basicpay > 1)
                {
                    finalized.Visible = true;
                    salary_pdfrow.Visible = true;
                }
                else
                {
                    finalized.Visible = false;
                    salary_pdfrow.Visible = false;
                    realtime.Visible = false;
                }


                Int32 pfpay = Convert.ToInt32(Rdr["PFPay"].ToString());
                Int32 esicpay = Convert.ToInt32(Rdr["ESICPay"].ToString());
                Int32 ded = Convert.ToInt32(Rdr["TotalDeduction"].ToString());

                Int32 earning = basicpay + otpay;
                Int32 ttlded = pfpay + esicpay + ded;

                lbl_ttldeductions.Text = ttlded.ToString();

                lbl_pfpay.Text = pfpay.ToString();
                lbl_esicpay.Text = esicpay.ToString();

                Int32 otherpay = Convert.ToInt32(Rdr["OthersPay"].ToString());
                //Int32 otherpay = esicgross - earning - ttlded;
                lbl_allowances.Text = otherpay.ToString();

                lbl_advance.Text = Rdr["TotalDeduction"].ToString();

                lbl_netpay2.Text = Rdr["NetPay2"].ToString();
            }
            dbcl.Conn.Close();
        }

        private void Clear()
        {
            lbl_grosspay.Text = "0.0";
            lbl_basic.Text = "0.0";
            lbl_otpay.Text = "0.0";
            lbl_netpay.Text = "0.0";
            lbl_ttldeductions.Text = "0.0";
            lbl_pfpay.Text = "0.0";
            lbl_esicpay.Text = "0.0";
            lbl_allowances.Text = "0.0";
            lbl_advance.Text = "0.0";
            lbl_netpay2.Text = "0.0";

            finalized.Visible = false;
            salary_pdfrow.Visible = false;
            realtime.Visible = false;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //PDF();
            //CreateImagePdf();
            //Demo();
            //Demo2();

            GeneratePay();
        }

        private void PDF()
        {
            Byte[] bytes;
            //Instead of a FileStream we'll use a MemoryStream
            using (var MS = new System.IO.MemoryStream())
            {

                //Standard PDF setup, iText doesn't care what type of stream we're using
                var doc = new iTextSharp.text.Document();
                var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, MS);
                doc.SetPageSize(PageSize.A4);
                doc.SetMargins(5, 5, 5, 5);
                doc.Open();
                //doc.Add(new iTextSharp.text.Paragraph("Work Order No :      " + "" + txt_workorderno.Text + ""));
                PdfPTable table = new PdfPTable(4);
                table.AddCell("Row 1, Col 1");
                table.AddCell("Row 1, Col 2");
                table.AddCell("Row 1, Col 3");

                table.AddCell("Row 2, Col 1");
                table.AddCell("Row 2, Col 2");
                table.AddCell("Row 2, Col 3");

                table.AddCell("Row 3, Col 1");
                table.AddCell("Row 3, Col 2");
                table.AddCell("Row 3, Col 3");


                PdfPCell cell = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                table.AddCell("Row 2, Col 1");
                table.AddCell("Row 2, Col 1");
                table.AddCell("Row 2, Col 1");

                table.AddCell("Row 3, Col 1");
                cell = new PdfPCell(new Phrase("Row 3, Col 2 and Col3"));
                cell.Colspan = 2;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Row 4, Col 1 and Col2"));
                cell.Colspan = 2;
                table.AddCell(cell);
                table.AddCell("Row 4, Col 3");

                doc.Add(table);
                doc.Close();

                //Grab the raw bytes from the MemoryStream
                bytes = MS.ToArray();
            }
            Response.Clear();
            //Instead of a normal text/html header tell the browser that we've got a PDF
            Response.ContentType = "application/pdf";
            //Tell the browser that you want the file downloaded (ideally) and give it a pretty filename
            Response.AddHeader("content-disposition", "attachment;filename=MySampleFile.pdf");
            //Write our bytes to the stream
            Response.BinaryWrite(bytes);
            //Close the stream (otherwise ASP.Net might continue to write stuff on our behalf)
            Response.End();
        }

        //private void CreateImagePdf()
        //{
        //    MemoryStream byteStream = new MemoryStream();


        //    Document document = new Document();
        //    PdfWriter writer = PdfWriter.GetInstance(document, byteStream);
        //    document.SetPageSize(PageSize.A4);

        //    document.Open();


        //    Bitmap awtImg = new Bitmap(100, 100, PixelFormat.Format32bppRgb);
        //    Graphics g = Graphics.FromImage(awtImg);
        //    g.FillRectangle(new SolidBrush(Color.Green), 10, 10, 80, 80);
        //    g.Save();
        //    iTextSharp.text.Image itextImg = iTextSharp.text.Image.GetInstance(awtImg, (BaseColor)null);
        //    document.Add(itextImg);

        //    document.Close();

        //    byte[] pdfBytes = byteStream.ToArray();

        //    //return pdfBytes;

        //    Response.Clear();
        //    //Instead of a normal text/html header tell the browser that we've got a PDF
        //    Response.ContentType = "application/pdf";
        //    //Tell the browser that you want the file downloaded (ideally) and give it a pretty filename
        //    Response.AddHeader("content-disposition", "attachment;filename=MySampleFile.pdf");
        //    //Write our bytes to the stream
        //    Response.BinaryWrite(pdfBytes);
        //    //Close the stream (otherwise ASP.Net might continue to write stuff on our behalf)
        //    Response.End();
        //}

        private void Demo()
        {
            Document document = new Document(PageSize.A4, 15f, 15f, 15f, 15f);
            Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                Phrase phrase = null;
                PdfPCell cell = null;
                PdfPTable table = null;

                document.Open();

                //Header Table
                table = new PdfPTable(1);
                table.TotalWidth = 400f;
                table.LockedWidth = true;
                table.SetWidths(new float[] { 1f });

                //Company Name and Address
                phrase = new Phrase();
                phrase.Add(new Chunk("Microsoft Northwind Traders Company\n\n", FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.RED)));
                phrase.Add(new Chunk("107, Park site,\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("Salt Lake Road,\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("Seattle, USA", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT);
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                table.AddCell(cell);

                document.Add(table);

                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                //Employee Details
                cell = PhraseCell(new Phrase("Employee Record", FontFactory.GetFont("Arial", 12, Font.UNDERLINE, BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);

                table = new PdfPTable(2);
                table.SetWidths(new float[] { 0.5f, 2f });
                table.TotalWidth = 340f;
                table.LockedWidth = true;
                table.SpacingBefore = 20f;
                table.HorizontalAlignment = Element.ALIGN_RIGHT;

                phrase = new Phrase();
                phrase.Add(new Chunk("Mr. Mudassar Ahmed Khan\n", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)));
                phrase.Add(new Chunk("(Moderator)", FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)));
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT);
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                cell.Colspan = 2;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase(" "), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);

                //Employee Id
                table.AddCell(PhraseCell(new Phrase("Employee code:", FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("0001", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 10f;
                table.AddCell(cell);


                //Address
                table.AddCell(PhraseCell(new Phrase("Address:", FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                phrase = new Phrase(new Chunk("507 - 20th Ave. E.\nApt. 2A\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("Seattle\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("WA USA 98122", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                table.AddCell(PhraseCell(phrase, PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 10f;
                table.AddCell(cell);

                //Date of Birth
                table.AddCell(PhraseCell(new Phrase("Date of Birth:", FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("25 FEBRUARY", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 10f;
                table.AddCell(cell);
                document.Add(table);

                //Add border to page
                PdfContentByte content = writer.DirectContent;
                iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(document.PageSize);
                rectangle.Left += document.LeftMargin;
                rectangle.Right -= document.RightMargin;
                rectangle.Top -= document.TopMargin;
                rectangle.Bottom += document.BottomMargin;
                content.SetColorStroke(BaseColor.BLACK);
                content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
                content.Stroke();

                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=Employee.pdf");
                Response.ContentType = "application/pdf";
                Response.Buffer = true;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
                Response.End();
                Response.Close();
            }
        }

        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.BorderColor = BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 0f;
            return cell;
        }

        private void Demo2()
        {
            //Create document
            Document doc = new Document();
            //Create PDF Table
            PdfPTable tableLayout = new PdfPTable(4);
            //Create a PDF file in specific path
            PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("Sample-PDF-File.pdf"), FileMode.Create));
            //Open the PDF document
            doc.Open();
            //Add Content to PDF
            doc.Add(Add_Content_To_PDF(tableLayout));
            // Closing the document
            doc.Close();

            //Open the PDF file
            Process.Start(Server.MapPath("Sample-PDF-File.pdf"));
        }

        private PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {
            float[] headers = { 20, 20, 30, 30 }; //Header Widths
            tableLayout.SetWidths(headers); //Set the pdf headers
            tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  //Add Title to the PDF file at the top
            tableLayout.AddCell(new PdfPCell(new Phrase("Creating PDF file using iTextsharp", new Font(Font.NORMAL, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0))))
            {
                Colspan = 4,
                Border = 0,
                PaddingBottom = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            //Add header
            AddCellToHeader(tableLayout, "Cricketer Name");
            AddCellToHeader(tableLayout, "Height");
            AddCellToHeader(tableLayout, "Born On");
            AddCellToHeader(tableLayout, "Parents");
            //Add body
            AddCellToBody(tableLayout, "Sachin Tendulkar");
            AddCellToBody(tableLayout, "1.65 m");
            AddCellToBody(tableLayout, "April 24, 1973");
            AddCellToBody(tableLayout, "Ramesh Tendulkar, Rajni Tendulkar");
            AddCellToBody(tableLayout, "Mahendra Singh Dhoni");
            AddCellToBody(tableLayout, "1.75 m");
            AddCellToBody(tableLayout, "July 7, 1981");
            AddCellToBody(tableLayout, "Devki Devi, Pan Singh");
            AddCellToBody(tableLayout, "Virender Sehwag");
            AddCellToBody(tableLayout, "1.70 m");
            AddCellToBody(tableLayout, "October 20, 1978");
            AddCellToBody(tableLayout, "Aryavir Sehwag, Vedant Sehwag");
            AddCellToBody(tableLayout, "Virat Kohli");
            AddCellToBody(tableLayout, "1.75 m");
            AddCellToBody(tableLayout, "November 5, 1988");
            AddCellToBody(tableLayout, "Saroj Kohli, Prem Kohli");
            return tableLayout;
        }

        // Method to add single cell to the header
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.NORMAL, 8, 1, iTextSharp.text.BaseColor.WHITE)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(0, 51, 102)
            });
        }
        // Method to add single cell to the body
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.NORMAL, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = iTextSharp.text.BaseColor.WHITE
            });
        }


        //------------ Salary Slip PDF Download ---------------START-------------//
        private void GeneratePay()
        {
            DataSet ds = new DataSet();
            string EmployeeID = Session["WORKMAN"].ToString();
            string Month = lbl_monthcode.Text.ToString();
            string Year = lbl_calyear.Text.ToString();
            ds = GetData(EmployeeID, Month, Year);
            if (ds != null && ds.Tables.Count == 5 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                Panel pnlPrintControl = new Panel();
                Document doc = new Document(PageSize.A4, 36f, 36f, 36f, 36f);//36f, 36f, 90f, 100f);
                PdfWriter.GetInstance(doc, Response.OutputStream);
                doc.Open();
                //var fontFamily = FontFactory.GetFont("TIMES ROMAN", 15, BaseColor.BLUE);
                // 1) Adding logo to right side top
                string imagePath = Server.MapPath("~\\erp_images") + "\\ats_translogo.png";
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imagePath);
                image.Alignment = Element.ALIGN_MIDDLE;
                // set width and height
                image.ScaleToFit(100f, 120f);
                doc.Add(image);



                Paragraph comp = new Paragraph();
                comp.Add(new Chunk("AUTOMATION & TECHNICAL SERVICES", new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK)));
                comp.Alignment = 1;
                doc.Add(comp);


                // 2) Addling blank paragraph
                doc.Add(new Paragraph("  "));

                Paragraph title1 = new Paragraph();
                title1.Add(new Chunk("FORM XV : [See Rule 77(2)(b)]", new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK)));
                title1.Alignment = 1;
                doc.Add(title1);


                // 3) Adding title table
                Paragraph title = new Paragraph();
                title.Add(new Chunk("Wages Slip for the month of " + lbl_calmonth.Text.ToString() + "," + lbl_calyear.Text.ToString(), new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK)));
                title.Alignment = 1;
                doc.Add(title);
                // 4) Addling blank paragraph
                doc.Add(new Paragraph("  "));
                // 5) Creating 1st table with 4 column
                PdfPTable table1 = new PdfPTable(4);
                int[] columnwidth = { 20, 25, 20, 25 };
                table1.SetWidths(columnwidth);
                table1.WidthPercentage = 100;
                table1.HorizontalAlignment = 0;
                // 6) Adding employee data to table1
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    string columnName = (ds.Tables[0].Columns[i].ColumnName);
                    string columnValue = (ds.Tables[0].Rows[0][i].ToString());
                    PdfPCell cellColumnName = new PdfPCell(new Phrase(columnName, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                    cellColumnName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                                            //Cellcolumnname.Border = 15;
                    table1.AddCell(cellColumnName);
                    PdfPCell cellColumnValue = new PdfPCell(new Phrase(columnValue, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                    cellColumnValue.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                                             //cellcolumnvalue.Border = 15;
                    table1.AddCell(cellColumnValue);
                }
                doc.Add(table1);
                // 6) Addling blank paragraph
                doc.Add(new Paragraph("  "));
                // 7) Creating 2nd table with 4 columns [which is main table]
                PdfPTable mainTable = new PdfPTable(4); //earnedTable1.TotalWidth = 500f;//earnedTable1.LockedWidth = true;
                int[] columnwidth1 = { 30, 20, 30, 20 }; //23, 20, 25, 32 };
                mainTable.SetWidths(columnwidth1);
                mainTable.WidthPercentage = 100;
                mainTable.HorizontalAlignment = 0;
                // a. adding 4 cells for header
                mainTable.AddCell(new PdfPCell(new Phrase("EARNINGS", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) }); //{ HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) };;
                mainTable.AddCell(new PdfPCell(new Phrase("RUPEES", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) }); //{ HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) };;
                mainTable.AddCell(new PdfPCell(new Phrase("DEDUCTIONS", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) }); //{ HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) };;
                mainTable.AddCell(new PdfPCell(new Phrase("RUPEES", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) }); //{ HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) };;
                                                                                                                                                                                                                                                        // b. creating earning table with 2 columns [left side]
                PdfPTable earning = new PdfPTable(2);
                int[] columnwidth3 = { 30, 20 };
                earning.SetWidths(columnwidth3);
                earning.WidthPercentage = 80;
                earning.HorizontalAlignment = 0;
                // c. adding earning data
                for (int i = 0; i < ds.Tables[1].Columns.Count; i++)
                {
                    string columnName = (ds.Tables[1].Columns[i].ColumnName);
                    string columnValue = (ds.Tables[1].Rows[0][i].ToString());
                    PdfPCell cellColumnName = new PdfPCell(new Phrase(columnName, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                    cellColumnName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    earning.AddCell(cellColumnName);
                    PdfPCell cellColumnValue = new PdfPCell(new Phrase(AppendComma(columnValue), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT };
                    earning.AddCell(cellColumnValue);
                }
                // d. creating deduction table with 2 columns [Right side]
                PdfPTable deduction = new PdfPTable(2);
                int[] columnwidth4 = { 30, 20 };
                deduction.SetWidths(columnwidth3);
                deduction.WidthPercentage = 80;
                deduction.HorizontalAlignment = 0;
                // e. adding deduction data
                for (int i = 0; i < ds.Tables[2].Columns.Count; i++)
                {
                    string columnName = (ds.Tables[2].Columns[i].ColumnName);
                    string columnValue = (ds.Tables[2].Rows[0][i].ToString());
                    PdfPCell cellColumnName = new PdfPCell(new Phrase(columnName, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                    cellColumnName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    deduction.AddCell(cellColumnName);
                    PdfPCell cellColumnValue = new PdfPCell(new Phrase(AppendComma(columnValue), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT };
                    deduction.AddCell(cellColumnValue);
                }
                // f. creating a new cell [cell1] with colspan=2
                //    adding earning table into cell1
                PdfPCell cell1 = new PdfPCell(earning);
                cell1.Colspan = 2;
                // adding cell1 into mainTable
                mainTable.AddCell(cell1);
                // g. creating a new cell [cell2] with colspan=2
                //    adding deduction table into cell2
                PdfPCell cell2 = new PdfPCell(deduction);
                cell2.Colspan = 2;
                // adding cell2 into mainTable
                mainTable.AddCell(cell2);
                mainTable.AddCell(new PdfPCell(new Phrase("Gross Earning", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, BackgroundColor = new BaseColor(236, 236, 236) });
                mainTable.AddCell(new PdfPCell(new Phrase(AppendComma(ds.Tables[3].Rows[0][0].ToString()), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 });
                mainTable.AddCell(new PdfPCell(new Phrase("Gross Deductions", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) });
                mainTable.AddCell(new PdfPCell(new Phrase(AppendComma(ds.Tables[4].Rows[0][0].ToString()), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 });
                PdfPCell netEarning = new PdfPCell(new Phrase("Net Salary", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, BackgroundColor = new BaseColor(236, 236, 236) };
                mainTable.AddCell(netEarning);
                string NetSalary = (Convert.ToDecimal(ds.Tables[3].Rows[0][0].ToString()) - Convert.ToDecimal(ds.Tables[4].Rows[0][0].ToString())).ToString();
                PdfPCell netSalary = new PdfPCell(new Phrase(AppendComma(NetSalary), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 };
                mainTable.AddCell(netSalary);
                PdfPCell blankCell = new PdfPCell();
                blankCell.Colspan = 2;
                mainTable.AddCell(blankCell);
                PdfPCell NetSalaryInWords = new PdfPCell(new Phrase("Net Salary In Word : " + GenerateWordsinRs(NetSalary), new Font(Font.FontFamily.COURIER, 8, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5 };
                NetSalaryInWords.Colspan = 4;
                mainTable.AddCell(NetSalaryInWords);
                // adding mainTable to document object
                doc.Add(mainTable);
                doc.Add(new Paragraph("  "));
                doc.Add(new Paragraph("  "));
                doc.Add(new Paragraph("  "));
                Paragraph Note = new Paragraph();
                Note.Add(new Chunk("This is computer generated payslip and does not require signature or company seal.", new Font(Font.FontFamily.COURIER, 9, 1, BaseColor.BLACK)));
                Note.Alignment = 1;
                doc.Add(Note);
                Paragraph address = new Paragraph();
                address.Add(new Chunk(@"(Automation & Techinal Services.)
        Address: Near Samudayik Vikas Bhawan, Jemco Basti, Telco, Jamshedpur - 831004.", new Font(Font.FontFamily.COURIER, 9, 1, BaseColor.BLACK)));
                address.Alignment = 1;
                doc.Add(address);
                doc.Close();
                string fileName = Session["WORKMAN"].ToString() + "_" + lbl_calmonth.Text.ToString() + "_" + lbl_calyear.Text.ToString() + ".pdf";
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;" + "filename=" + fileName);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(doc);
                Response.End();
            }
            else
            {
                string title = "Opps :";
                string body = "Salary slip not generated.";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }
        private string AppendComma(string value)
        {
            if (value == "" || value == "0")
            {
                return String.Format("{0:#,0.00}", "0");
            }
            else
            {
                return String.Format("{0:#,0.00}", Convert.ToDecimal(value));
            }
        }
        public string GenerateWordsinRs(string inputRs)
        {
            string input = inputRs;
            string a = "";
            string b = "";
            // take decimal part of input. convert it to word. add it at the end of method.
            string decimals = "";
            if (input.Contains("."))
            {
                decimals = input.Substring(input.IndexOf(".") + 1);
                // remove decimal part from input
                input = input.Remove(input.IndexOf("."));
            }
            string strWords = NumbersToWords(Convert.ToInt32(input));
            if (!inputRs.Contains("."))
            {
                a = strWords + " Rupees Only";
            }
            else
            {
                a = strWords + " Rupees";
            }
            if (decimals.Length > 0)
            {
                // if there is any decimal part convert it to words and add it to strWords.
                string strwords2 = NumbersToWords(Convert.ToInt32(decimals));
                b = " and " + strwords2 + " Paisa Only ";
            }
            string final2 = "";
            final2 = a + b;
            return final2;
        }
        public static string NumbersToWords(int inputNumber)
        {
            int inputNo = inputNumber;
            if (inputNo == 0)
                return "Zero";
            int[] numbers = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (inputNo < 0)
            {
                sb.Append("Minus ");
                inputNo = -inputNo;
            }
            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
            string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
            string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };
            numbers[0] = inputNo % 1000; // units
            numbers[1] = inputNo / 1000;
            numbers[2] = inputNo / 100000;
            numbers[1] = numbers[1] - 100 * numbers[2]; // thousands
            numbers[3] = inputNo / 10000000; // crores
            numbers[2] = numbers[2] - 100 * numbers[3]; // lakhs
            for (int i = 3; i > 0; i--)
            {
                if (numbers[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (numbers[i] == 0) continue;
                u = numbers[i] % 10; // ones
                t = numbers[i] / 10;
                h = numbers[i] / 100; // hundreds
                t = t - 10 * h; // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i == 0) sb.Append("");
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }
        public DataSet GetData(string EmpId, string PaidMonth, string PaidYear)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand("usp_GetSalaryDetailsats");
            cmd.Connection = dbcl.Conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpId", EmpId);
            cmd.Parameters.AddWithValue("@PaidMonth", PaidMonth);
            cmd.Parameters.AddWithValue("@PaidYear", PaidYear);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {

            }
            cmd.Dispose();
            return ds;
        }
        //------------ Salary Slip PDF Download ----------------END------------//
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            GeneratePay();
        }
    }
}