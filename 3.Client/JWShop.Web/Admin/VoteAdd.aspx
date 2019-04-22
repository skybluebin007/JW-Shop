<%@ Page Language="C#"  MasterPageFile="MasterPage.Master"  AutoEventWireup="true" CodeBehind="VoteAdd.aspx.cs" Inherits="JWShop.Web.Admin.VoteAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <div class="container ease" id="container">
    	<div class="path-title"></div>
        <div class="product-container product-container-border">
    	<div class="product-row">
            <div class="form-row">
                <div class="head">上级分类：</div>
		        <asp:DropDownList ID="FatherID" runat="server" CssClass="select" />
	        </div>                        
	         <div class="form-row">
                <div class="head">标题：</div>
                <SkyCES:TextBox ID="Title" CssClass="txt" runat="server" MaxLength="20" CanBeNull="必填"  placeholder="长度限制1-20个字符之间"  Width="400px" />
            </div> 
         
	        <div class="form-row">
		        <div class="head">排序号：</div>
		        <SkyCES:TextBox ID="OrderID" CssClass="txt" runat="server" Width="400px" Text="0" CanBeNull="必填" RequiredFieldType="数据校验"  />
	        </div>

        
	        <div class="form-row">
		        <div class="head">备注：</div>
		        <SkyCES:TextBox ID="Note" CssClass="txt" runat="server" Width="400px" Height="100px" TextMode="MultiLine" MaxLength="100" />
                 
	        </div>

        </div>
    </div>
    <div class="form-foot">
        <asp:Button CssClass="form-submit ease" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" />
    </div>
</div>
    
</asp:Content>	
