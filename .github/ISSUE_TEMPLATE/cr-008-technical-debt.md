---
name: CR-008 Technical Debt
about: JOBID V2.2 Refactor — shared encode helper, status constants, modal, dashboard query (behavior unchanged)
title: "[CR-008] Technical Debt"
labels: ["jobid-v2.2", "refactor"]
---

# CR-008 — Technical Debt

**Status:** Draft only for **remaining** debt. CR-010 already shipped shared helpers.  
**JOBID baseline:** `v2.1.0-jobid-remediation` (`a75bb1e`)  
**CR-010 close-out:** `docs/CR-010_CLOSEOUT.md`  
**Source:** `docs/JOBID_V2.2_ROADMAP.md` §4 Technical Debt  
**GitHub label:** `jobid-v2.2`. Security Foundation v2.2 is complete and is a different program.  
**Tracked as CR-008 for the JOBID V2.2 epic.** Classification is Refactor unless a predicate changes.

## Background

CR-010 already extracted `JobIdCodec`, `JobStatusConstants` (`App_Code`), `NotificationHelper`, JOB360 cockpit CSS, and unreachable `_OLD` handlers. Do not land a second constants class (rejected PR #80) or a second codec.

Remaining duplication (Close & Send modal markup, dashboard query copies) may still be extracted **if and only if** observable behavior stays identical to v2.1.0 / CR-010.

## Business Objective

No user-facing workflow change. Safer maintenance for later JOBID V2.2 enhancements.

## Technical Scope

Already done (out of this CR):

- `JobIdCodec` (PR #79)
- `JobStatusConstants` in `App_Code` (PR #81) — names `EntryExitCreated`, `CodeCreated`, …
- `NotificationHelper` + `job360-cockpit.css` (PR #82)
- Unreachable JOB360 `_OLD` handlers (PR #83)

Still allowed (behavior-preserving):

1. Common Close & Send modal markup/script — Close still uses `UpdateJOBTable1` with the same writes; `btn_FinalizeShift` ID unchanged in this CR.
2. Dashboard query consolidation — **keep M6 CASE semantics**. Do not fold inbox SQL into Pending Permit.

Do not re-extract encode/decode or status constants.

## Out of Scope

- New columns, SPs, or `MasterStatusCode` values.
- Permit-before-IN.
- Changing JOB360 AddDocs / SwapDate.
- Changing inbox, close, approval, or KPI predicates.
- Replacing `App_Code/JobStatusConstants.cs` with a second file under `bussiness/production/`.
- Implementation PRs until this draft is accepted as a remaining-debt refactor plan.

## Acceptance Criteria

- [ ] No second `JobStatusConstants` or `JobIdCodec` type.
- [ ] IN / permit / OUT inbox SQL strings (or equivalent constants) match v2.1.0 predicates.
- [ ] Close & Send still writes Out-Punch Done / 4 / Exit; double-submit still idempotent (UAT-040A / UAT-040B).
- [ ] Hub Pending Permit still `EntryExit='Entry' AND FinalUpldStatus='No'`.
- [ ] JOB360 UAT-046–049 still pass.

## UAT Impact

Regression only: UAT-006, UAT-013/018, UAT-014, UAT-029/040, UAT-004/005, UAT-046–049. No new UAT IDs for a pure refactor.

## Rollback Strategy

Application-only. Revert the remaining helper/modal/query move. Do not retag `v2.1.0-jobid-remediation`.

## Change Classification

**Refactor** — Engineering lead; prove behavior unchanged.  
If any predicate, control ID, or encode algorithm changes, stop and open a **Change Request** instead of shipping under CR-008.
