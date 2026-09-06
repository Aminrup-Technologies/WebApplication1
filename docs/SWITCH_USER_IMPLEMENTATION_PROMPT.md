# Cursor Agent Prompt — Implement Internal Switch User

**Mode:** Implementation. Execute this prompt only after `docs/AUTH_SESSION_ARCHITECTURE_AUDIT.md` is accepted.  
**Do not invent a second login path.** Architecture is proven there.  
**Output:** unified diffs only. Documentation header on every modified file.

---

## Objective

Add an internal admin-only Switch User page that impersonates an existing employee **without their password**, by rebuilding Session exactly as `GrantAuthenticatedSession` does, then landing on `homepage_v2.aspx`. Preserve every existing page-level Session check. Do not create Forms Authentication cookies. Do not bypass `webmaster.Master` or `USERTYPE` / WorkmanSL allowlists already in the app.

Search targets: `WorkmanSL`, `FirstName`, `FullName`.

---

## Non-negotiable rules

- No placeholder code.
- No wrapper facade that re-implements login SQL with a different column list.
- No `FormsAuthentication.SetAuthCookie`.
- No `Session.Abandon()` on switch (InProc session must stay; ORIG snapshot lives in it).
- No string-concatenated SQL (do not copy `searchemployee.aspx.cs` 67–82).
- No nested impersonation.
- Do not update target `LoginStatus` / `LastLogin` as a real login.
- Do not write `ATS_SavedID` for the target.
- Skip MFA (admin is already authenticated).
- Still require target `WorkStatus = 'Active'`.
- Page authorization on **every** request, not only `!IsPostBack`.
- Minimal footprint. DRY.

---

## Proven facts (do not rediscover)

Canonical login: `WebApplication1/Login.aspx.cs` class `login`.  
Identity constructor: `GrantAuthenticatedSession` lines 725–789.  
Login SELECT: `FetchLoginUser` lines 129–138, table `tbl_Employee_Mustertable`.  
Landing: `Response.Redirect("~/bussiness/production/homepage_v2.aspx", false)` line 787.  
Homepage extras: `USTATE`, `SKIL`, `DESG` in `homepage_v2.aspx.cs` 290–302.  
Master gate: `webmaster.Master.cs` 24–26 (`USERID`, `RolePermissionDB`, `UserRoleDB`, `USERNAME`, `WORKMAN`).  
`USERTYPE == "Admin"` is the role string. Office Staff is **not** allowed to switch users (`job_360_view.aspx.cs` 51–53 treats Office Staff as admin — do not copy that).  
Allowlist pattern: `PayrollAuthorizedUsers` in `Web.config.example` line 16, consumed in `viewupdate_empmustertabledata_v2.aspx.cs` 85–104.  
Audit table: `tbl_UserLoginAudit` insert shape at `Login.aspx.cs` 981–984.  
Full architecture: `docs/AUTH_SESSION_ARCHITECTURE_AUDIT.md`.

---

## File plan

### 1. `WebApplication1/bussiness/production/SessionKeys.cs`

Add constants only (no logic):

- `OrigUserID = "ORIG_USERID"`
- `OrigWorkmanSL = "ORIG_WORKMAN"`
- `OrigUserName = "ORIG_USERNAME"`
- `OrigUserType = "ORIG_USERTYPE"`
- `Impersonating = "IMPERSONATING"`  // bool

Header: WHEN / WHY / WHAT.

### 2. Extract session apply — `WebApplication1/bussiness/production/AuthenticatedSession.cs` (new)

This is the **login session-assignment path**, not a wrapper around it. Move the Session writes from `GrantAuthenticatedSession` here so login and Switch User call the same method.

Static class `AuthenticatedSession` in namespace `WebApplication1.bussiness.production`.

Required methods:

```csharp
public static DataTable FetchEmployeeForAuth(DB_Utility_OH4Y dbcl, string loginId)
// Identical SQL and parameters to Login.aspx.cs FetchLoginUser (lines 129-138).

public static DataTable FetchEmployeeForAuthByWorkman(DB_Utility_OH4Y dbcl, string workmanSL)
// Same column list. WHERE WorkmanSL = @WorkmanSL AND WorkStatus = 'Active'. TOP 1.

public static DataTable SearchEmployees(DB_Utility_OH4Y dbcl, string workmanSL, string firstName, string fullName)
// Parameterized. Any combination of the three filters. At least one required.
// SELECT TOP 50 WorkmanSL, FirstName, FullName, LoginID, WorkStatus, User_RoleType, WorkRegion, WorkCompany
// FROM tbl_Employee_Mustertable
// WHERE (@WorkmanSL empty OR WorkmanSL = @WorkmanSL)
//   AND (@FirstName empty OR FirstName LIKE @FirstNamePattern)
//   AND (@FullName empty OR FullName LIKE @FullNamePattern)
// ORDER BY FullName. Use parameterized LIKE ('%' + value + '%') in SQL parameters, not string concat.

public static void ApplyFromEmployeeRow(HttpSessionState session, DataRow row, string profilePhotoRoot)
// Copy Login.aspx.cs 767-785 exactly (USERID through User_Photo file-exists rule).
// Additionally set:
//   session["USTATE"] = row["WorkState"]
//   session["SKIL"] = row["SkillCategory"]
//   session["DESG"] = row["SkillDesignation"]
// Do not set ATS_SavedID. Do not write LastLogin. Do not insert SUCCESS audit.

public static bool IsImpersonating(HttpSessionState session)
public static void SnapshotOriginIfNeeded(HttpSessionState session) // no-op if ORIG_WORKMAN already set
public static void ClearOrigin(HttpSessionState session)
public static bool IsSwitchUserAdmin(HttpSessionState session)
// USERTYPE equals "Admin" (ordinal, case-insensitive) AND WorkmanSL (or ORIG_WORKMAN if impersonating)
// is in ConfigurationManager.AppSettings["SwitchUserAuthorizedUsers"] comma list.
// Compare trimmed uppercase, same pattern as viewupdate_empmustertabledata_v2.aspx.cs 87-94.
```

Photo rule must match `Login.aspx.cs` 782–785 (`Directory.Exists` + `File.Exists` else `"No_Image.jpg"`).  
Root path: `HostingEnvironment.MapPath("~/erp_images/ProfilePhoto")`.

### 3. `WebApplication1/Login.aspx.cs`

Documentation header on the edit.

- Replace body of `FetchLoginUser` with `AuthenticatedSession.FetchEmployeeForAuth(dbcl, id)`.
- In `GrantAuthenticatedSession`, keep: audit insert, `LastLogin`/`LoginStatus=1`, optional `MFALastVerified`, `ATS_SavedID`, redirect.
- Replace Session assignments (lines 767–785) with `AuthenticatedSession.ApplyFromEmployeeRow(Session, row, rootFolder)`.
- Login still owns password, MFA, and post-auth DB writes.

### 4. New pages

`WebApplication1/bussiness/production/SwitchUser.aspx`  
`WebApplication1/bussiness/production/SwitchUser.aspx.cs`  
`WebApplication1/bussiness/production/SwitchUser.aspx.designer.cs` as generated by the aspx controls.

Master: `~/bussiness/production/webmaster.Master` (same as other ERP pages).

`SwitchUser.aspx.cs` responsibilities:

**Authorize (Page_Load, every request including PostBack):**

- If not `AuthenticatedSession.IsSwitchUserAdmin(Session)` → `Response.Redirect("~/login.aspx", false); CompleteRequest(); return;`
- Use **original** workman when `IMPERSONATING` is true (allowlist the admin, not the target).

**UI:**

- Banner: current `USERNAME` / `WORKMAN`. If impersonating, also show `ORIG_USERNAME` / `ORIG_WORKMAN`.
- Search fields: WorkmanSL, FirstName, FullName. Button Search.
- Grid of matches: WorkmanSL, FirstName, FullName, LoginID, WorkStatus, User_RoleType, WorkRegion, WorkCompany. Button Switch on Active rows only. Disable Switch on the currently impersonated user and on the original admin’s own row.
- Button Return to Original User (visible only when `IMPERSONATING`).
- No password field.

**Switch handler:**

1. Re-check `IsSwitchUserAdmin`.
2. Reject if already impersonating.
3. `FetchEmployeeForAuthByWorkman`. If 0 rows or `WorkStatus != "Active"` → notify and return.
4. `SnapshotOriginIfNeeded`.
5. `ApplyFromEmployeeRow`.
6. `Session[SessionKeys.Impersonating] = true`.
7. `Session["Changer"] = null`.
8. Remove MFA keys if any remain (same list as `ClearMfaSession` in Login.aspx.cs 710–722). `Session.Remove` each; do not Abandon.
9. Insert `tbl_UserLoginAudit`: LoginID=target LoginID, WorkmanSL=target WorkmanSL, LoginResult=`IMPERSONATE`, FailureReason=`Admin={origWorkman};Target={targetWorkman}`, IP, UserAgent, SessionID. Reuse the insert shape from `InsertLoginAudit` (`Login.aspx.cs` 981–984). Do **not** update `LastLogin`/`LoginStatus`.
10. `Response.Redirect("~/bussiness/production/homepage_v2.aspx", false); CompleteRequest();`

**Return handler:**

1. Require `IMPERSONATING` and `ORIG_USERID`.
2. Fetch original via `FetchEmployeeForAuth(ORIG_USERID)`. If missing/inactive, still restore snapshot keys if fetch fails? **Fail closed:** notify, do not leave a half-switched session. If fetch fails, do not Clear origin; keep impersonating and show error.
3. Apply original row.
4. Audit `IMPERSONATE_RETURN` with FailureReason `Admin={orig};From={target}`.
5. `ClearOrigin`; `Impersonating = false`; `Changer = null`.
6. Redirect homepage_v2.

### 5. `webmaster.Master` + `webmaster.Master.cs`

- If `Session[Impersonating]` is true, show a persistent bar: `Viewing as {USERNAME} ({WORKMAN}). Original: {ORIG_USERNAME} ({ORIG_WORKMAN}).` plus a server button `Return to Original User` that posts to a master handler **or** hyperlink to `SwitchUser.aspx` (prefer hyperlink to SwitchUser.aspx to keep master thin).
- Add a menu child under an existing admin parent only if a permission row already exists for it. **Do not invent tlb_EmployeePermissions rows in C#.** If no permission key exists, do not hide the page behind a new unseeded ChildKey; link from the impersonation bar and/or a hardcoded visible link shown only when `IsSwitchUserAdmin`. Direct URL still enforces allowlist.
- Do not change the five-key GET gate.

### 6. `homepage_v2.aspx.cs`

Password expiry block lines 404–420 currently redirects to `emp_pwdchange.aspx` when expiry ≤ 15 days.  
If `Session[Impersonating]` is true, **skip that redirect** (still show the PNotify). Do not skip the rest of dashboard binding.  
Do not skip document/contact modals unless they would write the target’s data without admin intent; if a modal save would UPDATE the target, leave it (admin is acting as that user) but the password-change redirect is a trap — skip only that redirect.

### 7. `Web.config.example`

Under `appSettings`:

```xml
<add key="SwitchUserAuthorizedUsers" value="J8" />
```

Same comma-separated WorkmanSL pattern as `PayrollAuthorizedUsers`. Do not commit secrets. Do not put this in gitignored `Web.config` from this CR if that file is absent.

---

## Audit log

Every switch and every return must insert `tbl_UserLoginAudit` as specified. No new table unless `tbl_UserLoginAudit` cannot store both identities — it can, via `FailureReason` as already used for non-failure metadata (`Login.aspx.cs` 981–984). Do not add columns in this CR.

---

## Markup constraints

- Match existing ERP pages: webmaster.Master, Bootstrap/gentelella controls already used on `searchemployee.aspx` / `manage_rollsaccess.aspx`.
- Keep SwitchUser.aspx small: search form, grid, messages.
- `Inherits="WebApplication1.bussiness.production.SwitchUser"` (or `switchuser` if you must match an existing lowercase pattern — prefer `SwitchUser` and keep aspx Inherits in sync).

---

## Tests / verification (no browser ASPX host assumed)

1. Compile: `Login.aspx.cs` still compiles after FetchLoginUser / GrantAuthenticatedSession extraction.
2. Grep: `GrantAuthenticatedSession` Session assignments exist in **one** place (`AuthenticatedSession.ApplyFromEmployeeRow`) plus login cookie/audit/redirect.
3. Grep: no `SetAuthCookie` added.
4. Grep: SwitchUser SQL uses `SqlParameter`, not `'" + txt`.
5. Confirm `IsSwitchUserAdmin` uses ORIG workman while impersonating so the target cannot open SwitchUser.
6. Confirm Return restores `USERID`/`WORKMAN`/`RolePermissionDB` from the original row, not from leftover target keys.

If MSBuild is available, build `WebApplication1`. If not, say so.

---

## Out of scope

- Do not implement Switch User on `Loginold.aspx` / Admin CMS.
- Do not change MFA.
- Do not seed `tlb_EmployeePermissions`.
- Do not “fix” `Session["FullName"]` / `Session["UserName"]` typos on other pages.
- Do not add Global.asax.

---

## Done when

- Unified diffs cover the files above.
- Login still grants Session only through `ApplyFromEmployeeRow`.
- Switch User is admin-allowlist-only, searchable by WorkmanSL / FirstName / FullName, audited, reversible, and does not set an auth cookie.
- `docs/AUTH_SESSION_ARCHITECTURE_AUDIT.md` is not rewritten except a one-line pointer at the top if you must: “Implementation prompt: docs/SWITCH_USER_IMPLEMENTATION_PROMPT.md”.
