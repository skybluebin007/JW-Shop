<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="PointProductOrder.aspx.cs" Inherits="JWShop.Web.Admin.PointProductOrder" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Entity" %>
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
            <asp:HiddenField ID="hOrderStatus" runat="server" />
            <span <%if(orderStatus == int.MinValue){%>class="cur"<%} %>><a href="PointProductOrder.aspx">所有</a></span>
            <% foreach (var item in EnumHelper.ReadEnumList<PointProductOrderStatus>()) {%>
                <span <%if(orderStatus == item.Value){%>class="cur"<%} %>><a href="PointProductOrder.aspx?Action=search&orderStatus=<%= item.Value %>"><%= item.ChineseName %></a></span>
            <%} %>
        </div>
        <div class="product-container" style="padding:10px 0;">
            <dl class="product-filter clearfix">
                <dd>
                    <div class="head">商品名称：</div>
                    <SkyCES:TextBox CssClass="txt" ID="ProductName" runat="server" />
                </dd>
                <dd>
                    <div class="head">兑换编号：</div>
                    <SkyCES:TextBox CssClass="txt" ID="OrderNumber" runat="server" />
                </dd>
                <dd>
                    <div class="head">会员：</div>
                    <SkyCES:TextBox CssClass="txt" ID="UserName" runat="server" Width="60px" />
                </dd>
                <dd>
                    <div class="head">下单时间：</div>
                    <SkyCES:TextBox CssClass="txt" ID="StartAddDate" runat="server" width="70" /> <span class="tp">到</span>  <SkyCES:TextBox CssClass="txt" ID="EndAddDate" runat="server" width="70" />
                </dd>
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <table class="product-list">
                <thead>
                    <tr>
	                    <td style="width:10%;">兑换编号</td>
	                    <td style="width:10%;">状态</td>
	                    <td style="width:15%; text-align: left;">商品名称</td>
	                    <td style="width:7%">数量</td>
	                    <td style="width:15%">下单时间</td>
	                    <td style="width:15%">会员</td>
	                    <td style="width:8%">消费积分</td>
	                    <td style="width:20%">管理</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
	                    <ItemTemplate>	     
                            <tr>
			                    <td><%# Eval("OrderNumber") %></td>
			                    <td><%# EnumHelper.ReadEnumChineseName<PointProductOrderStatus>(Convert.ToInt32(Eval("OrderStatus"))) %></td>
	                            <td style="text-align: left;"><%# Eval("ProductName") %></td>
                                <td><%# Eval("BuyCount") %></td>
                                <td><%# Eval("AddDate") %></td>
                                <td><%# Eval("UserName") %></td>
                                <td><%# Eval("Point") %></td>
			                    <td class="link">
                                    <input type="hidden" name="OrderId" value="<%# Eval("Id") %>" />
                                    <input type="hidden" name="Consignee" value="<%# Eval("Consignee") %>" />
                                    <input type="hidden" name="Tel" value="<%# Eval("Tel") %>" />
                                    <input type="hidden" name="Region" value="<%# RegionBLL.RegionNameList(Eval("RegionId").ToString()) %>" />
                                    <input type="hidden" name="Address" value="<%# Eval("Address") %>" />
                                    <input type="hidden" name="ShippingName" value="<%# Eval("ShippingName") %>" />
                                    <input type="hidden" name="ShippingNumber" value="<%# Eval("ShippingNumber") %>" />
                                    <input type="hidden" name="Point" value="<%# Eval("Point") %>" />
                                    <a href="javascript:;" onclick="view('view', this);">查看</a>&nbsp;&nbsp;
                                    <a href="javascript:;" onclick="view('shipping', this);" style="display:<%# (Eval("OrderStatus").ToString() == ((int)PointProductOrderStatus.Shipping).ToString() ? "" : "none" )%>">发货</a>&nbsp;
                                    <a href="javascript:;" onclick="cancel(this);" style="display:<%# (Eval("OrderStatus").ToString() == ((int)PointProductOrderStatus.Shipping).ToString() ? "" : "none" )%>">取消兑换</a>
			                    </td>
		                    </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="8">
                            <SkyCES:CommonPager ID="MyPager" runat="server" />
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>

    <script>
        function view(action, that) {
            that = $(that).parent();
            var _order_id = that.find('input[name=OrderId]').val();
            var _consignee = that.find('input[name=Consignee]').val();
            var _tel = that.find('input[name=Tel]').val();
            var _region = that.find('input[name=Region]').val();
            var _address = that.find('input[name=Address]').val();
            var _shipping_name = that.find('input[name=ShippingName]').val();
            var _shipping_number = that.find('input[name=ShippingNumber]').val();

            var _html = '<div> 收 货 人 ：' + _consignee + '</div>';
            _html += '<div style="padding-top: 5px;">联系电话：' + _tel + '</div>';
            _html += '<div style="padding-top: 5px;">收货地址：' + _region + ' ' + _address + '</div>';

            if (action == 'view') {
                _html += '<div style="padding-top: 5px;">快递公司：' + _shipping_name + '</div>';
                _html += '<div style="padding-top: 5px;">快递单号：' + _shipping_number + '</div>';
            }
            else {
                _html += '<div style="padding-top: 5px;">快递公司：<input type="text" id="ShippingName" style="border: 1px solid #DDD;height: 24px;width: 200px;" /></div>';
                _html += '<div style="padding-top: 5px;">快递单号：<input type="text" id="ShippingNumber" style="border: 1px solid #DDD;height: 24px;width: 200px;" /></div>';
            }

            layer.open({
                title: action == 'view' ? '查看' : '发货',
                area: ['420px', '300px'],
                content: _html,
                btn: [action == 'view' ? '关闭' : '发货'],
                yes: function (index) {
                    if (action == 'shipping') {
                        _shipping_name = $('#ShippingName').val();
                        _shipping_number = $('#ShippingNumber').val();
                        if ($.trim(_shipping_name) == '' || $.trim(_shipping_number) == '') return false;

                        $.ajax({
                            url: '/Admin/PointProductOrder.aspx?Action=Shipping',
                            type: 'POST',
                            data: { OrderId: _order_id, ShippingName: _shipping_name, ShippingNumber: _shipping_number },
                            success: function () {
                                layer.msg('发货成功', { time: 1000 }, function () {
                                    window.location.reload();
                                });
                            }
                        });
                    }
                    else {
                        layer.close(index);
                        return true;
                    }
                }
            });
        }

        function cancel(that) {
            that = $(that).parent();
            var _order_id = that.find('input[name=OrderId]').val();
            var _point = that.find('input[name=Point]').val();
            layer.confirm('确认取消吗？将退还用户' + _point + '积分', function (index) {
                $.ajax({
                    url: '/Admin/PointProductOrder.aspx?Action=Cancel',
                    type: 'GET',
                    data: { OrderId: _order_id },
                    success: function () {
                        layer.msg('取消成功', { time: 1000 }, function () {
                            window.location.reload();
                        });
                    },
                    error: function () {
                        layer.msg('取消失败');
                        layer.close(index);
                    }
                });
            });
        }
    </script>
</asp:Content>
