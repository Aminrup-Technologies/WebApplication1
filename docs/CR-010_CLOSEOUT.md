# CR-010 Close-out — Shared Infrastructure Refactor

Status: Complete  
Date: September 2026  
Branch: `Jul_to_Sep_2026_Suport_N_Dev_Works`  
Change type: Documentation only  
Executable baseline: `ff531b0` (Phase 4 squash)

This document is the chronological index for CR-010 after Phases 1–4 shipped. It does **not** change executable code, SQL, helpers, config, schema, or frozen M1–M6 predicates.

CR-010 is a **behavior-preserving refactor** of shared JOBID V2 / JOB360 infrastructure. It did not reopen Create → IN → Permit → OUT → Close & Send → Approval.

---

## 1. Program status

| Area | Status | Merge |
|------|--------|-------|
| M1–M6 remediation | Frozen production contract | `a75bb1e` (`v2.1.0-jobid-remediation`) |
| CR-009 Phases A–E | Complete | `60ad135` (docs head `a95942a`) |
| CR-010 planning | Written on PR #78 (not required on Jul) | PR #78 |
| Phase 1 — JobIdCodec | Complete | PR #79 → `1b6e5f3` |
| Phase 2 — JobStatusConstants | Complete | PR #81 → `cf5f1a9` |
| Phase 3 — Shared UI | Complete | PR #82 → `9a5fa72` |
| Phase 4 — Dead-code retirement | Complete | PR #83 → `ff531b0` |
| Current executable baseline | CR-010 Phase 4 | **`ff531b0`** |

Executable progression after CR-009: `60ad135` → `a95942a` (docs) → `1b6e5f3` → `cf5f1a9` → `9a5fa72` → `ff531b0`.

PR #80 (earlier constants extract on a different path/names) was superseded by PR #81 and is not part of this baseline.

---

## 2. Completion matrix (Phases 1–4)

| Phase | Theme | PR | Squash SHA | Allowed files | Proof | Forbidden (held) |
|-------|-------|----|------------|---------------|-------|------------------|
| 1 | Shared `JobIdCodec` | #79 | `1b6e5f3` | `App_Code/JobIdCodec.cs`; create/IN/Permit/OUT wrappers | Encode/decode algorithm unchanged; 360 hops still call `create_jobid_v2.EncodeJobID()`; inbound 360 `?jobid=` raw | Alphabet/padding change; decode inbound 360 |
| 2 | Shared `JobStatusConstants` | #81 | `cf5f1a9` | `App_Code/JobStatusConstants.cs`; C# comparisons/params | SQL text identical; no new codes | Interpolating constants into inbox/KPI/Close SQL |
| 3 | Shared UI | #82 | `9a5fa72` | `NotificationHelper.cs`; `Content/job360-cockpit.css`; five `ShowNotification` wrappers; 360 aspx link | Full vs QuoteOnly escape preserved; 79 CSS selectors byte-identical; `job360-cockpit-tab` unchanged | Notification wording; Close modals on 360; sharing stepper with CR-001 |
| 4 | Dead-code retirement | #83 | `ff531b0` | `job_360_view.aspx.cs` only (362 deletions) | Six symbols unreachable; live methods byte-identical | Wiring `_OLD`; `keepAlive` early-return |

Classification throughout: **Refactor** per `docs/MAINTENANCE_GUIDELINES.md`. No new UAT IDs. Reused UAT-046–049 and CR009-UAT-005–008 / 026–040.

---

## 3. Chronology

| Order | Event | SHA / PR |
|-------|-------|----------|
| 1 | Certified JOB360/V2 runtime after CR-009 Phase E | `60ad135` |
| 2 | CR-009 docs close-out | `a95942a` (PR #77) |
| 3 | CR-010 planning (docs-only branch) | PR #78 |
| 4 | Phase 1 JobIdCodec | `1b6e5f3` (PR #79) |
| 5 | Phase 2 JobStatusConstants | `cf5f1a9` (PR #81) |
| 6 | Phase 3 NotificationHelper + `job360-cockpit.css` | `9a5fa72` (PR #82) |
| 7 | Phase 4 delete proven-unused handlers | `ff531b0` (PR #83) |
| 8 | This close-out | PR #84 (docs only) |

Do **not** retag `v2.1.0-jobid-remediation`. CR-010 did not change the frozen lifecycle.

---

## 4. Before / after technical debt

Planning inventory IDs (T1–T12) from CR-010 planning. After Phase 4:

| ID | Debt | After CR-010 | Residual |
|----|------|--------------|----------|
| T1 | Encode/Decode copied on V2 pages | **Closed.** Algorithm lives in `JobIdCodec`. Per-page `EncodeJobID` / `DecodeJobID` wrappers remain | Wrappers intentionally kept so 360 hops still call `create_jobid_v2.EncodeJobID()` |
| T2 | JOB360 hops couple to create page | **Closed for algorithm drift.** Call path unchanged | Inbound 360 `?jobid=` still **raw** (M3) |
| T3 | Status literals duplicated | **Closed for C# comparisons.** `JobStatusConstants` values equal former literals | SQL strings (inbox, Close WHERE, KPI CASE, Bypass/Cancel/Delete) remain literal text |
| T4 | PNotify `ShowNotification` copied | **Closed for the five CR-010 callers.** `NotificationHelper` with `Full` / `QuoteOnly` | `manage_jobid_v2` and other pages not in Phase 3; titles/messages unchanged |
| T5 | Close & Send modals | **Unchanged by design.** Stay on `job_outpunch_v2` | Do not extract onto JOB360 |
| T6 | `.modern-panel` CSS copied | **Partial.** 360 presentation (including `.modern-panel`) is in `job360-cockpit.css` | OUT/manage copies not deleted (computed-style match not proven) |
| T7 | JOB360 Phase E CSS inline | **Closed.** `Content/job360-cockpit.css`; 79 selectors preserved | Do not share with CR-001 wizard |
| T8 | `_OLD` matrix and click handlers | **Closed.** `EvaluateActionMatrix_OLD`, Resubmit/ForceOut/AdminRollback `_OLD` deleted | Live `EvaluateActionMatrix` / live `_Click` remain |
| T9 | `btnSaveEdit_Click` unwired | **Closed.** Deleted | Live save is `btn_SaveWorkerEdit_Click` |
| T10 | `btn_Act_UnblockJob_Click` stub | **Closed.** Deleted (SQL was commented) | Live Unblock is `btn_Act_Unblock_Click` |
| T11 | `keepAlive` GET re-runs `Load360View` | **Retained.** Phase 4 rejected an early-return because it would change `Page_Load` request flow | Future CR only; not a write; not authorization |
| T12 | Session key counts differ | **Out of scope** | Do not drop keys to “unify” |

Unsafe to fold into a shared workflow engine (unchanged): `EvaluateSmartLifecycle`, live `EvaluateActionMatrix`, Force OUT, Bypass, Cancel (`'6'`), Delete (`'0'`), inbox `ActiveJOB_Checker` SQL, hub KPI CASE.

---

## 5. Regression evidence index

### 5.1 Frozen contract (must still hold at `ff531b0`)

| Milestone | Predicate | CR-010 |
|-----------|-----------|--------|
| M1 IN eligibility | No `MasterStatusCode='3'` gate on `job_inpunch_v2` | Untouched |
| M2 permit inbox | `JOBID_Status='Active' AND EntryExit='Entry'` | Untouched |
| M3 JOB360 hops | Outbound `create_jobid_v2.EncodeJobID()`; inbound 360 `jobid` raw; Base64 is not authorization | Algorithm extracted; contract unchanged |
| M4 Close & Send | On `job_outpunch_v2` (`btn_FinalizeShift` → `UpdateJOBTable1`): `JOB_Status='Out-Punch Done'`, `MasterStatusCode='4'`, `EntryExit='Exit'` | Untouched |
| M5 Approval | `JOB_Status='Out-Punch Done' AND EntryExit='Exit'` | Untouched |
| M6 Hub KPIs | Pending IN = Created; Pending Permit = Entry AND `FinalUpldStatus=No`; Pending OUT = code 3 AND Entry | Untouched |

Permit-before-IN is not a required gate. No new `MasterStatusCode` / `JOB_Status` values.

### 5.2 Helper identity

| Artifact | Path | Notes |
|----------|------|-------|
| `JobIdCodec` | `WebApplication1/App_Code/JobIdCodec.cs` | UTF-8 URL-safe Base64; `+`→`-`, `/`→`_`, strip `=`; decode pad `%4==2` → `==`, `%4==3` → `=` |
| `JobStatusConstants` | `WebApplication1/App_Code/JobStatusConstants.cs` | `Created`/`Entry`/`Exit`; `Active`/`Out-Punch Done`; codes `1`/`3`/`4`/`5`; declared `6`/`0` (Cancel/Delete SQL still literal) |
| `NotificationHelper` | `WebApplication1/App_Code/NotificationHelper.cs` | `Success`/`Error`/`Warning`/`Info`/`Show`; script key `PNotify` |
| JOB360 CSS | `WebApplication1/Content/job360-cockpit.css` | Phase E selectors; wired from `job_360_view.aspx` head |

### 5.3 Phase 4 live-method hashes (vs `9a5fa72`)

| Method | Result | SHA-256 prefix |
|--------|--------|----------------|
| `EvaluateSmartLifecycle` | identical | `577be98adc40b4d9` |
| `EvaluateActionMatrix` | identical | `bf3c20b00cd75117` |
| `Page_Load` | identical (`jobid` still loads; no `keepAlive` early-return) | `68ee931d8e05143e` |
| `btn_SaveWorkerEdit_Click` | identical | `082874110ee3c228` |
| `btn_Act_Unblock_Click` | identical | `8ef5addc5db3ef3b` |
| `btn_Act_Resubmit_Click` | identical | `ff704736c0067633` |
| `btn_Act_ForceOut_Click` | identical | `2ca9850b600546e0` |
| `btn_Act_AdminRollback_Click` | identical | `723527bc029e01dc` |
| `btn_Act_ViewRawData_Click` | identical | `7a19a8bbcd536046` |
| `IsAdmin` | identical | `e8a1acad043f26b2` |

### 5.4 CR-009 A–E

| Phase | Expectation at `ff531b0` |
|-------|--------------------------|
| A | Six tabs; `sessionStorage` key `job360-cockpit-tab` |
| B | Same hop `OnClick` handlers; IN at Created; Permit while Entry |
| C | Click-time `IsAdmin()` on live writes |
| D | Inspector / Edit Core / audit timeline |
| E | `job360-cockpit.css` 79 selectors; ≤768px; sticky search/actions; grid scroll; 44px touch |

### 5.5 UAT reuse (no new IDs)

| UAT | Why it still applies |
|-----|----------------------|
| UAT-046 / 047 / 048 / 049 | Encode/decode contract; invalid token still `FormatException` on V2 `Page_Load` |
| CR009-UAT-005–008 | Encoded outbound hops; inbound 360 raw |
| CR009-UAT-026–028 | Phase B hop visibility |
| CR009-UAT-030–035 | Phase C/D auth and Inspector |
| CR009-UAT-036–040 | Phase E presentation |

### 5.6 Post-merge CI (Phase 4 on Jul)

| Check | Run | Result |
|-------|-----|--------|
| Build | `34049174253` | SUCCESS |
| Security | `34049174199` | SUCCESS |
| Commit | `ff531b01f08e3e9e168bfcd520fdc7caf8437eca` | green on `Jul_to_Sep_2026_Suport_N_Dev_Works` |

---

## 6. Explicit non-goals (still true)

- Do not decode inbound JOB360 `?jobid=`.
- Do not treat Base64 as authorization.
- Do not move Close & Send onto JOB360.
- Do not rewrite `EvaluateSmartLifecycle` or live `EvaluateActionMatrix`.
- Do not ignore `keepAlive` in `Page_Load` without a separate Change Request that accepts the request-flow change.
- Do not unify session keys by dropping checks.
- Do not introduce new status codes.

---

## 7. Executable confirmation for this documentation PR

This close-out pack adds markdown only.

- No `.aspx` / `.aspx.cs` / `.designer.cs` edits
- No V2 page edits
- No SQL, helpers, config, or workflow predicate edits
- `git diff --name-only` against the documentation branch parent must list only `docs/*.md`

---

## 8. Remaining work (out of CR-010)

- Optional `keepAlive` ignore (Enhancement / separate CR; changes `Page_Load` request flow)
- Optional `.modern-panel` deletion on OUT/manage after computed-style proof
- Optional `manage_jobid_v2` PNotify wrapper
- Cross-device runtime validation of Phase E CSS after the stylesheet extract
- Next feature stream from the V2.2 roadmap (not a CR-010 continuation)

Do not reopen M1–M6 or Close & Send ownership on JOB360 without a new Change Request.
