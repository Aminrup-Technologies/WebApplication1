# UAT Admin role analysis

**Mode:** Database architecture audit + idempotent SQL generator.  
**Branch:** `uat/security-foundation-v2.2`  
**Date:** 2026-09-07  
**Live UAT SQL:** queried `atserp_uat` on 2026-09-07. `scripts/promote_uat_admin.sql` dry-run and apply both ran against `J8`. Apply was a no-op: `J8` was already `Admin` / `ATS-OS` / `OS-HR`. Credentials are not stored in this repo.

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
| `tlb_permissions` | Overlay catalog (PR #96). Includes `SWITCH_USER`, `USER_ADMIN`. | **MISSING on UAT** — run `scripts/create_permission_overlay.sql` before the #102 canary INSERT |
| `tlb_permission_groups` | Overlay groups | **MISSING on UAT** |
| `tlb_group_permissions` | Overlay group ↔ permission | **MISSING on UAT** |
| `tlb_employee_group` | Overlay membership | **MISSING on UAT** |
| `tlb_employee_permissions` | Overlay **direct** grants (`WorkmanSL`, `PermissionId`). Not the sidebar table. | **MISSING on UAT** |

## Phase 2 — Schema inventory (from code, not guessed types)

Nullable / PK / exact SQL types are in Result 2 of the provisioning script (`INFORMATION_SCHEMA`). Columns the application actually reads or writes:

### `tbl_Employee_Mustertable` (identity / role assignment)

| Column | Used as | Notes |
| --- | --- | --- |
| `WorkmanSL` | `varchar(50)` NOT NULL | Employee id / overlay key |
| `LoginID` | `varchar(50)` NOT NULL | Session `USERID` |
| `FullName` | `varchar(100)` NOT NULL | Session `USERNAME` |
| `User_RoleType` | `varchar(50)` NULL | Session `USERTYPE`. Must be exact `Admin` for Switch User / `IsAdmin()` |
| `UserRoleDB` | `varchar(50)` NULL | `tlb_emp_roles.EmpType_Value`; presence-checked, value not compared for allow/deny |
| `RolePermissionDB` | `varchar(50)` NULL | `tlb_emp_roles_permission.Emp_PermissionValue`; menu lookup |
| `Role_Permission` | `varchar(50)` NULL | Display twin of `RolePermissionDB`; not sessioned |
| `WorkStatus` | `varchar(50)` NULL | Active filter |
| `LoginPassword` | `varchar(50)` NOT NULL | **Must not be updated** |
| `LastLogin` / `LastLogout` | `smalldatetime` NULL | **Must not be updated** |
| `LoginStatus` | `int` NULL | **Must not be updated** |
| `PasswordExpiry` | `smalldatetime` NULL | **Must not be updated** |

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

## Phase 3 — Existing Admin analysis (live)

Only one Active Admin exists. Catalog `Employee_Type='Admin'` is still absent; `J8` carries the Admin **string** on `User_RoleType` while keeping the Office Staff catalog id `ATS-OS`.

| WorkmanSL | LoginID | FullName | User_RoleType | UserRoleDB | RolePermissionDB | Role_Permission |
| --- | --- | --- | --- | --- | --- | --- |
| J8 | ATS002112 | ANUPAM SHARMA | Admin | ATS-OS | OS-HR | OS-HR |

## Phase 4 — Office Staff comparison

| Field | Admin (`J8`) | Office Staff (Active) |
| --- | --- | --- |
| `User_RoleType` | `Admin` (1 person) | `Office Staff` (37 people) |
| `UserRoleDB` | `ATS-OS` | `ATS-OS` (same catalog id) |
| `RolePermissionDB` | `OS-HR` | Mixed: SS-SA 18, OS-HR 11, OS-BL 4, SS-WK 2, OS-GP 1, SS-SS 1 |
| Switch User | Yes if also on `SwitchUserAuthorizedUsers` / overlay | **Never** (`USERTYPE` gate) |
| JOB360 / attendance chrome | Yes | Yes (module exception) |

`UserRoleDB` does **not** distinguish Admin from Office Staff on this UAT. Both use `ATS-OS`. Privilege is `User_RoleType`.

`J8` already matched the target row. Promotion apply updated **0 columns**. Overlay grants stay in `scripts/uat_switch_user_canary.sql` after `create_permission_overlay.sql`.

## Phase 5 — Full-access menu profile

| RolePermissionDB | MenuRowCount | VisibleMenuCount |
| --- | --- | --- |
| **OS-HR** | 31 | **31** |
| MT-AL | 31 | 30 |
| SS-SA | 31 | 16 |
| SS-SI | 31 | 16 |
| SS-SP | 31 | 16 |
| OS-BL | 31 | 13 |
| SS-SO | 31 | 13 |
| OS-SA | 31 | 12 |
| OS-SS | 31 | 9 |
| SS-SS | 31 | 9 |
| SS-SK | 31 | 8 |
| SS-WK | 31 | 5 |

Full-access on this UAT is **`OS-HR` (31 visible of 31)**. `J8` already uses it.

## Phase 6 — Role mapping

Fill from script Result 6. Do not guess ids.

| EmpType_Value | Employee_Type | Security Foundation |
| --- | --- | --- |
| *(absent)* | `Admin` | **No catalog row.** `J8.User_RoleType` is still the string `Admin`. |
| ATS-OS | Office Staff | JOB360/attendance; also the `UserRoleDB` on `J8` |
| ATS-SS | Site Staff | JOB create routing |
| ATS-MT | Management | label only unless a page compares the string |

## Phase 7 — Overlay readiness

| Permission | Exists (catalog) |
| --- | --- |
| `SWITCH_USER` | **NO** — overlay tables are not on `atserp_uat` |
| `USER_ADMIN` | **NO** — same |

Do not insert catalog rows from the Admin provisioner. Run `scripts/create_permission_overlay.sql` on UAT before `scripts/uat_switch_user_canary.sql`. Switch User for `J8` still works via `USERTYPE=Admin` + `SwitchUserAuthorizedUsers`.

## Phase 8 — Provisioning script

**Apply path:** `scripts/promote_uat_admin.sql`

`scripts/provision_admin_role.sql` is discovery-only on this UAT. It will stop (or invent, if forced) because there is no `Employee_Type='Admin'` catalog row. Do not run it with `@ApplyChanges = 1`.

1. Run `promote_uat_admin.sql` with `@ApplyChanges = 0` (default). Review table/column checks, BEFORE, OS-HR menu count, PLANNED.
2. Confirm `@WorkmanSL` is `J8` (or the intended UAT operator) and `WorkStatus = Active`.
3. Confirm live `Web.config` `SwitchUserAuthorizedUsers` still lists that WorkmanSL (`Web.config.example` has `J8`).
4. Set `@ApplyChanges = 1` and re-run. Second run is a no-op.
5. Recycle IIS (or log out/in) so Session `USERTYPE` is rebuilt from `User_RoleType`.
6. Overlay canary is blocked until `scripts/create_permission_overlay.sql` is applied on UAT.

**Live run (2026-09-07):** dry-run then apply against `J8`. Apply mode `APPLY`. `LastLogin` stayed `2026-09-07 16:56:00`. Zero columns changed.

## Phase 9 — Verification (live AFTER)

| Check | Expected | Live `J8` |
| --- | --- | --- |
| `User_RoleType` | `Admin` | **PASS** `Admin` |
| `UserRoleDB` | `ATS-OS` | **PASS** `ATS-OS` |
| `RolePermissionDB` | `OS-HR` | **PASS** `OS-HR` |
| `Role_Permission` | `OS-HR` | **PASS** `OS-HR` |
| Overlay grants | skipped if tables missing | **PASS** tables missing; not written |
| Duplicate execution | no extra rows; login columns unchanged | **PASS** `LastLogin`/`LoginStatus` unchanged |
| Login/session model | unchanged | **PASS** |

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

Live SQL for `J8` is **finalized** (already Admin; apply was a no-op). Remaining IIS work:

1. Confirm live `Web.config` still has `SwitchUserAuthorizedUsers=J8`.
2. Log out/in (or recycle IIS) so Session matches the DB if any older session still has a pre-Admin `USERTYPE`.
3. Run `scripts/create_permission_overlay.sql` on UAT before the #102 overlay canary INSERT. Overlay tables are currently **missing**.
4. Do not re-save `J8` from `emp_registration` while `tlb_emp_roles` has no `Admin` dropdown row.

### Success criteria (static + live UAT)

| Check | Required | Result |
| --- | --- | --- |
| Every referenced column exists | ✓ | PASS (INFORMATION_SCHEMA) |
| Every referenced table exists | ✓ | PASS (legacy four); overlay five MISSING |
| No invented Admin role ID | ✓ | PASS |
| ATS-OS preserved | ✓ | PASS live |
| OS-HR preserved | ✓ | PASS live (31/31) |
| AuthorizationService compatibility | ✓ | PASS |
| Idempotent | ✓ | PASS (`LastLogin` unchanged) |
| Dry-run supported | ✓ | PASS |
| No login/session regression | ✓ | PASS |
