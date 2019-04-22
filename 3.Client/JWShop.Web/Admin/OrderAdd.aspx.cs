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
    public partial class OrderAdd : JWShop.Page.AdminBasePage
    {
        protected string title = string.Empty;
        protected OrderInfo order = new OrderInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int orderId = RequestHelper.GetQueryString<int>("Id");
                if (orderId != int.MinValue)
                {
                    CheckAdminPower("ReadOrder", PowerCheckType.Single);
                    order = OrderBLL.Read(orderId);
                    int isCod = PayPlugins.ReadPayPlugins(order.PayKey).IsCod;
                    if ((order.OrderStatus == (int)OrderStatus.WaitPay || order.OrderStatus == (int)OrderStatus.WaitCheck && isCod == (int)BoolType.True) && (order.IsActivity == (int)OrderKind.Common || order.IsActivity == (int)OrderKind.GroupBuy))
                    {
                        RegionID.DataSource = RegionBLL.ReadRegionUnlimitClass();
                        OrderNote.Text = order.OrderNote;
                        ProductMoney.Text = order.ProductMoney.ToString();
                        PointMoney.Text = order.PointMoney.ToString();
                        Consignee.Text = order.Consignee;
                        RegionID.ClassID = order.RegionId;
                        Address.Text = order.Address;
                        ZipCode.Text = order.ZipCode;
                        Tel.Text = order.Tel;
                        Email.Text = order.Email;
                        Mobile.Text = order.Mobile;
                        ShippingDate.Text = order.ShippingDate.ToString("yyyy-MM-dd");
                        ShippingNumber.Text = order.ShippingNumber;
                        ShippingMoney.Text = order.ShippingMoney.ToString();
                        OtherMoney.Text = order.OtherMoney.ToString();                       
                        Balance.Text = order.Balance.ToString();
                        CouponMoney.Text = order.CouponMoney.ToString();
                        FavorableMoney.Text = order.FavorableMoney.ToString();
                        UserMessage.Text = order.UserMessage;
                        string action = RequestHelper.GetQueryString<string>("Action");
                        switch (action)
                        {
                            case "Shipping":
                                title = "邮寄信息";
                                Shipping.Visible = true;
                                if (order.OrderStatus >= (int)OrderStatus.HasShipping)
                                {
                                    ShippingInfo.Visible = true;
                                }
                                break;
                            case "Other":
                                title = "其他信息";
                                Other.Visible = true;
                                break;
                            case "Money":
                                title = "费用信息";
                                Money.Visible = true;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        string js = "<script language='javascript'>alert(\"订单已经审核，不能修改\");parent.cancel();</script>";
                        ResponseHelper.Write(js);
                        ResponseHelper.End();
                    }
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            int orderId = RequestHelper.GetQueryString<int>("Id");
            OrderInfo order = OrderBLL.Read(orderId);
            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "Shipping":
                    string regionID = RegionID.ClassID;
                    int shippingID = RequestHelper.GetForm<int>("ShippingID");
                    if (regionID == string.Empty || shippingID <= 0)
                    {
                        ScriptHelper.Alert("收货地区和配送方式不能为空");
                    }
                    if (order.RegionId != regionID || order.ShippingId != shippingID)
                    {
                        order.ShippingId = shippingID;
                        order.RegionId = regionID;
                        order.ShippingMoney = OrderBLL.ReadOrderShippingMoney(order);
                    }
                    order.OrderNote = OrderNote.Text;
                    order.Consignee = Consignee.Text;
                    order.Address = Address.Text;
                    order.ZipCode = ZipCode.Text;
                    order.Tel = Tel.Text;
                    order.Email = Email.Text;
                    order.Mobile = Mobile.Text;
                    order.ShippingDate = Convert.ToDateTime(ShippingDate.Text);
                    order.ShippingNumber = ShippingNumber.Text;
                    break;
                case "Other":
                    order.OrderNote = OrderNote.Text;
                    break;
                case "Money":
                    order.OtherMoney = Convert.ToDecimal(OtherMoney.Text);
                    break;
                default:
                    break;
            }
            CheckAdminPower("UpdateOrder", PowerCheckType.Single);
            OrderBLL.Update(order);
            AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Order"), order.Id);
            string alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }

    }
}