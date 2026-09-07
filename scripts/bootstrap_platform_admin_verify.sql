-- When: 2026-09-07
-- Why: Verify Platform Admin overlay pack (PR #104).
-- What: Read-only report. No INSERT/UPDATE/DELETE.

SET NOCOUNT ON;

IF OBJECT_ID(N'dbo.tlb_permissions', N'U') IS NULL
   OR OBJECT_ID(N'dbo.tlb_employee_permissions', N'U') IS NULL
   OR OBJECT_ID(N'dbo.tbl_Employee_Mustertable', N'U') IS NULL
BEGIN
    RAISERROR(N'Overlay or employee tables missing.', 16, 1);
    RETURN;
END

DECLARE @Catalog TABLE (PermissionCode NVARCHAR(64) NOT NULL PRIMARY KEY);
INSERT INTO @Catalog (PermissionCode)
VALUES (N'SWITCH_USER'), (N'USER_ADMIN'), (N'PAYROLL_OVERRIDE'), (N'EXPORT_PAYROLL');

/* 1 — Active Admin count */
SELECT COUNT(*) AS ActiveAdminCount
FROM dbo.tbl_Employee_Mustertable
WHERE WorkStatus = N'Active'
  AND User_RoleType = N'Admin';

SELECT WorkmanSL, LoginID, FullName, User_RoleType, UserRoleDB, RolePermissionDB, WorkStatus
FROM dbo.tbl_Employee_Mustertable
WHERE WorkStatus = N'Active'
  AND User_RoleType = N'Admin'
ORDER BY WorkmanSL;

/* 2 — Overlay grants (pack codes × Active Admin) */
SELECT
    e.WorkmanSL,
    e.LoginID,
    p.PermissionCode,
    CASE WHEN ep.PermissionId IS NULL THEN N'MISSING' ELSE N'GRANTED' END AS GrantState
FROM dbo.tbl_Employee_Mustertable e
CROSS JOIN dbo.tlb_permissions p
INNER JOIN @Catalog c ON c.PermissionCode = p.PermissionCode
LEFT JOIN dbo.tlb_employee_permissions ep
    ON ep.WorkmanSL = e.WorkmanSL
   AND ep.PermissionId = p.Id
WHERE e.WorkStatus = N'Active'
  AND e.User_RoleType = N'Admin'
ORDER BY e.WorkmanSL, p.PermissionCode;

SELECT
    p.PermissionCode,
    COUNT(DISTINCT CASE WHEN ep.PermissionId IS NOT NULL THEN e.WorkmanSL END) AS AdminsGranted,
    COUNT(DISTINCT e.WorkmanSL) AS ActiveAdmins
FROM dbo.tbl_Employee_Mustertable e
CROSS JOIN dbo.tlb_permissions p
INNER JOIN @Catalog c ON c.PermissionCode = p.PermissionCode
LEFT JOIN dbo.tlb_employee_permissions ep
    ON ep.WorkmanSL = e.WorkmanSL
   AND ep.PermissionId = p.Id
WHERE e.WorkStatus = N'Active'
  AND e.User_RoleType = N'Admin'
GROUP BY p.PermissionCode
ORDER BY p.PermissionCode;

/* 3 — Missing grants (must be 0 after apply) */
SELECT
    e.WorkmanSL,
    e.LoginID,
    p.PermissionCode AS MissingPermission
FROM dbo.tbl_Employee_Mustertable e
CROSS JOIN dbo.tlb_permissions p
INNER JOIN @Catalog c ON c.PermissionCode = p.PermissionCode
WHERE e.WorkStatus = N'Active'
  AND e.User_RoleType = N'Admin'
  AND p.IsActive = 1
  AND NOT EXISTS (
        SELECT 1
        FROM dbo.tlb_employee_permissions ep
        WHERE ep.WorkmanSL = e.WorkmanSL
          AND ep.PermissionId = p.Id
  )
ORDER BY e.WorkmanSL, p.PermissionCode;

SELECT COUNT(*) AS MissingGrantCount
FROM dbo.tbl_Employee_Mustertable e
CROSS JOIN dbo.tlb_permissions p
INNER JOIN @Catalog c ON c.PermissionCode = p.PermissionCode
WHERE e.WorkStatus = N'Active'
  AND e.User_RoleType = N'Admin'
  AND p.IsActive = 1
  AND NOT EXISTS (
        SELECT 1
        FROM dbo.tlb_employee_permissions ep
        WHERE ep.WorkmanSL = e.WorkmanSL
          AND ep.PermissionId = p.Id
  );

/* 4 — Duplicate grants (PK should make this empty) */
SELECT ep.WorkmanSL, ep.PermissionId, p.PermissionCode, COUNT(*) AS DuplicateCount
FROM dbo.tlb_employee_permissions ep
INNER JOIN dbo.tlb_permissions p ON p.Id = ep.PermissionId
INNER JOIN @Catalog c ON c.PermissionCode = p.PermissionCode
GROUP BY ep.WorkmanSL, ep.PermissionId, p.PermissionCode
HAVING COUNT(*) > 1;

SELECT COUNT(*) AS DuplicateGrantGroupCount
FROM (
    SELECT ep.WorkmanSL, ep.PermissionId
    FROM dbo.tlb_employee_permissions ep
    INNER JOIN dbo.tlb_permissions p ON p.Id = ep.PermissionId
    INNER JOIN @Catalog c ON c.PermissionCode = p.PermissionCode
    GROUP BY ep.WorkmanSL, ep.PermissionId
    HAVING COUNT(*) > 1
) d;

/* 5 — Orphan overlay rows for pack codes */
SELECT
    ep.WorkmanSL,
    p.PermissionCode,
    CASE
        WHEN e.WorkmanSL IS NULL THEN N'orphan WorkmanSL not in muster'
        WHEN e.WorkStatus <> N'Active' THEN N'orphan employee not Active'
        WHEN e.User_RoleType <> N'Admin' THEN N'orphan not User_RoleType Admin'
        WHEN p.Id IS NULL THEN N'orphan PermissionId not in catalog'
        ELSE N'ok'
    END AS OrphanReason
FROM dbo.tlb_employee_permissions ep
LEFT JOIN dbo.tlb_permissions p ON p.Id = ep.PermissionId
LEFT JOIN dbo.tbl_Employee_Mustertable e ON e.WorkmanSL = ep.WorkmanSL
WHERE p.PermissionCode IN (SELECT PermissionCode FROM @Catalog)
   OR p.Id IS NULL
ORDER BY OrphanReason, ep.WorkmanSL, p.PermissionCode;

SELECT SUM(CASE WHEN reason.OrphanReason <> N'ok' THEN 1 ELSE 0 END) AS OrphanGrantCount
FROM (
    SELECT
        CASE
            WHEN e.WorkmanSL IS NULL THEN N'orphan WorkmanSL not in muster'
            WHEN e.WorkStatus <> N'Active' THEN N'orphan employee not Active'
            WHEN e.User_RoleType <> N'Admin' THEN N'orphan not User_RoleType Admin'
            WHEN p.Id IS NULL THEN N'orphan PermissionId not in catalog'
            ELSE N'ok'
        END AS OrphanReason
    FROM dbo.tlb_employee_permissions ep
    LEFT JOIN dbo.tlb_permissions p ON p.Id = ep.PermissionId
    LEFT JOIN dbo.tbl_Employee_Mustertable e ON e.WorkmanSL = ep.WorkmanSL
    WHERE p.PermissionCode IN (SELECT PermissionCode FROM @Catalog)
       OR p.Id IS NULL
) reason;
