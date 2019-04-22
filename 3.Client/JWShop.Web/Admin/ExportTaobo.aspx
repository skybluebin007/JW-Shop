<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ExportTaobo.aspx.cs" Inherits="JWShop.Web.Admin.ExportTaobo" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <link rel="stylesheet" href="/Admin/js/jqdate/base/jquery.ui.all.css">    
    <script src="/Admin/js/jqdate/js/jquery.ui.core.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.widget.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
        <script language="javascript" type="text/javascript" src="/Admin/js/UnlimitClass.js"></script>
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_StartAddDate").datepicker({ changeMonth: true, changeYear: true });
            $("#ctl00_ContentPlaceHolder_EndAddDate").datepicker({ changeMonth: true, changeYear: true });

            if ($("#UnlimitClass1").length > 0) $("#UnlimitClass1").hide();
            if ($("#UnlimitClass4").length > 0) $("#UnlimitClass4").hide();
            $("#UnlimitClass3").change(function () {
                if ($("#UnlimitClass4").length > 0) $("#UnlimitClass4").hide();
            })
            $("#UnlimitClass2").change(function () {
                $("#UnlimitClass3").change(function () {
                    if ($("#UnlimitClass4").length > 0) $("#UnlimitClass4").hide();
                })
            })
        });
    </script>
    <div class="container ease" id="container">
    	<div class="product-container">
        	<dl class="product-filter product-filter-pro clearfix">
                <dd>
                	<div class="head">商品名称：</div>
                    <SkyCES:TextBox CssClass="txt" MaxLength="20" ID="Key" runat="server" Width="100px" placeholder="商品名称"/>                   
                </dd>
            	<dd>
                	<div class="head">商品分类：</div>
                    <asp:DropDownList ID="ClassID" CssClass="select" runat="server" />                    
                </dd>
                <dd>
                	<div class="head">品牌：</div>
                    <asp:DropDownList ID="BrandID" CssClass="select" runat="server" />                     
                </dd>
               
                <dd>
                	<div class="head">添加时间：</div>
                    <SkyCES:TextBox  CssClass="txt" ID="StartAddDate" runat="server" MaxLength="10" RequiredFieldType="日期时间" placeholder="年-月-日"/>
                    <span class="tp">到</span>
                    <SkyCES:TextBox  CssClass="txt" ID="EndAddDate" runat="server" MaxLength="10" RequiredFieldType="日期时间" placeholder="年-月-日"/>
                </dd>
                <dd style="display:none;">
                	<div class="head">是否特价：</div>
                    <asp:DropDownList ID="IsSpecial" runat="server" CssClass="select">
                        <asp:ListItem Value="">全部</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                        <asp:ListItem Value="1">是</asp:ListItem>
                    </asp:DropDownList>
                </dd>
                <dd style="display:none;">
                	<div class="head">是否新品：</div>
                    <asp:DropDownList ID="IsNew" runat="server" CssClass="select">
                        <asp:ListItem Value="">全部</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                        <asp:ListItem Value="1">是</asp:ListItem>
                    </asp:DropDownList>
                </dd>
                <dd>
                	<div class="head">是否热销：</div>
                    <asp:DropDownList ID="IsHot" runat="server" CssClass="select">
                        <asp:ListItem Value="">全部</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                        <asp:ListItem Value="1">是</asp:ListItem>
                    </asp:DropDownList>
                </dd>
                <dd>
                	<div class="head">是否推荐：</div>
                    <asp:DropDownList ID="IsTop" runat="server" CssClass="select">
                        <asp:ListItem Value="">全部</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                        <asp:ListItem Value="1">是</asp:ListItem>
                    </asp:DropDownList>
                </dd>
                <dt>
                    <asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server" OnClick="SearchButton_Click" />
                </dt>
                </dl>
            <dl class="product-filter clearfix" style="float: left;">
                <dd>
                    <div class="form-row" style="padding: 0 0 0 125px;">
                        <div class="head" style="left: 27px;">淘宝店铺区域：</div>
                        <SkyCES:SingleUnlimitControl ID="RegionID" runat="server" />
                    </div>
                </dd>
                <br/>
                <dt>
                    <asp:Button CssClass="submit ease" ID="Button2" Text="导出淘宝格式" runat="server" OnClick="ExportButton_Click" style="width: 120px;" />*导出格式为淘宝助理5.0版本
                </dt>
            </dl>
        	<table cellpadding="0" cellspacing="0" border="0" class="product-list product-list-img" width="100%">
                <thead>
                    <tr>
                        <%--
                        <td width="30">ID</td>
                    --%>
                        <td width="35%" colspan="2">商品名称</td>                 
                         <td width="10%">价格</td>
                        <td width="10%">库存</td>
                        <td  width="10%" >销量</td>                     
                        <td width="7%">热销</td>
                        <td width="7%">推荐</td>
                        <td width="10%" onclick="OrderByAddDate();">发布时间</td>                      
                    </tr> 
                </thead>
                <tbody>
                	<tr class="firstH">
                    	<td colspan="8" style="padding: 0;">
                        	<div class="button">
	                             <%if(Count>0){ %>
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
    	                    <tr height="80">                         
		                        <td style="display:none;"><%# Eval("ID") %></td>
		                        <td ><div class="scan-img"><img src="<%#ShopCommon.ShowImage(Eval("Photo").ToString().Replace("Original","90-90")) %>" /></div></td>
		                        <td style="overflow: visible;">
		                        	<div class="link">
		                        		<a href="/ProductDetail.aspx?id=<%# Eval("ID") %>&fw=admin" target="_blank" title="<%#Eval("Name") %>"><%#StringHelper.Substring(Eval("Name").ToString(),30) %></a>
		                        		</div>
		                        	<p class="icolist"><s class="s1"></s><s class="s2"></s><img class="codeimg" src="<%#Eval("Qrcode") %>" alt="<%#Eval("Name") %>" title="<%#Eval("Name") %>" /></p>	                      
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
       
                                <td ><span id='IsHot<%#Eval("ID") %>' style="cursor:pointer" onclick="updateStatus(<%#Eval("ID") %>,'IsHot')"><%# ShopCommon.GetBoolText(Convert.ToInt32(Eval("IsHot")))%></span></td>
                                <td ><span id='IsTop<%#Eval("ID") %>' style="cursor:pointer" onclick="updateStatus(<%#Eval("ID") %>,'IsTop')"><%# ShopCommon.GetBoolText(Convert.ToInt32(Eval("IsTop")))%></span></td>
                                <td><span style="width: 90px; display: block; white-space: normal; line-height: 20px;"><%#Convert.ToDateTime(Eval("AddDate")).ToString("yyyy-MM-dd HH:mm") %></span></td>	                        
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>                    
                </tbody>
                <tfoot>
                	 <%if(Count>0){ %><tr>
                    	<td colspan="8">
                            <SkyCES:CommonPager ID="MyPager" runat="server" />                            
                            <div class="clear"></div>
                    	</td>
                    </tr><%} %>
                </tfoot>
            </table>
        </div>
    </div>
   <script language="javascript" type="text/javascript">var productID = ""; var _selectclassid = ""; var isUpdate = 0;</script>	 
    <script language="javascript" type="text/javascript" src="/Admin/js/ProductAdd.js"></script>
   <script type="text/javascript">
       $("table.product-list-img .icolist .s1").hover(function(){
           $(this).siblings("img").show();
       },function(){
           $(this).siblings("img").hide();
       });
  </script>
</asp:Content>
