<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="MenuAdd.aspx.cs" Inherits="JWShop.Web.Admin.MenuAdd"  %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Register Assembly="SkyCES.EntLib" Namespace="SkyCES.EntLib" TagPrefix="SkyCES" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="container ease" id="container">
        <div class="product-container">
	<div class="form-row">
		<div class="head">所属分类：</div>
		<asp:DropDownList ID="FatherID" runat="server" Width="300px"/>
	</div>
	<div class="form-row">
		<div class="head">菜单名称：</div>
		<SkyCES:TextBox  ID="MenuName" CssClass="input" Width="300px" CanBeNull="必填" runat="server" />
	</div>
	<div class="form-row">
		<div class="head">菜单图标：</div>
		<asp:RadioButtonList ID="MenuImage" runat="server" RepeatDirection="Horizontal" RepeatColumns="10"></asp:RadioButtonList>
	</div>
	<div class="form-row">
		<div class="head">链接地址：</div>
		<SkyCES:TextBox ID="URL"  CssClass="input" Width="300px" HintInfo="如果是外部地址，请在地址前带上Http://" CanBeNull="必填" runat="server" />
	</div>
	<div class="form-row">
		<div class="head">排序ID：</div>
		<SkyCES:TextBox ID="OrderID" CssClass="input" runat="server" Width="300px" CanBeNull="必填" RequiredFieldType="数据校验"   HintInfo="数字越小越排前"/>
	</div>
<div class="action">
    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
</div>
            </div>
    </div>
</asp:Content>
