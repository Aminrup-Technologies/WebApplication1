using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class GrievanceReq_View : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    BindGrievanceTickets();
                }
            }
        }

        //private void BindGrievanceTickets()
        //{
        //    string query = @"
        //        SELECT ticket_id, CreatedOn, CreatedByName, CreatorRegion, 
        //               root1_value, root2_value, root3_value, 
        //               priority_level, status 
        //        FROM tbl_helpdesktickets 
        //        WHERE status != 'Withdrawn'
        //        ORDER BY CreatedOn DESC";

        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(query, con))
        //        {
        //            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        //            {
        //                DataTable dt = new DataTable();
        //                da.Fill(dt);

        //                gvGrievances.DataSource = dt;
        //                gvGrievances.DataBind();
        //            }
        //        }
        //    }
        //}

        private void BindGrievanceTickets()
        {
            string region = Session["REGION"].ToString();
            string query = @"
                SELECT ticket_id, CreatedOn, CreatedByName, CreatorRegion, 
                       root1_value, root2_value, root3_value, 
                       priority_level, status 
                FROM tbl_helpdesktickets 
                WHERE status != 'Withdrawn' AND CreatorRegion = @Region
                ORDER BY CreatedOn DESC";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Region", region);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        gvGrievances.DataSource = dt;
                        gvGrievances.DataBind();
                    }
                }
            }
        }


        protected void gvGrievances_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewTicket")
            {
                string ticketId = e.CommandArgument.ToString();
                Response.Redirect("helpdesk_ticketdetails.aspx?ticket_id=" + ticketId);
            }
        }
    }
}
