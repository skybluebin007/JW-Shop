<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="OrderRefund.aspx.cs" Inherits="JWShop.Web.Admin.OrderRefund" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
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
            <span <%if(intStatus==int.MinValue){%>class="cur"<%} %>><a href="OrderRefund.aspx">所有记录</a></span>
            <% foreach(var enumInfo in enumList) {%>
                <span <%if (intStatus == enumInfo.Value){%>class="cur"<%} %> style="width: 100px;"><a href="OrderRefund.aspx?Status=<%=enumInfo.Value %>"><%=enumInfo.ChineseName %></a></span>
            <%} %>
        </div>
        <div class="product-container" style="padding:10px 0;">
            <dl class="product-filter clearfix">
                <dd>
                    <div class="head">退款编号：</div>
                    <SkyCES:TextBox CssClass="txt" ID="RefundNumber" runat="server"  Width="100px"/>
                </dd>
                <dd>
                    <div class="head">订单号：</div>
                    <SkyCES:TextBox CssClass="txt" ID="OrderNumber" runat="server"  Width="100px"/>
                </dd>
                <dd>
                    <div class="head">状态：</div>
                    <asp:DropDownList ID="Status" runat="server" CssClass="select" />
                </dd>      
                <dd>
                    <div class="head">申请时间：</div>
                    <span class="tp">从</span><SkyCES:TextBox CssClass="txt" ID="StartAddDate" runat="server" RequiredFieldType="日期时间" width="70px" /> <span class="tp">到</span> <SkyCES:TextBox CssClass="txt" ID="EndAddDate" runat="server" RequiredFieldType="日期时间" width="70px" />
                </dd>                             
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
        
            <table class="product-list" cellpadding="0" cellspacing="0" border="0" width="100%">
            <thead>
                <tr> 
	                <td style="width:5%">Id</td>
	                <td style="width:12%; text-align:left;text-indent:8px;">退款编号</td>
	                <td style="width:12%; text-align:left;text-indent:8px;">订单号</td>
	                <td style="width:10%">退款金额</td>
	                <td style="width:8%">状态</td>
	                <td style="width:12%">申请时间</td>
	                <td style="width:12%">退款时间</td>
	                <td style="width:22%">退款说明</td>
	                <td style="width:5%">管理</td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater Id="RecordList" runat="server">
	                <ItemTemplate>	     
                        <tr>
                            <td><%# Eval("Id") %></td>
			                <td><%# Eval("RefundNumber") %></td>
			                <td><%# Eval("OrderNumber") %></td>
	                        <td><%# Eval("RefundMoney")%></td>
	                        <td><%# EnumHelper.ReadEnumChineseName<OrderRefundStatus>(Convert.ToInt32(Eval("Status")))%></td>
	                        <td><%# Eval("TmCreate") %></td>
	                        <td><%# Eval("TmRefund") %></td>
	                        <td><%# StringHelper.Substring(Eval("RefundRemark").ToString(), 8) %></td>
			                <td class="link">
                                <a href="OrderRefundAdd.aspx?Id=<%# Eval("Id") %>">查看</a>
			                </td>
		                </tr>
                        </ItemTemplate>
                </asp:Repeater>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="9">
                        <SkyCES:CommonPager ID="MyPager" runat="server" />                            
                        <div class="clear"></div>
                    </td>
                </tr>
            </tfoot>
        </table>
        </div>
    </div>
</asp:Content>
