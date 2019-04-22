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
    public partial class AdImageAdd : JWShop.Page.AdminBasePage
    {
        protected int _adType;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProductClass.DataSource = ProductClassBLL.ReadRootList();
                ProductClass.DataTextField = "Name";
                ProductClass.DataValueField = "ID";
                ProductClass.DataBind();

                int id = RequestHelper.GetQueryString<int>("Id");
                if (id > 0)
                {
                    CheckAdminPower("ReadAdImage", PowerCheckType.Single);

                    var adImage = AdImageBLL.Read(id);
                    Title.Text = adImage.Title;
                    SubTitle.Text = adImage.SubTitle;
                    LinkUrl.Text = adImage.LinkUrl;
                    ImageUrl.Text = adImage.ImageUrl;
                    OrderId.Text = adImage.OrderId.ToString();
                    BgColor.Text = adImage.MobileLinkUrl;
                    ProductClass.Text = adImage.ClassId.ToString();

                    _adType = adImage.AdType;
                }
                else
                {
                    OrderId.Text = AdImageBLL.MaxOrderId(AdImageInfo.TABLENAME).ToString();

                    _adType = RequestHelper.GetQueryString<int>("fp_type");
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            int id = RequestHelper.GetQueryString<int>("Id");
            var adImage = AdImageBLL.Read(id);
            adImage.Title = Title.Text;
            adImage.SubTitle = SubTitle.Text;
            adImage.LinkUrl =StringHelper.Substring(LinkUrl.Text.Trim(),192,false);
            adImage.ImageUrl = ImageUrl.Text;
            adImage.OrderId = int.Parse(OrderId.Text);           
            adImage.MobileLinkUrl = BgColor.Text.Trim();
            int _classId = 0;
            int.TryParse(ProductClass.Text, out _classId);
            adImage.ClassId = _classId;

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
                adImage.AdType = RequestHelper.GetQueryString<int>("fp_type");
                adImage.Tm = DateTime.Now;
                id = AdImageBLL.Add(adImage);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("AdImage"), id);
                alertMessage = ShopLanguage.ReadLanguage("AddOK");
            }
            ScriptHelper.Alert(alertMessage, "FpImage.aspx?fp_type=" + adImage.AdType + "");
        }
    }
}