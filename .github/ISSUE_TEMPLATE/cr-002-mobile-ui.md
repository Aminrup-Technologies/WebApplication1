---
name: CR-002 Mobile UI
about: JOBID V2.2 Change Request — mobile-first presentation for field supervisors
title: "[CR-002] Mobile UI"
labels: ["jobid-v2.2", "change-request"]
---

# CR-002 — Mobile UI

**Status:** Draft only. Not approved for implementation.  
**Baseline:** `v2.1.0-jobid-remediation` (`a75bb1e`)  
**Source:** `docs/JOBID_V2.2_ROADMAP.md` Priority 1  
**GitHub label:** `jobid-v2.2`. Security Foundation v2.2 is complete and is a different program.  
**This template is a Change Request draft.** Opening it does not authorize code.

## Background

V2 pages are desktop Gentelella layouts. Supervisors in the field use phones. Small targets, full postbacks, and dense badges slow IN, permit upload, and Close & Send. Remediation did not change presentation.

## Business Objective

Make Create, hub, IN, permit, OUT, and Close usable on a phone-width viewport without changing JOB states or the frozen workflow.

## Technical Scope

- Responsive / touch-first CSS and control sizing on existing V2 `.aspx` markup.
- Readable hub badges (Pending IN, outstanding permit work, Pending OUT).
- Fewer accidental double-taps on Close & Send (keep existing idempotent server lock).
- Optional reduction of full-page postbacks **only if** the same server methods and parameterized SQL run afterward.

Presentation-only work may be reclassified as Enhancement after review. New camera, GPS, or input rules stay in this CR (or move to CR-005).

## Out of Scope

- New JOB states (including “mobile session”).
- Changing inbox, close, approval, or dashboard KPI SQL.
- Native apps, new APIs, or schema.
- Weakening `USERID` / `WORKMAN` (or hub six-key) session checks.
- Implementation PRs until this CR is approved.

## Acceptance Criteria

- [ ] Primary actions are usable at a phone-width viewport (Create, IN submit, permit upload, OUT, Close & Send).
- [ ] Hub badges still mean M6: Pending IN = `EntryExit='Created'`; Pending Permit = `EntryExit='Entry' AND FinalUpldStatus='No'`; Pending OUT = `MasterStatusCode='3' AND EntryExit='Entry'`.
- [ ] Close & Send still writes Out-Punch Done / code 4 / Exit.
- [ ] No new columns or status codes.
- [ ] Desktop layout remains usable.

## UAT Impact

- Visual / device checks in addition to existing UAT-006, UAT-014, UAT-029, UAT-004 / UAT-005.
- No new workflow UAT IDs unless inputs or GPS rules are added.

## Rollback Strategy

Application-only. Revert markup/CSS (and any client scripts). Server predicates unchanged. Do not retag `v2.1.0-jobid-remediation`.

## Change Classification

**Change Request** until scoped as presentation-only.  
If the approved scope is CSS/markup only with identical postbacks, Engineering lead may reclassify as **Enhancement**. New inputs, camera, or GPS escalate and stay Change Request.
