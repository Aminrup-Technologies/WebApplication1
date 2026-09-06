# CR-010 Planning — Shared Infrastructure Refactor

Status: Planning only. Not approved for implementation.  
Date: September 2026  
Change type: **Refactor** per `docs/MAINTENANCE_GUIDELINES.md`  
Baseline: `Jul_to_Sep_2026_Suport_N_Dev_Works` at `a95942a` (docs head). Executable JOB360/V2 head remains `60ad135` (CR-009 Phase E).  
Protected freeze: `v2.1.0-jobid-remediation` (`a75bb1e`, PR #59–#65).

This document does **not** change executable code, SQL, helpers, config, schema, or workflow predicates.

CR-009 discovery previously labeled a helper extract as “CR-008”. That extract was never implemented. This document is the planning Change Request for that work, numbered **CR-010**.

---

## 1. Executive Summary

After CR-009 Phases A–E, JOB360 is a six-tab cockpit with frozen hops, click-time admin authorization, an Admin console, and mobile CSS. The V2 create / IN / permit / OUT pages still carry **copy-pasted** URL-safe Base64 encode/decode, duplicated `ShowNotification` / PNotify snippets, duplicated `.modern-panel` CSS, and JOB360 still compiles unused `_OLD` handlers.

CR-010 is a **behavior-preserving refactor**. It may extract helpers and delete proven-dead code. It must **not**:

- Alter Create → IN-Punch → Permit Upload → OUT-Punch → Close & Send → Approval
- Change the `EncodeJobID` / `DecodeJobID` algorithm or the producer/consumer contract
- Rewrite `EvaluateSmartLifecycle` or `EvaluateActionMatrix`
- Change M1–M6 SQL predicates, inbox filters, Close & Send writes, approval lists, or hub KPI CASE expressions
- Change CR-009 Phase A–E user-visible behavior (tabs, hops, `IsAdmin()` guards, Inspector/Edit Core, timeline sources, mobile CSS semantics)
- Treat Base64 as authorization
- Move Close & Send (`btn_FinalizeShift` / `UpdateJOBTable1`) onto JOB360
- Introduce new `MasterStatusCode` / `JOB_Status` values

Classification (`docs/MAINTENANCE_GUIDELINES.md`):

| If the PR… | Type |
|------------|------|
| Extracts encode/decode with a byte-identical round-trip proof | Refactor |
| Deletes unused `_OLD` methods after proving they are not wired | Refactor |
| Moves `.modern-panel` CSS without selector/behavior change | Enhancement |
| Changes encode padding, URL alphabet, inbox SQL, or status literals | **Change Request — out of CR-010** |

---

## 2. Technical Debt Inventory

| ID | Debt | Location | Risk if ignored | CR-010 action |
|----|------|----------|-----------------|---------------|
| T1 | Encode/Decode copied on V2 pages | `create_jobid_v2` Encode; IN/Permit Encode+Decode; OUT Decode only | Drift if one copy is patched | Extract to one helper; algorithm frozen |
| T2 | JOB360 hops call `create_jobid_v2.EncodeJobID` | `job_360_view.aspx.cs` Permit/IN/OUT redirects | Page coupling; create page is an encode library | Helper; 360 still encodes outbound, inbound `?jobid=` stays **raw** |
| T3 | Status literals duplicated | V2 + 360 + hub + `CountChecker` | Typo risk; accidental new codes | Constants **only** if values stay identical; SQL text proofs required |
| T4 | PNotify / `ShowNotification` copied | Create, IN, Permit, OUT, 360, manage | Escape differences | Optional presentation helper; do not change messages |
| T5 | Close & Send modals | `job_outpunch_v2.aspx` only | If copied to 360, Close ownership breaks | **Do not extract onto 360** |
| T6 | `.modern-panel` CSS copied | 360, OUT, manage, monthly attendance, others | Style drift | Shared CSS file for the class only |
| T7 | JOB360 Phase E CSS is page-inline | `job_360_view.aspx` | Harder to test | Optional `job360-cockpit.css`; keep selectors |
| T8 | `_OLD` matrix and click handlers | `job_360_view.aspx.cs` | Accidental rewire; `_OLD` Resubmit SQL differs | Delete after unused proof |
| T9 | `btnSaveEdit_Click` | 360 code-behind; not in markup | Confusion | Delete after unused proof |
| T10 | `btn_Act_UnblockJob_Click` SQL commented out | Not in markup | Duplicate of live `btn_Act_Unblock_Click` | Delete stub; keep live Unblock |
| T11 | `keepAlive` GET reloads 360 | `job_360_view.aspx` script + `Page_Load` | Extra `Load360View` on ping | Optional ignore `keepAlive` QS; not a write |
| T12 | Session key counts differ | 360: 3 keys; some V2: 2 keys | Unifying by dropping keys weakens auth | **Out of scope** |

Unsafe to fold into a “shared workflow engine”: `EvaluateSmartLifecycle`, `EvaluateActionMatrix`, Force OUT, Bypass, Cancel (`'6'`), Delete (`'0'`), inbox `ActiveJOB_Checker` queries, hub KPI CASE.

---

## 3. Duplicate Encode/Decode Analysis

### 3.1 Frozen contract (M3)

- Producers encode with URL-safe Base64: `+` → `-`, `/` → `_`, strip `=`.
- V2 consumers decode (restore alphabet, pad to multiple of 4, UTF-8).
- JOB360 **outbound** hops encode (`create_jobid_v2.EncodeJobID` today).
- JOB360 **inbound** `?jobid=` is **raw** (OUT success modal, exceptions). Do not start decoding inbound 360.
- Invalid tokens fail closed (`FormatException` caught on V2 `Page_Load` — UAT-049).
- Base64 is **not** authorization. Creator inbox + session remain the gate.

### 3.2 Copies today (`60ad135`)

| Page | Encode | Decode | Callers |
|------|--------|--------|---------|
| `create_jobid_v2.aspx.cs` | Yes (canonical for 360 hops) | No | Post-create redirect; 360 hops |
| `job_inpunch_v2.aspx.cs` | Yes (duplicate) | Yes | Inbox `?jobid=` |
| `job_permitupload_v2.aspx.cs` | Yes (duplicate) | Yes | Inbox `?jobid=`; encode on some redirects |
| `job_outpunch_v2.aspx.cs` | No | Yes | Inbox `?jobid=` |
| `job_360_view.aspx.cs` | No (calls create) | No | Permit / IN / OUT `Response.Redirect` |

The four algorithm bodies that exist are text-identical (UTF-8, URL-safe Base64, padding switch 2 → `==`, 3 → `=`).

### 3.3 Extract plan

1. Add a **single** static helper (name TBD at implementation; do not invent a workflow façade).
2. Move the **existing** encode and decode bodies unchanged.
3. Point create / IN / Permit / OUT / JOB360 hops at the helper.
4. Remove per-page duplicates.
5. Proof: same string in → same string out for encode and decode, including empty, JOBIDs with digits, and invalid tokens still throwing `FormatException` that V2 `Page_Load` catches.
6. Regression: UAT-046 / 047 / 048 / 049; CR009-UAT-005–008.

Do **not** change padding, alphabet, or start encoding inbound 360.

---

## 4. Shared Status Constants Plan

Frozen literals (do not add or rename):

| Token | Frozen meaning |
|-------|----------------|
| `EntryExit='Created'` | Pending IN (M6) |
| `EntryExit='Entry'` | After IN; permit inbox (M2) |
| `EntryExit='Exit'` | Close & Send write (M4) |
| `MasterStatusCode='1'` | Created / permit-required default |
| `MasterStatusCode='3'` | After IN / permit path; Pending OUT with Entry (M6) |
| `MasterStatusCode='4'` | Close & Send |
| `JOB_Status='Out-Punch Done'` | Close write + approval list (M4/M5) |
| `FinalUpldStatus='No'` | Pending Permit KPI (M6) |
| `MasterStatusCode='6'` / `'0'` | Unofficial Cancel / Delete on 360 — **keep; do not expand** |

Plan:

- Optional `JobidStatus` constants class whose **values equal today’s literals**.
- First PR may add the class unused, then replace call sites in a second PR with SQL-string diffs reviewed line by line.
- Do **not** “clean up” Cancel `'6'` or Delete `'0'` here.
- Do **not** reuse permit-inbox SQL as the Pending Permit KPI.

If a constant would change a predicate, stop and open a separate Change Request.

---

## 5. Shared Modal Strategy

| Modal / chrome | Owner | CR-010 |
|----------------|-------|--------|
| PNotify `showPNotify` / `ShowNotification` | Copied on V2 + 360 | Optional shared JS + C# escape helper; **messages unchanged** |
| Close confirm + success (`confirmCloseModal`, `successCloseModal`, `lockCloseAndSend`) | `job_outpunch_v2` only | **Leave on OUT.** Never extract onto JOB360 |
| Worker edit / Core edit / Raw Inspector | JOB360 Admin | Stay on 360; already Phase D |
| Generic `MyPopup` | Unrelated payroll/admin pages | Out of scope |

Close & Send ownership stays `btn_FinalizeShift` → `UpdateJOBTable1` on `job_outpunch_v2`.

---

## 6. CSS Consolidation Plan

| CSS | Today | Plan |
|-----|-------|------|
| `.modern-panel` | Duplicated in several ASPX `<style>` blocks | One production CSS file; delete copies if computed style matches |
| JOB360 stepper / audit / `.job360-*` / 44px touch | Inline in `job_360_view.aspx` (Phase E) | Optional `job360-cockpit.css`; **do not** share stepper with CR-001 wizard |
| Gentelella / Bootstrap | Master + vendors | Do not vendor-shuffle in CR-010 |

Phase E semantics stay: horizontal desktop timeline; vertical ≤768px; sticky search/actions; grid `overflow-x`. Moving CSS without selector change is an Enhancement. Changing breakpoints or hop layout is out of scope.

---

## 7. Dead-Code Retirement Plan

Prove unused, then delete. Do not wire `_OLD` handlers (Resubmit `_OLD` SQL differs from live).

| Symbol | Proof | Action |
|--------|-------|--------|
| `EvaluateActionMatrix_OLD` | Not called | Delete |
| `btn_Act_Resubmit_Click_OLD` | Not in markup | Delete |
| `btn_Act_ForceOut_Click_OLD` | Not in markup | Delete |
| `btn_Act_AdminRollback_Click_OLD` | Not in markup | Delete |
| `btnSaveEdit_Click` | Not in markup | Delete |
| `btn_Act_UnblockJob_Click` | Not in markup; SQL commented | Delete stub; live Unblock remains |

`keepAlive`: optional `Page_Load` ignore of `keepAlive` query string so pings do not re-run `Load360View`. Not a predicate change.

Do **not** delete live Phase C/D handlers or `EvaluateActionMatrix`.

---

## 8. Risk Assessment

| Risk | Likelihood | Impact | Mitigation |
|------|------------|--------|------------|
| Encode alphabet/padding drift | Med if edited by hand | JOB360 hops 500 or wrong job | Copy bodies verbatim; round-trip tests |
| Decoding inbound 360 | Low | Breaks OUT success / exceptions | Explicit non-goal |
| Status constant typo | Med | Inbox/KPI/close regression | Diff SQL strings; no value change |
| Wiring `_OLD` Resubmit | Low | Different attendance SQL | Delete, never call |
| Shared modal pulls Close onto 360 | Low | Violates M4 | Close modals stay on OUT |
| Shared CSS changes 360 mobile | Med | Phase E UAT | Keep `.job360-*` selectors |
| Treating helper as auth | Low | Security regression | Document Base64 ≠ auth |

---

## 9. Rollback Strategy

Each implementation PR is independently revertable.

| PR theme | Rollback |
|----------|----------|
| Encode helper | Restore per-page methods; 360 hops back to `create_jobid_v2.EncodeJobID` |
| Status constants | Revert to literals (values must already match) |
| Dead-code delete | Restore methods from `60ad135` / `a95942a` parent |
| CSS extract | Restore inline `<style>` |

Do not retag `v2.1.0-jobid-remediation`. If any PR changes a frozen predicate, it is rejected, not rolled forward.

---

## 10. UAT Impact (none)

CR-010 adds **no new UAT IDs**. Implementation PRs reuse existing coverage and must remain PASS without behavior change.

| Existing UAT | Why it still applies |
|--------------|----------------------|
| UAT-046 / 047 / 048 / 049 | Encode/decode contract |
| UAT-006 / UAT-021 | IN eligibility (no code-3 gate) |
| UAT-014 / 015 | Permit inbox `Active` + `Entry` |
| UAT-029 / UAT-040 | Close & Send on OUT, not 360 |
| UAT-004 / 005 / 035 | Hub KPI meanings |
| CR009-UAT-005–008 | 360 encoded hops; inbound raw |
| CR009-UAT-026–028 | Phase B hop visibility |
| CR009-UAT-030–035 | Phase C/D auth and Inspector |
| CR009-UAT-036–040 | Phase E presentation |

If a CR-010 PR requires new UAT, the work is misclassified and must stop.

---

## 11. Future PR Breakdown

Order is deliberate: encoding first (highest duplication, frozen algorithm), then dead code, then presentation. Do not mix predicate-adjacent constant replacement with encode extract in one squash.

| Seq | Theme | Allowed files (indicative) | Proof | Forbidden |
|-----|-------|----------------------------|-------|-----------|
| 1 | Encode/Decode helper | New helper + create/IN/Permit/OUT/360 call sites | Round-trip; UAT-046–049 | Algorithm change; decode inbound 360 |
| 2 | Delete JOB360 `_OLD` / unused stubs | `job_360_view.aspx.cs` only | Methods unreferenced | Wiring `_OLD`; touching live SQL |
| 3 | Status constants (optional) | New constants + mechanical replacements | SQL text identical | New codes; KPI/inbox predicate edits |
| 4 | `.modern-panel` CSS | Shared CSS + ASPX style deletions | Visual match | Stepper/timeline selector changes |
| 5 | Optional `job360-cockpit.css` | 360 aspx + new CSS | Phase E UAT 036–040 | Sharing 360 steps with CR-001 |
| 6 | Optional PNotify helper | JS + `ShowNotification` copies | Same titles/text | Close modals on 360 |
| 7 | Optional `keepAlive` ignore | 360 `Page_Load` + script | Search/load still works | Session weakening |

Each PR: classify as Refactor or Enhancement; attach this CR-010; tick `docs/MAINTENANCE_GUIDELINES.md` checklist; no schema; no new UAT unless the work is rejected.

---

## Files in this planning PR

| File | Role |
|------|------|
| `docs/CR-010_SHARED_INFRASTRUCTURE_REFACTOR.md` | This document (new) |

No executable files.
