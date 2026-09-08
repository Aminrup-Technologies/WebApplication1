# Expense management

**Baseline:** `v2.2.3-governance-final` (`495fc68`)

## Purpose

Record expenses against work orders / departments; maintain expense heads and subheads.

## Users

Operators with expense menus. Extra head access: `CanAccess(LEGACY_EXPENSE_HEADS)` (overlay or hardcoded J8/A84/K208).

## Navigation

`add_expenses.aspx`, `view_expenses.aspx`, `view_expensedetails.aspx`, `add_expense_heads.aspx`, `add_expense_subheads.aspx`.

## Workflow

Session presence on pages. Heads/subheads extra UI via AuthorizationService (**Verified**). `add_expenses.CheckUser` still locks dropdowns when `WORKMAN == "J4"` — **Verified** live gate **not** in the service. Do not copy.

## Inputs / Outputs

Company, department, work order, amounts, heads. **Inference:** expense tables named in code-behind; not fully reverse-documented this pass.

## Database / Security / Dependencies

Heads pages pass `Session["WORKMAN"]` as AddedBy. Overlay code `LEGACY_EXPENSE_HEADS`. Master Session.

## Known issues

Unwrapped `J4` gate on `add_expenses.aspx.cs`.

## Change history

Security Foundation wrapped J8/A84/K208 lists; J4 left on the page.
