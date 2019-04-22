<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pay.aspx.cs" Inherits="JWShop.Pay.WxPay.Pay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="content-type" content="text/html;charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/> 
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <title>微信支付</title>
    <style type="text/css">
        body { width: 100%; min-height: 100%; max-width: 640px; min-width: 320px; margin: 0 auto; font: .24rem/1.8 "Helvetica Neue", Helvetica, Arial, sans-serif; color: #333; background: #f5f5f5; -webkit-touch-callout: none; -webkit-user-select: none; -khtml-user-select: none; -moz-user-select: none; -ms-user-select: none; user-select: none; }
        .wrapper { width: 100%; background: none; box-shadow: 0 1px 2px rgba(0,0,0,0); padding: 0.8rem 0; }
        .wrapper .tips { padding: 0 0.8rem; font-size: 0.8rem; }
        .tips .title { font-size: 1rem; color: #f15353; font-weight: bold; margin-bottom: 0.3rem; }
        .tips .tel { color: #f15353; font-weight: bold; }
        .tips .go { margin-top: 1rem; }
        .tips .go a { padding-left: 0.3rem; color: #999; text-decoration: none; }
    </style>
</head>

<script type="text/javascript">
    //调用微信JS api 支付
    function jsApiCall()
    {
        WeixinJSBridge.invoke(
            'getBrandWCPayRequest',
            <%=wxJsApiParam%>,//josn串
            function (res)
            {
                WeixinJSBridge.log(res.err_msg);
                //alert(res.err_code + res.err_desc + res.err_msg);

                if (res.err_msg == "get_brand_wcpay_request:ok") {                      
                    //支付成功后的跳转页面 
                    window.location.href="/Plugins/Pay/WxPay/Return.aspx?order_id="+<%=HttpContext.Current.Request.QueryString["order_id"]%>;
                }
                else{
                    document.getElementById("payFail").style.display = "block";    
                }
            }
        );
    }

    function callpay()
    {
        if (typeof WeixinJSBridge == "undefined")
        {
            if (document.addEventListener)
            {
                document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
            }
            else if (document.attachEvent)
            {
                document.attachEvent('WeixinJSBridgeReady', jsApiCall);
                document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
            }
        }
        else
        {
            jsApiCall();
        }
    }
</script>

<body>
    <section class="wrapper" id="payFail" style="display: none;">
        <div class="tips">
            <div class="title">支付出现异常！</div>
            <div class="content">您可以查看微信交易记录。如果没有扣费，可以稍后尝试重新支付。如果已经扣费，请拨打客服电话：<span class="tel"><%=JWShop.Common.ShopConfig.ReadConfigInfo().Tel%></span> 联系我们。</div>
            <div class="go">您也可以返回商城查看<a href="/mobile/user/index.html">“我的订单”</a></div>
        </div>
    </section>

    <script>
        callpay();
    </script>
</body>
</html>