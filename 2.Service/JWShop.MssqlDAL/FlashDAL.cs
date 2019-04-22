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
    /// 广告位数据层说明。
    /// </summary>
    public sealed class FlashDAL : IFlash
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];
        public int Add(FlashInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO [Flash]([Title],[Introduce],[Width],[Height],[EndDate],[AddCol1],[AddCol2],[AddCol3]) VALUES(@Title,@Introduce,@Width,@Height,@EndDate,@AddCol1,@AddCol2,@AddCol3);
                            select SCOPE_IDENTITY()";
                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(FlashInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE [Flash] SET Title = @Title,[Introduce]=@Introduce,[Width]=@Width,[Height]=@Height,[EndDate]=@EndDate,[AddCol1]=@AddCol1,[AddCol2]=@AddCol2,[AddCol3]=@AddCol3  where Id=@Id";
                conn.Execute(sql, entity);
            }
        }
        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from [flash] where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }
        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from [flash] where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }
        public FlashInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [flash] where id=@id";

                var data = conn.Query<FlashInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new FlashInfo();
            }
        }

        public List<FlashInfo> SearchList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [Flash]";

                return conn.Query<FlashInfo>(sql).ToList();
            }
        }

        public List<FlashInfo> SearchList(int currentPage, int pageSize,ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "Flash";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Asc;
                pc.MssqlCondition.Add(" [Id]>10 ");
                count = pc.Count;
                return conn.Query<FlashInfo>(pc).ToList();
            }
        }


    }
}
