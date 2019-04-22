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
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Linq;
using System.Text.RegularExpressions;
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
                case "CheckEmail2":
                    CheckEmail2();
                    break;
                case "CheckMobile":
                    CheckMobile();
                    break;
                case "CheckUserNameAndMobile":
                    CheckUserNameAndMobile();
                    break;
                case"CheckLoginNameAndMobile":
                    CheckLoginNameAndMobile();
                    break;               
                case "GetVerifyCode":
                    GetVerifyCode();
                    break;
                case "GetProductPriceAndStore":
                    GetProductPriceAndStore();
                    break;
                case "CheckStore":
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
                case "GetHonorListMobile":
                    GetHonorListMobile();
                    break;
                case "GetArticleListMobile":
                    GetArticleListMobile();
                    break;
                case "DeleteHistorySearch":
                    DeleteHistorySearch();
                    break;
                case "ReadCart":
                    ReadCart();
                    break;
                case "ReadProductFavorList":
                    ReadProductFavorList();
                    break;
                case "GetHotKeys":
                    GetHotKeys();
                    break;
                case"GetProductsByFloor":
                    GetProductsByFloor();
                    break;
                case "VerifyLogin":
                    this.VerifyLogin();
                    break;
                case "CatchUserCoupon":
                    this.CatchUserCoupon();
                    break;
                case "GetVoteItemListMobile":
                    GetVoteItemListMobile();
                    break;
                case "GetVoteItemList":
                    GetVoteItemList();
                    break;
                case "GetVoteItemsFirst":
                    GetVoteItemsFirst();
                    break;
                case "GetVoteItemsFirstMobile":
                    GetVoteItemsFirstMobile();
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


            if (string.IsNullOrEmpty(mobile))
            {
                ResponseHelper.Write("error|请输入手机号码");
                ResponseHelper.End();
            }

            if (!ShopCommon.CheckMobile(mobile))
            {
                ResponseHelper.Write("error|手机号码错误");
                ResponseHelper.End();
            }
         
            if (CookiesHelper.ReadCookie("MobileCode" + StringHelper.AddSafe(mobile)) == null)
            {
                SendSms(mobile);
            }
            else 
            {
                ResponseHelper.Write("error|验证码已发送,请及时验证");
                ResponseHelper.End();
            }

        }


        /// <summary>
        /// 发送手机验证码 
        /// </summary>
        protected void SendSms(string mobile)
        {
            #region 网易云信短信接口
           /* string appKey = "69ebcae92e7a6ed7ca440cef0261ebed";
            string appSecret = "1ec83e586188";
            Random rd = new Random();
            string nonce = rd.Next(10001, 99999).ToString();
            string curTime = GenerateTimeStamp(DateTime.Now);
            string checkSum = SHA1_Encrypt(appSecret + nonce + curTime);
            string checkCode = rd.Next(10001, 99999).ToString();
            //参数
            string param = "templateid=14038&mobiles=['" + mobile + "']&params=['" + checkCode + "']";
        
            byte[] bs = Encoding.UTF8.GetBytes(param);    //参数转化为ascii码

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("https://api.netease.im/sms/sendtemplate.action");  //创建request
            req.Headers.Add("AppKey", appKey);
            req.Headers.Add("CurTime", curTime);
            req.Headers.Add("CheckSum", checkSum);
            req.Headers.Add("Nonce", nonce);
            req.Method = "POST";    //确定传值的方式，此处为post方式传值

            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            req.ContentLength = bs.Length;

            using (Stream reqStream = req.GetRequestStream())
            {

                reqStream.Write(bs, 0, bs.Length);


            }*/
            #endregion

            #region 竞网短信接口
            // http 请求方式: GET 
            string apikey = "22ecc85cbb9158d0c36037b4b6a68b24";
            string mark = "reg_code";
            Random rd = new Random();
            string checkCode = rd.Next(10001, 99999).ToString();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://app.hnjing.cn:9002/api/sms/" + apikey + "/send?phone=" + mobile + "&mark=" + mark + "&p=" + checkCode);

            req.Method = "GET";
            req.ContentType = "text/html;charset=utf-8";
            #endregion

            using (HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse())
            {
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string responseString = sr.ReadToEnd();

                #region 网易云信短信接口
                /*
                var json = JsonConvert.DeserializeObject<ResponseResult>(responseString);
if (json.Code == "200")
                {
                    CookiesHelper.AddCookie("MobileCode" + StringHelper.AddSafe(mobile), StringHelper.AddSafe(checkCode), 5, TimeType.Minute);
                    Response.Write("ok");

                }
                else
                {
                    Response.Write("error");
                }*/
                #endregion

                #region 竞网短信接口
                var json = JsonConvert.DeserializeObject<ResponseResultHnjing>(responseString);
                if (json.ok == "True")
                {
                    CookiesHelper.AddCookie("MobileCode" + StringHelper.AddSafe(mobile), StringHelper.AddSafe(checkCode), 5, TimeType.Minute);
                    Response.Write("ok|发送成功");

                }
                else
                {
                   // Response.Write(json.message);错误消息
                    Response.Write("error|" + json.message);
                }
                #endregion
            }
            Response.End();
        }


        /// <summary>
        /// 对字符串进行SHA1加密
        /// </summary>
        /// <param name="strIN">需要加密的字符串</param>
        /// <returns>密文[小写]</returns>
        public string SHA1_Encrypt(string Source_String)
        {

            byte[] StrRes = Encoding.Default.GetBytes(Source_String);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString().ToLower();
        }
        /// <summary>
        /// 系统时间转换成UTC时间戳 秒数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GenerateTimeStamp(DateTime dt)
        {
            // Default implementation of UNIX time of the current UTC time
            TimeSpan ts = dt.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        /// <summary>
        /// 网易云信返回json
        /// </summary>
        public class ResponseResult
        {
            public string Code { get; set; }
            public string Msg { get; set; }
            public string Obj { get; set; }
        }
        /// <summary>
        /// 竞网短信接口返回json
        /// </summary>
        public class ResponseResultHnjing
        {
            public string ok { get; set; }
            public string data { get; set; }
            public string message { get; set; }
        }


        private void GetProductPriceAndStore()
        {
            string result = "no";
            int id = RequestHelper.GetQueryString<int>("productID");
            string valueList = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("valueList"));
            List<ProductTypeStandardRecordInfo> standRecordList = ProductTypeStandardRecordBLL.ReadListByProduct(id, ProductBLL.Read(id).StandardType);
            foreach (var item in standRecordList)
            {
                if (item.ValueList.Trim() == valueList.Trim())
                {
                    result = ProductBLL.GetCurrentPrice(item.SalePrice, base.GradeID) + "|" + ProductBLL.GetCurrentPrice(item.MarketPrice, base.GradeID) + "|" + (item.Storage - item.OrderCount) + "|" + item.Photo;
                    break;
                }
            }

            Response.Write(result);
        }
        /// <summary>
        /// 检查库存
        /// </summary>
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
                        if (item.Storage - item.OrderCount >= buyCount)
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
        /// 检查用户名与手机号是否匹配
        /// </summary>
        protected void CheckUserNameAndMobile() {
            int result = 1;
            string userName = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("UserName"));
            string Mobile = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("Mobile"));
            if (userName != string.Empty)
            {
                //检查用户名与Mobile是否匹配
                if (!UserBLL.CheckMobile(Mobile, 0))
                {
                    if (UserBLL.Read(userName).Mobile==Mobile)
                    {
                        result = 2;
                    }
                }
            }
            ResponseHelper.Write(result.ToString());
            ResponseHelper.End();
        }
        /// <summary>
        /// 检查账户名与手机号是否匹配
        /// </summary>
        protected void CheckLoginNameAndMobile()
        {
            int result = 1;
            string loginName = StringHelper.AddSafe(Server.UrlDecode(RequestHelper.GetQueryString<string>("UserName")));
            string Mobile = StringHelper.AddSafe(Server.UrlDecode(RequestHelper.GetQueryString<string>("Mobile")));
            if (!string.IsNullOrEmpty(loginName))
            {
                //检查账户名名与Mobile是否匹配              
                    if (UserBLL.Read(loginName).Mobile == Mobile)
                    {
                        result = 2;
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
        /// 检查E-mail是否被其他用户占用
        /// </summary>
        protected void CheckEmail2()
        {
            int result = 1;
            string email = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("Email"));
            if (email != string.Empty)
            {
                if (UserBLL.CheckEmail(email, base.UserId))
                {
                    result = 2;
                }
            }
            ResponseHelper.Write(result.ToString());
            ResponseHelper.End();
        }
        /// <summary>
        /// 检查mobile是否占用
        /// </summary>
        protected void CheckMobile()
        {
            int result = 1;
            string mobile = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("Mobile"));
            int checkUserId = RequestHelper.GetQueryString<int>("checkUserId");
            if (mobile != string.Empty)
            {
                if (checkUserId > 0)
                {//修改时排除自己
                    if (UserBLL.CheckMobile(mobile, base.UserId))
                    {
                        result = 2;
                    }
                }
                else
                {//注册时检查所有 
                    if (UserBLL.CheckMobile(mobile, 0))
                    {
                        result = 2;
                    }
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
                        result = "您已经关注了该产品";
                    }
                    else
                    {
                        ProductCollectInfo productCollect = new ProductCollectInfo();
                        productCollect.ProductId = productID;
                        productCollect.Tm = RequestHelper.DateNow;
                        productCollect.UserId = base.UserId;
                        ProductCollectBLL.Add(productCollect);
                        result = "成功关注";
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
                CartInfo theCart = CartBLL.Read(productID, productName, base.UserId);
                if (theCart.Id > 0)
                {
                    theCart.BuyCount +=1;
                    CartBLL.Update(Array.ConvertAll<string, int>(theCart.Id.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k)), theCart.BuyCount, base.UserId);
                    Sessions.ProductBuyCount += buyCount;
                }
                else
                {
                    result = "该产品已经在购物车";
                }
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
            decimal totalProductMoney = RequestHelper.GetQueryString<decimal>("totalProductMoney");
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
                                if (totalProductMoney >= coupon.UseMinAmount)
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
                                    result = "结算金额小于该优惠券要求的最低消费的金额";
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
                result = "优惠券编号或者密码不能为空";
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
            if (Order.OrderStatus >= (int)OrderStatus.WaitCheck && (Order.PayKey.ToLower() == "alipay" || Order.PayKey.ToLower() == "wxpay"))
            {
                result = "ok";
            }
            else
            {
                result = "no";
            }
            Response.Clear();
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
            if (base.UserId > 0)
            {
                if (ShopConfig.ReadConfigInfo().CommentRestrictTime > 0 && commentCookies != string.Empty)
                {
                    result = "请不要频繁提交";
                }
                else
                {
                    var procomList = ProductCommentBLL.SearchProductCommentList(new ProductCommentSearchInfo() { OrderID = orderID, ProductId = productID, UserId = base.UserId });
                    if (procomList.Count > 0) return;
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
        }

        /// <summary>
        /// 删除订单：付款之后的逻辑删除，待付款的彻底删除
        /// </summary>
        private void DeleteOrder()
        {
            string result = "ok";

            int orderID = RequestHelper.GetQueryString<int>("OrderID");
            int userID = RequestHelper.GetQueryString<int>("UserID");
            if (orderID > 0 && userID > 0 && userID==base.UserId)
            {
                OrderInfo tmpOrder = OrderBLL.Read(orderID);
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
                    OrderBLL.Delete(orderID);
                    AdminLogBLL.Add("会员("+userID+")"+ShopLanguage.ReadLanguage("DeleteRecordCompletely"), ShopLanguage.ReadLanguage("Order"), orderID);
                }
                else
                { //已付款逻辑删除可恢复
                    if (tmpOrder.IsDelete == (int)BoolType.False)
                    {
                        tmpOrder.IsDelete = (int)BoolType.True;
                        OrderBLL.Update(tmpOrder);
                        AdminLogBLL.Add("会员(" + userID + ")" + ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Order"), orderID);
                    }
                }
            }
            else
            {
                result = "未登录或登录过期";
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }

        /// <summary>
        /// 手机站获取荣誉资质列表
        /// </summary>
        protected void GetHonorListMobile()
        {
            string result = "0";
            int page = RequestHelper.GetQueryString<int>("Page");
            int classID = RequestHelper.GetQueryString<int>("ClassID");
            int count = 0;
            if (page > 1)
            {
                ArticleSearchInfo arSearch = new ArticleSearchInfo();
                if (classID > 0) arSearch.ClassId = "|" + classID + "|";
                List<ArticleInfo> newsList = ArticleBLL.SearchList(page, 4, arSearch, ref count);
                if (newsList.Count > 0)
                {
                    result = string.Empty;
                    foreach (var article in newsList)
                    {
                        result += "<li><a href=\"/mobile/articledetail-I"+article.Id+".html\" title=\""+article.Title+"\"><img src=\""+ShopCommon.ShowImage(article.Photo.Replace("Original","320-240"))+"\" title=\""+article.Title+"\" alt=\""+article.Title+"\"/></a></li>";
                    }
                }
            }
            Response.Write(result);
            Response.End();
        }

        /// <summary>
        /// 手机站获取文章列表
        /// </summary>
        protected void GetArticleListMobile()
        {
            string result = "0";
            int page = RequestHelper.GetQueryString<int>("Page");
            int classID = RequestHelper.GetQueryString<int>("ClassID");
            int pageSize = RequestHelper.GetQueryString<int>("pageSize");
            string keywords = RequestHelper.GetQueryString<string>("KeyWords");
            int count = 0;
            if (page > 1)
            {
                ArticleSearchInfo arSearch = new ArticleSearchInfo();
                if (string.IsNullOrEmpty(keywords))
                { arSearch.ClassId = "|" + classID + "|"; }
                else
                {
                    arSearch.Title = keywords;
                    //articleSearch.Keywords = keywords;
                   // arSearch.InClassId = "38,44,47";//只搜索 指定的分类
                }

                List<ArticleInfo> newsList = ArticleBLL.SearchList(page, pageSize, arSearch, ref count);
                if (newsList.Count > 0)
                {
                    result = string.Empty;
                    foreach (var article in newsList)
                    {
                        string _url=string.IsNullOrEmpty(article.Url)?"/mobile/newsDetail.html?id="+article.Id : article.Url;
                        result += "<li class=\"\"><a href=\"" + _url + "\" title=\"" + article.Title + "\"><img src=\"" + ShopCommon.ShowImage(article.Photo.Replace("Original", "320-240")) + "\" title=\"" + article.Title + "\" alt=\"" + article.Title + "\"/><h3>" + StringHelper.Substring(article.Title, 13) + "</h3><p class=\"txt\">" + StringHelper.Substring(article.Summary, 28) + "</p><p class=\"b\"><span class=\"time\">时间：" + article.RealDate.ToString("yyyy-MM-dd") + "</span><span class=\"more fr\">查看详情</span></p></a></li>";
                    }
                }
            }
            Response.Write(result);
            Response.End();
        }
        
           /// <summary>
        /// 清除历史搜索记录
        /// </summary>
        protected void DeleteHistorySearch() {
            try
            {
                CookiesHelper.DeleteCookie("HistorySearch");
                Response.Clear();
                ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = "ok", msg = "操作成功" }));
            }
            catch (Exception ex)
            {
                Response.Clear();
                ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = "no", msg = "操作失败" }));
            }
            finally
            {
                ResponseHelper.End();
            }
        
        }

        /// <summary>
        /// 读取购物车最新4件
        /// </summary>
        private void ReadCart()
        {
            try
            {
                string htmlStr = string.Empty;
                var cartList = CartBLL.ReadList(base.UserId);

                //关联的商品
                int count = 0;
                int[] ids = cartList.Select(k => k.ProductId).ToArray();
                var products = ProductBLL.SearchList(1, ids.Count(), new ProductSearchInfo { InProductId = string.Join(",", ids) }, ref count);

                //int productCount = 0;
                //规格
                foreach (var cartInfo in cartList.Take(4))
                {
                    ProductInfo product = ProductBLL.Read(cartInfo.ProductId);
                    decimal curPrice = Convert.ToDecimal(GetPrice(product.Id, product.SalePrice, cartInfo.StandardValueList));
                    if (product.IsSale == 1)
                    {
                        htmlStr += "<li><a class=\"clearfix\" href=\"/ProductDetail-I" + product.Id + ".html\" title=\"" + cartInfo.ProductName + "\"><img class=\"fl\" src=\"" + ShopCommon.ShowImage(product.Photo.Replace("Original", "90-90")) + "\" title=\"" + cartInfo.ProductName + "\" alt=\"" + cartInfo.ProductName + "\" /><h3 class=\"fl\">" + StringHelper.Substring(cartInfo.ProductName, 23) + "</h3><div class=\"pricebox fr\"><span class=\"price\">￥" + curPrice + "×" + cartInfo.BuyCount + "</span></div></a><span class=\"delet\" style=\"cursor:pointer;\" _cid=\"" + cartInfo.Id + "\" _cprice=\"" + curPrice + "\" _dcount=\"" + cartInfo.BuyCount + "\">删除</span></li>";
                    }
                    else { //商品已下架
                        htmlStr += "<li><a class=\"clearfix\" href=\"javascript:alert('此商品已下架');\" title=\"" + cartInfo.ProductName + "\"><img class=\"fl\" src=\"" + ShopCommon.ShowImage(product.Photo.Replace("Original", "90-90")) + "\" title=\"" + cartInfo.ProductName + "\" alt=\"" + cartInfo.ProductName + "\" /><h3 class=\"fl\">" + StringHelper.Substring(cartInfo.ProductName, 23) + "</h3><div class=\"pricebox fr\"><span class=\"price\">￥" + curPrice + "×" + cartInfo.BuyCount + "</span></div></a><span style=\"color:red;\">此商品已下架</span><span class=\"delet\" style=\"cursor:pointer;\" _cid=\"" + cartInfo.Id + "\" _cprice=\"" + curPrice + "\" _dcount=\"" + cartInfo.BuyCount + "\">删除</span></li>";
                    }
                }
                Response.Clear();
                ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = "ok", msg = htmlStr }));
               
            }
            catch (Exception ex) {
                Response.Clear();
                ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = "error", msg = ex.ToString() }));
            }
            finally
            {
                Response.End();
            }
        }
        //计算会员价
        protected string GetPrice(int id, decimal price, string standardValue)
        {
            if (!string.IsNullOrEmpty(standardValue.Trim()))
            {
                return ProductBLL.GetCurrentPriceWithStandard(id, base.GradeID, standardValue).ToString();
            }
            else
            {
                return ProductBLL.GetCurrentPrice(price, base.GradeID).ToString();
            }
        }
        /// <summary>
        /// 查询商品可以享受的商品优惠活动
        /// </summary>
        /// <param name="productClassId">商品分类</param>
        /// <param name="prodcutPrice">商品价格</param>
        protected void ReadProductFavorList() 
        {
            string productClassId = RequestHelper.GetQueryString<string>("classId");
            decimal productPrice = RequestHelper.GetQueryString<decimal>("price");
            List<FavorableActivityInfo> productFavorableActivityList = new List<FavorableActivityInfo>();
            if (!string.IsNullOrEmpty(productClassId) && productPrice > 0)
            {
                var tmpfavorableActivityList = FavorableActivityBLL.ReadList(DateTime.Now, DateTime.Now).Where<FavorableActivityInfo>(f => f.Type == (int)FavorableType.ProductClass && ("," + f.UserGrade + ",").IndexOf("," + base.GradeID.ToString() + ",") > -1).ToList();
                foreach (var favorable in tmpfavorableActivityList)
                {
                    if (productClassId.IndexOf(favorable.ClassIds) > -1 && productPrice >= favorable.OrderProductMoney)
                    {
                        productFavorableActivityList.Add(favorable);
                    }
                }
            }
            Response.Clear();
            ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { count = productFavorableActivityList.Count, dataList = productFavorableActivityList }));
        }
        /// <summary>
        /// 根据输入关键词模糊搜索匹配的关键词集合
        /// </summary>
        protected void GetHotKeys() {
            string keyword = RequestHelper.GetQueryString<string>("keyword");
            List<HotSearchKeyInfo> hotkeys = HotSearchKeyBLL.ReadList(keyword);
            var dys = new List<dynamic>();
            foreach (HotSearchKeyInfo item in hotkeys)
            {
                dynamic dy = new System.Dynamic.ExpandoObject();
                var searchProList = ProductBLL.SearchList(new ProductSearchInfo { IsSale = 1, Key = item.Name });
                if (searchProList.Count > 0)
                {
                    dy.id = item.Id;
                    dy.name = item.Name;
                    dy.spell = ChineseCharacterHelper.ConvertToSpelling(item.Name);
                    dy.productcount = searchProList.Count;
                    dys.Add(dy);
                }

            }
            string json = string.Empty;
            if (dys.Count > 0)
            {
                json = Newtonsoft.Json.JsonConvert.SerializeObject(from item in dys select new { id = item.id, name = item.name, spell = item.spell, productcount = item.productcount });
            }
            else
            {
                json = Newtonsoft.Json.JsonConvert.SerializeObject(new { id = 0, name = string.Empty, spell = string.Empty, productcount = 0 });
            }
            ResponseHelper.Write(json);
        
            Response.End();
        }
        /// <summary>
        /// PC站首页获取楼层商品
        /// </summary>
        protected void GetProductsByFloor()
        {
            int productClassId = RequestHelper.GetQueryString<int>("classId");
          
            ProductSearchInfo prosearch = new ProductSearchInfo();
            prosearch.IsSale = (int)BoolType.True;
            prosearch.IsDelete = (int)BoolType.False;
            prosearch.ClassId = "|" + productClassId + "|";
           
            int count = 0;
            var productlist = ProductBLL.SearchList(1, 8, prosearch, ref count);
            var resultlist = new List<ProductInfo>();
            foreach (var pro in productlist)
            {
                pro.Photo = ShopCommon.ShowImage(pro.Photo.Replace("Original", "150-150"));
                pro.SalePrice = ProductBLL.GetCurrentPrice(pro.SalePrice, base.GradeID);
                resultlist.Add(pro);
            }
            ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { count = productlist.Count, dataList = resultlist }));
            Response.End();
        }
        /// <summary>
        /// 验证用户是否已登录
        /// </summary>
        private void VerifyLogin()
        {
            ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { haslogin = base.UserId > 0 }));
            ResponseHelper.End();
        }
        /// <summary>
        /// 会员获取优惠券
        /// </summary>
        protected void CatchUserCoupon()
        {
            int userId = RequestHelper.GetQueryString<int>("UserId");
            int couponId = RequestHelper.GetQueryString<int>("CouponId");
            if (userId > 0 && couponId > 0)
            {
                if (UserCouponBLL.UniqueUserCatch(userId, couponId))
                {
                    UserCouponInfo userCoupon = UserCouponBLL.ReadLast(couponId);
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
                    UserCouponInfo tmpCoupon = new UserCouponInfo();
                    tmpCoupon.CouponId = couponId;
                    tmpCoupon.GetType = (int)CouponType.CatchByUser;
                    tmpCoupon.Number = ShopCommon.CreateCouponNo(couponId, startNumber);
                    tmpCoupon.Password = ShopCommon.CreateCouponPassword(startNumber);
                    tmpCoupon.IsUse = (int)BoolType.False;
                    tmpCoupon.OrderId = 0;
                    tmpCoupon.UserId = userId;
                    tmpCoupon.UserName = UserBLL.Read(userId).UserName;
                    UserCouponBLL.Add(tmpCoupon);
                    ResponseHelper.Write(JsonConvert.SerializeObject(new { flag = "ok", msg = "领券成功" }));
                    Response.End();
                }
                else
                {
                    ResponseHelper.Write(JsonConvert.SerializeObject(new { flag = "no", msg = "您已领过此优惠券" }));
                    Response.End();
                }
            }
            else
            {
                ResponseHelper.Write(JsonConvert.SerializeObject(new { flag = "unlogin", msg = "请先登录" }));
                Response.End();
            }
        }

        /// <summary>
        /// 加载PC站投票项目
        /// </summary>
        protected void GetVoteItemList()
        {

            int voteID = RequestHelper.GetQueryString<int>("VoteID");

            List<VoteItemInfo> vtListM = VoteItemBLL.ReadVoteItemByVote(voteID);
            List<VoteItemInfo> resultList = new List<VoteItemInfo>();
            //#region #返回JSON

            //if (vtListM.Count > 0)
            //{

            //    foreach (var item in vtListM)
            //    {
            //        if (item.Exp2 == "1")
            //        {
            //            //item.Department = StringHelper.Substring(item.Department, 28);
            //            item.Image = ShopCommon.ShowImage(item.Image.Replace("Original", "195-195"));
            //            resultList.Add(item);
            //        }
            //    }

            //}
            //#endregion

            //Response.Write(JsonConvert.SerializeObject(new { count = resultList.Count, dataList = resultList }));

            var _voteList = VoteBLL.ReadChilds(voteID).OrderBy(k => k.OrderID);
            string _html = "";
            foreach (var _vote in _voteList)
            {
                _html += "<ul >";
                //List<VoteItemInfo> vtList1=VoteItemBLL.ReadVoteItemByVote(_vote.ID); 
                List<VoteItemInfo> vtList1 = VoteItemBLL.ReadVoteItemList(new VoteItemSearchInfo { VoteID = "|" + _vote.ID + "|", Exp2 = "1" });
                foreach (VoteItemInfo vf in vtList1)
                {
                    if (vf.Exp2 == "1")
                    {
                        _html += "<li><a href=\"/ShowMe/" + vf.ID + ".html\" target=\"_blank\" title=\"" + vf.ItemName + "\"><img src=\"" + ShopCommon.ShowImage(vf.Image.Replace("Original", "195-195")) + "\" alt=\"" + vf.ItemName + "\" title=\"" + vf.ItemName + "\" /></a><h3>" + VoteBLL.ReadLastVoteClass(vf.VoteID).Title + "</h3><h2 class=\"name\"><a href=\"/ShowMe/" + vf.ID + ".html\" target=\"_blank\" title=\"" + vf.ItemName + "\">" + StringHelper.Substring(vf.ItemName, 9) + "</a></h2><span class=\"inco\" title=\"" + vf.Department + "\">" + StringHelper.Substring(vf.Department, 29) + "</span><p><span class=\"fl\"><span class=\"red number\" id=\"voteitem" + vf.ID + "\">" + vf.VoteCount + "</span>票</span><a class=\"tp fr\" href=\"javascript:\" _vid=\"" + vf.ID + "\">投票</a></p></li>";
                    }
                }
                _html += "</ul>";
            }
            Response.Write(_html);
            Response.End();
        }
        /// <summary>
        /// PC站首次打开加载除了前面2个分类以外的选项
        /// </summary>
        protected void GetVoteItemsFirst()
        {
            int voteID = RequestHelper.GetQueryString<int>("VoteID");

            List<VoteItemInfo> vtListM = VoteItemBLL.ReadVoteItemByVote(voteID);
            List<VoteItemInfo> resultList = new List<VoteItemInfo>();


            var _voteList = VoteBLL.ReadChilds(voteID).OrderBy(k => k.OrderID).Skip(2);
            string _html = "";
            foreach (var _vote in _voteList)
            {
                _html += "<ul >";
                List<VoteItemInfo> vtList1 = VoteItemBLL.ReadVoteItemList(new VoteItemSearchInfo { VoteID = "|" + _vote.ID + "|", Exp2 = "1" });
                foreach (VoteItemInfo vf in vtList1)
                {
                    if (vf.Exp2 == "1")
                    {
                        _html += "<li><a href=\"/ShowMe/" + vf.ID + ".html\" target=\"_blank\" title=\"" + vf.ItemName + "\"><img src=\"" + ShopCommon.ShowImage(vf.Image.Replace("Original", "195-195")) + "\" alt=\"" + vf.ItemName + "\" title=\"" + vf.ItemName + "\" /></a><h3>" + VoteBLL.ReadLastVoteClass(vf.VoteID).Title + "</h3><h2 class=\"name\"><a href=\"/ShowMe/" + vf.ID + ".html\" target=\"_blank\" title=\"" + vf.ItemName + "\">" + StringHelper.Substring(vf.ItemName, 9) + "</a></h2><span class=\"inco\" title=\"" + vf.Department + "\">" + StringHelper.Substring(vf.Department, 29) + "</span><p><span class=\"fl\"><span class=\"red number\" id=\"voteitem" + vf.ID + "\">" + vf.VoteCount + "</span>票</span><a class=\"tp fr\" href=\"/ShowMe/" + vf.ID + ".html\" _vid=\"" + vf.ID + "\">投票</a></p></li>";
                    }
                }
                _html += "</ul>";
            }
            Response.Write(_html);
            Response.End();
        }
        /// <summary>
        /// 加载手机站投票项目
        /// </summary>
        protected void GetVoteItemListMobile()
        {
            string result = string.Empty;
            int voteID = RequestHelper.GetQueryString<int>("VoteID");
            List<VoteItemInfo> vtListM = VoteItemBLL.ReadVoteItemByVote(voteID);
            List<VoteItemInfo> resultList = new List<VoteItemInfo>();
            //#region #返回JSON

            //if (vtListM.Count > 0)
            //{

            //    foreach (var item in vtListM)
            //    {
            //        if (item.Exp2 == "1")
            //        {
            //            item.Department = StringHelper.Substring(item.Department, 28);
            //            //item.Image = ShopCommon.ShowImage(item.Image.Replace("Original", "250-250"));
            //            item.Image = ShopCommon.ShowImage(item.Image);
            //            resultList.Add(item);
            //        }
            //    }

            //}
            //#endregion

            //Response.Write(JsonConvert.SerializeObject(new { count = resultList.Count, dataList = resultList }));
            int vti = 1;
            var _voteList = VoteBLL.ReadChilds(voteID).OrderBy(k => k.OrderID);
            string _html = "";
            foreach (var _vote in _voteList)
            {
                _html += "<ul >";
                //List<VoteItemInfo> vtList1=VoteItemBLL.ReadVoteItemByVote(_vote.ID);
                List<VoteItemInfo> vtList1 = VoteItemBLL.ReadVoteItemList(new VoteItemSearchInfo { VoteID = "|" + _vote.ID + "|", Exp2 = "1" });
                if (vtList1.Count > 0)
                {
                    foreach (VoteItemInfo vf in vtList1)
                    {
                        if (vf.Exp2 == "1")
                        {
                            var _class = "";
                            if (vti % 2 != 0) { _class = "fl"; } else { _class = "fr"; };
                            _html += "<li class=\"fl\"><a class=\"" + _class + "\" href=\"/mobile/ShowMe/" + vf.ID + ".html\" title=\"" + vf.ItemName + "\"><img src=\"" + ShopCommon.ShowImage(vf.Image.Replace("Original", "195-195")) + "\" alt=\"" + vf.ItemName + "\" title=\"" + vf.ItemName + "\" /><h3>" + VoteBLL.ReadLastVoteClass(vf.VoteID).Title + "</h3><h2>" + StringHelper.Substring(vf.ItemName, 7) + "</h2><span>" + StringHelper.Substring(vf.Department, 28) + "</span></a><span class=\"fl w50 left\"><span class=\"red number\" id=\"voteitem" + vf.ID + "\">" + vf.VoteCount + "</span>票</span><span class=\"fr w50 right\"  _vid=\"" + vf.ID + "\"><a style=\"cursor:pointer\">投票</a></span></li>";
                            vti++;

                        }
                    }
                }
                _html += "</ul>";
            }
            Response.Write(_html);
            Response.End();
        }

        /// <summary>
        /// 手机站 首次打开加载除了前面2个分类以外的选项
        /// </summary>
        protected void GetVoteItemsFirstMobile()
        {
            int voteID = RequestHelper.GetQueryString<int>("VoteID");

            List<VoteItemInfo> vtListM = VoteItemBLL.ReadVoteItemByVote(voteID);
            List<VoteItemInfo> resultList = new List<VoteItemInfo>();

            int vti = 1;
            var _voteList = VoteBLL.ReadChilds(voteID).OrderBy(k => k.OrderID).Skip(2);
            string _html = "";
            foreach (var _vote in _voteList)
            {
                _html += "<ul >";
                List<VoteItemInfo> vtList1 = VoteItemBLL.ReadVoteItemList(new VoteItemSearchInfo { VoteID = "|" + _vote.ID + "|", Exp2 = "1" });
                foreach (VoteItemInfo vf in vtList1)
                {
                    if (vf.Exp2 == "1")
                    {
                        var _class = "";
                        if (vti % 2 != 0) { _class = "fl"; } else { _class = "fr"; };
                        _html += "<li class=\"fl\"><a class=\"" + _class + "\" href=\"/mobile/ShowMe/" + vf.ID + ".html\" title=\"" + vf.ItemName + "\"><img src=\"" + ShopCommon.ShowImage(vf.Image.Replace("Original", "195-195")) + "\" alt=\"" + vf.ItemName + "\" title=\"" + vf.ItemName + "\" /><h3>" + VoteBLL.ReadLastVoteClass(vf.VoteID).Title + "</h3><h2>" + StringHelper.Substring(vf.ItemName, 7) + "</h2><span>" + StringHelper.Substring(vf.Department, 28) + "</span></a><span class=\"fl w50 left\"><span class=\"red number\" id=\"voteitem" + vf.ID + "\">" + vf.VoteCount + "</span>票</span><span class=\"fr w50 right\"  _vid=\"" + vf.ID + "\"><a style=\"cursor:pointer\">投票</a></span></li>";
                        vti++;
                    }
                }
                _html += "</ul>";
            }
            Response.Write(_html);
            Response.End();
        }
    }
}