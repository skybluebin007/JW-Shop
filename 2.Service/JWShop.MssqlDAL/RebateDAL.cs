using System;
using System.Collections.Generic;
using System.Linq;
using JWShop.Entity;
using JWShop.IDAL;
using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using SkyCES.EntLib;

namespace JWShop.MssqlDAL
{
    /// <summary>
    /// 返佣记录 数据层
    /// </summary>
    public sealed class RebateDAL : IRebate
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];
        public int Add(RebateInfo entity)
        {
            int id = 0;
            #region 事务：添加返佣记录，计算分销商 总佣金
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        //添加返佣记录
                        string sql = @"INSERT INTO [Rebate]([Distributor_Id],[User_Id],[Order_Id],[Commission],[Time]) VALUES(@Distributor_Id,@User_Id,@Order_Id,@Commission,@Time);
                                    SELECT SCOPE_IDENTITY()";
                        id = conn.Query<int>(sql, entity, transaction).Single();
                        if (id > 0)
                        {
                            // 计算计算分销商 总佣金
                            sql = @"UPDATE [Usr] SET [Total_Commission]=[Total_Commission]+@commsion WHERE [Id]=@Distributor_Id";
                            int rows = conn.Execute(sql, new { commsion = entity.Commission, Distributor_Id = entity.Distributor_Id }, transaction);
                            if (rows > 0)
                            {
                                transaction.Commit();
                            }
                            else
                            {
                                id = 0;
                                transaction.Rollback();
                            }                           
                        }
                        else
                        {
                            id = 0;
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
            #endregion
            return id;
        }

        public RebateInfo Read(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                string sql = @"SELECT * FROM [Rebate] WHERE [Id]=@Id";
                return conn.Query<RebateInfo>(sql, new { Id = id }).SingleOrDefault() ?? new RebateInfo();
            }
        }

        public List<RebateInfo> SearchList(RebateSearchInfo searchModel)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                //string sql = @"SELECT * FROM (select a.*,b.[UserName],b.[RealName],b.[Mobile] from [Rebate] a
                //            LEFT JOIN [Usr] b ON a.[Distributor_Id] = b.[Id]) TMP";
                string sql = @"select a.*,b.[UserName],b.[RealName],b.[Mobile],c.[OrderNumber],
                                    [Buyer_UserName]=(SELECT [UserName] FROM [Usr] WHERE [Id]=a.[User_Id])
                                     from [Rebate] a
                                    LEFT JOIN [Usr] b ON a.[Distributor_Id] = b.[Id]
                                    LEFT JOIN [Order] c ON A.[Order_id]=c.[Id]";
                string condition = PrepareCondition(searchModel).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }
                sql += " Order by [Id] desc";

                return conn.Query<RebateInfo>(sql).ToList();
            }
        }

        public List<RebateInfo> SearchList(int currentPage, int pageSize, RebateSearchInfo searchModel, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                //pc.TableName = @"(select a.*,b.[UserName],b.[RealName],b.[Mobile] from [Rebate] a
                //                LEFT JOIN [Usr] b ON a.[Distributor_Id] = b.[Id]) TMP";
                pc.TableName = @"(select a.*,b.[UserName],b.[RealName],b.[Mobile],c.[OrderNumber],
                                    [Buyer_UserName]=(SELECT [UserName] FROM [Usr] WHERE [Id]=a.[User_Id])
                                     from [Rebate] a
                                    LEFT JOIN [Usr] b ON a.[Distributor_Id] = b.[Id]
                                    LEFT JOIN [Order] c ON A.[Order_id]=c.[Id]) TMP";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition = PrepareCondition(searchModel);

                count = pc.Count;
                return conn.Query<RebateInfo>(pc).ToList();
            }
        }

        public void Update(RebateInfo entity)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE [Rebate] SET [Distributor_Id]=@Distributor_Id,[Commission]=@Commission,[Time]=@Time  WHERE [Id]=@Id";
                conn.Execute(sql, entity);
            }
        }

        public MssqlCondition PrepareCondition(RebateSearchInfo searchModel)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[Distributor_Id]", searchModel.Distributor_Id, ConditionType.Equal);
            mssqlCondition.Add("[Time]", searchModel.StartTime, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[Time]", searchModel.EndTtime, ConditionType.LessOrEqual);
            mssqlCondition.Add("[UserName]", searchModel.UserName, ConditionType.Like);
            mssqlCondition.Add("[RealName]", searchModel.RealName, ConditionType.Like);
            mssqlCondition.Add("[Mobile]", searchModel.Mobile, ConditionType.Like);
            return mssqlCondition;
        }
        /// <summary>
        /// 统计分销商总佣金
        /// </summary>
        /// <param name="Distributor_Id"></param>
        /// <returns></returns>
        public decimal GetSumCommission(int distributor_Id)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                string sql = @"SELECT SUM([Commission]) FROM [Rebate] WHERE [Distributor_Id]=@Distributor_Id";
                return conn.Query<decimal>(sql, new { @Distributor_Id = distributor_Id }).Single();
            }
        }
    }
}
