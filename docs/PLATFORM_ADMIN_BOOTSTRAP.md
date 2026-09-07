# Platform Admin bootstrap (overlay permission pack)

**PR:** #104  
**Branch:** `feature/platform-admin-bootstrap`  
**Date:** 2026-09-07  
**Depends on:** overlay tables from PR #96 (`scripts/create_permission_overlay.sql`) and AuthorizationService from PR #95 / #102. Land **after** the production squash `#89 → #102`.

## Purpose

Give every **Active** employee with `User_RoleType = 'Admin'` a complete overlay pack so Inspector/Analyzer and future Security Admin work see the same four codes without inventing a new role id, menu profile, or Session model.

Pack codes:

| Code | Legacy path (unchanged) | After bootstrap |
| --- | --- | --- |
| `SWITCH_USER` | Admin + `SwitchUserAuthorizedUsers` (dual-path canary) | Overlay Direct **also** true for every Active Admin |
| `USER_ADMIN` | None (overlay only) | Overlay Direct for every Active Admin |
| `PAYROLL_OVERRIDE` | `PayrollAuthorizedUsers` CSV / overlay | Overlay Direct for every Active Admin |
| `EXPORT_PAYROLL` | Hardcoded WorkmanSL `J8` / overlay | Overlay Direct for every Active Admin |

`AuthorizationService` is **not** modified. Login, Session, master, CRUD UI, passwords, `LoginStatus`, `LastLogin`, `UserRoleDB`, and `RolePermissionDB` are **not** modified. Office Staff are **not** granted.

This expands overlay (and therefore `CanAccess`) for Active Admins who were not already on CSV/hardcoded lists. That is the point of the pack. Dual-path `SWITCH_USER` still requires `IsAdmin()` first.

## Schema touched

| Object | Operation |
| --- | --- |
| `tlb_permissions` | `INSERT` missing catalog rows for the four codes (`NOT EXISTS`) |
| `tlb_employee_permissions` | `INSERT` `(WorkmanSL, PermissionId)` for Active Admins (`NOT EXISTS`) |
| `tbl_Employee_Mustertable` | **SELECT only** (`WorkStatus`, `User_RoleType`, `WorkmanSL`) |

Not touched: `tlb_emp_roles`, `tlb_emp_roles_permission`, `tlb_EmployeePermissions` (sidebar), passwords, login columns, `UserRoleDB`, `RolePermissionDB`.

Prerequisite: `tlb_permissions` and `tlb_employee_permissions` must exist. If missing, the bootstrap `RAISERROR`s.

## Scripts

| File | Role |
| --- | --- |
| `scripts/bootstrap_platform_admin.sql` | Dry-run default (`@ApplyChanges = 0`). Catalog ensure + planned grants. Apply with `@ApplyChanges = 1`. |
| `scripts/bootstrap_platform_admin_verify.sql` | Read-only: Admin count, grants, missing, duplicates, orphans. |

Idempotency: second apply inserts **zero** rows. PK is `(WorkmanSL, PermissionId)`.

The last result set is always `BOOTSTRAP_SUMMARY` (`ResultSet = BOOTSTRAP_SUMMARY`). Nothing is logged to a table.

## BOOTSTRAP_SUMMARY

| Column | Meaning |
| --- | --- |
| `ActiveAdminCount` | Active `User_RoleType='Admin'` |
| `CatalogCodesExpected` | 4 (pack size) |
| `CatalogCodesPresent` | Pack codes found in `tlb_permissions` after ensure |
| `CatalogCreated` | Catalog rows inserted this run (`NOT EXISTS`) |
| `GrantsExpected` | `ActiveAdminCount × 4` |
| `GrantsInserted` | Employee overlay rows inserted this run (`0` on dry-run) |
| `GrantsSkippedExisting` | Pack grants that already existed before this run |
| `DuplicateGrantCount` | Duplicate `(WorkmanSL, PermissionId)` groups (PK ⇒ 0) |
| `MissingGrantCount` | Pack slots still absent after this run (dry-run = planned; apply must be 0) |
| `ExecutionMode` | `DRY_RUN` or `APPLY` |
| `Verdict` | `PASS` or `FAIL` |

### PASS criteria

| Check | Required |
| --- | --- |
| `CatalogCodesPresent` | = `CatalogCodesExpected` (4) |
| `DuplicateGrantCount` | = 0 |
| Dry-run `GrantsInserted` | = 0 |
| First apply `GrantsInserted` | = missing slots only (`GrantsExpected − GrantsSkippedExisting`) |
| Apply `MissingGrantCount` | = 0 |
| Second apply `GrantsInserted` | = 0 |
| Second apply `MissingGrantCount` | = 0 |

### Sample — first apply (empty overlay, one UAT Admin `J8`)

| Column | Value |
| --- | --- |
| ActiveAdminCount | 1 |
| CatalogCodesExpected | 4 |
| CatalogCodesPresent | 4 |
| CatalogCreated | 0 or 4 (0 if #96 already seeded) |
| GrantsExpected | 4 |
| GrantsInserted | 4 |
| GrantsSkippedExisting | 0 |
| DuplicateGrantCount | 0 |
| MissingGrantCount | 0 |
| ExecutionMode | APPLY |
| Verdict | PASS |

If `J8` already has `SWITCH_USER` from the canary: `GrantsInserted=3`, `GrantsSkippedExisting=1`, still PASS.

### Sample — second apply (idempotent)

| Column | Value |
| --- | --- |
| ActiveAdminCount | 1 |
| CatalogCodesExpected | 4 |
| CatalogCodesPresent | 4 |
| CatalogCreated | 0 |
| GrantsExpected | 4 |
| GrantsInserted | 0 |
| GrantsSkippedExisting | 4 |
| DuplicateGrantCount | 0 |
| MissingGrantCount | 0 |
| ExecutionMode | APPLY |
| Verdict | PASS |

Dry-run of the same state: `ExecutionMode=DRY_RUN`, `GrantsInserted=0`, `MissingGrantCount=0`, `GrantsSkippedExisting=4`, Verdict PASS.

## Before / After queries

### Before (Active Admins)

```sql
SELECT WorkmanSL, LoginID, FullName, User_RoleType, UserRoleDB, RolePermissionDB, WorkStatus
FROM dbo.tbl_Employee_Mustertable
WHERE WorkStatus = N'Active' AND User_RoleType = N'Admin';
```

UAT (`atserp_uat`, 2026-09-07): one row — `J8` / `ATS002112` / ANUPAM SHARMA / `ATS-OS` / `OS-HR`.

### Before (missing overlay grants)

```sql
-- or run bootstrap_platform_admin.sql with @ApplyChanges = 0
```

### After

```sql
-- scripts/bootstrap_platform_admin_verify.sql
-- MissingGrantCount = 0
-- DuplicateGrantGroupCount = 0
```

Expected after apply for each Active Admin: four `GRANTED` rows (`SWITCH_USER`, `USER_ADMIN`, `PAYROLL_OVERRIDE`, `EXPORT_PAYROLL`). Muster identity columns unchanged.

## Rollback

Deletes **only** overlay rows for the four pack codes on Active Admins. Does not drop catalog rows. Does not touch muster.

```sql
DELETE ep
FROM dbo.tlb_employee_permissions ep
INNER JOIN dbo.tlb_permissions p ON p.Id = ep.PermissionId
INNER JOIN dbo.tbl_Employee_Mustertable e ON e.WorkmanSL = ep.WorkmanSL
WHERE e.WorkStatus = N'Active'
  AND e.User_RoleType = N'Admin'
  AND p.PermissionCode IN (N'SWITCH_USER', N'USER_ADMIN', N'PAYROLL_OVERRIDE', N'EXPORT_PAYROLL');
```

Then recycle IIS or wait 5 minutes (`PermissionRepository` cache). Re-run verify. Recycle also undoes Inspector `OVERLAY_DIRECT` for those codes.

This rollback also removes a SWITCH_USER canary grant if the canary target was an Active Admin.

## Expected Inspector evidence

Search the Admin (e.g. `J8`) after apply + IIS recycle:

| Code | Allowed | Typical source after pack |
| --- | --- | --- |
| `SWITCH_USER` | true | `OVERLAY_DIRECT` (`OverlayWouldAllow` true; `LegacyWouldAllow` true if on CSV) |
| `USER_ADMIN` | true | `OVERLAY_DIRECT` |
| `PAYROLL_OVERRIDE` | true | `OVERLAY_DIRECT` (CSV still evaluated) |
| `EXPORT_PAYROLL` | true | `OVERLAY_DIRECT` (`J8` also hardcoded) |

`USERTYPE` remains `Admin`. `UserRoleDB` / `RolePermissionDB` unchanged (`ATS-OS` / `OS-HR` on UAT `J8`).

Office Staff: still no `SWITCH_USER` (Admin gate).

## Expected Analyzer evidence

- Permission scan: Active Admins show the four codes granted, source overlay Direct.
- Empty-overlay snapshot taken **before** this pack is the #102 canary baseline. This pack **will** change EffectiveAccess / source for Admins (expected). Freeze a new snapshot after bootstrap if further overlay work follows.
- Unexpected drift: grants to Office Staff or changes to muster role/menu columns = **FAIL**.
