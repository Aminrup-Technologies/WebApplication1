/*
    MANUAL UAT ONLY — SWITCH_USER overlay canary (PR E).

    Do not run automatically. Do not seed production grants.
    Overlay table is tlb_employee_permissions (WorkmanSL, PermissionId).
    This is not tlb_EmployeePermissions (legacy menu matrix).

    Replace @WorkmanSL with a real Admin WorkmanSL who is NOT on
    SwitchUserAuthorizedUsers, so Snapshot Compare shows exactly one
    EffectiveAccess change.

    After INSERT: recycle IIS or wait 5 minutes (PermissionRepository cache).
    After UAT: run the DELETE, then Snapshot Compare should PASS against
    the frozen PR #100 baseline.
*/

DECLARE @WorkmanSL NVARCHAR(50) = N'REPLACE_WITH_ADMIN_WORKMAN';

-- Grant
INSERT INTO dbo.tlb_employee_permissions (WorkmanSL, PermissionId)
SELECT @WorkmanSL, p.Id
FROM dbo.tlb_permissions p
WHERE p.PermissionCode = N'SWITCH_USER'
  AND p.IsActive = 1
  AND NOT EXISTS (
        SELECT 1
        FROM dbo.tlb_employee_permissions ep
        WHERE ep.WorkmanSL = @WorkmanSL
          AND ep.PermissionId = p.Id
    );

-- Verify
SELECT ep.WorkmanSL, p.PermissionCode
FROM dbo.tlb_employee_permissions ep
INNER JOIN dbo.tlb_permissions p ON p.Id = ep.PermissionId
WHERE ep.WorkmanSL = @WorkmanSL
  AND p.PermissionCode = N'SWITCH_USER';

/*
-- Rollback
DELETE ep
FROM dbo.tlb_employee_permissions ep
INNER JOIN dbo.tlb_permissions p ON p.Id = ep.PermissionId
WHERE ep.WorkmanSL = @WorkmanSL
  AND p.PermissionCode = N'SWITCH_USER';
*/
