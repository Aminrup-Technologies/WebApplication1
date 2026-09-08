# Stored procedure call contracts

**Baseline:** `v2.2.3-governance-final`  
**Rule:** Procedure **bodies are not in this repository**. This list is call-site only (`new SqlCommand("…")` + `CommandType.StoredProcedure`).

| Procedure | Call site | Notes |
| --- | --- | --- |
| `SP_InsertInto_EmployeeMusterTable` | `emp_registration.aspx.cs`, `emp_bulkregistration.aspx.cs` | [tbl_employee_mustertable.md](tbl_employee_mustertable.md) |
| `SP_Update_EmployeeMusterTable` | `viewupdate_empmustertabledata.aspx.cs` | Update path |
| `USP_Update_Employee_Password_Reset` | `view_emp_mastertbldata_v2.aspx.cs` | Password reset |
| `SP_InsertInto_JOBSTable` | `create_jobid_v2.aspx.cs`, `create_jobid.aspx.cs` | [tbl_jobs.md](tbl_jobs.md) |
| `SP_InsertInto_AttendanceTable` | `job_inpunch_v2.aspx.cs`, `job_inpunch.aspx.cs` | [tbl_attendance.md](tbl_attendance.md) |
| `SP_Update_AttendancePunchOUT` | `job_outpunch_v2.aspx.cs`, `job_outpunch.aspx.cs`, `job_360_view.aspx.cs` | OUT punch |
| `SP_InsertInto_SMJTable` | `create_supplymemo.aspx.cs` | Supply memo header; params include `@SMJID`, creator Session, `@Ref_JOBID`, shift counts |
| `SP_InsertInto_SupplyManpower` | `create_supplymemo.aspx.cs` | Same transaction |
| `SP_InsertInto_tbl_expenselogs` | `add_expenses.aspx.cs` | Header; `@AppByWrk` / `@AppByName` hardcoded `"J3"` / `"MAHESH CHOURASIA"` (**Verified**) |
| `SP_InsertInto_tbl_expenselogdetails` | `add_expenses.aspx.cs` | Lines + file bytes; same hardcoded approver |
| `SP_InsertInto_WOLineItemTable` | `add_workorder_lineitems.aspx.cs`, `upload_wrkordrlineitems.aspx.cs` | Work-order lines |
| `SP_InsertInto_TBTDataTable` | CSM TBT pages | [csm-hse.md](../modules/csm-hse.md) |
| `SP_InsertInto_tbl_soptraining` | `csm_soptraining.aspx.cs` | |
| `SP_InsertInto_tbl_trainingrecords` | `csm_globaltraining.aspx.cs` | |
| `usp_InsertHelpDeskTicket` | `HelpdeskCalls.cs` | [helpdesk.md](../modules/helpdesk.md) |
| `SP_GetEmpPayrollFactor_StatusMsg` | `pyrl_managedashbrd.aspx.cs` | Payroll status |
| `SP_GetEmpBankFactor_Status` | same | |
| `SP_GetDeduction_Status` | same | |
| `GetWorksiteCount` | same | Name has no `SP_` prefix (**Verified**) |

Overlay DDL lives in `scripts/create_permission_overlay.sql` (not called from pages).

## Related

- [database README](README.md)
