<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="AdImageMobile.aspx.cs" Inherits="JWShop.Web.Admin.AdImageMobile" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>移动端广告列表</div>	
    
    <%if(adType == (int)AdImageType.MobileFloorClass) {%>
        <div class="listBlock">
            <ul>
                <%foreach (var item in ProductClassBLL.ReadRootList()){ %>
                    <li <%if(classId == item.Id){%>class="listOn"<%} %> onclick="window.location='AdImageMobile.aspx?ad_type=<%=adType %>&class_id=<%= item.Id %>'"><%=item.Name%></li>   
                <%} %>     
            </ul>	
        </div>
    <%} %>

    <table class="listTable" cellpadding="0" cellpadding="0">
        <tr class="listTableHead">
            <td style="width:10%">图片</td>
	        <td style="width:30%; text-align:left;text-indent:8px;">标题</td>
	        <td style="width:40%;">链接地址</td>
	        <td style="width:10%">排序</td>
	        <td style="width:10%">管理</td>
        </tr>
    <asp:Repeater ID="RecordList" runat="server">
	    <ItemTemplate>	     
            <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">
                <td style="width:10%"><a href="<%# Eval("MobileImageUrl") %>" target="_blank"><img src="<%#Eval("MobileImageUrl") %>" style="height: 30px;" /></a></td>
			    <td style="width:30%; text-align:left;text-indent:8px;"><%# StringHelper.Substring(Eval("Title").ToString(), 18) %></td>
			    <td style="width:40%;"><a href="<%# Eval("MobileLinkUrl") %>" target="_blank"><%# StringHelper.Substring(Convert.ToString(Eval("MobileLinkUrl")),30) %></a></td>
	            <td style="width:10%"><%#Eval("OrderId") %></td>
			    <td style="width:10%">
                    <a href="javascript:pop('AdImageMobileAdd.aspx?Id=<%# Eval("Id") %>',600,400,'广告修改','AdImageAdd<%# Eval("Id") %>')"><img src="Style/Images/edit.gif" alt="修改" title="修改" /></a>
                    <a href='?Action=Delete&Id=<%# Eval("Id") %>&ad_type=<%=adType %>' onclick="return check()"><img src="Style/Images/delete.gif" alt="删除" title="删除" /></a>
			    </td>
		    </tr>
            </ItemTemplate>
    </asp:Repeater>
    </table>
    <div class="listPage"><SkyCES:CommonPager ID="MyPager" runat="server" /></div>
    <div class="action">
        <input type="button" value=" 添 加 " class="button" onclick="pop('AdImageMobileAdd.aspx?ad_type=<%=adType %>&class_id=<%= classId %>', 600, 400, '广告添加', 'AdImageAdd')"/>&nbsp;
    </div>
</asp:Content>
