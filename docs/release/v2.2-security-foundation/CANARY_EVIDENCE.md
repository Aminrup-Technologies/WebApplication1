# Canary evidence — SWITCH_USER overlay (PR #102)

**Script:** `scripts/uat_switch_user_canary.sql`  
**Schema:** `tlb_employee_permissions (WorkmanSL, PermissionId)` — not `PermissionCode`, not `tlb_EmployeePermissions`.  
**Cache:** recycle IIS or wait 5 minutes after INSERT/DELETE.

Do not run automatically. Do not seed production.

`Web.config.example` lists `J8` on `SwitchUserAuthorizedUsers`.

- Grant **J8**: Inspector `Source=OVERLAY_DIRECT`, `LegacyWouldAllow=true`. Snapshot **EffectiveAccess** may stay **0**.
- One-row EffectiveAccess delta: set `@WorkmanSL` to an **Admin who is not on the CSV**.

Chosen WorkmanSL: ______________________  
On `SwitchUserAuthorizedUsers`? yes / no

Save Analyzer downloads as:

- `docs/release/v2.2-security-foundation/artifacts/snapshot_before_canary.txt`
- `docs/release/v2.2-security-foundation/artifacts/snapshot_after_grant.txt`
- `docs/release/v2.2-security-foundation/artifacts/snapshot_after_rollback.txt`

(Create `artifacts/` locally; do not commit live snapshots if they contain employee PII beyond what the team already stores.)

---

## Empty overlay

Prereq: `create_permission_overlay.sql` applied; **0** rows in `tlb_employee_permissions` for `SWITCH_USER`.

Access Analyzer → Snapshot. Save `snapshot_before_canary.txt`.

| Metric | Expected | Observed |
| --- | --- | --- |
| Snapshot timestamp | | |
| Employee count | | |
| EffectiveAccess count | | |
| Payload SHA | | |
| Canary overlay grants | 0 | |
| Dual-path divergences | 0 | |

Inspector on `J8` (CSV Admin):

| Metric | Expected | Observed |
| --- | --- | --- |
| Allowed | true | |
| `LegacyWouldAllow` | true | |
| `OverlayWouldAllow` | false | |
| Source | `USERTYPE+LEGACY_CONFIG` | |

Office Staff (if sampled): Denied.

Screenshot — empty overlay Inspector/Analyzer:

```
(attach or path)
```

---

## Overlay grant

In SSMS, replace the placeholder then run the **Grant** + **Verify** batches in `uat_switch_user_canary.sql`.

```sql
DECLARE @WorkmanSL NVARCHAR(50) = N'REPLACE_WITH_ADMIN_WORKMAN';
```

Verify:

```sql
SELECT ep.WorkmanSL, p.PermissionCode
FROM dbo.tlb_employee_permissions ep
INNER JOIN dbo.tlb_permissions p ON p.Id = ep.PermissionId
WHERE ep.WorkmanSL = @WorkmanSL
  AND p.PermissionCode = N'SWITCH_USER';
```

Paste:

```

```

Recycle IIS or wait 5 minutes. Reload Inspector / Analyzer.

| Metric | Expected | Observed |
| --- | --- | --- |
| Allowed | true | |
| `OverlayWouldAllow` | true | |
| `LegacyWouldAllow` | true if WorkmanSL is on CSV; false if not | |
| Source | `OVERLAY_DIRECT` | |
| Unexpected permission changes | 0 | |

If the granted user was **not** on CSV: snapshot vs `snapshot_before_canary.txt` should show **exactly one** SWITCH_USER EffectiveAccess change.

If granted user was **J8**: record Source transition only; EffectiveAccess may be 0.

Save `snapshot_after_grant.txt`. Compare in Analyzer.

| Metric | Expected | Observed |
| --- | --- | --- |
| Unexpected changes | 0 | |
| Expected canary change | 0 (J8) or 1 (off-CSV Admin) | |
| Source transitions | overlay Direct for the granted WorkmanSL | |
| Payload SHA | different from before if EffectiveAccess or source hashed | |

Screenshot — after grant:

```
(attach or path)
```

---

## Overlay removal

Uncomment and run the rollback `DELETE` in `uat_switch_user_canary.sql` (canary grant only). Recycle IIS or wait 5 minutes.

| Metric | Expected | Observed |
| --- | --- | --- |
| `OverlayWouldAllow` | false | |
| Source (J8 / CSV Admin) | `USERTYPE+LEGACY_CONFIG` | |
| Off-CSV Admin Allowed | false | |
| Snapshot vs before | PASS (equal payload) | |

Save `snapshot_after_rollback.txt`.

Screenshot — after removal:

```
(attach or path)
```

## Gate

| Item | PASS / FAIL |
| --- | --- |
| Empty overlay baseline saved | |
| Grant uses `PermissionId` | |
| OverlayWouldAllow true after grant | |
| Source `OVERLAY_DIRECT` after grant | |
| Removal restores legacy path | |
| Unexpected snapshot drift | 0 |
