# Switch User (operations)

**Status:** Verified

## Who can use it

`AuthorizationService.CanAccess("SWITCH_USER")`:

1. Logged in (five Session keys)
2. Not already impersonating
3. `User_RoleType` is **Admin**
4. Overlay Direct or Group `SWITCH_USER`, **else** `WorkmanSL` in `SwitchUserAuthorizedUsers`

Office Staff cannot Switch User.

## How to start

Open `~/bussiness/production/SwitchUser.aspx` (not legacy `Admin/`). Search Active employees, preview, confirm. Landing is `homepage_v2.aspx`. MFA does **not** re-run.

## How to stop

Banner **Return to my account** on `webmaster.Master`. Then logout as Admin from `homepage_v2` (`LogoutUserfromATS`). There is **no** `Logout.aspx`. Logout while still impersonating stamps the **target** (`LastLogout`) — see [impersonation-logout](../troubleshooting/impersonation-logout.md).

## Audit

`ImpersonationAudit.Write` on start and stop (`IMPERSONATE` / `IMPERSONATE_RETURN`). Details: [impersonation-lifecycle](../architecture/impersonation-lifecycle.md).
