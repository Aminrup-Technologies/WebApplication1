/*
    Permission overlay tables (authorization modernization PR B).

    Additive only: does not alter tbl_Employee_Mustertable, tlb_emp_roles,
    tlb_emp_roles_permission, or tlb_EmployeePermissions (legacy menu matrix).

    tlb_employee_permissions (this script) is the overlay direct-grant table.
    tlb_EmployeePermissions (existing) remains the sidebar visibility matrix.

    Safe to re-run: creates missing objects and inserts missing seed rows.
    Does not assign permissions to any employee.
*/

IF OBJECT_ID(N'dbo.tlb_permissions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tlb_permissions
    (
        Id              INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        PermissionCode  NVARCHAR(64)  NOT NULL,
        Name            NVARCHAR(128) NOT NULL,
        Module          NVARCHAR(64)  NOT NULL,
        Description     NVARCHAR(400) NULL,
        IsActive        BIT           NOT NULL CONSTRAINT DF_tlb_permissions_IsActive DEFAULT (1),
        CONSTRAINT UQ_tlb_permissions_Code UNIQUE (PermissionCode)
    );
END
GO

IF OBJECT_ID(N'dbo.tlb_permission_groups', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tlb_permission_groups
    (
        Id          INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        GroupCode   NVARCHAR(64)  NOT NULL,
        Name        NVARCHAR(128) NOT NULL,
        Description NVARCHAR(400) NULL,
        IsActive    BIT           NOT NULL CONSTRAINT DF_tlb_permission_groups_IsActive DEFAULT (1),
        CONSTRAINT UQ_tlb_permission_groups_Code UNIQUE (GroupCode)
    );
END
GO

IF OBJECT_ID(N'dbo.tlb_group_permissions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tlb_group_permissions
    (
        GroupId      INT NOT NULL,
        PermissionId INT NOT NULL,
        CONSTRAINT PK_tlb_group_permissions PRIMARY KEY (GroupId, PermissionId),
        CONSTRAINT FK_tlb_group_permissions_Group
            FOREIGN KEY (GroupId) REFERENCES dbo.tlb_permission_groups (Id),
        CONSTRAINT FK_tlb_group_permissions_Permission
            FOREIGN KEY (PermissionId) REFERENCES dbo.tlb_permissions (Id)
    );
END
GO

IF OBJECT_ID(N'dbo.tlb_employee_group', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tlb_employee_group
    (
        WorkmanSL NVARCHAR(50) NOT NULL,
        GroupId   INT          NOT NULL,
        CONSTRAINT PK_tlb_employee_group PRIMARY KEY (WorkmanSL, GroupId),
        CONSTRAINT FK_tlb_employee_group_Group
            FOREIGN KEY (GroupId) REFERENCES dbo.tlb_permission_groups (Id)
    );
    CREATE INDEX IX_tlb_employee_group_Workman ON dbo.tlb_employee_group (WorkmanSL);
END
GO

IF OBJECT_ID(N'dbo.tlb_employee_permissions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tlb_employee_permissions
    (
        WorkmanSL    NVARCHAR(50) NOT NULL,
        PermissionId INT          NOT NULL,
        CONSTRAINT PK_tlb_employee_permissions PRIMARY KEY (WorkmanSL, PermissionId),
        CONSTRAINT FK_tlb_employee_permissions_Permission
            FOREIGN KEY (PermissionId) REFERENCES dbo.tlb_permissions (Id)
    );
    CREATE INDEX IX_tlb_employee_permissions_Workman ON dbo.tlb_employee_permissions (WorkmanSL);
END
GO

IF OBJECT_ID(N'dbo.tlb_permissions', N'U') IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM dbo.tlb_permissions WHERE PermissionCode = N'SWITCH_USER')
        INSERT INTO dbo.tlb_permissions (PermissionCode, Name, Module, Description, IsActive)
        VALUES (N'SWITCH_USER', N'Switch User', N'Admin', N'Impersonate another employee session.', 1);

    IF NOT EXISTS (SELECT 1 FROM dbo.tlb_permissions WHERE PermissionCode = N'PAYROLL_OVERRIDE')
        INSERT INTO dbo.tlb_permissions (PermissionCode, Name, Module, Description, IsActive)
        VALUES (N'PAYROLL_OVERRIDE', N'Payroll Override', N'Payroll', N'View locked payroll fields on employee master.', 1);

    IF NOT EXISTS (SELECT 1 FROM dbo.tlb_permissions WHERE PermissionCode = N'JOB360_OVERRIDE')
        INSERT INTO dbo.tlb_permissions (PermissionCode, Name, Module, Description, IsActive)
        VALUES (N'JOB360_OVERRIDE', N'JOB360 Override', N'JOB360', N'JOB360 admin chrome (legacy: Admin or Office Staff).', 1);

    IF NOT EXISTS (SELECT 1 FROM dbo.tlb_permissions WHERE PermissionCode = N'ATTENDANCE_OVERRIDE')
        INSERT INTO dbo.tlb_permissions (PermissionCode, Name, Module, Description, IsActive)
        VALUES (N'ATTENDANCE_OVERRIDE', N'Attendance Override', N'Attendance', N'Job exceptions and attendance anomaly dashboards.', 1);

    IF NOT EXISTS (SELECT 1 FROM dbo.tlb_permissions WHERE PermissionCode = N'EXPORT_PAYROLL')
        INSERT INTO dbo.tlb_permissions (PermissionCode, Name, Module, Description, IsActive)
        VALUES (N'EXPORT_PAYROLL', N'Export Payroll', N'Payroll', N'Payroll dashboard extra chrome (legacy: Workman J8).', 1);

    IF NOT EXISTS (SELECT 1 FROM dbo.tlb_permissions WHERE PermissionCode = N'USER_ADMIN')
        INSERT INTO dbo.tlb_permissions (PermissionCode, Name, Module, Description, IsActive)
        VALUES (N'USER_ADMIN', N'User Admin', N'Admin', N'Security administration (no legacy page gate; overlay only).', 1);
END
GO
