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
    public partial class emp_registration : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
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
                    string CmdString1 = "select Country_Name, Country_Code from tlb_work_country";
                    BindCountry(CmdString1);
                }
            }
        }

        private void BindCountry(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_WorkCountry.DataSource = Cmd.ExecuteReader();
            DDL_WorkCountry.DataTextField = "Country_Name";
            DDL_WorkCountry.DataValueField = "Country_Code";
            DDL_WorkCountry.DataBind();
            DDL_WorkCountry.Items.Insert(0, "Please Select Option");
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
            //GridView1.DataSource = ds;
            //GridView1.DataBind();
            dbcl.Conn.Close();
        }

        protected void DDL_WorkCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL_Value = DDL_WorkCountry.SelectedValue.ToString();
            string CmdString3 = "select State_Name, State_Code from tlb_work_state where Country_Code = '"+ DDL_Value + "' order by Id";
            BindWorkState(CmdString3);
        }

        private void BindWorkState(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_WorkStates.DataSource = Cmd.ExecuteReader();
            DDL_WorkStates.DataTextField = "State_Name";
            DDL_WorkStates.DataValueField = "State_Code";
            DDL_WorkStates.DataBind();
            DDL_WorkStates.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_WorkStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL1_String = DDL_WorkCountry.SelectedValue.ToString();
            string DDL2_String = DDL_WorkStates.SelectedValue.ToString();

            string CmdString3 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = '"+ DDL1_String + "' and State_Code='"+ DDL2_String + "' order by Id";
            BindWorkRegion(CmdString3);
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

        protected void DDL_Work_Region_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL1_Value = DDL_WorkCountry.SelectedValue.ToString();
            string DDL2_Value = DDL_WorkStates.SelectedValue.ToString();
            string DDL3_Value = DDL_WorkRegion.SelectedValue.ToString();

            string CmdString2 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = '" + DDL1_Value + "' and State_Code='" + DDL2_Value + "' and Work_Region_Code='" + DDL3_Value + "' order by Id";
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

        protected void DDL_company_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            Binder1();
            Binder2();
            EducationType_Binder();
        }

        protected void Binder1()
        {
            string DDL1_Value = DDL_WorkCountry.SelectedValue.ToString();
            string DDL2_Value = DDL_WorkStates.SelectedValue.ToString();
            string DDL3_Value = DDL_WorkRegion.SelectedValue.ToString();
            string DDL4_Value = DDL_Company.SelectedValue.ToString();

            string CmdString2 = "select Category_Type, Category_DB from tlb_payroll_category where Country_Code = '" + DDL1_Value + "' and State_Code='" + DDL2_Value + "' and WorkRegion_Code='" + DDL3_Value + "' and Company_Code='" + DDL4_Value + "' order by Id";
            BindSkillCategory(CmdString2);
        }

        protected void Binder2()
        {
            string DDL1_Value = DDL_WorkCountry.SelectedValue.ToString();
            string DDL2_Value = DDL_WorkStates.SelectedValue.ToString();
            string DDL3_Value = DDL_WorkRegion.SelectedValue.ToString();
            string DDL4_Value = DDL_Company.SelectedValue.ToString();

            string CmdString2 = "select Worksite_Name, Worksite_Code from tlb_atsworksites where Country_Code = '" + DDL1_Value + "' and State_Code='" + DDL2_Value + "' and WorkRegion_Code='" + DDL3_Value + "' and Company_Code='" + DDL4_Value + "' order by Id";
            BindWorksites(CmdString2);
        }

        protected void EducationType_Binder()
        {
            dbcl.FillCombo(DDL_HighestEdu, "select Eduction_Type from tlb_education_types order by Id");
        }

        protected void Binder3()
        {
            string CmdString2 = "select Work_HoursTypes, Work_Hours from tlb_payroll_hours order by Id";
            BindWorkHours(CmdString2);
        }

        private void BindWorkHours(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_WorkHours.DataSource = Cmd.ExecuteReader();
            DDL_WorkHours.DataTextField = "Work_HoursTypes";
            DDL_WorkHours.DataValueField = "Work_Hours";
            DDL_WorkHours.DataBind();
            DDL_WorkHours.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void Binder4()
        {
            string CmdString2 = "select OT_Type, OT_TypeValue from tlb_payroll_ottypes order by Id";
            BindOTFactor(CmdString2);
        }

        private void BindOTFactor(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_OTFactor.DataSource = Cmd.ExecuteReader();
            DDL_OTFactor.DataTextField = "OT_Type";
            DDL_OTFactor.DataValueField = "OT_TypeValue";
            DDL_OTFactor.DataBind();
            DDL_OTFactor.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        private void Binder5()
        {
            string CmdString2 = "select Employee_Type, EmpType_Value from tlb_emp_roles order by Id";
            BindEmployeeType(CmdString2);
        }

        private void BindEmployeeType(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_EmployeeType.DataSource = Cmd.ExecuteReader();
            DDL_EmployeeType.DataTextField = "Employee_Type";
            DDL_EmployeeType.DataValueField = "EmpType_Value";
            DDL_EmployeeType.DataBind();
            DDL_EmployeeType.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
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

        private void BindWorksites(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Worksites.DataSource = Cmd.ExecuteReader();
            DDL_Worksites.DataTextField = "Worksite_Name";
            DDL_Worksites.DataValueField = "Worksite_Code";
            DDL_Worksites.DataBind();
            DDL_Worksites.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_SkillCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL1_Value = DDL_WorkCountry.SelectedValue.ToString();
            string DDL2_Value = DDL_WorkStates.SelectedValue.ToString();
            string DDL3_Value = DDL_WorkRegion.SelectedValue.ToString();
            string DDL4_Value = DDL_Company.SelectedValue.ToString();
            string DDL5_Value = DDL_SkillCategory.SelectedValue.ToString();

            string CmdString2 = "select Designation_Type, Designation_DB from tlb_payroll_designation where Country_Code = '" + DDL1_Value + "' and State_Code='" + DDL2_Value + "' and WorkRegion_Code='" + DDL3_Value + "' and Company_Code='" + DDL4_Value + "' and Category_DB =  '"+ DDL5_Value + "' order by Id";
            BindSkillDesignation(CmdString2);

            Binder3();
            Binder4();
            Binder5();
        }

        private void BindSkillDesignation(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_SkillDesignation.DataSource = Cmd.ExecuteReader();
            DDL_SkillDesignation.DataTextField = "Designation_Type";
            DDL_SkillDesignation.DataValueField = "Designation_DB";
            DDL_SkillDesignation.DataBind();
            DDL_SkillDesignation.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_EmployeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CmdString2 = "select Emp_PermissionText, Emp_PermissionValue from tlb_emp_roles_permission where EmpType_Value = '" + DDL_EmployeeType.SelectedValue.ToString()+ "' order by Id";
            Bind_EmpRole_Permissions(CmdString2);
        }

        private void Bind_EmpRole_Permissions(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_RolePermissions.DataSource = Cmd.ExecuteReader();
            DDL_RolePermissions.DataTextField = "Emp_PermissionText";
            DDL_RolePermissions.DataValueField = "Emp_PermissionValue";
            DDL_RolePermissions.DataBind();
            DDL_RolePermissions.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (Insert_EmplyeeMusterData() != 0)
            {
                btn_submit.Enabled = false;
                btn_submit.CssClass = "btn bt-sm btn-success";
            }
            else
            {
                btn_submit.Text = "Re-Try";
            }
        }

        private Int32 Insert_EmplyeeMusterData()
        {
            //Code to Generate Unique Employee ID Goes here
            string LoginID = "";
            dbcl.GenerateLoginID(ref LoginID);

            //Code to Generate Unique Login Password Goes here
            string LoginPassword = "";
            Int32 password_length = 6;
            dbcl.GenerateLoginPassword(password_length, ref LoginPassword);

            //Code to Insert values into the DB goes here
            int flag = 0;
            try
            {
                dbcl.Sqlconnection();
                SqlCommand cmd = new SqlCommand("SP_InsertInto_EmployeeMusterTable", dbcl.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WorkStatus", "Active");
                cmd.Parameters.AddWithValue("@LoginID", LoginID);
                cmd.Parameters.AddWithValue("@LoginPassword", LoginPassword);
                cmd.Parameters.AddWithValue("@WorkCountry", DDL_WorkCountry.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WorkState", DDL_WorkStates.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WorkRegion", DDL_WorkRegion.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WorkCompany", DDL_Company.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WorkmanSL", txt_workman.Text.TrimEnd().ToString());
                cmd.Parameters.AddWithValue("@FirstName", txt_empfname.Text.ToString());

                string mdname = txt_empmdname.Text.ToString();
                if (mdname == "#N/A" || mdname == " " || mdname == "#NA")
                {
                    cmd.Parameters.AddWithValue("@MiddleName", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MiddleName", mdname);
                }

                string lstname = txt_emplstname.Text.ToString();
                if (lstname == "#N/A" || lstname == " " || lstname == "#NA")
                {
                    cmd.Parameters.AddWithValue("@LastName", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@LastName", lstname);
                }
                cmd.Parameters.AddWithValue("@FullName", txt_fullanme.Text.ToString());
                cmd.Parameters.AddWithValue("@Fathername", txt_empfathername.Text.ToString());
                cmd.Parameters.AddWithValue("@BloodGroup", txt_bloodgroup.Text.ToString());
                cmd.Parameters.AddWithValue("@MobileNo", txt_Mobile_Number.Text.ToString());
                cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(txt_DOB.Text.ToString()));
                cmd.Parameters.AddWithValue("@Qualification", DDL_HighestEdu.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@DOJ", Convert.ToDateTime(txt_DOJ.Text.ToString()));
                //Date of Registration is auto
                cmd.Parameters.AddWithValue("@WorkSite", DDL_Worksites.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Worksite_Code", DDL_Worksites.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@SkillCategory", DDL_SkillCategory.SelectedItem.Text.ToString());
                //Skill Category DB is Auto Feed from Backend
                cmd.Parameters.AddWithValue("@SkillDesignation", DDL_SkillDesignation.SelectedItem.Text.ToString());
                //Skill designation DB Code is auto feed from the back end
                cmd.Parameters.AddWithValue("@User_RoleType", DDL_EmployeeType.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Role_Permission", DDL_RolePermissions.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@WorkHours", DDL_WorkHours.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@OTFactor", DDL_OTFactor.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@SafetyPassNo", txt_rfidno.Text.ToString());
                cmd.Parameters.AddWithValue("@SafetyPassExpiry", Convert.ToDateTime(txt_rfidvalidity.Text.ToString()));
                cmd.Parameters.AddWithValue("@GatePassNo", txt_gpno.Text.ToString());
                cmd.Parameters.AddWithValue("@GatePassExpiry", Convert.ToDateTime(txt_gpvalidity.Text.ToString()));
                cmd.Parameters.AddWithValue("@PVExpiry", Convert.ToDateTime(txt_pvvalidity.Text.ToString()));
                cmd.Parameters.AddWithValue("@UANNo", txt_uanno.Text.ToString());
                cmd.Parameters.AddWithValue("@ESICNo", txt_esicno.Text.ToString());
                cmd.Parameters.AddWithValue("@Payment_Bank", txt_banknanme.Text.ToString());
                cmd.Parameters.AddWithValue("@Payment_Account", txt_accountno.Text.ToString());
                cmd.Parameters.AddWithValue("@Payment_IFSC", txt_ifsccode.Text.ToString());
                cmd.Parameters.AddWithValue("@BankBranch", txt_bankbranch.Text.ToUpper().ToString());
                cmd.Parameters.AddWithValue("@RegistrationType", "Single");
                cmd.Parameters.AddWithValue("@Registered_ByName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@Registered_ByWRK", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@LoginStatus", 0);
                cmd.Parameters.AddWithValue("@LastLogin", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@LastLogout", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@PasswordExpiry", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                dbcl.ConnectDb();
                flag = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                lbl_msg.Visible = true;
                lbl_msg.Text = ex.Message;
                lbl_msg.ForeColor = System.Drawing.Color.IndianRed;
                dbcl.DisconnectDb();
            }

            if (flag != 0)
            {
                lbl_msg.Visible = true;
                lbl_msg.Text = "Record Inserted Successfully..!";
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
            return flag;
        }

        protected void txt_workman_TextChanged(object sender, EventArgs e)
        {
            string inputstring = txt_workman.Text;
            string cmdString = "select COUNT(Id) as count from tbl_Employee_Mustertable where WorkmanSL='" + inputstring + "'";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                Int32 count = Convert.ToInt32(Rdr["count"].ToString());

                if (count == 1)
                {
                    txt_workman.Text = "";
                    btn_submit.Enabled=false;
                    txt_workman.Focus();
                    string title = "ERROR :";
                    string body = "Workmen SL NOT AVAILABLE....!!!";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    txt_workman.ReadOnly = true;
                    btn_submit.Enabled = true;
                }
            }
            dbcl.Conn.Close();
        }
    }
}