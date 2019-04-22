<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ArticleClass.aspx.cs" Inherits="JWShop.Web.Admin.ArticleClass" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
        <script>
            function ChangeOrder(id, oid) {
                $.get("Ajax.aspx", { action: "ChangeArticleClassOrder", id: id, oid: oid }, function (data) {
                    //alert(data);
                })
            }
    </script>
    <div class="container ease" id="container">
    	<div class="product-container" style="padding-top: 24px;">	
            <div class="add-button"><a href="ArticleClassAdd.aspx" title="添加新数据" class="ease"> 添 加 </a></div>
            <table cellpadding="0" cellspacing="0" border="0" class="product-list" width="100%">
                <thead>
                    <tr>
                        <td width="50" height="40">Id</td>
                        <td width="150">分类名称</td>
                        <td width="" align="left" style="padding-left: 20px;">排序</td>
                        <td width="100">是否系统</td>
                        <td width="250">操作</td>
                    </tr>
                </thead>
                <tbody>
                    <%
                        int topCount = 1;
                        foreach (ArticleClassInfo topClass in topClassList)
                        { %>

                        <tr bind="column-<%=topCount%>">
                  
                        <td colspan="3" align="left">
                            <div class="classify-column">
                                <div class="sub sub-hide"></div>
                                <div class="title"><%=topClass.Id%>&nbsp;&nbsp;&nbsp;&nbsp;<%=topClass.Name%></div>
                                <input type="text" placeholder="" value="<%=topClass.OrderId%>" maxlength="4" class="order" onblur="ChangeOrder(<%=topClass.Id %>,this.value)" />
                                <div class="clear"></div>
                            </div>
                        </td>
                        <td><%= ShopCommon.GetBoolText(topClass.IsSystem)%></td>
                        <td>
                            <a href="ArticleClassAdd.aspx?FatherID=<%= topClass.Id%>" class="ig-colink" title="新增下级">新增下级</a>|
                            <a href="ArticleClassAdd.aspx?ID=<%=topClass.Id%>" class="ig-colink">修改</a>| 
			                <a href='ArticleClass.aspx?Action=Delete&ID=<%=topClass.Id%>' onclick="return check()" class="ig-colink">删除</a>
                        </td>
                    </tr>
                    <%
                            List<ArticleClassInfo> subClassList = ArticleClassBLL.ReadChilds(Convert.ToInt32(topClass.Id));
                            foreach (ArticleClassInfo subClass in subClassList)
                            {
                    %>
                            <tr ig-bind="column-<%=topCount%>">                          
                                <td colspan="3" align="left">
                                    <div class="classify-column">
                                        <div class="sub"></div>
                                        <div class="title subtitle"><%=subClass.Id%>&nbsp;&nbsp;&nbsp;&nbsp;<%=subClass.Name%></div>
                                        <input type="text" placeholder="" value="<%=subClass.OrderId%>" maxlength="4" class="order" onblur="ChangeOrder(<%= subClass.Id%>,this.value)" />
                                        <div class="clear"></div>
                                    </div>
                                </td>
                                <td><%= ShopCommon.GetBoolText(subClass.IsSystem)%></td>
                                <td>
                                    <a href="ArticleClassAdd.aspx?FatherID=<%= subClass.Id%>" class="ig-colink" title="新增下级">新增下级</a>|
                                    <a href="ArticleClassAdd.aspx?ID=<%=subClass.Id%>" class="ig-colink">修改</a>| 
			                        <a href='ArticleClass.aspx?Action=Delete&ID=<%=subClass.Id%>' onclick="return check()" class="ig-colink">删除</a>
                                   
                                </td>
                            </tr>
                    <%
                                List<ArticleClassInfo> thirdClassList = ArticleClassBLL.ReadChilds(Convert.ToInt32(subClass.Id));
                                foreach (ArticleClassInfo thirdClass in thirdClassList)
                                {
                    %>
                                <tr ig-bind="column-<%=topCount%>">                                
                                    <td colspan="3" align="left">
                                        <div class="classify-column">
                                            <div class="sub"></div>
                                            <div class="title subtitle" style="margin-left: 90px;"><%=thirdClass.Id%>&nbsp;&nbsp;&nbsp;&nbsp;<%=thirdClass.Name%></div>
                                            <input type="text" placeholder="" value="<%=thirdClass.OrderId%>" maxlength="4" class="order" onblur="ChangeOrder(<%= thirdClass.Id%>,this.value)" />
                                            <div class="clear"></div>
                                        </div>
                                    </td>
                                    <td><%= ShopCommon.GetBoolText(thirdClass.IsSystem)%></td>
                                    <td><a href="ArticleClassAdd.aspx?ID=<%=thirdClass.Id%>" class="ig-colink">修改</a>| 
			                        <a href='ArticleClass.aspx?Action=Delete&ID=<%=thirdClass.Id%>' onclick="return check()" class="ig-colink">删除</a>
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
                        <td colspan="5" align="left">
                         
                            <div class="clear"></div>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</asp:Content>
