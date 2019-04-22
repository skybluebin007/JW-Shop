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

namespace JWShop.Page.Lab
{
    public class List : CommonBasePage
    {
        protected ProductClassInfo currentClass = new ProductClassInfo();
        protected Dictionary<string, ProductClassInfo> levelClass = new Dictionary<string, ProductClassInfo>();
        protected List<ProductClassInfo> showClassList = new List<ProductClassInfo>();
        protected List<ProductBrandInfo> showBrandList = new List<ProductBrandInfo>();
        protected List<ProductTypeAttributeInfo> showAttributeList = new List<ProductTypeAttributeInfo>();
        protected string baseQueryString;
        protected CommonPagerClass pager = new CommonPagerClass();
        protected int currentPage;

        //商品列表数据
        protected List<ProductInfo> productList = new List<ProductInfo>();
        protected List<ProductInfo> hotProductList = new List<ProductInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            var queryClass = RequestHelper.GetQueryString<string>("cat").Split(',');
            int classLevel1 = 0, classLevel2 = 0, classLevel3 = 0;
            if (queryClass.Length > 0) classLevel1 = ShopCommon.ConvertToT<int>(queryClass[0]);
            if (queryClass.Length > 1) classLevel2 = ShopCommon.ConvertToT<int>(queryClass[1]);
            if (queryClass.Length > 2) classLevel3 = ShopCommon.ConvertToT<int>(queryClass[2]);
            int currentClassId = classLevel3 > 0 ? classLevel3 : classLevel2 > 0 ? classLevel2 : classLevel1;
            if (currentClassId < 1) ResponseHelper.Redirect("/");

            //面包屑区域--分类
            showClassList = ProductClassBLL.ReadList();
            currentClass = showClassList.FirstOrDefault(k => k.Id == currentClassId) ?? new ProductClassInfo();
            levelClass.Add("level1", showClassList.FirstOrDefault(k => k.Id == classLevel1) ?? new ProductClassInfo());
            levelClass.Add("level2", showClassList.FirstOrDefault(k => k.Id == classLevel2) ?? new ProductClassInfo());
            levelClass.Add("level3", showClassList.FirstOrDefault(k => k.Id == classLevel3) ?? new ProductClassInfo());

            //热门商品
            int hotCount = 0;
            hotProductList = ProductBLL.SearchList(1, 7, new ProductSearchInfo { IsHot = (int)BoolType.True, IsSale = (int)BoolType.True }, ref hotCount);

            //商品列表
            currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 24; int count = 0;
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
            string isNew = RequestHelper.GetQueryString<string>("isNew");
            string isHot = RequestHelper.GetQueryString<string>("isHot");
            string isSpecial = RequestHelper.GetQueryString<string>("isSpecial");
            string isTop = RequestHelper.GetQueryString<string>("isTop");

            //productList = ProductBLL.SearchList(currentPage, pageSize, 0, "", "", "", orderField, orderType, minPrice, maxPrice, keywords, isNew,isHot,isSpecial,isTop, ref count, ref showAttributeList, ref showBrandList);

            pager.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));
        }
    }
}