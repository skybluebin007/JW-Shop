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


namespace JWShop.Web.Admin
{
    public partial class ArticleAdd : JWShop.Page.AdminBasePage
    {
        protected List<ProductPhotoInfo> productPhotoList = new List<ProductPhotoInfo>();

        protected ArticleClassInfo thisClass = new ArticleClassInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 获取当前分类
            if (RequestHelper.GetQueryString<string>("Action") == "GetThisClass") GetthisClass();
            #endregion

            
            if (!Page.IsPostBack)
            {
                foreach (ArticleClassInfo articleClass in ArticleClassBLL.ReadNamedList())
                {
                    ClassID.Items.Add(new ListItem(articleClass.Name, articleClass.Id.ToString()));
                }
                RealDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                int articleID = RequestHelper.GetQueryString<int>("ID");
                if (articleID != int.MinValue)
                {
                    productPhotoList = ProductPhotoBLL.ReadList(articleID, 1);
                    CheckAdminPower("ReadArticle", PowerCheckType.Single);
                    ArticleInfo article = ArticleBLL.Read(articleID);
                    Title.Text = article.Title;
                    string classID = article.ClassId;
                    if (classID != string.Empty)
                    {
                        classID = classID.Substring(1, classID.Length - 2);
                        if (classID.IndexOf('|') > -1)
                        {
                            classID = classID.Substring(classID.LastIndexOf('|') + 1);
                        }
                    }
                    ClassID.Text = classID;

                    int thisClassID = 0;
                    int.TryParse(classID, out thisClassID);
                    thisClass = ArticleClassBLL.Read(thisClassID);

                    IsTop.Text = article.IsTop.ToString();
                    Author.Text = article.Author;
                    Resource.Text = article.Resource;
                    Keywords.Text = article.Keywords;
                    Url.Text = article.Url;
                    Photo.Text = article.Photo;
                    Summary.Text = article.Summary;
                    Content.Value = article.Content;
                    MobileContent.Value = article.AddCol2;
                    RealDate.Text = article.RealDate.ToString();
                    OrderID.Text = article.OrderId.ToString();
                    FilePath.Text = article.FilePath;
                    Content1.Value = article.Content1;
                    MobileContent1.Value = article.Mobilecontent1;
                    Content2.Value = article.Content2;
                    MobileContent2.Value = article.Mobilecontent2;
                    //Content3.Value = article.AddCol3;
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            ArticleInfo article = new ArticleInfo();
            article.Id = RequestHelper.GetQueryString<int>("ID");
            article.Title = Title.Text;
            article.ClassId = ArticleClassBLL.ReadFullParentId(Convert.ToInt32(ClassID.Text));
            article.IsTop = Convert.ToInt32(IsTop.Text);
            article.Author = Author.Text;
            article.Resource = Resource.Text;
            article.Keywords = Keywords.Text;
            article.Url = Url.Text;
            article.Photo = Photo.Text;
            article.Summary = Summary.Text;
            article.Content = Content.Value.FilterBadwords();
            article.Date = RequestHelper.DateNow;

            #region 新增
            article.Content1 = Content1.Value.FilterBadwords();
            article.Mobilecontent1 = MobileContent1.Value.FilterBadwords();
            article.Content2 = Content2.Value.FilterBadwords();
            article.Mobilecontent2 = MobileContent2.Value.FilterBadwords();

            #endregion

            article.OrderId = Convert.ToInt32(OrderID.Text);
            article.ViewCount = 0;
            article.LoveCount = 0;
            article.RealDate = Convert.ToDateTime(RealDate.Text);
            article.FilePath = FilePath.Text;
            article.ParentId = 0;
            article.AddCol1 = 0;
            article.AddCol2 = MobileContent.Value.FilterBadwords();

            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (article.Id == int.MinValue)
            {
                CheckAdminPower("AddArticle", PowerCheckType.Single);
                int id = ArticleBLL.Add(article);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Article"), id);
            }
            else
            {
                CheckAdminPower("UpdateArticle", PowerCheckType.Single);
                ArticleBLL.Update(article);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Article"), article.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }

        /// <summary>
        /// 获取当前分类
        /// </summary>
        protected void GetthisClass() {
            bool flag=false;
            int classID = RequestHelper.GetQueryString<int>("classId");
            if (classID > 0)
            {
                thisClass = ArticleClassBLL.Read(classID);
                if(thisClass.ImageWidth>0){
                    flag=true;
                    Response.Clear();
                    Response.Write("ok|建议上传图片" + thisClass.ImageWidth + "×" + thisClass.ImageHeight + "为最佳视觉效果");
                    Response.End();
                }
             }
            if(!flag){
                Response.Clear();
                Response.Write("error|参数错误");
                Response.End();
            }
        }
    }
}