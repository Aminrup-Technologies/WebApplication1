using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Configuration;

namespace WebApplication1.bussiness.production
{
    public partial class add_expenses : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        DataTable dt1 = new DataTable();
        Boolean FileFlag = false;
        string EXPID = "";
        string Server_FileName = String.Empty;
        string Server_FilePath = String.Empty;
        string FileType = String.Empty;
        string ext = string.Empty;
        Byte[] bytes = { 0 };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["USERTYPE"] == null || Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    CheckFund();
                    if (Session["USTATE"].ToString() =="PI")
                    {
                        string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region order by Id";
                        Bind_WorkRegion(CmdString1);
                    }
                    else
                    {
                        string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region where State_Code='" + Session["USTATE"].ToString() + "'  order by Id";
                        Bind_WorkRegion(CmdString1);

                        DDL_Region.Enabled = false;
                        DDL_Region.SelectedValue = Session["REGION"].ToString();

                        comp_row1.Visible = true;
                        comp_row2.Visible = true;

                        string CmdString5 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='" + Session["USTATE"].ToString() + "' and Work_Region_Code = '" + Session["REGION"].ToString() + "' and Company_Code = '" + Session["COMPANY_CODE"].ToString() + "' order by Id ";
                        BindCompany(CmdString5);

                        DDL_Company.SelectedValue = Session["COMPANY_CODE"].ToString();
                        DDL_Company.Enabled = false;

                        dept_row1.Visible = true;
                        dept_row2.Visible = true;

                        string CmdString3 = "select Company_Department, DB_Code from tlb_workregion_compdept where Country_Code = 'IN' and State_Code ='" + Session["USTATE"].ToString() + "' and Work_Region_Code = '" + Session["REGION"].ToString() + "' and Company_Code = '" + Session["COMPANY_CODE"].ToString() + "'  order by Id ";
                        BindCompanyDept(CmdString3);

                        wrkordr_row1.Visible = true;
                        wrkordr_row2.Visible = true;

                        string CmdString2 = "select WO_Number, DB_Code from tlb_WO_Data where Work_Region_Code = '" + Session["REGION"].ToString() + "' and Company_Code = '" + Session["COMPANY_CODE"].ToString() + "' and WO_Status='Active' order by WO_Type";
                        BindWorkorder(CmdString2);

                        CheckUser();

                        string query = "select CompDept_Location,DB_Code from tlb_workregion_compdept_loc where Dept_DBCode='" + DDL_CompDept.SelectedValue.ToString() + "'";
                        BindDepLoc(query);

                        loc_row1.Visible = true;
                        loc_row2.Visible = true;
                    }
                }
            }
        }

        private void CheckUser()
        {
            if (Session["WORKMAN"].ToString() == "J4")
            {
                DDL_CompDept.SelectedIndex = 1;
                DDL_CompDept.Enabled = false;

                DDL_Workorder.SelectedIndex = 1;
                DDL_Workorder.Enabled = false;
            }
        }

        private void CheckFund()
        {
            try
            {
                string query = "select BudgetAmount,UsedAmount, RemAmount from tbl_expbudget where EmployeeWrk=@EmployeeWrk";
                SqlParameter[] pram = {
                                          new SqlParameter("@EmployeeWrk", Session["WORKMAN"].ToString()),
                                      };
                dt1 = dbcl.SPreturn_dt(query, pram);
                if (dt1.Rows.Count > 0)
                {
                    string Budgetamt = dt1.Rows[0]["BudgetAmount"].ToString();
                    lbl_budgetamnt.Text = Budgetamt;
                    string Usedamt = dt1.Rows[0]["UsedAmount"].ToString();
                    lbl_usedamnt.Text = Usedamt;
                    string Leftamt = dt1.Rows[0]["RemAmount"].ToString();
                    lbl_leftamnt.Text = Leftamt;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                lbl_msg.ForeColor = System.Drawing.Color.Red;
                lbl_msg.Text = "Error: " + ex.Message.ToString();
            }
        }

        private void Bind_WorkRegion(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Region.DataSource = Cmd.ExecuteReader();
            DDL_Region.DataTextField = "Work_Region_Name";
            DDL_Region.DataValueField = "Work_Region_Code";
            DDL_Region.DataBind();
            DDL_Region.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        private string Find_ExpenseDBCode()
        {
            string aa = null;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string kk = null;
            string cmdString1 = "select Id,ExpenseID from tbl_expenselogs where Id=(select max(Id)from tbl_expenselogs)";
            SqlCommand com1 = new SqlCommand(cmdString1, dbcl.Conn);
            SqlDataReader DR1 = com1.ExecuteReader();
            if (DR1.Read())
            {
                aa = DR1.GetValue(1).ToString();
                string bb = aa.Substring(5);
                int k = Convert.ToInt32(bb);
                k = k + 1;
                string q = Convert.ToString(k);
                kk = "EXP00" + q;
            }
            else
            {
                kk = "EXP001";
            }
            dbcl.DisconnectDb();
            return kk;
        }

        protected void ImportPermit(object sender, EventArgs e)
        {
            lblMessage.Visible = true;
            string filePath = FileUpload1.PostedFile.FileName; // getting the file path of uploaded file
            string filpath = Path.GetFileName(filePath); // getting the file name of uploaded file

            EXPID = Find_ExpenseDBCode();



            ext = Path.GetExtension(filpath); // getting the file extension of uploaded file
            lbl_ext.Text = ext;

            Server_FileName = EXPID + "_" + Path.GetFileName(filePath); // getting the file name of uploaded file
            lbl_filename.Text = Server_FileName;

            if (!FileUpload1.HasFile)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup1();", true);
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please Select File"; //if file uploader has no file selected

                lbl_fileyesno.Text = "No";
                FileFlag = false;
            }
            else
            if (FileUpload1.HasFile)
            {
                try
                {
                    if (DDL_UploadType.SelectedIndex == 1)
                    {
                        switch (ext) // this switch code validate the files which allow to upload only PDF file
                        {
                            case ".pdf":
                                FileType = "application/pdf";
                                break;
                        }
                    }
                    else if (DDL_UploadType.SelectedIndex == 2)
                    {
                        switch (ext) // this switch code validate the files which allow to upload only PDF file
                        {
                            case ".jpg":
                                FileType = "image/jpg";
                                break;

                            case ".jpeg":
                                FileType = "image/jpeg";
                                break;
                        }
                    }
                    lbl_filetype.Text = FileType;

                    if (FileType != String.Empty)
                    {
                        //Save the uploaded Excel file.
                        Server_FilePath = Server.MapPath(@"\erp_images\Expenses\")+ EXPID + "_" + Path.GetFileName(FileUpload1.PostedFile.FileName);
                        FileUpload1.SaveAs(Server_FilePath);

                        Stream fs = FileUpload1.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(fs); //reads the binary files
                        bytes = br.ReadBytes((Int32)fs.Length); //counting the file length into bytes

                        lbl_data.Text = bytes.ToString();
                        //ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup1();", true);

                        string title = "Notifications :";
                        string body = "File Uploaded, Proceed...!!";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                        lblMessage.ForeColor = System.Drawing.Color.Green;

                        //FileUpload1.Enabled = false;
                        //btnUpload.Enabled = false;
                        //btnUpload.Text = "Uploaded";
                        lblMessage.Text = "File Uploaded Successfully";

                        FileFlag = true;
                        lbl_fileyesno.Text = "Yes";

                        //InsertIntoDB(Server_FileName, FileType, ext, bytes);

                        pdfuploadbuttonrow1.Visible = false;
                        pdfuploadbuttonrow2.Visible = false;

                        Div3.Visible = true;
                        lbl_filename.Visible = true;
                        Div4.Visible = true;
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup1();", true);
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "Select Only PDF File having extension (.pdf) ";

                        FileFlag = false;
                        lbl_fileyesno.Text = "No";
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Error: " + ex.Message.ToString();

                    FileFlag = false;
                    lbl_fileyesno.Text = "No";
                }
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (Insert_JOBData() != 0)
            {
                expense_created.Visible = true;
                addexpenserow.Visible = false;
                submitbtns.Visible = false;

                AddExpensesButtons.Visible = false;
                btn_finish.Enabled = false;
            }
            else
            {
                expense_created.Visible = false;
                addexpenserow.Visible = true;
                submitbtns.Visible = true;
            }
        }

        private Int32 Insert_JOBData()
        {
            //Code to Generate Unique Employee ID Goes here
            if (EXPID == "")
            {
                EXPID = Find_ExpenseDBCode();
            }

            lbl_expid.Text = EXPID;
            string rgn_code = DDL_Region.SelectedValue.ToString();
            string rgn_name = DDL_Region.SelectedItem.ToString();

            string comp_code = DDL_Company.SelectedValue.ToString();
            string comp_name = DDL_Company.SelectedItem.ToString();

            string compdept_code = DDL_CompDept.SelectedValue.ToString();
            string compdept_name = DDL_CompDept.SelectedItem.ToString();

            //Code to Insert values into the DB goes here
            int flag = 0;
            try
            {
                dbcl.Sqlconnection();
                SqlCommand cmd = new SqlCommand("SP_InsertInto_tbl_expenselogs", dbcl.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ExpenseID", EXPID);
                cmd.Parameters.AddWithValue("@CountryCode", "IN");
                cmd.Parameters.AddWithValue("@StateCode", Session["USTATE"].ToString());
                cmd.Parameters.AddWithValue("@RegionCode", rgn_code);
                cmd.Parameters.AddWithValue("@CompanyCode", comp_code);
                cmd.Parameters.AddWithValue("@CompDept", compdept_code);
                cmd.Parameters.AddWithValue("@Location", DDL_Location.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@WorksiteCode", DDL_Worksite.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@WorksiteName", DDL_Worksite.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@WorkorderNo", DDL_Workorder.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@LoggedByWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@LoggedByName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@LoggedOn", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@AppStatus", "Pending");
                cmd.Parameters.AddWithValue("@AppByWrk", "J3");
                cmd.Parameters.AddWithValue("@AppByName", "MAHESH CHOURASIA");
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
                lbl_msg.Text = "Record Inserted Successfully..!";
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
            return flag;
        }

        protected void DDL_Region_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_Region.SelectedIndex != 0)
            {
                comp_row1.Visible = true;
                comp_row2.Visible = true;

                string rgn_code = DDL_Region.SelectedValue.ToString();
                string rgn_name = DDL_Region.SelectedItem.ToString();

                string CmdString3 = "select Company_Name, Company_Code from tlb_workregion_company where Country_Code = 'IN' and State_Code ='" + Session["USTATE"].ToString() + "' and Work_Region_Code = '" + rgn_code + "' order by Id ";
                BindCompany(CmdString3);
            }
            else
            {
                comp_row1.Visible = false;
                comp_row2.Visible = false;
            }
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
            if (DDL_Company.SelectedIndex != 0)
            {
                dept_row1.Visible = true;
                dept_row2.Visible = true;

                string rgn_code = DDL_Region.SelectedValue.ToString();
                string rgn_name = DDL_Region.SelectedItem.ToString();

                string comp_code = DDL_Company.SelectedValue.ToString();
                string comp_name = DDL_Company.SelectedItem.ToString();

                string CmdString3 = "select Company_Department, DB_Code from tlb_workregion_compdept where Country_Code = 'IN' and State_Code ='" + Session["USTATE"].ToString() + "' and Work_Region_Code = '" + rgn_code + "' and Company_Code = '" + comp_code + "'  order by Id ";
                BindCompanyDept(CmdString3);
            }
            else
            {
                dept_row1.Visible = false;
                dept_row2.Visible = false;
            }
        }

        private void BindCompanyDept(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_CompDept.DataSource = Cmd.ExecuteReader();
            DDL_CompDept.DataTextField = "Company_Department";
            DDL_CompDept.DataValueField = "DB_Code";
            DDL_CompDept.DataBind();
            DDL_CompDept.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_CompDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_CompDept.SelectedIndex != 0)
            {
                wrkordr_row1.Visible = true;
                wrkordr_row2.Visible = true;

                loc_row1.Visible = true;
                loc_row2.Visible = true;

                string rgn_code = DDL_Region.SelectedValue.ToString();
                string rgn_name = DDL_Region.SelectedItem.ToString();

                string comp_code = DDL_Company.SelectedValue.ToString();
                string comp_name = DDL_Company.SelectedItem.ToString();

                string compdept_code = DDL_CompDept.SelectedValue.ToString();
                string compdept_name = DDL_CompDept.SelectedItem.ToString();

                string CmdString2 = "select WO_Number, DB_Code from tlb_WO_Data where Work_Region_Code = '" + rgn_code + "' and Company_Code = '" + comp_code + "' and Dept_DBCode='" + compdept_code + "' and WO_Status='Active' order by WO_Type";
                BindWorkorder(CmdString2);

                string query = "select CompDept_Location,DB_Code from tlb_workregion_compdept_loc where Dept_DBCode='" + compdept_code + "'";
                BindDepLoc(query);
            }
            else
            {
                wrkordr_row1.Visible = false;
                wrkordr_row2.Visible = false;

                loc_row1.Visible = false;
                loc_row2.Visible = false;
            }
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

        private void BindDepLoc(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Location.DataSource = Cmd.ExecuteReader();
            DDL_Location.DataTextField = "CompDept_Location";
            DDL_Location.DataValueField = "DB_Code";
            DDL_Location.DataBind();
            DDL_Location.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_Location_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_Location.SelectedIndex != 0)
            {
                string rgn_code = DDL_Region.SelectedValue.ToString();
                string rgn_name = DDL_Region.SelectedItem.ToString();

                string comp_code = DDL_Company.SelectedValue.ToString();
                string comp_name = DDL_Company.SelectedItem.ToString();

                string compdept_code = DDL_CompDept.SelectedValue.ToString();
                string compdept_name = DDL_CompDept.SelectedItem.ToString();

                string CmdString2 = "select Worksite_Name, DB_Code from tlb_atsworksites where WorkRegion_Code='" + rgn_code + "' and Company_Code = '" + comp_code + "' and Dept_DBCode='" + compdept_code + "' order by Id";
                BindWorkSites(CmdString2);

                DDL_Worksite.SelectedIndex = 1;

                worksite_row1.Visible = true;
                worksite_row2.Visible = true;

                submitbtns.Visible = true;
            }
            else
            {
                worksite_row1.Visible = false;
                worksite_row2.Visible = false;

                submitbtns.Visible =false;
            }
        }

        private void Bind_ExpHeads(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_ExpHeads.DataSource = Cmd.ExecuteReader();
            DDL_ExpHeads.DataTextField = "ExpenseHead";
            DDL_ExpHeads.DataValueField = "ExpenseHeadCode";
            DDL_ExpHeads.DataBind();
            DDL_ExpHeads.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        private void BindWorkSites(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Worksite.DataSource = Cmd.ExecuteReader();
            DDL_Worksite.DataTextField = "Worksite_Name";
            DDL_Worksite.DataValueField = "DB_Code";
            DDL_Worksite.DataBind();
            DDL_Worksite.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_ExpHeads_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_ExpHeads.SelectedIndex != 0)
            {
                //subhead_row1.Visible = true;
                //subhead_row2.Visible = true;

                quantity_row1.Visible = true;
                quantity_row2.Visible = true;

                expdescp_row1.Visible = true;
                expdescp_row2.Visible = true;

                AddExpensesButtons.Visible = true;
                pdfuploadbuttonrow1.Visible = true;
                pdfuploadbuttonrow2.Visible = true;

                expamnt_row1.Visible = true;
                expamnt_row2.Visible = true;

                //string query = "select SubHead,SubHead from tlb_expsubheads where Region='" + Session["REGION"].ToString() + "' and HeadCode = '" + DDL_ExpHeads.SelectedValue.ToString() + "' order by Id";
                //Bind_ExpSubHeads(query);
            }
            else
            {
                subhead_row1.Visible = false;
                subhead_row2.Visible = false;

                quantity_row1.Visible = false;
                quantity_row2.Visible = false;
            }
        }

        private void Bind_ExpSubHeads(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_SubHeads.DataSource = Cmd.ExecuteReader();
            DDL_SubHeads.DataTextField = "SubHead";
            DDL_SubHeads.DataValueField = "SubHead";
            DDL_SubHeads.DataBind();
            DDL_SubHeads.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void DDL_SubHeads_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_SubHeads.SelectedIndex != 0)
            {
                expdescp_row1.Visible = true;
                expdescp_row2.Visible = true;

                AddExpensesButtons.Visible = true;
                pdfuploadbuttonrow1.Visible = true;
                pdfuploadbuttonrow2.Visible = true;

                expamnt_row1.Visible = true;
                expamnt_row2.Visible = true;
            }
            else
            {
                expdescp_row1.Visible = false;
                expdescp_row2.Visible = false;

                AddExpensesButtons.Visible = false;

                pdfuploadbuttonrow1.Visible = false;
                pdfuploadbuttonrow2.Visible = false;

                expamnt_row1.Visible = false;
                expamnt_row2.Visible = false;
            }
        }

        protected void btn_addexp_Click(object sender, EventArgs e)
        {
            Boolean Flag = false;

            if (Flag == false)
            {
                expense_add.Visible = true;
                heads_row1.Visible = true;
                heads_row2.Visible = true;

                string query = "select ExpenseHead,ExpenseHeadCode from tlb_expheads where Region='"+ Session["REGION"].ToString() + "' order by SlNo";
                Bind_ExpHeads(query);

                Flag = true;

                Addexpbtnrow.Visible = false;
                ViewState_TableRow.Visible = true;
                AddDefaultFirstRecord();
            }
        }

        private void AddDefaultFirstRecord()
        {
            //creating dataTable
            DataTable dt = new DataTable();
            DataRow dr;
            dt.TableName = "Expenses";
            dt.Columns.Add(new DataColumn("slno", typeof(string)));
            dt.Columns.Add(new DataColumn("ExpHead", typeof(string)));
            //dt.Columns.Add(new DataColumn("ExpSubHead", typeof(string)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("ClaimAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("FileName", typeof(string)));
            dt.Columns.Add(new DataColumn("FileType", typeof(string)));
            dt.Columns.Add(new DataColumn("Extension", typeof(string)));
            dt.Columns.Add(new DataColumn("Data", typeof(string)));
            dr = dt.NewRow();
            dt.Rows.Add(new object[] { 1, "No Data" , "No Data" , "No Data" , "No Data" , "No Data" , "No Data" , "No Data" , "No Data"});

            //saving databale into viewstate
            ViewState["Expenses"] = dt;

            //bind Gridview
            BindMyGridview();
        }

        public void BindMyGridview()
        {
            if (ViewState["Expenses"] != null)
            {
                DataTable dt = (DataTable)ViewState["Expenses"];

                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    GridView1.Visible = true;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                else
                {
                    GridView1.Visible = false;
                }
            }
        }

        protected void btn_addbtns_Click(object sender, EventArgs e)
        {
            if (DDL_ExpHeads.SelectedIndex != 0)
            {
                ADDTOLIST();
                Div3.Visible = false;
                finaldisptach.Visible = true;
                btn_finish.Enabled = true;

                pdfuploadbuttonrow1.Visible = true;
                pdfuploadbuttonrow2.Visible = true;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "719 : ____!";
            }
        }

        protected void ADDTOLIST()
        {
            if (ViewState["Expenses"] != null)
            {
                //get datatable from view state
                DataTable dtCurrentTable = (DataTable)ViewState["Expenses"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        drCurrentRow = dtCurrentTable.NewRow();

                        drCurrentRow["ExpHead"] = DDL_ExpHeads.SelectedItem.Text.ToString();
                        //drCurrentRow["ExpSubHead"] = DDL_SubHeads.SelectedItem.Text.ToString();
                        drCurrentRow["Quantity"] = txt_quantity.Text.ToString();

                        //----------Added on 13-05-20201--------------------------------//
                        drCurrentRow["Description"] = txt_expdescp.Text.ToString();
                        drCurrentRow["ClaimAmount"] = Convert.ToDecimal(txt_amount.Text.ToString());
                        //drCurrentRow["FileName"] = Server_FileName;
                        //drCurrentRow["FileType"] = FileType;
                        //drCurrentRow["Extension"] = ext;
                        //drCurrentRow["Data"] = bytes;

                        drCurrentRow["FileName"] = lbl_filename.Text.ToString();
                        drCurrentRow["FileType"] = lbl_filetype.Text.ToString();
                        drCurrentRow["Extension"] =lbl_ext.Text.ToString();
                        drCurrentRow["Data"] = lbl_data.Text.ToString();
                    }

                    if (dtCurrentTable.Rows[0][0].ToString() != "")
                    {
                        dtCurrentTable.Rows[0].Delete();
                        dtCurrentTable.AcceptChanges();
                    }


                    //add created Rows into dataTable
                    dtCurrentTable.Rows.Add(drCurrentRow);

                    //Save Data table into view state after creating each row
                    ViewState["Expenses"] = dtCurrentTable;

                    //Bind Gridview with latest Row
                    BindMyGridview();

                    DDL_ExpHeads.SelectedIndex = 0;
                    //DDL_SubHeads.SelectedIndex = 0;
                    txt_quantity.Text = "1";
                    txt_amount.Text = "";
                    txt_expdescp.Text = "N/A";

                    Server_FileName = "";
                    FileType = "";
                    ext = "";

                    Div3.Visible = false;
                    lbl_filename.Text = "";
                    lbl_filename.Visible = false;
                    lbl_filetype.Text = "";
                    lbl_ext.Text = "";
                    lbl_data.Text = "";

                }
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["Expenses"];
            DataRow dr = dtCurrentTable.Rows[e.RowIndex];
            dtCurrentTable.Rows.Remove(dr);
            GridView1.EditIndex = -1;
            BindMyGridview();
        }

        protected void btn_finish_Click(object sender, EventArgs e)
        {
            if (DataTablePull() == true)
            {
                expense_add.Visible = false;
                heads_row1.Visible = false;
                heads_row2.Visible = false;
                ViewState_TableRow.Visible = false;

                AddExpensesButtons.Visible = false;
                finaldisptach.Visible = false;
                ExpensesAdded.Visible = true;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "814 : ____!";
            }
        }

        protected Boolean DataTablePull()
        {
            Boolean flag = false;

            DataTable dt1;
            dt1 = (DataTable)ViewState["Expenses"];
            if (dt1 != null)
            {
                Int32 i;
                for (i = 0; i <= dt1.Rows.Count - 1; i++)
                {
                    string head = ((Label)GridView1.Rows[i].FindControl("lbl_ExpHead")).Text;
                    //string subhead = ((Label)GridView1.Rows[i].FindControl("lbl_ExpSubHead")).Text;
                    string qnty = ((Label)GridView1.Rows[i].FindControl("lbl_Quantity")).Text;

                    string descp = ((Label)GridView1.Rows[i].FindControl("lbl_Description")).Text;
                    string claimamnt = ((Label)GridView1.Rows[i].FindControl("lbl_ClaimAmount")).Text;
                    string filename = ((Label)GridView1.Rows[i].FindControl("lbl_FileName")).Text;

                    string filetype = ((Label)GridView1.Rows[i].FindControl("lbl_FileType")).Text;
                    string fileext = ((Label)GridView1.Rows[i].FindControl("lbl_Extension")).Text;
                    string data = ((Label)GridView1.Rows[i].FindControl("lbl_Data")).Text;

                    Int32 returnflag = InsertIntoAttendanceTable(head, qnty, descp, claimamnt, filename, filetype, fileext, data);
                    if (returnflag != 0)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            return flag;
        }

        private Int32 InsertIntoAttendanceTable(string head, string qnty, string descp, string claimamnt, string filename, string filetype, string fileext, string data)
        {
            Int32 flag = 0;
            Byte[] bytes = { 0 };
            try
            {
                dbcl.Sqlconnection();
                SqlCommand cmd = new SqlCommand("SP_InsertInto_tbl_expenselogdetails", dbcl.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ExpenseID", lbl_expid.Text.ToString());
                cmd.Parameters.AddWithValue("@ExpHead", head);
                cmd.Parameters.AddWithValue("@ExpSubHead", "No Data");
                cmd.Parameters.AddWithValue("@Quantity", qnty);
                cmd.Parameters.AddWithValue("@Description", descp);
                cmd.Parameters.AddWithValue("@ClaimAmount", Convert.ToDecimal(claimamnt));
                cmd.Parameters.AddWithValue("@FileName", filename);
                cmd.Parameters.AddWithValue("@FileType", filetype);
                cmd.Parameters.AddWithValue("@Extension", fileext);
                cmd.Parameters.AddWithValue("@Data", bytes);
                cmd.Parameters.AddWithValue("@AppStatus", "Pending");
                cmd.Parameters.AddWithValue("@AppByWrk", "J3");
                cmd.Parameters.AddWithValue("@AppByName", "MAHESH CHOURASIA");

                dbcl.ConnectDb();
                flag = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                lbl_fnlmsg.Visible = true;
                lbl_fnlmsg.Text = ex.Message;
                lbl_fnlmsg.ForeColor = System.Drawing.Color.IndianRed;
                dbcl.DisconnectDb();
            }

            if (flag != 0)
            {
                lbl_fnlmsg.Visible = true;
                lbl_fnlmsg.Text = "Record Inserted Successfully..!";
                lbl_fnlmsg.ForeColor = System.Drawing.Color.DarkGreen;
                dbcl.DisconnectDb();
            }
            else
            {
                lbl_fnlmsg.Visible = true;
                lbl_fnlmsg.Text = "Records Connot be Inserted into the Database";
                lbl_fnlmsg.ForeColor = System.Drawing.Color.IndianRed;
                dbcl.DisconnectDb();
            }
            return flag;
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            Response.Redirect("add_expenses.aspx");
        }
    }
}