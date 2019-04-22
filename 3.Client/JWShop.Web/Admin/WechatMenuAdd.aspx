<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MasterPage.Master" CodeBehind="WechatMenuAdd.aspx.cs" Inherits="JWShop.Web.Admin.WechatMenuAdd" %>


<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <script type="text/javascript" src="/Admin/Js/ProductAdd.js"></script>

    <div class="container ease" id="container">
        <div class="path-title">
        </div>
        <div class="product-container product-container-border">
            <div class="form-row">
                <div class="head">父级菜单：</div>
                <asp:DropDownList ID="FatherID" runat="server" CssClass="select" />
            </div>
            <div class="form-row">
                <div class="head">菜单名称：</div>
                <SkyCES:TextBox ID="Name" CssClass="txt" runat="server" Width="300px" CanBeNull="必填" MaxLength="15"/>一级菜单不超过4个汉字，二级菜单不超过7个汉字
            </div>
            <div class="form-row">
                <div class="head">菜单类型：</div>
                <asp:DropDownList ID="MenuType" CssClass="select" runat="server">
                    <asp:ListItem Value="click">关键词</asp:ListItem>
                    <asp:ListItem Value="view">外链</asp:ListItem>               
                </asp:DropDownList>
            </div>        
            <div class="form-row">
                <div class="head">菜单值/Url：</div>
                <SkyCES:TextBox ID="MenuKey" CssClass="txt" runat="server" Width="300px" CanBeNull="必填" maxlenth="100"/>
            </div>         
            <div class="form-row">
                <div class="head">排序ID：</div>
                <SkyCES:TextBox ID="OrderID" CssClass="txt" runat="server" Width="300px" CanBeNull="必填"  RequiredFieldType="数据校验" HintInfo="数字越小越排前" />
            </div>
        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" Style="margin: 0;" ID="SubmitButton" Text=" 确 定 "   runat="server" OnClick="SubmitButton_Click" />
             <input type="button" class="form-submit ease" Style="margin: 0;width:120px;" id="tomenulist" value="返回菜单列表 "  onclick="window.location.href='wechatmenu.aspx'" />
        </div>
    </div>
</asp:Content>

