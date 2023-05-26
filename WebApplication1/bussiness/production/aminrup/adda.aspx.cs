using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production.aminrup
{
    public partial class adda : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label1.Text = User.Identity.Name;
                Label2.Text = User.Identity.AuthenticationType;
                Label3.Text = User.Identity.IsAuthenticated.ToString();
                Label4.Text = User.IsInRole("Administrators").ToString();
            }
        }
    }
}