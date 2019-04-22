<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="Withdraw.aspx.cs" Inherits="JWShop.Web.Admin.Withdraw" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
        <link rel="stylesheet" href="/Admin/Js/jqdate/base/jquery.ui.all.css">    
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.core.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <link rel="stylesheet" href="/Admin/Js/jqdate/demos.css">
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_StartTime").datepicker({ changeMonth: true, changeYear: true });
            $("#ctl00_ContentPlaceHolder_EndTime").datepicker({ changeMonth: true, changeYear: true });
        });
    </script>
    <div class="container ease" id="container">
        <div class="product-container" style="padding-top: 24px;">

                        <dl class="product-filter clearfix">
                <dd>
                    <div class="head">微信昵称：</div>
                    <SkyCES:TextBox ID="UserName" CssClass="txt" runat="server" /> 
                </dd>
                 <dd>
                    <div class="head">手机号码：</div>
                    <SkyCES:TextBox ID="Mobile" CssClass="txt" runat="server" /> 
                </dd>
                <dd>
                    <div class="head">申请时间：</div>
                    <SkyCES:TextBox CssClass="txt" ID="StartTime" runat="server" /> <span class="tp">--</span> <SkyCES:TextBox CssClass="txt" ID="EndTime" runat="server" />
                </dd>  
                  <dd>
                    <div class="head">状态：</div>
                   <asp:DropDownList ID="Status" runat="server"  CssClass="select"></asp:DropDownList>
                </dd>                            
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <table class="product-list">
                <thead>
                    <tr>                      
                        <td style="width: 35%">分销商昵称</td>
                        <td style="width: 15%">手机号码</td>
                        <td style="width: 15%">提现金额</td>
                        <td style="width: 15%">申请时间</td>  
                        <td style="width: 15%">状态</td>   
                        <td style="width: 10%">操作</td>                    
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
                        <ItemTemplate>
                            <tr>                              
                                <td><%# Eval("UserName") %></td>
                                <td><%# Eval("Mobile") %></td>
                                <td><%# Eval("Amount") %></td>
                                <td><%# Eval("Time") %></td>
                                <td><%# EnumHelper.ReadEnumChineseName<Withdraw_Status>(Convert.ToInt32(Eval("Status")))%></td>                             
                                <td class="link">
                                    <a href="WithdrawDetail.aspx?ID=<%#Eval("ID")%>">操作</a>
                                </td>                                
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="6">
                          <%--  <div class="button">
                                <label class="ig-checkbox"><input type="checkbox" value="" class="checkall" bind="list" />全选</label>
                                <asp:Button CssClass="button-2 del" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server" OnClick="DeleteButton_Click" />
                            </div>--%>
                            <SkyCES:CommonPager ID="MyPager" runat="server" />
                            <div class="clear"></div>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</asp:Content>