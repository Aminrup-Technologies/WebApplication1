# Authorization Modernization — Technical Design (v2.2)

**Status:** PR A implemented. PRs B–E are planned, not built.  
**Date:** 2026-09-07  
**Authoritative audits:** `docs/ROLE_PERMISSION_ARCHITECTURE_AUDIT.md` (PR #94). Session identity remains as proven in PRs #86–#91.

## Governance freeze

Do not change these production truths:

| Truth | Implication |
| --- | --- |
| Authentication is Session-based | No Forms tickets. `GrantAuthenticatedSession` stays the only real login path. |
| `USERTYPE` is the runtime privilege string | `AuthorizationService.IsAdmin()` is `USERTYPE == "Admin"` only. |
| `RolePermissionDB` drives menu visibility | Master `LoadPermissions` is untouched. Overlay does not replace `tlb_EmployeePermissions`. |
| `WORKMAN` allowlists stay | Config + hardcoded lists remain a compatibility layer. |
| Existing pages stay as-is until explicitly migrated | PR A has **zero** page edits. |

Do not replace the hybrid model. Build on top of it.

## Program

| PR | Scope | This repo |
| --- | --- | --- |
| A | `AuthorizationService` wrapping today’s behavior | **This change** |
| B | Overlay tables (`tlb_permissions`, groups, assignments) SQL only | Not started |
| C | Security Admin WebForms under `bussiness/production/admin/security/` | Not started |
| D | Replace hardcoded `J8`/`A84`/… and config reads with `HasPermission` / `IsWorkmanAllowed`, keeping fallbacks | Not started |
| E | Module migration: Switch User, Payroll, Attendance, JOB360, Administration | Not started |

## PR A API

`WebApplication1/App_Code/AuthorizationService.cs`

| Method | Legacy equivalent |
| --- | --- |
| `IsAuthenticated()` | `webmaster.Master` / `homepage_v2` five-key presence: `USERID`, `RolePermissionDB`, `UserRoleDB`, `USERNAME`, `WORKMAN` |
| `IsAdmin()` | `Session["USERTYPE"] == "Admin"`. **Not** JOB360’s local `IsAdmin()` (Admin **or** Office Staff) |
| `CanAccess("SWITCH_USER")` | `ImpersonationAudit.CanImpersonate` (Admin + `SwitchUserAuthorizedUsers` + not impersonating) |
| `CanAccess("PAYROLL_OVERRIDE")` | `PayrollAuthorizedUsers` CSV vs `WORKMAN` |
| `CanAccess("JOB360_OVERRIDE")` | JOB360 `IsAdmin()`: Admin **or** Office Staff |
| `CanAccess("ATTENDANCE_OVERRIDE")` | `manage_job_exceptions` / `analyze_attendance_anomalies` page gate |
| `CanAccess("EXPORT_PAYROLL")` | Hardcoded `WORKMAN == J8` on payroll dashboards |
| `CanAccess("USER_ADMIN")` | No current page gate → **false** until overlay (PR B) |
| `IsWorkmanAllowed(code)` | Overlay (stub) → config CSV → hardcoded WorkmanSL |
| `HasPermission(code)` | Same as `CanAccess` in PR A |
| `GetEffectivePermissions()` | Source tags: `USERTYPE`, `LEGACY_CONFIG`, `LEGACY_HARDCODED`, `MODULE_EXCEPTION`, `DIRECT` (empty until PR B) |

`CanAccess` does **not** short-circuit “if Admin then true”. That would give every Admin Switch User and payroll override, which production does not do.

Overlay lookup is a stub that returns false and **does not query** tables that do not exist yet.

## Overlay (PR B, not in this PR)

Keep `tlb_emp_roles`, `tlb_emp_roles_permission`, `tlb_EmployeePermissions`, `tbl_Employee_Mustertable` unchanged.

Add: `tlb_permissions`, `tlb_permission_groups`, `tlb_group_permissions`, `tlb_employee_permissions`, `tlb_employee_group`.

Seed codes: `SWITCH_USER`, `PAYROLL_OVERRIDE`, `JOB360_OVERRIDE`, `ATTENDANCE_OVERRIDE`, `EXPORT_PAYROLL`, `USER_ADMIN`.

## Compatibility priority (PR D+)

```
Permission overlay (direct / group)
        ↓ (miss)
Config allowlist (SwitchUserAuthorizedUsers, PayrollAuthorizedUsers)
        ↓ (miss)
Hardcoded WorkmanSL (J8, A84, K208, N21)
        ↓ (miss)
Module USERTYPE exception (Office Staff on JOB360 / attendance)
```

`SWITCH_USER` stays delegated to `CanImpersonate` until PR E. Overlay must not bypass `USERTYPE == Admin` without a dedicated CR.

## What this PR does not do

- No `.aspx` / `.aspx.cs` / master changes
- No SQL
- No Security Admin UI
- No retirement of hardcoded lists
- No change to `ImpersonationAudit` or login
