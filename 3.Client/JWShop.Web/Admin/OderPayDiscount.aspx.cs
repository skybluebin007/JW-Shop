using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Web.Admin
{
    public partial class OderPayDiscount : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadConfig", PowerCheckType.Single);
                //满立减
                OrderMoney.Text = ShopConfig.ReadConfigInfo().OrderMoney.ToString();
                OrderDisCount.Text = ShopConfig.ReadConfigInfo().OrderDisCount.ToString();
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
            //满立减
            decimal orderMoney = 0; decimal orderDiscount = 0;
            config.PayDiscount = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$PayDiscount");
            if (config.PayDiscount == 1)
            {
                if (!decimal.TryParse(OrderMoney.Text, out orderMoney) || !decimal.TryParse(OrderDisCount.Text, out orderDiscount))
                {
                    ScriptHelper.Alert("满立减金额填写错误");
                }
                if (orderMoney <= orderDiscount)
                {
                    ScriptHelper.Alert("满立减金额必须小于订单金额");
                }
            }
            config.OrderMoney = orderMoney;
            config.OrderDisCount = orderDiscount;
            ShopConfig.UpdateConfigInfo(config);
            AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateConfig"));
            ScriptHelper.Alert(ShopLanguage.ReadLanguage("UpdateOK"), RequestHelper.RawUrl);
        }
    }
}