# MERGE_CHECKLIST — Security Foundation v2.2

Do **not** start until `FINAL_SIGNOFF.md` is **GO**. Squash each reviewed PR into `Jul_to_Sep_2026_Suport_N_Dev_Works` in order. **Never merge #103.**

Current verdict: **BLOCKED** (see `RELEASE_NOTES.md`).

## Pre-merge

- [ ] `BUILD_EVIDENCE.md` Rebuild succeeded
- [ ] `IIS_VALIDATION.md` J8 Session `USERTYPE=Admin`
- [ ] Switch User / Inspector / Analyzer opened
- [ ] `create_permission_overlay.sql` applied on UAT
- [ ] Canary + snapshot compare unexpected drift = 0
- [ ] `FINAL_SIGNOFF.md` = **GO**
- [ ] #94 marked ready for review (currently draft)

## Squash order

- [ ] #89 merged
- [ ] #91 merged
- [ ] #94 merged
- [ ] #95 merged
- [ ] #96 merged
- [ ] #98 merged
- [ ] #99 merged
- [ ] #100 merged
- [ ] #102 merged
- [ ] #104 merged
- [ ] #103 archived
- [ ] Tag created

Suggested tag after #102: `v2.2-security-foundation`. Tag after #104 only if bootstrap was applied in that environment.

## Post-merge

- [ ] Default branch IIS recycle
- [ ] Overlay schema on that environment if not already applied
- [ ] `#104` dry-run then apply only with a reviewed Active Admin roster
- [ ] `SwitchUserAuthorizedUsers` still lists intended operators

## Regression summary (completed UAT evidence only)

PASS is used only where live evidence exists. CI is not UAT.

| Area | Result | Evidence |
| --- | --- | --- |
| Login | **not evidenced** | IIS login not recorded |
| MFA | **not evidenced** | No MFA UAT notes |
| Session builder | **not evidenced** | Code review only; Session keys not observed on IIS |
| Switch User | **not evidenced** | Page not opened |
| Inspector | **not evidenced** | Page not opened |
| Analyzer | **not evidenced** | Page not opened |
| Overlay | **FAIL / not deployed** | Tables **MISSING** on `atserp_uat` 2026-09-07 |
| Canary | **not evidenced** | `uat_switch_user_canary.sql` not run |
| Bootstrap | **not evidenced** | #104 not run |
| Admin SQL (`J8`) | **PASS** | `promote_uat_admin.sql`; already Admin / ATS-OS / OS-HR |
| Repository | **PASS** | `965934f` / CI on stack PRs |

## Verdict

**BLOCKED**

Do not squash until the Pre-merge boxes are checked.
