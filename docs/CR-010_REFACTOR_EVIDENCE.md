# CR-010 Refactor Evidence

Status: Complete  
Date: September 2026  
Change type: Documentation  
Executable baseline: `ff531b0` on `Jul_to_Sep_2026_Suport_N_Dev_Works`

This file is the audit trail for CR-010. It does not change executable code. Narrative close-out is `docs/CR-010_CLOSEOUT.md`.

Classification: **Refactor** (behavior-preserving) per `docs/MAINTENANCE_GUIDELINES.md`.

---

## 1. Executable SHAs

| Phase | Theme | PR | Squash SHA |
|-------|-------|----|------------|
| Certified CR-009 Phase E runtime | JOB360 A–E | #76 | `60ad135` |
| CR-009 docs close-out | Docs | #77 | `a95942a` |
| 1 | JobIdCodec | #79 | `1b6e5f3` |
| 2 | JobStatusConstants | #81 | `cf5f1a9` |
| 3 | Shared UI | #82 | `9a5fa72` |
| 4 | Dead-code retirement | #83 | `ff531b0` |

Full Phase 4 commit: `ff531b01f08e3e9e168bfcd520fdc7caf8437eca`.

PR #80 (earlier constants extract, different names/path) is superseded by #81 and is not in this baseline.

Progression: `60ad135` → `a95942a` → `1b6e5f3` → `cf5f1a9` → `9a5fa72` → `ff531b0`.

---

## 2. Helper introduction

| Helper | Path | Introduced | Role |
|--------|------|------------|------|
| `JobIdCodec` | `WebApplication1/App_Code/JobIdCodec.cs` | `1b6e5f3` | Frozen M3 encode/decode |
| `JobStatusConstants` | `WebApplication1/App_Code/JobStatusConstants.cs` | `cf5f1a9` | Frozen C# status literals |
| `NotificationHelper` | `WebApplication1/App_Code/NotificationHelper.cs` | `9a5fa72` | PNotify startup-script helper |
| JOB360 CSS | `WebApplication1/Content/job360-cockpit.css` | `9a5fa72` | Phase E presentation (79 selectors) |

`JobIdCodec` algorithm (verbatim): UTF-8 Base64; `+`→`-`; `/`→`_`; strip `=`; decode restores alphabet; pad `%4==2` → `==`, `%4==3` → `=`.

`JobStatusConstants` values: `Created` / `Entry` / `Exit`; `Active` / `Out-Punch Done`; codes `1` / `3` / `4` / `5`; declared `6` / `0` (Cancel/Delete SQL remains literal `'6'` / `'0'`).

---

## 3. Wrapper preservation

Per-page method names were not removed. Callers were not renamed.

| Page | Wrapper kept | Delegates to |
|------|----------------|--------------|
| `create_jobid_v2.aspx.cs` | `EncodeJobID` | `JobIdCodec.Encode` |
| `job_inpunch_v2.aspx.cs` | `EncodeJobID` / `DecodeJobID` | `JobIdCodec` |
| `job_permitupload_v2.aspx.cs` | `EncodeJobID` / `DecodeJobID` | `JobIdCodec` |
| `job_outpunch_v2.aspx.cs` | `DecodeJobID` | `JobIdCodec.Decode` |
| `job_360_view.aspx.cs` hops | still `create_jobid_v2.EncodeJobID(...)` | 3 outbound hops |
| `create_jobid_v2` / JOB360 | `ShowNotification` | `NotificationHelper.Show` + `EscapeMode.Full` |
| IN / OUT / Permit | `ShowNotification` | `NotificationHelper.Show` + `EscapeMode.QuoteOnly` |

Inbound JOB360 `Request.QueryString["jobid"]` remains raw (count 2 at `ff531b0`). V2 `Page_Load` still catches `FormatException` (UAT-049).

Notification titles, messages, and type strings (including `"notice"`) were not edited.

---

## 4. Hash proofs

Live JOB360 methods at `ff531b0` vs `9a5fa72` (Phase 4 deleted only unreachable methods):

| Method | Result | SHA-256 prefix |
|--------|--------|----------------|
| `EvaluateSmartLifecycle` | identical | `577be98adc40b4d9` |
| `EvaluateActionMatrix` | identical | `bf3c20b00cd75117` |
| `Page_Load` | identical | `68ee931d8e05143e` |
| `IsAdmin` | identical | `e8a1acad043f26b2` |
| `btn_SaveWorkerEdit_Click` | identical | `082874110ee3c228` |
| `btn_Act_Unblock_Click` | identical | `8ef5addc5db3ef3b` |
| `btn_Act_Resubmit_Click` | identical | `ff704736c0067633` |
| `btn_Act_ForceOut_Click` | identical | `2ca9850b600546e0` |
| `btn_Act_AdminRollback_Click` | identical | `723527bc029e01dc` |
| `btn_Act_ViewRawData_Click` | identical | `7a19a8bbcd536046` |

`JobIdCodec.cs` and `JobStatusConstants.cs` were not modified in Phases 3–4.

Phase 3 CSS: 79 selectors; rule body byte-identical to the former `job_360_view.aspx` inline `<style>` block. `sessionStorage` key remains `job360-cockpit-tab`.

---

## 5. Deleted-symbol evidence

Phase 4 (`ff531b0`): `job_360_view.aspx.cs` only; 0 insertions / 362 deletions.

| Symbol | Markup OnClick | C# call | Designer | Reflection | JS / ScriptManager | Verdict |
|--------|----------------|---------|----------|------------|--------------------|---------|
| `EvaluateActionMatrix_OLD` | n/a | none (live call is `EvaluateActionMatrix`) | n/a | none | none | deleted |
| `btn_Act_Resubmit_Click_OLD` | none (live `btn_Act_Resubmit_Click`) | none | none | none | none | deleted |
| `btn_Act_ForceOut_Click_OLD` | none (live `btn_Act_ForceOut_Click`) | none | none | none | none | deleted |
| `btn_Act_AdminRollback_Click_OLD` | none (live `btn_Act_AdminRollback_Click`) | none | none | none | none | deleted |
| `btn_Act_UnblockJob_Click` | none (live `btn_Act_Unblock_Click`) | none | none | none | none | deleted (SQL commented stub) |
| `btnSaveEdit_Click` | none (live `btn_SaveWorkerEdit_Click`) | none | none | none | none | deleted |

Zero matches in `*.cs` / `*.aspx` / `*.js` at `ff531b0`. Historical mentions remain only in CR-009 docs (not rewritten).

`keepAlive` was **not** deleted. `Page_Load` still processes `jobid`. Heartbeat JS still sends `keepAlive=`. No early-return was introduced.

---

## 6. CI summary

| Event | Checks | Result |
|-------|--------|--------|
| PR #79 (Phase 1) | Build + Security | SUCCESS (merged `1b6e5f3`) |
| PR #81 (Phase 2) | Build + Security | SUCCESS (merged `cf5f1a9`) |
| PR #82 (Phase 3) | Build + Security | SUCCESS (merged `9a5fa72`) |
| PR #83 (Phase 4) | Build + Security | SUCCESS |
| Post-merge Jul `ff531b0` | Build `34049174253`; Security `34049174199` | SUCCESS |

No schema, SQL script, or config files in any CR-010 implementation diff.

---

## 7. UAT reuse summary

No new UAT IDs. Implementation PRs reused:

| UAT | Coverage |
|-----|----------|
| UAT-046–049 | Encode/decode contract; invalid token fail-closed |
| CR009-UAT-005–008 | Encoded outbound hops; inbound 360 raw |
| CR009-UAT-026–028 | Phase B hop visibility / same handlers |
| CR009-UAT-030–035 | Phase C/D `IsAdmin()` and Inspector |
| CR009-UAT-036–040 | Phase E presentation selectors |

PR #59–#65 (M1–M6) remain the production freeze. CR-010 did not alter those predicates.
