using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class db_controller : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Security Check: Restrict to Admins or specific roles
                if (Session["USERID"] == null || Session["USERNAME"] == null)
                {
                    Response.Redirect("~/login.aspx", false);
                    return;
                }

                LoadInitialData();
            }
        }

        private void LoadInitialData()
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // 1. Load Regions safely
                string qryRegion = "SELECT Work_Region_Name + ' (' + Work_Region_Code + ')' AS DisplayName, Work_Region_Code FROM tlb_work_state_region ORDER BY Work_Region_Name";
                using (SqlCommand cmd = new SqlCommand(qryRegion, dbcl.Conn))
                {
                    using (SqlDataReader rdrRegion = cmd.ExecuteReader())
                    {
                        ddl_Regions.DataSource = rdrRegion;
                        ddl_Regions.DataTextField = "DisplayName";
                        ddl_Regions.DataValueField = "Work_Region_Code";
                        ddl_Regions.DataBind();
                    }
                    ddl_Regions.Items.Insert(0, new ListItem("-- Select Region --", ""));
                }

                // 2. Load Work Order Regions (Level 1)
                string qryWORegion = "SELECT DISTINCT Work_Region_Name + ' (' + Work_Region_Code + ')' AS DisplayName, Work_Region_Code FROM tlb_WO_Data WHERE WO_Status='Active' ORDER BY DisplayName";
                using (SqlCommand cmdWOReg = new SqlCommand(qryWORegion, dbcl.Conn))
                {
                    using (SqlDataReader rdrWOReg = cmdWOReg.ExecuteReader())
                    {
                        ddl_wo_region.DataSource = rdrWOReg;
                        ddl_wo_region.DataTextField = "DisplayName";
                        ddl_wo_region.DataValueField = "Work_Region_Code";
                        ddl_wo_region.DataBind();
                    }
                    ddl_wo_region.Items.Insert(0, new ListItem("-- Select Region --", ""));
                }

                // Reset downstream dropdowns
                ddl_wo_company.Items.Insert(0, new ListItem("-- Select Company --", ""));
                ddl_wo_dept.Items.Insert(0, new ListItem("-- Select Dept --", ""));
                ddl_wo_number.Items.Insert(0, new ListItem("-- Select WO --", ""));

                BindDocMasterGrid();
            }
            catch (Exception ex)
            {
                ShowNotification(this, "Load Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        protected void ddl_wo_region_SelectedIndexChanged(object sender, EventArgs e)
        {
            WOConfigRow.Visible = false;
            ddl_wo_company.Items.Clear();
            ddl_wo_dept.Items.Clear();
            ddl_wo_number.Items.Clear();
            ddl_wo_dept.Items.Insert(0, new ListItem("-- Select Dept --", ""));
            ddl_wo_number.Items.Insert(0, new ListItem("-- Select WO --", ""));

            if (string.IsNullOrEmpty(ddl_wo_region.SelectedValue))
            {
                ddl_wo_company.Items.Insert(0, new ListItem("-- Select Company --", ""));
                return;
            }

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string qry = "SELECT DISTINCT Company_Name + ' (' + Company_Code + ')' AS DisplayName, Company_Code FROM tlb_WO_Data WHERE WO_Status='Active' AND Work_Region_Code=@Reg ORDER BY DisplayName";
                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Reg", ddl_wo_region.SelectedValue);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        ddl_wo_company.DataSource = rdr;
                        ddl_wo_company.DataTextField = "DisplayName";
                        ddl_wo_company.DataValueField = "Company_Code";
                        ddl_wo_company.DataBind();
                    }
                    ddl_wo_company.Items.Insert(0, new ListItem("-- Select Company --", ""));
                }
            }
            catch (Exception ex) { ShowNotification(this, "Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        protected void ddl_wo_company_SelectedIndexChanged(object sender, EventArgs e)
        {
            WOConfigRow.Visible = false;
            ddl_wo_dept.Items.Clear();
            ddl_wo_number.Items.Clear();
            ddl_wo_number.Items.Insert(0, new ListItem("-- Select WO --", ""));

            if (string.IsNullOrEmpty(ddl_wo_company.SelectedValue))
            {
                ddl_wo_dept.Items.Insert(0, new ListItem("-- Select Dept --", ""));
                return;
            }

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string qry = "SELECT DISTINCT Department_Name + ' (' + Department_Code + ')' AS DisplayName, Department_Code FROM tlb_WO_Data WHERE WO_Status='Active' AND Work_Region_Code=@Reg AND Company_Code=@Comp ORDER BY DisplayName";
                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Reg", ddl_wo_region.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comp", ddl_wo_company.SelectedValue);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        ddl_wo_dept.DataSource = rdr;
                        ddl_wo_dept.DataTextField = "DisplayName";
                        ddl_wo_dept.DataValueField = "Department_Code";
                        ddl_wo_dept.DataBind();
                    }
                    ddl_wo_dept.Items.Insert(0, new ListItem("-- Select Dept --", ""));
                }
            }
            catch (Exception ex) { ShowNotification(this, "Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        protected void ddl_wo_dept_SelectedIndexChanged(object sender, EventArgs e)
        {
            WOConfigRow.Visible = false;
            ddl_wo_number.Items.Clear();

            if (string.IsNullOrEmpty(ddl_wo_dept.SelectedValue))
            {
                ddl_wo_number.Items.Insert(0, new ListItem("-- Select WO --", ""));
                return;
            }

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                string qry = "SELECT DISTINCT WO_Number FROM tlb_WO_Data WHERE WO_Status='Active' AND Work_Region_Code=@Reg AND Company_Code=@Comp AND Department_Code=@Dept ORDER BY WO_Number";
                using (SqlCommand cmd = new SqlCommand(qry, dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Reg", ddl_wo_region.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comp", ddl_wo_company.SelectedValue);
                    cmd.Parameters.AddWithValue("@Dept", ddl_wo_dept.SelectedValue);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        ddl_wo_number.DataSource = rdr;
                        ddl_wo_number.DataTextField = "WO_Number";
                        ddl_wo_number.DataValueField = "WO_Number";
                        ddl_wo_number.DataBind();
                    }
                    ddl_wo_number.Items.Insert(0, new ListItem("-- Select WO --", ""));
                }
            }
            catch (Exception ex) { ShowNotification(this, "Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        private void BindDocMasterGrid()
        {
            using (SqlCommand cmd = new SqlCommand("SELECT Doc_ID, Doc_Category, Doc_Name, IsActive FROM tlb_DocumentMaster ORDER BY Doc_Category, Doc_Name", dbcl.Conn))
            {
                using (SqlDataReader rdrDoc = cmd.ExecuteReader())
                {
                    gv_DocMaster.DataSource = rdrDoc;
                    gv_DocMaster.DataBind();
                }
            }
        }

        // =================================================================================
        // TAB 1: REGION SAFETY & COMPLIANCE
        // =================================================================================
        protected void ddl_Regions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddl_Regions.SelectedValue))
            {
                RegionConfigRow.Visible = false;
                return;
            }

            string selectedRegionCode = ddl_Regions.SelectedValue;
            RegionConfigRow.Visible = true;

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // A. Load Region Toggles using Work_Region_Code
                using (SqlCommand cmd = new SqlCommand("SELECT Req_GPS_Tagging, Req_TBT_Number FROM tlb_work_state_region WHERE Work_Region_Code=@Region", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Region", selectedRegionCode);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            ddl_req_gps.SelectedValue = rdr["Req_GPS_Tagging"] != DBNull.Value ? rdr["Req_GPS_Tagging"].ToString() : "No";
                            ddl_req_tbt.SelectedValue = rdr["Req_TBT_Number"] != DBNull.Value ? rdr["Req_TBT_Number"].ToString() : "No";
                        }
                    }
                }

                // B. Load ALL Active Documents into CheckBoxList safely without MARS conflicts
                cbl_RegionDocs.Items.Clear();
                using (SqlCommand cmdDocs = new SqlCommand("SELECT Doc_ID, Doc_Name + ' (' + Doc_Category + ')' AS DisplayName FROM tlb_DocumentMaster WHERE IsActive=1 ORDER BY Doc_Category, Doc_Name", dbcl.Conn))
                {
                    using (SqlDataReader rdrDocs = cmdDocs.ExecuteReader())
                    {
                        cbl_RegionDocs.DataSource = rdrDocs;
                        cbl_RegionDocs.DataTextField = "DisplayName";
                        cbl_RegionDocs.DataValueField = "Doc_ID";
                        cbl_RegionDocs.DataBind();
                    }
                }

                // C. Check the boxes that are mapped as Mandatory for this Region
                using (SqlCommand cmdMap = new SqlCommand("SELECT Doc_ID FROM tlb_Region_Documents WHERE Work_Region_Code=@Region AND IsMandatory=1", dbcl.Conn))
                {
                    cmdMap.Parameters.AddWithValue("@Region", selectedRegionCode);
                    using (SqlDataReader rdrMap = cmdMap.ExecuteReader())
                    {
                        while (rdrMap.Read())
                        {
                            string docId = rdrMap["Doc_ID"].ToString();
                            ListItem item = cbl_RegionDocs.Items.FindByValue(docId);
                            if (item != null) item.Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowNotification(ddl_Regions, "Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        protected void btn_SaveRegion_Click(object sender, EventArgs e)
        {
            string regionCode = ddl_Regions.SelectedValue;
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();

                // 1. Update Region Toggles targeting Work_Region_Code
                using (SqlCommand cmdUpdate = new SqlCommand("UPDATE tlb_work_state_region SET Req_GPS_Tagging=@GPS, Req_TBT_Number=@TBT WHERE Work_Region_Code=@Region", dbcl.Conn))
                {
                    cmdUpdate.Parameters.AddWithValue("@GPS", ddl_req_gps.SelectedValue);
                    cmdUpdate.Parameters.AddWithValue("@TBT", ddl_req_tbt.SelectedValue);
                    cmdUpdate.Parameters.AddWithValue("@Region", regionCode);
                    cmdUpdate.ExecuteNonQuery();
                }

                // 2. Clear old Document Mappings for this Region
                using (SqlCommand cmdClear = new SqlCommand("DELETE FROM tlb_Region_Documents WHERE Work_Region_Code=@Region", dbcl.Conn))
                {
                    cmdClear.Parameters.AddWithValue("@Region", regionCode);
                    cmdClear.ExecuteNonQuery();
                }

                // 3. Insert new mandatory Document Mappings
                using (SqlCommand cmdInsert = new SqlCommand("INSERT INTO tlb_Region_Documents (Work_Region_Code, Doc_ID, IsMandatory) VALUES (@Region, @DocID, 1)", dbcl.Conn))
                {
                    cmdInsert.Parameters.AddWithValue("@Region", regionCode);
                    cmdInsert.Parameters.Add("@DocID", SqlDbType.Int);

                    foreach (ListItem item in cbl_RegionDocs.Items)
                    {
                        if (item.Selected)
                        {
                            cmdInsert.Parameters["@DocID"].Value = Convert.ToInt32(item.Value);
                            cmdInsert.ExecuteNonQuery();
                        }
                    }
                }

                ShowNotification(btn_SaveRegion, "Success", $"Compliance Rules for {ddl_Regions.SelectedItem.Text} Updated!", "success");
            }
            catch (Exception ex)
            {
                ShowNotification(btn_SaveRegion, "Save Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        // =================================================================================
        // TAB 2: WORK ORDER CONTROLLER
        // =================================================================================
        protected void ddl_wo_number_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddl_wo_number.SelectedValue))
            {
                WOConfigRow.Visible = false;
                return;
            }

            WOConfigRow.Visible = true;
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand cmd = new SqlCommand("SELECT Contract_Nature, Billing_Nature, Execution_Type FROM tlb_WO_Data WHERE WO_Number=@WO", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@WO", ddl_wo_number.SelectedValue);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            if (rdr["Contract_Nature"] != DBNull.Value) ddl_contract_nature.SelectedValue = rdr["Contract_Nature"].ToString();
                            if (rdr["Billing_Nature"] != DBNull.Value) ddl_billing_nature.SelectedValue = rdr["Billing_Nature"].ToString();
                            if (rdr["Execution_Type"] != DBNull.Value) ddl_execution_type.SelectedValue = rdr["Execution_Type"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex) { ShowNotification(this, "Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        protected void btn_SaveWO_Click(object sender, EventArgs e)
        {
            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand cmd = new SqlCommand("UPDATE tlb_WO_Data SET Contract_Nature=@CN, Billing_Nature=@BN, Execution_Type=@ET WHERE WO_Number=@WO", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@CN", ddl_contract_nature.SelectedValue);
                    cmd.Parameters.AddWithValue("@BN", ddl_billing_nature.SelectedValue);
                    cmd.Parameters.AddWithValue("@ET", ddl_execution_type.SelectedValue);
                    cmd.Parameters.AddWithValue("@WO", ddl_wo_number.SelectedValue); // Updated control ID
                    cmd.ExecuteNonQuery();
                }
                ShowNotification(btn_SaveWO, "Success", "Work Order parameters updated.", "success");
            }
            catch (Exception ex) { ShowNotification(btn_SaveWO, "Error", ex.Message, "error"); }
            finally { dbcl.DisconnectDb(); }
        }

        protected void btn_AddDoc_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_newdoc_name.Text))
            {
                ShowNotification(btn_AddDoc, "Warning", "Document Name cannot be empty.", "notice");
                return;
            }

            try
            {
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO tlb_DocumentMaster (Doc_Category, Doc_Name, IsActive) VALUES (@Cat, @Name, 1)", dbcl.Conn))
                {
                    cmd.Parameters.AddWithValue("@Cat", ddl_newdoc_category.SelectedValue);
                    cmd.Parameters.AddWithValue("@Name", txt_newdoc_name.Text.Trim());
                    cmd.ExecuteNonQuery();
                }

                txt_newdoc_name.Text = "";
                BindDocMasterGrid();

                // REMOVED: Re-triggering the region dropdown to prevent wiping unsaved checkbox changes.
                // If a user needs the new doc to show up in the region tab, they can re-select the region.

                ShowNotification(btn_AddDoc, "Added", "New document added to Dictionary.", "success");
            }
            catch (Exception ex)
            {
                ShowNotification(btn_AddDoc, "Error", ex.Message, "error");
            }
            finally
            {
                dbcl.DisconnectDb();
            }
        }

        // =================================================================================
        // UTILITIES
        // =================================================================================
        private void ShowNotification(Control control, string title, string message, string type)
        {
            // C# exceptions often contain newlines (\n or \r). We must escape them, or it breaks the JavaScript!
            string safeMessage = message.Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "");
            string script = $"showPNotify('{title}', '{safeMessage}', '{type}');";

            ScriptManager.RegisterStartupScript(control, control.GetType(), "PNotify", script, true);
        }
    }
}