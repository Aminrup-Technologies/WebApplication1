/*
    Portal + module Email/WhatsApp trigger switches.

    Run on each ATS ERP database. Safe to re-run: creates the table if needed
    and inserts missing module rows. Existing Enabled flags are not reset.
*/

IF OBJECT_ID(N'dbo.tbl_Notification_Triggers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tbl_Notification_Triggers
    (
        TriggerKey        VARCHAR(40)  NOT NULL,
        DisplayName       VARCHAR(100) NOT NULL,
        Scope             VARCHAR(20)  NOT NULL,
        SortOrder         INT          NOT NULL,
        EmailEnabled      BIT          NOT NULL CONSTRAINT DF_NotifTrig_Email DEFAULT (1),
        WhatsAppEnabled   BIT          NOT NULL CONSTRAINT DF_NotifTrig_WA DEFAULT (1),
        UpdatedBy         VARCHAR(80)  NULL,
        UpdatedOn         DATETIME     NULL,
        CONSTRAINT PK_Notification_Triggers PRIMARY KEY (TriggerKey)
    );
END
GO

MERGE dbo.tbl_Notification_Triggers AS t
USING (VALUES
    ('PORTAL',          'Portal (all modules)',                 'Portal', 0),
    ('LOGIN_MFA',       'Login MFA OTP',                        'Module', 10),
    ('PASSWORD_RESET',  'Login password reset OTP',             'Module', 20),
    ('PROFILE_OTP',     'Profile / email verification OTP',     'Module', 30),
    ('JOB_ALERT',       'Job out-punch / share alerts',         'Module', 40),
    ('JOB_INPUNCH',     'Job in-punch alerts',                  'Module', 50),
    ('HELPDESK',        'Helpdesk tickets',                     'Module', 60),
    ('PAYROLL',         'Payroll notifications',                'Module', 70),
    ('SUPPLY_MEMO',     'Supply memo',                          'Module', 80),
    ('EMPLOYEE_MASTER', 'Employee master emails',               'Module', 90),
    ('SYSTEM',          'System / utility emails',              'Module', 100)
) AS s (TriggerKey, DisplayName, Scope, SortOrder)
ON t.TriggerKey = s.TriggerKey
WHEN NOT MATCHED THEN
    INSERT (TriggerKey, DisplayName, Scope, SortOrder, EmailEnabled, WhatsAppEnabled)
    VALUES (s.TriggerKey, s.DisplayName, s.Scope, s.SortOrder, 1, 1)
WHEN MATCHED THEN
    UPDATE SET DisplayName = s.DisplayName, Scope = s.Scope, SortOrder = s.SortOrder;
GO
