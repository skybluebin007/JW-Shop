using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Business
{
    public sealed class OrderRefundActionBLL : BaseBLL
    {
        private static readonly IOrderRefundAction dal = FactoryHelper.Instance<IOrderRefundAction>(Global.DataProvider, "OrderRefundActionDAL");

        public static int Add(OrderRefundActionInfo entity)
        {
            return dal.Add(entity);
        }

        public static List<OrderRefundActionInfo> ReadList(int orderRefundId)
        {
            return dal.ReadList(orderRefundId);
        }

    }
}