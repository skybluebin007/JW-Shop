<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="BookingProduct.aspx.cs" Inherits="JWShop.Web.Admin.BookingProduct" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">   
    <div class="container ease" id="container">
    	<div class="product-container" id="bookingproduct_css" style="padding-top: 20px;">
            <dl class="product-filter product-filter-pro clearfix">
            	<dd>
                    <div class="head">状态：</div>
                	<asp:DropDownList ID="IsHandler" runat="server" CssClass="select"><asp:ListItem Value="">所有</asp:ListItem><asp:ListItem Value="0">未处理</asp:ListItem><asp:ListItem Value="1">已处理</asp:ListItem></asp:DropDownList> </dd>
                <dd>
                     <div class="head">联系人：</div>
                <SkyCES:TextBox CssClass="txt" ID="RelationUser" runat="server" /></dd>
                <dd>
                     <div class="head">商品名称：</div>
		        <SkyCES:TextBox CssClass="txt" ID="ProductName" runat="server" Width="150px" />                 
                </dd>
                <dt>
                    <asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" />
                </dt>
            </dl>
            <table cellpadding="0" cellspacing="0" border="0" class="product-list product-list-img" width="100%">
                <thead>
                    <tr>
                        <td width="5%">选择</td>    
                        <td width="5%" >ID</td>
	                    <td  width="35%">商品名称</td>
	                    <td  width="10%">联系人</td>       
	                    <td width="10%">电话</td>       
	                    <td  width="10%">Email</td> 
	                    <td width="10%" >登记时间</td>       
	                    <td width="8%">是否处理</td>       
	                    <td width="7%" >管理</td>      
	                                                              
                    </tr>
                </thead>
                <tbody>
                    <tr class="firstH">
                    	<td colspan="9" style="padding: 0;">
                        	<div class="button">
	                             <%if(Count>0){ %><label class="ig-checkbox" style="float: left;padding-right: 10px;"><input type="checkbox" name="All" onclick="selectAll(this)"  class="checkall" bind="list"/>全选</label>                  
                                <asp:Button CssClass="button-2 del" ID="Button1" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>
                                &nbsp;&nbsp;每页显示：<asp:DropdownList ID="AdminPageSize" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="AdminPageSize_SelectedIndexChanged">
                                    <asp:ListItem Value="20">20条</asp:ListItem>
                                    <asp:ListItem Value="50">50条</asp:ListItem>
                                    <asp:ListItem Value="100">100条</asp:ListItem>
                                     </asp:DropdownList>
                                 <%} %>                               
                                <span style="float: left;">共找到<%=Count %>条记录<%if(Count>0){ %> ，<%=MyPager.PageCount %>页<%} %></span>
                            </div>                          
                    	</td>
                    </tr>
                <asp:Repeater ID="RecordList" runat="server">
	                <ItemTemplate>	     
                    <tr>
                        <td><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("ID") %>" /></label></td> 
                        <td><%# Eval("ID") %></td>
			            <td ><%# Eval("ProductName")%></td>
			            <td><%# Eval("RelationUser")%></td>       
	                    <td ><%# Eval("Tel")%></td>       
	                    <td ><%# Eval("Email")%></td> 
	                    <td ><%# Eval("BookingDate","{0:yyyy-MM-dd}")%></td>       
	                    <td><%#ShopCommon.GetBoolText(Eval("IsHandler"))%></td> 
			            <td ><a href="BookingProductAdd.aspx?ID=<%# Eval("ID") %>" class="ig-colink">查看</a></td>			                                  
                    </tr>
                    </ItemTemplate>
                </asp:Repeater>
        </tbody>
        <tfoot>
                	<tr>
                    	<td colspan="9">
                        	<div class="button">
	                            <label class="ig-checkbox"><input type="checkbox" value="" class="checkall" bind="list" />全选</label>
                                <asp:Button CssClass="button-2 del" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>&nbsp;                                
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