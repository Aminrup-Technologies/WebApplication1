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
    public partial class Awards_Associations : System.Web.UI.Page
    {
        string DbConnection = ConfigurationManager.ConnectionStrings["atsDBConnectionString"].ConnectionString;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();
            string SelectQuery = "SELECT * FROM Projects";
            SqlCommand SelectQueryCmd = new SqlCommand(SelectQuery, conn);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(SelectQueryCmd);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);
            rptProjects.DataSource = dt;
            rptProjects.DataBind();
            BindProjects();
        }

        public class Project
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string ImagePath { get; set; }
            public int Order { get; set; }
        }



        private void BindProjects()
        {
            List<Project> projects = new List<Project>
            {
                new Project { Id = 1, Title = "Amit Bhakat", Description = "Cut out of an illustrated magazine and housed in a nice, gilded frame. It showed a lady fitted out.", ImagePath = "assets/images/award_1.jpg", Order = 1 },
                new Project { Id = 2, Title = "Protima Bhakat", Description = "Cut out of an illustrated magazine and housed in a nice, gilded frame. It showed a lady fitted out.", ImagePath = "assets/images/award_2.jpg", Order = 2 },
                new Project { Id = 3, Title = "Rahul Mahato", Description = "Cut out of an illustrated magazine and housed in a nice, gilded frame. It showed a lady fitted out.", ImagePath = "assets/images/award_3.jpg", Order = 3 },
                new Project { Id = 4, Title = "Prakash Bhakat", Description = "Cut out of an illustrated magazine and housed in a nice, gilded frame. It showed a lady fitted out.", ImagePath = "assets/images/award_4.jpg", Order = 4 }
            };

            rptProjects.DataSource = projects;
            rptProjects.DataBind();
        }
    }
}