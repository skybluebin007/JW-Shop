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
using System.Net;

namespace JWShop.Page
{
    public class Default : CommonBasePage
    {
        protected List<ArticleInfo> artListInfo = new List<ArticleInfo>();
        protected List<AdImageInfo> imageList = new List<AdImageInfo>();
        protected List<ProductInfo> likeProductList = new List<ProductInfo>();
        /// <summary>
        /// 最新动态  
        /// </summary>
        protected List<ArticleInfo> topnewsList = new List<ArticleInfo>();
        /// <summary>
        /// 养老政策
        /// </summary>
        protected List<ArticleInfo> topylzcList = new List<ArticleInfo>();

        protected string localIP = string.Empty;
        /// <summary>
        /// 图片链接
        /// </summary>
        protected List<LinkInfo> pictureLinkList = new List<LinkInfo>();
        protected List<ProductBrandInfo> proBranInfo = new List<ProductBrandInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            topNav = 1;

            IPHostEntry ipHost = System.Net.Dns.GetHostEntry(Dns.GetHostName());// Dns.Resolve(Dns.GetHostName()); ;

            foreach (IPAddress ipa in ipHost.AddressList)
            {
                if (ipa.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ipa.ToString();
                }
            }
            //IPAddress ipaddress = ipHost.AddressList[0];
            //localIP = ipaddress.ToString();
            //imageList = AdImageBLL.ReadList();
            //ProductBrandBLL.ReadList(
            proBranInfo = ProductBrandBLL.ReadList();
            List<AdImageInfo> flashList = AdImageBLL.ReadList(6);
            int count = 0;
            //likeProductList = ProductBLL.SearchList(1, 5, new ProductSearchInfo { ProductOrderType = "LikeNum", IsSale = (int)BoolType.True }, ref count);
            artListInfo = ArticleBLL.SearchList(1, 4, new ArticleSearchInfo { ClassId = "|58|" }, ref count);
            topnewsList = ArticleBLL.SearchList(1, 5, new ArticleSearchInfo { ClassId = "|38|", IsTop = (int)BoolType.True }, ref count);
            topylzcList = ArticleBLL.SearchList(1, 6, new ArticleSearchInfo { ClassId = "|47|", IsTop = (int)BoolType.True }, ref count);
            //newProductList = ProductBLL.SearchList(1, 5, new ProductSearchInfo { IsNew = (int)BoolType.True, IsTop = (int)BoolType.True, IsSale = (int)BoolType.True }, ref count);

            //hotProductList = ProductBLL.SearchList(1, 10, new ProductSearchInfo { IsHot = (int)BoolType.True, IsTop = (int)BoolType.True, IsSale = (int)BoolType.True }, ref count);

            //specialProductList = ProductBLL.SearchList(1, 10, new ProductSearchInfo { IsSpecial = (int)BoolType.True, IsTop = (int)BoolType.True, IsSale = (int)BoolType.True }, ref count);           

            //textLinkList = LinkBLL.ReadLinkCacheListByClass((int)LinkType.Text);

            //pictureLinkList = LinkBLL.ReadLinkCacheListByClass((int)LinkType.Picture);
            //在线咨询
            if (RequestHelper.GetQueryString<string>("Action") == "AskOnline")
            {
                AskOnline();
            }
        }
        /// <summary>
        /// 在线咨询
        /// </summary>
        protected void AskOnline()
        {
            string errorMsg = string.Empty;
            int messageType = 6;

            string name = StringHelper.SearchSafe(RequestHelper.GetForm<string>("Name"));

            string email = StringHelper.SearchSafe(RequestHelper.GetForm<string>("Email"));
            string Tel = StringHelper.SearchSafe(RequestHelper.GetForm<string>("Tel"));
            string Content = StringHelper.SearchSafe(RequestHelper.GetForm<string>("Content"));
            string safeCode = StringHelper.SearchSafe(RequestHelper.GetForm<string>("SafeCode"));
            //检查验证码
            if (errorMsg == string.Empty)
            {
                if (safeCode.ToLower() != Cookies.Common.CheckCode.ToLower())
                {
                    errorMsg = "验证码错误";
                    Response.Write("error|" + errorMsg);
                    Response.End();
                }
            }
            if (errorMsg == string.Empty)
            {
                try
                {
                    UserMessageInfo userMessage = new UserMessageInfo();
                    userMessage.MessageClass = messageType;
                    userMessage.UserIP = ClientHelper.IP;
                    userMessage.PostDate = RequestHelper.DateNow;
                    userMessage.IsHandler = (int)BoolType.False;
                    userMessage.AdminReplyContent = string.Empty; ;
                    userMessage.AdminReplyDate = RequestHelper.DateNow;
                    userMessage.UserId = 0;
                    userMessage.Title = string.Empty;
                    userMessage.UserName = name;
                    userMessage.Tel = Tel;
                    userMessage.Email = email;

                    userMessage.Content = Content;

                    UserMessageBLL.Add(userMessage);
                    Response.Clear();
                    Response.Write("ok|提交成功");

                }
                catch (Exception ex)
                {
                    //Response.Write("error|" + StringHelper.Substring(ex.ToString(), 10));
                    Response.Clear();
                    Response.Write("error|" + "出错了，请稍后重试");

                }
                finally
                {
                    Response.End();
                }
            }

        }
    }
}