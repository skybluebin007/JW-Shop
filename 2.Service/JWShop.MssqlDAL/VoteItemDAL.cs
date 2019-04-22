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
    /// <summary>
    /// 投票选项数据层说明。
    /// </summary>
    public sealed class VoteItemDAL : IVoteItem
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        /// <summary>
        /// 增加一条投票选项数据
        /// </summary>
        /// <param name="voteItem">投票选项模型变量</param>
        public int AddVoteItem(VoteItemInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
          {
              string sql = @"INSERT INTO VoteItem([VoteID],[ItemName],[VoteCount],[OrderID],[Image],[Department],[Solution],[Point],[CoverDepartment],[Detail],[Exp1],[Exp2],[Exp3],[Exp4],[Exp5]) VALUES(@VoteID,@ItemName,@VoteCount,@OrderID,@Image,@Department,@Solution,@Point,@CoverDepartment,@Detail,@Exp1,@Exp2,@Exp3,@Exp4,@Exp5);
                            select SCOPE_IDENTITY()";
              return conn.Query<int>(sql, entity).Single();
          }

        }


        /// <summary>
        /// 更新一条投票选项数据
        /// </summary>
        /// <param name="voteItem">投票选项模型变量</param>
        public void UpdateVoteItem(VoteItemInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"update VoteItem Set [VoteID]=@VoteID,[ItemName]=@ItemName,[VoteCount]=@VoteCount,[OrderID]=@OrderID,[Image]=@Image,[Department]=@Department,[Solution]=@Solution,[Point]=@Point,[CoverDepartment]=@CoverDepartment,[Detail]=@Detail,[Exp1]=@Exp1,[Exp2]=@Exp2,[Exp3]=@Exp3,[Exp4]=@Exp4,[Exp5]=@Exp5  WHERE [ID]=@ID";

                conn.Execute(sql, entity);
            }
        }

        /// <summary>
        /// 删除多条投票选项数据
        /// </summary>
        /// <param name="strID">投票选项的主键值,以,号分隔</param>
        public void DeleteVoteItem(int[] IdS)
        {
            if (IdS.Length > 0)
            {
                using (var conn = new SqlConnection(connectString))
                {
                    string sql = "delete from VoteItem where id in @ids";

                    conn.Execute(sql, new { ids = IdS });
                }
            }
        }

        /// <summary>
        /// 按分类删除投票选项数据
        /// </summary>
        /// <param name="strVoteID">分类ID,以,号分隔</param>
        public void DeleteVoteItemByVoteID(string strVoteID)
        {
            if (!string.IsNullOrEmpty(strVoteID))
            {
                using (var conn = new SqlConnection(connectString))
                {
                    string sql = "delete from VoteItem  WHERE [VoteID] like @voteId";

                    conn.Execute(sql, new { voteId = "|" + strVoteID + "|" });
                }
            }
        }

        /// <summary>
        /// 读取一条投票选项数据
        /// </summary>
        /// <param name="id">投票选项的主键值</param>
        /// <returns>投票选项数据模型</returns>
        public VoteItemInfo ReadVoteItem(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from VoteItem where id=@id";

                var data = conn.Query<VoteItemInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new VoteItemInfo();
            }
        }

        /// <summary>
        /// 准备投票选项模型
        /// </summary>
        /// <param name="dr">Datareader</param>
        /// <param name="voteItemList">投票选项的数据列表</param>
        public void PrepareVoteItemModel(SqlDataReader dr, List<VoteItemInfo> voteItemList)
        {
            while (dr.Read())
            {
                VoteItemInfo voteItem = new VoteItemInfo();
                voteItem.ID = dr.GetInt32(0);
                voteItem.VoteID = dr[1].ToString();
                voteItem.ItemName = dr[2].ToString();
                voteItem.VoteCount = dr.GetInt32(3);
                voteItem.OrderID = dr.GetInt32(4);
                voteItem.Image = dr[5].ToString();
                voteItem.Department = dr[6].ToString();
                voteItem.Solution = dr[7].ToString();
                voteItem.Point = dr[8].ToString();
                voteItem.CoverDepartment = dr[9].ToString();
                voteItem.Detail = dr[10].ToString();
                voteItem.Exp1 = dr[11].ToString();
                voteItem.Exp2 = dr[12].ToString();
                voteItem.Exp3 = dr[13].ToString();
                voteItem.Exp4 = dr[14].ToString();
                voteItem.Exp5 = dr[15].ToString();
                voteItemList.Add(voteItem);
            }
        }


        /// <summary>
        /// 获得投票选项的所有数据列表
        /// </summary>
        /// <returns>投票选项的所有数据列表</returns>
        public List<VoteItemInfo> ReadVoteItemAllList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from VoteItem ORDER BY [OrderID] desc,[ID] desc";
                return conn.Query<VoteItemInfo>(sql).ToList();
            }     
        }

        /// <summary>
        /// 获得投票选项所有数据
        /// </summary>
        /// <param name="voteID">分类ID</param>
        /// <returns>投票选项数据列表</returns>
        public List<VoteItemInfo> ReadVoteItemByVote(int voteID)
        {
            if (voteID > 0)
            {
                using (var conn = new SqlConnection(connectString))
                {
                    string sql = "select * from VoteItem WHERE [VoteID] like @voteId ORDER BY [OrderID] desc,[ID] desc";
                    return conn.Query<VoteItemInfo>(sql, new { voteId = "|" + voteID + "|" }).ToList();
                }
            }
            else
            {
                return new List<VoteItemInfo>();
            }
        }
        public List<VoteItemInfo> ReadVoteItemList(VoteItemSearchInfo voteItemSearch) {
            MssqlCondition mssqlCondition = new MssqlCondition();
            PrepareCondition(mssqlCondition, voteItemSearch);          
            List<VoteItemInfo> VoteItemList = new List<VoteItemInfo>();
            SqlParameter[] parameters ={
				new SqlParameter("@condition",SqlDbType.NVarChar)
			};
            parameters[0].Value = mssqlCondition.ToString() + " ORDER BY [OrderID] desc,[ID] desc";
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "SearchVoteItemList", parameters))
            {
                PrepareVoteItemModel(dr, VoteItemList);
            }
            return VoteItemList;
        }
        /// <summary>
        /// 获得投票选项所有数据~~随机排序
        /// </summary>
        /// <param name="voteID">分类ID</param>
        /// <returns>投票选项数据列表</returns>
        public List<VoteItemInfo> ReadVoteItemByVoteNEWID(int voteID)
        {
            if (voteID > 0)
            {
                using (var conn = new SqlConnection(connectString))
                {
                    string sql = "select * from VoteItem WHERE [VoteID] like @voteId ORDER BY NEWID()";
                    return conn.Query<VoteItemInfo>(sql, new { voteId = "|" + voteID + "|" }).ToList();
                }
            }
            else
            {
                return new List<VoteItemInfo>();
            }
        }


        /// <summary>
        /// 移动数据排序
        /// </summary>
        /// <param name="action">改变动作,向上:ChangeAction.Up;向下:ChangeActon.Down</param>
        /// <param name="id">当前记录的主键值</param>
        public void ChangeVoteItemOrder(ChangeAction action, int id)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@action",SqlDbType.NVarChar),
				new SqlParameter("@id",SqlDbType.Int)
			};
            parameters[0].Value = action.ToString();
            parameters[1].Value = id;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "ChangeVoteItemOrder", parameters);
        }

        /// <summary>
        /// 改变列表投票选项数量
        /// </summary>
        /// <param name="strID">投票选项的主键值</param>
        /// <param name="action">改变动作,减:ChangeAction.Minus;加:ChangeActon.Plus</param>
        public void ChangeVoteItemCount(string strID, ChangeAction action)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@strID",SqlDbType.NVarChar),
				new SqlParameter("@action",SqlDbType.NVarChar)
			};
            parameters[0].Value = strID;
            parameters[1].Value = action.ToString();
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "ChangeVoteItemCount", parameters);
        }

        /// <summary>
        /// 通过下级表改变列表投票选项数量
        /// </summary>
        /// <param name="strID">下级表的主键值,以,号分隔</param>
        /// <param name="action">改变动作,减:ChangeAction.Minus;加:ChangeActon.Plus</param>
        public void ChangeVoteItemCountByGeneral(string strID, ChangeAction action)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@strID",SqlDbType.NVarChar),
				new SqlParameter("@action",SqlDbType.NVarChar)
			};
            parameters[0].Value = strID;
            parameters[1].Value = action.ToString();
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "ChangeVoteItemCountByGeneral", parameters);
        }
        /// <summary>
        /// 获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>投票选项数据列表</returns>
        public List<VoteItemInfo> ReadVoteItemList(int currentPage, int pageSize, ref int count)
        {
            List<VoteItemInfo> voteItemList = new List<VoteItemInfo>();
            ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
            pc.TableName = "VoteItem";
            pc.Fields = "[ID],[VoteID],[ItemName],[VoteCount],[OrderID],[Image],[Department],[Solution],[Point],[CoverDepartment],[Detail],[Exp1],[Exp2],[Exp3],[Exp4],[Exp5]";
            pc.CurrentPage = currentPage;
            pc.PageSize = pageSize;
            pc.OrderField = "[ID]";
            pc.OrderType = OrderType.Desc;
            //pc.Count = count;
            count = pc.Count;
            using (SqlDataReader dr = pc.ExecuteReader())
            {
                PrepareVoteItemModel(dr, voteItemList);
            }
            return voteItemList;
        }
        /// <summary>
        /// 获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>投票选项数据列表</returns>
        public List<VoteItemInfo> ReadVoteItemList(int currentPage, int pageSize, ref int count,string orderType)
        {
            List<VoteItemInfo> voteItemList = new List<VoteItemInfo>();
            ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
            pc.TableName = "VoteItem";
            pc.Fields = "[ID],[VoteID],[ItemName],[VoteCount],[OrderID],[Image],[Department],[Solution],[Point],[CoverDepartment],[Detail],[Exp1],[Exp2],[Exp3],[Exp4],[Exp5]";
            pc.CurrentPage = currentPage;
            pc.PageSize = pageSize;
            if (string.IsNullOrEmpty(orderType))
                pc.OrderField = "[OrderID],[ID]";
            else
                pc.OrderField = "[" + orderType + "],[OrderID]";
            pc.OrderType = OrderType.Desc;
            //pc.Count = count;
            count = pc.Count;
            using (SqlDataReader dr = pc.ExecuteReader())
            {
                PrepareVoteItemModel(dr, voteItemList);
            }
            return voteItemList;
        }
        /// <summary>
        /// 获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>投票选项数据列表</returns>
        public List<VoteItemInfo> ReadVoteItemList(int currentPage, int pageSize, VoteItemSearchInfo voteitemSearch, ref int count, string orderType)
        {
            List<VoteItemInfo> voteItemList = new List<VoteItemInfo>();
            ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
            pc.TableName ="VoteItem";
            pc.Fields = "[ID],[VoteID],[ItemName],[VoteCount],[OrderID],[Image],[Department],[Solution],[Point],[CoverDepartment],[Detail],[Exp1],[Exp2],[Exp3],[Exp4],[Exp5]";
            pc.CurrentPage = currentPage;
            pc.PageSize = pageSize;
            if (string.IsNullOrEmpty(orderType))
                pc.OrderField = "[OrderID],[ID]";
            else
                pc.OrderField = "[" + orderType + "],[OrderID]";
            pc.OrderType = OrderType.Desc;
            PrepareCondition(pc.MssqlCondition, voteitemSearch);
            //pc.Count = count;
            count = pc.Count;
            using (SqlDataReader dr = pc.ExecuteReader())
            {
                PrepareVoteItemModel(dr, voteItemList);
            }
            return voteItemList;
        }
        /// <summary>
        /// 获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>投票选项数据列表</returns>
        public List<VoteItemInfo> ReadVoteItemList(int currentPage, int pageSize, ref int count, string orderType,int ascDesc)
        {
            List<VoteItemInfo> voteItemList = new List<VoteItemInfo>();
            ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
            pc.TableName = "VoteItem";
            pc.Fields = "[ID],[VoteID],[ItemName],[VoteCount],[OrderID],[Image],[Department],[Solution],[Point],[CoverDepartment],[Detail],[Exp1],[Exp2],[Exp3],[Exp4],[Exp5]";
            pc.CurrentPage = currentPage;
            pc.PageSize = pageSize;
            if (string.IsNullOrEmpty(orderType))
                pc.OrderField = "[ID]";
            else
                pc.OrderField = "[" + orderType + "]";
            if (ascDesc == 1)
            {
                pc.OrderType = OrderType.Asc;
            }
            else
            {
                pc.OrderType = OrderType.Desc;
            }
            //pc.Count = count;
            count = pc.Count;
            using (SqlDataReader dr = pc.ExecuteReader())
            {
                PrepareVoteItemModel(dr, voteItemList);
            }
            return voteItemList;
        }

        public List<VoteItemInfo> ReadVoteItemList(int currentPage, int pageSize, ref int count,string  invoteID, string orderType, int ascDesc)
        {
            List<VoteItemInfo> voteItemList = new List<VoteItemInfo>();
            ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
            pc.TableName = "VoteItem";
            pc.Fields = "[ID],[VoteID],[ItemName],[VoteCount],[OrderID],[Image],[Department],[Solution],[Point],[CoverDepartment],[Detail],[Exp1],[Exp2],[Exp3],[Exp4],[Exp5]";
            pc.CurrentPage = currentPage;
            pc.PageSize = pageSize;
            pc.MssqlCondition.Add("[VoteID]", invoteID, ConditionType.In);
            pc.MssqlCondition.Add("[VoteCount]",0, ConditionType.More);
            if (string.IsNullOrEmpty(orderType))
                pc.OrderField = "[ID]";
            else
                pc.OrderField = "[" + orderType + "],[ID]";
            if (ascDesc == 1)
            {
                pc.OrderType = OrderType.Asc;
            }
            else
            {
                pc.OrderType = OrderType.Desc;
            }
            pc.Count = count;
            count = pc.Count;
            using (SqlDataReader dr = pc.ExecuteReader())
            {
                PrepareVoteItemModel(dr, voteItemList);
            }
            return voteItemList;
        }

        /// <summary>
        /// 获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>投票选项数据列表</returns>
        public List<VoteItemInfo> ReadVoteItemListByVote(int currentPage, int pageSize, ref int count, string voteID)
        {
            List<VoteItemInfo> voteItemList = new List<VoteItemInfo>();
            ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
            pc.TableName ="VoteItem";
            pc.Fields = "[ID],[VoteID],[ItemName],[VoteCount],[OrderID],[Image],[Department],[Solution],[Point],[CoverDepartment],[Detail],[Exp1],[Exp2],[Exp3],[Exp4],[Exp5]";
            pc.CurrentPage = currentPage;
            pc.PageSize = pageSize;
            pc.MssqlCondition.Add("[VoteID]", voteID, ConditionType.Like);
            pc.OrderField = "[OrderID],[ID]";
            pc.OrderType = OrderType.Desc;
            //pc.Count = count;
            count = pc.Count;
            using (SqlDataReader dr = pc.ExecuteReader())
            {
                PrepareVoteItemModel(dr, voteItemList);
            }
            return voteItemList;
        }

        /// <summary>
        /// 获得投票选项数据列表
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="count">总数量</param>
        /// <returns>投票选项数据列表</returns>
        public List<VoteItemInfo> ReadVoteItemListByVote(int currentPage, int pageSize, ref int count, string voteID, int voteItemID, int ascDesc)
        {
            List<VoteItemInfo> voteItemList = new List<VoteItemInfo>();
            ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
            pc.TableName = "VoteItem";
            pc.Fields = "[ID],[VoteID],[ItemName],[VoteCount],[OrderID],[Image],[Department],[Solution],[Point],[CoverDepartment],[Detail],[Exp1],[Exp2],[Exp3],[Exp4],[Exp5]";
            pc.CurrentPage = currentPage;
            pc.PageSize = pageSize;
            pc.MssqlCondition.Add("[VoteID]",voteID, ConditionType.Like);            


            pc.OrderField = "[OrderID],[ID]";

            if (ascDesc == 1)
            {
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition.Add("[OrderID]", voteItemID, ConditionType.Less);
            }
            else
            {
                pc.OrderType = OrderType.Asc;
                pc.MssqlCondition.Add("[OrderID]", voteItemID, ConditionType.More);
            }
            
            pc.Count = count;
            count = pc.Count;
            using (SqlDataReader dr = pc.ExecuteReader())
            {
                PrepareVoteItemModel(dr, voteItemList);
            }
            return voteItemList;
        }
        /// <summary>
        /// 组合搜索条件
        /// </summary>
        /// <param name="mssqlCondition"></param>
        /// <param name="voteItemSearch"></param>
        public void PrepareCondition(MssqlCondition mssqlCondition, VoteItemSearchInfo voteItemSearch)
        {
            mssqlCondition.Add("[ItemName]", voteItemSearch.ItemName, ConditionType.Like);
            mssqlCondition.Add("[Exp2]", voteItemSearch.Exp2, ConditionType.Equal);
            mssqlCondition.Add("[VoteID]", voteItemSearch.VoteID, ConditionType.Like); ;
          
        }
        /// <summary>
        /// 获取当前最大排序号
        /// </summary>
        /// <returns></returns>
        public int MaxOrderID()
        {
            int maxOrderID = 0;

            ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
            pc.TableName = "(select max([OrderID]) as [MOID] from [VoteItem]) as temp";
            pc.Fields = "[MOID]";
            pc.OrderField = "[MOID]";
            pc.OrderType = OrderType.Desc;
            if (pc.ExecuteDataTable().Rows.Count > 0)
            {
                maxOrderID = Convert.IsDBNull(pc.ExecuteDataTable().Rows[0][0]) ? 0 : Convert.ToInt32(pc.ExecuteDataTable().Rows[0][0]);

            }


            return maxOrderID;
        }

    }
}