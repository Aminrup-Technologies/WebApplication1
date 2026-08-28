using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;

namespace WebApplication1.bussiness.production
{
    public partial class usertoggle : System.Web.UI.Page
    {
        public static string workman = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session == null || Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("~/login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                string userId = Convert.ToString(Session["USERID"]);
                string userName = Convert.ToString(Session["USERNAME"]);
                workman = Convert.ToString(Session["WORKMAN"]);

                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(workman))
                {
                    Response.Redirect("~/login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                if (!IsPostBack)
                {
                    // If your Load* methods accept a user id (recommended) call them with the userId.
                    // If they read Session internally, calling them directly is fine.
                    LoadAadhaarDetails(workman);
                    LoadPANDetails();
                    LoadBankDetails();
                    LoadEducationDetails();
                    // --- NEW: Load HR Rejection Feedback ---
                    LoadRejectionNote(workman);
                }
            }
            catch (ThreadAbortException) // redirect may still raise this in older code paths; let it bubble
            {
                throw;
            }
            catch (Exception ex)
            {
                // Log the exception for diagnostics
                try { System.Diagnostics.Trace.TraceError($"Page_Load error in usertoggle: {ex}"); } catch { /* ignore logging errors */ }

                // Friendly message to user; optionally redirect to an error page instead
                ScriptManager.RegisterStartupScript(this, this.GetType(), "pageLoadErr", "alert('An unexpected error occurred. Please try again or contact support.');", true);
            }
        }

        private void LoadRejectionNote(string workmanSL)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
                {
                    string query = "SELECT RejectionNote FROM tbl_EmployeeDocsDetails WHERE WorkmanSL = @WorkmanSL";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@WorkmanSL", workmanSL);
                        conn.Open();
                        object note = cmd.ExecuteScalar();

                        if (note != null && note != DBNull.Value && !string.IsNullOrWhiteSpace(note.ToString()))
                        {
                            divRejectionAlert.Visible = true;
                            lblRejectionNote.Text = note.ToString();
                        }
                        else
                        {
                            divRejectionAlert.Visible = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Failsafe: hide alert if DB errors out
                divRejectionAlert.Visible = false;
            }
        }
        private void LoadAadhaarDetails(string WorkmanSL)
        {
            //string WorkmanSL = Session["WorkmanSL"]?.ToString();
            if (string.IsNullOrEmpty(WorkmanSL))
            {
                lblAadhaarStatus.Text = "Session expired. Please login again.";
                lblAadhaarStatus.CssClass = "error-msg";
                return;
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                string query = @"
            SELECT TOP 1 AadhaarID, AadhaarNumber, AadhaarName, IssueDate, AadhaarImagePath 
            FROM AadhaarDetails 
            WHERE UserID = @UserID 
            ORDER BY AadhaarID DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", WorkmanSL);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtAadhaarNo.Text = reader["AadhaarNumber"]?.ToString();
                                txtAadhaarName.Text = reader["AadhaarName"]?.ToString();

                                DateTime issueDate;
                                if (DateTime.TryParse(reader["IssueDate"] != null ? reader["IssueDate"].ToString() : null, out issueDate))
                                {
                                    txtIssueDate.Text = issueDate.ToString("yyyy-MM-dd");
                                }


                                string imgPath = reader["AadhaarImagePath"]?.ToString();
                                if (!string.IsNullOrEmpty(imgPath))
                                {
                                    imgAadhaarPreview.Attributes["src"] = ResolveUrl(imgPath);
                                }
                                else
                                {
                                    imgAadhaarPreview.Attributes["src"] = ResolveUrl("~/Images/placeholder.png");
                                }

                                ViewState["AadhaarID"] = reader["AadhaarID"];

                                btnSubmitAadhaar.Text = "Update";
                                btnSubmitAadhaar.CssClass = "btn btn-warning";
                            }
                            else
                            {
                                // Reset if no record found
                                txtAadhaarNo.Text = "";
                                txtAadhaarName.Text = "";
                                txtIssueDate.Text = "";
                                //imgAadhaarPreview.ImageUrl = "~/Images/placeholder.png";
                                //imgAadhaarPreview.Visible = true;
                                btnSubmitAadhaar.Text = "Submit";
                                btnSubmitAadhaar.CssClass = "btn btn-primary";
                                ViewState.Remove("AadhaarID");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblAadhaarStatus.Text = "Error loading Aadhaar details: " + ex.Message;
                        lblAadhaarStatus.CssClass = "error-msg";
                    }
                }
            }
        }
        protected void btnSubmitAadhaar_Click(object sender, EventArgs e)
        {
            string WorkmanSL = Session["WORKMAN"]?.ToString();
            if (string.IsNullOrEmpty(WorkmanSL))
            {
                lblAadhaarStatus.Text = "Session expired. Please login again.";
                lblAadhaarStatus.CssClass = "error-msg";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session expired. Please login again.');", true);
                return;
            }

            // Reset previous error visuals
            txtAadhaarNo.CssClass = "form-control";
            txtAadhaarName.CssClass = "form-control";
            txtIssueDate.CssClass = "form-control";
            lblAadhaarStatus.Text = "";
            lblAadhaarStatus.CssClass = "";

            bool isValid = true;
            string errorMessages = "";

            // Aadhaar number validation
            string aadhaar = txtAadhaarNo.Text.Trim();
            if (string.IsNullOrEmpty(aadhaar) || !Regex.IsMatch(aadhaar, @"^\d{12}$"))
            {
                txtAadhaarNo.CssClass += " error-border";
                errorMessages += "Enter a valid 12-digit Aadhaar number.\n";
                isValid = false;
            }

            // Aadhaar name validation
            string aadhaarName = txtAadhaarName.Text.Trim();
            if (string.IsNullOrEmpty(aadhaarName))
            {
                txtAadhaarName.CssClass += " error-border";
                errorMessages += "Name as per Aadhaar is required.\n";
                isValid = false;
            }

            // Aadhaar issue date validation
            string issueDateStr = txtIssueDate.Text.Trim();
            DateTime issueDate = DateTime.MinValue;
            if (string.IsNullOrEmpty(issueDateStr) || !DateTime.TryParse(issueDateStr, out issueDate))
            {
                txtIssueDate.CssClass += " error-border";
                errorMessages += "Enter a valid Aadhaar issue date.\n";
                isValid = false;
            }
            else if (issueDate > DateTime.Today)
            {
                txtIssueDate.CssClass += " error-border";
                errorMessages += "Issue date cannot be in the future.\n";
                isValid = false;
            }

            // Aadhaar image upload validation for initial submit
            if (!fuAadhaarImage.HasFile && btnSubmitAadhaar.Text == "Submit")
            {
                errorMessages += "Please upload the Aadhaar card image.\n";
                isValid = false;
            }

            if (!isValid)
            {
                lblAadhaarStatus.Text = errorMessages.Replace("\n", "<br/>");
                lblAadhaarStatus.CssClass = "error-msg";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{errorMessages.Replace("\n", "\\n")}');", true);
                return;
            }

            // Save the image file using the centralized helper. It returns a relative path or "NA".
            string savedRelativePath = "NA";
            try
            {
                savedRelativePath = SaveEmployeeDocument(fuAadhaarImage, WorkmanSL, "Aadhaar"); // returns "/Uploads/{WorkmanSL}/Aadhaar/filename" or "NA"
            }
            catch (Exception exFile)
            {
                // File save failed — inform user and abort before DB changes
                lblAadhaarStatus.Text = "Error saving file: " + exFile.Message;
                lblAadhaarStatus.CssClass = "error-msg";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('Error saving file: {exFile.Message.Replace("'", "\\'")}');", true);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction tx = conn.BeginTransaction())
                    {
                        try
                        {
                            // Check if record exists
                            string checkQuery = "SELECT COUNT(*) FROM AadhaarDetails WHERE UserID = @UserID";
                            using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn, tx))
                            {
                                checkCmd.Parameters.AddWithValue("@UserID", WorkmanSL);
                                int recordExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                                using (SqlCommand cmd = conn.CreateCommand())
                                {
                                    cmd.Transaction = tx;
                                    if (recordExists > 0)
                                    {
                                        // Update; only set AadhaarImagePath when a new file was uploaded
                                        cmd.CommandText = @"UPDATE AadhaarDetails SET AadhaarNumber = @AadhaarNumber, AadhaarName = @AadhaarName, IssueDate = @IssueDate" + (fuAadhaarImage.HasFile ? ", AadhaarImagePath = @AadhaarImagePath" : "") + " WHERE UserID = @UserID";

                                        if (fuAadhaarImage.HasFile)
                                            cmd.Parameters.AddWithValue("@AadhaarImagePath", savedRelativePath);
                                        lblAadhaarStatus.Text = "Aadhaar details updated successfully!";
                                    }
                                    else
                                    {
                                        // Insert; if no file uploaded, store empty string
                                        cmd.CommandText = @"
                                    INSERT INTO AadhaarDetails
                                    (AadhaarNumber, AadhaarName, IssueDate, AadhaarImagePath, CreatedOn, UserID)
                                    VALUES
                                    (@AadhaarNumber, @AadhaarName, @IssueDate, @AadhaarImagePath, GETDATE(), @UserID)";

                                        cmd.Parameters.AddWithValue("@AadhaarImagePath", (fuAadhaarImage.HasFile ? (object)savedRelativePath : (object)""));
                                        lblAadhaarStatus.Text = "Aadhaar details saved successfully!";
                                    }

                                    // Common params
                                    cmd.Parameters.AddWithValue("@AadhaarNumber", aadhaar);
                                    cmd.Parameters.AddWithValue("@AadhaarName", aadhaarName);
                                    cmd.Parameters.Add("@IssueDate", SqlDbType.Date).Value = issueDate;
                                    cmd.Parameters.AddWithValue("@UserID", WorkmanSL);

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // Update tbl_EmployeeDocsDetails — only pass aadhaarPath when a new file was uploaded
                            UpdateAadhaarDocs(conn, tx,
                                workmanSL: WorkmanSL,
                                aadhaarYesNo: 1,
                                aadhaarID: aadhaar,
                                aadhaarPath: (fuAadhaarImage.HasFile ? savedRelativePath : null),
                                aadhaarDate: issueDate,
                                updatedByWrk: WorkmanSL,
                                updatedByName: Session["FullName"]?.ToString()
                            );

                            tx.Commit();
                            lblAadhaarStatus.CssClass = "success-msg";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{lblAadhaarStatus.Text}');", true);
                        }
                        catch
                        {
                            try { tx.Rollback(); } catch { /* ignore rollback errors */ }
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblAadhaarStatus.Text = "Error: " + ex.Message;
                lblAadhaarStatus.CssClass = "error-msg";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('Error: {ex.Message.Replace("'", "\\'")}');", true);
            }
        }

        protected void btnSubmitPAN_Click(object sender, EventArgs e)
        {
            lblPanMessage.Text = "";
            txtPANNumber.CssClass = "form-control";
            txtPANName.CssClass = "form-control";
            txtPANIssueDate.CssClass = "form-control";

            if (Session["WORKMAN"] == null)
            {
                lblPanMessage.Text = "User not logged in.";
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('User not logged in.');", true);
                return;
            }

            string WorkmanSL = Session["WORKMAN"].ToString();
            string panNumber = txtPANNumber.Text.Trim();
            string panName = txtPANName.Text.Trim();
            string issueDateStr = txtPANIssueDate.Text.Trim();
            string savedRelativePath = "NA";
            bool isValid = true;

            // Validation: PAN Number (10 chars, alphanumeric)
            if (string.IsNullOrEmpty(panNumber) || panNumber.Length != 10)
            {
                txtPANNumber.CssClass += " error-border";
                isValid = false;
            }

            // Validation: Name
            if (string.IsNullOrEmpty(panName))
            {
                txtPANName.CssClass += " error-border";
                isValid = false;
            }

            // Validation: Date
            DateTime issueDate;
            if (!DateTime.TryParse(issueDateStr, out issueDate) || issueDate > DateTime.Today)
            {
                txtPANIssueDate.CssClass += " error-border";
                isValid = false;
            }

            if (!isValid)
            {
                lblPanMessage.Text = "Please correct the highlighted fields.";
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please correct the highlighted fields.');", true);
                return;
            }

            // Save Image if uploaded (use centralized helper)
            try
            {
                if (fuPANImage.HasFile)
                {
                    savedRelativePath = SaveEmployeeDocument(fuPANImage, WorkmanSL, "PAN"); // returns "/Uploads/{WorkmanSL}/PAN/filename" or "NA"
                }
            }
            catch (Exception exFile)
            {
                lblPanMessage.Text = "Error saving file: " + exFile.Message;
                lblPanMessage.CssClass = "error-msg";
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error saving file: {exFile.Message.Replace("'", "\\'")}');", true);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
                {
                    con.Open();
                    using (SqlTransaction tx = con.BeginTransaction())
                    {
                        try
                        {
                            SqlCommand cmd;

                            if (ViewState["UserPANID"] != null)
                            {
                                // UPDATE existing
                                int panID = Convert.ToInt32(ViewState["UserPANID"]);
                                string updateQuery = @"UPDATE UserPANCard
                                               SET PANNumber = @PANNumber,
                                                   PANName = @PANName,
                                                   IssueDate = @IssueDate" +
                                                       (fuPANImage.HasFile ? ", PANImagePath = @ImagePath" : "") +
                                                       " WHERE UserPANID = @UserPANID";
                                cmd = new SqlCommand(updateQuery, con, tx);
                                cmd.Parameters.AddWithValue("@UserPANID", panID);
                            }
                            else
                            {
                                // INSERT new
                                string insertQuery = @"INSERT INTO UserPANCard 
                                               (PANNumber, PANName, IssueDate, PANImagePath, CreatedOn, UserID)
                                               VALUES (@PANNumber, @PANName, @IssueDate, @ImagePath, GETDATE(), @UserID)";
                                cmd = new SqlCommand(insertQuery, con, tx);
                                cmd.Parameters.AddWithValue("@UserID", WorkmanSL);
                                // For insert, even if no file uploaded, set ImagePath to empty string
                                cmd.Parameters.AddWithValue("@ImagePath", fuPANImage.HasFile ? (object)savedRelativePath : (object)"");
                            }

                            // Common params
                            cmd.Parameters.AddWithValue("@PANNumber", panNumber);
                            cmd.Parameters.AddWithValue("@PANName", panName);
                            cmd.Parameters.AddWithValue("@IssueDate", issueDate);

                            // If updating and file uploaded, add parameter
                            if (ViewState["UserPANID"] != null && fuPANImage.HasFile)
                            {
                                cmd.Parameters.AddWithValue("@ImagePath", savedRelativePath);
                            }

                            cmd.ExecuteNonQuery();

                            // Update docs table — pass panPath only when a new file was uploaded (null otherwise)
                            UpdatePanDocs(con, tx,
                                workmanSL: WorkmanSL,
                                panYesNo: 1,
                                panNo: panNumber,
                                panPath: (fuPANImage.HasFile ? savedRelativePath : null),
                                panDate: issueDate,
                                updatedByWrk: WorkmanSL,
                                updatedByName: Session["FullName"]?.ToString()
                            );

                            tx.Commit();

                            string successMessage = (ViewState["UserPANID"] != null)
                                ? "PAN details updated successfully!"
                                : "PAN details saved successfully!";

                            lblPanMessage.Text = successMessage;
                            lblPanMessage.CssClass = "success-msg";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('{successMessage}');", true);
                            LoadPANDetails(); // reload data
                        }
                        catch (Exception)
                        {
                            try { tx.Rollback(); } catch { /* ignore rollback errors */ }
                            throw; // will be caught in outer catch
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblPanMessage.Text = "Error: " + ex.Message;
                lblPanMessage.CssClass = "error-msg";
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error: {ex.Message.Replace("'", "\\'")}');", true);
            }
        }

        private void LoadPANDetails()
        {
            string WorkmanSL = Session["WORKMAN"]?.ToString();
            if (string.IsNullOrEmpty(WorkmanSL))
            {
                lblPanMessage.Text = "Session expired. Please login again.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                string query = @"SELECT TOP 1 UserPANID, PANNumber, PANName, IssueDate, PANImagePath 
                         FROM UserPANCard
                         WHERE UserID = @UserID 
                         ORDER BY UserPANID DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", WorkmanSL);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtPANNumber.Text = reader["PANNumber"].ToString();
                        txtPANName.Text = reader["PANName"].ToString();

                        DateTime issueDate;
                        if (DateTime.TryParse(reader["IssueDate"].ToString(), out issueDate))
                        {
                            txtPANIssueDate.Text = issueDate.ToString("yyyy-MM-dd");
                        }

                        string imgPath = reader["PANImagePath"].ToString();
                        if (!string.IsNullOrEmpty(imgPath))
                        {
                            imgPanPreview.Src = ResolveUrl(imgPath);
                        }

                        ViewState["UserPANID"] = reader["UserPANID"];
                        btnSubmitPAN.Text = "Update";
                        btnSubmitPAN.CssClass = "btn btn-warning";
                    }
                }
            }
        }


        protected void btnSubmitBank_Click(object sender, EventArgs e)
        {
            string WorkmanSL = Session["WORKMAN"]?.ToString();
            if (string.IsNullOrEmpty(WorkmanSL))
            {
                lblBankMessage.Text = "Session expired. Please login again.";
                lblBankMessage.CssClass = "error-msg";
                ScriptManager.RegisterStartupScript(this, GetType(), "alertSession", "alert('Session expired. Please login again.');", true);
                return;
            }

            // Reset previous error visuals
            txtAccountNumber.CssClass = "form-control";
            txtIFSC.CssClass = "form-control";
            txtBankName.CssClass = "form-control";
            txtAccountHolder.CssClass = "form-control";
            lblBankMessage.Text = "";
            lblBankMessage.CssClass = "";

            bool isValid = true;

            // Account Number validation
            string accountNumber = txtAccountNumber.Text.Trim();
            if (string.IsNullOrEmpty(accountNumber) || !Regex.IsMatch(accountNumber, @"^\d{9,18}$"))
            {
                txtAccountNumber.CssClass += " error-border";
                lblBankMessage.Text += "Enter a valid account number (9 to 18 digits).\\n";
                isValid = false;
            }

            // IFSC code validation
            string ifsc = txtIFSC.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(ifsc) || !Regex.IsMatch(ifsc, @"^[A-Z]{4}[0-9A-Z]{7}$"))
            {
                txtIFSC.CssClass += " error-border";
                lblBankMessage.Text += "Enter a valid IFSC code.\\n";
                isValid = false;
            }

            // Bank Name validation
            string bankName = txtBankName.Text.Trim();
            if (string.IsNullOrEmpty(bankName))
            {
                txtBankName.CssClass += " error-border";
                lblBankMessage.Text += "Bank name is required.\\n";
                isValid = false;
            }

            // Account Holder Name validation
            string accountHolder = txtAccountHolder.Text.Trim();
            if (string.IsNullOrEmpty(accountHolder))
            {
                txtAccountHolder.CssClass += " error-border";
                lblBankMessage.Text += "Account holder name is required.\\n";
                isValid = false;
            }

            // Check if record exists (outside transaction is fine)
            int recordExists = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                conn.Open();
                string checkQuery = "SELECT COUNT(*) FROM UserBankAccount WHERE UserID = @UserID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@UserID", WorkmanSL);
                    recordExists = Convert.ToInt32(checkCmd.ExecuteScalar());
                }
            }

            // If no existing record, image is mandatory on initial save
            if (!fuBankImage.HasFile && recordExists == 0)
            {
                lblBankMessage.Text += "Please upload the bank document image.\\n";
                isValid = false;
            }

            if (!isValid)
            {
                lblBankMessage.CssClass = "error-msg";
                ScriptManager.RegisterStartupScript(this, GetType(), "alertValidation", $"alert('{lblBankMessage.Text}');", true);
                return;
            }

            // Save the image file if uploaded (do this before DB transaction)
            string savedRelativePath = "NA";
            if (fuBankImage.HasFile)
            {
                try
                {
                    savedRelativePath = SaveEmployeeDocument(fuBankImage, WorkmanSL, "Bank"); // returns "/Uploads/{WorkmanSL}/Bank/filename" or "NA"
                }
                catch (Exception exFile)
                {
                    lblBankMessage.Text = "Error saving file: " + exFile.Message;
                    lblBankMessage.CssClass = "error-msg";
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertFileError", $"alert('Error saving file: {exFile.Message.Replace("'", "\\'")}');", true);
                    return;
                }
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction tx = conn.BeginTransaction())
                    {
                        try
                        {
                            SqlCommand cmd;
                            if (recordExists > 0)
                            {
                                // Update existing
                                string updateSql = @"
                            UPDATE UserBankAccount
                            SET AccountNumber = @AccountNumber,
                                IFSCCode = @IFSCCode,
                                BankName = @BankName,
                                AccountHolderName = @AccountHolderName" +
                                        (fuBankImage.HasFile ? ", BankImagePath = @BankImagePath" : "") +
                                    " WHERE UserID = @UserID";

                                cmd = new SqlCommand(updateSql, conn, tx);
                                if (fuBankImage.HasFile)
                                    cmd.Parameters.AddWithValue("@BankImagePath", savedRelativePath);
                            }
                            else
                            {
                                // Insert new (store empty string if no file)
                                string insertSql = @"
                            INSERT INTO UserBankAccount
                            (AccountNumber, IFSCCode, BankName, BankImagePath, CreatedOn, AccountHolderName, UserID)
                            VALUES (@AccountNumber, @IFSCCode, @BankName, @BankImagePath, GETDATE(), @AccountHolderName, @UserID)";
                                cmd = new SqlCommand(insertSql, conn, tx);
                                cmd.Parameters.AddWithValue("@BankImagePath", fuBankImage.HasFile ? (object)savedRelativePath : (object)"");
                            }

                            // Common params
                            cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                            cmd.Parameters.AddWithValue("@IFSCCode", ifsc);
                            cmd.Parameters.AddWithValue("@BankName", bankName);
                            cmd.Parameters.AddWithValue("@AccountHolderName", accountHolder);
                            cmd.Parameters.AddWithValue("@UserID", WorkmanSL);

                            cmd.ExecuteNonQuery();

                            // Update docs table — pass bankPath only when a new file was uploaded
                            UpdateBankDocs(conn, tx,
                                workmanSL: WorkmanSL,
                                bankYesNo: 1,
                                bankPath: (fuBankImage.HasFile ? savedRelativePath : null),
                                bankDate: (fuBankImage.HasFile ? (DateTime?)DateTime.Now : null),
                                updatedByWrk: WorkmanSL,
                                updatedByName: Session["FullName"]?.ToString()
                            );

                            tx.Commit();
                            lblBankMessage.CssClass = "success-msg";
                            lblBankMessage.Text = recordExists > 0 ? "Bank details updated successfully!" : "Bank details saved successfully!";
                            ScriptManager.RegisterStartupScript(this, GetType(), "alertSuccess", $"alert('{lblBankMessage.Text}');", true);
                        }
                        catch
                        {
                            try { tx.Rollback(); } catch { /* ignore rollback errors */ }
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblBankMessage.Text = "Error: " + ex.Message.Replace("'", "\\'");
                lblBankMessage.CssClass = "error-msg";
                ScriptManager.RegisterStartupScript(this, GetType(), "alertError", $"alert('{lblBankMessage.Text}');", true);
            }
        }

        private void LoadBankDetails()
        {
            string WorkmanSL = Session["WORKMAN"] != null ? Session["WORKMAN"].ToString() : null;
            if (string.IsNullOrEmpty(WorkmanSL))
            {
                lblBankMessage.Text = "Session expired. Please login again.";
                lblBankMessage.CssClass = "error-msg";
                return;
            }

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString);
            string query = @"
        SELECT TOP 1 AccountNumber, IFSCCode, BankName, BankImagePath, AccountHolderName 
        FROM UserBankAccount 
        WHERE UserID = @UserID 
        ORDER BY BankID DESC";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", WorkmanSL);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    if (reader["AccountNumber"] != DBNull.Value)
                        txtAccountNumber.Text = reader["AccountNumber"].ToString();
                    else
                        txtAccountNumber.Text = "";

                    if (reader["IFSCCode"] != DBNull.Value)
                        txtIFSC.Text = reader["IFSCCode"].ToString();
                    else
                        txtIFSC.Text = "";

                    if (reader["BankName"] != DBNull.Value)
                        txtBankName.Text = reader["BankName"].ToString();
                    else
                        txtBankName.Text = "";

                    if (reader["AccountHolderName"] != DBNull.Value)
                        txtAccountHolder.Text = reader["AccountHolderName"].ToString();
                    else
                        txtAccountHolder.Text = "";

                    string imgPath = "";
                    if (reader["BankImagePath"] != DBNull.Value)
                        imgPath = reader["BankImagePath"].ToString();

                    string bankImgPath = reader["BankImagePath"]?.ToString();

                    if (!string.IsNullOrEmpty(bankImgPath))
                    {
                        imgBankPreview.Attributes["src"] = ResolveUrl(bankImgPath);
                    }
                    else
                    {
                        imgBankPreview.Attributes["src"] = ResolveUrl("~/Images/placeholder.png");
                    }


                    btnSubmitBank.Text = "Update";
                    btnSubmitBank.CssClass = "btn btn-warning";
                }
                else
                {
                    // Reset fields if no record found
                    txtAccountNumber.Text = "";
                    txtIFSC.Text = "";
                    txtBankName.Text = "";
                    txtAccountHolder.Text = "";
                    // imgBankPreview.ImageUrl = "~/Images/placeholder.png";
                    // imgBankPreview.Visible = true;

                    btnSubmitBank.Text = "Submit";
                    btnSubmitBank.CssClass = "btn btn-primary";
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                lblBankMessage.Text = "Error loading bank details: " + ex.Message;
                lblBankMessage.CssClass = "error-msg";
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
                conn.Dispose();
            }
        }
        protected void btnSubmitEducation_Click(object sender, EventArgs e)
        {
            ClearAllHighlights();
            lblEducationStatus.Text = "";
            lblEducationStatus.CssClass = "";

            string userID = Session["WORKMAN"] != null ? Session["WORKMAN"].ToString() : null;
            if (string.IsNullOrEmpty(userID))
            {
                lblEducationStatus.Text = "Session expired. Please login again.";
                lblEducationStatus.CssClass = "error-msg";
                ScriptManager.RegisterStartupScript(this, GetType(), "alertSession", "alert('Session expired. Please login again.');", true);
                return;
            }

            int currentYear = DateTime.Now.Year;
            bool hasError = false;

            // NA flags for all levels (10th/12th/UG/PG)
            bool is10NA = chk10NA?.Checked ?? false;
            bool is12NA = chk12NA?.Checked ?? false;
            bool isUGNA = chkUGNA?.Checked ?? false;
            bool isPGNA = chkPGNA?.Checked ?? false;

            // Try parsing years (use 0 when NA)
            int year10 = is10NA ? 0 : TryParseYear(txt10Year.Text.Trim());
            int year12 = is12NA ? 0 : TryParseYear(txt12Year.Text.Trim());
            int yearUG = isUGNA ? 0 : TryParseYear(txtUGYear.Text.Trim());
            int yearPG = isPGNA ? 0 : TryParseYear(txtPGYear.Text.Trim());

            // Validation
            // 10th: validate only if NOT N/A
            if (!is10NA)
            {
                if (!ValidateBasicFields(txt10Board, txt10Year, txt10Marks) || !IsValid10thYear(year10, currentYear))
                {
                    AddErrorHighlight(txt10Year);
                    hasError = true;
                }
            }

            // 12th: validate only if NOT N/A
            if (!is12NA)
            {
                // Pass year10 (may be 0 when 10th is NA) so IsValid12thYear can handle that scenario.
                if (!ValidateBasicFields(txt12Board, txt12Year, txt12Marks) || !IsValid12thYear(year10, year12, currentYear))
                {
                    AddErrorHighlight(txt12Year);
                    hasError = true;
                }
            }

            // UG: validate only if NOT N/A
            if (!isUGNA)
            {
                if (!ValidateBasicFields(txtUGBoard, txtUGYear, txtUGMarks) || !IsValidUGYear(year12, yearUG, currentYear))
                {
                    AddErrorHighlight(txtUGYear);
                    hasError = true;
                }
            }

            // PG: validate only if NOT N/A
            if (!isPGNA)
            {
                if (!ValidateBasicFields(txtPGBoard, txtPGYear, txtPGMarks) || !IsValidPGYear(yearUG, yearPG, currentYear))
                {
                    AddErrorHighlight(txtPGYear);
                    hasError = true;
                }
            }

            if (hasError)
            {
                lblEducationStatus.Text = "Please fix the highlighted fields.";
                lblEducationStatus.CssClass = "error-msg";
                ScriptManager.RegisterStartupScript(this, GetType(), "alertValidation", "alert('Please fix the highlighted fields.');", true);
                return;
            }

            // === Save uploaded files FIRST (outside transaction) ===
            string file10 = "NA", file12 = "NA", fileUG = "NA", filePG = "NA";
            try
            {
                if (!is10NA && fu10Image.HasFile)
                    file10 = SaveEmployeeDocument(fu10Image, userID, "Education/10th");

                if (!is12NA && fu12Image.HasFile)
                    file12 = SaveEmployeeDocument(fu12Image, userID, "Education/12th");

                if (!isUGNA && fuUGImage.HasFile)
                    fileUG = SaveEmployeeDocument(fuUGImage, userID, "Education/UG");

                if (!isPGNA && fuPGImage.HasFile)
                    filePG = SaveEmployeeDocument(fuPGImage, userID, "Education/PG");
            }
            catch (Exception exFile)
            {
                lblEducationStatus.Text = "Error saving file(s): " + exFile.Message;
                lblEducationStatus.CssClass = "error-msg";
                string safeMsg = exFile.Message.Replace("'", "\\'");
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "alertFileError",
                    "alert('Error saving file(s): " + safeMsg + "');",
                    true
                );
                return;
            }

            // === DB transaction: Save/Update rows and update docs table ===
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                con.Open();
                using (SqlTransaction tx = con.BeginTransaction())
                {
                    try
                    {
                        // For NA fields, pass "N/A" / "0" as you did for UG/PG earlier
                        string saved10 = SaveOrUpdateEducation(
                            con, tx, userID,
                            "10th", "SSC",
                            is10NA ? "N/A" : txt10Board.Text.Trim(),
                            is10NA ? "0" : txt10Year.Text.Trim(),
                            is10NA ? "N/A" : txt10Marks.Text.Trim(),
                            file10,
                            is10NA
                        );

                        string saved12 = SaveOrUpdateEducation(
                            con, tx, userID,
                            "12th", "HSC",
                            is12NA ? "N/A" : txt12Board.Text.Trim(),
                            is12NA ? "0" : txt12Year.Text.Trim(),
                            is12NA ? "N/A" : txt12Marks.Text.Trim(),
                            file12,
                            is12NA
                        );

                        string savedUG = SaveOrUpdateEducation(
                            con, tx, userID,
                            "UG", "B.Tech",
                            isUGNA ? "N/A" : txtUGBoard.Text.Trim(),
                            isUGNA ? "0" : txtUGYear.Text.Trim(),
                            isUGNA ? "N/A" : txtUGMarks.Text.Trim(),
                            fileUG,
                            isUGNA
                        );

                        string savedPG = SaveOrUpdateEducation(
                            con, tx, userID,
                            "PG", "M.Tech",
                            isPGNA ? "N/A" : txtPGBoard.Text.Trim(),
                            isPGNA ? "0" : txtPGYear.Text.Trim(),
                            isPGNA ? "N/A" : txtPGMarks.Text.Trim(),
                            filePG,
                            isPGNA
                        );

                        int tenFlag = (saved10 != null && saved10 != "NA") ? 1 : 0;
                        int twelveFlag = (saved12 != null && saved12 != "NA") ? 1 : 0;
                        int gradFlag = (savedUG != null && savedUG != "NA") ? 1 : 0;

                        // Update docs table (nullable-aware)
                        UpdateEducationDocs(con, tx,
                            workmanSL: userID,
                            tenYesNo: tenFlag, tenPath: (tenFlag == 1 ? saved10 : null), tenDate: tenFlag == 1 ? (DateTime?)DateTime.Now : null,
                            twelveYesNo: twelveFlag, twelvePath: (twelveFlag == 1 ? saved12 : null), twelveDate: twelveFlag == 1 ? (DateTime?)DateTime.Now : null,
                            gradYesNo: gradFlag, gradPath: (gradFlag == 1 ? savedUG : null), gradDate: gradFlag == 1 ? (DateTime?)DateTime.Now : null,
                            updatedByWrk: userID, updatedByName: Session["FullName"]?.ToString()
                        );


                        tx.Commit();

                        lblEducationStatus.CssClass = "success-msg";
                        lblEducationStatus.Text = "Education details saved successfully.";
                        btnSubmitEducation.Text = "Update";
                        btnSubmitEducation.CssClass = "btn btn-warning";
                        ScriptManager.RegisterStartupScript(this, GetType(), "alertSuccess", "alert('Education details saved successfully.');", true);
                    }
                    catch (Exception ex)
                    {
                        try { tx.Rollback(); } catch { /* ignore rollback exceptions */ }
                        lblEducationStatus.Text = "Error saving education details: " + ex.Message;
                        lblEducationStatus.CssClass = "error-msg";
                        string safeMessage = ex.Message.Replace("'", "\\'").Replace(Environment.NewLine, " ");
                        ScriptManager.RegisterStartupScript(this, GetType(), "alertException", $"alert('Error saving education details: {safeMessage}');", true);
                    }
                }
            }
        }

        private string SaveOrUpdateEducation(SqlConnection con, SqlTransaction tx,
            string userID,
            string qualificationType,   // "10th", "12th", "UG", "PG"
            string qualification,       // "SSC", "HSC", "B.Tech", "M.Tech"
            string boardOrInstitute,
            string passingYear,
            string marksOrGrade,
            string documentPath,        // path returned by SaveEmployeeDocument or "NA"
            bool isNA)
        {
            // Normalize documentPath for DB storage: "" when none
            string storePath = (string.IsNullOrWhiteSpace(documentPath) || documentPath == "NA") ? "" : documentPath;

            string checkSql = "SELECT COUNT(*) FROM EducationDetails WHERE UserID = @UserID AND QualificationType = @QualificationType";
            using (SqlCommand cmdCheck = new SqlCommand(checkSql, con, tx))
            {
                cmdCheck.Parameters.AddWithValue("@UserID", userID);
                cmdCheck.Parameters.AddWithValue("@QualificationType", qualificationType);
                int exists = Convert.ToInt32(cmdCheck.ExecuteScalar());

                if (exists > 0)
                {
                    string updateSql = @"
                UPDATE EducationDetails
                SET Qualification = @Qualification,
                    BoardOrInstitute = @BoardOrInstitute,
                    PassingYear = @PassingYear,
                    MarksOrGrade = @MarksOrGrade,
                    DocumentPath = ISNULL(NULLIF(@DocumentPath, ''), DocumentPath),
                    IsNA = @IsNA
                WHERE UserID = @UserID AND QualificationType = @QualificationType";
                    using (SqlCommand cmdUpd = new SqlCommand(updateSql, con, tx))
                    {
                        cmdUpd.Parameters.AddWithValue("@Qualification", (object)qualification ?? DBNull.Value);
                        cmdUpd.Parameters.AddWithValue("@BoardOrInstitute", (object)boardOrInstitute ?? DBNull.Value);
                        cmdUpd.Parameters.AddWithValue("@PassingYear", (object)passingYear ?? DBNull.Value);
                        cmdUpd.Parameters.AddWithValue("@MarksOrGrade", (object)marksOrGrade ?? DBNull.Value);
                        cmdUpd.Parameters.AddWithValue("@DocumentPath", storePath);
                        cmdUpd.Parameters.AddWithValue("@IsNA", isNA ? 1 : 0);
                        cmdUpd.Parameters.AddWithValue("@UserID", userID);
                        cmdUpd.Parameters.AddWithValue("@QualificationType", qualificationType);
                        cmdUpd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string insertSql = @"
                INSERT INTO EducationDetails
                (QualificationType, Qualification, BoardOrInstitute, MarksOrGrade, PassingYear, DocumentPath, UserID, IsNA)
                VALUES
                (@QualificationType, @Qualification, @BoardOrInstitute, @MarksOrGrade, @PassingYear, @DocumentPath, @UserID, @IsNA)";
                    using (SqlCommand cmdIns = new SqlCommand(insertSql, con, tx))
                    {
                        cmdIns.Parameters.AddWithValue("@QualificationType", qualificationType);
                        cmdIns.Parameters.AddWithValue("@Qualification", (object)qualification ?? DBNull.Value);
                        cmdIns.Parameters.AddWithValue("@BoardOrInstitute", (object)boardOrInstitute ?? DBNull.Value);
                        cmdIns.Parameters.AddWithValue("@MarksOrGrade", (object)marksOrGrade ?? DBNull.Value);
                        cmdIns.Parameters.AddWithValue("@PassingYear", (object)passingYear ?? DBNull.Value);
                        cmdIns.Parameters.AddWithValue("@DocumentPath", storePath);
                        cmdIns.Parameters.AddWithValue("@UserID", userID);
                        cmdIns.Parameters.AddWithValue("@IsNA", isNA ? 1 : 0);
                        cmdIns.ExecuteNonQuery();
                    }
                }
            }

            return storePath == "" ? "NA" : storePath;
        }

        // 🔹 Helper: Basic field validation
        private bool ValidateBasicFields(TextBox board, TextBox year, TextBox marks)
        {
            bool isValid = true;

            if (board.Text == null || board.Text.Trim() == "")
            {
                AddErrorHighlight(board);
                isValid = false;
            }

            int parsedYear;
            if (year.Text == null || year.Text.Trim() == "" || !int.TryParse(year.Text.Trim(), out parsedYear))
            {
                AddErrorHighlight(year);
                isValid = false;
            }

            if (marks.Text == null || marks.Text.Trim() == "")
            {
                AddErrorHighlight(marks);
                isValid = false;
            }

            return isValid;
        }


        // 🔹 Helper: Convert year string to int
        private int TryParseYear(string yearText)
        {
            int year;
            return int.TryParse(yearText, out year) ? year : 0;
        }

        // 🔹 Year validations
        private bool IsValid10thYear(int year10, int currentYear)
        {
            return year10 > 1900 && year10 < currentYear;
        }

        private bool IsValid12thYear(int year10, int year12, int currentYear)
        {
            return year12 >= year10 + 2 && year12 < currentYear;
        }

        private bool IsValidUGYear(int year12, int yearUG, int currentYear)
        {
            return yearUG >= year12 + 3 && yearUG <= currentYear;
        }

        private bool IsValidPGYear(int yearUG, int yearPG, int currentYear)
        {
            return yearPG >= yearUG + 2 && yearPG <= currentYear;
        }


        private void UpdateAadhaarDocs(SqlConnection conn, SqlTransaction tx,
    string workmanSL, int aadhaarYesNo, string aadhaarID, string aadhaarPath, DateTime? aadhaarDate,
    string updatedByWrk = null, string updatedByName = null)
        {
            // Only Aadhaar-related params set; others null
            UpsertEmployeeDocsDetails(conn, tx,
                workmanSL,
                aadhaarYesNo: aadhaarYesNo, aadhaarID: aadhaarID, aadhaarPath: aadhaarPath, aadhaarDate: aadhaarDate,
                bankYesNo: null, bankPath: null, bankDate: null,
                panYesNo: null, panNo: null, panPath: null, panDate: null,
                tenYesNo: null, tenPath: null, tenDate: null,
                twelveYesNo: null, twelvePath: null, twelveDate: null,
                gradYesNo: null, gradPath: null, gradDate: null,
                updatedByWrk: updatedByWrk, updatedByName: updatedByName
            );
        }

        private void UpdatePanDocs(SqlConnection conn, SqlTransaction tx,
            string workmanSL, int panYesNo, string panNo, string panPath, DateTime? panDate,
            string updatedByWrk = null, string updatedByName = null)
        {
            UpsertEmployeeDocsDetails(conn, tx,
                workmanSL,
                aadhaarYesNo: null, aadhaarID: null, aadhaarPath: null, aadhaarDate: null,
                bankYesNo: null, bankPath: null, bankDate: null,
                panYesNo: panYesNo, panNo: panNo, panPath: panPath, panDate: panDate,
                tenYesNo: null, tenPath: null, tenDate: null,
                twelveYesNo: null, twelvePath: null, twelveDate: null,
                gradYesNo: null, gradPath: null, gradDate: null,
                updatedByWrk: updatedByWrk, updatedByName: updatedByName
            );
        }

        private void UpdateBankDocs(SqlConnection conn, SqlTransaction tx,
            string workmanSL, int bankYesNo, string bankPath, DateTime? bankDate,
            string updatedByWrk = null, string updatedByName = null)
        {
            UpsertEmployeeDocsDetails(conn, tx,
                workmanSL,
                aadhaarYesNo: null, aadhaarID: null, aadhaarPath: null, aadhaarDate: null,
                bankYesNo: bankYesNo, bankPath: bankPath, bankDate: bankDate,
                panYesNo: null, panNo: null, panPath: null, panDate: null,
                tenYesNo: null, tenPath: null, tenDate: null,
                twelveYesNo: null, twelvePath: null, twelveDate: null,
                gradYesNo: null, gradPath: null, gradDate: null,
                updatedByWrk: updatedByWrk, updatedByName: updatedByName
            );
        }

        private void UpdateEducationDocs(SqlConnection conn, SqlTransaction tx, string workmanSL, int? tenYesNo, string tenPath, DateTime? tenDate, int? twelveYesNo, string twelvePath, DateTime? twelveDate, int? gradYesNo, string gradPath, DateTime? gradDate, string updatedByWrk = null, string updatedByName = null)
        {
            UpsertEmployeeDocsDetails(conn, tx,
                workmanSL,
                aadhaarYesNo: null, aadhaarID: null, aadhaarPath: null, aadhaarDate: null,
                bankYesNo: null, bankPath: null, bankDate: null,
                panYesNo: null, panNo: null, panPath: null, panDate: null,
                tenYesNo: tenYesNo, tenPath: tenPath, tenDate: tenDate,
                twelveYesNo: twelveYesNo, twelvePath: twelvePath, twelveDate: twelveDate,
                gradYesNo: gradYesNo, gradPath: gradPath, gradDate: gradDate,
                updatedByWrk: updatedByWrk, updatedByName: updatedByName
            );
        }

        private void UpsertEmployeeDocsDetails(SqlConnection conn, SqlTransaction tx,
            string workmanSL,
            int? aadhaarYesNo, string aadhaarID, string aadhaarPath, DateTime? aadhaarDate,
            int? bankYesNo, string bankPath, DateTime? bankDate,
            int? panYesNo, string panNo, string panPath, DateTime? panDate,
            int? tenYesNo, string tenPath, DateTime? tenDate,
            int? twelveYesNo, string twelvePath, DateTime? twelveDate,
            int? gradYesNo, string gradPath, DateTime? gradDate,
            string updatedByWrk = null, string updatedByName = null)
        {
            try
            {
                string checkSql = "SELECT COUNT(*) FROM tbl_EmployeeDocsDetails WHERE WorkmanSL = @WorkmanSL";
                using (SqlCommand checkCmd = new SqlCommand(checkSql, conn, tx))
                {
                    checkCmd.Parameters.AddWithValue("@WorkmanSL", workmanSL);
                    int cnt = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (cnt > 0)
                    {
                        string updateSql = @"
                            UPDATE tbl_EmployeeDocsDetails SET
                                AadhaarYesNo = ISNULL(@AadhaarYesNo, AadhaarYesNo),
                                AadhaarID = COALESCE(@AadhaarID, AadhaarID),
                                AddhaarPath = COALESCE(@AddhaarPath, AddhaarPath),
                                AddhaarDate = ISNULL(@AddhaarDate, AddhaarDate),
                                BankYesNo = ISNULL(@BankYesNo, BankYesNo),
                                BankPath = COALESCE(@BankPath, BankPath),
                                BankDate = ISNULL(@BankDate, BankDate),
                                PanYesNo = ISNULL(@PanYesNo, PanYesNo),
                                PanNo = COALESCE(@PanNo, PanNo),
                                PanPath = COALESCE(@PanPath, PanPath),
                                PanDate = ISNULL(@PanDate, PanDate),
                                TenYesNo = ISNULL(@TenYesNo, TenYesNo),
                                TenPath = COALESCE(@TenPath, TenPath),
                                TenDate = ISNULL(@TenDate, TenDate),
                                TwelveYesNo = ISNULL(@TwelveYesNo, TwelveYesNo),
                                TwelvePath = COALESCE(@TwelvePath, TwelvePath),
                                TwelveDate = ISNULL(@TwelveDate, TwelveDate),
                                GraduationYesNo = ISNULL(@GraduationYesNo, GraduationYesNo),
                                GradPath = COALESCE(@GradPath, GradPath),
                                GradDate = ISNULL(@GradDate, GradDate),
                                UpdatedByWrk = COALESCE(@UpdatedByWrk, UpdatedByWrk),
                                UpdatedByName = COALESCE(@UpdatedByName, UpdatedByName),
                                Timestamp = GETDATE(), RejectionNote = NULL
                            WHERE WorkmanSL = @WorkmanSL";

                        using (SqlCommand upd = new SqlCommand(updateSql, conn, tx))
                        {
                            upd.Parameters.AddWithValue("@AadhaarYesNo", (object)aadhaarYesNo ?? DBNull.Value);
                            upd.Parameters.AddWithValue("@AadhaarID", (object)(aadhaarID ?? (string)null) ?? DBNull.Value);
                            upd.Parameters.AddWithValue("@AddhaarPath", (object)(aadhaarPath ?? (string)null) ?? DBNull.Value);
                            upd.Parameters.AddWithValue("@AddhaarDate", (object)aadhaarDate ?? DBNull.Value);

                            upd.Parameters.AddWithValue("@BankYesNo", (object)bankYesNo ?? DBNull.Value);
                            upd.Parameters.AddWithValue("@BankPath", (object)(bankPath ?? (string)null) ?? DBNull.Value);
                            upd.Parameters.AddWithValue("@BankDate", (object)bankDate ?? DBNull.Value);

                            upd.Parameters.AddWithValue("@PanYesNo", (object)panYesNo ?? DBNull.Value);
                            upd.Parameters.AddWithValue("@PanNo", (object)(panNo ?? (string)null) ?? DBNull.Value);
                            upd.Parameters.AddWithValue("@PanPath", (object)(panPath ?? (string)null) ?? DBNull.Value);
                            upd.Parameters.AddWithValue("@PanDate", (object)panDate ?? DBNull.Value);

                            upd.Parameters.AddWithValue("@TenYesNo", (object)tenYesNo ?? DBNull.Value);
                            upd.Parameters.AddWithValue("@TenPath", (object)(tenPath ?? (string)null) ?? DBNull.Value);
                            upd.Parameters.AddWithValue("@TenDate", (object)tenDate ?? DBNull.Value);

                            upd.Parameters.AddWithValue("@TwelveYesNo", (object)twelveYesNo ?? DBNull.Value);
                            upd.Parameters.AddWithValue("@TwelvePath", (object)(twelvePath ?? (string)null) ?? DBNull.Value);
                            upd.Parameters.AddWithValue("@TwelveDate", (object)twelveDate ?? DBNull.Value);

                            upd.Parameters.AddWithValue("@GraduationYesNo", (object)gradYesNo ?? DBNull.Value);
                            upd.Parameters.AddWithValue("@GradPath", (object)(gradPath ?? (string)null) ?? DBNull.Value);
                            upd.Parameters.AddWithValue("@GradDate", (object)gradDate ?? DBNull.Value);

                            upd.Parameters.AddWithValue("@UpdatedByWrk", (object)(updatedByWrk ?? (string)null) ?? DBNull.Value);
                            upd.Parameters.AddWithValue("@UpdatedByName", (object)(updatedByName ?? (string)null) ?? DBNull.Value);
                            upd.Parameters.AddWithValue("@WorkmanSL", workmanSL);

                            upd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertSql = @"
                            INSERT INTO tbl_EmployeeDocsDetails
                            (WorkmanSL, AadhaarYesNo, AadhaarID, AddhaarPath, AddhaarDate,
                             BankYesNo, BankPath, BankDate,
                             PanYesNo, PanNo, PanPath, PanDate,
                             TenYesNo, TenPath, TenDate,
                             TwelveYesNo, TwelvePath, TwelveDate,
                             GraduationYesNo, GradPath, GradDate,
                             UpdatedByWrk, UpdatedByName, Timestamp)
                            VALUES
                            (@WorkmanSL, ISNULL(@AadhaarYesNo,0), @AadhaarID, @AddhaarPath, @AddhaarDate,
                             ISNULL(@BankYesNo,0), @BankPath, @BankDate,
                             ISNULL(@PanYesNo,0), @PanNo, @PanPath, @PanDate,
                             ISNULL(@TenYesNo,0), @TenPath, @TenDate,
                             ISNULL(@TwelveYesNo,0), @TwelvePath, @TwelveDate,
                             ISNULL(@GraduationYesNo,0), @GradPath, @GradDate,
                             @UpdatedByWrk, @UpdatedByName, GETDATE())";

                        using (SqlCommand ins = new SqlCommand(insertSql, conn, tx))
                        {
                            ins.Parameters.AddWithValue("@WorkmanSL", workmanSL);
                            ins.Parameters.AddWithValue("@AadhaarYesNo", (object)aadhaarYesNo ?? DBNull.Value);
                            ins.Parameters.AddWithValue("@AadhaarID", (object)(aadhaarID ?? (string)null) ?? DBNull.Value);
                            ins.Parameters.AddWithValue("@AddhaarPath", (object)(aadhaarPath ?? (string)null) ?? DBNull.Value);
                            ins.Parameters.AddWithValue("@AddhaarDate", (object)aadhaarDate ?? DBNull.Value);

                            ins.Parameters.AddWithValue("@BankYesNo", (object)bankYesNo ?? DBNull.Value);
                            ins.Parameters.AddWithValue("@BankPath", (object)(bankPath ?? (string)null) ?? DBNull.Value);
                            ins.Parameters.AddWithValue("@BankDate", (object)bankDate ?? DBNull.Value);

                            ins.Parameters.AddWithValue("@PanYesNo", (object)panYesNo ?? DBNull.Value);
                            ins.Parameters.AddWithValue("@PanNo", (object)(panNo ?? (string)null) ?? DBNull.Value);
                            ins.Parameters.AddWithValue("@PanPath", (object)(panPath ?? (string)null) ?? DBNull.Value);
                            ins.Parameters.AddWithValue("@PanDate", (object)panDate ?? DBNull.Value);

                            ins.Parameters.AddWithValue("@TenYesNo", (object)tenYesNo ?? DBNull.Value);
                            ins.Parameters.AddWithValue("@TenPath", (object)(tenPath ?? (string)null) ?? DBNull.Value);
                            ins.Parameters.AddWithValue("@TenDate", (object)tenDate ?? DBNull.Value);

                            ins.Parameters.AddWithValue("@TwelveYesNo", (object)twelveYesNo ?? DBNull.Value);
                            ins.Parameters.AddWithValue("@TwelvePath", (object)(twelvePath ?? (string)null) ?? DBNull.Value);
                            ins.Parameters.AddWithValue("@TwelveDate", (object)twelveDate ?? DBNull.Value);

                            ins.Parameters.AddWithValue("@GraduationYesNo", (object)gradYesNo ?? DBNull.Value);
                            ins.Parameters.AddWithValue("@GradPath", (object)(gradPath ?? (string)null) ?? DBNull.Value);
                            ins.Parameters.AddWithValue("@GradDate", (object)gradDate ?? DBNull.Value);

                            ins.Parameters.AddWithValue("@UpdatedByWrk", (object)(updatedByWrk ?? (string)null) ?? DBNull.Value);
                            ins.Parameters.AddWithValue("@UpdatedByName", (object)(updatedByName ?? (string)null) ?? DBNull.Value);

                            ins.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    System.Diagnostics.Trace.TraceError($"UpsertEmployeeDocsDetails failed for WorkmanSL={workmanSL}: {ex}");
                }
                catch { /* ignore logging errors */ }

                throw;
            }
        }

        private void DeleteEducationRecord(SqlConnection con, string userID, string qualificationType)
        {
            string deleteQuery = "DELETE FROM EducationDetails WHERE UserID = @UserID AND QualificationType = @QualificationType";
            SqlCommand deleteCmd = new SqlCommand(deleteQuery, con);
            deleteCmd.Parameters.AddWithValue("@UserID", userID);
            deleteCmd.Parameters.AddWithValue("@QualificationType", qualificationType);
            deleteCmd.ExecuteNonQuery();
        }

        private string GetUploadedFileName(FileUpload fileUpload, string qualificationType)
        {
            if (fileUpload.HasFile)
            {
                string folder = Server.MapPath("~/Upload/Docs/");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string originalFile = Path.GetFileName(fileUpload.FileName);
                string fileName = qualificationType + "_" + DateTime.Now.Ticks + "_" + originalFile;
                string filePath = Path.Combine(folder, fileName);
                fileUpload.SaveAs(filePath);

                return fileName;
            }
            return "NA";
        }

        private void AddErrorHighlight(Control ctrl)
        {
            if (ctrl is TextBox)
                ((TextBox)ctrl).CssClass += " error-highlight";
        }

        private void ClearAllHighlights()
        {
            txt10Board.CssClass = txt10Board.CssClass.Replace(" error-highlight", "");
            txt10Year.CssClass = txt10Year.CssClass.Replace(" error-highlight", "");
            txt10Marks.CssClass = txt10Marks.CssClass.Replace(" error-highlight", "");

            txt12Board.CssClass = txt12Board.CssClass.Replace(" error-highlight", "");
            txt12Year.CssClass = txt12Year.CssClass.Replace(" error-highlight", "");
            txt12Marks.CssClass = txt12Marks.CssClass.Replace(" error-highlight", "");

            txtUGBoard.CssClass = txtUGBoard.CssClass.Replace(" error-highlight", "");
            txtUGYear.CssClass = txtUGYear.CssClass.Replace(" error-highlight", "");
            txtUGMarks.CssClass = txtUGMarks.CssClass.Replace(" error-highlight", "");

            txtPGBoard.CssClass = txtPGBoard.CssClass.Replace(" error-highlight", "");
            txtPGYear.CssClass = txtPGYear.CssClass.Replace(" error-highlight", "");
            txtPGMarks.CssClass = txtPGMarks.CssClass.Replace(" error-highlight", "");
        }

        private void LoadEducationDetails()
        {
            string userID = Session["WORKMAN"]?.ToString();
            if (string.IsNullOrEmpty(userID))
            {
                lblEducationStatus.Text = "Session expired. Please login again.";
                lblEducationStatus.CssClass = "error-msg";
                return;
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                string query = @"SELECT QualificationType, BoardOrInstitute, PassingYear, MarksOrGrade, DocumentPath, IsNA FROM EducationDetails WHERE UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    try
                    {
                        con.Open();
                        ClearEducationFields();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string qualType = reader["QualificationType"]?.ToString();
                                string board = reader["BoardOrInstitute"]?.ToString();
                                string passoutYear = reader["PassingYear"]?.ToString();
                                string marks = reader["MarksOrGrade"]?.ToString();
                                string docPath = reader["DocumentPath"]?.ToString();
                                bool isNA = reader["IsNA"] != DBNull.Value && Convert.ToBoolean(reader["IsNA"]);

                                // Combine path if valid
                                string imgPath = !string.IsNullOrEmpty(docPath) && docPath != "NA"
                                    ? "~/Upload/Docs/" + docPath
                                    : "~/Images/default-doc.png";
                                string resolvedPath = ResolveUrl(docPath);

                                switch (qualType)
                                {
                                    case "10th":
                                        txt10Board.Text = board;
                                        txt10Year.Text = passoutYear;
                                        txt10Marks.Text = marks;
                                        img10Preview.Attributes["src"] = resolvedPath;
                                        break;

                                    case "12th":
                                        txt12Board.Text = board;
                                        txt12Year.Text = passoutYear;
                                        txt12Marks.Text = marks;
                                        img12Preview.Attributes["src"] = resolvedPath;
                                        break;

                                    case "UG":
                                        if (isNA)
                                        {
                                            chkUGNA.Checked = true;
                                            txtUGBoard.Text = "N/A";
                                            txtUGYear.Text = "N/A";
                                            txtUGMarks.Text = "N/A";
                                        }
                                        else
                                        {
                                            chkUGNA.Checked = false;
                                            txtUGBoard.Text = board;
                                            txtUGYear.Text = passoutYear;
                                            txtUGMarks.Text = marks;
                                        }
                                        imgUGPreview.Attributes["src"] = resolvedPath;
                                        break;

                                    case "PG":
                                        if (isNA)
                                        {
                                            chkPGNA.Checked = true;
                                            txtPGBoard.Text = "N/A";
                                            txtPGYear.Text = "N/A";
                                            txtPGMarks.Text = "N/A";
                                        }
                                        else
                                        {
                                            chkPGNA.Checked = false;
                                            txtPGBoard.Text = board;
                                            txtPGYear.Text = passoutYear;
                                            txtPGMarks.Text = marks;
                                        }
                                        imgPGPreview.Attributes["src"] = resolvedPath;
                                        break;
                                }
                            }
                        }

                        btnSubmitEducation.Text = "Update";
                        btnSubmitEducation.CssClass = "btn btn-warning";
                    }
                    catch (Exception ex)
                    {
                        lblEducationStatus.Text = "Error loading Education details: " + ex.Message;
                        lblEducationStatus.CssClass = "error-msg";
                    }
                }
            }
        }

        private void ClearEducationFields()
        {
            txt10Board.Text = "";
            txt10Year.Text = "";
            txt10Marks.Text = "";

            txt12Board.Text = "";
            txt12Year.Text = "";
            txt12Marks.Text = "";

            txtUGBoard.Text = "";
            txtUGYear.Text = "";
            txtUGMarks.Text = "";
            chkUGNA.Checked = false;

            txtPGBoard.Text = "";
            txtPGYear.Text = "";
            txtPGMarks.Text = "";
            chkPGNA.Checked = false;

            btnSubmitEducation.Text = "Submit";
            btnSubmitEducation.CssClass = "btn btn-primary";
        }




        private string SaveEmployeeDocument_old(FileUpload fileUpload, string workmanSL, string docType)
        {
            if (fileUpload == null || !fileUpload.HasFile)
                return "NA";

            // Root → ~/Uploads/{WorkmanSL}/{DocType}/
            string folderPath = Server.MapPath($"~/Uploads/{workmanSL}/{docType}/");

            // Create directory if not exists
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Unique file name
            string extension = Path.GetExtension(fileUpload.FileName);
            string fileName = docType + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + extension;

            string fullPath = Path.Combine(folderPath, fileName);

            // Save file
            fileUpload.SaveAs(fullPath);

            // Return relative path for DB storage
            return $"/Uploads/{workmanSL}/{docType}/{fileName}";
        }

        private string SaveEmployeeDocument(FileUpload fileUpload, string workmanSL, string docType)
        {
            // Defensive checks
            if (fileUpload == null || !fileUpload.HasFile) return "NA";
            if (string.IsNullOrWhiteSpace(workmanSL)) throw new ArgumentException("workmanSL is required", nameof(workmanSL));

            // Normalize docType: remove leading/trailing slashes and backslashes
            docType = (docType ?? "").Trim();
            docType = docType.TrimStart('~', '/', '\\').TrimEnd('/', '\\');

            // Replace forward slashes with platform directory separator so nested folders like "Education/10th" work
            string safeDocType = docType.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);

            // Build folder path once: ~/Uploads/{workmanSL}/{safeDocType}/
            string virtualFolder = $"/Uploads/{workmanSL}";
            if (!string.IsNullOrEmpty(safeDocType))
                virtualFolder = virtualFolder + "/" + safeDocType.Replace(Path.DirectorySeparatorChar, '/'); // web-friendly

            string folderPath = Server.MapPath("~" + virtualFolder + "/"); // ensures mapped only once

            // Create directory if not exists
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Sanitize original filename (remove invalid chars)
            string orig = Path.GetFileName(fileUpload.FileName);
            foreach (char c in Path.GetInvalidFileNameChars())
                orig = orig.Replace(c, '_');

            // Build unique filename: {docTypePart}_{WorkmanSL}_{yyyyMMdd_HHmmss}_{random}{ext}
            string docShort = string.IsNullOrEmpty(safeDocType) ? "file" : safeDocType.Replace(Path.DirectorySeparatorChar, '_');
            string ext = Path.GetExtension(orig);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string rnd = new Random().Next(1000, 9999).ToString();
            string fileName = $"{docShort}_{workmanSL}_{timestamp}_{rnd}{ext}";

            string fullPath = Path.Combine(folderPath, fileName);

            // Save file (this may throw if disk/permissions problem)
            fileUpload.SaveAs(fullPath);

            // Return web-relative path (use forward slashes)
            string relativeWebPath = virtualFolder + "/" + fileName; // e.g. /Uploads/J8/Education/10th/file.jpg
            return relativeWebPath;
        }


    }
}