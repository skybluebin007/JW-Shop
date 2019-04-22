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
    public class BrandDetail : CommonBasePage
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
        /// 商品品牌
        /// </summary>
        protected ProductBrandInfo productBrand = new ProductBrandInfo();
        /// <summary>
        /// 该品牌下的推荐产品
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
            int id=RequestHelper.GetQueryString<int>("ID");
            productBrand = ProductBrandBLL.ReadProductBrandCache(id);

            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.BrandID = id;
            productSearch.IsTop = (int)BoolType.True;
            productList = ProductBLL.SearchProductList(productSearch);

            string strProductID = string.Empty;
            foreach (ProductInfo product in productList)
            {
                if (strProductID == string.Empty)
                {
                    strProductID = product.ID.ToString();
                }
                else
                {
                    strProductID += "," + product.ID.ToString();
                }
            }
            if (strProductID != string.Empty)
            {
                memberPriceList = MemberPriceBLL.ReadMemberPriceByProductGrade(strProductID, base.GradeID);
            }

            Title = productBrand.Name;
        }
    }
}
