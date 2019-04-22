<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductType.aspx.cs" Inherits="JWShop.Web.Admin.ProductType" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
    	<div class="product-container" style="padding-top: 30px;">
             <dl class="classify-menu">
                <dd class="add" onclick="location.href='ProductTypeAdd.aspx'">添加类型</dd>
             </dl>
            <table cellpadding="0" cellspacing="0" border="0" class="product-list" width="100%">
                <thead>
                    <tr>
                        <td width="50">选择</td>     
	                    <td width="50">ID</td>
	                    <td width="150">名称</td>
	                    <td >包含品牌</td>       
	                    <td width="100">管理</td>      
	                                                             
                    </tr>
                </thead>
                <tbody>
                <asp:Repeater ID="RecordList" runat="server">
	                <ItemTemplate>	     
        	                <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">
                            <td ><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("ID") %>"  ig-bind="list"/></label></td> 	  
			                <td ><%# Eval("ID") %></td>
			                <td ><%# Eval("Name") %></td>
                            <td ><%#GetBrandList(Eval("BrandIds").ToString()) %></td>
			                <td >
			                    <a href="ProductTypeAdd.aspx?ID=<%# Eval("ID") %>"  class="ig-colink">修改</a> 
			                </td>			                      
		                </tr>
                        </ItemTemplate>
                </asp:Repeater>
                </tbody>
                        <tfoot>
                	<tr>
                    	<td colspan="5">
                        	<div class="button">
	                            <label class="ig-checkbox"><input type="checkbox" value="" class="checkall" bind="list" />全选</label>
                               <input type="button"  value=" 添 加 " class="button-2 del" onclick="location.href = 'ProductTypeAdd.aspx'"/>&nbsp;<asp:Button CssClass="button-2 del" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>&nbsp;                                
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