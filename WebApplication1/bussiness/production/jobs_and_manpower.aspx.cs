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
    public partial class jobs_and_manpower : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["USERTYPE"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    Int32 createdjobs = Find_CreatedJOBID(Session["WORKMAN"].ToString());
                    lbl_activejobcount.Text = createdjobs.ToString();

                    Int32 inpunchjobs = Find_InPunchJOBID(Session["WORKMAN"].ToString());
                    lbl_inpunchcount.Text = inpunchjobs.ToString();

                    Int32 prmtupldjobs = Find_PermitUploadJOBID(Session["WORKMAN"].ToString());
                    lbl_prmtupldcount.Text = prmtupldjobs.ToString();

                    Int32 outpunchjobs = Find_OutPunchJOBID(Session["WORKMAN"].ToString());
                    lbl_outpndgcount.Text = outpunchjobs.ToString();

                    Int32 msjobs = Find_MSJOBID(Session["WORKMAN"].ToString());
                    lbl_splyjobscount.Text = msjobs.ToString();

                    Int32 lijobs = Find_LIJOBID(Session["WORKMAN"].ToString());
                    lbl_lijobscount.Text = lijobs.ToString();
                }
            }
        }


        public Int32 Find_CreatedJOBID(string workman)
        {
            string cmdString = "";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and JOBID_Status='Active' and YEAR(CreatedDate)='" + DateTime.Now.Year+"' and MONTH(CreatedDate)='"+DateTime.Now.Month+"'";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Creator_Workman", workman);
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.Conn.Close();
            return count;
        }

        public Int32 Find_InPunchJOBID(string workman)
        {
            string cmdString = "";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and JOBID_Status='Active' and  JOB_Status='In-Punch Done' and YEAR(CreatedDate)='" + DateTime.Now.Year + "' and MONTH(CreatedDate)='" + DateTime.Now.Month + "'";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Creator_Workman", workman);
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.Conn.Close();
            return count;
        }

        public Int32 Find_PermitUploadJOBID(string workman)
        {
            string cmdString = "";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and JOBID_Status='Active' and JOB_Status='Permit Uploaded' and YEAR(CreatedDate)='" + DateTime.Now.Year + "' and MONTH(CreatedDate)='" + DateTime.Now.Month + "'";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Creator_Workman", workman);
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.Conn.Close();
            return count;
        }

        public Int32 Find_OutPunchJOBID(string workman)
        {
            string cmdString = "";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and JOBID_Status='Active' and JOB_Status='Out-Punch Done' and YEAR(CreatedDate)='" + DateTime.Now.Year + "' and MONTH(CreatedDate)='" + DateTime.Now.Month + "'";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Creator_Workman", workman);
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.Conn.Close();
            return count;
        }


        public Int32 Find_MSJOBID(string workman)
        {
            string cmdString = "";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and BillingCode='MS' and YEAR(CreatedDate)='" + DateTime.Now.Year + "' and MONTH(CreatedDate)='" + DateTime.Now.Month + "'";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Creator_Workman", workman);
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.Conn.Close();
            return count;
        }

        public Int32 Find_LIJOBID(string workman)
        {
            string cmdString = "";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and BillingCode='LI' and YEAR(CreatedDate)='" + DateTime.Now.Year + "' and MONTH(CreatedDate)='" + DateTime.Now.Month + "'";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Creator_Workman", workman);
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.Conn.Close();
            return count;
        }
    }
}