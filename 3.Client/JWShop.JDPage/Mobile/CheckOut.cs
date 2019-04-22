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

namespace JWShop.Page.Mobile
{
    public class CheckOut : CommonBasePage
    {
        protected int islpProd = 0;
        protected string checkCart;
        protected List<CartInfo> cartList = new List<CartInfo>();
        protected List<UserAddressInfo> addressList = new List<UserAddressInfo>();
        protected List<PayPluginsInfo> payPluginsList = new List<PayPluginsInfo>();
        protected SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();
        //商品优惠活动
        protected List<FavorableActivityInfo> productFavorableActivityList = new List<FavorableActivityInfo>();
        /// <summary>
        /// 用户优惠券列表
        /// </summary>
        protected List<UserCouponInfo> userCouponList = new List<UserCouponInfo>();

        //积分
        protected decimal pointLeft;

        protected decimal totalProductMoney = 0;
        /// <summary>
        /// 礼品列表
        /// </summary>
        protected List<FavorableActivityGiftInfo> giftList = new List<FavorableActivityGiftInfo>();
        /// <summary>
        /// 优惠活动
        /// </summary>
        protected FavorableActivityInfo favorableActivity = new FavorableActivityInfo();

        protected override void PageLoad()
        {
            base.PageLoad();

            //登录验证
            if (base.UserId <= 0)
            {
                string redirectUrl = "/Mobile/User/login.html?RedirectUrl=/mobile/CheckOut.html";

                ResponseHelper.Redirect(redirectUrl);
                ResponseHelper.End();
            }

            //购物车验证
            checkCart = HttpUtility.UrlDecode(CookiesHelper.ReadCookieValue("CheckCart"));
            int[] cartIds = Array.ConvertAll<string, int>(checkCart.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
            if (string.IsNullOrEmpty(checkCart) || cartIds.Length < 1)
            {
                ResponseHelper.Redirect("/Mobile/cart.html");
                ResponseHelper.End();
            }

            //cart list
            #region cart list
            //商品清单
            cartList = CartBLL.ReadList(base.UserId);
            cartList = cartList.Where(k => cartIds.Contains(k.Id)).ToList();
            if (cartList.Count < 1)
            {
                ResponseHelper.Redirect("/Mobile/cart.html");
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

                if (cart.Product.StandardType == 1)
                {
                    //使用规格的价格和库存
                    var standardRecord = ProductTypeStandardRecordBLL.Read(cart.ProductId, cart.StandardValueList);
                    cart.Price = standardRecord.SalePrice;
                    cart.LeftStorageCount = standardRecord.Storage - OrderDetailBLL.GetOrderCount(cart.ProductId, cart.StandardValueList);
                    //规格集合
                    cart.Standards = ProductTypeStandardBLL.ReadList(Array.ConvertAll<string, int>(standardRecord.StandardIdList.Split(';'), k => Convert.ToInt32(k)));
                }
                else
                {
                    cart.Price = cart.Product.SalePrice;
                    cart.LeftStorageCount = cart.Product.TotalStorageCount - OrderDetailBLL.GetOrderCount(cart.ProductId, cart.StandardValueList);
                }

                if (cart.LeftStorageCount <= 0)
                {
                    ScriptHelper.AlertFront("您购物车中 " + cart.Product.Name + " 库存不足，请重新选择", "/Mobile/Cart.html");
                }
                if (cart.Product.ClassId == "|110|")
                {
                    islpProd = 1;
                }
            }
            #endregion

            //收货地址
            addressList = UserAddressBLL.ReadList(base.UserId);
            addressList = addressList.OrderByDescending(k => k.IsDefault).ToList();
            singleUnlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();

            totalProductMoney = cartList.Sum(k => k.BuyCount * k.Price);
            //用户信息
            var user = UserBLL.ReadUserMore(base.UserId);
            //剩余积分
            pointLeft = user.PointLeft;
            #region 优惠券
            if (user.Id > 0)
            {
                //读取优惠券
                List<UserCouponInfo> tempUserCouponList = UserCouponBLL.ReadCanUse(base.UserId);
                foreach (UserCouponInfo userCoupon in tempUserCouponList)
                {
                    CouponInfo tempCoupon = CouponBLL.Read(userCoupon.CouponId);
                    if (tempCoupon.UseMinAmount <= totalProductMoney)
                    {
                        userCouponList.Add(userCoupon);
                    }
                }

                //moneyLeft = UserBLL.ReadUserMore(base.UserId).MoneyLeft;
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

            //支付方式列表
            payPluginsList = PayPlugins.ReadProductBuyPayPluginsList();

            Title = "结算中心";
        }

        /// <summary>
        /// 提交数据
        /// </summary>
        protected override void PostBack()
        {
            string url = "/Mobile/CheckOut.html";
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
            //检查支付方式
            string payKey = RequestHelper.GetForm<string>("Pay");
            if (string.IsNullOrEmpty(payKey)) 
            {
                ScriptHelper.AlertFront("请选择付款方式", url);
            }
            PayPluginsInfo payPlugins = PayPlugins.ReadPayPlugins(payKey);
            //检查金额
            decimal productMoney = 0,pointMoney=0;
            var user = UserBLL.ReadUserMore(base.UserId);
            #region 计算订单金额
            checkCart = HttpUtility.UrlDecode(CookiesHelper.ReadCookieValue("CheckCart"));
            int[] cartIds = Array.ConvertAll<string, int>(checkCart.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));

            cartList = CartBLL.ReadList(base.UserId);
            cartList = cartList.Where(k => cartIds.Contains(k.Id)).ToList();
            if (cartList.Count < 1)
            {
                ResponseHelper.Redirect("/Mobile/cart.html");
                ResponseHelper.End();
            }

            //关联的商品
            int count = 0;
            int[] ids = cartList.Select(k => k.ProductId).ToArray();
            var products = ProductBLL.SearchList(1, ids.Length, new ProductSearchInfo { InProductId = string.Join(",", ids) }, ref count);

            //规格与库存判断
            foreach (var cart in cartList)
            {
                cart.Product = products.FirstOrDefault(k => k.Id == cart.ProductId) ?? new ProductInfo();

                if (!string.IsNullOrEmpty(cart.StandardValueList))
                {
                    //使用规格的价格和库存
                    var standardRecord = ProductTypeStandardRecordBLL.Read(cart.ProductId, cart.StandardValueList);
                    int leftStorageCount = standardRecord.Storage - OrderDetailBLL.GetOrderCount(cart.ProductId, cart.StandardValueList);
                    if (leftStorageCount >= cart.BuyCount)
                    {
                        cart.Price = standardRecord.SalePrice;
                        cart.LeftStorageCount = leftStorageCount;
                        //规格集合
                        cart.Standards = ProductTypeStandardBLL.ReadList(Array.ConvertAll<string, int>(standardRecord.StandardIdList.Split(';'), k => Convert.ToInt32(k)));
                    }
                    else
                    {
                        ScriptHelper.AlertFront("您购物车中 " + cart.Product.Name + " 库存不足，请重新选择", "/Mobile/Cart.html");
                    }
                }
                else
                {
                    int leftStorageCount = cart.Product.TotalStorageCount - OrderDetailBLL.GetOrderCount(cart.ProductId, cart.StandardValueList);
                    if (leftStorageCount >= cart.BuyCount)
                    {
                        cart.Price = cart.Product.SalePrice;
                        cart.LeftStorageCount = leftStorageCount;
                    }
                    else
                    {
                        ScriptHelper.AlertFront("您购物车中 " + cart.Product.Name + " 库存不足，请重新选择", "/Mobile/Cart.html");
                    }
                }
            }
            #endregion
            productMoney = cartList.Sum(k => k.BuyCount * k.Price);

           
            decimal shippingMoney = 0;
            //订单优惠活动
            var favor = new FavorableActivityInfo { Id = RequestHelper.GetForm<int>("FavorableActivity") };
            //商品优惠
            var productfavor = new FavorableActivityInfo { Id = RequestHelper.GetForm<int>("ProductFavorableActivity") };
            #region 计算运费
            string regionID = RequestHelper.GetForm<string>("RegionID");
            //计算配送费用
            ShippingInfo shipping = ShippingBLL.Read(shippingID);
            ShippingRegionInfo shippingRegion = ShippingRegionBLL.SearchShippingRegion(shippingID, regionID);
            switch (shipping.ShippingType)
            {
                case (int)ShippingType.Fixed:
                    shippingMoney = shippingRegion.FixedMoeny;
                    break;
                case (int)ShippingType.Weight:
                    decimal cartProductWeight = Sessions.ProductTotalWeight;
                    if (cartProductWeight <= shipping.FirstWeight)
                    {
                        shippingMoney = shippingRegion.FirstMoney;
                    }
                    else
                    {
                        shippingMoney = shippingRegion.FirstMoney + Math.Ceiling((cartProductWeight - shipping.FirstWeight) / shipping.AgainWeight) * shippingRegion.AgainMoney;
                    }
                    break;
                case (int)ShippingType.ProductCount:
                    int cartProductCount = Sessions.ProductBuyCount;
                    shippingMoney = shippingRegion.OneMoeny + (cartProductCount - 1) * shippingRegion.AnotherMoeny;
                    break;
                default:
                    break;
            }
            #endregion

            //decimal balance = RequestHelper.GetForm<decimal>("Balance");
            //moneyLeft = UserBLL.ReadUserMore(base.UserId).MoneyLeft;
            //if (balance > moneyLeft)
            //{
            //    balance = 0;
            //    ScriptHelper.AlertFront("金额有错误，请重新检查", url);
            //}
            #region 如果开启了：使用积分抵现,计算积分抵现的现金金额
            //输入的兑换积分数
            var costPoint = RequestHelper.GetForm<int>("costPoint");
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
                            ScriptHelper.AlertFront("结算金额小于该优惠券要求的最低消费的金额", url);
                        }
                    }
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
                    tmpcart.Product = products.FirstOrDefault(k => k.Id == tmpcart.ProductId) ?? new ProductInfo();
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
                    if (favor.ShippingWay == (int)FavorableShipping.Free && ShippingRegionBLL.IsRegionIn(regionID, favor.RegionId))
                    {
                        favorableMoney += shippingMoney;
                    }
                }
            }
            #endregion 
            /*-----------应付总价---------------------------------------------------*/
            decimal payMoney = productMoney + shippingMoney - couponMoney - pointMoney - favorableMoney - productfavorableMoney;
            //if (productMoney - favorableMoney + shippingMoney - balance - couponMoney <= 0)
            if (payMoney<0)
            {
                ScriptHelper.AlertFront("金额有错误，请重新检查", url);
            }
           
            //添加订单
            OrderInfo order = new OrderInfo();
            order.OrderNumber = ShopCommon.CreateOrderNumber();
            order.IsActivity = (int)BoolType.False;
            if (payMoney == 0 || payPlugins.IsCod == (int)BoolType.True)
            {
                order.OrderStatus = (int)OrderStatus.WaitCheck;
            }
            else
            {
                order.OrderStatus = (int)OrderStatus.WaitPay;
            }
            order.OrderNote = string.Empty;
            order.ProductMoney = productMoney;
            order.Balance = 0;
            order.FavorableMoney = favorableMoney + productfavorableMoney;
            order.OtherMoney = 0;
            order.CouponMoney = couponMoney;
            order.Point = costPoint;
            order.PointMoney = pointMoney;
            order.Consignee = consignee;
            SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();
            order.RegionId = singleUnlimitClass.ClassID;
            order.Address = address;
            order.ZipCode = zipCode;
            order.Tel = tel;
            if (base.UserId == 0)
            {
                order.Email = StringHelper.AddSafe(RequestHelper.GetForm<string>("Email"));
            }
            else
            {
                order.Email = CookiesHelper.ReadCookieValue("UserEmail");
            }
            order.Mobile = mobile;
            order.ShippingId = shippingID;
            order.ShippingDate = RequestHelper.DateNow;
            order.ShippingNumber = string.Empty;
            order.ShippingMoney = shippingMoney;
            order.PayKey = payKey;
            order.PayName = payPlugins.Name;
            order.PayDate = RequestHelper.DateNow; ;
            order.IsRefund = (int)BoolType.False;
            order.FavorableActivityId = RequestHelper.GetForm<int>("FavorableActivityID");
            order.GiftId = RequestHelper.GetForm<int>("GiftID");
            order.InvoiceTitle = StringHelper.AddSafe(RequestHelper.GetForm<string>("InvoiceTitle"));
            order.InvoiceContent = StringHelper.AddSafe(RequestHelper.GetForm<string>("InvoiceContent"));
            order.UserMessage = StringHelper.AddSafe(RequestHelper.GetForm<string>("UserMessage"));
            order.AddDate = RequestHelper.DateNow;
            order.IP = ClientHelper.IP;
            order.UserId = base.UserId;
            order.UserName = base.UserName;
            order.GiftMessige = RequestHelper.GetForm<string>("GiftMessige");
            order.IsNoticed = 0;

            int orderID = OrderBLL.Add(order);
            //使用余额
            /*if (balance > 0)
            {
                UserAccountRecordInfo userAccountRecord = new UserAccountRecordInfo();
                userAccountRecord.Money = -balance;
                userAccountRecord.Point = 0;
                userAccountRecord.Date = RequestHelper.DateNow;
                userAccountRecord.IP = ClientHelper.IP;
                userAccountRecord.Note = "支付订单：";
                userAccountRecord.UserId = base.UserId;
                userAccountRecord.UserName = base.UserName;
                UserAccountRecordBLL.Add(userAccountRecord);
            }*/
            #region 减少积分
            if (ShopConfig.ReadConfigInfo().EnablePointPay == 1 && costPoint > 0)
            {
                //减少积分
                UserAccountRecordInfo uarInfo = new UserAccountRecordInfo();
                uarInfo.RecordType = (int)AccountRecordType.Point;
                uarInfo.UserId = base.UserId;
                uarInfo.UserName = base.UserName;
                uarInfo.Note = "支付订单：" + order.OrderNumber;
                uarInfo.Point = -costPoint;
                uarInfo.Money = 0;
                uarInfo.Date = DateTime.Now;
                uarInfo.IP = ClientHelper.IP;
                UserAccountRecordBLL.Add(uarInfo);
            }
            #endregion 
            #region 使用优惠券
            string strUserCoupon = RequestHelper.GetForm<string>("UserCoupon");
            if (couponMoney > 0 && !string.IsNullOrEmpty(strUserCoupon) && strUserCoupon != "0|0")
            {
                userCoupon.IsUse = (int)BoolType.True;
                userCoupon.OrderId = orderID;
                UserCouponBLL.Update(userCoupon);
            }
            #endregion
            AddOrderProduct(orderID);
            //更改产品库存订单数量
            ProductBLL.ChangeOrderCountByOrder(orderID, ChangeAction.Plus);
            /*----------------------------------------------------------------------*/
         
            ResponseHelper.Redirect("/Mobile/Finish-I" + orderID.ToString() + ".html");
        }
        /// <summary>
        /// 添加订单产品
        /// </summary>
        /// <param name="orderID"></param>
        protected void AddOrderProduct(int orderID)
        {
            List<CartInfo> cartList = CartBLL.ReadList(base.UserId);
            //读取产品
            checkCart = HttpUtility.UrlDecode(CookiesHelper.ReadCookieValue("CheckCart"));
            int[] cartIds = Array.ConvertAll<string, int>(checkCart.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));

            cartList = CartBLL.ReadList(base.UserId);
            cartList = cartList.Where(k => cartIds.Contains(k.Id)).ToList();

            string strProductID = string.Empty;
            foreach (CartInfo cart in cartList)
            {
                if (strProductID == string.Empty)
                {
                    strProductID = cart.ProductId.ToString();
                }
                else
                {
                    strProductID += "," + cart.ProductId.ToString();
                }
            }
            List<ProductInfo> productList = new List<ProductInfo>();
            if (strProductID != string.Empty)
            {
                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.InProductId = strProductID;
                productList = ProductBLL.SearchList(productSearch);
            }
            //会员价格
            //List<MemberPriceInfo> memberPriceList = MemberPriceBLL.ReadMemberPriceByProductGrade(strProductID, base.GradeID);
            //添加订单产品
            Dictionary<string, bool> cartDic = new Dictionary<string, bool>();
            Dictionary<int, int> cartOrderDetailDic = new Dictionary<int, int>();
            foreach (CartInfo cart in cartList)
            {
                ProductInfo product = ProductBLL.ReadProductByProductList(productList, cart.ProductId);
                OrderDetailInfo orderDetail = new OrderDetailInfo();
                orderDetail.OrderId = orderID;
                orderDetail.ProductId = cart.ProductId;
                orderDetail.ProductName = cart.ProductName;
                orderDetail.StandardValueList = cart.StandardValueList;
                orderDetail.ProductWeight = product.Weight;
                orderDetail.SendPoint = product.SendPoint;

                if (!string.IsNullOrEmpty(cart.StandardValueList))
                {
                    var standardRecord = ProductTypeStandardRecordBLL.Read(cart.ProductId, cart.StandardValueList);
                    orderDetail.ProductPrice = ProductBLL.GetCurrentPrice(standardRecord.SalePrice, base.GradeID);
                }
                else
                {
                    orderDetail.ProductPrice = ProductBLL.GetCurrentPrice(cart.Product.SalePrice, base.GradeID);
                }

                orderDetail.BuyCount = cart.BuyCount;

                orderDetail.RandNumber = cart.RandNumber;
                int orderDetailID = OrderDetailBLL.Add(orderDetail);
                cartOrderDetailDic.Add(cart.Id, orderDetailID);
            }           
            /*-----------删除购物车中已下单的商品-----------------------------------*/
            CartBLL.Delete(cartIds, base.UserId);
            CookiesHelper.DeleteCookie("CheckCart");
            /*----------------------------------------------------------------------*/
           
        }

    }
}