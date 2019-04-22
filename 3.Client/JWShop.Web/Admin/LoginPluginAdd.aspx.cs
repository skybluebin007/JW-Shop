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
    public partial class LoginPluginAdd :JWShop.Page.AdminBasePage
    {
        protected Dictionary<string, string> nameDic = new Dictionary<string, string>();
        protected Dictionary<string, string> valueDic = new Dictionary<string, string>();
        protected Dictionary<string, string> selectValueDic = new Dictionary<string, string>();
        protected LoginPluginsInfo loginPlugins = new LoginPluginsInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string key = RequestHelper.GetQueryString<string>("Key");
                if (key != string.Empty)
                {

                    loginPlugins = Common.LoginPlugins.ReadLoginPlugins(key);
                    Description.Text = loginPlugins.Description;
                    IsEnabled.Text = loginPlugins.IsEnabled.ToString();
                    Common.LoginPlugins.ReadCanChangeLoginPlugins(key, ref nameDic, ref valueDic, ref selectValueDic);
                }
            }
        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string key = RequestHelper.GetQueryString<string>("Key");
            string alertMessage = ShopLanguage.ReadLanguage("UpdateOK");

            HanlerCanChangLoginPlugins(key);
            AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdatePay"), "快捷登录方式");
            ScriptHelper.Alert(alertMessage, "LoginPlugins.aspx");
        }

        /// <summary>
        /// 处理可配置的文件
        /// </summary>
        protected void HanlerCanChangLoginPlugins(string key)
        {
            Dictionary<string, string> configDic = new Dictionary<string, string>();
            string nameList = RequestHelper.GetForm<string>("ConfigNameList");
            foreach (string name in nameList.Split('|'))
            {
                if (name != string.Empty)
                {
                    configDic.Add(name, RequestHelper.GetForm<string>(name));
                }
            }
            configDic.Add("Description", Description.Text);
            configDic.Add("IsEnabled", IsEnabled.Text);
            Common.LoginPlugins.UpdateLoginPlugins(key, configDic);
        }
    }
}