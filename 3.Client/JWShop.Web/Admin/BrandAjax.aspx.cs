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
    public partial class BrandAjax : JWShop.Page.AdminBasePage
    {
        protected List<ProductBrandInfo> productBrandList = new List<ProductBrandInfo>();
        protected void Page_Load(object sender, EventArgs e)
        {
            ClearCache();
            int attributeClassID = RequestHelper.GetQueryString<int>("AttributeClassID");

            int proTypeID = ProductClassBLL.GetProductClassType(attributeClassID);

            ProductTypeInfo aci = ProductTypeBLL.Read(proTypeID);


            if (aci.Id > 0)
            {
                string[] strArr = aci.BrandIds.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                int[] intArr = Array.ConvertAll<string, int>(strArr, s => int.Parse(s));

                productBrandList = ProductBrandBLL.ReadList(intArr);
            }
        }
    }
}
