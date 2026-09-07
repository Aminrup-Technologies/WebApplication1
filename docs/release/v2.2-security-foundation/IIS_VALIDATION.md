# IIS runtime evidence — Security Foundation v2.2 RC

**Canonical login:** `/Login.aspx` (class `login`)  
**Landing:** `~/bussiness/production/homepage_v2.aspx`  
**Identity:** `ApplySessionFromEmployeeRow`  
**Session cookie:** `ASP.NET_SessionId` (InProc)

Operator: `J8` / LoginID `ATS002112` / ANUPAM SHARMA.

Confirm live `Web.config` still has `SwitchUserAuthorizedUsers` containing `J8`.

## Deploy

| Item | PASS |
| --- | --- |
| Site uses the VS2015 rebuild from `BUILD_EVIDENCE.md` | |
| Application pool recycled | |
| Site started | |
| `/Login.aspx` opens | |

Screenshot — login page:

```
(attach or path)
```

## Login (J8)

| Item | PASS |
| --- | --- |
| J8 / ATS002112 authenticates | |
| Homepage loads (loader overlay from #93 may appear once) | |

### Session (Immediate window, Inspector identity panel, or debugger)

| Session | Expected | Observed |
| --- | --- | --- |
| `USERTYPE` | `Admin` | |
| `WORKMAN` | `J8` | |
| `USERNAME` | ANUPAM SHARMA | |
| `RolePermissionDB` | `OS-HR` | |
| `UserRoleDB` | `ATS-OS` | |
| `USERID` | ATS002112 | |

Screenshot — homepage after login:

```
(attach or path)
```

## Runtime pages

### Switch User — `/bussiness/production/SwitchUser.aspx`

| Item | PASS |
| --- | --- |
| Opens (no redirect loop) | |
| Search visible | |
| Nested switch blocked while impersonating (if tested) | |

Screenshot:

```
(attach or path)
```

### Permission Inspector — `/bussiness/production/admin/security/PermissionInspector.aspx`

| Item | PASS |
| --- | --- |
| Opens (Admin only; no sidebar) | |
| Identity panel (USERTYPE / WORKMAN / LoginID) | |
| Source breakdown | |
| Cache / overlay health panel | |
| SWITCH_USER row: OverlayWouldAllow / LegacyWouldAllow | |

Screenshot:

```
(attach or path)
```

Empty overlay, `J8` on CSV: Allowed true, `LegacyWouldAllow` true, `OverlayWouldAllow` false, Source `USERTYPE+LEGACY_CONFIG`. Missing overlay tables: fail closed with a warning, page still opens.

### Access Analyzer — `/bussiness/production/admin/security/AccessAnalyzer.aspx`

| Item | PASS |
| --- | --- |
| Opens | |
| Snapshot section | |
| Compare section | |
| Overlay Canary panel | |

Screenshot:

```
(attach or path)
```

## Gate

| Item | PASS |
| --- | --- |
| Login.aspx opens | |
| J8 login | |
| `USERTYPE=Admin` | |
| Switch User | |
| Inspector | |
| Analyzer | |

All six must be PASS before overlay SQL (or record overlay-missing fail-closed on Inspector/Analyzer health, pages still PASS if they open).
