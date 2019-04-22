<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductBrand.aspx.cs" Inherits="JWShop.Web.Admin.ProductBrand" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
    	<div class="product-container" style="padding-top: 20px;">
            <dl class="product-filter product-filter-pro clearfix">
                 <dd style="margin-right: 90px;">
                	<div class="head">品牌名称：</div>
                    <SkyCES:TextBox CssClass="txt" MaxLength="20" ID="Key" runat="server" Width="150px" placeholder="品牌名称"/>                   
                </dd>             
             
               <dd style="margin-right: 90px;">
                	<div class="head">是否推荐：</div>
                    <asp:DropDownList ID="IsTop" Width="162px" runat="server" CssClass="select">
                        <asp:ListItem Value="">全部</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                        <asp:ListItem Value="1">是</asp:ListItem>
                    </asp:DropDownList>
                </dd>
                         
                <dt style="margin-left: 20px;">                          
                    <asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" />
                </dt>
            </dl>

            <table cellpadding="0" cellspacing="0" border="0" class="product-list" width="100%">
                <thead>
                    <tr>
                        <td width="50">选择</td>
                        <td width="100">ID</td>                        
                        <td width="auto">商品品牌</td>
                        <td width="150">图片</td>                        
                      <%--  <td width="150">Url</td>--%>
                        <td width="150">是否推荐</td>
                        <td width="50">管理</td>                                                
                    </tr>
                </thead>
                <tbody>
                    <tr class="firstH">
                        <td colspan="6" style="padding: 0;">
                            <div class="button">
                                <%if (Count > 0)
                                  { %><label class="ig-checkbox" style="float: left; padding-right: 10px;"><input type="checkbox" name="All" onclick="selectAll(this)" class="checkall" bind="list" />全选</label>
                                <input type="button" class="button-2 del" id="Button2" value=" 添 加 " onclick="location.href = 'ProductBrandAdd.aspx'" />
                                &nbsp;&nbsp;<asp:Button CssClass="button-2 del" ID="Button1" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>
                                 &nbsp;&nbsp;每页显示：<asp:DropDownList ID="AdminPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AdminPageSize_SelectedIndexChanged">
                                    <asp:ListItem Value="20">20条</asp:ListItem>
                                    <asp:ListItem Value="50">50条</asp:ListItem>
                                    <asp:ListItem Value="100">100条</asp:ListItem>
                                </asp:DropDownList>
                                <%} %>
                                <span style="float: left;">共找到<%=Count %>条记录<%if (Count > 0)
                                                                              { %> ，<%=MyPager.PageCount %>页<%} %></span>
                            </div>
                        </td>
                    </tr>
                <asp:Repeater ID="RecordList" runat="server">
	                <ItemTemplate>	     
                    <tr>
                        <td height="80"><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("ID") %>" ig-bind="list" /></label></td>
                        <td ><%# Eval("ID") %></td>
                        <td ><%# Eval("Name") %></td>
                        <td ><div class="scan-img"><img src="<%#ShopCommon.ShowImage(Eval("ImageUrl").ToString()) %>" /></div></td>                       
                        <%--<td ><%# Eval("LinkUrl") %></td>--%>
                        <td ><%#ShopCommon.GetBoolText(Convert.ToInt32(Eval("IsTop")))%></td>
                        <td ><a href="ProductBrandAdd.aspx?ID=<%# Eval("ID") %>" class="ig-colink">编辑</a>|
                            <a href="javascript:" class="ig-colink" onclick="if(confirm('确定要删除该品牌？')){window.location.href='?Action=Delete&ID=<%# Eval("ID") %>';}else {
                return false;}">删除</a>
                        </td>                        
                    </tr>
                    </ItemTemplate>
                </asp:Repeater>
        </tbody>
        <tfoot>
                	<tr>
                    	<td colspan="6">
                        	<div class="button">
	                            <label class="ig-checkbox"><input type="checkbox" value="" class="checkall" bind="list" />全选</label>
                                <input type="button"  value=" 添 加 " class="button-2 del"  onclick="location.href = 'ProductBrandAdd.aspx'"/>&nbsp;<asp:Button CssClass="button-2 del" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>&nbsp;                                
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