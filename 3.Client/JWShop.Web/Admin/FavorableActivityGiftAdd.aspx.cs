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
    public partial class FavorableActivityGiftAdd : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = RequestHelper.GetQueryString<int>("ID");
                if (id != int.MinValue)
                {
                    CheckAdminPower("ReadGift", PowerCheckType.Single);
                    var gift = FavorableActivityGiftBLL.Read(id);
                    Name.Text = gift.Name;
                    Photo.Text = gift.Photo;
                    Description.Text = gift.Description;
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            var gift = new FavorableActivityGiftInfo();
            gift.Id = RequestHelper.GetQueryString<int>("ID");
            gift.Name = Name.Text;
            gift.Photo = Photo.Text;
            gift.Description = Description.Text;
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (gift.Id == int.MinValue)
            {
                CheckAdminPower("AddGift", PowerCheckType.Single);
                int id = FavorableActivityGiftBLL.Add(gift);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Gift"), id);
            }
            else
            {
                CheckAdminPower("UpdateGift", PowerCheckType.Single);
                FavorableActivityGiftBLL.Update(gift);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Gift"), gift.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}