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
    public sealed class CartDAL : ICart
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(CartInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO Cart( ProductId,ProductName,StandardValueList,BuyCount,RandNumber,UserId,UserName) VALUES(@ProductId,@ProductName,@StandardValueList,@BuyCount,@RandNumber,@UserId,@UserName);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(int[] ids, int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE Cart SET BuyCount = @count where Id in @ids";

                conn.Execute(sql, new { count = count, ids = ids });
            }
        }

        public void Delete(int[] ids, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from Cart where id in @ids and userId=@userId";

                conn.Execute(sql, new { ids = ids, userId = userId });
            }
        }
        
        public void Clear(int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from Cart where userId=@userId";

                conn.Execute(sql, new { userId = userId });
            }
        }

        public List<CartInfo> ReadList(int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Cart where userId=@userId";

                return conn.Query<CartInfo>(sql, new { userId = userId }).ToList();
            }
        }

        public CartInfo Read(int productId, string productName, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Cart where productId=@productId and productName=@productName and userId=@userId";

                var data = conn.Query<CartInfo>(sql, new { productId = productId, productName = productName, userId = userId }).SingleOrDefault();
                return data ?? new CartInfo();
            }
        }

        public void UpdateCartNum(int cartId, int num,int uid)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "update  Cart Set BuyCount=@num Where Id=@cartId and UserId=@uid";

                conn.Execute(sql, new { num = num, cartId = cartId, uid= uid });
            }
        }
        /// <summary>
        /// 检查购物车是否存在该商品(普通商品购买需判断)
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="productName"></param>
        /// <param name="userID"></param>
        public bool IsProductInCart(int productID, string productName, int userID)
        {
            bool isIn = false;
            SqlParameter[] parameters ={
				new SqlParameter("@productID",SqlDbType.Int),
				new SqlParameter("@productName",SqlDbType.NVarChar),
                new SqlParameter("@userID",SqlDbType.Int)
			};
            parameters[0].Value = productID;
            parameters[1].Value = productName;
            parameters[2].Value = userID;
            object oj = ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "IsProductInCart", parameters);
            if (oj != null && oj != DBNull.Value)
            {
                if (Convert.ToUInt32(oj) > 0)
                {
                    isIn = true;
                }
            }
            return isIn;
        }
    }
}