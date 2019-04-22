<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" Inherits="JWShop.Web.Admin.UserPasswordAdd" %>
<%@ Register Assembly="SkyCES.EntLib" Namespace="SkyCES.EntLib" TagPrefix="SkyCES" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="container ease" id="container">
	<div class="path-title"></div>
	<div class="product-container product-container-border">
        <div class="add">
            <ul class="form-row">
                <li class="head">登陆名：</li>
                <li class=""><SkyCES:TextBox CssClass="txt" Width="" ID="Name" runat="server" Enabled="false" /></li>
            </ul>
            <ul class="form-row">
                <li class="head">新密码：</li>
                <li class=""><SkyCES:TextBox CssClass="txt" Width="" ID="NewPassword" runat="server" CanBeNull="必填" RequiredFieldType="自定义验证表达式" ValidationExpression="^[\W\w]{6,16}$" CustomErr="密码长度大于6位少于16位"  TextMode="Password"/></li>
            </ul>
            <ul class="form-row">
                <li class="head">重复密码：</li>
                <li class=""><SkyCES:TextBox CssClass="txt" Width="" ID="NewPassword2" runat="server" CanBeNull="必填" RequiredFieldType="自定义验证表达式" ValidationExpression="^[\W\w]{6,16}$" CustomErr="密码长度大于6位少于16位" TextMode="Password"/>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="两次密码不一致" ControlToCompare="NewPassword" ControlToValidate="NewPassword2" Display="Dynamic"></asp:CompareValidator>
                </li>
            </ul>
        </div>
    </div>
    <div class="form-foot">
        <asp:Button CssClass="form-submit" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" />
          <input type="button"  value="返回用户列表" class="form-submit ease"  onclick="window.location.href = 'User.aspx'" style="background: #cecece;width: 110px;"/>
    </div>
</div>
</asp:Content>
