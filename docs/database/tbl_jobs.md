# tbl_jobs (call contract)

**Baseline:** `v2.2.3-governance-final`; JOB predicates frozen at `v2.1.0-jobid-remediation`.  
**SP body:** `SP_InsertInto_JOBSTable` is **not in this repo**. Columns below are from V2 insert parameters, parameterized UPDATEs, and explicit SELECT lists. Do not treat this as a full CREATE TABLE.

## Purpose

One JOBID header row. Status machine is `EntryExit` / `JOB_Status` / `MasterStatusCode` / `JOBID_Status` as in [`JobStatusConstants.cs`](../../WebApplication1/App_Code/JobStatusConstants.cs) and [`JOBID_LIFECYCLE_FUNCTIONAL_AUDIT.md`](../JOBID_LIFECYCLE_FUNCTIONAL_AUDIT.md).

## Insert (`SP_InsertInto_JOBSTable` — `create_jobid_v2.aspx.cs`)

| Parameter | Typical source |
| --- | --- |
| `@CreatedDate` | Job date (DateTime) |
| `@Creator_Name`, `@Creator_Workman`, `@Creator_Region`, `@Creator_Company`, `@Creator_Site`, `@Creator_SiteCode` | Session |
| `@WorkOrderNo`, `@Workorder_Type` | WO dropdown / cache |
| `@JOBID` | Generated code |
| `@JOBID_Status` | `JobStatusConstants.StatusActive` |
| `@JOB_Region`, `@JOB_Company`, `@JOB_Site`, `@JOB_SiteCode` | Form |
| `@JOB_InchargeWrk`, `@JOB_InchargeName` | Approver |
| `@JOB_Dept`, `@JOB_Location`, `@JOB_Shift`, `@JOB_Title`, `@JOB_PermitNo` | Form |
| `@Incharge_Approval` | `'Pending'` |
| `@EntryExit` | `Created` |
| `@BillingType`, `@BillingCode`, `@AttendanceCode` | Form / WO |
| `@MasterStatusCode`, `@JOB_Status`, `@FinalUpldStatus`, `@PermitUpload` | ARC vs skip-permit matrix |
| `@FileCount` | `0` |
| `@CSM_Documents` | `'Yes'` / `'No'` |

## Follow-up UPDATE (same create method)

`Required_Documents`, `GPS_Latitude`, `GPS_Longitude`, `App_Version='V2'` WHERE `JOBID`.

## Columns also evidenced in SELECT / UPDATE elsewhere

From `view_jobdetails*.aspx.cs` / `job_360_view.aspx.cs` (not a complete list): `Id`, `CreatedDate`, `JOB_Shift`, `Creator_Workman`, `Creator_Name`, `WorkOrderNo`, `JOB_PermitNo`, `JOBID`, `JOB_Title`, `JOBID_Status`, `JOB_Site`, `JOB_SiteCode`, `JOB_InchargeWrk`, `JOB_InchargeName`, `JOB_Dept`, `JOB_Location`, `PermitDeleteDate`, `PermitDeletedByName`, `PermitDeletedByWrk`, `FinalUpldStatus`, `Incharge_Approval`, `EntryExit`, `JOB_Status`, `MasterStatusCode`, `FileCount`, `UploadType`, `PermitUpload`, `PermitUploadDate`, `ManpowerCount`, `IsBlocked`, `BlockedTimestamp`, `UnblockedUntil`.

## Related tables

- [tbl_attendance.md](tbl_attendance.md)
- [tbl_jobspermit.md](tbl_jobspermit.md)
