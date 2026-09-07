# Branch cleanup runbook — Security Foundation v2.2

**Baseline:** tag `v2.2.1-governance` (`19c286e`) on `Jul_to_Sep_2026_Suport_N_Dev_Works`.  
**When:** After governance PRs land and **before** v2.3 overlay-CRUD work.  
**This document does not delete anything.** An operator with `repo` permission runs the commands.

Security Foundation code already lives on the release branch as squash commits. The stacked feature branches are leftovers. GitHub shows those PRs as **CLOSED**, not MERGED, because they were squash-landed locally (new SHAs). Do not reopen them to "merge properly."

## Never delete

| Ref | Why |
| --- | --- |
| `Jul_to_Sep_2026_Suport_N_Dev_Works` | Release line |
| `v2.2-security-foundation` | Runtime baseline (`1f6c147`) |
| `v2.2.1-governance` | Governance baseline (`19c286e`) |
| `docs/release/v2.2-security-foundation/` | Evidence pack |
| `docs/release/TEMPLATE/` | Future packs |

Do not force-push the release branch. Do not retag. Do not mass-delete unrelated `cursor/*` branches (JOBID, MFA, CR-010, and older support lines).

## Eligible after squash (Security Foundation stack)

Delete **remote** branches only when the PR is closed **and** the squash commit is on `Jul_to_Sep_2026_Suport_N_Dev_Works`.

| PR | Branch | Squash on release | Delete remote? |
| ---: | --- | --- | --- |
| #89 | `cursor/impersonation-audit-foundation-cf5b` | `e10d6cb` | Yes |
| #91 | `cursor/switch-user-cf5b` | `6397c1e` | Yes |
| #94 | `cursor/role-permission-architecture-audit-cf5b` | `abd20b7` | Yes |
| #95 | `cursor/authorization-service-foundation-cf5b` | `883771a` | Yes |
| #96 | `cursor/permission-overlay-infrastructure-cf5b` | `4307111` | Yes |
| #98 | `cursor/permission-inspector-cf5b` | `ac0e863` | Yes |
| #99 | `cursor/access-analyzer-cf5b` | `30b60dd` | Yes |
| #100 | `cursor/legacy-migration-bridge-cf5b` | `36a7a4c` | Yes |
| #102 | `cursor/switch-user-overlay-canary-cf5b` | `d1c961b` | Yes |
| #104 | `feature/platform-admin-bootstrap` | `a7048c4` | Yes |
| #105 | `feature/security-foundation-governance` | `19c286e` | Yes |
| #103 | `uat/security-foundation-v2.2` | **never merged** | Yes, after operators confirm the pack on default is enough |
| #87 | `feature/refactor-session-builder` | already inside #89 | Yes, then close leftover open PR |
| #86 | `cursor/auth-session-architecture-audit-cf5b` | docs superseded by `docs/architecture/` | Yes, then close leftover open PR |
| #106 | `feature/security-ownership-governance` | superseded by this closure PR | After this PR squash-lands |

Leave historical `April26_*`, `June26_*`, `cursor/cr010*`, `cursor/login-mfa-*`, and other non-v2.2 branches alone unless a separate cleanup CR says otherwise.

## Procedure

1. Confirm release tip contains the squash commits (`git log --oneline origin/Jul_to_Sep_2026_Suport_N_Dev_Works | head`).
2. Confirm tags: `git rev-parse v2.2-security-foundation^{}` = `1f6c147…`, `v2.2.1-governance^{}` = `19c286e…` (or later governance squash on top of that tag — **do not move the tag**).
3. For each eligible branch: close any leftover open PR (do not merge), then:

```bash
git push origin --delete <branch>
```

4. Delete local leftovers: `git branch -d <branch>` (use `-D` only if it is fully superseded).
5. Record deleted refs in the next `docs/release/<version>/RELEASE_NOTES.md` (name + date). Do not invent IIS/SQL results.

If `git push origin --delete` is rejected, branch protection or missing permission is the cause. Stop. Do not force-push.

## UAT branch (#103)

`uat/security-foundation-v2.2` was **DO NOT MERGE**. The evidence pack was copied onto the release branch (`1f6c147`). Operators may keep the UAT branch until they no longer need the integration worktree, then delete it. Never squash-merge it onto default.

## Open leftovers (close, do not merge)

These were already absorbed or superseded. Merging them onto default would replay Session/docs history and conflict.

- [#87](https://github.com/Aminrup-Technologies/WebApplication1/pull/87) — `ApplySessionFromEmployeeRow` (inside #89)
- [#86](https://github.com/Aminrup-Technologies/WebApplication1/pull/86) — session architecture audit (replaced by `docs/architecture/`)
- [#106](https://github.com/Aminrup-Technologies/WebApplication1/pull/106) — ownership files (included in this closure PR)

JOBID / CR-010 open PRs are **out of scope**.

## After cleanup (start of v2.3)

v2.3 is overlay CRUD and permission groups on the **existing** engine. Branch from `Jul_to_Sep_2026_Suport_N_Dev_Works` after this governance lands. Do not branch from a deleted stack ref. Do not reopen Forms Authentication, `UserRoleDB` as authority, or new `WORKMAN` gates.

See `docs/governance/cursor_governance.md` and `docs/architecture/permission-overlay.md`.
