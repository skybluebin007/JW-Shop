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
using System.Text;

namespace JWShop.Web.Admin
{
    public partial class ProductCommentAdd : JWShop.Page.AdminBasePage
    {
        protected ProductCommentInfo productComment = new ProductCommentInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = RequestHelper.GetQueryString<int>("Id");
                if (id != int.MinValue)
                {
                    CheckAdminPower("ReadProductComment", PowerCheckType.Single);
                    productComment = ProductCommentBLL.Read(id);
                    Name.Text = ProductBLL.Read(productComment.ProductId).Name;
                    //Name.NavigateUrl = "/ProductDetail-I" + productComment.ProductId + ".html";
                    //Name.Target = "_blank";
                    Content.Text = productComment.Content;
                    UserIP.Text = productComment.UserIP;
                    PostDate.Text = productComment.PostDate.ToString();
                    Status.Text = productComment.Status.ToString();
                    Rank.Text = productComment.Rank.ToString();
                    UserName.Text = HttpUtility.UrlDecode(productComment.UserName, Encoding.UTF8);
                    AdminReplyContent.Text = productComment.AdminReplyContent;
                }
            }
        }

        /// <summary>
        /// 提交按钮点击方法
        /// </summary>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            int id = RequestHelper.GetQueryString<int>("Id");
            if (id > 0)
            {
                ProductCommentInfo productComment = ProductCommentBLL.Read(id);
                int oldStatus = productComment.Status;
                productComment.Status = Convert.ToInt32(Status.Text);
                productComment.AdminReplyContent = AdminReplyContent.Text;
                productComment.AdminReplyDate = RequestHelper.DateNow;
                CheckAdminPower("UpdateProductComment", PowerCheckType.Single);
                ProductCommentBLL.Update(productComment, oldStatus);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("ProductComment"), productComment.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}