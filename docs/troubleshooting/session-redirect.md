# Session redirect to login

**Evidence:** `webmaster.Master.cs` `Page_Load` `!IsPostBack` five-key check; `AuthorizationService.IsAuthenticated`.

## Symptom

Opening an ERP page sends the browser to `~/login.aspx`.

## Cause (Verified)

Any of these Session keys missing: `USERID`, `RolePermissionDB`, `UserRoleDB`, `USERNAME`, `WORKMAN`. Values are not compared. InProc Session + `timeout="20"` in `Web.config.example` (minutes). Idle UI timeout is `AutoLogoutTimeoutMinutes` (example value `20`) with redirect `AutoLogoutRedirectUrl`.

Pages on `aminrup.Master` do **not** use this five-key menu gate. Public `atsSite.Master` has no ERP Session model. Legacy `Admin.Master` `Page_Load` is empty.

## Checks

1. Cookie `ASP.NET_SessionId` present?
2. Completed MFA (or MFA disabled on the employee row) so `GrantAuthenticatedSession` ran?
3. App pool recycle / InProc wipe?
4. Direct URL to a page that also checks `REGION` (e.g. helpdesk, CSM hub) after five keys exist?

## Related

- [session-architecture](../architecture/session-architecture.md)
- [authentication-flow](../architecture/authentication-flow.md)
