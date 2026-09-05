# JOBID Lifecycle Functional Audit

**Audit type:** Forensic, evidence-backed functional comparison (not UI comparison)  
**Solution:** ASP.NET WebForms 4.8  
**Source branch:** `Jul_to_Sep_2026_Suport_N_Dev_Works`  
**Audit date:** 05-Sep-2026  
**Method:** Reverse-engineering of markup, code-behind, UserControls, Master Page, helpers, Session/QueryString, SQL call sites, and navigation. Every claim below cites file + method. Stored-procedure *bodies* (`SP_InsertInto_JOBSTable`, `SP_InsertInto_AttendanceTable`, `SP_Update_AttendancePunchOUT`, `SP_Insert_EmployeeAttendance`) are **not present in this repository**; only call contracts are evidenced.

**Primary limitation:** Internal DML, uniqueness constraints, and duplicate checks inside stored procedures cannot be asserted from source.

---

## Post-audit fix — UAT-006 / UAT-021 (05-Sep-2026)

Restore legacy-compatible IN-Punch eligibility for permit-required jobs. V2 no longer waits for `MasterStatusCode='3'` before listing a job on IN-Punch.

| Field | Value |
|---|---|
| **UAT IDs** | `UAT-006`, `UAT-021` |
| **Finding resolved** | Critical: V2 blocked IN-punch for permit-required jobs until `MasterStatusCode='3'`. Legacy listed Active 3-day creator jobs with no status-code gate. |
| **Files changed** | `WebApplication1/bussiness/production/job_inpunch_v2.aspx.cs`; `WebApplication1/bussiness/production/create_jobid_v2.aspx.cs` (comments only — insert writes unchanged) |
| **Methods changed** | `job_inpunch_v2.aspx.cs` `ActiveJOB_Checker()`; `create_jobid_v2.aspx.cs` `Insert_JOBData()` comments documenting legacy ARC vs skip-permit writes |
| **Methods unchanged** | `CheckDuplicateEntry()`, `btn_finalsubmit_Click()` duplicate `COUNT` on `AttendanceStatus='Entry'`; `Insert_JOBData()` skip-permit branch (`WO_MasterStatusCode == "3"` → `FinalUpldStatus=Yes`, `PermitUpload=N/A`) |

**Eligibility after this fix**

- Permit-required create (`MasterStatusCode='1'`, `EntryExit='Created'`, `FinalUpldStatus='No'`) **appears in V2 IN-Punch immediately** (same predicates as `job_inpunch.aspx.cs` `ActiveJOB_Checker()`: last 3 days, `Creator_Workman=@Workman`, `JOBID_Status='Active'`).
- Skip-permit / non-ARC create (`MasterStatusCode='3'`, `FinalUpldStatus='Yes'`) is **unchanged** in `Insert_JOBData()`.
- Duplicate Entry protection is **unchanged** (`CheckDuplicateEntry` + final-submit `AttendanceStatus='Entry'` count).
- Inbox SQL remains **parameterized** (`@Workman`).

---

## Post-audit fix — UAT-029 / UAT-040 / UAT-035 (05-Sep-2026)

Restore legacy automatic shift-close semantics on V2 OUT-Punch, with a confirmation modal instead of a mandatory Finalize Shift step.

| Field | Value |
|---|---|
| **UAT IDs** | `UAT-029`, `UAT-040`, `UAT-035` |
| **Finding resolved** | Critical: V2 last OUT did not close the job. Close required `btn_FinalizeShift_Click()`. Forgotten Finalize left `MasterStatusCode='3'` / `EntryExit='Entry'` forever, so the job never appeared in approval (`JOB_Status='Out-Punch Done' AND EntryExit='Exit'`) and stayed on the pending-OUT dashboard. |
| **Files changed** | `WebApplication1/bussiness/production/job_outpunch_v2.aspx`; `WebApplication1/bussiness/production/job_outpunch_v2.aspx.cs` |
| **Methods changed** | `CheckPendingOUT()` (prompt confirmation when `CheckforPendingOUT==0`); `PromptCloseConfirmation()` (new); `ShowShiftClosedPopup()` (success modal); `btn_FinalizeShift_Click()` (Close & Send → existing `UpdateJOBTable1`) |
| **Methods unchanged** | `UpdateJOBTable1()` parameterized close (`JOBID_Status`, `JOB_Status='Out-Punch Done'`, `MasterStatusCode='4'`, `EntryExit='Exit'`); `CountChecker.CheckforPendingOUT` / `CheckforPendingPermit`; Blocked vs Active rule |

**Close flow after this fix**

1. Last OUT (or last Entry delete) → `CheckPendingOUT()` sees `CheckforPendingOUT==0` → confirmation modal.
2. **Review Again** dismisses the modal only (grid remains for review; no DB write).
3. **Close & Send** runs the same `UpdateJOBTable1(Blocked|Active, "Out-Punch Done", "4", "Exit")` as legacy.
4. Success modal confirms submission. Job matches approval inbox predicates immediately. Dashboard pending-OUT (`MasterStatusCode='3' AND EntryExit='Entry'`) drops the job. No new columns/states.

**Hardening (UAT-040A / UAT-040B)**

| Field | Value |
|---|---|
| **UAT IDs** | `UAT-040A` Double-click protection; `UAT-040B` Refresh during close |
| **Finding resolved** | Close & Send could be posted twice (double-click or browser refresh of the close POST). Second write is skipped; success modal still opens. |
| **Files changed** | `job_outpunch_v2.aspx`; `job_outpunch_v2.aspx.cs` |
| **Methods changed** | `btn_FinalizeShift_Click()` (idempotent Close & Send — control ID not renamed); `UpdateJOBTable1()` WHERE still-open; `TryReadJobCloseState()` / `IsAlreadyClosed()` / `ShowCloseSuccessUi()` (new); client `lockCloseAndSend()` |
| **Behavior unchanged** | First eligible close still writes `JOB_Status='Out-Punch Done'`, `MasterStatusCode='4'`, `EntryExit='Exit'`. No new columns/states. Notifications fire only when this request actually updated a row. |

- **UAT-040A:** `lockCloseAndSend()` disables Close & Send, Review Again, and shows the loader on first click. A second click returns false and does not post.
- **UAT-040B:** Before `UpdateJOBTable1()`, verify `MasterStatusCode='3'`, `EntryExit='Entry'`, and `CheckforPendingOUT()==0`. If already `Out-Punch Done` / `4` / `Exit`, skip the update and reopen the success modal. The UPDATE itself is `WHERE JOBID=@JOBID AND MasterStatusCode='3' AND EntryExit='Entry'`.

---

# PHASE 1 — JOBID Dependency Graph

## Menu / Hub entry

```
webmaster.Master  (CreateJOBS → jobs_and_manpower_v2.aspx)
                  (ManageJOBS → manage_jobid_v2.aspx)
        │
        ├── jobs_and_manpower.aspx          [LEGACY HUB]
        │       ├── create_jobid.aspx
        │       ├── job_permitupload.aspx
        │       ├── job_inpunch.aspx
        │       ├── job_outpunch.aspx
        │       ├── vw_supplyjobs.aspx
        │       └── vw_lineitemjobs.aspx
        │
        └── jobs_and_manpower_v2.aspx       [V2 HUB — default menu target]
                ├── create_jobid_v2.aspx
                ├── job_permitupload_v2.aspx
                ├── job_inpunch_v2.aspx
                ├── job_outpunch_v2.aspx
                ├── manage_jobid_v2.aspx
                ├── vw_supplyjobs.aspx
                └── vw_lineitemjobs.aspx
```

Evidence: `webmaster.Master` lines 226–227; `jobs_and_manpower.aspx` lines 51–85; `jobs_and_manpower_v2.aspx` lines 152–204.

## Legacy execution graph (actual redirects and filters)

```
login.aspx
    ↓  Session USERID, RolePermissionDB, UserRoleDB, USERNAME, WORKMAN, REGION
jobs_and_manpower.aspx
    ↓
create_jobid.aspx
    │  SP_InsertInto_JOBSTable
    │  ARC  → MasterStatusCode=1, FinalUpldStatus=No, EntryExit=Created
    │  non-ARC → MasterStatusCode=3, FinalUpldStatus=Yes, EntryExit=Created
    │
    ├── ALWAYS shows btn_inpunch (btn_upload Visible=false)
    │       ↓  btn_inpunch_Click()
    │   job_inpunch.aspx
    │       │  lists ALL Active creator jobs last 3 days (no permit predicate)
    │       │  SP_InsertInto_AttendanceTable
    │       │  JOB_Status=In-Punch Done, MasterStatusCode=3, EntryExit=Entry
    │       ├── job_permitupload.aspx     (btn_upload_Click)
    │       ├── csm_toolboxtalk.aspx      (btn_tbtpage_Click)
    │       ├── csm_soptraining.aspx      (PostBackUrl)
    │       └── homepage.aspx
    │
    └── (handler exists, button hidden) job_permitupload.aspx

job_permitupload.aspx
    │  inbox: Active + last 3 days + EntryExit='Entry'   ← AFTER in-punch
    │  first upload → JOB_Status=Permit Uploaded, FinalUpldStatus=Yes, MasterStatusCode=3
    └── btn_inpunch_Click()  Text="OUT Punch"
            ↓
        job_outpunch.aspx
            │  inbox: Active + EntryExit=Entry + FinalUpldStatus=Yes
            │  SP_Update_AttendancePunchOUT
            │  last pending Entry → AUTO-CLOSE:
            │     JOB_Status=Out-Punch Done, MasterStatusCode=4, EntryExit=Exit
            │     JOBID_Status=Blocked if CheckforPendingPermit==0 else Active
            └── (no redirect)  job becomes visible to approver list

manage_jobid.aspx
    └── view_jobdetails.aspx?JOBID=&dbid=&supv=
            ├── attach_manpower.aspx?JOBID=     (whitelist A84/K208/N21/J8)
            └── back via UrlReferrer

view_jobsforapproval.aspx  (JOB_InchargeWrk, JOB_Status='Out-Punch Done', EntryExit='Exit')
    └── jobapprovalpage.aspx?JOBID=
            └── MasterStatusCode=5, JOB_Status='Approved by Approver', JOBID_Status=Blocked
```

## V2 execution graph (actual redirects and filters)

```
login.aspx
    ↓  (pages vary: hub still 6 session keys; create/permit/in/out only USERID+WORKMAN)
jobs_and_manpower_v2.aspx
    ↓
create_jobid_v2.aspx
    │  SP_InsertInto_JOBSTable + UPDATE Required_Documents/GPS/App_Version='V2'
    │  routing driven by tlb_WO_Rule_Matrix.Default_MasterStatusCode
    │
    ├── WO_MasterStatusCode == "3"
    │       ↓  EncodeJobID() Base64
    │   job_inpunch_v2.aspx?jobid={masked}
    │
    └── else (typically "1")
            ↓
        job_permitupload_v2.aspx?jobid={masked}
            │  inbox: Active + last 3 days + MasterStatusCode='1'  ← BEFORE in-punch
            │  FileCount>0 → MasterStatusCode=3, FinalUpldStatus=Yes
            │  DOES NOT set JOB_Status='Permit Uploaded'
            └── btn_inpunch_Click()
                    ↓
                job_inpunch_v2.aspx?jobid={masked}
                    │  inbox: Active + last 3 days + creator (legacy parity; code '1' jobs included)
                    │  SP_InsertInto_AttendanceTable (batch from ViewState)
                    │  healing UPDATE: JOB_Status=In-Punch Done, EntryExit=Entry
                    └── (no auto-route to outpunch)

job_outpunch_v2.aspx  [?jobid=masked]
    │  inbox: Active + MasterStatusCode='3' + EntryExit='Entry'
    │  also requires FinalUpldStatus='Yes' on selection
    │  individual SP_Update_AttendancePunchOUT
    │  last OUT → CheckPendingOUT==0 → confirmation modal (UAT-029)
    └── Close & Send → UpdateJOBTable1 (legacy writes)
            │  JOB_Status=Out-Punch Done, MasterStatusCode=4, EntryExit=Exit
            │  WhatsApp/email to Site In-Charge (fire-and-forget)
            └── success modal → job_360_view.aspx?jobid={RAW} | jobs_and_manpower_v2.aspx

manage_jobid_v2.aspx
    └── view_jobdetails_v2.aspx?JOBID=&dbid=&supv=
            ├── attach_manpower.aspx?JOBID=     (same whitelist)
            ├── TriggerShareNotifications()     (WhatsApp/email)
            └── manage_jobid_v2.aspx            (Session Grid_Year/Grid_Month)

job_360_view.aspx  (admin override; not a primary user page)
    ├── job_permitupload_v2.aspx?jobid={RAW}   ← DecodeJobID will throw
    ├── job_inpunch_v2.aspx?jobid={RAW}        ← DecodeJobID will throw
    └── job_outpunch_v2.aspx?jobid={RAW}       ← DecodeJobID will throw
```

## Alternate / failure / retry branches

| Branch | Legacy | V2 |
|---|---|---|
| V1↔V2 switch | Working link `jobs_and_manpower_v2.aspx`; per-page V2 links | Create/IN-punch log `tbl_Version_Switch_Log` then redirect to V1; permit/outpunch/manage are plain anchors; hub “Switch to OLD” is `href="#"` (dead) |
| Cancel create | `homepage.aspx` | `homepage.aspx` |
| Reset create | reload `create_jobid.aspx` | clear controls in place |
| Duplicate worker scan | reject, stay on page | reject + workflow log |
| Pending OUT on another job | block IN-punch | block IN-punch |
| Expired gatepass | disable submit; modal override | hide “Add to Roster”; “Renew Pass” |
| Last permit deleted | reset MasterStatusCode=1, JOB_Status=Created | MasterStatusCode=1, FinalUpldStatus=No; JOB_Status unchanged |
| Delete JOB | hard DELETE job/attendance/permit (hidden UI) | transactional soft-delete `DeleteStatus=1` |
| Rejected job | Re-Send sets status 4 / Pending | same, plus workflow log |
| CSM incomplete | OUT-punch blocked | OUT-punch blocked |
| Punch timeout | block if Now−IN exceeds config | same |

## Session variables (ecosystem)

| Key | Read by | Written by JOBID pages |
|---|---|---|
| `USERID` | all 14 pages (auth gate) | none |
| `USERNAME` | most pages (submitter/audit) | none |
| `WORKMAN` | all pages (creator scope) | none |
| `REGION` | legacy all; V2 create/outpunch OT | none |
| `RolePermissionDB` / `UserRoleDB` | legacy all + both hubs + legacy manage/view | none (null-check only) |
| `COMPANY_CODE`, `U_SITE`, `U_SITECODE` | create (both) | none |
| `USERTYPE` | create work-order filter; V2 also locks region for Site Staff | none |
| `Grid_Year` / `Grid_Month` | manage_jobid_v2 restore | manage_jobid_v2 `GridBinder()` |
| `RefUrl` (ViewState, not Session) | view_jobdetails back | Page_Load UrlReferrer |

## QueryString parameters

| Param | Pages | Notes |
|---|---|---|
| none | legacy create/permit/in/out; both hubs | JOB selected from dropdown |
| `jobid` (Base64) | V2 create→permit/in; permit→in; in/out Page_Load | `EncodeJobID` / `DecodeJobID` |
| `jobid` (raw) | `job_360_view` action buttons | incompatible with V2 decode |
| `JOBID`, `dbid`, `supv` | manage→view (both) | `supv` is trusted as creator, not compared to Session |
| `JOBID` only | attach_manpower; several memo/approval viewers | no ownership check on attach_manpower |
| `vw` | `vw_sopapproval` → legacy view | adjacent |

## SQL objects touched by the 14 pages

**Tables:** `tbl_jobs`, `tbl_jobspermit`, `tbl_attendance`, `tbl_Employee_Mustertable`, `tlb_work_state_region`, `tlb_WorkRegion_BillingMapping`, `tlb_JOB_BillingType`, `tlb_attendancecodes`, `tlb_WO_Data`, `tlb_atsworksites`, `tlb_atsworksiteIncharges`, `tlb_workregion_compdept_loc`, `tlb_WO_SkillCategory`, `tlb_WO_Rule_Matrix` (V2), `tlb_Company_Calendar` (V2), `tlb_Backdate_Exceptions` (V2), `tlb_DocumentMaster` / `tlb_Region_Documents` (V2), `tlb_System_Logs` (V2), `tbl_Version_Switch_Log` (V2), `NotificationTemplates` / `NotificationQueue` (V2 IN-punch), `tbl_toolboxtalkdata` / `tbl_soptraining` (OUT-punch CSM counts).

**Stored procedures:** `SP_InsertInto_JOBSTable`, `SP_InsertInto_AttendanceTable`, `SP_Update_AttendancePunchOUT`. Adjacent: `SP_Insert_EmployeeAttendance` (`attach_manpower.aspx.cs`).

**Helpers:** `CountChecker`, `DB_Utility_OH4Y`, `JobWorkflowLogger` (V2), `NotificationTriggerHelper` (V2).

---

# PHASE 2 — Legacy Business Process (seven pages)

## jobs_and_manpower.aspx

### Purpose
Launcher and 3-day/current-month KPI dashboard for the creator’s JOBIDs.

### Entry Conditions
- Login: `Page_Load` requires `USERID`, `RolePermissionDB`, `UserRoleDB`, `USERNAME`, `WORKMAN`, `REGION` else `~/login.aspx`. Method: `jobs_and_manpower.aspx.cs` `Page_Load()`.
- Role: presence only; `webmaster.Master` `LoadPermissions()` / `ApplyPermissions()` controls menu visibility via `tlb_EmployeePermissions`.
- Session: listed above. QueryString: none.

### Inputs
Mandatory / Optional / Hidden: none. Anchors only.

### Validation Rules
None.

### Business Rules
`LoadDashboardStats()` counts `tbl_jobs` where `Creator_Workman=@Workman`:
- Active: `JOBID_Status='Active'` last 3 days
- Pending permits: `MasterStatusCode='1'` + Active + 3 days
- Pending IN: `MasterStatusCode='3'` + `EntryExit='Created'` + Active + 3 days
- Pending OUT: `MasterStatusCode='3'` + `EntryExit='Entry'` + Active + 3 days
- Supply / Line-item: `BillingCode` `MS`/`LI` current calendar month (no Active/3-day filter)

### Database Operations
SELECT `tbl_jobs` (parameterized). No INSERT/UPDATE/DELETE/SP.

### Session Variables
Read: six auth keys; uses `WORKMAN`. Written/Cleared: none.

### Navigation
Incoming: menu (historically), V1 bookmark, V2 unused. Outgoing: create/permit/in/out + memo pages; `jobs_and_manpower_v2.aspx`.

### Error Handling
No try/catch. `DisconnectDb()` not in finally.

### Edge Cases
KPI for pending permits uses `MasterStatusCode='1'`, but the permit *page* lists `EntryExit='Entry'`. Dashboard and inbox can disagree.

### Side Effects
None.

---

## create_jobid.aspx

### Purpose
Create a JOBID row via `SP_InsertInto_JOBSTable`.

### Entry Conditions
- `Page_Load` null-check of six session keys → login.
- **Defect:** `Session["REGION"]` is dereferenced *before* the guard (`create_jobid.aspx.cs` `Page_Load()` lines 58–66 vs 72–80).
- No QueryString.

### Inputs
**Mandatory (ValidationGroup Submit):** Region, Work Order, Billing Type, Attendance Code, Worksite, Approver, Location, Permit No, Shift (1 letter), Title (alphanumeric+comma+space).  
**Optional:** date toggle Today/Yesterday (`btn_dateswap_Click`).  
**Hidden/derived:** `txt_workregion`, `txt_company` (visible=false), `txt_dept` ReadOnly, `lbl_wotype`, site codes, job date/day.

### Validation Rules
- Permit regex: `(?i:na|n/a)` or comma-separated digits (`create_jobid.aspx` markup).
- Shift: `^[a-zA-Z]+$` MaxLength 1.
- Title: `^[a-zA-Z0-9 ,]+$`.
- **Server gap:** `btn_submit_Click()` calls `Insert_JOBData()` with no `Page.IsValid`.

### Business Rules
- JOBID: `JOB` + `DateTime.Now` `yyMMdd` + random 000–999; loop until `tbl_jobs.JOBID` count=0. Method: `Find_DBCode()`. Process lock: `jobInsertLock`. No DB transaction around check+insert.
- Site Staff work orders: `WO_Type='ARC'` only. Office Staff / other: all Active `JOBID_Menu='Yes'`. Method: `Workorder_Binder()`.
- ARC → permit-required flags (`MasterStatusCode=1`, `FinalUpldStatus=No`). Non-ARC → skip permit (`MasterStatusCode=3`, `FinalUpldStatus=Yes`). Method: `Insert_JOBData()`.
- CSM_Documents=Yes only for ARC in AGL/KPO/NINL/JSR; other ARC = No. Same method.
- Same-region + same-company + non-ARC: `AutoBinder()`, permit forced `N/A` ReadOnly. Method: `DataChecker()`.
- Backdate: today or yesterday only. Sunday default attendance `OD` else `P`. JOBID suffix uses *submit clock date*, `@CreatedDate` uses label (possibly yesterday).
- After success, UI always shows IN-Punch; Permit Upload button stays `Visible="false"` (`create_jobid.aspx` lines 232–233). No code sets visibility.

### Database Operations
SELECT: region/billing/attendance/WO/worksite/incharge/location/`tbl_jobs` uniqueness.  
INSERT: `SP_InsertInto_JOBSTable` only. No direct UPDATE/DELETE.

### Session Variables
Read: USERID, RolePermissionDB, UserRoleDB, USERNAME, WORKMAN, REGION, COMPANY_CODE, U_SITE, U_SITECODE, USERTYPE. Written/Cleared: none.

### Navigation
Incoming: hub. Outgoing: `job_inpunch.aspx` (`btn_inpunch_Click`), hidden `job_permitupload.aspx` (`btn_upload_Click`), reset self, cancel `homepage.aspx`.

### Error Handling
WO/worksite exceptions → `ShowPopup` injecting raw `ex.Message`. SP errors → `lbl_msg`. `Find_DBCode` outside insert try/catch.

### Edge Cases
Mutable `static` fields for WO/region/worksite (`create_jobid.aspx.cs` class fields) — cross-request contamination under concurrent users. Unbounded uniqueness loop if 1000 daily suffixes exhausted. No business duplicate of same WO/date/site/shift.

### Side Effects
New `tbl_jobs` row: `JOBID_Status=Active`, `Incharge_Approval=Pending`, `EntryExit=Created`. No notification, no workflow log.

---

## job_permitupload.aspx

### Purpose
Attach PDF/photo permits and mark upload complete.

### Entry Conditions
Six-key session gate. Inbox via `CountChecker.Find_PendingPermitUpload(WORKMAN)` then SQL: last 3 days, creator, Active, **`EntryExit='Entry'`**. Method: `ActiveJOB_Checker()`. No QueryString.

### Inputs
**Mandatory (runtime):** file once `ImportPermit` runs.  
**Optional:** JOB dropdown (no RFV), upload type PDF/Photograph.  
**Hidden:** creator/region/company/site/incharge labels; grid `Id`/`JOBID`/`Name`.

### Validation Rules
PDF `.pdf`; photo `.jpg/.jpeg/.png`. No size/MIME/duplicate-name check. Photo resized 2000×1200, saved JPEG.

### Business Rules
- Status display: `FinalUpldStatus==Yes` → Uploaded; else PDF needs FileCount≥1, photo ≥2. Method: `Bind_JOBIDDetails()`.
- First successful upload still sets `FinalUpldStatus=Yes` and `MasterStatusCode=3` regardless of type. Method: `UpdatePermiStatus()`.
- Last remaining file deleted → reset job to Created / FinalUpldStatus=No / MasterStatusCode=1. Method: `GridView1_RowDeleting()` + `UpdatePermitZeroCount()`.
- Next-step button text is **“OUT Punch”** and redirects to `job_outpunch.aspx`. Method: `btn_inpunch_Click()`.

### Database Operations
SELECT `tbl_jobs`, `tbl_jobspermit`.  
INSERT `tbl_jobspermit`.  
UPDATE `tbl_jobs` status/count.  
DELETE `tbl_jobspermit` by concatenated Id+JOBID.

**Bug:** `InsertIntoDB(byte[] b)` allocates `Byte[] bytes = { 0 }` and writes that to `@Data`, not `b`. Method: `InsertIntoDB()`. Physical file is saved; DB blob is `0x00`.

### Session Variables
Read: six keys; USERNAME/WORKMAN as submitter and delete auditor. Written: none.

### Navigation
Incoming: hub, IN-punch `btn_upload_Click`. Outgoing: `job_outpunch.aspx`, V2 link.

### Error Handling
Popups via `ShowPopup`. Download catch. No transaction around file + DB.

### Edge Cases
Upload path `Server.MapPath(@"\erp_images\Permits\")`; delete checks `C:\atswork.in\wwwroot\erp_images\Permits`. FileCount=0 delete still removes DB row without status/file cleanup. PNG saved as JPEG with `.png` extension.

### Side Effects
Disk file; permit row; job FileCount/FinalUpldStatus/PermitUpload/MasterStatusCode/JOB_Status. No email.

---

## job_inpunch.aspx

### Purpose
Stage workers in ViewState, then insert Entry attendance.

### Entry Conditions
Six-key session. Inbox: `Find_ActiveJOBCountforJOBINPunch` — Active, last 3 days, creator; **no permit / MasterStatusCode / EntryExit filter**. Method: `ActiveJOB_Checker()`. No QueryString.

### Inputs
**Mandatory (ADDTOLIST):** JOB, workman, IN date, IN time.  
**Optional:** gatepass override (group GPDATA).  
**Hidden:** hours, designation/PO skill, gate/safety/PV, job metadata.

### Validation Rules
Scan order in `txt_empworkman_TextChanged()`:
1. Not already in ViewState (`CheckSDuplicateEntry`)
2. `CheckEmployeeActiveStatus` (muster exists and `WorkStatus='Active'`)
3. `Check_EmployeePunchOUT==0` else block (any other job, `AttendanceStatus='Entry'`, `Outpunch_Time IS NULL`, DeleteStatus 0/null)

Gatepass expiry `<0` disables submit; Safety/PV loaded but unused. `FindInpunchEligibilty` computed then ignored. No `Page.IsValid` on save.

### Business Rules
- PO skill from `tlb_WO_SkillCategory` by job region/company/WO; miss falls back to employee category and queues email row. Methods: `PO_SkillPull()`, `_EmployeeDataBinder()`.
- Save: per-row `SP_InsertInto_AttendanceTable` with `AttendanceStatus='Entry'`, `AttendanceCode='Ab'`, `SiteIncharge_Approval='Pending'`. Method: `InsertIntoAttendanceTable()`.
- Then `UpdateJOBTableStatus()`: `JOB_Status='In-Punch Done'`, `MasterStatusCode='3'`, `EntryExit='Entry'`.
- `UPDT_ManpowerCountJOBID` after existing Entry count and after draft save.
- Existing grid: hard DELETE by concatenated Id.
- NINL region changes OT helper and TBT navigation.

### Database Operations
SELECT jobs, attendance, WO skill, employee muster.  
SP insert attendance.  
UPDATE jobs status and ManpowerCount; UPDATE employee gatepass (`GP_UpdateApproval='Pending'`).  
DELETE attendance by Id.

### Session Variables
Read: WORKMAN, USERNAME, REGION, six-key gate. Written: none. ViewState: `Attendance`, `MailDataTable`. Static: `JOBID`.

### Navigation
`job_permitupload.aspx`, self, `csm_toolboxtalk.aspx`, `homepage.aspx`, SOP PostBackUrl, V2 link. `btn_createjobid_Click` exists without markup control.

### Error Handling
Popups for eligibility, SP, status, GP. Mapping email only if `maildataTable.Rows.Count > 1` and region in JSR/KPO/NINL/AGL. Method: `btn_finalsubmit_Click()`.

### Edge Cases
No transaction around multi-row SP calls. Existing-row delete does not recount ManpowerCount. `DataTablePull` overwrites `flag` per row.

### Side Effects
Entry punches; job EntryExit=Entry; optional HR email `hr@atswork.in` via `DB_Utility_OH4Y.SendEmail` gated by `NotificationTriggerHelper.ModuleJobInpunch`.

---

## job_outpunch.aspx

### Purpose
Individual OUT-punch and automatic shift close.

### Entry Conditions
Six-key session. Count: `Find_ActiveJOBCountforOUTPunch` (Active, 3 days, creator region+workman, `FinalUpldStatus='Yes'`). Dropdown adds `EntryExit='Entry'`. Method: `ActiveJOB_Checker()`. No QueryString.

### Inputs
**Mandatory (PunchOUT_Button):** lunch, attendance code, OT. Date/time have client JS, no RFV.  
**Hidden:** row Id, workman, scheduled hours.

### Validation Rules
Client OT 0–16; client blocks future datetime. Server: `CanPunchOutWithin32Hours(intime)` compares **DateTime.Now − IN**, not entered OUT. Config `PunchOutDurationMinutes`, fallback 1920 (32h). Message says “24 Hour”. No `Page.IsValid`.

### Business Rules
- If `CSM_Documents='Yes'`, require `TBT_Count>0` and `SOP_Count>0`. Method: `Bind_JOBIDDetails()`.
- After each successful punch or delete, `CheckPendingOUT()`: if no Entry rows remain, **auto-update** job to Out-Punch Done / code 4 / Exit; Blocked iff `CheckforPendingPermit==0`.
- `SP_Update_AttendancePunchOUT` with Exit statuses, lunch, worked mins/hours, calc+provided OT, attendance code. Method: `UpdateAttendanceTable()`.
- Hard delete attendance by concatenated Id, then re-evaluate close.

### Database Operations
SELECT jobs, attendance, TBT/SOP counts, attendance codes.  
SP update punch.  
UPDATE `tbl_jobs` close.  
DELETE attendance.

### Session Variables
Read: WORKMAN, REGION, USERNAME (gate only). Written: none.

### Navigation
V2 link only. No success redirect to approval.

### Error Handling
Popups; SP catch sets `lbl_msg` and returns false. `PunchOUT_Click` rethrows after `lbl_msg`.

### Edge Cases
`CommandArgument='<% #Eval("Id") %>'` has a space after `<%` (markup). Row fetch by Id only (no JOB/creator predicate). Grid SQL concatenates WORKMAN+JOBID.

### Side Effects
Attendance Exit; possible auto Blocked+code 4. No email.

---

## manage_jobid.aspx

### Purpose
Month-scoped creator JOB list: filter, status swap, navigate to details, latent hard-delete.

### Entry Conditions
Six-key session on first load. No QueryString. List: `Creator_Workman` AND `Creator_Name` + year/month.

### Inputs
Month prev/current/next; status filter 0–8; billing dropdown; grid commands View_Details, Swap_JOBIDStatus, hidden Delete.

### Validation Rules
None. `Convert.ToInt32(e.CommandArgument)` before command-name check.

### Business Rules
- Approved rows disable status button (`GridView1_RowDataBound`).
- Blocked → Active with **no** pending checks. Any other label → Blocked only if `CheckforPendingOUT==0` AND `CheckforPendingPermit==0`. Method: `GridView1_RowCommand()`.
- Swap re-reads DB: Active→Blocked else→Active. Method: `JOBID_Status_Swaper()`.
- Filter “Blocked” is `JOBID_Status!='Active'`. Permit pending is `FinalUpldStatus!='Yes'`.
- Delete (hidden): sequential hard DELETE `tbl_jobs` by Id+JOBID, `tbl_jobspermit` by JOBID, `tbl_attendance` by JOBID. No transaction. Method: `JOBID_Delete()`.

### Database Operations
SELECT jobs, billing types. UPDATE JOBID_Status. DELETE job/permit/attendance (concatenated SQL).

### Session Variables
Read: six keys, WORKMAN, USERNAME. Written: none.

### Navigation
`view_jobdetails.aspx?JOBID={jobid}&dbid={dbid}&supv={supv}`.

### Error Handling
Delete helpers swallow exceptions then still show success. Most list methods unguarded.

### Edge Cases
Grid labels trusted for identity (ViewState postback). Status/delete SQL has no session-user predicate. Month padding inconsistent (`"0"+month` vs unpadded initial).

### Side Effects
Status flip; irreversible hard delete if hidden command invoked.

---

## view_jobdetails.aspx

### Purpose
Display one JOB; edit title/permit/shift; latent permit/attendance delete; resend rejected jobs; WhatsApp share; privileged attach manpower.

### Entry Conditions
QueryString `JOBID`, `dbid`, `supv` on every request. Six-key session. Lookup: `JOBID=@JOBID AND Id=@Id AND Creator_Workman=@Creator_Workman` where creator = **query `supv`**, not Session WORKMAN. Method: `Bind_JOBIDDetails()`. `ViewState["RefUrl"]=Request.UrlReferrer.ToString()` — NRE if no referrer.

### Inputs
Query triple; title/shift/permit; attendance IN/OUT/lunch/OT/code; buttons Update/Cancel/Back/Resend/Attach.

### Validation Rules
Permit regex as create. Attendance parse `yyyy-MM-dd hh:mm:ss tt`. OT client ≤16; server `Convert.ToInt32` no range. No `Page.IsValid`.

### Business Rules
- Approved: disable basic update. Pending: enable only if `EntryExit != "Entry"`. Rejected: show Re-Send. Method: `Bind_JOBIDDetails()` visibility block.
- Permit delete column is markup col 6 hidden; code toggles col 5 (Time). Attendance action col 14 hidden; code toggles col 13 (Code). **Delete UIs never become visible.**
- Basic update: `JOB_PermitNo/Title/Shift` WHERE JOBID only (all rows with that JOBID). Method: `UpdateBasicJOBData()`.
- Resend: `UpdateJOBTable1("Blocked","Out-Punch Done","4","Exit")` plus `Incharge_Approval='Pending'` WHERE JOBID. Method: `btn_resend` path ~1035. No re-check that status is still Rejected.
- Attach button only for workman A84, K208, N21, J8. Method: `Checker()`. Redirect `attach_manpower.aspx?JOBID=` does not re-check whitelist.

### Database Operations
SELECT jobs/permits/attendance. UPDATE jobs (edit, resend, permit counts). UPDATE attendance. DELETE permit/attendance. SELECT permit by Id for download.

### Session Variables
Read: six keys, WORKMAN (whitelist), USERNAME/WORKMAN (audit). Written: none.

### Navigation
Back to UrlReferrer; attach_manpower; WhatsApp via `HF_Msg`; file download.

### Error Handling
`Error-251` on bind failure. Popups on mutations. File IOException swallowed then DB delete continues.

### Edge Cases
`SELECT TOP 10` no ORDER BY. Query `supv` is attacker-selectable. Updates by JOBID not Id. WhatsApp payload from server labels.

### Side Effects
Job field edits; approval reset; WhatsApp-ready message. Delete handlers exist but UI paths are inert.

---

# PHASE 3 — Complete Legacy JOBID Lifecycle

Documented user-control order (`jobstatus_flow.ascx`, registered on the legacy hub but not instantiated):

```
1 JOBID Created
  ↓
2 In-Punch Done
  ↓
3 Permit Uploaded
  ↓
4 Out-Punch Done
  ↓
5 Approved by Approver
```

This matches **runtime** filters and buttons, not the dashboard comment “permit then entry”.

## End-to-end happy path

### A. Non-ARC work order (permit skipped)

```
Create
  JOBID_Status=Active
  JOB_Status=Permit Uploaded
  FinalUpldStatus=Yes, PermitUpload=N/A
  MasterStatusCode=3, EntryExit=Created, CSM_Documents=No
  ↓ UI: In-Punch Page
IN-Punch workers
  AttendanceStatus=Entry, AttendanceCode=Ab, SiteIncharge_Approval=Pending
  JOB_Status=In-Punch Done, EntryExit=Entry, MasterStatusCode=3
  ↓ user navigates hub/outpunch (no auto-redirect)
OUT-Punch each worker
  SP_Update_AttendancePunchOUT → AttendanceStatus=Exit
  ↓ last Entry gone → AUTO
  JOB_Status=Out-Punch Done, MasterStatusCode=4, EntryExit=Exit
  JOBID_Status=Blocked (FinalUpldStatus already Yes ⇒ CheckforPendingPermit=0)
  Incharge_Approval still Pending (set at create)
  ↓ appears on view_jobsforapproval
Approver
  JOB_Status=Approved by Approver, MasterStatusCode=5, JOBID_Status=Blocked
  attendance SiteIncharge_Approval=Approved, AttendanceStatus=Present
```

### B. ARC work order (permit required) — actual order

```
Create
  JOB_Status=Created, FinalUpldStatus=No, PermitUpload=No
  MasterStatusCode=1, EntryExit=Created
  CSM_Documents=Yes for AGL/KPO/NINL/JSR else No
  ↓ UI STILL In-Punch (permit button hidden)
IN-Punch (inbox does not require permit)
  EntryExit=Entry, MasterStatusCode forced to 3, JOB_Status=In-Punch Done
  ↓
Permit Upload (inbox requires EntryExit=Entry)
  JOB_Status=Permit Uploaded, FinalUpldStatus=Yes, MasterStatusCode=3
  ↓ button “OUT Punch”
OUT-Punch (requires FinalUpldStatus=Yes AND EntryExit=Entry)
  if CSM_Documents=Yes, TBT and SOP counts must be >0 else block
  ↓ last Entry → AUTO close as above
Approval
```

## Decision / failure / retry

```
Create validation fail → stay; no row
Create SP fail → lbl_msg; no routing

IN scan: duplicate in roster → reject
IN scan: inactive/missing employee → reject
IN scan: pending OUT elsewhere → reject, show other JOBID/date/submitter
IN gatepass expired → cannot add until override saved (GP_UpdateApproval=Pending)
IN skill unmapped → still allow; email ATS HR if >1 queued rows and region in {JSR,KPO,NINL,AGL}

Permit no file / bad extension → popup
Permit last file deleted → roll MasterStatusCode back to 1, JOB_Status=Created
  OUT-punch inbox then excludes job until re-upload (FinalUpldStatus=Yes required)

OUT timeout (Now−IN > PunchOutDurationMinutes) → block
OUT CSM incomplete → block entire grid
OUT delete last Entry row → same auto-close as punch

Block job from manage:
  if pending Entry → refuse
  if FinalUpldStatus=No → refuse
  else Active↔Blocked
Unblock: no pending checks

Delete job (hidden): irreversible hard delete of job + all attendance + all permits

Rejected:
  view_jobdetails Re-Send → Blocked + Out-Punch Done + code 4 + Exit + Incharge_Approval=Pending
  no attendance rewrite

Reopen: there is no dedicated reopen. Unblock only flips JOBID_Status to Active.
Cancellation: Blocked status, not a Cancelled state.

3-day window: create/permit/in/out inboxes ignore older Active jobs.
```

## Duplicate prevention (legacy)

| Layer | What is prevented | What is not |
|---|---|---|
| JOBID string | random+count loop + process lock | same WO/date/site/shift second job |
| IN ViewState | same worker in current roster | concurrent two-browser insert |
| IN global | worker with open Entry anywhere | after OUT, same day re-entry on another job allowed |
| Attendance SP | unknown (body absent) | — |

---

# PHASE 4 — V2 Implementation (seven pages)

Do not assume parity. Below is actual V2 behavior.

## jobs_and_manpower_v2.aspx

### Purpose
Same KPI hub as legacy, plus Manage tile. Title: NEW Version.

### Entry Conditions
Identical six-key gate. Method: `Page_Load()`.

### Inputs
None. Dead control: “Switch to OLD Version” `href="#"` (`jobs_and_manpower_v2.aspx` line 133).

### Validation / Business Rules / Database
**Identical SQL** to legacy `LoadDashboardStats()`. Parameterized SELECT `tbl_jobs`.

### Session / Navigation
Read same six keys. Outgoing: `*_v2.aspx` create/permit/in/out, memos, **`manage_jobid_v2.aspx`**.

### Error Handling / Edge Cases / Side Effects
Same as legacy. Extra: manage badge `Span1` is never assigned (stays 0).

---

## create_jobid_v2.aspx

### Purpose
Matrix-driven create, GPS, document checklist, then **auto-redirect** to permit or IN-punch with Base64 JOBID.

### Entry Conditions
Only `USERID` and `WORKMAN`. Later uses REGION, USERTYPE, USERNAME, COMPANY_CODE, U_SITE, U_SITECODE unguarded. Method: `Page_Load()`, `Insert_JOBData()`.

### Inputs
**Mandatory (server `Page.Validate("Submit")` plus raw checks):** date, region, WO, site, approver, location, shift, title (>3 words). Conditionally: billing type, permit no, attendance, GPS lat/lon.  
**Optional:** document checkboxes, V1-switch reason/remarks.  
**Hidden:** `hf_latitude`, `hf_longitude`, `hf_gps_required`.

### Validation Rules
- `btn_submit_Click()`: `Page.Validate("Submit")` then anti-bypass emptiness checks, matrix conditionals, GPS if `WO_ReqGPS`, title word count > 3.
- Date picker min/max from `GetUserBackdateLimit()` default 2 days, override `tlb_Backdate_Exceptions.Max_Backdate_Days` where `IsActive=1`. `txt_jobdate_TextChanged()` resets out-of-range to today. **Submit does not re-check the range.**
- Permit keypress `[a-zA-Z0-9,\/]`. Title client word count. No legacy permit regex of NA-or-digits-only.

### Business Rules
- Work order JOIN `tlb_WO_Rule_Matrix` on `Billing_Nature` + `Execution_Type`. Missing matrix: defaults permit/CSM/attendance required, auto-title off, status `'1'`; UI error + `LogSystemIssue`. Method: `DDL_Workorder_SelectedIndexChanged()`.
- Matrix flags: `Req_PermitNo`, `Req_CSM_Docs`, `Req_Attendance`, `Auto_Generate_Title`, `Default_MasterStatusCode`. Non-Billing hides billing dropdown.
- Calendar: `tlb_Company_Calendar` restricts attendance codes (`EvaluateAttendanceCode()`).
- Site Staff: region dropdown disabled; WO list ARC-only. Method: `Page_Load()`, `Workorder_Binder()`.
- GPS: `tlb_work_state_region.Req_GPS_Tagging='Yes'` → `WO_ReqGPS`. Method: `WorkRegion_ControllerCheck()`.
- Duplicate same-date jobs: warning only (`CheckExistingJobsForDate()`), not a submit block.
- JOBID algorithm: same `JOByyMMddNNN` as legacy. Method: `Find_DBCode()`.
- After SP insert, second UPDATE sets `Required_Documents`, `GPS_Latitude/Longitude`, `App_Version='V2'`. Method: `Insert_JOBData()`.
- Route: `WO_MasterStatusCode=="3"` → `job_inpunch_v2.aspx?jobid={EncodeJobID}`; else permit V2. Method: `btn_submit_Click()`.
- Encode: URL-safe Base64, not a signature. Method: `EncodeJobID()`.
- **Defect:** `WO_BillingNature` and `WO_ContractNature` ViewState are never assigned. Locals `billingNature`/`contractNature` used for UI only. Insert therefore never takes the `Non-Billing`/`NB` ViewState branch (`Insert_JOBData()` lines 726–727). Location ARC branch (`WO_ContractNature=="ARC"`) never runs (`DDL_Worksite_SelectedIndexChanged()` line 986).
- **Defect:** success redirect block is duplicated (lines 628–685); both `Response.Redirect(..., false)` without `return`.
- Mandatory documents are UI-locked (`onclick="return false"`) but `GetSelectedDocuments()` does not verify mandatory IDs at submit.

### Database Operations
SELECT matrix/calendar/docs/exceptions/regions/WO/sites.  
SP `SP_InsertInto_JOBSTable` (33 parameters).  
UPDATE `tbl_jobs` V2 columns.  
INSERT `tlb_System_Logs`, `tbl_Version_Switch_Log`.

### Session Variables
Read: USERID, WORKMAN, REGION, USERTYPE, USERNAME, COMPANY_CODE, U_SITE, U_SITECODE. Written: none.

### Navigation
Success: masked permit or IN-punch. Cancel: homepage. V1: `create_jobid.aspx` after optional log.

### Error Handling
PNotify. Insert catch returns null. Backdate/duplicate-warning/system-log/version-log swallow errors.

### Edge Cases
Region change rebinds WO but not billing types (first bind uses Session REGION). GPS is non-empty string only. Unbounded JOBID loop. Static `jobInsertLock` as legacy.

### Side Effects
`JobWorkflowLogger.LogAction(..., "1. CREATE JOB (V2 Smart Workflow)", ...)`. GPS and required-doc columns. `App_Version='V2'`.

---

## job_permitupload_v2.aspx

### Purpose
Step 2: upload permits for `MasterStatusCode='1'` jobs, then route to IN-punch.

### Entry Conditions
`USERID`+`WORKMAN` only. QueryString `jobid` decoded then selected if in inbox; **else inserted into dropdown and bound anyway**. Methods: `Page_Load()`, `DecodeJobID()`, `ActiveJOB_Checker()`, `Bind_JOBIDDetails()`.

Inbox SQL: last 3 days, creator, Active, **`MasterStatusCode='1'`**.

### Inputs
JOB dropdown, upload type, file. Hidden job labels.

### Validation Rules
HasFile; extension PDF or jpg/jpeg/png. No size/signature. Unique disk name `{JOBID}-{ticks}{ext}`.

### Business Rules
- Proceed iff `FileCount>0` (shows `btn_inpunch`). No matrix document completeness. Method: `Bind_JOBIDDetails()`.
- `btn_inpunch_Click()` does not re-check count; logs and redirects masked IN-punch.
- `UpdatePermitStatus`: FileCount ±1; `FinalUpldStatus` Yes if count>0 else No; `MasterStatusCode` 3 or 1; sets `PermitUploadDate`. **Does not update `JOB_Status` or `PermitUpload`.** Method: `UpdatePermitStatus()`.
- Delete: parameterized DELETE permit + decrement + physical file under `~/erp_images/Permits/`.
- Download: SELECT Name BY Id only (no JOB/user scope).

### Database Operations
SELECT jobs/permits. INSERT `tbl_jobspermit` with **actual uploaded bytes** (`@Data`, bytes from stream). UPDATE jobs counts/codes. DELETE permit.

### Session Variables
Read: USERID, WORKMAN, USERNAME (insert). Written: none.

### Navigation
Outgoing: `job_inpunch_v2.aspx?jobid={masked}`. V1: plain `job_permitupload.aspx` (no switch log).

### Error Handling
PNotify. Decode has no try/catch (malformed Base64 throws in Page_Load). No transaction file+DB.

### Edge Cases
QueryString JOB not in inbox still binds (IDOR if JOBID guessed/decoded). `Bind_JOBIDDetails` queries by JOBID only. PNG saved as JPEG.

### Side Effects
Workflow logs add/remove/complete/error. Disk + DB blob.

---

## job_inpunch_v2.aspx

### Purpose
Step 3: stage scans, batch-insert Entry attendance, heal job to Entry.

### Entry Conditions
USERID+WORKMAN. Inbox: Active, 3 days, creator — **no `MasterStatusCode='3'` gate** (legacy parity as of UAT-006 / UAT-021). Method: `ActiveJOB_Checker()`. QueryString decoded JOB selected only if already in dropdown.

### Inputs
JOB, IN date/time, workman scan, GP override, V1-switch reason.

### Validation Rules
Scan (`txt_empworkman_TextChanged()`):
1. Duplicate in stage or existing Entry for this JOB (`CheckDuplicateEntry` — **does not exclude DeleteStatus=1**)
2. Inactive/missing employee
3. Global pending OUT (`Check_EmployeePunchOUT`)

Gatepass: only `GatePassExpiry` days<0 hides Add. Skill mismatch does not block; queues `SKILL_MISMATCH`. Client blocks future IN; **final submit does not re-validate date/active/pending-OUT/gatepass**. No `Page.IsValid`. RFV InitialValue `"--Select--"` vs inserted `"--Select Active Job--"`.

### Business Rules
Stage in ViewState; `btn_finalsubmit_Click()` loops `SP_InsertInto_AttendanceTable` (`Entry`/`Ab`/`Pending`). Then **always** healing UPDATE:

`JOB_Status='In-Punch Done', MasterStatusCode='3', EntryExit='Entry', ManpowerCount=COUNT Entry`

Existing worker remove: hard DELETE by Id + recount. GP override: update muster + `GP_UpdateApproval='Pending'` + employee edit log file. Notifications: `NotificationQueue` for skill mismatch / GP update (`QueueNotification()`).

V1 downgrade: insert `tbl_Version_Switch_Log`, workflow log “UI DOWNGRADE”, redirect `job_inpunch.aspx`.

### Database Operations
SELECT jobs/attendance/employee/templates. SP insert. UPDATE jobs heal; UPDATE muster; INSERT NotificationQueue / Version_Switch_Log. DELETE attendance.

### Session Variables
Read: WORKMAN, USERNAME. Written: none. ViewState Attendance.

### Navigation
No auto-route to OUT. V1 with logging.

### Error Handling
PNotify; workflow error log. No transaction around batch SP. Healing still runs after partial success.

### Edge Cases
Malformed `jobid` throws. Duplicate check vs DeleteStatus=1 can block re-add of a previously deleted punch. Empty batch still heals status.

### Side Effects
Workflow logs (scan reject, batch, GP, error, downgrade). NotificationQueue. No WhatsApp on IN complete.

---

## job_outpunch_v2.aspx

### Purpose
Step 4: individual OUT, optional hard-delete, **last-OUT confirmation then legacy `UpdateJOBTable1()` close**.

### Entry Conditions
USERID+WORKMAN. Inbox: Active, 3 days, creator, `MasterStatusCode='3'`, `EntryExit='Entry'`. Selection also requires `FinalUpldStatus='Yes'`. Methods: `ActiveJOB_Checker()`, `Pull_PermitStatus()`. QueryString Base64, must exist in dropdown.

### Inputs
OUT date (UI min = job date−1 day), time, lunch, code, OT.

### Validation Rules
Client OT 0–16 and no future datetime. Server timeout same Now−IN vs config/fallback 1920. No OUT-after-IN check. `FindEmployeeWorkedTime` can yield negative minutes. No `Page.IsValid`. `Session["REGION"]` used for NINL OT without being in the auth gate.

### Business Rules
CSM_Documents=Yes → TBT and SOP required. Method: `Bind_JOBIDDetails()`.  
Individual `SP_Update_AttendancePunchOUT`. After punch/delete, `CheckPendingOUT()`: if `CheckforPendingOUT==0`, show confirmation modal (UAT-029). **Review Again** dismisses only. **Close & Send** (`btn_FinalizeShift_Click` — control ID unchanged) reuses `UpdateJOBTable1`: Blocked vs Active by `CheckforPendingPermit`; `JOB_Status=Out-Punch Done`, code `4`, `EntryExit=Exit`; fire-and-forget WhatsApp/email to in-charge; success modal (UAT-040 / UAT-035). Idempotent (UAT-040A / UAT-040B): requires still-open code 3 / Entry and zero pending OUT; already-closed jobs skip the UPDATE and reopen the success modal. Client lock disables the button after first click.  
Delete: hard DELETE by Id only; no ManpowerCount update. Last Entry delete also prompts the close modal.

### Database Operations
SELECT jobs/attendance/codes/employee contact. SP punch. UPDATE jobs on Close & Send via `UpdateJOBTable1` (same four columns as legacy; WHERE still-open code 3 / Entry). DELETE attendance.

### Session Variables
Read: WORKMAN, REGION. Written: none.

### Navigation
Success modal links `job_360_view.aspx?jobid={raw}` and hub. V1: plain `job_outpunch.aspx`.

### Error Handling
PNotify. Parse/config/`REGION` before try. Notification failures logged, not shown (`Task.Run`).

### Edge Cases
`job_360_view` raw query vs DecodeJobID. Grid does not filter DeleteStatus or AttendanceStatus. Finalize does not set `Incharge_Approval` (remains Pending from create).

### Side Effects
Workflow logs; `~/Logs/OutPunch/{date}/Notification_Log.txt`; MSG91 WhatsApp + SMTP if `JOB_ALERT` enabled.

---

## manage_jobid_v2.aspx

### Purpose
Month list with parameterized filters, status swap, details, **soft delete**.

### Entry Conditions
USERID, USERNAME, WORKMAN (not RolePermissionDB/REGION). Restores `Session["Grid_Year"]`/`["Grid_Month"]`. Method: `Page_Load()`.

### Inputs
Status/billing filters, client search, month nav, commands including visible `DeleteRec`.

### Validation / Business Rules
SQL always `Creator_Workman=@Workman` and `ISNULL(DeleteStatus,0)=0`. Filters equivalent to legacy. Approved disables toggle in RowDataBound; server swap does not re-check approval. Blocked→Active unchecked; else Blocked only if pending OUT=0 and pending permit=0. Method: `GridView1_RowCommand()`, `JOBID_Status_Swaper()`.

Delete: transaction updates DeleteStatus/ViewStatus/DeletedOn/DeletedBy on `tbl_jobs` (Id+JOBID), `tbl_attendance` (JOBID), `tbl_jobspermit` (JOBID). Method: `JOBID_Delete()`. UI says “Permanently Delete”; implementation is soft.

### Database Operations
Parameterized SELECT/UPDATE. Soft-delete UPDATEs. No DELETE statement.

### Session Variables
Read: USERID, USERNAME, WORKMAN. Written: Grid_Year, Grid_Month every bind. Reset Remove then rewrite current month.

### Navigation
`view_jobdetails_v2.aspx?JOBID=&dbid=&supv=`. V1 plain link.

### Error Handling
try/finally + PNotify. Transaction rollback on delete failure.

### Edge Cases
Child soft-delete by JOBID only. Detail page does not filter DeleteStatus — direct URL can still open a soft-deleted job.

### Side Effects
Workflow logs status swap and soft delete.

---

## view_jobdetails_v2.aspx

### Purpose
V2 details: edit, attendance edit, resend, share notifications, attach manpower, back to V2 manage.

### Entry Conditions
Raw QueryString JOBID/dbid/supv before auth. USERID/USERNAME/WORKMAN. Parent query still uses **query `supv`** as Creator_Workman. No Session equality check. Method: `Page_Load()`, `Bind_JOBIDDetails()`.

### Inputs
Permit/title/shift; attendance edit; Share; Resend; Attach; Back.

### Validation / Business Rules
Edit enablement same as legacy (Pending and not Entry). JOB UPDATE still by JOBID only. Attendance edit by Id+JOBID+EmployeeWrk with OT recompute; no timeout/OUT-after-IN.  
Permit `OnRowDeleting` exists but **no Delete command control**; code toggles column 5 (Time). Attendance Delete link `Enabled="false"` never enabled. Soft-delete handlers exist (`DeleteStatus=1`) but are UI-inert. Last-permit reset still can set JOB_Status=Created / code 1.  
Resend: same Blocked/Out-Punch Done/4/Exit/Pending; logs; **does not send** share notifications.  
Attach: same four workman IDs; logs; redirects legacy `attach_manpower.aspx?JOBID=`.  
Share: `Task.Run(TriggerShareNotifications)` WhatsApp/email; does not fill `HF_Msg` / `openWhatsApp()`.

### Database Operations
SELECT/UPDATE jobs, attendance, permits (soft). Download by permit Id. External MSG91/SMTP.

### Session Variables
Read: USERID, USERNAME, WORKMAN. Written: none (manage sessions already set).

### Navigation
Back: `manage_jobid_v2.aspx` (month restore). Attach: legacy page.

### Error Handling
Error-251; PNotify; notification errors swallowed by `LogSystemEvent`.

### Edge Cases
Same IDOR via `supv`. Soft-deleted parent still bindable. Share vs WhatsApp protocol change.

### Side Effects
Workflow logs; optional approval-alert WhatsApp/email.

---

# PHASE 5 — Functional Diff (side-by-side)

Result labels used only: PRESERVED | CHANGED | ADDED | REMOVED | REGRESSION | SECURITY IMPROVEMENT | UX IMPROVEMENT | PERFORMANCE IMPROVEMENT

## jobs_and_manpower

| Capability | Legacy | V2 | Result |
|---|---|---|---|
| Auth gate (6 session keys) | Yes | Yes | PRESERVED |
| KPI SQL / 3-day window | Identical | Identical | PRESERVED |
| Links to create/permit/in/out | V1 pages | V2 pages | CHANGED |
| Manage JOBID tile | Absent | `manage_jobid_v2.aspx` | ADDED |
| Switch to other version | Works → V2 | `href="#"` dead | REGRESSION |
| jobstatus_flow.ascx | Registered, not rendered | Not registered | PRESERVED |
| Parameterized dashboard SQL | Yes | Yes | PRESERVED |

## create_jobid

| Capability | Legacy | V2 | Result |
|---|---|---|---|
| JOBID `JOByyMMddNNN` | Yes | Yes | PRESERVED |
| `SP_InsertInto_JOBSTable` | Yes | Yes | PRESERVED |
| Permit vs skip routing | Hardcoded ARC vs non-ARC | `tlb_WO_Rule_Matrix.Default_MasterStatusCode` | CHANGED |
| Post-create UX | Stay; only IN-punch button | Auto-redirect masked next step | CHANGED |
| Site Staff ARC-only WO | Yes | Yes | PRESERVED |
| Site Staff region lock | No | Dropdown disabled | SECURITY IMPROVEMENT |
| Backdate | Today/Yesterday toggle | Calendar min/max + exceptions table (default 2 days) | CHANGED |
| Arbitrary backdate beyond yesterday | Impossible | Possible if exception days >1 or submit bypasses changed event | CHANGED |
| Title validation | Regex charset | >3 words | CHANGED |
| Permit NA regex | Yes | Relaxed alphanumeric / comma / slash | CHANGED |
| Server `Page.IsValid` | No | Yes + raw checks | SECURITY IMPROVEMENT |
| GPS tagging | No | Region flag `Req_GPS_Tagging` | ADDED |
| Required documents | No | Checklist from master/region | ADDED |
| Company calendar attendance | Sunday OD else P | `tlb_Company_Calendar` | ADDED |
| Auto title | No | Matrix `Auto_Generate_Title` | ADDED |
| Same-date duplicate warn | No | Warning only | ADDED |
| App_Version / GPS / Required_Documents columns | No | UPDATE after insert | ADDED |
| Workflow text log | No | `JobWorkflowLogger` | ADDED |
| V1 fallback with reason | N/A | Modal + `tbl_Version_Switch_Log` | ADDED |
| Static WO fields | `static` class fields | ViewState (except unused Billing/Contract nature) | CHANGED |
| Office Staff vs other | Distinct Office vs Site vs else | Site vs everyone else | CHANGED |
| Stay-on-page after create | Yes | Removed (always redirect) | REMOVED |
| Hidden Permit Upload button | Present unused | N/A (redirect) | REMOVED |

## job_permitupload

| Capability | Legacy | V2 | Result |
|---|---|---|---|
| Auth | 6 keys | USERID+WORKMAN | CHANGED |
| Inbox predicate | `EntryExit='Entry'` | `MasterStatusCode='1'` | CHANGED |
| QueryString JOB | No | Base64 `jobid`; can bind JOB not in inbox | CHANGED |
| Next step | OUT-punch | IN-punch (masked) | CHANGED |
| Sets JOB_Status=Permit Uploaded | Yes | No | REGRESSION |
| Sets PermitUpload Yes/No | Yes | No | REGRESSION |
| Sets FinalUpldStatus + MasterStatusCode | Yes | Yes | PRESERVED |
| Last-file rollback to code 1 | Yes | Yes (FinalUpldStatus/code only) | CHANGED |
| DB blob `@Data` | Dummy `{0}` | Actual file bytes | SECURITY IMPROVEMENT |
| SQL concat vs params | Concat dropdown/grid/delete | Parameterized delete/insert | SECURITY IMPROVEMENT |
| Unique file name | Original name | `{JOBID}-{ticks}` | UX IMPROVEMENT |
| Path | Mixed MapPath vs hardcoded C:\ | `~/erp_images/Permits/` | SECURITY IMPROVEMENT |
| Proceed without FileCount re-check | Button shown by status | Shown if FileCount>0; click no re-check | PRESERVED |
| V1 switch logging | N/A | None (plain link) | CHANGED |
| Photo types jpg/jpeg/png | Yes | Yes | PRESERVED |

## job_inpunch

| Capability | Legacy | V2 | Result |
|---|---|---|---|
| Inbox | Any Active 3-day creator job | Any Active 3-day creator job (UAT-006 restored; no code-3 gate) | PRESERVED |
| Staging then SP insert | Yes | Yes | PRESERVED |
| AttendanceCode Ab / Approval Pending | Yes | Yes | PRESERVED |
| Pending OUT elsewhere | Yes | Yes | PRESERVED |
| Active employee check | Yes | Yes | PRESERVED |
| Gatepass expiry gate | Yes | Yes | PRESERVED |
| Safety/PV expiry unused | Yes | Yes | PRESERVED |
| Skill mismatch still allows punch | Yes | Yes + NotificationQueue | CHANGED |
| HR email skill mismatch | Conditional SMTP | Queue SKILL_MISMATCH; no same email path | CHANGED |
| Healing/status update | Per successful insert | Always after batch, even 0 inserts | CHANGED |
| Existing punch delete | Hard delete; no recount | Hard delete + ManpowerCount recount | CHANGED |
| Duplicate vs DeleteStatus=1 | N/A (hard delete) | Can block re-add | REGRESSION |
| TBT/SOP buttons | Yes | Not on this page | REMOVED |
| QueryString deep link | No | Masked jobid | ADDED |
| Workflow / reject logs | No | Yes | ADDED |
| Session RolePermissionDB required | Yes | No | CHANGED |
| Create JOBID button handler | Orphan CS | Absent | REMOVED |

## job_outpunch

| Capability | Legacy | V2 | Result |
|---|---|---|---|
| Inbox Entry + FinalUpldStatus Yes | Yes | Yes (+ MasterStatusCode=3) | CHANGED |
| CSM TBT/SOP gate | Yes | Yes | PRESERVED |
| Individual SP_Update_AttendancePunchOUT | Yes | Yes | PRESERVED |
| Auto-close when last Entry gone | Yes `CheckPendingOUT` | Confirmation modal then same `UpdateJOBTable1` (UAT-029 / UAT-040) | PRESERVED |
| Approver WhatsApp/email on close | No | Yes (async) | ADDED |
| Timeout Now−IN vs config | Yes | Yes | PRESERVED |
| Hard delete attendance | Concat Id | Param Id, no job scope | CHANGED |
| QueryString | No | Masked | ADDED |
| V1 switch log | N/A | None | CHANGED |
| Success redirect | None | Popup to 360/hub | UX IMPROVEMENT |
| Attendance code list Approver+Present | Yes | Yes | PRESERVED |

## manage_jobid

| Capability | Legacy | V2 | Result |
|---|---|---|---|
| Creator-scoped month grid | Workman AND Name concat SQL | Workman param + DeleteStatus=0 | SECURITY IMPROVEMENT |
| Status/billing filters | Yes | Yes | PRESERVED |
| Block rules pending OUT/permit | Yes | Yes | PRESERVED |
| Unblock without checks | Yes | Yes | PRESERVED |
| Delete | Hidden hard DELETE | Visible soft-delete + transaction | CHANGED |
| Details URL | view_jobdetails.aspx | view_jobdetails_v2.aspx | CHANGED |
| Month session restore | No | Grid_Year/Grid_Month | UX IMPROVEMENT |
| Client quick search | No | DOM filter | ADDED |
| RolePermissionDB in gate | Yes | No | CHANGED |
| Concatenated SQL | Yes | Parameterized | SECURITY IMPROVEMENT |

## view_jobdetails

| Capability | Legacy | V2 | Result |
|---|---|---|---|
| Query JOBID/dbid/supv | Yes | Yes | PRESERVED |
| Trusts query `supv` as owner | Yes | Yes | PRESERVED |
| Edit Pending and not Entry | Yes | Yes | PRESERVED |
| Update by JOBID only | Yes | Yes | PRESERVED |
| Attendance edit | Latent (column index hides Action) | Visible Edit (still hides Code col 13) | CHANGED |
| Permit/attendance delete UI | Inert | Inert (soft-delete handlers) | PRESERVED |
| Resend Rejected → code 4 Pending | Yes | Yes + log | PRESERVED |
| Attach manpower whitelist | A84/K208/N21/J8 | Same, logs | PRESERVED |
| Share | WhatsApp deep link `HF_Msg` | Async email/WhatsApp API | CHANGED |
| Back | UrlReferrer (can NRE) | manage_jobid_v2 + month session | UX IMPROVEMENT |
| Error-251 | Yes | Yes | PRESERVED |

---

# PHASE 6 — Business Rule Diff

## 1. JOBID generation

**Old Logic:** `JOB` + `DateTime.Now:yyMMdd` + random 000–999; `SELECT COUNT(*) FROM tbl_jobs WHERE JOBID=@jobid`; process `lock`.  
Evidence: `create_jobid.aspx.cs` `Find_DBCode()`, `Insert_JOBData()`.

**New Logic:** Same algorithm and lock.  
Evidence: `create_jobid_v2.aspx.cs` `Find_DBCode()`, `Insert_JOBData()`.

**Impact:** Identifiers remain compatible.  
**Risk:** Low. Still non-atomic vs DB unique index (SP body unknown). 1000/day theoretical exhaustion.

## 2. Duplicate JOB / same-day job

**Old Logic:** No same-date/WO duplicate check.  
**New Logic:** `CheckExistingJobsForDate()` warns; submit still proceeds. Evidence: `create_jobid_v2.aspx.cs` `CheckExistingJobsForDate()`, `btn_submit_Click()`.  
**Impact:** Supervisors can still create multiple jobs per day.  
**Risk:** Low. Warning may be ignored.

## 3. Permit mandatory / routing

**Old Logic:** `if (DB_WOType == "ARC")` → code 1 / FinalUpldStatus No; else code 3 / Yes. UI after create **always IN-punch**. Permit inbox is jobs already `EntryExit='Entry'`.  
Evidence: `create_jobid.aspx.cs` `Insert_JOBData()`; `create_jobid.aspx` btn_upload Visible=false; `job_permitupload.aspx.cs` `ActiveJOB_Checker()`; `jobstatus_flow.ascx` steps 2 then 3.

**New Logic:** Matrix `Default_MasterStatusCode`. `"3"` → create redirects to IN-punch; else create still redirects to permit. Permit inbox remains `MasterStatusCode='1'`. **IN-punch inbox no longer requires code 3** (UAT-006 / UAT-021): same Active 3-day creator predicates as legacy.  
Evidence: `create_jobid_v2.aspx.cs` `btn_submit_Click()`, `Insert_JOBData()`; `job_permitupload_v2.aspx.cs` `ActiveJOB_Checker()`; `job_inpunch_v2.aspx.cs` `ActiveJOB_Checker()`.

**Impact:** Post-create auto-redirect still prefers permit for matrix code `1`, but a newly created permit-required job **appears in IN-Punch immediately**.  
**Risk:** Medium remaining UX mismatch (redirect vs inbox). Cross-version mixing of permit pages can still diverge.

## 4. Punch eligibility (IN)

**Old Logic:** Active 3-day job, any MasterStatusCode; worker Active; not in roster; no open Entry anywhere.  
Evidence: `job_inpunch.aspx.cs` `ActiveJOB_Checker()`, `txt_empworkman_TextChanged()`.

**New Logic (UAT-006 / UAT-021):** Inbox matches legacy (Active, 3-day, creator — no code-3 gate). Same worker rules; DB duplicate still does not ignore DeleteStatus=1.  
Evidence: `job_inpunch_v2.aspx.cs` `ActiveJOB_Checker()`, `CheckDuplicateEntry()`.

**Impact:** Permit-pending V2 jobs are IN-punch eligible immediately after create (UAT-006). Duplicate Entry and pending-OUT worker rules are unchanged (UAT-021). Soft-deleted Entry rows can still block re-add (`DeleteStatus` omitted from the duplicate SQL).  
**Risk:** Low for inbox eligibility; Medium for DeleteStatus duplicate leftover.

## 5. Status transitions

See Phase 7. Material deltas:
- V2 permit does not set `JOB_Status='Permit Uploaded'` or `PermitUpload`. Evidence: `job_permitupload_v2.aspx.cs` `UpdatePermitStatus()`.
- V2 IN healing always sets Entry even if zero new rows. Evidence: `job_inpunch_v2.aspx.cs` `btn_finalsubmit_Click()`.
- V2 OUT last-punch close restored: confirmation modal then `UpdateJOBTable1` (UAT-029 / UAT-040). Evidence: `job_outpunch_v2.aspx.cs` `CheckPendingOUT()`, `btn_FinalizeShift_Click()`.

**Impact:** Close is no longer a forgotten extra step. Approver list (`JOB_Status='Out-Punch Done' AND EntryExit='Exit'`) sees the job after Close & Send. Review Again can still delay close until the supervisor confirms.  
**Risk:** Low remaining — only if the supervisor dismisses the modal and never Close & Send.

## 6. Close conditions

**Old Logic:** Auto when `CheckforPendingOUT==0`; Blocked if `CheckforPendingPermit==0` else Active.  
Evidence: `job_outpunch.aspx.cs` `CheckPendingOUT()`.

**New Logic (UAT-029 / UAT-040 / UAT-035):** Same Blocked/Active rule and same `UpdateJOBTable1` writes, triggered from Close & Send after last-OUT confirmation modal. Does not write `Incharge_Approval` (still Pending from create). Adds notifications. No new DB states.  
Evidence: `job_outpunch_v2.aspx.cs` `CheckPendingOUT()`, `UpdateJOBTable1()`, `btn_FinalizeShift_Click()`.

**Risk:** Low — confirmation is required UX; close writes match legacy so approval and dashboard update immediately.

## 7. Reopen / cancel / delete

**Old Logic:** Unblock without checks. Hidden hard delete. No Cancelled state.  
Evidence: `manage_jobid.aspx.cs` `GridView1_RowCommand()`, `JOBID_Delete()`.

**New Logic:** Same unblock. Soft delete with audit columns + transaction; list hides `DeleteStatus=1`.  
Evidence: `manage_jobid_v2.aspx.cs` `JOBID_Delete()`, `GridBinder()`.

**Impact:** Deleted V2 jobs recoverable in DB; V1 deletes are gone. Direct detail URL can still open soft-deleted jobs.  
**Risk:** Medium.

## 8. Edit restrictions

**Old/New:** Approved cannot edit basics; Pending+Entry cannot; Rejected can resend. Attendance/permit deletes are UI-inert in both (column-index bugs).  
Evidence: `view_jobdetails.aspx.cs` and `view_jobdetails_v2.aspx.cs` bind/visibility blocks.

**V2 change:** Attendance **Edit** is reachable (Action column not the one being hidden).  
**Risk:** Medium — more supervisors can alter punches without OUT-page timeout rules.

## 9. Approval dependency

**Old Logic:** Create sets Pending. Close does not change it. Approver page sets Approved + code 5. Resend resets to Pending + code 4.  
Evidence: `create_jobid.aspx.cs` `Insert_JOBData()`; `job_outpunch.aspx.cs` `UpdateJOBTable1()`; `jobapprovalpage.aspx.cs` `btn_approve_Click()`; `view_jobdetails.aspx.cs` resend.

**New Logic:** Same flags. V2 close additionally notifies in-charge. Resend does not notify.  
Evidence: `job_outpunch_v2.aspx.cs` `TriggerShiftClosureNotifications()`; `view_jobdetails_v2.aspx.cs` resend handler.

**Risk:** Low for flags; Medium if business expected resend to re-alert.

## 10. Backdating

**Old Logic:** Yesterday only.  
Evidence: `create_jobid.aspx.cs` `btn_dateswap_Click()`.

**New Logic:** Default 2 days; per-user `tlb_Backdate_Exceptions`. Submit does not re-validate min date.  
Evidence: `create_jobid_v2.aspx.cs` `GetUserBackdateLimit()`, `txt_jobdate_TextChanged()`, `btn_submit_Click()`.

**Risk:** Medium — more backdate than V1 for all users (2 vs 1) plus exception rows; possible POST of older date if TextChanged skipped.

## 11. CSM / TBT / SOP

**Old Logic:** Create sets CSM_Documents Yes for ARC in four regions. OUT requires TBT+SOP counts if Yes. IN page links TBT/SOP.  
**New Logic:** CSM from matrix `Req_CSM_Docs`, not region list. OUT gate preserved. IN-page TBT/SOP navigation removed. Documents checklist is create-time metadata only; permit page does not enforce those Doc_IDs.  
**Risk:** High if ARC jobs in AGL/KPO/NINL/JSR now skip CSM because matrix says No, or non-ARC now require CSM because matrix defaults Yes on missing mapping.

## 12. Billing type persistence

**Old Logic:** Always `DDL_BillingType` text/value into SP.  
**New Logic:** Intended Non-Billing → Type `Non-Billing` code `NB` via `WO_BillingNature`; **property never assigned**, so insert uses dropdown (or empty if hidden).  
Evidence: `create_jobid_v2.aspx.cs` lines 25–26, 319–321, 726–727.  
**Risk:** High — Non-Billing jobs may store empty BillingType/BillingCode; dashboard MS/LI counts and memo filters break.

---

# PHASE 7 — State Machines

## Canonical codes (both)

| MasterStatusCode | Meaning in comments/UI |
|---|---|
| 1 | Permit pending |
| 3 | Ready for / in IN-punch |
| 4 | Out-punch done / with approver |
| 5 | Approved (`jobapprovalpage.aspx.cs`) |

`JOBID_Status`: Active | Blocked (and any non-Active treated as blocked in filters).  
`EntryExit`: Created → Entry → Exit.  
`Incharge_Approval`: Pending | Approved | Rejected | Returned.

## Legacy state machine

```
[Create ARC]
  JOB_Status=Created
  MasterStatusCode=1
  EntryExit=Created
  FinalUpldStatus=No
        │
        │  IN-punch allowed anyway (inbox ignores code 1)
        ↓
  JOB_Status=In-Punch Done
  MasterStatusCode=3
  EntryExit=Entry
        │
        │  Permit upload (inbox requires Entry)
        ↓
  JOB_Status=Permit Uploaded
  MasterStatusCode=3
  FinalUpldStatus=Yes
        │
        │  last OUT  (or last attendance delete)
        ↓
  JOB_Status=Out-Punch Done
  MasterStatusCode=4
  EntryExit=Exit
  JOBID_Status=Blocked  (if no pending permit)
        │
        │  Approver approve
        ↓
  JOB_Status=Approved by Approver
  MasterStatusCode=5
  JOBID_Status=Blocked
  Incharge_Approval=Approved

[Create non-ARC]
  JOB_Status=Permit Uploaded, code 3, FinalUpldStatus=Yes, EntryExit=Created
        → IN → Entry → OUT auto-close → Approve

[Last permit deleted]
  JOB_Status=Created, code 1, FinalUpldStatus=No   (from Entry or later)

[Reject]
  Incharge_Approval=Rejected  (approver page; not fully traced here)

[Resend]
  → code 4, Out-Punch Done, Exit, Pending, Blocked

[Manage unblock]
  JOBID_Status Active  (other fields unchanged)
```

## V2 state machine

```
[Create matrix code 1]
  JOB_Status=Created
  MasterStatusCode=1
  EntryExit=Created
  FinalUpldStatus=No
  App_Version=V2
        │
        │  IN-punch inbox LISTS this job immediately (UAT-006)
        │  same predicates as legacy: Active, 3-day, creator
        ↓ IN batch + heal
  JOB_Status=In-Punch Done
  MasterStatusCode=3
  EntryExit=Entry
        │
        │  permit still available while code=1 until first file
        ↓ FileCount>0
  FinalUpldStatus=Yes
  MasterStatusCode=3
  JOB_Status remains "In-Punch Done" if IN already ran
  (or remains "Created" if permit ran first)
  PermitUpload remains "No"             ← NEW / CHANGED (V2 permit omit)
        │
        │  last OUT → confirmation modal (UAT-029)
        │  Close & Send → UpdateJOBTable1 (legacy writes)
        ↓
  JOB_Status=Out-Punch Done
  MasterStatusCode=4
  EntryExit=Exit
  JOBID_Status Blocked|Active
  + in-charge WhatsApp/email                           ← NEW
        ↓ Approve
  code 5 (unchanged approver page)

[Create matrix code 3]
  JOB_Status=Permit Uploaded, FinalUpldStatus=Yes, PermitUpload=N/A
  skip permit inbox
        → IN → last OUT confirm close → Approve

[Missing matrix]
  defaults to code 1 (permit path), strict flags on

NEW states/fields: App_Version=V2, GPS_*, Required_Documents, DeleteStatus/ViewStatus on job/attendance/permit.

REMOVED transition: none — last OUT again closes via `UpdateJOBTable1` after confirmation (UAT-029).

INVALID NOW ALLOWED:
  - Review Again dismisses the close modal without writing (temporary until Close & Send).
  - QueryString bind of another creator’s JOB on permit V2 if masked ID known.

PREVIOUS RESTRICTION REMOVED:
  - IN-punch while MasterStatusCode=1 is allowed again (legacy parity, UAT-006).
  - Site Staff cannot change region (stricter).
  - Attendance edit on details is more reachable (looser).
```

### Transition diff marks

- **New states:** `App_Version=V2`; soft-deleted (`DeleteStatus=1`). The prior “closed-on-finalize-only” waiting state is removed (UAT-029).
- **Removed states:** none of 1/3/4/5 removed; `JOB_Status='Permit Uploaded'` often **never entered** on V2 permit path.
- **Changed transitions:** IN-punch inbox restored to legacy (no code-3 gate, UAT-006); create redirect still may send code-1 jobs to permit first; last OUT prompts confirmation then `UpdateJOBTable1` (UAT-029 / UAT-040); permit upload no longer writes JOB_Status/PermitUpload.
- **Invalid now allowed:** Non-Billing insert with blank billing if dropdown hidden (ViewState bug).
- **Previous restrictions removed:** yesterday-only backdate; ARC-only permit rule (now matrix); WhatsApp-only share.

---

# PHASE 8 — Database Impact

| Table / object | Legacy | V2 | Change |
|---|---|---|---|
| `tbl_jobs` | INSERT via SP; UPDATE status/count/permit/close/edit | Same SP; extra UPDATE Required_Documents, GPS_*, App_Version; soft-delete columns; heal includes ManpowerCount subquery | new columns referenced |
| `tbl_jobspermit` | INSERT dummy Data; hard DELETE | INSERT real bytes; hard DELETE on permit page; soft DELETE on manage | writes changed |
| `tbl_attendance` | SP insert/out; hard DELETE | Same SPs; hard DELETE in/out; soft DELETE on manage | mixed |
| `tbl_Employee_Mustertable` | GP update | GP update + NotificationQueue | preserved + extra |
| `tlb_WO_Data` | SELECT | SELECT + matrix join | reads |
| `tlb_WO_Rule_Matrix` | unused | SELECT routing flags | new table referenced |
| `tlb_Company_Calendar` | unused | SELECT | new |
| `tlb_Backdate_Exceptions` | unused | SELECT | new |
| `tlb_DocumentMaster` / `tlb_Region_Documents` | unused | SELECT | new |
| `tlb_work_state_region` | SELECT menu regions | SELECT + `Req_GPS_Tagging` | new column referenced |
| `tlb_System_Logs` | unused | INSERT missing matrix | new |
| `tbl_Version_Switch_Log` | unused | INSERT V1 fallback | new |
| `NotificationTemplates` / `NotificationQueue` | unused | INSERT | new |
| `tlb_WO_SkillCategory` | SELECT concat | SELECT (V2 inpunch) | preserved |
| `tlb_attendancecodes` | SELECT | SELECT | preserved |
| `tbl_toolboxtalkdata` / `tbl_soptraining` | COUNT for CSM | COUNT for CSM | preserved |
| `SP_InsertInto_JOBSTable` | 33-ish params | same + post UPDATE | preserved SP, extra SQL |
| `SP_InsertInto_AttendanceTable` | per row | per row in batch | preserved |
| `SP_Update_AttendancePunchOUT` | per punch | per punch | preserved |
| Concatenated SQL | widespread | largely parameterized on V2 pages | obsolete SQL on V2 paths |
| `JobWorkflowLogger` files | none | `~/Logs/Job_Workflow_Logs/{JOBID}.txt` | new (filesystem) |

SP bodies themselves are not in repo — **no evidence they changed**.

---

# PHASE 9 — Security Audit

| Finding | Class | Evidence |
|---|---|---|
| V2 create/permit/in/out drop RolePermissionDB/UserRoleDB/REGION from gate | CHANGED / possible regression (authz still only login, as legacy also never checked role *value*) | `create_jobid_v2.aspx.cs` `Page_Load()` vs `create_jobid.aspx.cs` `Page_Load()` |
| Site Staff cannot switch region | SECURITY IMPROVEMENT | `create_jobid_v2.aspx.cs` `Page_Load()` |
| Server-side create validation | SECURITY IMPROVEMENT | `create_jobid_v2.aspx.cs` `btn_submit_Click()` |
| Permit V2 parameterized SQL; legacy concat inbox/delete | SECURITY IMPROVEMENT | `job_permitupload.aspx.cs` `ActiveJOB_Checker` / `GridView1_RowDeleting`; V2 equivalents parameterized |
| Manage V2 parameterized; legacy concat | SECURITY IMPROVEMENT | `manage_jobid.aspx.cs` vs `manage_jobid_v2.aspx.cs` `GridBinder()` |
| `CheckforPendingOUT` still concatenates jobid+supv | residual risk (both) | `CountChecker.cs` `CheckforPendingOUT()` |
| Permit V2 QueryString bind of JOB not in inbox | IDOR / hidden-field-equivalent | `job_permitupload_v2.aspx.cs` `Page_Load()` |
| `DecodeJobID` is reversible Base64, not auth | not a security control | `create_jobid_v2.aspx.cs` `EncodeJobID()` |
| view details trusts QueryString `supv` | IDOR (both) | `view_jobdetails.aspx.cs` / `_v2` `Bind_JOBIDDetails()` |
| Permit download by Id only (both V2 permit and view) | IDOR | `job_permitupload_v2.aspx.cs` `DownloadFile()`; `view_jobdetails_v2.aspx.cs` download |
| Legacy permit `@Data={0}` | data integrity | `job_permitupload.aspx.cs` `InsertIntoDB()` |
| V2 stores real bytes in SQL | SECURITY IMPROVEMENT (integrity) / residual malware in blob | `job_permitupload_v2.aspx.cs` `InsertIntoDB()` |
| Upload extension-only validation (both) | residual | `ImportPermit` both |
| Legacy hardcoded delete path `C:\atswork.in\...` | residual | `job_permitupload.aspx.cs` `rootFolder` |
| V2 MapPath `~/erp_images/Permits/` | SECURITY IMPROVEMENT | `job_permitupload_v2.aspx.cs` `ImportPermit()` |
| `job_360_view` raw jobid into DecodeJobID | exception / DoS of navigation | `job_360_view.aspx.cs` `btn_Act_*`; V2 `DecodeJobID()` |
| UrlReferrer NRE on view V1 | open-adjacent crash | `view_jobdetails.aspx.cs` `Page_Load()` |
| Back V2 is fixed manage URL | SECURITY IMPROVEMENT vs open redirect | `view_jobdetails_v2.aspx.cs` back |
| Legacy `ShowPopup` jQuery `.html(ex.Message)` | XSS if exception text hostile | `create_jobid.aspx.cs` / permit catch |
| V2 PNotify sanitizes quotes/newlines | SECURITY IMPROVEMENT | `create_jobid_v2.aspx.cs` `ShowNotification()` |
| GPS hidden fields trusted if non-empty | hidden field trust | `create_jobid_v2.aspx.cs` `btn_submit_Click()` |
| GridView labels as identity (both manage) | hidden-field analogue | RowCommand both |
| attach_manpower no whitelist/ownership | IDOR (both) | `attach_manpower.aspx.cs` `Page_Load()` |
| Soft delete not applied on detail GET | residual | `view_jobdetails_v2.aspx.cs` `Bind_JOBIDDetails()` |

No classic open-redirect found on V2 success paths (relative known pages). Legacy view back uses raw UrlReferrer — **open redirect** if referrer attacker-controlled.

---

# PHASE 10 — Regression Matrix

## Critical

### R1. Permit-required workflow order inverted
**Partially resolved (UAT-006 / UAT-021).** Create may still redirect to permit (`btn_submit_Click` when code ≠ 3), but V2 IN-Punch inbox no longer requires `MasterStatusCode='3'`. A newly created permit-required job **appears in IN-Punch immediately**, matching legacy `ActiveJOB_Checker()`.
**Remaining difference:** post-create auto-redirect still prefers permit for matrix code `1`; hub navigation to IN-Punch lists the job without a file upload.

### R2. OUT no longer auto-closes the JOB
**Resolved (UAT-029 / UAT-040 / UAT-035 / UAT-040A / UAT-040B).** Last OUT shows a confirmation modal. Close & Send reuses `UpdateJOBTable1` (`JOB_Status='Out-Punch Done'`, `MasterStatusCode='4'`, `EntryExit='Exit'`). Second click or refresh of the close POST skips the write if already closed and still shows the success modal. The job matches the approval inbox immediately and leaves the pending-OUT dashboard count. Finalize Shift is not a required extra step.

### R3. Cross-version inbox mismatch
**Inbox half resolved (UAT-006).** V2 IN-Punch now lists V1-created Active 3-day jobs (no code-3 filter). Dual-run can still mix permit pages (V2 permit inbox remains `MasterStatusCode='1'`).

## High

### R4. JOB_Status not set to Permit Uploaded on V2 permit
**Disappeared write.** Filters/reports on `JOB_Status='Permit Uploaded'` skip V2 permit-path jobs until IN-punch overwrites to In-Punch Done.  
**Repro:** Upload permit on V2; query `tbl_jobs.JOB_Status` — still `Created`. `PermitUpload` still `No`.

### R5. WO_BillingNature / WO_ContractNature never stored
**Works differently.** Non-Billing WO may insert blank billing; location SQL uses non-ARC binder.  
**Repro:** Select Non-Billing WO; hide billing dropdown; submit; inspect `BillingType`/`BillingCode`. Select ARC WO; compare location list vs V1.

### R6. job_360_view deep links throw on V2 punch/permit pages
**Broken navigation.**  
**Repro:** Close shift → popup 360 → use IN/OUT/Permit action buttons (`jobid=` raw). `DecodeJobID` `FromBase64String` throws.

### R7. CSM rule source changed (region list → matrix, default Yes if unmapped)
**Became stricter or more permissive depending on matrix data.** Missing matrix logs error and applies permit+CSM+attendance required.  
**Repro:** WO without matrix row; create; confirm code 1 and CSM docs UI. Compare V1 same WO type non-ARC skip permit.

### R8. Hub cannot switch back to OLD version
**Disappeared.** `jobs_and_manpower_v2.aspx` switch is `href="#"`. Menu CreateJOBS points only at V2.

### R9. Default backdate 2 days vs yesterday-only
**More permissive.** All V2 users can pick D-2 without exception row.

## Medium

### R10. IN-page TBT/SOP shortcuts removed
Users must navigate CSM module separately before OUT CSM gate.

### R11. Skill-mismatch email to hr@atswork.in removed from IN path
Replaced by NotificationQueue SKILL_MISMATCH. Ops relying on that mailbox stop seeing mail unless a worker consumes the queue.

### R12. Duplicate IN vs DeleteStatus=1
V2 may refuse a worker who has a leftover Entry row with DeleteStatus=1 (check SQL omits DeleteStatus).

### R13. Attendance edit more exposed on V2 details
Timeout rules of OUT page do not apply.

### R14. Soft vs hard delete
V2 “Permanent Delete” is recoverable; V1 hidden delete is not. Downstream SQL that ignores DeleteStatus still sees “deleted” attendance.

### R15. Title rules
V1 charset (no hyphen/slash). V2 >3 words (auto title `"{type} attendance for (dd-MM-yyyy)"` is 5 tokens — OK). Manual titles of 3 words or fewer fail.

### R16. Permit number format
V1 NA or digits. V2 allows letters/slash. Downstream permit parsing may break.

### R17. Resend does not notify
V2 share API is a separate button; resend only SQL+log.

### R18. WhatsApp `whatsapp://` share removed from details
Replaced by server-side MSG91/email. Users without that config get no share.

## Low

### R19. Duplicate redirect in create submit
Harmless extra `Response.Redirect(..., false)`.

### R20. Manage badge on V2 hub always 0

### R21. RFV InitialValue mismatch on V2 IN dropdown

### R22. Version-switch logging inconsistent
Create and IN log; permit/out/manage/hub do not.

### R23. Legacy static fields race
Unchanged on V1; V2 mostly ViewState.

---

# PHASE 11 — Executive Change Log

## Technical Summary

- **Architecture:** Parallel V1/V2 page pairs under `WebApplication1/bussiness/production/`. Menu default is V2 hub (`webmaster.Master` CreateJOBS/ManageJOBS). Shared SPs and `tbl_jobs` state machine remain the persistence spine.
- **Code movement:** Dashboard SQL copied V1→V2. Create rewritten around `tlb_WO_Rule_Matrix`, calendar, GPS, documents. Permit/IN/OUT rewritten with Base64 `jobid`, PNotify, `JobWorkflowLogger`. Manage/view parameterized; delete became soft+transactional.
- **New dependencies:** `tlb_WO_Rule_Matrix`, `tlb_Company_Calendar`, `tlb_Backdate_Exceptions`, `tlb_DocumentMaster`, `tlb_Region_Documents`, `tlb_System_Logs`, `tbl_Version_Switch_Log`, `NotificationTemplates`/`NotificationQueue`, `NotificationTriggerHelper`, MSG91/SMTP on close/share, filesystem logs under `~/Logs/`.
- **Unchanged SPs (call-level):** `SP_InsertInto_JOBSTable`, `SP_InsertInto_AttendanceTable`, `SP_Update_AttendancePunchOUT`. Bodies not in repo.
- **Known implementation defects in V2:** unassigned `WO_BillingNature`/`WO_ContractNature`; duplicated create redirects; permit status omitting `JOB_Status`/`PermitUpload`; 360-view raw vs Base64 jobid; dead hub switch-to-old link; permit QueryString IDOR bind.

## Client Business Summary

Written for the client SPOC.

**What stays the same**
- Supervisors still create a JOBID, add manpower (IN), record exit (OUT), and send the shift to the Site In-Charge for approval.
- JOBID format is unchanged (`JOByyMMdd` plus three digits).
- A worker cannot be IN-punched on a new job while still open on another.
- Expired gate pass still blocks adding the person until it is updated.
- Safety toolbox/SOP rules still block OUT when the job is marked as needing CSM documents.

**What changes in daily work**
1. **Permit timing.** On the old screens, people normally IN-punched first and uploaded the permit afterwards. Create V2 still auto-redirects permit-required jobs to permit upload. **IN-Punch inbox listing is restored (UAT-006):** a newly created permit-required job appears in IN-Punch immediately (no `MasterStatusCode='3'` gate). Duplicate Entry protection is unchanged (UAT-021).
2. **Closing the shift (UAT-029 / UAT-040 / UAT-035).** After the last OUT, a confirmation modal appears. **Close & Send** writes the same status as the old automatic close (Out-Punch Done / code 4 / Exit) and shows a success modal. **Review Again** only closes the modal so the supervisor can re-check punches.
3. **The menu opens the new screens.** “Create JOBID” goes to the new dashboard. The new dashboard’s “Switch to OLD Version” button does not actually go back.
4. **New create rules.** Job title must be more than three words. Some regions require sharing GPS at submit. Some work orders auto-fill the title or hide billing. Dates can go back two days by default (old screen: yesterday only), unless an exception allows more.
5. **Site staff cannot change work region** on the new create screen.
6. **Delete is softer.** Removing a job on the new Manage screen hides it rather than wiping it; old hidden delete removed rows permanently.
7. **Notifications.** When a shift is finalized, the Site In-Charge can receive WhatsApp/email. Sharing from job details uses that channel instead of opening the phone’s WhatsApp with a pre-typed message.
8. **Audit trail.** The new screens write a text history per JOBID (who created, uploaded, punched, closed).

**Removed or easy to miss**
- After create, users no longer stay on the page with an “In-Punch” button; they are sent forward automatically.
- IN-punch no longer offers Toolbox Talk / SOP buttons; those remain separate CSM screens but must be completed before OUT if required.
- The old HR email about unmapped skills may stop if operations only watch that mailbox.

**Operational impact**
- Retrain permit-required gangs: create may still land on permit, but IN-Punch lists the job immediately (UAT-006).
- Retrain OUT: last OUT prompts **Close & Send**; that is the legacy close, not an extra Finalize step.
- Do not mix old and new screens for the same job during rollout; inboxes filter differently and can skip or bypass steps.
- Confirm with Admin that each work order’s rule matrix (permit yes/no, CSM yes/no, skip-to-IN) matches how that contract is supposed to run. A missing matrix currently forces permit + CSM + attendance.
- Confirm Non-Billing work orders still store billing type correctly before month-end memo runs (technical defect in the new create save path).

---

## Evidence index (primary methods)

| Area | File | Methods |
|---|---|---|
| Hub V1/V2 | `jobs_and_manpower.aspx.cs`, `_v2.aspx.cs` | `Page_Load`, `LoadDashboardStats` |
| Create V1 | `create_jobid.aspx.cs` | `Page_Load`, `DataChecker`, `Insert_JOBData`, `Find_DBCode`, `Workorder_Binder`, `btn_inpunch_Click` |
| Create V2 | `create_jobid_v2.aspx.cs` | `Page_Load`, `GetUserBackdateLimit`, `DDL_Workorder_SelectedIndexChanged`, `btn_submit_Click`, `Insert_JOBData`, `EncodeJobID` |
| Permit V1 | `job_permitupload.aspx.cs` | `ActiveJOB_Checker`, `InsertIntoDB`, `UpdatePermiStatus`, `btn_inpunch_Click` |
| Permit V2 | `job_permitupload_v2.aspx.cs` | `ActiveJOB_Checker`, `UpdatePermitStatus`, `btn_inpunch_Click`, `DecodeJobID` |
| IN V1 | `job_inpunch.aspx.cs` | `ActiveJOB_Checker`, `txt_empworkman_TextChanged`, `InsertIntoAttendanceTable`, `UpdateJOBTableStatus` |
| IN V2 | `job_inpunch_v2.aspx.cs` | `ActiveJOB_Checker`, `btn_finalsubmit_Click`, `CheckDuplicateEntry` |
| OUT V1 | `job_outpunch.aspx.cs` | `ActiveJOB_Checker`, `CheckPendingOUT`, `UpdateAttendanceTable` |
| OUT V2 | `job_outpunch_v2.aspx.cs` | `CheckPendingOUT`, `UpdateJOBTable1`, `btn_FinalizeShift_Click` (Close & Send), `CanPunchOutWithin32Hours` |
| Manage | `manage_jobid.aspx.cs`, `_v2.aspx.cs` | `GridView1_RowCommand`, `JOBID_Delete`, `JOBID_Status_Swaper` |
| View | `view_jobdetails.aspx.cs`, `_v2.aspx.cs` | `Bind_JOBIDDetails`, `UpdateBasicJOBData`, resend, attach |
| Helpers | `CountChecker.cs`, `JobWorkflowLogger.cs`, `DB_Utility_OH4Y.cs` | pending OUT/permit, FileCount, manpower count |
| Intended V1 order | `jobstatus_flow.ascx` | wizard steps 1–5 |
| Menu | `webmaster.Master` / `.Master.cs` | CreateJOBS → V2 hub |

**Adjacent JOBID participants (not primary pages, but on the graph):** `attach_manpower.aspx`, `job_360_view.aspx`, `jobapprovalpage.aspx`, `view_jobsforapproval.aspx`, `jobs_approval.aspx`, `csm_toolboxtalk.aspx`, `csm_soptraining.aspx`, `homepage.aspx`, `vw_supplyjobs.aspx`, `vw_lineitemjobs.aspx`, `db_controller.aspx` (matrix status labels).
