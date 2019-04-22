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
    public partial class ProductAjax : JWShop.Page.AdminBasePage
    {
        protected List<ProductInfo> productList = new List<ProductInfo>();
        protected List<ArticleInfo> articleList = new List<ArticleInfo>();
        protected List<EmailContentInfo> emailContentList = new List<EmailContentInfo>();
        protected List<UserInfo> userList = new List<UserInfo>();
        protected string controlName = string.Empty;
        protected string action = string.Empty;
        protected string cssContent = string.Empty;
        protected string dobuleClickContent = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            ClearCache();

            controlName = RequestHelper.GetQueryString<string>("ControlName");
            action = RequestHelper.GetQueryString<string>("Action");
            string flag = RequestHelper.GetQueryString<string>("Flag");
            switch (action)
            {
                case "SearchRelationProduct":
                    SearchRelationProduct();
                    cssContent = "class=\"all\" multiple";
                    dobuleClickContent = " ondblclick=\"addSingle('" + IDPrefix + "CandidateProduct','" + IDPrefix + "Product')\"";
                    break;
                case "SearchProductAccessory":
                    SearchProductAccessory();
                    cssContent = "class=\"all\" multiple";
                    dobuleClickContent = " ondblclick=\"addProductAccessorySingle('" + IDPrefix + "CandidateAccessory','" + IDPrefix + "Accessory')\"";
                    break;
                case "SearchRelationArticle":
                    SearchRelationArticle();
                    cssContent = "class=\"all\" multiple";
                    dobuleClickContent = " ondblclick=\"addSingle('" + IDPrefix + "CandidateArticle','" + IDPrefix + "Article')\"";
                    break;
                case "SearchUser":
                    SearchUser();
                    cssContent = "style=\"width:180px; height:300px;\"multiple=\"multiple\"";
                    cssContent = "class=\"all\" multiple";
                    dobuleClickContent = " ondblclick=\"addSingle('" + IDPrefix + "CandidateUser','" + IDPrefix + "User')\"";
                    break;
                case "SearchProductByName":
                    SearchProductByName();
                    cssContent = "style=\"width:240px;\"";
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 搜索商品
        /// </summary>
        private void SearchProductByName()
        {
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.Name = RequestHelper.GetQueryString<string>("ProductName");
            productList = ProductBLL.SearchList(productSearch);
        }
        /// <summary>
        /// 搜索关联商品
        /// </summary>
        private void SearchRelationProduct()
        {
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.NotInProductId = RequestHelper.GetQueryString<string>("ID");
            productSearch.Name = RequestHelper.GetQueryString<string>("ProductName");
            productSearch.ClassId = RequestHelper.GetQueryString<string>("ClassID");
            productSearch.BrandId = RequestHelper.GetQueryString<int>("BrandID");
            productSearch.IsSale = (int)BoolType.True;
            productList = ProductBLL.SearchList(productSearch);
        }
        /// <summary>
        /// 搜索配件
        /// </summary>
        private void SearchProductAccessory()
        {
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.NotInProductId = RequestHelper.GetQueryString<string>("ID");
            productSearch.Name = RequestHelper.GetQueryString<string>("ProductName");
            productSearch.ClassId = RequestHelper.GetQueryString<string>("ClassID");
            productSearch.BrandId = RequestHelper.GetQueryString<int>("BrandID");
            productList = ProductBLL.SearchList(productSearch);
        }
        /// <summary>
        /// 搜索关联文章
        /// </summary>
        private void SearchRelationArticle()
        {
            ArticleSearchInfo articleSearch = new ArticleSearchInfo();
            articleSearch.Title = RequestHelper.GetQueryString<string>("ArticleTitle");
            articleSearch.ClassId = RequestHelper.GetQueryString<string>("ClassID");
            articleList = ArticleBLL.SearchList(articleSearch);
        }
        /// <summary>
        /// 搜索用户
        /// </summary>
        private void SearchUser()
        {
            UserSearchInfo userSearch = new UserSearchInfo();
            userSearch.UserName = RequestHelper.GetQueryString<string>("UserName");
            userList = UserBLL.SearchList(userSearch); 
        }
    }
}