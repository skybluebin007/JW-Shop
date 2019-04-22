<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" Inherits="JWShop.Web.Admin.PasswordAdd" %>
<%@ Register Assembly="SkyCES.EntLib" Namespace="SkyCES.EntLib" TagPrefix="SkyCES" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
    	<div class="path-title"></div>
        <div class="product-container product-container-border">
            <div class="form-row">
                <div class="head">登陆名：</div>
                <SkyCES:TextBox CssClass="txt" Width="300px" ID="Name" runat="server" Enabled="false" />
            </div>
            <div class="clear"></div>
            <asp:PlaceHolder ID="Add" runat="server">
                <div class="form-row">
                    <div class="head">新密码：</div>
                    <SkyCES:TextBox CssClass="txt" Width="300px" ID="NewPassword" runat="server" CanBeNull="必填" RequiredFieldType="自定义验证表达式" ValidationExpression="^[\W\w]{6,16}$" CustomErr="密码长度大于6位少于16位" TextMode="Password" />
                </div>
                <div class="clear"></div>
                <div class="form-row">
                    <div class="head">重复密码：</div>
                    <SkyCES:TextBox CssClass="txt" Width="300px" ID="NewPassword2" runat="server" CanBeNull="必填" RequiredFieldType="自定义验证表达式" ValidationExpression="^[\W\w]{6,16}$" CustomErr="密码长度大于6位少于16位" TextMode="Password" />
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="两次密码不一致" ControlToCompare="NewPassword" ControlToValidate="NewPassword2" Display="Dynamic"></asp:CompareValidator>
                </div>
                <div class="clear"></div>
            </asp:PlaceHolder>
            
            </div>
            <div class="form-foot">
            	<asp:Button CssClass="form-submit ease" ID="Button1" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
              
            <div class="clear"></div>
        </div>
    </div>
</asp:Content>
