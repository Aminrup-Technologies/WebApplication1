using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using OfficeOpenXml;
using System.Configuration;

namespace WebApplication1.bussiness.production
{
    public partial class bulk_employeeupdate : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindColumnsToGridView();
            }
        }

        private void BindColumnsToGridView()
        {
            string query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tbl_Employee_Mustertable'";
            DataTable dtColumns = new DataTable();
            dbcl.Sqlconnection();
            using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
            {
                dbcl.ConnectDb();
                SqlDataReader reader = cmd.ExecuteReader();
                dtColumns.Load(reader);
                reader.Close();
            }
            dbcl.DisconnectDb();

            GridViewColumns.DataSource = dtColumns;
            GridViewColumns.DataBind();
        }

        protected void btnMapColumns_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> columnMapping = new Dictionary<string, string>();

            foreach (GridViewRow row in GridViewColumnMapping.Rows)
            {
                string dbColumn = row.Cells[0].Text;
                DropDownList ddlExcelColumns = (DropDownList)row.FindControl("ddlExcelColumns");

                if (ddlExcelColumns != null && !string.IsNullOrEmpty(ddlExcelColumns.SelectedValue))
                {
                    columnMapping.Add(dbColumn, ddlExcelColumns.SelectedValue);
                }
            }

            if (columnMapping.Count > 0)
            {
                Session["ColumnMapping"] = columnMapping;
                lbl_msg2.Text = "Columns mapped successfully!";
                lbl_msg2.CssClass = "text-success";
            }
            else
            {
                lbl_msg2.Text = "Please map at least one column.";
                lbl_msg2.CssClass = "text-danger";
            }
        }

        private DataTable ReadExcelFile(string filePath)
        {
            DataTable dt = new DataTable();

            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                if (package.Workbook.Worksheets.Count == 0)
                {
                    throw new Exception("The uploaded Excel file contains no worksheets.");
                }

                ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];

                if (worksheet.Dimension == null)
                {
                    throw new Exception("The worksheet is empty.");
                }

                int colCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;

                for (int col = 1; col <= colCount; col++)
                {
                    dt.Columns.Add(worksheet.Cells[1, col].Text);
                }

                for (int row = 2; row <= rowCount; row++)
                {
                    DataRow dr = dt.NewRow();
                    for (int col = 1; col <= colCount; col++)
                    {
                        dr[col - 1] = worksheet.Cells[row, col].Text;
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        private void BindExcelColumnsToDropdown(DataTable excelData)
        {
            List<string> excelColumns = excelData.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToList();

            foreach (GridViewRow row in GridViewColumnMapping.Rows)
            {
                DropDownList ddlExcelColumns = (DropDownList)row.FindControl("ddlExcelColumns");
                if (ddlExcelColumns != null)
                {
                    ddlExcelColumns.DataSource = excelColumns;
                    ddlExcelColumns.DataBind();
                    ddlExcelColumns.Items.Insert(0, new ListItem("Select", ""));
                }
            }
        }

        protected void BindMappingGrid(List<string> selectedColumns, List<string> excelColumns)
        {
            DataTable dtMapping = new DataTable();
            dtMapping.Columns.Add("DatabaseColumn");

            foreach (string column in selectedColumns)
            {
                DataRow row = dtMapping.NewRow();
                row["DatabaseColumn"] = column;
                dtMapping.Rows.Add(row);
            }

            GridViewColumnMapping.DataSource = dtMapping;
            GridViewColumnMapping.DataBind();

            foreach (GridViewRow row in GridViewColumnMapping.Rows)
            {
                DropDownList ddlExcelColumns = (DropDownList)row.FindControl("ddlExcelColumns");
                if (ddlExcelColumns != null)
                {
                    ddlExcelColumns.DataSource = excelColumns;
                    ddlExcelColumns.DataBind();
                }
            }
        }

        private void BindMappingGrid(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                GridViewColumnMapping.DataSource = dt;
                GridViewColumnMapping.DataBind();
            }
            else
            {
                lbl_msg.Text = "No data found in the uploaded file.";
                lbl_msg.CssClass = "text-danger";
            }
        }

        protected void btnUploadExcel_Click(object sender, EventArgs e)
        {
            if (FileUploadExcel.HasFile)
            {
                string filePath = Server.MapPath("~/erp_images/EmpUpdate/" + FileUploadExcel.FileName);
                FileUploadExcel.SaveAs(filePath);

                DataTable excelData = ReadExcelFile(filePath);
                Session["ExcelData"] = excelData;
                BindExcelColumnsToDropdown(excelData);
                BindMappingGrid(excelData);

                lblUploadMessage.Text = "File uploaded and data loaded successfully!";
                lblUploadMessage.CssClass = "text-success";
            }
            else
            {
                lblUploadMessage.Text = "Please upload an Excel file.";
                lblUploadMessage.CssClass = "text-danger";
            }
        }

        protected void btnMapColumnsAndUpload_Click(object sender, EventArgs e)
        {
            List<string> selectedColumns = new List<string>();

            foreach (GridViewRow row in GridViewColumns.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect != null && chkSelect.Checked)
                {
                    string columnName = row.Cells[1].Text;
                    selectedColumns.Add(columnName);
                }
            }

            if (selectedColumns.Count > 0)
            {
                if (FileUploadExcel.HasFile)
                {
                    string filePath = Server.MapPath("~/erp_images/EmpUpdate/" + FileUploadExcel.FileName);
                    FileUploadExcel.SaveAs(filePath);

                    DataTable excelData = ReadExcelFile(filePath);

                    Session["ExcelData"] = excelData;
                    Session["SelectedColumns"] = selectedColumns;

                    BindMappingGrid(selectedColumns, excelData.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList());

                    lbl_msg.Text = "File uploaded and columns mapped successfully!";
                    lbl_msg.CssClass = "text-success";
                }
                else
                {
                    lbl_msg.Text = "Please upload an Excel file.";
                    lbl_msg.CssClass = "text-danger";
                }
            }
            else
            {
                lbl_msg.Text = "Please select at least one column to proceed.";
                lbl_msg.CssClass = "text-danger";
            }
        }

        // --- PREVIEW LOGIC ---
        protected void btnPreviewChanges_Click(object sender, EventArgs e)
        {
            DataTable excelData = Session["ExcelData"] as DataTable;
            Dictionary<string, string> columnMapping = Session["ColumnMapping"] as Dictionary<string, string>;

            if (excelData == null || columnMapping == null || columnMapping.Count == 0)
            {
                lbl_msg2.Text = "Mapping or Excel data is missing. Please Check Mapping first.";
                lbl_msg2.CssClass = "text-danger";
                return;
            }

            if (!excelData.Columns.Contains("WorkmanSL"))
            {
                lbl_msg2.Text = "WorkmanSL column not found in the uploaded Excel file. It is required for updates.";
                lbl_msg2.CssClass = "text-danger";
                return;
            }

            // Create a preview datatable
            DataTable dtPreview = new DataTable();
            dtPreview.Columns.Add("WorkmanSL");
            dtPreview.Columns.Add("Column_Changed");
            dtPreview.Columns.Add("Current_DB_Value");
            dtPreview.Columns.Add("New_Excel_Value");
            dtPreview.Columns.Add("Preview_Status");

            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            foreach (DataRow row in excelData.Rows)
            {
                string workmanSL = row["WorkmanSL"]?.ToString();
                if (string.IsNullOrEmpty(workmanSL)) continue;

                string selectCols = string.Join(", ", columnMapping.Keys);
                string selectQuery = $"SELECT {selectCols} FROM tbl_Employee_Mustertable WHERE WorkmanSL = @WorkmanSL";

                using (SqlCommand cmd = new SqlCommand(selectQuery, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@WorkmanSL", workmanSL);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            foreach (var map in columnMapping)
                            {
                                string dbColName = map.Key;
                                string excelColName = map.Value;

                                // Ignore the Key Column in comparisons to avoid cluttering preview
                                if (dbColName.Equals("WorkmanSL", StringComparison.OrdinalIgnoreCase))
                                {
                                    continue;
                                }

                                string currentDbVal = reader[dbColName]?.ToString();
                                string newExcelVal = row[excelColName]?.ToString();

                                DataRow previewRow = dtPreview.NewRow();
                                previewRow["WorkmanSL"] = workmanSL;
                                previewRow["Column_Changed"] = dbColName;
                                previewRow["Current_DB_Value"] = currentDbVal;
                                previewRow["New_Excel_Value"] = newExcelVal;

                                if (currentDbVal != newExcelVal)
                                {
                                    previewRow["Preview_Status"] = "Will Update";
                                }
                                else
                                {
                                    previewRow["Preview_Status"] = "No Change";
                                }
                                dtPreview.Rows.Add(previewRow);
                            }
                        }
                        else
                        {
                            DataRow previewRow = dtPreview.NewRow();
                            previewRow["WorkmanSL"] = workmanSL;
                            previewRow["Column_Changed"] = "ALL";
                            previewRow["Preview_Status"] = "Error: WorkmanSL Not Found in DB";
                            dtPreview.Rows.Add(previewRow);
                        }
                    }
                }
            }

            dbcl.DisconnectDb();

            GridViewPreview.DataSource = dtPreview;
            GridViewPreview.DataBind();
            Session["PreviewData"] = dtPreview;

            divSetup.Visible = false;
            divPreview.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            divSetup.Visible = true;
            divPreview.Visible = false;
            lbl_msg3.Text = "";
        }

        // --- EXECUTING THE ACTUAL UPDATE AFTER PREVIEW ---
        protected void btnConfirmUpdate_Click(object sender, EventArgs e)
        {
            DataTable excelData = Session["ExcelData"] as DataTable;
            Dictionary<string, string> columnMapping = Session["ColumnMapping"] as Dictionary<string, string>;

            if (excelData != null && columnMapping != null)
            {
                if (!excelData.Columns.Contains("Status"))
                    excelData.Columns.Add("Status", typeof(string));

                // Filter out WorkmanSL from the mapping so we don't try to update the primary key
                var updateMapping = columnMapping
                    .Where(m => !m.Key.Equals("WorkmanSL", StringComparison.OrdinalIgnoreCase))
                    .ToDictionary(m => m.Key, m => m.Value);

                if (updateMapping.Count == 0)
                {
                    lbl_msg3.Text = "No valid columns mapped for update (WorkmanSL cannot be updated).";
                    lbl_msg3.CssClass = "text-danger";
                    return;
                }

                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                foreach (DataRow row in excelData.Rows)
                {
                    try
                    {
                        string setClause = string.Join(", ", updateMapping.Select(map => $"{map.Key} = @{map.Key}"));
                        string condition = "WorkmanSL = @WorkmanSL";

                        string query = $"UPDATE tbl_Employee_Mustertable SET {setClause} WHERE {condition}";
                        List<string> logDetails = new List<string> { $"SQL Query: {query}" };
                        List<string> empUpdateDetails = new List<string>();

                        using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
                        {
                            // Add parameters only for the filtered columns
                            foreach (var map in updateMapping)
                            {
                                string databaseColumn = map.Key;
                                string excelColumn = map.Value;

                                if (excelData.Columns.Contains(excelColumn))
                                {
                                    object value = row[excelColumn];
                                    cmd.Parameters.AddWithValue($"@{databaseColumn}", value);
                                    logDetails.Add($"{databaseColumn} = {value}");
                                    empUpdateDetails.Add($"[{databaseColumn}]='{value}'");
                                }
                                else
                                {
                                    LogError($"Excel column '{excelColumn}' not found.");
                                    return;
                                }
                            }

                            object workmanSL = null;
                            if (excelData.Columns.Contains("WorkmanSL"))
                            {
                                workmanSL = row["WorkmanSL"];
                                cmd.Parameters.AddWithValue("@WorkmanSL", workmanSL);
                                logDetails.Add($"WorkmanSL = {workmanSL}");
                            }
                            else
                            {
                                LogError("WorkmanSL column not found in the uploaded Excel file.");
                                row["Status"] = "Error: WorkmanSL column not found";
                                continue;
                            }

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                row["Status"] = "Success: Row updated successfully";
                                string updateString = string.Join(", ", empUpdateDetails);
                                LogAudit(workmanSL.ToString(), "BULK_UPDATE", $"Updated Values: {updateString}");
                            }
                            else
                            {
                                row["Status"] = "Error: No rows updated, WorkmanSL not found.";
                            }

                            LogQuery(string.Join(Environment.NewLine, logDetails));
                        }
                    }
                    catch (Exception ex)
                    {
                        row["Status"] = $"Error: {ex.Message}";
                        LogError($"Error executing query: {ex.Message}");
                    }
                }

                dbcl.DisconnectDb();
                ExportToExcel(excelData);

                lbl_msg3.Text = "Database updated successfully! Results downloaded.";
                lbl_msg3.CssClass = "text-success font-weight-bold";
                btnConfirmUpdate.Enabled = false;
            }
            else
            {
                lbl_msg3.Text = "Session expired or data missing. Please start over.";
                lbl_msg3.CssClass = "text-danger";
            }
        }

        private void ExportToExcel(DataTable dataTable)
        {
            string attachment = "attachment; filename=UpdateResults.xlsx";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                using (OfficeOpenXml.ExcelPackage package = new OfficeOpenXml.ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Results");
                    worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                    package.SaveAs(memoryStream);
                }

                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }

        private void LogAudit(string emp, string type, string details)
        {
            try
            {
                string logPath = Server.MapPath("~/bussiness/production/Logs/EmployeeEdits/");
                if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);

                string userName = Session["USERNAME"] != null ? Session["USERNAME"].ToString() : "System";

                string line = string.Format("{0:yyyy-MM-dd HH:mm:ss} | {1} | {2} | By: {3}{4}", DateTime.Now, type, details, userName, Environment.NewLine);
                File.AppendAllText(Path.Combine(logPath, "EmpLog_" + emp + ".txt"), line);
            }
            catch { }
        }

        private void LogQuery(string message)
        {
            try
            {
                string logFilePath = Server.MapPath("~/bussiness/production/Logs/QueryLog.txt");
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
            }
            catch { }
        }

        private void LogError(string message)
        {
            try
            {
                string logFilePath = Server.MapPath("~/bussiness/production/Logs/ErrorLog.txt");
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
            }
            catch { }
        }
    }
}