using JWShop.Business;
using JWShop.XcxApi.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JWShop.Entity;
using SkyCES.EntLib;
using JWShop.Common;
using JWShop.XcxApi.Pay;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace JWShop.XcxApi.Controllers
{
    [Auth]
    public class BargainController : Controller
    {
    
        /// <summary>
        /// 加载活动列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            try
            {
                //读取正在进行的，且状态是有效的 所有活动,
                var BargainList = BargainBLL.View_ReadBargain();
                var BargainDetails = new List<BargainDetailsInfo>();
                //var ProductClassList = ProductClassBLL.ReadList();
                //读取所有活动下的商品
                foreach (var item in BargainList)
                {                   
                    //选择剩余库存大于0的（库存-已砍价成功支付下单>0）
                    var list = BargainDetailsBLL.ReadByBargainId(item.Id).Where(k => k.Stock-k.Sales > 0).ToList();
                    //加载虚拟人数
                    //list.ForEach(k => { k.Sales += item.NumberPeople; });
                    list.ForEach(k => k.Virtual_Sales = item.NumberPeople);
                    //加载开始 结束日期
                    list.ForEach(k => { k.StartDate = item.StartDate; k.EndDate = item.EndDate; });
                    //获取砍价用户头像 昵称
                    list.ForEach(k => k.BargainUserList = BargainDetailsBLL.GetBargainUsers(k.Id));
                    #region 本次活动共砍次数
                    int total_bargain = 0;                  
                    foreach (var bdt in list)
                    {
                        var bargain_orders = BargainOrderBLL.SearchBargainOrderList(new BargainOrderSearch { BargainDetailsId = bdt.Id });
                        foreach (var bo in bargain_orders)
                        {
                            total_bargain += RecordingBLL.SearchRecordingList(new RecordingSearch { BOrderId = bo.Id }).Where(k=>k.UserId>0).Count();
                        }
                        //本次砍价总砍次数
                        bdt.Bargain_Records_Total = total_bargain;
                    }                 
                    #endregion
                    BargainDetails.AddRange(list);
                }
                BargainDetails = BargainDetails.OrderByDescending(k => k.BargainId).ThenByDescending(k => k.Id).ThenByDescending(k => k.Sort).ToList();
                var json = new List<BargainDetail>();
                #region old method
                //var ProductList = ProductBLL.ReadList();
                //foreach (var item in ProductList)
                //{
                //    #region 加载商品的分类
                //    int classId = 0;
                //    try
                //    {
                //        classId = int.Parse(item.ClassId.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault());
                //    }
                //    catch (Exception)
                //    {
                //        classId = 0;
                //        throw;
                //    }
                //    if (classId > 0)
                //    {
                //        var proClass = _ProductClassList.Where(k => k.Id == classId).SingleOrDefault() ?? new ProductClassInfo();
                //        if (proClass.Id > 0)
                //        {
                //            item.Name = string.Format("【{0}】", proClass.Name) + item.Name;
                //        }
                //    }
                //    #endregion

                //}
                //var ProductClassList = ProductClassBLL.ReadList();


                ////将活动详情商品添加到Json集合


                //foreach (var item in BargainDetails)
                //{
                //    //var product = ProductList.Where(k => k.Id == item.ProductID).SingleOrDefault() ?? new ProductInfo();
                //    var product = ProductBLL.Read(item.ProductID);
                //    //上架 未删除
                //    if (product.Id > 0 && product.IsDelete == 0 && product.IsSale==1)
                //    {
                //        int classId = 0;
                //        try
                //        {
                //            classId = int.Parse(product.ClassId.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault());
                //        }
                //        catch (Exception)
                //        {
                //            classId = 0;
                //            throw;
                //        }
                //        var proClass = ProductClassList.FirstOrDefault(k => k.Id == classId) ?? new ProductClassInfo();
                //        product.Photo = ShopCommon.ShowImage(product.Photo.Replace("Original", "350-350"));
                //        product.Name = string.Format("【{0}】", proClass.Name) + product.Name;
                //        json.Add(new BargainDetail()
                //        {
                //            Bargain = item,
                //            Product = product
                //        });
                //    }
                //}

                #endregion
                var productIds = BargainDetails.Select(k => k.ProductID).ToArray();
                var products = ProductBLL.SearchList(new ProductSearchInfo { IsSale = 1, IsDelete = 0, InProductId = string.Join(",", productIds) });
                //foreach(var product in products)
                //{
                //    int classId = 0;
                //    try
                //    {
                //        classId = int.Parse(product.ClassId.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault());
                //    }
                //    catch (Exception)
                //    {
                //        classId = 0;
                //        throw;
                //    }
                //    var proClass = ProductClassList.FirstOrDefault(k => k.Id == classId) ?? new ProductClassInfo();
                //    product.Photo = ShopCommon.ShowImage(product.Photo.Replace("Original", "350-350"));
                //    product.Name = string.Format("【{0}】", proClass.Name) + product.Name;                   
                //}
              
                foreach(var item in BargainDetails)
                {
                    json.Add(new BargainDetail()
                    {
                        Bargain = item,
                        Product = products.Where(k => k.Id == item.ProductID).FirstOrDefault() ?? new ProductInfo()
                    });
                }
                return Json(new { flag = true, list = json }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { flag = false });
            }
        }


        /// <summary>
        /// 第一次分享
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Share()
        {
            try
            {
                #region 记录formId
                //string formId = RequestHelper.GetForm<string>("formId");
                //int userId = RequestHelper.GetForm<int>("uid");
                ////new TxtLog(Server.MapPath("/apilog/")).Write("-----Share:formId:" + formId + "----");
                //if (!string.IsNullOrWhiteSpace(formId) && userId > 0)
                //{
                //    WxFormIdBLL.Add(new WxFormIdInfo
                //    {
                //        FormId = formId,
                //        Used = 0,
                //        UserId = userId,
                //        AddDate = DateTime.Now
                //    });
                //}
                #endregion
                var bargainOrder = BargainOrderBLL.ReadBargainOrder(RequestHelper.GetForm<int>("OrderId"));
                if (bargainOrder.Id > 0 && bargainOrder.ShareStatus == 0)
                {
                    //更新已砍金额，加入分享金额
                    bargainOrder.BargainPrice += bargainOrder.SharePrice;
                    bargainOrder.ShareStatus = 1;
                    BargainOrderBLL.UpdateBargainOrder(bargainOrder);
                    CheckStatus(bargainOrder.Id);
                    bargainOrder = BargainOrderBLL.ReadBargainOrder(bargainOrder.Id);
                    return Json(new { flag = true, bargainStatus = bargainOrder.Status });
                }
                else
                {
                    return Json(new { flag = false });
                }
            }
            catch (Exception)
            {

                return Json(new { flag = false });
            }

        }

        /// <summary>
        /// 获取用户的砍价列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Users()
        {
            try
            {
                var user = UserBLL.Read(RequestHelper.GetQueryString<int>("uid"));
                var List = new List<BargainOrderList>();
                var ProductClassList = ProductClassBLL.ReadList();

                var bargain = BargainBLL.SearchBargainList(new BargainSearch());
                var bargainDetail = BargainDetailsBLL.ReadList();
                #region old method

                //var product = ProductBLL.ReadList();
                //foreach (var item in product)
                //{
                //    #region 加载商品的分类
                //    int classId = 0;
                //    try
                //    {
                //        classId = int.Parse(item.ClassId.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault());
                //    }
                //    catch (Exception)
                //    {
                //        classId = 0;
                //        throw;
                //    }
                //    if (classId > 0)
                //    {
                //        var proClass = _ProductClassList.Where(k => k.Id == classId).SingleOrDefault() ?? new ProductClassInfo();
                //        if (proClass.Id > 0)
                //        {
                //            item.Name = string.Format("【{0}】", proClass.Name) + item.Name;
                //        }
                //    }
                //    #endregion

                //}

                #endregion
                var bargainOrderList = BargainOrderBLL.SearchBargainOrderList(new BargainOrderSearch()
                {
                    UserId = user.Id
                });
               
                foreach (var item in bargainOrderList)
                {
                    var _bargainDetail = bargainDetail.Where(k => k.Id == item.BargainDetailsId).SingleOrDefault() ?? new BargainDetailsInfo();
                    if (_bargainDetail.Id > 0)
                    {
                        var _bargain = bargain.Where(k => k.Id == _bargainDetail.BargainId).SingleOrDefault() ?? new BargainInfo();
                        _bargain.BargainDateStatus = DateTime.Now < _bargain.StartDate ? -1 : DateTime.Now >= _bargain.StartDate && DateTime.Now < _bargain.EndDate ? 0 : 1;
                        //加载中判断砍价订单是否已结束
                        if (item.Status == (int)BargainOrderType.进行中 && DateTime.Now >= _bargain.EndDate)
                        {
                            item.Status = (int)BargainOrderType.砍价失败;
                            BargainOrderBLL.UpdateBargainOrder(item);
                        }
                        if (_bargain.Id > 0)
                        {
                            //var _product = product.Where(k => k.Id == _bargainDetail.ProductID).SingleOrDefault() ?? new ProductInfo();
                            var _product = new ProductInfo { Id = _bargainDetail.ProductID };
                            if (_product.Id > 0)
                            {
                                List.Add(new BargainOrderList()
                                {
                                    Bargain = _bargain,
                                    bargainOrder = item,
                                    Bargain_Detail= _bargainDetail,
                                    Difference = (_product.MarketPrice - _bargainDetail.ReservePrice - item.BargainPrice),
                                    Product = _product,

                                });

                            }

                        }
                    }
                }

                #region 完善product信息
                var productIds = List.Select(k => k.Product.Id).ToArray();
                var products = ProductBLL.SearchList(new ProductSearchInfo { IsSale = 1, IsDelete = 0, InProductId = string.Join(",", productIds) });
                //foreach (var product in products)
                //{
                //    int classId = 0;
                //    try
                //    {
                //        classId = int.Parse(product.ClassId.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault());
                //    }
                //    catch (Exception)
                //    {
                //        classId = 0;
                //        throw;
                //    }
                //    var proClass = ProductClassList.FirstOrDefault(k => k.Id == classId) ?? new ProductClassInfo();
                //    //product.Photo = ShopCommon.ShowImage(product.Photo.Replace("Original", "350-350"));
                //    product.Name = string.Format("【{0}】", proClass.Name) + product.Name;
                //}
                List.ForEach(k => k.Product =products.FirstOrDefault(p=>p.Id==k.Product.Id)??new ProductInfo());
                List.ForEach(k => k.Difference = k.Product.MarketPrice -k.Bargain_Detail.ReservePrice - k.bargainOrder.BargainPrice);
                #endregion

                return Json(new { flag = true, List = List }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { flag = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 砍价详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            bool End = false;
            int Id = RequestHelper.GetQueryString<int>("Id");
            int orderId = RequestHelper.GetQueryString<int>("orderId");
            int userId = RequestHelper.GetQueryString<int>("uid");
            //错误码 1:朋友砍价成功 2:本人砍价成功 -1:砍价失败
            int errorCode = 0;
            //砍价订单发起用户
            UserInfo user = UserBLL.ReadUserMore(userId);
            //当前登录用户（用于判断用户是否授权绑定微信昵称头像）
            UserInfo _loginUser = user.Id > 0 ? user : new UserInfo();
            var bargainDetail = BargainDetailsBLL.ReadBargainDetails(Id);
            var product = ProductBLL.Read(bargainDetail.ProductID);
            bool limit = false;
            string msg = string.Empty;


        
            var bargain = BargainBLL.ReadBargain(bargainDetail.BargainId);
          
            if (DateTime.Now >= bargain.EndDate)
            {
                End = true;
            }
           
            //帮好友砍价记录
            var helpRecording = new RecordingInfo();
            var helpPhoto = string.Empty;
            //Log.Error("CheckStatus",orderId.ToString());
            //Log.Error("CheckStatus", CheckStatus(orderId).ToString());
            //砍价订单
            var B_order = BargainOrderBLL.ReadBargainOrder(orderId);
            var recording = new RecordingInfo();
            if (orderId <= 0) {
                //判断当前商品是否还有库存
                if (bargainDetail.Stock - bargainDetail.Sales <= 0)
                {
                    return Json(new { ok = false, msg = "已抢完，请选择其他商品", errorCode = -2 }, JsonRequestBehavior.AllowGet);
                }
                //发起砍价本人进来查看
                B_order = BargainOrderBLL.SearchBargainOrderList(new BargainOrderSearch() { BargainDetailsId = bargainDetail.Id, UserId = user.Id }).SingleOrDefault() ?? new BargainOrderInfo();
                #region 如果本人没创建砍价，则新建一个
                if (B_order.Id <= 0)
                {
                    if (bargain.Status == 0)
                    {
                        return Json(new { ok = false, msg = "活动已取消", errorCode = -2 }, JsonRequestBehavior.AllowGet);
                    }
                    //新建一个记录砍价订单
                    B_order = new BargainOrderInfo()
                    {
                        UserId = user.Id,
                        UserName=user.UserName,
                        UserPhoto=user.Photo,
                        Status = (int)BargainOrderType.进行中,
                        BargainDetailsId = bargainDetail.Id,
                        //SharePrice = BargainHelper.BargainPrice(product.MarketPrice, 0, bargainDetail.ReservePrice, bargain.SuccessRate * (int)(new Random().Next(1,3) / 10.00)),
                        ShareStatus = 0,
                        BargainId = bargain.Id,
                        Total_Num=bargain.LimitCount,
                        Total_Money= product.MarketPrice - bargainDetail.ReservePrice
                };

                    //获取每刀砍的金额 、 分享金额               
                    List<decimal> bargain_Moneys = BargainHelper.ComputeBargainMoney(B_order);
                    if ((bargain_Moneys.Sum(k => k) + B_order.SharePrice) != B_order.Total_Money)
                    {
                        return Json(new { ok = false, msg = "创建砍价失败，请稍后重试", errorCode = -2 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //记录第一刀
                        B_order.BargainPrice = bargain_Moneys[0];

                        //事务操作：保存第一刀金额，分享金额，保存帮砍记录金额,砍价参与人数加1
                        B_order.Id=BargainOrderBLL.CreateBargainOrder(B_order, bargain_Moneys);
                        if (B_order.Id <= 0)
                        {
                            return Json(new { ok = false, msg = "创建砍价失败，请稍后重试", errorCode = -2 }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    #region old method
                    /*     
                B_order.Id = BargainOrderBLL.AddBargainOrder(B_order);
                if(B_order.Id >0){
                    //bargain的砍价参与人数加1
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("[Number]", bargain.Number + 1);
                    BargainBLL.UpdatePart("[Bargain]", dict, bargain.Id);

                    //首次砍价(砍掉28~35%)
                    recording = new RecordingInfo()
                    {
                        BOrderId = B_order.Id,
                        Price = (product.MarketPrice-B_order.BargainPrice-bargainDetail.ReservePrice)* (int)new Random().Next(28, 35) / 100,
                        //Price = BargainHelper.BargainPrice(product.MarketPrice, B_order.BargainPrice, bargainDetail.ReservePrice, bargain.SuccessRate * (int)new Random().Next(3, 4) / 10),
                        Photo = user.Photo,
                        AddDate = DateTime.Now,
                        UserId = user.Id,
                        UserName = user.UserName
                    };
                    //判断砍价金额是否超出
                    if (product.MarketPrice - recording.Price < bargainDetail.ReservePrice)
                    {
                        recording.Price = product.MarketPrice - bargainDetail.ReservePrice;
                    }
                    recording.Id = RecordingBLL.AddRecording(recording);
                    //记录砍价金额保存到砍价单
                    B_order.BargainPrice += recording.Price;
                    //判断分享的砍价金额是否低于底价
                    if (product.MarketPrice - B_order.BargainPrice - B_order.SharePrice < bargainDetail.ReservePrice)
                    {
                        var sharePrice = product.MarketPrice - B_order.BargainPrice - bargainDetail.ReservePrice;
                        B_order.SharePrice = sharePrice;
                    }
                    BargainOrderBLL.UpdateBargainOrder(B_order);

                }
                 */
                    #endregion
                }
                #endregion
            }
            else
            {
                //点击分享进来
                user = UserBLL.ReadUserMore(B_order.UserId);
            }
           
           
          
            //    //获取砍价记录
           var recordingList = RecordingBLL.SearchRecordingList(new RecordingSearch() { BOrderId = B_order.Id }).Where(k => k.UserId > 0).ToList();
            //获取砍价第一刀
            if (recordingList.Count > 0) recording = recordingList[0];
            if (user.Id != userId)
            {
                //点击分享进来
                helpRecording = recordingList.FirstOrDefault(k => k.UserId == userId) ?? new RecordingInfo();
            }
            if (B_order.Id > 0 && CheckStatus(B_order.Id))
            {
                //目前本活动已经砍了几刀
                int bargainCounts = recordingList.Count;
                //如果还没达到本活动砍价人数限制
                if (bargainCounts < bargain.LimitCount)
                { 
                        //砍价订单的用户头像
                        helpPhoto = UserBLL.Read(B_order.UserId).Photo;
                    
                }
                else
                {
                    limit = true;
                }

            }
            else
            {
                var ord = BargainOrderBLL.ReadBargainOrder(orderId);
                switch (ord.Status)
                {
                    case (int)BargainOrderType.砍价成功:
                    case (int)BargainOrderType.支付完成:
                    case (int)BargainOrderType.待付款:
                        errorCode = userId == B_order.UserId ? 2:1;
                        msg = userId==B_order.UserId? "您已经砍价成功了" : "您的朋友已经砍价成功了";                        
                        break;                   
                    case (int)BargainOrderType.砍价失败:
                        errorCode = -1;
                       msg = userId == B_order.UserId ? "您砍价失败了" : "您的朋友砍价失败了";
                        break;
                    default:
                        break;
                }
            }
            if (errorCode != 0)
            {
                return Json(new { ok = false, msg = msg, errorCode = errorCode },JsonRequestBehavior.AllowGet);
            }

            //帮砍记录最多显示15条
            recordingList = recordingList.Take(15).ToList();
            //解码后展示 用户名截断6字符
            recordingList.ForEach(k => k.UserName = StringHelper.Substring(System.Web.HttpUtility.UrlDecode(k.UserName, System.Text.Encoding.UTF8), 6));
            //获取已购买人数
            bargainDetail.Sales += bargain.NumberPeople;

            user.UserName = HttpUtility.UrlDecode(user.UserName, System.Text.Encoding.UTF8);
            #region 推荐商品
            int userGrade = UserGradeBLL.ReadByMoney(user.MoneyUsed).Id;
      
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
            return Json(new {ok=true, msg = msg, product = product, bargainDetail = bargainDetail, bargain = bargain, user = user, recording = recording, recordingList = recordingList, bargainOrder = B_order, helpRecording = helpRecording, helpPhoto = helpPhoto, End = End, limit = limit, loginUser= _loginUser, topproductlist = prorelist }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 砍价操作：帮好友砍1刀，自己也同时创建1个砍价（可分享给好友帮砍）
        /// </summary>
        /// <returns></returns>
        public ActionResult BargainOperate()
        {
           
            bool End = false;
            int Id = RequestHelper.GetQueryString<int>("Id");
            int orderId = RequestHelper.GetQueryString<int>("orderId");
            UserInfo user = UserBLL.Read(RequestHelper.GetQueryString<int>("uid"));
            //错误码 1:朋友砍价成功 2:本人砍价成功 -1:砍价失败
            int errorCode = 0;
            #region 判断是否绑定昵称头像
            if (string.IsNullOrWhiteSpace(user.Photo))
            {
                return Json(new { ok=false,msg="请先授权绑定微信昵称、头像",errorCode=-1}, JsonRequestBehavior.AllowGet);
            }
            #endregion
            var bargainDetail = BargainDetailsBLL.ReadBargainDetails(Id);
            var product = ProductBLL.Read(bargainDetail.ProductID);
            bool limit = false;
            string msg = string.Empty;

            #region 记录formId
            //string formId = RequestHelper.GetQueryString<string>("formId");        
            ////new TxtLog(Server.MapPath("/apilog/")).Write("-----Share:formId:" + formId + "----");
            //if (!string.IsNullOrWhiteSpace(formId) && user.Id > 0)
            //{
            //    WxFormIdBLL.Add(new WxFormIdInfo
            //    {
            //        FormId = formId,
            //        Used = 0,
            //        UserId = user.Id,
            //        AddDate = DateTime.Now
            //    });
            //}
            #endregion

            var bargain = BargainBLL.ReadBargain(bargainDetail.BargainId);
            if (bargain.Status == 0)
            {
                return Json(new { ok = false, msg = "活动已取消", errorCode = -2 }, JsonRequestBehavior.AllowGet);
            }
            if (DateTime.Now >= bargain.EndDate)
            {
                End = true;
            }

            //分享的其他用户砍价
            var helpRecording = new RecordingInfo();
            var helpPhoto = string.Empty;
            //Log.Error("CheckStatus",orderId.ToString());
            //Log.Error("CheckStatus", CheckStatus(orderId).ToString());
            //砍价订单
            var B_order = BargainOrderBLL.ReadBargainOrder(orderId);
            if (orderId > 0 && CheckStatus(orderId))
            {
                //目前本活动已经砍了几刀
                var bargain_record_list = RecordingBLL.SearchRecordingList(new RecordingSearch() { BOrderId = orderId }).Where(k => k.UserId > 0).ToList();
                int bargainCounts = bargain_record_list.Count;
                //如果还没达到本活动砍价人数限制
                if (bargainCounts < bargain.LimitCount)
                {
                    //防同一用户重复砍价
                    if (B_order.UserId != user.Id && bargain_record_list.Where(k => k.UserId == user.Id).Count() <= 0)
                    {
                        #region 每人每天限砍3刀
                        int recordingCount = RecordingBLL.GetRecordingCountByUser(user.Id);
                        new TxtLog(Server.MapPath("/apilog/")).Write("-----userId:"+ user.Id + "----今天砍价次数:"+ recordingCount);
                        if (recordingCount >= 3) return Json(new { ok = false, msg = "今天砍价次数已用完，请明天再来", errorCode =0 }, JsonRequestBehavior.AllowGet);
                        #endregion
                        #region Old Method
                        /*
                        helpRecording.AddDate = DateTime.Now;
                        helpRecording.BOrderId = B_order.Id;
                        helpRecording.Photo = user.Photo;
                        helpRecording.Price = BargainHelper.BargainPrice(product.MarketPrice, B_order.BargainPrice, bargainDetail.ReservePrice, bargain.SuccessRate);
                        Log.Error("PRICE", String.Format("{0}-{1}-{2}", helpRecording.Price, product.MarketPrice, bargainDetail.ReservePrice));
                        helpRecording.UserId = user.Id;
                        helpRecording.UserName = user.UserName;
                        

                        //判断砍价金额是否超出
                        if (product.MarketPrice - B_order.BargainPrice - helpRecording.Price < bargainDetail.ReservePrice)
                        {
                            helpRecording.Price = product.MarketPrice - B_order.BargainPrice - bargainDetail.ReservePrice;
                            Log.Error("超出", helpRecording.Price.ToString());
                        }
                        //如果砍价成功率设为100%，且是最后1刀，则默认砍完剩下的金额
                        if (bargain.SuccessRate>=100 && bargainCounts + 1 == bargain.LimitCount)
                        {
                            helpRecording.Price = product.MarketPrice - B_order.BargainPrice - bargainDetail.ReservePrice;
                        }
                        helpRecording.Id = RecordingBLL.AddRecording(helpRecording);
                        //砍价成功，更新bargainorder已砍金额
                        B_order.BargainPrice += helpRecording.Price;
                        BargainOrderBLL.UpdateBargainOrder(B_order);
                        */
                        #endregion
                        #region 事务操作：用户领取一刀(更新Recording)，更新BargainOrder.BargainPrice
                        helpRecording.Id = RecordingBLL.HelpBargain(B_order, user);
                        if (helpRecording.Id<=0)
                        {//如果帮砍失败
                            return Json(new { ok = false, msg = "砍价出错，请稍后重试", errorCode = -2 }, JsonRequestBehavior.AllowGet);
                        }
                       
                        #endregion
                        CheckStatus(orderId);

                        //砍价订单的用户头像
                        helpPhoto = UserBLL.Read(B_order.UserId).Photo;
                    }
                }
                else
                {
                    limit = true;                  
                }

            }
            else
            {
                var ord = BargainOrderBLL.ReadBargainOrder(orderId);
                switch (ord.Status)
                {
                    case (int)BargainOrderType.砍价成功:
                    case (int)BargainOrderType.支付完成:
                    case (int)BargainOrderType.待付款:
                        errorCode = 1;
                        msg = "您的朋友已经砍价成功了";
                        break;
                    case (int)BargainOrderType.砍价失败:
                        errorCode = -1;
                        msg = "您的朋友砍价失败了";
                        break;
                    default:
                        break;
                }

            }
            if (errorCode != 0)
            {
                return Json(new { ok = false, msg = msg, errorCode = errorCode }, JsonRequestBehavior.AllowGet);
            }

            var recording = new RecordingInfo();
            //查询砍价记录订单
            var BargainOrder = BargainOrderBLL.SearchBargainOrderList(new BargainOrderSearch() { BargainDetailsId = bargainDetail.Id, UserId = user.Id }).SingleOrDefault() ?? new BargainOrderInfo();//查询用户是否有此商品的砍价记录
            var recordingList = RecordingBLL.SearchRecordingList(new RecordingSearch() { BOrderId = BargainOrder.Id }).Where(k=>k.UserId>0).ToList();
            if (BargainOrder.Id <= 0)
            {
                //新建一个记录砍价订单
                BargainOrder = new BargainOrderInfo()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    UserPhoto = user.Photo,
                    Status = (int)BargainOrderType.进行中,
                    BargainDetailsId = bargainDetail.Id,
                    //SharePrice = BargainHelper.BargainPrice(product.MarketPrice, 0, bargainDetail.ReservePrice, bargain.SuccessRate * (int)new Random().Next(3) / 10),
                    ShareStatus = 0,
                    BargainId = bargain.Id,
                    Total_Num = bargain.LimitCount,
                    Total_Money = product.MarketPrice - bargainDetail.ReservePrice
                };
                //获取每刀砍的金额 、 分享金额               
                List<decimal> bargain_Moneys = BargainHelper.ComputeBargainMoney(BargainOrder);
                if ((bargain_Moneys.Sum(k => k) + BargainOrder.SharePrice) != BargainOrder.Total_Money)
                {
                    return Json(new { ok = false, msg = "创建砍价失败，请稍后重试", errorCode = -2 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //记录第一刀
                    BargainOrder.BargainPrice = bargain_Moneys[0];

                    //事务操作：保存第一刀金额，分享金额，保存帮砍记录金额,砍价参与人数加1
                    BargainOrder.Id = BargainOrderBLL.CreateBargainOrder(BargainOrder, bargain_Moneys);
                    if (BargainOrder.Id <= 0)
                    {
                        return Json(new { ok = false, msg = "创建砍价失败，请稍后重试", errorCode = -2 }, JsonRequestBehavior.AllowGet);
                    }
                }
                #region old method
                /*
            BargainOrder.Id = BargainOrderBLL.AddBargainOrder(BargainOrder);
            if (BargainOrder.Id > 0)
            {


                //bargain的砍价参与人数加1
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("[Number]", bargain.Number + 1);
                BargainBLL.UpdatePart("[Bargain]", dict, bargain.Id);
                //首次砍价(砍掉28~35%)
                recording = new RecordingInfo()
                {
                    BOrderId = BargainOrder.Id,
                    Price = (product.MarketPrice - B_order.BargainPrice - bargainDetail.ReservePrice) * (int)new Random().Next(28, 35) / 100,
                    //Price = BargainHelper.BargainPrice(product.MarketPrice, BargainOrder.BargainPrice, bargainDetail.ReservePrice, bargain.SuccessRate * (int)new Random().Next(3, 4) / 10),
                    Photo = user.Photo,
                    AddDate = DateTime.Now,
                    UserId = user.Id,
                    UserName = user.UserName
                };
                //判断砍价金额是否超出
                if (product.MarketPrice - recording.Price < bargainDetail.ReservePrice)
                {
                    recording.Price = product.MarketPrice - bargainDetail.ReservePrice;
                }

                recording.Id = RecordingBLL.AddRecording(recording);
                //获取砍价记录
                recordingList = RecordingBLL.SearchRecordingList(new RecordingSearch() { BOrderId = BargainOrder.Id });
                recordingList.ForEach(item => { BargainOrder.BargainPrice += item.Price; });
                //判断分享的砍价金额是否低于底价
                if (product.MarketPrice - BargainOrder.BargainPrice - BargainOrder.SharePrice < bargainDetail.ReservePrice)
                {
                    var sharePrice = product.MarketPrice - BargainOrder.BargainPrice - bargainDetail.ReservePrice;
                    BargainOrder.SharePrice = sharePrice;
                }


                BargainOrderBLL.UpdateBargainOrder(BargainOrder);
             
            }
               */
                #endregion
            }
            CheckStatus(BargainOrder.Id);
            //获取帮好友分享砍的记录
            helpRecording = RecordingBLL.ReadRecording(helpRecording.Id);
            //获取我创建的砍价单
            BargainOrder = BargainOrderBLL.ReadBargainOrder(BargainOrder.Id);
            //获取帮砍记录  帮砍记录最多显示15条
            recordingList = RecordingBLL.SearchRecordingList(new RecordingSearch() { BOrderId = BargainOrder.Id }).Where(k=>k.UserId>0).Take(15).ToList();
            //解码后展示 用户名截断6字符
            recordingList.ForEach(k => k.UserName = StringHelper.Substring(System.Web.HttpUtility.UrlDecode(k.UserName, System.Text.Encoding.UTF8), 6));
            //获取已购买人数
            bargainDetail.Sales += bargain.NumberPeople;

            user.UserName = HttpUtility.UrlDecode(user.UserName, System.Text.Encoding.UTF8);
            return Json(new { ok = true, msg = msg, product = product, bargainDetail = bargainDetail, bargain = bargain, user = user, recording = recording, recordingList = recordingList, bargainOrder = BargainOrder, helpRecording = helpRecording, helpPhoto = helpPhoto, End = End, limit = limit }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 检查砍价订单状态
        /// </summary>
        /// <param name="bargainOrderId"></param>
        /// <returns></returns>
        public bool CheckStatus(int bargainOrderId)
        {
            bool flag = false;
            //new TxtLog(Server.MapPath("/apilog/")).Write("-----bargainOrderId:"+ bargainOrderId+"----");
            var bargainOrder = BargainOrderBLL.ReadBargainOrder(bargainOrderId);
            //new TxtLog(Server.MapPath("/apilog/")).Write("-----BargainOrderType:" + bargainOrder.Status + "----");
            if (bargainOrder.Id > 0 && bargainOrder.Status == (int)BargainOrderType.进行中)
            {
                var bargainDetail = BargainDetailsBLL.ReadBargainDetails(bargainOrder.BargainDetailsId);
                var bargain = BargainBLL.ReadBargain(bargainDetail.BargainId);
                if (DateTime.Now >= bargain.EndDate)
                {//已经结束,砍价失败
                    bargainOrder.Status = (int)BargainOrderType.砍价失败;
                    BargainOrderBLL.UpdateBargainOrder(bargainOrder);
                    return flag;
                }
                var product = ProductBLL.Read(bargainDetail.ProductID);
                if (product.MarketPrice - bargainOrder.BargainPrice == bargainDetail.ReservePrice && bargainOrder.Status != (int)BargainOrderType.砍价成功)
                {
                    bargainOrder.Status = (int)BargainOrderType.砍价成功;
                    //BargainOrderBLL.UpdateBargainOrder(bargainOrder);
                    if (BargainOrderBLL.UpdateBargainOrder(bargainOrder))
                    {                        
                        //发送砍价成功模板消息
                        SendBargainMessage(bargainOrder);
                        //new TxtLog(Server.MapPath("/apilog/")).Write("-----发送砍价成功模板消息:End----");
                    }
                }
                else if (DateTime.Now >= bargain.EndDate && bargainOrder.Status != (int)BargainOrderType.砍价失败)
                {
                    bargainOrder.Status = (int)BargainOrderType.砍价失败;
                    BargainOrderBLL.UpdateBargainOrder(bargainOrder);
                }
                else
                {//进行中
                    flag = true;
                }
            }

            return flag;
        }
        /// <summary>
        /// 推送砍价成功模板消息
        /// </summary>
        /// <param name="bargainOrder">砍价活动</param>
        private JsonResult SendBargainMessage(BargainOrderInfo bargainOrder)
        {
            //new TxtLog(Server.MapPath("/apilog/")).Write("-----发送砍价成功模板消息:start----");
            try
            {
                if (bargainOrder.Id <= 0)
                {
                    //new TxtLog(Server.MapPath("/apilog/")).Write("-----推送砍价成功模板消息:no bargainOrder" + bargainOrder.Status + "----");
                    return Json(new { flag = false, msg = "no bargainOrder" }, JsonRequestBehavior.AllowGet);
                }
                if (bargainOrder.Status != (int)BargainOrderType.砍价成功)
                {
                    //new TxtLog(Server.MapPath("/apilog/")).Write("-----推送砍价成功模板消息:bargainOrder not success" + bargainOrder.Status + "----");
                    return Json(new { flag = false, msg = "bargainOrder not success" }, JsonRequestBehavior.AllowGet);
                }
                var wxFormId = WxFormIdBLL.ReadUnusedByUserId(bargainOrder.UserId).FirstOrDefault() ?? new WxFormIdInfo();
                if (wxFormId.Id <= 0 || string.IsNullOrEmpty(wxFormId.FormId))
                {
                    //new TxtLog(Server.MapPath("/apilog/")).Write("-----推送砍价成功模板消息:no form_id" + bargainOrder.Status + "----");
                    return Json(new { flag = false, msg = "no form_id" }, JsonRequestBehavior.AllowGet);
                }
                var bargainDetail = BargainDetailsBLL.ReadBargainDetails(bargainOrder.BargainDetailsId);
                var _product = ProductBLL.Read(bargainDetail.ProductID);
                string form_id = wxFormId.FormId;
                string template_id = ShopConfig.ReadConfigInfo().BarGainTemplateId;
                var _sendToUser = UserBLL.Read(bargainOrder.UserId);
                string touser = _sendToUser.OpenId;
                string pronames = bargainDetail.ProductName;

                string leaderName = string.Format("发起人：{0}", System.Web.HttpUtility.UrlDecode(_sendToUser.UserName, System.Text.Encoding.UTF8));
                //砍价记录
                var recordings = RecordingBLL.SearchRecordingList(new RecordingSearch { BOrderId = bargainOrder.Id });
                string[] signUserNames = recordings.Where(k => k.UserId != bargainOrder.UserId).Select(k => System.Web.HttpUtility.UrlDecode(k.UserName, System.Text.Encoding.UTF8)).ToArray();
                string _signNames = string.Join(",", signUserNames);
                var data = Json(new
                {
                    keyword1 = new { value = pronames, color = "#333333" },
                    keyword2 = new { value = _product.MarketPrice.ToString(), color = "#ff3333" },
                    keyword3 = new { value = (_product.MarketPrice - bargainDetail.ReservePrice).ToString(), color = "#333333" },
                    keyword4 = new { value = bargainDetail.ReservePrice.ToString(), color = "#333333" },
                    keyword5 = new { value = _signNames, color = "#333333" },
                    keyword6 = new { value = "砍价成功", color = "#333333" },
                    keyword7 = new { value = leaderName, color = "#333333" }
                });
                string access_token = WxGetInfo.IsExistAccess_Token();

                string url = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + access_token;
                //new TxtLog(Server.MapPath("/apilog/")).Write("-----推送砍价成功模板消息:access_token:" + access_token + "----");
                WxPayData jsondata = new WxPayData();
                jsondata.SetValue("touser", touser);
                jsondata.SetValue("template_id", template_id);
                jsondata.SetValue("page", "pages/poi/index");
                jsondata.SetValue("form_id", form_id);

                jsondata.SetValue("data", data.Data);
                //new TxtLog(Server.MapPath("/apilog/")).Write("-----推送砍价成功模板消息:" + jsondata.ToJson() + "----");
                string result = HttpService.PostByJson(jsondata.ToJson(), url, false, 6);

                Log.Debug("send opengroupmsg result", result);
                JObject jd = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result);

                //new TxtLog(Server.MapPath("/apilog/")).Write("-----推送砍价成功模板消息:" + (string)jd["errcode"] + "---- " + (string)jd["errmsg"] + "-----" + jsondata.ToJson() + bargainOrder.Status + "----");
                //if ((string)jd["errmsg"] == "ok")
                //{
                    //改变formid使用状态
                    WxFormIdBLL.ChangeUsed(wxFormId.Id);
                    #region 给发起者推送之后再给参与者发
                    foreach (var item in recordings.Where(k => k.UserId != bargainOrder.UserId))
                    {
                        wxFormId = WxFormIdBLL.ReadUnusedByUserId(item.UserId).FirstOrDefault() ?? new WxFormIdInfo();
                        if (wxFormId.Id <= 0 || string.IsNullOrEmpty(wxFormId.FormId))
                        {
                            //return Json(new { flag = false, msg = "no form_id" }, JsonRequestBehavior.AllowGet);
                            //如果此参与者没有FormId，则跳过此人，转向下一个参与者
                            continue;
                        }
                        form_id = wxFormId.FormId;
                        string template_id1 = ShopConfig.ReadConfigInfo().BarGainTemplateId;
                        var _sendToUser1 = UserBLL.Read(item.UserId);
                        var touser1 = _sendToUser1.OpenId;

                        var data1 = Json(new
                        {
                            keyword1 = new { value = pronames, color = "#333333" },
                            keyword2 = new { value = _product.MarketPrice.ToString(), color = "#ff3333" },
                            keyword3 = new { value = (_product.MarketPrice - bargainDetail.ReservePrice).ToString(), color = "#333333" },
                            keyword4 = new { value = bargainDetail.ReservePrice.ToString(), color = "#333333" },
                            keyword5 = new { value = _signNames, color = "#333333" },
                            keyword6 = new { value = "砍价成功", color = "#333333" },
                            keyword7 = new { value = leaderName, color = "#333333" }
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
                    return Json(new { flag = true, msg = "ok", result = jsondata.ToJson().ToString(), bargainOrderId = bargainOrder.Id }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    return Json(new { flag = false, msg = "send fail:" + (string)jd["errcode"] + " " + (string)jd["errmsg"], result = jsondata.ToJson() }, JsonRequestBehavior.AllowGet);
                //}
            }
            catch(Exception ex)
            {
                //new TxtLog(Server.MapPath("/apilog/")).Write("-----发送砍价成功模板消息:fail"+ex.Message+"----");
                return Json(new { flag = false, msg = ex.Message });
            }      

        }
        /// <summary>
        /// 快刀榜
        /// </summary>
        /// <returns></returns>
        public ActionResult Leaderboard()
        {
            //读取所有砍价成功的数据
            var list = new List<object>();
            var bargainOrder = BargainOrderBLL.SearchBargainOrderList(new BargainOrderSearch { Status = (int)BargainOrderType.支付完成 });
            foreach (var item in bargainOrder)
            {
                var bargainDetail = BargainDetailsBLL.ReadBargainDetails(item.BargainDetailsId);
                if (bargainDetail.Id > 0)
                {
                    var product = ProductBLL.Read(bargainDetail.ProductID);
                    if (product.Id > 0)
                    {
                        var user = UserBLL.Read(item.UserId);
                        list.Add(new
                        {
                            Count = RecordingBLL.SearchRecordingList(new RecordingSearch() { BOrderId = item.Id }).Count,
                            Name = StringHelper.Substring(HttpUtility.UrlDecode(user.UserName, System.Text.Encoding.UTF8), 8),
                            Photo = user.Photo,
                            Content = StringHelper.Substring(string.Format("{0}元拿【{1}】", bargainDetail.ReservePrice, product.Name), 12),
                            ProductName = bargainDetail.ProductName
                        });
                    }
                }

            }

            return Json(new { list = list }, JsonRequestBehavior.AllowGet);
        }

        public class BargainDetail
        {
            public BargainDetailsInfo Bargain { get; set; }

            public ProductInfo Product { get; set; }
        }

        /// <summary>
        /// 会员中心组装分类
        /// </summary>
        public class BargainOrderList
        {
            public BargainOrderInfo bargainOrder { get; set; }

            public BargainInfo Bargain { get; set; }

            public ProductInfo Product { get; set; }

            public BargainDetailsInfo Bargain_Detail { get; set; }
            public decimal Difference { get; set; }
        }

    }
}