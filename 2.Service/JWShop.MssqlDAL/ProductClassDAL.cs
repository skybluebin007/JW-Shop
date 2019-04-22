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
    public sealed class ProductClassDAL : IProductClass
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(ProductClassInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"insert into ProductClass(ParentId,ProductTypeId,Name,Number,ProductCount,Keywords,Remark,OrderId,Tm,Photo,EnClassName,PageTitle,PageKeyWord,PageSummary,IsSystem)
                            values(@ParentId,@ProductTypeId,@Name,@Number,@ProductCount,@Keywords,@Remark,@OrderId,@Tm,@Photo,@EnClassName,@PageTitle,@PageKeyWord,@PageSummary,@IsSystem);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(ProductClassInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"update ProductClass set ParentId = @ParentId, ProductTypeId = @ProductTypeId, Name = @Name, Number = @Number, ProductCount = @ProductCount, Keywords = @Keywords, Remark = @Remark, OrderId = @OrderId, Tm = @Tm,Photo=@Photo,EnClassName=@EnClassName,PageTitle=@PageTitle,PageKeyWord=@PageKeyWord,PageSummary=@PageSummary,IsSystem=@IsSystem
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductClass where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public ProductClassInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductClass where id=@id";

                var data = conn.Query<ProductClassInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new ProductClassInfo();
            }
        }

        public List<ProductClassInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductClass order by OrderId";

                return conn.Query<ProductClassInfo>(sql).ToList();
            }
        }

        public void Move(int id, ChangeAction action)
        {
            using (var conn = new SqlConnection(connectString))
            {
                conn.Query("usp_ProductClass", new { type = action.ToString().ToLower(), id = id }, commandType: CommandType.StoredProcedure);
            }
        }

        public int GetProductClassType(int productClassID)
        {
            SqlParameter[] parameters = { new SqlParameter("@productClassID", productClassID) };
            Object id = ShopMssqlHelper.ExecuteScalar("GetProductClassType", parameters);
            return (Convert.ToInt32(id));
        }
        /// <summary>
        /// 修改分类排序
        /// </summary>
        /// <param name="id">要移动的id</param>
        public void ChangeProductClassOrder(int pid, int oid)
        {
            ProductClassInfo tempProClass = Read(pid);
            tempProClass.OrderId = oid;

            Update(tempProClass);
        }

        public void ChangeProductCount(int[] classIds, ChangeAction action)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = action == ChangeAction.Plus
                        ? "update ProductClass set ProductCount = ProductCount + 1 where Id in @ids"
                        : "update ProductClass set ProductCount = ProductCount - 1 where Id in @ids and ProductCount > 0";

                conn.Execute(sql, new { ids = classIds });
            }
        }
    }
}