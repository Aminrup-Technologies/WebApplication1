using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication1.bussiness.production
{
    public partial class add_nwhelpdsk : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

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
                    string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region order by Id";
                    BindRootCategories(CmdString1);
                }
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
                            DDL_RootCategory.DataTextField = "Work_Region_Name";
                            DDL_RootCategory.DataValueField = "Work_Region_Code";
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

        protected void DDL_RootCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if the selected index is not equal to 0
            if (DDL_RootCategory.SelectedIndex != 0)
            {
                // Get the selected value from the DropDownList
                string selectedValue = DDL_RootCategory.SelectedValue;

                // Perform actions based on the selected value
                string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region order by Id";
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
                            DDL_ChildCategory.DataTextField = "Work_Region_Name";
                            DDL_ChildCategory.DataValueField = "Work_Region_Code";
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
                string selectedValue = DDL_RootCategory.SelectedValue;

                // Perform actions based on the selected value
                string CmdString1 = "select Work_Region_Name, Work_Region_Code from tlb_work_state_region order by Id";
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
    }
}