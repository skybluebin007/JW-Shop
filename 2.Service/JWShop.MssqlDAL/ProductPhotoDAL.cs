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
    public sealed class ProductPhotoDAL : IProductPhoto
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(ProductPhotoInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO ProductPhoto( ProductId,Name,ImageUrl,ProStyle,AddDate,[OrderID]) VALUES(@ProductId,@Name,@ImageUrl,@ProStyle,GetDate(),@OrderId);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(ProductPhotoInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE ProductPhoto SET ProductId = @ProductId, Name = @Name, ImageUrl = @ImageUrl
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id, int proStyle)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductPhoto where id=@id and proStyle=@proStyle";

                conn.Execute(sql, new { id = id, proStyle = proStyle });
            }
        }

        public void DeleteList(int productId, int proStyle)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductPhoto where productId=@productId and proStyle=@proStyle";

                conn.Execute(sql, new { productId = productId, proStyle = proStyle });
            }
        }

        public ProductPhotoInfo Read(int id, int proStyle)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductPhoto where id=@id and proStyle=@proStyle";

                var data = conn.Query<ProductPhotoInfo>(sql, new { id = id, proStyle = proStyle }).SingleOrDefault();
                return data ?? new ProductPhotoInfo();
            }
        }

        public List<ProductPhotoInfo> ReadList(int productId, int proStyle)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductPhoto where ProductId=@productId and proStyle=@proStyle order by [OrderID],[ID] ";

                return conn.Query<ProductPhotoInfo>(sql, new { productId = productId, proStyle = proStyle }).ToList();
            }
        }
        /// <summary>
        /// 获取所有productphoto
        /// </summary>
        /// <returns></returns>
        public List<ProductPhotoInfo> ReadAllProductPhotos()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductPhoto where proStyle=0";

                return conn.Query<ProductPhotoInfo>(sql).ToList();
            }
        }
        /// <summary>
        /// 获取当前商品图集的最大排序号
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns></returns>
        public int GetMaxOrderId(int productId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"select  isnull(max(orderid),0) as maxorderid from [ProductPhoto] where productid=@productId";

                return conn.Query<int>(sql, new { productId = productId }).Single();
            }
        }
        /// <summary>
        /// 上移图片
        /// </summary>
        /// <param name="id">要移动的id</param>
        public void MoveUpProductPhoto(int id)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@id",SqlDbType.Int)
			};
            parameters[0].Value = id;
            ShopMssqlHelper.ExecuteNonQuery("MoveUpProductPhoto", parameters);
        }


        /// <summary>
        /// 下移图片
        /// </summary>
        /// <param name="id">要移动的id</param>
        public void MoveDownProductPhoto(int id)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@id",SqlDbType.Int)
			};
            parameters[0].Value = id;
            ShopMssqlHelper.ExecuteNonQuery("MoveDownProductPhoto", parameters);
        }
    }
}