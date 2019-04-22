using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;
using SkyCES.EntLib;
using Newtonsoft.Json;
using System.Linq;

namespace JWShop.Web.Admin
{
    public partial class LotteryActivityAdd : System.Web.UI.Page
    {
        protected LotteryActivityInfo lotteryActivity = new LotteryActivityInfo();
        protected List<PrizeSetting> prizeList = new List<PrizeSetting>();
        protected void Page_Load(object sender, EventArgs e)
        {         
            int type = 0, activityid = 0;
            if (!int.TryParse(Request.QueryString["type"], out type))
            {
                ScriptHelper.Alert("参数错误");
            }
            else
            {
                if (!IsPostBack)
                {
                    lotteryActivity = LotteryActivityBLL.Read(RequestHelper.GetQueryString<int>("id"));
                    if (lotteryActivity.Id > 0)
                    {
                        Content.Value = lotteryActivity.ActivityDesc;
                       prizeList=lotteryActivity.PrizeSettingList = JsonConvert.DeserializeObject<List<PrizeSetting>>(lotteryActivity.PrizeSetting);
                        Photo.Text = lotteryActivity.ActivityPic;
                    }
                }
            }
        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string errorMsg = string.Empty;
            LotteryActivityInfo activity = new LotteryActivityInfo();
            activity.Id = RequestHelper.GetQueryString<int>("id");
            activity.ActivityType = RequestHelper.GetQueryString<int>("type") <= 0 ? 1 : RequestHelper.GetQueryString<int>("type"); 
            activity.ActivityName = RequestHelper.GetForm<string>("Title");
            if (string.IsNullOrEmpty(activity.ActivityName))
            {
                errorMsg="活动名称不得为空";
            }
            if (string.IsNullOrEmpty(errorMsg))
            {
                activity.ActivityKey = RequestHelper.GetForm<string>("Keyword").Trim();
                if (string.IsNullOrEmpty(activity.ActivityKey))
                {
                    errorMsg = "活动关键字不得为空";
                }
            }          
           if (string.IsNullOrEmpty(errorMsg) && !LotteryActivityBLL.UniqueKey(activity.ActivityKey,activity.Id)) {
               errorMsg = "活动关键字重复";
           }
           if (string.IsNullOrEmpty(errorMsg)) {
               string startTime=RequestHelper.GetForm<string>("StartDate");
               DateTime st=DateTime.Now;
               if (DateTime.TryParse(startTime, out st))
               {
                   activity.StartTime = st;
               }
               else
               {
                   errorMsg = "活动开始时间错误";
               }               
           }
           if (string.IsNullOrEmpty(errorMsg))
           {
               string endTime = RequestHelper.GetForm<string>("EndDate");
               DateTime et = DateTime.Now;
               if (DateTime.TryParse(endTime, out et))
               {
                   activity.EndTime = et;
               }
               else
               {
                   errorMsg = "活动结束时间错误";
               }
           }
           if (string.IsNullOrEmpty(errorMsg)) {
               if ((activity.StartTime.Date - activity.EndTime.Date).Days > 0) {
                   errorMsg = "结束时间不得小于开始时间";
               }
           }
           if (string.IsNullOrEmpty(errorMsg)) {
               activity.MaxNum = RequestHelper.GetForm<int>("MaxNum");
               if (activity.MaxNum <= 0) errorMsg = "可中奖次数必须是正整数";
           }
           if (string.IsNullOrEmpty(errorMsg)) {
              activity.ActivityDesc = Content.Value.FilterBadwords();
              string txtPrizeTitle1 = RequestHelper.GetForm<string>("txtPrizeTitle1");
              string txtPrize1 = RequestHelper.GetForm<string>("txtPrize1");              
              if (string.IsNullOrEmpty(txtPrizeTitle1)) errorMsg = "一等奖名称不得为空";
           }
           if (string.IsNullOrEmpty(errorMsg)) {
               int txtPrize1Num = RequestHelper.GetForm<int>("txtPrize1Num");
               if (txtPrize1Num <= 0) errorMsg = "一等奖奖品数量错误";
           }
           if (string.IsNullOrEmpty(errorMsg))
           {
               decimal txtProbability1 = RequestHelper.GetForm<decimal>("txtProbability1");
               if (txtProbability1 < 0) errorMsg = "一等奖中奖概率错误";
           }
           if (string.IsNullOrEmpty(errorMsg)) {
               string txtPrizeTitle2 = RequestHelper.GetForm<string>("txtPrizeTitle2");
               string txtPrize2 = RequestHelper.GetForm<string>("txtPrize2");
               if (string.IsNullOrEmpty(txtPrizeTitle2)) errorMsg = "二等奖名称不得为空";
           }
           if (string.IsNullOrEmpty(errorMsg))
           {
               int txtPrize2Num = RequestHelper.GetForm<int>("txtPrize2Num");
               if (txtPrize2Num <= 0) errorMsg = "二等奖奖品数量错误";
           }
           if (string.IsNullOrEmpty(errorMsg))
           {
               decimal txtProbability2 = RequestHelper.GetForm<decimal>("txtProbability2");
               if (txtProbability2 < 0) errorMsg = "二等奖中奖概率错误";
           }
           if (string.IsNullOrEmpty(errorMsg))
           {
               string txtPrizeTitle3 = RequestHelper.GetForm<string>("txtPrizeTitle3");
               string txtPrize3 = RequestHelper.GetForm<string>("txtPrize3");
               if (string.IsNullOrEmpty(txtPrizeTitle3)) errorMsg = "三等奖名称不得为空";
           }
           if (string.IsNullOrEmpty(errorMsg))
           {
               int txtPrize3Num = RequestHelper.GetForm<int>("txtPrize3Num");
               if (txtPrize3Num <= 0) errorMsg = "三等奖奖品数量错误";
           }
           if (string.IsNullOrEmpty(errorMsg))
           {
               decimal txtProbability3 = RequestHelper.GetForm<decimal>("txtProbability3");
               if (txtProbability3 < 0) errorMsg = "三等奖中奖概率错误";
           }
            List<PrizeSetting> prizeList = new List<PrizeSetting>();
           if (string.IsNullOrEmpty(errorMsg)) {

               prizeList.Add(new PrizeSetting
               {
                   PrizeTitle = RequestHelper.GetForm<string>("txtPrizeTitle1"),
                   PrizeLevel = "一等奖",
                   PrizeName = RequestHelper.GetForm<string>("txtPrize1"),
                   PrizeNum = RequestHelper.GetForm<int>("txtPrize1Num"),
                   Probability = RequestHelper.GetForm<decimal>("txtProbability1")
               });
               prizeList.Add(new PrizeSetting
               {
                   PrizeTitle = RequestHelper.GetForm<string>("txtPrizeTitle2"),
                   PrizeLevel = "二等奖",
                   PrizeName = RequestHelper.GetForm<string>("txtPrize2"),
                   PrizeNum = RequestHelper.GetForm<int>("txtPrize2Num"),
                   Probability = RequestHelper.GetForm<decimal>("txtProbability2")
               });
               prizeList.Add(new PrizeSetting
               {
                   PrizeTitle = RequestHelper.GetForm<string>("txtPrizeTitle3"),
                   PrizeLevel = "三等奖",
                   PrizeName = RequestHelper.GetForm<string>("txtPrize3"),
                   PrizeNum = RequestHelper.GetForm<int>("txtPrize3Num"),
                   Probability = RequestHelper.GetForm<decimal>("txtProbability3")
               });
           }
           #region 开启4-8等奖
           //如果开启了4-8等奖
           if (RequestHelper.GetForm<int>("ChkOpen") == 1)
           {
               if (string.IsNullOrEmpty(errorMsg))
               {
                   string txtPrizeTitle4 = RequestHelper.GetForm<string>("txtPrizeTitle4");
                   string txtPrize4 = RequestHelper.GetForm<string>("txtPrize4");
                   if (string.IsNullOrEmpty(txtPrizeTitle4)) errorMsg = "四等奖名称不得为空";
               }
               if (string.IsNullOrEmpty(errorMsg))
               {
                   int txtPrize4Num = RequestHelper.GetForm<int>("txtPrize4Num");
                   if (txtPrize4Num <= 0) errorMsg = "四等奖奖品数量错误";
               }
               if (string.IsNullOrEmpty(errorMsg))
               {
                   decimal txtProbability4 = RequestHelper.GetForm<decimal>("txtProbability4");
                   if (txtProbability4 < 0) errorMsg = "四等奖中奖概率错误";
               }
               if (string.IsNullOrEmpty(errorMsg))
               {
                   string txtPrizeTitle5 = RequestHelper.GetForm<string>("txtPrizeTitle5");
                   string txtPrize5 = RequestHelper.GetForm<string>("txtPrize5");
                   if (string.IsNullOrEmpty(txtPrizeTitle5)) errorMsg = "五等奖名称不得为空";
               }
               if (string.IsNullOrEmpty(errorMsg))
               {
                   int txtPrize5Num = RequestHelper.GetForm<int>("txtPrize5Num");
                   if (txtPrize5Num <= 0) errorMsg = "五等奖奖品数量错误";
               }
               if (string.IsNullOrEmpty(errorMsg))
               {
                   decimal txtProbability5 = RequestHelper.GetForm<decimal>("txtProbability5");
                   if (txtProbability5 < 0) errorMsg = "五等奖中奖概率错误";
               }
               if (string.IsNullOrEmpty(errorMsg))
               {
                   string txtPrizeTitle6 = RequestHelper.GetForm<string>("txtPrizeTitle6");
                   string txtPrize6 = RequestHelper.GetForm<string>("txtPrize6");
                   if (string.IsNullOrEmpty(txtPrizeTitle6)) errorMsg = "六等奖名称不得为空";
               }
               if (string.IsNullOrEmpty(errorMsg))
               {
                   int txtPrize6Num = RequestHelper.GetForm<int>("txtPrize6Num");
                   if (txtPrize6Num <= 0) errorMsg = "六等奖奖品数量错误";
               }
               if (string.IsNullOrEmpty(errorMsg))
               {
                   decimal txtProbability6 = RequestHelper.GetForm<decimal>("txtProbability6");
                   if (txtProbability6 < 0) errorMsg = "六等奖中奖概率错误";
               }
               if (string.IsNullOrEmpty(errorMsg))
               {
                   string txtPrizeTitle7 = RequestHelper.GetForm<string>("txtPrizeTitle7");
                   string txtPrize7 = RequestHelper.GetForm<string>("txtPrize7");
                   if (string.IsNullOrEmpty(txtPrizeTitle7)) errorMsg = "七等奖名称不得为空";
               }
               if (string.IsNullOrEmpty(errorMsg))
               {
                   int txtPrize7Num = RequestHelper.GetForm<int>("txtPrize7Num");
                   if (txtPrize7Num <= 0) errorMsg = "七等奖奖品数量错误";
               }
               if (string.IsNullOrEmpty(errorMsg))
               {
                   decimal txtProbability7 = RequestHelper.GetForm<decimal>("txtProbability7");
                   if (txtProbability7 < 0) errorMsg = "七等奖中奖概率错误";
               }
               if (string.IsNullOrEmpty(errorMsg))
               {
                   string txtPrizeTitle8 = RequestHelper.GetForm<string>("txtPrizeTitle8");
                   string txtPrize8 = RequestHelper.GetForm<string>("txtPrize8");
                   if (string.IsNullOrEmpty(txtPrizeTitle8)) errorMsg = "八等奖名称不得为空";
               }
               if (string.IsNullOrEmpty(errorMsg))
               {
                   int txtPrize8Num = RequestHelper.GetForm<int>("txtPrize8Num");
                   if (txtPrize8Num <= 0) errorMsg = "八等奖奖品数量错误";
               }
               if (string.IsNullOrEmpty(errorMsg))
               {
                   decimal txtProbability8 = RequestHelper.GetForm<decimal>("txtProbability8");
                   if (txtProbability8 < 0) errorMsg = "八等奖中奖概率错误";
               }
               if (string.IsNullOrEmpty(errorMsg))
               {
                   prizeList.Add(new PrizeSetting
                   {
                       PrizeTitle = RequestHelper.GetForm<string>("txtPrizeTitle4"),
                       PrizeLevel = "四等奖",
                       PrizeName = RequestHelper.GetForm<string>("txtPrize4"),
                       PrizeNum = RequestHelper.GetForm<int>("txtPrize4Num"),
                       Probability = RequestHelper.GetForm<decimal>("txtProbability4")
                   });
                   prizeList.Add(new PrizeSetting
                   {
                       PrizeTitle = RequestHelper.GetForm<string>("txtPrizeTitle5"),
                       PrizeLevel = "五等奖",
                       PrizeName = RequestHelper.GetForm<string>("txtPrize5"),
                       PrizeNum = RequestHelper.GetForm<int>("txtPrize5Num"),
                       Probability = RequestHelper.GetForm<decimal>("txtProbability5")
                   });
                   prizeList.Add(new PrizeSetting
                   {
                       PrizeTitle = RequestHelper.GetForm<string>("txtPrizeTitle6"),
                       PrizeLevel = "六等奖",
                       PrizeName = RequestHelper.GetForm<string>("txtPrize6"),
                       PrizeNum = RequestHelper.GetForm<int>("txtPrize6Num"),
                       Probability = RequestHelper.GetForm<decimal>("txtProbability6")
                   });
                   prizeList.Add(new PrizeSetting
                   {
                       PrizeTitle = RequestHelper.GetForm<string>("txtPrizeTitle7"),
                       PrizeLevel = "七等奖",
                       PrizeName = RequestHelper.GetForm<string>("txtPrize7"),
                       PrizeNum = RequestHelper.GetForm<int>("txtPrize7Num"),
                       Probability = RequestHelper.GetForm<decimal>("txtProbability7")
                   });
                   prizeList.Add(new PrizeSetting
                   {
                       PrizeTitle = RequestHelper.GetForm<string>("txtPrizeTitle8"),
                       PrizeLevel = "八等奖",
                       PrizeName = RequestHelper.GetForm<string>("txtPrize8"),
                       PrizeNum = RequestHelper.GetForm<int>("txtPrize8Num"),
                       Probability = RequestHelper.GetForm<decimal>("txtProbability8")
                   });
               }             
           }
           #endregion
           if (string.IsNullOrEmpty(errorMsg))
           {
               decimal sumProbability = prizeList.Sum(k => k.Probability);
               if (sumProbability > 100) errorMsg = "中奖概率总和不得超过100%";
           }
           if (string.IsNullOrEmpty(errorMsg)) {
               activity.PrizeSettingList = prizeList;
               string str = JsonConvert.SerializeObject(activity.PrizeSettingList);
               activity.PrizeSetting = str;
               activity.OpenTime = DateTime.Now;
               activity.ActivityPic = Photo.Text;
               if (string.IsNullOrEmpty(activity.ActivityPic)) errorMsg = "请上传图片";
           }
           if (string.IsNullOrEmpty(errorMsg))
           {
               string alertMessage = ShopLanguage.ReadLanguage("AddOK");
               if (activity.Id<=0)
               {
                   //CheckAdminPower("AddArticle", PowerCheckType.Single);
                   int id = LotteryActivityBLL.Add(activity);
                   AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("LotteryActivity"), id);
               }
               else
               {
                   //CheckAdminPower("UpdateArticle", PowerCheckType.Single);
                   LotteryActivityBLL.Update(activity);
                   AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("LotteryActivity"), activity.Id);
                   alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
               }
               ScriptHelper.Alert(alertMessage, "LotteryActivity.aspx?type=" + activity.ActivityType);
           }
           else {
               ScriptHelper.Alert(errorMsg);
           }
        }
    }
}