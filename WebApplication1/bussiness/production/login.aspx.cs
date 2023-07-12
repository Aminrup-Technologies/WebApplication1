using System;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace WebApplication1.bussiness.production
{
    public partial class login : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        UserActivity UActivity = new UserActivity();
        DataTable dt = new DataTable();

        static string User_Photo = string.Empty;
        // Default folder
        static readonly string rootFolder = @"C:\atswork.in\wwwroot\erp_images\ProfilePhoto";
        static readonly string localFolder = @"D:\RnD\OH4Y_19May23\WebApplication1\WebApplication1\erp_images\ProfilePhoto";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txt_loginid.Focus();
            }
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            CredentialChecker();
            //Response.Redirect("homepage.aspx");
        }


        private void CredentialChecker()
        {
            if (txt_loginid.Text != "" && txt_password.Text != "")
            {
                //string id = "ATS00200";
                //string pass = "UDB17v";

                //string id = "ATS0084";
                //string pass = "Anupriya@2020";

                string id = txt_loginid.Text;
                string pass = txt_password.Text;

                string query = "select * from tbl_Employee_Mustertable where LoginID=@LoginID and LoginPassword=@LoginPassword";
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
                        string User_Type = dt.Rows[0]["User_RoleType"].ToString();
                        string User_Permission = dt.Rows[0]["Role_Permission"].ToString();
                        string User_Worksite = dt.Rows[0]["WorkSite"].ToString();
                        string User_WRKSTCode = dt.Rows[0]["Worksite_Code"].ToString();
                        string User_Skill = dt.Rows[0]["SkillCategory"].ToString();
                        string User_Desg = dt.Rows[0]["SkillDesignation"].ToString();

                        User_Photo = dt.Rows[0]["PrfPicFile"].ToString();
                        //string User_PhotoPath = dt.Rows[0]["PrfPicPath"].ToString();

                        Session["USERID"] = UserID;
                        Session["WORKMAN"] = Workman;
                        Session["USERFNAME"] = User_FirstName;
                        Session["USERNAME"] = User_FullName;
                        Session["USERTYPE"] = User_Type;
                        Session["PERMISSION"] = User_Permission;
                        Session["REGION"] = Region;
                        Session["STATE"] = State;
                        Session["COMPANY_CODE"] = Company;
                        Session["U_SITE"] = User_Worksite;
                        Session["U_SITECODE"] = User_WRKSTCode;
                        Session["U_DESG"] = User_Desg;
                        Session["U_SKILL"] = User_Skill;
                        if (User_Photo == null || User_Photo =="")
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
                        dbcl.WriteToFile("User " + User_FullName + " Logined Successfully");
                        Response.Redirect("homepage.aspx");
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


    }

    public class UserActivity
    {
        private string UserID { get; set; }
        private string Region { get; set; }
        private string State { get; set; }
        private string Company { get; set; }
        private string Workman { get; set; }
        private string User_FirstName { get; set; }
        private string User_FullName { get; set; }
        private string User_Type { get; set; }
        private string User_Permission { get; set; }
        private string User_Worksite { get; set; }
        private string User_WRKSTCode { get; set; }
        private string User_Skill { get; set; }
        private string User_Desg { get; set; }
        private string User_Photo { get; set; }
    }
}