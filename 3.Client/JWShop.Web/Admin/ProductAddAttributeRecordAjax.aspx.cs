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
    public partial class ProductAddAttributeRecordAjax : JWShop.Page.AdminBasePage
    {
        protected List<ProductTypeAttributeInfo> attributeList = new List<ProductTypeAttributeInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            int classId = RequestHelper.GetQueryString<int>("classId");
            int productId = RequestHelper.GetQueryString<int>("productId");

            var cls = ProductClassBLL.Read(classId);
            attributeList = ProductTypeAttributeBLL.JoinAttribute(cls.ProductTypeId, productId);
        }
    }
}