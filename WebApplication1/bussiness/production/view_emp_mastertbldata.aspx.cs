using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using ClosedXML.Excel;
using System.Configuration;
using System.IO;

namespace WebApplication1.bussiness.production
{
    public partial class view_emp_mastertbldata : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        public static string state = string.Empty;
        public static string region = string.Empty;
        public static string comp = string.Empty;
        public static string datalock = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
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
                        //Session["Changer"]= null;
                    }
                    else
                    {
                        region = Session["REGION"].ToString();
                        comp = Session["COMPANY_CODE"].ToString();
                        state = Session["STATE"].ToString();
                        datalock = "0";
                    }

                    string CmdString2 = "select * from tbl_Employee_Mustertable where WorkRegion = '" + region + "' and WorkCompany='" + comp + "' order by Id desc";
                    BindGrid(CmdString2);
                }
            }
        }

        protected void ExportExcel(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select ROW_NUMBER() OVER (ORDER BY Id) AS SlNo, WorkStatus, WorkRegion, WorkCompany, WorkmanSL, FirstName, MiddleName, LastName, FullName, Fathername, BloodGroup, MobileNo, DOB, Qualification, DOJ, DOR, WorkSite, SkillCategory, SkillDesignation, User_RoleType, Role_Permission, SafetyPassNo, SafetyPassExpiry, GatePassNo, GatePassExpiry, PVExpiry, UANNo, ESICNo, Payment_Bank, Payment_Account, Payment_IFSC, BankBranch, QualificationDB from tbl_Employee_Mustertable where WorkRegion = '" + region + "' and WorkCompany='" + comp + "' order by Id"))

                //using (SqlCommand cmd = new SqlCommand("select ROW_NUMBER() OVER (ORDER BY Id) AS SlNo,WorkStatus, LoginID, WorkRegion, WorkCompany, WorkmanSL, FirstName, MiddleName, LastName, FullName, Fathername, BloodGroup, MobileNo, DOB, Qualification, DOJ, DOR, WorkSite, SkillCategory, SkillDesignation, User_RoleType, Role_Permission, WorkHours, OTFactor, SafetyPassNo, SafetyPassExpiry, GatePassNo, GatePassExpiry, PVExpiry, UANNo, ESICNo, Payment_Bank, Payment_Account, Payment_IFSC, BankBranch, QualificationDB, FixedSalary_YesNo, FixedAmount, DA_VDA, HRA, Conv_Allowance, Medical_Allowance, Washing_Allowance, ATT_Allowance, SPCL_Allowance, Misc_Earnings, OTMultiplier, OT_Divisibility from tbl_Employee_Mustertable where WorkRegion = '" + region + "' and WorkCompany='" + comp + "' order by Id"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dt, "Customers");

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=EmpExport.xlsx");
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
            }
        }

        private void BindGrid(string cmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            ad.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            dbcl.Conn.Close();
        }

        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            string CmdString2 = "select * from tbl_Employee_Mustertable where WorkRegion = '" + region + "' and WorkCompany='" + comp + "' order by Id desc";
            BindGrid(CmdString2);
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                Label lbl_WorkStatus = (Label)GridView1.Rows[i].FindControl("lbl_WorkStatus");

                Button btn_workstatus = (Button)GridView1.Rows[i].FindControl("btn_workstatus");

                string jobidstatus = lbl_WorkStatus.Text.ToString();

                if (jobidstatus == "Active")
                {
                    btn_workstatus.CssClass = "btn btn-success btn-sm";
                }
                else
                {
                    btn_workstatus.CssClass = "btn btn-sm btn-danger";
                }
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //string rowIndex = Convert.ToString(e.CommandArgument);

            //Determine the RowIndex of the Row whose Button was clicked.
            int rowIndex = Convert.ToInt32(e.CommandArgument);

            //Reference the GridView Row.
            GridViewRow row = GridView1.Rows[rowIndex];

            //Fetch value of Name.
            string dbid = (row.FindControl("lbl_Id") as Label).Text;
            string empwrk = (row.FindControl("lbl_WorkmanSL") as Label).Text;
            string empname = (row.FindControl("lbl_FullName") as Label).Text;
            string workstatus = (row.FindControl("lbl_WorkStatus") as Label).Text;

            if (e.CommandName == "Swap_WorkStatus")
            {
                if (workstatus == "InActive")
                {
                    dbcl.executeRdr("update tbl_Employee_Mustertable set WorkStatus ='Active' where WorkmanSL='" + empwrk + "' and Id = '" + dbid + "'");
                    string title = "Notification :";
                    string body = "Status Changed";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    dbcl.executeRdr("update tbl_Employee_Mustertable set WorkStatus ='InActive' where WorkmanSL='" + empwrk + "' and Id = '" + dbid + "'");
                    string title = "Notification :";
                    string body = "Status Changed";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                string CmdString2 = "select * from tbl_Employee_Mustertable where WorkRegion = '" + region + "' and WorkCompany='" + comp + "' order by Id desc";
                BindGrid(CmdString2);
            }
            else if (e.CommandName == "View_Details")
            {
                Response.Redirect("viewupdate_empmustertabledata.aspx?ID=" + empwrk + "");
            }
        }
    }
}