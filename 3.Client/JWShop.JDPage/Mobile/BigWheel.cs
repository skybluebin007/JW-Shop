using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using Newtonsoft.Json;

namespace JWShop.Page.Mobile
{    
   public class BigWheel:UserBasePage
    {
       protected LotteryActivityInfo activity = new LotteryActivityInfo();
       protected List<PrizeRecordInfo> precordList = new List<PrizeRecordInfo>();
       protected override void PageLoad()
       {
           base.PageLoad();
           int id = RequestHelper.GetQueryString<int>("id");           
           activity = LotteryActivityBLL.Read(id);
           if (activity.Id<=0)
           {
               ScriptHelper.AlertFrontApp("参数错误","/mobile/default.html");
           }
           if ((activity.StartTime.Date > DateTime.Now.Date) || (DateTime.Now.Date >activity.EndTime.Date))
           {
               ScriptHelper.AlertFrontApp("活动暂未开始或者已经结束", "/mobile/default.html");
           }
           activity.PrizeSettingList = JsonConvert.DeserializeObject<List<PrizeSetting>>(activity.PrizeSetting);
           precordList = PrizeRecordBLL.SearchList(new PrizeRecordSearchInfo { ActivityID=activity.Id,IsPrize=(int)BoolType.True});
           Title = activity.ActivityName;
       }

    }
}
