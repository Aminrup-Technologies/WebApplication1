# SQL change (template)

For additive scripts under `scripts/` or evidenced DDL. Do not invent Admin catalog rows. Do not claim SP bodies that are not in git.

| Field | Value |
| --- | --- |
| Script path | `scripts/` |
| Objects (tables / procs / grants) | |
| Environment | UAT / prod (fill only from measured SQL_EVIDENCE) |

## Evidence

- Before/After: `SQL_EVIDENCE.md` in this pack (no invented result sets):
- Call sites in `.cs` (if the app already calls the object):

## Impact

- Overlay vs sidebar tables (`tlb_employee_permissions` vs `tlb_EmployeePermissions`):
- Dual-path `CanAccess` still required? (yes until a signed overlay-only pack)
- Docs: [database](../../database/README.md) page and/or [stored-procedure-call-contracts.md](../../database/stored-procedure-call-contracts.md)

## Rollback

- Reverse script path (must exist before apply if destructive):
- Data loss risk:

## Documentation updates

- [ ] Database doc or call contract
- [ ] Close or add [EVIDENCE_GAP_REGISTER](../../EVIDENCE_GAP_REGISTER.md) row
- [ ] Security / overlay doc if `tlb_permissions` changed
- [ ] Index + coverage recount

See also [SQL_EVIDENCE.md](SQL_EVIDENCE.md).
