<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="SaleStop.aspx.cs" Inherits="JWShop.Web.Admin.SaleStop" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <style type="text/css">.product-list thead td { border:1px solid #ccc;}</style>	
    <div class="container ease" id="container">
        <div class="tab-title">
            <span><a href="SaleTotal.aspx">销售汇总</a></span>
            <span class="cur"><a href="SaleStop.aspx">滞销分析</a></span>
            <span><a href="SaleDetail.aspx">销售流水账</a></span>
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
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <table class="product-list">
                <thead>
                    <tr>
	                    <td style="width:5%" rowspan="2">Id</td>
	                    <td style="width:40%; text-align:left;text-indent:8px;" rowspan="2">商品名称</td>
	                    <td style="width:40%" colspan="3">最近一次销售</td>
	                    <td style="width:10%" rowspan="2">滞销天数</td>        
                    </tr>
                    <tr>
	                    <td style="width:10%">订单号</td>
                        <td style="width:10%">销售数量</td>
                        <td style="width:20%">日期</td>           
                    </tr>
                </thead>
                <tbody>
                    <%foreach (ProductInfo product in productList){
                      DataRow dr = ReadSaleStop(product.Id, dt);
                    %>
                        <tr>
		                    <td><%=product.Id%></td>
		                    <td style="text-align:left;text-indent:8px;"><%=product.Name%></td>
                            <%if(dr!=null){ %>
		  	                    <td><%=dr["OrderNumber"] %></td>
                                <td><%=dr["BuyCount"] %></td>
                                <td><%=dr["AddDate"]%></td>
                                <td><%=(DateTime.Now.Date-Convert.ToDateTime(dr["AddDate"]).Date).Days%> 天 </td>   
                            <%}else{ %>	 
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>  
                            <%} %>        
	                    </tr>
                    <%} %>
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