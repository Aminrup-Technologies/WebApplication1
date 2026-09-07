-- When: 2026-09-07
-- Why: Provision platform Admin compatible with Security Foundation (#89–#102).
-- What: Idempotent discovery + optional role/menu/employee/overlay provisioning.
--
-- This script does NOT invent EmpType_Value or Emp_PermissionValue.
-- It reads live UAT rows, then (only if @ApplyChanges = 1) reuses those values.
--
-- Default is discovery-only (@ApplyChanges = 0). Review result sets 1–8 first.
-- No DROP. No DELETE. Password, LastLogin, LoginStatus are not updated.
--
-- Name collision: tlb_EmployeePermissions = sidebar matrix.
--                 tlb_employee_permissions = overlay grants (PR #96).

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @WorkmanSL NVARCHAR(50) = N'J8';
DECLARE @ApplyChanges BIT = 0;              -- 0 = observe only; 1 = apply
DECLARE @ApplyOverlayGrants BIT = 1;        -- USER_ADMIN + SWITCH_USER if overlay tables exist
DECLARE @CreateMissingAdminRole BIT = 0;    -- do not invent a role unless you set @NewAdminEmpTypeValue
DECLARE @NewAdminEmpTypeValue NVARCHAR(20) = NULL; -- required only if creating a missing Admin catalog row
DECLARE @AddedByWrk NVARCHAR(50) = N'UAT_PROVISION';

/* =====================================================================
   Result 1 — Security table inventory (INFORMATION_SCHEMA)
   ===================================================================== */
SELECT
    t.TABLE_SCHEMA,
    t.TABLE_NAME,
    CASE t.TABLE_NAME
        WHEN N'tbl_Employee_Mustertable' THEN N'Employee master; login identity; User_RoleType / UserRoleDB / RolePermissionDB'
        WHEN N'tlb_emp_roles' THEN N'Role catalog; Employee_Type → USERTYPE; EmpType_Value → UserRoleDB'
        WHEN N'tlb_emp_roles_permission' THEN N'Menu-profile catalog; Emp_PermissionValue → RolePermissionDB'
        WHEN N'tlb_EmployeePermissions' THEN N'Sidebar visibility matrix keyed by Emp_PermissionValue'
        WHEN N'tlb_permissions' THEN N'Overlay permission catalog (PR #96)'
        WHEN N'tlb_permission_groups' THEN N'Overlay groups'
        WHEN N'tlb_group_permissions' THEN N'Overlay group ↔ permission'
        WHEN N'tlb_employee_group' THEN N'Overlay employee ↔ group'
        WHEN N'tlb_employee_permissions' THEN N'Overlay direct grants (not the sidebar matrix)'
        ELSE N'Matched search name'
    END AS Purpose,
    CASE WHEN t.TABLE_NAME IS NOT NULL THEN N'EXISTS' ELSE N'MISSING' END AS Presence
FROM (VALUES
    (N'tbl_Employee_Mustertable'),
    (N'tlb_emp_roles'),
    (N'tlb_emp_roles_permission'),
    (N'tlb_EmployeePermissions'),
    (N'tlb_permissions'),
    (N'tlb_permission_groups'),
    (N'tlb_group_permissions'),
    (N'tlb_employee_group'),
    (N'tlb_employee_permissions')
) AS wanted(TABLE_NAME)
LEFT JOIN INFORMATION_SCHEMA.TABLES t
    ON t.TABLE_NAME = wanted.TABLE_NAME
   AND t.TABLE_SCHEMA = N'dbo'
   AND t.TABLE_TYPE = N'BASE TABLE'
ORDER BY wanted.TABLE_NAME;

/* =====================================================================
   Result 2 — Column inventory for discovered tables
   ===================================================================== */
SELECT
    c.TABLE_NAME,
    c.COLUMN_NAME,
    c.DATA_TYPE,
    c.CHARACTER_MAXIMUM_LENGTH,
    c.IS_NULLABLE,
    CASE WHEN pk.COLUMN_NAME IS NOT NULL THEN N'PK' ELSE N'' END AS PK
FROM INFORMATION_SCHEMA.COLUMNS c
LEFT JOIN (
    SELECT ku.TABLE_NAME, ku.COLUMN_NAME
    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
    INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE ku
        ON tc.CONSTRAINT_NAME = ku.CONSTRAINT_NAME
       AND tc.TABLE_NAME = ku.TABLE_NAME
    WHERE tc.CONSTRAINT_TYPE = N'PRIMARY KEY'
) pk ON pk.TABLE_NAME = c.TABLE_NAME AND pk.COLUMN_NAME = c.COLUMN_NAME
WHERE c.TABLE_SCHEMA = N'dbo'
  AND c.TABLE_NAME IN (
        N'tbl_Employee_Mustertable',
        N'tlb_emp_roles',
        N'tlb_emp_roles_permission',
        N'tlb_EmployeePermissions',
        N'tlb_permissions',
        N'tlb_permission_groups',
        N'tlb_group_permissions',
        N'tlb_employee_group',
        N'tlb_employee_permissions'
  )
ORDER BY c.TABLE_NAME, c.ORDINAL_POSITION;

/* =====================================================================
   Result 3 — Existing Active Admins (do not guess values)
   ===================================================================== */
IF OBJECT_ID(N'dbo.tbl_Employee_Mustertable', N'U') IS NOT NULL
BEGIN
    SELECT
        WorkmanSL,
        LoginID,
        FullName,
        User_RoleType,
        UserRoleDB,
        RolePermissionDB,
        WorkStatus
    FROM dbo.tbl_Employee_Mustertable
    WHERE WorkStatus = N'Active'
      AND User_RoleType = N'Admin'
    ORDER BY WorkmanSL;
END

/* =====================================================================
   Result 4 — Admin vs Office Staff field comparison (Active)
   ===================================================================== */
IF OBJECT_ID(N'dbo.tbl_Employee_Mustertable', N'U') IS NOT NULL
BEGIN
    SELECT
        User_RoleType,
        UserRoleDB,
        RolePermissionDB,
        COUNT(*) AS EmployeeCount
    FROM dbo.tbl_Employee_Mustertable
    WHERE WorkStatus = N'Active'
      AND User_RoleType IN (N'Admin', N'Office Staff')
    GROUP BY User_RoleType, UserRoleDB, RolePermissionDB
    ORDER BY User_RoleType, EmployeeCount DESC;
END

/* =====================================================================
   Result 5 — Menu profile by visible-row count (full-access candidate)
   ===================================================================== */
IF OBJECT_ID(N'dbo.tlb_EmployeePermissions', N'U') IS NOT NULL
BEGIN
    SELECT
        Emp_PermissionValue AS RolePermissionDB,
        COUNT(*) AS MenuRowCount,
        SUM(CASE WHEN CONVERT(INT, IsVisible) <> 0 THEN 1 ELSE 0 END) AS VisibleMenuCount
    FROM dbo.tlb_EmployeePermissions
    GROUP BY Emp_PermissionValue
    ORDER BY VisibleMenuCount DESC, MenuRowCount DESC;
END

/* =====================================================================
   Result 6 — Role catalog (Admin / Office Staff / Employee labels)
   ===================================================================== */
IF OBJECT_ID(N'dbo.tlb_emp_roles', N'U') IS NOT NULL
BEGIN
    SELECT
        EmpType_Value,
        Employee_Type,
        CASE
            WHEN Employee_Type = N'Admin' THEN N'platform Admin (USERTYPE gate)'
            WHEN Employee_Type = N'Office Staff' THEN N'module-local (JOB360/attendance); not Switch User'
            WHEN Employee_Type = N'Site Staff' THEN N'JOB create routing; not overlay Admin'
            ELSE N'label as stored; do not guess privilege'
        END AS SecurityFoundationNote
    FROM dbo.tlb_emp_roles
    WHERE ISNULL(DeleteMode, 0) = 0
    ORDER BY Id;
END

/* =====================================================================
   Result 7 — Overlay catalog readiness (no inserts here)
   ===================================================================== */
IF OBJECT_ID(N'dbo.tlb_permissions', N'U') IS NOT NULL
BEGIN
    SELECT
        wanted.PermissionCode,
        CASE WHEN p.PermissionCode IS NULL THEN N'NO' ELSE N'YES' END AS ExistsInCatalog,
        p.IsActive
    FROM (VALUES (N'SWITCH_USER'), (N'USER_ADMIN')) AS wanted(PermissionCode)
    LEFT JOIN dbo.tlb_permissions p
        ON p.PermissionCode = wanted.PermissionCode;
END
ELSE
BEGIN
    SELECT N'(overlay catalog tlb_permissions is missing)' AS OverlayReadiness;
END

/* =====================================================================
   Result 8 — Target employee BEFORE
   ===================================================================== */
IF OBJECT_ID(N'dbo.tbl_Employee_Mustertable', N'U') IS NOT NULL
BEGIN
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
        LastLogin
    FROM dbo.tbl_Employee_Mustertable
    WHERE WorkmanSL = @WorkmanSL;
END

/* =====================================================================
   Derive Admin UserRoleDB + full-access RolePermissionDB from live data
   ===================================================================== */
DECLARE @AdminEmpTypeValue NVARCHAR(50);
DECLARE @AdminEmployeeType NVARCHAR(100) = N'Admin';
DECLARE @RolePermissionDB NVARCHAR(50);
DECLARE @RolePermissionText NVARCHAR(200);
DECLARE @MenuVisibleCount INT;

IF OBJECT_ID(N'dbo.tlb_emp_roles', N'U') IS NOT NULL
BEGIN
    SELECT TOP (1)
        @AdminEmpTypeValue = CONVERT(NVARCHAR(50), EmpType_Value),
        @AdminEmployeeType = Employee_Type
    FROM dbo.tlb_emp_roles
    WHERE Employee_Type = N'Admin'
      AND ISNULL(DeleteMode, 0) = 0
    ORDER BY Id;
END

-- Prefer the RolePermissionDB already used by the largest number of Active Admins.
IF OBJECT_ID(N'dbo.tbl_Employee_Mustertable', N'U') IS NOT NULL
   AND @AdminEmpTypeValue IS NOT NULL
BEGIN
    SELECT TOP (1)
        @RolePermissionDB = CONVERT(NVARCHAR(50), RolePermissionDB)
    FROM dbo.tbl_Employee_Mustertable
    WHERE WorkStatus = N'Active'
      AND User_RoleType = N'Admin'
      AND ISNULL(CONVERT(NVARCHAR(50), RolePermissionDB), N'') <> N''
    GROUP BY RolePermissionDB
    ORDER BY COUNT(*) DESC, RolePermissionDB;
END

-- Fallback: Admin catalog profile with the most visible sidebar rows.
IF @RolePermissionDB IS NULL
   AND OBJECT_ID(N'dbo.tlb_emp_roles_permission', N'U') IS NOT NULL
   AND OBJECT_ID(N'dbo.tlb_EmployeePermissions', N'U') IS NOT NULL
   AND @AdminEmpTypeValue IS NOT NULL
BEGIN
    SELECT TOP (1)
        @RolePermissionDB = CONVERT(NVARCHAR(50), p.Emp_PermissionValue)
    FROM dbo.tlb_emp_roles_permission p
    LEFT JOIN dbo.tlb_EmployeePermissions m
        ON CONVERT(NVARCHAR(50), m.Emp_PermissionValue) = CONVERT(NVARCHAR(50), p.Emp_PermissionValue)
    WHERE CONVERT(NVARCHAR(50), p.EmpType_Value) = @AdminEmpTypeValue
      AND ISNULL(p.delete_status, 0) = 0
    GROUP BY p.Emp_PermissionValue
    ORDER BY SUM(CASE WHEN m.IsVisible IS NULL THEN 0 WHEN CONVERT(INT, m.IsVisible) <> 0 THEN 1 ELSE 0 END) DESC;
END

IF @RolePermissionDB IS NOT NULL
   AND OBJECT_ID(N'dbo.tlb_emp_roles_permission', N'U') IS NOT NULL
BEGIN
    SELECT TOP (1)
        @RolePermissionText = Emp_PermissionText
    FROM dbo.tlb_emp_roles_permission
    WHERE CONVERT(NVARCHAR(50), Emp_PermissionValue) = @RolePermissionDB
    ORDER BY Id;
END

IF @RolePermissionDB IS NOT NULL
   AND OBJECT_ID(N'dbo.tlb_EmployeePermissions', N'U') IS NOT NULL
BEGIN
    SELECT @MenuVisibleCount = SUM(CASE WHEN CONVERT(INT, IsVisible) <> 0 THEN 1 ELSE 0 END)
    FROM dbo.tlb_EmployeePermissions
    WHERE CONVERT(NVARCHAR(50), Emp_PermissionValue) = @RolePermissionDB;
END

SELECT
    N'DERIVED' AS Phase,
    @WorkmanSL AS TargetWorkmanSL,
    @AdminEmployeeType AS AdminUser_RoleType,
    @AdminEmpTypeValue AS AdminUserRoleDB,
    @RolePermissionDB AS FullAccessRolePermissionDB,
    @RolePermissionText AS Role_PermissionText,
    @MenuVisibleCount AS VisibleMenuCount,
    CASE WHEN @ApplyChanges = 1 THEN N'APPLY' ELSE N'DRY-RUN' END AS Mode;

IF @ApplyChanges = 0
BEGIN
    SELECT N'Dry run. Set @ApplyChanges = 1 after reviewing results 1–8 and DERIVED.' AS NextStep;
    RETURN;
END

IF OBJECT_ID(N'dbo.tbl_Employee_Mustertable', N'U') IS NULL
BEGIN
    RAISERROR(N'tbl_Employee_Mustertable is missing.', 16, 1);
    RETURN;
END

IF NOT EXISTS (SELECT 1 FROM dbo.tbl_Employee_Mustertable WHERE WorkmanSL = @WorkmanSL)
BEGIN
    RAISERROR(N'WorkmanSL not found in tbl_Employee_Mustertable. Refusing to insert a new employee.', 16, 1);
    RETURN;
END

IF @AdminEmpTypeValue IS NULL
BEGIN
    IF @CreateMissingAdminRole = 1 AND ISNULL(@NewAdminEmpTypeValue, N'') <> N''
    BEGIN
        INSERT INTO dbo.tlb_emp_roles (Employee_Type, EmpType_Value, ViewMode, DeleteMode, AddedByWrk, TimeStamp)
        SELECT N'Admin', @NewAdminEmpTypeValue, 1, 0, @AddedByWrk, CONVERT(VARCHAR(23), GETDATE(), 121)
        WHERE NOT EXISTS (
            SELECT 1 FROM dbo.tlb_emp_roles WHERE Employee_Type = N'Admin' AND ISNULL(DeleteMode, 0) = 0
        );
        SET @AdminEmpTypeValue = @NewAdminEmpTypeValue;
    END
    ELSE
    BEGIN
        RAISERROR(N'No tlb_emp_roles row with Employee_Type = Admin. Will not invent EmpType_Value. Set @CreateMissingAdminRole=1 and @NewAdminEmpTypeValue from Result 6.', 16, 1);
        RETURN;
    END
END

IF ISNULL(@RolePermissionDB, N'') = N''
BEGIN
    RAISERROR(N'Could not discover a full-access RolePermissionDB from existing Admins or Admin menu profiles. Refusing to invent a menu matrix.', 16, 1);
    RETURN;
END

BEGIN TRANSACTION;

UPDATE dbo.tbl_Employee_Mustertable
SET
    User_RoleType = @AdminEmployeeType,
    UserRoleDB = @AdminEmpTypeValue,
    RolePermissionDB = @RolePermissionDB,
    Role_Permission = COALESCE(@RolePermissionText, Role_Permission)
WHERE WorkmanSL = @WorkmanSL
  AND (
        ISNULL(User_RoleType, N'') <> @AdminEmployeeType
     OR ISNULL(CONVERT(NVARCHAR(50), UserRoleDB), N'') <> @AdminEmpTypeValue
     OR ISNULL(CONVERT(NVARCHAR(50), RolePermissionDB), N'') <> @RolePermissionDB
     OR (
            @RolePermissionText IS NOT NULL
        AND ISNULL(Role_Permission, N'') <> @RolePermissionText
     )
  );

IF @ApplyOverlayGrants = 1
   AND OBJECT_ID(N'dbo.tlb_employee_permissions', N'U') IS NOT NULL
   AND OBJECT_ID(N'dbo.tlb_permissions', N'U') IS NOT NULL
BEGIN
    INSERT INTO dbo.tlb_employee_permissions (WorkmanSL, PermissionId)
    SELECT @WorkmanSL, p.Id
    FROM dbo.tlb_permissions p
    WHERE p.PermissionCode IN (N'SWITCH_USER', N'USER_ADMIN')
      AND p.IsActive = 1
      AND NOT EXISTS (
            SELECT 1
            FROM dbo.tlb_employee_permissions ep
            WHERE ep.WorkmanSL = @WorkmanSL
              AND ep.PermissionId = p.Id
        );
END

COMMIT TRANSACTION;

/* =====================================================================
   AFTER verification
   ===================================================================== */
SELECT
    N'AFTER' AS Phase,
    e.WorkmanSL,
    e.LoginID,
    e.FullName,
    e.User_RoleType,
    e.UserRoleDB,
    e.RolePermissionDB,
    e.Role_Permission,
    e.WorkStatus,
    e.LoginStatus,
    e.LastLogin,
    CASE WHEN e.User_RoleType = N'Admin' THEN N'PASS' ELSE N'FAIL' END AS UserTypeCheck,
    CASE WHEN CONVERT(NVARCHAR(50), e.UserRoleDB) = @AdminEmpTypeValue THEN N'PASS' ELSE N'FAIL' END AS UserRoleDBCheck,
    CASE WHEN CONVERT(NVARCHAR(50), e.RolePermissionDB) = @RolePermissionDB THEN N'PASS' ELSE N'FAIL' END AS RolePermissionDBCheck
FROM dbo.tbl_Employee_Mustertable e
WHERE e.WorkmanSL = @WorkmanSL;

SELECT
    N'AFTER-MENU' AS Phase,
    COUNT(*) AS MenuRowCount,
    SUM(CASE WHEN CONVERT(INT, IsVisible) <> 0 THEN 1 ELSE 0 END) AS VisibleMenuCount
FROM dbo.tlb_EmployeePermissions
WHERE CONVERT(NVARCHAR(50), Emp_PermissionValue) = @RolePermissionDB;

IF OBJECT_ID(N'dbo.tlb_employee_permissions', N'U') IS NOT NULL
BEGIN
    SELECT
        N'AFTER-OVERLAY' AS Phase,
        p.PermissionCode,
        CASE WHEN ep.PermissionId IS NULL THEN N'ABSENT' ELSE N'GRANTED' END AS GrantState
    FROM dbo.tlb_permissions p
    LEFT JOIN dbo.tlb_employee_permissions ep
        ON ep.PermissionId = p.Id
       AND ep.WorkmanSL = @WorkmanSL
    WHERE p.PermissionCode IN (N'SWITCH_USER', N'USER_ADMIN');
END
ELSE
BEGIN
    SELECT N'AFTER-OVERLAY' AS Phase, N'overlay tables not present; skipped' AS GrantState;
END
