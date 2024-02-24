using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Configuration;

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
                    Int32 createdjobs = dbcl.Find_CreatedJOBID(Session["WORKMAN"].ToString());
                    lbl_activejobcount.Text = createdjobs.ToString();

                    if (createdjobs == 1)
                    {
                        string result = dbcl.Find_JOBIDMasterCode(Session["WORKMAN"].ToString());
                        if (result != null && result.Length > 0)
                        {
                            string[] splittedval = result.Split('/');
                            string jobid = splittedval[0];
                            string mastercode = splittedval[1];
                            lbl_activejobid.Text = jobid.ToString();
                        }
                    }

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

                    //string activejobid = GetJobId(Session["WORKMAN"].ToString());
                    //lbl_activejobid.Text = activejobid.ToString();
                }
            }
        }

        public string GetJobId(string workman)
        {
            string jobId = string.Empty;
            string query = "SELECT JOBID FROM tbl_jobs WHERE Creator_Workman = @Creator AND JOBID_Status = 'Active' AND YEAR(CreatedDate) = @Year AND MONTH(CreatedDate) = @Month AND DAY(CreatedDate) = @Day ORDER BY Id DESC";

            string connectionString1 = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString1))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Creator", SqlDbType.VarChar).Value = workman;
                    command.Parameters.Add("@Year", SqlDbType.Int).Value = DateTime.Now.Year;
                    command.Parameters.Add("@Month", SqlDbType.Int).Value = DateTime.Now.Month;
                    command.Parameters.Add("@Day", SqlDbType.Int).Value = DateTime.Now.Day;

                    try
                    {
                        connection.Open();
                        var result = command.ExecuteScalar();
                        jobId = result != null ? result.ToString() : "N/A";
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that occur during the query execution
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                }
            }

            return jobId;
        }


        //public Int32 Find_CreatedJOBID(string workman)
        //{
        //    string cmdString = "";
        //    dbcl.Sqlconnection();
        //    dbcl.ConnectDb();
        //    cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and JOBID_Status='Active' and YEAR(CreatedDate)='" + DateTime.Now.Year+"' and MONTH(CreatedDate)='"+DateTime.Now.Month+"'";
        //    SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
        //    cmd.CommandType = CommandType.Text;
        //    cmd.Parameters.AddWithValue("@Creator_Workman", workman);
        //    Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
        //    dbcl.Conn.Close();
        //    return count;
        //}

        public Int32 Find_InPunchJOBID(string workman)
        {
            string cmdString = "";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and EntryExit='Entry' and [CreatedDate] >= DATEADD(DAY, -3, GETDATE())";
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
            cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and FinalUpldStatus='Yes' and JOB_Status='Permit Uploaded' and [CreatedDate] >= DATEADD(DAY, -3, GETDATE())";
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
            cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and EntryExit='Entry' and FinalUpldStatus='Yes' and JOB_Status='Permit Uploaded' and [CreatedDate] >= DATEADD(DAY, -3, GETDATE())";
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
            cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and BillingCode='MS' and MONTH(CreatedDate) = MONTH(GETDATE()) AND YEAR(CreatedDate) = YEAR(GETDATE())";
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
            cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and BillingCode='LI' and MONTH(CreatedDate) = MONTH(GETDATE()) AND YEAR(CreatedDate) = YEAR(GETDATE())";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Creator_Workman", workman);
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.Conn.Close();
            return count;
        }
    }
}