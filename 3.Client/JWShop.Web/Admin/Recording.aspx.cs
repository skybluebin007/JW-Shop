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
    public partial class Recording : JWShop.Page.AdminBasePage
    {
        protected BargainInfo bargain = new BargainInfo();
        protected BargainOrderInfo _bargainOrder = new BargainOrderInfo();
        protected List<RecordingInfo> recordingList = new List<RecordingInfo>();
        protected List<int> b_orderids = new List<int>();
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadRecording", PowerCheckType.Single);

                //var bargain = BargainOrderBLL.ReadBargainOrder(RequestHelper.GetQueryString<int>("ID"));
                bargain = BargainBLL.ReadBargain(RequestHelper.GetQueryString<int>("ID"));
                var BargainDetailList = BargainDetailsBLL.ReadByBargainId(bargain.Id);
                foreach (var item in BargainDetailList)
                {
                    var product = ProductBLL.Read(item.ProductID);
                    if (product.Id > 0 && product.IsDelete == 0)
                    {
                        BargainDetail.Items.Add(new ListItem(product.Name, item.Id.ToString()));
                    }
                }
                BargainDetail.Items.Insert(0, new ListItem { Text = "请选择", Value = "" });
                //BargainDetail.SelectedIndexChanged += new System.EventHandler(BargainDetail_SelectedIndexChanged);
                if (!string.IsNullOrEmpty(BargainDetail.SelectedValue))
                {
                    foreach (var item in BargainOrderBLL.SearchBargainOrderList(new BargainOrderSearch() { BargainDetailsId = int.Parse(BargainDetail.SelectedValue) }))
                    {
                        var user = UserBLL.Read(item.UserId);
                        user.UserName = System.Web.HttpUtility.UrlDecode(user.UserName, System.Text.Encoding.UTF8);
                        BargainOrder.Items.Add(new ListItem(user.UserName, item.Id.ToString()));
                    }

                }
                BargainOrder.Items.Insert(0, new ListItem { Text = "请选择", Value = "" });
                #region MyRegion
                var bargainDetails = BargainDetailsBLL.ReadByBargainId(bargain.Id);
                foreach (var bdt in bargainDetails)
                {
                    var bargain_orders = BargainOrderBLL.SearchBargainOrderList(new BargainOrderSearch { BargainDetailsId = bdt.Id });
                    b_orderids = b_orderids.Concat(bargain_orders.Select(k => k.Id)).ToList();
                }
                #endregion
                BindData();

            }
        }

        protected void BargainOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void BargainDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            BargainOrder.Items.Clear();
            if (!string.IsNullOrWhiteSpace(BargainDetail.SelectedValue))
            {
                foreach (var item in BargainOrderBLL.SearchBargainOrderList(new BargainOrderSearch() { BargainDetailsId = int.Parse(BargainDetail.SelectedValue) }))
                {
                    var user = UserBLL.Read(item.UserId);
                    user.UserName = System.Web.HttpUtility.UrlDecode(user.UserName, System.Text.Encoding.UTF8);
                    BargainOrder.Items.Add(new ListItem(user.UserName, item.Id.ToString()));
                }

                #region MyRegion

                var bargain_orders = BargainOrderBLL.SearchBargainOrderList(new BargainOrderSearch { BargainDetailsId = int.Parse(BargainDetail.SelectedValue) });
                b_orderids = b_orderids.Concat(bargain_orders.Select(k => k.Id)).ToList();

                #endregion
            }
            else
            {
                #region MyRegion
                var bargainDetails = BargainDetailsBLL.ReadByBargainId(RequestHelper.GetQueryString<int>("ID"));
                foreach (var bdt in bargainDetails)
                {
                    var bargain_orders = BargainOrderBLL.SearchBargainOrderList(new BargainOrderSearch { BargainDetailsId = bdt.Id });
                    b_orderids = b_orderids.Concat(bargain_orders.Select(k => k.Id)).ToList();
                }
                #endregion
            }
            BargainOrder.Items.Insert(0, new ListItem { Text = "请选择", Value = "" });
            BindData();
        }

        private void BindData()
        {
            if (string.IsNullOrWhiteSpace(BargainOrder.SelectedValue))
            {
                // 默认查看此砍价所有的砍记录
                if (b_orderids.Count > 0)
                {
                    string inBOrderId = string.Join(",", b_orderids);
                    recordingList = RecordingBLL.SearchRecordingList(new RecordingSearch() { InBOrderId = inBOrderId });
                }
                else
                {
                    //recordingList = RecordingBLL.SearchRecordingList(new RecordingSearch() { });
                }
            }
            else
            {
                recordingList = RecordingBLL.SearchRecordingList(new RecordingSearch() { BOrderId = int.Parse(BargainOrder.SelectedValue) });
            }
            recordingList = recordingList.Where(k => k.UserId > 0).ToList();
            recordingList.ForEach(k => k.UserName = System.Web.HttpUtility.UrlDecode(k.UserName, System.Text.Encoding.UTF8));
            recordingList = recordingList.OrderBy(k => k.AddDate).ToList();
            BindControl(recordingList, RecordList);
            if (!string.IsNullOrWhiteSpace(BargainOrder.SelectedValue))
            {
                _bargainOrder = BargainOrderBLL.ReadBargainOrder(int.Parse(BargainOrder.SelectedValue));
                CheckStatus(_bargainOrder.Id);
            }
        }
        /// <summary>
        /// 判断是否发起砍价人
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string ShowLeader(object id)
        {
            if (id == null)
            {
                return string.Empty;
            }
            else
            {
                int _id = 0;
                int.TryParse(id.ToString(), out _id);
                if (!string.IsNullOrWhiteSpace(BargainOrder.SelectedValue))
                {
                    _bargainOrder = BargainOrderBLL.ReadBargainOrder(int.Parse(BargainOrder.SelectedValue));
                    CheckStatus(_bargainOrder.Id);
                }
                if (_id > 0 && _bargainOrder.UserId == _id)
                {
                    string result = "<span class=\"red\">【发起砍价】</span>";
                    return result;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 检查砍价订单状态
        /// </summary>
        /// <param name="bargainOrderId"></param>
        /// <returns></returns>
        private void CheckStatus(int bargainOrderId)
        {

            //new TxtLog(Server.MapPath("/apilog/")).Write("-----bargainOrderId:"+ bargainOrderId+"----");
            var bargainOrder = BargainOrderBLL.ReadBargainOrder(bargainOrderId);
            //new TxtLog(Server.MapPath("/apilog/")).Write("-----BargainOrderType:" + bargainOrder.Status + "----");
            if (bargainOrder.Id > 0 && bargainOrder.Status == (int)BargainOrderType.进行中)
            {
                var bargainDetail = BargainDetailsBLL.ReadBargainDetails(bargainOrder.BargainDetailsId);
                var bargain = BargainBLL.ReadBargain(bargainDetail.BargainId);
                if (DateTime.Now >= bargain.EndDate)
                {//已经结束,砍价失败
                    bargainOrder.Status = (int)BargainOrderType.砍价失败;
                    BargainOrderBLL.UpdateBargainOrder(bargainOrder);

                }
                var product = ProductBLL.Read(bargainDetail.ProductID);
                if (product.MarketPrice - bargainOrder.BargainPrice == bargainDetail.ReservePrice && bargainOrder.Status != (int)BargainOrderType.砍价成功)
                {
                    bargainOrder.Status = (int)BargainOrderType.砍价成功;
                    //BargainOrderBLL.UpdateBargainOrder(bargainOrder);
                    if (BargainOrderBLL.UpdateBargainOrder(bargainOrder))
                    {
                        //发送砍价成功模板消息
                        //SendBargainMessage(bargainOrder);
                        //new TxtLog(Server.MapPath("/apilog/")).Write("-----发送砍价成功模板消息:End----");
                    }
                }
                else if (DateTime.Now >= bargain.EndDate && bargainOrder.Status != (int)BargainOrderType.砍价失败)
                {
                    bargainOrder.Status = (int)BargainOrderType.砍价失败;
                    BargainOrderBLL.UpdateBargainOrder(bargainOrder);
                }
                else
                {//进行中

                }
            }


        }
    }
}