using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace atsweb
{
    public partial class clients : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            img1.ImageUrl = "~/assets/images/clients/tatasteel-image.png";
            img2.ImageUrl = "~/assets/images/clients/Bhushan-Steel.jpg";
            img3.ImageUrl = "~/assets/images/clients/Jindal_logo_and_steel_power.jpg";
            img4.ImageUrl = "~/assets/images/clients/voltas.png";
            img5.ImageUrl = "~/assets/images/clients/nicco.jpeg";
            img6.ImageUrl = "~/assets/images/clients/LT_Construction.jpg";
            img7.ImageUrl = "~/assets/images/clients/artson.jpg";
        }
    }
}