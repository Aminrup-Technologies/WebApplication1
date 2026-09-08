# ATS ERP Documentation Program — Charter

**Version:** Documentation program rooted in `v2.2.3-governance-final` (`495fc68`)  
**Branch:** `feature/erp-documentation-program`  
**Base:** `Jul_to_Sep_2026_Suport_N_Dev_Works`  
**Status:** Active — first-pass coverage for phases 5–11 is on the index; deepen when new evidence appears (SP bodies, payroll sheet tables). Do not invent schema.  
**Kind:** Documentation only. Does not change application behavior.

This charter is the governing document for every documentation task on this branch. Navigation hub: [`ERP_DOCUMENTATION_INDEX.md`](ERP_DOCUMENTATION_INDEX.md). Living process (ownership, impact matrix, coverage, gaps): [`governance/documentation_governance.md`](governance/documentation_governance.md).

## Mission

Establish a living, version-controlled knowledge base for ATS ERP that continues until every business module, technical component, database object, security mechanism, administration process, and release history is documented — with repository evidence as the source of truth and traceability from implementation through governance and operations.

## Documentation principles

1. **Evidence first.** Prefer existing docs, source files, commits, PRs, tags, and SQL scripts over memory.
2. **Do not invent.** Never invent tables, workflows, permissions, or WorkmanSL gates.
3. **Preserve ATS terms.** Use `WorkmanSL`, `User_RoleType`, `UserRoleDB`, `RolePermissionDB`, `JOBID`, `tlb_EmployeePermissions` vs `tlb_employee_permissions` exactly.
4. **Do not duplicate.** Link existing documents (`docs/architecture/`, `SECURITY_FOUNDATION_BASELINE.md`, JOBID CRs) instead of rewriting them.
5. **Mark uncertainty.** Use **Verified**, **Inferred**, and **Recommendation** where the repo does not prove a claim.
6. **Cross-link.** Every new document appears in the master index.
7. **Ponytail discipline.** Incremental commits, unified diffs, evidence-first reports.
8. **C# 6 / Session identity.** Runtime rules in `CONTRIBUTING.md` are not reopened here.

## Evidence rules

| Preference | Examples |
| --- | --- |
| Existing documentation | `docs/architecture/*`, `docs/ROLE_PERMISSION_ARCHITECTURE_AUDIT.md`, JOBID CR packs |
| Repository files | `.aspx`, `.cs` (read-only), `.Master`, `SessionKeys.cs` |
| History | Commits, PRs (#89–#104, #105–#107, JOBID #59–#83) |
| Tags | `v2.2-security-foundation`, `v2.2.1-governance`, `v2.2.2-operational`, `v2.2.3-governance-final`, `v2.1.0-jobid-remediation` |
| Scripts | `scripts/*.sql` |

If evidence is missing, write **Inference:** and stop short of schema invention.

## Quality gate (every document)

Before marking a document complete:

- [ ] Purpose documented
- [ ] Business workflow documented (or N/A for pure technical notes)
- [ ] Technical implementation documented
- [ ] Security documented
- [ ] Database interactions documented (or N/A)
- [ ] Dependencies listed
- [ ] Cross-links added to the index
- [ ] Version / baseline recorded (`v2.2.3-governance-final` unless a topic is older)
- [ ] Evidence referenced

## Program phases

| Phase | Outcome |
| ---: | --- |
| 0 | Repository inventory (do not guess module lists) |
| 1 | This charter |
| 2 | Master index + progress tracker |
| 3 | Folder tree (create missing folders only) |
| 4 | Architecture (extend; do not fork Security Foundation docs) |
| 5 | One document per ERP business module |
| 6 | Important tables reverse-documented |
| 7 | Security Foundation consolidation |
| 8 | Administration guide |
| 9 | Developer handbook |
| 10 | Troubleshooting runbooks from real history |
| 11 | Appendix (glossary, keys, timeline) |

## Assistant operating contract

Until this program is complete, documentation agents on this branch must:

- Change **documentation only** unless a later instruction explicitly allows `.cs` / `.aspx` / `.sql`.
- Scan the tree before writing; link rather than copy.
- Work in reviewable increments (one area per commit).
- Update the index tracker when a phase or document lands.
- Produce a Documentation Progress Report at the end of every run.

## Completion criteria

The program is complete only when:

- every ERP module has documentation;
- every major database table in the priority list is documented;
- every security component is documented;
- administration, developer, and troubleshooting guides exist;
- release history is complete and cross-linked;
- quality gates pass on those documents.

Cutting `feature/security-admin-v2.3` is **not** blocked by this program, but v2.3 code PRs remain bound by `CONTRIBUTING.md` and `docs/governance/`.

## Related baselines

| Tag | SHA | Meaning |
| --- | --- | --- |
| `v2.2-security-foundation` | `1f6c147` | Security runtime baseline |
| `v2.2.1-governance` | `19c286e` | CONTRIBUTING + architecture docs |
| `v2.2.2-operational` | `20ba520` | JOBID roadmap + issue templates |
| `v2.2.3-governance-final` | `495fc68` | CODEOWNERS + governance runbooks — **this program's root** |
| `v2.1.0-jobid-remediation` | `a75bb1e` | Frozen JOBID operational predicates |
