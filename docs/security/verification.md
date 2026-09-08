# Security verification

**Status:** Verified procedures from code + UAT notes. Does **not** invent `tlb_emp_roles` Admin rows.

## Session gate

1. Clear cookies; open a `webmaster.Master` page.
2. Expect redirect to `~/login.aspx`.
3. Log in via `WebApplication1/Login.aspx`.
4. Confirm `USERID`, `USERNAME`, `WORKMAN`, `UserRoleDB`, `RolePermissionDB` are set.

## Authorization

1. Non-Admin: `IsAdmin()` false; `CanAccess("SWITCH_USER")` false.
2. Admin **without** overlay `SWITCH_USER` and **without** CSV: deny.
3. Admin **with** overlay Direct/Group `SWITCH_USER`: allow UI.
4. While impersonating: `CanAccess("SWITCH_USER")` false.

## UAT Admin fact (do not “fix” with SQL)

Live Admin used in prior UAT: **J8** / `ATS002112`, `User_RoleType=Admin`, `UserRoleDB=ATS-OS`, `RolePermissionDB=OS-HR`. Sidebar role strings are **not** the Admin flag.

## Overlay cache

After changing `tlb_employee_permissions`, wait **five minutes** or recycle the app pool.

## MFA

If the employee row has `MFAEnabled` on, complete Email OTP, WhatsApp OTP, or TOTP **before** `GrantAuthenticatedSession`. Missing MFA columns fail open (MFA off). There is no `MfaBypassUsers` key in `Web.config.example`.

## Related

- [mfa](../architecture/mfa.md)
- [switch-user](../architecture/switch-user.md)
- Evidence pack tagged `v2.2-security-foundation`
