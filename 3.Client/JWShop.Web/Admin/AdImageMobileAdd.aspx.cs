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
    public partial class AdImageMobileAdd : JWShop.Page.AdminBasePage
    {
        protected int _adType;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            int id = RequestHelper.GetQueryString<int>("Id");
            if (id > 0)
            {
                CheckAdminPower("ReadAdImage", PowerCheckType.Single);

                var adImage = AdImageBLL.Read(id);
                Title.Text = adImage.Title;
                SubTitle.Text = adImage.SubTitle;
                LinkUrl.Text = adImage.MobileLinkUrl;
                ImageUrl.Text = adImage.MobileImageUrl;
                OrderId.Text = adImage.OrderId.ToString();

                _adType = adImage.AdType;
            }
            else
            {
                OrderId.Text = AdImageBLL.MaxOrderId(AdImageInfo.TABLENAME).ToString();

                _adType = RequestHelper.GetQueryString<int>("ad_type");
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            int id = RequestHelper.GetQueryString<int>("Id");
            var adImage = AdImageBLL.Read(id);
            adImage.Title = Title.Text;
            adImage.SubTitle = SubTitle.Text;
            adImage.MobileLinkUrl = LinkUrl.Text;
            adImage.MobileImageUrl = ImageUrl.Text;
            adImage.OrderId = int.Parse(OrderId.Text);
            adImage.Tm = DateTime.Now;

            string alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            if (adImage.Id > 0)
            {
                CheckAdminPower("UpdateAdImage", PowerCheckType.Single);
                AdImageBLL.Update(adImage);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("AdImage"), adImage.Id);
            }
            else
            {
                CheckAdminPower("AddAdImage", PowerCheckType.Single);
                adImage.AdType = RequestHelper.GetQueryString<int>("ad_type");
                adImage.ClassId = RequestHelper.GetQueryString<int>("class_id");
                id = AdImageBLL.Add(adImage);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("AdImage"), id);
                alertMessage = ShopLanguage.ReadLanguage("AddOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}