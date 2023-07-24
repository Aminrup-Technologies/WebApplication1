using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class jobstatus_flow : System.Web.UI.UserControl
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
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
                    Int32 createdjobs = dbcl.Find_CreatedJOBID(Session["WORKMAN"].ToString());
                    if (createdjobs > 0)
                    {
                        string result = dbcl.Find_JOBIDMasterCode(Session["WORKMAN"].ToString());
                        if (result != null && result.Length > 0)
                        {
                            string[] splittedval = result.Split('/');
                            string jobid  = splittedval[0];
                            string mastercode = splittedval[1];

                            if (mastercode =="1")
                            {
                                step1.Attributes["class"] = "step_no bg-green";
                                step2.Attributes["class"] = "step_no bg-orange";
                                step3.Attributes["class"] = "step_no bg-orange";
                                step4.Attributes["class"] = "step_no bg-orange";
                                step5.Attributes["class"] = "step_no bg-orange";
                            }
                            else if (mastercode =="2") 
                            {
                                step1.Attributes["class"] = "step_no bg-green";
                                step2.Attributes["class"] = "step_no bg-green";
                                step3.Attributes["class"] = "step_no bg-orange";
                                step4.Attributes["class"] = "step_no bg-orange";
                                step5.Attributes["class"] = "step_no bg-orange";
                            }
                            else if (mastercode == "3") 
                            {
                                step1.Attributes["class"] = "step_no bg-green";
                                step2.Attributes["class"] = "step_no bg-green";
                                step3.Attributes["class"] = "step_no bg-green";
                                step4.Attributes["class"] = "step_no bg-orange";
                                step5.Attributes["class"] = "step_no bg-orange";
                            }
                            else if (mastercode == "4") 
                            {
                                step1.Attributes["class"] = "step_no bg-green";
                                step2.Attributes["class"] = "step_no bg-green";
                                step3.Attributes["class"] = "step_no bg-green";
                                step4.Attributes["class"] = "step_no bg-green";
                                step5.Attributes["class"] = "step_no bg-orange";
                            }
                            else if (mastercode == "5") 
                            {
                                step1.Attributes["class"] = "step_no bg-green";
                                step2.Attributes["class"] = "step_no bg-green";
                                step3.Attributes["class"] = "step_no bg-green";
                                step4.Attributes["class"] = "step_no bg-green";
                                step5.Attributes["class"] = "step_no bg-green";
                            }
                            else
                            {
                                step1.Attributes["class"] = "step_no bg-orange";
                                step2.Attributes["class"] = "step_no bg-orange";
                                step3.Attributes["class"] = "step_no bg-orange";
                                step4.Attributes["class"] = "step_no bg-orange";
                                step5.Attributes["class"] = "step_no bg-orange";
                            }
                        }
                    }
                }
            }
        }
    }
}