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
using System.Linq;

namespace JWShop.Page.Lab
{
    public class CheckOut : CommonBasePage
    {
        protected string checkCart;
        protected List<CartInfo> cartList = new List<CartInfo>();
        protected List<UserAddressInfo> addressList = new List<UserAddressInfo>();
        protected List<PayPluginsInfo> payPluginsList = new List<PayPluginsInfo>();
        protected SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();

        //积分、账户余额信息
        protected decimal pointLeft, pointCanUse, moneyLeft, moneyCanUse;

        protected override void PageLoad()
        {
            base.PageLoad();

            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "Submit":
                    this.Submit();
                    break;
            }

            //登录验证
            if (base.UserId <= 0)
            {
                string redirectUrl = string.IsNullOrEmpty(isMobile)
                    ? "/user/login.html?RedirectUrl=/checkout.html"
                    : isMobile + "/login.aspx?RedirectUrl=/mobile/CheckOut.aspx";

                ResponseHelper.Redirect(redirectUrl);
                ResponseHelper.End();
            }

            //购物车验证
            checkCart = HttpUtility.UrlDecode(CookiesHelper.ReadCookieValue("CheckCart"));
            int[] cartIds = Array.ConvertAll<string, int>(checkCart.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
            if (string.IsNullOrEmpty(checkCart) || cartIds.Length < 1)
            {
                ResponseHelper.Redirect("/cart.html");
                ResponseHelper.End();
            }

            //用户信息
            var user = UserBLL.Read(base.UserId);            
            //cart list
            #region cart list
            //商品清单
            cartList = CartBLL.ReadList(base.UserId);
            cartList = cartList.Where(k => cartIds.Contains(k.Id)).ToList();
            if (cartList.Count < 1)
            {
                ResponseHelper.Redirect("/cart.html");
                ResponseHelper.End();
            }

            //关联的商品
            int count = 0;
            int[] ids = cartList.Select(k => k.ProductId).ToArray();
            var products = ProductBLL.SearchList(1, ids.Length, new ProductSearchInfo { InProductId = string.Join(",", ids) }, ref count);

            //规格
            foreach (var cart in cartList)
            {
                cart.Product = products.FirstOrDefault(k => k.Id == cart.ProductId) ?? new ProductInfo();

                if (!string.IsNullOrEmpty(cart.StandardValueList))
                {
                    //使用规格的价格和库存
                    var standardRecord = ProductTypeStandardRecordBLL.Read(cart.ProductId, cart.StandardValueList);
                    cart.Price = standardRecord.SalePrice;
                    cart.LeftStorageCount = standardRecord.Storage - standardRecord.OrderCount;
                    //规格集合
                    cart.Standards = ProductTypeStandardBLL.ReadList(Array.ConvertAll<string, int>(standardRecord.StandardIdList.Split(';'), k => Convert.ToInt32(k)));
                }
                else
                {
                    cart.Price = cart.Product.SalePrice;
                    cart.LeftStorageCount = cart.Product.TotalStorageCount - cart.Product.OrderCount;
                }
            }
            #endregion

            //收货地址
            addressList = UserAddressBLL.ReadList(base.UserId);
            addressList = addressList.OrderByDescending(k => k.IsDefault).ToList();
            singleUnlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();

            var totalProductMoney = cartList.Sum(k => k.BuyCount * k.Price);
            //取得图楼卡余额的webservice
            if (!string.IsNullOrEmpty(user.CardNo) && !string.IsNullOrEmpty(user.CardPwd))
            {
                bool isSuccess; string msg;
                isSuccess = true;
                msg = "";
                //var account = WebService.Account.GetAccount(user.CardNo, user.CardPwd, out isSuccess, out msg);
                moneyLeft = 0;// account.Zacc + account.Sacc;
                if (moneyLeft > 0)
                {
                    moneyCanUse = moneyLeft > totalProductMoney ? totalProductMoney : moneyLeft;
                }
            }

            /*----------------不可使用积分-------------------------------------------------------------------------
            decimal totalRate = (decimal)ShopConfig.ReadConfigInfo().BuyPointTotalRate;
            decimal pointRate = (decimal)ShopConfig.ReadConfigInfo().BuyPointMoneyRate;
            if (totalRate > 0 && pointRate > 0 && pointLeft > 0)
            {
                var pointPayMoney = Math.Round(totalProductMoney * totalRate, 2, MidpointRounding.AwayFromZero);
                pointCanUse = pointPayMoney * pointRate;
                if (pointCanUse > pointLeft)
                {
                    pointCanUse = pointLeft;
                }
            }
            ----------------不可使用积分-------------------------------------------------------------------------*/

            //支付方式列表
            payPluginsList = PayPlugins.ReadProductBuyPayPluginsList();

            Title = "结算中心";
        }

        private void Submit()
        {
            /*-----------重新验证选择的商品------------------------------------------*/
            checkCart = StringHelper.AddSafe(RequestHelper.GetForm<string>("CheckCart"));
            int[] cartIds = Array.ConvertAll<string, int>(checkCart.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
            string checkCartCookies = HttpUtility.UrlDecode(CookiesHelper.ReadCookieValue("CheckCart"));
            if (checkCart != checkCartCookies)
            {
                ResponseHelper.Write("error|购买商品发生了变化，请重新提交|" + isMobile + "/cart.html");
                ResponseHelper.End();
            }

            if (string.IsNullOrEmpty(checkCart) || cartIds.Length < 1)
            {
                ResponseHelper.Write("error|请选择需要购买的商品|" + isMobile + "/cart.html");
                ResponseHelper.End();
            }
            /*----------------------------------------------------------------------*/

            /*-----------读取购物车清单---------------------------------------------*/
            List<CartInfo> cartList = CartBLL.ReadList(base.UserId);
            cartList = cartList.Where(k => cartIds.Contains(k.Id)).ToList();
            if (cartList.Count <= 0)
            {
                ResponseHelper.Write("error|请选择需要购买的商品|" + isMobile + "/cart.html");
                ResponseHelper.End();
            }
            /*----------------------------------------------------------------------*/

            /*-----------必要性检查：收货地址，配送方式，支付方式-------------------*/
            var address = new UserAddressInfo { Id = RequestHelper.GetForm<int>("address_id") };
            var shipping = new ShippingInfo { Id = RequestHelper.GetForm<int>("ShippingId") };
            var pay = new PayPluginsInfo { Key = StringHelper.AddSafe(RequestHelper.GetForm<string>("pay")) };

            bool reNecessaryCheck = false;
        doReNecessaryCheck:
            if (address.Id < 1)
            {
                ResponseHelper.Write("error|请选择收货地址|");
                ResponseHelper.End();
            }
            if (shipping.Id < 1)
            {
                ResponseHelper.Write("error|请选择配送方式|");
                ResponseHelper.End();
            }
            if (string.IsNullOrEmpty(pay.Key))
            {
                ResponseHelper.Write("error|请选择支付方式|");
                ResponseHelper.End();
            }

            //读取数据库中的数据，进行重复验证
            if (!reNecessaryCheck)
            {
                address = UserAddressBLL.Read(address.Id, base.UserId);
                shipping = ShippingBLL.Read(shipping.Id);
                pay = PayPlugins.ReadPayPlugins(pay.Key);

                reNecessaryCheck = true;
                goto doReNecessaryCheck;
            }
            /*----------------------------------------------------------------------*/

            /*-----------商品清单、商品总价、邮费价格、库存检查---------------------*/
            decimal productMoney = 0;
            int count = 0;
            int[] ids = cartList.Select(k => k.ProductId).ToArray();
            var products = ProductBLL.SearchList(1, ids.Length, new ProductSearchInfo { InProductId = string.Join(",", ids) }, ref count);
            foreach (var cart in cartList)
            {
                cart.Product = products.FirstOrDefault(k => k.Id == cart.ProductId) ?? new ProductInfo();

                if (!string.IsNullOrEmpty(cart.StandardValueList))
                {
                    //使用规格的价格和库存
                    var standardRecord = ProductTypeStandardRecordBLL.Read(cart.ProductId, cart.StandardValueList);
                    cart.Price = standardRecord.SalePrice;
                    cart.LeftStorageCount = standardRecord.Storage - standardRecord.OrderCount;
                }
                else
                {
                    cart.Price = cart.Product.SalePrice;
                    cart.LeftStorageCount = cart.Product.TotalStorageCount - cart.Product.OrderCount;
                }

                //不需要检查库存，所有商品均可购买
                ////检查库存
                //if (cart.BuyCount > cart.LeftStorageCount)
                //{
                //    ResponseHelper.Write("error|商品[" + cart.ProductName + "]库存不足，无法购买|");
                //    ResponseHelper.End();
                //}

                productMoney += cart.BuyCount * cart.Price;
            }

            decimal shippingMoney = 0;
            //首先根据ShopId分组，根据供应商的不同来分别计算运费
            //然后将分拆后的供应商商品，按单个商品独立计算运费（相同商品购买多个则叠加计算）
            ShippingRegionInfo shippingRegion = ShippingRegionBLL.SearchShippingRegion(shipping.Id, address.RegionId);

            var shopIds = cartList.GroupBy(k => k.Product.ShopId).Select(k => k.Key).ToList();
            foreach (var shopId in shopIds)
            {
                var shopCartList = cartList.Where(k => k.Product.ShopId == shopId).ToList();
                foreach (var shopCartSplit in shopCartList)
                {
                    shippingMoney += ShippingRegionBLL.ReadShippingMoney(shipping, shippingRegion, shopCartSplit);
                }
            }
            /*----------------------------------------------------------------------*/

            int point = 0;
            decimal pointMoney = 0;
            /*-----------计算积分金额(不可使用积分)-----------------------------------
            decimal totalRate = (decimal)ShopConfig.ReadConfigInfo().BuyPointTotalRate;
            decimal pointRate = (decimal)ShopConfig.ReadConfigInfo().BuyPointMoneyRate;
            int point = RequestHelper.GetForm<int>("point");
            decimal pointMoney = 0;
            if (totalRate > 0 && pointRate > 0 && point > 0)
            {
                var member = WebService.Member.GetMember();
                decimal leftPoint = member.Point;
                if (point > leftPoint)
                {
                    ResponseHelper.Write("error|您的积分不足|");
                    ResponseHelper.End();
                }
                else
                {
                    pointMoney = Math.Round(point / pointRate, 2);

                    if (pointMoney > productMoney * totalRate)
                    {
                        ResponseHelper.Write("error|" + "您最多可以使用 " + (productMoney * totalRate * pointRate) + " 积分|");
                        ResponseHelper.End();
                    }
                }
            }
            ------------------------------------------------------------------------*/

            /*-----------应付总价---------------------------------------------------*/
            //decimal payMoney = productMoney + shippingMoney - pointMoney;
            decimal payMoney = productMoney + shippingMoney;
            /*----------------------------------------------------------------------*/

            var user = UserBLL.Read(base.UserId);
            /*-----------计算图楼卡余额---------------------------------------------*/
            decimal balance = RequestHelper.GetForm<decimal>("money");
            if (balance > 0)
            {
                bool isSuccess; string msg;
                isSuccess = true;
                msg = "";
                //var account = WebService.Account.GetAccount(user.CardNo, user.CardPwd, out isSuccess, out msg);
                if (!isSuccess)
                {
                    ResponseHelper.Write("error|" + msg + "|");
                    ResponseHelper.End();
                }

                if (balance > 0/*(account.Zacc + account.Sacc)*/)
                {
                    ResponseHelper.Write("error|您的图楼卡余额不足|");
                    ResponseHelper.End();
                }
                else
                {
                    if (balance > payMoney)
                    {
                        ResponseHelper.Write("error|" + "您只需使用 " + payMoney + " 元即可支付订单|");
                        ResponseHelper.End();
                    }
                }
            }
            payMoney -= balance;
            /*----------------------------------------------------------------------*/

            /*-----------检查金额---------------------------------------------------*/
            if (payMoney < 0)
            {
                ResponseHelper.Write("error|金额有错误，请重新检查|");
                ResponseHelper.End();
            }
            /*----------------------------------------------------------------------*/

            /*-----------组装基础订单模型，循环生成订单-----------------------------*/
            OrderInfo order = new OrderInfo();
            order.ProductMoney = productMoney;
            order.Consignee = address.Consignee;
            order.RegionId = address.RegionId;
            order.Address = address.Address;
            order.ZipCode = address.ZipCode;
            order.Tel = address.Tel;
            order.Mobile = address.Mobile;
            order.Email = CookiesHelper.ReadCookieValue("UserEmail");
            order.ShippingId = shipping.Id;
            order.ShippingDate = RequestHelper.DateNow;
            order.ShippingMoney = shippingMoney;
            order.Point = point;
            order.PointMoney = pointMoney;
            order.Balance = balance;
            order.PayKey = pay.Key;
            order.PayName = pay.Name;
            order.PayDate = RequestHelper.DateNow;
            order.IsRefund = (int)BoolType.False;
            order.UserMessage = StringHelper.AddSafe(RequestHelper.GetForm<string>("msg"));
            order.AddDate = RequestHelper.DateNow;
            order.IP = ClientHelper.IP;
            order.UserId = base.UserId;
            order.UserName = base.UserName;

            //循环生成订单
            var orderIds = SplitShopProduct(cartList, order);
            /*----------------------------------------------------------------------*/

            var orders = OrderBLL.ReadList(orderIds.ToArray(), base.UserId);
            /*-----------如果使用了图楼卡支付，需同步到会员管理系统中---------------*/
            /*第二步，在订单付款操作（用户端）中，同步图楼卡余额*/
            if (balance > 0)
            {
                List<string[]> paras = new List<string[]>();
                foreach (var oo in orders)
                {
                    if (oo.Balance > 0 && oo.OrderStatus == (int)OrderStatus.WaitCheck)
                    {
                        string[] para = new string[2];
                        para[0] = oo.OrderNumber;
                        para[1] = oo.Balance.ToString();
                        paras.Add(para);
                    }
                }

                //如果有全额使用了图楼卡余额支付的订单，需同步到会员管理系统中
                if (paras.Count > 0)
                {
                    bool isSuccess; string msg;
                    isSuccess = true;
                    msg = "";
                    //WebService.Account.Purchase(user.CardNo, user.CardPwd, paras, out isSuccess, out msg);

                    //同步失败，删除订单及相关信息
                    if (!isSuccess)
                    {
                        //删除订单、订单详细、订单状态相关数据
                        OrderBLL.Delete(orderIds.ToArray(), base.UserId);

                        //更改产品库存订单数量
                        foreach (var orderId in orderIds)
                        {
                            ProductBLL.ChangeOrderCountByOrder(orderId, ChangeAction.Minus);
                        }
                        ResponseHelper.Write("error|" + msg + "|");
                        ResponseHelper.End();
                    }
                    else
                    {
                        //记录用户余额消费记录
                        foreach (var par in paras)
                        {
                            var accountRecord = new UserAccountRecordInfo
                            {
                                RecordType = (int)AccountRecordType.Money,
                                Money = -decimal.Parse(par[1]),
                                Point = 0,
                                Date = DateTime.Now,
                                IP = ClientHelper.IP,
                                Note = "支付订单：" + par[0],
                                UserId = base.UserId,
                                UserName = base.UserName
                            };
                            UserAccountRecordBLL.Add(accountRecord);
                        }
                    }
                }
            }
            /*----------------------------------------------------------------------*/

            /*-----------删除购物车中已下单的商品-----------------------------------*/
            CartBLL.Delete(cartIds, base.UserId);
            CookiesHelper.DeleteCookie("CheckCart");
            /*----------------------------------------------------------------------*/

            /*如果所有订单均由图楼卡支付完成，则跳转到会员中心，否则跳转到支付提示页面*/
            if (orders.Count(k => k.OrderStatus == (int)OrderStatus.WaitPay) > 0)
            {
                ResponseHelper.Write("ok||/finish.html?id=" + string.Join(",", orders.Select(k => k.Id).ToArray()));
            }
            else
            {
                ResponseHelper.Write("ok||/user/index.html");
            }
            ResponseHelper.End();
            /*----------------------------------------------------------------------*/
        }

        /// <summary>
        /// 拆分不同的供应商商品，生成订单
        /// </summary>
        private List<int> SplitShopProduct(List<CartInfo> cartList, OrderInfo mainOrder)
        {
            List<int> orderIds = new List<int>();

            /*-----------根据ShopId分组---------------------------------------------*/
            var shopIds = cartList.GroupBy(k => k.Product.ShopId).Select(k => k.Key).ToList();
            /*----------------------------------------------------------------------*/

            /*-----------积分抵扣额度、比率(不可使用积分)-----------------------------
            decimal totalRate = (decimal)ShopConfig.ReadConfigInfo().BuyPointTotalRate;
            decimal pointRate = (decimal)ShopConfig.ReadConfigInfo().BuyPointMoneyRate;
            /*----------------------------------------------------------------------*/

            var pay = PayPlugins.ReadPayPlugins(mainOrder.PayKey);
            //
            //循环产生订单
            foreach (var shopId in shopIds)
            {
                var shopCartList = cartList.Where(k => k.Product.ShopId == shopId).ToList();

                /*-----------分拆后的订单价格---------------------------------------*/
                //运费 （去掉平均分配运费，根据供应商的不同来分别计算运费）
                //decimal shippingMoney = mainOrder.ShippingMoney / shopIds.Count;

                decimal shippingMoney = 0;
                //然后将分拆后的供应商商品，按单个商品独立计算运费（相同商品购买多个则叠加计算）
                ShippingInfo shipping = ShippingBLL.Read(mainOrder.ShippingId);
                ShippingRegionInfo shippingRegion = ShippingRegionBLL.SearchShippingRegion(mainOrder.ShippingId, mainOrder.RegionId);

                foreach (var shopCartSplit in shopCartList)
                {
                    shippingMoney += ShippingRegionBLL.ReadShippingMoney(shipping, shippingRegion, shopCartSplit);
                }

                //产品金额
                decimal productMoney = 0;
                shopCartList.ForEach(k => productMoney += k.BuyCount * k.Price);
                /*------------------------------------------------------------------*/

                decimal pointMoney = 0;
                int point = 0;
                /*-----------根据比率，分配积分抵扣金额(不可使用积分)----------------
                decimal pointMoney = 0;
                int point = 0;
                if (mainOrder.Point > 0)
                {
                    pointMoney = productMoney * totalRate;
                    point = (int)(pointMoney * pointRate);
                    if (point > mainOrder.Point)
                    {
                        point = mainOrder.Point;
                        pointMoney = Math.Round(point / pointRate, 2);
                    }
                }
                /*----------------------------------------------------------------------*/

                /*-----------应付总价---------------------------------------------------*/
                //decimal payMoney = productMoney + shippingMoney - pointMoney;
                decimal payMoney = productMoney + shippingMoney;
                /*----------------------------------------------------------------------*/

                /*-----------分配余额---------------------------------------------------*/
                decimal balance = 0;
                if (mainOrder.Balance > 0)
                {
                    balance = mainOrder.Balance;
                    if (balance > payMoney)
                    {
                        balance = payMoney;
                    }
                }
                payMoney -= balance;
                /*----------------------------------------------------------------------*/


                /*-----------添加订单---------------------------------------------------*/
                OrderInfo order = new OrderInfo();
                order.ShopId = shopId;
                order.OrderNumber = ShopCommon.CreateOrderNumber();
                order.IsActivity = (int)BoolType.False;
                order.OrderStatus = payMoney == 0 || pay.IsCod == (int)BoolType.True ? (int)OrderStatus.WaitCheck : (int)OrderStatus.WaitPay;
                order.ProductMoney = productMoney;
                order.Consignee = mainOrder.Consignee;
                order.RegionId = mainOrder.RegionId;
                order.Address = mainOrder.Address;
                order.ZipCode = mainOrder.ZipCode;
                order.Tel = mainOrder.Tel;
                order.Mobile = mainOrder.Mobile;
                order.Email = mainOrder.Email;
                order.ShippingId = mainOrder.ShippingId;
                order.ShippingDate = RequestHelper.DateNow;
                order.ShippingMoney = shippingMoney;
                order.Point = point;
                order.PointMoney = pointMoney;
                order.Balance = balance;
                order.PayKey = mainOrder.PayKey;
                order.PayName = mainOrder.PayName;
                order.PayDate = RequestHelper.DateNow;
                order.IsRefund = (int)BoolType.False;
                order.UserMessage = mainOrder.UserMessage;
                order.AddDate = RequestHelper.DateNow;
                order.IP = ClientHelper.IP;
                order.UserId = base.UserId;
                order.UserName = base.UserName;
                int orderId = OrderBLL.Add(order);

                //添加订单产品
                foreach (var cart in shopCartList)
                {
                    var orderDetail = new OrderDetailInfo();
                    orderDetail.OrderId = orderId;
                    orderDetail.ProductId = cart.ProductId;
                    orderDetail.ProductName = cart.ProductName;
                    orderDetail.ProductWeight = cart.Product.Weight;
                    orderDetail.ProductPrice = cart.Price;
                    orderDetail.BidPrice = cart.Product.BidPrice;
                    orderDetail.BuyCount = cart.BuyCount;

                    OrderDetailBLL.Add(orderDetail);
                }
                /*----------------------------------------------------------------------*/

                /*-----------使用余额、积分（余额消费记录更改到同步订单金额到图楼泛生活会员管理系统成功后，才予记录）
                if (balance > 0 && order.OrderStatus == (int)OrderStatus.WaitCheck)
                {
                    var accountRecord = new UserAccountRecordInfo
                    {
                        RecordType = (int)AccountRecordType.Money,
                        Money = -balance,
                        Point = 0,
                        Date = DateTime.Now,
                        IP = ClientHelper.IP,
                        Note = "支付订单：" + order.OrderNumber,
                        UserId = base.UserId,
                        UserName = base.UserName
                    };
                    UserAccountRecordBLL.Add(accountRecord);
                }
                ------------------------------------------------------------------------*/
                /*-----------不可使用积分-------------------------------------------------
                if (point > 0)
                {
                    var accountRecord = new UserAccountRecordInfo
                    {
                        RecordType = (int)AccountRecordType.Point,
                        Money = 0,
                        Point = -point,
                        Date = DateTime.Now,
                        IP = ClientHelper.IP,
                        Note = "支付订单：" + order.OrderNumber,
                        UserId = base.UserId,
                        UserName = base.UserName
                    };
                    UserAccountRecordBLL.Add(accountRecord);
                }
                ------------------------------------------------------------------------*/
                /*----------------------------------------------------------------------*/

                /*-----------更改产品库存订单数量---------------------------------------*/
                ProductBLL.ChangeOrderCountByOrder(orderId, ChangeAction.Plus);
                /*----------------------------------------------------------------------*/

                mainOrder.Point -= point;
                mainOrder.Balance -= balance;
                orderIds.Add(orderId);
            }

            return orderIds;
        }

    }
}