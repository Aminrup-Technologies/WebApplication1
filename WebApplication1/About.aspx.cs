using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace atsweb
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_Header.Text = "<h2>Leading Experts in Mechanical Maintenance & Construction</h2>";
            lbl_About_Company.Text = " <p><strong>M/s Automation & Technical Services (ATS) </strong> <br/> M/s Automation & Technical Services (ATS) has been in maintenance for over 14 years.\t\r\n " +
                "We utilize some of the best engineers whose qualifications include outstanding excellence coupled with long practical experience in the field of Mechanical Maintenance, Construction and specialized Erection activities, especially in the Steel industry and power sectors.\r\n" +
                "We have managed to complete a vast array of important projects in our history from specialized Fabrication, Erection, Project & Mechanical Maintenance work.\r\n ATS has significantly strengthened its status in India. Our primary goal is to satisfy our customers, putting a strong emphasis on the qualiy of our services.\r\n" +
                "Your Reliable Partner in Building INDIA”</p>";
            Ceo.ImageUrl = "assets/images/work.jpg";
            about_pic.ImageUrl = "assets/images/work2.jpg";
            lbl_Years.Text = "14";
            img_Signature.ImageUrl = "assets/images/signature.png";
            lbl_Mission.Text = "<p>At Automation & Technical Services (ATS), our mission is to deliver timely and cost-effective project and maintenance solutions, utilizing proven technologies in the Power Generation and Steel Industries." +
                " We are dedicated to maintaining a relentless focus on safety and adhering to uncompromising standards of quality." +
                " Our established reputation among Steel and Power companies in the region reflects our commitment to excellence, ensuring we remain a leading company within the sector..</p>";
            lbl_vision.Text = "<p>Our vision is to be the preferred choice across India by 2030 for providing project and maintenance solutions and proven technologies in the fields of Power Generation and Steel Industries. " +
                "We aim to deliver timely and cost-effective solutions to our clients while maintaining a relentless focus on safety and uncompromising quality standards. " +
                "Automation & Technical Services is a well-recognized organization among Steel and Power companies in the region, enabling us to remain a leading company within the sector.\r\n</p>";
            lbl_commitment.Text = "<p>Our commitment is borne out of the experience gained since our inception. We are aware of the future demands from our clients for the highest standards of quality and a potentially more competitive environment. " +
                "We are dedicated to meeting these demands with the same level of excellence that has defined our past work. " +
                "Our commitment to quality, safety, and cost-effective solutions remains unwavering, ensuring our continued success and leadership in the industry.</p>";

        }
    }
}