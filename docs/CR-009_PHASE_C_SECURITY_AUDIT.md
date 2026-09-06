# CR-009 Phase C — JOB360 Security Audit

**Status:** Audit only. No executable, SQL, permission, or helper changes in this document.  
**Baseline:** `Jul_to_Sep_2026_Suport_N_Dev_Works` at `37a34d8` (Phase B squash merge).  
**File under audit:** `WebApplication1/bussiness/production/job_360_view.aspx.cs`  
**Markup (IDs/bindings only):** `WebApplication1/bussiness/production/job_360_view.aspx` (Phase A blob `38f9cbae`, unchanged by Phase B).

Change type: Documentation (forensic). Implementation of fixes requires a classified Phase C code CR.

## Verdict

JOB360 **visibility is not authorization**. Almost every live write handler can be invoked by a forged WebForms postback from any authenticated session that can load a JOBID. Grid Edit/Drop buttons are hidden for non-admins, but their `RowCommand` handlers do not re-check `IsAdmin()`. Permit download is keyed by **file `Id` only** (IDOR). `InvalidateWorker` still issues `UPDATE tbl_jobs WHERE JOBID=@JOBID` with **no SET**.

Phase C implementation must add click-time `IsAdmin()` on writes, bind permit download to `Id AND JOBID`, and repair the broken job UPDATE. Do **not** change Bypass / Cancel / Force OUT SQL in that CR unless a nested CR says so.

`IsAdmin()` today: `Session["USERTYPE"]` is `Admin` or `Office Staff` (`job_360_view.aspx.cs` lines 47–53). Office Staff ≡ Admin is existing; narrowing it needs a separate CR.

---

## 1. Threat model (this page)

| Actor | What they can do today after login |
|-------|--------------------------------------|
| Authenticated Viewer / Site Staff | Unscoped JOBID search (`Load360View` `SELECT * FROM tbl_jobs WHERE JOBID=@JOBID`). Overview hops. Open Admin tab. POST any `btn_Act_*` whose control exists in the tree (`Visible=false` does not block `__EVENTTARGET`). |
| Supervisor | Same 360 surface. Real punch/close remains on V2 (creator inboxes). |
| Admin / Office Staff | Intended tower + overrides. Writes still lack click-time `IsAdmin()` except the unused `btn_Act_UnblockJob_Click` stub. |

WebForms note: `Visible="false"` and matrix `Visible=false` hide chrome. They are **not** server authorization. Confirm dialogs (`OnClientClick`) are client-only.

Master `webmaster.Master` and this page both check session only when `!IsPostBack` (master lines 22–26; 360 lines 16–23). Platform-wide pattern; still not a substitute for per-handler `IsAdmin()`.

---

## 2. Handler inventory

Legend: **Write** = INSERT/UPDATE/DELETE or `ExecuteNonQuery` / SP. **DL** = file download. **Redir** = `Response.Redirect`. **Vis** = how the control is shown. **Click-time** = `IsAdmin()` (or equivalent) at the start of the handler before SQL.

| Handler | Lines | Write | DL | Redir | Requires admin (spec) | Current protection | Click-time `IsAdmin()` |
|---------|-------|-------|----|-------|------------------------|--------------------|------------------------|
| `Page_Load` | 14–42 | No | No | login if no session (GET only) | Session 3 keys | `!IsPostBack` session check | N/A (no write) |
| `btn_search_Click` | 66–77 | No | No | No | Viewer+ | Session assumed | No |
| `btn_reset_Click` | 79–83 | No | No | No | Viewer+ | UI only | No |
| `Load360View` | 85–197 | No | No | No | Viewer+ | JOBID parameterized; **no company/creator** | No |
| `LoadPermits` / `LoadTBT` / `LoadSOP` / `LoadManpower` | 202–295 | No | No | No | Viewer+ | JOBID parameterized | No |
| `chk_ShowDeleted_CheckedChanged` | 252–271 | No | No | No | Viewer+ | Reloads grid | No |
| `gvManpower_RowCommand` / `InvalidateWorker` | 297–348 | **Yes** attendance (+ broken job UPDATE) | No | No | Admin | Grid `Visible='<%# IsAdmin() %>'` only | **No** |
| `gvManpower_RowCommand` / `EditWorker` | 349–384 | No (read Id) | No | No | Admin | Grid visibility only | **No** |
| `gvManpower_RowDataBound` | 388–403 | No | No | No | — | Highlight only | N/A |
| `btnSaveEdit_Click` | 405–454 | **Yes** attendance | No | No | Admin if used | **Not in markup** | **No** |
| `btn_EditCoreDetails_Click` | 456–471 | No | No | No | Admin if enabled | Markup `Visible=false`; matrix keeps false | **No** |
| `btn_SaveCoreDetails_Click` | 473–501 | **Yes** `tbl_jobs` | No | No | Admin if enabled | Modal exists; opener hidden | **No** |
| `btn_SaveWorkerEdit_Click` | 504–538 | **Yes** attendance | No | No | Admin | Modal; opener grid-gated | **No** |
| `gvPermits_RowCommand` / `DownloadDoc` | 540–574 | **Yes** `DownloadStatus` | **Yes** | No | Viewer+ today; spec Phase C: session + Id+JOBID | Authenticated only | **No** |
| `EvaluateSmartLifecycle` | 592–844 | No | No | No | — | Read | N/A |
| `EvaluateActionMatrix` (+ helpers) | 953–1147 | No | No | No | Visibility only | `IsAdmin()` for **showing** some buttons | N/A (not a write) |
| `EvaluateActionMatrix_OLD` | 846–951 | No | No | No | — | Dead; not called | N/A |
| `btn_Act_UploadPermit_Click` | 1150–1154 | No | No | V2 encoded | Hop | V2 inbox is write gate | No (correct for hop) |
| `btn_Act_InPunch_Click` | 1156–1160 | No | No | V2 encoded | Hop | Same | No (correct for hop) |
| `btn_Act_OutPunch_Click` | 1162–1166 | No | No | V2 encoded | Hop | Same | No (correct for hop) |
| `btn_Act_AddDocs_Click` | 1168 | No | No | raw `jobid` | Hop | Target page must authorize | No |
| `btn_Act_SwapDate_Click` | 1169 | No (on 360) | No | raw `jobid` | Phase C: Admin | Visibility: no first IN; **not** `IsAdmin` | **No** |
| `btn_Act_Unblock_Click` | 1171–1187 | **Yes** | No | No | Admin | Visibility `IsAdmin` + blocked | **No** |
| `btn_Act_ForcePermitBypass_Click` | 1194–1210 | **Yes** | No | No | Admin | Visibility `IsAdmin` + code `1` | **No** |
| `btn_Act_ResetToCreated_Click` | 1213–1236 | **Yes** DELETE permits + UPDATE job | No | No | Admin | Visibility `IsAdmin` + code `3` + 0 workers | **No** |
| `btn_Act_CancelShift_Click` | 1239–1260 | **Yes** | No | No | Admin | Visibility `IsAdmin` + code 1 / empty 3 | **No** |
| `btn_Act_Delete_Click` | 1262–1300 | **Yes** | No | No | Phase C: Admin | Visibility: no first IN; **not** `IsAdmin` | **No** |
| `btn_Act_Resubmit_Click` | 1302–1353 | **Yes** | No | No | Phase C: Admin | Visibility: Rejected/Returned/Cancelled; **not** `IsAdmin` | **No** |
| `btn_Act_Resubmit_Click_OLD` | 1355–1411 | **Yes** (different SQL) | No | No | — | **Not wired** | **No** |
| `btn_Act_ForceOut_Click` | 1413–1504 | **Yes** SP + close-state UPDATE | No | No | Phase C: Admin always | In-window: **any user**. Lockout: vis `IsAdmin` | **No** |
| `btn_Act_ForceOut_Click_OLD` | 1506–1568 | **Yes** | No | No | — | **Not wired** | **No** |
| `btn_Act_AdminRollback_Click` | 1570–1624 | **Yes** | No | No | Admin | Visibility `IsAdmin` + Approved | **No** |
| `btn_Act_AdminRollback_Click_OLD` | 1626–1662 | **Yes** | No | No | — | **Not wired** | **No** |
| `btn_Act_ViewRawData_Click` | 1682–1762 | No | No (binds grids; no file export) | No | Phase D: Admin | Markup `Visible=false`; matrix keeps false | **No** |
| `btn_Act_UnblockJob_Click` | 1764–1808 | No (SQL commented out) | No | No | Admin | **Not in markup**; **has** click-time `IsAdmin()` | **Yes** (dead) |

No `btn_FinalizeShift` / `UpdateJOBTable1` on this page.

---

## 3. Authorization matrix (risk)

| Handler | Visibility | Click-time check | Class | Risk |
|---------|------------|------------------|-------|------|
| IN / Permit / OUT hops | Phase B window + frozen hops | None (redirect) | **Safe** | V2 `ActiveJOB_Checker` remains the write gate. Do not treat Base64 as auth. |
| Add Docs hop | `pipelineStep == 4` | None | **Informational** | Raw `jobid` QS; target page must authorize. Same as UAT-049 family. |
| Swap Date hop | No first IN | None | **Needs Phase C fix** | Any authenticated user can POST redirect; pre-IN date change lives on `swap_jobdate.aspx`. |
| Unblock | `IsAdmin` + blocked + not Exit | **Missing** | **Needs Phase C fix** | Forged POST grants 24h grace (`DATEADD(HOUR, 24, GETDATE())`). |
| Bypass | `IsAdmin` + code `1` | **Missing** | **Needs Phase C fix** | Writes `MasterStatusCode='3'`, `EntryExit='Entry'` without IN. **Do not change SQL** in Phase C. |
| Reset to Created | `IsAdmin` + code `3` + 0 workers | **Missing** | **Needs Phase C fix** | Hard `DELETE FROM tbl_jobspermit`; rolls job to Created / code `1`. |
| Cancel Shift | `IsAdmin` + (code `1` or empty code `3`) | **Missing** | **Needs Phase C fix** | Writes unofficial `MasterStatusCode='6'`, `EntryExit='Deleted'`. **Do not change SQL** unless nested CR. |
| Delete | No first IN; **not** `IsAdmin` | **Missing** | **Needs Phase C fix** | Visible to non-admin pre-IN. Writes unofficial `'0'` / Deleted. High. |
| Resubmit | Rejected/Returned/Cancelled; **not** `IsAdmin` | **Missing** | **Needs Phase C fix** | Visible to non-admin. Sets code `3` / Entry / `JOB_Status='Permit Uploaded'`. Live path does **not** reset attendance (tooltip overstates). |
| Force OUT | In-window: any user if Entry + first IN + created &lt; today. Lockout: vis `IsAdmin` | **Missing** | **Needs Phase C fix** | **Highest 360 write.** Calls `SP_Update_AttendancePunchOUT` then writes Close states (`Out-Punch Done` / `'4'` / `Exit`). Not certified Close & Send (`btn_FinalizeShift`). **Do not change SQL** in Phase C. |
| Admin Rollback | `IsAdmin` + Approved | **Missing** | **Needs Phase C fix** | Sets `Incharge_Approval='Returned'`, keeps code `4` / Out-Punch Done. |
| InvalidateWorker | Grid `IsAdmin()` | **Missing** | **Needs Phase C fix** | Soft-delete attendance; broken master UPDATE (no SET) can throw. |
| EditWorker + `btn_SaveWorkerEdit_Click` | Grid `IsAdmin()`; save always in modal | **Missing** | **Needs Phase C fix** | Save `WHERE Id=@Id` only — cross-JOB attendance edit if Id guessed. |
| `btnSaveEdit_Click` | Unwired | **Missing** | **Needs Phase C fix** if kept; else delete in refactor | Duplicate attendance write, Id-only. |
| `btn_SaveCoreDetails_Click` | Opener hidden | **Missing** | **Needs Phase C fix** if modal remains reachable | Updates shift/title by JOBID with no `IsAdmin()`. |
| Permit `DownloadDoc` | Any loaded dashboard | **Missing** (and no JOBID in WHERE) | **Needs Phase C fix** | IDOR: download + `DownloadStatus=1` by numeric Id. Filename concatenated into `TransmitFile`. |
| Raw Inspector | Hidden | **Missing** | **Informational** (Phase D) | `SELECT *` PII if enabled. Permits already exclude blob. Add click-time `IsAdmin()` when enabling. |
| Search / Reset / grids bind / stepper | Session + JOBID | N/A | **Safe** / **Informational** | Unscoped JOBID lookup is a tenancy CR, not Phase C. |
| `*_Click_OLD` / `btn_Act_UnblockJob_Click` | Not in markup | OLD: no; UnblockJob: yes | **Informational** | Do not wire `_OLD` (different SQL). UnblockJob is the only live-style click-time pattern — copy that guard onto real writes. |
| Close & Send | **None on 360** | N/A | **Safe** | Remains `job_outpunch_v2`. |

---

## 4. Write-path audit

All live commands observed use `@` parameters (no string-concatenated JOBID in UPDATEs). `LoadManpower` interpolates `deleteFilter` from a checkbox only (`AND (DeleteStatus = 0 OR DeleteStatus IS NULL)` vs empty) — not user text.

| Location | SQL / SP | Parameterized | Ownership | JOBID validation | Notes |
|----------|----------|---------------|-----------|------------------|-------|
| `InvalidateWorker` att | `UPDATE tbl_attendance SET DeleteStatus=1, AttendanceStatus='Invalidated', … WHERE JOBID=@JOBID AND EmployeeWrk=@Workman` | Yes | JOBID from `txt_jobid`; workman from `CommandArgument` | Loaded textbox only | No `IsAdmin()`. Argument is **EmployeeWrk**, not attendance Id. |
| `InvalidateWorker` job | `UPDATE tbl_jobs WHERE JOBID=@JOBID` | Yes | JOBID from textbox | Same | **No SET clause.** Will fail at SQL Server. Spec §9: drop no-op or add specified SET; transaction with attendance. **Do not invent a headcount formula.** |
| `btnSaveEdit_Click` | `UPDATE tbl_attendance … WHERE Id=@Id` | Yes | **Id only** | Jobid used only for log | Unwired. No JOBID match. |
| `btn_SaveWorkerEdit_Click` | `UPDATE tbl_attendance … WHERE Id=@Id` | Yes | **Id only** | `Load360View(txt_jobid)` after | Cross-job Id. No `IsAdmin()`. |
| `btn_SaveCoreDetails_Click` | `UPDATE tbl_jobs SET JOB_Shift, JOB_Title, … WHERE JOBID=@JOBID` | Yes | JOBID textbox | No tenant | No `IsAdmin()`. |
| Permit download SELECT | `SELECT Name FROM tbl_jobspermit WHERE Id=@Id` | Yes | **Id only** | **No JOBID** | IDOR. |
| Permit download UPDATE | `UPDATE tbl_jobspermit SET DownloadStatus=1 WHERE Id=@Id` | Yes | **Id only** | **No JOBID** | Side-effect write. |
| `btn_Act_Unblock_Click` | `UPDATE tbl_jobs SET IsBlocked=0, JOBID_Status='Active', BlockedTimestamp=NULL, UnblockedUntil=DATEADD(HOUR,24,GETDATE()) WHERE JOBID=@JOBID` | Yes | JOBID textbox | No re-check blocked | Spec: keep 24h SQL (UAT-032). |
| Bypass | `UPDATE tbl_jobs SET PermitUpload='Bypassed', FinalUpldStatus='Yes', JOB_Status='Permit Bypassed (Emergency)', MasterStatusCode='3', EntryExit='Entry' WHERE JOBID=@JOBID` | Yes | JOBID textbox | No re-check code `1` | Unofficial unlock. **Keep SQL.** |
| Reset | `DELETE FROM tbl_jobspermit WHERE JOBID=@JOBID` then UPDATE job to Created / `'1'` / FileCount=0 | Yes | JOBID textbox | No re-check 0 workers | Hard delete permits. Not transactional with the UPDATE. |
| Cancel | `JOBID_Status='Cancelled', JOB_Status='Voided by Admin', MasterStatusCode='6', EntryExit='Deleted', DeleteStatus=1, …` | Yes | JOBID textbox | No re-check empty/code 1 | Unofficial `'6'`. **Keep SQL** (UAT-036). |
| Delete | `DeleteStatus=1, JOBID_Status='Deleted', MasterStatusCode='0', EntryExit='Deleted', Incharge_Approval='Cancelled', …` | Yes | JOBID textbox | No attendance re-check | Unofficial `'0'`. Visible without `IsAdmin`. |
| Resubmit (live) | `MasterStatusCode='3', JOB_Status='Permit Uploaded', EntryExit='Entry', … WHERE JOBID=@JOBID` | Yes | JOBID textbox | No approval-status re-check | Transaction wraps a single UPDATE. Does **not** invalidate attendance. |
| Resubmit OLD | Job + attendance UPDATEs; attendance SQL **missing SET** (`UPDATE tbl_attendance Approval_Date=NULL`) | Partial | — | — | Must stay unwired. |
| Force OUT | `SELECT` stuck rows; `SP_Update_AttendancePunchOUT`; `UPDATE tbl_jobs SET JOBID_Status='Active', JOB_Status='Out-Punch Done', MasterStatusCode='4', EntryExit='Exit'` | Yes | JOBID textbox | No Entry/code-3 re-check | Transactional. Same close triple as `UpdateJOBTable1`. **Not** UAT-040. **Keep SQL.** |
| Rollback | `Incharge_Approval='Returned', MasterStatusCode='4', JOB_Status='Out-Punch Done', …` | Yes | JOBID textbox | No Approved re-check | Does not touch `EntryExit`. |
| Force OUT OLD / Rollback OLD | Similar writes, no transaction on OLD Force OUT | Yes | — | — | Unwired. |
| `JobWorkflowLogger.LogAction` | Called after some commits | — | Uses `Session["WORKMAN"]` | — | Logging is not authorization. |

No INSERT on this page. Stored procedure: **`SP_Update_AttendancePunchOUT`** only (Force OUT live + OLD).

---

## 5. Download / inspector audit

| Surface | How | JOB ownership | Admin required | Enumeration risk |
|---------|-----|---------------|----------------|------------------|
| Permit grid download | `gvPermits` `CommandArgument=Eval("Id")` → `DownloadDoc` | **No.** `WHERE Id=@Id` only | No | **High IDOR.** Any logged-in user who can POST `DownloadDoc` with another job’s Id gets `Name` and `TransmitFile(~/erp_images/Permits/{Name})`. Also sets `DownloadStatus=1`. |
| Filename | `content-disposition` + `Path.Combine(..., fileName)` | — | — | If `Name` contains `..\` or absolute path, traversal is possible. Phase C should constrain to the permits folder (still no SQL change to V2). |
| Other attachments (TBT/SOP) | Grids are read-only; no download command | N/A | N/A | None on 360. |
| Raw Inspector | Four inner grids; permits **without** blob (`Name`, metadata only) | JOBID parameterized | Hidden; **no** click-time `IsAdmin()` | If enabled, any poster can `SELECT *` jobs/attendance/TBT/SOP for any JOBID. Phase D: enable only with click-time `IsAdmin()`. No file export control found. |
| keepAlive GET | `job_360_view.aspx` lines 865–880; `$.get(...keepAlive=timestamp)` | If URL already has `jobid`, `Page_Load` **re-runs `Load360View`** | Session on GET | Spec Phase D: ignore `keepAlive` so load does not re-fire. Not a write. |

---

## 6. Known findings — verification

Evidence is from `37a34d8` `job_360_view.aspx.cs`. **Not fixed.**

### 6.1 `InvalidateWorker` — CONFIRMED

- Markup: `job_360_view.aspx` 557–560, `Visible='<%# IsAdmin() && … %>'`.
- Handler 297–348: **no** `IsAdmin()`.
- Attendance UPDATE parameterized (312–317).
- Job UPDATE 328–329: `UPDATE tbl_jobs WHERE JOBID = @JOBID` — **no SET**. Spec §9 / CR009-UAT-034.
- Not wrapped in a transaction: attendance can commit even if job UPDATE throws.

### 6.2 Raw Inspector — CONFIRMED (hidden; unsafe if invoked)

- Markup 693 `Visible="false"`; `ResetActionButtonVisibility` forces false (1028).
- Handler 1682–1762: no `IsAdmin()`. `SELECT *` jobs, attendance, TBT, SOP by JOBID. Permits exclude blob (1714–1717).
- Phase D to enable; Phase C may add the click-time guard without showing the button.

### 6.3 Force OUT — CONFIRMED

- Visibility: `ApplyInWindowForceOutVisibility` 1112–1117 — **any user** when Entry + first IN + created date &lt; today. Lockout path 1120–1126 requires `IsAdmin()` to **show** the button.
- Handler 1413–1504: no click-time `IsAdmin()`. SP punch-out + close triple `'4'` / Exit / Out-Punch Done.
- Spec: Phase C click-time Admin; **do not** replace Close & Send; **do not** change this SQL. UAT-030, UAT-035.

### 6.4 Reset to Created — CONFIRMED

- Visibility `IsAdmin` + code `3` + 0 workers (`ApplyAdminOverrideVisibility` 1042–1046).
- Handler 1213–1236: no click-time `IsAdmin()`. Hard DELETE all permits for JOBID, then reset job. No attendance-count re-check. Not transactional.

### 6.5 Force Permit Bypass — CONFIRMED

- Visibility `IsAdmin` + `masterCode == "1"` (1036–1040).
- Handler 1194–1210: no click-time `IsAdmin()`. Writes Entry + code `3` without IN. **Keep SQL.**

### 6.6 Cancel Shift — CONFIRMED

- Visibility `IsAdmin` + (code `1` or empty code `3`).
- Handler 1239–1260: no click-time `IsAdmin()`. Writes `'6'` / `EntryExit='Deleted'`. **Keep SQL** (UAT-036).

### 6.7 Admin Rollback — CONFIRMED

- Visibility `IsAdmin` + Approved (1048–1051).
- Handler 1570–1624: no click-time `IsAdmin()`. Sets Returned + code `4`.

### 6.8 Delete — CONFIRMED

- Visibility `ApplyPrePunchOverrideVisibility` 1137–1146: **any user** with no first IN (including lockout copy).
- Handler 1262–1300: no click-time `IsAdmin()`. Writes `'0'` / Deleted. Spec Phase C: Admin + optionally hide from non-admin.

### 6.9 Swap Date — CONFIRMED

- Same pre-punch visibility as Delete; **not** `IsAdmin`.
- Handler 1169: redirect `swap_jobdate.aspx?jobid={txt_jobid}` raw. 360 does not write. Phase C: click-time `IsAdmin()` before redirect; optionally hide.

### 6.10 Resubmit — CONFIRMED

- Visibility 1129–1134: Rejected/Returned/Cancelled; **not** `IsAdmin`.
- Live handler 1302–1353: no click-time `IsAdmin()`. Job-only UPDATE to code `3` / Permit Uploaded / Entry. Markup tooltip claims attendance invalidation — **live code does not**.
- `_OLD` 1355–1411: different SQL + broken attendance UPDATE; must remain unwired.

### Additional confirmed gaps

- Permit download Id-only (540–559) — CR009-UAT-033.
- Worker save Id-only (511–519).
- EditWorker load Id-only (356).
- Core save no `IsAdmin()` (473–501); opener hidden.
- Hops IN/Permit/OUT still `EncodeJobID` (1154, 1160, 1166) — do not change.
- No company / `Creator_Workman` on 360 writes (spec §8). Tenancy CR; do not fail Phase C for missing isolation.

---

## 7. Phase C implementation checklist

Do **not** implement in this audit PR. Next code CR, `job_360_view.aspx.cs` handlers only unless noted.

### Must do (authorization + two spec SQL repairs)

1. Add click-time `if (!IsAdmin()) { notify; return; }` at the top of every **live write** handler, copying the pattern at `btn_Act_UnblockJob_Click` (1767–1771):
   - `btn_Act_Unblock_Click`
   - `btn_Act_ForcePermitBypass_Click`
   - `btn_Act_ResetToCreated_Click`
   - `btn_Act_CancelShift_Click`
   - `btn_Act_Delete_Click`
   - `btn_Act_Resubmit_Click`
   - `btn_Act_ForceOut_Click`
   - `btn_Act_AdminRollback_Click`
   - `gvManpower_RowCommand` (`InvalidateWorker` and `EditWorker`)
   - `btn_SaveWorkerEdit_Click`
   - `btn_SaveCoreDetails_Click`
   - `btnSaveEdit_Click` if it remains
2. `gvPermits_RowCommand`: `SELECT`/`UPDATE` with `Id=@Id AND JOBID=@loadedJobId` (`txt_jobid` trimmed). Reject mismatch. Constrain `TransmitFile` to `~/erp_images/Permits/`.
3. `InvalidateWorker`: wrap attendance + job in one transaction; **fix or remove** the job UPDATE missing SET (spec §9). Do not invent a new `ManpowerCount` formula.
4. Worker save / EditWorker SELECT: `Id AND JOBID=@loadedJobId`.
5. Optionally (spec Phase C): hide Delete, Swap Date, in-window Force OUT, and Resubmit unless `IsAdmin()` — visibility only, after click-time exists.
6. Swap Date: click-time `IsAdmin()` before redirect (360 still must not write the date).

### Must not do

- Change Bypass / Cancel / Force OUT **SQL** or `MasterStatusCode` values.
- Add `btn_FinalizeShift` / `UpdateJOBTable1` to 360.
- Change `EncodeJobID` hops, V2 inboxes, hub KPIs, or Phase A/B markup IDs.
- Enable Raw Inspector or Edit Core (Phase D / product CR). Adding click-time `IsAdmin()` while keeping them hidden is OK.
- Extract helpers (this CR is audit; extraction is CR-008 / later refactor).
- Wire `*_Click_OLD` or `btn_Act_UnblockJob_Click`.
- Treat Base64 as authorization.

### Suggested order

1. Click-time `IsAdmin()` on Force OUT, Delete, Bypass, Cancel, Reset, Rollback, Unblock, Resubmit.  
2. Grid commands + worker/core saves.  
3. Permit download JOBID bind + path constrain.  
4. InvalidateWorker SET/transaction.  
5. Optional visibility tightening for non-admin.  
6. UAT-030–037.

---

## 8. UAT mapping (CR009-UAT-021 … CR009-UAT-030)

IDs 021–025 are Phase A lifecycle (still the frozen baseline). 026–029 are Phase B hops (merged `37a34d8`). **030 is the first Phase C security UAT.** This audit does not execute UAT; it records current vs expected.

| ID | Spec phase | Scenario | Expected | Audit result on `37a34d8` |
|----|------------|----------|----------|---------------------------|
| CR009-UAT-021 | A | Additional permit while Entry | Still in permit inbox | **Holds** (no 360 inbox SQL). 360 Permit hop while Entry (Phase B). |
| CR009-UAT-022 | A | After Close (`Exit`) | Not in permit inbox; Closed label | **Holds**. 360 hops hide on Exit. Close remains V2. |
| CR009-UAT-023 | A | OUT pending code 3 + Entry | Not Closed | **Holds**. Force OUT can still close via override (not this UAT). |
| CR009-UAT-024 | A | All workers OUT, Close not sent | Still Entry/code 3; **no** 360 Finalize | **Holds** (no `btn_FinalizeShift`). |
| CR009-UAT-025 | A | Closed Out-Punch Done / 4 / Exit | Closed; approval pending copy | **Holds**. |
| CR009-UAT-026 | B | IN hop on Created permit-required | Hop; must not require permit first | **Holds** (Phase B). |
| CR009-UAT-027 | B | Permit hop while Entry | V2 inbox Entry | **Holds**. |
| CR009-UAT-028 | B | Permit-before-IN not required | Created can IN on V2 | **Holds**. |
| CR009-UAT-029 | B | OUT hop still encoded | Same JOBID | **Holds** (`EncodeJobID` lines 1162–1166). |
| CR009-UAT-030 | C | Site Staff POST Force OUT | **Rejected server-side** | **FAIL today.** Handler 1413 has no `IsAdmin()`. In-window visibility is any user. **This is the Phase C gate.** |

Remaining Phase C IDs (spec §12; implement with the code CR, not this audit):

| ID | Scenario | Expected vs today |
|----|----------|-------------------|
| CR009-UAT-031 | Site Staff POST Bypass / Cancel / Delete | Rejected — **FAIL today** (no click-time; Delete not even vis-gated by admin) |
| CR009-UAT-032 | Admin Unblock | 24h SQL unchanged — SQL is correct; still add click-time `IsAdmin()` |
| CR009-UAT-033 | Permit download other JOB’s Id | Not found / forbidden — **FAIL today** (Id only) |
| CR009-UAT-034 | InvalidateWorker | Attendance invalid; no missing-SET error — **FAIL today** (no SET) |
| CR009-UAT-035 | Force OUT vs Close & Send | Override path; OUT page close still works — do not change Force OUT SQL |
| CR009-UAT-036 | Cancel still writes `'6'` | Keep unless nested CR |
| CR009-UAT-037 | Historical >3 day | Read works; hops follow lockout |

Always regress: UAT-046, UAT-047, UAT-048, UAT-049, UAT-006, UAT-014, UAT-015, UAT-029, UAT-040, UAT-004, UAT-005.

---

## 9. Discovery / spec cross-check

| Source | Binding used in this audit |
|--------|----------------------------|
| `docs/CR-009_IMPLEMENTATION_SPEC.md` §6, §8, Phase C, UAT-030–037 | Click-time `IsAdmin()`; download Id+JOBID; InvalidateWorker SET; do not change Bypass/Cancel/Force OUT SQL |
| `docs/CR-009_PHASE_B_ACTION_MATRIX.md` | Phase C debt list; visibility ≠ auth |
| `docs/MAINTENANCE_GUIDELINES.md` | Do not weaken session/creator checks; Change Request for auth behavior |
| `docs/JOBID_LIFECYCLE_FUNCTIONAL_AUDIT.md` | Permit download by Id only already listed as IDOR elsewhere; 360 matches that pattern |
| `docs/CR-009_JOB360_ADMIN_COCKPIT_DISCOVERY.md` | **Not in this branch** (discovery PR #69 was not merged). Spec + Phase B doc + this aspx.cs are sufficient. |

---

## 10. Files in this audit PR

| File | Role |
|------|------|
| `docs/CR-009_PHASE_C_SECURITY_AUDIT.md` | This document (new) |

No `.aspx`, `.aspx.cs`, `.sql`, helpers, or config changes.
