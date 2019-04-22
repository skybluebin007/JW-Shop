<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="UserGradeAdd.aspx.cs" Inherits="JWShop.Web.Admin.UserGradeAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>

<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
    	<div class="path-title"></div>
        <div class="product-container product-container-border">
            <div class="form-row">
                <div class="head">等级名称：</div>
                <SkyCES:TextBox ID="Name" CssClass="txt" runat="server" Width="" CanBeNull="必填" />
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">最低金额：</div>
                <SkyCES:TextBox ID="MinMoney" CssClass="txt" runat="server" HintInfo="大于或者等于最低金额" CanBeNull="必填" RequiredFieldType="金额" Width="" />
                大于或者等于最低金额
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">最高金额：</div>
                <SkyCES:TextBox ID="MaxMoney" CssClass="txt" runat="server" HintInfo="小于最高金额" CanBeNull="必填" RequiredFieldType="金额" Width="" />
                小于最高金额
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">折扣：</div>
                <SkyCES:TextBox ID="Discount" CssClass="txt" runat="server" HintInfo="如写入60表示6折" CanBeNull="必填" RequiredFieldType="金额" Width="" />
                输入90表示打9折，输入100表示不打折
            </div>
            <div class="clear"></div>
        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
        </div>
    </div>
</asp:Content>