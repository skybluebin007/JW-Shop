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
    public partial class PayAdd : JWShop.Page.AdminBasePage
    {
        protected Dictionary<string, string> nameDic = new Dictionary<string, string>();
        protected Dictionary<string, string> valueDic = new Dictionary<string, string>();
        protected Dictionary<string, string> selectValueDic = new Dictionary<string, string>();
        protected PayPluginsInfo payPlugins = new PayPluginsInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string key = RequestHelper.GetQueryString<string>("Key");
                if (key != string.Empty)
                {
                    CheckAdminPower("ReadPay", PowerCheckType.Single);
                    payPlugins = PayPlugins.ReadPayPlugins(key);
                    Description.Text = payPlugins.Description;
                    IsEnabled.Text = payPlugins.IsEnabled.ToString();
                    PayPlugins.ReadCanChangePayPlugins(key, ref nameDic, ref valueDic, ref selectValueDic);
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string key = RequestHelper.GetQueryString<string>("Key");
            string alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            CheckAdminPower("UpdatePay", PowerCheckType.Single);
            HanlerCanChangPayPlugins(key);
            AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdatePay"), "支付方式");
            ScriptHelper.Alert(alertMessage, "Pay.aspx");
        }

        /// <summary>
        /// 处理可配置的文件
        /// </summary>
        protected void HanlerCanChangPayPlugins(string key)
        {
            Dictionary<string, string> configDic = new Dictionary<string, string>();
            string nameList = RequestHelper.GetForm<string>("ConfigNameList");
            foreach (string name in nameList.Split('|'))
            {
                if (name != string.Empty)
                {
                    configDic.Add(name, RequestHelper.GetForm<string>(name).Trim());
                }
            }
            configDic.Add("Description", Description.Text.Trim());
            configDic.Add("IsEnabled", IsEnabled.Text.Trim());
            PayPlugins.UpdatePayPlugins(key, configDic);
        }
    }
}