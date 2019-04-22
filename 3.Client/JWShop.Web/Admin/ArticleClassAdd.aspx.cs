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
    public partial class ArticleClassAdd : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FatherID.DataSource = ArticleClassBLL.ReadNamedList();
                FatherID.DataTextField = "Name";
                FatherID.DataValueField = "Id";
                FatherID.DataBind();
                FatherID.Items.Insert(0, new ListItem("作为最大类", "0"));
                int ArticleClassID = RequestHelper.GetQueryString<int>("ID");
                int fatherID = RequestHelper.GetQueryString<int>("FatherID");
                if (ArticleClassID != int.MinValue)
                {
                    CheckAdminPower("ReadArticleClass", PowerCheckType.Single);
                    ArticleClassInfo articleClass = ArticleClassBLL.Read(ArticleClassID);
                    FatherID.Text = articleClass.ParentId.ToString();
                    OrderID.Text = articleClass.OrderId.ToString();
                    ClassName.Text = articleClass.Name;
                    Description.Text = articleClass.Description;
                    AddCol2.InnerText = articleClass.AddCol2;
                    Photo.Text = articleClass.Photo;
                    EnClassName.Text = articleClass.EnName;
                    ShowType.Text = articleClass.ShowType.ToString();
                    ShowTerminal.Text = articleClass.ShowTerminal.ToString();
                    ImageWidth.Text = articleClass.ImageWidth.ToString();
                    ImageHeight.Text = articleClass.ImageHeight.ToString();
                }
                else
                {
                    FatherID.Text = fatherID.ToString();
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            ArticleClassInfo articleClass = new ArticleClassInfo();
            articleClass.Id = RequestHelper.GetQueryString<int>("ID");
            if (FatherID.Text.Trim() == RequestHelper.GetQueryString<string>("ID"))
            {
                ScriptHelper.Alert("不能将上级分类设置成自己", RequestHelper.RawUrl);
                Response.End();
            }
            articleClass.ParentId = Convert.ToInt32(FatherID.Text);
            articleClass.OrderId = Convert.ToInt32(OrderID.Text);
            articleClass.Name = ClassName.Text;
           
            articleClass.Description = Description.Text;

            articleClass.EnName = EnClassName.Text;
            articleClass.Photo = Photo.Text;
            articleClass.ShowType = Convert.ToInt32(ShowType.Text);
            articleClass.ShowTerminal = Convert.ToInt32(ShowTerminal.Text);
            articleClass.ImageWidth = Convert.ToInt32(ImageWidth.Text) < 0 ? 0 : Convert.ToInt32(ImageWidth.Text);
            articleClass.ImageHeight = Convert.ToInt32(ImageHeight.Text) < 0 ? 0 : Convert.ToInt32(ImageHeight.Text);

            articleClass.AddCol1 = 0;
            articleClass.AddCol2 = AddCol2.InnerText;

            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (articleClass.Id == int.MinValue)
            {
                articleClass.IsSystem = 0;
                CheckAdminPower("AddArticleClass", PowerCheckType.Single);
                int id = ArticleClassBLL.Add(articleClass);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("ArticleClass"), id);
            }
            else
            {//修改时保持系统分类不变
                articleClass.IsSystem = ArticleClassBLL.Read(RequestHelper.GetQueryString<int>("ID")).IsSystem;
                CheckAdminPower("UpdateArticleClass", PowerCheckType.Single);
                ArticleClassBLL.Update(articleClass);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("ArticleClass"), articleClass.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}