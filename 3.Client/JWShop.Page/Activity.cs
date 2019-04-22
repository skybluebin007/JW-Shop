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
    public class Activity : CommonBasePage
    {
        private string footaddress = string.Empty;
        /// <summary>
        /// 标题
        /// </summary>
        public string FootAddress
        {
            set { this.footaddress = value; }
            get
            {
                string temp = ShopConfig.ReadConfigInfo().FootAddress;
                if (this.footaddress != string.Empty)
                {
                    temp = this.footaddress + " - " + ShopConfig.ReadConfigInfo().FootAddress;
                }
                return temp;
            }
        }
        /// <summary>
        /// 今日优惠
        /// </summary>
        protected FavorableActivityInfo favorableActivity = new FavorableActivityInfo();
        /// <summary>
        /// 优惠活动列表
        /// </summary>
        protected List<FavorableActivityInfo> favorableActivityList = new List<FavorableActivityInfo>();
        /// <summary>
        /// 活动插件列表
        /// </summary>
        protected List<ActivityPluginsInfo> activityPluginsList = new List<ActivityPluginsInfo>();
        /// <summary>
        /// 剩余时间
        /// </summary>
        protected long leftTime = 0;
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            favorableActivity = FavorableActivityBLL.ReadFavorableActivity(RequestHelper.DateNow, RequestHelper.DateNow, 0);
            if (favorableActivity.ID > 0)
            {
                TimeSpan timeSpan = favorableActivity.EndDate - RequestHelper.DateNow;
                leftTime = timeSpan.Days * 24 * 3600 + timeSpan.Hours * 3600 + timeSpan.Minutes * 60 + timeSpan.Seconds;
            }
            else
            {
                int count = int.MinValue;
                favorableActivityList = FavorableActivityBLL.ReadFavorableActivityList(1, 5, ref count);
            }
            activityPluginsList = SocoShop.Common.ActivityPlugins.ReadIsEnabledActivityPluginsList();
            Title = "商家活动";
        }
    }
}
