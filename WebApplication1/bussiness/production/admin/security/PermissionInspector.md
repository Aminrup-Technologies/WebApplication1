# Permission Inspector (PR C1)

**Page:** `bussiness/production/admin/security/PermissionInspector.aspx`  
**Access:** Direct URL only. No sidebar row. Gate is `AuthorizationService.IsAdmin()` (`USERTYPE == "Admin"`). Office Staff is denied (homepage redirect). Unauthenticated users follow the existing login redirect.  
**Writes:** none. No overlay INSERTs, no cache `Invalidate`, no Session identity leak (subject evaluation restores the live Session).

## What this page is

A read-only diagnostic console on top of PRs #95 / #96. It answers, for any Active employee:

| Question | Evidence on the page |
| --- | --- |
| Why can they log in? | Legacy `IsAuthenticated` (five identity keys on the muster row) |
| Why is Switch User blocked? | `CanImpersonate` false; permission row `SWITCH_USER` with `USERTYPE is not Admin` / allowlist miss / already-impersonating does not apply to the subject |
| Why is JOB360 allowed? | `JOB360_OVERRIDE` source `MODULE_EXCEPTION` (Admin or Office Staff) |
| Why is Payroll allowed? | `PayrollAuthorizedUsers` diagnostic and/or `PAYROLL_OVERRIDE` source `LEGACY_CONFIG` |
| Are overlay permissions active? | Overlay status: schema reachable, direct / group counts |
| Which layer granted each permission? | Effective permission `Source` + `Detail` from `AuthorizationService` |

## How decisions are made

The page does **not** reimplement gates.

1. Page access: live `AuthorizationService.IsAuthenticated()` / `IsAdmin()`.
2. Subject evaluation: `AuthorizationService.DescribeIdentity(...)` temporarily places the employee’s LoginID / USERTYPE / WORKMAN / role keys into Session, **clears `IS_IMPERSONATING`**, calls the existing `IsAuthenticated` / `IsAdmin` / `ImpersonationAudit.CanImpersonate` / config allowlist / `GetEffectivePermissions`, then restores the inspector’s Session in `finally`.
3. Overlay panel: `PermissionRepository` only (`IsOverlaySchemaAvailable`, `PeekSnapshotCached`, grant lists, `GetSnapshotLoadedAtUtc`). Missing tables fail closed and show a warning.

Search follows the Login / Switch User pattern: parameterized SQL, `WorkStatus = Active`, `TOP 100`, name `LIKE` contains. WorkmanSL is exact-or-contains (`LIKE`), which is slightly broader than Switch User’s exact WorkmanSL match so support can find prefixes. Identity load reuses `login.FetchEmployeeRowByLoginId`.

## Authorization regression report

No existing production page was modified. Login, Switch User, payroll, JOB360, attendance, and master menus keep their current callers.

| Existing behavior | Expected | This PR |
| --- | --- | --- |
| Login | Same | No `Login.aspx` edits |
| Switch User | Same | No `SwitchUser` / `ImpersonationAudit` edits |
| Payroll | Same | No payroll page edits |
| JOB360 | Same | No JOB360 page edits |
| Attendance | Same | No exception/anomaly page edits |
| Menus | Same | `webmaster.Master` untouched; inspector is not a menu item |

`CanAccess` order is unchanged. `DescribeIdentity` is inspect-only and restores Session.

## Permission source report

Sources are `EffectivePermission.Source` from `AuthorizationService`, not inferred by the page.

| Source tag | Meaning |
| --- | --- |
| `USERTYPE` | Platform administrator string |
| `USERTYPE+LEGACY_CONFIG` | Switch User via Admin + `SwitchUserAuthorizedUsers` (legacy fallback) |
| `OVERLAY_DIRECT` / `OVERLAY_GROUP` | Switch User overlay canary (Admin still required) |
| `MODULE_EXCEPTION` | JOB360 / attendance Admin **or** Office Staff |
| `GROUP` / `DIRECT` | Overlay |
| `LEGACY_CONFIG` | `SwitchUserAuthorizedUsers` or `PayrollAuthorizedUsers` CSV |
| `LEGACY_HARDCODED` | Hardcoded WorkmanSL |
| `NONE` | Denied; `Detail` explains why |

Breakdown rows are added only when that layer actually contributed (or overlay lists are non-empty). The page never invents a grant.

## Effective permission matrix

`GetEffectivePermissions` rows (same catalog as PR A/B):

| Permission | Typical empty-overlay result |
| --- | --- |
| `SWITCH_USER` | Dual-path canary: Admin + (overlay Direct/Group **or** `SwitchUserAuthorizedUsers`). Telemetry: `OverlayWouldAllow`, `LegacyWouldAllow`. |
| `PAYROLL_OVERRIDE` | `PayrollAuthorizedUsers` CSV |
| `JOB360_OVERRIDE` | Admin or Office Staff |
| `ATTENDANCE_OVERRIDE` | Admin or Office Staff |
| `EXPORT_PAYROLL` | Hardcoded `J8` |
| `USER_ADMIN` | false until overlay assignments exist |
| `LEGACY_PAYROLL_DASHBOARD` | Hardcoded `J8` |
| `LEGACY_ATTACH_MANPOWER` | J8 / A84 / K208 / N21 |
| `LEGACY_EXPENSE_HEADS` | J8 / A84 / K208 |

Worked example (empty overlay, production-shaped): **J8 / Office Staff**

| Question | Result |
| --- | --- |
| Login | `IsAuthenticated` ✓ if the five muster keys are populated |
| Switch User | ✗ `USERTYPE != Admin` |
| JOB360 | ✓ `MODULE_EXCEPTION` |
| Payroll | ✓ if `J8` is on `PayrollAuthorizedUsers` |
| Overlay | 0 direct / 0 groups unless SQL grants were added |
| `USER_ADMIN` | ✗ |

## Out of scope

- Security Admin CRUD (later PR C)
- Menu chrome
- Migrating page gates to `CanAccess` (PR E)
- Changing overlay schema or seed
