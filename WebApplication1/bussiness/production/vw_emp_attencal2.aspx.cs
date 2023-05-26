using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;

namespace WebApplication1.bussiness.production
{
    public partial class vw_emp_attencal2 : System.Web.UI.Page
    {
        private LinkButton lb;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gridbind();
            }
        }

        protected void gridbind()
        {
            if (Session["Events"] != null)
            {
                DataTable dt = (DataTable)Session["Events"];
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }

        }

        protected void CategoryGridbind(DataTable dt)
        {


            GridView2.DataSource = dt;
            GridView2.DataBind();


        }

        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            if (Session["Events"] != null)
            {
                DataTable dt = (DataTable)Session["Events"];
                var count = (from events in dt.AsEnumerable() where events.Field("Date").Contains(e.Day.Date.ToShortDateString()) select events).Count();
                if (Convert.ToInt32(count) > 0)
                {
                    var span = new HtmlGenericControl("div");
                    HyperLink lb = new HyperLink();
                    lb.Text = count.ToString();
                    lb.CssClass = "noti_bubble";
                    lb.NavigateUrl = Page.ClientScript.GetPostBackClientHyperlink(lnkButton, e.Day.Date.ToShortDateString(), true);

                    span.Controls.Add(lb);
                    span.ID = "noti_Container";

                    e.Cell.Controls.Add(span);



                }
            }
        }

        protected void lnkButton_Click(object sender, EventArgs e)
        {
            string date = Request.Form["__EVENTARGUMENT"].ToString();
            if (Session["Events"] != null)
            {
                DataTable dt = (DataTable)Session["Events"];
                DataTable catdt = (from events in dt.AsEnumerable() where events.Field("Date").Contains(date) group events by events.Field("Category") into groups select groups.First()).CopyToDataTable();
                CategoryGridbind(catdt);
            }
            hdnlnk3_Click(null, null);

        }
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            string date = Calendar1.SelectedDate.ToShortDateString();
            hdnlink_Click(null, null);

        }

        protected void Calendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            Calendar1_ModalPopupExtender.Hide();
        }
        protected void hdnlink_Click(object sender, EventArgs e)
        {
            Calendar1_ModalPopupExtender.Show();
        }
        protected void hdnlnk2_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
        }
        protected void hdnlnk3_Click(object sender, EventArgs e)
        {
            ModalPopupExtender2.Show();
        }
        protected void lnkSubmit_Click(object sender, EventArgs e)
        {
            if (Session["Events"] != null)
            {
                DataTable dt = (DataTable)Session["Events"];
                DataRow dr = dt.NewRow();
                dr[0] = txtEventName.Text.Trim();
                dr[1] = txtLocation.Text.Trim();
                dr[2] = Calendar1.SelectedDate.ToShortDateString();
                dr[3] = txtDesc.Text.Trim();
                dr[4] = ddlCategory.SelectedItem.Value.ToString();
                dt.Rows.Add(dr);
                Session["Events"] = dt;
            }
            else
            {

                DataTable events = new DataTable();

                DataColumn dc1 = new DataColumn("Event Name", typeof(String));
                DataColumn dc2 = new DataColumn("Location", typeof(String));
                DataColumn dc3 = new DataColumn("Date", typeof(String));
                DataColumn dc4 = new DataColumn("Description", typeof(String));
                DataColumn dc5 = new DataColumn("Category", typeof(String));

                events.Columns.Add(dc1);
                events.Columns.Add(dc2);
                events.Columns.Add(dc3);
                events.Columns.Add(dc4);
                events.Columns.Add(dc5);
                DataRow dr = events.NewRow();
                dr[0] = txtEventName.Text.Trim();
                dr[1] = txtLocation.Text.Trim();
                dr[2] = Calendar1.SelectedDate.ToShortDateString();
                dr[3] = txtDesc.Text.Trim();
                dr[4] = ddlCategory.SelectedItem.Value.ToString();

                events.Rows.Add(dr);
                Session["Events"] = events;
            }
            txtDesc.Text = string.Empty; txtEventName.Text = string.Empty; txtLocation.Text = string.Empty;
        }

        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Category")
            {
                if (Session["Events"] != null)
                {
                    DataTable dt = (DataTable)Session["Events"];
                    string[] arg = new string[2];
                    arg = e.CommandArgument.ToString().Split(';');
                    DataTable eventscategorywise = (from events in dt.AsEnumerable() where events.Field("Date").Contains(arg[1]) && events.Field("Category") == arg[0].ToString() select events).CopyToDataTable();
                    GridView1.DataSource = eventscategorywise;
                    GridView1.DataBind();
                    hdnlnk2_Click(null, null);
                }
            }
        }
    }
}