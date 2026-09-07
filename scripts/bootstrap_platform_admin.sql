-- When: 2026-09-07
-- Why: Platform Admin overlay permission pack (PR #104).
-- What: Idempotent catalog ensure + missing tlb_employee_permissions
--       grants for Active employees with User_RoleType = 'Admin'.
--       Final result set is BOOTSTRAP_SUMMARY (PASS/FAIL counts).
--
-- Prerequisite: overlay tables from scripts/create_permission_overlay.sql (PR #96).
-- Does not CREATE tables. Does not touch Login, Session, muster passwords,
-- LoginStatus, LastLogin, UserRoleDB, or RolePermissionDB.
--
-- Default is dry-run (@ApplyChanges = 0). Set @ApplyChanges = 1 to insert.
-- Second apply must insert zero rows (NOT EXISTS).
-- No DROP. No DELETE. No UPDATE of employee rows.

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @ApplyChanges BIT = 0; -- 0 = planned inserts only; 1 = apply

IF OBJECT_ID(N'dbo.tlb_permissions', N'U') IS NULL
   OR OBJECT_ID(N'dbo.tlb_employee_permissions', N'U') IS NULL
   OR OBJECT_ID(N'dbo.tbl_Employee_Mustertable', N'U') IS NULL
BEGIN
    RAISERROR(N'Overlay or employee tables missing. Run scripts/create_permission_overlay.sql first.', 16, 1);
    RETURN;
END

DECLARE @Catalog TABLE
(
    PermissionCode NVARCHAR(64) NOT NULL PRIMARY KEY,
    Name           NVARCHAR(128) NOT NULL,
    Module         NVARCHAR(64) NOT NULL,
    Description    NVARCHAR(400) NULL
);

INSERT INTO @Catalog (PermissionCode, Name, Module, Description)
VALUES
    (N'SWITCH_USER', N'Switch User', N'Admin', N'Impersonate another employee session.'),
    (N'USER_ADMIN', N'User Admin', N'Admin', N'Security administration (no legacy page gate; overlay only).'),
    (N'PAYROLL_OVERRIDE', N'Payroll Override', N'Payroll', N'View locked payroll fields on employee master.'),
    (N'EXPORT_PAYROLL', N'Export Payroll', N'Payroll', N'Payroll dashboard extra chrome (legacy: Workman J8).');

DECLARE @ActiveAdminCount INT;
DECLARE @CatalogCodesExpected INT;
DECLARE @CatalogCodesPresent INT;
DECLARE @CatalogCreated INT;
DECLARE @GrantsExpected INT;
DECLARE @GrantsInserted INT = 0;
DECLARE @GrantsSkippedExisting INT;
DECLARE @DuplicateGrantCount INT;
DECLARE @MissingGrantCount INT;
DECLARE @ExecutionMode NVARCHAR(20);

SELECT @CatalogCodesExpected = COUNT(*) FROM @Catalog;

INSERT INTO dbo.tlb_permissions (PermissionCode, Name, Module, Description, IsActive)
SELECT c.PermissionCode, c.Name, c.Module, c.Description, 1
FROM @Catalog c
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.tlb_permissions p WHERE p.PermissionCode = c.PermissionCode
);
SET @CatalogCreated = @@ROWCOUNT;

SELECT @CatalogCodesPresent = COUNT(*)
FROM @Catalog c
INNER JOIN dbo.tlb_permissions p ON p.PermissionCode = c.PermissionCode;

SELECT @ActiveAdminCount = COUNT(*)
FROM dbo.tbl_Employee_Mustertable
WHERE WorkStatus = N'Active'
  AND User_RoleType = N'Admin';

SET @GrantsExpected = @ActiveAdminCount * @CatalogCodesExpected;

SELECT
    c.PermissionCode,
    CASE WHEN p.Id IS NULL THEN N'MISSING' ELSE N'EXISTS' END AS CatalogState,
    p.Id AS PermissionId,
    p.IsActive
FROM @Catalog c
LEFT JOIN dbo.tlb_permissions p ON p.PermissionCode = c.PermissionCode
ORDER BY c.PermissionCode;

SELECT
    e.WorkmanSL,
    e.LoginID,
    e.FullName,
    e.User_RoleType,
    p.PermissionCode,
    N'PLANNED' AS Action
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

SELECT @MissingGrantCount = COUNT(*)
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

SET @GrantsSkippedExisting = @GrantsExpected - @MissingGrantCount;

SELECT @DuplicateGrantCount = COUNT(*)
FROM (
    SELECT ep.WorkmanSL, ep.PermissionId
    FROM dbo.tlb_employee_permissions ep
    INNER JOIN dbo.tlb_permissions p ON p.Id = ep.PermissionId
    INNER JOIN @Catalog c ON c.PermissionCode = p.PermissionCode
    GROUP BY ep.WorkmanSL, ep.PermissionId
    HAVING COUNT(*) > 1
) d;

SELECT COUNT(*) AS PlannedInsertCount, @MissingGrantCount AS MissingGrantCount;

IF @ApplyChanges = 1
BEGIN
    SET @ExecutionMode = N'APPLY';

    BEGIN TRANSACTION;

    INSERT INTO dbo.tlb_employee_permissions (WorkmanSL, PermissionId)
    SELECT e.WorkmanSL, p.Id
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

    SET @GrantsInserted = @@ROWCOUNT;

    COMMIT TRANSACTION;

    SELECT @MissingGrantCount = COUNT(*)
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

    SELECT @DuplicateGrantCount = COUNT(*)
    FROM (
        SELECT ep.WorkmanSL, ep.PermissionId
        FROM dbo.tlb_employee_permissions ep
        INNER JOIN dbo.tlb_permissions p ON p.Id = ep.PermissionId
        INNER JOIN @Catalog c ON c.PermissionCode = p.PermissionCode
        GROUP BY ep.WorkmanSL, ep.PermissionId
        HAVING COUNT(*) > 1
    ) d;
END
ELSE
BEGIN
    SET @ExecutionMode = N'DRY_RUN';
    SET @GrantsInserted = 0;
END

/* BOOTSTRAP_SUMMARY — last result set */
SELECT
    N'BOOTSTRAP_SUMMARY' AS ResultSet,
    @ActiveAdminCount AS ActiveAdminCount,
    @CatalogCodesExpected AS CatalogCodesExpected,
    @CatalogCodesPresent AS CatalogCodesPresent,
    @CatalogCreated AS CatalogCreated,
    @GrantsExpected AS GrantsExpected,
    @GrantsInserted AS GrantsInserted,
    @GrantsSkippedExisting AS GrantsSkippedExisting,
    @DuplicateGrantCount AS DuplicateGrantCount,
    @MissingGrantCount AS MissingGrantCount,
    @ExecutionMode AS ExecutionMode,
    CASE
        WHEN @CatalogCodesPresent = @CatalogCodesExpected
         AND @DuplicateGrantCount = 0
         AND (
                (@ExecutionMode = N'DRY_RUN' AND @GrantsInserted = 0)
             OR (@ExecutionMode = N'APPLY' AND @MissingGrantCount = 0 AND @GrantsInserted = @GrantsExpected - @GrantsSkippedExisting)
             )
        THEN N'PASS'
        ELSE N'FAIL'
    END AS Verdict;
