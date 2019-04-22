<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Return.aspx.cs" Inherits="JWShop.Pay.WxPay.Return" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/> 
    <title>支付成功</title>
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
<body>    
    <section class="wrapper">
        <div class="tips">
            <div class="title">支付成功！</div>
            <div class="content">系统正在处理收款，感谢您的耐心等待。</div>
            <div class="go">返回商城查看<a href="/mobile/user/index.html">“我的订单”</a></div>
        </div>
    </section>
</body>
</html>