<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="BargainAdd.aspx.cs" Inherits="JWShop.Web.Admin.BargainAdd" %>
<%@ Register Assembly="SkyCES.EntLib" Namespace="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <script type="text/javascript" src="/Admin/js/UnlimitClass.js"></script>
    <link rel="stylesheet" href="/Admin/Js/jqdate/base/jquery.ui.all.css">    
    <!--<script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.core.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>-->
    <link rel="stylesheet" href="/Admin/Js/jqdate/demos.css">
   <link rel="stylesheet" type="text/css" href="/Admin/Js/jqdate/css/jquery-ui.css" />
<link rel="stylesheet" type="text/css" href="/Admin/Js/jqdate/css/jquery-ui-timepicker-addon.min.css" />
<script type="text/javascript" src="/Admin/Js/jqdate/js/jquery-1.8.3.min.js"></script>
<script type="text/javascript" src="/Admin/Js/jqdate/js/jquery-ui.min.js"></script>
<script src="/Admin/layer/layer.js"></script>
<script type="text/javascript" src="/Admin/Js/jqdate/js/jquery-ui-sliderAccess.js"></script>
<script type="text/javascript" src="/Admin/Js/jqdate/js/jquery-ui-timepicker-addon.min.js"></script>
<script type="text/javascript" src="/Admin/Js/jqdate/js/jquery-ui-timepicker-zh-CN.js"></script>
<script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker-zh-CN.js.js"></script> 
    <style type="text/css">
    	.ui-corner-all{font-size:12px;}
        .form-txt .head, .form-txt2 .head { left: -40px; width: 120px; }
   
 .form-row table {
            box-shadow: none;
        }
 .form-row table td {
                border: none;
            }
    </style>
    <script>
        $(function () {
//          $("#ctl00_ContentPlaceHolder_StartDate").datepicker({ changeMonth: true, changeYear: true });
//          $("#ctl00_ContentPlaceHolder_EndDate").datepicker({ changeMonth: true, changeYear: true });
            $('#ctl00_ContentPlaceHolder_StartDate').datetimepicker({
				dateFormat:'yy-mm-dd',
				timeFormat: 'HH:mm:ss'
			});
			$('#ctl00_ContentPlaceHolder_EndDate').datetimepicker({
				dateFormat:'yy-mm-dd',
				timeFormat: 'HH:mm:ss'
			});
        });
    </script>

    <div class="container ease" id="container">
        <!--小程序砍价--!>
    	<!--<div class="path-title"></div>-->
        <div class="product-container product-container-border product-container-mt70">
            <div class="product-row">
                <!--<div class="product-tip">01</div>-->
                <div class="product-head">基础信息</div>
                <div class="product-main">
                	<div class="form-row">
                    	<div class="head">*活动名称：</div>
                        <SkyCES:TextBox ID="Name" CssClass="txt" width="400" runat="server" MaxLength="60" CanBeNull="必填"/>
                    </div>
                    <div class="clear"></div>
                </div>

                <div class="product-main">
                    <div class="clear"></div>
                    <div class="form-row">
                        <div class="head">*活动时间：</div>
                        <SkyCES:TextBox ID="StartDate" CssClass="txt" runat="server" Width="140px" RequiredFieldType="日期时间" CanBeNull="必填" /> <span>到</span> <SkyCES:TextBox ID="EndDate" CssClass="txt" runat="server" Width="140px" RequiredFieldType="日期时间"  CanBeNull="必填" />
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="product-main">
                    <div class="form-row">
                        <div class="head">
                            活动状态：
                        </div>
                        <div class="og-radio">
                            <label class="item  <%if (Status == 1)
                                { %>checked<%}%>">
                                开启<input type="radio" name="ctl00$ContentPlaceHolder$Status" value="1" <%if (Status == 1)
                                      { %>checked<%}%> /></label>
                            <label class="item <%if (Status == 0)
                                { %>checked<%}%>">
                                关闭<input type="radio" name="ctl00$ContentPlaceHolder$Status" value="0" <%if (Status == 0)
                                       { %>checked<%}%> /></label>

                        </div>
                </div>
                </div>
                <div class="product-main">
                    <div class="clear"></div>
                    <div class="form-row">
                        <div class="head">助力限制：</div>
                       <SkyCES:TextBox runat="server" ID="LimitCount" Width="50px" RequiredFieldType="正整数" Text="10" class="txt" CanBeNull="必填"/>次 <%--<asp:CheckBox runat="server" ID="Unlimited"/> 不限制--%>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="product-main">
                    <div class="clear"></div>
                    <div class="form-row">
                        <div class="head">成功人数：</div>
                       (人)虚拟数量，在实际成功人数基础上增加 <SkyCES:TextBox runat="server" ID="NumberPeople" Width="50" Text="0" RequiredFieldType="整数" class="txt"/> 
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="product-main" style="display:none;">
                    <div class="clear"></div>
                    <div class="form-row">
                        <div class="head">*砍价成功率：</div>
                    <SkyCES:TextBox runat="server" ID="SuccessRate" Text="100" Width="50" RequiredFieldType="百分比" class="txt"/> % 概率范围(0~100之间的整数)，概率越低，越难砍到底价
                    </div>
                    <div class="clear"></div>
                </div>
           
            
       
            </div>
            <div class="product-row product_shezhi">
                <!--<div class="product-tip">01</div>-->
                <div class="product-head">商品设置</div>
               <%-- <input type="button" value="选择商品"  class="form-submit"/>--%>
                <div id="products">
                	<div class="product_top">
                		<% for (int i=0;i<DetailList.Count;i++) {%>
                            <h3 class="<%=(i==0?"cur":"") %>">商品<%=i+1 %></h3>
                        <%} %>
                	</div>
                    <%if (enableUpdate)
                        { %>
                	<div class="btnico">
                		 <span class="add_ico" onclick="SelectProduct();"></span><span class="dele_ico" onclick="DelProduct();"></span>
                	</div>
                    <%} %>
                	<div class="contbox" id="product">
                		<%foreach (var item in DetailList){%>
                            <div class="cont <%=(DetailList.First()==item?"":"hide") %>" data-id="<%=item.Id %>"">
                                <input type="hidden" name="Id" value="<%=item.Id %>""/>
                                <input type="hidden" name="Product_Id" value="<%=item.ProductID %>"/>
                                <input type="hidden" name="BargainId" value="<%=item.BargainId %>"/>
                                <%var product = ProductBLL.Read(item.ProductID); %>
                                <input type="hidden" name="Product_IsSale" value="<%=product.IsSale %>"/>
                                <input type="hidden" name="Product_IsDelete" value="<%=product.IsDelete %>"/>
                                <input type="hidden" name="Product_Real_Id" value="<%=product.Id %>"/>
                                <div class="form-row">
		                            <div class="head"><s>*</s>商品名称：</div>
		                            <input type="text"  class="txt" name="Product_Name" value="<%=item.ProductName %>"/>
                                    <%if (product.IsDelete == 1 || product.IsSale == 0)
                                        { %><span class="red">此商品已下架或已删除，不会显示在前端砍价页面</span><%} %>
                                     <%if (product.Id<= 0)
                                        { %><span class="red">此商品不存在</span><%} %>
	                            </div>
	                            <div class="form-row">
		                            <div class="head"><s>*</s>商品原价：</div>
		                            <input type="text"  class="txt txt2" name="Product_OriginalPrice" readonly="readonly" value="<%=product.MarketPrice %>"/><i class="tps">支持两位小数</i>
	                            </div>
	                            <div class="form-row">
		                            <div class="head"><s>*</s>商品底价：</div>
		                            <input type="text"  class="txt txt2" onkeyup="value=value.replace(/[^(\-)?\d+(\.\d{1,2})?$]/g,'')" name="Product_ReservePrice" value="<%=item.ReservePrice %>"/><i class="tps">不高于原价，如果设置为0元，用户将不进行支付直接领取</i>
	                            </div>
	                            <div class="form-row">
		                            <div class="head"><s>*</s>商品库存：</div>
		                            <input type="text"  class="txt txt2" onkeyup="value=value.replace(/[^\d+$ ]/g,'')" name="Stock" value="<%=item.Stock %>"/>
	                            </div>
	                            <div class="form-row">
		                            <div class="head">排序号：</div>
		                            <input type="text"  class="txt txt2" onkeyup="value=value.replace(/[^\d+$]/g,'')" name="Sort" value="<%=item.Sort %>"/><i class="tps">用于控制商品排序，数字越大排序越靠前</i>
	                            </div>
                            </div>

                          <%} %>
                	</div>

                </div>
            </div>
        </div>
        <div class="form-foot form_fot2">
            <asp:Button CssClass="form-submit ease" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" OnClientClick="return Page_ClientValidate()"/>
        </div>
    </div>

    <script type="text/javascript" src="/Admin/Js/FavorableActivityAdd.js"></script>
    <script type="text/javascript">
   

        //Unlimited次数限制
<%--        $("#<%=Unlimited.ClientID%>").change(function () {
            if ($(this).is(":checked")) {
                $("#<%=LimitCount.ClientID%>").attr("disabled", "disabled");
            } else {
                $("#<%=LimitCount.ClientID%>").removeAttr("disabled");
            }
        });
        $(function () {
            if ($("#<%=Unlimited.ClientID%>").is(":checked")) {
                $("#<%=LimitCount.ClientID%>").attr("disabled", "disabled");
            }
        });--%>
        function SelectProduct() {
            layer.open({
                type: 2,
                //skin: 'layui-layer-lan',
                title: '选择产品',
                fix: false,
                shadeClose: true,
                maxmin: true,
                area: ['1200px', '600px'],
                content: 'BargainSelectProduct.aspx'
            });
        }
        var productIndex = <%=DetailList.Count%>;

        function loadProduct(id) {
            $.get("BargainProduct.aspx", { Id: id }, function (res) {
                $("#product").children("div").attr("class", "cont hide");
                $("#product").append(res);
                $(".product_top h3").removeClass("cur");
                $(".product_top").append("<h3 class=\"cur\">商品" + (++productIndex) + "</h3>");
            });
        }
         $(function () {
             $("#products .product_top").on('click', "h3", function () {
	            $(this).addClass("cur").siblings().removeClass("cur");
	            $("#products > .contbox").find(".cont").eq($(this).index()).removeClass("hide").siblings().addClass("hide");
	        });
        });
         
         $("#<%=SubmitButton.ClientID%>").click(function () {
             var flag = true;
             if (flag) {
                 $('input[name="Product_ReservePrice"]').each(function () {
                     if ($(this).val() == "") {
                         alertMessage("请填写商品底价");
                         flag = false;
                     }
                 });
                 $('input[name="Stock"]').each(function () {
                     if ($(this).val() == "") {
                         alertMessage("请填写商品库存");
                         flag = false;
                     }
                 });
                 $('input[name="Sort"]').each(function () {
                     if ($(this).val() == "") {
                         alertMessage("请填写排序号");
                         flag = false;
                     }
                 });
             } else if (flag) {

             }

        
             return flag;
         });

        function DelProduct(){
            $(".product_top .cur").remove();
            $("#product").children("div").each(function (){
                if (!$(this).hasClass("hide")) {
                    var detailId=$(this).data("id");
                    if (detailId>0) {
                        $.get("/Admin/BargainAdd.aspx",{Action:"Delete",ID:detailId},function(){});
                    }
                    $(this).remove();
                }
            });
        }
    </script>
</asp:Content>
