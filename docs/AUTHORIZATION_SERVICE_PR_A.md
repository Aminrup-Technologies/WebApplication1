# PR A — AuthorizationService foundation

**Code:** `WebApplication1/App_Code/AuthorizationService.cs`  
**Pages modified:** none  
**Callers in production:** none (additive; existing pages still use Session / `ImpersonationAudit` / hardcoded WorkmanSL)

## Authorization regression matrix

Every row is **Same** because no page was migrated. The service is specified to reproduce the legacy predicate so PR E can swap call sites without behavior change.

| Legacy scenario | Legacy predicate | `AuthorizationService` | New result |
| --- | --- | --- | --- |
| Admin login | Five Session keys set by `ApplySessionFromEmployeeRow` | `IsAuthenticated()` true | Same |
| Office Staff login | Same five keys; `USERTYPE=Office Staff` | `IsAuthenticated()` true; `IsAdmin()` false | Same |
| Site Staff / other type | Same five keys | `IsAuthenticated()` true; `IsAdmin()` false | Same |
| Missing Session | Master / homepage redirect login | `IsAuthenticated()` false; all `CanAccess` false | Same |
| Switch User (Admin + allowlist, not impersonating) | `ImpersonationAudit.CanImpersonate` | `CanAccess("SWITCH_USER")` delegates to `CanImpersonate` | Same |
| Switch User (allowlisted Office Staff) | `CanImpersonate` false (`USERTYPE` not Admin) | `CanAccess("SWITCH_USER")` false | Same |
| Switch User (Admin, not on `SwitchUserAuthorizedUsers`) | `CanImpersonate` false | false | Same |
| Switch User (already impersonating) | `CanImpersonate` false | false | Same |
| Payroll tab (`PayrollAuthorizedUsers`) | CSV vs `WORKMAN` | `CanAccess("PAYROLL_OVERRIDE")` | Same |
| JOB360 admin chrome | `job_360_view.IsAdmin()` Admin **or** Office Staff | `CanAccess("JOB360_OVERRIDE")` | Same |
| JOB360 platform admin | n/a (local helper includes Office Staff) | `IsAdmin()` is Admin **only** | Same as audit recommendation; pages still use local helper |
| Attendance / job exceptions pages | Admin **or** Office Staff | `CanAccess("ATTENDANCE_OVERRIDE")` | Same |
| Payroll dashboard extra chrome | `WORKMAN == J8` | `CanAccess("EXPORT_PAYROLL")` / `LEGACY_PAYROLL_DASHBOARD` | Same |
| Attach manpower row | J8 / A84 / K208 / N21 | `CanAccess("LEGACY_ATTACH_MANPOWER")` | Same |
| Expense heads extra access | J8 / A84 / K208 | `CanAccess("LEGACY_EXPENSE_HEADS")` | Same |
| Role catalog pages (`manage_rolls`) | Session presence only | `CanAccess("USER_ADMIN")` false until overlay | Same (pages not calling the service) |
| Menu visibility | `RolePermissionDB` → `tlb_EmployeePermissions` | Not wrapped | Same |

## Migration coverage (PR A)

### Pages still using Session directly

All production pages (~100) plus `webmaster.Master.cs`. Unchanged.

### Pages migrated

None.

### Hardcoded allowlists remaining

| Location | Values | Future feature code |
| --- | --- | --- |
| `Web.config.example` `SwitchUserAuthorizedUsers` | `J8` | `SWITCH_USER` |
| `Web.config.example` `PayrollAuthorizedUsers` | `J8` | `PAYROLL_OVERRIDE` |
| `pyrl_managedashbrd.aspx.cs` 126 | `J8` | `LEGACY_PAYROLL_DASHBOARD` / `EXPORT_PAYROLL` |
| `pyrl_gnrtdashbrd.aspx.cs` 37 | `J8` | same |
| `viewupdate_emppayrolldata.aspx.cs` 33 | `J8` | same |
| `view_jobdetails.aspx.cs` 61 | A84, K208, N21, J8 | `LEGACY_ATTACH_MANPOWER` |
| `view_jobdetails_v2.aspx.cs` 69 | same | same |
| `add_expense_heads.aspx.cs` 42 | J8, A84, K208 | `LEGACY_EXPENSE_HEADS` |
| `add_expense_subheads.aspx.cs` 45 | same | same |
| `create_supplymemo.aspx.cs` 89 | commented A84/K208 | ignore until uncommented |
| `view_monthlyjobs.aspx.cs` 36 | commented J8 | ignore |

## Security findings (snapshot, unchanged)

From `docs/ROLE_PERMISSION_ARCHITECTURE_AUDIT.md`:

| Class | What |
| --- | --- |
| Menu-only | Sidebar `Visible`; URL still works |
| Session presence only | Dominant (~100 pages) |
| `USERTYPE` value | Switch User (Admin only); JOB360 / exceptions / anomalies (Admin or Office Staff); JOB create Site Staff restrictions |
| Named-user exceptions | Config CSVs + hardcoded WorkmanSL |

## How to verify without IIS callers

No page calls the service yet. Review:

1. `CanAccess("SWITCH_USER")` body is only `ImpersonationAudit.CanImpersonate(session)`.
2. `IsAdmin()` compares `SessionKeys.UserType` to `ImpersonationAudit.AdminUserType` (`"Admin"`).
3. `HasOverlayPermission` is a stub `return false` with no SQL.
4. `WebApplication1.csproj` compiles `App_Code\AuthorizationService.cs`.
5. `git diff` contains no `.aspx` / `.Master` / `Login.aspx.cs` / `ImpersonationAudit.cs` / `SwitchUser.aspx.cs` changes.
