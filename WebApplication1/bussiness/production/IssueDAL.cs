using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.bussiness.production
{
    public class IssueDAL : BaseDAL
    {
        public IssueDAL(string connectionString) : base(connectionString) { }

        public int CreateIssue(int materialId, int qty, int issuedBy) =>
            ExecuteNonQuery("sp_Issue_Create", new[]
            {
            new SqlParameter("@MaterialID", materialId),
            new SqlParameter("@Quantity", qty),
            new SqlParameter("@IssuedBy", issuedBy)
            });

        public int CreateReturn(int issueId, int qty, int returnedBy) =>
            ExecuteNonQuery("sp_Return_Create", new[]
            {
            new SqlParameter("@IssueID", issueId),
            new SqlParameter("@Quantity", qty),
            new SqlParameter("@ReturnedBy", returnedBy)
            });
    }
}