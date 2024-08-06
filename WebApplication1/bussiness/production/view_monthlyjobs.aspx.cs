using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DocumentFormat.OpenXml.Wordprocessing;

namespace WebApplication1.bussiness.production
{
    public partial class view_monthlyjobs : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "Loader();", true);
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = 'IN' order by Id ";
                    BindRegions(CmdString1);

                    dbcl.CalDateCombo1(DDL_Day, DDL_Month, DDL_Year);

                    DateTime d = DateTime.Now;
                    string month = d.Month.ToString();
                    string year = d.Year.ToString();
                    string region = Session["REGION"].ToString();

                    if (Session["USTATE"].ToString() == "PI")
                    {
                        //string CmdString2 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = 'IN' order by Id ";
                        //BindRegions(CmdString2);
                    }
                    else
                    {
                        CheckforUser();
                        GridBinder(year, month, region);
                    }
                }
            }
        }


        private void CheckforUser()
        {
            DDL_Region.SelectedValue = Session["REGION"].ToString();
            DDL_Region.Enabled = false;
            string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='" + Session["USTATE"].ToString() + "' and Work_Region_Code = '" + Session["REGION"].ToString() + "' order by Id ";
            BindCompany(CmdString3);

            CompanyBinder();
        }


        private void CompanyBinder()
        {
            string usercomp = Session["COMPANY_CODE"].ToString();
            DDL_Company.SelectedValue = usercomp;
            DDL_Company.Enabled = false;
        }

        private void BindRegions(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Region.DataSource = Cmd.ExecuteReader();
            DDL_Region.DataTextField = "Work_Region_Name";
            DDL_Region.DataValueField = "Work_Region_Code";
            DDL_Region.DataBind();
            DDL_Region.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_Region_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='OD' and Work_Region_Code = '" + DDL_Region.SelectedValue.ToString() + "' order by Id ";
            BindCompany(CmdString3);
        }

        private void BindCompany(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Company.DataSource = Cmd.ExecuteReader();
            DDL_Company.DataTextField = "Company_Name";
            DDL_Company.DataValueField = "Company_Code";
            DDL_Company.DataBind();
            DDL_Company.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string year = DDL_Year.SelectedItem.Text.ToString();
            string month = DDL_Month.SelectedItem.Text.ToString();
            string region = DDL_Region.SelectedValue.ToString();

            GridBinder(year, month, region);
        }

        protected void GridBinder(string year, string month, string region)
        {
            string query = "select a.CreatedDate, (select count(Id) as count from tbl_jobs where YEAR(CreatedDate)='"+year+"' and MONTH(CreatedDate)='"+month+"' and JOB_Region='"+region+ "' and CreatedDate=a.CreatedDate) as total_count, (select count(Id) as count from tbl_attendance where YEAR(CreatedDate)='" + year + "' and MONTH(CreatedDate)='" + month + "' and JOB_Region='" + region + "' and CreatedDate=a.CreatedDate) as total_manpower, (select COALESCE(SUM(ProvidedOT),0) as count from tbl_attendance where YEAR(CreatedDate)='" + year + "' and MONTH(CreatedDate)='" + month + "' and JOB_Region='" + region + "' and CreatedDate=a.CreatedDate) as total_overtime, (select count(Id) as count from tbl_jobs where Incharge_Approval='Pending' and YEAR(CreatedDate)='" + year+"' and MONTH(CreatedDate)='"+month+"' and JOB_Region='"+region+"' and CreatedDate=a.CreatedDate) as pending_count, (select count(Id) as count from tbl_jobs where Incharge_Approval='Returned' and  YEAR(CreatedDate)='"+year+"' and MONTH(CreatedDate)='"+month+"' and JOB_Region='"+region+"' and CreatedDate=a.CreatedDate) as returned_count, (select count(Id) as count from tbl_jobs where Incharge_Approval='Rejected' and  YEAR(CreatedDate)='"+year+"' and MONTH(CreatedDate)='"+month+"' and JOB_Region='"+region+"' and CreatedDate=a.CreatedDate) as rejected_count, (select count(Id) as count from tbl_jobs where Incharge_Approval='Approved' and  YEAR(CreatedDate)='"+year+"' and MONTH(CreatedDate)='"+month+"' and JOB_Region='"+region+"' and CreatedDate=a.CreatedDate) as approved_count, (select count(Id) as count from tbl_jobs where EntryExit='Created' and  YEAR(CreatedDate)='"+year+"' and MONTH(CreatedDate)='"+month+"' and JOB_Region='"+region+"' and CreatedDate=a.CreatedDate) as Idlejob_count, (select count(Id) as count from tbl_jobs where EntryExit='Entry' and  YEAR(CreatedDate)='"+year+"' and MONTH(CreatedDate)='"+month+"' and JOB_Region='"+region+"' and CreatedDate=a.CreatedDate) as openjobs_count, (select count(Id) as count from tbl_jobs where EntryExit='Exit' and  YEAR(CreatedDate)='"+year+"' and MONTH(CreatedDate)='"+month+"' and JOB_Region='"+region+"' and CreatedDate=a.CreatedDate) as closedjobs_count, (select count(Id) as count from tbl_jobs where BillingType='Manpower Supply' and  YEAR(CreatedDate)='"+year+"' and MONTH(CreatedDate)='"+month+"' and JOB_Region='"+region+"' and CreatedDate=a.CreatedDate) as manpowerjobs, (select count(Id) as count from tbl_jobs where BillingType='Line Item' and  YEAR(CreatedDate)='"+year+"' and MONTH(CreatedDate)='"+month+"' and JOB_Region='"+region+"' and CreatedDate=a.CreatedDate) as lineitemjobs, (select count(Id) as count from tbl_jobs where BillingType is null and  YEAR(CreatedDate)='"+year+"' and MONTH(CreatedDate)='"+month+"' and JOB_Region='"+region+"' and CreatedDate=a.CreatedDate) as notassigned, (select count(Id) as count from tbl_jobs where BillingType='Non-Billing' and  YEAR(CreatedDate)='"+year+"' and MONTH(CreatedDate)='"+month+"' and JOB_Region='"+region+"' and CreatedDate=a.CreatedDate) as nonbillingjobs from tbl_jobs a where YEAR(CreatedDate)='"+year+"' and MONTH(CreatedDate)='"+month+"' and JOB_Region='"+region+"' group by CreatedDate";
            BindGridView1(query);
        }

        //private void BindGridView1(string query)
        //{
        //    dbcl.Sqlconnection();
        //    dbcl.ConnectDb();
        //    SqlCommand cmd = new SqlCommand(query, dbcl.Conn);
        //    SqlDataAdapter ad = new SqlDataAdapter(cmd);
        //    cmd.CommandTimeout = 0;
        //    DataTable dt = new DataTable();
        //    ad.Fill(dt);
        //    GridView1.DataSource = dt;
        //    GridView1.DataBind();

        //    Int32 total_org = 0;
        //    Int32 ttlmnpr_org = 0;
        //    decimal ttlot_org = 0.0m;
        //    Int32 idlejobs_org = 0;
        //    Int32 openjobs_org = 0;
        //    Int32 closedjobs_org = 0;
        //    Int32 pendingjobs_org = 0;
        //    Int32 returnedjobs_org = 0;
        //    Int32 rejectedjobs_org = 0;
        //    Int32 approvedjobs_org = 0;
        //    Int32 manpowerjobs_org = 0;
        //    Int32 lineitemjobs_org = 0;
        //    Int32 unassignedjobs_org = 0;
        //    Int32 nonbillingjobs_org = 0;

        //    Int32 total = dt.AsEnumerable().Sum(row => row.Field<Int32>("total_count"));
        //    Int32 ttlmnpr = dt.AsEnumerable().Sum(row => row.Field<Int32>("total_manpower"));
        //    decimal ttlot = dt.AsEnumerable().Sum(row => row.Field<decimal>("total_overtime"));
        //    Int32 idlejobs = dt.AsEnumerable().Sum(row => row.Field<Int32>("Idlejob_count"));
        //    Int32 openjobs = dt.AsEnumerable().Sum(row => row.Field<Int32>("openjobs_count"));
        //    Int32 closedjobs = dt.AsEnumerable().Sum(row => row.Field<Int32>("closedjobs_count"));
        //    Int32 pendingjobs = dt.AsEnumerable().Sum(row => row.Field<Int32>("pending_count"));
        //    Int32 returnedjobs = dt.AsEnumerable().Sum(row => row.Field<Int32>("returned_count"));
        //    Int32 rejectedjobs = dt.AsEnumerable().Sum(row => row.Field<Int32>("rejected_count"));
        //    Int32 approvedjobs = dt.AsEnumerable().Sum(row => row.Field<Int32>("approved_count"));
        //    Int32 manpowerjobs = dt.AsEnumerable().Sum(row => row.Field<Int32>("manpowerjobs"));
        //    Int32 lineitemjobs = dt.AsEnumerable().Sum(row => row.Field<Int32>("lineitemjobs"));
        //    Int32 unassignedjobs = dt.AsEnumerable().Sum(row => row.Field<Int32>("notassigned"));
        //    Int32 nonbillingjobs = dt.AsEnumerable().Sum(row => row.Field<Int32>("nonbillingjobs"));
        //    //Int32 pendingmeno = dt.AsEnumerable().Sum(row => row.Field<Int32>("pendingmemo"));
        //    //Int32 memocreated = dt.AsEnumerable().Sum(row => row.Field<Int32>("createdmemo"));

        //    GridView1.FooterRow.Cells[0].Text = "Total";
        //    GridView1.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;
        //    GridView1.FooterRow.Cells[2].Text = total.ToString("D");
        //    GridView1.FooterRow.Cells[3].Text = ttlmnpr.ToString("D");
        //    GridView1.FooterRow.Cells[4].Text = ttlot.ToString("F");
        //    GridView1.FooterRow.Cells[5].Text = idlejobs.ToString("D");
        //    GridView1.FooterRow.Cells[6].Text = openjobs.ToString("D");
        //    GridView1.FooterRow.Cells[7].Text = closedjobs.ToString("D");
        //    GridView1.FooterRow.Cells[8].Text = pendingjobs.ToString("D");
        //    GridView1.FooterRow.Cells[9].Text = returnedjobs.ToString("D");
        //    GridView1.FooterRow.Cells[10].Text = rejectedjobs.ToString("D");
        //    GridView1.FooterRow.Cells[11].Text = approvedjobs.ToString("D");
        //    GridView1.FooterRow.Cells[12].Text = manpowerjobs.ToString("D");
        //    GridView1.FooterRow.Cells[13].Text = lineitemjobs.ToString("D");
        //    GridView1.FooterRow.Cells[14].Text = unassignedjobs.ToString("D");
        //    GridView1.FooterRow.Cells[15].Text = nonbillingjobs.ToString("D");
        //    //GridView1.FooterRow.Cells[16].Text = pendingmeno.ToString("D");
        //    //GridView1.FooterRow.Cells[17].Text = memocreated.ToString("D");

        //    dbcl.Conn.Close();
        //}

        private void BindGridView1(string query)
        {
            try
            {
                dbcl.Sqlconnection();
                using (SqlConnection conn = dbcl.Conn)
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandTimeout = 100;

                        using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            ad.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                BindGridViewData(dt);
                            }
                            else
                            {
                                // No data to display
                                GridView1.DataSource = null;
                                GridView1.DataBind();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                // You can replace Console.WriteLine with an appropriate logging mechanism
                //Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private void BindGridViewData(DataTable dt)
        {
            GridView1.DataSource = dt;
            GridView1.DataBind();

            SetFooterValues(dt);
        }

        private void SetFooterValues(DataTable dt)
        {
            GridViewRow footerRow = GridView1.FooterRow;

            if (footerRow != null)
            {
                footerRow.Cells[0].Text = "Total";
                footerRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                footerRow.Cells[2].Text = dt.AsEnumerable().Sum(row => row.Field<int>("total_count")).ToString("D");
                footerRow.Cells[3].Text = dt.AsEnumerable().Sum(row => row.Field<int>("total_manpower")).ToString("D");
                footerRow.Cells[4].Text = dt.AsEnumerable().Sum(row => row.Field<decimal>("total_overtime")).ToString("F");
                footerRow.Cells[5].Text = dt.AsEnumerable().Sum(row => row.Field<int>("Idlejob_count")).ToString("D");
                footerRow.Cells[6].Text = dt.AsEnumerable().Sum(row => row.Field<int>("openjobs_count")).ToString("D");
                footerRow.Cells[7].Text = dt.AsEnumerable().Sum(row => row.Field<int>("closedjobs_count")).ToString("D");
                footerRow.Cells[8].Text = dt.AsEnumerable().Sum(row => row.Field<int>("pending_count")).ToString("D");
                footerRow.Cells[9].Text = dt.AsEnumerable().Sum(row => row.Field<int>("returned_count")).ToString("D");
                footerRow.Cells[10].Text = dt.AsEnumerable().Sum(row => row.Field<int>("rejected_count")).ToString("D");
                footerRow.Cells[11].Text = dt.AsEnumerable().Sum(row => row.Field<int>("approved_count")).ToString("D");
                footerRow.Cells[12].Text = dt.AsEnumerable().Sum(row => row.Field<int>("manpowerjobs")).ToString("D");
                footerRow.Cells[13].Text = dt.AsEnumerable().Sum(row => row.Field<int>("lineitemjobs")).ToString("D");
                footerRow.Cells[14].Text = dt.AsEnumerable().Sum(row => row.Field<int>("notassigned")).ToString("D");
                footerRow.Cells[15].Text = dt.AsEnumerable().Sum(row => row.Field<int>("nonbillingjobs")).ToString("D");
                //footerRow.Cells[16].Text = dt.AsEnumerable().Sum(row => row.Field<int>("pendingmemo")).ToString("D");
                //footerRow.Cells[17].Text = dt.AsEnumerable().Sum(row => row.Field<int>("createdmemo")).ToString("D");
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string date = Convert.ToString(e.CommandArgument);

            if (e.CommandName == "View_Details")
            {
                Response.Write("<script>window.open ('view_dailyjobs.aspx?Date=" + date + "','_parent');</script>");
            }
        }
    }
}