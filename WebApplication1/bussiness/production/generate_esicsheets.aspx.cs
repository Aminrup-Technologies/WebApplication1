using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.bussiness.production
{
    public partial class generate_esicsheets : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        string str = string.Empty;
        public static string state = string.Empty;
        public static string region = string.Empty;
        public static string comp = string.Empty;
        public static string datalock = string.Empty;


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
                    if (Session["Changer"] != null)
                    {
                        string[] retrievedArray = (string[])Session["Changer"];
                        region = retrievedArray[1].ToString();
                        comp = retrievedArray[2].ToString();
                        state = retrievedArray[0].ToString();
                        datalock = retrievedArray[3].ToString();
                        Session["Changer"] = null;
                        Session["Changer"] = retrievedArray;
                    }
                    else
                    {
                        region = Session["REGION"].ToString();
                        comp = Session["COMPANY_CODE"].ToString();
                        state = Session["STATE"].ToString();
                        datalock = "0";
                        string[] Bindervalue = { state, region, comp, "1" };
                        Session["Changer"] = null;
                        Session["Changer"] = Bindervalue;
                    }

                    string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='" + state + "' and Work_Region_Code = '" + region + "' order by Id ";
                    BindCompany(CmdString3);

                    dbcl.BindMonthAndYearDropdowns(DDL_Month, DDL_Year);
                    btnExport.Enabled = false;
                }
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

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string current_year = DDL_Year.SelectedItem.Text.ToString();
            string current_month1 = DDL_Month.SelectedItem.Text.ToString();
            string current_month2 = DDL_Month.SelectedValue.ToString();

            int month = int.Parse(current_month2);
            int year = int.Parse(current_year);
            int daysInMonth = DateTime.DaysInMonth(year, month);

            string strtday = "01";
            string endday = daysInMonth.ToString("D2");
            Int32 minday = Convert.ToInt32(strtday);
            Int32 maxday = Convert.ToInt32(endday);

            BindDefaultHeader(strtday, endday, current_year, current_month2, region);

            btnExport.Enabled = true;
        }


        private void BindDefaultHeader_OLD(string Date1, string Date2, string Year, string Month, string Region)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>SL NO</td>";
            str = str + "<td width='1%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>WL SO</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>FULLNAME</td>";
            str = str + "<td width='4%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>ESIC NO</td>";
            str = str + "<td width='8%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>Present</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>ESIC GROSS</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>---</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>LAST WORKING DATE</td>";
            BindRBIData(Year, Month, Region, Date1, Date2);
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='font:normal 16px/16px Century Gothic; padding:5px 20px 5px 20px;' align='cen ter'></td></tr></table>";
            lblTotalData.Text = str;
        }

        private void BindDefaultHeader(string Date1, string Date2, string Year, string Month, string Region)
        {
            str += "<table width='100%' style='border-collapse:collapse;'><tr>";
            str += "<td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight:bold; padding:10px 0;' align='center'>SL NO</td>";
            str += "<td width='3%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight:bold; padding:10px 0;' align='center'>WL SO</td>";
            str += "<td width='10%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight:bold; padding:10px 0;' align='center'>IP Name</td>";
            str += "<td width='8%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight:bold; padding:10px 0; mso-number-format:\"\\@\";' align='center'>IP Number (10 Digits)</td>";
            str += "<td width='6%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight:bold; padding:10px 0;' align='center'>No of Days</td>";
            str += "<td width='6%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight:bold; padding:10px 0;' align='center'>Total Monthly Wages</td>";
            str += "<td width='6%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight:bold; padding:10px 0;' align='center'>Reason Code</td>";
            str += "<td width='8%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight:bold; padding:10px 0;' align='center'>Last Working Day</td>";
            str += "</tr>";
            BindRBIData(Year, Month, Region, Date1, Date2);
            str += "</table>";
            lblTotalData.Text = str;
        }


        private void BindRBIData_OLD(string Year, string Month, string Region, string Date1, string Date2)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdString = "select ROW_NUMBER() OVER (ORDER BY b.Id) AS SrNo, b.ESICNo, a.WorkmanSL, a.FullName, a.Present, a.ESICGross, IIF(a.ESICGross > 20999.00, '1', IIF(a.Present = 0., '1', '0')) as value2, 0 as lastdate from tbl_trialpayroll a, tbl_Employee_Mustertable b where a.WorkRegion ='" + Region + "' and a.SalaryMonth='" + Month + "' and a.SalaryYear='" + Year + "' and a.SalaryStartDay='" + Date1 + "' and a.SalaryEndDay='" + Date2 + "' and a.WorkmanSL=b.WorkmanSL order by a.Id";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            using (SqlDataReader re = cmd.ExecuteReader())
            {
                while (re.Read())
                {
                    str = str + "<tr><td width = '2%' style = 'border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align = 'center' >" + re["SrNo"].ToString() + "</ td > ";
                    str = str + "<td width='3%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["WorkmanSL"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["FullName"].ToString() + "</td>";
                    str = str + "<td width='8%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["ESICNo"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Present"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["ESICGross"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["value2"].ToString() + "</td>";
                    str = str + "<td width='4%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["lastdate"].ToString() + "</td></tr>";
                }
            }
        }

        private void BindRBIData(string Year, string Month, string Region, string Date1, string Date2)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            string cmdString = @"
        SELECT 
            ROW_NUMBER() OVER (ORDER BY a.Id) AS SrNo,
            a.WorkmanSL,
            a.FullName,
            b.ESICNo,
            a.Present,
            a.ESICGross
        FROM tbl_trialpayroll a
        INNER JOIN tbl_Employee_Mustertable b ON a.WorkmanSL = b.WorkmanSL
        WHERE a.WorkRegion    = @Region
          AND a.SalaryMonth   = @Month
          AND a.SalaryYear    = @Year
          AND a.SalaryStartDay = @Date1
          AND a.SalaryEndDay   = @Date2
        ORDER BY a.Id;";

            using (SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Region", Region);
                cmd.Parameters.AddWithValue("@Month", Month);
                cmd.Parameters.AddWithValue("@Year", Year);
                cmd.Parameters.AddWithValue("@Date1", Date1);
                cmd.Parameters.AddWithValue("@Date2", Date2);

                using (SqlDataReader re = cmd.ExecuteReader())
                {
                    while (re.Read())
                    {
                        // values
                        string workmanSl = (re["WorkmanSL"] ?? "").ToString();
                        string fullName = (re["FullName"] ?? "").ToString();
                        string esicNo = (re["ESICNo"] ?? "").ToString().Trim();

                        int days = (int)Math.Round(Convert.ToDecimal(re["Present"] == DBNull.Value ? 0 : re["Present"]));
                        int wages = Convert.ToInt32(Math.Round(Convert.ToDecimal(re["ESICGross"] == DBNull.Value ? 0 : re["ESICGross"])));

                        // compute here (no DB columns needed)
                        int reasonCode = (days == 0 ? 11 : 0);
                        string lastWorkingDay = ""; // leave blank (no column available)

                        // render row
                        str += "<tr>";
                        str += $"<td style='border:1px solid #595959; padding:6px;' align='center'>{re["SrNo"]}</td>";
                        str += $"<td style='border:1px solid #595959; padding:6px;' align='center'>{workmanSl}</td>";
                        str += $"<td style='border:1px solid #595959; padding:6px;' align='center'>{fullName}</td>";
                        str += $"<td style='border:1px solid #595959; padding:6px; mso-number-format:\"\\@\";' align='center'>{esicNo}</td>";
                        str += $"<td style='border:1px solid #595959; padding:6px;' align='center'>{days}</td>";
                        str += $"<td style='border:1px solid #595959; padding:6px;' align='center'>{wages}</td>";
                        str += $"<td style='border:1px solid #595959; padding:6px;' align='center'>{reasonCode}</td>";
                        str += $"<td style='border:1px solid #595959; padding:6px;' align='center'>{lastWorkingDay}</td>";
                        str += "</tr>";
                    }
                }
            }
        }



        protected void btn_excelexport_Click(object sender, EventArgs e)
        {
            string strt = "PFSheet";
            string regn = region;
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

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            Response.Redirect("generate_banksheets.aspx");
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("pyrl_managedashbrd.aspx");
        }

        private string SanitizeNameForEsic(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "";
            var s = System.Text.RegularExpressions.Regex.Replace(name, @"[^A-Za-z ]+", " ");
            s = System.Text.RegularExpressions.Regex.Replace(s, @"\s{2,}", " ").Trim();
            return s;
        }

        private string BuildEsicCsv(string Year, string Month, string Region, string Date1, string Date2,
                                    bool includeHeader = true, bool tabSeparated = false)
        {
            var sb = new System.Text.StringBuilder();
            string sep = tabSeparated ? "\t" : ",";

            if (includeHeader)
            {
                sb.Append(string.Join(sep, new[]{
            "IP Number (10 Digits)",
            "IP Name",
            "No of Days for which wages paid/payable during the month",
            "Total Monthly Wages",
            "Reason Code for Zero working days",
            "Last Working Day"
        })).AppendLine();
            }

            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            string sql = @"
        SELECT 
            b.ESICNo,
            a.FullName,
            a.Present,
            a.ESICGross
        FROM tbl_trialpayroll a
        INNER JOIN tbl_Employee_Mustertable b ON a.WorkmanSL = b.WorkmanSL
        WHERE a.WorkRegion    = @Region
          AND a.SalaryMonth   = @Month
          AND a.SalaryYear    = @Year
          AND a.SalaryStartDay = @Date1
          AND a.SalaryEndDay   = @Date2
        ORDER BY a.Id;";

            using (var cmd = new SqlCommand(sql, dbcl.Conn))
            {
                cmd.Parameters.AddWithValue("@Region", Region);
                cmd.Parameters.AddWithValue("@Month", Month);
                cmd.Parameters.AddWithValue("@Year", Year);
                cmd.Parameters.AddWithValue("@Date1", Date1);
                cmd.Parameters.AddWithValue("@Date2", Date2);

                using (var re = cmd.ExecuteReader())
                {
                    while (re.Read())
                    {
                        string ip = (re["ESICNo"] ?? "").ToString().Trim();
                        // left-pad to 10 if needed
                        if (ip.Length < 10) ip = ip.PadLeft(10, '0');

                        string name = SanitizeNameForEsic((re["FullName"] ?? "").ToString());

                        int days = (int)Math.Round(Convert.ToDecimal(re["Present"] == DBNull.Value ? 0 : re["Present"]));
                        int wages = Convert.ToInt32(Math.Round(Convert.ToDecimal(re["ESICGross"] == DBNull.Value ? 0 : re["ESICGross"])));

                        int reason = (days == 0 ? 11 : 0);
                        string lwd = ""; // no column available; leave blank when reason==0 or unknown

                        if (!tabSeparated) name = "\"" + name.Replace("\"", "\"\"") + "\"";

                        sb.Append(ip).Append(sep)
                          .Append(name).Append(sep)
                          .Append(days).Append(sep)
                          .Append(wages).Append(sep)
                          .Append(reason).Append(sep)
                          .Append(lwd).AppendLine();
                    }
                }
            }
            return sb.ToString();
        }

        protected void btn_esicdownload_Click(object sender, EventArgs e)
        {
            string regn = region;
            string month = DDL_Month.SelectedValue;
            string year = DDL_Year.SelectedValue;

            string current_year = DDL_Year.SelectedItem.Text.ToString();
            string current_month1 = DDL_Month.SelectedItem.Text.ToString();
            string current_month2 = DDL_Month.SelectedValue.ToString();

            int month1 = int.Parse(current_month2);
            int year1 = int.Parse(current_year);
            int daysInMonth = DateTime.DaysInMonth(year1, month1);

            string strtday = "01";
            string endday = daysInMonth.ToString("D2");

            bool tabSeparated = false; // set true if your portal wants TSV
            string payload = BuildEsicCsv(year, month, regn, strtday, endday, includeHeader: true, tabSeparated: tabSeparated);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = tabSeparated ? "text/tab-separated-values" : "text/csv";
            Response.AddHeader("Content-Disposition", $"attachment;filename=ESIC_{regn}_{month}_{year}.csv");
            Response.Write(payload);
            Response.Flush();
            Response.End();
        }



    }
}