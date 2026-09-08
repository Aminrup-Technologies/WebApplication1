# CSM / HSE

**Baseline:** `v2.2.3-governance-final` (`495fc68`)  
**Master:** `webmaster.Master`

## Purpose

Contractor Safety Management: toolbox talks, SOP / global training, incident reporting, PPE request, CSM reports, and a designation-based KPI hub.

## Users

Operators with CSM menus. Hub chrome is further split by `Session["DESG"]` on `csms_mainview.aspx.cs` (**Verified**): `SUPERVISOR`, `SAFETY SUPERVISOR`, `SAFETY OFFICER`, `IN-CHARGE`, `STORE KEEPER`. `DESG` is set on `homepage_v2.aspx.cs`, **not** in `SessionKeys` / `ApplySessionFromEmployeeRow`.

## Navigation

| Page | Role |
| --- | --- |
| `csms_mainview.aspx` | Hub; extra `REGION` presence check |
| `csm_toolboxtalk.aspx`, `csm_tbtform.aspx`, `vw_csm_toolboxtalk.aspx`, `vw_csm_toolboxtalkdetails.aspx` | Toolbox talk |
| `csm_soptraining.aspx`, `csm_globaltraining.aspx` | Training records |
| `csm_incidentreporting.aspx` | Incidents |
| `csm_approvals.aspx`, `vw_tbtforapproval.aspx` | Approvals |
| `csm_reports.aspx` | CSM reports |
| `ppe_request.aspx` | PPE request (linked from hub) |
| `CSMDocShiftOverview.aspx` | Superset embed |

## Workflow

Session (five keys; hub also `REGION`). Submit paths call stored procedures (**bodies not in repo**):

| SP | Call site |
| --- | --- |
| `SP_InsertInto_TBTDataTable` | `csm_toolboxtalk.aspx.cs`, `csm_tbtform.aspx.cs`, `vw_csm_toolboxtalkdetails.aspx.cs` |
| `SP_InsertInto_tbl_soptraining` | `csm_soptraining.aspx.cs` |
| `csm_globaltraining.aspx.cs` | `SP_InsertInto_tbl_trainingrecords` |

Do not invent TBT / training table columns beyond those parameters.

## Security

No CSM overlay code in `AuthorizationFeatureCodes`. Designation branching is **page-local**, not `IsAdmin()`. PPE insert SPs on `ppe_request.aspx.cs` are **commented**.

## Related

- [jobid.md](jobid.md) (`@CSM_Documents` on JOB insert)
- [reports.md](reports.md)
