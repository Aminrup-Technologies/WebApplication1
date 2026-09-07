# Security Foundation baseline (v2.2)

**Status:** Permanent platform baseline. Do not reopen the identity or authorization model without a new versioned CR.

| Field | Value |
| --- | --- |
| Release tag | `v2.2-security-foundation` |
| Baseline commit | `1f6c147` (`1f6c14700351a203bb00bc9413f60556f8e8dd68`) |
| Release branch | `Jul_to_Sep_2026_Suport_N_Dev_Works` |
| Product | ATS ERP Security Foundation |
| Identity | ASP.NET InProc Session (`ASP.NET_SessionId`) |
| Canonical login | `WebApplication1/Login.aspx` + `Login.aspx.cs` (`login`) |
| Landing | `~/bussiness/production/homepage_v2.aspx` |

This document records what v2.2 delivered and the invariants later work must preserve. Architecture detail lives in `docs/architecture/`. Engineering rules live in `CONTRIBUTING.md`.

## Completed capabilities

| Capability | Where |
| --- | --- |
| Session identity builder | `login.ApplySessionFromEmployeeRow` from `tbl_Employee_Mustertable` |
| Login side effects | `GrantAuthenticatedSession` (audit, LastLogin, LoginStatus, remember-me cookie, homepage redirect) |
| MFA | Email OTP, WhatsApp OTP, TOTP challenge/enroll **before** Session identity |
| Impersonation foundation | `ImpersonationAudit`, `ORIGINAL_*`, `IS_IMPERSONATING`, `IMPERSONATION_CORR` |
| Switch User | `SwitchUser.aspx` — Admin dual-path; no nested switch; restore via original employee row |
| Master chrome | Impersonation banner + Switch User / Return link on `webmaster.Master` |
| Central gate | `AuthorizationService` (`IsAdmin`, `CanAccess`, `DescribeIdentity`, `DescribeSwitchUser`) |
| Overlay schema | `tlb_permissions`, `tlb_employee_permissions`, groups; `PermissionRepository` (5-minute cache) |
| Observability | Permission Inspector, Access Analyzer, authorization snapshot, SWITCH_USER canary |
| Platform Admin pack | `scripts/bootstrap_platform_admin.sql` — Active Admin × `SWITCH_USER`, `USER_ADMIN`, `PAYROLL_OVERRIDE`, `EXPORT_PAYROLL` |
| Release evidence | `docs/release/TEMPLATE/` and filled `docs/release/v2.2-security-foundation/` |

## Release merge history

Squash order onto `Jul_to_Sep_2026_Suport_N_Dev_Works`. **#103 was not merged.**

| PR | Squash commit | Title |
| ---: | --- | --- |
| #89 | `e10d6cb` | feat(auth): impersonation audit foundation (no Switch User) |
| #91 | `6397c1e` | feat(auth): Switch User impersonation (roadmap PR #90) |
| #94 | `abd20b7` | docs(auth): Role & Permission architecture audit |
| #95 | `883771a` | feat(auth): AuthorizationService foundation (modernization PR A) |
| #96 | `4307111` | feat(auth): permission overlay infrastructure (modernization PR B) |
| #98 | `ac0e863` | feat(auth): read-only Permission Inspector (modernization PR C1) |
| #99 | `30b60dd` | feat(auth): read-only Access Analyzer (modernization PR C2) |
| #100 | `36a7a4c` | feat(auth): legacy migration bridge + authorization snapshot (PR D) |
| #102 | `d1c961b` | feat(auth): SWITCH_USER overlay canary (PR E, dual-path) |
| #104 | `a7048c4` | Platform Admin bootstrap — overlay permission pack |
| — | `1f6c147` | docs(release): restore v2.2-security-foundation evidence pack |
| #103 | *archived* | DO NOT MERGE: UAT integration — Security Foundation v2.2 |

Tag `v2.2-security-foundation` points at `1f6c147`.

## Architectural invariants

1. **Session is identity.** Do not add Forms tickets or a second login Session builder.
2. **`ApplySessionFromEmployeeRow` is the only identity materializer.** Login and Switch User share it. MFA transients are cleared there; `USERID` is not set during an MFA challenge.
3. **`GrantAuthenticatedSession` is login-only.** Switch User must not update LastLogin / LoginStatus / `ATS_SavedID`.
4. **Master GET (`webmaster.Master`, `!IsPostBack`) requires five Session keys:** `USERID`, `RolePermissionDB`, `UserRoleDB`, `USERNAME`, `WORKMAN`. Presence only — values of the `*DB` keys are not compared.
5. **`UserRoleDB` is not runtime authority.** It is the numeric `tlb_emp_roles.EmpType_Value`. Pages null-check it. They must not compare its value for privilege.
6. **`RolePermissionDB` → `tlb_EmployeePermissions` is menu chrome.** It is not page authorization.
7. **`AuthorizationService.IsAdmin()` is `USERTYPE == "Admin"` only.** Office Staff is never global admin and never Switch User.
8. **Office Staff privilege is module-local** (JOB360 override, attendance/job-exceptions). Do not spread it.
9. **New checks go through `AuthorizationService`.** Do not add WorkmanSL string gates or new Session privilege comparisons.
10. **Overlay dual-path stays until a CR retires the fallback with canary evidence.** Empty overlay ⇒ same SWITCH_USER allow/deny as Admin + `SwitchUserAuthorizedUsers`.
11. **Overlay tables ≠ sidebar table.** `tlb_employee_permissions` (overlay) is not `tlb_EmployeePermissions` (menus).
12. **C# 6 / unique Compile items.** Do not introduce newer language syntax or duplicate `.csproj` Compile entries.

## Known legacy inventory (baseline scan)

Governance scan of `WebApplication1/` at `1f6c147`. **Not defects to fix in this PR.** New code must not add to this list.

### Hardcoded WorkmanSL gates (live)

| Location | Values | Routed through `AuthorizationService`? |
| --- | --- | --- |
| `AuthorizationService.IsWorkmanOnHardcodedList` | `J8` (`EXPORT_PAYROLL`, `LEGACY_PAYROLL_DASHBOARD`); `J8`/`A84`/`K208`/`N21` (`LEGACY_ATTACH_MANPOWER`); `J8`/`A84`/`K208` (`LEGACY_EXPENSE_HEADS`) | Yes — the allowed remaining copy |
| `add_expenses.aspx.cs` `CheckUser` | `J4` locks company/work-order dropdowns | **No** — legacy page gate; do not copy |
| `emp_registration.aspx.cs` | fallback `"J8"` when `Session["WORKMAN"]` is null (audit stamp, not a gate) | No |
| `rpts/testing.aspx.cs` | `EmployeeID = "A84"` | Test harness |

Commented (inactive): `create_supplymemo.aspx.cs` A84/K208; `view_monthlyjobs.aspx.cs` J8.

Config CSVs (not hardcoded): `SwitchUserAuthorizedUsers`, `PayrollAuthorizedUsers`.

### Direct Session privilege checks (value compared)

| Location | What |
| --- | --- |
| `create_jobid.aspx.cs` | `USERTYPE` Office Staff / Site Staff (WO / region rules) |
| `create_jobid_v2.aspx.cs` | `USERTYPE` Site Staff (query / region lock) |

JOB360 / attendance / payroll extra chrome now call `AuthorizationService.CanAccess`. ~100 pages still **null-check** Session keys for login presence; that is authentication, not privilege.

### Authorization bypasses (by design of the hybrid model)

- Sidebar hide does not block a direct URL.
- `aminrup.Master` and `Admin.Master` do not use the production five-key + menu gate.
- `ImpersonationAudit.CanImpersonate` remains Admin + CSV only; pages use `CanAccess("SWITCH_USER")` (dual-path). Do not call `CanImpersonate` for new UI.
- Logout while impersonating updates `LoginStatus` / LastLogout for the **current** (`USERID` / `WORKMAN`) — the target, not the captured admin. Return-to-admin is the supported exit.

### Duplicate permission logic

- Menu matrix (`tlb_EmployeePermissions`) vs overlay (`tlb_employee_permissions`) — two systems; do not merge them.
- `ImpersonationAudit.CanImpersonate` vs `AuthorizationService.DescribeSwitchUser` — legacy engine vs dual-path canary. New callers use the service.
- `job_360_view.IsAdmin()` is a page helper that delegates to `CanAccess(JOB360_OVERRIDE)` (Admin **or** Office Staff). It is **not** `AuthorizationService.IsAdmin()`.
