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
using System.Linq;

namespace JWShop.Page.Mobile
{
    public class Default : CommonBasePage
    {
        protected List<AdImageInfo> MobileAdList = new List<AdImageInfo>();
        protected List<ProductInfo> hotProductList = new List<ProductInfo>();
        /// <summary>
        /// 养老政策
        /// </summary>
        protected List<ArticleInfo> topylzcList = new List<ArticleInfo>();
        protected override void PageLoad()
        {
            base.PageLoad();

            MobileAdList = AdImageBLL.ReadList(8, 7);
                
            topNav = 110;
            int count = 0;
            topylzcList = ArticleBLL.SearchList(1, 4, new ArticleSearchInfo { ClassId = "|47|", IsTop = (int)BoolType.True }, ref count);
            //hotProductList = ProductBLL.SearchList(1, 4, new ProductSearchInfo { IsSale = (int)BoolType.True, IsTop = (int)BoolType.True, IsHot = (int)BoolType.True }, ref count);
            

        }
    }
}