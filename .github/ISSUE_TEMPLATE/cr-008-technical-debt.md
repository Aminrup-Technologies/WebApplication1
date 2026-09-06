---
name: CR-008 Technical Debt
about: JOBID V2.2 Refactor — shared encode helper, status constants, modal, dashboard query (behavior unchanged)
title: "[CR-008] Technical Debt"
labels: ["v2.2", "refactor"]
---

# CR-008 — Technical Debt

**Status:** Draft only. Not approved for implementation.  
**Baseline:** `v2.1.0-jobid-remediation` (`a75bb1e`)  
**Source:** `docs/JOBID_V2.2_ROADMAP.md` §4 Technical Debt  
**Tracked as CR-008 for the V2.2 epic.** Classification is Refactor unless a predicate changes.

## Background

Encode/decode, status strings, Close & Send modals, and dashboard SQL are duplicated across V2 pages. Extracting them reduces drift *if and only if* observable behavior stays identical to v2.1.0.

## Business Objective

No user-facing workflow change. Safer maintenance for later V2.2 enhancements.

## Technical Scope

Allowed (behavior-preserving):

1. Shared `EncodeJobID` / `DecodeJobID` helper — same URL-safe Base64 algorithm. JOB360 may call the helper; AddDocs / SwapDate stay raw unless a separate CR says otherwise.
2. Shared status constants — values must equal current strings/codes (`Created`, `Entry`, `Exit`, `1`/`3`/`4`/`5`, `Out-Punch Done`, etc.).
3. Common modal markup/script — Close still uses `UpdateJOBTable1` with the same writes; `btn_FinalizeShift` ID unchanged in this CR.
4. Dashboard query consolidation — **keep M6 CASE semantics**. Do not fold inbox SQL into Pending Permit.

## Out of Scope

- New columns, SPs, or `MasterStatusCode` values.
- Permit-before-IN.
- Changing JOB360 AddDocs / SwapDate.
- Changing inbox, close, approval, or KPI predicates.
- Implementation PRs until this draft is accepted as a refactor plan.

## Acceptance Criteria

- [ ] Encode/decode vectors match pre-change (including JOB360 UAT-046–049).
- [ ] IN / permit / OUT inbox SQL strings (or equivalent constants) match v2.1.0 predicates.
- [ ] Close & Send still writes Out-Punch Done / 4 / Exit; double-submit still idempotent (UAT-040A / UAT-040B).
- [ ] Hub Pending Permit still `EntryExit='Entry' AND FinalUpldStatus='No'`.
- [ ] Binary/behavior comparison or documented predicate test vs `a75bb1e`.

## UAT Impact

Regression only: UAT-006, UAT-013/018, UAT-014, UAT-029/040, UAT-004/005, UAT-046–049. No new UAT IDs for a pure refactor.

## Rollback Strategy

Application-only. Revert the helper/constants/modal/query move. Do not retag `v2.1.0-jobid-remediation`.

## Change Classification

**Refactor** — Engineering lead; prove behavior unchanged.  
If any predicate, control ID, or encode algorithm changes, stop and open a **Change Request** instead of shipping under CR-008.
