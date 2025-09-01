using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.bussiness.production
{
    public class PurchaseDAL : BaseDAL
    {
        public PurchaseDAL(string connectionString) : base(connectionString) { }

        public int CreatePurchaseRequest(int materialId, int quantity, int createdBy) =>
        ExecuteNonQuery("sp_PurchaseRequest_Create", new[]
        {
            new SqlParameter("@MaterialID", materialId),
            new SqlParameter("@Quantity", quantity),
            new SqlParameter("@CreatedBy", createdBy)
        });

        public int CreatePO(int prId, int vendorId, int createdBy) =>
            ExecuteNonQuery("sp_PO_Create", new[]
            {
            new SqlParameter("@PRID", prId),
            new SqlParameter("@VendorID", vendorId),
            new SqlParameter("@CreatedBy", createdBy)
            });

        public int CreateGRN(int poId, int receivedBy) =>
            ExecuteNonQuery("sp_GRN_Create", new[]
            {
            new SqlParameter("@POID", poId),
            new SqlParameter("@ReceivedBy", receivedBy)
            });
    }
}