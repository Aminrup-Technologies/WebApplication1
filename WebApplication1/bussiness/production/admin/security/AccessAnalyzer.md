# Access Analyzer (PR C2)

**Page:** `bussiness/production/admin/security/AccessAnalyzer.aspx`  
**Access:** Direct URL only. Gate is `AuthorizationService.IsAdmin()`. Office Staff → homepage. Anonymous → login.  
**Writes:** none.

## Why this page exists

The Permission Inspector answers “what does **this** employee have?”  
The Access Analyzer answers “**who** has this permission, and which holders are still on legacy layers?”

CRUD stays blocked until those questions are cheap to answer.

## Modes

| Mode | Question | Engine |
| --- | --- | --- |
| Permission | Who is granted this code? | `DescribeIdentity` per Active employee (cap 1500) |
| User | What does this person have? | Inspector-style search + one `DescribeIdentity` |
| Legacy | Who still wins via Config / Hardcoded / Module? | Same scan; filter winning sources |
| Overlay | What is in the overlay DB? | `PermissionRepository` inventory only |
| Compare | Do two snapshots have the same effective access? | `AuthorizationSnapshot.Compare` (payload SHA, not timestamps) |

**Overlay Canary Status** (always visible, read-only): `SWITCH_USER` dual-path health. Overlay grants from inventory, legacy grants from `SwitchUserAuthorizedUsers`, match % / divergences from `DescribeIdentity` after **Validate canary** or a Permission/Legacy scan. Divergences must stay 0.

Source display uses `AuthorizationService.DisplaySource` (never inferred on the page):

| Engine source | Display |
| --- | --- |
| `DIRECT` / `OVERLAY_DIRECT` | Direct |
| `GROUP` / `OVERLAY_GROUP` | Group |
| `LEGACY_CONFIG` | Config |
| `LEGACY_HARDCODED` | Hardcoded |
| `MODULE_EXCEPTION` | Module |
| `USERTYPE` | Admin |
| `USERTYPE+LEGACY_CONFIG` | Admin+Config |
| not granted / `NONE` | Denied |

## KPIs

| KPI | How it is counted |
| --- | --- |
| Pages using AuthorizationService | **15** (13 production gates from PR D + Inspector + Analyzer) |
| Overlay grants | Direct rows + group-inherited assignment rows |
| Config allowlist users | Distinct WorkmanSL in `SwitchUserAuthorizedUsers` + `PayrollAuthorizedUsers` |
| Hardcoded users | Distinct employees from the last scan with a winning `LEGACY_HARDCODED` grant |
| Module exceptions | Distinct employees from the last scan with a winning `MODULE_EXCEPTION` grant |

Permission / Legacy analysis warms overlay snapshots in bulk, then calls `DescribeIdentity` so `CanAccess` order is unchanged.

## Legacy exposure report

Winning source in `{ LEGACY_CONFIG, LEGACY_HARDCODED, MODULE_EXCEPTION, USERTYPE+LEGACY_CONFIG }`. Overlay winners are not listed. This is the PR D/E migration backlog.

## Overlay adoption report

- Direct grants table  
- Group grants table  
- Unused catalog codes (seeded, no assignments)  
- Orphan groups (zero members or zero permissions)  
- Missing tables: warning, empty inventory, no throw  

Empty overlay (PR B seed, no assignments) ⇒ overlay grants **0**, unused = full catalog, groups **0**.

## Permission distribution matrix

After a scan: per-code counts of Direct / Group / Config / Hardcoded / Module / Admin+Config.

## Export

CSV of the last permission, user, legacy, or compare-diff grid (plain text). Overlay CSV exports direct grants if no other grid was run. **Snapshot** downloads `AuthorizationSnapshot` (hash + every scanned Active employee’s effective permissions). **Compare** uploads two snapshot files and reports payload SHA equality plus changed decisions. Print uses a print stylesheet (no Excel).

Zero-drift acceptance: `Changed effective permission count = 0` and matching payload SHA-256. Procedure: `docs/AUTHORIZATION_MIGRATION_VALIDATION_PR_D.md`.

## Regression

No edits to Login, Switch User, payroll, JOB360, attendance, master, or Permission Inspector.

| Existing | This PR |
| --- | --- |
| Login / Switch User | Same |
| Payroll / JOB360 / Attendance | Same |
| Menus | Same |
| Permission Inspector | Same |

## Success check (`SWITCH_USER`)

| Question | Where |
| --- | --- |
| Who has it? | Permission mode list |
| Why? | Source enum + Why column from `DescribeIdentity` |
| Overlay / Config / Hardcoded / Module-only? | Yes/No line above the grid |
