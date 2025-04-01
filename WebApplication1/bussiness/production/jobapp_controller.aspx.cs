using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class jobapp_controller : System.Web.UI.Page
    {
        string connString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("~/login.aspx", false);
                }
                else
                {
                    DateTime now = DateTime.Now;

                    lbl_year.Text = now.Year.ToString();
                    lbl_monthcode.Text = now.Month.ToString();
                    lbl_month.Text = now.ToString("MMMM");

                    int viewLevel = int.Parse(ddlViewLevel.SelectedValue);
                    int year = now.Year;
                    int month = now.Month;

                    BindGridNew(1, year, month, null, null, null);

                    //BindGrid(1, null, null, null); // Default view (Region Level)
                }
            }
        }

        private void BindGrid(int viewLevel, string region, string inchargeWrk, string inchargeName)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetPendingApprovals", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ViewLevel", viewLevel);
                    cmd.Parameters.AddWithValue("@Region", (object)region ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@InchargeWrk", (object)inchargeWrk ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@InchargeName", (object)inchargeName ?? DBNull.Value);

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvPendingApprovals.DataSource = dt;
                        gvPendingApprovals.DataBind();
                    }
                }
            }
        }

        protected void gvPendingApprovals_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPendingApprovals.PageIndex = e.NewPageIndex;
            //BindGrid(1, null, null, null); // Re-bind grid with default parameters

            Int32 year = Convert.ToInt32(lbl_year.Text.ToString());
            Int32 month = Convert.ToInt32(lbl_monthcode.Text.ToString());
            int viewLevel = int.Parse(ddlViewLevel.SelectedValue);
            BindGridNew(viewLevel, year, month, null, null, null);
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            
        }

        protected void ddlViewLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int viewLevel = int.Parse(ddlViewLevel.SelectedValue);
            //BindGrid(viewLevel, null, null, null);

            Int32 year = Convert.ToInt32(lbl_year.Text.ToString());
            Int32 month = Convert.ToInt32(lbl_monthcode.Text.ToString());
            int viewLevel = int.Parse(ddlViewLevel.SelectedValue);
            BindGridNew(viewLevel, year, month, null, null, null);
        }


        protected void btn_prevmonth_Click(object sender, EventArgs e)
        {
            Int32 year = Convert.ToInt32(lbl_year.Text.ToString());
            Int32 month = Convert.ToInt32(lbl_monthcode.Text.ToString());

            if (month == 01 || month == 1)
            {
                month = 12;
                year = year - 1;
            }
            else
            {
                month = month - 1;
            }

            string Year = Convert.ToString(year);
            string Month = "";
            if (month <= 9)
            {
                Month = "0" + month.ToString();
            }
            else
            {
                Month = month.ToString();
            }

            string Monthname = "";
            dbcl.FindMonthName(Month, ref Monthname);
            lbl_month.Text = Monthname;
            lbl_year.Text = Year;
            lbl_monthcode.Text = Month;

            int viewLevel = int.Parse(ddlViewLevel.SelectedValue);
            BindGridNew(viewLevel, year, month, null, null, null);
        }
        protected void btn_currentdata_Click(object sender, EventArgs e)
        {

            int year = DateTime.Now.Year;  // Keep as integer
            int month = DateTime.Now.Month;  // Keep as integer
            int viewLevel = int.Parse(ddlViewLevel.SelectedValue);

            string Year = Convert.ToString(year);
            string Month = Convert.ToString(month);

            string Monthname = "";
            dbcl.FindMonthName(Month, ref Monthname);
            lbl_month.Text = Monthname;
            lbl_year.Text = Year;
            lbl_monthcode.Text = Month;

            BindGridNew(viewLevel, year, month, null, null, null);
        }
        protected void btn_nextmonth_Click(object sender, EventArgs e)
        {
            Int32 year = Convert.ToInt32(lbl_year.Text.ToString());
            Int32 month = Convert.ToInt32(lbl_monthcode.Text.ToString());

            if (month == 12)
            {
                month = 1;
                year = year + 1;
            }
            else
            {
                month = month + 1;
            }

            string Year = Convert.ToString(year);
            string Month = "";
            if (month <= 9)
            {
                Month = "0" + month.ToString();
            }
            else
            {
                Month = month.ToString();
            }

            string Monthname = "";
            dbcl.FindMonthName(Month, ref Monthname);
            lbl_month.Text = Monthname;
            lbl_year.Text = Year;
            lbl_monthcode.Text = Month;

            int viewLevel = int.Parse(ddlViewLevel.SelectedValue);
            BindGridNew(viewLevel, year, month, null, null, null);
        }

        private void BindGridNew(int viewLevel, int year, int month, string region, string inchargeWrk, string inchargeName)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetPendingApprovalsDynamic", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ViewLevel", viewLevel);
                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.Parameters.AddWithValue("@Month", month);
                    cmd.Parameters.AddWithValue("@Region", (object)region ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@InchargeWrk", (object)inchargeWrk ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@InchargeName", (object)inchargeName ?? DBNull.Value);

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvPendingApprovals.DataSource = dt;
                        gvPendingApprovals.DataBind();
                    }
                }
            }
        }
    }
}