using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication1.bussiness.production
{
    public partial class view_rejectedjobs : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    string CmdString2 = "select * from tbl_jobs where JOB_InchargeWrk='" + Session["WORKMAN"].ToString() + "' and JOB_InchargeName='" + Session["USERNAME"].ToString() + "' and Incharge_Approval='Rejected' order by CreatedDate desc";
                    BindGrid(CmdString2);
                }
            }
        }

        private void BindGrid(string cmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            dbcl.Conn.Close();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                Label lbl_JOBID_Status = (Label)GridView1.Rows[i].FindControl("lbl_JOBID_Status");
                Label lbl_JOB_InchargeName = (Label)GridView1.Rows[i].FindControl("lbl_JOB_InchargeName");
                Label lbl_Incharge_Approval = (Label)GridView1.Rows[i].FindControl("lbl_Incharge_Approval");

                Label lbl_JOB_PermitNo = (Label)GridView1.Rows[i].FindControl("lbl_JOB_PermitNo");
                Label lbl_FinalUpldStatus = (Label)GridView1.Rows[i].FindControl("lbl_FinalUpldStatus");

                Button btn_status = (Button)GridView1.Rows[i].FindControl("btn_jobidstatus");

                string jobidstatus = lbl_JOBID_Status.Text.ToString();
                string approvalstatus = lbl_Incharge_Approval.Text.ToString();

                string prmtno = lbl_JOB_PermitNo.Text.ToString();
                string lblupldstatus = lbl_FinalUpldStatus.Text.ToString();

                if (jobidstatus == "Active")
                {
                    btn_status.CssClass = "btn btn-success btn-sm";
                }
                else
                {
                    btn_status.CssClass = "btn btn-sm btn-danger";
                }

                if (approvalstatus == "Approved")
                {
                    lbl_Incharge_Approval.ForeColor = System.Drawing.Color.DarkSeaGreen;
                    lbl_JOB_InchargeName.ForeColor = System.Drawing.Color.DarkSeaGreen;
                }
                else
                {
                    lbl_Incharge_Approval.ForeColor = System.Drawing.Color.Red;
                    lbl_JOB_InchargeName.ForeColor = System.Drawing.Color.Red;
                }

                if (lblupldstatus == "Yes")
                {
                    lbl_JOB_PermitNo.ForeColor = System.Drawing.Color.DarkSeaGreen;
                }
                else
                {
                    lbl_JOB_PermitNo.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}