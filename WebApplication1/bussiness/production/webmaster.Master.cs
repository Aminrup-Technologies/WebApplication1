using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.bussiness.production;

namespace WebApplication1.gentelella_master.production
{
    public partial class webmaster : System.Web.UI.MasterPage
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();

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
                    Label lbl1 = (Label)Page.Master.FindControl("lbl_loginusername2");
                    lbl1.Text = Session["USERNAME"].ToString();

                    Label lbl2 = (Label)Page.Master.FindControl("lbl_loginusername1");
                    lbl2.Text = Session["USERFNAME"].ToString();

                    ProfilePic_1.Src = "../../erp_images/ProfilePhoto/"+ Session["User_Photo"].ToString() + "";
                    ProfilePic_2.Src = "../../erp_images/ProfilePhoto/" + Session["User_Photo"].ToString() + "";

                    //ProfilePic_1.Src = Session["User_Photo"].ToString();
                    //ProfilePic_2.Src = Session["User_Photo"].ToString();

                    GetIpValue();
                    //GetIpAddress();
                    PermissionCheck();

                    //Label lbl_pendingforappjob = (Label)Page.Master.FindControl("lbl_jobspendingcount");
                    //lbl_pendingforappjob.Text = Convert.ToString(CC.GetPendingJOBApprovalCount(Session["WORKMAN"].ToString()));

                    //Label lbl_approvedjobs = (Label)Page.Master.FindControl("lbl_approvedjobs");
                    //lbl_approvedjobs.Text = Convert.ToString(CC.GetApprovedJOBCount(Session["WORKMAN"].ToString()));

                    //Label lbl_pendingtbt = (Label)Page.Master.FindControl("lbl_tbtpendingapp");
                    //lbl_pendingtbt.Text = Convert.ToString(CC.GetPendingTBTCount(Session["WORKMAN"].ToString()));
                }
            }
        }

        private void GetIpValue()
        {
            string ipAdd = "";
            ipAdd = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ipAdd))
            {
                ipAdd = Request.ServerVariables["REMOTE_ADDR"];
                lbl_IPAddress.Text = ipAdd;
            }
            else
            {
                lbl_IPAddress.Text = ipAdd;
            }
        }

        private void GetIpAddress()
        {
            string userip = Request.UserHostAddress;
            if (Request.UserHostAddress != null)
            {
                Int64 macinfo = new Int64();
                string macSrc = macinfo.ToString("X");
                if (macSrc == "0")
                {
                    if (userip == "127.0.0.1")
                    {
                        lbl_IPAddress.Text = "LOCAL";
                    }
                    else
                    {
                        lbl_IPAddress.Text = userip;
                    }
                }
            }
        }

        // Method to get allowed regions from the database
        public HashSet<string> GetAllowedRegions()
        {
            var allowedRegions = new HashSet<string>();

            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            string query = "SELECT Work_Region_Code FROM tlb_work_state_region where JOBID_Menu='Yes'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    allowedRegions.Add(reader["Work_Region_Code"].ToString());
                }

                reader.Close();
            }

            return allowedRegions;
        }

        protected void PermissionCheck()
        {
            var allowedRegions = GetAllowedRegions();

            if (!allowedRegions.Contains(Session["REGION"].ToString()))
            //if (Session["REGION"].ToString() != "KPO" && Session["REGION"].ToString() != "AGL" && Session["REGION"].ToString() != "JSR" && Session["REGION"].ToString() != "NINL")
            {
                if (Session["USERTYPE"].ToString() == "Office Staff")
                {
                    if (Session["PERMISSION"].ToString() == "Human Resource")
                    {
                        Reltab.Visible = true;

                        DataMastering.Visible = true; Works.Visible = true; Clients.Visible = true; WorkOrder.Visible = true;
                        Payroll.Visible = true; ATSSItes.Visible = true; HRSection.Visible = true;

                        Analytics.Visible = false;
                        PayrollReports.Visible = false;
                        JOBManpower.Visible = false;
                        CSM.Visible = false;
                        JOBApproval.Visible = false;

                        Memo_Billing.Visible = true;

                        budget.Visible = true;
                        add_exphd.Visible = true;
                        add_expsbhd.Visible = true;
                        add_exp.Visible = true;
                        mng_exp.Visible = true;

                    }
                    else
                    {
                        Reltab.Visible = true;
                        DataMastering.Visible = false;
                        Works.Visible = true;
                        Clients.Visible = true;
                        WorkOrder.Visible = true;
                        Payroll.Visible = true;
                        ATSSItes.Visible = true;
                        HRSection.Visible = true;
                        Memo_Billing.Visible = true;
                        Analytics.Visible = false;
                        PayrollReports.Visible = false;
                        JOBManpower.Visible = false;
                        CSM.Visible = false;
                        JOBApproval.Visible = false;

                        budget.Visible = false;
                        add_exphd.Visible = false;
                        add_expsbhd.Visible = false;
                        add_exp.Visible = true;
                        mng_exp.Visible = true;
                    }
                }
                else
                {
                    Reltab.Visible = true;
                    DataMastering.Visible = false;
                    Works.Visible = true;
                    Clients.Visible = true;
                    WorkOrder.Visible = true;
                    Payroll.Visible = true;
                    ATSSItes.Visible = true;
                    HRSection.Visible = true;

                    Analytics.Visible = false;
                    PayrollReports.Visible = false;
                    JOBManpower.Visible = false;
                    CSM.Visible = false;
                    JOBApproval.Visible = false;

                    Memo_Billing.Visible = true;
                    budget.Visible = false;
                    add_exphd.Visible = false;
                    add_expsbhd.Visible = false;
                    add_exp.Visible = true;
                    mng_exp.Visible = true;
                }
            }
            //The below block is for Angul and KPO Employees
            else
            {
                if (Session["USERTYPE"].ToString() == "Office Staff")
                {
                    if (Session["PERMISSION"].ToString() == "Human Resource")
                    {
                        DataMastering.Visible = true;
                        Works.Visible = true;
                        Clients.Visible = true;
                        WorkOrder.Visible = true;
                        Payroll.Visible = true;
                        ATSSItes.Visible = true;
                        HRSection.Visible = true;

                        Analytics.Visible = true;
                        PayrollReports.Visible = true;
                        JOBManpower.Visible = true;
                        JOBApproval.Visible = true;
                        Memo_Billing.Visible = true;
                        CSM.Visible = true;
                        Reltab.Visible = true;
                    }
                    else if (Session["PERMISSION"].ToString() == "Billing")
                    {
                        DataMastering.Visible = true;
                            Works.Visible = false;
                            Clients.Visible = false;
                            WorkOrder.Visible = true;
                            Payroll.Visible = false;
                            ATSSItes.Visible = false;
                            HRSection.Visible = false;

                        Analytics.Visible = false;
                        PayrollReports.Visible = false;
                        JOBManpower.Visible = false;
                        JOBApproval.Visible = false;
                        Memo_Billing.Visible = true;
                        CSM.Visible = false;
                        Reltab.Visible = true;
                    }
                    else if (Session["PERMISSION"].ToString() == "Special Access")
                    {
                        DataMastering.Visible = false;
                        Works.Visible = false;
                        Clients.Visible = false;
                        WorkOrder.Visible = false;
                        Payroll.Visible = false;
                        ATSSItes.Visible = false;
                        HRSection.Visible = false;

                        Analytics.Visible = false;
                        PayrollReports.Visible = false;
                        JOBManpower.Visible = true;
                        JOBApproval.Visible = true;
                        Memo_Billing.Visible = true;
                        CSM.Visible = true;
                        Reltab.Visible = true;
                    }
                }
                //The below block is for Site Staff
                else if (Session["USERTYPE"].ToString() == "Site Staff")
                {
                    if (Session["PERMISSION"].ToString() == "Site Incharge")
                    {
                        DataMastering.Visible = false;
                        Works.Visible = false;
                        Clients.Visible = false;
                        WorkOrder.Visible = false;
                        Payroll.Visible = false;
                        ATSSItes.Visible = false;
                        HRSection.Visible = false;

                        Analytics.Visible = false;
                        PayrollReports.Visible = false;
                        JOBManpower.Visible = true;
                        JOBApproval.Visible = true;
                        Memo_Billing.Visible = true;
                        CSM.Visible = true;
                        Reltab.Visible = true;
                    }
                    else if (Session["PERMISSION"].ToString() == "Safety Officer")
                    {
                        DataMastering.Visible = false;
                        Works.Visible = false;
                        Clients.Visible = false;
                        WorkOrder.Visible = false;
                        Payroll.Visible = false;
                        ATSSItes.Visible = false;
                        HRSection.Visible = false;

                        Analytics.Visible = false;
                        PayrollReports.Visible = false;
                        JOBManpower.Visible = false;
                        JOBApproval.Visible = false;
                        Memo_Billing.Visible = true;
                        CSM.Visible = true;
                        Reltab.Visible = true;
                    }

                    else if (Session["PERMISSION"].ToString() == "Supervisor")
                    {
                        DataMastering.Visible = false;
                        Works.Visible = false;
                        Clients.Visible = false;
                        WorkOrder.Visible = false;
                        Payroll.Visible = false;
                        ATSSItes.Visible = false;
                        HRSection.Visible = false;

                        Analytics.Visible = false;
                        PayrollReports.Visible = false;
                        JOBManpower.Visible = true;
                        JOBApproval.Visible = false;
                        Memo_Billing.Visible = true;
                        CSM.Visible = true;
                        Reltab.Visible = true;
                    }

                    else if (Session["PERMISSION"].ToString() == "Safety Supervisor")
                    {
                        DataMastering.Visible = false;
                        Works.Visible = false;
                        Clients.Visible = false;
                        WorkOrder.Visible = false;
                        Payroll.Visible = false;
                        ATSSItes.Visible = false;
                        HRSection.Visible = false;

                        Analytics.Visible = false;
                        PayrollReports.Visible = false;
                        JOBManpower.Visible = true;
                        JOBApproval.Visible = false;
                        Memo_Billing.Visible = true;
                        CSM.Visible = true;
                        Reltab.Visible = true;
                    }

                    else if (Session["PERMISSION"].ToString() == "Special Access")
                    {
                        DataMastering.Visible = false;
                        Works.Visible = false;
                        Clients.Visible = false;
                        WorkOrder.Visible = false;
                        Payroll.Visible = false;
                        ATSSItes.Visible = false;
                        HRSection.Visible = false;

                        Analytics.Visible = false;
                        PayrollReports.Visible = false;
                        JOBManpower.Visible = true;
                        JOBApproval.Visible = true;
                        Memo_Billing.Visible = true;
                        CSM.Visible = true;
                        Reltab.Visible = true;
                    }

                    else if (Session["PERMISSION"].ToString() == "Worker")
                    {
                        DataMastering.Visible = false;
                        Works.Visible = false;
                        Clients.Visible = false;
                        WorkOrder.Visible = false;
                        Payroll.Visible = false;
                        ATSSItes.Visible = false;
                        HRSection.Visible = false;

                        Analytics.Visible = false;
                        PayrollReports.Visible = false;
                        JOBManpower.Visible = false;
                        JOBApproval.Visible = false;
                        Memo_Billing.Visible = true;
                        CSM.Visible = true;
                        Reltab.Visible = true;
                    }
                }

                else
                {
                    //This is for ATS management
                    DataMastering.Visible = true;
                    Works.Visible = true;
                    Clients.Visible = true;
                    WorkOrder.Visible = true;
                    Payroll.Visible = true;
                    ATSSItes.Visible = true;
                    HRSection.Visible = true;

                    Analytics.Visible = true;
                    PayrollReports.Visible = true;
                    JOBManpower.Visible = true;
                    JOBApproval.Visible = true;
                    Memo_Billing.Visible = true;
                    CSM.Visible = true;
                    Reltab.Visible = true;
                }
            }
        }

        protected void btn_lgout_Click(object sender, EventArgs e)
        {
            dbcl.WriteToFile("User :" + lbl_loginusername1.Text.ToString() + " Singout Successfully");
            //Update loginstatus and Last Login Information i.e. date
            dbcl.UPDT_EmpMuster_LogoutInfo(Session["WORKMAN"].ToString(), Session["USERID"].ToString());

            Session.Abandon();
            Response.Redirect("login.aspx");
        }
    }
}