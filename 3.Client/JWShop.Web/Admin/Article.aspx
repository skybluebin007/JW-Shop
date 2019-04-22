<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="Article.aspx.cs" Inherits="JWShop.Web.Admin.Article" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>

<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
        <div class="product-container" style="padding-top: 20px;">	
            <dl class="product-filter clearfix">
                <dd>
                    <div class="head">类别：</div>
                    <asp:DropDownList ID="ArticleClassID" runat="server" CssClass="select" />
                </dd>
                <dd>
                    <div class="head">标题：</div>
                    <SkyCES:TextBox CssClass="txt" ID="Title" runat="server"  Width="200px"/>
                </dd>
                <dd>
                    <div class="head">是否推荐：</div>
                    <asp:DropDownList ID="IsTop" runat="server" CssClass="select"><asp:ListItem Value="">全部</asp:ListItem><asp:ListItem Value="0">否</asp:ListItem><asp:ListItem Value="1">是</asp:ListItem></asp:DropDownList>
                </dd>                                
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <div class="add-button"><a href="ArticleAdd.aspx" title="添加新数据" class="ease"> 添 加 </a></div>
            <div class="clear"></div>
            <table class="product-list">
                <thead>
                    <tr>       
	                    <td>选择</td>
	                    <td>ID</td>
	                    <td>标题</td>
	                    <td>类别</td> 
	                    <td>是否推荐</td>  
	                    <td>管理</td>        
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
                        <ItemTemplate>	     
                            <tr>
                                <td><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("Id") %>" ig-bind="list" /></label></td> 	        
		                        <td><%# Eval("Id") %></td>
		                        <td><%# Eval("Title") %></td>
		                        <td><%#ArticleClassBLL.NameList(Eval("ClassId").ToString())%></td> 
	                            <td id="IsTop<%#Eval("ID") %>" onclick="updateArticleStatus(<%#Eval("ID") %>,'IsTop')" style="cursor:pointer;"><%#ShopCommon.GetBoolText(Eval("IsTop")) %></td> 
		                        <td class="link"><a href="ArticleAdd.aspx?ID=<%# Eval("Id") %>">修改</a></td>
	                        </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="6">
                            <div class="button">
	                            <label class="ig-checkbox"><input type="checkbox" name="All"  class="checkall" bind="list"/>全选</label>                  
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
    //更改文章状态：推荐等
    function updateArticleStatus(id, action) {
        var status = 0;
        var obj;
        try {
            obj =$("#"+action + id);
        } catch (e) {
            alertMessage("系统忙，请稍后重试");
        }
        if (obj.html() != '是') {
            status = 1;
        }
        $.ajax({
            url: "Ajax.aspx",
            type: "get",
            data: { Action: "ChangeArticleStatus", field: action, ID: id, Status: status },
            dataType: "text",
            success: function (data) {
                var list = data.split('|');
                try {
                    obj = $("#" + list[0] + list[1]);
                    if (obj.html() == '是') {
                        obj.html("否") ;
                    }
                    else {
                        obj.html("是");
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
