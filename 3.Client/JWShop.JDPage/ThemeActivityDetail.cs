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
    public class ThemeActivityDetail : CommonBasePage
    {
        /// <summary>
        /// 专题活动
        /// </summary>
        protected ThemeActivityInfo themeActivity = new ThemeActivityInfo();
        /// <summary>
        /// 样式
        /// </summary>
        protected string[] styleArray;
        /// <summary>
        /// 产品组
        /// </summary>
        protected string[] productGroupArray = new string[0];
        /// <summary>
        /// 产品列表
        /// </summary>
        protected List<ProductInfo> productList = new List<ProductInfo>();
        /// <summary>
        /// 会员价格
        /// </summary>
        protected List<MemberPriceInfo> memberPriceList = new List<MemberPriceInfo>();
        /// <summary> 
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            int id = RequestHelper.GetQueryString<int>("ID");
            themeActivity = ThemeActivityBLL.Read(id);
            styleArray = themeActivity.Style.Split('|');
            if (themeActivity.ProductGroup != string.Empty)
            {
                string productIDList = string.Empty;
                productGroupArray = themeActivity.ProductGroup.Split(new char[] { '#' },StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < productGroupArray.Length; i++)
                {
                    if (productGroupArray[i].Split('|')[4] != string.Empty)
                    {
                        if (productIDList == string.Empty)
                        {
                            productIDList = productGroupArray[i].Split('|')[4];
                        }
                        else
                        {
                            productIDList += "," + productGroupArray[i].Split('|')[4];
                        }
                    }
                }
                if (productIDList != string.Empty)
                {
                    ProductSearchInfo productSearch = new ProductSearchInfo();
                    productSearch.InProductId = productIDList;
                    productList = ProductBLL.SearchList(productSearch);
                }
            }
            Title = themeActivity.Name;
        }
    }
}
