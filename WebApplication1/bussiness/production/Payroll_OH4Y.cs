using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WebApplication1.bussiness.production
{
    public class Payroll_OH4Y
    {
        DB_Utility_OH4Y DbCL = new DB_Utility_OH4Y();

        public void FindEmployeeTotalPresentByDates(string month, string year, string day1, string day2, string empwrk, ref Int32 totalpresents)
        {
            string cmdString = "SELECT COUNT(DISTINCT CreatedDate) as totalpresent FROM tbl_attendance where EmployeeWrk='" + empwrk + "' and MONTH(CreatedDate)='" + month + "' and YEAR(CreatedDate) = '" + year + "' and DAY(CreatedDate) between '" + day1 + "' and '" + day2 + "' and (AttendanceCode='P' or AttendanceCode='NH' or AttendanceCode='FL') and SiteIncharge_Approval='Approved' and AttendanceStatus='Present'";
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                totalpresents = Convert.ToInt32(Rdr["totalpresent"].ToString());
            }
            DbCL.Conn.Close();
        }

        public void FindEmployeeTotalPresentByDates2(string day1, string day2, string empwrk, ref Int32 totalpresents)
        {
            string cmdString = "SELECT COUNT(DISTINCT CreatedDate) as totalpresent FROM tbl_attendance where EmployeeWrk='" + empwrk + "' and CreatedDate between '" + day1 + "' and '" + day2 + "' and (AttendanceCode='P' or AttendanceCode='NH' or AttendanceCode='FL') and SiteIncharge_Approval='Approved' and AttendanceStatus='Present'";
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                totalpresents = Convert.ToInt32(Rdr["totalpresent"].ToString());
            }
            DbCL.Conn.Close();
        }


        public void FindEmployeeTotalOTByDates(string month, string year, string day1, string day2, string empwrk, ref Int32 TotalOT)
        {
            string cmdString = "select COALESCE(SUM(ProvidedOT),0) as totalot from tbl_attendance where MONTH(CreatedDate) = '" + month + "' AND YEAR(CreatedDate) = '" + year + "' and DAY(CreatedDate) between '" + day1 + "' and '" + day2 + "' AND EmployeeWrk='" + empwrk + "' AND (AttendanceCode='P' or AttendanceCode='NH' or AttendanceCode='OD' or AttendanceCode='FL') and SiteIncharge_Approval='Approved' and AttendanceStatus='Present'";
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                TotalOT = Convert.ToInt32(Rdr["totalot"].ToString());
            }
            DbCL.Conn.Close();
        }

        public void FindEmployeeTotalOTByDates1(string day1, string day2, string empwrk, ref decimal TotalOT)
        {
            string cmdString = "select COALESCE(SUM(ProvidedOT),0) as totalot from tbl_attendance where CreatedDate between '" + day1 + "' and '" + day2 + "' AND EmployeeWrk='" + empwrk + "' AND (AttendanceCode='P' or AttendanceCode='NH' or AttendanceCode='OD' or AttendanceCode='FL') and SiteIncharge_Approval='Approved' and AttendanceStatus='Present'";
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                TotalOT = Convert.ToDecimal(Rdr["totalot"].ToString());
            }
            DbCL.Conn.Close();
        }

        public void FindEmployeeSkillType(string workman, ref string skilltype, ref string region)
        {
            string cmdString = "select SkillCategory,WorkRegion from tbl_Employee_Mustertable where WorkmanSL='" + workman + "'";
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                skilltype = Rdr["SkillCategory"].ToString();
                region = Rdr["WorkRegion"].ToString();
            }
            DbCL.Conn.Close();
        }

        public void FindPayCadre(string category, string wrkrgn, ref decimal dailyrate)
        {
            string cmdString = "select Total_Wages from tlb_payroll_wages where Category_Type='" + category.ToString() + "' and WorkRegion_Code = '" + wrkrgn + "' and Status='Active'";
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                dailyrate = Convert.ToDecimal(Rdr["Total_Wages"].ToString());
            }
            DbCL.Conn.Close();
        }

        public void FindCalWorkDays(string Year, string Month, string empregion, ref Int32 calwrkdays)
        {
            string cmdString = "select PayrollWorkDays from tbl_MonthlyPayrollStatus where PayrollYear='" + Year + "' and PayrollMonth='" + Month + "' and PayrollRegion = '" + empregion + "'";
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                calwrkdays = Convert.ToInt32(Rdr["PayrollWorkDays"].ToString());
            }
            DbCL.Conn.Close();
        }

        public void BasicSalaryCalculation(Int32 TOtalPresents, decimal basicrate, ref decimal BasicSalary)
        {
            BasicSalary = Math.Round(TOtalPresents * basicrate, 2);
        }

        public void PFPayCalculation(decimal BasicAmountforPFPay, ref decimal PFPay)
        {
            decimal pfpercent = 0.12m;
            PFPay = Math.Round(BasicAmountforPFPay * pfpercent, 0);
        }


        //--------------------------Homepage Attendance Viewer --------------------------//

        public void FindEmployeeTotalDaysByMonth(string month, string year, string empwrk, ref Int32 totalpresents)
        {
            string cmdString = "SELECT COUNT(DISTINCT CreatedDate) as totalpresent FROM tbl_attendance where EmployeeWrk='" + empwrk + "' and MONTH(CreatedDate)='" + month + "' and YEAR(CreatedDate) = '" + year + "' and  (AttendanceCode='P' or AttendanceCode='NH' or AttendanceCode='FL' or AttendanceCode='OD') and SiteIncharge_Approval='Approved' and AttendanceStatus='Present'";
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                totalpresents = Convert.ToInt32(Rdr["totalpresent"].ToString());
            }
            DbCL.Conn.Close();
        }

        public void HP_FindEmployeeTotalDaysByMonth(string month, string year, string empwrk, ref Int32 totalpresents)
        {
            //string cmdString = "SELECT COUNT(DISTINCT CreatedDate) as totalpresent FROM tbl_attendance where EmployeeWrk='" + empwrk + "' and MONTH(CreatedDate)='" + month + "' and YEAR(CreatedDate) = '" + year + "' and  (AttendanceCode='P' or AttendanceCode='NH' or AttendanceCode='FL' or AttendanceCode='OD') and SubmitterStatus='Exit'";

            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            string cmdString = "SELECT COUNT(DISTINCT CreatedDate) as totalpresent FROM tbl_attendance " +
                       "WHERE EmployeeWrk=@EmployeeWrk " +
                       "AND MONTH(CreatedDate)=@Month " +
                       "AND YEAR(CreatedDate)=@Year " +
                       "AND AttendanceCode IN ('P', 'NH', 'FL', 'OD') " +
                       "AND SubmitterStatus='Exit'";

            using (SqlCommand command = new SqlCommand(cmdString, DbCL.Conn))
            {
                // Assuming empwrk, month, and year are variables containing your values
                command.Parameters.AddWithValue("@EmployeeWrk", empwrk);
                command.Parameters.AddWithValue("@Month", month);
                command.Parameters.AddWithValue("@Year", year);

                // Execute the query and retrieve the result
                int totalPresent = (int)command.ExecuteScalar();
                DbCL.Conn.Close();
            }
        }

        public void FindEmployeeTotalPresentByMonth(string month, string year, string empwrk, ref Int32 totalpresents)
        {
            string cmdString = "SELECT COUNT(DISTINCT CreatedDate) as totalpresent FROM tbl_attendance where EmployeeWrk='" + empwrk + "' and MONTH(CreatedDate)='" + month + "' and YEAR(CreatedDate) = '" + year + "' and  AttendanceCode='P' and SiteIncharge_Approval='Approved' and AttendanceStatus='Present'";
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                totalpresents = Convert.ToInt32(Rdr["totalpresent"].ToString());
            }
            DbCL.Conn.Close();
        }

        public void FindEmployeeTotalODByMonth(string month, string year, string empwrk, ref Int32 totalpresents)
        {
            string cmdString = "SELECT COUNT(DISTINCT CreatedDate) as totalpresent FROM tbl_attendance where EmployeeWrk='" + empwrk + "' and MONTH(CreatedDate)='" + month + "' and YEAR(CreatedDate) = '" + year + "' and  AttendanceCode='OD' and SiteIncharge_Approval='Approved' and AttendanceStatus='Present'";
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                totalpresents = Convert.ToInt32(Rdr["totalpresent"].ToString());
            }
            DbCL.Conn.Close();
        }

        public void FindEmployeeTotalNHByMonth(string month, string year, string empwrk, ref Int32 totalpresents)
        {
            string cmdString = "SELECT COUNT(DISTINCT CreatedDate) as totalpresent FROM tbl_attendance where EmployeeWrk='" + empwrk + "' and MONTH(CreatedDate)='" + month + "' and YEAR(CreatedDate) = '" + year + "' and  AttendanceCode='NH' and SiteIncharge_Approval='Approved' and AttendanceStatus='Present'";
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                totalpresents = Convert.ToInt32(Rdr["totalpresent"].ToString());
            }
            DbCL.Conn.Close();
        }

        public void FindEmployeeTotalFLByMonth(string month, string year, string empwrk, ref Int32 totalpresents)
        {
            string cmdString = "SELECT COUNT(DISTINCT CreatedDate) as totalpresent FROM tbl_attendance where EmployeeWrk='" + empwrk + "' and MONTH(CreatedDate)='" + month + "' and YEAR(CreatedDate) = '" + year + "' and  AttendanceCode='FL' and SiteIncharge_Approval='Approved' and AttendanceStatus='Present'";
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                totalpresents = Convert.ToInt32(Rdr["totalpresent"].ToString());
            }
            DbCL.Conn.Close();
        }

        public void FindEmployeeTotalOTByMonth(string month, string year, string empwrk, ref decimal TotalOT)
        {
            //string qry = "select COALESCE(SUM(ProvidedOT),0) as totalot from tbl_attendance where CreatedDate between '" + day1 + "' and '" + day2 + "' AND EmployeeWrk='" + empwrk + "' AND (AttendanceCode='P' or AttendanceCode='NH' or AttendanceCode='OD' or AttendanceCode='FL') and SiteIncharge_Approval='Approved' and AttendanceStatus='Present'";

            string cmdString = "select COALESCE(SUM(ProvidedOT),0) as totalot from tbl_attendance where MONTH(CreatedDate) = '" + month + "' AND YEAR(CreatedDate) = '" + year + "' and EmployeeWrk='" + empwrk + "' AND (AttendanceCode='P' or AttendanceCode='NH' or AttendanceCode='OD' or AttendanceCode='FL') and SiteIncharge_Approval='Approved' and AttendanceStatus='Present'";
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                TotalOT = Convert.ToDecimal(Rdr["totalot"].ToString());
            }
            DbCL.Conn.Close();
        }


        //---------- Added on 27.07.2022 ------------- to pull employee payroll factors ----------------------//

        public void EmployeePayrollFactors(string empwrk, ref string FixedSalary_YesNo, ref decimal FixedAmount, ref Int32 WorkHours, ref Int32 OTFactor, ref string OTMultiplier, ref decimal DA_VDA, ref decimal HRA, ref decimal Conv_Allowance, ref decimal Medical_Allowance, ref decimal Washing_Allowance, ref decimal ATT_Allowance, ref decimal SPCL_Allowance, ref decimal Misc_Earnings, ref Int32 OT_Divisibility, ref Int32 Advance, ref Int32 Fines, ref Int32 Others)
        {
            string query = "select FixedSalary_YesNo, FixedAmount, WorkHours, OTFactor, OTMultiplier, DA_VDA, HRA,Conv_Allowance, Medical_Allowance, Washing_Allowance, ATT_Allowance, SPCL_Allowance, Misc_Earnings, OT_Divisibility, Cur_Advance, Cur_Fines, Cur_Others from tbl_Employee_Mustertable where WorkmanSL ='" + empwrk + "'";
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(query, DbCL.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                FixedSalary_YesNo = Rdr["FixedSalary_YesNo"].ToString();
                FixedAmount = Convert.ToDecimal(Rdr["FixedAmount"].ToString());
                WorkHours = Convert.ToInt32(Rdr["WorkHours"].ToString());
                OTFactor = Convert.ToInt32(Rdr["OTFactor"].ToString());
                OTMultiplier = Rdr["OTMultiplier"].ToString();
                DA_VDA = Convert.ToDecimal(Rdr["DA_VDA"].ToString());
                HRA = Convert.ToDecimal(Rdr["HRA"].ToString());
                Conv_Allowance = Convert.ToDecimal(Rdr["Conv_Allowance"].ToString());
                Medical_Allowance = Convert.ToDecimal(Rdr["Medical_Allowance"].ToString());
                Washing_Allowance = Convert.ToDecimal(Rdr["Washing_Allowance"].ToString());
                ATT_Allowance = Convert.ToDecimal(Rdr["ATT_Allowance"].ToString());
                SPCL_Allowance = Convert.ToDecimal(Rdr["SPCL_Allowance"].ToString());
                Misc_Earnings = Convert.ToDecimal(Rdr["Misc_Earnings"].ToString());
                OT_Divisibility = Convert.ToInt32(Rdr["OT_Divisibility"].ToString());
                Advance = Convert.ToInt32(Rdr["Cur_Advance"].ToString());
                Fines = Convert.ToInt32(Rdr["Cur_Fines"].ToString());
                Others = Convert.ToInt32(Rdr["Cur_Others"].ToString());
            }
            DbCL.Conn.Close();
        }

        public void EmployeeOthersPayCalculations1(Int32 TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            decimal hramult = Math.Round(hrapay * TOtalPresents, 2);
            hrmamnt = Math.Round(hramult / CalWorkingDays, 2);
        }

        public void EmployeeOthersPayCalculations2_NINL(Int32 TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, decimal FixRateSalary, ref decimal hrmamnt)
        {
            decimal hrapay = FixRateSalary;
            hrmamnt = Math.Round(FixRateSalary * .05m, 2);
        }

        public void EmployeeOthersPayCalculations2(Int32 TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            //decimal hramult = Math.Round(hrapay * TOtalPresents, 2);
            //hrmamnt = Math.Round(hramult / CalWorkingDays, 2);
            hrmamnt = hrapay;

            //This is updated on 01.05.2022 for calculating full HRA Amount and add it directly to NET Pay 2
        }

        public void EmployeeOthersPayCalculations3(Int32 TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            decimal hramult = Math.Round(hrapay * TOtalPresents, 2);
            hrmamnt = Math.Round(hramult / CalWorkingDays, 2);
        }

        public void EmployeeOthersPayCalculations3_NINL(Int32 TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, decimal FixRateSalary, ref decimal hrmamnt)
        {
            decimal hrapay = FixRateSalary;
            hrmamnt = Math.Round(FixRateSalary * .05m, 2);
        }

        public void EmployeeOthersPayCalculations4(Int32 TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            decimal hramult = Math.Round(hrapay * TOtalPresents, 2);
            hrmamnt = Math.Round(hramult / CalWorkingDays, 2);
        }

        public void EmployeeOthersPayCalculations5(Int32 TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            decimal hramult = Math.Round(hrapay * TOtalPresents, 2);
            hrmamnt = Math.Round(hramult / CalWorkingDays, 2);
        }

        public void EmployeeWashPayCalculations(Int32 TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            decimal hramult = Math.Round(hrapay * TOtalPresents, 0);
            hrmamnt = Math.Round(hramult / CalWorkingDays, 0);
        }

        public void EmployeeOthersPayCalculations5_NINL(Int32 TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, decimal BasicSalary, ref decimal hrmamnt)
        {
            decimal hrapay = BasicSalary;
            hrmamnt = Math.Round(BasicSalary * .05m, 0);
        }

        public void EmployeeOthersPayCalculations6(Int32 TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            decimal hramult = Math.Round(hrapay * TOtalPresents, 2);
            hrmamnt = Math.Round(hramult / CalWorkingDays, 2);
        }

        public void EmployeeOthersPayCalculations7(Int32 TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            decimal hramult = Math.Round(hrapay * TOtalPresents, 2);
            hrmamnt = Math.Round(hramult / CalWorkingDays, 2);
        }

        public void EmployeeOthersPayCalculations8(Int32 TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            decimal hramult = Math.Round(hrapay * TOtalPresents, 2);
            hrmamnt = Math.Round(hramult / CalWorkingDays, 2);
        }
    }
}