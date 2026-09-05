using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Diagnostics;

namespace WebApplication1.bussiness.production.rpts
{
    public partial class salaryslip : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void GeneratePay()
        {
            DataSet ds = new DataSet();
            int EmployeeID = Convert.ToInt32(ddlEmployeeID.SelectedValue);
            string Month = ddlMonth.SelectedValue;
            ds = GetData(EmployeeID, Month);
            if (ds != null && ds.Tables.Count == 5 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                Document doc = new Document(PageSize.A4, 36f, 36f, 36f, 36f);//36f, 36f, 90f, 100f);
                PdfWriter.GetInstance(doc, Response.OutputStream);
                doc.Open();
                //var fontFamily = FontFactory.GetFont("TIMES ROMAN", 15, BaseColor.BLUE);
                // 1) Adding logo to right side top
                string imagePath = Server.MapPath("~\\erp_images") + "\\ats_translogo.png";
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imagePath);
                image.Alignment = Element.ALIGN_MIDDLE;
                // set width and height
                image.ScaleToFit(100f, 120f);
                doc.Add(image);
                


                Paragraph comp = new Paragraph();
                comp.Add(new Chunk("AUTOMATION & TECHNICAL SERVICES", new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK)));
                comp.Alignment = 1;
                doc.Add(comp);


                // 2) Addling blank paragraph
                doc.Add(new Paragraph("  "));


                // 3) Adding title table
                Paragraph title = new Paragraph();
                title.Add(new Chunk("Pay Slip for the month of " + ddlMonth.SelectedValue, new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK)));
                title.Alignment = 1;
                doc.Add(title);
                // 4) Addling blank paragraph
                doc.Add(new Paragraph("  "));
                // 5) Creating 1st table with 4 column
                PdfPTable table1 = new PdfPTable(4);
                int[] columnwidth = { 20, 25, 20, 25 };
                table1.SetWidths(columnwidth);
                table1.WidthPercentage = 100;
                table1.HorizontalAlignment = 0;
                // 6) Adding employee data to table1
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    string columnName = (ds.Tables[0].Columns[i].ColumnName);
                    string columnValue = (ds.Tables[0].Rows[0][i].ToString());
                    PdfPCell cellColumnName = new PdfPCell(new Phrase(columnName, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                    cellColumnName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                                            //Cellcolumnname.Border = 15;
                    table1.AddCell(cellColumnName);
                    PdfPCell cellColumnValue = new PdfPCell(new Phrase(columnValue, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                    cellColumnValue.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                                             //cellcolumnvalue.Border = 15;
                    table1.AddCell(cellColumnValue);
                }
                doc.Add(table1);
                // 6) Addling blank paragraph
                doc.Add(new Paragraph("  "));
                // 7) Creating 2nd table with 4 columns [which is main table]
                PdfPTable mainTable = new PdfPTable(4); //earnedTable1.TotalWidth = 500f;//earnedTable1.LockedWidth = true;
                int[] columnwidth1 = { 30, 20, 30, 20 }; //23, 20, 25, 32 };
                mainTable.SetWidths(columnwidth1);
                mainTable.WidthPercentage = 100;
                mainTable.HorizontalAlignment = 0;
                // a. adding 4 cells for header
                mainTable.AddCell(new PdfPCell(new Phrase("EARNINGS", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) }); //{ HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) };;
                mainTable.AddCell(new PdfPCell(new Phrase("RUPEES", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) }); //{ HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) };;
                mainTable.AddCell(new PdfPCell(new Phrase("DEDUCTIONS", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) }); //{ HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) };;
                mainTable.AddCell(new PdfPCell(new Phrase("RUPEES", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) }); //{ HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) };;
                                                                                                                                                                                                                                                        // b. creating earning table with 2 columns [left side]
                PdfPTable earning = new PdfPTable(2);
                int[] columnwidth3 = { 30, 20 };
                earning.SetWidths(columnwidth3);
                earning.WidthPercentage = 80;
                earning.HorizontalAlignment = 0;
                // c. adding earning data
                for (int i = 0; i < ds.Tables[1].Columns.Count; i++)
                {
                    string columnName = (ds.Tables[1].Columns[i].ColumnName);
                    string columnValue = (ds.Tables[1].Rows[0][i].ToString());
                    PdfPCell cellColumnName = new PdfPCell(new Phrase(columnName, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                    cellColumnName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    earning.AddCell(cellColumnName);
                    PdfPCell cellColumnValue = new PdfPCell(new Phrase(AppendComma(columnValue), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT };
                    earning.AddCell(cellColumnValue);
                }
                // d. creating deduction table with 2 columns [Right side]
                PdfPTable deduction = new PdfPTable(2);
                int[] columnwidth4 = { 30, 20 };
                deduction.SetWidths(columnwidth3);
                deduction.WidthPercentage = 80;
                deduction.HorizontalAlignment = 0;
                // e. adding deduction data
                for (int i = 0; i < ds.Tables[2].Columns.Count; i++)
                {
                    string columnName = (ds.Tables[2].Columns[i].ColumnName);
                    string columnValue = (ds.Tables[2].Rows[0][i].ToString());
                    PdfPCell cellColumnName = new PdfPCell(new Phrase(columnName, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                    cellColumnName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    deduction.AddCell(cellColumnName);
                    PdfPCell cellColumnValue = new PdfPCell(new Phrase(AppendComma(columnValue), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT };
                    deduction.AddCell(cellColumnValue);
                }
                // f. creating a new cell [cell1] with colspan=2
                //    adding earning table into cell1
                PdfPCell cell1 = new PdfPCell(earning);
                cell1.Colspan = 2;
                // adding cell1 into mainTable
                mainTable.AddCell(cell1);
                // g. creating a new cell [cell2] with colspan=2
                //    adding deduction table into cell2
                PdfPCell cell2 = new PdfPCell(deduction);
                cell2.Colspan = 2;
                // adding cell2 into mainTable
                mainTable.AddCell(cell2);
                mainTable.AddCell(new PdfPCell(new Phrase("Gross Earning", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, BackgroundColor = new BaseColor(236, 236, 236) });
                mainTable.AddCell(new PdfPCell(new Phrase(AppendComma(ds.Tables[3].Rows[0][0].ToString()), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 });
                mainTable.AddCell(new PdfPCell(new Phrase("Gross Deductions", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) });
                mainTable.AddCell(new PdfPCell(new Phrase(AppendComma(ds.Tables[4].Rows[0][0].ToString()), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 });
                PdfPCell netEarning = new PdfPCell(new Phrase("Net Salary", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, BackgroundColor = new BaseColor(236, 236, 236) };
                mainTable.AddCell(netEarning);
                string NetSalary = (Convert.ToDecimal(ds.Tables[3].Rows[0][0].ToString()) - Convert.ToDecimal(ds.Tables[4].Rows[0][0].ToString())).ToString();
                PdfPCell netSalary = new PdfPCell(new Phrase(AppendComma(NetSalary), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 };
                mainTable.AddCell(netSalary);
                PdfPCell blankCell = new PdfPCell();
                blankCell.Colspan = 2;
                mainTable.AddCell(blankCell);
                PdfPCell NetSalaryInWords = new PdfPCell(new Phrase("Net Salary In Word : " + GenerateWordsinRs(NetSalary), new Font(Font.FontFamily.COURIER, 8, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5 };
                NetSalaryInWords.Colspan = 4;
                mainTable.AddCell(NetSalaryInWords);
                // adding mainTable to document object
                doc.Add(mainTable);
                doc.Add(new Paragraph("  "));
                doc.Add(new Paragraph("  "));
                doc.Add(new Paragraph("  "));
                Paragraph Note = new Paragraph();
                Note.Add(new Chunk("This is computer generated payslip and does not require signature or company seal.", new Font(Font.FontFamily.COURIER, 9, 1, BaseColor.BLACK)));
                Note.Alignment = 1;
                doc.Add(Note);
                Paragraph address = new Paragraph();
                address.Add(new Chunk(@"(Automation & Techinal Services.)
        Address: 2nd Floor, Steet Name, Bullding Number, Jamshedpur - 831001.", new Font(Font.FontFamily.COURIER, 9, 1, BaseColor.BLACK)));
                address.Alignment = 1;
                doc.Add(address);
                doc.Close();
                string fileName = ddlEmployeeID.SelectedValue + "_" + ddlMonth.SelectedValue + ".pdf";
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;" + "filename=" + fileName);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(doc);
                Response.End();
            }
            else
            {
                lblMessage.Text = "Salary slip not generated.";
            }
        }
        private string AppendComma(string value)
        {
            return String.Format("{0:#,0.00}", Convert.ToDecimal(value));
        }
        public string GenerateWordsinRs(string inputRs)
        {
            string input = inputRs;
            string a = "";
            string b = "";
            // take decimal part of input. convert it to word. add it at the end of method.
            string decimals = "";
            if (input.Contains("."))
            {
                decimals = input.Substring(input.IndexOf(".") + 1);
                // remove decimal part from input
                input = input.Remove(input.IndexOf("."));
            }
            string strWords = NumbersToWords(Convert.ToInt32(input));
            if (!inputRs.Contains("."))
            {
                a = strWords + " Rupees Only";
            }
            else
            {
                a = strWords + " Rupees";
            }
            if (decimals.Length > 0)
            {
                // if there is any decimal part convert it to words and add it to strWords.
                string strwords2 = NumbersToWords(Convert.ToInt32(decimals));
                b = " and " + strwords2 + " Paisa Only ";
            }
            string final2 = "";
            final2 = a + b;
            return final2;
        }
        public static string NumbersToWords(int inputNumber)
        {
            int inputNo = inputNumber;
            if (inputNo == 0)
                return "Zero";
            int[] numbers = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (inputNo < 0)
            {
                sb.Append("Minus ");
                inputNo = -inputNo;
            }
            string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ","Five " ,"Six ", "Seven ", "Eight ", "Nine "};
            string[] words1 = {"Ten ", "Eleven " , "Twelve ", "Thirteen " , "Fourteen ","Fifteen ", "Sixteen " ,"Seventeen ", "Eighteen " , "Nineteen "};
            string[] words2 = {"Twenty ", "Thirty " , "Forty ", "Fifty ", "Sixty ","Seventy ", "Eighty " , "Ninety "};
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };
            numbers[0] = inputNo % 1000; // units
            numbers[1] = inputNo / 1000;
            numbers[2] = inputNo / 100000;
            numbers[1] = numbers[1] - 100 * numbers[2]; // thousands
            numbers[3] = inputNo / 10000000; // crores
            numbers[2] = numbers[2] - 100 * numbers[3]; // lakhs
            for (int i = 3; i > 0; i--)
            {
                if (numbers[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (numbers[i] == 0) continue;
                u = numbers[i] % 10; // ones
                t = numbers[i] / 10;
                h = numbers[i] / 100; // hundreds
                t = t - 10 * h; // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i == 0) sb.Append("");
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }
        public DataSet GetData(int EmpId, string PaidMonth)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand("usp_GetSalaryDetails");
            cmd.Connection = dbcl.Conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpId", EmpId);
            cmd.Parameters.AddWithValue("@PaidMonth", PaidMonth);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {

            }
            cmd.Dispose();
            return ds;
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            GeneratePay();
        }
    }
}