<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductComment.aspx.cs" Inherits="JWShop.Web.Admin.ProductComment" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <link rel="stylesheet" href="/Admin/js/jqdate/base/jquery.ui.all.css">    
    <script src="/Admin/js/jqdate/js/jquery.ui.core.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.widget.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_StartPostDate").datepicker({ changeMonth: true, changeYear: true });
            $("#ctl00_ContentPlaceHolder_EndPostDate").datepicker({ changeMonth: true, changeYear: true });
        });
    </script>
    <div class="container ease" id="container">
    	<div class="product-container" style="padding-top: 20px;">
            <dl class="product-filter product-filter-pro clearfix">
                <dd>
                    <div class="head">状态：</div>
                    <asp:DropDownList ID="Status" runat="server" CssClass="select" Width="80px">
                        <asp:ListItem Value="">所有</asp:ListItem>
                        <asp:ListItem Value="1">未处理</asp:ListItem>
                        <asp:ListItem Value="2">显示</asp:ListItem>
                        <asp:ListItem Value="3">不显示</asp:ListItem>
                    </asp:DropDownList></dd>
                <dd>
                    <div class="head">商品名称：</div>
                    <asp:TextBox ID="Name" CssClass="select" runat="server" Width="80px" />
                </dd>

                <dd>
                    <div class="head">评论时间：</div>
                    <SkyCES:TextBox CssClass="txt" ID="StartPostDate" runat="server" RequiredFieldType="日期时间" Width="70" /><span class="tp"> 到 </span>
                    <SkyCES:TextBox CssClass="txt" ID="EndPostDate" runat="server" RequiredFieldType="日期时间" Width="70" /></dd>
                <dt>
                    <asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server" OnClick="SearchButton_Click" /></dt>
            </dl>
            <table cellpadding="0" cellspacing="0" border="0" class="product-list" width="100%">
                <thead>
                    <tr>
                        <td width="50" height="40">选择</td>
                        <td width="50">序号</td>
	                    <td width="150">商品名称</td>
	                    <td width="150">时间</td>   
	                    <td width="50">状态</td>
	                    <td width="150">管理</td>
                    </tr>
                </thead>
                <tbody>
                    <tr class="firstH">
                    	<td colspan="6" style="padding: 0;">
                        	<div class="button">
	                             <%if(Count>0){ %><label class="ig-checkbox" style="float: left;padding-right: 10px;"><input type="checkbox" name="All" onclick="selectAll(this)"  class="checkall" bind="list"/>全选</label>                  
                                <asp:Button CssClass="button-2 del" ID="Button1" Text=" 不显示 " OnClientClick="return checkSelect()" runat="server"  OnClick="NoShowButton_Click"/>&nbsp;
                                <asp:Button CssClass="button-2 del" ID="Button2" Text=" 显 示 " OnClientClick="return checkSelect()" runat="server"  OnClick="ShowButton_Click"/>&nbsp;
                                <asp:Button CssClass="button-2 del" ID="Button3" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>
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
                          <td ><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("ID") %>" ig-bind="list" /></label></td> 	
                          <td ><%# Eval("ID") %></td>
			            <td ><%#ProductBLL.Read(Convert.ToInt32(Eval("ProductId"))).Name %></td>  
	                    <td ><%#Eval("PostDate") %></td>    
	                    <td ><%#EnumHelper.ReadEnumChineseName<CommentStatus>(Convert.ToInt32(Eval("Status")))%></td> 
			            <td >
			                <a href="ProductCommentAdd.aspx?ID=<%# Eval("ID") %>" class="ig-colink">审核</a> 
			              
			            </td>
			                                            
                    </tr>
                    </ItemTemplate>
                </asp:Repeater>
        </tbody>
        <tfoot>
                	<tr>
                    	<td colspan="6">
                        	<div class="button">
	                            <label class="ig-checkbox"><input type="checkbox" name="All"  onclick="selectAll(this)"   class="checkall" bind="list" />全选</label>
                                <asp:Button CssClass="button-2 del" ID="NoHandlerButton" Text=" 未处理 " OnClientClick="return checkSelect()" runat="server"  OnClick="NoHandlerButton_Click" style="display:none;"/>&nbsp;
                                <asp:Button CssClass="button-2 del" ID="NoShowButton" Text=" 不显示 " OnClientClick="return checkSelect()" runat="server"  OnClick="NoShowButton_Click"/>&nbsp;
                                <asp:Button CssClass="button-2 del" ID="ShowButton" Text=" 显 示 " OnClientClick="return checkSelect()" runat="server"  OnClick="ShowButton_Click"/>&nbsp;
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
