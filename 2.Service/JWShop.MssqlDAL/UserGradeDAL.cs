using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using Dapper;
using System.Configuration;
using System.Linq;
using System.Text;

namespace JWShop.MssqlDAL
{
    public sealed class UserGradeDAL : IUserGrade
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(UserGradeInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO UsrGrade( Name,MinMoney,MaxMoney,Discount) VALUES(@Name,@MinMoney,@MaxMoney,@Discount);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(UserGradeInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE UsrGrade SET Name = @Name, MinMoney = @MinMoney, MaxMoney = @MaxMoney, Discount = @Discount
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from UsrGrade where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public UserGradeInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from UsrGrade where Id=@id";

                var data = conn.Query<UserGradeInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new UserGradeInfo();
            }
        }

        public List<UserGradeInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from UsrGrade";

                return conn.Query<UserGradeInfo>(sql).ToList();
            }
        }
    }
}