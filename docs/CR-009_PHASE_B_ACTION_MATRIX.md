# CR-009 Phase B — JOB360 Action Matrix Rationalization

Branch: `cursor/cr009-phase-b-action-matrix`  
Base: `Jul_to_Sep_2026_Suport_N_Dev_Works` (`e2c773d` Phase A cockpit)  
Change type: Change Request (visibility only)

This document is the Phase B inventory, before/after matrix, UAT map, and regression proof. Executable change is confined to `EvaluateActionMatrix` and extracted visibility helpers in `job_360_view.aspx.cs`.

## Scope

- Refactor hop **visibility** so Overview follows frozen M1–M6: Create → IN-Punch → Permit Upload → OUT-Punch → Close & Send → Approval.
- Close & Send remains on `job_outpunch_v2` (`btn_FinalizeShift` → `UpdateJOBTable1`).
- Permit-before-IN must never become mandatory.
- Do **not** rewrite handlers, SQL, redirects, encoding, inbox predicates, KPIs, or `MasterStatusCode`.
- Do **not** add click-time `IsAdmin()` (Phase C debt).

## Changed methods

| Method | Role |
|--------|------|
| `EvaluateActionMatrix` | Orchestrates reset + rule blocks; hides empty Overview `ActionBarRow` |
| `ResetActionButtonVisibility` | Single reset; keeps Raw Inspector / Edit Core hidden |
| `ApplyAdminOverrideVisibility` | Unchanged Bypass / Reset / Cancel / Rollback predicates |
| `ApplyUnblockVisibility` | Unchanged IsAdmin + blocked + not Exit |
| `ApplySupervisorHopVisibility` | **Phase B hop rules** (replaces `pipelineStep == 2/3/4/5`) |
| `IsPermitOutstanding` | Created-only outstanding permit test from already-loaded row |
| `ApplyInWindowForceOutVisibility` | Unchanged in-window Force OUT predicate |
| `ApplyLockoutForceOutVisibility` | Unchanged lockout Force OUT + copy |
| `ApplyResubmitVisibility` | Unchanged Rejected / Returned / Cancelled |
| `ApplyPrePunchOverrideVisibility` | Unchanged Swap Date / Delete (no first IN) |

Handlers, `EvaluateSmartLifecycle`, `EvaluateActionMatrix_OLD`, markup IDs, and `OnClick` / `OnRowCommand` / `OnCheckedChanged` are unchanged.

## Phase 1 — Inventory (owner)

| Button | Current visibility rule (pre-B / `e2c773d`) | Desired owner |
|--------|-----------------------------------------------|---------------|
| `btn_Act_UploadPermit` | 3-day/grace **and** `pipelineStep == 2` (permit-before-IN gate) | Supervisor (Overview hop) |
| `btn_Act_InPunch` | 3-day/grace **and** `pipelineStep == 3` (hidden while permit outstanding) | Supervisor (Overview hop) |
| `btn_Act_OutPunch` | 3-day/grace **and** `pipelineStep == 5` (hidden while CSM pending; hidden after `MAX(LastModified)` stepper advance) | Supervisor (Overview hop) |
| `btn_Act_AddDocs` | 3-day/grace **and** `pipelineStep == 4` | Supervisor (Overview hop) |
| `btn_Act_Unblock` | `IsAdmin` + blocked + not Exit | Admin |
| `btn_Act_Resubmit` | Rejected / Returned / Cancelled; **not** `IsAdmin` | Admin (Phase C: click `IsAdmin`) |
| `btn_Act_ForceOut` | In-window: Entry + first IN + created date &lt; today (any user). Lockout: `IsAdmin` + (Entry or Created) + first IN | Admin (not Close & Send) |
| `btn_Act_SwapDate` | No first IN; **not** `IsAdmin` | Admin (Phase C: `IsAdmin`) |
| `btn_Act_Delete` | No first IN; **not** `IsAdmin` | Admin (Phase C: `IsAdmin`) |
| `btn_Act_ForcePermitBypass` | `IsAdmin` + `MasterStatusCode='1'` | Admin |
| `btn_Act_ResetToCreated` | `IsAdmin` + code `3` + 0 workers | Admin |
| `btn_Act_CancelShift` | `IsAdmin` + (code `1` or empty code `3`) | Admin |
| `btn_Act_AdminRollback` | `IsAdmin` + Approved | Admin |
| `btn_Act_ViewRawData` | Markup `Visible=false`; matrix never enabled | Hidden/System (Phase D) |
| `btn_EditCoreDetails` | Markup `Visible=false`; matrix never enabled | Hidden/System (Phase C product CR) |
| `btn_SaveCoreDetails` | Modal save; not matrix-controlled | Hidden/System (modal; Phase C auth) |
| `btn_SaveWorkerEdit` | Modal save; not matrix-controlled | Hidden/System (modal; Phase C auth) |

Markup placement from Phase A is unchanged: Overview hosts the four hops; Admin hosts overrides.

## Phase 2–4 — Visibility matrix before / after

Window = created date within 3 days **or** `UnblockedUntil` grace. Admin `IsAdmin()` visibility is unchanged.

| Button | Before (pipelineStep) | After (frozen lifecycle) |
|--------|-----------------------|--------------------------|
| IN Punch | `pipelineStep == 3` | Window + (`EntryExit='Created'` **or** empty Entry). Permit outstanding no longer hides IN. |
| Upload Permit | `pipelineStep == 2` | Window + (Created **and** outstanding) **or** `EntryExit='Entry'` (PR #64 additional files). |
| Add Docs | `pipelineStep == 4` | Unchanged (`pipelineStep == 4`). |
| OUT Punch | `pipelineStep == 5` | Window + Entry + worker count &gt; 0. CSM pending does not hide OUT. Close still not on 360. |
| Force OUT / Unblock / Bypass / Reset / Cancel / Rollback / Resubmit / Swap / Delete | Same predicates as `e2c773d` | Same predicates. |
| View Raw / Edit Core | Hidden | Hidden. |

Deleted / Cancelled: no actions; Overview action bar hidden.

### Intended hop set by state (window open)

| State | Overview | Must not happen |
|-------|----------|-----------------|
| Created, permit-required, no files | IN + Permit | Permit as a gate that hides IN |
| Created, skip-permit or permit already Yes | IN | — |
| Entry, 0 workers | IN (+ Permit) | — |
| Entry, workers, permit outstanding | Permit + OUT (+ Add Docs if CSM step 4) | Blocking OUT until CSM |
| Entry, workers, permit complete | Permit (additional files) + OUT | Treating as Closed |
| Exit / Closed | no hops | Close & Send / Finalize on 360 |

## Phase 5 — Security / Phase C debt

Authorization is **not** weakened. Existing `IsAdmin()` visibility on Bypass, Reset, Cancel, Rollback, Unblock, and lockout Force OUT is preserved.

Recorded for Phase C (not implemented here):

- Click-time `IsAdmin()` on Unblock, Bypass, Reset, Cancel, Rollback, Delete, Resubmit, Force OUT, Swap Date.
- Optionally hide in-window Force OUT / Delete / Swap / Resubmit from non-admin.
- `InvalidateWorker` missing SET; permit download `Id AND JOBID`.
- Do not enable Raw Inspector or Edit Core in this phase.

## UAT mapping

### Phase A still holds (CR009-UAT-011 → CR009-UAT-020)

| ID | Scenario | Phase B expectation |
|----|----------|---------------------|
| CR009-UAT-011 | Overview label order | Unchanged markup: Create → IN → Permit → OUT → Close & Send → Approval |
| CR009-UAT-012 | Hop set vs `a75bb1e` | **Superseded for hops** by CR009-UAT-026–028. Phase A “unchanged matrix” no longer applies. |
| CR009-UAT-013 | Details fields | Unchanged |
| CR009-UAT-014 | Permits grid + download | Unchanged handler |
| CR009-UAT-015 | Manpower grid | Unchanged |
| CR009-UAT-016 | TBT + SOP grids | Unchanged |
| CR009-UAT-017 | Inspector / Edit Core | Still hidden (`ResetActionButtonVisibility`) |
| CR009-UAT-018 | Created job labels | Not Closed/Approved; IN hop now visible |
| CR009-UAT-019 | Entry after IN | Entry shown; V2 IN still no code-3 gate |
| CR009-UAT-020 | Permit pending Entry + `FinalUpldStatus=No` | Permit hop visible (UAT-027) |

### Phase B acceptance (spec §12)

| ID | Scenario | Expected |
|----|----------|----------|
| CR009-UAT-026 | IN hop on Created permit-required job | Hop visible; V2 inbox still lists it; **must not** require permit first |
| CR009-UAT-027 | Permit hop while Entry after IN | Hop visible; V2 inbox `EntryExit='Entry'` |
| CR009-UAT-028 | Permit-before-IN not required | Created job can still IN on V2; 360 does not hide IN behind permit |
| CR009-UAT-029 | OUT hop still encoded | `btn_Act_OutPunch_Click` still `EncodeJobID`; same JOBID |

Always regress: UAT-046, UAT-047, UAT-048, UAT-049, UAT-006, UAT-014, UAT-015, UAT-029, UAT-040, UAT-004, UAT-005.

## Regression proof (PR #59–#65, #71)

Phase B does not touch V2 pages, permit SQL, Close & Send, encoding helpers, hub KPIs, or Phase A markup. Compare blobs on this branch vs tag `v2.1.0-jobid-remediation` (`a75bb1e`) and vs `e2c773d` for the cockpit aspx.

| Frozen file | PRs | Must match tag `a75bb1e` |
|-------------|-----|--------------------------|
| `job_inpunch_v2.aspx.cs` | #59, #62 | IN eligibility: no `MasterStatusCode='3'` gate |
| `create_jobid_v2.aspx.cs` | #59 comments, #63 | WO ViewState |
| `job_outpunch_v2.aspx` / `.aspx.cs` | #60, #62 | Close writes Out-Punch Done / `4` / Exit |
| `job_permitupload_v2.aspx.cs` | #61, #62, #64 | Permit writes + inbox `Active` AND `Entry` |
| `jobs_and_manpower_v2.aspx.cs` | #65 | Hub KPIs |
| `job_360_view.aspx` | #71 | Phase A six-tab markup |

`job_360_view.aspx.cs` encoding redirects (`EncodeJobID` on Permit / IN / OUT hops) must remain byte-identical in the handlers. Only visibility methods change.

## Non-goals (stop conditions)

- `UpdateJOBTable1` / `btn_FinalizeShift` on JOB360
- New `MasterStatusCode` or JOB states
- Inbox / KPI SQL
- Enabling Inspector or Edit Core
- CR-001 wizard chrome
