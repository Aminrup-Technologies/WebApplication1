using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using Org.BouncyCastle.Math.EC.Multiplier;

namespace WebApplication1.bussiness.production
{
    public class DB_Utility_OH4Y
    {
        public static string Logs = @"C:\atswork.in\wwwroot\bussiness\production\WindowsServiceLog\";
        //public static string Logs = @"C:\atswebuat\bussiness\production\WindowsServiceLog\";
        //public static string Logs = @"D:\RnD\OH4Y_19Jun23\WebApplication1\WebApplication1\bussiness\production\WindowsServiceLog\";
        public SqlConnection Conn;
        public SqlDataReader dr;
        public SqlCommand cmd;
        public SqlDataAdapter da;
        public DataTable dt;
        public DataSet ds;
        public static string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString();

        int flag = 0;

        public int Sqlconnection()
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString();
            Conn = new SqlConnection(cnnString);
            flag = 1;
            return flag;
        }

        public void WriteToFile(string text)
        {
            string path = Logs + DateTime.Now.ToString("ddMMyyyy");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + @"\" + "ServiceLog.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " : " + text);
                writer.WriteLine();
                writer.Close();
            }
        }

        public void executeRdr(String SqlString)
        {
            try
            {
                Sqlconnection();
                ConnectDb();
                SqlCommand cmd = new SqlCommand(SqlString, Conn);
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }
            finally
            {
                Conn.Close();
            }
        }

        public void ConnectDb()
        {
            try
            {
                if (Conn.State != ConnectionState.Open)
                    Conn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DisconnectDb()
        {
            try
            {
                Conn.Close();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }



        public DataTable GetDataTable(String cmdString)
        {
            Sqlconnection();
            SqlCommand cmd = new SqlCommand(cmdString, Conn);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            Conn.Close();
            return dt;
        }

        public void BindCombo(DropDownList cmbName, string CmdString)
        {
            cmbName.Items.Clear();
            Sqlconnection();
            ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, Conn);
            Cmd.CommandType = CommandType.Text;
            SqlDataReader Rdr;
            Rdr = Cmd.ExecuteReader();
            while (Rdr.Read())
            {
                cmbName.DataSource = Cmd.ExecuteReader();
                cmbName.DataTextField = Rdr[0].ToString();
                cmbName.DataValueField = Rdr[1].ToString();
                cmbName.DataBind();
                cmbName.Items.Insert(0, "Please Select Options");
            }
            DisconnectDb();
        }

        public void FillCombo(DropDownList cmbName, string cmdString)
        {
            cmbName.Items.Clear();
            Sqlconnection();
            ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            cmbName.Items.Add("--Select--");
            while (Rdr.Read())
            {
                cmbName.Items.Add(Rdr[0].ToString());
            }
            Conn.Close();
        }

        public void FindMonthName(string Month, ref string MonthName)
        {

            if (Month == "01" || Month == "1")
            {
                MonthName = "January";
            }
            else if (Month == "02" || Month == "2")
            {
                MonthName = "February";
            }
            else if (Month == "03" || Month == "3")
            {
                MonthName = "March";
            }
            else if (Month == "04" || Month == "4")
            {
                MonthName = "April";
            }
            else if (Month == "05" || Month == "5")
            {
                MonthName = "May";
            }
            else if (Month == "06" || Month == "6")
            {
                MonthName = "June";
            }
            else if (Month == "07" || Month == "7")
            {
                MonthName = "July";
            }
            else if (Month == "08" || Month == "8")
            {
                MonthName = "August";
            }
            else if (Month == "09" || Month == "9")
            {
                MonthName = "September";
            }
            else if (Month == "10" || Month == "10")
            {
                MonthName = "October";
            }
            else if (Month == "11" || Month == "11")
            {
                MonthName = "November";
            }
            else if (Month == "12" || Month == "12")
            {
                MonthName = "December";
            }
        }

        public void FillCombo1(DropDownList cmbName, string cmdString)
        {
            cmbName.Items.Clear();
            Sqlconnection();
            ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();

            while (Rdr.Read())
            {
                cmbName.Items.Add(Rdr[0].ToString());
            }
            Conn.Close();
        }

        //The below function is to return the Workman of the site incharge whose name is selected in DDL  --Kaushik-
        public string FindWorkmanOfSiteIncharge(string ddl, string table)
        {
            string wrkman = "";

            Sqlconnection();
            ConnectDb();
            string cmdstring = "Select SIte_Incharge_workman from " + table + " where ddl_name = '" + ddl + "'";
            SqlCommand cmd = new SqlCommand(cmdstring, Conn);
            SqlDataReader re = cmd.ExecuteReader();
            if (re.Read())
            {
                wrkman = re["SIte_Incharge_workman"].ToString();
            }
            Conn.Close();

            return wrkman;
        }

        public string FindWorkmanOfSiteIncharge1(string ddl, string table)
        {
            string wrkman = "";

            Sqlconnection();
            ConnectDb();
            string cmdstring = "Select org_sitewrkman from " + table + " where ddl_name = '" + ddl + "'";
            SqlCommand cmd = new SqlCommand(cmdstring, Conn);
            SqlDataReader re = cmd.ExecuteReader();
            if (re.Read())
            {
                wrkman = re["org_sitewrkman"].ToString();
            }
            Conn.Close();

            return wrkman;
        }

        //this function is to check whether a entered workman number exists or not -- Kaushik ---
        public string FetchWorkman(string entered_wrk)
        {
            string wrkmn = "";
            string chkquery = "Select workman_sl from tbl_users where workman_sl = @workman_sl and emp_status = 'Active' ";
            SqlParameter[] param ={
                                    new SqlParameter("@workman_sl",entered_wrk)
                                    };
            dt = SPreturn_dt(chkquery, param);
            if (dt.Rows.Count > 0)
            {
                wrkmn = dt.Rows[0]["workman_sl"].ToString();
            }
            return wrkmn;
        }


        //--- the function is modified for ATS use
        public Boolean CheckEmployeeActiveStatus(string workman)
        {
            Boolean result = false;
            string status = "";
            CountChecker CC = new CountChecker();
            Int32 Activejobcount = CC.Check_WorkmanExitstence(workman);
            if (Activejobcount > 0)
            {
                Sqlconnection();
                ConnectDb();
                string cmdstring = "Select WorkStatus from tbl_Employee_Mustertable where WorkmanSL = '" + workman + "'";
                SqlCommand cmd = new SqlCommand(cmdstring, Conn);
                SqlDataReader re = cmd.ExecuteReader();
                if (re.Read())
                {
                    status = re["WorkStatus"].ToString();
                    if (status == "Active")
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                Conn.Close();

                return result;
            }
            else
            {
                return false;
            }
        }


        public Boolean CheckEmployeeStatus(string workman)
        {
            Boolean result = false;
            string wrkman = "";
            Sqlconnection();
            ConnectDb();
            string cmdstring = "Select emp_status,emp_wrk_status from tbl_users where workman_sl = '" + workman + "'";
            SqlCommand cmd = new SqlCommand(cmdstring, Conn);
            SqlDataReader re = cmd.ExecuteReader();
            if (re.Read())
            {
                wrkman = re["emp_status"].ToString();
                string wrkcode = re["emp_wrk_status"].ToString();
                if (wrkman == "Active")
                {
                    if (wrkcode == "1")
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            Conn.Close();

            return result;
        }

        //This function is used to return name aganist a workman number
        public void FindEmployeeDataforInPunch(string workman, ref string name, ref string workhours, ref string worksitename, ref string worksitecode, ref string category, ref string categorycode, ref string designation, ref string designationcode, ref string gatepassno, ref string gpexp, ref string sftyno, ref string sftyexp, ref string pvexp)
        {
            string cmdString = "select FullName, WorkSite, Worksite_Code, WorkHours, SkillCategory,SkillCategoryDB, SkillDesignation,SkillDesignationDB, GatePassNo, GatePassExpiry,SafetyPassNo, SafetyPassExpiry, PVExpiry from tbl_Employee_Mustertable where WorkmanSL='" + workman + "'";
            Sqlconnection();
            ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                name = Rdr["FullName"].ToString();
                worksitename = Rdr["WorkSite"].ToString();
                worksitecode = Rdr["Worksite_Code"].ToString();
                workhours = Rdr["WorkHours"].ToString();
                category = Rdr["SkillCategory"].ToString();
                categorycode = Rdr["SkillCategoryDB"].ToString();
                designation = Rdr["SkillDesignation"].ToString();
                designationcode = Rdr["SkillDesignationDB"].ToString();
                gatepassno = Rdr["GatePassNo"].ToString();
                gpexp = Rdr["GatePassExpiry"].ToString();

                sftyno = Rdr["SafetyPassNo"].ToString();
                sftyexp = Rdr["SafetyPassExpiry"].ToString();

                pvexp = Rdr["PVExpiry"].ToString();
            }
            Conn.Close();
        }

        public void FindEmployeeName(string workman, ref string name)
        {
            string cmdString = "select FullName from tbl_Employee_Mustertable where WorkmanSL='" + workman + "'";
            Sqlconnection();
            ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                name = Rdr["FullName"].ToString();
            }
            Conn.Close();
        }

        public void FindWorksite(string workman, ref string sitename)
        {
            string cmdString = "select emp_worksite from tbl_emp_given_work where workman_sl='" + workman.ToString() + "'";
            Sqlconnection();
            ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                sitename = Rdr["emp_worksite"].ToString();
            }
            Conn.Close();
        }

        //This function is used to return name aganist a workman number
        public void FindNameandWorkRgn(string workman, ref string name, ref string workregn)
        {
            string cmdString = "select emp_fullname,emp_workregion from tbl_users where workman_sl='" + workman.ToString() + "'";
            Sqlconnection();
            ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                name = Rdr["emp_fullname"].ToString();
                workregn = Rdr["emp_workregion"].ToString();
            }
            Conn.Close();
        }

        //This function is used to return name aganist a workman number
        public void FindName(string name, ref string workman)
        {
            string cmdString = "select workman_sl from tbl_users where emp_fullname='" + name.ToString() + "'";
            Sqlconnection();
            ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                workman = Rdr["workman_sl"].ToString();
            }
            Conn.Close();
        }
        public Boolean CheckForOUTPunch(string workman)
        {
            Boolean outyes = false;

            string cmdString = "select top(1) Supv_approval_status from tbl_employee_attendance where emp_wrk='" + workman.ToString() + "' order by id desc";
            Sqlconnection();
            ConnectDb();
            string name = "";
            SqlCommand cmd = new SqlCommand(cmdString, Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                name = Rdr["Supv_approval_status"].ToString();
                if (name == "Entry")
                {
                    outyes = false;
                }
                else
                {
                    outyes = true;
                }
            }
            else
            {
                outyes = true;
            }
            Conn.Close();
            return outyes;
        }


        //Employee Registration Page
        public Boolean CheckWorkmanAvailabilty(string enteredworkman)
        {
            Boolean available = false;
            string fetched_wrk = FetchWorkman(enteredworkman);
            if (fetched_wrk == enteredworkman)
            {
                available = false;  //Entered Workman is already USED
            }
            else
            {
                available = true; //Entered Workman is available for USE
            }
            return available;
        }

        //Employee Login ID Generation
        public void GenerateLoginID(ref string uniqueid)
        {
            //The code below is for generating -----Employee Id---- by coding  ******** START ************
            string aa = null;
            Sqlconnection();
            ConnectDb();
            string cmdString1 = "select Id,LoginID from tbl_Employee_Mustertable where Id=(select max(Id)from tbl_Employee_Mustertable)";
            SqlCommand com1 = new SqlCommand(cmdString1, Conn);
            SqlDataReader DR1 = com1.ExecuteReader();
            if (DR1.Read())
            {
                aa = DR1.GetValue(1).ToString();
                string bb = aa.Substring(5);
                int k = Convert.ToInt32(bb);
                k = k + 1;
                string q = Convert.ToString(k);
                uniqueid = "ATS00" + q;
            }
            else
            {
                uniqueid = "ATS001";
            }
            Conn.Close();
            //The code below is for generating -----Employee Id---- by coding  ******** END ************
        }

        //Employee Login Password Generation
        public void GenerateLoginPassword(Int32 PasswordLength, ref string password)
        {
            string _allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#abcdefghijklmnopqrstuvwxyz";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            password = new string(chars);
        }
        public void Findworktime(string emp_intime, string emp_outtime, ref Int32 totalmin)
        {
            DateTime intm = DateTime.Parse(emp_intime.ToString());
            DateTime outm = DateTime.Parse(emp_outtime.ToString());

            TimeSpan duration = outm.Subtract(intm);
            //int day = duration.Days;
            int hrs = duration.Hours;
            int hrstomin = hrs * 60;
            int min = duration.Minutes;
            totalmin = hrstomin + min;
        }

        public void FindInpunchEligibilty(string emp_intime, string emp_outtime, ref Int32 hrs)
        {
            DateTime intm = DateTime.Parse(emp_intime.ToString());
            DateTime outm = DateTime.Parse(emp_outtime.ToString());

            TimeSpan duration = outm.Subtract(intm);
            Int32 day = duration.Days;
            Int32 daysmin = day * 1440;
            //Int32 hrs = duration.Hours;
            hrs = duration.Hours;

            Int32 hrstomin = hrs * 60;
            Int32 min = duration.Minutes;
            //totalmin = daysmin + hrstomin + min;


        }

        public void FindEmployeeWorkedTime(string emp_intime, string emp_outtime, ref Int32 totalmin, ref decimal Hours)
        {
            DateTime intm = DateTime.Parse(emp_intime.ToString());
            DateTime outm = DateTime.Parse(emp_outtime.ToString());

            TimeSpan duration = outm.Subtract(intm);
            Int32 day = duration.Days;
            Int32 daysmin = day * 1440;

            Int32 hours = duration.Hours;
            Int32 hrstomin = hours * 60;

            Int32 min = duration.Minutes;


            totalmin = daysmin + hrstomin + min;
            Hours = Math.Round(Convert.ToDecimal(totalmin) / 60, 2);
        }

        public void Findworktime1(string emp_intime, string emp_outtime, ref Int32 totalmin)
        {
            DateTime intm = DateTime.Parse(emp_intime.ToString());
            DateTime outm = DateTime.Parse(emp_outtime.ToString());

            TimeSpan duration = outm.Subtract(intm);
            Int32 day = duration.Days;
            Int32 daysmin = day * 1440;
            Int32 hrs = duration.Hours;
            Int32 hrstomin = hrs * 60;
            Int32 min = duration.Minutes;
            totalmin = daysmin + hrstomin + min;


        }
        public void CalculateOvertime(Int32 EmpWorkHours, Int32 wrdtym, string lunch, ref decimal emp_calOThrs)
        {
            decimal emp_calOTMins = .0m;
            decimal emp_calOTMins1 = .0m;
            decimal emp_calOTMins2 = .0m;

            decimal result = .0m;
            if (EmpWorkHours == 1440)
            {
                emp_calOTMins = .0m;
            }
            else if (EmpWorkHours == 720)
            {
                if (wrdtym > 720)
                {
                    if (lunch == "YES" || lunch == "Yes")
                    {
                        emp_calOTMins1 = wrdtym - EmpWorkHours - 60;
                        if (emp_calOTMins1 < 0)
                        {
                            emp_calOTMins = .0m;
                        }
                        else
                        {
                            emp_calOTMins = emp_calOTMins1;
                        }
                    }
                    else
                    {
                        emp_calOTMins1 = wrdtym - EmpWorkHours + 60;
                        if (emp_calOTMins1 < .0m)
                        {
                            emp_calOTMins = .0m;
                        }
                        else
                        {
                            emp_calOTMins = emp_calOTMins1;
                        }
                    }
                }
                else
                {
                    if (lunch == "YES" || lunch == "Yes")
                    {
                        emp_calOTMins = .0m;
                    }
                    else
                    {
                        emp_calOTMins = 1;
                    }
                }
            }
            else if (EmpWorkHours == 480)
            {
                if (wrdtym > 480)
                {
                    if (lunch == "YES" || lunch == "Yes")
                    {
                        emp_calOTMins2 = wrdtym - EmpWorkHours - 60;
                        if (emp_calOTMins2 < .0m)
                        {
                            emp_calOTMins = .0m;
                        }
                        else
                        {
                            emp_calOTMins = emp_calOTMins2;
                        }
                    }
                    else
                    {
                        emp_calOTMins2 = wrdtym - EmpWorkHours;
                        if (emp_calOTMins2 < .0m)
                        {
                            emp_calOTMins = .0m;
                        }
                        else
                        {
                            emp_calOTMins = emp_calOTMins2;
                        }
                    }
                }
                else
                {
                    emp_calOTMins = .0m;
                }
            }
            //emp_calOThrs = emp_calOTMins;
            result = emp_calOTMins / 60;

            emp_calOThrs = Math.Round(result, 2);
        }

        public void calmonth(DropDownList cmbM1)
        {
            cmbM1.Items.Add("January");
            cmbM1.Items.Add("February");
            cmbM1.Items.Add("March");
            cmbM1.Items.Add("April");
            cmbM1.Items.Add("May");
            cmbM1.Items.Add("June");
            cmbM1.Items.Add("July");
            cmbM1.Items.Add("August");
            cmbM1.Items.Add("September");
            cmbM1.Items.Add("October");
            cmbM1.Items.Add("November");
            cmbM1.Items.Add("December");

        }

        public void CalDateCombo(DropDownList cmbD1, DropDownList cmbM1, DropDownList cmbY1)
        {


            int dd, yyend;
            string dt;
            cmbD1.Items.Clear();
            cmbM1.Items.Clear();
            cmbY1.Items.Clear();
            for (dd = 1; dd <= 31; dd++)
            {
                dt = dd.ToString();
                if (dt.Length == 1)
                { cmbD1.Items.Add("0" + dt); }
                else { cmbD1.Items.Add(dt); }
            }

            yyend = (DateTime.Now.Year);
            cmbY1.Items.Add((yyend - 14).ToString());
            cmbY1.Items.Add((yyend - 13).ToString());
            cmbY1.Items.Add((yyend - 12).ToString());
            cmbY1.Items.Add((yyend - 11).ToString());
            cmbY1.Items.Add((yyend - 10).ToString());
            cmbY1.Items.Add((yyend - 9).ToString());
            cmbY1.Items.Add((yyend - 8).ToString());
            cmbY1.Items.Add((yyend - 7).ToString());
            cmbY1.Items.Add((yyend - 6).ToString());
            cmbY1.Items.Add((yyend - 5).ToString());
            cmbY1.Items.Add((yyend - 4).ToString());
            cmbY1.Items.Add((yyend - 3).ToString());
            cmbY1.Items.Add((yyend - 2).ToString());
            cmbY1.Items.Add((yyend - 1).ToString());
            cmbY1.Items.Add(yyend.ToString());
            for (int i = 1; i <= 20; i++)
            {
                cmbY1.Items.Add((yyend + i).ToString());
            }
            cmbM1.Items.Add("01");
            cmbM1.Items.Add("02");
            cmbM1.Items.Add("03");
            cmbM1.Items.Add("04");
            cmbM1.Items.Add("05");
            cmbM1.Items.Add("06");
            cmbM1.Items.Add("07");
            cmbM1.Items.Add("08");
            cmbM1.Items.Add("09");
            cmbM1.Items.Add("10");
            cmbM1.Items.Add("11");
            cmbM1.Items.Add("12");
            DateTime now = DateTime.Now;
            cmbM1.Text = (now.ToString("MM"));
            cmbY1.Text = (now.ToString("yyyy"));
            cmbD1.Text = (now.ToString("dd"));

        }
        public void CalDateCombo8(DropDownList cmbM1, DropDownList cmbY1)
        {
            int yyend;
            cmbM1.Items.Clear();
            cmbY1.Items.Clear();
            yyend = (DateTime.Now.Year);
            for (int j = 10; j >= 1; j--)
            {
                cmbY1.Items.Add((yyend - j).ToString());
            }
            cmbY1.Items.Add(yyend.ToString());
            for (int i = 1; i <= 20; i++)
            {
                cmbY1.Items.Add((yyend + i).ToString());
            }

            cmbM1.Items.Add("Jan");
            cmbM1.Items.Add("Feb");
            cmbM1.Items.Add("Mar");
            cmbM1.Items.Add("Apr");
            cmbM1.Items.Add("May");
            cmbM1.Items.Add("Jun");
            cmbM1.Items.Add("Jul");
            cmbM1.Items.Add("Aug");
            cmbM1.Items.Add("Sep");
            cmbM1.Items.Add("Oct");
            cmbM1.Items.Add("Nov");
            cmbM1.Items.Add("Dec");
            DateTime now = DateTime.Now;
            cmbM1.Text = (now.ToString("MMM"));
            cmbY1.Text = (now.ToString("yyyy"));
        }

        public void CalDateCombo9(DropDownList cmbM1, DropDownList cmbY1)
        {
            int yyend;
            cmbM1.Items.Clear();
            cmbY1.Items.Clear();
            yyend = (DateTime.Now.Year);
            cmbY1.Items.Add((yyend - 1).ToString());
            cmbY1.Items.Add((yyend).ToString());
            cmbY1.Items.Add((yyend + 1).ToString());

            cmbM1.Items.Add("Jan");
            cmbM1.Items.Add("Feb");
            cmbM1.Items.Add("Mar");
            cmbM1.Items.Add("Apr");
            cmbM1.Items.Add("May");
            cmbM1.Items.Add("Jun");
            cmbM1.Items.Add("Jul");
            cmbM1.Items.Add("Aug");
            cmbM1.Items.Add("Sep");
            cmbM1.Items.Add("Oct");
            cmbM1.Items.Add("Nov");
            cmbM1.Items.Add("Dec");
            DateTime now = DateTime.Now;
            cmbM1.Text = (now.ToString("MMM"));
            cmbY1.Text = (now.ToString("yyyy"));
        }

        public void CalDateCombo90(DropDownList cmbM1, DropDownList cmbY1)
        {
            int yyend;
            cmbM1.Items.Clear();
            cmbY1.Items.Clear();

            yyend = (DateTime.Now.Year);
            cmbY1.Items.Add((yyend - 5).ToString());
            cmbY1.Items.Add((yyend - 4).ToString());
            cmbY1.Items.Add((yyend - 3).ToString());
            cmbY1.Items.Add((yyend - 2).ToString());
            cmbY1.Items.Add((yyend - 1).ToString());
            cmbY1.Items.Add((yyend).ToString());
            cmbY1.Items.Add((yyend + 1).ToString());
            cmbY1.Items.Add((yyend + 2).ToString());
            cmbY1.Items.Add((yyend + 3).ToString());
            cmbY1.Items.Add((yyend + 4).ToString());
            cmbY1.Items.Add((yyend + 5).ToString());

            cmbM1.Items.Add("Jan");
            cmbM1.Items.Add("Feb");
            cmbM1.Items.Add("Mar");
            cmbM1.Items.Add("Apr");
            cmbM1.Items.Add("May");
            cmbM1.Items.Add("Jun");
            cmbM1.Items.Add("Jul");
            cmbM1.Items.Add("Aug");
            cmbM1.Items.Add("Sep");
            cmbM1.Items.Add("Oct");
            cmbM1.Items.Add("Nov");
            cmbM1.Items.Add("Dec");
            DateTime now = DateTime.Now;
            cmbM1.Text = (now.ToString("MMM"));
            cmbY1.Text = (now.ToString("yyyy"));


        }
        public void CalDateCombo1(DropDownList cmbD1, DropDownList cmbM1, DropDownList cmbY1)
        {
            int dd, yyend;
            string dt;
            cmbD1.Items.Clear();
            cmbM1.Items.Clear();
            cmbY1.Items.Clear();
            for (dd = 1; dd <= 31; dd++)
            {
                dt = dd.ToString();
                if (dt.Length == 1)
                { cmbD1.Items.Add("0" + dt); }
                else { cmbD1.Items.Add(dt); }
            }

            yyend = (DateTime.Now.Year);
            for (int j = 10; j >= 1; j--)
            {


                cmbY1.Items.Add((yyend - j).ToString());
            }
            cmbY1.Items.Add(yyend.ToString());
            for (int i = 1; i <= 10; i++)
            {
                cmbY1.Items.Add((yyend + i).ToString());
            }
            cmbM1.Items.Add("01");
            cmbM1.Items.Add("02");
            cmbM1.Items.Add("03");
            cmbM1.Items.Add("04");
            cmbM1.Items.Add("05");
            cmbM1.Items.Add("06");
            cmbM1.Items.Add("07");
            cmbM1.Items.Add("08");
            cmbM1.Items.Add("09");
            cmbM1.Items.Add("10");
            cmbM1.Items.Add("11");
            cmbM1.Items.Add("12");
            DateTime now = DateTime.Now;
            cmbM1.Text = (now.ToString("MM"));
            cmbY1.Text = (now.ToString("yyyy"));
            cmbD1.Text = (now.ToString("dd"));

        }
        public void CalDateCombo5(DropDownList cmbD1, DropDownList cmbM1, DropDownList cmbY1)
        {
            int dd, yyend;
            string dt;
            cmbD1.Items.Clear();
            cmbM1.Items.Clear();
            cmbY1.Items.Clear();
            for (dd = 1; dd <= 31; dd++)
            {
                dt = dd.ToString();
                if (dt.Length == 1)
                { cmbD1.Items.Add("0" + dt); }
                else { cmbD1.Items.Add(dt); }
            }

            yyend = (DateTime.Now.Year);
            for (int j = 10; j >= 1; j--)
            {


                cmbY1.Items.Add((yyend - j).ToString());
            }
            cmbY1.Items.Add(yyend.ToString());
            for (int i = 1; i <= 20; i++)
            {
                cmbY1.Items.Add((yyend + i).ToString());
            }
            cmbM1.Items.Add("Month");
            cmbM1.Items.Add("Jan");
            cmbM1.Items.Add("Feb");
            cmbM1.Items.Add("Mar");
            cmbM1.Items.Add("Apr");
            cmbM1.Items.Add("May");
            cmbM1.Items.Add("Jun");
            cmbM1.Items.Add("Jul");
            cmbM1.Items.Add("Aug");
            cmbM1.Items.Add("Sep");
            cmbM1.Items.Add("Oct");
            cmbM1.Items.Add("Nov");
            cmbM1.Items.Add("Dec");
            DateTime now = DateTime.Now;
            cmbM1.Text = (now.ToString("MMM"));
            cmbY1.Text = (now.ToString("yyyy"));
            cmbD1.Text = (now.ToString("dd"));

        }

        public void CalDateComboLeave(DropDownList cmbD1, DropDownList cmbM1, DropDownList cmbY1)
        {
            int dd, yyend;
            string dt;
            cmbD1.Items.Clear();
            cmbM1.Items.Clear();
            cmbY1.Items.Clear();
            for (dd = 1; dd <= 31; dd++)
            {
                dt = dd.ToString();
                if (dt.Length == 1)
                { cmbD1.Items.Add("0" + dt); }
                else { cmbD1.Items.Add(dt); }
            }

            yyend = (DateTime.Now.Year);
            cmbY1.Items.Add((yyend - 1).ToString());
            cmbY1.Items.Add(yyend.ToString());

            for (int i = 1; i <= 20; i++)
            {
                cmbY1.Items.Add((yyend + i).ToString());
            }
            cmbM1.Items.Add("Month");
            cmbM1.Items.Add("Jan");
            cmbM1.Items.Add("Feb");
            cmbM1.Items.Add("Mar");
            cmbM1.Items.Add("Apr");
            cmbM1.Items.Add("May");
            cmbM1.Items.Add("Jun");
            cmbM1.Items.Add("Jul");
            cmbM1.Items.Add("Aug");
            cmbM1.Items.Add("Sep");
            cmbM1.Items.Add("Oct");
            cmbM1.Items.Add("Nov");
            cmbM1.Items.Add("Dec");
            DateTime now = DateTime.Now;
            cmbM1.Text = (now.ToString("MMM"));
            cmbY1.Text = (now.ToString("yyyy"));
            cmbD1.Text = (now.ToString("dd"));

        }
        public void Dob1DateCombo(DropDownList cmbD1, DropDownList cmbM1, DropDownList cmbY1)
        {
            int dd, yy, yyend;
            string dt;
            cmbD1.Items.Clear();
            cmbM1.Items.Clear();
            cmbY1.Items.Clear();

            cmbD1.Items.Add("Day");
            for (dd = 1; dd <= 31; dd++)
            {
                dt = dd.ToString();
                if (dt.Length == 1)
                { cmbD1.Items.Add("0" + dt); }
                else { cmbD1.Items.Add(dt); }
            }

            dd = (DateTime.Now.Year) - 70;
            yyend = (DateTime.Now.Year);
            cmbY1.Items.Add("Year");
            for (yy = dd; yy <= yyend; yy++)
            {
                cmbY1.Items.Add(yy.ToString());
            }

            cmbM1.Items.Add("Month");
            cmbM1.Items.Add("Jan");
            cmbM1.Items.Add("Feb");
            cmbM1.Items.Add("Mar");
            cmbM1.Items.Add("Apr");
            cmbM1.Items.Add("May");
            cmbM1.Items.Add("Jun");
            cmbM1.Items.Add("Jul");
            cmbM1.Items.Add("Aug");
            cmbM1.Items.Add("Sep");
            cmbM1.Items.Add("Oct");
            cmbM1.Items.Add("Nov");
            cmbM1.Items.Add("Dec");

        }

        //This is my creation//...........................
        public int SPExecDB(String SPName, SqlParameter[] SPParameter)
        {
            Sqlconnection();
            ConnectDb();
            cmd = new SqlCommand(SPName, Conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            if (SPParameter != null)
            {
                foreach (SqlParameter p in SPParameter)
                {
                    cmd.Parameters.Add(p);
                }
            }
            int retVal = cmd.ExecuteNonQuery();
            if (retVal > 0)
            {
                return 1;
            }
            else
            {
                return 0;

            }

        }

        //public SqlDataAdapter return_da(string s1)
        //{
        //    Sqlconnection();
        //    ConnectDb();
        //    cmd = new SqlCommand(s1, Conn);
        //    cmd.CommandType = CommandType.Text;
        //    da = new SqlDataAdapter(cmd);
        //    Conn.Close();
        //    return da;
        //}

        public DataTable return_dt(string s1)
        {
            Sqlconnection();
            ConnectDb();
            cmd = new SqlCommand(s1, Conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            Conn.Close();
            return dt;
        }

        public int delete_test(String strSql)
        {
            int delval = 0;
            Sqlconnection();
            ConnectDb();
            cmd = new SqlCommand(strSql, Conn);
            cmd.CommandType = CommandType.Text;

            delval = cmd.ExecuteNonQuery();
            Conn.Close();
            if (delval > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        //public int generate_max_id_reg(string strsql)
        //{
        //    int id;
        //    Sqlconnection();
        //    ConnectDb();
        //    SqlCommand cmd = new SqlCommand(strsql, Conn);
        //    string genValue = Convert.ToString(cmd.ExecuteScalar());
        //    Conn.Close();

        //    if (genValue == "")
        //    {
        //        id = 1;
        //    }
        //    else
        //    {
        //        //int id = Convert.ToInt32(genValue.PadRight(genValue.Substring(4, genValue.Length - 4));
        //        id = Convert.ToInt32(genValue) + 1;
        //    }
        //    return id;
        //}

        public void populate_combo(String strsql, System.Web.UI.WebControls.DropDownList cmb1)
        {
            try
            {
                Sqlconnection();
                ConnectDb();
                SqlCommand cmd = new SqlCommand(strsql, Conn);
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    cmb1.Items.Add(dr.GetValue(0).ToString());
                }
                Conn.Close();
            }
            catch
            {

            }
        }

        public void Sppopulate_Combo(String strsql, SqlParameter[] SPParameter, System.Web.UI.WebControls.DropDownList ddlDropdown)
        {
            try
            {
                ddlDropdown.Items.Clear();
                Sqlconnection();
                ConnectDb();
                SqlCommand cmd = new SqlCommand(strsql, Conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                if (SPParameter != null)
                {
                    foreach (SqlParameter p in SPParameter)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                dr = cmd.ExecuteReader();

                ddlDropdown.Items.Add("--Select--");

                while (dr.Read())
                {
                    ddlDropdown.Items.Add(dr.GetValue(0).ToString());
                }
                Conn.Close();
            }
            catch
            {

            }
        }

        public void Sppopulate_Comboforupdate(String strsql, SqlParameter[] SPParameter, System.Web.UI.WebControls.DropDownList ddlDropdown)
        {
            try
            {
                ddlDropdown.Items.Clear();
                Sqlconnection();
                ConnectDb();
                SqlCommand cmd = new SqlCommand(strsql, Conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                if (SPParameter != null)
                {
                    foreach (SqlParameter p in SPParameter)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ddlDropdown.Items.Add(dr.GetValue(0).ToString());
                }
                Conn.Close();
            }
            catch
            {

            }
        }

        public int ExecDB(String strSql)
        {
            int retVal = 0;
            cmd = new SqlCommand();
            Sqlconnection();
            ConnectDb();
            cmd.Connection = Conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strSql;
            retVal = cmd.ExecuteNonQuery();
            Conn.Close();
            if (retVal > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

        public SqlDataAdapter return_da(string s1)
        {
            Sqlconnection();
            ConnectDb();
            cmd = new SqlCommand(s1, Conn);
            cmd.CommandType = CommandType.Text;
            da = new SqlDataAdapter(cmd);
            Conn.Close();
            return da;
        }

        public int SPCount(String SPName, SqlParameter[] SPParameter)
        {
            Sqlconnection();
            ConnectDb();
            cmd = new SqlCommand(SPName, Conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            if (SPParameter != null)
            {
                foreach (SqlParameter p in SPParameter)
                {
                    cmd.Parameters.Add(p);
                }
            }
            int retVal = (int)cmd.ExecuteScalar();
            if (retVal > 0)
            {
                return retVal;
            }
            else
            {
                return 1;
            }
        }

        public string SPgetdatavalur(String SPName, SqlParameter[] SPParameter)
        {
            Sqlconnection();
            ConnectDb();
            cmd = new SqlCommand(SPName, Conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            if (SPParameter != null)
            {
                foreach (SqlParameter p in SPParameter)
                {
                    cmd.Parameters.Add(p);
                }
            }
            string retVal = (string)cmd.ExecuteScalar();
            if (retVal != "")
            {
                return retVal;
            }
            else
            {
                return retVal = "";
            }
        }

        public object SPgetRederValue(String SPName, SqlParameter[] SPParameter)
        {
            object strvalue = new object();

            Sqlconnection();
            ConnectDb();
            cmd = new SqlCommand(SPName, Conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            if (SPParameter != null)
            {
                foreach (SqlParameter p in SPParameter)
                {
                    cmd.Parameters.Add(p);
                }
            }
            dr = cmd.ExecuteReader();


            while (dr.Read())
            {
                //strvalue = (string)dr.GetString(0);
                strvalue = dr.GetValue(0).ToString();
            }
            return strvalue;


        }

        public SqlDataReader SPReturnRdr(String SPName, SqlParameter[] SPParameter)
        {
            Sqlconnection();
            ConnectDb();
            cmd = new SqlCommand(SPName, Conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            if (SPParameter != null)
            {
                foreach (SqlParameter p in SPParameter)
                {
                    cmd.Parameters.Add(p);
                }
            }
            dr = cmd.ExecuteReader();
            return dr;
        }

        public DataSet datasetreturn(string SPName, SqlParameter[] SPParameter)
        {
            Sqlconnection();
            ConnectDb();
            SqlDataAdapter da = new SqlDataAdapter(SPName, Conn);
            da.SelectCommand.CommandType = CommandType.Text;
            if (SPParameter != null)
            {
                foreach (SqlParameter p in SPParameter)
                {
                    da.SelectCommand.Parameters.Add(p);
                }
            }
            ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public DataSet SPreturn_dataset(string s1, SqlParameter[] SPParameter)
        {
            Sqlconnection();
            ConnectDb();
            cmd = new SqlCommand(s1, Conn);
            cmd.CommandType = CommandType.Text;
            if (SPParameter != null)
            {
                foreach (SqlParameter p in SPParameter)
                {
                    cmd.Parameters.Add(p);
                }
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            ds = new DataSet();
            da.Fill(ds);
            Conn.Close();
            return ds;
        }

        public DataTable SPreturn_dt(string s1, SqlParameter[] SPParameter)
        {
            Sqlconnection();
            ConnectDb();
            cmd = new SqlCommand(s1, Conn);
            cmd.CommandType = CommandType.Text;
            if (SPParameter != null)
            {
                foreach (SqlParameter p in SPParameter)
                {
                    cmd.Parameters.Add(p);
                }
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            dt = new DataTable();
            da.Fill(dt);
            Conn.Close();
            return dt;
        }


        //User login information updater ----- written on 01-07-2022

        public void UPDT_EmpMuster_LoginInfo(string empwrk, string emploginid)
        {
            try
            {

                Sqlconnection();
                ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Conn;
                string CmdString = "UPDATE tbl_Employee_Mustertable set LoginStatus=@LoginStatus, LastLogin=@LastLogin where WorkmanSL=@WorkmanSL and LoginID=@LoginID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@WorkmanSL", empwrk);
                cmd.Parameters.AddWithValue("@LoginID", emploginid);
                cmd.Parameters.AddWithValue("@LoginStatus", "1");
                cmd.Parameters.AddWithValue("@LastLogin", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.ExecuteNonQuery();
                DisconnectDb();
                cmd.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        public void UPDT_EmpMuster_LogoutInfo(string empwrk, string emploginid)
        {
            try
            {

                Sqlconnection();
                ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Conn;
                string CmdString = "UPDATE tbl_Employee_Mustertable set LoginStatus=@LoginStatus, LastLogout=@LastLogout where WorkmanSL=@WorkmanSL and LoginID=@LoginID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@WorkmanSL", empwrk);
                cmd.Parameters.AddWithValue("@LoginID", emploginid);
                cmd.Parameters.AddWithValue("@LoginStatus", "0");
                cmd.Parameters.AddWithValue("@LastLogout", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Conn.Close();
            }
            catch (Exception ex)
            {

            }
        }

        public void UPDT_EmpProfilePicInfo(string empwrk)
        {
            try
            {

                Sqlconnection();
                ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Conn;
                string CmdString = "UPDATE tbl_Employee_Mustertable set LoginStatus=@LoginStatus, LastLogout=@LastLogout where WorkmanSL=@WorkmanSL and LoginID=@LoginID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@WorkmanSL", empwrk);
                cmd.Parameters.AddWithValue("@LoginStatus", "0");
                cmd.Parameters.AddWithValue("@LastLogout", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Conn.Close();
            }
            catch (Exception ex)
            {

            }
        }

        public void UPDT_ManpowerCountJOBID(Int32 empwrk, string jobid)
        {
            try
            {

                Sqlconnection();
                ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Conn;
                string CmdString = "UPDATE tbl_jobs set ManpowerCount=@ManpowerCount where JOBID=@JOBID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ManpowerCount", empwrk);
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Conn.Close();
            }
            catch (Exception ex)
            {

            }
        }

        public Int32 Find_CreatedJOBID(string workman)
        {
            string cmdString = "";
            Sqlconnection();
            ConnectDb();
            cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and JOBID_Status='Active' and YEAR(CreatedDate)='" + DateTime.Now.Year + "' and MONTH(CreatedDate)='" + DateTime.Now.Month + "'";
            SqlCommand cmd = new SqlCommand(cmdString,Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Creator_Workman", workman);
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            Conn.Close();
            return count;
        }

        public string Find_JOBIDMasterCode(string workman)
        {
            string mastercode = string.Empty;
            string jobid = string.Empty;
            string cmdString = "select JOBID,MasterStatusCode from tbl_jobs where Creator_Workman=@Creator_Workman and JOBID_Status='Active' and YEAR(CreatedDate)='" + DateTime.Now.Year + "' and MONTH(CreatedDate)='" + DateTime.Now.Month + "'";
            Sqlconnection();
            ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, Conn);
            cmd.Parameters.AddWithValue("@Creator_Workman", workman);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                jobid = Rdr["JOBID"].ToString();
                mastercode = Rdr["MasterStatusCode"].ToString();
            }
            Conn.Close();
            return jobid + "/" + mastercode  ;
        }
    }
}