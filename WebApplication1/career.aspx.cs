using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;


namespace atsweb
{
    public partial class career : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindJobOpeningsDatafromDB();

            }
        }
        
        private void BindJobOpeningsDatafromDB()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetActiveJobOpenings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        rptJobOpenings.DataSource = dt;
                        rptJobOpenings.DataBind();
                    }
                }
            }
        }


        private void BindJobOpeningsDataOld()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PositionName", typeof(string));
            dt.Columns.Add("NoOfOpenings", typeof(int));
            dt.Columns.Add("Salary", typeof(string));
            dt.Columns.Add("WorkLocation", typeof(string));
            dt.Columns.Add("JobDescription", typeof(string));
            dt.Columns.Add("MinQualification", typeof(string));
            dt.Columns.Add("MinExperience", typeof(string));
            dt.Columns.Add("ApplyLink", typeof(string));

            dt.Rows.Add("Software Developer", 5, "$60,000 - $80,000", "New York", "Develop and maintain software applications.", "Bachelor's Degree in Computer Science", "2 years");
            dt.Rows.Add("Project Manager", 2, "$80,000 - $100,000", "San Francisco", "Oversee project timelines and deliverables.", "MBA or relevant degree", "5 years");
            dt.Rows.Add("supervisor", 8, 8, "India", " responsible for managing the workflow and training new hires on how they can best serve customers and teams of employees",
                         "Bachelor''s Degree", "5 years");
            rptJobOpenings.DataSource = dt;
            rptJobOpenings.DataBind();
        }

        protected void btnApplyNow_Click(object sender, EventArgs e)
        {
            Button btnApplyNow = (Button)sender;
            int jobId = int.Parse(btnApplyNow.CommandArgument);

            // Redirect to the application page with the job ID as a query parameter
            Response.Redirect($"apply.aspx?jobId={jobId}");
        }
    }
}
