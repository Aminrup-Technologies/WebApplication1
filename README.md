# ATS ERP (WebApplication1)

![LTS Baseline](https://img.shields.io/badge/LTS%20Baseline-v2.2.4--documentation--baseline-blue) ![Foundation Program](https://img.shields.io/badge/Foundation%20Program-Complete-green) ![Current Stream](https://img.shields.io/badge/Development%20Stream-security--admin--v2.3-orange)

ASP.NET WebForms ERP. Identity is **ASP.NET InProc Session**, not Forms tickets. Canonical login: `WebApplication1/Login.aspx`.

**Foundation Program Status:** ✅ Complete — Repository enters Long-Term Support (LTS) at `v2.2.4-documentation-baseline`  
**Current Development Stream:** `feature/security-admin-v2.3` (ready to start)

## Documentation hub

Start at [`docs/ERP_DOCUMENTATION_INDEX.md`](docs/ERP_DOCUMENTATION_INDEX.md).

| Resource | Path |
| --- | --- |
| Charter | [`docs/ERP_DOCUMENTATION_CHARTER.md`](docs/ERP_DOCUMENTATION_CHARTER.md) |
| Coverage | [`docs/DOCUMENTATION_COVERAGE.md`](docs/DOCUMENTATION_COVERAGE.md) |
| Evidence gaps | [`docs/EVIDENCE_GAP_REGISTER.md`](docs/EVIDENCE_GAP_REGISTER.md) |
| Documentation governance | [`docs/governance/documentation_governance.md`](docs/governance/documentation_governance.md) |
| PR impact matrix | [`docs/governance/documentation_impact_matrix.md`](docs/governance/documentation_impact_matrix.md) |
| Contributing | [`CONTRIBUTING.md`](CONTRIBUTING.md) |

## Foundation Program timeline

| Tag | Meaning | Program |
| --- | --- | --- |
| `v2.2-security-foundation` (`1f6c147`) | Security runtime | Security Foundation ✅ |
| `v2.2.1-governance` (`19c286e`) | CONTRIBUTING + architecture | Repository Governance ✅ |
| `v2.2.2-operational` (`20ba520`) | JOBID roadmap (label `jobid-v2.2`) | Operational Consolidation ✅ |
| `v2.2.3-governance-final` (`495fc68`) | CODEOWNERS — documentation program root | Repository Governance ✅ |
| `v2.2.4-documentation-baseline` (`e4552b3`) | **LTS Baseline** — documentation + governance | Documentation Program ✅ |
| `v2.1.0-jobid-remediation` (`a75bb1e`) | Frozen JOBID predicates (separate stream) | JOBID Product 🚧 |

**Foundation Program:** [`docs/FOUNDATION_PROGRAM_CLOSURE.md`](docs/FOUNDATION_PROGRAM_CLOSURE.md) — Complete  
**Security Baseline:** [`SECURITY_FOUNDATION_BASELINE.md`](SECURITY_FOUNDATION_BASELINE.md) — Historical  
**Architecture:** [`docs/architecture/`](docs/architecture/README.md) — Current

## LTS baseline

**Current LTS Baseline:** `v2.2.4-documentation-baseline` (`e4552b3`)  
**Foundation Status:** Complete — All programs delivered successfully  
**Next Development:** `feature/security-admin-v2.3` (Permission Groups CRUD, Delegated Administration)

Documentation baseline established via PR #108 (evidence-first coverage) and PR #109 (governance automation). Documentation now self-maintains through impact matrices. Do not invent tables or SP bodies; use evidence gap register instead.

## Governance

- [`docs/governance/cursor_governance.md`](docs/governance/cursor_governance.md)
- [`docs/governance/security_change_matrix.md`](docs/governance/security_change_matrix.md)
- [`docs/governance/branch_protection_runbook.md`](docs/governance/branch_protection_runbook.md)
- [`.github/CODEOWNERS`](.github/CODEOWNERS)
- Release packs: [`docs/release/TEMPLATE/`](docs/release/TEMPLATE/README.md)
