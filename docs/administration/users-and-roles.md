# Users and roles (operations)

**Status:** Verified

## Create an employee

Use `emp_registration.aspx` (see [employee-registration](../modules/employee-registration.md)). Insert goes through `SP_InsertInto_EmployeeMusterTable`. Stored-procedure **body is not in the repo**.

## Admin vs sidebar role

| Concept | Field / table | Operator meaning |
| --- | --- | --- |
| Runtime Admin | `tbl_Employee_Mustertable.User_RoleType = Admin` | `IsAdmin()`, Switch User prerequisite |
| Sidebar pack | `RolePermissionDB` → `tlb_EmployeePermissions` | Menu visibility only |
| Login group | `UserRoleDB` → `tlb_emp_roles` | Catalog / dropdown; **not** allow/deny |
| Overlay | `tlb_employee_permissions` | Direct/Group page tools |

Do **not** expect an Admin row in `tlb_emp_roles`. UAT Admin **J8** / `ATS002112` uses `UserRoleDB=ATS-OS`, `RolePermissionDB=OS-HR`, `User_RoleType=Admin`.

Catalog UI: `manage_rolls.aspx` / `manage_rollsaccess.aspx` — Session only, not `IsAdmin()` ([role-menus](../modules/role-menus.md)).

## Overlay grants

Apply via SQL in `scripts/create_permission_overlay.sql` / `scripts/bootstrap_platform_admin.sql` until v2.3 UI exists. Cache TTL is **five minutes** ([overlay-cache](../troubleshooting/overlay-cache.md)).

Inspect without writing: [security-admin](../modules/security-admin.md).
