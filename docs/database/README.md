# Database documentation

Reverse-document tables from **code and scripts**, not from an assumed schema dump.

## Priority backlog

| Table | Evidence so far | Status |
| --- | --- | --- |
| `tbl_Employee_Mustertable` | Login, `ApplySessionFromEmployeeRow`, `emp_registration` `SP_InsertInto_EmployeeMusterTable` | Planned |
| `tbl_UserLoginAudit` | `GrantAuthenticatedSession`, `ImpersonationAudit` | Planned |
| `tlb_emp_roles` | `emp_registration` dropdown `Employee_Type` / `EmpType_Value` | Planned |
| `tlb_emp_roles_permission` | `Emp_PermissionText` / `Emp_PermissionValue` | Planned |
| `tlb_EmployeePermissions` | `webmaster.Master` `LoadPermissions` — menus only | Planned |
| `tlb_permissions` / `tlb_employee_permissions` / groups | `scripts/create_permission_overlay.sql` | See [overlay](../architecture/permission-overlay.md) |
| Payroll / attendance / JOB | Pages `pyrl_*`, `job_*_v2`, `tbl_jobs` (JOBID docs) | Planned |

Never invent columns. Mark **Inference** when a stored procedure body is not in this repo.
