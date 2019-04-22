<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="UserDuihuan.aspx.cs" Inherits="JWShop.Web.Admin.UserDuihuan" %>
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
        <div class="product-container" style="padding-top: 20px;">	
            <dl class="product-filter clearfix">
                <dd>
                    <div class="head">电话：</div>
                    <SkyCES:TextBox ID="mobile" CssClass="txt" runat="server" /> 
                </dd>
                <dd>
                    <div class="head">姓名：</div>
                    <SkyCES:TextBox ID="truename" CssClass="txt" runat="server" /> 
                </dd>               
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <table class="product-list user-add-list" cellpadding="0" cellspacing="0" border="0" width="100%">
                <thead>
                    <tr>
	                    <td style="width:3%"></td>
	                    <td style="width:3%">Id</td>
	                    <td style="width:5%;">用户ID</td>
                        <td style="width:10%;">姓名</td>
                        <td style="width:10%;">电话</td>
                        <td style="width:8%;">使用积分</td>
                        <td style="width:8%">操作人ID</td>
                        <td>备注</td>
                        <td style="width:10%">兑换时间</td>
	                    <td style="width:10%">管理</td>
                    </tr>
                </thead>
                <tbody>
                    <tr class="firstH">
                    	<td colspan="11" style="padding: 0;">
                        	<div class="button">

                                <label class="ig-checkbox" style="float: left;padding-right: 10px;"><input type="checkbox" name="All" onclick="selectAll(this)"  class="checkall" bind="list"/>全选</label>  <input type="button" class="button-2 del" ID="Button3" value=" 添 加 " onclick="    location.href = 'UserDuihuanAdd.aspx'" />
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
			                    <td><%# Eval("id") %></td>
			                    <td><%# Eval("userid") %></td>
                                <td><%#Eval("truename") %></td>
                                <td><%#Eval("mobile") %></td>
                                <td><%#Eval("integral") %></td>
                                <td><%# Eval("adminid") %></td>
                                <td><%# Eval("note")%></td>
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
                                <input type="button" class="button-2 del" ID="Button1" value=" 添 加 " onclick="location.href = 'UserDuihuanAdd.aspx'" />
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
