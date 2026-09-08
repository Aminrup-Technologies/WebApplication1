# Gate pass (employee fields)

**Baseline:** `v2.2.3-governance-final` (`495fc68`)  
**Status:** Verified — **not** a standalone page module.

There is **no** `gatepass.aspx`, `view_gatepass.aspx`, `GATEPASS_OVERRIDE`, or `GATEPASS_MODULE.md` in this repository. Gate pass is **employee master data** used at JOB IN-Punch.

## Purpose

Store and renew an employee’s gate-pass number and expiry so IN-Punch can copy `GatePassNo` onto the attendance row and hide “Add to Roster” when expiry is past (JOBID audit).

## Evidence

| Item | Location |
| --- | --- |
| Columns | `tbl_Employee_Mustertable.GatePassNo`, `GatePassExpiry`, `GP_UpdateApproval`, `GP_ModifierWrk`, `GP_ModifierName`, `GP_ModifiedDate` — `viewupdate_empmustertabledata.aspx.cs` `ReflectNewGPData` |
| Registration insert | `SP_InsertInto_EmployeeMusterTable` params (see [employee-registration](employee-registration.md)) |
| IN-Punch copy | `job_inpunch_v2.aspx.cs` `@GatePassNo` on `SP_InsertInto_AttendanceTable` |
| Lookup helper | `DB_Utility_OH4Y.FindEmployeeDataforInPunch` reads `GatePassNo` |
| Lifecycle notes | [`JOBID_LIFECYCLE_FUNCTIONAL_AUDIT.md`](../JOBID_LIFECYCLE_FUNCTIONAL_AUDIT.md) (expired gatepass / `GP_UpdateApproval`) |

## Users / workflow

HR edits pass fields on `viewupdate_empmustertabledata.aspx` / `searchemployee.aspx`. Submit writes parameterized `UPDATE tbl_Employee_Mustertable` and sets `GP_UpdateApproval='Pending'`. Supervisors consume the values at IN-Punch. Session gate is the master five keys (those pages use `webmaster.Master`).

## Security

No overlay code. Do not invent `GATEPASS_OVERRIDE`. `AuthorizationService` does not mention gate pass.

## Related

- [safety-pass.md](safety-pass.md) (same UPDATE)
- [employee-registration.md](employee-registration.md)
- [jobid.md](jobid.md)
