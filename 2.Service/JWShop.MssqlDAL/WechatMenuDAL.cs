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
    /// 微信菜单数据层说明。
    /// </summary>
    public sealed class WechatMenuDAL:IWechatMenu
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(WechatMenuInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO [WechatMenu]( [FatherId],[Name],[Type],[Key],[Url],[OrderId]) VALUES(@FatherId,@Name,@Type,@Key,@Url,@OrderId);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }
        public void Update(WechatMenuInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE [WechatMenu] SET [FatherId]=@FatherId,[Name]=@Name,[Type]=@Type,[Key]=@Key,[Url]=@Url,[OrderId]=@OrderId where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from [WechatMenu] where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public WechatMenuInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [WechatMenu] where id=@id";

                var data = conn.Query<WechatMenuInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new WechatMenuInfo();
            }
        }
        public List<WechatMenuInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "SELECT * FROM [WechatMenu] ORDER BY OrderId Asc, Id ASC";

                return conn.Query<WechatMenuInfo>(sql).ToList();
            }
        }
        public List<WechatMenuInfo> ReadChildList(int fatherid)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "SELECT * FROM [WechatMenu] where [FatherId]=@fatherId ORDER BY  OrderId Asc, Id ASC";

                return conn.Query<WechatMenuInfo>(sql, new { fatherId = fatherid }).ToList();
            }
        }

        /// <summary>
        /// 上移图片
        /// </summary>
        /// <param name="id">要移动的id</param>
        public void MoveUpWechatMenu(int id)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@id",SqlDbType.Int)
			};
            parameters[0].Value = id;
            ShopMssqlHelper.ExecuteNonQuery("MoveUpWechatMenu", parameters);
        }


        /// <summary>
        /// 下移图片
        /// </summary>
        /// <param name="id">要移动的id</param>
        public void MoveDownWechatMenu(int id)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@id",SqlDbType.Int)
			};
            parameters[0].Value = id;
            ShopMssqlHelper.ExecuteNonQuery("MoveDownWechatMenu", parameters);
        }
    }
}
