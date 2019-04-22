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
    public sealed class ProductBrandDAL : IProductBrand
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(ProductBrandInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO ProductBrand( Name,Spelling,ImageUrl,LinkUrl,Remark,OrderId,IsTop) VALUES(@Name,@Spelling,@ImageUrl,@LinkUrl,@Remark,@OrderId,@IsTop);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(ProductBrandInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE ProductBrand SET Name = @Name,Spelling=@Spelling,ImageUrl = @ImageUrl, LinkUrl = @LinkUrl, Remark = @Remark, OrderId = @OrderId, IsTop = @IsTop
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ProductBrand where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public ProductBrandInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductBrand where id=@id";

                var data = conn.Query<ProductBrandInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new ProductBrandInfo();
            }
        }
        public ProductBrandInfo Read(string name)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductBrand where Name=@name";

                var data = conn.Query<ProductBrandInfo>(sql, new { name = name }).SingleOrDefault();
                return data ?? new ProductBrandInfo();
            }
        }
        public List<ProductBrandInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductBrand order by OrderId";

                return conn.Query<ProductBrandInfo>(sql).ToList();
            }
        }

        public List<ProductBrandInfo> ReadList(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductBrand where id in @ids order by OrderId";

                return conn.Query<ProductBrandInfo>(sql, new { ids = ids }).ToList();
            }
        }
        public List<ProductBrandInfo> SearchList(ProductBrandSearchInfo brandSearch)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ProductBrand";

                string condition = PrepareCondition(brandSearch).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }

                return conn.Query<ProductBrandInfo>(sql).ToList();
            }
        }
        public List<ProductBrandInfo> SearchList(int currentPage, int pageSize, ProductBrandSearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();             
                pc.TableName = "ProductBrand";
                pc.Fields = "[Id], Name,Spelling,ImageUrl,LinkUrl,Remark,OrderId,IsTop";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                //如果只用一个排序字段，分页存储过程则使用top方式。否则，使用row_number方式进行分页
                //pc.OrderField = "[OrderId]";
                pc.OrderField = "[OrderId],[Id]";         

               pc.OrderType =OrderType.Desc;
             
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<ProductBrandInfo>(pc).ToList();
            }
        }
        public MssqlCondition PrepareCondition(ProductBrandSearchInfo brandSearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            string condition = string.Empty;
            if (!string.IsNullOrEmpty(brandSearch.Key))
            {
                condition = "([Name]" + " LIKE '%" + StringHelper.SearchSafe(brandSearch.Key) + "%' OR ";
                condition += "[Spelling]" + " LIKE '%" + StringHelper.SearchSafe(brandSearch.Key) + "%')";            
                mssqlCondition.Add(condition);
            }
            mssqlCondition.Add("[Name]", brandSearch.Name, ConditionType.Like);
            mssqlCondition.Add("[Spelling]", brandSearch.Spelling, ConditionType.Like);
            mssqlCondition.Add("[IsTop]", brandSearch.IsTop, ConditionType.Equal);
            return mssqlCondition;
        }
        public void Move(int id, ChangeAction action)
        {
            using (var conn = new SqlConnection(connectString))
            {
                conn.Query("usp_ProductBrand", new { type = action.ToString().ToLower(), id = id }, null, true, null, CommandType.StoredProcedure);
            }
        }

    }
}