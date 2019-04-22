<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="Badwords.aspx.cs" Inherits="JWShop.Web.Admin.Badwords" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>

<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server"> 
    <div class="container ease" id="container">
        <div class="product-container product-container-border product-container-mt70">
            <div class="product-main">
                <div class="form-row">
                    <div class="head">敏感词：</div>
                    <SkyCES:TextBox CssClass="text" Width="600px" ID="txtBadwords" MaxLength="10000" runat="server" TextMode="MultiLine" Height="600px" />
                </div>
                <div class="clear"></div>
            </div>
        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" />
        </div>
    </div>
</asp:Content>
