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

namespace JWShop.Page
{
    public class Ajax : AjaxBasePage
    {
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "CheckUserName":
                    CheckUserName();
                    break;
                case "CheckEmail":
                    CheckEmail();
                    break;
                case "MessageSubmit":
                    //MessageSubmit(RequestHelper.GetQueryString<string>("type"));
                    break;
                case "GetVerifyCode":
                    GetVerifyCode();
                    break;
                case"GetProductPriceAndStore":
                    GetProductPriceAndStore();
                    break;
                case"CheckStore":
                    CheckStore();
                    break;
                case "Collect":
                    Collect();
                    break;                
                case "AddToCart":
                    AddToCart();
                    break;               
                case "SelectShipping":
                    SelectShipping();
                    break;
                case "CheckUserCoupon":
                    CheckUserCoupon();
                    break;
                case "checkOrderPay"://检查订单是否已支付
                    CheckOrderPay();
                    break;
                case "AddProductComment":
                    PostProductComment();
                    break;
                case "DeleteOrder":
                    DeleteOrder();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 获取短信验证码
        /// </summary>
        private void GetVerifyCode()
        {
            string mobile = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("mobile"));
            bool isSuccess = false;
            string msg = "";

            if (string.IsNullOrEmpty(mobile))
            {
                ResponseHelper.Write("error|请输入手机号码");
                ResponseHelper.End();
            }

            isSuccess = false;// WebService.GetHttp.PostSms(mobile, out msg);
            if (isSuccess)
            {
                CookiesHelper.AddCookie("verify_send", DateTime.Now.ToString(), 1, TimeType.Minute);
                ResponseHelper.Write("ok|");
                ResponseHelper.End();
            }
            else
            {
                ResponseHelper.Write("error|" + msg);
                ResponseHelper.End();
            }
        }

        private void GetProductPriceAndStore()
        {
            string result = "no";
            int id = RequestHelper.GetQueryString<int>("productID");
            string valueList=StringHelper.AddSafe(RequestHelper.GetQueryString<string>("valueList"));
            List<ProductTypeStandardRecordInfo> standRecordList = ProductTypeStandardRecordBLL.ReadListByProduct(id, 1);
            foreach (var item in standRecordList)
            {
                if (item.ValueList.Trim() == valueList.Trim())
                {
                    result = item.SalePrice + "|" + item.MarketPrice + "|" + item.Storage + "|" + item.Photo;
                    break;
                }
            }

            Response.Write(result);
        }

        private void CheckStore()
        {
            string result = "0";
            int id = RequestHelper.GetQueryString<int>("productID");
            int buyCount = RequestHelper.GetQueryString<int>("buyCount");
            string valueList = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("valueList"));
            int standardType = RequestHelper.GetQueryString<int>("standardType");
            if (standardType == 1)
            {
                List<ProductTypeStandardRecordInfo> standRecordList = ProductTypeStandardRecordBLL.ReadListByProduct(id, 1);
                foreach (var item in standRecordList)
                {
                    if (item.ValueList.Trim() == valueList.Trim())
                    {
                        if (item.Storage >= buyCount)
                        {
                            result = "1";
                        }
                        break;
                    }
                }
            }
            else
            {
                ProductInfo product = ProductBLL.Read(id);
                if (product.TotalStorageCount - product.OrderCount >= buyCount)
                {
                    result = "1";
                }
            }
            Response.Write(result);
        }
        /// <summary>
        /// 检查用户名
        /// </summary>
        protected void CheckUserName()
        {
            int result = 1;
            string userName = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("UserName"));
            if (userName != string.Empty)
            {
                //检查非法字符
                string forbiddinName = ShopConfig.ReadConfigInfo().ForbiddenName;
                if (forbiddinName != string.Empty)
                {
                    foreach (string TempName in forbiddinName.Split('|'))
                    {
                        if (userName.IndexOf(TempName.Trim()) != -1)
                        {
                            result = 3;
                            break;
                        }
                    }
                }
                //检查用户名
                if (result != 3)
                {
                    if (!UserBLL.UniqueUser(userName))
                    {
                        result = 2;
                    }
                }
            }
            ResponseHelper.Write(result.ToString());
            ResponseHelper.End();
        }
        /// <summary>
        /// 检查E-mail
        /// </summary>
        protected void CheckEmail()
        {
            int result = 1;
            string email = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("Email"));
            if (email != string.Empty)
            {
                if (UserBLL.CheckEmail(email))
                {
                    result = 2;
                }
            }
            ResponseHelper.Write(result.ToString());
            ResponseHelper.End();
        }
        /// <summary>
        /// 收藏产品
        /// </summary>
        public void Collect()
        {
            string result = string.Empty;
            int productID = RequestHelper.GetQueryString<int>("ProductID");
            if (productID > 0)
            {
                if (base.UserId == 0)
                {
                    result = "还未登录";
                }
                else
                {
                    if (ProductCollectBLL.Read(productID, base.UserId).Id > 0)
                    {
                        result = "您已经收藏了该产品";
                    }
                    else
                    {
                        ProductCollectInfo productCollect = new ProductCollectInfo();
                        productCollect.ProductId = productID;
                        productCollect.Tm = RequestHelper.DateNow;
                        productCollect.UserId = base.UserId;
                        ProductCollectBLL.Add(productCollect);
                        result = "成功收藏";
                    }
                }
            }
            else
            {
                result = "请选择产品";
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }

        /// <summary>
        /// 添加商品到购物车
        /// </summary>
        protected void AddToCart()
        {
            string result = "ok";
            int productID = RequestHelper.GetQueryString<int>("ProductID");
            string productName = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("ProductName"));
            string standardValueList = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("StandardValueList"));
            int buyCount = RequestHelper.GetQueryString<int>("BuyCount");
            decimal currentMemberPrice = RequestHelper.GetQueryString<decimal>("CurrentMemberPrice");
            if (!CartBLL.IsProductInCart(productID, productName, base.UserId))
            {
                CartInfo cart = new CartInfo();
                cart.ProductId = productID;
                cart.ProductName = productName;
                cart.BuyCount = buyCount;
                cart.StandardValueList = standardValueList;
                cart.RandNumber = string.Empty;
                cart.UserId = base.UserId;
                cart.UserName = base.UserName;
                int cartID = CartBLL.Add(cart, base.UserId);
                Sessions.ProductBuyCount += buyCount;
                //Sessions.ProductTotalPrice += buyCount * currentMemberPrice;
                //添加赠品,赠品另外下单(2016.3.16)
                //ProductInfo product = ProductBLL.Read(productID);
                //if (product.Accessory != string.Empty)
                //{
                //    ProductSearchInfo productSearch = new ProductSearchInfo();
                //    productSearch.InProductId = product.Accessory;
                //    List<ProductInfo> accessoryList = ProductBLL.SearchList(productSearch);
                //    foreach (ProductInfo accessory in accessoryList)
                //    {
                //        cart = new CartInfo();
                //        cart.ProductId = accessory.Id;
                //        cart.ProductName = accessory.Name;
                //        cart.BuyCount = buyCount;
                //        cart.RandNumber = string.Empty;
                //        cart.UserId = base.UserId;
                //        cart.UserName = base.UserName;
                //        CartBLL.Add(cart, base.UserId);
                //    }
                //}
            }
            else
            {
                result = "该产品已经在购物车";
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
        /// <summary>
        /// 选择配送方式
        /// </summary>
        protected void SelectShipping()
        {
            int shippingID = RequestHelper.GetQueryString<int>("ShippingID");
            string regionID = RequestHelper.GetQueryString<string>("RegionID");
            //计算配送费用
            decimal shippingMoney = 0;
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
            decimal favorableMoney = 0;
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
            ResponseHelper.Write(shippingMoney.ToString() + "|" + favorableMoney.ToString());
            ResponseHelper.End();
        }
        /// <summary>
        /// 检查用户优惠券
        /// </summary>
        protected void CheckUserCoupon()
        {
            string result = string.Empty;
            string number = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("Number"));
            string password = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("Password"));
            if (number != string.Empty && password != string.Empty)
            {
                UserCouponInfo userCoupon = UserCouponBLL.Read(number, password);
                if (userCoupon.Id > 0)
                {

                    if (userCoupon.UserId == 0)
                    {
                        if (userCoupon.IsUse == (int)BoolType.False)
                        {
                            CouponInfo coupon = CouponBLL.Read(userCoupon.CouponId);
                            if (RequestHelper.DateNow >= coupon.UseStartDate && RequestHelper.DateNow <= coupon.UseEndDate)
                            {
                                if (Sessions.ProductTotalPrice >= coupon.UseMinAmount)
                                {
                                    result = userCoupon.Id.ToString() + "|" + coupon.Money.ToString();
                                    if (base.UserId > 0)
                                    {
                                        userCoupon.UserId = base.UserId;
                                        userCoupon.UserName = base.UserName;
                                        UserCouponBLL.Update(userCoupon);
                                    }
                                }
                                else
                                {
                                    result = "购物车的金额小于该优惠券要求的最低消费的金额";
                                }
                            }
                            else
                            {
                                result = "该优惠券没在使用期限内";
                            }
                        }
                        else
                        {
                            result = "该优惠券已经使用了";
                        }
                    }
                    else
                    {
                        result = "该优惠券已经绑定了用户";
                    }
                }
                else
                {
                    result = "不存在该优惠券";
                }
            }
            else
            {
                result = "编号或者密码不能为空";
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }

        /// <summary>
        /// 检查订单是否支付
        /// </summary>
        protected void CheckOrderPay()
        {
            string result = string.Empty;
            int orderId = RequestHelper.GetQueryString<int>("OrderID");
            OrderInfo Order = OrderBLL.Read(orderId);
            if (Order.OrderStatus == 2)
            {
                result = "ok";
            }
            else
            {
                result = "no";
            }
            Response.Write(result);
            Response.End();
        }

        /// <summary>
        /// 提交评论
        /// </summary>
        public void PostProductComment()
        {
            string result = "ok";
            if (ShopConfig.ReadConfigInfo().AllowAnonymousComment == (int)BoolType.False && base.UserId == 0)
            {
                result = "还未登录";
            }
            else
            {
                AddProductComment(ref result);
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="result"></param>
        public void AddProductComment(ref string result)
        {
            int productID = RequestHelper.GetQueryString<int>("ProductID");
            int orderID = RequestHelper.GetQueryString<int>("OrderID");
            string commentCookies = CookiesHelper.ReadCookieValue("CommentCookies" + productID.ToString());
            if (ShopConfig.ReadConfigInfo().CommentRestrictTime > 0 && commentCookies != string.Empty)
            {
                result = "请不要频繁提交";
            }
            else
            {
                ProductCommentInfo productComment = new ProductCommentInfo();
                productComment.ProductId = productID;
                productComment.Title = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("Title"));
                productComment.Content = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("Content"));
                productComment.UserIP = ClientHelper.IP;
                productComment.PostDate = RequestHelper.DateNow;
                productComment.Support = 0;
                productComment.Against = 0;
                productComment.Status = ShopConfig.ReadConfigInfo().CommentDefaultStatus;
                productComment.Rank = RequestHelper.GetQueryString<int>("Rank");
                productComment.ReplyCount = 0;
                productComment.AdminReplyContent = string.Empty;
                productComment.AdminReplyDate = RequestHelper.DateNow;
                productComment.UserId = base.UserId;
                productComment.UserName = base.UserName;
                productComment.OrderId = orderID;
                ProductCommentBLL.Add(productComment);
                if (ShopConfig.ReadConfigInfo().CommentRestrictTime > 0)
                {
                    CookiesHelper.AddCookie("CommentCookies" + productID.ToString(), "CommentCookies" + productID.ToString(), ShopConfig.ReadConfigInfo().CommentRestrictTime, TimeType.Second);
                }
            }
        }

        /// <summary>
        /// 隐藏订单
        /// </summary>
        private void DeleteOrder()
        {
            string result = "ok";

            int orderID = RequestHelper.GetQueryString<int>("OrderID");
            int userID = RequestHelper.GetQueryString<int>("UserID");
            OrderInfo order = OrderBLL.Read(orderID);
            order.IsDelete = 1;//隐藏订单
            OrderBLL.Update(order);

            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
    }
}