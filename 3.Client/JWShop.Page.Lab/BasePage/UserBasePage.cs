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

namespace JWShop.Page.Lab
{
    public abstract class UserBasePage : CommonBasePage
    {
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            base.CheckUserLogin();
        }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        private UserInfo usr = new UserInfo();
        protected UserInfo CurrentUser
        {
            get
            {
                if (usr.Id < 1) usr = UserBLL.Read(base.UserId);
                return usr;
            }
            set { usr = value; }
        }

    }
}
