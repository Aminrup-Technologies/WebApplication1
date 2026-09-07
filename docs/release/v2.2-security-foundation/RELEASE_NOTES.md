# Security Foundation v2.2 — Release notes

**Date:** 2026-09-07  
**UAT branch:** `uat/security-foundation-v2.2` (PR **#103 — DO NOT MERGE**)  
**Integrity SHA:** `965934f` (RC pack `8b81906`)  
**Verdict:** **READY FOR SQUASH MERGE**

This document consolidates the squash sequence for production. It does not change application behavior. **#103 is excluded** from the merge list. Remaining work is GitHub governance only (squash, archive, tag).

## Executive summary

The Security Foundation stack (#89–#102) is implemented, CI-green, and integrated on the throwaway UAT branch. Live SQL on `atserp_uat` confirms operator `J8` is platform Admin (`User_RoleType=Admin`, `UserRoleDB=ATS-OS`, `RolePermissionDB=OS-HR`). Runtime UAT (VS2015, IIS, J8 login, Switch User, Inspector, Analyzer, overlay deploy, SWITCH_USER canary, zero unexpected authorization drift) is signed **PASS** in `FINAL_SIGNOFF.md` (**GO**). `#104` is additive SQL/docs and merges **after** `#102`.

## Architecture changes

Hybrid model is unchanged: ASP.NET Session identity (`ApplySessionFromEmployeeRow`), sidebar from `RolePermissionDB` → `tlb_EmployeePermissions`, privilege from `USERTYPE` (`User_RoleType`). Overlay (`tlb_permissions` / `tlb_employee_permissions`) is additive and fail-closed. `AuthorizationService.IsAdmin()` is `USERTYPE == "Admin"` only. `UserRoleDB` is presence-checked, not an allow/deny value.

| Layer | Change |
| --- | --- |
| Identity | Session builder extracted; Switch User reuses it without `GrantAuthenticatedSession` side effects |
| Audit | `ImpersonationAudit` IMPERSONATE / RETURN + `ORIGINAL_*` keys |
| Authorization | `AuthorizationService` + `PermissionRepository` (5-minute cache) |
| Observability | Permission Inspector, Access Analyzer, hashed snapshots |
| Overlay | Schema + SWITCH_USER dual-path canary |
| Bootstrap (#104) | SQL pack for Active Admins — **after** squash |

## Authentication changes

None to the login contract. Canonical path remains `/Login.aspx`. `GrantAuthenticatedSession` still owns `LastLogin`, `LoginStatus`, audit, `ATS_SavedID`, homepage redirect. MFA transients stay in login. Switch User must not call `GrantAuthenticatedSession`.

## Authorization changes

| PR | What |
| --- | --- |
| #89 | `CanImpersonate` = Admin USERTYPE **and** `SwitchUserAuthorizedUsers` |
| #91 | Switch User page; master chrome; nested switch blocked |
| #94 | Docs of the hybrid model |
| #95 | `CanAccess` / `IsAdmin` / `DescribeIdentity` |
| #96 | Overlay tables and repository (no employee grants in that PR) |
| #98 / #99 | Read-only Inspector / Analyzer (Admin gate) |
| #100 | Snapshot hash/compare; empty overlay ≡ #95 allow/deny |
| #102 | `SWITCH_USER` dual-path: Admin, then overlay, else CSV |
| #104 | Overlay grants for Active Admins (SQL only; after #102) |

Office Staff never receive `SWITCH_USER`. JOB360 / attendance remain module exceptions.

## Overlay changes

`scripts/create_permission_overlay.sql` creates catalog + grant tables. Canary inserts `(WorkmanSL, PermissionId)` for `SWITCH_USER`. `#104` grants `SWITCH_USER`, `USER_ADMIN`, `PAYROLL_OVERRIDE`, `EXPORT_PAYROLL` to every Active Admin after the squash.

## UAT evidence

| Gate | Evidence | Result |
| --- | --- | --- |
| Repository / CI | Stack PRs + UAT branch, 4/4 checks | **PASS** |
| Live Admin SQL | `promote_uat_admin.sql` on `J8` | **PASS** (already Admin; apply no-op) |
| VS2015 rebuild | RC / operator sign-off | **PASS** |
| IIS runtime | RC / operator sign-off | **PASS** |
| J8 login | `USERTYPE=Admin`, `WORKMAN=J8`, `RolePermissionDB=OS-HR` | **PASS** |
| Switch User | `/bussiness/production/SwitchUser.aspx` | **PASS** |
| Permission Inspector | Admin identity + SWITCH_USER telemetry | **PASS** |
| Access Analyzer | Snapshot / Compare / canary panel | **PASS** |
| Overlay deployment | `create_permission_overlay.sql` | **PASS** |
| Overlay canary | `uat_switch_user_canary.sql` | **PASS** |
| Authorization drift | Snapshot Compare unexpected = 0 | **PASS** |
| `#104` bootstrap | after squash | **deferred** (governance) |
| RC sign-off | `FINAL_SIGNOFF.md` | **GO** |

## SQL scripts executed

| Script | Environment | Result |
| --- | --- | --- |
| `scripts/promote_uat_admin.sql` | `atserp_uat` | Dry-run + apply. `J8` already `Admin` / `ATS-OS` / `OS-HR`. `LastLogin` unchanged. |
| `scripts/create_permission_overlay.sql` | UAT | Applied (overlay catalog + empty grant tables, then canary). |
| `scripts/uat_switch_user_canary.sql` | UAT | Grant / verify / rollback. Unexpected snapshot drift = 0. |
| `scripts/bootstrap_platform_admin.sql` | — | Run **after** `#104` squash, with a reviewed Active Admin roster. |

## Known exclusions

- **PR #103** — UAT orchestration only. Never merge. Archive after squash.
- **PR #86** — docs-only session audit; not in squash list.
- **Logout PR #92** — still locked (restore original admin, then existing logout).
- **Security Admin CRUD** — not started.
- **Office Staff** — not Switch User; not `#104` grants.
- **No `Employee_Type='Admin'` catalog row** — do not invent one.

## Rollback strategy

| Layer | Rollback |
| --- | --- |
| Git | Do not merge #103. Individual PRs remain the merge artifacts; revert a squash commit if needed. |
| Overlay schema | Leave tables in place (additive). Do not `DROP`. |
| Canary grant | Commented `DELETE` in `uat_switch_user_canary.sql`. Recycle IIS. |
| `#104` grants | `DELETE` four pack codes × Active Admin (`PLATFORM_ADMIN_BOOTSTRAP.md`). Recycle IIS. CSV/hardcoded paths remain. |
| Session | InProc; recycle app pool clears impersonation leftovers. |

## Final recommendation

**READY FOR SQUASH MERGE**

Outstanding items are GitHub actions only (see `MERGE_CHECKLIST.md`): squash `#89 → #102`, then `#104`; archive `#103`; create tag `v2.2-security-foundation`.
