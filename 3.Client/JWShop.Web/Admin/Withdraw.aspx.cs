using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;
using SkyCES.EntLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JWShop.Web.Admin
{
    public partial class Withdraw : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadWithdraw", PowerCheckType.Single);
                //状态
                var list= EnumHelper.ReadEnumList<Withdraw_Status>();
              foreach(EnumInfo item in list)
                {
                    Status.Items.Add(new ListItem(item.ChineseName, item.Value.ToString()));
                }
                Status.Items.Insert(0, new ListItem("请选择", string.Empty));

                int distributorId = RequestHelper.GetQueryString<int>("distributorId");
                UserInfo distributor = UserBLL.Read(distributorId);
                StartTime.Text = RequestHelper.GetQueryString<string>("StartTime");
                EndTime.Text = RequestHelper.GetQueryString<string>("EndTime");
                UserName.Text = HttpUtility.UrlDecode((distributor.Id > 0 ? distributor.UserName : RequestHelper.GetQueryString<string>("UserName")),System.Text.Encoding.UTF8);
                Mobile.Text = RequestHelper.GetQueryString<string>("Mobile");
                Status.Text = RequestHelper.GetQueryString<string>("Status");
                var dataList = WithdrawBLL.SearchList(
                    CurrentPage,
                    PageSize,
                    new WithdrawSearchInfo
                    {
                        Distributor_Id = distributorId,
                        StartTime = RequestHelper.GetQueryString<DateTime>("StartTime"),
                        EndTtime = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndTime")),
                        UserName = HttpUtility.UrlEncode(RequestHelper.GetQueryString<string>("UserName"), System.Text.Encoding.UTF8),
                        Mobile = RequestHelper.GetQueryString<string>("Mobile"),
                        Status = RequestHelper.GetQueryString<int>("Status")
                    },
                    ref Count);
                dataList.ForEach(k => k.UserName = HttpUtility.UrlDecode(k.UserName, System.Text.Encoding.UTF8));
                BindControl(dataList, RecordList, MyPager);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "Withdraw.aspx?Action=search&";
            URL += "UserName=" + UserName.Text + "&";
            URL += "Mobile=" + Mobile.Text + "&";
            URL += "StartTime=" + StartTime.Text + "&";
            URL += "EndTime=" + EndTime.Text + "&";
            URL += "Status=" + Status.SelectedValue;
            ResponseHelper.Redirect(URL);
        }
    }
}