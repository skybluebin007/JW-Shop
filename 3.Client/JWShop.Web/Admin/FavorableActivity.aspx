<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="FavorableActivity.aspx.cs" Inherits="JWShop.Web.Admin.FavorableActivity" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <div class="container ease" id="container">
        <div class="product-container" style="padding: 24px 0 40px;">
             <div class="tab-title" style="margin: 30px 15px -1px 15px;">         
          
            <span <%if (timePeriod== 1)
                    { %>class="cur"
                <%} %>><a href="FavorableActivity.aspx?timeperiod=1">未开始</a></span>
            <span <%if (timePeriod== 2)
                    { %>class="cur"
                <%} %>><a href="FavorableActivity.aspx?timeperiod=2">进行中</a></span>
            <span <%if (timePeriod== 3)
                    { %>class="cur"
                <%} %>>
                <a href="FavorableActivity.aspx?timeperiod=3">已结束</a></span>
          </div>
            <div class="add-button  newbtn"><a href="FavorableActivityAdd.aspx" title="添加新数据" class="ease"> 添 加 </a></div>
            <table class="product-list">
                <thead>
                    <tr>
	                    <td style="width:5%">选择</td>
	                    <td style="width:5%">Id</td>
                        <%--<td style="width:12%">类型</td>--%>
	                    <td style="width:35%;">活动标题</td>
	                    <td style="width:25%">时间</td>
	                    <td style="width:10%">最低总额</td>
                        <td style="width:12%">优惠金额</td>
	                    <td style="width:8%">管理</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("Id") %>" ig-bind="list" /></label></td>
                                <td><%# Eval("Id") %></td>
                               <%-- <td><%# FavorableActivityBLL.FavorableTypeName(Convert.ToInt32(Eval("Type"))) %></td>--%>
			                    <td><%# Eval("Name") %></td>
			                    <td><%# Eval("StartDate","{0:yyyy-MM-dd}") %> 到 <%# Eval("EndDate","{0:yyyy-MM-dd}") %></td>
	                            <td><%# Eval("OrderProductMoney")%></td>    
                                <td><%#Eval("ReduceMoney") %></td>
			                    <td class="link"><a href="FavorableActivityAdd.aspx?ID=<%# Eval("Id") %>">修改</a></td>                             
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="7">
                            <div class="button">
                                <label class="ig-checkbox"><input type="checkbox" value="" class="checkall" bind="list" />全选</label>
                                <asp:Button CssClass="button-2 del" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server" OnClick="DeleteButton_Click" />
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
