<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ThemeActivity.aspx.cs" Inherits="JWShop.Web.Admin.ThemeActivity" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<asp:Content Id="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
        <div class="product-container">	
    <table class="product-list">
                <thead>
        <tr class="listTableHead">
             <td style="width:5%">选择</td>  
	    <td style="width:5%">Id</td>
            <td style="width:10%">图片</td>
	    <td style="width:75%; text-align:left;text-indent:8px;">名称</td>
	    <td style="width:5%">管理</td>       
	         
    </tr>
                    </thead>
        <tbody>
    <asp:Repeater Id="RecordList" runat="server">
	    <ItemTemplate>	     
        	    <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">
                <td style="width:5%"><input type="checkbox" name="SelectID" value="<%# Eval("Id") %>" /></td> 
			    <td style="width:5%"><%# Eval("Id") %></td>
                    <td><a href="<%# Eval("Photo") %>" target="_blank"><img src="<%#Eval("Photo") %>" style="height: 30px;vertical-align: middle;" /></a></td>
			    <td style="width:75%; text-align:left;text-indent:8px;"><%# Eval("Name")%>
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    [<a href="<%#GetUrl(Eval("Id")) %>" target="_blank"><%#GetUrl(Eval("Id"))%></a>]
			    </td>
			    <td style="width:5%;"><a href="ThemeActivityAdd.aspx?Id=<%# Eval("Id")%>" class="ig-colink">修改</a></td>
			   	        
		    </tr>
            </ItemTemplate>
    </asp:Repeater>
            </tbody>
        <tfoot>
                    <tr>
                        <td colspan="5">
                            <div class="button">
            <input type="button"  value=" 添 加 " class="button-2" onclick="location.href='ThemeActivityAdd.aspx'"/>&nbsp;<asp:Button CssClass="button-2 del" Id="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>&nbsp;<input type="checkbox" name="All" onclick="    selectAll(this)" />全选/取消
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
