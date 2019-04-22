using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using JWShop.Common;

namespace JWShop.Business
{
    /// <summary>
    /// 提货码  业务逻辑层
    /// </summary>
    public sealed class PickUpCodeBLL
    {
        private static readonly IPickUpCode dal = FactoryHelper.Instance<IPickUpCode>(Global.DataProvider, "PickUpCodeDAL");

        public static int Add(PickUpCodeInfo entity)
        {
            return dal.Add(entity);
        }
        public static PickUpCodeInfo Read(int id)
        {
            return dal.Read(id);
        }
        public static PickUpCodeInfo ReadByOrderId(int orderId)
        {
            return dal.ReadByOrderId(orderId);
        }
        /// <summary>
        /// 提货码是否唯一，true:是
        /// </summary>
        /// <param name="pickCode"></param>
        /// <returns></returns>
        public static bool UniqueCheck(string pickCode)
        {
            return dal.UniqueCheck(pickCode);
        }
        /// <summary>
        /// 使用提货码，状态置为已使用status=1
        /// </summary>
        /// <param name="id"></param>
        public static int UsePickCode(int id)
        {
            return dal.UsePickCode(id);
        }
        /// <summary>
        /// 订单审核，使用提货码，状态置为已使用status=1
        /// </summary>
        /// <param name="id"></param>
        public static int UsePickCodeByOrder(int orderId)
        {
            return dal.UsePickCodeByOrder(orderId);
        }
        /// <summary>
        /// 生成唯一（有效状态的提货码中唯一）提货码
        /// </summary>
        public static string CreatePickUpCode()
        {
            Random rd = new Random();
            string result = rd.Next(100000, 999999).ToString();
            //如果提货码不唯一，则继续生成           
            while (!UniqueCheck(result))
            { result = rd.Next(100000, 999999).ToString(); }
            return result;
        }
        /// <summary>
        /// 根据提货码读取订单信息
        /// </summary>
        /// <param name="pickUpCode"></param>
        /// <param name="checkCode">返回状态码：1--有效，0--无效</param>
        /// <returns></returns>
        public static OrderInfo ReadByPickCode(string pickUpCode, ref int checkCode)
        {
            return dal.ReadByPickCode(pickUpCode, ref checkCode);
        }
    }
}
