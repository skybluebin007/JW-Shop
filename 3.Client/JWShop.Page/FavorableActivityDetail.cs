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
    public class FavorableActivityDetail:CommonBasePage
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
        /// 优惠活动
        /// </summary>
        protected FavorableActivityInfo favorableActivity = new FavorableActivityInfo();
        /// <summary>
        /// 用户等级
        /// </summary>
        protected string userGrade = string.Empty;
        /// <summary>
        /// 购买类型
        /// </summary>
        protected string buyClasss = string.Empty;
        /// <summary>
        /// 礼品列表
        /// </summary>
        protected List<GiftInfo> giftList = new List<GiftInfo>();
        /// <summary>
        /// 页面加载
        /// </summary>
         protected override void PageLoad()
        {
            base.PageLoad();
            int id = RequestHelper.GetQueryString<int>("ID");
            favorableActivity = FavorableActivityBLL.ReadFavorableActivity(id);
            if (favorableActivity.UserGrade != string.Empty)
            {
                foreach (string temp in favorableActivity.UserGrade.Split(','))
                {
                    if (userGrade == string.Empty)
                    {
                        userGrade = UserGradeBLL.ReadUserGradeCache(Convert.ToInt32(temp)).Name;
                    }
                    else
                    {
                        userGrade += "," + UserGradeBLL.ReadUserGradeCache(Convert.ToInt32(temp)).Name;
                    }
                }
            }
            if (favorableActivity.GiftID != string.Empty)
            {
                GiftSearchInfo giftSearch = new GiftSearchInfo();
                giftSearch.InGiftID = favorableActivity.GiftID;
                giftList = GiftBLL.SearchGiftList(giftSearch);
            }

            Title = favorableActivity.Name;
        }
    }
}

