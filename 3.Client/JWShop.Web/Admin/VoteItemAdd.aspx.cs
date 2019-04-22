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
    public partial class VoteItemAdd : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                foreach (VoteInfo voteInfo in VoteBLL.ReadNamedList())
                {
                    VoteType.Items.Add(new ListItem(voteInfo.Title, voteInfo.ID.ToString()));
                }
            
                int itemID = RequestHelper.GetQueryString<int>("ID");
                if (itemID != int.MinValue)
                {
                    CheckAdminPower("ReadVoteItem", PowerCheckType.Single);
                    VoteItemInfo voteItem = VoteItemBLL.ReadVoteItem(itemID);
                    ItemName.Text = voteItem.ItemName;
                    Department.Text = voteItem.Department;
                    Photo.Text = voteItem.Image;
                    Solution.Text = voteItem.Solution;
                    Point.Text = voteItem.Point;
                    CoverDepartment.Text = voteItem.CoverDepartment;
                    Content.Value = voteItem.Detail;
                    MobileContent.Value = voteItem.Exp1;//手机站内容
                    //修改时允许编辑排序号
                    OrderID.Enabled = true;
                    OrderID.Text = voteItem.OrderID.ToString();
                    VoteType.Text = VoteBLL.GetLastClassID(voteItem.VoteID).ToString();

                    IsShow.Text = string.IsNullOrEmpty(voteItem.Exp2) ? "0" : voteItem.Exp2.Trim().ToString();
                }
                else
                {
                    //添加时不允许编辑排序号
                    OrderID.Enabled = false;
                    OrderID.Text = (VoteItemBLL.MaxOrderID() + 1).ToString();                 
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
            VoteItemInfo voteItem = new VoteItemInfo();
            voteItem.ID = RequestHelper.GetQueryString<int>("ID");
          
            voteItem.VoteID = VoteBLL.ReadVoteFullFatherID(Convert.ToInt32(VoteType.Text));
            voteItem.ItemName = ItemName.Text;
            voteItem.Department = Department.Text;
            voteItem.Image = Photo.Text;
            voteItem.Solution = Solution.Text;
            voteItem.Point = Point.Text;
            voteItem.CoverDepartment = CoverDepartment.Text;
            voteItem.Detail = Content.Value;
            voteItem.Exp1 = MobileContent.Value;//手机站内容
            voteItem.OrderID = Convert.ToInt32(OrderID.Text);
            voteItem.Exp2 = IsShow.Text;
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (voteItem.ID == int.MinValue)
            {
                CheckAdminPower("AddVoteItem", PowerCheckType.Single);
                int id = VoteItemBLL.AddVoteItem(voteItem);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("VoteItem"), id);
            }
            else
            {
                CheckAdminPower("UpdateVoteItem", PowerCheckType.Single);
                VoteItemBLL.UpdateVoteItem(voteItem);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("VoteItem"), voteItem.ID);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}