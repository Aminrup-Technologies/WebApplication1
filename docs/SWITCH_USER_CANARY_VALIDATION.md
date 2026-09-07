# PR E — SWITCH_USER overlay canary (dual-path)

**Status:** Implemented. Not CRUD. Not a platform-wide overlay cutover.  
**Base:** PR #100 (`cursor/legacy-migration-bridge-cf5b`).  
**Permission:** `SWITCH_USER` only.

## Frozen production truths

| Truth | This PR |
| --- | --- |
| Session authentication | Unchanged |
| `USERTYPE == "Admin"` required for Switch User | Enforced before overlay and before config |
| `SwitchUserAuthorizedUsers` | Authoritative fallback when overlay is empty or misses |
| Other permissions | Unchanged |
| Login / Switch User page / master / SQL schema / SessionKeys | Unchanged |
| Security Admin CRUD | Still disabled |

`ImpersonationAudit.CanImpersonate` remains the **legacy-only** engine (Admin + CSV, not impersonating). Page gates stay on `AuthorizationService.CanAccess("SWITCH_USER")`.

## Dual-path

```text
CanAccess("SWITCH_USER")
        ↓
authenticated five-key Session?
        ↓
already impersonating? → deny (no nested switch)
        ↓
USERTYPE == Admin? → else deny
        ↓
overlay Direct/Group grant? → allow (source OVERLAY_DIRECT / OVERLAY_GROUP)
        ↓
SwitchUserAuthorizedUsers CSV? → allow (source USERTYPE+LEGACY_CONFIG)
        ↓
deny
```

Until overlay assignments exist, allow/deny matches Admin + config (same snapshot payload as #100 for that set).

Office Staff never receives `SWITCH_USER`, even with an overlay row or a CSV entry.

## Canary telemetry (`DescribeIdentity`)

For `SWITCH_USER` only, `EffectivePermission` now includes:

| Field | Meaning |
| --- | --- |
| `Granted` / Allowed | Dual-path result |
| `Source` | Winning layer (`OVERLAY_DIRECT`, `OVERLAY_GROUP`, or `USERTYPE+LEGACY_CONFIG`) |
| `LegacyWouldAllow` | Workman is on `SwitchUserAuthorizedUsers` |
| `OverlayWouldAllow` | Overlay Direct or Group row exists |

Example (Admin with both paths):

| Field | Example |
| --- | --- |
| Allowed | true |
| Source | OVERLAY_DIRECT |
| LegacyWouldAllow | true |
| OverlayWouldAllow | true |

These fields are **not** hashed into `AuthorizationSnapshot` (payload is still `WorkmanSL|LoginID|UserType|Code|Granted|Source|Display`).

## Access Analyzer — Overlay Canary Status

Read-only panel on `AccessAnalyzer.aspx`:

| Metric | Source |
| --- | --- |
| Overlay grants | Distinct WorkmanSL with `SWITCH_USER` in overlay Direct or Group inventory |
| Legacy grants | Distinct WorkmanSL in `SwitchUserAuthorizedUsers` |
| Dual-path matches | `Granted == (authenticated && Admin && (OverlayWouldAllow \|\| LegacyWouldAllow))` |
| Divergences | Should stay **0** |
| Health | Healthy iff divergences = 0 after **Validate canary** (or Permission/Legacy scan) |

No editing. After a manual UAT grant, recycle IIS or wait 5 minutes (`PermissionRepository` cache TTL) before Validate.

## UAT SQL (manual only)

Schema is `(WorkmanSL, PermissionId)`, not `PermissionCode`. Do **not** run this in production without a chosen Admin WorkmanSL who is **not** on `SwitchUserAuthorizedUsers` (clean EffectiveAccess delta of 1).

See `scripts/uat_switch_user_canary.sql`.

## UAT procedure

### Before

1. Archive the frozen #100 snapshot.
2. Deploy this build with **zero** overlay employee grants.
3. Access Analyzer **Snapshot** → **Compare** against the frozen file.
4. Expected: payload SHA match, **Changed effective permission count = 0**.
5. **Validate canary**: divergences = 0, overlay grants = 0, dual-path 100%.

If Compare is not PASS, **stop**. Do not insert overlay rows.

### During

1. Insert one Admin overlay grant (script above).
2. Recycle IIS or wait 5 minutes.
3. Permission Inspector on that WorkmanSL: `SWITCH_USER` Allowed true, OverlayWouldAllow true, Source `OVERLAY_DIRECT`.
4. Confirm a **different** Admin who is only on `SwitchUserAuthorizedUsers` still has Allowed true, OverlayWouldAllow false, Source `USERTYPE+LEGACY_CONFIG`.
5. Confirm Office Staff (with or without overlay/CSV) is Denied.
6. Delete the overlay row (rollback in the script). Fallback Admin+CSV still works.

### After (while the test grant is still present)

Access Analyzer **Snapshot** → Compare vs frozen #100 file:

| Metric | Result |
| --- | --- |
| Payload SHA | Different (expected after grant) |
| Changed decisions | Exactly one expected (`SWITCH_USER` for the test WorkmanSL) |
| Unexpected changes | Zero |
| Canary divergences | Zero |

Then delete the grant, Compare again, expect PASS vs frozen #100.

## Validation matrix

| Scenario | Expected |
| --- | --- |
| Admin + Overlay | Allow (`OVERLAY_DIRECT` or `OVERLAY_GROUP`) |
| Admin + Config only | Allow (`USERTYPE+LEGACY_CONFIG`) |
| Admin + Neither | Deny |
| Office Staff + Overlay | Deny |
| Office Staff + Config | Deny |
| Employee | Deny |
| Already impersonating | Deny (return path is `CanReturnFromImpersonation`, not this canary) |

## Regression report

| Surface | Result |
| --- | --- |
| Login / Session builder | Same (not touched) |
| `SwitchUser.aspx` / master link | Same callers (`CanAccess("SWITCH_USER")`); dual-path behind the service |
| JOB360 / attendance / payroll / expense / attach manpower | Same (not `SWITCH_USER`) |
| `create_jobid*` | Same (not overlay candidates) |
| Overlay schema | Same (no DDL) |
| Empty overlay snapshot | Same payload as #100 when CSV membership and Active roster are unchanged |
| CRUD | Still absent |

Known residual vs #100: if any **non-Admin** WorkmanSL is listed in `SwitchUserAuthorizedUsers`, #100 `CanAccess` could have allowed them via the generic config fallback. This canary rejects non-Admin first. Production CSV should be Admin-only; if Compare is not PASS before inserting a grant, inspect those rows and **stop**.

## Success criteria

- Legacy fallback still functions.
- Overlay grant works for Admin.
- Office Staff cannot receive `SWITCH_USER`.
- Snapshot Compare reports only the expected test-grant difference (or PASS with empty overlay).
- `DescribeIdentity` explains Allowed / Source / LegacyWouldAllow / OverlayWouldAllow.
- CRUD remains disabled.

## Next (not this PR)

Security Admin CRUD only after this canary is proven on IIS. Do not disable `SwitchUserAuthorizedUsers` yet.
