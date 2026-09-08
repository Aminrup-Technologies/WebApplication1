# Security change (template)

Use with [`security_change_matrix.md`](../../governance/security_change_matrix.md). New checks must call `AuthorizationService`. No new WorkmanSL page gates.

| Field | Value |
| --- | --- |
| PR | |
| Paths | Login / SessionKeys / AuthorizationService / PermissionRepository / SwitchUser / overlay SQL |
| Risk (matrix) | Critical / High / Medium |

## Evidence

- Inspector / Analyzer / canary (link `CANARY_EVIDENCE.md` or N/A):
- Dual-path still in force for `SWITCH_USER`? 

## Impact

- `IsAdmin()` / `CanAccess` codes added or changed:
- MFA sequencing preserved (no `USERID` before grant)?
- Impersonation: nested switch still denied; ORIGINAL_* capture?

## Rollback

- Revert squash; overlay SQL reverse script:
- Recycle IIS (overlay cache 5 minutes):

## Documentation updates

- [ ] `docs/architecture/` flow that matches the change
- [ ] `docs/security/` threat or verification if residual risk changed
- [ ] Administration / troubleshooting runbook if operator steps changed
- [ ] Appendix glossary / configuration if a new key or term
- [ ] Index + coverage

See [CONTRIBUTING.md](../../../CONTRIBUTING.md) and the pack `CHANGELOG.md` after copying this folder to `docs/release/<version>/`.
