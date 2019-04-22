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

namespace JWShop.Web.Admin
{
    public partial class VoteItem : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadVoteItem", PowerCheckType.Single);
                foreach (VoteInfo voteInfo in VoteBLL.ReadNamedList())
                {
                    VoteID.Items.Add(new ListItem(voteInfo.Title, "|" + voteInfo.ID + "|"));
                }
                VoteID.Items.Insert(0, new ListItem("所有类别", string.Empty));
                Title.Text = RequestHelper.GetQueryString<string>("Title");
                VoteID.Text = RequestHelper.GetQueryString<string>("VoteID");
                IsShow.Text = RequestHelper.GetQueryString<string>("IsShow");
             

                VoteItemSearchInfo itemSearch = new VoteItemSearchInfo();
                itemSearch.ItemName = RequestHelper.GetQueryString<string>("Title");
                itemSearch.VoteID = !string.IsNullOrEmpty(RequestHelper.GetQueryString<string>("VoteID")) ? "|" + RequestHelper.GetQueryString<string>("VoteID") + "|" : string.Empty;
                itemSearch.Exp2 = RequestHelper.GetQueryString<string>("IsShow");

                BindControl(VoteItemBLL.ReadVoteItemList(CurrentPage, PageSize, itemSearch, ref Count,string.Empty), RecordList, MyPager);             
            }
        }
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteArticle", PowerCheckType.Single);
            string deleteID = RequestHelper.GetIntsForm("SelectID");
            if(!string.IsNullOrEmpty(deleteID)){
                VoteItemBLL.DeleteVoteItem(deleteID);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("VoteItem"), deleteID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }          
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "VoteItem.aspx?Action=search&";
            URL += "Title=" + Title.Text + "&";
            URL += "VoteID=" + VoteID.Text + "&";
            URL += "IsShow=" + IsShow.Text;
            ResponseHelper.Redirect(URL);
        }
    }
}