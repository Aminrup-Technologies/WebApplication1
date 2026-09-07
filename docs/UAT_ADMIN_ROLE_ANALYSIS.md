# UAT Admin role analysis

**Mode:** Database architecture audit + idempotent SQL generator.  
**Branch:** `uat/security-foundation-v2.2`  
**Date:** 2026-09-07  
**Live UAT SQL from this agent:** not reachable (no `connections.config`, no `sqlcmd`). Schema below is reverse-engineered from application INSERT/SELECT. **Numeric Admin ids and menu counts must come from running `scripts/provision_admin_role.sql` on UAT with `@ApplyChanges = 0`.**

Do not invent `EmpType_Value` or a new `tlb_EmployeePermissions` matrix. Reuse the catalog and menu profile that existing Admins already use.

## Phase 1 — Security tables

| Table | Purpose | Evidence |
| --- | --- | --- |
| `tbl_Employee_Mustertable` | Employee master. Login reads `User_RoleType`, `UserRoleDB`, `RolePermissionDB`. | `Login.aspx.cs` `FetchEmployeeRowByLoginId` |
| `tlb_emp_roles` | Role catalog. `Employee_Type` → Session `USERTYPE`. `EmpType_Value` → Session `UserRoleDB`. | `manage_rolls.aspx.cs` INSERT |
| `tlb_emp_roles_permission` | Menu-profile catalog. `Emp_PermissionValue` → Session `RolePermissionDB`. `Emp_PermissionText` → muster `Role_Permission` (display). | `manage_rollsaccess.aspx.cs` INSERT |
| `tlb_EmployeePermissions` | Sidebar visibility (`ParentKey`, `ChildKey`, `IsVisible`) keyed by `Emp_PermissionValue`. **Cosmetic, not page ACL.** | `webmaster.Master.cs` `LoadPermissions` |
| `tlb_permissions` | Overlay catalog (PR #96). Includes `SWITCH_USER`, `USER_ADMIN`. | `scripts/create_permission_overlay.sql` |
| `tlb_permission_groups` | Overlay groups | same |
| `tlb_group_permissions` | Overlay group grants | same |
| `tlb_employee_group` | Overlay membership | same |
| `tlb_employee_permissions` | Overlay **direct** grants (`WorkmanSL`, `PermissionId`). Not the sidebar table. | same |

## Phase 2 — Schema inventory (from code, not guessed types)

Nullable / PK / exact SQL types are in Result 2 of the provisioning script (`INFORMATION_SCHEMA`). Columns the application actually reads or writes:

### `tbl_Employee_Mustertable` (identity / role assignment)

| Column | Used as | Notes |
| --- | --- | --- |
| `WorkmanSL` | Employee id / overlay key | |
| `LoginID` | Session `USERID` | |
| `User_RoleType` | Session `USERTYPE` | Must be exact `Admin` for Switch User / `IsAdmin()` |
| `UserRoleDB` | Session `UserRoleDB` | `tlb_emp_roles.EmpType_Value`; presence-checked, value not compared for allow/deny |
| `RolePermissionDB` | Session `RolePermissionDB` | `tlb_emp_roles_permission.Emp_PermissionValue`; menu lookup |
| `Role_Permission` | Display twin of `RolePermissionDB` | Not sessioned; written with the profile on register/edit |
| `WorkStatus` | Active filter | Analyzer / Switch User search |
| `LoginPassword` / `LastLogin` / `LoginStatus` | Auth lifecycle | **Must not be updated by provisioning** |

### `tlb_emp_roles`

| Column | Used as |
| --- | --- |
| `Id` | Grid key |
| `Employee_Type` | Label (`Admin`, `Office Staff`, `Site Staff`, …) |
| `EmpType_Value` | Numeric/alphanumeric id (UI max 20 chars) stored on the employee as `UserRoleDB` |
| `ViewMode` / `DeleteMode` | Soft-delete (`DeleteMode = 1`) |
| `AddedByWrk` / `TimeStamp` | Audit on insert |

### `tlb_emp_roles_permission`

| Column | Used as |
| --- | --- |
| `Id` | Grid key |
| `EmpType_Value` | FK-like filter to `tlb_emp_roles` |
| `Emp_PermissionValue` | Menu profile id → `RolePermissionDB` |
| `Emp_PermissionText` | Profile name → `Role_Permission` |
| `view_status` / `delete_status` | Soft flags on the catalog row |
| `added_on` | Displayed in manage_rollsaccess |

### `tlb_EmployeePermissions`

| Column | Used as |
| --- | --- |
| `Emp_PermissionValue` | Filter = Session `RolePermissionDB` |
| `ParentKey` | Sidebar `<li>` id (`Payroll`, `JOBManpower`, …) |
| `ChildKey` | Child control id |
| `IsVisible` | `Control.Visible` |

There is **no INSERT/UPDATE** of this table in the repo. Provisioning must **assign** an existing `Emp_PermissionValue`, not create menu rows.

### Overlay (PR #96)

| Column | Table |
| --- | --- |
| `PermissionCode`, `Id`, `IsActive` | `tlb_permissions` |
| `WorkmanSL`, `PermissionId` | `tlb_employee_permissions` (PK) |

## Phase 3 — Existing Admin analysis

Fill from script Result 3 after a dry run on UAT:

| WorkmanSL | LoginID | FullName | UserRoleDB | RolePermissionDB |
| --- | --- | --- | --- | --- |
| *(run script)* |  |  |  |  |

The script’s `DERIVED` result set picks:

1. `UserRoleDB` = `tlb_emp_roles.EmpType_Value` where `Employee_Type = 'Admin'` and `DeleteMode = 0`
2. `RolePermissionDB` = **mode** among Active employees with `User_RoleType = 'Admin'`
3. Fallback = Admin catalog profile with the highest `IsVisible` count in `tlb_EmployeePermissions`

Multiple Admin profiles are possible (same `USERTYPE`, different menus). The mode of live Admins is the compatibility choice.

## Phase 4 — Office Staff comparison

| Field | Admin | Office Staff |
| --- | --- | --- |
| `User_RoleType` | `Admin` | `Office Staff` |
| `UserRoleDB` | Admin `EmpType_Value` (live) | Office Staff `EmpType_Value` (live) |
| `RolePermissionDB` | Often a wide menu profile (data) | Any profile bound to that type; **not** what Switch User checks |
| Switch User | Yes only if also Admin + allowlist/overlay | **Never** (`USERTYPE` gate) |
| JOB360 / attendance chrome | Yes | Yes (module exception) |

What must change for a chosen WorkmanSL to become a platform Admin: **`User_RoleType` → Admin**, **`UserRoleDB` → existing Admin `EmpType_Value`**, **`RolePermissionDB` → existing full-access profile**. Overlay grants are additive and optional.

## Phase 5 — Full-access menu profile

Fill from script Result 5:

| RolePermissionDB | VisibleMenuCount |
| --- | --- |
| *(run script)* |  |

Full-access = the `Emp_PermissionValue` with the **highest visible child count**, preferring the one already used by Active Admins.

## Phase 6 — Role mapping

Fill from script Result 6. Do not guess ids.

| EmpType_Value | Employee_Type | Security Foundation |
| --- | --- | --- |
| *(live)* | `Admin` | `USERTYPE` gate |
| *(live)* | `Office Staff` | JOB360/attendance only |
| *(live)* | `Site Staff` | JOB create routing |
| *(live)* | other labels | not privilege unless a page compares the string |

## Phase 7 — Overlay readiness

| Permission | Exists (catalog) |
| --- | --- |
| `SWITCH_USER` | Seeded by `scripts/create_permission_overlay.sql` if that script was applied |
| `USER_ADMIN` | Same |

Do not insert catalog rows from the Admin provisioner. Grant employee overlay rows only if the tables and catalog codes exist.

## Phase 8 — Provisioning script

`scripts/provision_admin_role.sql`

1. Run with `@ApplyChanges = 0` (default). Capture results 1–8 and `DERIVED`.
2. Confirm `@WorkmanSL` (default `J8`) exists and is the intended UAT admin.
3. Confirm `SwitchUserAuthorizedUsers` in `Web.config` still lists that WorkmanSL (config, not SQL).
4. Set `@ApplyChanges = 1` and re-run. Duplicate execution is a no-op (`UPDATE` predicates + `NOT EXISTS` overlay inserts).
5. Recycle IIS or wait 5 minutes if overlay grants were added (`PermissionRepository` cache).

If `Employee_Type = 'Admin'` is missing from `tlb_emp_roles`, the script **stops**. It will not invent `EmpType_Value` unless you set `@CreateMissingAdminRole = 1` and supply `@NewAdminEmpTypeValue` taken from an agreed catalog id.

## Phase 9 — Verification

The same file prints `BEFORE`, `DERIVED`, `AFTER`, `AFTER-MENU`, `AFTER-OVERLAY`.

| Check | Expected |
| --- | --- |
| `User_RoleType` | `Admin` |
| `UserRoleDB` | Derived Admin `EmpType_Value` |
| `RolePermissionDB` | Derived full-access profile |
| Overlay `USER_ADMIN` | Granted if overlay tables exist |
| Overlay `SWITCH_USER` | Granted if overlay tables exist |
| Duplicate execution | No extra overlay rows; no password/login mutation |
| Login/session model | Unchanged (`ApplySessionFromEmployeeRow` still copies the three DB columns) |

## Success / limits

This agent could not fill live Admin WorkmanSL / menu-count tables. UAT operators must paste Results 3–7 into this document after the dry run.

After apply, `J8` (or the chosen WorkmanSL) satisfies `AuthorizationService.IsAdmin()`, receives the same sidebar profile as existing Admins, and is overlay-ready for the SWITCH_USER canary without a parallel role model.
