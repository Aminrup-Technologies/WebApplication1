# Safety pass (employee fields)

**Baseline:** `v2.2.3-governance-final` (`495fc68`)  
**Status:** Verified — **not** a standalone page module.

There is **no** `safety_pass.aspx`, `SAFETY_OVERRIDE`, or `SAFETY_PASS_MODULE.md` in this repository. Safety pass is **employee master data** alongside gate pass.

## Purpose

Store safety-pass number and expiry on the muster row; copy `SafetyPassNo` onto attendance at IN-Punch.

## Evidence

| Item | Location |
| --- | --- |
| Columns | `SafetyPassNo`, `SafetyPassExpiry` — same `ReflectNewGPData` UPDATE as gate pass |
| Registration | SP params on `emp_registration.aspx.cs` |
| IN-Punch | `job_inpunch_v2.aspx.cs` `@SafetyPassNo` |
| Helper | `FindEmployeeDataforInPunch` (`sftyno` / safety expiry) |

CSM / HSE pages (`csm_*.aspx`, `ppe_request.aspx`) are **training / PPE / TBT**, not this field. See [csm-hse.md](csm-hse.md).

## Security

No overlay code. Session via `webmaster.Master`.

## Related

- [gate-pass.md](gate-pass.md)
- [tbl_employee_mustertable.md](../database/tbl_employee_mustertable.md)
