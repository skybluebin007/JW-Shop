<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="UserCoupon.aspx.cs" Inherits="JWShop.Web.Admin.UserCoupon" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Entity" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<style>
body { width:100% !important; min-width:auto !important; }
</style>
    <div class="container ease" id="container" style="left: 0; min-width: auto; ">
        <div class="product-container" style="padding-top: 20px; margin:0 15px !important;">
            <dl class="product-filter clearfix" style="float: none; margin-bottom: 0;">
		        <dd>
                    <div class="head">发放类型：</div>
                    <asp:DropDownList ID="GetType" runat="server" CssClass="select">
                        <asp:ListItem Value="">所有</asp:ListItem>
                        <asp:ListItem Value="1">在线发放</asp:ListItem>
                        <asp:ListItem Value="2">线下发放</asp:ListItem>
                    </asp:DropDownList>
		        </dd>
                <dd>
                    <div class="head">是否使用：</div>
                    <asp:DropDownList ID="IsUse" runat="server" CssClass="select">
                        <asp:ListItem Value="">所有</asp:ListItem>
                        <asp:ListItem Value="0">未使用</asp:ListItem>
                        <asp:ListItem Value="1">已使用</asp:ListItem>
                    </asp:DropDownList>
		        </dd>
                <dd>
                    <div class="head">编号：</div>
                    <asp:TextBox ID="Number" CssClass="txt" runat="server" Width="150px" />
		        </dd>
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <table class="product-list">
                <thead>
                    <tr>    
	                    <td style="width:5%">选择</td>
	                    <td style="width:5%">Id</td>
	                    <td style="width:25%; text-align:left;text-indent:8px;">优惠券编号</td>
	                    <td style="width:15%">密码</td>
	                    <td style="width:15%">获取类型</td>
	                    <td style="width:15%">所属用户</td>
	                    <td style="width:10%">是否使用</td>
	                    <td style="width:10%">订单</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("Id") %>" ig-bind="list" /></label></td> 	
                                <td><%# Eval("Id") %></td>
			                    <td style="text-align:left;text-indent:8px;"><%# Eval("Number") %></td>
			                    <td><%#Eval("Password")%></td>
                                <td><%# EnumHelper.ReadEnumChineseName<CouponType>(Convert.ToInt32(Eval("GetType")))%></td>   
                                <td><%#Eval("UserName")%></td>   
                                <td><%#ShopCommon.GetBoolText(Eval("IsUse"))%></td>   
                                <td class="link"><%#ReadOrderLink(Convert.ToInt32(Eval("OrderID")))%></td>  
			                    
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="8">
                            <div class="button">
                                <label class="ig-checkbox"><input type="checkbox" value="" class="checkall" bind="list" />全选</label>
                                <asp:Button CssClass="button-2 del" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server" OnClick="DeleteButton_Click" />
                                <asp:Button CssClass="button-2" ID="ExportButton" Text="导出Excel" runat="server" OnClick="ExportButton_Click"/>
                            </div>
                            <SkyCES:CommonPager ID="MyPager" runat="server" />
                            <div class="clear"></div>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</asp:Content>
