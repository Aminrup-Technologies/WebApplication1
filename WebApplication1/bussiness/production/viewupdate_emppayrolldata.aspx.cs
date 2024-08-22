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
using System.Drawing;

namespace WebApplication1.bussiness.production
{
    public partial class viewupdate_emppayrolldata : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        public static string Query = string.Empty;
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
                    Response.Redirect("login.aspx");
                }
                else
                {
                    if (Session["WORKMAN"].ToString() == "J8")
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

                        string CmdString2 = "select Category_Type, Category_DB from tlb_payroll_category where Country_Code = 'IN' and State_Code='" + state + "' and WorkRegion_Code='" + region + "' and Company_Code='" + comp + "' order by Id desc";
                        BindSkillCategory(CmdString2);

                        string CmdString3 = "select Id, WorkmanSL, FullName, SkillCategory,SkillDesignation,DOJ,SafetyPassNo, F16_YesNo,F17_YesNo, FixedSalary_YesNo, FixedAmount,WorkHours, OTFactor, OTMultiplier, OT_Divisibility, DA_VDA, HRA, Conv_Allowance, Medical_Allowance, ATT_Allowance, SPCL_Allowance,Misc_Earnings,Washing_Allowance from tbl_Employee_Mustertable where WorkRegion = '" + region + "' and WorkCompany='" + comp + "' and WorkStatus='Active' and F16_YesNo='Yes' and F17_YesNo='Yes' order by Id desc";
                        BindGrid(CmdString3);

                        DDL_EmpWorkStatus.SelectedIndex = 2;
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                }
            }
        }

        //------------- Added on 15.02.2023 for excel export of the data displayed on screen ----------------------------//
        protected void ExportExcel(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select Id, WorkmanSL, FullName, SkillCategory,SkillDesignation,DOJ,SafetyPassNo, FixedSalary_YesNo, FixedAmount,WorkHours, OTFactor, OTMultiplier, OT_Divisibility, DA_VDA, HRA, Conv_Allowance, Medical_Allowance, ATT_Allowance, SPCL_Allowance,Misc_Earnings,Washing_Allowance from tbl_Employee_Mustertable where WorkRegion = '" + region + "' and WorkCompany='" + comp + "' and WorkStatus='Active' and F16_YesNo='Yes' and F17_YesNo='Yes' order by Id"))
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
                                wb.Worksheets.Add(dt, "PayrollFactors");

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=EmpPayrolExport.xlsx");
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

        private void BindSkillCategory(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_SkillCategory.DataSource = Cmd.ExecuteReader();
            DDL_SkillCategory.DataTextField = "Category_Type";
            DDL_SkillCategory.DataValueField = "Category_DB";
            DDL_SkillCategory.DataBind();
            DDL_SkillCategory.Items.Insert(0, "Please Select Option");
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

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            Binder();
        }

        protected void Binder()
        {
            if (DDL_EmpWorkStatus.SelectedIndex == 0)
            {
                DDL_EmpWorkStatus.Focus();
            }
            else
            {
                if (DDL_SkillCategory.SelectedIndex == 0 && DDL_Form17YesNo.SelectedIndex == 0 && DDL_FixedYesNo.SelectedIndex == 0)
                {
                    string CmdString3 = "select Id, WorkmanSL, FullName, SkillCategory, SkillDesignation, DOJ,SafetyPassNo, F16_YesNo,F17_YesNo, FixedSalary_YesNo, FixedAmount,WorkHours, OTFactor, OTMultiplier, OT_Divisibility, DA_VDA, HRA, Conv_Allowance, Medical_Allowance, ATT_Allowance, SPCL_Allowance,Misc_Earnings,Washing_Allowance from tbl_Employee_Mustertable where WorkRegion = '" + region + "' and WorkCompany='" + comp + "' and WorkStatus='" + DDL_EmpWorkStatus.SelectedItem.Text.ToString() + "' and F16_YesNo='Yes' and F17_YesNo='Yes' order by Id desc";
                    BindGrid(CmdString3);
                }
                else if (DDL_SkillCategory.SelectedIndex != 0 && DDL_Form17YesNo.SelectedIndex == 0 && DDL_FixedYesNo.SelectedIndex == 0)
                {
                    string CmdString3 = "select Id, WorkmanSL, FullName, SkillCategory, SkillDesignation,DOJ,SafetyPassNo, F16_YesNo,F17_YesNo, FixedSalary_YesNo, FixedAmount,WorkHours, OTFactor, OTMultiplier, OT_Divisibility, DA_VDA, HRA, Conv_Allowance, Medical_Allowance, ATT_Allowance, SPCL_Allowance,Misc_Earnings,Washing_Allowance from tbl_Employee_Mustertable where WorkRegion = '" + region + "' and WorkCompany='" + comp + "' and WorkStatus='" + DDL_EmpWorkStatus.SelectedItem.Text.ToString() + "' and SkillCategoryDB='" + DDL_SkillCategory.SelectedValue.ToString() + "' and F16_YesNo='Yes' and F17_YesNo='Yes' order by Id desc";
                    BindGrid(CmdString3);
                }
                else if (DDL_SkillCategory.SelectedIndex != 0 && DDL_Form17YesNo.SelectedIndex != 0 && DDL_FixedYesNo.SelectedIndex == 0)
                {
                    string CmdString3 = "select Id, WorkmanSL, FullName, SkillCategory, SkillDesignation,DOJ,SafetyPassNo, F16_YesNo,F17_YesNo, FixedSalary_YesNo, FixedAmount,WorkHours, OTFactor, OTMultiplier, OT_Divisibility, DA_VDA, HRA, Conv_Allowance, Medical_Allowance, ATT_Allowance, SPCL_Allowance,Misc_Earnings,Washing_Allowance from tbl_Employee_Mustertable where WorkRegion = '" + region + "' and WorkCompany='" + comp + "' and WorkStatus='" + DDL_EmpWorkStatus.SelectedItem.Text.ToString() + "' and SkillCategoryDB='" + DDL_SkillCategory.SelectedValue.ToString() + "' and F17_YesNo='" + DDL_Form17YesNo.SelectedItem.Text.ToString() + "' order by Id desc";
                    BindGrid(CmdString3);
                }
                else if (DDL_SkillCategory.SelectedIndex != 0 && DDL_Form17YesNo.SelectedIndex != 0 && DDL_FixedYesNo.SelectedIndex != 0)
                {
                    string CmdString3 = "select Id, WorkmanSL, FullName, SkillCategory, SkillDesignation,DOJ,SafetyPassNo, F16_YesNo,F17_YesNo, FixedSalary_YesNo, FixedAmount,WorkHours, OTFactor, OTMultiplier, OT_Divisibility, DA_VDA, HRA, Conv_Allowance, Medical_Allowance, ATT_Allowance, SPCL_Allowance,Misc_Earnings,Washing_Allowance from tbl_Employee_Mustertable where WorkRegion = '" + region + "' and WorkCompany='" + comp + "' and WorkStatus='" + DDL_EmpWorkStatus.SelectedItem.Text.ToString() + "' and SkillCategoryDB='" + DDL_SkillCategory.SelectedValue.ToString() + "' and F17_YesNo='" + DDL_Form17YesNo.SelectedItem.Text.ToString() + "' and FixedSalary_YesNo = '" + DDL_FixedYesNo.SelectedItem.Text.ToString() + "' order by Id desc";
                    BindGrid(CmdString3);
                }

                else if (DDL_SkillCategory.SelectedIndex != 0 && DDL_Form17YesNo.SelectedIndex == 0 && DDL_FixedYesNo.SelectedIndex != 0)
                {
                    string CmdString3 = "select Id, WorkmanSL, FullName, SkillCategory, SkillDesignation,DOJ,SafetyPassNo, F16_YesNo,F17_YesNo, FixedSalary_YesNo, FixedAmount,WorkHours, OTFactor, OTMultiplier, OT_Divisibility, DA_VDA, HRA, Conv_Allowance, Medical_Allowance, ATT_Allowance, SPCL_Allowance,Misc_Earnings,Washing_Allowance from tbl_Employee_Mustertable where WorkRegion = '" + region + "' and WorkCompany='" + comp + "' and WorkStatus='" + DDL_EmpWorkStatus.SelectedItem.Text.ToString() + "' and SkillCategoryDB='" + DDL_SkillCategory.SelectedValue.ToString() + "' and FixedSalary_YesNo = '" + DDL_FixedYesNo.SelectedItem.Text.ToString() + "' and F16_YesNo='Yes' and F17_YesNo='Yes' order by Id desc";
                    BindGrid(CmdString3);
                }
            }
        }


        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            Binder();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            Binder();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Binder();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            Label WorkmanSL = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_WorkmanSL");
            string empwrk = WorkmanSL.Text.ToString();

            DropDownList DDL_F16_YesNo = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDL_F16_YesNo");
            string F16YesNo = DDL_F16_YesNo.SelectedItem.Text.ToString();

            DropDownList DDL_F17_YesNo = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDL_F17_YesNo");
            string F17YesNo = DDL_F17_YesNo.SelectedItem.Text.ToString();

            DropDownList DDL_FixedSalary_YesNo = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDL_FixedSalary_YesNo");
            string FixedYesNo = DDL_FixedSalary_YesNo.SelectedItem.Text.ToString();

            DropDownList DDL_WorkHours = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDL_WorkHours");
            string WorkHours = DDL_WorkHours.SelectedValue.ToString();

            DropDownList DDL_OTFactor = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDL_OTFactor");
            string OTFactor = DDL_OTFactor.SelectedValue.ToString();

            DropDownList DDL_OTMultiplier = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDL_OTMultiplier");
            string OTMult = DDL_OTMultiplier.SelectedItem.Text.ToString();

            DropDownList DDL_OT_Divisibility = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDL_OT_Divisibility");
            string OTDiv = DDL_OT_Divisibility.SelectedValue.ToString();

            TextBox wrkhrs = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_FixedAmount");
            decimal fixedamount = Convert.ToDecimal(wrkhrs.Text.ToString());

            //------------------ Added on 28-09-2021----------------------//

            TextBox davda = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_DA_VDA");
            decimal nw_davda = Convert.ToDecimal(davda.Text.ToString());

            TextBox hra = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_HRA");
            decimal nw_hra = Convert.ToDecimal(hra.Text.ToString());

            TextBox conv = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_Conv_Allowance");
            decimal nw_conv = Convert.ToDecimal(conv.Text.ToString());

            TextBox medi = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_Medical_Allowance");
            decimal nw_medi = Convert.ToDecimal(medi.Text.ToString());

            TextBox att = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_ATT_Allowance");
            decimal nw_att = Convert.ToDecimal(att.Text.ToString());

            TextBox spcl = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_SPCL_Allowance");
            decimal nw_spcl = Convert.ToDecimal(spcl.Text.ToString());

            TextBox misc = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_Misc_Earnings");
            decimal nw_misc = Convert.ToDecimal(misc.Text.ToString());

            TextBox wash = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txt_Washing_Allowance");
            decimal nw_wash = Convert.ToDecimal(wash.Text.ToString());

            UpdatePayrollData(id, empwrk, WorkHours, OTFactor, F16YesNo, F17YesNo, FixedYesNo, fixedamount, OTMult, OTDiv, nw_davda, nw_hra, nw_conv, nw_medi, nw_att, nw_spcl, nw_misc, nw_wash);

            GridView1.EditIndex = -1;
            Binder();

            string title = "Notifications :";
            string body = "Data has been UPDATED !!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

        }

        private void UpdatePayrollData(string dbid, string empwrk, string workhours, string otfactor, string f16yn, string f17yn, string fxdyno, decimal fxdamnt, string otrt, string OTDiv, decimal nw_davda, decimal nw_hra, decimal nw_conv, decimal nw_medi, decimal nw_att, decimal nw_spcl, decimal nw_misc, decimal nw_wash)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_Employee_Mustertable set WorkHours=@WorkHours, OTFactor=@OTFactor, F16_YesNo=@F16_YesNo , F17_YesNo=@F17_YesNo, FixedSalary_YesNo=@FixedSalary_YesNo, FixedAmount=@FixedAmount,  OTMultiplier=@OTMultiplier,OT_Divisibility=@OT_Divisibility,DA_VDA=@DA_VDA, HRA=@HRA, Conv_Allowance=@Conv_Allowance, ATT_Allowance=@ATT_Allowance, SPCL_Allowance=@SPCL_Allowance, Misc_Earnings=@Misc_Earnings, Washing_Allowance=@Washing_Allowance, LastModifiedDate=@LastModifiedDate, LastModifiedByWrk=@LastModifiedByWrk, LastModifiedByName=@LastModifiedByName where Id=@Id and WorkmanSL=@WorkmanSL";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", dbid);
                cmd.Parameters.AddWithValue("@WorkmanSL", empwrk);
                cmd.Parameters.AddWithValue("@WorkHours", workhours);

                cmd.Parameters.AddWithValue("@OTFactor", otfactor);
                cmd.Parameters.AddWithValue("@F16_YesNo", f16yn);
                cmd.Parameters.AddWithValue("@F17_YesNo", f17yn);
                cmd.Parameters.AddWithValue("@FixedSalary_YesNo", fxdyno);
                cmd.Parameters.AddWithValue("@FixedAmount", fxdamnt);
                cmd.Parameters.AddWithValue("@OTMultiplier", otrt);
                cmd.Parameters.AddWithValue("@OT_Divisibility", OTDiv);

                cmd.Parameters.AddWithValue("@DA_VDA", nw_davda);
                cmd.Parameters.AddWithValue("@HRA", nw_hra);
                cmd.Parameters.AddWithValue("@Conv_Allowance", nw_conv);
                cmd.Parameters.AddWithValue("@Medical_Allowance", nw_medi);
                cmd.Parameters.AddWithValue("@ATT_Allowance", nw_att);
                cmd.Parameters.AddWithValue("@SPCL_Allowance", nw_spcl);
                cmd.Parameters.AddWithValue("@Misc_Earnings", nw_misc);
                cmd.Parameters.AddWithValue("@Washing_Allowance", nw_wash);

                cmd.Parameters.AddWithValue("@LastModifiedDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@LastModifiedByWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@LastModifiedByName", Session["USERNAME"].ToString());
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
        }
    }
}