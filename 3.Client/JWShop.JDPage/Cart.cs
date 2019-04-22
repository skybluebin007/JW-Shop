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

namespace JWShop.Page
{
    public class Cart : CommonBasePage
    {
        protected List<ProductInfo> likeProductList = new List<ProductInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();
            istop = 1;
            Title = "购物车";
            int count = 0;
            likeProductList = ProductBLL.SearchList(1, 4, new ProductSearchInfo { ProductOrderType = "LikeNum", IsSale = (int)BoolType.True }, ref count);

        }
    }
}