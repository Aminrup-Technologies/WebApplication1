/*
 * WHEN: 2026-09-07
 * WHY: Authorization modernization PR C2. Read-only Access Analyzer.
 * WHAT: Admin-only reports: who has a permission and why, user source summary,
 *      legacy exposure backlog, overlay adoption. Every permission row uses
 *      AuthorizationService.DescribeIdentity. No writes, no menu, no schema changes.
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.bussiness.production;

namespace WebApplication1.bussiness.production.admin.security
{
    public partial class AccessAnalyzer : System.Web.UI.Page
    {
        public const int AuthorizationServicePageCallers = 15;
        public const int ScanLimit = 1500;

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

            if (Page.Form != null)
            {
                Page.Form.Enctype = "multipart/form-data";
            }

            if (!IsPostBack)
            {
                BindPermissionList();
                BindOverlayAdoption();
            }
            BindKpi();
            BindSwitchUserCanary();
            ShowModePanels();
        }

        protected void rbl_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!EnsureAdmin()) return;
            ShowModePanels();
        }

        protected void btn_analyzePermission_Click(object sender, EventArgs e)
        {
            if (!EnsureAdmin()) return;
            string code = ddl_permission.SelectedValue;
            DataTable scan = ScanActiveEmployees();
            DataTable holders = FilterGranted(scan, code);
            gv_permission.DataSource = holders;
            gv_permission.DataBind();
            Session["AnalyzerExport"] = holders;
            Session["AnalyzerExportName"] = "permission-" + code + ".csv";

            int overlay = 0, config = 0, hardcoded = 0, module = 0;
            CountSources(holders, ref overlay, ref config, ref hardcoded, ref module);
            lbl_permOverlay.Text = overlay > 0 ? "Yes (" + overlay + ")" : "No";
            lbl_permConfig.Text = config > 0 ? "Yes (" + config + ")" : "No";
            lbl_permHardcoded.Text = hardcoded > 0 ? "Yes (" + hardcoded + ")" : "No";
            lbl_permModule.Text = module > 0 && overlay == 0 && config == 0 && hardcoded == 0
                ? "Yes (" + module + ")"
                : (module > 0 ? "Partial (" + module + ")" : "No");
            lbl_msg.Text = holders.Rows.Count + " Active employee(s) receive " + code + "." + TruncationNote();
            BindKpi();
            BindSwitchUserCanary();
        }

        protected void btn_searchUser_Click(object sender, EventArgs e)
        {
            if (!EnsureAdmin()) return;
            string workman = (txt_workman.Text ?? "").Trim();
            string firstName = (txt_firstname.Text ?? "").Trim();
            string fullName = (txt_fullname.Text ?? "").Trim();
            if (workman.Length == 0 && firstName.Length == 0 && fullName.Length == 0)
            {
                gv_userSearch.DataSource = null;
                gv_userSearch.DataBind();
                lbl_msg.Text = "Enter Workman SL, First Name, and/or Full Name.";
                return;
            }
            try
            {
                DataTable dt = SearchActiveEmployees(workman, firstName, fullName);
                gv_userSearch.DataSource = dt;
                gv_userSearch.DataBind();
                lbl_msg.Text = dt.Rows.Count == 0 ? "No Active employees matched." : dt.Rows.Count + " match(es).";
            }
            catch (Exception ex)
            {
                dbcl.WriteToFile("AccessAnalyzer search failed: " + ex);
                lbl_msg.Text = "Search failed.";
            }
        }

        protected void gv_userSearch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Summarize") return;
            if (!EnsureAdmin()) return;
            SummarizeUser(e.CommandArgument != null ? e.CommandArgument.ToString() : "");
        }

        protected void btn_analyzeLegacy_Click(object sender, EventArgs e)
        {
            if (!EnsureAdmin()) return;
            DataTable scan = ScanActiveEmployees();
            DataTable legacy = FilterLegacy(scan);
            gv_legacy.DataSource = legacy;
            gv_legacy.DataBind();
            Session["AnalyzerExport"] = legacy;
            Session["AnalyzerExportName"] = "legacy-exposure.csv";
            lbl_msg.Text = legacy.Rows.Count + " legacy-backed permission row(s)." + TruncationNote();
            BindKpi();
            BindSwitchUserCanary();
        }

        protected void btn_csv_Click(object sender, EventArgs e)
        {
            if (!EnsureAdmin()) return;
            DataTable table = Session["AnalyzerExport"] as DataTable;
            string name = Session["AnalyzerExportName"] as string;
            if (table == null || table.Rows.Count == 0)
            {
                if (CurrentMode() == "overlay")
                {
                    table = PermissionRepository.GetDirectGrantInventory();
                    name = "overlay-direct-grants.csv";
                }
                else if (CurrentMode() == "compare")
                {
                    lbl_msg.Text = "Nothing to export. Compare two snapshots first.";
                    return;
                }
                else
                {
                    lbl_msg.Text = "Nothing to export. Run a report first.";
                    return;
                }
            }
            if (string.IsNullOrEmpty(name)) name = "access-analyzer.csv";
            WriteCsv(table, name);
        }

        protected void btn_snapshot_Click(object sender, EventArgs e)
        {
            if (!EnsureAdmin()) return;
            try
            {
                AuthorizationSnapshotDocument doc = AuthorizationSnapshot.Generate(Session, dbcl);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "text/plain";
                Response.AddHeader("Content-Disposition", "attachment; filename=authorization-snapshot-" + DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ") + ".txt");
                Response.Write(doc.Body);
                Response.Flush();
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                dbcl.WriteToFile("AccessAnalyzer snapshot failed: " + ex);
                lbl_msg.Text = "Snapshot failed. Contact IT if this continues.";
            }
        }

        protected void btn_compare_Click(object sender, EventArgs e)
        {
            if (!EnsureAdmin()) return;
            string beforeBody = ReadUpload(fu_snapshotBefore);
            string afterBody = ReadUpload(fu_snapshotAfter);
            if (beforeBody.Length == 0 || afterBody.Length == 0)
            {
                lbl_compareVerdict.Text = "Choose both a Before snapshot and an After snapshot.";
                lbl_msg.Text = lbl_compareVerdict.Text;
                return;
            }

            AuthorizationSnapshotComparison comparison = AuthorizationSnapshot.Compare(beforeBody, afterBody);
            DataTable metrics = ComparisonMetricsTable(comparison);
            gv_compareMetrics.DataSource = metrics;
            gv_compareMetrics.DataBind();

            DataTable changes = ComparisonChangesTable(comparison);
            gv_compareChanges.DataSource = changes;
            gv_compareChanges.DataBind();
            Session["AnalyzerExport"] = changes;
            Session["AnalyzerExportName"] = "authorization-snapshot-diff.csv";

            if (!string.IsNullOrEmpty(comparison.Error))
            {
                lbl_compareVerdict.Text = comparison.Error;
            }
            else if (comparison.ChangedEffectivePermissionCount > 0)
            {
                lbl_compareVerdict.Text = "FAIL: Changed effective permission count = "
                    + comparison.ChangedEffectivePermissionCount.ToString()
                    + ". Stop and investigate before merging or starting the next PR.";
            }
            else if (comparison.SourceOnlyChangeCount > 0)
            {
                lbl_compareVerdict.Text = "FAIL: effective access is unchanged, but "
                    + comparison.SourceOnlyChangeCount.ToString()
                    + " source/display row(s) differ. Stop and investigate before PR E.";
            }
            else if (!comparison.PayloadShaEqual)
            {
                lbl_compareVerdict.Text = "FAIL: payload SHA-256 differs with no row diffs. Re-download both snapshots.";
            }
            else
            {
                lbl_compareVerdict.Text = "PASS: Changed effective permission count = 0. Payload SHA-256 matches.";
            }
            lbl_msg.Text = lbl_compareVerdict.Text;
        }

        protected void btn_print_Click(object sender, EventArgs e)
        {
            if (!EnsureAdmin()) return;
            ClientScript.RegisterStartupScript(GetType(), "AccessAnalyzerPrint", "window.print();", true);
        }

        private void SummarizeUser(string loginId)
        {
            loginId = (loginId ?? "").Trim();
            if (loginId.Length == 0) return;
            DataTable dt = login.FetchEmployeeRowByLoginId(dbcl, loginId);
            if (dt == null || dt.Rows.Count == 0)
            {
                lbl_msg.Text = "Employee was not found.";
                return;
            }
            DataRow row = dt.Rows[0];
            bool authenticated, admin, canImpersonate, payrollConfig;
            IList<EffectivePermission> permissions;
            AuthorizationService.DescribeIdentity(
                Session,
                ReadRow(row, "LoginID"),
                ReadRow(row, "RolePermissionDB"),
                ReadRow(row, "UserRoleDB"),
                ReadRow(row, "FullName"),
                ReadRow(row, "WorkmanSL"),
                ReadRow(row, "User_RoleType"),
                out authenticated,
                out admin,
                out canImpersonate,
                out payrollConfig,
                out permissions);

            DataTable summary = NewPermTable();
            int granted = 0;
            if (permissions != null)
            {
                for (int i = 0; i < permissions.Count; i++)
                {
                    EffectivePermission item = permissions[i];
                    if (item == null) continue;
                    if (item.Granted) granted++;
                    AddPermRow(summary, ReadRow(row, "WorkmanSL"), ReadRow(row, "FullName"), ReadRow(row, "User_RoleType"), item);
                }
            }
            gv_userPerms.DataSource = summary;
            gv_userPerms.DataBind();
            Session["AnalyzerExport"] = summary;
            Session["AnalyzerExportName"] = "user-" + ReadRow(row, "WorkmanSL") + ".csv";
            lbl_userIdentity.Text = ReadRow(row, "WorkmanSL") + " · " + ReadRow(row, "FullName");
            lbl_userGranted.Text = granted.ToString();
            lbl_msg.Text = "Summary from AuthorizationService.DescribeIdentity.";
        }

        private DataTable ScanActiveEmployees()
        {
            DataTable matrix = NewMatrixTable();
            DataTable scan = NewPermTable();
            DataTable employees;
            try
            {
                employees = dbcl.SPreturn_dt(@"
SELECT TOP " + ScanLimit.ToString() + @"
    LoginID, WorkmanSL, FirstName, FullName, User_RoleType, UserRoleDB, RolePermissionDB
FROM tbl_Employee_Mustertable
WHERE WorkStatus = N'Active'
  AND ISNULL(LoginID, N'') <> N''
  AND ISNULL(WorkmanSL, N'') <> N''
ORDER BY FullName, WorkmanSL", new SqlParameter[0]);
            }
            catch (Exception ex)
            {
                dbcl.WriteToFile("AccessAnalyzer scan failed: " + ex);
                lbl_msg.Text = "Employee scan failed.";
                return scan;
            }
            if (employees == null) employees = new DataTable();

            List<string> workmans = new List<string>();
            for (int i = 0; i < employees.Rows.Count; i++)
            {
                string workman = ReadRow(employees.Rows[i], "WorkmanSL");
                if (workman.Length > 0) workmans.Add(workman);
            }
            PermissionRepository.WarmSnapshots(workmans);

            Dictionary<string, int[]> counts = new Dictionary<string, int[]>(StringComparer.OrdinalIgnoreCase);
            string[] codes = AuthorizationService.GetTrackedCodes();
            for (int c = 0; c < codes.Length; c++)
            {
                counts[codes[c]] = new int[6];
            }

            Dictionary<string, bool> hardcodedUsers = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, bool> moduleUsers = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, bool> legacyUsers = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            int canaryEvaluated = 0;
            int canaryMatches = 0;
            DataTable canaryDivergences = NewCanaryTable();

            for (int i = 0; i < employees.Rows.Count; i++)
            {
                DataRow row = employees.Rows[i];
                bool authenticated, admin, canImpersonate, payrollConfig;
                IList<EffectivePermission> permissions;
                AuthorizationService.DescribeIdentity(
                    Session,
                    ReadRow(row, "LoginID"),
                    ReadRow(row, "RolePermissionDB"),
                    ReadRow(row, "UserRoleDB"),
                    ReadRow(row, "FullName"),
                    ReadRow(row, "WorkmanSL"),
                    ReadRow(row, "User_RoleType"),
                    out authenticated,
                    out admin,
                    out canImpersonate,
                    out payrollConfig,
                    out permissions);
                if (permissions == null) continue;
                for (int p = 0; p < permissions.Count; p++)
                {
                    EffectivePermission item = permissions[p];
                    if (item == null) continue;
                    AddPermRow(scan, ReadRow(row, "WorkmanSL"), ReadRow(row, "FullName"), ReadRow(row, "User_RoleType"), item);
                    if (string.Equals(item.Code, AuthorizationFeatureCodes.SwitchUser, StringComparison.OrdinalIgnoreCase))
                    {
                        canaryEvaluated++;
                        if (AuthorizationService.SwitchUserDualPathMatches(item, authenticated, admin))
                        {
                            canaryMatches++;
                        }
                        else
                        {
                            AddCanaryRow(canaryDivergences, ReadRow(row, "WorkmanSL"), ReadRow(row, "User_RoleType"), item);
                        }
                    }
                    int bucket = SourceBucket(item.Source, item.Granted);
                    int[] cells;
                    if (item.Code != null && counts.TryGetValue(item.Code, out cells) && bucket >= 0 && bucket < cells.Length)
                    {
                        cells[bucket]++;
                    }
                    if (item.Granted)
                    {
                        if (bucket == 3) hardcodedUsers[ReadRow(row, "WorkmanSL")] = true;
                        if (bucket == 4) moduleUsers[ReadRow(row, "WorkmanSL")] = true;
                        if (bucket == 2 || bucket == 3 || bucket == 4 || bucket == 5)
                        {
                            legacyUsers[ReadRow(row, "WorkmanSL")] = true;
                        }
                    }
                }
            }

            for (int c = 0; c < codes.Length; c++)
            {
                int[] cells = counts[codes[c]];
                DataRow m = matrix.NewRow();
                m["Permission"] = codes[c];
                m["Direct"] = cells[0].ToString();
                m["Group"] = cells[1].ToString();
                m["Config"] = cells[2].ToString();
                m["Hardcoded"] = cells[3].ToString();
                m["Module"] = cells[4].ToString();
                m["AdminConfig"] = cells[5].ToString();
                matrix.Rows.Add(m);
            }

            gv_matrix.DataSource = matrix;
            gv_matrix.DataBind();
            ViewState["KpiScanned"] = employees.Rows.Count;
            ViewState["KpiHardcoded"] = hardcodedUsers.Count;
            ViewState["KpiModule"] = moduleUsers.Count;
            ViewState["KpiLegacyUsers"] = legacyUsers.Count;
            ViewState["ScanTruncated"] = employees.Rows.Count >= ScanLimit;
            ViewState["CanaryEvaluated"] = canaryEvaluated;
            ViewState["CanaryMatches"] = canaryMatches;
            ViewState["CanaryDivergences"] = canaryEvaluated - canaryMatches;
            Session["CanaryDivergenceTable"] = canaryDivergences;
            return scan;
        }

        protected void btn_validateCanary_Click(object sender, EventArgs e)
        {
            if (!EnsureAdmin()) return;
            ScanActiveEmployees();
            BindKpi();
            BindSwitchUserCanary();
            object divergences = ViewState["CanaryDivergences"];
            lbl_msg.Text = "SWITCH_USER canary dual-path validation finished. Divergences: "
                + (divergences != null ? divergences.ToString() : "0")
                + "." + TruncationNote();
        }

        private void BindPermissionList()
        {
            string[] codes = AuthorizationService.GetTrackedCodes();
            ddl_permission.Items.Clear();
            for (int i = 0; i < codes.Length; i++)
            {
                ddl_permission.Items.Add(new ListItem(codes[i], codes[i]));
            }
        }

        private void BindOverlayAdoption()
        {
            bool ok = PermissionRepository.IsOverlaySchemaAvailable();
            pnl_overlayWarning.Visible = !ok;
            DataTable direct = PermissionRepository.GetDirectGrantInventory();
            DataTable groups = PermissionRepository.GetGroupGrantInventory();
            DataTable catalog = PermissionRepository.GetGroupCatalogInventory();
            gv_direct.DataSource = direct;
            gv_direct.DataBind();
            gv_groupGrants.DataSource = groups;
            gv_groupGrants.DataBind();

            DataTable orphans = catalog.Clone();
            for (int i = 0; i < catalog.Rows.Count; i++)
            {
                int members = ParseInt(catalog.Rows[i]["MemberCount"]);
                int perms = ParseInt(catalog.Rows[i]["PermissionCount"]);
                if (members == 0 || perms == 0) orphans.ImportRow(catalog.Rows[i]);
            }
            gv_orphans.DataSource = orphans;
            gv_orphans.DataBind();

            IList<string> unused = PermissionRepository.GetUnusedPermissionCodes();
            lbl_unused.Text = JoinCodes(unused);
            lbl_overlayGrants.Text = (direct.Rows.Count + groups.Rows.Count).ToString();
            lbl_overlayGroups.Text = catalog.Rows.Count.ToString();
            object legacy = ViewState["KpiLegacyUsers"];
            lbl_overlayLegacyUsers.Text = legacy != null ? legacy.ToString() : "—";
            ViewState["KpiOverlay"] = direct.Rows.Count + groups.Rows.Count;
        }

        private void BindKpi()
        {
            lbl_kpiPages.Text = AuthorizationServicePageCallers.ToString();
            object overlay = ViewState["KpiOverlay"];
            if (overlay == null)
            {
                DataTable direct = PermissionRepository.GetDirectGrantInventory();
                DataTable groups = PermissionRepository.GetGroupGrantInventory();
                overlay = direct.Rows.Count + groups.Rows.Count;
                ViewState["KpiOverlay"] = overlay;
            }
            lbl_kpiOverlay.Text = overlay.ToString();
            lbl_kpiConfig.Text = CountConfigCsvWorkmans().ToString();
            lbl_kpiHardcoded.Text = ViewState["KpiHardcoded"] != null ? ViewState["KpiHardcoded"].ToString() : "—";
            lbl_kpiModule.Text = ViewState["KpiModule"] != null ? ViewState["KpiModule"].ToString() : "—";
            lbl_kpiScanned.Text = ViewState["KpiScanned"] != null ? ViewState["KpiScanned"].ToString() : "0";
            object legacy = ViewState["KpiLegacyUsers"];
            if (lbl_overlayLegacyUsers != null)
            {
                lbl_overlayLegacyUsers.Text = legacy != null ? legacy.ToString() : "—";
            }
        }

        private void BindSwitchUserCanary()
        {
            int overlay = PermissionRepository.CountDistinctWorkmansForCode(AuthorizationFeatureCodes.SwitchUser);
            int config = CountSwitchUserCsvWorkmans();
            lbl_canaryOverlay.Text = overlay.ToString();
            lbl_canaryLegacy.Text = config.ToString();

            object evaluated = ViewState["CanaryEvaluated"];
            object matches = ViewState["CanaryMatches"];
            object divergences = ViewState["CanaryDivergences"];
            if (evaluated == null)
            {
                lbl_canaryEvaluated.Text = "—";
                lbl_canaryMatch.Text = "—";
                lbl_canaryDivergences.Text = "—";
                lbl_canaryHealth.Text = overlay == 0 ? "Healthy (no overlay grants; run Validate canary)" : "Pending scan";
                gv_canaryDivergences.DataSource = Session["CanaryDivergenceTable"] as DataTable;
                gv_canaryDivergences.DataBind();
                return;
            }

            int evaluatedCount = ParseInt(evaluated);
            int matchCount = ParseInt(matches);
            int divergenceCount = ParseInt(divergences);
            lbl_canaryEvaluated.Text = evaluatedCount.ToString();
            lbl_canaryDivergences.Text = divergenceCount.ToString();
            if (evaluatedCount == 0)
            {
                lbl_canaryMatch.Text = "n/a";
            }
            else
            {
                double pct = (100.0 * matchCount) / evaluatedCount;
                lbl_canaryMatch.Text = pct.ToString("0") + "%";
            }
            lbl_canaryHealth.Text = divergenceCount == 0 ? "Healthy" : "Unhealthy — stop";
            DataTable divergenceTable = Session["CanaryDivergenceTable"] as DataTable;
            gv_canaryDivergences.DataSource = divergenceTable;
            gv_canaryDivergences.DataBind();
        }

        private static int CountSwitchUserCsvWorkmans()
        {
            Dictionary<string, bool> unique = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            AddCsv(unique, ConfigurationManager.AppSettings[ImpersonationAudit.AuthorizedUsersAppSetting]);
            return unique.Count;
        }

        private void ShowModePanels()
        {
            string mode = CurrentMode();
            pnl_permission.Visible = mode == "permission";
            pnl_user.Visible = mode == "user";
            pnl_legacy.Visible = mode == "legacy";
            pnl_overlay.Visible = mode == "overlay";
            pnl_compare.Visible = mode == "compare";
            if (mode == "overlay") BindOverlayAdoption();
        }

        private string CurrentMode()
        {
            return rbl_mode.SelectedValue ?? "permission";
        }

        private static DataTable FilterGranted(DataTable scan, string code)
        {
            DataTable table = scan.Clone();
            for (int i = 0; i < scan.Rows.Count; i++)
            {
                DataRow row = scan.Rows[i];
                if (!Convert.ToBoolean(row["Granted"])) continue;
                if (!string.Equals(Convert.ToString(row["Code"]), code, StringComparison.OrdinalIgnoreCase)) continue;
                table.ImportRow(row);
            }
            return table;
        }

        private static DataTable FilterLegacy(DataTable scan)
        {
            DataTable table = scan.Clone();
            for (int i = 0; i < scan.Rows.Count; i++)
            {
                DataRow row = scan.Rows[i];
                if (!Convert.ToBoolean(row["Granted"])) continue;
                string source = Convert.ToString(row["Source"]);
                if (string.Equals(source, AuthorizationService.SourceLegacyConfig, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(source, AuthorizationService.SourceLegacyHardcoded, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(source, AuthorizationService.SourceModuleException, StringComparison.OrdinalIgnoreCase)
                    || (source != null && source.IndexOf(AuthorizationService.SourceLegacyConfig, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    table.ImportRow(row);
                }
            }
            return table;
        }

        private static void CountSources(DataTable holders, ref int overlay, ref int config, ref int hardcoded, ref int module)
        {
            for (int i = 0; i < holders.Rows.Count; i++)
            {
                string source = Convert.ToString(holders.Rows[i]["Source"]);
                int bucket = SourceBucket(source, true);
                if (bucket == 0 || bucket == 1) overlay++;
                else if (bucket == 2 || bucket == 5) config++;
                else if (bucket == 3) hardcoded++;
                else if (bucket == 4) module++;
            }
        }

        private static int SourceBucket(string source, bool granted)
        {
            if (!granted) return -1;
            if (string.Equals(source, AuthorizationService.SourceDirect, StringComparison.OrdinalIgnoreCase)
                || string.Equals(source, AuthorizationService.SourceOverlayDirect, StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }
            if (string.Equals(source, AuthorizationService.SourceGroup, StringComparison.OrdinalIgnoreCase)
                || string.Equals(source, AuthorizationService.SourceOverlayGroup, StringComparison.OrdinalIgnoreCase))
            {
                return 1;
            }
            if (string.Equals(source, AuthorizationService.SourceLegacyConfig, StringComparison.OrdinalIgnoreCase)) return 2;
            if (string.Equals(source, AuthorizationService.SourceLegacyHardcoded, StringComparison.OrdinalIgnoreCase)) return 3;
            if (string.Equals(source, AuthorizationService.SourceModuleException, StringComparison.OrdinalIgnoreCase)) return 4;
            if (source != null
                && source.IndexOf(AuthorizationService.SourceUserType, StringComparison.OrdinalIgnoreCase) >= 0
                && source.IndexOf(AuthorizationService.SourceLegacyConfig, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return 5;
            }
            return -1;
        }

        private static DataTable NewPermTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("WorkmanSL");
            table.Columns.Add("FullName");
            table.Columns.Add("UserType");
            table.Columns.Add("Code");
            table.Columns.Add("Granted", typeof(bool));
            table.Columns.Add("Source");
            table.Columns.Add("Display");
            table.Columns.Add("Detail");
            table.Columns.Add("OverlayWouldAllow");
            table.Columns.Add("LegacyWouldAllow");
            return table;
        }

        private static DataTable NewCanaryTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("WorkmanSL");
            table.Columns.Add("UserType");
            table.Columns.Add("Allowed");
            table.Columns.Add("Source");
            table.Columns.Add("OverlayWouldAllow");
            table.Columns.Add("LegacyWouldAllow");
            return table;
        }

        private static void AddCanaryRow(DataTable table, string workman, string userType, EffectivePermission item)
        {
            DataRow row = table.NewRow();
            row["WorkmanSL"] = workman;
            row["UserType"] = userType;
            row["Allowed"] = item != null && item.Granted ? "true" : "false";
            row["Source"] = item != null ? item.Source : "";
            row["OverlayWouldAllow"] = item != null && item.OverlayWouldAllow ? "true" : "false";
            row["LegacyWouldAllow"] = item != null && item.LegacyWouldAllow ? "true" : "false";
            table.Rows.Add(row);
        }

        private static DataTable NewMatrixTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Permission");
            table.Columns.Add("Direct");
            table.Columns.Add("Group");
            table.Columns.Add("Config");
            table.Columns.Add("Hardcoded");
            table.Columns.Add("Module");
            table.Columns.Add("AdminConfig");
            return table;
        }

        private static void AddPermRow(DataTable table, string workman, string fullName, string userType, EffectivePermission item)
        {
            DataRow row = table.NewRow();
            row["WorkmanSL"] = workman;
            row["FullName"] = fullName;
            row["UserType"] = userType;
            row["Code"] = item.Code ?? "";
            row["Granted"] = item.Granted;
            row["Source"] = item.Source ?? "";
            row["Display"] = AuthorizationService.DisplaySource(item.Source, item.Granted);
            row["Detail"] = item.Detail ?? "";
            row["OverlayWouldAllow"] = item.OverlayWouldAllow ? "true" : "false";
            row["LegacyWouldAllow"] = item.LegacyWouldAllow ? "true" : "false";
            table.Rows.Add(row);
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

        private static int CountConfigCsvWorkmans()
        {
            Dictionary<string, bool> unique = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            AddCsv(unique, ConfigurationManager.AppSettings[ImpersonationAudit.AuthorizedUsersAppSetting]);
            AddCsv(unique, ConfigurationManager.AppSettings["PayrollAuthorizedUsers"]);
            return unique.Count;
        }

        private static void AddCsv(Dictionary<string, bool> unique, string csv)
        {
            if (string.IsNullOrEmpty(csv)) return;
            string[] parts = csv.Split(',');
            for (int i = 0; i < parts.Length; i++)
            {
                string item = parts[i].Trim();
                if (item.Length > 0) unique[item] = true;
            }
        }

        private void WriteCsv(DataTable table, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            for (int c = 0; c < table.Columns.Count; c++)
            {
                if (c > 0) sb.Append(",");
                sb.Append(Csv(table.Columns[c].ColumnName));
            }
            sb.Append("\r\n");
            for (int r = 0; r < table.Rows.Count; r++)
            {
                for (int c = 0; c < table.Columns.Count; c++)
                {
                    if (c > 0) sb.Append(",");
                    object value = table.Rows[r][c];
                    sb.Append(Csv(value == null || value == DBNull.Value ? "" : value.ToString()));
                }
                sb.Append("\r\n");
            }
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.Write(sb.ToString());
            Response.Flush();
            Context.ApplicationInstance.CompleteRequest();
        }

        private static string ReadUpload(FileUpload upload)
        {
            if (upload == null || !upload.HasFile) return "";
            using (System.IO.StreamReader reader = new System.IO.StreamReader(upload.FileContent, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private static DataTable ComparisonMetricsTable(AuthorizationSnapshotComparison comparison)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Metric");
            table.Columns.Add("Before");
            table.Columns.Add("After");
            table.Columns.Add("Delta");
            AuthorizationSnapshotMetrics before = comparison != null ? comparison.BeforeMetrics : new AuthorizationSnapshotMetrics();
            AuthorizationSnapshotMetrics after = comparison != null ? comparison.AfterMetrics : new AuthorizationSnapshotMetrics();
            AddMetric(table, "Active employees scanned", before.EmployeesScanned, after.EmployeesScanned);
            AddMetric(table, "Permissions evaluated", before.PermissionsEvaluated, after.PermissionsEvaluated);
            AddMetric(table, "Allowed decisions", before.Allowed, after.Allowed);
            AddMetric(table, "Denied decisions", before.Denied, after.Denied);
            AddMetric(table, "Legacy Config grants", before.LegacyConfigGrants, after.LegacyConfigGrants);
            AddMetric(table, "Legacy Hardcoded grants", before.LegacyHardcodedGrants, after.LegacyHardcodedGrants);
            AddMetric(table, "Module grants", before.ModuleGrants, after.ModuleGrants);
            AddMetric(table, "Overlay grants", before.OverlayGrants, after.OverlayGrants);
            AddMetric(table, "Changed effective permission count", 0, comparison != null ? comparison.ChangedEffectivePermissionCount : 0);
            return table;
        }

        private static void AddMetric(DataTable table, string metric, int before, int after)
        {
            DataRow row = table.NewRow();
            row["Metric"] = metric;
            row["Before"] = before.ToString();
            row["After"] = after.ToString();
            row["Delta"] = (after - before).ToString();
            table.Rows.Add(row);
        }

        private static DataTable ComparisonChangesTable(AuthorizationSnapshotComparison comparison)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Kind");
            table.Columns.Add("WorkmanSL");
            table.Columns.Add("LoginID");
            table.Columns.Add("Code");
            table.Columns.Add("BeforeGranted");
            table.Columns.Add("AfterGranted");
            table.Columns.Add("BeforeSource");
            table.Columns.Add("AfterSource");
            if (comparison == null || comparison.Changes == null) return table;
            int limit = comparison.Changes.Count;
            if (limit > 200) limit = 200;
            for (int i = 0; i < limit; i++)
            {
                AuthorizationSnapshotChange change = comparison.Changes[i];
                if (change == null) continue;
                DataRow row = table.NewRow();
                row["Kind"] = change.Kind;
                row["WorkmanSL"] = change.WorkmanSL;
                row["LoginID"] = change.LoginID;
                row["Code"] = change.Code;
                row["BeforeGranted"] = change.BeforeGranted;
                row["AfterGranted"] = change.AfterGranted;
                row["BeforeSource"] = change.BeforeSource;
                row["AfterSource"] = change.AfterSource;
                table.Rows.Add(row);
            }
            return table;
        }

        private static string Csv(string value)
        {
            string text = value ?? "";
            if (text.IndexOfAny(new char[] { ',', '"', '\n', '\r' }) >= 0)
            {
                return "\"" + text.Replace("\"", "\"\"") + "\"";
            }
            return text;
        }

        private string TruncationNote()
        {
            object flag = ViewState["ScanTruncated"];
            if (flag is bool && (bool)flag)
            {
                return " Scan capped at " + ScanLimit.ToString() + " Active employees.";
            }
            return "";
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

        private static int ParseInt(object value)
        {
            if (value == null || value == DBNull.Value) return 0;
            int parsed;
            if (int.TryParse(value.ToString(), out parsed)) return parsed;
            return 0;
        }
    }
}
