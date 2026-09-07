# Authorization Modernization — Technical Design (v2.6)

**Status:** PRs A, B, C1, C2, and D implemented. PR C CRUD and E are planned.  
**Date:** 2026-09-07  
**Authoritative audits:** `docs/ROLE_PERMISSION_ARCHITECTURE_AUDIT.md` (PR #94). Session identity remains as proven in PRs #86–#91.  
**PR A:** `docs/AUTHORIZATION_SERVICE_PR_A.md`  
**PR B:** `docs/AUTHORIZATION_OVERLAY_PR_B.md`  
**PR C1:** `WebApplication1/bussiness/production/admin/security/PermissionInspector.md`  
**PR C2:** `WebApplication1/bussiness/production/admin/security/AccessAnalyzer.md`  
**PR D:** `docs/AUTHORIZATION_MIGRATION_BRIDGE_PR_D.md`

## Governance freeze

Do not change these production truths:

| Truth | Implication |
| --- | --- |
| Authentication is Session-based | No Forms tickets. `GrantAuthenticatedSession` stays the only real login path. |
| `USERTYPE` is the runtime privilege string | `AuthorizationService.IsAdmin()` is `USERTYPE == "Admin"` only. |
| `RolePermissionDB` drives menu visibility | Master `LoadPermissions` is untouched. Overlay does not replace `tlb_EmployeePermissions`. |
| `WORKMAN` allowlists stay | Config + hardcoded lists remain a compatibility layer. |
| Existing pages stay as-is until explicitly migrated | Login / Session builder unchanged. PR D migrated privilege *consumers* to `CanAccess` without changing predicates. |

Do not replace the hybrid model. Build on top of it.

## Program

| PR | Scope | This repo |
| --- | --- | --- |
| A | `AuthorizationService` wrapping today’s behavior | Done (`cursor/authorization-service-foundation-cf5b`) |
| B | Overlay tables + `PermissionRepository` + cache + effective-permission wiring | Done (`cursor/permission-overlay-infrastructure-cf5b`) |
| C1 | Read-only Permission Inspector (`admin/security/PermissionInspector.aspx`) | Done (`cursor/permission-inspector-cf5b`) |
| C2 | Read-only Access Analyzer (`admin/security/AccessAnalyzer.aspx`) | Done (`cursor/access-analyzer-cf5b`) |
| D | Legacy migration bridge: page gates call `CanAccess`; snapshot baseline | **This change** |
| C | Security Admin WebForms CRUD under `bussiness/production/admin/security/` | Not started |
| E | Remaining USERTYPE routing (`create_jobid`) and overlay-backed cutover | Not started |

## PR A API (canonical)

`WebApplication1/App_Code/AuthorizationService.cs`

| Method | Legacy equivalent |
| --- | --- |
| `IsAuthenticated()` | `webmaster.Master` / `homepage_v2` five-key presence: `USERID`, `RolePermissionDB`, `UserRoleDB`, `USERNAME`, `WORKMAN` |
| `IsAdmin()` | `Session["USERTYPE"] == "Admin"`. **Not** JOB360’s local `IsAdmin()` (Admin **or** Office Staff) |
| `CanAccess("SWITCH_USER")` | `ImpersonationAudit.CanImpersonate`, then Admin-gated overlay |
| `CanAccess("PAYROLL_OVERRIDE")` | Overlay, else `PayrollAuthorizedUsers` CSV vs `WORKMAN` |
| `CanAccess("JOB360_OVERRIDE")` | JOB360 `IsAdmin()`: Admin **or** Office Staff, else overlay |
| `CanAccess("ATTENDANCE_OVERRIDE")` | Same module exception as JOB360, else overlay |
| `CanAccess("EXPORT_PAYROLL")` | Overlay, else hardcoded `WORKMAN == J8` |
| `CanAccess("USER_ADMIN")` | Overlay only (no grants seeded → false) |
| `IsWorkmanAllowed(code)` | Overlay → config CSV → hardcoded WorkmanSL |
| `HasPermission(code)` | Same as `CanAccess` |
| `GetEffectivePermissions()` | Source tags: `USERTYPE`, `MODULE_EXCEPTION`, `GROUP`, `DIRECT`, `LEGACY_CONFIG`, `LEGACY_HARDCODED` |
| `DescribeIdentity(...)` | Same gates for another employee; restores live Session (PR C1 inspector / PR C2 analyzer) |
| `DisplaySource(source, granted)` | Stable labels for reports: Direct / Group / Config / Hardcoded / Module / Admin / Denied |

`CanAccess` does **not** short-circuit “if Admin then true”. That would give every Admin Switch User and payroll override, which production does not do.

## Overlay infrastructure (PR B)

Keep `tlb_emp_roles`, `tlb_emp_roles_permission`, `tlb_EmployeePermissions`, `tbl_Employee_Mustertable` unchanged.

Add (script `scripts/create_permission_overlay.sql`): `tlb_permissions`, `tlb_permission_groups`, `tlb_group_permissions`, `tlb_employee_permissions`, `tlb_employee_group`.

`tlb_employee_permissions` (overlay direct grants) is a different object from `tlb_EmployeePermissions` (menu matrix).

Seed codes: `SWITCH_USER`, `PAYROLL_OVERRIDE`, `JOB360_OVERRIDE`, `ATTENDANCE_OVERRIDE`, `EXPORT_PAYROLL`, `USER_ADMIN`. No employee or group assignments.

`PermissionRepository` loads catalog, group membership, and direct/group grants. `HttpRuntime.Cache`, 5-minute TTL, `Invalidate` / `InvalidateAll` for PR C. Missing tables fail closed.

## Effective permission order (PR B+)

```
Session (IsAuthenticated)
        ↓
Legacy module authority (CanImpersonate / Admin|Office Staff)
        ↓ (miss)
Permission overlay (direct / group)
        ↓ (miss)
Config allowlist (SwitchUserAuthorizedUsers, PayrollAuthorizedUsers)
        ↓ (miss)
Hardcoded WorkmanSL (J8, A84, K208, N21)
```

`SWITCH_USER` overlay requires `USERTYPE == Admin` and not impersonating. Overlay must not let Office Staff impersonate.

## Permission Inspector (PR C1)

Direct URL: `~/bussiness/production/admin/security/PermissionInspector.aspx`  
Gate: live `AuthorizationService.IsAdmin()` only. No menu item.

Subject evaluation uses `DescribeIdentity` (same `CanAccess` order) and restores Session. Overlay health and cache peek are read-only `PermissionRepository` calls. Missing overlay tables show a warning and do not throw.

## Access Analyzer (PR C2)

Direct URL: `~/bussiness/production/admin/security/AccessAnalyzer.aspx`  
Gate: live `AuthorizationService.IsAdmin()` only. No menu item.

Permission / User / Legacy rows come from `DescribeIdentity`. Overlay mode is inventory-only (`GetDirectGrantInventory`, groups, unused codes, orphan groups). CSV, print, and **Snapshot** are read-only.

## Legacy migration bridge (PR D)

Page privilege consumers call `AuthorizationService.CanAccess`. Predicates are unchanged (empty overlay). JOB360 / attendance stay Admin **or** Office Staff. Switch User stays Admin-only.

Read-only snapshot: `AuthorizationSnapshot` (SHA-256). Download from the Analyzer.

## What PR B does not do

- No `.aspx` / `.aspx.cs` / master changes
- No Security Admin UI
- No retirement of hardcoded lists
- No change to `ImpersonationAudit` or login
- No overlay assignments in production
