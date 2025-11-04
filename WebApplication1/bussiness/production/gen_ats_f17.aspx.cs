using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections.Generic;

namespace WebApplication1.bussiness.production
{
    public partial class gen_ats_f17 : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        Payroll_OH4Y PayRoll = new Payroll_OH4Y();
        DataTable dt_emps = new DataTable();

        public static Int32 minday = 0;
        public static Int32 maxday = 0;
        public static string year = "";
        public static string month = "";
        public static string region = "";
        public static string company = "";
        public static string startday = "";
        public static string endday = "";
        public static string date1 = "";
        public static string date2 = "";
        public static Int32 CalWorkingDays = 0;
        public static Int32 CalWorkingDaysF = 0;
        public static decimal TotalPresents = .0m;
        public static decimal GorssBreaker = 0;

        public static Int32 WashBreak = 1000; //Added for Calculating Washing Allowances

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
            {
                Response.Redirect("~/login.aspx");
            }
            if (!IsPostBack)
            {
                string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = 'IN' order by Id ";
                BindRegions(CmdString1);

                dbcl.BindMonthAndYearDropdowns(DDL_Month, DDL_Year);

                if (Session["REGION"].ToString() == "KPO")
                {
                    GorssBreaker = decimal.Parse(ConfigurationManager.AppSettings["F17_GrossBreaker_KPO"]);
                }
                else if (Session["REGION"].ToString() == "AGL")
                {
                    GorssBreaker = decimal.Parse(ConfigurationManager.AppSettings["F17_GrossBreaker_AGL"]);
                }
                else if (Session["REGION"].ToString() == "NINL")
                {
                    GorssBreaker = decimal.Parse(ConfigurationManager.AppSettings["F17_GrossBreaker_NINL"]);
                }
                else if (Session["REGION"].ToString() == "JSR")
                {
                    //GorssBreaker = 20500; -- Commented on 21-Aug-2024 Based on mail from Anupam Sharma dated : 19-Aug-2024 for changing ESIC Gross Breaker Amount from 19500 to 20999
                    GorssBreaker = decimal.Parse(ConfigurationManager.AppSettings["F17_GrossBreaker_JSR"]);
                }
                else if (Session["REGION"].ToString() == "RSP")
                {
                    GorssBreaker = decimal.Parse(ConfigurationManager.AppSettings["F17_GrossBreaker_RSP"]);
                }
                else
                {
                    GorssBreaker = decimal.Parse(ConfigurationManager.AppSettings["F17_GrossBreaker"]);
                }
                //dbcl.CalDateCombo1(DDL_Day, DDL_Month, DDL_Year);
                //dbcl.CalDateCombo1(DDL_D2, DDL_M2, DDL_Y2);
            }
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
            string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and Work_Region_Code = '" + DDL_Region.SelectedValue.ToString() + "' order by Id ";
            BindCompany(CmdString3);
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string current_year = DDL_Year.SelectedItem.Text.ToString();
            string current_month1 = DDL_Month.SelectedItem.Text.ToString();
            string current_month2 = DDL_Month.SelectedValue.ToString();
            year = current_year;
            month = current_month2;

            int month1 = int.Parse(current_month2);
            int year1 = int.Parse(current_year);
            int daysInMonth = DateTime.DaysInMonth(year1, month1);

            startday = "01";
            minday = Convert.ToInt32(startday);

            endday = daysInMonth.ToString("D2");
            maxday = Convert.ToInt32(endday);

            Int32 sundaycount = 0;
            sundaycount = dbcl.SundayCount(month1, year1);

            region = DDL_Region.SelectedValue.ToString();
            company = DDL_Company.SelectedValue.ToString();

            date1 = year + "-" + month + "-0" + minday;
            date2 = year + "-" + month + "-" + maxday;

            CalWorkingDays = daysInMonth - sundaycount;
            //CalWorkingDays = Convert.ToInt32(DDL_Days.SelectedItem.Text.ToString());


            if (DDL_Days.SelectedItem.Text.ToString() == CalWorkingDays.ToString())
            {
                if (DataRowChecker() != true) //If row does not exists then create row
                {
                    try
                    {
                        dbcl.Sqlconnection();
                        dbcl.ConnectDb();
                        string InsertQuery = "INSERT into tbl_MonthlyPayrollStatus(PayrollYear,PayrollMonth,PayrollRegion,PayrollCompany,PayrollStartDay,PayrollEndDay,PayrrollMonthDays,PayrollWorkDays,F17_TrialStatus,F17_TrialTimeStamp,F17_Trial_LoggerName,F17_Trial_LoggerWrk,F17_Trial_LoggerRegion) VALUES(@PayrollYear,@PayrollMonth,@PayrollRegion,@PayrollCompany,@PayrollStartDay,@PayrollEndDay,@PayrrollMonthDays,@PayrollWorkDays,@F17_TrialStatus,@F17_TrialTimeStamp,@F17_Trial_LoggerName,@F17_Trial_LoggerWrk,@F17_Trial_LoggerRegion)";
                        SqlCommand cmd = new SqlCommand(InsertQuery, dbcl.Conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@PayrollYear", year);
                        cmd.Parameters.AddWithValue("@PayrollMonth", month);
                        cmd.Parameters.AddWithValue("@PayrollRegion", region);
                        cmd.Parameters.AddWithValue("@PayrollCompany", company);
                        cmd.Parameters.AddWithValue("@PayrollStartDay", startday.ToString());
                        cmd.Parameters.AddWithValue("@PayrollEndDay", endday.ToString());
                        cmd.Parameters.AddWithValue("@PayrrollMonthDays", endday.ToString());
                        cmd.Parameters.AddWithValue("@PayrollWorkDays", CalWorkingDays);
                        cmd.Parameters.AddWithValue("@F17_TrialStatus", "No");
                        cmd.Parameters.AddWithValue("@F17_TrialTimeStamp", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                        cmd.Parameters.AddWithValue("@F17_Trial_LoggerName", Session["USERNAME"].ToString());
                        cmd.Parameters.AddWithValue("@F17_Trial_LoggerWrk", Session["WORKMAN"].ToString());
                        cmd.Parameters.AddWithValue("@F17_Trial_LoggerRegion", Session["REGION"].ToString());
                        cmd.ExecuteNonQuery();
                        dbcl.DisconnectDb();
                        dbcl.Conn.Close();
                    }
                    catch (Exception ex)
                    {
                        lbl_msg.Text = "Error : " + ex.Message;

                        string title = "Notifications :";
                        string body = ex.Message;
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                        //throw;
                    }
                }
                else
                {
                    if (StatusChecker() == true)
                    {
                        // if row exists and Status = "Yes" then display already finalized

                        btnInsertDB.Enabled = false;
                        btn_f17print.Enabled = true;
                        btn_f29print.Enabled = true;

                        string title = "Notifications :";
                        string body = "Trial Form 17 Already Finalized";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                    else
                    {
                        btnExport.Enabled = true;
                        btnInsertDB.Enabled = true;
                        btn_f17print.Enabled = false;
                        btn_f29print.Enabled = false;
                    }
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "Calender Working Wrong Selection....! re-try";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }


            ////string query = "select WorkmanSL, WorkRegion, FullName, SkillCategory, SkillDesignation,FixedSalary_YesNo, FixedAmount, WorkHours, OTFactor, OTMultiplier, DA_VDA, HRA,Conv_Allowance, Medical_Allowance, Washing_Allowance, ATT_Allowance, SPCL_Allowance, Misc_Earnings from tbl_Employee_Mustertable where WorkRegion='" + region + "' and WorkStatus='" + DDL_EmpWorkStatus.SelectedItem.Text.ToString() + "' order by Id";

            //string query = "select WorkmanSL, WorkRegion, FullName, SkillCategory, SkillDesignation,FixedSalary_YesNo, FixedAmount, WorkHours, OTFactor, OTMultiplier, DA_VDA, HRA,Conv_Allowance, Medical_Allowance, Washing_Allowance, ATT_Allowance, SPCL_Allowance, Misc_Earnings, OT_Divisibility, Cur_Advance, Cur_Fines, Cur_Others from tbl_Employee_Mustertable where WorkRegion='" + region + "' and WorkStatus='" + DDL_EmpWorkStatus.SelectedItem.Text.ToString() + "' and F17_YesNo='Yes' order by Id";

            //string query = "select WorkmanSL, WorkRegion, FullName, SkillCategory, SkillDesignation,FixedSalary_YesNo, FixedAmount, WorkHours, OTFactor, OTMultiplier, DA_VDA, HRA,Conv_Allowance, Medical_Allowance, Washing_Allowance, ATT_Allowance, SPCL_Allowance, Misc_Earnings, OT_Divisibility, Cur_Advance, Cur_Fines, Cur_Others from tbl_Employee_Mustertable where WorkRegion='" + region + "' and WorkStatus='" + DDL_EmpWorkStatus.SelectedItem.Text.ToString() + "' and F17_YesNo='Yes' and WorkmanSL='K1161' order by Id";

            //BindGridByQuery(query);

            string query = @"SELECT WorkmanSL, WorkRegion, FullName, SkillCategory, SkillDesignation, FixedSalary_YesNo, FixedAmount, WorkHours, OTFactor, OTMultiplier, DA_VDA, HRA, Conv_Allowance, Medical_Allowance, Washing_Allowance, ATT_Allowance, SPCL_Allowance, Misc_Earnings, OT_Divisibility, Cur_Advance, Cur_Fines, Cur_Others FROM tbl_Employee_Mustertable WHERE WorkRegion = @WorkRegion AND WorkStatus = @WorkStatus AND F17_YesNo = 'Yes' ORDER BY Id";

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@WorkRegion", SqlDbType.VarChar, 50) { Value = (object)region ?? DBNull.Value },
                new SqlParameter("@WorkStatus", SqlDbType.VarChar, 50) { Value = (object)DDL_EmpWorkStatus.SelectedItem.Text ?? DBNull.Value }
            };

            BindGridByQuery(query, parms);

        }


        private Boolean DataRowChecker()
        {
            Boolean flag = false;

            string cmdString = "";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            cmdString = "select COUNT(Id) from tbl_MonthlyPayrollStatus where PayrollYear=@PayrollYear and PayrollMonth=@PayrollMonth and PayrollRegion=@PayrollRegion and PayrollCompany=@PayrollCompany";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@PayrollYear", year);
            cmd.Parameters.AddWithValue("@PayrollMonth", month);
            cmd.Parameters.AddWithValue("@PayrollRegion", region);
            cmd.Parameters.AddWithValue("@PayrollCompany", company);
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.Conn.Close();

            if (count > 0)
            {
                flag = true;
            }
            return flag;
        }
        private Boolean StatusChecker()
        {
            Boolean flag = false;

            string cmdString = "";
            string status = "";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            cmdString = "select F17_TrialStatus from tbl_MonthlyPayrollStatus where PayrollYear=@PayrollYear and PayrollMonth=@PayrollMonth and PayrollRegion=@PayrollRegion and PayrollCompany=@PayrollCompany";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@PayrollYear", year);
            cmd.Parameters.AddWithValue("@PayrollMonth", month);
            cmd.Parameters.AddWithValue("@PayrollRegion", region);
            cmd.Parameters.AddWithValue("@PayrollCompany", company);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                status = Rdr["F17_TrialStatus"].ToString();
            }
            dbcl.Conn.Close();

            if (status == "Yes")
            {
                flag = true;
            }
            return flag;
        }

        // Keep your original single-argument method but route to the parameterized overload
        private void BindGridByQuery(string query)
        {
            BindGridByQuery(query, null);
        }

        // Parameterized overload using SqlParameter[]
        private void BindGridByQuery(string query, SqlParameter[] parameters)
        {
            try
            {
                // Ensure dbcl establishes the connection object (same as your current pattern)
                dbcl.Sqlconnection();

                // Do not 'using' the dbcl.Conn here (to avoid disposing a shared connection object).
                SqlConnection conn = dbcl.Conn;

                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // If parameters provided, add them
                        if (parameters != null && parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        // Optional: set a timeout (adjust if needed)
                        cmd.CommandTimeout = 90;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            // Ensure DataTable exists and is cleared
                            if (dt_emps == null) dt_emps = new DataTable();
                            else dt_emps.Clear();

                            da.Fill(dt_emps);

                            GridView.DataSource = dt_emps;
                            GridView.DataBind();
                            f17grid.Visible = true;
                        } // da disposed
                    } // cmd disposed
                }
                finally
                {
                    // Close connection if we opened it here
                    if (conn != null && conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception server-side in production. Show friendly message to user.
                string title = "Notifications :";
                string body = "Unable to load employee data. Contact administrator.";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                // Optionally set label for debugging (remove in production)
                // lbl_msg.Text = ex.Message;
            }
        }


        private void BindGridByQuery_OLD(string query)
        {
            try
            {
                dbcl.Sqlconnection();
                using (SqlConnection conn = dbcl.Conn)
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dt_emps.Clear(); // Clear the DataTable before filling with new data
                            da.Fill(dt_emps);
                            GridView.DataSource = dt_emps;
                            GridView.DataBind();
                            f17grid.Visible = true;
                        }
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
        protected void GridView_DataBound_OLD(object sender, EventArgs e)
        {
            for (int i = 0; i <= GridView.Rows.Count - 1; i++)
            {
                Label lbl_WorkRegion = (Label)GridView.Rows[i].FindControl("lbl_WorkRegion");
                Label lbl_WorkmanSL = (Label)GridView.Rows[i].FindControl("lbl_WorkmanSL");

                Label lbl_payrate = (Label)GridView.Rows[i].FindControl("lbl_payrate");
                Label lbl_skill_category = (Label)GridView.Rows[i].FindControl("lbl_SkillCategory");

                Label lbl_WorkHours = (Label)GridView.Rows[i].FindControl("lbl_WorkHours");
                Label lbl_OTFactor = (Label)GridView.Rows[i].FindControl("lbl_OTFactor");
                Label lbl_OTMultiplier = (Label)GridView.Rows[i].FindControl("lbl_OTMultiplier");
                Label lbl_OT_Divisibility = (Label)GridView.Rows[i].FindControl("lbl_OT_Divisibility");

                Label lbl_presents = (Label)GridView.Rows[i].FindControl("lbl_presents"); //Total Payable Days
                //Label lblAbsent = (Label)GridView.Rows[i].FindControl("lblAbsent");
                Label lbl_flcount = (Label)GridView.Rows[i].FindControl("lbl_flcount");
                Label lbl_flpcount = (Label)GridView.Rows[i].FindControl("lbl_flpcount");
                Label lbl_halfdaycount = (Label)GridView.Rows[i].FindControl("lbl_halfdaycount");
                Label lbl_nhpcount = (Label)GridView.Rows[i].FindControl("lbl_nhpcount");
                Label lbl_nhcount = (Label)GridView.Rows[i].FindControl("lbl_nhcount");
                Label lbl_oddayscount = (Label)GridView.Rows[i].FindControl("lbl_oddayscount");
                Label lbl_presentdayscount = (Label)GridView.Rows[i].FindControl("lbl_presentdayscount");  // Total P Counts
                //Label lbl_pendingcount = (Label)GridView.Rows[i].FindControl("lbl_pendingcount");
                Label lbl_ttlot = (Label)GridView.Rows[i].FindControl("lbl_ttlot");

                Label lbl_FixedSalary_YesNo = (Label)GridView.Rows[i].FindControl("lbl_FixedSalary_YesNo");
                Label lbl_FixedAmount = (Label)GridView.Rows[i].FindControl("lbl_FixedAmount");
                Label lbl_fixedwagerate = (Label)GridView.Rows[i].FindControl("lbl_fixedwagerate");
                Label lbl_fixratesal = (Label)GridView.Rows[i].FindControl("lbl_fixratesal");


                Label lbl_DA_VDA = (Label)GridView.Rows[i].FindControl("lbl_DA_VDA");
                Label lbl_HRA = (Label)GridView.Rows[i].FindControl("lbl_HRA");
                Label lbl_Conv_Allowance = (Label)GridView.Rows[i].FindControl("lbl_Conv_Allowance");
                Label lbl_Medical_Allowance = (Label)GridView.Rows[i].FindControl("lbl_Medical_Allowance");
                Label lbl_Washing_Allowance = (Label)GridView.Rows[i].FindControl("lbl_Washing_Allowance");
                Label lbl_ATT_Allowance = (Label)GridView.Rows[i].FindControl("lbl_ATT_Allowance");
                Label lbl_SPCL_Allowance = (Label)GridView.Rows[i].FindControl("lbl_SPCL_Allowance");
                Label lbl_Misc_Earnings = (Label)GridView.Rows[i].FindControl("lbl_Misc_Earnings");

                Label lbl_Cur_Advance = (Label)GridView.Rows[i].FindControl("lbl_Cur_Advance");
                Label lbl_Cur_Fines = (Label)GridView.Rows[i].FindControl("lbl_Cur_Fines");
                Label lbl_Cur_Others = (Label)GridView.Rows[i].FindControl("lbl_Cur_Others");


                Label lbl_otherspay = (Label)GridView.Rows[i].FindControl("lbl_otherspay");

                Label lbl_basicsalary = (Label)GridView.Rows[i].FindControl("lbl_basicsalary");
                Label lbl_PFPay = (Label)GridView.Rows[i].FindControl("lbl_PFPay");
                Label lbl_otwages = (Label)GridView.Rows[i].FindControl("lbl_otwages");

                Label lbl_actualgross = (Label)GridView.Rows[i].FindControl("lbl_actualgross");
                Label lbl_grossamount = (Label)GridView.Rows[i].FindControl("lbl_grossamount");

                Label lbl_esicpay = (Label)GridView.Rows[i].FindControl("lbl_esicpay");
                Label lbl_netpay1 = (Label)GridView.Rows[i].FindControl("lbl_netpay1");
                Label lbl_netpay2 = (Label)GridView.Rows[i].FindControl("lbl_netpay2");

                TotalPresents = .0m;
                //----------------- Function call to find out the Total Present aganist the Employee Workman Sl----------------//
                string empwrk = lbl_WorkmanSL.Text.ToString();
                PayRoll.FindEmployeeTotalPresentByDates2_SP(date1, date2, empwrk, ref TotalPresents);
                lbl_presents.Text = TotalPresents.ToString();

                Dictionary<string, int> attendanceData = new Dictionary<string, int>();
                PayRoll.GetEmployeeAttendanceCounts(month.ToString(), year.ToString(), empwrk, out attendanceData);

                //lblAbsent.Text = attendanceData["Ab"].ToString();
                lbl_flcount.Text = attendanceData["FL"].ToString();
                lbl_flpcount.Text = attendanceData["FP"].ToString();
                lbl_halfdaycount.Text = attendanceData["HD"].ToString();
                //lbl_halfdaycount.Text = (Convert.ToDecimal(attendanceData["HD"]) * 0.5m).ToString();
                lbl_nhpcount.Text = attendanceData["HP"].ToString();
                lbl_nhcount.Text = attendanceData["NH"].ToString();
                lbl_oddayscount.Text = attendanceData["OD"].ToString();
                lbl_presentdayscount.Text = attendanceData["P"].ToString();
                //lbl_pendingcount.Text = attendanceData["Pending"].ToString();

                //----------------- Function call to find out the Total OverTime Unit aganist the Employee Workman Sl----------------//
                decimal TotalOT = .0m;
                PayRoll.FindEmployeeTotalOTByDates1(date1, date2, empwrk, ref TotalOT);
                lbl_ttlot.Text = TotalOT.ToString();


                //----------------- Function call to find out the Daily Pay Rate aganist the Employee Skill Category----------------//
                decimal dr = 0.0m;
                string skillevel = lbl_skill_category.Text.ToString();
                string workregion = lbl_WorkRegion.Text.ToString();
                PayRoll.FindPayCadre(skillevel, workregion, ref dr);
                lbl_payrate.Text = dr.ToString();

                if (workregion == "KPO")
                {
                    if (empwrk == "K68" || empwrk == "K91" || empwrk == "K92" || empwrk == "K579" || empwrk == "K584" || empwrk == "K612" || empwrk == "K620")
                    {
                        CalWorkingDaysF = 26;
                    }
                    else
                    {
                        CalWorkingDaysF = CalWorkingDays;
                    }
                }
                else
                {
                    CalWorkingDaysF = CalWorkingDays;
                }

                //----------------- Calculation for Employee who are in Fixed Salary----------------//
                string fixedyesno = lbl_FixedSalary_YesNo.Text.ToString();
                decimal fixedamount = Convert.ToDecimal(lbl_FixedAmount.Text.ToString());
                decimal fdr = Convert.ToDecimal(fixedamount) / Convert.ToDecimal(CalWorkingDaysF);
                decimal fnlfdr = Math.Round(fdr, 2);
                lbl_fixedwagerate.Text = fnlfdr.ToString();


                //------------ Wage of Fixed rate ---------------   ( FixedAmount / CalenderDays ) x  PresentDays
                decimal FixRateSalary = 0.0m;
                FixRateSalary = Math.Round(fdr * TotalPresents, 2);
                lbl_fixratesal.Text = FixRateSalary.ToString();


                //-------------Basic Salary or Basic Wages  -----------   Daily PayRate x Present Days
                decimal BasicSalary = 0.0m;
                PayRoll.BasicSalaryCalculation(TotalPresents, dr, ref BasicSalary);
                lbl_basicsalary.Text = BasicSalary.ToString();

                decimal DaVdaAmount = Convert.ToDecimal(lbl_DA_VDA.Text.ToString());
                decimal HRAAmount = Convert.ToDecimal(lbl_HRA.Text.ToString());
                decimal ConvAmount = Convert.ToDecimal(lbl_Conv_Allowance.Text.ToString());
                decimal MedAmount = Convert.ToDecimal(lbl_Medical_Allowance.Text.ToString());
                decimal WashAmount = Convert.ToDecimal(lbl_Washing_Allowance.Text.ToString());
                decimal AttAmount = Convert.ToDecimal(lbl_ATT_Allowance.Text.ToString());
                decimal SPCLAmount = Convert.ToDecimal(lbl_SPCL_Allowance.Text.ToString());
                decimal MiscAmount = Convert.ToDecimal(lbl_Misc_Earnings.Text.ToString());

                int AdvanceAmt = Convert.ToInt32(lbl_Cur_Advance.Text.ToString());
                int FinesAmt = Convert.ToInt32(lbl_Cur_Fines.Text.ToString());
                int OthersAmt = Convert.ToInt32(lbl_Cur_Others.Text.ToString());

                decimal DaVdaPay = .0m;
                decimal HRAPay = .0m;
                decimal ConvPay = .0m;
                decimal MedPay = .0m;
                decimal WashPay = .0m;
                decimal WashPayF = .0m;
                decimal AttPay = .0m;
                decimal SPCLPay = .0m;
                decimal MiscPay = .0m;

                Label lbl_DaVdaPay = (Label)GridView.Rows[i].FindControl("lbl_DaVdaPay");
                Label lbl_HRAPay = (Label)GridView.Rows[i].FindControl("lbl_HRAPay");
                Label lbl_ConvPay = (Label)GridView.Rows[i].FindControl("lbl_ConvPay");
                Label lbl_MedPay = (Label)GridView.Rows[i].FindControl("lbl_MedPay");
                Label lbl_WashPay = (Label)GridView.Rows[i].FindControl("lbl_WashPay");
                Label lbl_AttPay = (Label)GridView.Rows[i].FindControl("lbl_AttPay");
                Label lbl_SPCLPay = (Label)GridView.Rows[i].FindControl("lbl_SPCLPay");
                Label lbl_MiscPay = (Label)GridView.Rows[i].FindControl("lbl_MiscPay");

                Label lbl_ttlded = (Label)GridView.Rows[i].FindControl("lbl_ttlded");
                Label lbl_netpayfnl = (Label)GridView.Rows[i].FindControl("lbl_netpayfnl");

                //---------------- DA/VDA Alowances Cal-------------------------------------//
                PayRoll.EmployeeOthersPayCalculations1(TotalPresents, CalWorkingDaysF, DaVdaAmount, ref DaVdaPay);
                lbl_DaVdaPay.Text = DaVdaPay.ToString();

                //---------------- HRA Alowances Cal-------------------------------------//
                if (workregion == "RSP")
                {
                    decimal hramult = 0.05m;
                    HRAPay = Math.Ceiling(BasicSalary * hramult) ;

                    lbl_HRAPay.Text = HRAPay.ToString();
                }
                else
                {
                    PayRoll.EmployeeOthersPayCalculations2(TotalPresents, CalWorkingDaysF, HRAAmount, ref HRAPay);
                    lbl_HRAPay.Text = HRAPay.ToString();
                }

                //---------------- Conv Alowances Cal-------------------------------------//
                PayRoll.EmployeeOthersPayCalculations3(TotalPresents, CalWorkingDaysF, ConvAmount, ref ConvPay);
                lbl_ConvPay.Text = ConvPay.ToString();

                //---------------- Medical Alowances Cal-------------------------------------//
                PayRoll.EmployeeOthersPayCalculations4(TotalPresents, CalWorkingDaysF, MedAmount, ref MedPay);
                lbl_MedPay.Text = MedPay.ToString();
                WashPay = 0;
                WashPayF = 0;
                //---------------- Wash Alowances Cal-------------------------------------//
                PayRoll.EmployeeWashPayCalculations(TotalPresents, CalWorkingDaysF, WashBreak, ref WashPay);
                if (WashPay <= 0)
                {
                    WashPayF = 0;
                }
                else
                {
                    WashPayF = WashPay;
                }

                //---------------- Att Alowances Cal-------------------------------------//
                PayRoll.EmployeeOthersPayCalculations6(TotalPresents, CalWorkingDaysF, AttAmount, ref AttPay);
                lbl_AttPay.Text = AttPay.ToString();

                if (workregion == "RSP")
                {
                    decimal awamult = 157.69m;
                    SPCLPay = Math.Round(TotalPresents * awamult,2);
                    lbl_SPCLPay.Text = SPCLPay.ToString();
                }
                else
                {
                    //---------------- SPCL Alowances Cal-------------------------------------//
                    PayRoll.EmployeeOthersPayCalculations7(TotalPresents, CalWorkingDaysF, SPCLAmount, ref SPCLPay);
                    lbl_SPCLPay.Text = SPCLPay.ToString();
                }
                

                //---------------- MISC Alowances Cal-------------------------------------//
                PayRoll.EmployeeOthersPayCalculations8(TotalPresents, CalWorkingDaysF, MiscAmount, ref MiscPay);
                lbl_MiscPay.Text = MiscPay.ToString();

                //--------------- PF Calucations --------------------//
                decimal PFPay = 0.0m;
                PayRoll.PFPayCalculation(BasicSalary, ref PFPay);
                lbl_PFPay.Text = PFPay.ToString();


                //---------------- OT Pay -------- Gross Rate
                decimal otpay = 0.0m;
                decimal wrkhrs = Convert.ToDecimal(lbl_WorkHours.Text.ToString());
                decimal otdiv = Convert.ToDecimal(lbl_OT_Divisibility.Text.ToString());  //Added on 29-11-2021
                decimal otfactor = Convert.ToDecimal(lbl_OTFactor.Text.ToString());
                string otrate = lbl_OTMultiplier.Text.ToString();

                decimal actualgross = 0.0m;
                decimal grossesic = 0.0m;
                decimal otherpay = 0.0m;
                decimal otherpayF = 0.0m;

                if (fixedyesno == "Yes")  ///Check whether the employee is in Fixed or Daily Rate Payroll
                {
                    if (otrate == "Gross")
                    {
                        if (TotalOT >= 1)
                        {
                            decimal otdays = TotalOT / otdiv;
                            // OTPay on FixedRate
                            otpay = Math.Round(fdr * otdays * otfactor, 0);

                            lbl_otwages.Text = otpay.ToString();
                        }
                        else
                        {
                            lbl_otwages.Text = otpay.ToString();
                        }
                        otherpay = FixRateSalary - BasicSalary - WashPayF;
                        actualgross = BasicSalary + otpay + otherpay + WashPayF;
                    }
                    else
                    {
                        if (TotalOT >= 1)
                        {
                            decimal otdays = TotalOT / otdiv;
                            //OTPay on DailyRate
                            otpay = Math.Round(dr * otdays * otfactor, 0);

                            lbl_otwages.Text = otpay.ToString();
                        }
                        else
                        {
                            lbl_otwages.Text = otpay.ToString();
                        }
                        otherpay = FixRateSalary - BasicSalary - WashPayF;

                        actualgross = BasicSalary + otpay + otherpay + WashPayF;
                    }
                    lbl_actualgross.Text = actualgross.ToString();
                }
                else
                {
                    if (otrate == "Gross")
                    {
                        if (TotalOT >= 1)
                        {
                            decimal otdays = TotalOT / otdiv;
                            // OTPay on FixedRate
                            otpay = Math.Round(fdr * otdays * otfactor, 0);

                            lbl_otwages.Text = otpay.ToString();
                        }
                        else
                        {
                            lbl_otwages.Text = otpay.ToString();
                        }
                        WashPayF = 0;
                        otherpay = 0;
                        actualgross = BasicSalary + otpay + otherpay + WashPayF;
                    }
                    else
                    {
                        if (TotalOT >= 1)
                        {
                            decimal otdays = TotalOT / otdiv;
                            //OTPay on DailyRate
                            otpay = Math.Round(dr * otdays * otfactor, 0);

                            lbl_otwages.Text = otpay.ToString();
                        }
                        else
                        {
                            lbl_otwages.Text = otpay.ToString();
                        }
                        WashPayF = 0;
                        otherpay = 0;
                        actualgross = BasicSalary + otpay + otherpay + WashPayF + HRAPay + SPCLPay;
                    }
                    lbl_actualgross.Text = actualgross.ToString();
                }


                //----------------Gross Calculation & NET Payment 2 -------------------------------//
                decimal washgross = actualgross + WashPayF + ConvPay;

                if (actualgross > GorssBreaker)
                {
                    grossesic = GorssBreaker - WashPayF;
                }
                else
                {
                    grossesic = actualgross - WashPayF;
                }

                lbl_grossamount.Text = grossesic.ToString();
                if (otherpay < 0)
                {
                    otherpayF = 0;
                }
                else
                {
                    otherpayF = otherpay;
                }
                lbl_otherspay.Text = otherpayF.ToString();
                lbl_WashPay.Text = WashPayF.ToString();
                //--------------------- ESIC pay---------------------------------------------------//

                decimal esicpay = 0.0m;
                esicpay = Math.Round(grossesic * 0.0075m, 0);
                lbl_esicpay.Text = esicpay.ToString();


                //--------------------- NET Payment -------------------------------------------------//
                decimal netpay1 = 0.0m;
                decimal newgross = grossesic;
                netpay1 = Math.Round(newgross - PFPay - esicpay + WashPayF + ConvPay, 0);

                //Here total deductions means --- Employee side deductions
                decimal ttldeductions = AdvanceAmt + FinesAmt + OthersAmt;
                lbl_ttlded.Text = ttldeductions.ToString();

                decimal netpay1final = .0m;
                decimal netpay2 = 0.0m;
                if (fixedyesno == "Yes")
                {
                    if (actualgross > WashBreak)
                    {
                        netpay2 = actualgross - newgross + HRAPay - WashPayF;
                    }
                    else
                    {
                        netpay2 = 0.0m;
                    }
                }
                else
                {
                    netpay2 = actualgross - netpay1 - PFPay - esicpay + HRAPay - WashPayF;
                }

                //to deduct max amount from Pay2, if it is greater then or equal to the total deduction amount

                decimal check = netpay2 - ttldeductions;
                decimal netpay2_finalaftrded = .0m;
                if (workregion == "KPO")
                {
                    if (check > 0)
                    {
                        netpay2_finalaftrded = netpay2 - ttldeductions;
                        netpay1final = netpay1;
                    }
                    else
                    {
                        netpay2_finalaftrded = netpay2;
                        netpay1final = netpay1 - ttldeductions;
                    }
                }
                else if (workregion == "AGL")
                {
                    netpay2_finalaftrded = netpay2;
                    netpay1final = netpay1 - ttldeductions;
                }
                else if (workregion == "NINL")
                {
                    netpay2_finalaftrded = netpay2;
                    netpay1final = netpay1 - ttldeductions;
                }
                else if (workregion == "JSR")
                {
                    netpay2_finalaftrded = netpay2;
                    netpay1final = netpay1 - ttldeductions;
                }
                else if(workregion == "RSP")
                {
                    netpay2_finalaftrded = netpay2 - HRAPay;
                    netpay1final = netpay1 - ttldeductions ;
                }
                else
                {
                    netpay2_finalaftrded = netpay2;
                    netpay1final = netpay1 - ttldeductions;
                }

                lbl_netpay1.Text = netpay1.ToString();
                lbl_netpayfnl.Text = netpay1final.ToString();
                lbl_netpay2.Text = netpay2_finalaftrded.ToString();///---1
            }

            //btnExport.Enabled = true;
        }


        // This prevents any carry-over between rows. No business logic changed.
        void ResetToZero(params Label[] labels)
        {
            if (labels == null) return;
            foreach (var l in labels)
                if (l != null) l.Text = "0";
        }

        protected void GridView_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i <= GridView.Rows.Count - 1; i++)
            {
                Label lbl_WorkRegion = (Label)GridView.Rows[i].FindControl("lbl_WorkRegion");
                Label lbl_WorkmanSL = (Label)GridView.Rows[i].FindControl("lbl_WorkmanSL");

                Label lbl_payrate = (Label)GridView.Rows[i].FindControl("lbl_payrate");
                Label lbl_skill_category = (Label)GridView.Rows[i].FindControl("lbl_SkillCategory");

                Label lbl_WorkHours = (Label)GridView.Rows[i].FindControl("lbl_WorkHours");
                Label lbl_OTFactor = (Label)GridView.Rows[i].FindControl("lbl_OTFactor");
                Label lbl_OTMultiplier = (Label)GridView.Rows[i].FindControl("lbl_OTMultiplier");
                Label lbl_OT_Divisibility = (Label)GridView.Rows[i].FindControl("lbl_OT_Divisibility");

                Label lbl_presents = (Label)GridView.Rows[i].FindControl("lbl_presents"); //Total Payable Days
                                                                                          //Label lblAbsent = (Label)GridView.Rows[i].FindControl("lblAbsent");
                Label lbl_flcount = (Label)GridView.Rows[i].FindControl("lbl_flcount");
                Label lbl_flpcount = (Label)GridView.Rows[i].FindControl("lbl_flpcount");
                Label lbl_halfdaycount = (Label)GridView.Rows[i].FindControl("lbl_halfdaycount");
                Label lbl_nhpcount = (Label)GridView.Rows[i].FindControl("lbl_nhpcount");
                Label lbl_nhcount = (Label)GridView.Rows[i].FindControl("lbl_nhcount");
                Label lbl_oddayscount = (Label)GridView.Rows[i].FindControl("lbl_oddayscount");
                Label lbl_presentdayscount = (Label)GridView.Rows[i].FindControl("lbl_presentdayscount");  // Total P Counts
                                                                                                           //Label lbl_pendingcount = (Label)GridView.Rows[i].FindControl("lbl_pendingcount");
                Label lbl_ttlot = (Label)GridView.Rows[i].FindControl("lbl_ttlot");

                Label lbl_FixedSalary_YesNo = (Label)GridView.Rows[i].FindControl("lbl_FixedSalary_YesNo");
                Label lbl_FixedAmount = (Label)GridView.Rows[i].FindControl("lbl_FixedAmount");
                Label lbl_fixedwagerate = (Label)GridView.Rows[i].FindControl("lbl_fixedwagerate");
                Label lbl_fixratesal = (Label)GridView.Rows[i].FindControl("lbl_fixratesal");

                Label lbl_DA_VDA = (Label)GridView.Rows[i].FindControl("lbl_DA_VDA");
                Label lbl_HRA = (Label)GridView.Rows[i].FindControl("lbl_HRA");
                Label lbl_Conv_Allowance = (Label)GridView.Rows[i].FindControl("lbl_Conv_Allowance");
                Label lbl_Medical_Allowance = (Label)GridView.Rows[i].FindControl("lbl_Medical_Allowance");
                Label lbl_Washing_Allowance = (Label)GridView.Rows[i].FindControl("lbl_Washing_Allowance");
                Label lbl_ATT_Allowance = (Label)GridView.Rows[i].FindControl("lbl_ATT_Allowance");
                Label lbl_SPCL_Allowance = (Label)GridView.Rows[i].FindControl("lbl_SPCL_Allowance");
                Label lbl_Misc_Earnings = (Label)GridView.Rows[i].FindControl("lbl_Misc_Earnings");

                Label lbl_Cur_Advance = (Label)GridView.Rows[i].FindControl("lbl_Cur_Advance");
                Label lbl_Cur_Fines = (Label)GridView.Rows[i].FindControl("lbl_Cur_Fines");
                Label lbl_Cur_Others = (Label)GridView.Rows[i].FindControl("lbl_Cur_Others");

                Label lbl_otherspay = (Label)GridView.Rows[i].FindControl("lbl_otherspay");

                Label lbl_basicsalary = (Label)GridView.Rows[i].FindControl("lbl_basicsalary");
                Label lbl_PFPay = (Label)GridView.Rows[i].FindControl("lbl_PFPay");
                Label lbl_otwages = (Label)GridView.Rows[i].FindControl("lbl_otwages");

                Label lbl_actualgross = (Label)GridView.Rows[i].FindControl("lbl_actualgross");
                Label lbl_grossamount = (Label)GridView.Rows[i].FindControl("lbl_grossamount");

                Label lbl_esicpay = (Label)GridView.Rows[i].FindControl("lbl_esicpay");
                Label lbl_netpay1 = (Label)GridView.Rows[i].FindControl("lbl_netpay1");
                Label lbl_netpay2 = (Label)GridView.Rows[i].FindControl("lbl_netpay2");

                Label lbl_DaVdaPay = (Label)GridView.Rows[i].FindControl("lbl_DaVdaPay");
                Label lbl_HRAPay = (Label)GridView.Rows[i].FindControl("lbl_HRAPay");
                Label lbl_ConvPay = (Label)GridView.Rows[i].FindControl("lbl_ConvPay");
                Label lbl_MedPay = (Label)GridView.Rows[i].FindControl("lbl_MedPay");
                Label lbl_WashPay = (Label)GridView.Rows[i].FindControl("lbl_WashPay");
                Label lbl_AttPay = (Label)GridView.Rows[i].FindControl("lbl_AttPay");
                Label lbl_SPCLPay = (Label)GridView.Rows[i].FindControl("lbl_SPCLPay");
                Label lbl_MiscPay = (Label)GridView.Rows[i].FindControl("lbl_MiscPay");

                Label lbl_ttlded = (Label)GridView.Rows[i].FindControl("lbl_ttlded");
                Label lbl_netpayfnl = (Label)GridView.Rows[i].FindControl("lbl_netpayfnl");

                // ========= RESET VALUE-HOLDER LABELS (per record) =========
                ResetToZero(
                    lbl_presents, lbl_flcount, lbl_flpcount, lbl_halfdaycount, lbl_nhpcount, lbl_nhcount,
                    lbl_oddayscount, lbl_presentdayscount, lbl_ttlot, lbl_payrate, lbl_fixedwagerate,
                    lbl_fixratesal, lbl_basicsalary, lbl_PFPay, lbl_otwages, lbl_actualgross,
                    lbl_grossamount, lbl_esicpay, lbl_netpay1, lbl_netpay2, lbl_otherspay,
                    lbl_DaVdaPay, lbl_HRAPay, lbl_ConvPay, lbl_MedPay, lbl_WashPay, lbl_AttPay,
                    lbl_SPCLPay, lbl_MiscPay, lbl_ttlded, lbl_netpayfnl
                );
                // ========= END RESET BLOCK =========

                TotalPresents = .0m;
                //----------------- Function call to find out the Total Present aganist the Employee Workman Sl----------------//
                string empwrk = lbl_WorkmanSL.Text.ToString();
                PayRoll.FindEmployeeTotalPresentByDates2_SP(date1, date2, empwrk, ref TotalPresents);
                lbl_presents.Text = TotalPresents.ToString();

                Dictionary<string, int> attendanceData = new Dictionary<string, int>();
                PayRoll.GetEmployeeAttendanceCounts(month.ToString(), year.ToString(), empwrk, out attendanceData);

                //lblAbsent.Text = attendanceData["Ab"].ToString();
                lbl_flcount.Text = attendanceData["FL"].ToString();
                lbl_flpcount.Text = attendanceData["FP"].ToString();
                lbl_halfdaycount.Text = attendanceData["HD"].ToString();
                //lbl_halfdaycount.Text = (Convert.ToDecimal(attendanceData["HD"]) * 0.5m).ToString();
                lbl_nhpcount.Text = attendanceData["HP"].ToString();
                lbl_nhcount.Text = attendanceData["NH"].ToString();
                lbl_oddayscount.Text = attendanceData["OD"].ToString();
                lbl_presentdayscount.Text = attendanceData["P"].ToString();
                //lbl_pendingcount.Text = attendanceData["Pending"].ToString();

                //----------------- Function call to find out the Total OverTime Unit aganist the Employee Workman Sl----------------//
                decimal TotalOT = .0m;
                PayRoll.FindEmployeeTotalOTByDates1(date1, date2, empwrk, ref TotalOT);
                lbl_ttlot.Text = TotalOT.ToString();

                //----------------- Function call to find out the Daily Pay Rate aganist the Employee Skill Category----------------//
                decimal dr = 0.0m;
                string skillevel = lbl_skill_category.Text.ToString();
                string workregion = lbl_WorkRegion.Text.ToString();
                PayRoll.FindPayCadre(skillevel, workregion, ref dr);
                lbl_payrate.Text = dr.ToString();

                if (workregion == "KPO")
                {
                    if (empwrk == "K68" || empwrk == "K91" || empwrk == "K92" || empwrk == "K579" || empwrk == "K584" || empwrk == "K612" || empwrk == "K620")
                    {
                        CalWorkingDaysF = 26;
                    }
                    else
                    {
                        CalWorkingDaysF = CalWorkingDays;
                    }
                }
                else
                {
                    CalWorkingDaysF = CalWorkingDays;
                }

                //----------------- Calculation for Employee who are in Fixed Salary----------------//
                string fixedyesno = lbl_FixedSalary_YesNo.Text.ToString();
                decimal fixedamount = Convert.ToDecimal(lbl_FixedAmount.Text.ToString());
                decimal fdr = Convert.ToDecimal(fixedamount) / Convert.ToDecimal(CalWorkingDaysF);
                decimal fnlfdr = Math.Round(fdr, 2);
                lbl_fixedwagerate.Text = fnlfdr.ToString();

                //------------ Wage of Fixed rate ---------------   ( FixedAmount / CalenderDays ) x  PresentDays
                decimal FixRateSalary = 0.0m;
                FixRateSalary = Math.Round(fdr * TotalPresents, 2);
                lbl_fixratesal.Text = FixRateSalary.ToString();

                //-------------Basic Salary or Basic Wages  -----------   Daily PayRate x Present Days
                decimal BasicSalary = 0.0m;
                PayRoll.BasicSalaryCalculation(TotalPresents, dr, ref BasicSalary);
                lbl_basicsalary.Text = BasicSalary.ToString();

                decimal DaVdaAmount = Convert.ToDecimal(lbl_DA_VDA.Text.ToString());
                decimal HRAAmount = Convert.ToDecimal(lbl_HRA.Text.ToString());
                decimal ConvAmount = Convert.ToDecimal(lbl_Conv_Allowance.Text.ToString());
                decimal MedAmount = Convert.ToDecimal(lbl_Medical_Allowance.Text.ToString());
                decimal WashAmount = Convert.ToDecimal(lbl_Washing_Allowance.Text.ToString());
                decimal AttAmount = Convert.ToDecimal(lbl_ATT_Allowance.Text.ToString());
                decimal SPCLAmount = Convert.ToDecimal(lbl_SPCL_Allowance.Text.ToString());
                decimal MiscAmount = Convert.ToDecimal(lbl_Misc_Earnings.Text.ToString());

                int AdvanceAmt = Convert.ToInt32(lbl_Cur_Advance.Text.ToString());
                int FinesAmt = Convert.ToInt32(lbl_Cur_Fines.Text.ToString());
                int OthersAmt = Convert.ToInt32(lbl_Cur_Others.Text.ToString());

                decimal DaVdaPay = .0m;
                decimal HRAPay = .0m;
                decimal ConvPay = .0m;
                decimal MedPay = .0m;
                decimal WashPay = .0m;
                decimal WashPayF = .0m;
                decimal AttPay = .0m;
                decimal SPCLPay = .0m;
                decimal MiscPay = .0m;

                //---------------- DA/VDA Alowances Cal-------------------------------------//
                PayRoll.EmployeeOthersPayCalculations1(TotalPresents, CalWorkingDaysF, DaVdaAmount, ref DaVdaPay);
                lbl_DaVdaPay.Text = DaVdaPay.ToString();

                //---------------- HRA Alowances Cal-------------------------------------//
                if (workregion == "RSP")
                {
                    decimal hramult = 0.05m;
                    HRAPay = Math.Ceiling(BasicSalary * hramult);
                    lbl_HRAPay.Text = HRAPay.ToString();
                }
                else
                {
                    PayRoll.EmployeeOthersPayCalculations2(TotalPresents, CalWorkingDaysF, HRAAmount, ref HRAPay);
                    lbl_HRAPay.Text = HRAPay.ToString();
                }

                //---------------- Conv Alowances Cal-------------------------------------//
                PayRoll.EmployeeOthersPayCalculations3(TotalPresents, CalWorkingDaysF, ConvAmount, ref ConvPay);
                lbl_ConvPay.Text = ConvPay.ToString();

                //---------------- Medical Alowances Cal-------------------------------------//
                PayRoll.EmployeeOthersPayCalculations4(TotalPresents, CalWorkingDaysF, MedAmount, ref MedPay);
                lbl_MedPay.Text = MedPay.ToString();
                WashPay = 0;
                WashPayF = 0;

                //---------------- Wash Alowances Cal-------------------------------------//
                PayRoll.EmployeeWashPayCalculations(TotalPresents, CalWorkingDaysF, WashBreak, ref WashPay);
                if (WashPay <= 0) { WashPayF = 0; } else { WashPayF = WashPay; }

                //---------------- Att Alowances Cal-------------------------------------//
                PayRoll.EmployeeOthersPayCalculations6(TotalPresents, CalWorkingDaysF, AttAmount, ref AttPay);
                lbl_AttPay.Text = AttPay.ToString();

                if (workregion == "RSP")
                {
                    decimal awamult = 157.69m;
                    SPCLPay = Math.Round(TotalPresents * awamult, 2);
                    lbl_SPCLPay.Text = SPCLPay.ToString();
                }
                else
                {
                    //---------------- SPCL Alowances Cal-------------------------------------//
                    PayRoll.EmployeeOthersPayCalculations7(TotalPresents, CalWorkingDaysF, SPCLAmount, ref SPCLPay);
                    lbl_SPCLPay.Text = SPCLPay.ToString();
                }

                //---------------- MISC Alowances Cal-------------------------------------//
                PayRoll.EmployeeOthersPayCalculations8(TotalPresents, CalWorkingDaysF, MiscAmount, ref MiscPay);
                lbl_MiscPay.Text = MiscPay.ToString();

                //--------------- PF Calucations --------------------//
                decimal PFPay = 0.0m;
                PayRoll.PFPayCalculation(BasicSalary, ref PFPay);
                lbl_PFPay.Text = PFPay.ToString();

                //---------------- OT Pay -------- Gross Rate
                decimal otpay = 0.0m;
                decimal wrkhrs = Convert.ToDecimal(lbl_WorkHours.Text.ToString());
                decimal otdiv = Convert.ToDecimal(lbl_OT_Divisibility.Text.ToString());  //Added on 29-11-2021
                decimal otfactor = Convert.ToDecimal(lbl_OTFactor.Text.ToString());
                string otrate = lbl_OTMultiplier.Text.ToString();

                decimal actualgross = 0.0m;
                decimal grossesic = 0.0m;
                decimal otherpay = 0.0m;
                decimal otherpayF = 0.0m;

                if (fixedyesno == "Yes")  ///Check whether the employee is in Fixed or Daily Rate Payroll
                {
                    if (otrate == "Gross")
                    {
                        if (TotalOT >= 1)
                        {
                            decimal otdays = TotalOT / otdiv;
                            // OTPay on FixedRate
                            otpay = Math.Round(fdr * otdays * otfactor, 0);
                            lbl_otwages.Text = otpay.ToString();
                        }
                        else
                        {
                            lbl_otwages.Text = otpay.ToString();
                        }
                        otherpay = FixRateSalary - BasicSalary - WashPayF;
                        actualgross = BasicSalary + otpay + otherpay + WashPayF;
                    }
                    else
                    {
                        if (TotalOT >= 1)
                        {
                            decimal otdays = TotalOT / otdiv;
                            //OTPay on DailyRate
                            otpay = Math.Round(dr * otdays * otfactor, 0);
                            lbl_otwages.Text = otpay.ToString();
                        }
                        else
                        {
                            lbl_otwages.Text = otpay.ToString();
                        }
                        otherpay = FixRateSalary - BasicSalary - WashPayF;

                        actualgross = BasicSalary + otpay + otherpay + WashPayF;
                    }
                    lbl_actualgross.Text = actualgross.ToString();
                }
                else
                {
                    if (otrate == "Gross")
                    {
                        if (TotalOT >= 1)
                        {
                            decimal otdays = TotalOT / otdiv;
                            // OTPay on FixedRate
                            otpay = Math.Round(fdr * otdays * otfactor, 0);
                            lbl_otwages.Text = otpay.ToString();
                        }
                        else
                        {
                            lbl_otwages.Text = otpay.ToString();
                        }
                        WashPayF = 0;
                        otherpay = 0;
                        actualgross = BasicSalary + otpay + otherpay + WashPayF;
                    }
                    else
                    {
                        if (TotalOT >= 1)
                        {
                            decimal otdays = TotalOT / otdiv;
                            //OTPay on DailyRate
                            otpay = Math.Round(dr * otdays * otfactor, 0);
                            lbl_otwages.Text = otpay.ToString();
                        }
                        else
                        {
                            lbl_otwages.Text = otpay.ToString();
                        }
                        WashPayF = 0;
                        otherpay = 0;
                        actualgross = BasicSalary + otpay + otherpay + WashPayF + HRAPay + SPCLPay;
                    }
                    lbl_actualgross.Text = actualgross.ToString();
                }

                //----------------Gross Calculation & NET Payment 2 -------------------------------//
                decimal washgross = actualgross + WashPayF + ConvPay;

                if (actualgross > GorssBreaker)
                {
                    grossesic = GorssBreaker - WashPayF;
                }
                else
                {
                    grossesic = actualgross - WashPayF;
                }

                lbl_grossamount.Text = grossesic.ToString();
                if (otherpay < 0)
                {
                    otherpayF = 0;
                }
                else
                {
                    otherpayF = otherpay;
                }
                lbl_otherspay.Text = otherpayF.ToString();
                lbl_WashPay.Text = WashPayF.ToString();
                //--------------------- ESIC pay---------------------------------------------------//
                decimal esicpay = 0.0m;
                esicpay = Math.Round(grossesic * 0.0075m, 0);
                lbl_esicpay.Text = esicpay.ToString();

                //--------------------- NET Payment -------------------------------------------------//
                decimal netpay1 = 0.0m;
                decimal newgross = grossesic;
                netpay1 = Math.Round(newgross - PFPay - esicpay + WashPayF + ConvPay, 0);

                //Here total deductions means --- Employee side deductions
                decimal ttldeductions = AdvanceAmt + FinesAmt + OthersAmt;
                lbl_ttlded.Text = ttldeductions.ToString();

                decimal netpay1final = .0m;
                decimal netpay2 = 0.0m;
                if (fixedyesno == "Yes")
                {
                    if (actualgross > WashBreak)
                    {
                        netpay2 = actualgross - newgross + HRAPay - WashPayF;
                    }
                    else
                    {
                        netpay2 = 0.0m;
                    }
                }
                else
                {
                    netpay2 = actualgross - netpay1 - PFPay - esicpay + HRAPay - WashPayF;
                }

                //to deduct max amount from Pay2, if it is greater then or equal to the total deduction amount
                decimal check = netpay2 - ttldeductions;
                decimal netpay2_finalaftrded = .0m;
                if (workregion == "KPO")
                {
                    if (check > 0)
                    {
                        netpay2_finalaftrded = netpay2 - ttldeductions;
                        netpay1final = netpay1;
                    }
                    else
                    {
                        netpay2_finalaftrded = netpay2;
                        netpay1final = netpay1 - ttldeductions;
                    }
                }
                else if (workregion == "AGL")
                {
                    netpay2_finalaftrded = netpay2;
                    netpay1final = netpay1 - ttldeductions;
                }
                else if (workregion == "NINL")
                {
                    netpay2_finalaftrded = netpay2;
                    netpay1final = netpay1 - ttldeductions;
                }
                else if (workregion == "JSR")
                {
                    netpay2_finalaftrded = netpay2;
                    netpay1final = netpay1 - ttldeductions;
                }
                else if (workregion == "RSP")
                {
                    netpay2_finalaftrded = netpay2 - HRAPay;
                    netpay1final = netpay1 - ttldeductions;
                }
                else
                {
                    netpay2_finalaftrded = netpay2;
                    netpay1final = netpay1 - ttldeductions;
                }

                lbl_netpay1.Text = netpay1.ToString();
                lbl_netpayfnl.Text = netpay1final.ToString();
                lbl_netpay2.Text = netpay2_finalaftrded.ToString();///---1
            }

            //btnExport.Enabled = true;
        }


        protected void btn_excelexport_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AppendHeader("content-disposition", "attachement;filename=" + DDL_Region.SelectedValue.ToString() + "_F17_Trial.xls");
            Response.ContentType = "application/excell";

            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

            GridView.HeaderRow.Style.Add("background-colour", "Skyblue");
            foreach (TableCell tabelCell in GridView.HeaderRow.Cells)
            {
                tabelCell.Style["background-colour"] = "#A55129";
            }
            foreach (GridViewRow gridViewRow in GridView.Rows)
            {
                gridViewRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell gridViewRowTableCell in gridViewRow.Cells)
                {
                    gridViewRowTableCell.Style["background-color"] = "white";
                }
            }

            GridView.RenderControl(htmlTextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        protected void btnInsertDB_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LogerName", typeof(string));
            dt.Columns.Add("LogerWrk", typeof(string));
            dt.Columns.Add("Region", typeof(string));
            dt.Columns.Add("Company", typeof(string));
            dt.Columns.Add("SalaryYear", typeof(string));
            dt.Columns.Add("SalaryMonth", typeof(string));
            dt.Columns.Add("SalaryStartDay", typeof(string));
            dt.Columns.Add("SalaryEndDay", typeof(string));
            dt.Columns.Add("CalendayDays", typeof(string));
            dt.Columns.Add("WorkmanSL", typeof(string));
            dt.Columns.Add("WorkRegion", typeof(string));
            dt.Columns.Add("FullName", typeof(string));
            dt.Columns.Add("SkillCategory", typeof(string));
            dt.Columns.Add("PayRate", typeof(decimal));
            dt.Columns.Add("SkillDesignation", typeof(string));
            dt.Columns.Add("FixedSalary_YesNo", typeof(string));
            dt.Columns.Add("FixedAmount", typeof(decimal));
            dt.Columns.Add("FixedWageRate", typeof(decimal));
            dt.Columns.Add("WorkHours", typeof(Int32));
            dt.Columns.Add("OTFactor", typeof(Int32));
            dt.Columns.Add("OTMultiplier", typeof(string));
            dt.Columns.Add("DA_VDA", typeof(decimal));
            dt.Columns.Add("HRA", typeof(decimal));
            dt.Columns.Add("Conv_Allowance", typeof(decimal));
            dt.Columns.Add("Medical_Allowance", typeof(decimal));
            dt.Columns.Add("Washing_Allowance", typeof(decimal));
            dt.Columns.Add("ATT_Allowance", typeof(decimal));
            dt.Columns.Add("SPCL_Allowance", typeof(decimal));
            dt.Columns.Add("Misc_Earnings", typeof(decimal));
            dt.Columns.Add("Present", typeof(decimal));

            //dt.Columns.Add("Absent", typeof(decimal));
            dt.Columns.Add("FL", typeof(decimal));
            dt.Columns.Add("FP", typeof(decimal));
            dt.Columns.Add("HalfDay", typeof(decimal));
            dt.Columns.Add("HP", typeof(decimal));
            dt.Columns.Add("NH", typeof(decimal));
            dt.Columns.Add("OD", typeof(decimal));
            dt.Columns.Add("P", typeof(decimal));
            //dt.Columns.Add("Pending", typeof(decimal));

            dt.Columns.Add("OverTime", typeof(decimal));
            dt.Columns.Add("BasicSalary", typeof(decimal));
            dt.Columns.Add("FixedRateSalary", typeof(decimal));
            dt.Columns.Add("OTSalary", typeof(decimal));
            dt.Columns.Add("DaVdaPay", typeof(decimal));
            dt.Columns.Add("HRAPay", typeof(decimal));
            dt.Columns.Add("ConvPay", typeof(decimal));
            dt.Columns.Add("MedPay", typeof(decimal));
            dt.Columns.Add("WashPay", typeof(decimal));
            dt.Columns.Add("AttPay", typeof(decimal));
            dt.Columns.Add("SPCLPay", typeof(decimal));
            dt.Columns.Add("MiscPay", typeof(decimal));
            dt.Columns.Add("OthersPay", typeof(decimal));
            dt.Columns.Add("ActualGross", typeof(decimal));
            dt.Columns.Add("ESICGross", typeof(decimal));
            dt.Columns.Add("PFPay", typeof(decimal));
            dt.Columns.Add("ESICPay", typeof(decimal));
            dt.Columns.Add("NetPay1", typeof(decimal));

            dt.Columns.Add("Advance", typeof(decimal));
            dt.Columns.Add("Fines", typeof(decimal));
            dt.Columns.Add("Others", typeof(decimal));
            dt.Columns.Add("TotalDeductions", typeof(decimal));
            dt.Columns.Add("NetPayFinal", typeof(decimal));

            dt.Columns.Add("NetPay2", typeof(decimal));

            int k = 0;
            for (int i = 0; i <= GridView.Rows.Count - 1; i++)
            {
                string LogerName = Session["USERNAME"].ToString();
                string LogerWrk = Session["WORKMAN"].ToString();
                string Region = DDL_Region.SelectedValue.ToString();
                string Company = DDL_Company.SelectedValue.ToString();
                string SalaryYear = DDL_Year.SelectedItem.Text.ToString();
                string SalaryMonth = DDL_Month.SelectedValue.ToString();
                string SalaryStartDay = startday;
                string SalaryEndDay = endday;
                string CalendayDays = CalWorkingDays.ToString();

                Label lbl_WorkmanSL = (Label)GridView.Rows[i].FindControl("lbl_WorkmanSL");
                string WorkmanSL = lbl_WorkmanSL.Text.ToString();

                Label lbl_WorkRegion = (Label)GridView.Rows[i].FindControl("lbl_WorkRegion");
                string WorkRegion = lbl_WorkRegion.Text.ToString();

                Label lbl_FullName = (Label)GridView.Rows[i].FindControl("lbl_FullName");
                string FullName = lbl_FullName.Text.ToString();

                Label lbl_SkillCategory = (Label)GridView.Rows[i].FindControl("lbl_SkillCategory");
                string SkillCategory = lbl_SkillCategory.Text.ToString();

                Label lbl_payrate = (Label)GridView.Rows[i].FindControl("lbl_payrate");
                decimal PayRate = Convert.ToDecimal(lbl_payrate.Text.ToString());

                Label lbl_SkillDesignation = (Label)GridView.Rows[i].FindControl("lbl_SkillDesignation");
                string SkillDesignation = lbl_SkillDesignation.Text.ToString();

                Label lbl_FixedSalary_YesNo = (Label)GridView.Rows[i].FindControl("lbl_FixedSalary_YesNo");
                string FixedSalary_YesNo = lbl_FixedSalary_YesNo.Text.ToString();

                Label lbl_FixedAmount = (Label)GridView.Rows[i].FindControl("lbl_FixedAmount");
                decimal FixedAmount = Convert.ToDecimal(lbl_FixedAmount.Text.ToString());

                Label lbl_fixedwagerate = (Label)GridView.Rows[i].FindControl("lbl_fixedwagerate");
                decimal FixedWageRate = Convert.ToDecimal(lbl_fixedwagerate.Text.ToString());

                Label lbl_WorkHours = (Label)GridView.Rows[i].FindControl("lbl_WorkHours");
                Int32 WorkHours = Convert.ToInt32(lbl_WorkHours.Text.ToString());

                Label lbl_OTFactor = (Label)GridView.Rows[i].FindControl("lbl_OTFactor");
                Int32 OTFactor = Convert.ToInt32(lbl_OTFactor.Text.ToString());

                Label lbl_OTMultiplier = (Label)GridView.Rows[i].FindControl("lbl_OTMultiplier");
                string OTMultiplier = lbl_OTMultiplier.Text.ToString();

                Label lbl_DA_VDA = (Label)GridView.Rows[i].FindControl("lbl_DA_VDA");
                decimal DA_VDA = Convert.ToDecimal(lbl_DA_VDA.Text.ToString());

                Label lbl_HRA = (Label)GridView.Rows[i].FindControl("lbl_HRA");
                decimal HRA = Convert.ToDecimal(lbl_HRA.Text.ToString());

                Label lbl_Conv_Allowance = (Label)GridView.Rows[i].FindControl("lbl_Conv_Allowance");
                decimal Conv_Allowance = Convert.ToDecimal(lbl_Conv_Allowance.Text.ToString());

                Label lbl_Medical_Allowance = (Label)GridView.Rows[i].FindControl("lbl_Medical_Allowance");
                decimal Medical_Allowance = Convert.ToDecimal(lbl_Medical_Allowance.Text.ToString());

                Label lbl_Washing_Allowance = (Label)GridView.Rows[i].FindControl("lbl_Washing_Allowance");
                decimal Washing_Allowance = Convert.ToDecimal(lbl_Washing_Allowance.Text.ToString());

                Label lbl_ATT_Allowance = (Label)GridView.Rows[i].FindControl("lbl_ATT_Allowance");
                decimal ATT_Allowance = Convert.ToDecimal(lbl_ATT_Allowance.Text.ToString());

                Label lbl_SPCL_Allowance = (Label)GridView.Rows[i].FindControl("lbl_SPCL_Allowance");
                decimal SPCL_Allowance = Convert.ToDecimal(lbl_SPCL_Allowance.Text.ToString());

                Label lbl_Misc_Earnings = (Label)GridView.Rows[i].FindControl("lbl_Misc_Earnings");
                decimal Misc_Earnings = Convert.ToDecimal(lbl_Misc_Earnings.Text.ToString());

                Label lbl_presents = (Label)GridView.Rows[i].FindControl("lbl_presents");
                decimal ttlPresent = Convert.ToDecimal(lbl_presents.Text.ToString());

                //Label lbl_absent = (Label)GridView.Rows[i].FindControl("lbl_absent");
                //decimal Absent = Convert.ToDecimal(lbl_absent.Text);

                Label lbl_flcount = (Label)GridView.Rows[i].FindControl("lbl_flcount");
                decimal FL = Convert.ToDecimal(lbl_flcount.Text);

                Label lbl_flpcount = (Label)GridView.Rows[i].FindControl("lbl_flpcount");
                decimal FP = Convert.ToDecimal(lbl_flpcount.Text);

                Label lbl_halfdaycount = (Label)GridView.Rows[i].FindControl("lbl_halfdaycount");
                decimal HalfDay = Convert.ToDecimal(lbl_halfdaycount.Text);

                Label lbl_nhpcount = (Label)GridView.Rows[i].FindControl("lbl_nhpcount");
                decimal HP = Convert.ToDecimal(lbl_nhpcount.Text);

                Label lbl_nhcount = (Label)GridView.Rows[i].FindControl("lbl_nhcount");
                decimal NH = Convert.ToDecimal(lbl_nhcount.Text);

                Label lbl_oddayscount = (Label)GridView.Rows[i].FindControl("lbl_oddayscount");
                decimal OD = Convert.ToDecimal(lbl_oddayscount.Text);

                Label lbl_presentdayscount = (Label)GridView.Rows[i].FindControl("lbl_presentdayscount");
                decimal Present = Convert.ToDecimal(lbl_presentdayscount.Text);

                //Label lbl_pendingcount = (Label)GridView.Rows[i].FindControl("lbl_pendingcount");
                //decimal Pending = Convert.ToDecimal(lbl_pendingcount.Text);

                Label lbl_ttlot = (Label)GridView.Rows[i].FindControl("lbl_ttlot");
                decimal OverTime = Convert.ToDecimal(lbl_ttlot.Text.ToString());

                Label lbl_basicsalary = (Label)GridView.Rows[i].FindControl("lbl_basicsalary");
                decimal BasicSalary = Convert.ToDecimal(lbl_basicsalary.Text.ToString());

                Label lbl_fixratesal = (Label)GridView.Rows[i].FindControl("lbl_fixratesal");
                decimal FixedRateSalary = Convert.ToDecimal(lbl_fixratesal.Text.ToString());

                Label lbl_otwages = (Label)GridView.Rows[i].FindControl("lbl_otwages");
                decimal OTSalary = Convert.ToDecimal(lbl_otwages.Text.ToString());

                Label lbl_DaVdaPay = (Label)GridView.Rows[i].FindControl("lbl_DaVdaPay");
                decimal DaVdaPay = Convert.ToDecimal(lbl_DaVdaPay.Text.ToString());

                Label lbl_HRAPay = (Label)GridView.Rows[i].FindControl("lbl_HRAPay");
                decimal HRAPay = Convert.ToDecimal(lbl_HRAPay.Text.ToString());

                Label lbl_ConvPay = (Label)GridView.Rows[i].FindControl("lbl_ConvPay");
                decimal ConvPay = Convert.ToDecimal(lbl_ConvPay.Text.ToString());

                Label lbl_MedPay = (Label)GridView.Rows[i].FindControl("lbl_MedPay");
                decimal MedPay = Convert.ToDecimal(lbl_MedPay.Text.ToString());

                Label lbl_WashPay = (Label)GridView.Rows[i].FindControl("lbl_WashPay");
                decimal WashPay = Convert.ToDecimal(lbl_WashPay.Text.ToString());

                Label lbl_AttPay = (Label)GridView.Rows[i].FindControl("lbl_AttPay");
                decimal AttPay = Convert.ToDecimal(lbl_AttPay.Text.ToString());

                Label lbl_SPCLPay = (Label)GridView.Rows[i].FindControl("lbl_SPCLPay");
                decimal SPCLPay = Convert.ToDecimal(lbl_SPCLPay.Text.ToString());

                Label lbl_MiscPay = (Label)GridView.Rows[i].FindControl("lbl_MiscPay");
                decimal MiscPay = Convert.ToDecimal(lbl_MiscPay.Text.ToString());

                Label lbl_others = (Label)GridView.Rows[i].FindControl("lbl_otherspay");
                decimal OthersPay = Convert.ToDecimal(lbl_others.Text.ToString());

                Label lbl_actualgross = (Label)GridView.Rows[i].FindControl("lbl_actualgross");
                decimal ActualGross = Convert.ToDecimal(lbl_actualgross.Text.ToString());

                Label lbl_grossamount = (Label)GridView.Rows[i].FindControl("lbl_grossamount");
                decimal ESICGross = Convert.ToDecimal(lbl_grossamount.Text.ToString());

                Label lbl_PFPay = (Label)GridView.Rows[i].FindControl("lbl_PFPay");
                decimal PFPay = Convert.ToDecimal(lbl_PFPay.Text.ToString());

                Label lbl_esicpay = (Label)GridView.Rows[i].FindControl("lbl_esicpay");
                decimal ESICPay = Convert.ToDecimal(lbl_esicpay.Text.ToString());

                Label lbl_netpay1 = (Label)GridView.Rows[i].FindControl("lbl_netpay1");
                decimal NetPay1 = Convert.ToDecimal(lbl_netpay1.Text.ToString());

                //Added on 06-03-2022

                Label lbl_Cur_Advance = (Label)GridView.Rows[i].FindControl("lbl_Cur_Advance");
                decimal Advance = Convert.ToDecimal(lbl_Cur_Advance.Text.ToString());

                Label lbl_Cur_Fines = (Label)GridView.Rows[i].FindControl("lbl_Cur_Fines");
                decimal Fines = Convert.ToDecimal(lbl_Cur_Fines.Text.ToString());

                Label lbl_Cur_Others = (Label)GridView.Rows[i].FindControl("lbl_Cur_Others");
                decimal Others = Convert.ToDecimal(lbl_Cur_Others.Text.ToString());

                Label lbl_ttlded = (Label)GridView.Rows[i].FindControl("lbl_ttlded");
                decimal TTLDed = Convert.ToDecimal(lbl_ttlded.Text.ToString());

                Label lbl_netpayfnl = (Label)GridView.Rows[i].FindControl("lbl_netpayfnl");
                decimal NetPayFinal = Convert.ToDecimal(lbl_netpayfnl.Text.ToString());

                Label lbl_netpay2 = (Label)GridView.Rows[i].FindControl("lbl_netpay2");
                decimal NetPay2 = Convert.ToDecimal(lbl_netpay2.Text.ToString());



                try
                {
                    dbcl.Sqlconnection();
                    SqlCommand cmd = new SqlCommand("SP_InsertIntoEmployeesMonthlyForm17Trail", dbcl.Conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("LogerName", LogerName);
                    cmd.Parameters.AddWithValue("LogerWrk", LogerWrk);
                    cmd.Parameters.AddWithValue("Region", Region);
                    cmd.Parameters.AddWithValue("Company", Company);
                    cmd.Parameters.AddWithValue("SalaryYear", SalaryYear);
                    cmd.Parameters.AddWithValue("SalaryMonth", SalaryMonth);
                    cmd.Parameters.AddWithValue("SalaryStartDay", SalaryStartDay);
                    cmd.Parameters.AddWithValue("SalaryEndDay", SalaryEndDay);
                    cmd.Parameters.AddWithValue("CalendayDays", CalendayDays);
                    cmd.Parameters.AddWithValue("WorkmanSL", WorkmanSL);
                    cmd.Parameters.AddWithValue("WorkRegion", WorkRegion);
                    cmd.Parameters.AddWithValue("FullName", FullName);
                    cmd.Parameters.AddWithValue("SkillCategory", SkillCategory);
                    cmd.Parameters.AddWithValue("PayRate", PayRate);
                    cmd.Parameters.AddWithValue("SkillDesignation", SkillDesignation);
                    cmd.Parameters.AddWithValue("FixedSalary_YesNo", FixedSalary_YesNo);
                    cmd.Parameters.AddWithValue("FixedAmount", FixedAmount);
                    cmd.Parameters.AddWithValue("FixedWageRate", FixedWageRate);
                    cmd.Parameters.AddWithValue("WorkHours", WorkHours);
                    cmd.Parameters.AddWithValue("OTFactor", OTFactor);
                    cmd.Parameters.AddWithValue("OTMultiplier", OTMultiplier);
                    cmd.Parameters.AddWithValue("DA_VDA", DA_VDA);
                    cmd.Parameters.AddWithValue("HRA", HRA);
                    cmd.Parameters.AddWithValue("Conv_Allowance", Conv_Allowance);
                    cmd.Parameters.AddWithValue("Medical_Allowance", Medical_Allowance);
                    cmd.Parameters.AddWithValue("ATT_Allowance", ATT_Allowance);
                    cmd.Parameters.AddWithValue("SPCL_Allowance", SPCL_Allowance);
                    cmd.Parameters.AddWithValue("Misc_Earnings", Misc_Earnings);
                    cmd.Parameters.AddWithValue("Washing_Allowance", Washing_Allowance);
                    cmd.Parameters.AddWithValue("Present", ttlPresent);
                    
                    //cmd.Parameters.AddWithValue("Absent", SqlDbType.Decimal).Value = Absent;
                    //cmd.Parameters.AddWithValue("Pending", SqlDbType.Decimal).Value = Pending;

                    cmd.Parameters.AddWithValue("P_FL", SqlDbType.Decimal).Value = FL;
                    cmd.Parameters.AddWithValue("P_FP", SqlDbType.Decimal).Value = FP;
                    cmd.Parameters.AddWithValue("P_HD", SqlDbType.Decimal).Value = HalfDay;
                    cmd.Parameters.AddWithValue("P_HP", SqlDbType.Decimal).Value = HP;
                    cmd.Parameters.AddWithValue("P_NH", SqlDbType.Decimal).Value = NH;
                    cmd.Parameters.AddWithValue("P_OD", SqlDbType.Decimal).Value = OD;
                    cmd.Parameters.AddWithValue("P_P", SqlDbType.Decimal).Value = Present;

                    cmd.Parameters.AddWithValue("OverTime", OverTime);
                    cmd.Parameters.AddWithValue("BasicSalary", BasicSalary);
                    cmd.Parameters.AddWithValue("FixedRateSalary", FixedRateSalary);
                    cmd.Parameters.AddWithValue("OTSalary", OTSalary);
                    cmd.Parameters.AddWithValue("DaVdaPay", DaVdaPay);
                    cmd.Parameters.AddWithValue("HRAPay", HRAPay);
                    cmd.Parameters.AddWithValue("ConvPay", ConvPay);
                    cmd.Parameters.AddWithValue("MedPay", MedPay);
                    cmd.Parameters.AddWithValue("WashPay", WashPay);
                    cmd.Parameters.AddWithValue("AttPay", AttPay);
                    cmd.Parameters.AddWithValue("SPCLPay", SPCLPay);
                    cmd.Parameters.AddWithValue("MiscPay", MiscPay);
                    cmd.Parameters.AddWithValue("OthersPay", OthersPay);
                    cmd.Parameters.AddWithValue("ActualGross", ActualGross);
                    cmd.Parameters.AddWithValue("ESICGross", ESICGross);
                    cmd.Parameters.AddWithValue("PFPay", PFPay);
                    cmd.Parameters.AddWithValue("ESICPay", ESICPay);
                    cmd.Parameters.AddWithValue("NetPay1", NetPay1);

                    cmd.Parameters.AddWithValue("Advance", Advance);
                    cmd.Parameters.AddWithValue("Fines", Fines);
                    cmd.Parameters.AddWithValue("Others", Others);
                    cmd.Parameters.AddWithValue("TotalDeduction", TTLDed);
                    cmd.Parameters.AddWithValue("NetPayFinal", NetPayFinal);
                    cmd.Parameters.AddWithValue("NetPay2", NetPay2);

                    dbcl.ConnectDb();
                    k = cmd.ExecuteNonQuery();
                    dbcl.DisconnectDb();

                    StatusUpdater();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    //throw;
                }
            }

            if (k != 0)
            {
                //Img_Insersuccess.Visible = true;
                //lbl_insertsuccess.Visible = true;
                //lbl_insertsuccess.Text = "Record Inserted Succesfully into the Database";
                //lbl_insertsuccess.ForeColor = System.Drawing.Color.DarkGreen;
                //UpdateForm17Status(Year, Month);

                btnInsertDB.Enabled = false;
                btnInsertDB.Text = "Finalized";
                btnInsertDB.CssClass = "btn btn-success btn-sm";
            }
            else
            {
                ////Img_Insersuccess.Visible = true;
                //lbl_insertsuccess.Visible = true;
                //lbl_insertsuccess.Text = "Records Connot be Inserted into the Database";
                //lbl_insertsuccess.ForeColor = System.Drawing.Color.IndianRed;
            }
        }
        //private void DTTableInsert()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("LogerName", typeof(string));
        //    dt.Columns.Add("LogerWrk", typeof(string));
        //    dt.Columns.Add("Region", typeof(string));
        //    dt.Columns.Add("Company", typeof(string));
        //    dt.Columns.Add("SalaryYear", typeof(string));
        //    dt.Columns.Add("SalaryMonth", typeof(string));
        //    dt.Columns.Add("SalaryStartDay", typeof(string));
        //    dt.Columns.Add("SalaryEndDay", typeof(string));
        //    dt.Columns.Add("CalendayDays", typeof(string));
        //    dt.Columns.Add("WorkmanSL", typeof(string));
        //    dt.Columns.Add("WorkRegion", typeof(string));
        //    dt.Columns.Add("FullName", typeof(string));
        //    dt.Columns.Add("SkillCategory", typeof(string));
        //    dt.Columns.Add("PayRate", typeof(decimal));
        //    dt.Columns.Add("SkillDesignation", typeof(string));
        //    dt.Columns.Add("FixedSalary_YesNo", typeof(string));
        //    dt.Columns.Add("FixedAmount", typeof(decimal));
        //    dt.Columns.Add("FixedWageRate", typeof(decimal));
        //    dt.Columns.Add("WorkHours", typeof(Int32));
        //    dt.Columns.Add("OTFactor", typeof(Int32));
        //    dt.Columns.Add("OTMultiplier", typeof(string));
        //    dt.Columns.Add("DA_VDA", typeof(decimal));
        //    dt.Columns.Add("HRA", typeof(decimal));
        //    dt.Columns.Add("Conv_Allowance", typeof(decimal));
        //    dt.Columns.Add("Medical_Allowance", typeof(decimal));
        //    dt.Columns.Add("Washing_Allowance", typeof(decimal));
        //    dt.Columns.Add("ATT_Allowance", typeof(decimal));
        //    dt.Columns.Add("SPCL_Allowance", typeof(decimal));
        //    dt.Columns.Add("Misc_Earnings", typeof(decimal));
        //    dt.Columns.Add("Present", typeof(Int32));
        //    dt.Columns.Add("OverTime", typeof(decimal));
        //    dt.Columns.Add("BasicSalary", typeof(decimal));
        //    dt.Columns.Add("FixedRateSalary", typeof(decimal));
        //    dt.Columns.Add("OTSalary", typeof(decimal));
        //    dt.Columns.Add("DaVdaPay", typeof(decimal));
        //    dt.Columns.Add("HRAPay", typeof(decimal));
        //    dt.Columns.Add("ConvPay", typeof(decimal));
        //    dt.Columns.Add("MedPay", typeof(decimal));
        //    dt.Columns.Add("WashPay", typeof(decimal));
        //    dt.Columns.Add("AttPay", typeof(decimal));
        //    dt.Columns.Add("SPCLPay", typeof(decimal));
        //    dt.Columns.Add("MiscPay", typeof(decimal));
        //    dt.Columns.Add("OthersPay", typeof(decimal));
        //    dt.Columns.Add("ActualGross", typeof(decimal));
        //    dt.Columns.Add("ESICGross", typeof(decimal));
        //    dt.Columns.Add("PFPay", typeof(decimal));
        //    dt.Columns.Add("ESICPay", typeof(decimal));
        //    dt.Columns.Add("NetPay1", typeof(decimal));
        //    dt.Columns.Add("NetPay2", typeof(decimal));

        //    for (int i = 0; i <= GridView.Rows.Count - 1; i++)
        //    {
        //        string LogerName = Session["USERNAME"].ToString();
        //        string LogerWrk = Session["WORKMAN"].ToString();
        //        string Region = DDL_Region.SelectedValue.ToString();
        //        string Company = DDL_Company.SelectedValue.ToString();
        //        string SalaryYear = DDL_Year.SelectedItem.Text.ToString();
        //        string SalaryMonth = DDL_Month.SelectedItem.Text.ToString();
        //        string SalaryStartDay = DDL_Day.SelectedItem.Text.ToString();
        //        string SalaryEndDay = DDL_D2.SelectedItem.Text.ToString();
        //        string CalendayDays = DDL_Days.SelectedItem.Text.ToString();

        //        Label lbl_WorkmanSL = (Label)GridView.Rows[i].FindControl("lbl_WorkmanSL");
        //        string WorkmanSL = lbl_WorkmanSL.Text.ToString();

        //        Label lbl_WorkRegion = (Label)GridView.Rows[i].FindControl("lbl_WorkRegion");
        //        string WorkRegion = lbl_WorkRegion.Text.ToString();

        //        Label lbl_FullName = (Label)GridView.Rows[i].FindControl("lbl_FullName");
        //        string FullName = lbl_FullName.Text.ToString();

        //        Label lbl_SkillCategory = (Label)GridView.Rows[i].FindControl("lbl_SkillCategory");
        //        string SkillCategory = lbl_SkillCategory.Text.ToString();

        //        Label lbl_payrate = (Label)GridView.Rows[i].FindControl("lbl_payrate");
        //        decimal PayRate = Convert.ToDecimal(lbl_payrate.Text.ToString());

        //        Label lbl_SkillDesignation = (Label)GridView.Rows[i].FindControl("lbl_SkillDesignation");
        //        string SkillDesignation = lbl_SkillDesignation.Text.ToString();

        //        Label lbl_FixedSalary_YesNo = (Label)GridView.Rows[i].FindControl("lbl_FixedSalary_YesNo");
        //        string FixedSalary_YesNo = lbl_FixedSalary_YesNo.Text.ToString();

        //        Label lbl_FixedAmount = (Label)GridView.Rows[i].FindControl("lbl_FixedAmount");
        //        decimal FixedAmount = Convert.ToDecimal(lbl_FixedAmount.Text.ToString());

        //        Label lbl_fixedwagerate = (Label)GridView.Rows[i].FindControl("lbl_fixedwagerate");
        //        decimal FixedWageRate = Convert.ToDecimal(lbl_fixedwagerate.Text.ToString());

        //        Label lbl_WorkHours = (Label)GridView.Rows[i].FindControl("lbl_WorkHours");
        //        Int32 WorkHours = Convert.ToInt32(lbl_WorkHours.Text.ToString());

        //        Label lbl_OTFactor = (Label)GridView.Rows[i].FindControl("lbl_OTFactor");
        //        Int32 OTFactor = Convert.ToInt32(lbl_OTFactor.Text.ToString());

        //        Label lbl_OTMultiplier = (Label)GridView.Rows[i].FindControl("lbl_OTMultiplier");
        //        string OTMultiplier = lbl_OTMultiplier.Text.ToString();

        //        Label lbl_DA_VDA = (Label)GridView.Rows[i].FindControl("lbl_DA_VDA");
        //        decimal DA_VDA = Convert.ToDecimal(lbl_DA_VDA.Text.ToString());

        //        Label lbl_HRA = (Label)GridView.Rows[i].FindControl("lbl_HRA");
        //        decimal HRA = Convert.ToDecimal(lbl_HRA.Text.ToString());

        //        Label lbl_Conv_Allowance = (Label)GridView.Rows[i].FindControl("lbl_Conv_Allowance");
        //        decimal Conv_Allowance = Convert.ToDecimal(lbl_Conv_Allowance.Text.ToString());

        //        Label lbl_Medical_Allowance = (Label)GridView.Rows[i].FindControl("lbl_Medical_Allowance");
        //        decimal Medical_Allowance = Convert.ToDecimal(lbl_Medical_Allowance.Text.ToString());

        //        Label lbl_Washing_Allowance = (Label)GridView.Rows[i].FindControl("lbl_Washing_Allowance");
        //        decimal Washing_Allowance = Convert.ToDecimal(lbl_Washing_Allowance.Text.ToString());

        //        Label lbl_ATT_Allowance = (Label)GridView.Rows[i].FindControl("lbl_ATT_Allowance");
        //        decimal ATT_Allowance = Convert.ToDecimal(lbl_ATT_Allowance.Text.ToString());

        //        Label lbl_SPCL_Allowance = (Label)GridView.Rows[i].FindControl("lbl_SPCL_Allowance");
        //        decimal SPCL_Allowance = Convert.ToDecimal(lbl_SPCL_Allowance.Text.ToString());

        //        Label lbl_Misc_Earnings = (Label)GridView.Rows[i].FindControl("lbl_Misc_Earnings");
        //        decimal Misc_Earnings = Convert.ToDecimal(lbl_Misc_Earnings.Text.ToString());

        //        Label lbl_presents = (Label)GridView.Rows[i].FindControl("lbl_presents");
        //        Int32 Present = Convert.ToInt32(lbl_presents.Text.ToString());

        //        Label lbl_ttlot = (Label)GridView.Rows[i].FindControl("lbl_ttlot");
        //        decimal OverTime = Convert.ToDecimal(lbl_ttlot.Text.ToString());

        //        Label lbl_basicsalary = (Label)GridView.Rows[i].FindControl("lbl_basicsalary");
        //        decimal BasicSalary = Convert.ToDecimal(lbl_basicsalary.Text.ToString());

        //        Label lbl_fixratesal = (Label)GridView.Rows[i].FindControl("lbl_fixratesal");
        //        decimal FixedRateSalary = Convert.ToDecimal(lbl_fixratesal.Text.ToString());

        //        Label lbl_otwages = (Label)GridView.Rows[i].FindControl("lbl_otwages");
        //        decimal OTSalary = Convert.ToDecimal(lbl_otwages.Text.ToString());

        //        Label lbl_DaVdaPay = (Label)GridView.Rows[i].FindControl("lbl_DaVdaPay");
        //        decimal DaVdaPay = Convert.ToDecimal(lbl_DaVdaPay.Text.ToString());

        //        Label lbl_HRAPay = (Label)GridView.Rows[i].FindControl("lbl_HRAPay");
        //        decimal HRAPay = Convert.ToDecimal(lbl_HRAPay.Text.ToString());

        //        Label lbl_ConvPay = (Label)GridView.Rows[i].FindControl("lbl_ConvPay");
        //        decimal ConvPay = Convert.ToDecimal(lbl_ConvPay.Text.ToString());

        //        Label lbl_MedPay = (Label)GridView.Rows[i].FindControl("lbl_MedPay");
        //        decimal MedPay = Convert.ToDecimal(lbl_MedPay.Text.ToString());

        //        Label lbl_WashPay = (Label)GridView.Rows[i].FindControl("lbl_WashPay");
        //        decimal WashPay = Convert.ToDecimal(lbl_WashPay.Text.ToString());

        //        Label lbl_AttPay = (Label)GridView.Rows[i].FindControl("lbl_AttPay");
        //        decimal AttPay = Convert.ToDecimal(lbl_AttPay.Text.ToString());

        //        Label lbl_SPCLPay = (Label)GridView.Rows[i].FindControl("lbl_SPCLPay");
        //        decimal SPCLPay = Convert.ToDecimal(lbl_SPCLPay.Text.ToString());

        //        Label lbl_MiscPay = (Label)GridView.Rows[i].FindControl("lbl_MiscPay");
        //        decimal MiscPay = Convert.ToDecimal(lbl_MiscPay.Text.ToString());

        //        Label lbl_others = (Label)GridView.Rows[i].FindControl("lbl_others");
        //        decimal OthersPay = Convert.ToDecimal(lbl_others.Text.ToString());

        //        Label lbl_actualgross = (Label)GridView.Rows[i].FindControl("lbl_actualgross");
        //        decimal ActualGross = Convert.ToDecimal(lbl_actualgross.Text.ToString());

        //        Label lbl_grossamount = (Label)GridView.Rows[i].FindControl("lbl_grossamount");
        //        decimal ESICGross = Convert.ToDecimal(lbl_grossamount.Text.ToString());

        //        Label lbl_PFPay = (Label)GridView.Rows[i].FindControl("lbl_PFPay");
        //        decimal PFPay = Convert.ToDecimal(lbl_PFPay.Text.ToString());

        //        Label lbl_esicpay = (Label)GridView.Rows[i].FindControl("lbl_esicpay");
        //        decimal ESICPay = Convert.ToDecimal(lbl_esicpay.Text.ToString());

        //        Label lbl_netpay1 = (Label)GridView.Rows[i].FindControl("lbl_netpay1");
        //        decimal NetPay1 = Convert.ToDecimal(lbl_netpay1.Text.ToString());

        //        Label lbl_netpay2 = (Label)GridView.Rows[i].FindControl("lbl_netpay2");
        //        decimal NetPay2 = Convert.ToDecimal(lbl_netpay2.Text.ToString());

        //        dt.Rows.Add(LogerName, LogerWrk, Region, Company, SalaryYear, SalaryMonth, SalaryStartDay, SalaryEndDay, CalendayDays, WorkmanSL, WorkRegion, FullName, SkillCategory, PayRate, SkillDesignation, FixedSalary_YesNo, FixedAmount, FixedWageRate, WorkHours, OTFactor, OTMultiplier, DA_VDA, HRA, Conv_Allowance, Medical_Allowance, ATT_Allowance, SPCL_Allowance, Misc_Earnings, Washing_Allowance, Present, OverTime, BasicSalary, FixedRateSalary, OTSalary, DaVdaPay, HRAPay, ConvPay, MedPay, WashPay, AttPay, SPCLPay, MiscPay, OthersPay, ActualGross, ESICGross, PFPay, ESICPay, NetPay1, NetPay2);
        //    }

        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            SqlBulkCopy sqlBulk = new SqlBulkCopy(connection);
        //            sqlBulk.DestinationTableName = "tbl_trialpayroll";
        //            sqlBulk.WriteToServer(dt);
        //            connection.Close();

        //            btnInsertDB.Enabled = false;
        //            btnInsertDB.CssClass = "btn btn-success btn-sm";
        //        }
        //        catch (Exception ex)
        //        {

        //            throw;
        //        }

        //        //if (dt.Rows.Count > 0)
        //        //{
        //        //    string consString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
        //        //    using (SqlConnection con = new SqlConnection(consString))
        //        //    {
        //        //        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
        //        //        {
        //        //            //Set the database table name
        //        //            sqlBulkCopy.DestinationTableName = "tbl_trialpayroll";

        //        //            //[OPTIONAL]: Map the DataTable columns with that of the database table
        //        //            sqlBulkCopy.ColumnMappings.Add("LogerName", "LogerName");
        //        //            sqlBulkCopy.ColumnMappings.Add("LogerWrk", "LogerWrk");
        //        //            sqlBulkCopy.ColumnMappings.Add("Region", "Region");
        //        //            sqlBulkCopy.ColumnMappings.Add("Company", "Company");
        //        //            sqlBulkCopy.ColumnMappings.Add("SalaryYear", "SalaryYear");
        //        //            sqlBulkCopy.ColumnMappings.Add("SalaryMonth", "SalaryMonth");
        //        //            sqlBulkCopy.ColumnMappings.Add("SalaryStartDay", "SalaryStartDay");
        //        //            sqlBulkCopy.ColumnMappings.Add("SalaryEndDay", "SalaryEndDay");
        //        //            sqlBulkCopy.ColumnMappings.Add("CalendayDays", "CalendayDays");
        //        //            sqlBulkCopy.ColumnMappings.Add("WorkmanSL", "WorkmanSL");
        //        //            sqlBulkCopy.ColumnMappings.Add("WorkRegion", "WorkRegion");
        //        //            sqlBulkCopy.ColumnMappings.Add("FullName", "FullName");
        //        //            sqlBulkCopy.ColumnMappings.Add("SkillCategory", "SkillCategory");
        //        //            sqlBulkCopy.ColumnMappings.Add("PayRate", "PayRate");
        //        //            sqlBulkCopy.ColumnMappings.Add("SkillDesignation", "SkillDesignation");
        //        //            sqlBulkCopy.ColumnMappings.Add("FixedSalary_YesNo", "FixedSalary_YesNo");
        //        //            sqlBulkCopy.ColumnMappings.Add("FixedAmount", "FixedAmount");
        //        //            sqlBulkCopy.ColumnMappings.Add("FixedWageRate", "FixedWageRate");
        //        //            sqlBulkCopy.ColumnMappings.Add("WorkHours", "WorkHours");
        //        //            sqlBulkCopy.ColumnMappings.Add("OTFactor", "OTFactor");
        //        //            sqlBulkCopy.ColumnMappings.Add("OTMultiplier", "OTMultiplier");
        //        //            sqlBulkCopy.ColumnMappings.Add("DA_VDA", "DA_VDA");
        //        //            sqlBulkCopy.ColumnMappings.Add("HRA", "HRA");
        //        //            sqlBulkCopy.ColumnMappings.Add("Conv_Allowance", "Conv_Allowance");
        //        //            sqlBulkCopy.ColumnMappings.Add("Medical_Allowance", "Medical_Allowance");
        //        //            sqlBulkCopy.ColumnMappings.Add("ATT_Allowance", "ATT_Allowance");
        //        //            sqlBulkCopy.ColumnMappings.Add("SPCL_Allowance", "SPCL_Allowance");
        //        //            sqlBulkCopy.ColumnMappings.Add("Misc_Earnings", "Misc_Earnings");
        //        //            sqlBulkCopy.ColumnMappings.Add("Washing_Allowance", "Washing_Allowance");
        //        //            sqlBulkCopy.ColumnMappings.Add("Present", "Present");
        //        //            sqlBulkCopy.ColumnMappings.Add("OverTime", "OverTime");
        //        //            sqlBulkCopy.ColumnMappings.Add("BasicSalary", "BasicSalary");
        //        //            sqlBulkCopy.ColumnMappings.Add("FixedRateSalary", "FixedRateSalary");
        //        //            sqlBulkCopy.ColumnMappings.Add("OTSalary", "OTSalary");
        //        //            sqlBulkCopy.ColumnMappings.Add("DaVdaPay", "DaVdaPay");
        //        //            sqlBulkCopy.ColumnMappings.Add("HRAPay", "HRAPay");
        //        //            sqlBulkCopy.ColumnMappings.Add("ConvPay", "ConvPay");
        //        //            sqlBulkCopy.ColumnMappings.Add("MedPay", "MedPay");
        //        //            sqlBulkCopy.ColumnMappings.Add("WashPay", "WashPay");
        //        //            sqlBulkCopy.ColumnMappings.Add("AttPay", "AttPay");
        //        //            sqlBulkCopy.ColumnMappings.Add("SPCLPay", "SPCLPay");
        //        //            sqlBulkCopy.ColumnMappings.Add("MiscPay", "MiscPay");
        //        //            sqlBulkCopy.ColumnMappings.Add("OthersPay", "OthersPay");
        //        //            sqlBulkCopy.ColumnMappings.Add("ActualGross", "ActualGross");
        //        //            sqlBulkCopy.ColumnMappings.Add("ESICGross", "ESICGross");
        //        //            sqlBulkCopy.ColumnMappings.Add("PFPay", "PFPay");
        //        //            sqlBulkCopy.ColumnMappings.Add("ESICPay", "ESICPay");
        //        //            sqlBulkCopy.ColumnMappings.Add("NetPay1", "NetPay1");
        //        //            sqlBulkCopy.ColumnMappings.Add("NetPay2", "NetPay2");
        //        //            con.Open();
        //        //            sqlBulkCopy.WriteToServer(dt);
        //        //            con.Close();
        //        //        }
        //        //    }
        //        //}
        //    }
        //}
        protected void btn_f17print_Click(object sender, EventArgs e)
        {

        }
        private void StatusUpdater()
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_MonthlyPayrollStatus set F17_TrialStatus=@F17_TrialStatus, F17_TrialTimeStamp=@F17_TrialTimeStamp where PayrollYear=@PayrollYear and PayrollMonth=@PayrollMonth and PayrollRegion=@PayrollRegion and PayrollCompany=@PayrollCompany";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@F17_TrialStatus", "Yes");
                cmd.Parameters.AddWithValue("@F17_TrialTimeStamp", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@PayrollYear", year);
                cmd.Parameters.AddWithValue("@PayrollMonth", month);
                cmd.Parameters.AddWithValue("@PayrollRegion", region);
                cmd.Parameters.AddWithValue("@PayrollCompany", company);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                btn_f17print.Enabled = true;
                string title = "Notifications :";
                string body = "Data saved Successfully";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }
        protected void btn_f29print_Click(object sender, EventArgs e)
        {
            Response.Write("<script>window.open ('/bussiness/production/rpts/f29.aspx?Year=" + year + "&Month=" + month + "&Region=" + region + "','_blank');</script>");
        }
    }
}