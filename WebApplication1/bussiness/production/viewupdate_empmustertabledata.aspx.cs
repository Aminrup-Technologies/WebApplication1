using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class viewupdate_empmustertabledata : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        // Used for dropdown cascading in Professional Tab
        public static string region = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Basic Session Security
                if (Session["USERID"] == null || Session["WORKMAN"] == null)
                {
                    Response.Redirect("~/login.aspx");
                    return;
                }

                // Initialize Region/Context if needed for DDL filtering
                if (Session["REGION"] != null) region = Session["REGION"].ToString();

                // 1. Load Dropdowns FIRST
                LoadDDLs();

                // 2. Determine Employee ID to Load
                string id = Request.QueryString["ID"];
                if (string.IsNullOrEmpty(id))
                {
                    // Fallback to Session ID if viewing own profile, otherwise error
                    // id = Session["WORKMAN"].ToString(); // Uncomment if you want fallback
                }

                if (!string.IsNullOrEmpty(id))
                {
                    lbl_EmpID.Text = id;
                    LoadEmployeeData(id);
                    LoadDocumentData(id);
                }
                else
                {
                    ShowPopup("Error", "No Employee ID provided in URL. Cannot load data.");
                    btn_SaveAll.Enabled = false;
                }
            }
        }

        private void LoadDDLs()
        {
            try
            {
                // Populate Dropdowns
                // Note: Ensure your DB_Utility_OH4Y.FillCombo handles errors gracefully
                dbcl.FillCombo(DDL_Worksite, "select Worksite_Name, DB_Code from tlb_atsworksites order by Worksite_Name");
                dbcl.FillCombo(DDL_SkillCat, "select Category_Type, Category_DB from tlb_payroll_category order by Id");
                // Initial Designation load (can be empty or full list)
                dbcl.FillCombo(DDL_Designation, "select Designation_Type, Designation_DB from tlb_payroll_designation order by Id");

                dbcl.FillCombo(DDL_Education, "select Eduction_Type, EducationDB from tlb_education_types order by Id");
                dbcl.FillCombo(DDL_Role, "select Emp_PermissionText, Emp_PermissionValue from tlb_emp_roles_permission order by Id");
                dbcl.FillCombo(DDL_WorkHours, "select Work_HoursTypes, Work_Hours from tlb_payroll_hours");
                dbcl.FillCombo(DDL_OTFactor, "select OT_Type, OT_TypeValue from tlb_payroll_ottypes");
                dbcl.FillCombo(DDL_BankName, "select BankName, BankName as Val from tlb_IndianBanks order by BankName");
            }
            catch (Exception ex)
            {
                lbl_msg.Text = "DDL Load Error: " + ex.Message;
            }
        }

        protected void DDL_SkillCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cascade logic for designation
            string cat = DDL_SkillCat.SelectedValue;
            string query = "select Designation_Type, Designation_DB from tlb_payroll_designation where Category_DB='" + cat + "' order by Id";
            dbcl.FillCombo(DDL_Designation, query);
        }

        private void LoadEmployeeData(string wsl)
        {
            try
            {
                string query = "SELECT * FROM tbl_Employee_Mustertable WHERE WorkmanSL = @ID";
                SqlParameter[] p = { new SqlParameter("@ID", wsl) };
                DataTable dt = dbcl.SPreturn_dt(query, p);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    lbl_EmpNameHeader.Text = dr["FullName"].ToString();

                    // --- 1. Personal ---
                    txt_fname.Text = dr["FirstName"].ToString();
                    txt_mname.Text = dr["MiddleName"].ToString();
                    txt_lname.Text = dr["LastName"].ToString();
                    txt_fullname.Text = dr["FullName"].ToString();
                    txt_father.Text = dr["Fathername"].ToString();
                    txt_dob.Text = FormatDate(dr["DOB"]);
                    txt_blood.Text = dr["BloodGroup"].ToString();
                    txt_mobile.Text = dr["MobileNo"].ToString();
                    txt_email.Text = dr["Email"].ToString();
                    SetDDL(DDL_Education, dr["QualificationDB"].ToString());

                    // --- 2. PROFESSIONAL MAPPING (Updated) ---

                    // A. Organization Hierarchy (ReadOnly)
                    txt_Country.Text = dr["WorkCountry"].ToString();
                    txt_State.Text = dr["WorkState"].ToString();
                    txt_Region.Text = dr["WorkRegion"].ToString();
                    txt_Company.Text = dr["WorkCompany"].ToString();

                    // B. Job Details
                    SetDDL(DDL_Worksite, dr["Worksite_Code"].ToString());

                    string skillCatDB = dr["SkillCategoryDB"].ToString().Trim();
                    SetDDL(DDL_SkillCat, skillCatDB);

                    // Force-Load Designation based on Category
                    if (!string.IsNullOrEmpty(skillCatDB))
                    {
                        DDL_Designation.Items.Clear();
                        string desgQry = "select Designation_Type, Designation_DB from tlb_payroll_designation where Category_DB='" + skillCatDB + "' order by Id";
                        dbcl.FillCombo(DDL_Designation, desgQry);
                    }
                    SetDDL(DDL_Designation, dr["SkillDesignationDB"].ToString());

                    // C. Roles & Hours
                    // Ensure DDL_UserRole is populated in LoadDDLs() using: "select User_RoleType, UserRoleDB from tlb_UserRoles..."
                    // If not, we bind the text directly or assume it's loaded.
                    SetDDL(DDL_UserRole, dr["UserRoleDB"].ToString());
                    SetDDL(DDL_Role, dr["RolePermissionDB"].ToString());
                    SetDDL(DDL_WorkHours, dr["WorkHours"].ToString());
                    txt_doj.Text = FormatDate(dr["DOJ"]);

                    // D. System Access Logs
                    bool isLoginActive = (dr["LoginStatus"] != DBNull.Value && Convert.ToInt32(dr["LoginStatus"]) == 1);
                    lbl_LoginStatus.Text = isLoginActive ? "YES (Active)" : "NO (Locked)";
                    lbl_LoginStatus.CssClass = isLoginActive ? "badge badge-verified" : "badge badge-pending";

                    txt_LastLogin.Text = dr["LastLogin"] != DBNull.Value ? Convert.ToDateTime(dr["LastLogin"]).ToString("yyyy-MM-dd HH:mm:ss") : "Never";
                    txt_LastLogout.Text = dr["LastLogout"] != DBNull.Value ? Convert.ToDateTime(dr["LastLogout"]).ToString("yyyy-MM-dd HH:mm:ss") : "Never";

                    // --- 3. Payroll ---
                    txt_fixed_amt.Text = dr["FixedAmount"].ToString();
                    txt_da_vda.Text = dr["DA_VDA"].ToString();
                    txt_hra.Text = dr["HRA"].ToString();
                    txt_conv.Text = dr["Conv_Allowance"].ToString();
                    txt_medical.Text = dr["Medical_Allowance"].ToString();
                    txt_washing.Text = dr["Washing_Allowance"].ToString();
                    txt_att_bonus.Text = dr["ATT_Allowance"].ToString();
                    SetDDL(DDL_OTFactor, dr["OTFactor"].ToString());

                    // --- 4. Financial ---
                    txt_uan.Text = dr["UANNo"].ToString();
                    txt_esic.Text = dr["ESICNo"].ToString();
                    SetDDL(DDL_BankName, dr["Payment_Bank"].ToString());
                    txt_acc_no.Text = dr["Payment_Account"].ToString();
                    txt_ifsc.Text = dr["Payment_IFSC"].ToString();

                    // --- 5. Compliance ---
                    txt_rfid.Text = dr["SafetyPassNo"].ToString();
                    txt_rfid_val.Text = FormatDate(dr["SafetyPassExpiry"]);
                    txt_pv_val.Text = FormatDate(dr["PVExpiry"]);
                    txt_gp_no.Text = dr["GatePassNo"].ToString();
                    txt_gp_val.Text = FormatDate(dr["GatePassExpiry"]);
                    txt_plain_pass.Text = dr["LoginPassword_Plain"].ToString();
                    txt_sq1.Text = dr["SQ1"].ToString();
                    txt_sqans1.Text = dr["SQAns1"].ToString();

                    // --- 7. Admin Status ---
                    string status = dr["WorkStatus"].ToString();
                    lbl_CurrentStatus.Text = status;
                    SetDDL(DDL_AdminStatus, status);
                    SetDDL(DDL_LoginAccess, dr["LoginStatus"].ToString());

                    if (status == "Exited")
                    {
                        pnl_Exit.Visible = true;
                        txt_DOR.Text = FormatDate(dr["DOR"]);
                        SetDDL(DDL_ExitType, dr["ExitType"].ToString());
                    }
                }
            }
            catch (Exception ex) { ShowPopup("Load Error", "Unable to load data: " + ex.Message); }
        }

        private void LoadDocumentData(string wsl)
        {
            string query = "SELECT * FROM tbl_EmployeeDocsDetails WHERE WorkmanSL = @ID";
            SqlParameter[] p = { new SqlParameter("@ID", wsl) };
            DataTable dt = dbcl.SPreturn_dt(query, p);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                BindDocRow(dr["AadhaarYesNo"], dr["AddhaarPath"], dr["AddhaarDate"], lbl_AadhaarStat, lbl_AadhaarDate, lnk_ViewAadhaar, btn_RejectAadhaar);
                BindDocRow(dr["PanYesNo"], dr["PanPath"], dr["PanDate"], lbl_PanStat, lbl_PanDate, lnk_ViewPan, btn_RejectPan);
                BindDocRow(dr["BankYesNo"], dr["BankPath"], dr["BankDate"], lbl_BankStat, lbl_BankDate, lnk_ViewBank, btn_RejectBank);
            }
        }

        private void BindDocRow(object yesNo, object path, object date, Label lblSt, Label lblDt, HyperLink lnk, Button btn)
        {
            bool hasDoc = (yesNo != DBNull.Value && Convert.ToInt32(yesNo) == 1);
            if (hasDoc && path != DBNull.Value && path.ToString() != "NA")
            {
                lblSt.Text = "VERIFIED";
                lblSt.CssClass = "badge-verified";
                lblDt.Text = Convert.ToDateTime(date).ToString("dd-MMM-yyyy");
                lnk.NavigateUrl = ResolveUrl("~" + path.ToString());
                lnk.Visible = true;
                btn.Visible = true;
            }
            else
            {
                lblSt.Text = "PENDING / REJECTED";
                lblSt.CssClass = "badge-pending";
                lblDt.Text = "-";
                lnk.Visible = false;
                btn.Visible = false;
            }
        }

        protected void DDL_AdminStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnl_Exit.Visible = (DDL_AdminStatus.SelectedValue == "Exited");
        }

        protected void btn_SaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // 1. Update Master Table
                string query = @"UPDATE tbl_Employee_Mustertable SET 
                    FirstName=@FN, MiddleName=@MN, LastName=@LN, FullName=@FULL,
                    Fathername=@DAD, DOB=@DOB, BloodGroup=@BG, MobileNo=@MOB, Email=@EMAIL,
                    Qualification=@QUAL, QualificationDB=@QUALDB,
                    Worksite_Code=@WS, SkillCategoryDB=@SC, SkillDesignationDB=@DESG,
                    DOJ=@DOJ, WorkHours=@WH, RolePermissionDB=@ROLE,
                    FixedAmount=@FIX, DA_VDA=@DA, HRA=@HRA, Conv_Allowance=@CONV,
                    Medical_Allowance=@MED, Washing_Allowance=@WASH, ATT_Allowance=@ATT, OTFactor=@OT,
                    UANNo=@UAN, ESICNo=@ESIC, Payment_Bank=@BNK, Payment_Account=@ACC, Payment_IFSC=@IFSC,
                    SafetyPassNo=@RFID, SafetyPassExpiry=@RFIDVAL, PVExpiry=@PVVAL,
                    GatePassNo=@GP, GatePassExpiry=@GPVAL,
                    WorkStatus=@STAT, LoginStatus=@LOGIN, DOR=@DOR, ExitType=@ETYPE,
                    LastModifiedDate=GETDATE(), LastModifiedByName=@MODBY, LastModifiedByWrk=@MODWRK
                    WHERE WorkmanSL=@ID";

                SqlCommand cmd = new SqlCommand(query, dbcl.Conn);

                // Params
                cmd.Parameters.AddWithValue("@ID", lbl_EmpID.Text);
                cmd.Parameters.AddWithValue("@FN", txt_fname.Text);
                cmd.Parameters.AddWithValue("@MN", (object)txt_mname.Text ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@LN", txt_lname.Text);
                cmd.Parameters.AddWithValue("@FULL", txt_fname.Text + " " + txt_lname.Text);
                cmd.Parameters.AddWithValue("@DAD", txt_father.Text);
                cmd.Parameters.AddWithValue("@DOB", ParseDate(txt_dob.Text));
                cmd.Parameters.AddWithValue("@BG", txt_blood.Text);
                cmd.Parameters.AddWithValue("@MOB", txt_mobile.Text);
                cmd.Parameters.AddWithValue("@EMAIL", txt_email.Text);

                cmd.Parameters.AddWithValue("@QUAL", DDL_Education.SelectedItem != null ? DDL_Education.SelectedItem.Text : "");
                cmd.Parameters.AddWithValue("@QUALDB", DDL_Education.SelectedValue);

                cmd.Parameters.AddWithValue("@WS", DDL_Worksite.SelectedValue);
                cmd.Parameters.AddWithValue("@SC", DDL_SkillCat.SelectedValue);
                cmd.Parameters.AddWithValue("@DESG", DDL_Designation.SelectedValue);
                cmd.Parameters.AddWithValue("@DOJ", ParseDate(txt_doj.Text));
                cmd.Parameters.AddWithValue("@WH", DDL_WorkHours.SelectedValue);
                cmd.Parameters.AddWithValue("@ROLE", DDL_Role.SelectedValue);

                cmd.Parameters.AddWithValue("@FIX", DecimalSafe(txt_fixed_amt.Text));
                cmd.Parameters.AddWithValue("@DA", DecimalSafe(txt_da_vda.Text));
                cmd.Parameters.AddWithValue("@HRA", DecimalSafe(txt_hra.Text));
                cmd.Parameters.AddWithValue("@CONV", DecimalSafe(txt_conv.Text));
                cmd.Parameters.AddWithValue("@MED", DecimalSafe(txt_medical.Text));
                cmd.Parameters.AddWithValue("@WASH", DecimalSafe(txt_washing.Text));
                cmd.Parameters.AddWithValue("@ATT", DecimalSafe(txt_att_bonus.Text));
                cmd.Parameters.AddWithValue("@OT", DDL_OTFactor.SelectedValue);

                cmd.Parameters.AddWithValue("@UAN", txt_uan.Text);
                cmd.Parameters.AddWithValue("@ESIC", txt_esic.Text);
                cmd.Parameters.AddWithValue("@BNK", DDL_BankName.SelectedValue);
                cmd.Parameters.AddWithValue("@ACC", txt_acc_no.Text);
                cmd.Parameters.AddWithValue("@IFSC", txt_ifsc.Text);

                cmd.Parameters.AddWithValue("@RFID", txt_rfid.Text);
                cmd.Parameters.AddWithValue("@RFIDVAL", ParseDate(txt_rfid_val.Text));
                cmd.Parameters.AddWithValue("@PVVAL", ParseDate(txt_pv_val.Text));
                cmd.Parameters.AddWithValue("@GP", txt_gp_no.Text);
                cmd.Parameters.AddWithValue("@GPVAL", ParseDate(txt_gp_val.Text));

                // Admin
                cmd.Parameters.AddWithValue("@STAT", DDL_AdminStatus.SelectedValue);
                cmd.Parameters.AddWithValue("@LOGIN", DDL_LoginAccess.SelectedValue);
                cmd.Parameters.AddWithValue("@DOR", DDL_AdminStatus.SelectedValue == "Exited" ? ParseDate(txt_DOR.Text) : DBNull.Value);
                cmd.Parameters.AddWithValue("@ETYPE", DDL_AdminStatus.SelectedValue == "Exited" ? (object)DDL_ExitType.SelectedValue : DBNull.Value);

                cmd.Parameters.AddWithValue("@MODBY", Session["USERNAME"]);
                cmd.Parameters.AddWithValue("@MODWRK", Session["WORKMAN"]);

                cmd.ExecuteNonQuery();

                // 2. Audit Log
                LogAudit(lbl_EmpID.Text, "FULL_UPDATE", "Status: " + DDL_AdminStatus.SelectedValue + ". Reason: " + txt_AdminReason.Text);

                ShowPopup("Success", "Employee Record Updated Successfully!");
                LoadEmployeeData(lbl_EmpID.Text); // Refresh UI
            }
            catch (Exception ex)
            {
                ShowPopup("Error", ex.Message);
            }
            finally { dbcl.DisconnectDb(); }
        }

        protected void btn_RejectDoc_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string docType = btn.CommandArgument;
            string reason = txt_DocAdminNote.Text.Trim();

            if (string.IsNullOrEmpty(reason))
            {
                ShowPopup("Note Required", "Please enter a rejection reason in the note box above.");
                return;
            }

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // Determine columns dynamically
                string pathCol = docType == "Aadhaar" ? "AddhaarPath" : docType + "Path";
                string dateCol = docType == "Aadhaar" ? "AddhaarDate" : docType + "Date";
                string yesNoCol = docType + "YesNo";

                string query = string.Format(@"UPDATE tbl_EmployeeDocsDetails SET 
                                {0} = 0, {1} = NULL, {2} = NULL,
                                RejectionNote = @NOTE, UpdatedByWrk=@WRK, Timestamp=GETDATE()
                                WHERE WorkmanSL = @ID", yesNoCol, pathCol, dateCol);

                SqlCommand cmd = new SqlCommand(query, dbcl.Conn);
                cmd.Parameters.AddWithValue("@NOTE", reason);
                cmd.Parameters.AddWithValue("@WRK", Session["WORKMAN"]);
                cmd.Parameters.AddWithValue("@ID", lbl_EmpID.Text);

                cmd.ExecuteNonQuery();

                LogAudit(lbl_EmpID.Text, "DOC_REJECT", docType + " rejected. Reason: " + reason);
                LoadDocumentData(lbl_EmpID.Text); // Refresh UI
                txt_DocAdminNote.Text = "";
                ShowPopup("Rejected", "Document rejected. Employee will be asked to re-upload.");
            }
            catch (Exception ex) { ShowPopup("Error", ex.Message); }
            finally { dbcl.DisconnectDb(); }
        }

        // --- C# 5.0 Compatible Helpers ---

        private void LogAudit(string emp, string type, string details)
        {
            try
            {
                string logPath = Server.MapPath("~/bussiness/production/Logs/EmployeeEdits/");
                if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);

                // Fixed string interpolation for older C# versions
                string line = string.Format("{0:yyyy-MM-dd HH:mm:ss} | {1} | {2} | By: {3}{4}",
                                            DateTime.Now, type, details, Session["USERNAME"], Environment.NewLine);

                File.AppendAllText(Path.Combine(logPath, "EmpLog_" + emp + ".txt"), line);
            }
            catch { }
        }

        private object ParseDate(string dt)
        {
            DateTime d; // Fix: Declare variable outside
            if (DateTime.TryParse(dt, out d))
            {
                return d;
            }
            return DBNull.Value;
        }

        private string FormatDate(object dt)
        {
            if (dt != DBNull.Value && dt != null)
            {
                return Convert.ToDateTime(dt).ToString("yyyy-MM-dd");
            }
            return "";
        }

        private decimal DecimalSafe(string val)
        {
            decimal d; // Fix: Declare variable outside
            if (decimal.TryParse(val, out d))
            {
                return d;
            }
            return 0;
        }

        private void SetDDL(DropDownList ddl, string val)
        {
            // Fix: Add Trim() to handle database values with whitespace
            if (val != null)
            {
                string safeVal = val.Trim();
                if (ddl.Items.FindByValue(safeVal) != null)
                {
                    ddl.SelectedValue = safeVal;
                }
            }
        }

        private void ShowPopup(string title, string body)
        {
            string cleanBody = body.Replace("'", "").Replace("\n", " ");
            string script = string.Format("ShowPopup('{0}', '{1}');", title, cleanBody);
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
        }
    }
}