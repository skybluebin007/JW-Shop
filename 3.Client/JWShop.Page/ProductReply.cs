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
    public class ProductReply : CommonBasePage
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
        /// 产品
        /// </summary>
        protected ProductInfo product = new ProductInfo();
        /// <summary>
        /// 产品评论
        /// </summary>
        protected ProductCommentInfo productComment = new ProductCommentInfo();
        protected int commentID;
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            commentID = RequestHelper.GetQueryString<int>("CommentID");
            productComment = ProductCommentBLL.ReadProductComment(commentID, 0);
            product = ProductBLL.ReadProduct(productComment.ProductID);
        }
    }
}
