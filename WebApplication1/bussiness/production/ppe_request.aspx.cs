using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;

namespace WebApplication1.bussiness.production
{
    public partial class ppe_request : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
        DataTable dt = new DataTable();
        static string imglink = "~\\images\\No_Image.jpg";
        static string imgfilename = "N/A";


        static string emp_dept = "";
        static string emp_deptcode = "";

        static string deptcode = "";

        static string emp_loc = "";
        static string emp_loccode = "";

        static string empwrk = "";
        static string name = "";
        static string WorkSite = "";
        static string Worksite_Code = "";

        static string desg = "";
        static string desg_Code = "";

        static string incharge = "";
        static string incharge_Code = "";

        string submittername = "";
        string submitterwrk = "";
        string submitterrgn = "";

        static string ppeitemname = "";


        static string helmet_rqid = "N/A";
        static string SftyShoes_rqid = "N/A";
        static string DutyShirt_rqid = "N/A";
        static string DutyPant_rqid = "N/A";
        static string SafetyGoogles_rqid = "N/A";
        static string NoseMask_rqid = "N/A";
        static string CottonGloves_rqid = "N/A";
        static string BlackGoogles_rqid = "N/A";
        static string PVCGloves_rqid = "N/A";
        static string LthrGloves_rqid = "N/A";
        static string LegGard_rqid = "N/A";
        static string HandSleves_rqid = "N/A";
        static string FRJacket_rqid = "N/A";
        static string Apron_rqid = "N/A";

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
                    input_row1.Visible = true;
                    input_row2.Visible = true;

                    txt_empworkman.Focus();
                }
            }
        }

        protected void WorksiteBinder()
        {
            string CmdString2 = "select Worksite_Name, DB_Code from tlb_atsworksites where WorkRegion_Code='" + Session["REGION"].ToString() + "' order by Id";
            BindWorkSites(CmdString2);
        }

        private void BindWorkSites(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Dept.DataSource = Cmd.ExecuteReader();
            DDL_Dept.DataTextField = "Worksite_Name";
            DDL_Dept.DataValueField = "DB_Code";
            DDL_Dept.DataBind();
            DDL_Dept.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        private void Binder()
        {
            loc_row1.Visible = true;
            loc_row2.Visible = true;


            string cmdString = "select Dept_DBCode from tlb_atsworksites where DB_Code='" + DDL_Dept.SelectedValue.ToString() + "'";
            string deptcode = "";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                deptcode = Rdr["Dept_DBCode"].ToString();
            }
            dbcl.Conn.Close();




            LocBinder(deptcode);

            sftyofcrrow1.Visible = true;
            sftyofcrrow2.Visible = true;
            string CmdString1 = "select FullName, WorkmanSL from tbl_Employee_Mustertable where SkillDesignation = 'SAFETY OFFICER' and WorkStatus = 'Active' and WorkRegion = '" + Session["REGION"].ToString() + "' order by Id";
            Bind_SafetyOfficer(DDL_SftyOfcr, CmdString1);

            inchargerow1.Visible = true;
            inchargerow2.Visible = true;
            string CmdString = "select Employee_Name, Employee_Workman from tlb_atsworksiteIncharges where DB_Code='" + DDL_Dept.SelectedValue.ToString() + "' and Status='Active' order by Id";
            Bind_Approver(CmdString);

            input_row1.Visible = true;
            input_row2.Visible = true;
        }

        protected void DDL_Dept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_Dept.SelectedIndex != 0)
            {
                loc_row1.Visible = true;
                loc_row2.Visible = true;


                string cmdString = "select Dept_DBCode from tlb_atsworksites where DB_Code='" + DDL_Dept.SelectedValue.ToString() + "'";
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                SqlDataReader Rdr;
                Rdr = cmd.ExecuteReader();
                if (Rdr.Read())
                {
                    deptcode = Rdr["Dept_DBCode"].ToString();
                }
                dbcl.Conn.Close();


                LocBinder(deptcode);

                sftyofcrrow1.Visible = true;
                sftyofcrrow2.Visible = true;
                string CmdString1 = "select FullName, WorkmanSL from tbl_Employee_Mustertable where SkillDesignation = 'SAFETY OFFICER' and WorkStatus = 'Active' and WorkRegion = '" + Session["REGION"].ToString() + "' order by FullName";
                Bind_SafetyOfficer(DDL_SftyOfcr, CmdString1);


                inchargerow1.Visible = true;
                inchargerow2.Visible = true;
                string CmdString = "select Employee_Name, Employee_Workman from tlb_atsworksiteIncharges where DB_Code='" + DDL_Dept.SelectedValue.ToString() + "' and Status='Active' order by Employee_Name";
                Bind_Approver(CmdString);
            }
            else
            {
                loc_row1.Visible = false;
                loc_row2.Visible = false;
            }
        }

        private void Bind_Approver(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_Approver.DataSource = Cmd.ExecuteReader();
            DDL_Approver.DataTextField = "Employee_Name";
            DDL_Approver.DataValueField = "Employee_Workman";
            DDL_Approver.DataBind();
            DDL_Approver.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }

        protected void LocBinder(string deptcode)
        {
            string CmdString3 = "select CompDept_Location, DB_Code from tlb_workregion_compdept_loc where Work_Region_Code='" + Session["REGION"].ToString() + "' and Company_Code='" + Session["COMPANY_CODE"].ToString() + "' and Dept_DBCode='" + deptcode + "' order by Id";
            BindLocations(CmdString3);
        }

        private void BindLocations(string CmdString)
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

        private void GridBinder(string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(CmdString, dbcl.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            dbcl.Conn.Close();
        }

        protected void txt_empworkman_TextChanged(object sender, EventArgs e)
        {
            string entryempwrk = txt_empworkman.Text.ToString();
            if (entryempwrk != "")
            {
                string query = "select WorkmanSL, FullName, MobileNo, WorkSite, SkillDesignation from tbl_Employee_Mustertable where WorkRegion = '" + Session["REGION"].ToString() + "' and WorkmanSL = '" + entryempwrk + "' and WorkStatus='Active'";
                GridBinder(query);

                //If ID created & saved in draft mode then , display that data or else start from fresh
                Int32 DraftCount = CC.Find_PPEAuditDraftModeCount(entryempwrk);
                if (DraftCount == 1)
                {
                    DraftDataBinder(entryempwrk);

                    CreateIDRow.Visible = false;
                    dept_row1.Visible = false;
                    dept_row2.Visible = false;

                    ID_CreatedMsg.Visible = true; id_creation.Visible = false; CreateIDRow.Visible = false;
                    Panel2Buttons.Visible = true;

                    TableBox.Visible = true;
                    string title = "Notifications :";
                    string body = "Draft Mode Resumed";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    WorksiteBinder();
                    CreateIDRow.Visible = true;
                    dept_row1.Visible = true;
                    dept_row2.Visible = true;
                }
            }
            else
            {
                dept_row1.Visible = false;
                dept_row2.Visible = false;

                CreateIDRow.Visible = false;
                string title = "Notifications :";
                string body = "Please enter Employee Workman SL";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }


        private void DraftDataBinder(string entryempwrk)
        {
            try
            {
                string query = "select * from tbl_manualppeaudit where EmpWrk=@EmpWrk and DraftMode='Yes' and Panel2Status='Pending'";
                SqlParameter[] pram = {
                                          new SqlParameter("@EmpWrk",entryempwrk),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    FindName(entryempwrk, ref name, ref WorkSite, ref Worksite_Code);
                    empwrk = entryempwrk;
                    lbl_PPCID.Text = dt.Rows[0]["PPCID"].ToString();

                    emp_dept = dt.Rows[0]["EmpDeptName"].ToString();
                    emp_deptcode = dt.Rows[0]["EmpDeptCode"].ToString();

                    emp_loc = dt.Rows[0]["EmpLocName"].ToString();
                    emp_loccode = dt.Rows[0]["EmpLocCode"].ToString();

                    WorkSite= dt.Rows[0]["EmpWorksiteName"].ToString();
                    Worksite_Code = dt.Rows[0]["EmpWorksiteCode"].ToString();

                    helmet_rqid = dt.Rows[0]["Helmet_RQID"].ToString();
                    if (helmet_rqid != "N/A")
                    {
                        lbl_helmetrqid.Text = helmet_rqid;
                        RBTN_Helmet.SelectedValue = "1";
                        RBTN_Helmet.Enabled = false;
                    }

                    SftyShoes_rqid = dt.Rows[0]["SafetyShoes_RQID"].ToString();
                    if (SftyShoes_rqid != "N/A")
                    {
                        lbl_SftyShoes_rqid.Text = SftyShoes_rqid;
                        RBTN_SftyShoes.SelectedValue = "1";
                        RBTN_SftyShoes.Enabled = false;
                    }

                    DutyShirt_rqid = dt.Rows[0]["DutyShirt_RQID"].ToString();
                    if (DutyShirt_rqid != "N/A")
                    {
                        lbl_DutyShirt_rqid.Text = DutyShirt_rqid;
                        RBTN_DutyShirt.SelectedValue = "1";
                        RBTN_DutyShirt.Enabled = false;
                    }

                    DutyPant_rqid = dt.Rows[0]["DutyPant_RQID"].ToString();
                    if (DutyPant_rqid != "N/A")
                    {
                        lbl_DutyPant_rqid.Text = DutyPant_rqid;
                        RBTN_DutyPant.SelectedValue = "1";
                        RBTN_DutyPant.Enabled = false;
                    }

                    SafetyGoogles_rqid = dt.Rows[0]["SafetyGoogles_RQID"].ToString();
                    if (SafetyGoogles_rqid != "N/A")
                    {
                        lbl_SafetyGoogles_rqid.Text = SafetyGoogles_rqid;
                        RBTN_SafetyGoogles.SelectedValue = "1";
                        RBTN_SafetyGoogles.Enabled = false;
                    }

                    NoseMask_rqid = dt.Rows[0]["NoseMask_RQID"].ToString();
                    if (NoseMask_rqid != "N/A")
                    {
                        lbl_NoseMask_rqid.Text = NoseMask_rqid;
                        RBTN_NoseMask.SelectedValue = "1";
                        RBTN_NoseMask.Enabled = false;
                    }

                    CottonGloves_rqid = dt.Rows[0]["CottonGloves_RQID"].ToString();
                    if (CottonGloves_rqid != "N/A")
                    {
                        lbl_CottonGloves_rqid.Text = CottonGloves_rqid;
                        RBTN_CottonGloves.SelectedValue = "1";
                        RBTN_CottonGloves.Enabled = false;
                    }

                    BlackGoogles_rqid = dt.Rows[0]["BlackGoogles_RQID"].ToString();
                    if (BlackGoogles_rqid != "N/A")
                    {
                        lbl_BlackGoogles_rqid.Text = BlackGoogles_rqid;
                        RBTN_BlackGoogles.SelectedValue = "1";
                        RBTN_BlackGoogles.Enabled = false;
                    }

                    PVCGloves_rqid = dt.Rows[0]["PVCGloves_RQID"].ToString();
                    if (PVCGloves_rqid != "N/A")
                    {
                        lbl_PVCGloves_rqid.Text = PVCGloves_rqid;
                        RBTN_PVCGloves.SelectedValue = "1";
                        RBTN_PVCGloves.Enabled = false;
                    }

                    LthrGloves_rqid = dt.Rows[0]["LeatherGloves_RQID"].ToString();
                    if (LthrGloves_rqid != "N/A")
                    {
                        lbl_LthrGloves_rqid.Text = LthrGloves_rqid;
                        RBTN_LthrGloves.SelectedValue = "1";
                        RBTN_LthrGloves.Enabled = false;
                    }

                    LegGard_rqid = dt.Rows[0]["LegGaurd_RQID"].ToString();
                    if (LegGard_rqid != "N/A")
                    {
                        lbl_LegGard_rqid.Text = LegGard_rqid;
                        RBTN_LegGard.SelectedValue = "1";
                        RBTN_LegGard.Enabled = false;
                    }

                    HandSleves_rqid = dt.Rows[0]["HandSleevs_RQID"].ToString();
                    if (HandSleves_rqid != "N/A")
                    {
                        lbl_HandSleves_rqid.Text = HandSleves_rqid;
                        RBTN_HandSleves.SelectedValue = "1";
                        RBTN_HandSleves.Enabled = false;
                    }

                    FRJacket_rqid = dt.Rows[0]["FRJacket_RQID"].ToString();
                    if (FRJacket_rqid != "N/A")
                    {
                        lbl_FRJacket_rqid.Text = FRJacket_rqid;
                        RBTN_FRJacket.SelectedValue = "1";
                        RBTN_FRJacket.Enabled = false;
                    }

                    Apron_rqid = dt.Rows[0]["Apron_RQID"].ToString();
                    if (Apron_rqid != "N/A")
                    {
                        lbl_Apron_rqid.Text = Apron_rqid;
                        RBTN_Apron.SelectedValue = "1";
                        RBTN_Apron.Enabled = false;
                    }

                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private string FindPPRId()
        {
            string aa = null;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string kk = null;
            string cmdString1 = "select Id,PPRID from tbl_manualppe_requestlogs where Id=(select max(Id)from tbl_manualppe_requestlogs)";
            SqlCommand com1 = new SqlCommand(cmdString1, dbcl.Conn);
            SqlDataReader DR1 = com1.ExecuteReader();
            if (DR1.Read())
            {
                aa = DR1.GetValue(1).ToString();
                if (aa == null || aa == "")
                {
                    kk = "PPR001";
                }
                else
                {
                    string bb = aa.Substring(5);
                    int k = Convert.ToInt32(bb);
                    k = k + 1;
                    string q = Convert.ToString(k);
                    kk = "PPR00" + q;
                }
            }
            else
            {
                kk = "PPR001";
            }
            dbcl.Conn.Close();
            return kk;
        }

        private Boolean UploadImage(HttpPostedFile file, ref string PhotoId)
        {
            Boolean imgsaved = false;

            DateTime d = DateTime.Now;
            string month = d.Month.ToString();
            string year = d.Year.ToString();
            string day = d.Day.ToString();
            string imgdate = day + month + year;

            // Check file exist or not
            if (file != null)
            {
                // Check the extension of image
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower() == ".png" || extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg")
                {
                    Stream strm = file.InputStream;
                    using (var image = System.Drawing.Image.FromStream(strm))
                    {
                        PhotoId = FindPPRId();



                        // Print Original Size of file (Height or Width)
                        //lblprev.Text = image.Size.ToString();

                        int newWidth = 440; // New Width of Image in Pixel
                        int newHeight = 540; // New Height of Image in Pixel
                        var thumbImg = new Bitmap(newWidth, newHeight);
                        var thumbGraph = Graphics.FromImage(thumbImg);

                        thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                        thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                        thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        var imgRectangle = new Rectangle(0, 0, newWidth, newHeight);
                        thumbGraph.DrawImage(image, imgRectangle);

                        // Save the file
                        string targetPath = Server.MapPath(@"\erp_images\PPRPhoto\") + PhotoId + "_" + imgdate + "_" + empwrk + ".jpg";
                        file.SaveAs(Server.MapPath(@"\erp_images\PPRPhoto\") + PhotoId + "_" + imgdate + "_" + empwrk + ".jpg");

                        //the below will be saved as database value
                        imglink = "\\erp_images\\PPRPhoto\\" + PhotoId + "_" + imgdate + "_" + empwrk + ".jpg";
                        thumbImg.Save(targetPath, image.RawFormat);
                        imgfilename = PhotoId + "_" + imgdate + "_" + empwrk + ".jpg";

                        // Print new Size of file (height or Width)
                        //lblaftr.Text = thumbImg.Size.ToString();


                        //Show Image instantly
                        //ImgDisplay.ImageUrl = @"\erp_images\PPRPhoto\" + TBPhotoId + "_" + imgdate + ".jpg";

                        //on successfully image is saved
                        imgsaved = true;

                        //string title = "Notifications :";
                        //string body = "Photograph Uploaded Successfully";
                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Kindly Select Appropriate File Type";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
            }
            return imgsaved;
        }

        private Boolean PPE_RequestRasier(string emp_dept, string emp_deptcode, string emp_loc, string emp_loccode, string empwrk, string name, string WorkSite, string Worksite_Code, string ppeitemname, string ppewhy, string ppeitemsize, string filename, string filepath, string PPRID)
        {
            Boolean datasaved = false;
            string rqstid = FindPPRId();
            int flag = 0;
            try
            {
                dbcl.Sqlconnection();
                //SqlCommand cmd = new SqlCommand("SP_InsertInto_TBTDataTable", dbcl.Conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                string query = "INSERT into tbl_manualppe_requestlogs (Region,SubmitterWrk,SubmitterName,PPCID,PPRID,PPE_ReceiverWrk,PPE_ReceiverName,PPE_EmpWorksiteCode,PPE_EmpWorksiteName,PPE_EmpDeptCode,PPE_EmpDeptName,PPE_EmpLocCode,PPE_EmpLocName,PPE_ItemName,PPE_Why,PPE_ItemSize,PPE_Photofile,PPE_PhotoPath) VALUES (@Region,@SubmitterWrk,@SubmitterName,@PPCID,@PPRID,@PPE_ReceiverWrk,@PPE_ReceiverName,@PPE_EmpWorksiteCode,@PPE_EmpWorksiteName,@PPE_EmpDeptCode,@PPE_EmpDeptName,@PPE_EmpLocCode,@PPE_EmpLocName,@PPE_ItemName,@PPE_Why,@PPE_ItemSize,@PPE_Photofile,@PPE_PhotoPath)";
                SqlCommand cmd = new SqlCommand(query, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Region", Session["REGION"].ToString());
                cmd.Parameters.AddWithValue("@SubmitterWrk", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@SubmitterName", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@PPCID", lbl_PPCID.Text.ToString());
                cmd.Parameters.AddWithValue("@PPRID", rqstid);
                cmd.Parameters.AddWithValue("@PPE_ReceiverWrk", empwrk);
                cmd.Parameters.AddWithValue("@PPE_ReceiverName", name);
                cmd.Parameters.AddWithValue("@PPE_EmpWorksiteName", WorkSite);
                cmd.Parameters.AddWithValue("@PPE_EmpWorksiteCode", Worksite_Code);
                cmd.Parameters.AddWithValue("@PPE_EmpDeptName", emp_dept);
                cmd.Parameters.AddWithValue("@PPE_EmpDeptCode", emp_deptcode);
                cmd.Parameters.AddWithValue("@PPE_EmpLocName", emp_loc);
                cmd.Parameters.AddWithValue("@PPE_EmpLocCode", emp_loccode);
                cmd.Parameters.AddWithValue("@PPE_ItemName", ppeitemname);
                cmd.Parameters.AddWithValue("@PPE_Why", ppewhy);
                cmd.Parameters.AddWithValue("@PPE_ItemSize", ppeitemsize);
                cmd.Parameters.AddWithValue("@PPE_Photofile", filename);
                cmd.Parameters.AddWithValue("@PPE_PhotoPath", filepath);
                dbcl.ConnectDb();
                flag = cmd.ExecuteNonQuery();

                if (flag != 0)
                {
                    datasaved = true;
                    ppeitemname = null;
                    ppewhy = null;
                    ppeitemsize = null;
                    filename = "N/A";
                    filepath = "~\\images\\No_Image.jpg";
                    PPRID = null;

                    lbl_msg.Visible = true;
                    lbl_msg.Text = "Record Inserted Successfully..!";
                    lbl_msg.ForeColor = System.Drawing.Color.DarkGreen;
                    dbcl.DisconnectDb();
                }
                else
                {
                    datasaved = false;
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                datasaved = false;
            }
            return datasaved;
        }

        protected void RBTN_Helmet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBTN_Helmet.SelectedIndex == 0)
            {
                HelmetNC.Visible = false;
            }
            else
            {
                HelmetNC.Visible = true;
            }
        }

        private Boolean PPEChecklistUpdater(string mode)
        {
            Boolean datasaved = false;
            int flag = 0;
            try
            {
                string helmet_yn = RBTN_Helmet.SelectedItem.Text.ToString();
                string SftyShoes_yn = RBTN_SftyShoes.SelectedItem.Text.ToString();
                string DutyShirt_yn = RBTN_DutyShirt.SelectedItem.Text.ToString();
                string DutyPant_yn = RBTN_DutyPant.SelectedItem.Text.ToString();
                string SafetyGoogles_yn = RBTN_SafetyGoogles.SelectedItem.Text.ToString();
                string NoseMask_yn = RBTN_NoseMask.SelectedItem.Text.ToString();
                string CottonGloves_yn = RBTN_CottonGloves.SelectedItem.Text.ToString();
                string BlackGoogles_yn = RBTN_BlackGoogles.SelectedItem.Text.ToString();
                string PVCGloves_yn = RBTN_PVCGloves.SelectedItem.Text.ToString();
                string LthrGloves_yn = RBTN_LthrGloves.SelectedItem.Text.ToString();
                string LegGard_yn = RBTN_LegGard.SelectedItem.Text.ToString();
                string HandSleves_yn = RBTN_HandSleves.SelectedItem.Text.ToString();
                string FRJacket_yn = RBTN_FRJacket.SelectedItem.Text.ToString();
                string Apron_yn = RBTN_Apron.SelectedItem.Text.ToString();

                string helmet_why = DDL_HelmetWhy.SelectedItem.Text.ToString();
                string SftyShoes_why = DDL_SftyShoesWhy.SelectedItem.Text.ToString();
                string DutyShirt_why = DDL_DutyShirtWhy.SelectedItem.Text.ToString();
                string DutyPant_why = DDL_DutyPantWhy.SelectedItem.Text.ToString();
                string SafetyGoogles_why = DDL_SafetyGooglesWhy.SelectedItem.Text.ToString();
                string NoseMask_why = DDL_NoseMaskWhy.SelectedItem.Text.ToString();
                string CottonGloves_why = DDL_CottonGlovesWhy.SelectedItem.Text.ToString();
                string BlackGoogles_why = DDL_BlackGooglesWhy.SelectedItem.Text.ToString();
                string PVCGloves_why = DDL_PVCGlovesWhy.SelectedItem.Text.ToString();
                string LthrGloves_why = DDL_LthrGlovesWhy.SelectedItem.Text.ToString();
                string LegGard_why = DDL_LegGardWhy.SelectedItem.Text.ToString();
                string HandSleves_why = DDL_HandSlevesWhy.SelectedItem.Text.ToString();
                string FRJacket_why = DDL_FRJacketWhy.SelectedItem.Text.ToString();
                string Apron_why = DDL_ApronWhy.SelectedItem.Text.ToString();


                string helmet_size = DDL_HelmetSize.SelectedItem.Text.ToString();
                string SftyShoes_size = DDL_SftyShoesSize.SelectedItem.Text.ToString();
                string DutyShirt_size = DDL_DutyShirtSize.SelectedItem.Text.ToString();
                string DutyPant_size = DDL_DutyPantSize.SelectedItem.Text.ToString();
                string SafetyGoogles_size = DDL_SafetyGooglesSize.SelectedItem.Text.ToString();
                string NoseMask_size = DDL_NoseMaskSize.SelectedItem.Text.ToString();
                string CottonGloves_size = DDL_CottonGlovesSize.SelectedItem.Text.ToString();
                string BlackGoogles_size = DDL_BlackGooglesSize.SelectedItem.Text.ToString();
                string PVCGloves_size = DDL_PVCGlovesSize.SelectedItem.Text.ToString();
                string LthrGloves_size = DDL_LthrGlovesSize.SelectedItem.Text.ToString();
                string LegGard_size = DDL_LegGardSize.SelectedItem.Text.ToString();
                string HandSleves_size = DDL_HandSlevesSize.SelectedItem.Text.ToString();
                string FRJacket_size = DDL_FRJacketSize.SelectedItem.Text.ToString();
                string Apron_size = DDL_ApronSize.SelectedItem.Text.ToString();

                dbcl.Sqlconnection();
                //SqlCommand cmd = new SqlCommand("SP_InsertInto_TBTDataTable", dbcl.Conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                string query = "update tbl_manualppeaudit set Helmet=@Helmet,Helmet_RQID=@Helmet_RQID,SafetyShoes=@SafetyShoes,SafetyShoes_RQID=@SafetyShoes_RQID,DutyShirt=@DutyShirt,DutyShirt_RQID=@DutyShirt_RQID,DutyPant=@DutyPant,DutyPant_RQID=@DutyPant_RQID,SafetyGoogles=@SafetyGoogles,SafetyGoogles_RQID=@SafetyGoogles_RQID,NoseMask=@NoseMask,NoseMask_RQID=@NoseMask_RQID,CottonGloves=@CottonGloves,CottonGloves_RQID=@CottonGloves_RQID,BlackGoogles=@BlackGoogles,BlackGoogles_RQID=@BlackGoogles_RQID, PVCGloves=@PVCGloves,PVCGloves_RQID=@PVCGloves_RQID,LeatherGloves=@LeatherGloves,LeatherGloves_RQID=@LeatherGloves_RQID,LegGaurd=@LegGaurd,LegGaurd_RQID=@LegGaurd_RQID,HandSleevs=@HandSleevs,HandSleevs_RQID=@HandSleevs_RQID, FRJacket=@FRJacket, FRJacket_RQID=@FRJacket_RQID,Apron=@Apron, Apron_RQID=@Apron_RQID, DraftMode=@DraftMode, DraftTimeStamp=@DraftTimeStamp,Panel2Status=@Panel2Status, Panel2TimeStamp=@Panel2TimeStamp  where PPCID=@PPCID and EmpWrk=@EmpWrk ";
                SqlCommand cmd = new SqlCommand(query, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PPCID", lbl_PPCID.Text.ToString());
                cmd.Parameters.AddWithValue("@EmpWrk", empwrk);
                if (mode == "1")
                {
                    cmd.Parameters.AddWithValue("@DraftMode", "Yes");
                    cmd.Parameters.AddWithValue("@DraftTimeStamp", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));

                    cmd.Parameters.AddWithValue("@Panel2Status", "Pending");
                    cmd.Parameters.AddWithValue("@Panel2TimeStamp", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DraftMode", "No");
                    cmd.Parameters.AddWithValue("@DraftTimeStamp", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));

                    cmd.Parameters.AddWithValue("@Panel2Status", "Completed");
                    cmd.Parameters.AddWithValue("@Panel2TimeStamp", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                }
                cmd.Parameters.AddWithValue("@Helmet", helmet_yn);
                cmd.Parameters.AddWithValue("@Helmet_RQID", helmet_rqid);
                cmd.Parameters.AddWithValue("@SafetyShoes", SftyShoes_yn);
                cmd.Parameters.AddWithValue("@SafetyShoes_RQID", SftyShoes_rqid);
                cmd.Parameters.AddWithValue("@DutyShirt", DutyShirt_yn);
                cmd.Parameters.AddWithValue("@DutyShirt_RQID", DutyShirt_rqid);
                cmd.Parameters.AddWithValue("@DutyPant", DutyPant_yn);
                cmd.Parameters.AddWithValue("@DutyPant_RQID", DutyPant_rqid);
                cmd.Parameters.AddWithValue("@SafetyGoogles", SafetyGoogles_yn);
                cmd.Parameters.AddWithValue("@SafetyGoogles_RQID", SafetyGoogles_rqid);
                cmd.Parameters.AddWithValue("@NoseMask", NoseMask_yn);
                cmd.Parameters.AddWithValue("@NoseMask_RQID", NoseMask_rqid);
                cmd.Parameters.AddWithValue("@CottonGloves", CottonGloves_yn);
                cmd.Parameters.AddWithValue("@CottonGloves_RQID", CottonGloves_rqid);
                cmd.Parameters.AddWithValue("@BlackGoogles", BlackGoogles_yn);
                cmd.Parameters.AddWithValue("@BlackGoogles_RQID", BlackGoogles_rqid);
                cmd.Parameters.AddWithValue("@PVCGloves", PVCGloves_yn);
                cmd.Parameters.AddWithValue("@PVCGloves_RQID", PVCGloves_rqid);
                cmd.Parameters.AddWithValue("@LeatherGloves", LthrGloves_yn);
                cmd.Parameters.AddWithValue("@LeatherGloves_RQID", LthrGloves_rqid);
                cmd.Parameters.AddWithValue("@LegGaurd", LegGard_yn);
                cmd.Parameters.AddWithValue("@LegGaurd_RQID", LegGard_rqid);
                cmd.Parameters.AddWithValue("@HandSleevs", HandSleves_yn);
                cmd.Parameters.AddWithValue("@HandSleevs_RQID", HandSleves_rqid);
                cmd.Parameters.AddWithValue("@FRJacket", FRJacket_yn);
                cmd.Parameters.AddWithValue("@FRJacket_RQID", FRJacket_rqid);
                cmd.Parameters.AddWithValue("@Apron", Apron_yn);
                cmd.Parameters.AddWithValue("@Apron_RQID", Apron_rqid);
                dbcl.ConnectDb();
                flag = cmd.ExecuteNonQuery();

                if (flag != 0)
                {
                    datasaved = true;
                    lbl_msg.Visible = true;
                    lbl_msg.Text = "Record Inserted Successfully..!";
                    lbl_msg.ForeColor = System.Drawing.Color.DarkGreen;
                    dbcl.DisconnectDb();
                }
                //else
                //{
                //    datasaved = false;
                //}
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                datasaved = false;
            }
            return datasaved;
        }

        protected void btn_HelmetRequest_Click(object sender, EventArgs e)
        {

            string ppewhy = DDL_HelmetWhy.SelectedItem.Text.ToString();
            string ppeitemsize = DDL_HelmetSize.SelectedItem.Text.ToString();
            ppeitemname = "Helmet";
            string PPRID = "";
            if (UploadImage(FU_Helemt.PostedFile, ref PPRID) == true)
            {
                lbl_helmetrqid.Text = PPRID;
                helmet_rqid = lbl_helmetrqid.Text.ToString();
                if (PPE_RequestRasier(emp_dept, emp_deptcode, emp_loc, emp_loccode, empwrk, name, WorkSite, Worksite_Code, ppeitemname, ppewhy, ppeitemsize, imgfilename, imglink, PPRID) == true)
                {
                    PPEChecklistUpdater("1");
                    HelmetNC.Visible = false;
                    RBTN_Helmet.Enabled = false;
                    string title = "Notifications :";
                    string body = "Request Created Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    HelmetNC.Visible = true;
                    RBTN_Helmet.Enabled = true;
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "PPE Request Photograph NOT Uploaded";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void RBTN_SftyShoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBTN_SftyShoes.SelectedIndex == 0)
            {
                SafetyShoes_NC.Visible = false;
            }
            else
            {
                SafetyShoes_NC.Visible = true;
            }
        }

        protected void btn_sftyshoesrequest_Click(object sender, EventArgs e)
        {
            string ppewhy = DDL_SftyShoesWhy.SelectedItem.Text.ToString();
            string ppeitemsize = DDL_SftyShoesSize.SelectedItem.Text.ToString();
            ppeitemname = "Safety Shoes";
            string PPRID = "";
            if (UploadImage(FU_SftyShoes.PostedFile, ref PPRID) == true)
            {
                lbl_SftyShoes_rqid.Text = PPRID;
                SftyShoes_rqid = lbl_SftyShoes_rqid.Text.ToString();
                if (PPE_RequestRasier(emp_dept, emp_deptcode, emp_loc, emp_loccode, empwrk, name, WorkSite, Worksite_Code, ppeitemname, ppewhy, ppeitemsize, imgfilename, imglink, PPRID) == true)
                {
                    PPEChecklistUpdater("1");
                    SafetyShoes_NC.Visible = false;
                    RBTN_SftyShoes.Enabled = false;

                    string title = "Notifications :";
                    string body = "Request Created Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Request cannot be Created";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    SafetyShoes_NC.Visible = true;
                    RBTN_SftyShoes.Enabled = true;
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "PPE Request Photograph NOT Uploaded";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void RBTN_DutyShirt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBTN_DutyShirt.SelectedIndex == 0)
            {
                DutyShirt_NC.Visible = false;
            }
            else
            {
                DutyShirt_NC.Visible = true;
            }
        }

        protected void btn_DutyShirtRequest_Click(object sender, EventArgs e)
        {
            string ppewhy = DDL_DutyShirtWhy.SelectedItem.Text.ToString();
            string ppeitemsize = DDL_DutyShirtSize.SelectedItem.Text.ToString();
            ppeitemname = "Duty Shirt";
            string PPRID = "";
            if (UploadImage(FU_DutyShirt.PostedFile, ref PPRID) == true)
            {
                lbl_DutyShirt_rqid.Text = PPRID;
                DutyShirt_rqid = lbl_DutyShirt_rqid.Text.ToString();
                if (PPE_RequestRasier(emp_dept, emp_deptcode, emp_loc, emp_loccode, empwrk, name, WorkSite, Worksite_Code, ppeitemname, ppewhy, ppeitemsize, imgfilename, imglink, PPRID) == true)
                {
                    PPEChecklistUpdater("1");
                    DutyShirt_NC.Visible = false;
                    RBTN_DutyShirt.Enabled = false;
                    string title = "Notifications :";
                    string body = "Request Created Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Request cannot be Created";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    DutyShirt_NC.Visible = true;
                    RBTN_DutyShirt.Enabled = true;
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "PPE Request Photograph NOT Uploaded";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void RBTN_DutyPant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBTN_DutyPant.SelectedIndex == 0)
            {
                DutyPant_NC.Visible = false;
            }
            else
            {
                DutyPant_NC.Visible = true;
            }
        }

        protected void btn_DutyPantRequest_Click(object sender, EventArgs e)
        {

            string ppewhy = DDL_DutyPantWhy.SelectedItem.Text.ToString();
            string ppeitemsize = DDL_DutyPantSize.SelectedItem.Text.ToString();
            ppeitemname = "Duty Pant";
            string PPRID = "";
            if (UploadImage(FU_DutyPant.PostedFile, ref PPRID) == true)
            {
                lbl_DutyPant_rqid.Text = PPRID;
                DutyPant_rqid = lbl_DutyPant_rqid.Text.ToString();

                if (PPE_RequestRasier(emp_dept, emp_deptcode, emp_loc, emp_loccode, empwrk, name, WorkSite, Worksite_Code, ppeitemname, ppewhy, ppeitemsize, imgfilename, imglink, PPRID) == true)
                {
                    PPEChecklistUpdater("1");
                    DutyPant_NC.Visible = false;
                    RBTN_DutyPant.Enabled = false;
                    string title = "Notifications :";
                    string body = "Request Created Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Request cannot be Created";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    DutyPant_NC.Visible = true;
                    RBTN_DutyPant.Enabled = true;
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "PPE Request Photograph NOT Uploaded";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }



        protected void RBTN_SafetyGoogles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBTN_SafetyGoogles.SelectedIndex == 0)
            {
                SafetyGoogles_NC.Visible = false;
            }
            else
            {
                SafetyGoogles_NC.Visible = true;
            }
        }
        protected void btn_SafetyGooglesRequest_Click(object sender, EventArgs e)
        {
            string ppewhy = DDL_SafetyGooglesWhy.SelectedItem.Text.ToString();
            string ppeitemsize = DDL_SafetyGooglesSize.SelectedItem.Text.ToString();
            ppeitemname = "Safety Googles";
            string PPRID = "";
            if (UploadImage(FU_SafetyGoogles.PostedFile, ref PPRID) == true)
            {
                lbl_SafetyGoogles_rqid.Text = PPRID;
                SafetyGoogles_rqid = lbl_SafetyGoogles_rqid.Text.ToString();

                if (PPE_RequestRasier(emp_dept, emp_deptcode, emp_loc, emp_loccode, empwrk, name, WorkSite, Worksite_Code, ppeitemname, ppewhy, ppeitemsize, imgfilename, imglink, PPRID) == true)
                {
                    PPEChecklistUpdater("1");
                    SafetyGoogles_NC.Visible = false;
                    RBTN_SafetyGoogles.Enabled = false;
                    string title = "Notifications :";
                    string body = "Request Created Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Request cannot be Created";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    SafetyGoogles_NC.Visible = true;
                    RBTN_SafetyGoogles.Enabled = true;
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "PPE Request Photograph NOT Uploaded";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }



        protected void RBTN_NoseMask_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBTN_NoseMask.SelectedIndex == 0)
            {
                NoseMask_NC.Visible = false;
            }
            else
            {
                NoseMask_NC.Visible = true;
            }
        }

        protected void btn_NoseMaskRequest_Click(object sender, EventArgs e)
        {
            string ppewhy = DDL_NoseMaskWhy.SelectedItem.Text.ToString();
            string ppeitemsize = DDL_NoseMaskSize.SelectedItem.Text.ToString();
            ppeitemname = "Nose Mask";
            string PPRID = "";
            if (UploadImage(FU_NoseMask.PostedFile, ref PPRID) == true)
            {
                lbl_NoseMask_rqid.Text = PPRID;
                NoseMask_rqid = lbl_NoseMask_rqid.Text.ToString();

                if (PPE_RequestRasier(emp_dept, emp_deptcode, emp_loc, emp_loccode, empwrk, name, WorkSite, Worksite_Code, ppeitemname, ppewhy, ppeitemsize, imgfilename, imglink, PPRID) == true)
                {
                    PPEChecklistUpdater("1");
                    NoseMask_NC.Visible = false;
                    RBTN_NoseMask.Enabled = false;
                    string title = "Notifications :";
                    string body = "Request Created Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Request cannot be Created";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    NoseMask_NC.Visible = true;
                    RBTN_NoseMask.Enabled = true;
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "PPE Request Photograph NOT Uploaded";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void RBTN_CottonGloves_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBTN_CottonGloves.SelectedIndex == 0)
            {
                CottonGloves_NC.Visible = false;
            }
            else
            {
                CottonGloves_NC.Visible = true;
            }
        }

        protected void btn_CottonGlovesRqst_Click(object sender, EventArgs e)
        {
            string ppewhy = DDL_CottonGlovesWhy.SelectedItem.Text.ToString();
            string ppeitemsize = DDL_CottonGlovesSize.SelectedItem.Text.ToString();
            ppeitemname = "Cotton Gloves";
            string PPRID = "";
            if (UploadImage(FU_CottonGlovesSize.PostedFile, ref PPRID) == true)
            {
                lbl_CottonGloves_rqid.Text = PPRID;
                CottonGloves_rqid = lbl_CottonGloves_rqid.Text.ToString();

                if (PPE_RequestRasier(emp_dept, emp_deptcode, emp_loc, emp_loccode, empwrk, name, WorkSite, Worksite_Code, ppeitemname, ppewhy, ppeitemsize, imgfilename, imglink, PPRID) == true)
                {
                    PPEChecklistUpdater("1");
                    CottonGloves_NC.Visible = false;
                    RBTN_CottonGloves.Enabled = false;
                    string title = "Notifications :";
                    string body = "Request Created Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Request cannot be Created";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    CottonGloves_NC.Visible = false;
                    RBTN_CottonGloves.Enabled = false;
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "PPE Request Photograph NOT Uploaded";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }


        protected void RBTN_BlackGoogles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBTN_BlackGoogles.SelectedIndex == 0)
            {
                BlackGoogles_NC.Visible = false;
            }
            else
            {
                BlackGoogles_NC.Visible = true;
            }
        }

        protected void btn_BlackGoogles_Click(object sender, EventArgs e)
        {
            string ppewhy = DDL_BlackGooglesWhy.SelectedItem.Text.ToString();
            string ppeitemsize = DDL_BlackGooglesSize.SelectedItem.Text.ToString();
            ppeitemname = "Black Googles";
            string PPRID = "";
            if (UploadImage(FU_BlackGoogles.PostedFile, ref PPRID) == true)
            {
                lbl_BlackGoogles_rqid.Text = PPRID;
                BlackGoogles_rqid = lbl_BlackGoogles_rqid.Text.ToString();

                if (PPE_RequestRasier(emp_dept, emp_deptcode, emp_loc, emp_loccode, empwrk, name, WorkSite, Worksite_Code, ppeitemname, ppewhy, ppeitemsize, imgfilename, imglink, PPRID) == true)
                {
                    PPEChecklistUpdater("1");
                    BlackGoogles_NC.Visible = false;
                    RBTN_BlackGoogles.Enabled = false;
                    string title = "Notifications :";
                    string body = "Request Created Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Request cannot be Created";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    BlackGoogles_NC.Visible = true;
                    RBTN_BlackGoogles.Enabled = true;
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "PPE Request Photograph NOT Uploaded";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void RBTN_PVCGloves_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBTN_PVCGloves.SelectedIndex == 0)
            {
                PVCGloves_NC.Visible = false;
            }
            else
            {
                PVCGloves_NC.Visible = true;
            }
        }

        protected void btn_PVCGlovesRequest_Click(object sender, EventArgs e)
        {
            string ppewhy = DDL_PVCGlovesWhy.SelectedItem.Text.ToString();
            string ppeitemsize = DDL_PVCGlovesSize.SelectedItem.Text.ToString();
            ppeitemname = "PVC Googles";
            string PPRID = "";
            if (UploadImage(FU_PVCGloves.PostedFile, ref PPRID) == true)
            {
                lbl_PVCGloves_rqid.Text = PPRID;
                PVCGloves_rqid = lbl_PVCGloves_rqid.Text.ToString();

                if (PPE_RequestRasier(emp_dept, emp_deptcode, emp_loc, emp_loccode, empwrk, name, WorkSite, Worksite_Code, ppeitemname, ppewhy, ppeitemsize, imgfilename, imglink, PPRID) == true)
                {
                    PPEChecklistUpdater("1");
                    PVCGloves_NC.Visible = false;
                    RBTN_PVCGloves.Enabled = false;
                    string title = "Notifications :";
                    string body = "Request Created Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Request cannot be Created";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    PVCGloves_NC.Visible = true;
                    RBTN_PVCGloves.Enabled = true;
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "PPE Request Photograph NOT Uploaded";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void RBTN_LthrGloves_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBTN_LthrGloves.SelectedIndex == 0)
            {
                LthrGloves_NC.Visible = false;
            }
            else
            {
                LthrGloves_NC.Visible = true;
            }
        }

        protected void btn_LthrGlovesRequest_Click(object sender, EventArgs e)
        {
            string ppewhy = DDL_LthrGlovesWhy.SelectedItem.Text.ToString();
            string ppeitemsize = DDL_LthrGlovesSize.SelectedItem.Text.ToString();
            ppeitemname = "Leather Gloves";
            string PPRID = "";
            if (UploadImage(FU_LthrGloves.PostedFile, ref PPRID) == true)
            {
                lbl_LthrGloves_rqid.Text = PPRID;
                LthrGloves_rqid = lbl_LthrGloves_rqid.Text.ToString();

                if (PPE_RequestRasier(emp_dept, emp_deptcode, emp_loc, emp_loccode, empwrk, name, WorkSite, Worksite_Code, ppeitemname, ppewhy, ppeitemsize, imgfilename, imglink, PPRID) == true)
                {
                    PPEChecklistUpdater("1");
                    LthrGloves_NC.Visible = false;
                    RBTN_LthrGloves.Enabled = false;
                    string title = "Notifications :";
                    string body = "Request Created Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Request cannot be Created";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    LthrGloves_NC.Visible = true;
                    RBTN_LthrGloves.Enabled = true;
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "PPE Request Photograph NOT Uploaded";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void RBTN_LegGard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBTN_LegGard.SelectedIndex == 0)
            {
                LegGard_NC.Visible = false;
            }
            else
            {
                LegGard_NC.Visible = true;
            }
        }

        protected void btn_LegGardRequest_Click(object sender, EventArgs e)
        {
            string ppewhy = DDL_LegGardWhy.SelectedItem.Text.ToString();
            string ppeitemsize = DDL_LegGardSize.SelectedItem.Text.ToString();
            ppeitemname = "Leag Gaurd";
            string PPRID = "";
            if (UploadImage(FU_LegGard.PostedFile, ref PPRID) == true)
            {
                lbl_LegGard_rqid.Text = PPRID;
                LegGard_rqid = lbl_LegGard_rqid.Text.ToString();

                if (PPE_RequestRasier(emp_dept, emp_deptcode, emp_loc, emp_loccode, empwrk, name, WorkSite, Worksite_Code, ppeitemname, ppewhy, ppeitemsize, imgfilename, imglink, PPRID) == true)
                {
                    PPEChecklistUpdater("1");
                    LegGard_NC.Visible = false;
                    RBTN_LegGard.Enabled = false;
                    string title = "Notifications :";
                    string body = "Request Created Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Request cannot be Created";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    LegGard_NC.Visible = true;
                    RBTN_LegGard.Enabled = true;
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "PPE Request Photograph NOT Uploaded";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }


        }

        protected void RBTN_HandSleves_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBTN_HandSleves.SelectedIndex == 0)
            {
                HandSleves_NC.Visible = false;
            }
            else
            {
                HandSleves_NC.Visible = true;
            }
        }

        protected void btn_HandSlevesRequest_Click(object sender, EventArgs e)
        {

            string ppewhy = DDL_HandSlevesWhy.SelectedItem.Text.ToString();
            string ppeitemsize = DDL_HandSlevesSize.SelectedItem.Text.ToString();
            ppeitemname = "Hand Sleeves";
            string PPRID = "";
            if (UploadImage(FU_HandSleves.PostedFile, ref PPRID) == true)
            {
                lbl_HandSleves_rqid.Text = PPRID;
                HandSleves_rqid = lbl_HandSleves_rqid.Text.ToString();

                if (PPE_RequestRasier(emp_dept, emp_deptcode, emp_loc, emp_loccode, empwrk, name, WorkSite, Worksite_Code, ppeitemname, ppewhy, ppeitemsize, imgfilename, imglink, PPRID) == true)
                {
                    PPEChecklistUpdater("1");
                    HandSleves_NC.Visible = false;
                    RBTN_HandSleves.Enabled = false;
                    string title = "Notifications :";
                    string body = "Request Created Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Request cannot be Created";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    HandSleves_NC.Visible = true;
                    RBTN_HandSleves.Enabled = true;
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "PPE Request Photograph NOT Uploaded";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }


        }

        protected void RBTN_FRJacket_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBTN_FRJacket.SelectedIndex == 0)
            {
                FRJacket_NC.Visible = false;
            }
            else
            {
                FRJacket_NC.Visible = true;
            }
        }

        protected void btn_FRJacketRequest_Click(object sender, EventArgs e)
        {
            string ppewhy = DDL_FRJacketWhy.SelectedItem.Text.ToString();
            string ppeitemsize = DDL_FRJacketSize.SelectedItem.Text.ToString();
            ppeitemname = "FR Jacket";
            string PPRID = "";
            if (UploadImage(FU_FRJacket.PostedFile, ref PPRID) == true)
            {
                lbl_FRJacket_rqid.Text = PPRID;
                FRJacket_rqid = lbl_FRJacket_rqid.Text.ToString();

                if (PPE_RequestRasier(emp_dept, emp_deptcode, emp_loc, emp_loccode, empwrk, name, WorkSite, Worksite_Code, ppeitemname, ppewhy, ppeitemsize, imgfilename, imglink, PPRID) == true)
                {
                    PPEChecklistUpdater("1");
                    FRJacket_NC.Visible = false;
                    RBTN_FRJacket.Enabled = false;
                    string title = "Notifications :";
                    string body = "Request Created Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Request cannot be Created";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    FRJacket_NC.Visible = false;
                    RBTN_FRJacket.Enabled = false;
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "PPE Request Photograph NOT Uploaded";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void RBTN_Apron_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBTN_Apron.SelectedIndex == 0)
            {
                Apron_NC.Visible = false;
            }
            else
            {
                Apron_NC.Visible = true;
            }
        }

        protected void btn_ApronRequest_Click(object sender, EventArgs e)
        {
            string ppewhy = DDL_ApronWhy.SelectedItem.Text.ToString();
            string ppeitemsize = DDL_ApronSize.SelectedItem.Text.ToString();
            ppeitemname = "Apron";
            string PPRID = "";
            if (UploadImage(FU_Apron.PostedFile, ref PPRID) == true)
            {
                lbl_Apron_rqid.Text = PPRID;
                Apron_rqid = lbl_Apron_rqid.Text.ToString();

                if (PPE_RequestRasier(emp_dept, emp_deptcode, emp_loc, emp_loccode, empwrk, name, WorkSite, Worksite_Code, ppeitemname, ppewhy, ppeitemsize, imgfilename, imglink, PPRID) == true)
                {
                    PPEChecklistUpdater("1");
                    Apron_NC.Visible = false;
                    RBTN_Apron.Enabled = false;
                    string title = "Notifications :";
                    string body = "Request Created Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    string title = "Notifications :";
                    string body = "Request cannot be Created";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    Apron_NC.Visible = true;
                    RBTN_Apron.Enabled = true;
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "PPE Request Photograph NOT Uploaded";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private string FindPPCId()
        {
            string aa = null;
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string kk = null;
            string cmdString1 = "select Id,PPCID from tbl_manualppeaudit where Id=(select max(Id)from tbl_manualppeaudit)";
            SqlCommand com1 = new SqlCommand(cmdString1, dbcl.Conn);
            SqlDataReader DR1 = com1.ExecuteReader();
            if (DR1.Read())
            {
                aa = DR1.GetValue(1).ToString();
                if (aa == null || aa == "")
                {
                    kk = "PPC001";
                }
                else
                {
                    string bb = aa.Substring(5);
                    int k = Convert.ToInt32(bb);
                    k = k + 1;
                    string q = Convert.ToString(k);
                    kk = "PPC00" + q;
                }
            }
            else
            {
                kk = "PPC001";
            }
            dbcl.Conn.Close();
            return kk;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (DDL_Dept.SelectedIndex != 0 && DDL_Location.SelectedIndex != 0 && txt_empworkman.Text != "")
            {
                emp_dept = DDL_Dept.SelectedItem.Text.ToString();
                emp_deptcode = DDL_Dept.SelectedValue.ToString();

                emp_loc = DDL_Location.SelectedItem.Text.ToString();
                emp_loccode = DDL_Location.SelectedValue.ToString();

                empwrk = txt_empworkman.Text.ToString();
                FindName(empwrk, ref name, ref WorkSite, ref Worksite_Code);

                submittername = Session["USERNAME"].ToString();
                submitterwrk = Session["WORKMAN"].ToString();

                submitterrgn = Session["REGION"].ToString();

                if (Panel1Data(submitterwrk, submittername, submitterrgn, emp_dept, emp_deptcode, emp_loc, emp_loccode, empwrk, name, WorkSite, Worksite_Code) == true)
                {
                    ID_CreatedMsg.Visible = true; id_creation.Visible = false; CreateIDRow.Visible = false;
                    Panel2Buttons.Visible = true;

                    TableBox.Visible = true;
                    string title = "Notifications :";
                    string body = "ID created :" +lbl_PPCID.Text.ToString()+ "...!";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    TableBox.Visible = false;
                    string title = "Notifications :";
                    string body = "ID Cannot be created";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
            }
        }

        public void FindName(string workman, ref string name, ref string WorkSite, ref string Worksite_Code)
        {
            string cmdString = "select FullName,WorkSite,Worksite_Code, SkillDesignation,SkillDesignationDB from tbl_Employee_Mustertable where WorkmanSL='" + workman.ToString() + "'";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            if (Rdr.Read())
            {
                name = Rdr["FullName"].ToString();
                WorkSite = Rdr["WorkSite"].ToString();
                Worksite_Code = Rdr["Worksite_Code"].ToString();
                desg = Rdr["SkillDesignation"].ToString();
                desg_Code = Rdr["SkillDesignationDB"].ToString();
            }
            dbcl.Conn.Close();
        }

        private Boolean Panel1Data(string submitterwrk, string submittername, string submitterrgn, string emp_dept, string emp_deptcode, string emp_loc, string emp_loccode, string empwrk, string name, string WorkSite, string Worksite_Code)
        {
            Boolean datasaved = false;
            incharge = DDL_Approver.SelectedItem.Text.ToString();
            incharge_Code = DDL_Approver.SelectedValue.ToString();
            int flag = 0;
            try
            {
                string id = FindPPCId();
                lbl_PPCID.Text = id;
                dbcl.Sqlconnection();
                //SqlCommand cmd = new SqlCommand("SP_InsertInto_TBTDataTable", dbcl.Conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                string query = "INSERT into tbl_manualppeaudit (SubmitterName,SubmitterWrk,Region,PPCID,EmpWrk,EmpName,EmpDesg,EmpDesgCode,EmpWorksiteName,EmpWorksiteCode, SiteInchargeName, SiteInchargeWrk, SiteIncharge_App, EmpDeptName, EmpDeptCode,EmpLocName, EmpLocCode, Panel1Status,DraftMode, Helmet,SafetyShoes,DutyShirt,DutyPant,SafetyGoogles,NoseMask,CottonGloves,BlackGoogles,PVCGloves,LeatherGloves,LegGaurd,HandSleevs,FRJacket,Apron, Panel2Status, SO_Name, SO_Wrk) VALUES (@SubmitterName,@SubmitterWrk,@Region,@PPCID,@EmpWrk,@EmpName,@EmpDesg,@EmpDesgCode,@EmpWorksiteName,@EmpWorksiteCode, @SiteInchargeName, @SiteInchargeWrk, @SiteIncharge_App, @EmpDeptName, @EmpDeptCode, @EmpLocName, @EmpLocCode,@Panel1Status,@DraftMode, @Helmet,@SafetyShoes,@DutyShirt,@DutyPant,@SafetyGoogles,@NoseMask,@CottonGloves,@BlackGoogles,@PVCGloves,@LeatherGloves,@LegGaurd,@HandSleevs,@FRJacket,@Apron,@Panel2Status, @SO_Name, @SO_Wrk)";
                SqlCommand cmd = new SqlCommand(query, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@SubmitterName", submittername);
                cmd.Parameters.AddWithValue("@SubmitterWrk", submitterwrk);
                cmd.Parameters.AddWithValue("@Region", submitterrgn);
                cmd.Parameters.AddWithValue("@PPCID", id);
                cmd.Parameters.AddWithValue("@EmpWrk", empwrk);
                cmd.Parameters.AddWithValue("@EmpName", name);
                cmd.Parameters.AddWithValue("@EmpDesg", desg);
                cmd.Parameters.AddWithValue("@EmpDesgCode", desg_Code);
                cmd.Parameters.AddWithValue("@EmpWorksiteName", WorkSite);
                cmd.Parameters.AddWithValue("@EmpWorksiteCode", Worksite_Code);

                cmd.Parameters.AddWithValue("@SiteInchargeName", incharge);
                cmd.Parameters.AddWithValue("@SiteInchargeWrk", incharge_Code);
                cmd.Parameters.AddWithValue("@SiteIncharge_App", "Pending");

                cmd.Parameters.AddWithValue("@EmpDeptName", emp_dept);
                cmd.Parameters.AddWithValue("@EmpDeptCode", emp_deptcode);
                cmd.Parameters.AddWithValue("@EmpLocName", emp_loc);
                cmd.Parameters.AddWithValue("@EmpLocCode", emp_loccode);
                cmd.Parameters.AddWithValue("@Panel1Status", "Completed");
                cmd.Parameters.AddWithValue("@DraftMode", "Yes");
                cmd.Parameters.AddWithValue("@Helmet", "Ok");
                cmd.Parameters.AddWithValue("@SafetyShoes", "Ok");
                cmd.Parameters.AddWithValue("@DutyShirt", "Ok");
                cmd.Parameters.AddWithValue("@DutyPant", "Ok");
                cmd.Parameters.AddWithValue("@SafetyGoogles", "Ok");
                cmd.Parameters.AddWithValue("@NoseMask", "Ok");
                cmd.Parameters.AddWithValue("@CottonGloves", "Ok");
                cmd.Parameters.AddWithValue("@BlackGoogles", "Ok");
                cmd.Parameters.AddWithValue("@PVCGloves", "Ok");
                cmd.Parameters.AddWithValue("@LeatherGloves", "Ok");
                cmd.Parameters.AddWithValue("@LegGaurd", "Ok");
                cmd.Parameters.AddWithValue("@HandSleevs", "Ok");
                cmd.Parameters.AddWithValue("@FRJacket", "Ok");
                cmd.Parameters.AddWithValue("@Apron", "Ok");
                cmd.Parameters.AddWithValue("@Panel2Status", "Pending");
                cmd.Parameters.AddWithValue("@SO_Name", DDL_SftyOfcr.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@SO_Wrk", DDL_SftyOfcr.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@SO_Approval", "Pending");
                cmd.Parameters.AddWithValue("@SO_Remarks", "N/A");

                dbcl.ConnectDb();
                flag = cmd.ExecuteNonQuery();

                if (flag != 0)
                {
                    datasaved = true;
                    lbl_msg.Visible = true;
                    lbl_msg.Text = "Record Inserted Successfully..!";
                    lbl_msg.ForeColor = System.Drawing.Color.DarkGreen;
                    dbcl.DisconnectDb();
                }
                else
                {
                    datasaved = false;
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                datasaved = false;
            }

            //datasaved = true;

            return datasaved;
        }

        protected void btn_draft_Click(object sender, EventArgs e)
        {
            Panel2Message.Visible = true;
            Panel2Buttons.Visible = false;

            lbl_panel2msg.Text = "ID Created & Saved in Draft Mode";

            try
            {
                PPEChecklistUpdater("1");
                string title = "Notifications :";
                string body = "Saved in draft Mode";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Saved in draft Mode";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void btn_finalsbmt_Click(object sender, EventArgs e)
        {

            if (PPEChecklistUpdater("2") == true)
            {
                string title = "Notifications :";
                string body = "Data Saved Successfully";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                Panel2Message.Visible = true;
                Panel2Buttons.Visible = false;

                lbl_panel2msg.Text = "Successfully Submitted";
            }
            else
            {
                string title = "Notifications :";
                string body = "Not Successfull";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        public void Bind_SafetyOfficer(DropDownList cmbName, string CmdString)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand Cmd = new SqlCommand(CmdString, dbcl.Conn);
            Cmd.CommandType = CommandType.Text;
            DDL_SftyOfcr.DataSource = Cmd.ExecuteReader();
            DDL_SftyOfcr.DataTextField = "FullName";
            DDL_SftyOfcr.DataValueField = "WorkmanSL";
            DDL_SftyOfcr.DataBind();
            DDL_SftyOfcr.Items.Insert(0, "Please Select Option");
            dbcl.DisconnectDb();
        }
    }
}