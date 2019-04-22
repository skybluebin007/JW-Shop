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
    public sealed class OrderRefundPhotoBLL : BaseBLL
    {
        private static readonly IOrderRefundPhoto dal = FactoryHelper.Instance<IOrderRefundPhoto>(Global.DataProvider, "OrderRefundPhotoDAL");

        public static int Add(OrderRefundPhotoInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
        }

        public static OrderRefundPhotoInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static List<OrderRefundPhotoInfo> ReadList(int orderRefundId)
        {
            return dal.ReadList(orderRefundId);
        }

    }
}