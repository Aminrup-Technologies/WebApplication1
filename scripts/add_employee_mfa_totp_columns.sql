/*
    TOTP (authenticator app) columns for tbl_Employee_Mustertable.

    Run AFTER scripts/add_employee_mfa_columns.sql (Email OTP MFA).
    Safe to re-run: skips columns that already exist.

    MFAMethod values:
      EmailOTP      - 6-digit code emailed after password (phase 1)
      Authenticator - TOTP app (Google / Microsoft Authenticator)
*/

IF OBJECT_ID(N'dbo.tbl_Employee_Mustertable', N'U') IS NULL
BEGIN
    RAISERROR('tbl_Employee_Mustertable was not found.', 16, 1);
    RETURN;
END
GO

IF COL_LENGTH('dbo.tbl_Employee_Mustertable', 'MFATotpSecret') IS NULL
BEGIN
    ALTER TABLE dbo.tbl_Employee_Mustertable
        ADD MFATotpSecret VARCHAR(64) NULL;
END
GO

IF COL_LENGTH('dbo.tbl_Employee_Mustertable', 'MFATotpEnrolled') IS NULL
BEGIN
    ALTER TABLE dbo.tbl_Employee_Mustertable
        ADD MFATotpEnrolled BIT NOT NULL CONSTRAINT DF_EmpMuster_MFATotpEnrolled DEFAULT (0);
END
GO
