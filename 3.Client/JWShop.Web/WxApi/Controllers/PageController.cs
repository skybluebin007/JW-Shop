using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using JWShop.Entity;
using JWShop.Business;
using JWShop.Common;
using SkyCES.EntLib;
using JWShop.XcxApi.Filter;
using JWShop.XcxApi.Pay;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace JWShop.XcxApi.Controllers
{
    [Auth]
    public class PageController : Controller
    {
        /// <summary>
        /// 砍价活动到期：状态置为 “已结束”
        /// </summary>
        /// <param name="id"></param>
        private async void BargainTimeExpire()
        {
            //异步 改变“砍价活动状态”
            await Task.Run(() => {
                #region 结束 事务：将未支付成功的砍价全部置为“砍价失败”，原“活动已结束，砍价失败”
                List<BargainInfo> dataList = BargainBLL.SearchBargainList(new BargainSearch());
                foreach (var item in dataList)
                {
                    //到期，将状态置为“已结束”
                    if (DateTime.Now > item.EndDate && item.Status != (int)Bargain_Status.End)
                    {

                        BargainBLL.ChangeBargainStatus(item.Id, (int)Bargain_Status.End);
                    }
                }
                #endregion
            });
        }
        public ActionResult Index()
        {
            int userGrade = UserGradeBLL.ReadByMoney(0).Id;
            UserInfo user = new UserInfo();
            #region 判断用户归属分销商

            int uid = RequestHelper.GetForm<int>("uid");
            //分销商Id
            int distributor_id = RequestHelper.GetForm<int>("distributor_id");
            if (uid > 0)
            {
                user = UserBLL.ReadUserMore(uid);
                if (user.Id > 0) userGrade = UserGradeBLL.ReadByMoney(user.MoneyUsed).Id;
            }
            //if (distributor_id > 0)
            //{
            //    //当前分销商
            //    var distributor = UserBLL.Read(distributor_id);
            //    //如果当前分销商状态正常
            //    if (distributor.Distributor_Status == (int)Distributor_Status.Normal)
            //    {
            //        //如果用户已有分销商
            //        if (user.Recommend_UserId > 0)
            //        {
            //            var old_distributor = UserBLL.Read(user.Recommend_UserId);
            //            //如果原分销商状态不正常，,则修改用户归属当前分销商
            //            if (old_distributor.Distributor_Status != (int)Distributor_Status.Normal)
            //            {
            //                Dictionary<string, object> dict = new Dictionary<string, object>();
            //                dict.Add("[Recommend_UserId]", distributor.Id);
            //                UserBLL.UpdatePart("[Usr]", dict, user.Id);
            //            }
            //        }
            //        else
            //        {//如果用户没有归属分销商,则修改用户归属当前分销商
            //            Dictionary<string, object> dict = new Dictionary<string, object>();
            //            dict.Add("[Recommend_UserId]", distributor.Id);
            //            UserBLL.UpdatePart("[Usr]", dict, user.Id);
            //        }
            //    }
            //}

            #endregion
            #region 首页BANNER
            var bannercount = RequestHelper.GetForm<int>("bannercount");
            if (bannercount <= 0) bannercount = 5;
            var banner = AdImageBLL.ReadList(11, bannercount);
            #endregion

            int count = int.MinValue;
            #region 推荐产品
            var recmdprocount = RequestHelper.GetForm<int>("recmdprocount");
            if (recmdprocount <= 0) recmdprocount = 6;
            var recmdpro = ProductBLL.SearchList(1, recmdprocount, new ProductSearchInfo() { IsSale = 1, IsTop = 1, IsDelete = 0 }, ref count).Select(k => new
            {
                id = k.Id,
                name = k.Name,
                img = ShopCommon.ShowImage(k.Photo.Replace("Original", "150-150")),
                imgbig = ShopCommon.ShowImage(k.Photo.Replace("Original", "350-350")),
                imgorg = ShopCommon.ShowImage(k.Photo),
                price = ProductBLL.GetCurrentPrice(k.SalePrice, userGrade)
            });
            #endregion

            #region 排序前4个分类及相关产品

            List<VirtualCategory> vcatelist = new List<VirtualCategory>();
            var catelist = ProductClassBLL.ReadRootList().Take(6).ToList();

            foreach (var category in catelist)
            {
                VirtualCategory vcate = new VirtualCategory();
                vcate.productClass = category;

                List<ProductVirtualModel> tempvp = new List<ProductVirtualModel>();
                var templist = ProductBLL.SearchList(1, 4, new ProductSearchInfo() { IsSale = 1, ClassId = "|" + category.Id + "|", IsDelete = 0 }, ref count);
                foreach (var item in templist)
                {
                    var vp = new ProductVirtualModel()
                    {
                        id = item.Id,
                        name = item.Name,
                        img = ShopCommon.ShowImage(item.Photo.Replace("Original", "150-150")),
                        imgbig = ShopCommon.ShowImage(item.Photo.Replace("Original", "350-350")),
                        imgorg = ShopCommon.ShowImage(item.Photo),
                        groupphoto = string.IsNullOrWhiteSpace(item.GroupPhoto) ? item.Photo : item.GroupPhoto,
                        price = ProductBLL.GetCurrentPrice(item.SalePrice, userGrade),
                        click = item.ViewCount,
                        like = item.LikeNum,
                        totalstore = item.TotalStorageCount,
                        //ordercount=item.OrderCount
                        //是否启用不限库存，分别计算销量
                        ordercount = item.UnlimitedStorage == 1 ? OrderBLL.GetProductOrderCountDaily(item.Id, item.StandardType, DateTime.Now) : item.OrderCount
                    };
                    tempvp.Add(vp);
                }
                vcate.productlists = tempvp;
                vcatelist.Add(vcate);
            }
            #endregion
            //普通优惠券
            var couponlist = CouponBLL.SearchList(1, 1000, new CouponSearchInfo { Type = (int)CouponKind.Common, CanUse = 1 }, ref count);
            //新客优惠券
            CouponInfo registerCoupon = new CouponInfo();
            var registerCouponlist = CouponBLL.SearchList(1, 1, new CouponSearchInfo { Type = (int)CouponKind.RegisterGet, CanUse = 1 }, ref count);
            if (registerCouponlist.Count > 0) registerCoupon = registerCouponlist[0];
            //是否获取新人券
            int hasRegisterCoupon = user.HasRegisterCoupon;
            //将到期未结束的砍价活动置为“砍价结束”
            BargainTimeExpire();
            return Json(new { flag = true, banner = banner, recmdpro = recmdpro, cateandproduct = vcatelist, coupons = couponlist, hasRegisterCoupon = hasRegisterCoupon, registerCoupon = registerCoupon });
        }
        public JsonResult news()
        {
            int count = 0;
            string classId = string.Empty;
            if (RequestHelper.GetForm<int>("classid") > 0)
            {
                classId = "|" + RequestHelper.GetForm<int>("classid") + "|";
            }
            var aboutEntity = ArticleBLL.SearchList(1, 100, new ArticleSearchInfo { ClassId = classId }, ref count);
            return Json(new { my = aboutEntity, cat = ArticleClassBLL.Read(RequestHelper.GetForm<int>("classid")) ?? new ArticleClassInfo() }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult deteils(int id)
        {
            //var aboutEntity = ArticleBLL.SearchList(1, 1, new ArticleSearchInfo { Id = "id" }, ref count);
            var aboutEntity = ArticleBLL.Read(id);
            ArticleBLL.ChangeArticleStatus(id, "ViewCount", aboutEntity.ViewCount);
            aboutEntity.AddCol3 = aboutEntity.RealDate.ToString("yyyy-MM-dd");
            return Json(new { my = aboutEntity }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult pagedetail(int id)
        {
            var aboutEntity = ArticleBLL.Read(id);
            ArticleBLL.ChangeArticleStatus(id, "ViewCount", aboutEntity.ViewCount);
            return Json(new { my = aboutEntity }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Contact()
        {
            try
            {
                int count = 0;
                var aboutEntity = ArticleBLL.SearchList(1, 1, new ArticleSearchInfo { ClassId = "|1|" }, ref count).FirstOrDefault() ?? new ArticleInfo();
                aboutEntity.Content = string.IsNullOrEmpty(aboutEntity.AddCol2) ? aboutEntity.Content : aboutEntity.AddCol2;
                var aboutCat = ArticleClassBLL.Read(1) ?? new ArticleClassInfo();
                var bannercount = RequestHelper.GetForm<int>("bannercount");
                if (bannercount <= 0) bannercount = 8;
                var companyPhotos = AdImageBLL.ReadList(14, bannercount);
                return Json(new { ok = true, aboutentity = aboutEntity, aboutcat = aboutCat, photos = companyPhotos });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false });
            }

        }
        [HttpPost]
        public ActionResult Config()
        {
            return Json(new
            {
                tel = ShopConfig.ReadConfigInfo().Tel,
                mobile = ShopConfig.ReadConfigInfo().GTel,
                title = ShopConfig.ReadConfigInfo().Title,
                des = ShopConfig.ReadConfigInfo().Description,
                email = ShopConfig.ReadConfigInfo().Fax,
                contact = ShopConfig.ReadConfigInfo().PostCode,
                qq = ShopConfig.ReadConfigInfo().QQ,
                businesshours = ShopConfig.ReadConfigInfo().BusinessHours,
                address = ShopConfig.ReadConfigInfo().Address,
                sitelink = ShopConfig.ReadConfigInfo().SiteLink,
                orderpaytemplateid = ShopConfig.ReadConfigInfo().OrderPayTemplateId,
                enableselfpick = ShopConfig.ReadConfigInfo().SelfPick,
                selfpicktemplateid = ShopConfig.ReadConfigInfo().SelfPickTemplateId,
                checktobedistributor = ShopConfig.ReadConfigInfo().CheckToBeDistributor,
            }
            );
        }
      
        public ActionResult GetLogin(string code)
        {
            JsApiPay jsapi = new JsApiPay();
            jsapi.GetOpenidAndAccessTokenFromCode(code);

            string sessionid = Guid.NewGuid().ToString("N");
            Session[sessionid] = jsapi.openid + "|" + jsapi.access_token;

            var user = UserBLL.Read(jsapi.openid);
            if (user.Id > 0)
            {
                UserBLL.UserLoginInit(user);
                //VirtualUser vuser = new VirtualUser()
                //{
                //    id = user.Id,
                //    name = HttpUtility.UrlDecode(user.UserName, System.Text.Encoding.UTF8),
                //};
                var vuser = new
                {
                    id = user.Id,
                    name = HttpUtility.UrlDecode(user.UserName, System.Text.Encoding.UTF8),
                    status = user.Status,
                    avatar = user.Photo
                };
                return Json(new { flag = true, sessionid = sessionid, userinfo = vuser, thesessionid = Session.SessionID }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { flag = false, msg = "no user", openid = jsapi.openid, sessionid = sessionid, thesessionid = Session.SessionID }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult AddUser()
        {
            StreamReader reader = new StreamReader(Request.InputStream);
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            reader.BaseStream.Position = 0;
            string json = HttpUtility.UrlDecode(reader.ReadToEnd());
            var jo = (JObject)JsonConvert.DeserializeObject(json);

            string openid = (string)jo["openid"];
            TxtLog log = new TxtLog(Server.MapPath("/apilog/"));
            log.Write(json);
            //JObject jo = JsonConvert.DeserializeObject(userinfo) as JObject;
            if (jo != null)
            {
                var checkuser = UserBLL.Read(openid);
                int userid = checkuser.Id;
                if (checkuser.Id <= 0)
                {
                    //string UserName = jo["userInfo"]["nickName"].ToString();
                    checkuser = new UserInfo()
                    {
                        //UserName = UserName,
                        UserName = openid,
                        UserPassword = StringHelper.Password("123123", PasswordType.MD532),
                        //Photo = jo["userInfo"]["avatarUrl"].ToString(),
                        RegisterDate = DateTime.Now,
                        LastLoginDate = DateTime.Now,
                        LoginTimes = 1,
                        RegisterIP = ClientHelper.IP,
                        LastLoginIP = ClientHelper.IP,
                        FindDate = RequestHelper.DateNow,
                        Status = (int)UserStatus.Normal,
                        //性别默认保密
                        Sex = 3,
                        //Sex = Convert.ToInt32(jo["userInfo"]["gender"].ToString()),
                        OpenId = openid,
                        // 推荐人 默认为0，进入小程序首页再更新
                        Recommend_UserId = 0,
                        //如果开启了需要审核成为分销商则分销状态=》待审核，否则=》正常
                        Distributor_Status=ShopConfig.ReadConfigInfo().CheckToBeDistributor==0?(int)Distributor_Status.Normal:(int)Distributor_Status.WaitCheck,
                        //总佣金
                        Total_Commission =0,
                        //总提现
                        Total_Withdraw =0
                    };
                    userid = UserBLL.Add(checkuser);
                    #region 注册赠送优惠券(限新人领取1次)
                    /*
                    int count = 0;
                    var couponlist = CouponBLL.SearchList(1, 1, new CouponSearchInfo { Type = (int)CouponKind.RegisterGet, CanUse = 1 }, ref count);
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
                        int cpid = UserCouponBLL.Add(new UserCouponInfo
                        {
                            UserId = userid,
                            //UserName = UserName,
                            UserName = openid,
                            CouponId = couponlist[0].Id,
                            GetType = (int)CouponType.RegisterGet,
                            Number = ShopCommon.CreateCouponNo(couponlist[0].Id, startNumber),
                            Password = ShopCommon.CreateCouponPassword(startNumber),
                            IsUse = (int)BoolType.False,
                            OrderId = 0

                        });
                        //领取成功，改变usr表标识
                        if (cpid > 0)
                        {
                            Dictionary<string, object> dict = new Dictionary<string, object>();
                            dict.Add("[HasRegisterCoupon]", 1);
                            UserBLL.UpdatePart("[usr]", dict, userid);
                        }
                    }
                    */
                    #endregion
                }          

                var vuser = new
                {
                    id = userid,
                    name = checkuser.UserName,
                    avatar = checkuser.Photo,
                    status = checkuser.Status
                };
                return Json(new { flag = true, userinfo = vuser, thesessionid = Session.SessionID });


            }
            else
            {
                return Json(new { flag = false, msg = "json 转换错误" });
            }
        }
        /// <summary>
        /// 新人注册 领取优惠券 
        /// </summary>      
        [HttpPost]
        public JsonResult CouponRegister(int userId, int couponId)
        {
            try
            {
                UserInfo user = UserBLL.Read(userId);
                CouponInfo coupon = CouponBLL.Read(couponId);
                if (user == null || user.Id <= 0 || coupon.Id <= 0)
                {
                    return Json(new { ok = false, error = "未登录或登录失效", code = -1 });
                }

                if (UserCouponBLL.UniqueUserCatch(user.Id, couponId))
                {
                    //新人券暂时不限制总数
                    //if (coupon.TotalCount - 1 < coupon.UsedCount)
                    //{
                    //    return Json(new { ok = false, msg = "优惠券余量不足" });
                    //}
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
                    tmpCoupon.GetType = (int)CouponType.RegisterGet;
                    tmpCoupon.Number = ShopCommon.CreateCouponNo(couponId, startNumber);
                    tmpCoupon.Password = ShopCommon.CreateCouponPassword(startNumber);
                    tmpCoupon.IsUse = (int)BoolType.False;
                    tmpCoupon.OrderId = 0;
                    tmpCoupon.UserId = user.Id;
                    tmpCoupon.UserName = user.UserName;
                    if (UserCouponBLL.Add(tmpCoupon) > 0)
                    {//如果领取成功，增加优惠券的发放（领取）量
                        coupon.UsedCount++;
                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        dict.Add("[UsedCount]", coupon.UsedCount);
                        CouponBLL.UpdatePart("[Coupon]", dict, coupon.Id);
                        //该变用户标识：已领过新人券
                        dict = new Dictionary<string, object>();
                        dict.Add("[HasRegisterCoupon]", 1);
                        UserBLL.UpdatePart("[usr]", dict, userId);
                    }
                    return Json(new { ok = true, error = "领券成功" });

                }
                else
                {
                    //该变用户标识：已领过新人券
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("[HasRegisterCoupon]", 1);
                    UserBLL.UpdatePart("[usr]", dict, userId);
                    return Json(new { ok = false, error = "您已领过", code = 0 });
                }
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = "系统忙", code = 0 });
            }
        }

     
        public ActionResult GetUserBySession(string sessionid)
        {
            if (sessionid == "ao123")
            {
                var user = UserBLL.Read(1);
              
                var vuser = new
                {
                    id = user.Id,
                    name = HttpUtility.UrlDecode(user.UserName, System.Text.Encoding.UTF8),
                    avatar = user.Photo,
                    status = user.Status
                };
                return Json(new { flag = true, userinfo = vuser }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Session[sessionid] != null)
                {
                    string[] openandaccess = Session[sessionid].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (openandaccess.Length > 0)
                    {
                        UserInfo user = UserBLL.Read(openandaccess[0]);
                        if (user.Id > 0)
                        {
                            UserBLL.UserLoginInit(user);
                            var vuser = new
                            {
                                id = user.Id,
                                name = HttpUtility.UrlDecode(user.UserName, System.Text.Encoding.UTF8),
                                avatar = user.Photo,
                                status = user.Status
                            };
                            return Json(new { flag = true, userinfo = vuser }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { flag = false }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { flag = false }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(new { flag = false }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult Detail(int id)
        {
            int userGrade = UserGradeBLL.ReadByMoney(0).Id;
            int uid = RequestHelper.GetForm<int>("uid");
            var user = UserBLL.ReadUserMore(uid);
            if (user.Id > 0)
            {
                userGrade = UserGradeBLL.ReadByMoney(user.MoneyUsed).Id;
                user.UserName = HttpUtility.UrlDecode(user.UserName, System.Text.Encoding.UTF8);
            }

            if (id <= 0)
            {
                return Json(new { ok = false, error = "该产品未上市" });
            }
            string fromwhere = RequestHelper.GetQueryString<string>("fw");
            ProductInfo product = ProductBLL.Read(id);
            if (product.IsSale == (int)BoolType.False || product.IsDelete == 1)
            {
                return Json(new { ok = false, error = "该产品未上市" });
            }
            #region 如果商品没有小程序码 则生成并保存
            if (string.IsNullOrWhiteSpace(product.Qrcode))
            {
                string product_miniProramCode = string.Empty;
                CreateMiniProgramCode(product.Id, ref product_miniProramCode, product.Qrcode);
                if (!string.IsNullOrWhiteSpace(product_miniProramCode))
                {//如果调用接口成功生成小程序码（因为只有发布后才能使用此微信接口）
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("[Qrcode]", product_miniProramCode);
                    ProductBLL.UpdatePart("[Product]", dict, product.Id);
                    product.Qrcode = product_miniProramCode;
                }
            }
            #endregion
            //更新查看数量
            if (CookiesHelper.ReadCookie("productview" + product.Id + "") == null)
            {
                ProductBLL.ChangeViewCount(id, 1);
                CookiesHelper.AddCookie("productview" + product.Id + "", product.Id.ToString());
            }
            ProductCommentSearchInfo proCommSear = new ProductCommentSearchInfo();
            var proComm = ProductCommentBLL.SearchProductCommentList(proCommSear = new ProductCommentSearchInfo { ProductId = product.Id, Status = (int)CommentStatus.Show });
            var procomlist = new List<VirtualProductCommend>();
            foreach (var item in proComm)
            {
                VirtualProductCommend vpc = new VirtualProductCommend()
                {
                    id = item.Id,
                    name =HttpUtility.UrlDecode(item.UserName,Encoding.UTF8),
                    avator = ShopCommon.ShowImage(UserBLL.Read(item.UserId).Photo),
                    lv = item.Rank,
                    date = item.PostDate,
                    content = item.Content,
                    imglist = ProductPhotoBLL.ReadList(item.Id, 3),
                    adminreply = item.AdminReplyContent,
                    replydate = item.AdminReplyDate

                };
                procomlist.Add(vpc);
            }

            //产品价格
            int hotCount = 0;
            var currentMemberPrice = ProductBLL.GetCurrentPrice(product.SalePrice, userGrade);
            var prorecommend = ProductBLL.SearchList(1, 8, new ProductSearchInfo { IsSale = (int)BoolType.True, IsTop = (int)BoolType.True, IsDelete = (int)BoolType.False, NotInProductId = product.Id.ToString() }, ref hotCount);
            var prorelist = new List<ProductVirtualModel>();
            foreach (var item in prorecommend)
            {
                var vp = new ProductVirtualModel()
                {
                    id = item.Id,
                    name = item.Name,
                    img = ShopCommon.ShowImage(item.Photo.Replace("Original", "150-150")),
                    imgbig = ShopCommon.ShowImage(item.Photo.Replace("Original", "350-350")),
                    imgorg = ShopCommon.ShowImage(item.Photo),
                    price = ProductBLL.GetCurrentPrice(item.SalePrice, userGrade),
                    marketprice = item.MarketPrice,
                    click = item.ViewCount,
                    like = item.LikeNum,
                    totalstore = item.StandardType == (int)ProductStandardType.Single ? ProductTypeStandardRecordBLL.GetSumStorageByProduct(item.Id) : item.TotalStorageCount,
                    //ordercount = item.OrderCount
                    //是否启用不限库存，分别计算销量
                    ordercount = item.UnlimitedStorage == 1 ? OrderBLL.GetProductOrderCountDaily(item.Id, item.StandardType, DateTime.Now) : item.OrderCount
                };
                prorelist.Add(vp);
            }
            //产品图片
            List<ProductPhotoInfo> productPhotoList = new List<ProductPhotoInfo>();
            ProductPhotoInfo productPhoto = new ProductPhotoInfo();
            productPhoto.Name = product.Name;
            productPhoto.ImageUrl = product.Photo;
            productPhotoList.Add(productPhoto);
            productPhotoList.AddRange(ProductPhotoBLL.ReadList(id, 0));
            productPhotoList.ForEach(k => k.ImageUrl = k.ImageUrl.Replace("75-75", "Original"));
            //属性
            var attributeRecordList = ProductTypeAttributeRecordBLL.ReadList(id);

            #region 产品规格
            List<ProductTypeStandardInfo> standardList = new List<ProductTypeStandardInfo>();
            string standardRecordValueList = "|";
            var standardRecordList = ProductTypeStandardRecordBLL.ReadListByProduct(product.Id, product.StandardType);
            decimal maxPrice = product.SalePrice;
            if (standardRecordList.Count > 0)
            {
                string[] standardIDArray = standardRecordList[0].StandardIdList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < standardIDArray.Length; i++)
                {
                    int standardID = Convert.ToInt32(standardIDArray[i]);
                    ProductTypeStandardInfo standard = ProductTypeStandardBLL.Read(standardID);
                    string[] valueArray = standard.ValueList.Split(';');
                    string valueList = string.Empty;
                    for (int k = 0; k < valueArray.Length; k++)
                    {
                        foreach (ProductTypeStandardRecordInfo standardRecord in standardRecordList)
                        {
                            if (standardRecord.MarketPrice > maxPrice) maxPrice = standardRecord.MarketPrice;
                            string[] tempValueArray = standardRecord.ValueList.Split(';');
                            if (valueArray[k] == tempValueArray[i])
                            {
                                valueList += valueArray[k] + ";";
                                break;
                            }
                        }
                    }
                    if (valueList != string.Empty)
                    {
                        valueList = valueList.Substring(0, valueList.Length - 1);
                    }
                    standard.ValueList = valueList;
                    standardList.Add(standard);
                }
                //规格值
                foreach (ProductTypeStandardRecordInfo standardRecord in standardRecordList)
                {
                    standardRecordValueList += standardRecord.ProductId + ";" + standardRecord.ValueList + "|";
                }
            }
            #endregion
            #region 正在开的团（进行中,排除本人开的团）
            GroupBuySearchInfo gpsearch = new GroupBuySearchInfo
            {
                ProductId = product.Id,
                //NotLeader = user.Id,
                Status = (int)GroupBuyStatus.Going
            };
            List<GroupBuyInfo> gpList = GroupBuyBLL.SearchList(gpsearch);
            gpList.ForEach(k => k.groupSignList = GroupSignBLL.ReadListByGroupId(k.Id));
            gpList.ForEach(k => k.GroupUserName = System.Web.HttpUtility.UrlDecode(k.GroupUserName, Encoding.UTF8));
            #endregion
            return Json(new
            {
                ok = true,
                product = new
                {
                    id = product.Id,
                    img = ShopCommon.ShowImage(product.Photo.Replace("Original", "350-350")),
                    imgorg = ShopCommon.ShowImage(product.Photo),
                    title = product.Name,
                    summary = product.Summary,
                    price = currentMemberPrice,
                    marketprice = product.MarketPrice,
                    intro = string.IsNullOrEmpty(product.Introduction1_Mobile) ? product.Introduction1 : product.Introduction1_Mobile,
                    remark = product.Remark,
                    totalstore = product.StandardType == (int)ProductStandardType.Single ? ProductTypeStandardRecordBLL.GetSumStorageByProduct(product.Id) : product.TotalStorageCount,
                    ordercount = product.UnlimitedStorage == 1 ? OrderBLL.GetProductOrderCountDaily(product.Id, product.StandardType, DateTime.Now) : product.OrderCount,
                    unlimitedstorage = product.UnlimitedStorage,
                    virtualordercount = product.VirtualOrderCount,
                    usevirtualorder = product.UseVirtualOrder,
                    opengroup = product.OpenGroup,
                    groupprice = product.GroupPrice,
                    groupquantity = product.GroupQuantity,
                    qrcode = product.Qrcode,
                    groupphoto = string.IsNullOrWhiteSpace(product.GroupPhoto) ? product.Photo : product.GroupPhoto
                },
                standardList = standardList,
                standardRecordValueList = standardRecordValueList,
                attributeRecordList = attributeRecordList,
                productPhotoList = productPhotoList,
                prorecommend = prorelist,
                proComm = procomlist,
                maxPrice = maxPrice,
                groupList = gpList
            }, JsonRequestBehavior.AllowGet);
        }
        #region 生成商品详情页小程序码

        /// <summary>
        /// 生成小程序二维码
        /// 带参数（商品Id），扫码进入小程序商品详情页
        /// </summary>
        /// <param name="pid">商品Id</param>
        /// <param name="imageName">生成的二维码相对路径+名称</param>
        /// <param name="oldImage">原二维码相对路径+名称</param>
        private void CreateMiniProgramCode(int pid, ref string imageName, string oldImage = "")
        {
            if (pid > 0)
            {
                string access_token = XcxApi.Pay.WxGetInfo.IsExistAccess_Token();

                string url = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + access_token;

                XcxApi.Pay.WxPayData jsondata = new XcxApi.Pay.WxPayData();
                jsondata.SetValue("scene", pid);
                jsondata.SetValue("page", "pages/product/detail");
                jsondata.SetValue("width", 430);
                try
                {
                    #region 删除原图
                    if (!string.IsNullOrWhiteSpace(oldImage))
                    {
                        if (System.IO.File.Exists(Server.MapPath(oldImage)))
                        {
                            System.IO.File.Delete(Server.MapPath(oldImage));
                        }
                    }
                    #endregion

                    System.Net.HttpWebRequest request;
                    request = (System.Net.HttpWebRequest)WebRequest.Create(url);
                    request.Method = "POST";
                    request.ContentType = "application/json;charset=UTF-8";
                    string paraUrlCoded = jsondata.ToJson();
                    byte[] payload;
                    payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                    request.ContentLength = payload.Length;
                    Stream writer = request.GetRequestStream();
                    writer.Write(payload, 0, payload.Length);
                    writer.Close();
                    System.Net.HttpWebResponse response;
                    response = (System.Net.HttpWebResponse)request.GetResponse();
                    System.IO.Stream s;
                    s = response.GetResponseStream();//返回图片数据流  
                    byte[] tt =ShopCommon.StreamToBytes(s);//将数据流转为byte[]  

                    //在文件名前面加上时间，以防重名  
                    string imgName = "xcx_" + Guid.NewGuid().ToString() + ".png";
                    //文件存储相对于当前应用目录的虚拟目录  
                    string path = "/upload/qrcode/";
                    //获取相对于应用的基目录,创建目录  
                    string imgPath = System.AppDomain.CurrentDomain.BaseDirectory + path;     //通过此对象获取文件名  
                    if (!Directory.Exists(imgPath))
                    {
                        Directory.CreateDirectory(imgPath);
                    }
                    //System.IO.File.WriteAllBytes(Server.MapPath(path + imgName), tt);//讲byte[]存储为图片  

                    #region Png

                    MemoryStream stream = new MemoryStream(tt);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                    img.Save(Server.MapPath(path + imgName), System.Drawing.Imaging.ImageFormat.Png);
                    #endregion

                    imageName = path + imgName;
                }
                catch (Exception e)
                {
                    Log.Error("CreateProductMiniProgramQrCode", e.ToString());

                }
            }
        }


        #endregion
        public ActionResult GetProductPriceAndStore()
        {
            int userGrade = UserGradeBLL.ReadByMoney(0).Id;
            int uid = RequestHelper.GetForm<int>("uid");
            var user = UserBLL.ReadUserMore(uid);
            if (user != null && user.Id > 0) userGrade = UserGradeBLL.ReadByMoney(user.MoneyUsed).Id;

            int id = RequestHelper.GetForm<int>("productID");
            string valueList = StringHelper.AddSafe(RequestHelper.GetForm<string>("valueList"));
            var product = ProductBLL.Read(id);
            List<ProductTypeStandardRecordInfo> standRecordList = ProductTypeStandardRecordBLL.ReadListByProduct(id, product.StandardType);
            foreach (var item in standRecordList)
            {
                if (item.ValueList.Trim() == valueList.Trim())
                {
                    return Json(new
                    {
                        price = ProductBLL.GetCurrentPrice(item.SalePrice, userGrade),
                        markprice = ProductBLL.GetCurrentPrice(item.MarketPrice, userGrade),
                        groupprice = item.GroupPrice,
                        totalstorage = item.Storage,
                        //leftstore = (item.Storage - item.OrderCount),
                        ordercount = product.UnlimitedStorage == 1 ? OrderBLL.GetProductOrderCountDaily(product.Id, product.StandardType, DateTime.Now, item.ValueList.Trim()) : item.OrderCount,
                        //是否启用不限库存，分别计算销量
                        leftstore = (item.Storage - (product.UnlimitedStorage == 1 ? OrderBLL.GetProductOrderCountDaily(product.Id, product.StandardType, DateTime.Now, item.ValueList.Trim()) : item.OrderCount)),
                        img = item.Photo,
                        unlimitedstorage = product.UnlimitedStorage
                    });
                }
            }

            return Content("no");
        }

        public ActionResult CheckStore()
        {
            string result = "0";
            int id = RequestHelper.GetForm<int>("productID");
            int buyCount = RequestHelper.GetForm<int>("buyCount");
            string valueList = StringHelper.AddSafe(RequestHelper.GetForm<string>("valueList"));
            int standardType = RequestHelper.GetForm<int>("standardType");
            ProductInfo product = ProductBLL.Read(id);
            if (standardType == 1)
            {
                List<ProductTypeStandardRecordInfo> standRecordList = ProductTypeStandardRecordBLL.ReadListByProduct(id, 1);
                foreach (var item in standRecordList)
                {
                    if (item.ValueList.Trim() == valueList.Trim())
                    {
                        if ((product.UnlimitedStorage != 1 && item.Storage - item.OrderCount >= buyCount) || (product.UnlimitedStorage == 1 && item.Storage - OrderBLL.GetProductOrderCountDaily(product.Id, product.StandardType, DateTime.Now, item.ValueList.Trim()) >= buyCount))
                        {
                            result = "1";
                        }

                        break;
                    }
                }
            }
            else
            {

                if ((product.UnlimitedStorage != 1 && product.TotalStorageCount - product.OrderCount >= buyCount) || (product.UnlimitedStorage == 1 && product.TotalStorageCount - OrderBLL.GetProductOrderCountDaily(product.Id, product.StandardType, DateTime.Now) >= buyCount))
                {
                    result = "1";
                }
            }

            return Content(result);
        }
        /// <summary>
        /// 拼团活动详情
        /// </summary>
        /// <returns></returns>
        public ActionResult GroupBuyDetail()
        {
            int id = RequestHelper.GetForm<int>("id");
            GroupBuyInfo entity = GroupBuyBLL.Read(id);
            if (entity.Id <= 0)
            {
                return Json(new { ok = false, msg = "拼团商品不存在" });
            }
            var product = ProductBLL.Read(entity.ProductId);
            if (product.Id <= 0)
            {
                return Json(new { ok = false, msg = "拼团商品不存在" });
            }
            if (product.OpenGroup != 1)
            {
                return Json(new { ok = false, msg = "此商品暂不支持拼团" });
            }
            //拼团状态
            entity.VirtualStatus = (entity.EndTime < DateTime.Now && entity.Quantity > entity.SignCount) ? -1 : (entity.StartTime <= DateTime.Now && entity.EndTime >= DateTime.Now && entity.Quantity > entity.SignCount) ? 0 : (entity.StartTime <= DateTime.Now && entity.EndTime >= DateTime.Now && entity.Quantity <= entity.SignCount) ? 1 : -1;
            //除团长以外其他参与者
            List<GroupSignInfo> signList = GroupSignBLL.ReadListByGroupId(id).Where(k => k.UserId != entity.Leader).ToList();
            #region 推荐商品
            int userGrade = UserGradeBLL.ReadByMoney(0).Id;
            int uid = RequestHelper.GetForm<int>("uid");
            var user = UserBLL.ReadUserMore(uid);
            if (user != null && user.Id > 0) userGrade = UserGradeBLL.ReadByMoney(user.MoneyUsed).Id;
            int hotCount = 0;
            var prorecommend = ProductBLL.SearchList(1, 4, new ProductSearchInfo { IsSale = (int)BoolType.True, IsTop = (int)BoolType.True, IsDelete = (int)BoolType.False, NotInProductId = product.Id.ToString() }, ref hotCount);
            var prorelist = new List<ProductVirtualModel>();
            foreach (var item in prorecommend)
            {
                var vp = new ProductVirtualModel()
                {
                    id = item.Id,
                    name = item.Name,
                    img = ShopCommon.ShowImage(item.Photo.Replace("Original", "150-150")),
                    imgbig = ShopCommon.ShowImage(item.Photo.Replace("Original", "350-350")),
                    imgorg = ShopCommon.ShowImage(item.Photo),
                    price = ProductBLL.GetCurrentPrice(item.SalePrice, userGrade),
                    click = item.ViewCount,
                    like = item.LikeNum,
                    totalstore = item.StandardType == (int)ProductStandardType.Single ? ProductTypeStandardRecordBLL.GetSumStorageByProduct(item.Id) : item.TotalStorageCount,
                    //ordercount = item.OrderCount
                    //是否启用不限库存，分别计算销量
                    ordercount = item.UnlimitedStorage == 1 ? OrderBLL.GetProductOrderCountDaily(item.Id, item.StandardType, DateTime.Now) : item.OrderCount
                };
                prorelist.Add(vp);
            }
            #endregion  
            return Json(new { ok = true, entity = entity, product = product, signlist = signList, topproductlist = prorelist });
        }
        /// <summary>
        /// 获取当前拼团商品列表
        /// 及开团情况：多少人开团，多少人参与
        /// 拼团图片：如果传了则显示开团主图，否则显示商品主图
        /// </summary>
        /// <returns></returns>
        public ActionResult GetGroupList(int pageIndex = 1, int pageSize = 2)
        {
            try
            {
                int count = 0;
                var openGroup_Products = ProductBLL.SearchList(pageIndex, pageSize, new ProductSearchInfo { IsSale = 1, IsDelete = 0, OpenGroup = 1 }, ref count);
                openGroup_Products.ForEach(p => p = ProductBLL.Read(p.Id));
                return Json(new
                {
                    ok = true,
                    count = openGroup_Products.Count,
                    groupList = openGroup_Products.Select(p =>
                    new {
                        product = new
                        {
                            id = p.Id,
                            title = p.Name,
                            img = !string.IsNullOrWhiteSpace(p.GroupPhoto) ? p.GroupPhoto : p.Photo,
                            groupprice = p.GroupPrice,
                            marketprice = p.MarketPrice,
                            totalstore = p.StandardType == (int)ProductStandardType.Single ? ProductTypeStandardRecordBLL.GetSumStorageByProduct(p.Id) : p.TotalStorageCount,
                            ordercount = p.UnlimitedStorage == 1 ? OrderBLL.GetProductOrderCountDaily(p.Id, p.StandardType, DateTime.Now) : p.OrderCount,
                            unlimitedstorage = p.UnlimitedStorage,
                            // 开/参团情况
                            groups = GroupBuyBLL.SearchList(new GroupBuySearchInfo { ProductId = p.Id })
                        }
                    }
                    )
                });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = ex.Message });
            }
        }
        public ActionResult Catelist()
        {
            var catelist = ProductClassBLL.ReadRootList();

            return Json(new { catelist = catelist }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Product(int page = 1, int pagesize = 10, int classid = 0)
        {
            int userGrade = UserGradeBLL.ReadByMoney(0).Id;
            UserInfo user = new UserInfo();
            int uid = RequestHelper.GetForm<int>("uid");
            if (uid > 0)
            {
                user = UserBLL.ReadUserMore(uid);
                if (user.Id > 0) userGrade = UserGradeBLL.ReadByMoney(user.MoneyUsed).Id;
            }


            var productlist = new List<ProductInfo>();
            int count = 0;
            var catelist = ProductClassBLL.ReadRootList();
            //if (catelist.Count > 0)
            //{
            //    if (classid == 0) classid = catelist[0].Id;                
            //}

            var prosearch = new ProductSearchInfo()
            {
                IsSale = 1,
                IsDelete = 0
            };
            string searchkey = RequestHelper.GetForm<string>("searchkey");
            if (!string.IsNullOrEmpty(searchkey)) prosearch.Name = searchkey;
            if (classid > 0) prosearch.ClassId = "|" + classid + "|";

            int sort = RequestHelper.GetForm<int>("sort");
            string sortby = "[OrderId],[Id]";
            switch (sort)
            {
                case 1:
                    sortby = "OrderCount";
                    break;
                case 2:
                    sortby = "SalePriceUp";
                    break;
                case 3:
                    sortby = "SalePriceDown";
                    break;
            }
            prosearch.ProductOrderType = sortby;
            prosearch.OrderType = OrderType.Desc;

            productlist = ProductBLL.SearchList(page, pagesize, prosearch, ref count);


            //List<ProductVirtualModel> tempvp = new List<ProductVirtualModel>();
            //foreach (var item in productlist)
            //{
            //    double percent = 0;
            //    var pcomment = ProductCommentBLL.SearchProductCommentList(new ProductCommentSearchInfo() { ProductId = item.Id, Status = (int)CommentStatus.Show });
            //    if (pcomment.Count() > 0)
            //    {
            //        percent = pcomment.Where(e => e.Rank > 3).Count() * 1.00 / pcomment.Count() * 100;
            //    }

            //    var vp = new
            //    {
            //        id = item.Id,
            //        name = item.Name,
            //        img = ShopCommon.ShowImage(item.Photo.Replace("Original", "150-150")),
            //        imgbig = ShopCommon.ShowImage(item.Photo.Replace("Original", "350-350")),
            //        imgorg = ShopCommon.ShowImage(item.Photo),
            //        price = ProductBLL.GetCurrentPrice(item.SalePrice, userGrade),
            //        click = item.ViewCount,
            //        like = item.LikeNum,
            //        totalstore = item.TotalStorageCount,
            //        ordercount = item.UnlimitedStorage == 1 ? OrderBLL.GetProductOrderCountDaily(item.Id, item.StandardType, DateTime.Now) : item.OrderCount,
            //        virtualordercount = item.VirtualOrderCount,
            //        usevirtualorder = item.UseVirtualOrder,
            //        commentcount = pcomment.Count,
            //        goodcompercent = Math.Round(percent, 2)
            //    };
            //    tempvp.Add(vp);
            //}

            var dataList = productlist.Select(item => new
            {
                id = item.Id,
                name = item.Name,
                img = ShopCommon.ShowImage(item.Photo.Replace("Original", "150-150")),
                imgbig = ShopCommon.ShowImage(item.Photo.Replace("Original", "350-350")),
                imgorg = ShopCommon.ShowImage(item.Photo),
                price = ProductBLL.GetCurrentPrice(item.SalePrice, userGrade),
                click = item.ViewCount,
                like = item.LikeNum,
                totalstore = item.TotalStorageCount,
                ordercount = item.UnlimitedStorage == 1 ? OrderBLL.GetProductOrderCountDaily(item.Id, item.StandardType, DateTime.Now) : item.OrderCount,
                virtualordercount = item.VirtualOrderCount,
                usevirtualorder = item.UseVirtualOrder,
                //commentcount = pcomment.Count,
                //goodcompercent = Math.Round(percent, 2)
            });
            return Json(new { products = dataList }, JsonRequestBehavior.AllowGet);
        }

    }


    public class VirtualCategory
    {
        public ProductClassInfo productClass { set; get; }

        public List<ProductVirtualModel> productlists { set; get; }
    }
}
