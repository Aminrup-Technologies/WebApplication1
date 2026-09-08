---
name: CR-005 GPS Validation
about: JOBID V2.2 Change Request — stronger create/submit location checks without new JOB states
title: "[CR-005] GPS Validation"
labels: ["jobid-v2.2", "change-request"]
---

# CR-005 — GPS Validation

**Status:** Draft only. Not approved for implementation.  
**Baseline:** `v2.1.0-jobid-remediation` (`a75bb1e`)  
**Source:** `docs/JOBID_V2.2_ROADMAP.md` Priority 2  
**GitHub label:** `jobid-v2.2`. Security Foundation v2.2 is complete and is a different program.  
**This template is a Change Request draft.** Opening it does not authorize code.

## Background

Create V2 already has region-driven GPS sharing at submit for some work. Failures or weak checks do not use a dedicated JOB status. Stronger validation must not invent “GPS failed” as a `JOB_Status` or `MasterStatusCode`.

## Business Objective

Block or warn on create/submit when required location is missing or implausible, while still inserting the same JOB row shape as v2.1.0 when submit succeeds.

## Technical Scope

- Client and/or server checks on existing create GPS fields for regions that already require GPS.
- On failure: existing notification / submit-block path, not a new persisted state.
- Successful insert still uses `SP_InsertInto_JOBSTable` with current `@EntryExit='Created'` and matrix `MasterStatusCode` / `FinalUpldStatus`.
- WO nature ViewState (M4) unchanged.

## Out of Scope

- New status codes or columns for GPS outcome (unless this CR is amended with a schema plan).
- Changing IN eligibility or permit-before-IN.
- Tracking pings after create as a new workflow step.
- Implementation PRs until this CR is approved.

## Acceptance Criteria

- [ ] Regions that require GPS cannot save without a valid fix (or the approved warn-and-continue behavior).
- [ ] Regions that do not require GPS are unchanged.
- [ ] Successful create still appears in IN immediately for permit-required jobs (UAT-006).
- [ ] No new `JOB_Status` / `MasterStatusCode` values.
- [ ] Billing / Non-Billing / ARC natures still persist (UAT-010 / UAT-011 / UAT-012).

## UAT Impact

- UAT-006, UAT-010, UAT-011, UAT-012
- Region-flagged GPS cases only after approval

## Rollback Strategy

Application-only. Revert create GPS checks; keep insert contract. No schema rollback unless a later amendment added columns (then the amendment must include rollback). Do not retag `v2.1.0-jobid-remediation`.

## Change Classification

**Change Request** — create validation rules. Not a Hotfix. Not a silent Enhancement if submit is blocked in new cases.
