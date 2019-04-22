<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="AdminLog.aspx.cs" Inherits="JWShop.Web.Admin.AdminLog" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Register Assembly="SkyCES.EntLib" Namespace="SkyCES.EntLib" TagPrefix="SkyCES" %>
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
        <div class="product-container" style="padding-top: 20px;">
            <dl class="product-filter clearfix">
                <dd>
                    <div class="head">管理员：</div>
                    <asp:DropDownList ID="AdminId" runat="server" CssClass="select" />
                </dd>
                <dd>
                    <div class="head">操作类型：</div>
                    <SkyCES:TextBox CssClass="txt" ID="LogType" runat="server"  Width="200px"/>
                </dd>
                <dd>
                    <div class="head">操作时间：</div>
                    <SkyCES:TextBox  CssClass="txt" ID="StartAddDate" runat="server" MaxLength="10" RequiredFieldType="日期时间" placeholder="年-月-日"/>
                    <span class="tp">到</span>
                    <SkyCES:TextBox  CssClass="txt" ID="EndAddDate" runat="server" MaxLength="10" RequiredFieldType="日期时间" placeholder="年-月-日"/>
                </dd>                                
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <div class="clear"></div>
            <table class="product-list">
                <thead>
                    <tr>
                        <td style="width:5%">选择</td>
                        <td style="width:5%">Id</td>
                        <td style="width:50%">操作记录</td>
                        <td style="width:10%">管理员</td>
                        <td style="width:20%">时间</td>
                        <td style="width:10%">IP</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("Id") %>" ig-bind="list" /></label></td>
                                <td><%# Eval("Id") %></td>
                                <td><%# Eval("Action") %></td>
                                <td><%# Eval("AdminName")%></td>
                                <td><%# Eval("AddDate")%></td>
                                <td><%# Eval("IP") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="6">
                            <div class="button">
                                <label class="ig-checkbox"><input type="checkbox" name="All" onclick="selectAll(this)" class="checkall" bind="list" />全选</label>
                                <asp:Button CssClass="button-2 del" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server" OnClick="DeleteButton_Click" />
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