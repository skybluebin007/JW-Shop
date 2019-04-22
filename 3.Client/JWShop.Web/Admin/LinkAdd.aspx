<%@ Page Language="C#" Debug="true" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="LinkAdd.aspx.cs" Inherits="JWShop.Web.Admin.LinkAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">    
<div class="container ease" id="container">
	<!--<div class="path-title"></div>-->
    <div class="product-container product-container-border product-container-mt70"> 
	    <div class="form-row">
		    <div class="head">文字：</div>
		   	<SkyCES:TextBox ID="TextDisplay" CssClass="txt" runat="server"  Width="400px" />
	    </div>	
	   
	    <div class="form-row">
		    <div class="head">URL：</div>
		   	<SkyCES:TextBox ID="URL" CssClass="txt" runat="server"  Width="400px" />
	    </div>
	
	    <div class="form-row">
		    <div class="head">备注信息：</div>
		  <SkyCES:TextBox ID="Remark" CssClass="txt" runat="server" Width="400px" TextMode="MultiLine" Height="50px" />
	    </div>
    </div>
    <div class="form-foot">
		<asp:Button CssClass="form-submit ease" Style="margin: 0; position: static;" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
    </div>
</div>
</asp:Content>
