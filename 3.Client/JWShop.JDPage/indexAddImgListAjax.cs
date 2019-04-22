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
    public class indexAddImgListAjax : AjaxBasePage
    {
        protected List<ProductInfo> proList = new List<ProductInfo>();
        protected override void PageLoad()
        {
            base.PageLoad();
            int CID = RequestHelper.GetQueryString<int>("CID");
            int count = 0;
           
            proList = ProductBLL.SearchList(1, 8, new ProductSearchInfo { ClassId = "|" + CID + "|",IsSale=1 }, ref count);
        }
    }
}
