# JOBID V2 Maintenance Guidelines

Version: 1.0  
Baseline: September 2026  
Branch: `Jul_to_Sep_2026_Suport_N_Dev_Works`

---

## Purpose

This repository now has a frozen JOBID V2 baseline (`v2.1.0-jobid-remediation` at `a75bb1e`, PR #59–#65).

Future work must preserve that baseline unless a formally approved Change Request modifies it.

These guidelines exist so later maintenance stays auditable: every change can be classified, checked against the frozen workflow, and recorded in the same control documents.

---

## Protected Baseline

These three documents form the controlled baseline:

- `docs/JOBID_LIFECYCLE_FUNCTIONAL_AUDIT.md` — functional specification
- `docs/RELEASE_READINESS_PACK_v1.0.md` — production freeze and deployment pack
- `docs/JOBID_CHANGELOG_v2.1.0.md` — release manifest

Do not treat comments, tickets, or informal chat as overriding these files.

---

## Change Classification

| Type | Example | Approval |
|------|---------|----------|
| Hotfix | Decode exception on a legitimate JOB360 link; Close & Send double-submit | Engineering lead; document in audit after deploy |
| Enhancement | UI copy, notification text, extra logging that does not change JOB states | Engineering lead; no workflow predicate change |
| Change Request | New JOB status, different Create→IN→Permit order, new dashboard meaning | Formal Change Request before implementation |
| Refactor | Extract a helper without changing SQL predicates or redirects | Engineering lead; prove behavior unchanged |
| Documentation | Audit, changelog, this file | Engineering lead; no executable change |

A Change Request is required for any alteration to JOB states, inbox filters, close writes, approval predicates, JOB360 encoding, or dashboard KPI meaning.

---

## Workflow Protection Rules

- Do not introduce new JOB states or status codes.
- Preserve Create → IN-Punch → Permit Upload → OUT-Punch → Close & Send → Approval.
- Do not require permit before IN for permit-required jobs.
- Preserve JOB360 encoding: V2 pages decode URL-safe Base64; producers encode; Base64 is not authorization.
- Preserve permit inbox semantics: Active, last 3 days, creator, `EntryExit='Entry'` (additional files after the first upload remain possible).
- Preserve permit write rules from M2: `PermitUpload` / `JOB_Status` follow file count; do not overwrite Out-Punch Done; do not roll `MasterStatusCode` to 1 after IN.
- Preserve Close & Send: last OUT confirms; close writes `JOB_Status='Out-Punch Done'`, `MasterStatusCode='4'`, `EntryExit='Exit'`.
- Preserve approval: `JOB_Status='Out-Punch Done'` AND `EntryExit='Exit'`.
- Preserve dashboard KPI meanings: Pending IN = `EntryExit='Created'`; Pending Permit = outstanding work (`EntryExit='Entry'` AND `FinalUpldStatus='No'`); Pending OUT = `MasterStatusCode='3'` AND `EntryExit='Entry'`.
- Do not reuse the permit inbox predicate as the Pending Permit badge.
- Do not add schema or stored-procedure changes unless a Change Request covers them.
- Do not weaken session, creator, or company checks to fix routing.

---

## Pull Request Checklist

- [ ] Change type is classified (Hotfix / Enhancement / Change Request / Refactor / Documentation).
- [ ] Change Request is attached if workflow, states, encoding, inbox, close, approval, or KPI meaning would change.
- [ ] Create → IN → Permit → OUT → Close → Approval is unchanged, or the CR explains the new path.
- [ ] No unintended workflow regression (IN eligibility, permit continuity, Close & Send, approval list).
- [ ] JOB360 encode/decode contract unchanged (unless the CR covers it).
- [ ] Dashboard KPI meanings unchanged (unless the CR covers it).
- [ ] No schema change, or the CR includes a schema plan and rollback.
- [ ] UAT updated for the affected area (use existing IDs; add IDs only when the CR requires new coverage).
- [ ] `docs/JOBID_LIFECYCLE_FUNCTIONAL_AUDIT.md` updated.
- [ ] Changelog updated if the change is release-impacting.
- [ ] Authorization checks are not weakened.

---

## Versioning Policy

- **Major:** workflow change (order, JOB states, close or approval predicates).
- **Minor:** approved enhancement that keeps the v1.0 baseline behavior.
- **Patch:** bug fix that restores documented baseline behavior.

The current production baseline is **v2.1.0-jobid-remediation**. The next release number must follow this policy and must not silently rewrite the frozen lifecycle.
