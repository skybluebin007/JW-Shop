using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SkyCES.EntLib;
using JWShop.Business;
using JWShop.Entity;
using JWShop.Common;


namespace JWShop.Pay.WxPay
{
    public partial class Refund : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Log.Info(this.GetType().ToString(), "page load");

            if (Page.IsPostBack) return;

            /*******************请求参数验证 start*****************************************************************/
            int orderRefundId = RequestHelper.GetQueryString<int>("order_refund_id");
            var orderRefund = OrderRefundBLL.Read(orderRefundId);
            if (orderRefund.Id < 1)
            {
                Response.Write("<p style=\"margin-left:130px\">无效的退款服务工单</p>");
                Response.End();
            }

            decimal refundMoney = orderRefund.RefundMoney;
            if (refundMoney <= 0)
            {
                Response.Write("<p style=\"margin-left:130px\">无效的退款金额，退款金额必须大于0</p>");
                Response.End();
            }

            if (orderRefund.Status == (int)OrderRefundStatus.HasReturn)
            {
                Response.Write("<p style=\"margin-left:130px\">无效的退款请求，该服务工单已处理退款</p>");
                Response.End();
            }
            if (orderRefund.Status != (int)OrderRefundStatus.Returning)
            {
                Response.Write("<p style=\"margin-left:130px\">无效的退款请求，请等待管理员审核</p>");
                Response.End();
            }

            var order = OrderBLL.Read(orderRefund.OrderId);
            if (order.Id < 1)
            {
                Response.Write("<p style=\"margin-left:130px\">无效的订单</p>");
                Response.End();
            }

            string tradeNo = order.WxPayTradeNo;
            if (string.IsNullOrEmpty(tradeNo))
            {
                Response.Write("<p style=\"margin-left:130px\">无效的微信支付交易号</p>");
                Response.End();
            }

            decimal totalMoney = order.ProductMoney - order.FavorableMoney + order.ShippingMoney + order.OtherMoney - order.Balance - order.CouponMoney - order.PointMoney;
            if (refundMoney > totalMoney)
            {
                Response.Write("<p style=\"margin-left:130px\">退款金额不能超过订单金额</p>");
                Response.End();
            }

            //商户退款单号
            //商户系统内部的退款单号，商户系统内部唯一，同一退款单号多次请求只退一笔
            string batch_no = orderRefund.BatchNo;
            if (string.IsNullOrEmpty(batch_no))
            {
                batch_no = DateTime.Now.ToString("yyyyMMddhhmmssfff");

                //记录退款批次号存入订单退款表
                OrderRefundBLL.UpdateBatchNo(orderRefundId, batch_no);
            }

            /*******************请求参数验证 end*****************************************************************/


            //订单总金额
            string total_fee = Convert.ToInt32(totalMoney * 100).ToString();
            //退款金额
            string refund_fee = Convert.ToInt32(refundMoney * 100).ToString();

            //申请退款
            /***
            * 申请退款完整业务流程逻辑
            * @param transaction_id 微信订单号（优先使用）
            * @param out_trade_no 商户订单号
            * @param out_refund_no 商户退款单号
            * @param total_fee 订单总金额
            * @param refund_fee 退款金额
            * @return 退款结果（xml格式）
            */
            string result = "";
            try
            {
                //result = RefundBusiness.Run(tradeNo, "", batch_no, total_fee, refund_fee);              

                //小程序支付的退款业务逻辑
                WxpayResult wxResult = JWShop.XcxApi.Pay.RefundBusiness.Run(tradeNo, "", batch_no, total_fee, refund_fee);
                //bool isSuccess = JWShop.XcxApi.Pay.RefundBusiness.Run(tradeNo, "", batch_no, total_fee, refund_fee);
                //测试，默认在线退款成功
                //bool isSuccess = true;
                string msg = "";
                //退款到余额
                //if (orderRefund.RefundBalance > 0)
                //{
                //    //在这里写自己的逻辑
                //}

                //if (isSuccess)
                if(wxResult.result_code)
                {
                    orderRefund.Remark = "退款完成";
                    #region 发送订单退款成功通知
                    /*
                    int isSendNotice = ShopConfig.ReadConfigInfo().RefundOrder;
                    int isSendEmail = ShopConfig.ReadConfigInfo().RefundOrderEmail;
                    int isSendMessage = ShopConfig.ReadConfigInfo().RefundOrderMsg;
                    string key = "RefundOrder";
                    OrderInfo theorder = OrderBLL.Read(orderRefund.OrderId);
                    if (isSendNotice == (int)BoolType.True && key != string.Empty)
                    {
                        if (isSendEmail == (int)BoolType.True)
                        {//发邮件
                            try
                            {
                                EmailContentInfo emailContent = EmailContentHelper.ReadSystemEmailContent(key);
                                EmailSendRecordInfo emailSendRecord = new EmailSendRecordInfo();
                                emailSendRecord.Title = emailContent.EmailTitle;
                                emailSendRecord.Content = emailContent.EmailContent.Replace("$UserName$", theorder.UserName).Replace("$OrderNumber$", theorder.OrderNumber).Replace("$PayName$", theorder.PayName).Replace("$ShippingName$", ShippingBLL.Read(theorder.ShippingId).Name).Replace("$ShippingNumber$", theorder.ShippingNumber).Replace("$ShippingDate$", theorder.ShippingDate.ToString("yyyy-MM-dd"));
                                emailSendRecord.IsSystem = (int)BoolType.True;
                                emailSendRecord.EmailList = theorder.Email;
                                emailSendRecord.IsStatisticsOpendEmail = (int)BoolType.False;
                                emailSendRecord.SendStatus = (int)SendStatus.No;
                                emailSendRecord.AddDate = RequestHelper.DateNow;
                                emailSendRecord.SendDate = RequestHelper.DateNow;
                                emailSendRecord.ID = EmailSendRecordBLL.AddEmailSendRecord(emailSendRecord);
                                EmailSendRecordBLL.SendEmail(emailSendRecord);
                                //result = "通知邮件已发送。";
                            }
                            catch
                            {
                                //result = "未发送通知邮件。";
                            }
                        }
                        if (isSendMessage == (int)BoolType.True)
                        {//发短信
                            //result += "未发送通知短信。";
                        }
                    }
                    */
                    #endregion

                    orderRefund.Status = (int)OrderRefundStatus.HasReturn;
                    orderRefund.TmRefund = DateTime.Now;
                    OrderRefundBLL.Update(orderRefund);

                    OrderRefundActionBLL.Add(new OrderRefundActionInfo
                    {
                        OrderRefundId = orderRefund.Id,
                        //Status = isSuccess ? 1 : 0,
                        Status = wxResult.result_code ? 1 : 0,
                        Tm = DateTime.Now,
                        UserType = 2,
                        UserId = 0,
                        UserName = "系统",
                        Remark = orderRefund.Remark
                    });
                    if (string.IsNullOrEmpty(RequestHelper.GetQueryString<string>("returnurl")))
                    {
                        Response.Redirect("/Admin/OrderRefundAdd.aspx?Id=" + orderRefundId);
                    }
                    else
                    {
                        Response.Redirect(RequestHelper.GetQueryString<string>("returnurl"));
                    }

                }
                else
                {
                    Response.Write("<p style=\"margin-left:130px\">" + orderRefund.RefundPayKey+ "退款失败," + wxResult.err_code_des+" </p>");
                    Response.End();
                    //orderRefund.Remark = orderRefund.RefundPayKey + " 已退款完成，余额退款失败，失败信息：" + msg + "，请线下处理。";
                }


            }
            catch
            {
                Response.Write("<p style=\"margin-left:130px\">退款出错, 请检查账户余额是否充足</p>");
                Response.End();
            }

            //Response.Write(result);
        }
    }
}