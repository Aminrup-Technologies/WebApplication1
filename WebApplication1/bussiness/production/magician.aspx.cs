using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.bussiness.production
{
    public partial class magician : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, bool> permissionDict = GenerateSamplePermissions();
            SetChildPageContentVisibility(permissionDict);
        }

        private Dictionary<string, bool> GenerateSamplePermissions()
        {
            // Sample permissions for testing
            Dictionary<string, bool> permissionDict = new Dictionary<string, bool>
            {
                { "div_roles", true },
                { "div_rolepermissions", true },
                { "div_accesspermissions", true }
            };

            return permissionDict;
        }

        private void SetChildPageContentVisibility(Dictionary<string, bool> permissionDict)
        {
            foreach (var permission in permissionDict)
            {
                Control control = FindControlRecursive(this.Page, permission.Key);
                if (control != null)
                {
                    control.Visible = permission.Value;
                }
            }
        }

        private Control FindControlRecursive(Control root, string id)
        {
            if (root.ID == id)
            {
                return root;
            }

            foreach (Control child in root.Controls)
            {
                Control foundControl = FindControlRecursive(child, id);
                if (foundControl != null)
                {
                    return foundControl;
                }
            }

            return null;
        }
    }
}