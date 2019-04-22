<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="Navigation.aspx.cs" Inherits="JWShop.Web.Admin.Navigation"  %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Register Assembly="SkyCES.EntLib" Namespace="SkyCES.EntLib" TagPrefix="SkyCES" %>

<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>网站导航列表</div>

    <div class="listBlock">
        <ul>
            <% foreach(var item in _NavigationTypeList) {%>
                <li <%if (_CurrentNavigationType == item.Value) {%>class="listOn"<%} %> 
                        onclick="window.location='Navigation.aspx?navigationType=<%= item.Value %>'">
                    <%= item.ChineseName %>
                </li>  
            <%} %> 
        </ul>	
    </div>

    <table class="listTable" cellpadding="0" cellpadding="0">
        <tr class="listTableHead">
            <td style="width:5%;">Id</td>
            <td style="width:25%;text-align:left;text-indent:8px;">Name</td>
            <td style="width:5%;text-align:left;text-indent:8px;">Desc</td>
            <td style="width:5%">OrderId</td>
            <td style="width:5%">可见</td>
            <td style="width:10%">内容类型</td>
            <td style="width:15%">Url</td>
            <td style="width:5%">内容Id</td>
            <td style="width:10%">内容组成</td>
            <td style="width:10%">显示方式</td>
            <td style="width:5%">修改</td>
        </tr>
    <asp:Repeater ID="RecordList" runat="server">
        <ItemTemplate>
            <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">
                <td><%#Eval("Id")%></td>
 	            <td style="text-align:left;text-indent:8px;"><%#Eval("Name")%></td>
                <td style="text-align:left;text-indent:8px;"><%#Eval("Remark") %></td>
                <td><%#Eval("OrderId")%></td>
                <td><%#ShopCommon.GetBoolText(Convert.ToInt32(Eval("IsVisible")))%></td>
                <td><%#EnumHelper.ReadEnumChineseName<NavigationClassType>(Convert.ToInt32(Eval("ClassType")))%></td>
                <td><%#Eval("Url") %></td>
                <td><%# Eval("ClassId").ToString() == "0" ? "" : Eval("ClassId")%></td>
                <td>
                    <%# Convert.ToInt32(Eval("ClassType")) == (int)NavigationClassType.Url ? "" : Convert.ToBoolean(Eval("IsSingle")) ? "单一的" : "列表" %>
                </td>
                <td><%#EnumHelper.ReadEnumChineseName<NavigationShowType>(Convert.ToInt32(Eval("ShowType")))%></td>
                <td>
                    <a href="NavigationAdd.aspx?id=<%# Eval("Id") %>"><img src="Style/Images/edit.gif" alt="修改" title="修改" /></a> 
                    <a href='?Action=Delete&Id=<%# Eval("Id") %>&navigationType=<%=_CurrentNavigationType%>' onclick="return check()"><img src="Style/Images/delete.gif" alt="删除" title="删除" /></a>
                </td>   
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </table>

    <div class="action">
       <div class="add-button"> <a class="ease" href="NavigationAdd.aspx?navigationType=<%=_CurrentNavigationType%>">添加</a></div>
    </div>
</asp:Content>
