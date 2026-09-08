# Geography catalog tables

**Baseline:** `v2.2.3-governance-final`  
**Evidence:** `emp_registration.aspx.cs` dropdowns and master pages. **Not** a schema dump.

| Table | Evidenced columns | Used by |
| --- | --- | --- |
| `tlb_work_country` | `Country_Name`, `Country_Code`, `Id` | Registration; `work_country.aspx` `SELECT *` |
| `tlb_work_state` | `State_Name`, `State_Code`, `Country_Code`, `Id` | Registration cascade |
| `tlb_work_state_region` | `Work_Region_Name`, `Work_Region_Code`, `Country_Code`, `State_Code`, `Id` | Registration |
| `tlb_workregion_company` | `Company_Name`, `Company_Code`, `Country_Code`, `State_Code`, `Work_Region_Code`, `Id` | Registration |
| `tlb_atsworksites` | `Worksite_Name`, `Worksite_Code`, `Country_Code`, `State_Code`, `WorkRegion_Code`, `Company_Code`, `Id` | Registration |

Do not invent extra geo columns. Department / in-charge pages exist ([geo-masters](../modules/geo-masters.md)); document those tables when their SQL is cited in a follow-up.

## Related

- [employee-registration.md](../modules/employee-registration.md)
