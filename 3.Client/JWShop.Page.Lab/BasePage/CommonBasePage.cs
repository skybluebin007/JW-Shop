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
    public abstract class CommonBasePage : BasePage
    {
        protected AdImageInfo TopBanner = new AdImageInfo();
        protected List<ProductClassInfo> ProductClassList = new List<ProductClassInfo>();

        protected override void PageLoad()
        {
            ProductClassList = ProductClassBLL.ReadList();
            TopBanner = AdImageBLL.ReadList((int)AdImageType.TopBanner).FirstOrDefault() ?? new AdImageInfo();
        }

        private string title = string.Empty;
        private string keywords = string.Empty;
        private string description = string.Empty;
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