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
        protected List<ProductInfo> likeProductList = new List<ProductInfo>();


        protected override void PageLoad()
        {
            base.PageLoad();

            MobileAdList = AdImageBLL.ReadList(5, 6);

            topNav = 9;
            int count = 0;
            likeProductList = ProductBLL.SearchList(1, 6, new ProductSearchInfo { ProductOrderType = "LikeNum", IsSale = (int)BoolType.True }, ref count);

        }
    }
}