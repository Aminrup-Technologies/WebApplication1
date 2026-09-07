# Platform Admin bootstrap runbook

**Script:** `scripts/bootstrap_platform_admin.sql`  
**Verify:** `scripts/bootstrap_platform_admin_verify.sql`  
**Design:** `docs/PLATFORM_ADMIN_BOOTSTRAP.md`  
**PR:** #104 — additive SQL + docs. No application code.

Prerequisite on the target database: `scripts/create_permission_overlay.sql` (PR #96) has been applied.

Default `@ApplyChanges = 0` (dry-run). Never run apply on production without a dry-run paste and a change window.

## UAT (`atserp_uat`)

1. Confirm overlay tables exist (`INFORMATION_SCHEMA` for `tlb_permissions`, `tlb_employee_permissions`). If missing, run `create_permission_overlay.sql` first.
2. Run `bootstrap_platform_admin.sql` with `@ApplyChanges = 0`. Capture PlannedInsertCount.
3. Confirm planned WorkmanSL list is only `User_RoleType = Admin` and `WorkStatus = Active` (UAT: `J8`).
4. Set `@ApplyChanges = 1` and execute.
5. Run `bootstrap_platform_admin_verify.sql`. **PASS** when `MissingGrantCount = 0` and `DuplicateGrantGroupCount = 0`.
6. Recycle IIS (or wait 5 minutes).
7. Login as `J8`. Inspector: four codes granted, `SWITCH_USER` `OVERLAY_DIRECT`.
8. Second apply: `RowsInserted = 0`.

Do not re-save `J8` from the employee role dropdown (no `Admin` catalog row).

## Staging

Same as UAT. Dry-run first. Diff PlannedInsertCount against the staging Active Admin roster. Apply only if the roster is the intended platform-admin set. Recycle the staging app pool. Capture Inspector on one Admin and one Office Staff (Office Staff must stay denied for `SWITCH_USER`).

## Production

1. Change ticket + backup/snapshot of `tlb_permissions` and `tlb_employee_permissions` (or full DB backup per ops policy).
2. Dry-run in production SSMS (`@ApplyChanges = 0`). Attach the planned grid to the ticket.
3. Confirm every planned `WorkmanSL` is a real platform Admin. If a non-operator has `User_RoleType = Admin`, fix muster **before** apply (this script does not change `User_RoleType`).
4. `@ApplyChanges = 1` in a transaction (script already uses `BEGIN TRANSACTION`).
5. Verify script. `MissingGrantCount` must be 0.
6. Recycle IIS / wait 5 minutes.
7. Smoke: login, Switch User (Admin), payroll chrome, no Office Staff Switch User.
8. Keep the rollback SQL from `PLATFORM_ADMIN_BOOTSTRAP.md` in the ticket until hypercare ends.

## Validation checklist

| Check | PASS |
| --- | --- |
| Overlay tables existed before apply (or #96 script run) | |
| Dry-run reviewed | |
| Apply `RowsInserted` = dry-run `PlannedInsertCount` | |
| Second apply `RowsInserted` = 0 | |
| `MissingGrantCount` = 0 | |
| `DuplicateGrantGroupCount` = 0 | |
| Muster `UserRoleDB` / `RolePermissionDB` / password / `LastLogin` unchanged | |
| Inspector Active Admin: four codes overlay Direct | |
| Inspector Office Staff: `SWITCH_USER` denied | |
| `AuthorizationService.cs` not modified in this PR | |
| Login.aspx / Session / master not modified | |

## Rollback checklist

| Step | PASS |
| --- | --- |
| Run rollback `DELETE` (four codes × Active Admin only) from `PLATFORM_ADMIN_BOOTSTRAP.md` | |
| Verify: Active Admin overlay pack rows gone; other overlay codes untouched | |
| Recycle IIS / wait 5 minutes | |
| `SWITCH_USER` for CSV Admins still works via `USERTYPE+LEGACY_CONFIG` | |
| `EXPORT_PAYROLL` for `J8` still works via hardcoded list | |
| `USER_ADMIN` overlay-only pages stay denied until re-granted | |
| Ticket updated with verify grids | |
