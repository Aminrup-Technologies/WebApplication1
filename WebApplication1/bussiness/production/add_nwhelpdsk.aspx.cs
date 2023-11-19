using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Net;

namespace WebApplication1.bussiness.production
{
    public partial class add_nwhelpdsk : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        HelpdeskCalls helpDeskService = new HelpdeskCalls();

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
                    string CmdString1 = "select root1_name, Id from tlb_hlpdsk_root1 order by Id";
                    BindRootCategories(CmdString1);

                    string CmdString2 = "select LevelCategory, Id from tlb_supportlvl order by Id";
                    BindSupportLevel(CmdString2);

                }
            }
            else
            {

            }
        }

        private void BindRootCategories(string cmdString)
        {
            dbcl.Sqlconnection();
            using (SqlConnection connection = dbcl.Conn)
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(cmdString, connection))
                    {
                        command.CommandType = CommandType.Text;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DDL_RootCategory.DataSource = reader;
                            DDL_RootCategory.DataTextField = "root1_name";
                            DDL_RootCategory.DataValueField = "Id";
                            DDL_RootCategory.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string title = "Notifications :";
                    string body = ex.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            DDL_RootCategory.Items.Insert(0, "Please Select Option");
        }

        private void BindSupportLevel(string cmdString)
        {
            dbcl.Sqlconnection();
            using (SqlConnection connection = dbcl.Conn)
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(cmdString, connection))
                    {
                        command.CommandType = CommandType.Text;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DDL_HelpLevel.DataSource = reader;
                            DDL_HelpLevel.DataTextField = "LevelCategory";
                            DDL_HelpLevel.DataValueField = "Id";
                            DDL_HelpLevel.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string title = "Notifications :";
                    string body = ex.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            DDL_HelpLevel.Items.Insert(0, "Please Select Option");
        }

        protected void DDL_RootCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if the selected index is not equal to 0
            if (DDL_RootCategory.SelectedIndex != 0)
            {
                // Get the selected value from the DropDownList
                string selectedValue = DDL_RootCategory.SelectedItem.Text.ToString();

                // Perform actions based on the selected value
                string CmdString1 = "select root2_name, Id from tlb_hlpdsk_root2 where root1_name = '"+ selectedValue + "' order by Id";
                Bind_SupportTopic(CmdString1);

            }
            else
            {
                // Display a message or perform actions when the selected index is 0
                string title = "Notifications :";
                string body = "Please select a valid option";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void Bind_SupportTopic(string cmdString)
        {
            dbcl.Sqlconnection();
            using (SqlConnection connection = dbcl.Conn)
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(cmdString, connection))
                    {
                        command.CommandType = CommandType.Text;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DDL_ChildCategory.DataSource = reader;
                            DDL_ChildCategory.DataTextField = "root2_name";
                            DDL_ChildCategory.DataValueField = "Id";
                            DDL_ChildCategory.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string title = "Notifications :";
                    string body = ex.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            DDL_ChildCategory.Items.Insert(0, "Please Select Option");
        }

        protected void DDL_ChildCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if the selected index is not equal to 0
            if (DDL_RootCategory.SelectedIndex != 0)
            {
                // Get the selected value from the DropDownList
                string selectedValue1 = DDL_RootCategory.SelectedItem.Text.ToString();
                string selectedValue2 = DDL_ChildCategory.SelectedItem.Text.ToString();

                // Perform actions based on the selected value
                string CmdString1 = "select root3_name, Id from tlb_hlpdsk_root3 where root1_name='" + selectedValue1 + "' and root2_name='" + selectedValue2 + "' order by Id";
                Bind_SupportSubject(CmdString1);

            }
            else
            {
                // Display a message or perform actions when the selected index is 0
                string title = "Notifications :";
                string body = "Please select a valid option";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void Bind_SupportSubject(string cmdString)
        {
            dbcl.Sqlconnection();
            using (SqlConnection connection = dbcl.Conn)
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(cmdString, connection))
                    {
                        command.CommandType = CommandType.Text;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DDL_Subject.DataSource = reader;
                            DDL_Subject.DataTextField = "Work_Region_Name";
                            DDL_Subject.DataValueField = "Work_Region_Code";
                            DDL_Subject.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string title = "Notifications :";
                    string body = ex.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            DDL_Subject.Items.Insert(0, "Please Select Option");
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string hlpdskid = ExecuteStoredProcedure();
            string root1Value = DDL_RootCategory.SelectedItem.Text.ToString();
            string root2Value = DDL_ChildCategory.SelectedItem.Text.ToString();
            string root3Value = DDL_Subject.SelectedItem.Text.ToString();

            string priorityLevel = "High";
            string description = txt_descp.Text.ToString();

            string status = "Created";

            string createdByWorkman = Session["WORKMAN"].ToString();
            string createdByName = Session["USERNAME"].ToString();
            string creatorRegion = Session["REGION"].ToString();
            string creatorComp = Session["COMPANY_CODE"].ToString();



            bool isInsertSuccessful = helpDeskService.InsertHelpDeskTicket(hlpdskid, createdByWorkman, createdByName, creatorRegion, creatorComp, root1Value, root2Value, root3Value, priorityLevel, description, status);

            // Check the flag and take appropriate action
            if (isInsertSuccessful)
            {
                helpDeskService.SendEmail(hlpdskid, createdByWorkman, createdByName, creatorRegion, creatorComp, root1Value, root2Value, root3Value, priorityLevel, description);
                string title = "Notifications :";
                string body = "Insertion successful!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            else
            {
                string title = "Notifications :";
                string body = "Insertion failed.";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }

        }

        private string ExecuteStoredProcedure()
        {
            string result = string.Empty;
            try
            {
                dbcl.Sqlconnection();
                using (SqlConnection connection = dbcl.Conn)
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("Generate_HelpdeskTicket_ID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        //using (SqlDataReader reader = command.ExecuteReader())
                        //{
                        //    if (reader.Read())
                        //    {
                        //        result = reader["ResultCode"].ToString();
                        //    }
                        //    else
                        //    {
                        //        result = "No result";
                        //    }
                        //}

                        // If the stored procedure returns a single value, you can also use ExecuteScalar
                        result = command.ExecuteScalar().ToString();

                        //Console.WriteLine("Result Code: " + result);
                    }
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            return result;
        }
    }
}