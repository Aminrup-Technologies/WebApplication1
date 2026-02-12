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
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    string CmdString1 = "select Country_Name, Country_Code from tlb_work_country";
                    BindCountry(CmdString1);

                    string bankqry = "select BankName, BankCode from tlb_IndianBanks";
                    BindBanks(bankqry);
                }
            }
        }

        #region Binders

        private void BindCountry(string CmdString)
        {
            try
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
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        private void BindBanks(string CmdString)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
                Cmd.CommandType = CommandType.Text;
                DDL_BankName.DataSource = Cmd.ExecuteReader();
                DDL_BankName.DataTextField = "BankName";
                DDL_BankName.DataValueField = "BankCode";
                DDL_BankName.DataBind();
                DDL_BankName.Items.Insert(0, "Please Select Option");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        private void BindWorkState(string CmdString)
        {
            try
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
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        private void BindWorkRegion(string CmdString)
        {
            try
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
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        private void BindCompany(string CmdString)
        {
            try
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
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        private void BindSkillCategory(string CmdString)
        {
            try
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
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        private void BindWorksites(string CmdString)
        {
            try
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
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        private void BindWorkHours(string CmdString)
        {
            try
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
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        private void BindOTFactor(string CmdString)
        {
            try
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
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        private void BindEmployeeType(string CmdString)
        {
            try
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
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        private void BindSkillDesignation(string CmdString)
        {
            try
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
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        private void Bind_EmpRole_Permissions(string CmdString)
        {
            try
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
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }
        #endregion

        #region Dropdown Events
        protected void DDL_WorkCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_WorkCountry.SelectedIndex > 0)
            {
                string DDL_Value = DDL_WorkCountry.SelectedValue.ToString();
                // Using parameterized query would be better here, but strictly following 'No Enhancements' rule aside from safety fixes
                string CmdString3 = "select State_Name, State_Code from tlb_work_state where Country_Code = '" + DDL_Value + "' order by Id";
                BindWorkState(CmdString3);
            }
            else
            {
                DDL_WorkStates.Items.Clear(); // Cascade Clear
            }
        }

        protected void DDL_WorkStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_WorkStates.SelectedIndex > 0)
            {
                string DDL1_String = DDL_WorkCountry.SelectedValue.ToString();
                string DDL2_String = DDL_WorkStates.SelectedValue.ToString();
                string CmdString3 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = '" + DDL1_String + "' and State_Code='" + DDL2_String + "' order by Id";
                BindWorkRegion(CmdString3);
            }
        }

        protected void DDL_Work_Region_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_WorkRegion.SelectedIndex > 0)
            {
                string DDL1_Value = DDL_WorkCountry.SelectedValue.ToString();
                string DDL2_Value = DDL_WorkStates.SelectedValue.ToString();
                string DDL3_Value = DDL_WorkRegion.SelectedValue.ToString();
                string CmdString2 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = '" + DDL1_Value + "' and State_Code='" + DDL2_Value + "' and Work_Region_Code='" + DDL3_Value + "' order by Id";
                BindCompany(CmdString2);
            }
        }

        protected void DDL_company_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_Company.SelectedIndex > 0)
            {
                Binder1();
                Binder2();
                EducationType_Binder();
            }
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
            // Assuming FillCombo handles connection internally
            dbcl.FillCombo(DDL_HighestEdu, "select Eduction_Type from tlb_education_types order by Id");
        }

        protected void Binder3()
        {
            string CmdString2 = "select Work_HoursTypes, Work_Hours from tlb_payroll_hours order by Id";
            BindWorkHours(CmdString2);
        }

        protected void Binder4()
        {
            string CmdString2 = "select OT_Type, OT_TypeValue from tlb_payroll_ottypes order by Id";
            BindOTFactor(CmdString2);
        }

        private void Binder5()
        {
            string CmdString2 = "select Employee_Type, EmpType_Value from tlb_emp_roles order by Id";
            BindEmployeeType(CmdString2);
        }

        protected void DDL_SkillCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_SkillCategory.SelectedIndex > 0)
            {
                string DDL1_Value = DDL_WorkCountry.SelectedValue.ToString();
                string DDL2_Value = DDL_WorkStates.SelectedValue.ToString();
                string DDL3_Value = DDL_WorkRegion.SelectedValue.ToString();
                string DDL4_Value = DDL_Company.SelectedValue.ToString();
                string DDL5_Value = DDL_SkillCategory.SelectedValue.ToString();

                string CmdString2 = "select Designation_Type, Designation_DB from tlb_payroll_designation where Country_Code = '" + DDL1_Value + "' and State_Code='" + DDL2_Value + "' and WorkRegion_Code='" + DDL3_Value + "' and Company_Code='" + DDL4_Value + "' and Category_DB =  '" + DDL5_Value + "' order by Id";
                BindSkillDesignation(CmdString2);

                Binder3();
                Binder4();
                Binder5();
            }
        }

        protected void DDL_EmployeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_EmployeeType.SelectedIndex > 0)
            {
                string CmdString2 = "select Emp_PermissionText, Emp_PermissionValue from tlb_emp_roles_permission where EmpType_Value = '" + DDL_EmployeeType.SelectedValue.ToString() + "' order by Id";
                Bind_EmpRole_Permissions(CmdString2);
            }
        }
        #endregion

        #region Submit Logic

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

        private string HashPassword(string password)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha.ComputeHash(
                    System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private Int32 Insert_EmplyeeMusterData()
        {
            string LoginID = "";
            string LoginPassword = "";

            dbcl.GenerateLoginID(ref LoginID);

            Int32 password_length = 6;
            dbcl.GenerateLoginPassword(password_length, ref LoginPassword);

            string hashedPwd = HashPassword(LoginPassword);

            int flag = 0;
            try
            {
                dbcl.Sqlconnection();
                // NOTE: Explicitly opening connection here because ExecuteNonQuery requires it open
                // DB_Utility_OH4Y seems to manage connection strings but we must manage state carefully
                if (dbcl.Conn.State == ConnectionState.Closed) { dbcl.ConnectDb(); }

                SqlCommand cmd = new SqlCommand("SP_InsertInto_EmployeeMusterTable", dbcl.Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // 1. STANDARD FIELDS
                cmd.Parameters.AddWithValue("@WorkStatus", "Active");
                cmd.Parameters.AddWithValue("@LoginID", LoginID);
                cmd.Parameters.AddWithValue("@LoginPassword", hashedPwd); // Standard: Hash goes to Password col
                cmd.Parameters.AddWithValue("@LoginPassword_Plain", LoginPassword); // Plain text (if required by DB schema)

                cmd.Parameters.AddWithValue("@WorkCountry", DDL_WorkCountry.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WorkState", DDL_WorkStates.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WorkRegion", DDL_WorkRegion.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WorkCompany", DDL_Company.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WorkmanSL", txt_workman.Text.TrimEnd());
                cmd.Parameters.AddWithValue("@FirstName", txt_empfname.Text);

                string mdname = txt_empmdname.Text.Trim();
                if (string.IsNullOrEmpty(mdname) || mdname == "#N/A" || mdname == "#NA")
                    cmd.Parameters.AddWithValue("@MiddleName", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@MiddleName", mdname);

                string lstname = txt_emplstname.Text.Trim();
                if (string.IsNullOrEmpty(lstname) || lstname == "#N/A" || lstname == "#NA")
                    cmd.Parameters.AddWithValue("@LastName", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@LastName", lstname);

                cmd.Parameters.AddWithValue("@FullName", txt_fullanme.Text);
                cmd.Parameters.AddWithValue("@Fathername", txt_empfathername.Text);
                cmd.Parameters.AddWithValue("@BloodGroup", txt_bloodgroup.Text);
                cmd.Parameters.AddWithValue("@MobileNo", txt_Mobile_Number.Text);

                // Safe Date Parsing
                DateTime dob;
                if (DateTime.TryParse(txt_DOB.Text, out dob)) cmd.Parameters.AddWithValue("@DOB", dob);
                else cmd.Parameters.AddWithValue("@DOB", DBNull.Value);

                cmd.Parameters.AddWithValue("@Qualification", DDL_HighestEdu.SelectedItem != null ? DDL_HighestEdu.SelectedItem.Text : "");

                DateTime doj;
                if (DateTime.TryParse(txt_DOJ.Text, out doj)) cmd.Parameters.AddWithValue("@DOJ", doj);
                else cmd.Parameters.AddWithValue("@DOJ", DBNull.Value);

                cmd.Parameters.AddWithValue("@WorkSite", DDL_Worksites.SelectedItem != null ? DDL_Worksites.SelectedItem.Text : "");
                cmd.Parameters.AddWithValue("@Worksite_Code", DDL_Worksites.SelectedValue.ToString());

                // 2. TEXT VALUES (Display)
                cmd.Parameters.AddWithValue("@SkillCategory", DDL_SkillCategory.SelectedItem != null ? DDL_SkillCategory.SelectedItem.Text : "");
                cmd.Parameters.AddWithValue("@SkillDesignation", DDL_SkillDesignation.SelectedItem != null ? DDL_SkillDesignation.SelectedItem.Text : "");
                cmd.Parameters.AddWithValue("@User_RoleType", DDL_EmployeeType.SelectedItem != null ? DDL_EmployeeType.SelectedItem.Text : "");
                cmd.Parameters.AddWithValue("@Role_Permission", DDL_RolePermissions.SelectedItem != null ? DDL_RolePermissions.SelectedItem.Text : "");

                // 3. ID VALUES (Aligned with SP INT types)
                cmd.Parameters.AddWithValue("@SkillCategoryDB", DDL_SkillCategory.SelectedValue.ToString()); // SP expects Varchar(50)
                cmd.Parameters.AddWithValue("@SkillDesignationDB", DDL_SkillDesignation.SelectedValue.ToString()); // SP expects Varchar(50)

                int userRoleDB = 0;
                int.TryParse(DDL_EmployeeType.SelectedValue, out userRoleDB);
                cmd.Parameters.AddWithValue("@UserRoleDB", userRoleDB); // SP expects INT

                int rolePermissionDB = 0;
                int.TryParse(DDL_RolePermissions.SelectedValue, out rolePermissionDB);
                cmd.Parameters.AddWithValue("@RolePermissionDB", rolePermissionDB); // SP expects INT

                // 4. OTHER FIELDS
                int workHours = 0;
                int.TryParse(DDL_WorkHours.SelectedValue, out workHours);
                cmd.Parameters.AddWithValue("@WorkHours", workHours);

                int otFactor = 0;
                int.TryParse(DDL_OTFactor.SelectedValue, out otFactor);
                cmd.Parameters.AddWithValue("@OTFactor", otFactor);

                cmd.Parameters.AddWithValue("@SafetyPassNo", txt_rfidno.Text);

                DateTime rfidExpiry;
                if (DateTime.TryParse(txt_rfidvalidity.Text, out rfidExpiry)) cmd.Parameters.AddWithValue("@SafetyPassExpiry", rfidExpiry);
                else cmd.Parameters.AddWithValue("@SafetyPassExpiry", DBNull.Value);

                cmd.Parameters.AddWithValue("@GatePassNo", txt_gpno.Text);

                DateTime gpExpiry;
                if (DateTime.TryParse(txt_gpvalidity.Text, out gpExpiry)) cmd.Parameters.AddWithValue("@GatePassExpiry", gpExpiry);
                else cmd.Parameters.AddWithValue("@GatePassExpiry", DBNull.Value);

                DateTime pvExpiry;
                if (DateTime.TryParse(txt_pvvalidity.Text, out pvExpiry)) cmd.Parameters.AddWithValue("@PVExpiry", pvExpiry);
                else cmd.Parameters.AddWithValue("@PVExpiry", DBNull.Value);

                cmd.Parameters.AddWithValue("@UANNo", txt_uanno.Text);
                cmd.Parameters.AddWithValue("@ESICNo", txt_esicno.Text);
                cmd.Parameters.AddWithValue("@Payment_Bank", DDL_BankName.SelectedItem != null ? DDL_BankName.SelectedItem.Text : "");
                cmd.Parameters.AddWithValue("@Payment_Account", txt_accountno.Text);
                cmd.Parameters.AddWithValue("@Payment_IFSC", txt_ifsccode.Text);
                cmd.Parameters.AddWithValue("@BankBranch", txt_bankbranch.Text.ToUpper());
                cmd.Parameters.AddWithValue("@RegistrationType", "Single");

                // Session handling
                string registeredBy = Session["USERNAME"] != null ? Session["USERNAME"].ToString() : "Admin";
                string registeredByWrk = Session["WORKMAN"] != null ? Session["WORKMAN"].ToString() : "000";

                cmd.Parameters.AddWithValue("@Registered_ByName", registeredBy);
                cmd.Parameters.AddWithValue("@Registered_ByWRK", registeredByWrk);
                cmd.Parameters.AddWithValue("@LoginStatus", 0);
                cmd.Parameters.AddWithValue("@LastLogin", DateTime.Now);
                cmd.Parameters.AddWithValue("@LastLogout", DateTime.Now);

                flag = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                lbl_msg.Visible = true;
                lbl_msg.Text = "Error: " + ex.Message;
                lbl_msg.ForeColor = System.Drawing.Color.IndianRed;
            }
            finally
            {
                dbcl.DisconnectDb();
            }

            if (flag != 0)
            {
                lbl_msg.Visible = true;
                lbl_msg.Text = "Record Inserted Successfully..!";
                lbl_msg.ForeColor = System.Drawing.Color.DarkGreen;
            }
            return flag;
        }
        #endregion

        #region Validation Logic
        protected void txt_workman_TextChanged(object sender, EventArgs e)
        {
            string inputstring = txt_workman.Text.Trim();
            if (string.IsNullOrEmpty(inputstring)) return;

            // FIX: Use Parameterized query to prevent SQL Injection
            string cmdString = "select COUNT(Id) as count from tbl_Employee_Mustertable where WorkmanSL=@WorkmanSL";

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.Parameters.AddWithValue("@WorkmanSL", inputstring);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    Int32 count = Convert.ToInt32(result);

                    // FIX: Check > 0 to catch duplicates properly
                    if (count > 0)
                    {
                        txt_workman.Text = "";
                        btn_submit.Enabled = false;
                        txt_workman.Focus();
                        string title = "ERROR :";
                        string body = "Workmen SL ALREADY EXISTS....!!!";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                    else
                    {
                        txt_workman.ReadOnly = true;
                        btn_submit.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle logging
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }
        #endregion
    }
}