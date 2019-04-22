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

namespace JWShop.Page
{
    public class Default : CommonBasePage
    {
        protected List<AdImageInfo> imageList = new List<AdImageInfo>();
        protected List<ProductInfo> likeProductList = new List<ProductInfo>();
        /// <summary>
        /// 新闻资讯
        /// </summary>
        protected List<ArticleInfo> newsList = new List<ArticleInfo>();
        /// <summary>
        /// 新品上市
        /// </summary>
        protected List<ProductInfo> newProductList = new List<ProductInfo>();
        /// <summary>
        /// 热销商品
        /// </summary>
        protected List<ProductInfo> hotProductList = new List<ProductInfo>();
        /// <summary>
        /// 特价商品
        /// </summary>
        protected List<ProductInfo> specialProductList = new List<ProductInfo>();
        /// <summary>
        /// 会员价格
        /// </summary>
        protected List<MemberPriceInfo> memberPriceList = new List<MemberPriceInfo>();
        /// <summary>
        /// 文字链接
        /// </summary>
        protected List<LinkInfo> textLinkList = new List<LinkInfo>();
        /// <summary>
        /// 图片链接
        /// </summary>
        protected List<LinkInfo> pictureLinkList = new List<LinkInfo>();
        

        protected override void PageLoad()
        {
            base.PageLoad();

            topNav = 1;

            imageList = AdImageBLL.ReadList();

            

            int count = 0;
            likeProductList = ProductBLL.SearchList(1, 5, new ProductSearchInfo { ProductOrderType = "LikeNum", IsSale = (int)BoolType.True }, ref count);

            newsList = ArticleBLL.SearchList(1, 7, new ArticleSearchInfo { ClassId = "|1|", IsTop = (int)BoolType.True }, ref count);
            
            newProductList = ProductBLL.SearchList(1, 5, new ProductSearchInfo { IsNew = (int)BoolType.True, IsTop = (int)BoolType.True, IsSale = (int)BoolType.True }, ref count);
            
            hotProductList = ProductBLL.SearchList(1, 10, new ProductSearchInfo { IsHot = (int)BoolType.True, IsTop = (int)BoolType.True, IsSale = (int)BoolType.True }, ref count);
            
            specialProductList = ProductBLL.SearchList(1, 10, new ProductSearchInfo { IsSpecial = (int)BoolType.True, IsTop = (int)BoolType.True, IsSale = (int)BoolType.True }, ref count);           

            textLinkList = LinkBLL.ReadLinkCacheListByClass((int)LinkType.Text);

            pictureLinkList = LinkBLL.ReadLinkCacheListByClass((int)LinkType.Picture);

        }
    }
}