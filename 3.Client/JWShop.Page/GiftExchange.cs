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
    public class GiftExchange : CommonBasePage
    {
        protected UserInfo CurrentUser = new UserInfo();
        protected PointProductInfo Gift = new PointProductInfo();
        protected SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();
        protected decimal userPoint = 0;

        protected override void PageLoad()
        {
            base.PageLoad();

            string action = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("Action"));
            if (action == "Submit") this.Submit();
            if (action == "CheckPoint") this.CheckPoint();

            int id = RequestHelper.GetQueryString<int>("Id");
            if (base.UserId == 0)
            {
                string redirectUrl = string.IsNullOrEmpty(isMobile)
                    ? "/user/login.html?RedirectUrl=/gift/exchange/" + id.ToString() + ".html"
                    : "/mobile/login.html?RedirectUrl=/mobile/gift/exchange/" + id.ToString() + ".html";
                ResponseHelper.Redirect(redirectUrl);
                ResponseHelper.End();
            }

            CurrentUser = UserBLL.Read(base.UserId);
            Gift = PointProductBLL.Read(id);
            userPoint = UserAccountRecordBLL.SumPoint(base.UserId);

            singleUnlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();
            singleUnlimitClass.ClassID = "|1" + CurrentUser.RegionId;

            Title = Gift.Name + " - 奖品兑换";
        }

        protected void CheckPoint()
        {
            if (base.UserId == 0)
            {
                ResponseHelper.Write("还未登录");
                ResponseHelper.End();
            }

            int id = RequestHelper.GetQueryString<int>("Id");
            var gift = PointProductBLL.Read(id);
            var memberPoint = UserAccountRecordBLL.SumPoint(base.UserId);
            if (gift.Point > memberPoint)
            {
                ResponseHelper.Write("您当前的积分不足以兑取该礼品");
            }
            else ResponseHelper.Write("ok");
            ResponseHelper.End();
        }

        protected void Submit()
        {
            string userName = base.UserName;
            string userEmail = CookiesHelper.ReadCookieValue("UserEmail");
            SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();

            //检测积分
            int id = RequestHelper.GetForm<int>("updateId");
            if (id < 1)
            {
                ResponseHelper.Write("error|无效的请求");
                ResponseHelper.End();
            }
            if (singleUnlimitClass.ClassID.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Length < 3)
            {
                ResponseHelper.Write("error|请填写完整的地区信息");
                ResponseHelper.End();
                ResponseHelper.End();
            }

            var pointLeft = UserAccountRecordBLL.SumPoint(base.UserId);
            Gift = PointProductBLL.Read(id);
            if (Gift.Point > pointLeft)
            {
                ResponseHelper.Write("error|您当前的积分不足以兑取该礼品");
                ResponseHelper.End();
            }
            //库存
            int num = RequestHelper.GetForm<int>("num");
            if (num > (Gift.TotalStorageCount - Gift.SendCount))
            {
                ResponseHelper.Write("error|该礼品库存不足");
                ResponseHelper.End();
            }

            //添加订单
            OrderInfo order = new OrderInfo();
            order.OrderNumber = ShopCommon.CreateOrderNumber();
            order.IsActivity = (int)BoolType.True;
            order.OrderStatus = (int)OrderStatus.WaitCheck;
            order.OrderNote = "积分兑换奖品";
            order.ProductMoney = 0;
            order.Balance = 0;
            order.FavorableMoney = 0;
            order.OtherMoney = 0;
            order.CouponMoney = 0;
            order.Consignee = StringHelper.AddSafe(RequestHelper.GetForm<string>("username"));
            order.RegionId = singleUnlimitClass.ClassID;
            order.Address = StringHelper.AddSafe(RequestHelper.GetForm<string>("address"));
            order.ZipCode = StringHelper.AddSafe(RequestHelper.GetForm<string>("zipcode"));
            order.Tel = StringHelper.AddSafe(RequestHelper.GetForm<string>("tel"));
            order.Email = userEmail;
            order.Mobile = StringHelper.AddSafe(RequestHelper.GetForm<string>("mobile"));
            order.ShippingId = 0;
            order.ShippingDate = RequestHelper.DateNow;
            order.ShippingNumber = string.Empty;
            order.ShippingMoney = 0;
            order.PayKey = string.Empty;
            order.PayName = string.Empty;
            order.PayDate = RequestHelper.DateNow; ;
            order.IsRefund = (int)BoolType.False;
            order.FavorableActivityId = 0;
            order.GiftId = 0;
            order.InvoiceTitle = string.Empty;
            order.InvoiceContent = string.Empty;
            order.UserMessage = string.Empty;
            order.AddDate = RequestHelper.DateNow;
            order.IP = ClientHelper.IP;
            order.UserId = base.UserId;
            order.UserName = userName;
            order.ActivityPoint = Gift.Point * num;
            int orderID = OrderBLL.Add(order);

            //添加订单详细
            OrderDetailInfo orderDetail = new OrderDetailInfo();
            orderDetail.OrderId = orderID;
            orderDetail.ProductId = Gift.Id;
            orderDetail.ProductName = Gift.Name;
            //orderDetail.ProductWeight = Gift.Weight;
            orderDetail.SendPoint = 0;
            orderDetail.ProductPrice = 0;
            orderDetail.ActivityPoint = Gift.Point;
            orderDetail.BuyCount = num;
            orderDetail.ParentId = 0;
            orderDetail.RandNumber = string.Empty;
            orderDetail.GiftPackId = 0;
            OrderDetailBLL.Add(orderDetail);

            //积分记录
            var accountRecord = new UserAccountRecordInfo
            {
                Money = 0,
                Point = -Gift.Point * num,
                Date = DateTime.Now,
                IP = ClientHelper.IP,
                Note = "兑换礼品-" + Gift.Name,
                UserId = base.UserId,
                UserName = base.UserName
            };
            UserAccountRecordBLL.Add(accountRecord);

            //更新商品的已兑换数量
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("SendCount", Gift.SendCount + num);
            PointProductBLL.UpdatePart(PointProductInfo.TABLENAME, dict, Gift.Id);

            ResponseHelper.Write("ok|" + orderID);
            ResponseHelper.End();
        }

    }
}