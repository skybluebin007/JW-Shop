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

namespace JWShop.Page
{
    public class CheckOut : CommonBasePage
    {
        protected string checkCart;
        protected List<CartInfo> cartList = new List<CartInfo>();
        protected List<UserAddressInfo> addressList = new List<UserAddressInfo>();
        protected List<PayPluginsInfo> payPluginsList = new List<PayPluginsInfo>();
        protected SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();

        /// <summary>
        /// 用户优惠券列表
        /// </summary>
        protected List<UserCouponInfo> userCouponList = new List<UserCouponInfo>();

        //积分、账户余额信息
        protected decimal pointLeft, pointCanUse, moneyLeft, moneyCanUse;

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
                string redirectUrl = string.IsNullOrEmpty(isMobile)
                    ? "/user/login.html?RedirectUrl=/checkout.html"
                    : isMobile + "/login.html?RedirectUrl=/mobile/CheckOut.html";

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

                if (cart.Product.StandardType==1)
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
                    ScriptHelper.AlertFront("您购物车中 " + cart.Product.Name + " 库存不足，请重新选择", "/Cart.html");
                }
            }
            #endregion

            //收货地址
            addressList = UserAddressBLL.ReadList(base.UserId);
            addressList = addressList.OrderByDescending(k => k.IsDefault).ToList();
            singleUnlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();

            totalProductMoney = cartList.Sum(k => k.BuyCount * k.Price);
            //用户信息
            var user = UserBLL.Read(base.UserId);
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

                moneyLeft = UserBLL.ReadUserMore(base.UserId).MoneyLeft;
            }
            //读取优惠活动
            favorableActivity = FavorableActivityBLL.Read(DateTime.Now, DateTime.Now, 0);
            if (favorableActivity.Id > 0)
            {
                if (("," + favorableActivity.UserGrade + ",").IndexOf("," + base.GradeID.ToString() + ",") > -1 && Sessions.ProductTotalPrice >= favorableActivity.OrderProductMoney)
                {
                    if (favorableActivity.GiftId != string.Empty)
                    {
                        FavorableActivityGiftSearchInfo giftSearch = new FavorableActivityGiftSearchInfo();
                        giftSearch.InGiftIds = Array.ConvertAll<string, int>(favorableActivity.GiftId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
                        giftList = FavorableActivityGiftBLL.SearchList(giftSearch);
                    }
                }
                else
                {
                    favorableActivity = new FavorableActivityInfo();
                }
            }
            //支付方式列表
            payPluginsList = PayPlugins.ReadProductBuyPayPluginsList();

            Title = "结算中心";
        }

        /// <summary>
        /// 提交数据
        /// </summary>
        protected override void PostBack()
        {
            string url = "/CheckOut.html";
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
            //if (zipCode == string.Empty)
            //{
            //    ScriptHelper.AlertFront("邮编不能为空", url);
            //}
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
            decimal productMoney=0;
            #region 计算订单金额
            checkCart = HttpUtility.UrlDecode(CookiesHelper.ReadCookieValue("CheckCart"));
            int[] cartIds = Array.ConvertAll<string, int>(checkCart.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
            
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
                        ScriptHelper.AlertFront("您购物车中 "+cart.Product.Name+" 库存不足，请重新选择", "/Cart.html");
                    }
                }
                else
                {
                    int leftStorageCount = cart.Product.TotalStorageCount - OrderDetailBLL.GetOrderCount(cart.ProductId, cart.StandardValueList);
                    if (leftStorageCount >= cart.BuyCount)
                    {
                        cart.Price = cart.Product.SalePrice;
                        cart.LeftStorageCount = cart.Product.TotalStorageCount - cart.Product.OrderCount;
                    }
                    else
                    {
                        ScriptHelper.AlertFront("您购物车中 " + cart.Product.Name + " 库存不足，请重新选择", "/Cart.html");
                    }
                }
            }
            #endregion
            productMoney = cartList.Sum(k => k.BuyCount * k.Price);

            decimal favorableMoney = 0;
            decimal shippingMoney = 0;
            #region 计算运费与优惠金额
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
            //计算优惠费用
            FavorableActivityInfo favorableActivity = FavorableActivityBLL.Read(DateTime.Now, DateTime.Now, 0);
            if (favorableActivity.Id > 0)
            {
                if (("," + favorableActivity.UserGrade + ",").IndexOf("," + base.GradeID.ToString() + ",") > -1 && Sessions.ProductTotalPrice >= favorableActivity.OrderProductMoney)
                {
                    switch (favorableActivity.ReduceWay)
                    {
                        case (int)FavorableMoney.Money:
                            favorableMoney += favorableActivity.ReduceMoney;
                            break;
                        case (int)FavorableMoney.Discount:
                            favorableMoney += Sessions.ProductTotalPrice * (10 - favorableActivity.ReduceDiscount) / 10;
                            break;
                        default:
                            break;
                    }
                    if (favorableActivity.ShippingWay == (int)FavorableShipping.Free && ShippingRegionBLL.IsRegionIn(regionID, favorableActivity.RegionId))
                    {
                        favorableMoney += shippingMoney;
                    }
                }
            }
            #endregion

            decimal balance = RequestHelper.GetForm<decimal>("Balance");
            moneyLeft = UserBLL.ReadUserMore(base.UserId).MoneyLeft;
            if (balance > moneyLeft)
            {
                balance = 0;
                ScriptHelper.AlertFront("金额有错误，请重新检查", url);
            }


            decimal couponMoney = 0;
            string userCouponStr= RequestHelper.GetForm<string>("UserCoupon");
            UserCouponInfo userCoupon = new UserCouponInfo();
            if (userCouponStr != string.Empty)
            {
                int couponID=0;
                if(int.TryParse(userCouponStr.Split(new char[]{'|'},StringSplitOptions.RemoveEmptyEntries)[0],out couponID))
                {
                    userCoupon = UserCouponBLL.Read(couponID, base.UserId);
                    if (userCoupon.UserId == base.UserId && userCoupon.IsUse == 0)
                    {
                        couponMoney = CouponBLL.Read(userCoupon.CouponId).Money;                        
                    }
                }
            }
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
            order.IsActivity = (int)BoolType.False;
            if (productMoney - favorableMoney + shippingMoney - balance - couponMoney == 0 || payPlugins.IsCod == (int)BoolType.True)
            {
                order.OrderStatus = (int)OrderStatus.WaitCheck;
            }
            else
            {
                order.OrderStatus = (int)OrderStatus.WaitPay;
            }
            order.OrderNote = string.Empty;
            order.ProductMoney = productMoney;
            order.Balance = balance;
            order.FavorableMoney = favorableMoney;
            order.OtherMoney = 0;
            order.CouponMoney = couponMoney;
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
            int orderID = OrderBLL.Add(order);
            //使用余额
            if (balance > 0)
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
            }
            //使用优惠券
            string strUserCoupon = RequestHelper.GetForm<string>("UserCoupon");
            if (couponMoney > 0 && strUserCoupon != "0|0")
            {                
                userCoupon.IsUse = (int)BoolType.True;
                userCoupon.OrderId = orderID;
                UserCouponBLL.Update(userCoupon);
            }
            AddOrderProduct(orderID);
            //更改产品库存订单数量
            ProductBLL.ChangeOrderCountByOrder(orderID, ChangeAction.Plus);
            ResponseHelper.Redirect("/Finish-I" + orderID.ToString() + ".html");
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
                orderDetail.ProductWeight = product.Weight;
                orderDetail.SendPoint = product.SendPoint;

                orderDetail.ProductPrice = ProductBLL.GetCurrentPriceWithStandard(product.Id, base.GradeID, cart.StandardValueList);
                
                orderDetail.BuyCount = cart.BuyCount;
                
                orderDetail.RandNumber = cart.RandNumber;
                int orderDetailID = OrderDetailBLL.Add(orderDetail);
                cartOrderDetailDic.Add(cart.Id, orderDetailID);
            }

            CartBLL.Delete(cartIds, base.UserId);
            //CartBLL.Clear(base.UserId);            
        }

    }
}