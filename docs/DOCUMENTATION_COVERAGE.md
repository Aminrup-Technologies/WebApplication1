# Documentation coverage dashboard

**Baseline:** `v2.2.3-governance-final` (`495fc68`)  
**Documentation baseline:** PR #108 (`feature/erp-documentation-program`)  
**Method:** Counts from the [master index](ERP_DOCUMENTATION_INDEX.md) and [repository inventory](appendix/repository-inventory.md). **Do not treat % as schema completeness.**

## How percentages are calculated

| Input | Evidence |
| --- | --- |
| Denominator | Documents **listed as required** on `ERP_DOCUMENTATION_INDEX.md` for that area (hub + topic pages) |
| Numerator | Those files **present** in the tree (`docs/**/*.md` glob) |
| Quality | **Complete** = Active doc with Verified call sites and no open evidence gap for that topic. **First Pass** = documented from inventory/SQL call sites; depth limited. **Evidence Gap** = index present but [gap register](EVIDENCE_GAP_REGISTER.md) still blocks a stronger claim |

Page-level coverage of all 158 `bussiness/production/*.aspx` files is **not** claimed. Inventory groups those pages into modules; module-level docs exist for every inventory row.

## Dashboard

| Area | Index topics | Present | Coverage | Quality |
| --- | ---: | ---: | ---: | --- |
| Architecture | 10 | 10 | 100% | Complete (Security Foundation flows + extensions; cross-links audited in documentation governance) |
| Business Modules | 19 | 19 | 100% | First Pass (inventory groups; exit/gate/safety recorded as non-pages where applicable) |
| Database | 10 | 10 | 100% | First Pass + Evidence Gap (call contracts; SP **bodies** not in repo) |
| Security | 6 | 6 | 100% | First Pass (hub + threat/verification/change-control + baseline/governance links) |
| Administration | 5 | 5 | 100% | First Pass |
| Operations | 3 | 3 | 100% | First Pass (hubs + existing R0 / TEMPLATE packs) |
| Integrations | 1 | 1 | 100% | First Pass (SMTP, MSG91, Superset cited; no invented SaaS) |
| Developer | 3 | 3 | 100% | First Pass (CONTRIBUTING, CODEOWNERS, handbook) |
| Troubleshooting | 7 | 7 | 100% | First Pass (history-cited runbooks) |
| Appendix | 5 | 5 | 100% | First Pass |

Index topic counts exclude this dashboard, the gap register, and governance process files added after PR #108. Those are tracked under **Governance (this layer)**.

## Governance (this layer)

| Artifact | Status |
| --- | --- |
| [documentation_governance.md](governance/documentation_governance.md) | Present — Active |
| [documentation_impact_matrix.md](governance/documentation_impact_matrix.md) | Present — Active |
| This dashboard | Present — Active |
| [EVIDENCE_GAP_REGISTER.md](EVIDENCE_GAP_REGISTER.md) | Present — Active |
| Release add-on templates | Present under `docs/release/TEMPLATE/` |

## Inventory cross-check (modules)

[appendix/repository-inventory.md](appendix/repository-inventory.md) lists **15** ERP production groupings (identity through legacy Admin). Each grouping has a module or architecture page on the index (**Verified** glob of `docs/modules/*.md` = 19 topic pages + README). Identity is covered by architecture, not a duplicate module page.

Production `.aspx` count in inventory: **158** under `bussiness/production/` (**Verified** glob). Coverage is **by module**, not one doc per aspx.

## Refresh rule

When a PR adds an index row, update **this file’s counts in the same PR**. Do not guess a new % without recounting files.

## Related

- [ERP_DOCUMENTATION_INDEX.md](ERP_DOCUMENTATION_INDEX.md)
- [EVIDENCE_GAP_REGISTER.md](EVIDENCE_GAP_REGISTER.md)
- [documentation_impact_matrix.md](governance/documentation_impact_matrix.md)
