# tbl_expenselogs (call contract)

**Baseline:** `v2.2.3-governance-final`  
**SP bodies not in repo.**

Insert parameters from `add_expenses.aspx.cs` → `SP_InsertInto_tbl_expenselogs`:

`@ExpenseID`, `@CountryCode` (`"IN"`), `@StateCode` (`Session["USTATE"]`), `@RegionCode`, `@CompanyCode`, `@CompDept`, `@Location`, `@WorksiteCode`, `@WorksiteName`, `@WorkorderNo`, `@LoggedByWrk`, `@LoggedByName`, `@LoggedOn`, `@AppStatus` (`Pending`), `@AppByWrk` (`"J3"`), `@AppByName` (`"MAHESH CHOURASIA"`).

Detail SP `SP_InsertInto_tbl_expenselogdetails`: `@ExpenseID`, `@ExpHead`, `@ExpSubHead`, `@Quantity`, `@Description`, `@ClaimAmount`, `@FileName`, `@FileType`, `@Extension`, `@Data`, `@AppStatus`, same hardcoded approver.

Do not invent remaining columns. This page reads `Session["USTATE"]` (set on `homepage_v2.aspx.cs`). Login builder sets `SessionKeys.UserState` (`STATE`) from `WorkState`. Both keys exist (**Verified**).

## Related

- [expenses.md](../modules/expenses.md)
