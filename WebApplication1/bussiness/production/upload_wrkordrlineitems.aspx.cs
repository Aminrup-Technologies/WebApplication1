using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.Configuration;

namespace WebApplication1.bussiness.production
{
    public partial class upload_wrkordrlineitems : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region order by Id";
                BindWorkRegion(CmdString1);

                //string CmdString2 = "select * from tlb_WO_LineItems_Data order by Id";
                //BindGrid(CmdString2);
            }
        }
        private void BindWorkRegion(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_WorkRegion.DataSource = Cmd.ExecuteReader();
            DDL_WorkRegion.DataTextField = "Work_Region_Name";
            DDL_WorkRegion.DataValueField = "Work_Region_Code";
            DDL_WorkRegion.DataBind();
            DDL_WorkRegion.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void ExportExcel(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT top(1) ItemNO,LineNumber,ServiceNumber,Service_Description,Order_Quantity,Rate,PerUnit_Value FROM tlb_WO_LineItems_Data"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dt, "WO_LineItems");

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=LineItemUpload.xlsx");
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void BindGrid(string cmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            dbcl.Conn.Close();
        }

        protected void DDL_WorkRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL_Value = DDL_WorkRegion.SelectedValue.ToString();

            string CmdString2 = "select Company_Name, Company_Code from tlb_workregion_company where Work_Region_Code='" + DDL_Value + "' order by Id";
            BindCompany(CmdString2);
        }

        private void BindCompany(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Company.DataSource = Cmd.ExecuteReader();
            DDL_Company.DataTextField = "Company_Name";
            DDL_Company.DataValueField = "Company_Code";
            DDL_Company.DataBind();
            DDL_Company.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_Company_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL_Value = DDL_WorkRegion.SelectedValue.ToString();

            string DDL_CompValue = DDL_Company.SelectedValue.ToString();

            string CmdString2 = "select Company_Department, DB_Code from tlb_workregion_compdept where Work_Region_Code='" + DDL_Value + "' and Company_Code = '" + DDL_CompValue + "' order by Id";
            BindDepartment(CmdString2);
        }

        private void BindDepartment(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Departments.DataSource = Cmd.ExecuteReader();
            DDL_Departments.DataTextField = "Company_Department";
            DDL_Departments.DataValueField = "DB_Code";
            DDL_Departments.DataBind();
            DDL_Departments.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_Departments_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL_Value = DDL_WorkRegion.SelectedValue.ToString();

            string DDL_CompValue = DDL_Company.SelectedValue.ToString();

            string DDL_DeptValue = DDL_Departments.SelectedValue.ToString();

            string CmdString2 = "select WO_Number, DB_Code from tlb_WO_Data where Work_Region_Code='" + DDL_Value + "' and Company_Code = '" + DDL_CompValue + "' and Dept_DBCode = '" + DDL_DeptValue + "' and WO_Status='Active' order by Id";
            BindWorkorder(CmdString2);
        }

        private void BindWorkorder(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Workorder.DataSource = Cmd.ExecuteReader();
            DDL_Workorder.DataTextField = "WO_Number";
            DDL_Workorder.DataValueField = "DB_Code";
            DDL_Workorder.DataBind();
            DDL_Workorder.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
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
                        //Save the uploaded Excel file.
                        string filePath = Server.MapPath("~/erp_images/WO_Files/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
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
                                        dt.Rows[dt.Rows.Count - 1][i] = GetValue(doc, cell);
                                        i++;
                                    }
                                }
                            }
                            ViewState["AgendaDetails"] = dt;
                            GridView1.DataSource = dt;
                            GridView1.DataBind();

                            lbl_msg.ForeColor = System.Drawing.Color.Green;
                            lbl_msg.Text = "Excel Sheet Data uploaded successfully, Click SUBMIT to Save";
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "Select Only Excel File having extension .xlsx or .xls ";
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Error: " + ex.Message.ToString();
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
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

        private string Find_DBCode()
        {
            string aa = null;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string kk = null;
            string cmdString1 = "select Id,WOI_DBCode from tlb_WO_LineItems_Data where Id=(select max(Id)from tlb_WO_LineItems_Data)";
            SqlCommand com1 = new SqlCommand(cmdString1, dbcl.Conn);
            SqlDataReader DR1 = com1.ExecuteReader();
            if (DR1.Read())
            {
                aa = DR1.GetValue(1).ToString();
                string bb = aa.Substring(5);
                int k = Convert.ToInt32(bb);
                k = k + 1;
                string q = Convert.ToString(k);
                kk = "WOI00" + q;
            }
            else
            {
                kk = "WOI001";
            }
            dbcl.DisconnectDb();
            return kk;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            DataTable dt1;
            dt1 = (DataTable)ViewState["AgendaDetails"];
            if (dt1 != null)
            {
                foreach (GridViewRow gvadd in GridView1.Rows)
                {
                    string DBCode = Find_DBCode();
                    string lineno = Server.HtmlDecode(gvadd.Cells[0].Text.ToString());
                    string serialno = Server.HtmlDecode(gvadd.Cells[1].Text.ToString());
                    string serviceno = Server.HtmlDecode(gvadd.Cells[2].Text.ToString());
                    string descp = Server.HtmlDecode(gvadd.Cells[3].Text.ToString());
                    string ordrqnty = Server.HtmlDecode(gvadd.Cells[4].Text.ToString());
                    string rate = Server.HtmlDecode(gvadd.Cells[5].Text.ToString());
                    string unit = Server.HtmlDecode(gvadd.Cells[6].Text.ToString());
                    Inser_WorkorderLineItems(DBCode, lineno, serialno, serviceno, descp, ordrqnty, rate, unit);
                }
            }
        }
        protected Int32 Inser_WorkorderLineItems(string DBCode, string lineno, string serialno, string serviceno, string descp, string ordrqnty, string rate, string unit)
        {
            int flag = 0;
            try
            {
                dbcl.Sqlconnection();
                SqlCommand cmd = new SqlCommand("SP_InsertInto_WOLineItemTable", dbcl.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Work_Region_Name", DDL_WorkRegion.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Work_Region_Code", DDL_WorkRegion.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Company_Name", DDL_Company.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Company_Code", DDL_Company.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Department_Name", DDL_Departments.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Department_Code", DDL_Departments.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WODB_Code", DDL_Workorder.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WO_Number", DDL_Workorder.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@WOI_DBCode", DBCode);
                cmd.Parameters.AddWithValue("@ItemNO", lineno);
                cmd.Parameters.AddWithValue("@LineNumber", serialno);
                cmd.Parameters.AddWithValue("@ServiceNumber", serviceno);
                cmd.Parameters.AddWithValue("@Service_Description", descp);
                cmd.Parameters.AddWithValue("@Order_Quantity", ordrqnty);
                cmd.Parameters.AddWithValue("@Rate", Convert.ToDecimal(rate));
                cmd.Parameters.AddWithValue("@PerUnit_Value", unit);
                dbcl.ConnectDb();
                flag = cmd.ExecuteNonQuery();

                if (flag != 0)
                {
                    lbl_msg.Visible = true;
                    lbl_msg.Text = "Record Inserted Succesfully..!";
                    lbl_msg.ForeColor = System.Drawing.Color.DarkGreen;
                    dbcl.DisconnectDb();
                }
                else
                {
                    //lbl_msg.Visible = true;
                    //lbl_msg.Text = "Records Connot be Inserted into the Database";
                    //lbl_msg.ForeColor = System.Drawing.Color.IndianRed;
                    //dbcl.DisconnectDb();
                }
            }
            catch (Exception ex)
            {
                lbl_msg.Visible = true;
                lbl_msg.Text = ex.Message;
                lbl_msg.ForeColor = System.Drawing.Color.IndianRed;
                dbcl.DisconnectDb();
            }
            return flag;
        }
    }
}