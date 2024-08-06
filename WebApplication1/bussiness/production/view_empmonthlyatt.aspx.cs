using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace WebApplication1.bussiness.production
{
    public partial class view_empmonthlyatt : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    dbcl.CalDateCombo1(DDL_Day, DDL_Month, DDL_Year);
                    DDL_SearchType.Focus();
                }
            }
        }


        protected void DDL_SearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_SearchType.SelectedIndex != 0)
            {
                if (DDL_SearchType.SelectedIndex == 1)
                {
                    Nameinputrow1.Visible = true;
                    Nameinputrow2.Visible = true;

                    WorkmanInput_Row1.Visible = false;
                    WorkmanInput_Row2.Visible = false;
                }
                else if (DDL_SearchType.SelectedIndex == 2)
                {
                    WorkmanInput_Row1.Visible = true;
                    WorkmanInput_Row2.Visible = true;

                    Nameinputrow1.Visible = false;
                    Nameinputrow2.Visible = false;
                }
            }
            else
            {
                Nameinputrow1.Visible = false;
                Nameinputrow2.Visible = false;

                WorkmanInput_Row1.Visible = false;
                WorkmanInput_Row2.Visible = false;

                DDL_SearchType.Focus();
                string title = "Notifications :";
                string body = "Please select search type";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            Binder();
        }

        private void Binder()
        {
            if (DDL_SearchType.SelectedIndex != 0)
            {
                if (DDL_SearchType.SelectedIndex == 1)
                {
                    if (txt_empname.Text.ToString() != "")
                    {
                        string query = "select * from tbl_attendance where YEAR(CreatedDate)='" + DDL_Year.SelectedItem.ToString() + "' and MONTH(CreatedDate)='" + DDL_Month.SelectedItem.Text.ToString() + "' and EmployeeName like '%" + txt_empname.Text.ToString() + "%' order by CreatedDate";
                        GridBinder(query);
                    }
                    else
                    {
                        string title = "Notifications :";
                        string body = "Please enter Employee First Name";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }
                else if (DDL_SearchType.SelectedIndex == 2)
                {
                    if (txt_empworkman.Text.ToString() != "")
                    {
                        string query = "select * from tbl_attendance where YEAR(CreatedDate)='" + DDL_Year.SelectedItem.ToString() + "' and MONTH(CreatedDate)='" + DDL_Month.SelectedItem.Text.ToString() + "' and EmployeeWrk = '" + txt_empworkman.Text.ToString() + "' order by CreatedDate";
                        GridBinder(query);
                    }
                    else
                    {
                        string title = "Notifications :";
                        string body = "Please enter Employee Workman SL";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "Please select search type..!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void GridBinder(string CmdString)
        {
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

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            Response.Redirect("view_empmonthlyatt.aspx");

        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("homepage.aspx");

        }


        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;

            Binder();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;

            Binder();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && GridView1.EditIndex == e.Row.RowIndex)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var DDL_AttendanceStatus = e.Row.FindControl("DDL_AttendanceStatus") as DropDownList;
                    if (DDL_AttendanceStatus != null)
                    {
                        var dt1 = new DataTable();
                        string cnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString();
                        using (var con = new SqlConnection(cnnString1))
                        {
                            con.Open();
                            var cmd1 = new SqlCommand("Select DISTINCT Status from tlb_attendancecodes where Approver='Yes'", con);
                            var da1 = new SqlDataAdapter(cmd1);
                            da1.Fill(dt1);
                            con.Close();
                        }

                        DDL_AttendanceStatus.DataSource = dt1;
                        DDL_AttendanceStatus.DataTextField = "Status";
                        DDL_AttendanceStatus.DataValueField = "Status";
                        DDL_AttendanceStatus.DataBind();
                        string AttendanceStatus = DataBinder.Eval(e.Row.DataItem, "AttendanceStatus").ToString();
                        DDL_AttendanceStatus.Items.FindByText(AttendanceStatus).Selected = true;
                    }



                    var DDL_AttendanceCode = e.Row.FindControl("DDL_AttendanceCode") as DropDownList;
                    if (DDL_AttendanceCode != null)
                    {
                        var dt2 = new DataTable();
                        string cnnString2 = System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString();
                        using (var con = new SqlConnection(cnnString2))
                        {
                            con.Open();
                            var cmd2 = new SqlCommand("Select Status_Name,Status_Code from tlb_attendancecodes where Approver='Yes' order by slno", con);
                            var da2 = new SqlDataAdapter(cmd2);
                            da2.Fill(dt2);
                            con.Close();
                        }

                        DDL_AttendanceCode.DataSource = dt2;
                        DDL_AttendanceCode.DataTextField = "Status_Name";
                        DDL_AttendanceCode.DataValueField = "Status_Code";
                        DDL_AttendanceCode.DataBind();
                        string AttendanceCode = DataBinder.Eval(e.Row.DataItem, "AttendanceCode").ToString();
                        DDL_AttendanceCode.Items.FindByValue(AttendanceCode).Selected = true;
                    }
                }
            }
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            Label JOBID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_JOBID");
            string jobid = JOBID.Text.ToString();

            Label empwrk = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_EmployeeWrk");
            string workmansl = empwrk.Text.ToString();

            Label wrkhrs = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_WourkHours");
            Int32 emp_wrkhours = Convert.ToInt32(wrkhrs.Text.ToString());
            Int32 emp_wrkmnis = emp_wrkhours * 60;

            TextBox TextBoxWithIntime = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_Inpunch_Time");
            string new_intitme = TextBoxWithIntime.Text.ToString();
            DateTime timein = DateTime.ParseExact(new_intitme, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
            string convtimein = timein.ToString("yyyy-MM-dd hh:mm:ss tt");


            TextBox TextBoxWithOuttime = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_Outpunch_Time");
            string new_outtime = TextBoxWithOuttime.Text.ToString();
            DateTime timeout = DateTime.ParseExact(new_outtime, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
            string convtimeout = timeout.ToString("yyyy-MM-dd hh:mm:ss tt");


            DropDownList DDL_Lunch = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDL_LunchYesNo");
            string lunchyesno = DDL_Lunch.SelectedItem.Text.ToString();

            TextBox TextBoxWithOt = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_ProvidedOT");
            string new_ot = TextBoxWithOt.Text.ToString();
            decimal new_pot = Convert.ToDecimal(new_ot);

            DropDownList AttendanceStatus = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDL_AttendanceStatus");
            string ddl_newattensttaus = AttendanceStatus.SelectedItem.Text.ToString();

            DropDownList AttendanceCode = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDL_AttendanceCode");
            string ddl_newattencode = AttendanceCode.SelectedValue.ToString();


            Int32 workdmins = 0;
            decimal workedhours = .0m;
            dbcl.FindEmployeeWorkedTime(convtimein, convtimeout, ref workdmins, ref workedhours);
            decimal emp_calOThrs = .0m;
            dbcl.CalculateOvertime(emp_wrkmnis, workdmins, lunchyesno, ref emp_calOThrs);

            UpdateDetails(id, jobid, workmansl, convtimein, convtimeout, lunchyesno, workdmins, workedhours, emp_calOThrs, new_pot, ddl_newattensttaus, ddl_newattencode);

            GridView1.EditIndex = -1;

            Binder();
        }

        private void UpdateDetails(string id, string jobid, string empwrk, string intime, string outime, string lunchyesno, Int32 workdmins, decimal workedhours, decimal emp_calOThrs, decimal new_pot, string ddl_newattensttaus, string ddl_newattencode)
        {
            try
            {
                DateTime timein = DateTime.ParseExact(intime, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
                string convtimein = timein.ToString("yyyy-MM-dd hh:mm:ss tt");

                DateTime timeout = DateTime.ParseExact(outime, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
                string convtimeout = timeout.ToString("yyyy-MM-dd hh:mm:ss tt");

                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_attendance set Inpunch_Time=@Inpunch_Time, Outpunch_Time=@Outpunch_Time, WorkedTime=@WorkedTime , WorkedHours=@WorkedHours, LunchFactor=@LunchFactor, Calc_OT=@Calc_OT,  ProvidedOT=@ProvidedOT, LastModified=@LastModified, ModifiedByWrk=@ModifiedByWrk,ModifiedByName=@ModifiedByName,  AttendanceStatus=@AttendanceStatus, AttendanceCode=@AttendanceCode where Id=@Id and JOBID=@JOBID and EmployeeWrk=@EmployeeWrk";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                cmd.Parameters.AddWithValue("@EmployeeWrk", empwrk);

                cmd.Parameters.AddWithValue("@Inpunch_Time", convtimein);
                cmd.Parameters.AddWithValue("@Outpunch_Time", convtimeout);
                cmd.Parameters.AddWithValue("@WorkedTime", workdmins);
                cmd.Parameters.AddWithValue("@WorkedHours", workedhours);
                cmd.Parameters.AddWithValue("@LunchFactor", lunchyesno);
                cmd.Parameters.AddWithValue("@Calc_OT", emp_calOThrs);
                cmd.Parameters.AddWithValue("@ProvidedOT", new_pot);
                //cmd.Parameters.AddWithValue("@Approval_Date", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@LastModified", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@ModifiedByWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@ModifiedByName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@AttendanceStatus", ddl_newattensttaus);
                cmd.Parameters.AddWithValue("@AttendanceCode", ddl_newattencode);
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

            //Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string jobid = Convert.ToString(e.CommandArgument);

            if (e.CommandName == "Approve")
            {
                //Response.Redirect("jobapprovalpage.aspx?JOBID=" + jobid + "");
                Response.Write("<script>window.open ('jobapprovalpage.aspx?JOBID=" + jobid + "','_blank');</script>");
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            Label JOBID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_JOBID");
            string jobid = JOBID.Text.ToString();

            try
            {
                string cmdString = ("delete from tbl_attendance where Id='" + id + "' and JOBID= '" + jobid + "'");
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
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
                //throw;
            }


            Binder();


        }
    }
}