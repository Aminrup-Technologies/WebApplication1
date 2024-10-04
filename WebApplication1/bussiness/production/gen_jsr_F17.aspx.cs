using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using DocumentFormat.OpenXml.Bibliography;
using System.Drawing;

namespace WebApplication1.bussiness.production
{
    public partial class gen_jsr_F17 : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        Payroll_OH4Y PayRoll = new Payroll_OH4Y();

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
        public static Int32 TotalPresents = 0;
        public static decimal GorssBreaker = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
            {
                Response.Redirect("~/login.aspx");
            }
            if (!IsPostBack)
            {
                string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = 'IN' and State_Code ='"+ Session["STATE"].ToString() + "' order by Id ";
                BindRegions(CmdString1);

                dbcl.CalDateCombo1(DDL_Day, DDL_Month, DDL_Year);
                dbcl.CalDateCombo1(DDL_D2, DDL_M2, DDL_Y2);
                if (Session["REGION"].ToString() == "KPO")
                {
                    CheckforUser();
                    GorssBreaker = 18500;
                }
                else if (Session["REGION"].ToString() == "AGL")
                {
                    CheckforUser();
                    GorssBreaker = 20500;
                }
                else if (Session["REGION"].ToString() == "NINL")
                {
                    CheckforUser();
                    GorssBreaker = 18500;
                }
                else if (Session["REGION"].ToString() == "JSR")
                {
                    CheckforUser();
                    //GorssBreaker = 20500; -- Commented on 21-Aug-2024 Based on mail from Anupam Sharma dated : 19-Aug-2024 for changing ESIC Gross Breaker Amount from 19500 to 20999
                    GorssBreaker = 20999;
                }
            }
        }

        private void CheckforUser()
        {
            DDL_Region.SelectedValue = Session["REGION"].ToString();
            DDL_Region.Enabled = false;
            string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='"+ Session["STATE"].ToString() + "' and Work_Region_Code = '" + Session["REGION"].ToString() + "' order by Id ";
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
            string startday = "";
            startday = DDL_Day.SelectedItem.Text.ToString();
            minday = Convert.ToInt32(startday);

            string endday = "";
            endday = DDL_D2.SelectedItem.Text.ToString();
            maxday = Convert.ToInt32(endday);

            year = DDL_Year.SelectedItem.Text.ToString();
            month = DDL_Month.SelectedItem.Text.ToString();
            //region = DDL_Region.SelectedValue.ToString();

            //year = "2021";
            //month = "08";
            region = DDL_Region.SelectedValue.ToString();
            company = DDL_Company.SelectedValue.ToString();

            date1 = year + "-" + month + "-" + minday;
            date2 = year + "-" + month + "-" + maxday;


            //CalWorkingDays = 26;

            CalWorkingDays = Convert.ToInt32(DDL_Days.SelectedItem.Text.ToString());

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
                    cmd.Parameters.AddWithValue("@PayrollWorkDays", DDL_Days.SelectedItem.Text.ToString());
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
                    btnInsertDB.Enabled = true;
                    btn_f17print.Enabled = false;
                    btn_f29print.Enabled = false;
                }
            }


            //string query = "select WorkmanSL, WorkRegion, FullName, SkillCategory, SkillDesignation,FixedSalary_YesNo, FixedAmount, WorkHours, OTFactor, OTMultiplier, DA_VDA, HRA,Conv_Allowance, Medical_Allowance, Washing_Allowance, ATT_Allowance, SPCL_Allowance, Misc_Earnings from tbl_Employee_Mustertable where WorkRegion='" + region + "' and WorkStatus='" + DDL_EmpWorkStatus.SelectedItem.Text.ToString() + "' order by Id";

            string query = "select WorkmanSL, WorkRegion, FullName, SkillCategory, SkillDesignation,FixedSalary_YesNo, FixedAmount, WorkHours, OTFactor, OTMultiplier, DA_VDA, HRA,Conv_Allowance, Medical_Allowance, Washing_Allowance, ATT_Allowance, SPCL_Allowance, Misc_Earnings, OT_Divisibility, Cur_Advance, Cur_Fines, Cur_Others from tbl_Employee_Mustertable where WorkRegion='" + region + "' and WorkStatus='" + DDL_EmpWorkStatus.SelectedItem.Text.ToString() + "' and F17_YesNo='Yes' order by Id";

            BindGridByQuery(query);
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
        private void BindGridByQuery(string query)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(query, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            GridView.DataSource = ds;
            GridView.DataBind();
            dbcl.Conn.Close();

            f17grid.Visible = true;
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

                Label lbl_presents = (Label)GridView.Rows[i].FindControl("lbl_presents");
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


                //----------------- Function call to find out the Total Present aganist the Employee Workman Sl----------------//
                string empwrk = lbl_WorkmanSL.Text.ToString();
                PayRoll.FindEmployeeTotalPresentByDates2(date1, date2, empwrk, ref TotalPresents);
                lbl_presents.Text = TotalPresents.ToString();


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


                //----------------- Calculation for Employee who are in Fixed Salary----------------//
                string fixedyesno = lbl_FixedSalary_YesNo.Text.ToString();
                decimal fixedamount = Convert.ToDecimal(lbl_FixedAmount.Text.ToString());
                decimal fdr = Convert.ToDecimal(fixedamount) / Convert.ToDecimal(CalWorkingDays);
                decimal fnlfdr = Math.Round(fdr, 2);
                lbl_fixedwagerate.Text = fnlfdr.ToString();


                //------------ Wage of Fixed rate ---------------   ( FixedAmount / CalenderDays ) x  PresentDays
                decimal FixRateSalary = 0.0m;
                FixRateSalary = Math.Ceiling(Math.Round(fdr * TotalPresents, 2));
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
                PayRoll.EmployeeOthersPayCalculations1(TotalPresents, CalWorkingDays, DaVdaAmount, ref DaVdaPay);
                lbl_DaVdaPay.Text = DaVdaPay.ToString();

                //---------------- HRA Alowances Cal-------------------------------------//
                PayRoll.EmployeeOthersPayCalculations2(TotalPresents, CalWorkingDays, HRAAmount, ref HRAPay);
                lbl_HRAPay.Text = HRAPay.ToString();

                //---------------- Conv Alowances Cal-------------------------------------//
                PayRoll.EmployeeOthersPayCalculations3(TotalPresents, CalWorkingDays, ConvAmount, ref ConvPay);
                lbl_ConvPay.Text = ConvPay.ToString();

                //---------------- Medical Alowances Cal-------------------------------------//
                PayRoll.EmployeeOthersPayCalculations4(TotalPresents, CalWorkingDays, MedAmount, ref MedPay);
                lbl_MedPay.Text = MedPay.ToString();

                //---------------- Wash Alowances Cal-------------------------------------//
                PayRoll.EmployeeOthersPayCalculations5(TotalPresents, CalWorkingDays, WashAmount, ref WashPay);
                lbl_WashPay.Text = WashPay.ToString();

                //---------------- Att Alowances Cal-------------------------------------//
                PayRoll.EmployeeOthersPayCalculations6(TotalPresents, CalWorkingDays, AttAmount, ref AttPay);
                lbl_AttPay.Text = AttPay.ToString();

                //---------------- SPCL Alowances Cal-------------------------------------//
                PayRoll.EmployeeOthersPayCalculations7(TotalPresents, CalWorkingDays, SPCLAmount, ref SPCLPay);
                lbl_SPCLPay.Text = SPCLPay.ToString();

                //---------------- MISC Alowances Cal-------------------------------------//
                PayRoll.EmployeeOthersPayCalculations8(TotalPresents, CalWorkingDays, MiscAmount, ref MiscPay);
                lbl_MiscPay.Text = MiscPay.ToString();






                //--------------- PF Calucations --------------------//
                decimal PFPay = 0.0m;
                PayRoll.PFPayCalculation(BasicSalary, ref PFPay);
                lbl_PFPay.Text = PFPay.ToString();


                //---------------- OT Pay -------- Gross Rate
                decimal otpay = 0.0m;
                decimal actualgross = 0.0m;

                decimal wrkhrs = Convert.ToDecimal(lbl_WorkHours.Text.ToString());

                decimal otdiv = Convert.ToDecimal(lbl_OT_Divisibility.Text.ToString());  //Added on 29-11-2021

                decimal otfactor = Convert.ToDecimal(lbl_OTFactor.Text.ToString());
                string otrate = lbl_OTMultiplier.Text.ToString();

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
                        actualgross = FixRateSalary + otpay + DaVdaPay + MedPay + AttPay + SPCLPay + MiscPay;
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
                        actualgross = FixRateSalary + otpay + DaVdaPay + MedPay + AttPay + SPCLPay + MiscPay;
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
                        actualgross = FixRateSalary + otpay + DaVdaPay + MedPay + AttPay + SPCLPay + MiscPay;
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
                        actualgross = BasicSalary + otpay + DaVdaPay + MedPay + AttPay + SPCLPay + MiscPay;
                    }
                    lbl_actualgross.Text = actualgross.ToString();
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

                lbl_grossamount.Text = grossesic.ToString();
                decimal otherpay = grossesic - BasicSalary;
                lbl_otherspay.Text = otherpay.ToString();

                //--------------------- ESIC pay---------------------------------------------------//

                decimal esicpay = 0.0m;
                esicpay = Math.Round(grossesic * 0.0075m, 0);
                lbl_esicpay.Text = esicpay.ToString();


                //--------------------- NET Payment -------------------------------------------------//
                decimal netpay1 = 0.0m;
                decimal newgross = grossesic;
                netpay1 = Math.Round(newgross - PFPay - esicpay + WashPay + ConvPay, 0);

                decimal ttldeductions = AdvanceAmt + FinesAmt + OthersAmt;
                lbl_ttlded.Text = ttldeductions.ToString();

                decimal netpay1final = .0m;
                decimal netpay2 = 0.0m;
                if (fixedyesno == "Yes")
                {
                    netpay2 = actualgross - newgross + HRAPay;
                    //netpay2 = newgross-actualgross;
                }
                else
                {
                    decimal p = netpay1;
                    netpay2 = actualgross - p - PFPay - esicpay + HRAPay;
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
                else if (workregion == "AGL" || workregion == "JSR" || workregion == "NINL" || workregion == "RSP")
                {
                    netpay2_finalaftrded = netpay2;
                    netpay1final = netpay1 - ttldeductions;
                }

                //decimal netpayfinal = netpay1 - ttldeductions;
                lbl_netpay1.Text = netpay1.ToString();
                lbl_netpayfnl.Text = netpay1final.ToString();


                //decimal netpay2 = 0.0m;
                //if (fixedyesno == "Yes")
                //{
                //    netpay2 = actualgross - newgross + HRAPay;
                //    //netpay2 = newgross-actualgross;
                //}
                //else
                //{
                //    decimal p = netpay1;
                //    netpay2 = actualgross - p - PFPay - esicpay + HRAPay;
                //}
                //lbl_netpay2.Text = netpay2.ToString();


                lbl_netpay2.Text = netpay2_finalaftrded.ToString();///---1
            }

            btnExport.Enabled = true;
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
            dt.Columns.Add("Present", typeof(Int32));
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
                string SalaryMonth = DDL_Month.SelectedItem.Text.ToString();
                string SalaryStartDay = DDL_Day.SelectedItem.Text.ToString();
                string SalaryEndDay = DDL_D2.SelectedItem.Text.ToString();
                string CalendayDays = DDL_Days.SelectedItem.Text.ToString();

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
                Int32 Present = Convert.ToInt32(lbl_presents.Text.ToString());

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
                    cmd.Parameters.AddWithValue("Present", Present);
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