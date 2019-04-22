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
   public class PointExchange:CommonBasePage
    {
        protected UserInfo currentUser = new UserInfo();
        protected PointProductInfo gift = new PointProductInfo();
        protected SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();
        protected int userPoint = 0;

        protected override void PageLoad()
        {
            base.PageLoad();

            string action = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("Action"));
            if (action == "Submit") this.Submit();
            if (action == "CheckPoint") this.CheckPoint();

            int id = RequestHelper.GetQueryString<int>("Id");
            if (base.UserId <= 0)
            {
                ResponseHelper.Redirect("/user/login.html?RedirectUrl=/pointexchange/" + id.ToString() + ".html");
                ResponseHelper.End();
            }

            gift = PointProductBLL.Read(id);
            if (gift.EndDate.Date < DateTime.Now.Date) ResponseHelper.Redirect("/pointproduct.html");

            var currentUser = UserBLL.Read(base.UserId);
            userPoint = UserAccountRecordBLL.SumPoint(base.UserId);

            singleUnlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();
            singleUnlimitClass.ClassID =base.UserId>0? currentUser.RegionId:"|1|";

            Title = gift.Name + " - 积分商品兑换";
        }

        protected void CheckPoint()
        {
            if (base.UserId <= 0)
            {
                ResponseHelper.Write("error|还未登录");
                ResponseHelper.End();
            }

           

            int pointLeft = UserAccountRecordBLL.SumPoint(base.UserId);
            int id = RequestHelper.GetQueryString<int>("id");
            var gift = PointProductBLL.Read(id);

            if (gift.IsSale != (int)BoolType.True)
            {
                ResponseHelper.Write("error|该商品已下架");
                ResponseHelper.End();
            }
            if (gift.Point > pointLeft)
            {
                ResponseHelper.Write("error|您当前的积分不足以兑取该商品");
                ResponseHelper.End();
            }

            ResponseHelper.Write("ok|");
            ResponseHelper.End();
        }

        protected void Submit()
        {
            if (base.UserId <= 0)
            {
                ResponseHelper.Write("error|还未登录");
                ResponseHelper.End();
            }

          

            int pointLeft = UserAccountRecordBLL.SumPoint(base.UserId);
            string userName = base.UserName;
            string userEmail = CookiesHelper.ReadCookieValue("UserEmail");

            //检测积分
            int id = RequestHelper.GetForm<int>("updateId");
            if (id < 1)
            {
                ResponseHelper.Write("error|无效的请求");
                ResponseHelper.End();
            }

            gift = PointProductBLL.Read(id);
            if (gift.IsSale != (int)BoolType.True)
            {
                ResponseHelper.Write("error|该商品已下架");
                ResponseHelper.End();
            }
            if (gift.Point > pointLeft)
            {
                ResponseHelper.Write("error|您当前的积分不足以兑取该商品");
                ResponseHelper.End();
            }
            if (gift.EndDate.Date < DateTime.Now.Date)
            {
                ResponseHelper.Write("error|该商品已过有效兑取时间");
                ResponseHelper.End();
            }

            //库存
            if (gift.TotalStorageCount - gift.SendCount < 1)
            {
                ResponseHelper.Write("error|该商品库存不足");
                ResponseHelper.End();
            }

            //添加订单
            var order = new PointProductOrderInfo();
            order.OrderNumber = ShopCommon.CreateOrderNumber();
            order.OrderStatus = (int)PointProductOrderStatus.Shipping;
            order.Point = gift.Point;
            order.ProductId = gift.Id;
            order.ProductName = gift.Name;
            order.BuyCount = 1;
            order.Consignee = StringHelper.AddSafe(RequestHelper.GetForm<string>("username"));
            SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();
            order.RegionId = singleUnlimitClass.ClassID;
            order.Address = StringHelper.AddSafe(RequestHelper.GetForm<string>("address"));
            order.Tel = StringHelper.AddSafe(RequestHelper.GetForm<string>("mobile"));
            order.AddDate = DateTime.Now;
            order.IP = ClientHelper.IP;
            order.UserId = base.UserId;
            order.UserName = base.UserName;
            order.Id = PointProductOrderBLL.Add(order);

            if (order.Id > 0)
            {
                //扣减积分
                UserAccountRecordBLL.Add(new UserAccountRecordInfo
                {
                    RecordType = (int)AccountRecordType.Point,
                    Money = 0,
                    Point = -order.Point,
                    Date = DateTime.Now,
                    IP = ClientHelper.IP,
                    Note = "兑取商品：" + order.ProductName + " 订单号：" + order.OrderNumber,
                    UserId = order.UserId,
                    UserName = order.UserName
                });
              
                //扣减库存
                PointProductBLL.ChangeSendCount(order.ProductId, ChangeAction.Plus);
            }

            ResponseHelper.Write("ok|" + order.Id);
            ResponseHelper.End();
        }
    }
}
