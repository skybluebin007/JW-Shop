<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="BargainSelectProduct.aspx.cs" Inherits="JWShop.Web.Admin.BargainSelectProduct" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
    	<div class="product-container">
<dl class="product-filter product-filter-pro clearfix">
    <dd>
        <div class="head">商品名称：</div>
        <SkyCES:TextBox CssClass="input" ID="Key" runat="server" Width="100px" title="商品名称" />
    </dd>
    <dd>
        <div class="head">商品分类</div>
        <asp:DropDownList ID="ClassID" runat="server"  CssClass="select"/>
    </dd>
    <dd>
        <div class="head">商品品牌</div>
        <asp:DropDownList ID="BrandID" runat="server"  CssClass="select"/>
    </dd>
    <dt>
        <asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" />
    </dt>
</dl>
<table  cellpadding="0" cellspacing="0" border="0" class="product-list product-list-img" width="100%">
<thead>
<tr>
	<td style="width:5%">ID</td>    
	<td style="width:65%; text-align:center;text-indent:8px;" colspan="2">商品名称</td>	
	<td style="width:20%">分类</td>      
	<td style="width:10%">选择</td>                
</tr>
</thead>
    <tbody>
<asp:Repeater ID="RecordList" runat="server">
	<ItemTemplate>	     
        	<tr height="80" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">
			<td height="80"><%# Eval("ID") %></td>
            <td ><div class="scan-img"><img src="<%#ShopCommon.ShowImage(Eval("Photo").ToString().Replace("Original","90-90")) %>" /></div></td>
			<td ><%# Eval("Name") %></td>			
			<td ><%# ProductClassBLL.ProductClassNameListAdmin(Eval("ClassID").ToString())%></td>    
	       	<td ><span onclick="selectThisProduct(<%# Eval("ID") %>)" style="cursor:pointer" class="ig-colink">选择</span></td> 	        
		</tr>
     </ItemTemplate>
</asp:Repeater>
    </tbody>
</table>
<div class="listPage"><SkyCES:CommonPager ID="MyPager" runat="server" /></div>
</div>
    </div>
<script language="javascript" type="text/javascript">
    function selectThisProduct(productID) {
   var index = parent.layer.getFrameIndex(window.name);
    //var tag=getQueryString("Tag");
    parent.loadProduct(productID);
    parent.layer.close(index);
}
</script>
</asp:Content>

