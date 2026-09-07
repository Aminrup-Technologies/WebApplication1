# PR D — Legacy migration bridge

**Goal:** Replace duplicated authorization *consumers* with `AuthorizationService.CanAccess`, keeping identical runtime behavior while overlay assignments are empty.  
**Not in this PR:** Security Admin CRUD, SQL, Login, Session key model, module-semantics changes.

**Snapshot:** `AuthorizationSnapshot.Generate` (SHA-256 over sorted Active-employee permission rows). Download from Access Analyzer **Snapshot**. This is the rollback baseline before CRUD.

## Validation matrix

| Legacy | New | Result |
| --- | --- | --- |
| Switch User (`CanImpersonate`) | `CanAccess("SWITCH_USER")` | Same (still Admin + CSV, not impersonating; overlay empty) |
| Payroll tab (`PayrollAuthorizedUsers` CSV) | `CanAccess("PAYROLL_OVERRIDE")` | Same until overlay grants exist |
| Payroll dashboards / extra chrome (`WORKMAN == J8`) | `CanAccess("EXPORT_PAYROLL")` | Same |
| Attach manpower (`J8/A84/K208/N21`) | `CanAccess("LEGACY_ATTACH_MANPOWER")` | Same |
| Expense heads (`J8/A84/K208`) | `CanAccess("LEGACY_EXPENSE_HEADS")` | Same |
| JOB360 chrome (`Admin` **or** `Office Staff`) | `CanAccess("JOB360_OVERRIDE")` via existing `IsAdmin()` helper | Same semantics, not normalized to platform Admin |
| Attendance / job exceptions (`Admin` **or** `Office Staff`) | `CanAccess("ATTENDANCE_OVERRIDE")` | Same |
| Login | Unchanged | Same |
| Master menus (`tlb_EmployeePermissions`) | Unchanged | Same |
| Master Switch User link | `CanAccess("SWITCH_USER")` instead of `CanImpersonate` | Same |

Not migrated (not the D3 feature codes; job-create routing):

- `create_jobid.aspx.cs` Office Staff vs Site Staff
- `create_jobid_v2.aspx.cs` Site Staff query filter

Left as-is (not privilege gates):

- `emp_registration.aspx.cs` default `"J8"` when WORKMAN missing (audit stamp)
- Commented J8/A84 checks in `view_monthlyjobs` / `create_supplymemo`
- `rpts/testing.aspx.cs` test EmployeeID
- Hardcoded lists **inside** `AuthorizationService` (canonical fallback)
- `ImpersonationAudit.CanImpersonate` (engine used by `CanAccess("SWITCH_USER")`)

## KPI (after this PR)

| Metric | Count |
| --- | --- |
| Pages using `AuthorizationService` | **15** (13 production + Inspector + Analyzer) |
| Production pages still on raw hardcoded WorkmanSL | **0** live gates |
| Remaining hardcoded lists | **1** location (`AuthorizationService.IsWorkmanOnHardcodedList`) |
| Remaining config CSV readers | Engine: `AuthorizationService` + `ImpersonationAudit`; reporting: Analyzer KPI |
| Remaining direct Session privilege checks | **3** (`create_jobid` / `create_jobid_v2` USERTYPE routing) |
| Overlay-backed assignments | **0** (unchanged) |

Config remains authoritative until overlay rows exist. `CanAccess` order is still Session → module authority → overlay → config → hardcoded.

## Snapshot format

```
ATS-AUTHORIZATION-SNAPSHOT
GeneratedUtc=...
Sha256=<sha256 of sorted data lines>
EmployeeCount=...
GrantCount=...
RowCount=...
Truncated=0|1
--
WorkmanSL|LoginID|UserType|Code|Granted|Source|Display
```

Hash is UTF-8 SHA-256 of the payload lines only (not the header). Cap 1500 Active employees, same as the Analyzer scan. Overlay snapshots are warmed first. Live Session is restored after each `DescribeIdentity`.
