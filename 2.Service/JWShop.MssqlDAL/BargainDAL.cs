using System.Data.SqlClient;
using System.Collections.Generic;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Configuration;
using Dapper;
using System.Linq;
using System;

namespace JWShop.MssqlDAL
{
    /// <summary>
    /// 砍价数据层说明。
    /// </summary>
    public sealed class BargainDAL:IBargain
	{
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        /// <summary>
        /// 增加一条砍价数据
        /// </summary>
        /// <param name="model">砍价模型变量</param>
        public int AddBargain(BargainInfo model)
		{
            using (var conn=new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO Bargain([Name],[StartDate],[EndDate],[LimitCount],[NumberPeople],[ActivityRules],[SuccessRate],[SalesVolume],[Number],[Status]) VALUES(@Name,@startDate,@endDate,@limitCount,@numberPeople,@activityRules,@successRate,@SalesVolume,@Number,@Status)
	SELECT @@identity";
                return conn.Query<int>(sql, model).Single();
            }
		}
		
		/// <summary>
		/// 更新一条砍价数据
		/// </summary>
		/// <param name="model">砍价模型变量</param>
		public void UpdateBargain(BargainInfo model)
		{
            using (var conn=new SqlConnection(connectString))
            {
                string sql = @"UPDATE Bargain Set [Name]=@Name, [StartDate]=@startDate,[EndDate]=@endDate,[LimitCount]=@limitCount,[NumberPeople]=@numberPeople,[ActivityRules]=@activityRules,[SuccessRate]=@successRate,[SalesVolume]=@SalesVolume,[Number]=@Number,[Status]=@Status WHERE [Id]=@id";
                conn.Execute(sql,model);
            }
		}

		/// <summary>
		/// 删除多条砍价数据
		/// </summary>
		/// <param name="strId">砍价的主键值,以,号分隔</param>
		public void DeleteBargain(string strId)
		{
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"delete from Bargain where id in (@ids)";
                conn.Execute(sql,new { ids=strId});
            }
        }


        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"delete from Bargain where id=@id";
                conn.Execute(sql, new { id=id });
            }
        }


		/// <summary>
		/// 读取一条砍价数据
		/// </summary>
		/// <param name="id">砍价的主键值</param>
		/// <returns>砍价数据模型</returns>
		public BargainInfo ReadBargain(int id)
		{
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"SELECT * FROM Bargain WHERE id=@id";
                var data = conn.Query<BargainInfo>(sql, new { id = id }).SingleOrDefault() ?? new BargainInfo();
                return data;
            }
        }

		/// <summary>
		/// 准备砍价模型
		/// </summary>
		/// <param name="dr">Datareader</param>
		/// <param name="modelList">砍价的数据列表</param>
		public void PrepareBargainModel(SqlDataReader dr,List<BargainInfo> modelList)
		{
			while (dr.Read())
			{
				BargainInfo model= new BargainInfo();
				model.Id=dr.GetInt32(0);
				model.StartDate=dr.GetDateTime(1);
				model.EndDate=dr.GetDateTime(2);
				model.LimitCount=dr.GetInt32(3);
				model.NumberPeople=dr.GetInt32(4);
				model.ActivityRules=dr[5].ToString();
				model.SuccessRate=dr.GetInt32(6);
				modelList.Add(model);
			}
		} 

		
		/// <summary>
		/// 搜索砍价数据列表
		/// </summary>
		/// <param name="search">BargainSearch模型变量</param>
		/// <returns>砍价数据列表</returns>
		public List<BargainInfo> SearchBargainList(BargainSearch search)
		{
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Bargain";

                string condition = PrepareCondition(search).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }

                return conn.Query<BargainInfo>(sql).ToList();
            }
        }

		/// <summary>
		/// 搜索砍价数据列表
		/// </summary>
		/// <param name="currentPage">当前的页数</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="search">BargainSearch模型变量</param>
		/// <param name="count">总数量</param>
		/// <returns>砍价数据列表</returns>
		public List<BargainInfo> SearchBargainList(int currentPage, int pageSize,BargainSearch search,ref int count)
		{
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "Bargain";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderType = OrderType.Desc;

                pc.MssqlCondition = PrepareCondition(search);

                count = pc.Count;
                return conn.Query<BargainInfo>(pc).ToList();
            }
        }


        	/// <summary>
       		/// 组合搜索条件
        	/// </summary>
        	/// <param name="mssqlCondition"></param>
        	/// <param name="search">BargainSearch模型变量</param>
        	public MssqlCondition PrepareCondition( BargainSearch search)
        	{
                MssqlCondition mssqlCondition = new MssqlCondition();
                mssqlCondition.Add("[Name]",search.Name,ConditionType.Like);	
			    //mssqlCondition.Add("[StartDate]",search.StartDate,ConditionType.Like);			
			    //mssqlCondition.Add("[EndDate]",search.EndDate,ConditionType.Like);			
			    mssqlCondition.Add("[LimitCount]",search.LimitCount,ConditionType.Equal);	
			    mssqlCondition.Add("[NumberPeople]",search.NumberPeople,ConditionType.Equal);	
			    mssqlCondition.Add("[ActivityRules]",search.ActivityRules,ConditionType.Like);			
			    mssqlCondition.Add("[SuccessRate]",search.SuccessRate,ConditionType.Equal);
                return mssqlCondition;
        	}


        /// <summary>
        /// 读取正在进行的并且有效的砍价活动
        /// </summary>
        /// <returns></returns>
        public List<BargainInfo> View_ReadBargain()
        {
            using (var conn=new SqlConnection(connectString))
            {
                string sql = @"SELECT * FROM [View_ReadBargain] ORDER BY [Id] DESC";

                return conn.Query<BargainInfo>(sql).ToList();
            }
        }
        /// <summary>
        /// 关闭活动事务：关闭、到期结束 活动，将未支付成功的砍价全部置为“砍价失败”，原因“活动已取消，砍价失败”“活动已结束，砍价失败”
        /// </summary>
        /// <param name="id"></param>
        public void ChangeBargainStatus(int id, int status)
        {
            using (var conn=new SqlConnection(connectString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        //砍价活动状态更改为“关闭”
                        string sql = "Update Bargain set [Status]=@Status Where [Id]=@Id";
                        int rows = conn.Execute(sql, new { Id = id,Status=status }, tran);
                        if (rows > 0)
                        {
                            sql = "update BargainOrder set [Status]=3,[Remark]=@Remark WHERE BargainId=@BargainId  AND ([Status]=1 or [Status]=2 or [Status]=4)";
                            conn.Execute(sql, new { @BargainId = id, @Remark = status==(int)Bargain_Status.ShutDown? "活动已取消，砍价失败": "活动已结束，砍价失败" }, tran);
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }
                    }
                    catch(Exception ex)
                    {
                        tran.Rollback();
                    }
                }
            }
        }
    }
}