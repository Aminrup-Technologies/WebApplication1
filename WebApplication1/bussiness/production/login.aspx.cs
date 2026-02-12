using System;
using System.Web.UI;

namespace WebApplication1.bussiness.production
{
    // CHANGE 'class login' TO 'class login_redirect'
    public partial class login_redirect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("https://atswork.co.in/", true);
        }
    }
}