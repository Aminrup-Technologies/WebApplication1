/*
 * WHEN: 2026-09-06
 * WHY: PR #90 Switch User — admin impersonation that replays the login Session builder without a password.
 * WHAT: Allowlisted Admin users search Active employees, capture ORIGINAL_* + correlation GUID, call
 *       login.ApplySessionFromEmployeeRow, write IMPERSONATE. Return verifies IsOriginalIdentityCaptured,
 *       restores the original employee row, writes IMPERSONATE_RETURN with the same GUID, then clears snapshot keys.
 *       Does not call GrantAuthenticatedSession (no LastLogin / LoginStatus / ATS_SavedID side effects).
 */

using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class SwitchUser : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!HasAuthenticatedSession())
            {
                Response.Redirect("~/login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            bool canSwitch = ImpersonationAudit.CanImpersonate(Session);
            bool canReturn = ImpersonationAudit.CanReturnFromImpersonation(Session);
            bool impersonating = ImpersonationAudit.IsImpersonating(Session);
            if (!canSwitch && !canReturn && !impersonating)
            {
                Response.Redirect("~/bussiness/production/homepage_v2.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            BindPanels(canSwitch, canReturn || impersonating);
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (!ImpersonationAudit.CanImpersonate(Session))
            {
                DenyAndHome();
                return;
            }

            string workman = (txt_workman.Text ?? "").Trim();
            string firstName = (txt_firstname.Text ?? "").Trim();
            string fullName = (txt_fullname.Text ?? "").Trim();
            if (workman.Length == 0 && firstName.Length == 0 && fullName.Length == 0)
            {
                gv_results.DataSource = null;
                gv_results.DataBind();
                lbl_msg.Text = "Enter Workman SL, First Name, and/or Full Name.";
                Notify("Search", "Enter at least one search field.", "error");
                return;
            }

            try
            {
                DataTable dt = login.SearchActiveEmployeesForSwitch(dbcl, workman, firstName, fullName);
                gv_results.DataSource = dt;
                gv_results.DataBind();
                lbl_msg.Text = dt.Rows.Count == 0
                    ? "No Active employees matched."
                    : dt.Rows.Count + " Active employee(s) found.";
            }
            catch (Exception ex)
            {
                dbcl.WriteToFile("SwitchUser search failed: " + ex);
                Notify("Error", "Search failed. Contact IT if this continues.", "error");
            }
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            if (!ImpersonationAudit.CanImpersonate(Session))
            {
                DenyAndHome();
                return;
            }

            txt_workman.Text = "";
            txt_firstname.Text = "";
            txt_fullname.Text = "";
            gv_results.DataSource = null;
            gv_results.DataBind();
            lbl_msg.Text = "Enter at least one search field, then SEARCH.";
        }

        protected void gv_results_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            LinkButton btn = e.Row.FindControl("btn_switch") as LinkButton;
            if (btn == null) return;

            string loginId = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "LoginID"));
            string workman = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "WorkmanSL"));
            if (IsSelf(loginId, workman))
            {
                btn.Enabled = false;
                btn.OnClientClick = "return false;";
                btn.Text = "You";
            }
        }

        protected void gv_results_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "SwitchUser") return;
            SwitchToUser(e.CommandArgument != null ? e.CommandArgument.ToString() : "");
        }

        protected void btn_return_Click(object sender, EventArgs e)
        {
            ReturnToOriginal();
        }

        private void SwitchToUser(string targetLoginId)
        {
            if (!ImpersonationAudit.CanImpersonate(Session))
            {
                DenyAndHome();
                return;
            }

            targetLoginId = (targetLoginId ?? "").Trim();
            Guid corr = ImpersonationAudit.NewCorrelationId();
            bool captured = false;

            try
            {
                if (string.IsNullOrEmpty(targetLoginId))
                {
                    WriteEvent(ImpersonationAudit.EventImpersonate, "", "", ImpersonationAudit.OutcomeFailure, corr);
                    Notify("Switch User", "Missing target login.", "error");
                    return;
                }

                DataTable dt = login.FetchEmployeeRowByLoginId(dbcl, targetLoginId);
                if (dt == null || dt.Rows.Count == 0)
                {
                    WriteEvent(ImpersonationAudit.EventImpersonate, targetLoginId, "", ImpersonationAudit.OutcomeFailure, corr);
                    Notify("Switch User", "Employee was not found.", "error");
                    return;
                }

                DataRow row = dt.Rows[0];
                string targetWorkman = ReadRow(row, "WorkmanSL");
                if (!IsActive(row))
                {
                    WriteEvent(ImpersonationAudit.EventImpersonate, targetLoginId, targetWorkman, ImpersonationAudit.OutcomeFailure, corr);
                    Notify("Switch User", "Only Active employees can be switched to.", "error");
                    return;
                }

                if (IsSelf(targetLoginId, targetWorkman))
                {
                    WriteEvent(ImpersonationAudit.EventImpersonate, targetLoginId, targetWorkman, ImpersonationAudit.OutcomeFailure, corr);
                    Notify("Switch User", "You are already this user.", "error");
                    return;
                }

                ImpersonationAudit.CaptureOriginalIdentity(Session);
                captured = true;
                if (!ImpersonationAudit.IsOriginalIdentityCaptured(Session))
                {
                    ImpersonationAudit.ClearOriginalIdentity(Session);
                    WriteEvent(ImpersonationAudit.EventImpersonate, targetLoginId, targetWorkman, ImpersonationAudit.OutcomeFailure, corr);
                    Notify("Switch User", "Could not capture the original identity. Switch aborted.", "error");
                    return;
                }

                ImpersonationAudit.StoreCorrelationId(Session, corr);
                login.ApplySessionFromEmployeeRow(row);
                Session[SessionKeys.IsImpersonating] = true;
                Session["Changer"] = null;
                try
                {
                    WriteEvent(ImpersonationAudit.EventImpersonate, targetLoginId, targetWorkman, ImpersonationAudit.OutcomeSuccess, corr);
                }
                catch (Exception auditEx)
                {
                    dbcl.WriteToFile("SwitchUser IMPERSONATE success audit failed: " + auditEx);
                }
                RedirectHome();
            }
            catch (System.Threading.ThreadAbortException)
            {
                throw;
            }
            catch (Exception ex)
            {
                dbcl.WriteToFile("SwitchUser switch failed: " + ex);
                if (captured && !ImpersonationAudit.IsImpersonating(Session))
                {
                    ImpersonationAudit.ClearOriginalIdentity(Session);
                }
                try
                {
                    WriteEvent(ImpersonationAudit.EventImpersonate, targetLoginId, "", ImpersonationAudit.OutcomeFailure, corr);
                }
                catch (Exception auditEx)
                {
                    dbcl.WriteToFile("SwitchUser IMPERSONATE failure audit failed: " + auditEx);
                }
                if (ImpersonationAudit.IsImpersonating(Session))
                {
                    RedirectHome();
                    return;
                }
                Notify("Switch User", "Switch failed. Your original session was left unchanged.", "error");
            }
        }

        private void ReturnToOriginal()
        {
            if (!ImpersonationAudit.CanReturnFromImpersonation(Session))
            {
                Notify("Return", "Original identity snapshot is incomplete. Stay in the current session and contact IT.", "error");
                return;
            }

            string targetLoginId = ReadSession(SessionKeys.UserID);
            string targetWorkman = ReadSession(SessionKeys.WorkmanSL);
            string originalLoginId = ReadSession(SessionKeys.OriginalUserID);
            string originalWorkman = ReadSession(SessionKeys.OriginalWorkmanSL);
            Guid corr = ImpersonationAudit.GetCorrelationId(Session);

            try
            {
                DataTable dt = login.FetchEmployeeRowByLoginId(dbcl, originalLoginId);
                if (dt == null || dt.Rows.Count == 0)
                {
                    WriteEvent(ImpersonationAudit.EventImpersonateReturn, targetLoginId, targetWorkman, ImpersonationAudit.OutcomeFailure, corr);
                    Notify("Return", "Original account was not found. You remain in the impersonated session.", "error");
                    return;
                }

                DataRow row = dt.Rows[0];
                if (!IsActive(row))
                {
                    WriteEvent(ImpersonationAudit.EventImpersonateReturn, targetLoginId, targetWorkman, ImpersonationAudit.OutcomeFailure, corr);
                    Notify("Return", "Original account is not Active. You remain in the impersonated session.", "error");
                    return;
                }

                if (!string.Equals(ReadRow(row, "WorkmanSL"), originalWorkman, StringComparison.OrdinalIgnoreCase))
                {
                    WriteEvent(ImpersonationAudit.EventImpersonateReturn, targetLoginId, targetWorkman, ImpersonationAudit.OutcomeFailure, corr);
                    Notify("Return", "Original snapshot does not match the employee record. You remain in the impersonated session.", "error");
                    return;
                }

                login.ApplySessionFromEmployeeRow(row);
                Session["Changer"] = null;
                try
                {
                    WriteEvent(ImpersonationAudit.EventImpersonateReturn, targetLoginId, targetWorkman, ImpersonationAudit.OutcomeSuccess, corr);
                }
                catch (Exception auditEx)
                {
                    dbcl.WriteToFile("SwitchUser IMPERSONATE_RETURN success audit failed: " + auditEx);
                }
                ImpersonationAudit.ClearOriginalIdentity(Session);
                RedirectHome();
            }
            catch (System.Threading.ThreadAbortException)
            {
                throw;
            }
            catch (Exception ex)
            {
                dbcl.WriteToFile("SwitchUser return failed: " + ex);
                try
                {
                    WriteEvent(ImpersonationAudit.EventImpersonateReturn, targetLoginId, targetWorkman, ImpersonationAudit.OutcomeFailure, corr);
                }
                catch (Exception auditEx)
                {
                    dbcl.WriteToFile("SwitchUser IMPERSONATE_RETURN failure audit failed: " + auditEx);
                }
                Notify("Return", "Restore failed. You remain in the impersonated session.", "error");
            }
        }

        private void BindPanels(bool canSwitch, bool canReturn)
        {
            pnl_search.Visible = canSwitch;
            pnl_return.Visible = canReturn;
            if (canReturn)
            {
                lbl_currentTarget.Text = ReadSession(SessionKeys.UserName);
                lbl_currentTargetWorkman.Text = ReadSession(SessionKeys.WorkmanSL);
                lbl_returnOriginal.Text = ReadSession(SessionKeys.OriginalUserName);
            }
        }

        private bool HasAuthenticatedSession()
        {
            return Session != null
                && Session[SessionKeys.UserID] != null
                && Session[SessionKeys.WorkmanSL] != null
                && Session[SessionKeys.UserName] != null
                && Session[SessionKeys.UserRoleDB] != null
                && Session[SessionKeys.RolePermissionDB] != null;
        }

        private bool IsSelf(string loginId, string workman)
        {
            return string.Equals(loginId, ReadSession(SessionKeys.UserID), StringComparison.OrdinalIgnoreCase)
                || string.Equals(workman, ReadSession(SessionKeys.WorkmanSL), StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsActive(DataRow row)
        {
            return string.Equals(ReadRow(row, "WorkStatus"), "Active", StringComparison.OrdinalIgnoreCase);
        }

        private static string ReadRow(DataRow row, string column)
        {
            if (row == null || !row.Table.Columns.Contains(column) || row[column] == DBNull.Value) return "";
            return row[column].ToString();
        }

        private string ReadSession(string key)
        {
            if (Session == null || Session[key] == null) return "";
            return Session[key].ToString();
        }

        private void WriteEvent(string eventType, string targetLoginId, string targetWorkman, string outcome, Guid correlationId)
        {
            ImpersonationAudit.WriteFromRequest(dbcl, Request, Session, eventType, targetLoginId, targetWorkman, outcome, correlationId);
        }

        private void DenyAndHome()
        {
            Notify("Switch User", "You are not allowed to switch users.", "error");
            RedirectHome();
        }

        private void RedirectHome()
        {
            Response.Redirect("~/bussiness/production/homepage_v2.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void Notify(string title, string text, string type)
        {
            string script = "new PNotify({ title: '" + HttpUtility.JavaScriptStringEncode(title)
                + "', text: '" + HttpUtility.JavaScriptStringEncode(text)
                + "', type: '" + HttpUtility.JavaScriptStringEncode(type)
                + "', styling: 'bootstrap3' });";
            ClientScript.RegisterStartupScript(GetType(), "SwitchUserNotify", script, true);
        }
    }
}
