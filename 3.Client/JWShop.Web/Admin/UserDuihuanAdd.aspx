<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="UserDuihuanAdd.aspx.cs" Inherits="JWShop.Web.Admin.UserDuihuanAdd" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <link rel="stylesheet" href="/Admin/Js/jqdate/base/jquery.ui.all.css">    
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.core.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <link rel="stylesheet" href="/Admin/Js/jqdate/demos.css">
        <style>
 .form-row table {
            box-shadow: none;
        }
 .form-row table td {
                border: none;
            }
    </style>
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_Birthday").datepicker({ changeMonth: true, changeYear: true });
         
        });
    </script>
<script language="javascript" type="text/javascript" src="/Admin/js/UnlimitClass.js"></script>
<div class="container ease" id="container">
    	<div class="path-title"></div>
        <div class="product-container product-container-border">
            <div class="form-row">
                <div class="head">用户ID：</div>
                <SkyCES:TextBox ID="userid" CssClass="txt" runat="server" onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this)" />
                &nbsp;<font color="red">*需要兑换商品的用户ID</font>
            </div>

            <div class="form-row" >
                <div class="head">姓名：</div>
                <SkyCES:TextBox ID="name" CssClass="txt" runat="server" Width="300px" />
            </div>
            <div class="form-row" >
                <div class="head">手机号码：</div>
                <SkyCES:TextBox ID="mobile" CssClass="txt" runat="server" Width="300px" />
            </div>
            <div class="form-row" >
                <div class="head">使用积分：</div>
                <SkyCES:TextBox ID="integral" CssClass="txt" runat="server" Text="1" onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this)" Width="300px" />
                &nbsp;<font color="red">*兑换商品所用积分</font>
            </div>
            <div class="form-row">
                <div class="head">备注：</div>
                <SkyCES:TextBox ID="note" CssClass="txt" runat="server" Width="300px" TextMode="MultiLine" Height="80px" />
                &nbsp;<font color="red">*兑换了什么商品,可在此备注记录</font>
            </div>

</div>
    <div class="form-foot">
        <asp:Button CssClass="form-submit ease" Style="margin: 0;" ID="SubmitButton" Text=" 确定兑换 " runat="server" OnClick="SubmitButton_Click" />
     <input type="button"  value="返回列表" class="form-submit ease"  onclick="location.href='UserDuihuan.aspx'" style="background: #cecece;width: 110px;"/>
        </div>
    </div>
</asp:Content>
