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
    public class GroupBuy : CommonBasePage
    {
        protected List<GroupBuyInfo> groupBuyList = new List<GroupBuyInfo>();
        protected List<ProductInfo> productList = new List<ProductInfo>();
        protected Dictionary<int, int> dicCount = new Dictionary<int, int>();
        protected CommonPagerClass commonPagerClass = new CommonPagerClass();


        protected override void PageLoad()
        {
            base.PageLoad();

            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 10;
            int count = 0;
            GroupBuySearchInfo groupBuySearch = new GroupBuySearchInfo();
            groupBuySearch.Status = GroupBuyStatus.Normal;
            groupBuyList = GroupBuyBLL.ReadGroupBuyList(currentPage, pageSize,groupBuySearch, ref count);
            commonPagerClass.CurrentPage = currentPage;
            commonPagerClass.PageSize = pageSize;
            commonPagerClass.Count = count;
            commonPagerClass.FirstPage = "<<首页";
            commonPagerClass.PreviewPage = "<<上一页";
            commonPagerClass.NextPage = "下一页>>";
            commonPagerClass.LastPage = "末页>>";
            commonPagerClass.ListType = false;
            commonPagerClass.DisCount = false;
            commonPagerClass.PrenextType = true;

            string productIDList = string.Empty;
            string idList = string.Empty;
            foreach (GroupBuyInfo groupBuy in groupBuyList)
            {
                    productIDList += "," + groupBuy.ProductID.ToString();
                    idList += "," + groupBuy.ID.ToString();
            }
            if (productIDList != string.Empty) {
                productIDList = productIDList.Substring(1);
            }
            if (idList != string.Empty) {
                idList = idList.Substring(1);
            }
            //读取商品
            if (productIDList != string.Empty)
            {
                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.InProductID = productIDList;
                productList = ProductBLL.SearchProductList(productSearch);
            }
            //读取购买人数
            if (idList != string.Empty)
            {
                dicCount = UserGroupBuyBLL.ReadUserGroupBuyCount(idList);
            }

            Title = "商品团购";
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
