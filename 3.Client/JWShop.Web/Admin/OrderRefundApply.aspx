<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="OrderRefundApply.aspx.cs" Inherits="JWShop.Web.Admin.OrderRefundApply" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<style>
body { min-width:100%; width:100%; }
</style>
<div class="layer-container">
    <div class="form-row form-row-word">
        <div class="head">订 单 号 ：</div>
        <%= order.OrderNumber %>
    </div>
    <div class="form-row form-row-word">
        <div class="head">订单状态：</div>
        <%= OrderBLL.ReadOrderStatus(order.OrderStatus,order.IsDelete) %>
    </div>
    <div class="form-row form-row-word">
        <div class="head">费用信息：</div>
        	  商品金额：<%=order.ProductMoney %> 元
              <% if(order.ShippingMoney > 0) {%>  + 运费：<%=order.ShippingMoney %> 元 <%} %>
              <% if(order.OtherMoney != 0) {%>  + 其它费用：<%=order.OtherMoney %> 元 <%} %>
              <% if(order.PointMoney > 0) {%>  - 积分抵扣金额：<%=order.PointMoney %> 元 <%} %>
             = 需支付金额：<b style="color:#B20000"><%=(OrderBLL.ReadOrderMoney(order) - order.PointMoney)%></b> 元

        <% if(orderDetail.Id < 1 && orderRefundList.Count > 0) {%>
            <br />
            <b>
                <% var _orderRefund = orderRefundList.FirstOrDefault(k => k.OrderDetailId < 1); %>
                <% if(_orderRefund != null) {%>
                    <% if(OrderRefundBLL.HasReturn(_orderRefund.Status)) {%>退款已完成<%} else {%>正在处理退款<%} %>
                <%} else {%>
                    <%
                        int _returningCount = orderRefundList.Count(k => OrderRefundBLL.CanToReturn(k.Status));
                        int _hasReturnCount = orderRefundList.Count(k => OrderRefundBLL.HasReturn(k.Status));
                    %>
                    <% if(_returningCount > 0) {%>
                        有<span style="color:#B20000"><%= _returningCount %></span>个商品正在退货中，
                    <%} %>
                    <% if(_hasReturnCount > 0) {%>
                        有<span style="color:#B20000"><%= _hasReturnCount %></span>个商品已退货，
                    <%} %>
                    本次最多可退<span style="color:#B20000"><%= JWRefund.CanRefund(order).CanRefundMoney %></span>元
                <%} %>
            </b>
        <%} %>
    </div>
    <% if(orderDetail.Id > 0) {%>
        <div class="form-row">
            <div class="head">商品信息：</div>
            <span title="<%=orderDetail.ProductName %>"><%= StringHelper.Substring(orderDetail.ProductName, 20) %></span><br />
            单价：<%= orderDetail.ProductPrice %>　X<%= orderDetail.BuyCount %>(数量)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            小计：<b style="color:#B20000"><%= orderDetail.ProductPrice * orderDetail.BuyCount %></b> 元

            <% var _orderDetailRefundList = orderRefundList.Where(k => k.OrderDetailId == orderDetail.Id).ToList();%>
            <br />
            <b>
                <% if(_orderDetailRefundList.Count > 0) {%>
                    <%
                        int _returningCount = _orderDetailRefundList.Count(k => OrderRefundBLL.CanToReturn(k.Status));
                        int _hasReturnCount = _orderDetailRefundList.Count(k => OrderRefundBLL.HasReturn(k.Status));
                    %>
                    <% if(_returningCount > 0) {%>
                        有<span style="color:#B20000"><%= _returningCount %></span>个商品正在退货中，
                    <%} %>
                    <% if(_hasReturnCount > 0) {%>
                        有<span style="color:#B20000"><%= _hasReturnCount %></span>个商品已退货
                    <%} %>
                <%} %>
                
            </b>
        </div>
        <div class="form-row">
            <div class="head">数量：</div>
            <SkyCES:TextBox ID="RefundCount" CssClass="txt" runat="server" Width="50px" RequiredFieldType="数据校验" Text="1" org-text="1" />
            <span>&nbsp;&nbsp;&nbsp;本次最多可退<span style="color:#B20000"><%= orderDetail.BuyCount - orderDetail.RefundCount %></span>个商品</span>
        </div>
    <%} %>
    <div class="form-row">
        <div class="head">金额：</div>
        <SkyCES:TextBox ID="RefundMoney" CssClass="txt" runat="server" Width="100px" RequiredFieldType="金额" />
    </div>
    <div class="form-row">
        <div class="head">退款说明：</div>
        <SkyCES:TextBox ID="RefundRemark" CssClass="txt" runat="server" Width="300px" TextMode="MultiLine" Height="60px" />
    </div>
    <div class="form-foot">
        <asp:Button CssClass="form-submit ease" Style="margin: 0;" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" OnClientClick="return confirm('确认退款吗？');" />
    </div>
</div>
    

    <script>
        var orderId = '<%=order.Id%>';
        var orderDetailId = '<%=orderDetail.Id%>';
        var canRefundMoney = '<%=canRefundMoney%>';

        $("#ctl00_ContentPlaceHolder_RefundCount").change(function () {
            var that = $('#ctl00_ContentPlaceHolder_RefundCount');
            var num = $(that).val();
            if (num < 1) {
                layer.msg('退款商品数量必须大于1');
                $(that).val($(that).attr('org-text'));
                return;
            }

            $.ajax({
                url: 'OrderRefundApply.aspx?Action=CalcCanRefundMoney',
                type: 'GET',
                data: { 'orderId' : orderId, 'orderDetailId' : orderDetailId, 'num' : num},
                success: function (result) {
                    var json = JSON.parse(result);

                    if (json.CanRefund) {
                        $(that).attr('org-text', num);
                        $('#ctl00_ContentPlaceHolder_RefundMoney').val(json.CanRefundMoney);
                        canRefundMoney = json.CanRefundMoney;
                    }
                    else {
                        layer.msg(json.ErrorCodeMsg);
                        $(that).val($(that).attr('org-text'));
                    }
                }
            });
        });

        $("#ctl00_ContentPlaceHolder_RefundMoney").change(function () {
            var that = $("#ctl00_ContentPlaceHolder_RefundMoney");
            var money = $(that).val();
            if (money <= 0) {
                layer.msg('退款金额必须大于0');
                $(that).val(canRefundMoney);
                return;
            }
            if (money > canRefundMoney) {
                $(that).val(canRefundMoney);
                layer.msg('本次最多退款 ' + canRefundMoney + ' 元');
                return;
            }
        });
    </script>
</asp:Content>