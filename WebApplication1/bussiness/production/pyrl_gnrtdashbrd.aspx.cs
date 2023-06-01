using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class pyrl_gnrtdashbrd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERTYPE"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
            {
                Response.Redirect("login.aspx");
            }
            if (!IsPostBack)
            {
                if (Session["REGION"].ToString() == "KPO")
                {
                    AGL_F17.Visible = false;
                    KPO_F17.Visible = false;
                    NINL_F17.Visible = false;
                    JSR_F17.Visible = false; ATS_F17.Visible = false;
                }
                else if (Session["REGION"].ToString() == "AGL")
                {
                    AGL_F17.Visible = false;
                    KPO_F17.Visible = false;
                    NINL_F17.Visible = false;
                    JSR_F17.Visible = false; ATS_F17.Visible = false;
                }
                else if (Session["REGION"].ToString() == "NINL")
                {
                    AGL_F17.Visible = false;
                    KPO_F17.Visible = false;
                    NINL_F17.Visible = false;
                    JSR_F17.Visible = false; ATS_F17.Visible = false;
                }
                else if (Session["REGION"].ToString() == "JSR")
                {
                    if (Session["WORKMAN"].ToString() == "J8")
                    {
                        AGL_F17.Visible = false;
                        KPO_F17.Visible = false;
                        NINL_F17.Visible = false;
                        JSR_F17.Visible = false;
                        ATS_F17.Visible = true;
                    }
                    else
                    {
                        AGL_F17.Visible = false;
                        KPO_F17.Visible = false;
                        NINL_F17.Visible = false;
                        JSR_F17.Visible = false; ATS_F17.Visible = false;
                    }     
                }
                else
                {
                    AGL_F17.Visible = false;
                    KPO_F17.Visible = false;
                    NINL_F17.Visible = false;
                    JSR_F17.Visible = false; ATS_F17.Visible = false;
                }
            }
        }

    }
}