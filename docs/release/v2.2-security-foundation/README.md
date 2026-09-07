# Security Foundation v2.2 — Release Candidate pack

**Status:** RC evidence collection. **Not a merge artifact.**  
**Branch:** `uat/security-foundation-v2.2`  
**PR:** [#103](https://github.com/Aminrup-Technologies/WebApplication1/pull/103) — **DO NOT MERGE**  
**Repository integrity commit:** `965934f`  
**Do not squash-merge this branch.** After Windows sign-off, squash the reviewed PRs in order.

This folder is the Windows/IIS sign-off pack. Application code is frozen. Fill the templates; do not invent results.

| File | Purpose |
| --- | --- |
| `README.md` | Release card |
| `RELEASE_NOTES.md` | Consolidation + BLOCKED verdict |
| `CHANGELOG.md` | PR purpose / risk / UAT |
| `MERGE_CHECKLIST.md` | Squash order (do not merge #103) |
| `BUILD_EVIDENCE.md` | VS2015 Clean + Rebuild record |
| `IIS_VALIDATION.md` | Login + three runtime pages |
| `SQL_EVIDENCE.md` | Before/After SQL (Admin + overlay tables) |
| `CANARY_EVIDENCE.md` | Empty / grant / remove + snapshot compare |
| `FINAL_SIGNOFF.md` | GO / NO GO only |

Source of truth (do not diverge):

- `SECURITY_FOUNDATION_UAT_CHECKLIST.md`
- `docs/SECURITY_FOUNDATION_UAT_ORCHESTRATION.md`
- `docs/UAT_ADMIN_ROLE_ANALYSIS.md`
- `docs/SWITCH_USER_CANARY_VALIDATION.md`
- `docs/SECURITY_FOUNDATION_UAT_INTEGRATION.md`
- `scripts/promote_uat_admin.sql`
- `scripts/create_permission_overlay.sql`
- `scripts/uat_switch_user_canary.sql`

## Release card

| Field | Value |
| --- | --- |
| Product | ATS ERP Security Foundation |
| RC | v2.2 |
| Branch | `uat/security-foundation-v2.2` |
| Governance PR | **#103 DO NOT MERGE** — archive after UAT |
| Integrity SHA | `965934f` |
| Database | `atserp_uat` |
| Operator | `J8` / LoginID `ATS002112` / ANUPAM SHARMA |
| `User_RoleType` | `Admin` (live, 2026-09-07) |
| `UserRoleDB` | `ATS-OS` |
| `RolePermissionDB` | `OS-HR` (31/31) |
| Build | Visual Studio 2015, .NET Framework 4.8 |
| IIS | Recycle app pool after deploy and after overlay/canary SQL |

### Included stack (squash after GO)

| Order | PR |
| ---: | --- |
| 1 | #89 Impersonation audit |
| 2 | #91 Switch User |
| 3 | #94 Role/permission audit |
| 4 | #95 AuthorizationService |
| 5 | #96 Overlay infrastructure |
| 6 | #98 Permission Inspector |
| 7 | #99 Access Analyzer |
| 8 | #100 Legacy migration bridge |
| 9 | #102 SWITCH_USER overlay canary |

## Windows sequence (~10 minutes)

| Step | Time | Evidence file |
| --- | --- | --- |
| Clean + Rebuild in VS2015 | 2 min | `BUILD_EVIDENCE.md` |
| IIS recycle | 30 sec | `IIS_VALIDATION.md` |
| Login as J8 | 30 sec | `IIS_VALIDATION.md` |
| Verify 3 pages | 2 min | `IIS_VALIDATION.md` |
| Run `create_permission_overlay.sql` | 1 min | `SQL_EVIDENCE.md` |
| Take Snapshot | 1 min | `CANARY_EVIDENCE.md` |
| Run canary | 2 min | `CANARY_EVIDENCE.md` |
| Compare Snapshot | 1 min | `CANARY_EVIDENCE.md` + `FINAL_SIGNOFF.md` |

`J8` is already on `SwitchUserAuthorizedUsers`. Overlay on `J8` proves `OVERLAY_DIRECT` with `LegacyWouldAllow=true`; snapshot **EffectiveAccess** may stay 0. For a one-row EffectiveAccess delta, grant a **different** Admin not on the CSV.

## Overall status

| Area | Status |
| --- | --- |
| Repository integrity | Complete (`965934f`) |
| Security Foundation code | Complete |
| UAT integration branch | Complete |
| Admin validation (SQL) | Complete |
| Runtime build | Windows |
| IIS validation | Windows |
| Overlay deployment | Windows |
| Canary | Windows |
| Production merge | Blocked until `FINAL_SIGNOFF.md` = **GO** |
