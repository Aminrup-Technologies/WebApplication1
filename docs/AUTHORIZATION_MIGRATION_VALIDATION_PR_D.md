# PR D — Post-migration authorization snapshot validation

**Status:** Required before merging PR #100 into the next permission-model change.  
**Do not start:** PR E (overlay canary) or Security Admin CRUD until this document’s live payload comparison is **PASS**.

The hashed snapshot is an **immutable pre-migration baseline**. Compare it to a post-migration snapshot **before** any overlay assignment, catalog edit, or CRUD work.

## Acceptance

Full snapshot files may differ (`GeneratedUtc=` is a header timestamp).

```text
SHA256(sorted authorization rows BEFORE)
==
SHA256(sorted authorization rows AFTER)
```

Equivalently:

```text
Effective authorization set BEFORE
==
Effective authorization set AFTER
```

for every employee/permission combination covered by the scan (TOP 1500 Active employees × 9 tracked codes).

### Zero-drift gate

```text
Changed effective permission count = 0
```

If this is non-zero, **stop**. Do not merge conceptually, do not start PR E, and do not start CRUD. Investigate the listed decision diffs.

`AuthorizationSnapshot.Compare` also fails on source/display-only row drift even when the Granted bit is unchanged.

## How to compare (IIS)

1. Archive the **Before** file. Treat it as immutable. Do not regenerate it after overlay or employee-status changes and then call that a baseline.
2. Deploy / recycle the After build (PR #100).
3. Sign in as a platform Admin (`USERTYPE == Admin`).
4. Open `~/bussiness/production/admin/security/AccessAnalyzer.aspx`.
5. Click **Snapshot**. Save `authorization-snapshot-*.txt`.
6. Mode **Compare** → upload Before and After → **Compare snapshots**.
7. Copy the metrics table and any changed rows into the live section below.

The hash in `Sha256=` is UTF-8 SHA-256 of the sorted payload lines only (not the header). Compare mode re-hashes the payload and diffs `WorkmanSL|LoginID|Code`.

Take both files against the **same database**, with no Active-employee roster change and no overlay writes between downloads. Otherwise the payload hash will move for data reasons, not code reasons.

## Construction proof (DescribeIdentity)

PR D did **not** edit `AuthorizationService.cs`. The snapshot is produced only by `DescribeIdentity` (temporary Session swap, `GetEffectivePermissions`, restore). Overlay assignments remain **0**.

Therefore, for the same employee set, config CSVs, and empty overlay:

| Check | Result |
| --- | --- |
| `AuthorizationService` gates | Unchanged vs PR #99 |
| Snapshot payload SHA-256 | Equal by construction |
| Changed effective permission count | **0** |
| Changed source/display rows | **0** |

The snapshot does not re-run deleted page-local `if (WORKMAN == "J8")` predicates. Page-consumer equivalence is the mapping table below, not the hash.

### Why a live Before file on #99 may not exist

`AuthorizationSnapshot` shipped in the same PR as the consumer migration. A true pre-D download requires either:

- cherry-picking `AuthorizationSnapshot.cs` plus the Analyzer **Snapshot** button onto the #99 build, then freezing that file, or
- treating the construction proof as the D DescribeIdentity gate, and freezing the **first** #100 snapshot as the immutable baseline for **PR E**.

Both are valid. The live table is still required on IIS before PR E.

## Metrics

Tracked codes (9): `SWITCH_USER`, `PAYROLL_OVERRIDE`, `JOB360_OVERRIDE`, `ATTENDANCE_OVERRIDE`, `EXPORT_PAYROLL`, `USER_ADMIN`, `LEGACY_PAYROLL_DASHBOARD`, `LEGACY_ATTACH_MANPOWER`, `LEGACY_EXPENSE_HEADS`.

Grant buckets (Granted = 1 only):

| Bucket | Winning `Source` |
| --- | --- |
| Legacy Config | `LEGACY_CONFIG` or `USERTYPE+LEGACY_CONFIG` |
| Legacy Hardcoded | `LEGACY_HARDCODED` |
| Module | `MODULE_EXCEPTION` |
| Overlay | `DIRECT` or `GROUP` |

### DescribeIdentity (construction)

Same employee set, empty overlay, unchanged service:

| Metric                   | Before | After | Delta |
| ------------------------ | -----: | ----: | ----: |
| Active employees scanned |      N |     N |     0 |
| Permissions evaluated    |    9×N |   9×N |     0 |
| Allowed decisions        |      A |     A |     0 |
| Denied decisions         |  9×N−A | 9×N−A |     0 |
| Legacy Config grants     |      C |     C |     0 |
| Legacy Hardcoded grants  |      H |     H |     0 |
| Module grants            |      M |     M |     0 |
| Overlay grants           |      0 |     0 |     0 |

Fill N / A / C / H / M from the snapshot header and Compare metrics on IIS. Deltas must be 0.

### Live IIS (fill from Compare)

| Metric                   | Before | After | Delta |
| ------------------------ | -----: | ----: | ----: |
| Active employees scanned |        |       |       |
| Permissions evaluated    |        |       |       |
| Allowed decisions        |        |       |       |
| Denied decisions         |        |       |       |
| Legacy Config grants     |        |       |       |
| Legacy Hardcoded grants  |        |       |       |
| Module grants            |        |       |       |
| Overlay grants           |        |       |       |

Payload SHA-256 Before:  
Payload SHA-256 After:  
Changed effective permission count:  
Compare verdict:

## Changed authorization decisions

**DescribeIdentity:** none (service unchanged).

**Live IIS:** list every Compare-grid row. The empty list is the only passing result.

| Kind | WorkmanSL | LoginID | Code | Before granted | After granted | Before source | After source |
| --- | --- | --- | --- | --- | --- | --- | --- |
| *(none)* |  |  |  |  |  |  |  |

If this table is not empty, stop.

## Page-consumer mapping (not hashed)

These replacements are equivalent **when overlay is empty** and the Session is a complete five-key login (`USERID`, `RolePermissionDB`, `UserRoleDB`, `USERNAME`, `WORKMAN`).

| Page | Legacy predicate | `CanAccess` | Same when |
| --- | --- | --- | --- |
| `SwitchUser.aspx.cs` (4 sites) | `ImpersonationAudit.CanImpersonate` | `SWITCH_USER` | Overlay empty; `CanAccess` still calls `CanImpersonate` first |
| `webmaster.Master.cs` Switch User link | `CanImpersonate` | `SWITCH_USER` | Same |
| `viewupdate_empmustertabledata_v2.aspx.cs` | `PayrollAuthorizedUsers` CSV vs `WORKMAN` | `PAYROLL_OVERRIDE` | Overlay empty; same CSV key |
| `pyrl_managedashbrd.aspx.cs` | `WORKMAN == J8` | `EXPORT_PAYROLL` | Overlay empty; hardcoded list is `J8` |
| `pyrl_gnrtdashbrd.aspx.cs` | `WORKMAN == J8` | `EXPORT_PAYROLL` | Same |
| `viewupdate_emppayrolldata.aspx.cs` | `WORKMAN == J8` | `EXPORT_PAYROLL` | Same |
| `view_jobdetails.aspx.cs` / `_v2` | `J8/A84/K208/N21` | `LEGACY_ATTACH_MANPOWER` | Same four WorkmanSL values |
| `add_expense_heads.aspx.cs` / `add_expense_subheads.aspx.cs` | `J8/A84/K208` | `LEGACY_EXPENSE_HEADS` | Same three WorkmanSL values |
| `job_360_view.aspx.cs` local `IsAdmin()` | `USERTYPE` Admin **or** Office Staff | `JOB360_OVERRIDE` | Module exception unchanged |
| `manage_job_exceptions.aspx.cs` / `analyze_attendance_anomalies.aspx.cs` | Admin **or** Office Staff | `ATTENDANCE_OVERRIDE` | Same |

Left alone (not overlay candidates): `create_jobid.aspx.cs`, `create_jobid_v2.aspx.cs` USERTYPE routing (3 remaining Session privilege checks).

### Residual deny-closed (incomplete Session only)

`CanAccess` requires `AuthorizationService.IsAuthenticated()` (five Session keys). Some pages previously tested only `USERID` + `USERTYPE`/`WORKMAN`. `webmaster.Master` already requires the five keys on GET (`!IsPostBack`). Complete logins via `ApplySessionFromEmployeeRow` are unchanged. A malformed postback Session that had `USERID` but was missing another identity key can now fail closed. That is not an effective-access change for production logins.

## KPI after D (unchanged by this validation)

| Metric                            | Before D | After D |
| --------------------------------- | -------: | ------: |
| AuthorizationService callers      |        2 |      15 |
| Live hardcoded Workman gates      |        7 |       0 |
| Overlay assignments               |        0 |       0 |
| Remaining direct privilege checks |        — |       3 |

The three remaining checks are `create_jobid*` job-create routing, not authorization-overlay candidates.

## Sequencing freeze

```text
#100 → payload SHA equivalence (this document) → PR E SWITCH_USER canary → Security Admin CRUD
```

PR E must **not** disable legacy Config / Workman fallback. Overlay grant is additive. The frozen #100 snapshot is the Before file for that canary.
