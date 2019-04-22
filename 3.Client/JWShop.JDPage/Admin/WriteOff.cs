using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Business;
using JWShop.Entity;

namespace JWShop.Page.Admin
{
   public class WriteOff:AdminBase
    {
        protected override void PageLoad()
        {
            base.PageLoad();
            topNav = 2;
            Title = "订单核销";

        }
    }
}
