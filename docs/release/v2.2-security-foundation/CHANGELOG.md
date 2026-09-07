# CHANGELOG — Security Foundation v2.2

Squash sequence only. **Exclude #103.** UAT column is runtime evidence, not CI.

| PR | Purpose | Risk | UAT |
| --- | --- | --- | --- |
| #89 | Impersonation audit foundation: `ImpersonationAudit`, `ORIGINAL_*` / `IS_IMPERSONATING`, no Switch User page yet | Medium — Session key surface | CI green. Runtime **not evidenced**. |
| #91 | Switch User page; `ApplySessionFromEmployeeRow` reuse; master chrome; nested switch blocked | Medium — impersonation | CI green. Page **not opened** on IIS. |
| #94 | Role/permission architecture audit (docs) | Low — documentation | CI green. Draft PR. No runtime. |
| #95 | `AuthorizationService` foundation (`IsAdmin`, `CanAccess`, `DescribeIdentity`) | Medium — central gate | CI green. Runtime **not evidenced**. |
| #96 | Overlay infrastructure: tables, `PermissionRepository`, catalog seeds, no employee grants | Medium — new schema | CI green. **Tables missing** on `atserp_uat`. |
| #98 | Read-only Permission Inspector (Admin only) | Low — observability | CI green. Page **not opened**. |
| #99 | Read-only Access Analyzer + canary panel | Low — observability | CI green. Page **not opened**. |
| #100 | Legacy migration bridge + hashed authorization snapshot | Medium — compare baseline | CI green. Snapshot files **not taken**. |
| #102 | `SWITCH_USER` dual-path canary (Admin, then overlay, else CSV) | Medium — allow/deny for Switch User | CI green. Canary SQL **not run**. |
| #104 | Platform Admin overlay pack (SQL + docs; Active Admin × four codes) | Medium — privilege expansion for all Active Admins | CI green. **Not executed.** Land after `#89–#102`. |

#103 (UAT integration, DO NOT MERGE) is **out of this changelog**.
