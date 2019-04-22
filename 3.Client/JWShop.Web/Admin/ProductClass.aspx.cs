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
using System.Linq;

namespace JWShop.Web.Admin
{
    public partial class ProductClass : JWShop.Page.AdminBasePage
    {
        protected List<ProductClassInfo> topProductClassList = new List<ProductClassInfo>(); 
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAdminPower("ReadProductClass", PowerCheckType.Single);
            string action = RequestHelper.GetQueryString<string>("Action");
            int productClassID = RequestHelper.GetQueryString<int>("ID");
            if (action != string.Empty && productClassID != int.MinValue)
            {
                switch (action)
                {                    
                    case "Delete":
                        CheckAdminPower("DeleteProductClass", PowerCheckType.Single);
                        ProductClassBLL.Delete(productClassID);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("ProductClass"), productClassID);
                        break;
                    default:
                        break;
                }
            }

            topProductClassList = ProductClassBLL.ReadRootList();
            //BindControl(ProductClassBLL.ReadRootList(), RecordList);
        }
        
    }
}