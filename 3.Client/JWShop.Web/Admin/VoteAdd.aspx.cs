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
    public partial class VoteAdd : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int voteID = RequestHelper.GetQueryString<int>("ID");
                VoteInfo vote = VoteBLL.ReadVote(voteID);
                FatherID.DataSource = VoteBLL.ReadNamedList().Where(k=>k.ID!=voteID);
                FatherID.DataTextField = "Title";
                FatherID.DataValueField = "ID";
                FatherID.DataBind();
                FatherID.Items.Insert(0, new ListItem("作为最大类", "0"));

                if (voteID != int.MinValue)
                {
                    CheckAdminPower("ReadVote", PowerCheckType.Single);

                    FatherID.Text = vote.FatherID.ToString();
                    Title.Text = vote.Title;                  
                    OrderID.Text = vote.OrderID.ToString();
                    Note.Text = vote.Note;
                }
            }
        }

        /// <summary>
        /// 提交按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            VoteInfo vote = new VoteInfo();
            vote.ID = RequestHelper.GetQueryString<int>("ID");
            vote.FatherID = Convert.ToInt32(FatherID.Text);
            //vote.VoteType = Convert.ToInt32(VoteType.Text);
            vote.Title = Title.Text;
            vote.Note = Note.Text;
            vote.OrderID = Convert.ToInt32(OrderID.Text.Trim());
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (vote.ID == int.MinValue)
            {
                CheckAdminPower("AddVote", PowerCheckType.Single);
                int id = VoteBLL.AddVote(vote);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Vote"), id);
            }
            else
            {
                CheckAdminPower("UpdateVote", PowerCheckType.Single);
                VoteBLL.UpdateVote(vote);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Vote"), vote.ID);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, "Vote.aspx");
        }
    }
}