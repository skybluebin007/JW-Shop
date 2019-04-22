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
using Newtonsoft.Json;

namespace JWShop.Page
{
    public class CheckOut : CommonBasePage
    {
        protected string checkCart;
        protected List<CartInfo> cartList = new List<CartInfo>();
        protected List<UserAddressInfo> addressList = new List<UserAddressInfo>();
        protected List<PayPluginsInfo> payPluginsList = new List<PayPluginsInfo>();
        protected SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();
        protected int islpProd = 0;
      
        //积分
        protected int pointLeft;
        //商品优惠活动
        protected List<FavorableActivityInfo> productFavorableActivityList = new List<FavorableActivityInfo>();
        /// <summary>
        /// 用户优惠券列表
        /// </summary>
        protected List<UserCouponInfo> userCouponList = new List<UserCouponInfo>();

        /// <summary>
        /// 优惠活动
        /// </summary>
        protected FavorableActivityInfo favorableActivity = new FavorableActivityInfo();
        protected override void PageLoad()
        {
            base.PageLoad();
            istop = 1;
            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "Submit":
                    this.Submit();
                    break;
                case"SelectProductFavor"://读取商品优惠
                    this.SelectProductFavor();
                    break;
                case "ReadingGifts"://读取礼品列表
                    this.ReadingGifts();
                    break;                    
            }

            //登录验证
            if (base.UserId <= 0)
            {
                ResponseHelper.Redirect("/user/login.html?RedirectUrl=/checkout.html");
                ResponseHelper.End();
            }
            if (base._UserType == (int)UserType.Provider)
            {
                ResponseHelper.Redirect("/");
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
            var user = UserBLL.ReadUserMore(base.UserId);
            //剩余积分
            pointLeft = user.PointLeft;
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
            var productList = ProductBLL.SearchList(1, ids.Length, new ProductSearchInfo { InProductId = string.Join(",", ids) }, ref count);

            //规格
            foreach (var cart in cartList)
            {
                cart.Product = productList.FirstOrDefault(k => k.Id == cart.ProductId) ?? new ProductInfo();

                if (!string.IsNullOrEmpty(cart.StandardValueList))
                {
                    //使用规格的库存
                    var standardRecord = ProductTypeStandardRecordBLL.Read(cart.ProductId, cart.StandardValueList);
                    cart.LeftStorageCount = standardRecord.Storage - standardRecord.OrderCount;
                    cart.Price = ProductBLL.GetCurrentPrice(standardRecord.SalePrice, base.GradeID);
                    //规格集合
                    if (!string.IsNullOrEmpty(standardRecord.StandardIdList))
                    {
                        cart.Standards = ProductTypeStandardBLL.ReadList(Array.ConvertAll<string, int>(standardRecord.StandardIdList.Split(';'), k => Convert.ToInt32(k)));
                    }
                }
                else
                {
                    cart.Price = ProductBLL.GetCurrentPrice(cart.Product.SalePrice, base.GradeID);
                    cart.LeftStorageCount = cart.Product.TotalStorageCount - cart.Product.OrderCount;
                }


                //检查库存
                if (cart.BuyCount > cart.LeftStorageCount)
                {
                    ScriptHelper.AlertFront("商品[" + cart.ProductName + "]库存不足，无法购买");
                    ResponseHelper.End();
                }
            }

            #endregion

            //收货地址
            addressList = UserAddressBLL.ReadList(base.UserId);
            addressList = addressList.OrderByDescending(k => k.IsDefault).ToList();
            singleUnlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();

            var totalProductMoney = cartList.Sum(k => k.BuyCount * k.Price);

            //支付方式列表
            payPluginsList = PayPlugins.ReadProductBuyPayPluginsList();
            #region 优惠券
            if (user.Id > 0)
            {
                //读取优惠券
                List<UserCouponInfo> tempUserCouponList = UserCouponBLL.ReadCanUse(base.UserId);
                foreach (UserCouponInfo userCoupon in tempUserCouponList)
                {
                    CouponInfo tempCoupon = CouponBLL.Read(userCoupon.CouponId);
                    if (tempCoupon.UseMinAmount<= totalProductMoney)
                    {
                        userCouponList.Add(userCoupon);
                    }
                }

              
            }
            #endregion
            #region 获取符合条件（时间段，用户等级，金额限制）的商品分类优惠活动列表，默认使用第一个

            var tmpfavorableActivityList = FavorableActivityBLL.ReadList(DateTime.Now, DateTime.Now).Where<FavorableActivityInfo>(f => f.Type == (int)FavorableType.ProductClass && ("," + f.UserGrade + ",").IndexOf("," + base.GradeID.ToString() + ",") > -1).ToList();
            foreach (var favorable in tmpfavorableActivityList)
            {
                decimal tmoney = 0;
                //tmoney = cartList.Where(c => c.Product.ClassId.IndexOf(favorable.ClassIds) > -1).Sum(k => k.BuyCount * k.Price);
                foreach (var tmpcart in cartList)
                {
                    if (tmpcart.Product.ClassId.IndexOf(favorable.ClassIds) > -1)
                    {
                        if (!string.IsNullOrEmpty(tmpcart.StandardValueList))
                        {
                            //使用规格的库存
                            var standardRecord = ProductTypeStandardRecordBLL.Read(tmpcart.ProductId, tmpcart.StandardValueList);
                            tmpcart.LeftStorageCount = standardRecord.Storage - standardRecord.OrderCount;
                            tmpcart.Price = ProductBLL.GetCurrentPrice(standardRecord.SalePrice, base.GradeID);
                            tmoney += tmpcart.Price*tmpcart.BuyCount;
                        }
                        else
                        {
                            tmpcart.Price = ProductBLL.GetCurrentPrice(tmpcart.Product.SalePrice, base.GradeID);
                            tmoney += tmpcart.Price*tmpcart.BuyCount;
                        }
                    }
                }
                if (tmoney >= favorable.OrderProductMoney) { productFavorableActivityList.Add(favorable); }
            }
            #endregion
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
                ResponseHelper.Write("error|购买商品发生了变化，请重新提交|/cart.html");
                ResponseHelper.End();
            }

            if (string.IsNullOrEmpty(checkCart) || cartIds.Length < 1)
            {
                ResponseHelper.Write("error|请选择需要购买的商品|/cart.html");
                ResponseHelper.End();
            }
            /*----------------------------------------------------------------------*/

            /*-----------读取购物车清单---------------------------------------------*/
            List<CartInfo> cartList = CartBLL.ReadList(base.UserId);
            cartList = cartList.Where(k => cartIds.Contains(k.Id)).ToList();
            if (cartList.Count <= 0)
            {
                ResponseHelper.Write("error|请选择需要购买的商品|/cart.html");
                ResponseHelper.End();
            }
            /*----------------------------------------------------------------------*/

            /*-----------必要性检查：收货地址，配送方式，支付方式-------------------*/
            var address = new UserAddressInfo { Id = RequestHelper.GetForm<int>("address_id") };
            var shipping = new ShippingInfo { Id = RequestHelper.GetForm<int>("ShippingId") };
            var pay = new PayPluginsInfo { Key = StringHelper.AddSafe(RequestHelper.GetForm<string>("pay")) };
            //订单优惠活动
            var favor = new FavorableActivityInfo { Id = RequestHelper.GetForm<int>("FavorableActivity") };
            //商品优惠
            var productfavor = new FavorableActivityInfo { Id = RequestHelper.GetForm<int>("ProductFavorableActivity") };
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
            var user = UserBLL.ReadUserMore(base.UserId);
            decimal productMoney = 0,pointMoney=0;
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
                    cart.LeftStorageCount = standardRecord.Storage - standardRecord.OrderCount;
                    productMoney += ProductBLL.GetCurrentPrice(standardRecord.SalePrice, base.GradeID) * (cart.BuyCount);
                }
                else
                {
                    cart.LeftStorageCount = cart.Product.TotalStorageCount - cart.Product.OrderCount;
                    productMoney += ProductBLL.GetCurrentPrice(cart.Product.SalePrice, base.GradeID) * (cart.BuyCount);
                }

                //检查库存
                if (cart.BuyCount > cart.LeftStorageCount)
                {
                    ResponseHelper.Write("error|商品[" + cart.ProductName + "]库存不足，无法购买|");
                    ResponseHelper.End();
                }
            }

            ShippingRegionInfo shippingRegion = ShippingRegionBLL.SearchShippingRegion(shipping.Id, address.RegionId);
            decimal shippingMoney = ShippingRegionBLL.ReadShippingMoney(shipping.Id, shippingRegion.RegionId, cartList);
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
                    userCoupon = UserCouponBLL.Read(couponID, base.UserId);
                    if (userCoupon.UserId == base.UserId && userCoupon.IsUse == 0)
                    {
                         CouponInfo tempCoupon = CouponBLL.Read(userCoupon.CouponId);
                         if (tempCoupon.UseMinAmount <= productMoney)
                         {
                             couponMoney = CouponBLL.Read(userCoupon.CouponId).Money;
                         }
                         else
                         {
                             ResponseHelper.Write("error|结算金额小于该优惠券要求的最低消费的金额|");
                             ResponseHelper.End();
                         }
                    }
                }
            }
            #endregion
            #region 如果开启了：使用积分抵现,计算积分抵现的现金金额
            if (ShopConfig.ReadConfigInfo().EnablePointPay == 1)
            {
                if (costPoint > user.PointLeft || costPoint < 0)
                {
                    ResponseHelper.Write("error|输入的兑换积分数[" + costPoint + "]错误，请检查|");
                    ResponseHelper.End();
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
                            tmpcart.LeftStorageCount = standardRecord.Storage - standardRecord.OrderCount;
                            tmpcart.Price = ProductBLL.GetCurrentPrice(standardRecord.SalePrice, base.GradeID);
                            tmoney += tmpcart.Price*tmpcart.BuyCount;
                        }
                        else
                        {
                            tmpcart.Price = ProductBLL.GetCurrentPrice(tmpcart.Product.SalePrice, base.GradeID);
                            tmoney += tmpcart.Price*tmpcart.BuyCount;
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
                if (("," + favor.UserGrade + ",").IndexOf("," + base.GradeID.ToString() + ",") > -1 && productMoney >= favor.OrderProductMoney)
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
            /*-----------应付总价---------------------------------------------------*/
            decimal payMoney = productMoney + shippingMoney - couponMoney - pointMoney - favorableMoney - productfavorableMoney;
            /*----------------------------------------------------------------------*/

            /*-----------检查金额---------------------------------------------------*/
            if (payMoney <= 0)
            {
                ResponseHelper.Write("error|金额有错误，请重新检查|");
                ResponseHelper.End();
            }
            /*----------------------------------------------------------------------*/

           
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
            order.UserId = base.UserId;
            order.UserName = base.UserName;
            order.UserMessage = RequestHelper.GetForm<string>("userMessage");
            order.GiftId = RequestHelper.GetForm<int>("GiftID");
            order.IsNoticed = 0;
            int orderId = OrderBLL.Add(order);

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
                    orderDetail.ProductPrice = ProductBLL.GetCurrentPrice(standardRecord.SalePrice, base.GradeID);
                }
                else
                {
                    orderDetail.ProductPrice = ProductBLL.GetCurrentPrice(cart.Product.SalePrice, base.GradeID);
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
                    uarInfo.UserId = base.UserId;
                    uarInfo.UserName = base.UserName;
                    uarInfo.Note = "支付订单：" +order.OrderNumber;
                    uarInfo.Point = -costPoint;
                    uarInfo.Money = 0;
                    uarInfo.Date = DateTime.Now;
                    uarInfo.IP = ClientHelper.IP;
                    UserAccountRecordBLL.Add(uarInfo);
            }
            #endregion 
            /*-----------更改产品库存订单数量---------------------------------------*/
            ProductBLL.ChangeOrderCountByOrder(orderId, ChangeAction.Plus);
            /*----------------------------------------------------------------------*/

            /*-----------删除购物车中已下单的商品-----------------------------------*/
            CartBLL.Delete(cartIds, base.UserId);
            CookiesHelper.DeleteCookie("CheckCart");
            /*----------------------------------------------------------------------*/

            ResponseHelper.Write("ok||/Finish.html?id=" + orderId);
            ResponseHelper.End();
        }
        //计算商品优惠金额
        protected void SelectProductFavor()
        {
            decimal favorMoney = 0;
            int favorId = RequestHelper.GetQueryString<int>("favorId");
            if (favorId > 0)
            {
                var theFavor = FavorableActivityBLL.Read(favorId);
                checkCart = HttpUtility.UrlDecode(CookiesHelper.ReadCookieValue("CheckCart"));
                int[] cartIds = Array.ConvertAll<string, int>(checkCart.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
                cartList = CartBLL.ReadList(base.UserId);
                cartList = cartList.Where(k => cartIds.Contains(k.Id)).ToList();
                //关联的商品
                int count = 0;
                int[] ids = cartList.Select(k => k.ProductId).ToArray();
                var productList = ProductBLL.SearchList(1, ids.Length, new ProductSearchInfo { InProductId = string.Join(",", ids) }, ref count);
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
                            tmpcart.LeftStorageCount = standardRecord.Storage - standardRecord.OrderCount;
                            tmpcart.Price = ProductBLL.GetCurrentPrice(standardRecord.SalePrice, base.GradeID);
                            tmoney += tmpcart.Price*tmpcart.BuyCount;
                        }
                        else
                        {
                            tmpcart.Price = ProductBLL.GetCurrentPrice(tmpcart.Product.SalePrice, base.GradeID);
                            tmoney += tmpcart.Price*tmpcart.BuyCount;
                        }
                    }
                }
                switch (theFavor.ReduceWay)
                {
                    case (int)FavorableMoney.Money:
                        favorMoney += theFavor.ReduceMoney;
                        break;
                    case (int)FavorableMoney.Discount:
                        favorMoney += tmoney * (100 - theFavor.ReduceDiscount) / 100;
                        break;
                    default:
                        break;
                }
                ResponseHelper.Write("ok|"+ Math.Round(favorMoney, 2));
            }           
            ResponseHelper.End();
        } 

        //读取礼品列表
        protected void ReadingGifts() {
         List<FavorableActivityGiftInfo> giftList = new List<FavorableActivityGiftInfo>();
         List<FavorableActivityGiftInfo> dataList = new List<FavorableActivityGiftInfo>();
        int favorId = RequestHelper.GetQueryString<int>("favorId");
        if (favorId > 0)
        {
            var theFavor = FavorableActivityBLL.Read(favorId);
            if (!String.IsNullOrEmpty(theFavor.GiftId))
            {
                FavorableActivityGiftSearchInfo giftSearch = new FavorableActivityGiftSearchInfo();
                giftSearch.InGiftIds = Array.ConvertAll<string, int>(theFavor.GiftId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
                giftList = FavorableActivityGiftBLL.SearchList(giftSearch);
                if (giftList.Count > 0)
                {
                    foreach (var tmp in giftList)
                    {
                        tmp.Photo = tmp.Photo.Replace("Original", "100-100");
                        dataList.Add(tmp);
                    }
                }
            }
        }
        Response.Clear();
        ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { count = giftList.Count, dataList = dataList }));
        ResponseHelper.End();
        }
    }
}