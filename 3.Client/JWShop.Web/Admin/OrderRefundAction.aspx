<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="OrderRefundAction.aspx.cs" Inherits="JWShop.Web.Admin.OrderRefundAction" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <style>
body { width:100% !important; min-width:auto !important; }
</style>
    <div class="container ease" id="container" style="left: 0; min-width: auto; ">
        <% foreach(var orderRefund in orderRefundList) {%>
            <div class="product-container product-container-border">
                <div class="form-row">
                    <div class="head">申请时间：</div>
                    <%= orderRefund.TmCreate %>
                </div>
                <div class="form-row">
                    <div class="head">退款金额：</div>
                    <% if(orderRefund.RefundBalance > 0) {%>余额：<%= orderRefund.RefundBalance %> <%} %>
                    <% if(orderRefund.RefundMoney > 0) {%><%=(orderRefund.RefundBalance > 0 ? "+" : "")%> <%= orderRefund.RefundPayName %>： <%= orderRefund.RefundMoney %> <%} %>
                    = <%= (orderRefund.RefundBalance + orderRefund.RefundMoney) %>
                </div>
                <div class="form-row">
                    <div class="head">退款说明：</div>
                    <%= orderRefund.RefundRemark %>
                </div>
                <% var orderRefundActionList = OrderRefundActionBLL.ReadList(orderRefund.Id); %>
                <%if(orderRefundActionList.Count > 0){ %>
                    <table class="product-list" style="margin-top: 5px;">
                        <thead>
                            <tr>
                                <td style="width:18%">处理人</td>
                                <td style="width:10%">处理状态</td>
	                            <td style="width:50%">备注</td>
	                            <td style="width:22%">处理时间</td>
                            </tr>
                        </thead>
                        <tbody>
                            <%foreach (var orderRefundAction in orderRefundActionList){ %>
                                <tr>
	                                <td><%= orderRefundAction.UserName %></td>
                                    <td><%= ShopCommon.GetBoolString(orderRefundAction.Status) %></td>
	                                <td><%= orderRefundAction.Remark %></td>
	                                <td><%= orderRefundAction.Tm %></td>
                                </tr>
	                        <%} %>
                        </tbody>
                    </table>
                <%} %>
            </div>
        <%} %>
    </div>
</asp:Content>
