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
    public class VoteResult : AjaxBasePage
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
        /// 当前投票
        /// </summary>
        protected VoteInfo vote = new VoteInfo();
        /// <summary>
        /// 投票选项
        /// </summary>
        protected List<VoteItemInfo> voteItemList = new List<VoteItemInfo>();
        /// <summary>
        /// 当前行为
        /// </summary>
        protected string action = string.Empty;
        /// <summary>
        /// 页面加载
        /// </summary>
         protected override void PageLoad()
        {
            base.PageLoad();
            int voteID = ShopConfig.ReadConfigInfo().VoteID;
            action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "Vote":
                    Vote(voteID);
                    break;
                case "View":
                    break;
                case "Prepare":
                    break;
                default:
                    break;
            }
            vote = VoteBLL.ReadVote(voteID);
            voteItemList = VoteItemBLL.ReadVoteItemByVote(voteID);
        }
        /// <summary>
        /// 投票
        /// </summary>
        /// <param name="voteID"></param>
        protected void Vote(int voteID)
        {
            string result = "ok";
            if (ShopConfig.ReadConfigInfo().AllowAnonymousVote == (int)BoolType.False && base.UserID == 0)
            {
                result = "还未登录";
            }
            else
            {
                string voteCookies = CookiesHelper.ReadCookieValue("VoteCookies" + voteID.ToString());
                if (ShopConfig.ReadConfigInfo().VoteRestrictTime > 0 && voteCookies != string.Empty)
                {
                    result = "请不要频繁提交";
                }
                else
                {
                    VoteRecordInfo voteRecord = new VoteRecordInfo();
                    voteRecord.VoteID = voteID;
                    voteRecord.ItemID =StringHelper.AddSafe(RequestHelper.GetQueryString<string>("ItemID"));
                    voteRecord.AddDate = RequestHelper.DateNow;
                    voteRecord.UserIP = ClientHelper.IP;
                    voteRecord.UserID = base.UserID;
                    voteRecord.UserName = base.UserName;
                    VoteRecordBLL.AddVoteRecord(voteRecord);
                    if (ShopConfig.ReadConfigInfo().VoteRestrictTime > 0)
                    {
                        CookiesHelper.AddCookie("VoteCookies" + voteID.ToString(), "VoteCookies" + voteID.ToString(), ShopConfig.ReadConfigInfo().VoteRestrictTime, TimeType.Second);
                    }
                }
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
    }
}
