# Integrations

**Verified** from `Web.config.example` and call sites.

| Integration | Evidence | Notes |
| --- | --- | --- |
| SMTP | `SmtpUser` / `SmtpPass` | Login MFA email; homepage notify; helpdesk uses **hardcoded** `smtp.zoho.in:587` in `HelpdeskCalls.cs` |
| MSG91 WhatsApp | `Msg91AuthKey`, `Msg91IntegratedNumber`, `Msg91MfaTemplateName`, namespace, language | `Msg91WhatsAppHelper.cs`; also used from some JOB pages |
| Apache Superset | `SupersetPassword`; base URL hardcoded `https://reports.aminruptechnologies.co.in` | Overview pages under [reports.md](../modules/reports.md) |
| Notification flags | `NotificationTriggerHelper` | Gates helpdesk (and other) email |

**Inferred:** bank/PF file formats are application-generated sheets, not a third-party payroll API in the paths cited this pass.

Do not invent other SaaS connectors without a call site.

## Related

- [appendix/configuration.md](../appendix/configuration.md)
- [mfa](../architecture/mfa.md)
