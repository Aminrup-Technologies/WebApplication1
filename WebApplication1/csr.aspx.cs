using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace atsweb
{
    public partial class csr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Binder();
                BindCSRActivities();
                
                
            }
        }

        private void Binder()
        {
            //SqlConnection conn = new SqlConnection(DbConnection);
            //conn.Open();
            //string SelectQuery = "SELECT * FROM CSRActivities";
            //SqlCommand SelectQueryCmd = new SqlCommand(SelectQuery, conn);
            //SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(SelectQueryCmd);
            //DataTable dt = new DataTable();
            //sqlDataAdapter.Fill(dt);
            //rptCSRActivities.DataSource = dt;
            //rptCSRActivities.DataBind();
        }

        public class CSRActivity
        {
            public int ActivityID { get; set; }
            public string ActivityTitle { get; set; }
            public string ActivityDescription { get; set; }
            public DateTime ActivityDate { get; set; }
            public string Location { get; set; }
            public string ActivityImage { get; set; }
            public string Outcomes { get; set; }
        }


        private void BindCSRActivities()
        {
            List<CSRActivity> activities = new List<CSRActivity>
            {
                new CSRActivity { ActivityID = 1, ActivityTitle = "Tree Plantation Drive", ActivityDescription = "A drive to plant trees in local community.", ActivityDate = new DateTime(2024, 8, 1), Location = "Local Park", ActivityImage = "assets/images/projects/img-6.jpg", Outcomes = "200 trees planted." },
                new CSRActivity { ActivityID = 2, ActivityTitle = "Beach Clean-Up", ActivityDescription = "Cleaning up the local beach to promote environmental awareness.", ActivityDate = new DateTime(2024, 7, 15), Location = "City Beach", ActivityImage = "assets/images/projects/img-7.jpg", Outcomes = "2 tons of waste removed." },
                new CSRActivity { ActivityID = 3, ActivityTitle = "Tree Plantation Drive", ActivityDescription = "A drive to plant trees in local community.", ActivityDate = new DateTime(2024, 8, 1), Location = "Local Park", ActivityImage = "assets/images/projects/img-6.jpg", Outcomes = "200 trees planted." },
                new CSRActivity { ActivityID = 4, ActivityTitle = "Beach Clean-Up", ActivityDescription = "Cleaning up the local beach to promote environmental awareness.", ActivityDate = new DateTime(2024, 7, 15), Location = "City Beach", ActivityImage = "assets/images/projects/img-7.jpg", Outcomes = "2 tons of waste removed." },
                new CSRActivity { ActivityID = 5, ActivityTitle = "Beach Clean-Up", ActivityDescription = "Cleaning up the local beach to promote environmental awareness.", ActivityDate = new DateTime(2024, 7, 15), Location = "City Beach", ActivityImage = "assets/images/projects/img-7.jpg", Outcomes = "2 tons of waste removed." },
                // Add more activities here
            };

            rptCSRActivities.DataSource = activities;
            rptCSRActivities.DataBind();
        }
    }
}