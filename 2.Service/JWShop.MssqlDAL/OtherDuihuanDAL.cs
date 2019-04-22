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
    /// <summary>
    /// 数据层说明。
    /// </summary>
    public sealed class OtherDuihuanDAL :IOtherDuihuan
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(OtherDuihuanInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO OtherDuihuan(userid,truename,mobile,note,integral,adminid,addtime) VALUES(@userid,@truename,@mobile,@note,@integral,@adminid,@addtime);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(OtherDuihuanInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE OtherDuihuan SET userid = @userid, truename = @truename, mobile = @mobile, note = @note, integral = @integral, adminid = @adminid, addtime = @addtime
                            where id=@id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from OtherDuihuan where id=" + id;

                var para = new DynamicParameters();
                para.Add("ids", id);

                conn.Execute(sql, para);
            }
        }

        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from OtherDuihuan where id in @ids";

                var para = new DynamicParameters();
                para.Add("ids", ids);

                conn.Execute(sql, para);
            }
        }

        public OtherDuihuanInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OtherDuihuan where id=@id";

                var para = new DynamicParameters();
                para.Add("id", id);

                var data = conn.Query<OtherDuihuanInfo>(sql, para).SingleOrDefault();
                return data ?? new OtherDuihuanInfo();
            }
        }

        public OtherDuihuanInfo Read_User(int userid)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OtherDuihuan where userid=@userid";

                var para = new DynamicParameters();
                para.Add("userid", userid);

                var data = conn.Query<OtherDuihuanInfo>(sql, para).SingleOrDefault();
                return data ?? new OtherDuihuanInfo();
            }
        }

        public List<OtherDuihuanInfo> ReadList(int userid)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select top 1 * from OtherDuihuan where userid=@userid";
                return conn.Query<OtherDuihuanInfo>(sql, new { userid = userid }).ToList();
            }
        }

        public List<OtherDuihuanInfo> SearchList(OtherDuihuanSearchInfo searchInfo)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OtherDuihuan";

                string condition = PrepareCondition(searchInfo).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }

                return conn.Query<OtherDuihuanInfo>(sql).ToList();
            }
        }

        public List<OtherDuihuanInfo> SearchList(int currentPage, int pageSize, OtherDuihuanSearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "OtherDuihuan";
                pc.Fields = "[id], [userid], [truename], [mobile], [note], [integral], [adminid], [addtime] ";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[id]";
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<OtherDuihuanInfo>(pc).ToList();
            }
        }

        /// <summary>
        /// 组合搜索条件
        /// </summary>
        /// <param name="mssqlCondition"></param>
        /// <param name="userMessageSearch">UserPifaSeachInfo模型变量</param>
        public MssqlCondition PrepareCondition(OtherDuihuanSearchInfo SearchInfo)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[truename]", SearchInfo.truename, ConditionType.Like);
            mssqlCondition.Add("[mobile]", SearchInfo.mobile, ConditionType.Like);

            return mssqlCondition;
        }
    }
}
