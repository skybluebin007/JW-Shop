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
    public abstract class CommonBasePage : BasePage
    {
        protected AdImageInfo TopBanner = new AdImageInfo();
        protected List<ProductClassInfo> ProductClassList = new List<ProductClassInfo>();

        
        protected int istop = 0;
        /// <summary>
        /// 热门搜索关键字
        /// </summary>
        protected string hotKeyword = string.Empty;
        /// <summary>
        /// 产品一级分类
        /// </summary>
        protected List<ProductClassInfo> productClassList = new List<ProductClassInfo>();
        /// <summary>
        /// 帮助分类
        /// </summary>
        protected List<ArticleClassInfo> helpClassList = new List<ArticleClassInfo>();
        /// <summary>
        /// 底部文章
        /// </summary>
        protected List<ArticleInfo> bottomList = new List<ArticleInfo>();

        protected string strHistorySearch = string.Empty;
        /// <summary>
        /// 友情链接
        /// </summary>
        protected List<LinkInfo> textLinkList = new List<LinkInfo>();

        protected List<NavMenuInfo> topNavMenuList = new List<NavMenuInfo>();

        protected override void PageLoad()
        {
            ProductClassList = ProductClassBLL.ReadList();
            TopBanner = AdImageBLL.ReadList((int)AdImageType.TopBanner).FirstOrDefault() ?? new AdImageInfo();

            hotKeyword = ShopConfig.ReadConfigInfo().HotKeyword;
            productClassList = ProductClassBLL.ReadRootList();
            helpClassList = ArticleClassBLL.ReadChilds(ArticleClass.Help);
            bottomList = ArticleBLL.ReadBottomList();

            strHistorySearch = Server.UrlDecode(CookiesHelper.ReadCookieValue("HistorySearch"));
            
            textLinkList = LinkBLL.ReadLinkCacheListByClass((int)LinkType.Text);

            topNavMenuList = NavMenuBLL.ReadList(true);

            ReadCart();
        }

        private string title = string.Empty;
        private string keywords = string.Empty;
        private string description = string.Empty;
        protected int topNav = 0;//主菜单当前状态
        protected string navList = "";//面包屑导航
        protected string navList2 = "";//手机站面包悄导航
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            set { this.title = value; }
            get
            {
                string temp = ShopConfig.ReadConfigInfo().Title;
                if (this.title != string.Empty)
                {
                    temp = this.title + " - " + ShopConfig.ReadConfigInfo().Title;
                }
                return temp;
            }
        }
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords
        {
            set { this.keywords = value; }
            get
            {
                string temp = this.keywords;
                if (temp == string.Empty)
                {
                    temp = ShopConfig.ReadConfigInfo().Keywords;
                }
                return temp;
            }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            set { this.description = value; }
            get
            {
                string temp = this.description;
                if (temp == string.Empty)
                {
                    temp = ShopConfig.ReadConfigInfo().Description;
                }
                return temp;
            }
        }
        /// <summary>
        ///  版权
        /// </summary>
        public string Copyright
        {
            get
            {
                return ShopConfig.ReadConfigInfo().Copyright;
            }
        }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author
        {
            get
            {
                return ShopConfig.ReadConfigInfo().Author;
            }
        }
        /// <summary>
        /// 手机站统计代码
        /// </summary>
        protected class CS
        {
            private int SiteId = 0;
            private const string ImageDomain = "c.cnzz.com";
            public CS(int SiteId)
            {
                this.SiteId = SiteId;
            }
            public string TrackPageView()
            {
                HttpRequest request = HttpContext.Current.Request;
                string scheme = request != null ? request.IsSecureConnection ? "https" : "http" : "http";
                string referer = request != null && request.UrlReferrer != null && "" != request.UrlReferrer.ToString() ? request.UrlReferrer.ToString() : "";
                String rnd = new Random().Next(0x7fffffff).ToString();
                return scheme + "://" + CS.ImageDomain + "/wapstat.php" + "?siteid=" + this.SiteId + "&r=" + HttpUtility.UrlEncode(referer) + "&rnd=" + rnd;
            }
        }

        /// <summary>
        /// 读取购物车--Sessions.ProductBuyCount
        /// </summary>
        private void ReadCart()
        {
          var  cartList = CartBLL.ReadList(base.UserId);

            //关联的商品
            int count = 0;
            int[] ids = cartList.Select(k => k.ProductId).ToArray();
            var products = ProductBLL.SearchList(1, ids.Count(), new ProductSearchInfo { InProductId = string.Join(",", ids) }, ref count);

            int productCount = 0;
            //规格
            foreach (var cart in cartList)
            {
                cart.Product = products.FirstOrDefault(k => k.Id == cart.ProductId) ?? new ProductInfo();

                if (!string.IsNullOrEmpty(cart.StandardValueList))
                {                  
                    productCount += cart.BuyCount;
                }
                else
                {
               
                    productCount += cart.BuyCount;
                }
            }
            Sessions.ProductBuyCount = productCount;
        }
    }
}