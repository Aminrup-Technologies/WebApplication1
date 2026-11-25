/* When: 2026-10-05 | Why: Restore the established legacy Bank Sheet export contract without changing payroll/business behavior | What: Restored the existing legacy export pathway and preserved the established bank-sheet format, structure, and presentation. */
/* When: 2026-11-03 | Why: Restore approved bank-sheet compatibility and prevent invalid exports. | What: Restore established bank-facing headers/layout, preserve account text formatting, and restrict export enablement to valid generated results. */
/* When: 2026-11-25 | Why: Prevent client-tampered bank-sheet exports. | What: Regenerate authoritative export HTML from the validated server-side payroll query and close the bank-sheet table structure. */
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class generate_banksheets : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        DataTable dt = new DataTable();

        string str = string.Empty;
        int bankSheetRow;

        public static string state = string.Empty;
        public static string region = string.Empty;
        public static string comp = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!HasRequiredSession())
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    ResolveAuthorizedContext();

                    //string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = 'IN' and State_Code ='"+ state + "' order by Id ";
                    //BindRegions(CmdString1);

                    string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='"+ state + "' and Work_Region_Code = '" + region + "' order by Id ";
                    BindCompany(CmdString3);

                    dbcl.BindMonthAndYearDropdowns(DDL_Month, DDL_Year);

                    Button1.Enabled = false;
                    btnExport.Enabled = false;
                    //dbcl.BindMonthAndYearDropdowns(DDL_M2, DDL_Y2);
                }
            }
        }

        private bool HasRequiredSession()
        {
            return Session["USERID"] != null
                && Session["RolePermissionDB"] != null
                && Session["UserRoleDB"] != null
                && Session["USERNAME"] != null
                && Session["WORKMAN"] != null
                && Session["REGION"] != null;
        }

        private void ResolveAuthorizedContext()
        {
            if (Session["Changer"] != null)
            {
                string[] retrievedArray = (string[])Session["Changer"];
                region = retrievedArray[1].ToString();
                comp = retrievedArray[2].ToString();
                state = retrievedArray[0].ToString();
                Session["Changer"] = null;
                Session["Changer"] = retrievedArray;
            }
            else
            {
                region = Session["REGION"].ToString();
                comp = Session["COMPANY_CODE"].ToString();
                state = Session["STATE"].ToString();
                string[] Bindervalue = { state, region, comp, "1" };
                Session["Changer"] = null;
                Session["Changer"] = Bindervalue;
            }
        }

        //------------- Added on 15.02.2023 for excel export of the data displayed on screen ----------------------------//
        protected void ExportExcel(object sender, EventArgs e)
        {
            if (ViewState["Manpower"] != null)
            {
                DataTable dt = (DataTable)ViewState["Manpower"];

                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "ATS_Portal");
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=ATSWebExport.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }
        }
        public DataTable GetDataTable(SqlCommand sqlCmd)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                da.Fill(dt);
                ViewState["Manpower"] = dt;
            }
            catch (Exception)
            {
                dt = null;
                //throw;
            }

            return dt;
        }

        //private void BindRegions(string CmdString)
        //{
        //    dbcl.Sqlconnection();
        //    dbcl.ConnectDb();
        //    SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
        //    Cmd.CommandType = CommandType.Text;
        //    DDL_Region.DataSource = Cmd.ExecuteReader();
        //    DDL_Region.DataTextField = "Work_Region_Name";
        //    DDL_Region.DataValueField = "Work_Region_Code";
        //    DDL_Region.DataBind();
        //    DDL_Region.Items.Insert(0, "Please Select Option");
        //    dbcl.DisconnectDb();
        //}

        //protected void DDL_Region_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='"+ Session["STATE"].ToString() + "' and Work_Region_Code = '" + DDL_Region.SelectedValue.ToString() + "' order by Id ";
        //    BindCompany(CmdString3);
        //}

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

        private string BuildBankSheetSelection(string year, string month, int reportTypeIndex, string workRegion)
        {
            return year + "|" + month + "|" + reportTypeIndex + "|" + workRegion;
        }

        private bool TryGetCurrentBankSheetSelection(out string year, out string monthValue, out int reportTypeIndex, out string selectionKey)
        {
            year = null;
            monthValue = null;
            reportTypeIndex = 0;
            selectionKey = null;

            if (DDL_Year.SelectedItem == null || DDL_Month.SelectedItem == null || DDL_ReportType.SelectedItem == null)
                return false;

            year = DDL_Year.SelectedItem.Text;
            monthValue = DDL_Month.SelectedValue;
            reportTypeIndex = DDL_ReportType.SelectedIndex;
            if (reportTypeIndex != 1 && reportTypeIndex != 2)
                return false;

            selectionKey = BuildBankSheetSelection(year, monthValue, reportTypeIndex, region);
            return true;
        }

        private string BuildAuthoritativeBankSheetHtml(string year, string month, int reportTypeIndex, string workRegion)
        {
            str = string.Empty;
            bankSheetRow = 0;

            if (reportTypeIndex == 1)
                BindDefaultHeader(year, month, workRegion);
            else if (reportTypeIndex == 2)
                BindDefaultHeader2(year, month, workRegion);
            else
                return string.Empty;

            return str;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (!HasRequiredSession())
            {
                btnExport.Enabled = false;
                ViewState["BankSheetSelection"] = null;
                Response.Redirect("~/login.aspx");
                return;
            }

            ResolveAuthorizedContext();

            string current_year = DDL_Year.SelectedItem.Text.ToString();
            string current_month2 = DDL_Month.SelectedValue.ToString();

            int month = int.Parse(current_month2);
            int year = int.Parse(current_year);
            int daysInMonth = DateTime.DaysInMonth(year, month);

            string strtday = "01";
            string endday = daysInMonth.ToString("D2");

            string html = string.Empty;
            if (DDL_ReportType.SelectedIndex == 1 || DDL_ReportType.SelectedIndex == 2)
                html = BuildAuthoritativeBankSheetHtml(current_year, current_month2, DDL_ReportType.SelectedIndex, region);

            lblTotalData.Text = html;
            Button1.Enabled = true;
            btnExport.Enabled = bankSheetRow > 0;
            ViewState["BankSheetSelection"] = btnExport.Enabled
                ? BuildBankSheetSelection(current_year, current_month2, DDL_ReportType.SelectedIndex, region)
                : null;
        }


        private void BindDefaultHeader(string Year, string Month, string Region)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>SL NO</td>";
            str = str + "<td width='1%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>WL SO</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>DEBIT ACCOUNT NO</td>";
            str = str + "<td width='4%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>BENEFICIARY NAME</td>";
            str = str + "<td width='8%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>CREDIT ACC NO</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>BANK NAME</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>IFSC CODE</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>BRANCH NAME</td>";
            str = str + "<td width='1%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>AMOUNT</td></tr>";
            BindRBIData(Year, Month, Region);
            str = str + "</table>";
        }

        private void BindDefaultHeader2(string Year, string Month, string Region)
        {
            str = str + "<table width='100%' style='border-collapse:collapse;'><tr><td width='2%' style='background-color:#92d050; border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align='center'>SL NO</td>";
            str = str + "<td width='1%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>WL SO</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>DEBIT ACCOUNT NO</td>";
            str = str + "<td width='4%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>BENEFICIARY NAME</td>";
            str = str + "<td width='8%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>CREDIT ACC NO</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>BANK NAME</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>IFSC CODE</td>";
            str = str + "<td width='5%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>BRANCH NAME</td>";
            str = str + "<td width='1%' style='background-color: #92d050; border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>AMOUNT</td></tr>";
            BindRBIData2(Year, Month, Region);
            str = str + "</table>";
        }

        private void BindRBIData(string Year, string Month, string Region)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdString = "select ROW_NUMBER() OVER (ORDER BY b.Id) AS SrNo, b.WorkmanSL as WorkmanSL,38233797214 as CreditAccount, b.FullName,  ISNULL((a.Payment_Account), 'N/A') as Payment_Account ,ISNULL((a.Payment_Bank), 'N/A') as Payment_Bank, ISNULL((a.Payment_IFSC), 'N/A') as Payment_IFSC, ISNULL((a.BankBranch), 'N/A') as BankBranch, b.NetPayFinal from tbl_Employee_Mustertable a, tbl_trialpayroll b where b.WorkmanSL = a.WorkmanSL and b.SalaryYear='" + Year + "' and b.SalaryMonth='" + Month + "' and b.WorkRegion='" + Region + "' and b.ViewMode=1 and b.DeleteMode=0 order by b.Id";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            using (SqlDataReader re = cmd.ExecuteReader())
            {
                while (re.Read())
                {
                    bankSheetRow++;
                    str = str + "<tr><td width = '2%' style = 'border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align = 'center' >" + re["SrNo"].ToString() + "</ td > ";
                    str = str + "<td width='3%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["WorkmanSL"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px; mso-number-format:\\@;' align='center'>38233797214</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["FullName"].ToString() + "</td>";
                    str = str + "<td width='8%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px; mso-number-format:\\@;' align='center'>" + re["Payment_Account"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Payment_Bank"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px; mso-number-format:\\@;' align='center'>" + re["Payment_IFSC"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["BankBranch"].ToString() + "</td>";
                    str = str + "<td width='4%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["NetPayFinal"].ToString() + "</td></tr>";
                }
            }
        }


        private void BindRBIData2(string Year, string Month, string Region)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdString = "select ROW_NUMBER() OVER (ORDER BY b.Id) AS SrNo, b.WorkmanSL as WorkmanSL,38233797214 as CreditAccount, b.FullName,  ISNULL((a.Payment_Account), 'N/A') as Payment_Account ,ISNULL((a.Payment_Bank), 'N/A') as Payment_Bank, ISNULL((a.Payment_IFSC), 'N/A') as Payment_IFSC, ISNULL((a.BankBranch), 'N/A') as BankBranch, b.NetPay2 as NetPay from tbl_Employee_Mustertable a, tbl_trialpayroll b where b.WorkmanSL = a.WorkmanSL and b.SalaryYear='" + Year + "' and b.SalaryMonth='" + Month + "' and b.WorkRegion='" + Region + "' and b.ViewMode=1 and b.DeleteMode=0 order by b.Id";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            using (SqlDataReader re = cmd.ExecuteReader())
            {
                while (re.Read())
                {
                    bankSheetRow++;
                    str = str + "<tr><td width = '2%' style = 'border:1px solid #595959; font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;'align = 'center' >" + re["SrNo"].ToString() + "</ td > ";
                    str = str + "<td width='3%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["WorkmanSL"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px; mso-number-format:\\@;' align='center'>38233797214</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["FullName"].ToString() + "</td>";
                    str = str + "<td width='8%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px; mso-number-format:\\@;' align='center'>" + re["Payment_Account"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["Payment_Bank"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px; mso-number-format:\\@;' align='center'>" + re["Payment_IFSC"].ToString() + "</td>";
                    str = str + "<td width='5%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["BankBranch"].ToString() + "</td>";
                    str = str + "<td width='4%' style='border:1px solid #595959;  font:normal 12px/12px Century Gothic; font-weight: bold; padding:10px 0px 10px 0px;' align='center'>" + re["NetPay"].ToString() + "</td></tr>";
                }
            }
        }


        protected void btn_excelexport_Click(object sender, EventArgs e)
        {
            if (!HasRequiredSession())
            {
                btnExport.Enabled = false;
                ViewState["BankSheetSelection"] = null;
                Response.Redirect("~/login.aspx");
                return;
            }

            ResolveAuthorizedContext();

            string year;
            string monthValue;
            int reportTypeIndex;
            string currentSelection;
            string storedSelection = ViewState["BankSheetSelection"] as string;

            if (!btnExport.Enabled
                || !TryGetCurrentBankSheetSelection(out year, out monthValue, out reportTypeIndex, out currentSelection)
                || string.IsNullOrEmpty(storedSelection)
                || storedSelection != currentSelection)
            {
                btnExport.Enabled = false;
                ViewState["BankSheetSelection"] = null;
                return;
            }

            string exportHtml = BuildAuthoritativeBankSheetHtml(year, monthValue, reportTypeIndex, region);
            if (bankSheetRow <= 0 || string.IsNullOrEmpty(exportHtml))
            {
                btnExport.Enabled = false;
                ViewState["BankSheetSelection"] = null;
                lblTotalData.Text = string.Empty;
                return;
            }

            // Keep on-screen grid aligned with the authoritative export payload.
            lblTotalData.Text = exportHtml;

            string strt = "BankSheet";
            //string regn = DDL_Region.SelectedItem.Text.ToString();
            string month = DDL_Month.SelectedItem.Text.ToString();
            string yearText = DDL_Year.SelectedItem.Text.ToString();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + strt + "_" + region + "_" + month + "_" + yearText + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            // Authoritative export: never trust Request.Form[hfGridHtml] for payout/account data.
            Response.Output.Write(exportHtml);
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


    }
}
