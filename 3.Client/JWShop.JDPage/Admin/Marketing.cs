using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCES.EntLib;
using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;
namespace JWShop.Page.Admin
{
   public class Marketing:AdminBase
    {
        protected override void PageLoad()
        {
            base.PageLoad();
            topNav = 2;
        }
    }
}
