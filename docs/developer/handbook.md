# Developer handbook

**Baseline:** `v2.2.3-governance-final` (`495fc68`)  
**This page is a map.** Canonical rules stay in [`CONTRIBUTING.md`](../../CONTRIBUTING.md). Do not fork them.

## Where to start

| Task | Read |
| --- | --- |
| Identity / Session | [authentication-flow](../architecture/authentication-flow.md), `Login.aspx.cs` `ApplySessionFromEmployeeRow` |
| New permission check | [authorization-flow](../architecture/authorization-flow.md), `AuthorizationService.CanAccess` |
| Overlay SQL | [permission-overlay](../architecture/permission-overlay.md), `scripts/create_permission_overlay.sql` |
| Switch User | [switch-user](../architecture/switch-user.md), [impersonation-lifecycle](../architecture/impersonation-lifecycle.md) |
| JOBID writes | [jobid.md](../modules/jobid.md), `JobStatusConstants`, `v2.1.0-jobid-remediation` |
| PR template | `.github/pull_request_template.md` |
| Who reviews | `.github/CODEOWNERS`, [security_change_matrix](../governance/security_change_matrix.md) |
| Cursor / agent | [cursor_governance](../governance/cursor_governance.md) |

## Branching

Default / release line: `Jul_to_Sep_2026_Suport_N_Dev_Works`.  
Documentation program: `feature/erp-documentation-program`.  
Security v2.3 overlay CRUD: **do not cut** until asked; then branch from default / `v2.2.3-governance-final`.

Squash-land reviewed feature PRs onto default. Do **not** squash-merge a temporary UAT integration PR (v2.2 pattern: archive after features land). Do **not** merge PR **#103**.

## Language and project file

- C# **6** (`/langversion:6` in `Web.config.example` codedom).
- Keep `WebApplication1.csproj` **Compile** items unique (duplicate entries fail the build).
- ASP.NET WebForms, .NET Framework 4.8, InProc Session.

## Session keys

Add identity keys only on `SessionKeys.cs`. Presence of `USERID`, `RolePermissionDB`, `UserRoleDB`, `USERNAME`, `WORKMAN` is login proof on `webmaster.Master`. Values of `*DB` keys are not privilege.

Homepage also sets `DESG` and `USTATE` (**Verified** `homepage_v2.aspx.cs`). Those are **not** in `SessionKeys`. Pages that read them assume the user hit homepage after login.

## Authorization checklist (new code)

1. Call `AuthorizationService` — never a new `WORKMAN == "J8"` on a page.
2. Never allow/deny on `UserRoleDB` or `tlb_EmployeePermissions`.
3. `IsAdmin()` is `USERTYPE == Admin` only. Office Staff is JOB360 / attendance only.
4. Overlay cache TTL is 5 minutes; recycle IIS after grant SQL.
5. Unique `.csproj` Compile include for any new `.cs`.

## Releases

Copy `docs/release/TEMPLATE/` to `docs/release/<version>/`. Fill IIS/SQL/canary from **measured** evidence. Sign GO or NO GO only.

## Rollback

Revert the squash commit on default (tags `v2.2-security-foundation` … `v2.2.3-governance-final` mark known-good docs/runtime baselines). Feature branches are **archived, not deleted**.

## Related

- [MAINTENANCE_GUIDELINES.md](../MAINTENANCE_GUIDELINES.md)
- [troubleshooting](../troubleshooting/README.md)
