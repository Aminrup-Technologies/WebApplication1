# ATS ERP Documentation Index

**Charter:** [`ERP_DOCUMENTATION_CHARTER.md`](ERP_DOCUMENTATION_CHARTER.md)  
**Baseline:** `v2.2.3-governance-final` (`495fc68`)  
**Rule:** Every new program document must be listed here.

## Progress tracker

| Phase | Status |
| --- | --- |
| Repository Discovery | ✅ |
| Charter | ✅ |
| Master Index | ✅ |
| Architecture | ✅ |
| Business Modules | ⏳ |
| Database | ⏳ |
| Security | ⏳ |
| Administration | ⏳ |
| Developer | ⏳ |
| Troubleshooting | ⏳ |
| Appendix | ⏳ |

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
| Attendance | Planned |
| Payroll | Planned |
| JOBID | Link existing `JOBID_LIFECYCLE_FUNCTIONAL_AUDIT.md` then module wrapper |
| Supply memo | Planned |
| Expense management | Planned |
| Gate pass | Planned |
| Safety pass | Planned |
| Exit process | Planned |
| Reports | Planned |
| Supervisor features | Planned (`supervisor_wizard.aspx`, CR-001) |
| HR operations | Planned |

See also [`modules/README.md`](modules/README.md).

## Database

| Document | Status |
| --- | --- |
| [Inventory / backlog](database/README.md) | Planned |
| `tbl_Employee_Mustertable` | Planned |
| `tbl_UserLoginAudit` | Planned |
| `tlb_emp_roles` / `tlb_emp_roles_permission` | Planned |
| `tlb_EmployeePermissions` | Planned |
| Overlay tables | Link `architecture/permission-overlay.md` |
| Payroll / attendance / JOB tables | Planned |

## Security

| Document | Status |
| --- | --- |
| [Security Foundation baseline](../SECURITY_FOUNDATION_BASELINE.md) | Present |
| [Governance](governance/cursor_governance.md) | Present |
| Consolidation hub | Planned — `security/README.md` |

## Administration

| Document | Status |
| --- | --- |
| [Platform Admin bootstrap](PLATFORM_ADMIN_BOOTSTRAP.md) | Present |
| [Platform Admin runbook](PLATFORM_ADMIN_RUNBOOK.md) | Present |
| Administration guide hub | Planned — `administration/README.md` |

## Operations

| Document | Status |
| --- | --- |
| [Release template](release/TEMPLATE/README.md) | Present |
| [R0 operations](R0_RELEASE_OPERATIONS.md) | Present |
| Operations hub | Planned — `operations/README.md` |

## Integrations

| Document | Status |
| --- | --- |
| Hub | Planned — `integrations/README.md` (SMTP, MSG91, **Inferred** others) |

## Developer

| Document | Status |
| --- | --- |
| [CONTRIBUTING](../CONTRIBUTING.md) | Present |
| [CODEOWNERS](../.github/CODEOWNERS) | Present |
| Developer handbook | Planned — `developer/handbook.md` |

## Troubleshooting

| Document | Status |
| --- | --- |
| Hub | Planned — `troubleshooting/README.md` |

## Appendix

| Document | Status |
| --- | --- |
| [Repository inventory](appendix/repository-inventory.md) | Present |
| Glossary | Planned |
| Configuration keys | Planned |
| Release timeline | Planned |

## Release history

| Tag | SHA | Notes |
| --- | --- | --- |
| `v2.2-security-foundation` | `1f6c147` | Security runtime |
| `v2.2.1-governance` | `19c286e` | CONTRIBUTING + architecture |
| `v2.2.2-operational` | `20ba520` | JOBID roadmap |
| `v2.2.3-governance-final` | `495fc68` | CODEOWNERS — **program root** |
| `v2.1.0-jobid-remediation` | `a75bb1e` | JOBID freeze |

Pack: [`release/v2.2-security-foundation/`](release/v2.2-security-foundation/README.md).
