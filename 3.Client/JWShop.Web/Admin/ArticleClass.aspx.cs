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
    public partial class ArticleClass : JWShop.Page.AdminBasePage
	{
        /// <summary>
        /// 一级文章分类
        /// </summary>
        protected List<ArticleClassInfo> topClassList = new List<ArticleClassInfo>();
		/// <summary>
		/// 页面加载方法
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
            CheckAdminPower( "ReadArticleClass", PowerCheckType.Single);
			string action = RequestHelper.GetQueryString<string>("Action");
            int articleClassID = RequestHelper.GetQueryString<int>("ID");
			if (action != string.Empty && articleClassID !=int.MinValue)
			{
				switch (action)
				{
					case "Up":
                        CheckAdminPower( "UpdateArticleClass", PowerCheckType.Single);
                        ArticleClassBLL.Move(articleClassID, ChangeAction.Up);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("MoveRecord"), ShopLanguage.ReadLanguage("ArticleClass"), articleClassID);
						break;
					case "Down":
                        CheckAdminPower( "UpdateArticleClass", PowerCheckType.Single);
                        ArticleClassBLL.Move(articleClassID, ChangeAction.Down);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("MoveRecord"), ShopLanguage.ReadLanguage("ArticleClass"), articleClassID);
						break;
					case "Delete":
                        CheckAdminPower( "DeleteArticleClass", PowerCheckType.Single);
                        if (ArticleClassBLL.Read(articleClassID).IsSystem== 0)
                        {
                            ArticleClassBLL.Delete(articleClassID);
                            AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("ArticleClass"), articleClassID);
                        }
                        else
                        {
                            ScriptHelper.Alert(ShopLanguage.ReadLanguage("CannotDeleteSystemClass"));
                        }
						break;
					default:
						break;
				}				
			}
            topClassList = ArticleClassBLL.ReadRootList();
            //BindControl( ArticleClassBLL.ReadNamedList(), RecordList);   
		}
	}
}