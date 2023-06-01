using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading;
using System.IO;

namespace WebApplication1.bussiness.production
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        Payroll_OH4Y PayRoll = new Payroll_OH4Y();

        // Default folder
        static readonly string rootFolder = @"C:\atswork.in\wwwroot\erp_images\ProfilePhoto";
        static readonly string localFolder = @"D:\OH4Y Works\OH4Y_2021\Demo\WebApplication1\WebApplication1\erp_images\ProfilePhoto";

        public static string UserPass = "";
        DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["USERTYPE"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    bool File =  FlieExistence();
                    if (File == true)
                    {
                        ProfilePic_3.Src = "../../erp_images/ProfilePhoto/" + Session["User_Photo"].ToString() + "";
                    }
                    else
                    {
                        ProfilePic_3.Src = "../../erp_images/ProfilePhoto/No_Image.jpg";
                    }
                    
                    // Perform the login action here, such as prompting the user for credentials and validating them
                    lbl_username.Text = Session["USERNAME"].ToString();
                    LoadLoginDetails();
                    EmployeeDataLoader();
                    AttendanceDataBinder();

                    EmployeeDeductionsBinder();
                }
            }
        }

        private bool FlieExistence()
        {
            if (File.Exists(Path.Combine(rootFolder, Session["User_Photo"].ToString())))
            {
                Response.Clear();
                Response.ContentType = "application/octect-stream";
                Response.AppendHeader("content-disposition", "filename=" + Session["User_Photo"].ToString());
                Response.TransmitFile(Server.MapPath(@"\erp_images\Permits\") + Session["User_Photo"].ToString());
                Response.End();
                return true;

            }
            else if (File.Exists(Path.Combine(localFolder, Session["User_Photo"].ToString())))
            {
                Response.Clear();
                Response.ContentType = "application/octect-stream";
                Response.AppendHeader("content-disposition", "filename=" + Session["User_Photo"].ToString());
                Response.TransmitFile(Server.MapPath(@"\erp_images\Permits\") + Session["User_Photo"].ToString());
                Response.End();
                return true;
            }
            else
            {
                string title = "Notifications :";
                string body = "NO Physical File Found...!!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                return false;
            }

            
        }

        private void EmployeeDataLoader()
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
                    lbl_id.Text = UserID;
                    string Region = dt.Rows[0]["WorkRegion"].ToString();
                    lbl_region.Text = Region;
                    string Company = dt.Rows[0]["WorkCompany"].ToString();
                    lbl_wrkcopmany.Text = Company;
                    string state = dt.Rows[0]["WorkState"].ToString();
                    Session["USTATE"] = state;
                    lbl_state.Text = state;

                    string Workman = dt.Rows[0]["WorkmanSL"].ToString();
                    lbl_workmansl.Text = Workman;

                    string User_FirstName = dt.Rows[0]["FirstName"].ToString();
                    string User_FullName = dt.Rows[0]["FullName"].ToString();
                    string User_Type = dt.Rows[0]["User_RoleType"].ToString();
                    string User_Permission = dt.Rows[0]["Role_Permission"].ToString();
                    string User_Worksite = dt.Rows[0]["WorkSite"].ToString();
                    lbl_wrksite.Text = User_Worksite;
                    string User_WRKSTCode = dt.Rows[0]["Worksite_Code"].ToString();
                    string User_Skill = dt.Rows[0]["SkillCategory"].ToString();
                    Session["SKIL"] = User_Skill;
                    string User_Desg = dt.Rows[0]["SkillDesignation"].ToString();
                    Session["DESG"] = User_Desg;

                    string doj = dt.Rows[0]["DOJ"].ToString();
                    DateTime oDate = Convert.ToDateTime(doj);
                    string age = CalculateYourWorkAge(oDate);
                    lbl_workage.Text = age.ToString();

                    lbl_doj.Text = DateBinder(doj);

                    string mobile = dt.Rows[0]["MobileNo"].ToString();
                    //lbl_mobile.Text = mobile;
                    lbl_desg.Text = User_Desg;
                    lbl_skillcat.Text = User_Skill;


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

                    string PasswordExpiry = dt.Rows[0]["PasswordExpiry"].ToString();
                    Int32 psexpdays = 0;
                    FindDaysLeft(PasswordExpiry, ref psexpdays);
                    if (psexpdays <= 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPasswordModal();", true);
                        txt_oldpass.Text = "";
                        txt_oldpass.Focus();
                    }

                    string bankname = dt.Rows[0]["Payment_Bank"].ToString();
                    lbl_bankname.Text = bankname;
                    txt_bankname.Text = bankname;
                    string bankacc = dt.Rows[0]["Payment_Account"].ToString();
                    lbl_accno.Text = bankacc;
                    txt_accno.Text = bankacc;
                    txt_cnfaccno.Text = bankacc;
                    string bankifsc = dt.Rows[0]["Payment_IFSC"].ToString();
                    lbl_ifsc.Text = bankifsc;
                    txt_ifsc.Text = bankifsc;
                    string branch = dt.Rows[0]["BankBranch"].ToString();
                    lbl_branch.Text = branch;
                    txt_branchname.Text = branch;
                    string updtdt = dt.Rows[0]["BankUpdatedOn"].ToString();
                    string updtbyname = dt.Rows[0]["BankUpdatedByName"].ToString();
                    lbl_bankupdtinfo.Text = "Last updated on " + updtdt + " by " + updtbyname + ".";

                    string pfno = dt.Rows[0]["UANNo"].ToString();
                    lbl_pfno.Text = pfno;
                    string esicno = dt.Rows[0]["ESICNo"].ToString();
                    lbl_esicno.Text = esicno;

                    dbcl.DisconnectDb();

                    //Update loginstatus and Last Login Information i.e. date
                    dbcl.UPDT_EmpMuster_LoginInfo(Workman, UserID);
                }
                else
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "AlertMessage", "<script>alert('User ID is InActive');</script>");
                }
            }
        }

        private void EmployeeDeductionsBinder()
        {
            int lastMonth = DateTime.Now.AddMonths(-1).Month;
            int Year = DateTime.Now.Year;

            string query = "select * from tbl_trialpayroll where WorkmanSL=@WorkmanSL and SalaryYear=@SalaryYear and SalaryMonth=@SalaryMonth";
            SqlParameter[] pram = {
                                          new SqlParameter("@WorkmanSL",Session["WORKMAN"].ToString()),
                                          new SqlParameter("@SalaryYear",Year),
                                          new SqlParameter("@SalaryMonth",lastMonth),
                                      };
            dt = dbcl.SPreturn_dt(query, pram);
            if (dt.Rows.Count > 0)
            {
                string pfpay = dt.Rows[0]["PFPay"].ToString();
                lbl_lastpfpay.Text = pfpay;
                string esicpay = dt.Rows[0]["ESICPay"].ToString();
                lbl_lastesicpay.Text = esicpay;
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

        private static string CalculateYourWorkAge(DateTime DOJ)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(DOJ).Ticks).Year - 1;
            DateTime PastYearDate = DOJ.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            int Hours = Now.Subtract(PastYearDate).Hours;
            int Minutes = Now.Subtract(PastYearDate).Minutes;
            int Seconds = Now.Subtract(PastYearDate).Seconds;
            return String.Format("{0} Year(s) {1} Month(s) {2} Day(s)", Years, Months, Days);
        }

        private void AttendanceDataBinder()
        {
            DateTime d = DateTime.Now;
            Int32 month = d.Month;
            string monthname = d.ToString("MMM");
            lbl_calmonth.Text = monthname;
            Int32 year = d.Year;
            lbl_calyear.Text = year.ToString();
            string empwrk = lbl_workmansl.Text.ToString();


            Int32 caldays = DateTime.DaysInMonth(year, d.Month);
            lbl_caldays.Text = caldays.ToString();

            Int32 ttldays = 0;
            PayRoll.FindEmployeeTotalDaysByMonth(month.ToString(), year.ToString(), empwrk, ref ttldays);
            lbl_totalpresent.Text = ttldays.ToString();
            lbl_dayswrkd.Text = ttldays.ToString();

            Int32 ttlp = 0;
            PayRoll.FindEmployeeTotalPresentByMonth(month.ToString(), year.ToString(), empwrk, ref ttlp);
            lbl_presentdayscount.Text = ttlp.ToString();

            Int32 ttlod = 0;
            PayRoll.FindEmployeeTotalODByMonth(month.ToString(), year.ToString(), empwrk, ref ttlod);
            lbl_oddayscount.Text = ttlod.ToString();

            Int32 ttlnh = 0;
            PayRoll.FindEmployeeTotalNHByMonth(month.ToString(), year.ToString(), empwrk, ref ttlnh);
            lbl_nhcount.Text = ttlnh.ToString();

            Int32 ttlfl = 0;
            PayRoll.FindEmployeeTotalFLByMonth(month.ToString(), year.ToString(), empwrk, ref ttlfl);
            lbl_flcount.Text = ttlfl.ToString();

            decimal ttlot = .0m;
            PayRoll.FindEmployeeTotalOTByMonth(month.ToString(), year.ToString(), empwrk, ref ttlot);
            lbl_totalot.Text = ttlot.ToString();
        }

        protected void btn_vwmntlyattn_Click(object sender, EventArgs e)
        {
            Response.Redirect("vw_monthlyatten.aspx");
        }

        protected void btn_bankedit_Click(object sender, EventArgs e)
        {
            if (btn_bankedit.Text.ToString() == "Make Changes")
            {
                txt_bankname.ReadOnly = false;
                txt_accno.ReadOnly = false;
                txt_cnfaccno.ReadOnly = false;
                txt_ifsc.ReadOnly = false;
                txt_branchname.ReadOnly = false;

                btn_bankedit.Text = "Save Changes";

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup1();", true);
            }
            else if (btn_bankedit.Text.ToString() == "Save Changes")
            {

                if (UpdateBankDetails() == true)
                {
                    txt_bankname.ReadOnly = false;
                    txt_accno.ReadOnly = false;
                    txt_cnfaccno.ReadOnly = false;
                    txt_ifsc.ReadOnly = false;
                    txt_branchname.ReadOnly = false;
                    btn_bankedit.Text = "Make Changes";
                    Refresh();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup1();", true);
                }
                else
                {
                    txt_bankname.ReadOnly = true;
                    txt_accno.ReadOnly = true;
                    txt_cnfaccno.ReadOnly = true;
                    txt_ifsc.ReadOnly = true;
                    txt_branchname.ReadOnly = true;
                    btn_bankedit.Text = "Make Changes";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup1();", true);
                }
            }
        }

        private Boolean UpdateBankDetails()
        {
            Boolean flag = false;
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_Employee_Mustertable set Payment_Bank=@Payment_Bank, Payment_Account=@Payment_Account, Payment_IFSC=@Payment_IFSC, BankBranch=@BankBranch, BankUpdatedOn=@BankUpdatedOn, BankUpdatedByName=@BankUpdatedByName , BankUpdatedByWrk=@BankUpdatedByWrk where WorkmanSL=@WorkmanSL and LoginID=@LoginID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@WorkmanSL", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@LoginID", Session["USERID"].ToString());
                cmd.Parameters.AddWithValue("@Payment_Bank", txt_bankname.Text.ToString());
                cmd.Parameters.AddWithValue("@Payment_Account", txt_accno.Text.ToString());
                cmd.Parameters.AddWithValue("@Payment_IFSC", txt_ifsc.Text.ToString());
                cmd.Parameters.AddWithValue("@BankBranch", txt_branchname.Text.ToString());
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
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Error: " + ex.Message.ToString();
            }

            return flag;
        }

        private void Refresh()
        {
            Response.Redirect("homepage.aspx");
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

        protected void btn_cancel_Click(object sender, EventArgs e)
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
                string CmdString = "UPDATE tbl_Employee_Mustertable set GatePassNo=@GatePassNo, GatePassExpiry=@GatePassExpiry, SafetyPassNo=@SafetyPassNo, SafetyPassExpiry=@SafetyPassExpiry, PVExpiry=@PVExpiry, GP_ModifierWrk=@GP_ModifierWrk, GP_ModifierName=@GP_ModifierName, GP_ModifiedDate=@GP_ModifiedDate, GP_UpdateApproval=@GP_UpdateApproval where WorkmanSL=@WorkmanSL and LoginID=@LoginID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@WorkmanSL", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@LoginID", Session["USERID"].ToString());
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
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Error: " + ex.Message.ToString();

                string title = "Notifications :";
                string body = ex.Message.ToString();
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }


        //------------ Added on 05-07-2022----------------//

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

                newpwd_row1.Visible = true; newpwd_row2.Visible = true;
                newpwd_row3.Visible = true; newpwd_row4.Visible = true;
                newpwd_row5.Visible = true; newpwd_row6.Visible = true;
                newpwd_row7.Visible = true; newpwd_row8.Visible = true;
                newpwd_row9.Visible = true; newpwd_row10.Visible = true;
                newpwd_row11.Visible = true; newpwd_row12.Visible = true;

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPasswordModal();", true);

                txt_oldpass.Text = "";
                txt_oldpass.Focus();
                txt_oldpass.BorderColor = System.Drawing.Color.Red;

                txt_newpass1.ReadOnly = true;
                txt_newpass2.ReadOnly = true;

                txt_newpass1.Text = "";
                txt_newpass2.Text = "";

                txt_SQAns1.ReadOnly = true;
                txt_SQAns2.ReadOnly = true;

                txt_SQAns1.Text = "";
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
                btn_closecvpass.Enabled = false;

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

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPasswordModal();", true);
        }
        protected void btn_relogin_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("login.aspx");
        }
        protected void btn_lgout_Click(object sender, EventArgs e)
        {
            LogoutUserfromATS();
        }

        private void LogoutUserfromATS()
        {
            //Update loginstatus and Last Login Information i.e. date
            dbcl.UPDT_EmpMuster_LogoutInfo(Session["WORKMAN"].ToString(), Session["USERID"].ToString());

            Session.Abandon();
            Response.Redirect("login.aspx");
        }
    }
}