<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NotifyPay.aspx.cs" Inherits="JWShop.Pay.WxPay.NotifyPay" %>
<head>
 <%--signalr--%>
<script src="/admin/Js/jquery-1.7.2.min.js"></script>
        <script src="/admin/Scripts/jquery.signalR-1.2.2.min.js"></script>
     <script src="/Signalr/Hubs"></script>  
        <script>
        $.connection.hub.logging = true;
        $.connection.hub.start();
    </script>
     <%--signalr--%>
    </head>