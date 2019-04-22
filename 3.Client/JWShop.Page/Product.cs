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

namespace JWShop.Page
{
    public class Product : CommonBasePage
    {        
        protected List<ProductInfo> ProductHot = new List<ProductInfo>();
        /// <summary>
        /// 搜索条件
        /// </summary>
        protected string searchCondition = string.Empty;
        /// <summary>
        /// 显示搜索的条件
        /// </summary>
        protected string showCondition = string.Empty;
        /// <summary>
        /// 显示的标题
        /// </summary>
        protected string showTitle = string.Empty;        
        /// <summary>
        /// 相关搜索
        /// </summary>
        protected string relationSearch = string.Empty;
        /// <summary>
        /// 会员价格
        /// </summary>
        protected List<MemberPriceInfo> memberPriceList = new List<MemberPriceInfo>();
        /// <summary>
        /// 是否要单独计算价格
        /// </summary>
        protected bool countPriceSingle = false;
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            SearchCondition();
            LoadHotProductList();
        }
        private void LoadHotProductList()
        {
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.IsHot = 1;
            productSearch.IsSale = 1;
            ProductHot = ProductBLL.SearchList(productSearch);

            //读取会员价格
            countPriceSingle = true;
            string strProductID = string.Empty;
            foreach (ProductInfo product in ProductHot)
            {

                if (strProductID == string.Empty)
                {
                    strProductID = product.Id.ToString();
                }
                else
                {
                    strProductID += "," + product.Id.ToString();
                }
            }
            
        }
        /// <summary>
        /// 搜索条件
        /// </summary>
        protected void SearchCondition()
        {
            int classID = RequestHelper.GetQueryString<int>("ClassID");
            string productName = RequestHelper.GetQueryString<string>("Keyword");
            int brandID = RequestHelper.GetQueryString<int>("BrandID");
            string tags = RequestHelper.GetQueryString<string>("Tags");
            int isNew = RequestHelper.GetQueryString<int>("IsNew");
            int isHot = RequestHelper.GetQueryString<int>("IsHot");
            int isSpecial = RequestHelper.GetQueryString<int>("IsSpecial");
            int isTop = RequestHelper.GetQueryString<int>("IsTop");
            searchCondition = "ClassID=" + classID.ToString() + "&ProductName=" + productName + "&BrandID="
                + brandID.ToString() + "&IsNew=" + isNew + "&IsHot=" + isHot + "&IsSpecial=" + isSpecial + "&IsTop=" + isTop + "";
                                   
            
            Title = showTitle + " - 商品展示";
        }
    }
}
