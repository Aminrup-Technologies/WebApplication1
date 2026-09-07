/*
 * WHEN: 2026-09-07
 * WHY: Authorization modernization PR B. Overlay infrastructure behind AuthorizationService.
 * WHAT: Load the permission catalog; resolve group membership and direct grants for a WorkmanSL;
 *      cache snapshots in HttpRuntime.Cache (5-minute TTL). Missing tables fail closed (empty
 *      grant set). Permission Inspector and Access Analyzer read this API; they do not write grants.
 *      Does not read tlb_EmployeePermissions (legacy menus).
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;

namespace WebApplication1.bussiness.production
{
    public sealed class OverlayPermissionDefinition
    {
        public string PermissionCode { get; set; }
        public string Name { get; set; }
        public string Module { get; set; }
        public string Description { get; set; }
    }

    public static class PermissionRepository
    {
        public const int CacheTtlMinutes = 5;
        public const string SourceDirect = "DIRECT";
        public const string SourceGroup = "GROUP";

        private const string VersionKey = "perm-overlay-ver";
        private const string SnapshotPrefix = "perm-overlay-";
        private const string CatalogKeyPrefix = "perm-overlay-catalog-";

        public static bool HasPermission(string workmanSL, string permissionCode)
        {
            string source;
            return TryGetSource(workmanSL, permissionCode, out source);
        }

        public static bool TryGetSource(string workmanSL, string permissionCode, out string source)
        {
            source = "";
            if (string.IsNullOrWhiteSpace(workmanSL) || string.IsNullOrWhiteSpace(permissionCode)) return false;
            OverlaySnapshot snap = LoadSnapshot(workmanSL);
            if (snap == null || snap.Codes == null) return false;
            string key = permissionCode.Trim();
            if (!snap.Codes.ContainsKey(key)) return false;
            source = snap.Codes[key];
            return true;
        }

        public static IList<string> GetPermissionCodes(string workmanSL)
        {
            return CodesWhere(workmanSL, null);
        }

        public static IList<string> GetDirectPermissionCodes(string workmanSL)
        {
            return CodesWhere(workmanSL, SourceDirect);
        }

        public static IList<string> GetGroupPermissionCodes(string workmanSL)
        {
            return CodesWhere(workmanSL, SourceGroup);
        }

        public static IList<string> GetGroupCodes(string workmanSL)
        {
            List<string> list = new List<string>();
            OverlaySnapshot snap = LoadSnapshot(workmanSL);
            if (snap == null || snap.Groups == null) return list;
            for (int i = 0; i < snap.Groups.Count; i++)
            {
                list.Add(snap.Groups[i]);
            }
            return list;
        }

        public static IList<OverlayPermissionDefinition> GetActivePermissions()
        {
            List<OverlayPermissionDefinition> cached = LoadCatalog();
            List<OverlayPermissionDefinition> copy = new List<OverlayPermissionDefinition>();
            if (cached == null) return copy;
            for (int i = 0; i < cached.Count; i++)
            {
                OverlayPermissionDefinition src = cached[i];
                OverlayPermissionDefinition item = new OverlayPermissionDefinition();
                item.PermissionCode = src.PermissionCode;
                item.Name = src.Name;
                item.Module = src.Module;
                item.Description = src.Description;
                copy.Add(item);
            }
            return copy;
        }

        public static bool IsRuntimeCacheAvailable()
        {
            return RuntimeCache() != null;
        }

        public static bool PeekSnapshotCached(string workmanSL)
        {
            if (string.IsNullOrWhiteSpace(workmanSL)) return false;
            Cache cache = RuntimeCache();
            if (cache == null) return false;
            return cache[SnapshotKey(workmanSL.Trim())] != null;
        }

        public static DateTime GetSnapshotLoadedAtUtc(string workmanSL)
        {
            OverlaySnapshot snap = LoadSnapshot(workmanSL);
            if (snap == null) return DateTime.MinValue;
            return snap.LoadedAtUtc;
        }

        public static bool IsOverlaySchemaAvailable()
        {
            string cnn = ConnectionString();
            if (string.IsNullOrEmpty(cnn)) return false;
            try
            {
                using (SqlConnection conn = new SqlConnection(cnn))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(@"
SELECT CASE WHEN
    OBJECT_ID(N'dbo.tlb_permissions', N'U') IS NOT NULL
    AND OBJECT_ID(N'dbo.tlb_permission_groups', N'U') IS NOT NULL
    AND OBJECT_ID(N'dbo.tlb_group_permissions', N'U') IS NOT NULL
    AND OBJECT_ID(N'dbo.tlb_employee_group', N'U') IS NOT NULL
    AND OBJECT_ID(N'dbo.tlb_employee_permissions', N'U') IS NOT NULL
THEN 1 ELSE 0 END", conn))
                    {
                        object value = cmd.ExecuteScalar();
                        if (value == null || value == DBNull.Value) return false;
                        return Convert.ToInt32(value) == 1;
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
            catch (ConfigurationErrorsException)
            {
                return false;
            }
        }

        public static void Invalidate(string workmanSL)
        {
            if (string.IsNullOrWhiteSpace(workmanSL)) return;
            Cache cache = RuntimeCache();
            if (cache == null) return;
            cache.Remove(SnapshotKey(workmanSL));
        }

        public static void InvalidateAll()
        {
            Cache cache = RuntimeCache();
            if (cache == null) return;
            int next = ReadVersion(cache) + 1;
            cache.Insert(VersionKey, next, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
        }

        private static IList<string> CodesWhere(string workmanSL, string sourceFilter)
        {
            List<string> list = new List<string>();
            OverlaySnapshot snap = LoadSnapshot(workmanSL);
            if (snap == null || snap.Codes == null) return list;
            foreach (KeyValuePair<string, string> pair in snap.Codes)
            {
                if (sourceFilter == null || string.Equals(pair.Value, sourceFilter, StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(pair.Key);
                }
            }
            return list;
        }

        public static void WarmSnapshots(IList<string> workmanSLs)
        {
            if (workmanSLs == null || workmanSLs.Count == 0) return;
            Cache cache = RuntimeCache();
            Dictionary<string, OverlaySnapshot> loaded = new Dictionary<string, OverlaySnapshot>(StringComparer.OrdinalIgnoreCase);
            if (IsOverlaySchemaAvailable())
            {
                loaded = QueryAllSnapshots();
            }

            DateTime now = DateTime.UtcNow;
            for (int i = 0; i < workmanSLs.Count; i++)
            {
                string workman = (workmanSLs[i] ?? "").Trim();
                if (workman.Length == 0) continue;
                if (PeekSnapshotCached(workman)) continue;

                OverlaySnapshot snap;
                if (!loaded.TryGetValue(workman, out snap) || snap == null)
                {
                    snap = new OverlaySnapshot();
                    snap.Codes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    snap.Groups = new List<string>();
                    snap.LoadedAtUtc = now;
                }

                if (cache != null)
                {
                    cache.Insert(
                        SnapshotKey(workman),
                        snap,
                        null,
                        DateTime.UtcNow.AddMinutes(CacheTtlMinutes),
                        Cache.NoSlidingExpiration);
                }
            }
        }

        public static DataTable GetDirectGrantInventory()
        {
            return QueryTable(@"
SELECT ep.WorkmanSL, p.PermissionCode
FROM dbo.tlb_employee_permissions ep
INNER JOIN dbo.tlb_permissions p ON p.Id = ep.PermissionId AND p.IsActive = 1
ORDER BY ep.WorkmanSL, p.PermissionCode", "WorkmanSL", "PermissionCode");
        }

        public static DataTable GetGroupGrantInventory()
        {
            return QueryTable(@"
SELECT eg.WorkmanSL, g.GroupCode, p.PermissionCode
FROM dbo.tlb_employee_group eg
INNER JOIN dbo.tlb_permission_groups g ON g.Id = eg.GroupId AND g.IsActive = 1
INNER JOIN dbo.tlb_group_permissions gp ON gp.GroupId = g.Id
INNER JOIN dbo.tlb_permissions p ON p.Id = gp.PermissionId AND p.IsActive = 1
ORDER BY eg.WorkmanSL, g.GroupCode, p.PermissionCode", "WorkmanSL", "GroupCode", "PermissionCode");
        }

        public static DataTable GetGroupCatalogInventory()
        {
            return QueryTable(@"
SELECT g.GroupCode, g.Name,
    (SELECT COUNT(*) FROM dbo.tlb_employee_group eg WHERE eg.GroupId = g.Id) AS MemberCount,
    (SELECT COUNT(*) FROM dbo.tlb_group_permissions gp WHERE gp.GroupId = g.Id) AS PermissionCount
FROM dbo.tlb_permission_groups g
WHERE g.IsActive = 1
ORDER BY g.GroupCode", "GroupCode", "Name", "MemberCount", "PermissionCount");
        }

        public static IList<string> GetUnusedPermissionCodes()
        {
            List<string> unused = new List<string>();
            IList<OverlayPermissionDefinition> catalog = GetActivePermissions();
            DataTable direct = GetDirectGrantInventory();
            DataTable groups = GetGroupGrantInventory();
            Dictionary<string, bool> used = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            AddCodes(used, direct, "PermissionCode");
            AddCodes(used, groups, "PermissionCode");
            for (int i = 0; i < catalog.Count; i++)
            {
                string code = catalog[i].PermissionCode;
                if (string.IsNullOrWhiteSpace(code)) continue;
                if (!used.ContainsKey(code)) unused.Add(code);
            }
            return unused;
        }

        private static void AddCodes(Dictionary<string, bool> used, DataTable table, string column)
        {
            if (table == null || !table.Columns.Contains(column)) return;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                object raw = table.Rows[i][column];
                if (raw == null || raw == DBNull.Value) continue;
                string code = raw.ToString();
                if (!string.IsNullOrWhiteSpace(code)) used[code] = true;
            }
        }

        private static DataTable QueryTable(string sql, params string[] columns)
        {
            DataTable table = new DataTable();
            if (columns != null)
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    table.Columns.Add(columns[i]);
                }
            }
            string cnn = ConnectionString();
            if (string.IsNullOrEmpty(cnn) || !IsOverlaySchemaAvailable()) return table;
            try
            {
                using (SqlConnection conn = new SqlConnection(cnn))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow row = table.NewRow();
                                for (int c = 0; c < table.Columns.Count; c++)
                                {
                                    string name = table.Columns[c].ColumnName;
                                    row[name] = reader[name] != DBNull.Value ? reader[name].ToString() : "";
                                }
                                table.Rows.Add(row);
                            }
                        }
                    }
                }
            }
            catch (SqlException)
            {
                table.Rows.Clear();
            }
            catch (ConfigurationErrorsException)
            {
                table.Rows.Clear();
            }
            return table;
        }

        private static Dictionary<string, OverlaySnapshot> QueryAllSnapshots()
        {
            Dictionary<string, OverlaySnapshot> map = new Dictionary<string, OverlaySnapshot>(StringComparer.OrdinalIgnoreCase);
            string cnn = ConnectionString();
            if (string.IsNullOrEmpty(cnn)) return map;
            DateTime now = DateTime.UtcNow;
            try
            {
                using (SqlConnection conn = new SqlConnection(cnn))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(@"
SELECT ep.WorkmanSL, p.PermissionCode, N'DIRECT' AS GrantSource
FROM dbo.tlb_employee_permissions ep
INNER JOIN dbo.tlb_permissions p ON p.Id = ep.PermissionId AND p.IsActive = 1
UNION ALL
SELECT eg.WorkmanSL, p.PermissionCode, N'GROUP' AS GrantSource
FROM dbo.tlb_employee_group eg
INNER JOIN dbo.tlb_permission_groups g ON g.Id = eg.GroupId AND g.IsActive = 1
INNER JOIN dbo.tlb_group_permissions gp ON gp.GroupId = g.Id
INNER JOIN dbo.tlb_permissions p ON p.Id = gp.PermissionId AND p.IsActive = 1", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string workman = reader["WorkmanSL"] != DBNull.Value ? reader["WorkmanSL"].ToString() : "";
                                string code = reader["PermissionCode"] != DBNull.Value ? reader["PermissionCode"].ToString() : "";
                                string grantSource = reader["GrantSource"] != DBNull.Value ? reader["GrantSource"].ToString() : SourceGroup;
                                if (string.IsNullOrWhiteSpace(workman) || string.IsNullOrWhiteSpace(code)) continue;
                                OverlaySnapshot snap = GetOrCreate(map, workman, now);
                                string existing;
                                if (snap.Codes.TryGetValue(code, out existing)
                                    && string.Equals(existing, SourceDirect, StringComparison.OrdinalIgnoreCase))
                                {
                                    continue;
                                }
                                snap.Codes[code] = string.Equals(grantSource, SourceDirect, StringComparison.OrdinalIgnoreCase)
                                    ? SourceDirect
                                    : SourceGroup;
                            }
                        }
                    }

                    using (SqlCommand groups = new SqlCommand(@"
SELECT eg.WorkmanSL, g.GroupCode
FROM dbo.tlb_employee_group eg
INNER JOIN dbo.tlb_permission_groups g ON g.Id = eg.GroupId AND g.IsActive = 1", conn))
                    {
                        using (SqlDataReader reader = groups.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string workman = reader["WorkmanSL"] != DBNull.Value ? reader["WorkmanSL"].ToString() : "";
                                string groupCode = reader["GroupCode"] != DBNull.Value ? reader["GroupCode"].ToString() : "";
                                if (string.IsNullOrWhiteSpace(workman) || string.IsNullOrWhiteSpace(groupCode)) continue;
                                OverlaySnapshot snap = GetOrCreate(map, workman, now);
                                snap.Groups.Add(groupCode);
                            }
                        }
                    }
                }
            }
            catch (SqlException)
            {
                map.Clear();
            }
            catch (ConfigurationErrorsException)
            {
                map.Clear();
            }
            return map;
        }

        private static OverlaySnapshot GetOrCreate(Dictionary<string, OverlaySnapshot> map, string workman, DateTime loadedAtUtc)
        {
            OverlaySnapshot snap;
            if (map.TryGetValue(workman, out snap) && snap != null) return snap;
            snap = new OverlaySnapshot();
            snap.Codes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            snap.Groups = new List<string>();
            snap.LoadedAtUtc = loadedAtUtc;
            map[workman] = snap;
            return snap;
        }

        private static OverlaySnapshot LoadSnapshot(string workmanSL)
        {
            if (string.IsNullOrWhiteSpace(workmanSL))
            {
                OverlaySnapshot empty = new OverlaySnapshot();
                empty.Codes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                empty.Groups = new List<string>();
                empty.LoadedAtUtc = DateTime.UtcNow;
                return empty;
            }

            string workman = workmanSL.Trim();
            Cache cache = RuntimeCache();
            string key = SnapshotKey(workman);
            if (cache != null)
            {
                OverlaySnapshot cached = cache[key] as OverlaySnapshot;
                if (cached != null) return cached;
            }

            OverlaySnapshot snap = QuerySnapshot(workman);
            if (cache != null && snap != null)
            {
                cache.Insert(
                    key,
                    snap,
                    null,
                    DateTime.UtcNow.AddMinutes(CacheTtlMinutes),
                    Cache.NoSlidingExpiration);
            }
            return snap;
        }

        private static List<OverlayPermissionDefinition> LoadCatalog()
        {
            Cache cache = RuntimeCache();
            string key = CatalogKey();
            if (cache != null)
            {
                List<OverlayPermissionDefinition> cached = cache[key] as List<OverlayPermissionDefinition>;
                if (cached != null) return cached;
            }

            List<OverlayPermissionDefinition> catalog = QueryCatalog();
            if (cache != null && catalog != null)
            {
                cache.Insert(
                    key,
                    catalog,
                    null,
                    DateTime.UtcNow.AddMinutes(CacheTtlMinutes),
                    Cache.NoSlidingExpiration);
            }
            return catalog;
        }

        private static OverlaySnapshot QuerySnapshot(string workmanSL)
        {
            OverlaySnapshot snap = new OverlaySnapshot();
            snap.Codes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            snap.Groups = new List<string>();
            snap.LoadedAtUtc = DateTime.UtcNow;

            string cnn = ConnectionString();
            if (string.IsNullOrEmpty(cnn)) return snap;

            try
            {
                using (SqlConnection conn = new SqlConnection(cnn))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(@"
SELECT p.PermissionCode, N'DIRECT' AS GrantSource
FROM dbo.tlb_employee_permissions ep
INNER JOIN dbo.tlb_permissions p ON p.Id = ep.PermissionId AND p.IsActive = 1
WHERE ep.WorkmanSL = @Workman
UNION ALL
SELECT p.PermissionCode, N'GROUP' AS GrantSource
FROM dbo.tlb_employee_group eg
INNER JOIN dbo.tlb_permission_groups g ON g.Id = eg.GroupId AND g.IsActive = 1
INNER JOIN dbo.tlb_group_permissions gp ON gp.GroupId = g.Id
INNER JOIN dbo.tlb_permissions p ON p.Id = gp.PermissionId AND p.IsActive = 1
WHERE eg.WorkmanSL = @Workman", conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@Workman", SqlDbType.NVarChar, 50)).Value = workmanSL;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string code = reader["PermissionCode"] != DBNull.Value ? reader["PermissionCode"].ToString() : "";
                                string grantSource = reader["GrantSource"] != DBNull.Value ? reader["GrantSource"].ToString() : SourceGroup;
                                if (string.IsNullOrWhiteSpace(code)) continue;
                                string existing;
                                if (snap.Codes.TryGetValue(code, out existing)
                                    && string.Equals(existing, SourceDirect, StringComparison.OrdinalIgnoreCase))
                                {
                                    continue;
                                }
                                snap.Codes[code] = string.Equals(grantSource, SourceDirect, StringComparison.OrdinalIgnoreCase)
                                    ? SourceDirect
                                    : SourceGroup;
                            }
                        }
                    }

                    using (SqlCommand groups = new SqlCommand(@"
SELECT g.GroupCode
FROM dbo.tlb_employee_group eg
INNER JOIN dbo.tlb_permission_groups g ON g.Id = eg.GroupId AND g.IsActive = 1
WHERE eg.WorkmanSL = @Workman
ORDER BY g.GroupCode", conn))
                    {
                        groups.Parameters.Add(new SqlParameter("@Workman", SqlDbType.NVarChar, 50)).Value = workmanSL;
                        using (SqlDataReader reader = groups.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string groupCode = reader["GroupCode"] != DBNull.Value ? reader["GroupCode"].ToString() : "";
                                if (!string.IsNullOrWhiteSpace(groupCode)) snap.Groups.Add(groupCode);
                            }
                        }
                    }
                }
            }
            catch (SqlException)
            {
                snap.Codes.Clear();
                snap.Groups.Clear();
            }
            catch (ConfigurationErrorsException)
            {
                snap.Codes.Clear();
                snap.Groups.Clear();
            }

            return snap;
        }

        private static List<OverlayPermissionDefinition> QueryCatalog()
        {
            List<OverlayPermissionDefinition> list = new List<OverlayPermissionDefinition>();
            string cnn = ConnectionString();
            if (string.IsNullOrEmpty(cnn)) return list;

            try
            {
                using (SqlConnection conn = new SqlConnection(cnn))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(@"
SELECT PermissionCode, Name, Module, Description
FROM dbo.tlb_permissions
WHERE IsActive = 1
ORDER BY Module, PermissionCode", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OverlayPermissionDefinition item = new OverlayPermissionDefinition();
                                item.PermissionCode = reader["PermissionCode"] != DBNull.Value ? reader["PermissionCode"].ToString() : "";
                                item.Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : "";
                                item.Module = reader["Module"] != DBNull.Value ? reader["Module"].ToString() : "";
                                item.Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "";
                                if (!string.IsNullOrWhiteSpace(item.PermissionCode)) list.Add(item);
                            }
                        }
                    }
                }
            }
            catch (SqlException)
            {
                list.Clear();
            }
            catch (ConfigurationErrorsException)
            {
                list.Clear();
            }

            return list;
        }

        private static string ConnectionString()
        {
            if (ConfigurationManager.ConnectionStrings["DbConn"] == null) return "";
            return ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString ?? "";
        }

        private static string SnapshotKey(string workmanSL)
        {
            return SnapshotPrefix + CurrentVersion().ToString() + ":" + workmanSL.Trim().ToUpperInvariant();
        }

        private static string CatalogKey()
        {
            return CatalogKeyPrefix + CurrentVersion().ToString();
        }

        private static int CurrentVersion()
        {
            Cache cache = RuntimeCache();
            if (cache == null) return 0;
            return ReadVersion(cache);
        }

        private static int ReadVersion(Cache cache)
        {
            object raw = cache[VersionKey];
            if (raw is int) return (int)raw;
            cache.Insert(VersionKey, 1, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
            return 1;
        }

        private static Cache RuntimeCache()
        {
            try
            {
                return HttpRuntime.Cache;
            }
            catch
            {
                return null;
            }
        }

        private sealed class OverlaySnapshot
        {
            public Dictionary<string, string> Codes;
            public List<string> Groups;
            public DateTime LoadedAtUtc;
        }
    }
}
