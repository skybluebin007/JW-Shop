<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductSingleEdit.aspx.cs" Inherits="JWShop.Web.Admin.ProductSingleEdit" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
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
    <script language="javascript" src="/Admin/js/ProductBatchEdit.js" type="text/javascript"></script>
    <style>
        .listBlock {width:100%; height:30px; margin:10px 0;
        }
        .listBlock a { padding:10px 15px; border:1px solid #ccc;
        }
    </style>
    <div class="container ease" id="container">
    	<div class="product-container">
            <dl class="product-filter product-filter-pro clearfix" style="clear: both;">
                <dd>
                    <div class="head">商品名称：</div>
                    <SkyCES:TextBox CssClass="txt" ID="Name" runat="server" Width="100px" /></dd>
                <dd>
                    <div class="head">商品分类：</div>
                    <asp:DropDownList ID="ClassID" runat="server" CssClass="select" />
                </dd>
                <dd>
                    <div class="head">品牌：</div>
                    <asp:DropDownList ID="BrandID" runat="server" CssClass="select" />
                </dd>
                <dd>

                    <div class="head">发布时间：</div>
                    <SkyCES:TextBox CssClass="txt" ID="StartAddDate" runat="server" RequiredFieldType="日期时间" />
                    <span class="tp">到</span>
                    <SkyCES:TextBox CssClass="txt" ID="EndAddDate" runat="server" RequiredFieldType="日期时间" /></dd>
                <dt>
                    <asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server" OnClick="SearchButton_Click" /></dt>
            </dl>
<div style="clear:both"></div>
<div class="listBlock" style=" clear:both; display:block;">
        <a href="ProductSingleEdit.aspx">逐个编辑</a>
        <a href="ProductUnionEdit.aspx">统一编辑</a>
</div>
<table  cellpadding="0" cellspacing="0" border="0" class="product-list" width="100%">
    <thead>
        <tr>
	    <td style="width:5%">ID</td>
	    <td style="width:15%; text-align:left;text-indent:8px;">商品</td>
        <td style="width: 12%">编码</td>
        <td style="width: 8%">市场价</td>
        <td style="width: 8%">本站价</td>
	    <td style="width:5%">重量</td>	   	    
	    <td style="width:8%;display:none;">赠送积分</td>	    
	    <td style="width:8%<%if (ShopConfig.ReadConfigInfo().ProductStorageType == (int)ProductStorageType.SelfStorageSystem){ %>;display:none<%} %>"> 总库存数量</td>
	    <td style="width:8%">库存下限</td>
	    <%--<td style="width:8%">库存上限</td> 	--%>
	    <td style="width:5%">管理</td>           
    </tr>
    </thead>
    <tbody>
    <%foreach(ProductInfo product in productList){ %>		
    <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">
	    <td><%=product.Id%></td>
	    <td style="text-align:left;text-indent:8px;" title="<%=product.Name%>"><%=StringHelper.Substring(product.Name,28)%></td>
        <td>
            <input id="ProductNumber<%=product.Id%>" value='<%=product.ProductNumber%>' style="width: 70px" class="input" title="编码" /></td>
        <td>
            <input id="MarketPrice<%=product.Id%>" value='<%=product.MarketPrice%>' style="width: 40px" class="input" title="市场价" /></td>
        <td>
            <input id="SalePrice<%=product.Id %>" value='<%=product.SalePrice%>' style="width: 40px" class="input" title="本站价" /></td>
        <td>
            <input id="Weight<%=product.Id%>" value='<%=product.Weight%>' style="width: 40px" class="input" title="重量" /></td>

        <td style="display:none;"><input id="SendPoint<%=product.Id%>" value='<%=product.SendPoint%>' style="width:40px" class="input" title="赠送积分"/></td>
        <td <%if (ShopConfig.ReadConfigInfo().ProductStorageType == (int)ProductStorageType.SelfStorageSystem){ %>style="display:none"<%} %>><input id="TotalStorageCount<%=product.Id%>" value='<%=product.TotalStorageCount%>' style="width:40px" class="input" title="总库存数量"/></td>
        <td><input id="LowerCount<%=product.Id%>" value='<%=product.LowerCount%>' style="width:40px" class="input" title="库存下限"/></td>
        <td style="display:none"><input id="UpperCount<%=product.Id%>" value='<%=product.UpperCount%>' style="width:40px" class="input" title="库存上限"/></td>
        <td><%--<input type="button" value="保存" onclick="saveSingleEdit(<%=product.Id%>)" />--%><a href="javascript:void(0);"  class="ig-colink" onclick="saveSingleEdit(<%=product.Id%>)" >保存</a> </td>
    </tr>
    <%} %>
    </tbody>
    <tfoot>
                	<tr>
                    	<td colspan="8">                        	
                            <SkyCES:CommonPager ID="MyPager" runat="server" />                       
                            <div class="clear"></div>
                    	</td>
                    </tr>
                </tfoot>
</table>
</div>
    </div>
    <script language="javascript" type="text/javascript">
var userGradeIDList="<%=userGradeIDList %>";
var userGradeNameList="<%=userGradeNameList %>";
</script>
</asp:Content>
