using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace atsweb
{
    public partial class csr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            img_comp_csr1.ImageUrl = "~/assets/images/projects/img-5.jpg";
            img_comp_csr2.ImageUrl = "~/assets/images/projects/img-6.jpg";
            img_comp_csr3.ImageUrl = "~/assets/images/projects/img-7.jpg";
            img_comp_csr4.ImageUrl = "~/assets/images/projects/img-7.jpg";
            img_comp_csr5.ImageUrl = "~/assets/images/projects/img-6.jpg";
            img_comp_csr6.ImageUrl = "~/assets/images/projects/img-5.jpg";
            
        }
    }
}