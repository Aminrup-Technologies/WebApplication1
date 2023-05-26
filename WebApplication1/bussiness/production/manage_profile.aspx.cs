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
    public partial class manage_profile : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        Payroll_OH4Y PayRoll = new Payroll_OH4Y();
        DataTable dt = new DataTable();

        public static string UserPass = "";
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
                    //Function to load Login Details
                    LoadLoginDetails();

                    txt_oldpass.Text = "";
                    txt_oldpass.Focus();
                }
            }
            else
            {

            }
        }

        private void LoadLoginDetails()
        {

            string query = "select * from tbl_Employee_Mustertable where WorkmanSL=@WorkmanSL and LoginID=@LoginID";
            SqlParameter[] pram = {
                                          new SqlParameter("@WorkmanSL",Session["WORKMAN"].ToString()),
                                          new SqlParameter("@LoginID",Session["USERID"].ToString()),
                                      };
            dt = dbcl.SPreturn_dt(query, pram);
            if (dt.Rows.Count > 0)
            {
                string WorkStatus = dt.Rows[0]["WorkStatus"].ToString();

                if (WorkStatus == "Active")
                {
                    string UserID = dt.Rows[0]["LoginID"].ToString();
                    txt_atsloginid.Text = UserID;

                    string Workman = dt.Rows[0]["WorkmanSL"].ToString();
                    txt_atsworkmenno.Text = Workman;

                    UserPass = dt.Rows[0]["LoginPassword"].ToString();


                }
                else
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "AlertMessage", "<script>alert('User ID is InActive');</script>");
                }
            }
        }

        protected void txt_oldpass_TextChanged(object sender, EventArgs e)
        {
            string inputoldpass = txt_oldpass.Text.TrimEnd().ToString();

            if (UserPass == inputoldpass)
            {
                //Bind the Security Question DDL
                string CmdString1 = "select Security_Questions, QNo from tlb_security_questions where Category = '1' order by Id ";
                BindSecurityQ1(CmdString1);

                string CmdString2 = "select Security_Questions, QNo from tlb_security_questions where Category = '2' order by Id ";
                BindSecurityQ2(CmdString2);

                txt_newpass1.ReadOnly = false;
                txt_newpass2.ReadOnly = false;

                txt_SQAns1.ReadOnly = false;
                txt_SQAns2.ReadOnly = false;

                txt_oldpass.BorderColor = System.Drawing.Color.Green;

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPasswordModal();", true);

                txt_oldpass.ReadOnly = true;

                DDL_SQ1.SelectedIndex = 0;
                DDL_SQ2.SelectedIndex = 0;

                txt_newpass1.Text = "";
                txt_newpass2.Text = "";

                txt_SQAns1.Text = "";
                txt_SQAns2.Text = "";

                btn_svpass.Enabled = true;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPasswordModal();", true);

                txt_oldpass.Text = "";
                txt_oldpass.Focus();
                txt_oldpass.BorderColor = System.Drawing.Color.Red;

                txt_newpass1.ReadOnly = true;
                txt_newpass2.ReadOnly = true;

                txt_newpass1.Text ="";
                txt_newpass2.Text = "";

                txt_SQAns1.ReadOnly = true;
                txt_SQAns2.ReadOnly = true;

                txt_SQAns1.Text ="";
                txt_SQAns2.Text = "";

                DDL_SQ1.SelectedIndex = 0;
                DDL_SQ2.SelectedIndex = 0;

                btn_svpass.Enabled = false;
            }
        }

        private void BindSecurityQ1(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_SQ1.DataSource = Cmd.ExecuteReader();
            DDL_SQ1.DataTextField = "Security_Questions";
            DDL_SQ1.DataValueField = "QNo";
            DDL_SQ1.DataBind();
            DDL_SQ1.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        private void BindSecurityQ2(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_SQ2.DataSource = Cmd.ExecuteReader();
            DDL_SQ2.DataTextField = "Security_Questions";
            DDL_SQ2.DataValueField = "QNo";
            DDL_SQ2.DataBind();
            DDL_SQ2.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void btn_svpass_Click(object sender, EventArgs e)
        {
            if (UpdateLoginCredentials() == true)
            {
                //If Login Credentials Update Successfull
                btn_svpass.Enabled = false;
                btn_svpass.Text = "Success!";
                btn_svpass.CssClass = "btn btn-success btn-sm";

                btn_discardsvpass.Enabled = false;
                btn_discardsvpass.Text = "Success!";
                btn_discardsvpass.CssClass = "btn btn-success btn-sm";

                btn_relogin.Enabled = true;
                //Session.Abandon();
                //Response.Redirect("login.aspx");

            }
            else
            {
                lbl_msgpass.ForeColor = System.Drawing.Color.Green;
                lbl_msgpass.Text = "Unsuccessfull Attempt.....!!";
                //If updating the login credentials failed
            }

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPasswordModal();", true);
        }

        private Boolean UpdateLoginCredentials()
        {
            Boolean flag = false;
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_Employee_Mustertable set LoginPassword=@LoginPassword, SQ1=@SQ1, SQAns1=@SQAns1, SQ2=@SQ2, SQAns2=@SQAns2, Pass_UpdateDate=@Pass_UpdateDate , PassUpdatedByName=@PassUpdatedByName, PassUpdatedByWrk=@PassUpdatedByWrk, PasswordExpiry=@PasswordExpiry where WorkmanSL=@WorkmanSL and LoginID=@LoginID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@WorkmanSL", txt_atsworkmenno.Text.ToString());
                cmd.Parameters.AddWithValue("@LoginID", txt_atsloginid.Text.ToString());
                cmd.Parameters.AddWithValue("@LoginPassword", txt_newpass2.Text.Trim().ToString());
                cmd.Parameters.AddWithValue("@SQ1", DDL_SQ1.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@SQAns1", txt_SQAns1.Text.ToString());
                cmd.Parameters.AddWithValue("@SQ2", DDL_SQ2.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@SQAns2", txt_SQAns2.Text.ToString());
                cmd.Parameters.AddWithValue("@Pass_UpdateDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@PassUpdatedByName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@PassUpdatedByWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@PasswordExpiry", DateTime.Today.AddDays(180));
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                flag = true;

                lbl_msgpass.ForeColor = System.Drawing.Color.Green;
                lbl_msgpass.Text = "Login Credentials Updated Successfully....!!";
            }
            catch (Exception ex)
            {
                flag = false;
                lbl_msgpass.ForeColor = System.Drawing.Color.Red;
                lbl_msgpass.Text = "Error: " + ex.Message.ToString();
            }

            return flag;
        }

        protected void btn_discardsvpass_Click(object sender, EventArgs e)
        {
            txt_oldpass.Text = "";
            txt_oldpass.ReadOnly = false;

            txt_newpass1.Text = "";
            txt_newpass1.ReadOnly = true;

            txt_newpass2.Text = "";
            txt_newpass2.ReadOnly = true;

            DDL_SQ1.SelectedIndex = 0;
            txt_SQAns1.Text = "";
            txt_SQAns1.ReadOnly = true;

            DDL_SQ2.SelectedIndex = 0;
            txt_SQAns2.Text = "";
            txt_SQAns2.ReadOnly = true;
        }

        protected void btn_relogin_Click(object sender, EventArgs e)
        {
            //Update loginstatus and Last Login Information i.e. date
            dbcl.UPDT_EmpMuster_LogoutInfo(Session["WORKMAN"].ToString(), Session["USERID"].ToString());

            Session.Abandon();
            Response.Redirect("login.aspx");
        }
    }
}