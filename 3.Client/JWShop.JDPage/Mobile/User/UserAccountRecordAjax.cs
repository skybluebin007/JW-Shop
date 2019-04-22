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

namespace JWShop.Page.Mobile
{
    public class UserAccountRecordAjax : UserAjaxBasePage
    {
        /// <summary>
        /// 操作
        /// </summary>
        protected string action = string.Empty;
        /// <summary>
        /// 剩余资金
        /// </summary>
        protected decimal moneyLeft = 0;
        /// <summary>
        /// 剩余积分
        /// </summary>
        protected int pointLeft = 0;
        /// <summary>
        /// 账户类型
        /// </summary>
        protected AccountRecordType accountType = 0;
        /// <summary>
        /// 账户明细列表
        /// </summary>
        protected List<UserAccountRecordInfo> userAccountRecordList = new List<UserAccountRecordInfo>();
        /// <summary>
        /// Ajax分页
        /// </summary>
        protected AjaxPagerClass ajaxPagerClass = new AjaxPagerClass();
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            action = RequestHelper.GetQueryString<string>("Action");
            int currentPage = RequestHelper.GetQueryString<int>("Page");
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int pageSize = 15;
            int count = 0;
            accountType = AccountRecordType.Point;
            if (action == "Money")
            {
                accountType = AccountRecordType.Money;
            }
            userAccountRecordList = UserAccountRecordBLL.ReadList(currentPage, pageSize, accountType, base.UserId, ref count);
            if (userAccountRecordList.Count > 0)
            {
                if (accountType == AccountRecordType.Money)
                {
                    moneyLeft = UserAccountRecordBLL.ReadMoneyLeftBeforID(userAccountRecordList[0].Id, base.UserId);
                }
                else
                {
                    pointLeft = UserAccountRecordBLL.ReadPointLeftBeforID(userAccountRecordList[0].Id, base.UserId);
                }
            }
            ajaxPagerClass.CurrentPage = currentPage;
            ajaxPagerClass.PageSize = pageSize;
            ajaxPagerClass.Count = count;
            ajaxPagerClass.FirstPage = "<<首页";
            ajaxPagerClass.PreviewPage = "<<上一页";
            ajaxPagerClass.NextPage = "下一页>>";
            ajaxPagerClass.LastPage = "末页>>";
            ajaxPagerClass.ListType = false;
            ajaxPagerClass.DisCount = false;
            ajaxPagerClass.PrenextType = true;
        }
    }
}

