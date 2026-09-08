# Supervisor features

**Baseline:** CR-001 Phase A certified (`238bd2f`); program root `v2.2.3-governance-final`.

## Purpose

Presentation shell so supervisors move Create → IN → Permit → OUT without a new JOB state machine.

## Users

Supervisors. Not JOB360 admin chrome.

## Navigation

`supervisor_wizard.aspx` (`webmaster.Master`). Spec: [`CR-001_UNIFIED_SUPERVISOR_WIZARD_SPEC.md`](../CR-001_UNIFIED_SUPERVISOR_WIZARD_SPEC.md). Execution freeze: [`CR-001_EXECUTION_BASELINE.md`](../CR-001_EXECUTION_BASELINE.md).

## Workflow

Wizard **orchestrates** V2 pages. It must not write JOB state, invent status codes, or host Close & Send (stays on `job_outpunch_v2`). Phase B+ needs a new CR (issue template `jobid-v2.2`).

## Security

Session presence. No Switch User. Base64 still not authorization.

## Dependencies

[JOBID](jobid.md), `JobIdCodec`, CR-010 helpers.

## Change history

Phase A certified; discovery PR #68 closed as superseded.
