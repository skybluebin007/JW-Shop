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

namespace JWShop.Page
{
    public class Table : CommonBasePage
    {
        protected SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();
        protected SingleUnlimitClass singleUnlimitClass1 = new SingleUnlimitClass();
        protected override void PageLoad()
        {
            base.PageLoad();
            singleUnlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();
            singleUnlimitClass1.DataSource = RegionBLL.ReadRegionUnlimitClass();
            singleUnlimitClass1.Prefix = "singclass1";
            if (RequestHelper.GetQueryString<string>("Action") == "UseMessage") { UseMessage(); }
            //singleUnlimitClass.ClassID;
        }

        public void UseMessage()
        {
            UserMessageInfo usmInfo = new UserMessageInfo();
            
            string result = "ok|/HZ/Table.html";
            
                
            
            string name = RequestHelper.GetForm<string>("name");
            string sex = RequestHelper.GetForm<string>("sex");
            string borthdate = RequestHelper.GetForm<string>("borthdate");
            string job = RequestHelper.GetForm<string>("job");
            string jiguan = RegionBLL.RegionNameList(singleUnlimitClass.ClassID);
            string xianzhuju = RegionBLL.RegionNameList(singleUnlimitClass1.ClassID);
            string address = RequestHelper.GetForm<string>("address");
            string phone = RequestHelper.GetForm<string>("phone");
            if (RequestHelper.GetQueryString<string>("cstyle") == "Mobile")
            {
                result = "ok|/HZ/Mobile/Table.html";
                borthdate = RequestHelper.GetForm<string>("datetime");
                jiguan = RequestHelper.GetForm<string>("checkCity") + " " + RequestHelper.GetForm<string>("checkCity1") + " " + RequestHelper.GetForm<string>("checkCity2");
                xianzhuju = RequestHelper.GetForm<string>("address") + " " + RequestHelper.GetForm<string>("address1") + " " + RequestHelper.GetForm<string>("address2");
                address = RequestHelper.GetForm<string>("orAddress");
            }
            usmInfo.Title = "申请成为照护人员";
            usmInfo.Liveplace = sex;
            usmInfo.UserName = name;
            usmInfo.Tel = phone;
            usmInfo.Email = job;
            usmInfo.Birthday = borthdate;
            usmInfo.Address = address;
            usmInfo.AddCol1 = jiguan;
            usmInfo.AddCol2 = xianzhuju;
            usmInfo.UserIP = ClientHelper.IP;
            usmInfo.MessageClass = 8;
            UserMessageBLL.Add(usmInfo);
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
    }
}
