<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="Shipping.aspx.cs" Inherits="JWShop.Web.Admin.Shipping" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
    	<div class="product-container" style="padding-top:22px;">	
           <%-- <div class="add-button"><a href="ShippingAdd.aspx" title="添加新数据" class="ease"> 添 加 </a></div>--%>
            <table class="product-list ship-add-list">
                <thead>
                    <tr>
                       <%-- <td style="width: 5%">选择</td>--%>
                        <td style="width: 5%">Id</td>
                        <td style="width: 15%">名称</td>
                        <td style="">描述</td>
                        <td style="width: 8%">是否启用</td>
                        <td style="width: 20%">管理</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
	                    <ItemTemplate>	     
        	                <tr>
			                    <%--<td><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("Id") %>" ig-bind="list" /></label></td> --%>	  
			                    <td><%# Eval("Id") %></td>
			                    <td><%# Eval("Name") %></td>
			                    <td align="left"><%# Eval("Description")%></td> 
			                    <td><%# ShopCommon.GetBoolText(Eval("IsEnabled"))%></td> 
			                    <td class="imgCz">
			                        <a href="ShippingAdd.aspx?ID=<%# Eval("Id") %>" class="ig-colink">修改</a>| 
                                    <a href="javascript:void(0);" class="ig-colink pmore">更多</a>
		                        	<div class="list">
	                                <a href="ShippingRegion.aspx?ShippingID=<%# Eval("Id") %>" class="ig-colink">区域设置</a>
				                    <a href="?Action=Up&Id=<%#Eval("Id")%>" class="ig-colink">上移</a>
			                        <a href="?Action=Down&Id=<%#Eval("Id")%>" class="ig-colink">下移</a>
                                    </div>
			                       
			                    </td>      
		                    </tr>
                            </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                    	<td colspan="5">
                        	<div class="button">
	                          <%--  <label class="ig-checkbox"><input type="checkbox" name="All" value="" class="checkall" bind="list" />全选</label>--%>
                                <asp:Button runat="server" Text=" 删 除 " CssClass="button-2 del" OnClientClick="return checkSelect()" OnClick="DeleteButton_Click" style="display:none;"/>
                            </div>
                            <div class="clear"></div>
                    	</td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        $(".pmore").mouseenter(function(){
			$(this).next().show();
		});
		
		$(".imgCz .list,.imgCz").mouseleave(function(){
			$(".imgCz .list").hide();
		});
    </script>
</asp:Content>
