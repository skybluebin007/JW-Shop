<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="PointProduct.aspx.cs" Inherits="JWShop.Web.Admin.PointProduct" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <link rel="stylesheet" href="/Admin/Js/jqdate/base/jquery.ui.all.css">    
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.core.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <link rel="stylesheet" href="/Admin/Js/jqdate/demos.css">
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_BeginDate").datepicker({ changeMonth: true, changeYear: true });
            $("#ctl00_ContentPlaceHolder_EndDate").datepicker({ changeMonth: true, changeYear: true });
        });
    </script>
    
    <div class="container ease" id="container">
    	<div class="product-container">
        	<dl class="product-filter clearfix">
            	<dd>
                	<div class="head">商品名称：</div>
                    <SkyCES:TextBox CssClass="txt" ID="ProductName" runat="server" Width="200px" placeholder="商品名称"/>
                </dd>
                <dd>
                	<div class="head">兑换时间：</div>
                    <SkyCES:TextBox  CssClass="txt" ID="BeginDate" runat="server" MaxLength="10" RequiredFieldType="日期时间" placeholder="年-月-日"/>
                    <span class="tp">到</span>
                    <SkyCES:TextBox  CssClass="txt" ID="EndDate" runat="server" MaxLength="10" RequiredFieldType="日期时间" placeholder="年-月-日"/>
                </dd>
                <dt>
                    <asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" />
                </dt>
            </dl>
            <div class="add-button"><a href="PointProductAdd.aspx" title="添加新数据" class="ease"> 添 加 </a></div>
            <div class="clear"></div>
            <table class="product-list">
                <thead>
                    <tr>
                        <td style="width:5%">选择</td>
                        <td style="width:5%">Id</td>
	                    <td style="width:6%">图片</td>
	                    <td style="width:20%; text-align:left;text-indent:8px;">商品</td>
	                    <td style="width:10%">所需积分</td>
	                    <td style="width:10%">可兑换总数</td>
	                    <td style="width:10%">已兑换数量</td>
	                    <td style="width:12%">兑换起始时间</td>
	                    <td style="width:12%">兑换结束时间</td>
	                    <td style="width:10%">管理</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
	                    <ItemTemplate>	     
                            <tr>
			                    <td><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("Id") %>" ig-bind="list" /></label></td>
                                <td><%# Eval("Id") %></td>
		                        <td><img src="<%#Eval("Photo").ToString().Replace("Original","90-90") %>" onload="photoLoad(this,40,40)"/></td>
			                    <td><a href="/gift/<%# Eval("Id") %>.html" target="_blank" title="<%# Eval("Name") %>"><%# StringHelper.Substring(Eval("Name").ToString(), 15)%></a></td>
			                    <td><%# Eval("Point")%></td>
			                    <td><%# Eval("TotalStorageCount")%></td>
			                    <td><%# Eval("SendCount")%></td>
			                    <td><%# Eval("BeginDate","{0:yyyy-MM-dd}")%></td>
			                    <td><%# Eval("EndDate","{0:yyyy-MM-dd}")%></td>
			                    <td class="link"><a href="PointProductAdd.aspx?Id=<%# Eval("Id") %>">修改</a></td>
		                    </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="10">
                        	<div class="button">
	                            <label class="ig-checkbox"><input type="checkbox" name="All" onclick="selectAll(this)" class="checkall" bind="list"/>全选</label>
                                <asp:Button CssClass="button-2 del" ID="Button2" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>
                            </div>
                            <SkyCES:CommonPager ID="MyPager" runat="server" />
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>	

    <div class="listPage"></div>
    <div class="action">
            <input type="button"  value=" 添 加 " class="button" onclick="pop('PointProductAdd.aspx',800,600,'积分兑换产品添加','PointProductAdd')" />&nbsp;<asp:Button CssClass="button" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>&nbsp;<input type="checkbox" name="All" onclick="selectAll(this)" />全选/取消
    </div>

    <script type="text/javascript">var productID = ""; var _selectclassid = ""; var isUpdate = 0;</script>	
    <script type="text/javascript" src="/Admin/js/ProductAdd.js"></script>
</asp:Content>
