using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.IDAL;
using JWShop.Entity;
using SkyCES.EntLib;
using JWShop.Common;

namespace JWShop.Business
{
    /// <summary>
    /// 微信 formid 业务逻辑层
    /// </summary>
   public sealed class WxFormIdBLL:BaseBLL
    {
        private static readonly IWxFormId dal = FactoryHelper.Instance<IWxFormId>(Global.DataProvider, "WxFormIdDAL");

        public static int Add(WxFormIdInfo entity)
        {
            return dal.Add(entity);
        }
        /// <summary>
        /// 改变使用状态
        /// </summary>
        /// <param name="id"></param>
        public static void ChangeUsed(int id)
        {
            dal.ChangeUsed(id);
        }
        /// <summary>
        /// 读取指定用户的有效formid 集合(7天内，未使用)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<WxFormIdInfo> ReadUnusedByUserId(int userId)
        {
            return dal.ReadUnusedByUserId(userId);
        }
    }
}
