# Repository inventory (Phase 0)

**Baseline:** `v2.2.3-governance-final` (`495fc68`)  
**Kind:** Discovery. Counts and paths from the tree at program start. **Verified** unless marked otherwise.

Solution: `WebApplication1.sln` → project `WebApplication1/` (ASP.NET WebForms, .NET Framework 4.8, C# 6).

## Module inventory (ERP production)

Grouping is by **page filename and existing docs**, not by a catalog table. **Inferred** module names in parentheses where no formal module code exists.

| Module | Representative pages | Existing docs |
| --- | --- | --- |
| Identity / login | `Login.aspx`, `homepage_v2.aspx`, `SwitchUser.aspx` | `docs/architecture/authentication-flow.md` |
| Employee / muster | `emp_registration.aspx`, `emp_bulkregistration.aspx`, `viewupdate_empmustertabledata*.aspx`, `view_emp_mastertbldata*.aspx` | None dedicated |
| JOBID | `create_jobid_v2.aspx`, `job_inpunch_v2.aspx`, `job_permitupload_v2.aspx`, `job_outpunch_v2.aspx`, `job_360_view.aspx`, `jobs_and_manpower_v2.aspx`, `supervisor_wizard.aspx` | JOBID CR packs, `JOBID_V2.2_ROADMAP.md` |
| Attendance | `view_empmonthlyatt.aspx`, `generate_atdncsheet.aspx`, `analyze_attendance_anomalies.aspx`, `manage_job_exceptions.aspx` | Partial (auth overlay codes) |
| Payroll | `pyrl_*.aspx`, `payroll_controller.aspx`, `generate_*sheets.aspx`, `PayrollSummary.aspx` | Partial (allowlists) |
| Expenses | `add_expenses.aspx`, `add_expense_heads.aspx`, `add_expense_subheads.aspx`, `view_expenses.aspx` | Hardcoded `J4` noted in baseline |
| Supply memo | `create_supplymemo.aspx`, `vw_supplyjobs.aspx` | Commented Workman gates |
| CSM / HSE | `csm_*.aspx`, `csms_mainview.aspx`, toolbox / SOP / TBT | None dedicated |
| Helpdesk | `add_nwhelpdsk.aspx`, `helpdesk_ticketdetails.aspx`, `GrievanceReq_View.aspx` | None dedicated |
| Masters / geo | `work_country.aspx`, `work_states.aspx`, `work_region.aspx`, `work_company.aspx`, `ats_work_sites.aspx` | None dedicated |
| Roles / menus | `manage_rolls.aspx`, `manage_rollsaccess.aspx`, `aminrup/manage_roles.aspx` | Architecture audit |
| Security admin | `admin/security/PermissionInspector.aspx`, `AccessAnalyzer.aspx` | Overlay / canary docs |
| Public marketing | `home.aspx`, `About.aspx`, `career.aspx`, `apply.aspx` (`atsSite.Master`) | Out of ERP session model |
| Legacy `atsweb.Admin` | `Admin/Dashboard.aspx`, `Category.aspx`, `Product.aspx` | Empty master `Page_Load` |

## Page inventory

| Area | `.aspx` count (glob) |
| --- | ---: |
| `WebApplication1/` total | 174 |
| `bussiness/production/` | 158 |
| Public / Admin / other | 16 |

Full production list is the glob under `WebApplication1/bussiness/production/**/*.aspx`. Do not treat `Loginold.aspx` as canonical login.

## Code-behind / App_Code

| Kind | Evidence |
| --- | --- |
| Code-behind | Matching `.aspx.cs` next to each page (not all pages inventoried line-by-line in this pass) |
| App_Code | `AuthorizationService.cs`, `PermissionRepository.cs`, `ImpersonationAudit.cs`, `AuthorizationSnapshot.cs`, `JobStatusConstants.cs`, `JobIdCodec.cs`, `NotificationHelper.cs` |
| Shared production helpers | `SessionKeys.cs`, `MfaAuthHelper.cs`, `MfaTotpHelper.cs` |

## Master pages

| File | Role (Verified) |
| --- | --- |
| `bussiness/production/webmaster.Master` | Canonical ERP chrome; five-key Session gate; `tlb_EmployeePermissions` menus; impersonation banner |
| `bussiness/production/aminrup/aminrup.Master` | Session presence only; no menu table |
| `atsSite.Master` | Public site |
| `Admin/Admin.Master` | Legacy `atsweb.Admin`; empty `Page_Load` |

## SQL scripts (`scripts/`)

| File | Role |
| --- | --- |
| `create_permission_overlay.sql` | Overlay schema (additive) |
| `bootstrap_platform_admin.sql` | Active Admin overlay pack |
| `bootstrap_platform_admin_verify.sql` | Verify pack |
| `uat_switch_user_canary.sql` | SWITCH_USER canary |
| `add_employee_mfa_columns.sql` / `add_employee_mfa_totp_columns.sql` | MFA columns |
| `add_notification_triggers.sql` | Notification triggers |
| `security-check.py` / `mfa_helper_check.py` | CI / helper checks |

## Configuration keys (`Web.config.example`)

Verified keys include: `AutoLogoutTimeoutMinutes`, `AutoLogoutRedirectUrl`, `PayrollAuthorizedUsers`, `SwitchUserAuthorizedUsers`, `TimeThresholdHours`, `F17_GrossBreaker_*`, `PunchOutDurationMinutes`, `SmtpUser` / `SmtpPass`, `Msg91*` MFA WhatsApp, `DefaultPasswordPrefix`. Connection string `DbConn` is not listed here (secrets). `<authentication mode="Forms">` is unused by login (**Verified** in architecture audit / CONTRIBUTING).

## Documentation already present

Do not rewrite:

- `docs/architecture/{authentication-flow,authorization-flow,impersonation-lifecycle,permission-overlay}.md`
- `docs/governance/*`, `CONTRIBUTING.md`, `SECURITY_FOUNDATION_BASELINE.md`, `.github/CODEOWNERS`
- `docs/release/TEMPLATE/` and `docs/release/v2.2-security-foundation/`
- JOBID: `JOBID_*`, `CR-001_*`, `CR-009_*`, `CR-010_*`, `MAINTENANCE_GUIDELINES.md`
- Security design notes: `ROLE_PERMISSION_ARCHITECTURE_AUDIT.md`, `AUTHORIZATION_*`, `PLATFORM_ADMIN_*`, `SWITCH_USER_CANARY_VALIDATION.md`

## PR / release references already documented

| Stream | References |
| --- | --- |
| Security Foundation | PRs #89–#104 squash; #103 not merged; tags `v2.2-security-foundation` … `v2.2.3-governance-final` |
| Governance | #105, #106 (closed), #107 |
| JOBID remediation | #59–#65, tag `v2.1.0-jobid-remediation` |
| CR-010 | #79, #81, #82, #83 (#80 superseded) |
| CR-001 | Phase A certified `238bd2f` |
