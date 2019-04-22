<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductStorage.aspx.cs" Inherits="JWShop.Web.Admin.ProductStorage" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">	
    <div class="container ease" id="container">
        <div class="tab-title">
            <span class="cur"><a href="ProductStorage.aspx">库存分析</a></span>
            <span><a href="ProductActive.aspx">关注度分析</a></span>
            <span><a href="ProductSale.aspx">销量分析</a></span>
        </div>
        <div class="product-container product-container-border">
            <dl class="product-filter clearfix" style="float: none; margin-bottom: 0;">
		        <dd>
                    <div class="head">分类：</div>
                    <asp:DropDownList ID="ClassID" runat="server" CssClass="select" /> 
		        </dd>
		        <dd>
                    <div class="head">品牌：</div>
                    <asp:DropDownList ID="BrandID" runat="server" CssClass="select" /> 
		        </dd>
		        <dd>
                    <div class="head">商品名称：</div>
                    <SkyCES:TextBox CssClass="txt" ID="Name" runat="server" Width="100px"/> 
		        </dd>
		        <dd>
                    <div class="head">库存分析：</div>
                    <asp:DropDownList ID="StorageAnalyse" runat="server" CssClass="select"> 
                        <asp:ListItem Value="0">请选择</asp:ListItem>
                        <asp:ListItem Value="1" style=" color:#ff0000">短缺商品</asp:ListItem> 
                        <asp:ListItem Value="2" style="color:#33dd33">安全商品</asp:ListItem>
                    </asp:DropDownList> 
		        </dd>
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <table class="product-list">
                <thead>
                    <tr>
	                    <td style="width:5%">Id</td>
	                    <td style="width:60%; text-align:left;text-indent:8px;">商品名称</td>
	                    <td style="width:10%">剩余库存量</td>
	                    <td style="width:10%">库存下限</td>
	                    <%--<td style="width:10%">库存上限</td>--%>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
	                    <ItemTemplate>	     
                            <tr>
			                    <td><%# Eval("Id") %></td>
			                    <td style="text-align:left;text-indent:8px;"><%# Eval("Name") %></td>
			                    <td style="color:<%# ShowColor(Convert.ToInt32(Eval("LowerCount")),Convert.ToInt32(Eval("TotalStorageCount"))-Convert.ToInt32(Eval("OrderCount")),Convert.ToInt32(Eval("ImportActualStorageCount")),Convert.ToInt32(Eval("UpperCount")))%>">
                                    <%#ShowStorageCount(Convert.ToInt32(Eval("TotalStorageCount"))-Convert.ToInt32(Eval("OrderCount")),Convert.ToInt32(Eval("ImportActualStorageCount")))%></td>
			                    <td><%#Eval("LowerCount")%></td>
			                    <%--<td><%#Eval("UpperCount")%></td>--%>
		                    </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="4">
                        	<div class="button">
                                <asp:Button CssClass="button-3" ID="ExportButton" Text=" 导 出 " runat="server" OnClick="ExportButton_Click" />                                
                            </div>
                            <SkyCES:CommonPager ID="MyPager" runat="server" />
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</asp:Content>