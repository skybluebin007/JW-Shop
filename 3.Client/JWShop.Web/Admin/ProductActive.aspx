<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductActive.aspx.cs" Inherits="JWShop.Web.Admin.ProductActive" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">	
    <div class="container ease" id="container">
        <div class="tab-title">
            <span><a href="ProductStorage.aspx">库存分析</a></span>
            <span class="cur"><a href="ProductActive.aspx">关注度分析</a></span>
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
                    <div class="head">名称：</div>
                    <SkyCES:TextBox CssClass="txt" ID="Name" runat="server" Width="100px"/> 
		        </dd>
		        <dd>
                    <div class="head">排序：</div>
                    <asp:DropDownList ID="ProductOrderType" runat="server" CssClass="select"> 
                        <asp:ListItem Value="CommentCount">评论数从大到小</asp:ListItem>
                        <asp:ListItem Value="PerPoint">平均分从大到小</asp:ListItem> 
                        <asp:ListItem Value="CollectCount">收藏数从大到小</asp:ListItem>
                        <asp:ListItem Value="ViewCount">浏览数从大到小</asp:ListItem>
                    </asp:DropDownList> 
		        </dd>
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <table class="product-list">
                <thead>
                    <tr>
	                    <td style="width:5%">Id</td>
	                    <td style="width:50%; text-align:left;text-indent:8px;">商品名称</td>
	                    <td style="width:10%">评论数</td>
	                    <td style="width:10%">平均分</td>
	                 <%--   <td style="width:10%">收藏数</td>  --%>
	                    <td style="width:10%">浏览数</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
	                    <ItemTemplate>	     
                            <tr>
			                    <td style="width:5%"><%# Eval("Id") %></td>
			                    <td style="text-align:left;text-indent:8px;"><%# Eval("Name") %></td>
			  	                <td><%#Eval("CommentCount")%></td> 
			  	                <td><%#Eval("PerPoint")%></td> 
			  	              <%--  <td><%#Eval("CollectCount")%></td> 	--%> 
			                    <td><%#Eval("ViewCount")%></td>
		                    </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="6">
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