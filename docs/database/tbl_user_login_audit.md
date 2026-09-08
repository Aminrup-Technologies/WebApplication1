# tbl_UserLoginAudit

**Baseline:** `v2.2.3-governance-final` (`495fc68`)  
**Purpose:** Login and impersonation event log.  
**Evidence:** INSERT lists in `Login.aspx.cs` and `ImpersonationAudit.Write`; UPDATE `LogoutTime` on homepage / webmaster.

## Column summary (evidenced)

| Column | Evidence |
| --- | --- |
| `LoginID` | INSERT |
| `WorkmanSL` | INSERT (nullable in impersonation when empty → DBNull) |
| `LoginTime` | `GETDATE()` on INSERT |
| `LoginResult` | Login: `SUCCESS`, `SUCCESS_MFA`, `FAILED`, `BLOCKED`, `MFA_*`; Impersonation: `IMPERSONATE`, `IMPERSONATE_RETURN` |
| `FailureReason` | Login failure text; impersonation `Corr=...;TargetUser=...;Outcome=...` |
| `IPAddress`, `UserAgent`, `SessionID` | INSERT |
| `LogoutTime` | UPDATE WHERE `SessionID=@SID AND LogoutTime IS NULL` |

No CREATE TABLE in repo. Extra columns may exist (**Inference**).

## Relationships

Logical link to `tbl_Employee_Mustertable.LoginID` / `WorkmanSL`. Not evidenced as FK.

## CRUD

| Op | Where |
| --- | --- |
| INSERT | `GrantAuthenticatedSession`; `InsertLoginAudit`; `ImpersonationAudit.Write` |
| UPDATE | `homepage_v2.LogoutUser`, `homepage.aspx.cs`, `webmaster.Master.cs` (logout stamp) |

## Security

Audit `LoginID`/`WorkmanSL` on impersonation rows is the **admin** (ORIGINAL_*), not the target. Pair IMPERSONATE / IMPERSONATE_RETURN via `Corr=` GUID in `FailureReason`. Logout while impersonating stamps the current Session (target). See [impersonation-lifecycle](../architecture/impersonation-lifecycle.md).
