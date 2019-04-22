using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using JWShop.XcxApi.Filter;
using JWShop.XcxApi.Pay;
using JWShop.Entity;
using JWShop.Business;
using JWShop.Common;
using SkyCES.EntLib;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace JWShop.XcxApi.Controllers
{
    [Auth]
    [CheckLogin]
    public class UserController : Controller
    {
        UserInfo user = new UserInfo();
        int uid = 0;
        int userGrade = UserGradeBLL.ReadByMoney(0).Id;
        UserGradeInfo userGradeInfo = new UserGradeInfo();
        public UserController()
        {
            uid = RequestHelper.GetForm<int>("uid");
            if (uid <= 0) uid = RequestHelper.GetQueryString<int>("uid");
            user = UserBLL.ReadUserMore(uid);
            if (user != null && user.Id > 0)
            {
                userGradeInfo = UserGradeBLL.ReadByMoney(user.MoneyUsed);
                userGrade = userGradeInfo.Id;
            }
        }
        /// <summary>
        /// 获取用户基本信息：真实姓名、生日、邮箱
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserInfo()
        {
            if (user.Id <= 0)
            {
                return Json(new { ok=false},JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ok=true,info=user}, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 用户完善基本信息：真实姓名、生日、邮箱
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateUserInfo()
        {
            if (user.Id <= 0) { return Json(new { ok = false, msg = "无效的请求" }); }
            else
            {
                string birthdayBeforeUpdate = user.Birthday;
                string realName = RequestHelper.GetForm<string>("realname");
                string birthDay = RequestHelper.GetForm<string>("birthday");
                string email = RequestHelper.GetForm<string>("email");
                if (string.IsNullOrEmpty(realName))
                {
                    return Json(new { ok = false, msg = "请填写真实姓名" });
                }
                if (string.IsNullOrEmpty(birthDay))
                {
                    return Json(new { ok = false, msg = "请填写生日" });
                }
                if (Convert.ToDateTime(birthDay) >= DateTime.Now)
                {
                    return Json(new { ok = false, msg = "请填写真实生日" });
                }
                if (!string.IsNullOrEmpty(email) && !ShopCommon.CheckEmail(email))
                {
                    return Json(new { ok = false, msg = "邮箱格式错误" });
                }
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("[RealName]", StringHelper.Substring(realName, 18, false));
                dict.Add("[Birthday]", StringHelper.Substring(birthDay, 30, false));
                dict.Add("[Email]", StringHelper.Substring(email, 30, false));
                UserBLL.UpdatePart("[usr]", dict, user.Id);
                //如果是首次填写生日信息则赠送积分20
                if (string.IsNullOrEmpty(birthdayBeforeUpdate))
                {
                    UserAccountRecordInfo uar = new UserAccountRecordInfo
                    {
                        RecordType = (int)AccountRecordType.Point,
                        Money = 0,
                        Point = 20,
                        Date = DateTime.Now,
                        IP = ClientHelper.IP,
                        Note = "填写生日赠送积分",
                        UserId = user.Id,
                        UserName = user.UserName
                    };
                    if (UserAccountRecordBLL.Add(uar) > 0)
                    {
                        user.PointLeft += 20;
                    }
                }           
                return Json(new { ok = true });

            }
        }
        public ActionResult Index()
        {
            int[] arrT = new int[5];
            //检查用户的待付款订单是否超时失效，超时则更新为失效状态
            OrderBLL.CheckOrderPayTime(user.Id);
            //订单自动收货
            OrderBLL.CheckOrderRecieveTimeProg(user.Id);
         
            OrderSearchInfo orderSearch = new OrderSearchInfo();
            orderSearch.OrderStatus = (int)(OrderStatus.WaitPay);
            orderSearch.UserId = uid;
            orderSearch.IsDelete = 0;
            arrT[0] = OrderBLL.SearchList(orderSearch).Count;
            orderSearch.OrderStatus = (int)(OrderStatus.WaitCheck);
            orderSearch.UserId = uid;
            orderSearch.IsDelete = 0;
            //待审核
            var orderList = OrderBLL.SearchList(orderSearch);
            arrT[1] =orderList.Count;
            #region 待分享
            var list1 = orderList.Where(k => k.IsActivity == (int)OrderKind.GroupBuy && k.FavorableActivityId > 0);
            var groupIds = list1.Select(k => k.FavorableActivityId).ToArray();
            var groupList = GroupBuyBLL.ReadList(groupIds).Where(k=>k.StartTime<=DateTime.Now && k.EndTime>=DateTime.Now && k.Quantity>k.SignCount);
            var ids = groupList.Select(k => k.Id);
            var list2 = list1.Where(k => ids.Contains(k.FavorableActivityId));
            arrT[4] = list2.Count();
            #endregion

            orderSearch.OrderStatus = (int)(OrderStatus.HasShipping);
            orderSearch.UserId = uid;
            orderSearch.IsDelete = 0;
            arrT[2] = OrderBLL.SearchList(orderSearch).Count;
            orderSearch.OrderStatus = (int)(OrderStatus.ReceiveShipping);
            orderSearch.UserId = uid;
            orderSearch.IsDelete = 0;
            arrT[3] = OrderBLL.SearchList(orderSearch).Count;
            //是否填写手机号
            bool hasMobile = !string.IsNullOrEmpty(user.Mobile.Trim());
            #region 生日前后5天登录会员中心，自动发放本年度生日礼券。（ 是否获取生日优惠券,本年生日前后5天出现提示）
            //判断是否能够获取本年度生日礼券
            bool canGetBirthdayCoupon = true;
            DateTime birthday = DateTime.MinValue;
            //如果用户已填写生日，则在生日前后5天提示领取生日礼券
            if (DateTime.TryParse(user.Birthday, out birthday) && (birthday.AddDays(-5).Month == DateTime.Now.Month && birthday.AddDays(-5).Day<= DateTime.Now.Day) && (birthday.AddDays(5).Month == DateTime.Now.Month && birthday.AddDays(5).Day >= DateTime.Now.Day))
            {
                int count = 0;
                var couponlist = CouponBLL.SearchList(1, 1, new CouponSearchInfo { Type = (int)CouponKind.BirthdayGet, CanUse = 1 }, ref count);
                //如果有生日礼券活动进行中
                if (couponlist.Count > 0)
                {
                    ////如果本年度已获取，则不能再获取
                    //if (user.HasBirthdayCoupon != 0 && user.GetBirthdayCouponDate.Year == DateTime.Now.Year) canGetBirthdayCoupon = false;
                    #region 自动发放，每年度限领1次
                    if (user.HasBirthdayCoupon == 0 || (user.HasBirthdayCoupon != 0 && user.GetBirthdayCouponDate.Year != DateTime.Now.Year))
                    {
                        UserCouponInfo userCoupon = UserCouponBLL.ReadLast(couponlist[0].Id);
                        if (UserCouponBLL.UniqueUserCatch(user.Id, couponlist[0].Id))
                        {
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
                            int cpid = UserCouponBLL.Add(new UserCouponInfo
                            {
                                UserId = user.Id,
                                UserName = user.UserName,
                                CouponId = couponlist[0].Id,
                                GetType = (int)CouponType.BirthdayGet,
                                Number = ShopCommon.CreateCouponNo(couponlist[0].Id, startNumber),
                                Password = ShopCommon.CreateCouponPassword(startNumber),
                                IsUse = (int)BoolType.False,
                                OrderId = 0

                            });
                            //领取成功改变user表标识
                            if (cpid > 0)
                            {
                                Dictionary<string, object> dict = new Dictionary<string, object>();
                                dict.Add("[HasBirthdayCoupon]", 1);
                                dict.Add("[GetBirthdayCouponDate]", DateTime.Now);
                                UserBLL.UpdatePart("[usr]", dict, user.Id);
                                //user对应的未使用优惠券数量加1
                                user.CouPonLeft++;
                            }

                        }
                    }
                    #endregion
                }
                else
                {
                    //如果商家后台没有设置正在进行中的会员生日券，则不能获取
                    canGetBirthdayCoupon = false;
                }
            }
            else
            {
                canGetBirthdayCoupon = false;
            }
            #endregion   
            //计算是否
            return Json(new { ordercount = arrT, hasmobile = hasMobile, usergrade = userGradeInfo, cangetbirthdaycoupon = canGetBirthdayCoupon,couponleft=user.CouPonLeft,pointleft=user.PointLeft }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddAddress()
        {
            var entity = new UserAddressInfo();

            int updateId = RequestHelper.GetForm<int>("updateId");
            entity.Consignee = StringHelper.AddSafe(RequestHelper.GetForm<string>("consignee"));
            entity.Address = StringHelper.AddSafe(RequestHelper.GetForm<string>("address"));
            entity.Mobile = StringHelper.AddSafe(RequestHelper.GetForm<string>("mobile"));
            entity.Tel = StringHelper.AddSafe(RequestHelper.GetForm<string>("tel"));
            entity.Email = StringHelper.AddSafe(RequestHelper.GetForm<string>("email"));
            entity.IsDefault = Convert.ToInt32(RequestHelper.GetForm<bool>("isdefault"));

            if (entity.IsDefault > 0) UserAddressBLL.SetDefault(entity.Id, uid);

            SingleUnlimitClass unlimitClass = new SingleUnlimitClass();
            entity.RegionId = unlimitClass.ClassID;

            if (string.IsNullOrEmpty(entity.Consignee))
            {
                return Content("请填写收货人");
            }
            var region = StringHelper.AddSafe(RequestHelper.GetForm<string>("region"));
            if (!string.IsNullOrEmpty(region))
            {
                string[] regarr = region.Split(',');
                if (regarr.Length < 3)
                {
                    return Content("请填写完整的地区信息");
                }
                else
                {
                    string province = regarr[0];
                    string city = regarr[1];
                    string town = regarr[2];

                    if (!string.IsNullOrEmpty(town))
                    {
                        entity.RegionId = RegionBLL.ReadRegionIdList(province, city, town);
                    }
                    else
                    {
                        entity.RegionId = RegionBLL.ReadRegionIdList(city);
                    }
                }
            }
            else
            {
                return Content("请填写完整的地区信息");
            }
            if (string.IsNullOrEmpty(entity.Address))
            {
                return Content("请填写详细地址");
            }
            if (string.IsNullOrEmpty(entity.Mobile))
            {
                return Content("手机号码不能为空");
            }

            entity.UserId = user.Id;
            entity.UserName = user.UserName;

            string msg = "添加成功";
            if (updateId > 0)
            {
                entity.Id = updateId;
                UserAddressBLL.Update(entity);
                msg = "修改成功";
            }
            else updateId = UserAddressBLL.Add(entity);

            return Json(new { flag = true, msg = msg, id = updateId });
        }

        public ActionResult GetAddress()
        {
            var address = UserAddressBLL.ReadList(uid);

            List<VirtualAddress> addresslist = new List<VirtualAddress>();

            foreach (var item in address)
            {
                VirtualAddress newaddress = new VirtualAddress();
                newaddress.id = item.Id;
                newaddress.name = item.Consignee;
                newaddress.address = RegionBLL.RegionNameList(item.RegionId) + " " + item.Address;
                newaddress.mobile = item.Mobile;
                newaddress.isdefault = Convert.ToBoolean(item.IsDefault);
                addresslist.Add(newaddress);
            }

            return Json(new { address = addresslist }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAddressById(int id)
        {
            var item = UserAddressBLL.Read(id, uid);
            VirtualAddress newaddress = new VirtualAddress();
            newaddress.id = item.Id;
            newaddress.name = item.Consignee;
            newaddress.address = item.Address;
            newaddress.mobile = item.Mobile;
            newaddress.isdefault = Convert.ToBoolean(item.IsDefault);
            newaddress.regionnames = string.Join(",", RegionBLL.RegionNameList(item.RegionId).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Skip(1));

            return Json(new { address = newaddress }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DelAddress(int id)
        {
            UserAddressBLL.Delete(id, uid);

            return Json(new { flag = true, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddProductComment(int productID, int orderID)
        {
            string commentCookies = CookiesHelper.ReadCookieValue("CommentCookies" + productID.ToString());
            if (ShopConfig.ReadConfigInfo().CommentRestrictTime > 0 && commentCookies != string.Empty)
            {
                return Content("请不要频繁提交");
            }
            else
            {
                var procomList = ProductCommentBLL.SearchProductCommentList(new ProductCommentSearchInfo() { OrderID = orderID, ProductId = productID, UserId = uid });
                if (procomList.Count > 0) return Content("已经提交过相关评论");
                ProductCommentInfo productComment = new ProductCommentInfo();
                productComment.ProductId = productID;
                productComment.Title = StringHelper.AddSafe(RequestHelper.GetForm<string>("Title"));
                productComment.Content = StringHelper.AddSafe(RequestHelper.GetForm<string>("Content"));
                productComment.UserIP = ClientHelper.IP;
                productComment.PostDate = RequestHelper.DateNow;
                productComment.Support = 0;
                productComment.Against = 0;
                productComment.Status = ShopConfig.ReadConfigInfo().CommentDefaultStatus;
                productComment.Rank = RequestHelper.GetForm<int>("Rank");
                productComment.ReplyCount = 0;
                productComment.AdminReplyContent = string.Empty;
                productComment.AdminReplyDate = RequestHelper.DateNow;
                productComment.UserId = uid;
                productComment.UserName = user.UserName;
                productComment.OrderId = orderID;
                int pcid = ProductCommentBLL.Add(productComment);
                if (ShopConfig.ReadConfigInfo().CommentRestrictTime > 0)
                {
                    CookiesHelper.AddCookie("CommentCookies" + productID.ToString(), "CommentCookies" + productID.ToString(), ShopConfig.ReadConfigInfo().CommentRestrictTime, TimeType.Second);
                }

                return Content("ok|" + pcid);
            }
        }
        /// <summary>
        /// 用户优惠券列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserCoupon()
        {
            int couponType = RequestHelper.GetQueryString<int>("userCouponType");
            UserCouponSearchInfo searchInfo = new UserCouponSearchInfo();
            searchInfo.UserId = uid;
            searchInfo.IsTimeOut = -1;//不限期限有没过期
            //未使用
            if (couponType == 1)
            {
                searchInfo.IsUse = (int)BoolType.False;
                searchInfo.IsTimeOut = (int)BoolType.False;
            }
            //已使用
            if (couponType == 2)
            {
                searchInfo.IsUse = (int)BoolType.True;
                searchInfo.IsTimeOut = -1;//不限期限有没过期
            }
            //已过期
            if (couponType == 3)
            {
                searchInfo.IsUse = (int)BoolType.False;
                searchInfo.IsTimeOut = (int)BoolType.True;

            }

            var ucouponlist = UserCouponBLL.SearchList(searchInfo);

            List<VirtualCoupon> vucoupon = new List<VirtualCoupon>();
            foreach (var item in ucouponlist)
            {
                CouponInfo tempCoupon = CouponBLL.Read(item.CouponId);
                VirtualCoupon vcou = new VirtualCoupon()
                {
                    id = item.Id,
                    name = tempCoupon.Name,
                    money = tempCoupon.Money,
                    minmoney = tempCoupon.UseMinAmount,
                    startdate = tempCoupon.UseStartDate,
                    enddate = tempCoupon.UseEndDate,
                    isused = item.IsUse,
                };
                vucoupon.Add(vcou);
            }

            return Json(new { ucouponlist = vucoupon }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 用户领取普通优惠券
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCouponByUser()
        {
            int couponId = RequestHelper.GetForm<int>("couponId");

            if (uid > 0 && couponId > 0)
            {
                CouponInfo coupon = CouponBLL.Read(couponId);
                if (UserCouponBLL.UniqueUserCatch(uid, couponId))
                {
                    if (coupon.TotalCount - 1 < coupon.UsedCount)
                    {
                        return Json(new { ok = false, msg = "优惠券余量不足" });
                    }
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
                    tmpCoupon.UserId = uid;
                    tmpCoupon.UserName = user.UserName;
                   if(UserCouponBLL.Add(tmpCoupon)>0)
                    {//如果领取成功，增加优惠券的发放（领取）量
                        coupon.UsedCount++;
                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        dict.Add("[UsedCount]", coupon.UsedCount);
                        CouponBLL.UpdatePart("[Coupon]", dict, coupon.Id);
                    }
                    return Json(new { ok = true, msg = "领券成功" });

                }
                else
                {
                    return Json(new { ok = false, msg = "您已领过此优惠券" });
                }
            }
            else
            {
                return Json(new { ok = false, msg = "请先登录" });
            }
        }
        /// <summary>
        /// 会员领取本年度生日礼券
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetBirthdayCoupon()
        {
            int count = 0;
            var couponlist = CouponBLL.SearchList(1, 1, new CouponSearchInfo { Type = (int)CouponKind.BirthdayGet, CanUse = 1 }, ref count);
            //如果有进行中的生日礼券，并且 用户本年度未领取生日礼券
            if (couponlist.Count > 0 && (user.HasBirthdayCoupon == 0 || (user.HasBirthdayCoupon != 0 && user.GetBirthdayCouponDate.Year != DateTime.Now.Year)))
            {
                UserCouponInfo userCoupon = UserCouponBLL.ReadLast(couponlist[0].Id);
                if (UserCouponBLL.UniqueUserCatch(user.Id, couponlist[0].Id))
                {
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
                    int cpid = UserCouponBLL.Add(new UserCouponInfo
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        CouponId = couponlist[0].Id,
                        GetType = (int)CouponType.CatchByUser,
                        Number = ShopCommon.CreateCouponNo(couponlist[0].Id, startNumber),
                        Password = ShopCommon.CreateCouponPassword(startNumber),
                        IsUse = (int)BoolType.False,
                        OrderId = 0

                    });
                    //领取成功改变user表标识
                    if (cpid > 0)
                    {
                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        dict.Add("[HasBirthdayCoupon]", 1);
                        dict.Add("[GetBirthdayCouponDate]", DateTime.Now);
                        UserBLL.UpdatePart("[usr]", dict, user.Id);
                        return Json(new { ok = true });
                    }
                    return Json(new { ok = false, msg = "领取失败，请稍后重试" });
                }
                else
                {
                    return Json(new { ok = false, msg = "领取失败，您已领取过生日礼券" });

                }
            }
            else
            {
                return Json(new { ok = false, msg = "活动暂停或者您已领取过生日礼券" });
            }
        }

        //weixin  getphonenumber 解密
        public ActionResult DecryptAES(string encryptedDataStr, string iv, string code)
        {
            //iv = iv.Replace(" ", "+");
            //sessionKey = sessionKey.Replace(" ", "+");
            //encryptedDataStr = encryptedDataStr.Replace(" ", "+");
            JsApiPay jsapi = new JsApiPay();
            jsapi.GetOpenidAndAccessTokenFromCode(code);
            if (jsapi.access_token != string.Empty)
            {
                RijndaelManaged rijalg = new RijndaelManaged();
                //设置 cipher 格式 AES-128-CBC      

                rijalg.KeySize = 128;
                rijalg.Padding = PaddingMode.PKCS7;
                rijalg.Mode = CipherMode.CBC;

                rijalg.Key = Convert.FromBase64String(jsapi.access_token);
                rijalg.IV = Convert.FromBase64String(iv);


                byte[] encryptedData = Convert.FromBase64String(encryptedDataStr);
                //解密      
                ICryptoTransform decryptor = rijalg.CreateDecryptor(rijalg.Key, rijalg.IV);

                string result;

                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            result = srDecrypt.ReadToEnd();
                        }
                    }
                }
                var phoneInfo = JsonConvert.DeserializeObject<WxPhone>(result);
                return Json(new { ok = true, data = phoneInfo }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ok = false, errmsg = "无效Code" }, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// 绑定用户微信对应的手机号码 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BindMobile()
        {
            bool ok = false;
            string msg = string.Empty;
            int pointLeft = user.PointLeft;
            string mobileBefore = user.Mobile;
            try
            {
                string mobile = RequestHelper.GetForm<string>("mobile");
                if (!string.IsNullOrEmpty(mobile))
                {                   
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Mobile", mobile.Trim());
                    UserBLL.UpdatePart(UserInfo.TABLENAME, dict, user.Id);
                    //首次绑定手机号默认赠送20积分
                    if (string.IsNullOrEmpty(mobileBefore)) {
                        UserAccountRecordInfo uar = new UserAccountRecordInfo
                        {
                            RecordType = (int)AccountRecordType.Point,
                            Money = 0,
                            Point = 20,
                            Date = DateTime.Now,
                            IP = ClientHelper.IP,
                            Note = "绑定手机号赠送积分",
                            UserId = user.Id,
                            UserName = user.UserName
                        };

                        if (UserAccountRecordBLL.Add(uar) > 0)
                        {
                            user.PointLeft += 20;
                        }
                    }
                    ok = true;
                    pointLeft = user.PointLeft;
                }
                else
                {
                    msg = "mobile empty";
                }
            }
            catch (Exception ex)
            {
                ok = false;
                msg = ex.Message;
            }
            return Json(new { ok = ok, msg = msg,pointleft= pointLeft });
        }
        [HttpPost]
        public ActionResult UploadImage()
        {
            string filePath = "/Upload/ProductPhoto/Original/" + RequestHelper.DateNow.ToString("yyyyMM") + "/";
            if (FileHelper.SafeFullDirectoryName(filePath))
            {
                try
                {
                    //上传文件
                    UploadHelper upload = new UploadHelper();
                    upload.Path = "/Upload/ProductPhoto/Original/" + RequestHelper.DateNow.ToString("yyyyMM") + "/";
                    upload.FileType = ShopConfig.ReadConfigInfo().UploadFile;
                    upload.FileNameType = FileNameType.Guid;
                    upload.MaxWidth = ShopConfig.ReadConfigInfo().AllImageWidth;//整站图片压缩开启后的压缩宽度
                    upload.AllImageIsNail = ShopConfig.ReadConfigInfo().AllImageIsNail;//整站图片压缩开关
                    int needNail = RequestHelper.GetQueryString<int>("NeedNail");
                    if (needNail <= 0) upload.AllImageIsNail = 0;//如果页面传值不压缩图片，以页面传值为准;
                    int curMaxWidth = RequestHelper.GetQueryString<int>("CurMaxWidth");//页面传值最大宽度
                    if (curMaxWidth > 0)
                    {
                        upload.AllImageIsNail = 1;
                        upload.MaxWidth = curMaxWidth;//如果有页面传值设置图片最大宽度，以页面传值为准
                    }
                    FileInfo file = null;
                    int waterType = ShopConfig.ReadConfigInfo().WaterType;

                    //前台上传图片不打水印直接保存
                    file = upload.SaveAs();

                    //生成处理
                    string originalFile = upload.Path + file.Name;
                    string otherFile = string.Empty;
                    string makeFile = string.Empty;

                    Dictionary<int, int> dic = new Dictionary<int, int>();

                    if (!dic.ContainsKey(75)) dic.Add(75, 75);//后台商品图集默认使用尺寸(如果不存在则手动添加)

                    foreach (KeyValuePair<int, int> de in dic)
                    {
                        makeFile = originalFile.Replace("Original", de.Key + "-" + de.Value);
                        otherFile += makeFile + "|";
                        ImageHelper.MakeThumbnailImage(ServerHelper.MapPath(originalFile), ServerHelper.MapPath(makeFile), Convert.ToInt32(de.Key), Convert.ToInt32(de.Value), ThumbnailType.AllFix);
                    }
                    otherFile = otherFile.Substring(0, otherFile.Length - 1);
                    int proStyle = RequestHelper.GetForm<int>("proStyle");
                    if (proStyle < 0)
                    {
                        proStyle = 0;
                    }

                    int pcid = RequestHelper.GetForm<int>("pcid");
                    ProductPhotoInfo productPhoto = new ProductPhotoInfo();
                    productPhoto.ProductId = pcid;
                    productPhoto.ImageUrl = originalFile;
                    productPhoto.ProStyle = proStyle;
                    productPhoto.OrderId = 0;

                    ProductPhotoBLL.Add(productPhoto);
                    return Content("ok|" + originalFile.Replace("Original", "75-75"));
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
            }
            else
            {
                return Content(ShopLanguage.ReadLanguage("ErrorPathName"));
            }
        }
        /// <summary>
        /// 获取我的拼团(开团、参团)
        /// </summary>
        /// <returns></returns>
        public ActionResult GroupList(int pageIndex,int pageSize)
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            if (pageSize <= 0)
            {
                return Json(new { ok = false, msg = "参数错误，请稍后重试" },JsonRequestBehavior.AllowGet);
            }
            int count = 0;
            List<GroupSignInfo> groupList = GroupSignBLL.SearchListByUserId(user.Id, pageIndex, pageSize, ref count);
          
            groupList.ForEach(k => k.VirtualStatus = ((k.EndTime < DateTime.Now && k.Quantity > k.SignCount) ? -1 : (k.StartTime <= DateTime.Now && k.EndTime >= DateTime.Now && k.Quantity > k.SignCount) ? 0 : (k.StartTime <= DateTime.Now && k.EndTime >= DateTime.Now && k.Quantity <= k.SignCount) ? 1 : -1));
            return Json(new { ok = true,datalist=groupList }, JsonRequestBehavior.AllowGet);
        }

        #region 微信返回userPhone
        public class WxPhone
        {
            public string phoneNumber { get; set; }
            public string purePhoneNumber { get; set; }
            public string countryCode { get; set; }
            public object watermark { get; set; }
        }
        #endregion
    }
}
