# IIS runtime evidence

Canonical login: `/Login.aspx`. Recycle the app pool after deploy and after security SQL.

| Item | PASS |
| --- | --- |
| Site started | |
| Login.aspx opens | |
| Operator login | |
| Session identity matches expected | |
| Page 1 | |
| Page 2 | |
| Page 3 | |

Expected Session (fill for this release):

| Session | Expected | Observed |
| --- | --- | --- |
| `USERTYPE` | | |
| `WORKMAN` | | |
| `RolePermissionDB` | | |

Screenshots:

```
(paths)
```
