# Documentation impact matrix

**Baseline:** `v2.2.3-governance-final` (`495fc68`)  
**Normative for future PRs.** Process only; no runtime behavior.

Use this table when filling the **Documentation Impact** checklist in `.github/pull_request_template.md`. Update **all** matching rows in the **same** PR as the code (or SQL/docs) change. If a cell would require inventing a table or SP body, add a row to [`EVIDENCE_GAP_REGISTER.md`](../EVIDENCE_GAP_REGISTER.md) instead.

Canonical names and evidence rules: [`ERP_DOCUMENTATION_CHARTER.md`](../ERP_DOCUMENTATION_CHARTER.md). Ownership: [`documentation_governance.md`](documentation_governance.md).

## Mandatory mapping

| Change | Documentation required |
| --- | --- |
| New or renamed `.aspx` (ERP production) | [Module documentation](../modules/README.md) for that inventory group; index row; inventory note if it is a **new** module |
| New master page / `.Master` gate | [master-pages.md](../architecture/master-pages.md); [session-architecture.md](../architecture/session-architecture.md) if Session keys change |
| New or changed Session key | [session-architecture.md](../architecture/session-architecture.md); [authentication-flow.md](../architecture/authentication-flow.md) if identity; [glossary.md](../appendix/glossary.md) |
| Login / MFA / `ApplySessionFromEmployeeRow` | [authentication-flow.md](../architecture/authentication-flow.md), [mfa.md](../architecture/mfa.md) as applicable |
| `AuthorizationService` / `CanAccess` / `IsAdmin` | [authorization-flow.md](../architecture/authorization-flow.md); [security/threat-model.md](../security/threat-model.md) if residual risk changes |
| New overlay permission code | `scripts/` comment + [permission-overlay.md](../architecture/permission-overlay.md); security hub; [glossary](../appendix/glossary.md) if a new ATS term |
| Switch User / impersonation | [switch-user.md](../architecture/switch-user.md), [impersonation-lifecycle.md](../architecture/impersonation-lifecycle.md), [switch-user-ops.md](../administration/switch-user-ops.md), [troubleshooting](../troubleshooting/README.md) |
| New SQL table (script or evidenced CREATE) | New `docs/database/` page; [database README](../database/README.md); index |
| New or changed stored procedure **call** | [stored-procedure-call-contracts.md](../database/stored-procedure-call-contracts.md) and the table doc if parameters map to columns |
| SP **body** added to the repo | Replace call-contract-only notes; close the matching [evidence gap](../EVIDENCE_GAP_REGISTER.md) |
| New AppSettings / config key | [appendix/configuration.md](../appendix/configuration.md), [administration/config-keys.md](../administration/config-keys.md), [integrations](../integrations/README.md) if SMTP/MSG91/Superset |
| New report or Superset embed | [reports.md](../modules/reports.md) |
| New release / tag | [appendix/release-timeline.md](../appendix/release-timeline.md); copy `docs/release/TEMPLATE/` including module/SQL/security/hotfix templates |
| JOBID status / punch path | [jobid.md](../modules/jobid.md); do not fork frozen predicates; [job-status-constants.md](../appendix/job-status-constants.md) if constants change |
| Payroll / attendance dashboards | [payroll.md](../modules/payroll.md) / [attendance.md](../modules/attendance.md) |
| Operator runbook change | [administration](../administration/README.md) and/or [troubleshooting](../troubleshooting/README.md) |
| Coverage or gap list change | [DOCUMENTATION_COVERAGE.md](../DOCUMENTATION_COVERAGE.md), [EVIDENCE_GAP_REGISTER.md](../EVIDENCE_GAP_REGISTER.md) |

## Same-PR rule

Do not land runtime or SQL without the docs row. If the author cannot evidence a schema claim, they must **not** write columns; they file a gap and document the **call contract** only.

## Documentation-only PRs

Still update the index. Do not modify `.cs`, `.aspx`, `.sql`, or `.config` unless a later instruction explicitly allows it.

## Related

- [`.github/pull_request_template.md`](../../.github/pull_request_template.md)
- [`CONTRIBUTING.md`](../../CONTRIBUTING.md)
