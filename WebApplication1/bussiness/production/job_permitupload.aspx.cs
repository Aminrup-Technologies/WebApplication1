using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.IO;
using System.Configuration;

namespace WebApplication1.bussiness.production
{
    public partial class job_permitupload : System.Web.UI.Page
    {
        // Default folder
        static readonly string rootFolder = @"C:\atswork.in\wwwroot\erp_images\Permits";

        //static readonly string rootFolder = @"D:\OH4Y Works\OH4Y_2021\Demo\WebApplication1\WebApplication1\erp_images\Permits";

        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();
        DataTable dt = new DataTable();

        //static string Server_FileName = String.Empty;
        //static string Server_FilePath = String.Empty;
        //static string FileType = String.Empty;
        //static string ext = string.Empty;
        //static Byte[] bytes = { 0 };

        Boolean FileFlag = false;

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
                    GridTable_Row.Visible = false;
                    ActiveJOB_Checker();
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

        private void ActiveJOB_Checker()
        {
            Int32 Activejobcount = CC.Find_PendingPermitUpload(Session["WORKMAN"].ToString());
            Int32 PendingUploadjobcount = CC.Find_PendingPermitUploadStatus(Session["WORKMAN"].ToString());

            if (Activejobcount > 0)
            {
                dbcl.FillCombo(DDL_JOBID, "select JOBID from tbl_jobs where Creator_Workman='" + Session["WORKMAN"].ToString() + "' and JOBID_Status='Active' and EntryExit='Entry' order by CreatedDate desc ");
            }
            else if (PendingUploadjobcount > 0)
            {
                dbcl.FillCombo(DDL_JOBID, "select JOBID from tbl_jobs where Creator_Workman='" + Session["WORKMAN"].ToString() + "' and JOBID_Status='Active' and FinalUpldStatus='No' order by CreatedDate desc ");
            }
            else
            {
                string title = "Notifications :";
                string body = "NO Active JOB ID Found...! Kindly create a JOB ID and proceed.";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void DDL_JOBID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_JOBID.SelectedIndex == 0)
            {
                PrmtStatus_Row1.Visible = false;
                PrmtStatus_Row2.Visible = false;

                AppStatus_Row1.Visible = false;
                AppStatus_Row2.Visible = false;

                FileCount_Row1.Visible = false;
                FileCount_Row2.Visible = false;

                PrmtUpldDate1.Visible = false;
                PrmtUpldDate2.Visible = false;
            }
            else
            {
                string ddljobid = DDL_JOBID.SelectedItem.Text.ToString();
                Bind_JOBIDDetails(ddljobid);

                string CmdString2 = "select * from tbl_jobspermit where JOBID='" + ddljobid + "' order by Id desc";
                BindGrid(CmdString2);
                GridTable_Row.Visible = true;

                PrmtStatus_Row1.Visible = true;
                PrmtStatus_Row2.Visible = true;

                AppStatus_Row1.Visible = true;
                AppStatus_Row2.Visible = true;

                jobid_details_row1.Visible = true;
                jobid_details_row2.Visible = true;

                FileCount_Row1.Visible = true;
                FileCount_Row2.Visible = true;

                PrmtUpldDate1.Visible = true;
                PrmtUpldDate2.Visible = true;
            }
        }

        private void Bind_JOBIDDetails(string jobid)
        {
            try
            {
                string CmdString2 = "select * from tbl_jobspermit where JOBID='" + jobid + "' order by Id desc";
                BindGrid(CmdString2);
                GridTable_Row.Visible = true;

                string query = "select * from tbl_jobs where JOBID=@JOBID";
                SqlParameter[] pram = {
                                          new SqlParameter("@JOBID",jobid),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    lbl_jobiddate.Text = dt.Rows[0]["CreatedDate"].ToString();
                    lbl_jobcreatorname.Text = dt.Rows[0]["Creator_Name"].ToString();
                    lbl_creatorwrk.Text = dt.Rows[0]["Creator_Workman"].ToString();
                    lbl_creatorregion.Text = dt.Rows[0]["Creator_Region"].ToString();
                    lbl_creatorcompany.Text = dt.Rows[0]["Creator_Company"].ToString();
                    lbl_crtrsitename.Text = dt.Rows[0]["Creator_Site"].ToString();
                    lbl_crtrsitecode.Text = dt.Rows[0]["Creator_SiteCode"].ToString();
                    lbl_wrkordr.Text = dt.Rows[0]["WorkOrderNo"].ToString();
                    lbl_jobid.Text = dt.Rows[0]["JOBID"].ToString();
                    lbl_jobrgn.Text = dt.Rows[0]["JOB_Region"].ToString();
                    lbl_jobcompay.Text = dt.Rows[0]["JOB_Company"].ToString();
                    lbl_jobsite.Text = dt.Rows[0]["JOB_Site"].ToString();
                    lbl_jobsitecode.Text = dt.Rows[0]["JOB_SiteCode"].ToString();
                    lbl_inchargewrk.Text = dt.Rows[0]["JOB_InchargeWrk"].ToString();
                    lbl_inchargename.Text = dt.Rows[0]["JOB_InchargeName"].ToString();
                    lbl_dept.Text = dt.Rows[0]["JOB_Dept"].ToString();
                    lbl_jobloc.Text = dt.Rows[0]["JOB_Location"].ToString();
                    lbl_jobshift.Text = dt.Rows[0]["JOB_Shift"].ToString();
                    lbl_permitno.Text = dt.Rows[0]["JOB_PermitNo"].ToString();

                    lbl_permituploaddate.Text = dt.Rows[0]["PermitUploadDate"].ToString();

                    lbl_permitdeleteddate.Text = dt.Rows[0]["PermitDeleteDate"].ToString();
                    lbl_permitdeletedby.Text = dt.Rows[0]["PermitDeletedByName"].ToString();

                    string uploadstatus = dt.Rows[0]["FinalUpldStatus"].ToString();
                    string permitstatus = dt.Rows[0]["PermitUpload"].ToString();
                    string uploadtype = dt.Rows[0]["UploadType"].ToString();
                    string uploadcount = dt.Rows[0]["FileCount"].ToString();
                    lbl_filecount.Text = uploadcount;

                    if (uploadstatus == "Yes")
                    {
                        lbl_permitstatus.Text = "Uploaded";
                        lbl_permitstatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        if (permitstatus == "Yes")
                        {
                            if (uploadtype == "PDF File")
                            {
                                if (Convert.ToInt32(uploadcount.ToString()) >= 1)
                                {
                                    lbl_permitstatus.Text = "Uploaded";
                                    lbl_permitstatus.ForeColor = Color.Green;
                                }
                                else
                                {
                                    lbl_permitstatus.Text = "Pending";
                                    lbl_permitstatus.ForeColor = Color.Red;
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(uploadcount.ToString()) >= 2)
                                {
                                    lbl_permitstatus.Text = "Uploaded";
                                    lbl_permitstatus.ForeColor = Color.Green;
                                }
                                else
                                {
                                    lbl_permitstatus.Text = "One Pending";
                                    lbl_permitstatus.ForeColor = Color.Green;
                                    btn_uploadbtn.Text = "ADD MORE";
                                    DDL_UploadType.SelectedIndex = 2;
                                }
                            }
                        }
                        else
                        {
                            lbl_permitstatus.Text = "Pending";
                            lbl_permitstatus.ForeColor = Color.Red;
                        }
                    }


                    string approvalstatus = dt.Rows[0]["Incharge_Approval"].ToString();
                    if (approvalstatus == "Approved")
                    {
                        lbl_approvalstatus.Text = "Approved";
                        lbl_approvalstatus.ForeColor = Color.Green;

                    }
                    else
                    {
                        lbl_approvalstatus.Text = "Pending";
                        lbl_approvalstatus.ForeColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Error: " + ex.Message.ToString();
            }
        }

        protected void DDL_UploadType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_UploadType.SelectedIndex == 0)
            {
                UpldTyp_Row1.Visible = false;
                UpldTyp_Row2.Visible = false;

                pdfuploadbuttonrow1.Visible = false;
                pdfuploadbuttonrow2.Visible = false;
            }
            else
            {
                UpldTyp_Row1.Visible = true;
                UpldTyp_Row2.Visible = true;

                pdfuploadbuttonrow1.Visible = true;
                pdfuploadbuttonrow2.Visible = true;

                //string DDLS = DDL_UploadType.SelectedItem.Text.ToString();
                //if (DDLS == "PDF File")
                //{
                //    pdfuploadbuttonrow1.Visible = true;
                //    pdfuploadbuttonrow2.Visible = true;
                //}
                //else
                //{
                //    pdfuploadbuttonrow1.Visible = true;
                //    pdfuploadbuttonrow2.Visible = true;
                //}
            }
        }

        protected void btn_uploadbtn_Click(object sender, EventArgs e)
        {
            if (btn_uploadbtn.Text == "ADD MORE")
            {
                DDL_UploadType.SelectedIndex = 2;
                pdfuploadbuttonrow1.Visible = true;
                pdfuploadbuttonrow2.Visible = true;
            }
            else
            {
                DDL_UploadType.SelectedIndex = 0;
            }
            lblMessage.Text = "";
            UpldTyp_Row1.Visible = true;
            UpldTyp_Row2.Visible = true;
            cancelbtn_row1.Visible = true;
            cancelbtn_row2.Visible = true;
        }

        protected void btn_cnclupld_Click(object sender, EventArgs e)
        {
            UpldTyp_Row1.Visible = false;
            UpldTyp_Row2.Visible = false;
            cancelbtn_row1.Visible = false;
            cancelbtn_row2.Visible = false;

            pdfuploadbuttonrow1.Visible = false;
            pdfuploadbuttonrow2.Visible = false;

            DDL_UploadType.SelectedIndex = 0;
        }

        protected void ImportPermit(object sender, EventArgs e)
        {
            lblMessage.Visible = true;
            string filePath = FileUpload1.PostedFile.FileName; // getting the file path of uploaded file
            string filpath = Path.GetFileName(filePath); // getting the file name of uploaded file


            string Server_FileName = String.Empty;
            string Server_FilePath = String.Empty;
            string FileType = String.Empty;
            string ext = string.Empty;
            Byte[] bytes = { 0 };

            ext = Path.GetExtension(filpath); // getting the file extension of uploaded file

            Server_FileName = lbl_jobid.Text.ToString() + "-" + Path.GetFileName(filePath); // getting the file name of uploaded file


            if (!FileUpload1.HasFile)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup1();", true);
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please Select File"; //if file uploader has no file selected

                lbl_fileyesno.Text = "No";
                FileFlag = false;
            }
            else if (FileUpload1.HasFile)
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
                        //switch (ext) // this switch code validate the files which allow to upload only PDF file
                        //{
                        //    case ".jpg":
                        //        FileType = "image/jpg";
                        //        break;

                        //    case ".jpeg":
                        //        FileType = "image/jpeg";
                        //        break;
                        //    case ".png":
                        //        FileType = "image/png";
                        //        break;
                        //}

                        switch (ext)
                        {
                            case ".jpg":
                            case ".jpeg":
                            case ".png":
                                FileType = "image/jpeg"; // Change this to the appropriate content type for the resized image

                                // Resize and compress the image before saving
                                using (System.Drawing.Image originalImage = System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream))
                                {
                                    int maxWidth = 800; // Adjust this value based on your requirements
                                    int maxHeight = 600; // Adjust this value based on your requirements

                                    // Calculate new dimensions while maintaining aspect ratio
                                    int newWidth, newHeight;
                                    if (originalImage.Width > originalImage.Height)
                                    {
                                        newWidth = maxWidth;
                                        newHeight = (int)((double)originalImage.Height / originalImage.Width * maxWidth);
                                    }
                                    else
                                    {
                                        newWidth = (int)((double)originalImage.Width / originalImage.Height * maxHeight);
                                        newHeight = maxHeight;
                                    }

                                    using (System.Drawing.Image resizedImage = new Bitmap(originalImage, newWidth, newHeight))
                                    {
                                        // Save the resized and compressed image
                                        Server_FilePath = Server.MapPath(@"\erp_images\Permits\") + lbl_jobid.Text.ToString() + "-" + Path.GetFileNameWithoutExtension(FileUpload1.PostedFile.FileName) + ext;
                                        resizedImage.Save(Server_FilePath, System.Drawing.Imaging.ImageFormat.Jpeg); // Change the format if needed
                                    }
                                }

                                break;
                        }
                    }

                    //if (FileType != String.Empty)
                    //{
                    //    Server_FilePath = Server.MapPath(@"\erp_images\Permits\") + lbl_jobid.Text.ToString() + "-" + Path.GetFileName(FileUpload1.PostedFile.FileName);
                    //    FileUpload1.SaveAs(Server_FilePath);

                    //    Stream fs = FileUpload1.PostedFile.InputStream;
                    //    BinaryReader br = new BinaryReader(fs); //reads the binary files
                    //    bytes = br.ReadBytes((Int32)fs.Length); //counting the file length into bytes

                    //    //ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup1();", true);

                    //    string title = "Notifications :";
                    //    string body = "File Uploaded, Proceed...!!";
                    //    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                    //    lblMessage.ForeColor = System.Drawing.Color.Green;
                    //    lblMessage.Text = "File Uploaded Successfully";

                    //    FileFlag = true;
                    //    lbl_fileyesno.Text = "Yes";

                    //    InsertIntoDB(Server_FileName, FileType, ext, bytes);

                    //    string ddljobid = DDL_JOBID.SelectedItem.Text.ToString();
                    //    Bind_JOBIDDetails(ddljobid);
                    //    VisibilityOffAfterLoading();
                    //}
                    //else
                    //{
                    //    ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup1();", true);
                    //    lblMessage.ForeColor = System.Drawing.Color.Red;
                    //    lblMessage.Text = "Select Only PDF File having extension (.pdf) ";

                    //    FileFlag = false;
                    //    lbl_fileyesno.Text = "No";
                    //}

                    if (FileType != String.Empty)
                    {
                        try
                        {
                            //Server_FilePath = Server.MapPath(@"\erp_images\Permits\") + lbl_jobid.Text.ToString() + "-" + Path.GetFileName(FileUpload1.PostedFile.FileName);
                            //FileUpload1.SaveAs(Server_FilePath);

                            using (Stream fs = FileUpload1.PostedFile.InputStream)
                            using (BinaryReader br = new BinaryReader(fs)) // reads the binary files
                            {
                                bytes = br.ReadBytes((Int32)fs.Length); // counting the file length into bytes
                            }

                            string title = "Notifications :";
                            string body = "File Uploaded, Proceed...!!";
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                            lblMessage.ForeColor = System.Drawing.Color.Green;
                            lblMessage.Text = "File Uploaded Successfully";

                            FileFlag = true;
                            lbl_fileyesno.Text = "Yes";

                            InsertIntoDB(Server_FileName, FileType, ext, bytes);

                            string ddljobid = DDL_JOBID.SelectedItem.Text.ToString();
                            Bind_JOBIDDetails(ddljobid);
                            VisibilityOffAfterLoading();
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

        private void VisibilityOffAfterLoading()
        {
            UpldTyp_Row1.Visible = false;
            UpldTyp_Row2.Visible = false;

            cancelbtn_row1.Visible = false;
            cancelbtn_row2.Visible = false;

            pdfuploadbuttonrow1.Visible = false;
            pdfuploadbuttonrow2.Visible = false;
        }

        protected void InsertIntoDB(string Server_FileName, string FileType, string ext, byte[] b)
        {
            Byte[] bytes = { 0 };
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "insert into tbl_jobspermit(JOBID,UploadType,Name,FileType,Extension,Data,Submitter_Name,Submitter_Wrk) VALUES(@JOBID,@UploadType,@Name,@FileType,@Extension,@Data,@Submitter_Name,@Submitter_Wrk) ";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@JOBID", lbl_jobid.Text.ToString());
                cmd.Parameters.AddWithValue("@UploadType", DDL_UploadType.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@Name", Server_FileName);  //Actula photograph filename
                cmd.Parameters.AddWithValue("@FileType", FileType);
                cmd.Parameters.AddWithValue("@Extension", ext);
                cmd.Parameters.AddWithValue("@Data", bytes);  //Bydefault 0x0
                cmd.Parameters.AddWithValue("@Submitter_Name", Session["USERNAME"].ToString());
                cmd.Parameters.AddWithValue("@Submitter_Wrk", Session["WORKMAN"].ToString());
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                UpdatePermiStatus();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Error: " + ex.Message.ToString();
            }
        }

        protected void UpdatePermiStatus()
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_jobs set UploadType=@UploadType,JOB_Status=@JOB_Status, FileCount=@FileCount,FinalUpldStatus=@FinalUpldStatus, PermitUpload=@PermitUpload, PermitUploadDate=@PermitUploadDate, MasterStatusCode=@MasterStatusCode where JOBID=@JOBID";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@JOBID", lbl_jobid.Text.ToString());
                cmd.Parameters.AddWithValue("@UploadType", DDL_UploadType.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@JOB_Status", "Permit Uploaded");
                Int32 count = CC.Find_PermitUploadCount(lbl_jobid.Text.ToString());
                Int32 newcount = 0;
                string UploadStatus = "";
                string Masterstatus = "";
                string MasterCode = "";
                if (count == 0)
                {
                    if (DDL_UploadType.SelectedItem.Text.ToString() == "PDF File")
                    {
                        newcount = 1;
                        UploadStatus = "Yes";
                        Masterstatus = "Yes";
                        MasterCode = "2";
                    }
                    else
                    {
                        newcount = 1;
                        UploadStatus = "Yes";
                        Masterstatus = "No";
                        MasterCode = "1";
                    }
                }
                else
                {
                    newcount = count + 1;
                    UploadStatus = "Yes";
                    Masterstatus = "Yes";
                    MasterCode = "2";
                }
                cmd.Parameters.AddWithValue("@FinalUpldStatus", Masterstatus);
                cmd.Parameters.AddWithValue("@FileCount", newcount);
                cmd.Parameters.AddWithValue("@PermitUpload", UploadStatus);
                cmd.Parameters.AddWithValue("@MasterStatusCode", MasterCode);  // JOBID created, Permit Uploaded, Can Proceed to Entry Page
                cmd.Parameters.AddWithValue("@PermitUploadDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Error: " + ex.Message.ToString();
                //throw;
            }
        }


        //protected void DownloadFile(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int id = int.Parse((sender as LinkButton).CommandArgument);
        //        byte[] bytes;
        //        string fileName, contentType;
        //        string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
        //        using (SqlConnection con = new SqlConnection(constr))
        //        {
        //            using (SqlCommand cmd = new SqlCommand())
        //            {
        //                cmd.CommandText = "select Name, Data, FileType from tbl_jobspermit where Id=@Id";
        //                cmd.Parameters.AddWithValue("@Id", id);
        //                cmd.Connection = con;
        //                con.Open();
        //                using (SqlDataReader sdr = cmd.ExecuteReader())
        //                {
        //                    sdr.Read();
        //                    bytes = (byte[])sdr["Data"];
        //                    contentType = sdr["FileType"].ToString();
        //                    fileName = sdr["Name"].ToString();
        //                }
        //                con.Close();
        //            }
        //        }
        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.Charset = "";
        //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        Response.ContentType = contentType;
        //        Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
        //        Response.BinaryWrite(bytes);
        //        Response.Flush();
        //        Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
        //        lblMessage.ForeColor = System.Drawing.Color.Red;
        //        lblMessage.Text = "Error: " + ex.Message.ToString();
        //        //throw;
        //    }
        //}

        protected void DownloadFile(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse((sender as LinkButton).CommandArgument);
                string fileName;
                string constr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "select * from tbl_jobspermit where Id=@Id";
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            sdr.Read();
                            fileName = sdr["Name"].ToString();
                        }
                        con.Close();
                    }
                }
                Response.Clear();
                Response.ContentType = "application/octect-stream";
                Response.AppendHeader("content-disposition", "filename=" + fileName);
                Response.TransmitFile(Server.MapPath(@"\erp_images\Permits\") + fileName);
                Response.End();
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                //lbl_msg.ForeColor = System.Drawing.Color.Red;
                //lbl_msg.Text = "Error: " + ex.Message.ToString();
                throw;
            }
        }

        protected void btn_inpunch_Click(object sender, EventArgs e)
        {
            Response.Redirect("job_outpunch.aspx");
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label ID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Id");
            string id = ID.Text.ToString();

            Label JOBID = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_JOBID");
            string jobid = JOBID.Text.ToString();

            Label filename = (Label)GridView1.Rows[e.RowIndex].FindControl("lbl_Name");
            string file = filename.Text.ToString();


            try
            {


                Int32 count = CC.Find_PermitUploadCount(jobid);
                Int32 newcount = 0;
                if (count == 1)
                {
                    newcount = 0;
                    UpdatePermitZeroCount(jobid);

                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    string cmdString = "delete from tbl_jobspermit where Id='" + id + "' and JOBID= '" + jobid + "' ";
                    SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    dbcl.Conn.Close();

                    DeletefromFolder(file);

                    string title = "Notifications :";
                    string body = "Attachment Deleted Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else if (count >= 1)
                {
                    newcount = count - 1;
                    UpdatePermitCount(jobid, newcount);

                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    string cmdString = "delete from tbl_jobspermit where Id='" + id + "' and JOBID= '" + jobid + "' ";
                    SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    dbcl.Conn.Close();

                    DeletefromFolder(file);

                    string title = "Notifications :";
                    string body = "Attachment Deleted Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                else
                {
                    dbcl.Sqlconnection();
                    dbcl.ConnectDb();
                    string cmdString = "delete from tbl_jobspermit where Id='" + id + "' and JOBID= '" + jobid + "' ";
                    SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    dbcl.Conn.Close();

                    string title = "Notifications :";
                    string body = "Attachment Deleted Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }

                string ddljobid = DDL_JOBID.SelectedItem.Text.ToString();
                Bind_JOBIDDetails(ddljobid);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                //throw;
            }
        }

        private void DeletefromFolder(string authorsFile)
        {
            //string authorsFile = "Authors.txt";

            try
            {
                // Check if file exists with its full path
                if (File.Exists(Path.Combine(rootFolder, authorsFile)))
                {
                    // If file found, delete it
                    File.Delete(Path.Combine(rootFolder, authorsFile));
                    //Console.WriteLine("File deleted.");
                }
                else
                {
                    string title = "Notifications :";
                    string body = "NO File Found...!!";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
            }
            catch (IOException ioExp)
            {
                string title = "Notifications :";
                string body = ioExp.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }
        private void UpdatePermitCount(string jobid, Int32 newcount)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbcl.Conn;
            string CmdString = "UPDATE tbl_jobs set FileCount=@FileCount, PermitDeleteDate=@PermitDeleteDate, PermitDeletedByName=@PermitDeletedByName , PermitDeletedByWrk=@PermitDeletedByWrk where JOBID=@JOBID";
            cmd.CommandText = CmdString;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@JOBID", jobid);
            cmd.Parameters.AddWithValue("@FileCount", newcount);
            cmd.Parameters.AddWithValue("@PermitDeleteDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
            cmd.Parameters.AddWithValue("@PermitDeletedByName", Session["USERNAME"].ToString());
            cmd.Parameters.AddWithValue("@PermitDeletedByWrk", Session["WORKMAN"].ToString());
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        private void UpdatePermitZeroCount(string jobid)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbcl.Conn;
            string CmdString = "UPDATE tbl_jobs set UploadType=@UploadType,JOB_Status=@JOB_Status, FileCount=@FileCount,FinalUpldStatus=@FinalUpldStatus, PermitUpload=@PermitUpload, PermitUploadDate=@PermitUploadDate, PermitDeleteDate=@PermitDeleteDate, MasterStatusCode=@MasterStatusCode, PermitDeletedByName=@PermitDeletedByName, PermitDeletedByWrk=@PermitDeletedByWrk where JOBID=@JOBID";
            cmd.CommandText = CmdString;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@JOBID", jobid);
            cmd.Parameters.AddWithValue("@UploadType", "");
            cmd.Parameters.AddWithValue("@JOB_Status", "Created");
            cmd.Parameters.AddWithValue("@FinalUpldStatus", "No");
            cmd.Parameters.AddWithValue("@FileCount", "0");
            cmd.Parameters.AddWithValue("@PermitUpload", "No");
            cmd.Parameters.AddWithValue("@MasterStatusCode", "1");  // JOBID created, Permit Uploaded, Can Proceed to Entry Page
            cmd.Parameters.AddWithValue("@PermitUploadDate", "");
            cmd.Parameters.AddWithValue("@PermitDeleteDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
            cmd.Parameters.AddWithValue("@PermitDeletedByName", Session["USERNAME"].ToString());
            cmd.Parameters.AddWithValue("@PermitDeletedByWrk", Session["WORKMAN"].ToString());
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
    }
}