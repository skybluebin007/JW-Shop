<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="OrderShiya.aspx.cs" Inherits="JWShop.Web.Admin.OrderShiya" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <link rel="stylesheet" href="/Admin/Js/jqdate/base/jquery.ui.all.css">    
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.core.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <link rel="stylesheet" href="/Admin/Js/jqdate/demos.css">
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_StartRegisterDate").datepicker({ changeMonth: true, changeYear: true });
            $("#ctl00_ContentPlaceHolder_EndRegisterDate").datepicker({ changeMonth: true, changeYear: true });
        });
    </script>
    
    <div class="container ease" id="container">
        <div class="tab-title" style="margin: 30px 15px -1px 15px;">
            <span <%if (status == -1)
                { %>class="cur"
                <%} %>><a href="OrderShiya.aspx?Action=search&status=-1">所有试压</a></span>
            <span <%if (status== 0)
                { %>class="cur"
                <%} %>><a href="OrderShiya.aspx?Action=search&status=0">已合格</a></span>
            <span <%if (status== 1)
                { %>class="cur"
                <%} %>><a href="OrderShiya.aspx?Action=search&status=1">未合格</a></span>

        </div>

        <div class="product-container" style="padding-top: 20px;">	
            <dl class="product-filter clearfix">
                <dd>
                    <div class="head">客户电话：</div>
                    <SkyCES:TextBox ID="mobile" CssClass="txt" runat="server" /> 
                </dd>
                <dd>
                    <div class="head">客户姓名：</div>
                    <SkyCES:TextBox ID="truename" CssClass="txt" runat="server" /> 
                </dd>               
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <table class="product-list user-add-list" cellpadding="0" cellspacing="0" border="0" width="100%">
                <thead>
                    <tr>
	                    <td style="width:3%"></td>
	                    <td style="width:10%">经销商</td>
	                    <td style="width:10%;">水工</td>
                        <td style="width:10%;">试压</td>
                        <td style="width:10%;">客户</td>
                        <td style="width:10%;">订单号</td>
                        <td style="width:10%;">业主地址</td>
                        <td style="width:10%">房屋类型</td>
                        <td style="width:5%">是否合格</td>
                        <td style="width:10%">试压时间</td>
	                    <td>管理</td>
                    </tr>
                </thead>
                <tbody>
                    <tr class="firstH">
                    	<td colspan="11" style="padding: 0;">
                        	<div class="button">

                                <label class="ig-checkbox" style="float: left;padding-right: 10px;"><input type="checkbox" name="All" onclick="selectAll(this)"  class="checkall" bind="list"/>全选</label>
                                &nbsp;&nbsp;每页显示：<asp:DropdownList ID="AdminPageSize" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="AdminPageSize_SelectedIndexChanged">
                                    <asp:ListItem Value="20">20条</asp:ListItem>
                                    <asp:ListItem Value="50">50条</asp:ListItem>
                                    <asp:ListItem Value="100">100条</asp:ListItem>
                                     </asp:DropdownList>
                            
                                <span style="float: left;">共找到<%=Count %>条记录<%if(Count>0){ %> ，<%=MyPager.PageCount %>页<%} %></span>
                            </div>                          
                    	</td>
                    </tr>
                    <asp:Repeater ID="RecordList" runat="server">
	                    <ItemTemplate>	     
        	                <tr>
			                    <td><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("id") %>" ig-bind="list" /></label></td> 	
			                    <td><%# getuserinfo(Eval("saleid").ToString()).UserName %></br><%# getuserinfo(Eval("saleid").ToString()).Mobile %></td>
			                    <td><%# Eval("shuigongid") %></td>
                                <td><%# Eval("shiyaid") %></td>
                                <td><%# Eval("customerid") %></td>
                                <td><%# Eval("orderId") %></td>
                                <td><%# Eval("address") %></td>
                                <td><%#Eval("housetype") %></td>
                                <td><%#Eval("stat").ToString()=="0"?"合格":"<font color='red'>未合格</font>" %></td>
	                            <td><%# Eval("addtime")%></td>
			                    <td class="imgCz">
                                    <a href="?Action=Delete&Id=<%#Eval("id") %>">删除</a>
			                    </td>
		                    </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="12">
                            <div class="button">
	                           <label class="ig-checkbox"><input type="checkbox" name="All" class="checkall" bind="list"/>全选</label>
                            </div>
                            <SkyCES:CommonPager ID="MyPager" runat="server" />             
                            <div class="clear"></div>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        //删除用户
        function deleteOrderYuding(id) {
            if (window.confirm("确定要删除该用户？")) {
                var url = "Ajax.aspx?Action=DeleteOrderYuding&id=" + id;
                Ajax.requestURL(url, dealDeleteUser);
            }
        }
        function dealDeleteUser(data) {
            if (data == "ok") {
                reloadPage();
            }
            else {
                layer.msg("该用户已处理，不能删除。");
            }
        }
        function loadPage(url, title, width, height) {
            layer.open({
                type: 2,
                //skin: 'layui-layer-lan',
                title: title,
                fix: false,
                shadeClose: true,
                maxmin: false,
                area: [width, height],
                content: url,

            });
        }
        $(".pmore").mouseenter(function(){
			$(this).next().show();
		});
		
		$(".imgCz .list,.imgCz").mouseleave(function(){
			$(".imgCz .list").hide();
		});
</script>
</asp:Content>
