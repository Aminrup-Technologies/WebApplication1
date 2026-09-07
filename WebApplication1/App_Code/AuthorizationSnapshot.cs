/*
 * WHEN: 2026-09-07
 * WHY: Authorization modernization PR D. Read-only baseline before further migration / CRUD.
 * WHAT: Snapshot every Active employee's effective permissions via DescribeIdentity.
 *      SHA-256 over the canonical row payload. No writes to overlay or Session identity.
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
