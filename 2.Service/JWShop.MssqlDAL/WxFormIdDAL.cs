using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Configuration;
using Dapper;
using System.Data.SqlClient;

namespace JWShop.MssqlDAL
{
    /// <summary>
    /// 微信formid 数据层
    /// </summary>
  public sealed  class WxFormIdDAL:IWxFormId
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(WxFormIdInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO [WxFormId](  [FormId], [UserId], [AddDate], [Used]) VALUES(@FormId, @UserId, @AddDate, @Used);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }
        /// <summary>
        /// 改变使用状态
        /// </summary>
        /// <param name="id"></param>
        public void ChangeUsed(int id)
        {
            using(var conn=new SqlConnection(connectString))
            {
                string sql = "UPDATE [WxFormId] SET [Used]=1 WHERE [Id]=@id";
                conn.Execute(sql, new { id=id});
            }
        }
        /// <summary>
        /// 读取指定用户的有效formid 集合(7天内，未使用)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<WxFormIdInfo> ReadUnusedByUserId(int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "SELECT * FROM [WxFormId] WHERE [UserId]=@userId AND [Used]!=1 AND DATEDIFF(day,[AddDate],GETDATE())<=7";
                return  conn.Query<WxFormIdInfo>(sql, new { userId=userId }).ToList();
            }
        }
    }
}
