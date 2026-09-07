---
name: CR-006 Permit Checklist
about: JOBID V2.2 Change Request — remaining permit files UI without breaking M2 writes or M5 inbox
title: "[CR-006] Permit Checklist"
labels: ["jobid-v2.2", "change-request"]
---

# CR-006 — Permit Checklist

**Status:** Draft only. Not approved for implementation.  
**Baseline:** `v2.1.0-jobid-remediation` (`a75bb1e`)  
**Source:** `docs/JOBID_V2.2_ROADMAP.md` Priority 2  
**GitHub label:** `jobid-v2.2`. Security Foundation v2.2 is complete and is a different program.  
**This template is a Change Request draft.** Opening it does not authorize code.

## Background

Permit V2 records FileCount, `FinalUpldStatus`, `PermitUpload`, and `JOB_Status='Permit Uploaded'` via `UpdatePermitStatus` (M2). The inbox lists Active + `EntryExit='Entry'` (M5). Supervisors do not get a remaining-file checklist before they leave the page. A checklist must not drop the job from the inbox after the first file.

## Business Objective

Show which permit types or remaining files are still expected, while the first successful file still marks permit complete on the job and the JOB stays selectable for more uploads.

## Technical Scope

- UI on `job_permitupload_v2` (or equivalent) listing required vs uploaded types using existing document/permit rows.
- `UpdatePermitStatus` / `ReadPermitJobState` / `InsertIntoDB` write rules unchanged:
  - `PermitUpload` / `JOB_Status` follow file count
  - do not overwrite Out-Punch Done
  - do not roll `MasterStatusCode` to 1 after IN
- Inbox SQL stays `JOBID_Status='Active' AND EntryExit='Entry'` (creator, 3-day window, parameterized).
- Dashboard Pending Permit still uses `FinalUpldStatus='No'` (M6), not the inbox predicate.

## Out of Scope

- Requiring permit before IN.
- Changing inbox to `MasterStatusCode='1'`.
- New permit status codes.
- Schema for a new checklist table unless this CR is amended with a plan and rollback.
- Implementation PRs until this CR is approved.

## Acceptance Criteria

- [ ] Checklist is informational (or blocks only as this CR later specifies) without removing the job from the M5 inbox after first upload.
- [ ] First file still sets `FinalUpldStatus='Yes'` and hub Pending Permit drops (UAT-035A).
- [ ] Second file increments FileCount (UAT-015).
- [ ] Last-file delete after IN does not set `MasterStatusCode='1'` (UAT-018A).
- [ ] Closed jobs (`EntryExit='Exit'`) leave the inbox (UAT-015B).

## UAT Impact

- UAT-013, UAT-018, UAT-018A, UAT-019
- UAT-014, UAT-015, UAT-015A, UAT-015B
- UAT-035A

## Rollback Strategy

Application-only. Remove checklist UI; keep M2 writes and M5 inbox. Do not retag `v2.1.0-jobid-remediation`.

## Change Classification

**Change Request** if the checklist can block submit or change required file rules.  
Read-only remaining-count display with identical writes may be **Enhancement** after Engineering-lead review.
