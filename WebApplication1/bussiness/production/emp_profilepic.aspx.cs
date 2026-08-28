using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;

namespace WebApplication1.bussiness.production
{
    public partial class emp_profilepic : System.Web.UI.Page
    {
        // Default folder
        static readonly string rootFolder = @"C:\atswork.in\wwwroot\erp_images\ProfilePhoto";

        //static readonly string rootFolder = @"D:\OH4Y Works\OH4Y_2021\Demo\WebApplication1\WebApplication1\erp_images\ProfilePhoto";

        static string PrfPicPath = "~\\erp_images\\ProfilePhoto\\No_Image.jpg";
        static string PrfPicFile = "N/A";

        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        Boolean FileFlag = false;


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
                    CheckAndDisplayEmpProfilePic();
                }
            }
        }


        public void CheckAndDisplayEmpProfilePic()
        {
            try
            {
                // Connect to the database
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;

                // SQL query to retrieve profile picture details
                string CmdString = "SELECT PrfPicPath, PrfPicFile FROM tbl_Employee_Mustertable WHERE WorkmanSL = @WorkmanSL";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@WorkmanSL", Session["WORKMAN"].ToString());

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string prfPicPath = reader["PrfPicPath"].ToString();
                    string prfPicFile = reader["PrfPicFile"].ToString();

                    // Check if both path and file are valid
                    if (!string.IsNullOrEmpty(prfPicPath) && !string.IsNullOrEmpty(prfPicFile))
                    {
                        // Combine the path and file name
                        string fullPath = Server.MapPath(prfPicPath);

                        // Check if the physical file exists on the server
                        if (File.Exists(fullPath))
                        {
                            // Set the image URL to the ImgDisplay control
                            ImgDisplay.ImageUrl = prfPicPath;

                            // Display the panel with the image
                            PanelViewPhoto.Visible = true;
                            Photouploaded.Visible = true;
                        }
                        else
                        {
                            // Hide the panel if the file doesn't exist
                            PanelViewPhoto.Visible = false;
                            Photouploaded.Visible = false;
                        }
                    }
                    else
                    {
                        // Hide the panel if the path or file is missing in the database
                        PanelViewPhoto.Visible = false;
                        Photouploaded.Visible = false;
                    }
                }

                reader.Close();
                cmd.Dispose();
                dbcl.Conn.Close();
            }
            catch (Exception)
            {
                // Handle the exception (log or display error)
            }
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

            Server_FileName = Session["WORKMAN"].ToString() + "-" + Path.GetFileName(filePath); // getting the file name of uploaded file


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
                    //if (2 == 2)
                    //{
                    //    switch (ext) // this switch code validate the files which allow to upload only PDF file
                    //    {
                    //        case ".jpg":
                    //            FileType = "image/jpg";
                    //            break;

                    //        case ".jpeg":
                    //            FileType = "image/jpeg";
                    //            break;
                    //        case ".png":
                    //            FileType = "image/png";
                    //            break;
                    //    }
                    //}
                    //if (FileType != String.Empty)
                    //{
                    //    Server_FilePath = Server.MapPath(@"\erp_images\ProfilePhoto\") + Session["WORKMAN"].ToString() + "-" + Path.GetFileName(FileUpload1.PostedFile.FileName);
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

                    //    //InsertIntoDB(Server_FileName, FileType, ext, bytes);
                    //}
                    //else
                    //{
                    //    ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup1();", true);
                    //    lblMessage.ForeColor = System.Drawing.Color.Red;
                    //    lblMessage.Text = "Select Only PDF File having extension (.pdf) ";

                    //    FileFlag = false;
                    //    lbl_fileyesno.Text = "No";
                    //}

                    if (UploadTBMImage() == true)
                    {
                        UPDT_EmpProfilePicInfo();
                        Photouploaded.Visible = true;
                        PanelViewPhoto.Visible = true;
                    }
                    else
                    {
                        Photouploaded.Visible = false;
                        PanelViewPhoto.Visible = false;
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

        public void UPDT_EmpProfilePicInfo()
        {
            try
            {

                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbcl.Conn;
                string CmdString = "UPDATE tbl_Employee_Mustertable set PrfPicPath=@PrfPicPath, PrfPicFile=@PrfPicFile where WorkmanSL=@WorkmanSL";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@WorkmanSL", Session["WORKMAN"].ToString());
                cmd.Parameters.AddWithValue("@PrfPicPath", PrfPicPath);
                cmd.Parameters.AddWithValue("@PrfPicFile", PrfPicFile);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbcl.Conn.Close();

                UploadMessage_Div.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        private Boolean UploadTBMImage()
        {
            Boolean imgsaved = false;
            string PicDate = DateTime.Now.ToString("yyyyMMdd");
            string WorkmanID = Session["WORKMAN"].ToString();

            // Check file exist or not
            if (FileUpload1.PostedFile != null)
            {
                // Check the extension of image
                string extension = Path.GetExtension(FileUpload1.FileName);
                if (extension.ToLower() == ".png" || extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg")
                {
                    Stream strm = FileUpload1.PostedFile.InputStream;
                    using (var image = System.Drawing.Image.FromStream(strm))
                    {
                        int newWidth = 220; // New Width of Image in Pixel
                        int newHeight = 220; // New Height of Image in Pixel
                        var thumbImg = new Bitmap(newWidth, newHeight);
                        var thumbGraph = Graphics.FromImage(thumbImg);

                        thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                        thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                        thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        var imgRectangle = new Rectangle(0, 0, newWidth, newHeight);
                        thumbGraph.DrawImage(image, imgRectangle);

                        // Save the file
                        string targetPath = Server.MapPath(@"~\erp_images\ProfilePhoto\") + WorkmanID + "_" + PicDate + ".jpg";
                        FileUpload1.SaveAs(Server.MapPath(@"~\erp_images\ProfilePhoto\") + WorkmanID + "_" + PicDate + ".jpg");

                        //the below will be saved as database value
                        PrfPicPath = "~\\erp_images\\ProfilePhoto\\" + WorkmanID + "_" + PicDate + ".jpg";
                        thumbImg.Save(targetPath, image.RawFormat);
                        PrfPicFile = WorkmanID + "_" + PicDate + ".jpg";

                        //Show Image instantly
                        ImgDisplay.ImageUrl = @"~\erp_images\ProfilePhoto\" + WorkmanID + "_" + PicDate + ".jpg";

                        //on successfully image is saved
                        imgsaved = true;
                    }
                }
                else
                {
                    //lblmsg.Visible = true;
                    //lblmsg.ForeColor = Color.Red;
                    //lblmsg.Text = "KIndly Select a photo to upload";
                }
            }
            return imgsaved;
        }
    }
}