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
    public partial class generate_pfsheets : System.Web.UI.Page
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
            string current_month2 = DDL_Month.SelectedValue.ToString();

            int month = int.Parse(current_month2);
            int year = int.Parse(current_year);
            int daysInMonth = DateTime.DaysInMonth(year, month);

            string strtday = "01";
            string endday = daysInMonth.ToString("D2");

            BindDefaultHeader(strtday, endday, current_year, current_month2, region);

            btnExport.Enabled = true;
        }


        private void BindDefaultHeader(string Date1, string Date2, string Year, string Month, string Region)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>SL NO</td>";
            str = str + "<td width='1%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>WL SO</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>FULLNAME</td>";
            str = str + "<td width='4%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>UAN NO</td>";
            str = str + "<td width='8%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>GROSS WAGES</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EPF WAGES</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EPS WAGES</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EDLI WAGES</td>";
            str = str + "<td width='1%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EPF CONTRIBUTION (EE Share)</td>";
            //str = str + "<td width='1%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EPF EPS DIFF REMITTED</td>";
            //str = str + "<td width='1%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>--</td>";
            str = str + "<td width='1%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EPS CONTRIBUTION (ER Share 8.33%)</td>";
            str = str + "<td width='1%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>EPF CONTRIBUTION (ER Share 3.67%)</td>";
            str = str + "<td width='1%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>SALARY DAYS</td>";
            str = str + "<td width='1%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>NCP DAYS</td>";
            str = str + "<td width='1%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>REFUND OF ADVANCES</td>";
            BindRBIData(Year, Month, Region, Date1, Date2);
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='100%' style='font:normal 16px/16px Century Gothic; padding:5px 20px 5px 20px;' align='cen ter'></td></tr></table>";
            lblTotalData.Text = str;
        }

        private void BindRBIData_old(string Year, string Month, string Region, string Date1, string Date2)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdString = "select ROW_NUMBER() OVER (ORDER BY b.Id) AS SrNo, a.WorkmanSL, a.FullName, b.UANNo, CEILING(a.BasicSalary) as Basic1, CEILING(a.BasicSalary) as Basic2, CEILING(a.BasicSalary) as Basic3, CEILING(a.BasicSalary) as Basic4, a.PFPay,CEILING(ROUND((a.BasicSalary*0.0367),0,3)) as Value1, CEILING(ROUND((a.BasicSalary*0.0833),0)) as Value2, a.SalaryEndDay, a.Present, (a.SalaryEndDay-a.Present) as NCPday, 0 as refund from tbl_trialpayroll a, tbl_Employee_Mustertable b where a.WorkRegion ='" + Region + "' and a.SalaryMonth='" + Month + "' and a.SalaryYear='" + Year + "' and a.SalaryStartDay='" + Date1 + "' and a.SalaryEndDay='" + Date2 + "' and a.WorkmanSL=b.WorkmanSL and a.ViewMode=1 and a.DeleteMode=0 order by a.Id";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            using (SqlDataReader re = cmd.ExecuteReader())
            {
                while (re.Read())
                {
                    str = str + "<tr><td width = '2%' style = 'border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align = 'center' >" + re["SrNo"].ToString() + "</ td > ";
                    str = str + "<td width='3%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["WorkmanSL"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["FullName"].ToString() + "</td>";
                    str = str + "<td width='8%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["UANNo"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Basic1"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Basic2"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Basic3"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Basic4"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["PFPay"].ToString() + "</td>";
                    //str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Value1"].ToString() + "</td>";
                    //str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Value2"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Value2"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Value1"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["SalaryEndDay"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["NCPday"].ToString() + "</td>";
                    str = str + "<td width='4%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["refund"].ToString() + "</td></tr>";
                }
            }
        }

        private void BindRBIData(string Year, string Month, string Region, string Date1, string Date2)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            // Updated query with parameterized values
            string cmdString = @"
            SELECT 
                ROW_NUMBER() OVER (ORDER BY b.Id) AS SrNo, 
                a.WorkmanSL, 
                a.FullName, 
                b.UANNo, 
                CEILING(a.BasicSalary) AS Basic1, 
                CEILING(a.BasicSalary) AS Basic2, 
                CASE 
                    WHEN CEILING(a.BasicSalary) > 15000 THEN 15000 
                    ELSE CEILING(a.BasicSalary) 
                END AS Basic3,  
                CASE 
                    WHEN CEILING(a.BasicSalary) > 15000 THEN 15000 
                    ELSE CEILING(a.BasicSalary) 
                END AS Basic4, 
                a.PFPay, 
                CEILING(ROUND((a.BasicSalary * 0.0367), 0, 3)) AS Value1, 
                CEILING(ROUND((a.BasicSalary * 0.0833), 0)) AS Value2, 
                a.SalaryEndDay, 
                a.Present, 
                (a.SalaryEndDay - a.Present) AS NCPday, 
                0 AS refund 
            FROM 
                tbl_trialpayroll a, 
                tbl_Employee_Mustertable b 
            WHERE 
                a.WorkRegion = @Region 
                AND a.SalaryMonth = @Month 
                AND a.SalaryYear = @Year 
                AND a.SalaryStartDay = @Date1 
                AND a.SalaryEndDay = @Date2 
                AND a.WorkmanSL = b.WorkmanSL
                and a.ViewMode=1 and a.DeleteMode=0 
            ORDER BY 
            a.Id";

            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;

            // Adding parameters to the command
            cmd.Parameters.AddWithValue("@Region", Region);
            cmd.Parameters.AddWithValue("@Month", Month);
            cmd.Parameters.AddWithValue("@Year", Year);
            cmd.Parameters.AddWithValue("@Date1", Date1);
            cmd.Parameters.AddWithValue("@Date2", Date2);

            using (SqlDataReader re = cmd.ExecuteReader())
            {
                while (re.Read())
                {
                    str = str + "<tr><td width='2%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["SrNo"].ToString() + "</td>";
                    str = str + "<td width='3%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["WorkmanSL"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["FullName"].ToString() + "</td>";
                    //str = str + "<td width='8%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["UANNo"].ToString() + "</td>";
                    str = str + "<td width='8%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px; mso-number-format:\"\\@\";' align='center'>" + re["UANNo"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Basic1"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Basic2"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Basic3"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Basic4"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["PFPay"].ToString() + "</td>";
                    //str = str + "<td width='5%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Value1"].ToString() + "</td>";
                    //str = str + "<td width='5%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Value2"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Value2"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Value1"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["SalaryEndDay"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["NCPday"].ToString() + "</td>";
                    str = str + "<td width='4%' style='border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["refund"].ToString() + "</td></tr>";
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

        private string BuildEcrText(string Year, string Month, string Region, string Date1, string Date2)
        {
            var sb = new System.Text.StringBuilder();
            const string delim = "#~#";

            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            // NOTE: Basic1 = Gross, Basic2 = EPF Wages, Basic3 = EPS Wages, Basic4 = EDLI Wages (capped 15k)
            string sql = @"
            SELECT 
                a.FullName,
                b.UANNo,
                CEILING(a.BasicSalary) AS Gross,              -- Basic1
                CEILING(a.BasicSalary) AS EPF_Wages,          -- Basic2
                CASE WHEN CEILING(a.BasicSalary) > 15000 THEN 15000 ELSE CEILING(a.BasicSalary) END AS EPS_Wages, -- Basic3
                CASE WHEN CEILING(a.BasicSalary) > 15000 THEN 15000 ELSE CEILING(a.BasicSalary) END AS EDLI_Wages, -- Basic4
                a.PFPay AS EPF_EE_12,                         -- Employee share (12%). If needed, compute: CEILING(ROUND(a.BasicSalary*0.12,0))
                CEILING(ROUND((a.BasicSalary * 0.0833), 0)) AS EPS_ER_833,  -- Employer EPS 8.33%
                CEILING(ROUND((a.BasicSalary * 0.0367), 0)) AS EPF_ER_367   -- Employer EPF 3.67%
            FROM tbl_trialpayroll a
            INNER JOIN tbl_Employee_Mustertable b ON a.WorkmanSL = b.WorkmanSL
            WHERE a.WorkRegion   = @Region
              AND a.SalaryMonth  = @Month
              AND a.SalaryYear   = @Year
              AND a.SalaryStartDay = @Date1
              AND a.SalaryEndDay   = @Date2
                and a.ViewMode=1 and a.DeleteMode=0
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
                        // Force safe strings (trim, avoid delimiter conflicts if any)
                        string uan = (re["UANNo"] ?? "").ToString().Trim();
                        string name = (re["FullName"] ?? "").ToString().Trim();

                        // Integers as strings
                        string gross = Convert.ToInt32(re["Gross"]).ToString();
                        string epfW = Convert.ToInt32(re["EPF_Wages"]).ToString();
                        string epsW = Convert.ToInt32(re["EPS_Wages"]).ToString();
                        string edliW = Convert.ToInt32(re["EDLI_Wages"]).ToString();
                        string epfEE = Convert.ToInt32(re["EPF_EE_12"]).ToString();     // 12%
                        string epsER = Convert.ToInt32(re["EPS_ER_833"]).ToString();    // 8.33%
                        string epfER = Convert.ToInt32(re["EPF_ER_367"]).ToString();    // 3.67%

                        // Per your note: NCP Days = 0, Refund of Advances = 0
                        const string ncpDays = "0";
                        const string refund = "0";

                        sb.Append(uan).Append(delim)
                          .Append(name).Append(delim)
                          .Append(gross).Append(delim)
                          .Append(epfW).Append(delim)
                          .Append(epsW).Append(delim)
                          .Append(edliW).Append(delim)
                          .Append(epfEE).Append(delim)
                          .Append(epsER).Append(delim)
                          .Append(epfER).Append(delim)
                          .Append(ncpDays).Append(delim)
                          .Append(refund).AppendLine();
                    }
                }
            }

            return sb.ToString();
        }

        protected void btn_txtdownload_Click_OLD(object sender, EventArgs e)
        {
            string regn = region; // your existing variable
            string month = DDL_Month.SelectedItem.Text;
            string year = DDL_Year.SelectedItem.Text;

            string current_year = DDL_Year.SelectedItem.Text.ToString();
            string current_month2 = DDL_Month.SelectedValue.ToString();

            int month1 = int.Parse(current_month2);
            int year1 = int.Parse(current_year);
            int daysInMonth = DateTime.DaysInMonth(year1, month1);

            string strtday = "01";
            string endday = daysInMonth.ToString("D2");

            string payload = BuildEcrText(year, current_month2, regn, strtday, endday);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "text/plain";
            Response.AddHeader("Content-Disposition", $"attachment;filename=ECR_{regn}_{month}_{year}.txt");
            // Avoid BOM; Write string directly
            Response.Write(payload);
            Response.Flush();
            Response.End();
        }

        protected void btn_txtdownload_Click(object sender, EventArgs e)
        {
            string current_year = DDL_Year.SelectedItem.Text.ToString();
            string current_month2 = DDL_Month.SelectedValue.ToString();

            int month1 = int.Parse(current_month2);
            int year1 = int.Parse(current_year);
            int daysInMonth = DateTime.DaysInMonth(year1, month1);

            string strtday = "01";
            string endday = daysInMonth.ToString("D2");

            var rows = LoadRows(DDL_Year.SelectedValue, DDL_Month.SelectedValue, region,
                                strtday as string ?? "", endday as string ?? "");
            string payload = BuildPfTxt(rows);

            Response.Clear(); Response.Buffer = true;
            Response.ContentType = "text/plain";
            Response.AddHeader("Content-Disposition",
                $"attachment;filename=ECR_{region}_{DDL_Month.SelectedValue}_{DDL_Year.SelectedValue}.txt");
            Response.Write(payload);
            Response.Flush(); Response.End();
        }

        // Simple DTO to hold payroll data
        public class PayrollRow
        {
            public int Id { get; set; }
            public string WorkmanSL { get; set; }
            public string FullName { get; set; }
            public string UANNo { get; set; }
            public string ESICNo { get; set; }
            public decimal BasicSalary { get; set; }
            public decimal Present { get; set; }
            public decimal PFPay { get; set; }
            public decimal ESICGross { get; set; }
        }

        private List<PayrollRow> LoadRows(string year, string month, string region, string date1, string date2)
        {
            var key = MakeKey(year, month, region, date1, date2);
            var cached = HttpRuntime.Cache[key] as List<PayrollRow>;
            if (cached != null) return cached;

            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            var rows = new List<PayrollRow>();
            string sql = @"
              SELECT a.Id, a.WorkmanSL, a.FullName, a.BasicSalary, a.Present, a.PFPay, a.ESICGross,
                     b.UANNo, b.ESICNo
              FROM tbl_trialpayroll a
              INNER JOIN tbl_Employee_Mustertable b ON a.WorkmanSL=b.WorkmanSL
              WHERE a.WorkRegion=@Region AND a.SalaryMonth=@Month AND a.SalaryYear=@Year
                AND a.SalaryStartDay=@Date1 AND a.SalaryEndDay=@Date2 and a.ViewMode=1 and a.DeleteMode=0
              ORDER BY a.Id;";

            using (var cmd = new SqlCommand(sql, dbcl.Conn))
            {
                cmd.Parameters.AddWithValue("@Region", region);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Date1", date1);
                cmd.Parameters.AddWithValue("@Date2", date2);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        rows.Add(new PayrollRow
                        {
                            Id = r["Id"] == DBNull.Value ? 0 : Convert.ToInt32(r["Id"]),
                            WorkmanSL = (r["WorkmanSL"] ?? "").ToString(),
                            FullName = (r["FullName"] ?? "").ToString(),
                            UANNo = (r["UANNo"] ?? "").ToString(),
                            ESICNo = (r["ESICNo"] ?? "").ToString(),
                            BasicSalary = r["BasicSalary"] == DBNull.Value ? 0 : Convert.ToDecimal(r["BasicSalary"]),
                            Present = r["Present"] == DBNull.Value ? 0 : Convert.ToDecimal(r["Present"]),
                            PFPay = r["PFPay"] == DBNull.Value ? 0 : Convert.ToDecimal(r["PFPay"]),
                            ESICGross = r["ESICGross"] == DBNull.Value ? 0 : Convert.ToDecimal(r["ESICGross"])
                        });
                    }
                }
            }

            HttpRuntime.Cache.Insert(key, rows, null,
                DateTime.UtcNow.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);

            return rows;
        }

        private string MakeKey(string y, string m, string r, string d1, string d2) => $"PAYROLL:{y}:{m}:{r}:{d1}:{d2}";

        private string BuildPfTxt(List<PayrollRow> rows)
        {
            const string delim = "#~#";
            // Rough capacity estimate: ~70 chars per line * rows
            var sb = new System.Text.StringBuilder(rows.Count * 80);

            foreach (var x in rows)
            {
                int basic = Ceil(x.BasicSalary);
                int gross = basic;
                int epfW = basic;
                int epsW = basic;
                int edliW = Math.Min(basic, 15000);

                int epfEE = x.PFPay > 0 ? (int)Math.Round(x.PFPay, 0) : Ceil(x.BasicSalary * 0.12m);
                int epsER = Ceil(x.BasicSalary * 0.0833m);
                int epfER = Ceil(x.BasicSalary * 0.0367m);

                const string ncp = "0";
                const string refund = "0";

                sb.Append(x.UANNo).Append(delim)
                  .Append((x.FullName ?? "").Trim()).Append(delim)
                  .Append(gross).Append(delim)
                  .Append(epfW).Append(delim)
                  .Append(epsW).Append(delim)
                  .Append(edliW).Append(delim)
                  .Append(epfEE).Append(delim)
                  .Append(epsER).Append(delim)
                  .Append(epfER).Append(delim)
                  .Append(ncp).Append(delim)
                  .Append(refund).AppendLine();
            }
            return sb.ToString();
        }

        // at class level
        private static readonly System.Text.RegularExpressions.Regex _esicNameNonAlpha =
            new System.Text.RegularExpressions.Regex(@"[^A-Za-z ]+", System.Text.RegularExpressions.RegexOptions.Compiled);
        private static readonly System.Text.RegularExpressions.Regex _esicNameMultiSpace =
            new System.Text.RegularExpressions.Regex(@"\s{2,}", System.Text.RegularExpressions.RegexOptions.Compiled);

        private static string SanitizeNameForEsicFast(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "";
            var s = _esicNameNonAlpha.Replace(name, " ");
            return _esicNameMultiSpace.Replace(s, " ").Trim();
        }

        private static int Ceil(decimal v) => (int)Math.Ceiling(v);

    }
}