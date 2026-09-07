# UAT Admin role analysis

**Mode:** Database architecture audit + idempotent SQL generator.  
**Branch:** `uat/security-foundation-v2.2`  
**Date:** 2026-09-07  
**Live UAT SQL from this agent:** not reachable (no `connections.config`, no `sqlcmd`). Schema is reverse-engineered from application INSERT/SELECT. **UAT operator facts below are treated as verified.**

Do not invent `EmpType_Value` or a new `tlb_EmployeePermissions` matrix.

### UAT discovery facts (authoritative)

| Fact | Value |
| --- | --- |
| `Employee_Type = 'Admin'` in `tlb_emp_roles` | **Absent.** Do not insert one. |
| Operational `UserRoleDB` | `ATS-OS` (preserve) |
| Full-access `RolePermissionDB` | `OS-HR` (preserve; 31 visible menus) |
| `AuthorizationService.IsAdmin()` | `Session USERTYPE` from `User_RoleType` only |
| `UserRoleDB` | Presence-checked at login; **not** a runtime allow/deny value |
| Apply script | `scripts/promote_uat_admin.sql` (not `provision_admin_role.sql`) |

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

What must change for `J8` on this UAT: **`User_RoleType` → `Admin`**, **`Role_Permission` → copy of `RolePermissionDB` (`OS-HR`)**. Preserve `UserRoleDB = ATS-OS` and `RolePermissionDB = OS-HR`. Do not invent an Admin catalog id. Overlay grants stay in `scripts/uat_switch_user_canary.sql`, not this promotion.

## Phase 5 — Full-access menu profile

Fill from script Result 5:

| RolePermissionDB | VisibleMenuCount |
| --- | --- |
| *(run script)* |  |

Full-access on this UAT is **`OS-HR` (31 visible menus)**. Do not assign a different profile.

## Phase 6 — Role mapping

Fill from script Result 6. Do not guess ids.

| EmpType_Value | Employee_Type | Security Foundation |
| --- | --- | --- |
| *(absent)* | `Admin` | **No catalog row.** `USERTYPE` is still the string `Admin` on the employee row. |
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

**Apply path:** `scripts/promote_uat_admin.sql`

`scripts/provision_admin_role.sql` is discovery-only on this UAT. It will stop (or invent, if forced) because there is no `Employee_Type='Admin'` catalog row. Do not run it with `@ApplyChanges = 1`.

1. Run `promote_uat_admin.sql` with `@ApplyChanges = 0` (default). Review table/column checks, BEFORE, OS-HR menu count, PLANNED.
2. Confirm `@WorkmanSL` is `J8` (or the intended UAT operator) and `WorkStatus = Active`.
3. Confirm live `Web.config` `SwitchUserAuthorizedUsers` still lists that WorkmanSL (`Web.config.example` has `J8`).
4. Set `@ApplyChanges = 1` and re-run. Second run is a no-op.
5. Recycle IIS (or log out/in) so Session `USERTYPE` is rebuilt from `User_RoleType`.
6. Execute the PR #102 canary (`scripts/uat_switch_user_canary.sql`) separately. This promotion does not write overlay rows.

## Phase 9 — Verification

`promote_uat_admin.sql` prints BEFORE, PLANNED, AFTER, VALIDATION.

| Check | Expected |
| --- | --- |
| `User_RoleType` | `Admin` |
| `UserRoleDB` | `ATS-OS` (unchanged) |
| `RolePermissionDB` | `OS-HR` (unchanged) |
| `Role_Permission` | `OS-HR` |
| Overlay grants | Unchanged (not written here) |
| Duplicate execution | 0 extra rows; login columns unchanged |
| Login/session model | Unchanged (`ApplySessionFromEmployeeRow` still copies the three DB columns) |

## Generated Script Verification

**Script:** `scripts/promote_uat_admin.sql`  
**Verified:** 2026-09-07 against `uat/security-foundation-v2.2`  
**Ready for dry-run execution:** **PASS** (all static gates below passed)

### Table verification

| Table | Exists | Evidence |
| --- | --- | --- |
| `tbl_Employee_Mustertable` | PASS | `Login.aspx.cs` `FetchEmployeeRowByLoginId` |
| `tlb_emp_roles` | PASS | `manage_rolls.aspx.cs` INSERT/SELECT |
| `tlb_emp_roles_permission` | PASS | `manage_rollsaccess.aspx.cs` INSERT (existence-checked only; not written) |
| `tlb_EmployeePermissions` | PASS | `webmaster.Master.cs` `LoadPermissions` |

No other base tables are referenced. `#TableCheck` / `#ColumnCheck` are session temp objects.

### Column verification (`tbl_Employee_Mustertable`)

| Column | Exists | Evidence | Script use |
| --- | --- | --- | --- |
| `User_RoleType` | PASS | `Login.aspx.cs` 149, 799 | SET to `Admin` |
| `UserRoleDB` | PASS | `Login.aspx.cs` 149, 800 | Read/guard `ATS-OS`; not SET |
| `RolePermissionDB` | PASS | `Login.aspx.cs` 149, 801 | Read/guard `OS-HR`; not SET |
| `Role_Permission` | PASS | `emp_registration.aspx.cs` 495; `view_emp_mastertbldata.aspx.cs` 55 | SET equal to `RolePermissionDB` |
| `WorkStatus` | PASS | `Login.aspx.cs` 148; Switch User search | Guard `Active` |
| `LoginStatus` | PASS | `GrantAuthenticatedSession` UPDATE | Selected; not SET |
| `LastLogin` | PASS | `GrantAuthenticatedSession` UPDATE | Selected; not SET |
| `LastLogout` | PASS | `webmaster.Master.cs` 228 | Selected; not SET |
| `PasswordExpiry` | PASS | `Login.aspx.cs` 148 | Selected; not SET |
| `LoginPassword` | PASS | `Login.aspx.cs` 148, 221 | Presence only; not SET |
| `WorkmanSL` / `LoginID` / `FullName` | PASS | `FetchEmployeeRowByLoginId` | Identity output |

MFA columns (`MFAEnabled`, `MFAMethod`, …) are **not referenced**. Login treats them as optional (`IsMissingColumnException`). They are not in the SET list.

`tlb_emp_roles.DeleteMode` is written by `manage_rolls.aspx.cs` 52 / 166. `tlb_EmployeePermissions.Emp_PermissionValue` / `IsVisible` are read by `webmaster.Master.cs`.

### Repository compatibility

| Check | Result |
| --- | --- |
| `Login.aspx.cs` `FetchEmployeeRowByLoginId` | PASS — still selects `User_RoleType`, `UserRoleDB`, `RolePermissionDB`. Script does not change the SELECT or password path. |
| `ApplySessionFromEmployeeRow` | PASS — copies `User_RoleType` → `USERTYPE`, `UserRoleDB`, `RolePermissionDB`. `Role_Permission` is not sessioned. After re-login, `USERTYPE` becomes `Admin`; menu key stays `OS-HR`. |
| `GrantAuthenticatedSession` | PASS — still owns `LastLogin` / `LoginStatus` / audit / `ATS_SavedID`. Script does not SET those columns. |
| `AuthorizationService.IsAdmin()` | PASS — `USERTYPE == "Admin"` only (`AuthorizationService.cs` 79–82). Independent of `UserRoleDB`. |
| `ImpersonationAudit.CanImpersonate()` | PASS — Admin USERTYPE **and** `SwitchUserAuthorizedUsers`. `Web.config.example` lists `J8`. Does not read `UserRoleDB`. |
| Invented Admin `EmpType_Value` | PASS — none. No INSERT into `tlb_emp_roles`. |
| `ATS-OS` preserved | PASS — apply guard requires `UserRoleDB = ATS-OS`. |
| `OS-HR` preserved | PASS — apply guard requires `RolePermissionDB = OS-HR`. |
| Idempotent | PASS — UPDATE only when `User_RoleType` or `Role_Permission` differs. |
| Dry-run | PASS — `@ApplyChanges = 0` default; RETURN before UPDATE. |
| Destructive SQL | PASS — no DROP/DELETE of base tables; temp `#` tables only. |

### Security Foundation PR compatibility

| PR | Compatibility |
| --- | --- |
| #89 Impersonation audit | PASS — `CanImpersonate` still Admin + CSV. Promotion supplies the Admin string. |
| #91 Switch User | PASS — `ApplySessionFromEmployeeRow` unchanged; Switch User still does not call `GrantAuthenticatedSession`. |
| #94 Role/permission audit | PASS — hybrid model unchanged: USERTYPE is privilege; `RolePermissionDB` is sidebar; `UserRoleDB` is presence. |
| #95 AuthorizationService | PASS — `IsAdmin()` remains USERTYPE-only. |
| #96 Overlay tables | PASS — this script does not INSERT overlay grants. Catalog may exist; unused here. |
| #98 Permission Inspector | PASS — page gate is `IsAdmin()`. J8 can open it after re-login. `DescribeIdentity` uses `User_RoleType` from muster. |
| #99 Access Analyzer | PASS — same Admin gate. Scan will show J8 as Admin with `OS-HR` menus. |
| #100 Snapshot / migration bridge | PASS — payload still hashes identity + effective access. Promoting J8 **will** change J8’s `User_RoleType` and likely `SWITCH_USER` Granted vs a pre-promotion snapshot. Take the pre-canary snapshot **after** this promotion. Frozen #100 baseline is still the empty-overlay code baseline. |
| #102 SWITCH_USER canary | PASS — dual-path unchanged. After promotion + re-login, `CanAccess("SWITCH_USER")` is true for `J8` via `USERTYPE+LEGACY_CONFIG` if live `SwitchUserAuthorizedUsers` still contains `J8`. Overlay INSERT remains a separate canary step. |

### Runtime expectations (after apply + re-login)

| Runtime | Expected |
| --- | --- |
| `Session["USERTYPE"]` | `Admin` |
| `AuthorizationService.IsAdmin()` | `true` |
| `CanAccess("SWITCH_USER")` | `true` (Admin + `SwitchUserAuthorizedUsers`=`J8`; overlay not required) |
| `ImpersonationAudit.CanImpersonate()` | `true` (same two gates) |
| Session `UserRoleDB` | `ATS-OS` |
| Session `RolePermissionDB` | `OS-HR` (31 visible sidebar rows) |
| Password / LastLogin / LoginStatus / LastLogout / PasswordExpiry / MFA | Unchanged by this script |

### Remaining manual step

1. Run dry-run (`@ApplyChanges = 0`). Confirm table/column EXISTS, target is Active, `UserRoleDB=ATS-OS`, `RolePermissionDB=OS-HR`, OS-HR visible count is 31.
2. Review BEFORE vs PLANNED.
3. Set `@ApplyChanges = 1` and re-run.
4. Recycle IIS or log out/in.
5. Confirm live `Web.config` still has `SwitchUserAuthorizedUsers=J8`.
6. Execute PR #102 canary (`scripts/uat_switch_user_canary.sql`) on a **different** Admin who is not on the CSV if you need a clean overlay EffectiveAccess delta of 1.

Do not re-save `J8` from `emp_registration` / employee edit while `tlb_emp_roles` has no `Admin` dropdown row — that UI would overwrite `User_RoleType` with the selected catalog label.

### Success criteria (static)

| Check | Required | Result |
| --- | --- | --- |
| Every referenced column exists | ✓ | PASS |
| Every referenced table exists | ✓ | PASS |
| No invented Admin role ID | ✓ | PASS |
| ATS-OS preserved | ✓ | PASS |
| OS-HR preserved | ✓ | PASS |
| AuthorizationService compatibility | ✓ | PASS |
| Idempotent | ✓ | PASS |
| Dry-run supported | ✓ | PASS |
| No login/session regression | ✓ | PASS |
