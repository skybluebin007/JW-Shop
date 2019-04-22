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
    public partial class AdKeywordAdd : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            int id = RequestHelper.GetQueryString<int>("Id");
            if (id > 0)
            {
                CheckAdminPower("ReadAdKeyword", PowerCheckType.Single);

                var adKeyword = AdKeywordBLL.Read(id);
                Name.Text = adKeyword.Name;
                Url.Text = adKeyword.Url;
                OrderId.Text = adKeyword.OrderId.ToString();
            }
            else
            {
                OrderId.Text = AdImageBLL.MaxOrderId(AdKeywordInfo.TABLENAME).ToString();
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            int id = RequestHelper.GetQueryString<int>("Id");
            var adKeyword = AdKeywordBLL.Read(id);
            adKeyword.Name = Name.Text;
            adKeyword.Url = Url.Text;
            adKeyword.OrderId = int.Parse(OrderId.Text);
            adKeyword.Tm = DateTime.Now;
            if (string.IsNullOrEmpty(adKeyword.Url)) adKeyword.Url = "/searchresults.html?kw=" + adKeyword.Name;

            string alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            if (adKeyword.Id > 0)
            {
                CheckAdminPower("UpdateAdKeyword", PowerCheckType.Single);
                AdKeywordBLL.Update(adKeyword);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("AdKeyword"), adKeyword.Id);
            }
            else
            {
                CheckAdminPower("AddAdKeyword", PowerCheckType.Single);
                id = AdKeywordBLL.Add(adKeyword);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("AdKeyword"), id);
                alertMessage = ShopLanguage.ReadLanguage("AddOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}