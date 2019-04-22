<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="OrderYeji.aspx.cs" Inherits="JWShop.Web.Admin.OrderYeji" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<asp:Content Id="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <link rel="stylesheet" href="/Admin/Js/jqdate/base/jquery.ui.all.css">    
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.core.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <link rel="stylesheet" href="/Admin/Js/jqdate/demos.css">
    <style>
        .tab-title span {
            width: 100px;
        }
        .product-filter dd{ margin-right: 12px; }
    </style>
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_StartAddDate").datepicker({ changeMonth: true, changeYear: true });
            $("#ctl00_ContentPlaceHolder_EndAddDate").datepicker({ changeMonth: true, changeYear: true });
        });
    </script>

    <div class="container ease" id="container">
        <div class="product-container" style="padding-top: 22px;">	
            <dl class="product-filter product-filter-pro clearfix">
                <dd style="display:none;">
                    <div class="head">订单类型：</div>
                    <asp:DropDownList ID="SelfPick" runat="server" CssClass="select">
                        <asp:ListItem Value="">所有</asp:ListItem>
                        <asp:ListItem Value="0">配送</asp:ListItem>
                        <asp:ListItem Value="1">自提</asp:ListItem>                     
                    </asp:DropDownList>
                </dd>
                <dd>
                    <div class="head">订单号：</div>
                    <SkyCES:TextBox CssClass="txt" Id="OrderNumber" runat="server" Width="130px"/>
                </dd>
                <dd>
                    <div class="head">客户姓名：</div>
                    <SkyCES:TextBox CssClass="txt" Id="Consignee" runat="server" Width="80px"/>
                </dd>      
                <dd>
                    <div class="head">下单时间：</div>
                    <span class="tp">从</span><SkyCES:TextBox CssClass="txt" ID="StartAddDate" runat="server" RequiredFieldType="日期时间" width="70px" /> <span class="tp">到</span> <SkyCES:TextBox CssClass="txt" ID="EndAddDate" runat="server" RequiredFieldType="日期时间" width="70px" />
                </dd>                             
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <table class="product-list" cellpadding="0" cellspacing="0" border="0" width="100%">
                <thead>
                    <tr>       
	                    <td width="50">Id</td>
	                    <td width="150" >订单号</td>     
	                    <td width="100">订单金额</td>
	                    <td width="100">客户姓名</td>
	                    <td width="100">客户电话</td>
	                    <td width="100">订单状态</td> 	       
	                    <td width="150">下单时间</td>             
	                    <td width="150">最近操作时间</td>     
	                    <td width="100">管理</td>         
                    </tr>
                </thead>
                <tbody>
                    <tr class="firstH">
                    	<td colspan="10" style="padding: 0;">
                        	<div class="button">
	                             <%if(Count>0){ %>        
                               <asp:Button CssClass="button-3" ID="Button1" Text=" 导 出 " runat="server" OnClick="ExportButton_Click" />
                                &nbsp;&nbsp;每页显示：<asp:DropdownList ID="AdminPageSize" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="AdminPageSize_SelectedIndexChanged">
                                    <asp:ListItem Value="20">20条</asp:ListItem>
                                    <asp:ListItem Value="50">50条</asp:ListItem>
                                    <asp:ListItem Value="100">100条</asp:ListItem>
                                     </asp:DropdownList>
                                 <%} %>                               
                                <span style="float: left;">共找到<%=Count %>条记录<%if(Count>0){ %> ，<%=MyPager.PageCount %>页<%} %></span>
                            </div>                          
                    	</td>
                    </tr>
                    <asp:Repeater Id="RecordList" runat="server">
	                    <ItemTemplate>	     
                            <tr>
			                    <td><%# Eval("Id") %></td>
			                    <td><%# Eval("OrderNumber") %></td>     
	                            <td><%# Convert.ToDecimal(Eval("ProductMoney")) + Convert.ToDecimal(Eval("ShippingMoney")) + Convert.ToDecimal(Eval("OtherMoney"))-Convert.ToDecimal(Eval("CouponMoney"))-Convert.ToDecimal(Eval("PointMoney"))-Convert.ToDecimal(Eval("FavorableMoney"))%></td>
	                            <td><%# Eval("Consignee")%></td>
	                            <td><%# Eval("Mobile")%></td> 
	                            <td><%# OrderBLL.ReadOrderStatus(Convert.ToInt32(Eval("OrderStatus")),Convert.ToInt32(Eval("IsDelete")))%></td> 	         
	                            <td><%# Eval("AddDate") %></td>   
                                <td><%# Eval("InvoiceTitle") %></td>   
			                    <td class="link"><a href='OrderDetail.aspx?Id=<%# Eval("Id") %>'>查看</a> 
                                    <%#Eval("IsDelete").ToString()=="1"?"| <a href=\"javascript:void(0)\" onclick=\"recoverOrder("+Eval("ID")+")\">恢复</a>":"| <a href=\"?Action=delete&orderId="+Eval("Id")+"\" >删除</a>" %></td>				        
		                    </tr>
                            </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="10">
                        	<div class="button">
                                <%if(Count>0){ %>
                                <asp:Button CssClass="button-3" ID="ExportButton" Text=" 导 出 " runat="server" OnClick="ExportButton_Click" />
                                <%} %>                                
                            </div>
                            <SkyCES:CommonPager ID="MyPager" runat="server" />                            
                            <div class="clear"></div>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
    <script language="javascript" type="text/javascript" src="/Admin/js/Order.js"></script>
</asp:Content>
