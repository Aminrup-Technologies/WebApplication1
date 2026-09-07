# PR B — Permission overlay infrastructure

**SQL:** `scripts/create_permission_overlay.sql`  
**Repository:** `WebApplication1/App_Code/PermissionRepository.cs`  
**Effective engine:** `WebApplication1/App_Code/AuthorizationService.cs` (overlay stub replaced)  
**Pages modified:** none  
**Callers in production:** none (pages still use Session / `ImpersonationAudit` / hardcoded WorkmanSL)

This PR is infrastructure, not Security Admin UI. Tables, repository, cache, and `CanAccess` wiring exist so PR C can call an API instead of writing SQL.

## Additive schema only

No ALTER of `tbl_Employee_Mustertable`, `tlb_emp_roles`, `tlb_emp_roles_permission`, or `tlb_EmployeePermissions`.

| New table | Purpose |
| --- | --- |
| `tlb_permissions` | Permission catalog (`PermissionCode` unique) |
| `tlb_permission_groups` | Named groups (logical roles) |
| `tlb_group_permissions` | Group → permission |
| `tlb_employee_group` | WorkmanSL → group |
| `tlb_employee_permissions` | WorkmanSL → permission (direct override) |

**Name collision:** `tlb_employee_permissions` (this overlay) is not `tlb_EmployeePermissions` (legacy sidebar matrix keyed by `Emp_PermissionValue`). Master `LoadPermissions` is unchanged.

Seed catalog only (no group rows, no employee grants):

| PermissionCode | Module |
| --- | --- |
| `SWITCH_USER` | Admin |
| `PAYROLL_OVERRIDE` | Payroll |
| `JOB360_OVERRIDE` | JOB360 |
| `ATTENDANCE_OVERRIDE` | Attendance |
| `EXPORT_PAYROLL` | Payroll |
| `USER_ADMIN` | Admin |

Until a DBA runs the script, `PermissionRepository` fail-closes: `SqlException` → empty grant set, cached 5 minutes. Production gates stay on Session / USERTYPE / config / hardcoded lists.

## Repository

`PermissionRepository` talks only to overlay tables via `DbConn`.

| Method | Responsibility |
| --- | --- |
| `GetActivePermissions()` | Load active catalog |
| `GetGroupCodes(workman)` | Resolve group membership |
| `GetDirectPermissionCodes(workman)` | Resolve direct grants |
| `GetGroupPermissionCodes(workman)` | Resolve grants inherited from groups |
| `GetPermissionCodes(workman)` | Union (direct wins over group for source tag) |
| `HasPermission` / `TryGetSource` | Lookup one code + `DIRECT` or `GROUP` |
| `Invalidate` / `InvalidateAll` | Drop one WorkmanSL snapshot, or bump cache generation |

Cache: `HttpRuntime.Cache`, 5-minute absolute TTL, generation key `perm-overlay-ver` so later admin writes can invalidate without an external cache. Empty miss results are cached so a missing-table environment does not hit SQL on every WebForms postback.

## Effective permission order

`AuthorizationService.CanAccess` / `Describe`:

1. Session (`IsAuthenticated` — five keys).
2. Legacy module authority (`CanImpersonate` for `SWITCH_USER`; Admin **or** Office Staff for JOB360 / attendance).
3. Overlay DB (direct / group).
4. Config CSV (`SwitchUserAuthorizedUsers`, `PayrollAuthorizedUsers`).
5. Hardcoded WorkmanSL (`J8`, `A84`, `K208`, `N21`).

`IsAdmin()` remains `USERTYPE == "Admin"` only. Overlay does not turn Admin into a blanket superuser.

**`SWITCH_USER` overlay rule:** overlay grant is usable only when `IsAdmin()` and not impersonating. Office Staff cannot impersonate via overlay. Empty seed ⇒ same as PR A (`CanImpersonate` only).

**`USER_ADMIN`:** still false until a later CR assigns overlay rows. No page reads it.

## Authorization regression matrix

Every row is **Same** because (a) no page was migrated and (b) no employee overlay grants are seeded. If the SQL script has not been applied, overlay lookups fail closed to empty.

| Legacy scenario | Legacy predicate | `AuthorizationService` | New result |
| --- | --- | --- | --- |
| Admin login | Five Session keys | `IsAuthenticated()` true | Same |
| Office Staff login | `USERTYPE=Office Staff` | `IsAuthenticated()` true; `IsAdmin()` false | Same |
| Missing Session | Master / homepage redirect | all `CanAccess` false | Same |
| Switch User (Admin + allowlist, not impersonating) | `CanImpersonate` | `CanAccess("SWITCH_USER")` true via step 2 | Same |
| Switch User (allowlisted Office Staff) | `CanImpersonate` false | false (overlay ignored without Admin) | Same |
| Switch User (Admin, not on CSV, no overlay row) | `CanImpersonate` false | false | Same |
| Switch User (already impersonating) | `CanImpersonate` false | false | Same |
| Payroll tab | `PayrollAuthorizedUsers` | `CanAccess("PAYROLL_OVERRIDE")` via config | Same |
| JOB360 / attendance chrome | Admin **or** Office Staff | `CanAccess` via module authority | Same |
| Payroll dashboard extra chrome | `WORKMAN == J8` | `EXPORT_PAYROLL` / `LEGACY_PAYROLL_DASHBOARD` | Same |
| Attach manpower / expense heads | hardcoded WorkmanSL | `LEGACY_*` codes | Same |
| Role catalog pages | Session presence only | `CanAccess("USER_ADMIN")` false | Same |
| Menu visibility | `tlb_EmployeePermissions` | not wrapped | Same |

## Migration KPI (this PR)

| Metric | Count |
| --- | --- |
| Direct Session checks | unchanged (~100 pages + master) |
| `AuthorizationService` callers | **0** pages |
| Hardcoded allowlists | **7** live (same as PR A) |
| Overlay-backed assignments | **0** (catalog seeded, no grants) |

## What this PR does not do

- No `.aspx` / `.Master` / `Login.aspx.cs` / `SwitchUser.aspx.cs` / `ImpersonationAudit.cs` edits
- No Security Admin UI (PR C)
- No retirement of config CSVs or hardcoded WorkmanSL (PR D)
- No page `CanAccess` migrations (PR E)
- No DROP / ALTER of existing tables

## How to verify

1. `scripts/create_permission_overlay.sql` creates five overlay tables and six catalog rows; no INSERTs into `tlb_employee_group` or `tlb_employee_permissions`.
2. `WebApplication1.csproj` compiles `App_Code\PermissionRepository.cs`.
3. `CanAccess` order is Session → `MatchesLegacyModuleAuthority` → overlay → config → hardcoded.
4. `HasOverlayPermission` calls `PermissionRepository.TryGetSource(WORKMAN, code)`.
5. `SWITCH_USER` overlay requires `IsAdmin()` and not impersonating.
6. `git diff` contains no page / master / login / Switch User / impersonation-audit changes.
7. Missing overlay tables do not throw to the request: `SqlException` is swallowed to an empty snapshot.
