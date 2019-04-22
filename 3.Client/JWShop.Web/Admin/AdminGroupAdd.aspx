<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="AdminGroupAdd.aspx.cs" Inherits="JWShop.Web.Admin.AdminGroupAdd" %>

<%@ Register Assembly="SkyCES.EntLib" Namespace="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
        <div class="tab-title">
            <span class="cur">基本设置</span>
            <span>权限设置</span>
        </div>
        <div class="product-container product-container-border">
            <div class="product-main"> 
                <div class="form-row">
                    <div class="head">组名称：</div>
                    <SkyCES:TextBox CssClass="txt" Width="300px" ID="Name" runat="server" CanBeNull="必填" />
                </div>
                <div class="form-row">
                    <div class="head">备注：</div>
                    <SkyCES:TextBox CssClass="txt" Width="300px" ID="Note" runat="server" TextMode="MultiLine" Height="80px" />
                </div>
            </div>
            <div class="product-main hidden">
                <%foreach (PowerInfo channelPower in channelPowerList)
                    { %>
                    <div class="form-checkbox">
                        <div class="head"><%=channelPower.Text%></div>
                        <%foreach (PowerInfo blockPower in ReadPowerBlock(channelPower.XML))
                            { %>
                        <ul>                            
                            <li class="title"><%=blockPower.Text%></li>
                            <li class="main">
                                <%foreach (PowerInfo itemPower in ReadPowerItem(blockPower.XML))
                                    { %>
                                <%if (itemPower.Value.IndexOf("AdminGroup") >= 0 && Cookies.Admin.GetAdminID(true) == 1) {%>
                                    <label class="ig-checkbox checked"><input name="Rights" type="checkbox" value="<%= channelPower.Key + itemPower.Value%>" checked="checked" /><%=itemPower.Text %></label>
                                  <% continue; %>  
                                <%} %>
                                <%if (power.IndexOf("|" + channelPower.Key + itemPower.Value + "|") > -1)
                                    {%>
                                    <label class="ig-checkbox checked"><input name="Rights" type="checkbox" value="<%= channelPower.Key + itemPower.Value%>" checked="checked" ig-bind="list"/><%=itemPower.Text %></label>
                                <%}
                                    else
                                    { %>
                                    <label class="ig-checkbox"><input name="Rights" type="checkbox" value="<%= channelPower.Key + itemPower.Value%>" ig-bind="list"/><%=itemPower.Text %></label>
                                <%}
                                } %>
                            </li>
                        </ul>
                        <%} %>
                    </div>
                <%} %>
                <div class="clear"></div>
                <div class="product-list">
                    <label class="ig-checkbox" style=""><input type="checkbox" value="" class="checkall" bind="list" />全选</label>
                </div>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" OnClientClick="return checkProduct()" />
        </div>
    </div>
<script type="text/javascript">
  var _groupId="<%= RequestHelper.GetQueryString<int>("ID")%>";
    if (_groupId == 1) {
        $("input[type='checkbox']").each(function () {
          //  $(this).attr("disabled", "disabled");
            
        });
    }
</script>
</asp:Content>