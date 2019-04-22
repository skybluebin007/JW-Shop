using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;
using SkyCES.EntLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JWShop.Web.Admin
{
    public partial class Rebate : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadRebate", PowerCheckType.Single);
                int distributorId = RequestHelper.GetQueryString<int>("distributorId");
                UserInfo distributor = UserBLL.Read(distributorId);
                StartTime.Text = RequestHelper.GetQueryString<string>("StartTime");
                EndTime.Text = RequestHelper.GetQueryString<string>("EndTime");
                UserName.Text =HttpUtility.UrlDecode((distributor.Id>0?distributor.UserName: RequestHelper.GetQueryString<string>("UserName")),System.Text.Encoding.UTF8);
                Mobile.Text = RequestHelper.GetQueryString<string>("Mobile");
                var dataList = RebateBLL.SearchList(
                    CurrentPage,
                    PageSize,
                    new RebateSearchInfo
                    {
                        Distributor_Id = distributorId,
                        StartTime = RequestHelper.GetQueryString<DateTime>("StartTime"),
                        EndTtime = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndTime")),
                        UserName = HttpUtility.UrlEncode(RequestHelper.GetQueryString<string>("UserName"), System.Text.Encoding.UTF8),
                        Mobile = RequestHelper.GetQueryString<string>("Mobile")
                    },
                    ref Count);
                //Task.Run(()=> {
                    //var userList = UserBLL.SearchList(new UserSearchInfo { });
                    //dataList.ForEach(k => k.Buyer_UserName = HttpUtility.UrlDecode(((userList.FirstOrDefault(a=>a.Id==k.User_Id)??new UserInfo())).UserName, System.Text.Encoding.UTF8));
                //});
                dataList.ForEach(k => k.UserName = HttpUtility.UrlDecode(k.UserName, System.Text.Encoding.UTF8));
                dataList.ForEach(k => k.Buyer_UserName = HttpUtility.UrlDecode(k.Buyer_UserName, System.Text.Encoding.UTF8));
                BindControl(dataList, RecordList,MyPager);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "Rebate.aspx?Action=search&";
            URL += "UserName=" + UserName.Text + "&";
            URL += "Mobile=" + Mobile.Text + "&";
            URL += "StartTime=" + StartTime.Text + "&";
            URL += "EndTime=" + EndTime.Text;
            ResponseHelper.Redirect(URL);
        }
    }
}