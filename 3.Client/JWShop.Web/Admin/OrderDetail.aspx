<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="OrderDetail.aspx.cs" Inherits="JWShop.Web.Admin.OrderDetail" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<asp:Content Id="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <link rel="stylesheet" href="/Admin/Js/jqdate/base/jquery.ui.all.css">    
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.core.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <link rel="stylesheet" href="/Admin/Js/jqdate/demos.css">
 <%--signalr--%>
        <script src="/admin/Scripts/jquery.signalR-1.2.2.min.js"></script>
     <script src="/Signalr/Hubs"></script>  
        <script>
        $.connection.hub.logging = true;
        $.connection.hub.start();
    </script>
     <%--signalr--%>
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_ShippingDate").datepicker({ changeMonth: true, changeYear: true });
        });
    </script>
    <style type="text/css">
        .product-tip { display: block; }
        .product-head { display: block; padding-left: 50px; }
        .product-row { display: block; }
		.product-head{ padding:0;}
		.form-row .head{ text-align:left; left:0;}
		#orderinfo_head { background:#fff; border-top:4px solid #e80012; padding:20px 40px; overflow:hidden;}
		#orderinfo_head .product-row{ float:left; width:33.33%;}
		#orderinfo_head .form-row{ margin-bottom:0;}
		#orderinfo_bottom{background:#fff;padding:20px 40px; overflow:hidden; margin-top:40px;}
    </style>

    <div class="container ease" id="container" style="background:#f8f8f8;">
        <div style="text-align:right; padding: 20px 50px 0; margin-bottom: 20px;">   
            <%=ReadPreNextOrderString(order.Id) %>  
            <input type="button" class="btn-button" value="网页打印" onclick="window.open('OrderPrint.aspx?Action=Html&OrderId=<%=order.Id %>')" />  
            <input type="button" class="btn-button" value="Excel打印" onclick="window.open('OrderPrint.aspx?Action=Excel&OrderId=<%=order.Id %>')"/>
            <input type="button" class="btn-button" value="导出" onclick="window.open('OrderPrint.aspx?Action=ExportSingle&OrderId=<%=order.Id %>    ')"/>
        </div>
        <div class="product-container product-container-border" style="border:none;">
        	<div id="orderinfo_head">
                <div class="product-row" md="cell-2">
                    <div class="product-head">基本信息</div>
                    <div class="product-main">
                        <div class="form-row">
                            <div class="head">订单号：</div>
                            <%=order.OrderNumber %><%if(order.IsActivity==(int)BoolType.True){ %>（活动订单）<%} %>
                        </div>
                        <div class="form-row">
                            <div class="head">订单状态：</div>
                            <%=OrderBLL.ReadOrderStatus(order.OrderStatus,order.IsDelete) %> <%if (order.OrderStatus == (int)OrderStatus.HasShipping && order.IsDelete == (int)BoolType.False){%>&nbsp;&nbsp;<a href="javascript:loadPage('<%=shippingLink%>','查看物流','600px','300px')" style="text-decoration:underline; color:#2679D8">查看物流</a><%} %>
                            <% if(orderRefundList.Count > 0) {%><b style="color:#BF0000">（该订单被退款或包含退款商品...）</b><a href="javascript:loadPage('OrderRefundAction.aspx?orderId=<%=order.Id %>','退款记录','950px','500px')">查看退款记录</a><%} %>
                        </div>
                        <div class="clear"></div>
                        <div class="form-row">
                            <div class="head">支付方式：</div>
                            <%=order.PayName%>
                        </div>
                        
                        <div class="clear"></div>
                        <div class="form-row">
                            <div class="head">下单时间：</div>
                            <%=order.AddDate%>
                        </div>
                        <%if(order.AddDate!=order.PayDate){ %>
                            <div class="form-row">
                                <div class="head">付款时间：</div>
                                <%=order.PayDate %>
                            </div>
                        <%} %>
                        <div class="clear"></div>
                    </div>
                </div>
                <!--收货方式不是自提，才显示邮寄信息-->
                <%-- <%if (order.SelfPick!=1){%>--%>
                <div class="product-row" md="cell-2">
                    <div class="product-head">客户信息 <% if (canEdit){ %><a href="javascript:loadPage('OrderAdd.aspx?Id=<%=order.Id %>&Action=Shipping','信息修改', '600px', '600px')" style="color:#2679D8">修改</a><%} %></div>
    
                    <div class="product-main">
                        <div class="form-row">
                            <div class="head">客户账号：</div>
                            <%=getusername(order.UserId) %>
                        </div>
                        <div class="form-row">
                            <div class="head">客户姓名：</div>
                            <%=order.Consignee %>
                        </div>
                        <div class="form-row">
                            <div class="head">手机号/电话：</div>
                            <%=order.Mobile %>&nbsp;/&nbsp;<%=order.Tel %>
                        </div>
                        <div class="form-row">
                            <div class="head">所在地区：</div>
                            <%=RegionBLL.RegionNameList(order.RegionId)%>
                        </div>
                        <div class="clear"></div>
                        <div class="form-row">
                            <div class="head">详细地址：</div>
                            <%=order.Address %>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <%--<%} %>--%>

                <div class="product-row">
                    <div class="product-head">经销商/安装/试压人员信息</div>
                    <div class="product-main">
                        <div class="form-row">
                            <div class="head">经销商账号：</div>
                            <%=getusername(order.Saleid) %>
                        </div>
                        <div class="form-row">
                            <div class="head">安装用户：</div>
                            <%=getusername(order.Shuigongid) %>
                        </div>
                        <div class="form-row">
                            <div class="head">安装人员姓名：</div>
                            <%=order.Shuigong_name %>
                        </div>
                        <div class="form-row">
                            <div class="head">安装人员电话：</div>
                            <%=order.Shuigong_tel %>
                        </div>

                        <div class="form-row">
                            <div class="head">试压用户：</div>
                            <%=getusername(order.Shiyaid) %>
                        </div>
                        <div class="form-row">
                            <div class="head">试压人员姓名：</div>
                            <%=order.Shiya_name %>
                        </div>
                        <div class="form-row">
                            <div class="head">试压人员电话：</div>
                            <%=order.Shiya_tel %>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>

                <div class="product-row" style="display:none;" >
                    <div class="product-head">其他信息 <% if (canEdit){ %><a href="javascript:loadPage('OrderAdd.aspx?Id=<%=order.Id %>&Action=Other','其他信息修改', '600px', '300px')" style="color:#2679D8">修改</a><%} %></div>
                    <div class="product-main">
                        <div class="form-row">
                            <div class="head">发票标题：</div>
                            <%=order.InvoiceTitle %>
                            </div>
                        <div class="clear"></div>
                        <div class="form-row">
                            <div class="head">发票内容：</div>
                            <%=order.InvoiceContent %>
                        </div>
                        <div class="clear"></div>
                        <div class="form-row">
                            <div class="head">用户留言：</div>
                            <%=order.UserMessage %>
                        </div>
                        <div class="clear"></div>
                        <div class="form-row" style="display:none;">
                            <div class="head">礼品贺卡留言：</div>
                            <%=order.GiftMessige%>
                        </div>
                        <div class="clear"></div>
                        <%if(order.GiftId>0){ %>
                         <div class="form-row">
                            <div class="head">礼品信息：</div>
                            <%=FavorableActivityGiftBLL.Read(order.GiftId).Name%>
                        </div><%} %>
                        <div class="clear"></div>
                        <%if(order.OrderNote!=string.Empty){ %>
                        <div class="form-row">
                            <div class="head">订单备注：</div>
                            <%=order.OrderNote %>
                        </div>
                        <div class="clear"></div>
                        <%} %>
                    </div>
                </div>
            </div>
            <div id="orderinfo_bottom">
                <div class="product-row" id="OrderAjax" > 
                  </div>
                <div class="product-row" id="" >
                    <div class="product-head">订单操作</div>
                    <div class="product-main">
                        <div class="form-row" style="padding-left: 90px;">
                            <div class="head" style="width: 80px;">备注：</div>
                            <SkyCES:TextBox CssClass="txt" Width="400px" ID="Note" runat="server" TextMode="MultiLine" Height="50px"/>
                        </div>
                        <div class="clear"></div>
                        <%if (SendButton.Visible){ %>
                            <div class="form-row">
                                <div class="head">配送日期：</div>
                                <SkyCES:TextBox Id="ShippingDate" runat="server" CssClass="txt" />
                            </div>
                            <div class="form-row">
                                <div class="head">配送单号：</div>
                                <SkyCES:TextBox Id="ShippingNumber" runat="server" CssClass="txt" />
                            </div>
                            <div class="clear"></div>
                        <%} %>
                        
                        <%if(orderActionList.Count>0){ %>
                            <div class="clear"></div>
                            <table class="product-list" style="margin-top: 5px;">
                                <thead>
                                    <tr>
                                        <td style="width:15%">订单操作</td>
                                        <td style="width:30%">订单状态变化</td>
                                        <td style="width:15%">操作时间</td>
                                        <td style="width:10%">操作人</td>
                                        <td style="width:30%">备注</td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <%foreach (OrderActionInfo orderAction in orderActionList){ %>
                                        <tr>
                                            <td style="width:15%"><%=OrderActionBLL.ReadOrderOperate(orderAction.OrderOperate) %></td>
                                            <td style="width:30%"><%=OrderBLL.ReadOrderStatus(orderAction.StartOrderStatus) %> ——> <%=OrderBLL.ReadOrderStatus(orderAction.EndOrderStatus) %></td>
                                            <td style="width:15%"><%=orderAction.Date %></td>
                                            <td style="width:10%"><%=orderAction.AdminName %></td>
                                            <td style="width:30%"><%=orderAction.Note %></td>
                                        </tr>
                                    <%} %>
                                </tbody>
                            </table>
                        <%} %>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="form-foot">
            <div id="divOperate" runat="server">
                <%--<asp:Button CssClass="form-submit" Id="PayButton" runat="server" Text=" 付 款 " Visible="false" OnClick="PayButton_Click"  />--%>
                <asp:Button CssClass="form-submit" Id="CheckButton" runat="server" Text=" 审 核 " Visible="false" OnClick="CheckButton_Click"  />		    
                <asp:Button CssClass="form-cancel" Id="CancelButton" runat="server" Text=" 取 消 "  Visible="false" OnClick="CancelButton_Click" OnClientClick="return cancel();" />
                <asp:Button CssClass="form-submit" Id="AnzhuangButton" runat="server" Text="安装完成"  Visible="false" OnClick="AnzhuangButton_Click" />
                <asp:Button CssClass="form-submit" Id="SendButton" runat="server" Text="提交试压"  Visible="false" OnClick="SendButton_Click" OnClientClick="return send()"/>
                <asp:Button CssClass="form-submit" Id="ReceivedButton" runat="server" Text="确认完全"  Visible="false" OnClick="ReceivedButton_Click"  OnClientClick="return received()"/>
                <asp:Button CssClass="form-submit" Id="ChangeButton" runat="server" Text="换货确认"  Visible="false" OnClick="ChangeButton_Click" />
                <asp:Button CssClass="form-submit" Id="ReturnButton" runat="server" Text="退货确认"  Visible="false" OnClick="ReturnButton_Click" />
                <asp:Button CssClass="form-submit" Id="RecoverButton" runat="server" Text=" 恢 复 "  Visible="false" OnClick="RecoverButton_Click" />
                
                <asp:Button CssClass="form-cancel" Id="BackButton" runat="server" Text=" 撤 销 " Visible="false"  OnClick="BackButton_Click" OnClientClick="return back()"/> 
            </div>
            <div id="divNotice" runat="server" visible="false">
                <asp:Label ID="lblNotice" runat="server" Font-Bold="true" ForeColor="#BF0000"></asp:Label>
            </div>

        </div>
    </div>

    <script type="text/javascript" src="/Admin/Js/OrderDetail.js"></script>
    <script type="text/javascript">    
        var sendPoint="<%=sendPoint %>";
        var isActivity="<%=order.IsActivity%>";
        var orderStatus="<%=order.OrderStatus %>";
        $(function(){
            showProduct(<%=order.Id %>);
        });

        function loadPage(url, title, width, height){
            layer.open({
                type: 2,
                //skin: 'layui-layer-lan',
                title: title,
                fix: false,
                shadeClose: true,
                maxmin: false,
                area: [width, height],
                content: url,
                end: function(){//layer关闭时刷新
              
                    window.location.reload();
                }
            });
        }
    </script>
</asp:Content>
