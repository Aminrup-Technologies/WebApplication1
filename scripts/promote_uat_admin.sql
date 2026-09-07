-- When: 2026-09-07
-- Why: Provision platform Admin compatible with Security Foundation.
-- What: Idempotent UAT-only promotion of one Active operator (default J8).
--
-- UAT facts (verified, do not invent):
--   No tlb_emp_roles row with Employee_Type = 'Admin'.
--   Operational UserRoleDB = ATS-OS (preserve).
--   Full-access RolePermissionDB = OS-HR (preserve; 31 visible menus).
--   AuthorizationService.IsAdmin() reads Session USERTYPE from User_RoleType only.
--
-- SET list is exactly two columns:
--   User_RoleType = 'Admin'
--   Role_Permission = RolePermissionDB   (align display twin to OS-HR)
--
-- Not updated: LoginPassword, LoginStatus, LastLogin, LastLogout,
-- PasswordExpiry, MFA*, UserRoleDB, RolePermissionDB.
-- No DROP. No DELETE. No INSERT. No catalog / overlay / menu writes.
-- Default is dry-run. Set @ApplyChanges = 1 only after reviewing BEFORE + PLANNED.

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @WorkmanSL NVARCHAR(50) = N'J8';
DECLARE @ApplyChanges BIT = 0; -- 0 = dry-run; 1 = apply

DECLARE @ExpectedUserType NVARCHAR(50) = N'Admin';
DECLARE @ExpectedUserRoleDB NVARCHAR(50) = N'ATS-OS';
DECLARE @ExpectedRolePermissionDB NVARCHAR(50) = N'OS-HR';
DECLARE @ExpectedVisibleMenus INT = 31;

/* =====================================================================
   Result 1 — Table verification (must exist; no guessed names)
   ===================================================================== */
SELECT
    wanted.TABLE_NAME AS [Table],
    CASE WHEN t.TABLE_NAME IS NOT NULL THEN N'EXISTS' ELSE N'MISSING' END AS ExistsFlag
INTO #TableCheck
FROM (VALUES
    (N'tbl_Employee_Mustertable'),
    (N'tlb_emp_roles'),
    (N'tlb_emp_roles_permission'),
    (N'tlb_EmployeePermissions')
) AS wanted(TABLE_NAME)
LEFT JOIN INFORMATION_SCHEMA.TABLES t
    ON t.TABLE_NAME = wanted.TABLE_NAME
   AND t.TABLE_SCHEMA = N'dbo'
   AND t.TABLE_TYPE = N'BASE TABLE';

SELECT [Table], ExistsFlag FROM #TableCheck ORDER BY [Table];

IF EXISTS (SELECT 1 FROM #TableCheck WHERE ExistsFlag = N'MISSING')
BEGIN
    RAISERROR(N'Required security table is missing. Refusing to continue.', 16, 1);
    DROP TABLE #TableCheck;
    RETURN;
END

/* =====================================================================
   Result 2 — Column verification (must exist)
   ===================================================================== */
SELECT
    wanted.COLUMN_NAME AS [Column],
    CASE WHEN c.COLUMN_NAME IS NOT NULL THEN N'EXISTS' ELSE N'MISSING' END AS ExistsFlag,
    c.DATA_TYPE,
    c.CHARACTER_MAXIMUM_LENGTH,
    c.IS_NULLABLE
INTO #ColumnCheck
FROM (VALUES
    (N'WorkmanSL'),
    (N'LoginID'),
    (N'FullName'),
    (N'User_RoleType'),
    (N'UserRoleDB'),
    (N'RolePermissionDB'),
    (N'Role_Permission'),
    (N'WorkStatus'),
    (N'LoginStatus'),
    (N'LastLogin'),
    (N'LastLogout'),
    (N'PasswordExpiry'),
    (N'LoginPassword')
) AS wanted(COLUMN_NAME)
LEFT JOIN INFORMATION_SCHEMA.COLUMNS c
    ON c.TABLE_SCHEMA = N'dbo'
   AND c.TABLE_NAME = N'tbl_Employee_Mustertable'
   AND c.COLUMN_NAME = wanted.COLUMN_NAME;

SELECT [Column], ExistsFlag, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
FROM #ColumnCheck
ORDER BY [Column];

IF EXISTS (SELECT 1 FROM #ColumnCheck WHERE ExistsFlag = N'MISSING')
BEGIN
    RAISERROR(N'Required tbl_Employee_Mustertable column is missing. Refusing to continue.', 16, 1);
    DROP TABLE #TableCheck;
    DROP TABLE #ColumnCheck;
    RETURN;
END

/* =====================================================================
   Result 3 — Catalog facts (read-only). Admin row must stay absent.
   ===================================================================== */
SELECT
    EmpType_Value,
    Employee_Type,
    CASE WHEN Employee_Type = N'Admin' THEN N'UNEXPECTED Admin catalog row' ELSE N'operational catalog' END AS Note
FROM dbo.tlb_emp_roles
WHERE ISNULL(DeleteMode, 0) = 0
  AND (
        CONVERT(NVARCHAR(50), EmpType_Value) = @ExpectedUserRoleDB
     OR Employee_Type = N'Admin'
  )
ORDER BY Id;

IF EXISTS (
    SELECT 1 FROM dbo.tlb_emp_roles
    WHERE Employee_Type = N'Admin' AND ISNULL(DeleteMode, 0) = 0
)
BEGIN
    SELECT N'WARNING' AS CatalogNote,
           N'Employee_Type=Admin exists. This UAT script still will not rewrite UserRoleDB; IsAdmin() does not use the catalog.' AS Detail;
END
ELSE
BEGIN
    SELECT N'PASS' AS CatalogNote,
           N'No Employee_Type=Admin catalog row. Script will not invent one.' AS Detail;
END

/* =====================================================================
   Result 4 — OS-HR visible menu count (read-only)
   ===================================================================== */
SELECT
    Emp_PermissionValue AS RolePermissionDB,
    COUNT(*) AS MenuRowCount,
    SUM(CASE WHEN CONVERT(INT, IsVisible) <> 0 THEN 1 ELSE 0 END) AS VisibleMenuCount,
    CASE
        WHEN SUM(CASE WHEN CONVERT(INT, IsVisible) <> 0 THEN 1 ELSE 0 END) = @ExpectedVisibleMenus THEN N'PASS'
        ELSE N'CHECK'
    END AS MatchesExpected31
FROM dbo.tlb_EmployeePermissions
WHERE CONVERT(NVARCHAR(50), Emp_PermissionValue) = @ExpectedRolePermissionDB
GROUP BY Emp_PermissionValue;

/* =====================================================================
   Result 5 — BEFORE (target employee)
   ===================================================================== */
IF NOT EXISTS (
    SELECT 1 FROM dbo.tbl_Employee_Mustertable WHERE WorkmanSL = @WorkmanSL
)
BEGIN
    RAISERROR(N'WorkmanSL not found. Refusing to insert a new employee.', 16, 1);
    DROP TABLE #TableCheck;
    DROP TABLE #ColumnCheck;
    RETURN;
END

SELECT
    N'BEFORE' AS Phase,
    WorkmanSL,
    LoginID,
    FullName,
    User_RoleType,
    UserRoleDB,
    RolePermissionDB,
    Role_Permission,
    WorkStatus,
    LoginStatus,
    LastLogin,
    LastLogout,
    PasswordExpiry,
    CASE WHEN LoginPassword IS NULL THEN N'NULL' ELSE N'PRESENT' END AS LoginPasswordPresent
FROM dbo.tbl_Employee_Mustertable
WHERE WorkmanSL = @WorkmanSL;

/* =====================================================================
   Result 6 — PLANNED delta (no writes yet)
   ===================================================================== */
SELECT
    N'PLANNED' AS Phase,
    @WorkmanSL AS WorkmanSL,
    @ExpectedUserType AS New_User_RoleType,
    CONVERT(NVARCHAR(50), RolePermissionDB) AS New_Role_Permission,
    UserRoleDB AS Unchanged_UserRoleDB,
    RolePermissionDB AS Unchanged_RolePermissionDB,
    CASE WHEN WorkStatus = N'Active' THEN N'PASS' ELSE N'FAIL not Active' END AS ActiveCheck,
    CASE WHEN CONVERT(NVARCHAR(50), UserRoleDB) = @ExpectedUserRoleDB THEN N'PASS' ELSE N'FAIL UserRoleDB is not ATS-OS' END AS UserRoleDBCheck,
    CASE WHEN CONVERT(NVARCHAR(50), RolePermissionDB) = @ExpectedRolePermissionDB THEN N'PASS' ELSE N'FAIL RolePermissionDB is not OS-HR' END AS RolePermissionDBCheck,
    CASE WHEN @ApplyChanges = 1 THEN N'APPLY' ELSE N'DRY-RUN' END AS Mode
FROM dbo.tbl_Employee_Mustertable
WHERE WorkmanSL = @WorkmanSL;

IF @ApplyChanges = 0
BEGIN
    SELECT N'Dry run complete. Review BEFORE + PLANNED. Set @ApplyChanges = 1 to apply.' AS NextStep;
    DROP TABLE #TableCheck;
    DROP TABLE #ColumnCheck;
    RETURN;
END

/* =====================================================================
   APPLY — Active + ATS-OS + OS-HR required. Two-column UPDATE only.
   ===================================================================== */
IF NOT EXISTS (
    SELECT 1
    FROM dbo.tbl_Employee_Mustertable
    WHERE WorkmanSL = @WorkmanSL
      AND WorkStatus = N'Active'
)
BEGIN
    RAISERROR(N'Target is missing or not Active. Refusing to update.', 16, 1);
    DROP TABLE #TableCheck;
    DROP TABLE #ColumnCheck;
    RETURN;
END

IF NOT EXISTS (
    SELECT 1
    FROM dbo.tbl_Employee_Mustertable
    WHERE WorkmanSL = @WorkmanSL
      AND CONVERT(NVARCHAR(50), UserRoleDB) = @ExpectedUserRoleDB
      AND CONVERT(NVARCHAR(50), RolePermissionDB) = @ExpectedRolePermissionDB
)
BEGIN
    RAISERROR(N'Target UserRoleDB/RolePermissionDB is not ATS-OS/OS-HR. Refusing to invent replacements.', 16, 1);
    DROP TABLE #TableCheck;
    DROP TABLE #ColumnCheck;
    RETURN;
END

BEGIN TRANSACTION;

UPDATE dbo.tbl_Employee_Mustertable
SET
    User_RoleType = @ExpectedUserType,
    Role_Permission = CONVERT(NVARCHAR(50), RolePermissionDB)
WHERE WorkmanSL = @WorkmanSL
  AND WorkStatus = N'Active'
  AND CONVERT(NVARCHAR(50), UserRoleDB) = @ExpectedUserRoleDB
  AND CONVERT(NVARCHAR(50), RolePermissionDB) = @ExpectedRolePermissionDB
  AND (
        ISNULL(User_RoleType, N'') <> @ExpectedUserType
     OR ISNULL(CONVERT(NVARCHAR(50), Role_Permission), N'') <> CONVERT(NVARCHAR(50), RolePermissionDB)
  );

COMMIT TRANSACTION;

/* =====================================================================
   Result 7 — AFTER
   ===================================================================== */
SELECT
    N'AFTER' AS Phase,
    WorkmanSL,
    LoginID,
    FullName,
    User_RoleType,
    UserRoleDB,
    RolePermissionDB,
    Role_Permission,
    WorkStatus,
    LoginStatus,
    LastLogin,
    LastLogout,
    PasswordExpiry,
    CASE WHEN LoginPassword IS NULL THEN N'NULL' ELSE N'PRESENT' END AS LoginPasswordPresent
FROM dbo.tbl_Employee_Mustertable
WHERE WorkmanSL = @WorkmanSL;

/* =====================================================================
   Result 8 — VALIDATION
   ===================================================================== */
SELECT
    N'VALIDATION' AS Phase,
    CASE WHEN User_RoleType = @ExpectedUserType THEN N'PASS' ELSE N'FAIL' END AS User_RoleType_Admin,
    CASE WHEN CONVERT(NVARCHAR(50), UserRoleDB) = @ExpectedUserRoleDB THEN N'PASS' ELSE N'FAIL' END AS UserRoleDB_ATS_OS,
    CASE WHEN CONVERT(NVARCHAR(50), RolePermissionDB) = @ExpectedRolePermissionDB THEN N'PASS' ELSE N'FAIL' END AS RolePermissionDB_OS_HR,
    CASE WHEN CONVERT(NVARCHAR(50), Role_Permission) = @ExpectedRolePermissionDB THEN N'PASS' ELSE N'FAIL' END AS Role_Permission_OS_HR,
    CASE WHEN WorkStatus = N'Active' THEN N'PASS' ELSE N'FAIL' END AS StillActive
FROM dbo.tbl_Employee_Mustertable
WHERE WorkmanSL = @WorkmanSL;

DROP TABLE #TableCheck;
DROP TABLE #ColumnCheck;

/* =====================================================================
   Separate re-runnable verification (copy below into a new batch).
   =====================================================================
DECLARE @WorkmanSL NVARCHAR(50) = N'J8';

-- BEFORE / AFTER identity
SELECT
    WorkmanSL, LoginID, FullName,
    User_RoleType, UserRoleDB, RolePermissionDB, Role_Permission
FROM dbo.tbl_Employee_Mustertable
WHERE WorkmanSL = @WorkmanSL;

-- Expected after apply
-- User_RoleType     = Admin
-- UserRoleDB        = ATS-OS
-- RolePermissionDB  = OS-HR
-- Role_Permission   = OS-HR
===================================================================== */
