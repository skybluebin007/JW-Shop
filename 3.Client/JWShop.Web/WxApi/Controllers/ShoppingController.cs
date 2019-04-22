using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using JWShop.XcxApi.Filter;
using JWShop.XcxApi.Pay;
using JWShop.Entity;
using JWShop.Business;
using JWShop.Common;
using SkyCES.EntLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace JWShop.XcxApi.Controllers
{
    [Auth]
    [CheckLogin]
    public class ShoppingController : Controller
    {
        UserInfo user = new UserInfo();
        int uid = 0;
        int userGrade = UserGradeBLL.ReadByMoney(0).Id;

        public ShoppingController()
        {
            uid = RequestHelper.GetForm<int>("uid");
            if (uid <= 0) uid = RequestHelper.GetQueryString<int>("uid");
            user = UserBLL.ReadUserMore(uid);
            if (user != null && user.Id > 0) userGrade = UserGradeBLL.ReadByMoney(user.MoneyUsed).Id;
            user.UserName = System.Web.HttpUtility.UrlDecode(user.UserName, System.Text.Encoding.UTF8);
        }

        [HttpPost]
        public ActionResult Cart(string cartids = "")
        {
            var cartList = GetCart(cartids);

            return Json(new { cartList=cartList, user=user }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddCart()
        {
            string result = "ok";
            bool flag = true;
            int productID = RequestHelper.GetForm<int>("ProductID");
            string productName = Server.UrlDecode(StringHelper.AddSafe(RequestHelper.GetForm<string>("ProductName")));
            string standardValueList = Server.UrlDecode(StringHelper.AddSafe(RequestHelper.GetForm<string>("StandardValueList")));
            int buyCount = RequestHelper.GetForm<int>("BuyCount");
            decimal currentMemberPrice = RequestHelper.GetForm<decimal>("CurrentMemberPrice");

            if (productID <= 0 || productName == string.Empty || buyCount <= 0)
            {
                result = "无效参数";
                flag = false;
            }

            if (flag)
            {
                if (!CartBLL.IsProductInCart(productID, productName, uid))
                {
                    CartInfo cart = new CartInfo();
                    cart.ProductId = productID;
                    cart.ProductName = productName;
                    cart.BuyCount = buyCount;
                    cart.StandardValueList = standardValueList;
                    cart.RandNumber = string.Empty;
                    cart.UserId = uid;
                    cart.UserName = user.UserName;
                    int cartID = CartBLL.Add(cart, uid);
                    return Json(new { flag = true, cartid = cartID });
                }
                else
                {
                    CartInfo theCart = CartBLL.Read(productID, productName, uid);
                    if (theCart.Id > 0)
                    {
                        theCart.BuyCount += 1;
                        CartBLL.Update(Array.ConvertAll<string, int>(theCart.Id.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k)), theCart.BuyCount, uid);
                        return Json(new { flag = true, cartid = theCart.Id });
                    }
                    else
                    {
                        result = "该产品已经在购物车";
                        flag = false;
                    }
                }
            }


            return Json(new { flag = flag, msg = result });
        }

        public ActionResult UpdateCartNum(int cartid, int num)
        {
            try
            {
                CartBLL.UpdateCartNum(cartid, num, uid);
                return Content("ok");
            }
            catch
            {
                return Content("err");
            }
        }

        [HttpPost]
        public ActionResult DelCart()
        {
            string strCartId = StringHelper.SearchSafe(RequestHelper.GetForm<string>("StrCartId"));
            if (string.IsNullOrEmpty(strCartId))
            {
                ResponseHelper.Write("error|请选择商品！");
                ResponseHelper.End();
            }

            int[] ids = Array.ConvertAll<string, int>(strCartId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));

            CartBLL.Delete(ids, uid);
            return Content("ok");
        }

        [HttpGet]
        public ActionResult CheckOut(string cartids = "")
        {
            var cartList = GetCart(cartids);
            if (cartList.Count > 0)
            {
                var addresslist = UserAddressBLL.ReadList(uid);
                List<VirtualAddress> newaddresslist = new List<VirtualAddress>();
                foreach (var item in addresslist)
                {
                    VirtualAddress newaddress = new VirtualAddress();
                    newaddress.id = item.Id;
                    newaddress.name = item.Consignee;
                    newaddress.address = RegionBLL.RegionNameList(item.RegionId) + " " + item.Address;
                    newaddress.mobile = item.Mobile;
                    newaddress.isdefault = Convert.ToBoolean(item.IsDefault);
                    newaddresslist.Add(newaddress);
                }
                var paylist = PayPlugins.ReadProductBuyPayPluginsList();

                //var totalProductMoney = cartList.Sum(k => k.BuyCount * k.Price);
                decimal totalProductMoney = 0;
                int[] ids = cartList.Select(k => k.ProductId).ToArray();
                int count = 0;
                var productList = ProductBLL.SearchList(1, ids.Length, new ProductSearchInfo { InProductId = string.Join(",", ids) }, ref count);
                foreach (var cart in cartList)
                {
                    cart.Product = productList.FirstOrDefault(k => k.Id == cart.ProductId) ?? new ProductInfo();

                    if (!string.IsNullOrEmpty(cart.StandardValueList))
                    {
                        //使用规格的库存
                        var standardRecord = ProductTypeStandardRecordBLL.Read(cart.ProductId, cart.StandardValueList);
                        cart.LeftStorageCount = standardRecord.Storage - (cart.Product.UnlimitedStorage != 1 ? standardRecord.OrderCount : OrderBLL.GetProductOrderCountDaily(cart.Product.Id, cart.Product.StandardType, DateTime.Now, standardRecord.ValueList));

                        totalProductMoney += ProductBLL.GetCurrentPrice(standardRecord.SalePrice, userGrade) * (cart.BuyCount);
                    }
                    else
                    {
                        cart.LeftStorageCount = cart.Product.TotalStorageCount - (cart.Product.UnlimitedStorage != 1 ? cart.Product.OrderCount : OrderBLL.GetProductOrderCountDaily(cart.Product.Id, cart.Product.StandardType, DateTime.Now));

                        totalProductMoney += ProductBLL.GetCurrentPrice(cart.Product.SalePrice, userGrade) * (cart.BuyCount);
                    }

                }

                #region 整站满立减，已停用
                /*
                //满立减提示信息
                string payDiscountNotice = string.Empty;
                decimal favorableMoney = 0;
                if (ShopConfig.ReadConfigInfo().PayDiscount == 1)
                {
                    if (totalProductMoney >= ShopConfig.ReadConfigInfo().OrderMoney)
                    {
                        totalProductMoney -= ShopConfig.ReadConfigInfo().OrderDisCount;
                        favorableMoney= ShopConfig.ReadConfigInfo().OrderDisCount;
                        payDiscountNotice = "满立减：商品金额达到" + ShopConfig.ReadConfigInfo().OrderMoney + "元立减" + ShopConfig.ReadConfigInfo().OrderDisCount + "元";
                    }
                }
                */
                #endregion
                //读取优惠券
                //var couponlist = new List<UserCouponInfo>();
                List<UserCouponInfo> tempUserCouponList = UserCouponBLL.ReadCanUse(uid);
                List<VirtualCoupon> vcouponlist = new List<VirtualCoupon>();
                foreach (UserCouponInfo userCoupon in tempUserCouponList)
                {
                    CouponInfo tempCoupon = CouponBLL.Read(userCoupon.CouponId);
                    if (tempCoupon.UseMinAmount <= totalProductMoney)
                    {
                        VirtualCoupon vcoupon = new VirtualCoupon
                        {
                            id = userCoupon.Id,
                            name = tempCoupon.Name,
                            money = tempCoupon.Money
                        };
                        vcouponlist.Add(vcoupon);
                    }
                }

                #region 小程序优惠活动--满立减，只有整站订单优惠 获取符合条件（时间段，用户等级，金额限制）的整站优惠活动列表

                var activitylist = FavorableActivityBLL.ReadList(DateTime.Now, DateTime.Now).Where<FavorableActivityInfo>(f => f.Type == (int)FavorableType.AllOrders && ("," + f.UserGrade + ",").IndexOf("," + userGrade + ",") > -1 && totalProductMoney >= f.OrderProductMoney).ToList();

                #endregion

                decimal shippingMoney = 0;
                if (addresslist.Count > 0)
                {
                    var address = addresslist[0];
                    ShippingRegionInfo shippingRegion = ShippingRegionBLL.SearchShippingRegion(1, address.RegionId);
                    shippingMoney = ShippingRegionBLL.ReadShippingMoney(1, address.RegionId, cartList);
                }
                //是否开启积分抵现
                int enablePointPay = ShopConfig.ReadConfigInfo().EnablePointPay;
                //积分抵现百分比 
                double pointToMoney = ShopConfig.ReadConfigInfo().PointToMoney;
                //return Json(new { cart = cartList, addresslist = newaddresslist, couponlist = vcouponlist, paylist = paylist, activitylist = activitylist, shippingMoney= shippingMoney, totalproductmoney= totalProductMoney, paydiscountnotice= payDiscountNotice, favorablemoney= favorableMoney, enablepointpay= enablePointPay, pointrate = pointToMoney,pointleft = user.PointLeft }, JsonRequestBehavior.AllowGet);
                return Json(new { cart = cartList, addresslist = newaddresslist, couponlist = vcouponlist, paylist = paylist, activitylist = activitylist, shippingMoney = shippingMoney, totalproductmoney = totalProductMoney, enablepointpay = enablePointPay, pointrate = pointToMoney, pointleft = user.PointLeft,user=user }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Content("no product");
            }
        }

        [HttpPost]
        public ActionResult CheckOut()
        {
            string result = "";
            bool flag = true;
            string checkCart = StringHelper.AddSafe(RequestHelper.GetForm<string>("CheckCart"));
            int[] cartIds = Array.ConvertAll<string, int>(checkCart.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));

            if (string.IsNullOrEmpty(checkCart) || cartIds.Length < 1)
            {
                result = "请选择需要购买的商品";
                flag = false;
            }

            List<CartInfo> cartList = CartBLL.ReadList(uid);
            cartList = cartList.Where(k => cartIds.Contains(k.Id)).ToList();
            if (cartList.Count <= 0)
            {
                result = "请选择需要购买的商品";
                flag = false;
            }
            #region 存储formid
            /*
            string formid = RequestHelper.GetForm<string>("formid");
            if (!string.IsNullOrEmpty(formid))
            {
                WxFormIdBLL.Add(new WxFormIdInfo
                {
                    FormId = formid,
                    Used = 0,
                    UserId = user.Id,
                    AddDate = DateTime.Now
                });
            }
            */
            #endregion
            /*-----------必要性检查：收货地址，配送方式，支付方式-------------------*/
            //1-自提，否则：配送
            int selfPick = RequestHelper.GetForm<int>("selfpick") <= 0 ? 0 : 1;
            var address = new UserAddressInfo { Id = RequestHelper.GetForm<int>("address_id") };
            var shipping = new ShippingInfo { Id = RequestHelper.GetForm<int>("ShippingId") };
            var pay = new PayPluginsInfo { Key = "WxPay" };
            //订单优惠活动
            var favor = new FavorableActivityInfo { Id = RequestHelper.GetForm<int>("favorableActivityId") };
            //商品优惠
            var productfavor = new FavorableActivityInfo { Id = RequestHelper.GetForm<int>("ProductFavorableActivity") };
            bool reNecessaryCheck = false;
            doReNecessaryCheck:
            //如果选择配送
            if (selfPick != 1)
            {
                if (address.Id < 1)
                {
                    result = "请选择收货地址";
                    flag = false;
                }
                if (shipping.Id < 1)
                {
                    result = "请选择配送方式";
                    flag = false;
                }

                //读取数据库中的数据，进行重复验证
                if (!reNecessaryCheck)
                {
                    address = UserAddressBLL.Read(address.Id, uid);
                    shipping = ShippingBLL.Read(shipping.Id);
                    pay = PayPlugins.ReadPayPlugins(pay.Key);

                    reNecessaryCheck = true;
                    goto doReNecessaryCheck;
                }
            }
            /*-----------商品清单、商品总价、邮费价格、库存检查---------------------*/
            decimal productMoney = 0, pointMoney = 0;
            int count = 0;
            //输入的兑换积分数
            var costPoint = RequestHelper.GetForm<int>("costPoint");

            int[] ids = cartList.Select(k => k.ProductId).ToArray();
            var productList = ProductBLL.SearchList(1, ids.Length, new ProductSearchInfo { InProductId = string.Join(",", ids) }, ref count);

            foreach (var cart in cartList)
            {
                cart.Product = productList.FirstOrDefault(k => k.Id == cart.ProductId) ?? new ProductInfo();

                if (!string.IsNullOrEmpty(cart.StandardValueList))
                {
                    //使用规格的库存
                    var standardRecord = ProductTypeStandardRecordBLL.Read(cart.ProductId, cart.StandardValueList);
                    cart.LeftStorageCount = standardRecord.Storage - (cart.Product.UnlimitedStorage != 1 ? standardRecord.OrderCount : OrderBLL.GetProductOrderCountDaily(cart.Product.Id, cart.Product.StandardType, DateTime.Now, standardRecord.ValueList));
                    productMoney += ProductBLL.GetCurrentPrice(standardRecord.SalePrice, userGrade) * (cart.BuyCount);
                }
                else
                {
                    cart.LeftStorageCount = cart.Product.TotalStorageCount - (cart.Product.UnlimitedStorage != 1 ? cart.Product.OrderCount : OrderBLL.GetProductOrderCountDaily(cart.Product.Id, cart.Product.StandardType, DateTime.Now));
                    productMoney += ProductBLL.GetCurrentPrice(cart.Product.SalePrice, userGrade) * (cart.BuyCount);
                }

                //检查库存
                if (cart.BuyCount > cart.LeftStorageCount)
                {
                    result = "商品[" + cart.ProductName + "]库存不足，无法购买";
                    flag = false;
                }
            }

            ShippingRegionInfo shippingRegion = selfPick == 1 ? new ShippingRegionInfo() : ShippingRegionBLL.SearchShippingRegion(shipping.Id, address.RegionId);
            decimal shippingMoney = selfPick == 1 ? 0 : ShippingRegionBLL.ReadShippingMoney(shipping.Id, shippingRegion.RegionId, cartList);
            /*----------------------------------------------------------------------*/
            #region 优惠券
            decimal couponMoney = 0;
            string userCouponStr = RequestHelper.GetForm<string>("UserCoupon");
            UserCouponInfo userCoupon = new UserCouponInfo();
            if (userCouponStr != string.Empty)
            {
                int couponID = 0;
                if (int.TryParse(userCouponStr.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0], out couponID))
                {
                    userCoupon = UserCouponBLL.Read(couponID, uid);
                    if (userCoupon.UserId == uid && userCoupon.IsUse == 0)
                    {
                        CouponInfo tempCoupon = CouponBLL.Read(userCoupon.CouponId);
                        if (tempCoupon.UseMinAmount <= productMoney)
                        {
                            couponMoney = CouponBLL.Read(userCoupon.CouponId).Money;
                        }
                        else
                        {
                            result = "结算金额小于该优惠券要求的最低消费的金额";
                            flag = false;
                        }
                    }
                }
            }
            #endregion
            #region 如果开启了：使用积分抵现,计算积分抵现的现金金额
            if (ShopConfig.ReadConfigInfo().EnablePointPay == 1)
            {
                if (costPoint > user.PointLeft)
                {
                    result = "输入的兑换积分数[" + costPoint + "]超过现有积分，请检查";
                    flag = false;
                }
                if (costPoint > 0)
                {
                    var PointToMoneyRate = ShopConfig.ReadConfigInfo().PointToMoney;
                    pointMoney = costPoint * (decimal)PointToMoneyRate / 100;
                }
            }
            #endregion
            #region 结算商品优惠金额
            decimal productfavorableMoney = 0;
            var theFavor = FavorableActivityBLL.Read(productfavor.Id);
            if (theFavor.Id > 0)
            {
                decimal tmoney = 0;
                foreach (var tmpcart in cartList)
                {
                    tmpcart.Product = productList.FirstOrDefault(k => k.Id == tmpcart.ProductId) ?? new ProductInfo();
                    if (tmpcart.Product.ClassId.IndexOf(theFavor.ClassIds) > -1)
                    {
                        if (!string.IsNullOrEmpty(tmpcart.StandardValueList))
                        {
                            //使用规格的库存
                            var standardRecord = ProductTypeStandardRecordBLL.Read(tmpcart.ProductId, tmpcart.StandardValueList);
                            //tmpcart.LeftStorageCount = standardRecord.Storage - standardRecord.OrderCount;
                            tmpcart.Price = ProductBLL.GetCurrentPrice(standardRecord.SalePrice, userGrade);
                            tmoney += tmpcart.Price * tmpcart.BuyCount;
                        }
                        else
                        {
                            tmpcart.Price = ProductBLL.GetCurrentPrice(tmpcart.Product.SalePrice, userGrade);
                            tmoney += tmpcart.Price * tmpcart.BuyCount;
                        }
                    }
                }
                switch (theFavor.ReduceWay)
                {
                    case (int)FavorableMoney.Money:
                        productfavorableMoney += theFavor.ReduceMoney;
                        break;
                    case (int)FavorableMoney.Discount:
                        productfavorableMoney += tmoney * (100 - theFavor.ReduceDiscount) / 100;
                        break;
                    default:
                        break;
                }
            }
            #endregion
            #region 计算订单优惠活动金额
            decimal favorableMoney = 0;
            favor = FavorableActivityBLL.Read(favor.Id);
            if (favor.Id > 0)
            {
                if (("," + favor.UserGrade + ",").IndexOf("," + userGrade.ToString() + ",") > -1 && productMoney >= favor.OrderProductMoney)
                {
                    switch (favor.ReduceWay)
                    {
                        case (int)FavorableMoney.Money:
                            favorableMoney += favor.ReduceMoney;
                            break;
                        case (int)FavorableMoney.Discount:
                            favorableMoney += productMoney * (100 - favor.ReduceDiscount) / 100;
                            break;
                        default:
                            break;
                    }
                    if (favor.ShippingWay == (int)FavorableShipping.Free && ShippingRegionBLL.IsRegionIn(address.RegionId, favor.RegionId))
                    {
                        favorableMoney += shippingMoney;
                    }
                }
            }
            #endregion

            decimal payMoney = productMoney + shippingMoney - couponMoney - pointMoney - favorableMoney - productfavorableMoney;
            #region 整站满立减,已停用
            /*
            string payDiscountNotice = string.Empty;
            if (ShopConfig.ReadConfigInfo().PayDiscount == 1)
            {
                if (productMoney >= ShopConfig.ReadConfigInfo().OrderMoney) {
                    favorableMoney+= ShopConfig.ReadConfigInfo().OrderDisCount;
                    payMoney -= ShopConfig.ReadConfigInfo().OrderDisCount;
                    payDiscountNotice = "满立减：商品金额达到" + ShopConfig.ReadConfigInfo().OrderMoney + "元立减" + ShopConfig.ReadConfigInfo().OrderDisCount + "元";
                }
            }
            */
            #endregion
            if (payMoney <= 0)
            {
                result = "金额有错误，请重新检查";
                flag = false;
            }

            int orderId = 0;
            if (flag)
            {
                /*-----------组装基础订单模型，循环生成订单-----------------------------*/
                OrderInfo order = new OrderInfo();
                order.ProductMoney = productMoney;
                order.OrderNumber = ShopCommon.CreateOrderNumber();
                string payKey = RequestHelper.GetForm<string>("Pay");
                PayPluginsInfo payPlugins = PayPlugins.ReadPayPlugins(payKey);
                if (payMoney == 0 || payPlugins.IsCod == (int)BoolType.True)
                {
                    order.OrderStatus = (int)OrderStatus.WaitCheck;
                }
                else
                {
                    order.OrderStatus = (int)OrderStatus.WaitPay;
                }
                order.Consignee = address.Consignee;
                order.RegionId = address.RegionId;
                order.Address = address.Address;
                order.ZipCode = address.ZipCode;
                order.Tel = address.Tel;
                order.Mobile = address.Mobile;
                order.InvoiceTitle = RequestHelper.GetForm<string>("InvoiceTitle");
                order.InvoiceContent = RequestHelper.GetForm<string>("InvoiceContent");
                order.GiftMessige = RequestHelper.GetForm<string>("GiftMessige");
                order.Email = CookiesHelper.ReadCookieValue("UserEmail");
                order.ShippingId = shipping.Id;
                order.ShippingDate = RequestHelper.DateNow;
                order.ShippingMoney = shippingMoney;
                order.CouponMoney = couponMoney;
                order.Point = costPoint;
                order.PointMoney = pointMoney;
                order.FavorableMoney = favorableMoney + productfavorableMoney;
                order.Balance = 0;
                order.PayKey = pay.Key;
                order.PayName = pay.Name;
                order.PayDate = RequestHelper.DateNow;
                order.IsRefund = (int)BoolType.False;
                order.AddDate = RequestHelper.DateNow;
                order.IP = ClientHelper.IP;
                order.UserId = uid;
                order.UserName = user.UserName;
                order.UserMessage = RequestHelper.GetForm<string>("userMessage");
                order.GiftId = RequestHelper.GetForm<int>("GiftID");
                order.IsNoticed = 0;
                order.SelfPick = selfPick;
                //order.OrderNote = payDiscountNotice;
                orderId = OrderBLL.Add(order);

                //添加订单产品
                foreach (var cart in cartList)
                {
                    var orderDetail = new OrderDetailInfo();
                    orderDetail.OrderId = orderId;
                    orderDetail.ProductId = cart.ProductId;
                    orderDetail.ProductName = cart.ProductName;
                    orderDetail.StandardValueList = cart.StandardValueList;
                    orderDetail.ProductWeight = cart.Product.Weight;
                    if (!string.IsNullOrEmpty(cart.StandardValueList))
                    {
                        var standardRecord = ProductTypeStandardRecordBLL.Read(cart.ProductId, cart.StandardValueList);
                        orderDetail.ProductPrice = ProductBLL.GetCurrentPrice(standardRecord.SalePrice, userGrade);
                    }
                    else
                    {
                        orderDetail.ProductPrice = ProductBLL.GetCurrentPrice(cart.Product.SalePrice, userGrade);
                    }

                    orderDetail.BidPrice = cart.Product.BidPrice;
                    orderDetail.BuyCount = cart.BuyCount;

                    OrderDetailBLL.Add(orderDetail);
                }
                #region 更新优惠券状态--已使用
                //使用优惠券
                if (couponMoney > 0 && userCouponStr != "0|0")
                {
                    userCoupon.IsUse = (int)BoolType.True;
                    userCoupon.OrderId = orderId;
                    UserCouponBLL.Update(userCoupon);
                }
                #endregion
                #region 减少积分
                if (ShopConfig.ReadConfigInfo().EnablePointPay == 1 && costPoint > 0)
                {
                    //减少积分
                    UserAccountRecordInfo uarInfo = new UserAccountRecordInfo();
                    uarInfo.RecordType = (int)AccountRecordType.Point;
                    uarInfo.UserId = uid;
                    uarInfo.UserName = user.UserName;
                    uarInfo.Note = "支付订单：" + order.OrderNumber;
                    uarInfo.Point = -costPoint;
                    uarInfo.Money = 0;
                    uarInfo.Date = DateTime.Now;
                    uarInfo.IP = ClientHelper.IP;
                    UserAccountRecordBLL.Add(uarInfo);
                }
                #endregion
                //更改产品库存订单数量
                ProductBLL.ChangeOrderCountByOrder(orderId, ChangeAction.Plus);
                //删除购物车中已下单的商品
                CartBLL.Delete(cartIds, uid);
            }

            return Json(new { flag = flag, msg = result, orderid = orderId });
        }

        #region  GroupBuy
        [HttpGet]
        public ActionResult GroupBuy()
        {
            //检查个人订单是否过期
            OrderBLL.CheckOrderPayTime(user.Id);
            int productId = RequestHelper.GetQueryString<int>("pid");
            string standValue = HttpContext.Server.UrlDecode(RequestHelper.GetQueryString<string>("standvalue"));
            ProductInfo product = ProductBLL.Read(productId);
            if (product.Id <= 0 || (product.StandardType == (int)ProductStandardType.Single && string.IsNullOrEmpty(standValue)))
            {
                return Json(new { ok = false, msg = "团购商品不存在", errorcode = 0 }, JsonRequestBehavior.AllowGet);
            }
            if (product.OpenGroup != 1)
            {
                return Json(new { ok = false, msg = "该商品暂不支持拼团", errorcode = 0 }, JsonRequestBehavior.AllowGet);
            }
            int groupId = RequestHelper.GetQueryString<int>("groupid");
            var groupBuy = GroupBuyBLL.Read(groupId);
            if (groupBuy.Id > 0)
            {
                //未开始
                if (groupBuy.StartTime > DateTime.Now)
                {
                    return Json(new { flag = false, msg = "拼团活动无效", errorcode = 0 }, JsonRequestBehavior.AllowGet);
                }
                //已结束
                if (groupBuy.EndTime < DateTime.Now)
                {
                    return Json(new { flag = false, msg = "拼团活动无效", errorcode = 0 }, JsonRequestBehavior.AllowGet);
                }
                //已拼满
                if (groupBuy.Quantity - groupBuy.SignCount <= 0)
                {
                    return Json(new { flag = false, msg = "已拼满", errorcode = 0 }, JsonRequestBehavior.AllowGet);
                }
                //团长不能参团
                if (groupBuy.Leader == user.Id)
                {
                    return Json(new { flag = false, msg = "您是团长，不能重复参与", errorcode = 0 }, JsonRequestBehavior.AllowGet);
                }
                //已参团不能重复参与
                if (GroupSignBLL.ReadListByGroupId(groupBuy.Id).Find(k => k.UserId == user.Id) != null)
                {
                    return Json(new { flag = false, msg = "您已参团，不能重复参与", errorcode = 0}, JsonRequestBehavior.AllowGet);
                }
                //已参团待付款，不能重复参与
                var _groupOrders = OrderBLL.SearchList(new OrderSearchInfo { UserId = user.Id, IsActivity = (int)OrderKind.GroupBuy, FavorableActivityId = groupBuy.Id,NotEqualStatus=(int)OrderStatus.NoEffect });
                if (_groupOrders.Count>0)
                {
                    return Json(new { flag = false, msg = "您已报名参团，请及时付款",errorcode=1 }, JsonRequestBehavior.AllowGet);
                }
            }
            //如果有规格则带上规格名
           if(!string.IsNullOrEmpty(standValue)) product.Name += string.Concat("(" + standValue + ")");
            var addresslist = UserAddressBLL.ReadList(uid);
            List<VirtualAddress> newaddresslist = new List<VirtualAddress>();
            foreach (var item in addresslist)
            {
                VirtualAddress newaddress = new VirtualAddress();
                newaddress.id = item.Id;
                newaddress.name = item.Consignee;
                newaddress.address = RegionBLL.RegionNameList(item.RegionId) + " " + item.Address;
                newaddress.mobile = item.Mobile;
                newaddress.isdefault = Convert.ToBoolean(item.IsDefault);
                newaddresslist.Add(newaddress);
            }
            //营销活动免运费
            decimal shippingMoney = 0;
            //if (addresslist.Count > 0)
            //{
            //    var address = addresslist[0];
            //    ShippingRegionInfo shippingRegion = ShippingRegionBLL.SearchShippingRegion(1, address.RegionId);
            //    shippingMoney = ShippingRegionBLL.ReadShippingMoney(1, address.RegionId, product);
            //}
            var paylist = PayPlugins.ReadProductBuyPayPluginsList();

            //var totalProductMoney = cartList.Sum(k => k.BuyCount * k.Price);
            decimal totalProductMoney = 0;
            if (product.StandardType != (int)ProductStandardType.Single)
            {
                totalProductMoney = product.GroupPrice;
            }
            else
            {
                var standardRecord = ProductTypeStandardRecordBLL.Read(productId, standValue);
                totalProductMoney = standardRecord.GroupPrice;
            }
            return Json(new { ok = true, msg = "",  errorcode = 200,addresslist = newaddresslist, paylist = paylist, shippingMoney = shippingMoney, totalproductmoney = totalProductMoney, product = product, user = user }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult GroupBuyCheckOut()
        {
            string result = "";
            bool flag = true;

            int productId = RequestHelper.GetForm<int>("productId");
            string standValue = RequestHelper.GetForm<string>("standvalue");
            int groupId = RequestHelper.GetForm<int>("groupid") <= 0 ? 0 : RequestHelper.GetForm<int>("groupid");
            ProductInfo product = ProductBLL.Read(productId);
            if (product.Id <= 0 || (product.StandardType == (int)ProductStandardType.Single && string.IsNullOrEmpty(standValue)))
            {
                return Json(new { flag = false, msg = "团购商品不存在", errorcode = 0 }, JsonRequestBehavior.AllowGet);
            }
            if (product.OpenGroup != 1)
            {
                return Json(new { flag = false, msg = "该商品暂不支持拼团", errorcode = 0 }, JsonRequestBehavior.AllowGet);
            }
            var groupBuy = GroupBuyBLL.Read(groupId);
            if (groupBuy.Id > 0)
            {
                //未开始
                if (groupBuy.StartTime > DateTime.Now)
                {
                    return Json(new { flag = false, msg = "拼团活动无效", errorcode = 0 });
                }
                //已结束
                if (groupBuy.EndTime < DateTime.Now)
                {
                    return Json(new { flag = false, msg = "拼团活动无效", errorcode = 0 });
                }
                //已拼满
                if (groupBuy.Quantity - groupBuy.SignCount <= 0)
                {
                    return Json(new { flag = false, msg = "已拼满", errorcode = 0 });
                }
                //团长不能参团
                if (groupBuy.Leader == user.Id)
                {
                    return Json(new { flag = false, msg = "您是团长，不能重复参与", errorcode = 0 });
                }
                //已参团不能重复参与
                if (GroupSignBLL.ReadListByGroupId(groupBuy.Id).Find(k => k.UserId == user.Id) != null)
                {
                    return Json(new { flag = false, msg = "您已参团，不能重复参与", errorcode = 0 });
                }
                //已参团待付款，不能重复参与
                var _groupOrders = OrderBLL.SearchList(new OrderSearchInfo { UserId = user.Id, IsActivity = (int)OrderKind.GroupBuy, FavorableActivityId = groupBuy.Id, NotEqualStatus = (int)OrderStatus.NoEffect });
                if (_groupOrders.Count > 0)
                {
                    return Json(new { flag = false, msg = "您已报名参团，请及时付款", errorcode = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
         
            //如果有规格值,product.name 带上规格值
            if (!string.IsNullOrEmpty(standValue)) product.Name += string.Concat("(" + standValue + ")");
            /*-----------必要性检查：收货地址，配送方式，支付方式-------------------*/
            //1-自提，否则：配送
            int selfPick = RequestHelper.GetForm<int>("selfpick") <= 0 ? 0 : 1;
            var address = new UserAddressInfo { Id = RequestHelper.GetForm<int>("address_id") };
            var shipping = new ShippingInfo { Id = RequestHelper.GetForm<int>("ShippingId") };
            var pay = new PayPluginsInfo { Key = "WxPay" };
            //订单优惠活动
            var favor = new FavorableActivityInfo { Id = RequestHelper.GetForm<int>("favorableActivityId") };
            //商品优惠
            var productfavor = new FavorableActivityInfo { Id = RequestHelper.GetForm<int>("ProductFavorableActivity") };
            bool reNecessaryCheck = false;
            doReNecessaryCheck:
            //如果选择配送
            if (selfPick != 1)
            {
                if (address.Id < 1)
                {
                    result = "请选择收货地址";
                    flag = false;
                }
                if (shipping.Id < 1)
                {
                    result = "请选择配送方式";
                    flag = false;
                }

                //读取数据库中的数据，进行重复验证
                if (!reNecessaryCheck)
                {
                    address = UserAddressBLL.Read(address.Id, uid);
                    shipping = ShippingBLL.Read(shipping.Id);
                    pay = PayPlugins.ReadPayPlugins(pay.Key);

                    reNecessaryCheck = true;
                    goto doReNecessaryCheck;
                }
            }
            /*-----------商品清单、商品总价、邮费价格、库存检查---------------------*/
            decimal productMoney = 0, pointMoney = 0;
            int count = 0;
            //输入的兑换积分数
            var costPoint = RequestHelper.GetForm<int>("costPoint");
            int leftStorageCount = 0;
            int buyCount = RequestHelper.GetForm<int>("buyCount") <= 0 ? 1 : RequestHelper.GetForm<int>("buyCount");
            if (product.StandardType == (int)ProductStandardType.Single)
            {
                if (!string.IsNullOrEmpty(standValue))
                {
                    //使用规格的库存
                    var standardRecord = ProductTypeStandardRecordBLL.Read(product.Id, standValue);
                    leftStorageCount = standardRecord.Storage - (product.UnlimitedStorage != 1 ? standardRecord.OrderCount : OrderBLL.GetProductOrderCountDaily(product.Id, product.StandardType, DateTime.Now, standardRecord.ValueList));
                    productMoney += standardRecord.GroupPrice * buyCount;
                }
            }
            else
            {
                leftStorageCount = product.TotalStorageCount - (product.UnlimitedStorage != 1 ? product.OrderCount : OrderBLL.GetProductOrderCountDaily(product.Id, product.StandardType, DateTime.Now));
                productMoney += product.GroupPrice * buyCount;
            }

            //检查库存
            if (buyCount > leftStorageCount)
            {
                result = "商品[" + product.Name + "]库存不足，无法购买";
                flag = false;
            }


            ShippingRegionInfo shippingRegion = selfPick == 1 ? new ShippingRegionInfo() : ShippingRegionBLL.SearchShippingRegion(shipping.Id, address.RegionId);
            //decimal shippingMoney = selfPick == 1 ? 0 : ShippingRegionBLL.ReadShippingMoney(shipping.Id, shippingRegion.RegionId, product);
            //营销活动免运费
            decimal shippingMoney = 0;
            /*----------------------------------------------------------------------*/
            #region 优惠券
            decimal couponMoney = 0;
            string userCouponStr = RequestHelper.GetForm<string>("UserCoupon");
            UserCouponInfo userCoupon = new UserCouponInfo();
            if (userCouponStr != string.Empty)
            {
                int couponID = 0;
                if (int.TryParse(userCouponStr.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0], out couponID))
                {
                    userCoupon = UserCouponBLL.Read(couponID, uid);
                    if (userCoupon.UserId == uid && userCoupon.IsUse == 0)
                    {
                        CouponInfo tempCoupon = CouponBLL.Read(userCoupon.CouponId);
                        if (tempCoupon.UseMinAmount <= productMoney)
                        {
                            couponMoney = CouponBLL.Read(userCoupon.CouponId).Money;
                        }
                        else
                        {
                            result = "结算金额小于该优惠券要求的最低消费的金额";
                            flag = false;
                        }
                    }
                }
            }
            #endregion
            #region 如果开启了：使用积分抵现,计算积分抵现的现金金额
            if (ShopConfig.ReadConfigInfo().EnablePointPay == 1)
            {
                if (costPoint > user.PointLeft)
                {
                    result = "输入的兑换积分数[" + costPoint + "]超过现有积分，请检查";
                    flag = false;
                }
                if (costPoint > 0)
                {
                    var PointToMoneyRate = ShopConfig.ReadConfigInfo().PointToMoney;
                    pointMoney = costPoint * (decimal)PointToMoneyRate / 100;
                }
            }
            #endregion
            #region 结算商品优惠金额
            decimal productfavorableMoney = 0;
            var theFavor = FavorableActivityBLL.Read(productfavor.Id);
            if (theFavor.Id > 0)
            {
                decimal tmoney = 0;


                if (product.ClassId.IndexOf(theFavor.ClassIds) > -1)
                {
                    if (!string.IsNullOrEmpty(standValue))
                    {
                        //使用规格的库存
                        var standardRecord = ProductTypeStandardRecordBLL.Read(product.Id, standValue);
                        tmoney += standardRecord.GroupPrice * buyCount;
                    }
                    else
                    {
                        tmoney += product.GroupPrice * buyCount;
                    }

                }
                switch (theFavor.ReduceWay)
                {
                    case (int)FavorableMoney.Money:
                        productfavorableMoney += theFavor.ReduceMoney;
                        break;
                    case (int)FavorableMoney.Discount:
                        productfavorableMoney += tmoney * (100 - theFavor.ReduceDiscount) / 100;
                        break;
                    default:
                        break;
                }
            }
            #endregion
            #region 计算订单优惠活动金额
            decimal favorableMoney = 0;
            favor = FavorableActivityBLL.Read(favor.Id);
            if (favor.Id > 0)
            {
                if (("," + favor.UserGrade + ",").IndexOf("," + userGrade.ToString() + ",") > -1 && productMoney >= favor.OrderProductMoney)
                {
                    switch (favor.ReduceWay)
                    {
                        case (int)FavorableMoney.Money:
                            favorableMoney += favor.ReduceMoney;
                            break;
                        case (int)FavorableMoney.Discount:
                            favorableMoney += productMoney * (100 - favor.ReduceDiscount) / 100;
                            break;
                        default:
                            break;
                    }
                    if (favor.ShippingWay == (int)FavorableShipping.Free && ShippingRegionBLL.IsRegionIn(address.RegionId, favor.RegionId))
                    {
                        favorableMoney += shippingMoney;
                    }
                }
            }
            #endregion

            decimal payMoney = productMoney + shippingMoney - couponMoney - pointMoney - favorableMoney - productfavorableMoney;

            if (payMoney <= 0)
            {
                result = "金额有错误，请重新检查";
                flag = false;
            }
            int orderId = 0;
            if (flag)
            {

                #region /*-----------组装基础订单模型，循环生成订单-----------------------------*/
                OrderInfo order = new OrderInfo();
                order.ProductMoney = productMoney;
                order.OrderNumber = ShopCommon.CreateOrderNumber();
                string payKey = RequestHelper.GetForm<string>("Pay");
                PayPluginsInfo payPlugins = PayPlugins.ReadPayPlugins(payKey);
                if (payMoney == 0 || payPlugins.IsCod == (int)BoolType.True)
                {
                    order.OrderStatus = (int)OrderStatus.WaitCheck;
                }
                else
                {
                    order.OrderStatus = (int)OrderStatus.WaitPay;
                }
                //新增一条团购订单（IsActivity=2）绑定团购Id（FavorableActivityId）
                //如果groupId=0则表示新开团，否则表示参团
                order.IsActivity = (int)OrderKind.GroupBuy;
                order.FavorableActivityId = groupId;

                order.Consignee = address.Consignee;
                order.RegionId = address.RegionId;
                order.Address = address.Address;
                order.ZipCode = address.ZipCode;
                order.Tel = address.Tel;
                order.Mobile = address.Mobile;
                order.InvoiceTitle = RequestHelper.GetForm<string>("InvoiceTitle");
                order.InvoiceContent = RequestHelper.GetForm<string>("InvoiceContent");
                order.GiftMessige = RequestHelper.GetForm<string>("GiftMessige");
                order.Email = CookiesHelper.ReadCookieValue("UserEmail");
                order.ShippingId = shipping.Id;
                order.ShippingDate = RequestHelper.DateNow;
                order.ShippingMoney = shippingMoney;
                order.CouponMoney = couponMoney;
                order.Point = costPoint;
                order.PointMoney = pointMoney;
                order.FavorableMoney = favorableMoney + productfavorableMoney;
                order.Balance = 0;
                order.PayKey = pay.Key;
                order.PayName = pay.Name;
                order.PayDate = RequestHelper.DateNow;
                order.IsRefund = (int)BoolType.False;
                order.AddDate = RequestHelper.DateNow;
                order.IP = ClientHelper.IP;
                order.UserId = uid;
                order.UserName = user.UserName;
                order.UserMessage = RequestHelper.GetForm<string>("userMessage");
                order.GiftId = RequestHelper.GetForm<int>("GiftID");
                order.IsNoticed = 0;
                order.SelfPick = selfPick;
                //order.OrderNote = payDiscountNotice;
                    #region 下单前再次检测拼团有效性                   
                if (groupBuy.Id > 0)
                {
                    groupBuy = GroupBuyBLL.Read(groupId);
                    //已拼满
                    if (groupBuy.Quantity - groupBuy.SignCount <= 0)
                    {
                        return Json(new { flag = false, msg = "已拼满", errorcode = 0 });
                    }
                    //团长不能参团
                    if (groupBuy.Leader == user.Id)
                    {
                        return Json(new { flag = false, msg = "您是团长，不能重复参与", errorcode = 0 });
                    }
                    //已参团不能重复参与
                    if (GroupSignBLL.ReadListByGroupId(groupBuy.Id).Find(k => k.UserId == user.Id) != null)
                    {
                        return Json(new { flag = false, msg = "您已参团，不能重复参与", errorcode = 0 });
                    }
                }
                    #endregion
                orderId = OrderBLL.Add(order);
                #region 添加拼团参与记录
                //增加参团记录
                GroupSignBLL.Add(new GroupSignInfo
                {
                    GroupId = groupId,
                    UserId = user.Id,
                    OrderId = orderId,
                    SignTime = DateTime.Now
                });
                //开团表signcount加1
                GroupBuyBLL.PlusSignCount(groupId);

                #endregion
                #endregion
                #region 添加订单详情--购买明细

                var orderDetail = new OrderDetailInfo();
                orderDetail.OrderId = orderId;
                orderDetail.ProductId = product.Id;
                orderDetail.ProductName = product.Name;
                orderDetail.StandardValueList = standValue;
                orderDetail.ProductWeight = product.Weight;
                if (!string.IsNullOrEmpty(standValue))
                {
                    var standardRecord = ProductTypeStandardRecordBLL.Read(product.Id, standValue);
                    orderDetail.ProductPrice = standardRecord.GroupPrice;
                }
                else
                {
                    orderDetail.ProductPrice = product.GroupPrice;
                }

                orderDetail.BidPrice = product.BidPrice;
                orderDetail.BuyCount = buyCount;

                OrderDetailBLL.Add(orderDetail);
                #endregion

              
                #region 更新优惠券状态--已使用
                //使用优惠券
                if (couponMoney > 0 && userCouponStr != "0|0")
                {
                    userCoupon.IsUse = (int)BoolType.True;
                    userCoupon.OrderId = orderId;
                    UserCouponBLL.Update(userCoupon);
                }
                #endregion
                #region 减少积分
                if (ShopConfig.ReadConfigInfo().EnablePointPay == 1 && costPoint > 0)
                {
                    //减少积分
                    UserAccountRecordInfo uarInfo = new UserAccountRecordInfo();
                    uarInfo.RecordType = (int)AccountRecordType.Point;
                    uarInfo.UserId = uid;
                    uarInfo.UserName = user.UserName;
                    uarInfo.Note = "支付订单：" + order.OrderNumber;
                    uarInfo.Point = -costPoint;
                    uarInfo.Money = 0;
                    uarInfo.Date = DateTime.Now;
                    uarInfo.IP = ClientHelper.IP;
                    UserAccountRecordBLL.Add(uarInfo);
                }
                #endregion
                //更改产品库存订单数量
                ProductBLL.ChangeOrderCountByOrder(orderId, ChangeAction.Plus);
                #region 存储formid
                //string formid = RequestHelper.GetForm<string>("formid");
                //if (!string.IsNullOrEmpty(formid))
                //{
                //    WxFormIdBLL.Add(new WxFormIdInfo
                //    {
                //        FormId = formid,
                //        Used = 0,
                //        UserId = user.Id,
                //        AddDate = DateTime.Now
                //    });
                //}
                #endregion
            }

            return Json(new { flag = flag, msg = result, errorcode = flag?200:0, orderid = orderId });
        }
        #endregion

        public ActionResult Pay(string code)
        {
            string orderIds = RequestHelper.GetQueryString<string>("order_id");

            if (string.IsNullOrEmpty(orderIds))
            {
                Response.Write("无效的请求");
                Response.End();
            }

            List<OrderInfo> orders = new List<OrderInfo>();
            try
            {
                int[] ids = Array.ConvertAll<string, int>(orderIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
                orders = OrderBLL.ReadList(ids);

                if (ids.Length != orders.Count)
                {
                    Response.Write("包含无效的订单");
                    Response.End();
                }
            }
            catch
            {
                Response.Write("无效的请求");
                Response.End();
            }
            /*******************请求参数验证 end*****************************************************************/


            JsApiPay jsApiPay = new JsApiPay();
            try
            {
                //调用【网页授权获取用户信息】接口获取用户的openid和access_token
                jsApiPay.GetOpenidAndAccessToken(orderIds, code);
            }
            catch (Exception ex)
            {
                return Json(new { flag = false, msg = "页面加载出错，请重试：" + ex.ToString() + "</span>" });
            }

            //付款金额
            decimal total_price = 0;
            foreach (var order in orders)
            {
                total_price += order.ProductMoney - order.FavorableMoney + order.ShippingMoney + order.OtherMoney - order.Balance - order.CouponMoney - order.PointMoney;
            }
            //支付金额,单位是分，不能有小数 
            jsApiPay.total_fee = Convert.ToInt32(total_price * 100);

            //附加数据,可能有多个订单合并付款，用逗号分隔
            jsApiPay.attach = string.Join(",", orders.Select(k => k.Id));

            //检测是否给当前页面传递了相关参数
            if (string.IsNullOrEmpty(jsApiPay.openid) || jsApiPay.total_fee <= 0)
            {
                Log.Error(this.GetType().ToString(), "This page have not get params, cannot be inited, exit...");
                return Json(new { flag = false, msg = "页面传参出错,请返回重试" });
            }

            jsApiPay.order_body = string.Join(",", orders.Select(k => k.OrderNumber));

            //JSAPI支付预处理
            try
            {
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                var wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);

                return Json(new { flag = true, msg = "参数生成成功" });
            }
            catch (Exception ex)
            {
                return Json(new { flag = false, msg = "下单失败，" + ex.Message + "，请返回重试" });
            }
        }

        private List<CartInfo> GetCart(string cartids = "")
        {
            var cartList = CartBLL.ReadList(uid);
            if (cartids != "")
            {
                var cartidarr = Array.ConvertAll<string, int>(cartids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
                cartList = (from cart in cartList
                            where cartidarr.Contains(cart.Id)
                            select cart).ToList();
            }

            //关联的商品
            int count = 0;
            int[] ids = cartList.Select(k => k.ProductId).ToArray();
            var products = ProductBLL.SearchList(1, ids.Count(), new ProductSearchInfo { InProductId = string.Join(",", ids) }, ref count);

            int productCount = 0;
            //规格
            foreach (var cart in cartList)
            {
                cart.Product = products.FirstOrDefault(k => k.Id == cart.ProductId) ?? new ProductInfo();

                if (!string.IsNullOrEmpty(cart.StandardValueList))
                {
                    //使用规格的价格和库存
                    var standardRecord = ProductTypeStandardRecordBLL.Read(cart.ProductId, cart.StandardValueList);
                    cart.Price = ProductBLL.GetCurrentPrice(standardRecord.SalePrice, userGrade);
                    cart.LeftStorageCount = standardRecord.Storage - (cart.Product.UnlimitedStorage != 1 ? OrderDetailBLL.GetOrderCount(cart.ProductId, cart.StandardValueList) : OrderBLL.GetProductOrderCountDaily(cart.Product.Id, cart.Product.StandardType, DateTime.Now, standardRecord.ValueList));
                    //规格集合
                    cart.Standards = ProductTypeStandardBLL.ReadList(Array.ConvertAll<string, int>(standardRecord.StandardIdList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k)));
                    productCount += cart.BuyCount;
                }
                else
                {
                    cart.Price = ProductBLL.GetCurrentPrice(cart.Product.SalePrice, userGrade);
                    cart.LeftStorageCount = cart.Product.TotalStorageCount - (cart.Product.UnlimitedStorage != 1 ? OrderDetailBLL.GetOrderCount(cart.ProductId, cart.StandardValueList) : OrderBLL.GetProductOrderCountDaily(cart.Product.Id, cart.Product.StandardType, DateTime.Now));
                    productCount += cart.BuyCount;
                }
            }

            return cartList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="otype">订单类型【7：待分享拼团订单】</param>
        /// <returns></returns>
        public ActionResult OrderList(int page, int pagesize, int otype = 0)
        {
            //检查用户的待付款订单是否超时失效，超时则更新为失效状态
            OrderBLL.CheckOrderPayTime(user.Id);
            //订单自动收货
            OrderBLL.CheckOrderRecieveTimeProg(user.Id);
            int count = int.MinValue;
            List<OrderInfo> orderlist = new List<OrderInfo>();
            var ordersearch = new OrderSearchInfo()
            {
                IsDelete = 0,
                UserId = uid,
            };
            if (otype >= 0 && otype != 7)
            {
                if (otype > 0) ordersearch.OrderStatus = otype;
                orderlist = OrderBLL.SearchList(page, pagesize, ordersearch, ref count);
            }
            //待分享
            if (otype == 7)
            {
                //待审核，且拼团在进行中
                ordersearch.OrderStatus = 2;
                ordersearch.Status = (int)GroupBuyStatus.Going;
                orderlist = OrderBLL.SearchGroupOrderList(page, pagesize, ordersearch, ref count);
            }


            List<VirtualOrder> vorders = new List<VirtualOrder>();
            foreach (var item in orderlist)
            {
                var orderDetailList = OrderDetailBLL.ReadList(item.Id);
                int[] productIds = orderDetailList.Select(k => k.ProductId).ToArray();
                var productList = new List<ProductInfo>();
                if (productIds.Length > 0)
                {
                    productList = ProductBLL.SearchList(1, productIds.Length, new ProductSearchInfo { InProductId = string.Join(",", productIds) }, ref count);
                }

                List<ProductCommentInfo>[] listPinfoArr = new List<ProductCommentInfo>[productList.Count];
                int pi = 0;
                bool isPL = true;
                List<ProductVirtualModel> vplist = new List<ProductVirtualModel>();
                foreach (ProductInfo item2 in productList)
                {
                    ProductCommentSearchInfo psi = new ProductCommentSearchInfo();
                    psi.ProductId = item2.Id;
                    psi.UserId = uid;
                    psi.OrderID = item.Id;
                    listPinfoArr[pi] = ProductCommentBLL.SearchProductCommentList(psi);
                    if (listPinfoArr[pi].Count <= 0)
                        isPL = false;
                }

                foreach (var item2 in orderDetailList)
                {
                    ProductVirtualModel vp = new ProductVirtualModel()
                    {
                        id = item2.ProductId,
                        name = item2.ProductName,
                        price = item2.ProductPrice,
                        buycount = item2.BuyCount,
                        img = ShopCommon.ShowImage(ProductBLL.Read(item2.ProductId).Photo)
                    };
                    vplist.Add(vp);
                }
                VirtualOrder vorder = new VirtualOrder()
                {
                    id = item.Id,
                    ordernum = item.OrderNumber,
                    orderstatus = EnumHelper.ReadEnumChineseName<OrderStatus>(item.OrderStatus),
                    realpay = item.ProductMoney - item.FavorableMoney - item.PointMoney - item.CouponMoney + item.ShippingMoney + item.OtherMoney,
                    iscommended = isPL,
                    proarr = vplist,
                    canrefund = JWRefund.CanRefund(item).CanRefund,
                    isactivity = item.IsActivity,
                    favorableactivityid = item.FavorableActivityId
                };

                vorders.Add(vorder);
            }

            return Json(new { orders = vorders }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DelOrder(int id)
        {
            OrderInfo tmpOrder = OrderBLL.Read(id);
            if (tmpOrder.UserId == uid)
            {
                if (tmpOrder.OrderStatus == (int)JWShop.Entity.OrderStatus.WaitPay)
                {//待付款直接删除退还积分库存
                    #region 退还积分
                    if (tmpOrder.Point > 0)
                    {
                        var accountRecord = new UserAccountRecordInfo
                        {
                            RecordType = (int)AccountRecordType.Point,
                            Money = 0,
                            Point = tmpOrder.Point,
                            Date = DateTime.Now,
                            IP = ClientHelper.IP,
                            Note = "取消订单：" + tmpOrder.OrderNumber + "，退回用户积分",
                            UserId = tmpOrder.UserId,
                            UserName = tmpOrder.UserName,
                        };
                        UserAccountRecordBLL.Add(accountRecord);
                    }
                    #endregion
                    //更新商品库存数量
                    ProductBLL.ChangeOrderCountByOrder(tmpOrder.Id, ChangeAction.Minus);
                    OrderBLL.Delete(id);
                    AdminLogBLL.Add("会员(" + uid + ")" + ShopLanguage.ReadLanguage("DeleteRecordCompletely"), ShopLanguage.ReadLanguage("Order"), id);
                }
                else
                { //已付款逻辑删除可恢复
                    if (tmpOrder.IsDelete == (int)BoolType.False)
                    {
                        tmpOrder.IsDelete = (int)BoolType.True;
                        OrderBLL.Update(tmpOrder);
                        AdminLogBLL.Add("会员(" + uid + ")" + ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Order"), id);
                    }
                }

                return Json(new { flag = true, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { flag = false, msg = "无权删除" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult OrderDetail(int id)
        {
            OrderBLL.CheckOrderPayTime(uid);
            int orderId = RequestHelper.GetQueryString<int>("id");
            var userGradeName = UserGradeBLL.Read(userGrade).Name;
            var order = OrderBLL.Read(orderId, uid);
            var ctype = RequestHelper.GetQueryString<string>("ctype");

            if (order.Id <= 0)
            {
                return Content("订单不存在");
            }
            var orderDetailList = OrderDetailBLL.ReadList(orderId);
            int[] productIds = orderDetailList.Select(k => k.ProductId).ToArray();
            List<ProductInfo> productList = new List<ProductInfo>();
            if (productIds.Length > 0)
            {
                int count = 0;
                productList = ProductBLL.SearchList(1, productIds.Length, new ProductSearchInfo { InProductId = string.Join(",", productIds) }, ref count);
            }

            bool iscommended = true;
            #region 判断是否已评论
            List<ProductCommentInfo>[] listPinfoArr = new List<ProductCommentInfo>[productList.Count];
            List<ProductVirtualModel> vplist = new List<ProductVirtualModel>();
            int pi = 0;
            foreach (ProductInfo item in productList)
            {
                ProductCommentSearchInfo psi = new ProductCommentSearchInfo();
                psi.ProductId = item.Id;
                psi.UserId = uid;
                psi.OrderID = orderId;
                listPinfoArr[pi] = ProductCommentBLL.SearchProductCommentList(psi);
                if (listPinfoArr[pi].Count <= 0)
                    iscommended = false;

            }
            #endregion
            foreach (var item in orderDetailList)
            {
                if (ctype == "commend")
                {
                    if (ProductCommentBLL.ReadList().Where(k => k.OrderId == orderId && k.ProductId == item.ProductId).Count() != 0) continue;
                }
                ProductVirtualModel vp = new ProductVirtualModel()
                {
                    id = item.ProductId,
                    name = item.ProductName,
                    price = item.ProductPrice,
                    buycount = item.BuyCount,
                    img = ShopCommon.ShowImage(ProductBLL.Read(item.ProductId).Photo)
                };
                vplist.Add(vp);
            }
            string addstr = RegionBLL.RegionNameList(order.RegionId) + " " + order.Address;
            #region 判断是否有退款正在处理中
            //订单是否有正在处理的退款
            bool is_order_refunding = false;
            //正在处理中的退款订单或商品
            var orderRefundList = OrderRefundBLL.ReadListValid(id);
            //有正在处理中的退款订单或商品，
            if (orderRefundList.Count(k => OrderRefundBLL.RefundGoing(k.Status)) > 0)
            {
                is_order_refunding = true;
            }
            #endregion
            var vorder = new
            {
                id = order.Id,
                ordernum = order.OrderNumber,
                orderstatus = EnumHelper.ReadEnumChineseName<OrderStatus>(order.OrderStatus),
                realpay = order.ProductMoney - order.FavorableMoney - order.PointMoney - order.CouponMoney + order.ShippingMoney + order.OtherMoney,
                iscommended = iscommended,
                proarr = vplist,
                shippingmoney = order.ShippingMoney,
                productmoney = order.ProductMoney,
                couponmoney = order.CouponMoney,
                favorablemoney = order.FavorableMoney,
                othermoney = order.OtherMoney,
                pointmoney = order.PointMoney,
                ordernote = order.OrderNote,
                orderdate = order.AddDate,
                usermessage = order.UserMessage,
                address = addstr,
                addinfo = new VirtualAddress()
                {
                    name = order.Consignee,
                    address = addstr,
                    mobile = order.Mobile
                },
                canrefund = JWRefund.CanRefund(order).CanRefund,
                is_order_refunding= is_order_refunding,
                selfpick = order.SelfPick
            };

            return Json(new { order = vorder }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 用户确认收货
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OrderConfirm(int id)
        {
            try
            {
                var order = OrderBLL.Read(id);
                if (order.Id <= 0 || order.UserId != user.Id)
                {
                    return Json(new { flag = false, msg = "订单不存在" }, JsonRequestBehavior.AllowGet);
                }
                if (order.OrderStatus!=(int)OrderStatus.HasShipping)
                {
                    return Json(new { flag = false, msg = "当前订单不能进行收货操作" }, JsonRequestBehavior.AllowGet);
                }
                #region 判断是否有退款正在处理中
                //正在处理中的退款订单或商品
                var  orderRefundList = OrderRefundBLL.ReadListValid(id);
                //有正在处理中的退款订单或商品，
                if (orderRefundList.Count(k => OrderRefundBLL.RefundGoing(k.Status)) > 0)
                {
                    return Json(new { flag = true, msg = "refunding" }, JsonRequestBehavior.AllowGet);
                }
                #endregion
                int startOrderStatus = order.OrderStatus;
                order.OrderStatus = (int)OrderStatus.ReceiveShipping;
                OrderBLL.UserUpdateOrderAddAction(order, "用户确认收货", (int)OrderOperate.Received, startOrderStatus);
                #region 会员确认收货赠送优惠券
                int count = 0;
                var couponlist = CouponBLL.SearchList(1, 1, new CouponSearchInfo { Type = (int)CouponKind.ReceiveShippingGet, CanUse = 1 }, ref count);
                if (couponlist.Count > 0)
                {
                    UserCouponInfo userCoupon = UserCouponBLL.ReadLast(couponlist[0].Id);
                    int startNumber = 0;
                    if (userCoupon.Id > 0)
                    {
                        string tempNumber = userCoupon.Number.Substring(3, 5);
                        while (tempNumber.Substring(0, 1) == "0")
                        {
                            tempNumber = tempNumber.Substring(1);
                        }
                        startNumber = Convert.ToInt32(tempNumber);
                    }
                    startNumber++;
                    UserCouponBLL.Add(new UserCouponInfo
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        CouponId = couponlist[0].Id,
                        GetType = (int)CouponType.ReceiveShippingGet,
                        Number = ShopCommon.CreateCouponNo(couponlist[0].Id, startNumber),
                        Password = ShopCommon.CreateCouponPassword(startNumber),
                        IsUse = (int)BoolType.False,
                        OrderId = 0

                    });
                }
                #endregion
                #region 确认收货获得积分
                //根据订单付款金额全额返还积分
                int sendPoint = (int)Math.Floor(OrderBLL.ReadNoPayMoney(order));
                if (sendPoint > 0 && order.IsActivity == (int)BoolType.False)
                {
                    var accountRecord = new UserAccountRecordInfo
                    {
                        RecordType = (int)AccountRecordType.Point,
                        Money = 0,
                        Point = sendPoint,
                        Date = DateTime.Now,
                        IP = ClientHelper.IP,
                        Note = ShopLanguage.ReadLanguage("OrderReceived").Replace("$OrderNumber", order.OrderNumber),
                        UserId = order.UserId,
                        UserName = order.UserName
                    };
                    UserAccountRecordBLL.Add(accountRecord);
                }
                #endregion
                #region 确认收货给分销商返佣
                //订单实际支付金额
                decimal paid_money = OrderBLL.ReadNoPayMoney(order);
                //购买者有推荐人 且 实际支付金额大于0才进行返佣
                if (user.Recommend_UserId>0 && paid_money > 0)
                {
                    RebateBLL.RebateToDistributor(user, paid_money,order.Id);
                }
                #endregion
                return Json(new { flag = true, msg = "用户确认收货" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { flag = false, msg = "收货出错" }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 用户取消退款并确认收货
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderConfirmWithRefund(int id)
        {
            try
            {
                var order = OrderBLL.Read(id);
                if (order.Id <= 0 || order.UserId!=user.Id)
                {
                    return Json(new { flag = false, msg = "订单不存在" });
                }
                if (order.OrderStatus != (int)OrderStatus.HasShipping)
                {
                    return Json(new { flag = false, msg = "当前订单不能进行收货操作" });
                }
                #region 判断是否有退款正在处理中
                //正在处理中的退款订单或商品
                var orderRefundList = OrderRefundBLL.ReadListValid(id);
                //有正在处理中的退款订单或商品，
                if (orderRefundList.Count(k => OrderRefundBLL.RefundGoing(k.Status)) > 0)
                {
                    //取消退款
                    foreach (var item in orderRefundList)
                    {
                        item.Status = (int)OrderRefundStatus.Cancel;
                        OrderRefundBLL.Update(item);
                        OrderRefundActionBLL.Add(new OrderRefundActionInfo
                        {
                            OrderRefundId = item.Id,
                            Status = (int)BoolType.True,
                            Tm = DateTime.Now,
                            UserType = 1,
                            UserId = user.Id,
                            UserName = user.UserName,
                            Remark = "用户取消退款"
                        });
                    }
                }
                #endregion
                int startOrderStatus = order.OrderStatus;
                order.OrderStatus = (int)OrderStatus.ReceiveShipping;
                OrderBLL.UserUpdateOrderAddAction(order, "用户确认收货", (int)OrderOperate.Received, startOrderStatus);
                #region 会员确认收货赠送优惠券
                int count = 0;
                var couponlist = CouponBLL.SearchList(1, 1, new CouponSearchInfo { Type = (int)CouponKind.ReceiveShippingGet, CanUse = 1 }, ref count);
                if (couponlist.Count > 0)
                {
                    UserCouponInfo userCoupon = UserCouponBLL.ReadLast(couponlist[0].Id);
                    int startNumber = 0;
                    if (userCoupon.Id > 0)
                    {
                        string tempNumber = userCoupon.Number.Substring(3, 5);
                        while (tempNumber.Substring(0, 1) == "0")
                        {
                            tempNumber = tempNumber.Substring(1);
                        }
                        startNumber = Convert.ToInt32(tempNumber);
                    }
                    startNumber++;
                    UserCouponBLL.Add(new UserCouponInfo
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        CouponId = couponlist[0].Id,
                        GetType = (int)CouponType.ReceiveShippingGet,
                        Number = ShopCommon.CreateCouponNo(couponlist[0].Id, startNumber),
                        Password = ShopCommon.CreateCouponPassword(startNumber),
                        IsUse = (int)BoolType.False,
                        OrderId = 0

                    });
                }
                #endregion
                #region 确认收货获得积分
                //根据订单付款金额全额返还积分
                int sendPoint = (int)Math.Floor(OrderBLL.ReadNoPayMoney(order));
                if (sendPoint > 0 && order.IsActivity == (int)BoolType.False)
                {
                    var accountRecord = new UserAccountRecordInfo
                    {
                        RecordType = (int)AccountRecordType.Point,
                        Money = 0,
                        Point = sendPoint,
                        Date = DateTime.Now,
                        IP = ClientHelper.IP,
                        Note = ShopLanguage.ReadLanguage("OrderReceived").Replace("$OrderNumber", order.OrderNumber),
                        UserId = order.UserId,
                        UserName = order.UserName
                    };
                    UserAccountRecordBLL.Add(accountRecord);
                }
                #endregion
                #region 确认收货给分销商返佣
                //订单实际支付金额
                decimal paid_money = OrderBLL.ReadNoPayMoney(order);
                //购买者有推荐人 且 实际支付金额大于0才进行返佣
                if (user.Recommend_UserId > 0 && paid_money > 0)
                {
                    RebateBLL.RebateToDistributor(user, paid_money,order.Id);
                }
                #endregion
                return Json(new { flag = true, msg = "用户确认收货" });
            }
            catch (Exception ex)
            {
                return Json(new { flag = false, msg = "fail" });
            }

        }

        #region 付款成功，打印送货单      
        public ActionResult PrintOrder(int orderid)
        {
            string USER = "suano2006@163.com";//*必填*：登录管理后台的账号名
            string UKEY = "keYsNeKcnWCmBVJ4";//*必填*: 注册账号后生成的UKEY
            string SN = ShopConfig.ReadConfigInfo().PrintSN;//*必填*：打印机编号，必须要在管理后台里添加打印机之后，才能调用API
            string URL = "http://api.feieyun.cn/Api/Open/";//不需要修改

            if (!string.IsNullOrEmpty(SN))
            {
                var orderlist = OrderBLL.SearchList(new OrderSearchInfo() { IsNoticed = 0, OrderStatus = (int)OrderStatus.WaitCheck });
                if (orderlist.Count > 0)
                {
                    try
                    {
                        foreach (var item in orderlist)
                        {
                            if (item.SelfPick != 1)
                            {//如果不是自提   才打印发货单
                                print(item, USER, UKEY, SN, URL);
                            }
                        }
                        int[] orderids = orderlist.Select(k => k.Id).ToArray();
                        OrderBLL.UpdateIsNoticed(orderids, 1);

                        return Json(new { flag = true, msg = "打印成功" }, JsonRequestBehavior.AllowGet);
                    }
                    catch
                    {
                        return Json(new { flag = false, msg = "打印出错" }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(new { flag = false, msg = "没有订单" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { flag = false, msg = "没有配置打印机" }, JsonRequestBehavior.AllowGet);
            }

        }
        private string print(OrderInfo order, string USER, string UKEY, string SN, string URL)
        {

            //拼凑订单内容时可参考如下格式
            string orderInfo;
            orderInfo = "<CB>" + order.OrderNumber + "</CB><BR>";//标题字体如需居中放大,就需要用标签套上
            orderInfo += "名称　　　　　 单价  数量 金额<BR>";
            orderInfo += "--------------------------------<BR>";
            var orderdlist = OrderDetailBLL.ReadList(order.Id);
            foreach (var item in orderdlist)
            {
                var tempPro = ProductBLL.Read(item.ProductId);
                orderInfo += item.ProductName + "　　　　　　 " + item.ProductPrice + "    " + item.BuyCount + "   " + item.BuyCount * item.ProductPrice + " <BR>";
            }

            orderInfo += "备注：" + order.UserMessage + "<BR>";
            orderInfo += "--------------------------------<BR>";
            orderInfo += "合计：" + OrderBLL.ReadNoPayMoney(order) + "元<BR>";
            orderInfo += "收货地点：" + RegionBLL.RegionNameList(order.RegionId) + " " + order.Address + "<BR>";

            orderInfo += "联系电话：" + order.Mobile + "<BR>";
            orderInfo += "下单时间：" + order.PayDate + "<BR>";
            //orderInfo += "----------请扫描二维码----------";
            //orderInfo += "<QR>http://www.dzist.com</QR>";//把二维码字符串用标签套上即可自动生成二维码
            //orderInfo += "<BR>";

            orderInfo = Uri.EscapeDataString(orderInfo);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(URL);
            req.Method = "POST";
            UTF8Encoding encoding = new UTF8Encoding();

            string postData = "sn=" + SN;
            postData += ("&content=" + orderInfo);
            postData += ("&times=" + "1");//默认1联

            int itime = DateTimeToStamp(System.DateTime.Now);//时间戳秒数
            string stime = itime.ToString();
            string sig = sha1(USER, UKEY, stime);

            //公共参数
            postData += ("&user=" + USER);
            postData += ("&stime=" + stime);
            postData += ("&sig=" + sig);
            postData += ("&apiname=" + "Open_printMsg");

            byte[] data = encoding.GetBytes(postData);

            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;
            Stream resStream = req.GetRequestStream();

            resStream.Write(data, 0, data.Length);
            resStream.Close();

            HttpWebResponse response;
            string strResult;
            try
            {
                response = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                strResult = reader.ReadToEnd();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                strResult = response.StatusCode.ToString();//错误信息
            }

            response.Close();
            req.Abort();

            return strResult;

        }
        //签名USER,UKEY,STIME
        public static string sha1(string user, string ukey, string stime)
        {
            var buffer = Encoding.UTF8.GetBytes(user + ukey + stime);
            var data = SHA1.Create().ComputeHash(buffer);

            var sb = new StringBuilder();
            foreach (var t in data)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString().ToLower();

        }
        private static int DateTimeToStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); return (int)(time - startTime).TotalSeconds;
        }
        #endregion
        public ActionResult GetOrderPayParams(string orderIds, string code,string formid)
        {
            #region 存储formid  
            /*        
            if (!string.IsNullOrEmpty(formid))
            {
                WxFormIdBLL.Add(new WxFormIdInfo
                {
                    FormId = formid,
                    Used = 0,
                    UserId = user.Id,
                    AddDate = DateTime.Now
                });
            }
            */
            #endregion
            JsApiPay jsApiPay = new JsApiPay();
            jsApiPay.GetOpenidAndAccessToken(orderIds, code);

            List<OrderInfo> orders = new List<OrderInfo>();
            try
            {
                int[] ids = Array.ConvertAll<string, int>(orderIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
                orders = OrderBLL.ReadList(ids);

                if (ids.Length != orders.Count)
                {
                    return Json(new { flag = false, msg = "包含无效的订单" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { flag = false, msg = "无效的请求" }, JsonRequestBehavior.AllowGet);
            }

            //付款金额
            decimal total_price = 0;
            foreach (var order in orders)
            {
                total_price += order.ProductMoney - order.FavorableMoney + order.ShippingMoney + order.OtherMoney - order.Balance - order.CouponMoney - order.PointMoney;
            }
            //支付金额， 单位是分，不能有小数 
            jsApiPay.total_fee = Convert.ToInt32(total_price * 100);

            //可能有多个订单合并付款，用逗号分隔
            jsApiPay.attach = string.Join(",", orders.Select(k => k.Id));

            //检测是否给当前页面传递了相关参数
            if (string.IsNullOrEmpty(jsApiPay.openid) || jsApiPay.total_fee <= 0)
            {
                Log.Error(this.GetType().ToString(), "This page have not get params, cannot be inited, exit...");
                return Json(new { flag = false, msg = "页面传参出错,请返回重试" }, JsonRequestBehavior.AllowGet);
            }
            jsApiPay.order_body = string.Join(",", orders.Select(k => k.OrderNumber));

            //JSAPI支付预处理
            try
            {
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                var wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                //JObject wxJsApiParam = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(jsApiPay.GetJsApiParameters());//获取H5调起JS API参数     
                Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);
                return Json(new { flag = true, wxparam = wxJsApiParam }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { flag = false, msg = "下单失败，请返回重试." + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 根据购物车计算物流费用
        /// </summary>
        /// <param name="shipid"></param>
        /// <param name="addressId"></param>
        /// <param name="checkCart"></param>
        /// <returns></returns>
        public ActionResult GetShippingMoney(int shipid, int addressId, string checkCart)
        {
            int[] cartIds = Array.ConvertAll<string, int>(checkCart.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
            if (string.IsNullOrEmpty(checkCart) || cartIds.Length < 1)
            {
                return Json(new { flag = false, msg = "请选择需要购买的商品" }, JsonRequestBehavior.AllowGet);
            }

            var address = UserAddressBLL.Read(addressId, uid);

            //计算配送费用
            List<CartInfo> cartList = CartBLL.ReadList(uid).Where(k => cartIds.Contains(k.Id)).ToList();
            if (cartList.Count < 1)
            {
                return Json(new { flag = false, msg = "请选择需要购买的商品" }, JsonRequestBehavior.AllowGet);
            }

            ShippingInfo shipping = ShippingBLL.Read(shipid);
            ShippingRegionInfo shippingRegion = ShippingRegionBLL.SearchShippingRegion(shipid, address.RegionId);

            decimal shippingMoney = ShippingRegionBLL.ReadShippingMoney(shipid, address.RegionId, cartList);

            return Json(new { flag = true, shipmoney = shippingMoney }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据团购商品计算物流费用
        /// </summary> 
        /// <param name="shipid"></param>
        /// <param name="addressId"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public ActionResult GetGroupBuyShippingMoney(int shipid, int addressId, int pid)
        {
            if (pid <= 0)
            {
                return Json(new { flag = false, msg = "请选择需要购买的商品" }, JsonRequestBehavior.AllowGet);
            }
            var product = ProductBLL.Read(pid);
            if (product.Id <= 0)
            {
                return Json(new { flag = false, msg = "请选择需要购买的商品" }, JsonRequestBehavior.AllowGet);
            }
            var address = UserAddressBLL.Read(addressId, uid);

            ShippingInfo shipping = ShippingBLL.Read(shipid);
            ShippingRegionInfo shippingRegion = ShippingRegionBLL.SearchShippingRegion(shipid, address.RegionId);

            //decimal shippingMoney = ShippingRegionBLL.ReadShippingMoney(shipid, address.RegionId, product);
            //营销活动免运费
            decimal shippingMoney = 0;

            return Json(new { flag = true, shipmoney = shippingMoney }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 支付成功推送微信通知
        /// </summary>
        /// <param name="form_id"></param>     
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult SendMsgPayed(string form_id,int orderid)
        {
            string touser = user.OpenId;          
            var order = OrderBLL.Read(orderid);
            if (order.Id > 0)
            {
                    string template_id = ShopConfig.ReadConfigInfo().OrderPayTemplateId;
                    var realpay = order.ProductMoney - order.FavorableMoney - order.PointMoney - order.CouponMoney + order.ShippingMoney + order.OtherMoney;
                    string pronames = string.Empty;
                    var orderDetailList = OrderDetailBLL.ReadList(orderid);
                    pronames = string.Join(",", orderDetailList.Select(k => k.ProductName).ToArray());
                 
                    var data = Json(new
                    {
                        //keyword1 = new { value = order.OrderNumber, color = "#333333" },
                        //keyword2 = new { value = realpay.ToString(), color = "#ff3333" },
                        //keyword3 = new { value = order.AddDate.ToString("yyyy-MM-dd HH:mm:ss"), color = "#333333" },
                        //keyword4 = new { value = pronames, color = "#333333" },
                        //keyword5 = new { value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), color = "#333333" },
                        //keyword6 = new { value = EnumHelper.ReadEnumChineseName<OrderStatus>((int)OrderStatus.WaitCheck), color = "#333333" }

                        keyword1 = new { value = ShopConfig.ReadConfigInfo().Title, color = "#333333" },
                        keyword2 = new { value = ShopConfig.ReadConfigInfo().Address, color = "#ff3333" },                        
                        keyword3 = new { value = ShopConfig.ReadConfigInfo().Tel, color = "#333333" },
                        keyword4 = new { value = order.AddDate.ToString("yyyy-MM-dd HH:mm:ss"), color = "#333333" },
                        keyword5 = new { value = pronames, color = "#333333" }
                    });
                    string access_token = WxGetInfo.IsExistAccess_Token();

                    string url = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + access_token;

                    WxPayData jsondata = new WxPayData();
                    jsondata.SetValue("touser", touser);
                    jsondata.SetValue("template_id", ShopConfig.ReadConfigInfo().OrderPayTemplateId);
                    jsondata.SetValue("page", "pages/poi/index");
                    jsondata.SetValue("form_id", form_id);

                    jsondata.SetValue("data", data.Data);
                    string result = HttpService.PostByJson(jsondata.ToJson(), url, false, 6);

                    JObject jd = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                #region 开团成功 消息推送
                if (order.IsActivity == (int)OrderKind.GroupBuy && order.FavorableActivityId > 0)
                {
                    var group = GroupBuyBLL.Read(order.FavorableActivityId);
                    //如果拼团正在进行并且已经拼满
                    if (group.Id > 0 && group.StartTime <= DateTime.Now && group.EndTime >= DateTime.Now && group.SignCount >= group.Quantity)
                    {
                        SendOpenGroupMsg(group);
                    }
                }
                #endregion
                if ((string)jd["errmsg"] == "ok")
                    {
                        return Json(new { flag = true, msg = "ok", result = jsondata.ToJson().ToString(),groupId=order.FavorableActivityId }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { flag = false, msg = "send fail:" + (string)jd["errcode"] + " " + (string)jd["errmsg"], result = jsondata.ToJson() }, JsonRequestBehavior.AllowGet);
                    }              
            }
            else
            {
                return Json(new { flag = false, msg = "no order" }, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// 自提订单付款成功推送微信通知
        /// </summary>
        /// <param name="form_id"></param>     
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult SendSelfPickMsg(string form_id,int orderid)
        {
            string touser = user.OpenId;
            var order = OrderBLL.Read(orderid);
            if (order.Id > 0 )
            {
                if (order.SelfPick!=1)
                {
                    //如果不是自提 则返回错误码
                    return Json(new { flag = false, msg = "It isn't selfpick" }, JsonRequestBehavior.AllowGet);
                }
                string template_id = ShopConfig.ReadConfigInfo().SelfPickTemplateId;
                var selfPick = PickUpCodeBLL.ReadByOrderId(order.Id);
                //while (selfPick.Id <= 0)
                //{
                //等待微信支付回调，产生提货码
                if (selfPick.Id <= 0)
                {
                    Thread.Sleep(800);
                    selfPick = PickUpCodeBLL.ReadByOrderId(order.Id);
                }
                //}
                if (selfPick.Id > 0)
                {
                var realpay = order.ProductMoney - order.FavorableMoney - order.PointMoney - order.CouponMoney + order.ShippingMoney + order.OtherMoney;
                string pronames = string.Empty;
                var orderDetailList = OrderDetailBLL.ReadList(orderid);
                    pronames = string.Join(",", orderDetailList.Select(k => k.ProductName).ToArray());

                    var data = Json(new
                    {
                        keyword1 = new { value = order.OrderNumber, color = "#333333" },
                        keyword2 = new { value = realpay.ToString(), color = "#ff3333" },
                        keyword3 = new { value = pronames, color = "#333333" },
                        keyword4 = new { value = selfPick.PickCode + "（作为到店提货唯一凭证请妥善保管）", color = "#333333" },
                        keyword5 = new { value = ShopConfig.ReadConfigInfo().Address, color = "#333333" },
                        keyword6 = new { value = ShopConfig.ReadConfigInfo().Tel, color = "#333333" }

                    });
                    string access_token = WxGetInfo.IsExistAccess_Token();

                    string url = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + access_token;

                    WxPayData jsondata = new WxPayData();
                    jsondata.SetValue("touser", touser);
                    jsondata.SetValue("template_id", template_id);
                    jsondata.SetValue("page", "pages/poi/index");
                    jsondata.SetValue("form_id", form_id);

                    jsondata.SetValue("data", data.Data);
                    string result = HttpService.PostByJson(jsondata.ToJson(), url, false, 6);

                    JObject jd = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result);

                    if ((string)jd["errmsg"] == "ok")
                    {                     
                        return Json(new { flag = true, msg = "ok", result = jsondata.ToJson().ToString() }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { flag = false, msg = "send fail:" + (string)jd["errcode"] + " " + (string)jd["errmsg"], result = jsondata.ToJson() }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { flag = false, msg = "no pickcode" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { flag = false, msg = "no order" }, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// 开团成功推送微信通知给团长(已拼满), 参团消息发送给参与者
        /// </summary>
        /// <param name="form_id"></param>     
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult SendOpenGroupMsg(GroupBuyInfo group)
        {
            if (group.Id <= 0)
            {
                return Json(new { flag = false, msg = "no group" }, JsonRequestBehavior.AllowGet);
            }
            if (group.StartTime <= DateTime.Now && group.EndTime >= DateTime.Now && group.SignCount >= group.Quantity)
            {
                var wxFormId = WxFormIdBLL.ReadUnusedByUserId(group.Leader).FirstOrDefault() ?? new WxFormIdInfo();
                if (wxFormId.Id <= 0 || string.IsNullOrEmpty(wxFormId.FormId))
                {
                    return Json(new { flag = false, msg = "no form_id" }, JsonRequestBehavior.AllowGet);
                }
                string form_id = wxFormId.FormId;
                int count = 0;
                var groupSignList = GroupSignBLL.SearchListByGroupId(group.Id, 1, group.Quantity, ref count);
                var groupSign = groupSignList.Where(k => k.UserId == group.Leader).FirstOrDefault() ?? new GroupSignInfo();

                if (groupSign.Id > 0)
                {
                    string template_id = ShopConfig.ReadConfigInfo().OpenGroupTemplateId;
                    var _sendToUser = UserBLL.Read(group.Leader);
                    _sendToUser.UserName = System.Web.HttpUtility.UrlDecode(_sendToUser.UserName, System.Text.Encoding.UTF8);
                    string touser = _sendToUser.OpenId;
                    string pronames = groupSign.ProductName;
                    string groupusers =string.Join("、",groupSignList.Select(k => System.Web.HttpUtility.UrlDecode(k.UserName, System.Text.Encoding.UTF8)).ToArray());

                    var data = Json(new
                    {
                        keyword1 = new { value = pronames, color = "#333333" },
                        keyword2 = new { value = _sendToUser.UserName, color = "#ff3333" },
                        keyword3 = new { value = group.Quantity, color = "#333333" },
                        keyword4 = new { value = group.StartTime.ToString("yyyy-MM-dd HH:mm:ss"), color = "#333333" },
                        keyword5 = new { value = group.EndTime.ToString("yyyy-MM-dd HH:mm:ss"), color = "#333333" },
                        keyword6 = new { value = groupSign.GroupOrderNumber, color = "#333333" },
                        keyword7 = new { value = groupusers, color = "#333333" },
                    });
                    string access_token = WxGetInfo.IsExistAccess_Token();

                    string url = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + access_token;

                    WxPayData jsondata = new WxPayData();
                    jsondata.SetValue("touser", touser);
                    jsondata.SetValue("template_id", template_id);
                    jsondata.SetValue("page", "pages/poi/index");
                    jsondata.SetValue("form_id", form_id);

                    jsondata.SetValue("data", data.Data);
                    string result = HttpService.PostByJson(jsondata.ToJson(), url, false, 6);
                    Log.Debug("send opengroupmsg result", result);
                    JObject jd = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result);


                    if ((string)jd["errmsg"] == "ok")
                    {
                        //改变formid使用状态
                        WxFormIdBLL.ChangeUsed(wxFormId.Id);
                        #region 给团长推送之后再给参与者发
                        foreach (var item in groupSignList.Where(k=>k.UserId!=group.Leader))
                        {
                             wxFormId = WxFormIdBLL.ReadUnusedByUserId(item.UserId).FirstOrDefault() ?? new WxFormIdInfo();
                            if (wxFormId.Id <= 0 || string.IsNullOrEmpty(wxFormId.FormId))
                            {

                                //return Json(new { flag = false, msg = "no form_id" }, JsonRequestBehavior.AllowGet);
                                //如果此参与者没有FormId，则跳过此人，转向下一个参与者
                                continue;
                            }
                             form_id = wxFormId.FormId;
                            string template_id1 = ShopConfig.ReadConfigInfo().GroupSignTemplateId;
                            var _sendToUser1 = UserBLL.Read(item.UserId);
                            var  touser1 = _sendToUser1.OpenId;                          

                           var  data1 = Json(new
                            {
                                keyword1 = new { value = item.SignTime.ToString("yyyy-MM-dd HH:mm:ss"), color = "#333333" },
                                keyword2 = new { value = group.EndTime.ToString("yyyy-MM-dd HH:mm:ss"), color = "#333333" },
                                keyword3 = new { value = item.GroupOrderNumber, color = "#333333" },
                                keyword4 = new { value = pronames, color = "#333333" },
                                keyword5 = new { value = _sendToUser.UserName, color = "#ff3333" },
                                keyword6 = new { value = group.Quantity, color = "#333333" },
                                keyword7 = new { value = groupusers, color = "#333333" }
                           });
                             access_token = WxGetInfo.IsExistAccess_Token();

                             url = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + access_token;

                            WxPayData jsondata1 = new WxPayData();
                            jsondata1.SetValue("touser", touser1);
                            jsondata1.SetValue("template_id", template_id1);
                            jsondata1.SetValue("page", "pages/poi/index");
                            jsondata1.SetValue("form_id", form_id);

                            jsondata1.SetValue("data", data1.Data);
                            var result1 = HttpService.PostByJson(jsondata1.ToJson(), url, false, 6);
                            Log.Debug("send groupsignmsg result", result1);
                            var jd1 = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result1);

                            if ((string)jd1["errmsg"] != "ok")
                            {
                                Log.Debug("send groupsignmsg error", "send fail:" + (string)jd1["errcode"] + " " + (string)jd1["errmsg"]);
                                Log.Debug("touserid", _sendToUser1.Id.ToString());
                                Log.Debug("fromid", form_id);
                                return Json(new { flag = false, msg = "send fail:" + (string)jd1["errcode"] + " " + (string)jd1["errmsg"], result = jsondata1.ToJson() }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                //改变formid使用状态
                                WxFormIdBLL.ChangeUsed(wxFormId.Id);
                            }
                        }
                        #endregion
                        return Json(new { flag = true, msg = "ok", result = jsondata.ToJson().ToString(),groupId = group.Id }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { flag = false, msg = "send fail:" + (string)jd["errcode"] + " " + (string)jd["errmsg"], result = jsondata.ToJson() }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { flag = false, msg = "no groupsign" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { flag = false, msg = "invalid group" }, JsonRequestBehavior.AllowGet);
            }

        }
        #region 参团成功推送微信通知给参与者(已拼满) ,已在开团通知中一并发送，此处无用
        /*
        /// <summary>
        /// 参团成功推送微信通知给参与者(已拼满)
        /// </summary>
        /// <param name="form_id"></param>     
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult SendGroupSignMsg(int groupId)
        {

            var group = GroupBuyBLL.Read(groupId);
            if (group.Id <= 0)
            {
                return Json(new { flag = false, msg = "no group" }, JsonRequestBehavior.AllowGet);
            }
            if (group.StartTime <= DateTime.Now && group.EndTime >= DateTime.Now && group.SignCount >= group.Quantity)
            {               
                int count = 0;
                var groupSignList = GroupSignBLL.SearchListByGroupId(group.Id, 1, group.Quantity, ref count).Where(k => k.UserId != group.Leader);
                //循环发给每个参与者
                if (groupSignList.Count() > 0)
                {
                    foreach (var groupSign in groupSignList) {
                        var wxFormId = WxFormIdBLL.ReadUnusedByUserId(groupSign.UserId).FirstOrDefault() ?? new WxFormIdInfo();
                        if (wxFormId.Id <= 0 || string.IsNullOrEmpty(wxFormId.FormId))
                        {
                            return Json(new { flag = false, msg = "no form_id" }, JsonRequestBehavior.AllowGet);
                        }
                        string form_id = wxFormId.FormId;
                        string template_id = ShopConfig.ReadConfigInfo().GroupSignTemplateId;
                        var _sendToUser = UserBLL.Read(groupSign.UserId);
                        string touser = _sendToUser.OpenId;
                        string pronames = groupSign.ProductName;

                        var data = Json(new
                        {
                            keyword1 = new { value = groupSign.SignTime.ToString("yyyy-MM-dd HH:mm:ss"), color = "#333333" },
                            keyword2 = new { value = group.EndTime.ToString("yyyy-MM-dd HH:mm:ss"), color = "#333333" },
                            keyword3 = new { value = groupSign.GroupOrderNumber, color = "#333333" },
                            keyword4 = new { value = pronames, color = "#333333" },
                            keyword5 = new { value = groupSignList.Where(k=>k.UserId==group.Leader).FirstOrDefault().UserName, color = "#ff3333" },
                            keyword6 = new { value = group.Quantity, color = "#333333" }                           
                        });
                        string access_token = WxGetInfo.IsExistAccess_Token();

                        string url = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + access_token;

                        WxPayData jsondata = new WxPayData();
                        jsondata.SetValue("touser", touser);
                        jsondata.SetValue("template_id", template_id);
                        jsondata.SetValue("page", "pages/poi/index");
                        jsondata.SetValue("form_id", form_id);

                        jsondata.SetValue("data", data.Data);
                        string result = HttpService.PostByJson(jsondata.ToJson(), url, false, 6);

                        JObject jd = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result);

                        if ((string)jd["errmsg"] != "ok")
                        {                       
                            return Json(new { flag = false, msg = "send fail:" + (string)jd["errcode"] + " " + (string)jd["errmsg"], result = jsondata.ToJson() }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //改变formid使用状态
                            WxFormIdBLL.ChangeUsed(wxFormId.Id);
                        }
                    }                  
                     return Json(new { flag = true, msg = "ok", result = "success" }, JsonRequestBehavior.AllowGet);
                  
                }
                else
                {
                    return Json(new { flag = false, msg = "no groupsign" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { flag = false, msg = "invalid group" }, JsonRequestBehavior.AllowGet);
            }

        }
        */
        #endregion
        /// <summary>
        /// 用户申请退款
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OrderRefund(int id)
        {          
            var order = OrderBLL.Read(id);
            if(order.Id<=0 || order.UserId != user.Id)
            {
                return Json(new { flag = false, msg = "订单不存在" }, JsonRequestBehavior.AllowGet);
            }
            if (order.OrderStatus != (int)OrderStatus.WaitCheck && order.OrderStatus != (int)OrderStatus.Shipping && order.OrderStatus != (int)OrderStatus.HasShipping)
            {
                return Json(new { flag = false, msg = "当前订单不能申请退款" }, JsonRequestBehavior.AllowGet);
            }
            //正在处理中的退款订单或商品
            var orderRefundList = OrderRefundBLL.ReadListValid(order.Id);
            //有正在处理中的退款订单或商品，禁用功能按钮
            if (orderRefundList.Count(k => !OrderRefundBLL.HasReturn(k.Status)) > 0)
            {
                return Json(new { flag = false, msg = "该订单有正在处理中的退款" }, JsonRequestBehavior.AllowGet);
            }
            //如果是团购单，且拼团正在进行中，暂不能申请退款
            if (order.IsActivity == (int)OrderKind.GroupBuy && order.FavorableActivityId > 0)
            {
                var groupBuy = GroupBuyBLL.Read(order.FavorableActivityId);
                if (groupBuy.StartTime <= DateTime.Now && groupBuy.EndTime >= DateTime.Now && groupBuy.Quantity > groupBuy.SignCount)
                {
                    return Json(new { flag = false, msg = "拼团正在进行，暂不能申请退款" }, JsonRequestBehavior.AllowGet);
                }
            }
            OrderRefundInfo orderRefund = new OrderRefundInfo();
            orderRefund.RefundNumber = ShopCommon.CreateOrderRefundNumber();
            orderRefund.OrderId = id;
            //if (orderDetailId > 0)
            //{
            //    orderRefund.OrderDetailId = orderDetailId;
            //    orderRefund.RefundCount = needRefundCount;
            //}
            orderRefund.Status = (int)OrderRefundStatus.Submit;
            orderRefund.TmCreate = DateTime.Now;
            orderRefund.RefundRemark = "用户提交退款申请";
            orderRefund.UserType = 1;
            orderRefund.UserId = user.Id;
            orderRefund.UserName = user.UserName;


            //默认退全部能退的额度
            var refundMsg = JWRefund.VerifySubmitOrderRefund(orderRefund, JWRefund.CanRefund(order).CanRefundMoney);
            if (refundMsg.CanRefund)
            {
                int refundId = OrderRefundBLL.Add(orderRefund);
                OrderRefundActionBLL.Add(new OrderRefundActionInfo
                {
                    OrderRefundId = refundId,
                    Status = (int)BoolType.True,
                    Tm = DateTime.Now,
                    UserType = 1,
                    UserId = user.Id,
                    UserName = user.UserName,
                    Remark = "用户提交退款申请"
                });
                return Json(new { flag = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { flag = false, msg = refundMsg.ErrorCodeMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 砍价成功 下单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BargainBuy()
        {
            int orderId = RequestHelper.GetQueryString<int>("orderId");
            string msg = string.Empty;
            var bargainOrder = BargainOrderBLL.ReadBargainOrder(orderId);
            var bargainDetail = BargainDetailsBLL.ReadBargainDetails(bargainOrder.BargainDetailsId);
            var product = ProductBLL.Read(bargainDetail.ProductID);
            decimal totalProductMoney = 0;
            //bool free = false;
            if (orderId > 0)
            {
                if (bargainOrder.Status == (int)BargainOrderType.砍价成功)
                {
                    totalProductMoney = bargainDetail.ReservePrice;
                    //if (totalProductMoney==0)
                    //{
                    //    free = true;
                    //}
                }
                else
                {
                    msg = "订单状态错误";
                    Log.Error("BargainBuy", msg);
                }
            }
            else
            {
                msg = "砍价订单错误";
            }
            var addresslist = UserAddressBLL.ReadList(uid);
            List<VirtualAddress> newaddresslist = new List<VirtualAddress>();
            foreach (var item in addresslist)
            {
                VirtualAddress newaddress = new VirtualAddress();
                newaddress.id = item.Id;
                newaddress.name = item.Consignee;
                newaddress.address = RegionBLL.RegionNameList(item.RegionId) + " " + item.Address;
                newaddress.mobile = item.Mobile;
                newaddress.isdefault = Convert.ToBoolean(item.IsDefault);
                newaddresslist.Add(newaddress);
            }
            //营销活动免运费
            decimal shippingMoney = 0;
            //if (addresslist.Count > 0)
            //{
            //    var address = addresslist[0];
            //    ShippingRegionInfo shippingRegion = ShippingRegionBLL.SearchShippingRegion(1, address.RegionId);
            //    shippingMoney = ShippingRegionBLL.ReadShippingMoney(1, address.RegionId, product);
            //}
            var paylist = PayPlugins.ReadProductBuyPayPluginsList();


            return Json(new { ok = true, msg = msg, addresslist = newaddresslist, paylist = paylist, shippingMoney = shippingMoney, totalproductmoney = totalProductMoney, product = product, bargainOrder = bargainOrder }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 砍价成功 提交订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BargainBuyCheckOut()
        {
            string result = "";
            bool flag = true, free = false;

            int bargainOrderId = RequestHelper.GetForm<int>("bargainOrderId");
            string standValue = HttpContext.Server.UrlDecode(RequestHelper.GetQueryString<string>("standvalue"));
            var bargainOrder = BargainOrderBLL.ReadBargainOrder(bargainOrderId);
            if (bargainOrder.Id < 0)
            {
                return Json(new { flag = false, msg = "砍价订单不存在", errorcode = 0 }, JsonRequestBehavior.AllowGet);
            }
            var bargainDetail = BargainDetailsBLL.ReadBargainDetails(bargainOrder.BargainDetailsId);
            if (bargainDetail.Id <= 0)
            {
                return Json(new { flag = false, msg = "活动商品信息不存在", errorcode = 0 }, JsonRequestBehavior.AllowGet);
            }
            if (bargainDetail.Stock -bargainDetail.Sales<= 0)
            {
                return Json(new { flag = false, msg = "砍价活动商品库存不足", errorcode = 0 }, JsonRequestBehavior.AllowGet);
            }
            var bargain = BargainBLL.ReadBargain(bargainDetail.BargainId);
            if (bargain.Id <= 0)
            {
                return Json(new { flag = false, msg = "砍价活动无效", errorcode = 0 }, JsonRequestBehavior.AllowGet);
            }
            else if (DateTime.Now >= bargain.EndDate)
            {
                return Json(new { flag = false, msg = "砍价活动已结束", errorcode = 0 }, JsonRequestBehavior.AllowGet);
            }

            var product = ProductBLL.Read(bargainDetail.ProductID);
            if (product.Id <= 0)
            {
                return Json(new { flag = false, msg = "商品不存在", errorcode = 0 }, JsonRequestBehavior.AllowGet);
            }
            else if (product.IsSale == 0)
            {
                return Json(new { flag = false, msg = "商品已下架", errorcode = 0 }, JsonRequestBehavior.AllowGet);
            }

            //如果有规格值,product.name 带上规格值
            //if (!string.IsNullOrEmpty(standValue)) product.Name += string.Concat("(" + standValue + ")");
            /*-----------必要性检查：收货地址，配送方式，支付方式-------------------*/
            //1-自提，否则：配送
            int selfPick = RequestHelper.GetForm<int>("selfpick") <= 0 ? 0 : 1;
            var address = new UserAddressInfo { Id = RequestHelper.GetForm<int>("address_id") };
            var shipping = new ShippingInfo { Id = RequestHelper.GetForm<int>("ShippingId") };
            var pay = new PayPluginsInfo { Key = "WxPay" };
            //订单优惠活动
            var favor = new FavorableActivityInfo { Id = RequestHelper.GetForm<int>("favorableActivityId") };
            //商品优惠
            var productfavor = new FavorableActivityInfo { Id = RequestHelper.GetForm<int>("ProductFavorableActivity") };
            bool reNecessaryCheck = false;
            doReNecessaryCheck:
            //如果选择配送
            if (selfPick != 1)
            {
                if (address.Id < 1)
                {
                    result = "请选择收货地址";
                    flag = false;
                }
                if (shipping.Id < 1)
                {
                    result = "请选择配送方式";
                    flag = false;
                }

                //读取数据库中的数据，进行重复验证
                if (!reNecessaryCheck)
                {
                    address = UserAddressBLL.Read(address.Id, uid);
                    shipping = ShippingBLL.Read(shipping.Id);
                    pay = PayPlugins.ReadPayPlugins(pay.Key);

                    reNecessaryCheck = true;
                    goto doReNecessaryCheck;
                }
            }
            /*-----------商品清单、商品总价、邮费价格、库存检查---------------------*/
            decimal productMoney = bargainDetail.ReservePrice, pointMoney = 0;
            int count = 0;
            //输入的兑换积分数
            var costPoint = RequestHelper.GetForm<int>("costPoint");
            int leftStorageCount = 0;
            int buyCount = RequestHelper.GetForm<int>("buyCount") <= 0 ? 1 : RequestHelper.GetForm<int>("buyCount");
            //if (product.StandardType == (int)ProductStandardType.Single)
            //{
            //    if (!string.IsNullOrEmpty(standValue))
            //    {
            //        //使用规格的库存
            //        var standardRecord = ProductTypeStandardRecordBLL.Read(product.Id, standValue);
            //        leftStorageCount = standardRecord.Storage - (product.UnlimitedStorage != 1 ? standardRecord.OrderCount : OrderBLL.GetProductOrderCountDaily(product.Id, product.StandardType, DateTime.Now, standardRecord.ValueList));
            //        productMoney += standardRecord.GroupPrice * buyCount;
            //    }
            //}
            //else
            //{
            //    leftStorageCount = product.TotalStorageCount - (product.UnlimitedStorage != 1 ? product.OrderCount : OrderBLL.GetProductOrderCountDaily(product.Id, product.StandardType, DateTime.Now));
            //    productMoney += product.GroupPrice * buyCount;
            //}

            //检查库存
            //if (buyCount > leftStorageCount)
            //{
            //    result = "商品[" + product.Name + "]库存不足，无法购买";
            //    flag = false;
            //}


            ShippingRegionInfo shippingRegion = selfPick == 1 ? new ShippingRegionInfo() : ShippingRegionBLL.SearchShippingRegion(shipping.Id, address.RegionId);
            //decimal shippingMoney = selfPick == 1 ? 0 : ShippingRegionBLL.ReadShippingMoney(shipping.Id, shippingRegion.RegionId, product);
            //营销活动免运费
            decimal shippingMoney = 0;
            /*----------------------------------------------------------------------*/
            #region 优惠券
            decimal couponMoney = 0;
            string userCouponStr = RequestHelper.GetForm<string>("UserCoupon");
            UserCouponInfo userCoupon = new UserCouponInfo();
            if (userCouponStr != string.Empty)
            {
                int couponID = 0;
                if (int.TryParse(userCouponStr.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0], out couponID))
                {
                    userCoupon = UserCouponBLL.Read(couponID, uid);
                    if (userCoupon.UserId == uid && userCoupon.IsUse == 0)
                    {
                        CouponInfo tempCoupon = CouponBLL.Read(userCoupon.CouponId);
                        if (tempCoupon.UseMinAmount <= productMoney)
                        {
                            couponMoney = CouponBLL.Read(userCoupon.CouponId).Money;
                        }
                        else
                        {
                            result = "结算金额小于该优惠券要求的最低消费的金额";
                            flag = false;
                        }
                    }
                }
            }
            #endregion
            #region 如果开启了：使用积分抵现,计算积分抵现的现金金额
            if (ShopConfig.ReadConfigInfo().EnablePointPay == 1)
            {
                if (costPoint > user.PointLeft)
                {
                    result = "输入的兑换积分数[" + costPoint + "]超过现有积分，请检查";
                    flag = false;
                }
                if (costPoint > 0)
                {
                    var PointToMoneyRate = ShopConfig.ReadConfigInfo().PointToMoney;
                    pointMoney = costPoint * (decimal)PointToMoneyRate / 100;
                }
            }
            #endregion
            #region 结算商品优惠金额
            decimal productfavorableMoney = 0;
            var theFavor = FavorableActivityBLL.Read(productfavor.Id);
            if (theFavor.Id > 0)
            {
                decimal tmoney = 0;


                if (product.ClassId.IndexOf(theFavor.ClassIds) > -1)
                {
                    if (!string.IsNullOrEmpty(standValue))
                    {
                        //使用规格的库存
                        var standardRecord = ProductTypeStandardRecordBLL.Read(product.Id, standValue);
                        tmoney += standardRecord.GroupPrice * buyCount;
                    }
                    else
                    {
                        tmoney += product.GroupPrice * buyCount;
                    }

                }
                switch (theFavor.ReduceWay)
                {
                    case (int)FavorableMoney.Money:
                        productfavorableMoney += theFavor.ReduceMoney;
                        break;
                    case (int)FavorableMoney.Discount:
                        productfavorableMoney += tmoney * (100 - theFavor.ReduceDiscount) / 100;
                        break;
                    default:
                        break;
                }
            }
            #endregion
            #region 计算订单优惠活动金额
            decimal favorableMoney = 0;
            favor = FavorableActivityBLL.Read(favor.Id);
            if (favor.Id > 0)
            {
                if (("," + favor.UserGrade + ",").IndexOf("," + userGrade.ToString() + ",") > -1 && productMoney >= favor.OrderProductMoney)
                {
                    switch (favor.ReduceWay)
                    {
                        case (int)FavorableMoney.Money:
                            favorableMoney += favor.ReduceMoney;
                            break;
                        case (int)FavorableMoney.Discount:
                            favorableMoney += productMoney * (100 - favor.ReduceDiscount) / 100;
                            break;
                        default:
                            break;
                    }
                    if (favor.ShippingWay == (int)FavorableShipping.Free && ShippingRegionBLL.IsRegionIn(address.RegionId, favor.RegionId))
                    {
                        favorableMoney += shippingMoney;
                    }
                }
            }
            #endregion
           
            decimal payMoney = productMoney + shippingMoney - couponMoney - pointMoney - favorableMoney - productfavorableMoney;
            //砍价   可以0元拿
            if (payMoney < 0)
            {
                result = "金额有错误，请重新检查";
                flag = false;
            }
            int orderId = 0;
            if (flag)
            {

                #region /*-----------组装基础订单模型，循环生成订单-----------------------------*/
                OrderInfo order = new OrderInfo();
                order.ProductMoney = productMoney;
                order.OrderNumber = ShopCommon.CreateOrderNumber();
                string payKey = RequestHelper.GetForm<string>("Pay");
                PayPluginsInfo payPlugins = PayPlugins.ReadPayPlugins(payKey);
                if (payMoney == 0 || payPlugins.IsCod == (int)BoolType.True)
                {
                    order.OrderStatus = (int)OrderStatus.WaitCheck;
                }
                else
                {
                    order.OrderStatus = (int)OrderStatus.WaitPay;
                }
                //新增一条砍价订单（IsActivity=3）bargainOrder  Id（FavorableActivityId）              
                order.IsActivity = (int)OrderKind.Bargain;
                order.FavorableActivityId = bargainOrder.Id;

                order.Consignee = address.Consignee;
                order.RegionId = address.RegionId;
                order.Address = address.Address;
                order.ZipCode = address.ZipCode;
                order.Tel = address.Tel;
                order.Mobile = address.Mobile;
                order.InvoiceTitle = RequestHelper.GetForm<string>("InvoiceTitle");
                order.InvoiceContent = RequestHelper.GetForm<string>("InvoiceContent");
                order.GiftMessige = RequestHelper.GetForm<string>("GiftMessige");
                order.Email = CookiesHelper.ReadCookieValue("UserEmail");
                order.ShippingId = shipping.Id;
                order.ShippingDate = RequestHelper.DateNow;
                order.ShippingMoney = shippingMoney;
                order.CouponMoney = couponMoney;
                order.Point = costPoint;
                order.PointMoney = pointMoney;
                order.FavorableMoney = favorableMoney + productfavorableMoney;
                order.Balance = 0;
                order.PayKey = pay.Key;
                order.PayName = pay.Name;
                order.PayDate = RequestHelper.DateNow;
                order.IsRefund = (int)BoolType.False;
                order.AddDate = RequestHelper.DateNow;
                order.IP = ClientHelper.IP;
                order.UserId = uid;
                order.UserName = user.UserName;
                order.UserMessage = RequestHelper.GetForm<string>("userMessage");
                order.GiftId = RequestHelper.GetForm<int>("GiftID");
                order.IsNoticed = 0;
                order.SelfPick = selfPick;
                order.BargainOrderId = bargainOrder.Id;
                //order.OrderNote = payDiscountNotice;
                orderId = OrderBLL.Add(order);
                #endregion
                #region 添加订单详情--购买明细

                var orderDetail = new OrderDetailInfo();
                orderDetail.OrderId = orderId;
                orderDetail.ProductId = product.Id;
                orderDetail.ProductName = product.Name;
                orderDetail.StandardValueList = standValue;
                orderDetail.ProductWeight = product.Weight;
                //if (!string.IsNullOrEmpty(standValue))
                //{
                //    var standardRecord = ProductTypeStandardRecordBLL.Read(product.Id, standValue);
                //    //orderDetail.ProductPrice = standardRecord.GroupPrice;
                //}
                //else
                //{
                //    orderDetail.ProductPrice = product.GroupPrice;
                //}
                orderDetail.ProductPrice = bargainDetail.ReservePrice;

                orderDetail.BidPrice = product.BidPrice;
                orderDetail.BuyCount = buyCount;

                OrderDetailBLL.Add(orderDetail);
                #endregion


                #region 更新优惠券状态--已使用
                //使用优惠券
                if (couponMoney > 0 && userCouponStr != "0|0")
                {
                    userCoupon.IsUse = (int)BoolType.True;
                    userCoupon.OrderId = orderId;
                    UserCouponBLL.Update(userCoupon);
                }
                #endregion
                #region 减少积分
                if (ShopConfig.ReadConfigInfo().EnablePointPay == 1 && costPoint > 0)
                {
                    //减少积分
                    UserAccountRecordInfo uarInfo = new UserAccountRecordInfo();
                    uarInfo.RecordType = (int)AccountRecordType.Point;
                    uarInfo.UserId = uid;
                    uarInfo.UserName = user.UserName;
                    uarInfo.Note = "支付订单：" + order.OrderNumber;
                    uarInfo.Point = -costPoint;
                    uarInfo.Money = 0;
                    uarInfo.Date = DateTime.Now;
                    uarInfo.IP = ClientHelper.IP;
                    UserAccountRecordBLL.Add(uarInfo);
                }
                #endregion
                //更改产品库存订单数量
                ProductBLL.ChangeOrderCountByOrder(orderId, ChangeAction.Plus);

                //更改活动商品销量
                //bargainDetail.Stock -= 1;
                if (order.ProductMoney==0)
                {//已支付完成
                    BargainOrderBLL.HandleBargainOrderPay(bargainOrder.Id);
                    //bargainOrder.Status = (int)BargainOrderType.支付完成;
                    //bargainDetail.Sales += 1;                   
                    free = true;
                }
                else
                {
                    //更新砍价订单状态
                    bargainOrder.Status = (int)BargainOrderType.待付款;
                    BargainOrderBLL.UpdateBargainOrder(bargainOrder);
                }
                //更新砍价订单状态  
                //if (BargainOrderBLL.UpdateBargainOrder(bargainOrder))
                //{
                //    //如果支付完成,则砍价商品成功数加1
                //    if (bargainOrder.Status == (int)BargainOrderType.支付完成)
                //    {
                //        BargainDetailsBLL.UpdateBargainDetails(bargainDetail);
                //        //bargain的砍价销量加1
                //        Dictionary<string, object> dict = new Dictionary<string, object>();
                //        dict.Add("[SalesVolume]", bargain.SalesVolume + 1);
                //        BargainBLL.UpdatePart("[Bargain]", dict, bargain.Id); 
                //    }
                //}
                #region 存储formid
                //string formid = RequestHelper.GetForm<string>("formid");
                //if (!string.IsNullOrEmpty(formid))
                //{
                //    WxFormIdBLL.Add(new WxFormIdInfo
                //    {
                //        FormId = formid,
                //        Used = 0,
                //        UserId = user.Id,
                //        AddDate = DateTime.Now
                //    });
                //}
                #endregion
            }

            return Json(new { flag = flag, msg = result, errorcode = flag ? 200 : 0, orderid = orderId, free = free });
        }
    }
}
