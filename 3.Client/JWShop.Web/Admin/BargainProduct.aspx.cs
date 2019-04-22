using JWShop.Business;
using JWShop.Entity;
using JWShop.Page;
using SkyCES.EntLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JWShop.Web.Admin
{
    public partial class BargainProduct:AdminBasePage
    {
        protected ProductInfo product = new ProductInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            int id = RequestHelper.GetQueryString<int>("Id");
            product = ProductBLL.Read(id);
        }
    }
}