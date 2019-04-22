<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="FlashAdd.aspx.cs" Inherits="JWShop.Web.Admin.FlashAdd" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">    
<div class="container ease" id="container">
	<!--<div class="path-title"></div>-->
    <div class="product-container product-container-border product-container-mt70"> 
	    <div class="form-row">
		   <div class="head">标题：</div>
          <SkyCES:TextBox ID="Title" CssClass="txt" runat="server"  Width="400px" CanBeNull="必填"/>
	    </div>	
	   
	    <div class="form-row">
		  <div class="head">说明：</div>
          <SkyCES:TextBox ID="Introduce" CssClass="txt" runat="server"  Width="400px" TextMode="MultiLine" Height="50px" />
	    </div>
	
	    <div class="form-row">
		<div class="head">建议宽度(px)：</div>
         <SkyCES:TextBox ID="Width" CssClass="txt" Text="0" runat="server" Width="100px" CanBeNull="必填" RequiredFieldType="数据校验"/>
	    </div>
        <div class="form-row">
		<div class="head">建议高度(px)：</div>
         <SkyCES:TextBox ID="Height" CssClass="txt" Text="0" runat="server" Width="100px" CanBeNull="必填" RequiredFieldType="数据校验"/>
	    </div>
    </div>
    <div class="form-foot">
		<asp:Button CssClass="form-submit ease" Style="margin: 0; position: static;" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
    </div>
</div>
</asp:Content>
