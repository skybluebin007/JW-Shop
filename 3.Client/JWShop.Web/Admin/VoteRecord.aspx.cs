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
using System.Linq;
namespace JWShop.Web.Admin
{
    public partial class VoteRecord : JWShop.Page.AdminBasePage
    {
        protected List<VoteItemInfo> voteItemList = new List<VoteItemInfo>();
        protected void Page_Load(object sender, EventArgs e)
        {
           if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadVoteRecord", PowerCheckType.Single);
                int voteID = RequestHelper.GetQueryString<int>("VoteID");
                int voteItemID = RequestHelper.GetQueryString<int>("VoteItemID");
                voteItemList = VoteItemBLL.ReadVoteItemAllList();
                if (voteID > 0) voteItemList = voteItemList.Where(k => k.VoteID.IndexOf("|" + voteID + "|") >= 0).ToList();
                VoteRecordSearchInfo searchInfo = new VoteRecordSearchInfo();
               if(voteID>0) searchInfo.VoteID="|"+voteID+"|";
               if (voteItemID > 0) searchInfo.ItemID = voteItemID.ToString();
               List<VoteRecordInfo> recordList = VoteRecordBLL.ReadVoteRecordList(CurrentPage, PageSize, searchInfo, ref Count);
               BindControl(recordList, RecordList, MyPager);
            }
        }

        /// <summary>
        /// 删除按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteVoteRecord", PowerCheckType.Single);
            string deleteID = RequestHelper.GetIntsForm("SelectID");
            if (deleteID != string.Empty)
            {
                VoteRecordBLL.DeleteVoteRecord(deleteID);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("VoteRecord"), deleteID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }
    }
}