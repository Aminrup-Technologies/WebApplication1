---
name: CR-001 Unified Wizard
about: JOBID V2.2 Change Request — single supervisor path through Create, IN, Permit, OUT, Close
title: "[CR-001] Unified Wizard"
labels: ["v2.2", "change-request"]
---

# CR-001 — Unified Wizard

**Status:** Draft only. Not approved for implementation.  
**Baseline:** `v2.1.0-jobid-remediation` (`a75bb1e`)  
**Source:** `docs/JOBID_V2.2_ROADMAP.md` Priority 1  
**This template is a Change Request draft.** Opening it does not authorize code.

## Background

V2 supervisors move between separate pages (`create_jobid_v2`, `job_inpunch_v2`, `job_permitupload_v2`, `job_outpunch_v2`) plus the hub (`jobs_and_manpower_v2`). Remediation M1–M6 restored the frozen lifecycle on those pages. Field users still leave one screen and re-select the JOBID on the next.

A unified wizard would present one path through the same steps without replacing the v2.1.0 state machine.

## Business Objective

Let a supervisor complete Create → IN → Permit (including additional files) → OUT → Close & Send without losing the job or inventing a new terminal state. Approval remains Site In-Charge work on the existing approval list.

## Technical Scope

- Presentation shell that sequences the existing V2 steps.
- Reuse current create, IN, permit, OUT, and Close & Send writes.
- Preserve IN eligibility (no `MasterStatusCode='3'` gate).
- Preserve permit inbox `JOBID_Status='Active' AND EntryExit='Entry'`.
- Preserve Close & Send: `UpdateJOBTable1` → `JOB_Status='Out-Punch Done'`, `MasterStatusCode='4'`, `EntryExit='Exit'`. Control ID `btn_FinalizeShift` stays unless this CR is amended.
- JOB360 Permit / IN / OUT continue to encode with the existing `EncodeJobID` contract.

## Out of Scope

- Permit-before-IN as a required gate.
- New `JOB_Status` or `MasterStatusCode` values.
- Replacing Close & Send with auto-close or a new “wizard complete” state.
- Schema or stored-procedure changes.
- Changing approval predicates (`JOB_Status='Out-Punch Done' AND EntryExit='Exit'`).
- Implementation PRs until this CR is approved.

## Acceptance Criteria

- [ ] Wizard step order is Create → IN → Permit → OUT → Close.
- [ ] ARC jobs remain IN-eligible immediately after create.
- [ ] After first permit upload, the job remains available for additional files while Active + Entry.
- [ ] Last OUT still opens Close & Send confirmation; Review Again does not close.
- [ ] Closed jobs match the approval list; Pending OUT drops.
- [ ] No new JOB states.
- [ ] v2.1.0 tag `a75bb1e` is not rewritten.

## UAT Impact

Reuse existing IDs; do not treat this CR as a new test design until approved.

- UAT-006 / UAT-021 (IN eligibility, duplicate Entry)
- UAT-014 / UAT-015 / UAT-015A / UAT-015B (permit continuity)
- UAT-029 / UAT-040 / UAT-035 (Close & Send)
- UAT-046–049 (JOB360) if wizard deep-links are added

New UAT IDs only if the approved CR adds uncovered wizard-only behavior.

## Rollback Strategy

Application-only. Revert the wizard shell and restore hub-tile navigation to the existing V2 pages. No schema rollback. Do not retag `v2.1.0-jobid-remediation`.

## Change Classification

**Change Request** — new supervisor path over the frozen lifecycle. Engineering-lead Enhancement is not sufficient if navigation or step order changes.
