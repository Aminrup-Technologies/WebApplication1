# Cursor governance — Security Foundation

**Baseline:** Security Foundation v2.2.1 (`v2.2.1-governance`, `19c286e`).  
**Audience:** Cursor agents and humans editing this repository.  
**Normative rules:** `CONTRIBUTING.md`, `SECURITY_FOUNDATION_BASELINE.md`, `docs/architecture/`.

This file is mandatory for future Cursor runs. Product behavior is frozen at v2.2. Do not "improve" identity or authorization by inventing a new model.

## Mandatory behavior

Future Cursor agents **must**:

- **Never bypass `AuthorizationService`.** New allow/deny goes through `IsAuthenticated`, `IsAdmin`, or `CanAccess` / `HasPermission`. Do not add page-local `IsAdmin()` helpers that compare Session strings.
- **Never introduce new `WORKMAN` gates.** No `Session["WORKMAN"] == "J8"` (or J4, A84, …) on a page. Existing hardcoded lists stay inside `AuthorizationService.IsWorkmanOnHardcodedList` until a CR retires them with overlay evidence.
- **Preserve Session architecture.** Identity is ASP.NET InProc Session, not Forms tickets. Login and Switch User share `login.ApplySessionFromEmployeeRow`. Do not add a second Session builder. Do not write `USERID` during an MFA challenge. Switch User must not call `GrantAuthenticatedSession`.
- **Preserve overlay precedence.** Session → `USERTYPE` → module exception → overlay Direct/Group → config CSV → hardcoded → deny. Empty overlay fails closed; SWITCH_USER still falls back to `SwitchUserAuthorizedUsers` while dual-path is in force. `tlb_employee_permissions` is not `tlb_EmployeePermissions`.
- **Use unified diffs.** Prefer `git diff` / patch-style edits. Do not rewrite unrelated files. Do not reformat production C# to modern syntax (this app is **C# 6**).
- **Preserve release evidence.** Never delete or invent results under `docs/release/`. Copy `docs/release/TEMPLATE/` for a new version. Never squash-merge a temporary UAT integration PR (the v2.2 `#103` pattern).

Also:

- `UserRoleDB` is presence-only. Do not compare its value for privilege.
- `AuthorizationService.IsAdmin()` is `USERTYPE == "Admin"` only. Office Staff is module-local (JOB360 / attendance), never Switch User.
- Do not modify `.aspx`, `.cs`, or SQL **logic** in a governance-only PR.
- Keep `WebApplication1.csproj` Compile items unique.

## CODEOWNERS

`.github/CODEOWNERS` requests `@Aminrup-Technologies` on security-core, architecture, and release paths. Repository owners may later replace that owner with a dedicated team. Agents must not remove CODEOWNERS patterns to avoid review.

## Diff discipline

| Do | Do not |
| --- | --- |
| Small unified diffs against the stated base tag/branch | Drive-by refactors adjacent to the task |
| One concern per PR (governance vs runtime vs SQL apply) | Mix overlay SQL apply with Session key edits |
| Quote existing constants (`AuthorizationFeatureCodes`, `SessionKeys`) | Duplicate string keys (`"SWITCH_USER"`, `"USERID"`) in new code |
| Read `docs/architecture/` before changing Login / Switch User / AuthorizationService | Re-open the hybrid model "to simplify it" |

## Release checklist

Every ERP release (agent or human):

1. Copy `docs/release/TEMPLATE/` → `docs/release/<version>/`.
2. Fill **build evidence** (`BUILD_EVIDENCE.md`) — VS2015 Clean + Rebuild; do not invent logs.
3. Fill **IIS validation** (`IIS_VALIDATION.md`) — login + runtime pages on the target IIS.
4. Fill **SQL evidence** (`SQL_EVIDENCE.md`) — before/after; no invented Admin catalog rows.
5. Fill **canary validation** (`CANARY_EVIDENCE.md`) unless the release has no canary (say N/A).
6. Fill **final signoff** (`FINAL_SIGNOFF.md`) — **GO** or **NO GO** only.
7. Complete `README.md`, `CHANGELOG.md`, `MERGE_CHECKLIST.md`, `RELEASE_NOTES.md`.
8. Squash reviewed feature PRs onto `Jul_to_Sep_2026_Suport_N_Dev_Works` in documented order.
9. Archive (do not merge) any UAT integration PR.
10. Tag the squash tip. Do not move tags `v2.2-security-foundation` or `v2.2.1-governance`.

See `docs/RELEASE_GOVERNANCE.md`, `docs/governance/branch_protection_runbook.md`, and `docs/governance/branch_cleanup_runbook.md`.

## Stop conditions (BLOCKED)

Stop and report BLOCKED if asked to:

- Add Forms Authentication or a second login pipeline.
- Grant Switch User to Office Staff.
- Use `UserRoleDB` or sidebar `tlb_EmployeePermissions` as page authorization.
- Force-push or delete the release branch.
- Merge a PR marked DO NOT MERGE / UAT integration.
- Invent UAT SQL or IIS results.

## Pointers

| Topic | Doc |
| --- | --- |
| Invariants | `SECURITY_FOUNDATION_BASELINE.md` |
| Authn flow | `docs/architecture/authentication-flow.md` |
| Authz tree | `docs/architecture/authorization-flow.md` |
| Impersonation | `docs/architecture/impersonation-lifecycle.md` |
| Overlay | `docs/architecture/permission-overlay.md` |
| Reviewer / risk | `docs/governance/security_change_matrix.md` |
| GitHub rules | `docs/governance/branch_protection_runbook.md` |
| Stack branch cleanup | `docs/governance/branch_cleanup_runbook.md` |
