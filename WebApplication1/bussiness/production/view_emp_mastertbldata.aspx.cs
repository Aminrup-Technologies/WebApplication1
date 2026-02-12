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
                        //Session["Changer"]= null;
                    }
                    else
                    {
                        region = Session["REGION"].ToString();
                        comp = Session["COMPANY_CODE"].ToString();
                        state = Session["STATE"].ToString();
                        datalock = "0";
                    }

                    // OPTIMIZED QUERY: Only select columns you display
                    string optimizedQuery = @"
                        SELECT Id, WorkmanSL, WorkStatus, FullName, SkillCategory, SkillDesignation, 
                               Fathername, DOR, DOJ, MobileNo, Email, WorkSite, SafetyPassNo, BloodGroup, SafetyPassExpiry, UANNo  -- <--- ADDED THESE COLUMNS
                        FROM tbl_Employee_Mustertable WHERE WorkRegion = '" + region + "' AND WorkCompany='" + comp + "' ORDER BY Id DESC";
                    BindGrid(optimizedQuery);
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
            using (SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn))
            {
                using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                {
                    DataTable ds = new DataTable();
                    ad.Fill(ds);

                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                }
            }
            dbcl.Conn.Close();
        }

        // REQUIRED FOR GENTELELLA DATATABLES TO WORK
        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count > 0)
            {
                // This is REQUIRED for DataTables to work
                GridView1.UseAccessibleHeader = true;
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Swap_WorkStatus")
            {
                // SPLIT THE ARGUMENT WE PASSED IN ASPX
                // Format: "ID,WorkmanSL,CurrentStatus"
                string[] args = e.CommandArgument.ToString().Split(',');
                string dbId = args[0];
                string empWrk = args[1];
                string currentStatus = args[2];

                string newStatus = (currentStatus == "Active") ? "InActive" : "Active";

                // Update Database
                string updateQry = "UPDATE tbl_Employee_Mustertable SET WorkStatus = '" + newStatus + "' WHERE Id = '" + dbId + "'";
                dbcl.executeRdr(updateQry);

                // Show Notification
                string title = "Success";
                string body = "Employee " + empWrk + " is now " + newStatus;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                // Rebind
                string optimizedQuery = @"SELECT Id, WorkmanSL, WorkStatus, FullName, SkillCategory, SkillDesignation, Fathername, DOR, DOJ, MobileNo, Email, WorkSite, SafetyPassNo FROM tbl_Employee_Mustertable WHERE WorkRegion = '" + region + "' AND WorkCompany='" + comp + "' ORDER BY Id DESC";
                BindGrid(optimizedQuery);
            }
            else if (e.CommandName == "View_Details")
            {
                string empWrk = e.CommandArgument.ToString();
                Response.Redirect("viewupdate_empmustertabledata.aspx?ID=" + empWrk);
            }
        }
    }
}