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
    public partial class AttributeRecordAjax : JWShop.Page.AdminBasePage
    {
        protected List<ProductTypeAttributeInfo> attributeList = new List<ProductTypeAttributeInfo>();
        protected void Page_Load(object sender, EventArgs e)
        {
            ClearCache();

            int attributeClassID = RequestHelper.GetQueryString<int>("AttributeClassID");
            int productID = RequestHelper.GetQueryString<int>("ProductID");
            string Action = RequestHelper.GetQueryString<string>("Action");
            if (!string.IsNullOrEmpty(Action))
            {
                int proTypeID = ProductClassBLL.GetProductClassType(attributeClassID);//通过产品分类ID获取所对应的产品类型ID;
                attributeList = ProductTypeAttributeBLL.JoinAttribute(proTypeID, productID);
            }
            else
            {
                attributeList = ProductTypeAttributeBLL.JoinAttribute(attributeClassID, productID);
            }
        }
    }
}
