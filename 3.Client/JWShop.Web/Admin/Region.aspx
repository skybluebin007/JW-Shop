<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="Region.aspx.cs" Inherits="JWShop.Web.Admin.Region" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>地区设置</div>
<div class="add">
    <ul>
        <li class="left">增加地区：</li>
        <li class="right"><asp:TextBox ID="RegionName" CssClass="input" runat="server" Width="400px" /> <input onclick="addRegion()" type="button" class="button" value=" 添 加 " />  </li>
    </ul>
    <div id="Region-Ajax"></div>
</div>
<script language="javascript" type="text/javascript" src="/Admin/Js/Region.js"></script>
<script language="javascript" type="text/javascript" src="/Admin/Js/Edit.js"></script>
<script language="javascript" type="text/javascript">readRegion(0);</script>
</asp:Content>