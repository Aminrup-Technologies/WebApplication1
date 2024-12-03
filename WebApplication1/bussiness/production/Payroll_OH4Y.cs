using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication1.bussiness.production
{
    public class Payroll_OH4Y
    {
        DB_Utility_OH4Y DbCL = new DB_Utility_OH4Y();

        public void FindEmployeeTotalPresentByDates(string month, string year, string day1, string day2, string empwrk, ref decimal totalpresents)
        {
            //string cmdString = "SELECT COUNT(DISTINCT CreatedDate) as totalpresent FROM tbl_attendance where EmployeeWrk='" + empwrk + "' and MONTH(CreatedDate)='" + month + "' and YEAR(CreatedDate) = '" + year + "' and DAY(CreatedDate) between '" + day1 + "' and '" + day2 + "' and (AttendanceCode='P' or AttendanceCode='NH' or AttendanceCode='FL') and SiteIncharge_Approval='Approved' and AttendanceStatus='Present'";

            string cmdString = "SELECT SUM(CASE " +
                   "WHEN AttendanceCode = 'HP' THEN 2 " +
                   "WHEN AttendanceCode = 'HD' THEN 0.5 " +
                   "WHEN AttendanceCode IN ('P', 'NH', 'FL') THEN 1 " +
                   "ELSE 0 END) AS totalpresent " +
                   "FROM tbl_attendance " +
                   "WHERE EmployeeWrk = '" + empwrk + "' " +
                   "AND MONTH(CreatedDate) = '" + month + "' " +
                   "AND YEAR(CreatedDate) = '" + year + "' " +
                   "AND DAY(CreatedDate) BETWEEN '" + day1 + "' AND '" + day2 + "' " +
                   "AND SiteIncharge_Approval = 'Approved' " +
                   "AND AttendanceStatus = 'Present'";

            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                totalpresents = Convert.ToDecimal(Rdr["totalpresent"].ToString());
            }
            DbCL.Conn.Close();
        }

        public void FindEmployeeTotalPresentByDates2(string day1, string day2, string empwrk, ref decimal totalpresents)
        {
            //string cmdString = "SELECT COUNT(DISTINCT CreatedDate) as totalpresent FROM tbl_attendance where EmployeeWrk='" + empwrk + "' and CreatedDate between '" + day1 + "' and '" + day2 + "' and (AttendanceCode='P' or AttendanceCode='NH' or AttendanceCode='FL') and SiteIncharge_Approval='Approved' and AttendanceStatus='Present'";

            string cmdString = "SELECT COALESCE(SUM(AttendanceValue), 0) AS totalpresent " +
                   "FROM (" +
                       "SELECT CreatedDate, " +
                              "CASE " +
                                  "WHEN MAX(CASE WHEN AttendanceCode = 'HP' THEN 2 WHEN AttendanceCode = 'HD' THEN 0.5 WHEN AttendanceCode IN ('P', 'NH', 'FL', 'FP') THEN 1 ELSE 0 END) = 2 THEN 2 " +
                                  "WHEN MAX(CASE WHEN AttendanceCode = 'HD' THEN 0.5 WHEN AttendanceCode IN ('P', 'NH', 'FL', 'FP') THEN 1 ELSE 0 END) = 0.5 THEN 0.5 " +
                                  "ELSE 1 " +
                              "END AS AttendanceValue " +
                       "FROM tbl_attendance " +
                       "WHERE EmployeeWrk = '" + empwrk + "' " +
                       "AND CreatedDate BETWEEN '" + day1 + "' AND '" + day2 + "' " +
                       "AND SiteIncharge_Approval = 'Approved' " +
                       "AND AttendanceStatus = 'Present' " +
                       "GROUP BY CreatedDate" +
                   ") AS DistinctDates;";

            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                totalpresents = Convert.ToDecimal(Rdr["totalpresent"].ToString());
            }
            DbCL.Conn.Close();
        }


        public void FindEmployeeTotalPresentByDates2_SP(string day1, string day2, string empwrk, ref decimal totalpresents)
        {
            try
            {
                // Establish database connection
                DbCL.Sqlconnection();
                DbCL.ConnectDb();

                // Prepare the SQL command to call the stored procedure
                SqlCommand cmd = new SqlCommand("FindEmployeeTotalPresentByDates_F17", DbCL.Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters
                cmd.Parameters.AddWithValue("@Day1", day1);
                cmd.Parameters.AddWithValue("@Day2", day2);
                cmd.Parameters.AddWithValue("@EmpWrk", empwrk);

                // Add the output parameter
                SqlParameter outputParam = new SqlParameter("@TotalPresents", SqlDbType.Decimal)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                // Execute the command
                cmd.ExecuteNonQuery();

                // Retrieve the output parameter value
                totalpresents = Convert.ToDecimal(outputParam.Value);
            }
            catch (Exception ex)
            {
                // Handle exceptions (logging, rethrowing, etc.)
                throw new Exception("An error occurred while retrieving the total presents.", ex);
            }
            finally
            {
                // Close the connection
                if (DbCL.Conn.State == ConnectionState.Open)
                    DbCL.Conn.Close();
            }
        }



        public void FindEmployeeTotalOTByDates(string month, string year, string day1, string day2, string empwrk, ref decimal TotalOT)
        {
            //string cmdString = "select COALESCE(SUM(ProvidedOT),0) as totalot from tbl_attendance where MONTH(CreatedDate) = '" + month + "' AND YEAR(CreatedDate) = '" + year + "' and DAY(CreatedDate) between '" + day1 + "' and '" + day2 + "' AND EmployeeWrk='" + empwrk + "' AND (AttendanceCode='P' or AttendanceCode='NH' or AttendanceCode='OD' or AttendanceCode='FL') and SiteIncharge_Approval='Approved' and AttendanceStatus='Present'";

            string cmdString = "SELECT COALESCE(SUM(ProvidedOT), 0) AS totalot " +
                   "FROM tbl_attendance " +
                   "WHERE MONTH(CreatedDate) = '" + month + "' " +
                   "AND YEAR(CreatedDate) = '" + year + "' " +
                   "AND DAY(CreatedDate) BETWEEN '" + day1 + "' AND '" + day2 + "' " +
                   "AND EmployeeWrk = '" + empwrk + "' " +
                   "AND AttendanceCode IN ('P', 'NH', 'OD', 'FL', 'HP', 'FP', 'HD') " +
                   "AND SiteIncharge_Approval = 'Approved' " +
                   "AND AttendanceStatus = 'Present'";

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

        public void FindEmployeeTotalOTByDates1(string day1, string day2, string empwrk, ref decimal TotalOT)
        {
            //string cmdString = "select COALESCE(SUM(ProvidedOT),0) as totalot from tbl_attendance where CreatedDate between '" + day1 + "' and '" + day2 + "' AND EmployeeWrk='" + empwrk + "' AND (AttendanceCode='P' or AttendanceCode='NH' or AttendanceCode='OD' or AttendanceCode='FL') and SiteIncharge_Approval='Approved' and AttendanceStatus='Present'";

            string cmdString = "SELECT COALESCE(SUM(ProvidedOT), 0) AS totalot " +
                   "FROM tbl_attendance " +
                   "WHERE CreatedDate BETWEEN '" + day1 + "' AND '" + day2 + "' " +
                   "AND EmployeeWrk = '" + empwrk + "' " +
                   "AND AttendanceCode IN ('P', 'NH', 'OD', 'FL', 'HP', 'FP', 'HD') " +
                   "AND SiteIncharge_Approval = 'Approved' " +
                   "AND AttendanceStatus = 'Present'";


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

        public void BasicSalaryCalculation(decimal TOtalPresents, decimal basicrate, ref decimal BasicSalary)
        {
            BasicSalary = Math.Round(TOtalPresents * basicrate, 2);
        }

        public void PFPayCalculation(decimal BasicAmountforPFPay, ref decimal PFPay)
        {
            decimal pfpercent = 0.12m;
            PFPay = Math.Round(BasicAmountforPFPay * pfpercent, 0);
        }


        //--------------------------Homepage Attendance Viewer --------------------------//

        public void FindEmployeeTotalDaysByMonth(string month, string year, string empwrk, ref decimal totalpresents)
        {
            ////string cmdString = "SELECT COUNT(DISTINCT CreatedDate) as totalpresent FROM tbl_attendance where EmployeeWrk='" + empwrk + "' and MONTH(CreatedDate)='" + month + "' and YEAR(CreatedDate) = '" + year + "' and  (AttendanceCode='P' or AttendanceCode='NH' or AttendanceCode='FL' or AttendanceCode='OD') and SiteIncharge_Approval='Approved' and AttendanceStatus='Present'";

            //string cmdString = "SELECT SUM(CASE " +
            //       "WHEN AttendanceCode = 'HP' THEN 2 " +
            //       "WHEN AttendanceCode = 'HD' THEN 0.5 " +
            //       "WHEN AttendanceCode IN ('P', 'NH', 'FL', 'OD') THEN 1 " +
            //       "ELSE 0 END) AS totalpresent " +
            //       "FROM tbl_attendance " +
            //       "WHERE EmployeeWrk = '" + empwrk + "' " +
            //       "AND MONTH(CreatedDate) = '" + month + "' " +
            //       "AND YEAR(CreatedDate) = '" + year + "' " +
            //       "AND SiteIncharge_Approval = 'Approved' " +
            //       "AND AttendanceStatus = 'Present'";


            //DbCL.Sqlconnection();
            //DbCL.ConnectDb();
            //SqlCommand cmd = new SqlCommand(cmdString, DbCL.Conn);
            //SqlDataReader Rdr;
            //Rdr = cmd.ExecuteReader();
            //if (Rdr.Read())
            //{
            //    totalpresents = Convert.ToDecimal(Rdr["totalpresent"].ToString());
            //}
            //DbCL.Conn.Close();

            // Ensure the database connection is established
            DbCL.Sqlconnection();
            DbCL.ConnectDb();

            string storedProcedure = "sp_FindEmployeeTotalDaysByMonthWithApprovalStatus"; // Use the new SP or existing SP as needed

            using (SqlCommand command = new SqlCommand(storedProcedure, DbCL.Conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters for the stored procedure
                command.Parameters.AddWithValue("@EmployeeWrk", empwrk);
                command.Parameters.AddWithValue("@Month", int.Parse(month));
                command.Parameters.AddWithValue("@Year", int.Parse(year));

                // Execute the stored procedure and retrieve the result
                using (SqlDataReader Rdr = command.ExecuteReader())
                {
                    if (Rdr.Read())
                    {
                        totalpresents = Rdr["totalpresent"] != DBNull.Value ? Convert.ToDecimal(Rdr["totalpresent"]) : 0;
                    }
                }
                DbCL.Conn.Close();
            }
        }

        public void HP_FindEmployeeTotalDaysByMonth(string month, string year, string empwrk, ref decimal totalpresents)
        {
            ////string cmdString = "SELECT COUNT(DISTINCT CreatedDate) as totalpresent FROM tbl_attendance where EmployeeWrk='" + empwrk + "' and MONTH(CreatedDate)='" + month + "' and YEAR(CreatedDate) = '" + year + "' and  (AttendanceCode='P' or AttendanceCode='NH' or AttendanceCode='FL' or AttendanceCode='OD') and SubmitterStatus='Exit'";

            //DbCL.Sqlconnection();
            //DbCL.ConnectDb();
            ////string cmdString = "SELECT COUNT(DISTINCT CreatedDate) as totalpresent FROM tbl_attendance " +
            ////           "WHERE EmployeeWrk=@EmployeeWrk " +
            ////           "AND MONTH(CreatedDate)=@Month " +
            ////           "AND YEAR(CreatedDate)=@Year " +
            ////           "AND AttendanceCode IN ('P', 'NH', 'FL', 'OD') " +
            ////           "AND SubmitterStatus='Exit'";

            ////The below code is added on 05-Nov-2024 to provision 2 Present for working on National Holiday and 0.5 Present for Half Working Day
            //string cmdString = "SELECT SUM(CASE " +
            //       "WHEN AttendanceCode = 'HP' THEN 2 " +
            //       "WHEN AttendanceCode = 'HD' THEN 0.5 " +
            //       "WHEN AttendanceCode IN ('P', 'NH', 'FL', 'OD') THEN 1 " +
            //       "ELSE 0 END) AS totalpresent " +
            //       "FROM tbl_attendance " +
            //       "WHERE EmployeeWrk = @EmployeeWrk " +
            //       "AND MONTH(CreatedDate) = @Month " +
            //       "AND YEAR(CreatedDate) = @Year " +
            //       "AND SubmitterStatus = 'Exit'";


            //using (SqlCommand command = new SqlCommand(cmdString, DbCL.Conn))
            //{
            //    // Assuming empwrk, month, and year are variables containing your values
            //    command.Parameters.AddWithValue("@EmployeeWrk", empwrk);
            //    command.Parameters.AddWithValue("@Month", month);
            //    command.Parameters.AddWithValue("@Year", year);

            //    // Execute the query and retrieve the result
            //    //int totalPresent = (int)command.ExecuteScalar();
            //    decimal totalPresent = (decimal)command.ExecuteScalar();
            //    DbCL.Conn.Close();
            //}

            // Ensure the database connection is established
            DbCL.Sqlconnection();
            DbCL.ConnectDb();

            string storedProcedure = "sp_FindEmployeeTotalDaysByMonth";

            using (SqlCommand command = new SqlCommand(storedProcedure, DbCL.Conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters for the stored procedure
                command.Parameters.AddWithValue("@EmployeeWrk", empwrk);
                command.Parameters.AddWithValue("@Month", int.Parse(month));
                command.Parameters.AddWithValue("@Year", int.Parse(year));

                // Execute the stored procedure and retrieve the result
                object result = command.ExecuteScalar();

                // Check if the result is null, and assign it to totalpresents
                totalpresents = result != DBNull.Value ? Convert.ToDecimal(result) : 0;

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

        public void EmployeeOthersPayCalculations1(decimal TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            decimal hramult = Math.Round(hrapay * TOtalPresents, 2);
            hrmamnt = Math.Round(hramult / CalWorkingDays, 2);
        }

        public void EmployeeOthersPayCalculations2_NINL(decimal TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, decimal FixRateSalary, ref decimal hrmamnt)
        {
            decimal hrapay = FixRateSalary;
            hrmamnt = Math.Round(FixRateSalary * .05m, 2);
        }

        public void EmployeeOthersPayCalculations2(decimal TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            //decimal hramult = Math.Round(hrapay * TOtalPresents, 2);
            //hrmamnt = Math.Round(hramult / CalWorkingDays, 2);
            hrmamnt = hrapay;

            //This is updated on 01.05.2022 for calculating full HRA Amount and add it directly to NET Pay 2
        }

        public void EmployeeOthersPayCalculations3(decimal TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            decimal hramult = Math.Round(hrapay * TOtalPresents, 2);
            hrmamnt = Math.Round(hramult / CalWorkingDays, 2);
        }

        public void EmployeeOthersPayCalculations3_NINL(decimal TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, decimal FixRateSalary, ref decimal hrmamnt)
        {
            decimal hrapay = FixRateSalary;
            hrmamnt = Math.Round(FixRateSalary * .05m, 2);
        }

        public void EmployeeOthersPayCalculations4(decimal TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            decimal hramult = Math.Round(hrapay * TOtalPresents, 2);
            hrmamnt = Math.Round(hramult / CalWorkingDays, 2);
        }

        public void EmployeeOthersPayCalculations5(decimal TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            decimal hramult = Math.Round(hrapay * TOtalPresents, 2);
            hrmamnt = Math.Round(hramult / CalWorkingDays, 2);
        }

        public void EmployeeWashPayCalculations(decimal TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            decimal hramult = Math.Round(hrapay * TOtalPresents, 0);
            hrmamnt = Math.Round(hramult / CalWorkingDays, 0);
        }

        public void EmployeeOthersPayCalculations5_NINL(decimal TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, decimal BasicSalary, ref decimal hrmamnt)
        {
            decimal hrapay = BasicSalary;
            hrmamnt = Math.Round(BasicSalary * .05m, 0);
        }

        public void EmployeeOthersPayCalculations6(decimal TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            decimal hramult = Math.Round(hrapay * TOtalPresents, 2);
            hrmamnt = Math.Round(hramult / CalWorkingDays, 2);
        }

        public void EmployeeOthersPayCalculations7(decimal TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            decimal hramult = Math.Round(hrapay * TOtalPresents, 2);
            hrmamnt = Math.Round(hramult / CalWorkingDays, 2);
        }

        public void EmployeeOthersPayCalculations8(decimal TOtalPresents, Int32 CalWorkingDays, decimal HRAAmount, ref decimal hrmamnt)
        {
            decimal hrapay = HRAAmount;
            decimal hramult = Math.Round(hrapay * TOtalPresents, 2);
            hrmamnt = Math.Round(hramult / CalWorkingDays, 2);
        }
    }
}