# Security administration (read-only inspectors)

**Baseline:** `v2.2.3-governance-final` (`495fc68`)  
**PRs:** C1 Permission Inspector (`ac0e863`), C2 Access Analyzer (`30b60dd`)

## Purpose

Support / UAT tools that **describe** `AuthorizationService` for an employee. No grant writes. No menu entry.

## Navigation

| Page | Gate |
| --- | --- |
| `bussiness/production/admin/security/PermissionInspector.aspx` | `IsAuthenticated()` else login; `IsAdmin()` else homepage |
| `bussiness/production/admin/security/AccessAnalyzer.aspx` | Same pattern (see file) |

Path is under **production** `admin/security/`, not legacy `WebApplication1/Admin/`.

## Workflow

Search Active employees → `DescribeIdentity` (temporarily overlays Session keys, restores in `finally`). Shows overlay Direct/Group vs CSV vs hardcoded vs module exception. Cache peek uses `PermissionRepository`.

## Security

Admin-only. Overlay `USER_ADMIN` is **catalogued** in SQL but these pages check `IsAdmin()`, not `CanAccess("USER_ADMIN")` (**Verified** Inspector `Page_Load`).

## Related

- [permission-overlay](../architecture/permission-overlay.md)
- [PLATFORM_ADMIN_RUNBOOK.md](../PLATFORM_ADMIN_RUNBOOK.md)
