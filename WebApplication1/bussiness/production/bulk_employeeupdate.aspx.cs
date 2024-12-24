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

        //protected void btnMapColumns_Click(object sender, EventArgs e)
        //{
        //    List<string> selectedColumns = new List<string>();

        //    // Iterate through GridView rows
        //    foreach (GridViewRow row in GridViewColumns.Rows)
        //    {
        //        CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
        //        if (chkSelect != null && chkSelect.Checked)
        //        {
        //            // Get the column name from the second column in the GridView
        //            string columnName = row.Cells[1].Text; // Assuming the column name is in the second cell
        //            selectedColumns.Add(columnName);
        //        }
        //    }

        //    if (selectedColumns.Count > 0)
        //    {
        //        // Store the selected columns in a session or pass them to the next step
        //        Session["SelectedColumns"] = selectedColumns;

        //        lbl_msg.Text = "Columns selected: " + string.Join(", ", selectedColumns);
        //        lbl_msg.CssClass = "text-success";
        //    }
        //    else
        //    {
        //        lbl_msg.Text = "Please select at least one column to proceed.";
        //        lbl_msg.CssClass = "text-danger";
        //    }
        //}

        protected void btnMapColumns_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> columnMapping = new Dictionary<string, string>(); // Key: DB Column, Value: Excel Column

            foreach (GridViewRow row in GridViewColumnMapping.Rows)
            {
                string dbColumn = row.Cells[0].Text; // Database column name
                DropDownList ddlExcelColumns = (DropDownList)row.FindControl("ddlExcelColumns");

                if (ddlExcelColumns != null && !string.IsNullOrEmpty(ddlExcelColumns.SelectedValue))
                {
                    columnMapping.Add(dbColumn, ddlExcelColumns.SelectedValue); // Map DB column to Excel column
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


        protected void ReadExcel()
        {
            if (FileUploadExcel.HasFile)
            {
                string filePath = Server.MapPath("~/Uploads/" + FileUploadExcel.FileName);
                FileUploadExcel.SaveAs(filePath);

                // Read the Excel file into a DataTable
                DataTable excelData = ReadExcelFile(filePath);

                // Store the data for mapping
                Session["ExcelData"] = excelData;
                BindExcelColumnsToDropdown(excelData);

                lblUploadMessage.Text = "File uploaded and data loaded successfully!";
                lblUploadMessage.CssClass = "text-success";
            }
            else
            {
                lblUploadMessage.Text = "Please upload an Excel file.";
                lblUploadMessage.CssClass = "text-danger";
            }
        }

        private DataTable ReadExcelFile(string filePath)
        {
            DataTable dt = new DataTable();

            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                // Check if the workbook contains worksheets
                if (package.Workbook.Worksheets.Count == 0)
                {
                    throw new Exception("The uploaded Excel file contains no worksheets.");
                }

                // Access the first worksheet
                //ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];

                // Check if the worksheet has valid data
                if (worksheet.Dimension == null)
                {
                    throw new Exception("The worksheet is empty.");
                }

                int colCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;

                // Add columns to DataTable
                for (int col = 1; col <= colCount; col++)
                {
                    dt.Columns.Add(worksheet.Cells[1, col].Text); // Assuming the first row contains column names
                }

                // Add rows to DataTable
                for (int row = 2; row <= rowCount; row++) // Start from the second row
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

            // Bind to dropdown in the mapping GridView
            foreach (GridViewRow row in GridViewColumnMapping.Rows)
            {
                DropDownList ddlExcelColumns = (DropDownList)row.FindControl("ddlExcelColumns");
                if (ddlExcelColumns != null)
                {
                    ddlExcelColumns.DataSource = excelColumns;
                    ddlExcelColumns.DataBind();
                    ddlExcelColumns.Items.Insert(0, new ListItem("Select", "")); // Add default option
                }
            }
        }

        protected void BindMappingGrid(List<string> selectedColumns, List<string> excelColumns)
        {
            // Create a DataTable for mapping
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

            // Populate the dropdown for Excel columns
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
                // Bind the data to the GridView
                GridViewColumnMapping.DataSource = dt;
                GridViewColumnMapping.DataBind();
            }
            else
            {
                lbl_msg.Text = "No data found in the uploaded file.";
                lbl_msg.CssClass = "text-danger";
            }
        }




        protected void GridViewColumnMapping_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Ensure you're only processing data rows
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Find the DropDownList in the current row
                DropDownList ddlExcelColumn = (DropDownList)e.Row.FindControl("DropDownListExcelColumn");

                if (ddlExcelColumn != null)
                {
                    // Assuming 'filePath' is the path to the Excel file
                    string filePath = "~\\erp_images\\EmpUpdate\\Uan_Esic.xlsx";

                    using (var package = new ExcelPackage(new FileInfo(filePath)))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int colCount = worksheet.Dimension.End.Column; // Total number of columns
                        ddlExcelColumn.Items.Clear(); // Clear previous items

                        // Add column headers to the DropDownList
                        for (int col = 1; col <= colCount; col++)
                        {
                            string header = worksheet.Cells[1, col].Text;
                            ddlExcelColumn.Items.Add(new ListItem(header, header));
                        }
                    }
                }
            }
        }


        protected void PerformBulkUpdate(Dictionary<string, string> columnMappings, DataTable excelData)
        {
            string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                foreach (DataRow row in excelData.Rows)
                {
                    string updateQuery = "UPDATE tbl_Employee_Mustertable SET ";

                    // Dynamically build SET clause
                    foreach (var mapping in columnMappings)
                    {
                        string dbColumn = mapping.Key;
                        string excelColumn = mapping.Value;
                        updateQuery += $"{dbColumn} = @{dbColumn}, ";
                    }

                    // Remove trailing comma and add WHERE clause
                    updateQuery = updateQuery.TrimEnd(',', ' ') + " WHERE Workman = @Workman";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                    {
                        // Add parameters dynamically
                        foreach (var mapping in columnMappings)
                        {
                            cmd.Parameters.AddWithValue($"@{mapping.Key}", row[mapping.Value]);
                        }
                        cmd.Parameters.AddWithValue("@Workman", row["Workman"]);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }


        protected void btnUploadExcel_Click(object sender, EventArgs e)
        {
            if (FileUploadExcel.HasFile)
            {
                string filePath = Server.MapPath("~/erp_images/EmpUpdate/" + FileUploadExcel.FileName);
                FileUploadExcel.SaveAs(filePath);

                // Read the Excel file into a DataTable
                DataTable excelData = ReadExcelFile(filePath);

                // Store the data for mapping
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

            // Iterate through GridView rows to get selected columns
            foreach (GridViewRow row in GridViewColumns.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect != null && chkSelect.Checked)
                {
                    // Get the column name from the second column in the GridView
                    string columnName = row.Cells[1].Text; // Assuming the column name is in the second cell
                    selectedColumns.Add(columnName);
                }
            }

            if (selectedColumns.Count > 0)
            {
                // Check if the Excel file is uploaded
                if (FileUploadExcel.HasFile)
                {
                    string filePath = Server.MapPath("~/erp_images/EmpUpdate/" + FileUploadExcel.FileName);
                    FileUploadExcel.SaveAs(filePath);

                    // Read the Excel file into a DataTable
                    DataTable excelData = ReadExcelFile(filePath);

                    // Store the data and selected columns in session
                    Session["ExcelData"] = excelData;
                    Session["SelectedColumns"] = selectedColumns;

                    // Bind the GridView and Dropdowns for mapping
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

        //protected void btnUpdateDatabase_Click(object sender, EventArgs e)
        //{
        //    DataTable excelData = Session["ExcelData"] as DataTable;
        //    Dictionary<string, string> columnMapping = Session["ColumnMapping"] as Dictionary<string, string>;

        //    if (excelData != null && columnMapping != null)
        //    {
        //        // Check if WorkmanSL exists in the DataTable
        //        if (!excelData.Columns.Contains("WorkmanSL"))
        //        {
        //            lbl_msg2.Text = "WorkmanSL column not found in the uploaded Excel file.";
        //            lbl_msg2.CssClass = "text-danger";
        //            return;
        //        }

        //        // Open database connection once before starting the loop
        //        dbcl.Sqlconnection();
        //        dbcl.ConnectDb();

        //        foreach (DataRow row in excelData.Rows)
        //        {
        //            // Build dynamic SQL query
        //            string setClause = string.Join(", ", columnMapping.Select(map => $"{map.Key} = @{map.Key}"));
        //            string condition = "WorkmanSL = @WorkmanSL"; // Adjust condition based on your table

        //            string query = $"UPDATE tbl_Employee_Mustertable SET {setClause} WHERE {condition}";

        //            using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
        //            {
        //                // Add parameters for each column mapping
        //                foreach (var map in columnMapping)
        //                {
        //                    cmd.Parameters.AddWithValue($"@{map.Key}", row[map.Value]);
        //                }

        //                // Assuming WorkmanSL is unique for each row
        //                cmd.Parameters.AddWithValue("@WorkmanSL", row["WorkmanSL"]);

        //                try
        //                {
        //                    cmd.ExecuteNonQuery();
        //                }
        //                catch (Exception ex)
        //                {
        //                    lbl_msg2.Text = $"Error updating database: {ex.Message}";
        //                    lbl_msg2.CssClass = "text-danger";
        //                    return; // Stop further execution if there's an error
        //                }
        //            }
        //        }

        //        // Disconnect from the database
        //        dbcl.DisconnectDb();

        //        lbl_msg2.Text = "Database updated successfully!";
        //        lbl_msg2.CssClass = "text-success";
        //    }
        //    else
        //    {
        //        lbl_msg2.Text = "Mapping or Excel data is missing.";
        //        lbl_msg2.CssClass = "text-danger";
        //    }
        //}


        //protected void btnUpdateDatabase_Click(object sender, EventArgs e)
        //{
        //    DataTable excelData = Session["ExcelData"] as DataTable;
        //    Dictionary<string, string> columnMapping = Session["ColumnMapping"] as Dictionary<string, string>;

        //    if (excelData != null && columnMapping != null)
        //    {
        //        dbcl.Sqlconnection();
        //        dbcl.ConnectDb();

        //        foreach (DataRow row in excelData.Rows)
        //        {
        //            // Build dynamic SQL query
        //            string setClause = string.Join(", ", columnMapping.Select(map => $"{map.Key} = @{map.Key}"));
        //            string condition = "WorkmanSL = @WorkmanSL";

        //            string query = $"UPDATE tbl_Employee_Mustertable SET {setClause} WHERE {condition}";

        //            using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
        //            {
        //                // Add parameters for each column mapping
        //                foreach (var map in columnMapping)
        //                {
        //                    string databaseColumn = map.Key; // Database column
        //                    string excelColumn = map.Value; // Corresponding Excel column

        //                    // Assign the correct value from the Excel data
        //                    if (excelData.Columns.Contains(excelColumn))
        //                    {
        //                        cmd.Parameters.AddWithValue($"@{databaseColumn}", row[excelColumn]);
        //                    }
        //                    else
        //                    {
        //                        lbl_msg2.Text = $"Excel column '{excelColumn}' not found.";
        //                        lbl_msg2.CssClass = "text-danger";
        //                        return;
        //                    }
        //                }

        //                // Add the condition parameter (e.g., WorkmanSL)
        //                if (excelData.Columns.Contains("WorkmanSL"))
        //                {
        //                    cmd.Parameters.AddWithValue("@WorkmanSL", row["WorkmanSL"]);
        //                }
        //                else
        //                {
        //                    lbl_msg2.Text = "WorkmanSL column not found in the uploaded Excel file.";
        //                    lbl_msg2.CssClass = "text-danger";
        //                    return;
        //                }

        //                try
        //                {
        //                    cmd.ExecuteNonQuery();
        //                }
        //                catch (Exception ex)
        //                {
        //                    lbl_msg2.Text = $"Error updating database: {ex.Message}";
        //                    lbl_msg2.CssClass = "text-danger";
        //                    return;
        //                }
        //            }
        //        }

        //        dbcl.DisconnectDb();

        //        lbl_msg2.Text = "Database updated successfully!";
        //        lbl_msg2.CssClass = "text-success";
        //    }
        //    else
        //    {
        //        lbl_msg2.Text = "Mapping or Excel data is missing.";
        //        lbl_msg2.CssClass = "text-danger";
        //    }
        //}


        protected void btnUpdateDatabase_Click(object sender, EventArgs e)
        {
            DataTable excelData = Session["ExcelData"] as DataTable;
            Dictionary<string, string> columnMapping = Session["ColumnMapping"] as Dictionary<string, string>;

            if (excelData != null && columnMapping != null)
            {
                // Add a Status column to track the update status
                if (!excelData.Columns.Contains("Status"))
                    excelData.Columns.Add("Status", typeof(string));

                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                foreach (DataRow row in excelData.Rows)
                {
                    // Build dynamic SQL query
                    string setClause = string.Join(", ", columnMapping.Select(map => $"{map.Key} = @{map.Key}"));
                    string condition = "WorkmanSL = @WorkmanSL";

                    string query = $"UPDATE tbl_Employee_Mustertable SET {setClause} WHERE {condition}";
                    List<string> logDetails = new List<string> { $"SQL Query: {query}" };

                    using (SqlCommand cmd = new SqlCommand(query, dbcl.Conn))
                    {
                        // Add parameters for each column mapping
                        foreach (var map in columnMapping)
                        {
                            string databaseColumn = map.Key; // Database column
                            string excelColumn = map.Value; // Corresponding Excel column

                            if (excelData.Columns.Contains(excelColumn))
                            {
                                object value = row[excelColumn];
                                cmd.Parameters.AddWithValue($"@{databaseColumn}", value);
                                logDetails.Add($"{databaseColumn} = {value}");
                            }
                            else
                            {
                                LogError($"Excel column '{excelColumn}' not found."); // Log error if column missing
                                return;
                            }
                        }

                        // Add the condition parameter (e.g., WorkmanSL)
                        if (excelData.Columns.Contains("WorkmanSL"))
                        {
                            object workmanSL = row["WorkmanSL"];
                            cmd.Parameters.AddWithValue("@WorkmanSL", workmanSL);
                            logDetails.Add($"WorkmanSL = {workmanSL}");
                        }
                        else
                        {
                            
                            LogError("WorkmanSL column not found in the uploaded Excel file.");
                            row["Status"] = "Error: WorkmanSL column not found";
                            continue;
                            //return;
                        }

                        // Log the query and parameters
                        LogQuery(string.Join(Environment.NewLine, logDetails));

                        try
                        {
                            cmd.ExecuteNonQuery();
                            row["Status"] = "Success: Row updated successfully";
                        }
                        catch (Exception ex)
                        {
                            row["Status"] = $"Error: {ex.Message}";
                            LogError($"Error executing query: {ex.Message}");
                            return;
                        }
                    }
                }

                dbcl.DisconnectDb();

                // Export the updated DataTable to Excel
                ExportToExcel(excelData);

                lbl_msg2.Text = "Database updated successfully!";
                lbl_msg2.CssClass = "text-success";
            }
            else
            {
                lbl_msg2.Text = "Mapping or Excel data is missing.";
                lbl_msg2.CssClass = "text-danger";
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


        // Logging methods
        private void LogQuery(string message)
        {
            string logFilePath = Server.MapPath("~/bussiness/production/Logs/QueryLog.txt");
            File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
        }

        private void LogError(string message)
        {
            string logFilePath = Server.MapPath("~/bussiness/production/Logs/ErrorLog.txt");
            File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
        }

    }
}