# JOBID V2.2 Roadmap

Status: Planning only  
Date: September 2026  
Branch: `Jul_to_Sep_2026_Suport_N_Dev_Works`

This document is a product and engineering plan. It does not change executable code, workflow predicates, schema, or the frozen v2.1.0 baseline.

---

## 1. Executive Summary

**v2.1.0 is the frozen production baseline.**

The JOBID V2 remediation program (milestones M1–M6, PR #59–#65) restored the intended operational path and is tagged as `v2.1.0-jobid-remediation` at executable commit `a75bb1e`. That tag must remain immutable. Future work must not rewrite those predicates silently.

**V2.2 begins after remediation.**

V2.2 is an enhancement program on top of the frozen lifecycle, not a second remediation. It may improve presentation, supervisor experience, and maintainability. It may not invert Create → IN → Permit → OUT → Close & Send → Approval, introduce new JOB states without a Change Request, or break the JOB360 encode/decode contract.

Control documents for the freeze:

- `docs/JOBID_LIFECYCLE_FUNCTIONAL_AUDIT.md` — functional specification (M1–M6)
- `docs/RELEASE_READINESS_PACK_v1.0.md` — production freeze pack
- `docs/JOBID_CHANGELOG_v2.1.0.md` — release manifest
- `docs/MAINTENANCE_GUIDELINES.md` — change classification and protection rules

---

## 2. Guiding Principles

- **Preserve Create → IN → Permit → OUT → Close → Approval.** Permit-before-IN is not a required gate. Additional permit files remain possible while the job is Active and `EntryExit='Entry'`. Close & Send remains the approval gate (`JOB_Status='Out-Punch Done'`, `MasterStatusCode='4'`, `EntryExit='Exit'`).
- **No new JOB states without a Change Request.** Do not add status codes, rename `JOB_Status` values, or change inbox / dashboard / approval predicates without a formal CR. See `docs/MAINTENANCE_GUIDELINES.md`.
- **Maintain the JOB360 contract.** V2 Permit / IN / OUT pages decode URL-safe Base64 `jobid`. Producers encode with the existing algorithm. Base64 is not authorization. AddDocs / SwapDate remain non-V2 raw-JOBID links unless a CR covers them.
- **Do not reuse the permit inbox as the Pending Permit badge.** Inbox = `EntryExit='Entry'`. Outstanding permit work = `EntryExit='Entry' AND FinalUpldStatus='No'`.
- **No schema or stored-procedure changes** unless a CR includes a schema plan and rollback. Rollback for V2.2 enhancements should remain application-only wherever possible.
- **Classify every change** (Hotfix / Enhancement / Change Request / Refactor / Documentation) before implementation.

---

## 3. Enhancement Backlog

Items below are proposals. None are approved for implementation by this document. Each item that would change workflow, encoding, inbox, close, approval, or KPI meaning requires a Change Request first.

### Priority 1

| Item | Intent | Constraint |
|------|--------|------------|
| Unified Wizard | One supervisor path through Create, IN, Permit, OUT, and Close, instead of separate V2 pages plus hub tiles. | Same state machine and page contracts underneath. Do not require permit before IN. Do not replace Close & Send with a new terminal state. |
| Mobile-first UI | Touch-first layout for field use (large targets, fewer full postbacks, readable badges). | Presentation only unless a CR covers new inputs or GPS/camera rules. |
| Better supervisor dashboard | Clearer outstanding work: pending IN, outstanding permits, pending OUT, recently closed. | Preserve M6 KPI meanings. Do not count the permit inbox as Pending Permit. |

### Priority 2

| Item | Intent | Constraint |
|------|--------|------------|
| QR code | Scan a JOBID (or encoded `jobid`) to open the correct V2 step. | Must produce the same encode/decode contract as JOB360 → V2. QR is not authorization. |
| GPS validation | Stronger create/submit location checks for regions that already require GPS. | Do not add new JOB states for “GPS failed.” Failure remains a submit block or existing notification path unless a CR says otherwise. |
| Permit checklist | Show remaining required files / types before the supervisor leaves the permit page. | FileCount / `PermitUpload` / `JOB_Status` rules from M2 stay in force. Checklist UI must not drop the job from the M5 inbox. |

### Priority 3

| Item | Intent | Constraint |
|------|--------|------------|
| Reporting | Shift and JOBID reports for Site In-Charge / office staff over a chosen period. | Read existing `tbl_jobs` / attendance columns. New report tables need a CR. |
| Analytics | Trends: time-to-IN, time-to-close, permit completion, close-to-approval lag. | Derived from current states. Do not invent new status codes for analytics. |
| Productivity KPIs | Supervisor and site throughput measures that complement (not replace) hub badges. | Hub Pending IN / Permit / OUT meanings stay as in M6. |

---

## 4. Technical Debt

Refactors that keep observable behavior unchanged are allowed as Engineering-lead work per the maintenance guidelines. Prove predicates and redirects are identical.

| Debt | Why | Safe approach |
|------|-----|----------------|
| Shared `EncodeJobID` / `DecodeJobID` helper | The same Base64 URL-safe pair is duplicated on create, permit, IN, OUT. | Extract one helper. Algorithm unchanged. JOB360 continues to call `create_jobid_v2.EncodeJobID()` or the extracted equivalent. |
| Shared status constants | Magic strings (`Created`, `Entry`, `Exit`, codes `1`/`3`/`4`/`5`, `Out-Punch Done`) are copied across pages. | Constants or a single names file. Values must match the frozen baseline exactly. |
| Common modal component | Close & Send confirmation/success and other PNotify/modals are page-local. | Shared markup/script. Close still calls `UpdateJOBTable1` with the same writes; control ID `btn_FinalizeShift` stays unless a CR renames it. |
| Dashboard query consolidation | Hub `LoadDashboardStats()` is one SQL CASE query; related counts exist on V1 hub and inbox pages. | Keep M6 CASE semantics. Do not merge inbox SQL into the Pending Permit badge. |

Out of scope for “debt only”: new columns, new SPs, new `MasterStatusCode` values, permit-before-IN, or changing JOB360 AddDocs/SwapDate without a CR.

---

## 5. Release Strategy

Follow `docs/MAINTENANCE_GUIDELINES.md` versioning. Do not silently rewrite the frozen lifecycle.

| Release | Type | Allowed work | Not allowed |
|---------|------|--------------|-------------|
| **v2.1.1** | Patch only | Hotfixes that restore documented v2.1.0 behavior (decode exceptions on legitimate tokens, Close & Send double-submit, copy/logging that does not change states). | Enhancements, new wizard, KPI meaning changes, schema. |
| **v2.2.0** | Enhancements | Priority 1–3 items that keep v1.0 baseline behavior, plus the technical-debt refactors above, with CRs where predicates would move. | New JOB state machine, architecture rewrite, dropping V2 encode/decode. |
| **v3.0** | Architecture | Only with a formal Change Request: for example a single wizard runtime, shared API layer, or schema evolution. | Shipping v3.0 behavior under a v2.x number. |

**Tag rule:** `v2.1.0-jobid-remediation` (`a75bb1e`) stays the production executable baseline until a later release is explicitly tagged. Planning documents do not retag it.

**PR rule:** Documentation and behavior-preserving refactors may proceed under the existing checklist. Anything that changes Create → IN → Permit → OUT → Close → Approval, JOB states, JOB360 encoding, inbox filters, close writes, approval predicates, or dashboard KPI meaning needs a Change Request before code.

---

## 6. What this document is not

- Not a Change Request.
- Not an implementation backlog with estimates or owners.
- Not a license to edit `job_*_v2.aspx.cs`, hub SQL, or schema.
- Not a replacement for the v2.1.0 audit, changelog, or maintenance guidelines.

---

## 7. Repository epic (issue drafts)

GitHub Issue templates live under `.github/ISSUE_TEMPLATE/`. Index: `docs/JOBID_V2.2_EPIC.md`.

| ID | Draft |
|----|--------|
| CR-001 | Unified Wizard |
| CR-002 | Mobile UI |
| CR-003 | Supervisor Dashboard |
| CR-004 | QR Workflow |
| CR-005 | GPS Validation |
| CR-006 | Permit Checklist |
| CR-007 | Reporting & Analytics |
| CR-008 | Technical Debt |

These are drafts only. They do not open GitHub issues by themselves and do not authorize implementation PRs.
