using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication1.bussiness.production
{
    public partial class generate_appvrsite_atdncsheet : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        string str = string.Empty;
        Int32 dayss = 0;
        Int32 ots = 0;
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
                    string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = 'IN' and State_Code ='" + Session["STATE"].ToString() + "' order by Id ";
                    BindRegions(CmdString1);

                    string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='"+ Session["STATE"].ToString() + "' and Work_Region_Code = '" + Session["REGION"].ToString() + "' order by Id ";
                    BindCompany(CmdString3);
                    DDL_Company.SelectedValue = Session["COMPANY_CODE"].ToString();

                    string CmdString2 = "select  DB_Code, Worksite_Name from tlb_atsworksiteIncharges where Employee_Workman='"+ Session["WORKMAN"] + "' and Status='Active'";
                    BindWorkSites(CmdString2);


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
            string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='" + Session["STATE"].ToString() + "' and Work_Region_Code = '" + Session["REGION"].ToString() + "' order by Id ";
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
            //string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='OD' and Work_Region_Code = '" + DDL_Region.SelectedValue.ToString() + "' order by Id ";
            //BindCompany(CmdString3);
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

            string wrksitecode = DDL_Worksite.SelectedValue.ToString();

            //Session["WRKRGN"] = region;

            if (DDL_ReportType.SelectedIndex == 1)
            {
                //Only Present Sheet
                BindAttendanceHead2(minday, maxday, current_month1, current_year, region, wrksitecode);
            }
            else if (DDL_ReportType.SelectedIndex == 2)
            {
                //Only OT Sheets
                BindAttendanceHead3(minday, maxday, current_month1, current_year, region, wrksitecode);
            }
            else if (DDL_ReportType.SelectedIndex == 3)
            {
                //Combined Sheet - Single Line
                BindAttendanceHead1(minday, maxday, current_month1, current_year, region, wrksitecode);
            }
            else if (DDL_ReportType.SelectedIndex == 4)
            {
                //Combined Sheets - Double Line
                BindAttendanceHead4(minday, maxday, current_month1, current_year, region, wrksitecode);
            }
            btnExport.Enabled = true;
        }

        private void BindAttendanceHead1(Int32 minday, Int32 maxday, string month_no, string year, string region, string wrksitecode)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='background-color:#00a8f3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center' colspan='35'> " + month_no.ToString() + " " + year.ToString() + "</span></td></tr></table>";
            BindHeading1(minday, maxday, month_no, year, region, wrksitecode);
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='font:normal 16px/16px Century Gothic; padding:5px 20px 5px 20px;' align='cen ter'></td></tr></table>";
            lblTotalData.Text = str;
        }
        //Function for binding Absent / Present Row
        private void BindHeading1(Int32 minday, Int32 maxday, string month_no, string year, string region, string wrksitecode)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>S. NO</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>WORK SL</td>";
            str = str + "<td width='18%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EMPLOYEE NAME</td>";
            for (int i = minday; i <= maxday; i++)
            {
                str = str + "<td width='2%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + i + "</td>";
            }
            str = str + "<td width='10%' style='background-color:#92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>TOTAL PRESENTS</td></tr></table>";

            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            int Sl = 1;
            string findempquery = "select distinct EmployeeWrk, EmployeeName from tbl_attendance where JOB_Region='" + region + "' and JOB_SiteCode='" + wrksitecode + "' and  YEAR(CreatedDate)='" + year + "' and MONTH(CreatedDate)='" + month_no + "' and DAY(CreatedDate) between '" + minday + "' and '" + maxday + "'";

            SqlCommand cmd = new SqlCommand(findempquery, dbcl.Conn);
            using (SqlDataReader re = cmd.ExecuteReader())
            {
                while (re.Read())
                {
                    Int32 daycount = 0;
                    Int32 ttlot = 0;
                    string wrkman = re["EmployeeWrk"].ToString();
                    string emp_name = re["EmployeeName"].ToString();
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
                        FindAttendance1(days, month_no, year, wrkman, ref daycount, ref ttlot, region, wrksitecode);
                        day = day + 1;
                    }
                    str = str + "<td width='10%' style='background-color:white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + daycount + "|" + ttlot + "</td></tr></table>";
                    //BindHeadingRow(minday, maxday, month_no, year, region, wrkman, emp_name);
                    Sl = Sl + 1;
                    dayss = 0;
                }
            }
        }
        private void FindAttendance1(string day, string month, string year, string wrk, ref Int32 dayss, ref Int32 ttlot, string region, string wrksitecode)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdstring1 = "select max(AttendanceStatus) as AttendanceStatus, AttendanceCode, ProvidedOT as OT from tbl_attendance where MONTH(CreatedDate)='" + month + "' and DAY(CreatedDate)='" + day + "' and YEAR(CreatedDate)='" + year + "' and EmployeeWrk='" + wrk + "' and SiteIncharge_Approval='Approved' and JOB_Region='" + region + "' and JOB_SiteCode='" + wrksitecode + "' group by AttendanceCode";
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
        private void BindHeadingRow1(Int32 minday, Int32 maxday, string month_no, string year, string region, string wrkman, string emp_name, string wrksitecode)
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
                FindAttendanceOT1(days, month_no, year, wrkman, ref daycount, ref ots, region, wrksitecode);
                day = day + 1;
                ttlot = ttlot + ots;
            }
            str = str + "<td width='10%' style='background-color:white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + ttlot + "</td></tr></table>";
            Sl = Sl + 1;
            dayss = 0;
            ttlot = 0;
        }
        private void FindAttendanceOT1(string day, string month, string year, string wrk, ref Int32 dayss, ref Int32 ots, string region, string wrksitecode)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdstring1 = "select COALESCE(SUM(ProvidedOT),0) as OT from tbl_attendance where MONTH(CreatedDate)='" + month + "' and DAY(CreatedDate)='" + day + "' and YEAR(CreatedDate)='" + year + "' and EmployeeWrk='" + wrk + "'and SiteIncharge_Approval='Approved' and JOB_Region='" + region + "' and JOB_SiteCode='" + wrksitecode + "'";
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

        private void BindAttendanceHead2(Int32 minday, Int32 maxday, string month_no, string year, string region, string wrksitecode)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='background-color:#00a8f3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center' colspan='35'> " + month_no.ToString() + " " + year.ToString() + "</span></td></tr></table>";
            BindHeading2(minday, maxday, month_no, year, region, wrksitecode);
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='font:normal 16px/16px Century Gothic; padding:5px 20px 5px 20px;' align='cen ter'></td></tr></table>";
            lblTotalData.Text = str;
        }

        private void BindHeading2(Int32 minday, Int32 maxday, string month_no, string year, string region, string wrksitecode)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>S. NO</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>WORK SL</td>";
            str = str + "<td width='18%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EMPLOYEE NAME</td>";
            for (int i = minday; i <= maxday; i++)
            {
                str = str + "<td width='2%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + i + "</td>";
            }
            str = str + "<td width='10%' style='background-color:#92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>TOTAL PRESENTS</td></tr></table>";

            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            int Sl = 1;
            string findempquery = "select distinct EmployeeWrk, EmployeeName from tbl_attendance where JOB_Region='" + region + "' and JOB_SiteCode='" + wrksitecode + "' and  YEAR(CreatedDate)='" + year + "' and MONTH(CreatedDate)='" + month_no + "' and DAY(CreatedDate) between '"+ minday + "' and '"+ maxday + "'";
            SqlCommand cmd = new SqlCommand(findempquery, dbcl.Conn);
            using (SqlDataReader re = cmd.ExecuteReader())
            {
                Int32 TotaPresentCount = 0;
                while (re.Read())
                {
                    Int32 daycount = 0;
                    Int32 halfdaycount = 0;
                    //Int32 ots = 0;
                    string wrkman = re["EmployeeWrk"].ToString();
                    //string wrkman = "1224";
                    str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:white; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + Sl + "</td>";
                    str = str + "<td width='5%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + wrkman + "</td>";
                    str = str + "<td width='18%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["EmployeeName"] + "</td>";
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
                        FindAttendance2(days, month_no, year, wrkman, ref daycount, ref halfdaycount,region, wrksitecode);
                        day = day + 1;
                    }
                    CalculatePresents2(daycount, halfdaycount, ref TotaPresentCount);
                    str = str + "<td width='10%' style='background-color:white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + TotaPresentCount + "</td></tr></table>";
                    Sl = Sl + 1;
                    dayss = 0;
                }
            }
        }

        private void CalculatePresents2(Int32 daycount, Int32 TOtalHalfPresents, ref Int32 TotaPresentCount)
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
        private void FindAttendance2(string day, string month, string year, string wrk, ref Int32 dayss, ref Int32 Halfdays,string region, string wrksitecode)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdstring1 = "select max(AttendanceStatus) as AttendanceStatus, AttendanceCode, COALESCE(SUM(ProvidedOT),0) as OT from tbl_attendance where MONTH(CreatedDate)='" + month + "' and DAY(CreatedDate)='" + day + "' and YEAR(CreatedDate)='" + year + "' and EmployeeWrk='" + wrk + "' and SiteIncharge_Approval='Approved' and JOB_Region='" + region + "' and JOB_SiteCode='" + wrksitecode + "' group by AttendanceCode";
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
                str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
            }
            dbcl.Conn.Close();
        }

        //----------------------------------------------- OT Sheets --------------------------------------------------------------//

        private void BindAttendanceHead3(Int32 minday, Int32 maxday, string month_no, string year, string region, string wrksitecode)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='background-color:#00a8f3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center' colspan='35'> " + month_no.ToString() + " " + year.ToString() + "</span></td></tr></table>";
            BindHeadingRow3(minday, maxday, month_no, year, region, wrksitecode);
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='font:normal 16px/16px Century Gothic; padding:5px 20px 5px 20px;' align='cen ter'></td></tr></table>";
            lblTotalData.Text = str;
        }

        private void BindHeadingRow3(Int32 minday, Int32 maxday, string month_no, string year, string region, string wrksitecode)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>S. NO</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>WORK SL</td>";
            str = str + "<td width='18%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EMPLOYEE NAME</td>";
            for (int i = minday; i <= maxday; i++)
            {
                str = str + "<td width='2%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + i + "</td>";
            }
            str = str + "<td width='10%' style='background-color:#92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>TOTAL PRESENTS</td></tr></table>";

            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            int Sl = 1;
            string findempquery = "select distinct EmployeeWrk, EmployeeName from tbl_attendance where JOB_Region='" + region + "' and JOB_SiteCode='" + wrksitecode + "' and  YEAR(CreatedDate)='" + year + "' and MONTH(CreatedDate)='" + month_no + "' and DAY(CreatedDate) between '" + minday + "' and '" + maxday + "'";
            SqlCommand cmd = new SqlCommand(findempquery, dbcl.Conn);
            using (SqlDataReader re = cmd.ExecuteReader())
            {
                while (re.Read())
                {
                    Int32 daycount = 0;
                    decimal ots = .0m;
                    decimal ttlot = .0m;
                    string wrkman = re["EmployeeWrk"].ToString();
                    str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:white; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + Sl + "</td>";
                    str = str + "<td width='5%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + wrkman + "</td>";
                    str = str + "<td width='18%' style='background-color: white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["EmployeeName"] + "</td>";
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
                        FindAttendance3(days, month_no, year, wrkman, ref daycount, ref ots, region, wrksitecode);
                        day = day + 1;
                        ttlot = ttlot + ots;
                    }
                    str = str + "<td width='10%' style='background-color:white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + ttlot + "</td></tr></table>";
                    Sl = Sl + 1;
                    dayss = 0;
                    ttlot = 0;
                }
            }
        }

        private void FindAttendance3(string day, string month, string year, string wrk, ref Int32 dayss, ref decimal ots,string region, string wrksitecode)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdstring1 = "select COALESCE(SUM(ProvidedOT),0) as OT from tbl_attendance where MONTH(CreatedDate)='" + month + "' and DAY(CreatedDate)='" + day + "' and YEAR(CreatedDate)='" + year + "' and EmployeeWrk='" + wrk + "' and SiteIncharge_Approval='Approved' and JOB_Region='" + region + "' and JOB_SiteCode='" + wrksitecode + "'";
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
                        str = str + "<td width='2%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + OT + "</td>";
                    }
                    else
                    {
                        ots = .0m;
                        str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>0</td>";
                    }
                }
            }
            else
            {
                str = str + "<td width='2%' style='background-color: yellow; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>0</td>";
            }
            dbcl.Conn.Close();
        }

        //----------------------------------- Combined Sheets - Double Line ------------------------------------------------------------------//
        private void BindAttendanceHead4(Int32 minday, Int32 maxday, string month_no, string year, string region, string wrksitecode)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='background-color:#00a8f3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center' colspan='35'> " + month_no.ToString() + " " + year.ToString() + "</span></td></tr></table>";
            BindHeading4(minday, maxday, month_no, year, region, wrksitecode);
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='font:normal 16px/16px Century Gothic; padding:5px 20px 5px 20px;' align='cen ter'></td></tr></table>";
            lblTotalData.Text = str;
        }


        //Function for binding Absent / Present Row
        private void BindHeading4(Int32 minday, Int32 maxday, string month_no, string year, string region, string wrksitecode)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='5%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>S. NO</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>WORK SL</td>";
            str = str + "<td width='18%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EMPLOYEE NAME</td>";
            for (int i = minday; i <= maxday; i++)
            {
                str = str + "<td width='2%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + i + "</td>";
            }
            str = str + "<td width='10%' style='background-color:#92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>TOTAL PRESENTS</td></tr></table>";

            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            int Sl = 1;
            string findempquery = "select distinct EmployeeWrk, EmployeeName from tbl_attendance where JOB_Region='" + region + "' and JOB_SiteCode='" + wrksitecode + "' and  YEAR(CreatedDate)='" + year + "' and MONTH(CreatedDate)='" + month_no + "' and DAY(CreatedDate) between '" + minday + "' and '" + maxday + "'";
            SqlCommand cmd = new SqlCommand(findempquery, dbcl.Conn);
            using (SqlDataReader re = cmd.ExecuteReader())
            {
                while (re.Read())
                {
                    Int32 daycount = 0;
                    //Int32 ots = 0;
                    string wrkman = re["EmployeeWrk"].ToString();
                    string emp_name = re["EmployeeName"].ToString();
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
                        FindAttendance4(days, month_no, year, wrkman, ref daycount, region, wrksitecode);
                        day = day + 1;
                    }
                    str = str + "<td width='10%' style='background-color:white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + daycount + "</td></tr></table>";
                    BindHeadingRow4(minday, maxday, month_no, year, region, wrkman, emp_name, wrksitecode);
                    Sl = Sl + 1;
                    dayss = 0;
                }
            }
        }
        private void FindAttendance4(string day, string month, string year, string wrk, ref Int32 dayss, string region, string wrksitecode)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdstring1 = "select max(AttendanceStatus) as AttendanceStatus, AttendanceCode, COALESCE(SUM(ProvidedOT),0) as OT from tbl_attendance where MONTH(CreatedDate)='" + month + "' and DAY(CreatedDate)='" + day + "' and YEAR(CreatedDate)='" + year + "' and EmployeeWrk='" + wrk + "' and SiteIncharge_Approval='Approved' and JOB_Region='" + region + "' and JOB_SiteCode='" + wrksitecode + "' group by AttendanceCode";
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
        private void BindHeadingRow4(Int32 minday, Int32 maxday, string month_no, string year, string region, string wrkman, string emp_name, string wrksitecode)
        {
            int Sl = 1;
            Int32 daycount = 0;
            decimal ots = .0m;
            decimal ttlot = .0m;
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
                FindAttendanceOT4(days, month_no, year, wrkman, ref daycount, ref ots, region, wrksitecode);
                day = day + 1;
                ttlot = ttlot + ots;
            }
            str = str + "<td width='10%' style='background-color:white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + ttlot + "</td></tr></table>";
            Sl = Sl + 1;
            dayss = 0;
            ttlot = .0m;
        }

        private void FindAttendanceOT4(string day, string month, string year, string wrk, ref Int32 dayss, ref decimal ots, string region, string wrksitecode)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdstring1 = "select COALESCE(SUM(ProvidedOT),0) as OT from tbl_attendance where MONTH(CreatedDate)='" + month + "' and DAY(CreatedDate)='" + day + "' and YEAR(CreatedDate)='" + year + "' and EmployeeWrk='" + wrk + "' and SiteIncharge_Approval='Approved' and JOB_Region='" + region + "' and JOB_SiteCode='" + wrksitecode + "'";
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

        protected void DDL_Company_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_Company.SelectedIndex != 0)
            {
                string CmdString2 = "select Worksite_Name, DB_Code from tlb_atsworksites where WorkRegion_Code='" + DDL_Region.SelectedValue.ToString() + "' and Company_Code = '" + DDL_Company.SelectedValue.ToString() + "' order by Id";
                BindWorkSites(CmdString2);


            }
            else
            {
                DDL_Company.Focus();
            }
        }

        private void BindWorkSites(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Worksite.DataSource = Cmd.ExecuteReader();
            DDL_Worksite.DataTextField = "Worksite_Name";
            DDL_Worksite.DataValueField = "DB_Code";
            DDL_Worksite.DataBind();
            DDL_Worksite.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void btn_excelexport_Click(object sender, EventArgs e)
        {
            string strt = "AttendanceReport";
            string regn = DDL_Worksite.SelectedItem.Text.ToString();
            string month = DDL_Month.SelectedItem.Text.ToString();
            string year = DDL_Year.SelectedItem.Text.ToString();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + strt + "_" + regn + "_"+month+"_"+year+".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.Output.Write(Request.Form[hfGridHtml.UniqueID]);
            Response.Flush();
            Response.End();
        }
    }
}