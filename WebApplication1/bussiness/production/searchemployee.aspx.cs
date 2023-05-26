using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;

namespace WebApplication1.bussiness.production
{
    public partial class searchemployee : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        DataTable dt = new DataTable();
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
                    Nameinputrow.Visible = true;
                    WorkmanInput_Row.Visible = false;
                }
                else if (DDL_SearchType.SelectedIndex == 2)
                {
                    WorkmanInput_Row.Visible = true;
                    Nameinputrow.Visible = false;
                }
            }
            else
            {
                Nameinputrow.Visible = false;
                WorkmanInput_Row.Visible = false;

                DDL_SearchType.Focus();
                string title = "Notifications :";
                string body = "Please select search type";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (DDL_SearchType.SelectedIndex != 0)
            {
                if (DDL_SearchType.SelectedIndex == 1)
                {
                    if (txt_empname.Text.ToString() != "")
                    {
                        string query = "select * from tbl_Employee_Mustertable where WorkRegion = '" + Session["REGION"].ToString() + "' and FullName like '%" + txt_empname.Text.ToString() + "%'";
                        GridBinder(query);
                        //EmployeeDataLoader();
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
                        string query = "select * from tbl_Employee_Mustertable where WorkRegion = '" + Session["REGION"].ToString() + "' and WorkmanSL = '" + txt_empworkman.Text.ToString() + "'";
                        GridBinder(query);
                        AttViewer.Visible = true;
                        EmployeeDataLoader();
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
            Response.Redirect("searchemployee.aspx");

        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("homepage.aspx");

        }


        private void EmployeeDataLoader()
        {
            string query = "select * from tbl_Employee_Mustertable where WorkmanSL=@WorkmanSL";
            SqlParameter[] pram = {
                                          new SqlParameter("@WorkmanSL",txt_empworkman.Text.ToString()),
                                          //new SqlParameter("@LoginID",Session["USERID"].ToString()),
                                      };
            dt = dbcl.SPreturn_dt(query, pram);
            if (dt.Rows.Count > 0)
            {
                string WorkStatus = dt.Rows[0]["WorkStatus"].ToString();

                if (WorkStatus == "Active")
                {
                    string gpno = dt.Rows[0]["GatePassNo"].ToString();
                    lbl_gpno.Text = gpno;
                    lbl_oldgpno.Text = gpno;
                    txt_nwgpno.Text = gpno;

                    string gpval = dt.Rows[0]["GatePassExpiry"].ToString();
                    Int32 gpdays = 0;
                    FindDaysLeft(gpval, ref gpdays);
                    if (gpdays < 14)
                    {
                        lbl_gpexpdays.ForeColor = Color.OrangeRed;
                        lbl_gpvalidity.ForeColor = Color.OrangeRed;
                        lbl_gpno.ForeColor = Color.OrangeRed;
                        lbl_oldgpno.ForeColor = Color.OrangeRed;

                        string title = "Notifications :";
                        string body = "Kindly update your Gatepass Data, Your Gatepass has expired...!!!";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                    lbl_gpexpdays.Text = gpdays.ToString();
                    string gpvaldt = DateBinder(gpval);
                    lbl_gpvalidity.Text = gpvaldt;
                    lbl_oldgpvalidity.Text = gpvaldt;
                    txt_nwgpvalidity.Text = gpvaldt;

                    string rfidno = dt.Rows[0]["SafetyPassNo"].ToString();
                    lbl_rfidno.Text = rfidno;
                    lbl_oldsftyno.Text = rfidno;
                    txt_nwsftyno.Text = rfidno;

                    string rfidval = dt.Rows[0]["SafetyPassExpiry"].ToString();
                    string rfidvaldt = DateBinder(rfidval);
                    lbl_rfidvalidity.Text = rfidvaldt;
                    lbl_oldsftyval.Text = rfidvaldt;
                    txt_nwsftyvalidity.Text = rfidvaldt;

                    Int32 rfiddays = 0;
                    FindDaysLeft(rfidval, ref rfiddays);
                    lbl_rfiddays.Text = rfiddays.ToString();
                    if (rfiddays < 14)
                    {
                        lbl_rfidno.ForeColor = Color.OrangeRed;
                        lbl_rfidvalidity.ForeColor = Color.OrangeRed;
                        lbl_rfiddays.ForeColor = Color.OrangeRed;
                    }

                    string pvvalidity = dt.Rows[0]["PVExpiry"].ToString();
                    string pvvaldt = DateBinder(pvvalidity);
                    lbl_pvvalidity.Text = pvvaldt;
                    lbl_oldpvvalidity.Text = pvvaldt;
                    txt_nwpvvalidity.Text = pvvaldt;

                    Int32 pvdays = 0;
                    FindDaysLeft(pvvalidity, ref pvdays);
                    lbl_pvdays.Text = pvdays.ToString();
                    if (pvdays < 14)
                    {
                        lbl_pvvalidity.ForeColor = Color.OrangeRed;
                        lbl_pvdays.ForeColor = Color.OrangeRed;
                    }

                    dbcl.DisconnectDb();
                }
                else
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "AlertMessage", "<script>alert('User ID is InActive');</script>");
                }
            }
        }

        public void FindDaysLeft(string emp_outtime, ref Int32 day)
        {
            string currentdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            DateTime intm = DateTime.Parse(currentdate.ToString());
            DateTime outm = DateTime.Parse(emp_outtime.ToString());

            TimeSpan duration = outm.Subtract(intm);
            day = duration.Days;
        }


        private string DateBinder(string date)
        {
            string newdate = "";
            DateTime oDate = Convert.ToDateTime(date);
            string day = "";
            string month = "";
            if (oDate.Day < 10)
            {
                day = "0" + oDate.Day.ToString();
            }
            else
            {
                day = oDate.Day.ToString();
            }
            if (oDate.Month < 10)
            {
                month = "0" + oDate.Month.ToString();
            }
            else
            {
                month = oDate.Month.ToString();
            }
            return newdate = day + "-" + month + "-" + oDate.Year;
        }


        protected void btn_gtpsedit_Click(object sender, EventArgs e)
        {
            if (btn_gtpsedit.Text.ToString() == "Make Changes")
            {
                nwgprow1.Visible = true;
                nwgprow2.Visible = true;

                nwgpvalrow1.Visible = true;
                nwgpvalrow2.Visible = true;

                nwsftyrow1.Visible = true;
                nwsftyrow2.Visible = true;

                nwrfidrow1.Visible = true;
                nwrfidrow2.Visible = true;

                nwpvrow1.Visible = true;
                nwpvrow2.Visible = true;

                btn_gtpsedit.Text = "Save Changes";

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup2();", true);
            }
            else if (btn_gtpsedit.Text.ToString() == "Save Changes")
            {

                ///function to make changes in DB
                ///
                ReflectNewGPData();

                nwgprow1.Visible = false;
                nwgprow2.Visible = false;

                nwgpvalrow1.Visible = false;
                nwgpvalrow2.Visible = false;

                nwsftyrow1.Visible = false;
                nwsftyrow2.Visible = false;

                nwrfidrow1.Visible = false;
                nwrfidrow2.Visible = false;

                nwpvrow1.Visible = false;
                nwpvrow2.Visible = false;

                btn_gtpsedit.Text = "Make Changes";

                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup2();", true);
            }
        }

        protected void btn_gtpscncl_Click(object sender, EventArgs e)
        {


            nwgprow1.Visible = false;
            nwgprow2.Visible = false;

            nwgpvalrow1.Visible = false;
            nwgpvalrow2.Visible = false;

            nwsftyrow1.Visible = false;
            nwsftyrow2.Visible = false;

            nwrfidrow1.Visible = false;
            nwrfidrow2.Visible = false;

            nwpvrow1.Visible = false;
            nwpvrow2.Visible = false;

            btn_gtpsedit.Text = "Make Changes";
        }


        //The function to update the gatepass related changes in DB

        private void ReflectNewGPData()
        {
            string nwgpno = txt_nwgpno.Text.ToString();
            string nwgpval = txt_nwgpvalidity.Text.ToString();

            string nwsftyno = txt_nwsftyno.Text.ToString();
            string nwsftyval = txt_nwsftyvalidity.Text.ToString();

            string nepvval = txt_nwpvvalidity.Text.ToString();

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_Employee_Mustertable set GatePassNo=@GatePassNo, GatePassExpiry=@GatePassExpiry, SafetyPassNo=@SafetyPassNo, SafetyPassExpiry=@SafetyPassExpiry, PVExpiry=@PVExpiry, GP_ModifierWrk=@GP_ModifierWrk, GP_ModifierName=@GP_ModifierName, GP_ModifiedDate=@GP_ModifiedDate, GP_UpdateApproval=@GP_UpdateApproval where WorkmanSL=@WorkmanSL";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@WorkmanSL", txt_empworkman.Text.ToString());
                cmd.Parameters.AddWithValue("@GatePassNo", nwgpno);
                cmd.Parameters.AddWithValue("@GatePassExpiry", nwgpval);
                cmd.Parameters.AddWithValue("@SafetyPassNo", nwsftyno);
                cmd.Parameters.AddWithValue("@SafetyPassExpiry", nwsftyval);
                cmd.Parameters.AddWithValue("@PVExpiry", nepvval);
                cmd.Parameters.AddWithValue("@GP_ModifierWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@GP_ModifierName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@GP_ModifiedDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@GP_UpdateApproval", "Pending");
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                EmployeeDataLoader();

                string title = "Notifications :";
                string body = "Data saved Successfully";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                //lblMessage.ForeColor = System.Drawing.Color.Red;
                //lblMessage.Text = "Error: " + ex.Message.ToString();

                string title = "Notifications :";
                string body = ex.Message.ToString();
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

    }
}