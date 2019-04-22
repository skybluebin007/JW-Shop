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

namespace JWShop.Web.Admin
{
    public partial class PointProductOrder : JWShop.Page.AdminBasePage
    {
        protected int orderStatus = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ManagePointProductOrder", PowerCheckType.Single);

                string action = RequestHelper.GetQueryString<string>("Action");
                if (action == "Shipping")
                {
                    int shippingId = RequestHelper.GetForm<int>("OrderId");
                    string shippingName = RequestHelper.GetForm<string>("ShippingName");
                    string shippingNumber = RequestHelper.GetForm<string>("ShippingNumber");

                    var pointProductOrder = PointProductOrderBLL.Read(shippingId);
                    if (pointProductOrder.OrderStatus == (int)PointProductOrderStatus.Shipping)
                    {
                        pointProductOrder.OrderStatus = (int)PointProductOrderStatus.HasShipping;
                        pointProductOrder.ShippingName = shippingName;
                        pointProductOrder.ShippingNumber = shippingNumber;
                        pointProductOrder.ShippingDate = DateTime.Now;
                        PointProductOrderBLL.Update(pointProductOrder);
                    }

                    ResponseHelper.End();
                }
                if (action == "Cancel")
                {
                    int orderId = RequestHelper.GetQueryString<int>("OrderId");
                    var pointProductOrder = PointProductOrderBLL.Read(orderId);
                    if (pointProductOrder.OrderStatus == (int)PointProductOrderStatus.Shipping)
                    {
                        pointProductOrder.OrderStatus = (int)PointProductOrderStatus.Cancel;
                        PointProductOrderBLL.Update(pointProductOrder);

                        //恢复用户积分
                        UserAccountRecordBLL.Add(new UserAccountRecordInfo
                        {
                            RecordType = (int)AccountRecordType.Point,
                            Money = 0,
                            Point = pointProductOrder.Point,
                            Date = DateTime.Now,
                            IP = ClientHelper.IP,
                            Note = "取消兑取礼品：" + pointProductOrder.ProductName + " 订单号：" + pointProductOrder.OrderNumber,
                            UserId = pointProductOrder.UserId,
                            UserName = pointProductOrder.UserName
                        });

                        //恢复兑换数量
                        PointProductBLL.ChangeSendCount(pointProductOrder.ProductId, ChangeAction.Minus);
                    }

                    ResponseHelper.End();
                }

                orderStatus = RequestHelper.GetQueryString<int>("OrderStatus");
                hOrderStatus.Value = orderStatus.ToString();

                PointProductOrderSearchInfo searchInfo = new PointProductOrderSearchInfo();
                searchInfo.OrderStatus = orderStatus;
                searchInfo.ProductName = RequestHelper.GetQueryString<string>("ProductName");
                searchInfo.OrderNumber = RequestHelper.GetQueryString<string>("OrderNumber");
                searchInfo.UserName = RequestHelper.GetQueryString<string>("UserName");
                searchInfo.StartAddDate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
                searchInfo.EndAddDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndAddDate"));
                var data = PointProductOrderBLL.SearchList(CurrentPage, PageSize, searchInfo, ref Count);

                ProductName.Text = searchInfo.ProductName;
                OrderNumber.Text = searchInfo.OrderNumber;
                UserName.Text = searchInfo.UserName;
                StartAddDate.Text = RequestHelper.GetQueryString<string>("StartAddDate");
                EndAddDate.Text = RequestHelper.GetQueryString<string>("EndAddDate");

                BindControl(data, RecordList, MyPager);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "PointProductOrder.aspx?Action=search&";
            URL += "OrderStatus=" + hOrderStatus.Value + "&";
            URL += "ProductName=" + ProductName.Text + "&";
            URL += "OrderNumber=" + OrderNumber.Text + "&";
            URL += "UserName=" + UserName.Text + "&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text;
            ResponseHelper.Redirect(URL);
        }

    }
}