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
    public class GroupBuyDetail : CommonBasePage
    {
        protected GroupBuyInfo groupBuy = new GroupBuyInfo();
        protected List<GroupBuyInfo> groupBuyList = new List<GroupBuyInfo>();
        protected List<ProductInfo> productList = new List<ProductInfo>();
        protected ProductInfo product = new ProductInfo();
        protected long leftTime = 0;
        protected int buyCount = 0;
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void PageLoad()
        {
            base.PageLoad();

            int id = RequestHelper.GetQueryString<int>("ID");
            groupBuy = GroupBuyBLL.ReadGroupBuy(id);
            TimeSpan timeSpan = groupBuy.EndDate - RequestHelper.DateNow;
            leftTime = timeSpan.Days * 24 * 3600 + timeSpan.Hours * 3600 + timeSpan.Minutes * 60 + timeSpan.Seconds;
            buyCount = UserGroupBuyBLL.ReadUserGroupBuyCount(id);
            product = ProductBLL.ReadProduct(groupBuy.ProductID);
            Title = product.Name + " - 商品团购";

            int count = 0;
            GroupBuySearchInfo groupBuySearch = new GroupBuySearchInfo();
            groupBuySearch.Status = GroupBuyStatus.Normal;
            groupBuyList = GroupBuyBLL.ReadGroupBuyList(1, 8, groupBuySearch, ref count);

            string productIDList = string.Empty;
            string idList = string.Empty;
            foreach (GroupBuyInfo groupBuyInfo in groupBuyList)
            {
                if (productIDList == string.Empty)
                {
                    productIDList = groupBuyInfo.ProductID.ToString();
                    idList = groupBuyInfo.ID.ToString();
                }
                else
                {
                    productIDList += "," + groupBuyInfo.ProductID.ToString();
                    idList += "," + groupBuyInfo.ID.ToString();
                }
            }
            //读取商品
            if (productIDList != string.Empty)
            {
                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.InProductID = productIDList;
                productList = ProductBLL.SearchProductList(productSearch);
            }
        }
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
    }
}
