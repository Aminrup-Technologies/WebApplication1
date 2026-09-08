# tbl_Employee_Mustertable

**Baseline:** `v2.2.3-governance-final` (`495fc68`)  
**Purpose:** Employee master. Login identity, geography, role strings, and MFA flags are read from this table.  
**SP body:** `SP_InsertInto_EmployeeMusterTable` is **not in this repo**. Columns below are from call sites and ALTER scripts only.

## Purpose

Single person record keyed in practice by `WorkmanSL` (business id) and `LoginID` (login). `ApplySessionFromEmployeeRow` materializes Session from a row. Registration inserts via the stored procedure.

## Column summary (evidenced)

| Column | Evidence | Role |
| --- | --- | --- |
| `Id` | `COUNT(Id)` duplicate check in `emp_registration.aspx.cs` | Surrogate (**Verified** used) |
| `LoginID` | Login SELECT; SP `@LoginID` | Login name → Session `USERID` |
| `LoginPassword` | Login compare; SP hashed `@LoginPassword` | Password hash |
| `LoginPassword_Plain` | SP `@LoginPassword_Plain` | Plain password at insert — **Verified** parameter |
| `WorkStatus` | Login; Switch User Active filter; SP `@WorkStatus='Active'` | Must be `Active` to log in / switch to |
| `DOR`, `PasswordExpiry` | Login SELECT | Read at login; unused in Session builder |
| `WorkmanSL` | Login; SP; uniqueness check | Session `WORKMAN` |
| `FirstName`, `FullName` | Login; SP | Session `USERFNAME` / `USERNAME` |
| `MiddleName`, `LastName`, `Fathername`, `BloodGroup` | SP params | Master data |
| `Email`, `MobileNo` | Login SELECT; MFA | MFA delivery |
| `User_RoleType` | Login; SP text from `tlb_emp_roles.Employee_Type` | Session `USERTYPE` |
| `UserRoleDB` | Login; SP value from `EmpType_Value` | Session presence; **not** privilege |
| `RolePermissionDB` | Login; SP | Menu key + Session presence |
| `Role_Permission` | SP text | Catalog label; **not** sessioned |
| `WorkRegion`, `WorkState`, `WorkCompany`, `WorkSite`, `Worksite_Code` | Login SELECT; SP | Session geo/site |
| `SkillDesignation`, `SkillCategory` (+ `*DB` SP params) | Login / SP | Session `U_DESG` / `U_SKILL` |
| `PrfPicFile` | Login | Session photo |
| `Qualification`, `DOB`, `DOJ` | SP | Master data |
| `WorkHours`, `OTFactor` | SP | Payroll-related master |
| `SafetyPassNo`, `SafetyPassExpiry`, `GatePassNo`, `GatePassExpiry`, `PVExpiry` | SP | Pass fields |
| `UANNo`, `ESICNo`, `Payment_Bank`, `Payment_Account`, `Payment_IFSC`, `BankBranch` | SP | Statutory / bank |
| `RegistrationType` | SP `'Single'` | Insert path |
| `Registered_ByName`, `Registered_ByWRK` | SP; fallback Workman `"J8"` | Audit stamp, not a gate |
| `LoginStatus`, `LastLogin`, `LastLogout` | SP; `GrantAuthenticatedSession`; homepage logout | Login bookkeeping |
| `MFAEnabled`, `MFAMethod`, `MFAEnforcedOn`, `MFAEnforcedBy`, `MFALastVerified` | `add_employee_mfa_columns.sql` | Email/WhatsApp MFA |
| `MFATotpSecret`, `MFATotpEnrolled` | `add_employee_mfa_totp_columns.sql` | Authenticator |

**Not claimed:** full CREATE TABLE, indexes, or uniqueness besides the app `WorkmanSL` COUNT check. **Inference:** the SP maps `@` names to like-named columns.

## Relationships

| Related | How |
| --- | --- |
| `tlb_emp_roles` | `User_RoleType` / `UserRoleDB` copied at registration |
| `tlb_emp_roles_permission` | `Role_Permission` / `RolePermissionDB` |
| `tlb_employee_permissions` | Overlay grants by `WorkmanSL` — separate |
| `tbl_UserLoginAudit` | LoginID / WorkmanSL on login and impersonation events |

## Indexes

Unknown in-repo except overlay table indexes. Do not invent.

## CRUD locations

| Op | Where |
| --- | --- |
| INSERT | `emp_registration.aspx.cs` → `SP_InsertInto_EmployeeMusterTable`; bulk sibling not fully traced |
| SELECT | `login.FetchEmployeeRowByLoginId`; Switch User search; Inspector/Analyzer |
| UPDATE | `GrantAuthenticatedSession` LastLogin/LoginStatus; MFA LastVerified / TOTP enroll; homepage LastLogout; employee edit pages `viewupdate_empmustertabledata*.aspx.cs` |

## Security implications

- Source of `USERTYPE` (Admin vs Office Staff).
- Overlay does not replace this table.
- Plain password parameter is sensitive (**Recommendation:** do not expand).
- `WorkStatus` is the login/switch allow filter.

## Related

[Employee registration](../modules/employee-registration.md), [authentication-flow](../architecture/authentication-flow.md), [mfa](../architecture/mfa.md).
