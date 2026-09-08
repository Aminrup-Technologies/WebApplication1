# Hotfix (template)

Small production fix. Still documentation-only impact analysis. Do not skip evidence. Do not invent IIS/SQL results.

| Field | Value |
| --- | --- |
| Incident / JOBID / ticket | |
| PR | |
| Severity | |
| Pages / App_Code | |

## Evidence

- Failure (log, UAT pack, or cited code path):
- Fix verification (IIS or test named in BUILD/IIS evidence):

## Impact

- Session / authz / JOBID predicates (frozen `v2.1.0-jobid-remediation` unless this hotfix is a JOBID CR):
- Config keys:

## Rollback

- Revert SHA:
- Data repair (script or none):

## Documentation updates

- [ ] Module or troubleshooting runbook (preferred for operator-visible hotfixes)
- [ ] Impact matrix rows that apply
- [ ] Release timeline if a hotfix tag is cut
- [ ] Gap register if the bug revealed missing evidence

Copy the rest of [TEMPLATE/README.md](README.md) files if this hotfix is shipped as its own `docs/release/<version>/` pack.
