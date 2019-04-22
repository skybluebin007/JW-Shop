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

namespace JWShop.Page.Mobile
{
    public class UserProductCollect : UserBasePage
    {
        /// <summary>
        /// 用户等级
        /// </summary>
        protected int userGradeId = 0;
        /// <summary>
        /// 用户信息
        /// </summary>
        protected UserInfo user = new UserInfo();
        /// <summary>
        /// 用户收藏
        /// </summary>
        protected List<ProductCollectInfo> productCollectList = new List<ProductCollectInfo>();
        /// <summary>
        /// 产品列表
        /// </summary>
        protected List<ProductInfo> productList = new List<ProductInfo>();
        /// <summary>
        /// 分页
        /// </summary>
        protected CommonPagerClass commonPagerClass = new CommonPagerClass();
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

            user = UserBLL.ReadUserMore(base.UserId);
            userGradeId = UserGradeBLL.Read(base.GradeID).Id;
            string action = RequestHelper.GetQueryString<string>("Action");
            if (action == "Delete")
            {
                string deleteID = RequestHelper.GetQueryString<string>("ID");
                ProductCollectBLL.Delete(Array.ConvertAll(deleteID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k)), base.UserId);
                ResponseHelper.Write("ok");
                ResponseHelper.End();
            }

            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 20;
            int count = 0;
            productCollectList = ProductCollectBLL.ReadList(currentPage, pageSize, ref count, base.UserId);
            //commonPagerClass.CurrentPage = currentPage;
            //commonPagerClass.PageSize = pageSize;
            //commonPagerClass.Count = count;
            //commonPagerClass.FirstPage = "<<首页";
            //commonPagerClass.PreviewPage = "<<上一页";
            //commonPagerClass.NextPage = "下一页>>";
            //commonPagerClass.LastPage = "末页>>";
            //commonPagerClass.ListType = false;
            //commonPagerClass.DisCount = false;
            //commonPagerClass.PrenextType = true;
            commonPagerClass.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));

            string strProductID = string.Empty;
            foreach (ProductCollectInfo productCollect in productCollectList)
            {
                if (strProductID == string.Empty)
                {
                    strProductID = productCollect.ProductId.ToString();
                }
                else
                {
                    strProductID += "," + productCollect.ProductId.ToString();
                }
            }

            if (strProductID != string.Empty)
            {
                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.InProductId = strProductID;
                productList = ProductBLL.SearchList(productSearch);
            }

        }
    }
}
