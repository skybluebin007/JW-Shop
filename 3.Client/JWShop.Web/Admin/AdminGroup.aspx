<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="AdminGroup.aspx.cs" Inherits="JWShop.Web.Admin.AdminGroup" %>

<%@ Register Assembly="SkyCES.EntLib" Namespace="SkyCES.EntLib" TagPrefix="SkyCES" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
        <div class="product-container" style="padding-top: 24px;">
            <div class="add-button"><a href="AdminGroupAdd.aspx" title="添加新数据" class="ease"> 添 加 </a></div>
            <table class="product-list">
                <thead>
                    <tr>
                        <td style="width: 5%">选择</td>
                        <td style="width: 10%">Id</td>
                        <td style="width: 50%">管理组名</td>
                        <td style="width: 25%">下属管理员</td>
                        <td style="width: 10%">管理</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%#Eval("Id").ToString()!="1"?"<label class=\"ig-checkbox\"><input type=\"checkbox\" name=\"SelectID\" value=\""+ Eval("Id")+"\" ig-bind=\"list\" /></label>":""%></td>
                                <td><%# Eval("Id") %></td>
                                <td><%# Eval("Name") %></td>
                                <td><a href="Admin.aspx?GroupID=<%# Eval("Id") %>"><b style="color: Red"><%# Eval("AdminCount") %></b> 个</a></td>
                                <td class="link"><a href="AdminGroupAdd.aspx?ID=<%# Eval("Id") %>">修改</a></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="5">
                            <div class="button">
                                <label class="ig-checkbox"><input type="checkbox" value="" class="checkall" bind="list" />全选</label>
                                <asp:Button CssClass="button-2 del" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server" OnClick="DeleteButton_Click" />&nbsp;                              
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