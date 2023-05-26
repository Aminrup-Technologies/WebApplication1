using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.IO;

namespace WebApplication1.bussiness.production
{
    public partial class view_expensedetails : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        DataTable dt = new DataTable();

        // Default folder
        static readonly string rootFolder = @"C:\atswork.in\wwwroot\erp_images\Expenses";

        //static readonly string rootFolder = @"D:\OH4Y Works\OH4Y_2021\ATS_Oct\WebApplication1\WebApplication1\erp_images\Expenses";
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
                    //ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                    string uniqueid = Request.QueryString["EXPID"];
                    Bind_BasicDetails(uniqueid);
                }
            }
        }

        private void Bind_BasicDetails(string uniqueid)
        {
            try
            {
                string query = "select * from tbl_expenselogs where ExpenseID=@ExpenseID";
                SqlParameter[] pram = {
                                          new SqlParameter("@ExpenseID",uniqueid),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    txt_expdate.Text = dt.Rows[0]["LoggedOn"].ToString();
                    txt_workorderno.Text = dt.Rows[0]["WorkOrderNo"].ToString();
                    lbl_expenseid.Text = dt.Rows[0]["ExpenseID"].ToString();
                    string appstatus = dt.Rows[0]["AppStatus"].ToString();
                    lbl_appstatus.Text = appstatus;
                    txt_worksitename.Text = dt.Rows[0]["WorksiteName"].ToString();
                    lbl_worksitedbcode.Text = dt.Rows[0]["WorksiteCode"].ToString();
                    lbl_requesterwrk.Text = dt.Rows[0]["LoggedByWrk"].ToString();
                    txt_requestername.Text = dt.Rows[0]["LoggedByName"].ToString();
                    txt_jobdept.Text = dt.Rows[0]["CompDept"].ToString();
                    txt_jobloc.Text = dt.Rows[0]["Location"].ToString();

                    lbl_appname.Text = dt.Rows[0]["AppByName"].ToString();
                    string appwrk= dt.Rows[0]["AppByWrk"].ToString();
                    lbl_aapwrk.Text = appwrk;

                    if (appwrk == Session["WORKMAN"].ToString())
                    {
                        GridView2.Columns[10].Visible = true;
                    }
                    else
                    {
                        GridView2.Columns[10].Visible = false;
                    }

                    lbl_aapdate.Text = dt.Rows[0]["AppDate"].ToString();
                    if (appstatus == "Approved")
                    {
                        lbl_appstatus.Text = "Approved";
                        lbl_appstatus.ForeColor = Color.Green;
                        //GridView2.Columns[12].Visible = true;
                    }
                    else
                    {
                        lbl_appstatus.Text = "Pending";
                        lbl_appstatus.ForeColor = Color.Red;
                    }

                    string CmdString3 = "select * from tbl_expenselogdetails where ExpenseID='" + uniqueid + "' order by Id desc";
                    BindGrid2(CmdString3);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                lbl_msg.ForeColor = System.Drawing.Color.Red;
                lbl_msg.Text = "Error: " + ex.Message.ToString();
            }
        }

        private void BindGrid2(string cmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            GridView2.DataSource = ds;
            GridView2.DataBind();
            dbcl.Conn.Close();
        }

        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView2.EditIndex = e.NewEditIndex;

            string jobid = lbl_expenseid.Text.ToString();
            string CmdString3 = "select * from tbl_expenselogdetails where ExpenseID='" + jobid + "' order by Id desc";
            BindGrid2(CmdString3);
        }

        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView2.EditIndex = -1;

            string jobid = lbl_expenseid.Text.ToString();
            string CmdString3 = "select * from tbl_expenselogdetails where ExpenseID='" + jobid + "' order by Id desc";
            BindGrid2(CmdString3);
        }

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Label ID = (Label)GridView2.Rows[e.RowIndex].FindControl("lbl_Id");
            //string id = ID.Text.ToString();

            //Label lblexpid = (Label)GridView2.Rows[e.RowIndex].FindControl("lbl_ExpenseID");
            //string expid = lblexpid.Text.ToString();

            //DeleteExpense(id, expid);

            //string jobid = lbl_expenseid.Text.ToString();
            //string CmdString3 = "select * from tbl_expenselogdetails where ExpenseID='" + jobid + "' order by Id desc";
            //BindGrid2(CmdString3);
        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label ID = (Label)GridView2.Rows[e.RowIndex].FindControl("lbl_Id");
            string dbid = ID.Text.ToString();

            Label JOBID = (Label)GridView2.Rows[e.RowIndex].FindControl("lbl_ExpenseID");
            string expid = JOBID.Text.ToString();

            TextBox Quantity = (TextBox)GridView2.Rows[e.RowIndex].FindControl("txt_Quantity");
            string new_qnty = Quantity.Text.ToString();

            TextBox txt_Description = (TextBox)GridView2.Rows[e.RowIndex].FindControl("txt_Description");
            string new_descp = txt_Description.Text.ToString();

            TextBox txt_ClaimAmount = (TextBox)GridView2.Rows[e.RowIndex].FindControl("txt_ClaimAmount");
            string new_amnt = txt_ClaimAmount.Text.ToString();

            UpdateExpenseDetails(dbid, expid, new_qnty, new_descp, new_amnt);

            GridView2.EditIndex = -1;

            string CmdString3 = "select * from tbl_expenselogdetails where ExpenseID='" + expid + "' order by Id desc";
            BindGrid2(CmdString3);
        }

        private void UpdateExpenseDetails(string dbid, string expid, string new_qnty, string new_descp, string new_amnt)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_expenselogdetails set Quantity=@Quantity, Description=@Description, ClaimAmount=@ClaimAmount where Id=@Id and ExpenseID=@ExpenseID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", dbid);
                cmd.Parameters.AddWithValue("@ExpenseID", expid);
                cmd.Parameters.AddWithValue("@Quantity", new_qnty);
                cmd.Parameters.AddWithValue("@Description", new_descp);
                cmd.Parameters.AddWithValue("@ClaimAmount", new_amnt);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                string title = "Notifications :";
                string body = "Data has been UPDATED !!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {

                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void DownloadFile(string dbid)
        {
            try
            {
                string fileName;
                string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "select FileName from tbl_expenselogdetails where Id=@Id";
                        cmd.Parameters.AddWithValue("@Id", dbid);
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            sdr.Read();
                            fileName = sdr["FileName"].ToString();
                        }
                        con.Close();
                    }
                }


                try
                {
                    // Check if file exists with its full path
                    if (File.Exists(Path.Combine(rootFolder, fileName)))
                    {
                        Response.Clear();
                        Response.ContentType = "application/octect-stream";
                        Response.AppendHeader("content-disposition", "filename=" + fileName);
                        Response.TransmitFile(Server.MapPath(@"\erp_images\Expenses\") + fileName);
                        Response.End();

                    }
                    else
                    {
                        string title = "Notifications :";
                        string body = "NO Physical File Found...!!";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }
                catch (IOException ioExp)
                {
                    string title = "Notifications :";
                    string body = ioExp.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                lbl_msg.ForeColor = System.Drawing.Color.Red;
                lbl_msg.Text = "Error: " + ex.Message.ToString();
                //throw;
            }
        }

        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //string jobid = Convert.ToString(e.CommandArgument);

            //Determine the RowIndex of the Row whose Button was clicked.
            int rowIndex = Convert.ToInt32(e.CommandArgument);

            //Reference the GridView Row.
            GridViewRow row = GridView2.Rows[rowIndex];

            //Fetch value of Name.
            string dbid = (row.FindControl("lbl_Id") as Label).Text;
            string expid = (row.FindControl("lbl_ExpenseID") as Label).Text;
            string jobidstatus = (row.FindControl("lbl_AppStatus") as Label).Text;

            if (e.CommandName == "Swap_Status")
            {
                if (jobidstatus == "Pending")
                {
                    JOBID_Status_Swaper(expid, dbid);
                    //Response.Redirect(Request.Url.AbsoluteUri);

                    string title = "Notification :";
                    string body = "Expense is Approved";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    string title = "Notification :";
                    string body = "Already Approved";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                Bind_BasicDetails(expid);
            }
            else if (e.CommandName == "Delete")
            {
                DeleteExpense(dbid, expid);
                Bind_BasicDetails(expid);
            }
            else if (e.CommandName == "DownloadFile")
            {
                DownloadFile(dbid);
                Bind_BasicDetails(expid);
            }


        }

        private void DeleteExpense(string id, string expid)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "delete from tbl_expenselogdetails where Id='" + id + "' and ExpenseID='" + expid + "'  ";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                dbcl.Conn.Close();

                string title = "Notifications :";
                string body = "Data has been DELETED !!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void JOBID_Status_Swaper(string jobid, string dbid)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string statusdata = "";
            string statusdata1 = "";
            string cmdstring = "select AppStatus from tbl_expenselogdetails where ExpenseID='" + jobid + "' and Id = '" + dbid + "'";
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            SqlDataReader re = cmd.ExecuteReader();
            if (re.Read())
            {
                statusdata = re["AppStatus"].ToString();
            }


            if (statusdata == "Approved")
            {
                statusdata1 = "Pending";

                dbcl.executeRdr("update tbl_expenselogdetails set AppStatus ='" + statusdata1 + "' where ExpenseID='" + jobid + "' and Id = '" + dbid + "'");
            }
            else
            {
                statusdata1 = "Approved";
                dbcl.executeRdr("update tbl_expenselogdetails set AppStatus ='" + statusdata1 + "' where ExpenseID='" + jobid + "' and Id = '" + dbid + "'");
            }
            dbcl.Conn.Close();
        }
    }
}