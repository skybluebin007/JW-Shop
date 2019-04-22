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
    public class List : CommonBasePage
    {
        protected ProductClassInfo currentClass = new ProductClassInfo();
        protected string keywords;
        protected int count = 0;
        
        //商品列表数据
        protected List<ProductInfo> productList = new List<ProductInfo>();

        /// <summary>
        /// 搜索条件
        /// </summary>
        protected string searchCondition = string.Empty;

        protected override void PageLoad()
        {
            base.PageLoad();

            int classID = RequestHelper.GetQueryString<int>("cat");
            //if (classID < 0)
            //{
            //    ScriptHelper.Alert("无效访问", "/Default.aspx");
            //    Response.End();
            //}
            string productName = RequestHelper.GetQueryString<string>("Keyword");
            int brandID = RequestHelper.GetQueryString<int>("BrandID");
            string tags = RequestHelper.GetQueryString<string>("Tags");
            int isNew = RequestHelper.GetQueryString<int>("IsNew");
            int isHot = RequestHelper.GetQueryString<int>("IsHot");
            int isSpecial = RequestHelper.GetQueryString<int>("IsSpecial");
            int isTop = RequestHelper.GetQueryString<int>("IsTop");
            searchCondition = "ClassID=" + classID.ToString() + "&ProductName=" + productName + "&BrandID=" + brandID.ToString() + "&IsNew=" + isNew + "&IsHot=" + isHot + "&IsSpecial=" + isSpecial + "&IsTop=" + isTop + "";   
        }

    }
}