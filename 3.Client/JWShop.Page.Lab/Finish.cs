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
using System.Linq;

namespace JWShop.Page.Lab
{
    public class Finish : CommonBasePage
    {
        protected decimal totalMoney = 0;
        protected decimal balanceMoney = 0;
        protected decimal payMoney = 0;
        protected PayPluginsInfo payPlugins = new PayPluginsInfo();

        protected override void PageLoad()
        {
            base.PageLoad();
            string id = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("id"));

            if (base.UserId <= 0)
            {
                ResponseHelper.Redirect("/user/login.html?RedirectUrl=/finish.html?id=" + id);
                ResponseHelper.End();
            }

            int[] ids = new int[] { };
            try
            {
                ids = Array.ConvertAll<string, int>(id.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
            }
            catch { }

            var orders = OrderBLL.ReadList(ids, base.UserId);
            if (orders.Count > 0)
            {
                totalMoney = orders.Sum(k => k.ProductMoney + k.ShippingMoney);
                balanceMoney = orders.Sum(k => k.Balance);
                payMoney = orders.Sum(k => k.ProductMoney + k.ShippingMoney - k.Balance);

                payPlugins = PayPlugins.ReadPayPlugins(orders[0].PayKey);
            }

            Title = "订单完成";
        }
    }
}