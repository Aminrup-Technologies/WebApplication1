using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace WebApplication1.bussiness.production
{
    public partial class supervisor_wizard : System.Web.UI.Page
    {
        private static readonly string[] StepLabels = { "Create", "IN", "Permit", "OUT", "Close", "Complete" };

        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["WORKMAN"] == null)
                {
                    Response.Redirect("~/login.aspx", false);
                    return;
                }

                if (Request.QueryString["jobid"] != null)
                {
                    txt_jobid.Text = Request.QueryString["jobid"].ToString().Trim();
                    LoadJob(txt_jobid.Text);
                }
                else
                {
                    ApplyChrome();
                    txt_jobid.Focus();
                }
            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_jobid.Text))
            {
                NotificationHelper.Show(this, "Validation", "Please enter a JOBID.", "warning", NotificationHelper.EscapeMode.Full);
                txt_jobid.Focus();
                return;
            }
            LoadJob(txt_jobid.Text.Trim());
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            txt_jobid.Text = "";
            hf_jobid.Value = "";
            hf_visualStep.Value = "0";
            hf_progress.Value = "";
            pnl_card.Visible = false;
            pnl_empty.Visible = true;
            ApplyChrome();
        }

        protected void btn_previous_Click(object sender, EventArgs e)
        {
            int step = ReadVisualStep();
            if (step > 0) step--;
            hf_visualStep.Value = step.ToString();
            ApplyChrome();
        }

        protected void btn_next_Click(object sender, EventArgs e)
        {
            int step = ReadVisualStep();
            if (step < 5) step++;
            hf_visualStep.Value = step.ToString();
            ApplyChrome();
        }

        protected void btn_open_Click(object sender, EventArgs e)
        {
            int step = ReadVisualStep();
            string jobid = (hf_jobid.Value ?? "").Trim();

            switch (step)
            {
                case 0:
                    Response.Redirect("create_jobid_v2.aspx", false);
                    break;
                case 1:
                    RedirectV2("job_inpunch_v2.aspx", jobid);
                    break;
                case 2:
                    RedirectV2("job_permitupload_v2.aspx", jobid);
                    break;
                case 3:
                case 4:
                    RedirectV2("job_outpunch_v2.aspx", jobid);
                    break;
                default:
                    Response.Redirect("jobs_and_manpower_v2.aspx", false);
                    break;
            }
        }

        private void RedirectV2(string page, string jobid)
        {
            if (string.IsNullOrEmpty(jobid))
            {
                Response.Redirect(page, false);
                return;
            }
            Response.Redirect(page + "?jobid=" + create_jobid_v2.EncodeJobID(jobid), false);
        }

        private void LoadJob(string jobid)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                string query = "SELECT JOBID, JOB_Site, JOB_Company, JOB_Shift, JOB_Status, EntryExit, MasterStatusCode, FileCount FROM tbl_jobs WHERE JOBID = @JOBID AND Creator_Workman = @Workman";
                using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@JOBID", jobid);
                    cmd.Parameters.AddWithValue("@Workman", Session["WORKMAN"].ToString());
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count == 0)
                        {
                            pnl_card.Visible = false;
                            pnl_empty.Visible = true;
                            hf_jobid.Value = "";
                            NotificationHelper.Show(this, "Not Found", "JOBID not found for this supervisor.", "info", NotificationHelper.EscapeMode.Full);
                            ApplyChrome();
                            return;
                        }

                        DataRow row = dt.Rows[0];
                        hf_jobid.Value = GetSafe(row, "JOBID", jobid);
                        lbl_jobid.Text = hf_jobid.Value;
                        lbl_site.Text = GetSafe(row, "JOB_Site", "");
                        lbl_contractor.Text = GetSafe(row, "JOB_Company", "");
                        lbl_shift.Text = GetSafe(row, "JOB_Shift", "");

                        string entryExit = GetSafe(row, "EntryExit", "");
                        string status = GetSafe(row, "JOB_Status", "");
                        string code = GetSafe(row, "MasterStatusCode", "");
                        lbl_state.Text = status + " / " + entryExit;

                        bool[] done = DeriveProgress(row);
                        hf_progress.Value = string.Join(",", Array.ConvertAll(done, b => b ? "1" : "0"));
                        int recommended = FirstIncomplete(done);
                        hf_visualStep.Value = recommended.ToString();
                        lbl_next.Text = StepLabels[recommended];

                        pnl_card.Visible = true;
                        pnl_empty.Visible = false;
                        ApplyChrome(done);
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationHelper.Show(this, "Error", ex.Message, "error", NotificationHelper.EscapeMode.Full);
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        private bool[] DeriveProgress(DataRow row)
        {
            string entryExit = GetSafe(row, "EntryExit", "");
            string code = GetSafe(row, "MasterStatusCode", "");
            int fileCount = 0;
            int.TryParse(GetSafe(row, "FileCount", "0"), out fileCount);

            bool create = true;
            bool inpunch = entryExit.Equals(JobStatusConstants.EntryExitEntry, StringComparison.OrdinalIgnoreCase)
                || entryExit.Equals(JobStatusConstants.EntryExitExit, StringComparison.OrdinalIgnoreCase);
            bool permit = fileCount > 0;
            bool closed = code == JobStatusConstants.CodeClosed || code == JobStatusConstants.CodeApproved;
            bool complete = code == JobStatusConstants.CodeApproved;
            bool outpunch = closed || complete
                || GetSafe(row, "JOB_Status", "").Equals(JobStatusConstants.StatusOutPunchDone, StringComparison.OrdinalIgnoreCase)
                || entryExit.Equals(JobStatusConstants.EntryExitExit, StringComparison.OrdinalIgnoreCase);

            return new[] { create, inpunch, permit, outpunch, closed, complete };
        }

        private static int FirstIncomplete(bool[] done)
        {
            for (int i = 0; i < done.Length; i++)
            {
                if (i == 2) continue;
                if (!done[i]) return i;
            }
            return 5;
        }

        private void ApplyChrome(bool[] done = null)
        {
            if (done == null) done = ReadProgress();
            int current = ReadVisualStep();
            HtmlGenericControl[] steps = { step_create, step_in, step_permit, step_out, step_close, step_complete };
            for (int i = 0; i < steps.Length; i++)
            {
                string css = "sw-step";
                if (i == 2) css += " is-optional";
                if (done != null && i < done.Length && done[i]) css += " is-complete";
                if (i == current) css += " is-current";
                steps[i].Attributes["class"] = css;
            }
            lit_step_title.InnerText = StepLabels[current];
            btn_previous.Enabled = current > 0;
            btn_next.Enabled = current < 5;
        }

        private bool[] ReadProgress()
        {
            string raw = hf_progress.Value ?? "";
            string[] parts = raw.Split(new[] { ',' }, StringSplitOptions.None);
            if (parts.Length != 6) return null;
            bool[] done = new bool[6];
            for (int i = 0; i < 6; i++) done[i] = parts[i] == "1";
            return done;
        }

        private int ReadVisualStep()
        {
            int step;
            if (!int.TryParse(hf_visualStep.Value, out step)) step = 0;
            if (step < 0) step = 0;
            if (step > 5) step = 5;
            return step;
        }

        private static string GetSafe(DataRow row, string col, string fallback)
        {
            if (row.Table.Columns.Contains(col) && row[col] != DBNull.Value)
            {
                string val = row[col].ToString().Trim();
                return string.IsNullOrEmpty(val) ? fallback : val;
            }
            return fallback;
        }
    }
}
