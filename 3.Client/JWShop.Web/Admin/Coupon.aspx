<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="Coupon.aspx.cs" Inherits="JWShop.Web.Admin.Coupon" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
        <div class="product-container" style="padding: 24px 0 50px;">
                    <div class="tab-title" style="margin: 30px 15px -1px 15px;">
          
            <span style="display:none;" <%if ( RequestHelper.GetQueryString<int>("timeperiod") <= 0)
                    { %>class="cur"
                <%} %>><a href="coupon.aspx?couponkind=<%=couponKind%>">所有</a></span>
            <span <%if (timeperiod== 1)
                    { %>class="cur"
                <%} %>><a href="coupon.aspx?timeperiod=1&couponkind=<%=couponKind%>">未开始</a></span>
            <span <%if (timeperiod== 2)
                    { %>class="cur"
                <%} %>><a href="coupon.aspx?timeperiod=2&couponkind=<%=couponKind%>">进行中</a></span>
            <span <%if (timeperiod== 3)
                    { %>class="cur"
                <%} %>>
                <a href="coupon.aspx?timeperiod=3&couponkind=<%=couponKind%>">已结束</a></span>
          </div>
            <div class="add-button newbtn"><a href="CouponAdd.aspx?couponkind=<%=couponKind%>" title="添加新数据" class="ease"> 添 加 </a></div>
            <table class="product-list">
                <thead>
                    <tr>
	                    <td style="width:5%">选择</td>
                        <td style="width:5%">Id</td>
	                    <td style="width:25%;">优惠券</td>
                        <td style="width:13%">金额</td>
                        <td style="width:15%">最小订单产品金额</td>
                        <td style="width:22%">使用时间</td>
	                    <td style="width:15%">管理</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("Id") %>" ig-bind="list" /></label></td>
                                <td><%# Eval("Id") %></td>
			                    <td><%# Eval("Name") %></td>
                                <td><%#Eval("Money") %></td>  
                                <td><%#Eval("UseMinAmount")%></td>  
                                <td><%#Eval("UseStartDate","{0:yyyy-MM-dd}")%> 到 <%#Eval("UseEndDate","{0:yyyy-MM-dd}")%></td>   
			                    <td class="link">
                                    <!--如果优惠券启用日期还没开始-->
                              <a href="CouponAdd.aspx?ID=<%# Eval("Id")%>&couponkind=<%=couponKind%>" >修改</a>                                  
                                 
                                    <a href="javascript:loadPage('UserCoupon.aspx?CouponID=<%# Eval("Id") %>','用户优惠券','950px','500px')">用户优惠券</a>
                                     <!--如果优惠券试用期已过期-->
                                     <%#ShowSendhref(Eval("Id")) %>
			                    </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="7">
                            <div class="button">
                                <label class="ig-checkbox"><input type="checkbox" value="" class="checkall" bind="list" />全选</label>
                                <asp:Button CssClass="button-2 del" ID="Button1" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server" OnClick="DeleteButton_Click" />
                            </div>
                            <SkyCES:CommonPager ID="MyPager" runat="server" />
                            <div class="clear"></div>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>

    <script type="text/javascript">
        function loadPage(url, title, width, height) {
            layer.open({
                type: 2,
                //skin: 'layui-layer-lan',
                title: title,
                fix: false,
                shadeClose: true,
                maxmin: false,
                area: [width, height],
                content: url,
                end: function () {//layer关闭时刷新页面

                    window.location.reload();
                }
            });
        }
    </script>
</asp:Content>
