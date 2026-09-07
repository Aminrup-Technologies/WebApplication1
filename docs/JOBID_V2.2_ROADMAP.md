# JOBID V2.2 Roadmap

Status: Product roadmap (JOBID). Not Security Foundation.  
Date: September 2026  
Branch: `Jul_to_Sep_2026_Suport_N_Dev_Works`  
GitHub label: `jobid-v2.2` (never `v2.2` — that string is reserved for Security Foundation)

This document is a **JOBID** product and engineering plan. It does not change executable code, workflow predicates, schema, or the frozen JOBID v2.1.0 baseline.

**Security Foundation v2.2 is complete** (`v2.2-security-foundation`, `v2.2.1-governance`). That program is identity, Session, `AuthorizationService`, overlay, and Switch User. This JOBID roadmap is a **separate product line**. Do not mix labels, tags, or CRs.

---

## 1. Executive Summary

**JOBID v2.1.0 is the frozen operational baseline.**

The JOBID V2 remediation program (milestones M1–M6, PR #59–#65) restored the intended operational path and is tagged as `v2.1.0-jobid-remediation` at executable commit `a75bb1e`. That tag must remain immutable. Future work must not rewrite those predicates silently.

**JOBID V2.2** is an enhancement program on top of that frozen lifecycle, not a second remediation, and **not** Security Foundation v2.2. It may improve presentation, supervisor experience, and maintainability. It may not invert Create → IN → Permit → OUT → Close & Send → Approval, introduce new JOB states without a Change Request, or break the JOB360 encode/decode contract.

### CR-001 — Phase A certified

Unified Supervisor Wizard **Phase A is certified**. Do not re-implement the shell.

| Fact | Location |
| --- | --- |
| Planning spec | `docs/CR-001_UNIFIED_SUPERVISOR_WIZARD_SPEC.md` |
| Execution baseline | `docs/CR-001_EXECUTION_BASELINE.md` (SHA `238bd2f`) |
| Status | **Phase A Certified** |
| Later slices | Phase B+ (Create experience and beyond) need a **new** CR against that baseline |

GitHub issue templates for CR-001 are for Phase B+ (or follow-on) only. Opening the template does not authorize another Phase A.

Control documents for the JOBID freeze:

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
- **No schema or stored-procedure changes** unless a CR includes a schema plan and rollback. Rollback for JOBID V2.2 enhancements should remain application-only wherever possible.
- **Do not reuse the GitHub label `v2.2`.** JOBID issues use `jobid-v2.2`. Security Foundation uses tags `v2.2-security-foundation` / `v2.2.1-governance`.
- **Classify every change** (Hotfix / Enhancement / Change Request / Refactor / Documentation) before implementation.

---

## 3. Enhancement Backlog

Items below are proposals except **CR-001 Phase A**, which is already certified. Each remaining item that would change workflow, encoding, inbox, close, approval, or KPI meaning requires a Change Request first.

### Priority 1

| Item | Intent | Constraint |
|------|--------|------------|
| Unified Wizard (CR-001) | One supervisor path through Create, IN, Permit, OUT, and Close. | **Phase A Certified** (`238bd2f`). Phase B+ only. Same state machine. Do not require permit before IN. Do not replace Close & Send. |
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
| Shared `EncodeJobID` / `DecodeJobID` helper | **Done in CR-010** (`JobIdCodec`, PR #79). | Do not extract a second helper. Algorithm stays frozen. |
| Shared status constants | **Done in CR-010** (`JobStatusConstants` in `App_Code`, PR #81). | Do not add a second class or rename constants in a drive-by PR. |
| Shared notification helper / JOB360 CSS | **Done in CR-010** (PR #82 / #83). | Do not re-extract the same CSS. |
| Common Close & Send modal | Close & Send confirmation/success can still be page-local. | Shared markup/script only. Close still calls `UpdateJOBTable1`; `btn_FinalizeShift` stays unless a CR renames it. |
| Dashboard query consolidation | Hub `LoadDashboardStats()` is one SQL CASE query; related counts exist on V1 hub and inbox pages. | Keep M6 CASE semantics. Do not merge inbox SQL into the Pending Permit badge. |

Out of scope for “debt only”: new columns, new SPs, new `MasterStatusCode` values, permit-before-IN, or changing JOB360 AddDocs/SwapDate without a CR.

---

## 5. Release Strategy

Follow `docs/MAINTENANCE_GUIDELINES.md` versioning. Do not silently rewrite the frozen lifecycle.

| Release | Type | Allowed work | Not allowed |
|---------|------|--------------|-------------|
| **jobid-v2.1.1** | Patch only | Hotfixes that restore documented JOBID v2.1.0 behavior (decode exceptions on legitimate tokens, Close & Send double-submit, copy/logging that does not change states). | Enhancements, new wizard, KPI meaning changes, schema. |
| **jobid-v2.2.0** | Enhancements | Priority 1–3 items that keep v2.1.0 baseline behavior, plus remaining debt, with CRs where predicates would move. Not Security Foundation `v2.2-security-foundation`. | New JOB state machine, architecture rewrite, dropping V2 encode/decode. |
| **jobid-v3.0** | Architecture | Only with a formal Change Request: for example a single wizard runtime, shared API layer, or schema evolution. | Shipping v3.0 behavior under a v2.x number. |

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

GitHub Issue templates live under `.github/ISSUE_TEMPLATE/`. Index: `docs/JOBID_V2.2_EPIC.md`. Every template uses label **`jobid-v2.2`**.

| ID | Draft | Notes |
|----|--------|--------|
| CR-001 | Unified Wizard | **Phase A Certified.** Template is for Phase B+ only. See `docs/CR-001_EXECUTION_BASELINE.md`. |
| CR-002 | Mobile UI | Not started |
| CR-003 | Supervisor Dashboard | Not started |
| CR-004 | QR Workflow | Not started |
| CR-005 | GPS Validation | Not started |
| CR-006 | Permit Checklist | Not started |
| CR-007 | Reporting & Analytics | Not started |
| CR-008 | Technical Debt | CR-010 already shipped codec / constants / notify / dead-code. Template is remaining debt only. |

These are drafts only. They do not open GitHub issues by themselves and do not authorize implementation PRs. They are not Security Foundation work.
