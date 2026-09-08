# Security change matrix

**Baseline:** Security Foundation v2.2.1 (`v2.2.1-governance`, `19c286e`).  
**Canonical authorization API:** `AuthorizationService` (`WebApplication1/App_Code/AuthorizationService.cs`).  
**Rules:** `CONTRIBUTING.md`. **Ownership:** `.github/CODEOWNERS`.

Use this matrix when opening a PR. The reviewer is whoever CODEOWNERS requests today (`@Aminrup-Technologies` until a dedicated security team is assigned). Risk is the impact of getting the change wrong, not a schedule.

| Area | Typical paths | Reviewer | Risk | Must not |
| --- | --- | --- | --- | --- |
| Authentication | `WebApplication1/Login.aspx`, `Login.aspx.cs`, MFA helpers | Code owner (security core) | **Critical** — identity grant, MFA skip, Forms tickets | Bypass `ApplySessionFromEmployeeRow`; call `GrantAuthenticatedSession` from Switch User; introduce `SetAuthCookie` |
| Session | `SessionKeys.cs`, master `Page_Load` five-key gate, homepage logout | Code owner (security core) | **Critical** — missing keys, duplicate builders, impersonation snapshot corruption | New Session privilege compares; new identity keys outside `SessionKeys`; second Session builder |
| Authorization | `AuthorizationService.cs`, page `CanAccess` call sites | Code owner (security core) | **Critical** — allow/deny for Admin, Switch User, payroll, JOB360 | New page-level `USERTYPE` or `UserRoleDB` gates; treat `tlb_EmployeePermissions` as page auth |
| Overlay | `PermissionRepository.cs`, `tlb_permissions`, `tlb_employee_permissions`, overlay SQL | Code owner (security core) | **High** — fail-open cache, wrong table, grants to Office Staff | Merge overlay with sidebar table; skip dual-path during migration; grant SWITCH_USER without Admin |
| Impersonation | `ImpersonationAudit.cs`, `SwitchUser.*`, master banner | Code owner (security core) | **Critical** — nested switch, lost ORIGINAL_*, target LastLogout | Nested impersonation; restore without `IsOriginalIdentityCaptured`; new Workman allowlist on the page |
| Release docs | `docs/release/`, `docs/release/TEMPLATE/` | Code owner (release) | **Medium** — unsigned GO, invented IIS/SQL | Squash-merge a UAT integration PR; skip build / IIS / SQL / canary / signoff |
| SQL scripts | `scripts/create_permission_overlay.sql`, `bootstrap_platform_admin*.sql`, `uat_switch_user_canary.sql`, `promote_uat_admin.sql` | Code owner (security core) | **High** — privilege expansion, invented Admin role catalog | Invent `tlb_emp_roles` Admin rows; grant Office Staff SWITCH_USER; apply canary without a chosen WorkmanSL |

## Canonical API

All **new** permission checks call `AuthorizationService`:

| Method | Use |
| --- | --- |
| `IsAuthenticated()` | Five-key Session presence |
| `IsAdmin()` | `USERTYPE == "Admin"` only |
| `CanAccess(code)` / `HasPermission(code)` | Feature gate (overlay → config → hardcoded, plus SWITCH_USER dual-path) |
| `DescribeIdentity` | Inspector / Analyzer only (restores live Session) |

Do not add a parallel helper that re-implements these predicates. `ImpersonationAudit.CanImpersonate` is the legacy Admin+CSV engine; new UI uses `CanAccess("SWITCH_USER")`.

## Overlay precedence (unchanged)

1. Session  
2. Platform role (`USERTYPE`)  
3. Module exception (JOB360 / attendance: Admin or Office Staff)  
4. Overlay Direct/Group  
5. Config CSV  
6. Hardcoded WorkmanSL (legacy, inside `AuthorizationService` only)  
7. Deny  

See `docs/architecture/authorization-flow.md`.

## SQL script classes

| Class | Example | Review bar |
| --- | --- | --- |
| Additive schema | `create_permission_overlay.sql` | No employee grants; no muster/role catalog writes |
| Dry-run bootstrap | `bootstrap_platform_admin.sql` (`@ApplyChanges = 0` default) | Active Admin only; four pack codes; `BOOTSTRAP_SUMMARY` |
| Canary | `uat_switch_user_canary.sql` | `(WorkmanSL, PermissionId)`; chosen Admin; IIS recycle |
| Discovery / no-op | `promote_uat_admin.sql` | Must not invent Admin `tlb_emp_roles` rows |

## How to use in a PR

1. Identify the area(s) in the table.  
2. Tick the Security Foundation checklist in `.github/pull_request_template.md`.  
3. If the PR touches CODEOWNERS paths, wait for the listed owner.  
4. If it is part of a release, copy `docs/release/TEMPLATE/` and fill evidence.
