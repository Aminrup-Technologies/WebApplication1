# ATS ERP Long-Term Support (LTS) Baseline

**LTS Baseline:** `v2.2.4-documentation-baseline`  
**Commit:** `e4552b3`  
**Established:** 2026-09-08  
**Support Status:** Active  
**Foundation Program:** Complete  

---

## 🏷️ Release Lineage

### Foundation Program Releases (Complete)

| Tag | SHA | Date | Program | Status |
| --- | --- | --- | --- | --- |
| `v2.2-security-foundation` | `1f6c147` | 2026-Q3 | Security Foundation | ✅ Complete |
| `v2.2.1-governance` | `19c286e` | 2026-Q3 | Repository Governance | ✅ Complete |
| `v2.2.2-operational` | `20ba520` | 2026-Q3 | Operational Consolidation | ✅ Complete |
| `v2.2.3-governance-final` | `495fc68` | 2026-Q3 | Repository Governance | ✅ Complete |
| **`v2.2.4-documentation-baseline`** | **`e4552b3`** | **2026-Q3** | **Documentation Program** | **✅ LTS** |

### Product Development Releases (Ongoing)

| Tag | SHA | Date | Program | Status |
| --- | --- | --- | --- | --- |
| `v2.1.0-jobid-remediation` | `a75bb1e` | 2026-Q2 | JOBID Product | 🚧 Active |

### Future Development Releases (Planned)

| Tag | SHA | Date | Program | Status |
| --- | --- | --- | --- | --- |
| `v2.3.0-security-admin` | TBD | 2026-Q4 | Security Admin Implementation | 📋 Planned |

---

## 📋 Supported Tags

### Long-Term Support
- **`v2.2.4-documentation-baseline`** — Primary LTS baseline for all future development

### Foundation Program (Historical)
- `v2.2-security-foundation` — Security infrastructure baseline
- `v2.2.1-governance` — Process governance baseline
- `v2.2.2-operational` — Operational consolidation
- `v2.2.3-governance-final` — Repository governance completion

### Product Development
- `v2.1.0-jobid-remediation` — JOBID operational baseline (separate stream)

---

## 📜 Historical Tags (Preserved)

All Foundation Program tags remain available for historical reference and architectural understanding:

### Security Foundation Implementation
- PRs #89-#104: Authorization service, permission overlays, Switch User, access tooling
- Evidence pack: [`v2.2-security-foundation/`](v2.2-security-foundation/README.md)

### Governance Implementation  
- PRs #105-#107: CONTRIBUTING, CODEOWNERS, branch protection, architectural documentation
- Governance documents: [`../governance/`](../governance/)

### Documentation Implementation
- PRs #108-#110: Evidence-first documentation, governance automation, project manifest
- Documentation charter: [`../ERP_DOCUMENTATION_CHARTER.md`](../ERP_DOCUMENTATION_CHARTER.md)

**Preservation Policy:** Historical tags and branches are preserved indefinitely for architectural reference and implementation lineage.

---

## 🔮 Future Version Policy

### Version Numbering
- **Major versions** (v3.x): Architectural changes requiring migration
- **Minor versions** (v2.x): Feature additions maintaining backward compatibility
- **Patch versions** (v2.2.x): Bug fixes and documentation updates

### LTS Policy
- **Current LTS**: `v2.2.4-documentation-baseline` (indefinite support)
- **Next LTS**: Determined after v2.3 security admin implementation
- **LTS Duration**: Minimum 2 years from establishment date

### Development Branches
- **Feature branches**: `feature/` prefix for major implementations
- **Hotfix branches**: `hotfix/` prefix for critical fixes
- **Documentation branches**: `docs/` prefix for doc-only changes
- **Cursor branches**: `cursor/` prefix for Cursor agent work

### Release Evidence Requirements
All future releases must include:
1. **Evidence pack**: Implementation artifacts, validation results, rollback procedures
2. **Documentation updates**: Impact matrix compliance, coverage maintenance
3. **Security validation**: Authorization compliance, security verification
4. **Process compliance**: CODEOWNERS review, branch protection adherence

---

## 🛡️ Long-Term Support Commitments

### Supported Components
- **Security Foundation**: AuthorizationService, permission overlays, Switch User infrastructure
- **Documentation Governance**: Impact matrices, coverage tracking, evidence gap management
- **Repository Standards**: CODEOWNERS, branch protection, development processes
- **Architecture Documentation**: Cross-linked flows, troubleshooting runbooks, administration guides

### Maintenance Guarantees
- **API Stability**: AuthorizationService interface remains backward compatible
- **Process Continuity**: Governance processes remain stable and self-maintaining
- **Documentation Currency**: Impact matrices ensure documentation tracks code changes
- **Security Standards**: Authorization patterns and security verification continue

### Evolution Path
- **Additive Changes**: New features build on LTS foundation without breaking changes
- **Deprecation Policy**: 6-month notice for any LTS component retirement (not currently planned)
- **Migration Support**: Clear upgrade paths provided for any future architectural changes

---

## 🔧 Technical Specifications

### Repository State at LTS Baseline
- **Primary Branch**: `Jul_to_Sep_2026_Suport_N_Dev_Works`
- **Technology Stack**: ASP.NET WebForms (.NET Framework 4.8, C# 6)
- **Identity Model**: InProc Session with standardized SessionKeys
- **Authorization**: Centralized AuthorizationService with permission overlays
- **Documentation**: Evidence-first with institutionalized governance
- **Database**: SQL Server with stored procedure data access

### Quality Gates
- **Code Review**: CODEOWNERS enforcement for security and governance changes
- **Documentation**: Impact matrix compliance for all changes
- **Security**: Authorization service integration for new features
- **Testing**: Evidence pack validation for all releases

### Dependencies
- **.NET Framework**: 4.8 (Long-term Microsoft support)
- **C# Language**: 6.0 (Repository standard)
- **SQL Server**: Compatible versions (repository does not specify minimum)
- **IIS**: ASP.NET WebForms hosting requirements

---

## 📊 LTS Baseline Health

### Documentation Coverage
- **Total Areas**: 10 (Architecture, Business, Database, Security, Administration, Developer, Troubleshooting, Operations, Integrations, Appendix)
- **Coverage**: 100% across all areas (69 topic files)
- **Quality**: Complete (Architecture), First Pass (most areas), Evidence Gap (Database SP bodies)
- **Governance**: Institutionalized impact matrices ensure self-maintenance

### Code Quality  
- **Authorization**: Centralized through AuthorizationService
- **Session Management**: Standardized keys and five-point validation
- **SQL Security**: Parameterized stored procedure calls
- **Error Handling**: Unified patterns across application

### Process Maturity
- **Review Enforcement**: CODEOWNERS with required approvals
- **Branch Protection**: Force-push prevention, status checks
- **Release Management**: Evidence packs, systematic tagging
- **Documentation**: Self-maintaining through governance automation

**Health Assessment**: Excellent — Stable foundation with mature processes ready for long-term development.

---

## 🚀 Development Readiness

### Next Approved Stream
**`feature/security-admin-v2.3`** — Permission Groups CRUD and delegated administration

### Prerequisites Met
- ✅ AuthorizationService operational
- ✅ Permission overlay infrastructure deployed
- ✅ Switch User capability enabled  
- ✅ Access analysis tools available
- ✅ Documentation governance institutionalized
- ✅ Evidence gap tracking established

### Implementation Guidelines
- **Branch From**: `v2.2.4-documentation-baseline` (`e4552b3`)
- **Base Branch**: `Jul_to_Sep_2026_Suport_N_Dev_Works`
- **Architecture**: Build on existing AuthorizationService and PermissionRepository
- **Documentation**: Follow impact matrix for required documentation updates
- **Security**: Maintain authorization service integration standards

---

## 📍 References

- **Foundation Program**: [`../FOUNDATION_PROGRAM_CLOSURE.md`](../FOUNDATION_PROGRAM_CLOSURE.md) — Complete program history
- **Project Manifest**: `../PROJECT_MANIFEST.md` (from PR #110) — Executive repository overview  
- **Security Baseline**: [`../../SECURITY_FOUNDATION_BASELINE.md`](../../SECURITY_FOUNDATION_BASELINE.md) — Security foundation evidence
- **Release Timeline**: [`../appendix/release-timeline.md`](../appendix/release-timeline.md) — Historical milestones
- **Documentation Charter**: [`../ERP_DOCUMENTATION_CHARTER.md`](../ERP_DOCUMENTATION_CHARTER.md) — Evidence-first principles
- **Release Templates**: [`TEMPLATE/`](TEMPLATE/README.md) — Future release requirements

---

**📅 Established:** 2026-09-08  
**🏷️ LTS Tag:** `v2.2.4-documentation-baseline`  
**📋 Support Status:** Active Long-Term Support  
**✨ Foundation:** Complete — Repository ready for feature development