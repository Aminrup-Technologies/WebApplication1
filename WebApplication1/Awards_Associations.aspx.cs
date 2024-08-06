using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace atsweb
{
    public partial class Awards_Associations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            img_award1.ImageUrl = "assets/images/award_1.jpg";
            img_award2.ImageUrl = "assets/images/award_2.jpg";
            img_award3.ImageUrl = "assets/images/award_3.jpg";
            img_award4.ImageUrl = "assets/images/award_4.jpg";
        }
    }
}