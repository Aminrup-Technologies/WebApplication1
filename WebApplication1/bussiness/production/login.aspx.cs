using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebApplication1.bussiness.production
{
    public partial class login : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        DataTable dt = new DataTable();
        static string User_Photo = string.Empty;
        static string rootFolder = string.Empty;
        static string localFolder = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txt_loginid.Focus();
                rootFolder = Server.MapPath("~/erp_images/ProfilePhoto");
                localFolder = Server.MapPath("~/erp_images/ProfilePhoto");
            }
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            try
            {
                CredentialChecker();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void CredentialChecker()
        {
            string id = txt_loginid.Text;
            string pass = txt_password.Text;

            string query = "SELECT WorkStatus, LoginID, WorkRegion, WorkState, WorkCompany, WorkmanSL, FirstName, FullName, User_RoleType, Role_Permission, WorkSite, Worksite_Code, SkillCategory, SkillDesignation, PrfPicFile FROM tbl_Employee_Mustertable WHERE LoginID=@LoginID AND LoginPassword=@LoginPassword";
            SqlParameter[] pram = {
                new SqlParameter("@LoginID", id),
                new SqlParameter("@LoginPassword", pass),
            };

            try
            {
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    string workStatus = dt.Rows[0]["WorkStatus"].ToString();

                    if (workStatus == "Active")
                    {
                        SetSessionVariables(dt);
                        Response.Redirect("homepage.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(typeof(Page), "AlertMessage", "<script>alert('User ID is InActive');</script>");
                        txt_loginid.Text = "";
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "AlertMessage", "<script>alert('Unknown user / password.');</script>");
                    txt_loginid.Text = "";
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void SetSessionVariables(DataTable dt)
        {
            DataRow row = dt.Rows[0];
            Session["USERID"] = row["LoginID"].ToString();
            Session["WORKMAN"] = row["WorkmanSL"].ToString();
            Session["USERFNAME"] = row["FirstName"].ToString();
            Session["USERNAME"] = row["FullName"].ToString();
            Session["USERTYPE"] = row["User_RoleType"].ToString();
            Session["PERMISSION"] = row["Role_Permission"].ToString();
            Session["REGION"] = row["WorkRegion"].ToString();
            Session["STATE"] = row["WorkState"].ToString();
            Session["COMPANY_CODE"] = row["WorkCompany"].ToString();
            Session["U_SITE"] = row["WorkSite"].ToString();
            Session["U_SITECODE"] = row["Worksite_Code"].ToString();
            Session["U_DESG"] = row["SkillDesignation"].ToString();
            Session["U_SKILL"] = row["SkillCategory"].ToString();
            Session["User_Photo"] = GetPhotoPath(row["PrfPicFile"].ToString());
            dbcl.WriteToFile("User " + Session["USERNAME"] + "[" + Session["WORKMAN"] + "]" + " Logined Successfully");
        }

        private string GetPhotoPath(string fileName)
        {
            if (File.Exists(Path.Combine(rootFolder, fileName)))
            {
                return Path.Combine(rootFolder, fileName);
            }
            else if (File.Exists(Path.Combine(localFolder, fileName)))
            {
                return Path.Combine(localFolder, fileName);
            }
            else
            {
                return "No_Image.jpg";
            }
        }

        private void HandleException(Exception ex)
        {
            string title = "Notifications :";
            string body = ex.Message;
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
        }
    }
}
