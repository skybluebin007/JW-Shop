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
    public sealed class OrderDetailBLL : BaseBLL
    {
        private static readonly IOrderDetail dal = FactoryHelper.Instance<IOrderDetail>(Global.DataProvider, "OrderDetailDAL");

        public static int Add(OrderDetailInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(OrderDetailInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
        }

        public static void Delete(int[] ids)
        {
            dal.Delete(ids);
        }

        public static void DeleteByOrderId(int[] orderIds)
        {
            dal.DeleteByOrderId(orderIds);
        }

        public static OrderDetailInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static List<OrderDetailInfo> ReadList(int orderId)
        {
            return dal.ReadList(orderId);
        }

        public static List<OrderDetailInfo> ReadListByProductId(int productId)
        {
            return dal.ReadListByProductId(productId);
        }

        /// <summary>
        /// 改变购买数量
        /// </summary>
        public static void ChangeBuyCount(int id, int buyCount)
        {
            dal.ChangeBuyCount(id, buyCount);
        }

        /// <summary>
        /// 改变退款数量
        /// </summary>
        public static void ChangeRefundCount(int id, int refundCount, ChangeAction action)
        {
            dal.ChangeRefundCount(id, refundCount, action);
        }

        public static DataTable StatisticsSaleDetail(int currentPage, int pageSize, OrderSearchInfo orderSearch, ProductSearchInfo productSearch, ref int count)
        {
            return dal.StatisticsSaleDetail(currentPage, pageSize, orderSearch, productSearch, ref count);
        }

        public static int GetOrderCount(int productId, string standardValueList)
        {
            return dal.GetOrderCount(productId,standardValueList);
        }

    }
}