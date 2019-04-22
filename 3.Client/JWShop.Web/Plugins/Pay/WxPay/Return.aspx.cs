using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Collections;

namespace JWShop.Pay.WxPay
{
    //
    //处理已付款的订单
    //
    public partial class Return : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string out_trade_no = RequestHelper.GetQueryString<string>("order_id");

            string[] orderIds = out_trade_no.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string orderId in orderIds)
            {
                var order = OrderBLL.Read(int.Parse(orderId));
                if (order.Id > 0)
                {
                    #region 正常流程 待付款--》待审核
                    if (order.OrderStatus == (int)OrderStatus.WaitPay)
                    {
                        order.PayKey = "WxPay";
                        order.PayName = "微信支付";
                        order.OrderStatus = (int)OrderStatus.WaitCheck;
                        order.PayDate = RequestHelper.DateNow;
                        OrderBLL.Update(order);

                        //增加操作记录
                        OrderActionInfo orderAction = new OrderActionInfo();
                        orderAction.OrderId = order.Id;
                        orderAction.OrderOperate = (int)OrderOperate.Pay;
                        orderAction.StartOrderStatus = (int)OrderStatus.WaitPay;
                        orderAction.EndOrderStatus = (int)OrderStatus.WaitCheck;
                        orderAction.Note = "客户微信在线支付";
                        orderAction.IP = ClientHelper.IP;
                        orderAction.Date = RequestHelper.DateNow;
                        orderAction.AdminID = 0;
                        orderAction.AdminName = string.Empty;
                        OrderActionBLL.Add(orderAction);
                    }
                    #endregion
                    #region 非正常流程 待付款超时已失效--》待审核
                    if (order.OrderStatus == (int)OrderStatus.NoEffect)
                    {
                        order.PayKey = "WxPay";
                        order.PayName = "微信支付";                       
                        order.OrderStatus = (int)OrderStatus.WaitCheck;
                        order.PayDate = RequestHelper.DateNow;
                        OrderBLL.Update(order);
                        #region 扣除支付积分
                        if (order.Point > 0)
                        {
                            //减少积分
                            UserAccountRecordInfo uarInfo = new UserAccountRecordInfo();
                            uarInfo.RecordType = (int)AccountRecordType.Point;
                            uarInfo.UserId = order.UserId;
                            uarInfo.UserName = order.UserName;
                            uarInfo.Note = "支付订单：" + order.OrderNumber;
                            uarInfo.Point = -order.Point;
                            uarInfo.Money = 0;
                            uarInfo.Date = DateTime.Now;
                            uarInfo.IP = ClientHelper.IP;
                            UserAccountRecordBLL.Add(uarInfo);
                        }
                        #endregion
                        #region 减少商品库存
                        ProductBLL.ChangeOrderCountByOrder(order.Id, ChangeAction.Plus);
                        #endregion

                        //增加操作记录
                        OrderActionInfo orderAction = new OrderActionInfo();
                        orderAction.OrderId = order.Id;
                        orderAction.OrderOperate = (int)OrderOperate.Pay;
                        orderAction.StartOrderStatus = (int)OrderStatus.WaitPay;
                        orderAction.EndOrderStatus = (int)OrderStatus.WaitCheck;
                        orderAction.Note = "客户微信在线支付";
                        orderAction.IP = ClientHelper.IP;
                        orderAction.Date = RequestHelper.DateNow;
                        orderAction.AdminID = 0;
                        orderAction.AdminName = string.Empty;
                        OrderActionBLL.Add(orderAction);
                    }
                    #endregion
                }
            }
        }
    }
}