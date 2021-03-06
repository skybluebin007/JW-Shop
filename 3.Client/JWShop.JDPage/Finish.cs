﻿using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Page
{
    public class Finish : CommonBasePage
    {        
        protected PayPluginsInfo payPlugins = new PayPluginsInfo();

        /// <summary>
        /// 订单
        /// </summary>
        protected OrderInfo order = new OrderInfo();

        protected override void PageLoad()
        {
            base.PageLoad();
            int id = RequestHelper.GetQueryString<int>("id");
            istop = 1;

            if (base.UserId <= 0)
            {
                ResponseHelper.Redirect("/user/login.html?RedirectUrl=/finish.html?id=" + id);
                ResponseHelper.End();
            }

            order = OrderBLL.Read(id, base.UserId);
            payPlugins = PayPlugins.ReadPayPlugins(order.PayKey);

            Title = "订单完成";
        }
    }
}