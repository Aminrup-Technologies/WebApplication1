/*
 * WHEN: 2026-09-07
 * WHY: Authorization modernization PR C1. Read-only Permission Inspector for support / UAT.
 * WHAT: Admin-only page. Search Active employees, then display identity, AuthorizationService
 *      effective permissions, source breakdown, legacy diagnostics, overlay health, and cache
 *      peek. No writes. No menu entry. No schema changes.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.bussiness.production;

namespace WebApplication1.bussiness.production.admin.security
{
    public partial class PermissionInspector : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AuthorizationService.IsAuthenticated())
            {
                Response.Redirect("~/login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!AuthorizationService.IsAdmin())
            {
                Response.Redirect("~/bussiness/production/homepage_v2.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (!EnsureAdmin()) return;

            string workman = (txt_workman.Text ?? "").Trim();
            string firstName = (txt_firstname.Text ?? "").Trim();
            string fullName = (txt_fullname.Text ?? "").Trim();
            if (workman.Length == 0 && firstName.Length == 0 && fullName.Length == 0)
            {
                gv_results.DataSource = null;
                gv_results.DataBind();
                pnl_inspect.Visible = false;
                lbl_msg.Text = "Enter Workman SL, First Name, and/or Full Name.";
                Notify("Search", "Enter at least one search field.", "error");
                return;
            }

            try
            {
                DataTable dt = SearchActiveEmployees(workman, firstName, fullName);
                gv_results.DataSource = dt;
                gv_results.DataBind();
                lbl_msg.Text = dt.Rows.Count == 0
                    ? "No Active employees matched."
                    : dt.Rows.Count + " Active employee(s) found.";
            }
            catch (Exception ex)
            {
                dbcl.WriteToFile("PermissionInspector search failed: " + ex);
                Notify("Error", "Search failed. Contact IT if this continues.", "error");
            }
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            if (!EnsureAdmin()) return;
            txt_workman.Text = "";
            txt_firstname.Text = "";
            txt_fullname.Text = "";
            gv_results.DataSource = null;
            gv_results.DataBind();
            pnl_inspect.Visible = false;
            lbl_msg.Text = "Enter at least one search field, then SEARCH.";
        }

        protected void gv_results_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Inspect") return;
            InspectLogin(e.CommandArgument != null ? e.CommandArgument.ToString() : "");
        }

        protected void gv_permissions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            Label lbl = e.Row.FindControl("lbl_allowed") as Label;
            if (lbl == null) return;
            bool granted = false;
            DataRowView row = e.Row.DataItem as DataRowView;
            if (row != null)
            {
                granted = Convert.ToBoolean(row["Granted"]);
            }
            MarkYesNo(lbl, granted);
        }

        protected void gv_legacy_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            Label lbl = e.Row.FindControl("lbl_result") as Label;
            if (lbl == null) return;
            DataRowView row = e.Row.DataItem as DataRowView;
            if (row == null) return;
            MarkYesNo(lbl, Convert.ToBoolean(row["Passed"]));
        }

        private void InspectLogin(string loginId)
        {
            if (!EnsureAdmin()) return;

            loginId = (loginId ?? "").Trim();
            if (loginId.Length == 0)
            {
                Notify("Inspect", "Missing login id.", "error");
                return;
            }

            try
            {
                DataTable dt = login.FetchEmployeeRowByLoginId(dbcl, loginId);
                if (dt == null || dt.Rows.Count == 0)
                {
                    pnl_inspect.Visible = false;
                    Notify("Inspect", "Employee was not found.", "error");
                    return;
                }

                BindInspect(dt.Rows[0]);
                pnl_inspect.Visible = true;
            }
            catch (Exception ex)
            {
                dbcl.WriteToFile("PermissionInspector inspect failed: " + ex);
                Notify("Error", "Inspect failed. Contact IT if this continues.", "error");
            }
        }

        private void BindInspect(DataRow row)
        {
            string loginId = ReadRow(row, "LoginID");
            string workman = ReadRow(row, "WorkmanSL");
            string fullName = ReadRow(row, "FullName");
            string userType = ReadRow(row, "User_RoleType");
            string userRoleDb = ReadRow(row, "UserRoleDB");
            string rolePermissionDb = ReadRow(row, "RolePermissionDB");
            string region = ReadRow(row, "WorkRegion");
            string site = ReadRow(row, "WorkSite");

            lbl_fullName.Text = fullName;
            lbl_workman.Text = workman;
            lbl_userType.Text = userType;
            lbl_region.Text = region;
            lbl_site.Text = site;
            lbl_loginId.Text = loginId;
            lbl_userRoleDb.Text = userRoleDb;
            lbl_rolePermissionDb.Text = rolePermissionDb;
            img_photo.ImageUrl = ResolvePhotoUrl(ReadRow(row, "PrfPicFile"));

            bool cacheHit = PermissionRepository.PeekSnapshotCached(workman);

            bool authenticated;
            bool admin;
            bool canImpersonate;
            bool payrollConfig;
            IList<EffectivePermission> permissions;
            AuthorizationService.DescribeIdentity(
                Session,
                loginId,
                rolePermissionDb,
                userRoleDb,
                fullName,
                workman,
                userType,
                out authenticated,
                out admin,
                out canImpersonate,
                out payrollConfig,
                out permissions);

            gv_permissions.DataSource = ToPermissionTable(permissions);
            gv_permissions.DataBind();

            BindBreakdown(userType, admin, payrollConfig, permissions, workman);
            BindLegacy(authenticated, admin, canImpersonate, payrollConfig, permissions);
            BindOverlayAndCache(workman, cacheHit);
        }

        private void BindBreakdown(string userType, bool admin, bool payrollConfig, IList<EffectivePermission> permissions, string workman)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Layer");
            table.Columns.Add("Kind");
            table.Columns.Add("Value");

            IList<string> groups = PermissionRepository.GetGroupCodes(workman);
            IList<string> direct = PermissionRepository.GetDirectPermissionCodes(workman);

            bool userTypeUsed = admin || HasGrantedSourceContaining(permissions, AuthorizationService.SourceUserType)
                || HasGrantedSource(permissions, AuthorizationService.SourceModuleException);
            if (userTypeUsed)
            {
                AddBreakdown(table, "Platform Role", "USERTYPE", userType);
            }

            if (groups != null && groups.Count > 0)
            {
                AddBreakdown(table, "Permission Group", "Overlay groups", JoinCodes(groups));
            }

            if (direct != null && direct.Count > 0)
            {
                AddBreakdown(table, "Direct Permission", "Employee grant", JoinCodes(direct));
            }

            string moduleCodes = JoinGrantedCodes(permissions, AuthorizationService.SourceModuleException);
            if (!string.IsNullOrEmpty(moduleCodes))
            {
                AddBreakdown(table, "Legacy Exception", "Module USERTYPE", moduleCodes);
            }

            string hardcodedCodes = JoinGrantedCodes(permissions, AuthorizationService.SourceLegacyHardcoded);
            if (!string.IsNullOrEmpty(hardcodedCodes))
            {
                AddBreakdown(table, "Legacy Exception", "WORKMAN allowlist", hardcodedCodes);
            }

            string configCodes = JoinGrantedCodes(permissions, AuthorizationService.SourceLegacyConfig);
            EffectivePermission switchUser = FindCode(permissions, AuthorizationFeatureCodes.SwitchUser);
            if (!string.IsNullOrEmpty(configCodes))
            {
                AddBreakdown(table, "Config Fallback", "CSV fallback", configCodes);
            }
            else if (switchUser != null && switchUser.Granted
                && switchUser.Source != null
                && switchUser.Source.IndexOf(AuthorizationService.SourceLegacyConfig, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                AddBreakdown(table, "Config Fallback", "CSV fallback", ImpersonationAudit.AuthorizedUsersAppSetting);
            }
            else if (payrollConfig)
            {
                AddBreakdown(table, "Config Fallback", "CSV fallback", "PayrollAuthorizedUsers");
            }

            rpt_breakdown.DataSource = table;
            rpt_breakdown.DataBind();
            rpt_breakdown.Visible = table.Rows.Count > 0;
            lbl_breakdownEmpty.Visible = table.Rows.Count == 0;
        }

        private void BindLegacy(bool authenticated, bool admin, bool canImpersonate, bool payrollConfig, IList<EffectivePermission> permissions)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Check");
            table.Columns.Add("Passed", typeof(bool));
            AddLegacy(table, "IsAuthenticated", authenticated);
            AddLegacy(table, "IsAdmin", admin);
            AddLegacy(table, "CanImpersonate", canImpersonate);
            AddLegacy(table, "PayrollAuthorizedUsers", payrollConfig);
            EffectivePermission job360 = FindCode(permissions, AuthorizationFeatureCodes.Job360Override);
            AddLegacy(table, "JOB360 Override", job360 != null && job360.Granted);
            gv_legacy.DataSource = table;
            gv_legacy.DataBind();
        }

        private void BindOverlayAndCache(string workman, bool cacheHit)
        {
            bool schemaOk = PermissionRepository.IsOverlaySchemaAvailable();
            pnl_overlayWarning.Visible = !schemaOk;
            lbl_overlayHealth.Text = schemaOk ? "Healthy" : "Unavailable";
            lbl_overlayAvailability.Text = schemaOk
                ? "Database objects reachable"
                : "Overlay tables missing or unreachable";

            bool cacheAvailable = PermissionRepository.IsRuntimeCacheAvailable();

            IList<string> direct = PermissionRepository.GetDirectPermissionCodes(workman);
            IList<string> groupPerms = PermissionRepository.GetGroupPermissionCodes(workman);
            IList<string> groups = PermissionRepository.GetGroupCodes(workman);

            lbl_directCount.Text = (direct != null ? direct.Count : 0).ToString();
            lbl_groupCount.Text = (groups != null ? groups.Count : 0).ToString();
            lbl_directCodes.Text = JoinCodes(direct);
            lbl_groupCodes.Text = JoinCodes(groups);
            lbl_groupPermCodes.Text = JoinCodes(groupPerms);

            DateTime loadedAt = PermissionRepository.GetSnapshotLoadedAtUtc(workman);
            lbl_cacheStatus.Text = cacheAvailable
                ? (cacheHit ? "Warm" : "Warm (populated this request)")
                : "Unavailable";
            lbl_cacheHit.Text = cacheHit ? "Cache Hit" : "Cache Miss";
            lbl_cacheRefresh.Text = loadedAt == DateTime.MinValue
                ? "—"
                : loadedAt.ToString("yyyy-MM-dd HH:mm:ss") + " UTC";
            lbl_cacheTtl.Text = (PermissionRepository.CacheTtlMinutes * 60).ToString() + " seconds";
        }

        private DataTable SearchActiveEmployees(string workmanSL, string firstName, string fullName)
        {
            List<string> filters = new List<string>();
            filters.Add("WorkStatus = @WorkStatus");
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@WorkStatus", "Active"));

            if (!string.IsNullOrWhiteSpace(workmanSL))
            {
                filters.Add("WorkmanSL LIKE @WorkmanSL");
                parameters.Add(new SqlParameter("@WorkmanSL", ToContainsLike(workmanSL.Trim())));
            }
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                filters.Add("FirstName LIKE @FirstName");
                parameters.Add(new SqlParameter("@FirstName", ToContainsLike(firstName.Trim())));
            }
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                filters.Add("FullName LIKE @FullName");
                parameters.Add(new SqlParameter("@FullName", ToContainsLike(fullName.Trim())));
            }

            string query = @"
                SELECT TOP 100
                    WorkmanSL, FirstName, FullName, LoginID, WorkStatus, User_RoleType, WorkRegion, WorkCompany
                FROM tbl_Employee_Mustertable
                WHERE " + string.Join(" AND ", filters.ToArray()) + @"
                ORDER BY FullName, WorkmanSL";

            return dbcl.SPreturn_dt(query, parameters.ToArray());
        }

        private bool EnsureAdmin()
        {
            if (!AuthorizationService.IsAuthenticated())
            {
                Response.Redirect("~/login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return false;
            }
            if (!AuthorizationService.IsAdmin())
            {
                Response.Redirect("~/bussiness/production/homepage_v2.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return false;
            }
            return true;
        }

        private string ResolvePhotoUrl(string fileName)
        {
            string photo = (fileName ?? "").Trim();
            string folder = HostingEnvironment.MapPath("~/erp_images/ProfilePhoto");
            if (string.IsNullOrEmpty(photo) || string.IsNullOrEmpty(folder) || !File.Exists(Path.Combine(folder, photo)))
            {
                photo = "No_Image.jpg";
            }
            return ResolveUrl("~/erp_images/ProfilePhoto/" + photo);
        }

        private static DataTable ToPermissionTable(IList<EffectivePermission> permissions)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Code");
            table.Columns.Add("Granted", typeof(bool));
            table.Columns.Add("Layer");
            table.Columns.Add("Source");
            table.Columns.Add("Detail");
            if (permissions == null) return table;
            for (int i = 0; i < permissions.Count; i++)
            {
                EffectivePermission item = permissions[i];
                if (item == null) continue;
                DataRow row = table.NewRow();
                row["Code"] = item.Code ?? "";
                row["Granted"] = item.Granted;
                row["Layer"] = LayerForSource(item.Source);
                row["Source"] = item.Source ?? "";
                row["Detail"] = item.Detail ?? "";
                table.Rows.Add(row);
            }
            return table;
        }

        private static string LayerForSource(string source)
        {
            if (string.IsNullOrWhiteSpace(source)
                || string.Equals(source, AuthorizationService.SourceNone, StringComparison.OrdinalIgnoreCase))
            {
                return "";
            }
            if (string.Equals(source, AuthorizationService.SourceDirect, StringComparison.OrdinalIgnoreCase)
                || string.Equals(source, AuthorizationService.SourceGroup, StringComparison.OrdinalIgnoreCase))
            {
                return "Overlay";
            }
            return "Legacy";
        }

        private static void AddBreakdown(DataTable table, string layer, string kind, string value)
        {
            DataRow row = table.NewRow();
            row["Layer"] = layer;
            row["Kind"] = kind;
            row["Value"] = value ?? "";
            table.Rows.Add(row);
        }

        private static void AddLegacy(DataTable table, string check, bool passed)
        {
            DataRow row = table.NewRow();
            row["Check"] = check;
            row["Passed"] = passed;
            table.Rows.Add(row);
        }

        private static void MarkYesNo(Label lbl, bool yes)
        {
            lbl.Text = yes ? "✓" : "✗";
            lbl.CssClass = yes ? "perm-yes" : "perm-no";
        }

        private static EffectivePermission FindCode(IList<EffectivePermission> permissions, string code)
        {
            if (permissions == null) return null;
            for (int i = 0; i < permissions.Count; i++)
            {
                if (permissions[i] != null
                    && string.Equals(permissions[i].Code, code, StringComparison.OrdinalIgnoreCase))
                {
                    return permissions[i];
                }
            }
            return null;
        }

        private static EffectivePermission FindGranted(IList<EffectivePermission> permissions, string source)
        {
            if (permissions == null) return null;
            for (int i = 0; i < permissions.Count; i++)
            {
                EffectivePermission item = permissions[i];
                if (item == null || !item.Granted) continue;
                if (string.Equals(item.Source, source, StringComparison.OrdinalIgnoreCase)) return item;
            }
            return null;
        }

        private static bool HasGrantedSource(IList<EffectivePermission> permissions, string source)
        {
            return FindGranted(permissions, source) != null;
        }

        private static bool HasGrantedSourceContaining(IList<EffectivePermission> permissions, string fragment)
        {
            if (permissions == null || string.IsNullOrEmpty(fragment)) return false;
            for (int i = 0; i < permissions.Count; i++)
            {
                EffectivePermission item = permissions[i];
                if (item == null || !item.Granted || item.Source == null) continue;
                if (item.Source.IndexOf(fragment, StringComparison.OrdinalIgnoreCase) >= 0) return true;
            }
            return false;
        }

        private static string JoinGrantedCodes(IList<EffectivePermission> permissions, string source)
        {
            if (permissions == null || string.IsNullOrEmpty(source)) return "";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < permissions.Count; i++)
            {
                EffectivePermission item = permissions[i];
                if (item == null || !item.Granted) continue;
                if (!string.Equals(item.Source, source, StringComparison.OrdinalIgnoreCase)) continue;
                if (sb.Length > 0) sb.Append(", ");
                sb.Append(item.Code);
                if (!string.IsNullOrWhiteSpace(item.Detail))
                {
                    sb.Append(" — ");
                    sb.Append(item.Detail);
                }
            }
            return sb.ToString();
        }

        private static string JoinCodes(IList<string> codes)
        {
            if (codes == null || codes.Count == 0) return "None";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < codes.Count; i++)
            {
                if (i > 0) sb.Append(", ");
                sb.Append(codes[i]);
            }
            return sb.ToString();
        }

        private static string ToContainsLike(string value)
        {
            string escaped = (value ?? "")
                .Replace("[", "[[]")
                .Replace("%", "[%]")
                .Replace("_", "[_]");
            return "%" + escaped + "%";
        }

        private static string ReadRow(DataRow row, string column)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(column) || row[column] == DBNull.Value)
            {
                return "";
            }
            return row[column].ToString();
        }

        private void Notify(string title, string text, string type)
        {
            string script = "new PNotify({ title: '" + HttpUtility.JavaScriptStringEncode(title)
                + "', text: '" + HttpUtility.JavaScriptStringEncode(text)
                + "', type: '" + HttpUtility.JavaScriptStringEncode(type)
                + "', styling: 'bootstrap3' });";
            ClientScript.RegisterStartupScript(GetType(), "PermissionInspectorNotify", script, true);
        }
    }
}
