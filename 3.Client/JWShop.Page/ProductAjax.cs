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

namespace JWShop.Page
{
    public class ProductAjax : AjaxBasePage
    {        
        /// <summary>
        /// 产品列表
        /// </summary>
        protected List<ProductInfo> productList = new List<ProductInfo>();
        /// <summary>
        /// Ajax分页
        /// </summary>
        protected AjaxPagerClass ajaxPagerClass = new AjaxPagerClass();
        /// <summary>
        /// 会员价格
        /// </summary>
        protected List<MemberPriceInfo> memberPriceList = new List<MemberPriceInfo>();
        /// <summary>
        /// 是否要单独计算价格
        /// </summary>
        protected bool countPriceSingle = false;
        /// <summary>
        /// 产品图片列表
        /// </summary>
        protected List<ProductPhotoInfo> productPhotoList = new List<ProductPhotoInfo>();
        /// <summary>
        /// 产品呈现方式
        /// </summary>
        protected int productShowWay = (int)ProductShowWay.Picture;
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            if (CookiesHelper.ReadCookieValue("ProductShowWay") != string.Empty)
            {
                productShowWay = Convert.ToInt32(CookiesHelper.ReadCookieValue("ProductShowWay"));
            }

            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 20;
            if (productShowWay == (int)ProductShowWay.List)
            {
                pageSize = 10;
            }
            int count = 0;
            //搜索条件处理
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.IsSale = (int)BoolType.True;
            productSearch.ProductOrderType = CookiesHelper.ReadCookieValue("ProductOrderType");
            int searchType = RequestHelper.GetQueryString<int>("SearchType");
            //if (searchType == (int)ProductSearchType.Key)
            //{
            string classID = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("cat"));
            if (classID != int.MinValue.ToString() && classID != "")
            {
                classID = "|" + classID + "|";
            }
            else
            {
                classID = string.Empty;
            }
            productSearch.ClassId = classID;
            productSearch.Key = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("ProductName"));
            productSearch.BrandId = RequestHelper.GetQueryString<int>("BrandID");
            //}
            //else
            //{
            productSearch.IsNew = RequestHelper.GetQueryString<int>("IsNew");
            productSearch.IsHot = RequestHelper.GetQueryString<int>("IsHot");
            productSearch.IsSpecial = RequestHelper.GetQueryString<int>("IsSpecial");
            productSearch.IsTop = RequestHelper.GetQueryString<int>("IsTop");
            //}
            //搜索产品
            
                productList = ProductBLL.SearchList(currentPage, pageSize, productSearch, ref count);


                //读取会员价格
                countPriceSingle = true;
                string strProductID = string.Empty;
                foreach (ProductInfo product in productList)
                {

                    if (strProductID == string.Empty)
                    {
                        strProductID = product.Id.ToString();
                    }
                    else
                    {
                        strProductID += "," + product.Id.ToString();
                    }
                }                
            

            ajaxPagerClass.CurrentPage = currentPage;
            ajaxPagerClass.PageSize = pageSize;
            ajaxPagerClass.Count = count;
            ajaxPagerClass.FirstPage = "<<首页";
            ajaxPagerClass.PreviewPage = "<<上一页";
            ajaxPagerClass.NextPage = "下一页>>";
            ajaxPagerClass.LastPage = "末页>>";
            ajaxPagerClass.ListType = false;
            ajaxPagerClass.DisCount = false;
            ajaxPagerClass.PrenextType = true;
        }
    }
}
