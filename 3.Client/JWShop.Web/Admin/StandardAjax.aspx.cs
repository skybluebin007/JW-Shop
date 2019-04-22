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

namespace JWShop.Web.Admin
{
    public partial class StandardAjax : JWShop.Page.AdminBasePage
    {
        protected List<ProductTypeStandardInfo> standardList = new List<ProductTypeStandardInfo>();
        protected int productID = int.MinValue;
        protected ProductInfo product = new ProductInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            ClearCache();
            int attributeClassID = RequestHelper.GetQueryString<int>("AttributeClassID");
            productID= RequestHelper.GetQueryString<int>("ProductID");
            if (productID > 0) product = ProductBLL.Read(productID);
            int proTypeID = ProductClassBLL.GetProductClassType(attributeClassID);
            standardList = ProductTypeStandardBLL.ReadList(proTypeID);
        }
    }
}
