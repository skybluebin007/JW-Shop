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

namespace JWShop.Page
{
    public abstract class UserBasePage : CommonBasePage
    {
        //安全级别
        protected int safetyGrade = 0;
        //安全等级名称展示
        protected string safetyGradeName = "危险";
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            topNav = 12;

            if (RequestHelper.RawUrl.ToLower().IndexOf("/mobile/") >= 0)
            {
                base.CheckUserLogin(1);
            }
            else
            {
                base.CheckUserLogin(0);
            }
            //安全级别
            if (!string.IsNullOrEmpty(CurrentUser.UserPassword)) safetyGrade += 1;
            if (!string.IsNullOrEmpty(CurrentUser.Email)) safetyGrade += 1;
            if (!string.IsNullOrEmpty(CurrentUser.Mobile)) safetyGrade += 1;
            switch (safetyGrade)
            {
                case 0:
                    safetyGradeName = "危险";
                    break;
                case 1:
                    safetyGradeName = "较低";
                    break;
                case 2:
                    safetyGradeName = "中等";
                    break;
                case 3:
                    safetyGradeName = "较高";
                    break;
            }
        }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        protected UserInfo usr = new UserInfo();
        protected UserInfo CurrentUser
        {
            get
            {
                if (usr.Id < 1) usr = UserBLL.ReadUserMore(base.UserId);
                return usr;
            }
            set { usr = value; }
        }

    }
}
