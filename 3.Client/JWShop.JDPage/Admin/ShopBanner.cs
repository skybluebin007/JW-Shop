using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCES.EntLib;
using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;
using Newtonsoft.Json;

namespace JWShop.Page.Admin
{
   public class ShopBanner:AdminBase
    {
        protected List<AdImageInfo> bannerList = new List<AdImageInfo>();
        protected int flashId = 0;
        protected override void PageLoad()
        {
            base.PageLoad();          

            //flashId 默认 11，店铺首页Banner
            flashId = RequestHelper.GetQueryString<int>("flashId") <= 0 ? 11 : RequestHelper.GetQueryString<int>("flashId");
            bannerList = AdImageBLL.ReadList(flashId);

            topNav = 2;
        }
      
    }
}
