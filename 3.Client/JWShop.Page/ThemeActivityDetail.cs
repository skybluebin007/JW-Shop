using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SocoShop.Common;
using SocoShop.Business;
using SocoShop.Entity;
using SkyCES.EntLib;

namespace SocoShop.Page
{
    public class ThemeActivityDetail:CommonBasePage
    {
        private string footaddress = string.Empty;
        /// <summary>
        /// 标题
        /// </summary>
        public string FootAddress
        {
            set { this.footaddress = value; }
            get
            {
                string temp = ShopConfig.ReadConfigInfo().FootAddress;
                if (this.footaddress != string.Empty)
                {
                    temp = this.footaddress + " - " + ShopConfig.ReadConfigInfo().FootAddress;
                }
                return temp;
            }
        }
        /// <summary>
        /// 专题活动
        /// </summary>
        protected ThemeActivityInfo themeActivity = new ThemeActivityInfo();
        /// <summary>
        /// 样式
        /// </summary>
        protected string[] styleArray;
        /// <summary>
        /// 产品组
        /// </summary>
        protected string[] productGroupArray=new string[0];
        /// <summary>
        /// 产品列表
        /// </summary>
        protected List<ProductInfo> productList = new List<ProductInfo>();
        /// <summary>
        /// 会员价格
        /// </summary>
        protected List<MemberPriceInfo> memberPriceList = new List<MemberPriceInfo>();
        /// <summary> 
        /// 页面加载
        /// </summary>
         protected override void PageLoad()
        {
            base.PageLoad();
            int id = RequestHelper.GetQueryString<int>("ID");
            themeActivity = ThemeActivityBLL.ReadThemeActivity(id);
            styleArray = themeActivity.Style.Split('|');           
            if (themeActivity.ProductGroup != string.Empty)
            {
                string productIDList = string.Empty;
                productGroupArray = themeActivity.ProductGroup.Split('#');
                for(int i=0;i<productGroupArray.Length;i++)
                {
                    if (productGroupArray[i].Split('|')[2] != string.Empty)
                    {
                        if (productIDList == string.Empty)
                        {
                            productIDList = productGroupArray[i].Split('|')[2];
                        }
                        else
                        {
                            productIDList += "," + productGroupArray[i].Split('|')[2];
                        }
                    }
                }
                if (productIDList != string.Empty)
                {
                    ProductSearchInfo productSearch = new ProductSearchInfo();
                    productSearch.InProductID = productIDList;
                    productList = ProductBLL.SearchProductList(productSearch);
                    memberPriceList = MemberPriceBLL.ReadMemberPriceByProductGrade(productIDList, base.GradeID);
                }
            }
            Title = themeActivity.Name;
        }
    }
}
