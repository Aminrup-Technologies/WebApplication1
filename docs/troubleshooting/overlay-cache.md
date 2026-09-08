# Overlay cache stale

**Evidence:** `PermissionRepository.CacheTtlMinutes = 5`; `HttpRuntime.Cache` inserts with absolute expiry.

## Symptom

SQL grant/revoke in `tlb_employee_permissions` (or group tables) does not change `CanAccess` immediately.

## Cause (Verified)

Snapshots are cached **five minutes** per WorkmanSL (and catalog). Recycle the IIS app pool or wait for TTL. `PermissionRepository` can bump a version key / remove a snapshot; pages do not call that on ordinary SQL edits.

## Checks

1. Confirm the row is the overlay table `tlb_employee_permissions`, not sidebar `tlb_EmployeePermissions`.
2. `WorkmanSL` matches Session `WORKMAN`.
3. PermissionCode matches `AuthorizationFeatureCodes` / `tlb_permissions`.
4. Recycle app pool on the UAT site.

## Related

- [permission-overlay](../architecture/permission-overlay.md)
- [PLATFORM_ADMIN_RUNBOOK.md](../PLATFORM_ADMIN_RUNBOOK.md)
