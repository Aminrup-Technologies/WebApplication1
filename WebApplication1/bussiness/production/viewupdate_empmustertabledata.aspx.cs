using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.IO;

namespace WebApplication1.bussiness.production
{
    public partial class viewupdate_empmustertabledata : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        DataTable dt = new DataTable();
        static string workhours = "";
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
                    string bankqry = "select BankName, BankCode from tlb_IndianBanks";
                    BindBanks(bankqry);

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

                    DDLBinder();
                    string workmansl = Request.QueryString["ID"];
                    //string workmansl = "A23";

                    EmpDataBinder(workmansl);

                    DisplayNewGPData();
                    EmpBankDataBinder();

                }
            }
        }

        private void BindBanks(string CmdString)
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
            dbcl.DisconnectDb();
        }

        protected void DDLBinder()
        {
            string CmdString1 = "select Country_Name, Country_Code from tlb_work_country";
            BindCountry(CmdString1);

            string CmdString2 = "select State_Name, State_Code from tlb_work_state order by Id";
            BindWorkState(CmdString2);

            string CmdString3 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region order by Id";
            BindWorkRegion(CmdString3);

            string CmdString4 = "select Company_Name, Company_Code from tlb_workregion_company order by Id";
            BindCompany(CmdString4);

            string CmdString5 = "select Category_Type, Category_DB from tlb_payroll_category where WorkRegion_Code ='"+ region + "' order by Id";
            BindSkillCategory(CmdString5);

            string CmdString6 = "select Worksite_Name, DB_Code from tlb_atsworksites where WorkRegion_Code ='" + region + "' order by Id";
            BindWorksites(CmdString6);

            string CmdString6a = "select Eduction_Type, EducationDB from tlb_education_types order by Id";
            BindEducations(CmdString6a);

            //dbcl.FillCombo(DDL_HighestEdu, "select Eduction_Type from tlb_education_types order by Id");

            string CmdString7 = "select Work_HoursTypes, Work_Hours from tlb_payroll_hours order by Id";
            BindWorkHours(CmdString7);

            string CmdString8 = "select OT_Type, OT_TypeValue from tlb_payroll_ottypes order by Id";
            BindOTFactor(CmdString8);

            string CmdString9 = "select Employee_Type, EmpType_Value from tlb_emp_roles order by Id";
            BindEmployeeType(CmdString9);

            string CmdString10 = "select Designation_Type, Designation_DB from tlb_payroll_designation where WorkRegion_Code ='" + region + "' order by Id";
            BindSkillDesignation(CmdString10);

            string CmdString11 = "select Emp_PermissionText, Emp_PermissionValue from tlb_emp_roles_permission order by Id";
            Bind_EmpRole_Permissions(CmdString11);
        }

        protected void EmpDataBinder(string workmansl)
        {
            try
            {
                string query = "select * from tbl_Employee_Mustertable where WorkmanSL=@WorkmanSL";
                SqlParameter[] pram = {
                                          new SqlParameter("@WorkmanSL",workmansl),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    txt_workman.Text = workmansl;

                    string country = dt.Rows[0]["WorkCountry"].ToString();
                    DDL_WorkCountry.SelectedValue = country;

                    string state = dt.Rows[0]["WorkState"].ToString();
                    DDL_WorkStates.SelectedValue = state;

                    string region = dt.Rows[0]["WorkRegion"].ToString();
                    DDL_WorkRegion.SelectedValue = region;

                    string company = dt.Rows[0]["WorkCompany"].ToString();
                    DDL_Company.SelectedValue = company;


                    txt_empfname.Text = dt.Rows[0]["FirstName"].ToString();
                    txt_empmdname.Text = dt.Rows[0]["MiddleName"].ToString();
                    txt_emplstname.Text = dt.Rows[0]["LastName"].ToString();
                    txt_fullanme.Text = dt.Rows[0]["FullName"].ToString();

                    txt_empfathername.Text = dt.Rows[0]["Fathername"].ToString();
                    //txt_empmothername.Text = dt.Rows[0]["Fathername"].ToString();
                    txt_empmothername.Text = "N/A";

                    //string dob = dt.Rows[0]["DOB"].ToString();
                    //txt_DOB.Text = dob;

                    if (dt.Rows[0]["DOB"] != DBNull.Value)
                    {
                        txt_DOB.Text = Convert.ToDateTime(dt.Rows[0]["DOB"])
                            .ToString("yyyy-MM-dd");
                    }

                    txt_bloodgroup.Text = dt.Rows[0]["BloodGroup"].ToString();
                    txt_Mobile_Number.Text = dt.Rows[0]["MobileNo"].ToString();

                    string qualification = dt.Rows[0]["QualificationDB"].ToString();
                    DDL_HighestEdu.SelectedValue = qualification;

                    //string doj = dt.Rows[0]["DOJ"].ToString();
                    //txt_DOJ.Text = doj;

                    if (dt.Rows[0]["DOJ"] != DBNull.Value)
                    {
                        txt_DOJ.Text = Convert.ToDateTime(dt.Rows[0]["DOJ"])
                            .ToString("yyyy-MM-dd");
                    }

                    string worksite = dt.Rows[0]["Worksite_Code"].ToString();
                    DDL_Worksites.SelectedValue = worksite;

                    string skillcat = dt.Rows[0]["SkillCategoryDB"].ToString();
                    DDL_SkillCategory.SelectedValue = skillcat;

                    //string CmdString10 = "select Designation_Type, Designation_DB from tlb_payroll_designation where WorkRegion_Code ='" + Session["REGION"].ToString() + "' Category_DB='"+ skillcat + "' order by Id";
                    //BindSkillDesignation(CmdString10);

                    string skilldesg = dt.Rows[0]["SkillDesignationDB"].ToString();
                    DDL_SkillDesignation.SelectedValue = skilldesg;

                    string emptype = dt.Rows[0]["UserRoleDB"].ToString();
                    DDL_EmployeeType.SelectedValue = emptype;

                    string roletype = dt.Rows[0]["RolePermissionDB"].ToString();
                    DDL_RolePermissions.SelectedValue = roletype;

                    workhours = dt.Rows[0]["WorkHours"].ToString();
                    DDL_WorkHours.SelectedValue = workhours;

                    string otfactor = dt.Rows[0]["OTFactor"].ToString();
                    DDL_OTFactor.SelectedValue = otfactor;

                    txt_uanno.Text = dt.Rows[0]["UANNo"].ToString();
                    txt_esicno.Text = dt.Rows[0]["ESICNo"].ToString();

                    CaptureOriginalValues();
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }


        private string GetAllFieldChanges()
        {
            if (ViewState["ORIGINAL_DATA"] == null)
                return "";

            Dictionary<string, string> original =
                (Dictionary<string, string>)ViewState["ORIGINAL_DATA"];

            List<string> changes = new List<string>();

            CompareValue(changes, original, "FirstName", txt_empfname.Text);
            CompareValue(changes, original, "MiddleName", txt_empmdname.Text);
            CompareValue(changes, original, "LastName", txt_emplstname.Text);
            CompareValue(changes, original, "FullName", txt_fullanme.Text);
            CompareValue(changes, original, "FatherName", txt_empfathername.Text);
            CompareValue(changes, original, "DOB", txt_DOB.Text);
            CompareValue(changes, original, "DOJ", txt_DOJ.Text);
            CompareValue(changes, original, "MobileNo", txt_Mobile_Number.Text);
            CompareValue(changes, original, "BloodGroup", txt_bloodgroup.Text);

            CompareValue(changes, original, "Qualification", DDL_HighestEdu.SelectedItem.Text);
            CompareValue(changes, original, "WorkCountry", DDL_WorkCountry.SelectedValue);
            CompareValue(changes, original, "WorkState", DDL_WorkStates.SelectedValue);
            CompareValue(changes, original, "WorkRegion", DDL_WorkRegion.SelectedValue);
            CompareValue(changes, original, "Company", DDL_Company.SelectedValue);
            CompareValue(changes, original, "Worksite", DDL_Worksites.SelectedValue);

            CompareValue(changes, original, "SkillCategory", DDL_SkillCategory.SelectedValue);
            CompareValue(changes, original, "SkillDesignation", DDL_SkillDesignation.SelectedValue);

            CompareValue(changes, original, "EmployeeType", DDL_EmployeeType.SelectedValue);
            CompareValue(changes, original, "RolePermission", DDL_RolePermissions.SelectedValue);

            CompareValue(changes, original, "WorkHours", DDL_WorkHours.SelectedValue);
            CompareValue(changes, original, "OTFactor", DDL_OTFactor.SelectedValue);

            CompareValue(changes, original, "UAN", txt_uanno.Text);
            CompareValue(changes, original, "ESIC", txt_esicno.Text);

            return string.Join(Environment.NewLine, changes);
        }

        private void WriteFullAuditLog(string changes)
        {
            string folder = Server.MapPath("~/bussiness/production/Logs/EmployeeEdits/");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string file = Path.Combine(
                folder,
                $"EMP_{txt_workman.Text}_{DateTime.Now:yyyyMMdd}.log"
            );

            string log =
                $"Timestamp : {DateTime.Now:dd-MM-yyyy HH:mm:ss}\n" +
                $"Modified By : {Session["USERNAME"]} ({Session["WORKMAN"]})\n" +
                $"Employee : {txt_workman.Text}\n" +
                $"Changes:\n{changes}\n" +
                $"------------------------------------------\n";

            File.AppendAllText(file, log);
        }



        private void CompareValue(
            List<string> changes,
            Dictionary<string, string> original,
            string key,
            string currentValue)
        {
            string oldValue = original.ContainsKey(key)
                ? (original[key] ?? "").Trim()
                : "";

            string newValue = (currentValue ?? "").Trim();

            if (oldValue != newValue)
            {
                changes.Add(key + ": '" + oldValue + "' → '" + newValue + "'");
            }
        }


        public void FindDaysLeft(string emp_outtime, ref Int32 day)
        {
            string currentdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            DateTime intm = DateTime.Parse(currentdate.ToString());
            DateTime outm = DateTime.Parse(emp_outtime.ToString());

            TimeSpan duration = outm.Subtract(intm);
            day = duration.Days;
        }

        private string DateBinder(string date)
        {
            string newdate = "";
            DateTime oDate = Convert.ToDateTime(date);
            string day = "";
            string month = "";
            if (oDate.Day < 10)
            {
                day = "0" + oDate.Day.ToString();
            }
            else
            {
                day = oDate.Day.ToString();
            }
            if (oDate.Month < 10)
            {
                month = "0" + oDate.Month.ToString();
            }
            else
            {
                month = oDate.Month.ToString();
            }
            return newdate = day + "-" + month + "-" + oDate.Year;
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


        protected void DDL_WorkCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL_Value = DDL_WorkCountry.SelectedValue.ToString();
            string CmdString3 = "select State_Name, State_Code from tlb_work_state where Country_Code = '" + DDL_Value + "' order by Id";
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

            string CmdString3 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = '" + DDL1_String + "' and State_Code='" + DDL2_String + "' order by Id";
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

        private void BindEducations(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_HighestEdu.DataSource = Cmd.ExecuteReader();
            DDL_HighestEdu.DataTextField = "Eduction_Type";
            DDL_HighestEdu.DataValueField = "EducationDB";
            DDL_HighestEdu.DataBind();
            DDL_HighestEdu.Items.Insert(0, "Please Select Option");
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

            string CmdString2 = "select Worksite_Name, DB_Code from tlb_atsworksites where Country_Code = '" + DDL1_Value + "' and State_Code='" + DDL2_Value + "' and WorkRegion_Code='" + DDL3_Value + "' and Company_Code='" + DDL4_Value + "' order by Id";
            BindWorksites(CmdString2);
        }

        protected void EducationType_Binder()
        {
            dbcl.FillCombo(DDL_HighestEdu, "select Eduction_Type from tlb_education_types order by Id");
        }

        protected void Bind_Workhours()
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

        protected void BindOvertime()
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
            DDL_Worksites.DataValueField = "DB_Code";
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

            string CmdString2 = "select Designation_Type, Designation_DB from tlb_payroll_designation where Country_Code = '" + DDL1_Value + "' and State_Code='" + DDL2_Value + "' and WorkRegion_Code='" + DDL3_Value + "' and Company_Code='" + DDL4_Value + "' and Category_DB =  '" + DDL5_Value + "' order by Id";
            BindSkillDesignation(CmdString2);

            DDL_SkillDesignation.Enabled = true;

            //Bind_Workhours();
            //BindOvertime();
            //Binder5();
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
            string CmdString2 = "select Emp_PermissionText, Emp_PermissionValue from tlb_emp_roles_permission where EmpType_Value = '" + DDL_EmployeeType.SelectedValue.ToString() + "' order by Id";
            Bind_EmpRole_Permissions(CmdString2);

            DDL_RolePermissions.Enabled = true;
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

        private void CaptureOriginalValues()
        {
            var original = new Dictionary<string, string>();

            original["FirstName"] = txt_empfname.Text;
            original["MiddleName"] = txt_empmdname.Text;
            original["LastName"] = txt_emplstname.Text;
            original["FullName"] = txt_fullanme.Text;
            original["FatherName"] = txt_empfathername.Text;
            original["DOB"] = txt_DOB.Text;
            original["DOJ"] = txt_DOJ.Text;
            original["MobileNo"] = txt_Mobile_Number.Text;
            original["BloodGroup"] = txt_bloodgroup.Text;

            original["Qualification"] = DDL_HighestEdu.SelectedItem.Text;
            original["QualificationDB"] = DDL_HighestEdu.SelectedValue;

            original["WorkCountry"] = DDL_WorkCountry.SelectedValue;
            original["WorkState"] = DDL_WorkStates.SelectedValue;
            original["WorkRegion"] = DDL_WorkRegion.SelectedValue;
            original["Company"] = DDL_Company.SelectedValue;
            original["Worksite"] = DDL_Worksites.SelectedValue;

            original["SkillCategory"] = DDL_SkillCategory.SelectedValue;
            original["SkillDesignation"] = DDL_SkillDesignation.SelectedValue;

            original["EmployeeType"] = DDL_EmployeeType.SelectedValue;
            original["RolePermission"] = DDL_RolePermissions.SelectedValue;

            original["WorkHours"] = DDL_WorkHours.SelectedValue;
            original["OTFactor"] = DDL_OTFactor.SelectedValue;

            original["UAN"] = txt_uanno.Text;
            original["ESIC"] = txt_esicno.Text;

            ViewState["ORIGINAL_DATA"] = original;
        }


        private Int32 Update_EmplyeeMusterData_OLD()
        {
            //Code to Insert values into the DB goes here
            int flag = 0;
            try
            {
                dbcl.Sqlconnection();
                SqlCommand cmd = new SqlCommand("SP_Update_EmployeeMusterTable", dbcl.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WorkmanSL", txt_workman.Text.TrimEnd().ToString());
                cmd.Parameters.AddWithValue("@FirstName", txt_empfname.Text.ToString());

                string mdname = txt_empmdname.Text.ToUpper().ToString();
                if (mdname == "#N/A" || mdname == " " || mdname == "#NA" || mdname == "NA" || mdname == "N/A" || mdname == "#NA")
                {
                    cmd.Parameters.AddWithValue("@MiddleName", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MiddleName", mdname);
                }

                string lstname = txt_emplstname.Text.ToUpper().ToString();
                if (lstname == "#N/A" || lstname == " " || lstname == "#NA")
                {
                    cmd.Parameters.AddWithValue("@LastName", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@LastName", lstname);
                }
                cmd.Parameters.AddWithValue("@FullName", txt_fullanme.Text.ToUpper().ToString());
                cmd.Parameters.AddWithValue("@Fathername", txt_empfathername.Text.ToUpper().ToString());
                cmd.Parameters.AddWithValue("@BloodGroup", txt_bloodgroup.Text.ToString());
                cmd.Parameters.AddWithValue("@MobileNo", txt_Mobile_Number.Text.ToString());

                cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(txt_DOB.Text.ToString()));

                cmd.Parameters.AddWithValue("@Qualification", DDL_HighestEdu.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@QualificationDB", DDL_HighestEdu.SelectedValue.ToString());

                cmd.Parameters.AddWithValue("@DOJ", Convert.ToDateTime(txt_DOJ.Text.ToString()));

                cmd.Parameters.AddWithValue("@WorkSite", DDL_Worksites.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Worksite_Code", DDL_Worksites.SelectedValue.ToString());

                cmd.Parameters.AddWithValue("@SkillCategory", DDL_SkillCategory.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@SkillCategoryDB", DDL_SkillCategory.SelectedValue.ToString());

                cmd.Parameters.AddWithValue("@SkillDesignation", DDL_SkillDesignation.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@SkillDesignationDB", DDL_SkillDesignation.SelectedValue.ToString());

                cmd.Parameters.AddWithValue("@User_RoleType", DDL_EmployeeType.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@UserRoleDB", DDL_EmployeeType.SelectedValue.ToString());

                cmd.Parameters.AddWithValue("@Role_Permission", DDL_RolePermissions.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@RolePermissionDB", DDL_RolePermissions.SelectedValue.ToString());

                cmd.Parameters.AddWithValue("@WorkHours", DDL_WorkHours.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@OTFactor", DDL_OTFactor.SelectedValue.ToString());

                //cmd.Parameters.AddWithValue("@SafetyPassNo", txt_rfidno.Text.ToString());
                //cmd.Parameters.AddWithValue("@SafetyPassExpiry", Convert.ToDateTime(txt_rfidvalidity.Text.ToString()));

                //cmd.Parameters.AddWithValue("@GatePassNo", txt_gpno.Text.ToString());
                //cmd.Parameters.AddWithValue("@GatePassExpiry", Convert.ToDateTime(txt_gpvalidity.Text.ToString()));

                //cmd.Parameters.AddWithValue("@PVExpiry", Convert.ToDateTime(txt_pvvalidity.Text.ToString()));

                cmd.Parameters.AddWithValue("@UANNo", txt_uanno.Text.ToString());
                cmd.Parameters.AddWithValue("@ESICNo", txt_esicno.Text.ToString());

                //cmd.Parameters.AddWithValue("@Payment_Bank", txt_banknanme.Text.ToUpper().ToString());
                //cmd.Parameters.AddWithValue("@Payment_Account", txt_accountno.Text.ToString());
                //cmd.Parameters.AddWithValue("@Payment_IFSC", txt_ifsccode.Text.ToUpper().ToString());

                cmd.Parameters.AddWithValue("@LastModifiedDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@LastModifiedByName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@LastModifiedByWrk", Session["WORKMAN"].ToString());
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
                lbl_msg.Text = "Record Updated Successfully..!";
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

        private Int32 Update_EmplyeeMusterData()
        {
            int flag = 0;
            try
            {
                dbcl.Sqlconnection();
                SqlCommand cmd = new SqlCommand("SP_Update_EmployeeMusterTable", dbcl.Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // 1. IDENTIFIER
                cmd.Parameters.AddWithValue("@WorkmanSL", txt_workman.Text.TrimEnd());

                // 2. PERSONAL DETAILS
                cmd.Parameters.AddWithValue("@FirstName", txt_empfname.Text);

                // Middle Name Logic
                string mdname = txt_empmdname.Text.ToUpper();
                if (string.IsNullOrWhiteSpace(mdname) || mdname == "#N/A" || mdname == "#NA" || mdname == "NA" || mdname == "N/A")
                {
                    cmd.Parameters.AddWithValue("@MiddleName", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MiddleName", mdname);
                }

                // Last Name Logic
                string lstname = txt_emplstname.Text.ToUpper();
                if (string.IsNullOrWhiteSpace(lstname) || lstname == "#N/A" || lstname == "#NA")
                {
                    cmd.Parameters.AddWithValue("@LastName", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@LastName", lstname);
                }

                cmd.Parameters.AddWithValue("@FullName", txt_fullanme.Text.ToUpper());
                cmd.Parameters.AddWithValue("@Fathername", txt_empfathername.Text.ToUpper());
                cmd.Parameters.AddWithValue("@BloodGroup", txt_bloodgroup.Text);
                cmd.Parameters.AddWithValue("@MobileNo", txt_Mobile_Number.Text);

                // Dates
                cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(txt_DOB.Text));
                cmd.Parameters.AddWithValue("@DOJ", Convert.ToDateTime(txt_DOJ.Text));

                // Qualification (Text & Value)
                cmd.Parameters.AddWithValue("@Qualification", DDL_HighestEdu.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@QualificationDB", DDL_HighestEdu.SelectedValue);

                // 3. WORK DETAILS (Passing both Text and ID)
                cmd.Parameters.AddWithValue("@WorkSite", DDL_Worksites.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@Worksite_Code", DDL_Worksites.SelectedValue);

                cmd.Parameters.AddWithValue("@SkillCategory", DDL_SkillCategory.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@SkillCategoryDB", DDL_SkillCategory.SelectedValue);

                cmd.Parameters.AddWithValue("@SkillDesignation", DDL_SkillDesignation.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@SkillDesignationDB", DDL_SkillDesignation.SelectedValue);

                cmd.Parameters.AddWithValue("@User_RoleType", DDL_EmployeeType.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@UserRoleDB", DDL_EmployeeType.SelectedValue); // Ensure this parses to INT in SP if needed, or pass as object

                cmd.Parameters.AddWithValue("@Role_Permission", DDL_RolePermissions.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@RolePermissionDB", DDL_RolePermissions.SelectedValue);

                cmd.Parameters.AddWithValue("@WorkHours", DDL_WorkHours.SelectedValue);
                cmd.Parameters.AddWithValue("@OTFactor", DDL_OTFactor.SelectedValue);

                // 4. STATUTORY DETAILS
                cmd.Parameters.AddWithValue("@UANNo", txt_uanno.Text);
                cmd.Parameters.AddWithValue("@ESICNo", txt_esicno.Text);

                // 5. AUDIT TRAILS
                cmd.Parameters.AddWithValue("@LastModifiedDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@LastModifiedByName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@LastModifiedByWrk", Session["WORKMAN"].ToString());

                dbcl.ConnectDb();
                flag = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                lbl_msg.Visible = true;
                lbl_msg.Text = ex.Message;
                lbl_msg.ForeColor = System.Drawing.Color.IndianRed;
            }
            finally
            {
                dbcl.DisconnectDb(); // Moved Disconnect to finally block for safety
            }

            if (flag != 0)
            {
                lbl_msg.Visible = true;
                lbl_msg.Text = "Record Updated Successfully..!";
                lbl_msg.ForeColor = System.Drawing.Color.DarkGreen;
            }

            return flag;
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            // Step 1: detect changes
            string changes = GetAllFieldChanges();

            if (string.IsNullOrWhiteSpace(changes))
            {
                // Use your EXISTING popup
                string title = "Info";
                string body = "No changes detected.";
                ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "Popup",
                    "ShowPopup('" + title + "', '" + body + "');",
                    true
                );
                return;
            }

            // Step 2: if not confirmed yet, show audit popup
            if (Request["__EVENTTARGET"] != btn_save.UniqueID)
            {
                string safeChanges = HttpUtility.JavaScriptStringEncode(changes);

                ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "auditPopup",
                    "showAuditPopup('" + safeChanges + "');",
                    true
                );
                return;
            }

            // Step 3: confirmed → update DB
            if (Update_EmplyeeMusterData() != 0)
            {
                //WriteFullAuditLog(changes);
                WriteModuleAuditLog("PERSONAL_PROF", changes);

                string title = "Notifications :";
                string body = "Data Updated Successfully";
                ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "Popup",
                    "ShowPopup('" + title + "', '" + body + "');",
                    true
                );
            }
        }


        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("view_emp_mastertbldata.aspx");
        }



        protected void btn_bankedit_Click(object sender, EventArgs e)
        {
            if (btn_bankedit.Text.ToString() == "Make Changes")
            {
                //txt_nwbankname.ReadOnly = false;
                DDL_BankName.Enabled = true;
                txt_nwaccno.ReadOnly = false;
                txt_nwcnfaccno.ReadOnly = false;
                txt_nwifsc.ReadOnly = false;
                txt_nwbranchname.ReadOnly = false;

                btn_bankedit.Text = "Save Changes";

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup1();", true);
            }
            else if (btn_bankedit.Text.ToString() == "Save Changes")
            {
                string bankChanges = GetBankFieldChanges();
                //if (UpdateBankDetails() == true)
                //{
                //    EmpBankDataBinder();
                //    DDL_BankName.Enabled = false;
                //    //txt_nwbankname.ReadOnly = false;
                //    txt_nwaccno.ReadOnly = false;
                //    txt_nwcnfaccno.ReadOnly = false;
                //    txt_nwifsc.ReadOnly = false;
                //    txt_nwbranchname.ReadOnly = false;
                //    btn_bankedit.Text = "Make Changes";

                //    string title = "Notifications :";
                //    string body = "Employee Bank Details has been updated...!";
                //    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                //    //ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup1();", true);
                //}

                if (UpdateBankDetails())
                {
                    if (!string.IsNullOrWhiteSpace(bankChanges))
                    {
                        WriteModuleAuditLog("BANK_DETAILS", bankChanges);
                    }

                    EmpBankDataBinder();
                    btn_bankedit.Text = "Make Changes";

                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "Popup",
                        "ShowPopup('Notifications :','Employee Bank Details updated');",
                        true
                    );
                }

                else
                {
                    //txt_nwbankname.ReadOnly = true;
                    DDL_BankName.Enabled = true;
                    txt_nwaccno.ReadOnly = true;
                    txt_nwcnfaccno.ReadOnly = true;
                    txt_nwifsc.ReadOnly = true;
                    txt_nwbranchname.ReadOnly = true;
                    btn_bankedit.Text = "Make Changes";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup1();", true);
                }
            }
        }

        private string GetBankFieldChanges()
        {
            if (ViewState["ORIGINAL_BANK_DATA"] == null)
                return "";

            Dictionary<string, string> original =
                (Dictionary<string, string>)ViewState["ORIGINAL_BANK_DATA"];

            List<string> changes = new List<string>();

            CompareValue(changes, original, "Payment_Bank", DDL_BankName.SelectedItem.Text);
            CompareValue(changes, original, "Payment_Account", txt_nwaccno.Text);
            CompareValue(changes, original, "Payment_IFSC", txt_nwifsc.Text);
            CompareValue(changes, original, "BankBranch", txt_nwbranchname.Text);

            return string.Join(Environment.NewLine, changes);
        }


        private Boolean UpdateBankDetails()
        {
            Boolean flag = false;
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_Employee_Mustertable set Payment_Bank=@Payment_Bank, Payment_Account=@Payment_Account, Payment_IFSC=@Payment_IFSC, BankBranch=@BankBranch, BankUpdatedOn=@BankUpdatedOn, BankUpdatedByName=@BankUpdatedByName , BankUpdatedByWrk=@BankUpdatedByWrk where WorkmanSL=@WorkmanSL";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@WorkmanSL", txt_workman.Text.ToString());
                //cmd.Parameters.AddWithValue("@Payment_Bank", txt_nwbankname.Text.ToString());
                cmd.Parameters.AddWithValue("@Payment_Bank", DDL_BankName.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Payment_Account", txt_nwaccno.Text.ToString());
                cmd.Parameters.AddWithValue("@Payment_IFSC", txt_nwifsc.Text.ToString());
                cmd.Parameters.AddWithValue("@BankBranch", txt_nwbranchname.Text.ToString());
                cmd.Parameters.AddWithValue("@BankUpdatedOn", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@BankUpdatedByName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@BankUpdatedByWrk", Session["WORKMAN"].ToString());
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Error: " + ex.Message.ToString();
            }

            return flag;
        }
        protected void EmpBankDataBinder()
        {
            try
            {
                string query = "select * from tbl_Employee_Mustertable where WorkmanSL=@WorkmanSL";
                SqlParameter[] pram = {
                                          new SqlParameter("@WorkmanSL",txt_workman.Text.ToString()),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    //string bankname = dt.Rows[0]["Payment_Bank"].ToString();
                    //txt_banknanme.Text = bankname;
                    //DDL_BankName.SelectedItem.Text = bankname;

                    string bankname = Convert.ToString(dt.Rows[0]["Payment_Bank"]);
                    txt_banknanme.Text = bankname;

                    ListItem item = DDL_BankName.Items.FindByText(bankname);
                    if (item != null)
                    {
                        DDL_BankName.ClearSelection();
                        item.Selected = true;
                    }

                    string bankacc = dt.Rows[0]["Payment_Account"].ToString();
                    txt_accountno.Text = bankacc;
                    txt_nwaccno.Text = bankacc;
                    txt_nwcnfaccno.Text = bankacc;

                    string bankifsc = dt.Rows[0]["Payment_IFSC"].ToString();
                    txt_ifsccode.Text = bankifsc;
                    txt_nwifsc.Text = bankifsc;

                    string branch = dt.Rows[0]["BankBranch"].ToString();
                    txt_branchnm.Text = branch;
                    txt_nwbranchname.Text = branch;

                    CaptureOriginalBankValues();
                }
                dbcl.DisconnectDb();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                lbl_msg.ForeColor = System.Drawing.Color.Red;
                lbl_msg.Text = "Error: " + ex.Message.ToString();
            }
        }

        private void CaptureOriginalBankValues()
        {
            Dictionary<string, string> bank = new Dictionary<string, string>();

            bank["Payment_Bank"] = txt_banknanme.Text;
            bank["Payment_Account"] = txt_accountno.Text;
            bank["Payment_IFSC"] = txt_ifsccode.Text;
            bank["BankBranch"] = txt_branchnm.Text;

            ViewState["ORIGINAL_BANK_DATA"] = bank;
        }

        private void CaptureOriginalGPValues()
        {
            Dictionary<string, string> gp = new Dictionary<string, string>();

            gp["GatePassNo"] = txt_gpno.Text;
            gp["GatePassExpiry"] = txt_gpvalidity.Text;
            gp["SafetyPassNo"] = txt_rfidno.Text;
            gp["SafetyPassExpiry"] = txt_rfidvalidity.Text;
            gp["PVExpiry"] = txt_pvvalidity.Text;

            ViewState["ORIGINAL_GP_DATA"] = gp;
        }


        //--------------- 10-11-2021-----------------------------//
        protected void btn_gtpsedit_Click(object sender, EventArgs e)
        {
            if (btn_gtpsedit.Text.ToString() == "Make Changes")
            {
                nwgprow1.Visible = true;
                nwgprow2.Visible = true;

                nwgpvalrow1.Visible = true;
                nwgpvalrow2.Visible = true;

                nwsftyrow1.Visible = true;
                nwsftyrow2.Visible = true;

                nwrfidrow1.Visible = true;
                nwrfidrow2.Visible = true;

                nwpvrow1.Visible = true;
                nwpvrow2.Visible = true;

                btn_gtpsedit.Text = "Save Changes";

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup2();", true);
            }
            else if (btn_gtpsedit.Text.ToString() == "Save Changes")
            {

                string gpChanges = GetGPFieldChanges();

                ReflectNewGPData();

                if (!string.IsNullOrWhiteSpace(gpChanges))
                {
                    WriteModuleAuditLog("GATE_PASS", gpChanges);
                }

                DisplayNewGPData();
                nwgprow1.Visible = false;
                nwgprow2.Visible = false;

                nwgpvalrow1.Visible = false;
                nwgpvalrow2.Visible = false;

                nwsftyrow1.Visible = false;
                nwsftyrow2.Visible = false;

                nwrfidrow1.Visible = false;
                nwrfidrow2.Visible = false;

                nwpvrow1.Visible = false;
                nwpvrow2.Visible = false;

                btn_gtpsedit.Text = "Make Changes";

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup2();", true);
            }
        }

        private void WriteModuleAuditLog(string module, string changes)
        {
            string folder = Server.MapPath("~/bussiness/production/Logs/EmployeeEdits/");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string file = Path.Combine(
                folder,
                $"EMP_{txt_workman.Text}_{DateTime.Now:yyyyMMdd}.log"
            );

            string log =
                $"[{DateTime.Now:dd-MM-yyyy HH:mm:ss}]\n" +
                $"Module : {module}\n" +
                $"Employee : {txt_workman.Text}\n" +
                $"Modified By : {Session["USERNAME"]} ({Session["WORKMAN"]})\n" +
                $"Changes:\n{changes}\n" +
                $"---------------------------------------------\n";

            File.AppendAllText(file, log);
        }


        protected void btn_cancelgpedit_Click(object sender, EventArgs e)
        {
            nwgprow1.Visible = false;
            nwgprow2.Visible = false;

            nwgpvalrow1.Visible = false;
            nwgpvalrow2.Visible = false;

            nwsftyrow1.Visible = false;
            nwsftyrow2.Visible = false;

            nwrfidrow1.Visible = false;
            nwrfidrow2.Visible = false;

            nwpvrow1.Visible = false;
            nwpvrow2.Visible = false;

            btn_gtpsedit.Text = "Make Changes";
        }
        //The function to update the gatepass related changes in DB
        private void ReflectNewGPData()
        {
            string nwgpno = txt_nwgpno.Text.ToString();
            string nwgpval = txt_nwgpvalidity.Text.ToString();

            string nwsftyno = txt_nwsftyno.Text.ToString();
            string nwsftyval = txt_nwsftyvalidity.Text.ToString();

            string nepvval = txt_nwpvvalidity.Text.ToString();

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_Employee_Mustertable set GatePassNo=@GatePassNo, GatePassExpiry=@GatePassExpiry, SafetyPassNo=@SafetyPassNo, SafetyPassExpiry=@SafetyPassExpiry, PVExpiry=@PVExpiry, GP_ModifierWrk=@GP_ModifierWrk, GP_ModifierName=@GP_ModifierName, GP_ModifiedDate=@GP_ModifiedDate, GP_UpdateApproval=@GP_UpdateApproval where WorkmanSL=@WorkmanSL";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@WorkmanSL", txt_workman.Text.ToString());
                cmd.Parameters.AddWithValue("@GatePassNo", nwgpno);
                cmd.Parameters.AddWithValue("@GatePassExpiry", nwgpval);
                cmd.Parameters.AddWithValue("@SafetyPassNo", nwsftyno);
                cmd.Parameters.AddWithValue("@SafetyPassExpiry", nwsftyval);
                cmd.Parameters.AddWithValue("@PVExpiry", nepvval);
                cmd.Parameters.AddWithValue("@GP_ModifierWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@GP_ModifierName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@GP_ModifiedDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@GP_UpdateApproval", "Pending");
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Error: " + ex.Message.ToString();
            }
        }

        //---------------added on 28-11-2021 to instantly reflect gatepass data modification after it is updated by the HR--------Kaushik

        protected void DisplayNewGPData()
        {
            try
            {
                string query = "select SafetyPassNo,SafetyPassExpiry,GatePassNo,GatePassExpiry,PVExpiry from tbl_Employee_Mustertable where WorkmanSL=@WorkmanSL";
                SqlParameter[] pram = {
                                          new SqlParameter("@WorkmanSL",txt_workman.Text.ToString()),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    string gpno = dt.Rows[0]["GatePassNo"].ToString();
                    txt_gpno.Text = gpno;
                    lbl_oldgpno.Text = gpno;
                    txt_nwgpno.Text = gpno;

                    string gpval = dt.Rows[0]["GatePassExpiry"].ToString();
                    Int32 gpdays = 0;
                    FindDaysLeft(gpval, ref gpdays);
                    if (gpdays < 14)
                    {
                        //lbl_gpexpdays.ForeColor = Color.OrangeRed;
                        txt_gpvalidity.ForeColor = Color.OrangeRed;
                        txt_nwgpno.ForeColor = Color.OrangeRed;
                        lbl_oldgpno.ForeColor = Color.OrangeRed;
                    }
                    //lbl_gpexpdays.Text = gpdays.ToString();
                    string gpvaldt = DateBinder(gpval);
                    txt_gpvalidity.Text = gpvaldt;
                    lbl_oldgpvalidity.Text = gpvaldt;
                    txt_nwgpvalidity.Text = gpvaldt;

                    string rfidno = dt.Rows[0]["SafetyPassNo"].ToString();
                    txt_rfidno.Text = rfidno;
                    lbl_oldsftyno.Text = rfidno;
                    txt_nwsftyno.Text = rfidno;

                    string rfidval = dt.Rows[0]["SafetyPassExpiry"].ToString();
                    string rfidvaldt = DateBinder(rfidval);
                    txt_rfidvalidity.Text = rfidvaldt;
                    lbl_oldsftyval.Text = rfidvaldt;
                    txt_nwsftyvalidity.Text = rfidvaldt;

                    Int32 rfiddays = 0;
                    FindDaysLeft(rfidval, ref rfiddays);
                    //lbl_rfiddays.Text = rfiddays.ToString();
                    if (rfiddays < 14)
                    {
                        txt_rfidno.ForeColor = Color.OrangeRed;
                        txt_rfidvalidity.ForeColor = Color.OrangeRed;
                        //lbl_rfiddays.ForeColor = Color.OrangeRed;
                    }

                    string pvvalidity = dt.Rows[0]["PVExpiry"].ToString();
                    string pvvaldt = DateBinder(pvvalidity);
                    txt_pvvalidity.Text = pvvaldt;
                    lbl_oldpvvalidity.Text = pvvaldt;
                    txt_nwpvvalidity.Text = pvvaldt;

                    Int32 pvdays = 0;
                    FindDaysLeft(pvvalidity, ref pvdays);
                    //lbl_pvdays.Text = pvdays.ToString();
                    if (pvdays < 14)
                    {
                        txt_pvvalidity.ForeColor = Color.OrangeRed;
                        //lbl_pvdays.ForeColor = Color.OrangeRed;
                    }

                    CaptureOriginalGPValues();
                }
                dbcl.DisconnectDb();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                lbl_msg.ForeColor = System.Drawing.Color.Red;
                lbl_msg.Text = "Error: " + ex.Message.ToString();
            }
        }

        private string GetGPFieldChanges()
        {
            if (ViewState["ORIGINAL_GP_DATA"] == null)
                return "";

            Dictionary<string, string> original =
                (Dictionary<string, string>)ViewState["ORIGINAL_GP_DATA"];

            List<string> changes = new List<string>();

            CompareValue(changes, original, "GatePassNo", txt_nwgpno.Text);
            CompareValue(changes, original, "GatePassExpiry", txt_nwgpvalidity.Text);
            CompareValue(changes, original, "SafetyPassNo", txt_nwsftyno.Text);
            CompareValue(changes, original, "SafetyPassExpiry", txt_nwsftyvalidity.Text);
            CompareValue(changes, original, "PVExpiry", txt_nwpvvalidity.Text);

            return string.Join(Environment.NewLine, changes);
        }

    }
}