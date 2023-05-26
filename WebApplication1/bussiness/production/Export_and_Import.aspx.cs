using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace WebApplication1.bussiness.production
{
    public partial class Export_and_Import : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            excel();
        }

        private void excel()
        {
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                Console.WriteLine("Excel is not properly installed!!");
            }
            else
            {
                //Excel.Workbook xlWorkBook;
                //Excel.Worksheet xlWorkSheet;
                //object misValue = System.Reflection.Missing.Value;
                //xlWorkBook = xlApp.Workbooks.Add(misValue);
                //xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                //xlWorkSheet.Cells[1, 1] = "ID";
                //xlWorkSheet.Cells[1, 2] = "Name";
                //xlWorkSheet.Cells[2, 1] = "1";
                //xlWorkSheet.Cells[2, 2] = "One";
                //xlWorkSheet.Cells[3, 1] = "2";
                //xlWorkSheet.Cells[3, 2] = "Two";
                //xlWorkBook.SaveAs("d:\\Export.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                //xlWorkBook.Close(true, misValue, misValue);
                //xlApp.Quit();
                //Marshal.ReleaseComObject(xlWorkSheet);
                //Marshal.ReleaseComObject(xlWorkBook);
                //Marshal.ReleaseComObject(xlApp);
                //Console.WriteLine("Excel file created , you can find the file d:\\Export.xls");



                //var wbook = new XLWorkbook();
                //var ws = wbook.Worksheets.Add("Sheet1");
                //ws.Cell("A1").Value = "150";
                //wbook.SaveAs("d:\\simple.xlsx");


                using (XLWorkbook wbook = new XLWorkbook())
                {
                    var ws = wbook.Worksheets.Add("Basics"); //This line of code is used to add sheets to the work book, with a name to the added sheet
                    ws.Cell("A1").Value = "150"; //This line of code add and insert a data into the cell A1, first column , first row

                    //-------------------------------------//
                    ws.Cell(3, 2).Value = "Hello there!";
                    ws.Cell("A6").SetValue("falcon").SetActive();
                    ws.Column(2).AdjustToContents();

                    //-------------------------------------//
                    var c1 = ws.Column("A");
                    c1.Width = 25; //Set column width

                    var c2 = ws.Column("B");
                    c2.Width = 15; //Set column width

                    ws.Cell("A3").Value = "an old falcon";  //set a string value
                    ws.Cell("B2").Value = "150";  //set a number value
                    ws.Cell("B5").Value = "Sunny day";  //set a string value to cell

                    ws.Cell("A3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;  //center align- horizontal
                    ws.Cell("A3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center; //vertical align - center
                    ws.Cell("A3").Style.Font.Italic = true; // font styling

                    ws.Cell("B2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin; //set border thin
                    ws.Cell("B5").Style.Font.FontColor = XLColor.Red; //cell text color


                    //-------------------------------------//


                    ws.Range("D2:G2").Style.Fill.BackgroundColor = XLColor.Gray;
                    ws.Ranges("C5, F5:H8").Style.Fill.BackgroundColor = XLColor.Gray;

                    var rand = new Random();
                    var range = ws.Range("C10:E15");

                    foreach (var cell in range.Cells())
                    {
                        cell.Value = rand.Next();
                    }

                    ws.Column("C").AdjustToContents();
                    ws.Column("D").AdjustToContents();
                    ws.Column("E").AdjustToContents();

                    //------------------------------------//

                    ws.Cell("A18").Value = "Sunny day";
                    ws.Cell("A18").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell("A18").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    ws.Range("A18:B18").Merge();

                    //------------------------------------//

                    var ws2 = wbook.Worksheets.Add("Sorting");
                    var rand1 = new Random();
                    var range1 = ws2.Range("A1:A15");

                    foreach (var cell in range1.Cells())
                    {
                        cell.Value = rand1.Next(1, 100);
                    }

                    ws2.Sort("A");

                    //------------------------------------//


                    var ws3 = wbook.Worksheets.Add("Sheet3");

                    ws3.Cell("A1").Value = "sky";
                    ws3.Cell("A2").Value = "cloud";
                    ws3.Cell("A3").Value = "book";
                    ws3.Cell("A4").Value = "cup";
                    ws3.Cell("A5").Value = "snake";
                    ws3.Cell("A6").Value = "falcon";
                    ws3.Cell("B1").Value = "in";
                    ws3.Cell("B2").Value = "tool";
                    ws3.Cell("B3").Value = "war";
                    ws3.Cell("B4").Value = "snow";
                    ws3.Cell("B5").Value = "tree";
                    ws3.Cell("B6").Value = "ten";

                    var n = ws3.Range("A1:C10").CellsUsed().Count();
                    Console.Write($"There are {n} words in the range");

                    Console.Write("The following words have three latin letters:");

                    var words = ws3.Range("A1:C10")
                        .CellsUsed()
                        .Select(c => c.Value.ToString())
                        .Where(c => c?.Length == 3)
                        .ToList();

                    words.ForEach(Console.Write);


                    //------------------------------------------------------------------------------------------//

                    var ws4 = wbook.Worksheets.Add("Functions");
                    var rand4 = new Random();
                    var range4 = ws4.Range("A1:A6"); //write random value inbetween 1 to 100 in A1 to A6 amd sum thoe in A7 CELL

                    foreach (var cell in range4.Cells())
                    {
                        cell.Value = rand4.Next(1, 100);
                    }

                    ws4.Cell("A7").FormulaA1 = "SUM(A1:A6)";
                    ws4.Cell("A7").Style.Font.Bold = true;


                    //---------------------------------------------------------------------------------------------//
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=ATSWebExport.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wbook.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
        }
    } 
}