<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductOffSale.aspx.cs" Inherits="JWShop.Web.Admin.ProductOffSale" %>
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
    	<div class="product-container" style="padding-top: 20px;">
        	<dl class="product-filter product-filter-pro clearfix">
            	<dd><label class="tp">分类：</label><asp:DropDownList ID="ClassID" runat="server" CssClass="select" Width="90" /> </dd>
                <dd><label class="tp">添加时间：</label><span class="tp">从</span><SkyCES:TextBox CssClass="txt" ID="StartAddDate" runat="server" RequiredFieldType="日期时间" width="70" /> <span class="tp">到</span> <SkyCES:TextBox CssClass="txt" ID="EndAddDate" runat="server" RequiredFieldType="日期时间" width="70" /></dd>
                <dd><label class="tp">品牌：</label><asp:DropDownList ID="BrandID" runat="server"  CssClass="select" width="90" /> </dd>
                <dd><label class="tp">商品名称：</label><SkyCES:TextBox CssClass="txt" ID="Key" runat="server" Width="100px" title="商品名称，名称拼音首字母或者商品编码"/></dd>
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <table cellpadding="0" cellspacing="0" border="0" class="product-list product-list-img" width="100%">
                <thead>
                    <tr>
                        <%--<td width="50" height="40">选择</td>
                        <td width="30">ID</td>
                        <td width="150">图片</td>--%>
                        <td width="35%" colspan="3">商品名称</td>
                       <%-- <td width="182">分类</td>--%>
                         <td width="10%">价格</td>
                        <td width="10%">库存</td>
                        <td width="10%">销量</td>
                        <%--<td width="50">特价</td>
                        <td width="50">新品</td>--%>
                        <td width="7%">推荐</td>
                        <td width="10%">发布时间</td>
                        <td width="11%">操作</td>
                    </tr>              
                   
                   
                </thead>
                <tbody>
                	<tr class="firstH">
                    	<td colspan="9" style="padding: 0;">
                        	<div class="button">
	                             <%if(Count>0){ %><label class="ig-checkbox" style="float: left;padding-right: 10px;"><input type="checkbox" name="All" onclick="selectAll(this)"  class="checkall" bind="list"/>全选</label>                  
                              <asp:Button CssClass="button-2 del" ID="Button1" Text=" 上 架 " OnClientClick="return checkSelect()" runat="server"  OnClick="OnSaleButton_Click"/>	         &nbsp;<asp:Button CssClass="button-2 del" ID="Button2" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>                             
                            <span style="float: left;">共找到<%=Count %>条记录<%if(Count>0){ %> ，<%=MyPager.PageCount %>页<%} %></span>
                            <asp:DropdownList ID="AdminPageSize" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="AdminPageSize_SelectedIndexChanged">
                                    <asp:ListItem Value="20">20条</asp:ListItem>
                                    <asp:ListItem Value="50">50条</asp:ListItem>
                                    <asp:ListItem Value="100">100条</asp:ListItem>
                                     </asp:DropdownList>
                                 <%} %>                               
                                
                            </div>                          
                    	</td>
                    </tr>
                    <asp:Repeater ID="RecordList" runat="server">
	                    <ItemTemplate>	     
    	                    <tr height="80">
                                <td  height="80"><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("ID") %>" ig-bind="list" /></label></td> 
		                        <td style="display:none;"><%# Eval("ID") %></td>
		                        <td ><div class="scan-img"><img src="<%#ShopCommon.ShowImage(Eval("Photo").ToString().Replace("Original","90-90")) %>" /></div></td>
		                        <td style="overflow: visible;">
		                        	<div class="link">
		                        		<%#StringHelper.Substring(Eval("Name").ToString(),30) %>		                        		
		                        	</div>
		                        
		                        </td>
		                        <td height="80" style="display:none;"> <%#ProductClassBLL.ProductClassNameListAdmin(Eval("ClassID").ToString()) %></td>
                                <td class="pPrice">
                                	<span><%#Eval("SalePrice") %></span>                                	
                                </td>
                                <td class="pPrice">
                                	<span><%#(Convert.ToInt32(Eval("totalStorageCount"))-Convert.ToInt32(Eval("OrderCount")) )%></span>
                                	
                                <td>
                                	<%#Eval("OrderCount") %>
                                </td>                             
                                <td ><span id='IsTop<%#Eval("ID") %>' style="cursor:pointer" onclick="updateStatus(<%#Eval("ID") %>,'IsTop')"><%# ShopCommon.GetBoolText(Convert.ToInt32(Eval("IsTop")))%></span></td>
                                <td><span style="width: 110px; display: block; white-space: normal; line-height: 20px;"><%#Convert.ToDateTime(Eval("AddDate")).ToString("yyyy-MM-dd HH:mm") %></span></td>
		                        <td class="imgCz">
		                        	<a href="ProductAdd.aspx?ID=<%# Eval("ID") %>"  class="ig-colink">编辑</a> |
                                    <span style="cursor:pointer; position:relative;" class="ig-colink pmore">更多		                        	<div class="list">
	                                   <a href="javascript:" onclick="if(confirm('确定要上架该商品？')){window.location.href='?ActionProduct=OnSaleProduct&ID=<%# Eval("ID") %>';}else {return false;}">上架</a>
                                        <a href="javascript:" onclick="if(confirm('确定要删除该商品？')){window.location.href='?ActionProduct=DeleteProduct&ID=<%# Eval("ID") %>';}else {return false;}">删除</a>	                               
                                    </div></span>

                                    
                                </td>		                        
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
    <script type="text/javascript">
        $("table.product-list-img .icolist .s1").hover(function () {
            $(this).siblings("img").show();
        }, function () {
            $(this).siblings("img").hide();
        });

        $("table.product-list-img .penIcon").click(function () {
            $(this).hide();
            $(this).siblings("a,span").hide();
            $(this).siblings(".tc").show();
        });

        $("table.product-list-img .pbtn").click(function () {
            $(this).parent(".tc").siblings(".penIcon").show();
            $(this).parent(".tc").siblings("a,span").show();
            $(this).parent(".tc").hide();
            var txt = $(this).siblings("input,textarea").val();
            $(this).parent(".tc").siblings("a,span").text(txt);

        });

        $(".pmore").mouseenter(function () {
            $(this).find(".list").show();
        });

        $(".imgCz .list,.imgCz").mouseleave(function () {
            $(".imgCz .list").hide();
        });
</script>
</asp:Content>