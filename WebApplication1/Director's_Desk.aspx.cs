using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace atsweb
{
    public partial class Director_s_Desk : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            img_director.ImageUrl = "assets/images/director.jpg";
            lbl_business_insights.Text= "Mr.Doglas Sosanko";
            lbl_Market_reacherch.Text = "Dear Valued Clients and Partners,\r\n\r\n" +
                "<br/>Welcome to Automation & Technical Services (ATS). For 14 years, we've excelled in the Power Generation and Steel Industries, delivering top-quality, safe, and cost-effective solutions. " +
                "<br/>Thank you for your trust and support.\r\n\r\n";
            img_Team_member1.ImageUrl = "assets/images/team/img-1.jpg";
            img_Team_member2.ImageUrl = "assets/images/team/img-2.jpg";
            img_Team_member3.ImageUrl = "assets/images/team/img-3.jpg";
            img_Team_member4.ImageUrl = "assets/images/team/img-4.jpg";
            lbl_Team_Member1.Text = "Doglas Sosanko";
            lbl_Team_Member2.Text = "Sosnako Kasi";
            lbl_Team_Member3.Text = "Takler Borovi";
            lbl_Team_Member4.Text = "Doglas Sosanko";
        }
    }
}