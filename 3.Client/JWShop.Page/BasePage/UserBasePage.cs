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
        protected string userGradeName = "";
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
            userGradeName = UserGradeBLL.Read(base.GradeID).Name;
        }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        protected UserInfo usr = new UserInfo();
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
