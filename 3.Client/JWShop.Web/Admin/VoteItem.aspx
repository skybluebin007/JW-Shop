<%@ Page Language="C#" MasterPageFile="MasterPage.Master"  AutoEventWireup="true" CodeBehind="VoteItem.aspx.cs" Inherits="JWShop.Web.Admin.VoteItem" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
        <div class="product-container" style="padding-top: 20px;">	
            <dl class="product-filter clearfix">
                <dd>
                    <div class="head">投票类别：</div>
                    <asp:DropDownList ID="VoteID" runat="server" CssClass="select" />
                </dd>
                <dd>
                    <div class="head">标题：</div>
                    <SkyCES:TextBox CssClass="txt" ID="Title" runat="server"  Width="200px"/>
                </dd>
                <dd>
                    <div class="head">是否显示：</div>
                    <asp:DropDownList ID="IsShow" runat="server" CssClass="select"><asp:ListItem Value="">全部</asp:ListItem><asp:ListItem Value="0">否</asp:ListItem><asp:ListItem Value="1">是</asp:ListItem></asp:DropDownList>
                </dd>                                
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <div class="add-button"><a href="VoteItemAdd.aspx" title="添加" class="ease"> 添 加 </a></div>
            <div class="clear"></div>
            <table class="product-list">
                <thead>
                    <tr>       
	                    <td>选择</td>
	                    <td>ID</td>
	                    <td>选项名称</td>
	                    <td>投票数量</td> 
                         <td>排序号</td> 
	                    <td>是否显示</td>  
	                    <td>管理</td>        
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
                        <ItemTemplate>	     
                            <tr>
                                <td><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("ID") %>" ig-bind="list" /></label></td> 	        
		                        <td><%# Eval("ID") %></td>
		                        <td><%# Eval("ItemName")%></td>
		                        <td><%#Eval("VoteCount")%></td> 
                                <td><%#Eval("OrderID")%></td>
	                            <td id='IsShow<%#Eval("ID") %>' onclick="ChangeShow(<%#Eval("ID") %>)" style="cursor:pointer;"><%#ShopCommon.GetBoolText(Eval("Exp2")) %></td> 
		                        <td class="link"><a href="VoteItemAdd.aspx?ID=<%# Eval("ID") %>"  class="ig-colink">修改</a>|
                                     <a href="VoteRecord.aspx?VoteItemID=<%# Eval("ID") %>" class="ig-colink" >投票记录</a>
		                        </td>
	                        </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="7">
                            <div class="button">
	                            <label class="ig-checkbox"><input type="checkbox" name="All"  class="checkall" bind="list"/>全选</label>   
                                <input type="button" class="button-2 del" value=" 添 加 " onclick="window.location.href='VoteItemAdd.aspx'" />               
                                <asp:Button CssClass="button-2 del" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>                                
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
    // 更改选项显示/不显示
    function ChangeShow(id) {
        if (parseInt(id) <= 0) {
            return;
        }
        var status = 0;
        var obj;
        try {
            obj = $("#IsShow"+id);
        } catch (e) {
            alertMessage("系统忙，请稍后重试");
        }
        if (obj.html() != '是') {
            status = 1;
        }
        $.ajax({
            url: "Ajax.aspx",
            type: "get",
            data: { Action: "ChangeVoteItemShow",ID: id, Status: status },
            dataType: "text",
            success: function (data) {
                var list = data.split('|');
                try {
                    obj = $("#IsShow" + list[1]);
                    if (list[0] == "ok") {
                        if (obj.html() == '是') {
                            obj.html("否");
                        }
                        else {
                            obj.html("是");
                        }
                    }
                } catch (e) {
                    alertMessage("系统忙，请稍后重试");
                }
            },
            error: function () { alertMessage("系统忙，请稍后重试"); },

        })
    }

</script>
</asp:Content>
