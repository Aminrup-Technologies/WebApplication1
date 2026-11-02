/* When: 2026-10-05 | Why: Restore the established legacy Bank Sheet export contract without changing payroll/business behavior | What: Restored the existing legacy export pathway and preserved the established bank-sheet format, structure, and presentation. */
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
        decimal bankSheetAmountTotal;
        int bankSheetRow;

        public static string state = string.Empty;
        public static string region = string.Empty;
        public static string comp = string.Empty;

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

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string current_year = DDL_Year.SelectedItem.Text.ToString();
            string current_month2 = DDL_Month.SelectedValue.ToString();

            int month = int.Parse(current_month2);
            int year = int.Parse(current_year);
            int daysInMonth = DateTime.DaysInMonth(year, month);

            string strtday = "01";
            string endday = daysInMonth.ToString("D2");

            if (DDL_ReportType.SelectedIndex == 1)
            {
                BindDefaultHeader(current_year, current_month2, region);
            }
            else if (DDL_ReportType.SelectedIndex == 2)
            {
                BindDefaultHeader2(current_year, current_month2, region);
                //-------------------
            }
            Button1.Enabled = true;
            btnExport.Enabled = true;
        }


        private void BindDefaultHeader(string Year, string Month, string Region)
        {
            bankSheetAmountTotal = 0;
            bankSheetRow = 0;
            string head = "font-family:Calibri; font-size:11pt; font-weight:bold; color:#FFFFFF; background-color:#4472C4; border-top:1px solid #8EA9DB; border-bottom:1px solid #8EA9DB; border-left:none; border-right:none;";
            string total = "font-family:Calibri; font-size:11pt; font-weight:bold; color:#000000; background-color:#FFFFFF; border-top:3px double #4472C4; border-bottom:1px solid #8EA9DB; border-left:none; border-right:none;";
            str = str + "<table style='border-collapse:collapse; font-family:Calibri; font-size:11pt;'><tr style='height:15pt;'>";
            str = str + "<td style='" + head + " width:53px; border-left:1px solid #8EA9DB;'>SrNo</td>";
            str = str + "<td style='" + head + " width:98px;'>WorkmanSL</td>";
            str = str + "<td style='" + head + " width:114px;'>CreditAccount</td>";
            str = str + "<td style='" + head + " width:158px;'>FullName</td>";
            str = str + "<td style='" + head + " width:140px;'>Payment_Account</td>";
            str = str + "<td style='" + head + " width:165px;'>Payment_Bank</td>";
            str = str + "<td style='" + head + " width:114px;'>Payment_IFSC</td>";
            str = str + "<td style='" + head + " width:147px;'>BankBranch</td>";
            str = str + "<td style='" + head + " width:100px; border-right:1px solid #8EA9DB;'>NetPayFinal 1</td></tr>";
            BindRBIData(Year, Month, Region);
            str = str + "<tr style='height:15pt;'>";
            str = str + "<td style='" + total + " border-left:1px solid #8EA9DB;'></td>";
            str = str + "<td style='" + total + "'></td>";
            str = str + "<td style='" + total + "'></td>";
            str = str + "<td style='" + total + "'></td>";
            str = str + "<td style='" + total + "'></td>";
            str = str + "<td style='" + total + "'></td>";
            str = str + "<td style='" + total + "'></td>";
            str = str + "<td style='" + total + "'></td>";
            str = str + "<td style='" + total + " border-right:1px solid #8EA9DB;' align='right'>" + bankSheetAmountTotal.ToString() + "</td></tr></table>";
            lblTotalData.Text = str;
        }

        private void BindDefaultHeader2(string Year, string Month, string Region)
        {
            bankSheetAmountTotal = 0;
            bankSheetRow = 0;
            string head = "font-family:Calibri; font-size:11pt; font-weight:bold; color:#FFFFFF; background-color:#4472C4; border-top:1px solid #8EA9DB; border-bottom:1px solid #8EA9DB; border-left:none; border-right:none;";
            string total = "font-family:Calibri; font-size:11pt; font-weight:bold; color:#000000; background-color:#FFFFFF; border-top:3px double #4472C4; border-bottom:1px solid #8EA9DB; border-left:none; border-right:none;";
            str = str + "<table style='border-collapse:collapse; font-family:Calibri; font-size:11pt;'><tr style='height:15pt;'>";
            str = str + "<td style='" + head + " width:53px; border-left:1px solid #8EA9DB;'>SrNo</td>";
            str = str + "<td style='" + head + " width:98px;'>WorkmanSL</td>";
            str = str + "<td style='" + head + " width:114px;'>CreditAccount</td>";
            str = str + "<td style='" + head + " width:158px;'>FullName</td>";
            str = str + "<td style='" + head + " width:140px;'>Payment_Account</td>";
            str = str + "<td style='" + head + " width:165px;'>Payment_Bank</td>";
            str = str + "<td style='" + head + " width:114px;'>Payment_IFSC</td>";
            str = str + "<td style='" + head + " width:147px;'>BankBranch</td>";
            str = str + "<td style='" + head + " width:100px; border-right:1px solid #8EA9DB;'>NetPay</td></tr>";
            BindRBIData2(Year, Month, Region);
            str = str + "<tr style='height:15pt;'>";
            str = str + "<td style='" + total + " border-left:1px solid #8EA9DB;'></td>";
            str = str + "<td style='" + total + "'></td>";
            str = str + "<td style='" + total + "'></td>";
            str = str + "<td style='" + total + "'></td>";
            str = str + "<td style='" + total + "'></td>";
            str = str + "<td style='" + total + "'></td>";
            str = str + "<td style='" + total + "'></td>";
            str = str + "<td style='" + total + "'></td>";
            str = str + "<td style='" + total + " border-right:1px solid #8EA9DB;' align='right'>" + bankSheetAmountTotal.ToString() + "</td></tr></table>";
            lblTotalData.Text = str;
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
                    string band = (bankSheetRow % 2 == 1) ? "#D9E1F2" : "#FFFFFF";
                    string cell = "font-family:Calibri; font-size:11pt; font-weight:normal; color:#000000; background-color:" + band + "; border-top:1px solid #8EA9DB; border-bottom:1px solid #8EA9DB; border-left:none; border-right:none;";
                    decimal amt;
                    decimal.TryParse(re["NetPayFinal"].ToString(), out amt);
                    bankSheetAmountTotal += amt;
                    str = str + "<tr style='height:15pt;'><td style='" + cell + " border-left:1px solid #8EA9DB;' align='right'>" + re["SrNo"].ToString() + "</td>";
                    str = str + "<td style='" + cell + " mso-number-format:\\@;' align='left'>" + re["WorkmanSL"].ToString() + "</td>";
                    str = str + "<td style='" + cell + "' align='right'>38233797214</td>";
                    str = str + "<td style='" + cell + " mso-number-format:\\@;' align='left'>" + re["FullName"].ToString() + "</td>";
                    str = str + "<td style='" + cell + " mso-number-format:\\@;' align='left'>" + re["Payment_Account"].ToString() + "</td>";
                    str = str + "<td style='" + cell + " mso-number-format:\\@;' align='left'>" + re["Payment_Bank"].ToString() + "</td>";
                    str = str + "<td style='" + cell + " mso-number-format:\\@;' align='left'>" + re["Payment_IFSC"].ToString() + "</td>";
                    str = str + "<td style='" + cell + " mso-number-format:\\@;' align='left'>" + re["BankBranch"].ToString() + "</td>";
                    str = str + "<td style='" + cell + " border-right:1px solid #8EA9DB;' align='right'>" + re["NetPayFinal"].ToString() + "</td></tr>";
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
                    string band = (bankSheetRow % 2 == 1) ? "#D9E1F2" : "#FFFFFF";
                    string cell = "font-family:Calibri; font-size:11pt; font-weight:normal; color:#000000; background-color:" + band + "; border-top:1px solid #8EA9DB; border-bottom:1px solid #8EA9DB; border-left:none; border-right:none;";
                    decimal amt;
                    decimal.TryParse(re["NetPay"].ToString(), out amt);
                    bankSheetAmountTotal += amt;
                    str = str + "<tr style='height:15pt;'><td style='" + cell + " border-left:1px solid #8EA9DB;' align='right'>" + re["SrNo"].ToString() + "</td>";
                    str = str + "<td style='" + cell + " mso-number-format:\\@;' align='left'>" + re["WorkmanSL"].ToString() + "</td>";
                    str = str + "<td style='" + cell + "' align='right'>38233797214</td>";
                    str = str + "<td style='" + cell + " mso-number-format:\\@;' align='left'>" + re["FullName"].ToString() + "</td>";
                    str = str + "<td style='" + cell + " mso-number-format:\\@;' align='left'>" + re["Payment_Account"].ToString() + "</td>";
                    str = str + "<td style='" + cell + " mso-number-format:\\@;' align='left'>" + re["Payment_Bank"].ToString() + "</td>";
                    str = str + "<td style='" + cell + " mso-number-format:\\@;' align='left'>" + re["Payment_IFSC"].ToString() + "</td>";
                    str = str + "<td style='" + cell + " mso-number-format:\\@;' align='left'>" + re["BankBranch"].ToString() + "</td>";
                    str = str + "<td style='" + cell + " border-right:1px solid #8EA9DB;' align='right'>" + re["NetPay"].ToString() + "</td></tr>";
                }
            }
        }


        protected void btn_excelexport_Click(object sender, EventArgs e)
        {
            string strt = "BankSheet";
            //string regn = DDL_Region.SelectedItem.Text.ToString();
            string month = DDL_Month.SelectedItem.Text.ToString();
            string year = DDL_Year.SelectedItem.Text.ToString();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + strt + "_" + region + "_" + month + "_" + year + ".xls");
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


    }
}