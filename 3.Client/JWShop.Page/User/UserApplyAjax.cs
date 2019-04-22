using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SocoShop.Common;
using SocoShop.Business;
using SocoShop.Entity;
using SkyCES.EntLib;

namespace SocoShop.Page
{
    public class UserApplyAjax : UserAjaxBasePage
    {
        /// <summary>
        /// 操作
        /// </summary>
        protected string action = string.Empty;
        /// <summary>
        /// 用户申请列表
        /// </summary>
        protected List<UserApplyInfo> userApplyList = new List<UserApplyInfo>();
        /// <summary>
        /// Ajax分页
        /// </summary>
        protected AjaxPagerClass ajaxPagerClass = new AjaxPagerClass();
        /// <summary>
        /// 剩余金额
        /// </summary>
        protected decimal moneyLeft = 0;
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
            int pageSize = 20;
            int count = 0;
            switch (action)
            {
                case "Read":
                    UserApplySearchInfo userApplySearch = new UserApplySearchInfo();
                    userApplySearch.UserID = base.UserID;
                    userApplyList = UserApplyBLL.SearchUserApplyList(currentPage, pageSize, userApplySearch, ref count); ajaxPagerClass.CurrentPage = currentPage;
                    ajaxPagerClass.PageSize = pageSize;
                    ajaxPagerClass.Count = count;
                    ajaxPagerClass.FirstPage = "<<首页";
                    ajaxPagerClass.PreviewPage = "<<上一页";
                    ajaxPagerClass.NextPage = "下一页>>";
                    ajaxPagerClass.LastPage = "末页>>";
                    ajaxPagerClass.ListType = false;
                    ajaxPagerClass.DisCount = false;
                    ajaxPagerClass.PrenextType = true;
                    break;
                case "Add":
                    moneyLeft = UserBLL.ReadUserMore(base.UserID).MoneyLeft;
                    break;
                case "AddUserApply":
                    AddUserApply();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 添加提现申请
        /// </summary>
        protected void AddUserApply()
        {
            string result = string.Empty;
            decimal money = RequestHelper.GetQueryString<decimal>("Money");
            string userNote = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("UserNote"));
            if (money <= 0 || userNote == string.Empty)
            {
                result = "请填写金额和备注";
            }
            else
            {
                UserInfo user = UserBLL.ReadUserMore(base.UserID);
                if (user.MoneyLeft < money)
                {
                    result = "提现金额大于剩余金额";
                }
                else
                {
                    UserApplyInfo userApply = new UserApplyInfo();
                    Random rd = new Random();
                    userApply.Number = RequestHelper.DateNow.ToString("yyMMddhh") + rd.Next(1000, 9999);
                    userApply.Money = money;
                    userApply.UserNote = userNote;
                    userApply.Status = (int)ApplyStatus.Indeterminate;
                    userApply.ApplyDate = RequestHelper.DateNow;
                    userApply.ApplyIP = ClientHelper.IP;
                    userApply.AdminNote = string.Empty;
                    userApply.UpdateDate = RequestHelper.DateNow;
                    userApply.UpdateAdminID = 0;
                    userApply.UpdateAdminName = string.Empty;
                    userApply.UserID = base.UserID;
                    userApply.UserName = base.UserName;
                    UserApplyBLL.AddUserApply(userApply);
                }
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
    }
}

