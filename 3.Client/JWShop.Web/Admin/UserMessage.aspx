<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true"
    CodeBehind="UserMessage.aspx.cs" Inherits="JWShop.Web.Admin.UserMessage" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <link rel="stylesheet" href="/Admin/js/jqdate/base/jquery.ui.all.css">
    <script src="/Admin/js/jqdate/js/jquery.ui.core.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.widget.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <link rel="stylesheet" href="/Admin/js/jqdate/demos.css">
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_StartPostDate").datepicker({ changeMonth: true, changeYear: true });
            $("#ctl00_ContentPlaceHolder_EndPostDate").datepicker({ changeMonth: true, changeYear: true });
        });
    </script>
    <div class="container ease" id="container">
        <div class="tab-title" style="display: none">
            <span <%if(classID==int.MinValue){%>class="cur" <%} %>><a href="UserMessage.aspx">所有留言</a></span>
            <span <%if(classID==1){%>class="cur" <%} %>><a href="UserMessage.aspx?Action=search&MessageClass=1">
                申请</a></span> <span <%if(classID==2){%>class="cur" <%} %>><a href="UserMessage.aspx?Action=search&MessageClass=2">
                    我需要服务</a></span>
        </div>
        <asp:Label Text="" ID="lab_sty"  runat="server" Visible="False" />
        <div class="product-container layerpt0" style="padding: 20px 0 10px;">
            <dl class="product-filter clearfix">
                <dd>
                    <div class="head">
                        留言类型：</div>
                    <asp:DropDownList ID="MessageClass" runat="server" CssClass="select">
                        <asp:ListItem Value="">全部</asp:ListItem>
                        <asp:ListItem Value="1">留言</asp:ListItem>
                        <asp:ListItem Value="2">投诉</asp:ListItem>
                        <asp:ListItem Value="3">询问</asp:ListItem>
                        <asp:ListItem Value="4">售后</asp:ListItem>
                        <asp:ListItem Value="5">求购</asp:ListItem>
                    </asp:DropDownList>
                </dd>
                <dd>
                    <div class="head">
                        标题：</div>
                    <SkyCES:TextBox CssClass="txt" ID="Title" runat="server" Width="60px" />
                </dd>
                <dd>
                    <div class="head">
                        用户：</div>
                    <SkyCES:TextBox CssClass="txt" ID="UserName" runat="server" Width="60px" />
                </dd>
                <dd>
                    <div class="head">
                        是否处理：</div>
                    <asp:DropDownList ID="IsHandler" runat="server" CssClass="select">
                        <asp:ListItem Value="">所有</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                        <asp:ListItem Value="1">是</asp:ListItem>
                    </asp:DropDownList>
                </dd>
                <dd>
                    <div class="head">
                        留言时间：</div>
                    <SkyCES:TextBox CssClass="txt" ID="StartPostDate" runat="server" RequiredFieldType="日期时间"
                        Width="70" />
                    <span class="tp">到</span>
                    <SkyCES:TextBox CssClass="txt" ID="EndPostDate" runat="server" RequiredFieldType="日期时间"
                        Width="70" />
                </dd>
                <dt>
                    <asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"
                        OnClick="SearchButton_Click" /></dt>
            </dl>
            <table class="product-list" cellpadding="0" cellspacing="0" border="0" width="100%">
                <thead>
                    <tr>
                        <td style="width: 5%">
                            选择
                        </td>
                        <td style="width: 5%">
                            Id
                        </td>
                        <td align="left" style="width: 300px;">
                            标题
                        </td>
                         <td style="">类型</td>
                        <td style="width: 15%">
                            时间
                        </td>
                        <td style="width: 10%">
                            姓名
                        </td>
                        <td style="width: 10%">
                            联系方式
                        </td>
                        <td style="width: 10%">
                            是否处理
                        </td>
                        <td style="width:100px">
                            管理
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <label class="ig-checkbox">
                                        <input type="checkbox" name="SelectID" value="<%# Eval("ID") %>" ig-bind="list" /></label>
                                </td>
                                <td>
                                    <%# Eval("Id") %>
                                </td>
                                <td align="left">
                                    <%# Eval("Title") %>
                                </td>
                                 <td><%# UserMessageBLL.ReadMessageType(Convert.ToInt32(Eval("MessageClass"))) %></td>  
                                <td>
                                    <%# Eval("PostDate") %>
                                </td>
                                <td>
                                    <%#Eval("UserName") %>
                                </td>
                                <td>
                                    <%# Eval("Tel")%>
                                </td>
                                <td>
                                    <%#ShopCommon.GetBoolText(Convert.ToInt32(Eval("IsHandler")))%>
                                </td>
                                <td class="link">
                                    <a href="UserMessageAdd.aspx?StryleID=<%=styleid %>&ID=<%# Eval("ID") %>">查看留言</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="9">
                            <div class="button">
                                <label class="ig-checkbox">
                                    <input type="checkbox" value="" class="checkall" bind="list" />全选</label>
                                <asp:Button CssClass="button-2 del" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()"
                                    runat="server" OnClick="DeleteButton_Click" />&nbsp;
                            </div>
                            <SkyCES:CommonPager ID="MyPager" runat="server" />
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</asp:Content>
