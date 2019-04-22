using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Configuration;
using Dapper;
using System.Linq;

namespace JWShop.MssqlDAL
{
    /// <summary>
    /// 文章分类数据层说明。
    /// </summary>
    public sealed class ArticleClassDAL : IArticleClass
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(ArticleClassInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO ArticleClass( ParentId,OrderId,Name,EnName,Description,Photo,ShowType,ShowTerminal,IsSystem,ImageWidth,ImageHeight,AddCol1,AddCol2) VALUES(@ParentId,@OrderId,@Name,@EnName,@Description,@Photo,@ShowType,@ShowTerminal,@IsSystem,@ImageWidth,@ImageHeight,@AddCol1,@AddCol2);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(ArticleClassInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE ArticleClass SET ParentId = @ParentId, OrderId = @OrderId, Name = @Name, EnName = @EnName, Description = @Description, Photo = @Photo, ShowType = @ShowType, ShowTerminal = @ShowTerminal, IsSystem = @IsSystem,ImageWidth=@ImageWidth,ImageHeight=@ImageHeight, AddCol1 = @AddCol1, AddCol2 = @AddCol2
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                conn.Execute("usp_ArticleClass", new { type = "delete", id = id }, commandType: CommandType.StoredProcedure);
            }
        }

        public ArticleClassInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ArticleClass where id=@id";

                var data = conn.Query<ArticleClassInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new ArticleClassInfo();
            }
        }
        public void ChangeArticleClassOrder(int id,int orderId) {
            if (id > 0 && orderId >= 0)
            {
                ArticleClassInfo tempProClass = Read(id);
                tempProClass.OrderId = orderId;
                Update(tempProClass);
            }
        }
        public List<ArticleClassInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "SELECT * FROM ArticleClass ORDER BY [OrderId] ASC,Id ASC";

                return conn.Query<ArticleClassInfo>(sql).ToList();
            }
        }

        public void Move(int id, ChangeAction action)
        {
            using (var conn = new SqlConnection(connectString))
            {
                conn.Query("usp_ArticleClass", new { type = action.ToString().ToLower(), id = id }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}