using System;
using System.Xml;
using System.Net;
using System.Web;
using System.Web.Security;
using System.IO;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Reflection;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Linq;
using NetDimension.Json;
using System.Media;

namespace JWShop.Web.Admin
{
    public partial class Ajax : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClearCache();

            string action =string.IsNullOrEmpty(RequestHelper.GetQueryString<string>("Action"))?RequestHelper.GetForm<string>("Action"):RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "CheckUserName":
                    CheckUserName();
                    break;
                case "SearchProduct":
                    SearchProduct();
                    break;
                case "SearchGift":
                    SearchGift();
                    break;
                case "ReadChildRegion":
                    int regionID = RequestHelper.GetQueryString<int>("RegionID");
                    ResponseHelper.Write(RegionBLL.SearchRegionList(regionID));
                    ResponseHelper.End();
                    break;
                case "ReadChildProductClass":
                    int classID = RequestHelper.GetQueryString<int>("ProductClassID");
                    ResponseHelper.Write(ProductClassBLL.SearchProductClassList(classID));
                    ResponseHelper.End();
                    break;
                case "AddProductPhoto":
                    AddProductPhoto();
                    break;
                case "DeleteProductPhoto":
                    DeleteProductPhoto();
                    break;
                case "ChangeProductStatus":
                    ChangeProductStatus();
                    break;
                case "ChangeArticleStatus":
                    ChangeArticleStatus();
                    break;
                case"ChangeVoteItemShow":
                    ChangeVoteItemShow();
                    break;
                case "DeleteProduct":
                    DeleteProduct();
                    break;
                case"OffSaleProduct":
                    OffSaleProduct();
                    break;
                case"OnSaleProduct":
                    OnSaleProduct();
                    break;
                case"DeleteProductCoverPhoto":
                    DeleteProductCoverPhoto();
                    break;
                case "DeleteCouponPhoto":
                    DeleteCouponPhoto();
                    break;
                case "DeleteUser":
                    DeleteUser();
                    break;
                case "DeleteOrder":
                    DeleteOrder();
                    break;
                case "RecoverOrder":
                    RecoverOrder();
                    break;
                case "GetPayCount":
                    int orderType = RequestHelper.GetQueryString<int>("orderType");
                    GetPayCount(orderType);
                    break;
                case "GetChartData":
                    int dataType = RequestHelper.GetQueryString<int>("dataType");
                    GetChartData(dataType);
                    break;
                case "GetUserData":
                    int userType = RequestHelper.GetQueryString<int>("dataType");
                    GetUserData(userType);
                    break;
                case "ChangeProClsOrder":
                    int pid = RequestHelper.GetQueryString<int>("pid");
                    int oid = RequestHelper.GetQueryString<int>("oid");
                    ChangeProClsOrder(pid, oid);
                    break;
                case"ChangeArticleClassOrder":
                    ChangeArticleClassOrder();
                    break;
                case "ChangeNavMenuOrder":
                    ChangeNavMenuOrder();
                    break;
                case "ChangeVoteOrder":
                    ChangeVoteOrder();
                    break;
                //case "GetVJINGList":
                //    //pid = RequestHelper.GetQueryString<int>("masterid");
                //    //GetVJINGList(pid);
                //    break;
                case "GetUserCount":
                    GetUserCount();
                    break;
                case "GetOrderCount":
                    GetOrderCount();
                    break;
                case "TestSendEmail":
                    TestSendEmail();
                    break;
                case "GetSaleTotal":
                    GetSaleTotal();
                    break;
                case "DeleteAttribute":
                    DeleteAttribute();
                    break;
                case "DeleteStandard":
                    DeleteStandard();
                    break;
                case"CreateAllPhotos":
                    CreateAllPhotos();
                    break;
                case"MoveDownProductPhoto":
                    MoveDownProductPhoto();
                    break;
                case"MoveUpProductPhoto":
                    MoveUpProductPhoto();
                    break;
                case "MoveDownWechatMenu":
                    MoveDownWechatMenu();
                    break;
                case "MoveUpWechatMenu":
                    MoveUpWechatMenu();
                    break;
                case"UpdateProductName":
                    UpdateProductName();
                    break;
                case"GetProductStandardRecord":
                    GetProductStandardRecord();
                    break;
                case "UpdateIsNoticed":
                    UpdateIsNoticed();
                    break;
                case "CheckOrderNotice":
                    CheckOrderNotice();
                    break;
                case "LoadSound":
                    LoadSound();
                    break;
                case "GroupBuyOrderRefund":
                    GroupBuyOrderRefund();
                    break;
                case "needrefund":
                    NeedRefund();
                    break;
                default:
                    break;
            }
        }

        private void LoadSound()
        {
            //int mtype = RequestHelper.GetQueryString<int>("mtype");
            //string musicsrc = "";
            //if (mtype == 1)
            //    musicsrc = @"/Upload/media/win.mp3";
            //else
            //    musicsrc = @"/Upload/media/win.mp3";

            Response.Write("<audio controls=\"controls\" id=\"audio_player\"  autoplay=\"autoplay\"><source src = '/Upload/media/win.mp3' type = \"audio/mpeg\" ></ audio > ");
            Response.End();
        }

        private void CheckOrderNotice()
        {
            var orderlist = OrderBLL.SearchList(new OrderSearchInfo() { IsNoticed = 0, OrderStatus = (int)OrderStatus.WaitCheck });

            ResponseHelper.Write(orderlist.Count.ToString());
            ResponseHelper.End();
        }
        private void UpdateIsNoticed()
        {
            string result = "ok";
            int orderid = RequestHelper.GetQueryString<int>("orderid");
            int isnoticed = RequestHelper.GetQueryString<int>("isnoticed");
            OrderBLL.UpdateIsNoticed(orderid, isnoticed);

            ResponseHelper.Write(result);
            ResponseHelper.End();
        }

        private void CheckUserName()
        {
            string result = "ok";
            string userName = RequestHelper.GetQueryString<string>("UserName");
            if (!UserBLL.UniqueUser(userName))
            {
                result = "error";
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }

        /// <summary>
        /// 搜索礼品
        /// </summary>
        protected void SearchGift()
        {
            string result = string.Empty;
            var giftSearch = new FavorableActivityGiftSearchInfo();
            giftSearch.Name = RequestHelper.GetQueryString<string>("Name");
            giftSearch.InGiftIds = Array.ConvertAll<string, int>(RequestHelper.GetQueryString<string>("GiftID").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
            var giftList = FavorableActivityGiftBLL.SearchList(giftSearch);
            foreach (var gift in giftList)
            {
                if (string.IsNullOrEmpty(result))
                {
                    result = gift.Id + "|" + gift.Name.Replace("|", "").Replace("#", "");
                }
                else
                {
                    result += "#" + gift.Id + "|" + gift.Name.Replace("|", "").Replace("#", "");
                }
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
        /// <summary>
        /// 搜索产品
        /// </summary>
        protected void SearchProduct()
        {
            string result = string.Empty;
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.Name = RequestHelper.GetQueryString<string>("ProductName");
            productSearch.ClassId = RequestHelper.GetQueryString<string>("ClassID");
            List<ProductInfo> productList = ProductBLL.SearchList(productSearch);
            foreach (ProductInfo product in productList)
            {
                if (result == string.Empty)
                {
                    result = product.Id + "|" + product.Name + "|" + product.Photo;
                }
                else
                {
                    result += "#" + product.Id + "|" + product.Name + "|" + product.Photo;
                }
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
        /// <summary>
        /// 添加产品图片
        /// </summary>
        protected void AddProductPhoto()
        {
            ProductPhotoInfo productPhoto = new ProductPhotoInfo();
            productPhoto.ProductId = RequestHelper.GetQueryString<int>("ProductID");
            productPhoto.Name = RequestHelper.GetQueryString<string>("Name");
            productPhoto.ImageUrl = RequestHelper.GetQueryString<string>("Photo");
            productPhoto.ProStyle = RequestHelper.GetQueryString<int>("ProStyle");
            productPhoto.OrderId = ProductPhotoBLL.GetMaxOrderId(productPhoto.ProductId) + 1;
            int id = ProductPhotoBLL.Add(productPhoto);
            string result = id.ToString() + "|" + productPhoto.Name + "|" + productPhoto.ImageUrl + "|" + productPhoto.ProStyle;
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
        /// <summary>
        /// 删除产品图片
        /// </summary>
        protected void DeleteProductPhoto()
        {
            int proStyle = RequestHelper.GetQueryString<int>("proStyle");
            if (proStyle < 0) { proStyle = 0; }
            ProductPhotoBLL.Delete(RequestHelper.GetQueryString<int>("ProductPhotoID"), proStyle);
            ResponseHelper.End();
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        private void DeleteProduct()
        {
            string result = "ok";
            int productID = RequestHelper.GetQueryString<int>("ProductID");
            List<OrderDetailInfo> odList = OrderDetailBLL.ReadListByProductId(productID);
            if (odList.Count > 0)
            {
                bool canDel = true;
                foreach (OrderDetailInfo myOD in odList)
                {
                    OrderInfo tempOrder = OrderBLL.Read(myOD.OrderId, 0);
                    if (tempOrder.IsDelete == 0)
                    {
                        canDel = false;
                        break;
                    }
                }
                if (!canDel)
                {
                    result = "error";
                }
                else
                {
                    ProductBLL.DeleteLogically(productID);
                }
            }
            else
            {
                ProductBLL.DeleteLogically(productID);
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
        /// <summary>
        /// 下架商品
        /// </summary>
        private void OffSaleProduct() {
            string result = "ok";
            int productID = RequestHelper.GetQueryString<int>("ProductID");
            if (productID > 0)
            {
                var product = ProductBLL.Read(productID);
                if (product.IsSale == (int)BoolType.True)
                {
                    product.IsSale = (int)BoolType.False;
                    ProductBLL.Update(product);
                }
            }
            else {
                result = "error";
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
        /// <summary>
        /// 上架商品
        /// </summary>
        private void OnSaleProduct()
        {
            string result = "ok";
            int productID = RequestHelper.GetQueryString<int>("ProductID");
            if (productID > 0)
            {
                var product = ProductBLL.Read(productID);
                if (product.IsSale == (int)BoolType.False)
                {
                    product.IsSale = (int)BoolType.True;
                    ProductBLL.Update(product);
                }
            }
            else {
                result = "error";
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
        
        /// <summary>
        /// 删除商品主图
        /// </summary>
        private void DeleteProductCoverPhoto() {
            string result = "ok";
            string productPhoto = RequestHelper.GetQueryString<string>("ProductPhoto");
            try
            {
                    if (!string.IsNullOrEmpty(productPhoto)) {                   
                        #region 删除缩略图
                        Dictionary<int, int> dic = new Dictionary<int, int>();
                        foreach (var phototype in PhotoSizeBLL.SearchList((int)PhotoType.Product))
                        {
                            dic.Add(phototype.Width, phototype.Height);
                        }
                        if (!dic.ContainsKey(90)) dic.Add(90, 90);//后台商品列表默认使用尺寸(如果不存在则手动添加)
                        if (dic.Count > 0)
                        {
                            foreach (KeyValuePair<int, int> kv in dic)
                            {
                               string nailPhoto = productPhoto.Replace("Original", kv.Key.ToString() + "-" + kv.Value.ToString());
                               if (System.IO.File.Exists(Server.MapPath(nailPhoto)))
                                {
                                    System.IO.File.Delete(Server.MapPath(nailPhoto));
                                }
                            }
                        }
                        #endregion
                        //删除原图
                        if (System.IO.File.Exists(Server.MapPath(productPhoto)))
                        {
                            System.IO.File.Delete(Server.MapPath(productPhoto));
                        }
                    }
            }
            catch (Exception ex)
            {
                result = "error";
            }
            finally {
                Response.Clear();
                Response.Write(result);
                Response.End();
            }
        }
        /// <summary>
        /// 删除优惠券图片
        /// </summary>
        private void DeleteCouponPhoto()
        {
            string result = "ok";
            string productPhoto = RequestHelper.GetQueryString<string>("photo");
            try
            {
                if (!string.IsNullOrEmpty(productPhoto))
                {
                    //删除原图
                    if (System.IO.File.Exists(Server.MapPath(productPhoto)))
                    {
                        System.IO.File.Delete(Server.MapPath(productPhoto));
                    }
                }
            }
            catch (Exception ex)
            {
                result = "error";
            }
            finally
            {
                Response.Clear();
                Response.Write(result);
                Response.End();
            }
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        private void DeleteUser()
        {
            string result = "ok";
            int userID = RequestHelper.GetQueryString<int>("UserID");
            if (UserAccountRecordBLL.ReadList(userID).Count > 0)
            {
                result = "error";
            }
            else
            {
                UserBLL.Delete(userID);
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        protected void DeleteOrder()
        {
            string result = "ok";
            int orderID = RequestHelper.GetQueryString<int>("OrderID");
            if (orderID > 0)
            {
                OrderInfo order = OrderBLL.Read(orderID, 0);
                if (order.Id > 0 && order.IsDelete == (int)BoolType.False)
                {
                    order.IsDelete = (int)BoolType.True;
                    //order.OrderStatus = (int)OrderStatus.HasDelete;
                    OrderBLL.Update(order);
                    AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Order"), order.Id);
                }
                else
                {
                    result = "订单不存在";
                }
            }
            else
            {
                result = "error";
            }

            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
        /// <summary>
        /// 恢复订单
        /// </summary>
        protected void RecoverOrder()
        {
            string result = "ok";
            int orderID = RequestHelper.GetQueryString<int>("OrderID");
            if (orderID > 0)
            {
                OrderInfo order = OrderBLL.Read(orderID, 0);
                if (order.Id > 0 && order.IsDelete == (int)BoolType.True)
                {
                    order.IsDelete = (int)BoolType.False;                
                    OrderBLL.Update(order);
                    AdminLogBLL.Add(ShopLanguage.ReadLanguage("RecoverRecord"), ShopLanguage.ReadLanguage("Order"), order.Id);
                }
                else
                {
                    result = "订单不存在";
                }
            }
            else
            {
                result = "error";
            }

            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
        
        /// <summary>
        /// 测试邮件发送
        /// </summary>
        protected void TestSendEmail()
        {
            string result = "ok";
            EmailContentInfo emailContent = EmailContentHelper.ReadSystemEmailContent("TestEmail");
            MailInfo mail = new MailInfo();
            mail.ToEmail = RequestHelper.GetQueryString<string>("ToEmail");
            mail.Title = emailContent.EmailTitle;
            mail.Content = emailContent.EmailContent;
            mail.UserName = RequestHelper.GetQueryString<string>("EmailUserName");
            mail.Password = RequestHelper.GetQueryString<string>("EmailPassword");
            mail.Server = RequestHelper.GetQueryString<string>("EmailServer");
            mail.ServerPort = RequestHelper.GetQueryString<int>("EmailServerPort");
            try
            {
                MailClass.SendEmail(mail);
            }
            catch
            {
                result = "error";
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
        /// <summary>
        /// 更新产品的状态
        /// </summary>
        /// <param name="action"></param>
        private void ChangeProductStatus()
        {
            string field = RequestHelper.GetQueryString<string>("field");
            int id = RequestHelper.GetQueryString<int>("ID");
            int status = RequestHelper.GetQueryString<int>("Status");
            switch (field)
            {
                case "IsSpecial":
                    ProductBLL.ChangeProductStatus(id, status, ProductStatusType.IsSpecial);
                    break;
                case "IsNew":
                    ProductBLL.ChangeProductStatus(id, status, ProductStatusType.IsNew);
                    break;
                case "IsHot":
                    ProductBLL.ChangeProductStatus(id, status, ProductStatusType.IsHot);
                    break;
                case "IsSale":
                    ProductBLL.ChangeProductStatus(id, status, ProductStatusType.IsSale);
                    break;
                case "IsTop":
                    ProductBLL.ChangeProductStatus(id, status, ProductStatusType.IsTop);
                    break;
                case "AllowComment":
                    ProductBLL.ChangeProductStatus(id, status, ProductStatusType.AllowComment);
                    break;
                default:
                    break;
            }
            ResponseHelper.Write(field + "|" + id.ToString());
            ResponseHelper.End();
        }
        
        /// <summary>
        /// 更新文章的状态
        /// </summary>
        /// <param name="action"></param>
        private void ChangeArticleStatus()
        {
            string field = RequestHelper.GetQueryString<string>("field");
            int id = RequestHelper.GetQueryString<int>("ID");
            int status = RequestHelper.GetQueryString<int>("Status");
            if (id > 0 && status >= 0 && !string.IsNullOrEmpty(field))
            {
                switch (field)
                {
                    case "IsTop":
                        ArticleBLL.ChangeArticleStatus(id, "[IsTop]", status);
                        break;
                    default:
                        break;
                }
            }
            ResponseHelper.Write(field + "|" + id.ToString());
            ResponseHelper.End();
        }
        /// <summary>
        /// 更改投票选项显示/不显示
        /// </summary>
        protected void ChangeVoteItemShow() {
            int id = RequestHelper.GetQueryString<int>("ID");
            int status = RequestHelper.GetQueryString<int>("Status");
            if (id > 0 && status >= 0)
            {
                VoteItemInfo voteItem = VoteItemBLL.ReadVoteItem(id);
                voteItem.Exp2 = status.ToString();
                VoteItemBLL.UpdateVoteItem(voteItem);
                ResponseHelper.Write("ok|" + id.ToString());
            }
            else
            {
                ResponseHelper.Write("no|" + id.ToString());
            }

            ResponseHelper.End();
        }
        /// <summary>
        /// 后台首页统计 支付金额  付款单数
        /// </summary>
        protected void GetPayCount(int getType)
        {
            string orderCount = string.Empty;
            string orderMoney = string.Empty;
            //全部汇总
            if (getType == 5)
            {
                //付款单数
                orderCount = OrderBLL.StatisticsAllTotal(new OrderSearchInfo()).Rows[0]["OrderCount"].ToString();
                //营业额
                orderMoney = OrderBLL.StatisticsAllTotal(new OrderSearchInfo()).Rows[0]["OrderMoney"].ToString();

            }
            else
            {
                OrderSearchInfo orderSearch = new OrderSearchInfo();
                switch (getType)
                {
                    //今天
                    case 1:
                        orderSearch.StartPayDate = DateTime.Now.Date;
                        orderSearch.EndPayDate = DateTime.Now.Date;
                        orderCount = OrderBLL.StatisticsPaidTotal(orderSearch).Rows[0]["OrderCount"].ToString();
                        orderMoney = OrderBLL.StatisticsPaidTotal(orderSearch).Rows[0]["OrderMoney"].ToString();
                        break;
                        //最近7天
                    case 2:
                        orderSearch.StartPayDate = DateTime.Now.Date.AddDays(-6);
                        orderSearch.EndPayDate = DateTime.Now.Date.AddDays(1);
                        orderCount = OrderBLL.StatisticsPaidTotal(orderSearch).Rows[0]["OrderCount"].ToString();
                        orderMoney = OrderBLL.StatisticsPaidTotal(orderSearch).Rows[0]["OrderMoney"].ToString();
                        break;
                        //1个月
                    case 3:
                        orderSearch.StartPayDate = DateTime.Now.Date.AddDays(-29);
                        orderSearch.EndPayDate = DateTime.Now.Date.AddDays(1);
                        orderCount = OrderBLL.StatisticsPaidTotal(orderSearch).Rows[0]["OrderCount"].ToString();
                        orderMoney = OrderBLL.StatisticsPaidTotal(orderSearch).Rows[0]["OrderMoney"].ToString();
                        break;
                        //最近3个月
                    case 4:
                        orderSearch.StartPayDate = DateTime.Now.Date.AddDays(-89);
                        orderSearch.EndPayDate = DateTime.Now.Date.AddDays(1);
                        orderCount = OrderBLL.StatisticsPaidTotal(orderSearch).Rows[0]["OrderCount"].ToString();
                        orderMoney = OrderBLL.StatisticsPaidTotal(orderSearch).Rows[0]["OrderMoney"].ToString();
                        break;
                }
            }
            if (string.IsNullOrEmpty(orderCount)) orderCount = "0";
            if (string.IsNullOrEmpty(orderMoney)) orderMoney = "0";
            Response.Clear();
            //ResponseHelper.Write("{\"money\": " + orderMoney + "}");

            Response.Write(JsonConvert.SerializeObject(new { ordercount = orderCount, money = orderMoney }));
            ResponseHelper.End();
        }

        protected void GetChartData(int getType)
        {
            string orderCount = string.Empty;
            DataTable saleTable = new DataTable();


            StringBuilder myStr = new StringBuilder();
            myStr.Append("{\"arr\":[");
            OrderSearchInfo orderSearch = new OrderSearchInfo();

            int endDay = 0;
            int startDay = 0;
            if (getType == 1)
            {
                orderSearch.StartAddDate = DateTime.Now.Date;
                orderSearch.EndAddDate = DateTime.Now.Date.AddDays(1);
                saleTable = OrderBLL.StatisticsSaleTotal(orderSearch, DateType.Hour);

                for (int hCount = 0; hCount <= 23; hCount++)
                {
                    bool hasValue = false;
                    string saleCount = string.Empty;
                    foreach (DataRow dr in saleTable.Rows)
                    {
                        if (dr["MyHour"].ToString() == hCount.ToString())
                        {
                            hasValue = true;
                            saleCount = dr["OrderCount"].ToString();
                            break;
                        }
                    }

                    if (hasValue)
                    {
                        if (hCount == 23)
                            myStr.Append(saleCount);
                        else
                            myStr.Append(saleCount + ",");
                    }
                    else
                    {
                        if (hCount == 23)
                            myStr.Append("0");
                        else
                            myStr.Append("0,");
                    }
                }
                myStr.Append("]}");
            }
            else if (getType == 2)
            {

                orderSearch.StartAddDate = DateTime.Now.Date.AddDays(-6);
                orderSearch.EndAddDate = DateTime.Now.Date.AddDays(1);
                saleTable = OrderBLL.StatisticsSaleTotal(orderSearch, DateType.Day);

                int nowDay = DateTime.Now.Day;
                if (nowDay <= 7)
                {
                    endDay = 7;
                    startDay = 1;
                }
                else
                {
                    endDay = DateTime.Now.Day;
                    startDay = DateTime.Now.Day - 6;
                }
                for (int hCount = startDay; hCount <= endDay; hCount++)
                {
                    bool hasValue = false;
                    string saleCount = string.Empty;
                    foreach (DataRow dr in saleTable.Rows)
                    {
                        if (dr["Day"].ToString() == hCount.ToString())
                        {
                            hasValue = true;
                            saleCount = dr["OrderCount"].ToString();
                            break;
                        }
                    }

                    if (hasValue)
                    {
                        if (hCount == endDay)
                            myStr.Append(saleCount);
                        else
                            myStr.Append(saleCount + ",");
                    }
                    else
                    {
                        if (hCount == endDay)
                            myStr.Append("0");
                        else
                            myStr.Append("0,");
                    }
                }
                myStr.Append("]}");
            }
            else if (getType == 3)
            {
                DateTime now = DateTime.Now;
                //本月
                //DateTime d1 = new DateTime(now.Year, now.Month, 1);
                //DateTime d2 = d1.AddMonths(1).AddDays(-1);
                //最近30天
                DateTime d1 = now.AddDays(-29).Date;
                DateTime d2 = now.Date;

                orderSearch.StartAddDate = d1;
                orderSearch.EndAddDate = d2;
                saleTable = OrderBLL.StatisticsSaleTotal(orderSearch, DateType.Day);


                for (; d1.CompareTo(d2) < 1; d1 = d1.AddDays(1))//CompareTo(d2)<1表示d2大于等于d1
                {
                    bool hasValue = false;
                    string saleCount = string.Empty;
                    foreach (DataRow dr in saleTable.Rows)
                    {
                        if (dr["Day"].ToString() == d1.Day.ToString())
                        {
                            hasValue = true;
                            saleCount = dr["OrderCount"].ToString();
                            break;
                        }
                    }

                    if (hasValue)
                    {
                        if (d1.Date == d2.Date)
                            myStr.Append(saleCount);
                        else
                            myStr.Append(saleCount + ",");
                    }
                    else
                    {
                        if (d1.Date == d2.Date)
                            myStr.Append("0");
                        else
                            myStr.Append("0,");
                    }
                }
                myStr.Append("]}");
            }
            else if (getType == 4)
            {
                DateTime now = DateTime.Now;
                DateTime d1 = new DateTime(now.Year, now.Month, 1);
                DateTime d2 = d1.AddMonths(1).AddDays(-1);

                orderSearch.StartAddDate = d1.AddMonths(-2);
                orderSearch.EndAddDate = d2;
                saleTable = OrderBLL.StatisticsSaleTotal(orderSearch, DateType.Month);

                if (now.Month > 3)
                {
                    startDay = now.Month - 2;
                    endDay = now.Month;
                }
                else
                {
                    startDay = 1;
                    endDay = 3;
                }
                for (int hCount = startDay; hCount <= endDay; hCount++)
                {
                    bool hasValue = false;
                    string saleCount = string.Empty;
                    foreach (DataRow dr in saleTable.Rows)
                    {
                        if (dr["Month"].ToString() == hCount.ToString())
                        {
                            hasValue = true;
                            saleCount = dr["OrderCount"].ToString();
                            break;
                        }
                    }

                    if (hasValue)
                    {
                        if (hCount == endDay)
                            myStr.Append(saleCount);
                        else
                            myStr.Append(saleCount + ",");
                    }
                    else
                    {
                        if (hCount == endDay)
                            myStr.Append("0");
                        else
                            myStr.Append("0,");
                    }
                }
                myStr.Append("]}");
            }
            //myStr =(StringBuilder)myStr.ToString().Substring(0, myStr.ToString().Length - 1);


            ResponseHelper.Write(myStr.ToString());
            ResponseHelper.End();
        }

        protected void GetUserData(int getType)
        {
            string orderCount = string.Empty;
            DataTable saleTable = new DataTable();


            StringBuilder myStr = new StringBuilder();
            myStr.Append("{\"arr\":[");
            UserSearchInfo orderSearch = new UserSearchInfo();

            int endDay = 0;
            int startDay = 0;
            if (getType == 5)
            {
                orderSearch.StartRegisterDate = DateTime.Now.Date;
                orderSearch.EndRegisterDate = DateTime.Now.Date.AddDays(1);
                saleTable = UserBLL.StatisticsUserCount(orderSearch, DateType.Hour);

                for (int hCount = 0; hCount <= 23; hCount++)
                {
                    bool hasValue = false;
                    string saleCount = string.Empty;
                    foreach (DataRow dr in saleTable.Rows)
                    {
                        if (dr["MyHour"].ToString() == hCount.ToString())
                        {
                            hasValue = true;
                            saleCount = dr["Count"].ToString();
                            break;
                        }
                    }

                    if (hasValue)
                    {
                        if (hCount == 23)
                            myStr.Append(saleCount);
                        else
                            myStr.Append(saleCount + ",");
                    }
                    else
                    {
                        if (hCount == 23)
                            myStr.Append("0");
                        else
                            myStr.Append("0,");
                    }
                }
                myStr.Append("]}");
            }
            else if (getType == 6)
            {

                orderSearch.StartRegisterDate = DateTime.Now.Date.AddDays(-6);
                orderSearch.EndRegisterDate = DateTime.Now.Date.AddDays(1);
                saleTable = UserBLL.StatisticsUserCount(orderSearch, DateType.Day);

                int nowDay = DateTime.Now.Day;
                if (nowDay <= 7)
                {
                    endDay = 7;
                    startDay = 1;
                }
                else
                {
                    endDay = DateTime.Now.Day;
                    startDay = DateTime.Now.Day - 6;
                }
                for (int hCount = startDay; hCount <= endDay; hCount++)
                {
                    bool hasValue = false;
                    string saleCount = string.Empty;
                    foreach (DataRow dr in saleTable.Rows)
                    {
                        if (dr["Day"].ToString() == hCount.ToString())
                        {
                            hasValue = true;
                            saleCount = dr["Count"].ToString();
                            break;
                        }
                    }

                    if (hasValue)
                    {
                        if (hCount == endDay)
                            myStr.Append(saleCount);
                        else
                            myStr.Append(saleCount + ",");
                    }
                    else
                    {
                        if (hCount == endDay)
                            myStr.Append("0");
                        else
                            myStr.Append("0,");
                    }
                }
                myStr.Append("]}");
            }
            else if (getType == 7)
            {
                DateTime now = DateTime.Now;
                //本月
                //DateTime d1 = new DateTime(now.Year, now.Month, 1);
                //DateTime d2 = d1.AddMonths(1).AddDays(-1);
                //最近30天
                DateTime d1 = now.AddDays(-29).Date;
                DateTime d2 = now.Date;
                orderSearch.StartRegisterDate = d1;
                orderSearch.EndRegisterDate = d2;
                saleTable = UserBLL.StatisticsUserCount(orderSearch, DateType.Day);


                for (; d1.CompareTo(d2) < 1; d1 = d1.AddDays(1))//CompareTo(d2)<1表示d2大于等于d1
                {
                    bool hasValue = false;
                    string saleCount = string.Empty;
                    foreach (DataRow dr in saleTable.Rows)
                    {
                        if (dr["Day"].ToString() == d1.Day.ToString())
                        {
                            hasValue = true;
                            saleCount = dr["Count"].ToString();
                            break;
                        }
                    }

                    if (hasValue)
                    {
                        if (d1.Date == d2.Date)
                            myStr.Append(saleCount);
                        else
                            myStr.Append(saleCount + ",");
                    }
                    else
                    {
                        if (d1.Date == d2.Date)
                            myStr.Append("0");
                        else
                            myStr.Append("0,");
                    }
                }
                myStr.Append("]}");
            }
            else if (getType == 8)
            {
                DateTime now = DateTime.Now;
                DateTime d1 = new DateTime(now.Year, now.Month, 1);
                DateTime d2 = d1.AddMonths(1).AddDays(-1);

                orderSearch.StartRegisterDate = d1.AddMonths(-2);
                orderSearch.EndRegisterDate = d2;
                saleTable = UserBLL.StatisticsUserCount(orderSearch, DateType.Month);

                if (now.Month > 3)
                {
                    startDay = now.Month - 2;
                    endDay = now.Month;
                }
                else
                {
                    startDay = 1;
                    endDay = 3;
                }

                for (int hCount = startDay; hCount <= endDay; hCount++)
                {
                    bool hasValue = false;
                    string saleCount = string.Empty;
                    foreach (DataRow dr in saleTable.Rows)
                    {
                        if (dr["Month"].ToString() == hCount.ToString())
                        {
                            hasValue = true;
                            saleCount = dr["Count"].ToString();
                            break;
                        }
                    }

                    if (hasValue)
                    {
                        if (hCount == endDay)
                            myStr.Append(saleCount);
                        else
                            myStr.Append(saleCount + ",");
                    }
                    else
                    {
                        if (hCount == endDay)
                            myStr.Append("0");
                        else
                            myStr.Append("0,");
                    }
                }
                myStr.Append("]}");
            }
            //myStr =(StringBuilder)myStr.ToString().Substring(0, myStr.ToString().Length - 1);


            ResponseHelper.Write(myStr.ToString());
            ResponseHelper.End();
        }
        /// <summary>
        /// 修改商品分类排序
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="oid"></param>
        protected void ChangeProClsOrder(int pid, int oid)
        {
            ProductClassBLL.ChangeProductClassOrder(pid, oid);
        }
        /// <summary>
        /// 修改导航菜单排序
        /// </summary>
        protected void ChangeNavMenuOrder() {
            int menuId = RequestHelper.GetQueryString<int>("id");
            int orderId = RequestHelper.GetQueryString<int>("oid");
            if (menuId > 0 && orderId >= 0) {
                NavMenuBLL.ChangeNavMenuOrder(menuId, orderId);
            }
        }
        /// <summary>
        /// 修改文章分类排序
        /// </summary>
        protected void ChangeArticleClassOrder(){
            int classId = RequestHelper.GetQueryString<int>("id");
            int orderId = RequestHelper.GetQueryString<int>("oid");
            if (classId > 0 && orderId >= 0)
            {
                ArticleClassBLL.ChangeArticleClassOrder(classId, orderId);
            }
        }
        //protected void AddProductTypeAttribute()
        //{
        //    string name = RequestHelper.GetForm<string>("Name");
        //    int classID = RequestHelper.GetForm<int>("AttributeClassID");
        //    int inputType = RequestHelper.GetForm<int>("InputType");
        //    string inputValue = RequestHelper.GetForm<string>("InputValue");
        //    //int orderID = RequestHelper.GetForm<int>("OrderID");

        //    string result = "ok";
        //    AttributeInfo attribute = new AttributeInfo();
        //    attribute.Name = name;
        //    attribute.AttributeClassID = classID;
        //    attribute.InputType = inputType;
        //    attribute.InputValue = inputValue;
        //    attribute.OrderID = 0;
        //    attribute.IsForSearch = 0;

        //    if (AttributeBLL.AddAttribute(attribute) <= 0)
        //    {
        //        result = "error";
        //    }

        //    Response.Write(result);
        //    Response.End();
        //}
        /// <summary>
        /// 修改投票类型排序号
        /// </summary>
        protected void ChangeVoteOrder()
        {
            int voteId = RequestHelper.GetQueryString<int>("voteId");
            int orderId = RequestHelper.GetQueryString<int>("orderId");
            if (voteId > 0 && orderId >= 0)
            {
                var vote = VoteBLL.ReadVote(voteId);
                vote.OrderID = orderId;
                VoteBLL.UpdateVote(vote);
                CacheHelper.Remove("Vote");
            }
        }
        #region 帮助中心
        /*
        protected void GetVJINGList(int pid)
        {
            if (pid > 0)
            {
                string result = string.Empty;
                #region 读取VHNJING内容
                string vconstr = "server=175.6.5.68,1433;pwd=sasfDSHAsdgh;uid=hnjing.cn;database=CasePlatform;";
                SqlConnection conn = new SqlConnection(vconstr);
                SqlDataAdapter adp = new SqlDataAdapter("select * from j_class where c_parentid=" + pid + " order by c_orderid asc", conn);
                DataSet dataset = new DataSet();
                adp.Fill(dataset);

                foreach (DataRow dt in dataset.Tables[0].Rows)
                {
                    if (pid == 83)
                    {
                        result += "<dd class=\"ease\"><a href=\"javascript:goUrl('Popularize.aspx?CID=" + dt["C_ID"] + "',0,0)\" title=\"" + dt["C_ClassName"] + "\">" + StringHelper.Substring(dt["C_ClassName"].ToString(), 4) + "</a></dd>\n";
                    }
                    else
                    {
                        result += "<dd class=\"ease\"><a href=\"javascript:goUrl('Helps.aspx?CID=" + dt["C_ID"] + "',0,0)\" title=\"" + dt["C_ClassName"] + "\">" + StringHelper.Substring(dt["C_ClassName"].ToString(), 4) + "</a></dd>\n";
                    }
                }
                conn.Close();
                conn.Dispose();

                Response.Write(result);
                Response.End();
                #endregion
            }
        }
        */
        #endregion
        protected void GetUserCount()
        {
            int year = 0;
            int month = 0;
            int days = 0;
            Dictionary<int, int> userCountDic = new Dictionary<int, int>();
            string date = RequestHelper.GetQueryString<string>("Date");
            year = Convert.ToInt32(date.Split('|')[0]);
            month = Convert.ToInt32(date.Split('|')[1]);
            UserSearchInfo userSearch = new UserSearchInfo();
            DateType dateType = DateType.Day;
            if (month == int.MinValue)
            {
                dateType = DateType.Month;
                userSearch.StartRegisterDate = Convert.ToDateTime(year + "-01-01");
                userSearch.EndRegisterDate = Convert.ToDateTime(year + "-01-01").AddYears(1);
            }
            else
            {
                days = ShopCommon.CountMonthDays(year, month);
                userSearch.StartRegisterDate = Convert.ToDateTime(year + "-" + month + "-01");
                userSearch.EndRegisterDate = Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1);
            }

            string jsonStr = "[";

            DataTable dt = UserBLL.StatisticsUserCount(userSearch, dateType);
            if (month == int.MinValue)
            {
                for (int i = 1; i <= 12; i++)
                {
                    int countNum = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr[0]) == i)
                        {
                            countNum = Convert.ToInt32(dr[1]);
                        }
                    }

                    jsonStr += countNum + ",";
                }
            }
            else
            {
                for (int i = 1; i <= days; i++)
                {
                    int countNum = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr[0]) == i)
                        {
                            countNum = Convert.ToInt32(dr[1]);
                        }
                    }

                    jsonStr += countNum + ",";
                }
            }
            jsonStr = jsonStr.Substring(0, jsonStr.Length - 1) + "]";
            Response.Write(jsonStr);
            Response.End();
        }
        

        protected void GetOrderCount()
        {
            int year = 0;
            int month = 0;
            int days = 0;
            Dictionary<int, int> orderCountDic = new Dictionary<int, int>();
            string date = RequestHelper.GetQueryString<string>("Date");
            year = Convert.ToInt32(date.Split('|')[0]);
            month = Convert.ToInt32(date.Split('|')[1]);
            OrderSearchInfo orderSearch = new OrderSearchInfo();
            DateType dateType = DateType.Day;
            if (month == int.MinValue)
            {
                dateType = DateType.Month;
                orderSearch.StartAddDate = Convert.ToDateTime(year + "-01-01");
                orderSearch.EndAddDate = Convert.ToDateTime(year + "-01-01").AddYears(1);
            }
            else
            {
                days = ShopCommon.CountMonthDays(year, month);
                orderSearch.StartAddDate = Convert.ToDateTime(year + "-" + month + "-01");
                orderSearch.EndAddDate = Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1);
            }
            DataTable dt = OrderBLL.StatisticsOrderCount(orderSearch, dateType);
            foreach (DataRow dr in dt.Rows)
            {
                orderCountDic.Add(Convert.ToInt32(dr[0]), Convert.ToInt32(dr[1]));
            }

            string jsonStr = "[";

            if (month == int.MinValue)
            {
                for (int i = 1; i <= 12; i++)
                {
                    int countNum = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr[0]) == i)
                        {
                            countNum = Convert.ToInt32(dr[1]);
                        }
                    }

                    jsonStr += countNum + ",";
                }
            }
            else
            {
                for (int i = 1; i <= days; i++)
                {
                    int countNum = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr[0]) == i)
                        {
                            countNum = Convert.ToInt32(dr[1]);
                        }
                    }

                    jsonStr += countNum + ",";
                }
            }
            jsonStr = jsonStr.Substring(0, jsonStr.Length - 1) + "]";
            Response.Write(jsonStr);
            Response.End();
        }

        protected void GetSaleTotal()
        {
            string date = RequestHelper.GetQueryString<string>("Date");
            int days = 0;
            int year = Convert.ToInt32(date.Split('|')[0]);
            int month = Convert.ToInt32(date.Split('|')[1]);
            OrderSearchInfo orderSearch = new OrderSearchInfo();
            DateType dateType = DateType.Day;
            if (month == int.MinValue)
            {
                dateType = DateType.Month;
                orderSearch.StartAddDate = Convert.ToDateTime(year + "-01-01");
                orderSearch.EndAddDate = Convert.ToDateTime(year + "-01-01").AddYears(1);
            }
            else
            {
                days = ShopCommon.CountMonthDays(year, month);
                orderSearch.StartAddDate = Convert.ToDateTime(year + "-" + month + "-01");
                orderSearch.EndAddDate = Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1);
            }
            DataTable dt = OrderBLL.StatisticsSaleTotal(orderSearch, dateType);


            string jsonStr = string.Empty;
            string countStr = string.Empty;
            string moneyStr = string.Empty;
            if (month == int.MinValue)
            {
                for (int i = 1; i <= 12; i++)
                {
                    int countNum = 0;
                    int moneyNum = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr[0]) == i)
                        {
                            countNum = Convert.ToInt32(dr[1]);
                            moneyNum = Convert.ToInt32(dr[2]);
                        }
                    }
                    countStr += countNum + ",";
                    moneyStr += moneyNum + ",";
                }
            }
            else
            {
                for (int i = 1; i <= days; i++)
                {
                    int countNum = 0;
                    int moneyNum = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr[0]) == i)
                        {
                            countNum = Convert.ToInt32(dr[1]);
                            moneyNum = Convert.ToInt32(dr[2]);
                        }
                    }
                    countStr += countNum + ",";
                    moneyStr += moneyNum + ",";
                }
            }
            countStr = countStr.Substring(0, countStr.Length - 1);
            countStr = "{\"name\":\"订单量\",\"type\":\"line\",\"itemStyle\": {\"normal\":{\"color\":\"#00ff00\"}},\"data\":[" + countStr + "]}";
            moneyStr = moneyStr.Substring(0, moneyStr.Length - 1);
            moneyStr = "{\"name\":\"销售额\",\"type\":\"line\",\"itemStyle\": {\"normal\":{\"color\":\"#ff0000\"}},\"data\":[" + moneyStr + "]}";
            jsonStr = "[" + countStr + "," + moneyStr + "]";

            Response.Write(jsonStr);
            Response.End();
        }

        protected void DeleteAttribute()
        {
            string result = "ok";
            int atid = RequestHelper.GetQueryString<int>("atid");
            ProductTypeAttributeBLL.Delete(atid);//删除属性
            ProductTypeAttributeRecordBLL.DeleteByAttr(atid);//删除所有此属性的属性记录

            Response.Write(result);
            Response.End();
        }

        protected void DeleteStandard()
        {
            string result = "ok";
            int skuid = RequestHelper.GetQueryString<int>("skuid");
            ProductTypeStandardBLL.Delete(skuid);//删除规格
            ProductTypeStandardRecordBLL.DeleteByStandardId(skuid);//删除所有此规格产生的规格记录

            Response.Write(result);
            Response.End();
        }
        /// <summary>
        ///  生成本站文章、产品所有缩略图、水印图
        /// </summary>
        protected void CreateAllPhotos() {
            try
            {
            
                #region 处理文章图片
                List<ArticleInfo> allArticleList = new List<ArticleInfo>();
                allArticleList = ArticleBLL.SearchList(new ArticleSearchInfo { });
                foreach (var item in allArticleList)                {
                   
                   
                    //如果图片不为空，且存在此路径才操作
                    if (!string.IsNullOrEmpty(item.Photo) && (System.IO.File.Exists(ServerHelper.MapPath(item.Photo)) || System.IO.File.Exists(ServerHelper.MapPath(item.Photo.Replace("_wm", "")))))
                    {
                        //无水印原图
                       string originalPhoto = item.Photo.Replace("_wm", "");
                        //带水印原图
                       string waterOriginalPhoto = string.Empty;
                        
                        //文件扩展名
                        string photoExt = originalPhoto.Substring(originalPhoto.LastIndexOf("."));
                        //文件路径
                        string photoPath = originalPhoto.Substring(0,originalPhoto.LastIndexOf("/")+1);
                        //文件名
                        string photoName = originalPhoto.Substring(originalPhoto.LastIndexOf("/")+1);
                        photoName = photoName.Substring(0, photoName.LastIndexOf("."));
                        //水印图片全路径
                        waterOriginalPhoto = photoPath + photoName + "_wm" + photoExt;

                        RecreateItemPhoto(item.Photo);
                        ArticleBLL.Update(item);
                        //生成缩略图
                        //缩略图尺寸列表
                        Dictionary<int, int> dic = new Dictionary<int, int>();
                        foreach (var phototype in PhotoSizeBLL.SearchList((int)PhotoType.Article))
                        {
                            dic.Add(phototype.Width, phototype.Height);
                        }
                        RecreateThumbnail(item.Photo, waterOriginalPhoto, dic);
                 

                    }
                }
                #endregion
                #region 处理商品图片
                List<ProductInfo> allProductList = new List<ProductInfo>();
                allProductList = ProductBLL.SearchList(new ProductSearchInfo {IsSale=1});
                foreach (var item in allProductList)
                {
                   
                    //如果图片不为空，且存在此路径才操作
                    if (!string.IsNullOrEmpty(item.Photo) && (System.IO.File.Exists(ServerHelper.MapPath(item.Photo)) || System.IO.File.Exists(ServerHelper.MapPath(item.Photo.Replace("_wm", "")))))
                    {
                        //无水印原图
                        string originalPhoto = item.Photo.Replace("_wm", "");
                        //带水印原图
                        string waterOriginalPhoto = string.Empty;
                       
                        //文件扩展名
                        string photoExt = originalPhoto.Substring(originalPhoto.LastIndexOf("."));
                        //文件路径
                        string photoPath = originalPhoto.Substring(0, originalPhoto.LastIndexOf("/") + 1);
                        //文件名
                        string photoName = originalPhoto.Substring(originalPhoto.LastIndexOf("/") + 1);
                        photoName = photoName.Substring(0, photoName.LastIndexOf("."));
                        //水印图片全路径
                        waterOriginalPhoto = photoPath + photoName + "_wm" + photoExt;
                                               
                        item.Photo = RecreateItemPhoto(item.Photo);
                        ProductBLL.Update(item);
                        //生成缩略图

                        //缩略图尺寸列表
                        Dictionary<int, int> dic = new Dictionary<int, int>();
                        foreach (var phototype in PhotoSizeBLL.SearchList((int)PhotoType.Product)) {
                            dic.Add(phototype.Width, phototype.Height);
                        }
                        if (!dic.ContainsKey(90)) dic.Add(90, 90);//后台商品列表默认使用尺寸(如果不存在则手动添加)
                        RecreateThumbnail(item.Photo, waterOriginalPhoto, dic);

                    }
                }
                #endregion
                #region 处理productphoto（商品图集）
                List<ProductPhotoInfo> allProductPhotos = ProductPhotoBLL.ReadAllProductPhotos();
                foreach (var tmpProduct in allProductList)
                {
                    //foreach (var item in ProductPhotoBLL.ReadList(tmpProduct.Id, 0))
                    foreach (var item in allProductPhotos.Where(p => p.ProductId == tmpProduct.Id))
                    {
                     
                    //如果图片不为空，且存在此路径才操作
                    if (!string.IsNullOrEmpty(item.ImageUrl) && (System.IO.File.Exists(ServerHelper.MapPath(item.ImageUrl.Replace("75-75", "Original"))) || System.IO.File.Exists(ServerHelper.MapPath(item.ImageUrl.Replace("75-75", "Original").Replace("_wm", "")))))
                    {
                        //无水印原图
                        string originalPhoto = item.ImageUrl.Replace("75-75", "Original").Replace("_wm", "");
                        //带水印原图
                        string waterOriginalPhoto = string.Empty;
                        //删除上水印的老图
                        if (item.ImageUrl.IndexOf("_wm") > -1 && System.IO.File.Exists(ServerHelper.MapPath(item.ImageUrl.Replace("75-75", "Original"))))
                        {
                            System.IO.File.Delete(ServerHelper.MapPath(item.ImageUrl.Replace("75-75", "Original")));
                        }
                        //文件扩展名
                        string photoExt = originalPhoto.Substring(originalPhoto.LastIndexOf("."));
                        //文件路径
                        string photoPath = originalPhoto.Substring(0, originalPhoto.LastIndexOf("/") + 1);
                        //文件名
                        string photoName = originalPhoto.Substring(originalPhoto.LastIndexOf("/") + 1);
                        photoName = photoName.Substring(0, photoName.LastIndexOf("."));
                        //水印图片全路径
                        waterOriginalPhoto = photoPath + photoName + "_wm" + photoExt;

                      
                        item.ImageUrl = RecreateItemPhoto(item.ImageUrl.Replace("75-75", "Original")).Replace("Original","75-75");
                        ProductPhotoBLL.Update(item);
                        //生成缩略图
                        string originalFile = item.ImageUrl.Replace("75-75", "Original");
                        //缩略图尺寸列表
                        Dictionary<int, int> dic = new Dictionary<int, int>();
                        
                        foreach (var phototype in PhotoSizeBLL.SearchList((int)PhotoType.ProductPhoto))
                        {
                            dic.Add(phototype.Width, phototype.Height);
                        }
                        if (!dic.ContainsKey(75)) dic.Add(75, 75);//后台商品图集默认使用尺寸(如果不存在则手动添加)
                        RecreateThumbnail(originalFile, waterOriginalPhoto, dic);
                       
                    }
                    }
                }
                #endregion
                Response.Clear();
                ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = "ok", msg = "操作成功" }));
            }
            catch (Exception ex)
            {
                Response.Clear();
                //ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = "no", msg = "操作失败" }));
                ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = "no", msg = ex.ToString() }));
            }
            finally
            {
                ResponseHelper.End();
            }
        }
        /// <summary>
        /// 根据文章/产品原封面图重新生成封面图
        /// </summary>
        /// <param name="itemPhoto">原封面图</param>
        /// <returns>生成封面图</returns>
        protected string RecreateItemPhoto(string itemPhoto) {
            string result = itemPhoto;
            UploadHelper upload = new UploadHelper();

            upload.FileNameType = FileNameType.Guid;
            upload.MaxWidth = ShopConfig.ReadConfigInfo().AllImageWidth;//整站图片压缩开启后的压缩宽度
            upload.AllImageIsNail = ShopConfig.ReadConfigInfo().AllImageIsNail;//整站图片压缩开关

            FileInfo file = null;
            int waterType = ShopConfig.ReadConfigInfo().WaterType;
              //无水印原图
            string originalPhoto = itemPhoto.Replace("_wm", "");
          
            //删除上水印的老图
            if (itemPhoto.IndexOf("_wm") > -1 && System.IO.File.Exists(ServerHelper.MapPath(itemPhoto)))
            {
                System.IO.File.Delete(ServerHelper.MapPath(itemPhoto));
            }
            //文件扩展名
            string photoExt = originalPhoto.Substring(originalPhoto.LastIndexOf("."));
            //文件路径
            string photoPath = originalPhoto.Substring(0, originalPhoto.LastIndexOf("/") + 1);
            //文件名
            string photoName = originalPhoto.Substring(originalPhoto.LastIndexOf("/") + 1);
            photoName = photoName.Substring(0, photoName.LastIndexOf("."));
         
            if (waterType == 2 || waterType == 3)
            {
                string needMark = RequestHelper.GetQueryString<string>("NeedMark");
                if (needMark == string.Empty || needMark == "1")
                {
                    int waterPossition = ShopConfig.ReadConfigInfo().WaterPossition;
                    string text = ShopConfig.ReadConfigInfo().Text;
                    string textFont = ShopConfig.ReadConfigInfo().TextFont;
                    int textSize = ShopConfig.ReadConfigInfo().TextSize;
                    string textColor = ShopConfig.ReadConfigInfo().TextColor;
                    string waterPhoto = Server.MapPath(ShopConfig.ReadConfigInfo().WaterPhoto);

                    file = upload.SaveAs(waterType, waterPossition, text, textFont, textSize, textColor, waterPhoto, photoPath, photoExt, photoName);
                    //生成新的图片并保存到数据库
                    result = photoPath + file.Name;

                }
            }
            else
            {
                result = itemPhoto.Replace("_wm", "");

            }
            return result;
        }
        /// <summary>
        /// 根据原图和缩略图尺寸生成缩略图
        /// </summary>
        /// <param name="originalPhoto">原图</param>
        /// <param name="waterOriginalPhoto">原水印图</param>
        /// <param name="dic">缩略图尺寸列表</param>
        protected void RecreateThumbnail(string originalFile,string waterOriginalPhoto, Dictionary<int, int> dic)
        {
            
            string makeFile = string.Empty;
            if (dic.Count > 0)
            {
                foreach (KeyValuePair<int, int> kv in dic)
                {
                    makeFile = originalFile.Replace("Original", kv.Key.ToString() + "-" + kv.Value.ToString());


                    if (makeFile.IndexOf("_wm") > -1)
                    {

                        if (System.IO.File.Exists(ServerHelper.MapPath(makeFile.Replace("_wm", ""))))
                        {
                            System.IO.File.Delete(ServerHelper.MapPath(makeFile.Replace("_wm", "")));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(waterOriginalPhoto))
                        {
                            string waterFile = waterOriginalPhoto.Replace("Original", kv.Key.ToString() + "-" + kv.Value.ToString());
                            if (System.IO.File.Exists(ServerHelper.MapPath(waterFile)))
                            {
                                System.IO.File.Delete(ServerHelper.MapPath(waterFile));
                            }
                        }
                    }
                    if (System.IO.File.Exists(ServerHelper.MapPath(makeFile)))
                    {
                        System.IO.File.Delete(ServerHelper.MapPath(makeFile));
                    }
                    ImageHelper.MakeThumbnailImage(ServerHelper.MapPath(originalFile), ServerHelper.MapPath(makeFile), kv.Key, kv.Value, ThumbnailType.InBox);
                }

            }
        }
        /// <summary>
        /// 商品图集下移一位
        /// </summary>
        protected void  MoveDownProductPhoto() {
            try
            {             
                Response.Clear();
                int photoId = RequestHelper.GetQueryString<int>("productPhotoId");
                if (photoId > 0)
                {

                    ProductPhotoBLL.MoveDownProductPhoto(photoId);
                    ProductPhotoInfo productPhoto = ProductPhotoBLL.Read(photoId, 0);
                    List<ProductPhotoInfo> productPhotoList = ProductPhotoBLL.ReadList(productPhoto.ProductId,0);
                    Response.Write(JsonConvert.SerializeObject(new{flag=true,dataList=productPhotoList}));

                }
                else {
                    Response.Write(JsonConvert.SerializeObject(new { flag = false}));
                }
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { flag = false }));
            }
            finally {
                Response.End();
            }
        }
        /// <summary>
        /// 商品图集前移一位
        /// </summary>
        protected void MoveUpProductPhoto()
        {
            try
            {
                Response.Clear();
                int photoId = RequestHelper.GetQueryString<int>("productPhotoId");
                if (photoId > 0)
                {

                    ProductPhotoBLL.MoveUpProductPhoto(photoId);
                    ProductPhotoInfo productPhoto = ProductPhotoBLL.Read(photoId, 0);
                    List<ProductPhotoInfo> productPhotoList = ProductPhotoBLL.ReadList(productPhoto.ProductId, 0);
                    Response.Write(JsonConvert.SerializeObject(new { flag = true, dataList = productPhotoList }));
                }
                else
                {
                    Response.Write(JsonConvert.SerializeObject(new { flag = false }));
                }
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { flag = false }));
            }
            finally
            {
                Response.End();
            }
        }

        /// <summary>
        /// 微信菜单下移一位
        /// </summary>
        protected void MoveDownWechatMenu()
        {
            try
            {
                Response.Clear();
                int id = RequestHelper.GetQueryString<int>("id");
                if (id > 0)
                {
                    WechatMenuBLL.MoveDownWechatMenu(id);
                
                    Response.Write("ok");

                }
                else
                {
                    Response.Write("error");
                }
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.Write("error");
            }
            finally
            {
                Response.End();
            }
        }
        /// <summary>
        /// 微信菜单下移一位
        /// </summary>
        protected void MoveUpWechatMenu()
        {
            try
            {
                Response.Clear();
                int id = RequestHelper.GetQueryString<int>("id");
                if (id > 0)
                {
                    WechatMenuBLL.MoveUpWechatMenu(id);
                    Response.Write("ok");

                }
                else
                {
                    Response.Write("error");
                }
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.Write("error");
            }
            finally
            {
                Response.End();
            }
        }
        /// <summary>
        /// 修改商品名称
        /// </summary>
        protected void UpdateProductName() {
            string result = "ok";
            try
            {
                int id = RequestHelper.GetQueryString<int>("ProductId");
                string name = Server.UrlDecode(RequestHelper.GetQueryString<string>("ProductName"));
                if (id > 0 && !string.IsNullOrEmpty(name))
                {
                    if (name.Length > 40)
                    {
                        result = "error|商品名称不能超过40字";
                    }
                    else
                    {
                        ProductInfo product = ProductBLL.Read(id);
                        product.Name = name;
                        ProductBLL.Update(product);
                    }
                }
            }
            catch (Exception ex)
            {
                result = "error";
            }
            finally {
                Response.Clear();
                ResponseHelper.Write(result);
                ResponseHelper.End();
            }
        }
        /// <summary>
        /// 获取商品规格值
        /// </summary>
        protected void GetProductStandardRecord() {
            int productID = RequestHelper.GetQueryString<int>("productId");
            int standardType = RequestHelper.GetQueryString<int>("standardType");
            List<ProductTypeStandardRecordInfo> _standardRecordList = new List<ProductTypeStandardRecordInfo>();
            if (productID > 0 && standardType==(int)ProductStandardType.Single)
            {
                _standardRecordList = ProductTypeStandardRecordBLL.ReadListByProduct(productID, standardType);
            }

            ResponseHelper.Write(JsonConvert.SerializeObject(new { count = _standardRecordList.Count, dataList = _standardRecordList.Take(5) }));
            ResponseHelper.End();
        }
        #region 团购订单后台退款
        /// <summary>
        /// 团购订单退款
        /// </summary>
        private void GroupBuyOrderRefund()
        {
            int orderId = RequestHelper.GetForm<int>("orderId");
            if (orderId <= 0)
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "无效的操作" }));
                Response.End();
            }
            var order = OrderBLL.Read(orderId);
            #region 添加退款申请
            //如果是团购单，且拼团正在进行中，暂不能申请退款
            if (order.IsActivity == (int)OrderKind.GroupBuy && order.FavorableActivityId > 0)
            {
                var groupBuy = GroupBuyBLL.Read(order.FavorableActivityId);
                if (groupBuy.StartTime <= DateTime.Now && groupBuy.EndTime >= DateTime.Now && groupBuy.Quantity > groupBuy.SignCount)
                {
                    Response.Clear();
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "拼团正在进行，暂不能退款" }));
                    Response.End();
                }
            }

            //正在处理中的退款订单或商品
            var orderRefundList = OrderRefundBLL.ReadListValid(order.Id);
            //有正在处理中的退款订单或商品
            if (orderRefundList.Count(k => !OrderRefundBLL.HasReturn(k.Status) && k.UserId != 0 && k.RefundRemark != "拼团失败，系统自动退款") > 0)
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "该订单有正在处理中的退款" }));
                Response.End();
            }
            //自动匹配到上次系统自动退款的记录
            OrderRefundInfo orderRefund = orderRefundList.Find(k => !OrderRefundBLL.HasReturn(k.Status) && k.UserId == 0 && k.RefundRemark == "拼团失败，系统自动退款") ?? new OrderRefundInfo();
            //如果之前没有退款记录则新增退款申请记录
            if (orderRefund.Id <= 0)
            {
                orderRefund.RefundNumber = ShopCommon.CreateOrderRefundNumber();
                orderRefund.OrderId = orderId;
                orderRefund.Status = (int)OrderRefundStatus.Submit;
                orderRefund.TmCreate = DateTime.Now;
                orderRefund.RefundRemark = "拼团失败，系统自动退款";
                orderRefund.UserType = 1;
                orderRefund.UserId = 0;
                orderRefund.UserName = "";

                //默认退全部能退的额度
                var refundMsg = JWRefund.VerifySubmitOrderRefund(orderRefund, JWRefund.CanRefund(order).CanRefundMoney);
                if (refundMsg.CanRefund)
                {
                    int refundId = OrderRefundBLL.Add(orderRefund);
                    orderRefund.Id = refundId;

                    OrderRefundActionBLL.Add(new OrderRefundActionInfo
                    {
                        OrderRefundId = refundId,
                        Status = (int)BoolType.True,
                        Tm = DateTime.Now,
                        UserType = 1,
                        UserId = 0,
                        UserName = "",
                        Remark = "拼团失败，系统自动退款"
                    });
                }
                else
                {
                    Response.Clear();
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = refundMsg.ErrorCodeMsg }));
                    Response.End();
                }
            }
            #endregion
            #region 审核通过   
            if (orderRefund.Status == (int)OrderRefundStatus.Submit)
            {
                orderRefund.Status = (int)OrderRefundStatus.Approve;
                orderRefund.Remark = "系统审核通过，等待处理退款： ";
                OrderRefundBLL.Update(orderRefund);
                //退款操作记录
                AddOrderRefundAction(orderRefund, (int)BoolType.True);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("OrderRefund"), orderRefund.Id);
            }
            #endregion
            #region 退款处理
            //更改状态为退款中...
            if (orderRefund.Status == (int)OrderRefundStatus.Approve)
            {
                orderRefund.Status = (int)OrderRefundStatus.Returning;
                orderRefund.Remark = "正在处理退款";

                OrderRefundBLL.Update(orderRefund);

                //退款操作记录
                AddOrderRefundAction(orderRefund, (int)BoolType.True);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("OrderRefund"), orderRefund.Id);
            }

            //退款到账户余额及各支付渠道
            if (orderRefund.Status == (int)OrderRefundStatus.Returning)
            {

                string tradeNo = order.WxPayTradeNo;
                if (string.IsNullOrEmpty(tradeNo))
                {
                    Response.Clear();
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "无效的微信支付交易号" }));
                    Response.End();
                }

                decimal totalMoney = order.ProductMoney - order.FavorableMoney + order.ShippingMoney + order.OtherMoney - order.Balance - order.CouponMoney - order.PointMoney;
                if (orderRefund.RefundMoney > totalMoney)
                {
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "退款金额不能超过订单金额" }));
                    Response.End();
                }

                //商户退款单号
                //商户系统内部的退款单号，商户系统内部唯一，同一退款单号多次请求只退一笔
                string batch_no = orderRefund.BatchNo;
                if (string.IsNullOrEmpty(batch_no))
                {
                    batch_no = DateTime.Now.ToString("yyyyMMddhhmmssfff");

                    //记录退款批次号存入订单退款表
                    OrderRefundBLL.UpdateBatchNo(orderRefund.Id, batch_no);
                }

                /*******************请求参数验证 end*****************************************************************/


                //订单总金额
                string total_fee = Convert.ToInt32(totalMoney * 100).ToString();
                //退款金额
                string refund_fee = Convert.ToInt32(orderRefund.RefundMoney * 100).ToString();

                //申请退款
                /***
                * 申请退款完整业务流程逻辑
                * @param transaction_id 微信订单号（优先使用）
                * @param out_trade_no 商户订单号
                * @param out_refund_no 商户退款单号
                * @param total_fee 订单总金额
                * @param refund_fee 退款金额
                * @return 退款结果（xml格式）
                */

                try
                {
                    //小程序支付的退款业务逻辑
                    //bool isSuccess = JWShop.XcxApi.Pay.RefundBusiness.Run(tradeNo, "", batch_no, total_fee, refund_fee);
                    WxpayResult wxResult = JWShop.XcxApi.Pay.RefundBusiness.Run(tradeNo, "", batch_no, total_fee, refund_fee);
                    //if (isSuccess)
                    if (wxResult.result_code)
                    {
                        orderRefund.Status = (int)OrderRefundStatus.HasReturn;
                        orderRefund.TmRefund = DateTime.Now;
                        OrderRefundBLL.Update(orderRefund);

                        OrderRefundActionBLL.Add(new OrderRefundActionInfo
                        {
                            OrderRefundId = orderRefund.Id,
                            //Status = isSuccess ? 1 : 0,
                            Status = wxResult.result_code ? 1 : 0,
                            Tm = DateTime.Now,
                            UserType = 2,
                            UserId = 0,
                            UserName = "系统",
                            Remark = orderRefund.Remark
                        });

                        Response.Clear();
                        Response.Write(JsonConvert.SerializeObject(new { ok = true }));
                    }
                    else
                    {
                        Response.Clear();
                        Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "微信退款失败," + wxResult.err_code_des }));

                    }
                }
                catch (Exception ex)
                {
                    Response.Clear();
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "微信退款出错, 请检查账户余额是否充足" }));
                    Response.End();
                }

            }
            else
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "无效的操作" }));
                Response.End();
            }

            #endregion
        }
        /// <summary>
        /// 审核订单退款申请
        /// </summary>
        /// <param name="approveStatus">1：通过；0：拒绝</param>
        private void Approve(int approveStatus, OrderRefundInfo orderRefund)
        {
            var submitOrderRefund = orderRefund;
          
            switch (submitOrderRefund.Status)
            {
                case (int)OrderRefundStatus.Submit:
                    CheckAdminPower("OrderRefundApprove", PowerCheckType.Single);
                    //如果是团购单，且拼团正在进行中，暂不能申请退款
                    var order = OrderBLL.Read(submitOrderRefund.OrderId);
                    if (order.IsActivity == (int)OrderKind.GroupBuy && order.FavorableActivityId > 0)
                    {
                        var groupBuy = GroupBuyBLL.Read(order.FavorableActivityId);
                        if (groupBuy.StartTime <= DateTime.Now && groupBuy.EndTime >= DateTime.Now && groupBuy.Quantity > groupBuy.SignCount)
                        {                            
                            Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "拼团正在进行，暂不能退款" }));
                            Response.End();
                        }
                    }
                    if (approveStatus == (int)BoolType.True)
                    {
                        submitOrderRefund.Status = (int)OrderRefundStatus.Approve;
                        submitOrderRefund.Remark = "系统审核通过，等待处理退款： ";
                    }
                    else
                    {
                        submitOrderRefund.Status = (int)OrderRefundStatus.Reject;
                        submitOrderRefund.Remark = "系统审核不通过： ";
                    }
                    break;
                case (int)OrderRefundStatus.Returning:                   
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "正在处理退款，请不要重复退款" }));
                    Response.End();
                    break;
                case (int)OrderRefundStatus.HasReturn:                    
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "退款已完成，请不要重复退款" }));
                    Response.End();
                    break;
                case (int)OrderRefundStatus.Reject:
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "退款已被拒绝，请不要重复退款" }));
                    Response.End();
                    break;
                default:
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "无效的操作" }));
                    Response.End();
                   break;
            }

            OrderRefundBLL.Update(submitOrderRefund);

            //退款操作记录
            AddOrderRefundAction(submitOrderRefund, approveStatus);

            AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("OrderRefund"), submitOrderRefund.Id);
        

        }
        //增加退款操作记录
        private void AddOrderRefundAction(OrderRefundInfo entity, int status)
        {
            OrderRefundActionInfo submitOrderRefundAction = new OrderRefundActionInfo
            {
                OrderRefundId = entity.Id,
                Status = status,
                Remark = entity.Remark,
                Tm = DateTime.Now,
                UserType = 2,
                UserId = Cookies.Admin.GetAdminID(false),
                UserName = Cookies.Admin.GetAdminName(false)
            };

            OrderRefundActionBLL.Add(submitOrderRefundAction);
        }
        #endregion

        #region 计算团购是否待退款、待审核
        private void NeedRefund()
        {
            bool needrefund = false,needcheck=false;
            int groupId = RequestHelper.GetQueryString<int>("id");
            int quantity = RequestHelper.GetQueryString<int>("quantity");
            var dataList = GroupSignBLL.SearchListByGroupId(Convert.ToInt32(groupId), 1, Convert.ToInt32(quantity), ref Count);
            needrefund = dataList.Count(k => k.EndTime < DateTime.Now && k.Quantity > k.SignCount && k.GroupOrderStatus == 2 && k.IsRefund == 0) > 0;
            needcheck = dataList.Count(k => k.StartTime <= DateTime.Now && k.EndTime >= DateTime.Now && k.SignCount >= k.Quantity && k.GroupOrderStatus == 2) > 0;
            Response.Clear();
            Response.Write(JsonConvert.SerializeObject(new { needrefund = needrefund, needcheck = needcheck }));
            Response.End();
        }
        #endregion
    }
}
