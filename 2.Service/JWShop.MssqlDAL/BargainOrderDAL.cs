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
	/// 砍价订单数据层说明。
	/// </summary>
	public sealed class BargainOrderDAL:IBargainOrder
	{
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        /// <summary>
        /// 增加一条砍价订单数据
        /// </summary>
        /// <param name="mode">砍价订单模型变量</param>
        public int AddBargainOrder(BargainOrderInfo mode)
		{
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO BargainOrder([UserId],[Status],[BargainDetailsId],[BargainPrice],[SharePrice],[ShareStatus],[BargainId],[Remark]) VALUES(@userId,@status,@bargainDetailsId,@bargainPrice,@sharePrice,@shareStatus,@BargainId,@Remark);SELECT @@identity";
                return conn.Query<int>(sql, mode).Single();
            }
        }
		
		/// <summary>
		/// 更新一条砍价订单数据
		/// </summary>
		/// <param name="mode">砍价订单模型变量</param>
		public bool UpdateBargainOrder(BargainOrderInfo mode)
		{
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE BargainOrder Set [UserId]=@userId,[Status]=@status,[BargainDetailsId]=@bargainDetailsId,[BargainPrice]=@bargainPrice,[SharePrice]=@sharePrice,[ShareStatus]=@shareStatus,[BargainId]=@BargainId WHERE [Id]=@id";
             return    conn.Execute(sql, mode)>0;
            }
        }

		/// <summary>
		/// 删除多条砍价订单数据
		/// </summary>
		/// <param name="strId">砍价订单的主键值,以,号分隔</param>
		public void DeleteBargainOrder(string strId)
		{
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"DELETE FROM BargainOrder WHERE [Id] IN(@strId)";
                conn.Execute(sql, new { strId=strId});
            }
        }


        public void Delete(int id)
        {
            using (var conn=new SqlConnection(connectString))
            {
                string sql = @"delete from BargainOrder where id=@id";
                conn.Execute(sql,new { id=id});
            }
        }


		/// <summary>
		/// 读取一条砍价订单数据
		/// </summary>
		/// <param name="id">砍价订单的主键值</param>
		/// <returns>砍价订单数据模型</returns>
		public BargainOrderInfo ReadBargainOrder(int id)
		{
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"SELECT * FROM BargainOrder WHERE [Id]=@id";
                return conn.Query<BargainOrderInfo>(sql, new { id = id }).SingleOrDefault()??new BargainOrderInfo();
            }
        }

		/// <summary>
		/// 准备砍价订单模型
		/// </summary>
		/// <param name="dr">Datareader</param>
		/// <param name="modeList">砍价订单的数据列表</param>
		public void PrepareBargainOrderModel(SqlDataReader dr,List<BargainOrderInfo> modeList)
		{
			while (dr.Read())
			{
				BargainOrderInfo mode= new BargainOrderInfo();
				mode.Id=dr.GetInt32(0);
				mode.UserId=dr.GetInt32(1);
				mode.Status=dr.GetInt32(2);
				mode.BargainDetailsId=dr.GetInt32(3);
				mode.BargainPrice=dr.GetDecimal(4);
				mode.SharePrice=dr.GetDecimal(5);
				mode.ShareStatus=dr.GetInt32(6);
				modeList.Add(mode);
			}
		} 

		
		/// <summary>
		/// 搜索砍价订单数据列表
		/// </summary>
		/// <param name="search">BargainOrderSearch模型变量</param>
		/// <returns>砍价订单数据列表</returns>
		public List<BargainOrderInfo> SearchBargainOrderList(BargainOrderSearch search)
		{
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from BargainOrder";

                string condition = PrepareCondition(search).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition+" Order by Id Desc";
                }

                return conn.Query<BargainOrderInfo>(sql).ToList();
            }
        }

		/// <summary>
		/// 搜索砍价订单数据列表
		/// </summary>
		/// <param name="currentPage">当前的页数</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="search">BargainOrderSearch模型变量</param>
		/// <param name="count">总数量</param>
		/// <returns>砍价订单数据列表</returns>
		public List<BargainOrderInfo> SearchBargainOrderList(int currentPage, int pageSize,BargainOrderSearch search,ref int count)
		{
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "BargainOrder";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderType = OrderType.Desc;

                pc.MssqlCondition = PrepareCondition(search);

                count = pc.Count;
                return conn.Query<BargainOrderInfo>(pc).ToList();
            }
        }


        	/// <summary>
       		/// 组合搜索条件
        	/// </summary>
        	/// <param name="mssqlCondition"></param>
        	/// <param name="search">BargainOrderSearch模型变量</param>
        	public MssqlCondition PrepareCondition(BargainOrderSearch search)
        	{
                MssqlCondition mssqlCondition = new MssqlCondition();
                    mssqlCondition.Add("[Id]",search.Id,ConditionType.Equal);	
			        mssqlCondition.Add("[UserId]",search.UserId,ConditionType.Equal);	
			        mssqlCondition.Add("[Status]",search.Status,ConditionType.Equal);	
			        mssqlCondition.Add("[BargainDetailsId]",search.BargainDetailsId,ConditionType.Equal);	
			        mssqlCondition.Add("[BargainPrice]",search.BargainPrice,ConditionType.Like);			
			        mssqlCondition.Add("[SharePrice]",search.SharePrice,ConditionType.Like);			
			        mssqlCondition.Add("[ShareStatus]",search.ShareStatus,ConditionType.Equal);
                return mssqlCondition;
        	}
        /// <summary>
        ///  事务操作：保存第一刀金额，分享金额，
        ///  保存帮砍记录金额,砍价参与人数加1
        ///  返回：BargainOrder.Id
        /// </summary>
        /// <returns></returns>
        public int CreateBargainOrder(BargainOrderInfo entity, List<decimal> bargain_Moneys)
        {
            int id = 0;
            using (var conn = new SqlConnection(connectString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        //添加BargainOrder
                        string sql = @"INSERT INTO BargainOrder([UserId],[Status],[BargainDetailsId],[BargainPrice],[SharePrice],[ShareStatus],[BargainId]) VALUES                  (@userId,@status,@bargainDetailsId,@bargainPrice,@sharePrice,@shareStatus,@BargainId);SELECT SCOPE_IDENTITY()";
                        id = conn.Query<int>(sql, entity,transaction).Single();
                        //如果添加BargainOrder成功,则逐条增加帮砍记录
                        if (id > 0)
                        {
                            foreach (decimal money in bargain_Moneys)
                            {
                                //如果是第一刀则记录发起砍价的用户名称、头像、Id，否则空出来，等其他用户帮砍时更新
                               var recording = new RecordingInfo()
                                {
                                    BOrderId = id,
                                    Price = money,
                                    Photo = bargain_Moneys.IndexOf(money)==0?entity.UserPhoto:string.Empty,
                                    AddDate = DateTime.Now,
                                    UserId =bargain_Moneys.IndexOf(money)==0?entity.UserId:0,
                                    UserName = bargain_Moneys.IndexOf(money) == 0 ? entity.UserName : string.Empty,
                               };
                                sql = @"INSERT INTO Recording([BOrderId],[Price],[Content],[Photo],[AddDate],[UserId],[UserName]) VALUES(@BOrderId,@price,@Content,@photo,@addDate,@userId,@userName)";
                                int rows = conn.Execute(sql, recording, transaction);
                                if (rows > 0)
                                {
                                    continue;
                                }
                                else
                                {
                                    transaction.Rollback();
                                }
                            }
                            //更新【Bargain】参与人数加1
                            sql = @"UPDATE [Bargain] SET [Number]=[Number]+1 WHERE [Id]=@Id";
                            int row = conn.Execute(sql, new { @Id = entity.BargainId }, transaction);
                            if (row > 0)
                            {
                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        id = 0;
                        transaction.Rollback();
                    }
                }
            }
            return id;
        }
        /// <summary>
        /// 砍价订单在线付款成功(如果砍到0则不需要支付)
        /// bargaindetails.sales+1 销量+1
        /// bargain.salesvolume+1 销量+1
        /// bargainorder.status=5 支付成功
        /// </summary>
        /// <param name="id">砍价订单Id</param>
        public void HandleBargainOrderPay(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                conn.Open();
                //开始事务
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // bargainorder.status=5 支付成功
                        string sql = "UPDATE [BargainOrder] SET [Status]=5  WHERE [Id]=@Id";

                        int rowAffected = conn.Execute(sql, new { Id = id }, transaction);
                        if (rowAffected > 0)
                        {
                            //bargaindetails.sales+1 销量+1
                            sql = "SELECT [BargainDetailsId] FROM [BargainOrder]  WHERE [Id]=@Id";
                            int bargainDetailsId = conn.ExecuteScalar<int>(sql, new { Id = id }, transaction);
                            if (bargainDetailsId > 0)
                            {
                                sql = "UPDATE [BargainDetails] SET [Sales]=[Sales]+1  WHERE [Id]=@Id";
                                rowAffected = conn.Execute(sql, new { Id = bargainDetailsId }, transaction);
                                if (rowAffected > 0)
                                {// bargain.salesvolume+1 销量+1
                                    sql = "SELECT [BargainId] FROM [BargainOrder]  WHERE [Id]=@Id";
                                    int bargainId = conn.ExecuteScalar<int>(sql, new { Id = id }, transaction);
                                    if (bargainId > 0)
                                    {
                                        sql = "UPDATE [Bargain] SET [SalesVolume]=[SalesVolume]+1  WHERE [Id]=@Id";
                                    }
                                    rowAffected = conn.Execute(sql, new { Id = bargainId }, transaction);
                                    if (rowAffected > 0)
                                    {
                                        //判断bargadindetail的库存和销量，如果已抢完，则把此商品其余正在进行的砍价置为“砍价失败”，原因“商品已抢完”
                                        sql = "SELECT * From [BargainDetails] WHERE [Id]=@Id";
                                        BargainDetailsInfo bargainDetail = conn.Query<BargainDetailsInfo>(sql, new { Id = bargainDetailsId }, transaction).FirstOrDefault() ?? new BargainDetailsInfo();
                                        if (bargainDetail.Id > 0 && bargainDetail.Sales >= bargainDetail.Stock)
                                        {
                                            //把除此单以外的，状态为“进行中”“砍价完成”"待付款"的置为“砍价失败”
                                            sql = @"Update BargainOrder set [Status]=3,[Remark]=CASE [Status] WHEN 1 THEN '商品已售罄，砍价失败'  WHEN 2 THEN '超时未下单，砍价失败' WHEN 4 THEN '超时未付款，砍价失败' ELSE '活动已取消，砍价失败' END  WHERE BargainDetailsId=@BargainDetailsId AND [Id]!=@Id AND ([Status]=1 or [Status]=2  or [Status]=4)";
                                            conn.Execute(sql, new { BargainDetailsId = bargainDetail.Id, Id = id }, transaction);
                                        }
                                        transaction.Commit();
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                    }
                                }
                                else
                                {
                                    transaction.Rollback();
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }



    }
}