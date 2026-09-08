# Logout while impersonating

**Evidence:** `homepage_v2.aspx.cs` `LogoutUserfromATS` / `LogoutUser`; [impersonation-lifecycle.md](../architecture/impersonation-lifecycle.md).

## Symptom

Admin was viewing as another employee, clicked homepage logout, and the **target** muster row got `LastLogout` / `LoginStatus=0`.

## Cause (Verified)

Logout is **not** impersonation-aware. It updates `tbl_Employee_Mustertable` and `tbl_UserLoginAudit` for the **current** `USERID` / `SessionID`. There is no `Logout.aspx` in this tree.

v2.2 accepted this. Supported sequence: **Return to my account**, then logout as Admin.

## Related

- [switch-user-ops](../administration/switch-user-ops.md)
