# Impersonation restore / nested switch

**Evidence:** `SessionKeys.IsImpersonating`, `ImpersonationAudit.CanReturnFromImpersonation`, `SwitchUser.aspx.cs`.

## Nested switch

`CanAccess("SWITCH_USER")` is false while `IS_IMPERSONATING` is set. There is no stack of impersonations.

## Restore fails

Return requires the flag **and** all six `ORIGINAL_*` keys (`IsOriginalIdentityCaptured`). If capture aborted mid-switch, `ClearOriginalIdentity` runs — do not improvise Session keys.

Supported exit: banner **Return to my account** on `webmaster.Master`, then work as Admin. Landing after switch is `homepage_v2.aspx`.

## Related

- [impersonation-lifecycle](../architecture/impersonation-lifecycle.md)
- [impersonation-logout.md](impersonation-logout.md)
