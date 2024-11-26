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
        public static string jobid = string.Empty;
        public static string dbid = string.Empty;
        public static string supv = string.Empty;

        public static string wo_number = string.Empty;
        public static string viewid = string.Empty;
        public static string yr = string.Empty;
        public static string mnt = string.Empty;

        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        DataTable dt = new DataTable();
        private static int MemoType = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            jobid = Request.QueryString["JOBID"];
            viewid = Request.QueryString["viewid"];
            yr = Request.QueryString["y"];
            mnt = Request.QueryString["m"];
            dbid = Request.QueryString["dbid"];
            supv = Request.QueryString["supv"];

            //string JOBID = "JOB0051152";
            Bind_JOBIDDetails(jobid);
            Bind_SMJIDDetails(jobid);
            Bind_Manpower(jobid);
            Bind_ShiftData(jobid);
            Bind_LineItemData(jobid);
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
                    lbl_smjid1.Text = lbl_smjid2.Text = smjid;

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

                    MemoType = Convert.ToInt32(dt.Rows[0]["MemoType"].ToString());

                    if (MemoType == 0)
                    {
                        Unified_LIGrid_0.Visible = false;
                        Unified_LIGrid_2.Visible = false;
                    }
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


        private void Bind_LineItemData(string jobid)
        {
            string ddljobid = lbl_jobid.Text.ToString();
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string CmdString = "select ServiceNumber, Service_Description, Order_Quantity, PerUnit_Value, Shift_Skill FROM tbl_SupMem_LineItems_Data where JOBID ='" + jobid + "' order by Id";
            SqlCommand cmd = new SqlCommand(CmdString, dbcl.Conn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(dr);

                string highSkillValue = string.Empty;
                string SkillValue = string.Empty;
                string semiSkillValue = string.Empty;
                string unSkillValue = string.Empty;

                // Check if there is exactly one row and the value in the 3rd index is "UNIFIED"
                if (dt.Rows.Count == 1 && dt.Rows[0][4].ToString() == "UNIFIED")
                {
                    Unified_LIGrid.Visible = true;
                    LineItems_Grid.DataSource = dt;
                    LineItems_Grid.DataBind();
                    LineItems_Grid.Columns[3].Visible = false;
                }
                else if (dt.Rows.Count > 1)
                {
                    Unified_LIGrid.Visible = false;
                    ShiftGrid.FooterRow.Visible = true;
                    Label lblFooterTotalHSShiftCount = (Label)ShiftGrid.FooterRow.FindControl("lbl_Footer_Total_HSShiftCount");
                    Label lblFooterTotalSShiftCount = (Label)ShiftGrid.FooterRow.FindControl("lbl_Footer_Total_SShiftCount");
                    Label lblFooterTotalSSShiftCount = (Label)ShiftGrid.FooterRow.FindControl("lbl_Footer_Total_SSShiftCount");
                    Label lblFooterTotalUSShiftCount = (Label)ShiftGrid.FooterRow.FindControl("lbl_Footer_Total_USShiftCount");


                    foreach (DataRow row in dt.Rows)
                    {
                        string value = row.Field<string>(4);
                        // Check values in the 4th index for different conditions
                        if (value == "HIGHLY-SKILLED")
                        {
                            lblFooterTotalHSShiftCount.Text = row.Field<string>(0);
                        }
                        else if (value == "SKILLED")
                        {
                            lblFooterTotalSShiftCount.Text = row.Field<string>(0);
                        }
                        else if (value == "SEMI-SKILLED")
                        {
                            lblFooterTotalSSShiftCount.Text = row.Field<string>(0);
                        }
                        else if (value == "UN-SKILLED")
                        {
                            lblFooterTotalUSShiftCount.Text = row.Field<string>(0);
                        }
                        // Add more conditions as needed
                        else
                        {
                            // Default actions if none of the specified conditions are met
                            // ...
                        }
                    }

                    LineItems_Grid.DataSource = dt;
                    LineItems_Grid.DataBind();
                }
                else
                {
                    LineItems_Grid.DataSource = dt;
                    LineItems_Grid.DataBind();
                }
            }
            else
            {
                DataTable dt5 = new DataTable();
                LineItems_Grid.DataSource = dt5;
                LineItems_Grid.DataBind();
            }
            dbcl.Conn.Close();
        }

        protected void LineItems_Grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Check if the row count is exactly 1
                if (LineItems_Grid.Rows.Count > 0)
                {
                    if (LineItems_Grid.Rows.Count == 1)
                    {
                        string shiftSkillValue = (e.Row.FindControl("lbl_Shift_Skill") as Label)?.Text;
                        if (shiftSkillValue == "UNIFIED")
                        {

                        }
                    }
                    else if (LineItems_Grid.Rows.Count > 1)
                    {

                    }
                }
                else
                {

                }
            }
        }

        protected void btn_back_Click(object sender, EventArgs e)
        {
            //Response.Redirect("../create_supplymemo.aspx?JOBID=" + jobid + "&viewid=1&y=" + yr + "&m=" + mnt + "");
            if (viewid == "1")
            {
                Response.Redirect($"../create_supplymemo.aspx?JOBID={jobid}&dbid={dbid}&supv={supv}&viewid=1&y={yr}&m={mnt}");
            }
            else if (viewid =="2")
            {
                Response.Redirect($"../create_supplymemo.aspx?JOBID={jobid}&dbid={dbid}&supv={supv}&viewid=2&y={yr}&m={mnt}");
            }

        }
    }
}