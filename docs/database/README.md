# Database documentation

Reverse-document tables from **code and scripts**, not from an assumed schema dump.

## Documents

| Table / topic | Doc |
| --- | --- |
| `tbl_Employee_Mustertable` | [tbl_employee_mustertable.md](tbl_employee_mustertable.md) |
| `tbl_UserLoginAudit` | [tbl_user_login_audit.md](tbl_user_login_audit.md) |
| `tlb_emp_roles`, `tlb_emp_roles_permission`, `tlb_EmployeePermissions` | [role-catalog-and-menus.md](role-catalog-and-menus.md) |
| Overlay `tlb_permissions` / `tlb_employee_permissions` / groups | [permission-overlay](../architecture/permission-overlay.md) |

## Still planned

Payroll / attendance punch tables / `tbl_jobs` — JOBID functional audit cites SPs not in repo (`SP_InsertInto_JOBSTable`, attendance SPs). Document call contracts from that audit; do not invent DML.
