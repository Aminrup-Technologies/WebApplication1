# Security change control

**Status:** Verified pointers. Do not duplicate the governance matrix.

## Canonical governance

| Doc | Use |
|-----|-----|
| [security_change_matrix.md](../governance/security_change_matrix.md) | What needs dual review |
| [CODEOWNERS](../../.github/CODEOWNERS) | Required reviewers for listed paths |
| [CONTRIBUTING.md](../../CONTRIBUTING.md) | Branch / PR rules |
| [SECURITY_FOUNDATION_BASELINE.md](../../SECURITY_FOUNDATION_BASELINE.md) | Frozen runtime facts |

## Rule for this documentation program

Documentation PRs **must not** change `.cs`, `.aspx`, or `.sql` unless a separate, explicitly requested runtime PR exists.

## v2.3 (not started)

Overlay CRUD / groups UI must branch from default / `v2.2.3-governance-final` and **reuse** `AuthorizationService` / `PermissionRepository`. Do not cut that branch until asked.
