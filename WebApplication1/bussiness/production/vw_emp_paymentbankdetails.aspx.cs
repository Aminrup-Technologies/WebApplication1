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
using System.Drawing;

namespace WebApplication1.bussiness.production
{
    public partial class vw_emp_paymentbankdetails : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        LoginUserData retrievedContext = new LoginUserData();

        public static string state = string.Empty;
        public static string region = string.Empty;
        public static string comp = string.Empty;
        public static string datalock = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["USERTYPE"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    // Retrieving from session only if it has a value
                    if (Session["Changer"] != null)
                    {
                        LoginUserData retrievedContext = (LoginUserData)Session["Changer"];

                        // Continue processing with retrievedContext
                        region = retrievedContext.Value;
                        comp = retrievedContext.CompValue;
                        state = retrievedContext.State;
                        datalock = retrievedContext.Datalock;
                    }
                    else
                    {
                        // Set data manually if session value is not available or not of type LoginUserData
                        region = Session["REGION"].ToString();
                        comp = Session["COMPANY_CODE"].ToString();
                        state = Session["STATE"].ToString();
                        datalock = "0";

                        LoginUserData userContext = new LoginUserData();
                        userContext.State = state;
                        userContext.Value = region;
                        userContext.CompValue = comp;
                        userContext.Datalock = "1";
                        Session["Changer"] = userContext;
                    }

                    BindGrid(state, region, comp);
                    DDL_EmpWorkStatus.SelectedIndex = 2;
                }
            }
        }


        //------------- Added on 15.02.2023 for excel export of the data displayed on screen ----------------------------//
        protected void ExportExcel(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select Id, WorkStatus, WorkmanSL, FullName, Fathername, Payment_Bank, Payment_Account, Payment_IFSC, BankBranch from tbl_Employee_Mustertable where WorkState ='" + state + "' and WorkRegion = '" + region + "' and WorkCompany='" + comp + "' and WorkStatus='Active' order by Id desc"))
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
                                wb.Worksheets.Add(dt, "BankDdetails");

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=EmpBankExport.xlsx");
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

        private void BindGrid(string state, string region, string comp)
        {
            string CmdString = "select Id, WorkStatus, WorkmanSL, FullName, Fathername, Payment_Bank, Payment_Account, Payment_IFSC, BankBranch from tbl_Employee_Mustertable where WorkState='" + state + "' and WorkRegion = '" + region + "' and WorkCompany='" + comp + "' and WorkStatus='Active' order by Id desc";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(CmdString, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            dbcl.Conn.Close();
        }


        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGrid(state, region, comp);
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid(state, region, comp);
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            BindGrid(state, region, comp);
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string dbid = ID.Text.ToString();

            Label WorkmanSL = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_WorkmanSL");
            string empwrk = WorkmanSL.Text.ToString();


            //------------------ Added on 29-09-2021----------------------//

            TextBox txt_Payment_Bank = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_Payment_Bank");
            string nw_pymtbank = txt_Payment_Bank.Text.ToString().ToUpper();

            TextBox txt_Payment_Account = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_Payment_Account");
            string nw_accno = txt_Payment_Account.Text.ToString();

            TextBox txt_Payment_IFSC = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_Payment_IFSC");
            string nw_ifsc = txt_Payment_IFSC.Text.ToString();

            TextBox txt_BankBranch = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_BankBranch");
            string nw_branch = txt_BankBranch.Text.ToString().ToUpper();

            if (UpdateBankDetails(dbid, empwrk, nw_pymtbank, nw_accno, nw_ifsc, nw_branch) == true)
            {
                string title = "Notifications :";
                string body = "Data has been UPDATED !!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            else
            {
                string title = "Notifications :";
                string body = "Data NOT UPDATED !!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }

            GridView1.EditIndex = -1;
            BindGrid(state, region, comp);
        }


        private Boolean UpdateBankDetails(string dbid, string empwrk, string nw_pymtbank, string nw_accno, string nw_ifsc, string nw_branch)
        {
            Boolean flag = false;
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_Employee_Mustertable set Payment_Bank=@Payment_Bank, Payment_Account=@Payment_Account, Payment_IFSC=@Payment_IFSC, BankBranch=@BankBranch, BankUpdatedOn=@BankUpdatedOn, BankUpdatedByName=@BankUpdatedByName , BankUpdatedByWrk=@BankUpdatedByWrk where WorkmanSL=@WorkmanSL and Id=@Id";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@WorkmanSL", empwrk);
                cmd.Parameters.AddWithValue("@Id", dbid);
                cmd.Parameters.AddWithValue("@Payment_Bank", nw_pymtbank);
                cmd.Parameters.AddWithValue("@Payment_Account", nw_accno);
                cmd.Parameters.AddWithValue("@Payment_IFSC", nw_ifsc);
                cmd.Parameters.AddWithValue("@BankBranch", nw_branch);
                cmd.Parameters.AddWithValue("@BankUpdatedOn", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@BankUpdatedByName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@BankUpdatedByWrk", Session["WORKMAN"].ToString());
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }

            return flag;
        }
    }
}