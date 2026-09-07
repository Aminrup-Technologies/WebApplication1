# CHANGELOG — Security Foundation v2.2

Squash sequence only. **Exclude #103.**

| PR | Purpose | Risk | UAT |
| --- | --- | --- | --- |
| #89 | Impersonation audit foundation: `ImpersonationAudit`, `ORIGINAL_*` / `IS_IMPERSONATING`, no Switch User page yet | Medium — Session key surface | **PASS** |
| #91 | Switch User page; `ApplySessionFromEmployeeRow` reuse; master chrome; nested switch blocked | Medium — impersonation | **PASS** |
| #94 | Role/permission architecture audit (docs) | Low — documentation | **PASS** (docs; mark Ready for Review) |
| #95 | `AuthorizationService` foundation (`IsAdmin`, `CanAccess`, `DescribeIdentity`) | Medium — central gate | **PASS** |
| #96 | Overlay infrastructure: tables, `PermissionRepository`, catalog seeds | Medium — new schema | **PASS** |
| #98 | Read-only Permission Inspector (Admin only) | Low — observability | **PASS** |
| #99 | Read-only Access Analyzer + canary panel | Low — observability | **PASS** |
| #100 | Legacy migration bridge + hashed authorization snapshot | Medium — compare baseline | **PASS** (unexpected drift = 0) |
| #102 | `SWITCH_USER` dual-path canary (Admin, then overlay, else CSV) | Medium — allow/deny for Switch User | **PASS** |
| #104 | Platform Admin overlay pack (SQL + docs; Active Admin × four codes) | Medium — privilege expansion for all Active Admins | Deferred until after `#89–#102` squash |

#103 (UAT integration, DO NOT MERGE) is **out of this changelog**.
