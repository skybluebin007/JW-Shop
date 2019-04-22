<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="OrderRefundAdd.aspx.cs" Inherits="JWShop.Web.Admin.OrderRefundAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
        <div class="product-container product-container-border">
            <div class="form-row">
                <div class="head">订 单 号 ：</div>
                <%= order.OrderNumber %>
            </div>
            <div class="form-row">
                <div class="head">订单状态：</div>
                <%= OrderBLL.ReadOrderStatus(order.OrderStatus,order.IsDelete) %>
            </div>
            <div class="form-row">
                <div class="head">费用信息：</div>
                商品金额：<%=order.ProductMoney %> 元
                      <% if(order.ShippingMoney > 0) {%>  + 运费：<%=order.ShippingMoney %> 元 <%} %>
                      <% if(order.OtherMoney != 0) {%>  + <span title="后台调节订单费用项，“+”表示增加订单费用，“-”表示减去订单费用">其它费用</span>：<%=order.OtherMoney %> 元 <%} %>
                      <% if(order.PointMoney > 0) {%>  - 积分抵扣金额：<%=order.PointMoney %> 元 <%} %>
                     = 需支付金额：<b style="color:#B20000"><%=(OrderBLL.ReadOrderMoney(order) - order.PointMoney)%></b> 元
            </div>
            <div class="form-row">
                <div class="head">退款商品信息：</div>
                <table cellpadding="0" cellspacing="0" border="0" width="100%"  class="tableList">
                    <tr class="tableHead">
	                    <td style="width:10%;">商品</td>
	                    <td style="width:65%">名称</td>
	                    <td style="width:10%">单价</td>
	                    <td style="width:10%">购买数量</td>
	                    <td style="width:5%">小计</td>
	                </tr>
                    <% foreach (var item in orderDetailList){
                        ProductInfo product = ProductBLL.Read(item.ProductId);
                    %>
	                    <tr class="tableMain">
                            <td><img src="<%=product.Photo.Replace("Original","90-90")%>" onload="photoLoad(this,60,60)" /></td>
                            <td><%=item.ProductName%></td>
                            <td><%=item.ProductPrice%></td>
                            <td><%=item.BuyCount%></td>
                            <td><%=(item.ProductPrice * item.BuyCount)%></td>
                        </tr>
                    <%} %>
                    <tr><td colspan="5" style="text-align:right; padding-right:15px"><span>产品数量合计</span>：<span class="orderMoney"><%=(orderDetailList.Sum(k => k.BuyCount))%></span>；价格合计：<span class="orderMoney"><%=(orderDetailList.Sum(k => k.ProductPrice * k.BuyCount))%></span> 元</td></tr>
                </table>
            </div>
            <div class="form-row">
                <div class="head">申请时间：</div>
                <%= orderRefund.TmCreate %>
            </div>
            <% if(orderRefund.OrderDetailId > 0) {%>
                <div class="form-row">
                    <div class="head">退款数量：</div>
		            <b style="color:#B20000"><%= orderRefund.RefundCount %></b>
	            </div>
            <%} %>
            <div class="form-row">
                <div class="head">退款金额：</div>
                <% if(orderRefund.RefundBalance > 0) {%>余额：<%= orderRefund.RefundBalance %> <%} %>
                <% if(orderRefund.RefundMoney > 0) {%><%=(orderRefund.RefundBalance > 0 ? "+" : "")%> <%= orderRefund.RefundPayName %>： <%= orderRefund.RefundMoney %> <%} %>
                    <% if (orderRefund.RefundBalance > 0)
                        {%>= <b style="color:#B20000"><%= (orderRefund.RefundBalance + orderRefund.RefundMoney) %><%} %></b>
            </div>
            <div class="form-row">
                <div class="head">退款说明：</div>
                <%= orderRefund.RefundRemark %>
            </div>
            <!--系统审核-->
            <% if(orderRefund.Status == (int)OrderRefundStatus.Submit) {%>
                <div class="form-row">
                    <div class="head">备注：</div>
                    <SkyCES:TextBox ID="Remark" CssClass="input" TextMode="MultiLine" Width="400px" Height="60px" runat="server"></SkyCES:TextBox>
                </div>
                <div class="form-row">
                    <div class="head"></div>
                    <asp:Button CssClass="button" Id="ApproveButton" style="    font-size: 12px;
    line-height: 12px;
    margin-right: 5px;
    padding: 8px 16px;
    border: 1px solid #dddddd;
    background: #f7f7f7;
    cursor: pointer;" runat="server" Text="审核通过" OnClick="ApproveButton_Click" OnClientClick="return confirm('确认通过该退款申请？')" />
                    <asp:Button CssClass="button" Id="RejectButton" style="    font-size: 12px;
    line-height: 12px;
    margin-right: 5px;
    padding: 8px 16px;
    border: 1px solid #dddddd;
    background: #f7f7f7;
    cursor: pointer;" runat="server" Text="审核拒绝" OnClick="RejectButton_Click" OnClientClick="return confirm('确认拒绝该退款申请？')" />
                </div>
            <%} else {%>
                <div class="form-row">
                    <div class="head">备注：</div>
                    <%= orderRefund.Remark %>
                </div>

                <!--对需通过第三方平台退款的服务单，如在系统审核通过后仍未处理退款，则退款按钮是可操作的（即便有需要退到余额里的钱，因为流程也是在第三方退款成功后才退，所以并不影响）-->
                <!--可以考虑的更加严谨一点，但对于支付宝或微信支付这样的平台，如确实进行了退款的操作，他们的回调是非常快的，所以暂时只做到这一步-->
                <% if(orderRefund.Status == (int)OrderRefundStatus.Approve || orderRefund.Status == (int)OrderRefundStatus.Returning) {%>
                    <div class="form-row">
                        <div class="head">备注：</div>
                        <asp:Button CssClass="button" Id="RefundButton" runat="server" Text=" 退 款 " OnClick="RefundButton_Click" OnClientClick="return confirm('确认退款？')" />
                        <asp:Button CssClass="button" Id="CancelButton" runat="server" Text=" 取 消 " OnClick="CancelButton_Click" OnClientClick="return confirm('确认取消该退款申请？')" />
                    </div>
                <%} %>
            <%} %>

            <%if(orderRefundActionList.Count > 0){ %>
            <table cellpadding="0" cellspacing="0" border="0" width="100%"  class="tableList">
                <tr class="tableHead">
                    <td style="width:18%">处理人</td>
                    <td style="width:10%">处理状态</td>
	                <td style="width:50%">备注</td>
	                <td style="width:22%">处理时间</td>
                </tr>
                <%foreach (var orderRefundAction in orderRefundActionList){ %>
                    <tr class="tableMain">
	                    <td><%= orderRefundAction.UserName %></td>
                        <td><%= ShopCommon.GetBoolString(orderRefundAction.Status) %></td>
	                    <td><%= orderRefundAction.Remark %></td>
	                    <td><%= orderRefundAction.Tm %></td>
                    </tr>
	            <%} %>
            </table>
        <%} %>
        </div>
    </div>
</asp:Content>