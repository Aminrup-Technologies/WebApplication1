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
    public partial class view_dailyjobs : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
        Boolean flag = false;

        public static string state = string.Empty;
        public static string region = string.Empty;
        public static string comp = string.Empty;
        public static string datalock = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    flag = false;

                    if (Session["Changer"] != null)
                    {
                        string[] retrievedArray = (string[])Session["Changer"];
                        region = retrievedArray[1].ToString();
                        comp = retrievedArray[2].ToString();
                        state = retrievedArray[0].ToString();
                        datalock = retrievedArray[3].ToString();
                        //Session["Changer"]= null;
                    }
                    else
                    {
                        region = Session["REGION"].ToString();
                        comp = Session["COMPANY_CODE"].ToString();
                        state = Session["STATE"].ToString();
                        datalock = "0";
                    }

                    string Date = Request.QueryString["Date"];
                    if (Date != null)
                    {
                        flag = true;
                        Session["DATE"] = Date;
                        dbcl.CalDateCombo1(DDL_Day, DDL_Month, DDL_Year);
                        GridBinder();
                    }
                    else
                    {
                        dbcl.CalDateCombo1(DDL_Day, DDL_Month, DDL_Year);
                        GridBinder();
                    }

                    string CmdString2 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = 'IN' and State_Code='" + state + "' order by Id ";
                    BindRegions(CmdString2);

                    WorksiteBinder();

                    //if (Session["USTATE"].ToString() == "PI")
                    //{
                    //    rgnrow1.Visible = true;
                    //    rgnrow2.Visible = true;

                    //    comprow1.Visible = true;
                    //    comprow2.Visible = true;
                    //    string CmdString2 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = 'IN' order by Id ";
                    //    BindRegions(CmdString2);
                    //}
                    //else
                    //{
                    //    WorksiteBinder();
                    //}
                }
            }
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
            string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and Work_Region_Code = '" + DDL_Region.SelectedValue.ToString() + "' order by Id ";
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

        protected void GridBinder()
        {
            string DD = "";
            string MM = "";
            string YYYY = "";

            if (flag == false)
            {
                DD = DDL_Day.SelectedItem.Text.ToString();
                MM = DDL_Month.SelectedItem.Text.ToString();
                YYYY = DDL_Year.SelectedItem.Text.ToString();
            }
            else
            {
                DateTime dt = Convert.ToDateTime(Session["DATE"].ToString());
                DD = dt.Date.ToString("dd");
                MM = dt.Month.ToString("00");
                YYYY = dt.Year.ToString();

                DDL_Day.SelectedItem.Text = DD;
                DDL_Month.SelectedItem.Text = MM;
                DDL_Year.SelectedItem.Text = YYYY;
            }

            DateTime d = DateTime.Now;
            string month = d.Month.ToString();
            string year = d.Year.ToString();
            string day = d.Day.ToString();
            //string region = Session["REGION"].ToString();

            string query = "select * from tbl_jobs a where YEAR(CreatedDate)='" + YYYY + "' and MONTH(CreatedDate)='" + MM + "' and DAY(CreatedDate)='" + DD + "' and JOB_Region='" + region + "' order by Id desc";
            BindGridView1(query);
        }

        private void BindGridView1(string query)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(query, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.CommandTimeout = 0;
            DataTable dt = new DataTable();
            ad.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
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

                Label lbl_Creator_Name = (Label)GridView1.Rows[i].FindControl("lbl_Creator_Name");
                Label lbl_EntryExit = (Label)GridView1.Rows[i].FindControl("lbl_EntryExit");

                string jobidstatus = lbl_JOBID_Status.Text.ToString();
                string approvalstatus = lbl_Incharge_Approval.Text.ToString();

                string prmtno = lbl_JOB_PermitNo.Text.ToString();
                string lblupldstatus = lbl_FinalUpldStatus.Text.ToString();

                string supvname = lbl_Creator_Name.Text.ToString();
                string entryexit = lbl_EntryExit.Text.ToString();


                if (approvalstatus == "Approved")
                {
                    lbl_Incharge_Approval.ForeColor = System.Drawing.Color.Green;
                    lbl_JOB_InchargeName.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lbl_Incharge_Approval.ForeColor = System.Drawing.Color.Red;
                    lbl_JOB_InchargeName.ForeColor = System.Drawing.Color.Red;
                }

                if (lblupldstatus == "Yes")
                {
                    lbl_JOB_PermitNo.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lbl_JOB_PermitNo.ForeColor = System.Drawing.Color.Red;
                }

                if (entryexit == "Exit")
                {
                    lbl_Creator_Name.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lbl_Creator_Name.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //string jobid = Convert.ToString(e.CommandArgument);

            //Determine the RowIndex of the Row whose Button was clicked.
            int rowIndex = Convert.ToInt32(e.CommandArgument);

            //Reference the GridView Row.
            GridViewRow row = GridView1.Rows[rowIndex];

            //Fetch value of Name.
            string dbid = (row.FindControl("lbl_Id") as Label).Text;
            string jobid = (row.FindControl("lbl_JOBID") as Label).Text;
            string supv = (row.FindControl("lbl_Creator_Workman") as Label).Text;

            if (e.CommandName == "Swap_JOBIDStatus")
            {
                if (CC.CheckforPendingOUT(jobid, supv) == 0)
                {
                    if (CC.CheckforPendingPermit(jobid, supv) == 0)
                    {
                        JOBID_Status_Swaper(jobid, dbid);
                        Response.Redirect(Request.Url.AbsoluteUri);
                    }
                    else
                    {
                        string title = "Notifications :";
                        string body = "Permit NOT Uploaded, Kindly Upload.";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Their are Pending OUT Punch, Kindly Punch OUT First.";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
            }
            else if (e.CommandName == "View_Details")
            {
                Response.Write("<script>window.open('view_jobdetails.aspx?JOBID=" + jobid + "&dbid=" + dbid + "&supv=" + supv + "', '_blank');</script>");

                //Response.Write("<script>window.open ('view_jobdetails.aspx?JOBID=" + jobid + "','_blank');</script>");

                //Response.Redirect("view_jobdetails.aspx?JOBID=" + jobid + "&dbid=" + dbid + "&supv=" + supv, false);
                //Response.Redirect("view_jobdetails.aspx?JOBID=" + jobid + "");
            }
            else if (e.CommandName == "Delete")
            {
                JOBID_Delete(dbid, jobid);
            }
        }

        private void JOBID_Status_Swaper(string jobid, string dbid)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string statusdata = "";
            string statusdata1 = "";
            string cmdstring = "select JOBID_Status from tbl_jobs where JOBID='" + jobid + "' and Id = '" + dbid + "'";
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            SqlDataReader re = cmd.ExecuteReader();
            if (re.Read())
            {
                statusdata = re["JOBID_Status"].ToString();
            }


            if (statusdata == "Active")
            {
                statusdata1 = "Blocked";

                dbcl.executeRdr("update tbl_jobs set JOBID_Status ='" + statusdata1 + "' where JOBID='" + jobid + "' and Id = '" + dbid + "'");
            }
            else
            {
                statusdata1 = "Active";
                dbcl.executeRdr("update tbl_jobs set JOBID_Status ='" + statusdata1 + "' where JOBID='" + jobid + "' and Id = '" + dbid + "'");
            }
            dbcl.Conn.Close();

            GridBinder();
        }

        protected void JOBID_Delete(string id, string dbcode)
        {
            try
            {
                Delete_from_JOBTable(id, dbcode);
                Delete_from_AttendanceTable(dbcode);
                Delete_from_PermitTable(dbcode);


                string title = "Notifications :";
                string body = "Data Deleted Successfully";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }

            GridBinder();
            //Response.Redirect(Request.Url.AbsoluteUri);
        }


        private void Delete_from_JOBTable(string id, string dbcode)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "delete from tbl_jobs where Id='" + id + "' and JOBID='" + dbcode + "'  ";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                dbcl.Conn.Close();
            }
            catch (Exception ex)
            {

                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void Delete_from_PermitTable(string dbcode)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "delete from tbl_jobspermit where JOBID='" + dbcode + "'  ";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                dbcl.Conn.Close();
            }
            catch (Exception ex)
            {

                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void Delete_from_AttendanceTable(string dbcode)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "delete from tbl_attendance where JOBID='" + dbcode + "'  ";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                dbcl.Conn.Close();
            }
            catch (Exception ex)
            {

                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void WorksiteBinder()
        {
            if (Session["USTATE"].ToString() == "PI")
            {
                string CmdString2 = "select Worksite_Name, DB_Code from tlb_atsworksites where WorkRegion_Code='" + DDL_Region.SelectedValue.ToString() + "' and Company_Code = '" + DDL_Company.SelectedValue.ToString() + "' order by Id";
                BindWorkSites(CmdString2);
            }
            else
            {
                string CmdString2 = "select Worksite_Name, DB_Code from tlb_atsworksites where WorkRegion_Code='" + region + "' and Company_Code = '" + comp + "' order by Id";
                BindWorkSites(CmdString2);
            }
        }

        private void BindWorkSites(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Worksite.DataSource = Cmd.ExecuteReader();
            DDL_Worksite.DataTextField = "Worksite_Name";
            DDL_Worksite.DataValueField = "DB_Code";
            DDL_Worksite.DataBind();
            DDL_Worksite.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string month = DDL_Month.SelectedItem.Text.ToString();
            string year = DDL_Year.SelectedItem.Text.ToString();
            string day = DDL_Day.SelectedItem.Text.ToString();
            //string region = Session["REGION"].ToString();
            if (Session["USTATE"].ToString() == "PI")
            {
                if (DDL_Region.SelectedIndex!=0)
                {
                    if (DDL_Company.SelectedIndex!=0)
                    {
                        //region = DDL_Region.SelectedValue.ToString();
                        if (DDL_Worksite.SelectedIndex == 0)
                        {
                            string query = "select * from tbl_jobs a where YEAR(CreatedDate)='" + year + "' and MONTH(CreatedDate)='" + month + "' and DAY(CreatedDate)='" + day + "' and JOB_Region='" + region + "' and JOB_Company='"+DDL_Company.SelectedValue.ToString()+"' order by Id desc";
                            BindGridView1(query);
                        }
                        else
                        {
                            string query = "select * from tbl_jobs a where YEAR(CreatedDate)='" + year + "' and MONTH(CreatedDate)='" + month + "' and DAY(CreatedDate)='" + day + "' and JOB_Region='" + region + "' and JOB_SiteCode='" + DDL_Worksite.SelectedValue.ToString() + "' and JOB_Company='" + DDL_Company.SelectedValue.ToString() + "' order by Id desc";
                            BindGridView1(query);
                        }
                    }
                }
            }
            else
            {
                if (DDL_Worksite.SelectedIndex == 0)
                {
                    string query = "select * from tbl_jobs a where YEAR(CreatedDate)='" + year + "' and MONTH(CreatedDate)='" + month + "' and DAY(CreatedDate)='" + day + "' and JOB_Region='" + region + "' order by Id desc";
                    BindGridView1(query);
                }
                else
                {
                    string query = "select * from tbl_jobs a where YEAR(CreatedDate)='" + year + "' and MONTH(CreatedDate)='" + month + "' and DAY(CreatedDate)='" + day + "' and JOB_Region='" + region + "' and JOB_SiteCode='" + DDL_Worksite.SelectedValue.ToString() + "' order by Id desc";
                    BindGridView1(query);
                }
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            Label JOBID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_JOBID");
            string dbjobid = JOBID.Text.ToString();

            try
            {
                JOBID_Delete(id, dbjobid);
                string title = "Notifications :";
                string body = "Data has been DELETED !!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void DDL_Company_SelectedIndexChanged(object sender, EventArgs e)
        {
            WorksiteBinder();
        }
    }
}