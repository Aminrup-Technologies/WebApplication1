# HR operations

**Baseline:** `v2.2.3-governance-final` (`495fc68`)  
**Status:** Verified hub. There is **no** `HR_MODULE.md` and **no** `HR` permission code in `AuthorizationService`.

## Purpose

Cross-cutting HR work already documented per module. This page is a map, not a second workflow.

## Map

| Area | Doc | Runtime keys (if any) |
| --- | --- | --- |
| Hire / master | [employee-registration.md](employee-registration.md) | Session + sidebar |
| Gate / safety fields | [gate-pass.md](gate-pass.md), [safety-pass.md](safety-pass.md) | Session |
| Attendance sheets / exceptions | [attendance.md](attendance.md) | `ATTENDANCE_OVERRIDE` on anomaly/exception pages |
| Payroll | [payroll.md](payroll.md) | `EXPORT_PAYROLL`, `PAYROLL_OVERRIDE`, `LEGACY_PAYROLL_DASHBOARD` |
| Role catalog UI | [role-menus.md](role-menus.md) | Session; catalog is not `IsAdmin()` |
| Exit | [exit-process.md](exit-process.md) | No dedicated pages |

## Access control (Verified)

Sidebar: `RolePermissionDB` → `tlb_EmployeePermissions`. Page tools: overlay / CSV / hardcoded lists inside `AuthorizationService`. `User_RoleType` `Admin` is **not** required for every HR page.

UAT Admin **J8** can have `UserRoleDB=ATS-OS` / `RolePermissionDB=OS-HR` while `User_RoleType=Admin`.

## Related

- [role-catalog-and-menus.md](../database/role-catalog-and-menus.md)
- [authorization-flow](../architecture/authorization-flow.md)
