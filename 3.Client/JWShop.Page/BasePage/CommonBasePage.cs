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

        protected List<NavMenuInfo> topNavMenuList = new List<NavMenuInfo>();

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


        protected override void PageLoad()
        {
            ProductClassList = ProductClassBLL.ReadList();
            TopBanner = AdImageBLL.ReadList((int)AdImageType.TopBanner).FirstOrDefault() ?? new AdImageInfo();

            hotKeyword = ShopConfig.ReadConfigInfo().HotKeyword;
            //productClassList = ProductClassBLL.ReadRootList();
            helpClassList = ArticleClassBLL.ReadChilds(ArticleClass.Help);
            bottomList = ArticleBLL.ReadBottomList();

            topNavMenuList = NavMenuBLL.ReadList(true);
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

    }
}