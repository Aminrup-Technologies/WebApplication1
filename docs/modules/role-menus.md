# Role catalog and menu profiles

**Baseline:** `v2.2.3-governance-final` (`495fc68`)

## Purpose

CRUD for `tlb_emp_roles` (login-group labels) and `tlb_emp_roles_permission` (sidebar profile keys). This is **not** `User_RoleType` Admin and **not** overlay `tlb_employee_permissions`.

## Navigation

| Page | Writes |
| --- | --- |
| `manage_rolls.aspx` | `INSERT INTO tlb_emp_roles` (`Employee_Type`, `EmpType_Value`, `ViewMode`, `DeleteMode`, `AddedByWrk`, `TimeStamp`) |
| `manage_rollsaccess.aspx` | Menu profile rows (see code-behind; binds `tlb_emp_roles`) |
| `aminrup/manage_roles.aspx` | Aminrup master UI — `aminrup.Master` (Session presence only, no sidebar table) |

## Access control

Five-key Session on production pages. **No** `CanAccess("USER_ADMIN")` on these files (**Verified** `manage_rolls` Page_Load). Anyone who can open the URL can insert a role catalog row.

`USER_ADMIN` exists in overlay SQL as “security administration (no legacy page gate)”. Inspector/Analyzer use `IsAdmin()`, not this catalog UI.

## Related

- [role-catalog-and-menus.md](../database/role-catalog-and-menus.md)
- [security-admin.md](security-admin.md)
