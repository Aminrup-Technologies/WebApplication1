using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.bussiness.production;

namespace WebApplication1.gentelella_master.production
{
    public partial class webmaster : System.Web.UI.MasterPage
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        protected int IdleTimeoutMinutes { get; set; } = 10; // Default fallback
        protected string AutoLogoutUrl { get; set; } = "https://atswork.in/"; // Default fallback

        protected void Page_Load(object sender, EventArgs e)
        {
            BindImpersonationChrome();

            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    // 1. Load Configurations from web.config
                    LoadConfigurations();

                    // 2. Set User Interface Elements
                    Label lbl1 = (Label)Page.Master.FindControl("lbl_loginusername2");
                    lbl1.Text = Session["USERNAME"].ToString();

                    Label lbl2 = (Label)Page.Master.FindControl("lbl_loginusername1");
                    lbl2.Text = Session["USERFNAME"].ToString();

                    ProfilePic_1.Src = "../../erp_images/ProfilePhoto/" + Session["User_Photo"].ToString();
                    ProfilePic_2.Src = "../../erp_images/ProfilePhoto/" + Session["User_Photo"].ToString();

                    GetIpValue();

                    // 3. Load and Apply Permissions
                    DataTable permissions = LoadPermissions(Session["RolePermissionDB"].ToString());
                    ApplyPermissions(permissions);
                }
            }
        }

        private void BindImpersonationChrome()
        {
            bool impersonating = ImpersonationAudit.IsImpersonating(Session);
            bool canSwitch = ImpersonationAudit.CanImpersonate(Session);

            lnk_switchUser.Visible = impersonating || canSwitch;
            lnk_switchUser.InnerText = impersonating ? "Return to my account" : "Switch User";

            pnl_impersonationBanner.Visible = impersonating;
            if (!impersonating) return;

            lbl_impersonatedUser.Text = Session[SessionKeys.UserName] != null ? Session[SessionKeys.UserName].ToString() : "";
            lbl_impersonatedWorkman.Text = Session[SessionKeys.WorkmanSL] != null ? Session[SessionKeys.WorkmanSL].ToString() : "";
            lbl_originalAdmin.Text = Session[SessionKeys.OriginalUserName] != null ? Session[SessionKeys.OriginalUserName].ToString() : "";
        }

        private void LoadConfigurations()
        {
            string configTimeout = ConfigurationManager.AppSettings["AutoLogoutTimeoutMinutes"];
            int timeout;
            if (!string.IsNullOrEmpty(configTimeout) && int.TryParse(configTimeout, out timeout))
            {
                IdleTimeoutMinutes = timeout;
            }

            string configUrl = ConfigurationManager.AppSettings["AutoLogoutRedirectUrl"];
            if (!string.IsNullOrEmpty(configUrl))
            {
                AutoLogoutUrl = configUrl;
            }
        }

        private void GetIpValue()
        {
            string ipAdd = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ipAdd))
            {
                ipAdd = Request.ServerVariables["REMOTE_ADDR"];
            }
            lbl_IPAddress.Text = HttpUtility.HtmlEncode(ipAdd);
        }

        private DataTable LoadPermissions(string roleId)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT ParentKey, ChildKey, IsVisible FROM tlb_EmployeePermissions WHERE Emp_PermissionValue = @Emp_PermissionValue", dbcl.Conn);
            sqlDa.SelectCommand.Parameters.AddWithValue("@Emp_PermissionValue", roleId);
            sqlDa.SelectCommand.CommandType = CommandType.Text;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            dbcl.Conn.Close();

            return dtbl;
        }

        private void ApplyPermissions(DataTable permissions)
        {
            Dictionary<string, bool> parentVisibility = new Dictionary<string, bool>();

            foreach (DataRow row in permissions.Rows)
            {
                string parentKey = row["ParentKey"].ToString();
                bool isChildVisible = Convert.ToBoolean(row["IsVisible"]);
                string childKey = row["ChildKey"].ToString();

                if (isChildVisible)
                {
                    if (!parentVisibility.ContainsKey(parentKey))
                    {
                        parentVisibility[parentKey] = true;
                    }
                }

                if (parentVisibility.ContainsKey(parentKey))
                {
                    switch (parentKey)
                    {
                        case "HomePage": HomePage.Visible = parentVisibility[parentKey]; break;
                        case "DataMastering": DataMastering.Visible = parentVisibility[parentKey]; break;
                        case "JOBManpower": JOBManpower.Visible = parentVisibility[parentKey]; break;
                        case "Memo_Billing": Memo_Billing.Visible = parentVisibility[parentKey]; break;
                        case "CSM": CSM.Visible = parentVisibility[parentKey]; break;
                        case "loans_adeductions": loans_adeductions.Visible = parentVisibility[parentKey]; break;
                        case "leaves_attendance": leaves_attendance.Visible = parentVisibility[parentKey]; break;
                        case "Payroll": Payroll.Visible = parentVisibility[parentKey]; break;
                        case "Expenses": Expenses.Visible = parentVisibility[parentKey]; break;
                        case "Helpdesk": Helpdesk.Visible = parentVisibility[parentKey]; break;
                        case "Analytics": Analytics.Visible = parentVisibility[parentKey]; break;
                    }
                }

                if (!string.IsNullOrEmpty(childKey))
                {
                    Control parentControl = FindControlRecursive(this, parentKey);
                    if (parentControl != null)
                    {
                        Control childControl = parentControl.FindControl(childKey);
                        if (childControl != null)
                        {
                            try
                            {
                                childControl.Visible = isChildVisible;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to set visibility on child control '{childKey}' under parent '{parentKey}'.", ex);
                            }
                        }
                    }
                }
            }
        }

        private Control FindControlRecursive(Control root, string id)
        {
            if (root == null || string.IsNullOrEmpty(id))
                return null;

            if (string.Equals(root.ID, id, StringComparison.OrdinalIgnoreCase))
                return root;

            foreach (Control child in root.Controls)
            {
                Control found = FindControlRecursive(child, id);
                if (found != null)
                    return found;
            }

            return null;
        }

        protected void btn_lgout_Click(object sender, EventArgs e)
        {
            // 1. Write to log
            dbcl.WriteToFile("User :" + lbl_loginusername1.Text + " Signout Successfully");

            // 2. Execute DB updates BEFORE destroying the session using strongly-typed keys!
            if (Session[SessionKeys.WorkmanSL] != null && Session[SessionKeys.UserID] != null)
            {
                dbcl.UPDT_EmpMuster_LogoutInfo(Session[SessionKeys.WorkmanSL].ToString(), Session[SessionKeys.UserID].ToString());
            }

            // 3. Finalize logout (This will clear sessions and redirect)
            LogoutUser();
        }

        protected void LogoutUser()
        {
            try
            {
                // Update User Audit Table
                dbcl.SPreturn_dt(
                    @"UPDATE tbl_UserLoginAudit
                      SET LogoutTime = GETDATE()
                      WHERE SessionID=@SID AND LogoutTime IS NULL",
                    new SqlParameter[]
                    {
                        new SqlParameter("@SID", Session.SessionID)
                    });

                // Update Muster Table Logout Status using strongly-typed key!
                if (Session[SessionKeys.UserID] != null)
                {
                    dbcl.SPreturn_dt(
                        "UPDATE tbl_Employee_Mustertable SET LastLogout=GETDATE(), LoginStatus=0 WHERE LoginID=@ID",
                        new SqlParameter[]
                        {
                            new SqlParameter("@ID", Session[SessionKeys.UserID].ToString())
                        });
                }
            }
            catch (Exception ex)
            {
                // If a DB error occurs during logout, we still want to ensure the user is logged out below
                dbcl.WriteToFile("Logout DB Error: " + ex.Message);
            }
            finally
            {
                // 4. Destroy ALL session keys instantly (No need to clear them 1-by-1)
                Session.Clear();
                Session.Abandon();
                Response.Redirect("~/login.aspx", false);
            }
        }
    }
}