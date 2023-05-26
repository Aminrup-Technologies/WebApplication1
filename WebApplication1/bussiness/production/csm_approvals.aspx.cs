using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class csm_approvals : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
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
                    CheckforUser();

                    //Count Binder
                    BindCounts();
                }
            }
        }
        private void CheckforUser()
        {
            if (true)
            {

            }
        }

        private void BindCounts()
        {
            Bind_TBMCount();
            Bind_SOPCount();
        }

        private void Bind_TBMCount()
        {
            string wrk = Session["WORKMAN"].ToString();
            Int32 cnt1 = CC.GetPendingTBTCount1(wrk);
            Int32 cnt2 = CC.GetPendingTBTCount2(wrk);
            if (cnt1 > 0)
            {
                lbl_tbtbx1count.Text = cnt1.ToString();
                sftysupvbox.Visible = true;
                sftyofcrbox.Visible = false;
            }
            else if (cnt2 > 0)
            {
                lbl_tbtbx2count.Text = cnt2.ToString();
                sftysupvbox.Visible = false;
                sftyofcrbox.Visible = true;
            }
            else
            {
                //sftysupvbox.Visible = false;
                //sftyofcrbox.Visible = false;
            }
        }


        private void Bind_SOPCount()
        {
            string wrk = Session["WORKMAN"].ToString();
            Int32 cnt1 = CC.GetPendingSOPCount1(wrk);
            Int32 cnt2 = CC.GetPendingSOPCount2(wrk);
            if (cnt1 > 0)
            {
                lbl_sopbx1count.Text = cnt1.ToString();
                //sftysupvbox.Visible = true;
                //sftyofcrbox.Visible = false;
            }
            else if (cnt2 > 0)
            {
                lbl_sopbx2count.Text = cnt2.ToString();
                //sftysupvbox.Visible = false;
                //sftyofcrbox.Visible = true;
            }
            else
            {
                //sftysupvbox.Visible = false;
                //sftyofcrbox.Visible = false;
            }
        }
    }
}