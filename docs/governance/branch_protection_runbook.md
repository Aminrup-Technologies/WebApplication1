# Branch protection runbook

**Baseline:** Security Foundation v2.2.1 (`v2.2.1-governance`, `19c286e`).  
**Applies to:** `Jul_to_Sep_2026_Suport_N_Dev_Works` (default / release branch).  
**Related:** `.github/CODEOWNERS`, `docs/governance/security_change_matrix.md`.

This runbook describes **recommended GitHub settings**. It does **not** claim they are already enabled. An organization owner or repository admin must apply them in the GitHub UI (or via the GitHub API) after reviewing this document.

Do not enable protection in a way that blocks hotfix operators without a documented bypass (repository admin, or a time-limited exemption recorded in the release pack).

## Target branches

Protect at least:

| Branch / ref | Why |
| --- | --- |
| `Jul_to_Sep_2026_Suport_N_Dev_Works` | Release line; squash target for Security Foundation |
| Tags `v2.2-security-foundation`, `v2.2.1-governance` | Baseline pins (tag immutability is a hosting setting; do not retag) |

Feature branches (`feature/*`, `cursor/*`) stay unprotected so agents can force-push their own work-in-progress. Never force-push the release branch.

## Required settings

Apply these on the release branch rule:

| Setting | Required value | Why |
| --- | --- | --- |
| Require a pull request before merging | On | No direct commits to the release line |
| Required approving reviews | At least 1 | Human sign-off |
| Require review from Code Owners | On | `.github/CODEOWNERS` becomes enforceable |
| Dismiss stale reviews when new commits are pushed | On (recommended) | Review matches the tip |
| Require status checks to pass | On | CI before merge |
| Require branches to be up to date before merging | On | No stale squash onto an old tip |
| Require conversation resolution before merging | On | No open review threads |
| Require linear history | On | Matches the squash-merge release process |
| Allow force pushes | Off | Protects `v2.2` / `v2.2.1` history |
| Allow deletions | Off | Release branch must remain |
| Allow bypassing the above settings | Restrict to repository admins only | Emergency only |

Do not enable "Allow merge commits" as the primary path. Squash (or rebase) keeps linear history.

## Status checks to require

Name the **job** GitHub reports, not the workflow file. After a PR has run CI once, pick the exact check names from the branch protection UI.

Checks that currently exist in this repository (names may appear as the job id):

| Workflow | Job id | File |
| --- | --- | --- |
| Build | `build` | `.github/workflows/build.yml` |
| Security check | `security-check` | `.github/workflows/security-check.yml` |

Until an admin has confirmed the displayed names, treat this table as a **candidate list**, not as already-required checks.

```
[screenshot: Settings → Branches → Branch protection rule → Require status checks]
Paste a screenshot of the required-checks picker here after the rule is saved.
```

## Apply (GitHub UI)

1. Open the repository on GitHub.
2. **Settings → Branches → Add branch protection rule** (or edit the rule for `Jul_to_Sep_2026_Suport_N_Dev_Works`).
3. Branch name pattern: `Jul_to_Sep_2026_Suport_N_Dev_Works`.
4. Enable each checkbox in **Required settings** above.
5. Under **Require status checks**, search for `build` and `security-check` (or the names shown on a recent PR).
6. Save the rule.

```
[screenshot: Settings → Branches → rule for Jul_to_Sep_2026_Suport_N_Dev_Works]
Paste a screenshot of the saved rule (reviews, code owners, status checks, linear history, no force push, no delete) here.
```

```
[screenshot: Pull request → Checks tab]
Paste a screenshot of a PR showing required checks plus Code Owner review requested.
```

## Apply (GitHub API, optional)

Admins who prefer the API can PUT a protection object for the release branch. Do not run this from a Cursor agent unless an org owner explicitly asks. Confirm team slugs and check names first.

Example shape (not executed; placeholders only):

```http
PUT /repos/Aminrup-Technologies/WebApplication1/branches/Jul_to_Sep_2026_Suport_N_Dev_Works/protection
```

Body fields to set: `required_pull_request_reviews.required_approving_review_count`, `require_code_owner_reviews: true`, `required_status_checks.strict: true`, `required_status_checks.contexts`, `required_linear_history: true`, `allow_force_pushes: false`, `allow_deletions: false`, `required_conversation_resolution: true`.

## CODEOWNERS enforcement

`.github/CODEOWNERS` lists `@Aminrup-Technologies` as the placeholder owner. GitHub only **requires** that owner after "Require review from Code Owners" is on.

Until a dedicated security team exists, the organization account is the reviewer of record. Replace `@Aminrup-Technologies` with `@Aminrup-Technologies/<team>` when that team is created. See comments in `.github/CODEOWNERS`.

```
[screenshot: CODEOWNERS file in a PR "Files changed" with owner requested]
Paste after the first PR that touches a protected path.
```

## Verify

After an admin saves the rule, confirm on a throwaway docs PR:

- [ ] Direct push to the release branch is rejected.
- [ ] Force push is rejected.
- [ ] Merge is blocked without an approving review.
- [ ] Merge is blocked without Code Owner approval when CODEOWNERS paths change.
- [ ] Merge is blocked while `build` or `security-check` is failing or pending.
- [ ] Merge is blocked if the branch is behind the release tip.
- [ ] Merge is blocked with unresolved review conversations.
- [ ] Merge commit button is hidden or fails; squash/rebase remains.
- [ ] Branch cannot be deleted from GitHub.

Record the date the rule was saved in the next `docs/release/<version>/` pack (`IIS_VALIDATION.md` is the wrong place; use `README.md` or `RELEASE_NOTES.md`).

## Rollback

If protection blocks a legitimate emergency:

1. A repository admin temporarily allows bypass (or disables the rule).
2. Record the reason and the commits in the release pack.
3. Re-enable the rule the same day.

Do not leave the release branch unprotected.
