# SQL evidence — Security Foundation v2.2 RC

**Catalog:** `atserp_uat`  
**Scripts:** `scripts/promote_uat_admin.sql`, `scripts/create_permission_overlay.sql`  
**Do not** run `provision_admin_role.sql` with `@ApplyChanges = 1` (no `Employee_Type=Admin` catalog row).

Credentials are not stored in this repo.

## Live facts already captured (2026-09-07, agent SQL)

Do not treat this as a substitute for overlay-table evidence. Promotion was a no-op.

| Field | Expected | Captured |
| --- | --- | --- |
| WorkmanSL | J8 | J8 |
| LoginID | ATS002112 | ATS002112 |
| FullName | ANUPAM SHARMA | ANUPAM SHARMA |
| `User_RoleType` | Admin | Admin |
| `UserRoleDB` | ATS-OS | ATS-OS |
| `RolePermissionDB` | OS-HR | OS-HR |
| `Role_Permission` | OS-HR | OS-HR |
| WorkStatus | Active | Active |
| OS-HR visible menus | 31 | 31 |
| Overlay tables | exist after create script | **MISSING** at capture |
| `LastLogin` at apply | unchanged | `2026-09-07 16:56:00` |

Re-run the queries below in SSMS and paste grids.

## Before — J8 identity

```sql
SELECT
    WorkmanSL, LoginID, FullName,
    User_RoleType, UserRoleDB, RolePermissionDB, Role_Permission,
    WorkStatus, LoginStatus, LastLogin, LastLogout, PasswordExpiry
FROM dbo.tbl_Employee_Mustertable
WHERE WorkmanSL = N'J8';
```

Paste SSMS output:

```

```

| Field | Expected | Observed |
| --- | --- | --- |
| `User_RoleType` | Admin | |
| `UserRoleDB` | ATS-OS | |
| `RolePermissionDB` | OS-HR | |
| `Role_Permission` | OS-HR | |

## Overlay tables — before `create_permission_overlay.sql`

```sql
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = N'dbo'
  AND TABLE_TYPE = N'BASE TABLE'
  AND TABLE_NAME IN (
        N'tlb_permissions',
        N'tlb_permission_groups',
        N'tlb_group_permissions',
        N'tlb_employee_group',
        N'tlb_employee_permissions'
  )
ORDER BY TABLE_NAME;
```

Paste:

```

```

Expected before create: **0 rows**.

## Run overlay schema (additive)

SSMS: open `scripts/create_permission_overlay.sql`, execute on `atserp_uat`.

Confirm the script has no `DROP` / `DELETE` / muster `UPDATE` (static review already PASS).

## After — overlay tables exist

Re-run the `INFORMATION_SCHEMA` query.

| Table | Expected | Observed |
| --- | --- | --- |
| `tlb_permissions` | EXISTS | |
| `tlb_permission_groups` | EXISTS | |
| `tlb_group_permissions` | EXISTS | |
| `tlb_employee_group` | EXISTS | |
| `tlb_employee_permissions` | EXISTS | |

```sql
SELECT PermissionCode, IsActive
FROM dbo.tlb_permissions
WHERE PermissionCode IN (N'SWITCH_USER', N'USER_ADMIN')
ORDER BY PermissionCode;
```

Paste:

```

```

| Permission | Expected | Observed |
| --- | --- | --- |
| `SWITCH_USER` | active | |
| `USER_ADMIN` | active | |

```sql
SELECT COUNT(*) AS OverlayGrantRows
FROM dbo.tlb_employee_permissions;
```

Expected immediately after create: **0** employee grants.

## After — J8 identity unchanged

Re-run the J8 `SELECT` from Before.

| Field | Expected | Observed |
| --- | --- | --- |
| `User_RoleType` | Admin | |
| `UserRoleDB` | ATS-OS | |
| `RolePermissionDB` | OS-HR | |
| `Role_Permission` | OS-HR | |
| `LastLogin` | same as Before | |
| `LoginStatus` | same as Before | |

Paste After grid:

```

```

Then recycle IIS (or wait 5 minutes) for `PermissionRepository` cache.
