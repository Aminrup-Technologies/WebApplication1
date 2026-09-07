# Security Foundation v2.2 — Release notes

**Date:** 2026-09-07  
**UAT branch:** `uat/security-foundation-v2.2` (PR **#103 — DO NOT MERGE**)  
**Integrity SHA:** `965934f` (RC pack `8b81906`)  
**Verdict:** **BLOCKED** (see blockers below)

This document consolidates the squash sequence for production. It does not change application behavior. **#103 is excluded** from the merge list.

## Executive summary

The Security Foundation stack (#89–#102) is implemented, CI-green, and integrated on a throwaway UAT branch. Live SQL on `atserp_uat` confirms operator `J8` is already platform Admin (`User_RoleType=Admin`, `UserRoleDB=ATS-OS`, `RolePermissionDB=OS-HR`). Overlay tables are **not** on UAT. VS2015/IIS runtime, Switch User pages, Inspector, Analyzer, overlay schema deploy, canary, and snapshot compare have **no completed evidence**. `#104` is additive SQL/docs for after the squash; it has not been executed.

Until `docs/release/v2.2-security-foundation/FINAL_SIGNOFF.md` is **GO**, do not squash-merge.

## Architecture changes

Hybrid model is unchanged: ASP.NET Session identity (`ApplySessionFromEmployeeRow`), sidebar from `RolePermissionDB` → `tlb_EmployeePermissions`, privilege from `USERTYPE` (`User_RoleType`). Overlay (`tlb_permissions` / `tlb_employee_permissions`) is additive and fail-closed. `AuthorizationService.IsAdmin()` is `USERTYPE == "Admin"` only. `UserRoleDB` is presence-checked, not an allow/deny value.

| Layer | Change |
| --- | --- |
| Identity | Session builder extracted; Switch User reuses it without `GrantAuthenticatedSession` side effects |
| Audit | `ImpersonationAudit` IMPERSONATE / RETURN + `ORIGINAL_*` keys |
| Authorization | `AuthorizationService` + `PermissionRepository` (5-minute cache) |
| Observability | Permission Inspector, Access Analyzer, hashed snapshots |
| Overlay | Schema + SWITCH_USER dual-path canary |
| Bootstrap (#104) | SQL pack for Active Admins — **after** squash; not in #103 |

## Authentication changes

None to the login contract. Canonical path remains `/Login.aspx`. `GrantAuthenticatedSession` still owns `LastLogin`, `LoginStatus`, audit, `ATS_SavedID`, homepage redirect. MFA transients stay in login. Switch User must not call `GrantAuthenticatedSession`.

## Authorization changes

| PR | What |
| --- | --- |
| #89 | `CanImpersonate` = Admin USERTYPE **and** `SwitchUserAuthorizedUsers` |
| #91 | Switch User page; master chrome; nested switch blocked |
| #94 | Docs of the hybrid model |
| #95 | `CanAccess` / `IsAdmin` / `DescribeIdentity` |
| #96 | Overlay tables and repository (no employee grants) |
| #98 / #99 | Read-only Inspector / Analyzer (Admin gate) |
| #100 | Snapshot hash/compare; empty overlay ≡ #95 allow/deny |
| #102 | `SWITCH_USER` dual-path: Admin, then overlay, else CSV |
| #104 | Overlay grants for Active Admins (SQL only; not run on UAT) |

Office Staff never receive `SWITCH_USER`. JOB360 / attendance remain module exceptions.

## Overlay changes

`scripts/create_permission_overlay.sql` creates catalog + empty grant tables. Canary inserts `(WorkmanSL, PermissionId)` for `SWITCH_USER`. `#104` would grant `SWITCH_USER`, `USER_ADMIN`, `PAYROLL_OVERRIDE`, `EXPORT_PAYROLL` to every Active Admin. **UAT overlay tables: MISSING** as of 2026-09-07.

## UAT evidence

| Gate | Evidence | Result |
| --- | --- | --- |
| Repository / CI | Stack PRs + UAT branch, 4/4 checks | PASS |
| Live Admin SQL | `promote_uat_admin.sql` on `J8` | PASS (already Admin; apply no-op) |
| VS2015 rebuild | `BUILD_EVIDENCE.md` | **not filled** |
| IIS / Session | `IIS_VALIDATION.md` | **not filled** |
| Overlay schema | INFORMATION_SCHEMA | **MISSING** |
| Canary / snapshot | `CANARY_EVIDENCE.md` | **not filled** |
| `#104` bootstrap | not executed | **not run** |
| RC sign-off | `FINAL_SIGNOFF.md` | **not GO** |

## SQL scripts executed

| Script | Environment | Result |
| --- | --- | --- |
| `scripts/promote_uat_admin.sql` | `atserp_uat` | Dry-run + apply. `J8` already `Admin` / `ATS-OS` / `OS-HR`. `LastLogin` unchanged. |
| `scripts/create_permission_overlay.sql` | UAT | **Not run** (tables missing) |
| `scripts/uat_switch_user_canary.sql` | UAT | **Not run** |
| `scripts/bootstrap_platform_admin.sql` | UAT | **Not run** (#104) |

## Known exclusions

- **PR #103** — UAT orchestration only. Never merge.
- **PR #86** — docs-only session audit; not in squash list.
- **Logout PR #92** — still locked (restore original admin, then existing logout).
- **Security Admin CRUD** — not started; wait for SWITCH_USER canary on IIS.
- **Office Staff** — not Switch User; not `#104` grants.
- **No `Employee_Type='Admin'` catalog row** — do not invent one.
- **Linux CI ≠ VS2015** — .NET 4.8 rebuild is Windows-only.

## Rollback strategy

| Layer | Rollback |
| --- | --- |
| Git | Do not merge #103. Individual PRs remain the merge artifacts; revert a squash commit if needed. |
| Overlay schema | Leave tables in place (additive). Do not `DROP`. |
| Canary grant | Commented `DELETE` in `uat_switch_user_canary.sql` (that WorkmanSL + `SWITCH_USER` only). Recycle IIS. |
| `#104` grants | `DELETE` four pack codes × Active Admin (`PLATFORM_ADMIN_BOOTSTRAP.md`). Recycle IIS. CSV/hardcoded paths remain. |
| Session | InProc; recycle app pool clears impersonation leftovers. |

## Final recommendation

**BLOCKED**

Exact blockers:

1. VS2015 Clean + Rebuild not recorded (`BUILD_EVIDENCE.md`).
2. IIS recycle / `Login.aspx` / Session for `J8` not recorded (`IIS_VALIDATION.md`).
3. Switch User, Permission Inspector, Access Analyzer pages not evidenced.
4. Overlay tables missing on `atserp_uat` (`create_permission_overlay.sql` not run).
5. SWITCH_USER canary INSERT/DELETE and snapshot compare not run (`CANARY_EVIDENCE.md`).
6. `FINAL_SIGNOFF.md` is not **GO**.
7. `#104` must wait until overlay tables exist and `#89–#102` are squashed; bootstrap not executed.
8. `#94` is still **draft** (docs-only; convert to ready before squash).
