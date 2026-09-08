# Configuration keys (inventory)

**Source:** `WebApplication1/Web.config.example` plus cited code. Production files are gitignored.

## appSettings (example file)

| Key | Example / notes |
| --- | --- |
| `AutoLogoutTimeoutMinutes` | `20` |
| `AutoLogoutRedirectUrl` | `https://atswork.in/` |
| `vs:EnableBrowserLink` | `false` |
| `PayrollAuthorizedUsers` | `J8` |
| `SwitchUserAuthorizedUsers` | `J8` |
| `ValidationSettings:UnobtrusiveValidationMode` | `None` |
| `TimeThresholdHours` | `72` (job approval page) |
| `F17_GrossBreaker_KPO` / `_AGL` / `_NINL` / `_JSR` / `_RSP` / `_TSKP` | Site breakers |
| `F17_GrossBreaker` | Default `20500` |
| `PunchOutDurationMinutes` | `4320` (example); OUT page falls back to `1920` if empty |
| `SmtpUser` | Email account |
| `SmtpPass` | Empty in example; secrets file |
| `Msg91AuthKey` | Empty in example |
| `Msg91IntegratedNumber` | WhatsApp sender |
| `Msg91MfaTemplateName` | `login_otp` |
| `Msg91WhatsAppNamespace` | UUID-style |
| `Msg91WhatsAppLanguage` | `en` |
| `SupersetPassword` | Empty in example |
| `DefaultPasswordPrefix` | `ATS@` |
| `EnableLoginTimingLog` | Read in login/homepage; **not** in the example file (**Verified** code vs example gap) |

## system.web (example)

| Setting | Value / note |
| --- | --- |
| `customErrors` | `RemoteOnly` |
| `httpCookies` | `httpOnlyCookies=true`, `requireSSL=false` |
| `compilation` | `targetFramework=4.8` |
| `authentication mode` | `Forms` — **unused by login** |
| `forms` | `loginUrl=~/login.aspx`, timeout 2880 — unused ticket |
| `httpRuntime` | large `maxRequestLength`, `requestValidationMode=2.0` |
| `sessionState` | `InProc`, timeout 20 |
| `machineKey` | Present in example — **do not** ship to production |
| codedom | C# `/langversion:6` |

## Code hardcoded (not AppSettings)

| Item | Where |
| --- | --- |
| SMTP host `smtp.zoho.in:587` | `HelpdeskCalls.cs` (and similar notify helpers) |
| Superset base URL | Overview `*.aspx.cs` (`https://reports.aminruptechnologies.co.in`) |

## Related

- [admin config-keys](../administration/config-keys.md)
- [mfa](../architecture/mfa.md)
