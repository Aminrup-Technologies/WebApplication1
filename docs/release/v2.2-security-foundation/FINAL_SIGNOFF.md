# Final sign-off — Security Foundation v2.2 RC

**Branch:** `uat/security-foundation-v2.2`  
**PR #103:** **DO NOT MERGE.** Archive after UAT.  
**Integrity SHA:** `965934f`  
**Filled by:** _________________  
**Date:** _________________

Evidence files in this folder must be complete before circling a recommendation. Empty Windows gates are **NO GO**.

## Gates

| Gate | Status |
| --- | --- |
| Repository | PASS |
| Build | |
| IIS | |
| Login | |
| Switch User | |
| Inspector | |
| Analyzer | |
| Overlay | |
| Canary | |
| Snapshot | |

Status values: **PASS** or **FAIL** only. Repository is PASS from `965934f` (clean branch, SQL/code integrity, live `J8` Admin SQL). Do not change Repository to FAIL unless a later commit on this branch broke that freeze.

## Final recommendation

Circle exactly one:

**GO**

**NO GO**

No other wording.

### GO means

All ten gates are PASS. Unexpected snapshot authorization drift is 0. Squash in this order; do not merge #103:

| Order | PR |
| ---: | --- |
| 1 | #89 |
| 2 | #91 |
| 3 | #94 |
| 4 | #95 |
| 5 | #96 |
| 6 | #98 |
| 7 | #99 |
| 8 | #100 |
| 9 | #102 |

### NO GO means

Stop. Do not squash. Leave #103 draft. Record the failing gate in the matching evidence file.
