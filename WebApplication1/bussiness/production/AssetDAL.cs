using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.bussiness.production
{
    public class AssetDAL :BaseDAL
    {
        public AssetDAL(string connectionString) : base(connectionString) { }

        public int UpsertAssetExtended(int assetId, string serial, string remarks) =>
            ExecuteNonQuery("sp_AssetExtended_Upsert", new[]
            {
            new SqlParameter("@AssetID", assetId),
            new SqlParameter("@SerialNumber", serial),
            new SqlParameter("@Remarks", remarks)
            });

        public int ExecuteAssetActionRequest(int assetId, string actionName, int requestedBy) =>
            ExecuteNonQuery("sp_AssetActionRequest_Execute", new[]
            {
            new SqlParameter("@AssetID", assetId),
            new SqlParameter("@ActionName", actionName),
            new SqlParameter("@RequestedBy", requestedBy)
            });
    }
}