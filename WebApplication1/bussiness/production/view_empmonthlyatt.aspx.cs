using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace WebApplication1.bussiness.production
{
    public partial class view_empmonthlyatt : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
        //        {
        //            Response.Redirect("~/login.aspx");
        //        }
        //        else
        //        {
        //            dbcl.CalDateCombo1(DDL_Day, DDL_Month, DDL_Year);
        //            DDL_SearchType.Focus();
        //        }
        //    }
        //}

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
                    // 1. Initialize the Date Combo Boxes
                    dbcl.CalDateCombo1(DDL_Day, DDL_Month, DDL_Year);

                    // 2. NEW LOGIC: Check if the page is receiving automated parameters from the Anomaly Ledger
                    if (Request.QueryString["empwrk"] != null && Request.QueryString["month"] != null && Request.QueryString["year"] != null)
                    {
                        string empWrk = Request.QueryString["empwrk"].ToString();
                        string monthVal = Request.QueryString["month"].ToString();
                        string yearVal = Request.QueryString["year"].ToString();

                        // Automate the "By Workman SL" Search Type Selection[cite: 1]
                        DDL_SearchType.SelectedValue = "2";

                        // Manually trigger the visibility logic for the textboxes[cite: 1]
                        DDL_SearchType_SelectedIndexChanged(null, null);

                        // Fill the text box with the Workman ID from the URL[cite: 1]
                        txt_empworkman.Text = empWrk;

                        // Match the month and year dropdowns to the URL parameters[cite: 1]
                        if (DDL_Month.Items.FindByValue(monthVal) != null)
                            DDL_Month.SelectedValue = monthVal;

                        if (DDL_Year.Items.FindByValue(yearVal) != null)
                            DDL_Year.SelectedValue = yearVal;

                        // Automatically fire the grid binder to display results[cite: 1]
                        Binder();
                    }
                    else
                    {
                        // Standard manual load behavior[cite: 1]
                        DDL_SearchType.Focus();
                    }
                }
            }
        }


        protected void DDL_SearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_SearchType.SelectedIndex != 0)
            {
                if (DDL_SearchType.SelectedIndex == 1)
                {
                    Nameinputrow1.Visible = true;
                    Nameinputrow2.Visible = true;

                    WorkmanInput_Row1.Visible = false;
                    WorkmanInput_Row2.Visible = false;
                }
                else if (DDL_SearchType.SelectedIndex == 2)
                {
                    WorkmanInput_Row1.Visible = true;
                    WorkmanInput_Row2.Visible = true;

                    Nameinputrow1.Visible = false;
                    Nameinputrow2.Visible = false;
                }
            }
            else
            {
                Nameinputrow1.Visible = false;
                Nameinputrow2.Visible = false;

                WorkmanInput_Row1.Visible = false;
                WorkmanInput_Row2.Visible = false;

                DDL_SearchType.Focus();
                string title = "Notifications :";
                string body = "Please select search type";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            Binder();
        }

        private void Binder()
        {
            if (DDL_SearchType.SelectedIndex != 0)
            {
                string queryCondition = "";

                if (DDL_SearchType.SelectedIndex == 1 && !string.IsNullOrEmpty(txt_empname.Text))
                    queryCondition = "EmployeeName LIKE '%" + txt_empname.Text.Trim() + "%'";
                else if (DDL_SearchType.SelectedIndex == 2 && !string.IsNullOrEmpty(txt_empworkman.Text))
                    queryCondition = "EmployeeWrk = '" + txt_empworkman.Text.Trim() + "'";
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('Notifications :', 'Please enter search value');", true);
                    return;
                }

                pnlAnalytics.Visible = true;

                // Force the Calendar to visually display the selected Month and Year
                AttendanceCalendar.VisibleDate = new DateTime(Convert.ToInt32(DDL_Year.SelectedValue), Convert.ToInt32(DDL_Month.SelectedValue), 1);

                string query = $@"
                WITH OrderedPunches AS (
                    SELECT 
                        Id, JOBID, CreatedDate, Inpunch_Time, Outpunch_Time, WorkedHours, 
                        ProvidedOT, AttendanceStatus, AttendanceCode,
            
                        -- MUST ADD THESE 3 COLUMNS SO THE UPDATE LOGIC CAN FIND THEM
                        EmployeeWrk, JOB_Region, WourkHours, 

                        LAG(Outpunch_Time) OVER (ORDER BY Inpunch_Time) AS Prev_Outpunch
                    FROM tbl_attendance
                    WHERE YEAR(CreatedDate) = '{DDL_Year.SelectedValue}' 
                      AND MONTH(CreatedDate) = '{DDL_Month.SelectedValue}' 
                      AND {queryCondition}
                      AND ISNULL(DeleteStatus, 0) = 0
                )
                SELECT 
                    *,
                    DATEDIFF(HOUR, Prev_Outpunch, Inpunch_Time) AS Rest_Gap_Hours,
                    DAY(CreatedDate) AS DayNum,
                    CASE 
                        WHEN AttendanceCode IN ('A', 'Ab') THEN 'bg-black'
                        WHEN ISNULL(WorkedHours, 0) >= 12 THEN 'bg-red'
                        WHEN ISNULL(WorkedHours, 0) > 8 THEN 'bg-yellow'
                        WHEN ISNULL(WorkedHours, 0) > 0 THEN 'bg-green'
                        ELSE 'bg-gray'
                    END AS ColorClass,
                    CASE 
                        WHEN AttendanceCode IN ('A', 'Ab') THEN 'label-inverse'
                        WHEN ISNULL(WorkedHours, 0) >= 12 THEN 'label-danger'
                        WHEN ISNULL(WorkedHours, 0) > 8 THEN 'label-warning'
                        ELSE 'label-info'
                    END AS StatusBadgeClass,
                    'Code: ' + AttendanceCode + ' | Hrs: ' + CAST(ISNULL(WorkedHours, 0) AS VARCHAR) AS TooltipDetails
                FROM OrderedPunches
                ORDER BY CreatedDate ASC;";

                GridBinder(query);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('Notifications :', 'Please select search type');", true);
            }
        }

        private void GridBinder(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            using (SqlCommand cmd = new SqlCommand(CmdString, dbcl.Conn))
            {
                using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    ad.Fill(dt);

                    // 1. Cache the raw punch data for the GridView and Calendar
                    ViewState["AttendanceData"] = dt;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    // 2. GENERATE THE 31-DAY CONTINUOUS HEATMAP
                    DataTable heatmapDt = new DataTable();
                    heatmapDt.Columns.Add("DayNum", typeof(int));
                    heatmapDt.Columns.Add("ColorClass", typeof(string));
                    heatmapDt.Columns.Add("TooltipDetails", typeof(string));
                    heatmapDt.Columns.Add("FullDate", typeof(string));

                    int selectedYear = Convert.ToInt32(DDL_Year.SelectedValue);
                    int selectedMonth = Convert.ToInt32(DDL_Month.SelectedValue);
                    int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);

                    for (int i = 1; i <= daysInMonth; i++)
                    {
                        DataRow[] dayRows = dt.Select($"DayNum = {i}");
                        DateTime currentDate = new DateTime(selectedYear, selectedMonth, i);

                        string color = "";
                        string tooltip = "";

                        if (dayRows.Length > 0)
                        {
                            // A record exists for this day (Green, Yellow, Red, or Black for explicit Absent)
                            color = dayRows[0]["ColorClass"].ToString();
                            tooltip = dayRows[0]["TooltipDetails"].ToString();
                        }
                        else
                        {
                            // NO RECORD EXISTS: Calculate the Non-Working Day notation
                            if (currentDate.DayOfWeek == DayOfWeek.Sunday)
                            {
                                color = "bg-info"; // Blue for Sunday
                                tooltip = "Weekly Off (Sunday)";
                            }
                            else if (currentDate.Date > DateTime.Now.Date)
                            {
                                color = "bg-gray"; // Future dates
                                tooltip = "Future Date";
                            }
                            else
                            {
                                color = "bg-gray"; // Past weekday with no punch data
                                tooltip = "No Punch Data Found";
                            }
                        }

                        //heatmapDt.Rows.Add(i, color, tooltip);
                        heatmapDt.Rows.Add(i, color, tooltip, currentDate.ToString("yyyy-MM-dd"));
                    }

                    // Bind the physically complete month to the heatmap
                    rptHeatmap.DataSource = heatmapDt;
                    rptHeatmap.DataBind();

                    // 3. Process the top KPI Cards (Logic remains identical)
                    if (dt.Rows.Count > 0)
                    {
                        int totalShifts = dt.Rows.Count;
                        decimal totalReg = 0;
                        decimal totalOT = 0;
                        int restViolations = 0;

                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["WorkedHours"] != DBNull.Value) totalReg += Convert.ToDecimal(row["WorkedHours"]);
                            if (row["ProvidedOT"] != DBNull.Value) totalOT += Convert.ToDecimal(row["ProvidedOT"]);
                            if (row["Rest_Gap_Hours"] != DBNull.Value && Convert.ToInt32(row["Rest_Gap_Hours"]) < 8) restViolations++;
                        }

                        lblTotalShifts.Text = totalShifts.ToString();
                        lblRegHours.Text = totalReg.ToString("0.##");
                        lblOTHours.Text = totalOT.ToString("0.##");
                        lblRestViolations.Text = restViolations.ToString();
                    }
                    else
                    {
                        lblTotalShifts.Text = "0"; lblRegHours.Text = "0"; lblOTHours.Text = "0"; lblRestViolations.Text = "0";
                    }
                }
            }
            dbcl.DisconnectDb();
        }

        // 4. THE CALENDAR RENDER EVENT
        // 4. THE CALENDAR RENDER EVENT (Updated for Non-Working Days)
        protected void AttendanceCalendar_DayRender(object sender, DayRenderEventArgs e)
        {
            // Hide previous/next month overlap days
            if (e.Day.IsOtherMonth)
            {
                e.Cell.Text = "";
                e.Cell.BackColor = System.Drawing.Color.White;
                e.Cell.BorderWidth = 0;
                return;
            }

            if (ViewState["AttendanceData"] != null)
            {
                DataTable dt = (DataTable)ViewState["AttendanceData"];
                DataRow[] rows = dt.Select($"CreatedDate = '{e.Day.Date.ToString("yyyy-MM-dd")}'");

                if (rows.Length > 0)
                {
                    // Worker punched in or was explicitly marked Absent ('A')
                    e.Cell.CssClass = rows[0]["ColorClass"].ToString();
                    e.Cell.ToolTip = rows[0]["TooltipDetails"].ToString();
                    e.Cell.CssClass += " clickable-day";
                    e.Cell.Attributes.Add("data-date", e.Day.Date.ToString("yyyy-MM-dd"));
                    e.Cell.Attributes.Add("style", "cursor: pointer;"); // Make it look clickable
                }
                else
                {
                    // NON-WORKING DAYS (No DB Record)
                    if (e.Day.Date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#3498DB"); // bg-info
                        e.Cell.ForeColor = System.Drawing.Color.White;
                        e.Cell.ToolTip = "Weekly Off (Sunday)";
                        e.Cell.CssClass += " clickable-day";
                        e.Cell.Attributes.Add("data-date", e.Day.Date.ToString("yyyy-MM-dd"));
                        e.Cell.Attributes.Add("style", "cursor: pointer;"); // Make it look clickable
                    }
                    else if (e.Day.Date <= DateTime.Now.Date)
                    {
                        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#BDC3C7"); // bg-gray
                        e.Cell.ForeColor = System.Drawing.Color.White;
                        e.Cell.ToolTip = "No Data Available";
                        e.Cell.CssClass += " clickable-day";
                        e.Cell.Attributes.Add("data-date", e.Day.Date.ToString("yyyy-MM-dd"));
                        e.Cell.Attributes.Add("style", "cursor: pointer;"); // Make it look clickable
                    }
                }
            }
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            Response.Redirect("view_empmonthlyatt.aspx");

        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("homepage.aspx");

        }


        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            Binder(); // Re-bind the grid to show the textboxes
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1; // -1 exits edit mode
            Binder(); // Re-bind the grid to go back to standard view
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string jobid = Convert.ToString(e.CommandArgument);

            if (e.CommandName == "Approve")
            {
                //Response.Redirect("jobapprovalpage.aspx?JOBID=" + jobid + "");
                Response.Write("<script>window.open ('jobapprovalpage.aspx?JOBID=" + jobid + "','_blank');</script>");
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // 1. Ensure we are only touching Data Rows
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // =========================================================
                // NEW LOGIC: Interactive Dashboard Tagging (Runs on ALL rows)
                // =========================================================
                DataRowView drv = (DataRowView)e.Row.DataItem;
                if (drv != null && drv["CreatedDate"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(drv["CreatedDate"]);
                    string formattedDate = date.ToString("yyyy-MM-dd");

                    // Assign a unique ID and data attribute to the row based on the date
                    e.Row.Attributes.Add("id", "row-" + formattedDate);
                    e.Row.Attributes.Add("data-date", formattedDate);
                }

                // =========================================================
                // EXISTING LOGIC: Edit Mode Dropdowns (Runs ONLY on edited row)
                // =========================================================
                if (GridView1.EditIndex == e.Row.RowIndex)
                {
                    var DDL_AttendanceStatus = e.Row.FindControl("DDL_AttendanceStatus") as DropDownList;
                    if (DDL_AttendanceStatus != null)
                    {
                        var dt1 = new DataTable();
                        string cnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString();
                        using (var con = new SqlConnection(cnnString1))
                        {
                            con.Open();
                            var cmd1 = new SqlCommand("Select DISTINCT Status from tlb_attendancecodes where Approver='Yes'", con);
                            var da1 = new SqlDataAdapter(cmd1);
                            da1.Fill(dt1);
                            con.Close();
                        }

                        DDL_AttendanceStatus.DataSource = dt1;
                        DDL_AttendanceStatus.DataTextField = "Status";
                        DDL_AttendanceStatus.DataValueField = "Status";
                        DDL_AttendanceStatus.DataBind();
                        string AttendanceStatus = DataBinder.Eval(e.Row.DataItem, "AttendanceStatus").ToString();
                        DDL_AttendanceStatus.Items.FindByText(AttendanceStatus).Selected = true;
                    }

                    var DDL_AttendanceCode = e.Row.FindControl("DDL_AttendanceCode") as DropDownList;
                    if (DDL_AttendanceCode != null)
                    {
                        var dt2 = new DataTable();
                        string cnnString2 = System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString();
                        using (var con = new SqlConnection(cnnString2))
                        {
                            con.Open();
                            var cmd2 = new SqlCommand("Select Status_Name,Status_Code from tlb_attendancecodes where Approver='Yes' order by slno", con);
                            var da2 = new SqlDataAdapter(cmd2);
                            da2.Fill(dt2);
                            con.Close();
                        }

                        DDL_AttendanceCode.DataSource = dt2;
                        DDL_AttendanceCode.DataTextField = "Status_Name";
                        DDL_AttendanceCode.DataValueField = "Status_Code";
                        DDL_AttendanceCode.DataBind();
                        string AttendanceCode = DataBinder.Eval(e.Row.DataItem, "AttendanceCode").ToString();
                        DDL_AttendanceCode.Items.FindByValue(AttendanceCode).Selected = true;
                    }
                }
            }
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            Label JOBID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_JOBID");
            string jobid = JOBID.Text.ToString();

            Label lbl_JOB_Region = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_JOB_Region");
            string region = lbl_JOB_Region.Text.ToString();

            Label empwrk = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_EmployeeWrk");
            string workmansl = empwrk.Text.ToString();

            Label wrkhrs = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_WourkHours");
            Int32 emp_wrkhours = Convert.ToInt32(wrkhrs.Text.ToString());
            Int32 emp_wrkmnis = emp_wrkhours * 60;

            TextBox TextBoxWithIntime = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_Inpunch_Time");
            string new_intitme = TextBoxWithIntime.Text.ToString();
            DateTime timein = DateTime.ParseExact(new_intitme, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
            string convtimein = timein.ToString("yyyy-MM-dd hh:mm:ss tt");


            TextBox TextBoxWithOuttime = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_Outpunch_Time");
            string new_outtime = TextBoxWithOuttime.Text.ToString();
            DateTime timeout = DateTime.ParseExact(new_outtime, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
            string convtimeout = timeout.ToString("yyyy-MM-dd hh:mm:ss tt");


            DropDownList DDL_Lunch = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDL_LunchYesNo");
            string lunchyesno = DDL_Lunch.SelectedItem.Text.ToString();

            TextBox TextBoxWithOt = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_ProvidedOT");
            string new_ot = TextBoxWithOt.Text.ToString();
            decimal new_pot = Convert.ToDecimal(new_ot);

            DropDownList AttendanceStatus = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDL_AttendanceStatus");
            string ddl_newattensttaus = AttendanceStatus.SelectedItem.Text.ToString();

            DropDownList AttendanceCode = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDL_AttendanceCode");
            string ddl_newattencode = AttendanceCode.SelectedValue.ToString();


            Int32 workdmins = 0;
            decimal workedhours = .0m;
            dbcl.FindEmployeeWorkedTime(convtimein, convtimeout, ref workdmins, ref workedhours);
            decimal emp_calOThrs = .0m;

            if (region == "NINL")
            {
                dbcl.CalculateOvertimeRev(emp_wrkmnis, workdmins, lunchyesno, ref emp_calOThrs);
            }
            else
            {
                dbcl.CalculateOvertime(emp_wrkmnis, workdmins, lunchyesno, ref emp_calOThrs);
            }

            //dbcl.CalculateOvertime(emp_wrkmnis, workdmins, lunchyesno, ref emp_calOThrs);

            UpdateDetails(id, jobid, workmansl, convtimein, convtimeout, lunchyesno, workdmins, workedhours, emp_calOThrs, new_pot, ddl_newattensttaus, ddl_newattencode);

            GridView1.EditIndex = -1;

            Binder();
        }

        private void UpdateDetails(string id, string jobid, string empwrk, string intime, string outime, string lunchyesno, Int32 workdmins, decimal workedhours, decimal emp_calOThrs, decimal new_pot, string ddl_newattensttaus, string ddl_newattencode)
        {
            try
            {
                DateTime timein = DateTime.ParseExact(intime, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
                string convtimein = timein.ToString("yyyy-MM-dd hh:mm:ss tt");

                DateTime timeout = DateTime.ParseExact(outime, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
                string convtimeout = timeout.ToString("yyyy-MM-dd hh:mm:ss tt");

                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_attendance set Inpunch_Time=@Inpunch_Time, Outpunch_Time=@Outpunch_Time, WorkedTime=@WorkedTime , WorkedHours=@WorkedHours, LunchFactor=@LunchFactor, Calc_OT=@Calc_OT,  ProvidedOT=@ProvidedOT, LastModified=@LastModified, ModifiedByWrk=@ModifiedByWrk,ModifiedByName=@ModifiedByName,  AttendanceStatus=@AttendanceStatus, AttendanceCode=@AttendanceCode where Id=@Id and JOBID=@JOBID and EmployeeWrk=@EmployeeWrk";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@JOBID", jobid);
                cmd.Parameters.AddWithValue("@EmployeeWrk", empwrk);

                cmd.Parameters.AddWithValue("@Inpunch_Time", convtimein);
                cmd.Parameters.AddWithValue("@Outpunch_Time", convtimeout);
                cmd.Parameters.AddWithValue("@WorkedTime", workdmins);
                cmd.Parameters.AddWithValue("@WorkedHours", workedhours);
                cmd.Parameters.AddWithValue("@LunchFactor", lunchyesno);
                cmd.Parameters.AddWithValue("@Calc_OT", emp_calOThrs);
                cmd.Parameters.AddWithValue("@ProvidedOT", new_pot);
                //cmd.Parameters.AddWithValue("@Approval_Date", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@LastModified", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@ModifiedByWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@ModifiedByName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@AttendanceStatus", ddl_newattensttaus);
                cmd.Parameters.AddWithValue("@AttendanceCode", ddl_newattencode);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                string title = "Notifications :";
                string body = "Data has been UPDATED !!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }

            //Response.Redirect(Request.Url.AbsoluteUri);
        }

        

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            Label JOBID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_JOBID");
            string jobid = JOBID.Text.ToString();

            try
            {
                string cmdString = ("delete from tbl_attendance where Id='" + id + "' and JOBID= '" + jobid + "'");
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                dbcl.Conn.Close();

                string title = "Notifications :";
                string body = "Data has been DELETED !!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                //throw;
            }


            Binder();


        }
    }
}