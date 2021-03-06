﻿<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="UserActive.aspx.cs" Inherits="JWShop.Web.Admin.UserActive" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
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
        <div class="tab-title">
            <span><a href="UserCount.aspx">数量分析</a></span>
            <span><a href="UserStatus.aspx">状态分析</a></span>
            <span class="cur"><a href="UserActive.aspx">活跃度分析</a></span>
            <span><a href="UserConsume.aspx">消费分析</a></span>
        </div>
        <div class="product-container product-container-border">
            <dl class="product-filter clearfix" style="float:none;">
		        <dd>
                    <div class="head">注册时间：</div>
                    <SkyCES:TextBox CssClass="txt" ID="StartRegisterDate" runat="server" width="70px" /> <span class="tp">到</span> <SkyCES:TextBox CssClass="txt" ID="EndRegisterDate" runat="server"  width="70px" /> 
		        </dd>
                <dd>
                    <div class="head">用户名：</div>
                    <SkyCES:TextBox ID="UserName" CssClass="txt" runat="server" />
		        </dd>
                <dd>
                    <div class="head">性别：</div>
                    <asp:DropDownList ID="Sex" runat="server" CssClass="select"></asp:DropDownList>
		        </dd>
                <dd>
                    <div class="head">排序：</div>
                    <asp:DropDownList ID="UserOrderType" runat="server" CssClass="select" width="150px" ><asp:ListItem Value="LoginTimes">登录次数从多到小</asp:ListItem><asp:ListItem Value="CommentCount">评论次数从多到小</asp:ListItem></asp:DropDownList>
		        </dd>
                <dt>
                    <asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <table class="product-list">
                <thead>
                    <tr>
	                    <td style="width:10%">Id</td>
	                    <td style="width:30%; text-align:left;text-indent:8px;">用户</td>
	                    <td style="width:10%">性别</td>
	                    <td style="width:20%">注册时间</td>
	                    <td style="width:15%">登陆次数</td>
	                    <td style="width:15%">评论次数</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
	                    <ItemTemplate>	     
                            <tr>
			                    <td><%# Eval("Id") %></td>
			                    <td style="text-align:left;text-indent:8px;"><%#HttpUtility.UrlDecode(Eval("UserName").ToString(),Encoding.UTF8) %></td>
			                    <td><%#EnumHelper.ReadEnumChineseName<SexType>(Convert.ToInt32(Eval("Sex")))%></td>
	                            <td><%# Eval("RegisterDate") %></td>
	                            <td><%#Eval("LoginTimes")%></td>
	                            <td><%#Eval("CommentCount")%></td>      
		                    </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="6">
                        	<div class="button">
                                
                                <%if(Count>0){ %>
                                <asp:Button CssClass="button-3" ID="ExportButton" Text=" 导 出 " runat="server" OnClick="ExportButton_Click" /> 
                                <%}else{%>共找到<%=Count %>条记录<%} %>                       
                            </div>
                            <SkyCES:CommonPager ID="MyPager" runat="server" />
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</asp:Content>