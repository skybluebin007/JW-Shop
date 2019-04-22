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
    public partial class PhotoSizeAdd : JWShop.Page.AdminBasePage
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
                int ID = RequestHelper.GetQueryString<int>("ID");
               
                if (ID != int.MinValue)
                {
                    CheckAdminPower("ReadPhotoSize", PowerCheckType.Single);
                    PhotoSizeInfo photoSize = PhotoSizeBLL.Read(ID);
                    Type.Text=photoSize.Type.ToString();
                    Title.Text = photoSize.Title;
                    Introduce.Text = photoSize.Introduce;
                    Width.Text = photoSize.Width.ToString();
                    Height.Text = photoSize.Height.ToString();
                }
            }
        }
        /// <summary>
        /// 提交按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            PhotoSizeInfo photoSize = new PhotoSizeInfo();
            photoSize.Id = RequestHelper.GetQueryString<int>("ID");
            int photoType = (int)PhotoType.Article;
            if (!int.TryParse(Type.SelectedValue, out photoType))
            {
                photoType = (int)PhotoType.Article;
            }
            photoSize.Type = photoType;
            photoSize.Title = Title.Text;
            photoSize.Introduce = Introduce.Text;
            photoSize.Width = Convert.ToInt32(Width.Text) < 0 ? 0 : Convert.ToInt32(Width.Text);
            photoSize.Height = Convert.ToInt32(Height.Text) < 0 ? 0 : Convert.ToInt32(Height.Text);
         
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (photoSize.Id == int.MinValue)
            {
                CheckAdminPower("AddPhotoSize", PowerCheckType.Single);
                int id = PhotoSizeBLL.Add(photoSize);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("PhotoSize"), id);
            }
            else
            {
                CheckAdminPower("UpdatePhotoSize", PowerCheckType.Single);
                PhotoSizeBLL.Update(photoSize);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("PhotoSize"), photoSize.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, "photosize.aspx");
        }
    }
}