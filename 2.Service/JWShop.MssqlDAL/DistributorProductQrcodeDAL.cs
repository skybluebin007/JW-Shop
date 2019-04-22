using System;
using System.Linq;
using JWShop.Entity;
using JWShop.IDAL;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace JWShop.MssqlDAL
{
    /// <summary>
    /// 分销商商品推广码 数据层
    /// </summary>
    public sealed class DistributorProductQrcodeDAL : IDistributorProductQrcode
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];
        public bool Add(DistributorProductQrcodeInfo entity)
        {
            //判断记录是否存在
            if (Read(entity.Distributor_Id, entity.Product_Id) != null)
            {
                return false;
            }
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO [Distributor_Product_Qrcode]([Product_Id],[Distributor_Id],[Qrcode]) VALUES(@Product_Id,@Distributor_Id,@Qrcode)";

                return conn.Execute(sql, entity)>0;
            }
        }

        public bool Delete(int distributor_Id, int product_Id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"DELETE * FROM [Distributor_Product_Qrcode] WHERE [Product_Id]=@Product_Id AND [Distributor_Id]=@Distributor_Id";

                return conn.Execute(sql, new { @Product_Id = product_Id, @Distributor_Id = distributor_Id })>0;
            }
        }

        public DistributorProductQrcodeInfo Read(int distributor_Id, int product_Id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"SELECT * FROM [Distributor_Product_Qrcode] WHERE [Product_Id]=@Product_Id AND [Distributor_Id]=@Distributor_Id";

                return conn.Query<DistributorProductQrcodeInfo>(sql, new { @Product_Id = product_Id, @Distributor_Id = distributor_Id }).SingleOrDefault();
            }
        }
        /// <summary>
        /// 更新二维码
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(DistributorProductQrcodeInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE [Distributor_Product_Qrcode] SET [Qrcode]=@Qrcode WHERE [Product_Id]=@Product_Id AND [Distributor_Id]=@Distributor_Id";

                return conn.Execute(sql, entity) > 0;
            }
        }
    }
}
