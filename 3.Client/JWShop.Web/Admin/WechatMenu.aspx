<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MasterPage.Master"  CodeBehind="WechatMenu.aspx.cs" Inherits="JWShop.Web.Admin.WechatMenu" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <script>
        function CreateWechatMenu() {
            $.get("/weixin/wechatmenu.ashx", function (data) {
                if (data != "") {
                    var arr = data.split("|");
                    if (arr[0] == "ok") {
                        alert("微信菜单发布成功");
                    }
                    else {
                        console.log(data);
                        alert(arr[1]);
                }
                }
                else {
                    alert("系统忙，请稍后重试");
                }
            })
        }
    </script>
    <div class="container ease" id="container">
        <div class="product-container" style="padding-top: 28px;">
            <dl class="classify-menu">
                <dd class="add" onclick="location.href='WechatMenuAdd.aspx'">添加菜单</dd>          
            </dl>
            <table cellpadding="0" cellspacing="0" border="0" class="product-list" width="100%">
                <thead>
                    <tr>
                        <td width="20%">分类名称</td>
                        <td width="10%" align="left"  style="padding-left: 20px;">排序</td>
                        <td width="8%">类型</td>
                         <td width="42%">菜单值/URL</td>
                        <td width="20%">操作</td>
                     
                    </tr>
                </thead>
                <tbody>
                    <%
                        int topCount = 1;
                        foreach (WechatMenuInfo wechatmenu in topMenuList)
                        { %>

                        <tr bind="column-<%=topCount%>">                  
                        <td colspan="2" align="left">
                            <div class="classify-column">
                                <div class="sub sub-hide"></div>
                                <div class="title"><%=wechatmenu.Name%></div>                             
                                <input type="text" placeholder="" value="<%=wechatmenu.OrderId%>" maxlength="4" class="order"  disabled />
                                <div class="clear"></div>
                            </div>
                        </td>
                       <td><%=(wechatmenu.Type=="click")?"关键词":"链接" %></td>
                       <td><%=(wechatmenu.Type=="click")?wechatmenu.Key:wechatmenu.Url %></td>
                       <td>
                             <a href="wechatmenuadd.aspx?ID=<%= wechatmenu.Id%>" class="ig-colink" title="编辑">编辑</a>
                                    |<a href="wechatmenu.aspx?Action=Delete&ID=<%= wechatmenu.Id%>" onclick="return check()" class="ig-colink" title="删除">删除</a>
                           |<a href="?Action=Up&ID=<%= wechatmenu.Id%>" class="ig-colink">上移</a> 
                |<a href="?Action=Down&ID=<%= wechatmenu.Id%>" class="ig-colink">下移</a>
                       </td>
                    </tr>
                    <%
                            var subClassList = WechatMenuBLL.ReadChildList(Convert.ToInt32(wechatmenu.Id));
                            foreach (WechatMenuInfo subProductClass in subClassList)
                            {
                    %>
                            <tr ig-bind="column-<%=topCount%>">                         
                                <td colspan="2" align="left">
                                    <div class="classify-column">
                                        <div class="sub"></div>
                                        <div class="title subtitle"><%=subProductClass.Name%></div>                                       
                                        <input type="text" placeholder="" value="<%=subProductClass.OrderId%>" maxlength="4" class="order" disabled/>
                                        <div class="clear"></div>
                                    </div>
                                </td>
                                <td><%=(subProductClass.Type=="click")?"关键词":"链接" %></td>
                       <td><%=(subProductClass.Type=="click")?subProductClass.Key:subProductClass.Url %></td>
                                <td>
                                  
                                            <a href="wechatmenuadd.aspx?ID=<%= subProductClass.Id%>" class="ig-colink" >编辑</a>
                                    |<a href="wechatmenu.aspx?Action=Delete&ID=<%= subProductClass.Id%>" onclick="return check()" class="ig-colink" >删除</a>
                                    |<a href="?Action=Up&ID=<%= subProductClass.Id%>" class="ig-colink">上移</a> 
                |<a href="?Action=Down&ID=<%= subProductClass.Id%>" class="ig-colink">下移</a>
                                </td>
                            </tr>
             

                    <%                           
                            }

                            topCount++;
                        } %>
                </tbody>
                <tfoot>
                   
                </tfoot>
            </table>

            <dl class="classify-menu" style="margin-top: 20px;">
                <dd class="add" onclick="CreateWechatMenu();">发布微信菜单</dd>               
            </dl>         
          

        </div>
    </div>
</asp:Content>

