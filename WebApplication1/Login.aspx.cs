using System;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Web.Hosting;

namespace WebApplication1.bussiness.production
{
    public partial class login : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        DataTable dt = new DataTable();
        static string User_Photo = string.Empty;
        // Default folders
        static readonly string rootFolder = HostingEnvironment.MapPath("~/erp_images/ProfilePhoto");
        //static readonly string rootFolder = @"C:\atswork.in\wwwroot\erp_images\ProfilePhoto";
        static readonly string localFolder = HostingEnvironment.MapPath("~/erp_images/ProfilePhoto");
        //static readonly string localFolder = @"D:\RnD\OH4Y_19May23\WebApplication1\WebApplication1\erp_images\ProfilePhoto";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txt_loginid.Focus();
            }
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            dbcl.WriteToFile($"Login attempt with User ID: {txt_loginid.Text}");
            try
            {
                CredentialChecker1();
            }
            catch (ThreadAbortException)
            {
                // Ignore the ThreadAbortException as it's expected after Response.Redirect
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


        private void CredentialChecker1()
        {
            if (txt_loginid.Text != "" && txt_password.Text != "")
            {
                //string id = "ATS00200";
                //string pass = "UDB17v";

                string id = txt_loginid.Text;
                string pass = txt_password.Text;

                string query = "select WorkStatus,LoginID,WorkRegion,WorkState,WorkCompany,WorkmanSL,FirstName,FullName,User_RoleType,UserRoleDB,RolePermissionDB,WorkSite,Worksite_Code,SkillCategory,SkillDesignation,PrfPicFile from tbl_Employee_Mustertable where LoginID=@LoginID and LoginPassword=@LoginPassword";
                SqlParameter[] pram = {
                                          new SqlParameter("@LoginID",id),
                                          new SqlParameter("@LoginPassword",pass),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    string WorkStatus = dt.Rows[0]["WorkStatus"].ToString();

                    if (WorkStatus == "Active")
                    {
                        string UserID = dt.Rows[0]["LoginID"].ToString();
                        string Region = dt.Rows[0]["WorkRegion"].ToString();
                        string State = dt.Rows[0]["WorkState"].ToString();
                        string Company = dt.Rows[0]["WorkCompany"].ToString();
                        string Workman = dt.Rows[0]["WorkmanSL"].ToString();
                        string User_FirstName = dt.Rows[0]["FirstName"].ToString();
                        string User_FullName = dt.Rows[0]["FullName"].ToString();
                        string User_RoleType = dt.Rows[0]["User_RoleType"].ToString();
                        string User_Type = dt.Rows[0]["UserRoleDB"].ToString();
                        string User_Permission = dt.Rows[0]["RolePermissionDB"].ToString();
                        string User_Worksite = dt.Rows[0]["WorkSite"].ToString();
                        string User_WRKSTCode = dt.Rows[0]["Worksite_Code"].ToString();
                        string User_Skill = dt.Rows[0]["SkillCategory"].ToString();
                        string User_Desg = dt.Rows[0]["SkillDesignation"].ToString();

                        User_Photo = dt.Rows[0]["PrfPicFile"].ToString();
                        //string User_PhotoPath = dt.Rows[0]["PrfPicPath"].ToString();

                        Session["USERID"] = UserID;
                        Session["Password"] = pass;
                        Session["WORKMAN"] = Workman;
                        Session["USERFNAME"] = User_FirstName;
                        Session["USERNAME"] = User_FullName;
                        Session["USERTYPE"] = User_RoleType;
                        Session["UserRoleDB"] = User_Type;
                        Session["RolePermissionDB"] = User_Permission;
                        Session["REGION"] = Region;
                        Session["STATE"] = State;
                        Session["COMPANY_CODE"] = Company;
                        Session["U_SITE"] = User_Worksite;
                        Session["U_SITECODE"] = User_WRKSTCode;
                        Session["U_DESG"] = User_Desg;
                        Session["U_SKILL"] = User_Skill;
                        if (User_Photo == null || User_Photo == "")
                        {
                            Session["User_Photo"] = "No_Image.jpg";
                        }
                        else
                        {
                            //Check for physical file
                            bool File = FlieExistence();
                            if (File == true)
                            {
                                Session["User_Photo"] = User_Photo;
                            }
                            else
                            {
                                Session["User_Photo"] = "No_Image.jpg";
                            }
                        }
                        dbcl.WriteToFile("User " + User_FullName + "[" + Workman + "]" + " Logined Successfully");
                        Response.Redirect("~/bussiness/production/homepage.aspx");
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
            else
            {
                ClientScript.RegisterStartupScript(typeof(Page), "AlertMessage", "<script>alert('Unknown user / password.');</script>");
                txt_loginid.Text = "";
            }
        }


        private void CredentialChecker()
        {
            string id = txt_loginid.Text;
            string pass = txt_password.Text;

            // Hash the password before querying the database
            //string hashedPassword = HashPassword(pass);

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
                        Response.Redirect("homepage.aspx", false); // Redirect without ending response
                        Context.ApplicationInstance.CompleteRequest(); // Complete the request to avoid ThreadAbortException
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
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
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


        private bool FlieExistence()
        {
            if (File.Exists(Path.Combine(rootFolder, User_Photo)))
            {
                return true;
            }
            else if (File.Exists(Path.Combine(localFolder, User_Photo)))
            {
                return true;
            }
            else
            {
                string title = "Notifications :";
                string body = "NO Physical File Found...!!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                return false;
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


        private void HandleException(Exception ex)
        {
            string title = "Notifications :";
            string body = ex.Message;
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
        }


    }
}