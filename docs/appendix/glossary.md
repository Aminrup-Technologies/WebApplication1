# Glossary

**Baseline:** `v2.2.3-governance-final`. Terms as used in ATS ERP code.

| Term | Meaning |
| --- | --- |
| `WorkmanSL` | Business employee id; Session `WORKMAN` |
| `LoginID` | Login name; Session `USERID` |
| `User_RoleType` | Muster text; Session `USERTYPE`; `IsAdmin()` is this equal to `Admin` |
| `UserRoleDB` | `tlb_emp_roles.EmpType_Value`; Session presence only — **not** privilege |
| `RolePermissionDB` | Sidebar profile key → `tlb_EmployeePermissions` |
| `tlb_EmployeePermissions` | Legacy **menu visibility** |
| `tlb_employee_permissions` | Overlay **grants** (different table) |
| `tlb_permissions` | Overlay catalog of codes (`SWITCH_USER`, …) |
| JOBID | Encoded/plain job identifier; V2 query uses `JobIdCodec` |
| `EntryExit` | JOB header state: `Created` / `Entry` / `Exit` |
| `MasterStatusCode` | `'1'` create / `'3'` IN / `'4'` closed / `'5'` approved |
| Office Staff | `USERTYPE` string; JOB360 / attendance module exception; **never** Switch User |
| Switch User | Impersonation via `SwitchUser.aspx`; not Forms auth |
| InProc Session | `sessionState mode="InProc"`; identity is Session, not a Forms ticket |
| Overlay cache | `PermissionRepository` 5-minute `HttpRuntime.Cache` |
| `DESG` | Homepage-only Session key for CSM KPI chrome |
| `USTATE` | Homepage-only Session key (state code, including `PI` = pan-India) |
| `STATE` | `SessionKeys.UserState` from login `WorkState` |

## Related

- [repository-inventory.md](repository-inventory.md)
- [authorization-flow](../architecture/authorization-flow.md)
