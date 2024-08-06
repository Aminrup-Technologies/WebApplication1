using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Runtime.InteropServices.ComTypes;

namespace WebApplication1.bussiness.production
{
    public partial class vw_emp_attencal : System.Web.UI.Page
    {
        SqlConnection con = null;
        SqlDataAdapter da = null;
        DataSet ds = null;
        DataTable dt = null;
        string strSqlCommand = string.Empty;


        // These fields will represent the date-range for the calendar control
        protected DateTime? _firstDate = null;
        protected DateTime? _lastDate = null;
        protected int _dayIndex = 0;  // must be class-level, this is used by DayRender
        List<Activity> _monthActivities;  // will use ONE db-query to get all activities for the viewed month.

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
                    AssignDateRange(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                }
            }
            
        }

        protected void QueryExecuter(string strSqlCommand)
        
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString);
            da = new SqlDataAdapter(strSqlCommand, con);
            ds = new DataSet();
            da.Fill(ds, "Contacts");
            dt = ds.Tables["Contacts"];
            ViewState["EmpAtten"] = dt;
        }
        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.IsOtherMonth)
            {
                e.Cell.Controls.Clear();
                e.Cell.Text = string.Empty;
            }


            DataRowCollection drc = dt.Rows;
            if (drc.Count > 0)
            {
                Literal literal1 = new Literal();
                literal1.Text = "<br/>";
                e.Cell.Controls.Add(literal1);
                foreach (DataRow dr in drc)
                {
                    DateTime dtDob = Convert.ToDateTime(dr["CreatedDate"]);
                    if (e.Day.Date.Day == dtDob.Day && e.Day.Date.Month == dtDob.Month)
                    {
                        string attencode = dr["AttendanceCode"].ToString();
                        string ot = dr["ProvidedOT"].ToString();
                        string comb = "[" + attencode + "," + ot + "]";

                        if (attencode == "P")
                        {
                            e.Cell.BackColor = System.Drawing.Color.LightGreen;
                            e.Cell.ForeColor = System.Drawing.Color.Green;
                            e.Cell.ToolTip = "Working Day";
                            Image img1 = new Image();
                            img1.ImageUrl = "~\\erp_images\\tick-icon.png";
                            img1.ToolTip = dr["AttendanceCode"].ToString();
                            e.Cell.Controls.Add(img1);

                            Label lbl = new Label();
                            lbl.Text = comb;
                            lbl.ForeColor = System.Drawing.Color.DarkBlue;
                            lbl.Font.Bold = true;
                            e.Cell.Controls.Add(lbl);
                        }
                        else if (attencode == "OD")
                        {

                            e.Cell.BackColor = System.Drawing.Color.Orange;
                            e.Cell.ForeColor = System.Drawing.Color.Green;
                            e.Cell.ToolTip = "Working Day";
                            Image img1 = new Image();
                            img1.ImageUrl = "~\\erp_images\\tick-icon.png";
                            img1.ToolTip = dr["AttendanceCode"].ToString();
                            e.Cell.Controls.Add(img1);

                            Label lbl = new Label();
                            lbl.Text = comb;
                            lbl.ForeColor = System.Drawing.Color.Brown;
                            lbl.Font.Bold = true;
                            e.Cell.Controls.Add(lbl);
                        }
                        else if (attencode == "Ab")
                        {
                            e.Cell.BackColor = System.Drawing.Color.LightYellow;
                            e.Cell.ForeColor = System.Drawing.Color.Green;
                            e.Cell.ToolTip = "Working Day";
                            Image img1 = new Image();
                            img1.ImageUrl = "~\\erp_images\\tick-icon.png";
                            img1.ToolTip = dr["AttendanceCode"].ToString();
                            e.Cell.Controls.Add(img1);

                            Label lbl = new Label();
                            lbl.Text = comb;
                            lbl.ForeColor = System.Drawing.Color.Red;
                            lbl.Font.Bold = true;
                            e.Cell.Controls.Add(lbl);
                        }
                        else
                        {
                            e.Cell.BackColor = System.Drawing.Color.White;
                            e.Cell.ForeColor = System.Drawing.Color.Black;
                            e.Cell.ToolTip = "Working Day";
                            Image img1 = new Image();
                            img1.ImageUrl = "~\\erp_images\\tick-icon.png";
                            img1.ToolTip = dr["AttendanceCode"].ToString();
                            e.Cell.Controls.Add(img1);

                            Label lbl = new Label();
                            lbl.Text = comb;
                            lbl.ForeColor = System.Drawing.Color.Black;
                            lbl.Font.Bold = true;
                            e.Cell.Controls.Add(lbl);
                        }
                        
                    }
                }
            }
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            DateTime dtSeleted = Calendar1.SelectedDate;
            string strDate = dtSeleted.ToString("yyyy-MM-dd");
            //strSqlCommand = "Select Id,CreatedDate,EmployeeName,AttendanceCode,ProvidedOT,WorkedHours from tbl_attendance Where CreatedDate='" + strDate + "' and EmployeeWrk = 'A84'";
            //da = new SqlDataAdapter(strSqlCommand, con);
            //da.SelectCommand.CommandType = CommandType.Text;
            //ds = new DataSet();
            //da.Fill(ds, "Contacts");
            //GridView1.DataSource = ds.Tables["Contacts"];
            //GridView1.DataBind();
            //GridView1.Caption = "<h3 style='color:green'>Birthday Even Details Of Selected Date i.e" + strDate + "</he>";

            DataTable dt2 = new DataTable();
            if (ViewState["EmpAtten"] != null)
            {
                DataTable dt = (DataTable)ViewState["EmpAtten"];

                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DateTime attendt = dr.Field<DateTime>("CreatedDate");
                        string pq = Convert.ToString(attendt);
                        if (pq == strDate)
                            dt2.Rows.Add(dr.ItemArray);
                    }
                }
            }

            GridView1.DataSource = dt2;
            GridView1.DataBind();
        }

        protected void AssignDateRange(DateTime selectedDate)
        {
            // The calendar control will display up to seven days of the previous month in the top row, even if the 1st
            // lands on a Sunday.  We want _firstDate to be the closest Sunday, PREVIOUS to today.  If the 1st lands
            // on a Sunday, we must call StartOfWeek with yesterday (-1) to get it to return the previous Sunday.
            //_firstDate = DateTimeExtensions.StartOfWeek(selectedDate.AddDays(-1), DayOfWeek.Sunday);
            DateTime now = selectedDate;
            _firstDate = new DateTime(now.Year, now.Month, 1);

            // There will always be 42 days in the control, so set the upper bounds by moving to midnight, 42 days later
            // The date-range will be the boundary used to query the database for activities
            //_lastDate = _firstDate.Value.AddDays(42).AddHours(23).AddMinutes(59);
            _lastDate = _firstDate.Value.AddMonths(1).AddDays(-1);

            //strSqlCommand = "select CreatedDate,AttendanceCode,ProvidedOT from tbl_attendance where YEAR(CreatedDate)=" + now.Year.ToString() + " and MONTH(CreatedDate)="+ now.Month.ToString() + " and EmployeeWrk = 'A84' order by CreatedDate";

            strSqlCommand = "select CreatedDate, JOBID, SubmitterName, CONCAT(WorkOrderNo, ' [' , PermitNo, ']') as WorkOrderNo, CONCAT(JOB_SiteName, ' [' , JOB_Location, ']') as JOB_SiteName, CONCAT(JOB_InchargeName, ' [' , SiteIncharge_Approval, ']') as JOB_InchargeName, Inpunch_Time, Outpunch_Time, WorkedHours, LunchFactor, ProvidedOT, AttendanceStatus, AttendanceCode from tbl_attendance where YEAR(CreatedDate)='" + now.Year.ToString() + "' and MONTH(CreatedDate)='"+ now.Month.ToString() + "' and EmployeeWrk='"+ Session["WORKMAN"].ToString() + "' order by CreatedDate";
            QueryExecuter(strSqlCommand);

            // Replace this structure with yours.  Make a database call, to get all activities within the date-range for this calendar page
            // Use the _firstDate and _lastDate calculated above, so that the last few days of the prev month and the first few days
            // of the next month are included.
            //_monthActivities = new List<Activity>()
            //{ new Activity(new DateTime(2023,2,18,9,0,0), "John Doe", "Volunteer at the downtown soup kitchen, three hours"),
            //  new Activity(new DateTime(2023,2,16,8,0,0), "Jane Doe", "Referee youth soccer at the Y, until noon") };

            DataTable dt2 = new DataTable();
            _monthActivities = new List<Activity>();
            if (ViewState["EmpAtten"] != null)
            {
                DataTable dt = (DataTable)ViewState["EmpAtten"];
                DateTime CreatedDate = DateTime.Now;//---------- 0--->DateTime
                string JOBID = string.Empty;
                string SubmitterName = string.Empty;
                string WorkOrderNo = string.Empty;
                string JOB_SiteName = string.Empty;
                string JOB_InchargeName = string.Empty;
                string Inpunch_Time = string.Empty; //---------6-->DateTime
                string Outpunch_Time = string.Empty;//----------7-->DateTime
                string WorkedHours = string.Empty;              //--------8-->Decimal
                string LunchFactor = string.Empty;
                string ProvidedOT = string.Empty;               //--------10-->Decimal
                string AttendanceStatus = string.Empty;
                string AttendanceCode = string.Empty;  //------------ 12 Activities
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CreatedDate = Convert.ToDateTime(dt.Rows[i][0]);
                        JOBID = dt.Rows[i][1].ToString();
                        SubmitterName = dt.Rows[i][2].ToString();
                        WorkOrderNo = dt.Rows[i][3].ToString();
                        JOB_SiteName = dt.Rows[i][4].ToString();
                        JOB_InchargeName = dt.Rows[i][5].ToString();
                        Inpunch_Time = dt.Rows[i][6].ToString();
                        Outpunch_Time =dt.Rows[i][7].ToString();
                        WorkedHours = dt.Rows[i][8].ToString();
                        LunchFactor = dt.Rows[i][9].ToString();
                        ProvidedOT = dt.Rows[i][10].ToString();
                        AttendanceStatus = dt.Rows[i][11].ToString();
                        AttendanceCode = dt.Rows[i][12].ToString();

                        _monthActivities.Add(new Activity(CreatedDate, JOBID, SubmitterName, WorkOrderNo, JOB_SiteName, JOB_InchargeName, Inpunch_Time, Outpunch_Time, WorkedHours, LunchFactor, ProvidedOT, AttendanceStatus, AttendanceCode));
                    }
                }
            }
        }

        protected void Calendar2_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime thisDate = e.Day.Date.Date;

            // Search the activities for the month (that we already retrieved from the DB), and then sort them chronologically
            List<Activity> todayActivities = _monthActivities.Where(x => x.CreatedDate.Date == thisDate.Date).ToList();
            todayActivities.Sort((a, b) => a.CreatedDate.CompareTo(b.CreatedDate));

            // If there is at least one activity today, add a className to the cell so that it gets an indicator image
            if (todayActivities.Count > 0)
                e.Cell.CssClass += " activityDay";

            // ASP.NET will, by default, invoke a postback during the click event.  Replace it with a JS call, so we can populate
            // the displayed activity-list client-side by pulling information from the corresponding hidden field.
            e.Cell.Text = string.Format("<a href='#' onclick='Showday(this,{0});return false;' title='{1}'>{2}</a>",
                                       _dayIndex.ToString(), thisDate.ToString("dddd, MMMM d, yyyy"), thisDate.Day.ToString());

            // Use javascript to handle mouse-hover, because the :hover CSS technique is not appreciated by iOS
            e.Cell.Attributes["onmouseover"] = "MouseOver(this)";
            e.Cell.Attributes["onmouseout"] = "MouseOut(this)";

            // Separate the activities with "|", and use "~" to separate the fields within each activity.
            string allActivitiesStr = string.Empty;
            foreach (Activity activity in todayActivities)
            {
                int itemIndex = todayActivities.IndexOf(activity);
                if (itemIndex != 0)
                    allActivitiesStr += "|";
                allActivitiesStr += string.Format("{0}~{1}~{2}~{3}~{4}~{5}~{6}~{7}~{8}~{9}~{10}~{11}~{12}",
                    activity.CreatedDate.ToString("dd MMM yyyy"),
                    activity.JOBID,
                    activity.SubmitterName,
                    activity.WorkOrderNo,
                    activity.JOB_SiteName,
                    activity.JOB_InchargeName,
                    activity.Inpunch_Time,
                    activity.Outpunch_Time,
                    activity.WorkedHours,
                    activity.LunchFactor,
                    activity.ProvidedOT,
                    activity.AttendanceStatus,
                    activity.AttendanceCode);
            }
            // Place the activities into the hidden field, so Javascript code can get them when the user clicks/taps
            GetHiddenField(_dayIndex).Value = allActivitiesStr;
            _dayIndex++;
        }

        protected HiddenField GetHiddenField(int index)
        {
            List<HiddenField> allHidden = new List<HiddenField>()
            { hdnDay0, hdnDay1, hdnDay2, hdnDay3, hdnDay4, hdnDay5, hdnDay6,
              hdnDay7, hdnDay8, hdnDay9, hdnDay10, hdnDay11, hdnDay12, hdnDay13,
              hdnDay14, hdnDay15, hdnDay16, hdnDay17, hdnDay18, hdnDay19, hdnDay20,
              hdnDay21, hdnDay22, hdnDay23, hdnDay24, hdnDay25, hdnDay26, hdnDay27,
              hdnDay28, hdnDay29, hdnDay30, hdnDay31, hdnDay32, hdnDay33, hdnDay34,
              hdnDay35, hdnDay36, hdnDay37, hdnDay38, hdnDay39, hdnDay40, hdnDay41 };

            return allHidden[index];
        }

        public class Activity
        {
            public DateTime CreatedDate { get; set; }
            public string JOBID { get; set; }
            public string SubmitterName { get; set; }
            public string WorkOrderNo { get; set; }
            public string JOB_SiteName { get; set; }
            public string JOB_InchargeName { get; set; }
            public string Inpunch_Time { get; set; }
            public string Outpunch_Time { get; set; }
            public string WorkedHours { get; set; }

            public string LunchFactor { get; set; }
            public string ProvidedOT { get; set; }
            public string AttendanceStatus { get; set; }
            public string AttendanceCode { get; set; }

            public Activity(DateTime dt, string name, string desc, string pono, string SiteName, string InchargeName, string InpunchTime, string OutpunchTime, string Worked, string Lunch, string OT, string Status, string Code)
            {
                CreatedDate = dt;
                JOBID = name;
                SubmitterName = desc;
                WorkOrderNo = pono;
                JOB_SiteName = SiteName;
                JOB_InchargeName = InchargeName;
                Inpunch_Time = InpunchTime;
                Outpunch_Time = OutpunchTime;
                WorkedHours = Worked;
                LunchFactor = Lunch;
                ProvidedOT = OT;
                AttendanceStatus = Status;
                AttendanceCode = Code;
            }
        }

        protected void Calendar2_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            // The  has changed the month -- do a new DB query
            AssignDateRange(e.NewDate);           
        }
    }
}

public static class DateTimeExtensions
{
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = dt.DayOfWeek - startOfWeek;
        if (diff < 0)
        {
            diff += 7;
        }

        return dt.AddDays(-1 * diff).Date;
    }
}