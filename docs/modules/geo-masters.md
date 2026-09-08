# Geography and site masters

**Baseline:** `v2.2.3-governance-final` (`495fc68`)

## Purpose

Maintain country → state → region → company → worksite catalogs used by registration, JOBID, and payroll filters.

## Navigation

| Page | Table (evidenced SELECT) |
| --- | --- |
| `work_country.aspx` | `tlb_work_country` (`Country_Name`, `Country_Code`, `Id`) |
| `work_states.aspx` | `tlb_work_state` |
| `work_region.aspx` | `tlb_work_state_region` |
| `work_company.aspx` | `tlb_workregion_company` (see registration cascade) |
| `workcompany_dept.aspx`, `workcomp_deptheads.aspx` | Company departments / heads |
| `ats_work_sites.aspx` | `tlb_atsworksites` |
| `atsworksite_incharges.aspx` | Site in-charges |

## Access control

Pages use `webmaster.Master` (five-key Session). `work_country.aspx.cs` `Page_Load` has **no** extra Session check (**Verified**); the master still redirects if keys are missing. `work_company.aspx.cs` also requires `REGION` (**Verified**).

These pages are **not** gated by `IsAdmin()` or overlay. URL reachability follows the Security Foundation rule: sidebar hide ≠ ACL.

## Related

- [employee-registration.md](employee-registration.md)
- [geo-catalog.md](../database/geo-catalog.md)
