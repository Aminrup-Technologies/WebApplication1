# R0 Hypercare Issue Register — v2.2 Platform Freeze

Release: `v2.2-platform-freeze` (`1dff864`)  
Window: 7–14 days after IIS go-live  
Rule: **Do not hot-fix the freeze in place.** Log here, then open a new Change Request from executable baseline `238bd2f` if a code change is required.

Copy this table into operations chat if needed. Keep IDs sequential (`HC-001` …).

## Status values

| Status | Meaning |
|--------|---------|
| Open | Observed; not yet classified |
| Watch | Monitor only; no CR yet |
| CR required | Will become a new Change Request after hypercare freeze |
| Closed — no change | Working as designed / training |
| Closed — CR raised | Linked CR / PR recorded |

## Severity

Critical / High / Medium / Low — production impact, not developer preference.

## Register

| ID | Date | Reporter | Module | Severity | Summary | Expected vs actual | Repro JOBID | Action | Status | Future CR |
|----|------|----------|--------|----------|---------|-------------------|-------------|--------|--------|-----------|
| HC-001 | *example* | | JOB360 | Medium | *Replace with a real observation or delete this row* | | | Future CR | Open | |
| HC-002 | *example* | | Wizard | Low | *Replace with a real observation or delete this row* | | | Future CR | Open | |

Delete the example rows when the first real ticket is logged.

## Classification (end of hypercare)

When the window closes:

1. Freeze this list (no more same-day production edits).
2. Group remaining Open / CR required items.
3. Each executable fix is a **new CR**:
   - starts from `238bd2f`
   - one bounded concern
   - no duplicated V2 logic
   - no M1–M6 predicate change unless the CR authorizes it
   - records rollback SHA
   - waits for post-merge Build + Security before the next slice
4. Items that only need training or runbook clarification stay Closed — no change.

## Modules in scope

Login, Create, IN, Permit, OUT, Close & Send, Approval, JOB360, Supervisor Wizard Phase A, Mobile (JOB360 Phase E), Hub KPIs, Shared infrastructure (`JobIdCodec` / constants / notifications).

Out of scope for in-place freeze work: Phase B–E wizard product increments, QR, GPS, Offline/PWA, tenancy.
