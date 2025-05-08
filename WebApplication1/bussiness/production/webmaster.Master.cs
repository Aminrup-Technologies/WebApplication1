using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.bussiness.production;
using System.Web.UI.HtmlControls;

namespace WebApplication1.gentelella_master.production
{
    public partial class webmaster : System.Web.UI.MasterPage
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        CountChecker CC = new CountChecker();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    Label lbl1 = (Label)Page.Master.FindControl("lbl_loginusername2");
                    lbl1.Text = Session["USERNAME"].ToString();

                    Label lbl2 = (Label)Page.Master.FindControl("lbl_loginusername1");
                    lbl2.Text = Session["USERFNAME"].ToString();

                    ProfilePic_1.Src = "../../erp_images/ProfilePhoto/" + Session["User_Photo"].ToString() + "";
                    ProfilePic_2.Src = "../../erp_images/ProfilePhoto/" + Session["User_Photo"].ToString() + "";

                    //ProfilePic_1.Src = Session["User_Photo"].ToString();
                    //ProfilePic_2.Src = Session["User_Photo"].ToString();

                    GetIpValue();
                    //GetIpAddress();
                    //PermissionLoader loader = new PermissionLoader();
                    //LoadPermissions(Session["RolePermissionDB"].ToString());

                    DataTable permissions = LoadPermissions(Session["RolePermissionDB"].ToString());
                    ApplyPermissions(permissions);
                    //PermissionCheck();

                    //Label lbl_pendingforappjob = (Label)Page.Master.FindControl("lbl_jobspendingcount");
                    //lbl_pendingforappjob.Text = Convert.ToString(CC.GetPendingJOBApprovalCount(Session["WORKMAN"].ToString()));

                    //Label lbl_approvedjobs = (Label)Page.Master.FindControl("lbl_approvedjobs");
                    //lbl_approvedjobs.Text = Convert.ToString(CC.GetApprovedJOBCount(Session["WORKMAN"].ToString()));

                    //Label lbl_pendingtbt = (Label)Page.Master.FindControl("lbl_tbtpendingapp");
                    //lbl_pendingtbt.Text = Convert.ToString(CC.GetPendingTBTCount(Session["WORKMAN"].ToString()));
                }
            }
        }

        private void GetIpValue()
        {
            string ipAdd = "";
            ipAdd = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ipAdd))
            {
                ipAdd = Request.ServerVariables["REMOTE_ADDR"];
                lbl_IPAddress.Text = HttpUtility.HtmlEncode(ipAdd);
            }
            else
            {
                lbl_IPAddress.Text = HttpUtility.HtmlEncode(ipAdd);
            }
        }

        private void GetIpAddress()
        {
            string userip = Request.UserHostAddress;
            if (Request.UserHostAddress != null)
            {
                Int64 macinfo = new Int64();
                string macSrc = macinfo.ToString("X");
                if (macSrc == "0")
                {
                    if (userip == "127.0.0.1")
                    {
                        lbl_IPAddress.Text = "LOCAL";
                    }
                    else
                    {
                        lbl_IPAddress.Text = HttpUtility.HtmlEncode(userip);
                    }
                }
            }
        }

        // Method to get allowed regions from the database
        public HashSet<string> GetAllowedRegions()
        {
            var allowedRegions = new HashSet<string>();

            string connectionString = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            string query = "SELECT Work_Region_Code FROM tlb_work_state_region where JOBID_Menu='Yes'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    allowedRegions.Add(reader["Work_Region_Code"].ToString());
                }

                reader.Close();
            }

            return allowedRegions;
        }

        protected void PermissionCheck()
        {
            var allowedRegions = GetAllowedRegions();

            if (!allowedRegions.Contains(Session["REGION"].ToString()))
            //if (Session["REGION"].ToString() != "KPO" && Session["REGION"].ToString() != "AGL" && Session["REGION"].ToString() != "JSR" && Session["REGION"].ToString() != "NINL")
            {
                if (Session["USERTYPE"].ToString() == "Office Staff")
                {
                    if (Session["PERMISSION"].ToString() == "Human Resource")
                    {
                        Expenses.Visible = true;
                        DataMastering.Visible = true;
                        Analytics.Visible = false;
                        Payroll.Visible = false;
                        JOBManpower.Visible = false;
                        CSM.Visible = false;
                        JOBApproval.Visible = false;
                        Memo_Billing.Visible = true;
                        budget.Visible = true;
                        add_exphd.Visible = true;
                        add_expsbhd.Visible = true;
                        add_exp.Visible = true;
                        mng_exp.Visible = true;

                    }
                    else
                    {
                        Expenses.Visible = true;
                        DataMastering.Visible = false;
                        Memo_Billing.Visible = true;
                        Analytics.Visible = false;
                        Payroll.Visible = false;
                        JOBManpower.Visible = false;
                        CSM.Visible = false;
                        JOBApproval.Visible = false;
                        budget.Visible = false;
                        add_exphd.Visible = false;
                        add_expsbhd.Visible = false;
                        add_exp.Visible = true;
                        mng_exp.Visible = true;
                    }
                }
                else
                {
                    Expenses.Visible = true;
                    DataMastering.Visible = false;
                    Analytics.Visible = false;
                    Payroll.Visible = false;
                    JOBManpower.Visible = false;
                    CSM.Visible = false;
                    JOBApproval.Visible = false;
                    Memo_Billing.Visible = true;
                    budget.Visible = false;
                    add_exphd.Visible = false;
                    add_expsbhd.Visible = false;
                    add_exp.Visible = true;
                    mng_exp.Visible = true;
                }
            }
            //The below block is for Angul and KPO Employees
            else
            {
                if (Session["USERTYPE"].ToString() == "Office Staff")
                {
                    if (Session["PERMISSION"].ToString() == "Human Resource")
                    {
                        DataMastering.Visible = true;
                        Analytics.Visible = true;
                        Payroll.Visible = true;
                        JOBManpower.Visible = true;
                        JOBApproval.Visible = true;
                        Memo_Billing.Visible = true;
                        CSM.Visible = true;
                        Expenses.Visible = true;
                    }
                    else if (Session["PERMISSION"].ToString() == "Billing")
                    {
                        DataMastering.Visible = true;
                        Analytics.Visible = false;
                        Payroll.Visible = false;
                        JOBManpower.Visible = false;
                        JOBApproval.Visible = false;
                        Memo_Billing.Visible = true;
                        CSM.Visible = false;
                        Expenses.Visible = true;
                    }
                    else if (Session["PERMISSION"].ToString() == "Special Access")
                    {
                        DataMastering.Visible = false;
                        Analytics.Visible = false;
                        Payroll.Visible = false;
                        JOBManpower.Visible = true;
                        JOBApproval.Visible = true;
                        Memo_Billing.Visible = true;
                        CSM.Visible = true;
                        Expenses.Visible = true;
                    }
                }
                //The below block is for Site Staff
                else if (Session["USERTYPE"].ToString() == "Site Staff")
                {
                    if (Session["PERMISSION"].ToString() == "Site Incharge")
                    {
                        DataMastering.Visible = false;
                        Analytics.Visible = false;
                        Payroll.Visible = false;
                        JOBManpower.Visible = true;
                        JOBApproval.Visible = true;
                        Memo_Billing.Visible = true;
                        CSM.Visible = true;
                        Expenses.Visible = true;
                    }
                    else if (Session["PERMISSION"].ToString() == "Safety Officer")
                    {
                        DataMastering.Visible = false;
                        Analytics.Visible = false;
                        Payroll.Visible = false;
                        JOBManpower.Visible = false;
                        JOBApproval.Visible = false;
                        Memo_Billing.Visible = true;
                        CSM.Visible = true;
                        Expenses.Visible = true;
                    }

                    else if (Session["PERMISSION"].ToString() == "Supervisor")
                    {
                        DataMastering.Visible = false;
                        Analytics.Visible = false;
                        Payroll.Visible = false;
                        JOBManpower.Visible = true;
                        JOBApproval.Visible = false;
                        Memo_Billing.Visible = true;
                        CSM.Visible = true;
                        Expenses.Visible = true;
                    }

                    else if (Session["PERMISSION"].ToString() == "Safety Supervisor")
                    {
                        DataMastering.Visible = false;
                        Analytics.Visible = false;
                        Payroll.Visible = false;
                        JOBManpower.Visible = true;
                        JOBApproval.Visible = false;
                        Memo_Billing.Visible = true;
                        CSM.Visible = true;
                        Expenses.Visible = true;
                    }

                    else if (Session["PERMISSION"].ToString() == "Special Access")
                    {
                        DataMastering.Visible = false;
                        Analytics.Visible = false;
                        Payroll.Visible = false;
                        JOBManpower.Visible = true;
                        JOBApproval.Visible = true;
                        Memo_Billing.Visible = true;
                        CSM.Visible = true;
                        Expenses.Visible = true;
                    }

                    else if (Session["PERMISSION"].ToString() == "Worker")
                    {
                        DataMastering.Visible = false;
                        Analytics.Visible = false;
                        Payroll.Visible = false;
                        JOBManpower.Visible = false;
                        JOBApproval.Visible = false;
                        Memo_Billing.Visible = true;
                        CSM.Visible = true;
                        Expenses.Visible = true;
                    }
                }

                else
                {
                    //This is for ATS management
                    DataMastering.Visible = true;
                    Analytics.Visible = true;
                    Payroll.Visible = true;
                    JOBManpower.Visible = true;
                    JOBApproval.Visible = true;
                    Memo_Billing.Visible = true;
                    CSM.Visible = true;
                    Expenses.Visible = true;
                }
            }
        }

        protected void btn_lgout_Click(object sender, EventArgs e)
        {
            dbcl.WriteToFile("User :" + lbl_loginusername1.Text.ToString() + " Singout Successfully");
            //Update loginstatus and Last Login Information i.e. date
            dbcl.UPDT_EmpMuster_LogoutInfo(Session["WORKMAN"].ToString(), Session["USERID"].ToString());
            Session.Abandon();
            Response.Redirect("~/login.aspx", false);
        }


        //-------------- below code is added on 03-08-2024------------//

        private void LoadUserPermissions(int userId)
        {
            string connectionString = "YourConnectionString";
            string query = @"
            SELECT ep.ParentKey, ep.ChildKey, ep.IsVisible
            FROM UserGroupMapping ugm
            JOIN GroupRoleMapping grm ON ugm.GroupID = grm.GroupID
            JOIN tlb_EmployeePermissions ep ON grm.RoleID = ep.RoleID
            WHERE ugm.UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string parentKey = reader["ParentKey"].ToString();
                    string childKey = reader["ChildKey"].ToString();
                    bool isVisible = Convert.ToBoolean(reader["IsVisible"]);

                    Control parentControl = FindControlRecursive(this, parentKey);
                    if (parentControl != null)
                    {
                        Control childControl = FindControlRecursive(parentControl, childKey);
                        if (childControl != null)
                        {
                            childControl.Visible = isVisible;
                        }
                    }
                }

                reader.Close();
            }
        }

        private Control FindControlRecursive_0(Control root, string id)
        {
            //if (root.ID == id)
            //{
            //    return root;
            //}

            //foreach (Control c in root.Controls)
            //{
            //    Control t = FindControlRecursive(c, id);
            //    if (t != null)
            //    {
            //        return t;
            //    }
            //}

            return null;
        }


        public void LoadPermissions_0(string empTypeValue, MasterPage masterPage)
        {
            //DataTable permissionsTable = GetPermissions(empTypeValue);

            //foreach (DataRow row in permissionsTable.Rows)
            //{
            //    string parentKey = row["ParentKey"].ToString();
            //    string childKey = row["ChildKey"].ToString();
            //    bool isVisible = Convert.ToBoolean(row["IsVisible"]);

            //    Control parentControl = masterPage.FindControl(parentKey);
            //    if (parentControl != null)
            //    {
            //        if (!string.IsNullOrEmpty(childKey))
            //        {
            //            Control childControl = parentControl.FindControl(childKey);
            //            if (childControl != null)
            //            {
            //                childControl.Visible = isVisible;
            //            }
            //        }
            //        else
            //        {
            //            parentControl.Visible = isVisible;
            //        }
            //    }
            //}
        }

        private DataTable LoadPermissions(string roleId)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT ParentKey, ChildKey, IsVisible FROM tlb_EmployeePermissions WHERE Emp_PermissionValue = @Emp_PermissionValue", dbcl.Conn);
            sqlDa.SelectCommand.Parameters.AddWithValue("@Emp_PermissionValue", roleId);
            sqlDa.SelectCommand.CommandType = CommandType.Text;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            dbcl.Conn.Close();

            return dtbl;
        }



        public DataTable GetPermissions_0(string empTypeValue)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();

            string query = "SELECT [Id], [Emp_PermissionValue], [ParentKey], [ChildKey], [IsVisible] " +
                           "FROM [tlb_EmployeePermissions] " +
                           "WHERE [Emp_PermissionValue] = @EmpTypeValue";

            SqlDataAdapter sqlDa = new SqlDataAdapter(query, dbcl.Conn);
            sqlDa.SelectCommand.Parameters.AddWithValue("@EmpTypeValue", empTypeValue);
            sqlDa.SelectCommand.CommandType = CommandType.Text;

            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            dbcl.Conn.Close();

            return dtbl;
        }

        //private void ApplyPermissions(DataTable permissions)
        //{
        //    foreach (DataRow row in permissions.Rows)
        //    {
        //        string parentKey = row["ParentKey"].ToString();
        //        bool isVisible = Convert.ToBoolean(row["IsVisible"]);

        //        switch (parentKey)
        //        {
        //            case "HomePage":
        //                HomePage.Visible = isVisible;
        //                break;
        //            case "DataMastering":
        //                DataMastering.Visible = isVisible;
        //                break;
        //            case "JOBManpower":
        //                JOBManpower.Visible = isVisible;
        //                break;
        //            case "Memo_Billing":
        //                Memo_Billing.Visible = isVisible;
        //                break;
        //            case "CSM":
        //                CSM.Visible = isVisible;
        //                break;
        //            case "loans_adeductions":
        //                loans_adeductions.Visible = isVisible;
        //                break;
        //            case "leaves_attendance":
        //                leaves_attendance.Visible = isVisible;
        //                break;
        //            case "Payroll":
        //                Payroll.Visible = isVisible;
        //                break;
        //            case "Expenses":
        //                Expenses.Visible = isVisible;
        //                break;
        //            case "Helpdesk":
        //                //Helpdesk.Visible = isVisible;
        //                //break;
        //                try
        //                {
        //                    Helpdesk.Visible = isVisible;
        //                }
        //                catch (Exception ex)
        //                {
        //                    throw new Exception("Error setting visibility for Memo_Billing", ex);
        //                }
        //                break;
        //            case "Analytics":
        //                Analytics.Visible = isVisible;
        //                break;
        //        }

        //        // Control visibility of child items
        //        //if (!string.IsNullOrEmpty(row["ChildKey"].ToString()))
        //        //{
        //        //    Control parentControl = FindControlRecursive(this, parentKey) as HtmlGenericControl;
        //        //    if (parentControl != null)
        //        //    {
        //        //        Control childControl = parentControl.FindControl(row["ChildKey"].ToString()) as HtmlGenericControl;
        //        //        if (childControl != null)
        //        //        {
        //        //            childControl.Visible = isVisible;
        //        //        }
        //        //    }
        //        //}

        //        // Control visibility of child items
        //        if (!string.IsNullOrEmpty(row["ChildKey"].ToString()))
        //        {
        //            string childKey = row["ChildKey"].ToString();

        //            // Finding the parent control first
        //            Control parentControl = FindControlRecursive(this, parentKey);

        //            if (parentControl != null)
        //            {
        //                // Finding the specific child control inside the parent
        //                Control childControl = parentControl.FindControl(childKey);
        //                if (childControl != null)
        //                {
        //                    try
        //                    {
        //                        // Hiding or showing the <li> element (not the <a> inside it)
        //                        childControl.Visible = isVisible;
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        throw new Exception($"Failed to set visibility on control '{childKey}' under parent '{parentKey}'.", ex);
        //                    }
        //                }
        //            }
        //        }



        //    }
        //}

        //private Control FindControlRecursive(Control root, string id)
        //{
        //    if (root == null || string.IsNullOrEmpty(id))
        //        return null;

        //    // Match ID
        //    if (string.Equals(root.ID, id, StringComparison.OrdinalIgnoreCase))
        //        return root;

        //    // Recurse through children
        //    foreach (Control child in root.Controls)
        //    {
        //        Control found = FindControlRecursive(child, id);
        //        if (found != null)
        //            return found;
        //    }

        //    return null;
        //}

        //private Control FindControlRecursive(Control root, string id)
        //{
        //    if (root.ID == id)
        //    {
        //        return root;
        //    }

        //    foreach (Control child in root.Controls)
        //    {
        //        Control foundControl = FindControlRecursive(child, id);
        //        if (foundControl != null)
        //        {
        //            return foundControl;
        //        }
        //    }

        //    return null;
        //}



        //-----------------Added on 08-05-2025------------------------//
        private void ApplyPermissions_OLD(DataTable permissions)
        {
            foreach (DataRow row in permissions.Rows)
            {
                string parentKey = row["ParentKey"].ToString();
                bool isVisible = Convert.ToBoolean(row["IsVisible"]);

                switch (parentKey)
                {
                    case "HomePage":
                        HomePage.Visible = isVisible;
                        break;
                    case "DataMastering":
                        DataMastering.Visible = isVisible;
                        break;
                    case "JOBManpower":
                        JOBManpower.Visible = isVisible;
                        break;
                    case "Memo_Billing":
                        Memo_Billing.Visible = isVisible;
                        break;
                    case "CSM":
                        CSM.Visible = isVisible;
                        break;
                    case "loans_adeductions":
                        loans_adeductions.Visible = isVisible;
                        break;
                    case "leaves_attendance":
                        leaves_attendance.Visible = isVisible;
                        break;
                    case "Payroll":
                        Payroll.Visible = isVisible;
                        break;
                    case "Expenses":
                        Expenses.Visible = isVisible;
                        break;
                    case "Helpdesk":
                        Helpdesk.Visible = isVisible;
                        // Handle visibility of child elements under Helpdesk
                        //SetChildVisibility("Helpdesk", isVisible);
                        break;
                    case "Analytics":
                        Analytics.Visible = isVisible;
                        break;
                }

                // Control visibility of child items
                if (!string.IsNullOrEmpty(row["ChildKey"].ToString()))
                {
                    string childKey = row["ChildKey"].ToString();

                    // Finding the parent control first
                    Control parentControl = FindControlRecursive(this, parentKey);

                    if (parentControl != null)
                    {
                        // Finding the specific child control inside the parent
                        Control childControl = parentControl.FindControl(childKey);
                        if (childControl != null)
                        {
                            try
                            {
                                // Hiding or showing the <li> element (not the <a> inside it)
                                childControl.Visible = isVisible;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to set visibility on control '{childKey}' under parent '{parentKey}'.", ex);
                            }
                        }
                    }
                }
            }
        }

        private void ApplyPermissions(DataTable permissions)
        {
            // To store visibility status of parents
            Dictionary<string, bool> parentVisibility = new Dictionary<string, bool>();

            // Iterate over all rows in the permissions DataTable
            foreach (DataRow row in permissions.Rows)
            {
                string parentKey = row["ParentKey"].ToString();
                bool isParentVisible = false; // Default is parent is not visible
                bool isChildVisible = Convert.ToBoolean(row["IsVisible"]);
                string childKey = row["ChildKey"].ToString();

                // Step 1: Check if the child is visible
                if (isChildVisible)
                {
                    // If the child is visible, make the parent visible
                    if (!parentVisibility.ContainsKey(parentKey))
                    {
                        // If parent visibility is not set yet, set it to true because at least one child is visible
                        parentVisibility[parentKey] = true;
                    }
                }

                // Step 2: Handle Parent Visibility (if not already set by child)
                if (parentVisibility.ContainsKey(parentKey))
                {
                    // Set parent visibility based on the parent's stored visibility value
                    switch (parentKey)
                    {
                        case "HomePage":
                            HomePage.Visible = parentVisibility[parentKey];
                            break;
                        case "DataMastering":
                            DataMastering.Visible = parentVisibility[parentKey];
                            break;
                        case "JOBManpower":
                            JOBManpower.Visible = parentVisibility[parentKey];
                            break;
                        case "Memo_Billing":
                            Memo_Billing.Visible = parentVisibility[parentKey];
                            break;
                        case "CSM":
                            CSM.Visible = parentVisibility[parentKey];
                            break;
                        case "loans_adeductions":
                            loans_adeductions.Visible = parentVisibility[parentKey];
                            break;
                        case "leaves_attendance":
                            leaves_attendance.Visible = parentVisibility[parentKey];
                            break;
                        case "Payroll":
                            Payroll.Visible = parentVisibility[parentKey];
                            break;
                        case "Expenses":
                            Expenses.Visible = parentVisibility[parentKey];
                            break;
                        case "Helpdesk":
                            Helpdesk.Visible = parentVisibility[parentKey];
                            break;
                        case "Analytics":
                            Analytics.Visible = parentVisibility[parentKey];
                            break;
                    }
                }

                // Step 3: Handle Child Visibility
                if (!string.IsNullOrEmpty(childKey))
                {
                    // Find the parent control and set the child visibility
                    Control parentControl = FindControlRecursive(this, parentKey);
                    if (parentControl != null)
                    {
                        Control childControl = parentControl.FindControl(childKey);
                        if (childControl != null)
                        {
                            try
                            {
                                // Set visibility of child control
                                childControl.Visible = isChildVisible;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to set visibility on child control '{childKey}' under parent '{parentKey}'.", ex);
                            }
                        }
                    }
                }
            }
        }



        private void SetChildVisibility(string parentKey, bool isVisible)
        {
            // You can add a switch case or logic to handle child items specifically for Helpdesk
            switch (parentKey)
            {
                case "Helpdesk":
                    // Set visibility for each child element of Helpdesk
                    SetControlVisibility("hlpdsk_new", isVisible);
                    SetControlVisibility("hlpdsk_view", isVisible);
                    break;
                    // You can handle other cases similarly for other menus if needed
            }
        }

        private void SetControlVisibility(string controlId, bool isVisible)
        {
            Control control = FindControlRecursive(this, controlId);
            if (control != null)
            {
                try
                {
                    control.Visible = isVisible;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to set visibility for control '{controlId}'.", ex);
                }
            }
        }

        private Control FindControlRecursive(Control root, string id)
        {
            if (root == null || string.IsNullOrEmpty(id))
                return null;

            // Match ID
            if (string.Equals(root.ID, id, StringComparison.OrdinalIgnoreCase))
                return root;

            // Recurse through children
            foreach (Control child in root.Controls)
            {
                Control found = FindControlRecursive(child, id);
                if (found != null)
                    return found;
            }

            return null;
        }

    }

}