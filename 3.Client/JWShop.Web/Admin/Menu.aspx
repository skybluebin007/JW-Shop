<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="JWShop.Web.Admin.Menu"  %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Register Assembly="SkyCES.EntLib" Namespace="SkyCES.EntLib" TagPrefix="SkyCES" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>菜单列表</div>

<div  class="listBlock">
    <ul>
       <%foreach(MenuInfo menu in MenuBLL.ReadMenuRootList()){ %>
        <li <%if(fatherID==menu.ID){%>class="listOn"<%} %> onclick="window.location='?FatherID=<%=menu.ID %>'"><%=menu.MenuName %></li>
        <%} %>
    </ul>	
</div>

<table class="listTable" cellpadding="0" cellpadding="0">
    <tr class="listTableHead">
	<td style="width:5%">ID</td>
    <td style="width:20%; text-align:left;text-indent:8px;">菜单标题</td>
    <td style="width:35%">URL</td>
    <td style="width:10%">图标</td>
    <td style="width:10%">排序</td>		
    <td style="width:20%">管理</td>    	
</tr>
<asp:Repeater ID="RecordList" runat="server">
    <ItemTemplate>
        <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">
 	        <td style="width:5%"><%#Eval("ID")%></td>
            <td style="width:20%; text-align:left;text-indent:8px;"><%#Eval("MenuName") %></td>
            <td style="width:35%"><%#Eval("URL")%></td>
            <td style="width:10%"><img src="/Admin/Style/Icon/<%#Eval("MenuImage")%>-icon.gif" /></td>	
            <td style="width:10%"><%#Eval("OrderID")%></td>	
            <td style="width:20%">
                <a href="?Action=Up&ID=<%# Eval("ID") %>&FatherID=<%=fatherID %>"><img src="Style/Images/moveUp.gif" alt="上移" title="上移" /></a> 
                <a href="?Action=Down&ID=<%# Eval("ID") %>&FatherID=<%=fatherID %>"><img src="Style/Images/moveDown.gif" alt="下移" title="下移" /></a>
                <a href="MenuAdd.aspx?ID=<%# Eval("ID") %>&FatherID=<%=fatherID %>"><img src="Style/Images/edit.gif" alt="修改" title="修改" /></a> 
                <a href='?Action=Delete&ID=<%# Eval("ID") %>&FatherID=<%=fatherID %>' onclick="return check()"><img src="Style/Images/delete.gif" alt="删除" title="删除" /></a>
            </td>   
        </tr>
    </ItemTemplate>
</asp:Repeater>
</table>
<div class="action">
    <input type="button" value=" 添 加 " class="button" onclick="location.href='MenuAdd.aspx?FatherID=<%=fatherID %>'"/>
</div>
</asp:Content>
