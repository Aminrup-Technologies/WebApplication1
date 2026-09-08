# Role catalog and sidebar menus

**Baseline:** `v2.2.3-governance-final` (`495fc68`)  
**Evidence:** `emp_registration.aspx.cs` dropdowns; `webmaster.Master.cs` `LoadPermissions`; architecture audit. Overlay tables are **not** these objects.

## tlb_emp_roles

| Column (evidenced SELECT) | Use |
| --- | --- |
| `Employee_Type` | Dropdown text → `User_RoleType` |
| `EmpType_Value` | Dropdown value → `UserRoleDB` |
| `Id` | ORDER BY |

Catalog of platform role **labels**. UAT Admin J8 used `UserRoleDB=ATS-OS` whose label is Office Staff while `User_RoleType=Admin` — **Verified** in Security Foundation notes. Do not invent an `Employee_Type='Admin'` row if none exists.

CRUD: `manage_rolls.aspx.cs` (catalog UI). Not used at runtime for allow/deny.

## tlb_emp_roles_permission

| Column | Use |
| --- | --- |
| `Emp_PermissionText` | → `Role_Permission` |
| `Emp_PermissionValue` | → `RolePermissionDB` |
| `EmpType_Value` | Filter profiles for the selected type |
| `Id` | ORDER BY |

Independent of `tlb_emp_roles` beyond the filter. A type can have a fat or thin menu profile.

CRUD: `manage_rollsaccess.aspx.cs`.

## tlb_EmployeePermissions (sidebar)

**Different from** `tlb_employee_permissions` (overlay).

| Column (evidenced SELECT) | Use |
| --- | --- |
| `ParentKey`, `ChildKey` | Menu panel ids in `ApplyPermissions` |
| `IsVisible` | Show/hide |
| `Emp_PermissionValue` | WHERE = Session `RolePermissionDB` |

Single consumer: `webmaster.Master`. Pages stay URL-reachable when hidden.

## Overlay (link only)

Schema and seeds: [permission-overlay](../architecture/permission-overlay.md) and `scripts/create_permission_overlay.sql`.

| Table | PK / notes |
| --- | --- |
| `tlb_permissions` | `PermissionCode` unique; seed SWITCH_USER, PAYROLL_OVERRIDE, JOB360_OVERRIDE, ATTENDANCE_OVERRIDE, EXPORT_PAYROLL, USER_ADMIN |
| `tlb_permission_groups` | `GroupCode` |
| `tlb_group_permissions` | `(GroupId, PermissionId)` |
| `tlb_employee_group` | `(WorkmanSL, GroupId)` + index on WorkmanSL |
| `tlb_employee_permissions` | `(WorkmanSL, PermissionId)` + index on WorkmanSL |

## Security

Never use sidebar or `UserRoleDB` as page authorization. New platform permissions go in `tlb_permissions`.
