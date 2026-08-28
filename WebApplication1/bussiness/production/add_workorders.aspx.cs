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
    public partial class add_workorders : System.Web.UI.Page
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
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
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


                    string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Work_Region_Code = '"+ region + "'  order by Id";
                    BindWorkRegion(CmdString1);

                    //DDL_WorkRegion.SelectedValue = Session["REGION"].ToString();

                    //DDL_WorkRegion.Enabled = false;

                    string CmdString2 = "select Company_Name, Company_Code from tlb_workregion_company where Work_Region_Code='" + region + "' order by Id";
                    BindCompany(CmdString2);

                    //DDL_Company.SelectedValue = Session["COMPANY_CODE"].ToString();
                    //DDL_Company.Enabled = false;

                    string CmdString3 = "select Company_Department, DB_Code from tlb_workregion_compdept where Work_Region_Code='" + region + "' and Company_Code='" + comp + "' order by Id";
                    BindDepartments(CmdString3);

                    string CmdString4 = "select * from tlb_WO_Data where Work_Region_Code= '"+ region + "' and Company_Code='"+ comp + "' and WO_Status='Active' order by Id";
                    BindGrid(CmdString4);
                }
            }
        }

        protected void ExportExcel(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select Work_Region_Code,Company_Code,Department_Name,WO_Type,WO_Number from tlb_WO_Data where Work_Region_Code= '" + Session["REGION"].ToString() + "' and Company_Code='"+ Session["COMPANY_CODE"].ToString() + "' and WO_Status='Active' order by Id"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dt, "Workorders");

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=Workorder.xlsx");
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }
                            }
                        }
                    }
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

        protected void DDL_WorkRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL_Value = DDL_WorkRegion.SelectedValue.ToString();

            string CmdString2 = "select Company_Name, Company_Code from tlb_workregion_company where Work_Region_Code='" + DDL_Value + "' order by Id";
            BindCompany(CmdString2);

            string CmdString4 = "select * from tlb_WO_Data where Work_Region_Code= '" + Session["REGION"].ToString() + "' and WO_Status='Active' order by Id";
            BindGrid(CmdString4);

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

        protected void DDL_Company_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL_RegionValue = DDL_WorkRegion.SelectedValue.ToString();

            string DDL_CompanyValue = DDL_Company.SelectedValue.ToString();

            string CmdString3 = "select Company_Department, DB_Code from tlb_workregion_compdept where Work_Region_Code='" + DDL_RegionValue + "' and Company_Code='" + DDL_CompanyValue + "' order by Id";
            BindDepartments(CmdString3);

            string CmdString4 = "select * from tlb_WO_Data where Work_Region_Code= '" + Session["REGION"].ToString() + "' and Company_Code='" + Session["COMPANY_CODE"].ToString() + "' and WO_Status='Active' order by Id";
            BindGrid(CmdString4);
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

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            Int32 flagvalue = Inser_WorkorderData();

            if (flagvalue != 0)
            {
                string CmdString2 = "select * from tlb_WO_Data order by Id";
                BindGrid(CmdString2);
            }
        }

        private string Find_DBCode()
        {
            string aa = null;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string kk = null;
            string cmdString1 = "select Id,DB_Code from tlb_WO_Data where Id=(select max(Id)from tlb_WO_Data)";
            SqlCommand com1 = new SqlCommand(cmdString1, dbcl.Conn);
            SqlDataReader DR1 = com1.ExecuteReader();
            if (DR1.Read())
            {
                aa = DR1.GetValue(1).ToString();
                string bb = aa.Substring(5);
                int k = Convert.ToInt32(bb);
                k = k + 1;
                string q = Convert.ToString(k);
                kk = "WOD00" + q;
            }
            else
            {
                kk = "WOD001";
            }
            dbcl.DisconnectDb();
            return kk;
        }

        protected Int32 Inser_WorkorderData()
        {
            int flag = 0;
            string DBCode = Find_DBCode();
            try
            {
                dbcl.Sqlconnection();
                SqlCommand cmd = new SqlCommand("SP_Insert_WorkorderData", dbcl.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Work_Region_Name", DDL_WorkRegion.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Work_Region_Code", DDL_WorkRegion.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Company_Name", DDL_Company.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Company_Code", DDL_Company.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Department_Name", DDL_Departments.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Dept_DBCode", DDL_Departments.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@DB_Code", DBCode);
                cmd.Parameters.AddWithValue("@WO_Type", txt_workordertype.Text.ToString());
                cmd.Parameters.AddWithValue("@WO_Number", txt_workorderno.Text.ToString());
                cmd.Parameters.AddWithValue("@WO_Status", DDL_Status.SelectedItem.Text.ToString());
                dbcl.ConnectDb();
                flag = cmd.ExecuteNonQuery();

                if (flag != 0)
                {
                    lbl_msg.Visible = true;
                    lbl_msg.Text = "Record Inserted Succesfully..!";
                    lbl_msg.ForeColor = System.Drawing.Color.DarkGreen;
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
                dbcl.DisconnectDb();
            }
            return flag;
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;

            GridBinder();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;

            GridBinder();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            Label DBCode = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_DB_Code");
            string dbcode = DBCode.Text.ToString();

            TextBox type = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_WO_Type");
            string new_type = type.Text.ToString();

            TextBox wonumber = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_WO_Number");
            string new_wonumber = wonumber.Text.ToString();

            DropDownList status = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDL_WO_Status");
            string new_status = status.SelectedItem.Text.ToString();

            DropDownList deptname = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDL_NewDept");
            string new_deptname = deptname.SelectedItem.Text.ToString();
            string new_deptcode = deptname.SelectedValue.ToString();

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "UPDATE tlb_WO_Data set Dept_DBCode=@Dept_DBCode, Department_Name=@Department_Name, WO_Type=@WO_Type, WO_Number=@WO_Number, WO_Status=@WO_Status where Id='" + id + "' and DB_Code='" + dbcode + "'  ";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Dept_DBCode", new_deptcode);
                cmd.Parameters.AddWithValue("@Department_Name", new_deptname);
                cmd.Parameters.AddWithValue("@WO_Type", new_type);
                cmd.Parameters.AddWithValue("@WO_Number", new_wonumber);
                cmd.Parameters.AddWithValue("@WO_Status", new_status);

                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                dbcl.Conn.Close();

                string title = "Notifications :";
                string body = "Data Updated Successfully";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception)
            {

                throw;
            }

            GridView1.EditIndex = -1;
            GridBinder();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            Label DBCode = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_DB_Code");
            string dbcode = DBCode.Text.ToString();

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string cmdString = "delete from tlb_WO_Data where Id='" + id + "' and DB_Code='" + dbcode + "'  ";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                dbcl.Conn.Close();
            }
            catch (Exception)
            {

                throw;
            }

            GridBinder();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && GridView1.EditIndex == e.Row.RowIndex)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var DropDownList1 = e.Row.FindControl("DDL_NewDept") as DropDownList;
                    if (DropDownList1 != null)
                    {
                        var dt = new DataTable();
                        string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString();
                        using (var con = new SqlConnection(cnnString))
                        {
                            con.Open();
                            var cmd = new SqlCommand("Select Company_Department,DB_Code from tlb_workregion_compdept where Work_Region_Code= '" + Session["REGION"].ToString() + "' order by Id", con);
                            var da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                        }

                        DropDownList1.DataSource = dt;
                        DropDownList1.DataTextField = "Company_Department";
                        DropDownList1.DataValueField = "DB_Code";
                        DropDownList1.DataBind();
                        string selectedCity = DataBinder.Eval(e.Row.DataItem, "Department_Name").ToString();
                        DropDownList1.Items.FindByText(selectedCity).Selected = true;
                    }
                }
            }
        }

        protected void DDL_Departments_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CmdString4 = "select * from tlb_WO_Data where Work_Region_Code= '" + Session["REGION"].ToString() + "' and Company_Code='" + Session["COMPANY_CODE"].ToString() + "' and Dept_DBCode='" + DDL_Departments.SelectedValue.ToString()+ "' and WO_Status='Active' order by Id";
            BindGrid(CmdString4);
        }


        private void GridBinder()
        {
            if (DDL_WorkRegion.SelectedIndex !=0 && DDL_Company.SelectedIndex != 0 )
            {
                if (DDL_Departments.SelectedIndex!=0)
                {
                    string CmdString4 = "select * from tlb_WO_Data where Work_Region_Code= '" + Session["REGION"].ToString() + "' and Company_Code='" + Session["COMPANY_CODE"].ToString() + "' and Dept_DBCode='" + DDL_Departments.SelectedValue.ToString() + "' and WO_Status='Active' order by Id";
                    BindGrid(CmdString4);
                }
                else
                {
                    string CmdString4 = "select * from tlb_WO_Data where Work_Region_Code= '" + Session["REGION"].ToString() + "' and Company_Code='" + Session["COMPANY_CODE"].ToString() + "' and WO_Status='Active' order by Id";
                    BindGrid(CmdString4);
                }
            }
        }
    }
}