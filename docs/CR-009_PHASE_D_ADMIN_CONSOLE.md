# CR-009 Phase D — Admin Inspector, Edit Core, and Audit Timeline

Status: Merged  
Merge: PR #75 → `04138e1`  
Date: 2026-09-06  
Base: `Jul_to_Sep_2026_Suport_N_Dev_Works` (`c0dd403` docs head; executable parent `7747a57` Phase C)  
Change type: Enhancement (Admin console)

This document is post-merge evidence for Phase D. It does not change executable behavior.

---

## Scope

Enable the existing Admin Inspector and Edit Core surfaces, and add a read-only Audit Timeline from timestamps already loaded by `EvaluateSmartLifecycle`.

| File | Role |
|------|------|
| `job_360_view.aspx` | Inspector / Edit Core / Overrides cards + timeline markup |
| `job_360_view.aspx.cs` | `ApplyAdminConsoleVisibility`, click-time `IsAdmin()`, `BindAuditTimeline` |
| `job_360_view.aspx.designer.cs` | New `runat="server"` fields |

No V2 pages. No schema. No inbox or KPI SQL. No new `MasterStatusCode`. Close & Send remains on `job_outpunch_v2`.

---

## Admin authorization

| Control | Visible when | Click-time `IsAdmin()` | SQL |
|---------|--------------|------------------------|-----|
| `btn_Act_ViewRawData` | Admin / Office Staff | Yes | Existing `SELECT *` jobs/attendance/TBT/SOP; permits metadata without blob |
| `btn_Act_EditCoreAdmin` | Admin / Office Staff | Same handler as Details Edit | Existing modal |
| `btn_EditCoreDetails` | Admin / Office Staff | Yes | Existing modal |
| `btn_SaveCoreDetails` | Modal | Yes | Existing `UPDATE tbl_jobs` shift/title only |
| Phase C override handlers | Unchanged from `7747a57` | Already gated | Unchanged |

Non-admin: Inspector/Edit buttons stay hidden. Forged postback returns Access Denied and runs no SQL.

---

## Timeline data sources

No new queries or tables. No audit inserts.

| Node | Source |
|------|--------|
| Created | `dtMasterJobSysCreation` (`tbl_jobs.TimeStamp`) |
| First IN | `MIN(Inpunch_Time)` |
| Permit | `PermitUploadDate` |
| OUT | `MAX(LastModified)` (existing stepper aggregate) |
| Close & Send | `EntryExit='Exit'` + `UpdatedOn` if present, else **Recorded** |
| Approval | `Incharge_ApprovalDate` |

`BindAuditTimeline` runs immediately before `EvaluateActionMatrix`. Stepper classes and `pipelineStep` are unchanged.

---

## Preserved behavior

- `ApplySupervisorHopVisibility` (Phase B) byte-identical
- `EncodeJobID` on Permit / IN / OUT hops
- Phase C click-time guards on the eight write handlers
- Permit download `Id AND JOBID`
- InvalidateWorker `ManpowerCount` Entry count
- Force OUT / Bypass / Cancel SQL (`'6'` / `'0'`)

---

## UAT (implementation IDs used at merge)

Spec §12 reserved CR009-UAT-038–039 for Inspector. The Phase D implementation prompt used CR009-UAT-031–035. Those live IDs are recorded here. The specification document is not rewritten.

| UAT | Scenario | Result at merge |
|-----|----------|-----------------|
| CR009-UAT-031 | Admin opens Raw Inspector | PASS (code) |
| CR009-UAT-032 | Non-admin blocked | PASS (code) |
| CR009-UAT-033 | Admin edits core | PASS (code) |
| CR009-UAT-034 | Timeline renders | PASS (code) |
| CR009-UAT-035 | Existing lifecycle unaffected | PASS (diff vs `7747a57`) |
