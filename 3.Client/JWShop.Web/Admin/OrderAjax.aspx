<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderAjax.aspx.cs" Inherits="JWShop.Web.Admin.OrderAjax" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="SkyCES.EntLib" %>

<div class="product-row">
    <div class="product-head">商品信息</div>
    <div class="product-main">
        <table class="product-list">
            <thead>
                <tr>
	                <td style="width:10%;">商品</td>
	                <td style="width:<%=(order.IsActivity==0 ?  45 : 55)%>%">名称</td>
	                <td style="width:10%"><%=(order.IsActivity==0 ?  "单价" : "所需积分")%></td>                  
	                <td style="width:10%">重量</td>
	                <td style="width:10%">购买数量</td>
	                <td style="width:5%">管理</td>
                </tr>
            </thead>
            <tbody>
                <% foreach (var item in orderDetailList){
                    ProductInfo product = ProductBLL.Read(item.ProductId);
                %>
	            <tr>
	                <td>
                        <a href="/ProductDetail-I<%= product.Id %>.html" target="_blank">
                            <img src="<%=product.Photo.Replace("Original","90-90")%>"  onload="photoLoad(this,60,60)" />
                        </a>
	                </td>
	                <td style="text-align:left;text-indent:8px;">
                        <a href="/ProductDetail-I<%= product.Id %>.html" target="_blank">
	                        <%=item.ProductName%>
                        </a>
	                </td>    	    
	                <td><%=(order.IsActivity==0 ?  item.ProductPrice + " 元" : item.ActivityPoint + " 分")%></td>    
                  	
	                <td><%=item.ProductWeight%> 克</td>
	                <% if (canEdit){ %>
	                    <%--<td><input type="text" value="<%=item.BuyCount%>" class="input" style="width:30px" org-value="<%=item.BuyCount%>" onblur="changeOrderProductBuyCount('<%=item.Id %>',this,'<%=item.ProductId %>',<%=item.BuyCount%>)" /></td>--%>
                    <td><%=item.BuyCount%></td>
                    <td class="link">--</td>
	                   <%-- <td class="link"><a href='javascript:void(0)' onclick="deleteOrderProduct('<%=item.Id %>','<%=item.ProductId %>','<%=item.BuyCount %>')">删除</a></td>--%>
	                <%}else{ %>
	                    <td><%=item.BuyCount%></td>
	                    <% var orderRefundList = OrderRefundBLL.ReadList(order.Id);%>
                        <td>
                            <% if(JWRefund.CanRefund(order, item, 1).CanRefund) {%>
                                <a href="javascript:loadPage('OrderRefundApply.aspx?orderId=<%=order.Id %>&orderDetailId=<%=item.Id %>','退款申请','600px','450px')">退款</a>
                            <%} else {%>
                                --
                            <%} %>
                            <% if(orderRefundList.Count(k => k.OrderDetailId == item.Id) > 0){ %>
                                <br /><a href="javascript:loadPage('OrderRefundAction.aspx?orderId=<%=order.Id %>&orderDetailId=<%=item.Id %>','退款记录','600px','450px')">退款记录</a>
                            <%} %>
                        </td>
	                <%} %>
	            </tr>
                <%} %>
            </tbody>
            <tfoot>
                <% if(order.IsActivity == 0) {%>
	                <tr><td colspan="<%= (order.IsActivity==0 ? 7 : 6) %>" style="text-align:right; padding-right:15px"><span>产品数量合计</span>：<span class="red"><%=totalProductCount%></span>；价格合计：<span class="red"><%=order.ProductMoney%></span> 元；重量合计：<span class="red"><%=totalWeight %></span> 克</td></tr>
                <% } else {%>
                    <tr><td colspan="<%= (order.IsActivity==0 ? 7 : 6) %>" style="text-align:right; padding-right:15px"><span>产品数量合计</span>：<span class="red"><%=totalProductCount%></span>；所需积分合计：<span class="red"><%=order.ActivityPoint%></span> 分；重量合计：<span class="red"><%=totalWeight %></span> 克</td></tr>
                <%} %>
            </tfoot>
        </table>
    </div>
</div>

<div class="product-row">
    <div class="product-head">费用信息 <% if (canEdit){ %><a href="javascript:loadPage('OrderAdd.aspx?ID=<%=order.Id %>&Action=Money','费用信息修改', '600px', '450px')" style="color:#2679D8">修改</a><%} %></div>
    <div class="product-main">
        <% if(order.IsActivity ==(int)OrderKind.Common || order.IsActivity == (int)OrderKind.GroupBuy || order.IsActivity==(int)OrderKind.Bargain) {%>
            <div class="clear"></div>
            <div class="form-row">
		        <div class="head">订单总金额：</div>
		        产品金额：<%=order.ProductMoney %> 元 + 运费：<%=order.ShippingMoney %> 元 + <literal title="后台调节订单费用项，“+”表示增加订单费用，“-”表示减去订单费用">其它费用</literal>：<%=order.OtherMoney %> 元 = 订单总金额：<literal class="red"><%=OrderBLL.ReadOrderMoney(order) %></literal> 元
	        </div>
            <div class="clear"></div>
            <div class="form-row">
		        <div class="head">已付金额：</div>
		        积分抵扣金额：<%=order.PointMoney %> 元 + 优惠券：<%=order.CouponMoney %> 元+优惠活动:<%=order.FavorableMoney %>元 = 已付金额：<literal class="red"><%=(OrderBLL.ReadHasPaidMoney(order)) %></literal> 元
	        </div>
            <div class="clear"></div>
            <div class="form-row">
		        <div class="head">需支付金额：</div>
		        订单总金额：<%=OrderBLL.ReadOrderMoney(order)%> 元 - 已付金额：<%=OrderBLL.ReadHasPaidMoney(order) %>元 = 需支付金额：<literal class="red"><%=OrderBLL.ReadNoPayMoney(order)%></literal> 元
	        </div>
         <div class="clear"></div>
        <%if (!string.IsNullOrEmpty(order.OrderNote))
            { %>
            <div class="form-row">
                <%=order.OrderNote%>
                </div>
            <div class="clear"></div>
        <%} %>
        <%} %>
    </div>
</div>