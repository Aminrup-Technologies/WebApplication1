# Security Foundation v2.2 — UAT orchestration

**Branch:** `uat/security-foundation-v2.2`  
**Date:** 2026-09-07  
**PR #103:** draft, **DO NOT MERGE**  
**Overall verdict:** **READY FOR SQUASH MERGE** — see `docs/release/v2.2-security-foundation/RELEASE_NOTES.md`. `#103` remains **DO NOT MERGE**.

Windows sign-off pack: `docs/release/v2.2-security-foundation/` (`BUILD_EVIDENCE.md`, `IIS_VALIDATION.md`, `SQL_EVIDENCE.md`, `CANARY_EVIDENCE.md`, `FINAL_SIGNOFF.md`). Fill those files on the IIS box. **GO** / **NO GO** only in `FINAL_SIGNOFF.md`.

This agent cannot recycle IIS or open pages. Those phases stay **PAUSED**.

## Repository Status

| Check | Result |
| --- | --- |
| Current branch | **PASS** `uat/security-foundation-v2.2` |
| Working tree | **PASS** clean (this file + doc alignment committed with the orchestration run) |
| `0f3c404` is ancestor | **PASS** (`2f164e4` is HEAD at start of this run) |
| PR #103 | **PASS** open draft, title DO NOT MERGE |
| Merge markers `<<<<<<<` | **PASS** none in repo (excluding vendor `=======` underline noise) |

`git log --oneline -15` at start: `2f164e4` … `722afe7` (stack #89→#102 present).

## Phase 2 — Security Foundation integrity

| Check | Result | Evidence |
| --- | --- | --- |
| `IsAdmin()` uses `User_RoleType` | **PASS** | Session `USERTYPE` from `ApplySessionFromEmployeeRow` (`employee["User_RoleType"]`). `IsAdmin()` compares `SessionKeys.UserType` to `"Admin"` (`AuthorizationService.cs` 79–82). |
| `CanAccess("SWITCH_USER")` dual-path | **PASS** | Admin required, then overlay Direct/Group, else `SwitchUserAuthorizedUsers` (`DescribeSwitchUser` 328–375). |
| `CanImpersonate()` preserved | **PASS** | Still Admin USERTYPE **and** CSV only (`ImpersonationAudit.cs` 42–52). Does not call overlay. |
| No duplicate Session keys | **PASS** | 39 unique string values in `SessionKeys.cs` (includes `ShowHomeLoader` + `ORIGINAL_*` + `IMPERSONATION_CORR`). |
| No merge markers | **PASS** | |
| Master Switch User chrome | **PASS** | `webmaster.Master.cs` `BindImpersonationChrome` uses `CanAccess(SWITCH_USER)`. Path is `WebApplication1/bussiness/production/webmaster.Master.cs`. |

Fail-closed: none of these regressions were found.

## Phase 3 — SQL Readiness Report

**PASS**

| Check | promote_uat_admin.sql | create_permission_overlay.sql | uat_switch_user_canary.sql |
| --- | --- | --- | --- |
| Idempotent | PASS (`UPDATE` predicate + dry-run RETURN) | PASS (`IF OBJECT_ID IS NULL` / `IF NOT EXISTS` seeds) | PASS (`NOT EXISTS` insert) |
| Dry-run supported | PASS `@ApplyChanges=0` default | N/A (schema create is additive; no employee grants) | Manual; default WorkmanSL is placeholder |
| No password updates | PASS | PASS (no muster UPDATE) | PASS |
| No `LastLogin` update | PASS | PASS | PASS |
| No `LoginStatus` update | PASS | PASS | PASS |
| Overlay additive | N/A | PASS — no DROP/DELETE/ALTER of legacy tables | Additive INSERT; DELETE is commented rollback only |
| Canary uses `PermissionId` | N/A | PK `(WorkmanSL, PermissionId)` | PASS `INSERT (WorkmanSL, PermissionId)` |

## Phase 4 — Documentation consistency

| Topic | Checklist | UAT analysis | Canary doc | PR D snapshot | Integration |
| --- | --- | --- | --- | --- | --- |
| `J8` | aligned this run | live row | pointer this run | pointer this run | aligned this run |
| `ATS-OS` | aligned this run | live | pointer this run | pointer this run | aligned this run |
| `OS-HR` | aligned this run | live | pointer this run | pointer this run | aligned this run |
| Admin gate = USERTYPE | yes | yes | yes | yes | yes |
| Overlay order | promote → IIS → overlay schema → empty snapshot → canary | same | schema then grant | freeze before canary INSERT | same |

**Prior gap (fixed this run):** checklist / integration / canary / PR D did not name `ATS-OS` / `OS-HR`. Those are live data facts; design docs already agreed on the Admin USERTYPE gate.

## Phase 5 — Pre-IIS operator checklist (exact)

### SQL (UAT `atserp_uat` only)

| Step | Script | Status |
| --- | --- | --- |
| 1 | `scripts/promote_uat_admin.sql` `@ApplyChanges=0` then `1` | **DONE** 2026-09-07 (see Phase 6) |
| 2 | `scripts/create_permission_overlay.sql` | **NOT RUN** — tables missing |
| 3 | `scripts/uat_switch_user_canary.sql` | **NOT RUN** — wait for empty snapshot |

### IIS (this agent cannot run)

1. Checkout `uat/security-foundation-v2.2`, VS2015 Clean + Rebuild, deploy.
2. Recycle IIS.
3. Login as `J8` / `ATS002112`.
4. Open Switch User.
5. Open Permission Inspector.
6. Open Access Analyzer.

## Phase 6 — Live SQL Admin evidence (already executed)

Not invented. Queried `atserp_uat` 2026-09-07.

| Field | Expected | Live AFTER |
| --- | --- | --- |
| `User_RoleType` | Admin | **PASS** Admin |
| `UserRoleDB` | ATS-OS | **PASS** ATS-OS |
| `RolePermissionDB` | OS-HR | **PASS** OS-HR |
| `Role_Permission` | OS-HR | **PASS** OS-HR |

Also: WorkmanSL `J8`, LoginID `ATS002112`, FullName `ANUPAM SHARMA`, WorkStatus Active, OS-HR visible menus 31/31, `LastLogin` unchanged at apply (`2026-09-07 16:56:00`). Only Active Admin. No `Employee_Type=Admin` catalog row.

**Gate: PASS. Continue.**

## Phase 7 — IIS runtime — PAUSED

Do not invent. Operator must recycle IIS and login as `J8`, then paste observations.

| Runtime | Expected | Result |
| --- | --- | --- |
| `Session["USERTYPE"]` | Admin | **PAUSED** |
| `IsAdmin()` | true | **PAUSED** |
| Switch User opens | yes | **PAUSED** |
| Permission Inspector opens | yes | **PAUSED** |
| Access Analyzer opens | yes | **PAUSED** |

## Phase 8 — Overlay deployment — PAUSED

Static review of `create_permission_overlay.sql`: **PASS** (additive, no employee grants, no muster writes).  
Live: overlay tables **MISSING**. Operator runs the script, then paste `INFORMATION_SCHEMA` table list.

## Phase 9 — SWITCH_USER canary — PAUSED

Do not run automatically (`uat_switch_user_canary.sql` header). Replace `REPLACE_WITH_ADMIN_WORKMAN`.

If the grant is on `J8` (on CSV):

| Metric | Expected |
| --- | --- |
| `OverlayWouldAllow` | true |
| `LegacyWouldAllow` | true |
| `Source` | `OVERLAY_DIRECT` |
| Snapshot EffectiveAccess | may stay 0 (already granted via CSV) |

For snapshot **exactly one** EffectiveAccess change, use another Admin **not** on `SwitchUserAuthorizedUsers`.

## Phase 10 — Snapshot — PAUSED

Empty overlay: payload SHA match, EffectiveAccess 0. After canary: expected change only, unexpected 0.

## Phase 11 — Security Foundation v2.2 UAT Verdict

| Phase | Result |
| --- | --- |
| Repository | **PASS** |
| SQL scripts | **PASS** |
| Admin (`J8`) | **PASS** (live SQL) |
| IIS | **PAUSED** |
| Switch User | **PAUSED** |
| Inspector | **PAUSED** |
| Analyzer | **PAUSED** |
| Overlay | **PAUSED** (tables missing) |
| Canary | **PAUSED** |
| Snapshot | **PAUSED** |

**Overall: not PASS.** Success criteria require IIS, overlay, canary, and snapshot. This orchestrator refuses a PASS until those are evidenced.

## Merge recommendation

Do **not** merge PR #103. Archive it after UAT.

When every live gate passes, squash in this order:

| Order | PR |
| ---: | --- |
| 1 | #89 |
| 2 | #91 |
| 3 | #94 |
| 4 | #95 |
| 5 | #96 |
| 6 | #98 |
| 7 | #99 |
| 8 | #100 |
| 9 | #102 |
