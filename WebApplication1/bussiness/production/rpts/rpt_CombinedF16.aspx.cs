using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;


namespace WebApplication1.bussiness.production.rpts
{
    public partial class rpt_CombinedF16 : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        string str = string.Empty;
        Int32 dayss = 0;
        Int32 ots = 0;
        Int32 CalMonthDays = 0;
        Int32 StartDay = 0;
        Int32 EndDay = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string Year = Request.QueryString["Year"].ToString();
            string Month = Request.QueryString["Month"].ToString();
            string Region = Request.QueryString["Region"].ToString();
            string Minday = Request.QueryString["minday"].ToString();
            string Maxday = Request.QueryString["maxday"].ToString();
            string EmpStatus = Request.QueryString["status"].ToString();

            BindAttendanceHead4(Convert.ToInt32(Minday), Convert.ToInt32(Maxday), Month, Year, Region, EmpStatus);
        }

        private void BindAttendanceHead4(Int32 minday, Int32 maxday, string month_no, string year, string region, string EmpStatus)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='background-color:#00a8f3; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center' colspan='35'> Form XVI || " + region + " || " + month_no.ToString() + " / " + year.ToString() + "</span></td></tr></table>";
            BindHeading4(minday, maxday, month_no, year, region, EmpStatus);
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='font:normal 16px/16px Century Gothic; padding:5px 20px 5px 20px;' align='cen ter'></td></tr></table>";
            lblTotalData.Text = str;
        }


        private void BindHeading4(Int32 minday, Int32 maxday, string month_no, string year, string region, string EmpStatus)
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
            string findempquery = "";
            if (EmpStatus == "All")
            {
                findempquery = "select WorkmanSL, FullName from tbl_Employee_Mustertable where WorkRegion='" + region + "' order by Id";
            }
            else
            {
                findempquery = "select WorkmanSL, FullName from tbl_Employee_Mustertable where WorkRegion='" + region + "' and WorkStatus='" + EmpStatus + "' order by Id";
            }

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(findempquery, dbcl.Conn))
            {
                cmd.CommandType = CommandType.Text;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                Int32 daycount = 0;
                //Int32 ots = 0;
                string wrkman = row["WorkmanSL"].ToString();
                string emp_name = row["FullName"].ToString();
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
                    FindAttendance4(days, month_no, year, wrkman, ref daycount, region);
                    day = day + 1;
                }
                str = str + "<td width='10%' style='background-color:white; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + daycount + "</td></tr></table>";
                BindHeadingOTRow4(minday, maxday, month_no, year, region, wrkman, emp_name);
                Sl = Sl + 1;
                dayss = 0;
            }
        }

        private void FindAttendance4(string day, string month, string year, string wrk, ref Int32 dayss, string region)
        {
            string date = day + "-" + month + "-" + year;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            string cmdstring1 = "select max(AttendanceStatus) as AttendanceStatus, AttendanceCode, COALESCE(SUM(ProvidedOT),0) as OT from tbl_attendance where MONTH(CreatedDate)='" + month + "' and DAY(CreatedDate)='" + day + "' and YEAR(CreatedDate)='" + year + "' and EmployeeWrk='" + wrk + "' and SiteIncharge_Approval='Approved' group by AttendanceCode";
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
                    else
                    {
                        str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
                    }
                }
            }
            else
            {
                str = str + "<td width='2%' style='background-color: #ff6c6c; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>A</td>";
            }
            dbcl.Conn.Close();
        }

        //Function for Binding OT Row
        private void BindHeadingOTRow4(Int32 minday, Int32 maxday, string month_no, string year, string region, string wrkman, string emp_name)
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
                FindAttendanceOT4(days, month_no, year, wrkman, ref daycount, ref ots, region);
                day = day + 1;
                ttlot = ttlot + ots;
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
            string cmdstring1 = "select COALESCE(SUM(ProvidedOT),0) as OT from tbl_attendance where CreatedDate='" + date + "' and EmployeeWrk='" + wrk + "' and SiteIncharge_Approval='Approved'";
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

        protected void btn_export_Click(object sender, EventArgs e)
        {
            string strt = "ATS_Form_XVI";
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