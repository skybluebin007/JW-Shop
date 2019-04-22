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
    public partial class OrderRefund : JWShop.Page.AdminBasePage
    {
        protected int intStatus = int.MinValue;
        protected List<EnumInfo> enumList = new List<EnumInfo>();
        protected OrderDetailInfo orderDetail = new OrderDetailInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAdminPower("ReadOrderRefund", PowerCheckType.Single);

            string action = RequestHelper.GetQueryString<string>("Action");
            if (!Page.IsPostBack)
            {
                enumList = EnumHelper.ReadEnumList<OrderRefundStatus>();
                Status.DataTextField = "ChineseName";
                Status.DataValueField = "Value";
                Status.DataSource = enumList;
                Status.DataBind();
                Status.Items.Insert(0, new ListItem { Text = "所有状态", Value = int.MinValue.ToString() });

                RefundNumber.Text = RequestHelper.GetQueryString<string>("RefundNumber");
                OrderNumber.Text = RequestHelper.GetQueryString<string>("OrderNumber");
                Status.Text = RequestHelper.GetQueryString<string>("Status");
                StartAddDate.Text = RequestHelper.GetQueryString<string>("StartAddDate");
                EndAddDate.Text = RequestHelper.GetQueryString<string>("EndAddDate");

                OrderRefundSearchInfo searchInfo = new OrderRefundSearchInfo();
                searchInfo.RefundNumber = RequestHelper.GetQueryString<string>("RefundNumber");
                searchInfo.OrderNumber = RequestHelper.GetQueryString<string>("OrderNumber");
                searchInfo.Status = RequestHelper.GetQueryString<int>("Status");
                searchInfo.StartTmCreate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
                searchInfo.EndTmCreate = RequestHelper.GetQueryString<DateTime>("EndAddDate");

                var orderRefundList = OrderRefundBLL.SearchList(CurrentPage, PageSize, searchInfo, ref Count);

                BindControl(orderRefundList, RecordList, MyPager);
                intStatus = searchInfo.Status;
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "OrderRefund.aspx?Action=search&";
            URL += "RefundNumber=" + RefundNumber.Text + "&";
            URL += "OrderNumber=" + OrderNumber.Text + "&";
            URL += "Status=" + Status.Text + "&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text;
            ResponseHelper.Redirect(URL);
        }

    }
}