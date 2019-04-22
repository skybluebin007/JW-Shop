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
    public class Product : CommonBasePage
    {
        protected SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();

        protected List<ProductPhotoInfo> proPhotoList = new List<ProductPhotoInfo>();
        protected ArticleInfo thisArticle = new ArticleInfo();
        protected List<ProductInfo> ProductHot = new List<ProductInfo>();
        /// <summary>
        /// 搜索条件
        /// </summary>
        protected string searchCondition = string.Empty;
        /// <summary>
        /// 显示搜索的条件
        /// </summary>
        protected string showCondition = string.Empty;
        /// <summary>
        /// 显示的标题
        /// </summary>
        protected string showTitle = string.Empty;
        /// <summary>
        /// 相关搜索
        /// </summary>
        protected string relationSearch = string.Empty;
        /// <summary>
        /// 会员价格
        /// </summary>
        protected List<MemberPriceInfo> memberPriceList = new List<MemberPriceInfo>();
        /// <summary>
        /// 是否要单独计算价格
        /// </summary>
        protected bool countPriceSingle = false;
        protected int artId = 0;
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            //singleUnlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();
            var regionList = RegionBLL.ReadRegionUnlimitClass();
            regionList.RemoveAll(k => k.FatherID == 0 || (k.FatherID == 1 && k.ClassID != 27) );
            regionList.ForEach(k => { if (k.FatherID == 1) k.FatherID = 0; });
            singleUnlimitClass.DataSource = regionList;
            //singleUnlimitClass.ClassID = "|27|";
            //singleUnlimitClass.Prefix = "湖南";

            SearchCondition();
            LoadHotProductList();
            artId = RequestHelper.GetQueryString<int>("ID");
            if (artId <= 0) artId = 1;
            thisArticle = ArticleBLL.Read(artId);

            switch (thisArticle.ClassId)
            {
                case "|58|54|": topNav = 2;
                    break;
                case "|58|55|": topNav = 3;
                    break;
                case "|58|56|": topNav = 4;
                    break;
                case "|58|57|": topNav = 5;
                    break;
                default:
                    break;
            }
            proPhotoList = ProductPhotoBLL.ReadList(artId, 1);
            if (RequestHelper.GetQueryString<string>("Action") == "UseMessage") { UseMessage(); }
        }
        private void UseMessage()
        {
            UserMessageInfo usmInfo = new UserMessageInfo();
            string result = "ok|/HZ/product.html";
            string borthdate = RequestHelper.GetForm<string>("J-xl");//到岗日期
            if (string.IsNullOrEmpty(borthdate))
            {
                borthdate = RequestHelper.GetForm<string>("datetime");
            }
            int fwdate = RequestHelper.GetForm<int>("fwdate");//服务天数
            string fwStyle = RequestHelper.GetForm<string>("fwStyle");//服务方式
            string jiguan = RegionBLL.RegionNameList(singleUnlimitClass.ClassID);
            if (string.IsNullOrEmpty(jiguan))
            {
                jiguan = RequestHelper.GetForm<string>("checkCity") + " " + RequestHelper.GetForm<string>("checkCity1") + " " + RequestHelper.GetForm<string>("checkCity2");
            }
            string address = RequestHelper.GetForm<string>("address");
            if (string.IsNullOrEmpty(address))
            {
                address = RequestHelper.GetForm<string>("orAddress");
            }
            string name = RequestHelper.GetForm<string>("name");
            string phone = RequestHelper.GetForm<string>("phone");
            string content = RequestHelper.GetForm<string>("content");

            usmInfo.Title = thisArticle.Title;
            usmInfo.Servedays = fwdate;
            usmInfo.UserName = name;
            usmInfo.Tel = phone;
            usmInfo.Email = fwStyle;
            usmInfo.Birthday = borthdate;
            usmInfo.Address = address;
            usmInfo.AddCol1 = jiguan;
            usmInfo.Content = content;
            usmInfo.MessageClass = 7;
            usmInfo.UserIP = ClientHelper.IP;
            UserMessageBLL.Add(usmInfo);
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
        private void LoadHotProductList()
        {
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.IsHot = 1;
            productSearch.IsSale = 1;
            ProductHot = ProductBLL.SearchList(productSearch);
            //读取会员价格
            countPriceSingle = true;
            string strProductID = string.Empty;
            foreach (ProductInfo product in ProductHot)
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

        }
        /// <summary>
        /// 搜索条件
        /// </summary>
        protected void SearchCondition()
        {
            int classID = RequestHelper.GetQueryString<int>("ClassID");
            string productName = RequestHelper.GetQueryString<string>("Keyword");
            int brandID = RequestHelper.GetQueryString<int>("BrandID");
            string tags = RequestHelper.GetQueryString<string>("Tags");
            int isNew = RequestHelper.GetQueryString<int>("IsNew");
            int isHot = RequestHelper.GetQueryString<int>("IsHot");
            int isSpecial = RequestHelper.GetQueryString<int>("IsSpecial");
            int isTop = RequestHelper.GetQueryString<int>("IsTop");
            searchCondition = "ClassID=" + classID.ToString() + "&ProductName=" + productName + "&BrandID="
                + brandID.ToString() + "&IsNew=" + isNew + "&IsHot=" + isHot + "&IsSpecial=" + isSpecial + "&IsTop=" + isTop + "";


            Title = showTitle + " - 商品展示";
        }
    }
}
