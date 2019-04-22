<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="Distributor.aspx.cs" Inherits="JWShop.Web.Admin.Distributor" %>

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
            <span <%if (distributor_Status == 1)
                { %>class="cur"
                <%} %>><a href="Distributor.aspx?Action=search&distributor_Status=1">正常</a></span>
            <span <%if (distributor_Status== 0)
                { %>class="cur"
                <%} %>><a href="Distributor.aspx?Action=search&distributor_Status=0">待审核</a></span>
            <span <%if (distributor_Status== -1)
                { %>class="cur"
                <%} %>><a href="Distributor.aspx?Action=search&distributor_Status=-1">冻结</a></span>

        </div>
        <div class="product-container" style="padding-top: 20px;">	
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
                    <div class="head">注册时间：</div>
                    <SkyCES:TextBox CssClass="txt" ID="StartRegisterDate" runat="server" /> <span class="tp">--</span> <SkyCES:TextBox CssClass="txt" ID="EndRegisterDate" runat="server" />
                </dd>                              
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <table class="product-list user-add-list" cellpadding="0" cellspacing="0" border="0" width="100%">
                <thead>
                    <tr>
	                    <td style="width:3%"></td>
	                    <td style="width:3%">Id</td>
	                    <td style="width:20%;">微信昵称</td>
                        <td style="width:10%;">手机</td>
                         <td style="width:10%">姓名</td>
                        <td style="width:10%">下级数</td>	      
	                    <td style="width:10%">最近登录</td>
	                    <td style="width:5%">状态</td>
	                    <td style="width:10%">管理</td>
                    </tr>
                </thead>
                <tbody>
                    <tr class="firstH">
                    	<td colspan="11" style="padding: 0;">
                        	<div class="button">
	                             <%if(Count>0){ %>
                                <label class="ig-checkbox" style="float: left;padding-right: 10px;"><input type="checkbox" name="All" onclick="selectAll(this)"  class="checkall" bind="list"/>全选</label>      
                              <%--  <input type="button" class="button-2 del" ID="Button3" value=" 添 加 " onclick="location.href = 'userAdd.aspx'" />       
                               <asp:Button CssClass="button-3" ID="Button2" Text=" 导 出 " runat="server" OnClick="ExportButton_Click" />--%> 
                                &nbsp;&nbsp;每页显示：<asp:DropdownList ID="AdminPageSize" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="AdminPageSize_SelectedIndexChanged">
                                    <asp:ListItem Value="20">20条</asp:ListItem>
                                    <asp:ListItem Value="50">50条</asp:ListItem>
                                    <asp:ListItem Value="100">100条</asp:ListItem>
                                     </asp:DropdownList>
                                 <%} %>                               
                                <span style="float: left;">共找到<%=Count %>条记录<%if(Count>0){ %> ，<%=MyPager.PageCount %>页<%} %></span>
                            </div>                          
                    	</td>
                    </tr>
                    <asp:Repeater ID="RecordList" runat="server">
	                    <ItemTemplate>	     
        	                <tr>
			                    <td><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("ID") %>" ig-bind="list" /></label></td> 	
			                    <td><%# Eval("Id") %></td>
			                    <td><%# Eval("UserName") %></td>
                                <td><%#Eval("Mobile") %></td>
                                <td><%#Eval("RealName") %></td>
                                <td><%#Eval("Total_Subordinate") %></td>
                                <td><%# Eval("LastLoginDate")%></td>
	                            <td><%# EnumHelper.ReadEnumChineseName<Distributor_Status>(Convert.ToInt32(Eval("Distributor_Status")))%></td>
			                    <td class="imgCz">
                                    <%--<a href="javascript:void(0);" class="ig-colink pmore">更多</a>
		                        	<div class="list">                           
                                    <a href="UserAccountRecord.aspx?UserID=<%#Eval("ID") %>&AccountType=<%=(int)AccountRecordType.Point %>"> 积分</a>
                                    <a href="javascript:loadPage('usercoupon.aspx?UserID=<%#Eval("ID") %>', '优惠券(会员：<%#UserBLL.Read(Convert.ToInt32(Eval("Id"))).UserName%>)', '950px', '500px')"> 优惠券</a>
                                    
                                    </div>			                        
--%>
			                    </td>
		                    </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="11">
                            <%if(Count>0){ %> <div class="button">
                               
	                           <label class="ig-checkbox"><input type="checkbox" name="All" class="checkall" bind="list"/>全选</label>
                                <%--<input type="button" class="button-2 del" ID="Button1" value=" 添 加 " onclick="location.href = 'userAdd.aspx'" />
                                <asp:Button CssClass="button-2" ID="Button4" Text=" 导 出 " runat="server" OnClick="ExportButton_Click" />--%>
                                <%if(distributor_Status==0){ %>&nbsp;<asp:Button CssClass="button-2 del" ID="ActiveButton" Text="审核通过" OnClientClick="return checkSelect()" runat="server"  OnClick="ActiveButton_Click"/>&nbsp;<%} %>
                                <%if(distributor_Status==1){ %>                              
                                    <asp:Button CssClass="button-2 del" ID="FreezeButton" Text=" 冻 结 " OnClientClick="return checkSelect()" runat="server"  OnClick="FreezeButton_Click"/>&nbsp;
                                <%} %>
                                <%if(distributor_Status==-1){ %><asp:Button CssClass="button-2 del" ID="UnFreezeButton" Text="解除冻结" OnClientClick="return checkSelect()" runat="server"  OnClick="UnFreezeButton_Click"/>&nbsp;<%} %>
                               
                            </div>
                            <SkyCES:CommonPager ID="MyPager" runat="server" />   <%} %>                          
                            <div class="clear"></div>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
     
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
