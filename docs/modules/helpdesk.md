# Helpdesk / grievance

**Baseline:** `v2.2.3-governance-final` (`495fc68`)

## Purpose

Create and view helpdesk / grievance tickets. Not Switch User. Not overlay `USER_ADMIN`.

## Navigation

| Page / helper | Role |
| --- | --- |
| `add_nwhelpdsk.aspx` | Create; extra `REGION` check |
| `helpdesk_ticketdetails.aspx` | Ticket detail |
| `HelpdeskCalls.cs` | `usp_InsertHelpDeskTicket` + SMTP notify |

`GrievanceReq_View.aspx` exists under `bussiness/production/` (**Verified** glob). Treat it as a grievance list companion; cite its `Page_Load` before claiming extra gates.

## Workflow

1. `add_nwhelpdsk` binds `tlb_hlpdsk_root1` and `tlb_supportlvl`, then tickets for `Session["USERNAME"]` (**Verified** comment + bind).
2. Insert: `usp_InsertHelpDeskTicket` (**body not in repo**) with `@ticket_id`, creator Workman/name/region/company, root1–3, priority, description, status.
3. Email: `HelpdeskCalls.SendEmail` uses **hardcoded** `smtp.zoho.in:587` plus `SmtpUser` / `SmtpPass`, gated by `NotificationTriggerHelper.ModuleHelpdesk`.

## Security

Session five keys (+ `REGION` on create). No helpdesk overlay code. SMTP host is **not** an AppSetting (**Verified**).

## Related

- [integrations](../integrations/README.md)
- [stored-procedure-call-contracts](../database/stored-procedure-call-contracts.md)
