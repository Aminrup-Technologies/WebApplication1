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
    public partial class testing : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        public static string EmployeeID = "A84";
        public static string Month = "12";
        public static string Year = "2022";
        public static string Monthname = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            
            dbcl.FindMonthName(Month, ref Monthname);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //GeneratePay();

            //PDF();

            //Demo();

            Demo2();
        }

        //------------ Salary Slip PDF Download ---------------START-------------//
        private void GeneratePay()
        {
            DataSet ds = new DataSet();
            ds = GetData(EmployeeID, Month, Year);
            if (ds != null && ds.Tables.Count == 5 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                Panel pnlPrintControl = new Panel();
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

                Paragraph title1 = new Paragraph();
                title1.Add(new Chunk("FORM XV : [See Rule 77(2)(b)]", new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK)));
                title1.Alignment = 1;
                doc.Add(title1);


                // 3) Adding title table
                Paragraph title = new Paragraph();
                title.Add(new Chunk("Wages Slip for the month of " + Month + "," + Year, new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK)));
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
        Address: Near Samudayik Vikas Bhawan, Jemco Basti, Telco, Jamshedpur - 831004.", new Font(Font.FontFamily.COURIER, 9, 1, BaseColor.BLACK)));
                address.Alignment = 1;
                doc.Add(address);
                doc.Close();
                string fileName = EmployeeID + "_" + Monthname + "_" + Year + ".pdf";
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;" + "filename=" + fileName);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(doc);
                Response.End();
            }
            else
            {
                string title = "Opps :";
                string body = "Salary slip not generated.";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }
        private string AppendComma(string value)
        {
            if (value == "" || value == "0")
            {
                return String.Format("{0:#,0.00}", "0");
            }
            else
            {
                return String.Format("{0:#,0.00}", Convert.ToDecimal(value));
            }
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
            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
            string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
            string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
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
        public DataSet GetData(string EmpId, string PaidMonth, string PaidYear)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand("usp_GetSalaryDetailsats");
            cmd.Connection = dbcl.Conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpId", EmpId);
            cmd.Parameters.AddWithValue("@PaidMonth", PaidMonth);
            cmd.Parameters.AddWithValue("@PaidYear", PaidYear);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {

            }
            cmd.Dispose();
            return ds;
        }
        //------------ Salary Slip PDF Download ----------------END------------//



        private void PDF()
        {
            Byte[] bytes;
            //Instead of a FileStream we'll use a MemoryStream
            using (var MS = new System.IO.MemoryStream())
            {

                //Standard PDF setup, iText doesn't care what type of stream we're using
                var doc = new iTextSharp.text.Document();
                var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, MS);
                doc.SetPageSize(PageSize.A4);
                doc.SetMargins(2, 2, 2, 2);
                doc.Open();
                //doc.Add(new iTextSharp.text.Paragraph("Work Order No :      " + "" + 21250 + ""));
                PdfPTable table = new PdfPTable(4);
                table.AddCell("Row 1, Col 1");
                table.AddCell("Row 1, Col 2");
                table.AddCell("Row 1, Col 3");

                table.AddCell("Row 2, Col 1");
                table.AddCell("Row 2, Col 2");
                table.AddCell("Row 2, Col 3");

                table.AddCell("Row 3, Col 1");
                table.AddCell("Row 3, Col 2");
                table.AddCell("Row 3, Col 3");


                PdfPCell cell = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                table.AddCell("Row 2, Col 1");
                table.AddCell("Row 2, Col 1");
                table.AddCell("Row 2, Col 1");

                table.AddCell("Row 3, Col 1");
                cell = new PdfPCell(new Phrase("Row 3, Col 2 and Col3"));
                cell.Colspan = 2;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Row 4, Col 1 and Col2"));
                cell.Colspan = 2;
                table.AddCell(cell);
                table.AddCell("Row 4, Col 3");

                doc.Add(table);
                doc.Close();

                //Grab the raw bytes from the MemoryStream
                bytes = MS.ToArray();
            }
            Response.Clear();
            //Instead of a normal text/html header tell the browser that we've got a PDF
            Response.ContentType = "application/pdf";
            //Tell the browser that you want the file downloaded (ideally) and give it a pretty filename
            Response.AddHeader("content-disposition", "attachment;filename=MySampleFile.pdf");
            //Write our bytes to the stream
            Response.BinaryWrite(bytes);
            //Close the stream (otherwise ASP.Net might continue to write stuff on our behalf)
            Response.End();
        }


        private void Demo()
        {
            Document document = new Document(PageSize.A4, 15f, 15f, 15f, 15f);
            Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                Phrase phrase = null;
                PdfPCell cell = null;
                PdfPTable table = null;

                document.Open();

                //Header Table
                table = new PdfPTable(1);
                table.TotalWidth = 400f;
                table.LockedWidth = true;
                table.SetWidths(new float[] { 1f });

                //Company Name and Address
                phrase = new Phrase();
                phrase.Add(new Chunk("Microsoft Northwind Traders Company\n\n", FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.RED)));
                phrase.Add(new Chunk("107, Park site,\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("Salt Lake Road,\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("Seattle, USA", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT);
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                table.AddCell(cell);

                document.Add(table);

                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                //Employee Details
                cell = PhraseCell(new Phrase("Employee Record", FontFactory.GetFont("Arial", 12, Font.UNDERLINE, BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);

                table = new PdfPTable(2);
                table.SetWidths(new float[] { 0.5f, 2f });
                table.TotalWidth = 340f;
                table.LockedWidth = true;
                table.SpacingBefore = 20f;
                table.HorizontalAlignment = Element.ALIGN_RIGHT;

                phrase = new Phrase();
                phrase.Add(new Chunk("Mr. Mudassar Ahmed Khan\n", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)));
                phrase.Add(new Chunk("(Moderator)", FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)));
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT);
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                cell.Colspan = 2;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase(" "), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);

                //Employee Id
                table.AddCell(PhraseCell(new Phrase("Employee code:", FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("0001", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 10f;
                table.AddCell(cell);


                //Address
                table.AddCell(PhraseCell(new Phrase("Address:", FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                phrase = new Phrase(new Chunk("507 - 20th Ave. E.\nApt. 2A\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("Seattle\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("WA USA 98122", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                table.AddCell(PhraseCell(phrase, PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 10f;
                table.AddCell(cell);

                //Date of Birth
                table.AddCell(PhraseCell(new Phrase("Date of Birth:", FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("25 FEBRUARY", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 10f;
                table.AddCell(cell);
                document.Add(table);

                //Add border to page
                PdfContentByte content = writer.DirectContent;
                iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(document.PageSize);
                rectangle.Left += document.LeftMargin;
                rectangle.Right -= document.RightMargin;
                rectangle.Top -= document.TopMargin;
                rectangle.Bottom += document.BottomMargin;
                content.SetColorStroke(BaseColor.BLACK);
                content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
                content.Stroke();

                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=Employee.pdf");
                Response.ContentType = "application/pdf";
                Response.Buffer = true;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
                Response.End();
                Response.Close();
            }
        }

        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.BorderColor = BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 0f;
            return cell;
        }

        private void Demo2()
        {
            //Create document
            Document doc = new Document();
            //Create PDF Table
            PdfPTable tableLayout = new PdfPTable(4);
            //Create a PDF file in specific path
            PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("Sample-PDF-File.pdf"), FileMode.Create));
            //Open the PDF document
            doc.Open();
            //Add Content to PDF
            doc.Add(Add_Content_To_PDF(tableLayout));
            // Closing the document
            doc.Close();

            //Open the PDF file
            Process.Start(Server.MapPath("Sample-PDF-File.pdf"));
        }

        private PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {
            float[] headers = { 10, 10, 20, 20 }; //Header Widths
            tableLayout.SetWidths(headers); //Set the pdf headers
            tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  //Add Title to the PDF file at the top
            tableLayout.AddCell(new PdfPCell(new Phrase("Creating PDF file using iTextsharp", new Font(Font.NORMAL, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0))))
            {
                Colspan = 4,
                Border = 0,
                PaddingBottom = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            //Add header
            AddCellToHeader(tableLayout, "Cricketer Name");
            AddCellToHeader(tableLayout, "Height");
            AddCellToHeader(tableLayout, "Born On");
            AddCellToHeader(tableLayout, "Parents");
            //Add body
            AddCellToBody(tableLayout, "Sachin Tendulkar");
            AddCellToBody(tableLayout, "1.65 m");
            AddCellToBody(tableLayout, "April 24, 1973");
            AddCellToBody(tableLayout, "Ramesh Tendulkar, Rajni Tendulkar");
            AddCellToBody(tableLayout, "Mahendra Singh Dhoni");
            AddCellToBody(tableLayout, "1.75 m");
            AddCellToBody(tableLayout, "July 7, 1981");
            AddCellToBody(tableLayout, "Devki Devi, Pan Singh");
            AddCellToBody(tableLayout, "Virender Sehwag");
            AddCellToBody(tableLayout, "1.70 m");
            AddCellToBody(tableLayout, "October 20, 1978");
            AddCellToBody(tableLayout, "Aryavir Sehwag, Vedant Sehwag");
            AddCellToBody(tableLayout, "Virat Kohli");
            AddCellToBody(tableLayout, "1.75 m");
            AddCellToBody(tableLayout, "November 5, 1988");
            AddCellToBody(tableLayout, "Saroj Kohli, Prem Kohli");
            return tableLayout;
        }

        // Method to add single cell to the header
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.NORMAL, 8, 1, iTextSharp.text.BaseColor.WHITE)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(0, 51, 102)
            });
        }
        // Method to add single cell to the body
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.NORMAL, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = iTextSharp.text.BaseColor.WHITE
            });
        }
    }
}