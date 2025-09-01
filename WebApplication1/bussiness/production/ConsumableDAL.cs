using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.bussiness.production
{
    public class ConsumableDAL :BaseDAL
    {
        public ConsumableDAL(string connectionString) : base(connectionString) { }

        public int CreateConsumableRequest(int materialId, int qty, int requestedBy) =>
            ExecuteNonQuery("sp_ConsumableRequest_Create", new[]
            {
            new SqlParameter("@MaterialID", materialId),
            new SqlParameter("@Quantity", qty),
            new SqlParameter("@RequestedBy", requestedBy)
            });

        public int ExecuteConsumableIssue(int requestId, int issuedBy) =>
            ExecuteNonQuery("sp_ConsumableIssue_Execute", new[]
            {
            new SqlParameter("@RequestID", requestId),
            new SqlParameter("@IssuedBy", issuedBy)
            });
    }
}