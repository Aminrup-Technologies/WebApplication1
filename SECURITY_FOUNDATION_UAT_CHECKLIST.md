# Security Foundation v2.2 — UAT checklist

Single Visual Studio + IIS sign-off for the integrated stack on `uat/security-foundation-v2.2`.  
**Do not merge that branch.** After this checklist is signed, squash the reviewed PRs in order (#89 → #91 → #94 → #95 → #96 → #98 → #99 → #100 → #102).

Integration details: `docs/SECURITY_FOUNDATION_UAT_INTEGRATION.md`.  
Orchestration status: `docs/SECURITY_FOUNDATION_UAT_ORCHESTRATION.md`.  
RC evidence pack (Windows sign-off): `docs/release/v2.2-security-foundation/`.

## UAT operator identity (live `atserp_uat`, 2026-09-07)

| Field | Value |
| --- | --- |
| WorkmanSL | `J8` |
| LoginID | ATS002112 |
| FullName | ANUPAM SHARMA |
| `User_RoleType` | `Admin` |
| `UserRoleDB` | `ATS-OS` (do not change) |
| `RolePermissionDB` | `OS-HR` (31/31 visible; do not change) |
| Admin gate | `AuthorizationService.IsAdmin()` = Session `USERTYPE` from `User_RoleType` |
| Overlay tables | **Missing until** `scripts/create_permission_overlay.sql` |
| SQL already applied | `scripts/promote_uat_admin.sql` (no-op; already Admin) |

`Web.config.example` lists `J8` on `SwitchUserAuthorizedUsers`. A clean snapshot **EffectiveAccess** delta of 1 requires a **different** Admin who is not on that CSV. Granting overlay to `J8` still proves Inspector `OVERLAY_DIRECT` while `LegacyWouldAllow` stays true.

## Overlay deployment order

1. `scripts/promote_uat_admin.sql` (done on UAT; dry-run then apply).
2. Recycle IIS → login as `J8` → Switch User / Inspector / Analyzer.
3. `scripts/create_permission_overlay.sql` (schema + catalog seeds only; no employee grants).
4. Recycle IIS or wait 5 minutes → freeze empty-overlay snapshot.
5. `scripts/uat_switch_user_canary.sql` INSERT → Inspector/Analyzer → DELETE rollback.

## Phase 1 — Environment

| Field | Value |
| --- | --- |
| Branch | `uat/security-foundation-v2.2` |
| Tester | |
| Date | |
| IIS version | |
| SQL Server | ATS UAT |
| Visual Studio | 2015 |
| Target | .NET Framework 4.8 |
| Build (Clean/Rebuild) | |
| Commit | |

## Phase 2 — Build validation

| Step | Result (PASS/FAIL) | Notes |
| --- | --- | --- |
| Solution loads |  | |
| Clean |  | |
| Rebuild |  | |
| No missing references |  | |
| IIS site starts |  | |
| Database connected |  | Confirm overlay tables exist or Inspector/Analyzer fail closed |

If overlay tables are missing, run `scripts/create_permission_overlay.sql` on **UAT only** (additive; no employee grants).

## Phase 3 — Authentication

Canonical login: `/Login.aspx` (class `login`). Session is InProc; cookie `ASP.NET_SessionId`. Identity is built by `ApplySessionFromEmployeeRow`. Landing: `~/bussiness/production/homepage_v2.aspx`. Master GET requires `USERID`, `RolePermissionDB`, `UserRoleDB`, `USERNAME`, `WORKMAN`.

| Test | Result | Notes |
| --- | --- | --- |
| Login |  | Homepage loader overlay from #93 should still appear |
| MFA |  | |
| Remember Me |  | |
| Session timeout |  | |
| Password expiry |  | |

## Phase 4 — Switch User

Open: `/bussiness/production/SwitchUser.aspx`

Gate is `AuthorizationService.CanAccess("SWITCH_USER")` (Admin, then overlay, else `SwitchUserAuthorizedUsers`). Nested impersonation stays blocked.

| Test | Result | Notes |
| --- | --- | --- |
| Admin on allowlist can open page |  | |
| Office Staff blocked |  | Redirect home / deny |
| Search Active employees |  | |
| Switch |  | Banner shows target + original admin |
| Return |  | Restores original Session keys |
| Nested switch blocked |  | While impersonating, cannot start another switch |
| Master “Switch User” / “Return to my account” |  | |

## Phase 5 — Permission Inspector

Open: `/bussiness/production/admin/security/PermissionInspector.aspx`  
Admin only. Direct URL. No sidebar. No writes.

| Test | Result | Notes |
| --- | --- | --- |
| Office Staff redirected |  | |
| Search Active employee |  | |
| Identity panel |  | USERTYPE / WORKMAN / LoginID |
| Effective permissions |  | Includes OverlayWouldAllow / LegacyWouldAllow on SWITCH_USER |
| Source / layer |  | |
| Overlay health / cache |  | Missing tables → warning, fail closed |
| Session restored |  | Inspector remains the logged-in admin |

## Phase 6 — Access Analyzer

Open: `/bussiness/production/admin/security/AccessAnalyzer.aspx`  
Admin only. Read-only.

| Mode | PASS | Notes |
| --- | --- | --- |
| Permission |  | |
| User |  | |
| Legacy |  | |
| Overlay |  | Inventory only |
| Snapshot |  | Downloads hashed snapshot |
| Compare |  | Two files; payload SHA, not timestamps |
| Canary |  | Overlay Canary Status + Validate canary |

## Phase 7 — Authorization snapshot (empty overlay)

Download **Before** and **After** with no overlay employee grants (same database, no roster change).

| Metric | Expected | Actual |
| --- | --- | --- |
| Payload SHA | Equal | |
| EffectiveAccess | 0 | |
| SourceOnly | 0 | |
| Added | 0 | |
| Removed | 0 | |

Verdict: PASS / FAIL

If FAIL before any canary INSERT, **stop**. Likely cause: non-Admin on `SwitchUserAuthorizedUsers` after the #102 Admin-first gate.

## Phase 8 — Overlay canary (`SWITCH_USER` only)

Script: `scripts/uat_switch_user_canary.sql`  
Schema is `(WorkmanSL, PermissionId)`, not `PermissionCode`. Recycle IIS or wait 5 minutes after INSERT/DELETE (`PermissionRepository` cache).

Use an **Admin** WorkmanSL who is **not** on `SwitchUserAuthorizedUsers`.

### Step 1 — Empty overlay

| Check | Expected | Actual |
| --- | --- | --- |
| Canary overlay grants | 0 | |
| Dual-path divergences | 0 | |
| Admin + config | Allow, source `USERTYPE+LEGACY_CONFIG` | |
| Office Staff | Deny | |

### Step 2 — One Admin grant

| Check | Expected | Actual |
| --- | --- | --- |
| Inspector Allowed | true | |
| Source | `OVERLAY_DIRECT` | |
| OverlayWouldAllow | true | |
| Config-only other Admin | Still allow (legacy fallback) | |
| Office Staff + overlay (if tested) | Deny | |
| Snapshot vs frozen Before | SHA different; **exactly one** expected SWITCH_USER change; unexpected = 0 | |
| Payload SHA | | |
| Changed rows | | |
| Unexpected rows | 0 | |

### Step 3 — Delete grant

| Check | Expected | Actual |
| --- | --- | --- |
| OverlayWouldAllow | false | |
| Config Admin | Allow again via fallback | |
| Snapshot vs frozen Before | PASS (equal payload) | |

## Phase 9 — Security regression

| Area | PASS | Notes |
| --- | --- | --- |
| Login |  | Session builder + MFA + homepage loader |
| Master |  | Five-key GET gate; menus from `tlb_EmployeePermissions` |
| Payroll |  | `CanAccess("PAYROLL_OVERRIDE")` / `EXPORT_PAYROLL` |
| JOB360 |  | Admin **or** Office Staff (`JOB360_OVERRIDE`) |
| Attendance |  | Admin **or** Office Staff (`ATTENDANCE_OVERRIDE`) |
| Switch User |  | |
| AuthorizationService |  | No Admin blanket superuser |
| PermissionRepository |  | Fail closed if tables missing |

`create_jobid*` USERTYPE checks are job-create routing, not overlay candidates.

## Phase 10 — Final sign-off

| Item | Result |
| --- | --- |
| Build |  |
| IIS |  |
| Snapshot (empty overlay) |  |
| Canary |  |
| Unexpected changes |  |
| Approved |  |

Tester: ________________________  Date: ____________

## Release recommendation

- **If all phases PASS:** squash-merge the individual PRs in the order above. Discard or leave `uat/security-foundation-v2.2` unmerged.
- **If snapshot or canary FAIL:** do not merge #100/#102; investigate using Inspector + Compare grid. Do not start Security Admin CRUD.
