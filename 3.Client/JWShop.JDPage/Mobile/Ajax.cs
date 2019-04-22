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
using Newtonsoft.Json;
using System.Text;
using System.Linq;

namespace JWShop.Page.Mobile
{
    public class Ajax : AjaxBasePage
    {
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "MessageSubmit":
                    //MessageSubmit(RequestHelper.GetQueryString<string>("type"));
                    break;
                case "GetVerifyCode":
                    GetVerifyCode();
                    break;
                case "GetPrize":
                    this.GetPrize();
                    break;
            }
        }

        /// <summary>
        /// 获取短信验证码
        /// </summary>
        private void GetVerifyCode()
        {
            string mobile = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("mobile"));
            bool isSuccess = false;
            string msg = "";

            if (string.IsNullOrEmpty(mobile))
            {
                ResponseHelper.Write("error|请输入手机号码");
                ResponseHelper.End();
            }

            isSuccess = true;
            //isSuccess = WebService.GetHttp.PostSms(mobile, out msg);
            if (isSuccess)
            {
                CookiesHelper.AddCookie("verify_send", DateTime.Now.ToString(), 1, TimeType.Minute);
                ResponseHelper.Write("ok|");
                ResponseHelper.End();
            }
            else
            {
                ResponseHelper.Write("error|" + msg);
                ResponseHelper.End();
            }
        }
        /// <summary>
        /// 大转盘抽奖
        /// </summary>
        private void GetPrize()
        {
            Response.ContentType = "application/json";
            //当前用户 
            UserInfo theUser = UserBLL.Read(base.UserId);
            int result = 1;
            if (int.TryParse(Request["activityid"], out result) && result > 0 && theUser.Id > 0)
            {// 如果参数 activityid有效才执行抽奖
                LotteryActivityInfo lotteryActivity = LotteryActivityBLL.Read(result);
                if (lotteryActivity != null)
                {
                    lotteryActivity.PrizeSettingList = JsonConvert.DeserializeObject<List<PrizeSetting>>(lotteryActivity.PrizeSetting);
                }               
                int userPrizeCount =PrizeRecordBLL.GetUserPrizeCountToday(result,theUser.Id);//获取该用户当天抽奖次数
                //if (MemberProcessor.GetCurrentMember() == null)
                //{
                //    MemberInfo member = new MemberInfo();
                //    string generateId = Globals.GetGenerateId();
                //    member.GradeId = MemberProcessor.GetDefaultMemberGrade();
                //    member.UserName = "";
                //    member.OpenId = "";
                //    member.CreateDate = DateTime.Now;
                //    member.SessionId = generateId;
                //    member.SessionEndTime = DateTime.Now;
                //    MemberProcessor.CreateMember(member);
                //    member = MemberProcessor.GetMember(generateId);
                //    HttpCookie cookie = new HttpCookie("Vshop-Member")
                //    {
                //        Value = member.UserId.ToString(),
                //        Expires = DateTime.Now.AddYears(10)
                //    };
                //    HttpContext.Current.Response.Cookies.Add(cookie);
                //}
                StringBuilder builder = new StringBuilder();
                builder.Append("{");
                int maxNumOfLottery = lotteryActivity.MaxNum;
                //分享增加一次投资机会            
                //if (theUser.HasShared == 1)
                //{
                //    maxNumOfLottery++;
                //}

                if ((DateTime.Now.Date < lotteryActivity.StartTime.Date) || (DateTime.Now.Date > lotteryActivity.EndTime.Date))
                {
                    builder.Append("\"No\":\"-3\"");
                    builder.Append("}");
                    Response.Write(builder.ToString());
                }
                else if (userPrizeCount >= maxNumOfLottery)
                {
                    builder.Append("\"No\":\"-1\"");
                    builder.Append("}");
                    Response.Write(builder.ToString());
                }
                else
                {
                    List<PrizeRecordInfo> prizeList = PrizeRecordBLL.SearchList(new PrizeRecordSearchInfo { ActivityID = result, IsPrize=1,});
                    int num3 = 0;
                    int num4 = 0;
                    int num5 = 0;
                    int num6 = 0;
                    int num7 = 0;
                    int num8 = 0;
                    int num7Count = 0;
                    int num8Count = 0;

                    bool hasPrized = false;
                    if ((prizeList != null) && (prizeList.Count > 0))
                    {
                        num3 =prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "一等奖");
                        num4 = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "二等奖");
                        num5 = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "三等奖");
                    }

                    //prizeList = PrizeRecordBLL.GetPrizeList(result, theUser.Id);
                    ////百分百中奖无须判断是否有获奖
                    //if ((prizeList != null) && (prizeList.Count > 0))
                    //{
                    //    //foreach (PrizeRecordInfo prizeRecord in prizeList)
                    //    //{
                    //    //    if (prizeRecord.Prizelevel != "六等奖")
                    //    //    {
                    //            hasPrized = true;
                    //            //break;
                    //    //    }
                    //    //}
                    //}
                    if (hasPrized)//如果已中奖则不能再参与抽奖                    
                    {
                        builder.Append("\"No\":\"-2\"");
                        builder.Append("}");
                        Response.Write(builder.ToString());
                    }
                    else
                    {
                        PrizeRecordInfo model = new PrizeRecordInfo
                        {
                            PrizeTime = new DateTime?(DateTime.Now),
                            UserID = theUser.Id,
                            UserName=theUser.UserName,
                            ActivityName = lotteryActivity.ActivityName,
                            ActivityID = result,
                            RealName = Request["RealName"],
                            Company = Request["Company"],
                            CellPhone = Request["CellPhone"],
                            IsPrize = (int)BoolType.True
                        };
                        List<PrizeSetting> prizeSettingList = lotteryActivity.PrizeSettingList;

                        decimal num9 = prizeSettingList[0].Probability * 100M;
                        decimal num10 = num9 + prizeSettingList[1].Probability * 100M;
                        decimal num11 = num10 + prizeSettingList[2].Probability * 100M;
                  
                        int num15 = new Random(Guid.NewGuid().GetHashCode()).Next(1, 0x2711);
                        #region //抽奖
                        if (prizeSettingList.Count > 3)//如果开启了4-8等奖
                        {                           
                            decimal num12 = num11 + prizeSettingList[3].Probability * 100M;
                            decimal num13 = num12 + prizeSettingList[4].Probability * 100M;
                            decimal num14 = num13 + prizeSettingList[5].Probability * 100M;
                            decimal num7Rate = num14 + prizeSettingList[6].Probability * 100M;
                            decimal num8Rate = num7Rate + prizeSettingList[7].Probability * 100M;

                            num6 = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "四等奖");
                            num7 = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "五等奖");
                            num8 = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "六等奖");
                            num7Count = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "七等奖");
                            num8Count = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "八等奖");

                            if (prizeList.Count >= (prizeSettingList[0].PrizeNum + prizeSettingList[1].PrizeNum + prizeSettingList[2].PrizeNum + prizeSettingList[3].PrizeNum + prizeSettingList[4].PrizeNum + prizeSettingList[5].PrizeNum + prizeSettingList[6].PrizeNum + prizeSettingList[7].PrizeNum))//如果奖品已经抽完
                            {
                                model.IsPrize = (int)BoolType.False;
                                builder.Append("\"No\":\"-4\"");
                            }
                            else if ((num15 <= num9) && (prizeSettingList[0].PrizeNum > num3))
                            {

                                builder.Append("\"No\":\"1\",\"Prizelevel\":\"" + prizeSettingList[0].PrizeLevel + "\",\"PrizeTitle\":\"" + prizeSettingList[0].PrizeTitle + "\",\"PrizeName\":\"" + prizeSettingList[0].PrizeName + "\"");
                               
                                model.Prizelevel = "一等奖";
                                model.PrizeName = prizeSettingList[0].PrizeName;
                            }
                            else if ((num15 > num9) && (num15 <= num10) && (prizeSettingList[1].PrizeNum > num4))
                            {
                                builder.Append("\"No\":\"2\",\"Prizelevel\":\"" + prizeSettingList[1].PrizeLevel + "\",\"PrizeTitle\":\"" + prizeSettingList[1].PrizeTitle + "\",\"PrizeName\":\"" + prizeSettingList[1].PrizeName + "\"");
                                model.Prizelevel = "二等奖";
                                model.PrizeName = prizeSettingList[1].PrizeName;
                            }
                            else if ((num15 > num10) && (num15 <= num11) && (prizeSettingList[2].PrizeNum > num5))
                            {
                                builder.Append("\"No\":\"3\",\"Prizelevel\":\"" + prizeSettingList[2].PrizeLevel + "\",\"PrizeTitle\":\"" + prizeSettingList[2].PrizeTitle + "\",\"PrizeName\":\"" + prizeSettingList[2].PrizeName + "\"");
                                model.Prizelevel = "三等奖";
                                model.PrizeName = prizeSettingList[2].PrizeName;
                            }
                            else if ((num15 > num11) && (num15 <= num12) && (prizeSettingList[3].PrizeNum > num6))
                            {
                                builder.Append("\"No\":\"4\",\"Prizelevel\":\"" + prizeSettingList[3].PrizeLevel + "\",\"PrizeTitle\":\"" + prizeSettingList[3].PrizeTitle + "\",\"PrizeName\":\"" + prizeSettingList[3].PrizeName + "\"");
                                model.Prizelevel = "四等奖";
                                model.PrizeName = prizeSettingList[3].PrizeName;
                            }
                            else if ((num15 > num12) && (num15 <= num13) && (prizeSettingList[4].PrizeNum > num7))
                            {
                                builder.Append("\"No\":\"5\",\"Prizelevel\":\"" + prizeSettingList[4].PrizeLevel + "\",\"PrizeTitle\":\"" + prizeSettingList[4].PrizeTitle + "\",\"PrizeName\":\"" + prizeSettingList[4].PrizeName + "\"");
                                model.Prizelevel = "五等奖";
                                model.PrizeName = prizeSettingList[4].PrizeName;
                            }
                            else if ((num15 > num13) && (num15 <= num14) && (prizeSettingList[5].PrizeNum > num8))
                            {
                                builder.Append("\"No\":\"6\",\"Prizelevel\":\"" + prizeSettingList[5].PrizeLevel + "\",\"PrizeTitle\":\"" + prizeSettingList[5].PrizeTitle + "\",\"PrizeName\":\"" + prizeSettingList[5].PrizeName + "\"");
                                model.Prizelevel = "六等奖";
                                model.PrizeName = prizeSettingList[5].PrizeName;
                            }
                            else if ((num15 > num14) && (num15 <= num7Rate) && (prizeSettingList[6].PrizeNum > num7Count))
                            {
                                //if (lotteryActivity.ActivityId == 6)
                                //{
                                //    model.IsPrize = false;
                                //    builder.Append("\"No\":\"0\"");
                                //}
                                //else
                                //{
                                builder.Append("\"No\":\"7\",\"Prizelevel\":\"" + prizeSettingList[6].PrizeLevel + "\",\"PrizeTitle\":\"" + prizeSettingList[6].PrizeTitle + "\",\"PrizeName\":\"" + prizeSettingList[6].PrizeName + "\"");
                                    model.Prizelevel = "七等奖";
                                    model.PrizeName = prizeSettingList[6].PrizeName;
                                //}
                            }
                            else if ((num15 > num7Rate) && (num15 <= num8Rate) && (prizeSettingList[7].PrizeNum > num8Count))
                            {
                                //if (lotteryActivity.ActivityId == 6)
                                //{
                                //    model.IsPrize = false;
                                //    builder.Append("\"No\":\"0\"");
                                //}
                                //else
                                //{
                                builder.Append("\"No\":\"8\",\"Prizelevel\":\"" + prizeSettingList[7].PrizeLevel + "\",\"PrizeTitle\":\"" + prizeSettingList[7].PrizeTitle + "\",\"PrizeName\":\"" + prizeSettingList[7].PrizeName + "\"");
                                    model.Prizelevel = "八等奖";
                                    model.PrizeName = prizeSettingList[7].PrizeName;
                                //}
                            }
                            else
                            {
                                model.IsPrize = (int)BoolType.False;
                                builder.Append("\"No\":\"0\"");
                            }
                        }
                        else
                        {//如果只开启了1-3等奖
                            if (prizeList.Count >= (prizeSettingList[0].PrizeNum + prizeSettingList[1].PrizeNum + prizeSettingList[2].PrizeNum))//如果奖品已经抽完
                            {
                                model.IsPrize = (int)BoolType.False;
                                builder.Append("\"No\":\"-4\"");
                            }
                            else if ((num15 < num9) && (prizeSettingList[0].PrizeNum > num3))
                            {
                                builder.Append("\"No\":\"1\",\"Prizelevel\":\"" + prizeSettingList[0].PrizeLevel + "\",\"PrizeTitle\":\"" + prizeSettingList[0].PrizeTitle + "\",\"PrizeName\":\"" + prizeSettingList[0].PrizeName + "\"");
                                model.Prizelevel = "一等奖";
                                model.PrizeName = prizeSettingList[0].PrizeName;
                            }
                            else if ((num15 < num10) && (prizeSettingList[1].PrizeNum > num4))
                            {
                                builder.Append("\"No\":\"2\",\"Prizelevel\":\"" + prizeSettingList[1].PrizeLevel + "\",\"PrizeTitle\":\"" + prizeSettingList[1].PrizeTitle + "\",\"PrizeName\":\"" + prizeSettingList[1].PrizeName + "\"");
                                model.Prizelevel = "二等奖";
                                model.PrizeName = prizeSettingList[1].PrizeName;
                            }
                            else if ((num15 < num11) && (prizeSettingList[2].PrizeNum > num5))
                            {
                                builder.Append("\"No\":\"3\",\"Prizelevel\":\"" + prizeSettingList[2].PrizeLevel + "\",\"PrizeTitle\":\"" + prizeSettingList[2].PrizeTitle + "\",\"PrizeName\":\"" + prizeSettingList[2].PrizeName + "\"");
                                model.Prizelevel = "三等奖";
                                model.PrizeName = prizeSettingList[2].PrizeName;
                            }
                            else
                            {
                                model.IsPrize = (int)BoolType.False;
                                builder.Append("\"No\":\"0\"");
                            }
                        }
                        #endregion
                        builder.Append("}");
                        if (Request["activitytype"] != "scratch")
                        {
                            if (model.IsPrize== (int)BoolType.True)
                            {
                                int addNum = PrizeRecordBLL.Add(model);//插入抽奖纪录

                                if (addNum <= 0) builder.Clear().Append("\"No\":\"0\"");
                                #region 发送短信
                                if (model.ActivityID == 5)
                                {
                                    string sendMobile = Request["cellPhone"];
                                    if (sendMobile != string.Empty)
                                    {
                                        string sendStr = string.Empty;
                                        switch (model.Prizelevel)
                                        {
                                            case "一等奖":
                                                sendStr = prizeSettingList[0].PrizeTitle;                                              
                                                break;
                                            case "二等奖":
                                                sendStr = prizeSettingList[1].PrizeTitle;
                                                break;
                                            case "三等奖":
                                                sendStr = prizeSettingList[2].PrizeTitle;
                                                break;
                                            case "四等奖":
                                                sendStr = prizeSettingList[3].PrizeTitle;
                                                break;
                                            case "五等奖":
                                                sendStr = prizeSettingList[4].PrizeTitle;
                                                break;
                                            case "六等奖":
                                                sendStr = prizeSettingList[5].PrizeTitle;
                                                break;
                                            case "七等奖":
                                                sendStr = prizeSettingList[6].PrizeTitle;
                                                break;
                                            case "八等奖":
                                                sendStr = prizeSettingList[7].PrizeTitle;
                                                break;
                                        }
                                        //SendSms(sendMobile, sendStr);//发送短信
                                    }
                                }
                                #endregion
                            }
                        }
                        Response.Clear();
                        Response.Write(builder.ToString());
                        Response.End();

                    }
                }
            }
          
        }

    }
}