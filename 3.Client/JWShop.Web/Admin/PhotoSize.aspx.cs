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
    public partial class PhotoSize : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadPhotoSize", PowerCheckType.Single);
                string action = RequestHelper.GetQueryString<string>("Action");
                int type = RequestHelper.GetQueryString<int>("photoType");
                Type.Text = type.ToString();
                if (action == "Delete")
                {
                    CheckAdminPower("DeletePhotoSize", PowerCheckType.Single);
                    int id = RequestHelper.GetQueryString<int>("Id");
                    if (id > 0)
                    {
                        PhotoSizeBLL.Delete(id);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("PhotoSize"), id);
                      
                        //ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
                    }
                }

                BindControl(PhotoSizeBLL.SearchList(type, CurrentPage, PageSize, ref Count), RecordList, MyPager);
            }
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "PhotoSize.aspx?Action=search&";
            URL += "photoType=" + Type.Text ;
           
            ResponseHelper.Redirect(URL);
        }
    }
}