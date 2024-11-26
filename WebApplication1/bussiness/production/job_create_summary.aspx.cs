using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class job_create_summary : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
        public static string region = string.Empty;
        public static string viewertype = string.Empty;
        public static string billingtype = string.Empty;
        public static string query1 = string.Empty;

        public static string JobidArray = "";
        public static string PermitnoArray = "";
        StringBuilder tempJobidArray = new StringBuilder();
        StringBuilder tempPermitnoArray = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    region = Session["REGION"].ToString();
                    Check_UserType();
                    DDL_ViewType.Focus();
                    RFV_DDL_ViewType.Enabled = true;
                    RFV_DDL_BillingType.Enabled = true;
                    RFV_DDL_Workorder.Enabled = true;
                    RFV_DDL_Supervisor.Enabled = false;
                }
            }
        }


        private void Check_UserType()
        {
            List<string> optionsToDisable = new List<string>();


            if (Session["U_DESG"].ToString() == "SUPERVISOR")
            {
                DDL_ViewType.SelectedIndex = 1;
                optionsToDisable.Add("2");
                optionsToDisable.Add("3");

                foreach (ListItem item in DDL_ViewType.Items)
                {
                    if (optionsToDisable.Contains(item.Value)) // replace with your condition
                    {
                        item.Enabled = false;
                    }
                    else
                    {
                        item.Enabled = true; // Ensure other items are enabled
                    }
                }
            }
            else if (Session["U_DESG"].ToString() == "SITE-IN-CHARGE" || Session["U_DESG"].ToString() == "IN-CHARGE")
            {
                DDL_ViewType.SelectedIndex = 2;

                optionsToDisable.Add("1");
                optionsToDisable.Add("3");

                foreach (ListItem item in DDL_ViewType.Items)
                {
                    if (optionsToDisable.Contains(item.Value)) // replace with your condition
                    {
                        item.Enabled = false;
                    }
                    else
                    {
                        item.Enabled = true; // Ensure other items are enabled
                    }
                }
            }
            else
            {
                DDL_ViewType.SelectedIndex = 0;
                optionsToDisable.Add("1");
                optionsToDisable.Add("2");

                foreach (ListItem item in DDL_ViewType.Items)
                {
                    if (optionsToDisable.Contains(item.Value)) // replace with your condition
                    {
                        item.Enabled = false;
                    }
                    else
                    {
                        item.Enabled = true; // Ensure other items are enabled
                    }
                }
            }
        }

        private void Bind_Workorders(string query)
        {
            //string query = "select distinct WorkOrderNo, CONCAT(WorkOrderNo, ' [', JOB_Dept, ']') AS JOB_Dept  from tbl_jobs where JOB_Status='Level1MemoCreated' and WO_Type='ARC' and Work_Region_Code=@Work_Region_Code";
            //string query = "SELECT WorkOrderNo, CONCAT(plant_name, '[', sap_code, ']') AS plant_name FROM MST_PlantDetails";
            string textField = "JOB_Site";
            string valueField = "WorkOrderNo";
            string billingtype = DDL_BillingType.SelectedValue.ToString();

            bool recordsBound;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Work_Region_Code", region),
                //new SqlParameter("@LineId", billingtype)
            };

            // Bind the DropDownList and get the flag indicating whether records were bound
            DatabaseHelper.BindDropDownList(query, DDL_Workorder, textField, valueField, parameters, out recordsBound);

            // Check if any records were bound
            if (!recordsBound)
            {
                DatabaseHelper.BindWithDefaultNoRecords(DDL_Workorder);
                string WorkorderBinder_Error_script = @"<script type='text/javascript'>
                            new PNotify({
                                title: 'Error',
                                text: 'An error occurred!',
                                type: 'error',
                                styling: 'bootstrap3'
                            });
                        </script>";

                // RegisterStartupScript adds the JavaScript code to the page
                ClientScript.RegisterStartupScript(this.GetType(), "ShowWorkorderBinderErrorNotification", WorkorderBinder_Error_script, false);

            }
        }

        

        protected void DDL_BillingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            viewertype = DDL_ViewType.SelectedValue.ToString();
            billingtype = DDL_BillingType.SelectedValue.ToString();

            if (DDL_ViewType.SelectedIndex != 0)
            {
                if (DDL_BillingType.SelectedIndex != 0)
                {
                    if (viewertype == "1")
                    {
                        query1 = "SELECT distinct WorkOrderNo, CONCAT(WorkOrderNo, ' [', JOB_Site, ']') AS JOB_Site  from tbl_jobs where Creator_Workman='" + Session["WORKMAN"].ToString() + "' and Incharge_Approval='Approved' and BillingCode='" + billingtype + "'";
                    }
                    else if (viewertype == "2")
                    {
                        query1 = "SELECT distinct WorkOrderNo, CONCAT(WorkOrderNo, ' [', JOB_Site, ']') AS JOB_Site  from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and Incharge_Approval='Approved' and BillingCode='" + billingtype + "'";
                    }
                    else if (viewertype == "3")
                    {
                        query1 = "SELECT distinct WorkOrderNo, CONCAT(WorkOrderNo, ' [', JOB_Site, ']') AS JOB_Site  from tbl_jobs where Incharge_Approval='Approved' and BillingCode='" + billingtype + "'";
                    }

                    Bind_Workorders(query1);
                }
            }
        }

        protected void DDL_ViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DDL_BillingType_SelectedIndexChanged(DDL_BillingType, EventArgs.Empty);
        }


        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string startdate = txt_date1.Text.ToString();
            string enddate = txt_date2.Text.ToString();
            string CmdString2 = "";
            string workorderno = DDL_Workorder.SelectedValue.ToString();

            if (DDL_ViewType.SelectedIndex != 0)
            {
                if (DDL_BillingType.SelectedIndex != 0)
                {
                    if (DDL_BillingType.SelectedIndex == 1) //Manpower Supply
                    {
                        if (DDL_Workorder.SelectedIndex != 0)
                        {

                            if (txt_date1.Text != "" && txt_date1.Text != "")
                            {
                                //CmdString2 = "select Id, CreatedDate, Creator_Workman, WorkOrderNo, JOBID, IIF(JOB_Status ='Level1MemoCreated', Level1_BillingCode,JOBID_Status) as JOBID_Status, JOBID_Status, JOB_Site, JOB_InchargeName, JOB_Shift, JOB_Title, JOB_PermitNo, JOB_Status, FinalUpldStatus, Incharge_Approval from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and WorkOrderNo='" + workorderno + "' and CreatedDate between '" + startdate + "' and '" + enddate + "' and JOBID_Status='Blocked' and EntryExit='Exit' and FinalUpldStatus='Yes' and Incharge_Approval='Approved' and JOB_Status='Level1MemoCreated' and BillingCode='" + billingtype + "' order by CreatedDate desc";
                                CmdString2 = "select a.Id, a.CreatedDate, a.Creator_Workman, a.WorkOrderNo, a.JOBID,IIF(a.JOB_Status ='Level1MemoCreated',a.Level1_BillingCode,a.JOBID_Status) as JOBID_Status, a.JOBID_Status, a.JOB_Site, a.JOB_InchargeName, a.JOB_Shift, a.JOB_Title, a.JOB_PermitNo, a.JOB_Status, a.FinalUpldStatus, a.Incharge_Approval, b.Total_HSShiftCount, b.Total_SShiftCount, b.Total_SSShiftCount, b.Total_USShiftCount, b.Total_ShiftCount from tbl_jobs a, tbl_supplymemojobsdetails b where a.Level1_BillingCode=b.SMJID and  a.JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and a.WorkOrderNo='" + workorderno + "' and a.CreatedDate between '" + startdate + "' and '" + enddate + "' and a.JOBID_Status='Blocked' and a.EntryExit='Exit' and a.FinalUpldStatus='Yes' and a.Incharge_Approval='Approved' and a.JOB_Status='Level1MemoCreated' and a.BillingCode='" + billingtype + "' order by a.CreatedDate desc";
                            }
                            else
                            {
                                //CmdString2 = "select Id,CreatedDate,Creator_Workman,WorkOrderNo,JOBID,IIF(JOB_Status ='Level1MemoCreated',Level1_BillingCode,JOBID_Status) as JOBID_Status, JOBID_Status,JOB_Site,JOB_InchargeName,JOB_Shift,JOB_Title,JOB_PermitNo,JOB_Status,FinalUpldStatus,Incharge_Approval from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and WorkOrderNo='" + workorderno + "' and JOBID_Status='Blocked' and EntryExit='Exit' and FinalUpldStatus='Yes' and Incharge_Approval='Approved' and JOB_Status='Level1MemoCreated' and BillingCode='" + billingtype + "' order by CreatedDate desc";

                                CmdString2 = "select a.Id, a.CreatedDate, a.Creator_Workman, a.WorkOrderNo, a.JOBID,IIF(a.JOB_Status ='Level1MemoCreated',a.Level1_BillingCode,a.JOBID_Status) as JOBID_Status, a.JOBID_Status, a.JOB_Site, a.JOB_InchargeName, a.JOB_Shift, a.JOB_Title, a.JOB_PermitNo, a.JOB_Status, a.FinalUpldStatus, a.Incharge_Approval, b.Total_HSShiftCount, b.Total_SShiftCount, b.Total_SSShiftCount, b.Total_USShiftCount, b.Total_ShiftCount from tbl_jobs a, tbl_supplymemojobsdetails b where a.Level1_BillingCode=b.SMJID and  a.JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and a.WorkOrderNo='" + workorderno + "' and a.JOBID_Status='Blocked' and a.EntryExit='Exit' and a.FinalUpldStatus='Yes' and a.Incharge_Approval='Approved' and a.JOB_Status='Level1MemoCreated' and a.BillingCode='" + billingtype + "' order by a.CreatedDate desc";
                            }


                            BindGrid(CmdString2);
                        }
                    }
                    //Line item jobs
                    else
                    {
                        if (DDL_Workorder.SelectedIndex != 0)
                        {

                            if (txt_date1.Text != "" && txt_date1.Text != "")
                            {
                                CmdString2 = "select Id, CreatedDate, Creator_Workman, WorkOrderNo, JOBID, IIF(JOB_Status ='Level1MemoCreated', Level1_BillingCode,JOBID_Status) as JOBID_Status, JOBID_Status, JOB_Site, JOB_InchargeName, JOB_Shift, JOB_Title, JOB_PermitNo, JOB_Status, FinalUpldStatus, Incharge_Approval, 0.0 as Total_HSShiftCount, 0.0 as Total_SShiftCount, 0.0 as Total_SSShiftCount, 0.0 as Total_USShiftCount, 0.0 as Total_ShiftCount from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and WorkOrderNo='" + workorderno + "' and CreatedDate between '" + startdate + "' and '" + enddate + "' and JOBID_Status='Blocked' and EntryExit='Exit' and FinalUpldStatus='Yes' and Incharge_Approval='Approved' and BillingCode='" + billingtype + "' order by CreatedDate desc";
                            }
                            else
                            {
                                CmdString2 = "select Id,CreatedDate,Creator_Workman,WorkOrderNo,JOBID,IIF(JOB_Status ='Level1MemoCreated',Level1_BillingCode,JOBID_Status) as JOBID_Status, JOBID_Status,JOB_Site,JOB_InchargeName,JOB_Shift,JOB_Title,JOB_PermitNo,JOB_Status,FinalUpldStatus,Incharge_Approval, 0.0 as Total_HSShiftCount, 0.0 as Total_SShiftCount, 0.0 as Total_SSShiftCount, 0.0 as Total_USShiftCount, 0.0 as Total_ShiftCount from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and WorkOrderNo='" + workorderno + "' and JOBID_Status='Blocked' and EntryExit='Exit' and FinalUpldStatus='Yes' and Incharge_Approval='Approved' and BillingCode='" + billingtype + "' order by CreatedDate desc";
                            }
                            BindGrid(CmdString2);
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
            DataSet ds = new DataSet();
            ad.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            dbcl.Conn.Close();
        }

        protected void btn_summary_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow gvrow in GridView1.Rows)
            {
                CheckBox chk = (CheckBox)gvrow.FindControl("chkone");
                Label lbl_JOBID = (Label)gvrow.FindControl("lbl_JOBID");

                if (chk != null && chk.Checked)
                {
                    tempJobidArray.Append(lbl_JOBID.Text + ",");
                    JobidArray = tempJobidArray.ToString().TrimEnd(',');
                }
            }
        }
    }
}