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
    public class UserRechargeAjax : UserAjaxBasePage
    {
        protected string action;
        protected List<UserRechargeInfo> userRechargeList = new List<UserRechargeInfo>();
        protected List<PayPluginsInfo> payPluginsList = new List<PayPluginsInfo>();
        protected decimal moneyLeft = 0;

        protected override void PageLoad()
        {
            base.PageLoad();
            action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "Read":
                    userRechargeList = UserRechargeBLL.ReadList(base.UserId);
                    break;
                case "Add":
                    payPluginsList = PayPlugins.ReadRechargePayPluginsList();
                    //webservice
                    //var account = WebService.Account.GetAccount();
                    //moneyLeft = account.Zacc;
                    break;
                case "AddUserRecharge":
                    AddUserRecharge();
                    break;
                default:
                    break;
            }
        }

        protected void AddUserRecharge()
        {
            decimal money = RequestHelper.GetForm<decimal>("money");
            string payKey = StringHelper.AddSafe(RequestHelper.GetForm<string>("pay"));

            if (money <= 0)
            {
                ResponseHelper.Write("error|请填写金额");
                ResponseHelper.End();
            }
            if (string.IsNullOrEmpty(payKey))
            {
                ResponseHelper.Write("error|请选择支付方式");
                ResponseHelper.End();
            }

            UserRechargeInfo userRecharge = new UserRechargeInfo();
            Random rd = new Random();
            userRecharge.Number = RequestHelper.DateNow.ToString("yyMMddhh") + rd.Next(1000, 9999);
            userRecharge.Money = money;
            userRecharge.PayKey = payKey;
            userRecharge.PayName = PayPlugins.ReadPayPlugins(payKey).Name;
            userRecharge.RechargeDate = RequestHelper.DateNow;
            userRecharge.RechargeIP = ClientHelper.IP;
            userRecharge.IsFinish = (int)BoolType.False;
            userRecharge.UserId = base.UserId;
            int id = UserRechargeBLL.Add(userRecharge);

            ResponseHelper.Write("ok|" + id);
            ResponseHelper.End();
        }
    }
}