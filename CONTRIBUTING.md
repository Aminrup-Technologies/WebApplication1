# Contributing to ATS ERP

Security Foundation **v2.2** (`v2.2-security-foundation`, commit `1f6c147`) is the permanent platform baseline. Read `SECURITY_FOUNDATION_BASELINE.md` and `docs/architecture/` before changing identity or authorization.

This file is mandatory. Pull requests that violate these rules must not merge.

## Authentication

ERP identity is **ASP.NET Session**, not a Forms Authentication ticket.

- Never introduce Forms Authentication (`SetAuthCookie`, ticket cookies, or a new auth pipeline). `Web.config` may still declare `<authentication mode="Forms">`; that setting is unused by login.
- Never bypass `login.ApplySessionFromEmployeeRow`. Login and Switch User both materialize identity from `tbl_Employee_Mustertable` through that method.
- Preserve MFA sequencing: password success may start Email OTP, WhatsApp OTP, or TOTP challenge/enroll **before** `GrantAuthenticatedSession`. Do not write `USERID` / `WORKMAN` until MFA completes (or MFA is disabled).
- `GrantAuthenticatedSession` owns LastLogin, LoginStatus, login audit, `ATS_SavedID`, `ATS_SHOW_HOME_LOADER`, and the redirect to `~/bussiness/production/homepage_v2.aspx`. Switch User must not call it.

Canonical page: `WebApplication1/Login.aspx` (`login` class).

## Authorization

- All **new** permission checks must call `AuthorizationService` (`IsAuthenticated`, `IsAdmin`, `CanAccess` / `HasPermission`).
- Never add a new `WORKMAN == "J8"` (or other named WorkmanSL) gate. Wrap existing lists only inside `AuthorizationService`.
- Never use `UserRoleDB` for allow/deny. Presence of the Session key proves login; its **value** is not runtime authority.
- Never use `tlb_EmployeePermissions` as page authorization. That table drives sidebar **visibility only**. Pages remain URL-reachable.

`AuthorizationService.IsAdmin()` is `USERTYPE == "Admin"` only. **Office Staff** is module-local (JOB360 / attendance). It is never Switch User.

See `docs/architecture/authorization-flow.md`.

## Overlay

Platform permissions live beside the legacy menu matrix. Names differ on purpose.

| Table | Role |
| --- | --- |
| `tlb_permissions` | Catalog of platform codes (`SWITCH_USER`, `USER_ADMIN`, …) |
| `tlb_employee_permissions` | Direct employee grants `(WorkmanSL, PermissionId)` |
| `tlb_permission_groups` / `tlb_group_permissions` / `tlb_employee_group` | Group grants |
| `tlb_EmployeePermissions` | Legacy sidebar chrome — **not** overlay |

- New platform permissions belong in `tlb_permissions`.
- Employee grants belong in `tlb_employee_permissions` (or a group).
- Preserve dual-path behavior during migrations: overlay Direct/Group first, then config CSV / hardcoded fallback, until a later CR retires the fallback with evidence.
- Overlay cache TTL is 5 minutes (`PermissionRepository`). Recycle IIS after grant SQL.

See `docs/architecture/permission-overlay.md`.

## Impersonation

- Nested Switch User is forbidden (`IS_IMPERSONATING` denies `CanAccess("SWITCH_USER")`).
- Capture `ORIGINAL_*` **before** applying the target row. Return requires `IsOriginalIdentityCaptured`.
- Switch User reuses `ApplySessionFromEmployeeRow` only. Do not invent a second Session builder.

See `docs/architecture/impersonation-lifecycle.md`.

## Releases

Every ERP release copies `docs/release/TEMPLATE/` to `docs/release/<version>/` and fills it during UAT. Do not invent IIS or SQL results. Sign **GO** or **NO GO** only.

Required pack files:

- Build evidence (`BUILD_EVIDENCE.md`)
- IIS validation (`IIS_VALIDATION.md`)
- SQL evidence (`SQL_EVIDENCE.md`)
- Canary validation (`CANARY_EVIDENCE.md`, omit only when N/A)
- Final signoff (`FINAL_SIGNOFF.md`)

Also complete `README.md`, `CHANGELOG.md`, `MERGE_CHECKLIST.md`, and `RELEASE_NOTES.md` from the template. Never squash-merge a temporary UAT integration PR (the v2.2 pattern: archive it after the reviewed feature PRs land).

See `docs/RELEASE_GOVERNANCE.md`.

## Language and compile

Production C# on this app is **C# 6** (`langversion` 6). Do not introduce newer syntax. Keep `WebApplication1.csproj` Compile items unique.

## Documentation

Every PR that changes ERP pages, `App_Code`, SQL scripts, or AppSettings must follow `docs/governance/documentation_impact_matrix.md` and complete the **Documentation Impact** checklist in `.github/pull_request_template.md`. Update `docs/ERP_DOCUMENTATION_INDEX.md` when adding a document. Unknown schema goes in `docs/EVIDENCE_GAP_REGISTER.md` — never invent tables or stored-procedure bodies.

Charter: `docs/ERP_DOCUMENTATION_CHARTER.md`. Process: `docs/governance/documentation_governance.md`. Hub: `docs/ERP_DOCUMENTATION_INDEX.md`.
