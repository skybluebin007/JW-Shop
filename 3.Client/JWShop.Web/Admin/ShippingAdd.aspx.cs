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
    public partial class ShippingAdd : JWShop.Page.AdminBasePage
    {
        protected ShippingInfo shipping = new ShippingInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int shippingId = RequestHelper.GetQueryString<int>("Id");
                if (shippingId != int.MinValue)
                {
                    CheckAdminPower("ReadShipping", PowerCheckType.Single);
                    shipping = ShippingBLL.Read(shippingId);
                    Name.Text = shipping.Name;
                    Description.Text = shipping.Description;
                    ShippingCode.Text = shipping.ShippingCode;
                    if (shipping.ShippingType == (int)JWShop.Entity.ShippingType.Weight)
                    {
                        FirstWeight.Text = shipping.FirstWeight.ToString();
                        AgainWeight.Text = shipping.AgainWeight.ToString();
                    }
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            ShippingInfo shipping = new ShippingInfo();
            shipping.Id = RequestHelper.GetQueryString<int>("Id");
            shipping.Name = Name.Text;
            shipping.Description = Description.Text;
            shipping.IsEnabled = RequestHelper.GetForm<int>("IsEnabled");
            shipping.ShippingType = RequestHelper.GetForm<int>("ShippingType");
            shipping.FirstWeight = Convert.ToInt32(FirstWeight.Text);
            shipping.AgainWeight = Convert.ToInt32(AgainWeight.Text);
            shipping.ShippingCode = ShippingCode.Text.Trim();

            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (shipping.Id == int.MinValue)
            {
                CheckAdminPower("AddShipping", PowerCheckType.Single);
                int id = ShippingBLL.Add(shipping);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Shipping"), id);
            }
            else
            {
                CheckAdminPower("UpdateShipping", PowerCheckType.Single);
                ShippingBLL.Update(shipping);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Shipping"), shipping.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}