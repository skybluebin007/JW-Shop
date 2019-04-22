<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="JWShop.Web.Admin.Product" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Entity" %>
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
    	<div class="tab-title" style="margin: 30px 15px -1px 15px;">
    		<a href="ProductRecycle.aspx" class="recyBin fr"><s></s>商品回收站</a>
            <span  <%if (RequestHelper.GetQueryString<string>("Action") == "search" || (RequestHelper.GetQueryString<int>("IsHot") != 1 && RequestHelper.GetQueryString<int>("IsTop") != 1)){ %>class="cur"<%} %>><a href="Product.aspx">出售的商品</a></span>
            <span <%if (RequestHelper.GetQueryString<string>("Action") != "search" &&  RequestHelper.GetQueryString<int>("IsTop")==1){ %>class="cur"<%} %>><a href="Product.aspx?IsTop=1">推荐商品</a></span>
            <span style="display:none;" <%if (RequestHelper.GetQueryString<string>("Action") != "search" && RequestHelper.GetQueryString<int>("IsHot") == 1)
                    { %>class="cur"<%} %>><a href="Product.aspx?IsHot=1">热销商品</a></span>
        </div>
    	<div class="product-container" style="padding-top: 20px;">            
		<div class="product-filter product-filter-pro clearfix" style="overflow: hidden;" <%if (RequestHelper.GetQueryString<string>("Action") != "search" && (RequestHelper.GetQueryString<int>("IsHot") == 1 || RequestHelper.GetQueryString<int>("IsTop") == 1)){ %>style="display:none;"<%} %>>
        	<dl style="width: 120%;">
                 <dd style="margin-right: 8%;">
                	<div class="head">商品名称：</div>
                    <SkyCES:TextBox CssClass="txt" MaxLength="20" ID="Key" runat="server" Width="150px" placeholder="商品名称"/>                   
                </dd>
                <dd style="margin-right: 8%;display:none;">
                	<div class="head">商品编码：</div>
                    <SkyCES:TextBox CssClass="txt" MaxLength="20" ID="ProductNumber" runat="server" Width="150px" placeholder="商品编码"/>                   
                </dd>
            	<dd>
                	<div class="head">商品分类：</div>
                    <asp:DropDownList ID="ClassID" Width="162px" CssClass="select" runat="server" />                    
                </dd>
             
                <dd style="margin-right: 8%;">
                	<div class="head">价格：</div>
                    <SkyCES:TextBox  CssClass="txt" ID="LowerSalePrice" runat="server" MaxLength="9"  onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this)"/>
                    <span class="tp">到</span>
                    <SkyCES:TextBox  CssClass="txt" ID="UpperSalePrice" runat="server" MaxLength="9" onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this)"/>
                </dd>
                   <dd class="clear" style="margin: 0;"></dd>
                 <dd style="margin-right: 8%;">
                	<div class="head">总销量：</div>
                    <SkyCES:TextBox  CssClass="txt" ID="LowerOrderCount" runat="server" MaxLength="9" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"/>
                    <span class="tp">到</span>
                    <SkyCES:TextBox  CssClass="txt" ID="UpperOrderCount" runat="server" MaxLength="9" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"/>
                </dd>
                <dd>
                	<div class="head">品牌：</div>
                    <asp:DropDownList ID="BrandID" Width="162px" CssClass="select" runat="server" />                     
                </dd>
                
                <dd style="display:none;margin: 0;">
                	<div class="head">添加时间：</div>
                    <SkyCES:TextBox  CssClass="txt" ID="StartAddDate" runat="server" MaxLength="10" RequiredFieldType="日期时间" placeholder="年-月-日"/>
                    <span class="tp">到</span>
                    <SkyCES:TextBox  CssClass="txt" ID="EndAddDate" runat="server" MaxLength="10" RequiredFieldType="日期时间" placeholder="年-月-日"/>
                </dd>
                  <dd style="margin-right: 8%;">
                	<div class="head">是否推荐：</div>
                    <asp:DropDownList ID="IsTop" Width="162px" runat="server" CssClass="select">
                        <asp:ListItem Value="">全部</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                        <asp:ListItem Value="1">是</asp:ListItem>
                    </asp:DropDownList>
                </dd>
                <dd class="clear" style="margin: 0;"></dd>
               
                <dd style="display: none;">
                	<div class="head" style="display:none;">是否特价：</div>
                    <asp:DropDownList ID="IsSpecial" runat="server" CssClass="select" style="display:none;">
                        <asp:ListItem Value="">全部</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                        <asp:ListItem Value="1">是</asp:ListItem>
                    </asp:DropDownList>
                </dd>
                <dd style="display: none;">
                	<div class="head"  style="display:none;">是否新品：</div>
                    <asp:DropDownList ID="IsNew" runat="server" CssClass="select" style="display:none;">
                        <asp:ListItem Value="">全部</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                        <asp:ListItem Value="1">是</asp:ListItem>
                    </asp:DropDownList>
                </dd>
             
                <dd style="margin-right: 8%; display: none;">
                	<div class="head">是否热销：</div>
                    <asp:DropDownList ID="IsHot" Width="162px" runat="server" CssClass="select">
                        <asp:ListItem Value="">全部</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                        <asp:ListItem Value="1">是</asp:ListItem>
                    </asp:DropDownList>
                </dd>
                
                <dt style="margin-left: 120px;">
                    <span style="cursor:pointer;" id="resetSearch" onclick="ResetSearch();">清空条件</span>                 
                    <asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" />
                </dt>
            </dl>
           </div>
     <%	string _queryOrderType = RequestHelper.GetQueryString<string>("OrderType"); 
	   string  _productOrderType = RequestHelper.GetQueryString<string>("ProductOrderType"); %>    
        <input type="hidden" id="rawurl" value="<%=Request.RawUrl%>" />
        	<table cellpadding="0" cellspacing="0" border="0" class="product-list product-list-img" width="100%">
                <thead>
                    <tr>
                        <%--<td width="50" height="40">选择</td>
                        <td width="30">ID</td>
                        <td width="150">图片</td>--%>
                        <td width="35%" colspan="3">商品名称</td>
                       <%-- <td width="182">分类</td>--%>
                         <td width="12%">价格</td>
                        <td width="9%">库存</td>
                        <td width="9%" id="volume" <%if (_productOrderType=="OrderCount" && _queryOrderType == "Asc")
                                          {%>class="reup"<%}
                                          else if (_productOrderType == "OrderCount" && _queryOrderType == "Desc")
                                          { %>class="redown"<%} %> onclick="OrderByOrderCount();">销量<s></s></td>
                        <%--<td width="50">特价</td>
                        <td width="50">新品</td>
                        <td width="7%">热销</td>--%>
                        <td width="7%">推荐</td>
                        <td width="10%" onclick="OrderByAddDate();" <%if (_productOrderType == "AddDate" && _queryOrderType == "Asc")
                                          {%>class="reup"<%}
                                                                      else if (_productOrderType == "AddDate" && _queryOrderType == "Desc")
                                          { %>class="redown"<%} %> >发布时间<s></s></td>
                        <td width="11%">操作</td>
                    </tr> 
                </thead>
                <tbody>
                	<tr class="firstH">
                    	<td colspan="9" style="padding: 0;">
                        	<div class="button">
	                             <%if(Count>0){ %><label class="ig-checkbox" style="float: left;padding-right: 10px;"><input type="checkbox" name="All" onclick="selectAll(this)"  class="checkall" bind="list"/>全选</label>                  
                                <input type="button" class="button-2 del" ID="Button2" value=" 添 加 " onclick="location.href = 'ProductAdd.aspx'" />                            
                                <asp:Button CssClass="button-2 del" ID="Button3" Text=" 下 架 " OnClientClick="return checkSelect()" runat="server"  OnClick="OffSaleButton_Click"/> 
                                <asp:Button CssClass="button-2 del" ID="Button4" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>
                                <span style="float: left; display: block;">共找到<%=Count %>条记录<%if(Count>0){ %> ，<%=MyPager.PageCount %>页<%} %></span>
                                <span style="float: left; display: block;">&nbsp;&nbsp;每页显示：</span><asp:DropdownList ID="AdminPageSize" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="AdminPageSize_SelectedIndexChanged">
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
		                        		<a title="<%#Eval("Name") %>"><%#StringHelper.Substring(Eval("Name").ToString(),30) %></a>
		                        		<s class="penIcon"  _pid="<%#Eval("ID")%>" _modifyType="name"></s>
		                        		<div class="tc">
		                        			<p class="pnum">还能输<span id="ptitle<%#Eval("ID")%>"><%#(40-Eval("Name").ToString().Length>=0?40-Eval("Name").ToString().Length:0) %></span>字</p>
			                        		<textarea class="txt" name="" rows="" cols="" onkeydown="gbcount(this,40,$('#ptitle<%#Eval("ID")%>'));" onkeyup="gbcount(this,40,$('#ptitle<%#Eval("ID")%>'));" onblur="checkProductName();gbcount(this,40,$('#ptitle<%#Eval("ID")%>'));" id="pname<%# Eval("ID") %>"  ><%#Eval("Name") %></textarea>
			                        		<span class="pbtn" _pid="<%#Eval("ID")%>" onclick="UpdateProductName(this);">保存</span>
		                        		</div>
		                        	</div>
		                        	<p class="icolist" style="display:none;"><s class="s1"></s><s class="s2"></s><img class="codeimg" src="<%#Eval("Qrcode") %>" alt="<%#Eval("Name") %>" title="<%#Eval("Name") %>" /></p>
		                        </td>
		                        <td height="80" style="display:none;"> <%#ProductClassBLL.ProductClassNameListAdmin(Eval("ClassID").ToString()) %></td>
                                <td class="pPrice">
                                	<span class="changeval"><%#Eval("SalePrice") %></span>
                                	<s class="penIcon" _pid="<%#Eval("ID")%>" _modifyType="price" _standType="<%#Eval("StandardType")%>"></s>
	                        		<div class="tc">                                      
                                        <div class="tc_close">X</div>
                                        <ul id="updatePrice<%#Eval("ID")%>" _standType="<%#Eval("StandardType")%>" _salePrice="<%#Eval("SalePrice") %>" _pid="<%#Eval("ID")%>">                                            
                                            <%--<li class="first"><label>一口价：</label><input type="text" value="<%#Eval("SalePrice") %>" id='pprice<%#Eval("ID") %>' name="pprice<%#Eval("ID") %>"/></li>                                        
                                            <li><label>黄色：</label><input type="text" value="<%#Eval("SalePrice") %>" id='Text2'/></li>
                                            <li><label><a href="ProductAdd.aspx?ID=<%# Eval("ID") %>" >修改更多价格：</a></label><span class="pbtn">保存</span></li>--%>
                                         
                                        </ul>
	                        		</div>
                                </td>
                                <td class="pPrice">
                                	<span  class="changeval"><%#(Convert.ToInt32(Eval("totalStorageCount"))-Convert.ToInt32(Eval("OrderCount")) )%></span>
                                	<s class="penIcon" _pid="<%#Eval("ID")%>" _modifyType="storage" _standType="<%#Eval("StandardType")%>"></s>
	                        		<div class="tc">
		                        		 <div class="tc_close">X</div>
                                        <ul id="updateStorage<%#Eval("ID")%>" _standType="<%#Eval("StandardType")%>" _totalStorage="<%#Eval("totalStorageCount") %>" _pid="<%#Eval("ID")%>"> 
                                         </ul>
	                        		</div>
                                <td id="orderCount<%#Eval("ID") %>">
                                	<%#Eval("OrderCount") %>
                                </td>                              
       
                                <td style="display:none;"><span id='IsHot<%#Eval("ID") %>' style="cursor:pointer" onclick="updateStatus(<%#Eval("ID") %>,'IsHot')"><%# ShopCommon.GetBoolText(Convert.ToInt32(Eval("IsHot")))%></span></td>
                                <td ><span id='IsTop<%#Eval("ID") %>' style="cursor:pointer" onclick="updateStatus(<%#Eval("ID") %>,'IsTop')"><%# ShopCommon.GetBoolText(Convert.ToInt32(Eval("IsTop")))%></span></td>
                                <td><span style="width: 110px; display: block; white-space: normal; line-height: 20px;"><%#Convert.ToDateTime(Eval("AddDate")).ToString("yyyy-MM-dd HH:mm") %></span></td>
		                        <td class="imgCz">
		                        	<a href="ProductAdd.aspx?ID=<%# Eval("ID") %>"  class="ig-colink">编辑</a> | 
		                        	<span style="cursor:pointer; position:relative;" class="ig-colink pmore">更多
                                    		                        	<div class="list">
	                                    <a href="javascript:" onclick="if(confirm('确定要下架该商品？')){window.location.href='?ActionProduct=OffSaleProduct&ID=<%# Eval("ID") %>';}else {return false;}">下架</a>
                                        <a href="javascript:" onclick="if(confirm('确定要删除该商品？')){window.location.href='?ActionProduct=DeleteProduct&ID=<%# Eval("ID") %>';}else {return false;}">删除</a>
	                                    <a href="ProductClone.aspx?ID=<%# Eval("ID") %>">复制</a>
                                    </div>
                                    </span>

                                </td>		                        
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>                    
                </tbody>
                <tfoot>
                	 <%if(Count>0){ %><tr>
                    	<td colspan="9">
                        	<div class="button">
	                            <label class="ig-checkbox"><input type="checkbox" name="All" onclick="selectAll(this)"  class="checkall" bind="list"/>全选</label>                  
                                <input type="button" class="button-2 del" ID="Button1" value=" 添 加 " onclick="location.href = 'ProductAdd.aspx'" />
                                <%--<a href="ProductExport.aspx?<%=HttpContext.Current.Request.QueryString%>" class="button-2 del">商品导出</a>--%>
                                <asp:Button CssClass="button-2 del" ID="OffSaleButton" Text=" 下 架 " OnClientClick="return checkSelect()" runat="server"  OnClick="OffSaleButton_Click"/> 
                                &nbsp;<asp:Button CssClass="button-2 del" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>                                
                            </div>
                            <SkyCES:CommonPager ID="MyPager" runat="server" />                            
                            <div class="clear"></div>
                    	</td>
                    </tr><%} %>
                </tfoot>
            </table>
        </div>
    </div>
       <script language="javascript" type="text/javascript">var productID="";var _selectclassid="";var isUpdate=0;</script>	 
    <script language="javascript" type="text/javascript" src="/Admin/js/ProductAdd.js"></script>
    <script language="javascript" type="text/javascript" src="/Admin/js/Product.js"></script>
<script type="text/javascript">
	$("table.product-list-img .icolist .s1").hover(function(){
		$(this).siblings("img").show();
	},function(){
		$(this).siblings("img").hide();
	});
	
	//$("table.product-list-img .pbtn").click(function(){
	//	$(this).parent(".tc").siblings(".penIcon").show();
	//	$(this).parent(".tc").siblings("a,span").show();
	//	$(this).parent(".tc").hide();
	//	var txt=$(this).siblings("input,textarea").val();
	//	$(this).parent(".tc").siblings("a,span").text(txt);
		
	//});
	var $last=$("table.product-list-img tbody tr:last");
	if($last.offset().top+300  > $(window).height()){
		$last.find(".tc:not('.tc-d')").addClass("abc");
	}
	
	$(".pmore").mouseenter(function(){
		$(this).find(".list").show();
	});
	
	$(".imgCz .list,.imgCz").mouseleave(function(){
		$(".imgCz .list").hide();
	});
	//排序
	var _queryOrderType = "<%=RequestHelper.GetQueryString<string>("OrderType")%>";
   
	var _orderType = _queryOrderType;
	var _productOrderType = "<%=RequestHelper.GetQueryString<string>("ProductOrderType")%>";
    var _url = "<%=Request.RawUrl%>";
  
</script>
</asp:Content>
