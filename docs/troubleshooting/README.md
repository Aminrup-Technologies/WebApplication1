# Troubleshooting

Runbooks from **repository history**, not invented outages.

Planned topics (each needs a cited PR/commit or UAT pack):

| Topic | Evidence seed |
| --- | --- |
| Session redirect to login | `webmaster.Master` five-key gate |
| Switch User denied | Dual-path `CanAccess(SWITCH_USER)`; Office Staff never allowed |
| Overlay cache stale | `PermissionRepository` 5-minute TTL; recycle IIS |
| Nested Switch User | `IS_IMPERSONATING` |
| Logout while impersonating | Deferred logout on target `USERID` |
| Duplicate compile items | `.csproj` uniqueness (CONTRIBUTING) |

Index: [`ERP_DOCUMENTATION_INDEX.md`](../ERP_DOCUMENTATION_INDEX.md).
