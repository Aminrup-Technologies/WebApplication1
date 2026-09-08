# Configuration keys (operations)

**Status:** Verified against `WebApplication1/Web.config.example`. Production `Web.config` is gitignored.

## Security-relevant

| Key | Role |
| --- | --- |
| `SwitchUserAuthorizedUsers` | CSV of `WorkmanSL` for Switch User **fallback** (with Admin) |
| `PayrollAuthorizedUsers` | CSV for payroll overlay fallbacks |
| `SmtpUser`, `SmtpPass` | Email (MFA OTP, notifications). Host for helpdesk is **hardcoded** `smtp.zoho.in:587` in `HelpdeskCalls.cs` |
| `Msg91AuthKey`, `Msg91IntegratedNumber`, `Msg91MfaTemplateName`, `Msg91WhatsAppNamespace`, `Msg91WhatsAppLanguage` | WhatsApp OTP |
| `SupersetPassword` | Embedded dashboards; URL hardcoded on overview pages |
| `DefaultPasswordPrefix` | Password reset prefix (`ATS@` in example) |
| `AutoLogoutTimeoutMinutes`, `AutoLogoutRedirectUrl` | Idle UI logout |
| `sessionState timeout` | InProc Session minutes (`20` in example) |

## MFA is not an AppSetting

Per-employee `MFAEnabled` / `MFAMethod` on `tbl_Employee_Mustertable`. There is **no** `MfaEnabled` / `MfaBypassUsers` key in `Web.config.example`.

## JOB / payroll tunables

`TimeThresholdHours`, `PunchOutDurationMinutes`, `F17_GrossBreaker` and `F17_GrossBreaker_*` site keys.

Connection string `DbConn` lives in gitignored `connections.config`. Secrets file: `appsettings.secrets.config`.

Full dump: [appendix/configuration.md](../appendix/configuration.md).
