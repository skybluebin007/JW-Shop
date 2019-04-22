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

namespace JWShop.Web.Admin
{
    public partial class ProductComment : JWShop.Page.AdminBasePage
    {

        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadProductComment", PowerCheckType.Single);

                Name.Text = RequestHelper.GetQueryString<string>("Name");

                StartPostDate.Text = RequestHelper.GetQueryString<string>("StartPostDate");
                EndPostDate.Text = RequestHelper.GetQueryString<string>("EndPostDate");
                Status.Text = RequestHelper.GetQueryString<string>("Status");

                ProductCommentSearchInfo productCommentSearch = new ProductCommentSearchInfo();
                productCommentSearch.ProductName = RequestHelper.GetQueryString<string>("Name");
                productCommentSearch.Title = RequestHelper.GetQueryString<string>("Title");
                productCommentSearch.Content = RequestHelper.GetQueryString<string>("Content");
                productCommentSearch.UserIP = RequestHelper.GetQueryString<string>("UserIP");
                productCommentSearch.StartPostDate = RequestHelper.GetQueryString<DateTime>("StartPostDate");
                productCommentSearch.EndPostDate = RequestHelper.GetQueryString<DateTime>("EndPostDate");
                productCommentSearch.Status = RequestHelper.GetQueryString<int>("Status");
                PageSize = Session["AdminPageSize"] == null ? 20 : Convert.ToInt32(Session["AdminPageSize"]);
                AdminPageSize.Text = Session["AdminPageSize"] == null ? "20" : Session["AdminPageSize"].ToString();
                BindControl(ProductCommentBLL.SearchList(CurrentPage, PageSize, productCommentSearch, ref Count), RecordList, MyPager);
            }
        }

        /// <summary>
        /// 删除按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteProductComment", PowerCheckType.Single);
            string[] deleteArr = RequestHelper.GetIntsForm("SelectID").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (deleteArr.Length>0)
            {
                ProductCommentBLL.Delete(Array.ConvertAll<string, int>(deleteArr, s => Convert.ToInt32(s)));
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("ProductComment"), string.Join(",",deleteArr));
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        /// <summary>
        /// 评论状态变成未处理状态按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NoHandlerButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("UpdateProductComment", PowerCheckType.Single);
            string[] selectID = RequestHelper.GetIntsForm("SelectID").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (selectID.Length>0)
            {
                ProductCommentBLL.ChangeStatus(Array.ConvertAll<string, int>(selectID, s => Convert.ToInt32(s)), (int)CommentStatus.NoHandler);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("OperateOK"), RequestHelper.RawUrl);
            }
        }
        /// <summary>
        /// 评论状态变成不显示状态按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NoShowButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("UpdateProductComment", PowerCheckType.Single);
            string[] selectID = RequestHelper.GetIntsForm("SelectID").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (selectID.Length > 0)
            {
                ProductCommentBLL.ChangeStatus(Array.ConvertAll<string, int>(selectID, s => Convert.ToInt32(s)), (int)CommentStatus.NoShow);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("OperateOK"), RequestHelper.RawUrl);
            }
        }
        /// <summary>
        /// 评论状态变成显示状态按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("UpdateProductComment", PowerCheckType.Single);
            string[] selectID = RequestHelper.GetIntsForm("SelectID").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (selectID.Length > 0)
            {
                ProductCommentBLL.ChangeStatus(Array.ConvertAll<string, int>(selectID, s => Convert.ToInt32(s)), (int)CommentStatus.Show);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("OperateOK"), RequestHelper.RawUrl);
            }
        }
        /// <summary>
        /// 搜索按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "ProductComment.aspx?Action=search&";
            URL += "Name=" + Name.Text + "&";
            //URL += "Title=" + Title.Text + "&";
            URL += "StartPostDate=" + StartPostDate.Text + "&";
            URL += "EndPostDate=" + EndPostDate.Text + "&";
            URL += "Status=" + Status.Text;
            ResponseHelper.Redirect(URL);
        }
        /// <summary>
        /// 每页显示条数控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AdminPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["AdminPageSize"] = AdminPageSize.Text;
            string URL = "ProductComment.aspx?Action=search&";
            URL += "Name=" + Name.Text + "&";
            //URL += "Title=" + Title.Text + "&";
            URL += "StartPostDate=" + StartPostDate.Text + "&";
            URL += "EndPostDate=" + EndPostDate.Text + "&";
            URL += "Status=" + Status.Text;
            ResponseHelper.Redirect(URL);
        }
    }
}