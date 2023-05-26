using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production.aminrup
{
    public partial class aminrup : System.Web.UI.MasterPage
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
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
                    Label lbl1 = (Label)Page.Master.FindControl("lbl_loginusername1");
                    lbl1.Text = Session["USERNAME"].ToString();

                    Label lbl2 = (Label)Page.Master.FindControl("lbl_loginusername2");
                    lbl2.Text = Session["USERFNAME"].ToString();

                    GetIpValue();
                }
            }
        }

        //--------------------//

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

        protected void btn_lgout_Click(object sender, EventArgs e)
        {
            dbcl.UPDT_EmpMuster_LogoutInfo(Session["WORKMAN"].ToString(), Session["USERID"].ToString());

            Session.Abandon();
            Response.Redirect("/login.aspx");
        }
    }
}