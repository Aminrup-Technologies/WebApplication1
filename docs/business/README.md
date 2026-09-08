# Business process notes

Narrative workflows that span multiple pages. Module pages live in [`../modules/`](../modules/README.md).

| Cross-cutting flow | Start |
| --- | --- |
| Hire → login | [employee-registration](../modules/employee-registration.md) → [authentication-flow](../architecture/authentication-flow.md) |
| JOB shift | [jobid](../modules/jobid.md) |
| Punch → attendance sheets | [jobid](../modules/jobid.md) IN/OUT → [attendance](../modules/attendance.md) |
| Pass renew → IN-Punch | [gate-pass](../modules/gate-pass.md) |
