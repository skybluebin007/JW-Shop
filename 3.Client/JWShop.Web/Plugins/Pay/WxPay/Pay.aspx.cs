using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Linq;
using System.Collections;

namespace JWShop.Pay.WxPay
{
    public partial class Pay : System.Web.UI.Page
    {
        //H5调起JS API参数
        public string wxJsApiParam { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            Log.Info(this.GetType().ToString(), "page load");

            /*******************请求参数验证 start*****************************************************************/
            string orderIds = RequestHelper.GetQueryString<string>("order_id");

            if (string.IsNullOrEmpty(orderIds))
            {
                Response.Write("无效的请求");
                Response.End();
            }

            List<OrderInfo> orders = new List<OrderInfo>();
            try
            {
                int[] ids = Array.ConvertAll<string, int>(orderIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
                orders = OrderBLL.ReadList(ids);

                if (ids.Length != orders.Count)
                {
                    Response.Write("包含无效的订单");
                    Response.End();
                }
            }
            catch
            {
                Response.Write("无效的请求");
                Response.End();
            }
            /*******************请求参数验证 end*****************************************************************/


            JsApiPay jsApiPay = new JsApiPay(this);
            try
            {
                //调用【网页授权获取用户信息】接口获取用户的openid和access_token
                jsApiPay.GetOpenidAndAccessToken(orderIds);
            }
            catch (Exception ex)
            {
                Response.Write("页面加载出错，请重试：" + ex.ToString() + "</span>");
                return;
            }

            //付款金额
            decimal total_price = 0;
            foreach (var order in orders)
            {
                total_price += order.ProductMoney - order.FavorableMoney + order.ShippingMoney + order.OtherMoney - order.Balance - order.CouponMoney - order.PointMoney;
            }
            //支付金额
            //单位是分，不能有小数 
            jsApiPay.total_fee = Convert.ToInt32(total_price * 100);

            //附加数据
            //可能有多个订单合并付款，用逗号分隔
            jsApiPay.attach = string.Join(",", orders.Select(k => k.Id));

            //检测是否给当前页面传递了相关参数
            if (string.IsNullOrEmpty(jsApiPay.openid) || jsApiPay.total_fee <= 0)
            {
                Response.Write("页面传参出错,请返回重试");
                Log.Error(this.GetType().ToString(), "This page have not get params, cannot be inited, exit...");

                return;
            }

            jsApiPay.order_body = string.Join(",", orders.Select(k => k.OrderNumber));

            //JSAPI支付预处理
            try
            {
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);

                //在页面上显示订单信息
                //  Response.Write("<span style='color:#00CD00;font-size:20px'>订单详情：</span><br/>");
                //  Response.Write("<span style='color:#00CD00;font-size:20px'>" + unifiedOrderResult.ToPrintStr() + "</span>");

            }
            catch (Exception ex)
            {
                Response.Write("下单失败，请返回重试");

                //Response.Write("<span style='color:#FF0000;font-size:20px'>" + "下单失败，请返回重试\\订单ID：+" +  ViewState["OrderID"].ToString()+"\\OPID:"+ViewState["openid"].ToString()+"</span><br/>");
                // Response.Write("<span style='color:#FF0000;font-size:20px'>" + ex.ToString());
            }
        }
    }
}