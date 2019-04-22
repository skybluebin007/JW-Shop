<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="UserProvider.aspx.cs" Inherits="JWShop.Web.Admin.UserProvider" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <script language="javascript" src="/Admin/js/calendar.js" type="text/javascript"></script>
    <div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>供应商列表</div>	
    <ul class="search">
        <li>
            用户名：<SkyCES:TextBox ID="UserName" CssClass="input" runat="server"  /> 
            供应商编号：<SkyCES:TextBox ID="ProviderNo" CssClass="input" runat="server"  /> 
         <asp:Button CssClass="button" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></li>
    </ul>

    <table class="listTable" cellpadding="0" cellpadding="0">
        <tr class="listTableHead">
	        <td style="width:5%">Id</td>
	        <td style="width:5%;">编号</td>
	        <td style="width:20%; text-align:left;text-indent:8px;">公司名称</td>
	        <td style="width:15%; text-align:left;text-indent:8px;">用户名</td>
	        <td style="width:10%">Email</td>
	        <td style="width:12%">注册时间</td>
	        <td style="width:6%">登陆次数</td>
	        <td style="width:12%">最近登录时间</td>
	        <td style="width:5%">状态</td>
	        <td style="width:10%">管理</td>        
        </tr>
    <asp:Repeater ID="RecordList" runat="server">
	    <ItemTemplate>	     
        	    <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">
			        <td><%# Eval("Id") %></td>
                    <td><%# Eval("ProviderNo") %></td>
			        <td style="text-align:left;text-indent:8px;"><%# Eval("ProviderName") %></td>
			        <td style="text-align:left;text-indent:8px;"><%# Eval("UserName") %></td>
			        <td><%# Eval("Email") %></td>
	                <td><%# Eval("RegisterDate") %></td>
	                <td><%#Eval("LoginTimes")%></td>
	                <td><%#Eval("LastLoginDate")%></td>
	                <td><%#EnumHelper.ReadEnumChineseName<UserStatus>(Convert.ToInt32(Eval("Status")))%></td>
			        <td>			    
                        <a href="javascript:pop('UserAdd.aspx?Id=<%#Eval("Id")%>',800,600,'供应商修改','UserAdd<%# Eval("Id") %>')"><img src="Style/Images/edit.gif" alt="修改" title="修改" /></a> 
			            <a href="javascript:popPageOnly('UserPasswordAdd.aspx?ID=<%#Eval("ID")%>',600,250,'修改密码','UserPasswordAdd<%# Eval("ID") %>')"><img src="Style/Images/password.gif" alt="修改密码" title="修改密码" /></a> 
                        <a href='?Action=Delete&Id=<%# Eval("Id") %>' onclick="return check()"><img src="Style/Images/delete.gif" alt="删除" title="删除" /></a>
			        </td>
		        </tr>
            </ItemTemplate>
    </asp:Repeater>
    </table>
    <div class="listPage"><SkyCES:CommonPager ID="MyPager" runat="server" /></div>
    <div class="action">
        <asp:Button CssClass="button" Text=" 导 出 " ID="btnExport" OnClick="btnExport_Click" runat="server" />
    </div>
</asp:Content>
