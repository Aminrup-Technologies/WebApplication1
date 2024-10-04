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
using System.Configuration;
using DocumentFormat.OpenXml.Office.Word;

namespace WebApplication1.bussiness.production
{
    public partial class pyrl_upldadvc : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        public static string state = string.Empty;
        public static string region = string.Empty;
        public static string comp = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    if (Session["Changer"] != null)
                    {
                        string[] retrievedArray = (string[])Session["Changer"];
                        region = retrievedArray[1].ToString();
                        comp = retrievedArray[2].ToString();
                        state = retrievedArray[0].ToString();

                        Session["Changer"] = null;
                    }
                    else
                    {
                        region = Session["REGION"].ToString();
                        comp = Session["COMPANY_CODE"].ToString();
                        state = Session["STATE"].ToString();
                    }
                }
            }
        }

        protected void ImportExcel(object sender, EventArgs e)
        {
            string filePath1 = FileUpload1.PostedFile.FileName;
            string filename1 = Path.GetFileName(filePath1);
            string ext = Path.GetExtension(filename1);
            string type = String.Empty;

            if (FileUpload1.HasFile)
            {
                try
                {
                    switch (ext)
                    {
                        case ".xls":

                            type = "application/vnd.ms-excel";

                            break;

                        case ".xlsx":
                            type = "application/vnd.ms-excel";

                            break;

                    }


                    if (type != String.Empty)
                    {
                        // Get the current date
                        DateTime currentDate = DateTime.Now;

                        // Convert the date to the desired format
                        string formattedDate = currentDate.ToString("yyyyMMdd");
                        //Save the uploaded Excel file.
                        string filePath = Server.MapPath("~/erp_images/ManualDedUpld/") + formattedDate + "_" + region+ "_" + state+ "_" + comp + "_" + Path.GetFileName(FileUpload1.PostedFile.FileName);
                        FileUpload1.SaveAs(filePath);

                        //Open the Excel file in Read Mode using OpenXml.
                        using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filePath, false))
                        {
                            //Read the first Sheets from Excel file.
                            Sheet sheet = doc.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();

                            //Get the Worksheet instance.
                            Worksheet worksheet = (doc.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;

                            //Fetch all the rows present in the Worksheet.
                            IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                            //Create a new DataTable.
                            DataTable dt = new DataTable();

                            //Loop through the Worksheet rows.
                            foreach (Row row in rows)
                            {
                                //Use the first row to add columns to DataTable
                                if (row.RowIndex.Value == 1)
                                {
                                    foreach (Cell cell in row.Descendants<Cell>())
                                    {
                                        dt.Columns.Add(GetValue(doc, cell));
                                    }
                                }
                                else
                                {
                                    //Add rows to DataTable.
                                    dt.Rows.Add();
                                    int i = 0;
                                    foreach (Cell cell in row.Descendants<Cell>())
                                    {
                                        string var = GetValue(doc, cell);
                                        if (var == null || var == "")
                                        {
                                            dt.Rows[dt.Rows.Count - 1][i] = "";
                                            i++;
                                        }
                                        else
                                        {
                                            dt.Rows[dt.Rows.Count - 1][i] = var;
                                            i++;
                                        }
                                    }
                                }
                            }
                            ViewState["DeductionDetails"] = dt;
                            GridView1.DataSource = dt;
                            GridView1.DataBind();

                            lbl_msg.ForeColor = System.Drawing.Color.Green;
                            lbl_msg.Text = "Excel Sheet Data uploaded successfully, Click SUBMIT to Save";
                        }
                    }
                    else
                    {
                        string title = "Notifications :";
                        string body = "Select Only Excel File having extension .xlsx or .xls ";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "Select Only Excel File having extension .xlsx or .xls ";
                    }
                }
                catch (Exception ex)
                {
                    string title = "Notifications :";
                    string body = "Error : " + ex.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Error: " + ex.Message.ToString();
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "No file Selected...!! ";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowUploaderModal();", true);
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "No file Selected...!! ";
            }
        }

        private string GetValue(SpreadsheetDocument doc, Cell cell)
        {
            string value = cell.CellValue.InnerText;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            DataTable dt1;
            dt1 = (DataTable)ViewState["DeductionDetails"];
            if (dt1 != null)
            {
                foreach (GridViewRow gvadd in GridView1.Rows)
                {
                    string workmanSL = gvadd.Cells[0].Text.ToString();
                    int curr_advc = Convert.ToInt32(gvadd.Cells[1].Text.ToString());
                    int curr_fines = Convert.ToInt32(gvadd.Cells[2].Text.ToString());
                    int curr_others = Convert.ToInt32(gvadd.Cells[3].Text.ToString());

                    if (UpdateEmployeeRecord(workmanSL, state, region, comp, curr_advc, curr_fines, curr_others, curr_advc, curr_advc, curr_fines, curr_fines, curr_others, curr_others) != true)
                    {
                        break;
                    }
                }

            }
            else
            {
                string title = "Notifications :";
                string body = "No data in Memory...!! ";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        public bool UpdateEmployeeRecord(string workmanSL, string WorkState, string WorkRegion, string WorkCompany, int advance, int fines, int others, int remAdvance, int curAdvance, int remFines, int curFines, int remOthers, int curOthers)
        {
            //Code to Insert values into the DB goes here
            int flag = 0;
            bool flag2 = false;

            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UpdateEmployeeDeductions", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@WorkmanSL", workmanSL);
                    command.Parameters.AddWithValue("@WorkState", WorkState);
                    command.Parameters.AddWithValue("@WorkRegion", WorkRegion);
                    command.Parameters.AddWithValue("@WorkCompany", WorkCompany);
                    command.Parameters.AddWithValue("@Advance", advance);
                    command.Parameters.AddWithValue("@Fines", fines);
                    command.Parameters.AddWithValue("@Others", others);
                    command.Parameters.AddWithValue("@Rem_Advance", remAdvance);
                    command.Parameters.AddWithValue("@Cur_Advance", curAdvance);
                    command.Parameters.AddWithValue("@Rem_Fines", remFines);
                    command.Parameters.AddWithValue("@Cur_Fines", curFines);
                    command.Parameters.AddWithValue("@Rem_Others", remOthers);
                    command.Parameters.AddWithValue("@Cur_Others", curOthers);

                    connection.Open();
                    flag = command.ExecuteNonQuery();
                    if (flag != 0)
                    {
                        flag2 = true;
                        lbl_msg.Visible = true;
                        lbl_msg.Text = "Record Inserted Succesfully..!";
                        lbl_msg.ForeColor = System.Drawing.Color.DarkGreen;
                    }
                    else
                    {
                        flag2 = false;
                        string title = "Notifications :";
                        string body = "Records Connot be Inserted into the Database...!! ";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                    connection.Close();
                }
            }
            return flag2;
        }
    }
}