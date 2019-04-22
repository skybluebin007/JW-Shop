<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductClass.aspx.cs" Inherits="JWShop.Web.Admin.ProductClass" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <script>
        function ChangeOrder(pid, oid) {
            $.get("Ajax.aspx", { action: "ChangeProClsOrder", pid: pid, oid: oid }, function (data) {
                //alert(data);
            })
        }
    </script>
    <div class="container ease" id="container">
        <div class="product-container" style="padding-top: 30px;">
            <dl class="classify-menu">
                <dd class="add" onclick="location.href='ProductClassAdd.aspx'">添加分类</dd>
                <%--<dd class="order">一键排序</dd>--%>
                <dd class="show" id="sub-showall">展开全部</dd>
                <dd class="hide" id="sub-hideall">收起全部</dd>
            </dl>
            <table cellpadding="0" cellspacing="0" border="0" class="product-list" width="100%">
                <thead>
                    <tr>
                        <%--<td width="50" height="40">选择</td>--%>
                        <td width="150">分类名称</td>
                        <td width="" align="left" style="padding-left: 20px;">排序</td>
                        <td width="100">商品数量</td>
                        <td width="250">操作</td>
                    </tr>
                </thead>
                <tbody>
                    <%
                        int topCount = 1;
                        foreach (ProductClassInfo productClass in topProductClassList)
                        { %>

                        <tr bind="column-<%=topCount%>">
                        <%--<td height="40"><label class="ig-checkbox"><input type="checkbox" value="" ig-bind="list" /></label></td>--%>
                        <td colspan="2" align="left">
                            <div class="classify-column">
                                <div class="sub sub-hide"></div>
                                <div class="title"><%=productClass.Name%></div>
                                <input type="text" placeholder="" value="<%=productClass.OrderId%>" maxlength="4" class="order" onblur="ChangeOrder(<%=productClass.Id %>,this.value)" />
                                <div class="clear"></div>
                            </div>
                        </td>
                        <td><a href="Product.aspx?Action=search&ClassId=|<%= productClass.Id%>|"><%= productClass.ProductCount%></a></td>
                        <td>
                          <%--  <a href="ProductClassAdd.aspx?FatherID=<%= productClass.Id%>" class="ig-colink" title="新增下级">新增下级</a>                            |--%>
                            <a href="ProductClassAdd.aspx?ID=<%= productClass.Id%>" class="ig-colink" title="编辑">编辑</a>
                            |
                            <a href="ProductClass.aspx?Action=Delete&ID=<%= productClass.Id%>" onclick="return check()" class="ig-colink" title="删除">删除</a>
                        </td>
                    </tr>
                    <%
                            List<ProductClassInfo> subClassList = ProductClassBLL.ReadChilds(Convert.ToInt32(productClass.Id));
                            foreach (ProductClassInfo subProductClass in subClassList)
                            {
                    %>
                            <tr ig-bind="column-<%=topCount%>">
                                <%--<td height="40"><label class="ig-checkbox"><input type="checkbox" value="" ig-bind="list" /></label></td>--%>
                                <td colspan="2" align="left">
                                    <div class="classify-column">
                                        <div class="sub"></div>
                                        <div class="title subtitle"><%=subProductClass.Name%></div>
                                        <input type="text" placeholder="" value="<%=subProductClass.OrderId%>" maxlength="4" class="order" onblur="ChangeOrder(<%= subProductClass.Id%>,this.value)" />
                                        <div class="clear"></div>
                                    </div>
                                </td>
                                <td><a href="Product.aspx?Action=search&ClassId=|<%= subProductClass.Id%>|"><%= subProductClass.ProductCount%></a></td>
                                <td>
                                   <%-- <a href="ProductClassAdd.aspx?FatherID=<%= subProductClass.Id%>" class="ig-colink" title="新增下级">新增下级</a>
                                    |--%>
                                            <a href="ProductClassAdd.aspx?ID=<%= subProductClass.Id%>" class="ig-colink" title="编辑">编辑</a>
                                    |
                                            <a href="ProductClass.aspx?Action=Delete&ID=<%= subProductClass.Id%>" onclick="return check()" class="ig-colink" title="删除">删除</a>
                                </td>
                            </tr>
                    <%
                                List<ProductClassInfo> thirdClassList = ProductClassBLL.ReadChilds(Convert.ToInt32(subProductClass.Id));
                                foreach (ProductClassInfo thirdProductClass in thirdClassList)
                                {
                    %>
                                <tr ig-bind="column-<%=topCount%>">
                                    <%--<td height="40"><label class="ig-checkbox"><input type="checkbox" value="" ig-bind="list" /></label></td>--%>
                                    <td colspan="2" align="left">
                                        <div class="classify-column">
                                            <div class="sub"></div>
                                            <div class="title subtitle" style="margin-left: 90px;"><%=thirdProductClass.Name%></div>
                                            <input type="text" placeholder="" value="<%=thirdProductClass.OrderId%>" maxlength="4" class="order" onblur="ChangeOrder(<%= thirdProductClass.Id%>,this.value)" />
                                            <div class="clear"></div>
                                        </div>
                                    </td>
                                    <td><a href="Product.aspx?Action=search&ClassId=|<%= thirdProductClass.Id%>|"><%= thirdProductClass.ProductCount%></a></td>
                                    <td><a href="ProductClassAdd.aspx?ID=<%= thirdProductClass.Id%>" class="ig-colink" title="编辑">编辑</a>
                                        |
                                                        <a href="ProductClass.aspx?Action=Delete&ID=<%= thirdProductClass.Id%>" onclick="return check()" class="ig-colink" title="删除">删除</a>
                                    </td>
                                </tr>

                    <%
                            }
                            }

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
