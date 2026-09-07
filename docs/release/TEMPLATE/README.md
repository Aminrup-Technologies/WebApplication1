# ERP release evidence pack (template)

Copy this folder to `docs/release/<version>/` at the start of every ERP release (UAT/RC). Fill the files during Windows/IIS UAT. Do not invent results. Final sign-off is **GO** or **NO GO** only.

This structure was proven on Security Foundation v2.2 (`docs/release/v2.2-security-foundation/` on the UAT orchestration branch).

| File | Purpose |
| --- | --- |
| `README.md` | Release card, merge order, exclusions |
| `BUILD_EVIDENCE.md` | VS2015 Clean + Rebuild |
| `IIS_VALIDATION.md` | Login + runtime pages |
| `SQL_EVIDENCE.md` | Before/After SQL |
| `CANARY_EVIDENCE.md` | Feature canary + snapshot compare (omit if N/A) |
| `FINAL_SIGNOFF.md` | GO / NO GO |
| `CHANGELOG.md` | PR purpose / risk / UAT |
| `MERGE_CHECKLIST.md` | Squash order; never merge the UAT integration PR |
| `RELEASE_NOTES.md` | Executive summary + blockers or READY |

Never merge the temporary UAT/RC integration PR. Squash the reviewed feature PRs onto the default branch, archive the RC PR, then tag.
