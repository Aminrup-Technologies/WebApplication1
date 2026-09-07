# R0 — Release Operations (v2.2 Platform Freeze)

Status: Operations artifacts (no executable change)  
Release: `v2.2-platform-freeze`  
Deployment commit: `1dff864`  
Executable baseline: `238bd2f`  
Recovery: `a75bb1e` (`v2.1.0-jobid-remediation`)

This pack transitions the completed stabilization program into **operations mode**. It is not CR-001 Phase B and does not authorize new executable development.

| Artifact | File | Audience |
|----------|------|----------|
| Production Deployment Runbook | `docs/R0_PRODUCTION_DEPLOYMENT_RUNBOOK.md` | IIS / deployment engineer |
| Business UAT Sign-off Workbook | `docs/R0_BUSINESS_UAT_SIGNOFF.xlsx` | Client SPOC |
| Hypercare Issue Register | `docs/R0_HYPERCARE_ISSUE_REGISTER.md` | Operations / product |

Companion audit: `docs/UAT_READINESS_v2.2_PLATFORM_FREEZE.md` (UAT CERTIFIED for this tag).

## R0 sequence

1. Deploy tag `v2.2-platform-freeze` (`1dff864`) to IIS.
2. Live smoke test (runbook §6).
3. Structured business UAT (workbook); SPOC signs off.
4. Hypercare 7–14 days; log issues in the register. Do not hot-fix the freeze in place.
5. Freeze the defect list; convert findings into **new Change Requests** from executable baseline `238bd2f`.

## Do not start yet

CR-001 Phase B, QR, GPS, Offline/PWA, multi-company tenancy.

New executable CRs still branch from `238bd2f`. Production remains anchored by `v2.2-platform-freeze` (`1dff864`).
