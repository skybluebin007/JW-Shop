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
    public class GiftPackDetail : CommonBasePage
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
        /// 礼品包
        /// </summary>
        protected GiftPackInfo giftPack = new GiftPackInfo();
        /// <summary>
        /// 产品列表
        /// </summary>
        protected List<ProductInfo> productList = new List<ProductInfo>();
        /// <summary>
        /// 名称
        /// </summary>
        protected string[] nameArray = new string[0];
        /// <summary>
        /// 购买数量
        /// </summary>
        protected string[] countArray = new string[0];
        /// <summary>
        /// 产品ID列表
        /// </summary>
        protected string[] productArray = new string[0];

        /// <summary>
        /// 页面加载
        /// </summary>
         protected override void PageLoad()
        {
            base.PageLoad();
            int id = RequestHelper.GetQueryString<int>("ID");
            giftPack = GiftPackBLL.ReadGiftPack(id);            
            if (giftPack.GiftGroup != string.Empty)
            {
                string idList = string.Empty;
                int count = giftPack.GiftGroup.Split('#').Length;
                nameArray = new string[count];
                countArray = new string[count];
                productArray = new string[count];
                for (int i = 0; i < count; i++)
                {
                    string[] giftGroupArray = giftPack.GiftGroup.Split('#')[i].Split('|');
                    nameArray[i] = giftGroupArray[0];
                    countArray[i] = giftGroupArray[1];
                    productArray[i] = giftGroupArray[2];
                    if (giftGroupArray[2] != string.Empty)
                    {
                        idList += giftGroupArray[2] + ",";
                    }
                }
                if (idList != string.Empty)
                {
                    idList = idList.Substring(0, idList.Length - 1);
                    ProductSearchInfo productSearch = new ProductSearchInfo();
                    productSearch.InProductID = idList;
                    productList = ProductBLL.SearchProductList(productSearch);
                }
            }
            Title = giftPack.Name;
        }
    }
}