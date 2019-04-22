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
    public partial class WexinConfig : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadConfig", PowerCheckType.Single);
              
                #region 微信等登录参数
                AppID.Text = ShopConfig.ReadConfigInfo().AppID;
                AppSecret.Text = ShopConfig.ReadConfigInfo().Appsecret;
                Token.Text = ShopConfig.ReadConfigInfo().Token;
                EncodingAESKey.Text = ShopConfig.ReadConfigInfo().EncodingAESKey;
                WechatLoginURL.Text = ShopConfig.ReadConfigInfo().WechatLoginURL;
                AttentionTitle.Text = ShopConfig.ReadConfigInfo().AttentionTitle;
                AttentionSummary.Text = ShopConfig.ReadConfigInfo().AttentionSummary;
                AttentionPicture.Text = ShopConfig.ReadConfigInfo().AttentionPicture;
                DefaultReply.Text = ShopConfig.ReadConfigInfo().DefaultReply;
                #endregion
            }
        }
        /// <summary>
        /// 提交按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("UpdateConfig", PowerCheckType.Single);
            ShopConfigInfo config = ShopConfig.ReadConfigInfo();
           
            #region 微信登录参数
            config.AppID = AppID.Text.Trim();
            config.Appsecret = AppSecret.Text.Trim();
            config.Token = Token.Text.Trim();
            config.EncodingAESKey = EncodingAESKey.Text.Trim();
            config.WechatLoginURL = WechatLoginURL.Text.Trim();
            config.AttentionTitle = AttentionTitle.Text;
            config.AttentionSummary = AttentionSummary.Text;
            config.AttentionPicture = AttentionPicture.Text;
            config.DefaultReply = DefaultReply.Text;
            #endregion
            ShopConfig.UpdateConfigInfo(config);
            AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateConfig"));
            ScriptHelper.Alert(ShopLanguage.ReadLanguage("UpdateOK"), RequestHelper.RawUrl);
        }
    }
}