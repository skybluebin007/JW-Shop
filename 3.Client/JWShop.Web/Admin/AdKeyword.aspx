<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="AdKeyword.aspx.cs" Inherits="JWShop.Web.Admin.AdKeyword" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
        <div class="product-container" style="padding-top: 24px;">
            <div class="add-button"><a href="AdKeywordAdd.aspx" title="添加新数据" class="ease">添 加 </a></div>
            <table class="product-list">
                <thead>
                    <tr>
                        <td style="width: 10%">Id</td>
                        <td style="width: 30%; text-align: left; text-indent: 8px;">关键词</td>
                        <td style="width: 40%;">链接地址</td>
                        <td style="width: 10%">排序</td>
                        <td style="width: 10%">管理</td>
                    </tr>
                </thead>
                <asp:Repeater ID="RecordList" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("Id") %></td>
                            <td style="width: 30%; text-align: left; text-indent: 8px;"><%# StringHelper.Substring(Eval("Name").ToString(), 18) %></td>
                            <td><a href="<%# Eval("Url") %>" target="_blank"><%# StringHelper.Substring(Convert.ToString(Eval("Url")),30) %></a></td>
                            <td><%#Eval("OrderId") %></td>
                            <td class="link">
                                <a href="AdKeywordAdd.aspx?Id=<%# Eval("Id") %>">修改</a> | 
                                <a href='?Action=Delete&Id=<%# Eval("Id") %>' onclick="return check()">删除</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="listPage">
                <SkyCES:CommonPager ID="MyPager" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>