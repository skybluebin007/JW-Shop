﻿<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="DistributorGrade.aspx.cs" Inherits="JWShop.Web.Admin.DistributorGrade" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
        <div class="product-container" style="padding-top: 24px;">
        	<div class="add-button"><a href="DistributorGradeAdd.aspx" title="添加新数据" class="ease"> 添 加 </a></div>
            <table class="product-list">
                <thead>
                    <tr>
                        <td style="width: 5%">选择</td>
                        <td style="width: 5%">Id</td>
                        <td style="width: 40%">等级名称</td>
                        <td style="width: 15%">总佣金下限</td>
                        <td style="width: 15%">总佣金上限</td>
                        <td style="width: 15%">返佣比例</td>
                        <td style="width: 5%">管理</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("Id") %>" ig-bind="list" /></label></td>
                                <td><%# Eval("Id") %></td>
                                <td><%# Eval("Title") %></td>
                                <td><%# Eval("Min_Amount") %></td>
                                <td><%# Eval("Max_Amount") %></td>
                                <td><%# Eval("Discount") %></td>
                                <td class="link">
                                    <a href="DistributorGradeAdd.aspx?ID=<%#Eval("ID")%>">修改</a>
                                </td>                                
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="7">
                            <div class="button">
                                <label class="ig-checkbox"><input type="checkbox" value="" class="checkall" bind="list" />全选</label>
                                <asp:Button CssClass="button-2 del" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server" OnClick="DeleteButton_Click" />
                            </div>
                            <SkyCES:CommonPager ID="MyPager" runat="server" />
                            <div class="clear"></div>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</asp:Content>