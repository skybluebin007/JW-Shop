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
    public partial class FlashAdd : JWShop.Page.AdminBasePage
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
                int flashID = RequestHelper.GetQueryString<int>("ID");
                if (flashID != int.MinValue)
                {
                    CheckAdminPower("ReadFlash", PowerCheckType.Single);
                    FlashInfo flash = FlashBLL.Read(flashID);
                    Title.Text = flash.Title;
                    Introduce.Text = flash.Introduce;
                    Width.Text = flash.Width.ToString();
                    Height.Text = flash.Height.ToString();
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
            FlashInfo flash = new FlashInfo();
            flash.Id = RequestHelper.GetQueryString<int>("ID");
            flash.Title = Title.Text;
            flash.Introduce = Introduce.Text;
            flash.Width = Convert.ToInt32(Width.Text) < 0 ?0 : Convert.ToInt32(Width.Text);
            flash.Height = Convert.ToInt32(Height.Text) < 0 ? 0 : Convert.ToInt32(Height.Text);
            flash.EndDate = DateTime.Now.AddYears(1);
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (flash.Id == int.MinValue)
            {
                CheckAdminPower("AddFlash", PowerCheckType.Single);
                int id = FlashBLL.Add(flash);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Flash"), id);
            }
            else
            {
                CheckAdminPower("UpdateFlash", PowerCheckType.Single);
                FlashBLL.Update(flash);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Flash"), flash.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
                    ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}