using System;
using System.Data;
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
    public class Index : UserBasePage
    {
        protected decimal money, point;
        protected List<OrderInfo> orderList = new List<OrderInfo>();
        protected DataTable dt = new DataTable();

        protected override void PageLoad()
        {
            base.PageLoad();

            if (CurrentUser.UserType == (int)UserType.Provider)
            {
                dt = UserBLL.ShopIndexStatistics(base.UserId);
                OrderSearchInfo orderSearch = new OrderSearchInfo();
                orderSearch.ShopId = base.UserId;
                orderSearch.IsDelete = (int)BoolType.False;
                int count = int.MinValue;
                orderList = OrderBLL.SearchList(1, 10, orderSearch, ref count);
            }
            else
            {
                OrderSearchInfo orderSearch = new OrderSearchInfo();
                orderSearch.UserId = base.UserId;
                orderSearch.IsDelete = (int)BoolType.False;
                int count = 0;
                orderList = OrderBLL.SearchList(1, 10, orderSearch, ref count);
                dt = UserBLL.UserIndexStatistics(base.UserId);

                //webservice
                //get account
                if (CurrentUser.IsTulouMember)
                {
                    bool isSuccess; string msg;
                    //var account = WebService.Account.GetAccount(CurrentUser.CardNo, CurrentUser.CardPwd, out isSuccess, out msg);
                    //money = account.Acc;
                }
                point = UserAccountRecordBLL.SumPoint(base.UserId);
            }
        }
    }
}