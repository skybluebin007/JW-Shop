using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Web.Admin
{
    public partial class OrderAjax : JWShop.Page.AdminBasePage
    {
        protected OrderInfo order = new OrderInfo();
        protected List<OrderDetailInfo> orderDetailList = new List<OrderDetailInfo>();
        protected bool canEdit = false;
        protected int totalProductCount = 0;
        protected decimal totalWeight = 0;
        protected int totalPoint = 0;
        protected List<ProductInfo> productList = new List<ProductInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            ClearCache();

            //订单产品操作
            string action = RequestHelper.GetQueryString<string>("Action");
            if (!string.IsNullOrEmpty(action))
            {
                int tempOrderId = RequestHelper.GetQueryString<int>("OrderId");
                OrderInfo tempOrder = OrderBLL.Read(tempOrderId);
                int isCod = PayPlugins.ReadPayPlugins(tempOrder.PayKey).IsCod;
                if ((tempOrder.OrderStatus == (int)OrderStatus.WaitPay || tempOrder.OrderStatus == (int)OrderStatus.WaitCheck && isCod == (int)BoolType.True) && (tempOrder.IsActivity == (int)OrderKind.Common || tempOrder.IsActivity == (int)OrderKind.GroupBuy))
                {
                    switch (action)
                    {
                        case "DeleteOrderProduct":
                            DeleteOrderProduct();
                            break;
                        case "ChangeOrderProductBuyCount":
                            ChangeOrderProductBuyCount();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    ResponseHelper.Write("订单已经审核，无法修改");
                    ResponseHelper.End();
                }
            }

            //读取订单产品
            int orderId = RequestHelper.GetQueryString<int>("Id");
            if (orderId != int.MinValue)
            {
                CheckAdminPower("ReadOrder", PowerCheckType.Single);
                order = OrderBLL.Read(orderId);
                int isCod = PayPlugins.ReadPayPlugins(order.PayKey).IsCod;
                if ((order.OrderStatus == (int)OrderStatus.WaitPay || order.OrderStatus == (int)OrderStatus.WaitCheck && isCod == (int)BoolType.True) && (order.IsActivity == (int)OrderKind.Common || order.IsActivity==(int)OrderKind.GroupBuy) )
                {
                    canEdit = true;
                }
                orderDetailList = OrderDetailBLL.ReadList(orderId);

                foreach (OrderDetailInfo orderDetail in orderDetailList)
                {
                    totalProductCount += orderDetail.BuyCount;
                    totalWeight += orderDetail.BuyCount * orderDetail.ProductWeight;
                    totalPoint += orderDetail.BuyCount * orderDetail.SendPoint;
                }
            }
        }

        /// <summary>
        /// 删除订单中的商品
        /// </summary>
        protected void DeleteOrderProduct()
        {
            int strOrderDetailID = RequestHelper.GetQueryString<int>("StrOrderDetailID");
            int strProductID = RequestHelper.GetQueryString<int>("StrProductID");
            int oldCount = RequestHelper.GetQueryString<int>("OldCount");
            int orderId = RequestHelper.GetQueryString<int>("OrderID");
            ProductBLL.ChangeOrderCount(strProductID, oldCount);
            OrderDetailBLL.Delete(strOrderDetailID);
            OrderUpdateHanlder(orderId);
            ResponseHelper.End();
        }

        /// <summary>
        /// 改变订单商品的数量
        /// </summary>
        protected void ChangeOrderProductBuyCount()
        {
            int strOrderDetailID = RequestHelper.GetQueryString<int>("StrOrderDetailID");
            int buyCount = RequestHelper.GetQueryString<int>("BuyCount");
            int strProductID = RequestHelper.GetQueryString<int>("StrProductID");
            int oldCount = RequestHelper.GetQueryString<int>("OldCount");
            int orderId = RequestHelper.GetQueryString<int>("OrderID");
            ProductBLL.ChangeOrderCount(strProductID, oldCount - buyCount);
            OrderDetailBLL.ChangeBuyCount(strOrderDetailID, buyCount);
            OrderUpdateHanlder(orderId);
            ResponseHelper.End();
        }

        /// <summary>
        /// 调整产品的价格，运费
        /// </summary>
        protected void OrderUpdateHanlder(int orderId)
        {
            OrderInfo order = OrderBLL.Read(orderId);
            order.ProductMoney = OrderBLL.ReadOrderProductPrice(orderId);
            order.ShippingMoney = OrderBLL.ReadOrderShippingMoney(order);
            OrderBLL.Update(order);
        }
    }
}