using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.bussiness.production
{
    public class TransferDAL :BaseDAL
    {
        public TransferDAL(string connectionString) : base(connectionString) { }

        public int CreateTransfer(int materialId, int fromLoc, int toLoc, int createdBy) =>
            ExecuteNonQuery("sp_TransferRequest_Create", new[]
            {
            new SqlParameter("@MaterialID", materialId),
            new SqlParameter("@FromLocationID", fromLoc),
            new SqlParameter("@ToLocationID", toLoc),
            new SqlParameter("@CreatedBy", createdBy)
            });
    }
}