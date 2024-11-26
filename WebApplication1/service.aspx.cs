using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace atsweb
{
    public partial class service : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            img1.ImageUrl = "~/assets/images/services/Picture1.jpg";
            img2.ImageUrl = "~/assets/images/services/Picture2.jpg";
            img3.ImageUrl = "~/assets/images/services/Picture3.jpg";
            img4.ImageUrl = "~/assets/images/services/Picture4.jpg";
            img5.ImageUrl = "~/assets/images/services/Picture5.png";
            img6.ImageUrl = "~/assets/images/services/Picture6.jpg";

        }

       
    }
}