<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="FpImage.aspx.cs" Inherits="JWShop.Web.Admin.AdImage" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="container ease" id="container">
        <div class="product-container" style="padding-top: 24px;">        
    <div class="add-button"><a href="FpImageAdd.aspx?fp_type=<%=Request["fp_type"] %>" title="添加新数据" class="ease"> 添 加 </a></div>
           
            <%if(theFlash.Width>0 && theFlash.Height>0) {%><span class="red">建议上传图片尺寸<%=theFlash.Width %>px X <%=theFlash.Height %>px</span><%} %>
    <table class="product-list" >
        <thead>
                    <tr>  
            <td style="width:10%">图片</td>
	        <td style="width:30%; text-align:left;text-indent:8px;">标题</td>
	        <td style="width:40%;">链接地址</td>
	        <td style="width:10%">排序</td>
	        <td style="width:10%">管理</td>
        </tr>
            </thead>
    <asp:Repeater ID="RecordList" runat="server">
	    <ItemTemplate>	     
            <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">
                <td style="width:10%"><a href="<%# Eval("ImageUrl") %>" target="_blank"><img src="<%#Eval("ImageUrl") %>" style="height: 30px;vertical-align: middle;" /></a></td>
			    <td style="width:30%; text-align:left;text-indent:8px;"><%# StringHelper.Substring(Eval("Title").ToString(), 18) %></td>
			    <td style="width:40%;"><a href="<%# Eval("LinkUrl") %>" target="_blank"><%# StringHelper.Substring(Convert.ToString(Eval("LinkUrl")),30) %></a></td>
	            <td style="width:10%"><%#Eval("OrderId") %></td>
			    <td style="width:10%">
                    <a href="FpImageAdd.aspx?Id=<%# Eval("Id") %>">修改</a> | <a href='?Action=Delete&Id=<%# Eval("Id") %>&fp_type=<%=adType %>' onclick="return check()">删除</a>
			    </td>
		    </tr>
            </ItemTemplate>
    </asp:Repeater>
    </table>
    <div class="listPage"><SkyCES:CommonPager ID="MyPager" runat="server" /></div>    
            </div>
    </div>
</asp:Content>
