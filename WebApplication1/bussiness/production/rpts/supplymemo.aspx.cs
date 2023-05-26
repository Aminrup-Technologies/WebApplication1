using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace WebApplication1.bussiness.production.rpts
{
    public partial class supplymemo : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            string JOBID = Request.QueryString["JOBID"];
            //string JOBID = "JOB0051152";
            Bind_JOBIDDetails(JOBID);
            Bind_SMJIDDetails(JOBID);
            Bind_Manpower(JOBID);
            Bind_ShiftData(JOBID);
        }


        private void Bind_JOBIDDetails(string jobid)
        {
            try
            {
                dbcl.Sqlconnection();
                if (dbcl.Conn.State == ConnectionState.Closed)
                { dbcl.ConnectDb(); }

                string query = "select * from tbl_jobs where JOBID=@JOBID";
                SqlParameter[] pram = {
                                          new SqlParameter("@JOBID",jobid),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    string smjid = dt.Rows[0]["Level1_BillingCode"].ToString();
                    lbl_smjid1.Text = lbl_smjid2.Text= smjid;

                    string jobdate = dt.Rows[0]["CreatedDate"].ToString();
                    DateTime dt1 = DateTime.Parse(jobdate);
                    DayOfWeek dow = dt1.DayOfWeek; //enum
                    string str = dow.ToString(); //string
                    string abc2 = DateBinder(jobdate) + " [" + str + "]" + " [" + dt.Rows[0]["JOB_Shift"].ToString() + "]";
                    lbl_jobdatedetails.Text = abc2.ToString();

                    lbl_pono.Text = dt.Rows[0]["WorkOrderNo"].ToString();
                    lbl_jobid.Text = dt.Rows[0]["JOBID"].ToString();
                    lbl_permitno.Text = dt.Rows[0]["JOB_PermitNo"].ToString();
                    lbl_jobtitle.Text = dt.Rows[0]["JOB_Title"].ToString();

                    lbl_jobrgn.Text = dt.Rows[0]["JOB_Region"].ToString();
                    lbl_jobdept.Text = dt.Rows[0]["JOB_Dept"].ToString();
                    lbl_jobloc.Text = dt.Rows[0]["JOB_Location"].ToString();
                    lbl_jobsupvname.Text = dt.Rows[0]["Creator_Name"].ToString();
                    lbl_jobsupvwrk.Text = dt.Rows[0]["Creator_Workman"].ToString();
                    lbl_siteincharge.Text = dt.Rows[0]["JOB_InchargeName"].ToString();
                    lbl_inchargewrk.Text = dt.Rows[0]["JOB_InchargeWrk"].ToString();


                }
            }
            catch (Exception ex)
            {
                //lbl_msg.ForeColor = System.Drawing.Color.Red;
                //lbl_msg.Text = "Error 217 : " + ex.Message.ToString();
            }
            finally
            {
                if (dbcl.Conn.State == ConnectionState.Open)
                    dbcl.Conn.Close();
            }
        }

        private void Bind_SMJIDDetails(string jobid)
        {
            try
            {
                dbcl.Sqlconnection();
                if (dbcl.Conn.State == ConnectionState.Closed)
                { dbcl.ConnectDb(); }

                string query = "select * from tbl_supplymemojobsdetails where Ref_JOBID=@Ref_JOBID";
                SqlParameter[] pram = {
                                          new SqlParameter("@Ref_JOBID",jobid),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    string jobdate = dt.Rows[0]["SMJ_Createdate"].ToString();
                    DateTime dt1 = DateTime.Parse(jobdate);
                    DayOfWeek dow = dt1.DayOfWeek; //enum
                    string str = dow.ToString(); //string
                    string abc2 = DateBinder(jobdate) + " [" + str + "]";
                    lbl_smjdate.Text = abc2.ToString();

                    lbl_smjcreatorname.Text = dt.Rows[0]["CreatorName"].ToString();
                    lbl_smjcreatorwrk.Text = dt.Rows[0]["CreatorWorkmen"].ToString();
                }
            }
            catch (Exception ex)
            {
                //lbl_msg.ForeColor = System.Drawing.Color.Red;
                //lbl_msg.Text = "Error 217 : " + ex.Message.ToString();
            }
            finally
            {
                if (dbcl.Conn.State == ConnectionState.Open)
                    dbcl.Conn.Close();
            }
        }
        private string DateBinder(string date)
        {
            string newdate = "";
            DateTime oDate = Convert.ToDateTime(date);
            string day = "";
            string month = "";
            if (oDate.Day < 10)
            {
                day = "0" + oDate.Day.ToString();
            }
            else
            {
                day = oDate.Day.ToString();
            }
            if (oDate.Month < 10)
            {
                month = "0" + oDate.Month.ToString();
            }
            else
            {
                month = oDate.Month.ToString();
            }
            return newdate = day + "-" + month + "-" + oDate.Year;
        }

        private void Bind_Manpower(string jobid)
        {
            string ddljobid = lbl_jobid.Text.ToString();
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string CmdString = "select concat(b.EmployeeName,' [',b.EmployeeWrk,']') as employeename, a.PO_SkillCategory, a.PO_EmpDesignation,b.Inpunch_Time, b.Outpunch_Time, b.safetypassno, a.ShiftCalc from tbl_supplymemojobmanpower a, tbl_attendance b where a.Ref_JOBID = b.JOBID and a.RefDBId = b.id and a.Ref_JOBID ='" + jobid + "' order by a.Id";
            SqlCommand cmd = new SqlCommand(CmdString, dbcl.Conn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                ManpowerGrid.DataSource = dr;
                ManpowerGrid.DataBind();
            }
            else
            {
                DataTable dt4 = new DataTable();
                ManpowerGrid.DataSource = dt4;
                ManpowerGrid.DataBind();
            }
            dbcl.Conn.Close();
        }

        private void Bind_ShiftData(string jobid)
        {
            string ddljobid = lbl_jobid.Text.ToString();
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string CmdString = "select Total_HSShiftCount, Total_SShiftCount, Total_SSShiftCount, Total_USShiftCount, Total_ShiftCount FROM tbl_supplymemojobsdetails where Ref_JOBID ='" + jobid + "' order by Id";
            SqlCommand cmd = new SqlCommand(CmdString, dbcl.Conn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                ShiftGrid.DataSource = dr;
                ShiftGrid.DataBind();
            }
            else
            {
                DataTable dt4 = new DataTable();
                ShiftGrid.DataSource = dt4;
                ShiftGrid.DataBind();
            }
            dbcl.Conn.Close();
        }

    }
}