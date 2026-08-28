using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using ClosedXML.Excel;
using System.Configuration;
using System.IO;

namespace WebApplication1.bussiness.production
{
    public partial class add_workorderdata : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        public static string state = string.Empty;
        public static string region = string.Empty;
        public static string comp = string.Empty;
        public static string datalock = string.Empty;

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

                    string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region order by Id";
                    BindWorkRegion(CmdString1);

                    DDL_WorkRegion.SelectedValue = region;

                    string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Work_Region_Code='" + region + "' order by Id";
                    BindCompany(CmdString3);

                    DDL_Company.SelectedValue =comp;

                    string CmdString4 = "select Company_Department, DB_Code from tlb_workregion_compdept where Work_Region_Code='" + region + "' and Company_Code='" + comp + "' order by Id";
                    BindDepartments(CmdString4);

                    string CmdString2 = "select * from tlb_WO_Details where Work_Region_Code='" + region + "' and Company_Code='" + comp + "' order by Id";
                    BindGrid(CmdString2);
                }
            }
        }

        private void BindWorkRegion(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_WorkRegion.DataSource = Cmd.ExecuteReader();
            DDL_WorkRegion.DataTextField = "Work_Region_Name";
            DDL_WorkRegion.DataValueField = "Work_Region_Code";
            DDL_WorkRegion.DataBind();
            DDL_WorkRegion.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        private void BindGrid(string cmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            dbcl.Conn.Close();
        }

        private void BindDepartments(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Departments.DataSource = Cmd.ExecuteReader();
            DDL_Departments.DataTextField = "Company_Department";
            DDL_Departments.DataValueField = "DB_Code";
            DDL_Departments.DataBind();
            DDL_Departments.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }


        protected void DDL_WorkRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL_Value = DDL_WorkRegion.SelectedValue.ToString();

            string CmdString2 = "select Company_Name, Company_Code from tlb_workregion_company where Work_Region_Code='"+ DDL_Value + "' order by Id";
            BindCompany(CmdString2);

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
            Int32 flagvalue = Inser_WorkorderDetails();

            if (flagvalue != 0)
            {
                GridBinder();
            }
        }

        private void GridBinder()
        {
            if (DDL_WorkRegion.SelectedIndex != 0 && DDL_Company.SelectedIndex != 0)
            {
                if (DDL_Departments.SelectedIndex != 0 && DDL_Workorder.SelectedIndex!=0)
                {
                    string CmdString2 = "select * from tlb_WO_Details where Work_Region_Code='" + DDL_WorkRegion.SelectedValue.ToString() + "' and Company_Code='" + DDL_Company.SelectedValue.ToString() + "' and Department_Code='" + DDL_Departments.SelectedValue.ToString() + "' and Ref_DBCode = '" + DDL_Workorder.SelectedValue.ToString() + "' order by Id";
                    BindGrid(CmdString2);
                }
                else if (DDL_Departments.SelectedIndex != 0 && DDL_Workorder.SelectedIndex == 0)
                {
                    string CmdString2 = "select * from tlb_WO_Details where Work_Region_Code='" + DDL_WorkRegion.SelectedValue.ToString() + "' and Company_Code='" + DDL_Company.SelectedValue.ToString() + "' and Department_Code='" + DDL_Departments.SelectedValue.ToString() + "' order by Id";
                    BindGrid(CmdString2);
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "Selection Required";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private string Find_DBCode()
        {
            string aa = null;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string kk = null;
            string cmdString1 = "select Id,DB_Code from tlb_WO_Details where Id=(select max(Id)from tlb_WO_Details)";
            SqlCommand com1 = new SqlCommand(cmdString1, dbcl.Conn);
            SqlDataReader DR1 = com1.ExecuteReader();
            if (DR1.Read())
            {
                aa = DR1.GetValue(1).ToString();
                string bb = aa.Substring(5);
                int k = Convert.ToInt32(bb);
                k = k + 1;
                string q = Convert.ToString(k);
                kk = "WOI00" + q;
            }
            else
            {
                kk = "WOI001";
            }
            dbcl.DisconnectDb();
            return kk;
        }

        protected Int32 Inser_WorkorderDetails()
        {
            int flag = 0;
            //string DBCode = Find_DBCode();
            try
            {
                dbcl.Sqlconnection();
                SqlCommand cmd = new SqlCommand("SP_Insert_WorkorderBasicDetails", dbcl.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Work_Region_Name", DDL_WorkRegion.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Work_Region_Code", DDL_WorkRegion.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Company_Name", DDL_Company.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Company_Code", DDL_Company.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Department_Name", DDL_Departments.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Department_Code", DDL_Departments.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Ref_DBCode", DDL_Workorder.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WO_Number", DDL_Workorder.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@WO_Description", txt_description.Text.ToString());
                cmd.Parameters.AddWithValue("@WO_Amount", Convert.ToDecimal(txt_oderamnt.Text.ToString()));
                cmd.Parameters.AddWithValue("@WO_BalAmount", Convert.ToDecimal(txt_balanceamnt.Text.ToString()));
                cmd.Parameters.AddWithValue("@WO_ValidFrom", txt_validform.Text.ToString());
                cmd.Parameters.AddWithValue("@WO_ValidTo", txt_validto.Text.ToString());
                cmd.Parameters.AddWithValue("@WO_BillingDue", txt_billingdue.Text.ToString());
                cmd.Parameters.AddWithValue("@WO_Status", RBTN_Status.SelectedItem.Text.ToString());
                dbcl.ConnectDb();
                flag = cmd.ExecuteNonQuery();

                if (flag != 0)
                {
                    lbl_msg.Visible = true;
                    lbl_msg.Text = "Record Inserted Successfully..!";
                    lbl_msg.ForeColor = System.Drawing.Color.DarkGreen;

                    string title = "Notifications :";
                    string body = "Records Inserted into the Database";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    dbcl.DisconnectDb();
                }
                else
                {
                    //lbl_msg.Visible = true;
                    //lbl_msg.Text = "Records Connot be Inserted into the Database";
                    //lbl_msg.ForeColor = System.Drawing.Color.IndianRed;
                    //dbcl.DisconnectDb();

                    string title = "Notifications :";
                    string body = "Records Connot be Inserted into the Database";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
            }
            catch (Exception ex)
            {
                lbl_msg.Visible = true;
                lbl_msg.Text = ex.Message;
                lbl_msg.ForeColor = System.Drawing.Color.IndianRed;

                string title = "Notifications :";
                string body = "Records Connot be Inserted into the Database";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                dbcl.DisconnectDb();
            }
            return flag;
        }

        protected void DDL_Company_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL_Value = DDL_WorkRegion.SelectedValue.ToString();

            string DDL_CompValue = DDL_Company.SelectedValue.ToString();

            string CmdString2 = "select Company_Department, CompDept_Code from tlb_workregion_compdept where Work_Region_Code='" + DDL_Value + "' and Company_Code = '"+DDL_CompValue+"' order by Id";
            BindDepartment(CmdString2);
        }

        private void BindDepartment(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Departments.DataSource = Cmd.ExecuteReader();
            DDL_Departments.DataTextField = "Company_Department";
            DDL_Departments.DataValueField = "CompDept_Code";
            DDL_Departments.DataBind();
            DDL_Departments.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_Departments_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL_Value = DDL_WorkRegion.SelectedValue.ToString();

            string DDL_CompValue = DDL_Company.SelectedValue.ToString();

            string DDL_DeptValue = DDL_Departments.SelectedValue.ToString();

            string CmdString2 = "select WO_Number, DB_Code from tlb_WO_Data where Work_Region_Code='" + DDL_Value + "' and Company_Code = '" + DDL_CompValue + "' and Dept_DBCode = '" + DDL_DeptValue+ "' and WO_Status='Active' order by Id";
            BindWorkorder(CmdString2);

            string CmdString3 = "select * from tlb_WO_Details where Work_Region_Code='" + DDL_WorkRegion.SelectedValue.ToString() + "' and Company_Code='" + DDL_Company.SelectedValue.ToString() + "' and Department_Code='" + DDL_Departments.SelectedValue.ToString() + "' order by Id";
            BindGrid(CmdString3);
        }

        private void BindWorkorder(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Workorder.DataSource = Cmd.ExecuteReader();
            DDL_Workorder.DataTextField = "WO_Number";
            DDL_Workorder.DataValueField = "DB_Code";
            DDL_Workorder.DataBind();
            DDL_Workorder.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_Workorder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_Workorder.SelectedIndex != 0)
            {
                string CmdString2 = "select * from tlb_WO_Details where Work_Region_Code='"+DDL_WorkRegion.SelectedValue.ToString()+ "' and Company_Code='"+DDL_Company.SelectedValue.ToString() + "' and Department_Code='"+DDL_Departments.SelectedValue.ToString() + "' and Ref_DBCode = '" + DDL_Workorder.SelectedValue.ToString()+"' order by Id";
                BindGrid(CmdString2);
            }
            else
            {

            }
        }
    }
}