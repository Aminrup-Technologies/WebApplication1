# tbl_attendance (call contract)

**Baseline:** `v2.2.3-governance-final`  
**SP bodies not in repo:** `SP_InsertInto_AttendanceTable`, `SP_Update_AttendancePunchOUT`.

## Purpose

Per-employee punch rows for a JOBID. IN writes a new row; OUT updates it. Duplicate IN is blocked when `AttendanceStatus='Entry'` already exists for `(JOBID, EmployeeWrk)` (`job_inpunch_v2.aspx.cs` **Verified**).

## Insert (`SP_InsertInto_AttendanceTable`)

Parameters from `job_inpunch_v2.aspx.cs`: `@CreatedDate`, `@Creator_Name`, `@Creator_Workman`, `@Creator_Region`, `@Creator_Company`, `@Creator_SiteName`, `@Creator_SiteCode`, `@JOBID`, `@WorkOrderNo`, `@PermitNo`, `@JOB_Region`, `@JOB_Company`, `@JOB_SiteName`, `@JOB_SiteCode`, `@JOB_InchargeWrk`, `@JOB_InchargeName`, `@JOB_Dept`, `@JOB_Location`, `@SiteIncharge_Approval` (`Pending`), `@SubmitterName`, `@SubmitterWrk`, `@SubmitterStatus` (`Entry`), employee identity/category/designation/site, `@GatePassNo`, `@SafetyPassNo`, `@Inpunch_Time`, `@AttendanceStatus` (`Entry`), `@AttendanceCode` (`Ab` on this path).

Note the parameter spelling `@WourkHours` (**Verified** typo in code).

After successful INs, V2 also `UPDATE tbl_jobs SET JOB_Status='In-Punch Done', MasterStatusCode='3', EntryExit='Entry', ManpowerCount=(COUNT of Entry rows)`.

## OUT (`SP_Update_AttendancePunchOUT`)

From `job_outpunch_v2.aspx.cs`: `@Id`, `@JOBID`, `@SubmitterStatus` (`Exit`), `@EmployeeWrk`, `@Outpunch_Time`, `@LunchFactor`, `@WorkedTime`, `@WorkedHours`, `@Calc_OT`, `@ProvidedOT`, `@LastModified`, `@AttendanceStatus` (`Exit`), `@AttendanceCode`.

## Columns also in SELECT / UPDATE

`view_jobdetails.aspx.cs` SELECT: `Id`, `JOBID`, `JOB_Region`, `CreatedDate`, `EmployeeWrk`, `EmployeeName`, `EmpDesignation`, `WourkHours`, `Inpunch_Time`, `Outpunch_Time`, `LunchFactor`, `ProvidedOT`, `AttendanceStatus`, `AttendanceCode`. Direct UPDATE also sets `WorkedTime`, `WorkedHours`, `Calc_OT`, `LastModified`, `ModifiedByWrk`, `ModifiedByName`.

## Related

- [jobid.md](../modules/jobid.md)
- [attendance.md](../modules/attendance.md)
