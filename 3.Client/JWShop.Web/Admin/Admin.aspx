<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="JWShop.Web.Admin.Admin" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
        <div class="product-container" style="padding-top: 22px;">
            <div class="add-button"><a href="AdminAdd.aspx" title="添加新数据" class="ease"> 添 加 </a></div>
            <table class="product-list">
                <thead>
                    <tr>
                        <td style="width:5%">选择</td>
                        <td style="width:5%">Id</td>
                        <td style="width:10%">管理员</td>
                        <td style="width:10%">管理员组</td>
                        <td style="width:10%">状态</td>
                        <td style="">上次登陆时间</td>
                        <td style="">上次登陆IP</td>
                        <td style="width:10%">登陆次数</td>
                        <td style="width:15%">管理</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# AdminBLL.NoDelete(Eval("IsCreate"), Eval("Id"),1)%></td>
                                <td><%# Eval("Id")%></td>
                                <td><%# Eval("Name")%></td>
                                <td><%# AdminGroupBLL.Read(Convert.ToInt32(Eval("GroupId"))).Name%></td>
                                <td><%# Convert.ToInt32(Eval("Status")) == (int)BoolType.True ? "正常" : "<span style='color:#E64652'>冻结</span>" %></td>
                                <td><%# Eval("LastLoginDate")%></td>
                                <td><%# Eval("LastLoginIP")%></td>
                                <td><%# Eval("LoginTimes")%></td>
                                <td class="link"><%#AdminBLL.NoUpdate(Eval("IsCreate"),Eval("Id"))%> 
                                    <%#AdminBLL.NoPasswordAdd(Eval("Id"),Eval("IsCreate")) %>
                                
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="9">
                            <div class="button">
                                <label class="ig-checkbox"><input type="checkbox" value="" class="checkall" bind="list" />全选</label>
                                <asp:Button CssClass="button-2 del" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server" OnClick="DeleteButton_Click" />&nbsp;                                
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