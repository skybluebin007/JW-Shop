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
    public partial class Article : JWShop.Page.AdminBasePage
    {
        protected string classID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadArticle", PowerCheckType.Single);
                foreach (ArticleClassInfo articleClass in ArticleClassBLL.ReadNamedList())
                {
                    ArticleClassID.Items.Add(new ListItem(articleClass.Name, "|" + articleClass.Id + "|"));
                }
                ArticleClassID.Items.Insert(0, new ListItem("所有分类", string.Empty));
                Title.Text = RequestHelper.GetQueryString<string>("Title");
                ArticleClassID.Text = RequestHelper.GetQueryString<string>("ClassID");
                IsTop.Text = RequestHelper.GetQueryString<string>("IsTop");
                //页签的判断
                classID = RequestHelper.GetQueryString<string>("ClassID");
                if (classID != string.Empty)
                {
                    classID = ArticleClassBLL.ReadFullParentId(Convert.ToInt32(classID.Replace("|", string.Empty)));
                    classID = classID.Substring(1, classID.Length - 2);
                    if (classID.IndexOf('|') > -1)
                    {
                        classID = classID.Substring(0, classID.IndexOf('|'));
                    }
                }

                ArticleSearchInfo articleSearch = new ArticleSearchInfo();
                articleSearch.Title = RequestHelper.GetQueryString<string>("Title");
                articleSearch.ClassId = RequestHelper.GetQueryString<string>("ClassID");
                articleSearch.IsTop = RequestHelper.GetQueryString<int>("IsTop");

                BindControl(ArticleBLL.SearchList(CurrentPage, PageSize, articleSearch, ref Count), RecordList, MyPager);
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteArticle", PowerCheckType.Single);
            string deleteID = RequestHelper.GetIntsForm("SelectID");
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length > 0)
            {
                ArticleBLL.Delete(Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)));
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Article"), deleteID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "Article.aspx?Action=search&";
            URL += "Title=" + Title.Text + "&";
            URL += "ClassID=" + ArticleClassID.Text + "&";
            URL += "IsTop=" + IsTop.Text;
            ResponseHelper.Redirect(URL);
        }
    }
}