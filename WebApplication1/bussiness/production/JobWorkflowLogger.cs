using System;
using System.IO;
using System.Text;
using System.Web;

namespace WebApplication1.bussiness.production
{
    public class JobWorkflowLogger
    {
        /*
        ======================================================================
        Method: LogAction
        Why: Centralized text file logging to track a JOB's complete lifecycle across multiple web pages.
        What: Creates or Appends to ~/Job_Workflow_Logs/[JOBID].txt
        ======================================================================
        */
        public static void LogAction(string jobId, string stepName, string workmanId, string actionDetails)
        {
            try
            {
                // 1. Define the dedicated folder path
                string logDirectory = HttpContext.Current.Server.MapPath("~/Logs/Job_Workflow_Logs/");

                // 2. Create directory if it doesn't exist
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // 3. Set the specific file path (e.g., JOB260506512.txt)
                string filePath = Path.Combine(logDirectory, jobId + ".txt");

                // 4. Build a beautifully formatted log entry
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("=========================================================");
                sb.AppendLine($"TIMESTAMP  : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                sb.AppendLine($"STEP       : {stepName}");
                sb.AppendLine($"WORKMAN ID : {workmanId}");
                sb.AppendLine("=========================================================");
                sb.AppendLine("DETAILS:");
                sb.AppendLine(actionDetails);
                sb.AppendLine(""); // Blank line for spacing between entries

                // 5. Append text to the file (Creates the file if it doesn't exist automatically)
                File.AppendAllText(filePath, sb.ToString());
            }
            catch
            {
                // We fail silently here. If a folder permissions issue occurs, 
                // we DO NOT want to crash the user's web page workflow.
            }
        }
    }
}