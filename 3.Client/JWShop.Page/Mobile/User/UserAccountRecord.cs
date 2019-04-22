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

namespace JWShop.Page.Mobile
{
    public class UserAccountRecord : UserBasePage
    {
        protected decimal left = 0;
        protected bool isViewPoint;
        //protected WebService.Entity.Account account = new WebService.Entity.Account();
        protected List<UserAccountRecordInfo> userAccountRecordList = new List<UserAccountRecordInfo>();
        protected CommonPagerClass pager = new CommonPagerClass();

        protected override void PageLoad()
        {
            base.PageLoad();

            string displayType = RequestHelper.GetQueryString<string>("v");
            if (displayType != "p" && displayType != "m") ResponseHelper.End();
            isViewPoint = displayType == "p";

            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 20;
            int count = 0;
            var recordType = isViewPoint ? AccountRecordType.Point : AccountRecordType.Money;
            userAccountRecordList = UserAccountRecordBLL.ReadList(currentPage, pageSize, recordType, base.UserId, ref count);

            //
            if (isViewPoint)
            {
                left = UserAccountRecordBLL.SumPoint(base.UserId);
            }
            else
            {
                if (!string.IsNullOrEmpty(CurrentUser.CardNo) && !string.IsNullOrEmpty(CurrentUser.CardPwd))
                {
                    bool isSuccess; string msg;
                    //account = WebService.Account.GetAccount(CurrentUser.CardNo, CurrentUser.CardPwd, out isSuccess, out msg);
                    //left = account.Acc;
                }
            }

            pager.Init(currentPage, pageSize, count, !string.IsNullOrEmpty(isMobile));

            Title = isViewPoint ? "我的积分" : "我的余额";
        }
    }
}