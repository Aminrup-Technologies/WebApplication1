using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace atsweb
{
    public partial class home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            img_comp_greetinghdr1.ImageUrl = "~/assets/images/slider/slide-2.jpg";
            lbl_comp_greetinghdr1.Text = "Welcome to Automation & Technical Service";

            lbl_comp_greetinghdr2.Text = "Welcome to Automation & Technical Service";
        }
    }
}