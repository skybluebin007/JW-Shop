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
    public sealed class OrderActionBLL : BaseBLL
    {
        private static readonly IOrderAction dal = FactoryHelper.Instance<IOrderAction>(Global.DataProvider, "OrderActionDAL");

        public static int Add(OrderActionInfo entity)
        {
            return dal.Add(entity);
        }

        /// <summary>
        /// 增加一条OrderAction数据（后台管理员）
        /// </summary>
        /// <param name="order">OrderInfo模型变量</param>
        /// <param name="note">备注</param>
        public static int AdminAddOrderAction(int orderId, int startOrderStatus, int endOrderStatus, string note, int orderOperate)
        {
            OrderActionInfo orderAction = new OrderActionInfo();
            orderAction.OrderId = orderId;
            orderAction.OrderOperate = orderOperate;
            orderAction.StartOrderStatus = startOrderStatus;
            orderAction.EndOrderStatus = endOrderStatus;
            orderAction.Note = note;
            orderAction.IP = ClientHelper.IP;
            orderAction.Date = RequestHelper.DateNow;
            orderAction.AdminID = Cookies.Admin.GetAdminID(false);
            orderAction.AdminName = Cookies.Admin.GetAdminName(false);
            orderAction.Id = dal.Add(orderAction);
            return orderAction.Id;
        }

        /// <summary>
        /// 增加一条OrderAction数据（前台用户）
        /// </summary>
        /// <param name="order">OrderInfo模型变量</param>
        /// <param name="note">备注</param>
        public static int UserAddOrderAction(int orderId, int startOrderStatus, int endOrderStatus, string note, int orderOperate)
        {
            OrderActionInfo orderAction = new OrderActionInfo();
            orderAction.OrderId = orderId;
            orderAction.OrderOperate = orderOperate;
            orderAction.StartOrderStatus = startOrderStatus;
            orderAction.EndOrderStatus = endOrderStatus;
            orderAction.Note = note;
            orderAction.IP = ClientHelper.IP;
            orderAction.Date = RequestHelper.DateNow;
            orderAction.AdminID = 0;
            orderAction.AdminName = string.Empty;
            orderAction.Id = dal.Add(orderAction);
            return orderAction.Id;
        }

        public static void Delete(int[] ids)
        {
            dal.Delete(ids);
        }

        public static void DeleteByOrderId(int[] orderIds)
        {
            dal.DeleteByOrderId(orderIds);
        }

        public static OrderActionInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static OrderActionInfo ReadLast(int orderId, int orderStatus)
        {
            return dal.ReadLast(orderId, orderStatus);
        }

        public static List<OrderActionInfo> ReadList(int orderId)
        {
            return dal.ReadList(orderId);
        }

        public static List<OrderActionInfo> ReadListLastDate(int[] orderIds)
        {
            return dal.ReadListLastDate(orderIds);
        }

        /// <summary>
        /// 读取订单操作
        /// </summary>
        /// <param name="orderOperate"></param>
        /// <returns></returns>
        public static string ReadOrderOperate(int orderOperate)
        {
            string result = string.Empty;
            switch (orderOperate)
            {
                case (int)OrderOperate.Pay:
                    result = "付款";
                    break;
                case (int)OrderOperate.Check:
                    result = "审核";
                    break;
                case (int)OrderOperate.Cancle:
                    result = "取消";
                    break;
                case (int)OrderOperate.Send:
                    result = "发货";
                    break;
                case (int)OrderOperate.Received:
                    result = "收货确认";
                    break;
                case (int)OrderOperate.Change:
                    result = "换货确认";
                    break;
                case (int)OrderOperate.Return:
                    result = "退货确认";
                    break;
                case (int)OrderOperate.Back:
                    result = "撤销";
                    break;
                case (int)OrderOperate.Refund:
                    result = "退款";
                    break;
                default:
                    break;
            }
            return result;
        }

    }
}