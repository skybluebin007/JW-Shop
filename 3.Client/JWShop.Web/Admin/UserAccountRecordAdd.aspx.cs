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

namespace SocoShop.Web.Admin
{
    public partial class UserAccountRecordAdd : JWShop.Page.AdminBasePage
    {
        protected UserInfo user = new UserInfo();
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            user=UserBLL.ReadUserMore( RequestHelper.GetQueryString<int>("UserID"));
        }
        /// <summary>
        /// 提交按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            UserAccountRecordInfo uarInfo = new UserAccountRecordInfo();
            uarInfo.UserId = RequestHelper.GetQueryString<int>("UserID");
            //uarInfo.Money = Convert.ToDecimal(Money.Text);
            uarInfo.Point = Convert.ToInt32(Point.Text);
            uarInfo.Note = Note.Text;
            uarInfo.RecordType = (int)AccountRecordType.Point;//积分类型
            uarInfo.IP = ClientHelper.IP;
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            CheckAdminPower( "AddUserAccountRecord", PowerCheckType.Single);
            int id = UserAccountRecordBLL.Add(uarInfo);
            AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("UserAccountRecord"), id);
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}