<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="SendMessage.aspx.cs" Inherits="JWShop.Web.Admin.SendMessage" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>

<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
        <link rel="stylesheet" href="/Admin/js/jqdate/base/jquery.ui.all.css">    
        <style type="text/css">
        	.layui-layer-content{ padding: 10px; }
        </style>
    <script src="/Admin/js/jqdate/js/jquery.ui.core.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.widget.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_StartAddDate").datepicker({ changeMonth: true, changeYear: true });
            $("#ctl00_ContentPlaceHolder_EndAddDate").datepicker({ changeMonth: true, changeYear: true });
        });
    </script>
    <div class="container ease" id="container">
        <div class="product-container" style="padding: 20px 0 40px;">
            <dl class="product-filter clearfix">
                <dd>
                    <div class="head">标题：</div>
                    <SkyCES:TextBox CssClass="txt" ID="Title" runat="server" Width="200px" MaxLength="20"/>
                </dd>
                <dd>
                    <div class="head">发送日期：</div>
                    <SkyCES:TextBox CssClass="txt" ID="StartAddDate" runat="server" MaxLength="10" RequiredFieldType="日期时间" placeholder="年-月-日" />
                    <span class="tp">到</span>
                    <SkyCES:TextBox CssClass="txt" ID="EndAddDate" runat="server" MaxLength="10" RequiredFieldType="日期时间" placeholder="年-月-日" />
                </dd>
                <dt>
                    <asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server" OnClick="SearchButton_Click" /></dt>
            </dl>
            <div class="add-button"><a href="SendMessageAdd.aspx" title="发送新消息" class="ease">发送新消息</a></div>
            <div class="clear"></div>
            <table class="product-list">
                <thead>
                    <tr>       
	                    <td>选择</td>
	                    <td>ID</td>
	                    <td>标题</td>
                        <td>接收用户</td> 
                        <td>发送日期</td> 
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
		                        <td><%#Eval("UserName")%></td> 
                                <td><%#Eval("Date")%></td> 
	                            <td class="link">
                                    <a href="javascript:loadPage('<%# Eval("Content") %>','消息内容','600px','300px')" title="查看消息内容">查看内容</a>
                                    <a onclick="return confirm('确定删除此消息吗？')" href="?Action=delete&ID=<%# Eval("Id") %>">删除</a>

	                            </td>
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

        function loadPage(url, title, width, height) {
            layer.open({
                type: 1,
                //skin: 'layui-layer-lan',
                title: title,
                fix: false,
                shadeClose: true,
                maxmin: false,
                area: [width, height],
                content: url,
              
            });
        }
    </script>
</asp:Content>
