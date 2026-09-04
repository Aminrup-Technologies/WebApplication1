/*
    Per-user MFA (Email OTP) columns for tbl_Employee_Mustertable.

    Run this on each ATS ERP database BEFORE enabling MFA for any user
    in Employee Master. Existing users stay password-only (MFAEnabled = 0).

    Safe to re-run: skips columns that already exist.
*/

IF OBJECT_ID(N'dbo.tbl_Employee_Mustertable', N'U') IS NULL
BEGIN
    RAISERROR('tbl_Employee_Mustertable was not found.', 16, 1);
    RETURN;
END
GO

IF COL_LENGTH('dbo.tbl_Employee_Mustertable', 'MFAEnabled') IS NULL
BEGIN
    ALTER TABLE dbo.tbl_Employee_Mustertable
        ADD MFAEnabled BIT NOT NULL CONSTRAINT DF_EmpMuster_MFAEnabled DEFAULT (0);
END
GO

IF COL_LENGTH('dbo.tbl_Employee_Mustertable', 'MFAMethod') IS NULL
BEGIN
    ALTER TABLE dbo.tbl_Employee_Mustertable
        ADD MFAMethod VARCHAR(20) NOT NULL CONSTRAINT DF_EmpMuster_MFAMethod DEFAULT ('EmailOTP');
END
GO

IF COL_LENGTH('dbo.tbl_Employee_Mustertable', 'MFAEnforcedOn') IS NULL
BEGIN
    ALTER TABLE dbo.tbl_Employee_Mustertable
        ADD MFAEnforcedOn DATETIME NULL;
END
GO

IF COL_LENGTH('dbo.tbl_Employee_Mustertable', 'MFAEnforcedBy') IS NULL
BEGIN
    ALTER TABLE dbo.tbl_Employee_Mustertable
        ADD MFAEnforcedBy VARCHAR(50) NULL;
END
GO

IF COL_LENGTH('dbo.tbl_Employee_Mustertable', 'MFALastVerified') IS NULL
BEGIN
    ALTER TABLE dbo.tbl_Employee_Mustertable
        ADD MFALastVerified DATETIME NULL;
END
GO
