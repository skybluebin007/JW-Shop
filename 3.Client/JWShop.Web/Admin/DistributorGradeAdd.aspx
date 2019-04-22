<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="DistributorGradeAdd.aspx.cs" Inherits="JWShop.Web.Admin.DistributorGradeAdd" %>

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
                <SkyCES:TextBox ID="Title" CssClass="txt" runat="server" Width="" CanBeNull="必填" />
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">总佣金下限：</div>
                <SkyCES:TextBox ID="Min_Amount" CssClass="txt" runat="server" HintInfo="大于或者等于总佣金下限" CanBeNull="必填" RequiredFieldType="金额" Width="" />
                大于或者等于总佣金下限
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">总佣金上限：</div>
                <SkyCES:TextBox ID="Max_Amount" CssClass="txt" runat="server" HintInfo="小于总佣金上限" CanBeNull="必填" RequiredFieldType="金额" Width="" />
                小于总佣金上限
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">返佣比例：</div>
                <SkyCES:TextBox ID="Discount" CssClass="txt" runat="server" HintInfo="如写入10表示10%" CanBeNull="必填" RequiredFieldType="金额" Width="" MinimumValue="1" MaximumValue="10" />
                输入10表示10%，在1~10之间
            </div>
            <div class="clear"></div>
        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
        </div>
    </div>
</asp:Content>