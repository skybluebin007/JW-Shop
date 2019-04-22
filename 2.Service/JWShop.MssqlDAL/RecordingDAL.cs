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
    /// 砍价记录表数据层说明。
    /// </summary>
    public sealed class RecordingDAL : IRecording
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        /// <summary>
        /// 增加一条砍价记录表数据
        /// </summary>
        /// <param name="mode">砍价记录表模型变量</param>
        public int AddRecording(RecordingInfo mode)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"	INSERT INTO Recording([BOrderId],[Price],[Content],[Photo],[AddDate],[UserId],[UserName]) VALUES(@BOrderId,@price,@Content,@photo,@addDate,@userId,@userName); SELECT @@identity";
                return conn.Query<int>(sql, mode).Single();
            }
        }

        /// <summary>
        /// 更新一条砍价记录表数据
        /// </summary>
        /// <param name="mode">砍价记录表模型变量</param>
        public void UpdateRecording(RecordingInfo mode)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"	INSERT INTO Recording([BOrderId],[Price],[Content],[Photo],[AddDate],[UserId],[UserName]) VALUES(@BOrderId,@price,@Content,@photo,@addDate,@userId,@userName)
	SELECT @@identity";
                conn.Execute(sql, mode);
            }
        }

        /// <summary>
        /// 删除多条砍价记录表数据
        /// </summary>
        /// <param name="strId">砍价记录表的主键值,以,号分隔</param>
        public void DeleteRecording(string strId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"DELETE FROM Recording WHERE [Id] IN(@strId)";
                conn.Execute(sql, new { strId = strId });
            }
        }


        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"DELETE FROM Recording WHERE ID=@id";
                conn.Execute(sql, new { id = id });
            }
        }

        /// <summary>
        /// 读取一条砍价记录表数据
        /// </summary>
        /// <param name="id">砍价记录表的主键值</param>
        /// <returns>砍价记录表数据模型</returns>
        public RecordingInfo ReadRecording(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"SELECT * FROM Recording WHERE ID=@id";
                return conn.Query<RecordingInfo>(sql, new { id = id }).Single();
            }
        }

        /// <summary>
        /// 准备砍价记录表模型
        /// </summary>
        /// <param name="dr">Datareader</param>
        /// <param name="modeList">砍价记录表的数据列表</param>
        public void PrepareRecordingModel(SqlDataReader dr, List<RecordingInfo> modeList)
        {
            while (dr.Read())
            {
                RecordingInfo mode = new RecordingInfo();
                mode.Id = dr.GetInt32(0);
                mode.BOrderId = dr.GetInt32(1);
                mode.Price = dr.GetDecimal(2);
                mode.Content = dr[3].ToString();
                mode.Photo = dr[4].ToString();
                mode.AddDate = dr.GetDateTime(5);
                mode.UserId = dr.GetInt32(6);
                mode.UserName = dr[7].ToString();
                modeList.Add(mode);
            }
        }


        /// <summary>
        /// 搜索砍价记录表数据列表
        /// </summary>
        /// <param name="search">RecordingSearch模型变量</param>
        /// <returns>砍价记录表数据列表</returns>
        public List<RecordingInfo> SearchRecordingList(RecordingSearch search)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Recording";

                string condition = PrepareCondition(search).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition + " Order by AddDate DESC";
                }

                return conn.Query<RecordingInfo>(sql).ToList();
            }
        }

        /// <summary>
        /// 搜索砍价记录表数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="search">RecordingSearch模型变量</param>
        /// <param name="count">总数量</param>
        /// <returns>砍价记录表数据列表</returns>
        public List<RecordingInfo> SearchRecordingList(int currentPage, int pageSize, RecordingSearch search, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "Recording";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition = PrepareCondition(search);

                count = pc.Count;
                return conn.Query<RecordingInfo>(pc).ToList();
            }
        }


        /// <summary>
        /// 组合搜索条件
        /// </summary>
        /// <param name="mssqlCondition"></param>
        /// <param name="search">RecordingSearch模型变量</param>
        public MssqlCondition PrepareCondition(RecordingSearch search)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();
            mssqlCondition.Add("[BOrderId]", search.BOrderId, ConditionType.Equal);
            mssqlCondition.Add("[Price]", search.Price, ConditionType.Like);
            mssqlCondition.Add("[Name]", search.Name, ConditionType.Like);
            mssqlCondition.Add("[Photo]", search.Photo, ConditionType.Like);
            mssqlCondition.Add("[AddDate]", search.AddDate, ConditionType.Like);
            mssqlCondition.Add("[UserId]", search.UserId, ConditionType.Equal);
            mssqlCondition.Add("[UserName]", search.UserName, ConditionType.Like);

            mssqlCondition.Add("[BOrderId]", search.InBOrderId, ConditionType.In);
            return mssqlCondition;
        }
        /// <summary>
        /// 获取用户当天砍价次数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetRecordingCountByUser(int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select COUNT(1) FROM RECORDING where DATEDIFF(DAY,AddDate,GETDATE())=0 AND UserId=@UserId";
                return conn.ExecuteScalar<int>(sql, new { UserId = userId });
            }
        }
        /// <summary>
        /// 用户帮砍操作
        /// 帮砍成功之后更新BargainOrder
        /// 返回帮砍记录Id
        /// </summary>
        /// <param name="bargainOrder"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int HelpBargain(BargainOrderInfo bargainOrder, UserInfo user)
        {
            int help_record_id = 0;
            using (var conn = new SqlConnection(connectString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        //查找用户该领取那一刀
                        string sql = "SELECT TOP 1 * From [Recording] WITH(UPDLOCK,READPAST) WHERE [BOrderId]=@BOrderId AND [UserId]<=0 Order By [Id] Asc";
                        var record = conn.Query<RecordingInfo>(sql, new { @BOrderId = bargainOrder.Id }, tran).SingleOrDefault();
                        if (record.Id > 0)
                        {
                            help_record_id = record.Id;
                            //更新该条记录信息：用户Id，头像，姓名,日期
                            sql = "UPDATE [Recording] SET [Photo]=@Photo,[AddDate]=@AddDate,[UserId]=@UserId,[UserName]=@UserName WHERE [Id]=@Id";
                            int row = conn.Execute(sql, new { @Photo = user.Photo, @AddDate = DateTime.Now, @UserId = user.Id, @UserName = user.UserName, @Id=record.Id }, tran);
                            if (row > 0)
                            {
                                //帮砍成功之后更新BargainOrder
                                sql = "UPDATE [BargainOrder] SET [BargainPrice]=@BargainPrice WHERE [Id]=@Id";
                                row = conn.Execute(sql, new { @BargainPrice = record.Price + bargainOrder.BargainPrice, @Id = bargainOrder.Id }, tran);
                                if (row > 0)
                                {
                                    tran.Commit();                                  
                                }
                                else
                                {
                                    tran.Rollback();
                                    help_record_id = 0;
                                }
                            }
                            else
                            {
                                tran.Rollback();
                                help_record_id = 0;
                            }
                        }
                        else
                        {
                            tran.Rollback();
                            help_record_id = 0;
                        }
                      
                    }
                    catch(Exception ex)
                    {
                        tran.Rollback();
                        //throw new Exception(ex.Message);
                        help_record_id = 0;
                    }
                }
                return help_record_id;
            }
        }
    }
}