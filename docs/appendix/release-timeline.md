# Release timeline

**Program root:** `v2.2.3-governance-final` (`495fc68`). JOBID operational freeze is a **different** version line.

| Tag | SHA | What |
| --- | --- | --- |
| `v2.1.0-jobid-remediation` | `a75bb1e` | Frozen JOBID predicates (not Security Foundation v2.2) |
| `v2.2-security-foundation` | `1f6c147` | Security runtime squash + evidence pack |
| `v2.2.1-governance` | `19c286e` | CONTRIBUTING + architecture flows |
| `v2.2.2-operational` | `20ba520` | JOBID V2.2 roadmap + `jobid-v2.2` templates |
| `v2.2.3-governance-final` | `495fc68` | CODEOWNERS + `docs/governance/*` |

## Security Foundation squash order (do not merge #103)

#89 `e10d6cb` → #91 `6397c1e` → #94 `abd20b7` → #95 `883771a` → #96 `4307111` → #98 `ac0e863` → #99 `30b60dd` → #100 `36a7a4c` → #102 `d1c961b` → #104 `a7048c4` → pack restore `1f6c147`.

Governance: #105 → `19c286e`; #107 → `495fc68` (#106 closed, superseded). JOBID roadmap #67 → `20ba520`.

Pack: [`release/v2.2-security-foundation/`](../release/v2.2-security-foundation/README.md).  
New releases copy [`release/TEMPLATE/`](../release/TEMPLATE/README.md).

GitHub issue label **`jobid-v2.2`** is the JOBID product roadmap (not Security Foundation v2.2).

v2.3 overlay CRUD UI is **not started**.
