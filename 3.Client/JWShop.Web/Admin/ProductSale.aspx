<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductSale.aspx.cs" Inherits="JWShop.Web.Admin.ProductSale" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">	
    <link rel="stylesheet" href="/Admin/Js/jqdate/base/jquery.ui.all.css">    
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.core.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <link rel="stylesheet" href="/Admin/Js/jqdate/demos.css">
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_StartDate").datepicker({ changeMonth: true, changeYear: true });
            $("#ctl00_ContentPlaceHolder_EndDate").datepicker({ changeMonth: true, changeYear: true });
        });
    </script>

    <div class="container ease" id="container">
        <div class="tab-title">
            <span><a href="ProductStorage.aspx">库存分析</a></span>
            <span><a href="ProductActive.aspx">关注度分析</a></span>
            <span class="cur"><a href="ProductSale.aspx">销量分析</a></span>
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
                    <div class="head">时间：</div>
                    <SkyCES:TextBox CssClass="txt" ID="StartDate" runat="server" /> <span class="tp">到</span> <SkyCES:TextBox CssClass="txt" ID="EndDate" runat="server" />
		        </dd>
		        <dd>
                    <div class="head">排序：</div>
                    <asp:DropDownList ID="ProductOrderType" runat="server" CssClass="select"> 
                        <asp:ListItem Value="SellCount">销售数量从大到小</asp:ListItem>
                        <asp:ListItem Value="SellMoney">销售金额从大到小</asp:ListItem>
                    </asp:DropDownList> 
		        </dd>
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <table class="product-list">
                <thead>
                    <tr>
	                    <td style="width:5%">Id</td>
	                    <td style="width:60%; text-align:left;text-indent:8px;">商品名称</td>
	                    <td style="width:10%">销售数量</td>
	                    <td style="width:10%">销售金额</td>
	                    <td style="width:10%">查看</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
	                    <ItemTemplate>	     
                            <tr>
			                    <td><%# Eval("Id") %></td>
			                    <td style="text-align:left;text-indent:8px;"><%# Eval("Name") %></td>
			  	                <td><%#Eval("SellCount")%></td> 
			  	                <td><%#Eval("SellMoney")%></td> 
			  	                <td><a href="SaleDetail.aspx?Action=search&Name=&ClassID=<%=RequestHelper.GetQueryString<string>("ClassID") %>&BrandID=<%=RequestHelper.GetQueryString<string>("BrandID") %>&StartAddDate=<%=RequestHelper.GetQueryString<string>("StartDate") %>&EndAddDate=<%= RequestHelper.GetQueryString<string>("EndDate") %>&ProductID=<%# Eval("ID") %>">详细</a></td> 	        
		                    </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="5">
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