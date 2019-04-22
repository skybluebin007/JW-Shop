using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SocoShop.Common;
using SocoShop.Business;
using SocoShop.Entity;
using SkyCES.EntLib;

namespace SocoShop.Page
{
    public class GiftPackDetailAjax : AjaxBasePage
    {
        private string footaddress = string.Empty;
        /// <summary>
        /// 标题
        /// </summary>
        public string FootAddress
        {
            set { this.footaddress = value; }
            get
            {
                string temp = ShopConfig.ReadConfigInfo().FootAddress;
                if (this.footaddress != string.Empty)
                {
                    temp = this.footaddress + " - " + ShopConfig.ReadConfigInfo().FootAddress;
                }
                return temp;
            }
        }
        /// <summary>
        /// 选择的商品
        /// </summary>
        protected string[] selectProuct = new string[0];
        /// <summary>
        /// 礼品包ID
        /// </summary>
        protected string giftPackID = string.Empty;
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            giftPackID = RequestHelper.GetQueryString<string>("GiftPackID");
            string value = CookiesHelper.ReadCookieValue("GiftPack" + giftPackID);
            if (value != string.Empty)
            {
                value = StringHelper.Decode(value, ShopConfig.ReadConfigInfo().SecureKey);
                selectProuct = value.Split('|');
            }
        }
    }
}