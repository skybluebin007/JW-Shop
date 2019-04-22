using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Page.Mobile
{
    public class ProductAjax : AjaxBasePage
    {
        /// <summary>
        /// 产品列表
        /// </summary>
        protected List<ProductInfo> productList = new List<ProductInfo>();

        protected List<ProductBrandInfo> showBrandList = new List<ProductBrandInfo>();
        protected List<ProductTypeAttributeInfo> showAttributeList = new List<ProductTypeAttributeInfo>();
        
        protected int currentPage;
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();

            var queryClass = RequestHelper.GetQueryString<string>("cat").Split(',');
            int classLevel1 = 0, classLevel2 = 0, classLevel3 = 0;
            if (queryClass.Length > 0) classLevel1 = ShopCommon.ConvertToT<int>(queryClass[0]);
            if (queryClass.Length > 1) classLevel2 = ShopCommon.ConvertToT<int>(queryClass[1]);
            if (queryClass.Length > 2) classLevel3 = ShopCommon.ConvertToT<int>(queryClass[2]);
            int currentClassId = classLevel3 > 0 ? classLevel3 : classLevel2 > 0 ? classLevel2 : classLevel1;

            //商品列表
            currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 6; int count = 0;
            string brandIds = RequestHelper.GetQueryString<string>("brand");
            string attributeIds = RequestHelper.GetQueryString<string>("at");
            string attributeValues = RequestHelper.GetQueryString<string>("ex");
            string orderField = RequestHelper.GetQueryString<string>("sort");
            string orderType = "";
            var orderParams = orderField.Split('_');
            if (orderParams.Length > 1)
            {
                orderField = orderParams[0];
                orderType = orderParams[1];
            }
            int minPrice = RequestHelper.GetQueryString<int>("min");
            int maxPrice = RequestHelper.GetQueryString<int>("max");
            string keywords = RequestHelper.GetQueryString<string>("kw");
            int isNew = RequestHelper.GetQueryString<int>("isNew");
            int isHot = RequestHelper.GetQueryString<int>("isHot");
            int isSpecial = RequestHelper.GetQueryString<int>("isSpecial");
            int isTop = RequestHelper.GetQueryString<int>("isTop");

            productList = ProductBLL.SearchList(currentPage, pageSize, currentClassId, brandIds, attributeIds, attributeValues, orderField, orderType, minPrice, maxPrice, keywords,"", isNew, isHot, isSpecial, isTop, ref count, ref showAttributeList, ref showBrandList);
        }
    }
}
