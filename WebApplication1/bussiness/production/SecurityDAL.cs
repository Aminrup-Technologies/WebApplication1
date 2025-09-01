using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;

namespace WebApplication1.bussiness.production
{
    public class SecurityDAL :BaseDAL
    {
        public SecurityDAL(string connectionString) : base(connectionString) { }

        public bool SecurityCheck(int userId, string actionName)
        {
            var result = ExecuteScalar("sp_SecurityCheck", new[]
            {
            new SqlParameter("@UserID", userId),
            new SqlParameter("@ActionName", actionName)
        });

            return Convert.ToBoolean(result);
        }

        public DataTable GetUserPermissions(int userId) =>
            ExecuteDataTable("sp_GetUserPermissions", new[] { new SqlParameter("@UserID", userId) });

        public DataTable GetRolePermissions(int roleId) =>
            ExecuteDataTable("sp_GetRolePermissions", new[] { new SqlParameter("@RoleID", roleId) });

    }
}