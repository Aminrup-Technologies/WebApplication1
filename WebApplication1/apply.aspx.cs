using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;


namespace atsweb
{


    public partial class apply : System.Web.UI.Page
    {

        SqlConnection con;
        SqlCommand cmd;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    LoadFormEntries();
            //}

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            try
            {
                con = new SqlConnection(Conection.GetConnectionString());
                cmd = new SqlCommand("ApplyformSp", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Action is 'INSERT' to add a new record
                cmd.Parameters.AddWithValue("@Action", "INSERT");
                cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@PostCode", txtPostCode.Text.Trim());
                // Handle file upload if available
                if (fuUserImage.HasFile)
                {
                    string imagePath = "Images/" + fuUserImage.FileName;
                    fuUserImage.SaveAs(Server.MapPath("~/" + imagePath));
                    cmd.Parameters.AddWithValue("@ImagrUrl", imagePath);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ImagrUrl", DBNull.Value);
                }

                con.Open();
                cmd.ExecuteNonQuery();
                lblMsg.Text = "Form submitted successfully!";
                lblMsg.CssClass = "alert alert-success";
                lblMsg.Visible = true;
                Clear();

            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
            }
            finally
            {
                con.Close();
            }
        }

        //private void LoadFormEntries()
        //{
        //    try
        //    {
        //        con = new SqlConnection(Conection.GetConnectionString());
        //        cmd = new SqlCommand("ApplyformSp", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Action", "SELECT");

        //        sda = new SqlDataAdapter(cmd);
        //        dt = new DataTable();
        //        sda.Fill(dt);

        //        gvFormEntries.DataSource = dt;
        //        gvFormEntries.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMsg.Text = "Error: " + ex.Message;
        //        lblMsg.CssClass = "alert alert-danger";
        //    }


        private void Clear()
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtMobile.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtPostCode.Text = string.Empty;
        }
    }  

 }


            
