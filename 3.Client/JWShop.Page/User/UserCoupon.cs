using System;
using System.IO;
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
    public class UserCoupon : UserBasePage
    {
        /// <summary>
        /// 用户等级
        /// </summary>
        protected string userGradeName = string.Empty;
        /// <summary>
        /// 用户信息
        /// </summary>
        protected UserInfo user = new UserInfo();
        /// <summary>
        /// 页面加载
        /// </summary>
         protected override void PageLoad()
        {
            base.PageLoad();
            user = UserBLL.ReadUserMore(base.UserId);
            userGradeName = UserGradeBLL.Read(base.GradeID).Name;
        }         
    }
}
