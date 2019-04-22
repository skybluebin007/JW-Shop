<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="AdminAdd.aspx.cs" Inherits="JWShop.Web.Admin.AdminAdd" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
        <style>
 .form-row table {
            box-shadow: none;
        }
 .form-row table td {
                border: none;
            }
    </style>
    <div class="container ease" id="container">
    	<div class="path-title"></div>
        <div class="product-container product-container-border">
            <div class="form-row">
                <div class="head">管理组：</div>
                <asp:DropDownList Width="300px" ID="GroupID" CssClass="select" runat="server" />
            </div>
            <div class="form-row">
                <div class="head">管理员名：</div>
                <SkyCES:TextBox CssClass="txt" Width="300px" ID="Name" runat="server" CanBeNull="必填" />
            </div>
            <div class="form-row">
                <div class="head">电子邮箱：</div>
                <SkyCES:TextBox CssClass="txt" Width="300px" ID="Email" runat="server" RequiredFieldType="电子邮箱" CanBeNull="必填" />
             
            </div>
            <div class="form-row">
                <div class="head">备注：</div>
                <SkyCES:TextBox CssClass="txt" Width="300px" ID="NoteBook" runat="server" TextMode="MultiLine" Height="60px" />
            </div>
            <asp:PlaceHolder ID="AddStatus" runat="server" Visible="false">
                <% if(pageAdmin.Status == (int)BoolType.False && pageAdmin.LoginErrorTimes > 0) {%>
                    <div class="form-row">
                        <div class="head">登录失败次数：</div>
                        <%= pageAdmin.LoginErrorTimes %>
                    </div>
                <%} %>

                <div class="form-row">
                    <div class="head">状态：</div>
                    <asp:RadioButtonList ID="Status" runat="server" RepeatDirection="Horizontal"><asp:ListItem Value="1">正常</asp:ListItem><asp:ListItem Value="0" Selected="True">冻结</asp:ListItem></asp:RadioButtonList>
                </div>
            </asp:PlaceHolder>
            <div class="clear"></div>
            <asp:PlaceHolder ID="Add" runat="server">
                <div class="form-row">
                    <div class="head">密码：</div>
                    <SkyCES:TextBox CssClass="txt" Width="300px" ID="Password" runat="server" CanBeNull="必填" RequiredFieldType="自定义验证表达式" ValidationExpression="^[\W\w]{6,16}$" CustomErr="密码长度大于6位少于16位" TextMode="Password" />
                </div>
                <div class="form-row">
                    <div class="head">重复密码：</div>
                    <SkyCES:TextBox CssClass="txt" Width="300px" ID="Password2" runat="server" CanBeNull="必填" RequiredFieldType="自定义验证表达式" ValidationExpression="^[\W\w]{6,16}$" CustomErr="密码长度大于6位少于16位" TextMode="Password" />
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="两次密码不一致" ControlToCompare="Password" ControlToValidate="Password2" Display="Dynamic"></asp:CompareValidator>
                </div>
            </asp:PlaceHolder>
        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" Style="margin: 0;" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" />
        </div>
    </div>

</asp:Content>