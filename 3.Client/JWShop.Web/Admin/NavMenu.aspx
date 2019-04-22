<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="NavMenu.aspx.cs" Inherits="JWShop.Web.Admin.NavMenu" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <script>
        function ChangeOrder(id, oid) {
            $.get("Ajax.aspx", { action: "ChangeNavMenuOrder", id: id, oid: oid }, function (data) {
                //alert(data);
            })
        }
    </script>
    <div class="container ease" id="container">
        <div class="product-container" style="padding-top: 28px;">
            <dl class="classify-menu">
                <dd class="add" onclick="location.href='NavMenuAdd.aspx'">添加导航</dd>
                <%--<dd class="order">一键排序</dd>--%>
                <dd class="show" id="sub-showall">展开全部</dd>
                <dd class="hide" id="sub-hideall">收起全部</dd>
            </dl>
            <table cellpadding="0" cellspacing="0" border="0" class="product-list" width="100%">
                <thead>
                    <tr>
                        <%--<td width="50" height="40">选择</td>--%>
                        <td width="150">导航名称</td>
                        <td width="" align="left" style="padding-left: 20px;">排序</td>
                        <td width="100">是否显示</td>
                        <td width="100">链接地址</td>
                        <td width="250">操作</td>
                    </tr>
                </thead>
                <tbody>
                    <%
                        int topCount = 1;
                        foreach (NavMenuInfo navMenu in navMenuList)
                        { %>

                        <tr bind="column-<%=topCount%>">
                        <%--<td height="40"><label class="ig-checkbox"><input type="checkbox" value="" ig-bind="list" /></label></td>--%>
                            <td> <div class="title"><%=navMenu.Name%></div></td>
                        <td  align="left">
                            <div class="classify-column">                               <%-- <div class="sub sub-hide"></div>--%>
                               
                                <input type="text" placeholder="" value="<%=navMenu.OrderId%>" maxlength="4" class="order" onblur="ChangeOrder(<%=navMenu.Id %>,this.value)" />
                                <div class="clear"></div>
                            </div>
                        </td>
                        <td><%=ShopCommon.GetBoolText(navMenu.IsShow)%></td>
                            <td><%=navMenu.LinkUrl%></td>
                        <td>                            
                            <a href="NavMenuAdd.aspx?ID=<%= navMenu.Id%>" class="ig-colink" title="编辑">编辑</a>
                            |
                            <a href="NavMenu.aspx?Action=Delete&ID=<%= navMenu.Id%>" onclick="return check()" class="ig-colink" title="删除">删除</a>
                        </td>
                    </tr>
                    <%

                            topCount++;
                        } %>
                </tbody>
                <tfoot style="display: none;">
                    <tr>
                        <td colspan="3" align="left">
                            <div class="button">
                                <label class="ig-checkbox">
                                    <input type="checkbox" value="" class="checkall" bind="list" />全选</label>

                                <%--<asp:Button ID="Delete_Button" CssClass="button-2 del" runat="server" Text="Button" OnClick="Delete_Button_Click" />--%>
                            </div>
                            <div class="clear"></div>
                        </td>
                    </tr>
                </tfoot>
            </table>

        </div>
    </div>
</asp:Content>
