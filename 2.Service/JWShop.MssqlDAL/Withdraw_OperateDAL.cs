using System;
using System.Collections.Generic;
using System.Linq;
using JWShop.IDAL;
using System.Configuration;
using Dapper;
using JWShop.Entity;
using System.Data.SqlClient;

namespace JWShop.MssqlDAL
{
    /// <summary>
    /// 提现申请 审核操作记录 数据层
    /// </summary>
    public sealed class Withdraw_OperateDAL : IWithdraw_Operate
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];
        /// <summary>
        /// 根据提现申请Id获取相关操作记录
        /// </summary>
        public List<WithdrawOperateInfo> ReadListByWithdraw(int withdraw_Id)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                string sql = @"SELECT a.*,b.[Name] AS Operator_Name  FROM [Withdraw_Operate]  a
                                LEFT JOIN [Admin] b ON a.[Operator_Id]=b.[Id]
                                WHERE a.[Withdraw_Id]=@Withdraw_Id";
                return conn.Query<WithdrawOperateInfo>(sql, new { @Withdraw_Id = withdraw_Id }).ToList();
            }
        }
    }
}
