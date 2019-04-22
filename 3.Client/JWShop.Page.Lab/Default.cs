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

namespace JWShop.Page.Lab
{
    public class Default : CommonBasePage
    {
        protected List<AdImageInfo> imageList = new List<AdImageInfo>();
        protected List<ProductInfo> likeProductList = new List<ProductInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            imageList = AdImageBLL.ReadList();

            int count = 0;
            likeProductList = ProductBLL.SearchList(1, 5, new ProductSearchInfo { ProductOrderType = "LikeNum", IsSale = (int)BoolType.True }, ref count);

        }
    }
}