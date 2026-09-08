# Switch User denied

**Evidence:** `AuthorizationService.CanAccess("SWITCH_USER")`, `ImpersonationAudit.CanImpersonate`, UAT Admin fact J8.

## Symptom

`SwitchUser.aspx` refuses the operator, or Inspector shows `Granted=false` for `SWITCH_USER`.

## Dual-path (Verified)

Allow only if **all** of:

1. Five-key Session present
2. Not already impersonating
3. `USERTYPE == Admin` (`IsAdmin()`)
4. Overlay Direct/Group `SWITCH_USER` **or** `WorkmanSL` in `SwitchUserAuthorizedUsers`

Office Staff never gets Switch User, including via overlay or CSV.

## Common misses

| Miss | Why |
| --- | --- |
| Sidebar pack `OS-HR` | Irrelevant; J8 UAT Admin uses that pack with `User_RoleType=Admin` |
| Overlay grant without Admin | Step 3 fails |
| Admin without overlay and without CSV | Step 4 fails |
| Already switched | Nested switch forbidden |
| Expecting `ImpersonationAudit.CanImpersonate` to match UI | That helper is Admin+CSV **only** (stricter than `CanAccess`) |

Do not “fix” UAT by inserting an Admin row into `tlb_emp_roles`.

## Related

- [switch-user](../architecture/switch-user.md)
- [security/verification](../security/verification.md)
