using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class csm_reports : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();

        Boolean submitter_flag = false;
        Boolean app1_flag = false;
        Boolean app2_flag = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null)
            {
                Response.Redirect("~/login.aspx");
            }
            else
            {
                CheckCoutnDisplay();
            }
        }

        private void CheckCoutnDisplay()
        {
            string wrkman = Session["WORKMAN"].ToString();
            Int32 count = 0;
            count = CC.GetSubmittedTBTCount(wrkman);
            //Check if User is Submitter
            if (count >= 1)
            {
                lbl_tbtcount.Text = count.ToString();
                submitter_flag = true;
                count = 0;
            }
            //Checl if user is Saftey Supervisor  -  Approver
            count = CC.GetApprovedTBTCount1(wrkman);
            if (count >= 1)
            {
                lbl_tbtcount.Text = count.ToString();
                app1_flag = true;
                count = 0;
            }

            //Check if user is Saftey Officer -  Approver
            count = CC.GetApprovedTBTCount2(wrkman);
            if (count >= 1)
            {
                lbl_tbtcount.Text = count.ToString();
                app2_flag = true;
                count = 0;
            }

            //If not any then Display ALL / Nothing
        }


        private void navigator()
        {
            if (submitter_flag == true)
            {
                Response.Redirect("vw_csm_toolboxtalk.aspx?vw=rpt1");
            }
            else if (app1_flag ==true)
            {
                Response.Redirect("vw_csm_toolboxtalk.aspx?vw=rpt2");
            }
            else if (app2_flag ==true)
            {
                Response.Redirect("vw_csm_toolboxtalk.aspx?vw=rpt3");
            }
            else
            {
                Response.Redirect("vw_csm_toolboxtalk.aspx?vw=rpt0");
            }
        }

        protected void LB_tbt_Click(object sender, EventArgs e)
        {
            navigator();
        }
    }
}