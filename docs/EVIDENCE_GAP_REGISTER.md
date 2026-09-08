# Evidence gap register

**Baseline:** `v2.2.3-governance-final` (`495fc68`)  
**Rule:** If the repo does not prove a table, column, or SP body, record it here. **Do not invent** the missing object in module or database docs.

Close a row only when the cited **Future source** lands in git (script, export, or call-site that names the object) and the matching doc is updated.

| ID | Area | Evidence missing | Current documentation | Blocking | Future source |
| --- | --- | --- | --- | --- | --- |
| EG-001 | Database / all SPs | Stored procedure **bodies** (`CREATE PROCEDURE`) for ERP SPs | [stored-procedure-call-contracts.md](database/stored-procedure-call-contracts.md) lists call sites only | Cannot document DML inside SPs | SQL export or `scripts/` adding procedure definitions |
| EG-002 | Payroll | Bodies for `SP_GetEmpPayrollFactor_StatusMsg`, `SP_GetEmpBankFactor_Status`, `SP_GetDeduction_Status`, `GetWorksiteCount`, `GetPayrollData`, `USP_Pre_Payroll_Audit`, `usp_GetSalaryDetailsats`, `SP_InsertIntoEmployeesMonthlyForm17Trail` | [payroll.md](modules/payroll.md); call-contract list | Cannot reverse payroll sheet tables | SQL export; cite additional `pyrl_*.aspx.cs` when documenting |
| EG-003 | Helpdesk | Persistence table for `usp_InsertHelpDeskTicket` (table name not in repo) | [helpdesk.md](modules/helpdesk.md) parameters only | Cannot name the ticket table | SP body or `SELECT`/`INSERT` that names the table |
| EG-004 | Geography / HR | Department hierarchy beyond geo catalogs (`tlb_workregion_compdept` evidenced on `ats_work_sites.aspx.cs`; `workcompany_dept.aspx` / `workcomp_deptheads.aspx` not reverse-documented) | [geo-catalog.md](database/geo-catalog.md), [geo-masters.md](modules/geo-masters.md) | Cannot list department columns | Parameterized SELECT lists from those pages |
| EG-005 | JOBID / CSM | `tbl_tbt`, `tbl_sop` (`SELECT *` in `job_360_view.aspx.cs`) | [csm-hse.md](modules/csm-hse.md); JOB360 file only | Cannot list TBT/SOP columns | Explicit column SELECT or SP params |
| EG-006 | Attendance codes | `tlb_attendancecodes` (DISTINCT Status / Status_Name, Status_Code) | [attendance.md](modules/attendance.md) | Catalog columns incomplete | Dedicated SELECT list + page doc |
| EG-007 | Notifications | `NotificationTemplates` (`TemplateID` / `TemplateCode` on `job_inpunch_v2.aspx.cs`) | Mentioned only via `NotificationTriggerHelper` in integrations | Template schema unknown | Table script or broader SELECT |
| EG-008 | Expenses | Remaining `tbl_expenselogs` / detail columns beyond insert params | [tbl_expenselogs.md](database/tbl_expenselogs.md) | Full CREATE unknown | SP body or `SELECT` list |
| EG-009 | Supply memo | `SP_InsertInto_SMJTable` / `SP_InsertInto_SupplyManpower` bodies and table names beyond params | [supply-memo.md](modules/supply-memo.md) | Table names not fully cited | SP body or INSERT table name in code |
| EG-010 | Work orders | `SP_InsertInto_WOLineItemTable` body / line-item table | Call-contract row only | Line-item schema unknown | Call-site table name or SP body |
| EG-011 | JOBID | `SP_InsertInto_JOBSTable` / attendance punch SP bodies | [tbl_jobs.md](database/tbl_jobs.md), [tbl_attendance.md](database/tbl_attendance.md) | Columns limited to parameters + evidenced UPDATE/SELECT | SQL export |
| EG-012 | Homepage | `GetEmployeeHomepageData`, `GetContactUpdateStatus`, `UpdateEmployeeContactInfo`, `UpdateContactStatus` | Not reverse-documented | Homepage data contract unknown | Homepage code-behind parameter lists |
| EG-013 | Attach manpower | `SP_Insert_EmployeeAttendance` body | [jobid.md](modules/jobid.md) / attendance wrap | Extra attendance insert path | SP body or params from `attach_manpower.aspx.cs` |
| EG-014 | Public / legacy | `ApplyformSp`, `GetActiveJobOpenings`, `Product_Crud`, `Category_Crud`, `Cart_Crud`, `Dashboard`, `User_Crud` | [public-site.md](modules/public-site.md), [legacy-admin.md](modules/legacy-admin.md) | Out of ERP Session model; not in ERP table priority | Only if those apps are brought into the ERP program |
| EG-015 | Overlay groups UI | v2.3 overlay CRUD / groups pages | Architecture notes “not started” | No UI to document | Future `feature/security-admin-v2.3` with evidence |

## How to add a row

1. ID = `EG-` next number.
2. Cite the file that **proves** the object is missing (no `CREATE PROCEDURE` in repo, or `SELECT *` only).
3. Point **Current documentation** at the Active call-contract or module page.
4. Do not fill “inferred columns.”

## Related

- [DOCUMENTATION_COVERAGE.md](DOCUMENTATION_COVERAGE.md)
- [documentation_governance.md](governance/documentation_governance.md)
- [charter](ERP_DOCUMENTATION_CHARTER.md)
