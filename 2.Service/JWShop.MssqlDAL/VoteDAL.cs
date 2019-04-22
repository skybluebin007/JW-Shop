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
    public sealed class VoteDAL:IVote
    {
      private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

      /// <summary>
		/// 增加一条投票数据
		/// </summary>
		/// <param name="vote">投票模型变量</param>
      public int AddVote(VoteInfo entity)
      {
          using (var conn = new SqlConnection(connectString))
          {
              string sql = @"INSERT INTO Vote([FatherID],[Title],[ItemCount],[VoteType],[Note],[OrderID]) VALUES(@FatherID,@Title,@ItemCount,@VoteType,@Note,@OrderID);
                            select SCOPE_IDENTITY()";
              return conn.Query<int>(sql, entity).Single();
          }
      }
      /// <summary>
		/// 更新一条投票数据
		/// </summary>
		/// <param name="vote">投票模型变量</param>
      public void UpdateVote(VoteInfo entity)
      {
          using (var conn = new SqlConnection(connectString))
          {
              string sql = @"update Vote Set [FatherID]=@FatherID,[Title]=@Title,[VoteType]=@VoteType,[Note]=@Note,[OrderID]=@OrderID  WHERE [ID]=@ID";

              conn.Execute(sql, entity);
          }
      }
      /// <summary>
      /// 删除多条投票数据
      /// </summary>
      /// <param name="strID">投票的主键值,以,号分隔</param>
      public void DeleteVote(int[] strID)
      {
          if (strID.Length > 0)
          {
              using (var conn = new SqlConnection(connectString))
              {
                  string sql = "delete from Vote where id in @ids";

                  conn.Execute(sql, new { ids = strID });
              }
          }
      }
      /// <summary>
      /// 读取一条投票数据
      /// </summary>
      /// <param name="id">投票的主键值</param>
      /// <returns>投票数据模型</returns>
      public VoteInfo ReadVote(int id)
      {
          using (var conn = new SqlConnection(connectString))
          {
              string sql = "select * from Vote where id=@id";

              var data = conn.Query<VoteInfo>(sql, new { id = id }).SingleOrDefault();
              return data ?? new VoteInfo();
          }
      }

      /// <summary>
      /// 获得投票类型列表--分页
      /// </summary>
      /// <param name="currentPage">当前的页数</param>
      /// <param name="pageSize">每页记录数</param>
      /// <param name="count">总数量</param>
      /// <returns>投票数据列表</returns>
      public List<VoteInfo> ReadVoteList(int currentPage, int pageSize, ref int count)
      {
          using (var conn = new SqlConnection(connectString))
          {
              List<VoteInfo> voteList = new List<VoteInfo>();
              ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
              pc.TableName ="Vote";
              pc.Fields = "[ID],[FatherID],[Title],[ItemCount],[VoteType],[Note],[OrderID]";
              pc.CurrentPage = currentPage;
              pc.PageSize = pageSize;
              pc.OrderField = "[OrderID],[ID]";
              pc.OrderType = OrderType.Asc;             
              count = pc.Count;
              return conn.Query<VoteInfo>(pc).ToList();
          }       
      }
      /// <summary>
      /// 获得投票类型列表--所有
      /// </summary>
      /// <returns></returns>
      public List<VoteInfo> ReadVoteList()
      {
          using (var conn = new SqlConnection(connectString))
          {
              string sql = "select * from Vote order by [OrderID] Asc,[ID] Asc";
              return conn.Query<VoteInfo>(sql).ToList();
          }        
      }
      /// <summary>
      /// 改变列表投票数量
      /// </summary>
      /// <param name="id">投票的主键值</param>
      /// <param name="action">改变动作,减:ChangeAction.Minus;加:ChangeActon.Plus</param>
      public void ChangeVoteCount(int id, ChangeAction action)
      {
          SqlParameter[] parameters ={
				new SqlParameter("@id",SqlDbType.Int),
				new SqlParameter("@action",SqlDbType.NVarChar)
			};
          parameters[0].Value = id;
          parameters[1].Value = action.ToString();
          ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "ChangeVoteCount", parameters);
      }

      /// <summary>
      /// 通过下级表改变列表投票数量
      /// </summary>
      /// <param name="strID">下级表的主键值,以,号分隔</param>
      /// <param name="action">改变动作,减:ChangeAction.Minus;加:ChangeActon.Plus</param>
      public void ChangeVoteCountByGeneral(string strID, ChangeAction action)
      {
          SqlParameter[] parameters ={
				new SqlParameter("@strID",SqlDbType.NVarChar),
				new SqlParameter("@action",SqlDbType.NVarChar)
			};
          parameters[0].Value = strID;
          parameters[1].Value = action.ToString();
          ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "ChangeVoteCountByGeneral", parameters);
      }
      /// <summary>
      /// 获取每个类型的选项数
      /// </summary>
      /// <param name="VoteID"></param>
      /// <returns></returns>
      public int GetVoteItemCountByVote(string VoteID)
      {
          using (var conn = new SqlConnection(connectString))
          {
              string sql = @"select count(*) as [ItemCount]  from VoteItem where [VoteID] like @voteid";
              return conn.Query<int>(sql, new { voteid = "%|" + VoteID + "|%" }).Single();
          }
      }

    }
}
