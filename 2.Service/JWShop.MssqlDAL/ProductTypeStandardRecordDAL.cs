using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using Dapper;
using System.Linq;

namespace JWShop.MssqlDAL
{
    public sealed class ProductTypeStandardRecordDAL : IProductTypeStandardRecord
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public void Add(ProductTypeStandardRecordInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO ProductTypeStandardRecord( ProductId,StandardIdList,ValueList,MarketPrice,SalePrice,Storage,OrderCount,SendCount,ProductCode,GroupTag,Photo,[GroupPrice]) VALUES(@ProductId,@StandardIdList,@ValueList,@MarketPrice,@SalePrice,@Storage,@OrderCount,@SendCount,@ProductCode,@GroupTag,@Photo,@GroupPrice)";

                conn.Execute(sql, entity);
            }
        }

        public void Update(ProductTypeStandardRecordInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE ProductTypeStandardRecord SET ProductId = @ProductId, StandardIdList = @StandardIdList, ValueList = @ValueList, MarketPrice = @MarketPrice, SalePrice = @SalePrice, Storage = @Storage, OrderCount=@OrderCount, SendCount=@SendCount, ProductCode = @ProductCode,GroupTag=@GroupTag,Photo=@Photo,[GroupPrice]=@GroupPrice where Id=@Id";

                conn.Execute(sql, entity);
            }
        }
        /// <summary>
        /// 修改规格价格
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateSalePrice(ProductTypeStandardRecordInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE ProductTypeStandardRecord SET  SalePrice = @SalePrice  where ProductId = @ProductId and ValueList = @ValueList";

                conn.Execute(sql, entity);
            }
        }
        /// <summary>
        /// 修改规格库存
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateStorage(ProductTypeStandardRecordInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE ProductTypeStandardRecord SET  Storage = @Storage  where ProductId = @ProductId and ValueList = @ValueList";

                conn.Execute(sql, entity);
            }
        }
        /// <summary>
        /// 获取规格总库存
        /// </summary>
        /// <param name="entity"></param>
        public int GetSumStorageByProduct(int productId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"select sum([Storage]) as sumstorage from [ProductTypeStandardRecord] group by [ProductId] having productid=@productId";

                return conn.ExecuteScalar<int>(sql, new { productId=productId});
            }
        }
        public void Delete(int productId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductTypeStandardRecord where productId=@productId";

                conn.Execute(sql, new { productId = productId });
            }
        }

        public void DeleteByStandardId(int standardId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductTypeStandardRecord where CHARINDEX(';"+ standardId + ";',';'+StandardIdList+';',0)>0";

                conn.Execute(sql);
            }
        }

        public ProductTypeStandardRecordInfo Read(int productId, string valueList)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select top 1 * from ProductTypeStandardRecord where productId=@productId and ValueList=@valueList";

                var data = conn.Query<ProductTypeStandardRecordInfo>(sql, new { productId = productId, valueList = valueList }).FirstOrDefault();
                return data ?? new ProductTypeStandardRecordInfo();
            }
        }

        public List<ProductTypeStandardRecordInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductTypeStandardRecord";

                return conn.Query<ProductTypeStandardRecordInfo>(sql).ToList();
            }
        }

        public List<ProductTypeStandardRecordInfo> ReadList(int productId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductTypeStandardRecord where ProductId=@productId";

                return conn.Query<ProductTypeStandardRecordInfo>(sql, new { productId = productId }).ToList();
            }
        }

        /// <summary>
        /// 按分类ID获得规格记录所有数据
        /// </summary>
        /// <param name="productID">分类ID</param>
        /// <returns>规格记录数据列表</returns>
        public List<ProductTypeStandardRecordInfo> ReadListByProduct(int productID, int standardType)
        {
            using (var conn = new SqlConnection(connectString))
            {
                return conn.Query<ProductTypeStandardRecordInfo>(ShopMssqlHelper.TablePrefix + "ReadStandardRecordByProduct", new
                {
                    productID = productID,
                    standardType = standardType
                }, null, false, null, CommandType.StoredProcedure).ToList();            
            }
        }
        /// <summary>
        /// 按产品删除规格记录数据
        /// </summary>
        /// <param name="strProductID">产品ID,以,号分隔</param>
        public void DeleteByProductID(string strProductID)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@strProductID",SqlDbType.NVarChar)
			};
            parameters[0].Value = strProductID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteStandardRecordByProductID", parameters);
        }

        public void ChangeOrderCount(int productId, string valueList, int buyCount, ChangeAction action)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE ProductTypeStandardRecord SET OrderCount = OrderCount + @buyCount
                            where productId=@productId and valueList = @valueList";

                if (action == ChangeAction.Minus)
                {
                    sql = @"UPDATE ProductTypeStandardRecord SET OrderCount = OrderCount-@buyCount
                            where productId=@productId and valueList = @valueList and OrderCount > 0";
                }

                conn.Execute(sql, new { productId = productId, valueList = valueList, buyCount = buyCount });
            }
        }

        public void ChangeSendCount(int productId, string valueList, int buyCount, ChangeAction action)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE ProductTypeStandardRecord SET SendCount = isnull(SendCount,0)+@buyCount
                            where productId=@productId and valueList = @valueList";

                if (action == ChangeAction.Minus)
                {
                    sql = @"UPDATE ProductTypeStandardRecord SET SendCount = SendCount-@buyCount
                            where productId=@productId and valueList = @valueList and SendCount > 0";
                }

                conn.Execute(sql, new { productId = productId, valueList = valueList, buyCount = buyCount });
            }
        }
    }
}