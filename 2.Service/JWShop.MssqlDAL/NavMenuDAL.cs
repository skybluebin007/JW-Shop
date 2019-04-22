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
    public sealed class NavMenuDAL : INavMenu
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(NavMenuInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"insert into NavMenu([Name],[IsShow],[OrderId],[LinkUrl],[Introduce])
                            values(@Name,@IsShow,@OrderId,@LinkUrl,@Introduce);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(NavMenuInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"update NavMenu set Name=@Name,IsShow=@IsShow,OrderId=@OrderId,LinkUrl=@LinkUrl,Introduce=@Introduce  where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from NavMenu where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public NavMenuInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from NavMenu where id=@id";

                var data = conn.Query<NavMenuInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new NavMenuInfo();
            }
        }
        /// <summary>
        /// 修改导航排序
        /// </summary>
        /// <param name="id">要移动的id</param>
        public void ChangeNavMenuOrder(int id, int orderId)
        {        
            NavMenuInfo navmenu = Read(id);
            navmenu.OrderId = orderId;
            Update(navmenu);
        }
        public List<NavMenuInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from NavMenu order by OrderId Asc,Id Asc";

                return conn.Query<NavMenuInfo>(sql).ToList();
            }
        }

        public List<NavMenuInfo> ReadList(bool isShow)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from NavMenu where isshow=" + Convert.ToInt32(isShow) + " order by OrderId Asc,Id Asc";

                return conn.Query<NavMenuInfo>(sql).ToList();
            }
        }

        public void Move(int id, ChangeAction action)
        {
            using (var conn = new SqlConnection(connectString))
            {
                conn.Query("usp_NavMenu", new { type = action.ToString().ToLower(), id = id }, commandType: CommandType.StoredProcedure);
            }
        }
       
       
        
    }
}