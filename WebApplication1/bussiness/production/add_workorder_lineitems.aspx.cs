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
    public partial class workorder_line_litems : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region order by Id";
                BindWorkRegion(CmdString1);

                DDL_WorkRegion.SelectedValue = Session["REGION"].ToString();

                string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Work_Region_Code='" + Session["REGION"].ToString() + "' order by Id";
                BindCompany(CmdString3);

                DDL_Company.SelectedValue = Session["COMPANY_CODE"].ToString();

                string CmdString4 = "select Company_Department, DB_Code from tlb_workregion_compdept where Work_Region_Code='" + Session["REGION"].ToString() + "' and Company_Code='" + Session["COMPANY_CODE"].ToString() + "' order by Id";
                BindDepartments(CmdString4);
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

        protected void DDL_WorkRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL_String = DDL_WorkRegion.SelectedItem.Text.ToString();
            string DDL_Value = DDL_WorkRegion.SelectedValue.ToString();

            string CmdString2 = "select Company_Name, Company_Code from tlb_workregion_company where Work_Region_Code='" + DDL_Value + "' order by Id";
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
            Int32 flagvalue = Inser_WorkorderLineItems();

            string DDL_RegionName = DDL_WorkRegion.SelectedItem.Text.ToString();
            string DDL_RegionValue = DDL_WorkRegion.SelectedValue.ToString();

            string DDL_CompanyName = DDL_Company.SelectedItem.Text.ToString();
            string DDL_CompanyValue = DDL_Company.SelectedValue.ToString();

            string DDL_WODNo = DDL_Workorder.SelectedItem.Text.ToString();
            string DDL_WODID = DDL_Workorder.SelectedValue.ToString();

            string DDL_WOIText = DDL_Workorder.SelectedItem.Text.ToString();
            string DDL_WOIValue = DDL_Workorder.SelectedValue.ToString();

            if (flagvalue != 0)
            {
                string CmdString2 = "select * from tlb_WO_LineItems_Data where Work_Region_Code='" + DDL_RegionValue + "' and Company_Code='" + DDL_CompanyValue + "' and WODB_Code = '"+ DDL_WODID + "' order by Id desc";
                BindGrid(CmdString2);

                txt_lineitemno.Text = string.Empty;
                txt_serialno.Text = string.Empty;
                txt_service_number.Text = string.Empty;
                txt_service_description.Text = string.Empty;
                txt_order_quantity.Text = string.Empty;
                txt_rate.Text = string.Empty;
                txt_perunit.Text = string.Empty;
            }
        }

        private string Find_DBCode()
        {
            string aa = null;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string kk = null;
            string cmdString1 = "select Id,WOI_DBCode from tlb_WO_LineItems_Data where Id=(select max(Id)from tlb_WO_LineItems_Data)";
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

        protected Int32 Inser_WorkorderLineItems()
        {
            int flag = 0;
            try
            {
                string DBCode = Find_DBCode();
                dbcl.Sqlconnection();
                SqlCommand cmd = new SqlCommand("SP_InsertInto_WOLineItemTable", dbcl.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Work_Region_Name", DDL_WorkRegion.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Work_Region_Code", DDL_WorkRegion.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Company_Name", DDL_Company.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Company_Code", DDL_Company.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Department_Name", DDL_Departments.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Department_Code", DDL_Departments.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WODB_Code", DDL_Workorder.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WO_Number", DDL_Workorder.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@WOI_DBCode", DBCode);
                cmd.Parameters.AddWithValue("@ItemNO", txt_lineitemno.Text.ToString());
                cmd.Parameters.AddWithValue("@LineNumber", txt_serialno.Text.ToString());
                cmd.Parameters.AddWithValue("@ServiceNumber", txt_service_number.Text.ToString());
                cmd.Parameters.AddWithValue("@Service_Description", txt_service_description.Text.ToString());
                cmd.Parameters.AddWithValue("@Order_Quantity", txt_order_quantity.Text.ToString());
                cmd.Parameters.AddWithValue("@Rate", Convert.ToDecimal(txt_rate.Text.ToString()));
                cmd.Parameters.AddWithValue("@PerUnit_Value", txt_perunit.Text.ToString());
                dbcl.ConnectDb();
                flag = cmd.ExecuteNonQuery();
                if (flag != 0)
                {
                    lbl_msg.Visible = true;
                    lbl_msg.Text = "Last Record Inserted Succesfully..!";
                    lbl_msg.ForeColor = System.Drawing.Color.DarkGreen;

                    string title = "Notifications :";
                    string body = "Record(s) Inserted into the Database";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                    dbcl.DisconnectDb();
                }
                else
                {
                    //lbl_msg.Visible = true;
                    //lbl_msg.Text = "Records Connot be Inserted into the Database";
                    //lbl_msg.ForeColor = System.Drawing.Color.IndianRed;
                    //dbcl.DisconnectDb();
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
            string DDL_String = DDL_WorkRegion.SelectedItem.Text.ToString();
            string DDL_Value = DDL_WorkRegion.SelectedValue.ToString();

            string DDL_CompText = DDL_Company.SelectedItem.Text.ToString();
            string DDL_CompValue = DDL_Company.SelectedValue.ToString();

            string CmdString2 = "select Company_Department, DB_Code from tlb_workregion_compdept where Work_Region_Code='" + DDL_Value + "' and Company_Code = '" + DDL_CompValue + "' order by Id";
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
            DDL_Departments.DataValueField = "DB_Code";
            DDL_Departments.DataBind();
            DDL_Departments.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_Departments_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL_String = DDL_WorkRegion.SelectedItem.Text.ToString();
            string DDL_Value = DDL_WorkRegion.SelectedValue.ToString();

            string DDL_CompText = DDL_Company.SelectedItem.Text.ToString();
            string DDL_CompValue = DDL_Company.SelectedValue.ToString();

            string DDL_DeptText = DDL_Departments.SelectedItem.Text.ToString();
            string DDL_DeptValue = DDL_Departments.SelectedValue.ToString();

            string CmdString2 = "select WO_Number, DB_Code from tlb_WO_Data where Work_Region_Code='" + DDL_Value + "' and Company_Code = '" + DDL_CompValue + "' and Dept_DBCode = '" + DDL_DeptValue+ "' and WO_Status='Active' order by Id";
            BindWorkorder(CmdString2);
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
            string DDL_String = DDL_WorkRegion.SelectedItem.Text.ToString();
            string DDL_Value = DDL_WorkRegion.SelectedValue.ToString();

            string DDL_CompText = DDL_Company.SelectedItem.Text.ToString();
            string DDL_CompValue = DDL_Company.SelectedValue.ToString();

            string DDL_DeptText = DDL_Departments.SelectedItem.Text.ToString();
            string DDL_DeptValue = DDL_Departments.SelectedValue.ToString();

            string DDL_WOText = DDL_Workorder.SelectedItem.Text.ToString();
            string DDL_WOValue = DDL_Workorder.SelectedValue.ToString();

            string CmdString2 = "select * from tlb_WO_LineItems_Data where Work_Region_Code='" + DDL_Value + "' and Company_Code = '" + DDL_CompValue + "' and Department_Code = '" + DDL_DeptValue + "' and WODB_Code='" + DDL_WOValue+"' order by Id";
            BindGrid(CmdString2);
        }
    }
}