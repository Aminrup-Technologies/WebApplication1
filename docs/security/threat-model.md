# Security threat model (ATS ERP)

**Status:** Verified against current runtime. Not a pentest report.

## Assets

| Asset | Evidence |
| --- | --- |
| InProc Session identity | `Login.aspx.cs`, `webmaster.Master` |
| Overlay grants | `tlb_employee_permissions`, `PermissionRepository` |
| Impersonation stack | `SessionKeys.Impersonation*` |
| MFA OTP / TOTP | Per-employee `MFAEnabled` on muster; SMTP / MSG91 AppSettings |
| Config CSVs | `SwitchUserAuthorizedUsers`, `PayrollAuthorizedUsers` |

## Threats and mitigations (Verified)

| Threat | Mitigation in code | Residual risk |
| --- | --- | --- |
| Unauthenticated ERP page | `webmaster.Master` redirects if any of five keys missing | `aminrup.Master` is presence-only; `Admin.Master` `Page_Load` empty; `work_country` relies on master only |
| Stolen cookie | `httpOnlyCookies=true`; `requireSSL=false` in example | TLS is ops; example does not force HTTPS cookies |
| Privilege via `RolePermissionDB` string | Master does **not** compare values | Writing Session still needs keys **present**; overlay is separate |
| Overlay grant without Admin | Intended for many codes | `SWITCH_USER` still requires Admin + overlay/CSV |
| CSV `SwitchUserAuthorizedUsers` | Dual-path fallback | CSV bypasses overlay for Switch User when Admin |
| Impersonation without audit | `ImpersonationAudit.Write` on start/stop | `CanImpersonate` is Admin+CSV only (stricter than UI) |
| MFA skip | Per-row `MFAEnabled`; missing columns fail open to MFA off (`Login.aspx.cs`) | Residual: fail-open if ALTER not applied; no config bypass list in example |
| Password in muster | Registration stores **plain + hashed** | Residual: **high** — plaintext at rest |
| Hardcoded WorkmanSL | `AuthorizationService` lists for payroll/manpower/expense heads | Residual: identity as privilege |
| Expense approver stamp | `@AppByWrk='J3'` hardcoded on insert | Residual: false approver identity |
| 5-minute overlay cache | `PermissionRepository` | Revoke lag up to TTL |
| `ReturnUrl` | Login `IsLocalUrl` | Relative-only |
| Example `machineKey` | Present in `Web.config.example` | Residual: **do not** copy example keys to production |
| Helpdesk SMTP host | Hardcoded Zoho host | Credential still from AppSettings |

## Out of scope

Network firewall, full SQLi/XSS inventory, Forms tickets (**not used** by login).

## Related

- [authentication-flow](../architecture/authentication-flow.md)
- [authorization-flow](../architecture/authorization-flow.md)
