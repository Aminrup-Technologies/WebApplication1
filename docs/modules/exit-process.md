# Exit process

**Baseline:** `v2.2.3-governance-final` (`495fc68`)  
**Status:** Verified — **no dedicated module in this repo**.

There is **no** `exit_process.aspx`, `view_exit_process.aspx`, `EXIT_OVERRIDE`, or `EXIT_PROCESS_MODULE.md`. A glob of `WebApplication1/` found no ExitProcess / emp_exit / separation pages.

## Closest related behavior

| Topic | Evidence |
| --- | --- |
| Employment state | `tbl_Employee_Mustertable.WorkStatus` — login and Switch User require `Active` |
| Session logout | `homepage_v2.aspx.cs` `LogoutUserfromATS` / `LogoutUser` — stamps `LastLogout` / `LoginStatus=0` and `tbl_UserLoginAudit.LogoutTime`; **not** an HR exit workflow |
| Impersonation stop | Banner restore on `webmaster.Master` — see [switch-user.md](../architecture/switch-user.md) |

Do not invent an HR exit pipeline. If a later CR adds pages, document them from those files.

## Related

- [employee-registration.md](employee-registration.md)
- [authentication-flow](../architecture/authentication-flow.md)
