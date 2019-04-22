<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="OrderRefundToBalance.aspx.cs" Inherits="JWShop.Web.Admin.OrderRefundToBalance" %>

<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <b style="<%=(message == "退款完成" ? "color:#008000" : "color:#ED1B1B")%>"><%= message %></b>
</asp:Content>