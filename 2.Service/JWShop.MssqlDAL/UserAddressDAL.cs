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
    public sealed class UserAddressDAL : IUserAddress
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(UserAddressInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO UsrAddress( Consignee,Email,RegionId,Address,ZipCode,Tel,Mobile,IsDefault,UserId,UserName) VALUES(@Consignee,@Email,@RegionId,@Address,@ZipCode,@Tel,@Mobile,@IsDefault,@UserId,@UserName);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(UserAddressInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE UsrAddress SET Consignee = @Consignee, Email = @Email, RegionId = @RegionId, Address = @Address, ZipCode = @ZipCode, Tel = @Tel, Mobile = @Mobile, IsDefault = @IsDefault, UserId = @UserId, UserName = @UserName
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from UsrAddress where id=@id and userId=@userId";

                conn.Execute(sql, new { id = id, userId = userId });
            }
        }

        public UserAddressInfo Read(int id, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from UsrAddress where id=@id and userId=@userId";

                var data = conn.Query<UserAddressInfo>(sql, new { id = id, userId = userId }).SingleOrDefault();
                return data ?? new UserAddressInfo();
            }
        }

        public List<UserAddressInfo> ReadList(int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from UsrAddress where userId=@userId order by [IsDefault] desc";

                return conn.Query<UserAddressInfo>(sql, new { userId = userId }).ToList();
            }
        }

        public void SetDefault(int id, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"update UsrAddress set isDefault=0 where userId=@userId;
                           update UsrAddress set isDefault=1 where id=@id and userId=@userId ";

                conn.Execute(sql, new { id = id, userId = userId });
            }
        }

    }
}