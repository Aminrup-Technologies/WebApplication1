# JOBID V2 Remediation Changelog

Version: v2.1.0-jobid-remediation  
Date: September 2026  
Branch: `Jul_to_Sep_2026_Suport_N_Dev_Works`

Purpose:  
Summarize the complete JOBID remediation program delivered through PR #59–#65.

---

## Release Summary

| Metric | Value |
|--------|-------|
| PRs | 7 |
| Milestones | 6 |
| Schema Changes | None |
| Workflow Changes | Legacy-compatible restoration |
| UAT Coverage | Existing mapped UAT set |

---

## Business Outcome

- ARC jobs immediately eligible for IN.
- Permit uploads support multiple sessions.
- JOB360 navigation corrected.
- Close & Send restores legacy completion.
- Dashboard reflects operational state.
- Approval workflow preserved.

---

## PR Timeline

| PR | Milestone | Purpose |
|----|-----------|---------|
| #59 | M1 | Restore IN-Punch eligibility for permit-required jobs immediately after create. |
| #60 | M1 | Restore last-OUT Close & Send (legacy close states, confirmation modal). |
| #61 | M2 | Restore permit `JOB_Status` / `PermitUpload` writes without reversing IN/OUT/close. |
| #62 | M3 | Encode JOB360 → V2 `jobid` links to match Base64 decode. |
| #63 | M4 | Persist work-order billing and contract nature across create postbacks. |
| #64 | M5 | Keep the permit inbox after first upload (`EntryExit='Entry'`). |
| #65 | M6 | Align hub KPIs; Pending Permit = outstanding work (`FinalUpldStatus='No'`). |

---

## Files Touched

Executable files actually modified in PR #59–#65. Audit markdown updated alongside each PR is omitted here.

| File | PR |
|------|----|
| `WebApplication1/bussiness/production/job_inpunch_v2.aspx.cs` | #59, #62 |
| `WebApplication1/bussiness/production/create_jobid_v2.aspx.cs` | #59 (comments), #63 |
| `WebApplication1/bussiness/production/job_outpunch_v2.aspx` | #60 |
| `WebApplication1/bussiness/production/job_outpunch_v2.aspx.cs` | #60, #62 |
| `WebApplication1/bussiness/production/job_permitupload_v2.aspx.cs` | #61, #62, #64 |
| `WebApplication1/bussiness/production/job_360_view.aspx.cs` | #62 |
| `WebApplication1/bussiness/production/jobs_and_manpower_v2.aspx.cs` | #65 |

V1 pages were not modified. No database schema files were modified.

---

## Business Impact Matrix

| Area | Before | After |
|------|--------|-------|
| IN | Permit-required jobs waited for status code 3 before appearing. | Created Active jobs appear on IN-Punch immediately. Duplicate Entry rules unchanged. |
| Permit | Status fields incomplete; first upload dropped the job from the inbox. | Permit Yes/No and Permit Uploaded stay consistent. Supervisors can add more files after leaving and returning. |
| OUT | Last OUT did not close; forgotten Finalize blocked approval. | Last OUT prompts Close & Send. Close writes Out-Punch Done / code 4 / Exit. |
| Dashboard | Counts used pre-IN status-code gates; permit badge matched the wrong idea of “pending.” | Pending IN = not yet punched. Pending Permit = outstanding permit work. Pending OUT clears at Close & Send. |
| JOB360 | Raw JOBID broke V2 Permit / IN / OUT. | Selected job opens on the V2 page. Existing encoded links still work. |
| Billing | Non-Billing / ARC natures were lost after postback. | Work-order billing and contract nature persist through save and location bind. |

---

## UAT Coverage

Existing completed IDs only.

### Core Workflow

- UAT-006
- UAT-021
- UAT-010
- UAT-011
- UAT-012
- UAT-029
- UAT-040
- UAT-040A
- UAT-040B
- UAT-035

### Permit

- UAT-013
- UAT-018
- UAT-018A
- UAT-019
- UAT-014
- UAT-015
- UAT-015A
- UAT-015B

### Navigation

- UAT-046
- UAT-047
- UAT-048
- UAT-049

### Dashboard

- UAT-004
- UAT-005
- UAT-035
- UAT-035A
- UAT-035B

---

## Production Notes

No database schema changes.

No new JOB status codes.

No approval redesign.

Rollback remains application-only.

---

## Companion Documents

- `docs/JOBID_LIFECYCLE_FUNCTIONAL_AUDIT.md`
- `docs/RELEASE_READINESS_PACK_v1.0.md`

This changelog is the official release manifest for v2.1.0-jobid-remediation.
