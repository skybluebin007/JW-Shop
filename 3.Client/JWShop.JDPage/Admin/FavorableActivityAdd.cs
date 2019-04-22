using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Business;
using JWShop.Entity;
using JWShop.Common;
using SkyCES.EntLib;

namespace JWShop.Page.Admin
{
   public class FavorableActivityAdd:AdminBase
    {
        protected FavorableActivityInfo entity = new FavorableActivityInfo();
        protected override void PageLoad()
        {
            base.PageLoad();
            int id = RequestHelper.GetQueryString<int>("id");
            entity = FavorableActivityBLL.Read(id);
        }
    }
}
