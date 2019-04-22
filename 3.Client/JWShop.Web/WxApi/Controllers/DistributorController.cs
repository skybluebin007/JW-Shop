using JWShop.Common;
using JWShop.XcxApi.Pay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using JWShop.XcxApi.Filter;

namespace JWShop.XcxApi.Controllers
{
    /// <summary>
    /// 分销
    /// </summary>
    [Auth]
    public class DistributorController : BaseController
    {
        private UserInfo distributor = new UserInfo();
        public DistributorController()
        {
            int uid = RequestHelper.GetForm<int>("uid");
            if (uid > 0)
            {
                distributor = UserBLL.Read(uid);
                distributor.UserName = System.Web.HttpUtility.UrlDecode(distributor.UserName, Encoding.UTF8);
            }
        }
        /// <summary>
        /// 判断用户归属哪个分销商
        /// </summary>
        /// <param name="uid">会员Id</param>
        /// <param name="distributor_id">分销商Id</param>
        /// <returns></returns>
        public ActionResult ChangeUserToDistributor(int uid, int distributor_id)
        {
            if (uid < 0)
            {
                return Json(new { ok = false, error = "用户编号不存在" });
            }
            try
            {
                var user = UserBLL.Read(uid);
                if (distributor_id > 0)
                {
                    //当前分销商
                    var distributor = UserBLL.Read(distributor_id);
                    //如果当前分销商状态正常,且此用户不属于当前分销商
                    if (distributor.Distributor_Status == (int)Distributor_Status.Normal && user.Recommend_UserId != distributor.Id)
                    {
                        //如果用户已有分销商 则 不能再分配分销商
                        if (user.Recommend_UserId > 0)
                        {
                            //var old_distributor = UserBLL.Read(user.Recommend_UserId);
                            ////如果原分销商状态不正常，,则修改用户归属当前分销商
                            //if (old_distributor.Distributor_Status != (int)Distributor_Status.Normal)
                            //{
                            //    Dictionary<string, object> dict = new Dictionary<string, object>();
                            //    dict.Add("[Recommend_UserId]", distributor.Id);
                            //    UserBLL.UpdatePart("[Usr]", dict, user.Id);
                            //}
                        }
                        else
                        {
                            //如果用户没有归属分销商 且 是首次登录   且 用户不是当前分销商的分销商（不能互为上下级）,则修改用户归属当前分销商
                            //如果不是首次登录，则表示用户已归属平台，不能再分配分销商
                            if (user.LoginTimes <= 1 &&　distributor.Recommend_UserId!=user.Id)
                            {
                                //Dictionary<string, object> dict = new Dictionary<string, object>();
                                //dict.Add("[Recommend_UserId]", distributor.Id);
                                //UserBLL.UpdatePart("[Usr]", dict, user.Id);
                                //事务：用户归属分销商，更新分销商及其上级分销商的总下级数
                                UserBLL.ChangeUserToDistributor(user.Id, distributor.Id);
                            }
                        }
                    }
                }
                return Json(new { ok = true });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = ex.Message });
            }
        }
        /// <summary>
        /// 生成小程序二维码
        /// 带参数（分销商Id），扫码进入小程序首页
        /// </summary>    
        [HttpPost]
        public ActionResult CreateDistributorCode(int uid)
        {
            if (uid != distributor.Id)
            {
                return Json(new { ok = false, error = "无权操作" });
            }
            //如果之前已经生成了小程序推广码，则直接返回
            if (!string.IsNullOrWhiteSpace(distributor.ProviderName))
            {
                return Json(new { ok = true, qrcodeurl = distributor.ProviderName });
            }
            string access_token = WxGetInfo.IsExistAccess_Token();

            string url = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + access_token;

            WxPayData jsondata = new WxPayData();
            jsondata.SetValue("scene", uid);
            jsondata.SetValue("page", "pages/poi/index");
            jsondata.SetValue("width", 430);
            string qrcodeUrl = string.Empty;
            CreateQrCode(jsondata, url, ref qrcodeUrl);
            if (!string.IsNullOrWhiteSpace(qrcodeUrl))
            {
                #region 更新到user表
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("[ProviderName]", qrcodeUrl);
                UserBLL.UpdatePart("[Usr]", dict, uid);
                #endregion
                return Json(new { ok = true, qrcodeurl = qrcodeUrl });
            }
            else
            {
                return Json(new { ok = false, error = "生成分销推广码出错" });
            }

        }
        /// <summary>
        /// 生成商品详情小程序按码
        /// 扫码进入商品详情页
        /// </summary>
        /// <param name="uid">分销商Id</param>
        /// <param name="pid">商品Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateDistributorProductCode(int uid, int pid)
        {
            if (uid != distributor.Id)
            {
                return Json(new { ok = false, error = "无权操作" });
            }
            string qrcodeUrl = string.Empty;
            DistributorProductQrcodeInfo qrcode = DistributorProductQrcodeBLL.Read(uid, pid);
            if (qrcode != null && !string.IsNullOrWhiteSpace(qrcode.Qrcode))
            {

                return Json(new { ok = true, qrcodeurl = qrcode.Qrcode });

            }
            else
            {
                string access_token = WxGetInfo.IsExistAccess_Token();

                string url = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + access_token;

                WxPayData jsondata = new WxPayData();
                jsondata.SetValue("scene", uid + "," + pid);
                jsondata.SetValue("page", "pages/product/detail");
                jsondata.SetValue("width", 430);
                CreateQrCode(jsondata, url, ref qrcodeUrl);
                if (!string.IsNullOrWhiteSpace(qrcodeUrl))
                {
                    #region 更新到DistributorProductCode表
                    if (qrcode == null)
                    {
                        DistributorProductQrcodeBLL.Add(new DistributorProductQrcodeInfo
                        {
                            Distributor_Id = uid,
                            Product_Id = pid,
                            Qrcode = qrcodeUrl
                        });
                    }
                    else
                    {
                        DistributorProductQrcodeBLL.Update(new DistributorProductQrcodeInfo
                        {
                            Distributor_Id = uid,
                            Product_Id = pid,
                            Qrcode = qrcodeUrl
                        });
                    }
                    #endregion
                    return Json(new { ok = true, qrcodeurl = qrcodeUrl });
                }
                else
                {
                    return Json(new { ok = false, error = "生成分销商品推广码出错" });
                }
            }

        }
        /// <summary>
        /// 获取返佣记录 分页展示
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCommissionPageList(int uid, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                if (uid != distributor.Id)
                {
                    return Json(new { ok = false, error = "无权操作" });
                }
                if (distributor.Distributor_Status != (int)Distributor_Status.Normal)
                {
                    return Json(new { ok = false, error = "分销商待审核或已冻结" });
                }
                if (pageIndex <= 1) pageIndex = 1;
                if (pageIndex <= 0) pageSize = 10;
                int count = 0;
                var dataList = RebateBLL.SearchList(pageIndex, pageSize, new RebateSearchInfo { Distributor_Id = uid }, ref count);
                return Json(new { ok = true,user=distributor, data = dataList, count = dataList.Count });
            }
            catch(Exception ex)
            {
                return Json(new { ok = false, error = ex.Message });
            }
        }
        /// <summary>
        /// 获取已完成的提现记录 分页展示
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetWithdrawPageList(int uid, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                if (uid != distributor.Id)
                {
                    return Json(new { ok = false, error = "无权操作" });
                }
                if (distributor.Distributor_Status != (int)Distributor_Status.Normal)
                {
                    return Json(new { ok = false, error = "分销商待审核或已冻结" });
                }
                if (pageIndex <= 1) pageIndex = 1;
                if (pageIndex <= 0) pageSize = 10;
                int count = 0;
                var dataList = WithdrawBLL.SearchList(pageIndex, pageSize, new WithdrawSearchInfo { Distributor_Id = uid}, ref count);
                dataList.ForEach(k => k.Status_Desc = EnumHelper.ReadEnumChineseName<Withdraw_Status>(k.Status));
                return Json(new { ok = true, user = distributor, data = dataList, count = dataList.Count });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = ex.Message });
            }
        }
        /// <summary>
        /// 分销商申请提现
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApplyWithdraw(int uid,decimal amount)
        {
            try
            {
                if (uid != distributor.Id)
                {
                    return Json(new { ok = false, error = "无权操作" });
                }
                if(WithdrawBLL.SearchList(new WithdrawSearchInfo { Distributor_Id = uid, Status = (int)Withdraw_Status.Apply }).Count > 0)
                {
                    return Json(new { ok=false,error="您已有待审核的提现申请，暂不能再次提交提现申请"});
                }
                if (amount <= 0)
                {
                    return Json(new { ok = false, error = "请输入正确的提现金额" });
                }
                if (amount > distributor.Total_Commission - distributor.Total_Withdraw)
                {
                    return Json(new { ok = false, error = "提现金额超出可提现额度["+(distributor.Total_Commission - distributor.Total_Withdraw) +"元]" });
                }
               int wid= WithdrawBLL.Add(new WithdrawInfo
                {
                    Distributor_Id = uid,
                    Amount = amount,
                    Time = DateTime.Now,
                    Status=(int)Withdraw_Status.Apply
                });
                if (wid > 0)
                {
                    return Json(new { ok = true });
                }
                else
                {
                    return Json(new { ok = false, error = "系统忙" });
                }
            }
            catch(Exception ex)
            {
                return Json(new { ok = false, error = ex.Message });
            }
        }
        /// <summary>
        /// 测试：企业付款给用户接口
        /// </summary>
        /// <returns></returns>
        //public ActionResult CashTransfers()
        //{
        //    string orderNumber = ShopCommon.CreateOrderNumber();
        //    //var order = new Order(){Amount = 1};
        //    // var openid = "oBSBmwQjqwjfzQlKsFNjxFLSixxx";
        //    var user = distributor;          
        //    var transfer = new Pay.TransferPay
        //    {
        //        openid = user.OpenId,
        //        amount = 100,
        //        partner_trade_no = orderNumber,
        //        re_user_name = "stoneniqiu",
        //        spbill_create_ip = ClientHelper.IP
        //    };
        //    var data = transfer.GetTransfersApiParameters();
        //    var result = WxPayApi.Transfers(data);
        //    return Content(result.ToJson());            
        //}
    }
}
