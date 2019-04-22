using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Page
{
   public class HistoryList : UserBasePage
    {
       protected List<ProductInfo> proHistoryList = new List<ProductInfo>();
       /// <summary>
       /// 分页
       /// </summary>
       protected CommonPagerClass commonPagerClass = new CommonPagerClass();
       /// <summary>
       /// 浏览过的商品ID串
       /// </summary>
       protected string strHistoryProduct = string.Empty;
       protected override void PageLoad()
       {
           base.PageLoad();

         
           topNav = 7;


           int count = 0;
           int currentPage = RequestHelper.GetQueryString<int>("Page");
           if (currentPage < 1)
           {
               currentPage = 1;
           }
           int pageSize = 12;
           //浏览过的商品
           strHistoryProduct = Server.UrlDecode(CookiesHelper.ReadCookieValue("HistoryProduct"));
           if (strHistoryProduct.StartsWith(","))
           {
               strHistoryProduct = strHistoryProduct.Substring(1);
           }
           if (strHistoryProduct.EndsWith(","))
           {
               strHistoryProduct = strHistoryProduct.Substring(0, strHistoryProduct.Length - 1);
           }
          if(!string.IsNullOrEmpty(strHistoryProduct)) proHistoryList = ProductBLL.SearchList(currentPage, pageSize, new ProductSearchInfo { IsSale = 1, InProductId = strHistoryProduct }, ref count);
           commonPagerClass.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));       
        
       }

       protected string GetPrice(int id, decimal price, string standardValue)
       {
           if (!string.IsNullOrEmpty(standardValue.Trim()))
           {
               return ProductBLL.GetCurrentPriceWithStandard(id, base.GradeID, standardValue).ToString();
           }
           else
           {
               return ProductBLL.GetCurrentPrice(price, base.GradeID).ToString();
           }
       }
    }
}
