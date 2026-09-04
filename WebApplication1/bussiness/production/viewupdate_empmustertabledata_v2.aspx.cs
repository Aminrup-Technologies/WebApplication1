using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class viewupdate_empmustertabledata_v2 : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["WORKMAN"] == null)
            {
                Response.Redirect("~/login.aspx");
                return;
            }

            ApplyTabSecurity();

            if (!IsPostBack)
            {
                LoadGlobalDDLs();
                string id = Request.QueryString["ID"];

                if (!string.IsNullOrEmpty(id))
                {
                    lbl_EmpID.Text = id;
                    LoadEmployeeData(id);
                    LoadDocumentData(id);

                    // --- NEW: Load Audit Logs ---
                    LoadAuditLogs(id);
                }
                else
                {
                    ShowPopup("Error", "No Employee ID provided.");
                    btn_SaveAll.Enabled = false;
                }
            }
        }

        private void LoadAuditLogs(string empId)
        {
            try
            {
                string logPath = Server.MapPath("~/bussiness/production/Logs/EmployeeEdits/EmpLog_" + empId + ".txt");

                if (File.Exists(logPath))
                {
                    string fileContent = File.ReadAllText(logPath);

                    // Regex lookahead pattern to split the file by the DateTime prefix: "YYYY-MM-DD HH:mm:ss |"
                    // This ensures multi-line details stay grouped with their parent timestamp
                    string pattern = @"(?=\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2} \|)";
                    string[] entries = System.Text.RegularExpressions.Regex.Split(fileContent, pattern);

                    // Filter empties, reverse to show newest first
                    var reversedEntries = entries.Where(e => !string.IsNullOrWhiteSpace(e)).Reverse();

                    // Format with a nice visual separator
                    string divider = "<hr style='margin: 10px 0; border-top: 1px dashed #ccc;' />";
                    string formattedContent = string.Join(divider, reversedEntries);

                    // Replace literal newlines with HTML breaks
                    lit_AuditLogs.Text = formattedContent.Replace(Environment.NewLine, "<br/>").Replace("\n", "<br/>");
                }
                else
                {
                    lit_AuditLogs.Text = "<span class='text-muted'>No audit logs found for this employee. System is tracking changes from now on.</span>";
                }
            }
            catch (Exception ex)
            {
                lit_AuditLogs.Text = "<span class='text-danger'>Unable to load audit logs: " + ex.Message + "</span>";
            }
        }

        private void ApplyTabSecurity()
        {
            string currentUser = Session["WORKMAN"] != null ? Session["WORKMAN"].ToString().Trim().ToUpper() : "";
            string authConfig = ConfigurationManager.AppSettings["PayrollAuthorizedUsers"];
            bool canViewPayroll = false;

            if (!string.IsNullOrEmpty(authConfig))
            {
                var authorizedList = authConfig.Split(',').Select(x => x.Trim().ToUpper()).ToList();
                if (authorizedList.Contains(currentUser)) { canViewPayroll = true; }
            }

            if (!canViewPayroll)
            {
                lnk_payroll.InnerHtml = "<i class='fa fa-lock text-danger'></i> <span style='color:#b3b3b3;'>Payroll (Locked)</span>";
                lnk_payroll.Attributes.Remove("data-toggle");
                lnk_payroll.Attributes["href"] = "javascript:void(0);";
                lnk_payroll.Attributes.Add("onclick", "ShowAccessDenied(); return false;");
                tab_payroll.Visible = false;
            }
        }

        private void LoadGlobalDDLs()
        {
            try
            {
                BindStandardDDL(DDL_UserRole, "select Employee_Type, EmpType_Value from tlb_emp_roles order by Id", "Employee_Type", "EmpType_Value");
                BindStandardDDL(DDL_Education, "select Eduction_Type, EducationDB from tlb_education_types order by Id", "Eduction_Type", "EducationDB");
                BindStandardDDL(DDL_WorkHours, "select Work_HoursTypes, Work_Hours from tlb_payroll_hours order by Id", "Work_HoursTypes", "Work_Hours");
                BindStandardDDL(DDL_OTFactor, "select OT_Type, OT_TypeValue from tlb_payroll_ottypes order by Id", "OT_Type", "OT_TypeValue");
                BindStandardDDL(DDL_BankName, "select BankName, BankCode from tlb_IndianBanks order by BankName", "BankName", "BankCode");
            }
            catch (Exception ex) { lbl_msg.Text = "DDL Error: " + ex.Message; }
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

                    string picPath = dr["PrfPicPath"] != DBNull.Value ? dr["PrfPicPath"].ToString().Trim() : "";
                    if (!string.IsNullOrEmpty(picPath) && picPath != "NA")
                    {
                        img_ProfilePic.ImageUrl = ResolveUrl(picPath.Replace("\\", "/"));
                    }
                    else
                    {
                        img_ProfilePic.ImageUrl = ResolveUrl("~/images/placeholder.png");
                    }

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
                    txt_ContactUpdateOn.Text = FormatDate(dr["ContactUpdateOn"]);
                    SetDDL(DDL_Education, dr["QualificationDB"].ToString());

                    // --- 2. Job & Skills ---
                    string country = dr["WorkCountry"].ToString();
                    string state = dr["WorkState"].ToString();
                    string region = dr["WorkRegion"].ToString();
                    string comp = dr["WorkCompany"].ToString();

                    txt_Country.Text = country;
                    txt_State.Text = state;
                    txt_Region.Text = region;
                    txt_Company.Text = comp;

                    string qryWS = string.Format("select Worksite_Name, DB_Code from tlb_atsworksites where Country_Code = '{0}' and State_Code='{1}' and WorkRegion_Code='{2}' and Company_Code='{3}' order by Id", country, state, region, comp);
                    BindStandardDDL(DDL_Worksite, qryWS, "Worksite_Name", "DB_Code");
                    SetDDL(DDL_Worksite, dr["Worksite_Code"].ToString());

                    string qryCat = string.Format("select Category_Type, Category_DB from tlb_payroll_category where Country_Code = '{0}' and State_Code='{1}' and WorkRegion_Code='{2}' and Company_Code='{3}' order by Id", country, state, region, comp);
                    BindStandardDDL(DDL_SkillCat, qryCat, "Category_Type", "Category_DB");
                    string skillCatDB = dr["SkillCategoryDB"].ToString().Trim();
                    SetDDL(DDL_SkillCat, skillCatDB);

                    if (!string.IsNullOrEmpty(skillCatDB))
                    {
                        string qryDesg = string.Format("select Designation_Type, Designation_DB from tlb_payroll_designation where Country_Code='{0}' and State_Code='{1}' and WorkRegion_Code='{2}' and Company_Code='{3}' and Category_DB='{4}' order by Id", country, state, region, comp, skillCatDB);
                        BindStandardDDL(DDL_Designation, qryDesg, "Designation_Type", "Designation_DB");
                        SetDDL(DDL_Designation, dr["SkillDesignationDB"].ToString());
                    }

                    string uRoleDB = dr["UserRoleDB"].ToString().Trim();
                    SetDDL(DDL_UserRole, uRoleDB);
                    if (!string.IsNullOrEmpty(uRoleDB))
                    {
                        string qryRole = string.Format("select Emp_PermissionText, Emp_PermissionValue from tlb_emp_roles_permission where EmpType_Value = '{0}' order by Id", uRoleDB);
                        BindStandardDDL(DDL_Role, qryRole, "Emp_PermissionText", "Emp_PermissionValue");
                        SetDDL(DDL_Role, dr["RolePermissionDB"].ToString());
                    }

                    SetDDL(DDL_WorkHours, dr["WorkHours"].ToString());
                    txt_doj.Text = FormatDate(dr["DOJ"]);
                    txt_RegType.Text = dr["RegistrationType"].ToString();

                    // --- 3. Payroll ---
                    txt_fixed_amt.Text = dr["FixedAmount"].ToString();
                    txt_da_vda.Text = dr["DA_VDA"].ToString();
                    txt_hra.Text = dr["HRA"].ToString();
                    txt_conv.Text = dr["Conv_Allowance"].ToString();
                    txt_medical.Text = dr["Medical_Allowance"].ToString();
                    txt_washing.Text = dr["Washing_Allowance"].ToString();
                    txt_att_bonus.Text = dr["ATT_Allowance"].ToString();
                    txt_spcl.Text = dr["SPCL_Allowance"].ToString();
                    txt_misc.Text = dr["Misc_Earnings"].ToString();
                    txt_OTMult.Text = dr["OTMultiplier"].ToString();
                    txt_OTDiv.Text = dr["OT_Divisibility"].ToString();

                    SetDDL(DDL_OTFactor, dr["OTFactor"].ToString());
                    SetDDL(DDL_F16, dr["F16_YesNo"].ToString());
                    SetDDL(DDL_F17, dr["F17_YesNo"].ToString());
                    SetDDL(DDL_FixedSal, dr["FixedSalary_YesNo"].ToString());

                    txt_RemAdv.Text = dr["Rem_Advance"].ToString();
                    txt_CurAdv.Text = dr["Cur_Advance"].ToString();
                    txt_RemFines.Text = dr["Rem_Fines"].ToString();
                    txt_CurFines.Text = dr["Cur_Fines"].ToString();
                    txt_RemOthers.Text = dr["Rem_Others"].ToString();
                    txt_CurOthers.Text = dr["Cur_Others"].ToString();

                    // --- 4. Financial ---
                    txt_uan.Text = dr["UANNo"].ToString();
                    txt_esic.Text = dr["ESICNo"].ToString();
                    SetDDLByText(DDL_BankName, dr["Payment_Bank"].ToString());
                    txt_acc_no.Text = dr["Payment_Account"].ToString();
                    txt_ifsc.Text = dr["Payment_IFSC"].ToString();
                    txt_bankbranch.Text = dr["BankBranch"].ToString();
                    txt_BankUpd.Text = FormatDate(dr["BankUpdatedOn"]);
                    txt_BankUpdBy.Text = dr["BankUpdatedByName"].ToString();

                    // --- 5. Compliance ---
                    txt_rfid.Text = dr["SafetyPassNo"].ToString();
                    txt_rfid_val.Text = FormatDate(dr["SafetyPassExpiry"]);
                    txt_pv_val.Text = FormatDate(dr["PVExpiry"]);
                    txt_gp_no.Text = dr["GatePassNo"].ToString();
                    txt_gp_val.Text = FormatDate(dr["GatePassExpiry"]);

                    txt_GPModBy.Text = dr["GP_ModifierName"].ToString();
                    txt_GPModDt.Text = FormatDate(dr["GP_ModifiedDate"]);

                    string gpStatus = dr["GP_UpdateApproval"].ToString();
                    txt_GPAppr.Text = string.IsNullOrEmpty(gpStatus) ? "N/A" : gpStatus;

                    // Toggle Button Visibility & Colors based on Approval Status
                    if (gpStatus == "Pending")
                    {
                        txt_GPAppr.ForeColor = System.Drawing.Color.DarkOrange;
                        btn_ApproveGP.Visible = true;
                        btn_RejectGP.Visible = true;
                    }
                    else if (gpStatus == "Approved")
                    {
                        txt_GPAppr.ForeColor = System.Drawing.Color.Green;
                        btn_ApproveGP.Visible = false;
                        btn_RejectGP.Visible = false;
                    }
                    else if (gpStatus == "Rejected")
                    {
                        txt_GPAppr.ForeColor = System.Drawing.Color.Red;
                        btn_ApproveGP.Visible = false;
                        btn_RejectGP.Visible = false;
                    }
                    else
                    {
                        txt_GPAppr.ForeColor = System.Drawing.Color.Black;
                        btn_ApproveGP.Visible = false;
                        btn_RejectGP.Visible = false;
                    }

                    // --- 7. Security / Admin ---
                    txt_LoginID.Text = dr["LoginID"].ToString();
                    txt_plain_pass.Text = dr["LoginPassword_Plain"].ToString();
                    txt_sq1.Text = dr["SQ1"].ToString();
                    txt_sqans1.Text = dr["SQAns1"].ToString();
                    txt_sq2.Text = dr["SQ2"].ToString();
                    txt_sqans2.Text = dr["SQAns2"].ToString();

                    bool isOnline = (dr["LoginStatus"] != DBNull.Value && Convert.ToInt32(dr["LoginStatus"]) == 1);
                    lbl_LoginStatus.Text = isOnline ? "Online" : "Offline";
                    lbl_LoginStatus.CssClass = isOnline ? "badge badge-verified" : "badge badge-pending";
                    SetDDL(DDL_LoginAccess, dr["LoginStatus"].ToString());

                    txt_LastLogin.Text = dr["LastLogin"] != DBNull.Value ? Convert.ToDateTime(dr["LastLogin"]).ToString("yyyy-MM-dd HH:mm:ss") : "Never";
                    txt_LastLogout.Text = dr["LastLogout"] != DBNull.Value ? Convert.ToDateTime(dr["LastLogout"]).ToString("yyyy-MM-dd HH:mm:ss") : "Never";

                    txt_PassExpiry.Text = dr["PasswordExpiry"] != DBNull.Value ? Convert.ToDateTime(dr["PasswordExpiry"]).ToString("yyyy-MM-dd HH:mm:ss") : "N/A";
                    txt_PassUpdateDt.Text = dr["Pass_UpdateDate"] != DBNull.Value ? Convert.ToDateTime(dr["Pass_UpdateDate"]).ToString("yyyy-MM-dd HH:mm:ss") : "N/A";
                    txt_PassUpdByName.Text = dr["PassUpdatedByName"].ToString();
                    txt_PassUpdByWrk.Text = dr["PassUpdatedByWrk"].ToString();

                    bool mfaOn = dr.Table.Columns.Contains("MFAEnabled") && MfaAuthHelper.IsEnabled(dr["MFAEnabled"]);
                    SetDDL(DDL_MFAEnabled, mfaOn ? "1" : "0");
                    if (dr.Table.Columns.Contains("MFAMethod") && dr["MFAMethod"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["MFAMethod"].ToString()))
                    {
                        SetDDL(DDL_MFAMethod, dr["MFAMethod"].ToString().Trim());
                    }
                    else
                    {
                        SetDDL(DDL_MFAMethod, MfaAuthHelper.MethodEmailOtp);
                    }
                    txt_MFAEnforcedOn.Text = (dr.Table.Columns.Contains("MFAEnforcedOn") && dr["MFAEnforcedOn"] != DBNull.Value)
                        ? Convert.ToDateTime(dr["MFAEnforcedOn"]).ToString("yyyy-MM-dd HH:mm:ss") : "N/A";
                    txt_MFAEnforcedBy.Text = (dr.Table.Columns.Contains("MFAEnforcedBy") && dr["MFAEnforcedBy"] != DBNull.Value)
                        ? dr["MFAEnforcedBy"].ToString() : "N/A";
                    txt_MFALastVerified.Text = (dr.Table.Columns.Contains("MFALastVerified") && dr["MFALastVerified"] != DBNull.Value)
                        ? Convert.ToDateTime(dr["MFALastVerified"]).ToString("yyyy-MM-dd HH:mm:ss") : "Never";

                    string status = dr["WorkStatus"].ToString();
                    lbl_CurrentStatus.Text = status;
                    SetDDLByText(DDL_AdminStatus, status);

                    if (status == "InActive")
                    {
                        SetDDL(ddlChangeType, dr["StatusChangeType"].ToString());
                        hfSavedReason.Value = dr["StatusChangeReason"].ToString();
                        txt_StatusRemarks.Text = dr["StatusRemarks"].ToString();

                        if (dr["StatusChangeType"].ToString() == "Permanent")
                        {
                            txt_DOR.Text = FormatDate(dr["DOR"]);
                            txt_DOE.Text = FormatDate(dr["DOE"]);
                            txt_DORelief.Text = FormatDate(dr["DO_Relief"]);
                        }
                    }

                    CaptureOriginalValues();
                }
            }
            catch (Exception ex) { ShowPopup("Load Error", ex.Message); }
        }

        protected void btn_ApproveGP_Click(object sender, EventArgs e)
        {
            UpdateGPStatus("Approved");
        }

        protected void btn_RejectGP_Click(object sender, EventArgs e)
        {
            UpdateGPStatus("Rejected");
        }

        private void UpdateGPStatus(string newStatus)
        {
            try
            {
                dbcl.Sqlconnection();
                if (dbcl.Conn.State == ConnectionState.Closed) dbcl.ConnectDb();

                string query = @"UPDATE tbl_Employee_Mustertable 
                                 SET GP_UpdateApproval = @Status, 
                                     GP_ModifierName = @MODBY, 
                                     GP_ModifierWrk = @MODWRK, 
                                     GP_ModifiedDate = GETDATE() 
                                 WHERE WorkmanSL = @ID";

                SqlCommand cmd = new SqlCommand(query, dbcl.Conn);
                cmd.Parameters.AddWithValue("@Status", newStatus);
                cmd.Parameters.AddWithValue("@MODBY", Session["USERNAME"] != null ? Session["USERNAME"].ToString() : "SYSTEM");
                cmd.Parameters.AddWithValue("@MODWRK", Session["WORKMAN"] != null ? Session["WORKMAN"].ToString() : "SYSTEM");
                cmd.Parameters.AddWithValue("@ID", lbl_EmpID.Text);

                cmd.ExecuteNonQuery();

                // Audit Log
                LogAudit(lbl_EmpID.Text, "COMPLIANCE_" + newStatus.ToUpper(), "Compliance details marked as " + newStatus + " by HR.");

                ShowPopup("Success", "Compliance details have been successfully marked as " + newStatus + ".");

                // Refresh the UI
                LoadEmployeeData(lbl_EmpID.Text);
                LoadAuditLogs(lbl_EmpID.Text);
            }
            catch (Exception ex)
            {
                ShowPopup("Error", "Failed to update compliance status: " + ex.Message);
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        protected void DDL_SkillCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_SkillCat.SelectedIndex > 0)
            {
                string qry = string.Format("select Designation_Type, Designation_DB from tlb_payroll_designation where Country_Code='{0}' and State_Code='{1}' and WorkRegion_Code='{2}' and Company_Code='{3}' and Category_DB='{4}' order by Id",
                    txt_Country.Text, txt_State.Text, txt_Region.Text, txt_Company.Text, DDL_SkillCat.SelectedValue);
                BindStandardDDL(DDL_Designation, qry, "Designation_Type", "Designation_DB");
            }
            else { DDL_Designation.Items.Clear(); }
        }

        protected void DDL_UserRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_UserRole.SelectedIndex > 0)
            {
                string cmd = string.Format("select Emp_PermissionText, Emp_PermissionValue from tlb_emp_roles_permission where EmpType_Value = '{0}' order by Id", DDL_UserRole.SelectedValue);
                BindStandardDDL(DDL_Role, cmd, "Emp_PermissionText", "Emp_PermissionValue");
            }
            else { DDL_Role.Items.Clear(); }
        }

        private void LoadDocumentData(string wsl)
        {
            try
            {
                string query = "SELECT * FROM tbl_EmployeeDocsDetails WHERE WorkmanSL = @ID";
                SqlParameter[] p = { new SqlParameter("@ID", wsl) };
                DataTable dt = dbcl.SPreturn_dt(query, p);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    // Notice we are now passing the Status column as well!
                    BindDocRow(dr["AadhaarYesNo"], dr["AddhaarPath"], dr["AddhaarDate"], dr["AadhaarID"], dr["AadhaarStatus"], lbl_AadhaarStat, lbl_AadhaarDate, lbl_AadhaarDet, lnk_ViewAadhaar, btn_ApproveAadhaar, btn_RejectAadhaar);
                    BindDocRow(dr["PanYesNo"], dr["PanPath"], dr["PanDate"], dr["PanNo"], dr["PanStatus"], lbl_PanStat, lbl_PanDate, lbl_PanDet, lnk_ViewPan, btn_ApprovePan, btn_RejectPan);
                    BindDocRow(dr["BankYesNo"], dr["BankPath"], dr["BankDate"], DBNull.Value, dr["BankStatus"], lbl_BankStat, lbl_BankDate, lbl_BankDet, lnk_ViewBank, btn_ApproveBank, btn_RejectBank);
                    BindDocRow(dr["TenYesNo"], dr["TenPath"], dr["TenDate"], DBNull.Value, dr["TenStatus"], lbl_TenStat, lbl_TenDate, lbl_TenDet, lnk_ViewTen, btn_ApproveTen, btn_RejectTen);
                    BindDocRow(dr["TwelveYesNo"], dr["TwelvePath"], dr["TwelveDate"], DBNull.Value, dr["TwelveStatus"], lbl_TwelveStat, lbl_TwelveDate, lbl_TwelveDet, lnk_ViewTwelve, btn_ApproveTwelve, btn_RejectTwelve);
                    BindDocRow(dr["GraduationYesNo"], dr["GradPath"], dr["GradDate"], DBNull.Value, dr["GradStatus"], lbl_GradStat, lbl_GradDate, lbl_GradDet, lnk_ViewGrad, btn_ApproveGrad, btn_RejectGrad);

                    bool accepted = dr["NoticeAccepted"] != DBNull.Value && Convert.ToInt32(dr["NoticeAccepted"]) == 1;
                    txt_DocPolicy.Text = accepted ? "YES" : "NO";
                    txt_DocPolicyDt.Text = FormatDate(dr["NoticeTimesatmp"]);

                    bool bypassed = dr["IsBypassed"] != DBNull.Value && Convert.ToBoolean(dr["IsBypassed"]);
                    txt_DocBypass.Text = bypassed ? "YES (Exempt)" : "NO";
                    txt_DocSkipDt.Text = FormatDate(dr["SkipDate"]);
                }
            }
            catch { }
        }

        private void BindDocRow(object yesNo, object path, object date, object detailId, object docStatusObj, Label lblSt, Label lblDt, Label lblDet, HyperLink lnk, Button btnApprove, Button btnReject)
        {
            if (detailId != DBNull.Value && detailId != null && !string.IsNullOrEmpty(detailId.ToString()))
                lblDet.Text = detailId.ToString();
            else
                lblDet.Text = "--";

            bool hasDoc = (yesNo != DBNull.Value && Convert.ToInt32(yesNo) == 1);
            string docStatus = (docStatusObj != DBNull.Value && docStatusObj != null) ? docStatusObj.ToString() : "Pending";

            if (hasDoc && path != DBNull.Value && path.ToString() != "NA" && !string.IsNullOrEmpty(path.ToString()))
            {
                lblDt.Text = Convert.ToDateTime(date).ToString("dd-MMM-yyyy");

                string cleanPath = path.ToString().Replace("\\", "/");
                lnk.NavigateUrl = cleanPath.StartsWith("~") ? ResolveUrl(cleanPath) : ResolveUrl("~" + cleanPath);
                lnk.Visible = true;

                if (docStatus == "Approved")
                {
                    lblSt.Text = "APPROVED";
                    lblSt.CssClass = "badge-verified";
                    btnApprove.Visible = false; // Hide approve button if already approved
                    btnReject.Visible = true;   // Can still reject later if an issue is found
                }
                else
                {
                    lblSt.Text = "PENDING REVIEW";
                    lblSt.CssClass = "badge-pending";
                    btnApprove.Visible = true;
                    btnReject.Visible = true;
                }
            }
            else
            {
                lblDt.Text = "-";
                lnk.Visible = false;
                btnApprove.Visible = false;
                btnReject.Visible = false;

                if (docStatus == "Rejected")
                {
                    lblSt.Text = "REJECTED";
                    lblSt.CssClass = "badge-rejected";
                }
                else
                {
                    lblSt.Text = "NOT UPLOADED";
                    lblSt.CssClass = "badge-missing";
                }
            }
        }

        protected void btn_ApproveDoc_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string docType = btn.CommandArgument;

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                string statusCol = "";
                switch (docType)
                {
                    case "Aadhaar": statusCol = "AadhaarStatus"; break;
                    case "Pan": statusCol = "PanStatus"; break;
                    case "Bank": statusCol = "BankStatus"; break;
                    case "Ten": statusCol = "TenStatus"; break;
                    case "Twelve": statusCol = "TwelveStatus"; break;
                    case "Grad": statusCol = "GradStatus"; break;
                }

                string query = string.Format(@"UPDATE tbl_EmployeeDocsDetails SET 
                                {0} = 'Approved', UpdatedByWrk=@WRK, Timestamp=GETDATE()
                                WHERE WorkmanSL = @ID", statusCol);

                SqlCommand cmd = new SqlCommand(query, dbcl.Conn);
                cmd.Parameters.AddWithValue("@WRK", Session["WORKMAN"]);
                cmd.Parameters.AddWithValue("@ID", lbl_EmpID.Text);

                cmd.ExecuteNonQuery();

                LogAudit(lbl_EmpID.Text, "DOC_APPROVE", string.Format("{0} document approved.", docType));
                LoadDocumentData(lbl_EmpID.Text);
                LoadAuditLogs(lbl_EmpID.Text);
                ShowPopup("Approved", "Document has been marked as Approved.");
            }
            catch (Exception ex) { ShowPopup("Error", ex.Message); }
            finally { dbcl.DisconnectDb(); }
        }

        protected void btn_RejectDoc_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string docType = btn.CommandArgument;
            string reason = txt_DocAdminNote.Text.Trim();

            if (string.IsNullOrEmpty(reason))
            {
                ShowPopup("Note Required", "Please enter a rejection reason in the note box before rejecting.");
                return;
            }

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                string pathCol = "";
                string dateCol = "";
                string yesNoCol = "";
                string statusCol = "";

                switch (docType)
                {
                    case "Aadhaar": pathCol = "AddhaarPath"; dateCol = "AddhaarDate"; yesNoCol = "AadhaarYesNo"; statusCol = "AadhaarStatus"; break;
                    case "Pan": pathCol = "PanPath"; dateCol = "PanDate"; yesNoCol = "PanYesNo"; statusCol = "PanStatus"; break;
                    case "Bank": pathCol = "BankPath"; dateCol = "BankDate"; yesNoCol = "BankYesNo"; statusCol = "BankStatus"; break;
                    case "Ten": pathCol = "TenPath"; dateCol = "TenDate"; yesNoCol = "TenYesNo"; statusCol = "TenStatus"; break;
                    case "Twelve": pathCol = "TwelvePath"; dateCol = "TwelveDate"; yesNoCol = "TwelveYesNo"; statusCol = "TwelveStatus"; break;
                    case "Grad": pathCol = "GradPath"; dateCol = "GradDate"; yesNoCol = "GraduationYesNo"; statusCol = "GradStatus"; break;
                }

                // YesNo becomes 0 so the employee login trigger forces them to upload it again!
                // Path becomes NULL so the broken file is unlinked.
                string query = string.Format(@"UPDATE tbl_EmployeeDocsDetails SET 
                                {0} = 0, {1} = NULL, {2} = NULL, {3} = 'Rejected',
                                RejectionNote = @NOTE, UpdatedByWrk=@WRK, Timestamp=GETDATE()
                                WHERE WorkmanSL = @ID", yesNoCol, pathCol, dateCol, statusCol);

                SqlCommand cmd = new SqlCommand(query, dbcl.Conn);
                cmd.Parameters.AddWithValue("@NOTE", string.Format("[{0} Rejected] {1}", docType, reason));
                cmd.Parameters.AddWithValue("@WRK", Session["WORKMAN"]);
                cmd.Parameters.AddWithValue("@ID", lbl_EmpID.Text);

                cmd.ExecuteNonQuery();

                LogAudit(lbl_EmpID.Text, "DOC_REJECT", string.Format("{0} rejected. Reason: {1}", docType, reason));
                LoadDocumentData(lbl_EmpID.Text);
                LoadAuditLogs(lbl_EmpID.Text);
                txt_DocAdminNote.Text = "";
                ShowPopup("Rejected", "Document rejected successfully. The employee will be forced to re-upload on next login.");
            }
            catch (Exception ex) { ShowPopup("Error", ex.Message); }
            finally { dbcl.DisconnectDb(); }
        }

        protected void btn_SaveAll_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_AdminReason.Text.Trim()))
            {
                ShowPopup("Audit Required", "Please enter an Audit Log Entry reason in the Admin Tab to save changes.");
                return;
            }

            bool mfaOn = DDL_MFAEnabled.SelectedValue == "1";
            if (mfaOn && !MfaAuthHelper.HasEmail(txt_email.Text))
            {
                ShowPopup("MFA Requires Email", "A registered Email on the Personal tab is required before MFA can be enabled for this user.");
                return;
            }

            string changes = GetAllFieldChanges();
            if (string.IsNullOrEmpty(changes))
            {
                ShowPopup("Info", "No data changes were detected.");
                return;
            }

            string status = DDL_AdminStatus.SelectedValue;
            string savedReason = hfSavedReason.Value;
            bool wasMfaOn = false;
            Dictionary<string, string> originalMfa = ViewState["ORIGINAL_DATA"] as Dictionary<string, string>;
            if (originalMfa != null && originalMfa.ContainsKey("MFAEnabled") && originalMfa["MFAEnabled"] == "1")
            {
                wasMfaOn = true;
            }
            string mfaEnforceSql = (mfaOn && !wasMfaOn)
                ? ", MFAEnforcedOn=GETDATE(), MFAEnforcedBy=@MFAADMIN"
                : "";

            try
            {
                dbcl.Sqlconnection();
                if (dbcl.Conn.State == ConnectionState.Closed) dbcl.ConnectDb();

                string query = string.Format(@"UPDATE tbl_Employee_Mustertable SET 
                    FirstName=@FN, MiddleName=@MN, LastName=@LN, FullName=@FULL,
                    Fathername=@DAD, DOB=@DOB, BloodGroup=@BG, MobileNo=@MOB, Email=@EMAIL,
                    
                    Qualification=@QUALTEXT, QualificationDB=@QUALDB,
                    WorkSite=@WSTEXT, Worksite_Code=@WS, 
                    SkillCategory=@SCTEXT, SkillCategoryDB=@SC, 
                    SkillDesignation=@DESGTEXT, SkillDesignationDB=@DESG,
                    User_RoleType=@UROLETEXT, UserRoleDB=@UROLE, 
                    Role_Permission=@ROLETEXT, RolePermissionDB=@ROLE, 
                    WorkHours=@WH, DOJ=@DOJ, 
                    
                    F16_YesNo=@F16, F17_YesNo=@F17, FixedSalary_YesNo=@FSAL,
                    FixedAmount=@FIX, DA_VDA=@DA, HRA=@HRA, Conv_Allowance=@CONV,
                    Medical_Allowance=@MED, Washing_Allowance=@WASH, ATT_Allowance=@ATT, 
                    SPCL_Allowance=@SPCL, Misc_Earnings=@MISC, OTFactor=@OT, OTMultiplier=@OTM, OT_Divisibility=@OTD,
                    
                    UANNo=@UAN, ESICNo=@ESIC, Payment_Bank=@BNK, Payment_Account=@ACC, Payment_IFSC=@IFSC, BankBranch=@BBRANCH,
                    
                    SafetyPassNo=@RFID, SafetyPassExpiry=@RFIDVAL, PVExpiry=@PVVAL,
                    GatePassNo=@GP, GatePassExpiry=@GPVAL, GP_UpdateApproval='Approved',
                    
                    SQ1=@SQ1, SQAns1=@SQA1, SQ2=@SQ2, SQAns2=@SQA2, LoginPassword_Plain=@LPP,
                    WorkStatus=@STAT, LoginStatus=@LOGIN, 
                    MFAEnabled=@MFA, MFAMethod=@MFAMETHOD{0},
                    
                    StatusChangeType=@SCTYPE, StatusChangeReason=@SCREAS, StatusRemarks=@SCREM,
                    StatusChangedByWrk=@SCWRK, StatusChangedDate=GETDATE(),
                    
                    DOR=@DOR, DOE=@DOE, DO_Relief=@DORELIEF, ExitType=@ETYPE, ExitInitiatedByWrk=@EINIT,

                    LastModifiedDate=GETDATE(), LastModifiedByName=@MODBY, LastModifiedByWrk=@MODWRK
                    WHERE WorkmanSL=@ID", mfaEnforceSql);

                SqlCommand cmd = new SqlCommand(query, dbcl.Conn);

                cmd.Parameters.AddWithValue("@ID", lbl_EmpID.Text);
                cmd.Parameters.AddWithValue("@FN", txt_fname.Text);
                cmd.Parameters.AddWithValue("@MN", string.IsNullOrEmpty(txt_mname.Text) ? (object)DBNull.Value : txt_mname.Text);
                cmd.Parameters.AddWithValue("@LN", string.IsNullOrEmpty(txt_lname.Text) ? (object)DBNull.Value : txt_lname.Text);

                string fullname = txt_fname.Text;
                if (!string.IsNullOrEmpty(txt_mname.Text)) fullname += " " + txt_mname.Text;
                if (!string.IsNullOrEmpty(txt_lname.Text)) fullname += " " + txt_lname.Text;
                cmd.Parameters.AddWithValue("@FULL", fullname);

                cmd.Parameters.AddWithValue("@DAD", txt_father.Text);
                cmd.Parameters.AddWithValue("@DOB", ParseDate(txt_dob.Text));
                cmd.Parameters.AddWithValue("@BG", txt_blood.Text);
                cmd.Parameters.AddWithValue("@MOB", txt_mobile.Text);
                cmd.Parameters.AddWithValue("@EMAIL", txt_email.Text);

                cmd.Parameters.AddWithValue("@QUALTEXT", DDL_Education.SelectedIndex > 0 ? DDL_Education.SelectedItem.Text : "");
                cmd.Parameters.AddWithValue("@QUALDB", DDL_Education.SelectedValue);
                cmd.Parameters.AddWithValue("@WSTEXT", DDL_Worksite.SelectedIndex > 0 ? DDL_Worksite.SelectedItem.Text : "");
                cmd.Parameters.AddWithValue("@WS", DDL_Worksite.SelectedValue);
                cmd.Parameters.AddWithValue("@SCTEXT", DDL_SkillCat.SelectedIndex > 0 ? DDL_SkillCat.SelectedItem.Text : "");
                cmd.Parameters.AddWithValue("@SC", DDL_SkillCat.SelectedValue);
                cmd.Parameters.AddWithValue("@DESGTEXT", DDL_Designation.SelectedIndex > 0 ? DDL_Designation.SelectedItem.Text : "");
                cmd.Parameters.AddWithValue("@DESG", DDL_Designation.SelectedValue);
                cmd.Parameters.AddWithValue("@UROLETEXT", DDL_UserRole.SelectedIndex > 0 ? DDL_UserRole.SelectedItem.Text : "");
                cmd.Parameters.AddWithValue("@UROLE", DDL_UserRole.SelectedValue);
                cmd.Parameters.AddWithValue("@ROLETEXT", DDL_Role.SelectedIndex > 0 ? DDL_Role.SelectedItem.Text : "");
                cmd.Parameters.AddWithValue("@ROLE", DDL_Role.SelectedValue);

                cmd.Parameters.AddWithValue("@WH", DDL_WorkHours.SelectedValue);
                cmd.Parameters.AddWithValue("@DOJ", ParseDate(txt_doj.Text));

                cmd.Parameters.AddWithValue("@F16", DDL_F16.SelectedValue);
                cmd.Parameters.AddWithValue("@F17", DDL_F17.SelectedValue);
                cmd.Parameters.AddWithValue("@FSAL", DDL_FixedSal.SelectedValue);
                cmd.Parameters.AddWithValue("@FIX", DecimalSafe(txt_fixed_amt.Text));
                cmd.Parameters.AddWithValue("@DA", DecimalSafe(txt_da_vda.Text));
                cmd.Parameters.AddWithValue("@HRA", DecimalSafe(txt_hra.Text));
                cmd.Parameters.AddWithValue("@CONV", DecimalSafe(txt_conv.Text));
                cmd.Parameters.AddWithValue("@MED", DecimalSafe(txt_medical.Text));
                cmd.Parameters.AddWithValue("@WASH", DecimalSafe(txt_washing.Text));
                cmd.Parameters.AddWithValue("@ATT", DecimalSafe(txt_att_bonus.Text));
                cmd.Parameters.AddWithValue("@SPCL", DecimalSafe(txt_spcl.Text));
                cmd.Parameters.AddWithValue("@MISC", DecimalSafe(txt_misc.Text));
                cmd.Parameters.AddWithValue("@OT", DDL_OTFactor.SelectedValue);
                cmd.Parameters.AddWithValue("@OTM", txt_OTMult.Text);
                cmd.Parameters.AddWithValue("@OTD", string.IsNullOrEmpty(txt_OTDiv.Text) ? "8" : txt_OTDiv.Text);

                cmd.Parameters.AddWithValue("@UAN", txt_uan.Text);
                cmd.Parameters.AddWithValue("@ESIC", txt_esic.Text);
                cmd.Parameters.AddWithValue("@BNK", DDL_BankName.SelectedIndex > 0 ? DDL_BankName.SelectedItem.Text : "");
                cmd.Parameters.AddWithValue("@ACC", txt_acc_no.Text);
                cmd.Parameters.AddWithValue("@IFSC", txt_ifsc.Text);
                cmd.Parameters.AddWithValue("@BBRANCH", txt_bankbranch.Text);

                cmd.Parameters.AddWithValue("@RFID", txt_rfid.Text);
                cmd.Parameters.AddWithValue("@RFIDVAL", ParseDate(txt_rfid_val.Text));
                cmd.Parameters.AddWithValue("@PVVAL", ParseDate(txt_pv_val.Text));
                cmd.Parameters.AddWithValue("@GP", txt_gp_no.Text);
                cmd.Parameters.AddWithValue("@GPVAL", ParseDate(txt_gp_val.Text));

                cmd.Parameters.AddWithValue("@SQ1", txt_sq1.Text);
                cmd.Parameters.AddWithValue("@SQA1", txt_sqans1.Text);
                cmd.Parameters.AddWithValue("@SQ2", txt_sq2.Text);
                cmd.Parameters.AddWithValue("@SQA2", txt_sqans2.Text);
                cmd.Parameters.AddWithValue("@LPP", txt_plain_pass.Text);

                cmd.Parameters.AddWithValue("@STAT", status);
                cmd.Parameters.AddWithValue("@LOGIN", DDL_LoginAccess.SelectedValue);
                cmd.Parameters.AddWithValue("@MFA", mfaOn ? 1 : 0);
                cmd.Parameters.AddWithValue("@MFAMETHOD", MfaAuthHelper.MethodEmailOtp);

                string safeUserWrk = Session["WORKMAN"] != null ? Session["WORKMAN"].ToString() : "SYSTEM";
                string safeUserName = Session["USERNAME"] != null ? Session["USERNAME"].ToString() : "SYSTEM";
                if (mfaOn && !wasMfaOn)
                {
                    cmd.Parameters.AddWithValue("@MFAADMIN", safeUserWrk);
                }

                if (status == "InActive")
                {
                    if (string.IsNullOrEmpty(txt_StatusRemarks.Text.Trim()))
                    {
                        ShowPopup("Validation Error", "Status Remarks are required when setting an employee to InActive.");
                        dbcl.DisconnectDb();
                        return;
                    }

                    cmd.Parameters.AddWithValue("@SCTYPE", ddlChangeType.SelectedValue);
                    string finalReason = Request.Form[hfSavedReason.UniqueID] ?? savedReason;
                    cmd.Parameters.AddWithValue("@SCREAS", finalReason);
                    cmd.Parameters.AddWithValue("@SCREM", txt_StatusRemarks.Text.Trim());
                    cmd.Parameters.AddWithValue("@SCWRK", safeUserWrk);

                    if (ddlChangeType.SelectedValue == "Permanent")
                    {
                        cmd.Parameters.AddWithValue("@DOR", ParseDate(txt_DOR.Text));
                        cmd.Parameters.AddWithValue("@DOE", ParseDate(txt_DOE.Text));
                        cmd.Parameters.AddWithValue("@DORELIEF", ParseDate(txt_DORelief.Text));
                        cmd.Parameters.AddWithValue("@ETYPE", finalReason);
                        cmd.Parameters.AddWithValue("@EINIT", safeUserWrk);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DOR", DBNull.Value);
                        cmd.Parameters.AddWithValue("@DOE", DBNull.Value);
                        cmd.Parameters.AddWithValue("@DORELIEF", DBNull.Value);
                        cmd.Parameters.AddWithValue("@ETYPE", DBNull.Value);
                        cmd.Parameters.AddWithValue("@EINIT", DBNull.Value);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SCTYPE", DBNull.Value);
                    cmd.Parameters.AddWithValue("@SCREAS", DBNull.Value);
                    cmd.Parameters.AddWithValue("@SCREM", DBNull.Value);
                    cmd.Parameters.AddWithValue("@SCWRK", DBNull.Value);
                    cmd.Parameters.AddWithValue("@DOR", DBNull.Value);
                    cmd.Parameters.AddWithValue("@DOE", DBNull.Value);
                    cmd.Parameters.AddWithValue("@DORELIEF", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ETYPE", DBNull.Value);
                    cmd.Parameters.AddWithValue("@EINIT", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@MODBY", safeUserName);
                cmd.Parameters.AddWithValue("@MODWRK", safeUserWrk);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ShowPopup("Database Error", "Unable to save changes: " + ex.Message);
                return;
            }
            finally
            {
                dbcl.DisconnectDb();
            }

            try
            {
                string auditDetail = string.Format("Reason: {0}{1}{1}CHANGES DETECTED:{1}{2}", txt_AdminReason.Text, Environment.NewLine, changes);
                LogAudit(lbl_EmpID.Text, "MASTER_UPDATE", auditDetail);

                txt_AdminReason.Text = "";
                ShowPopup("Success", "Employee Record Updated Successfully! Audit Log generated.");

                LoadEmployeeData(lbl_EmpID.Text);
                LoadAuditLogs(lbl_EmpID.Text);
            }
            catch (Exception ex)
            {
                ShowPopup("Refresh Error", "Data saved, but UI failed to refresh: " + ex.Message);
            }
        }

        // =========================================================
        // --- AUDIT AND UTILITY HELPERS ---
        // =========================================================

        private void CaptureOriginalValues()
        {
            Dictionary<string, string> original = new Dictionary<string, string>();

            original["FirstName"] = txt_fname.Text;
            original["MiddleName"] = txt_mname.Text;
            original["LastName"] = txt_lname.Text;
            original["FullName"] = txt_fullname.Text;
            original["FatherName"] = txt_father.Text;
            original["DOB"] = txt_dob.Text;
            original["BloodGroup"] = txt_blood.Text;
            original["MobileNo"] = txt_mobile.Text;
            original["Email"] = txt_email.Text;
            original["QualificationDB"] = DDL_Education.SelectedValue;

            original["Worksite_Code"] = DDL_Worksite.SelectedValue;
            original["SkillCategoryDB"] = DDL_SkillCat.SelectedValue;
            original["SkillDesignationDB"] = DDL_Designation.SelectedValue;
            original["UserRoleDB"] = DDL_UserRole.SelectedValue;
            original["RolePermissionDB"] = DDL_Role.SelectedValue;
            original["WorkHours"] = DDL_WorkHours.SelectedValue;
            original["DOJ"] = txt_doj.Text;

            original["FixedAmount"] = txt_fixed_amt.Text;
            original["DA_VDA"] = txt_da_vda.Text;
            original["HRA"] = txt_hra.Text;
            original["Conv_Allowance"] = txt_conv.Text;
            original["Medical_Allowance"] = txt_medical.Text;
            original["Washing_Allowance"] = txt_washing.Text;
            original["ATT_Allowance"] = txt_att_bonus.Text;
            original["SPCL_Allowance"] = txt_spcl.Text;
            original["Misc_Earnings"] = txt_misc.Text;
            original["OTFactor"] = DDL_OTFactor.SelectedValue;
            original["OTMultiplier"] = txt_OTMult.Text;
            original["OT_Divisibility"] = txt_OTDiv.Text;
            original["F16_YesNo"] = DDL_F16.SelectedValue;
            original["F17_YesNo"] = DDL_F17.SelectedValue;
            original["FixedSalary_YesNo"] = DDL_FixedSal.SelectedValue;

            original["UANNo"] = txt_uan.Text;
            original["ESICNo"] = txt_esic.Text;
            string currentBank = DDL_BankName.SelectedIndex > 0 ? DDL_BankName.SelectedItem.Text : "";
            original["Payment_Bank"] = currentBank;
            original["Payment_Account"] = txt_acc_no.Text;
            original["Payment_IFSC"] = txt_ifsc.Text;
            original["BankBranch"] = txt_bankbranch.Text;

            original["SafetyPassNo"] = txt_rfid.Text;
            original["SafetyPassExpiry"] = txt_rfid_val.Text;
            original["PVExpiry"] = txt_pv_val.Text;
            original["GatePassNo"] = txt_gp_no.Text;
            original["GatePassExpiry"] = txt_gp_val.Text;

            original["LoginPassword_Plain"] = txt_plain_pass.Text;
            original["WorkStatus"] = DDL_AdminStatus.SelectedValue;
            original["LoginStatus"] = DDL_LoginAccess.SelectedValue;
            original["MFAEnabled"] = DDL_MFAEnabled.SelectedValue;

            original["StatusReason"] = hfSavedReason.Value;
            original["DOR"] = txt_DOR.Text;
            original["DOE"] = txt_DOE.Text;
            original["DO_Relief"] = txt_DORelief.Text;

            // ExitType is tracked dynamically by JS reason drop-down in this version
            original["ExitType"] = hfSavedReason.Value;

            ViewState["ORIGINAL_DATA"] = original;
        }

        private string GetAllFieldChanges()
        {
            if (ViewState["ORIGINAL_DATA"] == null) return "";

            Dictionary<string, string> original = (Dictionary<string, string>)ViewState["ORIGINAL_DATA"];
            List<string> changes = new List<string>();

            CompareValue(changes, original, "FirstName", txt_fname.Text);
            CompareValue(changes, original, "MiddleName", txt_mname.Text);
            CompareValue(changes, original, "LastName", txt_lname.Text);
            CompareValue(changes, original, "FullName", txt_fullname.Text);
            CompareValue(changes, original, "FatherName", txt_father.Text);
            CompareValue(changes, original, "DOB", txt_dob.Text);
            CompareValue(changes, original, "BloodGroup", txt_blood.Text);
            CompareValue(changes, original, "MobileNo", txt_mobile.Text);
            CompareValue(changes, original, "Email", txt_email.Text);
            CompareValue(changes, original, "QualificationDB", DDL_Education.SelectedValue);

            CompareValue(changes, original, "Worksite_Code", DDL_Worksite.SelectedValue);
            CompareValue(changes, original, "SkillCategoryDB", DDL_SkillCat.SelectedValue);
            CompareValue(changes, original, "SkillDesignationDB", DDL_Designation.SelectedValue);
            CompareValue(changes, original, "UserRoleDB", DDL_UserRole.SelectedValue);
            CompareValue(changes, original, "RolePermissionDB", DDL_Role.SelectedValue);
            CompareValue(changes, original, "WorkHours", DDL_WorkHours.SelectedValue);
            CompareValue(changes, original, "DOJ", txt_doj.Text);

            CompareValue(changes, original, "FixedAmount", txt_fixed_amt.Text);
            CompareValue(changes, original, "DA_VDA", txt_da_vda.Text);
            CompareValue(changes, original, "HRA", txt_hra.Text);
            CompareValue(changes, original, "Conv_Allowance", txt_conv.Text);
            CompareValue(changes, original, "Medical_Allowance", txt_medical.Text);
            CompareValue(changes, original, "Washing_Allowance", txt_washing.Text);
            CompareValue(changes, original, "ATT_Allowance", txt_att_bonus.Text);
            CompareValue(changes, original, "SPCL_Allowance", txt_spcl.Text);
            CompareValue(changes, original, "Misc_Earnings", txt_misc.Text);
            CompareValue(changes, original, "OTFactor", DDL_OTFactor.SelectedValue);
            CompareValue(changes, original, "OTMultiplier", txt_OTMult.Text);
            CompareValue(changes, original, "OT_Divisibility", txt_OTDiv.Text);
            CompareValue(changes, original, "F16_YesNo", DDL_F16.SelectedValue);
            CompareValue(changes, original, "F17_YesNo", DDL_F17.SelectedValue);
            CompareValue(changes, original, "FixedSalary_YesNo", DDL_FixedSal.SelectedValue);

            CompareValue(changes, original, "UANNo", txt_uan.Text);
            CompareValue(changes, original, "ESICNo", txt_esic.Text);
            string currentBank = DDL_BankName.SelectedIndex > 0 ? DDL_BankName.SelectedItem.Text : "";
            CompareValue(changes, original, "Payment_Bank", currentBank);
            CompareValue(changes, original, "Payment_Account", txt_acc_no.Text);
            CompareValue(changes, original, "Payment_IFSC", txt_ifsc.Text);
            CompareValue(changes, original, "BankBranch", txt_bankbranch.Text);

            CompareValue(changes, original, "SafetyPassNo", txt_rfid.Text);
            CompareValue(changes, original, "SafetyPassExpiry", txt_rfid_val.Text);
            CompareValue(changes, original, "PVExpiry", txt_pv_val.Text);
            CompareValue(changes, original, "GatePassNo", txt_gp_no.Text);
            CompareValue(changes, original, "GatePassExpiry", txt_gp_val.Text);

            CompareValue(changes, original, "LoginPassword_Plain", txt_plain_pass.Text);
            CompareValue(changes, original, "WorkStatus", DDL_AdminStatus.SelectedValue);
            CompareValue(changes, original, "LoginStatus", DDL_LoginAccess.SelectedValue);
            CompareValue(changes, original, "MFAEnabled", DDL_MFAEnabled.SelectedValue);

            string savedReason = Request.Form[hfSavedReason.UniqueID] ?? hfSavedReason.Value;
            CompareValue(changes, original, "StatusReason", savedReason);
            CompareValue(changes, original, "DOR", txt_DOR.Text);
            CompareValue(changes, original, "DOE", txt_DOE.Text);
            CompareValue(changes, original, "DO_Relief", txt_DORelief.Text);
            CompareValue(changes, original, "ExitType", savedReason);

            return string.Join(Environment.NewLine, changes);
        }

        private void CompareValue(List<string> changes, Dictionary<string, string> original, string key, string currentValue)
        {
            string oldValue = original.ContainsKey(key) ? (original[key] ?? "").Trim() : "";
            string newValue = (currentValue ?? "").Trim();

            if (oldValue != newValue)
            {
                changes.Add(string.Format("{0}: '{1}' -> '{2}'", key, oldValue, newValue));
            }
        }

        private void BindStandardDDL(DropDownList ddl, string query, string textField, string valueField)
        {
            try
            {
                ddl.Items.Clear();
                dbcl.Sqlconnection();
                if (dbcl.Conn.State == ConnectionState.Closed) dbcl.ConnectDb();

                using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dtBind = new DataTable();
                        da.Fill(dtBind);
                        if (dtBind.Rows.Count > 0)
                        {
                            ddl.DataSource = dtBind;
                            ddl.DataTextField = textField;
                            ddl.DataValueField = valueField;
                            ddl.DataBind();
                        }
                    }
                }
                ddl.Items.Insert(0, new ListItem("--Select--", ""));
            }
            catch (Exception ex) { lbl_msg.Text += string.Format(" [Error binding {0}: {1}] ", ddl.ID, ex.Message); }
            finally { dbcl.DisconnectDb(); }
        }

        private void LogAudit(string emp, string type, string details)
        {
            try
            {
                string logPath = Server.MapPath("~/bussiness/production/Logs/EmployeeEdits/");
                if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);
                string line = string.Format("{0:yyyy-MM-dd HH:mm:ss} | {1} | {2} | By: {3}{4}", DateTime.Now, type, details, Session["USERNAME"], Environment.NewLine);
                File.AppendAllText(Path.Combine(logPath, "EmpLog_" + emp + ".txt"), line);
            }
            catch { }
        }

        private object ParseDate(string dt)
        {
            DateTime parsedDate;
            if (DateTime.TryParse(dt, out parsedDate)) { return parsedDate; }
            return DBNull.Value;
        }

        private string FormatDate(object dt)
        {
            if (dt != DBNull.Value && dt != null) { return Convert.ToDateTime(dt).ToString("yyyy-MM-dd"); }
            return "";
        }

        private decimal DecimalSafe(string val)
        {
            decimal result = 0m;
            decimal.TryParse(val, out result);
            return result;
        }

        private void SetDDL(DropDownList ddl, string val)
        {
            if (val != null)
            {
                string safeVal = val.Trim();
                if (ddl.Items.FindByValue(safeVal) != null)
                {
                    ddl.ClearSelection();
                    ddl.Items.FindByValue(safeVal).Selected = true;
                }
            }
        }

        private void SetDDLByText(DropDownList ddl, string text)
        {
            if (text != null && ddl.Items.Count > 0)
            {
                string safeText = text.Trim();
                ListItem item = ddl.Items.FindByText(safeText);
                if (item != null)
                {
                    ddl.ClearSelection();
                    item.Selected = true;
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