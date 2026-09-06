# CR-010 Close-out — Shared Infrastructure Refactor

Status: Complete  
Date: September 2026  
Branch: `Jul_to_Sep_2026_Suport_N_Dev_Works`  
Change type: Documentation  
Executable baseline: `ff531b0`

Companion evidence: `docs/CR-010_REFACTOR_EVIDENCE.md`

---

## 1. Executive Summary

CR-010 is complete.

It is a behavior-preserving refactor of shared JOBID V2 / JOB360 infrastructure. Observable workflow is unchanged.

There is no lifecycle change. There is no authorization change. There is no SQL change.

Create → IN-Punch → Permit Upload → OUT-Punch → Close & Send → Approval remains the certified path. Base64 is not authorization. Close & Send remains on `job_outpunch_v2`. The production freeze `v2.1.0-jobid-remediation` (`a75bb1e`) is not retagged.

---

## 2. Phase Timeline

| Phase | Merge |
|-------|--------|
| JobIdCodec | `1b6e5f3` |
| Status Constants | `cf5f1a9` |
| Shared UI | `9a5fa72` |
| Dead-Code Retirement | `ff531b0` |

| Phase | PR |
|-------|-----|
| 1 JobIdCodec | #79 |
| 2 Status Constants | #81 |
| 3 Shared UI | #82 |
| 4 Dead-Code Retirement | #83 |

PR #80 was superseded by PR #81 and is not part of this baseline.

---

## 3. Technical Debt Removed

- Duplicate encode/decode: algorithm centralized in `JobIdCodec`; per-page wrappers remain.
- Duplicated status literals: C# comparisons use `JobStatusConstants`; SQL text remains literal.
- Duplicated notification plumbing: five V2/360 wrappers delegate to `NotificationHelper` without changing message text.
- Duplicated JOB360 CSS: Phase E presentation moved to `Content/job360-cockpit.css` (79 selectors preserved).
- Six unreachable handlers: `EvaluateActionMatrix_OLD`, `btn_Act_Resubmit_Click_OLD`, `btn_Act_ForceOut_Click_OLD`, `btn_Act_AdminRollback_Click_OLD`, `btn_Act_UnblockJob_Click`, `btnSaveEdit_Click`.

---

## 4. Frozen Invariants

Reaffirmed at `ff531b0`:

- Create → IN → Permit → OUT → Close & Send → Approval is unchanged.
- `EncodeJobID` contract is unchanged: URL-safe UTF-8 Base64 (`+`→`-`, `/`→`_`, strip `=`); V2 decodes inbound tokens; JOB360 outbound hops still call `create_jobid_v2.EncodeJobID()`; inbound `job_360_view.aspx?jobid=` remains raw.
- `EvaluateSmartLifecycle` is unchanged (byte-identical through Phase 4).
- Live `EvaluateActionMatrix` is unchanged (byte-identical through Phase 4).
- V2 inbox predicates are unchanged (IN eligibility has no `MasterStatusCode='3'` gate; permit inbox remains `JOBID_Status='Active' AND EntryExit='Entry'`).
- KPI semantics are unchanged (Pending IN = Created; Pending Permit = Entry AND `FinalUpldStatus='No'`; Pending OUT = code 3 AND Entry).
- Base64 is not authorization. Session, creator, and company checks remain the gate.

---

## 5. Remaining Intentional Debt

Documented only; not in CR-010:

- `keepAlive` retained. An early-return in `Page_Load` would skip `Load360View` on heartbeat GETs and change request flow.
- Cancel status `"6"` policy. Unofficial 360 Cancel/Delete codes `'6'` / `'0'` are declared on `JobStatusConstants` but Cancel/Delete SQL stays literal; do not expand.
- Multi-company tenancy. Session-key unification is out of scope; do not drop keys to “unify.”
- Future architecture belongs in CR-001 / V2.2. Do not fold `EvaluateSmartLifecycle`, live `EvaluateActionMatrix`, inbox SQL, or hub KPI CASE into a shared workflow engine under CR-010.

---

## 6. Rollback

Rollback is application-only.

CR-010 introduced no schema, stored-procedure, or configuration changes. Reverting a phase is a git revert of that squash SHA (`1b6e5f3`, `cf5f1a9`, `9a5fa72`, or `ff531b0`). Database rollback is not required.

Do not retag `v2.1.0-jobid-remediation`.
