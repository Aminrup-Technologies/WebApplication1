---
name: CR-003 Supervisor Dashboard
about: JOBID V2.2 Change Request — clearer outstanding-work hub without changing M6 KPI meaning
title: "[CR-003] Supervisor Dashboard"
labels: ["jobid-v2.2", "change-request"]
---

# CR-003 — Supervisor Dashboard

**Status:** Draft only. Not approved for implementation.  
**Baseline:** `v2.1.0-jobid-remediation` (`a75bb1e`)  
**Source:** `docs/JOBID_V2.2_ROADMAP.md` Priority 1  
**GitHub label:** `jobid-v2.2`. Security Foundation v2.2 is complete and is a different program.  
**This template is a Change Request draft.** Opening it does not authorize code.

## Background

M6 aligned hub `LoadDashboardStats()` so Pending Permit means outstanding work, not the permit inbox. The hub is still a set of badges and app buttons. Supervisors cannot see *which* jobs sit in each bucket without opening each V2 page.

## Business Objective

Show outstanding IN, outstanding permit work, pending OUT, and recently closed jobs more clearly, without changing what the numbers mean.

## Technical Scope

- Hub UI on `jobs_and_manpower_v2.aspx` / `.aspx.cs` (lists, labels, filters) **or** equivalent presentation on top of the same CASE query.
- Preserve M6 predicates (creator `@Workman`, Active, 3-day window where currently used):
  - Pending IN: `EntryExit='Created'`
  - Pending Permit: `EntryExit='Entry' AND FinalUpldStatus='No'`
  - Pending OUT: `MasterStatusCode='3' AND EntryExit='Entry'`
- Optional “recently closed” display using existing close states (`Out-Punch Done` / code 4 / `Exit`) without a new status.
- Do not merge permit-inbox SQL (`EntryExit='Entry'` only) into the Pending Permit badge.

## Out of Scope

- Redefining KPI meaning.
- Counting skip-permit (`FinalUpldStatus='Yes'`) jobs as outstanding permit work.
- New dashboard tables or columns.
- Approval-queue redesign (`view_jobsforapproval` stays Out-Punch Done + Exit).
- Implementation PRs until this CR is approved.

## Acceptance Criteria

- [ ] Badge or list counts match M6 CASE semantics.
- [ ] After first permit upload, Pending Permit drops; permit page still lists the job (UAT-035A).
- [ ] After Close & Send, Pending OUT drops (UAT-004 / UAT-035).
- [ ] Created ARC and skip-permit jobs appear in Pending IN, not Pending Permit.
- [ ] V1 hub SQL is unchanged unless a separate CR says otherwise.

## UAT Impact

- UAT-004, UAT-005, UAT-035, UAT-035A, UAT-035B
- UAT-006 (Pending IN includes code-1 jobs)
- Visual checks that lists and badges agree

## Rollback Strategy

Application-only. Restore current hub markup and `LoadDashboardStats()` CASE query from `a75bb1e`. No schema rollback. Do not retag `v2.1.0-jobid-remediation`.

## Change Classification

**Change Request** if lists, filters, or query shape change.  
Copy-only badge labels with identical SQL may be **Enhancement**. Any change to CASE meaning remains Change Request and is out of the Enhancement path.
