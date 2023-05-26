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

namespace WebApplication1.bussiness.production
{
    public partial class emp_bulkregistration : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string CmdString1 = "select Country_Name, Country_Code from tlb_work_country";
                BindCountry(CmdString1);

                string CmdString2 = "select * from tlb_payroll_category order by Id";
                //BindGrid(CmdString2);
            }
        }
        private void BindCountry(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_WorkCountry.DataSource = Cmd.ExecuteReader();
            DDL_WorkCountry.DataTextField = "Country_Name";
            DDL_WorkCountry.DataValueField = "Country_Code";
            DDL_WorkCountry.DataBind();
            DDL_WorkCountry.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        private void BindGrid(string cmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            //GridView1.DataSource = ds;
            //GridView1.DataBind();
            dbcl.Conn.Close();
        }

        protected void DDL_WorkCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL_Value = DDL_WorkCountry.SelectedValue.ToString();
            string CmdString3 = "select State_Name, State_Code from tlb_work_state where Country_Code = '" + DDL_Value + "' order by Id";
            BindWorkState(CmdString3);
        }

        private void BindWorkState(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_WorkStates.DataSource = Cmd.ExecuteReader();
            DDL_WorkStates.DataTextField = "State_Name";
            DDL_WorkStates.DataValueField = "State_Code";
            DDL_WorkStates.DataBind();
            DDL_WorkStates.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_WorkStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL1_String = DDL_WorkCountry.SelectedValue.ToString();
            string DDL2_String = DDL_WorkStates.SelectedValue.ToString();

            string CmdString3 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where Country_Code = '" + DDL1_String + "' and State_Code='" + DDL2_String + "' order by Id";
            BindWorkRegion(CmdString3);
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

        protected void DDL_Work_Region_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DDL1_Value = DDL_WorkCountry.SelectedValue.ToString();
            string DDL2_Value = DDL_WorkStates.SelectedValue.ToString();
            string DDL3_Value = DDL_WorkRegion.SelectedValue.ToString();

            string CmdString2 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = '" + DDL1_Value + "' and State_Code='" + DDL2_Value + "' and Work_Region_Code='" + DDL3_Value + "' order by Id";
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
                        string filePath = Server.MapPath("~/erp_images/EmpFiles/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
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

        public string RemoveSpecialCharacters(string text)
        {
            return System.Text.RegularExpressions.Regex.Replace(text, @"(\s+|\*|\#|\@|\$)", "");
        }

        private enum Formats
        {
            General = 0,
            Number = 1,
            Decimal = 2,
            Currency = 164,
            Accounting = 44,
            DateShort = 14,
            DateLong = 165,
            Time = 166,
            Percentage = 10,
            Fraction = 12,
            Scientific = 11,
            Text = 49
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
            dt1 = (DataTable)ViewState["AgendaDetails"];
            if (dt1 != null)
            {
                foreach (GridViewRow gvadd in GridView1.Rows)
                {
                    string workmanno = gvadd.Cells[1].Text.ToString();                                               //  1--- Workman Register No
                    string status = gvadd.Cells[2].Text.ToString();                                                  //  2
                    string fname = gvadd.Cells[3].Text.ToString();                                                   //  3--- Employee First Name
                    string mdname = Server.HtmlDecode(gvadd.Cells[4].Text.ToString());                               //  4--- Employee Middle Name
                    string lstname = Server.HtmlDecode(gvadd.Cells[5].Text.ToString());                              //  5--- Employee Last Name
                    string fullname = Server.HtmlDecode(gvadd.Cells[6].Text.ToString());                             //  6
                    string fthrname = Server.HtmlDecode(gvadd.Cells[7].Text.ToString());                             //  7--- Employee Father Name
                    double dob = Convert.ToDouble(gvadd.Cells[8].Text.ToString());                                   //  8--- Employee DOB (ddmmyyyy)
                    DateTime dobdt = DateTime.FromOADate(dob);
                    string dobdate = dobdt.ToShortDateString();

                    string bloodgroup = Server.HtmlDecode(gvadd.Cells[9].Text.ToString());                           //  9--- Employee Blood Group
                    string mobile = Server.HtmlDecode(gvadd.Cells[10].Text.ToString());                              // 10--- Employee Mobile No
                    string qualification = Server.HtmlDecode(gvadd.Cells[11].Text.ToString());                       //  11--- Employee Qualification

                    //string doj = Server.HtmlDecode(gvadd.Cells[12].Text.ToString());                                 // 12--- Employee DOJ
                    double doj = Convert.ToDouble(gvadd.Cells[12].Text.ToString());                                   //  12--- Employee DOB (ddmmyyyy)
                    DateTime dojdt = DateTime.FromOADate(doj);
                    string dojdate = dojdt.ToShortDateString();

                    string worksite = Server.HtmlDecode(gvadd.Cells[13].Text.ToString());                            // 13--- Employee Work site
                    string skilllevel = Server.HtmlDecode(gvadd.Cells[14].Text.ToString());                          // 14--- Employee Skill Category
                    string skilldesg = Server.HtmlDecode(gvadd.Cells[15].Text.ToString());                           // 15--- Employee Skill Designation
                    string emproletype = Server.HtmlDecode(gvadd.Cells[16].Text.ToString());                         // 16--- Employee Type
                    string emprolepermission = Server.HtmlDecode(gvadd.Cells[17].Text.ToString());                   // 17--- Employee Type
                    string workhours = Server.HtmlDecode(gvadd.Cells[18].Text.ToString());                           // 18--- Employee Work Hours
                    string otfactor = Server.HtmlDecode(gvadd.Cells[19].Text.ToString());                            // 19--- Employee OT Factor
                    string rfidno = Server.HtmlDecode(gvadd.Cells[20].Text.ToString());                              // 20--- Employee RFID No

                    //string rfidvalidity = Server.HtmlDecode(gvadd.Cells[21].Text.ToString());                        // 21--- Employee RFID Validity
                    double rfidvalidity = Convert.ToDouble(gvadd.Cells[21].Text.ToString());                                   //  12--- Employee DOB (ddmmyyyy)
                    DateTime rfiddt = DateTime.FromOADate(rfidvalidity);
                    string rfiddate = rfiddt.ToShortDateString();

                    string gpno = Server.HtmlDecode(gvadd.Cells[22].Text.ToString());                                // 22--- Employee Gates Pass no
                    //string gpvalidity = Server.HtmlDecode(gvadd.Cells[23].Text.ToString());                          // 23--- Employee Gate Pass Validity
                    double gpvalidity = Convert.ToDouble(gvadd.Cells[23].Text.ToString());                                   //  12--- Employee DOB (ddmmyyyy)
                    DateTime gpdt = DateTime.FromOADate(gpvalidity);
                    string gpdate = gpdt.ToShortDateString();

                    //string pvvalidity = Server.HtmlDecode(gvadd.Cells[24].Text.ToString());                          // 24--- Employee PV Validity
                    double pvvalidity = Convert.ToDouble(gvadd.Cells[24].Text.ToString());                                   //  12--- Employee DOB (ddmmyyyy)
                    DateTime pvdt = DateTime.FromOADate(pvvalidity);
                    string pvdate = pvdt.ToShortDateString();

                    string uanno = Server.HtmlDecode(gvadd.Cells[25].Text.ToString());                               // 25--- Employee UAN No
                    string esicno = Server.HtmlDecode(gvadd.Cells[26].Text.ToString());                              // 26--- Employee ESIC No
                    string bankname = Server.HtmlDecode(gvadd.Cells[27].Text.ToString());                           // 27--- Employee Bank Account Name
                    string bankaccno = Server.HtmlDecode(gvadd.Cells[28].Text.ToString());                           // 28--- Employee Bank Account No
                    string bankifsc = Server.HtmlDecode(gvadd.Cells[29].Text.ToString());                            // 29--- Employee Bank IFSC Code

                    Insert_EmplyeeMusterData(workmanno, status, fname, mdname, lstname, fullname, fthrname, dobdate, bloodgroup, mobile, qualification, dojdate, worksite, skilllevel, skilldesg, emproletype, emprolepermission, workhours, otfactor, rfidno, rfiddate, gpno, gpdate, pvdate, uanno, esicno, bankname, bankaccno, bankifsc);
                }
            }
        }

        private void Insert_EmplyeeMusterData(string workmanno, string status, string fname, string mdname, string lstname, string fullname, string fthrname, string dobdate, string bloodgroup, string mobile, string qualification, string dojdate, string worksite, string skilllevel, string skilldesg, string emproletype, string emprolepermission, string workhours, string otfactor, string rfidno, string rfiddate, string gpno, string gpdate, string pvdate, string uanno, string esicno, string bankname, string bankaccno, string bankifsc)
        {
            //Code to Generate Unique Employee ID Goes here
            string LoginID = "";
            dbcl.GenerateLoginID(ref LoginID);

            //Code to Generate Unique Login Password Goes here
            string LoginPassword = "";
            Int32 password_length = 6;
            dbcl.GenerateLoginPassword(password_length, ref LoginPassword);

            //Code to Insert values into the DB goes here
            int flag = 0;
            try
            {
                dbcl.Sqlconnection();
                SqlCommand cmd = new SqlCommand("SP_InsertInto_EmployeeMusterTable", dbcl.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WorkStatus", status);
                cmd.Parameters.AddWithValue("@LoginID", LoginID);
                cmd.Parameters.AddWithValue("@LoginPassword", LoginPassword);
                cmd.Parameters.AddWithValue("@WorkCountry", DDL_WorkCountry.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WorkState", DDL_WorkStates.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WorkRegion", DDL_WorkRegion.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WorkCompany", DDL_Company.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WorkmanSL", workmanno);
                cmd.Parameters.AddWithValue("@FirstName", fname);

                if (mdname == "#N/A" || mdname == " ")
                {
                    cmd.Parameters.AddWithValue("@MiddleName", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MiddleName", mdname);
                }

                if (lstname == "#N/A" || lstname == " ")
                {
                    cmd.Parameters.AddWithValue("@LastName", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@LastName", lstname);
                }
                cmd.Parameters.AddWithValue("@FullName", fullname);
                cmd.Parameters.AddWithValue("@Fathername", fthrname);
                cmd.Parameters.AddWithValue("@BloodGroup", bloodgroup);
                cmd.Parameters.AddWithValue("@MobileNo", mobile);
                cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(dobdate));
                cmd.Parameters.AddWithValue("@Qualification", qualification);
                cmd.Parameters.AddWithValue("@DOJ", Convert.ToDateTime(dojdate));
                cmd.Parameters.AddWithValue("@WorkSite", worksite);
                cmd.Parameters.AddWithValue("@Worksite_Code", DBNull.Value);
                cmd.Parameters.AddWithValue("@SkillCategory", skilllevel);
                cmd.Parameters.AddWithValue("@SkillDesignation", skilldesg);
                cmd.Parameters.AddWithValue("@User_RoleType", emproletype);
                cmd.Parameters.AddWithValue("@Role_Permission", emprolepermission);
                cmd.Parameters.AddWithValue("@WorkHours", Convert.ToInt32(workhours));
                cmd.Parameters.AddWithValue("@OTFactor", Convert.ToInt32(otfactor));
                cmd.Parameters.AddWithValue("@SafetyPassNo", rfidno);
                cmd.Parameters.AddWithValue("@SafetyPassExpiry", Convert.ToDateTime(rfiddate));
                cmd.Parameters.AddWithValue("@GatePassNo", gpno);
                cmd.Parameters.AddWithValue("@GatePassExpiry", Convert.ToDateTime(gpdate));
                cmd.Parameters.AddWithValue("@PVExpiry", Convert.ToDateTime(pvdate));
                cmd.Parameters.AddWithValue("@UANNo", uanno);
                cmd.Parameters.AddWithValue("@ESICNo", esicno);
                cmd.Parameters.AddWithValue("@Payment_Bank", bankname);
                cmd.Parameters.AddWithValue("@Payment_Account", bankaccno);
                cmd.Parameters.AddWithValue("@Payment_IFSC", bankifsc);
                cmd.Parameters.AddWithValue("@BankBranch", "");
                cmd.Parameters.AddWithValue("@RegistrationType", "Bulk");
                cmd.Parameters.AddWithValue("@Registered_ByName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@Registered_ByWRK", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@LoginStatus", 0);
                cmd.Parameters.AddWithValue("@LastLogin", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@LastLogout", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@PasswordExpiry", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                dbcl.ConnectDb();
                flag = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                lbl_msg.Visible = true;
                lbl_msg.Text = ex.Message;
                lbl_msg.ForeColor = System.Drawing.Color.IndianRed;
                dbcl.DisconnectDb();
            }

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
            //return flag;
        }
    }
}