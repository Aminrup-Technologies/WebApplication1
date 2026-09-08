# ATS ERP (WebApplication1)

ASP.NET WebForms ERP. Identity is **ASP.NET InProc Session**, not Forms tickets. Canonical login: `WebApplication1/Login.aspx`.

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

## Security Foundation timeline

| Tag | Meaning |
| --- | --- |
| `v2.2-security-foundation` (`1f6c147`) | Security runtime |
| `v2.2.1-governance` (`19c286e`) | CONTRIBUTING + architecture |
| `v2.2.2-operational` (`20ba520`) | JOBID roadmap (label `jobid-v2.2`) |
| `v2.2.3-governance-final` (`495fc68`) | CODEOWNERS — documentation program root |
| `v2.1.0-jobid-remediation` (`a75bb1e`) | Frozen JOBID predicates (not Security Foundation v2.2) |

Baseline narrative: [`SECURITY_FOUNDATION_BASELINE.md`](SECURITY_FOUNDATION_BASELINE.md). Architecture: [`docs/architecture/`](docs/architecture/README.md).

## Documentation baseline

PR #108 (`feature/erp-documentation-program`) is the ERP documentation baseline. Documentation governance (impact matrix, coverage dashboard, gap register, release add-on templates) lives on `feature/documentation-governance-v2.2`. Do not invent tables or SP bodies; log gaps instead.

## Governance

- [`docs/governance/cursor_governance.md`](docs/governance/cursor_governance.md)
- [`docs/governance/security_change_matrix.md`](docs/governance/security_change_matrix.md)
- [`docs/governance/branch_protection_runbook.md`](docs/governance/branch_protection_runbook.md)
- [`.github/CODEOWNERS`](.github/CODEOWNERS)
- Release packs: [`docs/release/TEMPLATE/`](docs/release/TEMPLATE/README.md)
