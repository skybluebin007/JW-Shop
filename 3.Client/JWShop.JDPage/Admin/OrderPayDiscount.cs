using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCES.EntLib;
using JWShop.Business;
using JWShop.Entity;
using JWShop.Common;

namespace JWShop.Page.Admin
{
   public class OrderPayDiscount:AdminBase
    {
        protected override void PageLoad()
        {
            base.PageLoad();
            topNav = 2;
        }
    }
}
