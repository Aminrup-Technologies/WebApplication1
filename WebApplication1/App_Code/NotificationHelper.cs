using System.Web.UI;

namespace WebApplication1.bussiness.production
{
    /// <summary>
    /// Shared PNotify startup-script helper. Escape modes match the page that originally owned the wrapper.
    /// Message titles and text stay at the call site. Close &amp; Send modals stay on job_outpunch_v2.
    ///
    /// Inventory (CR-010 Phase 3):
    /// | Page                 | Wrapper           | Escape                                      | Script key |
    /// | create_jobid_v2      | ShowNotification  | Full (null default, ', ", \r, \n→&lt;br/&gt;) | PNotify    |
    /// | job_360_view         | ShowNotification  | Full                                        | PNotify    |
    /// | job_inpunch_v2       | ShowNotification  | QuoteOnly (' only)                          | PNotify    |
    /// | job_outpunch_v2      | ShowNotification  | QuoteOnly                                   | PNotify    |
    /// | job_permitupload_v2  | ShowNotification  | QuoteOnly                                   | PNotify    |
    /// Types in use: error, success, warning, info, notice.
    /// </summary>
    public static class NotificationHelper
    {
        public enum EscapeMode
        {
            Full,
            QuoteOnly
        }

        public static void Success(Page page, string title, string message)
        {
            Show(page, title, message, "success", EscapeMode.Full);
        }

        public static void Success(Page page, string title, string message, EscapeMode escapeMode)
        {
            Show(page, title, message, "success", escapeMode);
        }

        public static void Error(Page page, string title, string message)
        {
            Show(page, title, message, "error", EscapeMode.Full);
        }

        public static void Error(Page page, string title, string message, EscapeMode escapeMode)
        {
            Show(page, title, message, "error", escapeMode);
        }

        public static void Warning(Page page, string title, string message)
        {
            Show(page, title, message, "warning", EscapeMode.Full);
        }

        public static void Warning(Page page, string title, string message, EscapeMode escapeMode)
        {
            Show(page, title, message, "warning", escapeMode);
        }

        public static void Info(Page page, string title, string message)
        {
            Show(page, title, message, "info", EscapeMode.Full);
        }

        public static void Info(Page page, string title, string message, EscapeMode escapeMode)
        {
            Show(page, title, message, "info", escapeMode);
        }

        public static void Show(Page page, string title, string message, string type, EscapeMode escapeMode)
        {
            string cleanMessage;
            if (escapeMode == EscapeMode.Full)
            {
                if (string.IsNullOrEmpty(message)) message = "An unknown error occurred.";
                cleanMessage = message.Replace("'", "\\'")
                                      .Replace("\"", "\\\"")
                                      .Replace("\r", "")
                                      .Replace("\n", "<br/>");
            }
            else
            {
                cleanMessage = message.Replace("'", "\\'");
            }

            string script = $"showPNotify('{title}', '{cleanMessage}', '{type}');";
            ScriptManager.RegisterStartupScript(page, page.GetType(), "PNotify", script, true);
        }
    }
}
