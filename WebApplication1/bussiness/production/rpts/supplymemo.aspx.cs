using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

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

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(jobid))
                {
                    Bind_JOBIDDetails(jobid);
                }
            }
        }

        // Helper Method for Safe Null/Empty checks
        private string GetSafeValue(object obj)
        {
            if (obj == null || obj == DBNull.Value || string.IsNullOrWhiteSpace(obj.ToString()))
            {
                return "No Data";
            }
            return obj.ToString().Trim();
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
                    string smjid = GetSafeValue(dt.Rows[0]["Level1_BillingCode"]);
                    lbl_smjid1.Text = lbl_smjid2.Text = smjid;

                    string jobdate = dt.Rows[0]["CreatedDate"]?.ToString();

                    // FIXED: Declare variable before TryParse for C# 5.0 compatibility
                    DateTime dt1;
                    if (!string.IsNullOrWhiteSpace(jobdate) && DateTime.TryParse(jobdate, out dt1))
                    {
                        DayOfWeek dow = dt1.DayOfWeek; //enum
                        string str = dow.ToString(); //string
                        string shift = GetSafeValue(dt.Rows[0]["JOB_Shift"]);

                        string shiftDisplay = shift != "No Data" ? " [" + shift + "]" : "";
                        string abc2 = DateBinder(jobdate) + " [" + str + "]" + shiftDisplay;
                        lbl_jobdatedetails.Text = abc2;
                    }
                    else
                    {
                        lbl_jobdatedetails.Text = "No Data";
                    }

                    lbl_pono.Text = GetSafeValue(dt.Rows[0]["WorkOrderNo"]);
                    lbl_jobid.Text = GetSafeValue(dt.Rows[0]["JOBID"]);

                    string permitData = GetSafeValue(dt.Rows[0]["JOB_PermitNo"]);
                    if (permitData != "No Data")
                    {
                        string[] permits = permitData.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        if (permits.Length > 0)
                        {
                            int itemsPerLine = 3;
                            int maxLength = permits.Max(p => p.Trim().Length);

                            StringBuilder formatted = new StringBuilder();

                            for (int i = 0; i < permits.Length; i++)
                            {
                                string permit = permits[i].Trim().PadRight(maxLength); // Align all items
                                formatted.Append(permit);

                                // Add comma + space after every item except the last
                                if (i != permits.Length - 1)
                                    formatted.Append(", ");

                                // Wrap to new line after every N items
                                if ((i + 1) % itemsPerLine == 0)
                                    formatted.AppendLine();
                            }
                            lbl_permitno.Text = formatted.ToString();
                        }
                        else
                        {
                            lbl_permitno.Text = "No Data";
                        }
                    }
                    else
                    {
                        lbl_permitno.Text = "No Data";
                    }

                    lbl_jobtitle.Text = GetSafeValue(dt.Rows[0]["JOB_Title"]);
                    lbl_jobrgn.Text = GetSafeValue(dt.Rows[0]["JOB_Region"]);
                    lbl_jobdept.Text = GetSafeValue(dt.Rows[0]["JOB_Dept"]);
                    lbl_jobloc.Text = GetSafeValue(dt.Rows[0]["JOB_Location"]);
                    lbl_jobsupvname.Text = GetSafeValue(dt.Rows[0]["Creator_Name"]);
                    lbl_jobsupvwrk.Text = GetSafeValue(dt.Rows[0]["Creator_Workman"]);
                    lbl_siteincharge.Text = GetSafeValue(dt.Rows[0]["JOB_InchargeName"]);
                    lbl_inchargewrk.Text = GetSafeValue(dt.Rows[0]["JOB_InchargeWrk"]);

                    if (smjid != "No Data")
                    {
                        Bind_SMJIDDetails(smjid);
                        Bind_Manpower(smjid);
                        Bind_ShiftData(smjid);
                        Bind_LineItemData(smjid);
                    }
                }
            }
            catch (Exception ex)
            {
                // Optionally handle/log errors
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

                string query = "select * from tbl_supplymemojobsdetails where SMJID=@SMJID";
                SqlParameter[] pram = {
                                          new SqlParameter("@SMJID",jobid),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    string jobdate = dt.Rows[0]["SMJ_Createdate"]?.ToString();

                    // FIXED: Declare variable before TryParse for C# 5.0 compatibility
                    DateTime dt1;
                    if (!string.IsNullOrWhiteSpace(jobdate) && DateTime.TryParse(jobdate, out dt1))
                    {
                        DayOfWeek dow = dt1.DayOfWeek; //enum
                        string str = dow.ToString(); //string
                        string abc2 = DateBinder(jobdate) + " [" + str + "]";
                        lbl_smjdate.Text = lbl_createdon.Text = abc2;
                    }
                    else
                    {
                        lbl_smjdate.Text = lbl_createdon.Text = "No Data";
                    }

                    lbl_smjcreatorname.Text = lbl_createdbyname.Text = GetSafeValue(dt.Rows[0]["CreatorName"]);
                    lbl_smjcreatorwrk.Text = lbl_createdbyid.Text = GetSafeValue(dt.Rows[0]["CreatorWorkmen"]);

                    lbl_printedon.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

                    if (Session["USERNAME"] != null)
                        lbl_printedbyname.Text = GetSafeValue(Session["USERNAME"]);
                    else
                        lbl_printedbyname.Text = "No Data";

                    if (Session["WORKMAN"] != null)
                        lbl_printedbyid.Text = GetSafeValue(Session["WORKMAN"]);
                    else
                        lbl_printedbyid.Text = "No Data";

                    string memoTypeStr = GetSafeValue(dt.Rows[0]["MemoType"]);
                    if (memoTypeStr != "No Data" && int.TryParse(memoTypeStr, out MemoType))
                    {
                        if (MemoType == 0)
                        {
                            if (Unified_LIGrid_0 != null) Unified_LIGrid_0.Visible = false;
                            if (Unified_LIGrid_2 != null) Unified_LIGrid_2.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle Exception
            }
            finally
            {
                if (dbcl.Conn.State == ConnectionState.Open)
                    dbcl.Conn.Close();
            }
        }

        private string DateBinder(string date)
        {
            if (string.IsNullOrWhiteSpace(date)) return "No Data";

            // FIXED: Declare variable before TryParse for C# 5.0 compatibility
            DateTime oDate;
            if (DateTime.TryParse(date, out oDate))
            {
                string day = oDate.Day < 10 ? "0" + oDate.Day.ToString() : oDate.Day.ToString();
                string month = oDate.Month < 10 ? "0" + oDate.Month.ToString() : oDate.Month.ToString();
                return day + "-" + month + "-" + oDate.Year;
            }
            return "No Data";
        }

        private void Bind_Manpower(string jobid)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string CmdString = "select concat(b.EmployeeName,' [',b.EmployeeWrk,']') as employeename, a.PO_SkillCategory, a.PO_EmpDesignation,b.Inpunch_Time, b.Outpunch_Time, b.safetypassno, a.ShiftCalc from tbl_supplymemojobmanpower a, tbl_attendance b where a.Ref_JOBID = b.JOBID and a.RefDBId = b.id and a.SMJID ='" + jobid + "' order by a.Id";
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
            }
            catch (Exception ex)
            {
                // Log Error
            }
            finally
            {
                if (dbcl.Conn.State == ConnectionState.Open)
                    dbcl.Conn.Close();
            }
        }

        private void Bind_ShiftData(string jobid)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string CmdString = "select Total_HSShiftCount, Total_SShiftCount, Total_SSShiftCount, Total_USShiftCount, Total_ShiftCount FROM tbl_supplymemojobsdetails where SMJID ='" + jobid + "' order by Id";
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
            }
            catch (Exception ex)
            {
                // Log Error
            }
            finally
            {
                if (dbcl.Conn.State == ConnectionState.Open)
                    dbcl.Conn.Close();
            }
        }

        private void Bind_LineItemData(string jobid)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string CmdString = "select ServiceNumber, Service_Description, Order_Quantity, PerUnit_Value, Shift_Skill FROM tbl_SupMem_LineItems_Data where SMJID ='" + jobid + "' order by Id";
                SqlCommand cmd = new SqlCommand(CmdString, dbcl.Conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(dr);

                    if (dt.Rows.Count == 1 && dt.Rows[0][4].ToString() == "UNIFIED")
                    {
                        if (Unified_LIGrid != null) Unified_LIGrid.Visible = true;
                        LineItems_Grid.DataSource = dt;
                        LineItems_Grid.DataBind();
                        LineItems_Grid.Columns[3].Visible = false;
                    }
                    else if (dt.Rows.Count > 1)
                    {
                        if (Unified_LIGrid != null) Unified_LIGrid.Visible = false;
                        ShiftGrid.FooterRow.Visible = true;
                        Label lblFooterTotalHSShiftCount = (Label)ShiftGrid.FooterRow.FindControl("lbl_Footer_Total_HSShiftCount");
                        Label lblFooterTotalSShiftCount = (Label)ShiftGrid.FooterRow.FindControl("lbl_Footer_Total_SShiftCount");
                        Label lblFooterTotalSSShiftCount = (Label)ShiftGrid.FooterRow.FindControl("lbl_Footer_Total_SSShiftCount");
                        Label lblFooterTotalUSShiftCount = (Label)ShiftGrid.FooterRow.FindControl("lbl_Footer_Total_USShiftCount");

                        foreach (DataRow row in dt.Rows)
                        {
                            string value = GetSafeValue(row.Field<string>(4));
                            string amount = GetSafeValue(row.Field<string>(0));

                            if (value == "HIGHLY-SKILLED")
                            {
                                if (lblFooterTotalHSShiftCount != null) lblFooterTotalHSShiftCount.Text = amount;
                            }
                            else if (value == "SKILLED")
                            {
                                if (lblFooterTotalSShiftCount != null) lblFooterTotalSShiftCount.Text = amount;
                            }
                            else if (value == "SEMI-SKILLED")
                            {
                                if (lblFooterTotalSSShiftCount != null) lblFooterTotalSSShiftCount.Text = amount;
                            }
                            else if (value == "UN-SKILLED")
                            {
                                if (lblFooterTotalUSShiftCount != null) lblFooterTotalUSShiftCount.Text = amount;
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
            }
            catch (Exception ex)
            {
                // Handle Exception
            }
            finally
            {
                if (dbcl.Conn.State == ConnectionState.Open)
                    dbcl.Conn.Close();
            }
        }

        protected void LineItems_Grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
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
            }
        }

        protected void btn_back_Click(object sender, EventArgs e)
        {
            if (viewid == "1")
            {
                Response.Redirect($"../create_supplymemo.aspx?JOBID={jobid}&dbid={dbid}&supv={supv}&viewid=1&y={yr}&m={mnt}");
            }
            else if (viewid == "2")
            {
                Response.Redirect($"../create_supplymemo.aspx?JOBID={jobid}&dbid={dbid}&supv={supv}&viewid=2&y={yr}&m={mnt}");
            }
        }
    }
}