using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;

namespace WebApplication1.bussiness.production
{
    public class ApprovalDAL :BaseDAL
    {
        public ApprovalDAL(string connectionString) : base(connectionString) { }

        public DataTable GetNextApprover(int referenceId, string module, string action) =>
            ExecuteDataTable("sp_GetNextApprover", new[]
            {
            new SqlParameter("@ReferenceID", referenceId),
            new SqlParameter("@Module", module),
            new SqlParameter("@ActionName", action)
            });

        public int ExecuteApproval(int referenceId, string module, string action, int approverId, string remarks) =>
            ExecuteNonQuery("sp_Approval_Execute", new[]
            {
            new SqlParameter("@ReferenceID", referenceId),
            new SqlParameter("@Module", module),
            new SqlParameter("@ActionName", action),
            new SqlParameter("@ApprovedByUserID", approverId),
            new SqlParameter("@Remarks", remarks)
            });
    }
}