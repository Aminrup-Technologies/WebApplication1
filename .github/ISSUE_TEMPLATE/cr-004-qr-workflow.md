---
name: CR-004 QR Workflow
about: JOBID V2.2 Change Request — scan JOBID / encoded jobid into the V2 encode-decode contract
title: "[CR-004] QR Workflow"
labels: ["v2.2", "change-request"]
---

# CR-004 — QR Workflow

**Status:** Draft only. Not approved for implementation.  
**Baseline:** `v2.1.0-jobid-remediation` (`a75bb1e`)  
**Source:** `docs/JOBID_V2.2_ROADMAP.md` Priority 2  
**This template is a Change Request draft.** Opening it does not authorize code.

## Background

JOB360 Permit / IN / OUT now encode `jobid` with `create_jobid_v2.EncodeJobID()`. V2 pages decode URL-safe Base64 and catch `FormatException`. Supervisors still type or pick JOBIDs. A QR payload could open the same encoded links. QR must not become an authorization bypass.

## Business Objective

Let a supervisor scan a JOBID (or an encoded `jobid`) and land on the correct V2 Permit, IN, or OUT page for that job, with the same session and creator inbox rules as today.

## Technical Scope

- Generate and/or scan QR that carries either plain JOBID then encode, or already-encoded `jobid`.
- Redirects must match JOB360: `job_*_v2.aspx?jobid={EncodeJobID(jobid)}`.
- `DecodeJobID` algorithm unchanged; invalid tokens fail closed (no 500).
- Existing `USERID` + `WORKMAN` (and hub six-key) checks remain.
- Inbox filters unchanged (IN: Active 3-day creator; permit: Active + Entry; OUT: code 3 + Entry).
- QR is not a substitute for session, creator, or company checks.

## Out of Scope

- Using Base64 or QR as authorization.
- Changing AddDocs / SwapDate raw JOBID links without amending this CR.
- New JOB states for “scanned”.
- Public unauthenticated scan endpoints.
- Schema for QR secrets.
- Implementation PRs until this CR is approved.

## Acceptance Criteria

- [ ] Scan of a valid in-window job opens the intended V2 page with the same encoded `jobid` as JOB360.
- [ ] Invalid / truncated QR does not 500 (`FormatException` handled).
- [ ] Logged-out scan cannot bind another creator’s job.
- [ ] UAT-046–049 still pass for non-QR JOB360 links.
- [ ] Encode/decode pair remains URL-safe Base64 as in v2.1.0.

## UAT Impact

- UAT-046, UAT-047, UAT-048, UAT-049 (regression)
- New device/scan cases only after this CR is approved (do not mint IDs in this draft)

## Rollback Strategy

Application-only. Remove QR generate/scan UI and keep JOB360 encoding. Do not retag `v2.1.0-jobid-remediation`.

## Change Classification

**Change Request** — new entry path into V2 pages. Must not land as an Enhancement that weakens routing checks.
