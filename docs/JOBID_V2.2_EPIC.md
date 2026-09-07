# JOBID V2.2 Repository Epic

Status: JOBID product epic (not Security Foundation)  
Date: September 2026  
JOBID baseline: `v2.1.0-jobid-remediation` (`a75bb1e`)  
CR-001 Phase A: **Certified** — `docs/CR-001_EXECUTION_BASELINE.md` (`238bd2f`)  
Roadmap: `docs/JOBID_V2.2_ROADMAP.md`  
GitHub label: **`jobid-v2.2`**

**Security Foundation v2.2 is complete** (`v2.2-security-foundation`, `v2.2.1-governance`). This epic is the **JOBID** product roadmap only. Do not use the GitHub label `v2.2` on these issues.

These files are **GitHub Issue drafts / templates**. They are not GitHub issues and not implementation PRs. Paste a template into a new GitHub issue only after product/engineering accept the CR.

**GitHub templates (appear under New issue after merge):** `.github/ISSUE_TEMPLATE/cr-00*.md`

| ID | Title | Roadmap | Classification in draft | Status |
|----|-------|---------|-------------------------|--------|
| CR-001 | Unified Wizard | Priority 1 | Change Request | **Phase A Certified.** Execution baseline exists. Template = Phase B+ only. |
| CR-002 | Mobile UI | Priority 1 | Change Request (may reclassify to Enhancement if presentation-only) | Draft |
| CR-003 | Supervisor Dashboard | Priority 1 | Change Request (SQL meaning must stay M6) | Draft |
| CR-004 | QR Workflow | Priority 2 | Change Request | Draft |
| CR-005 | GPS Validation | Priority 2 | Change Request | Draft |
| CR-006 | Permit Checklist | Priority 2 | Change Request (read-only UI may reclassify to Enhancement) | Draft |
| CR-007 | Reporting & Analytics | Priority 3 | Change Request | Draft |
| CR-008 | Technical Debt | §4 Debt | Refactor (stop if predicates change) | CR-010 shipped codec / constants / notify. Remaining debt only. |

Do not implement Phase A again. Do not alter tag `v2.1.0-jobid-remediation`. Do not change executable code in the same PR as these drafts. Do not treat this epic as Security Foundation work.
