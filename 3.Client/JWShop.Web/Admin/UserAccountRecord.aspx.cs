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

namespace JWShop.Web.Admin
{
    public partial class UserAccountRecord : JWShop.Page.AdminBasePage
    {
       
        protected int userID = 0;
        protected UserInfo user = new UserInfo();
        protected int accountType = 0;
        protected decimal moneyLeft = 0;
        protected int pointLeft = 0;
        protected List<UserAccountRecordInfo> userAccountRecordList = new List<UserAccountRecordInfo>();
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower( "ReadUserAccountRecord", PowerCheckType.Single);
                userID = RequestHelper.GetQueryString<int>("UserID");
                user = UserBLL.ReadUserMore(userID);
                accountType = RequestHelper.GetQueryString<int>("AccountType");               
                PageSize = 12;
                //if (accountType == int.MinValue)
                //{
                //    userAccountRecordList = UserAccountRecordBLL.ReadList(CurrentPage, PageSize, AccountRecordType.Money, userID, ref Count);
                //}
                //else
                //{
                    userAccountRecordList = UserAccountRecordBLL.ReadList(CurrentPage, PageSize, AccountRecordType.Point, userID, ref Count);
                //}
               
                if (userAccountRecordList.Count > 0)
                {
                    //if (accountType == (int)AccountRecordType.Money)
                    //{
                    //    moneyLeft = UserAccountRecordBLL.ReadMoneyLeftBeforID(userAccountRecordList[0].Id, userID);
                    //}
                    //else
                    //{
                        pointLeft = UserAccountRecordBLL.ReadPointLeftBeforID(userAccountRecordList[0].Id, userID);
                    //}
                }
                //BindControl(MyPager);
                BindControl(userAccountRecordList, RecordList, MyPager);
            }
        }
        /// <summary>
        /// 根据现有积分计算剩余积分
        /// </summary>
        /// <param name="point">现有积分</param>
        /// <returns></returns>
        protected object ShowPointLeft(object id,object point)
        {
            int tmp = 0;
            int.TryParse(id.ToString(), out tmp);
            int tmpPoint=0;
            int.TryParse(point.ToString(),out tmpPoint);
            return UserAccountRecordBLL.ReadPointLeftBeforID(tmp, userID) + tmpPoint;
        }
    }
}