# Database documentation

Reverse-document tables from **code and scripts**, not from an assumed schema dump.

## Documents

| Table / topic | Doc |
| --- | --- |
| `tbl_Employee_Mustertable` | [tbl_employee_mustertable.md](tbl_employee_mustertable.md) |
| `tbl_UserLoginAudit` | [tbl_user_login_audit.md](tbl_user_login_audit.md) |
| `tlb_emp_roles`, `tlb_emp_roles_permission`, `tlb_EmployeePermissions` | [role-catalog-and-menus.md](role-catalog-and-menus.md) |
| Overlay `tlb_permissions` / `tlb_employee_permissions` / groups | [permission-overlay](../architecture/permission-overlay.md) |
| `tbl_jobs` | [tbl_jobs.md](tbl_jobs.md) |
| `tbl_attendance` | [tbl_attendance.md](tbl_attendance.md) |
| `tbl_jobspermit` | [tbl_jobspermit.md](tbl_jobspermit.md) |
| Geo catalogs | [geo-catalog.md](geo-catalog.md) |
| Expense logs | [tbl_expenselogs.md](tbl_expenselogs.md) |
| SP call list | [stored-procedure-call-contracts.md](stored-procedure-call-contracts.md) |

Payroll sheet tables beyond the SPs on `pyrl_managedashbrd.aspx.cs` are **not** reverse-documented (bodies not in repo). Helpdesk ticket table is only known via `usp_InsertHelpDeskTicket` parameters.
