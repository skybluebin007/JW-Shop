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
    public sealed class OrderBLL : BaseBLL
    {
        private static readonly IOrder dal = FactoryHelper.Instance<IOrder>(Global.DataProvider, "OrderDAL");

        public static int Add(OrderInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(OrderInfo entity)
        {
            dal.Update(entity);
        }

        public static void UpdateIsNoticed(int orderid, int isNoticed)
        {
            dal.UpdateIsNoticed(orderid, isNoticed);
        }
        public static void UpdateIsNoticed(int[] orderids, int isNoticed)
        {
            dal.UpdateIsNoticed(orderids, isNoticed);
        }

        /// <summary>
        /// 更新一条Order数据（后台管理员）
        /// </summary>
        /// <param name="order">Order模型变量</param>
        /// <param name="note">操作备注</param>
        /// <param name="orderOperate">操作</param>
        /// <param name="startOrderStatus">开始状态</param>
        public static void AdminUpdateOrderAddAction(OrderInfo order, string note, int orderOperate, int startOrderStatus)
        {
            dal.Update(order);
            OrderActionBLL.AdminAddOrderAction(order.Id, startOrderStatus, order.OrderStatus, note, orderOperate);
        }

        /// <summary>
        /// 更新一条Order数据（前台用户）
        /// </summary>
        /// <param name="order">Order模型变量</param>
        /// <param name="note">操作备注</param>
        /// <param name="orderOperate">操作</param>
        /// <param name="startOrderStatus">开始状态</param>
        public static void UserUpdateOrderAddAction(OrderInfo order, string note, int orderOperate, int startOrderStatus)
        {
            dal.Update(order);
            OrderActionBLL.UserAddOrderAction(order.Id, startOrderStatus, order.OrderStatus, note, orderOperate);
        }

        public static void Delete(int id)
        {
            OrderDetailBLL.DeleteByOrderId(new int[] { id });
            OrderActionBLL.DeleteByOrderId(new int[] { id });

            dal.Delete(id);
        }

        public static void Delete(int[] ids, int userId)
        {
            OrderDetailBLL.DeleteByOrderId(ids);
            OrderActionBLL.DeleteByOrderId(ids);

            dal.Delete(ids, userId);
        }

        public static OrderInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static OrderInfo Read(int id, int userId)
        {
            return dal.Read(id, userId);
        }

        public static OrderInfo ReadByShopId(int id, int shopId)
        {
            return dal.ReadByShopId(id, shopId);
        }

        public static OrderInfo Read(string orderNumber, int userId)
        {
            return dal.Read(orderNumber, userId);
        }
        public static OrderInfo Read(string orderNumber)
        {
            return dal.Read(orderNumber);
        }
        public static List<OrderInfo> ReadList()
        {
            return dal.ReadList();
        }

        public static List<OrderInfo> ReadList(int[] ids)
        {
            return dal.ReadList(ids);
        }

        public static List<OrderInfo> ReadList(int[] ids, int userId)
        {
            return dal.ReadList(ids, userId);
        }

        public static int ReadCount(int userId)
        {
            return dal.ReadCount(userId);
        }
        /// <summary>
        /// 待付款订单失效检查
        /// </summary>
        /// <param name="ids"></param>
        public static void CheckOrderPayTime(int[] ids)
        {
            dal.CheckOrderPayTime(ids);
            foreach (int orderId in ids) {
                OrderInfo order = Read(orderId);
                #region 待付款状态，退还用户下单时抵现的积分
                if (order.Point > 0)
                {
                    var accountRecord = new UserAccountRecordInfo
                    {
                        RecordType = (int)AccountRecordType.Point,
                        Money = 0,
                        Point = order.Point,
                        Date = DateTime.Now,
                        IP = ClientHelper.IP,
                        Note = "取消订单：" + order.OrderNumber + "，退回用户积分",
                        UserId = order.UserId,
                        UserName = order.UserName,
                    };
                    UserAccountRecordBLL.Add(accountRecord);
                }
                #endregion
                //更新商品库存数量
                ProductBLL.ChangeOrderCountByOrder(order.Id, ChangeAction.Minus);
            }
        }
       
        public static List<OrderInfo> SearchList(OrderSearchInfo searchInfo)
        {
            return dal.SearchList(searchInfo);
        }

        public static List<OrderInfo> SearchList(int currentPage, int pageSize, OrderSearchInfo searchInfo, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchInfo, ref count);
        }
        /// <summary>
        /// 搜索拼团订单
        /// </summary>     
        public static List<OrderInfo> SearchGroupOrderList(int currentPage, int pageSize, OrderSearchInfo searchInfo, ref int count)
        {
            return dal.SearchGroupOrderList(currentPage, pageSize, searchInfo, ref count);
        }
        #region 统计
        /// <summary>
        /// 统计订单状态
        /// </summary>
        public static DataTable StatisticsOrderStatus(OrderSearchInfo orderSearch)
        {
            return dal.StatisticsOrderStatus(orderSearch);
        }

        /// <summary>
        /// 统计订单数量
        /// </summary>
        public static DataTable StatisticsOrderCount(OrderSearchInfo orderSearch, DateType dateType)
        {
            return dal.StatisticsOrderCount(orderSearch, dateType);
        }

        /// <summary>
        /// 统计订单区域
        /// </summary>
        public static DataTable StatisticsOrderArea(OrderSearchInfo orderSearch)
        {
            return dal.StatisticsOrderArea(orderSearch);
        }

        /// <summary>
        /// 统计销售汇总
        /// </summary>
        public static DataTable StatisticsSaleTotal(OrderSearchInfo orderSearch, DateType dateType)
        {
            return dal.StatisticsSaleTotal(orderSearch, dateType);
        }

        /// <summary>
        /// 统计销售汇总
        /// </summary>
        /// <param name="orderSearch"></param>
        /// <returns></returns>
        public static DataTable StatisticsAllTotal(OrderSearchInfo orderSearch)
        {
            return dal.StatisticsAllTotal(orderSearch);
        }
        /// <summary>
        /// 统计已付款汇总
        /// </summary>
        /// <param name="orderSearch"></param>
        /// <returns></returns>
        public static DataTable StatisticsPaidTotal(OrderSearchInfo orderSearch)
        {
            return dal.StatisticsPaidTotal(orderSearch);
        }
        /// <summary>
        /// 统计滞销分析
        /// </summary>
        public static DataTable StatisticsSaleStop(string productIds)
        {
            return dal.StatisticsSaleStop(productIds);
        }

        public static List<OrderInfo> ReadPreNextOrder(int id)
        {
            return dal.ReadPreNextOrder(id);
        }
        #endregion

        /// <summary>
        /// 读取订单状态
        /// </summary>
        public static string ReadOrderStatus(int orderStatus)
        {
            string result = string.Empty;
            switch (orderStatus)
            {
                case (int)OrderStatus.WaitPay:
                    result = "待付款";
                    break;
                case (int)OrderStatus.WaitCheck:
                    result = "待审核";
                    break;
                case (int)OrderStatus.NoEffect:
                    result = "无效";
                    break;
                case (int)OrderStatus.Shipping:
                    result = "安装";
                    break;
                case (int)OrderStatus.HasShipping:
                    result = "试压";
                    break;
                case (int)OrderStatus.ReceiveShipping:
                    result = "已完成";
                    break;
                case (int)OrderStatus.HasReturn:
                    result = "已退货";
                    break;
                case (int)OrderStatus.HasDelete:
                    result = "已删除";
                    break;
                default:
                    break;
            }
            return result;
        }
         /// <summary>
        /// 读取订单状态,包含逻辑删除
        /// </summary>
        public static string ReadOrderStatus(int orderStatus,int isDelete)
        {
            string result = string.Empty;
            if (isDelete == (int)BoolType.True)
            {
                result = "已删除";
            }
            else
            {
                switch (orderStatus)
                {
                    case (int)OrderStatus.WaitPay:
                        result = "待付款";
                        break;
                    case (int)OrderStatus.WaitCheck:
                        result = "待审核";
                        break;
                    case (int)OrderStatus.NoEffect:
                        result = "无效";
                        break;
                    case (int)OrderStatus.Shipping:
                        result = "安装";
                        break;
                    case (int)OrderStatus.HasShipping:
                        result = "试压";
                        break;
                    case (int)OrderStatus.ReceiveShipping:
                        result = "已完成";
                        break;
                    case (int)OrderStatus.HasReturn:
                        result = "已退货";
                        break;
                    case (int)OrderStatus.HasDelete:
                        result = "已删除";
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
        
        /// <summary>
        /// 读取订单用户操作
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="checkStatus">订单状态</param>
        /// <param name="payKey">支付方式关键字</param>
        /// <returns></returns>
        public static string ReadOrderUserOperate(int orderId, int orderStatus, string payKey, int userId)
        {
            string result = string.Empty;
            OrderInfo order = OrderBLL.Read(orderId);
            if (order.IsDelete == (int)BoolType.False)
            {
                switch (orderStatus)
                {
                    case (int)OrderStatus.WaitPay:
                        if (payKey == "WxPay")
                        {
                            if (RequestHelper.UserAgent() && RequestHelper.IsMicroMessenger())
                            {
                                result += " <a href=\"/Plugins/Pay/WxPay/Pay.aspx?order_id=" + orderId.ToString() + "\" target=\"_blank\" class=\"red\">付款</a>";
                            }
                            else
                            {
                                result += " <a href=\"/finish.html?ID=" + orderId.ToString() + "\" class=\"red\" target=\"_blank\">付款</a>";
                            }
                        }
                        else
                        {
                            if (PayPlugins.ReadPayPlugins(payKey).IsOnline == (int)BoolType.True)
                            {
                                result = " <a href=\"/Plugins/Pay/" + payKey + "/Pay.aspx?order_id=" + orderId.ToString() + "\" target=\"_blank\" class=\"red\">付款</a>";
                            }
                        }
                        result += "<a href=\"javascript:orderOperate(" + orderId + "," + (int)OrderOperate.Cancle + ")\" class=\"red\">取消</a>";
                        break;
                    case (int)OrderStatus.WaitCheck:
                        if (PayPlugins.ReadPayPlugins(payKey).IsOnline != (int)BoolType.True)
                        {
                            result = "<a href=\"javascript:orderOperate(" + orderId + "," + (int)OrderOperate.Cancle + ")\"  class=\"red\">取消</a>";
                        }
                        break;
                    case (int)OrderStatus.NoEffect:
                    case (int)OrderStatus.Shipping:
                        break;
                    case (int)OrderStatus.HasShipping:
                        result = "<a href=\"/User/ShippingList.html?OrderID=" + orderId + "\" class=\"red\">查看物流</a>";
                        result += "   <a href=\"javascript:orderOperate(" + orderId + "," + (int)OrderOperate.Received + ")\" class=\"red\">确定收货</a>";
                        break;
                    case (int)OrderStatus.ReceiveShipping:
                        //if (!ProductCommentBLL.HasCommented(orderId, userId))
                        //{
                        //    result = "<a href=\"/user/userproductcommentadd.html?orderid=" + orderId + "\" title=\"订单评价\" class=\"a1\">评价</a>";
                        //}
                        break;
                    case (int)OrderStatus.HasReturn:
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
        /// <summary>
        /// 手机读取订单用户操作
        /// </summary>
        /// <param name="orderID">订单ID</param>
        /// <param name="checkStatus">订单状态</param>
        /// <param name="payKey">支付方式关键字</param>
        /// <returns></returns>
        public static string ReadOrderUserOperate2(int orderID, int orderStatus, string payKey)
        {
            string result = string.Empty;
            OrderInfo order = OrderBLL.Read(orderID);
             if (order.IsDelete == (int)BoolType.False)
             {
                 switch (orderStatus)
                 {
                     case (int)OrderStatus.WaitPay:
                         result = "<a href=\"javascript:orderOperate(" + orderID + "," + (int)OrderOperate.Cancle + ")\">取消</a>";
                         if (payKey == "WxPay")
                         {
                             if (RequestHelper.UserAgent() && RequestHelper.IsMicroMessenger())
                             {
                                 result += " <a href=\"/Plugins/Pay/WxPay/Pay.aspx?order_id=" + orderID.ToString() + "\" class=\"red\" style=\"background-color:#da251a;color:#fff;\">付款</a>";
                             }
                             else
                             {
                                 result += " <a href=\"/mobile/finish.html?ID=" + orderID.ToString() + "\" class=\"red\" style=\"background-color:#da251a;color:#fff;\">付款</a>";
                             }
                         }
                         else
                         {
                             if (PayPlugins.ReadPayPlugins(payKey).IsOnline == (int)BoolType.True)
                             {
                                 result += " <a href=\"/Plugins/Pay/" + payKey + "/Pay.aspx?order_id=" + orderID.ToString() + "\" class=\"red\" style=\"background-color:#da251a;color:#fff;\">付款</a>";
                             }
                         }
                         break;
                     case (int)OrderStatus.WaitCheck:
                         if (PayPlugins.ReadPayPlugins(payKey).IsOnline != (int)BoolType.True)
                         {
                             result = "<a href=\"javascript:orderOperate(" + orderID + "," + (int)OrderOperate.Cancle + ")\" >取消</a>";
                         }
                         break;
                     case (int)OrderStatus.NoEffect:
                     case (int)OrderStatus.Shipping:
                         break;
                     case (int)OrderStatus.HasShipping:
                         result = "<a href=\"/mobile/User/ShippingList.html?OrderID=" + orderID + "\" class=\"red\">查看物流</a>";
                         result += "<a href=\"javascript:orderOperate(" + orderID + "," + (int)OrderStatus.HasShipping + ")\">确定收货</a>";
                         break;
                     case (int)OrderStatus.ReceiveShipping:
                         result = "<a href=\"/Mobile/user/CommentOrderProduct-O" + orderID + ".html\" >评论</a>";
                         break;
                     case (int)OrderStatus.HasReturn:
                         break;
                     default:
                         break;
                 }
             }
            return result;
        }
        public static string ReadOrderUserOperate3(int orderID, int orderStatus, string payKey)
        {
            string result = string.Empty;
            OrderInfo order = OrderBLL.Read(orderID);
             if (order.IsDelete == (int)BoolType.False)
             {
                 switch (orderStatus)
                 {
                     case (int)OrderStatus.WaitPay:
                         result = "<a href=\"javascript:orderOperate(" + orderID + "," + (int)OrderOperate.Cancle + ")\">取消</a>";
                         if (payKey == "WxPay")
                         {
                             result += " <a href=\"/Plugins/Pay/WxPay/Pay.aspx?order_id=" + orderID.ToString() + "\" target=\"_blank\"  style=\"display:inline-block;background-color:#da251a;padding:0px 3px;color:#fff;\">付款</a>";

                         }
                         else
                         {
                             if (PayPlugins.ReadPayPlugins(payKey).IsOnline == (int)BoolType.True)
                             {
                                 result += " <a href=\"/Plugins/Pay/" + payKey + "/Pay.aspx?order_id=" + orderID.ToString() + "\" target=\"_blank\" style=\"display:inline-block;background-color:#da251a;padding:0px 3px;color:#fff;\">付款</a>";
                             }
                         }
                         break;
                     case (int)OrderStatus.WaitCheck:
                         if (PayPlugins.ReadPayPlugins(payKey).IsOnline != (int)BoolType.True)
                         {
                             result = "<a href=\"javascript:orderOperate(" + orderID + "," + (int)OrderOperate.Cancle + ")\" >取消</a>";
                         }
                         break;
                     case (int)OrderStatus.NoEffect:
                     case (int)OrderStatus.Shipping:
                         break;
                     case (int)OrderStatus.HasShipping:
                         result = "<a href=\"javascript:orderOperate(" + orderID + "," + (int)OrderStatus.HasShipping + ")\">确定收货</a>";
                         break;
                     case (int)OrderStatus.ReceiveShipping:
                         result = "<a href=\"/Mobile/User/CommentOrderProduct-O" + orderID + ".html\" target\"_blank\">评论</a>";
                         break;
                     case (int)OrderStatus.HasReturn:
                         break;
                     default:
                         break;
                 }
             }
            return result;
        }
        
        /// <summary>
        /// 读取订单总金额
        /// </summary>
        public static decimal ReadOrderMoney(OrderInfo order)
        {
            return order.ProductMoney + order.ShippingMoney + order.OtherMoney;
        }
        /// <summary>
        /// 读取已付金额
        /// </summary>
        public static decimal ReadHasPaidMoney(OrderInfo order)
        {
            return order.Balance + order.CouponMoney + order.FavorableMoney+order.PointMoney;
        }
        /// <summary>
        /// 读取未付金额
        /// </summary>
        public static decimal ReadNoPayMoney(OrderInfo order)
        {
            return ReadOrderMoney(order) - ReadHasPaidMoney(order);
        }

        /// <summary>
        /// 读取订单赠送积分
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int ReadOrderSendPoint(int id)
        {
            int result = 0;
            foreach (OrderDetailInfo orderDetail in OrderDetailBLL.ReadList(id))
            {
                result += orderDetail.SendPoint * orderDetail.BuyCount;
            }
            return result;
        }
        
        /// <summary>
        /// 读取订单商品价格
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static decimal ReadOrderProductPrice(int id)
        {
            decimal result = 0;
            foreach (OrderDetailInfo orderDetail in OrderDetailBLL.ReadList(id))
            {
                result += orderDetail.ProductPrice * orderDetail.BuyCount;
            }
            return result;
        }

        /// <summary>
        /// 计算订单的邮费
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static decimal ReadOrderShippingMoney(OrderInfo order)
        {
            decimal shippingMoney = order.ShippingMoney;
            ShippingInfo shipping = ShippingBLL.Read(order.ShippingId);
            ShippingRegionInfo shippingRegion = ShippingRegionBLL.SearchShippingRegion(order.ShippingId, order.RegionId);
            switch (shipping.ShippingType)
            {
                case (int)ShippingType.Fixed:
                    shippingMoney = shippingRegion.FixedMoeny;
                    break;
                case (int)ShippingType.Weight:
                    decimal orderProductWeight = 0;
                    foreach (OrderDetailInfo orderDetail in OrderDetailBLL.ReadList(order.Id))
                    {
                        orderProductWeight += orderDetail.ProductWeight * orderDetail.BuyCount;
                    }
                    if (orderProductWeight <= shipping.FirstWeight)
                    {
                        shippingMoney = shippingRegion.FirstMoney;
                    }
                    else
                    {
                        shippingMoney = shippingRegion.FirstMoney + Math.Ceiling((orderProductWeight - shipping.FirstWeight) / shipping.AgainWeight) * shippingRegion.AgainMoney;
                    }
                    break;
                case (int)ShippingType.ProductCount:
                    int orderProductCount = 0;
                    foreach (OrderDetailInfo orderDetail in OrderDetailBLL.ReadList(order.Id))
                    {
                        if (orderDetail.ParentId == 0)
                        {
                            orderProductCount += orderDetail.BuyCount;
                        }
                    }
                    shippingMoney = shippingRegion.OneMoeny + (orderProductCount - 1) * shippingRegion.AnotherMoeny;
                    break;
                default:
                    break;
            }
            return shippingMoney;
        }

        /// <summary>
        /// 检查待付款订单是否超时失效，已超时则更新为失效状态
        /// </summary>
        public static void CheckOrderPayTime()
        {
            int orderPayTime = ShopConfig.ReadConfigInfo().OrderPayTime;
            #region 存储过程
            //付款时限>0才启用
            if (orderPayTime > 0)
            {
                dal.CheckOrderPayTimeProg();
            }
            #endregion
            #region Dapper ORM
            //int orderPayTime = ShopConfig.ReadConfigInfo().OrderPayTime;
            ////付款时限>0才启用
            //if (orderPayTime > 0)
            //{
            //    //待付款且未删除的订单
            //    List<OrderInfo> orderList = SearchList(new OrderSearchInfo { OrderStatus = (int)OrderStatus.WaitPay });
            //    if (orderList.Count > 0)
            //    {
            //        int[] Ids = orderList.Where(k => k.AddDate.AddHours(orderPayTime) <= DateTime.Now).Select(k => k.Id).ToArray();
            //        CheckOrderPayTime(Ids);
            //    }
            //}
            #endregion
        }
        /// <summary>
        /// 检查指定用户的待付款订单是否超时失效，已超时则更新为失效状态
        /// </summary>
        public static void CheckOrderPayTime(int userId)
        {
            int orderPayTime = ShopConfig.ReadConfigInfo().OrderPayTime;
            #region Dapper ORM
            ////付款时限>0才启用
            //if (orderPayTime > 0)
            //{
            //    //待付款且未删除的订单
            //    List<OrderInfo> orderList = SearchList(new OrderSearchInfo { OrderStatus = (int)OrderStatus.WaitPay,UserId=userId });
            //    if (orderList.Count > 0)
            //    {
            //        int[] Ids = orderList.Where(k => k.AddDate.AddHours(orderPayTime) <= DateTime.Now).Select(k => k.Id).ToArray();
            //        CheckOrderPayTime(Ids);
            //    }
            //}
            #endregion
            #region 存储过程
              if (orderPayTime > 0)
            {
               dal.CheckOrderPayTimeProg(userId);
              }
           
            #endregion

        }
        /// <summary>
        /// 待收货订单自动收货--所有订单--存储过程
        /// </summary>       
        public static void CheckOrderRecieveTimeProg()
        {
            if (ShopConfig.ReadConfigInfo().OrderRecieveShippingDays > 0)
            {
                dal.CheckOrderRecieveTimeProg();
            }
        }

        /// <summary>
        /// 待收货订单自动收货--指定订单--存储过程
        /// </summary>       
       public static void CheckOrderRecieveTimeProg(int userId)
        {
            if (ShopConfig.ReadConfigInfo().OrderRecieveShippingDays > 0)
            {
                dal.CheckOrderRecieveTimeProg(userId);
            }
        }
        /// <summary>
        /// 启用不限库存后，获取商品单日销量
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public static int GetProductOrderCountDaily(int productId, int productStandType, DateTime date, string standardValueList = null)
        {
            return dal.GetProductOrderCountDaily(productId, productStandType,  date, standardValueList);
        }
    }
}