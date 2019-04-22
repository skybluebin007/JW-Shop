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
    public class Ad : CommonBasePage
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
        /// 页面加载
        /// </summary>
         protected override void PageLoad()
        {
            base.PageLoad();
            string url = RequestHelper.GetQueryString<string>("URL");
            AdRecordInfo adRecord = new AdRecordInfo();
            adRecord.AdID = RequestHelper.GetQueryString<int>("AdID");
            adRecord.IP = ClientHelper.IP;
            adRecord.Date = RequestHelper.DateNow;
            adRecord.Page = Request.ServerVariables["HTTP_REFERER"];
            adRecord.UserID = base.UserID;
            adRecord.UserName = base.UserName;
            AdRecordBLL.AddAdRecord(adRecord);
            ResponseHelper.Redirect(url);
        }
    }
}
