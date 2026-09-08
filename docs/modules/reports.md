# Reports

**Baseline:** `v2.2.3-governance-final` (`495fc68`)  
**Status:** Verified from filenames and cited code. There is **no** `REPORTS_MODULE.md` and **no** `Microsoft.ReportViewer` usage in `WebApplication1/` (search returned none).

## Purpose

Read-oriented outputs: printable HTML under `rpts/`, generated statutory/attendance sheets, analysis pages, and embedded Superset dashboards.

## Navigation (filename evidence)

| Kind | Pages |
| --- | --- |
| Print HTML | `rpts/supplymemo.aspx`, `rpts/salaryslip.aspx`, `rpts/f29.aspx`, `rpts/tbttalk_rpt.aspx`, `rpts/rpt_CombinedF16.aspx` |
| Generated sheets | `generate_atdncsheet.aspx`, `generate_siteatdncsheet.aspx`, `generate_pfsheets.aspx`, `generate_esicsheets.aspx`, `generate_banksheets.aspx`, `generate_f29sheet.aspx`, `gen_*_f17.aspx` |
| Analysis | `anlys_jobsdeta.aspx`, `anlys_musterdeta.aspx`, `anlys_employees.aspx` |
| Superset embed | `Live_overall_pnl.aspx`, `LiveJOB_pnl.aspx`, `JobOverview.aspx`, `PayrollSummary.aspx`, `PassMonitoring.aspx`, `ViolationsOverview.aspx`, `RolesOverview.aspx`, `CSMDocShiftOverview.aspx` |

## Access control

- Sheet / analysis pages typically use `webmaster.Master` (five-key Session). Individual `CanAccess` keys are **not** assumed; cite the page before claiming one.
- Superset pages read `SupersetPassword` from AppSettings (`Live_overall_pnl.aspx.cs` **Verified**). That page uses `webmaster.Master` (**Verified** directive); code-behind `Page_Load` only sets TLS.
- No global `REPORTS` overlay code in `AuthorizationFeatureCodes`.

## Database

Do not invent a reports schema. Sheets query existing JOB / attendance / payroll objects (see module docs). Superset is an **external** service (`https://reports.aminruptechnologies.co.in` hardcoded on `Live_overall_pnl.aspx.cs`).

## Related

- [payroll.md](payroll.md), [attendance.md](attendance.md), [jobid.md](jobid.md)
- [integrations](../integrations/README.md)
