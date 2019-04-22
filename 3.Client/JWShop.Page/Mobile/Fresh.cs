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

namespace JWShop.Page.Mobile
{
    public class Fresh : CommonBasePage
    {
        protected List<ProductInfo> productList = new List<ProductInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            string action = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("Action"));
            if (action == "LoadProducts") this.LoadProducts();

            ProductSearchInfo searchInfo = new ProductSearchInfo();
            searchInfo.IsNew = (int)BoolType.True;
            searchInfo.ProductOrderType = "AddDate";
            searchInfo.OrderType = OrderType.Desc;

            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 20;
            int count = 0;
            productList = ProductBLL.SearchList(currentPage, pageSize, searchInfo, ref count);

            Title = "新品上市";
        }

        private void LoadProducts()
        {
            ProductSearchInfo searchInfo = new ProductSearchInfo();
            searchInfo.IsNew = (int)BoolType.True;
            searchInfo.ProductOrderType = "AddDate";
            searchInfo.OrderType = OrderType.Desc;

            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 20;
            int count = 0;
            productList = ProductBLL.SearchList(currentPage, pageSize, searchInfo, ref count);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(from item in productList select new { id = item.Id, name = StringHelper.Substring(item.Name, 5), photo = item.Photo.Replace("Original", "150-150") });
            ResponseHelper.Write(json);
            ResponseHelper.End();
        }

    }
}