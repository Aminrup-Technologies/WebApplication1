---
name: CR-007 Reporting and Analytics
about: JOBID V2.2 Change Request — reports and KPIs from existing JOB states, not new ones
title: "[CR-007] Reporting and Analytics"
labels: ["v2.2", "change-request"]
---

# CR-007 — Reporting and Analytics

**Status:** Draft only. Not approved for implementation.  
**Baseline:** `v2.1.0-jobid-remediation` (`a75bb1e`)  
**Source:** `docs/JOBID_V2.2_ROADMAP.md` Priority 3 (Reporting, Analytics, Productivity KPIs)  
**This template is a Change Request draft.** Opening it does not authorize code.

## Background

Hub badges are 3-day creator operational counts (M6). Office staff and Site In-Charge need period reporting and trends (time-to-IN, time-to-close, permit completion, close-to-approval lag) without a second status model.

## Business Objective

Provide read-only reports and derived productivity measures from existing `tbl_jobs` / attendance columns, complementary to hub badges—not a replacement for them.

## Technical Scope

- Reads against existing JOBID and attendance data (parameterized).
- Period filters (month / date range) for jobs the user is already authorized to see.
- Derived metrics from current states only (`Created` / `Entry` / `Exit`, codes 1/3/4, `FinalUpldStatus`, `JOB_Status`).
- Hub CASE query remains M6 unless CR-003 is approved separately.
- New report *tables* require an amended CR with schema plan and rollback.

## Out of Scope

- New `MasterStatusCode` or `JOB_Status` values for analytics buckets.
- Changing Pending Permit to match the permit inbox.
- Replacing `view_jobsforapproval` predicates.
- Unbounded exports of other creators’ jobs.
- Implementation PRs until this CR is approved.

## Acceptance Criteria

- [ ] Reports can be explained using v2.1.0 states only.
- [ ] Hub Pending IN / Permit / OUT meanings unchanged.
- [ ] Closed jobs appear in close/approval reporting when `Out-Punch Done` + `Exit`, not while still Entry.
- [ ] Skip-permit jobs are not counted as outstanding permit work.
- [ ] Authorization is not weaker than existing pages.

## UAT Impact

- UAT-004 / UAT-005 / UAT-035 (hub unchanged)
- UAT-035B (approval still Close & Send)
- Report-specific checks only after approval; no new UAT IDs in this draft

## Rollback Strategy

Application-only if no new tables. If tables were added under an amendment, drop them per that plan. Do not retag `v2.1.0-jobid-remediation`.

## Change Classification

**Change Request** — new reporting surface.  
New tables or SPs remain Change Request with schema plan. Read-only pages on existing columns still need this CR before implementation.
