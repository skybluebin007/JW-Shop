<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MasterPage.Master" CodeBehind="ProductDraft.aspx.cs" Inherits="JWShop.Web.Admin.ProductDraft" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <link rel="stylesheet" href="/Admin/js/jqdate/base/jquery.ui.all.css">    
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
    	<div class="product-container">
        	<dl class="product-filter clearfix">
            	<dd><label class="tp">分类：</label><asp:DropDownList ID="ClassID" runat="server" CssClass="select" Width="90" /> </dd>
                <dd><label class="tp">添加时间：</label><span class="tp">从</span><SkyCES:TextBox CssClass="txt" ID="StartAddDate" runat="server" RequiredFieldType="日期时间" width="70" /> <span class="tp">到</span> <SkyCES:TextBox CssClass="txt" ID="EndAddDate" runat="server" RequiredFieldType="日期时间" width="70" /></dd>
                <dd><label class="tp">品牌：</label><asp:DropDownList ID="BrandID" runat="server"  CssClass="select" width="90" /> </dd>
                <dd><label class="tp">商品名称：</label><SkyCES:TextBox CssClass="txt" ID="Key" runat="server" Width="100px" title="商品名称"/></dd>
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <table cellpadding="0" cellspacing="0" border="0" class="product-list" width="100%">
                <thead>
                <tr>
                    <td  width="50">选择</td>    
	                <td  width="50" height="40">ID</td>
	                <td  width="150">图片</td>
	                <td >商品名称</td>
	                <td width="50">编码</td>
	                <td width="200">分类</td>
	                <td  width="50">品牌</td>
	                <td  width="150">发布时间</td>
	                <td  width="100">操作</td> 
                </tr>
                </thead>
                <tbody>
            <asp:Repeater ID="RecordList" runat="server">
	            <ItemTemplate>	     
    	            <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">
                        <td><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("ID") %>"  ig-bind="list"/></label></td> 
		                <td ><%# Eval("ID") %></td>
		                <td ><div class="scan-img"><img src="<%#ShopCommon.ShowImage(Eval("Photo").ToString().Replace("Original","90-90")) %>"   onload="photoLoad(this,40,40)" /></div></td>
		                <td ><%# Eval("Name") %></td>
		                <td ><%# Eval("ProductNumber") %></td>
		                <td ><%# ProductClassBLL.ProductClassNameList(Eval("ClassID").ToString()) %></td>
		                <td ><%# ProductBrandBLL.Read(Convert.ToInt32(Eval("BrandID"))).Name %></td>
		                <td><%#Convert.ToDateTime(Eval("AddDate")).ToString("yyyy-MM-dd HH:mm") %></td>
		                <td ><a href="ProductAdd.aspx?ID=<%# Eval("ID") %>"  class="ig-colink">修改</a> | <a href="javascript:deleteProduct(<%#Eval("ID") %>)"  class="ig-colink">删除</a></td>		                
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
                    </tbody>
                <tfoot>
                	<tr>
                    	<td colspan="9">
                        	<div class="button">
                                <label class="ig-checkbox"><input type="checkbox" name="All" onclick="selectAll(this)"  class="checkall" bind="list"/>全选</label>   
                                <asp:Button CssClass="button-2 del" ID="OnSaleButton" Text=" 上 架 " OnClientClick="return checkSelect()" runat="server"  OnClick="OnSaleButton_Click"/>	         &nbsp;<asp:Button CssClass="button-2 del" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>                                     
                                                                
                            </div>
                            <SkyCES:CommonPager ID="MyPager" runat="server" />                            
                            <div class="clear"></div>
                    	</td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
    <script language="javascript" type="text/javascript">var productID = ""; var _selectclassid = ""; var isUpdate = 0;</script>	 
    <script language="javascript" type="text/javascript" src="/Admin/js/ProductAdd.js"></script>
</asp:Content>