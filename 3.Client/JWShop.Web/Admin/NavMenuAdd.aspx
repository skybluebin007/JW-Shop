<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MasterPage.Master" CodeBehind="NavMenuAdd.aspx.cs" Inherits="JWShop.Web.Admin.NavMenuAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>

<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
    	<!--<div class="path-title"></div>-->
        <div class="product-container product-container-border product-container-mt70">
    	<div class="product-row">                                  
	         <div class="form-row">
                <div class="head">分类名称：</div>
                <SkyCES:TextBox ID="Name" CssClass="txt" runat="server" MaxLength="60" CanBeNull="必填"  placeholder="长度限制1-60个字符之间"/>
            </div>                     
	        <div class="form-row">
		        <div class="head">排序ID：</div>
		        <SkyCES:TextBox ID="OrderID" CssClass="txt" runat="server" Width="300px" CanBeNull="必填" RequiredFieldType="数据校验"  />
	        </div>

            <div class="form-row">
		        <div class="head">链接地址：</div>
		        <SkyCES:TextBox ID="LinkUrl" CssClass="txt" runat="server" Width="400px" />
	        </div>	      
           <div class="form-row">
                <div class="head">是否显示：</div>
                <asp:CheckBox runat="server" ID="IsShow"></asp:CheckBox>                
            </div>
	        <div class="form-row">
		        <div class="head">描述：</div>
		        <SkyCES:TextBox ID="Introduce" CssClass="txt" runat="server" Width="400px" Height="100px" TextMode="MultiLine" />
	        </div>
            
        </div>
    </div>
    <div class="form-foot">
        <asp:Button CssClass="form-submit ease" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" />
    </div>
</div>
</asp:Content>	
