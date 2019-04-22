using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using JWShop.Entity;
using System.Data.SqlClient;
using Dapper;
using JWShop.IDAL;
using System.Data;

namespace JWShop.MssqlDAL
{
    /// <summary>
    /// 提货码  数据层
    /// </summary>
    public sealed class PickUpCodeDAL : IPickUpCode
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(PickUpCodeInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"Insert Into [PickUpcode]([OrderId],[PickCode],[Status]) Values(@OrderId,@PickCode,@Status);
                                Select SCOPE_IDENTITY()";
                return conn.Query<int>(sql, entity).Single();
            }
        }
        public PickUpCodeInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "Select * from [PickUpCode] where id=@id";
                return conn.Query<PickUpCodeInfo>(sql, new { id = id }).SingleOrDefault() ?? new PickUpCodeInfo();
            }
        }
        public PickUpCodeInfo ReadByOrderId(int orderId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "SELECT * FROM [PickUpCode] WHERE [OrderId]=@OrderId AND [Status]=0";
                return conn.Query<PickUpCodeInfo>(sql, new { OrderId = orderId }).SingleOrDefault() ?? new PickUpCodeInfo();
            }
        }
        /// <summary>
        /// 提货码是否唯一，true:是
        /// </summary>
        /// <param name="pickCode"></param>
        /// <returns></returns>
        public bool UniqueCheck(string pickCode)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "Select count(1) from [PickUpCode] where [PickCode]=@PickCode and [Status]=0";
                return conn.Query<int>(sql, new { PickCode = pickCode }).Single() <= 0;
            }
        }
        /// <summary>
        /// 使用提货码，状态置为已使用status=1
        /// </summary>
        /// <param name="id"></param>
        public int UsePickCode(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "Update [PickUpCode] set [Status]=1 where id=@id";
                return conn.Execute(sql, new { id = id });
            }
        }
        /// <summary>
        /// 订单审核，使用提货码，状态置为已使用status=1
        /// </summary>
        /// <param name="id"></param>
        public int UsePickCodeByOrder(int orderId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "Update [PickUpCode] set [Status]=1 where [OrderId]=@OrderId AND [Status]=0";
                return conn.Execute(sql, new { OrderId = orderId });
            }
        }
        /// <summary>
        /// 根据提货码读取订单信息
        /// </summary>
        /// <param name="pickUpCode"></param>
        /// <param name="checkCode"></param>
        /// <returns></returns>
        public OrderInfo ReadByPickCode(string pickUpCode, ref int checkCode)
        {
            using (var conn = new SqlConnection(connectString))
            {
                OrderInfo entity = new OrderInfo();
                var param = new DynamicParameters();
                param.Add("@pickUpCode", pickUpCode);
                param.Add("@result", 0, DbType.Int32, ParameterDirection.Output);
                using (var multi = conn.QueryMultiple("WriteOffPickUpCode", param, null, null, CommandType.StoredProcedure))
                {

                    //multi.IsConsumed   reader的状态 ，true 是已经释放
                    if (!multi.IsConsumed)
                    {//如果提货码有效                      
                        try
                        {
                            entity = multi.Read<OrderInfo>().FirstOrDefault() ?? new OrderInfo();
                        }
                        catch (Exception ex)
                        {
                            //multi rowscount=0
                        }
                    }
                }
                // at this point we have consumed the TDS data, so the parameter
                // values have come back to the caller
                checkCode = param.Get<int>("@result");//获取数据库输出的值
                return entity;
            }
        }
    }
}
