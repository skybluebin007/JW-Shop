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
    public class GiftPackAjax : AjaxBasePage
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
        /// Ajax分页
        /// </summary>
        protected AjaxPagerClass ajaxPagerClass = new AjaxPagerClass();
        /// <summary>
        /// 特价礼品包
        /// </summary>
        protected List<GiftPackInfo> giftPackList = new List<GiftPackInfo>();
        /// <summary>
        /// 页面加载
        /// </summary>
         protected override void PageLoad()
        {
            base.PageLoad();
            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 6;
            int count = 0;
            giftPackList=GiftPackBLL.ReadGiftPackList(currentPage, pageSize, ref count);

            ajaxPagerClass.CurrentPage = currentPage;
            ajaxPagerClass.PageSize = pageSize;
            ajaxPagerClass.Count = count;
            ajaxPagerClass.DisCount = false;
            ajaxPagerClass.ListType = false;
        }
    }
}
