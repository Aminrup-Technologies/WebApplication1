# JOBID V2 Release Readiness Pack

Version: 1.0  
Baseline: September 2026  
Branch: `Jul_to_Sep_2026_Suport_N_Dev_Works`

Source of Truth:  
`docs/JOBID_LIFECYCLE_FUNCTIONAL_AUDIT.md`

Purpose:  
This document freezes the production-ready baseline established by PR #59–#65.

---

## Section 1 — Executive Summary

The forensic remediation program for JOBID V2 is complete. The restored operational path matches legacy: Create → IN-Punch → Permit Upload → OUT-Punch → Close & Send → Approval. No database schema was added, removed, or altered. Stored-procedure call contracts are unchanged. V2 presentation, parameterized SQL, Base64 JOBID links, confirmation-based Close & Send, and matrix-driven create remain in place.

| Area | Result |
|------|--------|
| Workflow | Restored |
| Navigation | Fixed |
| Permit | Stabilized |
| Dashboard | Realigned |
| Approval | Preserved |
| Schema | Unchanged |

---

## Section 2 — Milestone Timeline

| Milestone | PR | Outcome |
|-----------|----|---------|
| M1 | #59–60 | Legacy State Machine |
| M2 | #61 | Permit State Consistency |
| M3 | #62 | JOB360 Navigation |
| M4 | #63 | WO Persistence |
| M5 | #64 | Permit Inbox |
| M6 | #65 | Dashboard Alignment |

---

## Section 3 — Restored Business Workflow

```
Create
→ IN-Punch
→ Permit Upload
→ Additional Permit Uploads
→ OUT-Punch
→ Close & Send
→ Approval
```

**Create.** The supervisor creates a JOBID. Permit-required (ARC) jobs are immediately eligible for IN-Punch. Skip-permit jobs still skip the permit requirement at insert.

**IN-Punch.** Manpower is recorded as Entry. Duplicate Entry protection is unchanged. After IN, `EntryExit` becomes Entry.

**Permit Upload.** After IN, the supervisor uploads permit files. The first file records permit complete on the job. The permit page still lists the job so further files can be added in later sessions.

**Additional Permit Uploads.** Leaving and returning does not drop an in-progress job from the permit inbox while it remains Active and Entry.

**OUT-Punch.** Exit punches are recorded individually. The last open Entry prompts Close & Send. Review Again does not close the job.

**Close & Send.** Writes the same close states as legacy: Out-Punch Done, status code 4, Exit. The job leaves pending OUT and matches the approval list. Repeat submit is safe.

**Approval.** Site In-Charge approval continues to use Out-Punch Done and Exit. That gate was not redesigned.

---

## Section 4 — Technical Traceability Matrix

| PR | Files | Finding |
|----|-------|---------|
| #59 | `job_inpunch_v2.aspx.cs`; `create_jobid_v2.aspx.cs` (comments only) | IN-Punch listed only `MasterStatusCode='3'`, so permit-required jobs could not be punched immediately after create. |
| #60 | `job_outpunch_v2.aspx`; `job_outpunch_v2.aspx.cs` | Last OUT did not close the job. Forgotten Finalize left jobs off approval and on pending OUT. |
| #61 | `job_permitupload_v2.aspx.cs` | Permit upload/delete did not keep `JOB_Status` / `PermitUpload` consistent with file count, without rolling back IN/OUT/close. |
| #62 | `job_360_view.aspx.cs`; `job_inpunch_v2.aspx.cs`; `job_outpunch_v2.aspx.cs`; `job_permitupload_v2.aspx.cs` | JOB360 passed raw JOBID into V2 pages that decode Base64, causing failed or wrong navigation. |
| #63 | `create_jobid_v2.aspx.cs` | `WO_BillingNature` and `WO_ContractNature` were never written to ViewState, so Non-Billing save and ARC location binding broke after postback. |
| #64 | `job_permitupload_v2.aspx.cs` | Permit inbox required `MasterStatusCode='1'`. First upload wrote code 3, so the JOB disappeared and further files could not be added after leaving the page. |
| #65 | `jobs_and_manpower_v2.aspx.cs` | Hub KPIs still used pre-IN status-code gates. Pending Permit was then separated from the inbox: outstanding work is `EntryExit='Entry'` and `FinalUpldStatus='No'`. |

Each PR also updated `docs/JOBID_LIFECYCLE_FUNCTIONAL_AUDIT.md`. No schema scripts were included.

---

## Section 5 — UAT Certification Matrix

Recorded IDs only. Do not treat this list as a new test design.

### Core Workflow

| ID | Area | Result |
|----|------|--------|
| UAT-006 | Permit-required jobs appear on IN-Punch immediately | Certified |
| UAT-021 | Duplicate Entry protection unchanged | Certified |
| UAT-010 | Billing work order natures persist through save | Certified |
| UAT-011 | Non-Billing save writes Non-Billing / NB | Certified |
| UAT-012 | ARC location binding uses persisted contract nature | Certified |
| UAT-029 | Last OUT opens Close & Send confirmation | Certified |
| UAT-040 | Close & Send writes Out-Punch Done / code 4 / Exit | Certified |
| UAT-040A | Double-click does not double-close | Certified |
| UAT-040B | Refresh during close is idempotent | Certified |
| UAT-035 | After close, pending OUT decreases and approval can see the job | Certified |

### Permit

| ID | Area | Result |
|----|------|--------|
| UAT-013 | `JOB_Status` set to Permit Uploaded | Certified |
| UAT-018 | `PermitUpload` Yes/No follows file count | Certified |
| UAT-018A | Last-file delete after IN does not roll MasterStatusCode to 1 | Certified |
| UAT-019 | Out-Punch Done is not overwritten | Certified |
| UAT-014 | After first upload, leave and return: JOB still in inbox | Certified |
| UAT-015 | Second upload increments FileCount | Certified |
| UAT-015A | Repeated leave/return while Active + Entry | Certified |
| UAT-015B | Closed jobs (`EntryExit='Exit'`) leave the inbox | Certified |

### Navigation

| ID | Area | Result |
|----|------|--------|
| UAT-046 | JOB360 Permit opens the selected V2 JOBID | Certified |
| UAT-047 | JOB360 IN opens the selected V2 JOBID | Certified |
| UAT-048 | JOB360 OUT opens the selected V2 JOBID | Certified |
| UAT-049 | Encoded links still work; invalid/empty jobid does not throw; V1 unchanged | Certified |

### Dashboard

| ID | Area | Result |
|----|------|--------|
| UAT-004 | Close & Send removes the job from Pending OUT | Certified |
| UAT-005 | Pending Permit counts outstanding work only (`EntryExit='Entry'` and `FinalUpldStatus='No'`) | Certified |
| UAT-035 | Dashboard refresh after close: Pending OUT decreases | Certified |
| UAT-035A | First permit upload drops the Pending Permit badge; inbox still allows more files | Certified |
| UAT-035B | Approval list is Out-Punch Done + Exit; no hub approval badge | Certified |

---

## Section 6 — Deployment Checklist

### Pre-Deployment

- [ ] Confirm target branch is `Jul_to_Sep_2026_Suport_N_Dev_Works` with PR #59–#65 included
- [ ] Confirm no pending schema or stored-procedure deployments
- [ ] Back up current IIS site files and `web.config`
- [ ] Confirm SQL connection strings, file share for `~/erp_images/Permits/`, and notification settings
- [ ] Schedule a supervisor smoke window covering Create, IN, Permit, OUT, Close & Send, Approval, JOB360, dashboard
- [ ] Brief operations: IN is allowed immediately for permit-required jobs; last OUT uses Close & Send; extra permit files remain possible after the first upload

### Deployment

- [ ] Deploy the WebForms application binaries and markup (IIS site / app pool)
- [ ] Recycle the application pool
- [ ] Confirm the site comes up on the expected URL
- [ ] Confirm `web.config` connection strings and app settings match the previous environment
- [ ] Confirm permit disk path is writable
- [ ] No SQL scripts to run

### Smoke Test

- [ ] Login as a supervisor with a valid session
- [ ] Create a permit-required JOBID and confirm it appears on IN-Punch
- [ ] IN-Punch at least one worker
- [ ] Upload a permit, leave the page, return, and confirm the JOB is still listed
- [ ] Upload a second permit and confirm FileCount increases
- [ ] OUT-Punch remaining workers; on last OUT confirm Close & Send
- [ ] Confirm Close & Send success; job appears for Site In-Charge approval
- [ ] Confirm dashboard Pending OUT dropped and Pending Permit dropped after the first successful upload
- [ ] From JOB360, open Permit / IN / OUT for the same JOBID without a decode error
- [ ] Confirm no unexpected errors in IIS logs or application error pages

---

## Section 7 — Rollback Plan

No database schema rollback is required. PR #59–#65 did not add, drop, or alter tables or stored-procedure contracts. Job rows written under this baseline remain valid if the previous application bits are restored.

Rollback is application deployment only:

1. Redeploy the previously known-good site package (binaries, `.aspx`, `.aspx.cs` compiled output, views).
2. Restore the previous `web.config` if it was replaced.
3. Recycle the IIS application pool.
4. Confirm login, Create, IN, Permit, OUT, and Approval still operate on the prior package.
5. Do not run reverse SQL. Do not delete JOBID rows created during the failed release unless operations explicitly request data cleanup.

---

## Section 8 — Hypercare Plan

### First 24 hours

Watch every live shift through one full cycle: create, IN, permit (including a second file on at least one job), OUT, Close & Send, and one approval. Confirm dashboard badges move in the same direction as the pages. Capture any decode, timeout, or 500 errors immediately.

### 24–48 hours

Spot-check skip-permit (non-ARC) create, Non-Billing save, JOB360 deep links after close, and a job that left permit and returned later. Confirm closed jobs do not remain on Pending OUT or the permit inbox.

### Monitor

| Area | Watch for |
|------|-----------|
| Create | Failed save, missing billing on Non-Billing WO, unexpected redirect |
| IN | Permit-required jobs missing from inbox; duplicate Entry blocks |
| Permit | Job disappearing after first file; FileCount not incrementing |
| OUT | Last OUT without Close & Send prompt; close not reaching approval |
| Approval | Jobs missing until a second action; jobs appearing before Close & Send |
| Dashboard | Pending Permit still counting completed jobs; Pending OUT stuck after close |
| Errors | IIS / application exceptions, Base64/decode failures, SQL timeouts |

---

## Section 9 — Client Change Log

What supervisors and Site In-Charge will notice after this baseline:

- **Immediate ARC IN.** A newly created permit-required job can be IN-punched right away. Permit files can follow.
- **Multi-session Permit.** After the first permit file, the job remains available so the gang can add more files later the same day.
- **Close & Send.** After the last OUT, the supervisor confirms Close & Send. That is the step that sends the shift to the Site In-Charge. Review Again only pauses.
- **Reliable JOB360.** Permit, IN, and OUT actions from JOB360 open the selected job on the new screens.
- **Dashboard accuracy.** Pending IN, outstanding permit work, and pending OUT follow the same rules as the work queues. Completed permits do not keep the permit badge lit.
- **WO persistence.** Billing and non-billing work-order choices remain in effect through the rest of the create screen and save.

The new screens, GPS/title rules, and notifications introduced with V2 are unchanged by this freeze.

---

## Section 10 — Production Baseline

This freeze is the production-ready JOBID V2 baseline.

| Field | Value |
|--------|-------|
| Version | 1.0 |
| Date | September 2026 |
| Tag | `v2.1.0-jobid-remediation` |
| Executable baseline | `a75bb1e` |
| PRs | #59–#65 |
| Workflow | Create → IN → Permit → OUT → Close → Approval |
| Schema Changes | None |

This document, together with JOBID_LIFECYCLE_FUNCTIONAL_AUDIT.md, forms the official production baseline for future enhancements.

PR #66 packages these governance files into the branch. It does not modify `a75bb1e` or the release tag.
