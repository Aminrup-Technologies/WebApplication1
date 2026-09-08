# Supply memo

**Baseline:** `v2.2.3-governance-final` (`495fc68`)

## Purpose

Create and view supply memos / supply jobs.

## Users

Operators with memo menus.

## Navigation

`create_supplymemo.aspx`, `vw_supplyjobs.aspx`, report `rpts/supplymemo.aspx`.

## Workflow

Session presence. Commented historical gate `WORKMAN == A84 || K208` on create (**Verified** commented; inactive).

## Database / Security

Do not invent memo tables. No `CanAccess` on these pages in the Security Foundation wrap list. Treat as Session + menu chrome until a CR migrates them.

## Known issues

Do not uncomment Workman allowlists; use overlay if elevation is required.

## Change history

None in v2.2 security squash besides leaving the comment in place.
