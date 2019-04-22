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
using System.Linq;
using Dapper;

namespace JWShop.MssqlDAL
{
    /// <summary>
    /// 管理员数据层说明。
    /// </summary>
    public sealed class AdminDAL : IAdmin
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(AdminInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO [Admin]( Name,Email,GroupId,Password,LastLoginIP,LastLoginDate,LoginTimes,loginErrorTimes,NoteBook,IsCreate,Status,[SafeCode],[FindDate]) VALUES(@Name,@Email,@GroupId,@Password,@LastLoginIP,@LastLoginDate,@LoginTimes,@loginErrorTimes,@NoteBook,@IsCreate,@Status,@SafeCode,@FindDate);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(AdminInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE [Admin] SET Name = @Name, Email = @Email, GroupId = @GroupId, NoteBook = @NoteBook, [Status] = @Status, [LoginErrorTimes] = @LoginErrorTimes,[SafeCode]=@SafeCode,[FindDate]=@FindDate 
                            where Id=@Id";

                if (entity.Status == (int)BoolType.True) entity.LoginErrorTimes = 0;
                conn.Execute(sql, entity);
            }
        }

        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from [Admin] where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public void DeleteByGroupIds(int[] groupIds)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from [Admin] where GroupId in @groupIds";

                conn.Execute(sql, new { groupIds = groupIds });
            }
        }

        public AdminInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [Admin] where id=@id";

                var data = conn.Query<AdminInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new AdminInfo();
            }
        }

        public AdminInfo Read(string name)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [Admin] where Name=@name";

                var data = conn.Query<AdminInfo>(sql, new { name = name }).SingleOrDefault();
                return data ?? new AdminInfo();
            }
        }

        public List<AdminInfo> ReadList(int currentPage, int pageSize, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "[Admin]";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;

                count = pc.Count;
                return conn.Query<AdminInfo>(pc).ToList();
            }
        }

        public List<AdminInfo> ReadList(int groupId, int currentPage, int pageSize, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "[Admin]";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition.Add("[GroupId]", groupId, ConditionType.Equal);

                count = pc.Count;
                return conn.Query<AdminInfo>(pc).ToList();
            }
        }

        public void ChangePassword(int id, string newPassword)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "UPDATE [Admin] Set [Password]=@password WHERE [Id]=@id";

                conn.Execute(sql, new { password = newPassword, id = id });
            }
        }

        public void ChangePassword(int id, string oldPassword, string newPassword)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "UPDATE [Admin] Set [Password]=@newPassword WHERE [Id]=@id and [Password]=@oldPassword";

                conn.Execute(sql, new { id = id, oldPassword = oldPassword, newPassword = newPassword });
            }
        }

        public void UpdateLogin(int id, DateTime lastLoginDate, string lastLoginIP)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "UPDATE [Admin] SET [LastLoginDate]=@lastLoginDate,[LastLoginIP]=@lastLoginIP,[LoginTimes]=[LoginTimes]+1,[LoginErrorTimes]=0,[Status]=1 WHERE [Id]=@id";

                conn.Execute(sql, new { id = id, lastLoginDate = lastLoginDate, lastLoginIP = lastLoginIP });
            }
        }

        public void UpdateLogin(string name, DateTime lastLoginDate, string lastLoginIP, int maxErrorTimes)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "UPDATE [Admin] SET [LastLoginDate]=@lastLoginDate,[LastLoginIP]=@lastLoginIP,[LoginErrorTimes]=[LoginErrorTimes]+1,[Status]=case when [LoginErrorTimes]>=@maxErrorTimes-1 then 0 else 1 end WHERE [Name]=@name";

                conn.Execute(sql, new { name = name, lastLoginDate = lastLoginDate, lastLoginIP = lastLoginIP, maxErrorTimes = maxErrorTimes });
            }
        }

        public AdminInfo CheckLogin(string loginName, string loginPass)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "SELECT [Id],[Name],[GroupId],[Status],[LastLoginDate],[LoginErrorTimes] FROM [Admin] WHERE [Name]=@loginName AND [Password]=@loginPass";

                var data = conn.Query<AdminInfo>(sql, new { loginName = loginName, loginPass = loginPass }).SingleOrDefault();
                return data ?? new AdminInfo();
            }
        }
        /// <summary>
        /// 更改用户的的安全码（找回密码使用）
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="safeCode"></param>
        /// <param name="findDate"></param>
        public void ChangeAdminSafeCode(int adminID, string safeCode, DateTime findDate)
        {
            AdminInfo tempUser = Read(adminID);
            tempUser.SafeCode = safeCode;
            tempUser.FindDate = findDate;
            Update(tempUser);
        }
        /// <summary>
        /// 管理员状态解锁，错误次数清零
        /// </summary>
        /// <param name="id"></param>
        public void UpdateStatus(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "UPDATE [Admin] SET [LoginErrorTimes]=0,[Status]=1 WHERE [Id]=@id";

                conn.Execute(sql, new { id = id});
            }
        }
        /// <summary>
        /// 检查Email唯一性,true:唯一
        /// </summary>
        /// <param name="email"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UniqueEmail(string email, int id = 0)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select count(*) from [Admin] where Email=@email and [Id]<>@id ";

                return conn.ExecuteScalar<int>(sql, new { id = id, email = email }) < 1;
            }
        }
    }
}