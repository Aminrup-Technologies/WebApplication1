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
    public partial class csms_mainview : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    DashBoardCheck();
                }
            }
        }

        private void DashBoardCheck()
        {
            string user_desg = Session["DESG"].ToString();

            if (user_desg == "SUPERVISOR")
            {
                Supvkpirow.Visible = true;
                SafetySupvKPIrow.Visible = false;
                StoreKPIrow.Visible = false;
                SafetyOfficerKPIrow.Visible = false;
                siteinchargekpirow.Visible = false;
                GeneralKPI.Visible = true;
            }
            else if (user_desg == "SAFETY SUPERVISOR")
            {
                Supvkpirow.Visible = false;
                SafetySupvKPIrow.Visible = true;
                StoreKPIrow.Visible = false;
                SafetyOfficerKPIrow.Visible = false;
                siteinchargekpirow.Visible = false;
                GeneralKPI.Visible = true;
            }
            else if (user_desg == "SAFETY OFFICER")
            {
                Supvkpirow.Visible = false;
                SafetySupvKPIrow.Visible = false;
                StoreKPIrow.Visible = false;
                SafetyOfficerKPIrow.Visible = true;
                siteinchargekpirow.Visible = false;
                GeneralKPI.Visible = true;
            }
            else if (user_desg == "IN-CHARGE")
            {
                Supvkpirow.Visible = false;
                SafetySupvKPIrow.Visible = false;
                StoreKPIrow.Visible = false;
                SafetyOfficerKPIrow.Visible = false;
                siteinchargekpirow.Visible = true;
                GeneralKPI.Visible = true;
            }
            else if (user_desg == "STORE KEEPER")
            {
                Supvkpirow.Visible = false;
                SafetySupvKPIrow.Visible = false;
                StoreKPIrow.Visible = true;
                SafetyOfficerKPIrow.Visible = false;
                siteinchargekpirow.Visible = false;
                GeneralKPI.Visible = true;
            }
        }
    }
}