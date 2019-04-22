<%@ Page Language="C#" Debug="true" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="AdKeywordAdd.aspx.cs" Inherits="JWShop.Web.Admin.AdKeywordAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">    
    <div class="container ease" id="container">
	    <!--<div class="path-title"></div>-->
        <div class="product-container product-container-border product-container-mt70"> 
	        <div class="form-row">
		        <div class="head">关键词：</div>
		        <SkyCES:TextBox ID="Name" CssClass="txt" runat="server" Width="300px" CanBeNull="必填" />
	        </div>
	        <div class="form-row">
		        <div class="head">链接地址：</div>
		        <SkyCES:TextBox ID="Url" CssClass="txt" runat="server" Width="300px" />
	        </div>
	        <div class="form-row">
		        <div class="head">排序：</div>
		        <SkyCES:TextBox ID="OrderId" CssClass="txt" runat="server" Width="300px" CanBeNull="必填" RequiredFieldType="数据校验" />
	        </div>
        </div>
        <div class="form-foot">
		    <asp:Button CssClass="form-submit ease" Style="margin: 0; position: static;" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
        </div>
    </div>
</asp:Content>
