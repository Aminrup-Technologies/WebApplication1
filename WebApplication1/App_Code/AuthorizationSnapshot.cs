/*
 * WHEN: 2026-09-07
 * WHY: Authorization modernization PR D. Read-only baseline before further migration / CRUD.
 * WHAT: Snapshot every Active employee's effective permissions via DescribeIdentity.
 *      SHA-256 over the canonical row payload. No writes to overlay or Session identity.
 *      Parse/Compare ignore GeneratedUtc so a post-migration file can prove zero drift.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.SessionState;

namespace WebApplication1.bussiness.production
{
    public sealed class AuthorizationSnapshotDocument
    {
        public string GeneratedUtc { get; set; }
        public string Sha256 { get; set; }
        public int EmployeeCount { get; set; }
        public int GrantCount { get; set; }
        public int RowCount { get; set; }
        public bool Truncated { get; set; }
        public string Body { get; set; }
    }

    public sealed class AuthorizationSnapshotRow
    {
        public string WorkmanSL { get; set; }
        public string LoginID { get; set; }
        public string UserType { get; set; }
        public string Code { get; set; }
        public bool Granted { get; set; }
        public string Source { get; set; }
        public string Display { get; set; }
        public string Line { get; set; }

        public string DecisionKey
        {
            get { return (WorkmanSL ?? "") + "|" + (LoginID ?? "") + "|" + (Code ?? ""); }
        }
    }

    public sealed class AuthorizationSnapshotParsed
    {
        public string GeneratedUtc { get; set; }
        public string Sha256 { get; set; }
        public string PayloadSha256 { get; set; }
        public int EmployeeCount { get; set; }
        public int GrantCount { get; set; }
        public int RowCount { get; set; }
        public bool Truncated { get; set; }
        public int ScanLimit { get; set; }
        public bool HeaderHashMatchesPayload { get; set; }
        public string Error { get; set; }
        public List<AuthorizationSnapshotRow> Rows { get; set; }
    }

    public sealed class AuthorizationSnapshotMetrics
    {
        public int EmployeesScanned { get; set; }
        public int PermissionsEvaluated { get; set; }
        public int Allowed { get; set; }
        public int Denied { get; set; }
        public int LegacyConfigGrants { get; set; }
        public int LegacyHardcodedGrants { get; set; }
        public int ModuleGrants { get; set; }
        public int OverlayGrants { get; set; }
    }

    public sealed class AuthorizationSnapshotChange
    {
        public string WorkmanSL { get; set; }
        public string LoginID { get; set; }
        public string Code { get; set; }
        public string Kind { get; set; }
        public string BeforeGranted { get; set; }
        public string AfterGranted { get; set; }
        public string BeforeSource { get; set; }
        public string AfterSource { get; set; }
    }

    public sealed class AuthorizationSnapshotComparison
    {
        public AuthorizationSnapshotParsed Before { get; set; }
        public AuthorizationSnapshotParsed After { get; set; }
        public AuthorizationSnapshotMetrics BeforeMetrics { get; set; }
        public AuthorizationSnapshotMetrics AfterMetrics { get; set; }
        public bool PayloadShaEqual { get; set; }
        public int ChangedEffectivePermissionCount { get; set; }
        public int SourceOnlyChangeCount { get; set; }
        public List<AuthorizationSnapshotChange> Changes { get; set; }
        public string Error { get; set; }

        public bool ZeroDrift
        {
            get
            {
                return string.IsNullOrEmpty(Error) && ChangedEffectivePermissionCount == 0;
            }
        }
    }

    public static class AuthorizationSnapshot
    {
        public const int ScanLimit = 1500;

        public static AuthorizationSnapshotDocument Generate(HttpSessionState session, DB_Utility_OH4Y dbcl)
        {
            if (dbcl == null) throw new ArgumentNullException("dbcl");

            DataTable employees = dbcl.SPreturn_dt(@"
SELECT TOP " + ScanLimit.ToString() + @"
    LoginID, WorkmanSL, FullName, User_RoleType, UserRoleDB, RolePermissionDB
FROM tbl_Employee_Mustertable
WHERE WorkStatus = N'Active'
  AND ISNULL(LoginID, N'') <> N''
  AND ISNULL(WorkmanSL, N'') <> N''
ORDER BY WorkmanSL, LoginID", new SqlParameter[0]);
            if (employees == null) employees = new DataTable();

            List<string> workmans = new List<string>();
            for (int i = 0; i < employees.Rows.Count; i++)
            {
                string workman = Read(employees.Rows[i], "WorkmanSL");
                if (workman.Length > 0) workmans.Add(workman);
            }
            PermissionRepository.WarmSnapshots(workmans);

            List<string> lines = new List<string>();
            int grants = 0;

            for (int i = 0; i < employees.Rows.Count; i++)
            {
                DataRow row = employees.Rows[i];
                bool authenticated, admin, canImpersonate, payrollConfig;
                IList<EffectivePermission> permissions;
                AuthorizationService.DescribeIdentity(
                    session,
                    Read(row, "LoginID"),
                    Read(row, "RolePermissionDB"),
                    Read(row, "UserRoleDB"),
                    Read(row, "FullName"),
                    Read(row, "WorkmanSL"),
                    Read(row, "User_RoleType"),
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
                    if (item.Granted) grants++;
                    lines.Add(
                        Read(row, "WorkmanSL") + "|"
                        + Read(row, "LoginID") + "|"
                        + Read(row, "User_RoleType") + "|"
                        + (item.Code ?? "") + "|"
                        + (item.Granted ? "1" : "0") + "|"
                        + (item.Source ?? "") + "|"
                        + AuthorizationService.DisplaySource(item.Source, item.Granted));
                }
            }

            lines.Sort(StringComparer.Ordinal);
            StringBuilder payload = new StringBuilder();
            for (int i = 0; i < lines.Count; i++)
            {
                payload.Append(lines[i]);
                payload.Append("\n");
            }

            string generated = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string hash = Sha256Hex(payload.ToString());
            bool truncated = employees.Rows.Count >= ScanLimit;

            StringBuilder body = new StringBuilder();
            body.AppendLine("ATS-AUTHORIZATION-SNAPSHOT");
            body.AppendLine("GeneratedUtc=" + generated);
            body.AppendLine("Sha256=" + hash);
            body.AppendLine("EmployeeCount=" + employees.Rows.Count.ToString());
            body.AppendLine("GrantCount=" + grants.ToString());
            body.AppendLine("RowCount=" + lines.Count.ToString());
            body.AppendLine("Truncated=" + (truncated ? "1" : "0"));
            body.AppendLine("ScanLimit=" + ScanLimit.ToString());
            body.AppendLine("--");
            body.Append("WorkmanSL|LoginID|UserType|Code|Granted|Source|Display");
            body.Append("\n");
            body.Append(payload.ToString());

            AuthorizationSnapshotDocument doc = new AuthorizationSnapshotDocument();
            doc.GeneratedUtc = generated;
            doc.Sha256 = hash;
            doc.EmployeeCount = employees.Rows.Count;
            doc.GrantCount = grants;
            doc.RowCount = lines.Count;
            doc.Truncated = truncated;
            doc.Body = body.ToString();
            return doc;
        }

        public static AuthorizationSnapshotParsed Parse(string body)
        {
            AuthorizationSnapshotParsed parsed = new AuthorizationSnapshotParsed();
            parsed.Rows = new List<AuthorizationSnapshotRow>();
            parsed.GeneratedUtc = "";
            parsed.Sha256 = "";
            parsed.PayloadSha256 = "";
            parsed.Error = "";
            if (string.IsNullOrEmpty(body))
            {
                parsed.Error = "Snapshot is empty.";
                return parsed;
            }

            string[] lines = SplitLines(body);
            if (lines.Length == 0 || !string.Equals(lines[0], "ATS-AUTHORIZATION-SNAPSHOT", StringComparison.Ordinal))
            {
                parsed.Error = "Not an ATS authorization snapshot.";
                return parsed;
            }

            int dash = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "--")
                {
                    dash = i;
                    break;
                }
            }
            if (dash < 0)
            {
                parsed.Error = "Snapshot is missing the payload separator.";
                return parsed;
            }

            for (int i = 1; i < dash; i++)
            {
                string line = lines[i];
                int eq = line.IndexOf('=');
                if (eq <= 0) continue;
                string key = line.Substring(0, eq);
                string value = line.Substring(eq + 1);
                if (key == "GeneratedUtc") parsed.GeneratedUtc = value;
                else if (key == "Sha256") parsed.Sha256 = value;
                else if (key == "EmployeeCount") parsed.EmployeeCount = ParseInt(value);
                else if (key == "GrantCount") parsed.GrantCount = ParseInt(value);
                else if (key == "RowCount") parsed.RowCount = ParseInt(value);
                else if (key == "Truncated") parsed.Truncated = value == "1";
                else if (key == "ScanLimit") parsed.ScanLimit = ParseInt(value);
            }

            int start = dash + 1;
            if (start < lines.Length && lines[start].IndexOf("WorkmanSL|", StringComparison.Ordinal) == 0)
            {
                start++;
            }

            List<string> payloadLines = new List<string>();
            for (int i = start; i < lines.Length; i++)
            {
                if (i == lines.Length - 1 && lines[i].Length == 0) continue;
                payloadLines.Add(lines[i]);
                AuthorizationSnapshotRow row = ParseRow(lines[i]);
                if (row != null) parsed.Rows.Add(row);
            }

            StringBuilder payload = new StringBuilder();
            for (int i = 0; i < payloadLines.Count; i++)
            {
                payload.Append(payloadLines[i]);
                payload.Append("\n");
            }
            parsed.PayloadSha256 = Sha256Hex(payload.ToString());
            parsed.HeaderHashMatchesPayload = string.Equals(
                parsed.Sha256,
                parsed.PayloadSha256,
                StringComparison.OrdinalIgnoreCase);
            if (parsed.Rows.Count != parsed.RowCount && parsed.RowCount > 0)
            {
                parsed.Error = "RowCount header does not match payload rows.";
            }
            else if (!parsed.HeaderHashMatchesPayload)
            {
                parsed.Error = "Header Sha256 does not match the sorted payload.";
            }
            return parsed;
        }

        public static AuthorizationSnapshotMetrics Summarize(AuthorizationSnapshotParsed parsed)
        {
            AuthorizationSnapshotMetrics metrics = new AuthorizationSnapshotMetrics();
            if (parsed == null) return metrics;
            metrics.EmployeesScanned = parsed.EmployeeCount;
            if (parsed.Rows == null) return metrics;
            metrics.PermissionsEvaluated = parsed.Rows.Count;
            for (int i = 0; i < parsed.Rows.Count; i++)
            {
                AuthorizationSnapshotRow row = parsed.Rows[i];
                if (row == null) continue;
                if (row.Granted) metrics.Allowed++;
                else metrics.Denied++;
                int bucket = GrantBucket(row.Source, row.Granted);
                if (bucket == 0) metrics.OverlayGrants++;
                else if (bucket == 1) metrics.LegacyConfigGrants++;
                else if (bucket == 2) metrics.LegacyHardcodedGrants++;
                else if (bucket == 3) metrics.ModuleGrants++;
            }
            return metrics;
        }

        public static AuthorizationSnapshotComparison Compare(string beforeBody, string afterBody)
        {
            AuthorizationSnapshotComparison result = new AuthorizationSnapshotComparison();
            result.Changes = new List<AuthorizationSnapshotChange>();
            result.Before = Parse(beforeBody);
            result.After = Parse(afterBody);
            result.BeforeMetrics = Summarize(result.Before);
            result.AfterMetrics = Summarize(result.After);

            if (result.Before != null && !string.IsNullOrEmpty(result.Before.Error))
            {
                result.Error = "Before snapshot: " + result.Before.Error;
                return result;
            }
            if (result.After != null && !string.IsNullOrEmpty(result.After.Error))
            {
                result.Error = "After snapshot: " + result.After.Error;
                return result;
            }

            string beforeHash = result.Before != null ? result.Before.PayloadSha256 : "";
            string afterHash = result.After != null ? result.After.PayloadSha256 : "";
            result.PayloadShaEqual = string.Equals(beforeHash, afterHash, StringComparison.OrdinalIgnoreCase);

            Dictionary<string, AuthorizationSnapshotRow> beforeMap = IndexRows(result.Before);
            Dictionary<string, AuthorizationSnapshotRow> afterMap = IndexRows(result.After);

            foreach (KeyValuePair<string, AuthorizationSnapshotRow> pair in beforeMap)
            {
                AuthorizationSnapshotRow beforeRow = pair.Value;
                AuthorizationSnapshotRow afterRow;
                if (!afterMap.TryGetValue(pair.Key, out afterRow))
                {
                    result.ChangedEffectivePermissionCount++;
                    result.Changes.Add(MakeChange(beforeRow, null, "Removed"));
                    continue;
                }
                if (beforeRow.Granted != afterRow.Granted)
                {
                    result.ChangedEffectivePermissionCount++;
                    result.Changes.Add(MakeChange(beforeRow, afterRow, "EffectiveAccess"));
                }
                else if (!string.Equals(beforeRow.Line ?? "", afterRow.Line ?? "", StringComparison.Ordinal))
                {
                    result.SourceOnlyChangeCount++;
                    result.Changes.Add(MakeChange(beforeRow, afterRow, "SourceOnly"));
                }
            }
            foreach (KeyValuePair<string, AuthorizationSnapshotRow> pair in afterMap)
            {
                if (beforeMap.ContainsKey(pair.Key)) continue;
                result.ChangedEffectivePermissionCount++;
                result.Changes.Add(MakeChange(null, pair.Value, "Added"));
            }

            result.Changes.Sort(CompareChanges);
            return result;
        }

        private static Dictionary<string, AuthorizationSnapshotRow> IndexRows(AuthorizationSnapshotParsed parsed)
        {
            Dictionary<string, AuthorizationSnapshotRow> map = new Dictionary<string, AuthorizationSnapshotRow>(StringComparer.Ordinal);
            if (parsed == null || parsed.Rows == null) return map;
            for (int i = 0; i < parsed.Rows.Count; i++)
            {
                AuthorizationSnapshotRow row = parsed.Rows[i];
                if (row == null) continue;
                string key = row.DecisionKey;
                if (!map.ContainsKey(key)) map[key] = row;
            }
            return map;
        }

        private static AuthorizationSnapshotChange MakeChange(
            AuthorizationSnapshotRow beforeRow,
            AuthorizationSnapshotRow afterRow,
            string kind)
        {
            AuthorizationSnapshotRow sample = afterRow != null ? afterRow : beforeRow;
            AuthorizationSnapshotChange change = new AuthorizationSnapshotChange();
            change.Kind = kind;
            change.WorkmanSL = sample != null ? sample.WorkmanSL : "";
            change.LoginID = sample != null ? sample.LoginID : "";
            change.Code = sample != null ? sample.Code : "";
            change.BeforeGranted = beforeRow == null ? "" : (beforeRow.Granted ? "1" : "0");
            change.AfterGranted = afterRow == null ? "" : (afterRow.Granted ? "1" : "0");
            change.BeforeSource = beforeRow != null ? beforeRow.Source : "";
            change.AfterSource = afterRow != null ? afterRow.Source : "";
            return change;
        }

        private static int CompareChanges(AuthorizationSnapshotChange a, AuthorizationSnapshotChange b)
        {
            if (a == null && b == null) return 0;
            if (a == null) return -1;
            if (b == null) return 1;
            int kind = string.CompareOrdinal(a.Kind, b.Kind);
            if (kind != 0) return kind;
            int workman = string.CompareOrdinal(a.WorkmanSL, b.WorkmanSL);
            if (workman != 0) return workman;
            int login = string.CompareOrdinal(a.LoginID, b.LoginID);
            if (login != 0) return login;
            return string.CompareOrdinal(a.Code, b.Code);
        }

        private static AuthorizationSnapshotRow ParseRow(string line)
        {
            if (string.IsNullOrEmpty(line)) return null;
            string[] parts = line.Split(new char[] { '|' });
            if (parts.Length < 7) return null;
            AuthorizationSnapshotRow row = new AuthorizationSnapshotRow();
            row.WorkmanSL = parts[0];
            row.LoginID = parts[1];
            row.UserType = parts[2];
            row.Code = parts[3];
            row.Granted = parts[4] == "1";
            row.Source = parts[5];
            if (parts.Length == 7)
            {
                row.Display = parts[6];
            }
            else
            {
                StringBuilder display = new StringBuilder();
                for (int i = 6; i < parts.Length; i++)
                {
                    if (i > 6) display.Append("|");
                    display.Append(parts[i]);
                }
                row.Display = display.ToString();
            }
            row.Line = line;
            return row;
        }

        private static int GrantBucket(string source, bool granted)
        {
            if (!granted) return -1;
            if (string.Equals(source, AuthorizationService.SourceDirect, StringComparison.OrdinalIgnoreCase)
                || string.Equals(source, AuthorizationService.SourceGroup, StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }
            if (string.Equals(source, AuthorizationService.SourceLegacyHardcoded, StringComparison.OrdinalIgnoreCase))
            {
                return 2;
            }
            if (string.Equals(source, AuthorizationService.SourceModuleException, StringComparison.OrdinalIgnoreCase))
            {
                return 3;
            }
            if (string.Equals(source, AuthorizationService.SourceLegacyConfig, StringComparison.OrdinalIgnoreCase)
                || (source != null && source.IndexOf(AuthorizationService.SourceLegacyConfig, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                return 1;
            }
            return -1;
        }

        private static string[] SplitLines(string text)
        {
            if (text == null) return new string[0];
            return text.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
        }

        private static int ParseInt(string value)
        {
            int parsed;
            if (int.TryParse(value, out parsed)) return parsed;
            return 0;
        }

        private static string Sha256Hex(string text)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text ?? ""));
                StringBuilder hex = new StringBuilder(bytes.Length * 2);
                for (int i = 0; i < bytes.Length; i++)
                {
                    hex.Append(bytes[i].ToString("x2"));
                }
                return hex.ToString();
            }
        }

        private static string Read(DataRow row, string column)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(column) || row[column] == DBNull.Value)
            {
                return "";
            }
            return row[column].ToString();
        }
    }
}
