<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="SaleDetail.aspx.cs" Inherits="JWShop.Web.Admin.SaleDetail" %>
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
            $("#ctl00_ContentPlaceHolder_StartAddDate").datepicker({ changeMonth: true, changeYear: true });
            $("#ctl00_ContentPlaceHolder_EndAddDate").datepicker({ changeMonth: true, changeYear: true });
        });
    </script>

    <div class="container ease" id="container">
        <div class="tab-title">
            <span><a href="SaleTotal.aspx">销售汇总</a></span>
            <span><a href="SaleStop.aspx">滞销分析</a></span>
            <span class="cur"><a href="SaleDetail.aspx">销售流水账</a></span>
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
                    <SkyCES:TextBox CssClass="txt" ID="Name" runat="server" Width="200px"/> 
		        </dd>
		        <dd>
                    <div class="head">订单号：</div>
                    <SkyCES:TextBox CssClass="txt" ID="OrderNumber" runat="server" Width="100px"/> 
		        </dd>
		        <dd>
                    <div class="head">用户名：</div>
                    <SkyCES:TextBox CssClass="txt" ID="UserName" runat="server" Width="100px"/> 
		        </dd>
		        <dd>
                    <div class="head">时间：</div>
                    <SkyCES:TextBox CssClass="txt" ID="StartAddDate" runat="server" /> <span class="tp">到</span> <SkyCES:TextBox CssClass="txt" ID="EndAddDate" runat="server" />
		        </dd>
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <table class="product-list">
                <thead>
                    <tr>
	                    <td style="width:15%">时间</td>
	                    <td style="width:10%">单号</td>
	                    <td style="width:45%; text-align:left;text-indent:8px;">商品名称</td>
	                    <td style="width:5%">数量</td>
	                    <td style="width:10%">金额</td>
	                    <td style="width:10%">用户名</td>      
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
	                    <ItemTemplate>	     
        	                <tr>
			                    <td><%# Eval("AddDate") %></td>
			                    <td><%# Eval("OrderNumber") %></td>
			                    <td style="text-align:left;text-indent:8px;"><%# Eval("Name") %></td>
			                    <td><%#Eval("BuyCount")%></td> 
			                    <td><%#Eval("Money")%></td> 	 
			                    <td><%#HttpUtility.UrlDecode(Eval("UserName").ToString(),Encoding.UTF8) %></td>        
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