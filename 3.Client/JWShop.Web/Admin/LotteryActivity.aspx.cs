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
    public partial class LotteryActivity : JWShop.Page.AdminBasePage
    {
        /// <summary>
        /// 活动类型，默认1：大转盘
        /// </summary>
        protected int type = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //CheckAdminPower("ReadFlash", PowerCheckType.Single);
                string action = RequestHelper.GetQueryString<string>("Action");
                type = RequestHelper.GetQueryString<int>("type")<=0?1:RequestHelper.GetQueryString<int>("type");
                if (action == "Delete")
                {
                    CheckAdminPower("DeleteFlash", PowerCheckType.Single);
                    int id = RequestHelper.GetQueryString<int>("Id");
                    if (id > 0)
                    {
                        FlashBLL.Delete(id);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Flash"), id);
                        foreach (var adImage in AdImageBLL.ReadList(id))
                        {
                            AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("FlashPhoto"), adImage.Id);
                        }
                        AdImageBLL.DeleteByAdType(id);
                        //ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
                    }
                }
                LotteryActivitySearchInfo activitySearch = new LotteryActivitySearchInfo();
                activitySearch.ActivityType = (int)LotteryActivityType.Wheel;
                BindControl(LotteryActivityBLL.SearchList(CurrentPage, PageSize,activitySearch, ref Count), RecordList, MyPager);
            }        
        }

        protected string GetUrl(object activityId)
        {
            return string.Concat(new object[] { "http://", HttpContext.Current.Request.Url.Host, (HttpContext.Current.Request.Url.Port > 0 ? ":" + HttpContext.Current.Request.Url.Port : ""), "/mobile/bigwheel.html?id=", activityId });


        }
    }
}