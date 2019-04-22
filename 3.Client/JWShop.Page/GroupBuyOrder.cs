using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SocoShop.Common;
using SocoShop.Business;
using SocoShop.Entity;
using SkyCES.EntLib;

namespace SocoShop.Page
{
    public class GroupBuyOrder : CommonBasePage
    {
        /// <summary>
        /// 用户地址列表
        /// </summary>
        protected List<UserAddressInfo> userAddressList = new List<UserAddressInfo>();
        /// <summary>
        /// 支付方式列表
        /// </summary>
        protected List<PayPluginsInfo> payPluginsList = new List<PayPluginsInfo>();
        protected string result = string.Empty;
        protected GroupBuyInfo groupBuy = new GroupBuyInfo();
        protected ProductInfo product = new ProductInfo();
        protected int buyCount = 0;
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            int groupID = RequestHelper.GetQueryString<int>("ID");
            buyCount = RequestHelper.GetQueryString<int>("BuyCount");

            groupBuy = GroupBuyBLL.ReadGroupBuy(groupID);
            if (groupBuy.ID <= 0)
            {
                result = "该团购不存在！";
                return;
            }
            if (UserGroupBuyBLL.ReadUserGroupBuyByUser(groupID, base.UserID).ID > 0)
            {
                result = "您已经参加该团购了！";
                return;
            }
            if (groupBuy.StartDate > DateTime.Now)
            {
                result = "该团购还未开始，不能购买！";
                return;
            }
            if (groupBuy.EndDate < DateTime.Now)
            {
                result = "该团购已经结束，不能购买！";
                return;
            }
            if (buyCount <= 0)
            {
                result = "购买数量有误！";
                return;
            }
            if (buyCount > groupBuy.EachNumber)
            {
                result = "购买数量超过了该团购个人限购数！";
                return;
            }
            int hasBuy = 0;
            foreach (UserGroupBuyInfo userGroupBuy in UserGroupBuyBLL.ReadUserGroupBuyList(groupID))
            {
                hasBuy += userGroupBuy.BuyCount;
            }
            if (buyCount > (groupBuy.MaxCount - hasBuy))
            {
                result = "购买数量超过了该团购剩余数！";
                return;
            }
            product = ProductBLL.ReadProduct(groupBuy.ProductID);

            //登录验证
            if (ShopConfig.ReadConfigInfo().AllowAnonymousAddCart == (int)BoolType.False && base.UserID == 0)
            {
                ResponseHelper.Redirect("/User/Login.aspx?RedirectUrl=/GroupBuyOrder-" + groupID + "-" + buyCount + ".aspx");
                ResponseHelper.End();
            }
            //读取数据
            if (base.UserID > 0)
            {
                userAddressList = UserAddressBLL.ReadUserAddressByUser(base.UserID);
            }
            payPluginsList = PayPlugins.ReadProductBuyPayPluginsList();
            Title = "团购订单";
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        protected override void PostBack()
        {
            int groupID = RequestHelper.GetForm<int>("groupID");
            buyCount = RequestHelper.GetForm<int>("buyCount");
            string url = "/GroupBuyOrder-" + groupID + "-" + buyCount + ".aspx";

            groupBuy = GroupBuyBLL.ReadGroupBuy(groupID);
            if (groupBuy.ID <= 0)
            {
                ScriptHelper.AlertFront("该团购不存在！", url);
            }
            if (UserGroupBuyBLL.ReadUserGroupBuyByUser(groupID, base.UserID).ID > 0)
            {
                ScriptHelper.AlertFront("您已经参加该团购了！", url);
            }
            if (groupBuy.StartDate > DateTime.Now)
            {
                ScriptHelper.AlertFront("该团购还未开始，不能购买！", url);
            }
            if (groupBuy.EndDate < DateTime.Now)
            {
                ScriptHelper.AlertFront("该团购已经结束，不能购买！", url);
            }
            if (buyCount <= 0)
            {
                ScriptHelper.AlertFront("购买数量有误！", url);
            }
            if (buyCount > groupBuy.EachNumber)
            {
                ScriptHelper.AlertFront("购买数量超过了该团购个人限购数！", url);
            }
            int hasBuy = 0;
            foreach (UserGroupBuyInfo userGroupBuy in UserGroupBuyBLL.ReadUserGroupBuyList(groupID))
            {
                hasBuy += userGroupBuy.BuyCount;
            }
            if (buyCount > (groupBuy.MaxCount - hasBuy))
            {
                ScriptHelper.AlertFront("购买数量超过了该团购剩余数！", url);
            }
            product = ProductBLL.ReadProduct(groupBuy.ProductID);

            //检查地址
            string consignee = StringHelper.AddSafe(RequestHelper.GetForm<string>("Consignee"));
            if (consignee == string.Empty)
            {
                ScriptHelper.AlertFront("收货人姓名不能为空", url);
            }
            string tel = StringHelper.AddSafe(RequestHelper.GetForm<string>("Tel"));
            string mobile = StringHelper.AddSafe(RequestHelper.GetForm<string>("Mobile"));
            if (tel == string.Empty && mobile == string.Empty)
            {
                ScriptHelper.AlertFront("固定电话，手机必须得填写一个", url);
            }
            string zipCode = StringHelper.AddSafe(RequestHelper.GetForm<string>("ZipCode"));
            if (zipCode == string.Empty)
            {
                ScriptHelper.AlertFront("邮编不能为空", url);
            }
            string address = StringHelper.AddSafe(RequestHelper.GetForm<string>("Address"));
            if (address == string.Empty)
            {
                ScriptHelper.AlertFront("地址不能为空", url);
            }
            //验证配送方式
            int shippingID = RequestHelper.GetForm<int>("ShippingID");
            if (shippingID == int.MinValue)
            {
                ScriptHelper.AlertFront("请选择配送方式", url);
            }
            //检查金额
            decimal productMoney = groupBuy.Price * buyCount;
            decimal favorableMoney = RequestHelper.GetForm<decimal>("FavorableMoney");
            decimal shippingMoney = RequestHelper.GetForm<decimal>("ShippingMoney");
            decimal balance = RequestHelper.GetForm<decimal>("Balance");
            decimal couponMoney = RequestHelper.GetForm<decimal>("CouponMoney");
            if (productMoney - favorableMoney + shippingMoney - balance - couponMoney < 0)
            {
                ScriptHelper.AlertFront("金额有错误，请重新检查", url);
            }
            //支付方式
            string payKey = RequestHelper.GetForm<string>("Pay");
            PayPluginsInfo payPlugins = PayPlugins.ReadPayPlugins(payKey);
            //添加订单
            OrderInfo order = new OrderInfo();
            order.OrderNumber = ShopCommon.CreateOrderNumber();
            order.IsActivity = (int)BoolType.True;
            if (productMoney - favorableMoney + shippingMoney - balance - couponMoney == 0 || payPlugins.IsCod == (int)BoolType.True)
            {
                order.OrderStatus = (int)OrderStatus.WaitCheck;
            }
            else
            {
                order.OrderStatus = (int)OrderStatus.WaitPay;
            }
            order.OrderNote = "团购活动：" + groupBuy.Name;
            order.ProductMoney = productMoney;
            order.Balance = balance;
            order.FavorableMoney = favorableMoney;
            order.OtherMoney = 0;
            order.CouponMoney = couponMoney;
            order.Consignee = consignee;
            SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();
            order.RegionID = singleUnlimitClass.ClassID;
            order.Address = address;
            order.ZipCode = zipCode;
            order.Tel = tel;
            string userEmail = string.Empty;
            if (base.UserID == 0)
            {
                userEmail = StringHelper.AddSafe(RequestHelper.GetForm<string>("Email"));
            }
            else
            {
                userEmail = CookiesHelper.ReadCookieValue("UserEmail");
            }
                order.Email = userEmail;
            order.Mobile = mobile;
            order.ShippingID = shippingID;
            order.ShippingDate = RequestHelper.DateNow;
            order.ShippingNumber = string.Empty;
            order.ShippingMoney = shippingMoney;
            order.PayKey = payKey;
            order.PayName = payPlugins.Name;
            order.PayDate = RequestHelper.DateNow; ;
            order.IsRefund = (int)BoolType.False;
            order.FavorableActivityID = 0;
            order.GiftID = 0;
            order.InvoiceTitle = string.Empty;
            order.InvoiceContent = string.Empty;
            order.UserMessage = StringHelper.AddSafe(RequestHelper.GetForm<string>("UserMessage"));
            order.AddDate = RequestHelper.DateNow;
            order.IP = ClientHelper.IP;
            order.UserID = base.UserID;
            order.UserName = base.UserName;
            int orderID = OrderBLL.AddOrder(order);

            OrderDetailInfo orderDetail = new OrderDetailInfo();
            orderDetail.OrderID = orderID;
            orderDetail.ProductID = product.ID;
            orderDetail.ProductName = product.Name;
            orderDetail.ProductWeight = product.Weight;
            orderDetail.SendPoint = 0;
            orderDetail.ProductPrice = groupBuy.Price;
            orderDetail.BuyCount = buyCount;
            orderDetail.FatherID = 0;
            orderDetail.RandNumber = string.Empty;
            orderDetail.GiftPackID = 0;
            OrderDetailBLL.AddOrderDetail(orderDetail);
            //更改产品库存订单数量
            ProductBLL.ChangeProductOrderCountByOrder(orderID, ChangeAction.Plus);

            //添加团购单
            UserGroupBuyInfo buyInfo = new UserGroupBuyInfo();
            buyInfo.GroupBuyID = groupBuy.ID;
            buyInfo.Date = RequestHelper.DateNow;
            buyInfo.IP = ClientHelper.IP;
            buyInfo.BuyCount = buyCount;
            buyInfo.OrderID = orderID;
            buyInfo.UserID = base.UserID;
            buyInfo.UserName = base.UserName;
            buyInfo.Consignee = consignee;
            buyInfo.RegionID = singleUnlimitClass.ClassID;
            buyInfo.Address = address;
            buyInfo.ZipCode = zipCode;
            buyInfo.Tel = tel;
            buyInfo.Email = userEmail;
            buyInfo.Mobile = mobile;
            UserGroupBuyBLL.AddUserGroupBuy(buyInfo);

            ResponseHelper.Redirect("/Finish-I" + orderID.ToString() + ".aspx");
        }
    }
}
