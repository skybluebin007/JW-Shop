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
    public sealed class NavigationDAL : INavigation
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public void Add(NavigationInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"insert into Navigation(ParentId,NavigationType,Name,Remark,OrderId,ClassType,Url,ClassId,IsSingle,ShowType,IsVisible)
                            values(@ParentId,@NavigationType,@Name,@Remark,@OrderId,@ClassType,@Url,@ClassId,@IsSingle,@ShowType,@IsVisible)";

                conn.Execute(sql, entity);
            }
        }

        public void Update(NavigationInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"update Navigation set ParentId=@ParentId,NavigationType=@NavigationType,Name=@Name,Remark=@Remark,
                            OrderId=@OrderId,ClassType=@ClassType,Url=@Url,ClassId=@ClassId,IsSingle=@IsSingle,ShowType=@ShowType,IsVisible=@IsVisible
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from Navigation where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public NavigationInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Navigation where id=@id";

                var data = conn.Query<NavigationInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new NavigationInfo();
            }
        }

        public NavigationInfo Read(NavigationClassType classType, int classId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Navigation where classType=@classType and classId=@classId";

                var data = conn.Query<NavigationInfo>(sql, new { classType = (int)classType, classId = classId }).SingleOrDefault();
                return data ?? new NavigationInfo();
            }
        }

        public List<NavigationInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Navigation order by OrderId";

                return conn.Query<NavigationInfo>(sql).ToList();
            }
        }

        public List<NavigationInfo> ReadList(int navigationType)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Navigation where NavigationType=@navigationType order by OrderId";

                return conn.Query<NavigationInfo>(sql, new { navigationType = navigationType }).ToList();
            }
        }

        public List<NavigationInfo> ReadChildList(int classId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Navigation where ParentId =(select top 1 Id from Navigation where ClassId=@classId) order by OrderId";

                return conn.Query<NavigationInfo>(sql, new { classId = classId }).ToList();
            }
        }

    }
}