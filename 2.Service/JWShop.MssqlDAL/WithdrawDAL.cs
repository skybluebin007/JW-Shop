using System;
using System.Collections.Generic;
using System.Linq;
using JWShop.IDAL;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace JWShop.MssqlDAL
{
    /// <summary>
    /// 提现记录 数据层
    /// </summary>
    public sealed class WithdrawDAL : IWithdraw
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];
        /// <summary>
        /// 申请提现，增加用户总提现    
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Add(WithdrawInfo entity)
        {
            int id = 0;
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        //增加提现申请
                        string sql = @"INSERT INTO [Withdraw]([Distributor_Id],[Amount],[Time],[Status]) VALUES(@Distributor_Id,@Amount,@Time,@Status);
                                SELECT SCOPE_IDENTITY()";
                        id = conn.Query<int>(sql, entity, transaction).Single();
                        if (id > 0)
                        {
                            //增加用户总提现
                            sql = @"UPDATE [Usr] SET [Total_Withdraw]=[Total_Withdraw]+@Withdraw WHERE [Id]=@Distributor_Id";
                            int row = conn.Execute(sql, new { @Withdraw = entity.Amount, @Distributor_Id = entity.Distributor_Id }, transaction);
                            if (row > 0)
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
                            transaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        id = 0;
                        transaction.Rollback();
                    }
                }


                return id;
            }
        }
        public WithdrawInfo Read(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                string sql = @"SELECT * FROM [Withdraw] WHERE [Id]=@Id";
                return conn.Query<WithdrawInfo>(sql, new { @Id = id }).SingleOrDefault();
            }
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                string sql = @"DELETE FROM [Withdraw] WHERE [Id]=@Id";
                conn.Execute(sql, new { @Id = id });
            }
        }
        /// <summary>
        /// 统计分销商已提现总额
        /// 状态：complete
        /// </summary>   
        public decimal GetSumWithdraw(int distributor_Id)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                string sql = @"SELECT SUM(Amount) FROM [Withdraw] WHERE [Distributor_Id]=@Distributor_Id AND [Status]=2";
                return conn.Query<Decimal>(sql, new { @Distributor_Id = distributor_Id }).Single();
            }
        }
        /// <summary>
        /// 操作：拒绝、完成（已返现）
        /// 拒绝操作：减少用户总提现
        /// 增加操作记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="operate"></param>
        public bool Operate(int id, Withdraw_Operate operate, int operator_Id, string note)
        {
            bool result = true;
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        //改变【Withdraw】 申请状态
                        int status = operate == Withdraw_Operate.Reject ? -1 : 2;
                        string sql = @"UPDATE [Withdraw] SET [Status]=@Status WHERE [Id]=@Id AND [Status]=1";
                        int row = conn.Execute(sql, new { @Id = id, @Status = status }, transaction);
                        if (row > 0)
                        {
                            //记录此次操作
                            sql = @"INSERT INTO [Withdraw_Operate]([Withdraw_Id],[Operator_Id],[Operate],[Note],[Time]) VALUES(@Withdraw_Id,@Operator_Id,@Operate,@Note,@Time)";
                            row = conn.Execute(sql, new { @Withdraw_Id = id, @Operator_Id = operator_Id, @Operate = (int)operate, @Note = note, @Time = DateTime.Now }, transaction);
                            if (row > 0)
                            {
                                //如果是提现完成，则提交事务 
                                if (status == 2)
                                {
                                    transaction.Commit();
                                   
                                }
                                else
                                {//如果拒绝提现，则减少用户总提现
                                 //获得提现记录 withdraw

                                    sql = @"SELECT * FROM [Withdraw] WHERE [Id]=@Id";
                                    var withdraw = conn.Query<WithdrawInfo>(sql, new { @Id = id }, transaction).SingleOrDefault();
                                    if (withdraw.Id > 0)
                                    {
                                        //计算分销商总提现                               
                                        sql = @"UPDATE [Usr] SET [Total_Withdraw]=[Total_Withdraw]-@Withdraw WHERE [Id]=@Distributor_Id";
                                        row = conn.Execute(sql, new { @Withdraw = withdraw.Amount, Distributor_Id = withdraw.Distributor_Id }, transaction);
                                        if (row > 0)
                                        {
                                            transaction.Commit();
                                        }
                                        else
                                        {
                                            result = false;
                                            transaction.Rollback();
                                        }
                                    }
                                    else
                                    {
                                        result = false;
                                        transaction.Rollback();
                                    }
                                }
                            }
                            else
                            {
                                result = false;
                                transaction.Rollback();
                            }
                        }
                        else
                        {
                            result = false;
                            transaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        transaction.Rollback();
                    }
                }
            }
            return result;
        }

        public List<WithdrawInfo> SearchList(WithdrawSearchInfo searchModel)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                string sql = @"SELECT * FROM (select a.*,b.[UserName],b.[RealName],b.[Mobile] from [Withdraw] a
                                LEFT JOIN[Usr] b ON a.[Distributor_Id] = b.[Id]) TMP";
                string condition = PrepareCondition(searchModel).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " WHERE " + condition;
                }
                sql += " Order by [Id] desc";
                return conn.Query<WithdrawInfo>(sql).ToList();
            }
        }

        public List<WithdrawInfo> SearchList(int currentPage, int pageSize, WithdrawSearchInfo searchModel, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = @"(select a.*,b.[UserName],b.[RealName],b.[Mobile] from [Withdraw] a
                                LEFT JOIN[Usr] b ON a.[Distributor_Id] = b.[Id]) TMP";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition = PrepareCondition(searchModel);

                count = pc.Count;
                return conn.Query<WithdrawInfo>(pc).ToList();
            }
        }
        public MssqlCondition PrepareCondition(WithdrawSearchInfo searchModel)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[Distributor_Id]", searchModel.Distributor_Id, ConditionType.Equal);
            mssqlCondition.Add("[Time]", searchModel.StartTime, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[Time]", searchModel.EndTtime, ConditionType.LessOrEqual);
            mssqlCondition.Add("[Status]", searchModel.Status, ConditionType.Equal);
            mssqlCondition.Add("[UserName]", searchModel.UserName, ConditionType.Like);
            mssqlCondition.Add("[RealName]", searchModel.RealName, ConditionType.Like);
            mssqlCondition.Add("[Mobile]", searchModel.Mobile, ConditionType.Like);
            return mssqlCondition;
        }


    }
}
