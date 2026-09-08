# ATS ERP Documentation Index

**Breadcrumb:** Documentation Hub → this index  
**Charter:** [`ERP_DOCUMENTATION_CHARTER.md`](ERP_DOCUMENTATION_CHARTER.md)  
**Governance:** [`governance/documentation_governance.md`](governance/documentation_governance.md) · [`documentation_impact_matrix.md`](governance/documentation_impact_matrix.md)  
**Coverage:** [`DOCUMENTATION_COVERAGE.md`](DOCUMENTATION_COVERAGE.md) · **Gaps:** [`EVIDENCE_GAP_REGISTER.md`](EVIDENCE_GAP_REGISTER.md)  
**Baseline:** `v2.2.3-governance-final` (`495fc68`)  
**Rule:** Every new program document must be listed here.

## Quick navigation

| I need… | Go to |
| --- | --- |
| How login and Session work | [authentication-flow](architecture/authentication-flow.md) · [session-architecture](architecture/session-architecture.md) |
| How `CanAccess` / Admin / overlay work | [authorization-flow](architecture/authorization-flow.md) · [permission-overlay](architecture/permission-overlay.md) |
| Switch User | [switch-user](architecture/switch-user.md) · [switch-user-ops](administration/switch-user-ops.md) |
| A business module | [modules/README](modules/README.md) |
| A table or SP call | [database/README](database/README.md) |
| “Why was I sent to login?” | [troubleshooting](troubleshooting/README.md) |
| What to update in my PR | [impact matrix](governance/documentation_impact_matrix.md) |
| What we still do not know | [evidence gap register](EVIDENCE_GAP_REGISTER.md) |
| Cutting a release | [release TEMPLATE](release/TEMPLATE/README.md) |

## Audience

| Audience | Start here |
| --- | --- |
| Business users | [modules](modules/README.md), [JOBID](modules/jobid.md), [reports](modules/reports.md) |
| HR | [hr](modules/hr.md), [employee-registration](modules/employee-registration.md), [users-and-roles](administration/users-and-roles.md) |
| Payroll | [payroll](modules/payroll.md), [administration/config-keys](administration/config-keys.md) (`PayrollAuthorizedUsers`, F17 breakers) |
| Supervisors | [jobid](modules/jobid.md), [supervisor](modules/supervisor.md), [attendance](modules/attendance.md) |
| Administrators | [administration](administration/README.md), [switch-user-ops](administration/switch-user-ops.md), [security-admin](modules/security-admin.md) |
| Developers | [CONTRIBUTING](../CONTRIBUTING.md), [developer handbook](developer/handbook.md), [impact matrix](governance/documentation_impact_matrix.md) |
| IT support | [troubleshooting](troubleshooting/README.md), [verification](security/verification.md), [overlay-cache](troubleshooting/overlay-cache.md) |

## Progress tracker

| Phase | Status |
| --- | --- |
| Repository Discovery | ✅ |
| Charter | ✅ |
| Master Index | ✅ |
| Architecture | ✅ |
| Business Modules | ✅ (inventory coverage; exit process documented as absent) |
| Database | ✅ (call contracts; SP bodies remain out of repo) |
| Security | ✅ |
| Administration | ✅ |
| Developer | ✅ |
| Troubleshooting | ✅ |
| Appendix | ✅ |
| Documentation governance (PR #109) | ✅ process files on this branch |

## Architecture

| Document | Status |
| --- | --- |
| [Architecture hub](architecture/README.md) | Present |
| [Authentication flow](architecture/authentication-flow.md) | Present (Security Foundation) |
| [Authorization flow](architecture/authorization-flow.md) | Present |
| [Impersonation lifecycle](architecture/impersonation-lifecycle.md) | Present |
| [Permission overlay](architecture/permission-overlay.md) | Present |
| [MFA](architecture/mfa.md) | Present |
| [Session architecture](architecture/session-architecture.md) | Present |
| [Switch User](architecture/switch-user.md) | Present |
| [Master pages](architecture/master-pages.md) | Present |
| [Release architecture](architecture/release-architecture.md) | Present |

## Business modules

| Document | Status |
| --- | --- |
| [Employee registration](modules/employee-registration.md) | Present |
| [Attendance](modules/attendance.md) | Present |
| [Payroll](modules/payroll.md) | Present |
| [JOBID](modules/jobid.md) | Present (wrapper → lifecycle audit) |
| [Supply memo](modules/supply-memo.md) | Present |
| [Expense management](modules/expenses.md) | Present |
| [Gate pass fields](modules/gate-pass.md) | Present (not a standalone page module) |
| [Safety pass fields](modules/safety-pass.md) | Present (not a standalone page module) |
| [Exit process](modules/exit-process.md) | Present (no dedicated pages in repo) |
| [Reports](modules/reports.md) | Present |
| [Supervisor features](modules/supervisor.md) | Present |
| [HR operations](modules/hr.md) | Present (map) |
| [CSM / HSE](modules/csm-hse.md) | Present |
| [Helpdesk](modules/helpdesk.md) | Present |
| [Geography masters](modules/geo-masters.md) | Present |
| [Role catalog UI](modules/role-menus.md) | Present |
| [Security inspectors](modules/security-admin.md) | Present |
| [Public site](modules/public-site.md) | Present |
| [Legacy Admin](modules/legacy-admin.md) | Present |

Hub: [`modules/README.md`](modules/README.md). Cross-page notes: [`business/README.md`](business/README.md).

## Database

| Document | Status |
| --- | --- |
| [Inventory / backlog](database/README.md) | Present |
| [`tbl_Employee_Mustertable`](database/tbl_employee_mustertable.md) | Present |
| [`tbl_UserLoginAudit`](database/tbl_user_login_audit.md) | Present |
| [Role catalog and menus](database/role-catalog-and-menus.md) | Present |
| Overlay tables | [permission-overlay](architecture/permission-overlay.md) |
| [`tbl_jobs`](database/tbl_jobs.md) | Present (call contract) |
| [`tbl_attendance`](database/tbl_attendance.md) | Present (call contract) |
| [`tbl_jobspermit`](database/tbl_jobspermit.md) | Present (call contract) |
| [Geo catalogs](database/geo-catalog.md) | Present |
| [Expense logs](database/tbl_expenselogs.md) | Present (call contract) |
| [SP call contracts](database/stored-procedure-call-contracts.md) | Present |

## Security

| Document | Status |
| --- | --- |
| [Security hub](security/README.md) | Present |
| [Threat model](security/threat-model.md) | Present |
| [Verification](security/verification.md) | Present |
| [Change control](security/change-control.md) | Present |
| [Security Foundation baseline](../SECURITY_FOUNDATION_BASELINE.md) | Present |
| [Governance](governance/cursor_governance.md) | Present |
| [Documentation governance](governance/documentation_governance.md) | Present |

## Administration

| Document | Status |
| --- | --- |
| [Administration hub](administration/README.md) | Present |
| [Users and roles](administration/users-and-roles.md) | Present |
| [Switch User ops](administration/switch-user-ops.md) | Present |
| [Config keys](administration/config-keys.md) | Present |
| [Platform Admin bootstrap](PLATFORM_ADMIN_BOOTSTRAP.md) | Present |
| [Platform Admin runbook](PLATFORM_ADMIN_RUNBOOK.md) | Present |

## Operations

| Document | Status |
| --- | --- |
| [Operations hub](operations/README.md) | Present |
| [Release template](release/TEMPLATE/README.md) | Present |
| [R0 operations](R0_RELEASE_OPERATIONS.md) | Present |

## Integrations

| Document | Status |
| --- | --- |
| [Integrations hub](integrations/README.md) | Present (SMTP, MSG91, Superset) |

## Developer

| Document | Status |
| --- | --- |
| [CONTRIBUTING](../CONTRIBUTING.md) | Present |
| [CODEOWNERS](../.github/CODEOWNERS) | Present |
| [Developer handbook](developer/handbook.md) | Present |
| [Impact matrix](governance/documentation_impact_matrix.md) | Present |

## Troubleshooting

| Document | Status |
| --- | --- |
| [Hub](troubleshooting/README.md) | Present |
| [Session redirect](troubleshooting/session-redirect.md) | Present |
| [Switch User denied](troubleshooting/switch-user-denied.md) | Present |
| [Overlay cache](troubleshooting/overlay-cache.md) | Present |
| [Impersonation restore](troubleshooting/impersonation.md) | Present |
| [Impersonation logout](troubleshooting/impersonation-logout.md) | Present |
| [Duplicate Compile items](troubleshooting/duplicate-compile-items.md) | Present |

## Appendix

| Document | Status |
| --- | --- |
| [Repository inventory](appendix/repository-inventory.md) | Present |
| [Glossary](appendix/glossary.md) | Present |
| [Configuration keys](appendix/configuration.md) | Present |
| [Release timeline](appendix/release-timeline.md) | Present |
| [JOB status constants](appendix/job-status-constants.md) | Present |

## Release lineage

| Tag | SHA | Notes |
| --- | --- | --- |
| `v2.2-security-foundation` | `1f6c147` | Security runtime |
| `v2.2.1-governance` | `19c286e` | CONTRIBUTING + architecture |
| `v2.2.2-operational` | `20ba520` | JOBID roadmap |
| `v2.2.3-governance-final` | `495fc68` | CODEOWNERS — **program root** |
| `v2.1.0-jobid-remediation` | `a75bb1e` | JOBID freeze (different line from Security Foundation v2.2) |

**Documentation baseline:** PR #108 (`feature/erp-documentation-program`).  
**Documentation governance:** this branch (`feature/documentation-governance-v2.2`).  
**v2.3 overlay CRUD:** not started; do not cut until asked.

Pack: [`release/v2.2-security-foundation/`](release/v2.2-security-foundation/README.md). Narrative: [appendix/release-timeline.md](appendix/release-timeline.md).
