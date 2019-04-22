using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Entity;

namespace JWShop.IDAL
{
    /// <summary>
    /// 提货码  接口层
    /// </summary>
   public interface IPickUpCode
    {
        int Add(PickUpCodeInfo entity);
        PickUpCodeInfo Read(int id);
        PickUpCodeInfo ReadByOrderId(int orderId);
        /// <summary>
        /// 提货码是否唯一，true:是
        /// </summary>
        /// <param name="pickCode"></param>
        /// <returns></returns>
        bool UniqueCheck(string pickCode);
        /// <summary>
        /// 使用提货码，状态置为已使用status=1
        /// </summary>
        /// <param name="id"></param>
        int UsePickCode(int id);
        /// <summary>
        /// 订单审核，使用提货码，状态置为已使用status=1
        /// </summary>
        /// <param name="id"></param>
        int UsePickCodeByOrder(int orderId);
        OrderInfo ReadByPickCode(string pickUpCode, ref int checkCode);
    }
}
