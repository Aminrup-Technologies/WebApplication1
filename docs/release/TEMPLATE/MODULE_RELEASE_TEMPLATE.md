# Module release notes (template)

Copy into `docs/release/<version>/` or fill in the PR body. Do not invent UAT results.

| Field | Value |
| --- | --- |
| Release / PR | |
| Module (inventory name) | |
| Pages touched | |
| Baseline tag | `v2.2.3-governance-final` unless this release moves a later tag |

## Evidence

- Source files (`.aspx` / `.cs` paths):
- Behavior **Verified** in IIS / UAT (link pack files; do not invent):

## Impact

- Users / roles affected:
- Authorization (`CanAccess` codes, `IsAdmin`, Session):
- Database objects (tables/SPs) — call contract only if bodies are not in repo:
- Docs updated ([impact matrix](../../governance/documentation_impact_matrix.md)):

## Rollback

- Git: revert squash SHA / undeploy IIS:
- SQL: none / script to reverse (must exist in `scripts/`):
- Session/cache: overlay TTL 5 minutes / app-pool recycle if grants changed:

## Documentation updates

- [ ] Module page under `docs/modules/`
- [ ] Index row
- [ ] Coverage dashboard recount
- [ ] Gap register closed or new EG row

Related pack: [README.md](README.md).
