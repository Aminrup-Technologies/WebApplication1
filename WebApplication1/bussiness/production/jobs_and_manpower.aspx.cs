using System;
using System.Data.SqlClient;

namespace WebApplication1.bussiness.production
{
    public partial class jobs_and_manpower : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("~/login.aspx");
                    return;
                }

                LoadDashboardStats();
            }
        }

        private void LoadDashboardStats()
        {
            string workman = Session["WORKMAN"].ToString();

            // ONE SINGLE QUERY to get all 6 metrics instantly!
            // Notice how we use the new MasterStatusCode logic we built in V2.
            string query = @"
                SELECT 
                    SUM(CASE WHEN JOBID_Status = 'Active' AND CreatedDate >= DATEADD(DAY, -3, GETDATE()) THEN 1 ELSE 0 END) as ActiveJobs,
                    SUM(CASE WHEN MasterStatusCode = '1' AND JOBID_Status = 'Active' AND CreatedDate >= DATEADD(DAY, -3, GETDATE()) THEN 1 ELSE 0 END) as PendingPermits,
                    SUM(CASE WHEN MasterStatusCode = '3' AND EntryExit = 'Created' AND JOBID_Status = 'Active' AND CreatedDate >= DATEADD(DAY, -3, GETDATE()) THEN 1 ELSE 0 END) as PendingInPunch,
                    SUM(CASE WHEN MasterStatusCode = '3' AND EntryExit = 'Entry' AND JOBID_Status = 'Active' AND CreatedDate >= DATEADD(DAY, -3, GETDATE()) THEN 1 ELSE 0 END) as PendingOutPunch,
                    SUM(CASE WHEN BillingCode = 'MS' AND MONTH(CreatedDate) = MONTH(GETDATE()) AND YEAR(CreatedDate) = YEAR(GETDATE()) THEN 1 ELSE 0 END) as SupplyJobs,
                    SUM(CASE WHEN BillingCode = 'LI' AND MONTH(CreatedDate) = MONTH(GETDATE()) AND YEAR(CreatedDate) = YEAR(GETDATE()) THEN 1 ELSE 0 END) as LineItemJobs
                FROM tbl_jobs 
                WHERE Creator_Workman = @Workman";

            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
            {
                cmd.Parameters.AddWithValue("@Workman", workman);
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        // Parse values safely
                        int active = rdr["ActiveJobs"] != DBNull.Value ? Convert.ToInt32(rdr["ActiveJobs"]) : 0;
                        int permits = rdr["PendingPermits"] != DBNull.Value ? Convert.ToInt32(rdr["PendingPermits"]) : 0;
                        int inpunches = rdr["PendingInPunch"] != DBNull.Value ? Convert.ToInt32(rdr["PendingInPunch"]) : 0;
                        int outpunches = rdr["PendingOutPunch"] != DBNull.Value ? Convert.ToInt32(rdr["PendingOutPunch"]) : 0;
                        int supply = rdr["SupplyJobs"] != DBNull.Value ? Convert.ToInt32(rdr["SupplyJobs"]) : 0;
                        int li = rdr["LineItemJobs"] != DBNull.Value ? Convert.ToInt32(rdr["LineItemJobs"]) : 0;

                        // Assign values to badges
                        badge_activejobs.InnerText = active.ToString();
                        badge_permits.InnerText = permits.ToString();
                        badge_inpunches.InnerText = inpunches.ToString();
                        badge_outpunches.InnerText = outpunches.ToString();
                        badge_splyjobs.InnerText = supply.ToString();
                        badge_lijobs.InnerText = li.ToString();

                        // Smart Colors: Turn red/orange if action is required, stay green if clear
                        badge_activejobs.Attributes["class"] = active > 0 ? "badge bg-blue" : "badge bg-green";
                        badge_permits.Attributes["class"] = permits > 0 ? "badge bg-red" : "badge bg-green";
                        badge_inpunches.Attributes["class"] = inpunches > 0 ? "badge bg-orange" : "badge bg-green";
                        badge_outpunches.Attributes["class"] = outpunches > 0 ? "badge bg-blue" : "badge bg-green";
                    }
                }
            }
            dbcl.DisconnectDb();
        }
    }
}