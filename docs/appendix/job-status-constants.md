# JOB status constants

**Evidence:** `WebApplication1/App_Code/JobStatusConstants.cs` (CR-010). Do not invent extra codes here.

| Constant | Value |
| --- | --- |
| `EntryExitCreated` | `Created` |
| `EntryExitEntry` | `Entry` |
| `EntryExitExit` | `Exit` |
| `StatusActive` | `Active` |
| `StatusOutPunchDone` | `Out-Punch Done` |
| `CodeCreated` | `1` |
| `CodeInPunch` | `3` |
| `CodeClosed` | `4` |
| `CodeApproved` | `5` |
| `CodeCancelled` | `6` (declared; Cancel SQL may still be literal — see JOBID audit) |
| `CodeDeleted` | `0` |

Canonical lifecycle: [`JOBID_LIFECYCLE_FUNCTIONAL_AUDIT.md`](../JOBID_LIFECYCLE_FUNCTIONAL_AUDIT.md). Wrapper: [jobid.md](../modules/jobid.md).
